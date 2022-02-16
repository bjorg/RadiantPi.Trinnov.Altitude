/*
 * RadiantPi.Trinnov.Altitude - Communication client for Trinnov Altitude
 * Copyright (C) 2020-2022 - Steve G. Bjorg
 *
 * This program is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Affero General Public License along
 * with this program. If not, see <https://www.gnu.org/licenses/>.
 */

namespace RadiantPi.Trinnov.Altitude;

using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using RadiantPi.Telnet;

public class TrinnovAltitudeClientConfig {

    //--- Properties ---
    public string? Host { get; set; }
    public ushort? Port { get; set; }
}

public sealed class TrinnovAltitudeClient : ITrinnovAltitude {

    //--- Class Fields ---
    private static Regex _decoderRegex = new Regex(@"^DECODER NONAUDIO [01] PLAYABLE (?<playable>[01]) DECODER (?<decoder>.+) UPMIXER (?<upmixer>.+)", RegexOptions.Compiled);
    private static Regex _volumeRegex = new Regex(@"^VOLUME (?<volume>-?\d+(\.\d*)?)", RegexOptions.Compiled);
    private static Regex _muteRegex = new Regex(@"^MUTE (?<mute>[01])", RegexOptions.Compiled);

    //--- Fields ---
    private readonly ITelnet _telnet;
    private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);

    //--- Constructors ---
    public TrinnovAltitudeClient(TrinnovAltitudeClientConfig config, ILoggerFactory? loggerFactory = null)
        : this(
            new TelnetClient(
                config.Host ?? throw new ArgumentNullException("config.Host"),
                config.Port ?? 44100,
                loggerFactory?.CreateLogger<TelnetClient>()
            ),
            loggerFactory?.CreateLogger<TrinnovAltitudeClient>()
        ) { }

    public TrinnovAltitudeClient(ITelnet telnet, ILogger<TrinnovAltitudeClient>? logger) {
        Logger = logger;
        _telnet = telnet ?? throw new ArgumentNullException(nameof(telnet));
        _telnet.ValidateConnectionAsync = ValidateConnectionAsync;
        _telnet.MessageReceived += MessageReceived;
    }

    //--- Events ---
    public event EventHandler<AudioDecoderChangedEventArgs>? AudioDecoderChanged;
    public event EventHandler<VolumeChangedEventArgs>? VolumeChanged;
    public event EventHandler<MuteChangedEventArgs>? MuteChanged;

    //---  Properties ---
    private ILogger? Logger { get; }

    //--- Methods ---
    public Task ConnectAsync() => _telnet.ConnectAsync();
    public Task SetVolumeAsync(float volume) => _telnet.SendAsync($"volume {volume}");
    public Task AdjustVolumeAsync(float delta) => _telnet.SendAsync($"dvolume {delta}");
    public Task SelectPresetAsync(int preset) => _telnet.SendAsync($"loadp {preset}");
    public Task SelectProfileAsync(int source) => _telnet.SendAsync($"profile {source}");

    public void Dispose() {
        _mutex.Dispose();
        _telnet.Dispose();
    }

    private async Task ValidateConnectionAsync(ITelnet client, TextReader reader, TextWriter writer) {
        var handshake = await reader.ReadLineAsync() ?? "";

        // the Trinnov Altitude sends a welcome text to identify itself
        if(!handshake.StartsWith("Welcome on Trinnov Optimizer (", StringComparison.Ordinal)) {
            throw new NotSupportedException("Unrecognized device");
        }
        Logger?.LogDebug("Trinnov Altitude connection established");

        // announce client
        await writer.WriteLineAsync($"id radiant_pi_trinnov_{DateTimeOffset.UtcNow.Ticks}").ConfigureAwait(false);
    }

    private void MessageReceived(object? sender, TelnetMessageReceivedEventArgs args) {
        Logger?.LogDebug($"Received: {args.Message}");

        // check for decoder event
        var decoderMatch = _decoderRegex.Match(args.Message);
        if(decoderMatch.Success) {
            var playable = decoderMatch.Groups["playable"].Value;
            if(playable == "0") {

                // we can't use this signal, ignore it
                return;
            }

            // emit event
            var decoder = decoderMatch.Groups["decoder"].Value;
            var upmixer = decoderMatch.Groups["upmixer"].Value;
            AudioDecoderChanged?.Invoke(this, new(decoder, upmixer));
            return;
        }

        // check for volume event
        var volumeMatch = _volumeRegex.Match(args.Message);
        if(volumeMatch.Success) {
            var volumeText = volumeMatch.Groups["volume"].Value;
            if(float.TryParse(volumeText, out var volume)) {
                VolumeChanged?.Invoke(this, new(volume));
            } else {
                Logger?.LogWarning($"Invalid VOLUME value: {volumeText}");
            }
            return;
        }

        // check for mute event
        var muteMatch = _muteRegex.Match(args.Message);
        if(muteMatch.Success) {
            var muteText = muteMatch.Groups["mute"].Value;
            switch(muteText) {
            case "0":
                MuteChanged?.Invoke(this, new(false));
                break;
            case "1":
                MuteChanged?.Invoke(this, new(true));
                break;
            default:
                Logger?.LogWarning($"Unrecognized MUTE value: {muteText}");
                break;
            }
            return;
        }
    }
}

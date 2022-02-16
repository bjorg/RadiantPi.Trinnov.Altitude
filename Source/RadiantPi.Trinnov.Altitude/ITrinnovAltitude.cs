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

public interface ITrinnovAltitude : IDisposable {

    //--- Events ---

    /// <summary>
    /// Event triggered when the audio code changes.
    /// </summary>
    event EventHandler<AudioDecoderChangedEventArgs> AudioDecoderChanged;

    /// <summary>
    /// Event triggered when the volume changes.
    /// </summary>
    event EventHandler<VolumeChangedEventArgs>? VolumeChanged;

    /// <summary>
    /// Triggered when the mute status changes.
    /// </summary>
    event EventHandler<MuteChangedEventArgs>? MuteChanged;

    //--- Methods ---

    /// <summary>
    /// Connect to Trinnov Altitude.
    /// </summary>
    Task ConnectAsync();

    /// <summary>
    /// Set volume.
    /// </summary>
    /// <param name="volume">Volume attenuation in dB. Range -100 to 20.</param>
    Task SetVolumeAsync(float volume);

    /// <summary>
    /// Adjust volume.
    /// </summary>
    /// <param name="delta">Delta volume value.</param>
    Task AdjustVolumeAsync(float delta);

    /// <summary>
    /// Select preset.
    /// </summary>
    /// <param name="preset">Preset number: 0 = built-in preset, ...</param>
    Task SelectPresetAsync(int preset);

    /// <summary>
    /// Select profile (a.k.a. source).
    /// </summary>
    /// <param name="source">Source number: 0 = HDMI1, 1 = HDMI2, ...</param>
    Task SelectProfileAsync(int source);
}

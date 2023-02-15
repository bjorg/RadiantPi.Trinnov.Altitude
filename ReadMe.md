# RadiantPi.Trinnov.Altitude

`TrinnovAltitudeClient` enables control of a Trinnov Altitude over Telnet. The library is platform agnostic and works on Windows or Linux, including on a Raspberry Pi.

Run the `dotnet` command from your project folder to add the `RadiantPi.Trinnov.Altitude` assembly:
```
dotnet add package RadiantPi.Trinnov.Altitude
```

Find a description of the latest changes in the [release notes](ReleaseNotes.md).

## Sample: Show Audio Codec

Use `TrinnovAltitudeClient` to connect to an Trinnov Altitude processor and show audio codec changes.

```csharp
using System;
using RadiantPi.Trinnov.Altitude;

// initialize client
using var client = new TrinnovAltitudeClient(new() {
    Host = "192.168.1.180",
    Port = 44100
});

// hook-up event handlers
client.AudioDecoderChanged += delegate (object? sender, AudioDecoderChangedEventArgs args) {
    Console.WriteLine($"Audio Codec: Decoder='{args.Decoder}' Upmixer='{args.Upmixer}'");
};

// connect to device
await client.ConnectAsync();

// wait until the enter key is pressed
Console.WriteLine("Press ENTER to exit.");
Console.ReadLine();
```

## Output

```
ress ENTER to exit.
Audio Codec: Decoder='DD' Upmixer='Dolby Surround'
Audio Codec: Decoder='DTS:X MA' Upmixer='Neural:X'
```

# License

This application is distributed under the GNU Affero General Public License v3.0 or later.

Copyright (C) 2020-2023 - Steve G. Bjorg
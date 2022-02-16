# RadiantPi.Trinnov.Altitude - ShowAudioCodec

Show audio codec changes in the Trinnov Altitude.

## Code

```csharp
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
Press ENTER to exit.
Audio Codec: Decoder='DD' Upmixer='Dolby Surround'
Audio Codec: Decoder='DTS:X MA' Upmixer='Neural:X'
```
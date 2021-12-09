# RadiantPi.Trinnov.Altitude - ShowVolume

Show volume changes in the Trinnov Altitude.

## Code

```csharp
using System;
using RadiantPi.Trinnov.Altitude;

// initialize client
using var client = new TrinnovAltitudeClient(new() {
    Host = "192.168.1.180",
    Port = 44100
});

// hook-up event handlers
client.VolumeChanged += delegate (object? sender, VolumeChangedEventArgs args) {
    Console.WriteLine($"Volume: {args.Volume}");
};
client.MuteChanged += delegate (object? sender, MuteChangedEventArgs args) {
    Console.WriteLine($"Mute: {args.Muted}");
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
Volume: -31.6
Volume: -30.4
Mute: True
Mute: False
Volume: -31.6
```
using System;
using RadiantPi.Trinnov.Altitude;
using RadiantPi.Lumagen;

// initialize clients
using var client = new TrinnovAltitudeClient(new() {
    Host = "192.168.1.180",
    Port = 44100
});
using var lumagenClient = new RadianceProClient(new RadianceProClientConfig {
    PortName = "/dev/ttyUSB0",
    BaudRate = 9600
});

// hook-up event handlers
client.AudioDecoderChanged += async delegate (object? sender, AudioDecoderChangedEventArgs args) {

    // show current decoder and upmixer on Lumagen RadiancePro
    await lumagenClient.ShowMessageAsync($"{args.Decoder} ({args.Upmixer})", 3);
};

// connect to device
await client.ConnectAsync();

// wait until the enter key is pressed
Console.WriteLine("Press ENTER to exit.");
Console.ReadLine();

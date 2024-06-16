using System;
using FCUUsbService.Models;

namespace FCUUsbService.Services;

public class UsbConfigurationProvider : ModelProvider
{
    private const string DevPath = Constants.Usb.DevPath;
    private const string DevName = Constants.Usb.DevName;
    private Udevadm udev = new();
    public UsbConfiguration GetUsbConfiguration()
    {
        var ports = udev.GetDeviceATPorts();

        if (ports is { Count: <= 0 }) return new();

        var conf = ports.Select(x => KeyValuePair.Create(Capture(x[DevPath], Shared.RegExpr.UsbPath), x[DevName]));

        return new()
        {
            UsbDevices = new(conf),
        };
    }
}

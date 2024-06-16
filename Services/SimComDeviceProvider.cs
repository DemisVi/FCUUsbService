using System;
using FCUUsbService.Models;

namespace FCUUsbService;

public class SimComDeviceProvider : ModelProvider
{
    protected Udevadm udev = new();
    public SimComDevice[] GetSimComDevices()
    {
        var udevOutput = udev.GetDeviceModemPorts();
        return udevOutput.Select(x => new SimComDevice()
        {
            BusPath = Capture(x[Constants.Usb.DevPath], Shared.RegExpr.UsbPath),
            UsbName = x[Constants.Usb.SysName],
            SystemPath = x[Constants.Usb.DevName],
            UsbSN = x[Constants.Usb.UsbSerialShort],
            UsbModel = x[Constants.Usb.UsbModel],
        }).ToArray();
    }
}

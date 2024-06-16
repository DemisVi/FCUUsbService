using System;

namespace FCUUsbService.Models;

public struct SimComDevice
{
    public string BusPath { get; init; }
    public string SystemPath { get; init; }
    public string UsbName { get; init; }
    public string UsbModel { get; init; }
    public string UsbSN { get; init; }
}

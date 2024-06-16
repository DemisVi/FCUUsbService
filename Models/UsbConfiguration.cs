using System;
using System.Collections;

namespace FCUUsbService.Models;

public class UsbConfiguration
{
    public Dictionary<string, string>? Devices { get; init; }
}

using System;
using FCUUsbService.Models;

internal class TcpServiceEventArgs : EventArgs
{
    public TcpServiceEventType EventType { get; set; }
    public UsbConfiguration? UsbConfiguration { get; set; }
    public IEnumerable<SimComDevice>? SimComDevices { get; set; }
}

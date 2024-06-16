using System;

namespace FCUUsbService.Models;

internal class Constants
{
    public struct RegexConstants
    {
        public const string SysUsbPath = @"((?<=/)(\d+-\d+){1}([\.\d]*):([\.\d]+)(?=/))";
        public const string UsbPath = @"(\d+-\d+){1}([\.\d]*):([\.\d]+)";
    }
    public struct ToolConstants
    {
        public const string Udevadm = "udevadm";
        public const string SimComAtPortQuery = "info --export-db --property-match=ID_MODEL=\"SimTech*\" --property-match=ID_USB_INTERFACE_NUM=02 --json=short";
        public const string SimComModemPortQuery = "info --export-db --property-match=ID_MODEL=\"SimTech*\" --property-match=ID_USB_INTERFACE_NUM=03 --json=short";
        //  --property-match=ID_USB_INTERFACE_NUM=02
    }
    public struct Usb
    {
        public const string DevPath = "DEVPATH";
        public const string SysName = "SYSNAME";
        public const string DevName = "DEVNAME";
        public const string UsbModel = "ID_USB_MODEL";
        public const string UsbModelEnc = "ID_USB_MODEL_ENC";
        public const string UsbModelId = "ID_USB_MODEL_ID";
        public const string UsbSerial = "ID_USB_SERIAL";
        public const string UsbSerialShort = "ID_USB_SERIAL_SHORT";
        public const string UsbVendor = "ID_USB_VENDOR";
        public const string UsbVendorEnc = "ID_USB_VENDOR_ENC";
        public const string UsbVendorId = "ID_USB_VENDOR_ID";
        public const string UsbRevision = "ID_USB_REVISION";
        public const string UsbType = "ID_USB_TYPE";
        public const string UsbInterfaces = "ID_USB_INTERFACES";
        public const string UsbInterfaceNum = "ID_USB_INTERFACE_NUM";
    }
}

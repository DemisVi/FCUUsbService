using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FCUUsbService.Models;

internal partial class Shared
{
    public static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        AllowTrailingCommas = true,
        WriteIndented = true,
    };
    public partial struct RegExpr
    {
        public static Regex UsbPath { get; } = UsbPathRegex();

        [GeneratedRegex(Constants.RegexConstants.UsbPath)]
        private static partial Regex UsbPathRegex();
    }
}

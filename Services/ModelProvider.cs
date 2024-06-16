using System;
using System.Text.RegularExpressions;

namespace FCUUsbService.Services;

public abstract class ModelProvider
{
    /// <summary>
    /// Distinct specific substring within Udevadm output property
    /// </summary>
    /// <param name="input">Udevadm property</param>
    /// <param name="expr">Stored regex from Constants.RegExpr</param>
    /// <returns></returns>
    protected virtual string Capture(string input, Regex expr) => expr.Match(input).Value;
}

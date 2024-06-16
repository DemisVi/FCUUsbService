using System.Text.Json;
using FCUUsbService.Models;

namespace FCUUsbService;

public class Udevadm : Tool
{
    private const string Format = "[{0}]";
    private const int ZeroCapacity = 0;
    private const string Comma = ",";

    public override string FileName { get; } = Constants.ToolConstants.Udevadm;

    protected List<Dictionary<string, string>> Query(string arguments)
    {
        base.Execute(arguments);

        if (StdOutput is { Length: <= 0 }) return new(ZeroCapacity);

        var output = StdOutput.Replace(Environment.NewLine, Comma, StringComparison.OrdinalIgnoreCase);
        var formatted = string.Format(Format, output);

        return JsonSerializer.Deserialize<List<Dictionary<string, string>>>(formatted, Shared.SerializerOptions)!;
    }
    public List<Dictionary<string, string>> GetDeviceATPorts() => Query(Constants.ToolConstants.SimComAtPortQuery);
    public List<Dictionary<string, string>> GetDeviceModemPorts() => Query(Constants.ToolConstants.SimComModemPortQuery);
}

using System;
using System.Diagnostics;

namespace FCUUsbService;

public abstract class Tool : IDisposable
{
    private bool disposedValue;
    private readonly Process toolProcess;

    public abstract string FileName { get; }
    public virtual string? Arguments { get; }
    public virtual int TimeoutSeconds { get; set; } = 1;
    public virtual string StdOutput { get; set; } = string.Empty;
    public virtual string StdError { get; set; } = string.Empty;

    public Tool()
    {
        toolProcess = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
    }

    public virtual void Execute(string? arguments = null)
    {
        if (string.IsNullOrEmpty(FileName) || (string.IsNullOrEmpty(Arguments) && string.IsNullOrEmpty(arguments)))
            throw new InvalidOperationException($"Props '{nameof(FileName)}' and '{nameof(Arguments)}' must be set before execution. Or specify {nameof(arguments)} parameter.");

        toolProcess.StartInfo.FileName = FileName;
        toolProcess.StartInfo.Arguments = arguments ?? Arguments;

        toolProcess.Start();
        toolProcess.WaitForExit(TimeSpan.FromSeconds(TimeoutSeconds));

        if (toolProcess.HasExited is false) toolProcess.Kill();

        StdError = toolProcess.StandardError.ReadToEnd();
        StdOutput = toolProcess.StandardOutput.ReadToEnd();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                toolProcess.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~Tool()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

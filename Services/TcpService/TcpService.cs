using System;
using System.Net;
using FCUUsbService.Services;
using FCUUsbService.Models;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Text.Json;

internal class TcpService : IDisposable
{
    private const int Port = 13370;
    private readonly TcpListener listener = new(IPAddress.Any, Port);
    private bool disposedValue;
    private readonly TimeSpan timeout = TimeSpan.FromMilliseconds(100);
    private TcpClient? client;

    private TcpServiceEventHandler? tcpServiceEventHandler;
    private CancellationTokenSource? cts;

    public bool Connected => client?.Connected ?? false;

    public event TcpServiceEventHandler? CommandReceived { add => tcpServiceEventHandler += value; remove => tcpServiceEventHandler -= value; }

    // public void Start() => listener.Start();
    public void Send(TcpServiceEventArgs args)
    {
        if (Connected is not true) throw new SocketException();

        var buffer = JsonSerializer.Serialize(args, Shared.SerializerOptions);
        using var stream = client?.GetStream();
        stream?.Write(Encoding.ASCII.GetBytes(buffer));
    }
    public async void StopAsync()
    {
        if (cts is null or { IsCancellationRequested: true }) return;

        await cts.CancelAsync();
        listener.Stop();
    }

    public async void StartAsync()
    {
        cts = new CancellationTokenSource();

        await Task.Factory.StartNew(async () =>
        {
            try
            {
                listener.Start();

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        if (!listener.Pending())
                        {
                            await Task.Delay(timeout);
                            continue;
                        }
                        client = await listener.AcceptTcpClientAsync();
                        await using var stream = client.GetStream();
                        while (client is { Available: <= 0 }) await Task.Delay(timeout);

                        var buffer = new byte[1_024];
                        var sb = new StringBuilder();

                        while (stream is { DataAvailable: true })
                        {
                            var read = stream.Read(buffer);
                            sb.Append(Encoding.ASCII.GetString(buffer, 0, read));
                        }

                        var dateReceived = sb.ToString();
                        var argsReceived = JsonSerializer.Deserialize<TcpServiceEventArgs>(dateReceived);
                        tcpServiceEventHandler?.Invoke(this, argsReceived);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(JsonSerializer.Serialize(new TcpServiceEventArgs() { EventType = TcpServiceEventType.None, }));
                    }
                    finally
                    {
                        client?.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client?.Dispose();
                listener.Stop();
            }
        }
        ,
        cts.Token);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                // listener.Stop();
                client?.Dispose();
                listener.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TcpService()
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

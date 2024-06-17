using System;
using System.Net.Sockets;
using FCUUsbService.Services;

public static class ServiceCollectionExtension
{
    public static void AddTcpServices(this IServiceCollection collection)
    {
        var tcpService = new TcpService();
        tcpService.CommandReceived += (s, e) => 
        {
            var configProvider = new UsbConfigurationProvider();
            var deviceProvider = new SimComDeviceProvider();
            var service = s as TcpService;
            if (service is null or { Connected: false }) throw new SocketException(11, $"{nameof(TcpService)} connection state is {service?.Connected}");

            switch (e?.EventType)
            {
                case TcpServiceEventType.UsbConfig:
                    service.Send(new() { EventType = TcpServiceEventType.UsbConfig, UsbConfiguration = configProvider.GetUsbConfiguration() });
                    break;
                case TcpServiceEventType.SimComDevices:
                    service.Send(new() { EventType = TcpServiceEventType.SimComDevices, SimComDevices = deviceProvider.GetSimComDevices() });
                    break;
                default:
                    break;
            };
        };
        tcpService.StartAsync();
        collection.AddSingleton(tcpService);
    }
}

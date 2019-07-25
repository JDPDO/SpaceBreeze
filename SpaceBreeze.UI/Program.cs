using ElectronNET.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace JDPDO.SpaceBreeze.UI
{
    public class Program
    {
        private static IWebHost host;
        private InstanceRegister register;

        public static void Main(string[] args)
        {
            host = BuildWebHost(args);
            host.Run();
            
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseElectron(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}

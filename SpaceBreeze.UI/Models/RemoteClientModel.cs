using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;
using ElectronNET.API;
using System.Linq;

namespace JDPDO.SpaceBreeze.UI.Models
{
    public class RemoteClientModel
    {
        /// <summary>
        /// Contains the app's instance register.
        /// </summary>
        private InstanceRegister register;

        /// <summary>
        /// Storing current avaiable client managers.
        /// </summary>
        public InstanceType ClientTypes { get; set; }

        /// <summary>
        /// Creates a new 'RemoteClientModel' instance.
        /// </summary>
        public RemoteClientModel()
        {
            register = InstanceRegister.GetFirstInstanceRegister();
            ClientTypes = InstanceType.FtpsClient;
        }

        /// <summary>
        /// Creates a new client and saves it to the configuration. (No exception handling!)
        /// </summary>
        /// <param name="type">Type of the new client instance.</param>
        /// <param name="title">Unique title of the new client instance.</param>
        /// <param name="host">Address of the remote server for the client instance.</param>
        /// <param name="port">Port of the remote server for the client instance.</param>
        /// <param name="user">User on the remote server for the client instance.</param>
        /// <param name="password">Password of user on the remote server for the client instance.</param>
        /// <returns>True if client was created.</returns>
        public bool CreateClientInstance(InstanceType type, string title, string host, int port, string user, string password)
        {
            if (type != InstanceType.Unknown)
            {
                IProtocolManager manager;
                switch (type)
                {
                    case InstanceType.FtpsClient:
                        manager = new FtpsRemoteManager(host, port, user, password);
                        break;
                    default:
                        return false;
                }
                register.RegisterInstance(type, title, manager);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Provides a instance of any type of the instance register.
        /// </summary>
        /// <param name="type">Type of the searched instance.</param>
        /// <param name="title">Unique title of searched instance.</param>
        /// <returns>Protocol manager instance if it is known.</returns>
        public IProtocolManager GetInstance(InstanceType type, string title)
        {
            return register.ProvideInstance(type, title) as IProtocolManager;
        }

        /// <summary>
        /// Provides all instances of any type of the instance register.
        /// </summary>
        /// <param name="type">Type of the searched instances.</param>
        /// <returns>Dictionary containing all instances with their title as key.</returns>
        public Dictionary<string, object> GetInstances(InstanceType type)
        {
            Dictionary<string, object> pairs = register.GetRegister(type);
            return pairs ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates new browser window.
        /// </summary>
        /// <param name="linkTitle">Client for which the window is.</param>
        /// <returns>Browser window buid Task.</returns>
        public async Task<BrowserWindow> NewBrowserWindow(string linkTitle, ElectronNET.API.Entities.BrowserWindowOptions options = null)
        {
            // If there are no defined options, use default ones.
            if (options == null)
            {
                options = new ElectronNET.API.Entities.BrowserWindowOptions()
                {
                    Title = linkTitle,
                    Show = false,
                };
            }
            else
            {
                options.Title = options.Title ?? linkTitle;
            }
            Task<BrowserWindow> createWindow = Electron.WindowManager.CreateWindowAsync(options);
            //Dictionary<string, object> managers = GetInstances(ClientTypes);
            //IProtocolManager manager = managers[clientTitle] as IProtocolManager;
            //window.SetParentWindow(Electron.WindowManager.BrowserWindows.First());
            //register.RegisterInstance(InstanceType.BrowserWindow, clientTitle, window);
            return await createWindow;
        }

        /// <summary>
        /// Creates new client browser window for connected server.
        /// </summary>
        /// <param name="linkTitle">Title of the server uplink.</param>
        /// <param name="url"></param>
        public async void NewClientBrowserWindow(string linkTitle, string url)
        {
            //var options = new ElectronNET.API.Entities.BrowserWindowOptions()
            //{
            //    Show = false;
            //}
            BrowserWindow window = await NewBrowserWindow(linkTitle);
            window.LoadURL(url);
            window.OnReadyToShow += () => window.Show(); 
        }
    }
}

using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;
using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Linq;
using JDPDO.SpaceBreeze.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace JDPDO.SpaceBreeze.UI.Models
{
    public class OverviewWindowModel
    {
        /// <summary>
        /// Contains the app's instance register.
        /// </summary>
        private readonly InstanceRegister register;

        /// <summary>
        /// Contains the app's configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Storing current avaiable client managers.
        /// </summary>
        public string ClientTypes { get; set; }

        /// <summary>
        /// Creates a new 'RemoteClientModel' instance.
        /// </summary>
        public OverviewWindowModel(IConfiguration configuration)
        {
            register = InstanceRegister.GetFirstInstanceRegister();
            Configuration = configuration;
        }

        /// <summary>
        /// Returns the instances of a given type from its current subregister.
        /// </summary>
        /// <param name="type">The type of requested instances.</param>
        /// <returns>
        /// Key-value pairs, with id and object of requested type. 
        /// If no subregister was found returns null.
        /// </returns>
        public Dictionary<string, object> GetSubRegister(string type)
        {
            var subRegister = register.GetSubRegister(type);
            return subRegister;
        }

        /// <summary>
        /// Registers an instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <param name="instance"></param>
        public bool RegisterInstance(string type, string title, object instance)
        {
            bool success = false;
            try
            {
                register.RegisterInstance(type, title, instance);
                success = true;
            }
            catch (ArgumentNullException e)
            {
                ExceptionHandler.LogException("Instance and/or type parameter are null.", e);
            }
            catch (ArgumentException e)
            {
                ExceptionHandler.LogException("Id may not be null.", e);
            }
            return success;
        }

        /// <summary>
        /// Provides an instance with given type and unique title.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns>Requested object from subregister or if nothing found null.</returns>
        public object ProvideInstance(string type, string title)
        {
            try
            {
                return register.ProvideInstance(type, title);
            }
            catch (ArgumentNullException e)
            {
                ExceptionHandler.LogException("Type or titel are null.", e);
            }
            catch (KeyNotFoundException e)
            {
                ExceptionHandler.LogException("The property is retrieved. Type and/or id does not exist in the subregister.", e);
            }
            return null;
        }

        /// <summary>
        /// Removes an instance from the registers.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns>True if success, else false.</returns>
        public bool RemoveInstance(string type, string title)
        {
            bool success = false;
            try
            {
                register.RemoveInstance(type, title);
                success = true;
            }
            catch (ArgumentNullException e)
            {
                ExceptionHandler.LogException("Type or titel are null.", e);
            }
            return success;
        }

        public async Task<bool> OpenUplinkWindowAsync(string title, BrowserWindow mainWindow = null, BrowserWindowOptions options = null)
        {
            // Checkout main window.
            mainWindow = mainWindow ?? Electron.WindowManager.BrowserWindows.First();
            // If there are no defined options, use default ones.
            if (options == null)
            {
                options = new ElectronNET.API.Entities.BrowserWindowOptions()
                {
                    Title = title,
                    Show = false,
                };
            }

            // Start creating window.
            Task<BrowserWindow> createWindow = Electron.WindowManager.CreateWindowAsync(options);
            try
            {
                /* Check if wanted uplink was activated at runtime yet. */
                UplinkModel uplink = (UplinkModel)register.ProvideInstance("uplink", title);
                if (!register.InstanceExists(uplink.Type, title))
                {
                    // Init remote manager.
                    IProtocolManager protocolManager = new FtpsRemoteManager(uplink.Host, uplink.Host.UrlGetPort(), uplink.User, uplink.Password);
                    register.RegisterInstance(uplink.Type, title, protocolManager);
                    // Init root directory.
                    ManagedDirectory rootDirectory = protocolManager.GetWorkingDirectory();
                    register.RegisterInstance("rootDirectory", title, rootDirectory);
                }
            }
            catch (ArgumentNullException e) { ExceptionHandler.LogException("Instance and/or type is null.", e); }
            catch (ArgumentException e) { }
            catch (InvalidOperationException e) { }

            /* Create window. */
            BrowserWindow window = await createWindow;
            // End here if creation was not successful.
            if (createWindow.IsFaulted)
            {
                ExceptionHandler.LogException("The window could not be created.", createWindow.Exception);
                return false;
            }
            window.LoadURL(@"/uplink/index/{title}?type={type}");
            window.OnReadyToShow += () => window.Show();
            window.SetParentWindow(mainWindow);

            return true;
        }
    }
}

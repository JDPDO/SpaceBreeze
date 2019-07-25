using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;

namespace JDPDO.SpaceBreeze.UI.Models
{
    public class ServerModel
    {
        InstanceRegister register;

        public ServerModel()
        {
            register = new InstanceRegister();
        }

        /// <summary>
        /// Creates a new client and saves it to the configuration. (No exception handling!)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
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
        /// Provides a client instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public IProtocolManager GetClientInstance(InstanceType type, string title)
        {
            return register.ProvideInstance(type, title) as IProtocolManager;
        }

        public Dictionary<string, object> GetClientInstances(InstanceType type)
        {
            Dictionary<string, object> pairs = register.GetRegister(type);
            return pairs != null ? pairs : new Dictionary<string, object>();
        }
    }
}

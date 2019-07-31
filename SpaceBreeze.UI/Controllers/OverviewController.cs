using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using JDPDO.SpaceBreeze;
using JDPDO.SpaceBreeze.UI.Models;

namespace JDPDO.SpaceBreeze.UI.Controllers
{
    public class OverviewController : Controller
    {
        //InstanceRegister register;
        RemoteClientModel serverClientModel;

        public OverviewController(IConfiguration configuration)
        {
            _configuration = configuration;
            serverClientModel = new RemoteClientModel();
        }

        private IConfiguration _configuration;

        public IActionResult Index()
        {
            return View();
        }

        #region Server links
        public IActionResult Server()
        {
            return View(serverClientModel.GetInstances(InstanceType.FtpsClient));
        }

        public IActionResult AddServerLink(string type, string title, string host, int port, string user, string password)
        {
            if (String.IsNullOrEmpty(type) && String.IsNullOrEmpty(title) && String.IsNullOrEmpty(host) && String.IsNullOrEmpty(user) && port == 0) return NoContent();
            serverClientModel.CreateClientInstance((InstanceType)System.Enum.Parse(typeof(InstanceType), type), title, host, port, user, password);
            return View("Index");
        }

        public IActionResult EditServerLink(string type, string title, string host = null, int port = -1, string user = null, string password = null)
        {
            throw new NotImplementedException();
        }

        public IActionResult RemoveServerLink(string type, string title)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Settings
        public IActionResult Settings()
        {
            return View();
        }
        #endregion
    }
}

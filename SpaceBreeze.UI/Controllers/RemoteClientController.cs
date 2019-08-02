using JDPDO.SpaceBreeze.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using ElectronNET.API;
using System.Text;
using System;
using System.Net;
using System.Net.Http;

namespace JDPDO.SpaceBreeze.UI.Controllers
{
    //[ApiController]
    public class ServerConnectionController : Controller
    {
        private RemoteClientModel model = new RemoteClientModel();

        [HttpGet]
        public IActionResult Index()
        {
            Dictionary<string, object> managers = model.GetInstances(model.ClientTypes);
            return View("Index", managers);
        }

        public IActionResult Open(string id)
        {
            model.NewClientBrowserWindow(id, Url.Action("Index", "RemoteClient"));
            return Index();
        }
        public IActionResult Info(string id)
        {
            return Index();
        }
        public IActionResult Delete(string id)
        {
            return Index();
        }

        [HttpGet]
        public HttpResponseMessage Add(string type, string title, string host, int port, string user, string password, string a_type)
        {
            if (String.IsNullOrEmpty(type) && String.IsNullOrEmpty(title) && String.IsNullOrEmpty(host) && String.IsNullOrEmpty(user) && port == 0) return new HttpResponseMessage(HttpStatusCode.BadRequest);
            model.CreateClientInstance((InstanceType)System.Enum.Parse(typeof(InstanceType), type), title, host, port, user, password);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public IActionResult Edit(string type, string title, string host = null, int port = -1, string user = null, string password = null)
        {
            throw new NotImplementedException();
        }
    }
}
using JDPDO.SpaceBreeze.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JDPDO.SpaceBreeze.UI.Controllers
{
    public class ServerController : Controller
    {
        ServerModel model = new ServerModel();

        public IActionResult Index()
        {
            InstanceType clientTypes = InstanceType.FtpsClient;
            Dictionary<string, object> managers = model.GetClientInstances(clientTypes);
            return View("Index", managers);
        }

        [Route("/Server/Open", Name = "ServerOpen")]
        public IActionResult Open(string id)
        {
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
    }
}
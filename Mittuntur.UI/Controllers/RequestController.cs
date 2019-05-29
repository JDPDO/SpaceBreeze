using JDPDO.Mittuntur.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace JDPDO.Mittuntur.UI.Controllers
{
    [ApiController, Route("Request")]
    public class RequestController : ControllerBase
    {
        [HttpGet("{uri}")]
        public IActionResult GetDirectoryContent(string uri)
        {
            return null;
        }
    }
}
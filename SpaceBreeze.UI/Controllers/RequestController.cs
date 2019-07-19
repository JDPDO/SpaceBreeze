using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JDPDO.SpaceBreeze.UI.Controllers
{
    [ApiController, Route("Request")]
    public class RequestController : ControllerBase
    {
        [HttpGet("{uri}")]
        public IActionResult GetDirectoryContentIActionResult(string uri)
        {
            return null;
        }

        public IEnumerable<IFileSystemItem> GetDirectoryContent(string uri)
        {
            FileTree<Directory> fileTree = new FileTree<Directory>(uri);
            return fileTree.GetChildren();
        }
    }
}
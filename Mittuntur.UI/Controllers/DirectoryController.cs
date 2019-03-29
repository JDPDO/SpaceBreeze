using JDPDO.Mittuntur.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IO = System.IO;

namespace JDPDO.Mittuntur.UI.Controllers
{
    public class DirectoryController : Controller
    {
        public DirectoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration;

        public IActionResult Index()
        {
            string path = _configuration["localhost:path"];
            if (path == String.Empty) _configuration["localhost:path"] = IO.Directory.GetDirectoryRoot(IO.Directory.GetCurrentDirectory());
            return Index(path);
        }

        public IActionResult Index(string path)
        {
            FileTree<LocalDirectory> tree = new FileTree<LocalDirectory>(path);
            IHtmlContentBuilder items = DirectoryView.GetHtmlListItems(tree);

            foreach (var item in tree.GetSubDirectories())
            {
                string channel = $"subdirectory-{item.Name}";
                string responseChannel = $"subdirectory-{item.Name}";
                items.AppendHtml(
                    Ipc.RendererSendOnTrigger(channel, item.Name, item.GetUri().AbsoluteUri));
                items.AppendHtml(
                    Ipc.RendererOnIpc(responseChannel, "directory-overview"));

                IHtmlContentBuilder nextList = DirectoryView.GetHtmlListItems(tree.GetSubFileTree(item.GetUri().AbsoluteUri));
                IHtmlContentBuilder error = new HtmlContentBuilder().Append("Error.");

                Ipc.On(channel, responseChannel, window: null, data: nextList ?? error);
            }
            return View(items);
        }
    }
}
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
    public class DirectoryController : Controller
    {
        public IActionResult Index(string path = @"D:\Downloads")
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
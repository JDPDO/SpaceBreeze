using JDPDO.SpaceBreeze.UI.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JDPDO.SpaceBreeze.UI.Controllers
{
    public class DirectoryController : Controller
    {
        public DirectoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration;

        //// If configuration is enabled
        //public IActionResult Index()
        //{
        //    string path = _configuration["localhost:path"];
        //    if (path == String.Empty) _configuration["localhost:path"] = IO.Directory.GetDirectoryRoot(IO.Directory.GetCurrentDirectory());
        //    return Index(path);
        //}

        public IActionResult Index(string path = @"D:\Downloads")
        {
            FileTree<Directory> tree = new FileTree<Directory>(path);
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
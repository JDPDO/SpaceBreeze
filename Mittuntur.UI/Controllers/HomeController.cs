using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JDPDO.Mittuntur.UI.Models;
using M = JDPDO.Mittuntur;

namespace Mittuntur.UI.Controllers
{
    public class HomeController : Controller
    {
        readonly string startPage = "Folder";
        const string rootDirectory = @"C:\";

        public IActionResult Index()
        {
            // Configure site attributes
            ViewData["Title"] = startPage;

            // Setting tree of rootDirectory up.
            M.FileTree<M.LocalDirectory> tree = new M.FileTree<M.LocalDirectory>(rootDirectory);

            // Hand over start page name and tree object for data visualisation.
            return View(startPage, tree);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet("{path}")]
        public IActionResult Folder(string path)
        {
            // Configure site attributes
            ViewData["Title"] = "Folder";

            // Setting file tree up
            M.FileTree<M.LocalDirectory> tree = new M.FileTree<M.LocalDirectory>(path);

            return View(tree);
        }

        //public IActionResult Folder(string path = rootDirectory)
        //{
        //    // Setting tree of rootDirectory up.
        //    M.FileTree<M.LocalDirectory> tree = new M.FileTree<M.LocalDirectory>(path);

        //    // Initialize possible events
        //    foreach (var directory in tree.GetSubDirectories())
        //    {
        //        object nextTree() => Folder(directory.Name);
        //        //Action<IActionResult> nextTree = new Action((result) => Folder(directory.GetUri().AbsolutePath));
        //        Ipc.On(directory.Name, new Action( () => nextTree() ));
        //    }

        //    // Hand over start page name and tree object for data visualisation.
        //    return View(tree);
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

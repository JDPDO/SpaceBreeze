using Microsoft.AspNetCore.Html;
using System;
using System.Text.Encodings.Web;

namespace JDPDO.SpaceBreeze.UI.Models
{
    public class DirectoryViewModel
    {
        //List<FileTree<Directory>> localTrees;

        //public DirectoryView()
        //{
        //    localTrees = new List<FileTree<Directory>>();
        //    foreach (var drive in Environment.GetLogicalDrives())
        //    {
        //        localTrees.Add(new FileTree<Directory>(new LocalDirectory(drive)));
        //    }
        //}

        public static IHtmlContentBuilder GetHtmlListItems(FileTree<Directory> tree)
        {
            IHtmlContentBuilder builder = new HtmlContentBuilder();

            try
            {
                foreach (var item in tree.GetChildren())
                {
                    HtmlString name = new HtmlString(item.Name);
                    HtmlString htmlString = new HtmlString(
                        "<a class=\"uk-link uk-link-text\" id=\"" + name + "\">" +
                        name +
                        "</a><br>");
                    builder.AppendHtml(htmlString);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }

            return builder;
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}

using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace JDPDO.Mittuntur.UI.Models
{
    public class DirectoryView
    {
        public static IHtmlContentBuilder GetHtmlListItems(FileTree<LocalDirectory> tree)
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
                ExeptionHandler.NewException(e);
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

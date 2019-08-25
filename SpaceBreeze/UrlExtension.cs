using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JDPDO.SpaceBreeze.Extensions
{
    public static class UrlExtension
    {
        public static int UrlGetPort(this string url)
        {
            Regex regex = new Regex(@"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/", RegexOptions.None, TimeSpan.FromMilliseconds(150));
            Match match = regex.Match(url);
            if (match.Success) return Convert.ToInt32(match.Groups["port"]);
            return -1;
        }
    }
}

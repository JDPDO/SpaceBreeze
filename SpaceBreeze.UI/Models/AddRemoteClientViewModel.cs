using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;
using ElectronNET.API;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace JDPDO.SpaceBreeze.UI.ViewModels
{
    public class AddRemoteClientViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Host (Domain/IP/URL)")]
        public string Host { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

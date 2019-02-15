using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JDPDO.Mittuntur;

namespace JDPDO.Mittuntur.UI.Models
{
    public class FileSystemItem
    {
        public FileAttributes Attributes { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastAccess { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastWrite { get; set; }
        public string Name { get; set; }
    }
}

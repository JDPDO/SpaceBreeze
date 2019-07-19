using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace JDPDO.SpaceBreeze.UI.Models
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

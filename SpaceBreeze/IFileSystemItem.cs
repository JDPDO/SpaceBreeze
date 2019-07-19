using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JDPDO.SpaceBreeze
{
    public interface IFileSystemItem
    {
        /* 
         * Attributes
         */
        // Attributes of elment.
        FileAttributes Attributes { get; set; }

        // FileInfo obejct
        FileInfo FileInfo { get; }

        // Access, create and write times.
        DateTime LastAccess { get; set; }
        DateTime Created { get; set; }
        DateTime LastWrite { get; set; }

        // Name of element without path, but with ending.
        string Name { get; set; }
        
        /*
         * Methods
         */
        // Traslations
        // bool CopyTo(string path);
        bool Delete();
        // bool MoveTo(string path);

        // Element information
        Uri GetUri();
        bool IsDirectory();
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JDPDO.Mittuntur
{
    public interface IFileSystemItem
    {
        /* 
         * Attributes
         */
        // Attributes of elment.
        FileAttributes Attributes { get; set; }

        // Access and write times.
        DateTime LastAccess { get; set; }
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

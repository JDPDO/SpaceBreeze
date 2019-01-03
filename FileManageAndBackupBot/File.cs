using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public class File
    {
        private Uri uri;

        public Uri Uri { get { return uri; } set { uri = value; } }

        public File(string uri)
        {
            
        }
    }
}

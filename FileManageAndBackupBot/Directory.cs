using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Text;

namespace FileManageAndBackupBot
{
    public abstract class Directory : IO.FileSystemInfo
    {
        // private fields
        private Uri uri;
        //private IO.DirectoryInfo directoryInfo;

        /// <summary>
        /// Get uri.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return uri;
            }
        }

        public Directory()
        {
        }
    }
}

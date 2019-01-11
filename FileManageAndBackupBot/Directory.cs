using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    class Directory : IO.FileSystemInfo, IFileSystemItem
    {
        // private fields
        private Uri uri;
        private IO.DirectoryInfo directoryInfo;

        // public properties

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

        /// <summary>
        /// Returns true if directory physically exists.
        /// </summary>
        public override bool Exists => directoryInfo.Exists;

        /// <summary>
        /// Returns name of directory.
        /// </summary>
        public override string Name => directoryInfo.Name;

        /// <summary>
        /// Sets up directory object with defined path. Create Directory if location is not avaiable.
        /// </summary>
        /// <param name="path"></param>
        public Directory(string path)
        {
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                uri = new Uri(path);
                directoryInfo = new IO.DirectoryInfo(path);
                if (!Exists) IO.Directory.CreateDirectory(path);
            }
            else throw new Exception("Directory path is wrong formed.");
        }

        public Uri GetUri() => Uri;

        /// <summary>
        /// Deletes directory recursivly.
        /// </summary>
        public override void Delete()
        {
            try
            {
                directoryInfo.Delete(true);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: An error occured.", e.GetType().Name);
            }
        }
    }
}

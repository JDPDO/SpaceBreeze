using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public class Directory : IO.FileSystemInfo, IFileSystemItem
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
            //if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            //{
                uri = new Uri(path);
                directoryInfo = new IO.DirectoryInfo(path);
                if (!Exists) IO.Directory.CreateDirectory(path);
            //}
            //else throw new Exception("Directory path is wrong formed.");
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

        public string[] GetChildrenNames()
        {
            var children = GetChildren();
            List<string> output = new List<string>();
            foreach (var child in children) output.Add(child.Name);
            return output.ToArray();
        }

        public IO.FileSystemInfo[] GetChildren()
        {
            IO.DirectoryInfo[] directories = directoryInfo.GetDirectories();
            IO.FileInfo[] files = directoryInfo.GetFiles();
            List<IO.FileSystemInfo> children = new List<IO.FileSystemInfo>();

            foreach (var directory in directories) children.Add(directory);
            foreach (var file in files) children.Add(file);
            return children.ToArray();
        }
    }
}

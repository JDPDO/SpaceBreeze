using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public class LocalDirectory : Directory, IFileSystemItem
    {
        // private fields
        private Uri uri;
        private IO.DirectoryInfo directoryInfo;

        /// <summary>
        /// Sets up directory object with defined path. Create Directory if location is not avaiable.
        /// </summary>
        /// <param name="path">Path of the directory location.</param>
        public LocalDirectory(string path)
        {
                uri = new Uri(path);
                directoryInfo = new IO.DirectoryInfo(path);
                if (!Exists) IO.Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Sets up directory object with defined uri object. Create Directory if location is not avaiable.
        /// </summary>
        /// <param name="uri">Path of the directory location.</param>
        public LocalDirectory(Uri uri)
        {
            directoryInfo = new IO.DirectoryInfo(uri.AbsolutePath);
            if (!Exists) IO.Directory.CreateDirectory(uri.AbsolutePath);
        }

        /// <summary>
        /// Returns true if directory physically exists.
        /// </summary>
        public override bool Exists => directoryInfo.Exists;

        /// <summary>
        /// Returns name of directory.
        /// </summary>
        public override string Name => directoryInfo.Name;

#region IFileSystemItem implementation

        /// <summary>
        /// Returns uri of managed directory.
        /// </summary>
        /// <returns>Uri object.</returns>
        public Uri GetUri() => Uri;

        /// <summary>
        /// Returns true.
        /// </summary>
        /// <returns>Retruns true.</returns>
        public bool IsDirectory() => true;

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

        /// <summary>
        /// Moves directory to new path
        /// </summary>
        public void Move(string destPath)
        {
            try
            {
                IO.Directory.Move(uri.AbsolutePath, destPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: An error occured.", e.GetType().Name);
            }
        }

#endregion

        /// <summary>
        /// Retruns the names of the sub files and folders.
        /// </summary>
        /// <returns>Array of strings with one name per field.</returns>
        public string[] GetChildrenNames()
        {
            var children = GetChildren();
            List<string> output = new List<string>();
            foreach (var child in children) output.Add(child.Name);
            return output.ToArray();
        }
        
        /// <summary>
        /// Retruns all children of the directory object managed folder.
        /// </summary>
        /// <returns>Array of FileSystemInfo objects.</returns>
        public IO.FileSystemInfo[] GetChildren()
        {
            return directoryInfo.GetFileSystemInfos();
        }
    }
}

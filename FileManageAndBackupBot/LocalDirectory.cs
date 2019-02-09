using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public class LocalDirectory : IDirectory<LocalDirectory>
    {
        // private fields
        private IO.DirectoryInfo directoryInfo;

        /// <summary>
        /// Sets up directory object with defined path. Create Directory if location is not avaiable.
        /// </summary>
        /// <param name="path">Path of the directory location.</param>
        public LocalDirectory(string path)
        {
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

        public LocalDirectory(IO.DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        /// <summary>
        /// Returns true if directory physically exists.
        /// </summary>
        public bool Exists => directoryInfo.Exists;

        /// <summary>
        /// Returns and sets name of current directory.
        /// </summary>
        string IDirectory<LocalDirectory>.Name { get => directoryInfo.Name; }

        public string FullName => throw new NotImplementedException();

        #region IDirectory implementation

        /// <summary>
        /// Returns uri of managed directory.
        /// </summary>
        /// <returns>Uri object.</returns>
        public Uri GetUri() => new Uri(directoryInfo.FullName);

        /// <summary>
        /// Returns true.
        /// </summary>
        /// <returns>Retruns true.</returns>
        public bool IsDirectory() => true;

        /// <summary>
        /// Deletes directory recursivly.
        /// </summary>
        public void Delete()
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
                IO.Directory.Move(directoryInfo.FullName, destPath);
                directoryInfo.Refresh();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: An error occured.", e.GetType().Name);
            }
        }

        /// <summary>
        /// Retruns all children of the directory object managed folder.
        /// </summary>
        /// <returns>Array of FileSystemInfo objects.</returns>
        public IO.FileSystemInfo[] GetFileSystemInfos()
        {
            throw new NotImplementedException();
        }

        public LocalDirectory[] GetDirectories()
        {
            IO.DirectoryInfo[] directories = directoryInfo.GetDirectories();
            List<LocalDirectory> localDirectories = new List<LocalDirectory>();
            foreach(var directory in directories)
            {
                localDirectories.Add(new LocalDirectory(directory));
            }
            return localDirectories.ToArray();
        }

        public IO.FileInfo[] GetFiles()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Retruns the names of the sub files and folders.
        /// </summary>
        /// <returns>Array of strings with one name per field.</returns>
        public string[] GetChildrenNames()
        {
            var children = GetFileSystemInfos();
            List<string> output = new List<string>();
            foreach (var child in children) output.Add(child.Name);
            return output.ToArray();
        }

        bool IFileSystemItem.Exists()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;
using IO = System.IO;

namespace JDPDO.Mittuntur
{
    public class LocalDirectory : Directory
    {
        private IO.DirectoryInfo directoryInfo;

        /// <summary>
        /// Sets up directory object with defined path. Create Directory if location is not avaiable.
        /// </summary>
        /// <param name="path">Path of the directory location.</param>
        public LocalDirectory(string path) : base(path)
        {
            try
            {
                directoryInfo = new IO.DirectoryInfo(path);
            }
            catch (Exception e)
            {
                ExeptionHandler.NewException(e);
            }
        }

        /// <summary>
        /// Sets up directory object with defined uri object. Create Directory if location is not avaiable.
        /// </summary>
        /// <param name="uri">Path of the directory location.</param>
        public LocalDirectory(Uri path) : base(path)
        {
            try
            {
                directoryInfo = new IO.DirectoryInfo(path.AbsoluteUri);
            }
            catch (Exception e)
            {
                ExeptionHandler.NewException(e);
            }
        }

        public LocalDirectory(IO.DirectoryInfo directoryInfo) : base (directoryInfo.FullName)
        {
            this.directoryInfo = directoryInfo;
        }

        /// <summary>
        /// Provides attributes of directory.
        /// </summary>
        public override IO.FileAttributes Attributes { get => directoryInfo.Attributes; set => directoryInfo.Attributes = value; }

        /// <summary>
        /// Deletes directory, if it is empty.
        /// </summary>
        public override bool Delete()
        {
            directoryInfo.Delete();
            directoryInfo.Refresh();
            return !directoryInfo.Exists;
        }

        /// <summary>
        /// Deletes directory.
        /// </summary>
        /// <param name="recusive">Deletes directory recusivly, if it is true.</param>
        /// <returns></returns>
        public override bool Delete(bool recusive)
        {
            directoryInfo.Delete(recusive);
            directoryInfo.Refresh();
            return !directoryInfo.Exists;
        }

        /// <summary>
        /// Enumerates direct children of current directory.
        /// </summary>
        /// <returns>Returns IEnumerable object.</returns>
        public override IEnumerable<IFileSystemItem> EnumerateChildren()
        {
            IEnumerable<IO.FileSystemInfo> fileSystemInfos = directoryInfo.EnumerateFileSystemInfos();
            Queue<IFileSystemItem> items = new Queue<IFileSystemItem>();
            foreach(var element in fileSystemInfos)
            {
                if (element is IO.DirectoryInfo) items.Enqueue(new LocalDirectory(element as IO.DirectoryInfo));
                else items.Enqueue(new File(element as IO.FileInfo));
            }
            return items;
        }

        /// <summary>
        /// Enumerates direct subdirectories of current directory.
        /// </summary>
        /// <returns>Returns IEnumerable object.</returns>
        public override IEnumerable<Directory> EnumerateDirectories()
        {
            IEnumerable<IO.DirectoryInfo> directoryInfos = directoryInfo.EnumerateDirectories();
            Queue<Directory> items = new Queue<Directory>();
            foreach (var directory in directoryInfos) items.Enqueue(new LocalDirectory(directory) as Directory);
            return items;
        }

        /// <summary>
        /// Enumerates direct subfiles of current directory.
        /// </summary>
        /// <returns>Returns IEnumerable object.</returns>
        public override IEnumerable<File> EnumerateFiles()
        {
            IEnumerable<IO.FileInfo> fileInfos = directoryInfo.EnumerateFiles();
            Queue<File> items = new Queue<File>();
            foreach (var file in fileInfos) items.Enqueue(new File(file));
            return items;
        }


        /// <summary>
        /// Returns an array object with all direct children.
        /// </summary>
        /// <returns>Returns an IFileSystemItem array.</returns>
        public override IFileSystemItem[] GetChildren()
        {
            return new List<IFileSystemItem>(EnumerateChildren()).ToArray();
        }

        /// <summary>
        /// Returns an array object with all direct subdirectories.
        /// </summary>
        /// <returns>Returns an Directory array.</returns>
        public override Directory[] GetDirectories()
        {
            return new List<Directory>(EnumerateDirectories()).ToArray();
        }

        /// <summary>
        /// Returns an array object with all direct subfiles.
        /// </summary>
        /// <returns>Returns an File array.</returns>
        public override File[] GetFiles()
        {
            return new List<File>(EnumerateFiles()).ToArray();
        }
    }
}

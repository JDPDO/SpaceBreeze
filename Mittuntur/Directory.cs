using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace JDPDO.Mittuntur
{
    public abstract class Directory : IFileSystemItem
    {
        /// <summary>
        /// Containing path of the directory.
        /// And provide various uri Attributes for easier access.
        /// </summary>
        protected Uri uri;

        /// <summary>
        /// Containing all directory attributes in binary representation.
        /// </summary>
        IO.FileAttributes attributes;

        public Directory(string path)
        {
            uri = new Uri(path);
        }

        public Directory(Uri path)
        {
            uri = path;
        }

        #region IFileSystemItem implementation.

        /* 
         * IFileSystemItem: Attributes
         */

        /// <summary>
        /// Provides attributes of directory.
        /// </summary>
        public virtual IO.FileAttributes Attributes { get => attributes; set => attributes = value; }

        /// <summary>
        /// Provides last access time of directory.
        /// </summary>
        public virtual DateTime LastAccess
        {
            get
            {
                return IO.Directory.GetLastAccessTime(uri.AbsolutePath);
            }
            set
            {
                IO.Directory.SetLastAccessTime(uri.AbsolutePath, value);
            }
        }

        /// <summary>
        /// Provides last write time of directory.
        /// </summary>
        public virtual DateTime LastWrite
        {
            get
            {
                return IO.Directory.GetLastWriteTime(uri.AbsolutePath);
            }
            set
            {
                IO.Directory.SetLastWriteTime(uri.AbsolutePath, value);
            }
        }

        /// <summary>
        /// Provides the name of managed directory.
        /// </summary>
        public virtual string Name
        {
            get
            {
                string[] parts = uri.AbsoluteUri.Split('/', '\\');
                return parts[parts.Length - 1];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /* 
         * IFileSystemItem: Methods
         */

        // public abstract bool CopyTo(string path);

        /// <summary>
        /// Deletes the pysical representaiton of the Directory, if it is empty.
        /// </summary>
        /// <returns>True, if process succeeded.</returns>
        public abstract bool Delete();

        /// <summary>
        /// Returns current uri of directory.
        /// </summary>
        /// <returns>Uri object with directory location.</returns>
        public Uri GetUri() => uri;

        /// <summary>
        /// Returns everytime true.
        /// </summary>
        /// <returns>Returns true.</returns>
        public bool IsDirectory() => true;

        /// <summary>
        /// Returns FileInfo. Not implemented yet!
        /// </summary>
        public IO.FileInfo FileInfo { get => throw new NotImplementedException(); }

        /// <summary>
        /// Provides the creation time of the managed directory.
        /// </summary>
        public abstract DateTime Created { get; set; }

        // public abstract bool MoveTo(string path);

        #endregion

        /* 
         * Methods for direct directory management and listing.
         */

        /// <summary>
        /// Deletes the pysical representaiton of the Directory.
        /// </summary>
        /// <param name="recusive">Only deletes non empty directories, if true.</param>
        /// <returns></returns>
        public abstract bool Delete(bool recusive);

        /// <summary>
        /// Returns an IEnumerable object containing all direct children elements.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IFileSystemItem> EnumerateChildren();

        /// <summary>
        /// Returns an IEnumerable object containing all direct children directories.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Directory> EnumerateDirectories();

        /// <summary>
        /// Returns an IEnumerable object containing all direct children files.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<File> EnumerateFiles();

        /// <summary>
        /// Returns an array containing all direct children elements.
        /// </summary>
        /// <returns></returns>
        public abstract IFileSystemItem[] GetChildren();

        /// <summary>
        /// Returns an array containing all direct children directories.
        /// </summary>
        /// <returns></returns>
        public abstract Directory[] GetDirectories();

        /// <summary>
        /// Returns an array containing all direct children files.
        /// </summary>
        /// <returns></returns>
        public abstract File[] GetFiles();

        /*
         * Methods for sub-directory and -file management.
         */

        ///// <summary>
        ///// Deletes child if it is a file or an empty directory.
        ///// </summary>
        ///// <param name="child">To deleting child.</param>
        ///// <returns>True if deletion successful.</returns>
        //public abstract bool DeleteChild(IFileSystemItem child);

        /// <summary>
        /// Deletes child if it is a file or an empty directory and deletes directory if 'recusive' is ture.
        /// </summary>
        /// <param name="child">To deleting child.</param>
        /// <param name="recusive">Determine if filled directory may deleted.</param>
        /// <returns>True if deletion successful.</returns>
        public abstract bool DeleteChild(IFileSystemItem child, bool recusive = false);

        public abstract IO.FileStream GetFileStream(File child);
    }
}

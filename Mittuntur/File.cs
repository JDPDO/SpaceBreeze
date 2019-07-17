using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace JDPDO.Mittuntur
{
    public class File : IFileSystemItem
    {
        // private fields
        private IO.FileInfo fileInfo;

        public IO.FileInfo FileInfo { get; }

        /// <summary>
        /// Creates new File object with predefined uri string.
        /// </summary>
        /// <param name="uri">Uri string refering to file.  </param>
        public File(string uri)
        {
            Uri _uri = new Uri(uri);
            if (_uri.IsFile)
            {
                fileInfo = new IO.FileInfo(uri);
            }
        }

        /// <summary>
        /// Creates new File object with predefined uri object.
        /// </summary>
        /// <param name="uri">Uri object refering to file.</param>
        public File(Uri uri)
        {
            fileInfo = new IO.FileInfo(uri.AbsoluteUri);
        }

        /// <summary>
        /// Creates new File object from a FileInfo object.
        /// </summary>
        /// <param name="fileInfo">A FileInfo object containing informations for current file.</param>
        public File(IO.FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                this.fileInfo = fileInfo;
            }
        }

        /// <summary>
        /// Provides Attributes of file.
        /// </summary>
        public IO.FileAttributes Attributes { get => fileInfo.Attributes; set => fileInfo.Attributes = value; }

        /// <summary>
        /// Provides last access time of file.
        /// </summary>
        public DateTime LastAccess { get => fileInfo.LastAccessTime; set => fileInfo.LastAccessTime = value; }

        /// <summary>
        /// Provides last write time of file.
        /// </summary>
        public DateTime LastWrite { get => fileInfo.LastWriteTime; set => fileInfo.LastWriteTime = value; }

        /// <summary>
        /// Provides the name of managed file.
        /// </summary>
        public virtual string Name
        {
            get => fileInfo.Name;
            set => Name = value;
        }

        /// <summary>
        /// Provides creation time.
        /// </summary>
        public DateTime Created { get => fileInfo.CreationTime; set => fileInfo.CreationTime = value; }

        /// <summary>
        /// Deletes the pysical representaiton of the file.
        /// </summary>
        /// <returns>True, if process succeeded.</returns>
        public bool Delete()
        {
            fileInfo.Delete();
            fileInfo.Refresh();
            return !fileInfo.Exists;
        }

        /// <summary>
        /// Returns current uri of file.
        /// </summary>
        /// <returns>Uri object with file location.</returns>
        public Uri GetUri()
        {
            return new Uri(fileInfo.FullName);
        }

        /// <summary>
        /// Returns everytime false.
        /// </summary>
        /// <returns>Returns false.</returns>
        public bool IsDirectory()
        {
            return false;
        }
    }


    /* Old implementation. */
    //public class File : IO.FileSystemInfo, IFileSystemItem
    //{
    //    // private fields
    //    private Uri uri;
    //    private IO.FileInfo fileInfo;

    //    // public property
    //    public Uri Uri { get { return uri; }
    //        set
    //        {
    //            if (uri.IsFile)
    //            {
    //                uri = value;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Returns true if file exists.
    //    /// </summary>
    //    public override bool Exists => fileInfo.Exists;

    //    /// <summary>
    //    /// Returns file name.
    //    /// </summary>
    //    public override string Name => fileInfo.Name;

    //    /// <summary>
    //    /// Creates new File object with predefined uri string.
    //    /// </summary>
    //    /// <param name="uri">Uri string refering to file.  </param>
    //    public File(string uri)
    //    {
    //        Uri _uri = new Uri(uri);
    //        if (_uri.IsFile)
    //        {
    //            this.uri = _uri;
    //            fileInfo = new IO.FileInfo(uri);
    //        }
    //    }

    //    /// <summary>
    //    /// Creates new File object with predefined uri object.
    //    /// </summary>
    //    /// <param name="uri">Uri object refering to file.</param>
    //    public File(Uri uri)
    //    {
    //        if (uri.IsFile) this.uri = uri;
    //        fileInfo = new IO.FileInfo(uri.AbsoluteUri);
    //    }

    //    public File(IO.FileInfo fileInfo)
    //    {
    //        if (fileInfo.Exists)
    //        {
    //            uri = new Uri(fileInfo.FullName);
    //            this.fileInfo = fileInfo;
    //        }
    //    }

    //    /// <summary>
    //    /// Copies the current file of file object and sets the current file uri to {destFileUri}.
    //    /// </summary>
    //    /// <param name="destFileUri">Location uri of copied file.</param>
    //    /// <param name="overwrite">Defines, if occupied file destination should be overwritten.</param>
    //    /// <returns>Returns true if job finished without error.</returns>
    //    public bool CopyFile(string destFileUri, bool overwrite)
    //    {
    //        Uri newUri = new Uri(destFileUri);
    //        try
    //        {
    //            IO.File.Copy(uri.AbsoluteUri, newUri.AbsoluteUri);
    //            uri = newUri;
    //        }
    //        catch (IO.IOException e)
    //        {
    //            Console.Error.WriteLine(
    //                "{0}: The write operation could not be performed" +
    //                "because the specified part of the file is not avaiable.",
    //                e.GetType().Name);
    //            return false;
    //            //throw e;
    //        }
    //        catch (Exception e)
    //        {
    //            Console.Error.WriteLine("{0}: An error occured.",
    //                e.GetType().Name);
    //            return false;
    //            //throw e;
    //        }
    //        return true;
    //    }

    //    /// <summary>
    //    /// Copies the current file of file object and sets the current file uri to {destFileUri},
    //    /// if file destination is not occupied.
    //    /// </summary>
    //    /// <param name="destFileUri">Location uri of copied file.</param>
    //    /// <returns>Returns true if job finished without error.</returns>
    //    public bool CopyFile(string destFileUri)
    //    {
    //        return CopyFile(destFileUri, false);
    //    }

    //    /// <summary>
    //    /// Moves the current file of file object and sets the current file uri to {destFileUri}.
    //    /// </summary>
    //    /// <param name="destFileUri">Location uri of moved file.</param>
    //    /// <param name="overwrite">Defines, if occupied file destination should be overwritten.</param>
    //    /// <returns>Returns true if job finished without error.</returns>
    //    public bool MoveFile(string destFileUri, bool overwrite)
    //    {
    //        bool isCopied = CopyFile(destFileUri, overwrite);
    //        if (isCopied)
    //        {
    //            IO.File.Delete(uri.AbsoluteUri);
    //            return true;
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// Moves the current file of file object and sets the current file uri to {destFileUri}.
    //    /// </summary>
    //    /// <param name="destFileUri">Location uri of copied file.</param>
    //    /// <returns>Returns true if job finished without error.</returns>
    //    public bool MoveFile(string destFileUri)
    //    {
    //        return MoveFile(destFileUri, false);
    //    }

    //    /// <summary>
    //    /// Rename the current file of file object.
    //    /// </summary>
    //    /// <param name="destFileName">New file name.</param>
    //    /// <returns>Returns true if job finished without error.</returns>
    //    public bool ChangeFileName(string destFileName, bool overwrite)
    //    {
    //        return MoveFile(uri.AbsolutePath + destFileName, overwrite);
    //    }

    //    /// <summary>
    //    /// Rename the current file of file object, if file destination is not occupied.
    //    /// </summary>
    //    /// <param name="destFileName">New file name.</param>
    //    /// <returns>Returns true if job finished without error.</returns>
    //    public bool ChangeFileName(string destFileName)
    //    {
    //        return ChangeFileName(destFileName, false);
    //    }

    //    /// <summary>
    //    /// Returns file location uri.
    //    /// </summary>
    //    public Uri GetUri() => uri;

    //    /// <summary>
    //    /// Returns all file attributes.
    //    /// </summary>
    //    public IO.FileAttributes GetFileAttributes() => new IO.FileInfo(uri.AbsoluteUri).Attributes;

    //    /// <summary>
    //    /// Deletes file.
    //    /// </summary>
    //    public override void Delete()
    //    {
    //        try
    //        {
    //            IO.File.Delete(uri.AbsolutePath);
    //        }
    //        catch (Exception e)
    //        {
    //            Console.Error.WriteLine("{0}: An error occured.", e.GetType().Name);
    //        }
    //    }

    //    /// <summary>
    //    /// Returns false.
    //    /// </summary>
    //    /// <returns>Returns false.</returns>
    //    public bool IsDirectory()
    //    {
    //        return false;
    //    }

    //    public void Move(string destPath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //bool IFileSystemItem.Exists()
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}
}

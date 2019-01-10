using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public class File
    {
        private Uri uri;

        public Uri Uri { get { return uri; }
            set
            {
                if (uri.IsFile)
                {
                    uri = value;
                }
            }
        }

        public File(string uri)
        {
            Uri _uri = new Uri(uri);
            if (_uri.IsFile) this.uri = _uri;
        }

        /// <summary>
        /// Copies the current file of file object and sets the current file uri to {destFileUri}.
        /// </summary>
        /// <param name="destFileUri">Location uri of copied file.</param>
        /// <param name="overwrite">Defines, if occupied file destination should be overwritten.</param>
        /// <returns>Returns true if job finished without error.</returns>
        public bool CopyFile(string destFileUri, bool overwrite)
        {
            Uri newUri = new Uri(destFileUri);
            try
            {
                IO.File.Copy(uri.AbsoluteUri, newUri.AbsoluteUri);
                uri = newUri;
            }
            catch (IO.IOException e)
            {
                Console.Error.WriteLine(
                    "{0}: The write operation could not be performed" +
                    "because the specified part of the file is not avaiable.",
                    e.GetType().Name);
                return false;
                //throw e;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: An error occured",
                    e.GetType().Name);
                return false;
                //throw e;
            }
            return true;
        }

        /// <summary>
        /// Copies the current file of file object and sets the current file uri to {destFileUri},
        /// if file destination is not occupied.
        /// </summary>
        /// <param name="destFileUri">Location uri of copied file.</param>
        /// <returns>Returns true if job finished without error.</returns>
        public bool CopyFile(string destFileUri)
        {
            return CopyFile(destFileUri, false);
        }

        /// <summary>
        /// Moves the current file of file object and sets the current file uri to {destFileUri}.
        /// </summary>
        /// <param name="destFileUri">Location uri of moved file.</param>
        /// <param name="overwrite">Defines, if occupied file destination should be overwritten.</param>
        /// <returns>Returns true if job finished without error.</returns>
        public bool MoveFile(string destFileUri, bool overwrite)
        {
            bool isCopied = CopyFile(destFileUri, overwrite);
            if (isCopied)
            {
                IO.File.Delete(uri.AbsoluteUri);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the current file of file object and sets the current file uri to {destFileUri}.
        /// </summary>
        /// <param name="destFileUri">Location uri of copied file.</param>
        /// <returns>Returns true if job finished without error.</returns>
        public bool MoveFile(string destFileUri)
        {
            return MoveFile(destFileUri, false);
        }

        /// <summary>
        /// Rename the current file of file object.
        /// </summary>
        /// <param name="destFileName">New file name.</param>
        /// <returns>Returns true if job finished without error.</returns>
        public bool ChangeFileName(string destFileName, bool overwrite)
        {
            return MoveFile(uri.AbsolutePath + destFileName, overwrite);
        }

        /// <summary>
        /// Rename the current file of file object, if file destination is not occupied.
        /// </summary>
        /// <param name="destFileName">New file name.</param>
        /// <returns>Returns true if job finished without error.</returns>
        public bool ChangeFileName(string destFileName)
        {
            return ChangeFileName(destFileName, false);
        }
    }
}

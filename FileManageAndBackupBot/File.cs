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

        public bool MoveFile(string destFileName)
        {
            Uri newUri = new Uri(this.uri.AbsolutePath + destFileName);
            try
            {
                IO.File.Move(uri.AbsoluteUri, newUri.AbsoluteUri);
            }
            catch (IO.IOException e)
            {
                Console.Error.WriteLine(
                    "{0}: The write operation could not be performed" +
                    "because the specified part of the file is not avaiable.",
                    e.GetType().Name);
                return false;
                throw e;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: An error occured",
                    e.GetType().Name);
                return false;
                throw e;
            }
            return true;
        }

        public bool ChangeFileName(string destFileName)
        {
            Uri newUri = new Uri(this.uri.AbsolutePath + destFileName);
            try
            {
                MoveFile(newUri.AbsoluteUri);
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }
            return true;
        }
    }
}

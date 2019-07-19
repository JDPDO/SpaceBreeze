using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using FluentFTP;
using System.Security;
using IO = System.IO;

namespace JDPDO.SpaceBreeze
{
    class FtpsRemoteManager : IProtocolManager
    {
        private FtpClient client;

        public InstanceType InstanceType => InstanceType.FtpsDirectory;

        /// <summary>
        /// Creates a new instance with given host, user, password and port.
        /// </summary>
        /// <param name="host">The remote host.</param>
        /// <param name="port">The remote connection port.</param>
        /// <param name="user">The remote user.</param>
        /// <param name="password">The remotes user password.</param>
        public FtpsRemoteManager(string host, int port, string user, SecureString password)
        {
            client = new FtpClient(host, port, user, password.ToString());
            try
            {
                client.Connect();
            }
            catch (ObjectDisposedException e)
            {
                ExceptionHandler.LogException(e);
            }
        }

        /// <summary>
        /// Returns the memory stream for the given file.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="callback">NOT IMPLEMENTED YET!</param>
        /// <returns>'MemoryStream' object with file contents.</returns>
        public IO.MemoryStream GetFileStream(File item, AsyncCallback callback = null)
        {
            try
            {
                IO.MemoryStream stream = new IO.MemoryStream();
                bool downloaded = client.Download(stream, item.GetUri().AbsoluteUri);
                return stream;
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return null;
        }

        public File SaveFile(File item, Directory directory)
        {
            IO.MemoryStream remoteStream = GetFileStream(item);
            string localFileUri = IO.Path.Combine(directory.GetUri().AbsolutePath, item.Name);
            IO.FileStream localStream = new IO.FileStream(localFileUri, IO.FileMode.Create, IO.FileAccess.Write);
            IO.StreamReader reader = new IO.StreamReader(remoteStream);
            IO.StreamWriter writer = new IO.StreamWriter(IO.Path.Combine(directory.GetUri().AbsolutePath, item.Name));
            remoteStream.CopyTo(localStream);
            return new File(localFileUri);
        }

        public bool Copy(IFileSystemItem item, Directory directory, bool overwrite)
        {
            FtpExists exists = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
            try
            {
                if (item.IsDirectory())
                {
                    client.CreateDirectory(IO.Path.Combine(directory.GetUri().AbsolutePath, item.Name));
                }
                else
                {
                    IO.MemoryStream stream = GetFileStream(item as File);
                    return client.Upload(stream, directory.GetUri().AbsolutePath, exists);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
        }

        public bool Delete(IFileSystemItem item)
        {
            try
            {
                if (item.IsDirectory())
                {
                    client.DeleteDirectory(item.GetUri().AbsolutePath);
                    return !client.DirectoryExists(item.GetUri().AbsoluteUri);
                }
                else
                {
                    client.DeleteFile(item.GetUri().AbsoluteUri);
                    return !FileExists(item.GetUri().AbsoluteUri);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
        }

        public bool AddItem(IFileSystemItem item, Directory directory, bool overwrite)
        {
            try
            {
                FtpExists exists = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
                if (item.IsDirectory())
                {
                    client.CreateDirectory(IO.Path.Combine(directory.GetUri().AbsolutePath, item.Name));
                    List<IO.FileInfo> fileInfos = new List<IO.FileInfo>();
                    foreach (IFileSystemItem subitem in (item as Directory).EnumerateChildren())
                    {
                        fileInfos.Add(subitem.FileInfo);
                    }
                    int uploaded = client.UploadFiles(
                        fileInfos,
                        directory.GetUri().AbsolutePath,
                        exists,
                        createRemoteDir: true);
                    return uploaded == fileInfos.Count;
                }
                else
                {
                    return AddFile(item as File, directory, overwrite);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
        }

        public bool AddFile(File item, Directory directory, bool overwrite)
        {
            try
            {
                IO.FileStream stream = new IO.FileStream(item.GetUri().AbsoluteUri, IO.FileMode.Open, IO.FileAccess.Read);
                return AddFile(stream, item.Name, directory, overwrite);
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
            
        }

        public bool AddFile(IO.Stream stream, string name, Directory directory, bool overwrite = false)
        {
            FtpExists exists = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
            try
            {
                string remoteUri = IO.Path.Combine(directory.GetUri().AbsolutePath, name);
                return client.Upload(stream, remoteUri, exists, createRemoteDir: true);
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
        }

        public bool Rename(IFileSystemItem item, string newName, bool overwrite = false)
        {
            FtpExists exists = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
            string currentPath = item.GetUri().AbsolutePath;
            try
            {
                if (item.IsDirectory())
                {
                    string upperPath = IO.Path.GetFullPath(IO.Path.Combine(currentPath, ".."));
                    string destUri = IO.Path.GetFullPath(IO.Path.Combine(upperPath, newName));
                    return client.MoveDirectory(currentPath, destUri, exists);
                }
                else
                {
                    string destUri = IO.Path.GetFullPath(IO.Path.Combine(currentPath, newName));
                    return client.MoveFile(item.GetUri().AbsoluteUri, destUri, exists);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
        }

        public bool Move(IFileSystemItem item, Directory destDirectory, bool overwrite = false)
        {
            FtpExists exists = overwrite ? FtpExists.Overwrite : FtpExists.Skip;
            string destPath = destDirectory.GetUri().AbsolutePath;
            try
            {
                if (item.IsDirectory())
                {
                    return client.MoveDirectory(item.GetUri().AbsolutePath, destPath, exists);
                }
                else
                {
                    return client.MoveFile(item.GetUri().AbsoluteUri, destPath, exists);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            return false;
        }

        private bool FileExists(string remotePath) => client.FileExists(remotePath);

        private bool DownloadFile(string localPath, string remotePath, FtpLocalExists localExists = FtpLocalExists.Skip, FtpVerify ftpVerify = FtpVerify.OnlyChecksum)
            => client.DownloadFile(
               localPath,
               remotePath,
               localExists,
               ftpVerify);

        public IEnumerable<IFileSystemItem> EnumerateChildren(Directory directory)
        {
            List<IFileSystemItem> items = new List<IFileSystemItem>();
            FtpListItem[] ftpItems = client.GetListing(directory.GetUri().AbsolutePath);
            foreach (FtpListItem ftpItem in ftpItems)
            {
                items.Add(ConvertFtpListItemToIFileSystemItem(ftpItem));
            }

            return items;
        }

        /// <summary>
        /// Converts FtpListItem type in a IFileSytemItem.
        /// </summary>
        /// <param name="ftpItem">The item being converted.</param>
        /// <returns>Converted item as Directory or File object.</returns>
        protected IFileSystemItem ConvertFtpListItemToIFileSystemItem(FtpListItem ftpItem)
        {
            IFileSystemItem item;
            if (ftpItem.Type == FtpFileSystemObjectType.Directory) item = new ManagedDirectory(ftpItem.FullName, this);
            else item = new File(ftpItem.FullName);

            IO.FileAttributes attributes;
            item.Created = ftpItem.Created;
            item.LastWrite = ftpItem.Modified;
            item.LastAccess = ftpItem.Modified;
            attributes = (ftpItem.Chmod == 4) ? IO.FileAttributes.ReadOnly : 0;
            item.Attributes = attributes;

            return item;
        }

        public bool CreateDirectory(Directory directory)
        {
            string path = directory.GetUri().AbsolutePath;
            client.CreateDirectory(path);
            if (client.DirectoryExists(path)) return true;
            return false;
        }
    }
}

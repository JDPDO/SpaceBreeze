using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;

namespace FileManageAndBackupBot
{
    class FileManageClient
    {
        // private fields
        private FtpDirectory ftpDirectory;
        private LocalDirectory rootDirectory;
        private Queue<File> fileQueue;
        private Queue<LocalDirectory> directoryQueue;


        /// <summary>
        /// File control manager. Managing local and remote files using the libary.
        /// </summary>
        public FileManageClient(string localRoot, string host, int port, string username, string password)
        {
            fileQueue = new Queue<File>();
            ftpDirectory = new FtpDirectory(host, port, username, password);
            rootDirectory = new LocalDirectory(localRoot);
            //ftpDirectory.
        }
    }
}

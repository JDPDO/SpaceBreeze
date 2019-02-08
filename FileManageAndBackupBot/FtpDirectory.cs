using System;
using System.Collections.Generic;
using System.Text;
using FluentFTP;

namespace FileManageAndBackupBot
{
    class FtpDirectory
    {
        //private fields
        private FtpClient client;

        public FtpDirectory(string host, int port, string username, string password)
        {
            client = new FtpClient(host, port, username, password);
            client.ConnectAsync();
        }
    }
}

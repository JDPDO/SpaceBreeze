using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManageAndBackupBot
{
    public interface IFileSystemItem
    {
        void Delete();
        string FullName { get; }
        Uri GetUri();
        bool IsDirectory();
        void Move(string destPath);
    }
}

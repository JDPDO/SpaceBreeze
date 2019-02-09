using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManageAndBackupBot
{
    public interface IFileSystemItem
    {
        bool Exists();
        void Delete();
        string FullName { get; }
        // string Name { get; set; }
        Uri GetUri();
        bool IsDirectory();
        void Move(string destPath);
    }
}

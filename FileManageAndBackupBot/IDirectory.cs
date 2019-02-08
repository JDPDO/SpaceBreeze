using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public interface IDirectory : IFileSystemItem
    {
        string[] GetChildrenNames();
        IO.FileSystemInfo[] GetChildren();
    }
}

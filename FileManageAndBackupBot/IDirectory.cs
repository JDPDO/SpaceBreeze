using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    public interface IDirectory<TDir> : IFileSystemItem
    {
        string[] GetChildrenNames();
        IO.FileSystemInfo[] GetFileSystemInfos();
        TDir[] GetDirectories();
        IO.FileInfo[] GetFiles();
        string Name { get; }
    }
}

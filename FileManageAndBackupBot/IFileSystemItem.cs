using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileManageAndBackupBot
{
    interface IFileSystemItem
    {
        Uri GetUri();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JDPDO.SpaceBreeze
{
    /// <summary>
    /// Specifies the used instance types of the JDPDO.SpaceBreeze assembly.
    /// </summary>
    [Flags]
    public enum InstanceType
    {
        Unknown = 0x00,
        File = 0x01,
        Directory = 0x02,
        LocalDirectory = 0x03,
        FtpsDirectory = 0x04,
        FtpsClient = 0x05,
        BrowserWindow = 0x06,
    }
}

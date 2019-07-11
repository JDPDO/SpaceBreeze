using System;
using System.Collections.Generic;
using System.Text;

namespace JDPDO.Mittuntur
{
    /// <summary>
    /// Specifies the used instance types of the JDPDO.Mittuntur assembly.
    /// </summary>
    enum InstanceType
    {
        Unknown = 0,
        File = 2^0,
        Directory = 2^1,
        LocalDirectory = 2^2,
        FtpsDirectory = 2^3,
        FtpsClient = 2^4,
    }
}

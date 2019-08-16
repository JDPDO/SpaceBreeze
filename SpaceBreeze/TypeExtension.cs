using System;
using System.Collections.Generic;
using System.Text;

namespace JDPDO.SpaceBreeze.Extensions
{
    static class TypeExtension
    {
        public static string GetTypeName(this object instance) => instance.GetType().Name;
    }
}

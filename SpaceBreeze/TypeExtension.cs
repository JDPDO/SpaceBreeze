using System;
using System.Collections.Generic;
using System.Text;

namespace JDPDO.SpaceBreeze.Extensions
{
    public static class TypeExtension
    {
        public static string GetTypeName(this object instance) => instance.GetType().Name;
    }
}

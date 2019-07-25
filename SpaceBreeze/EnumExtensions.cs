using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JDPDO.SpaceBreeze.Extensions
{
    /// <summary>
    /// Provides additional useful methods for enumerations. 
    /// </summary>
    /// <remarks>
    /// By Jeff Mercado on https://stackoverflow.com/questions/4171140/iterate-over-values-in-flags-enum/4171168#4171168.
    /// </remarks>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns flags of a enum value.
        /// </summary>
        /// <param name="value">The value which flags are to provide.</param>
        /// <returns>A collection.</returns>
        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
        }

        /// <summary>
        /// Returns all individual flags of a enum value, so that all values containing multiple bits are left out.
        /// </summary>
        /// <param name="value">The value which individual flags are to provide.</param>
        /// <returns>A collection.</returns>
        public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
        {
            return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
        }

        private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
        {
            ulong bits = Convert.ToUInt64(value);
            List<Enum> results = new List<Enum>();
            // For each element in values Array.
            for (int i = values.Length - 1; i >= 0; i--)
            {
                ulong mask = Convert.ToUInt64(values[i]);
                if (i == 0 && mask == 0L)
                    break;
                if ((bits & mask) == mask)
                {
                    results.Add(values[i]);
                    bits -= mask;
                }
            }
            if (bits != 0L)
                return Enumerable.Empty<Enum>();
            if (Convert.ToUInt64(value) != 0L)
                return results.Reverse<Enum>();
            if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
                return values.Take(1);
            return Enumerable.Empty<Enum>();
        }

        /// <summary>
        /// Returns all flag values of an given enumeration type.
        /// </summary>
        /// <param name="enumType">The enumeration type which flag values should be revealed.</param>
        /// <returns>A collection with all flag values in enumeration type. Empty if there are none or the zero value.</returns>
        private static IEnumerable<Enum> GetFlagValues(Type enumType)
        {
            // Represents the position in the enumeration type.
            ulong flag = 0x1;
            foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                if (bits == 0L)
                    //yield return value;
                    continue; // skip the zero value
                while (flag < bits) flag <<= 1;
                if (flag == bits)
                    yield return value;
            }
        }
    }
}

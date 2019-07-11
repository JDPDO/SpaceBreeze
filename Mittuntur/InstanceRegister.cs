using System;
using System.Collections.Generic;
using System.Text;

namespace JDPDO.Mittuntur
{
    class InstanceRegister
    {
        /// <summary>
        /// Containing register for various types that are holding instances of that classes. 
        /// </summary>
        private Dictionary<InstanceType, Dictionary<string, object>> registers;

        private InstanceRegister(IEnumerable<KeyValuePair<InstanceType, Dictionary<string, object>>> collection)
        {
            registers = new Dictionary<InstanceType, Dictionary<string, object>>(collection);
            
        }

        public InstanceRegister() : this(null) { }

        /// <summary>
        /// Registers an instance.
        /// </summary>
        /// <param name="type">The instance type of the object to be registered.</param>
        /// <param name="id">A id for the instance to be registered.</param>
        /// <param name="instance">The instance to be registered.</param>
        public void RegisterInstance(InstanceType type, string id, object instance)
        {
            if (instance != null)
            {
                try
                {
                    if (!registers.ContainsKey(type))
                    {
                        registers.Add(type, new Dictionary<string, object>());
                    }
                    registers[type].Add(id, instance);
                }
                catch (Exception e)
                {
                    ExeptionHandler.NewException(e);
                }
            }
            else ExeptionHandler.NewArgumentNullExeption(nameof(instance), "The Object have to be initialized.");
        }

        /// <summary>
        /// Provides an instance of the register.
        /// </summary>
        /// <param name="type">The instance type of the object to be provided.</param>
        /// <param name="id">The id of the instance to be provided.</param>
        /// <returns>The instance to given id.</returns>
        internal object ProvideInstance(InstanceType type, string id)
        {
            if (type != InstanceType.Unknown && id != null)
            {
                return registers[type][id];
            }
            // If an argument equals null.
            ExeptionHandler.NewException(new ArgumentNullException(nameof(id), "The id value have to be defined."));
            return null;
        }
    }
}

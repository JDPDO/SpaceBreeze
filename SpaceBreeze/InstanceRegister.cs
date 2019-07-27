using System;
using System.Collections.Generic;
using System.Text;
using JDPDO.SpaceBreeze.Extensions;

namespace JDPDO.SpaceBreeze
{
    public class InstanceRegister
    {
        #region staticVariablesAndMethods

        /// <summary>
        /// Stores the first created InstanceRegister object.
        /// </summary>
        private static InstanceRegister firstInstanceRegister;

        /// <summary>
        /// Returns the first created 'InstanceRegister' object or creates one, if none was declared before.
        /// </summary>
        /// <returns>First created InstanceRegister object in runtime.</returns>
        public static InstanceRegister GetFirstInstanceRegister()
        {
            if (firstInstanceRegister == null) firstInstanceRegister = new InstanceRegister();
            return firstInstanceRegister;
        }

        #endregion

        /// <summary>
        /// Containing register for various types that are holding instances of that classes. 
        /// </summary>
        private Dictionary<InstanceType, Dictionary<string, object>> registers;

        public InstanceRegister(IEnumerable<KeyValuePair<InstanceType, Dictionary<string, object>>> collection)
        {
            // Init register with collection if given or not if not given.
            registers = (collection != null) ? new Dictionary<InstanceType, Dictionary<string, object>>(collection) : new Dictionary<InstanceType, Dictionary<string, object>>();

            // Declare first InstanceRegister to Type if none declared before.
            if (firstInstanceRegister == null) firstInstanceRegister = this;
        }

        public InstanceRegister() : this(null) { }

        /// <summary>
        /// Returns all subregisters for given types ('InstanceType').
        /// </summary>
        /// <param name="type">To returning registers of types or type.</param>
        /// <returns>NUll if none subregister was found.</returns>
        public Dictionary<string, object> GetRegister(InstanceType type)
        {
            // Check if key exists in registers.
            if (registers.ContainsKey(type))
            {
                List<Enum> individualFlags = type.GetIndividualFlags() as List<Enum>;
                Enum[] enums = individualFlags.Count > 0 ? individualFlags.ToArray() : null;
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                if (enums == null)
                {
                    foreach (Enum subEnum in enums)
                    {
                        InstanceType subtype = (InstanceType)subEnum;
                        pairs.Add(subtype.ToString(), registers[subtype]);
                    }
                }
                return pairs;
            }
            return null;
        }

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
                    ExceptionHandler.LogException(e);
                }
            }
            else ExceptionHandler.NewArgumentNullException(nameof(instance), "The Object have to be initialized.");
        }

        /// <summary>
        /// Provides an instance of the register.
        /// </summary>
        /// <param name="type">The instance type of the object to be provided.</param>
        /// <param name="id">The id of the instance to be provided.</param>
        /// <returns>The instance to given id.</returns>
        public object ProvideInstance(InstanceType type, string id)
        {
            if (type != InstanceType.Unknown && String.IsNullOrEmpty(id))
            {
                return registers[type][id];
            }
            // If an argument equals null.
            ExceptionHandler.NewArgumentNullException(nameof(id), "The id value have to be defined.");
            return null;
        }
    }
}

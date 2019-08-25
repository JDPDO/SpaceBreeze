using System;
using System.Collections.Generic;
using System.Text;

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
        private Dictionary<string, Dictionary<string, object>> registers;

        public InstanceRegister(IEnumerable<KeyValuePair<string, Dictionary<string, object>>> collection)
        {
            // Init register with collection if given or not if not given.
            registers = (collection != null)
                ? new Dictionary<string, Dictionary<string, object>>(collection) 
                : new Dictionary<string, Dictionary<string, object>>();

            // Declare first InstanceRegister to Type if none declared before.
            if (firstInstanceRegister == null) firstInstanceRegister = this;
        }

        public InstanceRegister() : this(null) { }

        /// <summary>
        /// Returns all subregisters for given types.
        /// </summary>
        /// <param name="type">To returning registers of types or type.</param>
        /// <returns>NUll if none subregister was found.</returns>
        public Dictionary<string, object> GetSubRegister(string type)
        {
            return registers.ContainsKey(type) ? registers[type] : new Dictionary<string, object>();
        }

        /// <summary>
        /// Registers an instance.
        /// </summary>
        /// <param name="type">The instance type of the object to be registered.</param>
        /// <param name="id">A id for the instance to be registered.</param>
        /// <param name="instance">The instance to be registered.</param>
        public void RegisterInstance(string type, string id, object instance)
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
        public object ProvideInstance(string type, string id)
        {
            try
            {
                if (!(String.IsNullOrEmpty(type) && String.IsNullOrEmpty(id)))
                {
                    return registers[type][id];
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.LogException(e);
            }
            // If an argument equals null.
            ExceptionHandler.NewArgumentNullException(nameof(id), "The id value have to be defined.");
            return null;
        }

        /// <summary>
        /// Removes an instance of the register.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns>True if removing succeed, else false.</returns>
        public bool RemoveInstance(string type, string id)
        {
            try { return registers[type].Remove(id); }
            catch (Exception e) { ExceptionHandler.LogException(e); }
            return false;
        }

        /// <summary>
        /// Checks if instance exists.
        /// </summary>
        /// <param name="type">Type of checked instance.</param>
        /// <param name="id">Identifier of instance.</param>
        /// <returns></returns>
        public bool InstanceExists(string type, string id)
        {
            try { return registers[type].ContainsKey(id); }
            catch (Exception e) { ExceptionHandler.LogException(e); }
            return false;
        }
    }
}

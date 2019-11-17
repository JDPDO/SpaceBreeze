using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Initialize new instance register with predefined collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">Dictionary contains one or more duplicate keys.</exception>
        /// <param name="collection">A predefined collection.</param>
        public InstanceRegister(IEnumerable<KeyValuePair<string, Dictionary<string, object>>> dictionary)
        {
            // Init register with collection if given or not if not given.
            registers = (dictionary != null)
                ? new Dictionary<string, Dictionary<string, object>>(dictionary) 
                : new Dictionary<string, Dictionary<string, object>>();

            // Declare first InstanceRegister to Type if none declared before.
            if (firstInstanceRegister == null) firstInstanceRegister = this;
        }

        /// <summary>
        /// Creates a new empty instance register.
        /// </summary>
        public InstanceRegister() : this(null) { }

        /// <summary>
        /// Returns all subregisters for given types.
        /// </summary>
        /// <param name="type">To returning registers of types or type.</param>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and type does not exist in the subregister.</exception>
        /// <returns>Empty subregister if none matching type was found.</returns>
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
        /// <exception cref="ArgumentException">Id is null.</exception>
        /// <exception cref="ArgumentNullException">Instance and/or type is null.</exception>
        public void RegisterInstance(string type, string id, object instance)
        {

            if (instance != null)
            {
                if (!registers.ContainsKey(type))
                {
                    registers.Add(type, new Dictionary<string, object>());
                }
                registers[type].Add(id, instance);
            }
            else throw new ArgumentNullException(nameof(instance), "The Object have to be initialized.");
        }

        /// <summary>
        /// Provides an instance of the register.
        /// </summary>
        /// <param name="type">The instance type of the object to be provided.</param>
        /// <param name="id">The id of the instance to be provided.</param>
        /// <exception cref="ArgumentNullException">One or multiple arguments are empty or null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved. Type and/or id does not exist in the subregister.</exception>
        /// <returns>The instance to given id.</returns>
        public object ProvideInstance(string type, string id) => registers[type][id];

        /// <summary>
        /// Removes an instance of the register.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException">Type and/or id is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and type does not exist in the collection.</exception>
        public void RemoveInstance(string type, string id) => registers[type].Remove(id);

        /// <summary>
        /// Checks if instance exists.
        /// </summary>
        /// <param name="type">Type of checked instance.</param>
        /// <param name="id">Identifier of instance.</param>
        /// <exception cref="ArgumentNullException">Type and/or id is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and type does not exist in the collection.</exception>
        /// <returns>True if instance of object is avaiable. False if not.</returns>
        public bool InstanceExists(string type, string id) => registers[type].ContainsKey(id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    /// <summary>
    /// Class of the object Container
    /// </summary>
    class Container
    {
        // MM attributes
        private int containerId;
        private string containerType;
        private string containerAvailability;

        /// <summary>
        ///  MM Container constructor for new item (No id, auto increment)
        /// </summary>
        /// <param name="containerType"></param>
        /// <param name="containerAvailability"></param>
        public Container(string containerType, string containerAvailability)
        {
            this.containerType = containerType;
            this.containerAvailability = containerAvailability;
        }
        /// <summary>
        ///  MM Container constructor for update (with id)
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="containerType"></param>
        /// <param name="containerAvailability"></param>
        public Container(int containerId, string containerType, string containerAvailability)
        {
            this.containerId = containerId;
            this.containerType = containerType;
            this.containerAvailability = containerAvailability;
        }
        // MM properties
        public int ContainerId
        {
            get { return containerId; }
            set { containerId = value; }
        }

        public string ContainerType
        {
            get { return containerType; }
            set { containerType = value; }
        }

        public string ContainerAvailability
        {
            get { return containerAvailability; }
            set { containerAvailability = value; }
        }
    }
}

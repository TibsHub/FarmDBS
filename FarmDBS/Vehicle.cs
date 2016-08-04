using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    /// <summary>
    /// Class of the object Vehicle
    /// </summary>
    class Vehicle
    {
        // MM attributes
        private string regNumber;
        private string vehicleType;
        private string vehicleAvailability;

        /// <summary>
        ///  MM Vehicle constructor
        /// </summary>
        /// <param name="regNumber"></param>
        /// <param name="vehicleType"></param>
        /// <param name="vehicleAvailability"></param>
        public Vehicle(string regNumber, string vehicleType, string vehicleAvailability)
        {
            this.regNumber = regNumber;
            this.vehicleType = vehicleType;
            this.vehicleAvailability = vehicleAvailability;
        }

        // MM properties
        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
        }

        public string VehicleType
        {
            get { return vehicleType; }
            set { vehicleType = value; }
        }

        public string VehicleAvailability
        {
            get { return vehicleAvailability; }
            set { vehicleAvailability = value; }
        }

    }
}

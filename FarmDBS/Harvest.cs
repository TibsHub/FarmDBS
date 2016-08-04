using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    // Class of object Harvest
    class Harvest
    {
        // MM attributes
        private string harvestStartDate;
        private string harvestEndDate;
        private string harvestMethod;
        private string harvestCrop;
        private string harvestVehicle;
        private int harvestEmpoyeeID;
        private int harvestContainer;

        /// <summary>
        ///  MM Harvest constructor
        /// </summary>
        /// <param name="harvestStartDate"></param>
        /// <param name="harvestEndDate"></param>
        /// <param name="harvestMethod"></param>
        /// <param name="harvestCrop"></param>
        /// <param name="harvestVehicle"></param>
        /// <param name="harvestEmpoyeeID"></param>
        /// <param name="harvestContainer"></param>
        public Harvest(string harvestStartDate, string harvestEndDate, string harvestMethod, string harvestCrop, string harvestVehicle, int harvestEmpoyeeID, int harvestContainer)
        {
            this.harvestStartDate = harvestStartDate;
            this.harvestEndDate = harvestEndDate;
            this.harvestMethod = harvestMethod;
            this.harvestCrop = harvestCrop;
            this.harvestVehicle = harvestVehicle;
            this.harvestEmpoyeeID = harvestEmpoyeeID;
            this.harvestContainer = harvestContainer;
        }

        // MM properties
        public string HarvestStartDate
        {
            get { return harvestStartDate; }
            set { harvestStartDate = value; }
        }

        public string HarvestEndDate
        {
            get { return harvestEndDate; }
            set { harvestEndDate = value; }
        }

        public string HarvestMethod
        {
            get { return harvestMethod; }
            set { harvestMethod = value; }
        }

        public string HarvestCrop
        {
            get { return harvestCrop; }
            set { harvestCrop = value; }
        }

        public string HarvestVehicle
        {
            get { return harvestVehicle; }
            set { harvestVehicle = value; }
        }

        public int HarvestEmployeeID
        {
            get { return harvestEmpoyeeID; }
            set { harvestEmpoyeeID = value; }
        }

        public int HarvestContainer
        {
            get { return harvestContainer; }
            set { harvestContainer = value; }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Fertiliser : Treatment
    {
        // Private attributes of this class
        private string fertiliserType;
        private int fertiliserStockLevel = 0;

        // Public propeties of this class
        public string FertiliserType
        {
            get { return fertiliserType; }
            set { fertiliserType = value; }
        }

        public int FertiliserStockLevel
        {
            get { return fertiliserStockLevel; }
            set { fertiliserStockLevel = value; }
        }

        /// <summary>
        /// Constructor for the Fertiliser class
        /// </summary>
        /// <param name="fertiliserType"></param>
        /// <param name="fertiliserStockLevel"></param>
        public Fertiliser(string fertiliserType, int fertiliserStockLevel)
        {
            this.FertiliserType = fertiliserType;
            this.FertiliserStockLevel = fertiliserStockLevel;
            this.TreatmentStartDate = TreatmentStartDate;
            this.TreatmentEndDate = TreatmentEndDate;
        }

        // Methods
        /// <summary>
        /// Displaying the stock level of a certain fertaliser
        /// </summary>
        /// <param name="fertiliserType">The name of the fertiliser you want information of</param>
        public int displayFertiliserInStock(string fertiliserType)
        {
            return FertiliserStockLevel;
        }
    }
}

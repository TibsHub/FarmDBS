using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Treatment
    {
        // Private attributes of this class
        private string treatmentStartDate;
        private string treatmentEndDate;
        private string treatmentCropName;
        private int treatmentEmployeeID;
        private string treatmentFertiliserType;

        // Public properties of this class
        public string TreatmentStartDate
        {
            get { return treatmentStartDate; }
            set { treatmentStartDate = value; }
        }

        public string TreatmentCropName
        {
            get { return treatmentCropName; }
            set { treatmentCropName = value; }
        }

        public int TreatmentEmployeeID
        {
            get { return treatmentEmployeeID; }
            set { treatmentEmployeeID = value; }
        }

        public string TreatmentFertiliserType
        {
            get { return treatmentFertiliserType; }
            set { treatmentFertiliserType = value; }
        }
    
        public string TreatmentEndDate
        {
            get { return treatmentEndDate; }
            set { treatmentEndDate = value; }
        }

        ///*
        /// <summary>
        /// Default Constructor for Treatment class (no attributes)
        /// </summary>
        public Treatment()
        { }
        //*/


        /// <summary>
        /// Constructor for the Treatment class
        /// </summary>
        /// <param name="treatmentStartDate">Start date/time of the treatment</param>
        /// <param name="treatmentEndDate">Ending date/time of the treatment</param>
        public Treatment(string treatmentStartDate, string treatmentEndDate, string treatmentCropName, int treatmentEmployeeID, string treatmentFertiliserType)
        {
            this.treatmentStartDate = treatmentStartDate;
            this.treatmentEndDate = treatmentEndDate;
            this.treatmentCropName = treatmentCropName;
            this.treatmentEmployeeID = treatmentEmployeeID;
            this.TreatmentFertiliserType = treatmentFertiliserType;
        }


        // Methods
        public void fertiliserAssignedToEmployee()
        { }

        public void getFertiliserAmount()
        { }
    }
}

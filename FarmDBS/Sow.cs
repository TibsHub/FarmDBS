using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Sow
    {
        // Private attributes of this class
        private DateTime sowTime;
        private string sowMethod;
        private float sowLabourHours;

        // Public properties of this class
        public DateTime SowTime
        {
            get { return sowTime; }
            set { sowTime = value; }
        }

        public string SowMethod
        {
            get { return sowMethod; }
            set { sowMethod = value; }
        }

        public float SowLabourHours
        {
            get { return sowLabourHours; }
            set { sowLabourHours = value; }
        }

        /// <summary>
        /// Constructor for the Sow class
        /// </summary>
        /// <param name="sowTime">Time of sowing</param>
        /// <param name="sowMethod">Method of sowing</param>
        /// <param name="sowLabourHours">Labour hours needed for sowing</param>
 
        public Sow(DateTime sowTime, string sowMethod, float sowLabourHours)
        {
            this.sowTime = sowTime;
            this.sowMethod = sowMethod;
            this.sowLabourHours = sowLabourHours;
        }

        /// <summary>
        /// Show labour of a certain crop
        /// </summary>
        /// <param name="cropType">The name of the crop of which information is wanted</param>
        /// <returns></returns>
        public float displaySowLabour(string cropType)
        {
            return SowLabourHours;
        }
    }
}

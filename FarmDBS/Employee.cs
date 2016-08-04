using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace FarmDBS
{
    class Employee : Person
    {
        // RB Attributes
        private int privilegeLevel = 0;
        private string jobRole = "";
        private double hourlyRate = 0;

        // RB Properties
        public int PrivilegeLevel
        {
            get { return privilegeLevel; }
            set { privilegeLevel = value; }
        }

        public string JobRole
        {
            get { return jobRole; }
            set { jobRole = value; }
        }

        public double HourlyRate
        {
            get { return hourlyRate; }
            set { hourlyRate = value; }
        }

        /// <summary>
        /// Employee Default Constructor
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="address"></param>
        /// <param name="privilegeLevel"></param>
        /// <param name="jobRole"></param>
        /// <param name="hourlyRate"></param>
        /// <param name="hoursPerCrop"></param>
        public Employee(int personID, string firstName, string lastName, string address, int priviledgeLevel, string jobRole, double hourlyRate)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.PrivilegeLevel = priviledgeLevel;
            this.JobRole = jobRole;
            this.HourlyRate = hourlyRate;
        }
        
    }
}

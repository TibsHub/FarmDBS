using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Person
    {
        //attributes
        private int personID;
        private string firstName = "";
        private string lastName = "";
        private string address = "";

        //properties
        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

    }
}

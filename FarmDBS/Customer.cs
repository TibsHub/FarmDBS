using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Customer : Person
    {
        // RB Constructor
        public Customer(int personID, string firstName, string lastName, string address)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
        }
    }
}

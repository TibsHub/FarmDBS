using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class Stock
    {
        //attributes

        private int amountInStock;

        // constructor

        public Stock(int amountInStock)
        {
            this.amountInStock = amountInStock;
        }

        // properties

        public int AmountInStock
        {
            get { return amountInStock; }
            set { amountInStock = value; }
        }

        public void getStockLevel()
        {
        }

        public void updateStockLevel()
        {
        }

    }
}

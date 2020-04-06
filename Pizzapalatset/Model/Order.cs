using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pizzapalatset.Model
{
    class Order : INotifyPropertyChanged
    {
        private int totalCost = 0;
        private int orderid = 0;
        private string display = "";
        private string displaytot = "";

        public int TotalCost {
            get { return totalCost; }
            set { totalCost = value;
                  NotifyPropertyChanged("TotalCost"); 
                }
        }
        public int OrderID {
            get { return orderid; }
            set { orderid = value;
                NotifyPropertyChanged("OrderID");
            }
        }

        public string DisplayOrders
        {
            get { return $"Ordernummer: {OrderID.ToString()}"; }
            set
            {
                display = value;
                NotifyPropertyChanged("DisplayOrders");
            }
        }

        public string DisplayTotalCost
        {
            get { return $"Totalkostnad: {TotalCost.ToString()} kr"; }
            set
            {
                displaytot = value;
                NotifyPropertyChanged("DisplayTotalCost");
            }
        }

        public Order(int orderid, int totalcost)
        {
            OrderID = orderid;
            TotalCost = totalcost;
        }
        public Order()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}

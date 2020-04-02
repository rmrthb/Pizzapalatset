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
        public int OrderID { get; set; }
        private int totalCost = 0;
        public int TotalCost {
            get { return totalCost; }
            set { totalCost = value; this.NotifyPropertyChanged("TotalCost"); }
        }

        
        public Order()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

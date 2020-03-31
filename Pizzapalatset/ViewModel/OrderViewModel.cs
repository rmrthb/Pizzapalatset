using Pizzapalatset.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Pizzapalatset.ViewModel
{
    class OrderViewModel
    {
        public ObservableCollection<Pizza> OrderList = new ObservableCollection<Pizza>();


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        //public int TotalPrice {
        //    get { return totalprice; }
        //    set { totalprice = value;
        //        NotifyPropertyChanged("TotalPrice");
        //    }
        //}

        public OrderViewModel()
        {

        }

        public void AddToCart(Pizza pizza)
        {
            OrderList.Add(pizza);
        }
        public async void CancelOrder()
        {
            var cancel = new MessageDialog("Är du säker på att du vill avbryta din order?", "Avbryt order");

            cancel.Commands.Clear();
            cancel.Commands.Add(new UICommand { Label = "Ja", Id = 0 });
            cancel.Commands.Add(new UICommand { Label = "Avbryt", Id = 1 });

            var result = await cancel.ShowAsync();

            if ((int)result.Id == 0)
            {
                MessageDialog remove = new MessageDialog("Din order är avbruten.");
                await remove.ShowAsync();
                OrderList.Clear();
            }
        }
    }
}

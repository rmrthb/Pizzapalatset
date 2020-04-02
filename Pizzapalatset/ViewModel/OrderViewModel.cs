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


        public Order MyOrder { get; set; }

        public OrderViewModel()
        {

        }

        public void AddToCart(Pizza pizza)
        {
            if (pizza != null)
            {
                OrderList.Add(pizza);
                CalculateTotalCost(pizza);
            }
        }

        public void RemoveFromCart(Pizza pizza)
        {
            if (pizza != null)
            {
                OrderList.Remove(pizza);
            }
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
        public void CalculateTotalCost(Pizza p)
        {
            MyOrder.TotalCost += p.PizzaPrice;
        }
    }
}

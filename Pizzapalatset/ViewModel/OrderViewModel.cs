using Newtonsoft.Json;
using Pizzapalatset.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Pizzapalatset.ViewModel
{
    class OrderViewModel
    {
        public ObservableCollection<Pizza> OrderList = new ObservableCollection<Pizza>();
        public ObservableCollection<Order> MyCart = new ObservableCollection<Order>();

        HttpClient httpClient;

        string orderUrl = "https://localhost:44360/api/orders/";
        string pOrderUrl = "https://localhost:44360/api/pizzaorders/";
        public Order MyOrder { get; set; }

        public OrderViewModel()
        {
            MyOrder = new Order();
            httpClient = new HttpClient();
        }

        public void AddToCart(Pizza pizza)
        {
            if (pizza != null)
            {
                OrderList.Add(pizza);
                CalculateTotalCost(pizza, true);
            }
        }
        public void RemoveFromCart(Pizza pizza)
        {
            if (pizza != null)
            {
                OrderList.Remove(pizza);
                CalculateTotalCost(pizza, false);
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
                MyOrder.TotalCost = 0;
            }
        }

        public async Task ConfirmOrder(Order o)
        {
            var ready = new MessageDialog("Är du redo att skicka din beställning till köket?", "Bekräfta order");

            ready.Commands.Clear();
            ready.Commands.Add(new UICommand { Label = "Ja", Id = 0 });
            ready.Commands.Add(new UICommand { Label = "Avbryt", Id = 1 });

            var result = await ready.ShowAsync();

            if ((int)result.Id == 0 && OrderList.Count > 0)
            {
                MessageDialog confirm = new MessageDialog($"Din order har skickats till köket. Smaklig måltid!");
                await confirm.ShowAsync();
                await CreateOrderinDB(o);
                OrderList.Clear();
                MyOrder.TotalCost = 0;
            }
            else
            {
                MessageDialog empty = new MessageDialog($"Din order kan inte vara tom! Lägg till pizzor i ordern och försök igen.");
                await empty.ShowAsync();
            }
        }
        public int CalculateTotalCost(Pizza p, bool tf)
        {
            if (tf)
            {
                MyOrder.TotalCost += p.PizzaPrice;
            }
            else
            {
                MyOrder.TotalCost -= p.PizzaPrice;
            }
            return MyOrder.TotalCost;
        }

        /// <summary>
        /// Creates and posts an order to the order table + post to the junction table. 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        /// 
        public async Task CreateOrderinDB(Order o)
        {
            var myOrder = JsonConvert.SerializeObject(o);
            HttpContent httpContent = new StringContent(myOrder);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var orderDB = await httpClient.PostAsync(orderUrl, httpContent);
            var readDB = await orderDB.Content.ReadAsStringAsync();

            int orderNo = int.Parse(readDB);

            if (OrderList.Count > 0)
            {
                foreach (var item in OrderList)
                {
                    var pOrder = new PizzaOrder() { OrderID = orderNo, PizzaID = item.PizzaID };
                    myOrder = JsonConvert.SerializeObject(pOrder);
                    httpContent = new StringContent(myOrder);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    await httpClient.PostAsync(pOrderUrl, httpContent);
                }
            }
            OrderList.Clear();

            MessageDialog msg = new MessageDialog($"Du har fått ordernummer: {orderNo}");
            await msg.ShowAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var httpClient = new HttpClient();

            var result = await httpClient.DeleteAsync(orderUrl + id);

            MessageDialog msg = new MessageDialog($"Order med ordernummer: {id} har ångrats, och köket har meddelats.");

            await msg.ShowAsync();
            /*TEST*/
        }
    }
}
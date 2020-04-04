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
        
        public async Task CreateOrderinDB(Order o)
        {
            var orderId = JsonConvert.SerializeObject(o);
            HttpContent httpContent = new StringContent(orderId);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var order = await httpClient.PostAsync(orderUrl, httpContent);
            var temp = await order.Content.ReadAsStringAsync();
            int maxId = int.Parse(temp);

            if (OrderList != null)
            {
                foreach (var item in OrderList)
                {
                    var test = new PizzaOrder() { OrderID = maxId, PizzaID = item.PizzaID };
                    orderId = JsonConvert.SerializeObject(test);
                    httpContent = new StringContent(orderId);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    await httpClient.PostAsync(pOrderUrl, httpContent);
                }

                MessageDialog msg = new MessageDialog($"Order {o.OrderID} has been sent to the kitchen!");
                await msg.ShowAsync();
                OrderList.Clear();
            }



            //HttpResponseMessage response = await httpClient.PostAsync(orderUrl, httpContent);

            //var read = await response.Content.ReadAsStringAsync();

            //var deserialised = JsonConvert.DeserializeObject<int>(read);

            //MyOrder.OrderID = deserialised;

        }

        //public async Task PostOrder(Order o)
        //{
            //var orderId = JsonConvert.SerializeObject(o);

            //HttpContent httpContent = new StringContent(orderId);

            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //var order = await httpClient.PostAsync(pOrderUrl, httpContent);
            //var temp = await order.Content.ReadAsStringAsync();

            //int maxId = int.Parse(temp);

            //foreach (var item in OrderList)
            //{
            //    var test = new PizzaOrder() { OrderID = maxId, PizzaID = item.PizzaID };
            //    orderId = JsonConvert.SerializeObject(test);
            //    httpContent = new StringContent(orderId);
            //    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    await httpClient.PostAsync(pOrderUrl, httpContent);
            //}
        //}
        
        //Metod för att beställa allt i kundvagnen

    }
}

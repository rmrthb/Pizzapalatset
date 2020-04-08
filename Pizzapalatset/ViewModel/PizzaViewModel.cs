using Newtonsoft.Json;
using Pizzapalatset.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Pizzapalatset.ViewModel
{
    class PizzaViewModel
    {
        #region
        public ObservableCollection<Pizza> PizzaList = new ObservableCollection<Pizza>();

        public ObservableCollection<Pizza> httpPizzaList = new ObservableCollection<Pizza>();

        HttpClient httpClient;

        string url = "https://localhost:44360/api/pizzas/";
        #endregion
        public PizzaViewModel()
        {
            //InitPizzaList();
            httpPizzaList = new ObservableCollection<Pizza>();
            httpClient = new HttpClient();
        }
        /// <summary>
        /// Method that retrieves all the pizzas in the server database.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Pizza>> GetProductsAsync()
        {
            //steg 1
            var jsonProducts = await httpClient.GetStringAsync(url);
            //steg 2
            var pizzas = JsonConvert.DeserializeObject<ObservableCollection<Pizza>>(jsonProducts);
            //steg 3
            return pizzas;
        }

        /// <summary>
        /// In case the admin wants to add a pizza.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task AddProductAsync(string pizzaname, string pprice)
        {

            int.TryParse(pprice, out int pizzaprice);

            Pizza p = new Pizza(pizzaname, pizzaprice);
            //steg 1: serializera object till sträng
            var pizza = JsonConvert.SerializeObject(p);

            //steg 2: Berätta att strängen är ett httpcontent (meddelande som ska skickas iväg)
            HttpContent httpContent = new StringContent(pizza);

            //steg 3: berätta om format som är json
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //steg 4: Posta
            await httpClient.PostAsync(url, httpContent);
        }

        public async Task UpdateProductAsync(Pizza p, string newp)
        {
            int.TryParse(newp, out int newprice);
            p.PizzaPrice = newprice;
            var updatedPizza = JsonConvert.SerializeObject(p);
            HttpContent httpContent = new StringContent(updatedPizza);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await httpClient.PutAsync(url + p.PizzaID, httpContent);
        }

        public async Task RemoveProductAsync(Pizza p)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.DeleteAsync(url + p.PizzaID);
            MessageDialog msg = new MessageDialog($"'{p.PizzaName}' har tagits bort från sortimentet.");
            await msg.ShowAsync();
        }
        /// <summary>
        /// Method that generates a set of pizzas, if I want to test the application offline.
        /// </summary>
        ///
        public void InitPizzaList()
        {
            PizzaList.Add(new Pizza(0, "Margarita", 59));
            PizzaList.Add(new Pizza(1, "Al Funghi", 59));
            PizzaList.Add(new Pizza(2, "Vesuvio", 59));
            PizzaList.Add(new Pizza(3, "Kebabpizza", 59));
            PizzaList.Add(new Pizza(4, "Gudfader", 59));
        }
    }
}

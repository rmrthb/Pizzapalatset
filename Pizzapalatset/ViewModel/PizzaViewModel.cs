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

namespace Pizzapalatset.ViewModel
{
    class PizzaViewModel
    {
        public ObservableCollection<Pizza> PizzaList = new ObservableCollection<Pizza>();

        public ObservableCollection<Pizza> httpPizzaList = new ObservableCollection<Pizza>();

        HttpClient httpClient;

        string url = "https://localhost:44360/api/pizzas/";

        public PizzaViewModel()
        {
            //InitPizzaList();
            httpPizzaList = new ObservableCollection<Pizza>();
            httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Pizza>> GetProductsAsync()
        {
            //steg 1
            var jsonProducts = await httpClient.GetStringAsync(url);
            //steg 2
            var pizzas = JsonConvert.DeserializeObject<ObservableCollection<Pizza>>(jsonProducts);
            //steg 3
            return pizzas;
        }

        public async Task AddProductAsync(Pizza p)
        {
            //steg 1: serializera object till sträng
            var product = JsonConvert.SerializeObject(p);

            //steg 2: Berätta att strängen är ett httpcontent (meddelande som ska skickas iväg)
            HttpContent httpContent = new StringContent(product);

            //steg 3: berätta om format som är json
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //steg 4: Posta
            await httpClient.PostAsync(url, httpContent);
        }

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

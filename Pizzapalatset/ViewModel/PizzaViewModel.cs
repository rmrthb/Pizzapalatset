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
            var jsonProducts = await httpClient.GetStringAsync(url);
            var pizzas = JsonConvert.DeserializeObject<ObservableCollection<Pizza>>(jsonProducts);
            return pizzas;
        }

        /// <summary>
        /// Method that adds a pizza to the database using a POST request.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task AddProductAsync(string pName, string pPrice)
        {
            if (!string.IsNullOrEmpty(pName) && !string.IsNullOrWhiteSpace(pPrice)) 
            {
                int.TryParse(pPrice, out int pizzaprice);

                Pizza p = new Pizza(pName, pizzaprice);
                var pizza = JsonConvert.SerializeObject(p);
                HttpContent httpContent = new StringContent(pizza);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                await httpClient.PostAsync(url, httpContent);

                MessageDialog msg = new MessageDialog($"'{pName}' med pris {pPrice} har lagts till i menyn.");
                await msg.ShowAsync();
            }
            else
            {
                MessageDialog msg = new MessageDialog($"Se till att båda fälten är ifyllda!");
                await msg.ShowAsync();
            }
        }
        
        /// <summary>
        /// Method that updates the price of a pizza in the database using a PUT request.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="newPizza"></param>
        /// <returns></returns>
        public async Task UpdateProductAsync(Pizza p, string newPizzaPrice)
        {
            if (!string.IsNullOrWhiteSpace(newPizzaPrice) && p != null)
            {
                if (int.TryParse(newPizzaPrice, out int newPrice))
                {
                    p.PizzaPrice = newPrice;
                    var updatedPizza = JsonConvert.SerializeObject(p);
                    HttpContent httpContent = new StringContent(updatedPizza);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    await httpClient.PutAsync(url + p.PizzaID, httpContent);

                    MessageDialog update = new MessageDialog($"'{p.PizzaName}' har uppdaterats med det nya priset {newPrice}.");
                    await update.ShowAsync();
                }
                else
                {
                    MessageDialog msg = new MessageDialog($"Endast siffror tillåtna!");
                    await msg.ShowAsync();
                }
            }
            else
            {
                MessageDialog valid = new MessageDialog($"Ett fel har uppstått. " +
                    $"Välj pizzan du vill byta pris på eller fyll i ett giltigt värde.");
                await valid.ShowAsync();
            }

        }
        
        /// <summary>
        /// Method that deletes a pizza in the database using a DELETE request.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task RemoveProductAsync(Pizza p)
        {
            if (p != null)
            {
                var remove = new MessageDialog($"Är du säker på att du vill ta bort {p.PizzaName}?");
                remove.Commands.Clear();
                remove.Commands.Add(new UICommand { Label = "Ja", Id = 0 });
                remove.Commands.Add(new UICommand { Label = "Avbryt", Id = 1 });

                var answer = await remove.ShowAsync();

                if ((int)answer.Id == 0)
                {
                    var httpClient = new HttpClient();
                    var result = await httpClient.DeleteAsync(url + p.PizzaID);
                    MessageDialog msg = new MessageDialog($"'{p.PizzaName}' har tagits bort från menyn.");
                    await msg.ShowAsync();
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog($"Välj pizzan du vill ta bort i listan!");
                await msg.ShowAsync();
            }
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

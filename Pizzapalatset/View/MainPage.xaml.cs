using Pizzapalatset.ViewModel;
using Pizzapalatset.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pizzapalatset
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        PizzaViewModel pizzaViewModel;
        OrderViewModel orderViewModel;
        public MainPage()
        {
            this.InitializeComponent();
            pizzaViewModel = new PizzaViewModel();
            orderViewModel = new OrderViewModel();
            PizzaListView.ItemsSource = pizzaViewModel.httpPizzaList;
            GetPizzas();
        }

        private async void GetPizzas()
        {
            var pizzas = await pizzaViewModel.GetProductsAsync();

            foreach(Pizza p in pizzas)
            {
                pizzaViewModel.httpPizzaList.Add(p);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            //orderViewModel.AddToCart((Pizza)PizzaListView.SelectedItem);

        }
        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            orderViewModel.CancelOrder();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            orderViewModel.RemoveFromCart((Pizza)OrderListView.SelectedItem);
        }

        private void PizzaListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orderViewModel.AddToCart((Pizza)PizzaListView.SelectedItem);
            PizzaListView.SelectedItem = null;
        }

        private async void ToPayment_Click(object sender, RoutedEventArgs e)
        {
            Order p = orderViewModel.MyOrder;
            await orderViewModel.CreateOrderinDB(p);
        }

        private async void CancelOrderDB_Click(object sender, RoutedEventArgs e)
        {
            await orderViewModel.DeleteOrderAsync(Convert.ToInt32(CancelInDB.Text));
            /*TEST*/
        }
    }
}

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
    public sealed partial class MainPage : Page
    {
        PizzaViewModel pizzaViewModel;
        OrderViewModel orderViewModel;
        public MainPage()
        {
            this.InitializeComponent();
            Init();
        }
        private void Init()
        {
            pizzaViewModel = new PizzaViewModel();
            orderViewModel = new OrderViewModel();
            PizzaListView.ItemsSource = pizzaViewModel.httpPizzaList;
            OrderAsyncListView.ItemsSource = orderViewModel.httpOrderList;
            UpdatePizzaListView.ItemsSource = pizzaViewModel.httpPizzaList;
            GetPizzas();
            GetOrders();
        }

        private async void GetPizzas()
        {
            var pizzas = await pizzaViewModel.GetProductsAsync();

            foreach(Pizza p in pizzas)
            {
                pizzaViewModel.httpPizzaList.Add(p);

            }
        }
        private async void GetOrders()
        {
            var orders = await orderViewModel.GetOrdersAsync();

            foreach(Order o in orders)
            {
                orderViewModel.httpOrderList.Add(o);
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
            Order o = orderViewModel.MyOrder;
            await orderViewModel.ConfirmOrder(o);
            orderViewModel.httpOrderList.Clear();
            GetOrders();
        }

        private async void CancelOrderDB_Click(object sender, RoutedEventArgs e)
        {
            await orderViewModel.DeleteOrderAsync(CancelInDB.Text);
            CancelInDB.Text = "";
            orderViewModel.httpOrderList.Clear();
            GetOrders();
        }

        private async void UpdatePizzaPriceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPizza = UpdatePizzaListView.SelectedItem as Pizza;
            await pizzaViewModel.UpdateProductAsync(selectedPizza, UpdatePizzaPriceBox.Text);
            pizzaViewModel.httpPizzaList.Clear();
            GetPizzas();
            UpdatePizzaPriceBox.Text = "";
        }

        private async void RemovePizzaButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPizza = UpdatePizzaListView.SelectedItem as Pizza;
            await pizzaViewModel.RemoveProductAsync(selectedPizza);
            pizzaViewModel.httpPizzaList.Clear();
            GetPizzas();
        }
    }
}

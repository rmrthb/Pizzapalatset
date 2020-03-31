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

        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var selectedPizza = PizzaListView.SelectedItem as Pizza;
            orderViewModel.AddToCart(selectedPizza);
        }
        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            orderViewModel.CancelOrder();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

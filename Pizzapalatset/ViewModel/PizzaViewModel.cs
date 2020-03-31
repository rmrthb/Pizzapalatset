using Pizzapalatset.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzapalatset.ViewModel
{
    class PizzaViewModel
    {
        public ObservableCollection<Pizza> PizzaList = new ObservableCollection<Pizza>();

        public PizzaViewModel()
        {
            InitPizzaList();
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

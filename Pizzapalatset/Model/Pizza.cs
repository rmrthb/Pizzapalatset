using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzapalatset.Model
{
    class Pizza
    {
        public int PizzaID { get; set; }
        public string PizzaName { get; set; }
        public int PizzaPrice { get; set; }

        public Pizza(int pizzaid, string pizzaname, int pizzaprice)
        {
            PizzaID = pizzaid;
            PizzaName = pizzaname;
            PizzaPrice = pizzaprice;
        }
        public string DisplayPrice { get { return $"{PizzaPrice}kr"; } }
    }
}

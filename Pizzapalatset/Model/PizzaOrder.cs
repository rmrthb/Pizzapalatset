using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzapalatset.Model
{
    class PizzaOrder
    {
        public int PizzaID { get; set; }
        public int OrderID { get; set; }

        public PizzaOrder(int orderId, int pizzaId)
        {
            OrderID = orderId;
            PizzaID = pizzaId;
        }
        public PizzaOrder()
        {
        }
    }
}

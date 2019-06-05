using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
    public class Resource
    {
        public Main main;
        public string name = "", type = "";
        public double price = 0, basePrice = 0, flexibility = 0, supply = 0, demand = 0;

        public Resource(Main main, string name, string type, double price, double amount, double basePrice, double flexibility, double supply, double demand)
        {
            this.main = main;
            this.name = name;
            this.type = type;
            this.price = price;
            this.basePrice = basePrice;
            this.flexibility = flexibility;
            this.supply = supply;
            this.demand = demand;
        }
        public void ResourcePriceAverage()//find resource price average across sales
        {
            List<ResourceSale> sales = main.FindSales(name);
            double amount = 0;
            double cost = 0;
            foreach (var sale in sales)
            {
                amount += sale.amount + sale.soldLastTick;
                cost += (sale.amount + sale.soldLastTick) * sale.price;
            }
            price = cost / amount;
        }
    }
}

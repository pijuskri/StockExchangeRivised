using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
    public class Resource
    {
        public Instances main;
        public string name = "", type = "";
        public double price = 0, basePrice = 0, flexibility = 0, supply = 0, demand = 0;

		public Resource() {}
		public Resource(Instances main, string name, string type, double basePrice, double flexibility)
        {
            this.main = main;
            this.name = name;
            this.type = type;
            this.basePrice = basePrice;
            this.flexibility = flexibility;
            price = basePrice;
			main.resourceSales.Add(new ResourceSaleCategory(name));
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
        /// <summary>
        /// Returns total resource sold/total amount, 0 - none was sold, 1 - all was sold
        /// </summary>
        /// <returns></returns>
        public double ResourceDemand()
        {
            List<ResourceSale> sales = main.FindSales(name);
            double totalAmount = 0;
            double totalSold = 0;
            foreach (var sale in sales)
            {
                totalAmount += sale.amount + sale.soldLastTick;
                totalSold += sale.soldLastTick;
            }
            return totalSold / totalAmount;
        }
    }
}

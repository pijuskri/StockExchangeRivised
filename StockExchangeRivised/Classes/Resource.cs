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
		public List<ResourceSale> sales;

		public Resource() {}
		public Resource(Instances main, string name, string type, double basePrice, double flexibility)
        {
            this.main = main;
            this.name = name;
            this.type = type;
            this.basePrice = basePrice;
            this.flexibility = flexibility;
            price = basePrice;
			sales = new List<ResourceSale>();
        }
        public void ResourcePriceAverage()//find resource price average across sales
        {
            double amount = 0;
            double cost = 0;
            foreach (var sale in main.FindResource(name).sales)
            {
                amount += sale.amount + sale.soldLastTick;
                cost += (sale.amount + sale.soldLastTick) * sale.price;
            }
            if(amount>0)price = cost / amount;
        }

        /// <summary>
        /// Returns total resource sold/total amount, 0 - none was sold, 1 - all was sold
        /// </summary>
        /// <returns></returns>
        public double ResourceDemand()
        {
            double totalAmount = 0;
            double totalSold = 0;
            foreach (var sale in main.FindResource(name).sales)
            {
				if(sale.amount >= sale.produced) totalAmount += sale.amount - sale.produced;
				totalAmount += sale.soldLastTick;
				totalSold += sale.soldLastTick;
            }
			if (totalAmount > 0) return totalSold / totalAmount;
			else if (totalSold > 0) return 1;
			else
			{
				foreach (var company in main.companyList)
				{
					foreach (var resource in main.FindRecipe(company.productionRecipe).input)
					{
						if (resource.name == name) return 1;
					}
				}
			}
			return 0;
        }
    }
}

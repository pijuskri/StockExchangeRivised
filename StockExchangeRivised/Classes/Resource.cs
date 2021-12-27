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
        public double price = 0, basePrice = 0, flexibility = 0, priceLast=0, inflation25 = 0;
        public double production = 0, demand = 0, expectedProduction = 0, expectedDemand=0;
		public List<ResourceSale> sales;

		public Resource(string name, string type, double basePrice, double flexibility)
        {
	        main = Main.ic;
	        this.name = name;
            this.type = type;
            this.basePrice = basePrice;
            price = basePrice;
            this.flexibility = flexibility;
            price = basePrice;
			sales = new List<ResourceSale>();
        }
        public void ResourcePriceAverage()//find resource price average across sales
        {
	        priceLast = price;
            double amount = 0;
            double cost = 0;
            foreach (var sale in main.FindResource(name).sales) if(sale.soldLastTick>0)
            {
                amount += sale.amount + sale.soldLastTick;
                cost += (sale.amount + sale.soldLastTick) * sale.price;
            }
            if(amount>0)price = cost / amount;
        }

        public void CalculateInflation()
        {
	        double inflationCurrent = (price - priceLast) * 100;
	        if (Instances.currentTurn < 25)
	        {
		        inflation25 = (inflationCurrent + inflation25) / 2;
	        }
	        else
	        {
		        inflation25 = (inflation25 * 24 + inflationCurrent) / 25;
	        }
        }

        /// <summary>
        /// Returns total resource sold/total amount, 0 - none was sold, 1 - all was sold
        /// </summary>
        /// <returns></returns>
        public double ResourceDemand()
        {
            double totalAmount = 0;
            double totalSold = 0;
            foreach (var sale in sales)
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
					foreach (var resource in company.productionRecipe.input)
					{
						if (resource.resource.name == name) return 1;
					}
				}
			}
			return 0;
        }

        public double ResourceSupplyDemandRatio()
        {
	        double totalDemand = 0, totalSupply = 0; //demand
	        foreach (var demand in main.populationDemandList) if (demand.resource == this)
		    {
			    totalDemand += demand.amount * main.population.people * main.population.buyRatio;
		    }

		    foreach (var company in main.companyList)
		    {
			    foreach (var input in company.productionRecipe.input) if (input.resource == this)
			    {
			       totalDemand += input.amount * company.productionVolume * company.productionInput;
			    }
		    }
		    /*foreach (var sale in sales)
		    {
			    totalSupply += sale.produced;
		    }*/
		    foreach (var company in CompaniesProducingResource())
		    {
			    totalSupply += company.productionVolume * company.productionOutput;
		    }

            if (totalDemand > totalSupply && totalSupply > 0) return totalDemand / totalSupply;
            else if (totalSupply > totalDemand && totalDemand > 0) return -totalSupply / totalDemand;
            else if (totalSupply == 0 && totalDemand > 0) return 10;
		    else if (totalDemand == 0 && totalSupply > 0) return -10;
            else return 0;
        }

        public List<Company> CompaniesUsingResource()
        {
            List<Company> companies = new List<Company>();
            foreach (var company in main.companyList) foreach (var input in company.productionRecipe.input)
            {
	            if (input.resource == this) companies.Add(company);
            }
            return companies;
        }
        public List<Company> CompaniesProducingResource()
        {
	        List<Company> companies = new List<Company>();
	        foreach (var company in main.companyList) foreach (var output in company.productionRecipe.output)
	        {
		        if (output.resource == this) companies.Add(company);
	        }
	        return companies;
        }
    }
}

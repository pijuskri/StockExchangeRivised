using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
	public class Population
	{
		public Instances main;
		public int people = 100;
		public double money = 100, labourCost = 1, labourCostPast = 1;
		public int turnsUntilAi = 0;
		public double inflation5 = 1;
		public Population(Instances main, int people, double money, double labourCost)
		{
			this.main = main;
			this.people = people;
			this.money = money;
			this.labourCost = labourCost;
		}
		public void CreateNewAi()
		{
			if (turnsUntilAi == 100)
			{
				main.AIList.Add(new AI(main, "John" + main.AIList.Count, 1000 * main.population.labourCost, 0.1, 0.1));
				main.population.money -= 1000 * main.population.labourCost;
				turnsUntilAi = 0;
			}
			else turnsUntilAi++;
		}
		public void PopulationBuy()
		{
			foreach (var resource in main.populationDemandList) //browse all required resources for input
			{
				double amountNeeded = resource.amount * main.population.people;
				double amountBought = 0;
				double totalOnSale = main.TotalOnSale(resource.name);
				if (totalOnSale <= 0) continue;
				List<ResourceSale> sales = main.FindResource(resource.name).sales;//get sales by resource name
				ResourceSale.OrderSalesByPrice(sales);//sort by best price
				foreach (var sale in sales) //browse sales
				{
					//if (sale.price > main.FindResource(resource.name).basePrice * 5) break;
					if (amountBought >= amountNeeded || money < 0) break;
					double buyDist = (sale.amount / totalOnSale) * Math.Pow(main.FindResource(resource.name).price / sale.price, 2);
					if (main.FindResource(resource.name).basePrice < sale.price) buyDist *= Math.Pow(main.FindResource(resource.name).basePrice / sale.price, 0.5);//adjust amount to buy based on how
																																								   //more expensive than base price, for inflation control

					if (buyDist > 1) buyDist = 1;

					double toBuy = 0;
					if (amountNeeded > totalOnSale) toBuy = totalOnSale * buyDist;
					else toBuy = amountNeeded * buyDist;


					if (sale.amount < toBuy) toBuy = sale.amount;

					sale.soldThisTick += toBuy;
					sale.amount -= toBuy;
					money -= toBuy * sale.price;
					amountBought += toBuy;
				}
			}
		}
		public void Wages()
		{
			double sum = 0;
			labourCostPast = labourCost;
			foreach (var demand in main.populationDemandList)
			{
				Resource resource = main.FindResource(demand.name);
				sum += resource.price / resource.basePrice;
			}
			double prices = sum / main.populationDemandList.Count;
			if (prices > 1)
			{
				money += 5 * (Math.Pow(prices, 0.6) - 1);
			}
			if (prices > labourCost * 13) labourCost = (prices / 10 + labourCost * 9) / 10;

		}
		public void PopulationGrowth()
		{
			people += (int)(Math.Sqrt(people) * 0.02) + 1;
		}
		public void CalculateInflation5()
		{
			if (Instances.currentTurn < 5)
			{
				inflation5 += labourCost - labourCostPast;
			}
			else
			{
				inflation5 = (inflation5 * 4 + labourCost - labourCostPast) / 5;
			}
		}
	}
}

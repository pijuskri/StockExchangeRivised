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
		public double money = 5000, labourCost = 1, labourCostPast = 1, spent = 0, spentLast = 0;
		public double buyRatio = 0; // for outside usage
		public int turnsUntilAi = 0;
		public double inflation25 = 1;
		private Random random;
		public Population(Instances main, int people, double money, double labourCost)
		{
			this.main = main;
			this.people = people;
			this.money = money;
			this.labourCost = labourCost;
			random = new Random(Instances.randomizerSeed++ * DateTime.Now.Millisecond);
		}
		public void CreateNewAi()
		{
			if (turnsUntilAi >= 100 && main.population.money>100)
			{
				main.AIList.Add(new AI(main, "John" + main.AIList.Count, 1000 * main.population.labourCost, 0.1, 0.1));
				main.population.money -= 1000 * main.population.labourCost;
				turnsUntilAi = 0;
			}
			else turnsUntilAi++;
		}
		public void PopulationBuy()
		{
			spentLast = spent;
			spent = 0;
			buyRatio = 0;
			foreach (var resource in main.populationDemandList) //browse all required resources for input
			{
				double amountNeeded = resource.amount * main.population.people;
				double amountBought = 0;
				double totalOnSale = main.TotalOnSale(resource.resource.name);
				if (totalOnSale <= 0) continue;
				List<ResourceSale> sales = main.FindResource(resource.resource.name).sales;//get sales by resource name
				ResourceSale.OrderSalesByPrice(sales, new Location(0,0));//sort by best price
				foreach (var sale in sales) //browse sales
				{
					//if (sale.price > main.FindResource(resource.resource.name).basePrice * 5) break;
					if (amountBought >= amountNeeded || money < 0) return;
					double buyDist = (sale.amount / totalOnSale) * Math.Pow(resource.resource.price / sale.price, 2);
					double mult = 1;
					//adjust amount to buy based on how more expensive than base price, for inflation control
					if (resource.resource.basePrice < sale.price) mult *= Math.Pow(resource.resource.basePrice / sale.price, 0.5);

					if(spentLast*5>money) mult*=Math.Pow(money/(spentLast*5),2);
					buyDist *= mult;
					buyRatio += mult/main.populationDemandList.Count;
					if (buyDist > 1) buyDist = 1;

					double toBuy = 0;
					if (amountNeeded > totalOnSale) toBuy = totalOnSale * buyDist;
					else toBuy = amountNeeded * buyDist;
					//Console.WriteLine("\ntoBuy:{0} ",toBuy);


					if (sale.amount < toBuy) toBuy = sale.amount;

					sale.soldThisTick += toBuy;
					sale.amount -= toBuy;
					money -= toBuy * sale.price;
					spent += toBuy * sale.price;
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
				sum += demand.resource.price / demand.resource.basePrice;
			}
			double prices = sum / main.populationDemandList.Count;
			if (inflation25 > 0)
			{
				money += inflation25 * people * 0.01;
			}
			if (prices > labourCost * 13) labourCost = (prices / 10 + labourCost * 199) / 200;
			labourCost *= 0.995d;

		}
		public void PopulationGrowth()
		{
			people += (int)(Math.Pow(people,0.7) * 0.005) + 1;
			if (random.Next(0, 100) > 98) people -= (int)(people * 0.1);
		}
		public void CalculateLongtermInflation()
		{
			double inflationCurrent = (labourCost - labourCostPast) * 1000;
			if (Instances.currentTurn < 25)
			{
				inflation25 = (inflationCurrent + inflation25)/2;
			}
			else
			{
				inflation25 = (inflation25 * 24 + inflationCurrent) / 25;
			}
		}
	}
}

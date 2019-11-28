using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
	public class AI
	{
		Instances main;
		public string name;
		public double money, riskFactor, randomness;

		public AI(Instances main, string name, double money, double riskFactor, double randomness)
		{
			this.main = main;
			this.name = name;
			this.money = money;
			this.riskFactor = riskFactor;
			this.randomness = randomness;
		}
		public void AIMechanics() //AI investments
		{
			double moneyCap = main.population.labourCost * 20000;
			if (money > moneyCap)
			{
				main.population.money += (money - moneyCap);
				money = moneyCap;
			}
			money += 1;
			main.population.money -= 1;

			Random random = new Random(Instances.randomizerSeed++ * DateTime.Now.Millisecond);
			if (random.Next(0, 100) > 95) //build new companies
			{
				if (money > 50)
				{
					double max = -1;
					string found = "";
					foreach (var resource in main.resourceList)
					{
						if (resource.ResourceDemand() > max) { max = resource.ResourceDemand(); found = resource.name; }
					}
					if (max > 0.9) //found a decent investment
					{
						bool bought = false;
						foreach (var recipe in main.productionRecipeList)
						{
							foreach (var resource in recipe.output)
							{
								if (resource.name == found)
								{
									main.companyList.Add(new Company(main, found + main.companyList.Count, 10, 10, 0.05, 1, 1, 1, recipe.name));
									//create new company
									money -= recipe.costToBuild + 10;
									bought = true;
									break;
								}
							}
							if (bought) break;
						}

					}
				}
			}
			else if (random.Next(0, 100) > 80) //buy shares
			{
				if (money > 10)
				{
					foreach (var companyName in RankCompaniesByInvestability()) // go through best company list
					{
						Company company = main.FindCompany(companyName);
						int shareAmountToBuy = 0;
						if (money < 0) break;
						if (company.sharesOwnedByCompany > 0) //if company has shares to sell
						{
							if (money > company.sharePrice * company.sharesOwnedByCompany) shareAmountToBuy = company.sharesOwnedByCompany; //can buy all shares
							else shareAmountToBuy = (int)Math.Floor(money / company.sharePrice); //can only buy some, currently uses all money

							if (shareAmountToBuy > 0) //can buy any
							{
								company.sharesOwnedByCompany -= shareAmountToBuy;
								money -= company.sharePrice * shareAmountToBuy;
								company.money += company.sharePrice * shareAmountToBuy;
								company.AddShare(name, shareAmountToBuy);
							}
						}
						else // buy shares from other ai
						{
							foreach (var sale in main.shareSaleListing)
							{
								if (sale.company == company.name)
								{
									if (money > sale.price * sale.amount) shareAmountToBuy = (int)sale.amount; //can buy all shares
									else shareAmountToBuy = (int)Math.Floor(money / sale.price); //can only buy some, currently uses all money

									if (shareAmountToBuy > 0) //can buy any
									{
										sale.amount -= shareAmountToBuy;
										money -= sale.price * shareAmountToBuy;
										company.AddShare(name, shareAmountToBuy);
									}
									if (sale.amount == 0) main.shareSaleListing.Remove(sale);
									break;
								}
							}
						}
					}
				}
			}
			else if (random.Next(0, 100) > 60) //sell shares
			{
				bool sold = false;
				List<string> temp = RankCompaniesByInvestability();
				temp.Reverse(); // order worst to best, so worst company shares are sold
				foreach (var companyName in temp)
				{
					Company company = main.FindCompany(companyName);
					foreach (var share in company.shareList)
					{
						if (share.ownerName == name)
						{
							bool found = false;
							foreach (var shareSale in main.shareSaleListing)
							{
								if (shareSale.sellerName == name && shareSale.company == company.name)
								{
									found = true;
									shareSale.amount += share.amount;
									shareSale.price = company.sharePrice;
								}
							}
							if (!found) main.shareSaleListing.Add(new ShareSale(name, company.name, share.amount, company.sharePrice));
							company.shareList.Remove(share);
							sold = true;
							break;
						}
					}
					if (sold) break;
				}
			}

		}
		public List<string> RankCompaniesByInvestability() //based on point system, creates list based on them
		{
			List<string> nameList = new List<string>();
			List<double> pointList = new List<double>();
			foreach (var company in main.companyList)
			{
				double points = 0;
				points += company.value / 10; //favour big companies
				points += company.dividendPercent * 3;
				if (company.revenue > 0) points += company.revenue / company.value; //focus on efficiency
				else points += company.revenue * 10 / company.value;

				if (pointList.Count == 0) { pointList.Add(points); nameList.Add(company.name); continue; } //if list is empty, add first member
				for (int i = 0; i < pointList.Count; i++) //put in correct place
				{
					if (points > pointList[i]) { pointList.Insert(i, points); nameList.Insert(i, company.name); break; } //insert in place if more points
				}
				{ pointList.Add(points); nameList.Add(company.name); }
			}
			return nameList; //return only names
		}
	}
}

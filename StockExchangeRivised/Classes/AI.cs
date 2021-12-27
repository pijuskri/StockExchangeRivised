using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace StockExchangeRivised
{
	public class AI
	{
		Instances main;
		public string name;
		public double money, riskFactor, randomness;
		private Random random;
		public List<Loan> loanList;

		public AI(Instances main, string name, double money, double riskFactor, double randomness)
		{
			this.main = main;
			this.name = name;
			this.money = money;
			this.riskFactor = riskFactor;
			this.randomness = randomness;
			random = new Random(Instances.randomizerSeed++ * DateTime.Now.Millisecond);
			loanList = new List<Loan>();
		}
		public void AIMechanics() //AI investments
		{
			double moneyCap = main.population.labourCost * 8000;
			if (money > moneyCap)
			{
				main.population.money += (money - moneyCap);
				money = moneyCap;
			}

			if (main.population.money > 2000 * main.population.labourCost)
			{
				double toGet = main.population.money / (100 * main.AIList.Count);
				money += toGet;
				main.population.money -= toGet;
			}

			if (money > -1500 * main.population.labourCost) PayLoans();
			if(random.Next(0,100)>=95 && money<-40 && loanList.Count<3) GetLoan(100);
			Invest();
		}

		public void Invest()
		{
			if (money > 30 && random.Next(0, 100) > 90) //invest in or build new companies
			{
				double max = -1;
				string found = "";
				foreach (var resource in main.resourceList)
				{
					double points = resource.ResourceSupplyDemandRatio() + (resource.price - resource.basePrice) * 10;
					if(resource.CompaniesProducingResource().Count == 0 && ( resource.CompaniesUsingResource().Count>0 || resource.expectedDemand>0)) points = 999999;
					if (points > max) { max = points; found = resource.name; }
				}
				if (max > 0) //found a decent investment
				{
					bool bought = false;
					foreach (var recipe in main.productionRecipeList)
					{
						foreach (var resource in recipe.output) if (resource.resource.name == found) //currently only handles 1 source of resource production
						{
							if (random.Next(0, 100) > 50 || resource.resource.CompaniesProducingResource().Count>4)
									foreach(var company in RankCompaniesByInvestability(resource.resource.CompaniesProducingResource()))
							{
								//if (company.sharesOwnedByCompany == 0) continue; 
								BuyCompanyShares(company, true);
								if(money < 20)break;
							}
							else if(recipe.costToBuild < money + 50) //build comapany with size 1
							{
								Company newCompany = new Company(found + main.companyList.Count, 10, 10, 0.05, 1, 1,
									1, recipe.name);
								main.companyList.Add(newCompany);
								newCompany.AddShare(this, 10);
								money -= recipe.costToBuild;
							}
							
							bought = true;
							break;
						}
						if (bought) break;
					}

				}
			}
			else if (random.Next(0, 100) > 90 && money>10) //buy shares
			{
				BuyShares();
			}
			if (random.Next(0, 100) > 95) //sell shares
			{
				bool sold = false;
				List<Company> temp = RankCompaniesByInvestability(main.companyList);
				temp.Reverse(); // order worst to best, so worst company shares are sold
				foreach (var company in temp)
				{
					foreach (var share in company.shareList) if (share.owner == this)
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
					if (sold) break;
				}
			}
		}

		private void BuyShares()
		{
			Console.WriteLine("\ngamer\n");
			foreach (var company in RankCompaniesByInvestability(main.companyList)) // go through best company list
			{
				int shareAmountToBuy = 0;
				if (company.sharesOwnedByCompany > 0) //if company has shares to sell
				{
					BuyCompanyShares(company, false);
				}
				else // buy shares from other ai
				{
					foreach (var sale in main.shareSaleListing) if (sale.company == company.name)
					{
						if (money > sale.price * sale.amount) shareAmountToBuy = (int)sale.amount; //can buy all shares
						else shareAmountToBuy = (int)Math.Floor(money / sale.price); //can only buy some, currently uses all money

						if (shareAmountToBuy > 0) //can buy any
						{
							sale.amount -= shareAmountToBuy;
							money -= sale.price * shareAmountToBuy;
							company.AddShare(this, shareAmountToBuy);
						}
						if (sale.amount <= 0.01) main.shareSaleListing.Remove(sale);
						break;
					}
				}

				//if (shareAmountToBuy > 0) break;
				if (money < 30) break;
			}
		}
		public void BuyCompanyShares(Company company, bool investment) {
			int need = (int)(Math.Floor((money + 50) / company.sharePrice) * 0.5);
			if (company.sharesOwnedByCompany < need && investment) company.IssueShares(need - company.sharesOwnedByCompany);
			if (company.sharesOwnedByCompany < need && !investment) need = company.sharesOwnedByCompany;
			if (need > 0)
			{
				company.AddShare(this, need);
				money -= need * company.sharePrice;
				//main.population.money += need * company.sharePrice;
				company.money += company.sharePrice * need;
			}
		}

		private void GetLoan(double amount)
		{
			Bank bank = main.bankList[0];
			if (bank.money < amount) amount = bank.money;
			if (amount < 5) return;
			Loan loan = new Loan(bank, amount, bank.interest, 200);
			loanList.Add(loan);
			money += amount;
			bank.money -= amount;
		}

		public void PayLoans()
		{
			double sum = 0;
			for (int i = 0; i < loanList.Count; i++)
			{
				double toPay = 0;
				Loan loan = loanList[i];
				toPay += loan.totalMoneyLent / loan.timeToPay;
				loan.moneyPaidBack += toPay;
				toPay += loan.interest * (loan.totalMoneyLent - loan.moneyPaidBack) * 0.01;
				money -= toPay;
				loan.bank.money += toPay;
				sum += toPay;
				if (loan.moneyPaidBack >= loan.totalMoneyLent) { loanList.RemoveAt(i); i--; }
			}
		}

		public double OwnedAssets() {
			return main.companyList.Select(company => company.shareList.Where(share => share.owner == this).Select(share => share.amount * company.sharePrice).Sum()).Sum();
		}

		public List<Company> RankCompaniesByInvestability(List<Company> companies) //based on point system, creates list based on them
		{
			List<Company> nameList = new List<Company>();
			List<double> pointList = new List<double>();
			nameList.Select(company => {
				double points = 0;
				points += company.value / 100; //favour big companies
				points += company.dividendPercent * 100;
				if (company.revenue > 0) points += company.revenue / company.value; //focus on efficiency
				else points += company.revenue * 10 / company.value;
				points += (random.NextDouble()) * randomness * 10;
				return points;
			});
			/*
			foreach (var company in companies)
			{
				

				if (pointList.Count == 0) { pointList.Add(points); nameList.Add(company); continue; } //if list is empty, add first member
				for (int i = 0; i < pointList.Count; i++) //put in correct place
				{
					if (points > pointList[i]) { pointList.Insert(i, points); nameList.Insert(i, company); break; } //insert in place if more points
				}
				{ pointList.Add(points); nameList.Add(company); }
			}
			*/
			return nameList.Zip(pointList, (first, second) => new Tuple<Company, double>(first, second)).OrderByDescending(x => x.Item2).Select(x => x.Item1).ToList(); //return only names
		}
	}
}

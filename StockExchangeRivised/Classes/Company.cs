using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
	public class Company
	{
		public Instances main;
		public string name;
		public int totalShares = 0, sharesOwnedByCompany = 0;

		public double money = 0,
			dividendPercent = 0,
			revenue = 0,
			value = 0,
			sharePrice = 0,
			productionVolume = 0,
			productionOutput = 0,
			productionInput = 0;

		public Info info;
		public ProductionRecipe productionRecipe;
		public List<Share> shareList;
		public List<Loan> loanList;
		public List<ResourceOrder> resourceOrders;

		public Location location;
		public double transportationCost = 0;

		public Company(string name, int totalShares, double money, double dividendPercent,
			double productionVolume, double productionOutput, double productionInput, string productionRecipe)
		{
			main = Main.ic;
			this.name = name;
			this.totalShares = totalShares;
			sharesOwnedByCompany = totalShares;
			this.money = money;
			this.dividendPercent = dividendPercent;
			this.productionVolume = productionVolume;
			this.productionOutput = productionOutput;
			this.productionInput = productionInput;
			this.productionRecipe = main.FindRecipe(productionRecipe);
			shareList = new List<Share>();
			loanList = new List<Loan>();
			resourceOrders = new List<ResourceOrder>();
			info = new Info(this);
			location = Location.Random();
		}

		public void CompanyMoneyMechanics(List<Company> toRemove)
		{
			if (money < -1 * Math.Pow(value, 0.9) - 100)
			{
				toRemove.Add(this);
				return;
			}

			double moneyCap = (2000d / Math.Sqrt(main.companyList.Count)) * Math.Pow(main.population.labourCost * 10,0.8);
			if (money > moneyCap)
			{
				//main.population.money += (money - moneyCap);
				//money = moneyCap;
			}

			value = productionVolume * productionRecipe.costToBuild * main.population.labourCost * 10;
			sharePrice = value / totalShares; //calculate share price
			revenue = 0;
			transportationCost = 0;

			SoldResources();
			BuyResources();

			LabourCosts();
			DividendPayment();

			ResourceProduction();

			info.Calculate5Turns();

			Administration();
			PayPopulation(transportationCost);
			Console.WriteLine("transport:" + transportationCost);
			PayLoans();

			money += revenue;
			
			CompanyInvestment();
		}

		public void CompanyInvestment()
		{
			Random random = new Random(Instances.randomizerSeed++ * DateTime.Now.Millisecond);
			if (sharePrice > 50 * main.population.labourCost && random.Next(0, 100) > 95
			) //make some shares if price too big
			{
				IssueShares(Convert.ToInt32(value / (1000*main.population.labourCost)));
			}

			// chose best money raising option
			if (money < 0 && info.revenue5Turns < 0 && random.Next(0, 100) > 90)
			{
				if (random.Next(0, 100) > 95 && sharesOwnedByCompany == 0)
				{
					IssueShares(Convert.ToInt32(Math.Pow(value, 0.5) / 2)); //TODO - issue shares more logically
				}
				else if (false) //TODO: loans
				{

				}

			}
			//if doing well, expand
			else if (money > 0 && revenue > 0 && random.Next(0, 100) > 80)
			{
				Console.WriteLine("INVEST: {0}:{1},{2}", name, CompanyOutputDemand(), CompanyOutputGlobalDemand());
				if (CompanyOutputDemand() > 0.85 && info.actualProductionPast5Turns > 0.8 && CompanyOutputGlobalDemand() > 0.85 && CompanyOutputExpectedDemand()>-0.5) //is there demand
				{
					double cost = productionRecipe.costToBuild * main.population.labourCost * 10;
					if (money < cost) GetLoan(cost-money);

					int numberToBuild = 0;
					numberToBuild = (int) Math.Floor(money / cost); //TODO: base numbertobuild on demand 
					money -= numberToBuild * cost; //lose money
					main.population.money += numberToBuild * cost;
					productionVolume += numberToBuild; //add number
				}
				else //if not enough demand, invest in efficiency
				{
					double cost = 10 * value * main.population.labourCost;
					if (money > cost) //can afford
					{
						money -= cost; //lose money
						main.population.money += cost;
						productionInput *= 0.99;
					}
				}
			}

			// if not doing well, close factories
			if (info.actualProductionPast5Turns < 1 && info.revenue5Turns < 0 && random.Next(0, 100) > 90 &&
			    (CompanyOutputGlobalDemand() < 0.8 || info.inputFulfilled < 0.7))
			{
				int amountToClose = (int) Math.Floor((1 - info.actualProductionPast5Turns) * productionVolume) / 2;
				money += amountToClose * productionRecipe.costToBuild;
				main.population.money += amountToClose * productionRecipe.costToBuild;
				productionVolume -= amountToClose;

			}
		}

		#region Misc

		public void DividendPayment()
		{
			if (dividendPercent > 0) foreach (var share in shareList) //if pays dividends
			{
				double pay = revenue * dividendPercent * (share.amount / totalShares);
				if (pay > 0)
				{
					share.owner.money += pay;

					if (info.totalProduced > 0) info.costToProduce += pay / info.totalProduced;
					else info.costToProduce += pay;

					revenue -= pay;
				}
			}
		}

		public void Bankrupcy()
		{
			foreach (var resource in main.resourceList)
			{
				for (int i = 0; i < resource.sales.Count; i++)
				{
					if (resource.sales[i].company == this)
					{
						resource.sales.Remove(resource.sales[i]);
						i--;
					}
				}
			}

			for (int i = 0; i < main.shareSaleListing.Count; i++)
			{
				if (main.shareSaleListing[i].company == name)
				{
					main.shareSaleListing.RemoveAt(i);
					i--;
				}
			}

			//main.population.money += money + value;
			main.companyList.RemoveAt(main.FindCompanyID(name));
		}

		public void Administration()
		{
			//double cost = Math.Pow(productionVolume, 1.1) * 0.02;
			double cost = Math.Tanh(productionVolume/10) * 0.1 * productionVolume;

			main.population.money += cost;
			revenue -= cost;
		}

		#endregion

		#region Production

		public void BuyResources()
		{
			double partToBuy = 1;
			info.costToProduce = 0;
			if (info.producedPast5Turns > 0 && CompanyOutputGlobalDemand() < 0.95)
				partToBuy = info.soldPast5Turns / info.producedPast5Turns; //calculate last turns demand
			if (CompanyOutputExpectedDemand() > 0) partToBuy = 1;
			double amountStored = 0;
			if (productionRecipe.output[0].resource.sales.Find(x => x.company == this) != null) amountStored = productionRecipe.output[0].resource.sales.Find(x => x.company == this).amount;
			if (amountStored > info.soldPast5Turns && info.soldPast5Turns > 0) partToBuy = info.soldPast5Turns / amountStored;
			Console.WriteLine("amount stored: " + amountStored + " " + info.soldPast5Turns);
			if (partToBuy > 1) partToBuy = 1;
			Console.WriteLine("\ncompany demand: " + CompanyOutputDemand() + " " + this.name);

			if (productionRecipe.input.Count == 0) //if have no inputs to produce, don't buy resources
			{
				info.actualProduction = partToBuy;
				return;
			}

			double totalAmountBought = 0, totalAmountNeeded = 0, totalPrice = 0;
			foreach (var resource in productionRecipe.input) //browse all required resources for input
			{
				double amountNeeded = resource.amount * productionVolume * productionInput * partToBuy;
				double amountBought = 0;
				double totalOnSale = main.TotalOnSale(resource.resource.name);
				if (totalOnSale <= 0) continue;
				
				foreach (var order in resourceOrders) if(order.sale.resource == resource.resource) {
					PayOrder(order, ref totalPrice, ref amountBought);
				}

				List<ResourceSale> sales = new List<ResourceSale>(main.FindResource(resource.resource.name).sales); //get sales by resource name
				ResourceSale.OrderSalesByPrice(sales, location); //sort by best price
				foreach (var sale in sales) //browse sales
				{
					if (amountBought >= amountNeeded) break;
					double buyDist = (sale.amount / totalOnSale) * Math.Pow(resource.resource.price / sale.price, 1);

					double toBuy = amountNeeded * buyDist;
					if (sale.amount < toBuy) toBuy = sale.amount;
					if (toBuy > amountNeeded - amountBought) toBuy = amountNeeded - amountBought;

					PayOrder(sale.MakeOrder(toBuy, this), ref totalPrice, ref amountBought);
					
					/*
					sale.soldThisTick += toBuy;
					totalPrice += toBuy * sale.price;
					sale.amount -= toBuy;
					amountBought += toBuy;
					TransportationCosts(sale.company, toBuy);
					*/
				}

				totalAmountBought += amountBought;
				totalAmountNeeded += amountNeeded;
			}
			resourceOrders = resourceOrders.FindAll(x => x.timeLeft > 0);

			if (totalAmountNeeded > 0)
				info.inputFulfilled = (totalAmountBought / totalAmountNeeded); //0-1, actual company production
			if (info.inputFulfilled > 1) info.inputFulfilled = 1;
			info.actualProduction = info.inputFulfilled * partToBuy;
			revenue -= totalPrice;
			if (totalAmountBought > 0 && totalPrice > 0) info.costToProduce = totalPrice / totalAmountBought;
			else if (totalPrice > 0) info.costToProduce = totalPrice;
			else info.costToProduce = main.population.labourCost * 10;
			Console.WriteLine("cost to do stuff:{0}", info.costToProduce);
		}

		private void PayOrder(ResourceOrder order, ref double totalPrice, ref double amountBought) {
			double amount = order.sale.amount > order.amount ? order.amount : order.sale.amount;
			order.sale.soldThisTick += amount;
			totalPrice += amount * order.price;
			order.sale.amount -= amount;
			amountBought += amount;
			TransportationCosts(order.sale.company, amount);
			order.timeLeft--;
		}

		public void TransportationCosts(Company other, double count) {
			transportationCost += count * location.Distance(other.location)/1000;
		}

		public void ResourceProduction()
		{
			info.totalProduced = 0;
			foreach (var resource in productionRecipe.output)
			{
				double production = resource.amount * productionVolume * productionOutput * info.actualProduction;
				info.totalProduced += production;
				List<ResourceSale> sales = resource.resource.sales;
				bool found = false;
				foreach (var sale in sales) if (sale.company == this)
				{
					found = true;
					double speed = 0.01 * Math.Sqrt(main.population.labourCost * 10);
					if (main.population.inflation25 < 0.01 || sale.soldLastTick <= 0.1 || true)
					{
						double ratio = 0;
						if (sale.soldLastTick >= production * 0.9 && production > 0) ratio = sale.soldLastTick / production - 0.5;
						else if (sale.soldLastTick > 0) ratio = -production / sale.soldLastTick + 1;
						else if (sale.soldLastTick <= 0.01) ratio -= 2;
						if (CompanyOutputExpectedDemand() > 0) ratio += CompanyOutputExpectedDemand();

						// if (CompanyOutputGlobalDemand() < 0.3) ratio += CompanyOutputGlobalDemand() - 0.3;
						//ratio += RatioN(sale.price, sale.resource.price) * 0.1;
						if (AverageCompanyRevenue() < 0 && CompanyOutputExpectedDemand() > 0) ratio += 2;
						ratio -= Math.Max((sale.resource.inflation25 + main.population.inflation25 * 0.2) * Math.Pow(sale.price * 10, 1.5), 0);
						if(AverageOutputCompanyRevenue()<0)ratio-=0.2;
						ratio = MyMath.Clamp(ratio, -10, 10);

						sale.price += resource.resource.basePrice * ratio * 0.001;
						if (sale.price <= 0) sale.price = 0.01;
						Console.WriteLine("ratio for sale:{0}", ratio);
						Console.WriteLine("resource demand:{0}", CompanyOutputExpectedDemand());

					}

					sale.amount += production;
					sale.produced = production;

					break;
				}

				if (!found) sales.Add(new ResourceSale(resource.resource, production, resource.resource.price, this));
			}

		}

		///Calculates the labour costs and impacts revenue and population income
		public void LabourCosts()
		{
			double labourCosts = 0;
			labourCosts = productionVolume * (info.actualProduction / 2 + 0.5) * productionRecipe.labourToProduce * main.population.labourCost * productionInput;
			main.population.money += labourCosts;
			revenue -= labourCosts;
			Console.WriteLine("labour:{0}", labourCosts);

			if (info.totalProduced > 0) info.costToProduce += labourCosts / info.totalProduced;
			else info.costToProduce += labourCosts;
		}

		/// Calculates how many resources were sold, add to revenue
		public void SoldResources()
		{
			info.totalSold = 0;
			foreach (var resource in main.resourceList)
			{
				foreach (var sale in resource.sales) if (sale.company == this)
				{
					revenue += sale.soldThisTick * sale.price;
					info.totalSold += sale.soldThisTick;
					sale.soldLastTick = sale.soldThisTick;
					if (sale.age < 5) sale.soldPast5Turns += sale.soldThisTick;
					else sale.soldPast5Turns = (sale.soldPast5Turns * 4 + sale.soldThisTick) / 5;
					sale.soldThisTick = 0;
					sale.age++;
				}
			}
		}

		#endregion

		#region Loan

		private void GetLoan(double amount)
		{
			Bank bank = main.bankList[0];
			if (bank.money < amount) amount = bank.money;
			Loan loan  = new Loan(bank,amount,bank.interest,200);
			loanList.Add(loan);
			money += amount;
			bank.money -= amount;
		}

		public void PayLoans()
		{
			double sum = 0;
			for (int i=0;i<loanList.Count;i++)
			{
				double toPay = 0;
				Loan loan = loanList[i];
				toPay += loan.totalMoneyLent / loan.timeToPay;
				loan.moneyPaidBack += toPay;
				toPay += loan.interest * (loan.totalMoneyLent - loan.moneyPaidBack) * 0.01;
				revenue -= toPay;
				loan.bank.money += toPay;
				sum += toPay;
				if (loan.moneyPaidBack>=loan.totalMoneyLent) { loanList.RemoveAt(i); i--; }
			}
			Console.WriteLine("loan payment:{0}", sum);
			
			
		}

		#endregion Loan
        #region Shares
        public void AddShare(AI owner, int amount)
        {
            foreach (var share in shareList) if (share.owner == owner) //check if already on list
            {
                 share.amount += amount;
                 return;
            }
            shareList.Add(new Share(owner, amount));
        }
        public void IssueShares(int number)
        {
            totalShares += number;
            sharesOwnedByCompany += number;
            sharePrice = totalShares / value;
        }
		#endregion
		#region Demand
		/// <summary>
		/// calculates total demand of resources produced by company
		/// </summary>
		/// <returns>0-1 ratio of demand</returns>
		public double CompanyOutputDemand()
        {
			double totalAmount = 0;
			double totalSold = 0;
			foreach (var sale in main.FindSalesCompany(name))
			{
				double pricediff = Math.Pow(sale.resource.basePrice / sale.price, 0.5);//adjust amount to buy based on how
				if (pricediff > 1) pricediff = 1;

				if (sale.amount* pricediff >= sale.produced) totalAmount += (sale.amount - sale.produced)* pricediff;
				totalAmount += sale.soldLastTick * pricediff;
				totalSold += sale.soldLastTick;
			}
			if (totalAmount > 0) return totalSold / totalAmount;
			else if (totalSold > 0) return 1;
			else
			{
				foreach (var resource in productionRecipe.output)
				{
					foreach (var demand in main.populationDemandList)
					{
						if (demand.resource.name == resource.resource.name && demand.amount * main.population.people > 0) return 1;
					}
				}

			}
			return 0;
		}

		public double CompanyOutputGlobalDemand()
		{
			double totalAmount = 0;
			double totalSold = 0;
			foreach (var resource in productionRecipe.output)
			{
				foreach (var sale in main.FindResource(resource.resource.name).sales)
				{
					double pricediff = Math.Pow(sale.resource.basePrice / sale.price, 0.5);//adjust amount to buy based on how
					if (pricediff > 1) pricediff = 1;

					if (sale.amount * pricediff >= sale.produced) totalAmount += (sale.amount - sale.produced) * pricediff;
					totalAmount += sale.soldLastTick * pricediff;
					totalSold += sale.soldLastTick;
				}
			}
			if (totalAmount > 0) return totalSold / totalAmount;
			else if (totalSold > 0) return 1;
			else
			{
				foreach (var resource in productionRecipe.output)
				{
					foreach (var demand in main.populationDemandList)
					{
						if (demand.resource.name == resource.resource.name && demand.amount * main.population.people > 0) return 1;
					}
				}
				
			}
			return 0;
		}

		public double CompanyOutputExpectedDemand()
		{
			double totalAmount = 0; //demand
			double totalSold = 0; //actually sold
			foreach (var resource in productionRecipe.output)
			{
				foreach (var sale in main.FindResource(resource.resource.name).sales)
				{
					totalSold += sale.soldLastTick;
				}

                foreach (var demand in main.populationDemandList) if (demand.resource.name == resource.resource.name)
                {
					 totalAmount += demand.amount * main.population.people * main.population.buyRatio;
				}

				foreach (var company in main.companyList)
				{
					foreach (var input in company.productionRecipe.input) if(input.resource.name==resource.resource.name)
					{
						totalAmount += input.amount * company.productionVolume * company.productionInput *company.info.actualProduction; 
					}
				}
			}
			return (totalSold>0)? totalAmount/totalSold - 1 : -1;
        }

		public double AverageCompanyRevenue()
		{
			double sum=0;
			int count=0;
			foreach (var company in main.companyList) if(company.productionRecipe == productionRecipe)
			{
				sum += company.info.revenue5Turns;
				count++;
			}

			return sum / count;
		}

		public double AverageOutputCompanyRevenue() //0 if outputs arent used by companies
		{
			double sum = 0;
			int count = 0;
			foreach (var sale in productionRecipe.output) foreach(var company in sale.resource.CompaniesUsingResource())
			{
				count++;
				sum += company.revenue;
			}
			if (count > 0) return sum / count;
			else return 0;
		}

		#endregion
        #region Experimantal
        public double RatioN(double a, double b)
        {
            double ans = 0;
            if (a>=b) ans = a/b - 1;
            else ans = b / a - 1;
			return ans;
        }
        public double SaleRatioCalc(double a, double b)
        {
            double ats = 0;
            if (a > b && b > 0) ats = a / b;
            else if (b>a && a > 0) ats = b / a;
            ats = (ats/50)+1;
            return ats;
        }
		private double tanh (double x, double cap, double accent) {
			return cap * ( (Math.Exp(accent * x) - 1) / (Math.Exp(accent * x) + 1) );
		}
		private void PayPopulation(double amount) {
			revenue -= amount;
			main.population.money += amount;
		}
        #endregion
	}

	///Stores all company related statistics and information
	public class Info
	{
		private Company company;
		public double totalSold = 0, totalProduced = 0, actualProduction = 0, inputFulfilled = 0, costToProduce = 0, producedPast5Turns = 0,
			soldPast5Turns = 0, actualProductionPast5Turns=0, revenue5Turns=0;

		public Info(Company company)
		{
			this.company = company;
		}

		public void Calculate5Turns()
		{
			if (Instances.currentTurn < 5)
            {
                producedPast5Turns += totalProduced;
                soldPast5Turns += totalSold;
                actualProductionPast5Turns = (actualProductionPast5Turns + actualProduction) / 2;
                revenue5Turns = (revenue5Turns + company.revenue) / 2;
            }
			else
            {
                producedPast5Turns = (producedPast5Turns * 4 + totalProduced)/5;
                soldPast5Turns = (soldPast5Turns * 4 + totalSold) / 5;
                actualProductionPast5Turns = (actualProductionPast5Turns * 4 + actualProduction) / 5;
                revenue5Turns = (revenue5Turns * 4 + company.revenue) / 5;
            }
		}
    }
}

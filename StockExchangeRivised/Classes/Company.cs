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
        public string name = "";
        public int totalShares = 0, sharesOwnedByCompany = 0;
        public double money = 0, dividendPercent = 0, revenue = 0, value = 0, sharePrice = 0, productionVolume = 0, productionOutput = 0, productionInput = 0;
		public Info info;
        public string productionRecipe = "";
        public List<Share> shareList;

        public Company(Instances main, string name, int totalShares, double money, double dividendPercent, 
            double productionVolume, double productionOutput, double productionInput, string productionRecipe)
        {
            this.main = main;
            this.name = name;
            this.totalShares = totalShares;
            this.money = money;
            this.dividendPercent = dividendPercent;
            this.productionVolume = productionVolume;
            this.productionOutput = productionOutput;
            this.productionInput = productionInput;
            this.productionRecipe = productionRecipe;
            sharesOwnedByCompany = this.totalShares;
            shareList = new List<Share>();
			info = new Info();
        }

        public void CompanyMoneyMechanics(List<Company> toRemove)
        {
            if (money < -1 * (value) * main.population.labourCost * 10 - 100)  { toRemove.Add(this); return; }
			double moneyCap = (1000 / main.companyList.Count) * main.population.labourCost * 10;
			if (money > moneyCap)
            {
                main.population.money += (money - moneyCap);
                money = moneyCap;
            }
            value = productionVolume * main.FindRecipe(productionRecipe).costToBuild * main.population.labourCost*10;
			sharePrice = value / totalShares; //calculate share price
			revenue = 0;

            SoldResources();
            BuyResources();

			LabourCosts();
			Administration();
			DividendPayment();

			ResourceProduction();
            info.Calculate5Turns();
			Administration();

            
            money += revenue;

            
            CompanyInvestment();
        }

        public void CompanyInvestment()
        {
			Random random = new Random(Instances.randomizerSeed++ * DateTime.Now.Millisecond);
			/*if (random.Next(0, 100) > 95) //make some shares if price too big
            {
                if (sharePrice > 3)
                {
                    IssueShares(company, Convert.ToInt32(value / 2));
                }
            }*/
			// chose best money raising option
			if (money < 0 && revenue < 0)
            {
                if (random.Next(0, 100) > 95 && sharesOwnedByCompany == 0)
                {
                    IssueShares(Convert.ToInt32(value / 2)); //TODO - issue shares more logically
                }
                else if (false) //TODO: loans
                {

                }

            }
            //if doing well, expand
            else if (money > 0 && revenue > 0 && random.Next(0, 100)>80)
            {
				Console.WriteLine("INVEST: {0}:{1},{2}", name, CompanyOutputDemand(), CompanyOutputGlobalDemand());
				if (CompanyOutputDemand() > 0.9 && info.actualProductionPast5Turns > 0.9 && CompanyOutputGlobalDemand() > 0.9) //is there demand
                {
					double cost = main.FindRecipe(productionRecipe).costToBuild * main.population.labourCost * 10;
					if (money > cost) //can afford
                    {
                        int numberToBuild = 0;
                        numberToBuild = (int)Math.Floor(money / cost); //TODO: base numbertobuild on demand 
                        money -= numberToBuild * cost; //lose money
                        main.population.money += numberToBuild * cost;
                        productionVolume += numberToBuild; //add number
                    }
                }
                else //if not enough demand, invest in efficiency
                {
					double cost = 1000 * (1 / productionInput) * main.population.labourCost;
					//Console.WriteLine("INVEST: {0}:{1},{2}", name, CompanyOutputDemand(), CompanyOutputGlobalDemand());
					if (money > cost) //can afford
                    {
                        money -= cost; //lose money
                        main.population.money += cost;
                        productionInput *= 0.95;
                    }
                }
            }
            // if not doing well, close factories
            if (info.actualProductionPast5Turns < 1 && revenue < 0 && random.Next(0, 100) > 90 && CompanyOutputDemand()<0.8 && CompanyOutputGlobalDemand()<0.8)
            {
                int amountToClose = (int)Math.Floor((1 - info.actualProductionPast5Turns) * productionVolume) / 2;
                money += amountToClose * main.FindRecipe(productionRecipe).costToBuild;
				main.population.money += amountToClose * main.FindRecipe(productionRecipe).costToBuild;
				productionVolume -= amountToClose;

            }
        }

        public void DividendPayment()
        {
            if (dividendPercent > 0)//if pays dividends
            {
                foreach (var share in shareList) //
                {
					double pay = revenue * dividendPercent * (share.amount / totalShares);
					if (pay > 0)
					{
						main.FindAI(share.ownerName).money += pay;

						if (info.totalProduced > 0) info.costToProduce += pay / info.totalProduced;
						else info.costToProduce += pay;

						revenue -= pay;
					}
                }
            }
        }
        public void Bankrupcy()
        {
            foreach (var resource in main.resourceList)
            {
                for (int i = 0; i < resource.sales.Count; i++)
                {
                    if (resource.sales[i].company == name)
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
			main.population.money += money + value;
            main.companyList.RemoveAt(main.FindCompanyID(name));
        }
		public void Administration()
		{
			double cost = revenue * Math.Pow(productionVolume, 1.3) * 0.001;
			

			main.population.money += cost;
			revenue -= cost;
		}
        #region Production
        public void BuyResources()
        {
            double partToBuy = 1;
            info.costToProduce = 0;
            if (info.producedPast5Turns > 0 && CompanyOutputDemand() < 0.95) partToBuy = info.soldPast5Turns / info.producedPast5Turns; //calculate last turns demand
            if (partToBuy > 1) partToBuy = 1;
            Console.WriteLine("\ncompany demand: "+ CompanyOutputDemand() + " " + this.name); 

            if (main.productionRecipeList[main.FindRecipeID(productionRecipe)].input.Count == 0)//if have no inputs to produce, don't buy resources
            {
                info.actualProduction = partToBuy;
                return;
            }
            double totalAmountBought = 0, totalAmountNeeded = 0, totalPrice = 0;
            foreach (var resource in main.FindRecipe(productionRecipe).input) //browse all required resources for input
			{
                double amountNeeded = resource.amount * productionVolume * productionInput * partToBuy;
                double amountBought = 0;
				double totalOnSale = main.TotalOnSale(resource.name);
				if (totalOnSale <= 0) continue;
				List<ResourceSale> sales = main.FindResource(resource.name).sales;//get sales by resource name
                ResourceSale.OrderSalesByPrice(sales);//sort by best price
                foreach (var sale in sales) //browse sales
                {
                    if (amountBought >= amountNeeded) break;
					double buyDist = (sale.amount / totalOnSale) * Math.Pow(main.FindResource(resource.name).price / sale.price, 1);

					double toBuy = amountNeeded * buyDist;
					if (sale.amount < toBuy) toBuy = sale.amount;

					sale.soldThisTick += toBuy;
                    totalPrice += toBuy * sale.price;
                    sale.amount -= toBuy;
                    amountBought += toBuy;
                }
                totalAmountBought += amountBought;
                totalAmountNeeded += amountNeeded;
            }
            if (totalAmountNeeded > 0) info.actualProduction = (totalAmountBought / totalAmountNeeded) * partToBuy; //0-1, actual company production
            revenue -= totalPrice;
            if (totalAmountBought > 0) info.costToProduce = totalPrice / totalAmountBought;
            else info.costToProduce = totalPrice;
        }
        public void ResourceProduction()
        {


            info.totalProduced = 0;
            foreach (var resource in main.FindRecipe(productionRecipe).output)
            {
                double production = resource.amount * productionVolume * productionOutput * info.actualProduction;
                info.totalProduced += production;
                List<ResourceSale> sales = main.FindResource(resource.name).sales;
                bool found = false;
                foreach (var sale in sales)
                {
                    if (sale.company == name)
                    {
                        found = true;
						double speed = 0.01 * Math.Sqrt(main.population.labourCost * 10);
						double inflationControl = sale.price / main.FindResource(resource.name).basePrice;
						//if(main.FindResource(resource.name).ResourceDemand()<0.9 && production > sale.soldLastTick) sale.price = main.FindResource(resource.name).price * 0.9;
						if (inflationControl < 100)
						{
							if ((production <= sale.soldLastTick && main.FindResource(resource.name).ResourceDemand() > 0.9 && CompanyOutputDemand() > 0.9) || info.costToProduce > sale.price) sale.price += speed;
							if (revenue < 0 && money < 0 && info.costToProduce > sale.price && production>0) sale.price = info.costToProduce * 1.1;
						}
						if (production > sale.soldLastTick && CompanyOutputDemand() < 0.9 && info.costToProduce < sale.price) sale.price -= speed;
						if (CompanyOutputDemand() < 0.1 && CompanyOutputGlobalDemand() > 0.9) sale.price = main.FindResource(resource.name).price;
						
						//else if(productionVolume>=1 && sale.soldLastTick<1 && revenue<0) sale.price = Main.changeNumberTowards
						sale.amount += production;
						sale.produced = production;

						break;
                    }
                }
                if (!found) sales.Add(new ResourceSale(resource.name,production, main.FindResource(resource.name).price, name));
            }

        }

        ///Calculates the labour costs and impacts revenue and population income
        public void LabourCosts()
        {
			double labourCosts = 0;
			labourCosts = productionVolume * (info.actualProduction / 2 + 0.5) *
             main.FindRecipe(productionRecipe).labourToProduce * main.population.labourCost * productionInput;
            main.population.money += labourCosts;
            revenue -= labourCosts;
            //Console.WriteLine("labour:{0}", labourCosts);

            if (info.totalProduced > 0) info.costToProduce += labourCosts / info.totalProduced;
            else info.costToProduce += labourCosts;
        }

        /// Calculates how many resources were sold, add to revenuw
        public void SoldResources()
        {
            info.totalSold = 0;
            foreach (var resource in main.resourceList)
            {
                foreach (var sale in resource.sales)
                {
                    if (sale.company == name)
                    {
                        revenue += sale.soldThisTick * sale.price;
                        info.totalSold += sale.soldThisTick;
                        //Console.WriteLine("{0} {1}", resource.name, sale.soldThisTick);
                        sale.soldLastTick = sale.soldThisTick;
                        sale.soldThisTick = 0;
                    }
                }
            }
        }
        #endregion

        public void AddShare(string ownerName, int amount)
        {
            foreach (var share in shareList) //check if already on list
            {
                if (share.ownerName == ownerName) { share.amount += amount; return; }
            }
            shareList.Add(new Share(ownerName, amount));
        }
        public void IssueShares(int number)
        {
            totalShares += number;
            sharesOwnedByCompany += number;
            sharePrice = totalShares / value;
            money += sharePrice * number;
        }
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
				double pricediff = Math.Pow(main.FindResource(sale.resource).basePrice / sale.price, 0.5);//adjust amount to buy based on how
				if (pricediff > 1) pricediff = 1;

				if (sale.amount* pricediff >= sale.produced) totalAmount += (sale.amount - sale.produced)* pricediff;
				totalAmount += sale.soldLastTick * pricediff;
				totalSold += sale.soldLastTick;
			}
			if (totalAmount > 0) return totalSold / totalAmount;
			else if (totalSold > 0) return 1;
			else
			{
				foreach (var resource in main.FindRecipe(productionRecipe).output)
				{
					foreach (var demand in main.populationDemandList)
					{
						if (demand.name == resource.name && demand.amount * main.population.people > 0) return 1;
					}
				}

			}
			return 0;
		}

		public double CompanyOutputGlobalDemand()
		{
			double totalAmount = 0;
			double totalSold = 0;
			foreach (var resource in main.FindRecipe(productionRecipe).output)
			{
				foreach (var sale in main.FindResource(resource.name).sales)
				{
					double pricediff = Math.Pow(main.FindResource(sale.resource).basePrice / sale.price, 0.5);//adjust amount to buy based on how
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
				foreach (var resource in main.FindRecipe(productionRecipe).output)
				{
					foreach (var demand in main.populationDemandList)
					{
						if (demand.name == resource.name && demand.amount * main.population.people > 0) return 1;
					}
				}
				
			}
			return 0;
		}
		public double SaleRatioCalcExperimental(double a, double b)
        {
            double ans = 0;
            if (a + b > 0) ans = Math.Abs(a - b) / (a + b);
            ans = (ans) + 1;
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
    }

	///Stores all company related statistics and information
    public class Info
    {
		public double totalSold = 0, totalProduced = 0, actualProduction = 0, costToProduce = 0, producedPast5Turns = 0,
			soldPast5Turns = 0, actualProductionPast5Turns=0;
		public void Calculate5Turns()
		{
			if (Instances.currentTurn < 5)
            {
                producedPast5Turns += totalProduced;
                soldPast5Turns += totalSold;
                actualProductionPast5Turns = (actualProductionPast5Turns + actualProduction) / 2;
            }
			else
            {
                producedPast5Turns = (producedPast5Turns * 4 + totalProduced)/5;
                soldPast5Turns = (soldPast5Turns * 4 + totalSold) / 5;
                actualProductionPast5Turns = (actualProductionPast5Turns * 4 + actualProduction) / 5;
            }
		}
    }
}

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
            if (money < -1 * (value) - 100) { toRemove.Add(this); return; }
            if (money > 300)
            {
                main.population.money += (money - 300);
                money = 300;
            }
            value = productionVolume * 10;//TODO - base value on something more cool
            revenue = 0;

            SoldResources();
            main.population.PopulationBuy();
            BuyResources();
            ResourceProduction();
            info.Calculate5Turns();

            sharePrice = value / totalShares; //calculate share price
            money += revenue;

            DividendPayment();
            CompanyInvestment();
        }

        public void CompanyInvestment()
        {
            Random random = new Random(Instances.randomizerSeed++);
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
            else if (money > 0 && revenue > 0 && random.Next(0, 100) > 90)
            {
                Console.WriteLine("INVEST: {0}:{1}",name,CompanyOutputDemand());
                if (CompanyOutputDemand() > 0.9 && info.actualProductionPast5Turns > 0.9) //is there demand
                {
                    if (money > 20) //can afford
                    {
                        int numberToBuild = 0;
                        numberToBuild = (int)Math.Floor(money / 20); //TODO: base numbertobuild on demand 
                        money -= numberToBuild * 20; //lose money
                        main.population.money += numberToBuild * 20;
                        productionVolume += numberToBuild; //add number
                    }
                }
                else //if not enough demand, invest in efficiency
                {
                    if (money > 100 * (1 / productionInput)) //can afford
                    {
                        money -= 100 * (1 / productionInput); //lose money
                        main.population.money += 100 * (1 / productionInput);
                        productionInput *= 0.95;
                    }
                }
            }
            // if not doing well, close factories
            if (info.actualProductionPast5Turns < 0.8 && revenue < 0 && random.Next(0, 100) > 90)
            {
                int amountToClose = (int)Math.Floor((1 - info.actualProductionPast5Turns) * productionVolume) / 2;
                money += amountToClose * 10;
                main.population.money += amountToClose * 10;
                productionVolume -= amountToClose;

            }
        }

        public void DividendPayment()
        {
            if (dividendPercent > 0)//if pays dividends
            {
                foreach (var share in shareList) //
                {
                    if (revenue > 0) main.AIList[main.FindAIID(share.ownerName)].money += revenue * dividendPercent * (share.amount / totalShares);
                }
            }
        }

        public void Bankrupcy()
        {
            foreach (var category in main.resourceSales)
            {
                for (int i = 0; i < category.sales.Count; i++)
                {
                    if (category.sales[i].company == name)
                    {
                        category.sales.Remove(category.sales[i]);
                        i--;
                    }
                }
            }
            main.companyList.RemoveAt(main.FindCompanyID(name));
        }

        #region Production
        public void BuyResources()
        {
            double partToBuy = 1;
            info.costToProduce = 0;
            if (info.producedPast5Turns > 0 && CompanyOutputDemand() < 0.90) partToBuy = info.soldPast5Turns / info.producedPast5Turns; //calculate last turns demand
            if (partToBuy > 1) partToBuy = 1;
            Console.WriteLine("\ncompany demand: "+ CompanyOutputDemand() + " " + this.name); 

            if (main.productionRecipeList[main.FindRecipeID(productionRecipe)].input.Count == 0)//if have no inputs to produce, don't buy resources
            {
                info.actualProduction = partToBuy;
                return;
            }
            double totalAmountBought = 0, totalAmountNeeded = 0, totalPrice = 0;
            foreach (var resource in main.productionRecipeList[main.FindRecipeID(productionRecipe)].input)
            {
                double amountNeeded = resource.amount * productionVolume * productionInput * partToBuy;
                double amountBought = 0;
                List<ResourceSale> sales = main.FindSales(resource.name);//get sales by resource name
                ResourceSale.OrderSalesByPrice(sales);//sort by best price
                foreach (var sale in sales) //browse sales
                {
                    if (amountBought >= amountNeeded) break;
                    if (sale.amount <= amountNeeded - amountBought)//if whole sale not enough to fullfill needs
                    {
                        sale.soldThisTick += sale.amount;
                        amountBought += sale.amount;
                        totalPrice += sale.amount * sale.price;
                        sale.amount = 0;
                    }
                    else
                    {
                        double toBuy = amountNeeded - amountBought;
                        sale.soldThisTick += toBuy;
                        totalPrice += toBuy * sale.price;
                        sale.amount -= toBuy;
                        amountBought += toBuy;
                    }
                }
                totalAmountBought += amountBought;
                totalAmountNeeded += amountNeeded;
            }//browse all required resources for input
            if (totalAmountNeeded > 0) info.actualProduction = (totalAmountBought / totalAmountNeeded) * partToBuy; //0-1, actual company production
            revenue -= totalPrice;
            if (totalAmountBought > 0) info.costToProduce = totalPrice / totalAmountBought;
            else info.costToProduce = totalPrice;
        }
        public void ResourceProduction()
        {
            double labourCosts = 0;
            LabourCosts(ref labourCosts);

            info.totalProduced = 0;
            foreach (var resource in main.productionRecipeList[main.FindRecipeID(productionRecipe)].output)
            {
                double production = resource.amount * productionVolume * productionOutput * info.actualProduction;
                info.totalProduced += production;
                List<ResourceSale> sales = main.FindSales(resource.name);
                bool found = false;
                foreach (var sale in sales)
                {
                    if (sale.company == name)
                    {
                        found = true;
                        //double saleRatio = SaleRatioCalcExperimental(production,sale.soldLastTick);
                        //Console.WriteLine("ratio:{0}", saleRatio);
                        //if(main.FindResource(resource.name).ResourceDemand()<0.9 && production > sale.soldLastTick) sale.price = main.FindResource(resource.name).price * 0.9;
                        if ((production <= sale.soldLastTick) || info.costToProduce > sale.price && main.FindResource(resource.name).ResourceDemand() > 0) sale.price *= (0.01 * main.FindResource(resource.name).ResourceDemand()) + 1;
                        else if (production > sale.soldLastTick && main.FindResource(resource.name).ResourceDemand() < 0.9 && info.costToProduce < sale.price) sale.price *= 0.990;
                        //else if(productionVolume>=1 && sale.soldLastTick<1 && revenue<0) sale.price = Main.changeNumberTowards
                        sale.amount += production;
                        break;
                    }
                }
                if (!found) sales.Add(new ResourceSale(production, main.resourceList[main.FindResourceID(resource.name)].price, name));
            }

        }

        ///Calculates the labour costs and impacts revenue and population income
        public void LabourCosts(ref double labourCosts)
        {
            labourCosts = productionVolume * (info.actualProduction / 2 + 0.5) *
             main.FindRecipe(productionRecipe).labourToProduce * main.population.labourCost;
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
            foreach (var category in main.resourceSales)
            {
                foreach (var sale in category.sales)
                {
                    if (sale.company == name)
                    {
                        revenue += sale.soldThisTick * sale.price;
                        info.totalSold += sale.soldThisTick;
                        Console.WriteLine("{0} {1}", category.name, sale.soldThisTick);
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
            double demandTotal = 0;
            foreach (var resource in main.productionRecipeList[main.FindRecipeID(productionRecipe)].output)
            {
                demandTotal += main.FindResource(resource.name).ResourceDemand();
            }
            return demandTotal / main.FindRecipe(productionRecipe).output.Count;
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
		public double totalSold = 0, totalProduced = 0, actualProduction = 0, costToProduce = 0, producedPast5Turns = 0, soldPast5Turns = 0, actualProductionPast5Turns=0;
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

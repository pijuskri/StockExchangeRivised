using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
    public class Company
    {
        public Main main;
        public string name = "";
        public int totalShares = 0, sharesOwnedByCompany = 0;
        public double money = 0, dividendPercent = 0, revenue = 0, value = 0, sharePrice = 0, productionVolume = 0, productionOutput = 0, productionInput = 0, actualProduction = 0,
            costToProduce = 0, totalSold = 0, totalProduced = 0;
        public string productionRecipe = "";
        public List<Share> shareList;

        public Company(Main main, string name, int totalShares, int sharesOwnedByCompany, double money, double dividendPercent, double revenue, double value, double sharePrice,
            double productionVolume, double actualProduction, double productionOutput, double productionInput, string productionRecipe)
        {
            this.main = main;
            this.name = name;
            this.totalShares = totalShares;
            this.sharesOwnedByCompany = sharesOwnedByCompany;
            this.money = money;
            this.dividendPercent = dividendPercent;
            this.revenue = revenue;
            this.value = value;
            this.sharePrice = sharePrice;
            this.productionVolume = productionVolume;
            this.actualProduction = actualProduction;
            this.productionOutput = productionOutput;
            this.productionInput = productionInput;
            this.productionRecipe = productionRecipe;
            shareList = new List<Share>();

        }

        public void CompanyMoneyMechanics(List<Company> toRemove)
        {
            if (money < -1 * (value) - 100) { toRemove.Add(this); return; }
            if (money > 300) { main.population.money += (money - 300); money = 300; }
            value = productionVolume * 10;//TODO - base value on something more cool
            revenue = 0;

            SellResources();
            BuyResources();
            ResourceProduction();

            sharePrice = value / totalShares; //calculate share price
            money += revenue;

            DividendPayment();
            CompanyInvestment();
        }
        public void CompanyInvestment()
        {
            Random random = new Random(main.randomizerSeed++);
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
                //Console.WriteLine("{0}:{1}",name,CompanyOutputDemand(company));
                if (CompanyOutputDemand() > 0.9 && actualProduction > 0.9) //is there demand
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
            if (actualProduction < 0.8 && revenue < 0 && random.Next(0, 100) > 90)
            {
                int amountToClose = (int)Math.Floor((1 - actualProduction) * productionVolume) / 2;
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
        public void BuyResources() //buy resources, create an actual production metric
        {
            double partToBuy = 1;
            costToProduce = 0;
            if (totalProduced > 0) partToBuy = totalSold / totalProduced; //calculate last turns demand
            //partToBuy += (actualProduction - partToBuy) * 0.1;//normalize compared to last turns production
            if (partToBuy > 1) partToBuy = 1;

            if (main.productionRecipeList[main.FindRecipeID(productionRecipe)].input.Count == 0)//if have no inputs to produce, don't buy resources
            {
                actualProduction = partToBuy;
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
                        sale.soldLastTick += sale.amount;
                        amountBought += sale.amount;
                        totalPrice += sale.amount * sale.price;
                        sale.amount = 0;
                    }
                    else
                    {
                        double toBuy = amountNeeded - amountBought;
                        sale.soldLastTick += toBuy;
                        totalPrice += toBuy * sale.price;
                        sale.amount -= toBuy;
                        amountBought += toBuy;
                    }
                }
                totalAmountBought += amountBought;
                totalAmountNeeded += amountNeeded;
            }//browse all required resources for input
            if (totalAmountNeeded > 0) actualProduction = (totalAmountBought / totalAmountNeeded) * partToBuy; //0-1, actual company production
            revenue -= totalPrice;
            if (totalAmountBought > 0) costToProduce = totalPrice / totalAmountBought;
        }
        public void ResourceProduction()
        {
            double labourCosts = 0;
            LabourCosts(ref labourCosts);

            totalProduced = 0;
            foreach (var resource in main.productionRecipeList[main.FindRecipeID(productionRecipe)].output)
            {
                double production = resource.amount * productionVolume * productionOutput * actualProduction;
                totalProduced += production;
                List<ResourceSale> sales = main.FindSales(resource.name);
                bool found = false;
                foreach (var sale in sales)
                {
                    if (sale.company == name)
                    {
                        found = true;
                        //double saleRatio = SaleRatioCalcExperimental(production,sale.soldLastTick);
                        //Console.WriteLine("ratio:{0}", saleRatio);
                        if (production <= sale.soldLastTick || costToProduce > sale.price) sale.price *= 1.005;
                        if (production > sale.soldLastTick && costToProduce < sale.price) sale.price *= 0.995;
                        sale.amount += production;
                        sale.soldLastTick = 0;
                        break;
                    }
                }
                if (!found) sales.Add(new ResourceSale(production, main.resourceList[main.FindResourceID(resource.name)].price, name));
            }

        }
        public void LabourCosts(ref double labourCosts)
        {
            labourCosts = productionVolume * (actualProduction / 2 + 0.5) *
             main.productionRecipeList[main.FindRecipeID(productionRecipe)].labourToProduce * main.population.labourCost;
            main.population.money += labourCosts;
            revenue -= labourCosts;
            //Console.WriteLine("labour:{0}", labourCosts);

            if (totalProduced > 0) costToProduce += labourCosts / totalProduced;
            else costToProduce += labourCosts;
        }
        public void SellResources()
        {
            totalSold = 0;
            foreach (var category in main.resourceSales)
            {
                foreach (var sale in category.sales)
                {
                    if (sale.company == name)
                    {
                        revenue += sale.soldLastTick * sale.price;
                        totalSold += sale.soldLastTick;
                        Console.WriteLine("{0} {1}", category.name, sale.soldLastTick);
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
        public double CompanyOutputDemand()
        {
            double demandTotal = 0;
            foreach (var resource in main.productionRecipeList[main.FindRecipeID(productionRecipe)].output)
            {
                demandTotal += main.ResourceDemand(resource.name);
            }
            return demandTotal / main.productionRecipeList[main.FindRecipeID(productionRecipe)].output.Count;
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
}

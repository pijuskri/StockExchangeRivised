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
        public int people =100;
        public double money =100, labourCost=1;
        public Population(Instances main, int people,double money, double labourCost)
        {
            this.main = main;
            this.people = people;
            this.money = money;
            this.labourCost = labourCost;
        }
        public void PopulationBuy()
        {
            foreach (var resource in main.populationDemandList) //browse all required resources for input
            {
                double amountNeeded = resource.amount * main.population.people;
                double amountBought = 0;
                List<ResourceSale> sales = main.FindSales(resource.name);//get sales by resource name
                ResourceSale.OrderSalesByPrice(sales);//sort by best price
                foreach (var sale in sales) //browse sales
                {
                    if (sale.price > main.resourceList[main.FindResourceID(resource.name)].basePrice * 5) break;
                    if (amountBought >= amountNeeded) break;
                    if (money < 0) break;
                    double toBuy = amountNeeded - amountBought;
                    if (sale.amount < toBuy) toBuy = sale.amount;

                    sale.soldThisTick += toBuy;
                    sale.amount -= toBuy;
                    money -= toBuy * sale.price;
                    amountBought += toBuy;
                }
            }
        }
        public void PopulationGrowth()
        {
            people += (int)(Math.Sqrt(people) * 0.02) + 1;
        }
    }
    
    public class AI
    {
        Instances main;
        public string name;
        public double money, riskFactor, randomness;

        public AI(Instances main,string name, double money, double riskFactor, double randomness)
        {
            this.main = main;
            this.name = name;
            this.money = money;
            this.riskFactor = riskFactor;
            this.randomness = randomness;
        }
        public void AIMechanics() //AI investments
        {
            Random random = new Random(Instances.randomizerSeed++ * DateTime.MinValue.Millisecond);

            if (random.Next(0, 100) > 90)
            {
                if (money > 10) //buy shares
                {
                    foreach (var companyName in RankCompaniesByInvestability()) // go through best company list
                    {
                        Company company = main.companyList[main.FindCompanyID(companyName)];
                        int shareAmountToBuy = 0;
                        if (company.sharesOwnedByCompany > 0) //if company has shares to sell
                        {
                            if (money > company.sharePrice * company.sharesOwnedByCompany) shareAmountToBuy = company.sharesOwnedByCompany; //can buy all shares
                            else shareAmountToBuy = (int)Math.Floor(money / company.sharePrice); //can only buy some, currently uses all money

                            if (shareAmountToBuy != 0) //can buy any
                            {
                                company.sharesOwnedByCompany -= shareAmountToBuy;
                                money -= company.sharePrice * shareAmountToBuy;
                                company.AddShare(name, shareAmountToBuy);
                            }
                        }
                        else { } // TODO: buy shares from others
                    }
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
    public class Share
    {
        public string ownerName;
        public double amount;

        public Share(string ownerName, double amount)
        {
            this.ownerName = ownerName;
            this.amount = amount;
        }
    }
    public class Bank
    {
        public string name;
        public double interest, money, minimumCredit;

        public Bank(string name, double interest, double money, double minimumCredit)
        {
            this.name = name;
            this.interest = interest;
            this.money = money;
            this.minimumCredit = minimumCredit;
        }
    }
    public class ShareSale
    {
        public string sellerName, company;
        public double amount, price;

        public ShareSale(string sellerName, string company, double amount, double price)
        {
            this.sellerName = sellerName;
            this.company = company;
            this.amount = amount;
            this.price = price;
        }
    }
    public class Loan
    {
        public string company, bank;
        public double totalMoneyLent, interest, timeToPay, moneyPaidBack;

        public Loan(string company, string bank, double totalMoneyLent, double interest, double timeToPay, double moneyPaidBack)
        {
            this.company = company;
            this.bank = bank;
            this.totalMoneyLent = totalMoneyLent;
            this.interest = interest;
            this.timeToPay = timeToPay;
            this.moneyPaidBack = moneyPaidBack;
        }
    }
    /*public class PopulationDemand
    {
        public string name;
        public double amountPerHuman;

        public PopulationDemand(string name, double amountPerHuman)
        {
            this.name = name;
            this.amountPerHuman = amountPerHuman;
        }
    }*/
    public class ProductionRecipe
    {
        public string name="";
        public double labourToProduce=0;
        public List<ResourceAmount> input=new List<ResourceAmount>();
        public List<ResourceAmount> output=new List<ResourceAmount>();

        public ProductionRecipe(string name, double labourToProduce, List<ResourceAmount> input, List<ResourceAmount> output)
        {
            this.name = name;
            this.labourToProduce = labourToProduce;
            this.input = input;
            this.output = output;
        }
    }
    public class ResourceAmount
    {
        public string name;
        public double amount;

        public ResourceAmount(string name, double amount)
        {
            this.name = name;
            this.amount = amount;
        }
    }
    public class HumanPlayer
    {
        public double money;

        public HumanPlayer(double money)
        {
            this.money = money;
        }
    }
    public class ResourceSaleCategory
    {
        public string name;
        public List<ResourceSale> sales;
        public ResourceSaleCategory(string name)
        {
            this.name = name;
            sales = new List<ResourceSale>();
        }
    }
    public class ResourceSale
    {
        public double amount;
        public double price;
        public string company;
        public double soldLastTick=0;
        public double soldThisTick = 0;
        public ResourceSale(double amount, double price, string company)
        {
            this.amount = amount;
            this.price = price;
            this.company = company;
        }
        public static void OrderSalesByPrice(List<ResourceSale> sales)
        {
            sales.Sort(new ResourceSaleComparer());
            /*if (sales.Count <= 2) return;
            for (int i = 0; i < sales.Count; i++)
            {
                for (int d = sales.Count - 2; d > i; d--)
                {
                    if (sales[d].price > sales[d + 1].price)
                    {
                        ResourceSale temp = sales[d];
                        sales[d] = sales[d + 1];
                        sales[d + 1] = temp;
                    }
                }
            }*/
        }
        class ResourceSaleComparer : IComparer<ResourceSale>
        {
            public int Compare(ResourceSale r1, ResourceSale r2)
            {
                if (r1.price > r2.price) return 1;
                else if (r1.price == r2.price) return 0;
                else return -1;
            }
        }
    }
}

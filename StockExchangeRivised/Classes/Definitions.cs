using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{

    
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
		public double costToBuild = 0;
        public List<ResourceAmount> input=new List<ResourceAmount>();
        public List<ResourceAmount> output=new List<ResourceAmount>();

        public ProductionRecipe(string name, double labourToProduce, double costToBuild,List<ResourceAmount> input, List<ResourceAmount> output)
        {
            this.name = name;
            this.labourToProduce = labourToProduce;
			this.costToBuild = costToBuild;
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
    public class ResourceSale
    {
        public double amount=0;
		public double price;
        public string company,resource;
        public double soldLastTick=0;
        public double soldThisTick = 0;
		public double produced = 0;
        public ResourceSale(string resource, double amount, double price, string company)
        {
			this.resource = resource;
            this.amount = amount;
            this.price = price;
            this.company = company;
        }
        public static void OrderSalesByPrice(List<ResourceSale> sales)
        {
            sales.Sort(new ResourceSaleComparer());
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

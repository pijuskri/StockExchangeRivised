using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{


    public class Share
    {
        public AI owner;
        public double amount;

        public Share(AI owner, double amount)
        {
            this.owner = owner;
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
        public Bank bank;
        public double totalMoneyLent, interest, timeToPay, moneyPaidBack;

        public Loan(Bank bank, double totalMoneyLent, double interest, double timeToPay)
        {
            this.bank = bank;
            this.totalMoneyLent = totalMoneyLent;
            this.interest = interest;
            this.timeToPay = timeToPay;
        }
    }
    public class ProductionRecipe
    {
        public string name = "";
        public double labourToProduce = 0;
        public double costToBuild = 0;
        public List<ResourceAmount> input;
        public List<ResourceAmount> output;

        public ProductionRecipe(string name, double labourToProduce, double costToBuild, List<ResourceAmount> input, List<ResourceAmount> output)
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
        public Resource resource;
        public double amount;

        public ResourceAmount(string name, double amount)
        {
            resource = Main.ic.FindResource(name);
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
        public double amount = 0;
        public double price;
        public Company company;
        public Resource resource;
        public double soldLastTick = 0;
        public double soldThisTick = 0;
        public double soldPast5Turns = 0;
        public double produced = 0;
        public double age = 0;
        public List<ResourceOrder> orders;
        public ResourceSale(string resource, double amount, double price, string company)
        {
            this.resource = Main.ic.FindResource(resource);
            this.amount = amount;
            this.price = price;
            this.company = Main.ic.FindCompany(company);
            orders = new List<ResourceOrder>();
        }
        public ResourceSale(Resource resource, double amount, double price, Company company)
        {
            this.resource = resource;
            this.amount = amount;
            this.price = price;
            this.company = company;
            orders = new List<ResourceOrder>();
        }
        public ResourceOrder MakeOrder(double amount, Company orderCompany) {
            ResourceOrder order = new ResourceOrder(this, orderCompany, price, 10, amount);
            //orders.Add(order);
            orderCompany.resourceOrders.Add(order);
            return order;
        }
        public static void OrderSalesByPrice(List<ResourceSale> sales, Location cur)
        {
            sales.Sort(new ResourceSaleComparer(cur));
        }
        class ResourceSaleComparer : IComparer<ResourceSale>
        {
            Location cur;
            public ResourceSaleComparer(Location cur) {
                this.cur = cur;
            }
            public int Compare(ResourceSale r1, ResourceSale r2)
            {
                double p1 = r1.price + r1.resource.price * cur.Distance(r1.company.location) / 100;
                double p2 = r2.price + r1.resource.price * cur.Distance(r2.company.location) / 100;
                if (p1 > p2) return 1;
                else if (p1 == p2) return 0;
                else return -1;
            }
        }
    }
    public class ResourceOrder {
        public ResourceSale sale;
        public Company orderCompany;
        public double price;
        public double timeLeft;
        public double amount;

		public ResourceOrder(ResourceSale sale, Company orderCompany, double price, double timeLeft, double amount)
		{
			this.sale = sale;
			this.orderCompany = orderCompany;
			this.price = price;
			this.timeLeft = timeLeft;
			this.amount = amount;
		}
	}
    public class Location
    {
        public int x, y;
        private static Random random = new Random(Instances.randomizerSeed++ * DateTime.Now.Millisecond);
        /*public Location()
        {
            
        }*/
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public double Distance(Location other)
        {
            return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
        }
        public static Location Random() {
            int x = random.Next(-50, 50);
            int y = random.Next(-50, 50);
            return new Location(x, y);
        }
    }
}

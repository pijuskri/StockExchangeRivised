using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised
{
    public class Company
    {
        public string name;
        public int totalShares, sharesOwnedByCompany;
        public double money, dividendPercent, revenue, value, sharePrice, productionVolume, productionOutput, productionInput, actualProduction;
        public string productionRecipe;
        public List<Share> shareList;

        public Company(string name, int totalShares, int sharesOwnedByCompany, double money, double dividendPercent, double revenue, double value, double sharePrice,
            double productionVolume, double actualProduction, double productionOutput, double productionInput, string productionRecipe)
        {
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
        public void AddShare(string ownerName, int amount)
        {
            foreach (var share in shareList) //check if already on list
            {
                if (share.ownerName == ownerName) { share.amount += amount; return; }
            }
            shareList.Add(new Share(ownerName, amount));
        }
    }
    public class Resource
    {
        public string name, type;
        public double price, amount, costToProduce, basePrice, flexibility, supply, demand;

        public Resource(string name, string type, double price, double amount, double costToProduce, double basePrice, double flexibility, double supply, double demand)
        {
            this.name = name;
            this.type = type;
            this.price = price;
            this.amount = amount;
            this.costToProduce = costToProduce;
            this.basePrice = basePrice;
            this.flexibility = flexibility;
            this.supply = supply;
            this.demand = demand;
        }
    }
    public class AI
    {
        public string name;
        public double money, riskFactor, randomness;

        public AI(string name, double money, double riskFactor, double randomness)
        {
            this.name = name;
            this.money = money;
            this.riskFactor = riskFactor;
            this.randomness = randomness;
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
    public class PopulationDemand
    {
        public string name;
        public double amountPerHuman;

        public PopulationDemand(string name, double amountPerHuman)
        {
            this.name = name;
            this.amountPerHuman = amountPerHuman;
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
    public class ProductionRecipe
    {
        public string name;
        public List<ResourceAmount> input;
        public List<ResourceAmount> output;

        public ProductionRecipe(string name, List<ResourceAmount> input, List<ResourceAmount> output)
        {
            this.name = name;
            this.input = input;
            this.output = output;
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
        public ResourceSale(double amount, double price, string company)
        {
            this.amount = amount;
            this.price = price;
            this.company = company;
        }
        public static void OrderSalesByPrice(List<ResourceSale> sales)
        {
            if (sales.Count <= 2) return;
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
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace StockExchangeRivised
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        string inputFile = "..\\..\\data.txt";

        List<Company> companyList = new List<Company>();
        List<Resource> resourceList = new List<Resource>();
        List<Bank> bankList = new List<Bank>();
        List<Loan> loanList = new List<Loan>();
        List<AI> AIList =  new List<AI>();
        List<Share> shareList = new List<Share>();
        List<ShareSale> shareSaleListing = new List<ShareSale>();
        List<PopulationDemand> populationDemandList = new List<PopulationDemand>();
        List<ProductionRecipe> productionRecipeList = new List<ProductionRecipe>();
        List<ResourceSaleCategory> resourceSales = new List<ResourceSaleCategory>();

        HumanPlayer human = new HumanPlayer(100);
        int population = 100;
        int currentTurn = 0;
        int randomizerSeed = 1;

        void Read()
        {
            using (StreamReader file = new StreamReader(inputFile))
            {
                string line="";
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    companyList.Add(new Company(parts[0], Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToDouble(parts[3]), Convert.ToDouble(parts[4]),
                         Convert.ToDouble(parts[5]), Convert.ToDouble(parts[6]), Convert.ToDouble(parts[7]), Convert.ToDouble(parts[8]), Convert.ToDouble(parts[9]),
                         Convert.ToDouble(parts[10]), Convert.ToDouble(parts[11]), parts[12]));
                }
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    resourceList.Add(new Resource( parts[0], parts[1], Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3]), Convert.ToDouble(parts[4]),
                        Convert.ToDouble(parts[5]), Convert.ToDouble(parts[5]), Convert.ToDouble(parts[6]), Convert.ToDouble(parts[7])));
                }
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    AIList.Add(new AI(parts[0], Convert.ToDouble(parts[1]), Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3])));
                }
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    bankList.Add(new Bank(parts[0], Convert.ToDouble(parts[1]), Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3])));
                }
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    populationDemandList.Add(new PopulationDemand(parts[0], Convert.ToDouble(parts[1])));

                }
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;

                    string[] parts = line.Split(' ');
                    string name = parts[0];
                    List<ResourceAmount> input = new List<ResourceAmount>();
                    List<ResourceAmount> output = new List<ResourceAmount>();

                    int current = 0;
                    for (int i = 1; i < parts.Length; i+=2)
                    {
                        current = i;
                        if (parts[i] == "/") break;
                        input.Add(new ResourceAmount(parts[i], Convert.ToDouble(parts[i+1])));
                    }
                    current++;
                    for (int i = current; i < parts.Length; i+=2)
                    {
                        output.Add(new ResourceAmount(parts[i], Convert.ToDouble(parts[i + 1])));
                    }

                    productionRecipeList.Add(new ProductionRecipe(name, input, output));
                }
            }
            foreach (var resource in resourceList)
            {
                resourceSales.Add(new ResourceSaleCategory(resource.name));
            }
        }

        void NextCalculation()
        {
            population += (int)(Math.Sqrt(population) * 0.05) + 1;

            //SupplyDemandMechanics();
            CompanyMoneyMechanics();
            AIMechanics();
            PopulationBuy();
            ResourcePriceAverage();
            PrintToConsole();
        }
        #region OLD*SupplyDemand*
        /*void SupplyDemandMechanics()
        {
            foreach (var resource in resourceList)
            {
                resource.supply = CalculateSupply(resource.name);
                resource.demand = CalculateDemand(resource.name);

                if (resource.demand > 0 && resource.supply > 0)
                {
                    double supplyDemandRatio = 1 - resource.supply / resource.demand;
                    if (supplyDemandRatio > 0) supplyDemandRatio = 1 / supplyDemandRatio; //if <1, then flip so 0.5 -> 2
                    resource.price += resource.price * supplyDemandRatio / ( RatioBetweenTwoNumbers(resource.basePrice, resource.price)  * 10 )
                        * resource.flexibility * 0.01; //change price based on supplydemandratio and resource flexibility
                }
            }
        }
        double RatioBetweenTwoNumbers(double number1, double number2)
        {
            if (number1 > number2) return number1 / number2;
            else return number2 / number1;
        }
        double CalculateSupply(string resourceName)
        {
            double sum=0;
            foreach (var company in companyList) // parse companies
            {
                foreach (var outputResource in productionRecipeList[FindRecipeID(company.productionRecipe)].output) // parse recipe list for resource
                {
                    if (outputResource.name == resourceName) //if names match
                    {
                       sum += outputResource.amount * company.productionOutput * company.productionVolume; // add to sum, multiply recipe amount by company output and volume
                    }
                }
                
            }
            return sum;
        }
        double CalculateDemand(string resourceName)
        {
            double sum = 0;
            foreach (var company in companyList) // parse companies
            {
                foreach (var inputResource in productionRecipeList[FindRecipeID(company.productionRecipe)].input) // parse recipe list for resource
                {
                    if (inputResource.name == resourceName) //if names match
                    {
                        sum += inputResource.amount * company.productionInput * company.productionVolume; // add to sum, multiply recipe amount by company input and volume
                    }
                }

            }
            foreach (var populationDemand in populationDemandList)
            {
                if (populationDemand.name == resourceName) sum += populationDemand.amountPerHuman * population;
            }
            return sum;
        }
        double RecipeSupplyDemandRatio(ProductionRecipe recipe)
        {
            double ratio = 0;
            double supplySum=0;
            double demandSum=0;
            foreach (var output in recipe.output)
            {
                Resource resource = resourceList[FindResourceID(output.name)];
                supplySum += resource.supply * output.amount;
                demandSum += resource.demand * output.amount;
            }
            ratio = supplySum / demandSum;

            return ratio;
        }
        */
        #endregion
        #region CompanyMechanics
        void CompanyMoneyMechanics()
        {
            foreach (var company in companyList)
            {
                company.value = company.productionVolume * 10;//TODO - base value on something more cool
                company.revenue = 0;

                SellResources(company);
                BuyResources(company);
                ResourceProduction(company);

                company.sharePrice = company.value / company.totalShares; //calculate share price
                company.money += company.revenue;

                if (company.dividendPercent > 0)//if pays dividends
                {
                    foreach (var share in company.shareList) //
                    {
                        if (company.revenue>0) AIList[ FindAIID( share.ownerName)].money += company.revenue * company.dividendPercent * (share.amount/company.totalShares);
                    }
                }

                CompanyInvestment(company);
            }
        }
        void CompanyInvestment(Company company)
        {
            Random random = new Random(randomizerSeed++);
            if (random.Next(0, 100) > 95) //make some shares if price too big
            {
                if (company.sharePrice > 3)
                {
                    IssueShares(company, Convert.ToInt32(company.value / 2));
                }
            }
            // chose best money raising option
            if (company.money < 0 && company.revenue < 0)
            {
                if (random.Next(0,100)>95 && company.sharesOwnedByCompany==0)
                {
                    IssueShares(company, Convert.ToInt32(company.value / 2)); //TODO - issue shares more logically
                }
                else if (false) //TODO: loans
                {

                }

            }
            //if doing well, expand
            else if (company.money > 0 && company.revenue > 0)
            {
                if (random.Next(0,100)>90) //randomizer
                {
                    Console.WriteLine(CompanyOutputDemand(company));
                    if (CompanyOutputDemand(company)>0.9) //is there demand
                    {
                        if (company.money > 20) //can afford
                        {
                            int numberToBuild = 0;
                            numberToBuild = (int)Math.Floor(company.money / 20); //TODO: base numbertobuild on demand 
                            company.money -= numberToBuild * 20; //lose money
                            company.productionVolume += numberToBuild; //add number
                        }
                    }
                    else //if not enough demand, invest in efficiency
                    {
                        if (company.money > 100 * (1 / company.productionInput)) //can afford
                        {
                            company.money -= 100 * (1 / company.productionInput); //lose money
                            company.productionInput *= 0.95; 
                        }
                    }
                }
            }
        }
        void IssueShares(Company company, int number)
        {
            company.totalShares += number;
            company.sharesOwnedByCompany += number;
            company.sharePrice = company.totalShares / company.value;
            company.money += company.sharePrice * number;
        }
        #region NewProduction
        void BuyResources(Company company) //buy resources, create an actual production metric
        {
            if (productionRecipeList[FindRecipeID(company.productionRecipe)].input.Count == 0)
            {
                company.actualProduction = 1;
                return;
            }
            double totalAmountBought = 0;
            double totalAmountNeeded = 0;
            double totalPrice = 0;
            foreach (var resource in productionRecipeList[FindRecipeID( company.productionRecipe)].input) //brose all required resources for input
            {
                double amountNeeded = resource.amount * company.productionVolume * company.productionInput;
                double amountBought=0;
                List<ResourceSale> sales = FindSales(resource.name);//get sales by resource name
                ResourceSale.OrderSalesByPrice(sales);//sort by best price
                foreach (var sale in sales) //browse sales
                {
                    if (amountBought >= amountNeeded) break;
                    if (sale.amount <= amountNeeded - amountBought)
                    {
                        sale.soldLastTick += sale.amount;
                        amountBought += sale.amount;
                        totalPrice += sale.amount*sale.price;
                        sale.amount = 0;
                    }
                    else
                    {
                        sale.soldLastTick += amountNeeded - amountBought;
                        totalPrice += (amountNeeded - amountBought) * sale.price;
                        sale.amount -= amountNeeded - amountBought;
                        amountBought += amountNeeded - amountBought;
                    }
                }
                totalAmountBought += amountBought;
                totalAmountNeeded += amountNeeded;
            }
            company.actualProduction = totalAmountBought / totalAmountNeeded; //0-1, actual company production
            company.revenue -= totalPrice;
        }
        void ResourceProduction(Company company)
        {
            foreach (var resource in productionRecipeList[FindRecipeID(company.productionRecipe)].output)
            {
                double production = resource.amount * company.productionVolume * company.productionOutput * company.actualProduction;
                List<ResourceSale> sales = FindSales(resource.name);
                bool found = false;
                foreach (var sale in sales)
                {
                    if (sale.company==company.name)
                    {
                        found = true;
                        if (sale.soldLastTick > sale.amount * 10) sale.price = sale.price * 1.01;
                        else if (sale.soldLastTick < sale.amount * 5) sale.price = sale.price * 0.99;
                        sale.amount += production;
                    }
                }
                if (!found) sales.Add(new ResourceSale(production, resourceList[FindResourceID(resource.name)].price, company.name));
            }
        }
        void SellResources(Company company)
        {
            foreach (var category in resourceSales)
            {
                foreach (var sale in category.sales)
                {
                    if (sale.company == company.name)
                    {
                        company.revenue += sale.soldLastTick * sale.price;
                        Console.WriteLine("{0} {1}",category.name, sale.soldLastTick);
                        sale.soldLastTick = 0;
                    }
                }
            }
        }
        #endregion
        /* old production mechanics
        double CompanyInputDemandMet(List<ResourceAmount> inputList)
        {
            if (inputList.Count==0) //check if not a natural resource producer
            {
                return 1;
            }

            double demandMet = 0; //ratio of demand met
            double totalDemand = 0; //total demand
            double totalDemandMet = 0; //total demand met
            foreach (var inputResource in inputList)
            {
                totalDemand += inputResource.amount;
                Resource resourceReference = resourceList[FindResourceID(inputResource.name)]; //reference to resource list
                if (resourceReference.supply >= resourceReference.demand) totalDemandMet += inputResource.amount; //if supply more tahn demand
                else if (resourceReference.supply + resourceReference.amount >= resourceReference.demand) totalDemandMet += inputResource.amount; //if need to use stocked resource
                else totalDemandMet += inputResource.amount * resourceReference.supply / resourceReference.demand; //else multiply by supplydemand ratio
            }
            demandMet = totalDemandMet / totalDemand;
            return demandMet;
        }
        double MoneyFromRecipe(ProductionRecipe recipe)
        {
            double balance=0, expenses=0, income=0;
            foreach (var input in recipe.input)
            {
                expenses += resourceList[FindResourceID(input.name)].price * input.amount;
            }
            foreach (var output in recipe.output)
            {
                income +=resourceList[ FindResourceID( output.name)].price * output.amount;
            }
            balance = income - expenses;
            return balance;
        }*/

        #endregion
        #region AIMechanics
        void AIMechanics() //baseMechnics
        {
            Random random = new Random(randomizerSeed++);
            foreach (var human in AIList)
            {
                if (random.Next(0, 100) > 90)
                {
                    if (human.money > 10) //buy shares
                    {
                        foreach (var companyName in RankCompaniesByInvestability()) // go through best company list
                        {
                            Company company = companyList[ FindCompanyID( companyName)];
                            int shareAmountToBuy = 0;
                            if (company.sharesOwnedByCompany > 0) //if company has shares to sell
                            {
                                if (human.money > company.sharePrice * company.sharesOwnedByCompany) shareAmountToBuy = company.sharesOwnedByCompany; //can buy all shares
                                else shareAmountToBuy = (int)Math.Floor(human.money / company.sharePrice); //can only buy some, currently uses all money

                                if (shareAmountToBuy != 0) //can buy any
                                {
                                    company.sharesOwnedByCompany -= shareAmountToBuy;
                                    human.money -= company.sharePrice * shareAmountToBuy;
                                    company.AddShare(human.name, shareAmountToBuy);
                                }
                            }
                            else { } // TODO: buy shares from others
                        }
                    }
                }
            }
        }
        List <string> RankCompaniesByInvestability() //based on point system, creates list based on 
        {
            List <string> nameList = new List<string>();
            List<double> pointList = new List<double>();
            foreach (var company in companyList)
            {
                double points = 0;
                points += company.value / 10; //favour big companies
                points += company.dividendPercent * 3;
                if(company.revenue>0)points += company.revenue / company.value; //focus on efficiency
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
        #endregion
        #region PopulationMechanics
        void PopulationBuy()
        {
            foreach (var resource in populationDemandList) //brose all required resources for input
            {
                double amountNeeded = resource.amountPerHuman * population;
                double amountBought = 0;
                List<ResourceSale> sales = FindSales(resource.name);//get sales by resource name
                ResourceSale.OrderSalesByPrice(sales);//sort by best price
                foreach (var sale in sales) //browse sales
                {
                    if (sale.price > resourceList[FindResourceID(resource.name)].basePrice * 5) break;
                    if (amountBought >= amountNeeded) break;
                    if (sale.amount <= amountNeeded - amountBought)
                    {
                        sale.soldLastTick += sale.amount;
                        amountBought += sale.amount;
                        sale.amount = 0;
                    }
                    else
                    {
                        sale.soldLastTick += amountNeeded - amountBought;
                        sale.amount -= amountNeeded - amountBought;
                        amountBought += amountNeeded - amountBought;
                    }
                }
            }
        }
        #endregion
        #region FindID
        int FindResourceID(string name)
        {
            for (int i = 0; i < resourceList.Count; i++)
            {
                if (resourceList[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        int FindRecipeID(string name)
        {
            for (int i = 0; i < productionRecipeList.Count; i++)
            {
                if (productionRecipeList[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        int FindCompanyID(string name)
        {
            for (int i = 0; i < companyList.Count; i++)
            {
                if (companyList[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        int FindAIID(string name)
        {
            for (int i = 0; i < AIList.Count; i++)
            {
                if (AIList[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
        #region Misc
        List<ResourceSale> FindSales(string name)//returns list of sales of specified resource
        {
            foreach (var saleCategory in resourceSales)
            {
                if (name == saleCategory.name) return saleCategory.sales;
            }
            return null;
        }
        void ResourcePriceAverage()//find resource price average across sales
        {
            foreach (var resource in resourceList)
            {
                List<ResourceSale> sales = FindSales(resource.name);
                double amount = 0;
                double cost = 0;
                foreach (var sale in sales)
                {
                    amount += sale.amount + sale.soldLastTick;
                    cost += (sale.amount + sale.soldLastTick) * sale.price;
                }
                resource.price = cost / amount;
            }
        }
        double ResourceDemand(string name)//Returns total resource sold/total amount
        {
            List<ResourceSale> sales = FindSales(name);
            double totalAmount = 0;
            double totalSold = 0;
            foreach (var sale in sales)
            {
                totalAmount += sale.amount + sale.soldLastTick;
                totalSold += sale.amount;
            }
            return totalSold / totalAmount;
        }
        double CompanyOutputDemand(Company company)
        {
            double demandTotal = 0;
            foreach (var resource in productionRecipeList[FindRecipeID(company.productionRecipe)].output)
            {
                demandTotal+= ResourceDemand(resource.name);
            }
            return demandTotal / productionRecipeList[FindRecipeID(company.productionRecipe)].output.Count;
        }
        #endregion

        void PrintToConsole()
        {
            //Console.Clear();
            Console.WriteLine("///////Turn:{0}////////", currentTurn++);
            foreach (var resource in resourceList)
            {
                Console.WriteLine("{0} -price:{1:F3}, supplyDemand:{2:F}", resource.name, resource.price, resource.supply / resource.demand);
            }
            Console.WriteLine();
            foreach (var company in companyList)
            {
                Console.WriteLine("{0} - revenue:{1:F}, money:{2:F}, value:{3:F}", company.name, company.revenue, company.money, company.value);
            }
            Console.WriteLine();
            foreach (var human in AIList)
            {
                Console.WriteLine("{0} - money:{1:F}", human.name, human.money);
            }
            Console.WriteLine("{0}", population);
            Console.WriteLine();
        }

        public Form1()
        {
            InitializeComponent();
            Read();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
        }

        private void nextTurn_Click(object sender, EventArgs e)
        {
            NextCalculation();
        }
        
    }
}

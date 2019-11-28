using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace StockExchangeRivised
{
    public class Instances
    {
        public Population population;
        public List<Company> companyList;
        public List<Resource> resourceList;
        public List<Bank> bankList;
        public List<Loan> loanList;
        public List<AI> AIList;
        public List<ShareSale> shareSaleListing;
        public List<ResourceAmount> populationDemandList;
        public List<ProductionRecipe> productionRecipeList;
        
        public HumanPlayer human;
        public static int currentTurn = 0;
        public static int randomizerSeed = 1;
        public Instances()
        {
            companyList = new List<Company>();
            resourceList = new List<Resource>();
            bankList = new List<Bank>();
            loanList = new List<Loan>();
            AIList = new List<AI>();
            shareSaleListing = new List<ShareSale>();
            populationDemandList = new List<ResourceAmount>();
            productionRecipeList = new List<ProductionRecipe>();
            human = new HumanPlayer(100);
        }
        public void NextCalculation()
        {
            population.PopulationGrowth();
			population.PopulationBuy();
			population.CreateNewAi();
			population.CalculateInflation5();
			CompanyCalc();

            foreach (var human in AIList)
            {
                human.AIMechanics();
            }
            // population.PopulationBuy();
            foreach (var resource in resourceList)
            {
                resource.ResourcePriceAverage();
            }
			population.Wages();

        }
        public void CompanyCalc()
        {
            List<Company> toRemove = new List<Company>();
            foreach (var company in companyList)
            {
                company.CompanyMoneyMechanics(toRemove);

            }
            foreach (var company in toRemove)
            {
                company.Bankrupcy();
            }
        }
        public void Read()
        {

            using (StreamReader file = new StreamReader(Main.inputFile))
            {
                string line = "";
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    /*companyList.Add(new Company(parts[0], Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToDouble(parts[3]), Convert.ToDouble(parts[4]),
                         Convert.ToDouble(parts[5]), Convert.ToDouble(parts[6]), Convert.ToDouble(parts[7]), Convert.ToDouble(parts[8]), Convert.ToDouble(parts[9]),
                         Convert.ToDouble(parts[10]), Convert.ToDouble(parts[11]), parts[12]));*/
                }
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    // resourceList.Add(new Resource( parts[0], parts[1], Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3]), Convert.ToDouble(parts[4]),
                    //    Convert.ToDouble(parts[5]), Convert.ToDouble(parts[5]), Convert.ToDouble(parts[6]), Convert.ToDouble(parts[7])));
                }
                while (true)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    AIList.Add(new AI(this, parts[0], Convert.ToDouble(parts[1]), Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3])));
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
                    populationDemandList.Add(new ResourceAmount(parts[0], Convert.ToDouble(parts[1])));

                }
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    if (line == "") break;
                    if (line[0] == '#') continue;

                    /*string[] parts = line.Split(' ');
                    string name = parts[0];
                    double labour = Convert.ToDouble(parts[1]);
                    List<ResourceAmount> input = new List<ResourceAmount>();
                    List<ResourceAmount> output = new List<ResourceAmount>();

                    int current = 0;
                    for (int i = 2; i < parts.Length; i+=2)
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

                    productionRecipeList.Add(new ProductionRecipe(name, labour, input, output));
                    */
                }
            }

            string resourceJson = File.ReadAllText(Main.fileResources);
			resourceList = JsonConvert.DeserializeObject<List<Resource>>(resourceJson);
            foreach (var resource in resourceList)
            {
                resource.main = this;
                resource.price = resource.basePrice;
				resource.sales = new List<ResourceSale>();
            }

            string companyJson = File.ReadAllText(Main.fileCompanies);
            companyList = JsonConvert.DeserializeObject<List<Company>>(companyJson);

            foreach (var company in companyList)
            {
                company.main = this;
                company.sharesOwnedByCompany = company.totalShares;
                company.shareList = new List<Share>();
            }

            string recipeJson = File.ReadAllText(Main.fileRecipes);
            productionRecipeList = JsonConvert.DeserializeObject<List<ProductionRecipe>>(recipeJson);


        }
        #region FindID
        public int FindResourceID(string name)
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
        public int FindRecipeID(string name)
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
        public int FindCompanyID(string name)
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
        public int FindAIID(string name)
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

        public Resource FindResource(string name)
        {
            for (int i = 0; i < resourceList.Count; i++)
            {
                if (resourceList[i].name == name)
                {
                    return resourceList[i];
                }
            }
            return null;
        }
        public ProductionRecipe FindRecipe(string name)
        {
            for (int i = 0; i < productionRecipeList.Count; i++)
            {
                if (productionRecipeList[i].name == name)
                {
                    return productionRecipeList[i];
                }
            }
            return null;
        }
        public Company FindCompany(string name)
        {
            for (int i = 0; i < companyList.Count; i++)
            {
                if (companyList[i].name == name)
                {
                    return companyList[i];
                }
            }
            return null;
        }
        public AI FindAI(string name)
        {
            for (int i = 0; i < AIList.Count; i++)
            {
                if (AIList[i].name == name)
                {
                    return AIList[i];
                }
            }
            return null;
        }
        #endregion
        #region Misc
		public List<ResourceSale> FindSalesCompany(string name)//returns list of sales of specified resource
		{
			List<ResourceSale> sales = new List<ResourceSale>();
			foreach (var resource in resourceList)
			{
				foreach (var sale in resource.sales)
				{
					if (name == sale.company) sales.Add(sale);
				}
			}
			return sales;
		}
		/// <summary>
		/// Calculates total amoount of resource on sale
		/// </summary>
		/// <param name="name">name of resource</param>
		/// <returns></returns>
		public double TotalOnSale(string name)
		{
			double totalAmount = 0;
			foreach (var sale in FindResource(name).sales)
			{
				totalAmount += sale.amount;
			}
			return totalAmount;
		}

		public static double changeNumberTowards(double input, double goal, double percent)
        {
            double output = 0;
            if (input < goal) output = input + (goal - input) * percent;
            if (input > goal) output = input - (input - goal) * percent;
            return output;
        }
        public static void changeNumberTowards(ref double input, double goal, double percent)
        {
            double output = 0;
            if (input < goal) output = input + (goal - input) * percent;
            if (input > goal) output = input - (input - goal) * percent;
            input = output;
        }
        #endregion

    }
}

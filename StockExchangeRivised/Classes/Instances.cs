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
			population.CalculateLongtermInflation();
			CompanyCalc();

            foreach (var human in AIList)
            {
                human.AIMechanics();
            }
            // population.PopulationBuy();
            foreach (var resource in resourceList)
            {
                resource.ResourcePriceAverage();
                resource.CalculateInflation();
            }
			population.Wages();
            currentTurn++;
        }
        public void CompanyCalc()
        {
            List<Company> toRemove = new List<Company>();
            foreach (var company in MyMath.Shuffle(companyList))
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

	            string resourceJson = File.ReadAllText(Main.fileResources);
	            resourceList = JsonConvert.DeserializeObject<List<Resource>>(resourceJson);
	            
	            string recipeJson = File.ReadAllText(Main.fileRecipes);
	            productionRecipeList = JsonConvert.DeserializeObject<List<ProductionRecipe>>(recipeJson);

	            string companyJson = File.ReadAllText(Main.fileCompanies);
	            companyList = JsonConvert.DeserializeObject<List<Company>>(companyJson);


	            string line = "";
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
                    if (string.IsNullOrEmpty(line)) break;
                    if (line[0] == '#') continue;
                    string[] parts = line.Split(' ');
                    populationDemandList.Add(new ResourceAmount(parts[0], Convert.ToDouble(parts[1])));

                }
            }

            


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
					if (name == sale.company.name) sales.Add(sale);
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

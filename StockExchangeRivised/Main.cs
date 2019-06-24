using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace StockExchangeRivised
{
    public partial class Main : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        string inputFile = "..\\..\\data.txt";
        string fileResources = "..\\..\\Resources.json";
        string fileCompanies = "..\\..\\Companies.json";
        string fileRecipes = "..\\..\\Recipes.json";

        public Population population;
        public List<Company> companyList = new List<Company>();
        public List<Resource> resourceList = new List<Resource>();
        public List<Bank> bankList = new List<Bank>();
        public List<Loan> loanList = new List<Loan>();
        public List<AI> AIList =  new List<AI>();
        public List<ShareSale> shareSaleListing = new List<ShareSale>();
        public List<PopulationDemand> populationDemandList = new List<PopulationDemand>();
        public List<ProductionRecipe> productionRecipeList = new List<ProductionRecipe>();
        public List<ResourceSaleCategory> resourceSales = new List<ResourceSaleCategory>();
        HumanPlayer human = new HumanPlayer(100);

        public int currentTurn = 0;
        public int randomizerSeed = 1;

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
                    AIList.Add(new AI(this,parts[0], Convert.ToDouble(parts[1]), Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3])));
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

            string resourceJson = File.ReadAllText(fileResources);
            resourceList = JsonConvert.DeserializeObject<List<Resource>>(resourceJson);
            foreach (var resource in resourceList)
            {
                resource.main = this;
                resource.price = resource.basePrice;
                resourceSales.Add(new ResourceSaleCategory(resource.name));
            }

            string companyJson = File.ReadAllText(fileCompanies);
            companyList = JsonConvert.DeserializeObject<List<Company>>(companyJson);

            foreach (var company in companyList)
            {
                company.main = this;
                company.sharesOwnedByCompany = company.totalShares;
                company.shareList = new List<Share>();
            }

            string recipeJson = File.ReadAllText(fileRecipes);
            productionRecipeList = JsonConvert.DeserializeObject<List<ProductionRecipe>>(recipeJson);

            
        }
        void Initialize()
        {
            population = new Population(this, 100, 1000, 0.1);
        }

        void NextCalculation()
        {
            population.PopulationGrowth();
            CompanyCalc();

            foreach (var human in AIList)
            {
                human.AIMechanics();
            }
            population.PopulationBuy();
            foreach (var resource in resourceList)
            {
                resource.ResourcePriceAverage();
            }
            
        }
        void CompanyCalc()
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
        #endregion
        #region Misc
        public List<ResourceSale> FindSales(string name)//returns list of sales of specified resource
        {
            foreach (var saleCategory in resourceSales)
            {
                if (name == saleCategory.name) return saleCategory.sales;
            }
            return null;
        }
        
        public double ResourceDemand(string name)//Returns total resource sold/total amount
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
        
        /*double changeNumberTowards(double input,double goal,double percent)
        {
            double output = 0;
            if (input < goal) output = input + Math.Abs(goal - input) * percent;
        }*/
        #endregion

        #region UI
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
            Console.WriteLine("pop:{0}, money:{1}", population.people, population.money);
            Console.WriteLine();
        }
        void PopulateCompanyTable()
        {
            //Console.WriteLine(CompanyTable.Rows.Count);
            while (CompanyTable.Rows.Count > 1)
            {
                CompanyTable.Rows.RemoveAt(0);
            }

            try
            {
                foreach (var company in companyList)
                {
                    int index = CompanyTable.Rows.Add();
                    DataGridViewRow row = CompanyTable.Rows[index];
                    row.Cells[0].Value = company.name;
                    row.Cells[1].Value = Math.Round(company.money, 3);
                    row.Cells[2].Value = Math.Round(company.revenue, 3);
                    row.Cells[3].Value = Math.Round(company.value, 3);
                    row.Cells[4].Value = company.productionVolume;
                    row.Cells[5].Value = company.productionOutput;
                    row.Cells[6].Value = company.productionInput;
                    row.Cells[7].Value = Math.Round(company.actualProduction, 3) * 100 + "%";
                    row.Cells[8].Value = company.productionRecipe;
                    row.Cells[9].Value = company.costToProduce;
                }
            }
            catch { }
        }
        void PopulateResourceTable()
        {
            while (ResourceTable.Rows.Count > 1)
            {
                ResourceTable.Rows.RemoveAt(0);
            }

            try
            {
                foreach (var resource in resourceList)
                {
                    int index = ResourceTable.Rows.Add();
                    DataGridViewRow row = ResourceTable.Rows[index];
                    row.Cells[0].Value = resource.name;
                    row.Cells[1].Value = resource.type;
                    row.Cells[2].Value = Math.Round(resource.price, 3);
                    row.Cells[3].Value = Math.Round(resource.basePrice, 3);

                }
            }
            catch { }
        }
        #endregion
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;
        List<SaleWindow> saleWindows = new List<SaleWindow>();

        public Main()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            Initialize();
            Read();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
            PopulateCompanyTable();
            PopulateResourceTable();
            PrintToConsole();
        }

        private void nextTurn_Click(object sender, EventArgs e)
        {
            NextCalculation();
            PopulateCompanyTable();
            PopulateResourceTable();
            PrintToConsole();
            
        }

        async private void Next10Turns_Click(object sender, EventArgs e)
        {
            await Task.Run(() => 
            {
                for (int i = 0; i < 10; i++)
                {
                    NextCalculation();
                    UpdateUI();
                    //Thread.Sleep(100);
                }
            });
            PrintToConsole();
        }
        
        public void UpdateUI()
        {
            var timeNow = DateTime.Now;

            if ((DateTime.Now - previousTime).Milliseconds <= 50) return;

            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                PopulateCompanyTable();
                PopulateResourceTable();
            }), value);

            previousTime = timeNow;
        }

        private void ResourceTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ResourceTable.RowCount > 0)
            {
                string name = (string)ResourceTable.Rows[e.RowIndex].Cells[0].Value;
                List< ResourceSale> sales = FindSales(name);
                SaleWindow saleWindow = new SaleWindow(name,sales);
                saleWindow.Show();
                saleWindows.Add(saleWindow);
            }
        }
    }
}

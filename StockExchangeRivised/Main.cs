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

        public static string inputFile = "..\\..\\data.txt";
        public static string fileResources = "..\\..\\JSON\\Resources.json";
        public static string fileCompanies = "..\\..\\JSON\\Companies.json";
        public static string fileRecipes = "..\\..\\JSON\\Recipes.json";

        public Instances ic;


        #region UI
        void PrintToConsole()
        {
            //Console.Clear();
            Console.WriteLine("///////Turn:{0}////////", Instances.currentTurn++);
            /*foreach (var resource in ic.resourceList)
            {
                Console.WriteLine("{0} -price:{1:F3}, supplyDemand:{2:F}", resource.name, resource.price, resource.supply / resource.demand);
            }
            Console.WriteLine();
            foreach (var company in ic.companyList)
            {
                Console.WriteLine("{0} - revenue:{1:F}, money:{2:F}, value:{3:F}, actualproduction5:{4:F}", company.name, company.revenue, company.money, company.value, company.info.actualProductionPast5Turns);
            }
            Console.WriteLine();
            foreach (var human in ic.AIList)
            {
                Console.WriteLine("{0} - money:{1:F}", human.name, human.money);
            }
			*/
            Console.WriteLine("pop:{0}, money:{1}", ic.population.people, ic.population.money);
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
                foreach (var company in ic.companyList)
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
                    row.Cells[7].Value = Math.Round(company.info.actualProduction, 3) * 100 + "%";
                    row.Cells[8].Value = company.productionRecipe;
                    row.Cells[9].Value = company.info.costToProduce;
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
                foreach (var resource in ic.resourceList)
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
		void PopulatePeopleTable()
		{
			while (PeopleTable.Rows.Count > 1)
			{
				PeopleTable.Rows.RemoveAt(0);
			}

			try
			{
				foreach (var person in ic.AIList)
				{
					int index = PeopleTable.Rows.Add();
					DataGridViewRow row = PeopleTable.Rows[index];
					row.Cells[0].Value = person.name;
					row.Cells[1].Value = Math.Round(person.money, 1);

				}
			}
			catch { }
		}
		void PopulateShareSaleTable()
		{
			while (ShareSaleTable.Rows.Count > 1)
			{
				ShareSaleTable.Rows.RemoveAt(0);
			}

			try
			{
				foreach (var sale in ic.shareSaleListing)
				{
					int index = ShareSaleTable.Rows.Add();
					DataGridViewRow row = ShareSaleTable.Rows[index];
					row.Cells[0].Value = sale.sellerName;
					row.Cells[1].Value = sale.company;
					row.Cells[2].Value = Math.Round(sale.amount, 1);
					row.Cells[3].Value = Math.Round(sale.price, 3);
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
            ic.Read();
			
        }
        void Initialize()
        {
            ic = new Instances();
            ic.population = new Population(ic, 100, 1000, 0.1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
			UpdateUI();
		}

        private void nextTurn_Click(object sender, EventArgs e)
        {
            ic.NextCalculation();
			UpdateUI();
            
        }

        async private void Next10Turns_Click(object sender, EventArgs e)
        {
            await Task.Run(() => 
            {
				lock (this)
				{
					for (int i = 0; i < 100; i++)
					{
						ic.NextCalculation();
						
						//Thread.Sleep(35);
					}
					UpdateUI();
					
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
				PopulatePeopleTable();
				if (ShareSaleTable.Visible) PopulateShareSaleTable();
				LabourCost.Text = ""+ Math.Round(ic.population.labourCost,3);
				PrintToConsole();
				
            }), value);

            previousTime = timeNow;
        }

        private void ResourceTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ResourceTable.RowCount > 0)
            {
                string name = (string)ResourceTable.Rows[e.RowIndex].Cells[0].Value;
                List< ResourceSale> sales = ic.FindResource(name).sales;
                SaleWindow saleWindow = new SaleWindow(name,sales);
                saleWindow.Show();
                saleWindows.Add(saleWindow);
            }
        }
    }
}

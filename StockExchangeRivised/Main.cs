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
using System.Linq;

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

        public static Instances ic;


        #region UI
        void PrintToConsole()
        {
            //Console.Clear();
            Console.WriteLine("///////Turn:{0}////////", Instances.currentTurn);
            Console.WriteLine("pop:{0}, money:{1}, spent:{2}", ic.population.people, ic.population.money, ic.population.spentLast);
            Console.WriteLine("inflation:{0}", ic.population.inflation25);
            Console.WriteLine();
        }
        void PopulateCompanyTable()
        {
            //Console.WriteLine(CompanyTable.Rows.Count);
            /*  while (CompanyTable.Rows.Count > 1)
              {
                  CompanyTable.Rows.RemoveAt(0);
              }*/

            try
            {
                for (int i = 0; i < ic.companyList.Count; i++)
                {
                    int index = i;
                    if (CompanyTable.RowCount - 1 <= i) index = CompanyTable.Rows.Add();
                    DataGridViewRow row = CompanyTable.Rows[index];
                    Company company = ic.companyList[i];
                    row.Cells[0].Value = company.name;
                    row.Cells[1].Value = Math.Round(company.money, 2);
                    row.Cells[2].Value = Math.Round(company.revenue, 3);
                    row.Cells[3].Value = Math.Round(company.value, 1);
                    row.Cells[4].Value = company.productionVolume;
                    row.Cells[5].Value = company.productionOutput;
                    row.Cells[6].Value = company.productionInput;
                    row.Cells[7].Value = Math.Round(company.info.actualProduction, 3) * 100 + "%";
                    row.Cells[8].Value = company.productionRecipe.name;
                    row.Cells[9].Value = Math.Round(company.info.costToProduce, 2);
                }
                for (int i = ic.companyList.Count; i < CompanyTable.RowCount - 1; i++)
                {
                    CompanyTable.Rows.RemoveAt(i);
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
        List<ChartData> chartDataPoints = new List<ChartData>();
        ChartType chartType = ChartType.CompanyValue;
        struct ChartData {
            public int time;
            public double gdp;
            public double marketValue;

			public ChartData(int time, double gdp, double marketValue)
			{
				this.time = time;
				this.gdp = gdp;
				this.marketValue = marketValue;
			}
		}
        int maxTimeGap = 500;
        public enum ChartType
        {
            GDP,
            CompanyValue
        }
        void UpdateChart() {
            chartDataPoints.Add(new ChartData(Instances.currentTurn, ic.companyList.Sum(x => x.productionVolume), ic.companyList.Sum(x => x.value)));
            DrawChart();
        }
        int windowIndex = 0;
        void DrawChart() {
            List<ChartData> dataWindow = new List<ChartData>(chartDataPoints);
            var timespots = chartDataPoints.Select(x => x.time).ToList().GetRange(windowIndex, chartDataPoints.Count - windowIndex);
            if (timespots.Count > 0 && timespots[timespots.Count - 1] - timespots[0] > maxTimeGap) {
                windowIndex += timespots.FindLastIndex(x => timespots[timespots.Count - 1] - x > maxTimeGap);
                dataWindow = chartDataPoints.GetRange(windowIndex, chartDataPoints.Count - windowIndex);
            }
           

            chart1.Series["Series1"].Points.DataBindXY(dataWindow.Select(x => x.time).ToList(), dataWindow.Select(x => x.gdp).ToList());
            chart1.ChartAreas[0].AxisY.Title = chartType.ToString();
            switch (chartType) {
                case ChartType.GDP:
                    chart1.Series["Series1"].Points.DataBindXY(dataWindow.Select(x => x.time).ToList(), dataWindow.Select(x => x.gdp).ToList());
                    break;
                case ChartType.CompanyValue:
                    chart1.Series["Series1"].Points.DataBindXY(dataWindow.Select(x => x.time).ToList(), dataWindow.Select(x => x.marketValue).ToList());
                    break;
            }
            List<double> dataY = chart1.Series["Series1"].Points.Select(x => x.YValues[0]).ToList();
            //if(dataY.Count>0)chart1.ChartAreas[0].AxisY.Minimum = (int)MyMath.Clamp(dataY[0] - dataY[0] / 6, 0, dataY[0]);
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
            ChartSelection.DataSource = Enum.GetValues(typeof(ChartType));
            //ChartSelection.SelectedValue = chartType.ToString();
        }
        void Initialize()
        {
            ic = new Instances();
            ic.population = new Population(ic, 100, 3000, 0.1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
            BankTable.CellValueNeeded += BankTable_CellValueNeeded;
            PeopleTable.CellValueNeeded += PeopleTable_CellValueNeeded;
            
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
					for (int i = 0; i < 30; i++)
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

                PeopleTable.RowCount = ic.AIList.Count + 1;
                PeopleTable.Invalidate();

                if (ShareSaleTable.Visible) PopulateShareSaleTable();

                BankTable.RowCount = ic.bankList.Count + 1;
                BankTable.Invalidate();

                UpdateChart();

                LabourCost.Text = ""+ Math.Round(ic.population.labourCost,3);
                Population.Text = "" + ic.population.people;
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
        private void BankTable_CellValueNeeded(object sender,
	        DataGridViewCellValueEventArgs e)
        {
	        if (e.RowIndex == BankTable.RowCount - 1) return;
	        Bank bank = ic.bankList[e.RowIndex]; ;
	        // Set the cell value to paint using the Customer object retrieved.
	        switch (e.ColumnIndex)
	        {
		        case 0:
			        e.Value = bank.name;
			        break;

		        case 1:
			        e.Value = bank.money;
			        break;
	        }
        }
        private void PeopleTable_CellValueNeeded(object sender,
	        DataGridViewCellValueEventArgs e)
        {
	        if (e.RowIndex == PeopleTable.RowCount - 1) return;
	        AI person = ic.AIList[e.RowIndex];
	        // Set the cell value to paint using the Customer object retrieved.
	        switch (e.ColumnIndex)
	        {
		        case 0:
			        e.Value = person.name;
			        break;
		        case 1:
			        e.Value = person.money;
			        break;
                case 2:
                    e.Value = person.OwnedAssets();
                    break;
            }
        }

		private void ChartSelection_SelectedIndexChanged(object sender, EventArgs e)
		{
            Enum.TryParse(ChartSelection.SelectedValue.ToString(), out chartType);
            DrawChart();
        }
	}

	public static class MyMath
    {
	    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
	    {
		    if (val.CompareTo(min) < 0) return min;
		    else if (val.CompareTo(max) > 0) return max;
		    else return val;
	    }
        private static Random rng = new Random();  

        public static List<T> Shuffle<T>(this List<T> list)  
        {  
            List<T> newlist = new List<T>(list);
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = newlist[k];  
                newlist[k] = newlist[n];  
                newlist[n] = value;  
            }
            return newlist;
        }
    }
   
}

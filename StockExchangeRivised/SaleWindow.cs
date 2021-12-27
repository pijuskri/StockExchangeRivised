using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockExchangeRivised
{
    public partial class SaleWindow : Form
    {
        public List<ResourceSale> sales;
        public string name="";
        public SaleWindow(string name, List<ResourceSale> sales)
        {
            InitializeComponent();
            this.name = name;
            this.sales = sales;
        }
        public void UpdateTable()
        {
            while (SaleTable.Rows.Count > 1)
            {
                SaleTable.Rows.RemoveAt(0);
            }

            try
            {
                foreach (var sale in sales)
                {
                    int index = SaleTable.Rows.Add();
                    DataGridViewRow row = SaleTable.Rows[index];
                    row.Cells[0].Value = Math.Round(sale.amount,3);
                    row.Cells[1].Value = Math.Round(sale.price,3);
                    row.Cells[2].Value = sale.company.name;
                    row.Cells[3].Value = Math.Round(sale.soldLastTick,3);
                }
            }
            catch { }
        }

        private void SaleWindow_Load(object sender, EventArgs e)
        {
            UpdateTable();
            label1.Text = name;
        }
    }
}

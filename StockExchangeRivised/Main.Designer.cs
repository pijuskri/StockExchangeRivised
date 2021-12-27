namespace StockExchangeRivised
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.nextTurn = new System.Windows.Forms.Button();
			this.Next10Turns = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.ResourceTable = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.resourceSale = new System.Windows.Forms.DataGridViewButtonColumn();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.CompanyTable = new System.Windows.Forms.DataGridView();
			this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.money = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.revenue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.productionVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.productionOutput = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.productionInput = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.actualProduction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.productionRecipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CostToProduce = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.PeopleTable = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Moneyperson = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Assets = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.ShareSaleTable = new System.Windows.Forms.DataGridView();
			this.SellerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PriceShare = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.BankTable = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.LabourCost = new System.Windows.Forms.TextBox();
			this.LabourCostLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.Population = new System.Windows.Forms.TextBox();
			this.ChartSelection = new System.Windows.Forms.ComboBox();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ResourceTable)).BeginInit();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.CompanyTable)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PeopleTable)).BeginInit();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ShareSaleTable)).BeginInit();
			this.tabPage5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.BankTable)).BeginInit();
			this.tabPage6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// nextTurn
			// 
			this.nextTurn.Location = new System.Drawing.Point(1016, 14);
			this.nextTurn.Name = "nextTurn";
			this.nextTurn.Size = new System.Drawing.Size(100, 23);
			this.nextTurn.TabIndex = 0;
			this.nextTurn.Text = "Next turn";
			this.nextTurn.UseVisualStyleBackColor = true;
			this.nextTurn.Click += new System.EventHandler(this.nextTurn_Click);
			// 
			// Next10Turns
			// 
			this.Next10Turns.Location = new System.Drawing.Point(1016, 43);
			this.Next10Turns.Name = "Next10Turns";
			this.Next10Turns.Size = new System.Drawing.Size(100, 23);
			this.Next10Turns.TabIndex = 3;
			this.Next10Turns.Text = "Next turn x10";
			this.Next10Turns.UseVisualStyleBackColor = true;
			this.Next10Turns.Click += new System.EventHandler(this.Next10Turns_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.ResourceTable);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(903, 618);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Resources";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// ResourceTable
			// 
			this.ResourceTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ResourceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Type,
            this.Price,
            this.BasePrice,
            this.resourceSale});
			this.ResourceTable.Location = new System.Drawing.Point(0, 2);
			this.ResourceTable.Name = "ResourceTable";
			this.ResourceTable.RowHeadersWidth = 51;
			this.ResourceTable.Size = new System.Drawing.Size(900, 615);
			this.ResourceTable.TabIndex = 2;
			this.ResourceTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResourceTable_CellContentClick);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Name";
			this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.Width = 125;
			// 
			// Type
			// 
			this.Type.HeaderText = "Type";
			this.Type.MinimumWidth = 6;
			this.Type.Name = "Type";
			this.Type.Width = 125;
			// 
			// Price
			// 
			this.Price.HeaderText = "Price";
			this.Price.MinimumWidth = 6;
			this.Price.Name = "Price";
			this.Price.Width = 125;
			// 
			// BasePrice
			// 
			this.BasePrice.HeaderText = "Base Price";
			this.BasePrice.MinimumWidth = 6;
			this.BasePrice.Name = "BasePrice";
			this.BasePrice.Width = 125;
			// 
			// resourceSale
			// 
			this.resourceSale.HeaderText = "Sale";
			this.resourceSale.MinimumWidth = 6;
			this.resourceSale.Name = "resourceSale";
			this.resourceSale.Text = "Show";
			this.resourceSale.Width = 125;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.CompanyTable);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(903, 618);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Companies";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// CompanyTable
			// 
			this.CompanyTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.CompanyTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.money,
            this.revenue,
            this.value,
            this.productionVolume,
            this.productionOutput,
            this.productionInput,
            this.actualProduction,
            this.productionRecipe,
            this.CostToProduce});
			this.CompanyTable.Location = new System.Drawing.Point(0, 3);
			this.CompanyTable.Name = "CompanyTable";
			this.CompanyTable.RowHeadersWidth = 51;
			this.CompanyTable.Size = new System.Drawing.Size(903, 615);
			this.CompanyTable.TabIndex = 1;
			// 
			// name
			// 
			this.name.HeaderText = "Name";
			this.name.MinimumWidth = 6;
			this.name.Name = "name";
			this.name.Width = 150;
			// 
			// money
			// 
			this.money.HeaderText = "Money";
			this.money.MinimumWidth = 6;
			this.money.Name = "money";
			this.money.Width = 75;
			// 
			// revenue
			// 
			this.revenue.HeaderText = "Revenue";
			this.revenue.MinimumWidth = 6;
			this.revenue.Name = "revenue";
			this.revenue.Width = 75;
			// 
			// value
			// 
			this.value.HeaderText = "Value";
			this.value.MinimumWidth = 6;
			this.value.Name = "value";
			this.value.Width = 75;
			// 
			// productionVolume
			// 
			this.productionVolume.HeaderText = "Production Volume";
			this.productionVolume.MinimumWidth = 6;
			this.productionVolume.Name = "productionVolume";
			this.productionVolume.Width = 75;
			// 
			// productionOutput
			// 
			this.productionOutput.HeaderText = "Production Output";
			this.productionOutput.MinimumWidth = 6;
			this.productionOutput.Name = "productionOutput";
			this.productionOutput.Width = 50;
			// 
			// productionInput
			// 
			this.productionInput.HeaderText = "Production Input";
			this.productionInput.MinimumWidth = 6;
			this.productionInput.Name = "productionInput";
			this.productionInput.Width = 50;
			// 
			// actualProduction
			// 
			this.actualProduction.HeaderText = "Actual Production";
			this.actualProduction.MinimumWidth = 6;
			this.actualProduction.Name = "actualProduction";
			this.actualProduction.Width = 50;
			// 
			// productionRecipe
			// 
			this.productionRecipe.HeaderText = "Production Recipe";
			this.productionRecipe.MinimumWidth = 6;
			this.productionRecipe.Name = "productionRecipe";
			this.productionRecipe.Width = 125;
			// 
			// CostToProduce
			// 
			this.CostToProduce.HeaderText = "Cost To Produce";
			this.CostToProduce.MinimumWidth = 6;
			this.CostToProduce.Name = "CostToProduce";
			this.CostToProduce.Width = 75;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Location = new System.Drawing.Point(12, -1);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(911, 644);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.PeopleTable);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
			this.tabPage3.Size = new System.Drawing.Size(903, 618);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "AI";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// PeopleTable
			// 
			this.PeopleTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.PeopleTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.Moneyperson,
            this.Assets});
			this.PeopleTable.Location = new System.Drawing.Point(1, 3);
			this.PeopleTable.Name = "PeopleTable";
			this.PeopleTable.ReadOnly = true;
			this.PeopleTable.RowHeadersWidth = 51;
			this.PeopleTable.Size = new System.Drawing.Size(902, 615);
			this.PeopleTable.TabIndex = 3;
			this.PeopleTable.VirtualMode = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.HeaderText = "Name";
			this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 125;
			// 
			// Moneyperson
			// 
			this.Moneyperson.HeaderText = "Money";
			this.Moneyperson.MinimumWidth = 6;
			this.Moneyperson.Name = "Moneyperson";
			this.Moneyperson.ReadOnly = true;
			this.Moneyperson.Width = 125;
			// 
			// Assets
			// 
			this.Assets.HeaderText = "Assets";
			this.Assets.Name = "Assets";
			this.Assets.ReadOnly = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.ShareSaleTable);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(903, 618);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Share Sales";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// ShareSaleTable
			// 
			this.ShareSaleTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ShareSaleTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SellerName,
            this.dataGridViewTextBoxColumn4,
            this.Amount,
            this.PriceShare});
			this.ShareSaleTable.Location = new System.Drawing.Point(0, 2);
			this.ShareSaleTable.Name = "ShareSaleTable";
			this.ShareSaleTable.RowHeadersWidth = 51;
			this.ShareSaleTable.Size = new System.Drawing.Size(903, 615);
			this.ShareSaleTable.TabIndex = 4;
			// 
			// SellerName
			// 
			this.SellerName.HeaderText = "Seller Name";
			this.SellerName.MinimumWidth = 6;
			this.SellerName.Name = "SellerName";
			this.SellerName.Width = 125;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.HeaderText = "Company";
			this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.Width = 125;
			// 
			// Amount
			// 
			this.Amount.HeaderText = "Amount";
			this.Amount.MinimumWidth = 6;
			this.Amount.Name = "Amount";
			this.Amount.Width = 125;
			// 
			// PriceShare
			// 
			this.PriceShare.HeaderText = "Price";
			this.PriceShare.MinimumWidth = 6;
			this.PriceShare.Name = "PriceShare";
			this.PriceShare.Width = 125;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.BankTable);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Margin = new System.Windows.Forms.Padding(2);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(2);
			this.tabPage5.Size = new System.Drawing.Size(903, 618);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = " Banks";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// BankTable
			// 
			this.BankTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.BankTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
			this.BankTable.Location = new System.Drawing.Point(1, 3);
			this.BankTable.Name = "BankTable";
			this.BankTable.ReadOnly = true;
			this.BankTable.RowHeadersWidth = 51;
			this.BankTable.Size = new System.Drawing.Size(903, 615);
			this.BankTable.TabIndex = 5;
			this.BankTable.VirtualMode = true;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.HeaderText = "Bank name";
			this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 125;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.HeaderText = "Money";
			this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Width = 125;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.HeaderText = "Amount";
			this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Width = 125;
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.HeaderText = "Price";
			this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			this.dataGridViewTextBoxColumn7.Width = 125;
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.chart1);
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(903, 618);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "Charts";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(4, 4);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			series1.YValuesPerPoint = 2;
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(899, 614);
			this.chart1.TabIndex = 0;
			this.chart1.Text = "chart1";
			// 
			// LabourCost
			// 
			this.LabourCost.Location = new System.Drawing.Point(1016, 72);
			this.LabourCost.Name = "LabourCost";
			this.LabourCost.Size = new System.Drawing.Size(100, 20);
			this.LabourCost.TabIndex = 4;
			// 
			// LabourCostLabel
			// 
			this.LabourCostLabel.AutoSize = true;
			this.LabourCostLabel.Location = new System.Drawing.Point(944, 75);
			this.LabourCostLabel.Name = "LabourCostLabel";
			this.LabourCostLabel.Size = new System.Drawing.Size(66, 13);
			this.LabourCostLabel.TabIndex = 5;
			this.LabourCostLabel.Text = "Labour cost:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(944, 101);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Population:";
			// 
			// Population
			// 
			this.Population.Location = new System.Drawing.Point(1016, 98);
			this.Population.Name = "Population";
			this.Population.Size = new System.Drawing.Size(100, 20);
			this.Population.TabIndex = 6;
			// 
			// ChartSelection
			// 
			this.ChartSelection.FormattingEnabled = true;
			this.ChartSelection.Location = new System.Drawing.Point(947, 124);
			this.ChartSelection.Name = "ChartSelection";
			this.ChartSelection.Size = new System.Drawing.Size(121, 21);
			this.ChartSelection.TabIndex = 8;
			this.ChartSelection.SelectedIndexChanged += new System.EventHandler(this.ChartSelection_SelectedIndexChanged);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1125, 650);
			this.Controls.Add(this.ChartSelection);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Population);
			this.Controls.Add(this.LabourCostLabel);
			this.Controls.Add(this.LabourCost);
			this.Controls.Add(this.Next10Turns);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.nextTurn);
			this.Name = "Main";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ResourceTable)).EndInit();
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.CompanyTable)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PeopleTable)).EndInit();
			this.tabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ShareSaleTable)).EndInit();
			this.tabPage5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.BankTable)).EndInit();
			this.tabPage6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button nextTurn;
        private System.Windows.Forms.Button Next10Turns;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView ResourceTable;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView CompanyTable;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn BasePrice;
        private System.Windows.Forms.DataGridViewButtonColumn resourceSale;
		private System.Windows.Forms.TextBox LabourCost;
		private System.Windows.Forms.Label LabourCostLabel;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.DataGridView PeopleTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn name;
		private System.Windows.Forms.DataGridViewTextBoxColumn money;
		private System.Windows.Forms.DataGridViewTextBoxColumn revenue;
		private System.Windows.Forms.DataGridViewTextBoxColumn value;
		private System.Windows.Forms.DataGridViewTextBoxColumn productionVolume;
		private System.Windows.Forms.DataGridViewTextBoxColumn productionOutput;
		private System.Windows.Forms.DataGridViewTextBoxColumn productionInput;
		private System.Windows.Forms.DataGridViewTextBoxColumn actualProduction;
		private System.Windows.Forms.DataGridViewTextBoxColumn productionRecipe;
		private System.Windows.Forms.DataGridViewTextBoxColumn CostToProduce;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.DataGridView ShareSaleTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn SellerName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
		private System.Windows.Forms.DataGridViewTextBoxColumn PriceShare;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.DataGridView BankTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Population;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Moneyperson;
		private System.Windows.Forms.DataGridViewTextBoxColumn Assets;
		private System.Windows.Forms.ComboBox ChartSelection;
	}
}


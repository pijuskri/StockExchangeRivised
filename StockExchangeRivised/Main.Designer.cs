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
            this.nextTurn = new System.Windows.Forms.Button();
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ResourceTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Next10Turns = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CompanyTable)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceTable)).BeginInit();
            this.SuspendLayout();
            // 
            // nextTurn
            // 
            this.nextTurn.Location = new System.Drawing.Point(1077, 12);
            this.nextTurn.Name = "nextTurn";
            this.nextTurn.Size = new System.Drawing.Size(75, 23);
            this.nextTurn.TabIndex = 0;
            this.nextTurn.Text = "Next turn";
            this.nextTurn.UseVisualStyleBackColor = true;
            this.nextTurn.Click += new System.EventHandler(this.nextTurn_Click);
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
            this.CompanyTable.Size = new System.Drawing.Size(1055, 615);
            this.CompanyTable.TabIndex = 1;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // money
            // 
            this.money.HeaderText = "Money";
            this.money.Name = "money";
            // 
            // revenue
            // 
            this.revenue.HeaderText = "Revenue";
            this.revenue.Name = "revenue";
            // 
            // value
            // 
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // productionVolume
            // 
            this.productionVolume.HeaderText = "Production Volume";
            this.productionVolume.Name = "productionVolume";
            // 
            // productionOutput
            // 
            this.productionOutput.HeaderText = "Production Output";
            this.productionOutput.Name = "productionOutput";
            // 
            // productionInput
            // 
            this.productionInput.HeaderText = "Production Input";
            this.productionInput.Name = "productionInput";
            // 
            // actualProduction
            // 
            this.actualProduction.HeaderText = "Actual Production";
            this.actualProduction.Name = "actualProduction";
            // 
            // productionRecipe
            // 
            this.productionRecipe.HeaderText = "Production Recipe";
            this.productionRecipe.Name = "productionRecipe";
            // 
            // CostToProduce
            // 
            this.CostToProduce.HeaderText = "Cost To Produce";
            this.CostToProduce.Name = "CostToProduce";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1063, 644);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CompanyTable);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1055, 618);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ResourceTable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1055, 618);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ResourceTable
            // 
            this.ResourceTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResourceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Type,
            this.Price,
            this.BasePrice});
            this.ResourceTable.Location = new System.Drawing.Point(0, 2);
            this.ResourceTable.Name = "ResourceTable";
            this.ResourceTable.Size = new System.Drawing.Size(1055, 615);
            this.ResourceTable.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // BasePrice
            // 
            this.BasePrice.HeaderText = "Base Price";
            this.BasePrice.Name = "BasePrice";
            // 
            // Next10Turns
            // 
            this.Next10Turns.Location = new System.Drawing.Point(1077, 41);
            this.Next10Turns.Name = "Next10Turns";
            this.Next10Turns.Size = new System.Drawing.Size(75, 23);
            this.Next10Turns.TabIndex = 3;
            this.Next10Turns.Text = "Next turn";
            this.Next10Turns.UseVisualStyleBackColor = true;
            this.Next10Turns.Click += new System.EventHandler(this.Next10Turns_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 650);
            this.Controls.Add(this.Next10Turns);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.nextTurn);
            this.Name = "Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CompanyTable)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResourceTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button nextTurn;
        private System.Windows.Forms.DataGridView CompanyTable;
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView ResourceTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn BasePrice;
        private System.Windows.Forms.Button Next10Turns;
    }
}


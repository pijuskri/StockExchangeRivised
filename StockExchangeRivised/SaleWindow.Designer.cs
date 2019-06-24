namespace StockExchangeRivised
{
    partial class SaleWindow
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
            this.SaleTable = new System.Windows.Forms.DataGridView();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.soldLastTick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SaleTable)).BeginInit();
            this.SuspendLayout();
            // 
            // SaleTable
            // 
            this.SaleTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SaleTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.amount,
            this.price,
            this.company,
            this.soldLastTick});
            this.SaleTable.Location = new System.Drawing.Point(0, 38);
            this.SaleTable.Name = "SaleTable";
            this.SaleTable.Size = new System.Drawing.Size(446, 337);
            this.SaleTable.TabIndex = 3;
            // 
            // amount
            // 
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            // 
            // price
            // 
            this.price.HeaderText = "Price";
            this.price.Name = "price";
            // 
            // company
            // 
            this.company.HeaderText = "Company";
            this.company.Name = "company";
            // 
            // soldLastTick
            // 
            this.soldLastTick.HeaderText = "Sold";
            this.soldLastTick.Name = "soldLastTick";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // SaleWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 375);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaleTable);
            this.Name = "SaleWindow";
            this.Text = "SaleWindow";
            this.Load += new System.EventHandler(this.SaleWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SaleTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView SaleTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn soldLastTick;
        private System.Windows.Forms.Label label1;
    }
}
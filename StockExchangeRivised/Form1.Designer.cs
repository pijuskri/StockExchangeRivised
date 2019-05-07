namespace StockExchangeRivised
{
    partial class Form1
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
            this.SuspendLayout();
            // 
            // nextTurn
            // 
            this.nextTurn.Location = new System.Drawing.Point(106, 127);
            this.nextTurn.Name = "nextTurn";
            this.nextTurn.Size = new System.Drawing.Size(75, 23);
            this.nextTurn.TabIndex = 0;
            this.nextTurn.Text = "Next turn";
            this.nextTurn.UseVisualStyleBackColor = true;
            this.nextTurn.Click += new System.EventHandler(this.nextTurn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.nextTurn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button nextTurn;
    }
}


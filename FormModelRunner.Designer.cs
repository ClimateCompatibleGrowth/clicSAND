namespace ModelRunner
{
    partial class FormModelRunner
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
            this.labelDataSource = new System.Windows.Forms.Label();
            this.textBoxDataSource = new System.Windows.Forms.TextBox();
            this.labelModel = new System.Windows.Forms.Label();
            this.textBoxModel = new System.Windows.Forms.TextBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonOpenXLS = new System.Windows.Forms.Button();
            this.buttonOpenModel = new System.Windows.Forms.Button();
            this.buttonOpenResults = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDataSource
            // 
            this.labelDataSource.AutoSize = true;
            this.labelDataSource.Location = new System.Drawing.Point(13, 13);
            this.labelDataSource.Name = "labelDataSource";
            this.labelDataSource.Size = new System.Drawing.Size(88, 13);
            this.labelDataSource.TabIndex = 0;
            this.labelDataSource.Text = "Data Source (xls)";
            // 
            // textBoxDataSource
            // 
            this.textBoxDataSource.Location = new System.Drawing.Point(108, 13);
            this.textBoxDataSource.Name = "textBoxDataSource";
            this.textBoxDataSource.Size = new System.Drawing.Size(681, 20);
            this.textBoxDataSource.TabIndex = 1;
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.Location = new System.Drawing.Point(13, 43);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(36, 13);
            this.labelModel.TabIndex = 2;
            this.labelModel.Text = "Model";
            // 
            // textBoxModel
            // 
            this.textBoxModel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxModel.Location = new System.Drawing.Point(108, 40);
            this.textBoxModel.Name = "textBoxModel";
            this.textBoxModel.Size = new System.Drawing.Size(681, 20);
            this.textBoxModel.TabIndex = 3;
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(108, 66);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(100, 23);
            this.buttonRun.TabIndex = 6;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(13, 95);
            this.textBoxOutput.MaxLength = 100000;
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(811, 342);
            this.textBoxOutput.TabIndex = 7;
            // 
            // buttonOpenXLS
            // 
            this.buttonOpenXLS.Location = new System.Drawing.Point(795, 13);
            this.buttonOpenXLS.Name = "buttonOpenXLS";
            this.buttonOpenXLS.Size = new System.Drawing.Size(28, 20);
            this.buttonOpenXLS.TabIndex = 8;
            this.buttonOpenXLS.Text = "...";
            this.buttonOpenXLS.UseVisualStyleBackColor = true;
            this.buttonOpenXLS.Click += new System.EventHandler(this.buttonOpenXLS_Click);
            // 
            // buttonOpenModel
            // 
            this.buttonOpenModel.Location = new System.Drawing.Point(795, 40);
            this.buttonOpenModel.Name = "buttonOpenModel";
            this.buttonOpenModel.Size = new System.Drawing.Size(28, 20);
            this.buttonOpenModel.TabIndex = 9;
            this.buttonOpenModel.Text = "...";
            this.buttonOpenModel.UseVisualStyleBackColor = true;
            this.buttonOpenModel.Click += new System.EventHandler(this.buttonOpenModel_Click);
            // 
            // buttonOpenResults
            // 
            this.buttonOpenResults.Location = new System.Drawing.Point(214, 66);
            this.buttonOpenResults.Name = "buttonOpenResults";
            this.buttonOpenResults.Size = new System.Drawing.Size(100, 23);
            this.buttonOpenResults.TabIndex = 10;
            this.buttonOpenResults.Text = "Open Log";
            this.buttonOpenResults.UseVisualStyleBackColor = true;
            this.buttonOpenResults.Click += new System.EventHandler(this.buttonOpenResults_Click);
            // 
            // FormModelRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 449);
            this.Controls.Add(this.buttonOpenResults);
            this.Controls.Add(this.buttonOpenModel);
            this.Controls.Add(this.buttonOpenXLS);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.textBoxModel);
            this.Controls.Add(this.labelModel);
            this.Controls.Add(this.textBoxDataSource);
            this.Controls.Add(this.labelDataSource);
            this.Name = "FormModelRunner";
            this.Text = "ModelRunner";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDataSource;
        private System.Windows.Forms.TextBox textBoxDataSource;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.TextBox textBoxModel;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button buttonOpenXLS;
        private System.Windows.Forms.Button buttonOpenModel;
        private System.Windows.Forms.Button buttonOpenResults;
    }
}


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
            this.textBoxResults = new System.Windows.Forms.TextBox();
            this.labelResults = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
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
            this.textBoxDataSource.Size = new System.Drawing.Size(716, 20);
            this.textBoxDataSource.TabIndex = 1;
            this.textBoxDataSource.Text = "E:\\Nicki\\Downloads\\ModelRunner\\CCG - SAND Interface (clean) 2020 (ver8).xlsx";
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
            this.textBoxModel.Size = new System.Drawing.Size(716, 20);
            this.textBoxModel.TabIndex = 3;
            this.textBoxModel.Text = "E:\\Nicki\\Downloads\\ModelRunner\\model_new.txt";
            // 
            // textBoxResults
            // 
            this.textBoxResults.Location = new System.Drawing.Point(108, 67);
            this.textBoxResults.Name = "textBoxResults";
            this.textBoxResults.Size = new System.Drawing.Size(716, 20);
            this.textBoxResults.TabIndex = 4;
            // 
            // labelResults
            // 
            this.labelResults.AutoSize = true;
            this.labelResults.Location = new System.Drawing.Point(12, 70);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(42, 13);
            this.labelResults.TabIndex = 5;
            this.labelResults.Text = "Results";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(108, 94);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(100, 23);
            this.buttonRun.TabIndex = 6;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(13, 133);
            this.textBoxOutput.MaxLength = 100000;
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(811, 304);
            this.textBoxOutput.TabIndex = 7;
            // 
            // FormModelRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 449);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.labelResults);
            this.Controls.Add(this.textBoxResults);
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
        private System.Windows.Forms.TextBox textBoxResults;
        private System.Windows.Forms.Label labelResults;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TextBox textBoxOutput;
    }
}


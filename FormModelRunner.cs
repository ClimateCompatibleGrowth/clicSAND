using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Configuration;

namespace ModelRunner
{
    public partial class FormModelRunner : Form
    {
        string logFileName = null;

        public FormModelRunner()
        {
            InitializeComponent();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string dataFileName = textBoxDataSource.Text + ".txt";
            string lpFileName = textBoxDataSource.Text + ".lp";
            string resultsFileName = textBoxDataSource.Text + ".results.txt";
            logFileName = string.Format("{0}{1}.log.txt", textBoxDataSource.Text, DateTime.Now.ToString("yyyyMMddHHmmss"));

            textBoxOutput.Clear();

            textBoxOutput.Text += new string('-', 150) + "\r\n";
            textBoxOutput.Text += "Data file: " + dataFileName + "\r\n";
            textBoxOutput.Text += "Model file: " + textBoxModel.Text+ "\r\n";
            textBoxOutput.Text += "GLPSOL Output file: " + lpFileName + "\r\n";
            textBoxOutput.Text += "Results file: " + resultsFileName+ "\r\n";
            textBoxOutput.Text += "Log file: " + logFileName + "\r\n";
            textBoxOutput.Text += new string('-', 150) + "\r\n";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                bool result = false;
                result = ExtractDataFromXLS(textBoxDataSource.Text, dataFileName);
                if (result)
                {
                    result = RunGLPSOL(dataFileName, textBoxModel.Text, lpFileName);
                }
                if (result)
                {
                    result = RunCBC(lpFileName, resultsFileName);
                }
                if (result)
                {
                    result = RunReporting(dataFileName, resultsFileName);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error Running Model");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                try
                {
                    SaveLog(logFileName, textBoxOutput.Lines);
                }
                catch (Exception exc)
                {
                    textBoxOutput.Text += "Unable to save log: " + exc.Message + "\r\n";
                }
            }
        }

        private bool ExtractDataFromXLS(string xlsFileName, string dataFileName)
        {
            try
            {
                var Excel = new Excel.Application();
                var workBook = Excel.Workbooks.Open(xlsFileName);
                Excel.Application.Visible = true;
                Excel.Application.Run(String.Format("'{0}'!Module1.writefile", xlsFileName));
                workBook.Saved = true;
                workBook.Close();
                Excel.Quit();
            }
            catch (Exception exc)
            {
                textBoxOutput.Text += String.Format("Error extracting data:\r\n{0}", exc.Message);
                return false;
            }
            return true;
        }
        private bool RunGLPSOL(string dataFileName, string modelFileName, string lpFileName)
        {
            return RunProcess(String.Format(@"{0}\utils\glpsol.exe", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)), string.Format("--check -m \"{0}\" -d \"{1}\" --wlp \"{2}\"", modelFileName, dataFileName, lpFileName));
        }

        private bool RunCBC(string inputFileName, string outputFileName)
        {
            return RunProcess(String.Format(@"{0}\utils\cbc.exe", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)), String.Format("\"{0}\" solve -solu \"{1}\"", inputFileName, outputFileName));
        }

        private bool RunReporting(string dataFileName, string cbcOutputfileName)
        {
            return RunProcess(String.Format(@"{0}\python.exe", ConfigurationManager.AppSettings["pythonLocation"]), string.Format(@"{0} {1} {2}", ConfigurationManager.AppSettings["pythonScript"], dataFileName, cbcOutputfileName));
        }
        private bool RunProcess(string filename, string args)
        {
            try
            {
                textBoxOutput.Text += new string('-', 150) + "\r\n";
                textBoxOutput.Text += string.Format("Running {0} {1}\r\n", filename, args);
                textBoxOutput.Text += new string('-', 150) + "\r\n";
                Process compiler = new Process();
                compiler.StartInfo.FileName = filename;
                compiler.StartInfo.Arguments = args;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.CreateNoWindow = false;

                compiler.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                // Prepend line numbers to each line of the output.
                if (!String.IsNullOrEmpty(e.Data))
                    {
                        textBoxOutput.Text += e.Data;
                    }
                });

                compiler.Start();

                textBoxOutput.Text += compiler.StandardOutput.ReadToEnd();

                compiler.WaitForExit();

                return true;
            }
            catch (Exception e)
            {
                textBoxOutput.Text += "Error: " + e.Message + "\r\n";
                return false;
            }
        }

        private void buttonOpenXLS_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("Excel spreadsheets|*.xls?");

            if (!String.IsNullOrEmpty(filename))
            {
                textBoxDataSource.Text = filename;
            }
        }

        private string OpenFile(string filter)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = filter;
            fileDialog.Multiselect = false;

            DialogResult dialogResult = fileDialog.ShowDialog();

            if (dialogResult.Equals(DialogResult.OK))
            {
                return fileDialog.FileName;
            }
            return null;
        }

        private void buttonOpenModel_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("Text files|*.txt");

            if (!String.IsNullOrEmpty(filename))
            {
                textBoxModel.Text = filename;
            }
        }

        private void SaveLog(string logFileName, string[] lines)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(logFileName))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        private void buttonOpenResults_Click(object sender, EventArgs e)
        {
            RunProcess(@"notepad.exe", logFileName);
        }
    }
}

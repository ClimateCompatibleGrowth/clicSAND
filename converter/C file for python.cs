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
            string processedResultsFileName = resultsFileName + ".processed_results.csv";
            logFileName = string.Format("{0}{1}.log.txt", textBoxDataSource.Text, DateTime.Now.ToString("yyyyMMddHHmmss"));

            textBoxOutput.Clear();

            textBoxOutput.Text += new string('-', 150) + "\r\n";
            textBoxOutput.Text += "Data file: " + dataFileName + "\r\n";
            textBoxOutput.Text += "Model file: " + textBoxModel.Text + "\r\n";
            textBoxOutput.Text += "GLPSOL Output file: " + lpFileName + "\r\n";
            textBoxOutput.Text += "Results file: " + resultsFileName + "\r\n";
            textBoxOutput.Text += "Processed Results file: " + processedResultsFileName + "\r\n";
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
                    result = RunCBC(lpFileName, resultsFileName, checkCBCRatio.Checked, numericRatio.Value.ToString());
                }
                //if (result)
                //{
                //    result = RunReporting(dataFileName, resultsFileName, txtYear.Text);
                //}
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error Running Model");
            }
            try
            {
                textBoxOutput.Text += "Converting results for visualisation";
                ConvertResults(resultsFileName);
            }
            catch (Exception exc)
            {
                textBoxOutput.Text += "Unable to convert results: " + exc.Message + "\r\n";
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

        private void ConvertResults(string input)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.CreateNoWindow = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"Converter\dist\python_converter.exe");
            textBoxOutput.Text += "Running converter from: " + path;
            startInfo.FileName = path;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            string input_path = Path.GetDirectoryName(input);
            startInfo.Arguments = "\"" + input + "\"" + " " + "\"" + input_path + "\"";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    string output = exeProcess.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    textBoxOutput.Text += output;
                    string err = exeProcess.StandardError.ReadToEnd();
                    Console.WriteLine(err);
                    textBoxOutput.Text += err;
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception exc)
            {
                textBoxOutput.Text += "Unable to run python converter: " + exc.Message + "\r\n";
            }
        }


        private bool RunGLPSOL(string dataFileName, string modelFileName, string lpFileName)
        {
            return RunProcess(String.Format(@"{0}\utils\glpsol.exe", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)), string.Format("--check -m \"{0}\" -d \"{1}\" --wlp \"{2}\"", modelFileName, dataFileName, lpFileName));
        }

        private bool RunCBC(string inputFileName, string outputFileName, bool applyRatio, string ratio)
        {
            string cbcParams = null;

            if (applyRatio)
            {
                cbcParams = String.Format("\"{0}\" ratio {1} solve -solu \"{2}\"", inputFileName, ratio, outputFileName);
            }
            else
            {
                cbcParams = String.Format("\"{0}\" solve -solu \"{1}\"", inputFileName, outputFileName);
            }
            return RunProcess(String.Format(@"{0}\cbc.exe", ConfigurationManager.AppSettings["cbcLocation"]), cbcParams);
        }

        private bool RunReporting(string dataFileName, string cbcOutputfileName, string year)
        {
            return RunProcess(String.Format("\"{0}\\python.exe\"", ConfigurationManager.AppSettings["pythonLocation"]), string.Format("\"{0}\" \"{1}\" \"{2}\" {3}", ConfigurationManager.AppSettings["pythonScript"], dataFileName, cbcOutputfileName, year));
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

        private void checkCBCRatio_CheckedChanged(object sender, EventArgs e)
        {
            numericRatio.Enabled = checkCBCRatio.Checked;
        }

        private void buttonSaveTemplates_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Extract templates";
            folderDialog.ShowNewFolderButton = true;
            DialogResult result = folderDialog.ShowDialog(this);

            if (result.Equals(DialogResult.OK))
            {
                try
                {
                    foreach (string fileName in Directory.GetFiles(@"Templates"))
                    {
                        File.Copy(fileName, Path.Combine(folderDialog.SelectedPath, Path.GetFileName(fileName)), true);
                    }
                    MessageBox.Show(String.Format("Templates exported to {0}", folderDialog.SelectedPath));
                }
                catch (Exception exc)
                {
                    MessageBox.Show(String.Format("Failed to copy files: \r\n{0}", exc.Message));
                }
            }
        }
    }
}
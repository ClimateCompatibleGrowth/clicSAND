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
using ClosedXML.Excel;

namespace ModelRunner
{
    public partial class FormModelRunner : Form
    {
        public FormModelRunner()
        {
            InitializeComponent();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string dataFileName = textBoxDataSource.Text + ".txt";
            string lpFileName = textBoxDataSource.Text + ".lp";
            string resultsFileName = textBoxDataSource.Text + ".results.txt";

            textBoxOutput.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ExtractDataFromXLS(textBoxDataSource.Text, dataFileName);
                RunGLPSOL(dataFileName, textBoxModel.Text, lpFileName);
                RunCBC(lpFileName, resultsFileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error Running Model");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ExtractDataFromXLS(string xlsFileName, string dataFileName)
        {
            var wb = new XLWorkbook(xlsFileName);
            var sheet = wb.Worksheet("ToDataFile");
            var range = sheet.RangeUsed();

            foreach (var row in range.Rows())
            {
                //Console.WriteLine(row.RowNumber());

                foreach (var col in row.Cells())
                {
                    //sb.Append(col.Text);
                    Console.Write(col.CachedValue + "\t");
                }
                Console.WriteLine();
            }


            return true;
        }
        private bool RunGLPSOL(string dataFileName, string modelFileName, string lpFileName)
        {
            return RunProcess("utils\\glpsol.exe", string.Format("--check -m {0} -d {1} --wlp {2}", modelFileName, dataFileName, lpFileName));
        }

        private bool RunCBC(string inputFileName, string outputFileName)
        {
            return RunProcess("utils\\cbc.exe", String.Format("{0} solve -solu {1}", inputFileName, outputFileName));
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GUI_principles
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static int Add(int X, int Y)
        {
            return X + Y;
        }

        public static int Substract(int X, int Y)
        {
            return X - Y;
        }

        public static int Multiply(int X, int Y)
        {
            return X * Y;
        }

        public static int Divide(int X, int Y)
        {
            return X / Y;
        }

        public static bool checkNum(string value)
        {
            double num;
            return double.TryParse(value, out num);
        }

        public static string invertNum(string value)
        {
            double num = 0;

            if(checkNum(value))
            {
                num = int.Parse(value);
                num = num * (-1);
            }

            return num.ToString();
        }

        public static void WriteLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}

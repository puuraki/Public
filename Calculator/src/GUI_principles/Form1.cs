using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_principles
{
    public partial class Form1 : Form
    {
        private string strX = "", strY = "";
        private string myHex, logText = "";
        private int X = 0, Y = 0, result = 0;
        private int calcOperator = 0; // 1 = divide, 2 = multiply, 3 = substract, 4 = add
        private bool isFirstSet = false;
        private bool isHex = false;
        private bool equalPressed = false;
        private bool divByZero = false;

        static AutoResetEvent autoEvent1 = new AutoResetEvent(false);
        static AutoResetEvent autoEvent2 = new AutoResetEvent(false);

        public Form1()
        {
            InitializeComponent();
            tbScreen.Text = "0";
            backgroundWorker1.RunWorkerAsync(logText);
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "0";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "0";
                tbScreen.Text = strX;
            }
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            if (isFirstSet)
            {
                strY += "1";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "1";
                tbScreen.Text = strX;
            }
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "2";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "2";
                tbScreen.Text = strX;
            }
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "3";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "3";
                tbScreen.Text = strX;
            }
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "4";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "4";
                tbScreen.Text = strX;
            }
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "5";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "5";
                tbScreen.Text = strX;
            }
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "6";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "6";
                tbScreen.Text = strX;
            }
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "7";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "7";
                tbScreen.Text = strX;
            }
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "8";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "8";
                tbScreen.Text = strX;
            }
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "9";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "9";
                tbScreen.Text = strX;
            }
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "A";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "A";
                tbScreen.Text = strX;
            }
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "B";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "B";
                tbScreen.Text = strX;
            }
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "C";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "C";
                tbScreen.Text = strX;
            }
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "D";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "D";
                tbScreen.Text = strX;
            }
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "E";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "E";
                tbScreen.Text = strX;
            }
        }

        private void btnF_Click(object sender, EventArgs e)
        {
            if(isFirstSet)
            {
                strY += "F";
                tbScreen.Text = strY;
            }
            else
            {
                strX += "F";
                tbScreen.Text = strX;
            }
        }

        private void btnInverse_Click(object sender, EventArgs e)
        {
            if(!isFirstSet)
            {
                strX = tbScreen.Text;
                strX = Program.invertNum(strX);
                tbScreen.Text = strX;
            }
            else
            {
                strY = tbScreen.Text;
                strY = Program.invertNum(strY);
                tbScreen.Text = strY;
            }
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            if (!isFirstSet)
            {
                isFirstSet = true;
                strX = tbScreen.Text;
                tbEquation.Text += strX + " ÷ ";
            }
            else
            {
                if (equalPressed || calcOperator != 1)
                {
                    tbEquation.Text = strX + " ÷ ";
                    equalPressed = false;
                }
                else
                {
                    strY = tbScreen.Text;
                    if (isHex)
                    {
                        X = Convert.ToInt32(strX, 16);
                        Y = Convert.ToInt32(strY, 16);
                        if (X != 0 && Y != 0)
                        {
                            myHex = Program.Divide(X, Y).ToString("X");
                            logText = strX + " ÷ " + strY + " = " + myHex;
                        }
                        else
                        {
                            logText = "Tried dividing by zero!";
                            divByZero = true;
                        }
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = myHex;
                        logText = "";
                    }
                    else
                    {
                        X = int.Parse(strX);
                        Y = int.Parse(strY);
                        if (X != 0 && Y != 0)
                        {
                            myHex = Program.Divide(X, Y).ToString("X");
                            logText = strX + " ÷ " + strY + " = " + result;
                        }
                        else
                        {
                            logText = "Tried dividing by zero!";
                            divByZero = true;
                        }
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = result.ToString();
                        logText = "";
                    }
                    tbEquation.Text = strX + " ÷ ";
                    strY = "";
                    tbScreen.Text = "0";
                    divByZero = false;
                }
            }
            calcOperator = 1;
        }

        private void btnMult_Click(object sender, EventArgs e)
        {
            if (!isFirstSet)
            {
                isFirstSet = true;
                strX = tbScreen.Text;
                tbEquation.Text += strX + " × ";
            }
            else
            {
                if (equalPressed || calcOperator != 2)
                {
                    tbEquation.Text = strX + " × ";
                    equalPressed = false;
                }
                else
                {
                    strY = tbScreen.Text;
                    if (isHex)
                    {
                        X = Convert.ToInt32(strX, 16);
                        Y = Convert.ToInt32(strY, 16);
                        myHex = Program.Multiply(X, Y).ToString("X");
                        logText = strX + " × " + strY + " = " + myHex;
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = myHex;
                        logText = "";
                    }
                    else
                    {
                        X = int.Parse(strX);
                        Y = int.Parse(strY);
                        result = Program.Multiply(X, Y);
                        logText = strX + " × " + strY + " = " + result;
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = result.ToString();
                        logText = "";
                    }
                    tbEquation.Text = strX + " × ";
                    strY = "";
                    tbScreen.Text = "0";
                }
            }
            calcOperator = 2;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (!isFirstSet)
            {
                isFirstSet = true;
                strX = tbScreen.Text;
                tbEquation.Text += strX + " - ";
            }
            else
            {
                if (equalPressed || calcOperator != 3)
                {
                    tbEquation.Text = strX + " - ";
                    equalPressed = false;
                }
                else
                {
                    strY = tbScreen.Text;
                    if (isHex)
                    {
                        X = Convert.ToInt32(strX, 16);
                        Y = Convert.ToInt32(strY, 16);
                        myHex = Program.Substract(X, Y).ToString("X");
                        logText = strX + " - " + strY + " = " + myHex;
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = myHex;
                        logText = "";
                    }
                    else
                    {
                        X = int.Parse(strX);
                        Y = int.Parse(strY);
                        result = Program.Substract(X, Y);
                        logText = strX + " - " + strY + " = " + result;
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = result.ToString();
                        logText = "";
                    }
                    tbEquation.Text = strX + " - ";
                    strY = "";
                    tbScreen.Text = "0";
                }
            }
            calcOperator = 3;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (!isFirstSet)
            {
                isFirstSet = true;
                strX = tbScreen.Text;
                tbEquation.Text += strX + " + ";
            }
            else
            {
                if (equalPressed || calcOperator != 4)
                {
                    tbEquation.Text = strX + " + ";
                    equalPressed = false;
                }
                else
                {
                    strY = tbScreen.Text;
                    if(isHex)
                    {
                        X = Convert.ToInt32(strX, 16);
                        Y = Convert.ToInt32(strY, 16);
                        myHex = Program.Add(X, Y).ToString("X");
                        logText = strX + " + " + strY + " = " + myHex;
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = myHex;
                        logText = "";
                    }
                    else
                    {
                        X = int.Parse(strX);
                        Y = int.Parse(strY);
                        result = Program.Add(X, Y);
                        logText = strX + " + " + strY + " = " + result;
                        autoEvent1.Set();
                        autoEvent2.WaitOne();
                        strX = result.ToString();
                        logText = "";
                    }
                    tbEquation.Text = strX + " + ";
                    strY = "";
                    tbScreen.Text = "0";
                }
            }
            calcOperator = 4;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            if (!isFirstSet)
            {
                isFirstSet = true;
                strX = tbScreen.Text;
            }
            strY = tbScreen.Text;
            if(isHex)
            {
                X = Convert.ToInt32(strX, 16);
                Y = Convert.ToInt32(strY, 16);
            }
            else
            {
                X = int.Parse(strX);
                Y = int.Parse(strY);
            }

            switch (calcOperator)
            {
                case 1:
                    {
                        if (X != 0 && Y != 0)
                        {
                            result = Program.Divide(X, Y);
                        }
                        else
                        {
                            divByZero = true;
                        }
                        break;
                    }
                case 2:
                    {
                        result = Program.Multiply(X, Y);
                        break;
                    }
                case 3:
                    {
                        result = Program.Substract(X, Y);
                        break;
                    }
                case 4:
                    {
                        result = Program.Add(X, Y);
                        break;
                    }
            }

            if(isHex)
            {
                myHex = result.ToString("X");
                tbScreen.Text = myHex;
                if(calcOperator == 1)
                {
                    if(!divByZero)
                    {
                        logText = strX + " ÷ " + strY + " = " + myHex;
                    }
                    else
                    {
                        logText = "Tried dividing by zero!";
                    }
                }
                else if(calcOperator == 2)
                {
                    logText = strX + " × " + strY + " = " + myHex;
                }
                else if(calcOperator == 3)
                {
                    logText = strX + " - " + strY + " = " + myHex;
                }
                else if(calcOperator == 4)
                {
                    logText = strX + " + " + strY + " = " + myHex;
                }
                strX = myHex;
            }
            else
            {
                tbScreen.Text = result.ToString();
                if (calcOperator == 1)
                {
                    if (!divByZero)
                    {
                        logText = strX + " ÷ " + strY + " = " + result;
                    }
                    else
                    {
                        logText = "Tried dividing by zero!";
                    }
                }
                else if (calcOperator == 2)
                {
                    logText = strX + " × " + strY + " = " + result;
                }
                else if (calcOperator == 3)
                {
                    logText = strX + " - " + strY + " = " + result;
                }
                else if (calcOperator == 4)
                {
                    logText = strX + " + " + strY + " = " + result;
                }
                strX = result.ToString();
            }
            autoEvent1.Set();
            autoEvent2.WaitOne();
            calcOperator = 0;
            logText = "";
            strY = "";
            tbEquation.Text = strX;
            equalPressed = true;
            divByZero = false;
        }

        private void btnHex_Click(object sender, EventArgs e)
        {
            btnA.Enabled = true;
            btnB.Enabled = true;
            btnC.Enabled = true;
            btnD.Enabled = true;
            btnE.Enabled = true;
            btnF.Enabled = true;
            btnDec.Enabled = true;
            btnHex.Enabled = false;
            isHex = true;
            strX = strY = "";
            X = Y = result = calcOperator = 0;
            isFirstSet = equalPressed = false;
            tbScreen.Text = "0";
            tbEquation.Text = "";
            logText = "Using hexadecimal calculator.";
            autoEvent1.Set();
            autoEvent2.WaitOne();
            logText = "";

        }

        private void btnDec_Click(object sender, EventArgs e)
        {
            btnA.Enabled = false;
            btnB.Enabled = false;
            btnC.Enabled = false;
            btnD.Enabled = false;
            btnE.Enabled = false;
            btnF.Enabled = false;
            btnDec.Enabled = false;
            btnHex.Enabled = true;
            isHex = false;
            strX = strY = "";
            X = Y = result = calcOperator = 0;
            isFirstSet = equalPressed = false;
            tbScreen.Text = "0";
            tbEquation.Text = "";
            logText = "Using decimal calculator.";
            autoEvent1.Set();
            autoEvent2.WaitOne();
            logText = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            strX = strY = "";
            X = Y = result = calcOperator = 0;
            isFirstSet = equalPressed = false;
            tbScreen.Text = "0";
            tbEquation.Text = "";
            logText = "Memory cleared.";
            autoEvent1.Set();
            autoEvent2.WaitOne();
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            tbScreen.Text = "0";
            if(isFirstSet)
            {
                strY = "";
            }
            else
            {
                strX = "";
            }
        }

        private void tbScreen_TextChanged(object sender, EventArgs e)
        {
            string newStr = "", oldStr = tbScreen.Text;

            if (!isHex)
            {
                for (int i = 0; i < oldStr.Length; i++)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(Char.ToString(oldStr[i]), "[^0-9-]"))
                    {
                        newStr += oldStr[i];
                    }
                }
                tbScreen.Text = newStr;
                tbScreen.Select(tbScreen.Text.Length, 0);
            }
            else
            {
                {
                    for (int i = 0; i < oldStr.Length; i++)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(Char.ToString(oldStr[i]), "[^0-9A-Fa-f,-]"))
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(Char.ToString(oldStr[i]), "[^a-f]"))
                            {
                                newStr += Char.ToUpper(oldStr[i]);
                            }
                            else
                            {
                                newStr += oldStr[i];
                            }
                        }
                    }
                    tbScreen.Text = newStr;
                    tbScreen.Select(tbScreen.Text.Length, 0);
                }
            }
        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            autoEvent1.WaitOne();
            Thread.Sleep(50);
            Program.WriteLog(logText);
            autoEvent2.Set();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(logText);
        }
    }
}

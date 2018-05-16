using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace TCP_client
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!clientSocket.ConnectAsync("193.166.72.3", 4360).Wait(5000))
                {
                    lblStatus.Text = "Connection error";
                }
                else
                {
                    lblStatus.Text = "Connected to server";
                    stream = clientSocket.GetStream();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Connection error";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int value1, value2;

            if (tbValue1.Text.Length == 0 || tbValue2.Text.Length == 0)
            {
                stream.Close();
                clientSocket.Close();
                lblStatus.Text = "Disconnected from the server";
                this.Close();
            }

            if (int.TryParse(tbValue1.Text, out value1) && int.TryParse(tbValue2.Text, out value2))
            {
                tbReceive.Text = tbReceive.Text + Environment.NewLine + ">> " + value1 + lblOperation.Text + value2;

                try
                {
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(value1 + lblOperation.Text + value2 + '\0');
                    stream.Write(outStream, 0, outStream.Length);
                    stream.Flush();

                    byte[] inStream = new byte[256];
                    stream.Read(inStream, 0, inStream.Length);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    msg(returndata);
                    tbValue1.Text = "";
                    tbValue2.Text = "";
                    tbValue1.Focus();
                }
                catch (Exception ex)
                {
                    tbReceive.Text = tbReceive.Text + Environment.NewLine + "An error occurred.";
                    try
                    {
                        stream.Close();
                    }
                    catch (Exception err)
                    {
                        tbReceive.Text += " Connection is already closed";
                    }
                }
            }
            else
            {
                lblError.Text = "Incorrect input. Use whole numbers only.";
            }
        }

        public void msg(string msg)
        {
            tbReceive.Text = tbReceive.Text + Environment.NewLine + "<< " + msg;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                stream.Close();
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                tbReceive.Text = tbReceive.Text + Environment.NewLine + "An error occurred.";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblOperation.Text = "+";
        }

        private void btnSubsctract_Click(object sender, EventArgs e)
        {
            lblOperation.Text = "-";
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_TCP_client
{
    public partial class Form1 : Form
    {
        private Functions func = new Functions();
        private string status = "";
        private bool canDblClck = true;
        private string message = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            status = func.Connect();
            lblStatus.Text = status;
            if (status == "Connected to server")
            {
                message = string.Format("0&{0}&{1}&{2}&{3}", tbServer.Text, tbUname.Text, tbPass.Text, tbDb.Text);
                btnDisconnect.Enabled = true;
                btnShowTbl.Enabled = true;
                btnConnect.Enabled = false;
                tbDb.Enabled = false;
                tbPass.Enabled = false;
                tbServer.Enabled = false;
                tbUname.Enabled = false;
                func.Send(message, dgvTable);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            func.Send("3", dgvTable);
            status = func.Disconnect();
            lblStatus.Text = status;
            if (status == "Status - disconnected")
            {
                btnDisconnect.Enabled = false;
                btnShowTbl.Enabled = false;
                btnConnect.Enabled = true;
                tbDb.Enabled = true;
                tbPass.Enabled = true;
                tbServer.Enabled = true;
                tbUname.Enabled = true;
            }
            Application.Exit();
        }

        private void btnShowTbl_Click(object sender, EventArgs e)
        {
            canDblClck = true;
            string command = "2";
            func.Send(command, dgvTable);
        }

        private void dgvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (canDblClck)
            {
                canDblClck = false;
                string command = "1;" + dgvTable.CurrentCell.Value;
                lblError.Text = command;
                func.Send(command, dgvTable);
            }
        }
    }
}

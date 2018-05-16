using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_TCP_client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Functions
    {
        private TcpClient clientSocket = new TcpClient();
        private NetworkStream stream;
        private bool isTable = false;
        private bool isConnecting = false;

        public string Connect()
        {
            string status;
            try
            {
                if (!clientSocket.ConnectAsync("193.166.72.3", 4360).Wait(5000)) // change the ip and the port to match the server's ip and listening port (default port is 4360)
                {
                    status = "Connection error";
                }
                else
                {
                    status = "Connected to server";
                    stream = clientSocket.GetStream();
                }
            }
            catch (Exception ex)
            {
                status = "Connection error";
            }

            isConnecting = true;
            return status;
        }

        public string Disconnect()
        {
            stream.Close();
            clientSocket.Close();
            return "Status - disconnected";
        }

        public string Send(string command, DataGridView dgvTable)
        {
            if(command[0] == '1')
            {
                isTable = true;
            }
            else
            {
                isTable = false;
            }

            string status = "";

            try
            {
                string[] headerSplit;
                int packets = 0;
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(command + '\0');
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
                byte[] inStream = new byte[2048];
                stream.Read(inStream, 0, inStream.Length);
                string returnData = System.Text.Encoding.ASCII.GetString(inStream);
                headerSplit = returnData.Split('|');
                if (int.TryParse(headerSplit[0], out packets))
                {
                    returnData = headerSplit[1];
                    for (int i = 0; i < packets-1; i++)
                    {
                        inStream = new byte[2048];
                        stream.Read(inStream, 0, inStream.Length);
                        returnData += System.Text.Encoding.ASCII.GetString(inStream);
                    }
                }

                if (!isConnecting)
                {
                    handleData(returnData, dgvTable);
                }
                else
                {
                    isConnecting = false;
                }
            }
            catch (Exception ex)
            {
                status = "An error occurred.";
            }

            return status;
        }

        public void handleData(string data, DataGridView dgvTable)
        {
            dgvTable.Rows.Clear();
            dgvTable.Columns.Clear();
            string[] columns, rows;
            List<string[]> cells = new List<string[]>();

            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            if(data != "" && !isTable)
            {
                int colIndex = dgvTable.Columns.Add(col);
                columns = data.Split('&');
                col.HeaderText = "Tables";
                for (int i = 0; i < columns.Count(); i++)
                {
                    dgvTable.Rows.Add(columns[i]);
                }
            }
            else
            {
                rows = data.Split('&');

                foreach (string row in rows)
                {
                    columns = row.Split(';');
                    cells.Add(columns);
                }

                for (int i = 0; i < cells[0].Count(); i++)
                {
                    col = new DataGridViewTextBoxColumn();
                    int colIndex = dgvTable.Columns.Add(col);
                    col.HeaderText = cells[0][i];
                }

                for (int i = 1; i < cells.Count(); i++)
                {
                    dgvTable.Rows.Add();
                    for (int j = 0; j < cells[i].Count(); j++)
                    {
                        dgvTable[j, i-1].Value = cells[i][j];
                    }
                }
            }
            dgvTable.Refresh();
        }
    }
}

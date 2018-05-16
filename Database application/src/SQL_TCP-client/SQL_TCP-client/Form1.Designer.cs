namespace SQL_TCP_client
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.dgvTable = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnShowTbl = new System.Windows.Forms.Button();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbDb = new System.Windows.Forms.TextBox();
            this.tbUname = new System.Windows.Forms.TextBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblDb = new System.Windows.Forms.Label();
            this.lblUname = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(335, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(109, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(335, 30);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(109, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect and exit";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // dgvTable
            // 
            this.dgvTable.AllowUserToAddRows = false;
            this.dgvTable.AllowUserToDeleteRows = false;
            this.dgvTable.AllowUserToOrderColumns = true;
            this.dgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTable.Location = new System.Drawing.Point(12, 119);
            this.dgvTable.Name = "dgvTable";
            this.dgvTable.ReadOnly = true;
            this.dgvTable.Size = new System.Drawing.Size(1135, 509);
            this.dgvTable.TabIndex = 2;
            this.dgvTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTable_CellDoubleClick);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(450, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(110, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status - disconnected";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(450, 35);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(54, 13);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "Error label";
            // 
            // btnShowTbl
            // 
            this.btnShowTbl.Enabled = false;
            this.btnShowTbl.Location = new System.Drawing.Point(12, 90);
            this.btnShowTbl.Name = "btnShowTbl";
            this.btnShowTbl.Size = new System.Drawing.Size(75, 23);
            this.btnShowTbl.TabIndex = 7;
            this.btnShowTbl.Text = "Show tables";
            this.btnShowTbl.UseVisualStyleBackColor = true;
            this.btnShowTbl.Click += new System.EventHandler(this.btnShowTbl_Click);
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(62, 6);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(100, 20);
            this.tbServer.TabIndex = 8;
            // 
            // tbDb
            // 
            this.tbDb.Location = new System.Drawing.Point(62, 32);
            this.tbDb.Name = "tbDb";
            this.tbDb.Size = new System.Drawing.Size(100, 20);
            this.tbDb.TabIndex = 9;
            // 
            // tbUname
            // 
            this.tbUname.Location = new System.Drawing.Point(229, 6);
            this.tbUname.Name = "tbUname";
            this.tbUname.Size = new System.Drawing.Size(100, 20);
            this.tbUname.TabIndex = 10;
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(229, 32);
            this.tbPass.Name = "tbPass";
            this.tbPass.Size = new System.Drawing.Size(100, 20);
            this.tbPass.TabIndex = 11;
            this.tbPass.UseSystemPasswordChar = true;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(9, 9);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 12;
            this.lblServer.Text = "Server";
            // 
            // lblDb
            // 
            this.lblDb.AutoSize = true;
            this.lblDb.Location = new System.Drawing.Point(9, 35);
            this.lblDb.Name = "lblDb";
            this.lblDb.Size = new System.Drawing.Size(53, 13);
            this.lblDb.TabIndex = 13;
            this.lblDb.Text = "Database";
            // 
            // lblUname
            // 
            this.lblUname.AutoSize = true;
            this.lblUname.Location = new System.Drawing.Point(168, 9);
            this.lblUname.Name = "lblUname";
            this.lblUname.Size = new System.Drawing.Size(55, 13);
            this.lblUname.TabIndex = 14;
            this.lblUname.Text = "Username";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(168, 35);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(53, 13);
            this.lblPass.TabIndex = 15;
            this.lblPass.Text = "Password";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 640);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.lblUname);
            this.Controls.Add(this.lblDb);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.tbUname);
            this.Controls.Add(this.tbDb);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.btnShowTbl);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.dgvTable);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.DataGridView dgvTable;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnShowTbl;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbDb;
        private System.Windows.Forms.TextBox tbUname;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.Label lblUname;
        private System.Windows.Forms.Label lblPass;
    }
}


namespace TCP_client
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblReceive = new System.Windows.Forms.Label();
            this.lblSend = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbReceive = new System.Windows.Forms.TextBox();
            this.tbValue1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubsctract = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblOperation = new System.Windows.Forms.Label();
            this.tbValue2 = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(27, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(110, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status - disconnected";
            // 
            // lblReceive
            // 
            this.lblReceive.AutoSize = true;
            this.lblReceive.Location = new System.Drawing.Point(12, 32);
            this.lblReceive.Name = "lblReceive";
            this.lblReceive.Size = new System.Drawing.Size(62, 13);
            this.lblReceive.TabIndex = 1;
            this.lblReceive.Text = "From server";
            // 
            // lblSend
            // 
            this.lblSend.AutoSize = true;
            this.lblSend.Location = new System.Drawing.Point(12, 173);
            this.lblSend.Name = "lblSend";
            this.lblSend.Size = new System.Drawing.Size(52, 13);
            this.lblSend.TabIndex = 2;
            this.lblSend.Text = "To server";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(12, 311);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(99, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send message";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbReceive
            // 
            this.tbReceive.Location = new System.Drawing.Point(12, 62);
            this.tbReceive.Multiline = true;
            this.tbReceive.Name = "tbReceive";
            this.tbReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReceive.Size = new System.Drawing.Size(397, 57);
            this.tbReceive.TabIndex = 4;
            // 
            // tbValue1
            // 
            this.tbValue1.Location = new System.Drawing.Point(15, 224);
            this.tbValue1.Multiline = true;
            this.tbValue1.Name = "tbValue1";
            this.tbValue1.Size = new System.Drawing.Size(125, 23);
            this.tbValue1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "---Sending an empty line will close the program---";
            // 
            // btnSubsctract
            // 
            this.btnSubsctract.Location = new System.Drawing.Point(151, 245);
            this.btnSubsctract.Name = "btnSubsctract";
            this.btnSubsctract.Size = new System.Drawing.Size(75, 23);
            this.btnSubsctract.TabIndex = 7;
            this.btnSubsctract.Text = "-";
            this.btnSubsctract.UseVisualStyleBackColor = true;
            this.btnSubsctract.Click += new System.EventHandler(this.btnSubsctract_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(151, 199);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblOperation
            // 
            this.lblOperation.AutoSize = true;
            this.lblOperation.Location = new System.Drawing.Point(182, 227);
            this.lblOperation.Name = "lblOperation";
            this.lblOperation.Size = new System.Drawing.Size(13, 13);
            this.lblOperation.TabIndex = 9;
            this.lblOperation.Text = "+";
            // 
            // tbValue2
            // 
            this.tbValue2.Location = new System.Drawing.Point(232, 224);
            this.tbValue2.Name = "tbValue2";
            this.tbValue2.Size = new System.Drawing.Size(121, 20);
            this.tbValue2.TabIndex = 10;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(12, 146);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 340);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.tbValue2);
            this.Controls.Add(this.lblOperation);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSubsctract);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbValue1);
            this.Controls.Add(this.tbReceive);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblSend);
            this.Controls.Add(this.lblReceive);
            this.Controls.Add(this.lblStatus);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblReceive;
        private System.Windows.Forms.Label lblSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbReceive;
        private System.Windows.Forms.TextBox tbValue1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubsctract;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblOperation;
        private System.Windows.Forms.TextBox tbValue2;
        private System.Windows.Forms.Label lblError;
    }
}


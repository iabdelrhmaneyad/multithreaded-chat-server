namespace ClinetGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.MessageList = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(14, 29);
            this.txtIpAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(153, 27);
            this.txtIpAddress.TabIndex = 0;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(174, 29);
            this.txtPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(114, 27);
            this.txtPort.TabIndex = 1;
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(14, 321);
            this.textMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(274, 27);
            this.textMessage.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(295, 29);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(86, 31);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // MessageList
            // 
            this.MessageList.FormattingEnabled = true;
            this.MessageList.ItemHeight = 20;
            this.MessageList.Location = new System.Drawing.Point(14, 68);
            this.MessageList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MessageList.Name = "MessageList";
            this.MessageList.Size = new System.Drawing.Size(274, 244);
            this.MessageList.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 321);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 31);
            this.button1.TabIndex = 5;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(294, 171);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 27);
            this.textBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Send to Client No.";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(607, 319);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 31);
            this.button2.TabIndex = 8;
            this.button2.Text = "choose Img";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(506, 87);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(157, 217);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(506, 49);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(156, 27);
            this.textBox2.TabIndex = 11;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 600);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MessageList);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIpAddress);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtIpAddress;
        private TextBox txtPort;
        private TextBox textMessage;
        private Button btnConnect;
        private ListBox MessageList;
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private Button button2;
        private PictureBox pictureBox1;
        private TextBox textBox2;
    }
}
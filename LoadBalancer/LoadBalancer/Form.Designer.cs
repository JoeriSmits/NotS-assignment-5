using System.Windows.Forms;

namespace LoadBalancer
{
    partial class Form
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
            this.serversLst = new System.Windows.Forms.ListBox();
            this.serversLst_Label = new System.Windows.Forms.Label();
            this.LoadBalancer_Label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.port_Label = new System.Windows.Forms.Label();
            this.startStop_btn = new System.Windows.Forms.Button();
            this.method_Label = new System.Windows.Forms.Label();
            this.method_ComboBox = new System.Windows.Forms.ComboBox();
            this.addServer_txtBox = new System.Windows.Forms.TextBox();
            this.addServer_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serversLst
            // 
            this.serversLst.FormattingEnabled = true;
            this.serversLst.ItemHeight = 25;
            this.serversLst.Items.AddRange(new object[] {
            "http://localhost:8081",
            "http://localhost:8082",
            "http://localhost:8083"});
            this.serversLst.Location = new System.Drawing.Point(19, 60);
            this.serversLst.Name = "serversLst";
            this.serversLst.Size = new System.Drawing.Size(557, 429);
            this.serversLst.Sorted = true;
            this.serversLst.TabIndex = 0;
            this.serversLst.DoubleClick += new System.EventHandler(this.serversLst_DoubleClick);
            this.serversLst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serversLst_KeyDown);
            // 
            // serversLst_Label
            // 
            this.serversLst_Label.AutoSize = true;
            this.serversLst_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.serversLst_Label.Location = new System.Drawing.Point(12, 9);
            this.serversLst_Label.Name = "serversLst_Label";
            this.serversLst_Label.Size = new System.Drawing.Size(175, 37);
            this.serversLst_Label.TabIndex = 1;
            this.serversLst_Label.Text = "Server Lijst";
            // 
            // LoadBalancer_Label
            // 
            this.LoadBalancer_Label.AutoSize = true;
            this.LoadBalancer_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LoadBalancer_Label.Location = new System.Drawing.Point(808, 9);
            this.LoadBalancer_Label.Name = "LoadBalancer_Label";
            this.LoadBalancer_Label.Size = new System.Drawing.Size(224, 37);
            this.LoadBalancer_Label.TabIndex = 2;
            this.LoadBalancer_Label.Text = "Load Balancer";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(736, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(296, 31);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "8080";
            // 
            // port_Label
            // 
            this.port_Label.AutoSize = true;
            this.port_Label.Location = new System.Drawing.Point(643, 63);
            this.port_Label.Name = "port_Label";
            this.port_Label.Size = new System.Drawing.Size(51, 25);
            this.port_Label.TabIndex = 4;
            this.port_Label.Text = "Port";
            // 
            // startStop_btn
            // 
            this.startStop_btn.Location = new System.Drawing.Point(736, 106);
            this.startStop_btn.Name = "startStop_btn";
            this.startStop_btn.Size = new System.Drawing.Size(296, 62);
            this.startStop_btn.TabIndex = 5;
            this.startStop_btn.Text = "Start LoadBalancer";
            this.startStop_btn.UseVisualStyleBackColor = true;
            this.startStop_btn.Click += new System.EventHandler(this.startStop_btn_Click);
            // 
            // method_Label
            // 
            this.method_Label.AutoSize = true;
            this.method_Label.Location = new System.Drawing.Point(598, 188);
            this.method_Label.Name = "method_Label";
            this.method_Label.Size = new System.Drawing.Size(96, 25);
            this.method_Label.TabIndex = 6;
            this.method_Label.Text = "Methode";
            // 
            // method_ComboBox
            // 
            this.method_ComboBox.FormattingEnabled = true;
            this.method_ComboBox.Items.AddRange(new object[] {
            "Session Based",
            "Cookie Based",
            "Round Robin",
            "Random",
            "Load"});
            this.method_ComboBox.Location = new System.Drawing.Point(736, 185);
            this.method_ComboBox.Name = "method_ComboBox";
            this.method_ComboBox.Size = new System.Drawing.Size(296, 33);
            this.method_ComboBox.TabIndex = 7;
            this.method_ComboBox.SelectedIndexChanged += new System.EventHandler(this.method_ComboBox_SelectedIndexChanged);
            // 
            // addServer_txtBox
            // 
            this.addServer_txtBox.Location = new System.Drawing.Point(603, 292);
            this.addServer_txtBox.Name = "addServer_txtBox";
            this.addServer_txtBox.Size = new System.Drawing.Size(236, 31);
            this.addServer_txtBox.TabIndex = 8;
            // 
            // addServer_btn
            // 
            this.addServer_btn.Location = new System.Drawing.Point(845, 292);
            this.addServer_btn.Name = "addServer_btn";
            this.addServer_btn.Size = new System.Drawing.Size(187, 43);
            this.addServer_btn.TabIndex = 9;
            this.addServer_btn.Text = "Add Server";
            this.addServer_btn.UseVisualStyleBackColor = true;
            this.addServer_btn.Click += new System.EventHandler(this.addServer_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(598, 251);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "Add server with port";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 507);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addServer_btn);
            this.Controls.Add(this.addServer_txtBox);
            this.Controls.Add(this.method_ComboBox);
            this.Controls.Add(this.method_Label);
            this.Controls.Add(this.startStop_btn);
            this.Controls.Add(this.port_Label);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.LoadBalancer_Label);
            this.Controls.Add(this.serversLst_Label);
            this.Controls.Add(this.serversLst);
            this.Name = "Form";
            this.Text = " ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox serversLst;
        private System.Windows.Forms.Label serversLst_Label;
        private System.Windows.Forms.Label LoadBalancer_Label;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label port_Label;
        private System.Windows.Forms.Button startStop_btn;
        private System.Windows.Forms.Label method_Label;
        private System.Windows.Forms.ComboBox method_ComboBox;
        private System.Windows.Forms.TextBox addServer_txtBox;
        private System.Windows.Forms.Button addServer_btn;
        private System.Windows.Forms.Label label1;
    }
}


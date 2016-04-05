using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancer
{
    public partial class Form : System.Windows.Forms.Form
    { 
        private int port = 8080;
        public Form()
        {
            InitializeComponent();
            Algoritme.algoritme = method_ComboBox.SelectedItem;
            HealthMonitor.serversLst = serversLst;
        }

        private ListBox ServersLst
        {
            get { return serversLst; }
            set { serversLst = value; }
        }

        private void startStop_btn_Click(object sender, EventArgs e)
        {
            var loadBalancer = new Listener(port, ServersLst);
            loadBalancer.StartListener();
        }

        private void method_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Algoritme.algoritme = method_ComboBox.SelectedItem;
        }

        private void addServer_btn_Click(object sender, EventArgs e)
        {
            if (addServer_txtBox.Text != null || addServer_txtBox.Text != "")
            {
                serversLst.Items.Add(addServer_txtBox.Text); 
            }
        }

        private void serversLst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(serversLst);
                selectedItems = serversLst.SelectedItems;

                if (serversLst.SelectedIndex != -1 && serversLst.Items.Count > 1)
                {
                    for (int i = selectedItems.Count - 1; i >= 0; i--)
                        serversLst.Items.Remove(selectedItems[i]);
                }
            }
        }

        private void serversLst_DoubleClick(object sender, EventArgs e)
        {
            if (HealthMonitor.servers != null)
            {
                foreach (var server in HealthMonitor.servers)
                {
                    if (server[0] == (string) serversLst.SelectedItem)
                    {
                        MessageBox.Show(server[1] + " Requests on this server since the load balancer started");
                    }
                }
            }
        }
    }
}

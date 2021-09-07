using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear
{
    public partial class orden_columnas : Form
    {
        public orden_columnas()
        {
            InitializeComponent();
        }

        private void orden_columnas_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(10);
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
        }
    }
}

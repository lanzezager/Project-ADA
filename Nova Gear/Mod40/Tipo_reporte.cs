using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear.Mod40
{
    public partial class Tipo_reporte : Form
    {
        public Tipo_reporte()
        {
            InitializeComponent();
        }

        int actual = 0;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                actual = 1; //reporte oficial
                this.Dispose();
            }

            if (radioButton2.Checked == true)
            {
                actual = 2;//reporte informal
                this.Dispose();
            }
        }

        public int mandar()
        {
            return actual;
        }

        private void Tipo_reporte_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            radioButton2.Checked = true;
        }
    }
}

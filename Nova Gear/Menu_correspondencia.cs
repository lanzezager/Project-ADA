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
    public partial class Menu_correspondencia : Form
    {
        public Menu_correspondencia()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Correspondencia_acta_citatorio actas = new Correspondencia_acta_citatorio();
            actas.Show();
            this.Hide();
            actas.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Correspondencia_V2 actas = new Correspondencia_V2();
            //Correspondencia_acta_citatorio actas = new Correspondencia_acta_citatorio();
            actas.Show();
            this.Hide();
            actas.Focus();
        }

        private void Menu_correspondencia_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
        }
    }
}

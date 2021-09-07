using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear.Aclaraciones
{
    public partial class Menu_Aclaraciones : Form
    {
        public Menu_Aclaraciones()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Aclaraciones.Marcar_Aclaraciones marca_aclara = new Aclaraciones.Marcar_Aclaraciones();
            marca_aclara.Show();
            marca_aclara.Focus();
        }

        private void Menu_Aclaraciones_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Aclaraciones.Consulta_aclaraciones consulta = new Consulta_aclaraciones();
            consulta.Show();
            consulta.Focus();
        }
    }
}

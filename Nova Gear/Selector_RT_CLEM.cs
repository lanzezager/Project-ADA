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
    public partial class Selector_RT_CLEM : Form
    {
        public Selector_RT_CLEM()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lector_Fac.tipo_anual = 1;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Lector_Fac.tipo_anual = 2;
            this.Close();
        }
    }
}

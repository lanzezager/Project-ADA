using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear.Depuracion
{
    public partial class Sel_archivo : Form
    {
        public Sel_archivo()
        {
            InitializeComponent();
        }
        int seleccion = 0;

        public int mandar()
        {
            return seleccion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                seleccion = 1;
                this.Dispose();
            }
            else
            {
                if (radioButton2.Checked == true)
                {
                    seleccion = 2;
                    this.Dispose();
                } 
            }
        }

        private void Sel_archivo_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            seleccion = 0;
        }
    }
}

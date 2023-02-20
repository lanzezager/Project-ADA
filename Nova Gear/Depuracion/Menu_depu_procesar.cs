using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Depuracion
{
    public partial class Menu_depu_procesar : Form
    {
        public Menu_depu_procesar()
        {
            InitializeComponent();
        }

        Conexion conex = new Conexion();
        DataTable anti_dupli = new DataTable();

        private void Menu_depu_procesar_Load(object sender, EventArgs e)
        {
            conex.conectar("base_principal");
            anti_dupli = conex.consultar("SELECT fecha_ingreso FROM procesar ORDER BY fecha_pago DESC LIMIT 1");
            conex.cerrar();
            String fecha = anti_dupli.Rows[0][0].ToString().Substring(0, 10);
            label8.Text = "Último Ingresado: " + fecha;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Depuración2.menu_pro_res = 1;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Depuración2.menu_pro_res = 2;
            this.Close();
        }

        private void Menu_depu_procesar_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}

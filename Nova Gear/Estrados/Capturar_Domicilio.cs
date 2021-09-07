using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Estrados
{
    public partial class Capturar_Domicilio : Form
    {
        public Capturar_Domicilio()
        {
            InitializeComponent();
        }

        //Conexion MySQL
        Conexion conex = new Conexion();

        private void Capturar_Domicilio_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            
            if (MainForm.reg_pat_sindo.Length > 1)
            {
                if (MainForm.reg_pat_sindo.Length == 10 && MainForm.reg_pat_sindo.StartsWith("4"))
                {
                    textBox1.Text = "0"+MainForm.reg_pat_sindo;
                }
                else
                {
                    textBox1.Text = MainForm.reg_pat_sindo;
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int entra = 0;
            string mensaje = "";

            mensaje = "Los siguientes campos no pueden estar vacíos:\n";

            if (textBox1.Text.Length > 9)
            {
                entra++;
            }
            else
            {
                mensaje += "•Registro Patronal\n";
            }

            if (textBox2.Text.Length > 9)
            {
                entra++;
            }
            else
            {
                mensaje += "•R.F.C.\n";
            }

            if (textBox3.Text.Length > 5)
            {
                entra++;
            }
            else
            {
                mensaje += "•Razon Social\n";
            }

            if (textBox4.Text.Length > 9)
            {
                entra++;
            }
            else
            {
                mensaje += "•Actividad\n";
            }

            if (textBox5.Text.Length > 9)
            {
                entra++;
            }
            else
            {
                mensaje += "•Domicilio\n";
            }

            if (textBox6.Text.Length > 9)
            {
                entra++;
            }
            else
            {
                mensaje += "•Localidad\n";
            }

            if (textBox7.Text.Length > 4)
            {
                entra++;
            }
            else
            {
                mensaje += "•Codigo Postal\n";
            }


            if (entra == 7)
            {
                conex.conectar("base_principal");
                conex.consultar("INSERT INTO sindo (registro_patronal,rfc,nombre,actividad,domicilio,localidad,cp) VALUES("+
                                "\""+textBox1.Text+"\","+
                                "\""+textBox2.Text+"\","+
                                "\""+textBox3.Text+"\","+
                                "\""+textBox4.Text+"\","+
                                "\""+textBox5.Text+"\","+
                                "\""+textBox6.Text+"\","+
                                "\""+textBox7.Text+"\")");
                MessageBox.Show("Los datos fueron guardados correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
            }
            else
            {
                MessageBox.Show(mensaje, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void Capturar_Domicilio_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.reg_pat_sindo = "-";
        }
    }
}

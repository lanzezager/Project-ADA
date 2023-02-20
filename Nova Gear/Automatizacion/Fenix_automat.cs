using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Automatizacion
{
    public partial class Fenix_automat : Form
    {
        public Fenix_automat()
        {
            InitializeComponent();
        }

        FileStream fichero;

        String ruta;

        private void Fenix_automat_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix_automat/temp.LZ") == true)
            {
                System.IO.File.Delete(@"fenix_estrados/temp.LZ");
            }

            //Crear archivos nuevos
            fichero = System.IO.File.Create(@"fenix_automat/temp.LZ");
            ruta = fichero.Name;
            fichero.Close();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix8.bat";

            if (File.Exists(@""+bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 07.I_A.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix9.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 08.SISCOB.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix10.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 09.CRED_SALDADOS.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix11.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 10.PROSIP_GP.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix12.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 11.ACATAMIENTOS.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix13.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 12.Thunder_C-Rale.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            config_fenix fenix_config = new config_fenix(1);
            fenix_config.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix14.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start Fenix_PDF_Extract.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String bat_name = "fenix15.bat";

            if (File.Exists(@"" + bat_name) == true)
            {
                System.IO.File.Delete(@"" + bat_name);
            }

            StreamWriter wr1 = new StreamWriter(@"" + bat_name);
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start X_Zone.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"" + bat_name);
        }
    }
}

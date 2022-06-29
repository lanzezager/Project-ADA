
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

namespace Nova_Gear
{
    public partial class Fenix_estrados : Form
    {
        public Fenix_estrados()
        {
            InitializeComponent();
        }

        FileStream fichero;

        String ruta;

        private void Fenix_estrados_Load(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix_estrados/temp.LZ") == true)
            {
                System.IO.File.Delete(@"fenix_estrados/temp.LZ");
            }    

            //Crear archivos nuevos
            fichero = System.IO.File.Create(@"fenix_estrados/temp.LZ");         
            ruta = fichero.Name;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix1.bat") == true)
            {
                System.IO.File.Delete(@"fenix1.bat");
            } 

            StreamWriter wr1 = new StreamWriter(@"fenix1.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 00.Act-Two.exe");
            
            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix1.bat");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix2.bat") == true)
            {
                System.IO.File.Delete(@"fenix2.bat");
            }

            StreamWriter wr1 = new StreamWriter(@"fenix2.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 01.Fenix_Es_Extract-C.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix2.bat");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix3.bat") == true)
            {
                System.IO.File.Delete(@"fenix3.bat");
            }

            StreamWriter wr1 = new StreamWriter(@"fenix3.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 02.Fenix_Es_PDF_Stamp_K.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix3.bat");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix4.bat") == true)
            {
                System.IO.File.Delete(@"fenix4.bat");
            }

            StreamWriter wr1 = new StreamWriter(@"fenix4.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 03.Fenix_Es_N-Excel_K.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix4.bat");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix5.bat") == true)
            {
                System.IO.File.Delete(@"fenix5.bat");
            }

            StreamWriter wr1 = new StreamWriter(@"fenix5.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 04.Fenix_Es_Gold_MultiNavegador_K.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix5.bat");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix6.bat") == true)
            {
                System.IO.File.Delete(@"fenix6.bat");
            }

            StreamWriter wr1 = new StreamWriter(@"fenix6.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 05.Fenix_Es_Acuse_DLD.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix6.bat");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"fenix7.bat") == true)
            {
                System.IO.File.Delete(@"fenix7.bat");
            }

            StreamWriter wr1 = new StreamWriter(@"fenix7.bat");
            wr1.WriteLine("@echo off");
            wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
            wr1.WriteLine("cd " + ruta.Substring(0, (ruta.Length - 8)));
            wr1.WriteLine("start 06.For_Snarf.exe");

            wr1.Close();

            System.Diagnostics.Process.Start(@"fenix7.bat");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            config_fenix fenix_config = new config_fenix(0);
            fenix_config.ShowDialog();
        }
    }
}

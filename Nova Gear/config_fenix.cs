
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
    public partial class config_fenix : Form
    {
        public config_fenix(int tipo_load)
        {
            InitializeComponent();
            this.tipo_carga = tipo_load;
        }

        FileStream fichero;
        int tipo_carga;

        private void button26_Click(object sender, EventArgs e)
        {//GUARDAR
            if (tipo_carga == 0)
            {//Fenix Estrados
                if (File.Exists(@"fenix_estrados/fenix_config.lz") == true)
                {
                    System.IO.File.Delete(@"fenix_estrados/fenix_config.lz");
                }

                StreamWriter wr1 = new StreamWriter(@"fenix_estrados/fenix_config.lz");

                for (int i = 0; i < textBox1.Lines.Length; i++)
                {
                    wr1.WriteLine(textBox1.Lines[i].ToString());
                }

                wr1.Close();
            }
            else
            {//Fenix Automatización (Automat)
                if (File.Exists(@"fenix_automat/fenix_config.lz") == true)
                {
                    System.IO.File.Delete(@"fenix__automat/fenix_config.lz");
                }

                StreamWriter wr1 = new StreamWriter(@"fenix_automat/fenix_config.lz");

                for (int i = 0; i < textBox1.Lines.Length; i++)
                {
                    wr1.WriteLine(textBox1.Lines[i].ToString());
                }

                wr1.Close();
            }

            MessageBox.Show("Archivo Guardado Exitosamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void config_fenix_Load(object sender, EventArgs e)
        {
           if(tipo_carga==0){//Fenix Estrados
                if (File.Exists(@"fenix_estrados/fenix_config.lz") == true)
                {
                    StreamReader rdr1 = new StreamReader(@"fenix_estrados/fenix_config.lz");
                
                    while(rdr1.EndOfStream != true){
                        textBox1.AppendText(rdr1.ReadLine());
                        textBox1.AppendText("\r\n");
                    }

                    rdr1.Close();
                }
            }else{//Fenix Automatización (Automat)
                if (File.Exists(@"fenix_automat/fenix_config.lz") == true)
                {
                    StreamReader rdr1 = new StreamReader(@"fenix_automat/fenix_config.lz");

                    while (rdr1.EndOfStream != true)
                    {
                        textBox1.AppendText(rdr1.ReadLine());
                        textBox1.AppendText("\r\n");
                    }

                    rdr1.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult resul = MessageBox.Show("Se procederá a crear los Directorios listados.\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (resul == DialogResult.Yes)
            {
                for (int i = 0; i < textBox1.Lines.Length; i++)
                {
                    try{
                           if (Directory.Exists(@"" + textBox1.Lines[i].ToString()) == false)
                           {
                       
                                   if (textBox1.Lines[i].ToString().Length > 0)
                                   {
                                       System.IO.Directory.CreateDirectory(@"" + textBox1.Lines[i].ToString());
                                   }
                       
                           }
                       }catch (Exception ex){
                       }
                }

                MessageBox.Show("Proceso concluido exitosamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace Nova_Gear.Depuracion
{
    public partial class Opcs_depu : Form
    {
        public Opcs_depu()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void Opcs_depu_Load(object sender, EventArgs e)
        {
             String verif_rale="", pago_min="";
             try
             {
                 StreamReader rdr1 = new StreamReader(@"opcs_depu.lz");
                 verif_rale = rdr1.ReadLine();
                 pago_min = rdr1.ReadLine();
                 rdr1.Close();

                 verif_rale = verif_rale.Substring(11,verif_rale.Length-11);
                 pago_min = pago_min.Substring(9, pago_min.Length - 9);



                 if (verif_rale == "1")
                 {
                     radioButton3.Checked = true;
                     radioButton4.Checked = false;
                 }
                 else
                 {
                     if (verif_rale == "0")
                     {
                         radioButton3.Checked = false;
                         radioButton4.Checked = true;
                     }
                 }

                 textBox1.Text = pago_min;
                 trackBar1.Value = Convert.ToInt32(pago_min);
             }
             catch (Exception error)
             {
                 MessageBox.Show("Ha ocurrido un error al leer el archivo de opciones de depuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);                
             }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String ruta;
            FileStream fichero;

            try
            {
                //Borrar archivos para comenzar de 0
                if (File.Exists(@"opcs_depu.lz") == true)
                {
                    System.IO.File.Delete(@"opcs_depu.lz");
                }
                    //Crear archivos nuevos
                    fichero = System.IO.File.Create(@"opcs_depu.lz");

                    ruta = fichero.Name;
                    fichero.Close();

                    //Abrir archivo
                    StreamWriter wr = new StreamWriter(@"opcs_depu.lz");
                    if(radioButton3.Checked==true){
                        wr.WriteLine("Verif_Rale:1");
                    }else{
                        if (radioButton4.Checked == true)
                        {
                            wr.WriteLine("Verif_Rale:0");
                        }
                    }
                    
                    wr.WriteLine("Pago_min:" + textBox1.Text);                   
                    wr.Close();

                    MessageBox.Show("Se guardaron las opciones correctamente", "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al guardar las opciones", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

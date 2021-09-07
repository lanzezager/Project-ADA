using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Nova_Gear.Mod40
{
    public partial class Datos_inf40 : Form
    {
        public Datos_inf40()
        {
            InitializeComponent();
        }

        int tipo_rep;
        String fecha, fecha_formal;
        String del, del_num, mpio, sub, sub_num, subdele, jefe_cob, jefe_emi, jefe_afi, ref_baja, ref_ofi;
        Conexion conex = new Conexion();
        
        public void leer_config(int tipo_r)
        {
           this.tipo_rep = tipo_r;
            try
            {
                StreamReader rdr = new StreamReader(@"mod40_info_config.lz");

                /*del = rdr.ReadLine();
                del_num = rdr.ReadLine();
                mpio = rdr.ReadLine();
                sub = rdr.ReadLine();
                sub_num = rdr.ReadLine();
                subdele = rdr.ReadLine();
                jefe_cob = rdr.ReadLine();
                jefe_emi = rdr.ReadLine();
                jefe_afi = rdr.ReadLine();*/
                ref_baja = rdr.ReadLine();
                ref_ofi = rdr.ReadLine();
                rdr.Close();

                /*del = del.Substring(11, del.Length - 11);
                del_num = del_num.Substring(15, del_num.Length - 15);
                mpio = mpio.Substring(10, mpio.Length - 10);
                sub = sub.Substring(14, sub.Length - 14);
                sub_num = sub_num.Substring(18, sub_num.Length - 18);
                subdele = subdele.Substring(12, subdele.Length - 12);
                jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
                jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
                jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);*/
                ref_baja = ref_baja.Substring(9, ref_baja.Length - 9);
                ref_ofi = ref_ofi.Substring(8, ref_ofi.Length - 8);

                comboBox1.Items.Add(conex.leer_config_sub()[5]);//subdelegado
                comboBox1.Items.Add(conex.leer_config_sub()[7]);//jefe_cob)
                comboBox1.Items.Add(conex.leer_config_sub()[8]);//jefe_emi
                

                if (tipo_r == 1)
                {
                    label1.Text = "Datos específicos\npara el informe Oficial";
                    textBox1.Text = ref_ofi;
                }
                else
                {
                    if (tipo_r == 2)
                    {
                        label1.Text = "Datos específicos\npara el informe de Baja";
                        textBox1.Text = ref_baja;
                        dateTimePicker1.Enabled = false;
                        comboBox1.Enabled = false;
                    }
                }

            }
            catch (Exception error)
            {
                //MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        
        public void guardar_config(){
        	try
            {
                //Borrar archivo existente
                System.IO.File.Delete(@"mod40_info_config.lz");
                //Abrir archivo
                StreamWriter wr = new StreamWriter(@"mod40_info_config.lz");

                /*wr.WriteLine("delegacion:" + del);
                wr.WriteLine("num_delegacion:" + del_num);
                wr.WriteLine("municipio:" + mpio);
                wr.WriteLine("subdelegacion:" + sub);
                wr.WriteLine("num_subdelegacion:" + sub_num );
                wr.WriteLine("subdelegado:" + subdele);
                wr.WriteLine("jefe_cob:" + jefe_cob);
                wr.WriteLine("jefe_epo:" + jefe_emi);
                wr.WriteLine("jefe_afi_vig:" + jefe_afi);*/
                if(tipo_rep==1){
                	wr.WriteLine("ref_baja:" +  ref_baja);
                	wr.WriteLine("ref_ofi:" + textBox1.Text);
                }else{
                	if(tipo_rep==2){
	                	wr.WriteLine("ref_baja:" + textBox1.Text);
	                	wr.WriteLine("ref_ofi:" + ref_ofi);
                	}
                }
                
                wr.WriteLine("DON'T CHANGE THIS SETTINGS!!!!!");
                wr.WriteLine("By LZ");
                wr.WriteLine("Arriba el Atlas!!!!!");
                wr.Close();
                //MessageBox.Show("Ajustes Guardados correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo guardar la configuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
        //informe oficial
        public string[] regresar(){
            string[] datos_ofi = new string[4];

            fecha_formal = dateTimePicker1.Text;
            fecha = fecha_formal;

            if (fecha.Substring(3, 2).Equals("01"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Enero de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("02"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Febrero de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("03"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Marzo de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("04"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Abril de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("05"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Mayo de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("06"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Junio de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("07"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Julio de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("08"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Agosto de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("09"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Septiembre de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("10"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Octubre de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("11"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Noviembre de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("12"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Diciembre de " + fecha.Substring(6, 4);
            }

            datos_ofi[0] = fecha_formal;//fecha completa
            datos_ofi[1] = comboBox1.Text;//nombre jefe


            if (comboBox1.SelectedIndex == 0)
            {
                datos_ofi[2] = "Titular de la Subdelegación";//puesto jefe
            }
            else
            {
                if (comboBox1.SelectedIndex == 1)
                {
                    datos_ofi[2] = "Jefe del Departamento de Cobranza";//puesto jefe
                }
                else
                {
                    if (comboBox1.SelectedIndex == 2)
                    {
                        datos_ofi[2] = "Jefe de Oficina de Emisión y Pago Oportuno";//puesto jefe
                    }
                }
            }

            datos_ofi[3] = textBox1.Text;//folio ref

            return datos_ofi;
 
        }
        //informe baja
        public string[] regresar1()
        {
            string[] datos_ofi = new string[1];

            datos_ofi[0] = textBox1.Text;//folio ref

            return datos_ofi;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            guardar_config();
            this.Close();
        }

        private void Datos_inf40_Load(object sender, EventArgs e)
        {

            String window_name = this.Text;
            window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int candado = 0;

            if (comboBox1.SelectedIndex >= 0)
            {
                candado++;
            }

            if(textBox1.Text.Length>11){
                candado++;
            }

            if (tipo_rep == 1)
            {
                if (candado >= 2)
                {
                    button4.Enabled = true;
                }
                else
                {
                    button4.Enabled = false;
                }
            }
            else
            {
                if (candado >= 1)
                {
                    button4.Enabled = true;
                }
                else
                {
                    button4.Enabled = false;
                }
            }
            candado = 0;

        }
    
    }
}

/*
 * Creado por SharpDevelop.
 * Usuario: miguel.banuelos
 * Fecha: 21/03/2018
 * Hora: 10:34 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Nova_Gear.ops_subs
{
	/// <summary>
	/// Description of selector_sub.
	/// </summary>
	public partial class selector_sub : Form
	{
		public selector_sub(int orig)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.ori=orig;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		Conexion conex = new Conexion();
		
		int ori=0;
		public void cargar_info(){

            String del, no_del, muni, sub, no_sub, subdele, jefe_afi, jefe_cob, jefe_emi, sec_emi, jefe_cobro, info, dias_not;
        	
            try{
                StreamReader rdr1 = new StreamReader(@"sub_config.lz");
                del =rdr1.ReadLine();
                no_del = rdr1.ReadLine();
                muni = rdr1.ReadLine(); 
                sub = rdr1.ReadLine();
                no_sub = rdr1.ReadLine();
                subdele = rdr1.ReadLine();
                jefe_afi = rdr1.ReadLine();
                jefe_cob = rdr1.ReadLine();
                jefe_emi = rdr1.ReadLine();
                sec_emi = rdr1.ReadLine();
                jefe_cobro = rdr1.ReadLine();
                dias_not = rdr1.ReadLine();
                rdr1.Close();

                del = del.Substring(11,del.Length-11);
                no_del = no_del.Substring(7,no_del.Length-7);
                muni = muni.Substring(10, muni.Length - 10);
                sub = sub.Substring(14, sub.Length - 14);
                no_sub = no_sub.Substring(7, no_sub.Length - 7);
                subdele = subdele.Substring(12, subdele.Length - 12);
                jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);
                jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
                jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
                sec_emi= sec_emi.Substring(14,sec_emi.Length-14);
                jefe_cobro= jefe_cobro.Substring(12,jefe_cobro.Length-12);
                dias_not = dias_not.Substring(9, dias_not.Length - 9);

                info = del+"|"+no_del+"|"+muni+"|"+sub+"|"+no_sub+"|"+subdele+"|"+jefe_afi+"|"+jefe_cob+"|"+jefe_emi+"|"+sec_emi;
                //MessageBox.Show(info);
                /*String ruta;
                ruta = cado_con;
                ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';')-1) - (ruta.IndexOf('='))));
                ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
                MessageBox.Show(ruta);*/
                
                comboBox1.Text=del;
                comboBox5.Text=no_del;
                comboBox2.Text=muni;
                comboBox3.SelectedItem=sub;
                comboBox4.SelectedItem=no_sub;
                textBox1.Text=subdele;
                textBox2.Text=jefe_afi;
                textBox3.Text=jefe_cob;
                textBox4.Text=jefe_emi;
                textBox5.Text=sec_emi;
                textBox6.Text=jefe_cobro;
                textBox7.Text = dias_not;
            }catch(Exception error){
                MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de ubicación","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
		}

		public void guardar_info(){
			int guarda=0;
			
			if(ori==1){
				DialogResult re=MessageBox.Show("Para guardar los cambios se cerrará la sesión.\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
				
				if(re== DialogResult.Yes){
					guarda=1;
				}else{
					guarda=0;
				}
			}else{
				guarda=1;
			}
			
			if(guarda==1){
				try
				{
					//Borrar archivo existente
					System.IO.File.Delete(@"sub_config.lz");
					//Abrir archivo
					StreamWriter wr = new StreamWriter(@"sub_config.lz");

					wr.WriteLine("delegacion:" + comboBox1.SelectedItem);
					wr.WriteLine("no_del:" + comboBox5.SelectedItem);
					wr.WriteLine("municipio:" + comboBox2.SelectedItem);
					wr.WriteLine("subdelegacion:" + comboBox3.SelectedItem);
					wr.WriteLine("no_sub:" + comboBox4.SelectedItem);
					wr.WriteLine("subdelegado:" + textBox1.Text);
					wr.WriteLine("jefe_afi_vig:" + textBox2.Text);
					wr.WriteLine("jefe_cob:" + textBox3.Text);
					wr.WriteLine("jefe_epo:" + textBox4.Text);
					wr.WriteLine("jefe_secc_epo:" + textBox5.Text);
                    wr.WriteLine("jefe_cobros:" + textBox6.Text);
                    wr.WriteLine("dias_not:" + textBox7.Text);
					wr.WriteLine("DON'T CHANGE THIS SETTINGS!!!!!");
					wr.WriteLine("By LZ");
					wr.WriteLine("Arriba el Atlas!!!!!");
					wr.Close();
					MessageBox.Show("Ajustes Guardados correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
					//conex.leer_config_sub();
					this.Close();
				}
				catch (Exception error)
				{
					MessageBox.Show("No se pudo guardar la configuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			
			if(guarda==1 && ori==1){
				MainForm.cierra=1;
				this.Dispose();
				Application.Exit();
				System.Diagnostics.Process.Start("restart_ng.exe");
			}
		}
		
		void Selector_subLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			comboBox1.SelectedIndex=0;
			comboBox5.SelectedIndex=0;
			cargar_info();
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			int indice = comboBox3.SelectedIndex;
			
			switch(indice){
				case 0 :
					//hidalgo 38
					comboBox2.SelectedIndex=0;
					comboBox4.SelectedIndex=3;
				break;
				
				case 1 :
					//juarez 40
					comboBox2.SelectedIndex=0;
					comboBox4.SelectedIndex=5;
				break;
				
				case 2 :
					//reforma-libertad 39
					comboBox2.SelectedIndex=0;
					comboBox4.SelectedIndex=4;
				break;
				
				case 3 :
					//cd guzman 22
					comboBox2.SelectedIndex=1;
					comboBox4.SelectedIndex=2;
				break;
				
				case 4 :
					//ocotlan 15
					comboBox2.SelectedIndex=2;
					comboBox4.SelectedIndex=1;
				break;
				
				case 5 :
					//vallarta 50
					comboBox2.SelectedIndex=3;
					comboBox4.SelectedIndex=6;
				break;
				
				case 6 :
					//tepa 12
					comboBox2.SelectedIndex=4;
					comboBox4.SelectedIndex=0;
				break;
			}
		}
		
		void ComboBox4SelectedIndexChanged(object sender, EventArgs e)
		{
			/*int indice = comboBox4.SelectedIndex;
			
			switch(indice){
				case 0 :
					//hidalgo 38
					comboBox2.SelectedIndex=0;
					comboBox3.SelectedIndex=0;
				break;
				
				case 1 :
					//juarez 40
					comboBox2.SelectedIndex=0;
					comboBox3.SelectedIndex=1;
				break;
				
				case 2 :
					//reforma-libertad 39
					comboBox2.SelectedIndex=0;
					comboBox3.SelectedIndex=2;
				break;
				
				case 3 :
					//cd guzman 22
					comboBox2.SelectedIndex=1;
					comboBox3.SelectedIndex=3;
				break;
				
				case 4 :
					//ocotlan 15
					comboBox2.SelectedIndex=2;
					comboBox3.SelectedIndex=4;
				break;
				
				case 5 :
					//vallarta 50
					comboBox2.SelectedIndex=3;
					comboBox3.SelectedIndex=5;
				break;
				
				case 6 :
					//tepa 12
					comboBox2.SelectedIndex=4;
					comboBox3.SelectedIndex=6;
				break;
			}*/
		}
		
		void Button4Click(object sender, EventArgs e)
		{
            int verif = 0;

            if (comboBox3.SelectedIndex > -1)
            {
                verif++;
            }

            if (textBox1.Text.Length > 15)
            {
                verif++;
            }

            if (textBox2.Text.Length > 15)
            {
                verif++;
            }

            if (textBox3.Text.Length > 15)
            {
                verif++;
            }

            if (textBox4.Text.Length > 15)
            {
                verif++;
            }

            if (textBox5.Text.Length > 15)
            {
                verif++;
            }

            if (textBox6.Text.Length > 15)
            {
                verif++;
            }

            if(verif==7){

                guardar_info();
            }

		}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Nova_Gear
{
    public partial class Setup_mysql : Form
    {
        public Setup_mysql(int orig)
        {
            InitializeComponent();
            this.ori=orig;
        }

        String cad_con,bd_user,bd_pass,bd_server,bd_base;
        int bandera=0,ori;
        bool prueba;

        Conexion conex = new Conexion();

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bandera = 0;

            if (textBox1.Text.Length < 4)
            {
                bandera = 1;
            }

            if (textBox2.Text.Length < 4)
            {
                bandera = 1;
            }

            if (textBox3.Text.Length < 4)
            {
                bandera = 1;
            }

            if (textBox4.Text.Length < 4)
            {
                bandera = 1;
            }

            if (bandera == 0)
            {
                cad_con = @"server=" + textBox3.Text + "; userid=" + textBox1.Text + "; password=" + textBox2.Text + "; database=" + textBox4.Text + ";";
                
                if ((conex.probar(cad_con)) == true)
                {
                    MessageBox.Show("Conexión realizada exitosamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No se pudo realizar la conexión a la Base de Datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Ningún Campo puede quedar vacío y deben contener por lo menos 3 caracteres.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
        			System.IO.File.Delete(@"mysql_config.lz");
        			//Abrir archivo
        			StreamWriter wr = new StreamWriter(@"mysql_config.lz");

        			wr.WriteLine("user:" + textBox1.Text);
        			wr.WriteLine("password:" + textBox2.Text);
        			wr.WriteLine("server:" + textBox3.Text);
        			wr.WriteLine("database:" + textBox4.Text);
        			wr.WriteLine("DON'T CHANGE THIS SETTINGS!!!!!");
        			wr.WriteLine("By LZ");
        			wr.WriteLine("Arriba el Atlas!!!!!");
        			wr.Close();
        			MessageBox.Show("Ajustes Guardados correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			conex.leer_config();
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

        private void Setup_mysql_Load(object sender, EventArgs e)
        {
            leer_config();
        }

        public void leer_config()
        {
            try
            {
                StreamReader rdr = new StreamReader(@"mysql_config.lz");
                bd_user = rdr.ReadLine();
                bd_pass = rdr.ReadLine();
                bd_server = rdr.ReadLine();
                bd_base = rdr.ReadLine();
                rdr.Close();

                bd_user = bd_user.Substring(5, bd_user.Length - 5);
                bd_pass = bd_pass.Substring(9, bd_pass.Length - 9);
                bd_server = bd_server.Substring(7, bd_server.Length - 7);
                bd_base = bd_base.Substring(9, bd_base.Length - 9);

                textBox1.Text = bd_user;
                textBox2.Text = bd_pass;
                textBox3.Text = bd_server;
                textBox4.Text = bd_base;

            }
            catch (Exception error)
            {
               MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        void Label1Click(object sender, EventArgs e)
        {
        	
        }
    }
}

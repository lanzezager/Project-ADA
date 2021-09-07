/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 13/04/2018
 * Hora: 03:46 p. m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Config_backup.
	/// </summary>
	public partial class Config_backup : Form
	{
		public Config_backup()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String version, copyright,dont,lz,atlas,url,url_real;
		Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
		DataTable databases = new DataTable();
		DataTable tablas = new DataTable();
		
		public void genera_backup(){
			int i=0,j=0,z=0;
			FileStream fi;
			
			guarda_pass();
			
			conex.conectar("");
			databases=conex.consultar("SHOW DATABASES");			
			
			if(File.Exists(@"respaldo.bat")==true){
				System.IO.File.Delete(@"respaldo.bat");
				System.IO.File.Delete(@"arch.bat");
			}
			
			fi = File.Open(@"config.lz",FileMode.Open);
			url_real=fi.Name;
			fi.Close();
			
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"respaldo.bat");
			
			wr.WriteLine("@echo off");
			wr.WriteLine(""+textBox1.Text.Substring(0,2));
			wr.WriteLine("cd \""+textBox1.Text+"\"");
			wr.WriteLine("md Respaldo_BD_Nova_%Date:~6,4%%Date:~3,2%%Date:~0,2%");
			//wr.WriteLine("cd Respaldo_BD_Nova_%Date:~6,4%%Date:~3,2%%Date:~0,2%");
			wr.WriteLine(""+url_real.Substring(0,2));
			wr.WriteLine("cd \""+url_real.Substring(0,(url_real.Length-10))+"\"");
            //MessageBox.Show("" + databases.Rows.Count);
			while(i<databases.Rows.Count){
                //MessageBox.Show(databases.Rows[i][0].ToString());
                if(databases.Rows[i][0].ToString().Contains("schema")==true){
                    z=1;
                }

                if(databases.Rows[i][0].ToString().Contains("sql")==true){
                    z = 1;
                }
                
				if(z==0){
					conex2.conectar(databases.Rows[i][0].ToString());
					tablas=conex2.consultar("SHOW TABLES");
                    j = 0;
					while(j<tablas.Rows.Count){
                    	wr.WriteLine("mysqldump.exe --defaults-extra-file=\""+url_real+"\" --single-transaction \""+databases.Rows[i][0].ToString()+"\" \""+tablas.Rows[j][0].ToString()+"\" > \""+textBox1.Text+"\\"+"Respaldo_BD_Nova_%Date:~6,4%%Date:~3,2%%Date:~0,2%\\"+databases.Rows[i][0].ToString()+"_"+tablas.Rows[j][0].ToString()+".sql\"");
						j++;
					}
				}
				i++;
                z = 0;
			}
			
			wr.Close();
			
		}

		public void guarda_pass(){
			
			String user,pass,host;
			user=conex.leer_config_2()[0];
			pass=conex.leer_config_2()[1];
			host=conex.leer_config_2()[2];
			
			if(File.Exists(@"config.lz")==true){
				System.IO.File.Delete(@"config.lz");
			}
			
			//Abrir archivo
			StreamWriter wr1 = new StreamWriter(@"config.lz");
			
			wr1.WriteLine("[client]");
			wr1.WriteLine("user="+user+"");
			wr1.WriteLine("password="+pass+"");
			wr1.WriteLine("host="+host+"");
			wr1.Close();
		}
		
		public void agendar_tarea(String periodo, String hora){
			
			if(File.Exists(@"agendar_task.bat")==true){
				System.IO.File.Delete(@"agendar_task.bat");
			}
			
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"agendar_task.bat");
			
			wr.WriteLine("@echo off");
			if(periodo.Equals("diariamente")==true){
				wr.WriteLine("schtasks /create /TN Respaldo_BD_Nova /TR \""+url_real.Substring(0,(url_real.Length-10))+"\\backup_nova.exe\" /SC DAILY /ST "+hora+":00 /F");
				//wr.WriteLine("schtasks /create /TN Respaldo_BD_Nova /TR \""+url_real.Substring(0,(url_real.Length-9))+"\\backup_nova.exe\" /SC DAILY /ST "+hora+":00 /F");
			}
			
			if(periodo.Equals("semanalmente")==true){
				wr.WriteLine("schtasks /create /TN Respaldo_BD_Nova /TR \""+url_real.Substring(0,(url_real.Length-10))+"\\backup_nova.exe\" /SC WEEKLY /D FRI /ST "+hora+":00 /F");
				//wr.WriteLine("schtasks /create /TN Respaldo_BD_Nova /TR \""+url_real.Substring(0,(url_real.Length-9))+"\\backup_nova.exe\" /SC WEEKLY /D FRI /ST "+hora+":00 /F");
			}
			//wr.WriteLine("pause");
			wr.Close();
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			FolderBrowserDialog asigna_carpeta = new FolderBrowserDialog();
			asigna_carpeta.Description="Selecciona o crea la carpeta en la que deseas que se guarden los archivos de Respaldo:";
			
			DialogResult result = asigna_carpeta.ShowDialog();
			
			if(result == DialogResult.OK){
				textBox1.Text= asigna_carpeta.SelectedPath;
			}
		}
		
		void Config_backupLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			String dato_extra,ubicacion,periodi,hora;
			
			try{
				StreamReader rdr = new StreamReader(@"NG_info.lz");
				version = @""+rdr.ReadLine();
				url= rdr.ReadLine();
				ubicacion=rdr.ReadLine();
				periodi=rdr.ReadLine();
				hora=rdr.ReadLine();
				copyright = rdr.ReadLine();
				dont = rdr.ReadLine();
				lz = rdr.ReadLine();
				atlas = rdr.ReadLine();
				rdr.Close();
				
				ubicacion=ubicacion.Substring(11,ubicacion.Length-11);
				periodi=periodi.Substring(15,periodi.Length-15);
				hora=hora.Substring(12,hora.Length-12);
				
				textBox1.Text=ubicacion;
				if(periodi.Equals("diariamente")==true){
					radioButton1.Checked=true;
				}else{
					if(periodi.Equals("semanalmente")==true){
						radioButton2.Checked=true;
					}
				}
				comboBox1.SelectedItem=hora;
				
			}catch(Exception x){
				MessageBox.Show("Ocurrió un Error al momento de verificar la configuración del Respaldo:\n\n"+x,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
				textBox1.Text="";
				radioButton1.Checked=true;
				comboBox1.SelectedItem="15:50";
			}	
		}
		//guardar
		void Button2Click(object sender, EventArgs e)
		{
			String period="";
			int guarda=0;
			
			if(radioButton1.Checked==true){
				period="diariamente";
			}else{
				if(radioButton2.Checked==true){
					period="semanalmente";
				}
			}
			
			if(textBox1.Text.Length>2){
				guarda=1;
			}
			
			if(guarda==1){
				try{
					//Borrar archivo existente
	        			System.IO.File.Delete(@"NG_info.lz");
	        			//Abrir archivo
	        			StreamWriter wr = new StreamWriter(@"NG_info.lz");
	
	        			wr.WriteLine(version);
	        			wr.WriteLine(url);
	        			wr.WriteLine("Url_backup:" + textBox1.Text);
	        			wr.WriteLine("Periodo_backup:" + period);
	        			wr.WriteLine("Hora_backup:" + comboBox1.SelectedItem.ToString());
	        			wr.WriteLine(copyright);
	        			wr.WriteLine(dont);
	        			wr.WriteLine(lz);
	        			wr.WriteLine(atlas);
	        			wr.Close();
	        			
	        			genera_backup();
						agendar_tarea(period,comboBox1.SelectedItem.ToString());
						System.Diagnostics.Process.Start(@"agendar_task.bat");
						
	        			MessageBox.Show("Ajustes Guardados correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
	        			this.Close();
				}catch(Exception ex){
					MessageBox.Show("No se pudo guardar la configuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(textBox1.Text.Length>2){
				genera_backup();
				System.Diagnostics.Process.Start(@"respaldo.bat");
			}
		}
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 30/04/2018
 * Hora: 11:13 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;

namespace Nova_Gear.Sepomex
{
	/// <summary>
	/// Description of Menu_sepomex.
	/// </summary>
	public partial class Menu_sepomex : Form
	{
		public Menu_sepomex()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		Conexion conex = new Conexion();
		DataTable consulta = new DataTable();
		
		void Button1Click(object sender, EventArgs e)
		{
			Carga_sepo sepo_carga = new Carga_sepo();
			sepo_carga.Show();
			sepo_carga.Focus();
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			Generar_fac_sepo fac_sepo = new Generar_fac_sepo();
			fac_sepo.Show();
			fac_sepo.Focus();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Fecha_not_sepo fech_sepo = new Fecha_not_sepo();
			fech_sepo.Show();
			fech_sepo.Focus();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			Consulta_sepo cons_sepo = new Consulta_sepo();
			cons_sepo.Show();
			cons_sepo.Focus();
		}
		
		void Menu_sepomexLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			String rango;
			rango = MainForm.datos_user_static[2];
			
			if(Convert.ToInt32(rango)<3){
				button1.Enabled=true;
				button2.Enabled=true;
				button3.Enabled=true;
				button4.Enabled=true;
				button5.Enabled=true;
			}
			
			if(rango=="3"){
				//button1.Enabled=true;
				button2.Enabled=true;
				button3.Enabled=true;
				button4.Enabled=true;
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			Sectorizacion_sepo sect_sepo = new Sectorizacion_sepo();
			sect_sepo.Show();
			sect_sepo.Focus();
		}
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 03/03/2018
 * Hora: 01:16 a. m.
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

namespace Nova_Gear.Inventario
{
	/// <summary>
	/// Description of Menu_invs.
	/// </summary>
	public partial class Menu_invs : Form
	{
		public Menu_invs()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Menu_inventario menu1 = new Menu_inventario();
			menu1.Show();
			this.Hide();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Cartera.Menu_inv_c menu2 = new Cartera.Menu_inv_c();
			menu2.Show();
			this.Hide();
		}

		private void Menu_invs_Load(object sender, EventArgs e)
		{

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


		}
	}
}

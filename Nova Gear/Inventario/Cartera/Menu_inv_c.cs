/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 03/03/2018
 * Hora: 01:15 a. m.
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

namespace Nova_Gear.Inventario.Cartera
{
	/// <summary>
	/// Description of Menu_inv_c.
	/// </summary>
	public partial class Menu_inv_c : Form
	{
		public Menu_inv_c()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			opciones_inv_c opc_inv_c = new opciones_inv_c();
			opc_inv_c.Show();
		}
		
		void Menu_inv_cLoad(object sender, EventArgs e)
		{

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


		}
	}
}

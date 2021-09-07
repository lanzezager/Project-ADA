/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 08/09/2017
 * Hora: 11:57 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
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
using Microsoft.SqlServer.Types;
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;

namespace Nova_Gear.Depuracion
{
	/// <summary>
	/// Description of Grid_checador.
	/// </summary>
	public partial class Grid_checador : Form
	{
		public Grid_checador(DataTable tabla)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            tabla_checar = tabla;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        DataTable tabla_checar = new DataTable();
		void Grid_checadorLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			dataGridView1.DataSource = tabla_checar;
		}
	}
}

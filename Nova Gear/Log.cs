/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 11/05/2016
 * Hora: 03:56 p. m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Log.
	/// </summary>
	public partial class Log : Form
	{
		public Log(String filtro_)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.filtro=filtro_;
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		 Conexion conex = new Conexion();
		 String filtro;
		void LogLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			conex.conectar("base_principal");
			dataGridView1.DataSource = conex.consultar("SELECT * FROM log_eventos "+filtro+" ORDER BY idlog_eventos DESC limit 1000");
			//dataGridView1.DataSource = conex.consultar("SELECT * FROM log_eventos "+filtro+" ORDER BY idlog_eventos");
			dataGridView1.Columns[0].HeaderText="ID";
			dataGridView1.Columns[1].HeaderText="FECHA/HORA";
			dataGridView1.Columns[2].HeaderText="EVENTO";
			dataGridView1.Columns[3].HeaderText="USUARIO";
			
			//dataGridView1.Columns[0].MinimumWidth=30;
			dataGridView1.Columns[0].FillWeight=10F;
			//dataGridView1.Columns[1].MinimumWidth=150;
			dataGridView1.Columns[1].FillWeight=15F;
			//dataGridView1.Columns[2].MinimumWidth=300;
			dataGridView1.Columns[2].FillWeight=60F;
			//dataGridView1.Columns[3].MinimumWidth=100;
			dataGridView1.Columns[3].FillWeight=15F;
		}
	}
}

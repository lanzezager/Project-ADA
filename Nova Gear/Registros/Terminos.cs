/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 15/05/2018
 * Hora: 11:54 a. m.
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

namespace Nova_Gear.Registros
{
	/// <summary>
	/// Description of Terminos.
	/// </summary>
	public partial class Terminos : Form
	{
		public Terminos()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void TerminosLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			//Comienza Lectura
			StreamReader t4 = new StreamReader(@"terms.lz");
				
				while(!t4.EndOfStream){
					textBox1.AppendText(t4.ReadLine()+"\r\n");
				}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}

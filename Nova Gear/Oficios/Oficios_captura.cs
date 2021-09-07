/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 19/12/2016
 * Hora: 10:24 a.m.
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
using System.ComponentModel;

namespace Nova_Gear.Oficios
{
	/// <summary>
	/// Description of Oficios_captura.
	/// </summary>
	public partial class Oficios_captura : Form
	{
		public Oficios_captura()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        
		void Oficios_capturaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			String tipo;
            tipo = MainForm.datos_user_static[5];//puesto

            if (tipo.Equals("Controlador"))
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			Oficios_consulta ofi_consu = new Oficios_consulta();
			ofi_consu.Show();
			this.Hide();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Oficios_proceso ofi_proc = new Oficios_proceso();
			ofi_proc.Show();
			this.Hide();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Oficios_ingreso ofi_ing = new Oficios_ingreso();
			ofi_ing.Show();
			this.Hide();
		}

        private void button4_Click(object sender, EventArgs e)
        {
            Oficios_imprimir ofi_imprime = new Oficios_imprimir();
            ofi_imprime.Show();
            this.Hide();            
        }
	}
}

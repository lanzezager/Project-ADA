/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 29/05/2018
 * Hora: 10:37 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Menu_cores.
	/// </summary>
	public partial class Menu_cores : Form
	{
		public Menu_cores()
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
			Jarviscob jar = new Jarviscob();
	   		jar.Show();
	   		jar.Focus();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Envio_31 env_31 = new Envio_31();
			env_31.Show();
			env_31.Focus();
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void Menu_coresLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

		}
	}
}

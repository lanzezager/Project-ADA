/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 10/12/2016
 * Hora: 12:57 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear.Inventario
{
	/// <summary>
	/// Description of Menu_inventario.
	/// </summary>
	public partial class Menu_inventario : Form
	{
		public Menu_inventario()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Inventario.Consulta_verificacion inve = new Consulta_verificacion();
			inve.Show();
			this.Hide();
		}
		
		void Menu_inventarioLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			if ((MainForm.datos_user_static[2].Equals("0")) || (MainForm.datos_user_static[2].Equals("21"))){
				button2.Visible=true;
				button3.Visible=true;			  
			 }
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			Opciones_inventario opcs_inv = new Opciones_inventario();
			opcs_inv.Show();
            opcs_inv.Focus();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Generador_informes info_inv = new Generador_informes();
			info_inv.Show();
            info_inv.Focus();
            //this.WindowState= FormWindowState.Minimized;
		}
	}
}

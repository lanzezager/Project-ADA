/*
 * Creado por SharpDevelop.
 * Usuario: miguel.banuelos
 * Fecha: 21/03/2018
 * Hora: 02:46 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Menu_nova.
	/// </summary>
	public partial class Menu_nova : Form
	{
		public Menu_nova(int ori)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.origen=ori;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int origen=0;
		void Button2Click(object sender, EventArgs e)
		{
			Setup_mysql config = new Setup_mysql(origen);
            config.Show();
            config.Focus();
            this.Hide();
		}
		
		void Menu_novaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			String id_us ="";
			
			if(origen==0){
				button3.Enabled=false;
			}else{
				id_us=MainForm.datos_user_static[2];
				if(Convert.ToInt32(id_us)>1){
					button3.Enabled=false;
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			ops_subs.selector_sub sel_sub = new Nova_Gear.ops_subs.selector_sub(origen);
        	sel_sub.Show();
        	sel_sub.Focus();
        	this.Hide();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			Config_backup respaldo = new Config_backup();
			respaldo.Show();
			respaldo.Focus();
			this.Hide();
		}
	}
}

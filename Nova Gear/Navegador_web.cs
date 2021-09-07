/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 27/11/2018
 * Hora: 04:11 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Navegador_web.
	/// </summary>
	public partial class Navegador_web : Form
	{
		public Navegador_web(String url)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.dir=url;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
		}
		
		String dir="";
		
		void Navegador_webLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			Uri ini;
			ini = new Uri(dir);
			this.webBrowser1.Url=ini;
		}
	}
}

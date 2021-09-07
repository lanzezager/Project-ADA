/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 13/08/2015
 * Hora: 02:15 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Carga.
	/// </summary>
	public partial class Carga : Form
	{
		public Carga()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int actual=0;
		void Button10Click(object sender, EventArgs e)
		{
			if(radioButton1.Checked == true){
				actual=1; //COP
			//	Puente miPuente = this.Owner as Jarviscob;
			 //   miPuente.puente_res(actual);
				this.Dispose();
			}
			
			if(radioButton2.Checked == true){
				actual=2;//RCV
				//Puente miPuente = this.Owner as Jarviscob;
				//miPuente.puente_res(2);
				this.Dispose();
			}
		}
		
		void CargaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			radioButton1.Checked = true;			
		}
		
		public int mandar(){
			return actual;
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }
	}
}

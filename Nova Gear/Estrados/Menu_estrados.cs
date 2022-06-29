/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 27/09/2017
 * Hora: 03:29 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear.Estrados
{
	/// <summary>
	/// Description of Menu_estrados.
	/// </summary>
	public partial class Menu_estrados : Form
	{
		public Menu_estrados()
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
			//captura y modificacion estrados
			Captura_info_estrados estrados_ingreso = new Captura_info_estrados(1,0,"");
			estrados_ingreso.Show();
			//this.Hide();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			//consulta estrados
			Captura_info_estrados estrados_ingreso = new Captura_info_estrados(2,0,"");
			estrados_ingreso.Show();
			//this.Hide();
		}
		
		void Menu_estradosLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			String puesto, rango, id_user;
			puesto = MainForm.datos_user_static[5];
			rango = MainForm.datos_user_static[2];
			
			if(puesto.Equals("Auxiliar Estrados")==true){
				button1.Enabled=true;
			}else{
				button1.Enabled=false;
			}
			
			if((rango.Equals("0")==true)){
				button1.Enabled=true;
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			Reporte_estrados reports = new Reporte_estrados();
			reports.Show();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Opciones_estrados opcs = new Opciones_estrados();
			opcs.Show();
            opcs.Focus();
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			Depuracion.Depu_manu form_nn = new Depuracion.Depu_manu("nada",2);
	   		//form_nn.MdiParent=this;
	   		form_nn.Show();
	   		form_nn.Focus();
		}

        private void button6_Click(object sender, EventArgs e)
        {
            Procesar_PDFs procesar_pdf = new Procesar_PDFs();
            procesar_pdf.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            control_por_volumen_estrados volumen = new control_por_volumen_estrados();
            volumen.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Fenix_estrados fenix = new Fenix_estrados();
            fenix.Show();
        }
	}
}

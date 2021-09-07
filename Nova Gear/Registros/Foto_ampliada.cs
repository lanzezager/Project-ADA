/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 28/07/2015
 * Hora: 11:36 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear.Registros
{
	/// <summary>
	/// Description of Foto_ampliada.
	/// </summary>
	public partial class Foto_ampliada : Form
	{
		public Foto_ampliada(String img, String namee)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.pictureBox1.ImageLocation = img;
			this.nombre = namee;
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String nombre;
		int ancho=0, alto=0;
		
		
		void Foto_ampliadaLoad(object sender, EventArgs e)
		{
			//this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2)-80;
			//this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
			button1.Location = new System.Drawing.Point((this.Width - 57),12);
			this.Text = nombre.ToUpper();
			if(nombre.Equals("JAJAJAJAJAJA")){
				this.BackColor = System.Drawing.Color.Black;
				this.WindowState= FormWindowState.Maximized;
			}
			
			
		}
		
		void Foto_ampliadaResize(object sender, EventArgs e)
		{
			
			alto = this.Height -35;
			ancho = this.Width -12;
			pictureBox1.Width = ancho;
			pictureBox1.Height = alto;
			textBox1.Text= ancho+", "+this.Width;
			
			if(this.Width>750){
			button1.Location = new System.Drawing.Point((this.Width - 70),12);
			button1.Visible = true;
			}else{
				button1.Visible = false;
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}

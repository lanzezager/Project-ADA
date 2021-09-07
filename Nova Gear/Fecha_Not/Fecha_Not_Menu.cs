/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 24/08/2015
 * Hora: 09:48 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear.Fecha_Not
{
	/// <summary>
	/// Description of Fecha_Not_Menu.
	/// </summary>
	public partial class Fecha_Not_Menu : Form
	{
		public Fecha_Not_Menu()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void abrir_opc1(){
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		}
		
		public void abrir_opc2(){
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		}
		
		public void abrir_opc3(){
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			Fecha_Not_Cartera cartera = new Fecha_Not_Cartera();
			cartera.MdiParent = MdiParent;
			cartera.Show();
			cartera.Focus();
			this.Dispose();
		}
		
		void Fecha_Not_MenuLoad(object sender, EventArgs e)
		{
			this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2)-30;
			this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;
		}
		
		void Panel1MouseEnter(object sender, EventArgs e)
		{
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void Panel1MouseLeave(object sender, EventArgs e)
		{
			this.panel1.BackColor = System.Drawing.Color.Transparent;
		}
		
		void PictureBox1MouseEnter(object sender, EventArgs e)
		{
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void PictureBox1MouseLeave(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Label1MouseEnter(object sender, EventArgs e)
		{
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		
		}
		
		void Label1MouseLeave(object sender, EventArgs e)
		{
			this.panel1.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Panel2MouseEnter(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void Panel2MouseLeave(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.Transparent;
		}
		
		void PictureBox2MouseEnter(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void PictureBox2MouseLeave(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Label4MouseEnter(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void Label4MouseLeave(object sender, EventArgs e)
		{
			this.panel2.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Panel3MouseEnter(object sender, EventArgs e)
		{
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void Panel3MouseLeave(object sender, EventArgs e)
		{
			this.panel3.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Label5MouseEnter(object sender, EventArgs e)
		{
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void Label5MouseLeave(object sender, EventArgs e)
		{
			this.panel3.BackColor = System.Drawing.Color.Transparent;
		}
		
		void PictureBox3MouseEnter(object sender, EventArgs e)
		{
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
		}
		
		void PictureBox3MouseLeave(object sender, EventArgs e)
		{
			this.panel3.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Panel1Click(object sender, EventArgs e)
		{
			abrir_opc1();
		}
		
		void PictureBox1Click(object sender, EventArgs e)
		{
			abrir_opc1();
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			abrir_opc1();
		}
		
		void Label2Click(object sender, EventArgs e)
		{
			abrir_opc1();
		}
		
		void PictureBox2Click(object sender, EventArgs e)
		{
			abrir_opc2();
		}
		
		void Label4Click(object sender, EventArgs e)
		{
			abrir_opc2();
		}
		
		void Label3Click(object sender, EventArgs e)
		{
			abrir_opc2();
		}
		
		void Panel2Click(object sender, EventArgs e)
		{
			abrir_opc2();
		}
		
		void Panel3Click(object sender, EventArgs e)
		{
			abrir_opc3();
		}
		
		void PictureBox3Click(object sender, EventArgs e)
		{
			abrir_opc3();
		}
		
		void Label6Click(object sender, EventArgs e)
		{
			abrir_opc3();
		}
		
		void Label5Click(object sender, EventArgs e)
		{
			abrir_opc3();
		}
		
		void CarteraToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_opc3();
		}
	}
}

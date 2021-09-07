/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 29/07/2015
 * Hora: 11:24 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Login.
	/// </summary>
	public partial class Login : Form
	{
		public Login()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String mensaje,sql;
		int error=0;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		
		
		public void ingreso(){
			error=0;
			mensaje="Ingrese correctamente:\n\n";
			
			if((textBox1.Text.Equals("Usuario..."))||(textBox1.Text.Length < 5)){
				mensaje+="Usuario\n";
				error=1;
			}
			
			if((textBox2.Text.Equals("Contraseña..."))||(textBox2.Text.Length < 6)){
				mensaje+="Contraseña\n";
				error=1;
			}
			
			if(error==1){
				MessageBox.Show(mensaje,"Error");
			}else{
				conex.conectar("base_principal");
				sql="SELECT nom_usuario,contrasena,rango,url_imagen,nombre,puesto FROM usuarios WHERE nom_usuario = \""+textBox1.Text+"\" AND contrasena = \""+textBox2.Text+"\"";
				dataGridView1.DataSource = conex.consultar(sql);
				
				if(dataGridView1.RowCount > 0){
					if(((dataGridView1.Rows[0].Cells[0].Value.ToString()).Equals(textBox1.Text))&&((dataGridView1.Rows[0].Cells[1].Value.ToString()).Equals(textBox2.Text))){
						MessageBox.Show("Acceso Concedido","Nova Gear");
						
						
					}
				}else{
					MessageBox.Show("Acceso Denegado","Nova Gear");
				}
			}
		}
		
		public void respawn(){
			this.Show();
		}
		
		void LoginLoad(object sender, EventArgs e)
		{
			this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
			this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;		
			
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
			
		}
		
		void TextBox1Enter(object sender, EventArgs e)
		{
			if(textBox1.Text.Equals("Usuario...")){
			textBox1.Clear();
			this.textBox1.ForeColor = System.Drawing.Color.White;
			}
		}
		
		void TextBox1Leave(object sender, EventArgs e)
		{
			if(textBox1.Text.Length<1){
				this.textBox1.ForeColor = System.Drawing.Color.LightBlue;
				textBox1.Text = "Usuario...";
			}else{
				this.textBox1.ForeColor = System.Drawing.Color.White;
			}
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			
		}
	
		void TextBox2Enter(object sender, EventArgs e)
		{
			if(textBox2.Text.Equals("Contraseña...")){
			textBox2.Clear();
			this.textBox2.ForeColor = System.Drawing.Color.White;
			textBox2.PasswordChar='•';
			}
		}
		
		void TextBox2Leave(object sender, EventArgs e)
		{
			if(textBox2.Text.Length<1){
				this.textBox2.ForeColor = System.Drawing.Color.LightBlue;
				//textBox2.PasswordChar= null;
				textBox2.Text = "Contraseña...";
			}else{
				this.textBox2.ForeColor = System.Drawing.Color.White;
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			//System.Diagnostics.Process.Start("close_ng.bat");
            this.Dispose();

		}
		
		
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
				ingreso();
			}
		}
		
		void LoginClick(object sender, EventArgs e)
		{
		
		}

        void Button5Click(object sender, EventArgs e)
        {
            About abo = new About();
            abo.ShowDialog();
        }
        //captura fecha not
        private void button1_Click(object sender, EventArgs e)
        {
            Siscobnator sis = new Siscobnator();
            sis.Show();
            this.Hide();
        }
        //captura incidencias 03,09,31
        private void button4_Click(object sender, EventArgs e)
        {
            Jarviscob jar = new Jarviscob();
            jar.Show();
            this.Hide();
        }
		//captura clave ajuste 12 y 42
		void Button6Click(object sender, EventArgs e)
		{
			Autocob auto = new Autocob();
			auto.Show();
			this.Hide();
		}
        //captura sicofi
        void Button2Click(object sender, EventArgs e)
        {
            Sicofitron sicofi = new Sicofitron();
            sicofi.Show();
            this.Hide();
        }
	}
}

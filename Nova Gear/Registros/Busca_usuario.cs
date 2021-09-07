/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 11/03/2016
 * Hora: 10:08 a. m.
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

namespace Nova_Gear.Registros
{
	/// <summary>
	/// Description of Busca_usuario.
	/// </summary>
	public partial class Busca_usuario : Form
	{
		public Busca_usuario(string[] datos_user)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.user_datos = new string[datos_user.Length];
			this.user_datos = datos_user;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String sql,nombre,pic_lz_img,borrado,ruta;
		string[] user_datos;
		int i=0,act_pass=0,encontrado=0,id=0;
		DialogResult respuesta;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
		DataTable consultamysql = new DataTable();
		
		public void estilo_grid(){
			dataGridView1.Columns[0].HeaderText="ID";
			dataGridView1.Columns[1].HeaderText="NOMBRE";
			dataGridView1.Columns[2].HeaderText="PUESTO";
			dataGridView1.Columns[3].HeaderText="USUARIO";
			dataGridView1.Columns[4].Visible=false;
			dataGridView1.Columns[5].Visible=false;
			dataGridView1.Columns[6].Visible=false;
			
			dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			
			dataGridView1.Columns[0].Width=40;
			dataGridView1.Columns[1].MinimumWidth=180;
		}
		
		public void checa_inactivos(){
			i=0;
			do{
				if(dataGridView1.Rows[i].Cells[4].FormattedValue.ToString().Equals("inactivo")){
					dataGridView1.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.Red;
					dataGridView1.Rows[i].Cells[0].Style.ForeColor = System.Drawing.Color.White;
					dataGridView1.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.Red;
					dataGridView1.Rows[i].Cells[1].Style.ForeColor = System.Drawing.Color.White;
					dataGridView1.Rows[i].Cells[2].Style.BackColor = System.Drawing.Color.Red;
					dataGridView1.Rows[i].Cells[2].Style.ForeColor = System.Drawing.Color.White;
					dataGridView1.Rows[i].Cells[3].Style.BackColor = System.Drawing.Color.Red;
					dataGridView1.Rows[i].Cells[3].Style.ForeColor = System.Drawing.Color.White;
				}
				//MessageBox.Show(dataGridView1.Rows[i].Cells[4].FormattedValue.ToString());
				i++;
			}while(i<dataGridView1.RowCount);
			
		}
		
		public void combinar_nombre_full(){
			i=0;
			do{
				dataGridView1.Rows[i].Cells[1].Value = dataGridView1.Rows[i].Cells[6].Value.ToString()+" "+dataGridView1.Rows[i].Cells[1].Value.ToString();
				i++;
			}while(i<dataGridView1.RowCount);
			
		}
		
		public void confirma_pass(){
			
			if(textBox1.Text.Equals(user_datos[1])){
				groupBox2.Enabled=true;
			}else{
				MessageBox.Show("Contraseña Incorrecta","Error");
			}
				groupBox1.Text="Buscar Usuario";
				textBox1.Text="";
				textBox1.PasswordChar ='\0';	
				act_pass=0;
				button5.Enabled=false;
		}
		
		public void buscar_user(){
			nombre=textBox1.Text;
			encontrado=0;
			
			if(i >= dataGridView1.RowCount){
				i=0;
			}
			
				do{
					if((dataGridView1.Rows[i].Cells[1].FormattedValue.ToString().ToLower().Contains(nombre.ToLower())) == true){
						//MessageBox.Show(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString()+","+i);
						dataGridView1.Rows[i].Cells[1].Selected=true;
						encontrado=2;
						//i=dataGridView1.RowCount;
					}
					
					i++;
					if(i>=dataGridView1.RowCount){
						if(encontrado==0){
							encontrado=1;
						}else{
							encontrado=2;
						}
					}
					
				}while(encontrado == 0);
			//MessageBox.Show(""+i+","+encontrado+","+dataGridView1.RowCount);
			if((i>=dataGridView1.RowCount)&&(encontrado<2)){
				MessageBox.Show("Se termino de revisar la lista y no se encontraron mas coincidencias","Aviso");
			 i=0;
			}
			
		}
		
		public void pic_lz(){
			Foto_ampliada form_fa = new Foto_ampliada(pic_lz_img,"JAJAJAJAJAJA");
			MainForm mani = (MainForm)this.MdiParent;
			form_fa.MdiParent=mani;
			form_fa.Show();
		}
		
		void Busca_usuarioLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;

			conex.conectar("base_principal");
			sql="SELECT id_usuario,nombre,puesto,nom_usuario,estatus,url_imagen,apellido from usuarios";
			consultamysql=conex.consultar(sql);		
			dataGridView1.DataSource = consultamysql;

            ruta = conex.leer_config();
            ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
            ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
			            
            pic_lz_img = ruta + dataGridView1.Rows[0].Cells[5].Value.ToString();
            pic_lz_img = @"recursos/iconos/JollyRoger.png";
			
			estilo_grid();
			label2.Text="Usuarios Totales: "+dataGridView1.RowCount;
			combinar_nombre_full();
			//groupBox2.Enabled=true;
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString().Equals("lanzezager")){
				//MessageBox.Show("Este usuario no se puede desactivar muajajajajaja");
				pic_lz();
			}else{
				if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value.ToString().Equals("activo")){
					respuesta = MessageBox.Show("Los usuarios desactivados no pueden iniciar sesión.\n\n¿Está Seguro de querer desactivar este usuario?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
					
					if(respuesta ==DialogResult.Yes){
						
						id=Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
						sql="UPDATE usuarios SET estatus=\"inactivo\" WHERE id_usuario ="+(id)+";";
						borrado = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
						conex.consultar(sql);
						sql="SELECT id_usuario,nombre,puesto,nom_usuario,estatus from usuarios";
						consultamysql=conex.consultar(sql);     
                        dataGridView1.DataSource=consultamysql;
                        //dataGridView1.DataSource = conex.consultar(sql);
						checa_inactivos();
						label2.Text="Usuarios Totales: "+dataGridView1.RowCount;
						conex2.guardar_evento("Se Configuró como inactivo el usuario "+borrado);
					}
				}else{
					respuesta = MessageBox.Show("Los usuarios activos pueden iniciar sesión y hacer modificaciones según sus permisos.\n\n¿Está Seguro de querer activar este usuario?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
					
					if(respuesta ==DialogResult.Yes){
						String id_log;
						id_log = MainForm.datos_user_static[7];	
						id=Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
						sql="UPDATE usuarios SET estatus=\"activo\",usuario_k_registro="+id_log+" WHERE id_usuario ="+(id)+";";
						borrado = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
						conex.consultar(sql);
						sql="SELECT id_usuario,nombre,puesto,nom_usuario,estatus from usuarios";
						consultamysql=conex.consultar(sql);
						dataGridView1.DataSource = consultamysql;
						checa_inactivos();
						label2.Text="Usuarios Totales: "+dataGridView1.RowCount;
						conex2.guardar_evento("Se Configuró como activo el usuario "+borrado);
											
					}
					
				}
			}
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			checa_inactivos();
			timer1.Enabled=false;
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			if(act_pass==1){
				confirma_pass();
			}else{
			
			textBox1.Text="";
			textBox1.PasswordChar = '•';	
			groupBox1.Text="Ingresar Contraseña";
			textBox1.Focus();
			act_pass=1;
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(act_pass==1){
				confirma_pass();
			}else{  
				buscar_user();
			}
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(act_pass==1){
			   if (e.KeyChar == (char)(Keys.Enter))
	           {
					confirma_pass();
			   }
			}else{
				if (e.KeyChar == (char)(Keys.Enter))
	           {
			   		buscar_user();
			   }
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			i=0;
			nombre="";
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString().Equals("lanzezager")){
					//MessageBox.Show("Este usuario no se puede borrar muajajajajaja");
					pic_lz();
				}else{
			respuesta = MessageBox.Show("¡ATENCION! Esta Acción no podrá deshacerse.\n\n¿Está Seguro de querer borrar este usuario?"+
			                            "","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(respuesta ==DialogResult.Yes){
				conex2.conectar("base_principal");
				borrado = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
				id=Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
				//sql="UPDATE usuarios SET estatus=\"activo\" WHERE id_usuario ="+(id)+";";
				conex2.consultar("DELETE FROM usuarios WHERE id_usuario="+id);
				
				sql="SELECT id_usuario,nombre,puesto,nom_usuario,estatus from usuarios";
				consultamysql=conex2.consultar(sql);
				dataGridView1.DataSource = consultamysql;
				checa_inactivos();
				label2.Text="Usuarios Totales: "+dataGridView1.RowCount;
				conex.guardar_evento("Se elimino el usuario "+borrado+" de la base de datos.");
				MessageBox.Show("Se elimino exitosamente el usuario: "+borrado,"Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			}
			
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			respuesta = MessageBox.Show("Está por modificar la información de un usuario.\n\n¿Está Seguro de querer continuar?"+
			                            "","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button2);
			
			if(respuesta ==DialogResult.Yes){
				if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString().Equals("lanzezager")){
					//MessageBox.Show("Este usuario no se puede modificar muajajajajaja");
					pic_lz();
				}else{
				id=Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
				//MainForm mani = (MainForm)this.MdiParent;
				//mani.abrir_modificar_users(2,id);
				
				//Registros.Usuarios form3_1 = new Registros.Usuarios(2,id);
				//form3_1.Show();
            	//form3_1.Focus();
            	
            	Reg_Usuarios_2 ru2de= new Reg_Usuarios_2(2,id);
            	ru2de.Show();
            	ru2de.Focus();
				}
				
			}		
		}
		
		void DataGridView1CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{		
				id=Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
				if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString().Equals("lanzezager")){
					//MessageBox.Show("Este usuario no se puede mostrar muajajajajaja");
					pic_lz();
				}else{
				//MainForm mani = (MainForm)this.MdiParent;
				//mani.abrir_modificar_users(3,id);
				
				//Registros.Usuarios form3_1 = new Registros.Usuarios(3,id);
            	//form3_1.Show();
            	//form3_1.Focus();
            	
            	Reg_Usuarios_2 ru2de= new Reg_Usuarios_2(3,id);
            	ru2de.Show();
            	ru2de.Focus();
				}
		}
		
		void DataGridView1ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			checa_inactivos();
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			//Registros.Usuarios form3 = new Registros.Usuarios(1,0);
			//form3.MdiParent=this;
            //form3.Show();
            //form3.Focus();
            
            Reg_Usuarios_2 ru2= new Reg_Usuarios_2(1,0);
            ru2.Show();
            ru2.Focus();
            
		}
	}
}

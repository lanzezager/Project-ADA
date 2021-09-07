/*
 * Creado por SharpDevelop.
 * Usuario: miguel.banuelos
 * Fecha: 26/03/2018
 * Hora: 02:35 p.m.
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
using System.Diagnostics;

namespace Nova_Gear.Registros
{
	/// <summary>
	/// Description of Reg_Usuarios_2.
	/// </summary>
	public partial class Reg_Usuarios_2 : Form
	{
		public Reg_Usuarios_2(int modo, int id_usuario)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			this.mode=modo;
			this.id_bus=id_usuario;
		}
		
		String imagen,error;
		string[] guarda_datos= new string[26];
		int pic_loc=0,mode,id_bus,llena_inf=0,rep=0;
		
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		
		public int verif_campos(){
			int verif=0;
			Int64 nume=0;
			error="";
			
			//datos personales
			if(pictureBox2.ImageLocation != null){
				verif++;
			}else{
				error+="•Fotografía\n";
			}
			
			if(textBox1.Text.Length>5){
				verif++;
				guarda_datos[0]=textBox1.Text;
			}else{
				error+="•Nombre\n";
			}
			
			if(textBox2.Text.Length>5){
				verif++;
				guarda_datos[1]=textBox2.Text;
			}else{
				error+="•Apellido\n";
			}
			
			if(comboBox1.SelectedIndex>-1){
				verif++;
				guarda_datos[2]=comboBox1.SelectedItem.ToString();
			}else{
				error+="•Documento de Identificación\n";
			}
			
			if(textBox3.Text.Length>5){
				verif++;
				guarda_datos[3]=textBox3.Text;
			}else{
				error+="•Número de Doc. de Identificación\n";
			}
			
			if(comboBox2.SelectedIndex>-1){
				verif++;
				guarda_datos[4]=comboBox2.SelectedItem.ToString();
			}else{
				error+="•Estado Civil\n";
			}
			
			if(dateTimePicker1.Value < DateTime.Today){
				verif++;
				guarda_datos[5]=adapta_fecha(dateTimePicker1.Value.ToShortDateString());
			}else{
				error+="•Fecha de Nacimiento\n";
			}
			
			if(textBox4.Text.Length>5){
				verif++;
				guarda_datos[6]=textBox4.Text;
			}else{
				error+="•Domicilio\n";
			}
			
			if((textBox5.Text.Length>5)&&(textBox5.Text.Contains("@"))){
				verif++;
				guarda_datos[7]=textBox5.Text;
			}else{
				error+="•Correo Eléctronico\n";
			}
			
			if(maskedTextBox1.MaskCompleted==true){
				verif++;
				guarda_datos[8]=maskedTextBox1.Text;
			}else{
				error+="•N° Celular\n";
			}
			
			if(comboBox3.SelectedIndex>-1){
				verif++;
				guarda_datos[9]=comboBox3.SelectedItem.ToString();
			}else{
				error+="•Escolaridad\n";
			}
			
			if(textBox6.Text.Length>5){
				verif++;
				guarda_datos[10]=textBox6.Text;
			}else{
				error+="•Profesión/Oficio\n";
			}
			
			//datos laborales
			
			if(comboBox4.SelectedIndex>-1){
				verif++;
				guarda_datos[11]=comboBox4.SelectedItem.ToString();
			}else{
				error+="•Categoria\n";
			}
			
			if(comboBox5.SelectedIndex>-1){
				verif++;
				guarda_datos[12]=comboBox5.SelectedItem.ToString();
			}else{
				error+="•Cargo Mesa\n";
			}
			
			if(comboBox6.SelectedIndex>-1){
				verif++;
				guarda_datos[13]=comboBox6.SelectedItem.ToString();
			}else{
				error+="•Departamento/Área\n";
			}
			
			if(comboBox7.SelectedIndex>-1){
				verif++;
				guarda_datos[14]=comboBox7.SelectedItem.ToString();
				guarda_datos[25]="0";
				//rango primario
				switch(comboBox7.SelectedItem.ToString()){
					case "Jefe de Área" :
						guarda_datos[25]="1";
						break;
					case "Auxiliar Multifunción" :
						guarda_datos[25]="2";
						break;
					case "Auxiliar" :
						guarda_datos[25]="7";
						break;
					case "Auxiliar Estrados" :
						guarda_datos[25]="5";
						break;
					case "Auxiliar Oficios" :
						guarda_datos[25]="6";
						break;
					case "Controlador" :
						guarda_datos[25]="3";
						break;
					case "Notificador" :
						guarda_datos[25]="4";
						break;
					case "Ventanilla" :
						guarda_datos[25]="4";
						break;
					case "Cartera" :
						guarda_datos[25]="4";
						break;
				}
				
			}else{
				error+="•Actividad Nova\n";
			}
			
			if(comboBox8.SelectedIndex>-1){
				verif++;
				guarda_datos[15]=comboBox8.SelectedItem.ToString();
				
				if(comboBox8.SelectedItem.ToString().Equals("Operativo TTD")){
					if((dateTimePicker2.Value.ToShortDateString() != DateTime.Today.ToShortDateString())&&(dateTimePicker2.Value<dateTimePicker3.Value)){
						verif++;
						guarda_datos[16]=adapta_fecha(dateTimePicker2.Value.ToShortDateString());
					}else{
						error+="•Fecha de Inicio de Contrato\n";
					}
					
					if((dateTimePicker3.Value.ToShortDateString() != DateTime.Today.ToShortDateString())&&(dateTimePicker3.Value>dateTimePicker2.Value)){
						verif++;
						guarda_datos[17]=adapta_fecha(dateTimePicker3.Value.ToShortDateString());
					}else{
						error+="•Fecha de Fin de Contrato\n";
					}
				}else{
					guarda_datos[16]=adapta_fecha("01/01/2000");
					guarda_datos[17]=adapta_fecha("02/01/2000");
					verif=verif+2;
				}
			}else{
				error+="•Tipo de Trabajador\n";
			}
			
			if(textBox7.Text.Length>5){
				verif++;
				guarda_datos[18]=textBox7.Text;
			}else{
				error+="•N.S.S. (Número de Seguro Social)\n";
			}
			
			if(textBox8.Text.Length>5){
				if(Int64.TryParse(textBox8.Text, out nume)==true){
					verif++;
					guarda_datos[19]=textBox8.Text;
				}else{
					error+="•Matricula/N° Empleado \n";
				}
			}else{
				error+="•Matricula/N° Empleado \n";
			}
			
			//datos usuario
			if(textBox9.Text.Length>3){
				verif++;
				guarda_datos[21]=textBox9.Text;
			}else{
				error+="•Usuario\n";
			}
			
			if(textBox10.Text.Length>5){
				verif++;
				guarda_datos[22]=textBox10.Text;
			}else{
				error+="•Contraseña\n";
			}
			
			if(mode==1){
				if(checkBox1.Checked==true){
					verif++;
				}else{
					error+="•Aceptar Términos y Condiciones\n";
				}
			}
			//MessageBox.Show(verif.ToString());
			
			if(mode==1){
				if(verif>=24){
					return 1;
				}else{
					return 0;
				}
			}else{
				if(verif>=23){
					return 1;
				}else{
					return 0;
				}
			}
		}
		
		public int verif_usuario(){
			conex.conectar("base_principal");
			dataGridView1.DataSource=conex.consultar("SELECT id_usuario FROM usuarios WHERE nom_usuario= \""+textBox9.Text+"\"");
			
			if(dataGridView1.RowCount<1){
				return 1;
			}else{
				if(mode==2){//si es modificacion
					//MessageBox.Show(id_bus+"|"+dataGridView1.Rows[0].Cells[0].Value.ToString()+"|");
					if((dataGridView1.RowCount==1) && (id_bus.ToString().Equals(dataGridView1.Rows[0].Cells[0].Value.ToString()))){
						return 1;
					}else{
						return 0;
					}
				}else{
					return 0;
				}
			}
		}
		
		public void registrar_usuario(){
			try{
				String sql;
				int rank=0;
				conex.conectar("base_principal");
				guardar_pic();
				guarda_datos[23]=imagen;
				
				//rango secundario
				switch(guarda_datos[13]){
					case "Emisión y Pago Oportuno":
						break;
					case "Cobros":
						rank=Convert.ToInt32(guarda_datos[25])+10;
						guarda_datos[25]=rank.ToString();
						break;
					case "Registro y Control de la Cartera":
						rank=Convert.ToInt32(guarda_datos[25])+20;
						guarda_datos[25]=rank.ToString();
						break;
				}
                
				sql="INSERT INTO usuarios  (nombre,apellido,doc_ide,num_ide,edo_civil,fech_nac,domicilio,email,celular,escolaridad,profesion,categoria,mesa,area,puesto," +
					"tipo_trab,contrato_ini,contrato_fin,nss_trab,num_mat,unidad,nom_usuario,contrasena,url_imagen,estatus,rango,fecha_registro)\n "+
					"VALUES( " +
					"\""+guarda_datos[0]+"\"," +
					"\""+guarda_datos[1]+"\"," +
					"\""+guarda_datos[2]+"\"," +
					"\""+guarda_datos[3]+"\"," +
					"\""+guarda_datos[4]+"\"," +
					"\""+guarda_datos[5]+"\"," +
					"\""+guarda_datos[6]+"\"," +
					"\""+guarda_datos[7]+"\"," +
					"\""+guarda_datos[8]+"\"," +
					"\""+guarda_datos[9]+"\"," +
					"\""+guarda_datos[10]+"\"," +
					"\""+guarda_datos[11]+"\"," +
					"\""+guarda_datos[12]+"\"," +
					"\""+guarda_datos[13]+"\"," +
					"\""+guarda_datos[14]+"\"," +
					"\""+guarda_datos[15]+"\"," +
					"\""+guarda_datos[16]+"\"," +
					"\""+guarda_datos[17]+"\"," +
					"\""+guarda_datos[18]+"\"," +
					""+guarda_datos[19]+"," +
					"\"Subdelegación "+conex.leer_config_sub()[3]+"\"," +
					"\""+guarda_datos[21]+"\"," +
					"AES_ENCRYPT(\""+guarda_datos[22]+"\",\"Nova Gear & AKD ATLAS & LZ RULES!!!\")," +
					"\""+guarda_datos[23]+"\"," +
					"\"inactivo\"," +
					""+guarda_datos[25]+"," +
					"\""+adapta_fecha(DateTime.Today.ToShortDateString())+"\")";
				textBox11.Text=sql;
				conex.consultar(sql);
				MessageBox.Show("Se Guardó el Usuario Correctamente","¡Exito!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                Visor_Formato_Registro vfr = new Visor_Formato_Registro();
                vfr.recibir_datos(guarda_datos);
                vfr.Show();
                this.Hide();
			}catch(Exception e){
				MessageBox.Show("Ocurrió el Siguiente error al momento de Registrar el Usuario:\n\n "+e.ToString(),"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
		
 		public void modificar_usuario(){
			
			if(verif_cambios_modif()[0].Length>1){
				DialogResult res= MessageBox.Show("Se Actualizarán los Siguientes datos:\n\n "+verif_cambios_modif()[1]+"\n\n¿Desea Continuar?","Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button2);
			
				if(res== DialogResult.Yes){
					conex2.conectar("base_principal");
					//MessageBox.Show(verif_cambios_modif()[0]);
					conex2.consultar(verif_cambios_modif()[0]);
					
					
					MessageBox.Show("Se Modificó correctamente el usuario","Exito!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
					this.Close();
				}
			}else{
				MessageBox.Show("No se detectó ninguna modificación","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			
		}
		
		public void consultar(){
			
			String ruta = conex2.leer_config();
			
			ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
			ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
			
			conex2.conectar("base_principal");
			dataGridView2.DataSource=conex2.consultar("SELECT nombre,apellido,doc_ide,num_ide,edo_civil,fech_nac,domicilio,email,celular,escolaridad,profesion,categoria,mesa,area,puesto,tipo_trab,"+
			                                          "contrato_ini,contrato_fin,nss_trab,num_mat,unidad,nom_usuario,contrasena,url_imagen,estatus,rango FROM usuarios WHERE id_usuario= \""+id_bus+"\"");
			
			/*
 			nombre,0
			apellido,1
			doc_ide,2
			num_ide,3
			edo_civil,4
			fech_nac,5
			domicilio,6
			e-mail,7
			celular,8
			escolaridad,9
			profesion,10
			categoria,11
			mesa,12
			area,13
			puesto,14
			tipo_trab,15
			contrato_ini,16
			contrato_fin,17
			nss_trab,18
			num_mat,19
			unidad,20
			nom_usuario,21
			contrasena,22
			url_imagen,23
			estatus,24
			rango,25
			 */
			try{
				if(dataGridView2.RowCount>0){
					pictureBox2.ImageLocation=ruta+dataGridView2.Rows[0].Cells[23].Value.ToString();
					textBox1.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
					textBox2.Text=dataGridView2.Rows[0].Cells[1].Value.ToString();
					comboBox1.SelectedItem=dataGridView2.Rows[0].Cells[2].Value.ToString();
					textBox3.Text=dataGridView2.Rows[0].Cells[3].Value.ToString();
					comboBox2.SelectedItem=dataGridView2.Rows[0].Cells[4].Value.ToString();
					if(dataGridView2.Rows[0].Cells[5].Value == DBNull.Value){
						llena_inf++;
					}else{
						//MessageBox.Show("La Información de este Usuario se Encuentra Incompleta, Favor de Solicitar el correcto llenado de este Formulario","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
						dateTimePicker1.Value=Convert.ToDateTime(dataGridView2.Rows[0].Cells[5].Value.ToString());
					}
					textBox4.Text=dataGridView2.Rows[0].Cells[6].Value.ToString();
					textBox5.Text=dataGridView2.Rows[0].Cells[7].Value.ToString();
					maskedTextBox1.Text=dataGridView2.Rows[0].Cells[8].Value.ToString();
					comboBox3.SelectedItem=dataGridView2.Rows[0].Cells[9].Value.ToString();
					textBox6.Text=dataGridView2.Rows[0].Cells[10].Value.ToString();
					comboBox4.SelectedItem=dataGridView2.Rows[0].Cells[11].Value.ToString();
					comboBox5.SelectedItem=dataGridView2.Rows[0].Cells[12].Value.ToString();
					comboBox6.SelectedItem=dataGridView2.Rows[0].Cells[13].Value.ToString();
					comboBox7.SelectedItem=dataGridView2.Rows[0].Cells[14].Value.ToString();
					comboBox8.SelectedItem=dataGridView2.Rows[0].Cells[15].Value.ToString();
					if(dataGridView2.Rows[0].Cells[16].Value == DBNull.Value){
						llena_inf++;
					}else{
						dateTimePicker2.Value=Convert.ToDateTime(dataGridView2.Rows[0].Cells[16].Value.ToString());
					}
					if(dataGridView2.Rows[0].Cells[17].Value == DBNull.Value){
						llena_inf++;
					}else{
						dateTimePicker3.Value=Convert.ToDateTime(dataGridView2.Rows[0].Cells[17].Value.ToString());
					}
					textBox7.Text=dataGridView2.Rows[0].Cells[18].Value.ToString();
					textBox8.Text=dataGridView2.Rows[0].Cells[19].Value.ToString();
					label19.Text="Subdelegación: "+dataGridView2.Rows[0].Cells[20].Value.ToString();
					textBox9.Text=dataGridView2.Rows[0].Cells[21].Value.ToString();
					textBox10.Text=dataGridView2.Rows[0].Cells[22].Value.ToString();
					
					if(dataGridView2.Rows[0].Cells[24].Value.ToString().Equals("activo")){
						label9.Text="Activo";
						label9.BackColor=Color.MediumSeaGreen;
					}else{
						if(dataGridView2.Rows[0].Cells[24].Value.ToString().Equals("inactivo")){
							label9.Text="Inactivo";
							label9.BackColor=Color.Red;
						}
					}
					
					if(llena_inf>0){
						MessageBox.Show("La Información de este Usuario se Encuentra Incompleta, Favor de Solicitar el correcto llenado de este Formulario","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
				}
			}catch(Exception w){
				MessageBox.Show("Ocurrió un error al cargar la información de este usuario, es probable que se muestre incompleta","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			
		}
		
		public String adapta_fecha(String fecha){
			fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
			return fecha;
		}
		
		public void guardar_pic(){
			String ruta,ruta_alt_pic,nom,ext;
			int ind=0;
			try{

				ruta = conex.leer_config();//sacar ip del servidor
				ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
				ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
				
				if(!(File.Exists(ruta+imagen))){
					System.IO.File.Copy(pictureBox2.ImageLocation, ruta+imagen, false);
				}else{
					if (pic_loc == 0){
						ruta_alt_pic = "_0";
					}else{
						ruta_alt_pic = "";
					}
					imagen=imagen.Substring(0, imagen.Length-4)+ruta_alt_pic+imagen.Substring(imagen.Length-4,4);
					pic_loc = 1;
					if(!(File.Exists(ruta+imagen))){
						System.IO.File.Copy(pictureBox2.ImageLocation, ruta+imagen, false);
					}else{
						nom=imagen.Substring(0,imagen.LastIndexOf('_')+1);
						ext=imagen.Substring(imagen.LastIndexOf('.'),(imagen.Length-(imagen.LastIndexOf('.'))));
					    ind=Convert.ToInt32(imagen.Substring(imagen.LastIndexOf('_')+1,(imagen.Length-(nom.Length+ext.Length))));
						ind=ind+1;
						imagen=nom+ind.ToString()+ext;
						guardar_pic();
						}
				}

			}catch(IOException exc){
				MessageBox.Show("Ocurrio un problema al guardar la imagen de perfil\n\nError:\n"+exc,"Error");
			}
			
		}
		
		public void desactivar_campos(){
			//groupBox1.Enabled=false;
			//groupBox2.Enabled=false;
			//groupBox3.Enabled=false;
			
			//datos personales GroupBox1
			textBox1.ReadOnly=true;
			textBox2.ReadOnly=true;
			textBox3.ReadOnly=true;
			textBox4.ReadOnly=true;
			textBox5.ReadOnly=true;
			textBox6.ReadOnly=true;
			comboBox1.Enabled=false;
			comboBox2.Enabled=false;
			comboBox3.Enabled=false;
			maskedTextBox1.ReadOnly=true;
			dateTimePicker1.Enabled=false;
			
			//datos laborales
			textBox7.ReadOnly=true;
			textBox8.ReadOnly=true;
			comboBox4.Enabled=false;
			comboBox5.Enabled=false;
			comboBox6.Enabled=false;
			comboBox7.Enabled=false;
			comboBox8.Enabled=false;
			dateTimePicker2.Enabled=false;
			dateTimePicker3.Enabled=false;
			
			//datos usuario
			textBox9.ReadOnly=true;
			textBox10.ReadOnly=true;
			
			button1.Visible=false;
			checkBox1.Visible=false;
		}
		
		public string[] verif_cambios_modif(){
			
			string[] info_mod = new string[2];
			info_mod[0]="";
			info_mod[1]="";
			//try{
			String sql,sql2,modi="",aviso="";
			int rank=0;
			//guardar_pic();
			guarda_datos[23]=pictureBox2.ImageLocation;
			guarda_datos[23]=guarda_datos[23].Substring(guarda_datos[23].LastIndexOf('\\')+1,(guarda_datos[23].Length-(guarda_datos[23].LastIndexOf('\\')+1)));
			
			//rango secundario
			if(rep==0){
				switch(guarda_datos[13]){
					case "Emisión y Pago Oportuno":
						break;
					case "Cobros":
						rank=Convert.ToInt32(guarda_datos[25])+10;
						guarda_datos[25]=rank.ToString();
						break;
					case "Registro y Control de la Cartera":
						rank=Convert.ToInt32(guarda_datos[25])+20;
						guarda_datos[25]=rank.ToString();
						break;
				}
				rep++;
			}
			sql="UPDATE usuarios SET ";
			
			if(dataGridView2.Rows[0].Cells[23].Value.ToString() != guarda_datos[23]){
				modi+="url_imagen=\""+guarda_datos[23]+"\",";
				aviso+="•Fotografía\n";
			}
			
			if(dataGridView2.Rows[0].Cells[0].Value.ToString() != guarda_datos[0]){
				modi+="nombre=\""+guarda_datos[0]+"\",";
				aviso+="•Nombre\n";
			}
			
			if(dataGridView2.Rows[0].Cells[1].Value.ToString() != guarda_datos[1]){
				modi+="apellido=\""+guarda_datos[1]+"\",";
				aviso+="•Apellido\n";
			}
			
			if(dataGridView2.Rows[0].Cells[2].Value.ToString() != guarda_datos[2]){
				modi+="doc_ide=\""+guarda_datos[2]+"\",";
				aviso+="•Documento de Identificación\n";
			}
			
			if(dataGridView2.Rows[0].Cells[3].Value.ToString() != guarda_datos[3]){
				modi+="num_ide=\""+guarda_datos[3]+"\",";
				aviso+="•Número de Doc. de Identificación\n";
			}
			
			if(dataGridView2.Rows[0].Cells[4].Value.ToString() != guarda_datos[4]){
				modi+="edo_civil=\""+guarda_datos[4]+"\",";
				aviso+="•Estado Civil\n";
			}
			
			if(llena_inf==0){
				if(adapta_fecha(dataGridView2.Rows[0].Cells[5].Value.ToString().Substring(0,10)) != guarda_datos[5]){
					modi+="fech_nac=\""+guarda_datos[5]+"\",";
					aviso+="•Fecha de Nacimiento\n";
				}
			}else{
					modi+="fech_nac=\""+guarda_datos[5]+"\",";
					aviso+="•Fecha de Nacimiento\n";
			}
			
			if(dataGridView2.Rows[0].Cells[6].Value.ToString() != guarda_datos[6]){
				modi+="domicilio=\""+guarda_datos[6]+"\",";
				aviso+="•Domicilio\n";
			}
			
			if(dataGridView2.Rows[0].Cells[7].Value.ToString() != guarda_datos[7]){
				modi+="email=\""+guarda_datos[7]+"\",";
				aviso+="•Correo Electrónico\n";
			}
			
			if(dataGridView2.Rows[0].Cells[8].Value.ToString() != guarda_datos[8]){
				modi+="celular=\""+guarda_datos[8]+"\",";
				aviso+="•Celular\n";
			}
			
			if(dataGridView2.Rows[0].Cells[9].Value.ToString() != guarda_datos[9]){
				modi+="escolaridad=\""+guarda_datos[9]+"\",";
				aviso+="•Escolaridad\n";
			}
			
			if(dataGridView2.Rows[0].Cells[10].Value.ToString() != guarda_datos[10]){
				modi+="profesion=\""+guarda_datos[10]+"\",";
				aviso+="•Profesión\n";
			}
			
			if(dataGridView2.Rows[0].Cells[11].Value.ToString() != guarda_datos[11]){
				modi+="categoria=\""+guarda_datos[11]+"\",";
				aviso+="•Categoría\n";
			}
			
			if(dataGridView2.Rows[0].Cells[12].Value.ToString() != guarda_datos[12]){
				modi+="mesa=\""+guarda_datos[12]+"\",";
				aviso+="•Cargo Mesa\n";
			}
			
			if(dataGridView2.Rows[0].Cells[13].Value.ToString() != guarda_datos[13]){
				modi+="area=\""+guarda_datos[13]+"\",";
				aviso+="•Depto./Área\n";
			}
			
			if(dataGridView2.Rows[0].Cells[14].Value.ToString() != guarda_datos[14]){
				modi+="puesto=\""+guarda_datos[14]+"\",";
				aviso+="•Actividad Nova\n";
			}
			
			if(dataGridView2.Rows[0].Cells[15].Value.ToString() != guarda_datos[15]){
				modi+="tipo_trab=\""+guarda_datos[15]+"\",";
				aviso+="•Tipo de Trabajador\n";
			}
			
			if(llena_inf==0){
				if(adapta_fecha(dataGridView2.Rows[0].Cells[16].Value.ToString().Substring(0,10)) != guarda_datos[16]){
					modi+="contrato_ini=\""+guarda_datos[16]+"\",";
					aviso+="•Fecha de Inicio de Contrato\n";
				}
				
				if(adapta_fecha(dataGridView2.Rows[0].Cells[17].Value.ToString().Substring(0,10)) != guarda_datos[17]){
					modi+="contrato_fin=\""+guarda_datos[17]+"\",";
					aviso+="•Fecha de fin de Contrato\n";
				}
			}else{
					modi+="contrato_ini=\""+guarda_datos[16]+"\",";
					aviso+="•Fecha de Inicio de Contrato\n";
					
					modi+="contrato_fin=\""+guarda_datos[17]+"\",";
					aviso+="•Fecha de fin de Contrato\n";					
			}
			
			if(dataGridView2.Rows[0].Cells[18].Value.ToString() != guarda_datos[18]){
				modi+="nss_trab=\""+guarda_datos[18]+"\",";
				aviso+="•N.S.S. (Número de Seguro Social)\n";
			}
			
			if(dataGridView2.Rows[0].Cells[19].Value.ToString() != guarda_datos[19]){
				modi+="num_mat="+guarda_datos[19]+",";
				aviso+="•Matricula/N° Empleado\n";
			}
			
			if(dataGridView2.Rows[0].Cells[21].Value.ToString() != guarda_datos[21]){
				modi+="nom_usuario=\""+guarda_datos[21]+"\",";
				aviso+="•Nombre de Usuario\n";
			}
			
			if(dataGridView2.Rows[0].Cells[22].Value.ToString() != guarda_datos[22]){
				modi+="contrasena=AES_ENCRYPT(\""+guarda_datos[22]+"\",\"Nova Gear & AKD ATLAS & LZ RULES!!!\"),";
				aviso+="•Contraseña\n";
			}
			
			if(dataGridView2.Rows[0].Cells[25].Value.ToString() != guarda_datos[25]){
				modi+="rango="+guarda_datos[25]+",";
			}
			
			modi=modi.Substring(0,modi.Length-1);
			//MessageBox.Show(modi);
			sql2=" WHERE id_usuario="+id_bus+"";
			sql=sql+modi+sql2;
			
			info_mod[0]=sql;
			info_mod[1]=aviso;
			
			return info_mod;
			//}catch{
			//	return info_mod;
			//}
		}
		
		public void PictureBox2Click(object sender, EventArgs e)
		{
			if(mode!=3){
	            OpenFileDialog dialog = new OpenFileDialog();
				dialog.Filter = "Archivos de Imagen (*.JPG *.PNG *.BMP *.GIF)|*.jpg;*.png;*.bmp;*.gif";
				dialog.Title = "Seleccione el archivo de Imagen";//le damos un titulo a la ventana
				dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
				
				//si al seleccionar el archivo damos Ok
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					pictureBox2.ImageLocation = dialog.FileName;
					imagen = dialog.SafeFileName;
				}
			}
			
		}
        
       	public void Button1Click(object sender, EventArgs e)
		{
       		if(mode==1){
				if(verif_campos()==1){//todos los campos llenos
					if(verif_usuario()==1){//usuario disponible
							registrar_usuario();
					}else{
						MessageBox.Show("El Usuario que Eligió no se encuentra Disponible, Favor de elegir Otro.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
					}
				}else{
					MessageBox.Show("Falta Información de los Siguientes campos:\n\n"+error,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
				}
       		}
       		
       		if(mode==2){
       			if(verif_campos()==1){//todos los campos llenos
					if(verif_usuario()==1){//usuario disponible
       					modificar_usuario();
					}else{
						MessageBox.Show("El Usuario que Eligió no se encuentra Disponible, Favor de elegir Otro.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
					}
				}else{
					MessageBox.Show("Falta Información de los Siguientes campos:\n\n"+error,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
				}
       		}
       		
		}
		
		public void Reg_Usuarios_2Load(object sender, EventArgs e)
		{
			

			switch (mode){
			 	case 1: this.Text="Nova Gear: Registro de Usuario";
			 			label4.Text="Registro de Usuario";
			 	break;

				case 2: this.Text="Nova Gear: Modificación de Usuarios";
						label4.Text="Modificación de Usuarios";
						checkBox1.Visible=false;
						consultar();
			 	break;

				case 3: this.Text="Nova Gear: Detalle de Usuario";
						label4.Text="Detalle de Usuario";
						consultar();
						desactivar_campos();
			 	break;			 	
			 }

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

		}
		
		void ComboBox8SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox8.SelectedItem.ToString().Equals("Operativo TTD")){
				dateTimePicker2.Enabled=true;
				dateTimePicker3.Enabled=true;
			}else{
				dateTimePicker2.Enabled=false;
				dateTimePicker3.Enabled=false;				
			}
		}
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			//Terminos terms = new Terminos();
			//terms.ShowDialog();
			FileStream fichero;
			String ruta="";
			if(File.Exists(@"temp/cookie.lz")==true){
				System.IO.File.Delete(@"temp/cookie.lz");
			}
			fichero=System.IO.File.Create(@"temp/cookie.lz");
			ruta=fichero.Name;
			ruta=ruta.Substring(0,ruta.Length-9);
			ruta=ruta+"Manual_Nova.pdf#page=6";
			Navegador_web nav = new Navegador_web("file:///"+ruta);
			nav.Show();
		}
		
		void ComboBox6SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox6.SelectedIndex==0){
				comboBox7.Items.Clear();
				comboBox7.Items.Add("Jefe de Área");
				comboBox7.Items.Add("Auxiliar Multifunción");
				comboBox7.Items.Add("Auxiliar Estrados");
				comboBox7.Items.Add("Auxiliar Oficios");
				comboBox7.Items.Add("Auxiliar");
				comboBox7.Items.Add("Controlador");
				comboBox7.Items.Add("Notificador");
				comboBox7.Items.Add("Ventanilla");
				comboBox7.Items.Add("Cartera");
			}else{
				comboBox7.Items.Clear();
				comboBox7.Items.Add("Jefe de Área");
				comboBox7.Items.Add("Auxiliar");
			}
		}
		
		void ComboBox7SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
	}
}

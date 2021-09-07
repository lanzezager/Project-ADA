/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 14/07/2015
 * Hora: 09:50 a. m.
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
	/// Description of Usuarios.
	/// </summary>
	public partial class Usuarios : Form
	{
		public Usuarios(int tipo,int id_bus)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.tipo_func = tipo;
			this.id_cargar=id_bus;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String imagen,error,contrasena,conf_cons,sql,fecha,ruta,ruta_alt_pic,imagen_full,id_user,user,id_cont,fecha_ini,fecha_fin;
		int c0=0,c1=0,c2=0,c3=0,c4=0,c5=0,c6=0,c7=0,c8=0,c9=0,c10=0,c11=0,suma_c=0,rango=0,ind_img_repe=0,pic_loc=0,guion=0,savepic=0,digi_pic=0,userkregistro=0,tipo_func=0,id_cargar=0,i=0,user_repe=0,valid=0,c12=0,c13=0;
		int[] cont_not_asignados,not_cont_no_asignados;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		DataTable consultamysql = new DataTable();
		Conexion conex2 = new Conexion();
		DataTable consultamysql2 = new DataTable();
		
		public void carga_pic_users(){
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Archivos de Imagen (*.JPG *.PNG *.BMP *.GIF)|*.jpg;*.png;*.bmp;*.gif";
			dialog.Title = "Seleccione el archivo de Imagen";//le damos un titulo a la ventana
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				pictureBox1.ImageLocation = dialog.FileName;
				
				imagen = dialog.SafeFileName;
				button1.Visible = false;
				c10=1;
			}
			
		}
		
		public void guardar_pic(){
		try{

            ruta = conex.leer_config();
            ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
            ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
            
				if(!(File.Exists(ruta+imagen))){
					System.IO.File.Copy(pictureBox1.ImageLocation, ruta+imagen, false);
				}else{
					if (pic_loc == 0){
						ruta_alt_pic = "_0";
					}else{
						ruta_alt_pic = "";
					}
					imagen=imagen.Substring(0, imagen.Length-4)+ruta_alt_pic+imagen.Substring(imagen.Length-4,4);
					pic_loc = 1;
					if(!(File.Exists(ruta+imagen))){
						System.IO.File.Copy(pictureBox1.ImageLocation, ruta+imagen, false);
					}else{
						guion = imagen.Length-6;
						do{
							digi_pic=imagen.Length - (guion+5);
							ind_img_repe = Convert.ToInt32(imagen.Substring(guion+1,digi_pic));
							ind_img_repe =ind_img_repe+1;
							imagen=imagen.Substring(0, guion+1)+ind_img_repe+imagen.Substring(imagen.Length-4,4);
							
							if(!(File.Exists(ruta+imagen))){
								System.IO.File.Copy(pictureBox1.ImageLocation, ruta+imagen, false);
								savepic = 1;
							}
							
						}while(savepic != 1);
						savepic=0;
					}
				}

			}catch(IOException exc){
				MessageBox.Show("Ocurrio un problema al guardar la imagen de perfil\n\nError:\n"+exc,"Error");
			}
		
		}
		
		public void registrar_en_bd(){

            if((comboBox1.Text.Equals("Notificador"))||(comboBox1.Text.Equals("Controlador"))){
                if (c11 == 2)
                {
                    DialogResult result = MessageBox.Show("Este Notificador no tiene asignado ningún sector.\n\n¿Está seguro que desea continuar? ","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                    
                    }
                    else
                    {
                        c11 = 0;
                    }
                }
            }else{
                c11=1;
            }

			suma_c = c0+c1+c2+c3+c4+c5+c6+c7+c8+c9+c10+c11+c12+c13;
	
			if(suma_c >= 14){
				
				if(valid!=1){
					contrasena="";
				}
				
				if(textBox7.Text.Equals(contrasena)){
				
				conex.conectar("base_principal");
				sql="SELECT nom_usuario from usuarios where nom_usuario=\""+textBox5.Text+"\"";
				dataGridView1.DataSource=conex.consultar(sql);
				try{
					if(dataGridView1.RowCount > 0){
						user_repe=1;
						//MessageBox.Show(user_repe.ToString());
						if(tipo_func>1){
							//if(dataGridView1.Rows[0].Cells[0].Value.ToString().Equals(textBox1.Text)){
								user_repe=0;
							//}
						}	
					}else{
						user_repe=0;
					}
					
					 //MessageBox.Show(user_repe.ToString());
					
					if(user_repe < 1){
						MainForm mani = (MainForm)this.MdiParent;
				        fecha = mani.toolStripStatusLabel2.Text;
				        fecha = fecha.Substring(0,10);
				        fecha = fecha.Substring(6,4)+"/"+fecha.Substring(3,2)+"/"+fecha.Substring(0,2);
						guardar_pic();
						
						switch(comboBox1.SelectedItem.ToString()){
								
							case "Jefe de Area":rango=1;
		             		break;
		             		case "Jefe de Grupo":rango=2;
		             		break;
		             		case "Auxiliar Multifuncion":rango=2;
		             		break;
		             		case "Auxiliar Estrados":rango=2;
		             		break;
		             		case "Auxiliar":rango=5;
		             		break;
		             		case "Controlador":rango=3;
		             		break;
		             		case "Notificador":rango=4;
		             		break;	
							case "Atencion Ventanilla":rango=4;
		             		break;				             		
								
						}
						
						switch(comboBox2.SelectedItem.ToString()){
								
							case "Emisiones"://nada
		             		break;
		             		case "Cobranza":rango=rango+10;
		             		break;
		             		case "Registros":rango=rango+20;
		             		break;	
						}
						
                       // MessageBox.Show("|" + mani.datos_user[7] + "|");
						userkregistro = Convert.ToInt32(mani.datos_user[7]);
						user = textBox5.Text;
						
						fecha_ini=dateTimePicker1.Text;		
						fecha_fin=dateTimePicker2.Text;					
						fecha_ini=fecha_ini.Substring(6,4)+"-"+fecha_ini.Substring(3,2)+"-"+fecha_ini.Substring(0,2);
						fecha_fin=fecha_fin.Substring(6,4)+"-"+fecha_fin.Substring(3,2)+"-"+fecha_fin.Substring(0,2);
						//MessageBox.Show(fecha_ini+"|"+fecha_fin);
						if(tipo_func==1){
							sql="INSERT INTO usuarios (nombre,domicilio,celular,apellido,puesto,area,unidad,nom_usuario,contrasena,url_imagen,rango,fecha_registro,usuario_k_registro,estatus,num_mat,contrato_ini,contrato_fin) VALUES"+
								"(\""+textBox1.Text+"\",\""+textBox2.Text+"\",\""+textBox3.Text+"\",\""+textBox4.Text+"\",\""+comboBox1.SelectedItem.ToString()+"\",\""+comboBox2.SelectedItem.ToString()+"\",\""+comboBox3.SelectedItem.ToString()+"\",\""+textBox5.Text+"\",AES_ENCRYPT(\""+textBox7.Text+"\",\"Nova Gear & AKD ATLAS & LZ RULES!!!\"),\""+imagen+"\","+rango+",\""+fecha+"\","+userkregistro+",\"activo\","+textBox8.Text+",\""+fecha_ini+"\",\""+fecha_fin+"\")";
							conex.guardar_evento("Se Crea el usuario: "+textBox5.Text);
						}else{
							if(valid==1){	
								sql="UPDATE usuarios SET nombre=\""+textBox1.Text+"\",domicilio=\""+textBox2.Text+"\",celular=\""+textBox3.Text+"\",apellido=\""+textBox4.Text+"\",puesto=\""+comboBox1.SelectedItem.ToString()+"\",area=\""+comboBox2.SelectedItem.ToString()+"\",unidad=\""+comboBox3.SelectedItem.ToString()+"\","+
								"nom_usuario=\""+textBox5.Text+"\",contrasena=AES_ENCRYPT(\""+textBox7.Text+"\",\"Nova Gear & AKD ATLAS & LZ RULES!!!\"),url_imagen=\""+imagen+"\",rango="+rango+",num_mat="+textBox8.Text+",contrato_ini=\""+fecha_ini+"\",contrato_fin=\""+fecha_fin+"\" WHERE id_usuario= "+id_cargar+";";
								}else{
									sql="UPDATE usuarios SET nombre=\""+textBox1.Text+"\",domicilio=\""+textBox2.Text+"\",celular=\""+textBox3.Text+"\",apellido=\""+textBox4.Text+"\",puesto=\""+comboBox1.SelectedItem.ToString()+"\",area=\""+comboBox2.SelectedItem.ToString()+"\",unidad=\""+comboBox3.SelectedItem.ToString()+"\","+
									"url_imagen=\""+imagen+"\",rango="+rango+",num_mat="+textBox8.Text+",contrato_ini=\""+fecha_ini+"\",contrato_fin=\""+fecha_fin+"\" WHERE id_usuario= "+id_cargar+";";
								}
							conex.guardar_evento("Se Modificó el usuario: "+textBox5.Text);
						}
						conex.consultar(sql);
						
						
						if(comboBox1.SelectedItem.ToString().Equals("Controlador")){
							registrar_controlador();
						}else{
							if(comboBox1.SelectedItem.ToString().Equals("Notificador")){
								registrar_notificador();
							}
						}
						
						
					}else{
						MessageBox.Show("El usuario ya existe","Error");
					}
				}catch(Exception exc){
					MessageBox.Show("Ocurrio un problema al momento del registro\n\nError:\n"+exc,"Error");
				}
				if(user_repe==0){
					if(tipo_func==1){
						MessageBox.Show("Registro Exitoso","Exito");
					}else{
						MessageBox.Show("Se Modificó el Registro Exitosamente","Exito");
					}
					this.Close();
				}
				
				}else{
					MessageBox.Show("Las Contraseñas no coinciden","Error");
				}
				
			}else{
				error="Los siguientes campos quedaron vacíos o fueron llenados incorrectamente: \n\n";
				if(c0==0){error+="Nombre\n";}
				if(c3==0){error+="Apellidos\n";}
				if(c1==0){error+="Domicilio\n";}
				if(c2==0){error+="Información de Contacto\n";}
				if(c10==0){error+="Fotografía\n";}
				if(c12==0){error+="Matrícula/N° Empleado\n";}
				if(c13==0){error+="Fecha Contrato\n";}
				if(c4==0){error+="Puesto\n";}
				if(c11==0){error+="Asignación de Sectores/Notificadores\n";}
				if(c5==0){error+="Área:\n";}
				if(c6==0){error+="Unidad\n";}
				if(c7==0){error+="Nombre de Usuario\n";}
				if(c8==0){error+="Contraseña\n";}
				if(c9==0){error+="Confirmación de Contraseña\n";}
				
				
				MessageBox.Show(error,"Error al Ingresar Usuario");
			}
            c11 = 0;
		}
		
		public void registrar_controlador(){
			sql="SELECT id_usuario from usuarios where nom_usuario=\""+user+"\"";
			dataGridView1.DataSource=conex.consultar(sql);
			id_user=dataGridView1.Rows[0].Cells[0].Value.ToString();
			
			i=0;
            if (cont_not_asignados.Length > 0)
            {
                do
                {
                    sql = "UPDATE usuarios SET controlador= \"" + id_user + "\" WHERE id_usuario =" + cont_not_asignados[i] + " ;";
                    conex.consultar(sql);

                    sql = "UPDATE sectores SET id_controlador= \"" + id_user + "\" WHERE id_notificador =" + cont_not_asignados[i] + " ;";
                    conex.consultar(sql);

                    i++;
                } while (i < cont_not_asignados.Length);
            }

			i=0;
            if (not_cont_no_asignados.Length > 0)
            {
                do
                {
                    sql = "UPDATE usuarios SET controlador= \"0\" WHERE id_usuario =" + not_cont_no_asignados[i] + " ;";
                    conex.consultar(sql);

                    sql = "UPDATE sectores SET id_controlador= \"0\" WHERE id_notificador =" + not_cont_no_asignados[i] + " ;";
                    conex.consultar(sql);

                    i++;
                } while (i < not_cont_no_asignados.Length);
            }
		}
		
		public void registrar_notificador(){

			sql="SELECT id_usuario,controlador from usuarios where nom_usuario=\""+user+"\"";
			dataGridView1.DataSource=conex.consultar(sql);
			id_user=dataGridView1.Rows[0].Cells[0].Value.ToString();
			id_cont=dataGridView1.Rows[0].Cells[1].Value.ToString();
			
			i=0;
            if (c11 == 1)
            {
            	while (i < cont_not_asignados.Length){
                       sql = "UPDATE sectores SET id_notificador= " + id_user + ",id_controlador=" + id_cont + " WHERE sector =" + cont_not_asignados[i] + " ;";
                       conex.consultar(sql);
                       i++;
                    }
            }

			i=0;
            
			    while(i<not_cont_no_asignados.Length){
				    sql="UPDATE sectores SET id_notificador= 0, id_controlador= 0 WHERE sector ="+not_cont_no_asignados[i]+" ;";
				    conex.consultar(sql);
				    i++;
			    }
            
		}
		
		public void cargar_usuario(){
			
			if(tipo_func==1){
             	button1.Visible=true;
             }else{
             	conex.conectar("base_principal");
             	conex2.conectar("base_principal");
             	sql="SELECT nombre,domicilio,celular,apellido,puesto,area,unidad,nom_usuario,contrasena,url_imagen,num_mat,contrato_ini,contrato_fin FROM usuarios WHERE id_usuario="+id_cargar;
             	consultamysql=conex.consultar(sql);
             	dataGridView1.DataSource=consultamysql;
             	
             	button1.Visible=false;
             	textBox1.Text=dataGridView1.Rows[0].Cells[0].Value.ToString();
             	textBox2.Text=dataGridView1.Rows[0].Cells[1].Value.ToString();
             	textBox3.Text=dataGridView1.Rows[0].Cells[2].Value.ToString();
             	textBox4.Text=dataGridView1.Rows[0].Cells[3].Value.ToString();
             	textBox8.Text=dataGridView1.Rows[0].Cells[10].Value.ToString();
             	if(dataGridView1.Rows[0].Cells[11].Value.ToString().Length>9){
             		dateTimePicker1.Value=Convert.ToDateTime(dataGridView1.Rows[0].Cells[11].Value.ToString());
             		dateTimePicker2.Value=Convert.ToDateTime(dataGridView1.Rows[0].Cells[12].Value.ToString());
             	}
             	
             	switch(dataGridView1.Rows[0].Cells[4].Value.ToString()){
             		case "Jefe de Area":comboBox1.SelectedIndex=0;
             							c11=1;
             							break;
             		case "Jefe de Grupo":comboBox1.SelectedIndex=1;
             							c11=1;
             							break;
             		case "Auxiliar Multifuncion":comboBox1.SelectedIndex=2;
             		             		c11=1;
             							break;
             		case "Auxiliar Estrados":comboBox1.SelectedIndex=3;
             		             		c11=1;
             							break;
             		case "Auxiliar":comboBox1.SelectedIndex=4;
             		             		c11=1;
             							break;
             							
             		case "Controlador":comboBox1.SelectedIndex=5;
             			sql="SELECT id_usuario FROM usuarios WHERE controlador =\""+id_cargar+"\";";
             			consultamysql2=conex2.consultar(sql);
             			dataGridView2.DataSource=consultamysql2;
             		
             			if(dataGridView2.RowCount>0){
             				pictureBox14.Image = global::Nova_Gear.Properties.Resources.accept16;
							c11=1;
             			}
             			
             		break;
             		case "Notificador":comboBox1.SelectedIndex=6;
             			sql="SELECT id_sectores FROM sectores WHERE id_notificador =\""+id_cargar+"\";";
             			consultamysql2=conex2.consultar(sql);
             			dataGridView2.DataSource=consultamysql2;
             		
             			if(dataGridView2.RowCount>0){
             				pictureBox14.Image = global::Nova_Gear.Properties.Resources.accept16;
							c11=1;
             			}
             			

             		break;

				    case "Atencion Ventanilla":comboBox1.SelectedIndex=7;
             		             	c11=1;
             						break;             		
             	}
             	
             	switch(dataGridView1.Rows[0].Cells[5].Value.ToString()){
             		case "Emisiones":comboBox2.SelectedIndex=0;
             		break;
             		case "Registros":comboBox2.SelectedIndex=1;
             		break;
             		case "Cobranza":comboBox2.SelectedIndex=2;
             		break;	
             	}
             	
             	comboBox3.SelectedIndex=0;
             	
             	textBox5.Text=dataGridView1.Rows[0].Cells[7].Value.ToString();
             	textBox6.Text=dataGridView1.Rows[0].Cells[8].Value.ToString();

                ruta = conex.leer_config();
                ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
                ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";

             	pictureBox1.ImageLocation =ruta+dataGridView1.Rows[0].Cells[9].Value.ToString();
             	imagen=dataGridView1.Rows[0].Cells[9].Value.ToString();	
             	c10=1;
             	
             	if(tipo_func==3){
             		textBox1.ReadOnly=true;
             		textBox2.ReadOnly=true;
             		textBox3.ReadOnly=true;
             		textBox4.ReadOnly=true;
             		
             		textBox8.ReadOnly=true;
             		dateTimePicker1.Enabled=false;
             		dateTimePicker2.Enabled=false;
             		comboBox1.Enabled=false;
             		comboBox2.Enabled=false;
             		comboBox3.Enabled=false;
             		//button6.Enabled=false;
             		
             		textBox5.ReadOnly=true;
             		textBox6.ReadOnly=true;
             		textBox7.ReadOnly=true;
             		
             		button2.Enabled=false;
             		//button3.Enabled=false;
             		button4.Enabled=false;
             	}
             }
		}
		
		void UsuariosLoad(object sender, EventArgs e)
		{
			String window_name = this.Text;
			window_name = window_name.Replace("Nova Gear", "Gear Prime");
			this.Text = window_name;

			this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;
			 
			 switch(tipo_func){
			 	case 1: this.Text="Nova Gear: Registro de Usuarios";
			 	break;

				case 2: this.Text="Nova Gear: Modificación de Usuarios";
			 	break;

				case 3: this.Text="Nova Gear: Detalle de Usuarios";
			 	break;			 	
			 }
			 
			 toolTip1.SetToolTip(button4,"Cambiar Imagen");
             toolTip1.SetToolTip(button5,"Ver Imagen en otra ventana");
             toolTip2.SetToolTip(pictureBox3,"Todos los Campos son obligatorios.\nPuede utilizar las iniciales de su nombre\no su mátricula como usuario.\nSe recomienda ingresar una contraseña\nque incluya mayúsculas, minúsculas y números.");     
 
             cargar_usuario();
		
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			carga_pic_users();
		}
		
		void PictureBox1Click(object sender, EventArgs e)
		{
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if(tipo_func==2){//si modifica
				if(textBox7.Text.Length <= 0){//si no hay pass
					//pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.accept16;
					//c8=1;
					c9=1;
					valid=0;
				}else{
					valid=1;
				}
			}else{
				valid=1;
			}
			
			registrar_en_bd();
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			if((textBox1.Text.Length > 0)){
			pictureBox4.Image = global::Nova_Gear.Properties.Resources.error;
			c0=0;
			
			if(textBox1.Text.Length > 5){
				pictureBox4.Image = global::Nova_Gear.Properties.Resources.accept16;
				c0=1;
			}
			
			}else{
				pictureBox4.Image = global::Nova_Gear.Properties.Resources.cancel;
					c0=0;	
			}
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			if((textBox2.Text.Length > 0)){
			pictureBox5.Image = global::Nova_Gear.Properties.Resources.error;
			c1=0;
			
			if(textBox2.Text.Length > 14){
				pictureBox5.Image = global::Nova_Gear.Properties.Resources.accept16;
				c1=1;
				}
			}else{
					pictureBox5.Image = global::Nova_Gear.Properties.Resources.cancel;
					c1=0;	
			}
		}
		
		void TextBox3TextChanged(object sender, EventArgs e)
		{
			if((textBox3.Text.Length > 0)){
			pictureBox6.Image = global::Nova_Gear.Properties.Resources.error;
			c2=0;
			
			if(textBox3.Text.Length > 9){
				pictureBox6.Image = global::Nova_Gear.Properties.Resources.accept16;
				c2=1;
			}
			}else{
				pictureBox6.Image = global::Nova_Gear.Properties.Resources.cancel;
				c2=0;		
			}
		}
		
		void TextBox4TextChanged(object sender, EventArgs e)
		{
			if((textBox4.Text.Length > 0)){
			pictureBox7.Image = global::Nova_Gear.Properties.Resources.error;
			c3=0;
			
				if((textBox4.Text.Length > 9)){
					pictureBox7.Image = global::Nova_Gear.Properties.Resources.accept16;
					c3=1;
				}
			}else{
				if(textBox4.Text.Length < 1){
					pictureBox7.Image = global::Nova_Gear.Properties.Resources.cancel;	
				}		
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedItem == null){
				pictureBox8.Image = global::Nova_Gear.Properties.Resources.error;
				c4=0;
			}else{
				pictureBox8.Image = global::Nova_Gear.Properties.Resources.accept16;
				c4=1;
				
				if(comboBox1.SelectedItem.ToString().Equals("Notificador")){
					this.button6.Image = global::Nova_Gear.Properties.Resources.table_select;
					button6.Visible=true;
					pictureBox14.Visible=true;
					toolTip1.SetToolTip(button6,"Selector de Sectores");
				}else{
					if(comboBox1.SelectedItem.ToString().Equals("Controlador")){
						this.button6.Image = global::Nova_Gear.Properties.Resources.edit_recipient_list;
						button6.Visible=true;
						pictureBox14.Visible=true;
						toolTip1.SetToolTip(button6,"Selector de Notificadores");
					}else{
						button6.Visible=false;
						pictureBox14.Visible=false;
					}
				}
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedItem == null){
				pictureBox9.Image = global::Nova_Gear.Properties.Resources.error;
				c5=0;
			}else{
				c5=1;
				pictureBox9.Image = global::Nova_Gear.Properties.Resources.accept16;
			}
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedItem == null){
				pictureBox10.Image = global::Nova_Gear.Properties.Resources.error;
				c6=0;
			}else{
				pictureBox10.Image = global::Nova_Gear.Properties.Resources.accept16;
				c6=1;
			}
		}
		
		void TextBox5TextChanged(object sender, EventArgs e)
		{
			if((textBox5.Text.Length > 0)&&(textBox5.Text.Length < 5)){
			pictureBox11.Image = global::Nova_Gear.Properties.Resources.error;
			c7=0;
			}else{
				if(textBox5.Text.Length > 4){
				pictureBox11.Image = global::Nova_Gear.Properties.Resources.accept16;
				c7=1;
				}else{
					c7=0;
					pictureBox11.Image = global::Nova_Gear.Properties.Resources.cancel;	
				}
			}
		}
		
		void TextBox6Enter(object sender, EventArgs e)
		{
			if(textBox7.Text.Length > 5){
			   if(textBox7.Text == textBox6.Text){
				c8=1;
				c9=1;				
			   	pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
			   	pictureBox13.Image = global::Nova_Gear.Properties.Resources.accept16;
			   }else{
				c8=0;	
			   	pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
			   }
			}
		}
		
		void TextBox6TextChanged(object sender, EventArgs e)
		{
			if((textBox6.Text.Length > 0)&&(textBox6.Text.Length < 6)){
			pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
			c8=0;
			}else{
				if(textBox7.Text.Length > 5){
				if((textBox6.Text.Length > 5)&&(textBox7.Text.Equals(textBox6.Text))){
				c8=1;
				c9=1;
				pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
				pictureBox13.Image = global::Nova_Gear.Properties.Resources.accept16;
				
				}else{
						c8=0;
						c9=0;
						if(textBox6.Text.Length < 1){
							pictureBox12.Image = global::Nova_Gear.Properties.Resources.cancel;
						}else{
							pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
							pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
							
							
						}
					}
				}else{
					if(textBox6.Text.Length > 5){
				pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
				c8=1;
				}else{
						c8=0;
						if(textBox6.Text.Length < 1){
							pictureBox12.Image = global::Nova_Gear.Properties.Resources.cancel;
						}else{
							pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
						}
					}
					
				}
			}
			
		}
		
		void TextBox6Leave(object sender, EventArgs e)
		{
			if(textBox7.Text.Length > 5){
				if(textBox7.Text.Equals(textBox6.Text)){
					pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.accept16;
					c8=1;
					c9=1;
					contrasena = textBox7.Text;
				}
			}
		}
		
		void TextBox7TextChanged(object sender, EventArgs e)
		{
			
				if((textBox7.Text.Length > 0)&&(textBox7.Text.Length < 6)){
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.error;
					c9=0;
				}else{
					if(textBox7.Text.Equals(textBox6.Text)){
						c8=1;
						c9=1;
						pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
						pictureBox13.Image = global::Nova_Gear.Properties.Resources.accept16;
						
					}else{
					c9=0;
						if(textBox7.Text.Length < 1){
							pictureBox13.Image = global::Nova_Gear.Properties.Resources.cancel;
						}else{
							pictureBox13.Image = global::Nova_Gear.Properties.Resources.error;
						}
					}
				}
			//}
		}
		
		void TextBox7Enter(object sender, EventArgs e)
		{
			if(textBox6.Text.Length > 5){
				if(textBox7.Text.Equals(textBox6.Text)){
					pictureBox12.Image = global::Nova_Gear.Properties.Resources.accept16;
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.accept16;
					c8=1;
					c9=1;
				}else{				
					pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.error;
					c8=0;
					c9=0;
				}
			}else{
				if(textBox7.Text.Length <= 5){
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.error;
				    }
				if(textBox7.Text.Length < 1){
					pictureBox13.Image = global::Nova_Gear.Properties.Resources.cancel;
					
				}
			}
		}
		
		void TextBox7Leave(object sender, EventArgs e)
		{
			if(textBox7.Text.Equals(textBox6.Text)){
			   	contrasena = textBox7.Text;
			   	c8=1;
			   	c9=1;
			   }else{
			   	pictureBox12.Image = global::Nova_Gear.Properties.Resources.error;
			   	pictureBox13.Image = global::Nova_Gear.Properties.Resources.error;
			   	c8=0;
			   	c9=0;
			   }
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			carga_pic_users();
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			imagen_full = pictureBox1.ImageLocation;
			Foto_ampliada form_fa = new Foto_ampliada(imagen_full,imagen);
			MainForm mani = (MainForm)this.MdiParent;
			form_fa.MdiParent=mani;
			form_fa.Show();	
		}
		
		void Button4MouseEnter(object sender, EventArgs e)
		{
			
		}
		
		void Button4MouseLeave(object sender, EventArgs e)
		{
			
		}
		
		void Button5MouseEnter(object sender, EventArgs e)
		{
		}
		
		void Button5MouseLeave(object sender, EventArgs e)
		{
			
			
		}
		
		void PictureBox1MouseEnter(object sender, EventArgs e)
		{
			//button4.Visible = true;
			//button5.Visible = true;
		}
		
		void PictureBox1MouseLeave(object sender, EventArgs e)
		{
			//button4.Visible = false;
			//button5.Visible = false;
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			MainForm mani = (MainForm)this.MdiParent;
			
			if(comboBox1.SelectedItem.ToString().Equals("Controlador")){
				Selector_notificador select_not = new Selector_notificador(id_cargar);
				select_not.MdiParent=mani;
				select_not.Show();
				select_not.Focus();
				//MessageBox.Show(mani.ActiveMdiChild.Name);
				this.Enabled=false;
				timer1.Enabled=true;
				
			}
			
			if(comboBox1.SelectedItem.ToString().Equals("Notificador")){
				Selector_sector select_sec = new Selector_sector(id_cargar,0);
				select_sec.MdiParent=mani;
				select_sec.Show();
				select_sec.Focus();
				this.Enabled=false;
				timer1.Enabled=true;
			}
				
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			MainForm mani = (MainForm)this.MdiParent;
			if(mani.activo == 1){
					this.Enabled=true;
					//MessageBox.Show("termino modificacion");
					timer1.Enabled=false;
					this.Focus();
					this.cont_not_asignados = new int[mani.asig.Length];
					this.not_cont_no_asignados=new int[mani.no_asig.Length];
					cont_not_asignados = mani.asig;
					not_cont_no_asignados=mani.no_asig;
					mani.activo=0;
					//MessageBox.Show(""+cont_not_asignados[0]+","+cont_not_asignados.Length);
					//MessageBox.Show(""+not_cont_no_asignados[0]+","+not_cont_no_asignados.Length);
					if(cont_not_asignados.Length>0){
						pictureBox14.Image = global::Nova_Gear.Properties.Resources.accept16;
						c11=1;
					}else{
						pictureBox14.Image = global::Nova_Gear.Properties.Resources.cancel;
						c11=2;
					}
			}
		}
		
		void GroupBox2Enter(object sender, EventArgs e)
		{
			
		}
		
		void TextBox8TextChanged(object sender, EventArgs e)
		{
			Int64 num=0;
			if((textBox8.Text.Length > 0)){
			pictureBox15.Image = global::Nova_Gear.Properties.Resources.error;
			c12=0;
			if(Int64.TryParse(textBox8.Text, out num)==true){
				if(textBox8.Text.Length > 2){
					pictureBox15.Image = global::Nova_Gear.Properties.Resources.accept16;
					c12=1;
				}
			}
			}else{
				pictureBox15.Image = global::Nova_Gear.Properties.Resources.cancel;
					c12=0;	
			}
		}
		
		void DateTimePicker2ValueChanged(object sender, EventArgs e)
		{
			if((dateTimePicker2.Value > dateTimePicker1.Value)){
				pictureBox16.Image = global::Nova_Gear.Properties.Resources.accept16;
				c13=1;
			}else{
				if((dateTimePicker2.Value <= dateTimePicker1.Value)){
					pictureBox16.Image = global::Nova_Gear.Properties.Resources.error;
					c13=0;
				}else{
					pictureBox16.Image = global::Nova_Gear.Properties.Resources.cancel;
				c13=0;
				}
			}
		}
	}
}

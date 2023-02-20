/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 * Fecha: 30/07/2015
 * Hora: 08:34 a. m.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Threading;
using System.IO;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Pantalla.
	/// </summary>
	public partial class Pantalla : Form
	{
		public Pantalla()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String mensaje,sql,nombre_full,fecha_servidor="";
		int error=0,cuenta=0,err_cuenta=0;
		public int login_exitoso=0;
		string[] nombre,datos;
        public static int estado_pantalla=0;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		
		//Voz de Bienvenida
		SpeechSynthesizer voz = new SpeechSynthesizer();
		
		private Thread hilosecundario = null;
		
		public void ingreso(){
			int error_fatal=0;
			error=0;
			DataTable tabla = new DataTable();
			
			mensaje="Ingrese correctamente:\n\n";
			
			if((textBox1.Text.Equals("Usuario..."))||(textBox1.Text.Length < 1)){
				mensaje+="Usuario\n";
				error=1;
			}
			
			if((textBox2.Text.Equals("Contraseña..."))||(textBox2.Text.Length < 1)){
				mensaje+="Contraseña\n";
				error=1;
			}
			
			if(error==1){
				MessageBox.Show(mensaje,"Error");
			}else{
				conex.conectar("base_principal");
				sql="SELECT nom_usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena FROM usuarios WHERE nom_usuario=\"lanzezager\"";
				tabla=conex.consultar(sql);
				
				if(tabla.Rows.Count>0){
					//if((Convert.ToDateTime(fecha_servidor) < Convert.ToDateTime("14/01/2019")==true)){
						if((tabla.Rows[0][1].ToString()).Equals("Rojinegro 100% Novatlas")){
							sql="SELECT nom_usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) contrasena ,rango,url_imagen,nombre,puesto,area,id_usuario,estatus,apellido FROM usuarios WHERE nom_usuario = \""+textBox1.Text+"\" AND contrasena = AES_ENCRYPT(\""+textBox2.Text+"\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\")";
							//sql="SELECT nom_usuario,contrasena,rango,url_imagen,nombre,puesto,area,id_usuario,estatus,apellido FROM usuarios WHERE nom_usuario = \"lanzezager\"";
							//MessageBox.Show(sql);
							dataGridView1.DataSource=conex.consultar(sql);
						}else{
							error_fatal=1;
						}
					/*}else{
						if((tabla.Rows[0][1].ToString()).Equals("Novatlas2019")){
							sql="SELECT nom_usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) contrasena ,rango,url_imagen,nombre,puesto,area,id_usuario,estatus,apellido FROM usuarios WHERE nom_usuario = \""+textBox1.Text+"\" AND contrasena = AES_ENCRYPT(\""+textBox2.Text+"\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\")";
							//sql="SELECT nom_usuario,contrasena,rango,url_imagen,nombre,puesto,area,id_usuario,estatus,apellido FROM usuarios WHERE nom_usuario = \"lanzezager\"";
							//MessageBox.Show(sql);
							dataGridView1.DataSource=conex.consultar(sql);
						}else{
							error_fatal=1;
						}
					}*/
				}else{
					error_fatal=1;
				}
				
				if(dataGridView1.RowCount > 0){
					if((dataGridView1.Rows[0].Cells[8].Value.ToString()).Equals("activo")){
						if(((dataGridView1.Rows[0].Cells[0].Value.ToString()).Equals(textBox1.Text))&&((dataGridView1.Rows[0].Cells[1].Value.ToString()).Equals(textBox2.Text)))
						{
							//MessageBox.Show("Acceso Concedido","Nova Gear");
							this.pictureBox2.BackgroundImage = global::Nova_Gear.Properties.Resources.acceso1_2;
							pictureBox2.Visible=true;
							pictureBox3.Visible = true;
							label1.Visible=true;
							label1.Refresh();
							pictureBox2.Refresh();
							pictureBox3.Refresh();
						//	button2.Enabled=true;
						//	button2.Visible=true;
						//	button2.Focus();
							textBox2.ReadOnly=true;
							//cuenta=0;
							//timer2.Enabled = true;
							//timer1.Enabled = true;
							//hilosecundario = new Thread(new ThreadStart(entrar));
							//hilosecundario.Start();
							
							entrar();
							
						}else{
							this.pictureBox2.BackgroundImage = global::Nova_Gear.Properties.Resources.acceso2_2;
							pictureBox2.Visible=true;
						}
					}else{
					MessageBox.Show("El Usuario se encuentra desactivado, no puede iniciar sesión","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					textBox1.Text="";
					textBox2.Text="";
					textBox1.Focus();
				}
				}else{
					if(error_fatal==1){
						this.pictureBox2.BackgroundImage = global::Nova_Gear.Properties.Resources.jollyRoger;
					}else{
						this.pictureBox2.BackgroundImage = global::Nova_Gear.Properties.Resources.acceso2_2;
					}
					pictureBox2.Visible=true;
					ee();
					//MessageBox.Show("Acceso Denegado","Nova Gear");
				}
				
			}
		}
		
		public void entrar(){
			
			nombre_full = dataGridView1.Rows[0].Cells[4].Value.ToString();
			nombre = nombre_full.Split(' ');
			if(nombre.Length >= 2){
				nombre_full = nombre[0] +" "+ nombre[1];
			}else{
				nombre_full = nombre[0];
			}
			
			if(dataGridView1.Rows[0].Cells[4].Value.ToString().Contains(" ")==true){
				nombre_full = dataGridView1.Rows[0].Cells[4].Value.ToString().Substring(0,(dataGridView1.Rows[0].Cells[4].Value.ToString().IndexOf(' '))+1);
			}else{
				nombre_full = dataGridView1.Rows[0].Cells[4].Value.ToString().Substring(0,(dataGridView1.Rows[0].Cells[4].Value.ToString().Length));
			}
			
			if(dataGridView1.Rows[0].Cells[9].Value.ToString().Contains(" ")==true){
				nombre_full = nombre_full+" "+dataGridView1.Rows[0].Cells[9].Value.ToString().Substring(0,(dataGridView1.Rows[0].Cells[9].Value.ToString().IndexOf(' '))+1);
			}else{
				nombre_full = nombre_full+" "+dataGridView1.Rows[0].Cells[9].Value.ToString().Substring(0,(dataGridView1.Rows[0].Cells[9].Value.ToString().Length));
			}
			
			datos = new string[10];
			datos[0]= dataGridView1.Rows[0].Cells[0].Value.ToString();//nombre_user
			datos[1]= dataGridView1.Rows[0].Cells[1].Value.ToString();//contraseña
			datos[2]= dataGridView1.Rows[0].Cells[2].Value.ToString();//rango
			datos[3]= dataGridView1.Rows[0].Cells[3].Value.ToString();//url_imagen
			datos[4]= dataGridView1.Rows[0].Cells[4].Value.ToString();//nombre
			datos[5]= dataGridView1.Rows[0].Cells[5].Value.ToString();//puesto
			datos[6]= dataGridView1.Rows[0].Cells[6].Value.ToString();//area
			datos[7]= dataGridView1.Rows[0].Cells[7].Value.ToString();//id_usuario
			datos[8]= dataGridView1.Rows[0].Cells[8].Value.ToString();//estatus
			datos[9]= dataGridView1.Rows[0].Cells[9].Value.ToString();//apellido
			
			//fecha_servidor="10/01/2019";
			//MessageBox.Show(""+Convert.ToDateTime(fecha_servidor));
			
			//if((Convert.ToDateTime(fecha_servidor) < Convert.ToDateTime("14/01/2019")==true)){
				
				pictureBox3.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Interfaz Gráfica.";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Interfaz Gráfica..";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Interfaz Gráfica...";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Base de Datos.";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Base de Datos..";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Base de Datos...";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Datos de Usuario.";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Datos de Usuario...";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Configuración Inicial.";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				label1.Text="Cargando Configuración Inicial...";
				pictureBox3.Refresh();
				label1.Refresh();
				Thread.Sleep(500);
				pictureBox3.Refresh();
				
				this.Hide();
				this.Width=1;
				this.Height=1;
				this.Opacity=0;
				this.ShowIcon=false;
				this.ShowInTaskbar=false;
				
				//this.Visible = false;
				MainForm mani = new MainForm();
				mani.log_user(datos);
				mani.Show();
				
				//MessageBox.Show(this.HasChildren.ToString());
				//this.Hide();
				//voz.Speak("¡Arriba el Atlas! y que chingue a su madre el américa");
				
				pictureBox2.Visible=false;
				//**timer1.Enabled=false;
				try
				{
					//MessageBox.Show("" + nombre_full + "\nBienvenid@ a Nova Gear", "LOGIN EXITOSO");
					voz.Speak("" + nombre_full + ", Welcome to Nova Gear");
				}
				catch
				{
					MessageBox.Show("" + nombre_full + "\nBienvenid@ a Nova Gear","LOGIN EXITOSO");
				}
				
				/*if(DateTime.Today > Convert.ToDateTime("2017-12-31 23:59:00")){
	            if(File.Exists(@"despedida.t")==false){
	            	File.Create(@"despedida.t");
	            	MessageBox.Show("Hasta Luego\nGracias por TODO\n    L.Z.","GRACIAS",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
	            	MessageBox.Show("ARRIBA EL ATLAS\nFIEL X LOS COLORES","ROJINEGRO DE CORAZÓN",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
	            	File.Copy(@"C:\CDI\Tapiz.bmp",@"C:\CDI\Tapiz_feo.bmp");
	            	File.Copy(@"\\11.13.14.164\Nova_Gear\Recursos\Imagenes\Usuarios\Tapiz.bmp",@"c:\CDI\Tapiz.bmp",true);
	            }
            	}*/
				//Guarda_Evento save = new Guarda_Evento();
				//save.guardar("Inicio sesión",datos[0]);
				
			/*}else{
				if((datos[6].Equals("Registros")==true)&&(Convert.ToInt32(datos[2])>20)){
				   	pictureBox3.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Interfaz Gráfica.";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Interfaz Gráfica..";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Interfaz Gráfica...";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Base de Datos.";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Base de Datos..";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Base de Datos...";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Datos de Usuario.";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Datos de Usuario...";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Configuración Inicial.";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	label1.Text="Cargando Configuración Inicial...";
				   	pictureBox3.Refresh();
				   	label1.Refresh();
				   	Thread.Sleep(500);
				   	pictureBox3.Refresh();
				   	
				   	this.Hide();
				   	this.Width=1;
				   	this.Height=1;
				   	this.Opacity=0;
				   	this.ShowIcon=false;
				   	this.ShowInTaskbar=false;
				   	
				   	//this.Visible = false;
				   	MainForm mani = new MainForm();
				   	mani.log_user(datos);
				   	mani.Show();
				   	
				   	//MessageBox.Show(this.HasChildren.ToString());
				   	//this.Hide();
				   	//voz.Speak("¡Arriba el Atlas! y que chingue a su madre el américa");
				   	
				   	pictureBox2.Visible=false;
				   	//**timer1.Enabled=false;
				   	try
				   	{
				   		//MessageBox.Show("" + nombre_full + "\nBienvenid@ a Nova Gear", "LOGIN EXITOSO");
				   		voz.Speak("" + nombre_full + ", Welcome to Nova Gear");
				   	}
				   	catch
				   	{
				   		MessageBox.Show("" + nombre_full + "\nBienvenid@ a Nova Gear","LOGIN EXITOSO");
				   	}
				}else{
					conex.consultar("UPDATE usuarios SET contrasena =\"Arriba el ATLAS!!!\" WHERE id_usuario=1 ");
					MessageBox.Show("Lo sentimos el periodo de la licencia ha terminado","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Close();
				}
			}*/
		}
		
		public void actualizaciones(){
			
			String version, copyright,dont,lz,atlas,url,nom_arch,nueva_url,ruta,url_raiz,dato_extra;
			int candado=0,y=0,z=0,msi_tots=0,k=0,ind_msi=0,cacha_error=0;
			decimal ver_pro=0,ver_act=0;
			String[] url_act;
			FileStream fichero;
			
			try{
			    StreamReader rdr = new StreamReader(@"NG_info.lz");
			    cacha_error++;//contador para saber si falló la linea anterior
			    version = @""+rdr.ReadLine();
			    url = rdr.ReadLine();
			    dato_extra=rdr.ReadLine();
			    dato_extra=rdr.ReadLine();
			    dato_extra=rdr.ReadLine();
			    copyright = rdr.ReadLine();
			    dont = rdr.ReadLine();
			    lz = rdr.ReadLine();
			    atlas = rdr.ReadLine();
			    rdr.Close();
			
			    if(atlas.Equals("Arriba el Atlas!!!!!")){
				    candado++;
			    }
			    if(lz.Equals("By LZ")){
				    candado++;
			    }
			    if(dont.Equals("DON'T CHANGE THIS SETTINGS!!!!!")){
				    candado++;
			    }
			    if(copyright.Equals("Nova Gear by Mario Espinoza & Miguel Bañuelos")){
				    candado++;
			    }
			    if(copyright.Equals("Nova Gear by Mario Espinoza & Miguel Bañuelos")){
				    candado++;
			    }
			
			    if(candado==5){
				    version=version.Substring(8,version.Length-8);
				    url=url.Substring(4,url.Length-4);
				    url_act = new string[10];
                    if (Directory.Exists(url))
                    {
                        try
                        {
                            //MessageBox.Show(url);
                            url_act = Directory.GetFiles(url);
                            //MessageBox.Show(url_act[0]);
                            //MessageBox.Show(url+"|"+version+"|"+url_act[0].ToString()+"|"+url_act[1].ToString());

                            while (k < url_act.Length)
                            {
                                if (url_act[k].EndsWith(".msi") || url_act[k].EndsWith(".MSI"))
                                {
                                    msi_tots++;
                                    ind_msi = k;
                                }
                                k++;
                            }

                        }
                        catch
                        {
                            msi_tots = 0;
                        }

                        if (msi_tots == 1)
                        {
                            y = url_act[ind_msi].LastIndexOf('\\');
                            nom_arch = url_act[ind_msi].ToString();
                            nom_arch = nom_arch.Substring(y + 1, nom_arch.Length - (y + 1));
                            //MessageBox.Show(url+"|"+version+"|"+nom_arch);
                            ver_pro = Convert.ToDecimal(version);
                            ver_act = Convert.ToDecimal(nom_arch.Substring(3, nom_arch.Length - 7));

                            if (ver_act > ver_pro)
                            {
                                DialogResult resultado;
                                resultado = MessageBox.Show("Se Encontró una Nueva Versión del Programa (" + (ver_act) + ").\n\n¿Desea Proceder con la Actualización?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                                if (resultado == DialogResult.Yes)
                                {
                                    System.IO.File.Delete(@"actualizador/instalado.txt");
                                    //Crear archivos nuevos
                                    fichero = System.IO.File.Create(@"actualizador/instalado.txt");
                                    ruta = fichero.Name;
                                    fichero.Close();
                                    z = ruta.LastIndexOf('\\');
                                    nueva_url = ruta.Substring(0, z);
                                    z = nueva_url.LastIndexOf('\\');
                                    url_raiz = nueva_url.Substring(0, z);
                                    nueva_url = url_raiz + @"\temp\update.msi";
                                    if (Directory.Exists(url_raiz + @"\temp") == false)
                                    {
                                        Directory.CreateDirectory(url_raiz + @"\temp");
                                    }

                                    File.Copy(url_act[0].ToString(), nueva_url, true);

                                    StreamWriter wr1 = new StreamWriter(@"actualizador/instalacion_silenciosa.bat");
                                    wr1.WriteLine("@echo off");
                                    wr1.WriteLine("title ACTUALIZADOR DE NOVA GEAR");
                                    wr1.WriteLine("msiexec.exe /i \"" + nueva_url + "\" /q");
                                    wr1.WriteLine("del /q \"" + nueva_url + "\"");
                                    wr1.WriteLine("del /q desinstalado.txt");
                                    wr1.WriteLine("\"instalacion completa\" >instalado.txt");
                                    wr1.Close();

                                    StreamWriter wr3 = new StreamWriter(@"actualizador/desinstalacion_silenciosa.bat");
                                    wr3.WriteLine("@echo off");
                                    wr3.WriteLine("title ACTUALIZADOR DE NOVA GEAR");
                                    wr3.WriteLine("wmic product where name=\"Nova Gear\" call uninstall");
                                    wr3.WriteLine("y");
                                    wr3.WriteLine("del /q instalado.txt");
                                    wr3.WriteLine("\"desinstalacion completa\" >desinstalado.txt");
                                    wr3.Close();

                                    StreamWriter wr2 = new StreamWriter(@"actualizer.bat");
                                    wr2.WriteLine("@echo off");
                                    wr2.WriteLine("C:");
                                    wr2.WriteLine("cd \"" + url_raiz + "\\actualizador\"");
                                    wr2.WriteLine("start actualizabot.exe");
                                    wr2.Close();
                                    System.Diagnostics.Process.Start(@"actualizer.bat");
                                    this.Close();
                                }

                            }
                            //MessageBox.Show(url+"|"+version+"|"+url_act[0]);
                        }
                        else
                        {
                            MessageBox.Show("Verifique el URL de Actualización, no se pudo buscar la nueva version.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puede acceder a la URL de Actualización.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
			    }else{
				    MessageBox.Show("No se pudo verificar la versión del programa. \nLa Aplicación se cerrará","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
				    this.Close();
			    }
			
			}catch(Exception error){
				if(cacha_error==0){
					MessageBox.Show("No se pudo verificar la versión del programa. \nLa Aplicación se cerrará","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Close();
				}
				
				if(cacha_error>0){
					MessageBox.Show("Ocurrió un Error al momento de verificar la version del programa:\n\n"+error,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Close();
				}
			}
		}
		
		public void post_login(int exito){
            MessageBox.Show("pantalla se ocultará");
			if(exito==1){
				this.Hide();
			}
		}
		
		public void verifi_sub(){
			try{
				if(File.Exists(@"sub_config.lz")){
					if(conex.leer_config_sub()[4].Length>0){
						
					}else{
						MessageBox.Show("Ocurrió un Error al momento de comprobar la configuracion de ubicación del programa","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
						this.Close();
					}
					
				}else{
					if(err_cuenta==3){
						System.Diagnostics.Process.Start("close_ng.exe");
					}
					ops_subs.selector_sub sub_sel = new Nova_Gear.ops_subs.selector_sub(0);
					sub_sel.ShowDialog();
					err_cuenta++;
					verifi_sub();
				}
			}catch(Exception x){
				MessageBox.Show("Ocurrió un Error al momento de leer la configuracion de ubicación del programa:\n\n"+x.ToString(),"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
				this.Close();
			}
		}
		
		public void ee(){
			String user,pass,controles_snes,controles_gba,controles_n64;
			user=textBox1.Text.ToUpper();
			pass=textBox2.Text.ToUpper();

            controles_snes = "Controles:\n\n" +
                                        "DIRECCION: Flechas Direccionales\n" +
                                        "START: Enter\n" +
                                        "SELECT: NumPad 0\n" +
                                        "A: NumPad 6 \n" +
                                        "B: NumPad 2 \n" +
                                        "X: NumPad 8 \n" +
                                        "Y: NumPad 4 \n" +
                                        "L: NumPad 7 \n" +
                                        "R: NumPad 9 \n" +
                                        "OPCIONES: Esc \n";

            controles_gba = "Controles:\n\n" +
                                        "DIRECCION: Flechas Direccionales\n" +
                                        "START: Enter\n" +
                                        "SELECT: NumPad 0\n" +
                                        "A: NumPad 6 \n" +
                                        "B: NumPad 2 \n" +
                                        "L: NumPad 7 \n" +
                                        "R: NumPad 9 \n" +
                                        "OPCIONES: Esc \n";

            controles_n64 = "Controles:\n\n" +
                                        "PALANCA ANÁLOGA: Flechas Direccionales\n" +
                                        "START: Enter\n" +
                                        "GATILLO Z: NumPad 0\n" +
                                        "A: NumPad 5 \n" +
                                        "B: NumPad 3 \n" +
                                        "L: NumPad 7 \n" +
                                        "R: NumPad 9 \n" +
                                        "\n" +
                                        "Boton-C Arriba: NumPad 8 \n" +
                                        "Boton-C Abajo: NumPad 2 \n" +
                                        "Boton-C Izquierdo: NumPad 4 \n" +
                                        "Boton-C Derecho: NumPad 6 \n" +
                                        "\n" +
                                        "Pad Digital Arriba: Tecla W \n" +
                                        "Pad Digital Abajo: Tecla Z \n" +
                                        "Pad Digital Izquierdo: Tecla A \n" +
                                        "Pad Digital Derecho: Tecla S \n" +
                                        "Pantalla Completa: Esc \n";
			
			if(user.Equals("SNES")){
				switch(pass){
					case "FZERO":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\f0.smc");                       
						break;
					case "ISSSD":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\ISSSD.zip");
						break;
					case "BOMBERMAN2":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\SB2.smc");
						break;
					case "MARIOBROS":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\SMAS.zip");
						break;
					case "MARIOKART":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\SMK.smc");
						break;
					case "MARIOWORLD":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\SMW.zip");
						break;
					case "PUNCHOUT":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\SPO.zip");
						break;
					case "TOPGEAR":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\TG1.sfc");
						break;
					case "TOPGEAR2000":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\TG2.sfc");
                        break;
					case "YOSHI":
                        MessageBox.Show(controles_snes);
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe", @"temp\EMU\SNES\roms\Yoshi.zip");
						break;
					default:
						MessageBox.Show("CASI DESCUBRES UN GRAN SECRETO!");
						break;
				}
			}
			
			if(user.Equals("GBA")){
				switch(pass){
					case "CASTLEVANIA":
                        MessageBox.Show(controles_gba);
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe", @"temp\EMU\GBA\roms\CCotM.zip");                        
						break;
					case "METROID":
                        MessageBox.Show(controles_gba);
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe", @"temp\EMU\GBA\roms\Met_F.zip");                        
						break;
					case "POKEMONESMERALDA":
                        MessageBox.Show(controles_gba);
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe", @"temp\EMU\GBA\roms\P_E.zip");                       
						break;
					case "POKEMONROJOFUEGO":
                        MessageBox.Show(controles_gba);
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe", @"temp\EMU\GBA\roms\Poke_RF.zip");                       
						break;
					case "MARIOWORLD":
                        MessageBox.Show(controles_gba);
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe", @"temp\EMU\GBA\roms\SMA2.zip");                       
						break;						
					case "ZOIDS":
                        MessageBox.Show(controles_gba);
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe", @"temp\EMU\GBA\roms\Zoids_L.zip");                        
						break;
					default:
						MessageBox.Show("CASI DESCUBRES UN GRAN SECRETO!");
						break;
				}
			}

            if (user.Equals("N64"))
            {
                switch (pass)
                {
                    case "MARIOKART":
                        MessageBox.Show(controles_n64);
                        System.Diagnostics.Process.Start(@"temp\EMU\N64\Project64.exe", @"temp\EMU\N64\ROM\MK64.n64");
                        break;
                  
                    default:
                        MessageBox.Show("CASI DESCUBRES UN GRAN SECRETO!");
                        break;
                }
            }

            if (user.Equals("CHRONO"))
            {
                if (pass.Equals("TRIGGER"))
                {
                    MessageBox.Show("¡¡¡HAS DESCUBIERTO EL SECRETO MÁXIMO!!!");
                    MessageBox.Show("Para Configurar el Control presiona la Tecla ESC\n¡Disfruta la Joya de SNES!");
                    System.Diagnostics.Process.Start(@"temp\CT.exe");
                    this.Dispose();                    
                }
            }

            if (user.Equals("SUPERNOVA"))
            {
                if (pass.Equals("121221"))
                {
                    Supernova.Login super_log = new Supernova.Login();
                    super_log.Show();
                    this.Hide();
                    super_log.Focus();
                }
            }
		}
		
		void PantallaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			panel1.Location = new System.Drawing.Point(((Screen.PrimaryScreen.WorkingArea.Width - this.panel1.Width) / 2),(((Screen.PrimaryScreen.WorkingArea.Height - this.panel1.Height) / 2)-50));
			panel1.Visible = true;
			
			//button3.Location = new System.Drawing.Point((Screen.PrimaryScreen.WorkingArea.Width - (this.button3.Width +12)),12);
			button3.Visible = true;

            //button2.Location = new System.Drawing.Point((Screen.PrimaryScreen.WorkingArea.Width - (this.button2.Width + 12)), ((Screen.PrimaryScreen.WorkingArea.Height) + (this.button2.Height -150)));

			pictureBox2.Location = new System.Drawing.Point(((Screen.PrimaryScreen.WorkingArea.Width - this.panel1.Width) / 2),(((Screen.PrimaryScreen.WorkingArea.Height - this.pictureBox2.Height) / 2)+100));
			pictureBox3.Location = new System.Drawing.Point(((Screen.PrimaryScreen.WorkingArea.Width - this.pictureBox3.Width) / 2),(((Screen.PrimaryScreen.WorkingArea.Height - this.pictureBox3.Height / 2)-150)));
			
			//label1.Location =new System.Drawing.Point(((Screen.PrimaryScreen.WorkingArea.Width - this.label1.Width) / 2),(((Screen.PrimaryScreen.WorkingArea.Height - this.label1.Height) / 2)+300));
			//button2.Location = new System.Drawing.Point(((Screen.PrimaryScreen.WorkingArea.Width - this.panel1.Width) / 2),(((Screen.PrimaryScreen.WorkingArea.Height - this.button2.Height) / 2)+304));
			textBox2.ReadOnly=false;
			actualizaciones();
			verifi_sub();
			/*
			try{
				var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://animelz.blogspot.com/");
				var response = myHttpWebRequest.GetResponse();

				string[] dt = response.Headers.GetValues("Date");
				DateTime t = Convert.ToDateTime(dt[0]);
				linkLabel1.Text=t.ToString();
				fecha_servidor=linkLabel1.Text.Substring(0,10);
			}catch(Exception lz){
				//MessageBox.Show("No se pudo conectar con el Servidor de Fecha.\nFavor de revisar su conexión a Internet","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
				//this.Close();
			}
            */
		}
		
		void Button2Click(object sender, EventArgs e)
		{
            Menu_nova config = new Menu_nova(0);
            timer1.Start();
            config.Show();
            config.Focus();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			this.Dispose();
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
				textBox2.PasswordChar='\0';
				textBox2.Text = "Contraseña...";
			}else{
				this.textBox2.ForeColor = System.Drawing.Color.White;
			}
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
				ingreso();
			}
		}
		
		void Button2Enter(object sender, EventArgs e)
		{
			this.button2.BackColor= System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49)))));
		}
		
		void Button2Leave(object sender, EventArgs e)
		{
			this.button2.BackColor = System.Drawing.Color.Transparent;
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(estado_pantalla==1){
                this.Hide();
            }
		}
		
		void Timer2Tick(object sender, EventArgs e)
		{
			
			if(cuenta==0){ label1.Text="Cargando Interfaz Gráfica.";	}
			
			if(cuenta==1){ label1.Text="Cargando Interfaz Gráfica..";	}
			
			if(cuenta==2){ label1.Text="Cargando Interfaz Gráfica...";	}
			
			if(cuenta==3){ label1.Text="Cargando Base de Datos.";	}
			
			if(cuenta==4){ label1.Text="Cargando Base de Datos..";	}
			
			if(cuenta==5){ label1.Text="Cargando Base de Datos...";	}
			
			if(cuenta==6){ label1.Text="Cargando Datos de Usuario.";	}
			
			if(cuenta==7){ label1.Text="Cargando Datos de Usuario...";	}
			
			if(cuenta==8){ label1.Text="Cargando Configuración Inicial.";	}
			
			if(cuenta==9){ label1.Text="Cargando Configuración Inicial...";	}
			
			if(cuenta==10){ 
				cuenta=0;
				timer2.Enabled=false;	
			}
				
			label1.Refresh();
			cuenta++;
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			About abo = new About();
			abo.ShowDialog();
		}
		
		void PictureBox1Click(object sender, EventArgs e)
		{
           //Mod40.Menu40 men40 = new Mod40.Menu40();
           //men40.Show();
           //Mod40.Carga_excel40 carga40 = new Mod40.Carga_excel40();
		   //carga40.Show();

          /*  IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            MessageBox.Show("Tú IP Local Es: " + localIP);*/
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
		
		void Button4Click(object sender, EventArgs e)
		{
			ingreso();
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			Registros.Reg_Usuarios_2 reg2 = new Nova_Gear.Registros.Reg_Usuarios_2(1,0);
			reg2.Show();
			reg2.Focus();
		}
		
		void PantallaFormClosed(object sender, FormClosedEventArgs e)
		{
			//try{
			System.Diagnostics.Process.Start("close_ng.exe");
			//}catch{}
		}
		
		void PantallaFormClosing(object sender, FormClosingEventArgs e)
		{
			//try{
			System.Diagnostics.Process.Start("close_ng.exe");
			//}catch{}
		}
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			//Navegador_web nav  = new Navegador_web();
			//nav.Show();
		}

        private void terminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "lanzezager";
            textBox2.Text = "";
            textBox2.Text = "Rojinegro 100% Novatlas";
            textBox2.PasswordChar='•';
            textBox2.UseSystemPasswordChar = true;
            textBox2.Refresh();
            textBox2.Focus();
        }
	}
}

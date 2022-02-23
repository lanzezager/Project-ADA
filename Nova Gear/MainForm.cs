/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 30/04/2015
 * Hora: 11:27 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace Nova_Gear
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			InitializeComponent();
			
			//Conexion conect = new Conexion();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	
	   //String con = @"server=localhost; userid=lanzezager; password=mario; database=base_principal";
	   String sql,tipo_cred,nom_save,credito,per2,reg_pat,f1,fecha,ruta;
	   int tot_rows=0,i=0,j=0,k=0,tipo_carga=0,tot_errs=0,candado=0,rango=0;
	   public int activo=0; 
	   string[] errores_sis, datos_usuario;
	   public static string[] datos_user_static;
	   public string[] datos_user;
	   public int[] asig,no_asig;
	   public static int cierra=0;
	   public static int reset = 0;
       public static int cartera_estrados = 0;
       public static string nombre_periodo = "";
       public static string reg_pat_sindo = "";

	   DialogResult respuesta;
	   Conexion conex = new Conexion();
       Conexion conex1 = new Conexion();
	   
	   DataTable consultamysql;
	   DataTable data3 = new DataTable();
       DataTable consultaini;
       

	   public void abrir_lector(){
	   	  Lector_Fac form1 = new Lector_Fac();
		  form1.MdiParent=this;
          form1.Show();
          form1.Focus();
	   }
	   
	   public void abrir_siscobnator(){
	   		Siscobnator form2 = new Siscobnator();
			form2.MdiParent=this;
            form2.Show();
            form2.Focus();
	   }
	   
	   public void abrir_registro_users(){
	   	    Registros.Usuarios form3 = new Registros.Usuarios(1,0);
			form3.MdiParent=this;
            form3.Show();
            form3.Focus();
	   }
	   
	   public void abrir_lista_users(){
	   		Registros.Busca_usuario list_user = new Registros.Busca_usuario(datos_usuario);
	   		//list_user.MdiParent=this;
	   		list_user.Show();
	   		list_user.Focus();
	   }
	   
	   public void abrir_modificar_users(int tipo, int id){
	   		Registros.Usuarios form3_1 = new Registros.Usuarios(tipo,id);
			form3_1.MdiParent=this;
            form3_1.Show();
            form3_1.Focus();
	   }
	   
	   public void abrir_fecha_not(){
	   		Fecha_Not.Fecha_Not_Menu fech_menu = new Fecha_Not.Fecha_Not_Menu();
	   		fech_menu.MdiParent=this;
	   		fech_menu.Show();
	   		fech_menu.Focus();
	   }
	   
	   public void abrir_incidencias(){
	   		Jarviscob jar = new Jarviscob();
	   		//jar.MdiParent=this;
	   		jar.Show();
	   		jar.Focus();
	   		
	   		//Menu_cores mc= new Menu_cores();
	   		//mc.Show();
	   		//mc.Focus();
	   }
	   
	   public void abrir_depu(){
			
				Depuracion.Depuración2 depu = new Depuracion.Depuración2();
				//depu.MdiParent=this;
				depu.Show();
				depu.Focus();
				timer2.Start();
	   }
	   
	   public void abrir_consulta_pat(){
	   		Consulta_patron cons_pat = new Consulta_patron();
	   		//ns_pat.MdiParent=this;
	   		cons_pat.Show();
	   		cons_pat.Focus();
	   }
	   
	   public void abrir_generar_reportes(){
	   		Generador_reportes gen_repor = new Generador_reportes();
	   		gen_repor.MdiParent=this;
	   		gen_repor.Show();
	   		gen_repor.Focus();	   		
	   }
	   
	   public void log_user(string[] datos_user){
	   	datos_usuario = new string[datos_user.Length];
	    this.datos_usuario = datos_user;
	    
		datos_user_static = new string[datos_user.Length];
		datos_user_static = datos_user;
		
		//MessageBox.Show(datos_user_static[0]);
	   }
	   
	   public void form_activo(int acti, int[] asign,int[] noasig){
	   	this.asig = new int[asign.Length];
	   	this.no_asig = new int[noasig.Length];
	 
	    this.asig = asign;
	   	this.activo=acti;
	   	this.no_asig=noasig;
	   	
	   }
	   
	   public void ocultar_perfil(){
	   		if(panel1.Visible){	
			panel1.Visible= false;
			panel3.Visible=false;
			this.button1.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_right;
			button1.Location = new System.Drawing.Point(5,5);
			
			}else{	
			panel1.Visible= true;
			this.button1.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_left;
			button1.Location = new System.Drawing.Point(232,5);
			}
	   }
	   
	   public void ocultar_sesion(){
	   		if(panel3.Visible){	
			panel3.Visible= false;
			this.button4.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_right;
			
			}else{	
			panel3.Visible= true;
			this.button4.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_left;

			}
	   }
	   
	   public void cargar_permisos(){
	   	int lv_permiso=100;
	   	rango = Convert.ToInt32(datos_usuario[2]);
	   	
	   	if(rango<=7){
	   		lv_permiso=rango;
	   	}
	   	
	   	if(rango>=8 && rango<=10){
	   		lv_permiso=4;
	   	}
	   	
	   	if(rango==11){
	   		lv_permiso=rango;
	   	}
	   	
	   	if(rango>11 && rango<21){
	   		lv_permiso=12;
	   	}
	   	
	   	if(rango==21){
	   		lv_permiso=rango;
	   	}
	   	
	   	if(rango>21 && rango<=30){
	   		lv_permiso=22;
	   	}
	   	
	   	if(rango>30){
	   		lv_permiso=4;
	   	}

        
        	   	
	   	switch(lv_permiso){
				case 0: //Procesos Notificacion
	   					button5.Enabled=true;
						button21.Enabled=true;
						button13.Enabled=true;
						button20.Enabled=true;
						button15.Enabled=true;
						button8.Enabled=true;
						button40.Enabled=true;
						button34.Enabled=true;
						button30.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
						button25.Enabled=true;
						button41.Enabled=true;
						button43.Enabled=true;
						button44.Enabled=true;
                        button45.Enabled=true;
                        button47.Enabled = true;

						//Automatizacion
						button6.Enabled=true;
						button12.Enabled=true;
						button16.Enabled=true;
						button35.Enabled=true;
						button36.Enabled=true;
						button18.Enabled=true;
						
						//Otros
						button31.Enabled=true;
						button26.Enabled=true;
						button32.Enabled=true;
                        button48.Enabled = true;
                        button39.Enabled = true;
                        button39.Visible = true;
						
						//Opciones
						button17.Enabled=true;
						button23.Enabled=true;
						
                        
			    break;
      			    
			    case 1: //Procesos Notificacion
			    		button5.Enabled=true;
						button21.Enabled=true;
						button13.Enabled=true;
						button20.Enabled=true;
						button15.Enabled=true;
						button8.Enabled=true;
						button40.Enabled=true;
						button34.Enabled=true;
						button30.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
						button25.Enabled=true;
						button41.Enabled=true;
						button43.Enabled=true;
						button44.Enabled=true;
                        button45.Enabled = true;
                        button47.Enabled = true;
						
						//Automatizacion
						button6.Enabled=true;
						button12.Enabled=true;
						button16.Enabled=true;
						button35.Enabled=true;
						button36.Enabled=true;
						
						
						//Otros
						button31.Enabled=true;
                        button48.Enabled = true;
						
						//Opciones						
						button17.Enabled=true;
						button23.Enabled=true;
                        
			    break;

                case 2: //Procesos Notificacion
			    		button5.Enabled=true;
						button21.Enabled=true;
						button13.Enabled=true;
						button20.Enabled=true;
						button15.Enabled=true;
						button8.Enabled=true;
						button40.Enabled=true;
						button34.Enabled=true;
						button30.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
						button25.Enabled=true;
						button41.Enabled=true;
						button43.Enabled=true;
						button44.Enabled=true;
                        button45.Enabled = true;
						
						//Automatizacion
						button6.Enabled=true;
						button12.Enabled=true;
						button16.Enabled=true;
						button35.Enabled=true;
						button36.Enabled=true;
						
						//Otros
						button31.Enabled=true;
						
						//Opciones						
						button23.Enabled=true;
			    break;

                case 3: //Procesos Notificacion
			    		button8.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
						button44.Enabled=true;
                        button25.Enabled=true;
						//Automatizacion
												
						//Otros
												
						//Opciones						
						
			   break;

                case 4: //Procesos Notificacion
			    		button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones
			   break;	
			   
			   case 5:  //Procesos Notificacion -- Estrados
			    		button40.Enabled=true;
						button34.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones						
						
			   break;
			   
			   case 6:  //Procesos Notificacion -- Oficios
			    		button34.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones						
						
			   break;
			   
			   case 7:  //Procesos Notificacion -- Auxiliar Limitado
			    		button15.Enabled=true;
						button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones						
						
			   break;

			   
			   case 11: //Procesos Notificacion
			    		button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones
						button26.Enabled = true;
						
					    radioButton3.Checked=true;
			   break;

               case 12: //Procesos Notificacion
			    		button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones
						button26.Enabled = true;
						
					    radioButton3.Checked=true;
               break;	
			
               case 21: //Procesos Notificacion
			    		button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
						button16.Enabled=true;
						button18.Enabled=true;						
						//Otros
												
						//Opciones
						button32.Enabled = true;
						
					    radioButton3.Checked=true;
               break;   
               
			   case 22: //Procesos Notificacion
			    	    button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
						button16.Enabled=true;
						button18.Enabled=true;						
						//Otros
												
						//Opciones
						button32.Enabled = true;
						
					    radioButton3.Checked=true;
               break;
               
                case 27: //Procesos Notificacion
			    	    button27.Enabled=true;
						button14.Enabled=true;
												
						//Automatizacion
												
						//Otros
												
						//Opciones
						button32.Enabled = true;
						
					    radioButton3.Checked=true;
               break;
			   
			}
	   }
	   
	   public void abrir_autocob(){
	   	//Autocob auto = new Autocob();
	   	//auto.MdiParent=this;
	   	//auto.Show();
	   	//auto.Focus();
	   	
		//Envío multiple de cores inc31 cm12
	   	Envio_31 env_31 = new Envio_31();
		env_31.Show();
		env_31.Focus();
	   }
	   
	   public void cargar_consulta_controlador(){
	   	DataTable tabla = new DataTable();
	   	
	   	conex.conectar("base_principal");
        dataGridView2.DataSource = conex.consultar("SELECT nombre_periodo, COUNT(id) AS Total FROM datos_factura WHERE controlador=\"" + datos_usuario[9] + " " + datos_usuario[4] + "\" AND status=\"EN TRAMITE\" AND nn = \"-\" AND observaciones NOT LIKE \"PAGADO%\" GROUP BY nombre_periodo ORDER BY total desc");
	    //dataGridView2.DataSource=conex.consultar("SELECT nombre_periodo, COUNT(id) AS Total FROM datos_factura WHERE controlador=\"Palacios Alonso Francisco Javier\" AND status=\"EN TRAMITE\" AND nn<>\"NN\" AND observaciones NOT LIKE \"PAGADO%\" GROUP BY nombre_periodo ORDER BY total desc");
	   /*DataView vista = new DataView(tabla);
	   	vista.RowFilter="Total>100";
	   	vista.Sort="Total DESC";
	   	dataGridView2.DataSource=vista;*/
	   	//dataGridView2.DataSource=conex.consultar("SELECT nombre_periodo, COUNT(id) AS Total FROM datos_factura WHERE controlador=\"Medina Saldaña Maria Eugenia\" AND status=\"EN TRAMITE\" AND nn<>\"NN\" AND observaciones NOT LIKE \"PAGADO%\" GROUP BY nombre_periodo ORDER BY total desc");
	   	dataGridView2.Columns[0].HeaderText="Nombre Periodo";
	   	dataGridView2.Columns[0].MinimumWidth=250;
	   	dataGridView2.Columns[1].HeaderText="Casos";
	   	dataGridView2.Columns[1].Width=57;
	   	conex.cerrar();
	   }

       void MainFormLoad(object sender, EventArgs e)
       {
           String window_name = this.Text;
           window_name = window_name.Replace("Gear Prime", "Nova Gear");
           this.Text = window_name;
           this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
			//Interface_Para_Cerrar post_logeo =  this.Owner as Interface_Para_Cerrar;
			//if (post_logeo != null)
			//{
			// MessageBox.Show("se manda a ocultar a pantalla");
			//post_logeo.post_login(1);
			//}

			toolStripStatusLabel1.Size = new System.Drawing.Size((Screen.PrimaryScreen.WorkingArea.Width - 195), 17);
           //toolStripStatusLabel2.Visible = true;

           //panel2.Location = new System.Drawing.Point((Screen.PrimaryScreen.WorkingArea.Width - 339),4);
           //panel2.Visible= true;
           //MessageBox.Show(""+this.Height);
           button4.Location = new System.Drawing.Point(button4.Location.X,(this.Height - button4.Height)-80);
           panel3.Location = new System.Drawing.Point((Screen.PrimaryScreen.WorkingArea.Width - 278), 11);

          /* button7.Visible = false;
           label9.Visible = false;
           label8.Visible = false;

           label7.Visible = false;
           button6.Visible = false;
           button12.Visible = false;

           button5.Visible = false;
           button8.Visible = false;
           button13.Visible = false;
           button14.Visible = false;*/


           toolTip1.SetToolTip(button1, "Ocultar/Mostrar Opciones");
           toolTip1.SetToolTip(button5, "Lectura de\nFacturas");
           toolTip1.SetToolTip(button8, "Actualización\nFecha de Notificacion/PAGADO/NN");
           toolTip1.SetToolTip(button7, "Agregar\nUsuarios");
           toolTip1.SetToolTip(button6, "Captura de Fecha de\nNotificación");
           toolTip1.SetToolTip(button12, "Captura de\nCambio de Incidencia");
           toolTip1.SetToolTip(button13, "Depuración");
           toolTip1.SetToolTip(button14, "Consulta de Créditos");
           toolTip1.SetToolTip(button15, "Generar Reportes");
           toolTip1.SetToolTip(button16, "Captura de\nAjuste Manual");
           toolTip1.SetToolTip(button17, "Lista de\nUsuarios");
           toolTip1.SetToolTip(button18, "Captura de Cambio a Incidencia 02 \n(Necesario Folio SICOFI)");
           toolTip1.SetToolTip(button19, "Captura de Estrados");
           toolTip1.SetToolTip(button20, "Sectorización");
           toolTip1.SetToolTip(button21, "Capturar Fechas\nProductos");
           toolTip1.SetToolTip(button22, "Generar Reporte\nSituación Productos");
           toolTip1.SetToolTip(button23, "Historial de \nEventos");
           toolTip1.SetToolTip(button25, "Actualización Individual de\nCréditos");
           toolTip1.SetToolTip(button26, "Sección\nModalidad 40");
           toolTip1.SetToolTip(button27, "Estado de la Notificación");
           toolTip1.SetToolTip(button30, "Carga Manual Individual de Créditos");
           toolTip1.SetToolTip(button31, "R.A.L.E.");
           toolTip1.SetToolTip(button32, "Sección\nInventario");
           toolTip1.SetToolTip(button33, "Productividad\nde Notificadores");
           toolTip1.SetToolTip(button34, "Sección\nOficios");
		   toolTip1.SetToolTip(button39, "Generador de Archivo de Correspondencia\npara Actas y Citatorios");
		   toolTip1.SetToolTip(button40, "Sección\nEstrados");

           label1.Text = datos_usuario[0];
           label2.Text = datos_usuario[4] + "\n" + datos_usuario[9];
           label3.Text = datos_usuario[6];
           label4.Text = datos_usuario[5];

           ruta = conex.leer_config();
           ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
           ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";

           pictureBox1.ImageLocation = ruta + datos_usuario[3];
           pictureBox1.Visible = true;

           //conex.guardar_evento("Si Chacho la funcion1");
           /*Login logie = new Login();
           logie.MdiParent=this;
           logie.Show();*/

           cargar_permisos();

           datos_user = new string[datos_usuario.Length];
           datos_user = datos_usuario;
           
           if(datos_usuario[5].Equals("Controlador")){
           	cargar_consulta_controlador();
           	panel2.Enabled=true;
           	panel2.Visible=true;
           	panel5.Visible=true;
           }else{
           	panel2.Enabled=false;
           	panel2.Visible=false;
           	panel5.Visible=false;
           }

           int nivel = Convert.ToInt32(datos_usuario[2]);

           if (nivel == 1)
           {
               conex1.conectar("base_principal");
               consultaini = conex1.consultar("SELECT COUNT(id) FROM datos_factura");

               if (Convert.ToInt32(consultaini.Rows[0][0].ToString()) == 0)
               {
                   DialogResult resul = MessageBox.Show("¡La base de datos se encuentra vacía!\nSe recomienda que efectúe la inicialización automática.\n\n¿Desea Proceder con la inicialización?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                   if (resul == DialogResult.Yes)
                   {
                       Inicializacion_nova ini_nova = new Inicializacion_nova();
                       this.WindowState = FormWindowState.Minimized; 
                       ini_nova.Show();
                       //MessageBox.Show("" +this.WindowState.ToString());
                       
                       ini_nova.Focus();
                   }
               }
           }
       }
	   
		void LectorDeFacturasToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_lector();
		}
		
		void AutomatizadorToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_siscobnator();
		}
		
		void UsuariosToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_registro_users();		
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			toolStripStatusLabel2.Text = System.DateTime.Now.ToString();
			statusStrip1.Refresh();
		}	
		
		void SalirToolStripMenuItemClick(object sender, EventArgs e)
		{
            respuesta = MessageBox.Show("Está a punto de salir de Gear Prime\n¿Desea Continuar?", "ATENCIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (respuesta == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("close_ng.exe");
            }
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			ocultar_perfil();
		}
		
		void FocoToolStripMenuItemClick(object sender, EventArgs e)
		{
			Siscobnator sisi = new Siscobnator();
			sisi.Focus();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			/*respuesta = MessageBox.Show("Está a punto de salir de Nova Gear\n¿Desea Continuar?" ,"ATENCIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
			
			if(respuesta == DialogResult.Yes){
				System.Diagnostics.Process.Start("close_ng.exe");
			}*/
			Menu_nova config = new Menu_nova(1);
            config.Show();
            config.Focus();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			respuesta = MessageBox.Show("Está apunto de cerrar su sesión de Gear Prime\n¿Desea Continuar?" ,"ATENCIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
			
			if(respuesta == DialogResult.Yes){
				//Pantalla pant = new Pantalla();
				//pant.Show();
				this.Dispose();
				Application.Exit();
				System.Diagnostics.Process.Start("restart_ng.exe");
			}
		}
		
		void OcultarperfilToolStripMenuItemClick(object sender, EventArgs e)
		{
			ocultar_perfil();
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			ocultar_sesion();
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			abrir_lector();
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			abrir_siscobnator();
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			abrir_registro_users();
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			//abrir_fecha_not();
			Fecha_Not.Fecha_Not_Cartera carte = new Fecha_Not.Fecha_Not_Cartera();
	   		//carte.MdiParent=this;
	   		carte.Show();
	   		//panel4.Visible=false;
		}
		
		void FechaNotificacionToolStripMenuItemClick(object sender, EventArgs e)
		{
			//abrir_fecha_not();
			/*Fecha_Not.Fecha_Not_Cartera carte = new Fecha_Not.Fecha_Not_Cartera();
	   		carte.MdiParent=this;
	   		carte.Show();
	   		panel4.Visible=false;*/

            //Depuracion.Visor_Reporte repor = new Depuracion.Visor_Reporte();
            //repor.Show();
		}
		
		void Button8MouseEnter(object sender, EventArgs e)
		{
			/*if(panel4.Visible==true){
				panel4.Visible=false;
			}else{
				panel4.Visible=true;
			}*/
			
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			Fecha_Not.Fecha_Not_Cartera carte = new Fecha_Not.Fecha_Not_Cartera();
	   		carte.MdiParent=this;
	   		carte.Show();
	   		panel4.Visible=false;
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			abrir_incidencias();
		}
		
		void IncidenciasToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_incidencias();
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			abrir_depu();
		}
		
		void DepuracionToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_depu();
		}
		
		void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			/*respuesta = MessageBox.Show("Está a punto de salir de Nova Gear\n¿Desea Continuar?" ,"ATENCIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
			
			if(respuesta == DialogResult.Yes){
				System.Diagnostics.Process.Start("close_ng.exe");
			}else{
				break;
			}*/
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(cierra==0){
				respuesta = MessageBox.Show("Está a punto de salir de Nova Gear\n¿Desea Continuar?" ,"ATENCIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
				
				if(respuesta ==DialogResult.Yes){
					System.Diagnostics.Process.Start("close_ng.exe");
				}else{
					e.Cancel=true;
				}
			}
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			abrir_consulta_pat();
		}
		
		void Button15Click(object sender, EventArgs e)
		{
			abrir_generar_reportes();
		}
		
		void ReporteToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_generar_reportes();
		}
		
		void ConsultaToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_consulta_pat();
		}
		
		void Button16Click(object sender, EventArgs e)
		{
			abrir_autocob();
		}
		
		void AutocobToolStripMenuItemClick(object sender, EventArgs e)
		{
			abrir_autocob();
		}
		
		void Button17Click(object sender, EventArgs e)
		{
			abrir_lista_users();
		}

        private void label5_Click(object sender, EventArgs e)
        {
            //Sicofitron sicof = new Sicofitron();
            //sicof.Show();

            //Depuracion.Visor_Reporte viso = new Depuracion.Visor_Reporte();
            //viso.Show();
        }
		
		void Panel1Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void Button18Click(object sender, EventArgs e)
		{
			/*Sicofitron sicof = new Sicofitron();
	   		sicof.MdiParent=this;
	   		sicof.Show();
	   		sicof.Focus();*/
			
			SiscobEva sis_eva = new SiscobEva();
			sis_eva.Show();
			sis_eva.Focus();
		}
		
		void Button19Click(object sender, EventArgs e)
		{
			Depuracion.Depu_manu form_nn = new Depuracion.Depu_manu("nada",2);
	   		//form_nn.MdiParent=this;
	   		form_nn.Show();
	   		form_nn.Focus();
		}
		
		void Button20Click(object sender, EventArgs e)
		{
			Busca_nn sectorizar = new Busca_nn();
			//sectorizar.MdiParent=this;
			sectorizar.Show();
			sectorizar.Focus();
			//conex.guardar_evento("Inicio Sectorización");
		}
		
		void Button21Click(object sender, EventArgs e)
		{
			Fechas_periodos fechs = new Fechas_periodos();
			fechs.MdiParent=this;
			fechs.Show();
			fechs.Focus();
		}
		
		void Button22Click(object sender, EventArgs e)
		{
			DialogResult resus = MessageBox.Show("La Elaboración del reporte tomará varios minutos.\n\n Puede que su equipo se ralentice durante el proceso.\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			if (resus == DialogResult.Yes){
				Visor_reporte_mensual repor = new Visor_reporte_mensual();
				//repor.MdiParent = this;
	            repor.Show();
	            repor.Focus();
			}
		}
		
		void Button23Click(object sender, EventArgs e)
		{
			Log historial = new Log(" ");
			historial.Show();
			historial.Focus();
		}
		
		void Button24Click(object sender, EventArgs e)
		{
			About abo = new About();
			abo.ShowDialog();			
		}
		
		void Button25Click(object sender, EventArgs e)
		{
			Universal.Insertar uni_ins= new Universal.Insertar();
            uni_ins.modificacion();
			uni_ins.Show();
            uni_ins.Focus();
		}

        private void button26_Click(object sender, EventArgs e)
        {
            Mod40.Menu40 m40 = new Mod40.Menu40();
            m40.Show();
            m40.Focus();

        }
		
		void Button27Click(object sender, EventArgs e)
		{
			Supervision.Estado_noti estado_not = new Supervision.Estado_noti();
			estado_not.Show();
			estado_not.Focus();
		}
		
		void DataGridView2CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void Button28Click(object sender, EventArgs e)
		{
			try{
				Supervision.Estado_noti estado_not1 = new Supervision.Estado_noti();
				estado_not1.recibe(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].FormattedValue.ToString());
				estado_not1.Show();
				estado_not1.Focus();
			}catch(Exception df){
				
			}
		}
		
		void StatusStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			
		}
		
		void Button29Click(object sender, EventArgs e)
		{
			if(panel2.Location.Y==(this.Height-92)){
				
				panel5.Location = new System.Drawing.Point(panel5.Location.X,this.Height-236);
				panel2.Location = new System.Drawing.Point(panel2.Location.X,this.Height-238);
				this.button29.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_down;
				button28.Enabled=true;
			}else{
				panel5.Location = new System.Drawing.Point(panel5.Location.X,this.Height-88);
				panel2.Location = new System.Drawing.Point(panel2.Location.X,this.Height-92);
				this.button29.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_up;
				button28.Enabled=false;
				
			}
		}
		
		void Button30Click(object sender, EventArgs e)
		{
            Universal.Insertar uni_ins1 = new Universal.Insertar();
            uni_ins1.carga();
            uni_ins1.Show();
            uni_ins1.Focus();
		}
		
		void Button31Click(object sender, EventArgs e)
		{
			Depuracion.Depu_rale depule = new Nova_Gear.Depuracion.Depu_rale(1);
			depule.Show();
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			if (reset==1)
			{
				reset = 0;

				Depuracion.Depuración2 depu = new Depuracion.Depuración2();
				//depu.MdiParent=this;
				depu.Show();
				depu.Focus();
			}
		}

		void Button32Click(object sender, EventArgs e)
		{
			Inventario.Menu_invs invent = new Inventario.Menu_invs();
			invent.Show();
		}
		
		void Button33Click(object sender, EventArgs e)
		{
			Supervision.Productividad_noti produ = new Nova_Gear.Supervision.Productividad_noti();
			produ.Show();
		}
		
		void Button34Click(object sender, EventArgs e)
		{
			Oficios.Oficios_captura oficios_menu = new Nova_Gear.Oficios.Oficios_captura();
			oficios_menu.Show();
			
		}
		
		void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
		{
			
		}
		
		void Button35Click(object sender, EventArgs e)
		{
			Automatizacion.Sindo_nator sindo_n = new Nova_Gear.Automatizacion.Sindo_nator();
			sindo_n.Show();
		}
		
		void Button36Click(object sender, EventArgs e)
		{
			Automatizacion.Sectorizador secto = new Nova_Gear.Automatizacion.Sectorizador();
			secto.Show();
		}
		
		void Button37Click(object sender, EventArgs e)
		{
			Actualizador rale_vig = new Actualizador();
			rale_vig.Show();
		}
		
		void Button38Click(object sender, EventArgs e)
		{
			Generador_factura gen_fact = new Generador_factura();
			gen_fact.Show();
		}
		
		void Button39Click(object sender, EventArgs e)
		{
			//Correspondencia_acta_citatorio actas = new Correspondencia_acta_citatorio();
			//actas.Show();
            Inicializacion_nova ini_nova = new Inicializacion_nova();
            ini_nova.Show();
		}
		
		void Button40Click(object sender, EventArgs e)
		{
			Estrados.Menu_estrados menu_e = new Nova_Gear.Estrados.Menu_estrados();
			menu_e.Show();
		}
		
		void Button42Click(object sender, EventArgs e)
		{
			if(panel3.Visible){	
			panel3.Visible= false;
			//this.button4.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_right;
			
			}else{	
			panel3.Visible= true;
			//this.button4.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_left;

			}
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton1.Checked==true){
				panel4.Visible=true;				
				panel6.Visible=false;
				panel7.Visible=false;
				
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton2.Checked==true){
				panel4.Visible=false;				
				panel6.Visible=true;
				panel7.Visible=false;
			}
		}
		
		void RadioButton3CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton3.Checked==true){
				panel4.Visible=false;				
				panel6.Visible=false;
				panel7.Visible=true;
			}
		}
		
		void Button41Click(object sender, EventArgs e)
		{
			Registros.Configura_ctrldr asigna_notis = new Nova_Gear.Registros.Configura_ctrldr();
			asigna_notis.Show();
			asigna_notis.Focus();
		}
		void Button43Click(object sender, EventArgs e)
		{
			Universal.Entrega_especial ee = new Universal.Entrega_especial();
			ee.Show();
			ee.Focus();
		}
		
		void Button44Click(object sender, EventArgs e)
		{
			Sepomex.Menu_sepomex menu_sepo = new Nova_Gear.Sepomex.Menu_sepomex();
			menu_sepo.Show();
			menu_sepo.Focus();
		}

        private void button45_Click(object sender, EventArgs e)
        {
            Estrados.Procesar_PDFs proce_pdf = new Estrados.Procesar_PDFs();
            proce_pdf.Show();
            proce_pdf.Focus();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            //Terminos terms = new Terminos();
            //terms.ShowDialog();
            FileStream fichero;
            String ruta = "";
            if (File.Exists(@"temp/cookie.lz") == true)
            {
                System.IO.File.Delete(@"temp/cookie.lz");
            }
            fichero = System.IO.File.Create(@"temp/cookie.lz");
            ruta = fichero.Name;
            ruta = ruta.Substring(0, ruta.Length - 9);
            ruta = ruta + "Manual_Nova.pdf#page=1";
            Navegador_web nav = new Navegador_web("file:///" + ruta);
            nav.Show();
        }

        private void button47_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Es recomendable que realice un respaldo antes de efectuar cualquier modificación en la base de datos\n\n¿Desea Proceder con el Respaldo?", "AVISO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
             if (res == DialogResult.Yes)
             {
                 Config_backup respaldo = new Config_backup();
                 respaldo.Show();
                 respaldo.Focus();
             }
             else
             {
                 if (res == DialogResult.Cancel)
                 {
                     Universal.Edicion_en_Volumen edit_vol = new Universal.Edicion_en_Volumen();
                     edit_vol.Show();
                     edit_vol.Focus();
                 }
             }
        }

        private void button48_Click(object sender, EventArgs e)
        {
            Aclaraciones.Menu_Aclaraciones menu_aclara = new Aclaraciones.Menu_Aclaraciones();
            menu_aclara.Show();
            menu_aclara.Focus();
        }
}
}
/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 23/03/2017
 * Hora: 10:39 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.IO;
using Office = Microsoft.Office.Interop.Word;

namespace Nova_Gear.Automatizacion
{
	/// <summary>
	/// Description of Sindo_nator.
	/// </summary>
	public partial class Sindo_nator : Form
	{
		public Sindo_nator()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();

		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		DataTable tablarow = new DataTable();
		DataTable tablarow_consulta = new DataTable();
		DataTable tablasindo = new DataTable();
        DataTable excel = new DataTable();
		
		//Declaracion del Delegado y del Hilo para ejecutar un subproceso
		private Thread hilosecundario = null;
		
		int i=0,z=0,tot_ing=0,tot_act=0,tot_errs=0;
		String archivo,ext,ruta_sindo,per; 
		
		public void llenar_Cb2_todos(){
			conex.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			
            do{
				comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView2.RowCount);
			i=0;
            conex.cerrar();
		}

        public void llenar_Cb2_todos_especiales()
        {
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT(periodo_factura) FROM datos_factura WHERE periodo_factura <> \"-\" ORDER BY periodo_factura");
            
            do
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView2.RowCount);            
            i = 0;
           conex.cerrar();
        }
		
		public void consultar(){
			
			if(comboBox2.SelectedIndex>-1){

                if(radioButton1.Checked==true)
                {
                    per = "nombre_periodo=\"" + comboBox2.SelectedItem.ToString() + "\"";
                }

                if (radioButton2.Checked == true)
                {
                    per = "periodo_factura=\"" + comboBox2.SelectedItem.ToString() + "\""; 
                }

				conex.conectar("base_principal");
				dataGridView3.DataSource=conex.consultar("SELECT registro_patronal2, credito_cuotas,credito_multa FROM datos_factura WHERE "+per);
				label5.Text="Registros: "+dataGridView3.RowCount;
				label5.Refresh();
				
				//estilo datagrid
				dataGridView3.Columns[0].HeaderText="REGISTRO PATRONAL";
				dataGridView3.Columns[1].HeaderText="CREDITO CUOTA";
				dataGridView3.Columns[2].HeaderText="CREDITO MULTA";
				
				
				if(dataGridView3.RowCount>0){
					button4.Enabled=true;
					//button7.Enabled=true;
				}else{
					button4.Enabled=false;
					//button7.Enabled=false;
				}
			}else{
				MessageBox.Show("Seleccione el periodo que se consultará.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}
		
		public void carga_excel(){
			String cad_con;
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos excel
			dialog.Title = "Seleccione el archivo de Excel";//le damos un titulo a la ventana
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				label2.Text = dialog.SafeFileName;
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();
				
				if(ext.Equals("lsx")){
					MessageBox.Show("Asegurate de Cerrar el archivo en Excel, Antes de abrirlo aqui","Advertencia");
				}
				
				//esta cadena es para archivos excel 2007 y 2010
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion
				
				carga_chema_excel();
				
			}
		}
		
		public void carga_chema_excel(){
			
			int filas = 0;
			String tabla;
			i=0;
			comboBox1.Items.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
		    dataGridView2.DataSource =dt;
		    filas=(dataGridView2.RowCount);
					do{
						if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
							if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
								tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
								if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
									tabla = tabla.Remove((tabla.Length-1),1);
									comboBox1.Items.Add(tabla);
								}
							}
						}
						i++;
					}while(i<filas);
					
                    dt.Clear();
                    dataGridView2.DataSource = dt; //vaciar datagrid
		}
		
		public void cargar_hoja_excel(){
			String hoja,cons_exc;
				hoja = comboBox1.SelectedItem.ToString();
			
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select * from [" + hoja + "$] ";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
						dataGridView3.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						conexion.Close();//cerramos la conexion
						dataGridView3.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						label5.Text="Registros: "+dataGridView3.RowCount;
						label5.Refresh();
												
						//estilo datagrid
                        i = 0;
                        do
                        {
                        	dataGridView3.Columns[i].HeaderText=dataGridView3.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView3.ColumnCount);
                        i = 0;			

						if(dataGridView3.RowCount>0){
							button4.Enabled=true;
							//button7.Enabled=true;							
						}else{
							button4.Enabled=false;
							//button7.Enabled=false;	
						}                        
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n"+ex,"Error al Abrir Archivo de Excel");
				}
				
			}
			
		}
		
		public void filtro(){
			String rp,ruta;
			int rp_num=0,j=0;
			FileStream fichero,fichero1;
			
			 try{
				i=0;
				tablarow.Columns.Clear();
				tablarow_consulta.Columns.Clear();
				if(tablarow.Columns.Contains("REGISTRO PATRONAL")==false){
					tablarow.Columns.Add("REGISTRO PATRONAL");
					tablarow_consulta.Columns.Add("REGISTRO PATRONAL");
				}
            	//Borrar archivos para comenzar de 0
            	System.IO.File.Delete(@"sindo/lista_nrp.txt");
            	System.IO.File.Delete(@"sindo/config_sindo.txt");
            	if(File.Exists(@"sindo/consulta_sindo.txt")==true){
            		System.IO.File.Delete(@"sindo/consulta_sindo.txt");
            	}
        	    //Crear archivos nuevos
                fichero = System.IO.File.Create(@"sindo/lista_nrp.txt");
                fichero1= System.IO.File.Create(@"sindo/config_sindo.txt");
                
                ruta=fichero.Name;
                
                fichero.Close();
           		fichero1.Close();
           
                //Abrir archivo
                StreamWriter wr = new StreamWriter(@"sindo/lista_nrp.txt");
                
                do{
                	rp=dataGridView3.Rows[i].Cells[0].Value.ToString();
                	
                	if(rp.Length==10){
	                	if(int.TryParse(rp.Substring(1,9),out rp_num)==true){
                			wr.WriteLine(rp);
                			tablarow_consulta.Rows.Add(rp);
                			j++;
	                	}else{
                			tablarow.Rows.Add(rp);
	                	}
                	}else{
                		tablarow.Rows.Add(rp);
                	}
                	i++;
                }while(i<dataGridView3.RowCount);
                wr.WriteLine("%&");
                wr.Close();
                
				
                StreamWriter wr1 = new StreamWriter(@"sindo/config_sindo.txt");
                if(textBox1.Text.Equals("consulta_sindo.txt")){
                	ruta_sindo=ruta.Substring(0,(ruta.Length-14))+"\\"+textBox1.Text;
                	 wr1.WriteLine(ruta_sindo);
                }else{
                	ruta_sindo=textBox1.Text;
                	wr1.WriteLine(textBox1.Text);
                }
                wr1.WriteLine(j);
                wr1.Close();
                
                dataGridView3.DataSource=tablarow;
                
               DialogResult respuesta = MessageBox.Show("Se Consultarán: "+j+" Registros y se Omitirán: " +tablarow.Rows.Count+" Registros por no estar bien escritos.\n "+
                                            "Está a punto de comenzar el proceso de consulta automática.\n"+
				                			"Una vez comenzada la consulta automática NO se deberá manipular el\n"+
				                			"equipo hasta que haya finalizado el proceso de consulta.\n"+
				                			"El programa le informará cuando el proceso haya concluido\n\n"+
				                
				                "¿Desea comenzar el proceso de consulta?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
                if(respuesta ==DialogResult.Yes){
                	
                	MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
                	StreamWriter wr2 = new StreamWriter(@"busca_sindo.bat");
                	wr2.WriteLine("@echo off");
                	wr2.WriteLine("C:");
                	wr2.WriteLine("cd \""+ruta.Substring(0,(ruta.Length-14))+"\"");
                	wr2.WriteLine("start sindo_zeke.exe");
                	wr2.Close();
                	System.Diagnostics.Process.Start(@"busca_sindo.bat");
                }
               
                
			}catch{
				
			}
		}
		
		public void lectura_archivo(){
			Invoke(new MethodInvoker(delegate{
			int tot=0,j=0,band_ini=0,band_nom=0,k=0,l=0,encontrado=0;
			string[] aux;
			String linea,rfc="",reg="",nom="",act="",dom="",loc="",cp="",s_not="",fecmov="",tipomov="",causa_b="",subdele="",reg_pat1="",reg_pat2="";
			
			StreamReader sr = new StreamReader(ruta_sindo);
			textBox2.Text = sr.ReadToEnd();
			aux=textBox2.Lines;
			tot=aux.Length;
			sr.Close();
			progressBar1.Value=0;
			
			tablasindo.Rows.Clear();
			
			StreamReader sr1 = new StreamReader(ruta_sindo);
			//MessageBox.Show("lineas:"+tot);
			label8.Text="Leyendo Archivo";
			label8.Refresh();
            tot_errs = 0;

			
			do{
				linea=sr1.ReadLine();
				
				try{
				
				if(band_ini==1){
					
					if (linea.Contains("REGISTRO PATRONAL :") == true)
                    {
                        reg_pat2 = linea.Substring(23, 10);
                    }
					
					if(linea.Length>16){
						if(linea.Substring(1,17).Equals("REGISTRO PATRONAL")){
							reg=linea.Substring(21,14);
							rfc=linea.Substring(linea.Length-13,13);
						}
					}
						
					if(band_nom==1){
						nom=linea;
						band_nom=0;
					}
						
					if(linea.Contains("NOMBRE")){
						band_nom=1;
						//MessageBox.Show("nombre activado");
					}
						
					if(linea.Length>8){	
						if(linea.Substring(1,9).Equals("ACTIVIDAD")){
							act=linea.Substring(22,linea.Length-22);
						}
					
						if(linea.Substring(1,9).Equals("DOMICILIO")){
							dom=linea.Substring(22,linea.Length-22);
						}
						
						if(linea.Substring(1,9).Equals("LOCALIDAD")){
							loc=linea.Substring(22,46);
							loc=loc.TrimEnd(null);
							cp=linea.Substring(linea.Length-7,7);
							cp=cp.TrimStart(null);
						}
					}
					
					if(linea.Contains("SUBDELEGACION")==true){
						subdele=linea.Substring(47,2);    
					}
						
					if(linea.Contains("SECTOR DE NOTIFICACION")==true){
						s_not=linea.Substring(linea.Length-2,2);
					}
					
					if(linea.Contains("FECHA DE MOVIMIENTO")==true){
						fecmov=linea.Substring(22,10);
						tipomov=linea.Substring(linea.Length-1,1);
					}
					
					if(linea.Contains("CAUSA DE BAJA")==true){
						if(tipomov.Equals("2")){
							causa_b=linea.Substring(19,35);
							causa_b=causa_b.TrimEnd(null);
						}
					}
				}
					
				if(linea.Length>9){
					if(linea.Substring(1,9).Equals("S.IN.D.O.")){
						band_ini=1;
					}
				}
					
				if(linea.Contains("DIGITE EL REGISTRO DEL PATRON")==true){
					band_ini=0;
					if(rfc.Contains("/")){
						rfc=rfc.Substring(1,rfc.Length-1);
					}
					
					reg=reg.Substring(0,10)+reg.Substring(reg.Length-1,1);
                    reg_pat1 = reg.Substring(0, 10);

                    if ((reg.TrimEnd(null)).Length == 11)
                    {
                        tablasindo.Rows.Add(rfc.TrimEnd(null), reg.TrimEnd(null), nom.TrimEnd(null), act.TrimEnd(null), dom.TrimEnd(null), loc.TrimEnd(null), subdele.TrimEnd(null), cp.TrimEnd(null), s_not.TrimEnd(null), fecmov.TrimEnd(null), tipomov.TrimEnd(null), causa_b.TrimEnd(null), "");
                    }
                    else
                    {
                        tot_errs++;
                    }

                    rfc="";
					reg="";
					nom="";
					act="";
					loc="";
					cp="";
					s_not="";
					fecmov="";
					tipomov="";
					causa_b="";
					subdele="";
                    reg_pat1 = "";
                    reg_pat2 = "";
				}
					
				}catch{}
				progreso(tot,j);
				j++;
			}while(j<tot);
			
			
			dataGridView1.DataSource=tablasindo;
			dataGridView1.Columns[12].Visible=false;
			dataGridView1.Visible=true;
			label5.Text="Registros: "+dataGridView1.RowCount;
			label5.Refresh();
			
			try {
				
			
			//verificar erroneos
			while(k<tablarow_consulta.Rows.Count){
				while(l<tablasindo.Rows.Count){
					if(tablarow_consulta.Rows[k][0].ToString().Equals(tablasindo.Rows[l][1].ToString().Substring(0,10))){
						encontrado=1;
					}
					
					if(encontrado==1){
						l=tablasindo.Rows.Count+1;
					}
					
					l++;
				}
				
				if(encontrado==0){
					tablarow.Rows.Add(tablarow_consulta.Rows[k][0].ToString());
					tot_errs++;
				}
				
				l=0;
				encontrado=0;
				k++;
			}
			
			dataGridView3.DataSource=tablarow;
			} catch (Exception xc) {
				
			}
			ingreso_base();
			
			                         }));
		}
		
		public void ingreso_base(){
			
			int k=0,l=0,m=0;
			String sql,fecha,rfc_sin_diag,nom="";
			
			conex.conectar("base_principal");
			z=0;
			
			label8.Text="Ingresando Información a la Base de Datos";
			label8.Refresh();
			
			do{
				sql="SELECT registro_patronal FROM sindo WHERE registro_patronal=\""+tablasindo.Rows[k][1].ToString()+"\"";
				dataGridView2.DataSource=conex.consultar(sql);
				
				if(dataGridView2.RowCount>0){
					tablasindo.Rows[k][12]="1";
				}else{
					tablasindo.Rows[k][12]="0";
				}
				
				k++;
			}while(k < tablasindo.Rows.Count);
			
			k=0;
			
			do{
				if(tablasindo.Rows[k][12].ToString().Equals("0")){
					fecha=tablasindo.Rows[k][9].ToString();
					fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
					rfc_sin_diag=tablasindo.Rows[k][0].ToString();
                    nom = tablasindo.Rows[k][2].ToString();

					while(rfc_sin_diag.Contains("/")==true){
						rfc_sin_diag=rfc_sin_diag.Remove(rfc_sin_diag.IndexOf('/'),1);
					}

                    while (nom.Contains("\"") == true)
                    {
                        nom = nom.Remove(nom.IndexOf('"'), 1);
                    }
					
					sql="INSERT INTO sindo (registro_patronal,rfc,nombre,actividad,domicilio,localidad,subdel,cp,sector_not,fecha_mov,tipo_mov,causa_baja) "+
						"VALUES (\""+tablasindo.Rows[k][1].ToString()+"\","+
								"\""+rfc_sin_diag+"\","+
								"\""+nom+"\","+
								"\""+tablasindo.Rows[k][3].ToString()+"\","+
								"\""+tablasindo.Rows[k][4].ToString()+"\","+
								"\""+tablasindo.Rows[k][5].ToString()+"\","+
								"\""+tablasindo.Rows[k][6].ToString()+"\","+
								"\""+tablasindo.Rows[k][7].ToString()+"\","+
								"\""+tablasindo.Rows[k][8].ToString()+"\","+
								"\""+fecha+"\","+
								"\""+tablasindo.Rows[k][10].ToString()+"\","+
								"\""+tablasindo.Rows[k][11].ToString()+"\")";
					conex.consultar(sql);
					l++;
				}else{
					fecha=tablasindo.Rows[k][9].ToString();
					fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
					sql="UPDATE sindo SET rfc=\""+tablasindo.Rows[k][0].ToString()+"\","+
						"nombre=\""+nom+"\","+
						"actividad=\""+tablasindo.Rows[k][3].ToString()+"\","+
						"domicilio=\""+tablasindo.Rows[k][4].ToString()+"\","+
						"localidad=\""+tablasindo.Rows[k][5].ToString()+"\","+
						"subdel=\""+tablasindo.Rows[k][6].ToString()+"\","+
						"cp=\""+tablasindo.Rows[k][7].ToString()+"\","+
						"sector_not=\""+tablasindo.Rows[k][8].ToString()+"\","+
						"fecha_mov=\""+fecha+"\","+
						"tipo_mov=\""+tablasindo.Rows[k][10].ToString()+"\","+
						"causa_baja=\""+tablasindo.Rows[k][11].ToString()+"\" WHERE registro_patronal=\""+tablasindo.Rows[k][1].ToString()+"\"";
						
					conex.consultar(sql);
					m++;
				}
				progreso2(tablasindo.Rows.Count,k);
				k++;
			}while(k < tablasindo.Rows.Count);
			
			progressBar1.Value=100;
			label7.Text="100%";
			label8.Text="Proceso Completado Exitosamente";
			MessageBox.Show("El proceso terminó correctamente.\nSe Ingresaron "+l+" Nuevos Registros.\nSe Actualizó la información de: "+m+" Registros Previamente Ingresados.\nSe omitieron: "+tot_errs+" Registros por contener algún Error de Captura.","¡EXITO!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			panel3.Visible=false;
			button4.Enabled=true;
			button8.Enabled=true;
			button9.Visible=true;
		}
		
		public void progreso(int total,int progreso){
			int porcent_act=0,prog_tot=0,x=0;
			prog_tot=total;
			x=progreso;
			
			porcent_act=Convert.ToInt32(((x*100)/prog_tot)/2);
			label7.Text=(porcent_act)+"%";
			label7.Refresh();
			if(porcent_act>=99){
				porcent_act=100;
			}
			progressBar1.Value=porcent_act;
			progressBar1.Refresh();
			z++;
			
			if(z==52){
				label8.Text="Leyendo Archivo.";
				label8.Refresh();
			}
			
			if(z==104){
				label8.Text="Leyendo Archivo..";
				label8.Refresh();
			}
			
			if(z==156){
				label8.Text="Leyendo Archivo...";
				label8.Refresh();
			}
			
			if(z==208){
				label8.Text="Leyendo Archivo";
				label8.Refresh();
				z=0;
			}
		}
		
		public void progreso2(int total,int progreso){
			int porcent_act=0,prog_tot=0,x=0;
			prog_tot=total;
			x=progreso;
			
			porcent_act=Convert.ToInt32((((x*100)/prog_tot)/2)+50);
			label7.Text=(porcent_act)+"%";
			label7.Refresh();
			if(porcent_act>=99){
				porcent_act=100;
			}
			progressBar1.Value=porcent_act;
			progressBar1.Refresh();
			
			z++;
			
			if(z==10){
				label8.Text="Ingresando Información a la Base de Datos.";
				label8.Refresh();
			}
			
			if(z==20){
				label8.Text="Ingresando Información a la Base de Datos..";
				label8.Refresh();
			}
			
			if(z==30){
				label8.Text="Ingresando Información a la Base de Datos...";
				label8.Refresh();
			}
			
			if(z==40){
				label8.Text="Ingresando Información a la Base de Datos";
				label8.Refresh();
				z=0;
			}
		}
		
		void Sindo_natorLoad(object sender, EventArgs e)
		{
			String window_name = this.Text;
			//window_name = window_name.Replace("Nova Gear", "Gear Prime");
			this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
			llenar_Cb2_todos();
			
			tablasindo.Columns.Add("rfc");
			tablasindo.Columns.Add("reg_pat");
			tablasindo.Columns.Add("nombre");
			tablasindo.Columns.Add("actividad");
			tablasindo.Columns.Add("domicilio");
			tablasindo.Columns.Add("localidad");
			tablasindo.Columns.Add("subdele");
			tablasindo.Columns.Add("cp");
			tablasindo.Columns.Add("sector_not");
			tablasindo.Columns.Add("fecha_mov");
			tablasindo.Columns.Add("tipo_mov");
			tablasindo.Columns.Add("causa_baja");
			tablasindo.Columns.Add("existente");
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			panel2.Visible=true;			
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			panel2.Visible=false;
			panel1.Visible=true;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			consultar();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			carga_excel();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex>-1){
				cargar_hoja_excel();
			}
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "Archivos de texto (*.txt)|*.txt"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos excel
			dialog.Title = "Guardar Archivo de Texto";//le damos un titulo a la ventana
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textBox1.Text=dialog.FileName;
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			button9.Visible=false;
			dataGridView1.Visible=false;
			filtro();
			button4.Enabled=false;
		}
		
		void TerminarToolStripMenuItemClick(object sender, EventArgs e)
		{
			button4.Enabled=false;
			button8.Enabled=false;
			label8.Text="Comenzando Lectura de Archivo";
			label8.Refresh();
			panel3.Visible=true;
			dataGridView1.Visible=true;
			
			hilosecundario = new Thread(new ThreadStart(lectura_archivo));
			hilosecundario.Start();
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			String ruta;
			FileStream fichero;
			
			if(textBox1.Text.Equals("consulta_sindo.txt")){
				fichero = System.IO.File.Create(@"sindo/vacio.txt");
				ruta=fichero.Name;
				fichero.Close();
				
				ruta_sindo=ruta.Substring(0,(ruta.Length-10))+"\\"+textBox1.Text;
			}else{
				ruta_sindo=textBox1.Text;
			}
			
			DialogResult res = MessageBox.Show("Se Llevará a cabo la lectura de archivo:\n•"+ruta_sindo+"\nEsto también ingresará informacion a la base de datos\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(res==DialogResult.Yes){
				try{
					button4.Enabled=false;
					button8.Enabled=false;
					label8.Text="Comenzando Lectura de Archivo";
					label8.Refresh();
					panel3.Visible=true;
					hilosecundario = new Thread(new ThreadStart(lectura_archivo));
					hilosecundario.Start();
				}catch(Exception er){
					MessageBox.Show("Ocurrió el siguiente error al leer el archivo:\n"+er,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}
			
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			if((progressBar1.Value==100)){
				if(dataGridView1.Visible==true){
					dataGridView1.Visible=false;
					label5.Text="Registros: "+dataGridView3.RowCount;
					label5.Refresh();
					button9.Text="Ocultar\n Omitidos";
					
				}else{
					dataGridView1.Visible=true;
					label5.Text="Registros: "+dataGridView1.RowCount;
					label5.Refresh();
					button9.Text=" Ver\n Omitidos";
				}
			}
		}

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            llenar_Cb2_todos_especiales();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            llenar_Cb2_todos();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            conex2.conectar("base_principal");

            SaveFileDialog dialog_save = new SaveFileDialog();
            dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
            dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

            //excel = conex3.consultar("SELECT * FROM estrados");

            if (dialog_save.ShowDialog() == DialogResult.OK)
            {

                //dataGridView1.DataSource = conex3.consultar("SELECT * FROM estrados WHERE id_estrados = 11415 limit 11399");
                excel = conex2.consultar("SELECT * FROM sindo");
                //dataGridView1.DataSource = conex2.consultar("SELECT * FROM estrados");
                /*
                excel2.Columns.Add("id_estrados");
                excel2.Columns.Add("id_estrados1");
                excel2.Columns.Add("id_estrados2");
                excel2.Columns.Add("id_estrados3");
                excel2.Columns.Add("id_estrados4");
                excel2.Columns.Add("id_estrados5");
                excel2.Columns.Add("id_estrados6");
                excel2.Columns.Add("id_estrados7");
                excel2.Columns.Add("id_estrados8");
                excel2.Columns.Add("id_estrados9");
                excel2.Columns.Add("id_estrados10");
                excel2.Columns.Add("id_estrados11");
                excel2.Columns.Add("id_estrados12");
                excel2.Columns.Add("id_estrados13");
                excel2.Columns.Add("id_estrados14");
                excel2.Columns.Add("id_estrados15");
                excel2.Columns.Add("id_estrados16");
                excel2.Columns.Add("id_estrados17");
                excel2.Columns.Add("id_estrados18");
                excel2.Columns.Add("id_estrados19");

                MessageBox.Show("|"+dataGridView1[18, 0].Value.ToString()+"|");

                excel2.Rows.Add(dataGridView1[0, 0].Value.ToString(),
                                dataGridView1[1, 0].Value.ToString(),
                                dataGridView1[2, 0].Value.ToString(),
                                dataGridView1[3, 0].Value.ToString(),
                                dataGridView1[4, 0].Value.ToString(),
                                dataGridView1[5, 0].Value.ToString(),
                                dataGridView1[6, 0].Value.ToString(),
                                dataGridView1[7, 0].Value.ToString(),
                                dataGridView1[8, 0].Value.ToString(),
                                dataGridView1[9, 0].Value.ToString(),
                                dataGridView1[10, 0].Value.ToString(),
                                dataGridView1[11, 0].Value.ToString(),
                                dataGridView1[12, 0].Value.ToString(),
                                dataGridView1[13, 0].Value.ToString(),
                                dataGridView1[14, 0].Value.ToString(),
                                dataGridView1[15, 0].Value.ToString(),
                                dataGridView1[16, 0].Value.ToString(),
                                dataGridView1[17, 0].Value.ToString(),
                                dataGridView1[18, 0].Value.ToString(),
                                dataGridView1[19, 0].Value.ToString()
                                );
                 */
                //excel = copiar_datagrid();

                //tabla_excel
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(excel, "Sindo");
                wb.SaveAs(@"" + dialog_save.FileName + "");
                MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
            }
        }
	}
}

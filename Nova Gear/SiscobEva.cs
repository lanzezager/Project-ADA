/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 17/01/2019
 * Hora: 11:56 p. m.
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
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using Ionic.Zip;

namespace Nova_Gear
{
	/// <summary>
	/// Description of SiscobEva.
	/// </summary>
	public partial class SiscobEva : Form
	{
		public SiscobEva()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int tipo_capt=0,total_env=0;
		String nom_save="";
		
		FileStream fichero,fichero1,fichero2;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		DataTable tablarow = new DataTable();
		DataTable tabla_inc_errs = new DataTable();
		DataTable tabla_am_cop_errs = new DataTable();
		DataTable tabla_am_rcv_errs = new DataTable();
		
		public void activar_botones(){
			panel2.Enabled=true;
			textBox1.Enabled=true;
			button10.Enabled=true;
			button10.Enabled=true;
			comboBox2.Enabled=true;
		}
		
		public void desactivar_botones(){
			panel2.Enabled=false;
			textBox1.Enabled=false;
			button10.Enabled=false;
			button10.Enabled=false;
			comboBox2.Enabled=false;
		}
		
		public void carga_excel(){
			String ext="",cad_con="",archivo="";
			
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
			
			int i=0,filas=0;
			String tabla="";
			
			comboBox1.Items.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
					dataGridView2.DataSource =dt;
					filas=(dataGridView2.RowCount)-2;
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
					}while(i<=filas);
					
                    dt.Clear();
                    dataGridView2.DataSource = dt; //vaciar datagrid
		}
		
		public void cargar_hoja_excel(){
			String hoja="",cons_exc="";
			int i=0,j=0;
			
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
						//dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						tablarow=dataSet.Tables[0];
						//inicializar datagrid
						dataGridView1.DataSource=null;
						
						//crear columnas en datagrid
						while(j<tablarow.Columns.Count){
							dataGridView1.Columns.Add(tablarow.Columns[j].ColumnName.ToUpper(),tablarow.Columns[j].ColumnName.ToUpper());
							j++;
						}
						
						//pasar datos al datagrid
						j=0;
						i=0;
						while(i<tablarow.Rows.Count){
							dataGridView1.Rows.Add();
							while(j<tablarow.Columns.Count){
								dataGridView1.Rows[i].Cells[j].Value=tablarow.Rows[i][j].ToString();
								j++;
							}
							j=0;
							i++;
						}
												
						conexion.Close();//cerramos la conexion
						dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						label5.Text="Casos: "+dataGridView1.RowCount;
						label5.Refresh();
						button10.Enabled=true;		
						button4.Enabled=false;	

						
                        //MessageBox.Show(dataGridView1.Columns[2].HeaderText.ToUpper());
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n"+ex,"Error al Abrir Archivo de Excel");
				}
			}
			
		}
		
		public String valida_decimales(String valor){
			int pos_punto=0;
			string valor_corregido="";
			
			if(valor.Contains(".")){
				pos_punto=valor.IndexOf('.');
				if(pos_punto==valor.Length-3){
					valor_corregido=valor;
				}else{
                    if(valor.Length>=(pos_punto+3)){
					    valor_corregido=valor.Substring(0,pos_punto+3);
                    }else{
                        valor_corregido = valor.Substring(0, pos_punto + 2)+"0";
                    }
				}
			}else{ 
				valor_corregido=valor+".00";
			}
			
			return valor_corregido;
		}
			
		public void filtro_inc(){
			int i=0,res=0,tot=0;
			
			//Borrar archivos para comenzar de 0
			if(System.IO.File.Exists(@"capturador_universal/inc/temp1.txt")==true){
				System.IO.File.Delete(@"capturador_universal/inc/temp1.txt");
			}
			if(System.IO.File.Exists(@"capturador_universal/inc/temp_aux1.txt")==true){
				System.IO.File.Delete(@"capturador_universal/inc/temp_aux1.txt");
			}
			
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"capturador_universal/inc/temp1.txt");
			
			if(comboBox2.SelectedIndex==0){//COP
				tabla_inc_errs.Rows.Clear();
				while(i<dataGridView1.RowCount){
					
					res=0;
					if(dataGridView1.Rows[i].Cells[0].Value.ToString().Length==10){
						res=1;
					}else{
						res=2;
					}
					if(dataGridView1.Rows[i].Cells[1].Value.ToString().Length==9){
						res=res+1;
					}else{
						res=res+2;
					}
					if((dataGridView1.Rows[i].Cells[2].Value.ToString().Length==6)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().StartsWith("20"))){
						res=res+1;
					}else{
						res=res+2;
					}
					
					if(res==3){
						wr.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());//reg_pat
						wr.WriteLine(dataGridView1.Rows[i].Cells[1].Value.ToString());//credito
						wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(4, 2));//per_anio
						wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(2, 2));//per_mes
						wr.WriteLine(textBox1.Text);//incidencia
						wr.WriteLine("$");
						tot++;
					}else{
						tabla_inc_errs.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),dataGridView1.Rows[i].Cells[1].Value.ToString(),dataGridView1.Rows[i].Cells[2].Value.ToString());
					}
					
					i++;
				}
				
			}else{//RCV
				tabla_inc_errs.Rows.Clear();
				while(i<dataGridView1.RowCount){
					
					res=0;
					if(dataGridView1.Rows[i].Cells[0].Value.ToString().Length==10){
						res=1;
					}else{
						res=2;
					}
					if(dataGridView1.Rows[i].Cells[1].Value.ToString().Length==9){
						res=res+1;
					}else{
						res=res+2;
					}
					if((dataGridView1.Rows[i].Cells[2].Value.ToString().Length==6)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().StartsWith("20"))){
						res=res+1;
					}else{
						res=res+2;
					}
					
					if(res==3){
						wr.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());//reg_pat
                        wr.WriteLine(dataGridView1.Rows[i].Cells[1].Value.ToString());//credito	
						wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(4, 2));//per_anio
						wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(2, 2));//per_mes
						//wr.WriteLine(dataGridView1.Rows[i].Cells[1].Value.ToString());//credito						
						wr.WriteLine(textBox1.Text);//incidencia
						wr.WriteLine("$");
						tot++;
					}else{
						tabla_inc_errs.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),dataGridView1.Rows[i].Cells[1].Value.ToString(),dataGridView1.Rows[i].Cells[2].Value.ToString());
					}
					
					i++;
				}
			}
			
			wr.WriteLine("%&");
			wr.Close();

			StreamWriter wr1 = new StreamWriter(@"capturador_universal/inc/temp_aux.txt");
			wr1.WriteLine("0");
			wr1.WriteLine(tot);
			wr1.Close();
			total_env=tot;
			MessageBox.Show("Se Listaron: "+tot+" Registros y se Omitieron: "+tabla_inc_errs.Rows.Count+" Por contener algún error.","Verificación Exitosa",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1);
		}
		
		public void filtro_am(){
			int i=0,res=0,tot=0;
			decimal importe_tot=0,importe_ret_p=0,importe_rcv_o=0,importe_rcv_p=0;
			
			//Borrar archivos para comenzar de 0
			if(System.IO.File.Exists(@"capturador_universal/am/temp1.txt")==true){
				System.IO.File.Delete(@"capturador_universal/am/temp1.txt");
			}
			if(System.IO.File.Exists(@"capturador_universal/am/temp_aux1.txt")==true){
				System.IO.File.Delete(@"capturador_universal/am/temp_aux1.txt");
			}
			
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"capturador_universal/am/temp1.txt");
			
			if(comboBox2.SelectedIndex==0){//COP
				
				tabla_am_cop_errs.Rows.Clear();
				while(i<dataGridView1.RowCount){
					
					res=0;
					if(dataGridView1.Rows[i].Cells[0].Value.ToString().Length==10){
						res=1;
					}else{
						res=2;
					}
					if(dataGridView1.Rows[i].Cells[1].Value.ToString().Length==9){
						res=res+1;
					}else{
						res=res+2;
					}
					if((dataGridView1.Rows[i].Cells[2].Value.ToString().Length==6)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().StartsWith("20"))){
						res=res+1;
					}else{
						res=res+2;
					}
					if(decimal.TryParse(dataGridView1.Rows[i].Cells[3].Value.ToString(),out importe_tot)){
						res=res+1;
						dataGridView1.Rows[i].Cells[3].Value=valida_decimales(dataGridView1.Rows[i].Cells[3].Value.ToString());
					}else{
						res=res+2;
					}
					
					if(res==4){
						wr.WriteLine(textBox1.Text);//ajuste
						wr.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());//reg_pat
						wr.WriteLine(dataGridView1.Rows[i].Cells[1].Value.ToString());//credito
						wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(4, 2));//per_anio
						wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(2, 2));//per_mes
						wr.WriteLine(dataGridView1.Rows[i].Cells[3].Value.ToString());//importe
						wr.WriteLine("$");
						tot++;
					}else{
						tabla_am_cop_errs.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),dataGridView1.Rows[i].Cells[1].Value.ToString(),dataGridView1.Rows[i].Cells[2].Value.ToString(),dataGridView1.Rows[i].Cells[3].Value.ToString());
					}
					
					i++;
				}
			}else{//RCV
				if(dataGridView1.Columns.Count>6){
					tabla_am_rcv_errs.Rows.Clear();
					while(i<dataGridView1.RowCount){
						
						res=0;
						if(dataGridView1.Rows[i].Cells[0].Value.ToString().Length==10){
							res=1;
						}else{
							res=2;
						}
						if(dataGridView1.Rows[i].Cells[1].Value.ToString().Length==9){
							res=res+1;
						}else{
							res=res+2;
						}
						if((dataGridView1.Rows[i].Cells[2].Value.ToString().Length==6)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().StartsWith("20"))){
							res=res+1;
						}else{
							res=res+2;
						}
						if(decimal.TryParse(dataGridView1.Rows[i].Cells[3].Value.ToString(),out importe_tot)){
							res=res+1;
							dataGridView1.Rows[i].Cells[3].Value=valida_decimales(dataGridView1.Rows[i].Cells[3].Value.ToString());
							importe_tot=Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
							
						}else{
							res=res+2;
						}
						if(decimal.TryParse(dataGridView1.Rows[i].Cells[4].Value.ToString(),out importe_ret_p)){
							res=res+1;
							dataGridView1.Rows[i].Cells[4].Value=valida_decimales(dataGridView1.Rows[i].Cells[4].Value.ToString());
							importe_ret_p=Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value.ToString());
							
						}else{
							res=res+2;
						}
						if(decimal.TryParse(dataGridView1.Rows[i].Cells[5].Value.ToString(),out importe_rcv_o)){
							res=res+1;
							dataGridView1.Rows[i].Cells[5].Value=valida_decimales(dataGridView1.Rows[i].Cells[5].Value.ToString());
							importe_rcv_o=Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
						}else{
							res=res+2;
						}
						if(decimal.TryParse(dataGridView1.Rows[i].Cells[6].Value.ToString(),out importe_rcv_p)){
							res=res+1;
							dataGridView1.Rows[i].Cells[6].Value=valida_decimales(dataGridView1.Rows[i].Cells[6].Value.ToString());
							importe_rcv_p=Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString());
						}else{
							res=res+2;
						}
						
						if((importe_rcv_o+importe_rcv_p+importe_ret_p)==importe_tot){
							res=res+1;
						}else{
							res=res+2;
						}
						
						if(res==8){
							wr.WriteLine(textBox1.Text);//ajuste
							wr.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());//reg_pat
							wr.WriteLine(dataGridView1.Rows[i].Cells[1].Value.ToString());//credito
							wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(4, 2));//per_anio
							wr.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(2, 2));//per_mes
							wr.WriteLine("#");
							wr.WriteLine(dataGridView1.Rows[i].Cells[3].Value.ToString());//importe_tot
							wr.WriteLine(dataGridView1.Rows[i].Cells[4].Value.ToString());//importe_ret_p
							wr.WriteLine(dataGridView1.Rows[i].Cells[5].Value.ToString());//importe_rcv_o
							wr.WriteLine(dataGridView1.Rows[i].Cells[6].Value.ToString());//importe_rcv_p
							wr.WriteLine("$");
							tot++;
						}else{
							tabla_am_rcv_errs.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),dataGridView1.Rows[i].Cells[1].Value.ToString(),dataGridView1.Rows[i].Cells[2].Value.ToString(),dataGridView1.Rows[i].Cells[3].Value.ToString(),dataGridView1.Rows[i].Cells[4].Value.ToString(),dataGridView1.Rows[i].Cells[5].Value.ToString(),dataGridView1.Rows[i].Cells[6].Value.ToString());
						}
						
						i++;
					}
				}
			}
			
			wr.WriteLine("%&");
			wr.Close();

			StreamWriter wr1 = new StreamWriter(@"capturador_universal/am/temp_aux1.txt");
			wr1.WriteLine("0");
			wr1.WriteLine(tot);
			wr1.Close();
			
			if(tot>0){
				if(comboBox2.SelectedIndex==0){//COP
					MessageBox.Show("Se Listaron: "+tot+" Registros y se Omitieron: "+tabla_am_cop_errs.Rows.Count+" Por contener algún error.","Verificación Exitosa",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1);
				}else{
					MessageBox.Show("Se Listaron: "+tot+" Registros y se Omitieron: "+tabla_am_rcv_errs.Rows.Count+" Por contener algún error.","Verificación Exitosa",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1);
				}
			}else{
				MessageBox.Show(" No se Pudo Verificar el Contenido de la Hoja Por contener algún error o por No contener el número de columnas adecuado","Verificación Exitosa",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
			}
			total_env=tot;
		}
		
		public int guardar_resultados(){

        		 SaveFileDialog dialog_save = new SaveFileDialog();
        		 dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
        		 dialog_save.Title = "Guardar Resultados de Captura";//le damos un titulo a la ventana

        		  if (dialog_save.ShowDialog() == DialogResult.OK)
        			  {
	        			 nom_save = dialog_save.FileName; //open file
	        			 return 1;
        			     //MessageBox.Show("El archivo se generó correctamente.\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
        		  }else{
        		  		 return 0;
        		  }
		}
		
		public void envio_inc(){
			String ruta="",nom_zip="";
			int nom_dif=0;
			
			if (guardar_resultados()==1){
				try{
					MessageBox.Show("Asegúrese de seguir estos pasos antes de continuar:\n\n"+
					                "1.- Ejecute Siscob y abra la ventana donde se van capturar los datos\n"+
					                "2.- Coloque el cursor en el campo donde se va a comenzar a escribir\n"+
					                "3.- De click al boton Aceptar de este mensaje","ATENCIÓN",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
					
					DialogResult respuesta = MessageBox.Show("Está a punto de comenzar el proceso de captura automática.\n"+
					                                         "Los Archivos Temporales de la Captura Anterior se Borrarán y no podrá recuperarlos si es que estos NO fueron guardados.\n"+
					                                         "Una vez comenzada la captura NO se deberá manipular el equipo\n"+
					                                         "hasta que haya finalizado el proceso de captura.\n"+
					                                         "El programa le informará cuando el proceso de captura haya concluido\n\n"+
					                                         "Teclas Especiales:\n"+
					                                         "[PAUSA] - Pausa el proceso de captura (no se recomienda)\n"+
					                                         "[INICIO] - Reanuda el proceso de captura\n"+
					                                         "[FIN] - Finaliza el proceso de captura\n\n"+
					                                         "¿Desea comenzar el proceso de captura?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
					
					if(respuesta ==DialogResult.Yes){
						/*
 					System.IO.File.Copy(@"capturator_inc/temp.txt",@"capturator_inc/respaldo/temp.txt",true);
					System.IO.File.Copy(@"capturator_inc/errores_siscob.txt",@"capturator_inc/respaldo/errores_siscob.txt",true);
	        	    System.IO.File.Copy(@"capturator_inc/acierto_siscob.txt",@"capturator_inc/respaldo/acierto_siscob.txt",true);
	        	    System.IO.File.Copy(@"capturator_inc/aciertos_siscob.txt",@"capturator_inc/respaldo/aciertos_siscob.txt",true);
					System.IO.File.Copy(@"capturator_inc/info_core.txt",@"capturator_inc/respaldo/info_core.txt",true);
						 */
						
						//Respaldar Archivos de Resultados Previos
						StreamReader rdra = new StreamReader(@"capturador_universal/inc/info_envio.txt");
						nom_zip=rdra.ReadLine();
						rdra.Close();
						
						try{
							ZipFile arch = new ZipFile();
							arch.AddFile(@"capturador_universal/inc/temp.txt","");
							arch.AddFile(@"capturador_universal/inc/errores_siscob.txt","");
							arch.AddFile(@"capturador_universal/inc/acierto_siscob.txt","");
							arch.AddFile(@"capturador_universal/inc/aciertos_siscob.txt","");
							arch.AddFile(@"capturador_universal/inc/info_envio.txt","");
							
							if(nom_zip.Length>0){
							}else{
								nom_zip="Respaldo_INC_";
							}
							
							if(File.Exists(@"capturador_universal/inc/respaldo/"+nom_zip+".LZ")==true){
								while(File.Exists(@"capturador_universal/inc/respaldo/"+nom_zip+"_"+nom_dif+".LZ")==true){
									nom_dif++;
								}
								nom_zip=nom_zip+"_"+nom_dif+".LZ";
							}else{
								nom_zip=nom_zip+".LZ";
							}
							//arch.Save(arch_lz40);
							arch.Save(@"capturador_universal/inc/respaldo/"+nom_zip);
							//MessageBox.Show("El archivo se guardó correctamente.");
							
						}catch(Exception es){
							MessageBox.Show("Ocurrió el siguiente problema al crear el respaldo del último envío:\n\n"+es);
						}
						
						//colocar archivo correcto
						System.IO.File.Copy(@"capturador_universal/inc/temp1.txt",@"capturador_universal/inc/temp.txt",true);
						
						MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
						//Borrar archivos para comenzar de 0
						System.IO.File.Delete(@"capturador_universal/inc/errores_siscob.txt");
						System.IO.File.Delete(@"capturador_universal/inc/acierto_siscob.txt");
						System.IO.File.Delete(@"capturador_universal/inc/aciertos_siscob.txt");
						//Crear archivos nuevos
						fichero = System.IO.File.Create(@"capturador_universal/inc/errores_siscob.txt");
						fichero1= System.IO.File.Create(@"capturador_universal/inc/acierto_siscob.txt");
						fichero2= System.IO.File.Create(@"capturador_universal/inc/aciertos_siscob.txt");
						ruta = fichero.Name;
						fichero.Close();
						fichero1.Close();
						fichero2.Close();
						
						StreamWriter wr1 = new StreamWriter(@"capture_inc.bat");
						wr1.WriteLine("@echo off");
                        wr1.WriteLine("" + ruta.Substring(0, 1) + ":");
						wr1.WriteLine("cd "+ruta.Substring(0,(ruta.Length-19)));
						
						if(comboBox2.SelectedIndex==0){
							wr1.WriteLine("start captron.exe");
						}else{
							wr1.WriteLine("start captron_rcv.exe");
						}
						wr1.Close();
						
						if(System.IO.File.Exists(@"capturador_universal/inc/info_envio.txt")==true){
							System.IO.File.Delete(@"capturador_universal/inc/info_envio.txt");
						}
						
						StreamWriter wr3 = new StreamWriter(@"capturador_universal/inc/info_envio.txt");
						wr3.WriteLine("Respaldo_INC_"+textBox1.Text+"_"+comboBox2.SelectedItem.ToString()+"_"+total_env);
						wr3.Close();
						
						System.Diagnostics.Process.Start(@"capture_inc.bat");
						conex.conectar("base_principal");
						conex.guardar_evento("Se Mando a capturar en Siscob "+total_env+" Registros de Cambio de Incidencia "+textBox1.Text+" de "+comboBox2.SelectedItem.ToString());
						button18.Visible=true;
						button18.Enabled=true;
						button4.Enabled=false;
						button10.Enabled=false;
					}else{
						MessageBox.Show("El proceso no se iniciará.","Información");
					}
					
				}catch(Exception e1){
					MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser iniciado adecuadamente.\n\n Error:\n"+e1,"Información",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}else{
				MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
			}
		}
		
		public void envio_am(){
			String ruta="",nom_zip="";
			int nom_dif=0;
			
			if (guardar_resultados()==1){
				try{
					MessageBox.Show("Asegúrese de seguir estos pasos antes de continuar:\n\n"+
					                "1.- Ejecute Siscob y abra la ventana donde se van capturar los datos\n"+
					                "2.- Coloque el cursor en el campo donde se va a comenzar a escribir\n"+
					                "3.- De click al boton Aceptar de este mensaje","ATENCIÓN",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
					
					DialogResult respuesta = MessageBox.Show("Está a punto de comenzar el proceso de captura automática.\n"+
					                                         "Los Archivos Temporales de la Captura Anterior se Borrarán y no podrá recuperarlos si es que estos NO fueron guardados.\n"+
					                                         "Una vez comenzada la captura NO se deberá manipular el equipo\n"+
					                                         "hasta que haya finalizado el proceso de captura.\n"+
					                                         "El programa le informará cuando el proceso de captura haya concluido\n\n"+
					                                         "Teclas Especiales:\n"+
					                                         "[PAUSA] - Pausa el proceso de captura (no se recomienda)\n"+
					                                         "[INICIO] - Reanuda el proceso de captura\n"+
					                                         "[FIN] - Finaliza el proceso de captura\n\n"+
					                                         "¿Desea comenzar el proceso de captura?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
					
					if(respuesta ==DialogResult.Yes){
						/*
 					System.IO.File.Copy(@"capturator_inc/temp.txt",@"capturator_inc/respaldo/temp.txt",true);
					System.IO.File.Copy(@"capturator_inc/errores_siscob.txt",@"capturator_inc/respaldo/errores_siscob.txt",true);
	        	    System.IO.File.Copy(@"capturator_inc/acierto_siscob.txt",@"capturator_inc/respaldo/acierto_siscob.txt",true);
	        	    System.IO.File.Copy(@"capturator_inc/aciertos_siscob.txt",@"capturator_inc/respaldo/aciertos_siscob.txt",true);
					System.IO.File.Copy(@"capturator_inc/info_core.txt",@"capturator_inc/respaldo/info_core.txt",true);
						 */
						
							//Respaldar Archivos de Resultados Previos
						StreamReader rdra = new StreamReader(@"capturador_universal/am/info_envio.txt");
						nom_zip=rdra.ReadLine();
						rdra.Close();
						
						try{
							ZipFile arch = new ZipFile();
							arch.AddFile(@"capturador_universal/am/temp.txt","");
							arch.AddFile(@"capturador_universal/am/errores_siscob.txt","");
							arch.AddFile(@"capturador_universal/am/acierto_siscob.txt","");
							arch.AddFile(@"capturador_universal/am/aciertos_siscob.txt","");
							arch.AddFile(@"capturador_universal/am/info_envio.txt","");
							
							if(nom_zip.Length>0){
							}else{
								nom_zip="Respaldo_CM_";
							}
							
							if(File.Exists(@"capturador_universal/am/respaldo/"+nom_zip+".LZ")==true){
								while(File.Exists(@"capturador_universal/am/respaldo/"+nom_zip+"_"+nom_dif+".LZ")==true){
									nom_dif++;
								}
								nom_zip=nom_zip+"_"+nom_dif+".LZ";
							}else{
								nom_zip=nom_zip+".LZ";
							}
							//arch.Save(arch_lz40);
							arch.Save(@"capturador_universal/am/respaldo/"+nom_zip);
							//MessageBox.Show("El archivo se guardó correctamente.");
							
						}catch(Exception es){
							MessageBox.Show("Ocurrió el siguiente problema al crear el respaldo del último envío:\n\n"+es);
						}
						
						//colocar archivo correcto
						System.IO.File.Copy(@"capturador_universal/am/temp1.txt",@"capturador_universal/am/temp.txt",true);
						
						
						MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
						//Borrar archivos para comenzar de 0
						System.IO.File.Delete(@"capturador_universal/am/errores_siscob.txt");
						System.IO.File.Delete(@"capturador_universal/am/acierto_siscob.txt");
						System.IO.File.Delete(@"capturador_universal/am/aciertos_siscob.txt");
						//Crear archivos nuevos
						fichero = System.IO.File.Create(@"capturador_universal/am/errores_siscob.txt");
						fichero1= System.IO.File.Create(@"capturador_universal/am/acierto_siscob.txt");
						fichero2= System.IO.File.Create(@"capturador_universal/am/aciertos_siscob.txt");
						ruta = fichero.Name;
						fichero.Close();
						fichero1.Close();
						fichero2.Close();
						
						StreamWriter wr1 = new StreamWriter(@"capture_am.bat");
						wr1.WriteLine("@echo off");
						wr1.WriteLine(""+ruta.Substring(0,1)+":");
						wr1.WriteLine("cd "+ruta.Substring(0,(ruta.Length-19)));
						
						if(comboBox2.SelectedIndex==0){
							wr1.WriteLine("start bumblecapt.exe");
						}else{
							wr1.WriteLine("start bumblecapt_rcv.exe");
						}
						wr1.Close();
						
						StreamWriter wr3 = new StreamWriter(@"capturador_universal/am/info_envio.txt");
						wr3.WriteLine("Respaldo_CM_"+textBox1.Text+"_"+comboBox2.SelectedItem.ToString()+"_"+total_env);
						wr3.Close();
						
						System.Diagnostics.Process.Start(@"capture_am.bat");
						conex.conectar("base_principal");
						conex.guardar_evento("Se Mando a capturar en Siscob "+total_env+" Registros de Ajuste Manual "+textBox1.Text+" de "+comboBox2.SelectedItem.ToString());
						button18.Visible=true;
						button18.Enabled=true;
						button4.Enabled=false;
						button10.Enabled=false;
					}else{
						MessageBox.Show("El proceso no se iniciará.","Información");
					}
					
				}catch(Exception e1){
					MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser iniciado adecuadamente.\n\n Error:\n"+e1,"Información",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}else{
				MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
			}
		}
		
		public void verificar_captura(){
			int i=0,correctos=0,errores=0;
			
			StreamReader rdr,rdr1;
			if(tipo_capt==1){//incidencias
				rdr = new StreamReader(@"capturador_universal/inc/errores_siscob.txt");
				rdr1 = new StreamReader(@"capturador_universal/inc/aciertos_siscob.txt");
				textBox2.Text=rdr.ReadToEnd();//aciertos
				textBox3.Text=rdr1.ReadToEnd();//errores
			}
			
			if(tipo_capt==2){//ajustes
				rdr = new StreamReader(@"capturador_universal/am/errores_siscob.txt");
				rdr1 = new StreamReader(@"capturador_universal/am/aciertos_siscob.txt");
				textBox2.Text=rdr.ReadToEnd();//aciertos
				textBox3.Text=rdr1.ReadToEnd();//errores
			}
			
			
			
			if(tablarow.Columns[tablarow.Columns.Count-1].ColumnName=="RESULTADOS"){}else{
				tablarow.Columns.Add("RESULTADOS");
			}
			
			while(i<tablarow.Rows.Count){
				tablarow.Rows[i][tablarow.Columns.Count-1]="0";
				i++;
			}
			
			i=0;
			while(i<tablarow.Rows.Count){
				if(textBox2.Text.Contains(tablarow.Rows[i][0]+"_"+tablarow.Rows[i][1])==true){
					tablarow.Rows[i][tablarow.Columns.Count-1]="CORRECTO";
					correctos++;
				}
				
				if(tablarow.Rows[i][tablarow.Columns.Count-1].ToString().Length==1){
					if(textBox3.Text.Contains(tablarow.Rows[i][0]+"_"+tablarow.Rows[i][1])==true){
						tablarow.Rows[i][tablarow.Columns.Count-1]="ERROR";
						errores++;
					}
				}
				i++;
			}
			
			XLWorkbook wb = new XLWorkbook();
        	wb.Worksheets.Add(tablarow,"Resultados_Captura_LZ");
        	wb.SaveAs(nom_save);
        	
        	MessageBox.Show("Se terminó correctamente la captura\n"+correctos+" :Registros Correctos\n"+errores+" :Registros con Error\n"+(i-(correctos+errores))+" :Registros Omitidos","Captura Exitosa",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1);
        	MessageBox.Show("El archivo de resultados se guardo correctamente en la siguiente ubicación: "+nom_save,"Captura Exitosa",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1);
        	
        	try {
        		dataGridView1.Rows.Clear();
        		
        	} catch (Exception j) {
        		
        		
        	}
        	try {
        		dataGridView1.Columns.Clear();
        	} catch (Exception j1) {
        		
        		
        	}
        	try {
        		dataGridView1.DataSource=tablarow;
        	} catch (Exception j2) {
        		
        		
        	}
        	
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			tipo_capt=1;
			panel4.Visible=false;
			activar_botones();
			label3.Text="Incidencia";
			panel2.Visible=true;
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			tipo_capt=2;
			panel4.Visible=false;
			activar_botones();
			label3.Text="Ajuste";
			panel2.Visible=true;
		}
		
		void SiscobEvaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			tabla_inc_errs.Columns.Add("REG_PAT");
			tabla_inc_errs.Columns.Add("CREDITO");
			tabla_inc_errs.Columns.Add("PERIODO");
			
			tabla_am_cop_errs.Columns.Add("REG_PAT");
			tabla_am_cop_errs.Columns.Add("CREDITO");
			tabla_am_cop_errs.Columns.Add("PERIODO");
			tabla_am_cop_errs.Columns.Add("IMPORTE");
			
			tabla_am_rcv_errs.Columns.Add("REG_PAT");
			tabla_am_rcv_errs.Columns.Add("CREDITO");
			tabla_am_rcv_errs.Columns.Add("PERIODO");
			tabla_am_rcv_errs.Columns.Add("IMPORTE_TOTAL");
			tabla_am_rcv_errs.Columns.Add("IMPORTE_RETIRO PATRONAL");
			tabla_am_rcv_errs.Columns.Add("IMPORTE_RCV OBRERO");
			tabla_am_rcv_errs.Columns.Add("IMPORTE_RCV PATRONAL");
			
			
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			carga_excel();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex > -1){
				cargar_hoja_excel();
			}
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				if(textBox1.Text.Length>0){
					if(comboBox2.SelectedIndex>-1){
						
						if(tipo_capt==1){
							desactivar_botones();
							filtro_inc();
							//MessageBox.Show(""+total_env);
							if(total_env>0){
								button4.Enabled=true;
							}
							dataGridView1.Rows.Clear();
							dataGridView1.Columns.Clear();
							dataGridView1.DataSource=tabla_inc_errs;
						}
						
						if(tipo_capt==2){
							desactivar_botones();
							filtro_am();
							if(total_env>0){
								button4.Enabled=true;
							}
							
							dataGridView1.Rows.Clear();
							dataGridView1.Columns.Clear();
							if(comboBox2.SelectedIndex==0){
								dataGridView1.DataSource=tabla_am_cop_errs;
							}else{
								dataGridView1.DataSource=tabla_am_rcv_errs;
							}
						}
						
					}
				}
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
            /*
			if(tipo_capt==1){//incidencias
				if(textBox1.Text=="31"){
					textBox1.Text="";
					MessageBox.Show("Para Utilizar la Incidencia 31 utilice el Envío Principal","Valor Incorrecto",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
				}
			}else{//ajustes
				if(textBox1.Text=="12"){
					textBox1.Text="";
					MessageBox.Show("Para Utilizar la Clave de Ajuste 12 utilice el Envío Principal","Valor Incorrecto",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
				}
			}
          */
		}
		
		void TextBox1Leave(object sender, EventArgs e)
		{
			int num=0;
			
			if(int.TryParse(textBox1.Text,out num)){
				if(textBox1.Text.Length==1){
					textBox1.Text="0"+textBox1.Text;
				}
			}else{
				textBox1.Text="";
				MessageBox.Show("Sólo Números (0-99)","Valor Incorrecto",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(total_env>0){
				if(textBox1.Text.Length>0){
					if(comboBox2.SelectedIndex>-1){
						
						if(tipo_capt==1){
							desactivar_botones();
							envio_inc();
						}
						
						if(tipo_capt==2){
							desactivar_botones();
							envio_am();
						}
						
					}
				}
			}
		}
		
		void Button18Click(object sender, EventArgs e)
		{
			verificar_captura();
			activar_botones();
			button18.Visible=false;
			
		}
		
		void FinalizarToolStripMenuItemClick(object sender, EventArgs e)
		{
			verificar_captura();
			activar_botones();
			button18.Visible=false;
			
		}
	}
}

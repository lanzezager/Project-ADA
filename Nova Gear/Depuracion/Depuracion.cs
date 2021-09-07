/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 08/10/2015
 * Hora: 01:44 p. m.
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
using Microsoft.SqlServer.Types;
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;


namespace Nova_Gear.Depuracion
{
	/// <summary>
	/// Description of Depuracion.
	/// </summary>
	public partial class Depuracion : Form
	{
		public Depuracion()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//Declaración de Variables
		String archivo,arch_pro,arch_sip,cad_con,cad_con2,ext,tabla,cons_exc,cons_exc2,hoja,csv_line,periodo_sipare,periodo_sipare_rcv,csv_line_copy,nrp,stat_sql,folio_pago,fecha_pripago,per_sel,per_sel_full,stat_cred_sql,archs_gp_tb,periodo_sipare1;
		String sql,f_sua,fecha_pago,i4ss,rcv_sip,comp_dupli,nrp_sql,f_sua_sql,fecha_pago_sql,i4ss_sql,id_sql,f_sua_acu,fecha_pago_acu,fecha_dep,cre_sql,td_sql,sub_sql,imp_mul_sql,statuz,cad_con3,cre_cuo_sql,tot_archs,fecha_not_dep,per_pag_gp;
		int i=0,j=0,filas=0,tot_cop_est=0,tot_cop_mec=0,tot_rcv_est=0,tot_rcv_mec=0,tipo_arch=0,tipo_cre=0,k=0,l=0,m=0,n=0,p=0,x=0,y=0,num_pago=0,porcen=0,coun=0,counpri=0,arch_sel=0,tipo_doc_sel=0,tipo_dep=0,ev_tot_dep1=1,ev_tot_dep2=0,ev_tot_dep3=0,ev_tot_dep4=0;
		int	pila=0,activo_1=0,activo_2=0,activo_3=0,activo_4=0,valor_tomado1=0,valor_tomado2=0,valor_tomado3=0,valor_tomado4=0,duplicados=0,omitidos=0,tot_no_depurar=0,statu=0,carga_gp=0,gp_count=0,dupli_en=0;
		int mes_periodo=0,tipo_sipare=0,cont_wait=0,error_vacio=0,acumular=0,tot_per_eco_est=0,tot_per_eco_mec=0,tot_per_rcv_est=0,tot_per_rcv_mec=0,no_depurar=0,b18=0,b19=0,b20=0,b21=0,seguro_per=0,ev_depu1=0,ev_depu2=0,ev_depu3=0,ev_depu4=0;
		int tot_pag_cop_est=0,tot_pag_cop_mec=0,tot_pag_rcv_est=0,tot_pag_rcv_mec=0,tot_menores=0,total_cop_est=0,total_cop_mec=0,total_rcv_est=0,total_rcv_mec=0,por_dep=0,dgrc2=0,dgrc1=0,yy=0,tot_gp=0;
		double porcentaje_min_pago=0,importe_pago=0,importe_pago_sql=0,porcentaje_sql=0;
		string[] csv_pre,periodo, ingresados,checar_folio,text,text_lista,archivos_gp,archivos_tot;
		Type tipo_cadena;
		DialogResult respuesta;
        String solicita = "Lic. Ernesto García Casillas\nOficina de Emisiones P.O.", autoriza = "Lic. Alicia Guadalupe Pérez Dávila\nJefe Oficina de Emisiones y P.O.";

		//Declaracion del Hilo para ejecutar un subproceso
		private Thread hilosecundario = null;
		private Thread hiloterciario = null;
		private Thread hilotetrario = null;

		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex1 = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable data_vacio; //En Uso
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		
		//Declaracion de elementos para conexion office 2
		OleDbConnection conexion2 = null;
		DataSet dataSet2 = null;
		OleDbDataAdapter dataAdapter2 = null;
		
		//Declaracion de elementos para conexion office 2
		OleDbConnection conexion3 = null;
		DataSet dataSet3 = null; 
		OleDbDataAdapter dataAdapter3 = null;
		
		//Declaracion de elementos para conexion office 4 RALE
		OleDbConnection conexion4 = null;
		DataSet dataSet4 = null; 
		OleDbDataAdapter dataAdapter4 = null;
		
		
		//Declarar Datatables
		DataTable tablarow = new DataTable();//En Uso
		DataTable tablarow2 = new DataTable();//En Uso
		DataTable tablarow3 = new DataTable();//En Uso
		DataTable tablarow4 = new DataTable();//En Uso
		DataTable data_3 = new DataTable(); //En Uso
		DataTable data_acumulador = new DataTable(); //En Uso
		DataTable data_cop = new DataTable(); //En Uso
		DataTable data_rcv = new DataTable(); //En Uso
		
		DataTable data_report1 = new DataTable(); //En Uso
		DataTable data_report2 = new DataTable(); //En Uso
		DataTable data_report3 = new DataTable(); //En Uso
		DataTable data_report4 = new DataTable(); //En Uso
		
		DataTable data_rale = new DataTable(); //En Uso
		
		/*DataTable data_depura1 = new DataTable(); //En Uso
		DataTable data_depura2 = new DataTable(); //En Uso
		DataTable data_depura3 = new DataTable(); //En Uso
		DataTable data_depura4 = new DataTable(); //En Uso*/
		
		public void carga_excel(){
			
			OpenFileDialog dialog = new OpenFileDialog();
			
			if(tipo_arch==1){
					dialog.Title = "Seleccione el archivo PROCESAR";//le damos un titulo a la ventana
					dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos excel
				}else{
					dialog.Title = "Seleccione el archivo SIPARE";//le damos un titulo a la ventana
					dialog.Filter = "Archivos de Excel (*.xls *.xlsx *.csv)|*.xls;*.xlsx;*.csv"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos excel
				}
			
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();
				
				if(ext.Equals("lsx")){
						//MessageBox.Show("Asegurate de Cerrar el archivo en Excel, Antes de abrirlo aqui","Advertencia");
					}
				
				if(tipo_arch==1){	
					textBox1.Text = archivo;
					//textBox1.Text= @"C:\Users\usuario\Desktop\DEPURACIONES\PROCESAR.csv";
					arch_pro = dialog.SafeFileName;
					if((ext.Equals("xls"))||(ext.Equals("lsx"))){
						//esta cadena es para archivos excel 2007 y 2010
						cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + textBox1.Text + "';Extended Properties=Excel 12.0;";
						conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
						conexion.Open(); //abrimos la conexion
						carga_chema_excel();
					}else{
						
						//StreamReader rdr = new StreamReader(textBox1.Text);
					}
					
					label5.Visible=true;
					label5.Refresh();
				}else{
					textBox2.Text = archivo;
					arch_sip = dialog.SafeFileName;
					if((ext.Equals("xls"))||(ext.Equals("lsx"))){
						tipo_sipare=1;
						//esta cadena es para archivos excel 2007 y 2010
						cad_con2 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + textBox2.Text + "';Extended Properties=Excel 12.0;";
						conexion2 = new OleDbConnection(cad_con2);//creamos la conexion con la hoja de excel
						conexion2.Open(); //abrimos la conexion
						carga_chema_excel2();
					}else{
						tipo_sipare=2;
						//StreamReader rdr = new StreamReader(textBox2.Text);
					}
					
					label6.Visible=true;
					label6.Refresh();
				}
				
			}
		}
	
		public void carga_excel2(){

			dataAdapter3 = new OleDbDataAdapter(cons_exc2, conexion3); //traemos los datos de la hoja y las guardamos en un dataSdapter
			// creamos la instancia del objeto DataSet
			dataSet3 = new DataSet();
			dataAdapter3.Fill(dataSet3, "hoja_lz");//llenamos el dataset
			dataGridView1.DataSource = dataSet3.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
			//conexion3.Close();//cerramos la conexion
			dataGridView1.AllowUserToAddRows = false;
		}
		
		public void carga_excel3(){

			dataAdapter4 = new OleDbDataAdapter(cons_exc, conexion4); //traemos los datos de la hoja y las guardamos en un dataSdapter
			// creamos la instancia del objeto DataSet
			dataSet4 = new DataSet();
			//conexion3.Close();//cerramos la conexion
			dataAdapter4.Fill(dataSet4, "hoja_lz");//llenamos el dataset
			dataGridView5.DataSource = dataSet4.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
		}

        public void carga_excel_general_pagos(String archy){ 
		   
			/*OpenFileDialog dialog = new OpenFileDialog();
			
			dialog.Title = "Seleccione el archivo GENERAL DE PAGOS";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			
			
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{*/
				//archivo = dialog.FileName;
                archivo = archy;
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();
				Invoke(new MethodInvoker(delegate
					             {
				textBox1.AppendText("• "+archivo);
					//textBox1.Text= @"C:\Users\usuario\Desktop\DEPURACIONES\PROCESAR.csv";
				                         }));
					if((ext.Equals("xls"))||(ext.Equals("lsx"))){
						//esta cadena es para archivos excel 2007 y 2010
						cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
						conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
						conexion.Open(); //abrimos la conexion
						carga_chema_general_pagos();
					}
					 
				/*	label26.Visible=true;
					label26.Refresh();
					textBox2.Text="N/A";
			}*/
		}
		
		public void carga_excel_rale(){
			
			OpenFileDialog dialog = new OpenFileDialog();
			
			dialog.Title = "Seleccione el archivo RALE";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque

			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();
					
					textBox3.Text = archivo;
					//textBox1.Text= @"C:\Users\usuario\Desktop\DEPURACIONES\PROCESAR.csv";
					
					if((ext.Equals("xls"))||(ext.Equals("lsx"))){
						//esta cadena es para archivos excel 2007 y 2010
						cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + textBox3.Text + "';Extended Properties=Excel 12.0;";
						conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
						conexion4.Open(); //abrimos la conexion
						carga_chema_excel_rale();
					}
					 
					label28.Visible=true;
					label28.Refresh();
					
			}
		}
		
		public void carga_chema_excel(){
			i=0;
			filas = 0;
			comboBox1.Items.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			//System.Data.DataTable dt2 = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);
		    dataGridView2.DataSource = dt;
		    //dataGridView1.DataSource = dt2;
		    filas=(dataGridView2.RowCount)-1;
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
		
		public void carga_chema_excel2(){
			i=0;
			filas = 0;
			comboBox2.Items.Clear();
			System.Data.DataTable dt2 = conexion2.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
		    dataGridView2.DataSource =dt2;
		    filas=(dataGridView2.RowCount)-1;
					do{
						if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
							if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
								tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
								if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
									tabla = tabla.Remove((tabla.Length-1),1);
									comboBox2.Items.Add(tabla);
								}
							}
						}
						i++;
					}while(i<=filas);
					
                    dt2.Clear();
                    dataGridView2.DataSource = dt2; //vaciar datagrid
		}
		
		public void carga_chema_excel_rale(){
			i=0;
			filas = 0;
			comboBox4.Items.Clear();
			System.Data.DataTable dt2 = conexion4.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
		    dataGridView5.DataSource =dt2;
		    filas=(dataGridView5.RowCount)-1;
					do{
						if (!(dataGridView5.Rows[i].Cells[3].Value.ToString()).Equals("")){
							if ((dataGridView5.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
								tabla=dataGridView5.Rows[i].Cells[2].Value.ToString();
								if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
									tabla = tabla.Remove((tabla.Length-1),1);
									comboBox4.Items.Add(tabla);
								}
							}
						}
						i++;
					}while(i<=filas);
					
                    dt2.Clear();
                    dataGridView5.DataSource = dt2; //vaciar datagrid
		}
		
		public void carga_chema_general_pagos(){
			i=0;
			filas = 0;
			Invoke(new MethodInvoker(delegate
					             {
			comboBox1.Items.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			//System.Data.DataTable dt2 = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);
		    dataGridView2.DataSource = dt;
		    //dataGridView1.DataSource = dt2;
		    filas=(dataGridView2.RowCount)-1;
			                         }));
					do{
						Invoke(new MethodInvoker(delegate
					    {
						if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
							if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
								tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
								if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
									tabla = tabla.Remove((tabla.Length-1),1);
									comboBox1.Items.Add(tabla);
								}
							}
						}
				        }));
						i++;
					}while(i<=filas);
					Invoke(new MethodInvoker(delegate
					             {
                    //dt.Clear();
                    dataGridView2.DataSource = null; //vaciar datagrid
			                         }));
		}
		
		public void cargar_hoja_excel_procesar(){
			
				hoja = comboBox1.SelectedItem.ToString();
				
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select [NRP],[F#SUA],[FECHA PAGO],[4SS],[RCV] from [" + hoja + "$] where [PERIODO PAGO] like \""+periodo_sipare+"\" and [DIAG 1] <> 146 and [DIAG 1] <> 857";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					//tablarow3.Rows.Clear();
					//dataSet.Tables.Add(tablarow3);	
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						if (dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                            
						dataGridView3.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						data_acumulador.Merge(dataSet.Tables[0]);
						conexion.Close();//cerramos la conexion
						dataGridView3.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						 
					}
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n\n"+ex,"Error al Abrir Archivo de Excel");
				}
				
			}
			
		}
		
		public void cargar_hoja_excel_sipare(){
			    comboBox2.SelectedIndex=0;
				hoja = comboBox2.SelectedItem.ToString();
			
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select * from [" + hoja + "$]";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter2 = new OleDbDataAdapter(cons_exc, conexion2); //traemos los datos de la hoja y las guardamos en un dataSdapter
					dataSet2 = new DataSet(); // creamos la instancia del objeto DataSet
					if(dataAdapter2.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						dataAdapter2.Fill(dataSet2, hoja);//llenamos el dataset
						dataGridView4.DataSource = dataSet2.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						conexion2.Close();//cerramos la conexion
						dataGridView4.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
                        //dataGridView4.DataSource = tablarow;
                        periodo_sipare = dataGridView4.Rows[1].Cells[1].Value.ToString();
						//estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView4.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView4.ColumnCount);
                        i = 0;			

						               
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n"+ex,"Error al Abrir Archivo de Excel");
				}
				
			}
			
		}
		
		public void cargar_hoja_excel_general_pagos(int iter){
			Invoke(new MethodInvoker(delegate
					             {	 
			comboBox1.SelectedIndex=0;
			hoja = comboBox1.SelectedItem.ToString();
			
			                         }));
			int tot_filas_dgv3=0;
			
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select [REGISTRO],[RC_NUM_FOL],[RC_FEC_MOV],[RC_IMP_TOT],[RC_PER] from [" + hoja + "$] where [RC_PER] like \""+per_sel+"\" and ([RC_MOD] = 10 or [RC_MOD] = 13 or [RC_MOD] = 17) and ([RC_DOC] = 1 or [RC_DOC] = 2)";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					//tablarow3.Rows.Clear();
					//dataSet.Tables.Add(tablarow3);	
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						if (dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
						
						//dataGridView3.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet	
						//tot_filas_dgv3=dataGridView3.RowCount;
						Invoke(new MethodInvoker(delegate
					             {
						label22.Text="Leyendo archivo: "+iter+" de GENERAL DE PAGOS";
						label22.Refresh();
						                         }));
						tablarow3.Rows.Clear();
						tablarow3 = dataSet.Tables[0];
						tot_filas_dgv3=tablarow3.Rows.Count;
						//MessageBox.Show("ya cargo la consulta");						
						//conexion.Dispose();//cerramos la conexion
						}
						
						/*p=0;
						y=0;
						gp_count=0;
						tablarow3.Rows.Clear();*/
		
                        if(tot_filas_dgv3>0){
							/*do{
									Invoke(new MethodInvoker(delegate
						             {
								nrp=dataGridView3.Rows[p].Cells[0].FormattedValue.ToString();
								f_sua=dataGridView3.Rows[p].Cells[1].FormattedValue.ToString();
								fecha_pago=dataGridView3.Rows[p].Cells[2].FormattedValue.ToString();
								i4ss=dataGridView3.Rows[p].Cells[3].FormattedValue.ToString();
								rcv_sip="";
									                         }));
									
								
								if(((Convert.ToDouble(i4ss))>1)){
									
									if((nrp.Length<=10)&&(nrp.Substring(0,1).Equals("4"))){
										nrp="0"+nrp;
									}
	
	                                nrp = nrp.Substring(0, 10);
									tablarow3.Rows.Add(nrp,f_sua,fecha_pago,i4ss,rcv_sip);
									y++;
									
								}
								p++;	
								gp_count++;
								
								Invoke(new MethodInvoker(delegate
						             {
								if(gp_count==400){
									label22.Text="Leyendo archivo: "+iter+" de GENERAL DE PAGOS";
									label22.Refresh();
									gp_count=0;
								}
								
								if(gp_count==100){
									label22.Text="Leyendo archivo: "+iter+" de GENERAL DE PAGOS.";
									label22.Refresh();
								}
								
								if(gp_count==200){
									label22.Text="Leyendo archivo: "+iter+" de GENERAL DE PAGOS..";
									label22.Refresh();
								}
								
								if(gp_count==300){
									label22.Text="Leyendo archivo: "+iter+" de GENERAL DE PAGOS...";
									label22.Refresh();							
								}
								                         }));
								
							}while(p<tot_filas_dgv3);*/
							
							data_acumulador.Merge(tablarow3);
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
		
		public void cargar_hoja_excel_rale(){
			Invoke(new MethodInvoker(delegate
					                         {
			comboBox4.SelectedIndex=0;
            hoja = "";
			hoja = comboBox4.SelectedItem.ToString();
				
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select [REG# PAT#],[IMPORTE] from [" + hoja + "$] where [PERIODO] like \""+(per_sel.Substring(5,2)+"/"+per_sel.Substring(0,4))+"\" and [TD] =\"02\"";
				//MessageBox.Show(cons_exc+"error");
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter4 = new OleDbDataAdapter(cons_exc, conexion4); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet4 = new DataSet(); // creamos la instancia del objeto DataSet
					//tablarow3.Rows.Clear();
					//dataSet.Tables.Add(tablarow3);	
					if(dataAdapter4.Equals(null)){
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
					}else{
						if(dataAdapter4 == null){}else{
							dataAdapter4.Fill(dataSet4, hoja);//llenamos el dataset
							dataGridView5.DataSource = dataSet4.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
							conexion4.Close();//cerramos la conexion
						}
						p=0;
						yy=0;
						gp_count=0;
						data_rale.Rows.Clear();
						label22.Text="Leyendo RALE";
						label22.Refresh();
						do{
							nrp=dataGridView5.Rows[p].Cells[0].FormattedValue.ToString();
							i4ss=dataGridView5.Rows[p].Cells[1].FormattedValue.ToString();
							
							if(((Convert.ToDouble(i4ss))>1)){
								if((nrp.Length==11)&&(nrp.StartsWith("4"))){
									nrp="0"+nrp;
								}
								nrp=nrp.Substring(0,3)+nrp.Substring(4,5)+nrp.Substring(10,2);
								data_rale.Rows.Add(nrp,i4ss);
								yy++;
							}
							p++;	
							gp_count++;
							
							if(gp_count==40){
								label22.Text="Leyendo RALE";
								label22.Refresh();
								gp_count=0;
							}
							
							if(gp_count==10){
								label22.Text="Leyendo RALE.";
								label22.Refresh();
							}
							
							if(gp_count==20){
								label22.Text="Leyendo RALE..";
								label22.Refresh();
							}
							
							if(gp_count==30){
								label22.Text="Leyendo RALE...";
								label22.Refresh();							
							}
							
						}while(p<dataGridView5.RowCount);
						
						label22.Text="Creando datos temporales del RALE...";
						label22.Refresh();
						//guardar_excel_temp();
						XLWorkbook wb = new XLWorkbook();	
						wb.Worksheets.Add(data_rale,"hoja_lz");
						wb.SaveAs(@"temporal_rale.xlsx");
						
						label22.Text=" Lectura del archivo RALE finalizada!.";
						label22.Refresh();
						MessageBox.Show("Se recopilaron:\n"+yy+" registros del archivo RALE\n\n Da click en aceptar para continuar. ","Información");
						panel3.Visible=true;
						panel2.Visible=false;
						label22.Visible=false;
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n"+ex,"Error al Abrir Archivo de Excel");
				}
				
			}
			                         }));
		}
		
		public void cargar_archivos_general_pagos(){
            Invoke(new MethodInvoker(delegate
                                         {
                                             button4.Enabled = false;
                                             button5.Enabled = false;
                                             panel2.Enabled = false; 
                                             panel12.Enabled=false;
                                         }));
			int xy=0,tot_filas_da=0,cerrar=0;
			data_acumulador.Rows.Clear();
			xy=0;
			
			do{
				carga_excel_general_pagos(archivos_gp[xy]);
				cargar_hoja_excel_general_pagos((xy+1));
				xy++;
			}while(xy<archivos_gp.Length);
			
			p=0;
			gp_count=0;
			tot_filas_da=data_acumulador.Rows.Count;
            tablarow3.Columns.Clear();
            tablarow3.Rows.Clear();
            tablarow3.Columns.Add("NRP", tipo_cadena);
            tablarow3.Columns.Add("F#SUA", tipo_cadena);
            tablarow3.Columns.Add("FECHA PAGO", tipo_cadena);
            tablarow3.Columns.Add("4SS",porcentaje_min_pago.GetType());
            tablarow3.Columns.Add("RCV", porcentaje_min_pago.GetType());
            tablarow3.Columns.Add("PERIODO_PAGO", tipo_cadena);
            tablarow3.Columns.Add("ARCHIVO_ORIGEN", tipo_cadena);

            //MessageBox.Show("fecha_pago = " + data_acumulador.Rows[p][2].ToString().Substring(0, 10) + " 4ss =" + data_acumulador.Rows[p][3].ToString()+" tipo col="+tablarow3.Columns[3].DataType.ToString());
			while(p<tot_filas_da){
				Invoke(new MethodInvoker(delegate
				                         {
				                         	nrp=data_acumulador.Rows[p][0].ToString();
				                         	f_sua=data_acumulador.Rows[p][1].ToString();
				                         	fecha_pago=data_acumulador.Rows[p][2].ToString().Substring(0,10);
				                         	i4ss=data_acumulador.Rows[p][3].ToString();
				                         	per_pag_gp=data_acumulador.Rows[p][4].ToString();
				                         	rcv_sip="0.00";
				                         }));
				
				if(((Convert.ToDouble(i4ss))>1)){
					
					if((nrp.Length<=10)&&(nrp.Substring(0,1).Equals("4"))){
						nrp="0"+nrp;
					}
					
					nrp = nrp.Substring(0, 10);
					tablarow3.Rows.Add(nrp,f_sua,fecha_pago,Convert.ToDouble(i4ss),Convert.ToDouble(rcv_sip),per_pag_gp);
					y++;
				}
				p++;
				
				Invoke(new MethodInvoker(delegate
				                         {
				                         label22.Text="Analizando datos extraídos de de GENERAL DE PAGOS..."+p+" de "+tot_filas_da ;
				                         label22.Refresh();
				                         }));
				
			}
			
			Invoke(new MethodInvoker(delegate
			                         {
			                         	label22.Text="Creando datos temporales...";
			                         	label22.Refresh();
			                         	//guardar_excel_temp();
			                         	XLWorkbook wb = new XLWorkbook();
			                         	wb.Worksheets.Add(tablarow3,"hoja_lz");
			                         	wb.SaveAs(@"temporal.xlsx");
			                         }));
			
			Invoke(new MethodInvoker(delegate
			                         {
			                         	label22.Text="Listo! Se completo la lectura del (los) archivo(s) GENERAL DE PAGOS";
			                         	label22.Refresh();
			                         	MessageBox.Show("Se recopilaron:\n"+tablarow3.Rows.Count+" registros de "+archivos_gp.Length+" archivos de GENERAL DE PAGOS\n\n Da click en aceptar para continuar. ","Información");
			                         	panel3.Visible=true;
			                         	panel2.Visible=false;
			                         	label22.Visible=false;
                                        button4.Enabled = true;
                                        button5.Enabled = true;
                                        panel2.Enabled = false; 
                                        panel12.Enabled=false;
                                        
                                        try {
                                        	cargar_depu_man();
                                        } catch (Exception de) {
                                        	MessageBox.Show("Imposible continuar con la depuración.","AVISO");
                                        	button8.Enabled = false;
                                        	button6.Enabled = false;
                                        	button7.Enabled = false;
                                        	button22.Enabled = false;
                                        	button23.Enabled = false;
                                        	cerrar=1;
                                        	
                                        }
			                         	
			                         }));
            if(cerrar==1){
            	this.Hide();
            }
			
		}
		
		public void procesar_rale(){
			
			//cargar RALE
			cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
			conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
			conexion4.Open(); //abrimos la conexion
			cons_exc = "Select * from [hoja_lz$] ";
			carga_excel3(); //Cargar RALE carga en dg5
			
			//cargar temporal GENERAL o PROSIP
			label23.Text="Buscando Pagos";
			label23.Refresh();
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal.xlsx';Extended Properties=Excel 12.0;";
			conexion3 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion3.Open(); //abrimos la conexion
			
			m=0;
			p=0;
			cont_wait=0;
			dgrc2 = dataGridView5.RowCount;
			tablarow3.Rows.Clear();
			
			do{
				nrp_sql = dataGridView5.Rows[p].Cells[0].Value.ToString();
				cons_exc2 = "Select [NRP],[F#SUA],[FECHA PAGO],[4SS] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\" and [4SS] > \"0\"";
				carga_excel2(); //carga en datagridview1
				dgrc1 = dataGridView1.RowCount;
				m=0;
				importe_pago=0;
				f_sua_acu="";
				fecha_pago_acu="";
				importe_pago=0;
				
				ingresados = new string[dgrc1];
				
				if(dgrc1>0){
					do{
						nrp=dataGridView1.Rows[m].Cells[0].Value.ToString();
						f_sua=dataGridView1.Rows[m].Cells[1].Value.ToString();
						fecha_pago=dataGridView1.Rows[m].Cells[2].Value.ToString();
						i4ss=dataGridView1.Rows[m].Cells[3].Value.ToString();
						
						comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago + "_" + i4ss;
						acumular=0;
						
						if(fecha_pago.Length<2){
							fecha_pago="";
						}
						
						if (m != 0){
							for (i = 0; i < ingresados.Length; i++){
								if (ingresados[i] != null){
									if (ingresados[i].Equals(comp_dupli)){
										//i4ss = "0.00";
										acumular=1;
										dupli_en++;
									}else{
										acumular=0;
									}
								}
							}
						}
						
						if(acumular==0){
							importe_pago = importe_pago + (Convert.ToDouble(i4ss));
							
							if(f_sua_acu.Length==0){
								f_sua_acu = f_sua;
							}else{
								f_sua_acu = f_sua_acu + " - " + f_sua;
							}
							
							if(fecha_pago_acu.Length==0){
								fecha_pago_acu = fecha_pago;
							}else{
								fecha_pago_acu = fecha_pago_acu + " - " + fecha_pago;
							}
							
							//acumular = 0;
							ingresados[m] = comp_dupli;
						}
						
						m++;
					}while(m < dgrc1);
					
					tablarow3.Rows.Add(nrp,f_sua_acu,fecha_pago_acu,importe_pago);
				}
				
				cont_wait++;
				if (cont_wait == 10)
				{
					label23.Text = "Buscando Pagos.";
					label23.Refresh();
				}
				if (cont_wait == 20)
				{
					label23.Text = "Buscando Pagos..";
					label23.Refresh();
				}
				if (cont_wait == 30)
				{
					label23.Text = "Buscando Pagos...";
					label23.Refresh();
				}
				if (cont_wait == 40)
				{
					label23.Text = "Buscando Pagos";
					label23.Refresh();
					cont_wait = 0;
				}
				
				p++;
			}while( p < dgrc2);
			
			SaveFileDialog fichero = new SaveFileDialog();
			fichero.Title = "Guardar archivo de Resultados del RALE";
			fichero.Filter = "Archivo Excel (*.XLSX)|*.xlsx";
			
			if(fichero.ShowDialog() == DialogResult.OK){}
			//System.IO.File.Delete(@"temporal.xlsx");
			label22.Text="Creando datos temporales...";
			label22.Refresh();
			//guardar_excel_temp();
			XLWorkbook wb = new XLWorkbook();	
			wb.Worksheets.Add(tablarow3,"resultados_rale");
			wb.SaveAs(@fichero.FileName);
			
			MessageBox.Show("Listo! Depuracion de RALE Terminada");
			
		}
		
		public void cargar_csv(){
			StreamReader rdr;
			tablarow.Columns.Clear();
			tablarow.Rows.Clear();
			if(tipo_arch==1){
				rdr = new StreamReader(textBox1.Text);
			}else{
				rdr = new StreamReader(textBox2.Text);
			}
			i=0;
			
			do{
				if(i==0){
					csv_line = rdr.ReadLine();
					csv_pre = csv_line.Split(',');
					for(j=0;j<csv_pre.Length;j++){
						if(tipo_arch==1){
							dataGridView3.Columns.Add(csv_pre[j].Substring(1,csv_pre[j].Length-2),csv_pre[j].Substring(1,csv_pre[j].Length-2));
						}else{
							//dataGridView4.Columns.Add(csv_pre[j].Substring(1,csv_pre[j].Length-2),csv_pre[j].Substring(1,csv_pre[j].Length-2));	
						}
						tablarow.Columns.Add(csv_pre[j].Substring(1,csv_pre[j].Length-2));
					}
					
				}else{
					csv_line = rdr.ReadLine();
					csv_line = csv_line.Replace('"','~');
					csv_line_copy="";
					n=0;
					do{
						
						if(csv_line.Substring(n,1).Equals("~")){}else{
							csv_line_copy+=csv_line.Substring(n,1);
						}
						n++;
						//MessageBox.Show(csv_line_copy);
					}while(n<csv_line.Length-1);
					csv_pre = csv_line_copy.Split(',');
					if(tipo_arch==1){
						dataGridView3.Rows.Add(csv_pre);
					}else{
						//dataGridView4.Rows.Add(csv_pre);
					}
					tablarow.Rows.Add(csv_pre);
				}

				i++;
			}while(!(rdr.EndOfStream));
			
			dataGridView4.DataSource=tablarow;
			periodo_sipare= dataGridView4.Rows[0].Cells[1].Value.ToString();
			
		}
		
		public void confirmar_datos(){
			
			if(radioButton1.Checked==true){
				textBox3.Text = radioButton1.Text;
			}
			
			if(radioButton2.Checked==true){
                textBox3.Text = radioButton2.Text;
			}
			
			/*if(radioButton3.Checked==true){
                textBox3.Text = "Depuración de " + radioButton3.Text;
			}*/
			textBox4.Text="";
			if (arch_sel==1){
				textBox4.AppendText("PROCESAR: "+arch_pro+"\n");
				textBox4.AppendText("SIPARE: "+arch_sip+"\n");// textBox1.Text;
			}else{
				textBox4.AppendText("GENERAL DE PAGOS: "+archs_gp_tb);
			}
            //textBox5.Text = ;//textBox2.Text;

            textBox12.Text = fecha_not_dep; 

			if(tipo_doc_sel==2){
				textBox5.Text = "02";
            }else{
            	if(tipo_doc_sel==1){
		            if(tipo_cre==1){
						textBox5.Text = "80";
					}else{
		                textBox5.Text = "81";
					}
            	}else{
            		if(tipo_doc_sel==3){
            			textBox5.Text = "06";
            		}
            	}
            }
            
 			data_3.Rows.Add("primer_boton",valor_tomado1);
 			data_3.Rows.Add("segundo_boton",valor_tomado2);
 			data_3.Rows.Add("tercer_boton",valor_tomado3);
 			data_3.Rows.Add("cuarto_boton",valor_tomado4);
 			dataGridView1.DataSource=data_3;
 			
 			dataGridView1.Sort(dataGridView1.Columns[1],System.ComponentModel.ListSortDirection.Ascending);
 			
 			periodo = new string[4];
 			
 			if(Convert.ToInt32(dataGridView1.Rows[0].Cells[1].Value.ToString()) != 0){
 				switch(dataGridView1.Rows[0].Cells[0].Value.ToString()){
 						case "primer_boton": periodo[0]=label14.Text;
 						break;
 						case "segundo_boton": periodo[0]=label16.Text;
 						break;
 						case "tercer_boton": periodo[0]=label18.Text;
 						break;
 						case "cuarto_boton": periodo[0]=label20.Text;
 						break;
 				}
 			}
 			
 			if(Convert.ToInt32(dataGridView1.Rows[1].Cells[1].Value.ToString()) != 0){
 				switch(dataGridView1.Rows[1].Cells[0].Value.ToString()){
 						case "primer_boton": periodo[1]=label14.Text;
 						break;
 						case "segundo_boton": periodo[1]=label16.Text;
 						break;
 						case "tercer_boton": periodo[1]=label18.Text;
 						break;
 						case "cuarto_boton": periodo[1]=label20.Text;
 						break;
 				}
 			}
 			
 			if(Convert.ToInt32(dataGridView1.Rows[2].Cells[1].Value.ToString()) != 0){
 				switch(dataGridView1.Rows[2].Cells[0].Value.ToString()){
 						case "primer_boton": periodo[2]=label14.Text;
 						break;
 						case "segundo_boton": periodo[2]=label16.Text;
 						break;
 						case "tercer_boton": periodo[2]=label18.Text;
 						break;
 						case "cuarto_boton": periodo[2]=label20.Text;
 						break;
 				}
 			}
 			
 			if(Convert.ToInt32(dataGridView1.Rows[3].Cells[1].Value.ToString()) != 0){
 				switch(dataGridView1.Rows[3].Cells[0].Value.ToString()){
 						case "primer_boton": periodo[3]=label14.Text;
 						break;
 						case "segundo_boton": periodo[3]=label16.Text;
 						break;
 						case "tercer_boton": periodo[3]=label18.Text;
 						break;
 						case "cuarto_boton": periodo[3]=label20.Text;
 						break;
 				}
 			}
 			l=0;
 			for(k=0;k<4;k++){
 				if((periodo[k] == null)){}else{
 					l++;
 					textBox6.AppendText(l+".- "+periodo[k]+"\n");
 				}
 				
 				//textBox6.AppendText(""+valor_tomado1+","+valor_tomado2+","+valor_tomado3+","+valor_tomado4+"");
 				/*textBox6.AppendText("2.- "+periodo[1]+"\n");
 				textBox6.AppendText("3.- "+periodo[2]+"\n");
 				textBox6.AppendText("4.- "+periodo[3]+"\n");*/
 				
 			}
 			data_3.Rows.Clear();
		}
		
		public void combinar_procesar_sipare(){
		
			Invoke(new MethodInvoker(delegate
					                         {
			
			label22.Text="Leyendo SIPARE...";
			label22.Refresh();
			if(tipo_sipare==1){
				cargar_hoja_excel_sipare();
			}else{
				cargar_csv();
			}
			//periodo_sipare = (per_sel.Substring(0,4))+(per_sel.Substring(5,2));
			//MessageBox.Show("|"+periodo_sipare+"|");
			label22.Text="Leyendo PROCESAR...";
			label22.Refresh();
			dataGridView3.DataSource=data_vacio;
			data_acumulador.Columns.Clear();
			data_acumulador.Rows.Clear();
			m=0;
			do{
				comboBox1.SelectedIndex = m;
				cargar_hoja_excel_procesar();
			
				if((m >(comboBox1.Items.Count/3))&&(m <((comboBox1.Items.Count/3)*2))){
					label22.Text="Filtrando Tablas PROCESAR...";
					label22.Refresh();
					
				}
				if(m >= ((comboBox1.Items.Count/3)*2)){
						label22.Text="Fusionando Tablas PROCESAR...";
						label22.Refresh();
					}
				
				m++;
			}while(m<comboBox1.Items.Count);
			
			label22.Text="Compactando SIPARE...";
			label22.Refresh();
			tablarow2.Rows.Clear();
			tablarow3.Rows.Clear();
			dataGridView3.DataSource=data_acumulador;
			p=0;
			x=0;
			while(p<dataGridView4.RowCount){
				nrp=dataGridView4.Rows[p].Cells[0].FormattedValue.ToString();
				f_sua=dataGridView4.Rows[p].Cells[2].FormattedValue.ToString();
				//fecha_pago=dataGridView4.Rows[p].Cells[].FormattedValue.ToString();
				i4ss=dataGridView4.Rows[p].Cells[10].FormattedValue.ToString();
				rcv_sip=dataGridView4.Rows[p].Cells[11].FormattedValue.ToString();
				if ((dataGridView4.Rows[p].Cells[1].FormattedValue.ToString()).Equals(periodo_sipare)){
					if(((Convert.ToDouble(i4ss))>1)||((Convert.ToDouble(rcv_sip))>1)){
						
						if((nrp.Length==10)&&(nrp.Substring(0,1).Equals("4"))){
							nrp="0"+nrp;
						}
						nrp=nrp.Substring(0,10);
						tablarow2.Rows.Add(nrp,f_sua,"-",i4ss,rcv_sip);
						x++;
						
					}
				}
				p++;
			}
			
			p=0;
			y=0;
			label22.Text="Compactando PROCESAR...";
			label22.Refresh();
			while(p<dataGridView3.RowCount){
				nrp=dataGridView3.Rows[p].Cells[0].FormattedValue.ToString();
				f_sua=dataGridView3.Rows[p].Cells[1].FormattedValue.ToString();
				fecha_pago=dataGridView3.Rows[p].Cells[2].FormattedValue.ToString();
				i4ss=dataGridView3.Rows[p].Cells[3].FormattedValue.ToString();
				rcv_sip=dataGridView3.Rows[p].Cells[4].FormattedValue.ToString();
				
				if(((Convert.ToDouble(i4ss))>1)||((Convert.ToDouble(rcv_sip))>1)){
					
					if((nrp.Length==10)&&(nrp.Substring(0,1).Equals("4"))){
						nrp="0"+nrp;
					}
					nrp=nrp.Substring(0,10);
					tablarow3.Rows.Add(nrp,f_sua,fecha_pago,i4ss,rcv_sip);
					y++;
				}
				p++;	
			}
			
			label22.Text="Combinando PROCESAR y SIPARE...";
			label22.Refresh();
			tablarow3.Merge(tablarow2);
			dataGridView1.DataSource=tablarow3;
			//estilo datagrid
			i = 0;
			
			while (i < dataGridView1.ColumnCount){
				dataGridView1.Columns[i].HeaderText.ToUpper();
				i++;
			}
			
			i = 0;

			//System.IO.File.Delete(@"temporal.xlsx");
			label22.Text="Creando datos temporales...";
			label22.Refresh();
			//guardar_excel_temp();
			XLWorkbook wb = new XLWorkbook();	
			wb.Worksheets.Add(tablarow3,"hoja_lz");
			wb.SaveAs(@"temporal.xlsx");
			
			
			label22.Text="Listo! Puedes Continuar al Siguiente Paso";
			label22.Refresh();
			MessageBox.Show("Se recopilaron:\n"+x+" registros del archivo SIPARE\n"+y+" registros del archivo PROCESAR\n\n Da click en aceptar para continuar. ","Información");
			panel3.Visible=true;
			panel2.Visible=false;
			label22.Visible=false;

			cargar_depu_man();
            
			                         }));
				
		}
 		
		public void lanzador_COP_EST(){
			
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"COP_ECO_EST_"+periodo_sipare+"\"";
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_eco_est = dataGridView3.RowCount;
			
			textBox7.Text="";
			//textBox8.Text="";
			hiloterciario = new Thread(new ThreadStart(consultar_COP_EST));
			hiloterciario.Start();
			//consultar_COP_EST();
			
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: COP_ECO_EST_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel6.Enabled=false;
			}
		}
		
		public void lanzador_COP_MEC(){
			//data_vacio.Rows.Clear();
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"COP_ECO_MEC_"+periodo_sipare+"\"";
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_eco_mec = dataGridView3.RowCount;
		
			//textBox7.Text="";
			textBox8.Text="";
			hilotetrario = new Thread(new ThreadStart(consultar_COP_MEC));
			hilotetrario.Start();
			//consultar_COP_MEC();
			
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: COP_ECO_MEC_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel7.Enabled=false;
			}
			
		}
		
		public void lanzador_RCV_EST(){
			textBox9.Text="";
			//textBox10.Text="";
			
			if(tipo_dep==1){
				periodo_sipare_rcv=periodo_sipare.Remove(periodo_sipare.Length-2,2);
				if(mes_periodo<=9){
					periodo_sipare_rcv=periodo_sipare_rcv+"0"+mes_periodo.ToString();
				}else{
					periodo_sipare_rcv=periodo_sipare_rcv+mes_periodo.ToString();
				}
			}
			//MessageBox.Show(periodo_sipare_rcv);
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"RCV_ECO_EST_"+periodo_sipare_rcv+"\"";
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_rcv_est = dataGridView3.RowCount;
			
			hiloterciario = new Thread(new ThreadStart(consultar_RCV_EST));
			hiloterciario.Start();
			//consultar_COP_EST();
			
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: RCV_ECO_EST_" + periodo_sipare_rcv + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel8.Enabled=false;
			}
		}
		
		public void lanzador_RCV_MEC(){
			if(tipo_dep==1){
				periodo_sipare_rcv=periodo_sipare.Remove(periodo_sipare.Length-2,2);
				if(mes_periodo<=9){
					periodo_sipare_rcv=periodo_sipare_rcv+"0"+mes_periodo.ToString();
				}else{
					periodo_sipare_rcv=periodo_sipare_rcv+mes_periodo.ToString();
				}
			}
			textBox10.Text="";
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"RCV_ECO_MEC_"+periodo_sipare_rcv+"\"";
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_rcv_mec = dataGridView3.RowCount;
			
			hilotetrario = new Thread(new ThreadStart(consultar_RCV_MEC));
			hilotetrario.Start();
			//consultar_COP_MEC();
			
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: RCV_ECO_MEC_" + periodo_sipare_rcv + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel9.Enabled=false;
			}
		}
		
		public void consultar_COP(){
			lanzador_COP_EST();
			lanzador_COP_MEC();
		}
		
		public void consultar_RCV(){

			lanzador_RCV_EST();
			lanzador_RCV_MEC();
			
		}
		
		public void consultar_doble_COP_RCV(){
			
			lanzador_COP_EST();
			lanzador_COP_MEC();
			lanzador_RCV_EST();
			lanzador_RCV_MEC();
			/*textBox7.Text="";
			textBox8.Text="";
			
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"COP_ECO_EST_"+periodo_sipare+"\"";
			//MessageBox.Show(sql);
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_eco_est = dataGridView3.RowCount;
			
			hiloterciario = new Thread(new ThreadStart(consultar_COP_EST));
			hiloterciario.Start();
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: COP_ECO_EST_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel6.Enabled=false;
			}
			
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"COP_ECO_MEC_"+periodo_sipare+"\"";
			//MessageBox.Show(sql);
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_eco_mec = dataGridView3.RowCount;
			
			hilotetrario = new Thread(new ThreadStart(consultar_COP_MEC));
			hilotetrario.Start();
			//consultar_COP_MEC();
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: COP_ECO_MEC_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel7.Enabled=false;
			}
			
			textBox9.Text="";
			textBox10.Text="";
			periodo_sipare_rcv=periodo_sipare.Remove(periodo_sipare.Length-2,2);
			if(mes_periodo<=9){
		    	periodo_sipare_rcv=periodo_sipare_rcv+"0"+mes_periodo.ToString();
			}else{
				periodo_sipare_rcv=periodo_sipare_rcv+mes_periodo.ToString();
			}
			
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"RCV_ECO_EST_"+periodo_sipare_rcv+"\"";
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_rcv_est = dataGridView3.RowCount;
			
			hiloterciario = new Thread(new ThreadStart(consultar_RCV_EST));
			hiloterciario.Start();
			//consultar_COP_EST();
			
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: RCV_ECO_EST_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel8.Enabled=false;
			}
			
			sql="SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo=\"RCV_ECO_MEC_"+periodo_sipare_rcv+"\"";
			data_vacio=conex.consultar(sql);
			dataGridView3.DataSource=data_vacio;
			tot_per_rcv_mec = dataGridView3.RowCount;
			hilotetrario = new Thread(new ThreadStart(consultar_RCV_MEC));
			hilotetrario.Start();
			//consultar_COP_MEC();
			
			if(error_vacio==1){
				MessageBox.Show("La Búsqueda del Periodo: RCV_ECO_MEC_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				error_vacio=0;
				panel9.Enabled=false;
			}*/
		}
		
		public void consultar_COP_EST(){
			
			Invoke(new MethodInvoker(delegate
					                         {
			label23.Text="COP EST: Consultando Base de Datos...";
			label23.Refresh();
			                         
			sql="SELECT registro_patronal2,folio_sipare_sua,fecha_pago,importe_cuota,id,status_credito,importe_pago,num_pago,porcentaje_pago,credito_multa,importe_multa,tipo_documento,subdelegacion,status,credito_cuotas FROM datos_factura WHERE "+
				"nombre_periodo = \"COP_ECO_EST_"+periodo_sipare+"\" AND (status = \"EN TRAMITE\" OR status = \"0\") ORDER BY registro_patronal2,credito_multa";
			data_cop=conex.consultar(sql);
			
			dataGridView2.DataSource=data_cop; 
			m=0;
			p=0;
			x=0;
			y=0;
			tot_cop_est=0;
			duplicados=0;
			cont_wait=0;
			omitidos=0;
			tot_pag_cop_est=0;
			tot_no_depurar=0;
			error_vacio=0;
			tot_menores=0;
			porcentaje_sql=0;
			dupli_en=0;
			
			//cargar temporal GENERAL o PROSIP
			label23.Text="COP EST: Leyendo Datos Temporales...";
			label23.Refresh();  
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal.xlsx';Extended Properties=Excel 12.0;";
			conexion3 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion3.Open(); //abrimos la conexion
			
			if(tipo_doc_sel>=2){
				//cargar RALE
				cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
				conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
				conexion4.Open(); //abrimos la conexion
			}
			
			label23.Text="COP EST: Analizando Datos";
			label23.Refresh();
			dgrc2= dataGridView2.RowCount;                          
			                         
            if (dgrc2 > 0)
            {
                do
                {
                	
                    nrp_sql = dataGridView2.Rows[p].Cells[0].FormattedValue.ToString();
                    f_sua_sql = dataGridView2.Rows[p].Cells[1].FormattedValue.ToString();
                    fecha_pago_sql = dataGridView2.Rows[p].Cells[2].FormattedValue.ToString();
                    i4ss_sql = dataGridView2.Rows[p].Cells[3].FormattedValue.ToString();
                    id_sql = dataGridView2.Rows[p].Cells[4].FormattedValue.ToString();
                    stat_cred_sql = dataGridView2.Rows[p].Cells[5].FormattedValue.ToString();
                    importe_pago_sql = Convert.ToDouble(dataGridView2.Rows[p].Cells[6].FormattedValue.ToString());
                    num_pago = Convert.ToInt32(dataGridView2.Rows[p].Cells[7].FormattedValue.ToString());
                    porcentaje_sql = Convert.ToDouble(dataGridView2.Rows[p].Cells[8].FormattedValue.ToString());
					cre_sql = dataGridView2.Rows[p].Cells[9].FormattedValue.ToString();
					imp_mul_sql = dataGridView2.Rows[p].Cells[10].FormattedValue.ToString();
					td_sql = dataGridView2.Rows[p].Cells[11].FormattedValue.ToString();
					sub_sql = dataGridView2.Rows[p].Cells[12].FormattedValue.ToString();
					stat_sql = dataGridView2.Rows[p].Cells[13].FormattedValue.ToString();	
					cre_cuo_sql	= dataGridView2.Rows[p].Cells[14].FormattedValue.ToString();								
					
                    cons_exc2 = "Select [NRP],[F#SUA],[FECHA PAGO],[4SS] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\" and [4SS] > 0 order by [F#SUA]";
                    carga_excel2(); //carga en datagridview1
                    separar_sipare();
                    
                    dgrc1= dataGridView1.RowCount;                        	
                    ingresados = new string[dgrc1];
                    
                    if(tipo_doc_sel>=2){
                    	
                    	cons_exc = "Select [NRP] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"";
                        //cons_exc = "Select * from [hoja_lz$] ";
	            		carga_excel3(); //registro existe en el RALE??
	            		stat_sql=stat_cred_sql;
                        if (dataGridView5.RowCount == 0)
                        {
                            dgrc1 = 0;
                        }
                        else
                        {
                            //MessageBox.Show("lineas del rale/carga_manu: " + dataGridView5.RowCount);
                        }
                    }
                          
                    m = 0;
                    importe_pago=0;
                    porcentaje_min_pago=0;
                    fecha_pago_acu="";
                    f_sua_acu="";
                    acumular=0;
                    counpri=0;
                    importe_pago=importe_pago_sql;
                    
                    if (f_sua_sql.Length > 1)
                    {
                        f_sua_acu = f_sua_sql;
                    }
                    else
                    {
                        f_sua_acu = "";
                    }

                    if (fecha_pago_sql.Length > 1)
                    {
                        if(fecha_pago_sql.Equals("-")){
                    	   	fecha_pago_acu= "";
                    	}else{
                        	fecha_pago_acu = fecha_pago_sql;
                    	}
                    }
                    else
                    {
                        fecha_pago_acu = "";
                    }
                    
                    if (dgrc1 > 0)
                    {
                    	do
                    	{ 
                    		nrp = dataGridView1.Rows[m].Cells[0].FormattedValue.ToString();
                    		f_sua = dataGridView1.Rows[m].Cells[1].FormattedValue.ToString();
                    		fecha_pago = dataGridView1.Rows[m].Cells[2].FormattedValue.ToString();
                    		i4ss = dataGridView1.Rows[m].Cells[3].FormattedValue.ToString();
                    		          
                    		comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago + "_" + i4ss;
							acumular=0;
							
							if(fecha_pago.Length<2){
                    			fecha_pago="";
                    		}
							
                    		if (m != 0){
                    			for (i = 0; i < ingresados.Length; i++){
                    				if (ingresados[i] != null){
                    					if (ingresados[i].Equals(comp_dupli)){
                    						//i4ss = "0.00";
                    						acumular=1;
                    						dupli_en++;
                    					}else{
                    						acumular=0;
                    					}
                    				}
                    			}
                    		}

                            x = 0;
                            if (f_sua_sql.Length > 1){
                            	checar_folio = f_sua_sql.Split(' ');
                            	for (y = 0; y < checar_folio.Length; y++){
                                    if(f_sua.Length>checar_folio[y].Length){
                            		    if (checar_folio[y].Equals(f_sua.Substring(0, checar_folio[y].Length)))
                                        {
                            			    x = 1;
                            		    }
                                    }else{
                                        if ((checar_folio[y].Substring(0,f_sua.Length)).Equals(f_sua))
                                        {
                                            x = 1;
                                        }
                            	    }	
                                }
                            }else{
                            	if((porcentaje_sql>0)||(fecha_pago_sql.Length>1)||(importe_pago_sql>0)){
                            	x=2;
                            	}
                            }
                            
                            if((acumular==0)&&(x==0)){
                            	x=3;
                            }
                    		
                            if((x==3)){
                    			importe_pago=importe_pago+(Convert.ToDouble(i4ss));
                                porcentaje_min_pago = porcentaje_min_pago + ((Convert.ToDouble(i4ss) * 100) / (Convert.ToDouble(i4ss_sql)));
                    			if(f_sua_acu.Length==0){
                                	f_sua_acu = f_sua;
                                }else{
                                	f_sua_acu = f_sua_acu + " - " + f_sua;
                                }
                                if(fecha_pago_acu.Length==0){
                                	fecha_pago_acu = fecha_pago;
                                }else{
                                	fecha_pago_acu = fecha_pago_acu + " - " + fecha_pago;
                                }
                    			//acumular=0;
                    			ingresados[m] = comp_dupli;
                                num_pago = num_pago + 1;
                                if(counpri==0){
                                	if(f_sua.Length>5){
                                		folio_pago=f_sua.Substring(0,6);
                                	}else{
                                		folio_pago=f_sua.Substring(0,f_sua.Length);
                                	}
                                	fecha_pripago = fecha_pago;
                                	counpri=1;
                                }
                                statu=0; 
                                if((stat_sql.StartsWith("DEPURACION"))&&((stat_sql.Length)<15)){
                                	statu= Convert.ToInt32(stat_sql.Substring(10,((stat_sql.Length)-10)));
                                }
                    		}
                    		m++;
                    	} while (m < dgrc1);
                    	
                    	if(x==2){
                    		porcentaje_min_pago=0;
							x=3;						 
                    	}
                    	
                    	porcentaje_min_pago=porcentaje_min_pago+porcentaje_sql;
                    	
                    	if(((Convert.ToDouble(i4ss_sql)-importe_pago)>=500) && (x==3)){
                    		if((porcentaje_min_pago>=75)&&(porcentaje_min_pago<76)){
                                	tot_no_depurar++;
                                	no_depurar=1;
                    		}
                        }
                    	
                    	if ((porcentaje_min_pago>=75.00)&&(x==3))
                    		{
                    			if(no_depurar==1){	   
                    			//data_depura1.Rows.Add(nrp_sql,cre_sql,i4ss_sql,importe_pago,f_sua_acu,statuz,id_sql,fecha_pago_acu,num_pago);
	                    			
                    			textBox7.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+" - \",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() +
                    			                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    			                       
                    			                    no_depurar=0;
	                    		}else{
                    				if(statu>0){
                    					statuz="DEPURACION"+(statu+1);
                    				}else{
	                    				if(stat_sql.Equals("DEPURACION MANUAL")){
	                    					statuz="DEPURACION MANUAL";
	                    				}else{
	                    					statuz="DEPURACION1";
	                    				}
                    				}
                    				
                    			
	                    			if(tipo_doc_sel==2){
	                    				textBox7.AppendText("UPDATE datos_factura SET status_credito=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() +
	                    			                   ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", observaciones=\"CORE_"+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    				                    // ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}else{
                    					textBox7.AppendText("UPDATE datos_factura SET status=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() +
                    			                         ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				                    // ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				}
                    				
                    			                        
                    				tot_cop_est++;
	                    			if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(1);
                                    }
	                    			// textBox7.AppendText("ingresados:" + tot_cop_est + " procesados:" + p);
	                    		}
                    		}else{
                    		if(x==3){
	                    		if(porcentaje_min_pago>=100.00){
	                    			/*
                    				if(tipo_doc_sel==2){
                    					textBox7.AppendText("UPDATE datos_factura SET status_credito=\"DEPURACION_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}else{
                    					textBox7.AppendText("UPDATE datos_factura SET status=\"DEPURACION_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}
                    				                         
                    				                         tot_pag_cop_est++;
                                    if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(1);
                                    }*/
                                }else{
                                    textBox7.AppendText("UPDATE datos_factura SET folio_sipare_sua=\"" + f_sua_acu + "\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= " + num_pago +", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    				                        
                    				                    tot_menores++;
                    			}
                    		}else{
                    			duplicados++;
                    		}
                    		}
                    }else{
                    	omitidos++;
                    }
                    p++;
                    //label7.Text="data1:"+m+" data2:"+p+" regpat:"+nrp_sql;
                    //label7.Refresh();
                    cont_wait++;
                   
                    if (cont_wait == 10)
                    {
                        label23.Text = "COP EST: Analizando Datos.";
                        label23.Refresh();
                    }
                    if (cont_wait == 20)
                    {
                        label23.Text = "COP EST: Analizando Datos..";
                        label23.Refresh();
                    }
                    if (cont_wait == 30)
                    {
                        label23.Text = "COP EST: Analizando Datos...";
                        label23.Refresh();
                    }
                    if (cont_wait == 40)
                    {
                        label23.Text = "COP EST: Analizando Datos";
                        label23.Refresh();
                        cont_wait = 0;
                    }
                                            

                } while (p < dgrc2);
            }
            else
            {
            	error_vacio=1;
            	
            	                         	panel6.Enabled=false;
                MessageBox.Show("La Búsqueda del Periodo: COP_ECO_EST_"+periodo_sipare+" no arrojó resultados.\nFavor de revisar la Base de Datos","Aviso");
            }
            conexion3.Close();
            
			
			label14.Text= "COP_ECO_EST_"+periodo_sipare;
			
			if(error_vacio==0){
				total_cop_est=tot_pag_cop_est+tot_cop_est+tot_menores+tot_no_depurar;
				label15.Text = "PAGO > 75%  ("+(tot_cop_est+tot_pag_cop_est)+")\nPAGO < 75%  ("+(tot_menores+tot_no_depurar)+")\nTOTAL: "+(total_cop_est);
                ev_depu1 = b18;
                ev_tot_dep1 = tot_cop_est + tot_pag_cop_est;
                toolTip1.ToolTipTitle = "Detalle de "+label14.Text;
                toolTip1.SetToolTip(label15,   "\nPagado al 100%\t     "+tot_pag_cop_est+ "\nPago Mayor a 75%     " + (tot_cop_est) +  "\nPago Menor a 75%     "+ (tot_menores) +
                                  "\nDiferencia > $500       " + tot_no_depurar + "\nPrimera Depuracion     " + b18 + "\n\nPagos Totales\t     " + ((dataGridView2.RowCount) - omitidos) + "\nSin Pago\t\t     " + omitidos +
                                  "\nPagos Previos\t     "+(duplicados-dupli_en)+"\nLineas Duplicadas\t "+dupli_en+"\n\nCasos Totales\t     "+dataGridView2.RowCount+"\nOmitidos\t      "+(tot_per_eco_est-dataGridView2.RowCount)+"\n\nTotal del Periodo\t     "+tot_per_eco_est);
            }else{
			label15.Text="PAGO > 75%  ("+(0)+")\nPAGO < 75%  ("+(0)+")\nTOTAL: "+(0);	
			}
			
			if(total_cop_est>0){
				panel6.Enabled=true;
			}

            panel5.Visible = true;
            panel3.Visible = false;
                //this.panel6.Location = new System.Drawing.Point(240, 110);
                //this.panel7.Location = new System.Drawing.Point(240, 233);
  
			//MessageBox.Show(encont.ToString());
			                         }));
		}		
		
		public void consultar_COP_MEC(){
			
			Invoke(new MethodInvoker(delegate
					                         {
			label23.Text="COP MEC: Consultando Base de Datos...";
			label23.Refresh();
			sql="SELECT registro_patronal2,folio_sipare_sua,fecha_pago,importe_cuota,id,status_credito,importe_pago,num_pago,porcentaje_pago,credito_multa,importe_multa,tipo_documento,subdelegacion,status,credito_cuotas FROM datos_factura WHERE "+ 
                "nombre_periodo = \"COP_ECO_MEC_"+periodo_sipare+"\" AND (status = \"EN TRAMITE\" OR status = \"0\") ORDER BY registro_patronal2,credito_multa";
			data_cop=conex.consultar(sql);
			//MessageBox.Show(sql);
			dataGridView2.DataSource=data_cop;
			m=0;
			p=0;
			y=0;
			tot_cop_mec=0;
			duplicados=0;
			cont_wait=0;
			omitidos=0;
			tot_no_depurar=0;
			tot_pag_cop_mec=0;
			error_vacio=0;
			tot_menores=0;
			porcentaje_sql=0;
			dupli_en=0;
			
			label23.Text="COP MEC: Leyendo Datos Temporales...";
			label23.Refresh();
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal.xlsx';Extended Properties=Excel 12.0;";
			conexion3 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion3.Open(); //abrimos la conexion
			
			if(tipo_doc_sel>=2){
				//cargar RALE
				cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
				conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
				conexion4.Open(); //abrimos la conexion
			}
			
			label23.Text="COP MEC: Analizando Datos";
			label23.Refresh();
			dgrc2 = dataGridView2.RowCount; 
			
            if (dgrc2 > 0)
            {
                do
                {
                    nrp_sql = dataGridView2.Rows[p].Cells[0].FormattedValue.ToString();
                    f_sua_sql = dataGridView2.Rows[p].Cells[1].FormattedValue.ToString();
                    fecha_pago_sql = dataGridView2.Rows[p].Cells[2].FormattedValue.ToString();
                    i4ss_sql = dataGridView2.Rows[p].Cells[3].FormattedValue.ToString();
                    id_sql = dataGridView2.Rows[p].Cells[4].FormattedValue.ToString();
                    importe_pago_sql= Convert.ToDouble(dataGridView2.Rows[p].Cells[6].FormattedValue.ToString());
					num_pago = Convert.ToInt32(dataGridView2.Rows[p].Cells[7].FormattedValue.ToString());
					porcentaje_sql = Convert.ToDouble(dataGridView2.Rows[p].Cells[8].FormattedValue.ToString());
					cre_sql = dataGridView2.Rows[p].Cells[9].FormattedValue.ToString();
					imp_mul_sql = dataGridView2.Rows[p].Cells[10].FormattedValue.ToString();
					td_sql = dataGridView2.Rows[p].Cells[11].FormattedValue.ToString();
					sub_sql = dataGridView2.Rows[p].Cells[12].FormattedValue.ToString();
					stat_sql = dataGridView2.Rows[p].Cells[13].FormattedValue.ToString();
					cre_cuo_sql	= dataGridView2.Rows[p].Cells[14].FormattedValue.ToString();
					stat_cred_sql = dataGridView2.Rows[p].Cells[5].FormattedValue.ToString();

                    cons_exc2 = "Select [NRP],[F#SUA],[FECHA PAGO],[4SS] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"  and [4SS] > 0 order by [F#SUA]";
                    carga_excel2();
                    separar_sipare();
                    
                    dgrc1= dataGridView1.RowCount;                        	
                    ingresados = new string[dgrc1];
                    
                    if(tipo_doc_sel>=2){
	                    
                    	cons_exc = "Select [NRP] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"";
	                    carga_excel3(); //registro existe en el RALE??
	                    stat_sql=stat_cred_sql;
	            		if(dataGridView5.RowCount==0){
	            			dgrc1=0;
	            		}
                    }
                    
                    m = 0;
                    importe_pago=0;
                    porcentaje_min_pago=0;
                    fecha_pago_acu="";
                    f_sua_acu="";
                    acumular=0;
                    counpri=0;
                    importe_pago=importe_pago_sql;

                    if (f_sua_sql.Length > 1)
                    {
                        f_sua_acu = f_sua_sql;
                    }
                    else
                    {
                        f_sua_acu = "";
                    }

                    if (fecha_pago_sql.Length > 1)
                    {
                        if(fecha_pago_sql.Equals("-")){
                    	   	fecha_pago_acu= "";
                    	}else{
                        	fecha_pago_acu = fecha_pago_sql;
                    	}
                    }
                    else
                    {
                        fecha_pago_acu = "";
                    }
                    
                    if (dgrc1 > 0)
                    {
                    	do
                    	{ 
                    		nrp = dataGridView1.Rows[m].Cells[0].FormattedValue.ToString();
                    		f_sua = dataGridView1.Rows[m].Cells[1].FormattedValue.ToString();
                    		fecha_pago = dataGridView1.Rows[m].Cells[2].FormattedValue.ToString();
                    		i4ss = dataGridView1.Rows[m].Cells[3].FormattedValue.ToString();

                    		comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago + "_" + i4ss;
                    		
                    		if(fecha_pago.Length<2){
                    			fecha_pago="";
                    		}
                    		
							acumular=0;
                    		if (m != 0)
                    		{
                    			for (i = 0; i < ingresados.Length; i++)
                    			{
                    				if (ingresados[i] != null)
                    				{
                    					if (ingresados[i].Equals(comp_dupli))
                    					{
                    						//i4ss = "0.00";
                    						acumular=1;
                    						dupli_en++;
                    					}else{
                    						acumular=0;
                    					}
                    				}
                    			}
                    		}

                            x = 0;
                            if (f_sua_sql.Length > 1){
                            	checar_folio = f_sua_sql.Split(' ');
                            	for (y = 0; y < checar_folio.Length; y++){
                                    if (f_sua.Length > checar_folio[y].Length)
                                    {
                                        if (checar_folio[y].Equals(f_sua.Substring(0, checar_folio[y].Length)))
                                        {
                                            x = 1;
                                        }
                                    }
                                    else
                                    {
                                        if ((checar_folio[y].Substring(0, f_sua.Length)).Equals(f_sua))
                                        {
                                            x = 1;
                                        }
                                    }	    		
                            	}	
                            }else{
                            	if((porcentaje_sql>0)||(fecha_pago_sql.Length>1)||(importe_pago_sql>0)){
                            	x=2;
                            	}
                            }
                    		
                    		 if((acumular==0)&&(x==0)){
                            	x=3;	
                            }
                    		
                            if((x==3)){
                                importe_pago = importe_pago + (Convert.ToDouble(i4ss));
                                porcentaje_min_pago = porcentaje_min_pago + ((Convert.ToDouble(i4ss) * 100) / (Convert.ToDouble(i4ss_sql)));
                                if(f_sua_acu.Length==0){
                                	f_sua_acu = f_sua;
                                }else{
                                	f_sua_acu = f_sua_acu + " - " + f_sua;
                                }
                                if(fecha_pago_acu.Length==0){
                                	fecha_pago_acu = fecha_pago;
                                }else{
                                	fecha_pago_acu = fecha_pago_acu + " - " + fecha_pago;
                                }
                                //acumular = 0;
                                ingresados[m] = comp_dupli;
                                num_pago = num_pago + 1;
                                 if(counpri==0){
                                	if(f_sua.Length>5){
                                		folio_pago=f_sua.Substring(0,6);
                                	}else{
                                		folio_pago=f_sua.Substring(0,f_sua.Length);
                                	}
                                	fecha_pripago = fecha_pago;
                                	counpri=1;
                                }
                                
                                statu=0; 
                                if((stat_sql.StartsWith("DEPURACION"))&&((stat_sql.Length)<15)){
                                	
                                	statu= Convert.ToInt32(stat_sql.Substring(10,((stat_sql.Length)-10)));
                                }
                    		}
                    		m++;
                    	} while (m < dgrc1);
                    	
                    	if(x==2){
                    		porcentaje_min_pago=0;
							x=3;						 
                    	}
                    	
                    	porcentaje_min_pago=porcentaje_min_pago+porcentaje_sql;
                    	if(((Convert.ToDouble(i4ss_sql)-importe_pago)>=500) && (x==3)){
                    		if((porcentaje_min_pago>=75)&&(porcentaje_min_pago<76)){
                                	tot_no_depurar++;
                                	no_depurar=1;
                    		}
                        }
                    	
                    	if ((porcentaje_min_pago>=75.00)&&(x==3))
                    		{
                    			if(no_depurar==1){
	                    			textBox8.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+" - \",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
	                    			no_depurar=0;
	                    		}else{
                    			
                    				if(statu>0){
                    					statuz="DEPURACION"+(statu+1);
                    				}else{
	                    				if(stat_sql.Equals("DEPURACION MANUAL")){
	                    					statuz="DEPURACION MANUAL";
	                    				}else{
	                    					statuz="DEPURACION1";
	                    				}
                    				}
	                    			
                    				if(tipo_doc_sel==2){
	                    				textBox8.AppendText("UPDATE datos_factura SET status_credito=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				                //", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    			}else{
                    					textBox8.AppendText("UPDATE datos_factura SET status=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    								//", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				}
                    			
                    				tot_cop_mec++;
	                    			if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(2);
                                    }
	                    		}
                    	
                    			// textBox7.AppendText("ingresados:" + tot_cop_est + " procesados:" + p);
                    		}else{
	                    		if(x==3){
		                    		if (porcentaje_min_pago>=100.00){
		                    			/*
                    					if(tipo_doc_sel==2){
                    						textBox8.AppendText("UPDATE datos_factura SET status_credito=\"DEPURACION_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() +
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    					}else{
                    						textBox8.AppendText("UPDATE datos_factura SET status=\"DEPURACION_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() +
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    					}
                    				
                    					tot_pag_cop_mec++;
	                                    if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    		llena_report(2);
                                    	}*/
	                                }else{
		                    			textBox8.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
		                    			tot_menores++;
                    			}
	                    		}else{
	                    			duplicados++;
	                    		}
                    		}
                    
             
                    }else{
                    	omitidos++;
                    }
                    p++;
                    //label7.Text="data1:"+m+" data2:"+p+" regpat:"+nrp_sql;
                    //label7.Refresh();
					 cont_wait++;
                            if (cont_wait == 10)
                            {
                                label23.Text = "COP MEC: Analizando Datos.";
                                label23.Refresh();
                            }
                            if (cont_wait == 20)
                            {
                                label23.Text = "COP MEC: Analizando Datos..";
                                label23.Refresh();
                            }
                            if (cont_wait == 30)
                            {
                                label23.Text = "COP MEC: Analizando Datos...";
                                label23.Refresh();
                            }
                            if (cont_wait == 40)
                            {
                                label23.Text = "COP MEC: Analizando Datos";
                                label23.Refresh();
                                cont_wait = 0;
                            }
                } while (p < dgrc2);
            }
            else
            {
            	error_vacio=1;
                panel7.Enabled=false;
            	MessageBox.Show("La Búsqueda del Periodo: COP_ECO_MEC_" + periodo_sipare + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
            }
			conexion3.Close();
			//label16.Text= "COP_ECO_MEC_"+periodo_sipare;
			//label17.Text="PAGO > 75%  ("+(tot_cop_mec+1)+")\nPAGO < 75%  ("+((dataGridView2.RowCount - (tot_cop_mec+1))-duplicados)+")\nTOTAL: "+(dataGridView2.RowCount-duplicados);
			
			
			label16.Text= "COP_ECO_MEC_"+periodo_sipare;
			if(error_vacio==0){
				total_cop_mec=tot_menores+tot_no_depurar+tot_pag_cop_mec+tot_cop_mec;
                label17.Text = "PAGO > 75%  ("+(tot_cop_mec+tot_pag_cop_mec)+")\nPAGO < 75%  ("+(tot_menores+tot_no_depurar)+")\nTOTAL: "+(total_cop_mec);
                ev_depu2 = b19;
                ev_tot_dep2 = tot_cop_mec + tot_pag_cop_mec;
                toolTip2.ToolTipTitle = "Detalle de "+label16.Text;
                 toolTip2.SetToolTip(label17,  "\nPagado al 100%\t     "+tot_pag_cop_mec+ "\nPago Mayor a 75%     " + (tot_cop_mec) + "\nPago Menor a 75%     "+ (tot_menores) +
                                    "\nDiferencia > $500       " + tot_no_depurar + "\nPrimera Depuracion     " + b19 + "\n\nPagos Totales\t     " + ((dataGridView2.RowCount) - omitidos) + "\nSin Pago\t\t     " + omitidos +
                                    "\nPagos Previos\t     "+(duplicados-dupli_en)+"\nLineas Duplicadas\t "+dupli_en+"\n\nCasos Totales\t     "+dataGridView2.RowCount+"\nOmitidos\t      "+(tot_per_eco_mec-dataGridView2.RowCount)+"\n\nTotal del Periodo\t     "+tot_per_eco_mec);
			}else{
			label17.Text="PAGO > 75%  ("+(0)+")\nPAGO < 75%  ("+(0)+")\nTOTAL: "+(0);
			}
			
			if(total_cop_mec>0){
				panel7.Enabled=true;
			}
            panel5.Visible = true;
            panel3.Visible = false;
			//MessageBox.Show(encont.ToString());
			/*if(tipo_cre==1){
			this.panel6.Location = new System.Drawing.Point(240, 110);
			this.panel7.Location = new System.Drawing.Point(240, 233);
			panel8.Visible=false;
			panel9.Visible=false;
			panel5.Visible=true;
			panel3.Visible=false;
			}*/
			
			                         }));
		}
		
		public void consultar_RCV_EST(){
			
			Invoke(new MethodInvoker(delegate
					                         {
			
			label23.Text="RCV EST: Consultando Base de Datos...";
			label23.Refresh();
			//periodo_sipare=(periodo_sipare.Substring(0,4))+mes_periodo.ToString();
			sql="SELECT registro_patronal2,folio_sipare_sua,fecha_pago,importe_cuota,id,status_credito,importe_pago,num_pago,porcentaje_pago,credito_multa,importe_multa,tipo_documento,subdelegacion,status,credito_cuotas FROM datos_factura WHERE "+
                "nombre_periodo =\"RCV_ECO_EST_"+periodo_sipare_rcv+"\" AND (status = \"EN TRAMITE\" OR status = \"0\") ORDER BY registro_patronal2,credito_multa";
			//MessageBox.Show(sql);
			data_cop=conex.consultar(sql);
			dataGridView2.DataSource=data_cop;
			m=0;
			p=0;
			y=0;
			x=0;
			tot_rcv_est=0;
			duplicados=0;
			cont_wait=0;
			omitidos=0;
			tot_no_depurar=0;
			tot_pag_rcv_est=0;
			error_vacio=0;
			tot_menores=0;
			porcentaje_sql=0;
			dupli_en=0;
			
			label23.Text="RCV EST: Leyendo Datos Temporales...";
			label23.Refresh();
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal.xlsx';Extended Properties=Excel 12.0;";
			conexion3 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion3.Open(); //abrimos la conexion
			
			if(tipo_doc_sel>=2){
				//cargar RALE
				cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
				conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
				conexion4.Open(); //abrimos la conexion
			}
			
			label23.Text="RCV EST: Analizando Datos";
			label23.Refresh();
			dgrc2= dataGridView2.RowCount;                          
			                         
            if (dgrc2 > 0)
            {
                do
                {
                   nrp_sql = dataGridView2.Rows[p].Cells[0].FormattedValue.ToString();
                    f_sua_sql = dataGridView2.Rows[p].Cells[1].FormattedValue.ToString();
                    fecha_pago_sql = dataGridView2.Rows[p].Cells[2].FormattedValue.ToString();
                    i4ss_sql = dataGridView2.Rows[p].Cells[3].FormattedValue.ToString();
                    id_sql = dataGridView2.Rows[p].Cells[4].FormattedValue.ToString();
                    importe_pago_sql= Convert.ToDouble(dataGridView2.Rows[p].Cells[6].FormattedValue.ToString());
					num_pago = Convert.ToInt32(dataGridView2.Rows[p].Cells[7].FormattedValue.ToString());
					porcentaje_sql = Convert.ToDouble(dataGridView2.Rows[p].Cells[8].FormattedValue.ToString());
					cre_sql = dataGridView2.Rows[p].Cells[9].FormattedValue.ToString();
					imp_mul_sql = dataGridView2.Rows[p].Cells[10].FormattedValue.ToString();
					td_sql = dataGridView2.Rows[p].Cells[11].FormattedValue.ToString();
					sub_sql = dataGridView2.Rows[p].Cells[12].FormattedValue.ToString();
					stat_sql = dataGridView2.Rows[p].Cells[13].FormattedValue.ToString();
					cre_cuo_sql	= dataGridView2.Rows[p].Cells[14].FormattedValue.ToString();
					stat_cred_sql = dataGridView2.Rows[p].Cells[5].FormattedValue.ToString();

                    cons_exc2 = "Select [NRP],[F#SUA],[FECHA PAGO],[RCV] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"  and [RCV]  > 0 order by [F#SUA]";
                    carga_excel2();
                    separar_sipare();
                    
                    dgrc1 = dataGridView1.RowCount;                        	
                    ingresados = new string[dgrc1];
                    
                    if(tipo_doc_sel>=2){
                    	
                    	cons_exc = "Select [NRP] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"";
	            		carga_excel3(); //registro existe en el RALE?? o en la DEP_MANUAL
	            		stat_sql=stat_cred_sql;
	            		if(dataGridView5.RowCount==0){
	            			dgrc1=0;
	            		}
                    }
                    
                    m = 0;
                    importe_pago=0;
                    porcentaje_min_pago=0;
                    fecha_pago_acu="";
                    f_sua_acu="";
                    acumular=0;
                    counpri=0;
                   	importe_pago=importe_pago_sql;

                    if (f_sua_sql.Length > 1)
                    {
                    	if(f_sua_sql.Equals(" -")){
                    		f_sua_acu = "";
                    	}else{
                    		f_sua_acu = f_sua_sql;
                    	}
                    }
                    else
                    {
                        f_sua_acu = "";
                    }

                    if (fecha_pago_sql.Length > 1)
                    {
                    	if(fecha_pago_sql.Equals(" -")){
                    	   	fecha_pago_acu= "";
                    	}else{
                        	fecha_pago_acu = fecha_pago_sql;
                    	}
                    }
                    else
                    {
                        fecha_pago_acu = "";
                    }
                    
                    if (dgrc1 > 0)
                    {
                    	do
                    	{ 
                    		nrp = dataGridView1.Rows[m].Cells[0].FormattedValue.ToString();
                    		f_sua = dataGridView1.Rows[m].Cells[1].FormattedValue.ToString();
                    		fecha_pago = dataGridView1.Rows[m].Cells[2].FormattedValue.ToString();
                    		i4ss = dataGridView1.Rows[m].Cells[3].FormattedValue.ToString();

                    		comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago + "_" + i4ss;
                    		
                    		if(fecha_pago.Length<2){
                    			fecha_pago="";
                    		}
                    		
							acumular=0;
                    		if (m != 0)
                    		{
                    			for (i = 0; i < ingresados.Length; i++)
                    			{
                    				if (ingresados[i] != null)
                    				{
                    					if (ingresados[i].Equals(comp_dupli))
                    					{
                    						//i4ss = "0.00";
                    						acumular=1;
                    						dupli_en++;
                    					}else{
                    						acumular=0;
                    					}
                    				}
                    			}
                    		}

                            x = 0;
                            if (f_sua_sql.Length > 1){
                            	checar_folio = f_sua_sql.Split(' ');
                            	for (y = 0; y < checar_folio.Length; y++){
                                    if (f_sua.Length > checar_folio[y].Length)
                                    {
                                        if (checar_folio[y].Equals(f_sua.Substring(0, checar_folio[y].Length)))
                                        {
                                            x = 1;
                                        }
                                    }
                                    else
                                    {
                                        if ((checar_folio[y].Substring(0, f_sua.Length)).Equals(f_sua))
                                        {
                                            x = 1;
                                        }
                                    }	   		
                            	}	
                            }else{
                            	if((porcentaje_sql>0)||(fecha_pago_sql.Length>1)||(importe_pago_sql>0)){
                            	x=2;
                            	}
                            }
                    		
                    		if((acumular==0)&&(x==0)){
                            	x=3;  	
                            }
                    		
                            if((x==3)){
                    			importe_pago=importe_pago+(Convert.ToDouble(i4ss));
                                porcentaje_min_pago = porcentaje_min_pago + ((Convert.ToDouble(i4ss) * 100) / (Convert.ToDouble(i4ss_sql)));
                    			if(f_sua_acu.Length==0){
                                	f_sua_acu = f_sua;
                                }else{
                                	f_sua_acu = f_sua_acu + " - " + f_sua;
                                }
                                if(fecha_pago_acu.Length==0){
                                	fecha_pago_acu = fecha_pago;
                                }else{
                                	fecha_pago_acu = fecha_pago_acu + " - " + fecha_pago;
                                }
                    			acumular=0;
                    			ingresados[m] = comp_dupli;
                                num_pago = num_pago + 1;
                                
                                if(counpri==0){
                                	if(f_sua.Length>5){
                                		folio_pago=f_sua.Substring(0,6);
                                	}else{
                                		folio_pago=f_sua.Substring(0,f_sua.Length);
                                	}
                                	fecha_pripago = fecha_pago;
                                	counpri=1;
                                }
                                
                                statu=0; 
                                if((stat_sql.StartsWith("DEPURACION"))&&((stat_sql.Length)<15)){
                                	
                                	statu= Convert.ToInt32(stat_sql.Substring(10,((stat_sql.Length)-10)));
                                }
                    		}
                    		m++;
                    	} while (m < dgrc1);
                    	
                    	if(x==2){
                    		porcentaje_min_pago=0;
							x=3;						 
                    	}
                    	
                    	porcentaje_min_pago=porcentaje_min_pago+porcentaje_sql;
                    	if(((Convert.ToDouble(i4ss_sql)-importe_pago)>=500) && (x==3)){
                    		if((porcentaje_min_pago>=75)&&(porcentaje_min_pago<76)){
                                	tot_no_depurar++;
                                	no_depurar=1;
                    		}
                        }
                    	
                    	if(statu>0){
                    		statuz="DEPURACION"+(statu+1);
                    	}else{
                    		//if(stat_sql.Equals("DEPURACION MANUAL")){
                    		statuz="DEPURACION MANUAL";
                    		//}else{
                    		//	statuz="DEPURACION1";
                    		//	}
                    	}
                    	
                    	if ((porcentaje_min_pago>=75.00)&&(no_depurar==0)&&(x==3))
                    		{
                    			if(no_depurar==1){
                    				textBox9.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() +
                    			                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    				no_depurar=0;
                    			}else{		
                    			
                    				if(tipo_doc_sel==2){
                    					textBox9.AppendText("UPDATE datos_factura SET status_credito=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                       ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\",observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				                 //", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				}else{
                    					textBox9.AppendText("UPDATE datos_factura SET status=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\",observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				                //", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    				}
                    				tot_rcv_est++;
                    				if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(3);
                                    }
                    			}
                    			// textBox7.AppendText("ingresados:" + tot_cop_est + " procesados:" + p);
                    		}else{
                    			 if((x==3)){
	                    		 if (porcentaje_min_pago>=100.00){
	                    			/*
                    				if(tipo_doc_sel==2){
                    					//textBox9.AppendText("UPDATE datos_factura SET status_credito=\"PAGADO\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				   //                ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    				   	  textBox9.AppendText("UPDATE datos_factura SET status_credito=\"PAGADO_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				                   ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}else{
                    					textBox9.AppendText("UPDATE datos_factura SET status=\"PAGADO_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}
                    				
                    				tot_pag_rcv_est++;
	                    			if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(3);
                                    }*/
                    		}else{
	                    			textBox9.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    				                    ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    			    tot_menores++;
                    			}
                    		}else{
                    			duplicados++;
                    		}
                    		}
             
                    }else{
                    	omitidos++;
                    }
                    p++;
                    //label7.Text="data1:"+m+" data2:"+p+" regpat:"+nrp_sql;
                    //label7.Refresh();
                    cont_wait++;
                            if (cont_wait == 10)
                            {
                                label23.Text = "RCV EST: Analizando Datos.";
                                label23.Refresh();
                            }
                            if (cont_wait == 20)
                            {
                                label23.Text = "RCV EST: Analizando Datos..";
                                label23.Refresh();
                            }
                            if (cont_wait == 30)
                            {
                                label23.Text = "RCV EST: Analizando Datos...";
                                label23.Refresh();
                            }
                            if (cont_wait == 40)
                            {
                                label23.Text = "RCV EST: Analizando Datos";
                                label23.Refresh();
                                cont_wait = 0;
                            }

                } while (p < dgrc2);
            }
            else
            {
            	error_vacio=1;
            	MessageBox.Show("La Búsqueda del Periodo: RCV_ECO_EST_" + periodo_sipare_rcv + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
				panel8.Enabled=false;
            }
			conexion3.Close();
			label18.Text= "RCV_ECO_EST_"+periodo_sipare_rcv;
			
			if(error_vacio==0){
				total_rcv_est=tot_menores+tot_no_depurar+tot_pag_rcv_est+tot_rcv_est;
                label19.Text = "PAGO > 75%  ("+(tot_rcv_est+tot_pag_rcv_est)+")\nPAGO < 75%  ("+(tot_menores+tot_no_depurar)+")\nTOTAL: "+(total_rcv_est);
                 toolTip3.ToolTipTitle = "Detalle de "+label18.Text;
                 ev_depu3 = b20;
                 ev_tot_dep3 = tot_rcv_est + tot_pag_rcv_est;
                 toolTip3.SetToolTip(label19, "\nPagado al 100%\t     "+tot_pag_rcv_est+ "\nPago Mayor a 75%     " + (tot_rcv_est) +"\nPago Menor a 75%     "+ (tot_menores) +
                                    "\nDiferencia > $500       " + tot_no_depurar + "\nPrimera Depuracion     " + b20 + "\n\nPagos Totales\t     " + ((dataGridView2.RowCount) - omitidos) + "\nSin Pago\t\t     " + omitidos +
                                    "\nPagos Previos\t     "+(duplicados-dupli_en)+"\nLineas Duplicadas\t "+dupli_en+"\n\nCasos Totales\t     "+dataGridView2.RowCount+"\nOmitidos\t      "+(tot_per_rcv_est-dataGridView2.RowCount)+"\n\nTotal del Periodo\t     "+tot_per_rcv_est);
			}else{
			    label19.Text="PAGO > 75%  ("+(0)+")\nPAGO < 75%  ("+(0)+")\nTOTAL: "+(0);
			}

			if(total_rcv_est>0){
				panel8.Enabled=true;
			}

            panel5.Visible = true;
            panel3.Visible = false;
			//MessageBox.Show(encont.ToString());
			                         }));
		}
		
		public void consultar_RCV_MEC(){
			
			Invoke(new MethodInvoker(delegate
					                         {
			label23.Text="RCV MEC: Consultando Base de Datos...";
			label23.Refresh();
			//periodo_sipare=(periodo_sipare.Substring(0,4))+mes_periodo.ToString();
			sql="SELECT registro_patronal2,folio_sipare_sua,fecha_pago,importe_cuota,id,status_credito,importe_pago,num_pago,porcentaje_pago,credito_multa,importe_multa,tipo_documento,subdelegacion,status,credito_cuotas FROM datos_factura WHERE "+
                "nombre_periodo =  \"RCV_ECO_MEC_"+periodo_sipare_rcv+"\" AND (status = \"EN TRAMITE\" OR status = \"0\") ORDER BY registro_patronal2,credito_multa";
			data_cop=conex.consultar(sql);
			dataGridView2.DataSource=data_cop;
			m=0;
			p=0;
			x=0;
			tot_rcv_mec=0;
			duplicados=0;
			cont_wait=0;
			omitidos=0;
			tot_pag_rcv_mec=0;
			tot_no_depurar=0;
			error_vacio=0;
			tot_menores=0;
			porcentaje_sql=0;
			dupli_en=0;
			
			label23.Text="RCV MEC: Leyendo Datos Temporales...";
			label23.Refresh();
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal.xlsx';Extended Properties=Excel 12.0;";
			conexion3 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion3.Open(); //abrimos la conexion
			
			if(tipo_doc_sel>=2){
				//cargar RALE
				cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
				conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
				conexion4.Open(); //abrimos la conexion
			}
			
			label23.Text="RCV MEC: Analizando Datos";
			label23.Refresh();
			dgrc2 = dataGridView2.RowCount;
			
            if (dgrc2 > 0)
            {
                 do
                {
                    nrp_sql = dataGridView2.Rows[p].Cells[0].FormattedValue.ToString();
                    f_sua_sql = dataGridView2.Rows[p].Cells[1].FormattedValue.ToString();
                    fecha_pago_sql = dataGridView2.Rows[p].Cells[2].FormattedValue.ToString();
                    i4ss_sql = dataGridView2.Rows[p].Cells[3].FormattedValue.ToString();
                    id_sql = dataGridView2.Rows[p].Cells[4].FormattedValue.ToString();
                    importe_pago_sql= Convert.ToDouble(dataGridView2.Rows[p].Cells[6].FormattedValue.ToString());
					num_pago = Convert.ToInt32(dataGridView2.Rows[p].Cells[7].FormattedValue.ToString());
					porcentaje_sql = Convert.ToDouble(dataGridView2.Rows[p].Cells[8].FormattedValue.ToString());
					cre_sql = dataGridView2.Rows[p].Cells[9].FormattedValue.ToString();
					imp_mul_sql = dataGridView2.Rows[p].Cells[10].FormattedValue.ToString();
					td_sql = dataGridView2.Rows[p].Cells[11].FormattedValue.ToString();
					sub_sql = dataGridView2.Rows[p].Cells[12].FormattedValue.ToString();
					stat_sql = dataGridView2.Rows[p].Cells[13].FormattedValue.ToString();
					cre_cuo_sql	= dataGridView2.Rows[p].Cells[14].FormattedValue.ToString();
					stat_cred_sql = dataGridView2.Rows[p].Cells[5].FormattedValue.ToString();
					
                    cons_exc2 = "Select [NRP],[F#SUA],[FECHA PAGO],[RCV] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"  and [RCV] > 0 order by [F#SUA]";
                    carga_excel2();
                    separar_sipare();
                    
                    dgrc1 = dataGridView1.RowCount;                        	
                    ingresados = new string[dgrc1];
                    
                    //MessageBox.Show("|"+f_sua_sql+"|, |"+fecha_pago_sql+"|");
                    
                    if(tipo_doc_sel>=2){
	                  
                    	cons_exc = "Select [NRP] from [hoja_lz$] where [NRP] =\"" + nrp_sql + "\"";
	            		carga_excel3(); //registro existe en el RALE??
	            		stat_sql=stat_cred_sql;
	            		if(dataGridView5.RowCount==0){
	            			dgrc1=0;
	            		}
                    }
                    
                    m = 0;
                    importe_pago=0;
                    porcentaje_min_pago=0;
                    fecha_pago_acu="";
                    f_sua_acu="";
                    acumular=0;
                    counpri=0;
                    importe_pago=importe_pago_sql;

                    if (f_sua_sql.Length > 1)
                    {
                        if(f_sua_sql.Equals("-")){
                    		f_sua_acu = "";
                    	}else{
                    		f_sua_acu = f_sua_sql;
                    	}
                    }
                    else
                    {
                        f_sua_acu="";
                    }

                    if (fecha_pago_sql.Length > 1)
                    {
                        if(fecha_pago_sql.Equals("-")){
                    	   	fecha_pago_acu= "";
                    	}else{
                        	fecha_pago_acu = fecha_pago_sql;
                    	}
                    }
                    else
                    {
                        fecha_pago_acu = "";
                    }
                    
                    if (dgrc1 > 0)
                    {
                    	do
                    	{ 
                    		nrp = dataGridView1.Rows[m].Cells[0].FormattedValue.ToString();
                    		f_sua = dataGridView1.Rows[m].Cells[1].FormattedValue.ToString();
                    		fecha_pago = dataGridView1.Rows[m].Cells[2].FormattedValue.ToString();
                    		i4ss = dataGridView1.Rows[m].Cells[3].FormattedValue.ToString();

                    		//comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago + "_" + i4ss;
                    		  comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago;
                    		  //MessageBox.Show(comp_dupli);
                    		
                    		if(fecha_pago.Length<2){
                    			fecha_pago="";
                    		}
                    		
							acumular=0;
							
                    		if (m != 0)
                    		{
                    			for (i = 0; i < ingresados.Length; i++)
                    			{
                    				if (ingresados[i] != null)
                    				{
                    					if (ingresados[i].Equals(comp_dupli))
                    					{
                    						//i4ss = "0.00";
                    						acumular=1;
                    						dupli_en++;
                    						
                    						//MessageBox.Show("1 , "+comp_dupli);
                    					}else{
                    						acumular=0;
                    						//MessageBox.Show("0 , "+comp_dupli);
                    					}
                    				}
                    			}
                    		}

                            x = 0;
                            if (f_sua_sql.Length > 1){
                            	checar_folio = f_sua_sql.Split(' ');
                            	for (y = 0; y < checar_folio.Length; y++){
                                    if (f_sua.Length > checar_folio[y].Length)
                                    {
                                        if (checar_folio[y].Equals(f_sua.Substring(0, checar_folio[y].Length)))
                                        {
                                            x = 4;
                                        }
                                    }
                                    else
                                    {
                                        if ((checar_folio[y].Substring(0, f_sua.Length)).Equals(f_sua))
                                        {
                                            x = 4;
                                        }
                                    }	     		
                            	}	
                            }else{
                            	if((porcentaje_sql>0)||(fecha_pago_sql.Length>1)||(importe_pago_sql>0)){
                            	x=2;
                            	}
                            }
                    		
                    		 if((acumular==0)&&(x==0)){
                            	x=3;
                            	
                            }
                    		
                            if((x==3)){
                                importe_pago = importe_pago + (Convert.ToDouble(i4ss));
                                porcentaje_min_pago = porcentaje_min_pago + ((Convert.ToDouble(i4ss) * 100) / (Convert.ToDouble(i4ss_sql)));
                                
                                if(f_sua_acu.Length==0){
                                	f_sua_acu = f_sua;
                                }else{
                                	f_sua_acu = f_sua_acu + " - " + f_sua;
                                }
                                if(fecha_pago_acu.Length==0){
                                	fecha_pago_acu = fecha_pago;
                                }else{
                                	fecha_pago_acu = fecha_pago_acu + " - " + fecha_pago;
                                }
                                
                                //acumular = 0;
                                ingresados[m] = comp_dupli;
                                num_pago = num_pago + 1;
                                
                                 if(counpri==0){
                                	if(f_sua.Length>5){
                                		folio_pago=f_sua.Substring(0,6);
                                	}else{
                                		folio_pago=f_sua.Substring(0,f_sua.Length);
                                	}
                                	fecha_pripago = fecha_pago;
                                	counpri=1;
                                }
                                
                                statu=0; 
                                if((stat_sql.StartsWith("DEPURACION"))&&((stat_sql.Length)<15)){
                                	
                                	statu= Convert.ToInt32(stat_sql.Substring(10,((stat_sql.Length)-10)));
                                }
                    		}
                    		m++;
                    	} while (m < dataGridView1.RowCount);
                    	
                    	if(x==2){
                    		porcentaje_min_pago=0;
							x=3;						 
                    	}
                    	
                    	porcentaje_min_pago=porcentaje_min_pago+porcentaje_sql;
                    	if(((Convert.ToDouble(i4ss_sql)-importe_pago)>=500) && (x==3)){
                    		if((porcentaje_min_pago>=75)&&(porcentaje_min_pago<76)){
                                	tot_no_depurar++;
                                	no_depurar=1;
                    		}
                        }
                    	
                    	if ((porcentaje_min_pago>=75.00)&&(x==3))
                    	{
                    		if(no_depurar==1){
	                    			textBox10.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                     ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
	                    	}else{
                    			
                    			if(statu>0){
                    					statuz="DEPURACION"+(statu+1);
                    				}else{
	                    				//if(stat_sql.Equals("DEPURACION MANUAL")){
	                    					statuz="DEPURACION MANUAL";
	                    				//}else{
	                    				//	statuz="DEPURACION1";
	                    				//}
                    				}

                    			if(tipo_doc_sel==2){
                    				textBox10.AppendText("UPDATE datos_factura SET status_credito=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                     ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\",observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    							    //", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    			}else{
                    				textBox10.AppendText("UPDATE datos_factura SET status=\""+statuz+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    			                     ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\",observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    								 //", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\", observaciones=\"CORE_"+fecha_dep+"\"  WHERE id=" + id_sql + ";\n");
                    			}
                    			
                    			tot_rcv_mec++;
	                    		if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(4);
                                    }
	                    		
	                  
	                    	}
                    			// textBox7.AppendText("ingresados:" + tot_cop_est + " procesados:" + p);
                    	}else{
                    		if((x==3)){
                    			if (porcentaje_min_pago>=100.00){
	                    		/*
                    			if(tipo_doc_sel==2){
                    					textBox10.AppendText("UPDATE datos_factura SET status_credito=\"PAGADO_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				                     ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}else{
                    					textBox10.AppendText("UPDATE datos_factura SET status=\"PAGADO_"+stat_sql+"\", folio_sipare_sua=\""+f_sua_acu+"\",fecha_pago=\"" + fecha_pago_acu + "\", importe_pago=" + importe_pago.ToString() + 
                    				                     ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\", fecha_notificacion= \""+fecha_not_dep+"\" WHERE id=" + id_sql + ";\n");
                    				}
                    				
                    			tot_pag_rcv_mec++;
	                    		if(((stat_sql.Substring(0,1)).Equals("D"))||((stat_sql.Substring(0,1)).Equals("d"))){}else{
                                    	llena_report(4);
                                    }*/
                    			}else{
	                    		textBox10.AppendText("UPDATE datos_factura SET folio_sipare_sua=\" "+f_sua_acu+" \",fecha_pago=\"" + fecha_pago_acu + "\" , importe_pago=" + importe_pago.ToString() + 
                    				                     ", porcentaje_pago=" + porcentaje_min_pago.ToString() + ", num_pago= "+num_pago+", fecha_depuracion= \""+fecha_dep+"\" WHERE id=" + id_sql + ";\n");
                    			tot_menores++;
                    			}
                    		}else{
                    			duplicados++;
                    		}
                    	}
             
                    }else{
                    	omitidos++;
                    }
                    p++;
                    //label7.Text="data1:"+m+" data2:"+p+" regpat:"+nrp_sql;
                    //label7.Refresh();
                    cont_wait++;
                            if (cont_wait == 10)
                            {
                                label23.Text = "RCV MEC: Analizando Datos.";
                                label23.Refresh();
                            }
                            if (cont_wait == 20)
                            {
                                label23.Text = "RCV MEC: Analizando Datos..";
                                label23.Refresh();
                            }
                            if (cont_wait == 30)
                            {
                                label23.Text = "RCV MEC: Analizando Datos...";
                                label23.Refresh();
                            }
                            if (cont_wait == 40)
                            {
                                label23.Text = "RCV MEC: Analizando Datos";
                                label23.Refresh();
                                cont_wait = 0;
                            }

                } while (p < dgrc2);
            }
            else
            {
            	error_vacio=1;
				panel9.Enabled=false;
                MessageBox.Show("La Búsqueda del Periodo: RCV_ECO_MEC_" + periodo_sipare_rcv + " no arrojó resultados.\nFavor de revisar la Base de Datos", "Aviso");
            }
			conexion3.Close();
			label20.Text= "RCV_ECO_MEC_"+periodo_sipare_rcv;
			if(error_vacio==0){
			total_rcv_mec= tot_menores+tot_no_depurar+tot_pag_rcv_mec+tot_rcv_mec;
			label21.Text = "PAGO > 75%  ("+(tot_rcv_mec+tot_pag_rcv_mec)+")\nPAGO < 75%  ("+(tot_no_depurar+tot_menores)+")\nTOTAL: "+(total_rcv_mec);
		     toolTip4.ToolTipTitle = "Detalle de "+label20.Text;
             ev_depu4 = b21;
             ev_tot_dep4 = tot_rcv_mec + tot_pag_rcv_mec;
             toolTip4.SetToolTip(label21, "\nPagado al 100%\t     "+tot_pag_rcv_mec+ "\nPago Mayor a 75%     " + (tot_rcv_mec) + "\nPago Menor a 75%     "+ (tot_menores) +
                                    "\nDiferencia > $500       " + tot_no_depurar + "\nPrimera Depuracion     " + b21 + "\n\nPagos Totales\t     " + ((dataGridView2.RowCount) - omitidos) + "\nSin Pago\t\t     " + omitidos +
                                    "\nPagos Previos\t     "+(duplicados-dupli_en)+"\nLineas Duplicadas\t "+dupli_en+"\n\nCasos Totales\t     "+dataGridView2.RowCount+"\nOmitidos\t      "+(tot_per_rcv_mec-dataGridView2.RowCount)+"\n\nTotal del Periodo\t     "+tot_per_rcv_mec);
			}else{
			 label21.Text="PAGO > 75%  ("+(0)+")\nPAGO < 75%  ("+(0)+")\nTOTAL: "+(0);	
			}
			
			if(total_rcv_mec>0){
				panel9.Enabled=true;
			}

            panel5.Visible = true;
            panel3.Visible = false;
			//MessageBox.Show(encont.ToString());
			/*if(tipo_cre==2){
			panel6.Visible=false;
			panel7.Visible=false;
			this.panel8.Location = new System.Drawing.Point(240, 110);
			this.panel9.Location = new System.Drawing.Point(240, 233);
			panel5.Visible=true;
			panel3.Visible=false;
			}
			
			if(tipo_cre==3){
			panel6.Visible=true;
			panel7.Visible=true;
			panel8.Visible=true;
			panel9.Visible=true;
			this.panel6.Location = new System.Drawing.Point(75, 110);
			this.panel7.Location = new System.Drawing.Point(75, 233);
			this.panel8.Location = new System.Drawing.Point(372, 110);
			this.panel9.Location = new System.Drawing.Point(372, 233);
			panel5.Visible=true;
			panel3.Visible=false;
			}*/
			                         }));
		}
		
		public void depurar(){
			try{
                String per="";
				k=0;
				por_dep=0;
				text_lista=textBox6.Lines;
				
				do{
					if(text_lista[k].Length>0){
					
						if(text_lista[k].Substring(4,11)=="COP_ECO_EST"){
							por_dep=por_dep+total_cop_est-1;
						}
						if(text_lista[k].Substring(4,11)=="COP_ECO_MEC"){
							por_dep=por_dep+total_cop_mec;
						}
						if(text_lista[k].Substring(4,11)=="RCV_ECO_EST"){
							por_dep=por_dep+total_rcv_est;
						}
						if(text_lista[k].Substring(4,11)=="RCV_ECO_MEC"){
							por_dep=por_dep+total_rcv_mec;
						}
					}
					
					//MessageBox.Show("1-."+text_lista[k]);
					k++;
				}while(k<text_lista.Length);
				
				//MessageBox.Show("1-."+text_lista.Length);
				//***DEPURAR EN LA BD
				k=0;
				coun=0;
                b18 = 0;
                b19 = 0;
                b20 = 0;
                b21 = 0;
                l=0;
				do{
					if(text_lista[k].Length>0){
						
						if(text_lista[k].Substring(4,11)=="COP_ECO_EST"){
							//MessageBox.Show(text_lista[k]);
								Invoke(new MethodInvoker(delegate
								                         {
								                         	text=textBox7.Lines;
								                         }));
								do{
								sql= text[l];
								conex.consultar(sql);
								l++;
								//MessageBox.Show(""+l);
								Invoke(new MethodInvoker(delegate
								                         {
								                         	progreso_depuracion();
								                         }));
							}while(l<(text.Length-1));
                            b18 = 1;
                            per = "COP_ECO_EST_" + periodo_sipare;
                            conex.guardar_evento("Se Afectaron: " + l + " registros y Se Depuraron: "+ev_tot_dep1+" créditos del periodo " + per+"");
						}
						l=0;
						
						if(text_lista[k].Substring(4,11)=="COP_ECO_MEC"){
							//MessageBox.Show(text_lista[k]);
								Invoke(new MethodInvoker(delegate
								                         {
								                         	text=textBox8.Lines;
								                         }));
							do{
								sql= text[l];
								conex.consultar(sql);
								l++;
								Invoke(new MethodInvoker(delegate
								                         {
								                         	progreso_depuracion();
								                         }));
							}while(l<(text.Length-1));
                            b19 = 1;
                            per = "COP_ECO_MEC_" + periodo_sipare;
                            conex.guardar_evento("Se Afectaron: " + l + " registros y Se Depuraron: " + ev_tot_dep2 + " créditos del periodo " + per + "");
						}
						
						l=0;
						if(text_lista[k].Substring(4,11)=="RCV_ECO_EST"){
							//MessageBox.Show(text_lista[k]);
								Invoke(new MethodInvoker(delegate
								                         {
								                         	text=textBox9.Lines;
								                         }));
							do{
								sql= text[l];
								conex.consultar(sql);
								l++;
								Invoke(new MethodInvoker(delegate
								                         {
								                         	progreso_depuracion();
								                         }));
							}while(l<(text.Length-1));
                            b20 = 1;
                            per = "RCV_ECO_EST_" + periodo_sipare_rcv;
                            conex.guardar_evento("Se Afectaron: " + l + " registros y Se Depuraron: " + ev_tot_dep3 + " créditos del periodo " + per + "");
						}
						
						l=0;
						if(text_lista[k].Substring(4,11)=="RCV_ECO_MEC"){
							//MessageBox.Show(text_lista[k]);
								Invoke(new MethodInvoker(delegate
								                         {
								                         	text=textBox10.Lines;
								                         }));
							do{
								sql= text[l];
								conex.consultar(sql);
								l++;
								Invoke(new MethodInvoker(delegate
								                         {
								                         	progreso_depuracion();
								                         }));
							}while(l<(text.Length-1));
                            b21 = 1;
                            per = "RCV_ECO_MEC_"+periodo_sipare_rcv;
                            conex.guardar_evento("Se Afectaron: " + l + " registros y Se Depuraron: " + ev_tot_dep4 + " créditos del periodo " + per + "");
						}
					}
					
					k++;
				}while(k<text_lista.Length);
				Invoke(new MethodInvoker(delegate
								                         {
								                         	progreso_depuracion();
								                         }));

				MessageBox.Show("El proceso ha concluido exitosamente","Proceso Terminado");
                

				Invoke(new MethodInvoker(delegate
				                         {
                                             
				                         	//label24.Visible=false;
				                         	//progressBar1.Visible=false;
				                         	panel11.Visible=true;
                                            progressBar1.Visible = false;
                                            label24.Visible = false;

                                            if (b18 == 1)
                                            {
                                                button18.Enabled = true;
                                            }
                                            else
                                            {
                                                button18.Enabled = false;
                                            }
                                            if (b19 == 1)
                                            {
                                                button19.Enabled = true;
                                            }
                                            else
                                            {
                                                button19.Enabled = false;
                                            }

                                            if (b20 == 1)
                                            {
                                                button20.Enabled = true;
                                            }
                                            else
                                            {
                                                button20.Enabled = false;
                                            }
                                            if (b21 == 1)
                                            {
                                                button21.Enabled = true;
                                            }
                                            else
                                            {
                                                button21.Enabled = false;
                                            }
				                         	
				                         }));
				}catch(Exception erro){
				MessageBox.Show(erro.ToString());
			}
			
		}
		
		public void progreso_depuracion(){
			//coun=l+coun;
            try
            {
                porcen = Convert.ToInt32((coun * 100) / por_dep);
                if (porcen >= 99)
                {
                    porcen = 100;
                }
                progressBar1.Value = porcen;
                progressBar1.Refresh();

                if (j == 10)
                {
                    label24.Text = porcen + "% Depurado";
                    label24.Refresh();
                }
                if (j == 20)
                {
                    label24.Text = porcen + "% Depurado.";
                    label24.Refresh();
                }
                if (j == 30)
                {
                    label24.Text = porcen + "% Depurado..";
                    label24.Refresh();
                }
                if (j == 40)
                {
                    label24.Text = porcen + "% Depurado...";
                    label24.Refresh();
                    j = 0;
                }
                j++;
                coun++;
            }
            catch (Exception ef)
            {
            }
		}
		
		public void llena_report(int num){

            //textBox5.Text = td_sql;
            //MessageBox.Show(textBox5.Text);
				//td_sql="80";3
					
				if(num==1){
					data_report1.Rows.Add(nrp_sql,cre_sql,periodo_sipare,textBox5.Text,sub_sql,imp_mul_sql,folio_pago,fecha_pripago);
					b18 = b18 + 1;
				}
				if(num==2){
					data_report2.Rows.Add(nrp_sql,cre_sql,periodo_sipare,textBox5.Text,sub_sql,imp_mul_sql,folio_pago,fecha_pripago);
					b19 = b19+ 1;
				}
				if(num==3){
					data_report3.Rows.Add(nrp_sql,cre_sql,periodo_sipare_rcv,textBox5.Text,sub_sql,imp_mul_sql,folio_pago,fecha_pripago);
					b20 = b20 + 1;
				}
				if(num==4){
					data_report4.Rows.Add(nrp_sql,cre_sql,periodo_sipare_rcv,textBox5.Text,sub_sql,imp_mul_sql,folio_pago,fecha_pripago);
					b21 = b21 + 1;
				}
					
		}
		
		public void cargar_archivos_prosip_gp(){
		
		try{
							Sel_archivo sel = new Sel_archivo();
							sel.ShowDialog(this);
							arch_sel = sel.mandar();

							if (arch_sel == 1)
							{
								MessageBox.Show("Seleccione el Archivo PROCESAR.", "Aviso");
								tipo_arch = 1;
								carga_excel();
								/*panel1.Visible=false;
	                			panel2.Visible=false;
	                			panel3.Visible=false;
	               				panel4.Visible=false;
	               				panel5.Visible=false;*/
                                MessageBox.Show("Seleccione el Archivo SIPARE.\nEl archivo debe contener solo los registros del periodo a depurar.", "Aviso");
								tipo_arch = 2;
								carga_excel();

							}
							else
							{
								if (arch_sel == 2){
									//MessageBox.Show("Seleccione la carpeta donde esten los archivos de GENERAL DE PAGOS", "Aviso");
									FolderBrowserDialog fbd = new FolderBrowserDialog();
									fbd.Description = "Selecciona la carpeta que contiene los archivos de GENERAL DE PAGOS";
									fbd.ShowNewFolderButton =false;
									DialogResult result = fbd.ShowDialog();
									int ultimo_punto=0,fin_cadena=0,xy=0;
									tot_gp=0;
									String archivo_gp_x,ext_arch;
									
									if(result == DialogResult.OK){
										archivos_tot = Directory.GetFiles(fbd.SelectedPath);
										
										do{
											ultimo_punto = archivos_tot[xy].LastIndexOf('.')+1;
											fin_cadena = (archivos_tot[xy].Length-ultimo_punto);
											archivo_gp_x = archivos_tot[xy].ToString();
											ext_arch= archivo_gp_x.Substring(ultimo_punto,fin_cadena);
											
											if(ext_arch == "xlsx"){
												tot_gp++;
											}
											
											if(ext_arch == "xls"){
												//archivos_gp[tot_gp] = archivo_gp_x;
												tot_gp++;
											}
											//MessageBox.Show("total de archivos excel: "+tot_gp+" archivo 1: "+archivos_tot[0].ToString(), "Aviso");
											xy++;
										}while(xy<archivos_tot.Length);
										
										xy=0;
										archivos_gp = new string[tot_gp];
										tot_gp=0;
										tot_archs ="";
                                        archs_gp_tb = "";
										
										do{
											ultimo_punto = archivos_tot[xy].LastIndexOf('.')+1;
											fin_cadena = (archivos_tot[xy].Length-ultimo_punto);
											archivo_gp_x = archivos_tot[xy].ToString();
											ext_arch= archivo_gp_x.Substring(ultimo_punto,fin_cadena);
											//MessageBox.Show("total de archivos excel del do: "+archivos_tot.Length+","+ext_arch+","+tot_gp, "Aviso");
											
											if(ext_arch == "xlsx"){
												archivos_gp[tot_gp] = archivo_gp_x;
												tot_archs +="• "+archivo_gp_x+"\n";
                                                archs_gp_tb += "• " +archivo_gp_x+ "\r\n";
												tot_gp++;
											}
											
											if(ext_arch == "xls"){
												archivos_gp[tot_gp] = archivo_gp_x;
												tot_archs +="• "+archivo_gp_x+"\n";
                                                archs_gp_tb += "• " + archivo_gp_x + "\r\n";
												tot_gp++;
											}
											//MessageBox.Show("total de archivos excel: "+tot_gp+" archivo 1: "+archivos_tot[0].ToString(), "Aviso");
											xy++;
										}while(xy<archivos_tot.Length);

										//MessageBox.Show("total de archivos excel: " + tot_gp + " archivo 7: " + archivos_gp[tot_gp - 1], "Aviso");
										carga_gp=0;
										
										if(archivos_gp.Length>0){
											DialogResult result1 = MessageBox.Show("Archivos Existentes En la Carpeta: " + archivos_tot.Length + "\nArchivos que se serán procesados: " +archivos_gp.Length+
										                                       "\n\nLos Datos de los Pagos serán extraídos de estos archivos: \n"+tot_archs+"\n\n¿Está seguro de querer cargar los archivos?",
										                                       "Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
											if(result1 == DialogResult.Yes){
												carga_gp=1;//carga_excel_general_pagos();
												label26.Visible=true;
												label26.Refresh();
												//MessageBox.Show(carga_gp.ToString());
											}else{
												carga_gp=0;
											}
										}else{
											MessageBox.Show("La Carpeta Seleccionada no contiene ningún archivo compatible.", "Aviso");
											carga_gp=0;
										}
									}
									
								}
							}
							
							if ((textBox1.Text.Length > 0) && (textBox2.Text.Length > 0)){
								button5.Enabled = true;
							}else{
								button5.Enabled = false;
							}
							
							if(tipo_doc_sel==2){
								if (textBox3.Text.Length > 0){
									button5.Enabled = true;
								}else{
									button5.Enabled = false;
								}
							}
							
							if(arch_sel == 2){
								
								if(carga_gp == 1){
									button5.Enabled = true;
								}else{
									button5.Enabled = false;
								}
							}
							//MessageBox.Show(carga_gp.ToString());
						}catch(Exception ee){
							
						}
		
		}
		
		public void boton_examinar(){
		
			seguro_per=0;
			tipo_doc_sel = comboBox3.SelectedIndex + 1;
			per_sel_full="";

			label5.Visible = false;
			label6.Visible = false;
			label26.Visible = false;
			label28.Visible = false;
			textBox1.Text = "";
			textBox2.Text = "";
			
			
			if(tipo_dep==1){//depu auto
				
				if(comboBox3.SelectedIndex == -1){
					seguro_per=2;
				}
				
				if((maskedTextBox1.Text.Length<7)){
					if(comboBox3.SelectedIndex == -1){
						seguro_per=2;
						//MessageBox.Show(tipo_doc_sel.ToString()+",|"+maskedTextBox1.Text+"|,"+seguro_per.ToString());
					}
					seguro_per=seguro_per+1;
				}
				
				//MessageBox.Show(tipo_doc_sel.ToString()+",|"+maskedTextBox1.Text+"|,"+seguro_per.ToString());
				
				if(seguro_per>0){
					switch(seguro_per){
							case 1:MessageBox.Show("Debe ingresar un periodo válido y con el formato indicado", "Aviso");
							break;
							case 2:MessageBox.Show("Debe seleccionar un tipo de documento", "Aviso");
							break;
							case 3:MessageBox.Show("Debe ingresar un periodo válido y con el formato indicado.\nDebe seleccionar un tipo de documento.", "Aviso");
							break;
					}
					seguro_per=0;
					
				}else{
					per_sel = maskedTextBox1.Text;
					//MessageBox.Show((per_sel.Substring(0,4)+"/"+per_sel.Substring(5,2)+"/01"));
					if(verificar_fecha((per_sel.Substring(0,4)+"/"+per_sel.Substring(5,2)+"/01")).Length>1)
					{
						periodo_sipare = (per_sel.Substring(0, 4)) + (per_sel.Substring(5, 2));
						comboBox3.Enabled=false;
						maskedTextBox1.Enabled=false;
						per_sel_full="";
						
						if(tipo_doc_sel==2){
							MessageBox.Show("Seleccione el Archivo RALE", "Aviso");
							carga_excel_rale();
                            button23.Enabled = false;
						}
						
						cargar_archivos_prosip_gp();
						
					}else{
						MessageBox.Show("Debe ingresar un periodo válido", "Aviso");
					}
				}

                
			}else{//depu manu
				
			    if(comboBox5.SelectedIndex == -1){
					seguro_per=seguro_per+1;
				}
			    
			    /*if(comboBox6.SelectedIndex == -1){
					seguro_per=seguro_per+2;
				}
				
				if((maskedTextBox2.Text.Length<7)){
					seguro_per=seguro_per+4;
				}*/
			    
				if(seguro_per>0){
					switch(seguro_per){
								case 1:MessageBox.Show("Debe seleccionar un Tipo de Periodo.", "Aviso");
								break;
								//case 2:MessageBox.Show("Debe seleccionar un Tipo de Registro.", "Aviso");
								//break;
								//case 3:MessageBox.Show("Debe seleccionar un Tipo de Periodo.\nDebe seleccionar un Tipo de Registro.", "Aviso");
								//break;
								//case 4:MessageBox.Show("Debe ingresar un periodo válido.", "Aviso");
								//break;
								//case 5:MessageBox.Show("Debe seleccionar un Tipo de Periodo.\nDebe ingresar un periodo válido.", "Aviso");
								//break;
								//case 6:MessageBox.Show("Debe seleccionar un Tipo de Registro.\nDebe ingresar un periodo válido.", "Aviso");
								//break;
								//case 7:MessageBox.Show("Debe seleccionar un Tipo de Periodo.\nDebe seleccionar un Tipo de Registro.\nDebe ingresar un periodo válido.", "Aviso");
								//break;
						}
			    	  seguro_per=0;
			    }else{
					
				    
				    //if((((Convert.ToInt32(per_sel.Substring(5,(per_sel.Length-5))))>0)&&((Convert.ToInt32(per_sel.Substring(5,(per_sel.Length-5))))<=12))&&(((Convert.ToInt32(per_sel.Substring(0,4)))>=2011)&&((Convert.ToInt32(per_sel.Substring(0,4)))<=2016)))
					//{
				    	comboBox5.Enabled=false;
						
				    	per_sel = comboBox5.SelectedItem.ToString();
				    	per_sel = per_sel.Substring(per_sel.Length-6,4)+"/"+per_sel.Substring(per_sel.Length-2,2);
						per_sel_full=comboBox5.SelectedItem.ToString();

                        periodo_sipare = (per_sel.Substring(0, 4)) + (per_sel.Substring(5, 2));
                        mes_periodo = Convert.ToInt32(periodo_sipare.Substring(periodo_sipare.Length - 2, 2));
                        
                        button6.Text = "     COP EST  " + periodo_sipare;
		                button23.Text = "     COP MEC  " + periodo_sipare;
		                button7.Text = "     RCV EST  " + periodo_sipare;
		                button22.Text = "     RCV MEC  " + periodo_sipare;
		                
                        if (per_sel_full.StartsWith("RCV"))
                        {
                            periodo_sipare_rcv = periodo_sipare;
                            mes_periodo = mes_periodo * 2;
                            if (mes_periodo <= 9)
                            {
                                periodo_sipare = periodo_sipare.Substring(0, 4) + "0" + mes_periodo.ToString();
                            }
                            else
                            {
                                periodo_sipare = periodo_sipare.Substring(0, 4) + mes_periodo.ToString();
                            }
                            
                            button6.Text = "     COP EST  " + periodo_sipare;
			                button23.Text = "     COP MEC  " + periodo_sipare;
			                button7.Text = "     RCV EST  " + periodo_sipare_rcv;
			                button22.Text = "     RCV MEC  " + periodo_sipare_rcv;
			                
			                label14.Text= "COP_ECO_EST_"+periodo_sipare;
            				label16.Text= "COP_ECO_MEC_"+periodo_sipare;
            				label18.Text= "RCV_ECO_EST_"+periodo_sipare_rcv;
        		    		label20.Text= "RCV_ECO_MEC_"+periodo_sipare_rcv;
                        }
						cargar_archivos_prosip_gp();
				    
				    //}else{
				    //	MessageBox.Show("Debe ingresar un periodo válido", "Aviso");
				    //}
			    }
			    
					
			}
		
		}
		
		public void cargar_depu_man(){
           int tipo_carga_dep_man = 0,cerrar=0;

		   if (tipo_dep == 2)
           {
		   	Invoke(new MethodInvoker(delegate
					             		{
                if (radioButton4.Checked==true){//Todo
                    tipo_carga_dep_man = 1;
                }
                
                if (radioButton5.Checked==true){//Pagados
                    tipo_carga_dep_man = 3;
                }
		   	                         }));
		   	
		   	if(radioButton6.Checked==true){// DEPURACION RB
		   		//per_sel_full= comboBox5.SelectedItem.ToString();
		   		per_sel_full="RB_SISCOB";
		   		tipo_carga_dep_man = 4;
		   	}
		   	//MessageBox.Show(per_sel_full+"|"+tipo_carga_dep_man);
                Depu_manu dep_man = new Depu_manu(per_sel_full, tipo_carga_dep_man);
                
                Invoke(new MethodInvoker(delegate
				{
                label22.Text="Seleccionando Registros a depurar...";
				label22.Refresh();                         
                this.Visible = false;
                }));
                
                dep_man.ShowDialog();
                            
                dep_man.mandar_lista_dep();

                if ((System.IO.File.Exists(@"temporal_rale.xlsx"))==true)
                {
                	cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
                	conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
                	conexion4.Open(); //abrimos la conexion
                	cons_exc = "Select * from [hoja_lz$] \"";
                	carga_excel3(); //registro existe en el RALE?? o en la DEP_MANUAL
                	
                	if(dataGridView5.RowCount > 0){
                		if(dataGridView5.Rows[0].Cells[0].FormattedValue.ToString().Length <= 0){
	                		MessageBox.Show("Imposible continuar con la depuración.","AVISO");
	                		button8.Enabled = false;
	                		button6.Enabled = false;
	                		button7.Enabled = false;
	                		button22.Enabled = false;
	                		button23.Enabled = false;
	                		cerrar=1;
                		}
                	}
                	
                	conexion4.Close();
                }
                else
                {
                    MessageBox.Show("Imposible continuar con la depuración.","AVISO");
                    button8.Enabled = false;
                    button6.Enabled = false;
                    button7.Enabled = false;
                    button22.Enabled = false;
                    button23.Enabled = false;
                   cerrar=1;
                }
                //MessageBox.Show(""+cerrar);
                if(cerrar==1){
                	this.Hide();
                }
                try{
                Invoke(new MethodInvoker(delegate
				{
                	this.Visible = true;
                	if(cerrar==1){
                		this.Hide();
                	}
                }));
                }catch{
                	this.Hide();
                }
                //MessageBox.Show("Depurar: "+data_rale.Rows.Count+" Casos");
                
				tipo_doc_sel=3;
            }		
		}
		
		public void llenar_Cb5(){
			conex.conectar("base_principal");
			comboBox5.Items.Clear();
			i=0;
			dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE nombre_periodo LIKE \"%_ECO_%\" ORDER BY nombre_periodo;");
			do{
				comboBox5.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView2.RowCount);
			i=0;
		}

        public void clasificar_periodo() {
    
            if (per_sel_full != "")
            {
                switch ((per_sel_full.Substring(0, 11)))//DEPU MANUAL
                {
                    case "COP_ECO_EST": button6.Enabled = true;
                        button23.Enabled = false;
                        button7.Enabled = false;
                        button22.Enabled = false;
                        textBox5.Text="80";
                        break;

                    case "COP_ECO_MEC": button6.Enabled = false;
                        button23.Enabled = true;
                        button7.Enabled = false;
                        button22.Enabled = false;
                        textBox5.Text="80";
                        break;

                    case "RCV_ECO_EST": button6.Enabled = false;
                        button23.Enabled = false;
                        button7.Enabled = true;
                        button22.Enabled = false;
                        textBox5.Text="81";
                        break;

                    case "RCV_ECO_MEC": button6.Enabled = false;
                        button23.Enabled = false;
                        button7.Enabled = false;
                        button22.Enabled = true;
                        textBox5.Text="81";
                        break;
                }

            }
            else
            {//DEPU AUTO
                mes_periodo = Convert.ToInt32(periodo_sipare.Substring(periodo_sipare.Length - 2, 2));
                if ((mes_periodo == 1) || (mes_periodo == 3) || (mes_periodo == 5) || (mes_periodo == 7) || (mes_periodo == 9) || (mes_periodo == 11))
                {
                    button7.Enabled = false;
                    button22.Enabled = false;
                    
                    button6.Text = "     COP EST  " + periodo_sipare;
	                button23.Text = "     COP MEC  " + periodo_sipare;
	                button7.Text = "     RCV EST  " + periodo_sipare;
	                button22.Text = "     RCV MEC  " + periodo_sipare;
                }
                else
                {
                    mes_periodo = mes_periodo / 2;
                    
                    periodo_sipare_rcv = periodo_sipare.Remove(periodo_sipare.Length - 2, 2);
	                if (mes_periodo <= 9)
	                {
	                    periodo_sipare_rcv = periodo_sipare_rcv + "0" + mes_periodo.ToString();
	                }
	                else
	                {
	                    periodo_sipare_rcv = periodo_sipare_rcv + mes_periodo.ToString();
	                }
	
	                button6.Text = "     COP EST  " + periodo_sipare;
	                button23.Text = "     COP MEC  " + periodo_sipare;
	                button7.Text = "     RCV EST  " + periodo_sipare_rcv;
	                button22.Text = "     RCV MEC  " + periodo_sipare_rcv;
	                
	                label14.Text= "COP_ECO_EST_"+periodo_sipare;
            		label16.Text= "COP_ECO_MEC_"+periodo_sipare;
            		label18.Text= "RCV_ECO_EST_"+periodo_sipare_rcv;
        		    label20.Text= "RCV_ECO_MEC_"+periodo_sipare_rcv;

                }
            }
           // MessageBox.Show(periodo_sipare);
           // MessageBox.Show(periodo_sipare_rcv);
        }
		
		public void separar_sipare(){
			
		 int q=0,v=0;
		 String nrp2,f_sua2,fecha_pago2,i4ss2,comp_dupli2;
		 
		 DataTable tabla_sep_sip = new DataTable();//En Uso
		 tabla_sep_sip.Columns.Add();
		 
		 while(q<dataGridView1.RowCount){
		 	nrp = dataGridView1.Rows[q].Cells[0].FormattedValue.ToString();
            f_sua = dataGridView1.Rows[q].Cells[1].FormattedValue.ToString();
            fecha_pago = dataGridView1.Rows[q].Cells[2].FormattedValue.ToString();
            i4ss = dataGridView1.Rows[q].Cells[3].FormattedValue.ToString();

            comp_dupli = nrp + "_" + f_sua + "_" + fecha_pago + "_" + i4ss;
            
            if((q+1)<dataGridView1.RowCount){
            	nrp2 = dataGridView1.Rows[q+1].Cells[0].FormattedValue.ToString();
            	f_sua2 = dataGridView1.Rows[q+1].Cells[1].FormattedValue.ToString();
         	    fecha_pago2 = dataGridView1.Rows[q+1].Cells[2].FormattedValue.ToString();
           		i4ss2 = dataGridView1.Rows[q+1].Cells[3].FormattedValue.ToString();
           		
           		comp_dupli2 = nrp2 + "_" + f_sua2 + "_" + fecha_pago2 + "_" + i4ss2;
           		
           		if(comp_dupli.Equals(comp_dupli2)){
           			if(f_sua.Length > 9){
           					tablarow4.Rows.Add(nrp,f_sua,fecha_pago,i4ss);
           			}
           			tabla_sep_sip.Rows.Add((q+1));
           		}else{
           			if(i4ss.Equals(i4ss2)){
           				if(f_sua.Length > 9){
           					tablarow4.Rows.Add(nrp,f_sua,fecha_pago,i4ss);
           					tabla_sep_sip.Rows.Add((q));
           				}
           				
           				if(f_sua2.Length > 9){
           					tablarow4.Rows.Add(nrp2,f_sua2,fecha_pago2,i4ss2);
           					tabla_sep_sip.Rows.Add((q+1));
           				}
           				
           			}
           		}
            }
		 	
		 	q++;
		 }
		 
		 v=tabla_sep_sip.Rows.Count-1;
		// MessageBox.Show("borrados: "+v);
		 while(v >= 0){
		 	dataGridView1.Rows.RemoveAt(Convert.ToInt32(tabla_sep_sip.Rows[v][0].ToString()));
		 	v=v-1;
		 }
		 
		}
		
		public String verificar_fecha(String fecha){
			
			DateTime fecha_not,fecha_min;
			TimeSpan dif_fecha;
			
			if(DateTime.TryParse(fecha,out fecha_not)){
				
				fecha_min=System.DateTime.Today.Date;
				dif_fecha=fecha_min.Subtract(fecha_not);
				//MessageBox.Show(fecha+"|"+dif_fecha);
				if(fecha_not <= System.DateTime.Today && dif_fecha.Days <= 1826 ){
					return fecha_not.ToShortDateString();
				}else{
					return "0";
				}
			}else{
				return "0";
			}
		}

		public void rb_depu(){
			String sqlz,td_sel="",importe="";
			int i_rb=0,tipo_repor=0;
			//MessageBox.Show(dataGridView5.RowCount.ToString());
			if ((System.IO.File.Exists(@"temporal_rale.xlsx"))==true)
			{
				cad_con3 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_rale.xlsx';Extended Properties=Excel 12.0;";
				conexion4 = new OleDbConnection(cad_con3);//creamos la conexion con la hoja de excel
				conexion4.Open(); //abrimos la conexion
				cons_exc = "Select * from [hoja_lz$] \"";
				carga_excel3();
				conexion4.Close();
			}
			
			if(textBox5.Text.Equals("02")){
				td_sel="credito_cuotas";
				importe="importe_cuota";
				
			}
			
			if(textBox5.Text.Equals("80")){
				td_sel="credito_multa";
				importe="importe_multa";
				
			}
			
			if(textBox5.Text.Equals("06")){
				td_sel="credito_cuotas";
				importe="importe_cuota";
				
			}
			
			if(textBox5.Text.Equals("81")){
				td_sel="credito_multa";
				importe="importe_multa";
				
			}
			
			/*if(per_sel_full.Substring(0,3).Equals("COP")){
				if(per_sel_full.Substring(8,3).Equals("EST")){
					tipo_repor=1;
				}else{
					tipo_repor=2;
				}
			}else{
				if(per_sel_full.Substring(8,3).Equals("EST")){
					tipo_repor=3;
				}else{
					tipo_repor=4;
				}
			}
			*/
			  tipo_repor=1;
			if(td_sel.Length>3){
				button10.Enabled=false;
				conex.conectar("base_principal");
				conex1.conectar("base_principal");
				data_report1.Rows.Clear();
				
				do{
					conex1.consultar("UPDATE datos_factura SET observaciones=\"SE DEPURA CREDITO POR PRESENTAR RB EN SISCOB, CORE ELABORADA EL: "+System.DateTime.Today.ToShortDateString()+" A LAS "+System.DateTime.Now.ToShortTimeString()+" CONTENIENDO "+dataGridView5.RowCount+" CASOS\",fecha_depuracion=\""+System.DateTime.Today.ToShortDateString()+"\",status=\"DEPURACION MANUAL\""+
					                 "WHERE registro_patronal2 =\""+dataGridView5.Rows[i_rb].Cells[0].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView5.Rows[i_rb].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView5.Rows[i_rb].Cells[2].Value.ToString()+"\"");
					                 
					/*data_report1.Rows.Add(dataGridView3.Rows[0].Cells[0].Value.ToString(),
					                      dataGridView3.Rows[0].Cells[1].Value.ToString(),
					                      dataGridView3.Rows[0].Cells[2].Value.ToString(),
					                      textBox5.Text," 38",
					                      dataGridView3.Rows[0].Cells[3].Value.ToString()," RB","");
					                 */
					                 i_rb++;
				}while(i_rb < dataGridView5.RowCount);
				
				i_rb=0;
				sqlz="SELECT registro_patronal2,"+td_sel+",periodo,"+importe+" FROM datos_factura WHERE observaciones=\"SE DEPURA CREDITO POR PRESENTAR RB EN SISCOB, CORE ELABORADA EL: "+System.DateTime.Today.ToShortDateString()+" A LAS "+System.DateTime.Now.ToShortTimeString()+" CONTENIENDO "+dataGridView5.RowCount+" CASOS\"";
				tablarow=conex.consultar(sqlz);
				dataGridView3.DataSource=tablarow;
				
				do{
					data_report1.Rows.Add(dataGridView3.Rows[i_rb].Cells[0].Value.ToString(),
					                      dataGridView3.Rows[i_rb].Cells[1].Value.ToString(),
					                      dataGridView3.Rows[i_rb].Cells[2].Value.ToString(),
					                      " "," 38",
					                      dataGridView3.Rows[i_rb].Cells[3].Value.ToString()," RB","");
					
					i_rb++;
				}while(i_rb < tablarow.Rows.Count);
				
				Visor_Reporte visor1 = new Visor_Reporte();
				visor1.envio(data_report1,solicita,autoriza);
				visor1.Show();
			
			}else{
				MessageBox.Show("El Tipo de Documento Ingresado no es válido ","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}
		
		void DepuracionLoad(object sender, EventArgs e)
		{
			//radioButton1.Checked=true;
			data_3.Columns.Add("col1");
            data_3.Columns.Add("col2");	

            tipo_cadena = "hola".GetType();    
            
            tablarow2.Columns.Add("NRP",tipo_cadena);
            tablarow2.Columns.Add("F#SUA",tipo_cadena);
            tablarow2.Columns.Add("FECHA PAGO",tipo_cadena);
            tablarow2.Columns.Add("4SS",porcentaje_min_pago.GetType());
            tablarow2.Columns.Add("RCV",porcentaje_min_pago.GetType());
            
            tablarow3.Columns.Add("NRP",tipo_cadena);
            tablarow3.Columns.Add("F#SUA",tipo_cadena);
            tablarow3.Columns.Add("FECHA PAGO",tipo_cadena);
            tablarow3.Columns.Add("4SS",porcentaje_min_pago.GetType());
            tablarow3.Columns.Add("RCV",porcentaje_min_pago.GetType());
            tablarow3.Columns.Add("PERIODO_PAGO",tipo_cadena);
            tablarow3.Columns.Add("ARCHIVO_ORIGEN",tipo_cadena);
            
            tablarow4.Columns.Add("NRP",tipo_cadena);
            tablarow4.Columns.Add("F#SUA",tipo_cadena);
            tablarow4.Columns.Add("FECHA PAGO",tipo_cadena);
            tablarow4.Columns.Add("4SS",porcentaje_min_pago.GetType());
            tablarow4.Columns.Add("RCV",porcentaje_min_pago.GetType());
            tablarow4.Columns.Add("PERIODO_PAGO",tipo_cadena);
            tablarow4.Columns.Add("ARCHIVO_ORIGEN",tipo_cadena);
            
            //datos reporte COP_EST
            data_report1.Columns.Add("RP");
            data_report1.Columns.Add("CREDITO");
            data_report1.Columns.Add("PERIODO");
            data_report1.Columns.Add("TD");   
			//data_report1.Columns.Add("INC");              
            data_report1.Columns.Add("SUB");
            data_report1.Columns.Add("IMPORTE");
            data_report1.Columns.Add("FOLIO");
            data_report1.Columns.Add("FECHA"); 
            
             //datos reporte COP_MEC
            data_report2.Columns.Add("RP");
            data_report2.Columns.Add("CREDITO");
            data_report2.Columns.Add("PERIODO");
            data_report2.Columns.Add("TD");  
 			//data_report2.Columns.Add("INC");            
            data_report2.Columns.Add("SUB");
           	data_report2.Columns.Add("IMPORTE");
            data_report2.Columns.Add("FOLIO");
            data_report2.Columns.Add("FECHA"); 
            
             //datos reporte RCV_EST
            data_report3.Columns.Add("RP");
            data_report3.Columns.Add("CREDITO");
            data_report3.Columns.Add("PERIODO");
            data_report3.Columns.Add("TD");  
			//data_report3.Columns.Add("INC");            
            data_report3.Columns.Add("SUB");
            data_report3.Columns.Add("IMPORTE");
            data_report3.Columns.Add("FOLIO");
            data_report3.Columns.Add("FECHA"); 
            
             //datos reporte RCV_MEC
            data_report4.Columns.Add("RP");
            data_report4.Columns.Add("CREDITO");
            data_report4.Columns.Add("PERIODO");
            data_report4.Columns.Add("TD"); 
            //data_report4.Columns.Add("INC");
            data_report4.Columns.Add("SUB");
            data_report4.Columns.Add("IMPORTE");
            data_report4.Columns.Add("FOLIO");
            data_report4.Columns.Add("FECHA"); 
            
            //DATOS RALE
            data_rale.Columns.Add("NRP");
            data_rale.Columns.Add("IMPORTE");
            
            dateTimePicker1.Value = System.DateTime.Today;
            dateTimePicker1.MaxDate = System.DateTime.Today;
            
           /* 
            //datos confirma dep
            data_depura1.Columns.Add("RP");
            data_depura1.Columns.Add("CRED");
            data_depura1.Columns.Add("imp_cuo");
            data_depura1.Columns.Add("imp_pag");
            data_depura1.Columns.Add("folio");
            data_depura1.Columns.Add("sidep");
            data_depura1.Columns.Add("statu");
            data_depura1.Columns.Add("id");
            data_depura1.Columns.Add("fecha");
            data_depura1.Columns.Add("num_pago");
            */
            //MessageBox.Show(tipo_cadena.FullName.ToString());
		}
		//boton panel1 - panel2 (paso 1-2)
		void Button1Click(object sender, EventArgs e)
		{
			if((radioButton1.Checked==true)||(radioButton2.Checked==true)){
				panel2.Visible=true;
				panel1.Visible=false;
				
				if(radioButton1.Checked==true){//Depuracion Automática
					tipo_dep=1;
					panel12.Visible=false;
					label4.Text="Ajustes de la Depuración";
				}
				
				if(radioButton2.Checked==true){//Depuracion Manual
					tipo_dep=2;
					panel12.Visible=true;
					label4.Text="Ajustes de la Depuración Manual";
					llenar_Cb5();
					timer1.Enabled=true;
				}
				
				if((textBox1.Text.Length>0)&&(textBox2.Text.Length>0)){
					button5.Enabled=true;
				}else{
					button5.Enabled=false;
				}
			}else{
				MessageBox.Show("Selecciona una opción.");
			}
		}
		//Examinar-----------
		void Button2Click(object sender, EventArgs e)
		{
			boton_examinar();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			
		}
		//anterior panel2 - panel1
		void Button4Click(object sender, EventArgs e)
		{
			panel2.Visible=false;
			panel1.Visible=true;
			
			comboBox3.SelectedIndex=-1;
			maskedTextBox1.Clear();
			comboBox3.Enabled=true;
			maskedTextBox1.Enabled=true;
			panel12.Visible=false;
			
			//Depuración Manual
			panel12.Visible=false;
			comboBox5.SelectedIndex=-1;
			comboBox5.Enabled=true;
			comboBox6.SelectedIndex=-1;
			comboBox6.Enabled=true;
			maskedTextBox2.Clear();
			maskedTextBox2.Enabled=true;
		
		}
		//siguiente panel2-panel2 (paso 2-3) - proceso RB
		void Button5Click(object sender, EventArgs e)
		{
            try
            {
                //cargar_hoja_excel_procesar();
                //cargar_hoja_excel_sipare();
                label22.Visible = true;

                //td_sel = comboBox3.SelectedItem.ToString();
                fecha_not_dep= dateTimePicker1.Value.ToShortDateString().Substring(6,4)+"-"+dateTimePicker1.Value.ToShortDateString().Substring(3,2)+"-"+dateTimePicker1.Value.ToShortDateString().Substring(0,2);
                clasificar_periodo();
                //combinar_procesar_sipare();
                if (arch_sel == 1)
                {
                    hilosecundario = new Thread(new ThreadStart(combinar_procesar_sipare));
                    hilosecundario.Start();
                }
                //general de pagos
                if (arch_sel == 2)
                {
                    button7.Enabled = false;
                    button22.Enabled = false;
                    hilosecundario = new Thread(new ThreadStart(cargar_archivos_general_pagos));
                    hilosecundario.Start();
                }

                //cargar rale
                if (tipo_doc_sel == 2)
                {
                    hiloterciario = new Thread(new ThreadStart(cargar_hoja_excel_rale));
                    hiloterciario.Start();
                    button6.Text = "DEPURAR\nRALE";
                }
            }catch(Exception exep){
            	if(radioButton6.Checked==true){
            		cargar_depu_man();
            		panel4.Visible=true;
            		button11.Enabled=false;
 					panel2.Visible = false;
 					confirmar_datos();
					textBox4.Text="• LISTADO RB";
					textBox5.Text="80";
            		textBox6.Text="• "+per_sel_full; 					
            	}else{
            	
                	MessageBox.Show("Ha ocurrido un error al leer los archivos, reviselos y vuelva a intentar.\n\nError:\n"+exep);
            	}
            }

		}
		//boton COP
		void Button6Click(object sender, EventArgs e)
		{
			if(this.button6.BackColor == System.Drawing.Color.Transparent){
			this.button6.BackColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49))))); //System.Drawing.Color.Transparent;
			this.button6.FlatAppearance.BorderSize = 1;
			tipo_cre=tipo_cre+1;
			button8.Enabled=true;
			
			}else{
			this.button6.BackColor = System.Drawing.Color.Transparent;
			this.button6.FlatAppearance.BorderSize = 0;
			tipo_cre=tipo_cre-1;
			if(tipo_cre==0){
					button8.Enabled=false;
				}
			}
			//this.button6.ForeColor = System.Drawing.Color.White;	
		}
		//boton RCV
		void Button7Click(object sender, EventArgs e)
		{
			if(this.button7.BackColor == System.Drawing.Color.Transparent){
			this.button7.BackColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49))))); //System.Drawing.Color.Transparent;
			this.button7.FlatAppearance.BorderSize = 1;
			tipo_cre=tipo_cre+3;
			button8.Enabled=true;
			}else{
				this.button7.BackColor = System.Drawing.Color.Transparent;
				this.button7.FlatAppearance.BorderSize = 0;
				tipo_cre=tipo_cre-3;
				if(tipo_cre==0){
					button8.Enabled=false;
				}
			}
		}
		//boton anterior panel3-panel2 
		void Button9Click(object sender, EventArgs e)
		{
			panel3.Visible=false;
			panel2.Visible=true;
            label5.Visible=true;
			label6.Visible=true;   
			textBox1.Text="";
			textBox2.Text="";
			label5.Visible=false;
			label6.Visible=false;
            button8.Enabled = true;
			if((textBox1.Text.Length>0)&&(textBox2.Text.Length>0)){
			button5.Enabled=true;
			}else{
				button5.Enabled=false;
			}
			
			//dataGridView2.DataSource=data_vacio;
			//dataGridView3.DataSource=data_vacio;
			
		}
		//boton siguiente panel3-panel5 (paso 3-4)
		void Button8Click(object sender, EventArgs e)
		{
            if(tipo_doc_sel == 2){
                textBox5.Text = "02";
            }else{
                if(tipo_doc_sel == 1){
                    if(tipo_cre == 1){
                        textBox5.Text = "80";
                    }else{
                        textBox5.Text = "81";
                    }
                }else{
                    if (tipo_doc_sel == 3){
                        textBox5.Text = "06";
                    }
                }
            }

            if(tipo_doc_sel==2){
                label23.Visible = true;
                procesar_rale();
            }else{

                conex.conectar("base_principal");
			    label23.Visible=true;
			    fecha_dep=System.DateTime.Today.ToShortDateString();
			    fecha_dep=fecha_dep.Substring(fecha_dep.Length-4,4)+"-"+fecha_dep.Substring(3,2)+"-"+fecha_dep.Substring(0,2);
			    //MessageBox.Show(fecha_dep);
                b18 = 0;
                b19 = 0;
                b20 = 0;
                b21 = 0;
            
                textBox9.Text="";
			    textBox10.Text="";

			    /*if(tipo_cre==1){
				    consultar_COP();
			    }else{
			        if(tipo_cre==2){
					    consultar_RCV();
				    }else{
					    consultar_doble_COP_RCV();
				    }
			    }*/
			    //label13.Text=tipo_cre.ToString();

                label15.Text = "PAGO > 75%  (" + (0) + ")\nPAGO < 75%  (" + (0) + ")\nTOTAL: " + (0);
                label17.Text = "PAGO > 75%  (" + (0) + ")\nPAGO < 75%  (" + (0) + ")\nTOTAL: " + (0);
                label19.Text = "PAGO > 75%  (" + (0) + ")\nPAGO < 75%  (" + (0) + ")\nTOTAL: " + (0);
                label21.Text = "PAGO > 75%  (" + (0) + ")\nPAGO < 75%  (" + (0) + ")\nTOTAL: " + (0);

                panel6.Enabled = false;
                panel7.Enabled = false;
                panel8.Enabled = false;
                panel9.Enabled = false;

                if (this.button6.BackColor != System.Drawing.Color.Transparent) 
                {
                    lanzador_COP_EST();
                }

                if (this.button23.BackColor != System.Drawing.Color.Transparent)
                {
                    lanzador_COP_MEC();
                }

                if (this.button7.BackColor != System.Drawing.Color.Transparent)
                {
                    lanzador_RCV_EST();
                }

                if (this.button22.BackColor != System.Drawing.Color.Transparent)
                {
                    lanzador_RCV_MEC();
                }
            
            }
           
		}
		//boton anterior panel4-panel5
		void Button11Click(object sender, EventArgs e)
		{
			panel4.Visible=false;
            panel5.Visible = true;
            textBox6.Text="";
            textBox4.Text="";
           /* activo_1=0;
			activo_2=0;
			activo_3=0;
			activo_4=0;
			this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;*/
		}
		//boton  anterior panel5-panel3
 		void Button13Click(object sender, EventArgs e)
		{
			panel3.Visible = true;
            panel5.Visible = false;		
			label23.Text="";
			label23.Visible=false;
			activo_1=0;
			activo_2=0;
			activo_3=0;
			activo_4=0;
			this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
			tipo_cre=0;
			this.button6.BackColor = System.Drawing.Color.Transparent;
			this.button6.FlatAppearance.BorderSize = 0;
			this.button7.BackColor = System.Drawing.Color.Transparent;
			this.button7.FlatAppearance.BorderSize = 0;
			textBox7.Text="";
			textBox8.Text="";
			textBox9.Text="";
			textBox10.Text="";
			
		}
		//boton siguiente panel5-panel4 (paso 4-5)
 		void Button12Click(object sender, EventArgs e)
 		{
 			panel4.Visible=true;
 			panel5.Visible = false;
 			confirmar_datos();	
 		}
		//boton siguiente panel4-fondo paso(5-6) DEPURAR***
		void Button10Click(object sender, EventArgs e)
		{
			/*panel1.Visible=false;
			panel2.Visible=false;
			panel3.Visible=false;
			panel4.Visible=false;
			panel5.Visible=false;	
			*/
			respuesta = MessageBox.Show("Este proceso ingresará información a la base de datos.\nEsta Acción no se podrá revertir.\n\n¿Esta seguro que desea continuar a la depuración?  ","Advertencia",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(respuesta== DialogResult.Yes){
				if(radioButton6.Checked==false){
					/*textBox3.Enabled=false;
					textBox4.Enabled=false;
					textBox5.Enabled=false;
					textBox6.Enabled=false;*/
					button10.Enabled=false;
					button11.Enabled=false;
					label24.Visible=true;
					progressBar1.Visible=true;
					progressBar1.Value=0;
					//MessageBox.Show("1-."+total_cop_est+" 2.-"+total_cop_mec+" 3.-"+total_rcv_est+" 4.-"+total_rcv_mec);
					button18.Text="  CORE\n  COP ECO EST    \n  "+label14.Text.Substring(label14.Text.Length-6,6);
					button19.Text="  CORE\n  COP ECO MEC    \n  "+label16.Text.Substring(label16.Text.Length-6,6);
					button20.Text="  CORE\n  RCV ECO EST    \n  "+label18.Text.Substring(label18.Text.Length-6,6);
					button21.Text="  CORE\n  RCV ECO MEC    \n  "+label20.Text.Substring(label20.Text.Length-6,6);
					button18.Enabled=panel6.Enabled;
					button19.Enabled=panel7.Enabled;
					button20.Enabled=panel8.Enabled;
					button21.Enabled=panel9.Enabled;
					hilosecundario = new Thread(new ThreadStart(depurar));
					hilosecundario.Start();
					//depurar();
					XLWorkbook wz = new XLWorkbook();
			 		wz.Worksheets.Add(tablarow4,"hoja_lz");
			 		wz.SaveAs(@"temporal_excluidos.xlsx");
				}else{
					//button10.Enabled=false;
					rb_depu();
				}
			}else{
				MessageBox.Show("El proceso no se iniciará, puede volver y modificar la información previa","Aviso");
			}
		}
		
		void Panel3Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton1.Checked==true){
				button1.Enabled=true;
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton2.Checked==true){
				button1.Enabled=true;
			}
		}
		
		void RadioButton3CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton3.Checked==true){
				button1.Enabled=true;
			}
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			activo_1++;
			
			if(activo_1 > 1){
				
				if(pila<valor_tomado1){
				}else{
					if(pila==valor_tomado1){
						pila--;
						valor_tomado1=0;
						activo_1=0;
						this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
					}
				}
				
			}else{
				pila++;
				switch(pila){
						case 1: this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_1;
						valor_tomado1=1;
						break;
						case 2: this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_2;
						valor_tomado1=2;
						break;
						case 3: this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_3;
						valor_tomado1=3;
						break;
						case 4: this.button14.Image = global::Nova_Gear.Properties.Resources.check_box_4;
						valor_tomado1=4;
						break;
						
						default: break;
				}
				
			}
			//label13.Text="Pila:"+pila;
			if(pila>=1){
				button12.Enabled=true;
			}else{
				button12.Enabled=false;
			}
		}
		
		void Button15Click(object sender, EventArgs e)
		{
			activo_2++;
			if(activo_2 > 1){
				
				if(pila < valor_tomado2){
				}else{
					if(pila==valor_tomado2){
						pila--;
						valor_tomado2=0;
						activo_2 = 0;
						this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
					}
				}
			}else{
				pila++;
				switch(pila){
						case 1: this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_1;
						valor_tomado2=1;
						break;
						case 2: this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_2;
						valor_tomado2=2;
						break;
						case 3: this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_3;
						valor_tomado2=3;
						break;
						case 4: this.button15.Image = global::Nova_Gear.Properties.Resources.check_box_4;
						valor_tomado2=4;
						break;
						
						default: break;
				}
			}
			//label13.Text="Pila:"+pila;
			if(pila>=1){
				button12.Enabled=true;
			}else{
				button12.Enabled=false;
			}
		}
		
		void Button16Click(object sender, EventArgs e)
		{
			
			activo_3++;
			if(activo_3 > 1){
				
				if(pila < valor_tomado3){
				}else{   
					if(pila==valor_tomado3){
						pila--;
						valor_tomado3=0;
						activo_3 = 0;
						this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
					}
				}
			}else{
				pila++;
				switch(pila){
						case 1: this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_1;
						valor_tomado3=1;
						break;
						case 2: this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_2;
						valor_tomado3=2;
						break;
						case 3: this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_3;
						valor_tomado3=3;
						break;
						case 4: this.button16.Image = global::Nova_Gear.Properties.Resources.check_box_4;
						valor_tomado3=4;
						break;
						
						default: break;
				}
				
			}
			//label13.Text="Pila:"+pila;
			if(pila>=1){
				button12.Enabled=true;
			}else{
				button12.Enabled=false;
			}
		}
		
		void Button17Click(object sender, EventArgs e)
		{
		
			activo_4++;
			if(activo_4 > 1){
				
				if(pila < valor_tomado4){
				}else{
					if(pila==valor_tomado4){
						pila--;
						valor_tomado4=0;
						activo_4 = 0;
						this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_uncheck;
					}
				}
			}else{
				pila++;
				switch(pila){
						case 1: this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_1;
						valor_tomado4=1;
						break;
						case 2: this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_2;
						valor_tomado4=2;
						break;
						case 3: this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_3;
						valor_tomado4=3;
						break;
						case 4: this.button17.Image = global::Nova_Gear.Properties.Resources.check_box_4;
						valor_tomado4=4;
						break;
						
						default: break;
				}
			}
			//label13.Text="Pila:"+pila;
			
			if(pila>=1){
				button12.Enabled=true;
			}else{
				button12.Enabled=false;
			}
		}

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
		
		void Label15MouseEnter(object sender, EventArgs e)
		{
			toolTip1.Active=true;
		}
		
		void Label15MouseLeave(object sender, EventArgs e)
		{
			toolTip1.Active=false;
		}
		//generacion de reportes --COP EST
		void Button18Click(object sender, EventArgs e)
		{
			Visor_Reporte visor1 = new Visor_Reporte();
			visor1.envio(data_report1,solicita,autoriza);
			visor1.Show();
		}
        //generacion de reportes --COP MEC
		void Button19Click(object sender, EventArgs e)
		{
			Visor_Reporte visor2 = new Visor_Reporte();
            visor2.envio(data_report2, solicita, autoriza);
			visor2.Show();
		}
        //generacion de reportes --RCV EST
		void Button20Click(object sender, EventArgs e)
		{
			Visor_Reporte visor3 = new Visor_Reporte();
            visor3.envio(data_report3, solicita, autoriza);
			visor3.Show();
		}
        //generacion de reportes --RCV MEC
		void Button21Click(object sender, EventArgs e)
		{
			Visor_Reporte visor4 = new Visor_Reporte();
            visor4.envio(data_report4, solicita, autoriza);
			visor4.Show();
		}
        
        private void button22_Click(object sender, EventArgs e)
        {
            if (this.button22.BackColor == System.Drawing.Color.Transparent)
            {
                this.button22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49))))); //System.Drawing.Color.Transparent;
                this.button22.FlatAppearance.BorderSize = 1;
                tipo_cre = tipo_cre + 3;
                button8.Enabled = true;
            }
            else
            {
                this.button22.BackColor = System.Drawing.Color.Transparent;
                this.button22.FlatAppearance.BorderSize = 0;
                tipo_cre = tipo_cre - 3;
                if (tipo_cre == 0)
                {
                    button8.Enabled = false;
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (this.button23.BackColor == System.Drawing.Color.Transparent)
            {
                this.button23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49))))); //System.Drawing.Color.Transparent;
                this.button23.FlatAppearance.BorderSize = 1;
                tipo_cre = tipo_cre + 1;
                button8.Enabled = true;
            }
            else
            {
                this.button23.BackColor = System.Drawing.Color.Transparent;
                this.button23.FlatAppearance.BorderSize = 0;
                tipo_cre = tipo_cre - 1;
                if (tipo_cre == 0)
                {
                    button8.Enabled = false;
                }
            }
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }
		
		void Timer1Tick(object sender, EventArgs e)
		{
			/*int valido=0;
			if(radioButton6.Checked==true){
				if(comboBox5.Enabled==false){
					button5.Enabled=true;
				}
			}*/
		}
		
		void RadioButton6CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton6.Checked==true){
				comboBox5.Enabled=false;
				button5.Enabled=true;
			}else{
				comboBox5.Enabled=true;
				button5.Enabled=false;
			}
		}
	}
}

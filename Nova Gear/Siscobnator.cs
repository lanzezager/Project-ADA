/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 10/06/2015
 * Hora: 11:37 a. m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Siscobnator.
	/// </summary>
	public partial class Siscobnator : Form
	{
		public Siscobnator()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String archivo,ext,cad_con,tabla,cons_exc,hoja,sql,sql2,t_d,t_c,t_p_c,per,reg_pat,per2,credito,fecha,f1,f2,f3,f4,f5,ruta,id,cad_fech_des,cad_fech_has,nom_save,f1_1,nvo_stat_cap,ant_stat_cap,cons_fech_cop,acierto_reg_cre;
		int filas=0,i=0,tot_rows=0,tot_cols=0,n=0,j=0,tipo_carga=0,fecha_vac=0,gen_list=0,x=0,tipo_credito=0,capturado_ant=0,y=0,z=0,w=0,tot_errs=0,candado=0,k=0,tot_rows1=0,filtro_cpo=0,m=0,acierto_act=0,mochado=0,cont_omis=0,no_vig=0;
		int[] error_list,error_list2,error_list3,error_list4;
		string[] errores_sis,aciertos_sis;
		double totr=0;
		DialogResult respuesta,respuesta2;
		FileStream fichero,fichero1,fichero2;
		DateTime fecha_desde, fecha_hasta;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable data_3 = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		DataTable tablarow = new DataTable();
		DataTable tablarow2 = new DataTable();
		DataTable data_query = new DataTable();
		DataTable rale_tipo = new DataTable();
		DataTable guardar_arch = new DataTable();

        public void llenar_Cb2_todos()
        {
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
            do
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            conex.cerrar();
        }
		
		public void carga_excel(){
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
			i=0;
			filas = 0;
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
		
		public void carga_access(){
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Archivos de Access (*.mdb *.accdb)|*.mdb;*.accdb"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos Access
			dialog.Title = "Seleccione el archivo de Access";//le damos un titulo a la ventana
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo=dialog.FileName;
				label13.Text = dialog.SafeFileName;
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();
				if(ext.Equals("mdb")){
					//esta cadena es para archivos access anteriores a la version 2007
					cad_con = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + archivo + "';";
					
				}else{
					//esta cadena es para archivos access 2007 y posteriores
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';";
				}
				
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la tabla de Access
				conexion.Open(); //abrimos la conexion
				
				cargar_chema_access();
			}
		}
		
		public void cargar_chema_access(){
			i=0;
			filas = 0;
			comboBox5.Items.Clear();
			System.Data.DataTable dt = conexion.GetSchema("TABLES");
				dataGridView2.DataSource =dt;
				filas=(dataGridView2.RowCount)-1;
				do{
					if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
						if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
							tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
							comboBox5.Items.Add(tabla);
						}
					}
					i++;
				}while(i<=filas);
				i = 0;
				filas = 0;
                dt.Clear();
                dataGridView2.DataSource = dt; //vaciar datagrid
		}
		
		public void cargar_hoja_excel_access(){
			
			if(tipo_carga==1){
				hoja = comboBox1.SelectedItem.ToString();
			}else{
				hoja = comboBox5.SelectedItem.ToString();
			}
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				if(tipo_carga==1){
				cons_exc = "Select * from [" + hoja + "$] ";
				}else{
				cons_exc = "Select * from " + hoja + " ";
				}
				try
					
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					if(dataAdapter.Equals(null)){
						if(tipo_carga==1){
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						}else{
						MessageBox.Show("Error, Verificar el archivo o el nombre de la tabla\n","Error al Abrir Archivo de Access/");	
						}
					}else{
						dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
						dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						conexion.Close();//cerramos la conexion
						dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						tot_rows=dataGridView1.RowCount;
						totr=Convert.ToDouble(tot_rows);
						tot_cols=dataGridView1.ColumnCount;
						label5.Text="Registros: "+tot_rows;
						label5.Refresh();
						button10.Enabled=true;		
						button4.Enabled=false;	

						//estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView1.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView1.ColumnCount);
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
		
		public void consultar_bd(){
			
			conex.conectar("base_principal");
			dataGridView1.DataSource="";
			tot_rows=0;
			per="";
			
			String fecha_limite=System.DateTime.Now.Year.ToString()+"-"+System.DateTime.Now.Month.ToString()+"-"+System.DateTime.Now.Day.ToString();
			
            if (comboBox2.SelectedIndex < -1){
                MessageBox.Show("Selecciona algún periodo para consultar", "Error");
            }else{
				
				sql= "SELECT id,capturado_siscob,registro_patronal2,periodo,fecha_notificacion,credito_cuotas,credito_multa,nombre_periodo,fecha_recepcion FROM datos_factura WHERE nombre_periodo LIKE ";
				sql2="";
				
				if(comboBox2.SelectedIndex > -1){
					sql2 = comboBox2.SelectedItem.ToString();
				}//fin combobox2	
				 
				sql=sql+" \""+sql2+"\" AND fecha_notificacion <= \""+fecha_limite+"\"";
				dataGridView1.DataSource = conex.consultar(sql);
				tot_rows = dataGridView1.RowCount;
				label5.Text="Registros: "+dataGridView1.RowCount;
				n=1;
				button10.Enabled=true;
				button4.Enabled=false;
			
				dataGridView1.Columns[0].HeaderText="ID";
				dataGridView1.Columns[1].HeaderText="CAPTURADO SISCOB";
				dataGridView1.Columns[2].HeaderText="REGISTRO PATRONAL";
				dataGridView1.Columns[3].HeaderText="PERIODO";
				dataGridView1.Columns[4].HeaderText="FECHA DE NOTIFICACIÓN";
				dataGridView1.Columns[5].HeaderText="CRÉDITO CUOTA";
				dataGridView1.Columns[6].HeaderText="CRÉDITO MULTA";
				dataGridView1.Columns[7].HeaderText="NOMBRE DE PERIODO";
				dataGridView1.Columns[8].HeaderText="FECHA DE RECEPCIÓN";
				
			}

			//MessageBox.Show("Consulta: "+sql);
		}
		
		public void consultar_bd_fecha(){
			
        	String fecha_limite=System.DateTime.Now.Year.ToString()+"-"+System.DateTime.Now.Month.ToString()+"-"+System.DateTime.Now.Day.ToString();
        	
        	String tipo_fecha;
			fecha_hasta = dateTimePicker2.Value;
			fecha_desde = dateTimePicker1.Value;
			
			if(fecha_desde > fecha_hasta){
				MessageBox.Show("Verifique las fechas seleccionadas y seleccione un intervalo válido","Error en la Selección de Fechas",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}else{
				if(filtro_cpo < 1){
					MessageBox.Show("Seleccione el filtro que se va a aplicar","Filtro no Seleccionado",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				}else{
					if(filtro_cpo==1){
						cons_fech_cop="COP";
					}else{
						cons_fech_cop="RCV";
					}
					
				cad_fech_des = (fecha_desde.ToShortDateString().Substring(6,4))+"/"+(fecha_desde.ToShortDateString().Substring(3,2))+"/"+(fecha_desde.ToShortDateString().Substring(0,2));
				cad_fech_has = (fecha_hasta.ToShortDateString().Substring(6,4))+"/"+(fecha_hasta.ToShortDateString().Substring(3,2))+"/"+(fecha_hasta.ToShortDateString().Substring(0,2));
				conex.conectar("base_principal");
				
				if(comboBox3.SelectedIndex==0){
					tipo_fecha="fecha_recepcion";
				}else{
					tipo_fecha="fecha_notificacion";
				}
				
				sql= "SELECT id,capturado_siscob,registro_patronal2,periodo,fecha_notificacion,credito_cuotas,credito_multa,nombre_periodo,fecha_recepcion FROM datos_factura WHERE nombre_periodo LIKE \""+cons_fech_cop+"%\" AND "+tipo_fecha+" BETWEEN \""+cad_fech_des+"\" AND \""+cad_fech_has+"\" AND fecha_notificacion <= \""+fecha_limite+"\"";
			  
				//MessageBox.Show(sql);
				dataGridView1.DataSource="";
				tot_rows=0;
				
				dataGridView1.DataSource = conex.consultar(sql);
				tot_rows = dataGridView1.RowCount;
				label5.Text="Registros: "+dataGridView1.RowCount;
				n=1;
				button10.Enabled=true;
				button4.Enabled=false;
			
				dataGridView1.Columns[0].HeaderText="ID";
				dataGridView1.Columns[1].HeaderText="CAPTURADO SISCOB";
				dataGridView1.Columns[2].HeaderText="REGISTRO PATRONAL";
				dataGridView1.Columns[3].HeaderText="PERIODO";
				dataGridView1.Columns[4].HeaderText="FECHA DE NOTIFICACIÓN";
				dataGridView1.Columns[5].HeaderText="CRÉDITO CUOTA";
				dataGridView1.Columns[6].HeaderText="CRÉDITO MULTA";
				dataGridView1.Columns[7].HeaderText="NOMBRE DE PERIODO";
				dataGridView1.Columns[8].HeaderText="FECHA DE RECEPCIÓN";
				}
			}
		}
		
		public bool checar_rale(String rp, String cred, String peri){
        	if(radioButton5.Checked==true){
				DataView vista = new DataView(data_query);
				
				vista.RowFilter="registro_patronal='"+rp+"' AND credito='"+cred+"' AND periodo='"+peri+"'";
				dataGridView2.DataSource=vista;
				
				if(dataGridView2.RowCount>0){
					return true;
				}else{
					return false;
				}
        	}else{
        		return true;
        	}
		}
		
		public void filtro()
        {
			String fecha_limite;
			
            try
            {
            	//Borrar archivos para comenzar de 0
            	System.IO.File.Delete(@"capturator/temp.txt");
        	    //Crear archivos nuevos
                //System.IO.File.Create(@"capturator/temp.txt");
                //Abrir archivo
                StreamWriter wr = new StreamWriter(@"capturator/temp.txt");

                if (radioButton1.Checked)
                {
                    j = 5;
                }
                else
                {
                    if (radioButton2.Checked)
                    {
                        j = 6;
                    }
                }
                
                tipo_credito = j;
                fecha_vac = 0;
                gen_list = 0;
                capturado_ant = 0;
                w = 0;
                x = 0;
                y = 0;
                z = 0;
                no_vig = 0;
                error_list = new int[tot_rows+1];
                error_list2 = new int[tot_rows+1];
                error_list3 = new int[tot_rows+1];
                error_list4 = new int[tot_rows+1];
                //write rows to excel file

                consultamysql.Columns.Clear();
                consultamysql.Rows.Clear();

                //columnas
                if (n == 1)
                {
                    consultamysql.Columns.Add("ID");
                    consultamysql.Columns.Add("REGISTRO PATRONAL");
                    consultamysql.Columns.Add("CAPTURADO SISCOB");
                    consultamysql.Columns.Add("CREDITO");
                    
                }
                else {
                    //Carga de Excel o Access
                    consultamysql.Columns.Add("REGISTRO PATRONAL");
                    consultamysql.Columns.Add("PERIODO");
                    consultamysql.Columns.Add("CREDITO");
                    consultamysql.Columns.Add("FECHA");
                }

                tablarow.Clear();
                tablarow2.Clear();
                
                conex2.conectar("base_principal");
                
                
                	
                if(radioButton3.Checked==true){
                	//COP
                	sql="SELECT registro_patronal,credito,periodo FROM rale WHERE tipo_rale=\"COP\" AND fecha_noti LIKE \"0000-00-00\"";
                }else{
                	if(radioButton4.Checked==true){
                		//RCV
                		sql="SELECT registro_patronal,credito,periodo FROM rale WHERE tipo_rale=\"RCV\" AND fecha_noti LIKE \"0000-00-00\"";
                	}
                }
                
                data_query=conex2.consultar(sql);
                //MessageBox.Show(data_query.Rows.Count.ToString());

                for (i = 0; i < tot_rows; i++)
                {

                    if (n == 1)
                    {
                        id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                        f1 = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                        f2 = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                        f3 = dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                        f4 = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                        f5 = dataGridView1.Rows[i].Cells[8].FormattedValue.ToString();

                        reg_pat = Convert.ToString(dataGridView1.Rows[i].Cells[2].FormattedValue);
                        per2 = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                        fecha = Convert.ToString(dataGridView1.Rows[i].Cells[4].FormattedValue);
                        credito = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);

                        fecha_vac = 0;

                        if (tipo_credito == 5)
                        {
                            f1_1 = f1.Substring(0, 1);
                        }
                        else
                        {
                            f1_1 = f1.Substring(2, 1);
                        }

                        if (f1_1 == "1")
                        {
                            fecha_vac = 1;
                            capturado_ant++;
                        }
                            if (credito.Length <= 1)
                            {
                                fecha_vac = 1;
                                x = x + 1;
                                error_list2[x] = i;

                            }

                            if (fecha.Length <= 1)
                            {
                                fecha_vac = 1;
                                gen_list = gen_list + 1;
                                error_list[gen_list] = i;

                            }

                           /* if (fecha_vac == 0)
                            {
                                consultamysql.Rows.Add(id, reg_pat,f1,credito);
                            }
                            else
                            {
                                tablarow.Rows.Add(id, f1, reg_pat, per2, fecha, f2, f3, f4, f5);
                                w++;
                            }*/

                    }
                    else
                    {
                        //Carga de Excel o Access
                        reg_pat = Convert.ToString(dataGridView1.Rows[i].Cells[0].FormattedValue);
                        per2 = Convert.ToString(dataGridView1.Rows[i].Cells[2].FormattedValue);
                        credito = Convert.ToString(dataGridView1.Rows[i].Cells[1].FormattedValue);
                        fecha = Convert.ToString(dataGridView1.Rows[i].Cells[3].FormattedValue);

                        fecha_vac = 0;

                        if ((credito.Length < 9) || (credito.Length > 9))
                        {
                            fecha_vac = 1;
                            x = x + 1;
                            error_list2[x] = i;
                        }

                        if ((fecha.Length < 10) || (fecha.Length > 10))
                        {
                            fecha_vac = 1;
                            gen_list = gen_list + 1;
                            error_list[gen_list] = i;
                        }

                        if ((reg_pat.Length < 10) || (reg_pat.Length > 10))
                        {
                            fecha_vac = 1;
                            y = y + 1;
                            error_list3[y] = i;
                        }

                        if ((per2.Length < 6) || (per2.Length > 6))
                        {
                            fecha_vac = 1;
                            z = z + 1;
                            error_list4[z] = i;
                        }

                       /* if (fecha_vac == 0)
                        {
                            consultamysql.Rows.Add(reg_pat, per2, credito, fecha);
                        }
                        else
                        {
                            tablarow2.Rows.Add(reg_pat, per2, credito, fecha);
                            w++;
                        }*/

                    }

                    if (fecha_vac == 0)
                    {
                    	if(checar_rale(reg_pat,credito,per2)== true){
                    	   	if(n==1){
                    	   		consultamysql.Rows.Add(id, reg_pat,f1,credito);
                    	   	}else{
                    	   		consultamysql.Rows.Add(reg_pat, per2, credito, fecha);
                    	   	}
                    	   	
                    	   	wr.WriteLine(reg_pat);
                    	   	wr.WriteLine(per2.Substring(4, 2));
                    	   	wr.WriteLine(per2.Substring(2, 2));
                    	   	wr.WriteLine(credito);
                    	   	wr.WriteLine((fecha.Substring(0,2)+fecha.Substring(3,2)+fecha.Substring(8,2)));
                    	   	wr.WriteLine("$");
                    	   	
                    	   }else{
                    	   	no_vig++;
                    	   	if (n == 1){
                    	   		tablarow.Rows.Add(id, f1, reg_pat, per2, fecha, f2, f3, f4, f5);
                    	   	}else{
                    	   		tablarow2.Rows.Add(reg_pat, credito, per2);
                    	   	}
                    	   }
                    	}else{
                    	w++;
                    	if(n==1){
                    		tablarow.Rows.Add(id, f1, reg_pat, per2, fecha, f2, f3, f4, f5);
                    	}else{
                    		tablarow2.Rows.Add(reg_pat, per2, credito, fecha);
                    	}
                    }
                    //dataGridView1.Rows[i].Visible=false;
                }

                wr.WriteLine("%&");

                //MessageBox.Show("Archivo Creado con: "+i+" Registros");

                //MessageBox.Show("El Archivo:\n"+fichero.FileName.ToString()+"\nSe ha creado correctamente","¡Exito!");
                //n=0;
                //close file
                wr.Close();

                StreamWriter wr1 = new StreamWriter(@"capturator/temp_aux.txt");
                wr1.WriteLine("0");
                wr1.WriteLine(consultamysql.Rows.Count.ToString());

                wr1.Close();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                //en caso de haber una excepcion que nos mande un mensaje de error
                MessageBox.Show("No se pudo generar la lista\n\nError:\n" + ex, "Error al Generar Lista");
            }
        }
		
		public void finalizar()
        {
        	//tipo_carga = n;
        	//MessageBox.Show("tipo_car:" + tipo_car + "  tipo_cred" + tipo_cred);
        	//Proceso final de captura en siscob

        	try
        	{ 
        		
        		StreamReader rdr = new StreamReader(@"capturator/errores_siscob.txt");
        		StreamReader rdr1 = new StreamReader(@"capturator/temp_aux.txt");
        		
        		Invoke(new MethodInvoker(delegate
        		                         {
        		                         	
        		                         	errores_sis = new string[consultamysql.Rows.Count];
        		                         	aciertos_sis = new string[2];
        		                         	i=0;
        		                         	m=0;
        		                         	int dot_pos=0;
        		                         	
        		                         	do{        		                         	
        		                         	errores_sis[i]=rdr.ReadLine();
        		                         	i++;
        		                         	}while(!(rdr.EndOfStream));   
        		                         	
        		                         	do{        		                         	
        		                         	if(dot_pos>1){
        		                         		dot_pos=0;
        		                         	}
        		                         	
        		                         	aciertos_sis[dot_pos]=rdr1.ReadLine();
        		                         	m++;
        		                         	
        		                         	dot_pos++;
        		                         	}while(!(rdr1.EndOfStream));   
        		                         	
        		                         	tot_rows1 = 0;
        		                         	dataGridView3.DataSource = null;
        		                         	dataGridView3.DataSource = consultamysql;
        		                         	tot_rows1 = dataGridView3.RowCount;
        		                         	
        		                         	if (n == 1)
        		                         	{
        		                         		conex.conectar("base_principal");
        		                         	}                	
        		                         }));
        		rdr.Close();
        		rdr1.Close();
        		tot_errs = 0;
        		tot_errs = i;
        		acierto_act = (Convert.ToInt32(aciertos_sis[0]));
        		acierto_reg_cre = aciertos_sis[1];
        		
        		//MessageBox.Show(" Registros Actualizada  "+tot_errs+", "+tot_rows1);
        		candado = 0;
        		i = 0;
        		j = 0;
        		k = 0;
        		w = 0;
        		cont_omis=0;
        		
        		if (n == 1)
        		{
        			Invoke(new MethodInvoker(delegate
        			                         {
        			                         	//MainForm mani = (MainForm)this.MdiParent;
        			                         	try{
	        			                         		guardar_arch.Columns.Add("ID");
	        			                         		guardar_arch.Columns.Add("REGISTRO PATRONAL");
	        			                         		guardar_arch.Columns.Add("CREDITO");
	        			                         		guardar_arch.Columns.Add("CAPTURADO");
        			                         	}catch{}
        			                         	
        			                         	try{
        			                         			guardar_arch.Rows.Clear();
        			                         	}catch{}
        			                         	do
        			                         	{	
        			                         		guardar_arch.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(),dataGridView3.Rows[i].Cells[1].Value.ToString(),dataGridView3.Rows[i].Cells[3].Value.ToString(),"");
        			                         		
        			                         		 if (errores_sis[0] == null) {
                                                        w = 1;
                                                    }
        			                         		if ((k < tot_errs)&&(w==0))    
        			                         		{

        			                         			for (j = 0; j < tot_errs; j++)
        			                         			{    
                                                                reg_pat = errores_sis[j].Substring(0, 10);
                                                                credito = errores_sis[j].Substring(11, 9);

                                                                if (((dataGridView3.Rows[i].Cells[1].Value.ToString()).Equals(reg_pat)) && ((dataGridView3.Rows[i].Cells[3].Value.ToString()).Equals(credito)))
                                                                {
                                                                    if (tipo_credito == 5)
                                                                    {
                                                                        nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
                                                                        //nvo_stat_cap = "2" + nvo_stat_cap.Substring(1, 2);
                                                                        nvo_stat_cap = nvo_stat_cap.Remove(0,1);
                                                            			nvo_stat_cap = nvo_stat_cap.Insert(0,"2");
                                                                    }
                                                                    else
                                                                    {
                                                                        nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
                                                                        //nvo_stat_cap = nvo_stat_cap.Substring(0, 2) + "2";
                                                                        nvo_stat_cap = nvo_stat_cap.Remove(2,1);
                                                            			nvo_stat_cap = nvo_stat_cap.Insert(2,"2");
                                                                    }
                                                                    sql = "UPDATE datos_factura SET capturado_siscob =\"" + nvo_stat_cap + "\"  WHERE id =" + dataGridView3.Rows[i].Cells[0].Value.ToString() + "";
                                                                    conex.consultar(sql);
                                                                    k = k + 1;
                                                                    candado = 1;
                                                                    
                                                                    guardar_arch.Rows[i][3]="ERROR";
                                                                }
                                                            
        			                         				//mani.toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k);
        			                         				//mani.statusStrip1.Refresh();
        			                         				
        			                         				  toolStripStatusLabel1.Text ="Procesando Resultados. Registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k);
        			                         				  statusStrip1.Refresh();
        			                         				
        			                         			}
        			                         			
        			                         			
        			                         			if (candado == 0)
        			                         			{
        			                         				ant_stat_cap = dataGridView3.Rows[0].Cells[2].Value.ToString();
        			                         				if (tipo_credito == 5)
        			                         				{
        			                         					nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         					//nvo_stat_cap = "1" + nvo_stat_cap.Substring(1, 2);
        			                         					nvo_stat_cap = nvo_stat_cap.Remove(0,1);
                                                            	nvo_stat_cap = nvo_stat_cap.Insert(0,"1");
        			                         				}
        			                         				else
        			                         				{
        			                         					nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         					//nvo_stat_cap = nvo_stat_cap.Substring(0, 2) + "1";
        			                         					nvo_stat_cap = nvo_stat_cap.Remove(2,1);
                                                            	nvo_stat_cap = nvo_stat_cap.Insert(2,"1");
        			                         				}
        			                         				sql = "UPDATE datos_factura SET capturado_siscob =\""+nvo_stat_cap+"\" WHERE id =" + dataGridView3.Rows[i].Cells[0].Value.ToString() + "";
        			                         				conex.consultar(sql);
        			                         				guardar_arch.Rows[i][3]="CAPTURADO";
        			                         			}

        			                         		}
        			                         		else
        			                         		{
        			                         			if((i+1) > acierto_act){
        			                         				candado = 1;
        			                         				cont_omis++;
        			                         				guardar_arch.Rows[i][3]="0";
        			                         			}
        			                         			
        			                         			if(candado==0){
	        			                         			if (tipo_credito == 5)
	        			                         			{
	        			                         				nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
	        			                         				//nvo_stat_cap = "1" + nvo_stat_cap.Substring(1, 2);
	        			                         				nvo_stat_cap = nvo_stat_cap.Remove(0,1);
	                                                            nvo_stat_cap = nvo_stat_cap.Insert(0,"1");
	        			                         			}
	        			                         			else
	        			                         			{
	        			                         				nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
	        			                         				//nvo_stat_cap = nvo_stat_cap.Substring(0, 2) + "1";
	        			                         				nvo_stat_cap = nvo_stat_cap.Remove(2,1);
	                                                            nvo_stat_cap = nvo_stat_cap.Insert(2,"1");
	        			                         			}
	        			                         			sql = "UPDATE datos_factura SET capturado_siscob = \""+nvo_stat_cap+"\" WHERE id =" + dataGridView3.Rows[i].Cells[0].Value.ToString() + "";
	        			                         			conex.consultar(sql);
	        			                         			//toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k) + "  lineas leidas: " + j + "    buscando: " + errores_sis[j];
	        			                         			
	        			                         			//mani.toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k);
	        			                         			//mani.statusStrip1.Refresh();
	        			                         			
	        			                         			toolStripStatusLabel1.Text ="Procesando Resultados. Registros leidos: " + i + ".   Erroneos por asignar: " + (tot_errs - k)+".   Omitidos: "+(i-acierto_act)+".";
	        			                         		    statusStrip1.Refresh();
	        			                         		    guardar_arch.Rows[i][3]="CAPTURADO";
        			                         			}
        			                         		}
        			                         		candado = 0;
        			                         		i++;
        			                         	} while ((i < tot_rows1));

        			                         	if (tot_errs < 0) { tot_errs = 0; }  	
        			                         	//mani.toolStripStatusLabel1.Text = " " + k + " Registros actualizados como 'Error al capturar'.     " + (tot_rows1 - k) + " Registros actualizados como 'Capturados'.";
        			                         	//mani.statusStrip1.Refresh();
        			                         	
        			                         	toolStripStatusLabel1.Text = " " + k + " Registros actualizados como 'Error al capturar'.     " + ((tot_rows1 - k)-cont_omis) + " Registros actualizados como 'Capturados'.     "+cont_omis+" Omitidos.";
        			                         	statusStrip1.Refresh();
        			                         	conex.cerrar();
        			                         }));
        			
					                       if(mochado==0){
        				MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nHa terminado correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
					        			   }else{
					        			         MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nSe guardo el resultado de: "+acierto_act+" registros de un total de: "+tot_rows1+"  \nHa terminado correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
					        			   }
        			
        			
        									XLWorkbook wb = new XLWorkbook();	
											wb.Worksheets.Add(guardar_arch,"hoja_lz");
											wb.SaveAs(nom_save);
        								conex.guardar_evento("Se Capturaron en Siscob "+acierto_act+" Registros de Fecha de Notificación de un total de "+tot_rows1);
        		}
        		else
        		{  //finalizar cuando es archivo excel y access
        			consultamysql.Columns.Add("CAPTURADO SISCOB");
        			

        			Invoke(new MethodInvoker(delegate
        			                         {
        			                         	MainForm mani = (MainForm)this.MdiParent;
        			                         	dataGridView3.DataSource = consultamysql;
        			                         	tot_rows = dataGridView3.RowCount;

        			                         	do
        			                         	{
        			                         		dataGridView3.Rows[i].Cells[4].Value = "0";
        			                         		i++;
        			                         	} while (i < tot_rows);

        			                         	i = 0;

        			                         	do
        			                         	{
        			                         		if (errores_sis[0] == null)
                                                    {
                                                        w = 1;
                                                    }
        			                         		
        			                         		if ((k < tot_errs)&& (w == 0))
        			                         		{
        			                         			for (j = 0; j < tot_errs; j++)
        			                         			{
        			                         				reg_pat = errores_sis[j].Substring(0, 10);
                                                            credito = errores_sis[j].Substring(11, 9);
                                                            
        			                         				if (((dataGridView3.Rows[i].Cells[0].Value.ToString()).Equals(reg_pat)) && ((dataGridView3.Rows[i].Cells[2].Value.ToString()).Equals(credito)))
        			                         				{
        			                         					dataGridView3.Rows[i].Cells[4].Value = "ERROR";
        			                         					k = k + 1;
        			                         					candado = 1;
        			                         				}
        			                         				//mani.toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + (i+1) + "   Erroneos por asignar: " + (tot_errs - k);
        			                         				//mani.statusStrip1.Refresh();
        			                         				
        			                         				toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + (i+1) + "   Erroneos por asignar: " + (tot_errs - k);
        			                         				statusStrip1.Refresh();
        			                         			}

        			                         			if (candado == 0)
        			                         			{
        			                         				dataGridView3.Rows[i].Cells[4].Value = "CAPTURADO";
        			                         			}
        			                         		}
        			                         		else
        			                         		{
        			                         			if((i+1) > acierto_act){
        			                         				candado = 1;
        			                         				cont_omis++;
        			                         			}
        			                         			
        			                         			if(candado==0){
        			                         			dataGridView3.Rows[i].Cells[4].Value = "CAPTURADO";
        			                         			//toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k) + "  lineas leidas: " + j + "    buscando: " + errores_sis[j];
        			                         			//mani.toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + (i+1) + "   Erroneos por asignar: " + (tot_errs - k);
        			                         			//mani.statusStrip1.Refresh();
        			                         			
        			                         			}else{
        			                         				dataGridView3.Rows[i].Cells[4].Value = "0";
        			                         			}
        			                         			
        			                         			toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + (i+1) + ".   Erroneos por asignar: " + (tot_errs - k)+".   Omitidos: "+(i-acierto_act)+".";
        			                         			statusStrip1.Refresh();

        			                         		}
        			                         		candado = 0;
        			                         		i++;
        			                         	} while (i < tot_rows);

        			                         	if (tot_errs < 0) { tot_errs = 0; }
        			                         	//mani.toolStripStatusLabel1.Text = " " + k + " Registros actualizados como 'Error al capturar'.     " + (tot_rows - k)+ " Registros actualizados como 'Capturados'.";
        			                         	//mani.statusStrip1.Refresh();
        			                         	
        			                         	toolStripStatusLabel1.Text = " " + k + " Registros actualizados como 'Error al capturar'.     " + ((tot_rows - k)-cont_omis) + " Registros actualizados como 'Capturados'.     "+(cont_omis+dataGridView1.RowCount)+" Omitidos.";
        			                         	statusStrip1.Refresh();
        			                         
        			                         	 i = 0;
                                                tot_rows = dataGridView1.RowCount;

                                                while (i < tot_rows){
                                                    reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
        			                         		per2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
        			                         		credito = dataGridView1.Rows[i].Cells[2].Value.ToString();
        			                         		fecha = dataGridView1.Rows[i].Cells[3].Value.ToString();

                                                    consultamysql.Rows.Add(reg_pat,per2,credito,fecha,"0");
                                                    i++;
                                                    //reg_pat, per2, credito, fecha, "0"
                                                } 
                                                
                                                dataGridView3.Refresh();
        			                         	label5.Text="Registros: "+dataGridView3.RowCount;                                             
        			                         	dataGridView3.Visible = true;
        			                         	
        			                            //open file
        			                         	StreamWriter wr2 = new StreamWriter(nom_save);
        			                         	i = 0;
        			                         	tot_rows=dataGridView3.RowCount;
        			                         	wr2.WriteLine("REGISTRO PATRONAL,PERIODO,CREDITO,FECHA NOTIFICACION,CAPTURADO SISCOB");
        			                         	do
        			                         	{
        			                         		reg_pat = dataGridView3.Rows[i].Cells[0].Value.ToString();
        			                         		per2 = dataGridView3.Rows[i].Cells[1].Value.ToString();
        			                         		credito = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         		fecha = dataGridView3.Rows[i].Cells[3].Value.ToString();
        			                         		f1 = dataGridView3.Rows[i].Cells[4].Value.ToString();
        			                         		wr2.WriteLine(reg_pat + "," + per2 + "," + credito + "," + fecha + "," + f1);
        			                         		i++;
        			                         	} while (i < tot_rows);
        			                         	wr2.Close();
        			                         			
        			                         	if(mochado==0){
        			                         		MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nHa terminado correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			                         	}else{
        			                         		MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nSe guardo el resultado de: "+acierto_act+" registros de un total de: "+tot_rows+".  \nHa terminado de forma parcial correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			                         	}
        			                         	
        			                         	conex.guardar_evento("Se Capturaron en Siscob "+acierto_act+" Registros de Fecha de Notificación de un total de "+tot_rows);
        			                         }));
        		}
        		
        	}
        	catch (Exception e1)
        	{
        		MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser terminado adecuadamente.\n\n Error:\n" + e1, "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
        	}
        	Invoke(new MethodInvoker(delegate
        	                         {
        	                         	//MainForm mani1 = (MainForm)this.MdiParent;
        	                         	//mani1.toolStripStatusLabel1.Text = "Listo";
        	                         	toolStripStatusLabel1.Text = "Listo";
        	                         	button18.Visible=false;
        	                         	button4.Visible=true;
        	                         	button10.Visible=true;
        	                         	button3.Enabled=true;
        	                         }));

        }
		
		public int guardar_results(){
		   SaveFileDialog dialog_save = new SaveFileDialog();
           if(n==0){
           		dialog_save.Filter = "Archivos de Excel (*.CSV)|*.CSV"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
           }else{
        	 	dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
           }
		   dialog_save.Title = "Guardar resultados de Captura";//le damos un titulo a la ventana

           if (dialog_save.ShowDialog() == DialogResult.OK){
        	   nom_save = dialog_save.FileName; //open file
        	   return 1;
        	   //MessageBox.Show("El archivo se generó correctamente.\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
           }else{
          		return 0;
        		}
		}
		
		void SiscobnatorLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            //this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2)-30;
            //this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;
            //this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);

            llenar_Cb2_todos();
			
			tablarow.Columns.Add("ID");
			tablarow.Columns.Add("CAPTURADO SISCOB");
			tablarow.Columns.Add("REGISTRO PATRONAL");
			tablarow.Columns.Add("PERIODO");
			tablarow.Columns.Add("FECHA DE NOTIFICACIÓN");
			tablarow.Columns.Add("CRÉDITO CUOTA");	
			tablarow.Columns.Add("CRÉDITO MULTA");
			tablarow.Columns.Add("NOMBRE PERIODO");
			tablarow.Columns.Add("FECHA DE RECEPCION");
			
			tablarow2.Columns.Add("REGISTRO PATRONAL");
            tablarow2.Columns.Add("PERIODO");
            tablarow2.Columns.Add("CRÉDITO");
            tablarow2.Columns.Add("FECHA DE NOTIFICACIÓN");
		
            dateTimePicker1.MaxDate = System.DateTime.Today;
            dateTimePicker2.MaxDate = System.DateTime.Today;

            dateTimePicker1.Value = System.DateTime.Today;
            dateTimePicker2.Value = System.DateTime.Today;
            toolTip1.SetToolTip(label9, "0 - No Capturado\n1 - Capturado\n2 - Error al Capturar");
            
            panel1.Visible=true;
			panel2.Visible=false;
			panel3.Visible=false;
			label10.Visible=false;
			radioButton1.Visible=true;
			radioButton2.Visible=true;
			groupBox2.Visible=true;
			conex2.conectar("base_principal");
			dataGridView2.DataSource=conex.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_COP%\" order by idlog_eventos desc");
			label3.Text="Rale Ingresado el Día: "+dataGridView2.Rows[0].Cells[1].FormattedValue.ToString().Substring(0,10);
			comboBox3.SelectedIndex=0;
		}
	
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			carga_excel();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			dataGridView3.Visible = false;
            dataGridView3.DataSource = null;
			consultar_bd();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(comboBox1.SelectedItem == null){
				MessageBox.Show("Selecciona una Hoja");
			}else{
			tipo_carga=1;
			n = 0;
            dataGridView3.Visible = false;
            dataGridView3.DataSource = null;
			cargar_hoja_excel_access();
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(guardar_results()==1){
				try{
				MessageBox.Show("Asegúrese de seguir estos pasos antes de continuar:\n\n"+
				                "1.- Ejecute Siscob y abra la ventana donde se van capturar los datos\n"+
				                "2.- Coloque el cursor en el campo donde se va a comenzar a escribir\n"+
				                "3.- De click al boton Aceptar de este mensaje","ATENCIÓN",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
				                
				respuesta = MessageBox.Show("Está a punto de comenzar el proceso de captura automática.\n"+
				                "Una vez comenzada la captura NO se deberá manipular el equipo\n"+
				                "hasta que haya finalizado el proceso de captura.\n"+
				                "El programa le informará cuando el proceso de captura haya concluido\n\n"+
				                "Teclas Especiales:\n"+
				                "[PAUSA] - Pausa el proceso de captura (no se recomienda)\n"+
				                "[INICIO] - Reanuda el proceso de captura\n"+
				                "[FIN] - Finaliza el proceso de captura\n\n"+
				                "¿Desea comenzar el proceso de captura?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
				
				if(respuesta ==DialogResult.Yes){
						
					MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
	                //Borrar archivos para comenzar de 0
					System.IO.File.Delete(@"capturator/errores_siscob.txt");
	        	    System.IO.File.Delete(@"capturator/acierto_siscob.txt");
	        	    System.IO.File.Delete(@"capturator/aciertos_siscob.txt");
	        	    //Crear archivos nuevos
	                fichero = System.IO.File.Create(@"capturator/errores_siscob.txt");
					fichero1= System.IO.File.Create(@"capturator/acierto_siscob.txt");
					fichero2= System.IO.File.Create(@"capturator/aciertos_siscob.txt");
	                ruta = fichero.Name;
	                fichero.Close();
	                fichero1.Close();
	                fichero2.Close();
	                
	                StreamWriter wr1 = new StreamWriter(@"capture.bat");
	                wr1.WriteLine("@echo off");
	                wr1.WriteLine("C:");
	                wr1.WriteLine("cd \""+ruta.Substring(0,(ruta.Length-19))+"\"");
	                wr1.WriteLine("start optimuscapt.exe");
	                wr1.Close();
	                
	                /* MainForm mani = (MainForm)this.MdiParent;
					mani.capt_siscob(consultamysql,tipo_credito,n);
					n=0;*/
	                button18.Visible=true;
	                button3.Enabled=false;
        	        button4.Visible=false;
        	        button10.Visible=false;
					System.Diagnostics.Process.Start(@"capture.bat");
					
					conex.guardar_evento("Se Mando a capturar en Siscob "+consultamysql.Rows.Count.ToString()+" Registros de Fecha de Notificación.");
					
				}else{
					MessageBox.Show("El proceso no se iniciará.","Información");
				}
				
				button4.Enabled=false;
				}catch(Exception e1){
				MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser iniciado adecuadamente.\n\n Error:\n"+e1,"Información",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}else{
				MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
			}
		}
		//boton bd
		void Button5Click(object sender, EventArgs e)
		{
			panel1.Visible=true;
			panel2.Visible=false;	
			panel3.Visible=false;
			//label10.Visible=true;
			radioButton1.Visible=true;
			radioButton2.Visible=true;
			groupBox2.Visible=true;
		}
		//boton excel
		void Button6Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			panel2.Visible=true;
			panel3.Visible=false;
			label10.Visible=false;
			radioButton1.Visible=false;
			radioButton2.Visible=false;
			groupBox2.Visible=false;
		}
		//boton access
		void Button7Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			panel2.Visible=false;
			panel3.Visible=true;
			label10.Visible=false;
			radioButton1.Visible=false;
			radioButton2.Visible=false;
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			if(comboBox5.SelectedItem == null){
				MessageBox.Show("Selecciona una Tabla");
			}else{
			tipo_carga=2;
			n = 0;
                dataGridView3.Visible = false;
                dataGridView3.DataSource = null;
			cargar_hoja_excel_access();
			}
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			carga_access();
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			filtro();
            if (n == 1)
            {
            	MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron: " + ((tot_rows - w)-no_vig) + " registros\nSe omitieron: " + (w+no_vig) + " registros\n\n" +
                             "" + gen_list + " registros por no contar con una fecha de notificación válida\n" +
                             "" + x + " registros por no contar con el crédito elegido válido\n" +
                             "" + capturado_ant + " registros que ya estaban ingresados\n" +
                             "" + no_vig + " registros que ya no están Vigentes\n\n" +
                             "Ahora puede iniciar la captura automática", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = tablarow;
            }
            else
            {
                MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron:" + ((tot_rows - w)-no_vig)  + " registros\nSe omitieron: " + (w+no_vig) + " registros\n\n" +
                         /*   "" + y + " registros por no contar con un registro patronal válido\n" +         
                            "" + gen_list + " registros por no contar con una fecha de notificación válida\n" +
                             "" + z + " registros por no contar con un  periodo válido\n" +
                             "" + x + " registros por no contar con un crédito válido\n\n" +*/
                             "Ahora puede iniciar la captura automática", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = tablarow2;
            }
			
            if(((tot_rows - w)-no_vig) > 0){
            	button4.Enabled = true;
            }else{
            	button4.Enabled = false;
            }
            
            label5.Text = "Registros: " + dataGridView1.RowCount;
            button10.Enabled = false;
			
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
                dataGridView3.Visible = false;
                dataGridView3.DataSource = null;
				consultar_bd();
			}
		}
		
		void FinToolStripMenuItemClick(object sender, EventArgs e)
		{
			//finalizar();
		}

        private void button11_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel5.Visible = false;
           // toolStripStatusLabel1.Text="nombre de periodo";
            statusStrip1.Refresh();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel5.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
        }
		
		void DateTimePicker1ValueChanged(object sender, EventArgs e)
		{
		
		}
		
		void DateTimePicker2ValueChanged(object sender, EventArgs e)
		{
			
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			consultar_bd_fecha();
		}
		
		void FinToolStripMenuItem1Click(object sender, EventArgs e)
		{
			finalizar();
		}
		
		void Button16Click(object sender, EventArgs e)
		{
			this.button16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(90)))));
		    this.button17.BackColor = System.Drawing.Color.Transparent;
		    filtro_cpo=1;
		}
		
		void Button17Click(object sender, EventArgs e)
		{
			this.button17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(90)))));
			this.button16.BackColor = System.Drawing.Color.Transparent;
			filtro_cpo=2;
		}
		
		void Button18Click(object sender, EventArgs e)
		{
			mochado=1;
			finalizar();
			
		}
		
		void Button19Click(object sender, EventArgs e)
		{
			Login inic = new Login();
			inic.respawn();
			this.Dispose();
		}
		
		void RadioButton3CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton3.Checked==true){
				rale_tipo.Clear();
				rale_tipo=conex2.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_COP%\" order by idlog_eventos desc");
				label3.Text="Rale Ingresado el Día: "+rale_tipo.Rows[0][1].ToString().Substring(0,10);
			}
		}
		
		void RadioButton4CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton4.Checked==true){
				rale_tipo.Clear();
				rale_tipo=conex2.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_RCV%\" order by idlog_eventos desc");
				label3.Text="Rale Ingresado el Día: "+rale_tipo.Rows[0][1].ToString().Substring(0,10);
			}
		}
		
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 02/09/2015
 * Hora: 02:10 p. m.
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
	/// Description of Jarviscob.
	/// </summary>
	public partial class Jarviscob : Form, Puente
	{
		public Jarviscob()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String archivo,ext,cad_con,tabla,cons_exc,hoja,sql,sql2,t_d,t_c,t_p_c,per,reg_pat,per2,credito,fecha,f1,f2,f3,f4,f5,ruta,id,cad_fech_des,cad_fech_has,nom_save,f1_1,nvo_stat_cap,ant_stat_cap,cons_fech_cop,inci,acierto_reg_cre,tipo_ev,fecha_quince,rale_cop,rale_rcv,ids_core="",incid_rale="",td_rale="",ids_core_cuota="",ids_core_multa="",folio_final="";
		int filas=0,i=0,tot_rows=0,tot_cols=0,n=0,j=0,tipo_carga=0,fecha_vac=0,gen_list=0,x=0,tipo_credito=0,capturado_ant=0,y=0,z=0,w=0,tot_errs=0,candado=0,k=0,tot_rows1=0,filtro_cpo=0,filtr=0,m=0,acierto_act=0,mochado=0,cont_omis=0,no_vig=0,no_fecha_vac=0,x2=0,cap_ant2=0,no_vig2=0,tipo_per_cons=0;
		int[] error_list,error_list2,error_list3,error_list4;
		string[] errores_sis,aciertos_sis;
		double totr=0;
		DialogResult respuesta,respuesta2;
		FileStream fichero,fichero1,fichero2;
		DateTime fecha_antes;
		
		Carga rcv_cop;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		
		DataTable consultamysql = new DataTable();
		DataTable data_3 = new DataTable();
		DataTable data_rale = new DataTable();
		DataTable data_query = new DataTable();
		DataTable data_core = new DataTable();
		DataTable data_core_multa = new DataTable();
		DataTable data_core_comb = new DataTable();
		DataTable data_ids_folio_core = new DataTable();
		DataTable data_guarda_folio_core = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		DataTable tablarow = new DataTable();
		DataTable tablarow2 = new DataTable();
		DataTable guardar_arch = new DataTable();
		
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
		
		public void cargar_hoja_excel(){
			
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
						dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						conexion.Close();//cerramos la conexion
						dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						tot_rows=dataGridView1.RowCount;
						totr=Convert.ToDouble(tot_rows);
						tot_cols=dataGridView1.ColumnCount;
						label5.Text="Registros: "+tot_rows;
						label5.Refresh();
						//button10.Enabled=true;		
						button4.Enabled=false;	
						maskedTextBox2.Enabled=true;

						//estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView1.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView1.ColumnCount);
                        i = 0;			

						if(tot_rows>0){
							maskedTextBox2.Enabled=true;						
						}else{
							maskedTextBox2.Enabled=false;
							button10.Enabled=false;
							button4.Enabled=false;
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
		
		public void consultar_bd(){
			
			conex.conectar("base_principal");
			data_query.Rows.Clear();
			dataGridView1.DataSource="";
			tot_rows=0;
			per="";
			
			if(comboBox2.SelectedIndex < -1){
                MessageBox.Show("Selecciona algún periodo para consultar", "Error");
			}else{
				
				sql= "SELECT id,capturado_siscob,registro_patronal2,credito_cuotas,credito_multa,periodo,incidencia,nombre_periodo,fecha_notificacion,importe_cuota,importe_multa,tipo_documento,incidencia_multa,tipo_documento_multa FROM datos_factura WHERE status in(\"NOTIFICADO\",\"AFILIACION\",\"CARTERA\") and nombre_periodo LIKE ";
				sql2="";
				
				if(comboBox2.SelectedIndex > -1){
					sql2 = comboBox2.SelectedItem.ToString();
				}//fin combobox2	
				
				sql=sql+" \""+sql2+"\" ORDER BY registro_patronal2,credito_cuotas,credito_multa";
				dataGridView1.DataSource = conex.consultar(sql);
				
				tot_rows = dataGridView1.RowCount;
				label5.Text="Registros: "+dataGridView1.RowCount;
				n=1;
				//button10.Enabled=true;
				button4.Enabled=false;
				maskedTextBox2.Enabled=true;
			
				dataGridView1.Columns[0].HeaderText="ID";
				dataGridView1.Columns[1].HeaderText="CAPTURADO SISCOB";
				dataGridView1.Columns[2].HeaderText="REGISTRO PATRONAL";
				dataGridView1.Columns[3].HeaderText="CRÉDITO CUOTA";
				dataGridView1.Columns[4].HeaderText="CRÉDITO MULTA";
				dataGridView1.Columns[5].HeaderText="PERIODO";
				dataGridView1.Columns[6].HeaderText="INCIDENCIA ACTUAL";
				dataGridView1.Columns[7].HeaderText="NOMBRE DE PERIODO";
				dataGridView1.Columns[8].HeaderText="FECHA NOTIFICACIÓN";
				dataGridView1.Columns[9].HeaderText="IMPORTE CUOTA";
				dataGridView1.Columns[10].HeaderText="IMPORTE MULTA";
				dataGridView1.Columns[11].HeaderText="TIPO DOCUMENTO";	
				dataGridView1.Columns[12].HeaderText="INCIDENCIA MULTA";
				dataGridView1.Columns[13].HeaderText="TIPO DOCUMENTO MULTA";				
			}
			if(tot_rows>0){
				maskedTextBox2.Enabled=true;
			}else{
				maskedTextBox2.Enabled=false;
				button10.Enabled=false;
				button4.Enabled=false;
			}
			//MessageBox.Show("Consulta: "+sql);
		}

        public void consultar_fecha_not() { 
			
        	String fecha_antes_cadena;
            conex.conectar("base_principal");
            data_query.Rows.Clear();
            dataGridView1.DataSource="";
			tot_rows=0;
			per="";
            fecha_quince = dateTimePicker1.Text;           
            fecha_antes = dateTimePicker2.Value;
            fecha_quince=fecha_quince.Substring(6,4)+"-"+fecha_quince.Substring(3,2)+"-"+fecha_quince.Substring(0,2);
            fecha_antes_cadena=fecha_antes.ToShortDateString().Substring(6,4)+"-"+fecha_antes.ToShortDateString().Substring(3,2)+"-"+fecha_antes.ToShortDateString().Substring(0,2);
            
            if (comboBox2.SelectedIndex < -1)
            {
                MessageBox.Show("Selecciona algún criterio para la consulta", "Error de Consulta");
            }
            else
            {
            	if(radioButton3.Checked==true){
            		sql = "SELECT id,capturado_siscob,registro_patronal2,credito_cuotas,credito_multa,periodo,incidencia,nombre_periodo,fecha_notificacion,importe_cuota,importe_multa,tipo_documento,incidencia_multa,tipo_documento_multa FROM datos_factura WHERE status in(\"NOTIFICADO\",\"AFILIACION\",\"CARTERA\") AND fecha_notificacion BETWEEN  \""+fecha_antes_cadena+"\" AND \"" + fecha_quince + "\" AND nombre_periodo like\"%COP%\" ORDER BY registro_patronal2,credito_cuotas,credito_multa";
            		tipo_per_cons=1;
            	}else{
            		if(radioButton4.Checked==true){
            			sql = "SELECT id,capturado_siscob,registro_patronal2,credito_cuotas,credito_multa,periodo,incidencia,nombre_periodo,fecha_notificacion,importe_cuota,importe_multa,tipo_documento,incidencia_multa,tipo_documento_multa FROM datos_factura WHERE status in(\"NOTIFICADO\",\"AFILIACION\",\"CARTERA\") AND fecha_notificacion BETWEEN  \""+fecha_antes_cadena+"\" AND \"" + fecha_quince + "\" AND nombre_periodo like\"%RCV%\" ORDER BY registro_patronal2,credito_cuotas,credito_multa";
            			tipo_per_cons=2;
            		}
            	}

                
            	//dataGridView1.Rows.Clear();
                //MessageBox.Show(sql);
                dataGridView1.DataSource = conex.consultar(sql);
               	
				tot_rows = dataGridView1.RowCount;
                label5.Text = "Registros: " + dataGridView1.RowCount;
				n=1;
                button4.Enabled = false;
                maskedTextBox2.Enabled = true;
                

                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "CAPTURADO SISCOB";
                dataGridView1.Columns[2].HeaderText = "REGISTRO PATRONAL";
                dataGridView1.Columns[3].HeaderText = "CRÉDITO CUOTA";
                dataGridView1.Columns[4].HeaderText = "CRÉDITO MULTA";
                dataGridView1.Columns[5].HeaderText = "PERIODO";
                dataGridView1.Columns[6].HeaderText = "INCIDENCIA ACTUAL";
                dataGridView1.Columns[7].HeaderText = "NOMBRE DE PERIODO";
                dataGridView1.Columns[8].HeaderText = "FECHA NOTIFICACIÓN";
                dataGridView1.Columns[9].HeaderText="IMPORTE CUOTA";
				dataGridView1.Columns[10].HeaderText="IMPORTE MULTA";
				dataGridView1.Columns[11].HeaderText="TIPO DOCUMENTO";	
				dataGridView1.Columns[12].HeaderText="INCIDENCIA MULTA";
				dataGridView1.Columns[13].HeaderText="TIPO DOCUMENTO MULTA";
            }
        
        }
		
		public bool checar_rale(String rp, String cred, String peri){
			//MessageBox.Show("rp: "+rp+" cred:"+cred+" periodo:"+peri);
			DataView vista = new DataView(data_query);
			
			vista.RowFilter="registro_patronal='"+rp+"' AND credito='"+cred+"' AND periodo='"+peri+"'";
			dataGridView2.DataSource=vista;
			
			if(dataGridView2.RowCount>0){
				incid_rale=dataGridView2.Rows[0].Cells[3].Value.ToString();
				td_rale=dataGridView2.Rows[0].Cells[4].Value.ToString();
				return true;
			}else{
				return false;
			}
            //return true;
		}

		public void filtro()
        {
            try
            {
            	String fecha_not,cred_mul="";
            	int incid=0,tidoc=0,tiimport=0,fecha_vac_2=0;
            	maskedTextBox2.Enabled=false;
            	//Borrar archivos para comenzar de 0
            //	System.IO.File.Delete(@"capturator_inc/temp.txt");
        	    //Crear archivos nuevos
                //System.IO.File.Create(@"capturator/temp.txt");
                //Abrir archivo
            //    StreamWriter wr = new StreamWriter(@"capturator_inc/temp.txt");
				
                if (tipo_per_cons==2)//si se consultó un periodo RCV en la BD
                {
                	if(tipo_carga==2){
	                    j = 3;//cuota
	                    incid=6;
	                    tidoc=11;
	                    tiimport=9;
                	}else{
                		j = 4;//multa 
						incid=12;
						tidoc=13;	
						tiimport=10;
                	}
                }
                else
                {
                    if (tipo_per_cons==1)//si se consultó un periodo COP en la BD
                    {
                    	j = 3;//cuota
                    	incid=6;
                    	tidoc=11;
                    	tiimport=9;
                        /*j = 4;//multa 
						incid=12;
						tidoc=13;	
						tiimport=10;*/
						
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
                no_vig=0;
                
                x2=0;
                cap_ant2=0;
                fecha_vac_2=0;
                no_vig2=0;
                
                ids_core=",";
                ids_core_cuota=",";
                ids_core_multa=",";
                data_core.Rows.Clear();
                data_core_comb.Rows.Clear();
                data_core_multa.Rows.Clear();
                
                error_list = new int[tot_rows+1];
                error_list2 = new int[tot_rows+1];
                error_list3 = new int[tot_rows+1];
                error_list4 = new int[tot_rows+1];

                consultamysql.Columns.Clear();
                consultamysql.Rows.Clear();

                //columnas
                if (n == 1)//directo de BD o de Excel
                {
                    consultamysql.Columns.Add("ID");
                    consultamysql.Columns.Add("REGISTRO PATRONAL");
                    consultamysql.Columns.Add("CAPTURADO SISCOB");
                    consultamysql.Columns.Add("CREDITO");
                    consultamysql.Columns.Add("INCIDENCIA");
                    
                }
                else {
                    consultamysql.Columns.Add("REGISTRO PATRONAL");    
                    consultamysql.Columns.Add("CREDITO");
                    consultamysql.Columns.Add("PERIODO");
                    consultamysql.Columns.Add("INCIDENCIA");
                }

                tablarow.Clear();
                tablarow2.Clear(); 
                data_core.Rows.Clear(); 
                data_core_multa.Rows.Clear(); 
                
               conex2.conectar("base_principal");
                
                if(tipo_per_cons==2){
                	if(tipo_carga==1){//COP
               			label8.Text=rale_cop;
               			label8.Refresh();
                        sql = "SELECT registro_patronal,credito,periodo,incidencia,td FROM rale WHERE tipo_rale=\"COP\" AND (incidencia = \"01\" OR incidencia = \"1\" OR incidencia = \"02\" OR incidencia = \"2\") AND fecha_noti <> \"0000-00-00\"";
                
						//MessageBox.Show("entra a credito_multa");	
               		}else{//RCV
               			label8.Text=rale_rcv;
               			label8.Refresh();
                        sql = "SELECT registro_patronal,credito,periodo,incidencia,td  FROM rale WHERE tipo_rale=\"RCV\" AND (incidencia = \"01\" OR incidencia = \"1\" OR incidencia = \"02\" OR incidencia = \"2\") AND fecha_noti <> \"0000-00-00\"";
                	}
                }else{
                	if(tipo_per_cons==1){
               			label8.Text=rale_cop;
               			label8.Refresh();
                        sql = "SELECT registro_patronal,credito,periodo,incidencia,td  FROM rale WHERE tipo_rale=\"COP\" AND (incidencia = \"01\" OR incidencia = \"1\" OR incidencia = \"02\" OR incidencia = \"2\") AND fecha_noti <> \"0000-00-00\"";
                	}
                }
                
                 data_query=conex2.consultar(sql);
                 // MessageBox.Show("filas: "+data_query.Rows.Count+" |columna1: "+data_query.Columns[0].ColumnName);

                for (i = 0; i < tot_rows; i++)
                {

                    if (n == 1)//si es de bd
                    {
				
                        id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                        f1 = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                        f2 = dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                        f3 = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                        f4 = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
                        f5 = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                     	fecha_not = dataGridView1.Rows[i].Cells[8].FormattedValue.ToString();
                     	
                        reg_pat = Convert.ToString(dataGridView1.Rows[i].Cells[2].FormattedValue);
                        credito = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        cred_mul= Convert.ToString(dataGridView1.Rows[i].Cells[4].Value);
                        per2 = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value);
                        fecha = maskedTextBox2.Text; //incidencia
                        

                        fecha_vac = 0;
                        fecha_vac_2=0;
                        
                        /*if (tipo_credito == 3)
                        {
                            f1_1 = f1.Substring(4, 1);
                        }
                        else
                        {                  
                        	f1_1 = f1.Substring(6, 1);
                        }*/
                        
                        if (f1.Substring(4, 1).Equals("1"))
                        {
                        	fecha_vac = 1;
                        	capturado_ant++;
                        }
                        
                        if(tipo_per_cons==1){
	                        if (f1.Substring(6, 1).Equals("1"))
	                        {
	                        	fecha_vac_2 = 1;
	                        	cap_ant2++;
	                        }
	                         
	                        if ((cred_mul.Length <= 1 || cred_mul.Equals("000000000")) && fecha_vac_2==0)
	                        {
	                        	fecha_vac_2 = 1;
	                        	x2++;
	                        	//error_list2[x] = i;
	                        	//MessageBox.Show("credito multa invalido:"+cred_mul);
	                        }
                        }
                        
                        if ((credito.Length <= 1|| credito.Equals("000000000")) && fecha_vac==0)
                        {
                        	fecha_vac = 1;
                        	x = x + 1;
                        	error_list2[x] = i;
                        	//MessageBox.Show("credito cuota invalido:"+credito);
                        }
                          
                        if (fecha_not.Length < 1  && fecha_vac==0)
                        {
                        	fecha_vac = 1;
                        	gen_list = gen_list + 1;
                        	error_list[gen_list] = i;
                        }
                    

                          /*  if (fecha_vac == 0)
                            {
                                consultamysql.Rows.Add(id,reg_pat,f1,credito,fecha);
                            }
                            else
                            {
                                tablarow.Rows.Add(id, f1, reg_pat, f4, f5, per2, f2, f3);
                                w++;
                            }*/
                    }
                    else
                    {
                        //Carga de Excel o Access
                        reg_pat = Convert.ToString(dataGridView1.Rows[i].Cells[0].FormattedValue);
                        credito = Convert.ToString(dataGridView1.Rows[i].Cells[1].FormattedValue);
                        per2 = Convert.ToString(dataGridView1.Rows[i].Cells[2].FormattedValue);   
                        fecha = maskedTextBox2.Text; //incidencia

                        fecha_vac = 0;
                        
                         if ((reg_pat.Length < 10) || (reg_pat.Length > 10))
                        {
                            fecha_vac = 1;
                            y = y + 1;
                            error_list3[y] = i;
                        }
                          
                        if ((credito.Length < 9) || (credito.Length > 9))
                        {
                            fecha_vac = 1;
                            x = x + 1;
                            error_list2[x] = i;
                        }

                        if ((per2.Length < 6) || (per2.Length > 6))
                        {
                            fecha_vac = 1;
                            z = z + 1;
                            error_list4[z] = i;
                        }

                      /*  if (fecha_vac == 0)
                        {
                            consultamysql.Rows.Add(reg_pat, credito, per2, fecha);
                        }
                        else
                        {
                            tablarow2.Rows.Add(reg_pat, credito, per2);
                            w++;
                        }*/

                    }
                    
                    if (fecha_vac == 0 || fecha_vac_2 == 0){
                    	
                    	if(tipo_carga == 1){//captura en COP
                    		//MessageBox.Show("entro en COP");
                    		
                    		if(checar_rale(reg_pat,credito,per2)==true && fecha_vac==0){
                    			
                    			//MessageBox.Show("entro en COP");
                    			/*wr.WriteLine(reg_pat);
                    			wr.WriteLine(credito);
                    			wr.WriteLine(per2.Substring(4, 2));
                    			wr.WriteLine(per2.Substring(2, 2));
                    			wr.WriteLine(fecha);//incidencia
                    			wr.WriteLine("$");*/
                    			
                    			data_core.Rows.Add(reg_pat,credito,per2,td_rale,incid_rale
                    			                   /*dataGridView1.Rows[i].Cells[tidoc].FormattedValue.ToString(),
                    			                   dataGridView1.Rows[i].Cells[incid].FormattedValue.ToString()*/,conex.leer_config_sub()[4],
                    			                   dataGridView1.Rows[i].Cells[tiimport].FormattedValue.ToString(),
                    			                   dataGridView1.Rows[i].Cells[8].FormattedValue.ToString());
                    			
                    				ids_core+=dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+",";
                    				ids_core_cuota+=dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+",";
                    			//data_core.Merge(data_core_multa);
                    			
                    			if (n == 1){
                    				consultamysql.Rows.Add(id,reg_pat,f1,credito,fecha);
                    			}else{
                    				consultamysql.Rows.Add(reg_pat, credito, per2, fecha);
                    			}
                    		}else{
                    			if(fecha_vac==0){
	                    			no_vig++;
	                    			if (n == 1){
	                    				tablarow.Rows.Add(id,reg_pat,f1, f4, f5, per2, f2, f3);
	                    			}else{
	                    				tablarow2.Rows.Add(reg_pat, credito, per2);
	                    			}
                    			}
                    		}
                    		
                    		if(n==1){
                    			if(fecha_vac_2==0){
                    				no_fecha_vac++;
                    				if(checar_rale(reg_pat,cred_mul,per2)==true){
                    					data_core_multa.Rows.Add(reg_pat,cred_mul,per2,td_rale,incid_rale
                    					                         /*dataGridView1.Rows[i].Cells[13].FormattedValue.ToString(),
                    					                         dataGridView1.Rows[i].Cells[12].FormattedValue.ToString()*/,conex.leer_config_sub()[4],
                    					                         dataGridView1.Rows[i].Cells[10].FormattedValue.ToString(),
                    					                         dataGridView1.Rows[i].Cells[8].FormattedValue.ToString());
                    					
                    					if(ids_core.Contains(","+dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+",")==false){
                    						ids_core+=dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+",";
                    					}
                    					 ids_core_multa+=dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+",";
                    				}else{
                    					no_vig2++;
                    				}
                    			}else{
                    				
                    			}
                    			
                    			
                    		}
                    		
                    	}else{//captura en RCV
                    		if(checar_rale(reg_pat,credito,per2)==true){
                    			/*wr.WriteLine(reg_pat);
                    			wr.WriteLine(per2.Substring(4, 2));
                    			wr.WriteLine(per2.Substring(2, 2));
                    			wr.WriteLine(credito);
                    			wr.WriteLine(fecha);//incidencia
                    			wr.WriteLine("$");*/
                    			
                    			data_core.Rows.Add(reg_pat,credito,per2,td_rale,incid_rale,/*
                    			                   dataGridView1.Rows[i].Cells[tidoc].FormattedValue.ToString(),
                    			                   dataGridView1.Rows[i].Cells[incid].FormattedValue.ToString(),*/conex.leer_config_sub()[4],
                    			                   dataGridView1.Rows[i].Cells[tiimport].FormattedValue.ToString(),
                    			                   dataGridView1.Rows[i].Cells[8].FormattedValue.ToString());
                    			
                    			
                    			ids_core+=dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+",";
                    			
                    			if (n == 1){
                    				consultamysql.Rows.Add(id,reg_pat,f1,credito,fecha);
                    			}else{
                    				consultamysql.Rows.Add(reg_pat, credito, per2, fecha);
                    			}
                    		}else{
                    			no_vig++;
                    			if (n == 1){
                    				tablarow.Rows.Add(id,  reg_pat, f1, f4, f5, per2, f2, f3);
                    			}else{
                    				tablarow2.Rows.Add(reg_pat, credito, per2);
                    			}
                    		}
                    	}
                    	//wr.WriteLine((fecha.Substring(0,2)+fecha.Substring(3,2)+fecha.Substring(8,2)));
                    	
                    }else{
                    	if (n == 1){
                    		tablarow.Rows.Add(id, reg_pat, f1, f4, f5, per2, f2, f3);
                    	}else{
                    		tablarow2.Rows.Add(reg_pat, credito, per2);
                    	}
                    	w++;
                    }
                    //dataGridView1.Rows[i].Visible=false;
                }

               /* wr.WriteLine("%&");*/

                //MessageBox.Show("Archivo Creado con: "+i+" Registros");

                //MessageBox.Show("El Archivo:\n"+fichero.FileName.ToString()+"\nSe ha creado correctamente","¡Exito!");
                //n=0;
                //close file
             /*   wr.Close();

                StreamWriter wr1 = new StreamWriter(@"capturator_inc/temp_aux.txt");
                wr1.WriteLine("0");
               	wr1.WriteLine(consultamysql.Rows.Count.ToString());

                wr1.Close();*/
                dataGridView1.Refresh();
                maskedTextBox2.Enabled=true;
                
            }
            catch (Exception ex)
            {
                //en caso de haber una excepcion que nos mande un mensaje de error
                MessageBox.Show("No se pudo generar la lista\n\nError:\n" + ex, "Error al Generar Lista");
            }
        }
		
		public void preparar(){
			filtro();
			String mensaje="";
			
			if(tipo_per_cons==1){
				data_core_comb.Merge(data_core);
				data_core_comb.Merge(data_core_multa);
			}else{
				data_core_comb.Merge(data_core);
			}
			//MessageBox.Show("num entradas a verificar credito_multa: "+no_fecha_vac);
			int saves=0;
			String solicita,autoriza;
			
			saves=((tot_rows - w)-no_vig);
			
            if (n == 1)
            {
            	if(tipo_per_cons==1){
            		MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron: " + data_core_comb.Rows.Count + " Créditos \n("+(data_core.Rows.Count)+" Créditos Cuota y "+(data_core_multa.Rows.Count)+" Créditos Multa)\nSe omitieron: " + ((gen_list+x+capturado_ant+no_vig)+(gen_list+x2+cap_ant2+no_vig2)) + " Créditos\n("+(gen_list+x+capturado_ant+no_vig)+" Créditos Cuota y "+(gen_list+x2+cap_ant2+no_vig2)+" Créditos Multa)\n\n" +
            		             "" + (gen_list*2) + " Créditos por no contar con una fecha de notificación válida\n("+gen_list+" Créditos Cuota y "+gen_list+" Créditos Multa)\n" +
            		             "" + (x+x2) + " Créditos por no contar con el crédito elegido válido\n("+x+"Créditos Cuota y "+x2+" Créditos Multa)\n" +
            		             "" + (capturado_ant+cap_ant2) + " Créditos que ya estaban ingresados\n("+capturado_ant+" Créditos Cuota y "+cap_ant2+" Créditos Multa)\n" +
	                             "" + (no_vig+no_vig2) + " Créditos que ya no están Vigentes o que ya no están en la Incidencia 01\n("+no_vig+" Créditos Cuota y "+no_vig2+" Créditos Multa)\n\n", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            	}else{
            		MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron: " + data_core_comb.Rows.Count + " registros\nSe omitieron: " + (gen_list+x+capturado_ant+no_vig) + " registros\n\n" +
	                             "" + gen_list + " registros por no contar con una fecha de notificación válida\n" +
	                             "" + x + " registros por no contar con el crédito elegido válido\n" +
	                             "" + capturado_ant + " registros que ya estaban ingresados\n" +
	                             "" + no_vig + " registros que ya no están Vigentes o que ya están en otra Incidencia\n\n", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            	}
            	dataGridView1.DataSource=null;
            	dataGridView1.DataSource = tablarow;
            	label5.Text="Registros: "+dataGridView1.RowCount;
            }
            else
            {
            	MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron:" + saves + " registros\nSe omitieron: " + (w+no_vig) + " registros\n\n" +
                         /*   "" + y + " registros por no contar con un registro patronal válido\n" +         
                            "" + gen_list + " registros por no contar con una fecha de notificación válida\n" +
                             "" + z + " registros por no contar con un  periodo válido\n" +
                             "" + x + " registros por no contar con un crédito válido\n\n" +*/
                             "Ahora puede iniciar la captura automática", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = tablarow2;
                label5.Text="Registros: "+dataGridView1.RowCount;
            }
            
            if(data_core_comb.Rows.Count >0){
            	data_core_comb.DefaultView.Sort="RP ASC,CREDITO ASC";
	            Depuracion.Visor_Reporte visor1 = new Depuracion.Visor_Reporte();
	            solicita = conex.leer_config_sub()[8]+"\nJefe Oficina de Emisiones y P.O.";
	            autoriza = conex.leer_config_sub()[10]+"\nJefe de Oficina Para Cobros";
				visor1.envio_inc31(data_core_comb,tipo_carga,solicita,autoriza);
                visor1.Show();
                folio_final=visor1.id_core();
                //MessageBox.Show("folio_colocado:"+visor1.id_core());
                
                guardar_info_core();
            }
			             
            label5.Text = "Registros: " + dataGridView1.RowCount;
            button10.Enabled = false;
            inci=maskedTextBox2.Text;
            maskedTextBox2.Enabled=false;
            
            if(saves > 0){
	            button4.Enabled = true;  
            }else{
            	button4.Enabled = false;
            }
		}
		
		public void preparar_rcv(){
			if(tipo_per_cons==2){
				rcv_cop = new Carga();
				rcv_cop.ShowDialog(this);
				tipo_carga = rcv_cop.mandar();  //1= COP|2=RCV
				//MessageBox.Show(tipo_carga.ToString());
				if(tipo_carga<1){
					MessageBox.Show("Tiene que seleccionar el modo de captura de acuerdo a la sección en la que se va a efectuar la captura automática en SISCOB");
				}else{
					preparar();
				}
			}else{
				tipo_carga=1;
				preparar();
			}
		}
		
		public void puente_res(int actual)
		{
			tipo_carga=actual;
		}
		
		public void finalizar()
        {
        	//tipo_carga = n;
        	//MessageBox.Show("tipo_car:" + tipo_car + "  tipo_cred" + tipo_cred);
        	//Proceso final de captura en siscob

        	//try
        	//{ 
        		StreamReader rdr = new StreamReader(@"capturator_inc/errores_siscob.txt");
        		StreamReader rdr1 = new StreamReader(@"capturator_inc/temp_aux.txt");
        		
        		
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
                                                                    if (tipo_credito == 4)
                                                                    {
                                                                        nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
                                                                        nvo_stat_cap = nvo_stat_cap.Remove(6,1);
                                                                        nvo_stat_cap = nvo_stat_cap.Insert(6,"2");
                                                                        //nvo_stat_cap = "2" + nvo_stat_cap.Substring(6, 2);
                                                                    }
                                                                    else
                                                                    {
                                                                        nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
                                                                        nvo_stat_cap = nvo_stat_cap.Remove(4,1);
                                                                        nvo_stat_cap = nvo_stat_cap.Insert(4,"2");
                                                                    }
                                                                    sql = "UPDATE datos_factura SET capturado_siscob =\"" + nvo_stat_cap + "\"  WHERE id =" + dataGridView3.Rows[i].Cells[0].Value.ToString() + "";
                                                                    conex.consultar(sql);
                                                                    k = k + 1;
                                                                    candado = 1;
                                                                    guardar_arch.Rows[i][3]="ERROR";
                                                                }
                                                            
        			                         				//mani.toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k);
        			                         				//mani.statusStrip1.Refresh();
        			                         				
        			                         				toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k);
        			                         				statusStrip1.Refresh();
        			                         				
        			                         			}

        			                         			if (candado == 0)
        			                         			{
        			                         				ant_stat_cap = dataGridView3.Rows[0].Cells[2].Value.ToString();
        			                         				if (tipo_credito == 4)
        			                         				{
        			                         					nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         					nvo_stat_cap = nvo_stat_cap.Remove(6,1);
                                                                nvo_stat_cap = nvo_stat_cap.Insert(6,"1");
        			                         				}
        			                         				else
        			                         				{
        			                         					nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         					nvo_stat_cap = nvo_stat_cap.Remove(4,1);
                                                                nvo_stat_cap = nvo_stat_cap.Insert(4,"1");
        			                         				}
        			                         				sql = "UPDATE datos_factura SET capturado_siscob =\""+nvo_stat_cap+"\", incidencia=\""+inci+"\" WHERE id =" + dataGridView3.Rows[i].Cells[0].Value.ToString() + "";
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
        			                         				if (tipo_credito == 4 )
        			                         			{
        			                         				nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         				nvo_stat_cap = nvo_stat_cap.Remove(6,1);
                                                            nvo_stat_cap = nvo_stat_cap.Insert(6,"1");
        			                         			}
        			                         			else
        			                         			{
        			                         				nvo_stat_cap = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         				nvo_stat_cap = nvo_stat_cap.Remove(4,1);
                                                            nvo_stat_cap = nvo_stat_cap.Insert(4,"1");
        			                         			}
        			                         			sql = "UPDATE datos_factura SET capturado_siscob = \""+nvo_stat_cap+"\", incidencia=\""+inci+"\" WHERE id =" + dataGridView3.Rows[i].Cells[0].Value.ToString() + "";
        			                         			//MessageBox.Show(sql+" "+tipo_credito);
        			                         			conex.consultar(sql);
        			                         			//toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k) + "  lineas leidas: " + j + "    buscando: " + errores_sis[j];
        			                         			
        			                         			//mani.toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k);
        			                         			//mani.statusStrip1.Refresh();
        			                         		    }
        			                         			toolStripStatusLabel1.Text ="Procesando Resultados. Registros leidos: " + i + ".   Erroneos por asignar: " + (tot_errs - k)+".   Omitidos: "+(i-acierto_act)+".";
	        			                         		statusStrip1.Refresh();
	        			                         		guardar_arch.Rows[i][3]="CAPTURADO";
        			                         		}
        			                         		candado = 0;
        			                         		i++;
        			                         	} while (i < tot_rows1);

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
											
        									conex.guardar_evento("Se Capturaron en Siscob "+acierto_act+" Registros de Cambio de Incidencia "+maskedTextBox2.Text+" de "+tipo_ev+" de un total de "+tot_rows1);
        			   //MessageBox.Show(" Registros Actualizados.\n Ya puede manipular el equipo \n Ha finalizado todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

        		}
        		else
        		{  //finalizar cuando es archivo excel y access
        			consultamysql.Columns.Add("CAPTURADO SISCOB");
        			

        			Invoke(new MethodInvoker(delegate
        			                         {
        			                         	//MainForm mani = (MainForm)this.MdiParent;
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
                                                            
        			                         				if (((dataGridView3.Rows[i].Cells[0].Value.ToString()).Equals(reg_pat)) && ((dataGridView3.Rows[i].Cells[1].Value.ToString()).Equals(credito)))
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
        			                         			
        			                         			toolStripStatusLabel1.Text = "Procesando Resultados. Registros leidos: " + (i+1) + ".   Erroneos por asignar: " + (tot_errs - k)+".  Omitidos: "+(i-acierto_act)+".";
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
        			                         		credito = dataGridView1.Rows[i].Cells[1].Value.ToString();
        			                         		per2 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                                    fecha = "1";// dataGridView1.Rows[i].Cells[3].Value.ToString();

                                                    consultamysql.Rows.Add(reg_pat,credito,per2,fecha,"0");
                                                    i++;
                                                    //reg_pat, per2, credito, fecha, "0"
                                                } 
                                                dataGridView3.Refresh();
        			                         	label5.Text="Registros: "+dataGridView3.RowCount;  
												dataGridView3.Visible = true;        			                         	

        			                              //*****Guardar Archivo de Resultados
        			                         			//nom_save = dialog_save.FileName;
        			                         			//open file
        			                         			StreamWriter wr2 = new StreamWriter(nom_save);
        			                         			i = 0;
        			                         			tot_rows=dataGridView3.RowCount;
        			                         			wr2.WriteLine("REGISTRO PATRONAL,CREDITO,PERIODO,INCIDENCIA,CAPTURADO SISCOB");
        			                         			do
        			                         			{
        			                         				reg_pat = dataGridView3.Rows[i].Cells[0].Value.ToString();
        			                         				per2 = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         				credito = dataGridView3.Rows[i].Cells[1].Value.ToString();
        			                         				fecha = dataGridView3.Rows[i].Cells[3].Value.ToString();
        			                         				f1 = dataGridView3.Rows[i].Cells[4].Value.ToString();
        			                         				wr2.WriteLine(reg_pat + "," + credito + "," + per2  + "," + fecha + "," + f1);
        			                         				i++;
        			                         			} while (i < tot_rows);
        			                         			wr2.Close();
        			                         		
        			                         		//MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
        			                         		if(mochado==0){
        			                         			MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nHa terminado correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			                         		}else{
        			                         			MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nSe guardo el resultado de: "+acierto_act+" registros de un total de: "+tot_rows+".  \nHa terminado de forma parcial correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			                         		}
												conex.guardar_evento("Se Capturaron en Siscob "+acierto_act+" Registros de Cambio de Incidencia "+maskedTextBox2.Text+" de "+tipo_ev+" de un total de "+tot_rows);
        			                         }));
        		}
        		
        	/*}
        	catch (Exception e1)
        	{
        		MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser terminado adecuadamente.\n\n Error:\n" + e1, "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
        	}*/
        	Invoke(new MethodInvoker(delegate
        	                         {
        	                         	//MainForm mani1 = (MainForm)this.MdiParent;
        	                         	//mani1.toolStripStatusLabel1.Text = "Listo";
        	                         	//mani1.statusStrip1.Refresh();
        	                         	toolStripStatusLabel1.Text = "Listo";
        	                         	statusStrip1.Refresh();
        	                         	button18.Visible=false;
        	                         	button4.Visible=true;
        	                         	button10.Visible=true;
        	                         	button3.Enabled=true;
        	                         	
        	                         }));

        }
		
		public int guardar_resultados(){

        		 SaveFileDialog dialog_save = new SaveFileDialog();
        		 if(n==0){
        	     	dialog_save.Filter = "Archivos de Excel (*.CSV)|*.CSV"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
        		 }else{
        		 	dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
        		 }
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
		
		public void guardar_info_core(){
			try{
			conex3.conectar("base_principal");
			
			int iii=0;
			String rp,cred,peri,folio_not,nvo_folio="",tipo_folio="";
			ids_core=ids_core.Substring(1,ids_core.Length-2);
			//MessageBox.Show("Num_ids: "+(ids_core.CharCount(',')+1));
			data_ids_folio_core=conex3.consultar("SELECT id,folio_not_core,registro_patronal2,credito_cuotas,credito_multa,periodo FROM datos_factura WHERE id in ("+ids_core+") ");
			//MessageBox.Show("num_ids_consultados= "+data_ids_folio_core.Rows.Count);
			DataView vista1 = new DataView(data_ids_folio_core);
			
			data_guarda_folio_core.Rows.Clear();
			
			//compara ids de las cuotas
			while(iii<data_core.Rows.Count){
				rp=data_core.Rows[iii][0].ToString();
				cred=data_core.Rows[iii][1].ToString();
				peri=data_core.Rows[iii][2].ToString();
				
				vista1.RowFilter="registro_patronal2='"+rp+"' AND credito_cuotas='"+cred+"' AND periodo='"+peri+"'";
				dataGridView2.DataSource=vista1;
				if(dataGridView2.RowCount>0){
					
					folio_not=dataGridView2.Rows[0].Cells[1].Value.ToString();
					
					if(tipo_carga==1){
						tipo_folio="COP";
					}else{
						if(tipo_carga==2){
							tipo_folio="RCV";
						}
					}
					//MessageBox.Show(ids_core_multa+"|"+dataGridView2.Rows[0].Cells[0].Value.ToString());
					//verifica si se manda solo el credito de la cuota o el de la multa también  
					if(ids_core_multa.Contains(","+dataGridView2.Rows[0].Cells[0].Value.ToString()+",")==true){
						if(folio_not.Length<=1){
							data_guarda_folio_core.Rows.Add(dataGridView2.Rows[0].Cells[0].Value.ToString(),(tipo_folio+"_"+folio_final+"_3"));
						}else{
							data_guarda_folio_core.Rows.Add(dataGridView2.Rows[0].Cells[0].Value.ToString(),(folio_not+","+tipo_folio+"_"+folio_final+"_3"));
						}
					}else{
						if(folio_not.Length<=1){
							data_guarda_folio_core.Rows.Add(dataGridView2.Rows[0].Cells[0].Value.ToString(),(tipo_folio+"_"+folio_final+"_1"));
						}else{
							data_guarda_folio_core.Rows.Add(dataGridView2.Rows[0].Cells[0].Value.ToString(),(folio_not+","+tipo_folio+"_"+folio_final+"_1"));
						}
					}
				}
				iii++;
			}
			
			iii=0;
			//compara ids de las multas
			while(iii<data_core_multa.Rows.Count){
				rp=data_core_multa.Rows[iii][0].ToString();
				cred=data_core_multa.Rows[iii][1].ToString();
				peri=data_core_multa.Rows[iii][2].ToString();
				
				vista1.RowFilter="registro_patronal2='"+rp+"' AND credito_multa='"+cred+"' AND periodo='"+peri+"'";
				dataGridView2.DataSource=vista1;
				if(dataGridView2.RowCount>0){
					folio_not=dataGridView2.Rows[0].Cells[1].Value.ToString();
					
					if(tipo_carga==1){
						tipo_folio="COP";
					}else{
						if(tipo_carga==2){
							tipo_folio="RCV";
						}
					}
					//verifica si se manda el puro credito de la multa
					if(ids_core_cuota.Contains(","+dataGridView2.Rows[0].Cells[0].Value.ToString()+",")==false){
						if(folio_not.Length<=1){
							data_guarda_folio_core.Rows.Add(dataGridView2.Rows[0].Cells[0].Value.ToString(),(tipo_folio+"_"+folio_final+"_2"));
						}else{
							data_guarda_folio_core.Rows.Add(dataGridView2.Rows[0].Cells[0].Value.ToString(),(folio_not+","+tipo_folio+"_"+folio_final+"_2"));
						}
					}
				}
				iii++;
			}
			
			iii=0;
			conex4.conectar("base_principal");
			while(iii <data_guarda_folio_core.Rows.Count){
				conex4.consultar("UPDATE datos_factura SET folio_not_core=\""+data_guarda_folio_core.Rows[iii][1].ToString()+"\" WHERE id="+data_guarda_folio_core.Rows[iii][0].ToString()+" ");
				iii++;
			}
			conex4.cerrar();
			//MessageBox.Show("Filas Consulta Original: "+data_ids_folio_core.Rows.Count+"\n Filas de data_guarda_folio:"+data_guarda_folio_core.Rows.Count);
			}catch(Exception exc){
				MessageBox.Show("Ocurrió un Error al momento de guardar la información de la core en la Base de Datos:\n\n"+exc,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		
		}
		
		void JarviscobLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            //this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2)-30;
            //this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;
            //this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);

            comboBox2.SelectedItem = "CUALQUIERA";
			
			tablarow.Columns.Add("ID");
			tablarow.Columns.Add("REGISTRO PATRONAL");
			tablarow.Columns.Add("CAPTURADO SISCOB");
			tablarow.Columns.Add("CRÉDITO CUOTA");	
			tablarow.Columns.Add("CRÉDITO MULTA");
			tablarow.Columns.Add("PERIODO");
			tablarow.Columns.Add("INCIDENCIA");
			tablarow.Columns.Add("NOMBRE PERIODO");
			
			
			tablarow2.Columns.Add("REGISTRO PATRONAL");
            tablarow2.Columns.Add("CRÉDITO");
            tablarow2.Columns.Add("PERIODO");
            //mtablarow2.Columns.Add("INCIDENCIA");			
			
			toolTip1.SetToolTip(label9, "0 - No Capturado\n1 - Capturado\n2 - Error al Capturar");
			toolTip1.SetToolTip(button2, "Consultar\nen la BD");
			
			panel1.Visible=true;
			panel2.Visible=false;
			//label10.Visible=true;
			//radioButton1.Visible=true;
			//radioButton2.Visible=true;
			
			llenar_Cb2_todos();
			dateTimePicker2.Value = System.DateTime.Today.AddDays(-27);
            dateTimePicker1.Value = System.DateTime.Today.AddDays(-17);            
            dateTimePicker1.MaxDate = System.DateTime.Today.AddDays(-17);
            //fecha_antes = System.DateTime.Today.AddDays(-365);
            conex2.conectar("base_principal");
			dataGridView2.DataSource=conex.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_COP%\" order by idlog_eventos desc");
			rale_cop="Rale Ingresado el Día: "+dataGridView2.Rows[0].Cells[1].FormattedValue.ToString().Substring(0,10);
			dataGridView2.DataSource=conex.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_RCV%\" order by idlog_eventos desc");
			rale_rcv="Rale Ingresado el Día: "+dataGridView2.Rows[0].Cells[1].FormattedValue.ToString().Substring(0,10);
			label8.Text=rale_cop;
			label8.Refresh();
			
			data_core.Columns.Add("RP");
            data_core.Columns.Add("CREDITO");
            data_core.Columns.Add("PERIODO");
            data_core.Columns.Add("TD");
 			data_core.Columns.Add("INC");
            data_core.Columns.Add("SUB");
           	data_core.Columns.Add("IMPORTE");
            data_core.Columns.Add("FOLIO");
            data_core.Columns.Add("FECHA");
            
            data_core_multa.Columns.Add("RP");
            data_core_multa.Columns.Add("CREDITO");
            data_core_multa.Columns.Add("PERIODO");
            data_core_multa.Columns.Add("TD");  
 			data_core_multa.Columns.Add("INC");            
            data_core_multa.Columns.Add("SUB");
           	data_core_multa.Columns.Add("IMPORTE");
            data_core_multa.Columns.Add("FOLIO");
            data_core_multa.Columns.Add("FECHA"); 
            
            data_core_comb.Columns.Add("RP");
            data_core_comb.Columns.Add("CREDITO");
            data_core_comb.Columns.Add("PERIODO");
            data_core_comb.Columns.Add("TD");
 			data_core_comb.Columns.Add("INC");
            data_core_comb.Columns.Add("SUB");
           	data_core_comb.Columns.Add("IMPORTE");
            data_core_comb.Columns.Add("FOLIO");
            data_core_comb.Columns.Add("FECHA");
            
            data_guarda_folio_core.Columns.Add("ID");
           	data_guarda_folio_core.Columns.Add("FOLIO");
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			carga_excel();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			dataGridView3.Visible = false;
            dataGridView3.DataSource = null;
            //maskedTextBox2.Text="";
			consultar_bd();
			maskedTextBox2.Enabled=true;		
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(comboBox1.SelectedItem == null){
				MessageBox.Show("Selecciona una Hoja");
			}else{
			//tipo_carga=1;
			n = 0;
            dataGridView3.Visible = false;
            dataGridView3.DataSource = null;
            //maskedTextBox2.Text="";
			cargar_hoja_excel();
			maskedTextBox2.Enabled=true;
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{  		 
			 if (guardar_resultados()==1){
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
					System.IO.File.Delete(@"capturator_inc/errores_siscob.txt");
	        	    System.IO.File.Delete(@"capturator_inc/acierto_siscob.txt");
	        	    System.IO.File.Delete(@"capturator_inc/aciertos_siscob.txt");
	        	    //Crear archivos nuevos
	                fichero = System.IO.File.Create(@"capturator_inc/errores_siscob.txt");
					fichero1= System.IO.File.Create(@"capturator_inc/acierto_siscob.txt");
					fichero2= System.IO.File.Create(@"capturator_inc/aciertos_siscob.txt");
	                ruta = fichero.Name;
	                fichero.Close();
	                fichero1.Close();
	                fichero2.Close();
	                
	                StreamWriter wr1 = new StreamWriter(@"capture_inc.bat");
	                wr1.WriteLine("@echo off");
	                wr1.WriteLine("C:");
	                wr1.WriteLine("cd "+ruta.Substring(0,(ruta.Length-19)));
	                
	                if(tipo_carga==1){
	                wr1.WriteLine("start captron.exe");
	                tipo_ev="COP";
	                }else{
	                wr1.WriteLine("start captron_rcv.exe"); 
	                tipo_ev="RCV";
	                }
	                wr1.Close();
	               /* MainForm mani = (MainForm)this.MdiParent;
					mani.capt_siscob(consultamysql,tipo_credito,n);
					n=0;*/
	               button18.Visible = false;
	               button3.Enabled = false;
        	       button4.Visible=false;
        	       button10.Visible=false;
				   System.Diagnostics.Process.Start(@"capture_inc.bat");
				   conex.guardar_evento("Se Mando a capturar en Siscob "+consultamysql.Rows.Count.ToString()+" Registros de Cambio de Incidencia "+maskedTextBox2.Text+" de "+tipo_ev);
					
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
	
		void Button5Click(object sender, EventArgs e)
		{
			panel1.Visible=true;
			panel2.Visible=false;
			label10.Visible=true;
			radioButton1.Visible=true;
			radioButton2.Visible=true;
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			panel2.Visible=true;
			label10.Visible=false;
			radioButton1.Visible=false;
			radioButton2.Visible=false;
		}
		
		void Button10Click(object sender, EventArgs e)
		{	
			DialogResult res = MessageBox.Show("Está pór generar una CORE de envío a la INCIDENCIA 31\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk);
			if(res == DialogResult.Yes){
				preparar_rcv();
			}
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			
		}
		
		void Button12Click(object sender, EventArgs e)
		{
		
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			 
		}
		
		void Button15Click(object sender, EventArgs e)
		{
			 
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
		
		void FinToolStripMenuItem1Click(object sender, EventArgs e)
		{
			finalizar();
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.Text.Length==2){
				filtr = Convert.ToInt32(maskedTextBox2.Text);
				if(filtr<56){
					button10.Enabled=true;
					button10.Focus();
				}
				
			}else{
				button10.Enabled=false;
			}
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(maskedTextBox2.Text.Length==2){
				if (e.KeyChar == (char)(Keys.Enter)){
					DialogResult res = MessageBox.Show("Está pór generar una CORE de envío a la INCIDENCIA "+maskedTextBox2.Text+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk);
					if(res == DialogResult.Yes){
						preparar_rcv();
					}
				}
			}
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

        private void button11_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel3.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
        	//maskedTextBox2.Clear();
            consultar_fecha_not();
            button10.Enabled=true;
			button10.Focus();
            //maskedTextBox2.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel3.Visible = false;
        }
		
		void RadioButton3CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton3.Checked==true){
				//radioButton1.Enabled=false;
				//radioButton2.Enabled=false;
			}
		}
		
		void RadioButton4CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton4.Checked==true){
				//radioButton1.Enabled=true;
				//radioButton2.Enabled=true;
			}
		}
	}
}

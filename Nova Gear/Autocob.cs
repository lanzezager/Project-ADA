/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 17/02/2016
 * Hora: 03:23 p. m.
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
	/// Description of Autocob.
	/// </summary>
	public partial class Autocob : Form
	{
		public Autocob()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String archivo,ext,cad_con,tabla,cons_exc,hoja,sql,sql2,t_d,t_c,t_p_c,per,reg_pat,per2,credito,fecha,f1,f2,f3,f4,f5,ruta,id,cad_fech_des,cad_fech_has,nom_save,f1_1,nvo_stat_cap,ant_stat_cap,cons_fech_cop,inci,acierto_reg_cre,importe,retiro_p,rcv_o,rcv_p,tipo_ev;
		int filas=0,i=0,tot_rows=0,tot_cols=0,n=0,j=0,tipo_carga=0,fecha_vac=0,gen_list=0,x=0,tipo_credito=0,capturado_ant=0,y=0,z=0,w=0,tot_errs=0,candado=0,k=0,tot_rows1=0,filtro_cpo=0,filtr=0,m=0,acierto_act=0,mochado=0,cont_omis=0,dot_pos=0;
		int[] error_list,error_list2,error_list3,error_list4;
		string[] errores_sis,aciertos_sis;
		double totr=0,importe_d=0;
		DialogResult respuesta,respuesta2;
		FileStream fichero,fichero1,fichero2;
		
		Carga rcv_cop;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable data_3 = new DataTable();
		
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
						//maskedTextBox2.Enabled=true;
						comboBox3.Enabled=true;

						//estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView1.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView1.ColumnCount);
                        i = 0;			

						if(tot_rows>0){
							//maskedTextBox2.Enabled=true;
							comboBox3.Enabled=true;							
						}else{
							//maskedTextBox2.Enabled=false;
							comboBox3.Enabled=false;
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
		
		public void filtro()
        {
            try
            {
            	//maskedTextBox2.Enabled=false;
            	comboBox3.Enabled=false;
            	//Borrar archivos para comenzar de 0
            	System.IO.File.Delete(@"capturator_am/temp.txt");
        	    //Crear archivos nuevos
                //System.IO.File.Create(@"capturator/temp.txt");
                //Abrir archivo
                StreamWriter wr = new StreamWriter(@"capturator_am/temp.txt");

                if (radioButton1.Checked)
                {
                    j = 3;
                }
                else
                {
                    if (radioButton2.Checked)
                    {
                        j = 4;   
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
                error_list = new int[tot_rows+1];
                error_list2 = new int[tot_rows+1];
                error_list3 = new int[tot_rows+1];
                error_list4 = new int[tot_rows+1];

                consultamysql.Columns.Clear();
                consultamysql.Rows.Clear();

                
                //columnas
                
                if(tipo_carga == 1){    //CARGA COP
                	if (n == 1) //CARGAR BD
                	{
                		consultamysql.Columns.Add("ID");
                		consultamysql.Columns.Add("REGISTRO PATRONAL");
                		consultamysql.Columns.Add("CAPTURADO SISCOB");
                		consultamysql.Columns.Add("CREDITO");
                		consultamysql.Columns.Add("IMPORTE");
                		
                	}else {  //CARGAR EXCEL
                		consultamysql.Columns.Add("REGISTRO PATRONAL");
                		consultamysql.Columns.Add("CREDITO");
                		consultamysql.Columns.Add("PERIODO");
                		consultamysql.Columns.Add("IMPORTE");
                	}
                	
                	if(tablarow2.Columns.Count>4){
                		tablarow2.Columns.RemoveAt(4);
                		tablarow2.Columns.RemoveAt(4);
                		tablarow2.Columns.RemoveAt(4);
                	}
                }else{//CARGA RCV
                	if (n == 1)
                	{  //CARGAR BD
                		consultamysql.Columns.Add("ID");
                		consultamysql.Columns.Add("REGISTRO PATRONAL");
                		consultamysql.Columns.Add("CAPTURADO SISCOB");
                		consultamysql.Columns.Add("CREDITO");
                		consultamysql.Columns.Add("IMPORTE");
                		
                	}else {  //CARGAR EXCEL
                		consultamysql.Columns.Add("REGISTRO PATRONAL");
                		consultamysql.Columns.Add("CREDITO");
                		consultamysql.Columns.Add("PERIODO");
                		consultamysql.Columns.Add("IMPORTE TOTAL");
                		consultamysql.Columns.Add("IMPORTE RETIRO PATRONAL");
                		consultamysql.Columns.Add("IMPORTE RCV OBRERO");
                		consultamysql.Columns.Add("IMPORTE RCV PATRONAL");
                		
                		if(tablarow2.Columns.Count == 4){
	                		tablarow2.Columns.Add("IMPORTE RETIRO PATRONAL");
	                		tablarow2.Columns.Add("IMPORTE RCV OBRERO");
	                		tablarow2.Columns.Add("IMPORTE RCV PATRONAL");
                		}
                	}
                }

                tablarow.Clear();
                tablarow2.Clear();

                for (i = 0; i < tot_rows; i++)
                {

                    if (n == 1)
                    {
				
                        id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                        f1 = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                        f2 = dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                        f3 = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                        f4 = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
                        f5 = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                     
                        reg_pat = Convert.ToString(dataGridView1.Rows[i].Cells[2].FormattedValue);
                        credito = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        per2 = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value);
                        fecha = comboBox3.SelectedItem.ToString(); //ajuste manual

                        fecha_vac = 0;
                        
                        if (tipo_credito == 3)
                        {
                            f1_1 = f1.Substring(4, 1);
                        }
                        else
                        {                  
                        	f1_1 = f1.Substring(6, 1);
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

                           /* if (fecha.Length <= 1)
                            {
                                fecha_vac = 1;
                                gen_list = gen_list + 1;
                                error_list[gen_list] = i;
                            }*/

                            if (fecha_vac == 0)
                            {
                                consultamysql.Rows.Add(id,reg_pat,f1,credito,fecha);
                            }
                            else
                            {
                                tablarow.Rows.Add(id, f1, reg_pat, f4, f5, per2, f2, f3);
                                w++;
                            }
                    }
                    else
                    {
                        //Carga de Excel o Access
                        //fecha = maskedTextBox2.Text; //ajuste manual
                        fecha = comboBox3.SelectedItem.ToString(); //ajuste manual
                        reg_pat = Convert.ToString(dataGridView1.Rows[i].Cells[0].FormattedValue);
                        credito = Convert.ToString(dataGridView1.Rows[i].Cells[1].FormattedValue);
                        per2 = Convert.ToString(dataGridView1.Rows[i].Cells[2].FormattedValue);
                        importe =  Convert.ToString(dataGridView1.Rows[i].Cells[3].FormattedValue);
                        
                        if(tipo_carga==2){
                        	retiro_p =  Convert.ToString(dataGridView1.Rows[i].Cells[4].FormattedValue);
                        	rcv_o =  Convert.ToString(dataGridView1.Rows[i].Cells[5].FormattedValue);
                        	rcv_p =  Convert.ToString(dataGridView1.Rows[i].Cells[6].FormattedValue);	
                        }
                        
                        fecha_vac = 0;
                        
                        if ((importe.Contains("."))==true){
                        }else{
                        	importe = importe+".00";
                        }
                        
                        if(double.TryParse(importe,out importe_d)){
                        	importe_d = Convert.ToDouble(importe);
                        	
                        	/*if(importe_d > 99999.99){
                        		fecha_vac=1;
                        	}*/
                        	
                        	importe = importe_d.ToString();
                        	
                        	if ((importe.Contains("."))==true){
                       		}else{
                        		importe = importe+".00";
                        	}
                        	
                        	if(importe.LastIndexOf('.')==(importe.Length-2)){
                        		importe = importe+"0";
                        	}else{
                        		if(((importe.Length)-(importe.LastIndexOf('.')+1))>2){
                        			importe=importe.Substring(0,(importe.LastIndexOf('.')+3));
                        		}
                        	}
 	
                        }else{
                        	fecha_vac=1;
                        	//MessageBox.Show(importe);
                        	importe=Convert.ToString(dataGridView1.Rows[i].Cells[3].FormattedValue);
                        }
                        
                        
                        //RETIRO_P +++++++++++++++++++++++++++
                        
                        if(tipo_carga==2){
                        	if(fecha_vac==0){
                        		if ((retiro_p.Contains("."))==true){
                        		}else{
                        			retiro_p = retiro_p+".00";
                        		}
                        		
                        		if(double.TryParse(retiro_p,out importe_d)){
                        			importe_d = Convert.ToDouble(retiro_p);
                        			
                        			/*if(importe_d > 99999.99){
                        		fecha_vac=1;
                        	}*/
                        			
                        			retiro_p = importe_d.ToString();
                        			
                        			if ((retiro_p.Contains("."))==true){
                        			}else{
                        				retiro_p = retiro_p+".00";
                        			}
                        			
                        			if(retiro_p.LastIndexOf('.')==(retiro_p.Length-2)){
                        				retiro_p = retiro_p+"0";
                        			}else{
                        				if(((retiro_p.Length)-(retiro_p.LastIndexOf('.')+1))>2){
                        					retiro_p=retiro_p.Substring(0,(retiro_p.LastIndexOf('.')+3));
                        				}
                        			}
                        			
                        		}else{
                        			fecha_vac=1;
                        			//MessageBox.Show(importe);
                        			retiro_p=Convert.ToString(dataGridView1.Rows[i].Cells[3].FormattedValue);
                        		}
                        	}
                        	//RCV_O ++++++++++++++++++++++++++++++++++++
                        	if(fecha_vac==0){
                        		if ((rcv_o.Contains("."))==true){
                        		}else{
                        			rcv_o = rcv_o+".00";
                        		}
                        		
                        		if(double.TryParse(rcv_o,out importe_d)){
                        			importe_d = Convert.ToDouble(rcv_o);
                        			
                        			/*if(importe_d > 99999.99){
                        		fecha_vac=1;
                        	}*/
                        			
                        			rcv_o = importe_d.ToString();
                        			
                        			if ((rcv_o.Contains("."))==true){
                        			}else{
                        				rcv_o = rcv_o+".00";
                        			}
                        			
                        			if(rcv_o.LastIndexOf('.')==(rcv_o.Length-2)){
                        				rcv_o = rcv_o+"0";
                        			}else{
                        				if(((rcv_o.Length)-(rcv_o.LastIndexOf('.')+1))>2){
                        					rcv_o=rcv_o.Substring(0,(rcv_o.LastIndexOf('.')+3));
                        				}
                        			}
                        			
                        		}else{
                        			fecha_vac=1;
                        			//MessageBox.Show(importe);
                        			rcv_o=Convert.ToString(dataGridView1.Rows[i].Cells[3].FormattedValue);
                        		}
                        	}
                        	//RCV_P ++++++++++++++++++++++++++++++++++++++++++++++
                        	if(fecha_vac==0){
                        		if ((rcv_p.Contains("."))==true){
                        		}else{
                        			rcv_p = rcv_p+".00";
                        		}
                        		
                        		if(double.TryParse(rcv_p,out importe_d)){
                        			importe_d = Convert.ToDouble(rcv_p);
                        			
                        			/*if(importe_d > 99999.99){
                        		fecha_vac=1;
                        	}*/
                        			
                        			rcv_p = importe_d.ToString();
                        			
                        			if ((rcv_p.Contains("."))==true){
                        			}else{
                        				rcv_p = rcv_p+".00";
                        			}
                        			
                        			if(rcv_p.LastIndexOf('.')==(rcv_p.Length-2)){
                        				rcv_p = rcv_p+"0";
                        			}else{
                        				if(((rcv_p.Length)-(rcv_p.LastIndexOf('.')+1))>2){
                        					rcv_p=rcv_p.Substring(0,(rcv_p.LastIndexOf('.')+3));
                        				}
                        			}
                        			
                        		}else{
                        			fecha_vac=1;
                        			//MessageBox.Show(importe);
                        			rcv_p=Convert.ToString(dataGridView1.Rows[i].Cells[3].FormattedValue);
                        		}
                        	}
                        
                        
                        
                        if(fecha_vac==0){
                        		//MessageBox.Show((((Convert.ToDouble(retiro_p))+(Convert.ToDouble(rcv_o))+(Convert.ToDouble(rcv_p)))).ToString()+","+(Convert.ToDouble(importe)).ToString());
                        		if(((Convert.ToDouble(importe))).ToString().Equals(((Convert.ToDouble(retiro_p))+(Convert.ToDouble(rcv_o))+(Convert.ToDouble(rcv_p))).ToString())){
	                        	
                        	}else{
                        		fecha_vac=1;
                        	}
                        }
                        }//++++++++++++++++++++++++++++++++++++++++++++++
                        
                        
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

                        if ((importe.Contains(".")==true))
                        {}else{
							//MessageBox.Show(importe);
                            fecha_vac = 1;
                        }
                        
                        if (fecha_vac == 0)
                        {
                        	if(tipo_carga == 1){
                           		consultamysql.Rows.Add(reg_pat, credito, per2, importe);
                        	}else{
                        		consultamysql.Rows.Add(reg_pat, credito, per2, importe,retiro_p,rcv_o,rcv_p);
                        	}
                        }
                        else
                        {
                        	if(tipo_carga == 1){
                        		tablarow2.Rows.Add(reg_pat, credito, per2, importe);
                        	}else{
                            	tablarow2.Rows.Add(reg_pat, credito, per2, importe,retiro_p,rcv_o,rcv_p);
                        	}
                            w++;
                        }

                    }
                    if (fecha_vac == 0)
                    {
                    	if(tipo_carga == 1){//Carga COP
                    		wr.WriteLine(fecha);//ajuste manual
	                        wr.WriteLine(reg_pat);
	                        wr.WriteLine(credito);
	                        wr.WriteLine(per2.Substring(4, 2));
	                        wr.WriteLine(per2.Substring(2, 2));
	                        wr.WriteLine(importe);
	                        
                    	}else{//Carga RCV
                    		wr.WriteLine(fecha);//ajuste manual
                    		wr.WriteLine(reg_pat);
	                        wr.WriteLine(credito);
	                        wr.WriteLine(per2.Substring(4, 2));
	                        wr.WriteLine(per2.Substring(2, 2));
	                        wr.WriteLine("#");
	                        wr.WriteLine(importe);
	                        wr.WriteLine(retiro_p);
	                        wr.WriteLine(rcv_o);
	                        wr.WriteLine(rcv_p);
	                        
                    	}
                        //wr.WriteLine((fecha.Substring(0,2)+fecha.Substring(3,2)+fecha.Substring(8,2)));
                        wr.WriteLine("$");
                    }
                    //dataGridView1.Rows[i].Visible=false;
                }

                wr.WriteLine("%&");

                //MessageBox.Show("Archivo Creado con: "+i+" Registros");

                //MessageBox.Show("El Archivo:\n"+fichero.FileName.ToString()+"\nSe ha creado correctamente","¡Exito!");
                //n=0;
                //close file
                wr.Close();

                StreamWriter wr1 = new StreamWriter(@"capturator_am/temp_aux.txt");
                wr1.WriteLine("0");
                wr1.WriteLine(consultamysql.Rows.Count.ToString());

                wr1.Close();
                dataGridView1.Refresh();
                //maskedTextBox2.Enabled=true;
                comboBox3.Enabled=true;
            }
            catch (Exception ex)
            {
                //en caso de haber una excepcion que nos mande un mensaje de error
                MessageBox.Show("No se pudo generar la lista\n\nError:\n" + ex, "Error al Generar Lista");
                filtr = 1;
            }
        }
		
		public void preparar(){
			filtro();
            if ((tot_rows - w) == 0)
            {
                filtr = 1;
            }

            if (n == 1)
            {
                
                    MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron: " + (tot_rows - w) + " registros\nSe omitieron: " + w + " registros\n\n" +
                                 "" + gen_list + " registros por no contar con una fecha de notificación válida\n" +
                                 "" + x + " registros por no contar con el crédito elegido válido\n" +
                                 "" + capturado_ant + " registros que ya estaban ingresados\n\n" +
                                 "Ahora puede iniciar la captura automática", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = tablarow;
            }
            else
            {
                if (filtr == 0)
                {
                MessageBox.Show("Se ha guardado la lista.\n\nSe guardaron: " + (tot_rows - w) + " registros\nSe omitieron: " + w + " registros\n\n" +
                         /*   "" + y + " registros por no contar con un registro patronal válido\n" +         
                            "" + gen_list + " registros por no contar con una fecha de notificación válida\n" +
                             "" + z + " registros por no contar con un  periodo válido\n" +
                             "" + x + " registros por no contar con un crédito válido\n\n" +*/
                             "Ahora puede iniciar la captura automática", "Lista Generada", MessageBoxButtons.OK, MessageBoxIcon.Information);               
                }
                else
                {
                	MessageBox.Show("No se guardo ningún registro, revisa el archivo.","No se Generó la lista");
                	filtr = 0;
                }
                    dataGridView1.DataSource = tablarow2;
            }

            if (filtr == 0)
            {
                button4.Enabled = true;
                label5.Text = "Registros: " + dataGridView1.RowCount;
                button10.Enabled = false;
                //inci=maskedTextBox2.Text;
                //maskedTextBox2.Enabled=false; 
                comboBox3.Enabled = false;
            }
            else
            {
                label5.Text = "Registros: " + dataGridView1.RowCount;
                filtr = 0;
            }
		}
		
		public void preparar_rcv(){
			rcv_cop = new Carga();
			rcv_cop.ShowDialog(this);
			tipo_carga = rcv_cop.mandar();
			//MessageBox.Show(tipo_carga.ToString());
			if(tipo_carga<1){
				MessageBox.Show("Tiene que seleccionar el modo de captura de acuerdo a la sección en la que se va a efectuar la captura automática en SISCOB");
			}else{
				preparar();
			}
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
		
		public void finalizar()
        {
        	//tipo_carga = n;
        	//MessageBox.Show("tipo_car:" + tipo_car + "  tipo_cred" + tipo_cred);
        	//Proceso final de captura en siscob

        	try
        	{ 
        		StreamReader rdr = new StreamReader(@"capturator_am/errores_siscob.txt");
        		StreamReader rdr1 = new StreamReader(@"capturator_am/temp_aux.txt");
        		
        		
        		Invoke(new MethodInvoker(delegate
        		                         {
        		                         	
        		                         	errores_sis = new string[consultamysql.Rows.Count];
        		                         	aciertos_sis = new string[2];
        		                         	i=0;
        		                         	m=0;
        		                         	dot_pos=0;
        		                         	
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
        		//MessageBox.Show("num acierto: "+acierto_act+", Linea: "+acierto_reg_cre+", tot_lins: "+aciertos_sis.Length+", primera linea: "+aciertos_sis[0]);
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
									
        							conex.guardar_evento("Se Capturaron en Siscob "+acierto_act+" Registros de Alta de Ajuste Manual "+comboBox3.SelectedItem.ToString()+" de "+tipo_ev+" de un total de "+tot_rows1);
        			   //MessageBox.Show(" Registros Actualizados.\n Ya puede manipular el equipo \n Ha finalizado todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
        		}
        		else
        		{  //finalizar cuando es archivo excel y access
        			
        			
	        			consultamysql.Columns.Add("CLAVE AJUSTE");
	        			consultamysql.Columns.Add("CAPTURADO SISCOB");
        			

        			Invoke(new MethodInvoker(delegate
        			                         {
        			                         	//MainForm mani = (MainForm)this.MdiParent;
        			                         	dataGridView3.DataSource = consultamysql;
        			                         	tot_rows = dataGridView3.RowCount;
												
        			                         	if(tipo_carga == 1){
	        			                         	do
	        			                         	{
	        			                         		dataGridView3.Rows[i].Cells[4].Value = comboBox3.SelectedItem.ToString();
	        			                         		dataGridView3.Rows[i].Cells[5].Value = "0";
	        			                         		i++;
	        			                         	} while (i < tot_rows);
        			                         	}else{
        			                         		do
	        			                         	{
	        			                         		dataGridView3.Rows[i].Cells[7].Value = comboBox3.SelectedItem.ToString();
	        			                         		dataGridView3.Rows[i].Cells[8].Value = "0";
	        			                         		i++;
	        			                         	} while (i < tot_rows);
        			                         	}
        			                         	
        			                         	
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
        			                         					if(tipo_carga == 1){
        			                         						dataGridView3.Rows[i].Cells[5].Value = "ERROR";
        			                         					}else{
        			                         						dataGridView3.Rows[i].Cells[8].Value = "ERROR";
        			                         					}
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
        			                         				if(tipo_carga == 1){
        			                         					dataGridView3.Rows[i].Cells[5].Value = "CAPTURADO";
        			                         				}else{
        			                         					dataGridView3.Rows[i].Cells[8].Value = "CAPTURADO";
        			                         				}
        			                         			}
        			                         		}
        			                         		else
        			                         		{
        			                         			if((i+1) > acierto_act){
        			                         				candado = 1;
        			                         				cont_omis++;
        			                         			}
        			                         			
        			                         			if(candado==0){
        			                         				
        			                         			   if(tipo_carga == 1){
        			                         					dataGridView3.Rows[i].Cells[5].Value = "CAPTURADO";
        			                         				}else{
        			                         					dataGridView3.Rows[i].Cells[8].Value = "CAPTURADO";
        			                         				}
        			                         			//toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + i + "   Erroneos por asignar: " + (tot_errs - k) + "  lineas leidas: " + j + "    buscando: " + errores_sis[j];
        			                         			//mani.toolStripStatusLabel1.Text = "Procesando Resultados, registros leidos: " + (i+1) + "   Erroneos por asignar: " + (tot_errs - k);
        			                         			//mani.statusStrip1.Refresh();
        			                         			}else{
        			                         				 if(tipo_carga == 1){
        			                         					dataGridView3.Rows[i].Cells[5].Value = "0";
        			                         				}else{
        			                         					dataGridView3.Rows[i].Cells[8].Value = "0";
        			                         				}
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
												//combinar tablas/grids
                                               
												while (i < tot_rows){
                                                    reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
        			                         		credito = dataGridView1.Rows[i].Cells[1].Value.ToString();
        			                         		per2 = dataGridView1.Rows[i].Cells[2].Value.ToString();
        			                         		importe = dataGridView1.Rows[i].Cells[3].Value.ToString();
        			                         		
													if(tipo_carga == 2){
        			                         			retiro_p = dataGridView1.Rows[i].Cells[4].Value.ToString();
        			                         			rcv_o = dataGridView1.Rows[i].Cells[5].Value.ToString();
        			                         			rcv_p = dataGridView1.Rows[i].Cells[6].Value.ToString();
        			                         			
        			                         			consultamysql.Rows.Add(reg_pat,credito,per2,importe,retiro_p,rcv_o,rcv_p,"-","0");
        			                         		}else{
                                                    	consultamysql.Rows.Add(reg_pat,credito,per2,importe,"-","0");
        			                         		}
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
        			                         			
        			                         			
                		
        			                         			if(tipo_carga == 1){
        			                         				wr2.WriteLine("REGISTRO PATRONAL,CREDITO,PERIODO,IMPORTE,CLAVE AJUSTE,CAPTURA SISCOB");
        			                         			}else{
        			                         				wr2.WriteLine("REGISTRO PATRONAL,CREDITO,PERIODO,IMPORTE TOTAL,IMPORTE RETIRO PATRONAL,RCV OBRERO,RCV PATRONAL,CLAVE AJUSTE,CAPTURA SISCOB");
        			                         			}
        			                         			do
        			                         			{
        			                         				reg_pat = dataGridView3.Rows[i].Cells[0].Value.ToString();
        			                         				credito = dataGridView3.Rows[i].Cells[1].Value.ToString();
        			                         				per2 = dataGridView3.Rows[i].Cells[2].Value.ToString();
        			                         				importe = dataGridView3.Rows[i].Cells[3].Value.ToString();
        			                         				
        			                         				
        			                         				if(tipo_carga == 2){
        			                         					retiro_p = dataGridView3.Rows[i].Cells[4].Value.ToString();
        			                         					rcv_o = dataGridView3.Rows[i].Cells[5].Value.ToString();
        			                         					rcv_p = dataGridView3.Rows[i].Cells[6].Value.ToString();
        			                         					fecha = dataGridView3.Rows[i].Cells[7].Value.ToString();
        			                         					f1 = dataGridView3.Rows[i].Cells[8].Value.ToString();
        			                         			
        			                         					wr2.WriteLine(reg_pat+","+credito+","+per2+","+importe+","+retiro_p+","+rcv_o+","+ rcv_p+","+fecha+","+f1);
        			                         				}else{
        			                         					fecha = dataGridView3.Rows[i].Cells[4].Value.ToString();
        			                         					f1 = dataGridView3.Rows[i].Cells[5].Value.ToString();
        			                         					
                                                    			wr2.WriteLine(reg_pat+","+credito+","+per2+","+importe+","+fecha+","+f1);
        			                         				}
        			                         				
        			                         				
        			                         				i++;
        			                         			} while (i < tot_rows);
        			                         			wr2.Close();
        			                         		
        			                         		//MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
        			                         		if(mochado==0){
        			                         			MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nHa terminado correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			                         		}else{
        			                         			MessageBox.Show("El archivo de resultados se generó correctamente.\nRuta: "+nom_save+"\nSe guardo el resultado de: "+acierto_act+" registros de un total de: "+tot_rows+".  \nHa terminado de forma parcial correctamente todo el proceso de captura.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        			                         		}
        			                         		conex.guardar_evento("Se Capturaron en Siscob "+acierto_act+" Registros de Alta de Ajuste Manual "+comboBox3.SelectedItem.ToString()+" de "+tipo_ev+" de un total de "+tot_rows1);
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
        	                         	//mani1.statusStrip1.Refresh();
        	                         	toolStripStatusLabel1.Text = "Listo";
        	                         	statusStrip1.Refresh();
        	                         	button18.Visible=false;
        	                         	button4.Visible=true;
        	                         	button10.Visible=true;
        	                         	button3.Enabled=true;
        	                         	
        	                         }));

        }
		
		void AutocobLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            panel1.Visible=false;
			panel2.Visible=true;
			label10.Visible=false;
			radioButton1.Visible=false;
			radioButton2.Visible=false;
			
			tablarow.Columns.Add("ID");
			tablarow.Columns.Add("REGISTRO PATRONAL");
			tablarow.Columns.Add("CAPTURADO SISCOB");
			tablarow.Columns.Add("CRÉDITO CUOTA");	
			tablarow.Columns.Add("CRÉDITO MULTA");
			tablarow.Columns.Add("PERIODO");
			tablarow.Columns.Add("IMPORTE");
			tablarow.Columns.Add("NOMBRE PERIODO");		
			
			tablarow2.Columns.Add("REGISTRO PATRONAL");
            tablarow2.Columns.Add("CRÉDITO");
            tablarow2.Columns.Add("PERIODO");
            tablarow2.Columns.Add("IMPORTE TOTAL");
			
			toolTip1.SetToolTip(label9, "0 - No Capturado\n1 - Capturado\n2 - Error al Capturar");
			toolTip1.SetToolTip(button2, "Consultar\nen la BD");
		}
		
		void Button19Click(object sender, EventArgs e)
		{
			Login inic = new Login();
			inic.respawn();
			//conexion.Close();
			this.Dispose();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			carga_excel();
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
            comboBox3.SelectedIndex = -1;
			cargar_hoja_excel();
			//maskedTextBox2.Enabled=true;
			comboBox3.Enabled=true;
			}
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			//tipo_carga = 1;
			//preparar();
			preparar_rcv();
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
					System.IO.File.Delete(@"capturator_am/errores_siscob.txt");
	        	    System.IO.File.Delete(@"capturator_am/acierto_siscob.txt");
	        	    System.IO.File.Delete(@"capturator_am/aciertos_siscob.txt");
	        	    //Crear archivos nuevos
	                fichero = System.IO.File.Create(@"capturator_am/errores_siscob.txt");
                   
					fichero1= System.IO.File.Create(@"capturator_am/acierto_siscob.txt");
					fichero2= System.IO.File.Create(@"capturator_am/aciertos_siscob.txt");
	                ruta = fichero.Name;
	                fichero.Close();
	                fichero1.Close();
	                fichero2.Close();
	                
	                StreamWriter wr1 = new StreamWriter(@"capture_am.bat");
	                wr1.WriteLine("@echo off");
	                wr1.WriteLine("C:");
	                wr1.WriteLine("cd "+ruta.Substring(0,(ruta.Length-19)));
	                
	                if(tipo_carga==1){
	                wr1.WriteLine("start bumblecapt.exe");
	                tipo_ev="COP";
	                
	                }else{
	                wr1.WriteLine("start bumblecapt_rcv.exe");
               		tipo_ev="RCV";
	                }
	                wr1.Close();
	               /* MainForm mani = (MainForm)this.MdiParent;
					mani.capt_siscob(consultamysql,tipo_credito,n);
					n=0;*/
	               button18.Visible = true;
	               button3.Enabled = false;
        	       button4.Visible=false;
        	       button10.Visible=false;
				   System.Diagnostics.Process.Start(@"capture_am.bat");
				   conex.guardar_evento("Se Mando a capturar en Siscob "+consultamysql.Rows.Count.ToString()+" Registros de Alta de Ajuste Manual "+comboBox3.SelectedItem.ToString()+" de "+tipo_ev);
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
		
		void Button18Click(object sender, EventArgs e)
		{
			mochado=1;
			finalizar();
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedIndex >= 0){
				button10.Enabled=true;
			}else{
				button10.Enabled=false;
			}
		}
		
		void FinToolStripMenuItem1Click(object sender, EventArgs e)
		{
			finalizar();
		}
		
		void Panel1Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			
		}
	}
}

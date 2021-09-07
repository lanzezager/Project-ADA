/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 14/04/2016
 * Hora: 02:13 p. m.
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
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;
using Microsoft.SqlServer.Types;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Busca_nn.
	/// 
	/// </summary>
	public partial class Busca_nn : Form
	{
		public Busca_nn()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//

		}
		
		String sql,nrp,nrp_limpio,nombre_tabla_ema,reg_pat,cad_con,hoja,cons_exc;
        int i=0,j=0,k=0,no_acu=0,actualizar=0,sector_nuevo=0,tipo_save=0,acts_archs=0;

        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        DataTable tabla_grid= new DataTable();
        DataTable data_acumulador= new DataTable();
        DataTable consultamysql = new DataTable();
        
        //Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
        
        DataGridViewTextBoxColumn columna_s_n = new DataGridViewTextBoxColumn();
       	Font fuente = new Font("Arial Unicode MS", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        
       	SaveFileDialog fichero = new SaveFileDialog();
       	
       	//periodos
        public void llenar_Cb1()
        {
        	int coun=0;
            conex2.conectar("base_principal");
            //conex.conectar("base_principal");
            comboBox1.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex2.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE status=\"0\" or status=\"EN TRAMITE\" ORDER BY nombre_periodo;");
            do{
            
            //	dataGridView3.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+dataGridView2.Rows[i].Cells[0].Value.ToString()+"\" and (status=\"0\" or status=\"EN TRAMITE\") ");
            //	coun=Convert.ToInt32(dataGridView3.Rows[0].Cells[0].FormattedValue);
            //	if(coun>0){
            		comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
            //	}
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            label5.Text="Periodos Listados: "+comboBox1.Items.Count;
            conex2.cerrar();
            //conex.cerrar();
        }
		//notificadores
		public void llenar_Cb2(){
			conex2.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			dataGridView2.DataSource = conex2.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
			do{
				comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString()+" "+dataGridView2.Rows[i].Cells[1].Value.ToString());
				i++;
			}while(i<dataGridView2.RowCount);
			i=0;
			//comboBox2.Items.Add("--NINGUNO--");
            conex2.cerrar();
		}
		//sectores
        public void llenar_Cb4()
        {
            conex2.conectar("base_principal");
            comboBox4.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex2.consultar("SELECT sector FROM base_principal.sectores;");
            do
            {
                comboBox4.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i <dataGridView2.RowCount);
            i = 0;
            //MessageBox.Show(dataGridView2.Rows[0].Cells[0].Value.ToString());
            conex2.cerrar();
        }
		//ema
        public void llenar_Cb3()
        {
            conex2.conectar("ema");
            comboBox3.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex2.consultar("SHOW TABLES FROM ema ");
            do
            {
                comboBox3.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i <dataGridView2.RowCount);
            i = 0;
            //MessageBox.Show(dataGridView2.Rows[0].Cells[0].Value.ToString());
            conex2.cerrar();
        }      
        //periodos especiales
		public void llenar_Cb1_esp(){
			int coun=0;
            conex2.conectar("base_principal");
            //conex.conectar("base_principal");
            comboBox1.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex2.consultar("SELECT DISTINCT periodo_factura FROM base_principal.datos_factura WHERE (status=\"0\" or status=\"EN TRAMITE\") and periodo_factura <> \"-\" ORDER BY periodo_factura;");
            do{
            
            //	dataGridView3.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+dataGridView2.Rows[i].Cells[0].Value.ToString()+"\" and (status=\"0\" or status=\"EN TRAMITE\") ");
            //	coun=Convert.ToInt32(dataGridView3.Rows[0].Cells[0].FormattedValue);
            //	if(coun>0){
            		comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
            //	}
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            label5.Text="Periodos Listados: "+comboBox1.Items.Count;
            label5.Refresh();
            conex2.cerrar();
            //conex.cerrar();
		}
        
		public void columna_Sector_Nuevo(){

			if(dataGridView1.Columns.Count==0){
        	columna_s_n.Name="sector_nuevo";
			columna_s_n.HeaderText="SECTOR\nNUEVO";
			columna_s_n.Frozen=true;
			columna_s_n.DefaultCellStyle.BackColor = System.Drawing.Color.Navy;
			columna_s_n.DefaultCellStyle.Font = fuente;
			columna_s_n.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
			columna_s_n.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			//columna_s_n.SortMode= DataGridViewColumnSortMode.;  
			
			dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {this.columna_s_n});
			}
			
			
			dataGridView1.DataSource=consultamysql;
			
			
			dataGridView1.Columns[1].HeaderText="SECTOR\nACTUAL";
			dataGridView1.Columns[2].HeaderText="REGISTRO\nPATRONAL";
			dataGridView1.Columns[3].HeaderText="RAZÓN\nSOCIAL";
			dataGridView1.Columns[4].HeaderText="DOMICILIO";
			dataGridView1.Columns[5].HeaderText="LOCALIDAD";
			dataGridView1.Columns[6].HeaderText="NOTIFICADOR";
			
			dataGridView1.Columns[3].Width=200;
			dataGridView1.Columns[4].Width=200;
			dataGridView1.Columns[5].Width=200;
			dataGridView1.Columns[6].Width=200;
			
			dataGridView1.Columns[1].ReadOnly=true;
			dataGridView1.Columns[2].ReadOnly=true;
			dataGridView1.Columns[3].ReadOnly=true;
			dataGridView1.Columns[4].ReadOnly=true;
			dataGridView1.Columns[5].ReadOnly=true;
			dataGridView1.Columns[6].ReadOnly=true;
			
			dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
			
			dataGridView1.Focus();

		}
		
        public void consulta_patron(){
        	conex.conectar("base_principal");
        	i=0;
        	dataGridView3.Columns.Clear();
        	if(comboBox6.SelectedIndex==0){
	        	sql="SELECT registro_patronal,razon_social,sector_notificacion_actualizado,notificador "+
	        		"FROM datos_factura WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" "+
	        		"AND registro_patronal1 LIKE \"%"+nrp_limpio+"%\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal";
        	}else{
        		sql="SELECT registro_patronal,razon_social,sector_notificacion_actualizado,notificador "+
	        		"FROM datos_factura WHERE periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" "+
	        		"AND registro_patronal1 LIKE \"%"+nrp_limpio+"%\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal";
        	}
        	dataGridView3.DataSource=conex.consultar(sql);
        	
        	if(dataGridView3.RowCount>0){
        		if(radioButton1.Checked==true){
        			consulta_periodo_sindo();
        		}else{
        			if(radioButton2.Checked==true){
        				consulta_periodo_ema();
        			}
        		}
        	}else{
        		MessageBox.Show("Patrón Inexistente en el Periodo Seleccionado.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		button1.Enabled=false;
        	}
        	
        }
        
        public void consulta_multiple_patrones(){
        	conex.conectar("base_principal");
        	i=0;
        	dataGridView3.Columns.Clear();
        	consultamysql.Rows.Clear();
        	if(comboBox6.SelectedIndex==0){
	        	sql="SELECT registro_patronal,razon_social,sector_notificacion_actualizado,notificador "+
	        		"FROM datos_factura WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" "+
	        		"AND notificador = \""+comboBox2.SelectedItem.ToString()+"\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal";
        	}else{
        		sql="SELECT registro_patronal,razon_social,sector_notificacion_actualizado,notificador "+
	        		"FROM datos_factura WHERE periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" "+
	        		"AND notificador = \""+comboBox2.SelectedItem.ToString()+"\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal";
        	}
        	
        	dataGridView3.DataSource=conex.consultar(sql);
        	
        	if(dataGridView3.RowCount>0){
        	if(radioButton1.Checked==true){
        			consulta_periodo_sindo();
        		}else{
        			if(radioButton2.Checked==true){
        				consulta_periodo_ema();
        			}
        		}
        	}else{
        		MessageBox.Show("Todos los Patrones del filtro elegido ya han sido sectorizados.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		button1.Enabled=false;
        	}
        	
        }
        
        public void consulta_periodo_ema(){
        	
        	
        	if(dataGridView3.RowCount>0){
        	conex2.conectar("ema");
			
        	if(dataGridView3.Columns.Count==4){
        	dataGridView3.Columns.Add("domicilio", "domicilio");
        	dataGridView3.Columns.Add("localidad", "localidad");
        	}
        	i=0;
        	j=comboBox3.Items.Count-1;
        	
        		do{
        			nombre_tabla_ema=comboBox3.Items[j].ToString();
        			do
        			{
        				//if((dataGridView3.Rows[i].Cells[4].Value == null)){
        					
        					reg_pat = dataGridView3.Rows[i].Cells[0].FormattedValue.ToString();
        					reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
        					dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad FROM " + nombre_tabla_ema + " WHERE reg_pat1 =\"" + reg_pat + "\"");
        					if (dataGridView2.RowCount > 0)
        					{
        						dataGridView3.Rows[i].Cells[4].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
        						dataGridView3.Rows[i].Cells[5].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();
        					}
        					else
        					{
        						if(j==0){
        							dataGridView3.Rows[i].Cells[4].Value = "-";
        							dataGridView3.Rows[i].Cells[5].Value = "-";
        							
        						}
        					}
        				//}
        				
        				i++;
        			} while (i < dataGridView3.RowCount);
        			
        			i=0;
        			
        			j--;
        		}while(j>=0);
        	
        	i=0;
        	do{
        		consultamysql.Rows.Add(dataGridView3.Rows[i].Cells[2].FormattedValue.ToString(),//sector_not
        		                       dataGridView3.Rows[i].Cells[0].FormattedValue.ToString(),//reg_pat
        		                       dataGridView3.Rows[i].Cells[1].FormattedValue.ToString(),//razon_soc
        		                       dataGridView3.Rows[i].Cells[4].FormattedValue.ToString(),//domicilio
        		                       dataGridView3.Rows[i].Cells[5].FormattedValue.ToString(),//localidad
        		                       dataGridView3.Rows[i].Cells[3].FormattedValue.ToString());//notificador
        		
        		i++;
        	}while(i<dataGridView3.RowCount);
        	columna_Sector_Nuevo();
        	button1.Enabled=true;
        	
        	}else{
        		MessageBox.Show("Todos los Patrones del filtro elegido ya han sido sectorizados.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		button1.Enabled=false;
        	}
        }
        
        public void consulta_periodo_sindo(){
        	
        	if(dataGridView3.RowCount>0){
        		conex2.conectar("base_principal");
        		
        		if(dataGridView3.Columns.Count==4){
        			dataGridView3.Columns.Add("domicilio", "domicilio");
        			dataGridView3.Columns.Add("localidad", "localidad");
        		}
        		i=0;
        		
        		do
        		{
        			//if((dataGridView3.Rows[i].Cells[4].Value == null)){
        			
        			reg_pat = dataGridView3.Rows[i].Cells[0].FormattedValue.ToString();
        			reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
        			dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad,cp FROM sindo WHERE registro_patronal =\"" + reg_pat + "\"");
        			if (dataGridView2.RowCount > 0)
        			{
        				dataGridView3.Rows[i].Cells[4].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
        				dataGridView3.Rows[i].Cells[5].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString()+"  CP: "+dataGridView2.Rows[0].Cells[2].FormattedValue.ToString();
        			}
        			else
        			{
        				if(j==0){
        					dataGridView3.Rows[i].Cells[4].Value = "-";
        					dataGridView3.Rows[i].Cells[5].Value = "-";
        					
        				}
        			}
        			//}
        			
        			i++;
        		} while (i < dataGridView3.RowCount);
        		
        		i=0;
        		
        		
        		i=0;
        		do{
        			consultamysql.Rows.Add(dataGridView3.Rows[i].Cells[2].FormattedValue.ToString(),//sector_not
        			                       dataGridView3.Rows[i].Cells[0].FormattedValue.ToString(),//reg_pat
        			                       dataGridView3.Rows[i].Cells[1].FormattedValue.ToString(),//razon_soc
        			                       dataGridView3.Rows[i].Cells[4].FormattedValue.ToString(),//domicilio
        			                       dataGridView3.Rows[i].Cells[5].FormattedValue.ToString(),//localidad
        			                       dataGridView3.Rows[i].Cells[3].FormattedValue.ToString());//notificador
        			
        			i++;
        		}while(i<dataGridView3.RowCount);
        		columna_Sector_Nuevo();
        		button1.Enabled=true;
        		
        	}else{
        		MessageBox.Show("Todos los Patrones del filtro elegido ya han sido sectorizados.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		button1.Enabled=false;
        	}
        }
        
        public void checar_repetidos(){
        	i=0;
        	if(dataGridView1.RowCount>0){
        	do{
        			if((dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(0,9)).Equals(nrp_limpio.ToUpper().Substring(0,3)+"-"+nrp_limpio.Substring(3,5))){
        				MessageBox.Show("El Patrón que busca ya se encuentra en la lista.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		}else{
        				consulta_patron();
        			}
        		i++;
        	}while(i<dataGridView1.RowCount);
        	}else{
        		consulta_patron();
        	}
        } 
        
        public void carga_chema_excel(){
			i=0;
			int filas = 0;
			String tabla;
			
			comboBox5.Items.Clear();
		    System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
		    dataGridView2.DataSource = dt;
		    filas=dataGridView2.RowCount;
					do{
						if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
							if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
								tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
								if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
									tabla = tabla.Remove((tabla.Length-1),1);
									comboBox5.Items.Add(tabla);
								}
							}
						}
						i++;
					}while(i<filas);
					
                    dt.Clear();
                    dataGridView2.DataSource = dt; //vaciar datagrid
                    i=0;
		}
        
        public void cargar_hoja_excel_procesar(){
			
				hoja = comboBox5.SelectedItem.ToString();
				
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select * From [" + hoja + "$]";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
			
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						if (dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                            
						//dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						data_acumulador.Merge(dataSet.Tables[0]);
						//conexion.Close();//cerramos la conexion
						//dataGridView2.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						 
					}
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja.\n\n"+ex,"Error al Abrir Archivo de Excel");
				}
				
			}
			
		}
        
        public void guardar_normal(){
        	i=0;
        	actualizar=0;
        	do{
        		if(dataGridView1.Rows[i].Cells[0].Value != null){
        			actualizar++;
        		}
        		i++;
        	}while(i<dataGridView1.RowCount);
        	
        	DialogResult resuls= MessageBox.Show("Se va a actualizar el sector de notificación de: "+actualizar+" Patrones.\nEsto afectará la Base de Datos.\n\n¿Está seguro de querer Continuar?"
        	                                     ,"AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
        	
        	if(resuls == DialogResult.Yes){
        		comboBox1.Enabled=false;
        		conex2.conectar("base_principal");
        		i=0;
        		do{
        			if(dataGridView1.Rows[i].Cells[0].Value != null){
        				if(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Equals("70")){
        					if(comboBox6.SelectedIndex==0){
	        					sql="UPDATE datos_factura SET sector_notificacion_actualizado = sector_notificacion_inicial WHERE "+
	        						" nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" AND registro_patronal=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\";";
        					}else{
        						sql="UPDATE datos_factura SET sector_notificacion_actualizado = sector_notificacion_inicial WHERE "+
	        						"periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" AND registro_patronal=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\";";
        					}
        					conex2.consultar(sql);

        				}else{
        					if(comboBox6.SelectedIndex==0){
	        					sql="UPDATE datos_factura SET sector_notificacion_actualizado="+dataGridView1.Rows[i].Cells[0].Value.ToString()+" WHERE "+
	        						"nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" AND registro_patronal=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\";";
        					}else{
        						sql="UPDATE datos_factura SET sector_notificacion_actualizado="+dataGridView1.Rows[i].Cells[0].Value.ToString()+" WHERE "+
	        						"periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" AND registro_patronal=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\";";
        					}
        					
        					conex2.consultar(sql);
        					//MessageBox.Show(sql);
        				}
        			}
        			i++;
        		}while(i<dataGridView1.RowCount);
        		conex2.cerrar();
        		
        		i=dataGridView1.RowCount-1;
        		do{
        			if(dataGridView1.Rows[i].Cells[0].Value != null){
        				dataGridView1.Rows.RemoveAt(i);
        			}
        			i--;
        		}while(i>=0);
        		
        		if(actualizar>0){
        			Asigna_noti asinot = new Asigna_noti(comboBox1.SelectedItem.ToString(),comboBox6.SelectedIndex);
        			asinot.Show();
        		}
        		
        		label5.Text="Patrones Listados: "+dataGridView1.RowCount;
        		label5.Refresh();
        		conex2.guardar_evento("Se Sectorizaron "+actualizar+" registros del Periodo: "+comboBox1.SelectedItem.ToString());
        		MessageBox.Show("Se actualizaron correctamente "+actualizar+" Sectores de Notificación","¡Exito!");
        		
        		comboBox1.Enabled=true;
        	}else{
        		MessageBox.Show("No se hará Nada","AVISO");
        	}
        	
        }
        
        public void guardar_con_archivo(){
        	
        	if(dataGridView4.RowCount>0){
        		DialogResult res = MessageBox.Show("Se intentará actualizar el sector de "+data_acumulador.Rows.Count+" patrones.\nEsto afectará la Base de Datos.\n\n¿Desea Continuar con la actualización?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
        		
        		if(res == DialogResult.Yes){
        			try{
        				i=0;
        				acts_archs=0;
        				do{
        					if(dataGridView4.Rows[i].Cells[0].Value.ToString().Length>0){
        						if(comboBox6.SelectedIndex==0){
        							sql="UPDATE datos_factura SET sector_notificacion_actualizado ="+dataGridView4.Rows[i].Cells[0].Value.ToString()+" WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" AND registro_patronal2=\""+dataGridView4.Rows[i].Cells[1].Value.ToString().Substring(0,10)+"\";";
        						}else{
        							sql="UPDATE datos_factura SET sector_notificacion_actualizado ="+dataGridView4.Rows[i].Cells[0].Value.ToString()+" WHERE periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" AND registro_patronal2=\""+dataGridView4.Rows[i].Cells[1].Value.ToString().Substring(0,10)+"\";";	
        						}
        					conex.consultar(sql);	
        					acts_archs++;
        					//MessageBox.Show(sql);
        					}
        					i++;
        				}while(i<dataGridView4.RowCount);
        				Asigna_noti asinot = new Asigna_noti(comboBox1.SelectedItem.ToString(),comboBox6.SelectedIndex);
        				asinot.Show();
        				MessageBox.Show("Se actualizaron con éxito "+acts_archs+" registros en la Base de Datos.","EXITO");
        				conex2.guardar_evento("Se Sectorizaron "+acts_archs+" registros del Periodo: "+comboBox1.SelectedItem.ToString());
        			}catch(Exception ed){
        				MessageBox.Show("Ocurrió un error al actualizar los sectores en la Base de Datos.\n\n"+ed,"ERROR");
        			}
        			dataGridView4.Visible=false;
        		}
        		conexion.Close();
        		button1.Enabled=false;
        	}
        	
        	tipo_save=0;
        	
        }
        
        public void guardar_vigentes(){
        	int conti=0;
        	
        	do{
        		sql="UPDATE datos_factura SET status =\"NO VIGENTE\", fecha_recepcion=\"2016-07-15\", observaciones=\"Credito NO vigente en RALE al dia 15/07/2016\" WHERE id="+dataGridView4.Rows[conti].Cells[0].FormattedValue.ToString()+";";
        		conex.consultar(sql);
        		conti++;
        	}while(conti< dataGridView4.RowCount);
        	MessageBox.Show("Se modificaron "+conti+" registros");
        }
        
        //LOAD----------
		void Busca_nnLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llenar_Cb1();
			llenar_Cb2();
			llenar_Cb3();
			llenar_Cb4();
			
			consultamysql.Columns.Add();
			consultamysql.Columns.Add();
			consultamysql.Columns.Add();
			consultamysql.Columns.Add();
			consultamysql.Columns.Add();
			consultamysql.Columns.Add();
			
			comboBox6.SelectedIndex=0;
		}
        
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox2.Enabled=true;
			maskedTextBox1.Enabled=true;
			button4.Enabled=true;	
			button5.Enabled=true;	
			//button6.Enabled=true;	
			
		}
		//Guardar----
		void Button1Click(object sender, EventArgs e)
		{	
			if(tipo_save==2){
				guardar_con_archivo();
			}else{
				guardar_normal();
			}	
			
			//guardar_vigentes();
		}
		//acumulable
		void Button2Click(object sender, EventArgs e)//consulta patron
		{
			//MessageBox.Show(no_acu.ToString());
			if(no_acu==1){
				consultamysql.Rows.Clear();
				dataGridView1.DataSource=consultamysql;
				//dataGridView1.Rows.Clear();
				no_acu=0;
			}
			
			checar_repetidos();
			label5.Text="Patrones Listados: "+dataGridView1.RowCount;
			label5.Refresh();
		}
		//no acumulable
		void Button3Click(object sender, EventArgs e)//consulta multiple
		{
			consulta_multiple_patrones();
			no_acu=1;
			label5.Text="Patrones Listados: "+dataGridView1.RowCount;
			label5.Refresh();
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			conex.conectar("base_principal");
        	i=0;
        	dataGridView3.Columns.Clear();
        	consultamysql.Rows.Clear();
        	if(comboBox6.SelectedIndex==0){
	        	sql="SELECT registro_patronal,razon_social,sector_notificacion_actualizado,notificador "+
	        		"FROM datos_factura WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" "+
	        		"AND status=\"0\" ORDER BY sector_notificacion_actualizado,registro_patronal";
        	}else{
        		sql="SELECT registro_patronal,razon_social,sector_notificacion_actualizado,notificador "+
	        		"FROM datos_factura WHERE periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" "+
	        		"AND status=\"0\" ORDER BY sector_notificacion_actualizado,registro_patronal";
        	}
        	//MessageBox.Show(sql);
        	dataGridView3.DataSource=conex.consultar(sql);
        	
        	if(radioButton1.Checked==true){
        		consulta_periodo_sindo();
        	}else{
        		if(radioButton2.Checked==true){
        			consulta_periodo_ema();
        		}
        	}
        	
			no_acu=1;
			label5.Text="Patrones Listados: "+dataGridView1.RowCount;
			label5.Refresh();
			
		}
		
		void MaskedTextBox1MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
				
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			nrp=maskedTextBox1.Text.ToUpper();
			i=0;
			nrp_limpio="";
			//se quitan guiones y espacios en blanco 
			if(nrp.Length > 8){
				do{
					if(((nrp.Substring(i,1)).Equals(" "))||((nrp.Substring(i,1)).Equals("-"))){
					}else{
						nrp_limpio += nrp.Substring(i,1);
					}
					i++;
				  }while(i<nrp.Length);
			}
			
			if(nrp_limpio.Length>=8){
				button2.Enabled=true;
			}else{
				button2.Enabled=false;
			}
		}
		
		void MaskedTextBox1Enter(object sender, EventArgs e)
		{
			nrp=maskedTextBox1.Text.ToUpper();
			nrp_limpio="";
			i=0;
			//se quitan guiones y espacios en blanco 
			if(nrp.Length > 0){
				do{
					if(((nrp.Substring(i,1)).Equals(" "))||((nrp.Substring(i,1)).Equals("-"))){
					}else{
						nrp_limpio += nrp.Substring(i,1);
					}
					i++;
				  }while(i<nrp.Length);
			}
			
			if(nrp_limpio.Length==0){
				maskedTextBox1.Clear();
				maskedTextBox1.SelectionStart=0;
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			button3.Enabled=true;			
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				if(nrp_limpio.Length>=8){
					//MessageBox.Show(no_acu.ToString());
					if(no_acu==1){
						consultamysql.Rows.Clear();
						dataGridView1.DataSource=consultamysql;
						//dataGridView1.Rows.Clear();
						no_acu=0;
					}
					
					checar_repetidos();
					label5.Text="Patrones Listados: "+dataGridView1.RowCount;
					label5.Refresh();
				}
			}
		}
		
		void MaskedTextBox1Leave(object sender, EventArgs e)
		{
			maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
		}
		
		void MaskedTextBox1Click(object sender, EventArgs e)
		{
			nrp=maskedTextBox1.Text.ToUpper();
			nrp_limpio="";
			i=0;
			//se quitan guiones y espacios en blanco 
			if(nrp.Length > 0){
				do{
					if(((nrp.Substring(i,1)).Equals(" "))||((nrp.Substring(i,1)).Equals("-"))){
					}else{
						nrp_limpio += nrp.Substring(i,1);
					}
					i++;
				  }while(i<nrp.Length);
			}
			
			if(nrp_limpio.Length==0){
			maskedTextBox1.Clear();
			maskedTextBox1.SelectionStart=0;
			}
		}
		
		void DataGridView1CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			sector_nuevo=0;
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value != null){
			
			   	if(Int32.TryParse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString(),out sector_nuevo))
			{
				k=0;
				do{
					if(comboBox4.Items[k].ToString().Equals(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()))
					{
						sector_nuevo=1;
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Style.BackColor=System.Drawing.Color.PaleTurquoise;
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Style.BackColor=System.Drawing.Color.PaleTurquoise;
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Style.BackColor=System.Drawing.Color.PaleTurquoise;
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Style.BackColor=System.Drawing.Color.PaleTurquoise;
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Style.BackColor=System.Drawing.Color.PaleTurquoise;
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[6].Style.BackColor=System.Drawing.Color.PaleTurquoise;
						//MessageBox.Show(comboBox4.Items[k].ToString());
					}else{
						sector_nuevo=0;
					}
					k++;
				}while((k < comboBox4.Items.Count)&&(sector_nuevo==0));
			}
			
			if(sector_nuevo==0){
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = null;
				
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Style.BackColor=System.Drawing.Color.White;			
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[6].Style.BackColor=System.Drawing.Color.White;	
			}
			}else{
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Style.BackColor=System.Drawing.Color.White;			
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[5].Style.BackColor=System.Drawing.Color.White;	
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[6].Style.BackColor=System.Drawing.Color.White;	
			}
		 }
		//Cargar Excel
		void Button5Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
		
			dialog.Title = "Seleccione el archivo de Actualización de Sectores";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			dataGridView4.DataSource=null;
			dataGridView4.Columns.Clear();
			dataGridView4.Rows.Clear();
			conex.conectar("base_principal");
			data_acumulador.Clear();
			//dataGridView1.Rows.Clear();
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dialog.FileName + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion
				carga_chema_excel();
				j=0;
				do{
					comboBox5.SelectedIndex = j;
					cargar_hoja_excel_procesar();
					j++;
				}while(j<comboBox5.Items.Count);
				
				dataGridView4.DataSource=data_acumulador;
				dataGridView4.Refresh();
				label5.Text="Patrones Listados: "+dataGridView4.RowCount;
				label5.Refresh();
				button1.Enabled=true;
				dataGridView4.Visible=true;
				tipo_save=2;
				
			}
		}
		//Guardar Excel
		void Button6Click(object sender, EventArgs e)
		{
			fichero.Title = "Guardar archivo de Excel";
			fichero.Filter = "Archivo Excel (*.XLSX)|*.xlsx|Archivo Compatible con Excel (*.CSV)|*.csv";
			
			if(fichero.ShowDialog() == DialogResult.OK){
				try{
					dataGridView5.DataSource=null;
					dataGridView5.DataSource = dataGridView1.DataSource as DataTable;
					dataGridView1.ResetBindings();
					j=0;
					while(j<dataGridView5.RowCount){
						dataGridView5.Rows[j].Cells[1].Value = dataGridView5.Rows[j].Cells[1].Value.ToString().Substring(0,3)+
															   dataGridView5.Rows[j].Cells[1].Value.ToString().Substring(4,5)+
															   dataGridView5.Rows[j].Cells[1].Value.ToString().Substring(10,2);
						j++;
					}
					
					tabla_grid = dataGridView5.DataSource as DataTable;
					
					tabla_grid.Columns[0].ColumnName="SECTOR ACTUAL";
					tabla_grid.Columns[1].ColumnName="REGISTRO PATRONAL";
					tabla_grid.Columns[2].ColumnName="RAZON SOCIAL";
					tabla_grid.Columns[3].ColumnName="DOMICILIO";
					tabla_grid.Columns[4].ColumnName="LOCALIDAD";
					tabla_grid.Columns[5].ColumnName="NOTIFICADOR";
					XLWorkbook wb = new XLWorkbook();	
					wb.Worksheets.Add(tabla_grid,"hoja_lz");
					wb.SaveAs(fichero.FileName);
					MessageBox.Show("Se Ha creado correctamente el archivo:\n"+fichero.FileName,"Exito");
				}catch(Exception es){
					MessageBox.Show("Ha ocurrido un error al intentar crear el archivo:\n"+fichero.FileName+"\n\n"+es,"ERROR");
				}
			}
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				button6.Enabled=true;
			}else{
				button6.Enabled=false;
			}
		}
		
		void ComboBox1TextUpdate(object sender, EventArgs e)
		{
			if(comboBox1.Items.Contains(comboBox1.Text)==true){
				comboBox2.Enabled=true;
				maskedTextBox1.Enabled=true;
				button4.Enabled=true;	
				button5.Enabled=true;	
			}else{
				comboBox2.Enabled=false;
				maskedTextBox1.Enabled=false;
				button4.Enabled=false;	
				button5.Enabled=false;	
			}
		}
		
		void ComboBox2TextUpdate(object sender, EventArgs e)
		{
			if(comboBox2.Items.Contains(comboBox2.Text)==true){
				button3.Enabled=true;
			}else{
				button3.Enabled=false;
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox6SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox6.SelectedIndex==0){
				llenar_Cb1();
			}else{
				llenar_Cb1_esp();
			}
		}
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 10/04/2017
 * Hora: 05:45 p.m.
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

namespace Nova_Gear.Automatizacion
{
	/// <summary>
	/// Description of Sectorizador.
	/// </summary>
	public partial class Sectorizador : Form
	{
		public Sectorizador()
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
		DataTable tabla_results = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		DataTable tablarow = new DataTable();
		
		
		String archivo,ext,nom_save; 
		int i=0;
		
		public void llenar_Cb2_todos(){
			int i=0;
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
		
		public void consultar(){
			
			if(comboBox2.SelectedIndex>-1){
				conex.conectar("base_principal");
				dataGridView3.DataSource=conex.consultar("SELECT registro_patronal2, sector_notificacion_actualizado,sector_notificacion_inicial FROM datos_factura WHERE nombre_periodo=\""+comboBox2.SelectedItem.ToString()+"\" AND sector_notificacion_actualizado <> sector_notificacion_inicial");
				label5.Text="Registros: "+dataGridView3.RowCount;
				label5.Refresh();
				
				//estilo datagrid
				dataGridView3.Columns[0].HeaderText="REGISTRO PATRONAL";
				dataGridView3.Columns[1].HeaderText="SECTOR\nACTUALIZADO";
				dataGridView3.Columns[2].HeaderText="SECTOR\nINICIAL";
				
				if(dataGridView3.RowCount>0){
					button4.Enabled=true;
				}else{
					button4.Enabled=false;
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
						
						if(dataGridView3.RowCount>0){
							button4.Enabled=true;
						}else{
							button4.Enabled=false;
						}
						
						//estilo datagrid
                        i = 0;
                        do
                        {
                        	dataGridView3.Columns[i].HeaderText=dataGridView3.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView3.ColumnCount);
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
		
		public void filtro(){
			
			String rp,sec,ruta;
			int rp_num=0,sec_num=0,j=0;
			FileStream fichero;
			i=0;
			
			if(tablarow.Columns.Contains("REGISTRO PATRONAL")==false){
				tablarow.Columns.Add("REGISTRO PATRONAL");
			}
			//Borrar archivos para comenzar de 0
			System.IO.File.Delete(@"sectores/lista_nrp_sec.txt");
			System.IO.File.Delete(@"sectores/resultados_sectores.txt");
						
			//Crear archivos nuevos
			fichero = System.IO.File.Create(@"sectores/lista_nrp_sec.txt");
			
			ruta=fichero.Name;
			
			fichero.Close();
			
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"sectores/lista_nrp_sec.txt");
			wr.WriteLine(dataGridView3.RowCount.ToString().ToString());
			
			do{
				rp=dataGridView3.Rows[i].Cells[0].Value.ToString();
				sec=dataGridView3.Rows[i].Cells[1].Value.ToString();
				if(rp.Length==10){
					if(int.TryParse(rp.Substring(1,9),out rp_num)==true){
						if(int.TryParse(sec,out sec_num)==true){
							if(sec_num<80){
								wr.WriteLine(rp+","+sec);
								j++;
							}else{
								tablarow.Rows.Add(rp);
							}
						}else{
							tablarow.Rows.Add(rp);
						}
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
			
			dataGridView3.DataSource=tablarow;
			
			DialogResult respuesta = MessageBox.Show("Se Guardarán: "+j+" Registros y se Omitirán: " +tablarow.Rows.Count+" Registros/Sectores por no estar bien escritos.\n "+
			                                         "Está a punto de comenzar el proceso de consulta automática.\n"+
			                                         "Una vez comenzada la consulta automática NO se deberá manipular el\n"+
			                                         "equipo hasta que haya finalizado el proceso de consulta.\n"+
			                                         "El programa le informará cuando el proceso haya concluido\n\n"+
			                                         
			                                         "¿Desea comenzar el proceso de envío de Sectores?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
			if(respuesta ==DialogResult.Yes){
				
				MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
				StreamWriter wr2 = new StreamWriter(@"sectores_siscob.bat");
				wr2.WriteLine("@echo off");
				wr2.WriteLine("C:");
				wr2.WriteLine("cd \""+ruta.Substring(0,(ruta.Length-18))+"\"");
				wr2.WriteLine("start sectoptimus.exe");
				wr2.Close();
				System.Diagnostics.Process.Start(@"sectores_siscob.bat");
			}
		}
		
		public int guarda_xls(){
			SaveFileDialog dialog_save = new SaveFileDialog();
			dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
			dialog_save.Title = "Guardar resultados de Captura";//le damos un titulo a la ventana

			if (dialog_save.ShowDialog() == DialogResult.OK){
				nom_save = dialog_save.FileName; //open file
				return 1;
				//MessageBox.Show("El archivo se generó correctamente.\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
			}else{
				return 0;
			}
		}
		
		public void leer_resultados(){
			int i=0,corr=0,repe=0,err=0;
			string[] columna;
			String linea="";
			try
			{
				columna = new string[3];
				StreamReader rdr = new StreamReader(@"sectores/resultados_sectores.txt");
				while(!rdr.EndOfStream){
					linea=rdr.ReadLine();
					columna=linea.Split(',');
					tabla_results.Rows.Add(columna[0],columna[1],columna[2]);
					
					if(columna[2].Equals("CORRECTO")){
						corr++;
					}
					
					if(columna[2].Equals("REPETIDO")){
						repe++;
					}
					
					columna=null;
					i++;
				}
				
				XLWorkbook wb = new XLWorkbook();
				wb.Worksheets.Add(tabla_results,"Resultados");
				wb.SaveAs(nom_save);
				conex.guardar_evento("Se Mandó a Actualizar el Sector de Notificacion de "+i+"  Registros Patronales siendo actualizados correctamente "+corr+" Casos y resultando Repetidos "+repe+" Casos" );
				if(err==0){
					MessageBox.Show("Se completó el proceso de captura de sectores actualizados Exitosamente\n Totales: "+i+", Actualizados: "+corr+", Repetidos: "+repe,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
				}
			}catch(Exception et){
				MessageBox.Show("Ocurrió el siguiente problema al momento de leer el archivo de resultados: \n"+et.ToString(),"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=1;
			}
		}
		
		void SectorizadorLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			this.Text = window_name;

			llenar_Cb2_todos();
			tabla_results.Columns.Add("REGISTRO PATRONAL");
			tabla_results.Columns.Add("SECTOR ACTUALIZADO");
			tabla_results.Columns.Add("RESULTADO CAPTURA");
			
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			panel1.Visible=true;
			panel2.Visible=false;
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			panel2.Visible=true;
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
			cargar_hoja_excel();	
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(guarda_xls()==1){
				filtro();
			}else{
				MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
			}
		}
		
		void TerminarToolStripMenuItemClick(object sender, EventArgs e)
		{
 			
		}
	}
}
	
	
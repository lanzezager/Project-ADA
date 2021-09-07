/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 27/10/2016
 * Hora: 12:01 p.m.
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

namespace Nova_Gear.Depuracion
{
	/// <summary>
	/// Description of Depu_rale.
	/// </summary>
	public partial class Depu_rale : Form
	{
		public Depu_rale(int tipo_carga)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.tipo_inicio= tipo_carga;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String archivo_sip,tipo_rale="";
		int sip_tipo_load=0,gp_load=0,pro_load=0,tipo_inicio;
		string[] archivos_gp,archivos_tot;
		
		Conexion conex = new Conexion();
		DataTable hojas = new DataTable();
		DataTable hojas1 = new DataTable();
		DataTable hojas2 = new DataTable();
		DataTable rale = new DataTable();
		DataTable rale_fecha = new DataTable();
		DataTable data_acumulador = new DataTable();
		DataTable data_acumulador1 = new DataTable();
		DataTable tablarow3 = new DataTable();
		DataTable tablarow4 = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		
		//Declaracion de elementos para conexion office 2
		OleDbConnection conexion2 = null;
		DataSet dataSet2 = null;
		OleDbDataAdapter dataAdapter2 = null;
		
		//Declaracion de Hilo
		private Thread hilosecundario = null;
		
		public void carga_chema_excel_rale(){
			int i=0,filas = 0;
			String tabla;
			
			hojas.Rows.Clear();
			dataGridView1.DataSource  = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			
			filas=(dataGridView1.RowCount)-1;
			do{
				if (!(dataGridView1.Rows[i].Cells[3].Value.ToString()).Equals("")){
					if ((dataGridView1.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
						tabla=dataGridView1.Rows[i].Cells[2].Value.ToString();
						if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
							tabla = tabla.Remove((tabla.Length-1),1);
							hojas.Rows.Add(tabla);
						}
					}
				}
				i++;
			}while(i<=filas);
			
			dataGridView1.DataSource=null;
		}
		
		public void cargar_hoja_rale(){
		
			String hoja,cons_exc="";
			hoja = hojas.Rows[0][0].ToString();
			
			//MessageBox.Show(hojas.Rows[0][0].ToString());
				
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				if(tipo_inicio==1){//si se va a cargar el rale en la base de datos principal
					if(tipo_rale=="COP"){
                        //cons_exc = "Select [reg_pat],[mov],[fecha_mov],[sector],[credito],[ce],[periodo],[td],[fecha_alta],[fecha_notif],[inc],[fecha_inc],[dias],[importe] from [" + hoja + "$] where [inc] NOT in (\"20\",\"21\",\"22\",\"23\",\"25\") and ([td] =\"00\" or [td] =\"02\" or [td] =\"03\" or [td] =\"70\" or [td] =\"80\" or [td] =\"81\" or [td] =\"82\" or [td] =\"89\")";
                        cons_exc = "Select [reg_pat],[mov],[fecha_mov],[sector],[credito],[ce],[periodo],[td],[fecha_alta],[fecha_notif],[inc],[fecha_inc],[dias],[importe] from [" + hoja + "$])";
                    }else{
						if(tipo_rale=="RCV"){
                            //cons_exc = "Select [reg_pat],[mov],[fecha_mov],[sector],[credito],[ce],[periodo],[td],[fecha_alta],[fecha_notif],[inc],[fecha_inc],[dias],[importe] from [" + hoja + "$] where [inc] NOT in (\"20\",\"21\",\"22\",\"23\",\"25\") and ([td] =\"06\" or [td] =\"03\")";
                            cons_exc = "Select [reg_pat],[mov],[fecha_mov],[sector],[credito],[ce],[periodo],[td],[fecha_alta],[fecha_notif],[inc],[fecha_inc],[dias],[importe] from [" + hoja + "$])";
                        }
					}
				}else{//si se va a cargar el rale en la base de inventario, la misma linea para cop y rcv
					//cons_exc = "Select [reg_pat],[mov],[fecha_mov],[sector],[credito],[ce],[periodo],[td],[fecha_alta],[fecha_notif],[inc],[fecha_inc],[dias],[importe] from [" + hoja + "$] where [inc] NOT in (\"00\")";
                    cons_exc = "Select [reg_pat],[mov],[fecha_mov],[sector],[credito],[ce],[periodo],[td],[fecha_alta],[fecha_notif],[inc],[fecha_inc],[dias],[importe] from [" + hoja + "$] ";
				}
				//MessageBox.Show(cons_exc+"error");
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
						if(dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
							rale.Rows.Clear();
							rale=dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
							
							conexion.Close();//cerramos la conexion
						}
					}
				}catch(Exception fd){
					MessageBox.Show("Ocurrió un error al momento de leer el archivo:\n"+fd,"ERROR");
				}
			}
		}
		
		public void importar_rale(){
			
			int i=0,faltantes=0,j=0,k=0,l=0;
			String reg_pat,periodo,fecha_not,fecha_mov,fecha_inc,fecha_alta,ce;
			conex.conectar("base_principal");
			//MessageBox.Show(""+rale.Rows.Count);
			
			if(tipo_rale.Equals("COP")){
				tablarow4=conex.consultar("SELECT count(idrale) from rale where tipo_rale=\"COP\"");
				if(Convert.ToInt32(tablarow4.Rows[0][0].ToString())> 0){
					conex.consultar("DELETE FROM base_principal.rale WHERE tipo_rale=\"COP\"");
				}
			}else{
				if(tipo_rale.Equals("RCV")){
					tablarow4=conex.consultar("SELECT count(idrale) from rale where tipo_rale=\"RCV\"");
					if(Convert.ToInt32(tablarow4.Rows[0][0].ToString()) > 0){
						conex.consultar("DELETE FROM base_principal.rale WHERE tipo_rale=\"RCV\"");
					}
				}
			}
			
			while(i<rale.Rows.Count){
				j=i;
				k=0;
				rale_fecha.Rows.Clear();
				while(k<10){
					rale_fecha.Rows.Add();
					reg_pat=rale.Rows[j][0].ToString();
					reg_pat=reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
					rale.Rows[j][0]=reg_pat;
					
					if(rale.Rows[j][1].ToString().Length < 1){
						rale.Rows[j][1]="0";
					}
					
					fecha_mov=rale.Rows[j][2].ToString();
					fecha_mov=fecha_mov.Substring(6,4)+"-"+fecha_mov.Substring(3,2)+"-"+fecha_mov.Substring(0,2);
					//rale.Rows[j][2]=fecha_mov;
					rale_fecha.Rows[k][0]=fecha_mov.ToString();
					
					rale.Rows[j][4]=rale.Rows[j][4].ToString().Substring(0,9);
					
					periodo=rale.Rows[j][6].ToString();
					periodo=periodo.Substring(3,4)+periodo.Substring(0,2);
					rale.Rows[j][6]=periodo;
					
					fecha_alta=rale.Rows[j][8].ToString();
					fecha_alta=fecha_alta.Substring(6,4)+"-"+fecha_alta.Substring(3,2)+"-"+fecha_alta.Substring(0,2);
					//rale.Rows[j][8]=fecha_alta;
					rale_fecha.Rows[k][1]=fecha_alta.ToString();
					
					fecha_not=rale.Rows[j][9].ToString();
					if(fecha_not.Length>1){
						if(!(fecha_not.Substring(0,2).Equals("0/"))&&(fecha_not.Contains("2"))){
							fecha_not="\""+fecha_not.Substring(6,4)+"-"+fecha_not.Substring(3,2)+"-"+fecha_not.Substring(0,2)+"\"";
						}else{
							fecha_not="\"0001-01-01\"";
						}
					}else{
							fecha_not="\"0001-01-01\"";
					}
					rale.Rows[j][9]=fecha_not;
					
					//MessageBox.Show(rale.Rows[j][11].ToString());
					fecha_inc=rale.Rows[j][11].ToString();
					if(fecha_inc.Length>9){
						fecha_inc=fecha_inc.Substring(6,4)+"-"+fecha_inc.Substring(3,2)+"-"+fecha_inc.Substring(0,2);
						
						if(fecha_inc.Substring(0,1).Equals("#")){
							rale.Rows[j][11]="0001-01-01";
						}else{
							rale.Rows[j][11]=fecha_inc;
						}
					}else{
						rale.Rows[j][11]="0001-01-01";
					}
					
					j++;
					k++;
					
					
					
				}
				
				conex.consultar("INSERT INTO rale (registro_patronal,mov,fecha_mov,sector,credito,ce,periodo,td,fecha_alta,fecha_noti,incidencia,fecha_incidencia,dias,importe,tipo_rale) VALUES " +
				                "(\""+rale.Rows[i+0][0].ToString()+"\","+rale.Rows[i+0][1].ToString()+",\""+rale_fecha.Rows[0][0].ToString().Substring(0,10)+"\","+rale.Rows[i+0][3].ToString()+",\""+rale.Rows[i+0][4].ToString()+"\",\""+rale.Rows[i+0][5].ToString()+"\",\""+rale.Rows[i+0][6].ToString()+"\",\""+rale.Rows[i+0][7].ToString()+"\",\""+rale_fecha.Rows[0][1].ToString().Substring(0,10)+"\","+rale.Rows[i+0][9].ToString()+",\""+rale.Rows[i+0][10].ToString()+"\",\""+rale.Rows[i+0][11].ToString()+"\","+rale.Rows[i+0][12].ToString()+",\""+rale.Rows[i+0][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+1][0].ToString()+"\","+rale.Rows[i+1][1].ToString()+",\""+rale_fecha.Rows[1][0].ToString().Substring(0,10)+"\","+rale.Rows[i+1][3].ToString()+",\""+rale.Rows[i+1][4].ToString()+"\",\""+rale.Rows[i+1][5].ToString()+"\",\""+rale.Rows[i+1][6].ToString()+"\",\""+rale.Rows[i+1][7].ToString()+"\",\""+rale_fecha.Rows[1][1].ToString().Substring(0,10)+"\","+rale.Rows[i+1][9].ToString()+",\""+rale.Rows[i+1][10].ToString()+"\",\""+rale.Rows[i+1][11].ToString()+"\","+rale.Rows[i+1][12].ToString()+",\""+rale.Rows[i+1][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+2][0].ToString()+"\","+rale.Rows[i+2][1].ToString()+",\""+rale_fecha.Rows[2][0].ToString().Substring(0,10)+"\","+rale.Rows[i+2][3].ToString()+",\""+rale.Rows[i+2][4].ToString()+"\",\""+rale.Rows[i+2][5].ToString()+"\",\""+rale.Rows[i+2][6].ToString()+"\",\""+rale.Rows[i+2][7].ToString()+"\",\""+rale_fecha.Rows[2][1].ToString().Substring(0,10)+"\","+rale.Rows[i+2][9].ToString()+",\""+rale.Rows[i+2][10].ToString()+"\",\""+rale.Rows[i+2][11].ToString()+"\","+rale.Rows[i+2][12].ToString()+",\""+rale.Rows[i+2][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+3][0].ToString()+"\","+rale.Rows[i+3][1].ToString()+",\""+rale_fecha.Rows[3][0].ToString().Substring(0,10)+"\","+rale.Rows[i+3][3].ToString()+",\""+rale.Rows[i+3][4].ToString()+"\",\""+rale.Rows[i+3][5].ToString()+"\",\""+rale.Rows[i+3][6].ToString()+"\",\""+rale.Rows[i+3][7].ToString()+"\",\""+rale_fecha.Rows[3][1].ToString().Substring(0,10)+"\","+rale.Rows[i+3][9].ToString()+",\""+rale.Rows[i+3][10].ToString()+"\",\""+rale.Rows[i+3][11].ToString()+"\","+rale.Rows[i+3][12].ToString()+",\""+rale.Rows[i+3][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+4][0].ToString()+"\","+rale.Rows[i+4][1].ToString()+",\""+rale_fecha.Rows[4][0].ToString().Substring(0,10)+"\","+rale.Rows[i+4][3].ToString()+",\""+rale.Rows[i+4][4].ToString()+"\",\""+rale.Rows[i+4][5].ToString()+"\",\""+rale.Rows[i+4][6].ToString()+"\",\""+rale.Rows[i+4][7].ToString()+"\",\""+rale_fecha.Rows[4][1].ToString().Substring(0,10)+"\","+rale.Rows[i+4][9].ToString()+",\""+rale.Rows[i+4][10].ToString()+"\",\""+rale.Rows[i+4][11].ToString()+"\","+rale.Rows[i+4][12].ToString()+",\""+rale.Rows[i+4][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+5][0].ToString()+"\","+rale.Rows[i+5][1].ToString()+",\""+rale_fecha.Rows[5][0].ToString().Substring(0,10)+"\","+rale.Rows[i+5][3].ToString()+",\""+rale.Rows[i+5][4].ToString()+"\",\""+rale.Rows[i+5][5].ToString()+"\",\""+rale.Rows[i+5][6].ToString()+"\",\""+rale.Rows[i+5][7].ToString()+"\",\""+rale_fecha.Rows[5][1].ToString().Substring(0,10)+"\","+rale.Rows[i+5][9].ToString()+",\""+rale.Rows[i+5][10].ToString()+"\",\""+rale.Rows[i+5][11].ToString()+"\","+rale.Rows[i+5][12].ToString()+",\""+rale.Rows[i+5][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+6][0].ToString()+"\","+rale.Rows[i+6][1].ToString()+",\""+rale_fecha.Rows[6][0].ToString().Substring(0,10)+"\","+rale.Rows[i+6][3].ToString()+",\""+rale.Rows[i+6][4].ToString()+"\",\""+rale.Rows[i+6][5].ToString()+"\",\""+rale.Rows[i+6][6].ToString()+"\",\""+rale.Rows[i+6][7].ToString()+"\",\""+rale_fecha.Rows[6][1].ToString().Substring(0,10)+"\","+rale.Rows[i+6][9].ToString()+",\""+rale.Rows[i+6][10].ToString()+"\",\""+rale.Rows[i+6][11].ToString()+"\","+rale.Rows[i+6][12].ToString()+",\""+rale.Rows[i+6][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+7][0].ToString()+"\","+rale.Rows[i+7][1].ToString()+",\""+rale_fecha.Rows[7][0].ToString().Substring(0,10)+"\","+rale.Rows[i+7][3].ToString()+",\""+rale.Rows[i+7][4].ToString()+"\",\""+rale.Rows[i+7][5].ToString()+"\",\""+rale.Rows[i+7][6].ToString()+"\",\""+rale.Rows[i+7][7].ToString()+"\",\""+rale_fecha.Rows[7][1].ToString().Substring(0,10)+"\","+rale.Rows[i+7][9].ToString()+",\""+rale.Rows[i+7][10].ToString()+"\",\""+rale.Rows[i+7][11].ToString()+"\","+rale.Rows[i+7][12].ToString()+",\""+rale.Rows[i+7][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+8][0].ToString()+"\","+rale.Rows[i+8][1].ToString()+",\""+rale_fecha.Rows[8][0].ToString().Substring(0,10)+"\","+rale.Rows[i+8][3].ToString()+",\""+rale.Rows[i+8][4].ToString()+"\",\""+rale.Rows[i+8][5].ToString()+"\",\""+rale.Rows[i+8][6].ToString()+"\",\""+rale.Rows[i+8][7].ToString()+"\",\""+rale_fecha.Rows[8][1].ToString().Substring(0,10)+"\","+rale.Rows[i+8][9].ToString()+",\""+rale.Rows[i+8][10].ToString()+"\",\""+rale.Rows[i+8][11].ToString()+"\","+rale.Rows[i+8][12].ToString()+",\""+rale.Rows[i+8][13].ToString()+"\",\""+tipo_rale+"\"),"+
				                "(\""+rale.Rows[i+9][0].ToString()+"\","+rale.Rows[i+9][1].ToString()+",\""+rale_fecha.Rows[9][0].ToString().Substring(0,10)+"\","+rale.Rows[i+9][3].ToString()+",\""+rale.Rows[i+9][4].ToString()+"\",\""+rale.Rows[i+9][5].ToString()+"\",\""+rale.Rows[i+9][6].ToString()+"\",\""+rale.Rows[i+9][7].ToString()+"\",\""+rale_fecha.Rows[9][1].ToString().Substring(0,10)+"\","+rale.Rows[i+9][9].ToString()+",\""+rale.Rows[i+9][10].ToString()+"\",\""+rale.Rows[i+9][11].ToString()+"\","+rale.Rows[i+9][12].ToString()+",\""+rale.Rows[i+9][13].ToString()+"\",\""+tipo_rale+"\")");
				i=i+10;
				Invoke(new MethodInvoker(delegate{
					label4.Text="Importando registros del RALE a la base de datos "+(i)+" de "+rale.Rows.Count;
					label4.Refresh();
				                         }));
				
				if((rale.Rows.Count-i)<10){
					l=i;
					i=rale.Rows.Count+1;
				}
				
				llenar_barra1(i);
			}
			
			i=l;
			
			while(i<rale.Rows.Count){
				
				reg_pat=rale.Rows[i][0].ToString();
				reg_pat=reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
				rale.Rows[i][0]=reg_pat;
				
				if(rale.Rows[j][1].ToString().Length < 1){
					rale.Rows[j][1]="0";
				}
				
				fecha_mov=rale.Rows[i][2].ToString();
				fecha_mov=fecha_mov.Substring(6,4)+"-"+fecha_mov.Substring(3,2)+"-"+fecha_mov.Substring(0,2);
				rale.Rows[i][2]=fecha_mov;
				
				periodo=rale.Rows[i][6].ToString();
				periodo=periodo.Substring(3,4)+periodo.Substring(0,2);
				rale.Rows[i][6]=periodo;
				
				fecha_alta=rale.Rows[i][8].ToString();
				fecha_alta=fecha_alta.Substring(6,4)+"-"+fecha_alta.Substring(3,2)+"-"+fecha_alta.Substring(0,2);
				rale.Rows[i][8]=fecha_alta;
				
				
				fecha_not=rale.Rows[i][9].ToString();
				if(fecha_not.Length>1){
					if(!(fecha_not.Substring(0,2).Equals("0/"))&&(fecha_not.Contains("2"))){
						fecha_not="\""+fecha_not.Substring(6,4)+"-"+fecha_not.Substring(3,2)+"-"+fecha_not.Substring(0,2)+"\"";
					}else{
						fecha_not="\"0001-01-01\"";
					}
				}else{
					fecha_not="\"0001-01-01\"";
				}
				rale.Rows[i][9]=fecha_not;
				
				//MessageBox.Show(rale.Rows[j][11].ToString());
				fecha_inc=rale.Rows[i][11].ToString();
				if(fecha_inc.Length>9){
					fecha_inc=fecha_inc.Substring(6,4)+"-"+fecha_inc.Substring(3,2)+"-"+fecha_inc.Substring(0,2);
					if(fecha_inc.Substring(0,1).Equals("#")){
						rale.Rows[i][11]="0001-01-01";
					}else{
						rale.Rows[i][11]=fecha_inc;
					}
				}else{
					rale.Rows[i][11]="0001-01-01";
				}
				
				rale.Rows[j][4]=rale.Rows[j][4].ToString().Substring(0,9);
				
				conex.consultar("INSERT INTO rale (registro_patronal,mov,fecha_mov,sector,credito,ce,periodo,td,fecha_alta,fecha_noti,incidencia,fecha_incidencia,dias,importe,tipo_rale) VALUES " +
				                "(\""+rale.Rows[i+0][0].ToString()+"\","+rale.Rows[i+0][1].ToString()+",\""+fecha_mov+"\","+rale.Rows[i+0][3].ToString()+",\""+rale.Rows[i+0][4].ToString()+"\",\""+rale.Rows[i+0][5].ToString()+"\",\""+rale.Rows[i+0][6].ToString()+"\",\""+rale.Rows[i+0][7].ToString()+"\",\""+fecha_alta+"\","+rale.Rows[i+0][9].ToString()+",\""+rale.Rows[i+0][10].ToString()+"\",\""+rale.Rows[i+0][11].ToString()+"\","+rale.Rows[i+0][12].ToString()+",\""+rale.Rows[i+0][13].ToString()+"\",\""+tipo_rale+"\")");
				
				Invoke(new MethodInvoker(delegate{
					label4.Text="Importando registros del RALE a la base de datos "+(i)+" de "+rale.Rows.Count;
					label4.Refresh();
				                         }));
				i++;
				llenar_barra1(i);
			}
			llenar_barra1(rale.Rows.Count);
			Invoke(new MethodInvoker(delegate{
					label4.Text="Importando registros del RALE a la base de datos "+(rale.Rows.Count)+" de "+rale.Rows.Count;
					label4.Refresh();
				                         }));
			conex.guardar_evento("Se Ingreso el RALE_"+tipo_rale.ToUpper()+" con "+rale.Rows.Count+" registros");
		}
		
		public void importar_rale_inventario(){
			
			int i=0,faltantes=0,j=0,k=0,l=0;
			String reg_pat,periodo,fecha_not,fecha_mov,fecha_inc,fecha_alta,ce,periodo_mes="";
			conex.conectar("inventario");
			//MessageBox.Show(""+rale.Rows.Count);
			
			if(tipo_rale.Equals("COP")){
				tablarow4=conex.consultar("SELECT count(idbase_inventario) from base_inventario where clase_doc=\"COP\"");
				if(Convert.ToInt32(tablarow4.Rows[0][0].ToString())> 0){
					conex.consultar("DELETE FROM base_principal.rale WHERE clase_doc=\"COP\"");
				}
			}else{
				if(tipo_rale.Equals("RCV")){
					tablarow4=conex.consultar("SELECT count(idbase_inventario) from base_inventario where clase_doc=\"RCV\"");
					if(Convert.ToInt32(tablarow4.Rows[0][0].ToString()) > 0){
						conex.consultar("DELETE FROM base_principal.rale WHERE clase_doc=\"RCV\"");
					}
				}
			}
			
			while(i<rale.Rows.Count){
				j=i;
				k=0;
				rale_fecha.Rows.Clear();
				
				while(k<10){
					rale_fecha.Rows.Add();
					reg_pat=rale.Rows[j][0].ToString();
					//reg_pat=reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
					rale.Rows[j][0]=reg_pat;
					
					if(rale.Rows[j][1].ToString().Length < 1){
						rale.Rows[j][1]="0";
					}
					
					fecha_mov=rale.Rows[j][2].ToString();
					fecha_mov=fecha_mov.Substring(6,4)+"-"+fecha_mov.Substring(3,2)+"-"+fecha_mov.Substring(0,2);
					//rale.Rows[j][2]=fecha_mov;
					rale_fecha.Rows[k][0]=fecha_mov.ToString();
					
					rale.Rows[j][4]=rale.Rows[j][4].ToString().Substring(0,9);
					
					periodo=rale.Rows[j][6].ToString();
					periodo=periodo.Substring(3,4);//periodo año
					rale.Rows[j][6]=periodo;
					periodo_mes=periodo.Substring(0,2);//periodo mes
					
					fecha_alta=rale.Rows[j][8].ToString();
					fecha_alta=fecha_alta.Substring(6,4)+"-"+fecha_alta.Substring(3,2)+"-"+fecha_alta.Substring(0,2);
					//rale.Rows[j][8]=fecha_alta;
					rale_fecha.Rows[k][1]=fecha_alta.ToString();
					
					fecha_not=rale.Rows[j][9].ToString();
					if(fecha_not.Length>1){
						if(!(fecha_not.Substring(0,2).Equals("0/"))&&(fecha_not.Contains("2"))){
							fecha_not="\""+fecha_not.Substring(6,4)+"-"+fecha_not.Substring(3,2)+"-"+fecha_not.Substring(0,2)+"\"";
						}else{
							fecha_not="\"0001-01-01\"";
						}
					}else{
							fecha_not="\"0001-01-01\"";
					}
					rale.Rows[j][9]=fecha_not;
					
					//MessageBox.Show(rale.Rows[j][11].ToString());
					fecha_inc=rale.Rows[j][11].ToString();
					if(fecha_inc.Length>9){
						fecha_inc=fecha_inc.Substring(6,4)+"-"+fecha_inc.Substring(3,2)+"-"+fecha_inc.Substring(0,2);
						
						if(fecha_inc.Substring(0,1).Equals("#")){
							rale.Rows[j][11]="0001-01-01";
						}else{
							rale.Rows[j][11]=fecha_inc;
						}
					}else{
						rale.Rows[j][11]="0001-01-01";
					}
					
					j++;
					k++;
					
					
					
				}
				
				conex.consultar("INSERT INTO base_inventario (reg_pat,mov,fecha_mov,sector,credito,ce,periodo_anio,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,periodo_mes) VALUES " +
				                "(\""+rale.Rows[i+0][0].ToString()+"\","+rale.Rows[i+0][1].ToString()+",\""+rale_fecha.Rows[0][0].ToString().Substring(0,10)+"\","+rale.Rows[i+0][3].ToString()+",\""+rale.Rows[i+0][4].ToString()+"\",\""+rale.Rows[i+0][5].ToString()+"\",\""+rale.Rows[i+0][6].ToString()+"\",\""+rale.Rows[i+0][7].ToString()+"\",\""+rale_fecha.Rows[0][1].ToString().Substring(0,10)+"\","+rale.Rows[i+0][9].ToString()+",\""+rale.Rows[i+0][10].ToString()+"\",\""+rale.Rows[i+0][11].ToString()+"\","+rale.Rows[i+0][12].ToString()+",\""+rale.Rows[i+0][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+1][0].ToString()+"\","+rale.Rows[i+1][1].ToString()+",\""+rale_fecha.Rows[1][0].ToString().Substring(0,10)+"\","+rale.Rows[i+1][3].ToString()+",\""+rale.Rows[i+1][4].ToString()+"\",\""+rale.Rows[i+1][5].ToString()+"\",\""+rale.Rows[i+1][6].ToString()+"\",\""+rale.Rows[i+1][7].ToString()+"\",\""+rale_fecha.Rows[1][1].ToString().Substring(0,10)+"\","+rale.Rows[i+1][9].ToString()+",\""+rale.Rows[i+1][10].ToString()+"\",\""+rale.Rows[i+1][11].ToString()+"\","+rale.Rows[i+1][12].ToString()+",\""+rale.Rows[i+1][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+2][0].ToString()+"\","+rale.Rows[i+2][1].ToString()+",\""+rale_fecha.Rows[2][0].ToString().Substring(0,10)+"\","+rale.Rows[i+2][3].ToString()+",\""+rale.Rows[i+2][4].ToString()+"\",\""+rale.Rows[i+2][5].ToString()+"\",\""+rale.Rows[i+2][6].ToString()+"\",\""+rale.Rows[i+2][7].ToString()+"\",\""+rale_fecha.Rows[2][1].ToString().Substring(0,10)+"\","+rale.Rows[i+2][9].ToString()+",\""+rale.Rows[i+2][10].ToString()+"\",\""+rale.Rows[i+2][11].ToString()+"\","+rale.Rows[i+2][12].ToString()+",\""+rale.Rows[i+2][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+3][0].ToString()+"\","+rale.Rows[i+3][1].ToString()+",\""+rale_fecha.Rows[3][0].ToString().Substring(0,10)+"\","+rale.Rows[i+3][3].ToString()+",\""+rale.Rows[i+3][4].ToString()+"\",\""+rale.Rows[i+3][5].ToString()+"\",\""+rale.Rows[i+3][6].ToString()+"\",\""+rale.Rows[i+3][7].ToString()+"\",\""+rale_fecha.Rows[3][1].ToString().Substring(0,10)+"\","+rale.Rows[i+3][9].ToString()+",\""+rale.Rows[i+3][10].ToString()+"\",\""+rale.Rows[i+3][11].ToString()+"\","+rale.Rows[i+3][12].ToString()+",\""+rale.Rows[i+3][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+4][0].ToString()+"\","+rale.Rows[i+4][1].ToString()+",\""+rale_fecha.Rows[4][0].ToString().Substring(0,10)+"\","+rale.Rows[i+4][3].ToString()+",\""+rale.Rows[i+4][4].ToString()+"\",\""+rale.Rows[i+4][5].ToString()+"\",\""+rale.Rows[i+4][6].ToString()+"\",\""+rale.Rows[i+4][7].ToString()+"\",\""+rale_fecha.Rows[4][1].ToString().Substring(0,10)+"\","+rale.Rows[i+4][9].ToString()+",\""+rale.Rows[i+4][10].ToString()+"\",\""+rale.Rows[i+4][11].ToString()+"\","+rale.Rows[i+4][12].ToString()+",\""+rale.Rows[i+4][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+5][0].ToString()+"\","+rale.Rows[i+5][1].ToString()+",\""+rale_fecha.Rows[5][0].ToString().Substring(0,10)+"\","+rale.Rows[i+5][3].ToString()+",\""+rale.Rows[i+5][4].ToString()+"\",\""+rale.Rows[i+5][5].ToString()+"\",\""+rale.Rows[i+5][6].ToString()+"\",\""+rale.Rows[i+5][7].ToString()+"\",\""+rale_fecha.Rows[5][1].ToString().Substring(0,10)+"\","+rale.Rows[i+5][9].ToString()+",\""+rale.Rows[i+5][10].ToString()+"\",\""+rale.Rows[i+5][11].ToString()+"\","+rale.Rows[i+5][12].ToString()+",\""+rale.Rows[i+5][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+6][0].ToString()+"\","+rale.Rows[i+6][1].ToString()+",\""+rale_fecha.Rows[6][0].ToString().Substring(0,10)+"\","+rale.Rows[i+6][3].ToString()+",\""+rale.Rows[i+6][4].ToString()+"\",\""+rale.Rows[i+6][5].ToString()+"\",\""+rale.Rows[i+6][6].ToString()+"\",\""+rale.Rows[i+6][7].ToString()+"\",\""+rale_fecha.Rows[6][1].ToString().Substring(0,10)+"\","+rale.Rows[i+6][9].ToString()+",\""+rale.Rows[i+6][10].ToString()+"\",\""+rale.Rows[i+6][11].ToString()+"\","+rale.Rows[i+6][12].ToString()+",\""+rale.Rows[i+6][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+7][0].ToString()+"\","+rale.Rows[i+7][1].ToString()+",\""+rale_fecha.Rows[7][0].ToString().Substring(0,10)+"\","+rale.Rows[i+7][3].ToString()+",\""+rale.Rows[i+7][4].ToString()+"\",\""+rale.Rows[i+7][5].ToString()+"\",\""+rale.Rows[i+7][6].ToString()+"\",\""+rale.Rows[i+7][7].ToString()+"\",\""+rale_fecha.Rows[7][1].ToString().Substring(0,10)+"\","+rale.Rows[i+7][9].ToString()+",\""+rale.Rows[i+7][10].ToString()+"\",\""+rale.Rows[i+7][11].ToString()+"\","+rale.Rows[i+7][12].ToString()+",\""+rale.Rows[i+7][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+8][0].ToString()+"\","+rale.Rows[i+8][1].ToString()+",\""+rale_fecha.Rows[8][0].ToString().Substring(0,10)+"\","+rale.Rows[i+8][3].ToString()+",\""+rale.Rows[i+8][4].ToString()+"\",\""+rale.Rows[i+8][5].ToString()+"\",\""+rale.Rows[i+8][6].ToString()+"\",\""+rale.Rows[i+8][7].ToString()+"\",\""+rale_fecha.Rows[8][1].ToString().Substring(0,10)+"\","+rale.Rows[i+8][9].ToString()+",\""+rale.Rows[i+8][10].ToString()+"\",\""+rale.Rows[i+8][11].ToString()+"\","+rale.Rows[i+8][12].ToString()+",\""+rale.Rows[i+8][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\"),"+
				                "(\""+rale.Rows[i+9][0].ToString()+"\","+rale.Rows[i+9][1].ToString()+",\""+rale_fecha.Rows[9][0].ToString().Substring(0,10)+"\","+rale.Rows[i+9][3].ToString()+",\""+rale.Rows[i+9][4].ToString()+"\",\""+rale.Rows[i+9][5].ToString()+"\",\""+rale.Rows[i+9][6].ToString()+"\",\""+rale.Rows[i+9][7].ToString()+"\",\""+rale_fecha.Rows[9][1].ToString().Substring(0,10)+"\","+rale.Rows[i+9][9].ToString()+",\""+rale.Rows[i+9][10].ToString()+"\",\""+rale.Rows[i+9][11].ToString()+"\","+rale.Rows[i+9][12].ToString()+",\""+rale.Rows[i+9][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\")");
				i=i+10;
				Invoke(new MethodInvoker(delegate{
					label4.Text="Importando registros del RALE a la base de datos "+(i)+" de "+rale.Rows.Count;
					label4.Refresh();
				                         }));
				
				if((rale.Rows.Count-i)<10){
					l=i;
					i=rale.Rows.Count+1;
				}
				
				llenar_barra1(i);
			}
			
			i=l;
			
			while(i<rale.Rows.Count){
				
				reg_pat=rale.Rows[i][0].ToString();
				//reg_pat=reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
				rale.Rows[i][0]=reg_pat;
				
				if(rale.Rows[j][1].ToString().Length < 1){
					rale.Rows[j][1]="0";
				}
				
				fecha_mov=rale.Rows[i][2].ToString();
				fecha_mov=fecha_mov.Substring(6,4)+"-"+fecha_mov.Substring(3,2)+"-"+fecha_mov.Substring(0,2);
				rale.Rows[i][2]=fecha_mov;
				
				periodo=rale.Rows[i][6].ToString();
				periodo=periodo.Substring(3,4);//periodo año
				rale.Rows[i][6]=periodo;
				periodo_mes=periodo.Substring(0,2);//periodo mes
				
				fecha_alta=rale.Rows[i][8].ToString();
				fecha_alta=fecha_alta.Substring(6,4)+"-"+fecha_alta.Substring(3,2)+"-"+fecha_alta.Substring(0,2);
				rale.Rows[i][8]=fecha_alta;
				
				
				fecha_not=rale.Rows[i][9].ToString();
				if(fecha_not.Length>1){
					if(!(fecha_not.Substring(0,2).Equals("0/"))&&(fecha_not.Contains("2"))){
						fecha_not="\""+fecha_not.Substring(6,4)+"-"+fecha_not.Substring(3,2)+"-"+fecha_not.Substring(0,2)+"\"";
					}else{
						fecha_not="\"0001-01-01\"";
					}
				}else{
					fecha_not="\"0001-01-01\"";
				}
				rale.Rows[i][9]=fecha_not;
				
				//MessageBox.Show(rale.Rows[j][11].ToString());
				fecha_inc=rale.Rows[i][11].ToString();
				if(fecha_inc.Length>9){
					fecha_inc=fecha_inc.Substring(6,4)+"-"+fecha_inc.Substring(3,2)+"-"+fecha_inc.Substring(0,2);
					if(fecha_inc.Substring(0,1).Equals("#")){
						rale.Rows[i][11]="0001-01-01";
					}else{
						rale.Rows[i][11]=fecha_inc;
					}
				}else{
					rale.Rows[i][11]="0001-01-01";
				}
				
				rale.Rows[j][4]=rale.Rows[j][4].ToString().Substring(0,9);
				
				conex.consultar("INSERT INTO base_inventario (reg_pat,mov,fecha_mov,sector,credito,ce,periodo_anio,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,periodo_mes) VALUES " +
				                "(\""+rale.Rows[i+0][0].ToString()+"\","+rale.Rows[i+0][1].ToString()+",\""+fecha_mov+"\","+rale.Rows[i+0][3].ToString()+",\""+rale.Rows[i+0][4].ToString()+"\",\""+rale.Rows[i+0][5].ToString()+"\",\""+rale.Rows[i+0][6].ToString()+"\",\""+rale.Rows[i+0][7].ToString()+"\",\""+fecha_alta+"\","+rale.Rows[i+0][9].ToString()+",\""+rale.Rows[i+0][10].ToString()+"\",\""+rale.Rows[i+0][11].ToString()+"\","+rale.Rows[i+0][12].ToString()+",\""+rale.Rows[i+0][13].ToString()+"\",\""+tipo_rale+"\",\""+periodo_mes+"\")");
				
				Invoke(new MethodInvoker(delegate{
					label4.Text="Importando registros del RALE a la base de datos "+(i)+" de "+rale.Rows.Count;
					label4.Refresh();
				                         }));
				i++;
				llenar_barra1(i);
			}
			llenar_barra1(rale.Rows.Count);
			Invoke(new MethodInvoker(delegate{
					label4.Text="Importando registros del RALE a la base de datos "+(rale.Rows.Count)+" de "+rale.Rows.Count;
					label4.Refresh();
				                         }));
			conex.guardar_evento("Se Ingreso el RALE_"+tipo_rale.ToUpper()+" con "+rale.Rows.Count+" casos En La Base Inventario");
		}
		
		public void cargar_rale(){
			try{
			
				Invoke(new MethodInvoker(delegate
				                         {label4.Text="Leyendo RALE";}));
				cargar_hoja_rale();
				
				Invoke(new MethodInvoker(delegate
				                         {label4.Text="Comenzando Importación de RALE a la base de datos.";}));
				if(tipo_inicio==1){//cargar rale en base_principal
					importar_rale();
				}else{//cargar rale en inventario
					importar_rale_inventario();
				}
			}catch(Exception es){
				MessageBox.Show("Ocurrió un error durante la carga del archivo de Excel:\n"+es,"ERROR");
			}
		}

		public void carga_chema_excel(){
			int i=0,filas = 0;
			String tabla;
			
			hojas1.Rows.Clear();
			dataGridView1.DataSource  = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			
			filas=(dataGridView1.RowCount)-1;
			do{
				if (!(dataGridView1.Rows[i].Cells[3].Value.ToString()).Equals("")){
					if ((dataGridView1.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
						tabla=dataGridView1.Rows[i].Cells[2].Value.ToString();
						if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
							tabla = tabla.Remove((tabla.Length-1),1);
							hojas1.Rows.Add(tabla);
						}
					}
				}
				i++;
			}while(i<=filas);
			dataGridView1.DataSource=null;
		}
		
		public void cargar_archivos(){
			if(sip_tipo_load==1){
				
			}
			
			if(sip_tipo_load==2){
				
			}
			
			if(pro_load==1){
				cargar_procesar();
			}
			
			if(gp_load==1){
				cargar_archivos_general_pagos();
			}
		}
		
		public void carga_excel_general_pagos(String archy){
			String archivo,ext,cad_con;
			//archivo = dialog.FileName;
			archivo = archy;
			ext=archivo.Substring(((archivo.Length)-3),3);
			ext=ext.ToLower();
			
			if((ext.Equals("xls"))||(ext.Equals("lsx"))){
				//esta cadena es para archivos excel 2007 y 2010
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion
				carga_chema_general_pagos();
			}
			
			
		}
		
		public void carga_chema_general_pagos(){
			int i=0,filas = 0;
			String tabla;
			
			hojas2.Rows.Clear();
			dataGridView1.DataSource  = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			
			filas=(dataGridView1.RowCount)-1;
			do{
				if (!(dataGridView1.Rows[i].Cells[3].Value.ToString()).Equals("")){
					if ((dataGridView1.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
						tabla=dataGridView1.Rows[i].Cells[2].Value.ToString();
						if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
							tabla = tabla.Remove((tabla.Length-1),1);
							hojas2.Rows.Add(tabla);
						}
					}
				}
				i++;
			}while(i<=filas);
			dataGridView1.DataSource=null;
		}
		
		public void cargar_hoja_excel_general_pagos(int iter){
			
			String hoja,cons_exc,per_sel;
			
			hoja = hojas2.Rows[0][0].ToString();
			per_sel = maskedTextBox1.Text;  
			
			
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select [REGISTRO],[RC_NUM_FOL],[RC_FEC_MOV],[RC_IMP_TOT],[RC_PER] from [" + hoja + "$] where [RC_PER] like \""+per_sel.Substring(0,4)+"/"+per_sel.Substring(4,2)+"\" and ([RC_MOD] = 10 or [RC_MOD] = 13 or [RC_MOD] = 17) and ([RC_DOC] = 1 or [RC_DOC] = 2)";
				
				try
				{
					
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						if (dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
						
						label4.Text="Leyendo archivo: "+iter+" de GENERAL DE PAGOS";
						label4.Refresh();
						                         
						tablarow3.Rows.Clear();
						tablarow3 = dataSet.Tables[0];
						
						}
						
                        if(tablarow3.Rows.Count>0){
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
		
		public void cargar_archivos_general_pagos(){
            
			int xy=0,tot_filas_da=0,p=0;
			data_acumulador.Rows.Clear();
			xy=0;
			String dd="";
			double sd=0;
			String nrp="",f_sua="",fecha_pago="",i4ss="",per_pag_gp="",rcv_sip="";
			
			do{
				carga_excel_general_pagos(archivos_gp[xy]);
				cargar_hoja_excel_general_pagos((xy+1));
				xy++;
			}while(xy<archivos_gp.Length);
			
			
			//gp_count=0;
			tot_filas_da=data_acumulador.Rows.Count;
            tablarow3.Columns.Clear();
            tablarow3.Rows.Clear();
            tablarow3.Columns.Add("NRP", dd.GetType());
            tablarow3.Columns.Add("F#SUA", dd.GetType());
            tablarow3.Columns.Add("FECHA PAGO", dd.GetType());
            tablarow3.Columns.Add("4SS",sd.GetType());
            tablarow3.Columns.Add("RCV", sd.GetType());
            tablarow3.Columns.Add("PERIODO_PAGO", dd.GetType());
            tablarow3.Columns.Add("ARCHIVO_ORIGEN", dd.GetType());

            
            MessageBox.Show("fecha_pago = " + data_acumulador.Rows[p][2].ToString().Substring(0, 10) + " 4ss =" + data_acumulador.Rows[p][3].ToString()+" tipo col="+tablarow3.Columns[3].DataType.ToString());
			while(p<tot_filas_da){
				
				                         	nrp=data_acumulador.Rows[p][0].ToString();
				                         	f_sua=data_acumulador.Rows[p][1].ToString();
				                         	fecha_pago=data_acumulador.Rows[p][2].ToString().Substring(0,10);
				                         	i4ss=data_acumulador.Rows[p][3].ToString();
				                         	per_pag_gp=data_acumulador.Rows[p][4].ToString();
				                         	rcv_sip="0.00";
				
				if(((Convert.ToDouble(i4ss))>1)){
					
					if((nrp.Length<=10)&&(nrp.Substring(0,1).Equals("4"))){
						nrp="0"+nrp;
					}
					
					nrp = nrp.Substring(0, 10);
					tablarow3.Rows.Add(nrp,f_sua,fecha_pago,Convert.ToDouble(i4ss),Convert.ToDouble(rcv_sip),per_pag_gp);
					//y++;
				}
				p++;
				
				Invoke(new MethodInvoker(delegate
				                         {
				                         label4.Text="Analizando datos extraídos de de GENERAL DE PAGOS..."+p+" de "+tot_filas_da ;
				                         label4.Refresh();
				                         }));
				
			}
			
			Invoke(new MethodInvoker(delegate
			                         {
			                         	label4.Text="Creando datos temporales...";
			                         	label4.Refresh();
			                         	//guardar_excel_temp();
			                         	XLWorkbook wb = new XLWorkbook();
			                         	wb.Worksheets.Add(tablarow3,"hoja_lz");
			                         	wb.SaveAs(@"extracto_gp.xlsx");
			                         }));
			
			Invoke(new MethodInvoker(delegate
			                         {
			                         	label4.Text="Listo! Se completo la lectura del (los) archivo(s) GENERAL DE PAGOS";
			                         	label4.Refresh();
			                         	MessageBox.Show("Se recopilaron:\n"+tablarow3.Rows.Count+" registros de "+archivos_gp.Length+" archivos de GENERAL DE PAGOS\n\n Da click en aceptar para continuar. ","Información");
			                         	
                                       
			                         	
			                         }));
			
		}
		
		public void cargar_hoja_excel_procesar(int i){
			
			String hoja,per_sel,cons_exc;
			per_sel = maskedTextBox1.Text;
			hoja = hojas1.Rows[i][0].ToString();
				
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select [NRP],[F#SUA],[FECHA PAGO],[4SS],[RCV] from [" + hoja + "$] where [PERIODO PAGO] like \""+per_sel+"\" and [DIAG 1] <> 146 and [DIAG 1] <> 857";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion2); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					//tablarow3.Rows.Clear();
					//dataSet.Tables.Add(tablarow3);	
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						if (dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                            
						//dataGridView3.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						data_acumulador1.Merge(dataSet.Tables[0]);
						conexion.Close();//cerramos la conexion
						//dataGridView3.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						 
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
		
		public void cargar_procesar(){
			int i=0;
			
			MessageBox.Show(hojas1.Rows[4][0].ToString());
			while(i<hojas1.Rows.Count){
				
				cargar_hoja_excel_procesar(i);
				label4.Text="Leyendo Hoja "+(i+1)+" de "+hojas1.Rows.Count;
				label4.Refresh();
				i++;
			}
			
			label4.Text="Creando datos temporales...";
			label4.Refresh();
			//guardar_excel_temp();
			XLWorkbook wb = new XLWorkbook();
			wb.Worksheets.Add(data_acumulador1,"hoja_lz");
			wb.SaveAs(@"extracto_pro.xlsx");
		
		}
		
		public void llenar_barra1(int prog){
			int porcentaje=0;
			
			porcentaje=Convert.ToInt32((prog*100)/rale.Rows.Count);
			Invoke(new MethodInvoker(delegate{
					
				label3.Text=""+porcentaje+" %";
				if(porcentaje<101){
					progressBar1.Value=porcentaje;
				}else{
					progressBar1.Value=100;
				}
			                         }));
		}
		
		public void llenar_barra2(){
			
			
		}
		
		//RALE
		void Button1Click(object sender, EventArgs e)
		{
			String archivo,ext,cad_con;
			
			OpenFileDialog dialog = new OpenFileDialog();
			
			dialog.Title = "Seleccione el archivo RALE";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque

			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				label1.Text="RALE: "+archivo;
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();

				if((ext.Equals("xls"))||(ext.Equals("lsx"))){
					try{
						//esta cadena es para archivos excel 2007 y 2010
						cad_con= "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
						conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
						conexion.Open(); //abrimos la conexion
						carga_chema_excel_rale();
						button1.Enabled=false;
						pictureBox1.Visible=true;
					}catch(Exception es){
						
					}
				}
				
			}
		}
		
		void Label2Click(object sender, EventArgs e)
		{
			
		}
		
		void Depu_raleLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			hojas.Columns.Add("hoja");
			hojas1.Columns.Add("hoja");
			hojas2.Columns.Add("hoja");
			
			String dde="";
			double sx=0;
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			tablarow3.Columns.Add();
			
			rale.Columns.Add("columna01",tipo_rale.GetType());
			rale.Columns.Add("columna02",tipo_rale.GetType());
			rale.Columns.Add("columna03",tipo_rale.GetType());
			rale.Columns.Add("columna04",tipo_rale.GetType());
			rale.Columns.Add("columna05",tipo_rale.GetType());
			rale.Columns.Add("columna06",tipo_rale.GetType());
			rale.Columns.Add("columna07",tipo_rale.GetType());
			rale.Columns.Add("columna08",tipo_rale.GetType());
			rale.Columns.Add("columna09",tipo_rale.GetType());
			rale.Columns.Add("columna10",tipo_rale.GetType());
			rale.Columns.Add("columna11",tipo_rale.GetType());
			rale.Columns.Add("columna12",tipo_rale.GetType());
			rale.Columns.Add("columna13",tipo_rale.GetType());
			rale.Columns.Add("columna14",tipo_rale.GetType());
			
			rale_fecha.Columns.Add("columna01",tipo_rale.GetType());
			rale_fecha.Columns.Add("columna02",tipo_rale.GetType());
			
			/*tablarow3.Columns.Add("NRP", dde.GetType());
            tablarow3.Columns.Add("F#SUA", dde.GetType());
            tablarow3.Columns.Add("FECHA PAGO", dde.GetType());
            tablarow3.Columns.Add("4SS",sx.GetType());
            tablarow3.Columns.Add("RCV", sx.GetType());
            tablarow3.Columns.Add("PERIODO_PAGO", dde.GetType());
            tablarow3.Columns.Add("ARCHIVO_ORIGEN", dde.GetType());*/
            
           /* data_acumulador.Columns.Add("NRP", dde.GetType());
            data_acumulador.Columns.Add("F#SUA", dde.GetType());
            data_acumulador.Columns.Add("FECHA PAGO", dde.GetType());
            data_acumulador.Columns.Add("4SS",sx.GetType());
            data_acumulador.Columns.Add("RCV", sx.GetType());
            data_acumulador.Columns.Add("PERIODO_PAGO", dde.GetType());
            data_acumulador.Columns.Add("ARCHIVO_ORIGEN", dde.GetType());*/
		}
		//General_Pagos
		void Button2Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "Selecciona la carpeta que contiene los archivos de GENERAL DE PAGOS";
			fbd.ShowNewFolderButton =false;
			DialogResult result = fbd.ShowDialog();
			int ultimo_punto=0,fin_cadena=0,xy=0,tot_gp=0;
			String archivo_gp_x,ext_arch,tot_archs,archs_gp_tb;
			
			
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
				int carga_gp=0;
				
				if(archivos_gp.Length>0){
					DialogResult result1 = MessageBox.Show("Archivos Existentes En la Carpeta: " + archivos_tot.Length + "\nArchivos que se serán procesados: " +archivos_gp.Length+
					                                       "\n\nLos Datos de los Pagos serán extraídos de estos archivos: \n"+tot_archs+"\n\n¿Está seguro de querer cargar los archivos?",
					                                       "Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
					if(result1 == DialogResult.Yes){
						carga_gp=1;//carga_excel_general_pagos();
						pictureBox2.Visible=true;
						button2.Enabled=false;
						button3.Enabled=false;
						button4.Enabled=false;
						label2.Text="Carpeta GP: "+fbd.SelectedPath;
						toolTip1.SetToolTip(label2,fbd.SelectedPath);
						gp_load=1;
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
		//Procesar
		void Button3Click(object sender, EventArgs e)
		{
			String archivo,ext,cad_con;
			
			OpenFileDialog dialog = new OpenFileDialog();
			
			dialog.Title = "Seleccione el archivo PROCESAR";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque

			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				label2.Text="Procesar: "+archivo;
				toolTip1.SetToolTip(label2,archivo );
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();

				if((ext.Equals("xls"))||(ext.Equals("lsx"))){
					try{
						//esta cadena es para archivos excel 2007 y 2010
						cad_con= "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
						conexion2 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
						conexion2.Open(); //abrimos la conexion
						pictureBox3.Visible=true;
						button2.Enabled=false;
						button3.Enabled=false;
						button4.Enabled=false;
						pro_load=1;
						carga_chema_excel();
					}catch(Exception es){
						
					}
				}
				
			}
		}
		//SIPARE
		void Button4Click(object sender, EventArgs e)
		{
			String archivo,ext,cad_con;
			
			OpenFileDialog dialog = new OpenFileDialog();
			
			dialog.Title = "Seleccione el archivo SIPARE";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls, *.xlsx, *.csv)|*.xls;*.xlsx;*.csv";//le indicamos el tipo de filtro en este caso que busque

			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				label2.Text="SIPARE: "+archivo;
				toolTip1.SetToolTip(label2,archivo );
				ext=archivo.Substring(((archivo.Length)-3),3);
				ext=ext.ToLower();

				if((ext.Equals("xls"))||(ext.Equals("lsx"))){
					try{
						sip_tipo_load=2;
						//esta cadena es para archivos excel 2007 y 2010
						cad_con= "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
						conexion2 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
						conexion2.Open(); //abrimos la conexion
						
						pictureBox4.Visible=true;
						button2.Enabled=false;
						button3.Enabled=false;
						button4.Enabled=false;
						carga_chema_excel();
					}catch(Exception es){
						
					}
				}else{
					sip_tipo_load=1;
					archivo_sip=archivo;
					pictureBox4.Visible=true;
						button2.Enabled=false;
						button3.Enabled=false;
						button4.Enabled=false;
				}
				
			}
		}
		//continuar
		void Button5Click(object sender, EventArgs e)
		{
			panel1.Visible=true;
			tipo_rale=comboBox1.SelectedItem.ToString();
			
			//cargar_rale();
			button5.Visible=false;
			hilosecundario = new Thread(new ThreadStart(cargar_rale));
			hilosecundario.Start();
			//cargar_archivos();
			
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			//if((button1.Enabled==false)&&(button2.Enabled==false)&&(button3.Enabled==false)&&(button4.Enabled==false)&&(comboBox1.SelectedIndex>-1)&&(maskedTextBox1.Text.Length>5)){
			if((button1.Enabled==false)&&(comboBox1.SelectedIndex>-1)){
				button5.Enabled=true;
			}else{
				button5.Enabled=false;
			}
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		void Button7Click(object sender, EventArgs e)
		{
			panel2.Visible=false;
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			panel2.Visible=true;
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			Lector_rale_txt rale_txt = new Lector_rale_txt(tipo_inicio);
			rale_txt.ShowDialog();
			this.Hide();
			rale_txt.Focus();
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			panel3.Visible=false;	
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			Actualizador rale_vig = new Actualizador();
			rale_vig.Show();
			this.Hide();
			rale_vig.Focus();
		}
	}
}

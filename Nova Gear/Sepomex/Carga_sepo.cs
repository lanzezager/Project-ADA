/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 30/04/2018
 * Hora: 02:35 p. m.
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

namespace Nova_Gear.Sepomex
{
	/// <summary>
	/// Description of Carga_sepo.
	/// </summary>
	public partial class Carga_sepo : Form
	{
		public Carga_sepo()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		
		DataTable consulta = new DataTable();
		DataTable archivo_xls = new DataTable();
		DataTable lista_tablas_xls = new DataTable();
		DataTable consulta_ema = new DataTable();
		DataTable consulta_datos_ema = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		
		
		public void llenar_Cb2()
        {
			int i=0;
            conex2.conectar("ema");
            comboBox2.Items.Clear();
            i = 0;
            consulta_ema = conex2.consultar("SHOW TABLES FROM ema ");
            i=(consulta_ema.Rows.Count)-1;
            while(i>-1){
            	comboBox2.Items.Add(consulta_ema.Rows[i][0].ToString());
                i--;
            }
            i = 0;
            //MessageBox.Show(dataGridView2.Rows[0].Cells[0].Value.ToString());
            conex2.cerrar();
        }
		
		public void carga_chema_excel(){
			
			int filas = 0,i=0;
			String tabla;
			
			//lista_tablas_xls.Clear();
			comboBox1.Items.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			filas=dt.Rows.Count;
			//lista_tablas_xls.Columns.Add();
			
			while(i<filas){
				if (!(dt.Rows[i][3].ToString()).Equals("")){
					if ((dt.Rows[i][3].ToString()).Equals("TABLE")){
						tabla=dt.Rows[i][2].ToString();
						if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
							tabla = tabla.Remove((tabla.Length-1),1);
							//lista_tablas_xls.Rows.Add(tabla);
							comboBox1.Items.Add(tabla);
						}
					}
				}
				i++;
			}
			
			dt.Clear();
			i=0;
		}
		
		public void cargar_hoja_excel_procesar(){
			String cons_exc,hoja;
			
			hoja=comboBox1.SelectedItem.ToString();
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
							archivo_xls=dataSet.Tables[0];
							//dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
							//data_acumulador.Merge(dataSet.Tables[0]);
							//conexion.Close();//cerramos la conexion
							//dataGridView2.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
							dataGridView1.DataSource=archivo_xls;
							label4.Text="N° Créditos: "+dataGridView1.Rows.Count;
							label4.Refresh();
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
		
		public void importar_sepo(){
			String per,ra_soc;
			int i=0,ingresados=0,ind;
			conex.conectar("base_principal");
			if(dataGridView1.RowCount>0 && dataGridView1.ColumnCount>0 ){
				per="20"+dataGridView1.Rows[0].Cells[1].Value.ToString();
				DialogResult res = MessageBox.Show("Está por ingresar el archivo SEPOMEX de la E.M.A. del Período: "+per+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				
				if(res == DialogResult.Yes){
					try{
						while(i<dataGridView1.RowCount){
							if(dataGridView1.Rows[i].Cells[16].Value.ToString().Equals("D") ||dataGridView1.Rows[i].Cells[16].Value.ToString().Equals("d")){
								while(dataGridView1.Rows[i].Cells[10].Value.ToString().Contains("\"")){
									dataGridView1.Rows[i].Cells[10].Value=dataGridView1.Rows[i].Cells[10].Value.ToString().Replace('"','\'');
								}
								conex.consultar("INSERT INTO ema_sepomex (registro_patronal,razon_social,credito_ema,folio,periodo_ema,status) VALUES \n" +
							                "(\""+dataGridView1.Rows[i].Cells[7].Value.ToString()+"\",\""+dataGridView1.Rows[i].Cells[10].Value.ToString()+"\",\""+dataGridView1.Rows[i].Cells[9].Value.ToString()+"\",\""+dataGridView1.Rows[i].Cells[8].Value.ToString()+"\",\"EMA_20"+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\",\"0\")");
								ingresados++;
							}
							
							i++;
						}
						
						MessageBox.Show("Se Ingresaron Correctamente "+ingresados+" créditos del archivo de la E.M.A. de SEPOMEX","EXITO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
						conex.guardar_evento("Se ingresaron: "+ingresados+" créditos del periodo: "+dataGridView1.Rows[0].Cells[9].Value.ToString()+" de la EMA SEPOMEX");
					}catch(Exception ed){
						MessageBox.Show("Ocurrió el siguiente error al tratar de ingresar el archivo de la E.M.A. de SEPOMEX: "+ed.ToString(),"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}
			}
		}
		
		public void importa_ema(String reg_pat){
			
			if(comboBox2.SelectedIndex>-1 && reg_pat.Length==10){
								
				conex2.conectar("ema");
				consulta_datos_ema=conex2.consultar("SELECT razon_social,num_credito,periodo FROM "+comboBox2.SelectedItem.ToString()+" WHERE reg_pat=\""+reg_pat+"\" ");
				
				if(consulta_datos_ema.Rows.Count>0){
					if(consulta_datos_ema.Rows[0][1].ToString().Length>9){
						dataGridView2.Rows.Add(reg_pat.ToUpper(),consulta_datos_ema.Rows[0][0].ToString(),consulta_datos_ema.Rows[0][1].ToString().Substring(1,9),consulta_datos_ema.Rows[0][2].ToString());
					}else{
						dataGridView2.Rows.Add(reg_pat.ToUpper(),consulta_datos_ema.Rows[0][0].ToString(),consulta_datos_ema.Rows[0][1].ToString(),consulta_datos_ema.Rows[0][2].ToString());
					}
					maskedTextBox1.Clear();
					maskedTextBox1.Focus();
					label5.Text="N° Créditos: "+dataGridView2.Rows.Count;
					label5.Refresh();
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			String cad_con;
			OpenFileDialog dialog = new OpenFileDialog();
		
			dialog.Title = "Seleccione el archivo de la E.M.A. de SEPOMEX";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			archivo_xls.Clear();
			
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dialog.FileName + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion
				carga_chema_excel();
				textBox1.Text=dialog.SafeFileName;
			}
			
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex > -1){
				button2.Enabled=true;
			}else{
				button2.Enabled=false;
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			cargar_hoja_excel_procesar();			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			importar_sepo();
		}
		
		void Carga_sepoLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llenar_Cb2();
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			String reg_pat="";
			if(maskedTextBox1.MaskCompleted==true){
				reg_pat=maskedTextBox1.Text;
				reg_pat=reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
				importa_ema(reg_pat);
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedIndex>-1){
				maskedTextBox1.Focus();
			}
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.MaskCompleted==true){
				button4.Focus();
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			int i=0,ee=0;
			String per_ingreso="";
			if(dataGridView2.RowCount>0){
				try{
					DialogResult res = MessageBox.Show("Está por ingresar los "+dataGridView2.RowCount+" créditos listados en la Tabla de la E.M.A\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
					
					if(res == DialogResult.Yes){
						conex.conectar("base_principal");
						while(i<dataGridView2.RowCount){
							if(radioButton1.Checked==true){
								per_ingreso="EMA_"+dataGridView2.Rows[i].Cells[3].Value.ToString();
							}
							
							if(radioButton2.Checked==true){
								per_ingreso="PO_"+dataGridView2.Rows[i].Cells[3].Value.ToString();
							}
							
							conex.consultar("INSERT INTO ema_sepomex (registro_patronal,razon_social,credito_ema,folio,periodo_ema,status) VALUES(\""+dataGridView2.Rows[i].Cells[0].Value.ToString()+"\",\""+dataGridView2.Rows[i].Cells[1].Value.ToString()+"\",\""+dataGridView2.Rows[i].Cells[2].Value.ToString()+"\",\"-\",\""+per_ingreso+"\",\"0\")");
							i++;
						}
						if(ee==0){
							MessageBox.Show("Se ingresaron todos los créditos Correctamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
							conex.guardar_evento("Se Ingresaron "+dataGridView2.RowCount+" creditos individuales a la EMA_SEPOMEX");
							dataGridView2.DataSource=null;
							comboBox2.SelectedIndex=-1;
							maskedTextBox1.Clear();
							label5.Text="N° Créditos: 0";
							label5.Refresh();
							
						}
					}
				}catch(Exception ep){
					MessageBox.Show("Ocurrió el siguiente error al intentar ingresar los créditos: \n"+ep.ToString(),"Exito",MessageBoxButtons.OK,MessageBoxIcon.Error);
					ee=1;
				}
			}
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.MaskCompleted==true){
				maskedTextBox3.Focus();
			}
		}
		
		void MaskedTextBox3TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox3.MaskCompleted==true){
				maskedTextBox4.Focus();
			}
		}
		
		void MaskedTextBox4TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox4.MaskCompleted==true){
				textBox2.Focus();
			}
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				maskedTextBox5.Focus();
			}
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			if(textBox2.Text.Length==100){
				maskedTextBox5.Focus();
			}
		}
		
		void MaskedTextBox5TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox5.MaskCompleted==true){
				button6.Focus();
			}
		}
		
		void MaskedTextBox5KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				if(maskedTextBox5.Text.Length==1){
					maskedTextBox5.Text="0"+maskedTextBox5.Text;
				}
				button6.Focus();
			}
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			int next=0,ee=0;
			String err="",rp,cred,per,ra_soc,sector;
			
			if(maskedTextBox2.MaskCompleted==true){
				next++;					
			}else{
				err+="•Registro Patronal\n";
			}
			
			if(maskedTextBox3.MaskCompleted==true){
				next++;	
			}else{
				err+="•Crédito\n";
			}
			
			if(maskedTextBox4.MaskCompleted==true){
				next++;	
			}else{
				err+="•Periodo\n";
			}		
			
			if(textBox2.Text.Length>9){
				next++;
			}else{
				err+="•Razón Social\n";
			}
			
			if(maskedTextBox5.MaskCompleted==true){
				next++;	
			}else{
				err+="•Sector de Notificación\n";
			}
			
			if(next==5){
				try{
					DialogResult res = MessageBox.Show("Está por ingresar el crédito: "+maskedTextBox3.Text+"  en la Tabla de la E.M.A\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
					
					if(res == DialogResult.Yes){
						conex.conectar("base_principal");
						
						rp=maskedTextBox2.Text.ToUpper();
						cred=maskedTextBox3.Text;
						per=maskedTextBox4.Text;
						ra_soc=textBox2.Text.ToUpper();
						sector=maskedTextBox5.Text;
						
						rp=rp.Substring(0,3)+rp.Substring(4,5)+rp.Substring(10,2);
						per=per.Substring(0,4)+per.Substring(5,2);
						                                  
						conex.consultar("INSERT INTO ema_sepomex (registro_patronal,razon_social,credito_ema,folio,periodo_ema,status,sector_notificacion) VALUES(\""+rp+"\",\""+ra_soc+"\",\""+cred+"\",\"-\",\"EMA_"+per+"\",\"0\",\""+sector+"\")");
							
						if(ee==0){
							MessageBox.Show("Se ingresaron todos los créditos Correctamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
							conex.guardar_evento("Se Ingresó el credito: "+maskedTextBox3.Text+" de forma manual a la EMA_SEPOMEX");
							maskedTextBox2.Clear();
							maskedTextBox3.Clear();
							maskedTextBox4.Clear();
							maskedTextBox5.Clear();
							textBox2.Text="";
							maskedTextBox2.Focus();
							
						}
					}
				}catch(Exception ec){
					MessageBox.Show("Ocurrió el siguiente error al intentar ingresar los créditos: \n"+ec.ToString(),"Exito",MessageBoxButtons.OK,MessageBoxIcon.Error);
					ee=1;
				}
			}else{
				MessageBox.Show("La siguiente informacion no se ingresó:\n"+err,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}
		
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(tabControl1.SelectedIndex==0){
				button1.Focus();
			}
			
			if(tabControl1.SelectedIndex==1){
				comboBox2.Focus();
			}
			
			if(tabControl1.SelectedIndex==2){
				maskedTextBox2.Focus();
			}
		}
		//ingresar desde excel universal
		void Button7Click(object sender, EventArgs e)
		{
			if(comboBox2.SelectedIndex>-1){
				String cad_con;
				int i=0;
				OpenFileDialog dialog = new OpenFileDialog();
				
				dialog.Title = "Seleccione el archivo de Excel a Cargar";//le damos un titulo a la ventana
				dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
				
				dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
				
				archivo_xls.Clear();
				
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dialog.FileName + "';Extended Properties=Excel 12.0;";
					conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
					conexion.Open(); //abrimos la conexion
					carga_chema_excel();
					comboBox1.SelectedIndex=0;
					cargar_hoja_excel_procesar();
					
					while(i<dataGridView1.RowCount){
						importa_ema(dataGridView1.Rows[i].Cells[0].Value.ToString());
						i++;
					}
					
					comboBox1.Items.Clear();
					dataGridView1.DataSource=null;
					
					if(dataGridView2.RowCount>0){
						MessageBox.Show("Se Leyeron "+dataGridView2.RowCount+" Registros de la EMA" ,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
					}else{
						MessageBox.Show("No se pudo importar ningún registro de la EMA" ,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
				}
			}
		}
	}
}

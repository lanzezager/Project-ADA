/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 24/05/2018
 * Hora: 09:28 a. m.
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
	/// Description of Sectorizacion_sepo.
	/// </summary>
	public partial class Sectorizacion_sepo : Form
	{
		public Sectorizacion_sepo()
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
		
		DataTable consulta = new DataTable();
		DataTable archivo_xls = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		
		public void llena_cb2(){
			int i=0;
			conex.conectar("base_principal");
			consulta=conex.consultar("SELECT DISTINCT(periodo_ema) FROM ema_sepomex ORDER BY periodo_ema DESC");
			
			while(i<consulta.Rows.Count){
				comboBox2.Items.Add(consulta.Rows[i][0].ToString());
				i++;
			}
			conex.cerrar();
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
		
		void Button2Click(object sender, EventArgs e)
		{
			MessageBox.Show("La hoja a leer debe tener en la primera columna el sector a colocar y en la segunda columna el registro patronal de 10 dígitos al cual se le actualizará el sector,ambas columnas con una fila de encabezado.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			cargar_hoja_excel_procesar();
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex > -1){
				button2.Enabled=true;
			}else{
				button2.Enabled=false;
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			String per="";
			int i=0,err=0;
			
			if(dataGridView1.RowCount>0 && comboBox2.SelectedIndex > -1){
				per=comboBox2.SelectedItem.ToString();
				DialogResult res = MessageBox.Show("Está por Actualizar los sectores de "+dataGridView1.RowCount+" Registros Patronales de la E.M.A. del Período: "+per+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				
				if(res == DialogResult.Yes){
					try{
						conex.conectar("base_principal");
						
						while(i<dataGridView1.RowCount){
							conex.consultar("UPDATE ema_sepomex SET sector_notificacion=\""+dataGridView1.Rows[i].Cells[0].Value.ToString()+"\" WHERE registro_patronal=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" and periodo_ema= \""+per+"\" ");
							i++;
						}
						
						if(err==0){
							MessageBox.Show("Se Actualizaron Correctamente los sectores de "+i+" créditos del periodo "+per+" de la E.M.A.","EXITO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
							conex.guardar_evento("Se Actualizaron Correctamente los sectores de "+i+" créditos del periodo "+per+" de la EMA_SEPOMEX");
							dataGridView1.DataSource=null;
							
							comboBox1.Items.Clear();
							comboBox2.SelectedIndex=-1;
							textBox1.Text="";
							conex.cerrar();
							conexion.Close();
						}
						
						
						
					}catch(Exception ef){
						err=1;
						MessageBox.Show("Ocurrió el siguiente error al tratar de ingresar el archivo de la E.M.A. de SEPOMEX: "+ef.ToString(),"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}
			}
			
		}
		
		void Sectorizacion_sepoLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llena_cb2();
		}
		
		void Sectorizacion_sepoFormClosing(object sender, FormClosingEventArgs e)
		{
			try{
				conexion.Close();
			}catch{}
		}
	}
}

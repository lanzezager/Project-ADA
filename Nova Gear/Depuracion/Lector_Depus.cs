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
    public partial class Lector_Depus : Form
    {
        public Lector_Depus()
        {
            InitializeComponent();
        }

		String archivo = "", ext = "", cad_con = "",tabla="",hoja="", cons_exc="";
		int i = 0, filas = 0, tipo_ced=0;

		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;

		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();

		private void label8_Click(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count>0) {

				String tipo_fac = "";

				switch(tipo_ced)
				{
					case 1: tipo_fac = "\nEsto borrará el archivo temporal de la Depuración";
						break;
					case 2:	tipo_fac = "\nEsto borrará el archivo temporal de la Depuración";
						break;
					case 3:	tipo_fac = "";
						break;
					default: tipo_fac = "";
						break;
				}

				DialogResult result1 = MessageBox.Show("Se procederá a Crear la Cédula de Captura."+tipo_fac+"  \n\n¿Desea continuar?",
													   "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
				if (result1 == DialogResult.Yes)
				{
					String jefe_sec, jefe_ofi;
					jefe_ofi = conex.leer_config_sub()[8];
					jefe_sec = conex.leer_config_sub()[9];
					String solicita = jefe_sec + "\nOficina de Emisiones P.O.", autoriza = jefe_ofi + "\nJefe Oficina de Emisiones y P.O.";

					DataTable data = (DataTable)(dataGridView1.DataSource);

					if (tipo_ced == 1)
					{
						conexion.Close();
						conexion.Dispose();
						System.IO.File.Delete(@"temp\depuracion_final_COP.xlsx");
					}

					if (tipo_ced == 2)
					{
						conexion.Close();
						conexion.Dispose();
						System.IO.File.Delete(@"temp\depuracion_final_RCV.xlsx");
					}

					Visor_Reporte visor1 = new Visor_Reporte();
					visor1.envio(data, solicita, autoriza);
					visor1.Show();					
				}
			}
			else
			{
				MessageBox.Show("Sin Datos a Enviar","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton2.Checked == true)
			{
				archivo = @"temp\depuracion_final_RCV.xlsx";

				if ((System.IO.File.Exists(archivo)) == true)
				{
					//esta cadena es para archivos excel 2007 y 2010
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
					conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
					conexion.Open(); //abrimos la conexion

					cons_exc = "Select * from [hoja_lz$] ";//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet

					if (dataAdapter.Equals(null))
					{ }
					else
					{
						dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
						dataGridView1.DataSource = dataSet.Tables[0];
						label2.Text = "depuracion_final_RCV.xlsx";
						label8.Text = "Casos: "+dataGridView1.RowCount;
						tipo_ced = 2;
					}
				}
			}
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{ 
			if (radioButton1.Checked==true) { 
				archivo = @"temp\depuracion_final_COP.xlsx";

				if ((System.IO.File.Exists(archivo)) == true)
				{
					//esta cadena es para archivos excel 2007 y 2010
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
					conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
					conexion.Open(); //abrimos la conexion

					cons_exc = "Select * from [hoja_lz$] ";//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet

					if (dataAdapter.Equals(null))
					{ }
					else
					{
						dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
						dataGridView1.DataSource = dataSet.Tables[0];
						label2.Text = "depuracion_final_COP.xlsx";
						label8.Text = "Casos: " + dataGridView1.RowCount;
						tipo_ced = 1;
					}
				}
			}
		}

		public void carga_excel()
		{
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
				ext = archivo.Substring(((archivo.Length) - 3), 3);
				ext = ext.ToLower();

				if (ext.Equals("lsx"))
				{
					MessageBox.Show("Asegurate de Cerrar el archivo en Excel, Antes de abrirlo aqui", "Advertencia");
				}

				//esta cadena es para archivos excel 2007 y 2010
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion

				carga_chema_excel();

			}
		}

		public void carga_chema_excel()
		{
			i = 0;
			filas = 0;
			comboBox1.Items.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			dataGridView2.DataSource = dt;
			filas = (dataGridView2.RowCount) - 2;
			do
			{
				if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals(""))
				{
					if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE"))
					{
						tabla = dataGridView2.Rows[i].Cells[2].Value.ToString();
						if ((tabla.Substring((tabla.Length - 1), 1)).Equals("$"))
						{
							tabla = tabla.Remove((tabla.Length - 1), 1);
							comboBox1.Items.Add(tabla);
						}
					}
				}
				i++;
			} while (i <= filas);

			dt.Clear();
			dataGridView2.DataSource = dt; //vaciar datagrid
		}

		public void cargar_hoja_excel()
		{			
			hoja = comboBox1.SelectedItem.ToString();
			
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select * from [" + hoja + "$] ";				
				
				try{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					if (dataAdapter.Equals(null))
					{
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");
					}
					else
					{
						dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
						dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						conexion.Close();//cerramos la conexion
						dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
					    label8.Text = "Casos: " + dataGridView1.RowCount;
						label8.Refresh();
						tipo_ced = 3;
						//button3.Enabled = false;

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
				catch (AccessViolationException ex)
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
				}
			}
		}

		private void Lector_Depus_Load(object sender, EventArgs e)
        {
			archivo = @"temp\depuracion_final_COP.xlsx";

            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			if ((System.IO.File.Exists(archivo)) == true)
			{
				//esta cadena es para archivos excel 2007 y 2010
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion

				cons_exc = "Select * from [hoja_lz$] ";//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
				dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
				dataSet = new DataSet(); // creamos la instancia del objeto DataSet

				if (dataAdapter.Equals(null))
				{ }
				else
				{
					dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
					dataGridView1.DataSource = dataSet.Tables[0];
					label2.Text = "depuracion_final_COP.xlsx";
					label8.Text = "Casos: " + dataGridView1.RowCount;
					tipo_ced = 1;
				}
			}
		}

        private void button2_Click(object sender, EventArgs e)
        {
			if (comboBox1.SelectedItem == null)
			{
				MessageBox.Show("Selecciona una Hoja");
			}
			else
			{
				cargar_hoja_excel();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			carga_excel();
            dataGridView1.DataSource=null;
            tipo_ced = 3;
            label8.Text = "Casos: 0";
		}
    
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 23/04/2018
 * Hora: 03:45 p. m.
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

namespace Nova_Gear.Universal
{
	/// <summary>
	/// Description of Entrega_especial.
	/// </summary>
	public partial class Entrega_especial : Form
	{
		public Entrega_especial()
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
		DataTable lista_tablas_xls = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
		
		int not_found=0;
		
		public void buscar(){
			String rp,per;
			int i=0;
			
			if((maskedTextBox1.MaskCompleted)&&(maskedTextBox2.MaskCompleted)&&(maskedTextBox3.MaskCompleted)){
				rp=maskedTextBox1.Text;
				rp=rp.Substring(0,3)+rp.Substring(4,5)+rp.Substring(10,2);
				per=maskedTextBox3.Text;
				per=per.Substring(0,4)+per.Substring(5,2);
				
				consulta=conex.consultar("SELECT nombre_periodo,registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,periodo,sector_notificacion_inicial,sector_notificacion_actualizado,controlador,notificador, "+
										"status,observaciones,id FROM datos_factura WHERE registro_patronal2 =\""+rp+"\" and credito_cuotas=\""+maskedTextBox2.Text+"\" and periodo=\""+per+"\"");
				
				if(consulta.Rows.Count>0){
					dataGridView1.Rows.Add(consulta.Rows[0][0].ToString(),
					                       consulta.Rows[0][1].ToString(),
					                       consulta.Rows[0][2].ToString(),
					                       consulta.Rows[0][3].ToString(),
					                       consulta.Rows[0][4].ToString(),
					                       consulta.Rows[0][5].ToString(),
					                       consulta.Rows[0][6].ToString(),
					                       consulta.Rows[0][7].ToString(),
					                       consulta.Rows[0][8].ToString(),
					                       consulta.Rows[0][9].ToString(),
					                       consulta.Rows[0][10].ToString(),
					                       consulta.Rows[0][11].ToString(),
					                       consulta.Rows[0][12].ToString(),
					                       consulta.Rows[0][13].ToString(),
					                       consulta.Rows[0][14].ToString());
					maskedTextBox1.Clear();
					maskedTextBox2.Clear();
					maskedTextBox3.Clear();
					maskedTextBox1.Focus();
					label5.Text="Total: "+dataGridView1.RowCount;
					label5.Refresh();
				}
			}
			
		}
		
		public void buscar_xls(String rp, String cred, String per){
			
			int i=0;
			
			//if((maskedTextBox1.MaskCompleted)&&(maskedTextBox2.MaskCompleted)&&(maskedTextBox3.MaskCompleted)){
				
				consulta=conex.consultar("SELECT nombre_periodo,registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,periodo,sector_notificacion_inicial,sector_notificacion_actualizado,controlador,notificador, "+
										"status,observaciones,id FROM datos_factura WHERE registro_patronal2 =\""+rp+"\" and credito_cuotas=\""+cred+"\" and periodo=\""+per+"\"");
				
				if(consulta.Rows.Count>0){
					dataGridView1.Rows.Add(consulta.Rows[0][0].ToString(),
					                       consulta.Rows[0][1].ToString(),
					                       consulta.Rows[0][2].ToString(),
					                       consulta.Rows[0][3].ToString(),
					                       consulta.Rows[0][4].ToString(),
					                       consulta.Rows[0][5].ToString(),
					                       consulta.Rows[0][6].ToString(),
					                       consulta.Rows[0][7].ToString(),
					                       consulta.Rows[0][8].ToString(),
					                       consulta.Rows[0][9].ToString(),
					                       consulta.Rows[0][10].ToString(),
					                       consulta.Rows[0][11].ToString(),
					                       consulta.Rows[0][12].ToString(),
					                       consulta.Rows[0][13].ToString(),
					                       consulta.Rows[0][14].ToString());
			}else{
				not_found++;
			}
			//}
			
		}
		
		public void carga_chema_excel(){
			
			int filas = 0,i=0;
			String tabla;
			
			lista_tablas_xls.Clear();
			System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			filas=dt.Rows.Count;
			lista_tablas_xls.Columns.Add();
			
			while(i<filas){
				if (!(dt.Rows[i][3].ToString()).Equals("")){
					if ((dt.Rows[i][3].ToString()).Equals("TABLE")){
						tabla=dt.Rows[i][2].ToString();
						if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
							tabla = tabla.Remove((tabla.Length-1),1);
							lista_tablas_xls.Rows.Add(tabla);
						}
					}
				}
				i++;
			}
			
			dt.Clear();
			i=0;
		}
		
		public void cargar_hoja_excel_procesar(){
			String cons_exc;
				
			if (string.IsNullOrEmpty(lista_tablas_xls.Rows[0][0].ToString()))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				cons_exc = "Select * From [" + lista_tablas_xls.Rows[0][0].ToString() + "$]";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
			
					if(dataAdapter.Equals(null)){
						
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						
					}else{
						if (dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, lista_tablas_xls.Rows[0][0].ToString());//llenamos el dataset
							archivo_xls=dataSet.Tables[0];
						//dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						//data_acumulador.Merge(dataSet.Tables[0]);
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
		
		public  void llena_cb1(){
			int i=0;
			consulta=conex.consultar("SELECT DISTINCT(periodo_factura) FROM datos_factura WHERE periodo_factura <> \"-\" ORDER BY periodo_factura");
			while(i<consulta.Rows.Count){
                if (consulta.Rows[i][0].ToString().Length>1)
                {
				    comboBox1.Items.Add(consulta.Rows[i][0].ToString());
                }
				i++;
			}
		}
		
		void GroupBox1Enter(object sender, EventArgs e)
		{
	
		}
		
		void Entrega_especialLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			conex.conectar("base_principal");
			llena_cb1();
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.MaskCompleted){
				maskedTextBox2.Focus();
			}
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.MaskCompleted){
				maskedTextBox3.Focus();
			}
		}
		
		void MaskedTextBox3TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox3.MaskCompleted){
				button2.Focus();
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			buscar();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			int j=0;
			String cad_con;
			OpenFileDialog dialog = new OpenFileDialog();
		
			dialog.Title = "Seleccione el archivo de Actualización de Sectores";//le damos un titulo a la ventana
			dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			archivo_xls.Clear();
			
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dialog.FileName + "';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion
				carga_chema_excel();
				cargar_hoja_excel_procesar();
				
				while(j<archivo_xls.Rows.Count){
					buscar_xls(archivo_xls.Rows[j][0].ToString(),archivo_xls.Rows[j][1].ToString(),archivo_xls.Rows[j][2].ToString());
					j++;
				}
				
				if(not_found>0){
					MessageBox.Show(not_found+" Créditos no Fueron encontrados en la Base de Datos.","AVISO");
				}
				
				label5.Text="Total: "+dataGridView1.RowCount;
				label5.Refresh();
				not_found=0;
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			int c=0;
            int ind_esp = 0,mayor=0;

			if(comboBox1.Items.Count==0){
				comboBox1.Items.Add("ESPECIAL_00");
			}else{
                for (int j = 0; j < comboBox1.Items.Count;j++)
                {
                    ind_esp = comboBox1.Items[j].ToString().IndexOf('_');
                    c = Convert.ToInt32(comboBox1.Items[j].ToString().Substring(ind_esp + 1, (comboBox1.Items[j].ToString().Length - (ind_esp + 1))));
                    if(c>mayor){
                        mayor = c;
                    }
                }
                c = mayor;
                //c=Convert.ToInt32(comboBox1.Items[comboBox1.Items.Count-1].ToString().Substring(comboBox1.Items[comboBox1.Items.Count-1].ToString().Length-2,2));
				c++;
				comboBox1.Items.Add("ESPECIAL_"+c);
				comboBox1.SelectedItem="ESPECIAL_"+c;
				button4.Enabled=false;
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			int can=0,i=0;
			String error="",modifs="";
			if(dataGridView1.RowCount>0){
				can++;
			}else{
				error="•Ingresa algún crédito a modificar";
			}
			
			if(comboBox1.SelectedIndex>-1){
				can++;
			}else{
				error="•Selecciona un Periodo de Entrega Válido";
			}
			
			if(can==2){
				DialogResult re=MessageBox.Show("Los "+dataGridView1.RowCount+" créditos listados se van a anotar en el periodo de apoyo: "+comboBox1.SelectedItem.ToString()+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				
				if(re == DialogResult.Yes){
					while(i<dataGridView1.RowCount){
						modifs+=dataGridView1.Rows[i].Cells[14].Value.ToString()+",";
						i++;
					}
					
					modifs=modifs.Substring(0,(modifs.Length-1));
					
					conex.consultar("UPDATE datos_factura SET periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\", status=\"0\", fecha_entrega=null WHERE id IN("+modifs+")");
					MessageBox.Show("Se guardaron exitosamente "+i+" créditos en la factura: "+comboBox1.SelectedItem.ToString(),"EXITO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
					conex.guardar_evento("Se guardaron exitosamente "+i+" créditos en la factura: "+comboBox1.SelectedItem.ToString());
					dataGridView1.Rows.Clear();
					label5.Text="Total: "+dataGridView1.RowCount;
					label5.Refresh();
				}
				
			}else{
				MessageBox.Show("No se puede continuar por el (los) siguiente(s) motivo(s):\n"+error,"ERROR");
			}
		}
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 27/06/2018
 * Hora: 04:34 p. m.
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
	/// Description of DepuRB.
	/// </summary>
	public partial class DepuRB : Form
	{
		public DepuRB()
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
		DataTable consultamysql = new DataTable();
		DataTable lista_dep = new DataTable();
		
		int salir=0;
		
		void DepuRBLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			//RB SISCOB
			conex.conectar("base_principal");
						
			lista_dep.Columns.Add("NRP");
			lista_dep.Columns.Add("CREDITO CUOTA");
			lista_dep.Columns.Add("CREDITO MULTA");
			lista_dep.Columns.Add("ID");
			
			dataGridView1.Columns.Add("reg","REGISTRO PATRONAL");
			dataGridView1.Columns.Add("ra_soc","RAZÓN SOCIAL");
			dataGridView1.Columns.Add("per","PERIODO");
			dataGridView1.Columns.Add("cred_cuo","CRÉDITO CUOTA");
			dataGridView1.Columns.Add("cred_mul","CRÉDITO MULTA");
			dataGridView1.Columns.Add("imp_cuo","IMPORTE CUOTA");
			dataGridView1.Columns.Add("imp_mul","IMPORTE MULTA");
			dataGridView1.Columns.Add("status","STATUS");
			dataGridView1.Columns.Add("id","id");
			dataGridView1.Columns[8].Visible=false;
			dataGridView1.Columns[1].MinimumWidth=300;
			try{
				System.IO.File.Delete(@"temporal_datos_depu.xlsx");
			}catch{}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			String sql="",reg="",cred="",tipo_cred="";
			int i=0;
			
			if(maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted){
				reg=maskedTextBox2.Text;
				cred=maskedTextBox1.Text;
				reg=reg.Substring(0,3)+reg.Substring(6,5)+reg.Substring(14,2);
				
				if(radioButton1.Checked){
					tipo_cred="credito_cuotas";
				}else{
					tipo_cred="credito_multa";
				}
				
				sql="SELECT registro_patronal,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,status,id FROM datos_factura WHERE " +
					"registro_patronal2 =\""+reg+"\" and "+tipo_cred+"=\""+cred+"\" ";
				consultamysql=conex.consultar(sql);
				
				while(i<consultamysql.Rows.Count){
					dataGridView1.Rows.Add(consultamysql.Rows[i][0].ToString(),
					                       consultamysql.Rows[i][1].ToString(),
					                       consultamysql.Rows[i][2].ToString(),
					                       consultamysql.Rows[i][3].ToString(),
					                       consultamysql.Rows[i][4].ToString(),
					                       consultamysql.Rows[i][5].ToString(),
					                       consultamysql.Rows[i][6].ToString(),
					                       consultamysql.Rows[i][7].ToString(),
					                       consultamysql.Rows[i][8].ToString());
					i++;
				}
				
				//label2.Text="Registros Totales: "+dataGridView1.RowCount;
				maskedTextBox1.Clear();
				maskedTextBox2.Clear();
				maskedTextBox2.Focus();
				timer1.Start();
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			int i=0;
			String reg_pat,cred_cuo,cred_mul,id;
			if(dataGridView1.RowCount>0){
				DialogResult resu = MessageBox.Show("Se Depurarán un total de: "+dataGridView1.RowCount+" Elementos\n\n¿Desea Continuar?", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				
				if(resu==DialogResult.Yes){
					lista_dep.Rows.Clear();
					try{
						System.IO.File.Delete(@"temporal_datos_depu.xlsx");
					}catch{}
					
					while(i<dataGridView1.RowCount){
						reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
						
						reg_pat = reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
						cred_cuo=dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
						cred_mul=dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
						id=dataGridView1.Rows[i].Cells[8].FormattedValue.ToString();
						
						lista_dep.Rows.Add(reg_pat,cred_cuo,cred_mul,id);
						i++;
					}
									
					XLWorkbook wb = new XLWorkbook();
					wb.Worksheets.Add(lista_dep, "hoja_lz");
					wb.SaveAs(@"temporal_datos_depu.xlsx");
					salir = 1;
					this.Close();
				}
			}
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.MaskCompleted){
				maskedTextBox1.Focus();
			}
		}
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.MaskCompleted){
				button4.Focus();
			}
		}
		void DepuRBFormClosing(object sender, FormClosingEventArgs e)
		{
			if(salir==0){

					DialogResult respuesta = MessageBox.Show("Si cierra la ventana sin dar click a la opción de DEPURAR no se podrá realizar ninguna depuración.\n\n¿Desea Salir?", "ATENCIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

					if (respuesta == DialogResult.Yes)
					{
						e.Cancel = false;
					}
					else
					{
						e.Cancel = true;
					}
				}
		}
		
		void Label4Click(object sender, EventArgs e)
		{
			
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				label2.Text="Registros Totales: "+dataGridView1.RowCount;
				label2.Refresh();
			}
		}
	}
} 

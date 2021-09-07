/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 13/07/2018
 * Hora: 03:13 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Inventario.Cartera
{
	/// <summary>
	/// Description of Vigs.
	/// </summary>
	public partial class Vigs : Form
	{
		public Vigs()
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
		DataTable no_vig = new DataTable();
		
		int i_perma=0;
		
		public void buscar_cred(){
			int i=0;
			
			if(maskedTextBox1.MaskCompleted==true){
				if(dataGridView1.RowCount>0){
					i=i_perma+1;
					while(i<dataGridView1.RowCount){
						if(dataGridView1.Rows[i].Cells[1].Value.ToString().Equals(maskedTextBox1.Text)==true){
							dataGridView1.ClearSelection();
        					dataGridView1.Rows[i].Cells[0].Selected=true;
        					dataGridView1.FirstDisplayedScrollingRowIndex=i;	
        					i_perma=i;
        					i=dataGridView1.RowCount+1;
						}
						i++;
					}
					
				}
			}
		}
		
		void VigsLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			conex.conectar("cartera_inv");
			no_vig=conex.consultar("SELECT reg_pat,credito,periodo,incidencia,fecha_incidencia,mov,fecha_mov,sector,ce,tipo_doc,fecha_alta,fecha_not,dias,importe,clase_doc,fecha_rale FROM cartera_inv_base WHERE status=\"VIGENTE\"");
			
			no_vig.Columns[0].ColumnName="REGISTRO PATRONAL";
			no_vig.Columns[1].ColumnName="CRÉDITO";
			no_vig.Columns[2].ColumnName="PERIODO";
			no_vig.Columns[3].ColumnName="INCIDENCIA";
			no_vig.Columns[4].ColumnName="FECHA INCIDENCIA";
			no_vig.Columns[5].ColumnName="MOVIMIENTO";
			no_vig.Columns[6].ColumnName="FECHA MOVIMIENTO";
			no_vig.Columns[7].ColumnName="SECTOR";
			no_vig.Columns[8].ColumnName="CE";
			no_vig.Columns[9].ColumnName="TIPO DOCUMENTO";
			no_vig.Columns[10].ColumnName="FECHA ALTA";
			no_vig.Columns[11].ColumnName="FECHA NOTIFICACION";
			no_vig.Columns[12].ColumnName="DIAS";
			no_vig.Columns[13].ColumnName="IMPORTE";
			no_vig.Columns[14].ColumnName="CLASE DOCUMENTO";
			no_vig.Columns[15].ColumnName="FECHA RALE";
			
			dataGridView1.DataSource=no_vig;
			label3.Text="Casos: "+dataGridView1.RowCount;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			buscar_cred();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog_save = new SaveFileDialog();
			dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
			dialog_save.Title = "Guardar Activos";//le damos un titulo a la ventana

			if (dialog_save.ShowDialog() == DialogResult.OK)
			{
				XLWorkbook wb = new XLWorkbook();
				wb.Worksheets.Add(no_vig, "hoja_lz");
				wb.SaveAs(dialog_save.FileName);
				MessageBox.Show("Se ha Generado el Archivo Correctamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			}
			
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			i_perma=0;
		}
		
		void MaskedTextBox1DoubleClick(object sender, EventArgs e)
		{
			maskedTextBox1.Clear();
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				buscar_cred();
			}
		}
		
	}
}

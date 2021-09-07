/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 31/10/2017
 * Hora: 03:04 p.m.
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

namespace Nova_Gear.Inventario
{
	/// <summary>
	/// Description of Opciones_inventario.
	/// </summary>
	public partial class Opciones_inventario : Form
	{
		public Opciones_inventario()
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
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		Conexion conex5 = new Conexion();
		Conexion conex6 = new Conexion();
		
		DataTable base_inv = new DataTable();
		DataTable sobrante = new DataTable();
		
		public void borrar_respon(){
			
			DialogResult reso = MessageBox.Show("Borrará a este usuario de la Tabla: \n"+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString()+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(reso == DialogResult.Yes){
				conex3.conectar("inventario");
				conex3.consultar("DELETE FROM responsables WHERE idresponsables="+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString()+"");
				MessageBox.Show("Registro Eliminado Exitosamente","AVISO");
			}
		
		}
		
		public void borrar_auxiliar(){
			
			DialogResult reso = MessageBox.Show("Borrará a este usuario de la Tabla: \n"+dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[1].Value.ToString()+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(reso == DialogResult.Yes){
				conex3.conectar("inventario");
				conex3.consultar("DELETE FROM auxiliares WHERE id_auxiliares="+dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString()+"");
				MessageBox.Show("Registro Eliminado Exitosamente","AVISO");
			}
		
		}
		
		public void actualizar_dgv1(){
			conex.conectar("inventario");
			dataGridView1.DataSource=conex.consultar("SELECT idresponsables,nombre,libro,rango_inicial,rango_final,casos FROM responsables ORDER BY idresponsables");
			dataGridView1.Columns[0].HeaderText="ID";
			dataGridView1.Columns[0].Width=30;
			dataGridView1.Columns[1].HeaderText="Nombre";
			dataGridView1.Columns[1].Width=200;
			dataGridView1.Columns[2].HeaderText="N° Libro";
			dataGridView1.Columns[2].Width=50;
			dataGridView1.Columns[3].HeaderText="Caso Inicial";
			dataGridView1.Columns[4].HeaderText="Caso Final";
			dataGridView1.Columns[5].HeaderText="Total Asignados";
		}
		
		public void actualizar_dgv2(){
			conex2.conectar("inventario");
			dataGridView2.DataSource=conex2.consultar("SELECT id_auxiliares,nombre_auxiliar,turno,responsable_asignado FROM auxiliares ORDER BY id_auxiliares");
			dataGridView2.Columns[0].HeaderText="ID";
			dataGridView2.Columns[0].Width=30;
			dataGridView2.Columns[1].HeaderText="NOMBRE";
			dataGridView2.Columns[1].Width=200;
			dataGridView2.Columns[2].HeaderText="TURNO";
			dataGridView2.Columns[3].HeaderText="ASIGNADO A";
			dataGridView2.Columns[3].Width=200;
		}
		
		void Opciones_inventarioLoad(object sender, EventArgs e)
		{

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			actualizar_dgv1();
			actualizar_dgv2();
		}
		//agregar responsable
		void Button1Click(object sender, EventArgs e)
		{
			Responsables res = new Responsables(1,-1);
			res.ShowDialog();
			actualizar_dgv1();
		}
		//editar responsable
		void Button2Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				if(dataGridView1.CurrentCell.RowIndex > -1){
					Responsables res1 = new Responsables(2,Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString()));
					res1.ShowDialog();
					actualizar_dgv1();
				}
			}
		}
		//quitar responsable
		void Button3Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				if(dataGridView1.CurrentCell.RowIndex > -1){
					borrar_respon();
					actualizar_dgv1();
				}
			}
		}
		//agregar auxiliar
		void Button6Click(object sender, EventArgs e)
		{
			Auxiliares auxi = new Auxiliares(1,-1);
			auxi.ShowDialog();
			actualizar_dgv2();
		}
		//editar auxiliar
		void Button5Click(object sender, EventArgs e)
		{
			if(dataGridView2.RowCount>0){
				if(dataGridView2.CurrentCell.RowIndex > -1){
					Auxiliares auxi1 = new Auxiliares(2,Convert.ToInt32(dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString()));
					auxi1.ShowDialog();
					actualizar_dgv2();
				}
			}
		}
		//quitar auxiliar
		void Button4Click(object sender, EventArgs e)
		{
			if(dataGridView2.RowCount>0){
				if(dataGridView2.CurrentCell.RowIndex > -1){
					borrar_auxiliar();
					actualizar_dgv2();
				}
			}
		}
		
		void Panel2Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		//exportar a excel
		void Button9Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			SaveFileDialog dialog_save = new SaveFileDialog();
			dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
			dialog_save.Title = "Guardar Respaldo de la Base";//le damos un titulo a la ventana

			if (dialog_save.ShowDialog() == DialogResult.OK){
				try {
					conex4.conectar("inventario");
					conex5.conectar("inventario");
					base_inv=conex4.consultar("SELECT * FROM base_inventario");
					sobrante=conex5.consultar("SELECT * FROM sobrante");
				} catch (Exception es) {
					
				}
				XLWorkbook wb = new XLWorkbook();
				wb.Worksheets.Add(base_inv, "Base_Inventario_Nova");
				wb.Worksheets.Add(sobrante, "Base_Sobrante_Nova");
				wb.SaveAs(@""+dialog_save.FileName+"");
				MessageBox.Show("Archivo guardado correctamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			}
		}
		//ingresar BD
		void Button7Click(object sender, EventArgs e)
		{
			conex4.conectar("inventario");
			base_inv=conex4.consultar("SELECT count(idbase_inventario) FROM base_inventario");
			if(Convert.ToInt32(base_inv.Rows[0][0].ToString()) < 1){
				panel1.Visible=true;
			}else{
				MessageBox.Show("No se Pueden Cargar Datos si la Base Anterior todavia no ha sido Reiniciada.","AVISO");
			}
		}
		//borrar BD
		void Button8Click(object sender, EventArgs e)
		{
			panel1.Visible=false;
			DialogResult respu= MessageBox.Show("Está por Reiniciar la Base de Datos.\nEsta Acción no puede ser revertida.\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
		
			if(respu== DialogResult.Yes){
				conex4.conectar("inventario");
				conex5.conectar("inventario");
				base_inv=conex4.consultar("SELECT * FROM base_inventario");
				sobrante=conex5.consultar("SELECT * FROM sobrante");
				
				XLWorkbook wb = new XLWorkbook();
				wb.Worksheets.Add(base_inv, "Base_Inventario_Nova");
				wb.Worksheets.Add(sobrante, "Base_Sobrante_Nova");
				wb.SaveAs(@"temp/Respaldo_Base_Inventario_Nova.XLSX");
				conex4.consultar("TRUNCATE inventario.base_inventario");
				conex4.consultar("TRUNCATE inventario.sobrante");

				MessageBox.Show("La Base de Datos Fue Reiniciada Exitosamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			}
		}
		
		void TabPage3Click(object sender, EventArgs e)
		{
			
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			Depuracion.Depu_rale ingresa_rale = new Nova_Gear.Depuracion.Depu_rale(2);
			ingresa_rale.ShowDialog();
			//Lector_rale_txt rale_txt = new Lector_rale_txt(2);
			//rale_txt.ShowDialog();
			//this.Hide();
			//rale_txt.Focus();
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			int i=0;
			DialogResult respu= MessageBox.Show("Se Borrarán las Asignaciones de Libro previas en la BD.\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
		
			if(respu== DialogResult.Yes){
				conex6.conectar("inventario");
				conex6.consultar("UPDATE base_inventario SET no_cuaderno=0");
				
				while(i<dataGridView1.RowCount){
					if(i==0){
						conex6.consultar("UPDATE inventario.base_inventario SET no_cuaderno="+dataGridView1.Rows[i].Cells[2].Value.ToString()+" order by reg_pat,credito limit "+dataGridView1.Rows[i].Cells[5].Value.ToString()+"");
					}else{
						conex6.consultar("UPDATE inventario.base_inventario SET no_cuaderno="+dataGridView1.Rows[i].Cells[2].Value.ToString()+" WHERE no_cuaderno=0 order by reg_pat,credito limit "+dataGridView1.Rows[i].Cells[5].Value.ToString()+"");
					}
					i++;
				}
				
				MessageBox.Show("Libros Asignados correctamente en la BD","AVISO");
			}
			
		}
	}
}

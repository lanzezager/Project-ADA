/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 14/03/2016
 * Hora: 04:49 p. m.
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

namespace Nova_Gear.Registros
{
	/// <summary>
	/// Description of Selector_sector.
	/// </summary>
	public partial class Selector_sector : Form
	{
		public Selector_sector(int id,int id_conti)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.id_user=id;
			this.id_cont=id_conti;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int i=0,j=0,k=0,l=0,id_user=0,verdes=0,blancos=0,id_cont=0;
		String sql,sec_bus,user_bus;
		DialogResult respuesta;
		public int[] asignados,no_asignados;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable users = new DataTable();
		
		
		
		public void afectar_en_bd(){
			
			i=0;
            while(i < asignados.Length){
                       sql = "UPDATE sectores SET id_notificador= " + id_user + ",id_controlador=" + id_cont + " WHERE sector =" + asignados[i] + " ;";
                       conex.consultar(sql);
                       i++;
                  }
            
			i=0;
            while(i<no_asignados.Length){
				    sql="UPDATE sectores SET id_notificador= 0, id_controlador= 0 WHERE sector ="+no_asignados[i]+" ;";
				    conex.consultar(sql);
				    i++;
			    }
		}
		
		void Selector_sectorLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			sql="SELECT * FROM sectores ORDER BY sector";
			consultamysql = conex.consultar(sql);
			dataGridView2.DataSource = consultamysql;
			dataGridView1.Rows.Add();
			//id_user=1;
			
			do{
				dataGridView1.Rows[j].Cells[k].Value = dataGridView2.Rows[i].Cells[3].Value.ToString();
				
				if(dataGridView2.Rows[i].Cells[2].Value.ToString().Equals("0")){
					dataGridView1.Rows[j].Cells[k].Style.BackColor=System.Drawing.Color.White;
					dataGridView1.Rows[j].Cells[k].Style.ForeColor=System.Drawing.Color.Black;
					blancos++;
				}else{
					if(dataGridView2.Rows[i].Cells[2].Value.ToString().Equals(id_user.ToString())){
						dataGridView1.Rows[j].Cells[k].Style.BackColor=System.Drawing.Color.LimeGreen;
						dataGridView1.Rows[j].Cells[k].Style.ForeColor=System.Drawing.Color.White;
						verdes++;
					}else{
						dataGridView1.Rows[j].Cells[k].Style.BackColor=System.Drawing.Color.DarkGray;
						dataGridView1.Rows[j].Cells[k].Style.ForeColor=System.Drawing.Color.White;
						
						users=conex2.consultar("SELECT nombre,apellido FROM usuarios WHERE id_usuario ="+dataGridView2.Rows[i].Cells[2].Value.ToString());
						dataGridView3.DataSource=users;
						dataGridView1.Rows[j].Cells[k].ToolTipText = dataGridView3.Rows[0].Cells[0].Value.ToString()+" "+dataGridView3.Rows[0].Cells[1].Value.ToString();
					}
				}
				
				k++;
				if(k==dataGridView1.ColumnCount){
					j++;
					k=0;
					dataGridView1.Rows.Add();
				}
				i++;
			}while(i<dataGridView2.RowCount);
			
			//dataGridView1.Rows.RemoveAt(dataGridView1.RowCount-1);
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void DataGridView1CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			
		}
		
		void DataGridView1CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			
		}
		
		void DataGridView1Click(object sender, EventArgs e)
		{
			
		}
		
		void DataGridView1MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right){
				if ((0 <= (dataGridView1.HitTest(e.X, e.Y).RowIndex)) && ((dataGridView1.HitTest(e.X, e.Y).RowIndex) <= (dataGridView1.RowCount-1)))
                {
					dataGridView1.Rows[dataGridView1.HitTest(e.X, e.Y).RowIndex].Cells[dataGridView1.HitTest(e.X, e.Y).ColumnIndex].Selected=true;
				}
			}
		}
		
		void ContextMenuStrip1Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//MessageBox.Show(""+dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor.ToString());
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor==System.Drawing.Color.DarkGray){
				asignarSectorToolStripMenuItem.Enabled=false;
				removerSectorToolStripMenuItem.Enabled=false;
			}
			
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor==System.Drawing.Color.LimeGreen){
				asignarSectorToolStripMenuItem.Enabled=false;
				removerSectorToolStripMenuItem.Enabled=true;
			}
			
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor==System.Drawing.Color.White){
				asignarSectorToolStripMenuItem.Enabled=true;
				removerSectorToolStripMenuItem.Enabled=false;
			}
			
			if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor==System.Drawing.Color.Empty){
					//MessageBox.Show("lalala  "+dataGridView1.CurrentRow.Index+","+dataGridView1.CurrentCell.ColumnIndex);
					asignarSectorToolStripMenuItem.Enabled=false;
					removerSectorToolStripMenuItem.Enabled=false;
			}
		}
		
		void AsignarSectorToolStripMenuItemClick(object sender, EventArgs e)
		{
			//respuesta = MessageBox.Show("Está por modificar la información de un usuario.\n\n¿Está Seguro de querer continuar?"+
			//                            "","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button2);
			
			//if(respuesta ==DialogResult.Yes){
			dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor=System.Drawing.Color.LimeGreen;
			dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.ForeColor=System.Drawing.Color.White;
			verdes++;
			blancos--;
			//}
		}
		
		void RemoverSectorToolStripMenuItemClick(object sender, EventArgs e)
		{
			//respuesta = MessageBox.Show("Está por remover un sect la información de un usuario.\n\n¿Está Seguro de querer continuar?"+
			//                           "","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button2);
			
			//if(respuesta ==DialogResult.Yes){
			dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor=System.Drawing.Color.White;
			dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.ForeColor=System.Drawing.Color.Black;
			verdes--;
			blancos++;
			//}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			i=0;
			j=0;
			k=0;
			l=0;
			asignados = new int[verdes];
			no_asignados = new int[blancos];
			//MessageBox.Show(verdes.ToString());
			
			do{
				if(dataGridView1.Rows[j].Cells[k].Style.BackColor==System.Drawing.Color.LimeGreen){
					asignados[i]=Convert.ToInt32(dataGridView1.Rows[j].Cells[k].Value.ToString());
					i++;
				}
				
				if(dataGridView1.Rows[j].Cells[k].Style.BackColor==System.Drawing.Color.White){
					no_asignados[l]=Convert.ToInt32(dataGridView1.Rows[j].Cells[k].Value.ToString());
					l++;
				}

				k++;
				if(k==dataGridView1.ColumnCount){
					j++;
					k=0;
				}
				
			}while(j<dataGridView1.RowCount);
			
			//MessageBox.Show(asignados.Length+","+asignados[asignados.Length-1]);
			//MessageBox.Show(""+no_asignados[no_asignados.Length-1]+","+no_asignados.Length);
			
			//MainForm mani = (MainForm)this.MdiParent;
			//mani.form_activo(1,asignados,no_asignados);
			afectar_en_bd();
			this.Close();
		}
		
		void DataGridView1MouseHover(object sender, EventArgs e)
		{
		/*	if(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Style.BackColor==System.Drawing.Color.DarkGray)
			{
				i=0;
				j=0;
				k=0;
				sec_bus=dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].Value.ToString();
			
				do{
					if(dataGridView2.Rows[i].Cells[3].Value.ToString().Equals(sec_bus)){
						user_bus=dataGridView2.Rows[i].Cells[3].Value.ToString();
						i=dataGridView2.RowCount;
					}
					i++;
				}while(i<dataGridView2.RowCount);
				
				users=conex.consultar("SELECT nombre FROM usuarios WHERE id_usuario ="+user_bus);
				dataGridView3.DataSource=users;
				dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[dataGridView1.CurrentCell.ColumnIndex].ToolTipText=dataGridView3.Rows[0].Cells[0].Value.ToString();
		
		    }*/
		}
	}
}

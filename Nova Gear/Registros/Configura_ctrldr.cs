/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 11/04/2018
 * Hora: 09:42 a. m.
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
using System.ComponentModel;

namespace Nova_Gear.Registros
{
	/// <summary>
	/// Description of Configura_ctrldr.
	/// </summary>
	public partial class Configura_ctrldr : Form
	{
		public Configura_ctrldr()
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
		
		DataTable tabla_ctrldr=new DataTable();
		DataTable tabla_ntfcdr_info=new DataTable();
		DataTable tabla_ntfcdr_todos=new DataTable();		
		DataTable tabla_ntfcdr_sector=new DataTable();
		DataTable tabla_sctr=new DataTable();
		
		int id_cn=0,id_not=0;
		
		public void carga_notis(int id_cont){
			int i=0;
			//Notificadores
			listBox2.Items.Clear();			
			//tabla_ntfcdr_sector=conex3.consultar("SELECT DISTINCT id_notificador FROM sectores WHERE id_controlador="+id_cont);
			//tabla_ntfcdr_sector=conex3.consultar("SELECT id_usuario FROM usuarios WHERE controlador="+id_cont);
			
			tabla_ntfcdr_info=conex2.consultar("SELECT nombre, apellido,id_usuario FROM usuarios WHERE controlador="+id_cont);
			
			//if(tabla_ntfcdr_sector.Rows.Count>0){
				//while(i<tabla_ntfcdr_sector.Rows.Count){
				while(i<tabla_ntfcdr_info.Rows.Count){
					//tabla_ntfcdr_info=conex2.consultar("SELECT nombre, apellido,id_usuario FROM usuarios WHERE id_usuario="+tabla_ntfcdr_sector.Rows[i][0].ToString());
					//listBox2.Items.Add(tabla_ntfcdr_info.Rows[][1].ToString()+" "+tabla_ntfcdr_info.Rows[0][0].ToString());
					listBox2.Items.Add(tabla_ntfcdr_info.Rows[i][1].ToString()+" "+tabla_ntfcdr_info.Rows[i][0].ToString());
					i++;
				}
			//}else{
				//listBox2.Items.Add("Vacío");
			//}
		}
		
		public void carga_sectores(int id_noti){
			int i=0;
			//sectores
			listBox3.Items.Clear();			
			tabla_sctr=conex4.consultar("SELECT sector FROM sectores WHERE id_notificador="+id_noti+" order by sector asc");
			if(tabla_sctr.Rows.Count>0){
				while(i<tabla_sctr.Rows.Count){
					listBox3.Items.Add(tabla_sctr.Rows[i][0].ToString());
					i++;
				}
			}else{
				
			}
			
		}
		
		void Configura_ctrldrLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			int i =0;
			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			conex3.conectar("base_principal");
			conex4.conectar("base_principal");
			conex5.conectar("base_principal");
			
			//Controlador
			tabla_ctrldr=conex.consultar("SELECT nombre, apellido,id_usuario FROM usuarios WHERE puesto=\"Controlador\" Order by apellido");
			
			while(i<tabla_ctrldr.Rows.Count){
				listBox1.Items.Add(tabla_ctrldr.Rows[i][1].ToString()+" "+tabla_ctrldr.Rows[i][0].ToString());
				i++;
			}
			
			
			//Notificadores
			tabla_ntfcdr_todos=conex5.consultar("SELECT nombre, apellido,id_usuario FROM usuarios WHERE puesto=\"Notificador\" Order by apellido");
		
		}
		
		void ListBox1Click(object sender, EventArgs e)
		{
			
		}
		
		void ListBox1MouseDown(object sender, MouseEventArgs e)
		{
			int id_buscar=0;
			if(listBox1.Items.Count>0){
				if(e.Button.Equals(MouseButtons.Left)==true){
				   	if((0 <= (listBox1.IndexFromPoint(e.X,e.Y)))&&(listBox1.IndexFromPoint(e.X,e.Y)<= (listBox1.Items.Count-1))){
				   		//id_buscar=listBox1.Items[listBox1.IndexFromPoint(e.X,e.Y)].ToString();
						id_buscar=listBox1.IndexFromPoint(e.X,e.Y);
						listBox1.SelectedIndex= listBox1.IndexFromPoint(e.X,e.Y);
						id_cn=Convert.ToInt32(tabla_ctrldr.Rows[id_buscar][2].ToString());
						carga_notis(id_cn);
						//MessageBox.Show(listBox1.Items[listBox1.IndexFromPoint(e.X,e.Y)].ToString());
				   	}
				}
				   
				if(e.Button.Equals(MouseButtons.Right)){
				      if((0 <= (listBox1.IndexFromPoint(e.X,e.Y)))&&(listBox1.IndexFromPoint(e.X,e.Y)<= (listBox1.Items.Count-1))){
				   		//id_buscar=listBox1.Items[listBox1.IndexFromPoint(e.X,e.Y)].ToString();
						id_buscar=listBox1.IndexFromPoint(e.X,e.Y);
						listBox1.SelectedIndex= listBox1.IndexFromPoint(e.X,e.Y);
						id_cn=Convert.ToInt32(tabla_ctrldr.Rows[id_buscar][2].ToString());
						carga_notis(id_cn);
						//MessageBox.Show(listBox1.Items[listBox1.IndexFromPoint(e.X,e.Y)].ToString());
				   	}	
				}
		    }
		}
		
		void ListBox2MouseDown(object sender, MouseEventArgs e)
		{
			int id_buscar=0,i=0;
			if(listBox2.Items.Count>0){
				if(e.Button.Equals(MouseButtons.Left)==true){
				   	if((0 <= (listBox2.IndexFromPoint(e.X,e.Y)))&&(listBox2.IndexFromPoint(e.X,e.Y)<= (listBox2.Items.Count-1))){
				   		//id_buscar=listBox2.Items[listBox2.IndexFromPoint(e.X,e.Y)].ToString();
						id_buscar=listBox2.IndexFromPoint(e.X,e.Y);
						listBox2.SelectedIndex= listBox2.IndexFromPoint(e.X,e.Y);
						i=0;
						while(i<tabla_ntfcdr_todos.Rows.Count){
							if((tabla_ntfcdr_todos.Rows[i][1].ToString()+" "+tabla_ntfcdr_todos.Rows[i][0].ToString()).Equals(listBox2.SelectedItem.ToString())){
								id_not=Convert.ToInt32(tabla_ntfcdr_todos.Rows[i][2].ToString());
								i=tabla_ntfcdr_todos.Rows.Count+1;
							}
							i++;
						}
						
						carga_sectores(id_not);
						//MessageBox.Show(listBox2.Items[listBox2.IndexFromPoint(e.X,e.Y)].ToString());
				   	}
				}
				   
				if(e.Button.Equals(MouseButtons.Right)){
				      if((0 <= (listBox2.IndexFromPoint(e.X,e.Y)))&&(listBox2.IndexFromPoint(e.X,e.Y)<= (listBox2.Items.Count-1))){
				   		//id_buscar=listBox2.Items[listBox2.IndexFromPoint(e.X,e.Y)].ToString();
						id_buscar=listBox2.IndexFromPoint(e.X,e.Y);
						listBox2.SelectedIndex= listBox2.IndexFromPoint(e.X,e.Y);
						i=0;
						while(i<tabla_ntfcdr_todos.Rows.Count){
							if((tabla_ntfcdr_todos.Rows[i][1].ToString()+" "+tabla_ntfcdr_todos.Rows[i][0].ToString()).Equals(listBox2.SelectedItem.ToString())){
								id_not=Convert.ToInt32(tabla_ntfcdr_todos.Rows[i][2].ToString());
								i=tabla_ntfcdr_todos.Rows.Count+1;
							}
							i++;
						}
						
						carga_sectores(id_not);
						//MessageBox.Show(listBox2.Items[listBox2.IndexFromPoint(e.X,e.Y)].ToString());
				   	}	
				}
		    }
		}
		
		void AsignarNotificadorToolStripMenuItemClick(object sender, EventArgs e)
		{
			Selector_notificador sel_not = new Selector_notificador(id_cn);
			sel_not.ShowDialog();
			carga_notis(id_cn);
		}
		
		void EditarSectoresToolStripMenuItemClick(object sender, EventArgs e)
		{
			Selector_sector sel_sec = new Selector_sector(id_not,id_cn);
			sel_sec.ShowDialog();
			carga_sectores(id_not);
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex>-1){
				int id_buscar=0;
				id_buscar=listBox1.SelectedIndex;
				id_cn=Convert.ToInt32(tabla_ctrldr.Rows[id_buscar][2].ToString());
				carga_notis(id_cn);
				listBox3.Items.Clear();
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex>-1){
				Selector_notificador sel_not = new Selector_notificador(id_cn);
				sel_not.ShowDialog();
				carga_notis(id_cn);
			}else{
				MessageBox.Show("Selecciona un Controlador","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if(listBox2.SelectedIndex>-1){
				Selector_sector sel_sec = new Selector_sector (id_not,id_cn);
				//MessageBox.Show(id_not+"|"+id_cn);
				sel_sec.ShowDialog();
				carga_sectores(id_not);
			}else{
				MessageBox.Show("Selecciona un Notificador","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}
		
		void ListBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			int i=0;
			if(listBox2.SelectedIndex>-1){
				while(i<tabla_ntfcdr_todos.Rows.Count){
					if((tabla_ntfcdr_todos.Rows[i][1].ToString()+" "+tabla_ntfcdr_todos.Rows[i][0].ToString()).Equals(listBox2.SelectedItem.ToString())){
						id_not=Convert.ToInt32(tabla_ntfcdr_todos.Rows[i][2].ToString());
						i=tabla_ntfcdr_todos.Rows.Count+1;
					}
					i++;
				}
				carga_sectores(id_not);
			}
		}
		
		void ListBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				if(listBox1.SelectedIndex>-1){
					Selector_notificador sel_not = new Selector_notificador(id_cn);
					//MessageBox.Show(""+id_cn+"|"+listBox1.SelectedItem.ToString());
					sel_not.ShowDialog();
					carga_notis(id_cn);
				}else{
					MessageBox.Show("Selecciona un Controlador","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}
		}
		
		void ListBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				if(listBox2.SelectedIndex>-1){
					Selector_sector sel_sec = new Selector_sector (id_not,id_cn);
					sel_sec.ShowDialog();
					carga_sectores(id_not);
				}else{
					MessageBox.Show("Selecciona un Notificador","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
	}
}

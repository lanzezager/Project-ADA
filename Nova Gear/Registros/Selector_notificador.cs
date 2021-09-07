/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 15/03/2016
 * Hora: 03:37 p. m.
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
	/// Description of Selector_notificador.
	/// </summary>
	public partial class Selector_notificador : Form
	{
		public Selector_notificador(int id)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.id_user=id;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		int id_user=0,i=0,j=0,k=0,found=0;
		public int cerrado=0;
		String sql,pasa_valor,compara;
		public int[] asignados,no_asignados;
		
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		
		DataTable consultamysql = new DataTable();
		DataTable consulta2 = new DataTable();
		DataTable consulta3 = new DataTable();
		
		public void afectar_bd(){
            
			i=0;
			while(i < asignados.Length){
				sql = "UPDATE usuarios SET controlador= \"" + id_user + "\" WHERE id_usuario =" + asignados[i] + " ;";
                //MessageBox.Show(""+sql);
				conex.consultar(sql);

				sql = "UPDATE sectores SET id_controlador= \"" + id_user + "\" WHERE id_notificador =" + asignados[i] + " ;";
                //MessageBox.Show("" + sql);
				conex.consultar(sql);

				i++;
			}
			
			i=0;
			while(i < no_asignados.Length){
				sql = "UPDATE usuarios SET controlador= \"0\" WHERE id_usuario =" + no_asignados[i] + " ;";
                //MessageBox.Show("" + sql);
				conex.consultar(sql);

				sql = "UPDATE sectores SET id_controlador= \"0\" WHERE id_notificador =" + no_asignados[i] + " ;";
                //MessageBox.Show("" + sql);
				conex.consultar(sql);

				i++;
			}
			
			conex.cerrar();
		}
		
		void Selector_notificadorLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			conex3.conectar("base_principal");
            //MessageBox.Show("" + id_user);
			sql = "SELECT id_usuario,nombre,apellido FROM base_principal.usuarios where controlador= \"" +id_user+"\" AND puesto =\"Notificador\";";
			consultamysql = conex.consultar(sql);
			consultamysql.Columns.Add();
			dataGridView1.DataSource = consultamysql;
			
			sql = "SELECT id_usuario,nombre,apellido FROM base_principal.usuarios where controlador = \"0\" AND puesto =\"Notificador\";"; 
			consulta2 = conex2.consultar(sql);
			consulta2.Columns.Add();
			dataGridView2.DataSource = consulta2;
			
			if(dataGridView1.RowCount>0){
				//dataGridView1.Columns.Add("full_name","full_name");
				i=0;
				do{
					if(dataGridView1.Rows[i].Cells[1].Value != null){
						dataGridView1.Rows[i].Cells[3].Value = (dataGridView1.Rows[i].Cells[2].Value.ToString()+" "+dataGridView1.Rows[i].Cells[1].Value.ToString()).ToString();
						listBox1.Items.Add(dataGridView1.Rows[i].Cells[3].Value.ToString());
					}
					i++;
				}while(i<dataGridView1.RowCount);
			}
			
			i=0;
			if(dataGridView2.RowCount>0){
				//dataGridView2.Columns.Add("full_name","full_name");
				i=0;
				do{
					if(dataGridView2.Rows[i].Cells[1].Value != null){
						dataGridView2.Rows[i].Cells[3].Value = (dataGridView2.Rows[i].Cells[2].Value.ToString()+" "+dataGridView2.Rows[i].Cells[1].Value.ToString()).ToString();
						listBox2.Items.Add(dataGridView2.Rows[i].Cells[3].Value.ToString());
					}
					i++;
				}while(i<dataGridView2.RowCount);
			}
			
			label1.Text="Asignados: "+listBox1.Items.Count;
			label2.Text="Disponibles: "+listBox2.Items.Count;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if((listBox2.Items.Count>0)&&(listBox2.SelectedItem != null)){
			pasa_valor = listBox2.SelectedItem.ToString();
			listBox2.Items.Remove(listBox2.SelectedItem.ToString());
			listBox1.Items.Add(pasa_valor);
			
			label1.Text="Asignados: "+listBox1.Items.Count;
			label2.Text="Disponibles: "+listBox2.Items.Count;
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if((listBox1.Items.Count>0)&&(listBox1.SelectedItem != null)){
			pasa_valor = listBox1.SelectedItem.ToString();
			listBox1.Items.Remove(listBox1.SelectedItem.ToString());
			listBox2.Items.Add(pasa_valor);
			
			label1.Text="Asignados: "+listBox1.Items.Count;
			label2.Text="Disponibles: "+listBox2.Items.Count;
			}
			
		}
		
		void ListBox2MouseHover(object sender, EventArgs e)
		{
			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			//try{
				i=0;
				j=0;			
				
				if(listBox1.Items.Count>0){
					
					asignados = new int[listBox1.Items.Count];
					
					do{
						compara=listBox1.Items[i].ToString();
						if(dataGridView2.RowCount>0){
							do{
								if(dataGridView2.Rows[j].Cells[3].Value.ToString().Equals(compara)){
									asignados[i]=Convert.ToInt32(dataGridView2.Rows[j].Cells[0].Value.ToString());
									found=1;
                                    //MessageBox.Show("" + asignados[i]);
								}
								j++;
							}while(j<dataGridView2.RowCount);
						}
						j=0;
						
						if(found==0){
							if(dataGridView1.RowCount>0){
								do{
									if(dataGridView1.Rows[j].Cells[3].Value.ToString().Equals(compara)){
										asignados[i]=Convert.ToInt32(dataGridView1.Rows[j].Cells[0].Value.ToString());
										found=1;
									}
									j++;
								}while(j<dataGridView1.RowCount);
							}
						}
						j=0;
						found=0;
						i++;
					}while(i<listBox1.Items.Count);
					//MessageBox.Show(""+asignados[asignados.Length-1]+","+asignados.Length);
					
				}
				
				i=0;
				j=0;
				if(listBox2.Items.Count>0){
					no_asignados= new int[listBox2.Items.Count];
					do{
						//if(i>0){
						compara=listBox2.Items[i].ToString();
						//}else{
						//	compara="NULL";
						//}
						if(dataGridView2.RowCount>0){
							do{
								if(dataGridView2.Rows[j].Cells[3].Value.ToString().Equals(compara)){
									no_asignados[i]=Convert.ToInt32(dataGridView2.Rows[j].Cells[0].Value.ToString());
									found=1;
								}
								j++;
							}while(j<dataGridView2.RowCount);
						}
						j=0;
						
						if(found==0){
							if(dataGridView1.RowCount>0){
								do{
									if(dataGridView1.Rows[j].Cells[3].Value.ToString().Equals(compara)){
										no_asignados[i]=Convert.ToInt32(dataGridView1.Rows[j].Cells[0].Value.ToString());
										found=1;
									}
									j++;
								}while(j<dataGridView1.RowCount);
							}
						}
						j=0;
						found=0;
						i++;
					}while(i<listBox2.Items.Count);
				}
				//MessageBox.Show(""+no_asignados[no_asignados.Length-1]+","+no_asignados.Length);
				
				if(listBox1.Items.Count==0){//si borran todos los notifs del controlador
					//MessageBox.Show("borrar notifs |"+listBox1.Items.Count);
					i=0;
					j=0;
						
					asignados = new int[1];
					asignados[0]=0;
					no_asignados= new int[dataGridView1.RowCount];
					while(j<dataGridView1.RowCount){
						no_asignados[i]=Convert.ToInt32(dataGridView1.Rows[j].Cells[0].Value.ToString());
						i++;
						j++;
					}
				}
				
				
                //if (asignados.Length == 0 || no_asignados.Length == 0)
                if (asignados == null)
                {
					asignados = new int[1];
					asignados[0]=0;
					//MainForm mani = (MainForm)this.MdiParent;
					//mani.form_activo(1,asignados,no_asignados);					
				}

                if (no_asignados == null)
                {
                    no_asignados = new int[1];
                    no_asignados[0] = 0;
                    //MainForm mani = (MainForm)this.MdiParent;
                    //mani.form_activo(1,asignados,no_asignados);					
                }
				//MainForm mani = (MainForm)this.MdiParent;
				//mani.form_activo(1,asignados,no_asignados);
                //MessageBox.Show("" + asignados[0]);
				afectar_bd();
				
				this.Close();
			//}catch(Exception ex){
			//	MessageBox.Show("borrar notifs ");
			//	this.Close();
			//}
			
		}
	}
}


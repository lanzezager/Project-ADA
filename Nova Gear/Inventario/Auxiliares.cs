/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 31/10/2017
 * Hora: 04:51 p.m.
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
	/// Description of Auxiliares.
	/// </summary>
	public partial class Auxiliares : Form
	{
		public Auxiliares(int tipo_ini,int id_ini)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.tipo_inicio=tipo_ini;
			this.id_inicio=id_ini;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int tipo_inicio=0,id_inicio=0;
		Conexion conex = new Conexion();
		Conexion conex1 = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		
		DataTable consulta_users = new DataTable();
		DataTable consulta = new DataTable();
		
		public void llena_cb2(){
			int i=0;
			comboBox2.Items.Clear();
			conex1.conectar("inventario");
			consulta_users=conex1.consultar("SELECT nombre FROM responsables ORDER BY nombre");
			while(i<consulta_users.Rows.Count){
				comboBox2.Items.Add(consulta_users.Rows[i][0].ToString());
				i++;
			}
		}
		
		public void consulta_auxi(){
			int j=0,k=0;
			String nombre_respo="",turno="";
			conex.conectar("inventario");
			consulta=conex.consultar("SELECT nombre_auxiliar,turno,responsable_asignado FROM auxiliares WHERE id_auxiliares= "+id_inicio+"");
			if(consulta.Rows.Count>0){
				//MessageBox.Show(consulta.Rows[0][0].ToString()+"|"+j);
				nombre_respo=consulta.Rows[0][2].ToString();
				turno=consulta.Rows[0][1].ToString();
				
				while(j<comboBox2.Items.Count){
					if(nombre_respo.Equals(comboBox2.Items[j].ToString())==true){
						comboBox2.SelectedIndex=j;
					}
					j++;
				}
				j=0;
				while(j<comboBox1.Items.Count){
					if(turno.Equals(comboBox1.Items[j].ToString())==true){
						comboBox1.SelectedIndex=j;
					}
					j++;
				}
				
				textBox1.Text=consulta.Rows[0][0].ToString();
			}
		}
		
		public void ingresar(){
			conex2.conectar("inventario");
			conex2.consultar("INSERT INTO auxiliares(nombre_auxiliar,turno,responsable_asignado) VALUES(\""+textBox1.Text+"\",\""+comboBox1.SelectedItem.ToString()+"\",\""+comboBox2.SelectedItem.ToString()+"\")");
			MessageBox.Show("Registro Ingresado Exitosamente","AVISO");
			this.Close();
		}
		
		public void actualizar(){
			conex2.conectar("inventario");
			conex2.consultar("UPDATE auxiliares SET nombre_auxiliar=\""+textBox1.Text+"\",turno=\""+comboBox1.SelectedItem.ToString()+"\",responsable_asignado=\""+comboBox2.SelectedItem.ToString()+"\" WHERE id_auxiliares= "+id_inicio+" ");
			MessageBox.Show("Registro Actualizado Exitosamente","AVISO");
			this.Close();
		}
		
		void AuxiliaresLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			llena_cb2();
			if(tipo_inicio==2){
				consulta_auxi();
			}
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			int k=0;
			String msj_err="";
			if(textBox1.Text.Length<10){
				k++;
				msj_err+="•El Nombre de Auxiliar es Muy Corto (Mínimo 10 caracteres)\n";
			}
			
			if(comboBox1.SelectedIndex < 0){
				k++;
				msj_err+="•Selecciona un Turno\n";
			}
			
			if(comboBox2.SelectedIndex < 0){
				k++;
				msj_err+="•Selecciona el Responsable a Asignar\n";
			}
			
			if(k==0){
				if(tipo_inicio==2){
					actualizar();
				}else{
					ingresar();
				}
			}else{
				MessageBox.Show(msj_err,"AVISO");
			}
		}
	}
}

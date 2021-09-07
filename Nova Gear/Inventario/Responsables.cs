/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 31/10/2017
 * Hora: 04:16 p.m.
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
	/// Description of Responsables.
	/// </summary>
	public partial class Responsables : Form
	{
		public Responsables(int tipo_ini,int id_ini)
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
		
		int tipo_inicio=0,id_inicio=0,id_user=0;
		
		Conexion conex = new Conexion();
		Conexion conex1 = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		
		DataTable consulta = new DataTable();
		DataTable consulta_users = new DataTable();
		DataTable lista_prohibida = new DataTable();
		
		void Label4Click(object sender, EventArgs e)
		{
			
		}
		
		public void llena_cb1(){
			int i=0;
			comboBox1.Items.Clear();
			conex1.conectar("base_principal");
			consulta_users=conex1.consultar("SELECT id_usuario,nombre,apellido FROM usuarios WHERE estatus=\"activo\" AND id_usuario NOT in (Select id_usuario from inventario.responsables) ORDER BY id_usuario");
			while(i<consulta_users.Rows.Count){
				comboBox1.Items.Add(consulta_users.Rows[i][1].ToString()+" "+consulta_users.Rows[i][2].ToString());
				i++;
			}
		}
		
		public void llena_cb1_edicion(){
			int i=0;
			comboBox1.Items.Clear();
			conex1.conectar("base_principal");
			consulta_users=conex1.consultar("SELECT id_usuario,nombre,apellido FROM usuarios WHERE estatus=\"activo\" ORDER BY id_usuario");
			while(i<consulta_users.Rows.Count){
				comboBox1.Items.Add(consulta_users.Rows[i][1].ToString()+" "+consulta_users.Rows[i][2].ToString());
				i++;
			}
		}
		
		public void llena_lista_prohi(){
			conex3.conectar("inventario");
			lista_prohibida=conex3.consultar("SELECT libro,rango_inicial,rango_final,idresponsables FROM responsables ORDER BY rango_final DESC");			
		}
		
		public void consulta_respon(){
			int j=0,k=0;
			String nombre_respo="";
			conex.conectar("inventario");
			consulta=conex.consultar("SELECT id_usuario,libro,rango_inicial,rango_final,casos FROM responsables WHERE idresponsables= "+id_inicio+" ORDER BY idresponsables");
			if(consulta.Rows.Count>0){
				//MessageBox.Show(consulta.Rows[0][0].ToString()+"|"+j);
				while(j<consulta_users.Rows.Count){
					if(consulta.Rows[0][0].ToString().Equals(consulta_users.Rows[j][0].ToString())==true){
						nombre_respo=consulta_users.Rows[j][1].ToString()+" "+consulta_users.Rows[j][2].ToString();
					}
					j++;
				}
				
				j=0;
				while(j<comboBox1.Items.Count){
					if(nombre_respo.Equals(comboBox1.Items[j].ToString())==true){
						comboBox1.SelectedIndex=j;
					}
					j++;
				}
				
				textBox1.Text=consulta.Rows[0][1].ToString();
				textBox2.Text=consulta.Rows[0][2].ToString();
				textBox3.Text=consulta.Rows[0][4].ToString();
				textBox4.Text=consulta.Rows[0][3].ToString();
			}
		}
		
		public void ingresar(){
			int i=0,j=0,id_us=0;
			String mensaje_err="",nom_re="";
			
			if(comboBox1.SelectedIndex<0){
				i++;
				mensaje_err="•Seleccione un Usuario\n";
			}
			if((textBox1.Text.Length<=0) || (textBox1.BackColor != SystemColors.Window)){
				i++;
				mensaje_err+="•Escriba Correctamente el Número de Libro\n";
			}
			if((textBox2.Text.Length<=0) || (textBox2.BackColor != SystemColors.Window)){
				i++;
				mensaje_err+="•Escriba Correctamente el Número de Caso Inicial\n";
			}
			if((textBox3.Text.Length<=0) || (textBox3.BackColor == Color.Red)){
				i++;
				mensaje_err+="•Escriba Correctamente el Número de Casos a Asignar\n";
			}
			
			if(i==0){
				nom_re=comboBox1.SelectedItem.ToString();
				while(j<consulta_users.Rows.Count){
					if(nom_re.Equals((consulta_users.Rows[j][1].ToString()+" "+consulta_users.Rows[j][2].ToString()))==true){
						id_us=Convert.ToInt32(consulta_users.Rows[j][0].ToString());
					}
					j++;
				}
				
				conex2.conectar("inventario");
				conex2.consultar("INSERT INTO responsables(id_usuario,nombre,libro,rango_inicial,casos,rango_final) VALUES("+id_us+",\""+nom_re+"\",\""+textBox1.Text+"\","+textBox2.Text+","+textBox3.Text+","+textBox4.Text+")");
				MessageBox.Show("Registro Ingresado Exitosamente","AVISO");
				this.Close();
			}else{
				MessageBox.Show(mensaje_err,"AVISO");
			}
		}
		
		public void actualizar(){
			int i=0,j=0,id_us=0;
			String mensaje_err="",nom_re="";
			
			if(comboBox1.SelectedIndex<0){
				i++;
				mensaje_err="•Seleccione un Usuario\n";
			}
			if((textBox1.Text.Length<=0) || (textBox1.BackColor != SystemColors.Window)){
				i++;
				mensaje_err+="•Escriba Correctamente el Número de Libro\n";
			}
			if((textBox2.Text.Length<=0) || (textBox2.BackColor != SystemColors.Window)){
				i++;
				mensaje_err+="•Escriba Correctamente el Número de Caso Inicial\n";
			}
			if((textBox3.Text.Length<=0) || (textBox3.BackColor != SystemColors.Window)){
				i++;
				mensaje_err+="•Escriba Correctamente el Número de Casos a Asignar\n";
			}
			
			if(i==0){
				nom_re=comboBox1.SelectedItem.ToString();
				while(j<consulta_users.Rows.Count){
					if(nom_re.Equals((consulta_users.Rows[j][1].ToString()+" "+consulta_users.Rows[j][2].ToString()))==true){
						id_us=Convert.ToInt32(consulta_users.Rows[j][0].ToString());
					}
					j++;
				}
				conex2.conectar("inventario");
				conex2.consultar("UPDATE responsables SET id_usuario="+id_us+",nombre=\""+nom_re+"\",libro=\""+textBox1.Text+"\",rango_inicial="+textBox2.Text+",casos="+textBox3.Text+",rango_final="+textBox4.Text+" WHERE idresponsables= "+id_inicio+"");
				MessageBox.Show("Registro Actualizado Exitosamente","AVISO");
				this.Close();
			}else{
				MessageBox.Show(mensaje_err,"AVISO");
			}
		}
		
		public bool comprobar_libro(String no_libro){
			int i=0;
			bool existe=false;
			while(i<lista_prohibida.Rows.Count){
				if(no_libro.Equals(lista_prohibida.Rows[i][0].ToString())){
					existe=true;
					if(id_inicio.ToString().Equals(lista_prohibida.Rows[i][3].ToString())){
						existe=false;
					}
				}
				i++;
			}
			
			return existe;
		}
		
		public bool comprobar_caso(int no_caso){
			return true;
		}
		
		void ResponsablesLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			if (tipo_inicio==2){
				llena_cb1_edicion();
			}else{
				llena_cb1();
			}
			
			llena_lista_prohi();
			if(tipo_inicio==2){
				
				consulta_respon();
			}else{
				if(lista_prohibida.Rows.Count>0){
					textBox1.Text=""+(Convert.ToInt32(lista_prohibida.Rows[0][0].ToString())+1);
					textBox2.Text=""+(Convert.ToInt32(lista_prohibida.Rows[0][2].ToString())+1);
				}else{
					textBox1.Text="1";
					textBox2.Text="1";
				}
			}
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if(tipo_inicio==1){
				ingresar();
			}else{
				actualizar();
			}
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				textBox2.Focus();
			}
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				textBox3.Focus();
			}
		}
		
		void TextBox3KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				textBox4.Focus();
			}
		}
		
		void TextBox1Leave(object sender, EventArgs e)
		{
			int num=0;
			if(int.TryParse(textBox1.Text,out num)==false){
				textBox1.BackColor=Color.Red;
			}else{
				if(comprobar_libro(textBox1.Text)==false){
					textBox1.BackColor=SystemColors.Window;
				}else{
					textBox1.BackColor=Color.Red;
				}
			}
		}
		
		void TextBox2Leave(object sender, EventArgs e)
		{
			int num=0,n1,n2,resu=0;
			if(int.TryParse(textBox2.Text,out num)==false){
				textBox2.BackColor=Color.Red;
				textBox4.Text="";
			}else{
				textBox2.BackColor=SystemColors.Window;
				if((textBox3.BackColor==SystemColors.Window)&&(textBox3.Text.Length>0)){
					n1=Convert.ToInt32(textBox2.Text);
					n2=Convert.ToInt32(textBox3.Text);
					resu=(n1+n2)-1;
					textBox4.Text=""+resu;
				}
			}
		}
		
		void TextBox3Leave(object sender, EventArgs e)
		{
			int num=0,n1,n2,resu=0;
			if(int.TryParse(textBox3.Text,out num)==false){
				textBox3.BackColor=Color.Red;
				textBox4.Text="";
			}else{
				textBox3.BackColor=SystemColors.Window;
				if((textBox1.BackColor==SystemColors.Window)&&(textBox2.BackColor==SystemColors.Window)){
					n1=Convert.ToInt32(textBox2.Text);
					n2=Convert.ToInt32(textBox3.Text);
					resu=(n1+n2)-1;
					textBox4.Text=""+resu;
				}
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			int num=0;
			if(int.TryParse(textBox1.Text,out num)==false){
				textBox1.BackColor=Color.Red;
			}else{
				if(comprobar_libro(textBox1.Text)==false){
					textBox1.BackColor=SystemColors.Window;
				}else{
					textBox1.BackColor=Color.Red;
				}
			}
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			int num=0,n1,n2,resu=0;
			if(int.TryParse(textBox2.Text,out num)==false){
				textBox2.BackColor=Color.Red;
				textBox4.Text="";
			}else{
				textBox2.BackColor=SystemColors.Window;
				if((textBox3.BackColor==SystemColors.Window)&&(textBox3.Text.Length>0)){
					n1=Convert.ToInt32(textBox2.Text);
					n2=Convert.ToInt32(textBox3.Text);
					resu=(n1+n2)-1;
					textBox4.Text=""+resu;
				}
			}
		}
		
		void TextBox3TextChanged(object sender, EventArgs e)
		{
			int num=0,n1,n2,resu=0;
			if(int.TryParse(textBox3.Text,out num)==false){
				textBox3.BackColor=Color.Red;
				textBox4.Text="";
			}else{
				textBox3.BackColor=SystemColors.Window;
				if(textBox2.BackColor==SystemColors.Window){
					n1=Convert.ToInt32(textBox2.Text);
					n2=Convert.ToInt32(textBox3.Text);
					resu=(n1+n2)-1;
					textBox4.Text=""+resu;
				}
			}
		}
		
		void TextBox4TextChanged(object sender, EventArgs e)
		{
			int num=0,n1,n2,resu=0;
			if(int.TryParse(textBox4.Text,out num)==false){
				textBox4.BackColor=Color.Red;
				textBox3.Text="";
			}else{
				textBox4.BackColor=SystemColors.Window;
				if(textBox2.BackColor==SystemColors.Window){
					n1=Convert.ToInt32(textBox2.Text);
					n2=Convert.ToInt32(textBox4.Text);
					resu=(n2-n1)+1;
					textBox3.Text=""+resu;
				}
			}
		}
		
		void TextBox4Leave(object sender, EventArgs e)
		{
			
		}
		
		void TextBox4KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				button6.Focus();
			}
		}
		
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 28/12/2016
 * Hora: 10:25 a.m.
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

namespace Nova_Gear.Oficios
{
	/// <summary>
	/// Description of Oficios_ingreso.
	/// </summary>
	public partial class Oficios_ingreso : Form
	{
		public Oficios_ingreso()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		//Conexion MySQL
        Conexion conex = new Conexion();
        Conexion conex1 = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        DataTable periodos = new DataTable();
       	DataTable sectores = new DataTable();
       	DataTable notificadores = new DataTable();
       	DataTable controladores = new DataTable();
        DataTable ultimo_id = new DataTable();
        DataTable noms_docs = new DataTable();
       	
        int i=0;
        
        public void llenar_cb1(){
        	conex.conectar("base_principal");
        	comboBox1.Items.Clear();
        	i=0;
        	periodos= conex.consultar("SELECT DISTINCT periodo_oficio FROM oficios ORDER BY periodo_oficio ASC");
        	while(i< periodos.Rows.Count){
        		comboBox1.Items.Add(periodos.Rows[i][0].ToString());
        		i++;
        	}
        	comboBox1.SelectedIndex=(comboBox1.Items.Count-1);

            ultimo_id = conex.consultar("SELECT id_oficios FROM base_principal.oficios ORDER BY id_oficios DESC LIMIT 1");
            textBox8.Text = ((Convert.ToInt32(ultimo_id.Rows[0][0].ToString())) + 1).ToString();
        	conex.cerrar();
        	
        }
        
        public void llenar_cb2(){
        	conex.conectar("base_principal");
        	comboBox2.Items.Clear();
        	i=0;
        	noms_docs= conex.consultar("SELECT DISTINCT nombre_oficio FROM oficios ORDER BY nombre_oficio");
        	while(i < noms_docs.Rows.Count){
        		comboBox2.Items.Add(noms_docs.Rows[i][0].ToString().ToUpper());
        		i++;
        	}
        	conex.cerrar();
        }
        
        public void llenar_sectores_notificadores(){
        	conex.conectar("base_principal");
        	conex1.conectar("base_principal");
        	conex2.conectar("base_principal");
        	sectores=conex.consultar("SELECT * FROM sectores");
        	notificadores=conex1.consultar("SELECT id_usuario,apellido,nombre FROM usuarios WHERE controlador <> 0 ORDER BY apellido ASC");
        	controladores=conex2.consultar("SELECT id_usuario,apellido,nombre FROM usuarios WHERE puesto=\"controlador\"");
        	conex.cerrar();
        	conex1.cerrar();
        	conex2.cerrar();
        }
        
        public String verificar_fecha(String fecha){
			
			DateTime fecha_not,fecha_min;
			TimeSpan dif_fecha;
			
			if(DateTime.TryParse(fecha,out fecha_not)){
				
				fecha_min=System.DateTime.Today.Date;
				dif_fecha=fecha_min.Subtract(fecha_not);
				//MessageBox.Show(""+dif_fecha);
				if(fecha_not <= System.DateTime.Today && dif_fecha.Days <= 1826 ){
					return fecha_not.ToShortDateString();
				}else{
					return "0";
				}
			}else{
				return "0";
			}
		}
        
		void Oficios_ingresoLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			llenar_cb1();
			llenar_cb2();
			llenar_sectores_notificadores();
			
            textBox1.Focus();
			//MessageBox.Show("periodos:"+periodos.Rows.Count+" sectores: "+sectores.Rows.Count+" notificadores: "+notificadores.Rows.Count+" controladores: "+controladores.Rows.Count);
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			int llave=0;
			String mensaje_error="",sql="",fecha="",fecha_captura="",ccjal="",id_of="";

			mensaje_error="Los Siguientes campos estan vacíos o con muy poca informacion:\n\n";
			
			if(textBox1.Text.Length>9){
				llave++;
			}else{
				mensaje_error+="•Registro Patronal/N.S.S.\n";
			}
			
			if(textBox2.Text.Length>9){
				llave++;
			}else{
				mensaje_error+="•Razon Social/Nombre\n";
			}
			
			if(textBox9.Text.Length>9){
				llave++;
			}else{
				mensaje_error+="•Domicilio\n";
			}
			
			if(textBox10.Text.Length>9){
				llave++;
			}else{
				mensaje_error+="•Localidad\n";
			}
			
			if(textBox11.Text.Length>3){
				llave++;
			}else{
				mensaje_error+="•C.P. (Código Postal)\n";
			}
			
			if(textBox12.Text.Length>3){
				llave++;
			}else{
				mensaje_error+="•R.F.C.(Registro Federal de Contribuyentes)\n";
			}
			
			if(textBox3.Text.Length>3){
				llave++;
			}else{
				mensaje_error+="•Folio\n";
			}
			
			if(textBox4.Text.Length>3){
				llave++;
			}else{
				mensaje_error+="•Acuerdo\n";
			}
			
			if(!maskedTextBox2.BackColor.Name.Equals("Red")){
				llave++;
			}else{
				mensaje_error+="•Fecha Oficio\n";
			}
			
			if(comboBox2.Text.Length>4){
				llave++;
			}else{
				mensaje_error+="•Nombre Documento\n";
			}
			
			if(textBox5.Text.Length>9){
				llave++;
			}else{
				mensaje_error+="•Emisor\n";
			}
			
			if(maskedTextBox1.Text.Length>1){
				llave++;
			}else{
				mensaje_error+="•Sector\n";
			}
			
			if(textBox6.Text.Length>5){
				llave++;
			}else{
				mensaje_error+="•Sector Incorrecto\n";
			}
			
			if(textBox7.Text.Length>5){
				llave++;
			}else{
				
			}
			
			if(comboBox1.SelectedIndex>-1){
				llave++;
			}else{
				mensaje_error+="•Periodo\n";
			}
			
			
			if(llave>13){
				
				if(checkBox1.Checked==true){
					ccjal="CCJAL";
				}else{
					ccjal="-";
				}
				
				//fecha=dateTimePicker1.Text;
				fecha=maskedTextBox2.Text;
				fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
				fecha_captura=System.DateTime.Today.Year.ToString()+"-"+System.DateTime.Today.Month.ToString()+"-"+System.DateTime.Today.Day.ToString();
				conex3.conectar("base_principal");
				sql="INSERT INTO oficios (reg_nss,razon_social,folio,acuerdo,fecha_oficio,emisor,sector,receptor,controlador,periodo_oficio,fecha_captura,ccjal,estatus,domicilio_oficio,localidad_oficio,cp_oficio,rfc_oficio,nombre_oficio) VALUES "+
					"(\""+textBox1.Text+"\",\""+textBox2.Text+"\",\""+textBox3.Text+"\",\""+textBox4.Text+"\",\""+fecha+"\",\""+textBox5.Text+"\",\""+maskedTextBox1.Text+"\",\""+textBox6.Text+"\",\""+textBox7.Text+"\",\""+comboBox1.SelectedItem.ToString()+"\",\""+fecha_captura+"\",\""+ccjal+"\",\"0\",\""+textBox9.Text+"\",\""+textBox10.Text+"\",\""+textBox11.Text+"\",\""+textBox12.Text+"\",\""+comboBox2.Text+"\")";
				
				conex3.consultar(sql);
                ultimo_id = conex3.consultar("SELECT id_oficios FROM base_principal.oficios ORDER BY id_oficios DESC LIMIT 1");
                id_of=ultimo_id.Rows[0][0].ToString();
                conex3.guardar_evento("SE CAPTURÓ EL OFICIO CON EL ID: "+id_of);
                
                MessageBox.Show("Registro Añadido Correctamente.\nID: " + id_of, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox8.Text = ((Convert.ToInt32(id_of)) + 1).ToString();

				textBox1.Text="";
				//checkBox1.Checked=false;
				textBox2.Text="";
				textBox3.Text="";
				textBox4.Text="";
				if(checkBox1.Checked==false){
					textBox5.Text="";
				}
				textBox6.Text="";
				textBox7.Text="";
				textBox9.Text="";
				textBox10.Text="";
				textBox11.Text="";
				textBox12.Text="";
				comboBox2.Text="";
				comboBox2.SelectedIndex=-1;
				maskedTextBox1.Text="";
				maskedTextBox2.Clear();
				dateTimePicker1.Text=System.DateTime.Today.ToShortDateString();
				textBox1.Focus();
			
			}else{
				MessageBox.Show(mensaje_error,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void TextBox3TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void TextBox4TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void TextBox5TabIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.Text.Length==2){
				button2.Focus();
			}
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox2.Focus();
			}
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox9.Focus();
			}
		}
		
		void TextBox3KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox4.Focus();
			}
		}
		
		void TextBox4KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox2.Focus();
			}
		}
		
		void DateTimePicker1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox5.Focus();
			}
		}
		
		void TextBox5KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox1.Focus();
			}
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				button2.Focus();
			}
		}
		
		void MaskedTextBox1Leave(object sender, EventArgs e)
		{
			String sector,id_not="",id_contro="",receptor="",controlador="";
			int i=0;
			
			sector=maskedTextBox1.Text;
			if(sector.Length==1){
				sector="0"+sector;
				
			}
			
			maskedTextBox1.Text=sector;
			textBox6.Text="";
			textBox7.Text="";
			i=0;
			if(sector.Length>0){
				while(i<sectores.Rows.Count){
					if(Convert.ToInt32(sector) == Convert.ToInt32(sectores.Rows[i][3].ToString())){
						id_contro=sectores.Rows[i][1].ToString();
						id_not=sectores.Rows[i][2].ToString();
						i=sectores.Rows.Count;
					}
					i++;
				}
			}else{
				textBox6.Text="";
				textBox7.Text="";
			}
			
			if(id_not.Length>0 && id_contro.Length>0){
			
				i=0;
				while(i<notificadores.Rows.Count){
					if(id_not.Equals(notificadores.Rows[i][0].ToString())){
						receptor=notificadores.Rows[i][1].ToString()+" "+notificadores.Rows[i][2].ToString();
						i=notificadores.Rows.Count;
					}
					i++;
				}
				
				i=0;
				while(i<controladores.Rows.Count){
					if(id_contro.Equals(controladores.Rows[i][0].ToString())){
						controlador=controladores.Rows[i][1].ToString()+" "+controladores.Rows[i][2].ToString();
						i=notificadores.Rows.Count;
					}
					i++;
				}
				
				textBox6.Text=receptor;
				textBox7.Text=controlador;
				
			}else{
				if(sector.Equals("72")){
					textBox6.Text="SUBDELEGACION LIBERTAD-REFORMA";
					textBox7.Text="SUBDELEGACION LIBERTAD-REFORMA";
				}else{
					if(sector.Equals("73")){
						textBox6.Text="SUBDELEGACION JUAREZ";
						textBox7.Text="SUBDELEGACION JUAREZ";
					}else{
						if(sector.Equals("76")){
							textBox6.Text="FORANEOS";
							textBox7.Text="FORANEOS";
						}else{
							
						}
					}
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			String per_of,anterior;
			
			anterior=comboBox1.Items[comboBox1.Items.Count-1].ToString();
			anterior=(Convert.ToInt32(anterior.Substring(8,(anterior.Length-8)))+1).ToString();
			
			per_of="OFICIOS_"+anterior;
			comboBox1.Items.Add(per_of);
			comboBox1.SelectedIndex=(comboBox1.Items.Count-1);
			button1.Enabled=false;
			button2.Focus();
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			textBox1.Text="";
			textBox2.Text="";
			textBox3.Text="";
			textBox4.Text="";
			textBox5.Text="";
			textBox6.Text="";
			textBox7.Text="";
			textBox9.Text="";
			textBox10.Text="";
			textBox11.Text="";
			textBox12.Text="";
			comboBox2.Text="";
			comboBox2.SelectedIndex=-1;
			maskedTextBox1.Text="";
			dateTimePicker1.Text=System.DateTime.Today.ToShortDateString();
			textBox1.Focus();
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true){
				checkBox1.BackColor=System.Drawing.Color.DodgerBlue;
				textBox5.Text="INCONFORMIDADES";
			}else{
				checkBox1.BackColor=System.Drawing.Color.Transparent;
				textBox5.Text="";
			}
			
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.Text.Length==10){
				if(verificar_fecha(maskedTextBox2.Text).Length>1){
					maskedTextBox2.Text=verificar_fecha(maskedTextBox2.Text);
					maskedTextBox2.BackColor=System.Drawing.SystemColors.Window;
					maskedTextBox2.ForeColor=System.Drawing.SystemColors.ControlText;
				}else{
					maskedTextBox2.BackColor=System.Drawing.Color.Red;
					maskedTextBox2.ForeColor=System.Drawing.Color.White;
				}
			}
		}
		
		void MaskedTextBox2Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox2.Text).Length>1){
				maskedTextBox2.Text=verificar_fecha(maskedTextBox2.Text);
				maskedTextBox2.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox2.ForeColor=System.Drawing.SystemColors.ControlText;
			}else{
				maskedTextBox2.BackColor=System.Drawing.Color.Red;
				maskedTextBox2.ForeColor=System.Drawing.Color.White;
			}
			
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				comboBox2.Focus();
			}
		}
		
		void TextBox9KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox11.Focus();
			}
		}
		
		void TextBox11KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox10.Focus();
			}
		}
		
		void TextBox10KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox12.Focus();
			}
		}
		
		void TextBox12KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox3.Focus();
			}
		}
		
		void ComboBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				textBox5.Focus();
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			textBox5.Focus();
		}
		
		void ComboBox2MouseHover(object sender, EventArgs e)
		{
			//toolTip1.SetToolTip(comboBox2, comboBox2.SelectedItem.ToString());
		}
	}
}
/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 25/07/2016
 * Hora: 11:15 a.m.
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

namespace Nova_Gear.Mod40
{
	/// <summary>
	/// Description of Insertar_40.
	/// </summary>
	public partial class Insertar_40 : Form
	{
		public Insertar_40()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String nss_pago,fecha,per_cont,per_count_par,modalidad,per_ini_log,just40,just10,per_act;
		int per_ini_anio=0,per_ini_mes=0,per_fin=0,i=0;	
		decimal importe=0;		
		
		//Conexion MySQL
        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
		
		void Insertar_40Load(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			dateTimePicker1.Value=System.DateTime.Today;
			maskedTextBox1.Text=per_act.Substring(0,4);
			
			switch(per_act.Substring(4,2)){
				case "01":comboBox1.SelectedIndex=0;
				break;
				case "02":comboBox1.SelectedIndex=0;
				break;
				case "03":comboBox1.SelectedIndex=1;
				break;
				case "04":comboBox1.SelectedIndex=1;
				break;
				case "05":comboBox1.SelectedIndex=2;
				break;
				case "06":comboBox1.SelectedIndex=2;
				break;
				case "07":comboBox1.SelectedIndex=3;
				break;
				case "08":comboBox1.SelectedIndex=3;
				break;
				case "09":comboBox1.SelectedIndex=4;
				break;
				case "10":comboBox1.SelectedIndex=4;
				break;
				case "11":comboBox1.SelectedIndex=5;
				break;
				case "12":comboBox1.SelectedIndex=5;
				break;
			}
		}
		
		public bool existe_pago(String per,String nss){
			conex3.conectar("base_principal");
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(periodo_pago) FROM base_principal.mod40_sua WHERE periodo_pago=\""+per+"\" AND nss=\""+nss_pago+"\"");
			if(Convert.ToInt32(dataGridView2.Rows[0].Cells[0].FormattedValue.ToString())<=1){
				return false;
			}else{
				return true;
			}
		}
		
		public void insertar_en_bd40(){
			
			//try{
			maskedTextBox1.Enabled=false;
			maskedTextBox2.Enabled=false;
			comboBox1.Enabled=false;
			comboBox2.Enabled=false;
			comboBox3.Enabled=false;
			textBox1.Enabled=false;
			textBox3.Enabled=false;
			textBox4.Enabled=false;
			dateTimePicker1.Enabled=false;
			button4.Enabled=false;
			
			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			dataGridView1.DataSource=conex.consultar("SELECT del,sub,registro_patronal,nss,rfc,curp,nombre_trabajador FROM mod40_sua WHERE nss=\""+nss_pago+"\"");
			
			fecha=dateTimePicker1.Value.ToShortDateString();
			fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
			
			per_ini_anio=Convert.ToInt32(maskedTextBox1.Text);
			per_ini_mes=Convert.ToInt32(comboBox1.SelectedItem.ToString());
			per_fin=Convert.ToInt32((maskedTextBox2.Text+comboBox2.SelectedItem.ToString()));
			
			per_cont=maskedTextBox1.Text+comboBox1.SelectedItem.ToString();
			per_count_par=per_cont;
			per_ini_log=per_cont;
			
			if(modalidad=="10"){
				fecha="1010-10-10";
				just40="-";
				if(comboBox3.SelectedIndex==0){
					just10="10";
				}
				if(comboBox3.SelectedIndex==1){
					just10="0";
				}
				if(comboBox3.SelectedIndex==2){
					just10="0/10";
				}
			}else{
				if(modalidad=="40"){
					if(textBox1.Text.Length>15){
						just10=modalidad;
						just40=textBox1.Text;
					}
				}
			}
			
			if(per_cont.Equals(per_fin.ToString())){
				if(existe_pago(per_cont,nss_pago) == false){
					conex2.consultar("INSERT INTO mod40_sua (del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago,folio_sua,fecha_pago,importe_pago,tipo_modalidad,observaciones) VALUES ("+
					                 "\""+dataGridView1.Rows[0].Cells[0].FormattedValue.ToString()+"\"," +//del
					                 "\""+dataGridView1.Rows[0].Cells[1].FormattedValue.ToString()+"\"," +//sub
					                 "\"C0000000\","+												     //cve_unit
					                 "\""+dataGridView1.Rows[0].Cells[2].FormattedValue.ToString()+"\"," +//registro_patronal
					                 "\""+dataGridView1.Rows[0].Cells[3].FormattedValue.ToString()+"\"," +//nss
					                 "\""+dataGridView1.Rows[0].Cells[4].FormattedValue.ToString()+"\"," +//rfc
					                 "\""+dataGridView1.Rows[0].Cells[5].FormattedValue.ToString()+"\"," +//curp
					                 "\""+dataGridView1.Rows[0].Cells[6].FormattedValue.ToString()+"\"," +//nombre_trabajador
					                 "30,"+								      							 //dias_cotizados
					                 "000,"+								      							 //pza_sucursal
					                 "\""+per_cont+"\","+							 					 //periodo
					                 "\""+textBox4.Text.ToString()+"\","+								 //folio
					                 "\""+fecha+"\","+								 					 //fecha
					                 "\""+textBox3.Text.ToString()+"\","+								 //importe
					                 "\""+just10+"\","+								 				 	 //tipo_modalidad
					                 "\""+just40+"\""+												 	 //observaciones
					                 ")");
				}else{
					MessageBox.Show("El asegurado ya cuenta con un pago del periodo: "+per_cont+".","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}else{
				//MessageBox.Show(""+per_cont+"|"+per_fin);
				if(Convert.ToInt32(per_cont) < per_fin){

					do{
						if(existe_pago(per_count_par,nss_pago) == false){
							conex2.consultar("INSERT INTO mod40_sua(del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago,folio_sua,fecha_pago,importe_pago,tipo_modalidad,observaciones) VALUES ("+
							                 "\""+dataGridView1.Rows[0].Cells[0].FormattedValue.ToString()+"\"," +//del
							                 "\""+dataGridView1.Rows[0].Cells[1].FormattedValue.ToString()+"\"," +//sub
							                 "\"C0000000\","+														 //cve_unit
							                 "\""+dataGridView1.Rows[0].Cells[2].FormattedValue.ToString()+"\"," +//registro_patronal
							                 "\""+dataGridView1.Rows[0].Cells[3].FormattedValue.ToString()+"\"," +//nss
							                 "\""+dataGridView1.Rows[0].Cells[4].FormattedValue.ToString()+"\"," +//rfc
							                 "\""+dataGridView1.Rows[0].Cells[5].FormattedValue.ToString()+"\"," +//curp
							                 "\""+dataGridView1.Rows[0].Cells[6].FormattedValue.ToString()+"\"," +//nombre_trabajador
							                 "30,"+								      							 //dias_cotizados
							                 "000,"+								      							 //pza_sucursal
							                 "\""+per_count_par+"\","+							 			     //periodo
							                 "\""+textBox4.Text.ToString()+"\","+								 //folio
							                 "\""+fecha+"\","+								 					 //fecha
							                 "\""+textBox3.Text.ToString()+"\","+								 //importe_pago
							                 "\""+just10+"\","+								 				 	 //tipo_modalidad
							                 "\""+just40+"\""+												 	 //observaciones
							                 ")");
							
							}else{
							MessageBox.Show("El asegurado ya cuenta con un pago del periodo: "+per_cont+".","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
							}
							//aumentar contador
							per_ini_mes++;
							if(per_ini_mes>12){
								per_ini_mes=1;
								per_ini_anio++;
							}
							
							if(per_ini_mes<=9){
								per_cont=per_ini_anio.ToString()+"0"+per_ini_mes.ToString();
							}else{
								per_cont=per_ini_anio.ToString()+per_ini_mes.ToString();
							}
							
							if((per_ini_mes==2)||(per_ini_mes==4)||(per_ini_mes==6)||(per_ini_mes==8)||(per_ini_mes==10)||(per_ini_mes==12)){
								per_count_par=per_cont;
							}
						
					}while(Convert.ToInt32(per_cont)<=per_fin);
					
					if(existe_pago(per_count_par,nss_pago) == false){
						//PAGO FINAL ** NO BORRAR
						conex2.consultar("INSERT INTO mod40_sua(del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago,folio_sua,fecha_pago,importe_pago,tipo_modalidad,observaciones) VALUES ("+
						                 "\""+dataGridView1.Rows[0].Cells[0].FormattedValue.ToString()+"\"," +//del
						                 "\""+dataGridView1.Rows[0].Cells[1].FormattedValue.ToString()+"\"," +//sub
						                 "\"C0000000\","+													 //cve_unit
						                 "\""+dataGridView1.Rows[0].Cells[2].FormattedValue.ToString()+"\"," +//registro_patronal
						                 "\""+dataGridView1.Rows[0].Cells[3].FormattedValue.ToString()+"\"," +//nss
						                 "\""+dataGridView1.Rows[0].Cells[4].FormattedValue.ToString()+"\"," +//rfc
						                 "\""+dataGridView1.Rows[0].Cells[5].FormattedValue.ToString()+"\"," +//curp
						                 "\""+dataGridView1.Rows[0].Cells[6].FormattedValue.ToString()+"\"," +//nombre_trabajador
						                 "30,"+								      							 //dias_cotizados
						                 "000,"+								      							 //pza_sucursal
						                 "\""+per_count_par+"\","+							 			     //periodo
						                 "\""+textBox4.Text.ToString()+"\","+								 //folio
						                 "\""+fecha+"\","+								 					 //fecha
						                 "\""+textBox3.Text.ToString()+"\","+								 //importe
						                 "\""+just10+"\","+								 				 	 //tipo_modalidad
						                 "\""+just40+"\""+												 	 //observaciones
						                 ")");
					}else{
						MessageBox.Show("El asegurado ya cuenta con un pago del periodo: "+per_cont+".","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
					
				}else{
					MessageBox.Show("Periodos Ingresados Incorrectamente","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}
			
			if(per_cont.Equals(per_fin.ToString())){
				conex2.guardar_evento("Se Agregó información de pago del periodo: "+per_ini_log+" al NSS: "+dataGridView1.Rows[0].Cells[3].FormattedValue.ToString()+" de la modalidad: "+modalidad );
			}else{
				conex2.guardar_evento("Se Agregó información de pago de los periodos: "+per_ini_log+" - "+per_fin+ " al NSS: "+dataGridView1.Rows[0].Cells[3].FormattedValue.ToString()+" de la modalidad: "+modalidad );
			}
			
			MessageBox.Show("¡Pago ingresado exitosamente!","EXITO");
			
			maskedTextBox1.Enabled=true;
			maskedTextBox1.Clear();
			comboBox1.Enabled=true;
			comboBox1.SelectedIndex=-1;
			textBox4.Enabled=true;
			textBox4.Text="";
			button4.Enabled=true;
			
			if(modalidad.Equals("10")){
				maskedTextBox2.Enabled=true;
				maskedTextBox2.Clear();
				comboBox2.Enabled=true;
				comboBox2.SelectedIndex=-1;
				comboBox3.Enabled=true;
				comboBox3.SelectedIndex=-1;
				dateTimePicker1.Enabled=false;
				
			}else{
				if(modalidad.Equals("40")){
					textBox1.Enabled=true;
					textBox1.Text="";
					textBox3.Enabled=true;
					textBox3.Text="";
					textBox4.Enabled=true;
					textBox4.Text="";
					dateTimePicker1.Enabled=true;
					dateTimePicker1.Value=System.DateTime.Today;
				}
			}
			
			
			//	}catch(Exception es){
			//		MessageBox.Show("Ocurrió el siguiente error al intentar ingresar el pago: \n\n"+es,"ERROR");
			//	}
			
			
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			DialogResult re=MessageBox.Show("Se ingresará nueva información de pago de modalidad "+modalidad+".\n¿Desea Continuar?\n\n", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(re == DialogResult.Yes){
				
				insertar_en_bd40();
			}
		
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void Label9Click(object sender, EventArgs e)
		{
			
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void Label3Click(object sender, EventArgs e)
		{
			
		}
		
		public void regresar(String nss,String tipo,String per_act1){
			this.nss_pago= nss;
			this.per_act=per_act1;
			if(tipo.Equals("10")){
				textBox3.Enabled=false;
				modalidad="10";
				maskedTextBox2.Enabled=true;
				comboBox2.Enabled=true;
				this.Text="Nova Gear: Modalidad 40 - Captura de periodo de Modalidad 10";
				this.Height=165;
				textBox1.Visible=false;
				textBox4.Text="00000";
				textBox4.Enabled=false;
				dateTimePicker1.Enabled=false;
			}else{
				if(tipo.Equals("40")){
					textBox3.Enabled=true;
					modalidad="40";
					maskedTextBox2.Enabled=false;
					comboBox2.Enabled=false;
					this.Text="Nova Gear: Modalidad 40 - Captura de pagos de la Modalidad 40";
					this.Height=240;
					comboBox3.Visible=false;
				}
			}
		}
			
		void TextBox3TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void MaskedTextBox1MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			maskedTextBox2.Text=maskedTextBox1.Text;
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			i=0;
			
			if(maskedTextBox1.Text.Length==4){
				i=i+1;
			}
			
			if(maskedTextBox2.Text.Length==4){
				i=i+1;
			}
			
			if(comboBox1.SelectedIndex>=0){
				i=i+1;
			}
			
			if(comboBox2.SelectedIndex>=0){
				i=i+1;
			}
			
			if(textBox3.Text.Length>0){
				if(decimal.TryParse(textBox3.Text, out importe)){
					i=i+1;
				}
			}
			
			if(textBox4.Text.Length>0){
				i=i+1;
			}
			
			if(modalidad=="10"){
				if(comboBox3.SelectedIndex>=0){
					i=i+1;
				}
			
			}else{
				if(modalidad=="40"){
					if(textBox1.Text.Length>15){
						i=i+1;
					}
				}
			}
			
			
			if(i>=7){
				button4.Enabled=true;
			}else{
				button4.Enabled=false;
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox2.SelectedIndex=comboBox1.SelectedIndex;			
		}
		
		void TextBox3Enter(object sender, EventArgs e)
		{
				textBox3.Text="";
		}
		
		void TextBox4Leave(object sender, EventArgs e)
		{
			textBox4.Text=textBox4.Text.ToUpper();
		}
	}
}

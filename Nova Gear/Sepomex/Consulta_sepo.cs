/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 08/05/2018
 * Hora: 01:19 p. m.
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

namespace Nova_Gear.Sepomex
{
	/// <summary>
	/// Description of Consulta_sepo.
	/// </summary>
	public partial class Consulta_sepo : Form
	{
		public Consulta_sepo()
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
		DataTable consulta = new DataTable();
		DataTable consulta_notifs = new DataTable();
		
		public void llena_cb3(){
			int i=0;
			comboBox3.Items.Clear();
			consulta_notifs=conex2.consultar("SELECT apellido,nombre,controlador FROM usuarios WHERE puesto=\"Notificador\" and estatus=\"activo\" ORDER BY apellido");
			while(i<consulta_notifs.Rows.Count){
				comboBox3.Items.Add(consulta_notifs.Rows[i][0].ToString()+ " "+consulta_notifs.Rows[i][1].ToString());
				i++;
			}
		}
		
		public void buscar(){
			String filtro="",valor="";
			if(comboBox1.SelectedIndex>-1 && textBox1.Text.Length > 0){
				
				if(comboBox1.SelectedItem.ToString()=="Registro Patronal"){
					filtro="registro_patronal";
					valor=textBox1.Text;
				}
				
				if(comboBox1.SelectedItem.ToString()=="Folio"){
					filtro="folio";
					
					if(textBox1.Text.Length==1){
						valor="00000"+textBox1.Text;
					}
					
					if(textBox1.Text.Length==2){
						valor="0000"+textBox1.Text;
					}
					
					if(textBox1.Text.Length==3){
						valor="000"+textBox1.Text;
					}
					
					if(textBox1.Text.Length==4){
						valor="00"+textBox1.Text;
					}
					
					if(textBox1.Text.Length==5){
						valor="0"+textBox1.Text;
					}
					
					if(textBox1.Text.Length==6){
						valor=textBox1.Text;
					}
					
					if(textBox1.Text.Length==19){
						valor=textBox1.Text.Substring((textBox1.Text.Length-7),6);
					}
				}
				
				if(comboBox1.SelectedItem.ToString()=="Crédito"){
					filtro="credito_ema";
					valor=textBox1.Text;
				}
				
				dataGridView1.DataSource=conex.consultar("SELECT registro_patronal,credito_ema,folio,periodo_ema,razon_social,status,fecha_entrega,fecha_recepcion,fecha_notificacion,notificador,observaciones,id_ema_sepomex FROM ema_sepomex WHERE "+filtro+"=\""+valor+"\" ");
				
				dataGridView1.Columns[0].HeaderText="Registro Patronal";
				dataGridView1.Columns[1].HeaderText="Crédito";
				dataGridView1.Columns[2].HeaderText="Folio";
				dataGridView1.Columns[3].HeaderText="Periodo";
				dataGridView1.Columns[4].HeaderText="Razón Social";
				dataGridView1.Columns[4].MinimumWidth=250;
				dataGridView1.Columns[5].Visible=false;
				dataGridView1.Columns[6].Visible=false;
				dataGridView1.Columns[7].Visible=false;
				dataGridView1.Columns[8].Visible=false;
				dataGridView1.Columns[9].Visible=false;
				dataGridView1.Columns[10].Visible=false;
				dataGridView1.Columns[11].Visible=false;
				
				if(dataGridView1.RowCount>0){
					button3.Enabled=true;
					textBox1.Clear();
					timer1.Enabled=true;
					groupBox2.Visible=false;
					button2.Enabled=false;
				}else{
					button3.Enabled=false;
				}
				
				label25.Text="Casos: "+dataGridView1.RowCount;
				label25.Refresh();
			}
		}
		
		public void buscar_id(String id){
			
			dataGridView1.DataSource=conex.consultar("SELECT registro_patronal,credito_ema,folio,periodo_ema,razon_social,status,fecha_entrega,fecha_recepcion,fecha_notificacion,notificador,observaciones,id_ema_sepomex FROM ema_sepomex WHERE id_ema_sepomex=\""+id+"\" ");
				
				dataGridView1.Columns[0].HeaderText="Registro Patronal";
				dataGridView1.Columns[1].HeaderText="Crédito";
				dataGridView1.Columns[2].HeaderText="Folio";
				dataGridView1.Columns[3].HeaderText="Periodo";
				dataGridView1.Columns[4].HeaderText="Razón Social";
				dataGridView1.Columns[4].MinimumWidth=250;
				dataGridView1.Columns[5].Visible=false;
				dataGridView1.Columns[6].Visible=false;
				dataGridView1.Columns[7].Visible=false;
				dataGridView1.Columns[8].Visible=false;
				dataGridView1.Columns[9].Visible=false;
				dataGridView1.Columns[10].Visible=false;
				dataGridView1.Columns[11].Visible=false;
				
				if(dataGridView1.RowCount>0){
					button3.Enabled=true;
					textBox1.Clear();
					timer1.Enabled=true;
					groupBox2.Visible=false;
					button2.Enabled=false;
				}else{
					button3.Enabled=false;
				}
				
				label25.Text="Casos: "+dataGridView1.RowCount;
				label25.Refresh();
		}
		
		public String re_ordena_fechs(String fecha){
			fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
			return fecha;
		}
		
		void GroupBox1Enter(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex>-1){
				textBox1.Focus();
			}
		}
		
		void Consulta_sepoLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			llena_cb3();
			
			String rango;
			rango = MainForm.datos_user_static[2];
			
			if(Convert.ToInt32(rango)<3){
				button3.Enabled=true;
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			buscar();
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				buscar();
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			if(textBox1.Text.Length==19){
				buscar();
			}
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			try{
				textBox2.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
				textBox3.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
				textBox4.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString();
				textBox5.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value.ToString();
				textBox6.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value.ToString();
				textBox7.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[5].Value.ToString();
				
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString().Length>9){
					textBox8.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString().Substring(0,10);
				}else{
					textBox8.Text="-";
				}
				
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString().Length>9){
					textBox9.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString().Substring(0,10);
				}else{
					textBox9.Text="-";
				}
				
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString().Length>9){
					textBox10.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString().Substring(0,10);
				}else{
					textBox10.Text="-";
				}
				
				textBox11.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[9].Value.ToString();
				textBox12.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[10].Value.ToString();
				
				//#################################################################################################################
				
				textBox23.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
				textBox22.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
				textBox21.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString();
				textBox20.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value.ToString();
				textBox19.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value.ToString();
				
				switch(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[5].Value.ToString()){
					case "0" : comboBox2.SelectedIndex=0; 
								break;
					case "EN TRAMITE": comboBox2.SelectedIndex=1;
							break;
					case "NOTIFICADO": comboBox2.SelectedIndex=2;
							break;
					case "NN": comboBox2.SelectedIndex=3;
							break;
					
				}
				
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString().Length>9){
					maskedTextBox1.Text=(Convert.ToDateTime(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString().Substring(0,10))).ToShortDateString();
				}else{
					maskedTextBox1.Clear();
				}
				
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString().Length>9){
					maskedTextBox2.Text=(Convert.ToDateTime(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString().Substring(0,10))).ToShortDateString();
				}else{
					maskedTextBox2.Clear();
				}
				
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString().Length>9){
					maskedTextBox3.Text=(Convert.ToDateTime(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString().Substring(0,10))).ToShortDateString();
				}else{
					maskedTextBox3.Clear();
				}
				
				comboBox3.SelectedItem=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[9].Value.ToString();
				if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[9].Value.ToString().Equals("-")==true){
					comboBox3.SelectedIndex=-1;
				}
				textBox13.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[10].Value.ToString();
				
				
			}catch{}
			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(groupBox2.Visible==false){
				groupBox2.Visible=true;
				button2.Enabled=true;
				timer1.Enabled=false;
			}else{
				groupBox2.Visible=false;
				timer1.Enabled=true;
				button2.Enabled=false;
			}
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			try{
				conex3.conectar("base_principal");
				String sql="",sql2="",mensaje="";
				
				sql="UPDATE ema_sepomex SET";
				
				if(!textBox23.Text.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString())){
					sql2+=" registro_patronal=\""+textBox23.Text+"\",";
					mensaje+="•Se Modificará el Registro Patronal\n";					
				}
				
				if(!textBox22.Text.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString())){
					sql2+=" credito_ema=\""+textBox22.Text+"\",";	
					mensaje+="•Se Modificará el Crédito\n";
				}
				
				if(!textBox21.Text.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString())){
					sql2+=" folio=\""+textBox21.Text+"\",";
					mensaje+="•Se Modificará el Folio\n";					
				}
				
				if(!textBox20.Text.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value.ToString())){
					sql2+=" periodo_ema=\""+textBox20.Text+"\",";
					mensaje+="•Se Modificará el Periodo\n";					
				}
				
				if(!textBox19.Text.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value.ToString())){
					sql2+=" razon_social=\""+textBox19.Text+"\",";	
					mensaje+="•Se Modificará la Razón Social\n";
				}
				
				if(!comboBox2.SelectedItem.ToString().Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[5].Value.ToString())){
					sql2+=" status=\""+comboBox2.SelectedItem.ToString()+"\",";	
					mensaje+="•Se Modificará el Status\n";
				}
				
				if(maskedTextBox1.BackColor==Color.LimeGreen){
					if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString().Length>9){
						if(!re_ordena_fechs(maskedTextBox1.Text).Equals(re_ordena_fechs(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString()))){
							sql2+=" fecha_entrega=\""+re_ordena_fechs(maskedTextBox1.Text)+"\",";
							mensaje+="•Se Modificará la Fecha de Entrega\n";
						}
					}else{
						sql2+=" fecha_entrega=\""+re_ordena_fechs(maskedTextBox1.Text)+"\",";
						mensaje+="•Se Modificará la Fecha de Entrega\n";
					}
				}else{
					if(maskedTextBox1.Text.Equals("  /  /")){
						if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value.ToString().Length>1){
							sql2+=" fecha_entrega=NULL,";
							mensaje+="•Se Borrará la Fecha de Entrega\n";
						}
					}
				}
				
				if(maskedTextBox2.BackColor==Color.LimeGreen){
					if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString().Length>9){
						if(!re_ordena_fechs(maskedTextBox2.Text).Equals(re_ordena_fechs(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString()))){
							sql2+=" fecha_recepcion=\""+re_ordena_fechs(maskedTextBox2.Text)+"\",";
							mensaje+="•Se Modificará la Fecha de Recepción\n";
						}
					}else{
						sql2+=" fecha_recepcion=\""+re_ordena_fechs(maskedTextBox2.Text)+"\",";
						mensaje+="•Se Modificará la Fecha de Recepción\n";
					}
				}else{
					if(maskedTextBox2.Text.Equals("  /  /")){
						if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].Value.ToString().Length>1){
							sql2+=" fecha_recepcion=NULL,";
							mensaje+="•Se Borrará la Fecha de Recepción\n";
						}
					}
				}
				
				if(maskedTextBox3.BackColor==Color.LimeGreen){
					if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString().Length>9){
						if(!re_ordena_fechs(maskedTextBox3.Text).Equals(re_ordena_fechs(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString()))){
							sql2+=" fecha_notificacion=\""+re_ordena_fechs(maskedTextBox3.Text)+"\",";
							mensaje+="•Se Modificará la Fecha de Notificación\n";
						}
					}else{
							sql2+=" fecha_notificacion=\""+re_ordena_fechs(maskedTextBox3.Text)+"\",";
							mensaje+="•Se Modificará la Fecha de Notificación\n";
					}
				}else{
					if(maskedTextBox3.Text.Equals("  /  /")){
						if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].Value.ToString().Length>1){
							sql2+=" fecha_notificacion=NULL,";
							mensaje+="•Se Borrará la Fecha de Notificación\n";
						}
					}
				}
				
				if(comboBox3.SelectedIndex>-1){
					if(!comboBox3.SelectedItem.ToString().Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[9].Value.ToString())){
						sql2+=" notificador=\""+comboBox3.SelectedItem.ToString()+"\",";
						mensaje+="•Se Modificará el Notificador\n";
					}
				}else{
					if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[9].Value.ToString().Equals("-")==true){
						//sql2+=" notificador=\"-\",";
						//mensaje+="•Se Modificará el Notificador\n";
					}
				}
				
				if(!textBox13.Text.Equals(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[10].Value.ToString())){
					sql2+=" observaciones=\""+textBox13.Text+"\",";
					mensaje+="•Se Modificarán las Observaciones\n";
				}
				
				if(sql2.Length>0){
					
					DialogResult rs=MessageBox.Show("Se realizará(n) el(los) siguiente(s) cambio(s):\n\n"+mensaje+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					
					if(rs== DialogResult.Yes){
						sql=sql+sql2.Substring(0,sql2.Length-1);
						sql+=" WHERE id_ema_sepomex= "+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[11].Value.ToString()+"";
						//MessageBox.Show(sql);
						conex3.consultar(sql);
						conex3.guardar_evento("Se Modificó el credito de la EMA_SEPOMEX con el id: "+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[11].Value.ToString()+"");
						MessageBox.Show("Se Realizaron los Cambios Correctamente.","Éxito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
						groupBox2.Visible=false;
						buscar_id(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[11].Value.ToString());
					}
				}
			}catch(Exception ed){
				MessageBox.Show("Ocurrió el Siguiente error al intentar guardar los cambios:\n\n"+ed.ToString(),"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			DateTime fech;
			if(maskedTextBox1.MaskCompleted==true){
				if(DateTime.TryParse(maskedTextBox1.Text,out fech)){
					if((fech>Convert.ToDateTime("31/12/2015"))&&(fech<=DateTime.Today)){
						maskedTextBox1.BackColor=Color.LimeGreen;
						maskedTextBox1.ForeColor=Color.White;
					}else{
						maskedTextBox1.BackColor=Color.Red;
						maskedTextBox1.ForeColor=Color.White;
					}
				}else{
					maskedTextBox1.BackColor=Color.Red;
					maskedTextBox1.ForeColor=Color.White;
				}
			}else{
				maskedTextBox1.BackColor=Color.White;
				maskedTextBox1.ForeColor=Color.Black;
			}
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			DateTime fech;
			if(maskedTextBox2.MaskCompleted==true){
				if(DateTime.TryParse(maskedTextBox2.Text,out fech)){
					if((fech>Convert.ToDateTime("31/12/2015"))&&(fech<=DateTime.Today)){
						maskedTextBox2.BackColor=Color.LimeGreen;
						maskedTextBox2.ForeColor=Color.White;
					}else{
						maskedTextBox2.BackColor=Color.Red;
						maskedTextBox2.ForeColor=Color.White;
					}
				}else{
					maskedTextBox2.BackColor=Color.Red;
					maskedTextBox2.ForeColor=Color.White;
				}
			}else{
				maskedTextBox2.BackColor=Color.White;
				maskedTextBox2.ForeColor=Color.Black;
			}
		}
				
		void MaskedTextBox3TextChanged(object sender, EventArgs e)
		{
			DateTime fech;
			if(maskedTextBox3.MaskCompleted==true){
				if(DateTime.TryParse(maskedTextBox3.Text,out fech)){
					if((fech>Convert.ToDateTime("31/12/2015"))&&(fech<=DateTime.Today)){
						maskedTextBox3.BackColor=Color.LimeGreen;
						maskedTextBox3.ForeColor=Color.White;
					}else{
						maskedTextBox3.BackColor=Color.Red;
						maskedTextBox3.ForeColor=Color.White;
					}
				}else{
					maskedTextBox3.BackColor=Color.Red;
					maskedTextBox3.ForeColor=Color.White;
				}
			}else{
				maskedTextBox3.BackColor=Color.White;
				maskedTextBox3.ForeColor=Color.Black;
			}
		}
		
		void MaskedTextBox3MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}
	}
}

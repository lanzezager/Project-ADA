/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 07/05/2018
 * Hora: 10:40 a. m.
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
	/// Description of Fecha_not_sepo.
	/// </summary>
	public partial class Fecha_not_sepo : Form
	{
		public Fecha_not_sepo()
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
		DataTable consulta = new DataTable();
		DataTable consulta_notifs = new DataTable();
		
		public void llena_cb1(){
			int i=0;
			consulta=conex.consultar("SELECT DISTINCT(periodo_ema) FROM ema_sepomex ORDER BY periodo_ema");
			while(i<consulta.Rows.Count){
				comboBox1.Items.Add(consulta.Rows[i][0].ToString());
				i++;
			}
		}
		
		public void llena_cb2(){
			int i=0;
			consulta_notifs=conex.consultar("SELECT apellido,nombre,controlador FROM usuarios WHERE puesto=\"Notificador\" and estatus=\"activo\" ORDER BY apellido");
			while(i<consulta_notifs.Rows.Count){
				comboBox2.Items.Add(consulta_notifs.Rows[i][0].ToString()+ " "+consulta_notifs.Rows[i][1].ToString());
				i++;
			}
		}
		
		public void busca_creds(){
			String folio="",fecha="";
			int fol_val=0,cand=0;
			DateTime aux_fech;
			
			conex.conectar("base_principal");
			
			if(DateTime.TryParse(textBox2.Text, out aux_fech)){
				cand=1;
				fecha=aux_fech.ToShortDateString();
				fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
			}else{
				if(textBox2.Text.ToUpper().Equals("NN")){
					fecha="NN";
					cand=1;
				}else{
					cand=0;
				}
			}
			
			if(comboBox1.SelectedIndex>-1 && cand==1){
				if(comboBox3.SelectedIndex==0){
					if(textBox1.Text.Length==19){
						folio=textBox1.Text.Substring((textBox1.Text.Length-7),6);
						fol_val++;
					}
					
					if(textBox1.Text.Length==1){
						folio="00000"+textBox1.Text;
						fol_val++;
					}
					
					if(textBox1.Text.Length==2){
						folio="0000"+textBox1.Text;
						fol_val++;
					}
					
					if(textBox1.Text.Length==3){
						folio="000"+textBox1.Text;
						fol_val++;
					}
					
					if(textBox1.Text.Length==4){
						folio="00"+textBox1.Text;
						fol_val++;
					}
					
					if(textBox1.Text.Length==5){
						folio="0"+textBox1.Text;
						fol_val++;
					}
					
					if(textBox1.Text.Length==6){
						folio=textBox1.Text;
						fol_val++;
					}
				}else{
					folio=textBox1.Text;
					fol_val++;
				}
				
				if(fol_val>0){
					if(comboBox3.SelectedIndex==0){
						consulta=conex.consultar("SELECT observaciones,registro_patronal,razon_social,credito_ema,folio,periodo_ema,id_ema_sepomex FROM ema_sepomex WHERE status=\"EN TRAMITE\" A periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" and folio=\""+folio+"\"");
					}else{
						if(comboBox3.SelectedIndex==1){
							consulta=conex.consultar("SELECT observaciones,registro_patronal,razon_social,credito_ema,folio,periodo_ema,id_ema_sepomex FROM ema_sepomex WHERE status=\"EN TRAMITE\" and periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" and credito_ema=\""+folio+"\"");
						}else{
							consulta=conex.consultar("SELECT observaciones,registro_patronal,razon_social,credito_ema,folio,periodo_ema,id_ema_sepomex FROM ema_sepomex WHERE status=\"EN TRAMITE\" and periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" and registro_patronal=\""+folio+"\"");
						}
					}
					if(consulta.Rows.Count>0){
						if(buscar_repe(consulta.Rows[0][3].ToString())==false){
							dataGridView1.Rows.Add(fecha,consulta.Rows[0][1].ToString(),consulta.Rows[0][2].ToString(),consulta.Rows[0][3].ToString(),consulta.Rows[0][4].ToString(),consulta.Rows[0][5].ToString(),consulta.Rows[0][6].ToString());
							textBox1.Clear();
							if(checkBox1.Checked==false){
								textBox2.Clear();
							}
							textBox1.Focus();
							label5.Text="Casos: "+dataGridView1.RowCount;
						}else{
							MessageBox.Show("El crédito ya se encuentra en la lista","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
						}
					}
				}
			}
		}
		
		public bool buscar_repe(String cred){
			int i=0,r=0;
			
			while(i<dataGridView1.RowCount){
				if(dataGridView1.Rows[i].Cells[3].Value.ToString().Equals(cred)==true){
					i=dataGridView1.RowCount+1;
					r=1;
				}
				i++;
			}
			
			if(r==1){
				return true;
			}else{
				return false;
			}
		}
		
		void Fecha_not_sepoLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			conex.conectar("base_principal");
			llena_cb1();
			llena_cb2();
			comboBox3.SelectedIndex=0;
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			if(textBox1.Text.Length==19){
				busca_creds();
			}
			
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
				busca_creds();
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			busca_creds();
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex>-1){
				textBox2.Focus();
			}
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			DateTime aux_fech;
			if(textBox2.Text.Length>0){
				if(DateTime.TryParse(textBox2.Text, out aux_fech)){
					textBox2.BackColor= Color.LimeGreen;
					textBox2.ForeColor=Color.White;
					textBox1.Focus();
				}else{
					if(textBox2.Text.ToUpper().Equals("NN")){
						textBox2.BackColor= Color.DarkViolet;
						textBox2.ForeColor= Color.White;
					}else{
						textBox2.BackColor= Color.Red;
						textBox2.ForeColor=Color.White;
					}
				}
			}else{
				textBox2.BackColor= System.Drawing.SystemColors.Window;
				textBox2.ForeColor=System.Drawing.SystemColors.ControlText;
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			int i=0;
			String id_mods="",fecha="",noti="";
			
			
			if(dataGridView1.RowCount>0){
				DialogResult res = MessageBox.Show("Se guardará la informacion de la notificación de  "+dataGridView1.RowCount+" casos\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				
				if(res == DialogResult.Yes){
					try{
						fecha = System.DateTime.Today.ToShortDateString();
						fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);
						
						if(checkBox2.Checked==true){
							noti=", notificador=\""+comboBox2.SelectedItem.ToString()+"\"";
						}else{
							noti="";
						}
						
						while(i<dataGridView1.RowCount){
							if(!dataGridView1.Rows[i].Cells[0].Value.ToString().Equals("NN")){
								conex.consultar("UPDATE ema_sepomex SET status=\"NOTIFICADO\", fecha_notificacion=\""+dataGridView1.Rows[i].Cells[0].Value.ToString()+"\", fecha_recepcion=\""+fecha+"\" "+noti+" WHERE id_ema_sepomex= "+dataGridView1.Rows[i].Cells[6].Value.ToString()+"");
								conex.guardar_evento("Se guardó como NOTIFICADO el crédito de la EMA_SEPOMEX con el id: "+dataGridView1.Rows[i].Cells[6].Value.ToString());
							}else{
								conex.consultar("UPDATE ema_sepomex SET status=\"NN\", fecha_recepcion=\""+fecha+"\"  "+noti+"  WHERE id_ema_sepomex= "+dataGridView1.Rows[i].Cells[6].Value.ToString()+"");
								conex.guardar_evento("Se guardó como NN el crédito de la EMA_SEPOMEX con el id: "+dataGridView1.Rows[i].Cells[6].Value.ToString());
							}
							i++;
						}
						MessageBox.Show("Se guardó exitosamente la información de la notificación","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
						label5.Text="Casos:";
						dataGridView1.Rows.Clear();
						comboBox1.SelectedIndex=-1;
						textBox1.Text="";
						textBox2.Text="";
						comboBox2.SelectedIndex=-1;
						checkBox2.Checked= false;
					}catch(Exception f){
						MessageBox.Show("Ocurrió el siguiente problema al intentar guardar la información de la notificación: \n"+f,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}
			}
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton1.Checked==true){
				label4.Text="Folio a Recibir";
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton2.Checked==true){
				label4.Text="Crédito a Recibir";
			}
		}
		
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox2.Checked==true){
				comboBox2.Enabled=true;
			}else{
				comboBox2.Enabled=false;
			}
		}
		
		void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedIndex==0){
				label4.Text="Folio a Recibir";
				label4.Refresh();
			}
			
			if(comboBox3.SelectedIndex==1){
				label4.Text="Crédito a Recibir";
				label4.Refresh();
			}
			
			if(comboBox3.SelectedIndex==2){
				label4.Text="Reg. Pat. a Recibir";
				label4.Refresh();
			}
		}
	}
	
}

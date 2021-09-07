/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 03/05/2018
 * Hora: 11:17 a. m.
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
	/// Description of Generar_fac_sepo.
	/// </summary>
	public partial class Generar_fac_sepo : Form
	{
		public Generar_fac_sepo()
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
		DataTable consulta = new DataTable();
		DataTable consulta_notifs = new DataTable();
		DataTable tablarow = new DataTable();
		
		public void llena_cb1(){
			int i=0;
			consulta=conex.consultar("SELECT DISTINCT(periodo_ema) FROM ema_sepomex ORDER BY periodo_ema DESC");
			while(i<consulta.Rows.Count){
				comboBox1.Items.Add(consulta.Rows[i][0].ToString());
				i++;
			}
		}
		
		public void llena_cb2(){
			int i=0;
			consulta_notifs=conex2.consultar("SELECT apellido,nombre,controlador FROM usuarios WHERE puesto=\"Notificador\" and estatus=\"activo\" ORDER BY apellido");
			while(i<consulta_notifs.Rows.Count){
				comboBox2.Items.Add(consulta_notifs.Rows[i][0].ToString()+ " "+consulta_notifs.Rows[i][1].ToString());
				i++;
			}
		}
		
		public void busca_creds(){
			String folio="";
			int fol_val=0;
			conex.conectar("base_principal");
			if(comboBox1.SelectedIndex>-1 && comboBox2.SelectedIndex>-1){
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
				
				if(fol_val>0){
					consulta=conex.consultar("SELECT registro_patronal,razon_social,credito_ema,folio,periodo_ema,id_ema_sepomex FROM ema_sepomex WHERE status=\"0\" and periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" and folio=\""+folio+"\"");
					if(consulta.Rows.Count>0){
						if(buscar_repe(consulta.Rows[0][2].ToString())==false){
							dataGridView1.Rows.Add(consulta.Rows[0][0].ToString(),consulta.Rows[0][1].ToString(),consulta.Rows[0][2].ToString(),consulta.Rows[0][3].ToString(),consulta.Rows[0][4].ToString(),consulta.Rows[0][5].ToString());
							textBox1.Clear();
							textBox1.Focus();
							label5.Text="Casos: "+dataGridView1.RowCount;
							label5.Refresh();
						}else{
							MessageBox.Show("El crédito ya se encuentra en la lista","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
						}
					}
					
				}
			}
		}
		
		public void busca_creds_por_cred(){
			String folio="";
			int fol_val=0;
			conex.conectar("base_principal");
			if(comboBox1.SelectedIndex>-1 && comboBox2.SelectedIndex>-1){
								
				if(textBox1.Text.Length==9){
					folio=textBox1.Text;
					fol_val++;
				}
				
				if(fol_val>0){
					if(buscar_repe(folio)==false){
						consulta=conex.consultar("SELECT registro_patronal,razon_social,credito_ema,folio,periodo_ema,id_ema_sepomex FROM ema_sepomex WHERE status=\"0\" and periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" and credito_ema=\""+folio+"\"");
						if(consulta.Rows.Count>0){
							dataGridView1.Rows.Add(consulta.Rows[0][0].ToString(),consulta.Rows[0][1].ToString(),consulta.Rows[0][2].ToString(),consulta.Rows[0][3].ToString(),consulta.Rows[0][4].ToString(),consulta.Rows[0][5].ToString());
							textBox1.Clear();
							textBox1.Focus();
							label5.Text="Casos: "+dataGridView1.RowCount;
							label5.Refresh();
						}
					}else{
						MessageBox.Show("El crédito ya se encuentra en la lista","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
				}
			}
		}
		
		public void busca_creds_por_rp(){
			String folio="";
			int fol_val=0;
			conex.conectar("base_principal");
			if(comboBox1.SelectedIndex>-1 && comboBox2.SelectedIndex>-1){
								
				if(textBox1.Text.Length==10){
					folio=textBox1.Text;
					fol_val++;
				}
				
				if(fol_val>0){
					if(buscar_repe(folio)==false){
						consulta=conex.consultar("SELECT registro_patronal,razon_social,credito_ema,folio,periodo_ema,id_ema_sepomex FROM ema_sepomex WHERE status=\"0\" and periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" and registro_patronal=\""+folio+"\"");
						if(consulta.Rows.Count>0){
							dataGridView1.Rows.Add(consulta.Rows[0][0].ToString(),consulta.Rows[0][1].ToString(),consulta.Rows[0][2].ToString(),consulta.Rows[0][3].ToString(),consulta.Rows[0][4].ToString(),consulta.Rows[0][5].ToString());
							textBox1.Clear();
							textBox1.Focus();
							label5.Text="Casos: "+dataGridView1.RowCount;
							label5.Refresh();
						}
					}else{
						MessageBox.Show("El crédito ya se encuentra en la lista","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
				}
			}
		}
		
		public bool buscar_repe(String cred){
			int i=0,r=0;
			
			while(i<dataGridView1.RowCount){
				if(dataGridView1.Rows[i].Cells[2].Value.ToString().Equals(cred)==true){
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
		
		public void gen_fac(String carpeta_sel){
			int llave=0,i=0;
			String error="",contro="",fecha="",ra_soc="",ids_mod="";
			
			if(dataGridView1.RowCount>0){
				llave++;
			}else{
				error +="•No hay Créditos para Entregar\n";
			}
			
			if(comboBox1.SelectedIndex>-1){
				llave++;
			}else{
				error +="•No Seleccionó Periodo\n";
			}
			
			if(comboBox2.SelectedIndex>-1){
				llave++;
			}else{
				error +="•No Seleccionó Notificador\n";
			}
			
			if(llave==3){
				DialogResult res = MessageBox.Show("Se generará la factura del periodo: EMA_"+comboBox1.SelectedItem.ToString()+" para el notificador: "+comboBox2.SelectedItem.ToString().ToUpper()+" con "+dataGridView1.RowCount+" casos\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
					
				if(res == DialogResult.Yes){
					try{
						
						
					consulta=conex.consultar("SELECT apellido,nombre FROM usuarios WHERE id_usuario="+consulta_notifs.Rows[comboBox2.SelectedIndex][2].ToString()+"");
					
					fecha = System.DateTime.Today.ToShortDateString();
            		fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);
            
					if(consulta.Rows.Count>0){
						contro=consulta.Rows[0][0]+" "+consulta.Rows[0][1].ToString();
					}else{
						contro ="-";
					}
					
					while(i<dataGridView1.RowCount){
            			            			
            			if(dataGridView1.Rows[i].Cells[1].Value.ToString().Length>17){
            				ra_soc=dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0,18);
            			}else{
            				ra_soc=dataGridView1.Rows[i].Cells[1].Value.ToString();
            			}
            			
						tablarow.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString()+" "+ra_soc,
						                  dataGridView1.Rows[i].Cells[2].Value.ToString(),dataGridView1.Rows[i].Cells[4].Value.ToString(),dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(2,1),
						                  "-","0.00",comboBox2.SelectedItem.ToString(),contro,comboBox1.SelectedItem.ToString(),"-","-","-","-",fecha,conex.leer_config_sub()[0].ToUpper(),conex.leer_config_sub()[3].ToUpper());
            			ids_mod+=dataGridView1.Rows[i].Cells[5].Value.ToString()+",";
            			i++;
					}
            		ids_mod=ids_mod.Substring(0,(ids_mod.Length-1));
            		conex.consultar("UPDATE ema_sepomex SET notificador=\""+comboBox2.SelectedItem.ToString()+"\", controlador=\""+contro+"\", status=\"EN TRAMITE\", fecha_entrega=\""+fecha+"\" WHERE id_ema_sepomex IN ("+ids_mod+") ");
            		
            		Visor_reporte_factura visor = new Visor_reporte_factura();
            		visor.envio2(tablarow, carpeta_sel);
					visor.Show();
					
					conex.guardar_evento("Se Generó correctamente la factura de entrega de créditos EMA_SEPOMEX para el notificador: "+comboBox2.SelectedItem.ToString()+" conteniendo "+i+" créditos cuyos ids son:"+ids_mod);
					MessageBox.Show("La factura se generó correctamente","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
					dataGridView1.Rows.Clear();
					label5.Text="Casos: "+dataGridView1.RowCount;
					label5.Refresh();
					comboBox1.SelectedIndex=-1;
					comboBox1.Text="";
					comboBox2.SelectedIndex=-1;
					comboBox2.Text="";
					tablarow.Rows.Clear();
					
					}catch(Exception s){
						MessageBox.Show("Ocurrió el siguiente error al momento de generar la factura: \n"+s,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}
				
			}else{
				MessageBox.Show("No se puede continuar debido a que:\n"+error,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedIndex==0){
				if(textBox1.Text.Length==19){
					busca_creds();
				}
			}else{
				if(textBox1.Text.Length==9){
					busca_creds_por_cred();
				}
				
				if(textBox1.Text.Length==10){
					busca_creds_por_rp();
				}
			}
		}
		
		void Generar_fac_sepoLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			conex.conectar("base_principal");
		    conex2.conectar("base_principal");
			llena_cb1();
			llena_cb2();
			comboBox3.SelectedIndex=0;
			
			tablarow.Columns.Add("reg_pat");
			tablarow.Columns.Add("credito_cuota");
			tablarow.Columns.Add("periodo");
			tablarow.Columns.Add("tipo doc");
			tablarow.Columns.Add("sector");
			tablarow.Columns.Add("importe");
			tablarow.Columns.Add("noti");
			tablarow.Columns.Add("contro");
            tablarow.Columns.Add("nom_periodo");
            tablarow.Columns.Add("sector_inicial");
            tablarow.Columns.Add("nn");
            tablarow.Columns.Add("cifra");
            tablarow.Columns.Add("fecha_cifra");
            tablarow.Columns.Add("fecha_fact");
            tablarow.Columns.Add("del");
            tablarow.Columns.Add("subdel");
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			//MessageBox.Show(""+comboBox3.SelectedIndex);
			if(comboBox3.SelectedIndex==0){
				if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
					busca_creds();
				}
			}else{
				if(comboBox3.SelectedIndex==1){
					if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
						busca_creds_por_cred();
					}
				}else{
					if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))){
						busca_creds_por_cred();
					}
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
				FolderBrowserDialog fbd = new FolderBrowserDialog();
				fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
				DialogResult result = fbd.ShowDialog();
				
				if (result == DialogResult.OK)
				{
					gen_fac(fbd.SelectedPath.ToString());
				}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			if(comboBox3.SelectedIndex==0){
				busca_creds();
			}else{
				if(comboBox3.SelectedIndex==1){
					busca_creds_por_cred();
				}else{
					busca_creds_por_rp();
				}
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex>-1){
				comboBox2.Focus();
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedIndex>-1){
				textBox1.Focus();
			}
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedIndex>-1){
				textBox1.Focus();
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			String fecha_entr="";
			int casos=0;
			if(comboBox1.SelectedIndex>-1){
				conex.conectar("base_principal");
				consulta=conex.consultar("SELECT count(id_ema_sepomex) FROM ema_sepomex WHERE periodo_ema= \""+comboBox1.SelectedItem.ToString()+"\" AND status=\"0\" ");
				if(consulta.Rows.Count>0){
					casos=Convert.ToInt32(consulta.Rows[0][0].ToString());
					if(casos>0){
						DialogResult res = MessageBox.Show("Se Marcarán como entregados el día de hoy "+casos+" registros del Periodo: "+comboBox1.SelectedItem.ToString()+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
						if(res == DialogResult.Yes){
							try{
								comboBox1.Enabled=false;
								fecha_entr=DateTime.Today.ToShortDateString();
								fecha_entr=fecha_entr.Substring(6,4)+"-"+fecha_entr.Substring(3,2)+"-"+fecha_entr.Substring(0,2);
								conex.consultar("UPDATE ema_sepomex SET fecha_entrega=\""+fecha_entr+"\", status=\"EN TRAMITE\" WHERE periodo_ema=\""+comboBox1.SelectedItem.ToString()+"\" AND status=\"0\" ");
								
								MessageBox.Show("Los Registros han sido actualizados exitosamente como ENTREGADOS.","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
								conex.guardar_evento("Se Entregó para Notificar el periodo_ema: "+comboBox1.SelectedItem.ToString()+" en su Totalidad, conteniendo: "+casos+" casos");
								comboBox1.Enabled=true;
							}catch(Exception asd){
								MessageBox.Show("Ocurrió el siguiente error al momento de procesar la Entrega: \n"+asd,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
							}
						}
					}else{
						MessageBox.Show("No hay Documentos por entregar del periodo solicitado.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
				}
			}
		}
	}
}

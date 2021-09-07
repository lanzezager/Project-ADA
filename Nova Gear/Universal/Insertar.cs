/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 03/06/2016
 * Hora: 01:48 p.m.
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

namespace Nova_Gear.Universal
{
	/// <summary>
	/// Description of Insertar.
	/// </summary>
	public partial class Insertar : Form
	{
		public Insertar()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int tipo_load=0,i=0;
		int[] contros;
        string[] valores = new string[12];
        string[] resu_grid = new string[30];
        
		String rp,cc,cm,id,sub_real;
        //Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		
        //periodos
		public void llenar_Cb4(){
			conex.conectar("base_principal");
			comboBox4.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE nombre_periodo LIKE \"%CM%\" ORDER BY nombre_periodo;");
			do{
				comboBox4.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
            conex.cerrar();
		}	
        //periodos
		public void llenar_Cb4_todos(){
			conex.conectar("base_principal");
			comboBox4.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			do{
				comboBox4.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
            conex.cerrar();
		}	
		//controladores
		public void llenar_Cb1(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre,id_usuario FROM base_principal.usuarios WHERE puesto =\"Controlador\" ORDER BY apellido;");
			//dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (controlador) FROM base_principal.datos_factura ORDER BY controlador;");
			contros = new int[dataGridView3.RowCount];
			do{
				comboBox1.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				contros[i] = Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			//comboBox4.Items.Add("--NINGUNO--");
            conex.cerrar();
		}	
		//notificadores
		public void llenar_Cb2(){
			conex.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			//dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (notificador) FROM base_principal.datos_factura ORDER BY notificador;");
			do{
				//comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			//comboBox3.Items.Add("--NINGUNO--");
            conex.cerrar();
		}
		//notificadores v2
		public void llenar_Cb2_v2(){
			conex2.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex2.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" AND controlador = \""+contros[comboBox1.SelectedIndex]+"\" ORDER BY apellido;");
			do{
				comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			//comboBox3.Items.Add("--NINGUNO--");
            conex2.cerrar();
		}
		
		public void carga(){
			label1.Text="Carga Manual Individual de Créditos";
			this.Text="Nova Gear: Carga Manual Individual de Créditos";
			panel1.Enabled=false;
			panel1.Visible=false;
			panel2.Visible=false;
			groupBox2.Visible=false;
			groupBox3.Visible=false;
			tipo_load=1;
            label1.Location=new System.Drawing.Point(label1.Location.X, 28);
            this.Height=300;
		}
		
		public void modificacion(){
			label1.Text="Actualización Individual de Créditos";
			this.Text="Nova Gear: Actualización Individual de Créditos";
			panel1.Enabled=true;
			panel1.Visible=true;
			panel3.Visible=true;
			radioButton1.Enabled=false;
			radioButton2.Enabled=false;
			button3.Enabled=false;
			radioButton1.Enabled=false;
			radioButton2.Enabled=false;
			tipo_load=2;
			this.Height=600;
			button5.Visible=false;
		}
		
		public void guardar(){
			int k = 0,activar=0,falta=0,a=0,ingresado=0;
			String reg_pat,reg_pat1,reg_pat2;
			
            while (k < 12)
            {	
            	try{
	            	if(valores[k].Length>0){
	                    activar++;
	                }
                
            	}catch(Exception fv){}
            	k++;
            }
            
            if(activar==10){
            	if(valores[2].Length<1 && valores[5].Length<1){
            		activar=12;
            		falta=1;
            	}
            	
            	if(valores[3].Length<1 && valores[6].Length<1){
            		activar=12;
            		falta=2;
            	}
            }
            
            if(activar==12){
            	conex.conectar("base_principal");
            	if(valores[0].Length==10){
            		
	            	dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1 FROM datos_factura WHERE registro_patronal2 =\""+valores[0].Substring(0,10)+"\" ");
	            	if(dataGridView1.RowCount>0){
	            		if(dataGridView1.Rows[0].Cells[0].FormattedValue.ToString().Length>10){
	            		valores[0]=dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
	            		}else{
	            			valores[0]=valores[0]+"0";
	            		}
	            	}else{
	            		valores[0]=valores[0]+"0";
	            	}
            	}
            	
            	reg_pat2=valores[0].Substring(0,10);
            	reg_pat1=valores[0];
            	reg_pat=valores[0].Substring(0,3)+"-"+valores[0].Substring(3,5)+"-"+valores[0].Substring(8,2)+"-"+valores[0].Substring(10,1);
            	
            	if(falta==0){
            		dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1 FROM datos_factura WHERE registro_patronal2 =\""+valores[0].Substring(0,10)+"\" AND credito_cuotas=\""+valores[2]+"\" AND credito_multa=\""+valores[3]+"\" AND periodo=\""+valores[1]+"\"");
            		if(dataGridView1.RowCount < 1){
            			try{
	            			conex.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, credito_multa, importe_cuota, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion,incidencia) " +
	            		               	 	" VALUES (\"" + valores[11] + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + valores[4] + "\",\"" + valores[1] + "\",\"" + valores[9] + "\",\"" + valores[2] + "\",\"" + valores[3]+ "\",\"" + valores[5] + "\",\"" + valores[6] + "\",\"" + valores[10]+ "\",\"" + valores[10] + "\",\"" + valores[7] + "\",\"" + valores[8] + "\");\n");
            				ingresado=1;
            			}catch(Exception es){}	
            		}else{
            			ingresado=2;
            		}
            	}
            	
            	if(falta==1){
            		dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1 FROM datos_factura WHERE registro_patronal2 =\""+valores[0].Substring(0,10)+"\"  AND credito_multa=\""+valores[3]+"\" AND periodo=\""+valores[1]+"\"");
            		if(dataGridView1.RowCount < 1){
            			try{
	            			conex.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_multa, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion,incidencia) " +
	            		               	 	" VALUES (\"" + valores[11] + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + valores[4] + "\",\"" + valores[1] + "\",\"" + valores[9] + "\",\"" + valores[3]+ "\",\"" + valores[6] + "\",\"" + valores[10]+ "\",\"" + valores[10] + "\",\"" + valores[7] + "\",\"" + valores[8] + "\");\n");
            				ingresado=1;
            			}catch(Exception es1){}	
            		}else{
            			ingresado=2;
            		}
            	}
            	
            	if(falta==2){
            		dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1 FROM datos_factura WHERE registro_patronal2 =\""+valores[0].Substring(0,10)+"\" AND credito_cuotas=\""+valores[2]+"\" AND periodo=\""+valores[1]+"\"");
            		if(dataGridView1.RowCount < 1){
            			try{
	            			conex.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, importe_cuota, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion,incidencia) " +
	            		               	 	" VALUES (\"" + valores[11] + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + valores[4] + "\",\"" + valores[1] + "\",\"" + valores[9] + "\",\"" + valores[2] + "\",\"" + valores[5] + "\",\"" + valores[10]+ "\",\"" + valores[10] + "\",\"" + valores[7] + "\",\"" + valores[8] + "\");\n");
            				ingresado=1;
            			}catch(Exception es2){}	
            		}else{
            			ingresado=2;
            		}
            	}
            	
            	if(ingresado==1){
            		
            		conex.guardar_evento("Se Ingresó el Registro: "+reg_pat2+" con el crédito cuota: "+valores[2]+" y credito multa: "+valores[3]);
            		
            		maskedTextBox1.Text="";
            		maskedTextBox1.BackColor=Color.White;
            		maskedTextBox2.Text="";
            		maskedTextBox2.BackColor=Color.White;
            		maskedTextBox3.Text="";
            		maskedTextBox3.BackColor=Color.White;
            		maskedTextBox4.Text="";
            		maskedTextBox4.BackColor=Color.White;
            		//maskedTextBox5.Text="";
            		maskedTextBox5.BackColor=Color.White;
            		//maskedTextBox6.Text="";
            		maskedTextBox6.BackColor=Color.White;
            		maskedTextBox7.Text="";
            		maskedTextBox7.BackColor=Color.White;
            		maskedTextBox8.Text="";
            		maskedTextBox8.BackColor=Color.White;
            		textBox1.Text="";
            		textBox1.BackColor=Color.White;
            		textBox2.Text="";
            		textBox2.BackColor=Color.White;
            		textBox3.Text="";
            		textBox3.BackColor=Color.White;
            		 
            		MessageBox.Show("Registro Guardado exitosamente","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            		maskedTextBox1.Focus();
            		
            		a=0;
            		while(a<(valores.Length-1)){
            			valores[a]="";
            			a++;
            		}
            	}else{
            		MessageBox.Show("El Registro Patronal con el Crédito Cuota y/o Crédito Multa que intenta ingresar ya se encuentra en la base de datos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            	}
          
            }else{
            	MessageBox.Show("Falta información, todos los campos son obligatorios.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            
            //MessageBox.Show(valores[0]+" |"+activar+"| "+falta);
		}
		
		public void guardar_mod(){
			
			String reg_pat2,reg_pat1,reg_pat,fecha_entrega;
			int falta=0,guarda_tras=0;
			
			if(label31.ForeColor.Name.Equals("LawnGreen")){
				
				DialogResult resultado = MessageBox.Show("Esta por modificar el registro.\nLa Nueva Información Sobreescribirá la anterior.\n ¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				//MessageBox.Show(valores[0]+"|"+valores[1]+"|"+valores[2]+"|"+valores[3]+"|"+valores[4]+"|"+valores[5]+"|");
				//MessageBox.Show(valores[6]+"|"+valores[7]+"|"+valores[8]+"|"+valores[9]+"|"+valores[10]+"|"+valores[11]+"|");
				if(resultado == DialogResult.Yes){
					conex.conectar("base_principal");
					if(valores[0].Length==10){
						
						dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1 FROM datos_factura WHERE registro_patronal2 =\""+valores[0].Substring(0,10)+"\" ");
						if(dataGridView1.RowCount>0){
							if(dataGridView1.Rows[0].Cells[0].FormattedValue.ToString().Length>10){
								valores[0]=dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
							}else{
								valores[0]=valores[0]+"0";
							}
						}else{
							valores[0]=valores[0]+"0";
						}
					}
					
					reg_pat2=valores[0].Substring(0,10);
					reg_pat1=valores[0];
					reg_pat=valores[0].Substring(0,3)+"-"+valores[0].Substring(3,5)+"-"+valores[0].Substring(8,2)+"-"+valores[0].Substring(10,1);
					
					if(valores[2].Length<1 && valores[5].Length<1){
						
						falta=1;
					}
					
					if(valores[3].Length<1 && valores[6].Length<1){
						
						falta=2;
					}
					
					if(falta==0){
						try{
							conex.consultar("UPDATE datos_factura SET nombre_periodo=\"" + valores[11] + "\", registro_patronal=\"" + reg_pat + "\", registro_patronal1=\"" + reg_pat1 + "\", registro_patronal2=\"" + reg_pat2 + "\", razon_social=\"" + valores[4] + "\", periodo=\"" + valores[1] + "\", tipo_documento=\"" + valores[9] + "\", credito_cuotas=\"" + valores[2] + "\", credito_multa=\"" + valores[3]+ "\", importe_cuota=\"" + valores[5] + "\", importe_multa=\"" + valores[6] + "\", sector_notificacion_inicial=\"" + valores[10]+ "\", subdelegacion=\"" + valores[7] + "\",incidencia=\"" + valores[8] + "\" " +
							                "WHERE id="+id+" ");
							
						}catch(Exception es){}
					}
					
					if(falta==1){
						try{
							conex.consultar("UPDATE datos_factura SET nombre_periodo=\"" + valores[11] + "\", registro_patronal=\"" + reg_pat + "\", registro_patronal1=\"" + reg_pat1 + "\", registro_patronal2=\"" + reg_pat2 + "\", razon_social=\"" + valores[4] + "\", periodo=\"" + valores[1] + "\", tipo_documento=\"" + valores[9] + "\",  credito_multa=\"" + valores[3]+ "\",  importe_multa=\"" + valores[6] + "\", sector_notificacion_inicial=\"" + valores[10]+ "\", subdelegacion=\"" + valores[7] + "\",incidencia=\"" + valores[8] + "\" " +
							                "WHERE id="+id+" ");
						}catch(Exception es1){}
					}
					
					if(falta==2){
						try{
							conex.consultar("UPDATE datos_factura SET nombre_periodo=\"" + valores[11] + "\", registro_patronal=\"" + reg_pat + "\", registro_patronal1=\"" + reg_pat1 + "\", registro_patronal2=\"" + reg_pat2 + "\", razon_social=\"" + valores[4] + "\", periodo=\"" + valores[1] + "\", tipo_documento=\"" + valores[9] + "\", credito_cuotas=\"" + valores[2] + "\",  importe_cuota=\"" + valores[5] + "\",  sector_notificacion_inicial=\"" + valores[10]+ "\", subdelegacion=\"" + valores[7] + "\",incidencia=\"" + valores[8] + "\" " +
							                "WHERE id="+id+" ");
						}catch(Exception es2){}
					}
					
					if(maskedTextBox9.Text.Length==2){
						conex.consultar("UPDATE datos_factura SET sector_notificacion_actualizado=\""+maskedTextBox9.Text+"\" WHERE id="+id+" ");
					}
					
					if(comboBox1.SelectedIndex > -1){
						conex.consultar("UPDATE datos_factura SET controlador=\""+comboBox1.SelectedItem.ToString()+"\" WHERE id="+id+" ");
					}
					
					if(comboBox2.SelectedIndex > -1){
						conex.consultar("UPDATE datos_factura SET notificador=\""+comboBox2.SelectedItem.ToString()+"\" WHERE id="+id+" ");
					}
					
					if(checkBox2.Checked==true){
							fecha_entrega=dateTimePicker1.Text;
							fecha_entrega=fecha_entrega.Substring(6,4)+"-"+fecha_entrega.Substring(3,2)+"-"+fecha_entrega.Substring(0,2);
							conex.consultar("UPDATE datos_factura SET fecha_entrega=\""+fecha_entrega+"\" WHERE id="+id+" ");
					}
					
					if(checkBox8.Checked==true){
						conex.consultar("UPDATE datos_factura SET fecha_entrega=NULL WHERE id="+id+" ");
					}
					
					if(checkBox3.Checked==true){
						fecha_entrega=dateTimePicker2.Text;
						fecha_entrega=fecha_entrega.Substring(6,4)+"-"+fecha_entrega.Substring(3,2)+"-"+fecha_entrega.Substring(0,2);
						conex.consultar("UPDATE datos_factura SET fecha_recepcion=\""+fecha_entrega+"\" WHERE id="+id+" ");
					}
					
					if(checkBox9.Checked==true){
						conex.consultar("UPDATE datos_factura SET fecha_recepcion=NULL WHERE id="+id+" ");
					}
					
					if(checkBox4.Checked==true){
						fecha_entrega=dateTimePicker3.Text;
						fecha_entrega=fecha_entrega.Substring(6,4)+"-"+fecha_entrega.Substring(3,2)+"-"+fecha_entrega.Substring(0,2);
						conex.consultar("UPDATE datos_factura SET fecha_notificacion=\""+fecha_entrega+"\" WHERE id="+id+" ");
					}
					
					if(checkBox10.Checked==true){
						conex.consultar("UPDATE datos_factura SET fecha_notificacion=NULL WHERE id="+id+" ");
					}
					/*
					if(checkBox1.Checked==true){
						conex.consultar("UPDATE datos_factura SET nn=\"NN\" WHERE id="+id+" ");
					}else{
						conex.consultar("UPDATE datos_factura SET nn=\"-\" WHERE id="+id+" ");
					}
					*/
					
					if(comboBox6.SelectedIndex==0){//Normal
						conex.consultar("UPDATE datos_factura SET nn=\"-\" WHERE id="+id+" ");
					}else{
						if(comboBox6.SelectedIndex==1){//Estrados
							conex.consultar("UPDATE datos_factura SET nn=\"ESTRADOS\" WHERE id="+id+" ");
						}else{
							if(comboBox6.SelectedIndex==2){//NN
								conex.consultar("UPDATE datos_factura SET nn=\"NN\" WHERE id="+id+" ");
							}
						}
					}
						
					if(comboBox3.SelectedIndex > -1){
						if(comboBox3.SelectedIndex == 7){
							if(checkBox7.Checked==true && textBox8.Text.Length>5){
								conex.consultar("UPDATE datos_factura SET status=\""+comboBox3.SelectedItem.ToString()+"\" WHERE id="+id+" ");
								guarda_tras=1;
							}else{
								MessageBox.Show("No se actualizará el estatus.\nFalta información del traspaso.","AVISO");
								guarda_tras=0;
							}
						}else{
							conex.consultar("UPDATE datos_factura SET status=\""+comboBox3.SelectedItem.ToString()+"\" WHERE id="+id+" ");
						}
					}else{
						
					}
					
					if(textBox4.Text.Length>4){
						conex.consultar("UPDATE datos_factura SET folio_sipare_sua=\""+textBox4.Text+"\" WHERE id="+id+" ");
					}else{
						if(textBox4.Text.Length==0){
							conex.consultar("UPDATE datos_factura SET folio_sipare_sua=\"-\" WHERE id="+id+" ");
						}
					}
					
					if(textBox5.Text.Length>4){
						conex.consultar("UPDATE datos_factura SET fecha_pago=\""+textBox5.Text+"\" WHERE id="+id+" ");
					}else{
						if(textBox5.Text.Length==0){
							conex.consultar("UPDATE datos_factura SET fecha_pago=\"-\" WHERE id="+id+" ");
						}
					}
					
					if(checkBox5.Checked==true){
						fecha_entrega=dateTimePicker4.Text;
						fecha_entrega=fecha_entrega.Substring(6,4)+"-"+fecha_entrega.Substring(3,2)+"-"+fecha_entrega.Substring(0,2);
						conex.consultar("UPDATE datos_factura SET fecha_cartera=\""+fecha_entrega+"\" WHERE id="+id+" ");
					}
					
					if(checkBox11.Checked==true){
						conex.consultar("UPDATE datos_factura SET fecha_cartera=NULL, estado_cartera=\"-\" WHERE id="+id+" ");
					}
					
					if(checkBox6.Checked==true){
						fecha_entrega=dateTimePicker5.Text;
						fecha_entrega=fecha_entrega.Substring(6,4)+"-"+fecha_entrega.Substring(3,2)+"-"+fecha_entrega.Substring(0,2);
						conex.consultar("UPDATE datos_factura SET fecha_depuracion=\""+fecha_entrega+"\" WHERE id="+id+" ");
					}
					
					if(checkBox12.Checked==true){
						conex.consultar("UPDATE datos_factura SET fecha_depuracion=\"-\" WHERE id="+id+" ");
					}
					
					if(checkBox7.Checked==true && textBox8.Text.Length>5 && guarda_tras==1){
						fecha_entrega=dateTimePicker6.Text;
						fecha_entrega=fecha_entrega.Substring(6,4)+"-"+fecha_entrega.Substring(3,2)+"-"+fecha_entrega.Substring(0,2);
						conex.consultar("UPDATE datos_factura SET fecha_traspaso=\""+fecha_entrega+"\" WHERE id="+id+" ");
					}
					
					if(checkBox13.Checked==true){
						conex.consultar("UPDATE datos_factura SET fecha_traspaso=NULL WHERE id="+id+" ");
					}
					
					if(textBox6.Text.Length>1 && textBox6.BackColor.Name.Equals("PaleGreen")){
						conex.consultar("UPDATE datos_factura SET importe_pago=\""+textBox6.Text+"\" WHERE id="+id+" ");
					}
					
					if(maskedTextBox10.Text.Length==2){
						conex.consultar("UPDATE datos_factura SET num_pago="+maskedTextBox10.Text+" WHERE id="+id+" ");
					}
					
					if(textBox7.Text.Length > 0){
						conex.consultar("UPDATE datos_factura SET observaciones=\""+textBox7.Text+"\" WHERE id="+id+" ");
					}
					
					if(textBox8.Text.Length>5 && guarda_tras==1){
						conex.consultar("UPDATE datos_factura SET info_traspaso=\""+textBox8.Text+"\" WHERE id="+id+" ");
					}
					
					if(comboBox4.SelectedIndex>-1 && comboBox4.BackColor.Name.Equals("PaleGreen")){
						conex.consultar("UPDATE datos_factura SET nombre_periodo=\""+comboBox4.SelectedItem.ToString()+"\" WHERE id="+id+" ");
					}
					
					checkBox2.Checked=false;
					checkBox3.Checked=false;
					checkBox4.Checked=false;
					checkBox5.Checked=false;
					checkBox6.Checked=false;
					checkBox7.Checked=false;
					
					checkBox8.Checked=false;
            		checkBox9.Checked=false;
            		checkBox10.Checked=false;
            		checkBox11.Checked=false;
            		checkBox12.Checked=false;
            		checkBox13.Checked=false;
					panel2.Visible=true;
					panel3.Enabled=false;
					groupBox1.Enabled=false;
					groupBox2.Enabled=false;
					groupBox3.Enabled=false;
					comboBox4.Enabled=false;
					button1.Enabled=false;
					//comboBox2.SelectedIndex=-1;
					//comboBox2.Text="";
					
					MessageBox.Show("¡Modificación Exitosa!","AVISO");
					conex.guardar_evento("Se Modificó el Registro: "+reg_pat2+" con el crédito cuota: "+valores[2]+" y credito multa: "+valores[3]+" y el ID: "+id );
					maskedTextBox11.Focus();
				}else{
					MessageBox.Show("No se hizo nada","AVISO");
				}
			}
			
		}
		
		public void buscar(){
			
			int i=0,j=0,k=0;
			
			resu_grid.Initialize();
			//MessageBox.Show(rp+"|"+cc+"|"+cm);
			if((rp.Length > 0 && cc.Length > 0)||(rp.Length > 0 && cm.Length > 0)){
				conex.conectar("base_principal");
				if(cc.Length>0 && (cm.Length <= 0)){
					dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1,periodo,credito_cuotas,credito_multa,razon_social,importe_cuota,importe_multa,subdelegacion,incidencia,tipo_documento,sector_notificacion_inicial," +
					                                         "sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,nn," +
					                                         "status,folio_sipare_sua,fecha_pago,fecha_cartera,fecha_depuracion,importe_pago,num_pago,observaciones,nombre_periodo,id,fecha_traspaso,info_traspaso FROM datos_factura WHERE registro_patronal2=\""+rp.Substring(0,10)+"\" AND credito_cuotas=\""+cc+"\"");
				}else{
					
					if(cm.Length > 0 && cc.Length <= 0){
						dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1,periodo,credito_cuotas,credito_multa,razon_social,importe_cuota,importe_multa,subdelegacion,incidencia,tipo_documento,sector_notificacion_inicial," +
						                                         "sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,nn," +
						                                         "status,folio_sipare_sua,fecha_pago,fecha_cartera,fecha_depuracion,importe_pago,num_pago,observaciones,nombre_periodo,id,fecha_traspaso,info_traspaso FROM datos_factura WHERE registro_patronal2=\""+rp.Substring(0,10)+"\" AND credito_multa=\""+cm+"\"");
					}else{
						if(cc.Length > 0 && cm.Length > 0){
						dataGridView1.DataSource=conex.consultar("SELECT registro_patronal1,periodo,credito_cuotas,credito_multa,razon_social,importe_cuota,importe_multa,subdelegacion,incidencia,tipo_documento,sector_notificacion_inicial," +
						                                         "sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,nn," +
						                                         "status,folio_sipare_sua,fecha_pago,fecha_cartera,fecha_depuracion,importe_pago,num_pago,observaciones,nombre_periodo,id,fecha_traspaso,info_traspaso FROM datos_factura WHERE registro_patronal2=\""+rp.Substring(0,10)+"\" AND credito_cuotas=\""+cc+"\" AND credito_multa=\""+cm+"\"");
						}
					}
					
				}
				
				//Thread.Sleep(500);
				
				if(dataGridView1.RowCount>0){
					
					panel2.Visible=false;
					groupBox1.Enabled=true;
					groupBox2.Enabled=true;
					groupBox3.Enabled=true;
					comboBox4.Enabled=true;
					radioButton1.Enabled=true;
					radioButton2.Enabled=true;
					maskedTextBox5.Text="";
					maskedTextBox6.Text="";
					panel3.Enabled=true;
					button1.Enabled=true;
					comboBox2.SelectedIndex=-1;
					comboBox2.Text="";
					
					
					while(i<dataGridView1.Columns.Count){
						resu_grid[i]=dataGridView1.Rows[0].Cells[i].FormattedValue.ToString();
						
						//MessageBox.Show(resu_grid[i]);
						
						i++;
					}
					
					maskedTextBox1.Text=resu_grid[0].Substring(0,3)+" - "+resu_grid[0].Substring(3,5)+" - "+resu_grid[0].Substring(8,2)+" - "+resu_grid[0].Substring(10,1);
					maskedTextBox2.Text=resu_grid[1];
					maskedTextBox3.Text=resu_grid[2];
					maskedTextBox4.Text=resu_grid[3];
					textBox1.Text=resu_grid[4];
					if(Convert.ToDecimal(resu_grid[5])>0){
						textBox2.Text=resu_grid[5];
					}else{
						textBox2.Text="";
					}
					
					if(Convert.ToDecimal(resu_grid[6])>0){
						textBox3.Text=resu_grid[6];
					}else{
						textBox3.Text="";
					}
					
					try{
						if(Convert.ToInt32(resu_grid[7])<10){
							maskedTextBox5.Text="0"+resu_grid[7];
						}else{
							maskedTextBox5.Text=resu_grid[7];
						}
					}catch(Exception es){
						maskedTextBox5.Text="0";
					}
					
					try{
						if(Convert.ToInt32(resu_grid[8])<10){
							maskedTextBox6.Text="0"+resu_grid[8];
						}else{
							maskedTextBox6.Text=resu_grid[8];
						}
					}catch(Exception es1){
						maskedTextBox6.Text="0";
					}
					
					try{
						if(Convert.ToInt32(resu_grid[9])<10){
							maskedTextBox7.Text="0"+resu_grid[9];
						}else{
							maskedTextBox7.Text=resu_grid[9];
						}
					}catch(Exception es2){
						maskedTextBox7.Text="0";
					}
					
					try{
						if(Convert.ToInt32(resu_grid[10])<10){
							maskedTextBox8.Text="0"+resu_grid[10];
						}else{
							maskedTextBox8.Text=resu_grid[10];
						}
					}catch(Exception es3){
						maskedTextBox8.Text="0";
					}
					
					try{
						if(Convert.ToInt32(resu_grid[11])<10){
							maskedTextBox9.Text="0"+resu_grid[11];
						}else{
							maskedTextBox9.Text=resu_grid[11];
						}
					}catch(Exception es4){
						maskedTextBox9.Text="0";
					}
					
					i=0;
					while(i<comboBox2.Items.Count){
						if(comboBox2.Items[i].ToString().Equals(resu_grid[13])){
							comboBox2.Text=resu_grid[13];
							comboBox2.SelectedIndex=i;
						}
						i++;
					}
					
					i=0;
					while(i<comboBox1.Items.Count){
						if(comboBox1.Items[i].ToString().Equals(resu_grid[12])){
							comboBox1.Text=resu_grid[12];
							comboBox1.SelectedIndex=i;
						}
						i++;
					}
					
					if(resu_grid[14].Length > 1){
						textBox9.Visible=false;
						dateTimePicker1.Text=resu_grid[14];
					}else{
						textBox9.Visible=true;
					}
					
					if(resu_grid[15].Length > 1){
						textBox10.Visible=false;
						dateTimePicker2.Text=resu_grid[15];
					}else{
						textBox10.Visible=true;
					}
					
					if(resu_grid[16].Length > 1){
						textBox11.Visible=false;
						dateTimePicker3.Text=resu_grid[16];
					}else{
						textBox11.Visible=true;
					}
					
					
					if(resu_grid[17].Equals("NN")){
						comboBox6.SelectedIndex=2;
						//checkBox1.Checked=true;
						//checkBox1.BackColor=System.Drawing.Color.MediumBlue;
					}else{
						if(resu_grid[17].Equals("-")){
							comboBox6.SelectedIndex=0;
						}else{
							if(resu_grid[17].Equals("ESTRADOS")){
							comboBox6.SelectedIndex=1;
							}
						}
						//checkBox1.Checked=false;
						//checkBox1.BackColor=System.Drawing.Color.Transparent;
					}
					
					i=0;
					while(i<comboBox3.Items.Count){
						if(comboBox3.Items[i].ToString().Equals(resu_grid[18])){
							comboBox3.Text=resu_grid[18];
							comboBox3.SelectedIndex=i;
						}
						i++;
					}
					
					textBox4.Text=resu_grid[19];
					textBox5.Text=resu_grid[20];
					
					if(resu_grid[21].Length > 1){
						textBox12.Visible=false;
						dateTimePicker4.Text=resu_grid[21];
					}else{
						textBox12.Visible=true;
					}
					
					if(resu_grid[22].Length>1){
						textBox13.Visible=false;
						dateTimePicker5.Text=resu_grid[22];
					}else{
						textBox13.Visible=true;
					}
					
					textBox6.Text=resu_grid[23];
					
					if(Convert.ToInt32(resu_grid[24])<10){
						maskedTextBox10.Text="0"+resu_grid[24];
					}else{
						maskedTextBox10.Text=resu_grid[24];
					}
					textBox7.Text=resu_grid[25];
					
					i=0;
					while(i<comboBox4.Items.Count){
						if(comboBox4.Items[i].ToString().Equals(resu_grid[26])){
							comboBox4.Text=resu_grid[26];
							comboBox4.SelectedIndex=i;
						}
						i++;
					}
					//MessageBox.Show(""+resu_grid[26]);
					id=resu_grid[27];
					
					//MessageBox.Show("|"+resu_grid[28]+"|"+resu_grid[28].Length);
					
					if(resu_grid[28].Length >1){
						textBox14.Visible=false;
						dateTimePicker6.Text=resu_grid[28];
					}else{
						textBox14.Visible=true;
					}
					
					textBox8.Text=resu_grid[29];
					
					maskedTextBox11.Clear();
					maskedTextBox12.Clear();
					maskedTextBox13.Clear();
					maskedTextBox11.Focus();
					
				}else{
					panel2.Visible=true;
					groupBox1.Enabled=false;
					groupBox2.Enabled=false;
					groupBox3.Enabled=false;
					comboBox4.Enabled=false;
					button3.Enabled=false;
					radioButton1.Enabled=false;
					radioButton2.Enabled=false;
					
					MessageBox.Show("No se encontró ningún registro con el registro patronal y crédito(s) ingresado(s).","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					
				}
				
				
			}else{
				MessageBox.Show("Falta información para efectuar la búsqueda.\nDebe ingresar por lo menos el registro patronal y  uno de los créditos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}
	
		public void modificacion_inicial(String reg_pat, String credi, String credi_mu){
			
			maskedTextBox11.Text=reg_pat;
			maskedTextBox12.Text=credi;
			maskedTextBox13.Text=credi_mu;
			
			rp=reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2)+reg_pat.Substring(13,1);
			cc=maskedTextBox12.Text;
			cm=maskedTextBox13.Text;
			
			maskedTextBox1.Clear();
			maskedTextBox2.Clear();
			maskedTextBox3.Clear();
			maskedTextBox4.Clear();
			maskedTextBox5.Text=sub_real;
			maskedTextBox6.Text="01";
			maskedTextBox7.Clear();
			maskedTextBox8.Clear();
			textBox1.Clear();
			textBox2.Clear();
			textBox3.Clear();
			
			buscar();
		}
		
		void InsertarLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			if (tipo_load==1){
				llenar_Cb4();
			}else{
				groupBox1.Enabled=false;
				groupBox2.Enabled=false;
				groupBox3.Enabled=false;
				comboBox4.Enabled=false;
				button3.Enabled=false;
				radioButton1.Enabled=false;
				radioButton2.Enabled=false;
				llenar_Cb4_todos();
			}
			
			int a=0;
			while(a<valores.Length){
				valores[a]="";
				a++;
			}
			
			llenar_Cb1();
			llenar_Cb2();
			sub_real=conex.leer_config_sub()[4];
			maskedTextBox5.Text=sub_real;
		}

        public bool checar_importe(String cadena)
        {
            double importe = 0.00;
            if (!cadena.Contains("."))
            {
                cadena = cadena + ".00";
            }

            if (double.TryParse(cadena, out importe))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
		//reg_pat
        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
        	
        }
        //periodo
        private void maskedTextBox2_Leave(object sender, EventArgs e)
        {
        	
        }
        //cred_cuota
        private void maskedTextBox3_Leave(object sender, EventArgs e)
        {
        	
        }
        //cred_mul
        private void maskedTextBox4_Leave(object sender, EventArgs e)
        {
        	
        }
        //razon social
        private void textBox1_Leave(object sender, EventArgs e)
        {
            
        }
        //importe_cuota
        private void textBox2_Leave(object sender, EventArgs e)
        {	
        	if(textBox2.Text.Length>0){
	        	if(checar_importe(textBox2.Text) == true){
	        			if (!textBox2.Text.Contains("."))
	        			{
	        				textBox2.Text = textBox2.Text + ".00";
	        			}
	        				valores[5] = textBox2.Text;
	        	}
        	}else{
        			valores[5]="";
        	}
        }
        //importe_multa
        private void textBox3_Leave(object sender, EventArgs e)
        {
        	if(textBox3.Text.Length>0){
        		if(checar_importe(textBox3.Text) == true){
        			if(!textBox3.Text.Contains(".")){
        				textBox3.Text = textBox3.Text + ".00";
        			}
        			valores[6] = textBox3.Text;
        		}
        	}else{
        			valores[6]="";
        	}
        }
        //sub
        private void maskedTextBox5_Leave(object sender, EventArgs e)
        {
            if (maskedTextBox5.Text.Length > 0){
        		if (maskedTextBox5.Text.Length == 1){
                	maskedTextBox5.Text="0"+maskedTextBox5.Text;
        		}
                	valores[7] = maskedTextBox5.Text;
                    maskedTextBox5.BackColor=Color.PaleGreen;
                    
            }
        }
        //incidencia
        private void maskedTextBox6_Leave(object sender, EventArgs e)
        {
            if (maskedTextBox6.Text.Length > 0){
        		if (maskedTextBox6.Text.Length == 1){
                	maskedTextBox6.Text="0"+maskedTextBox6.Text;
        		}
                	valores[8] = maskedTextBox6.Text;
                	 maskedTextBox6.BackColor=Color.PaleGreen;
            }
        }
		//tipo doc
        private void maskedTextBox7_Leave(object sender, EventArgs e)
        {
            if (maskedTextBox7.Text.Length > 0){
        		if (maskedTextBox7.Text.Length == 1){
                	maskedTextBox7.Text="0"+maskedTextBox7.Text;
        		}
                	valores[9] = maskedTextBox7.Text;
                	 maskedTextBox7.BackColor=Color.PaleGreen;
            }
        }
        //sector
        void MaskedTextBox8Leave(object sender, EventArgs e)
		{
			if(maskedTextBox8.Text.Length > 0){
        		if(maskedTextBox8.Text.Length == 1){
                	maskedTextBox8.Text="0"+maskedTextBox8.Text;
        		}
                	valores[10] = maskedTextBox8.Text;
                	 maskedTextBox8.BackColor=Color.PaleGreen;
             }
		}
		//nombre_periodo
		void ComboBox4Leave(object sender, EventArgs e)
		{
			if(comboBox4.SelectedIndex>-1){
				
				valores[11]=comboBox4.SelectedItem.ToString();
				 comboBox4.BackColor=Color.PaleGreen;
				 this.GetNextControl(ActiveControl,true);
			}else{
				valores[11] = "";
				 comboBox4.BackColor=Color.Tomato;
			}
		}
        
		private void timer1_Tick(object sender, EventArgs e)
        {
            int k = 0,activar=0;
           
            while (k < 12)
            {	try{
	            	if(valores[k].Length>0){
	                    activar++;
	                }
                
            	}catch(Exception fv){}
            	k++;
            }
			            
            if(activar==10){
            	if((valores[2].Length==9) && (valores[5].Length > 0)){
            		if(maskedTextBox4.Text.Length==0 && textBox3.Text.Length==0){
		            		activar=12;
		            		maskedTextBox4.BackColor=Color.PaleGreen;
		            		textBox3.BackColor=Color.PaleGreen;
		            		//MessageBox.Show("val2: "+valores[2].Length+" val3: "+valores[3].Length+" val5: "+valores[5].Length+" val6: "+valores[6].Length);
	            	}
            	}
            	
            	if(valores[3].Length==9 && valores[6].Length > 0){
            		if(maskedTextBox3.Text.Length==0 && textBox2.Text.Length==0){
	            		activar=12;
	            		maskedTextBox3.BackColor=Color.PaleGreen;
	            		textBox2.BackColor=Color.PaleGreen;
            		}
            	}
            }
            
            if(activar>0 && activar<12){
            	label31.Text="Incompleto";
            	label31.ForeColor=Color.DarkOrange;
            	label32.Text="Incompleto";
            	label32.ForeColor=Color.DarkOrange;
            	label33.Text="Incompleto";
            	label33.ForeColor=Color.DarkOrange;
            	//button1.Enabled = false;
            }else{
            	
            	if (activar == 12)
            	{
            		//button1.Enabled = true;
            		label31.Text="Completo";
            		label31.ForeColor=Color.LawnGreen;
            		label32.Text="Opcional";
            		label32.ForeColor=Color.DeepSkyBlue;
            		label33.Text="Opcional";
            		label33.ForeColor=Color.DeepSkyBlue;
            		maskedTextBox4.BackColor=Color.PaleGreen;
            		textBox3.BackColor=Color.PaleGreen;
            		maskedTextBox3.BackColor=Color.PaleGreen;
            		textBox2.BackColor=Color.PaleGreen;
            		
            	}
            	else
            	{
            		//button1.Enabled = false;
            		label31.Text="Obligatorio";
            		label31.ForeColor=Color.Red;
            		label32.Text="Incompleto";
            		label32.ForeColor=Color.DarkOrange;
            		label33.Text="Incompleto";
            		label33.ForeColor=Color.DarkOrange;
            		
            	}
            	
            }
            
        }
		
        void Label6Click(object sender, EventArgs e)
		{
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(tipo_load==1){
				guardar();
			}else{
				guardar_mod();
			}
		}
	
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex > -1){
				try{
					llenar_Cb2_v2();
				}catch(Exception es){
					comboBox2.Items.Clear();
				}
			}else{
				if(comboBox1.Text.Length<1){
					comboBox2.Items.Clear();
				}else{
					llenar_Cb2();
				}
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			String tipo="";
			int j=0,mayor=0;
			
			if(radioButton1.Checked==true){
				tipo="COP";
			}
			
			if(radioButton2.Checked==true){
				tipo="RCV";
			}
			
			while(j<comboBox4.Items.Count){
				if(tipo.Equals(comboBox4.Items[j].ToString().Substring(0,3))){
					
					if(Convert.ToInt32(comboBox4.Items[j].ToString().Substring(7)) > mayor){
						mayor=Convert.ToInt32(comboBox4.Items[j].ToString().Substring(7));
					}
				}
				j++;
			}
			
			mayor=mayor+1;
			comboBox4.Items.Add(tipo+"_CM_"+mayor.ToString());
			comboBox4.SelectedIndex=j;
			valores[11]=comboBox4.SelectedItem.ToString();
			comboBox4.BackColor=Color.PaleGreen;
			
			button3.Enabled=false;
			radioButton1.Enabled=false;
			radioButton2.Enabled=false;
		}
		
		//reg_pat
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			String nrp,cad="";
			i=0;
			
			nrp=maskedTextBox1.Text;
			
			while (i < nrp.Length){
				if (((nrp.Substring(i, 1)).Equals(" ")) || ((nrp.Substring(i, 1)).Equals("-")))
				{
				}
				else
				{
					cad += nrp.Substring(i, 1);
				}
				i++;
			}
			
			if(cad.Length>9){
				valores[0] = cad.ToUpper();
				
				maskedTextBox1.BackColor=Color.PaleGreen;
			}
			else
			{
				valores[0] = "";
				maskedTextBox1.BackColor=Color.Tomato;
			}
			
			if(maskedTextBox1.Text.Length==20){
				maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
				maskedTextBox2.Focus();
			}
		}
		//periodo
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			int anio=0,mes=0;
        	
            if (maskedTextBox2.Text.Length == 6)
            {
            	anio=Convert.ToInt32(maskedTextBox2.Text.Substring(0,4));
            	mes=Convert.ToInt32(maskedTextBox2.Text.Substring(4,2));
            	
            	if(anio > 2000){
            		if(mes>0 && mes<13){
            			valores[1] = maskedTextBox2.Text;
            			maskedTextBox2.BackColor= Color.PaleGreen;
            			
            		}else{
            			valores[1] = "";
            			maskedTextBox2.BackColor=Color.Tomato;
            		}
            	}else{
            		valores[1] = "";
            		maskedTextBox2.BackColor=Color.Tomato;
            	}
            }
            else
            {
                valores[1] = "";
                maskedTextBox2.BackColor=Color.Tomato;
            }
            
			if(maskedTextBox2.Text.Length==6){
				this.GetNextControl(ActiveControl,true).Focus();
			}
		}
		//cred_cuota
		void MaskedTextBox3TextChanged(object sender, EventArgs e)
		{
			String cred_cuo,cad="";
        	i=0;
        	cred_cuo=maskedTextBox3.Text;
        		
        	while (i < cred_cuo.Length){
				if ((cred_cuo.Substring(i, 1)).Equals(" "))
				{
				}
				else
				{
					cad += cred_cuo.Substring(i, 1);
				}
				i++;
			}
        	
            if (cad.Length >= 9)
            {
                valores[2] = cad;
                maskedTextBox3.BackColor=Color.PaleGreen;
            }
            else
            {
                valores[2] = "";
                maskedTextBox3.BackColor=Color.Tomato;
            }
			
			if(maskedTextBox3.Text.Length==9){
				this.GetNextControl(ActiveControl,true).Focus();
			}
		}
		//cred_mul
		void MaskedTextBox4TextChanged(object sender, EventArgs e)
		{
			String cred_cuo,cad="";
        	i=0;
        	cred_cuo=maskedTextBox4.Text;
        		
        	while (i < cred_cuo.Length){
				if ((cred_cuo.Substring(i, 1)).Equals(" "))
				{
				}
				else
				{
					cad += cred_cuo.Substring(i, 1);
				}
				i++;
			}
        	
            if (cad.Length == 9)
            {
                valores[3] = cad;
                maskedTextBox4.BackColor=Color.PaleGreen;
            }
            else
            {
                valores[3] = "";
                maskedTextBox4.BackColor=Color.Tomato;
            }
            
			if(maskedTextBox4.Text.Length==9){
				textBox1.Focus();
			}
		}
		//razon social
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			if (textBox1.Text.Length > 5)
            {
                valores[4] = textBox1.Text.ToUpper();
              //textBox1.CharacterCasing=CharacterCasing.Upper;
                textBox1.BackColor=Color.PaleGreen;
                this.GetNextControl(ActiveControl,true);
            }
            else
            {
                valores[4] = "";
                textBox1.BackColor=Color.Tomato;
            }
		}
		//importe_cuota
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			if(textBox2.Text.Length>0){
        		if (checar_importe(textBox2.Text) == true)
        		{
        			textBox2.BackColor=Color.PaleGreen;
        			valores[5] = textBox2.Text;
        		}
        		else
        		{
        			valores[5] = "";
        			textBox2.BackColor=Color.Tomato;
        		}
        	}else{
        		valores[5] = "";
        		textBox2.BackColor=Color.Tomato;
        	}
		}
		//importe_multa
		void TextBox3TextChanged(object sender, EventArgs e)
		{
			if(textBox3.Text.Length>0){
        		if (checar_importe(textBox3.Text) == true)
        		{
        			textBox3.BackColor=Color.PaleGreen;
        			valores[6] = textBox3.Text;
        		}
        		else
        		{
        			valores[6] = "";
        			textBox3.BackColor=Color.Tomato;
        		}
        	}else{
        		valores[6] = "";
        		textBox3.BackColor=Color.Tomato;
        	}
		}
		//sub
		void MaskedTextBox5TextChanged(object sender, EventArgs e)
		{
			if (maskedTextBox5.Text.Length == 2)
            {
                valores[7] = maskedTextBox5.Text;
                maskedTextBox5.BackColor=Color.PaleGreen; 
            }
            else
            {
                valores[7] = "";
                maskedTextBox5.BackColor=Color.Tomato;
            }
            
			if(maskedTextBox5.Text.Length==2){
				maskedTextBox6.Focus();
			}
		}
		//incidencia
		void MaskedTextBox6TextChanged(object sender, EventArgs e)
		{
			if (maskedTextBox6.Text.Length == 2)
            {
                valores[8] = maskedTextBox6.Text;
                 maskedTextBox6.BackColor=Color.PaleGreen;
            }
            else
            {
                valores[8] = "";
                 maskedTextBox6.BackColor=Color.Tomato;
            }
            
			if(maskedTextBox6.Text.Length==2){
				maskedTextBox7.Focus();
			}
		}
		//tipo doc
		void MaskedTextBox7TextChanged(object sender, EventArgs e)
		{
			if (maskedTextBox7.Text.Length == 2)
            {
                valores[9] = maskedTextBox7.Text;
                 maskedTextBox7.BackColor=Color.PaleGreen;
            }
            else
            {
                valores[9] = "";
                 maskedTextBox7.BackColor=Color.Tomato;
                 
            }
            
			if(maskedTextBox7.Text.Length==2){
				maskedTextBox8.Focus();
			}
		}
		 //sector
		void MaskedTextBox8TextChanged(object sender, EventArgs e)
		{
			if (maskedTextBox8.Text.Length == 2)
            {
                valores[10] = maskedTextBox8.Text;
                 maskedTextBox8.BackColor=Color.PaleGreen;
            }
            else
            {
                valores[10] = "";
                 maskedTextBox8.BackColor=Color.Tomato;
            }
			
			if(maskedTextBox8.Text.Length==2){
				if(comboBox4.SelectedIndex>-1){
					button1.Focus();
				}else{
					comboBox4.Focus();
				}
			}
		}
		
		void ComboBox4SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox4.SelectedIndex>-1){
				
				valores[11]=comboBox4.SelectedItem.ToString();
				 comboBox4.BackColor=Color.PaleGreen;
				 this.GetNextControl(ActiveControl,true);
			}else{
				valores[11] = "";
				 comboBox4.BackColor=Color.Tomato;
			}
		}
		
		void ComboBox4Enter(object sender, EventArgs e)
		{
			//if(comboBox4.SelectedIndex>-1){
			//	button1.Focus();
			//}
		}
		
		void MaskedTextBox1Validating(object sender, CancelEventArgs e)
		{
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			maskedTextBox1.Clear();
			maskedTextBox2.Clear();
			maskedTextBox3.Clear();
			maskedTextBox4.Clear();
			maskedTextBox5.Text=sub_real;
			maskedTextBox6.Text="01";
			maskedTextBox7.Clear();
			maskedTextBox8.Clear();
			textBox1.Clear();
			textBox2.Clear();
			textBox3.Clear();
			
			buscar();
		}
		
		void MaskedTextBox11TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox11.Text.Length==20){
				maskedTextBox12.Focus();
			}
		}
		
		void MaskedTextBox12TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox12.Text.Length==9){
				maskedTextBox13.Focus();
			}
		}
		
		void MaskedTextBox13TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox13.Text.Length==9){
				button2.Focus();
			}
		}
		
		void MaskedTextBox11Leave(object sender, EventArgs e)
		{
			String nrp,cad="";
			i=0;
			
			nrp=maskedTextBox11.Text;
			
			while (i < nrp.Length){
				if (((nrp.Substring(i, 1)).Equals(" ")) || ((nrp.Substring(i, 1)).Equals("-")))
				{
				}
				else
				{
					cad += nrp.Substring(i, 1);
				}
				i++;
			}
			
			if(cad.Length>9){
				//valores[0] = cad.ToUpper();
				maskedTextBox11.Text = maskedTextBox11.Text.ToUpper();
				rp=cad;
				//maskedTextBox11.BackColor=Color.PaleGreen;
			}
			else
			{
				rp="";
				//valores[0] = "";
				//maskedTextBox11.BackColor=Color.Tomato;
			}
		}
		
		void MaskedTextBox12Leave(object sender, EventArgs e)
		{
			String cred_cuo,cad="";
        	i=0;
        	cred_cuo=maskedTextBox12.Text;
        		
        	while (i < cred_cuo.Length){
				if ((cred_cuo.Substring(i, 1)).Equals(" "))
				{
				}
				else
				{
					cad += cred_cuo.Substring(i, 1);
				}
				i++;
			}
        	
            if (cad.Length == 9)
            {
            	cc=cad;
               // valores[2] = cad;
                //maskedTextBox12.BackColor=Color.PaleGreen;
               // this.GetNextControl(ActiveControl,true);
            }
            else
            {
                //valores[2] = "";
                //maskedTextBox12.BackColor=Color.Tomato;
                cc="";
            }
		}
		
		void MaskedTextBox13Leave(object sender, EventArgs e)
		{
			String cred_cuo,cad="";
        	i=0;
        	cred_cuo=maskedTextBox13.Text;
        		
        	while (i < cred_cuo.Length){
				if ((cred_cuo.Substring(i, 1)).Equals(" "))
				{
				}
				else
				{
					cad += cred_cuo.Substring(i, 1);
				}
				i++;
			}
        	
            if (cad.Length == 9)
            {
                //valores[2] = cad;
                //maskedTextBox13.BackColor=Color.PaleGreen;
                //this.GetNextControl(ActiveControl,true);
                cm=cad;
            }
            else
            {
                //valores[2] = "";
               //maskedTextBox13.BackColor=Color.Tomato;
               cm="";
            }
		}
			
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox2.Checked==true){
				dateTimePicker1.Enabled=true;
				textBox9.Visible=false;
			}else{
				dateTimePicker1.Enabled=false;
				
			}
		}
		
		void CheckBox3CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox3.Checked==true){
				dateTimePicker2.Enabled=true;
				textBox10.Visible=false;
			}else{
				dateTimePicker2.Enabled=false;
			}
		}
		
		void CheckBox4CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox4.Checked==true){
				dateTimePicker3.Enabled=true;
				textBox11.Visible=false;
			}else{
				dateTimePicker3.Enabled=false;
			}
		}
		
		void CheckBox5CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox5.Checked==true){
				dateTimePicker4.Enabled=true;
				textBox12.Visible=false;
			}else{
				dateTimePicker4.Enabled=false;
			}
		}
		
		void CheckBox6CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox6.Checked==true){
				dateTimePicker5.Enabled=true;
				textBox13.Visible=false;
			}else{
				dateTimePicker5.Enabled=false;
			}
		}

		void ComboBox2MouseClick(object sender, MouseEventArgs e)
		{
			if(comboBox1.Text.Length<1){
				comboBox1.SelectedIndex=-1;
				llenar_Cb2();
			}
		}
		
		void ComboBox5SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox5.SelectedIndex==5||comboBox5.SelectedIndex==11){
				maskedTextBox14.Enabled=false;
				maskedTextBox14.BackColor= Color.PaleGreen;
			}else{
				maskedTextBox14.Enabled=true;
				maskedTextBox14.BackColor= Color.White;
			}
		}
		
		void MaskedTextBox14TextChanged(object sender, EventArgs e)
		{
			int anio=0,mes=0;
        	
			if (maskedTextBox14.Text.Length == 6)
            {
            	anio=Convert.ToInt32(maskedTextBox14.Text.Substring(0,4));
            	mes=Convert.ToInt32(maskedTextBox14.Text.Substring(4,2));
            	
            	if(anio > 2000){
            		if(mes>0 && mes<13){
            			//valores[1] = maskedTextBox14.Text;
            			maskedTextBox14.BackColor= Color.PaleGreen;
            			
            		}else{
            			//valores[1] = "";
            			maskedTextBox14.BackColor=Color.Tomato;
            		}
            	}else{
            		//valores[1] = "";
            		maskedTextBox14.BackColor=Color.Tomato;
            	}
            }
            else
            {
                //valores[1] = "";
                if(maskedTextBox14.Mask.Equals("000")){
                   	maskedTextBox14.BackColor= Color.PaleGreen;
                }else{
                	maskedTextBox14.BackColor= Color.Tomato;
                }
            }
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			String periodo_nvo;
			int j=0,k=0,mayor=0;
			
			comboBox5.Enabled=true;
			
			if((maskedTextBox14.BackColor.Name.Equals("PaleGreen")) && (comboBox5.SelectedIndex>-1)){
				if(maskedTextBox14.Text.Length>5){
                    //rcv
					if(comboBox5.SelectedItem.ToString().StartsWith("RCV")){
						if(Convert.ToInt32(maskedTextBox14.Text.Substring(4,2))< 7){
							periodo_nvo=comboBox5.SelectedItem.ToString()+"_"+maskedTextBox14.Text;
							
							while(j < comboBox4.Items.Count){
								if(comboBox4.SelectedItem.ToString().Equals(periodo_nvo)){
									k=1;
								}
								j++;
							}
							
							if(k==0){
								comboBox4.Items.Add(periodo_nvo);
								comboBox4.SelectedIndex=comboBox4.Items.Count-1;
								comboBox5.Enabled=false;
								maskedTextBox14.Enabled=false;
								button4.Enabled=false;
							}else{
								MessageBox.Show("El periodo que intenta crear, ya existe","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
							}
						}
					}else{//COP
						periodo_nvo=comboBox5.SelectedItem.ToString()+"_"+maskedTextBox14.Text;
						while(j < comboBox4.Items.Count){
								if(comboBox4.SelectedItem.ToString().Equals(periodo_nvo)){
									k=1;
								}
								j++;
							}
							
							if(k==0){
								comboBox4.Items.Add(periodo_nvo);
								comboBox4.SelectedIndex=comboBox4.Items.Count-1;
								comboBox5.Enabled=false;
								maskedTextBox14.Enabled=false;
								button4.Enabled=false;
							}else{
								MessageBox.Show("El periodo que intenta crear ya existe","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
							}
					}
				}else{
						periodo_nvo=comboBox5.SelectedItem.ToString();
						j=0;
						mayor=0;
						//MessageBox.Show(periodo_nvo+"|"+j+"|"+comboBox4.Items[j].ToString().Substring(0,5));
						while(j<comboBox4.Items.Count){
							if(periodo_nvo.Equals(comboBox4.Items[j].ToString().Substring(0,6))){
								if(Convert.ToInt32(comboBox4.Items[j].ToString().Substring(7)) > mayor){
									mayor=Convert.ToInt32(comboBox4.Items[j].ToString().Substring(7));
								}
							}
							j++;
						}
						
						periodo_nvo=periodo_nvo+"_"+(mayor+1);
						comboBox4.Items.Add(periodo_nvo);
						comboBox4.SelectedIndex=j;
						comboBox5.Enabled=false;
						maskedTextBox14.Enabled=false;
						button4.Enabled=false;
					
				}
			}
		}
		
		void TextBox6TextChanged(object sender, EventArgs e)
		{
			if(textBox6.Text.Length>0){
        		if (checar_importe(textBox6.Text) == true)
        		{
        			textBox6.BackColor=Color.PaleGreen;
        		}
        		else
        		{
        			textBox6.BackColor=Color.Tomato;
        		}
        	}else{
        		textBox6.BackColor=Color.Tomato;
        	}
		}
		
		void TextBox6Leave(object sender, EventArgs e)
		{
			if(checar_importe(textBox6.Text) == true){
        			if (!textBox6.Text.Contains("."))
        			{
        				textBox6.Text = textBox6.Text + ".00";
        			}	
        	}
		}

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 7)
            {
                textBox8.Enabled=true;
                checkBox7.Checked=true;
                checkBox7.Enabled=true;
                label21.BackColor=System.Drawing.Color.MediumBlue;
                //dateTimePicker6.Enabled = true;
            }
            else
            {
                textBox8.Enabled=false;
                checkBox7.Checked=false;
                checkBox7.Enabled=false;
                label21.BackColor=System.Drawing.Color.Transparent;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                dateTimePicker6.Enabled = true;
                textBox14.Visible=false;
            }
            else
            {
                dateTimePicker6.Enabled = false;
            }
        }
		
		void Button5Click(object sender, EventArgs e)
		{
			maskedTextBox1.Clear();
			maskedTextBox2.Clear();
			maskedTextBox3.Clear();
			maskedTextBox4.Clear();
			maskedTextBox5.Text=sub_real;
			maskedTextBox6.Text="01";
			maskedTextBox7.Clear();
			maskedTextBox8.Clear();
			textBox1.Clear();
			textBox2.Clear();
			textBox3.Clear();
			maskedTextBox1.Focus();
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
				textBox3.Focus();
			}
		}
		
		void TextBox3KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox5.Focus();
			}
		}
		
		void MaskedTextBox5KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox6.Focus();
			}
		}
		
		void MaskedTextBox6KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox7.Focus();
			}
		}
		
		void MaskedTextBox7KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox8.Focus();
			}
		}
		
		void MaskedTextBox8KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				if(comboBox4.SelectedIndex>-1){
						button1.Focus();
					}else{
						comboBox4.Focus();
				}
			}
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true){
				checkBox1.BackColor=System.Drawing.Color.MediumBlue;
			}else{
				checkBox1.BackColor=System.Drawing.Color.Transparent;
			}
		}
		
		void TextBox9TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void Label30Click(object sender, EventArgs e)
		{
			
		}
		
		void CheckBox8CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox8.Checked==true){
				textBox9.Visible=true;
			}else{
				textBox9.Visible=false;
			}
		}
		
		void CheckBox9CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox9.Checked==true){
				textBox10.Visible=true;
			}else{
				textBox10.Visible=false;
			}
		}
		
		void CheckBox10CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox10.Checked==true){
				textBox11.Visible=true;
			}else{
				textBox11.Visible=false;
			}
		}
		
		void CheckBox11CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox11.Checked==true){
				textBox12.Visible=true;
			}else{
				textBox12.Visible=false;
			}
		}
		
		void CheckBox12CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox12.Checked==true){
				textBox13.Visible=true;
			}else{
				textBox13.Visible=false;
			}
		}
		
		void CheckBox13CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox13.Checked==true){
				textBox14.Visible=true;
			}else{
				textBox14.Visible=false;
			}
		}
		
		void MaskedTextBox11KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox12.Focus();
			}
		}
		
		void MaskedTextBox12KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				maskedTextBox13.Focus();
			}
		}
		
		void MaskedTextBox13KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				button2.Focus();
			}
		}
	}
}

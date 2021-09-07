/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 27/09/2017
 * Hora: 04:40 p.m.
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

namespace Nova_Gear.Estrados
{
	/// <summary>
	/// Description of Captura_info_estrados.
	/// </summary>
	public partial class Captura_info_estrados : Form
	{
		public Captura_info_estrados(int tipo,int id_ofi,String rp_cc_cm)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.tipo_inicio=tipo;
            this.id_oficio=id_ofi;
            this.datos_consu=rp_cc_cm;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//Conexion MySQL
		Conexion conex0 = new Conexion();
        Conexion conex = new Conexion();
        Conexion conex1 = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        Conexion conex4 = new Conexion();
        Conexion conex5 = new Conexion();
        Conexion conex6 = new Conexion();
        
        DataTable tabla_dias_fest =new DataTable();
        DataTable tabla_datos_fact =new DataTable();
        DataTable oficios =new DataTable();
        DataTable tabla_sindo =new DataTable();
        DataTable tabla_estrados =new DataTable();
        DataTable tabla_estrados2 =new DataTable();
         
        

        int tipo_inicio,id_user,id_oficio;
        String subdele;
        String puesto, rango, id_us,id_dat_fact, datos_consu;
        String[] datos_sub = new String[11];
        
        public void leer_config()
        {
            String del, del_num, mpio, sub, sub_num,jefe_cob, jefe_emi, jefe_afi,ref_baja,ref_ofi;
            //try
            //{
                StreamReader rdr = new StreamReader(@"sub_config.lz");

                del=rdr.ReadLine();
                del_num=rdr.ReadLine();
                mpio=rdr.ReadLine();
                sub=rdr.ReadLine(); 
                sub_num=rdr.ReadLine(); 
                subdele=rdr.ReadLine();
                jefe_afi = rdr.ReadLine();
                jefe_cob=rdr.ReadLine(); 
                jefe_emi=rdr.ReadLine();                 
                //ref_baja = rdr.ReadLine();
                //ref_ofi = rdr.ReadLine();
                rdr.Close();

                del=del.Substring(11, del.Length - 11);
                del_num=del_num.Substring(7, del_num.Length - 7);
                mpio = mpio.Substring(10, mpio.Length - 10);
                sub = sub.Substring(14, sub.Length - 14);
                sub_num = sub_num.Substring(7, sub_num.Length - 7);
                subdele = subdele.Substring(12, subdele.Length - 12);
                jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);
                jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
                jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
                
                //ref_baja = ref_baja.Substring(9, ref_baja.Length - 9);
                //ref_ofi = ref_ofi.Substring(8, ref_ofi.Length - 8);
                
            //}
            //catch (Exception error)
            //{
                //MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            //}
        }
        
        public void colocar_folio(){
        	
        	String folio_act="",folio_nvo="",sub="",del="";
        	int num_fol=0;
        	
        	conex6.conectar("base_principal");
            del=conex6.leer_config_sub()[1];
            sub = conex6.leer_config_sub()[4];

        	tabla_estrados2=conex6.consultar("SELECT folio FROM estrados ORDER BY id_estrados DESC LIMIT 1 ");
        	if(tabla_estrados2.Rows.Count>0){
	        	folio_act=tabla_estrados2.Rows[0][0].ToString();
	        	if((Convert.ToInt32(folio_act.Substring(6,2)))<(Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2,2)))){
	        		folio_act="E"+del+sub+"/"+(Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2,2)))+"-00001";
	        	}else{
	        		num_fol=(Convert.ToInt32(folio_act.Substring(9,folio_act.Length-9)));
	        		num_fol=num_fol+1;
	        		if(num_fol<10){
	        			folio_act=folio_act.Substring(0,9)+"0000"+num_fol.ToString();
	        		}else{
	        			if(num_fol<100){
	        				folio_act=folio_act.Substring(0,9)+"000"+num_fol.ToString();
	        			}else{
	        				if(num_fol<1000){
	        					folio_act=folio_act.Substring(0,9)+"00"+num_fol.ToString();
		        			}else{
                                if (num_fol < 10000)
                                {
                                    folio_act = folio_act.Substring(0, 9) + "0" + num_fol.ToString();
                                }
                                else
                                {
                                    folio_act = folio_act.Substring(0, 9) + num_fol.ToString();
                                }
		        			}
	        			}
	        		}
	        	}
	        	
        	}else{
                folio_act = "E" + del + sub + "/" + (Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2, 2))) + "-00000";
        	}
        	
        	maskedTextBox1.Text=folio_act;
        	maskedTextBox1.ReadOnly=true;
        	
        }
        
        public void llena_auto(){
        	
        	leer_config();
        	try{
        	if(tabla_datos_fact.Rows[0][9].ToString().Equals("38")==true){
        		comboBox2.SelectedIndex=0;
        		textBox10.Text=subdele.ToUpper();
        	}
        	
        	switch(tabla_datos_fact.Rows[0][10].ToString()){
        		case "0": comboBox3.SelectedIndex=4;
        		break;
        		case "2": comboBox3.SelectedIndex=0;
        		break;
        		case "3": 
	        		if(tabla_datos_fact.Rows[0][10].ToString().StartsWith("COP")==true){
	        			comboBox3.SelectedIndex=2;
	        		}else{
	        			comboBox3.SelectedIndex=3;
	        		}
        		break;
        		case "6": comboBox3.SelectedIndex=1;
        		break;
        	}
        	}catch{}
        	dateTimePicker4.Value=cuenta_dias_hab(dateTimePicker3.Value,1);
        	dateTimePicker5.Value=cuenta_dias_hab(dateTimePicker3.Value,2);
            dateTimePicker6.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(datos_sub[11]) + 1));
            dateTimePicker7.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(datos_sub[11]) + 2));
        }
        
        public void consulta(){
        	String reg_pat,cc="",cm="",cad="";
			int i=0;
			conex.conectar("base_principal");
			conex1.conectar("base_principal");
			conex2.conectar("base_principal");

            datos_sub = conex.leer_config_sub();
			
			comboBox1.SelectedIndex=-1;
			comboBox1.Text="";
			comboBox2.SelectedIndex=-1;
			comboBox2.Text="";
			comboBox3.SelectedIndex=-1;
			comboBox3.Text="";
			dateTimePicker1.Value=DateTime.Today;
			dateTimePicker2.Value=DateTime.Today;
			dateTimePicker3.Value=DateTime.Today;
			dateTimePicker4.Value=DateTime.Today;
			dateTimePicker5.Value=DateTime.Today;
			dateTimePicker6.Value=DateTime.Today;
			dateTimePicker7.Value=DateTime.Today;
			maskedTextBox1.Clear();
			maskedTextBox2.Clear();
			maskedTextBox3.Clear();
			maskedTextBox3.Text="12:00";
			textBox1.Text="";
			textBox2.Text="";
			textBox3.Text="";
			textBox4.Text="";
			textBox5.Text="";
			textBox6.Text="";
			textBox8.Text="";
			textBox10.Text="";
			textBox11.Text="";
			textBox12.Text="";
			textBox13.Text="";
			label7.Text="Registro Patronal:";    
        	label5.Text="Credito Cuota:";
        	label6.Text="Credito Multa:";
        	label8.Text="Periodo:";
			//groupBox2.Enabled=false;
			//groupBox3.Enabled=false;
			desactivar_controles();
			if(tipo_inicio==2){
				button4.Enabled=true;
				maskedTextBox1.ReadOnly=false;
			}
			
			reg_pat=maskedTextBox11.Text;
			while (i < reg_pat.Length){
				if (((reg_pat.Substring(i, 1)).Equals(" ")) || ((reg_pat.Substring(i, 1)).Equals("-"))){ 
				}else{
					cad += reg_pat.Substring(i, 1);
				}
				i++;
			}
			
			if(cad.Length>9){
				maskedTextBox11.Text = maskedTextBox11.Text.ToUpper();
				reg_pat=cad;
				
			}else{
				reg_pat="";
			}
			
			if(reg_pat.Length>9){
				if((maskedTextBox12.MaskCompleted==true)||(maskedTextBox13.MaskCompleted==true)){
					if(maskedTextBox12.MaskCompleted==true){
						cc=" AND credito_cuotas=\""+maskedTextBox12.Text+"\"";
					}
					
					if(maskedTextBox13.MaskCompleted==true){
						cm=" AND credito_multa=\""+maskedTextBox13.Text+"\"";
					}
					
					tabla_datos_fact=conex.consultar("SELECT id,registro_patronal,credito_cuotas,credito_multa,periodo,razon_social,notificador,registro_patronal1,nombre_periodo,subdelegacion,tipo_documento FROM datos_factura WHERE registro_patronal2=\""+reg_pat.Substring(0,10)+"\" "+cc+" "+cm+"");
					
					if(tabla_datos_fact.Rows.Count>0){
						textBox1.Text=tabla_datos_fact.Rows[0][1].ToString();
						textBox2.Text=tabla_datos_fact.Rows[0][2].ToString();
						textBox3.Text=tabla_datos_fact.Rows[0][3].ToString();
						textBox4.Text=tabla_datos_fact.Rows[0][4].ToString();
						textBox12.Text=tabla_datos_fact.Rows[0][5].ToString();
						textBox13.Text=tabla_datos_fact.Rows[0][6].ToString();
						id_dat_fact=tabla_datos_fact.Rows[0][0].ToString();
						
						if(tipo_inicio==1){//si es captura o modificacion
							//groupBox2.Enabled=true;
							//groupBox3.Enabled=true;
							activar_controles();
							textBox11.Enabled=true;
							button1.Enabled=true;
							
						}
						
						tabla_sindo=conex1.consultar("SELECT id_sindo,domicilio,localidad,cp,rfc FROM sindo WHERE registro_patronal= \""+tabla_datos_fact.Rows[0][7].ToString()+"\"");
						tabla_estrados=conex2.consultar("SELECT id_estrados,nombre_documento,fecha_emision_doc,fecha_acta_circunstanciada,fojas,sub_emisora,titular_sub,folio,supuesto_estrados,motivo_estrados,"+
					                               		"fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,hora_not,observaciones,paquete FROM estrados WHERE id_credito=\""+tabla_datos_fact.Rows[0][0].ToString()+"\"");
						
						if(tabla_sindo.Rows.Count>0){
							textBox5.Text=tabla_sindo.Rows[0][1].ToString()+" "+tabla_sindo.Rows[0][2].ToString()+" "+tabla_sindo.Rows[0][3].ToString();
							textBox6.Text=tabla_sindo.Rows[0][4].ToString();
						}
					
						if(tabla_estrados.Rows.Count>0){
							//textBox7.Text=tabla_estrados.Rows[0][1].ToString();
							comboBox3.Text=tabla_estrados.Rows[0][1].ToString();
							dateTimePicker1.Value=Convert.ToDateTime(tabla_estrados.Rows[0][2].ToString());
							dateTimePicker2.Value=Convert.ToDateTime(tabla_estrados.Rows[0][3].ToString());
							maskedTextBox2.Text=tabla_estrados.Rows[0][4].ToString();
							//textBox5.Text=tabla_estrados.Rows[0][5].ToString();
							comboBox2.Text=tabla_estrados.Rows[0][5].ToString();
							textBox10.Text=tabla_estrados.Rows[0][6].ToString();
							
							maskedTextBox1.Text=tabla_estrados.Rows[0][7].ToString();
							comboBox1.Text=tabla_estrados.Rows[0][8].ToString();
							textBox8.Text=tabla_estrados.Rows[0][9].ToString();
							dateTimePicker3.Value=Convert.ToDateTime(tabla_estrados.Rows[0][10].ToString());
							dateTimePicker4.Value=Convert.ToDateTime(tabla_estrados.Rows[0][11].ToString());
							dateTimePicker5.Value=Convert.ToDateTime(tabla_estrados.Rows[0][12].ToString());
							dateTimePicker6.Value=Convert.ToDateTime(tabla_estrados.Rows[0][13].ToString());
							dateTimePicker7.Value=Convert.ToDateTime(tabla_estrados.Rows[0][14].ToString());
							maskedTextBox3.Text=tabla_estrados.Rows[0][15].ToString();
							textBox11.Text=tabla_estrados.Rows[0][16].ToString();
                            textBox14.Text = tabla_estrados.Rows[0][17].ToString();
							if(tipo_inicio==1){
								button3.Enabled=true;
								maskedTextBox1.ReadOnly=true;
							}
						}else{//si no existe en estrados
							//MessageBox.Show(""+tipo_inicio);
							if(tipo_inicio==2){
								
							}else{
								llena_auto();
							}
							if(tipo_inicio==1){
								button3.Enabled=false;
							}
						}
					
					}else{
						//groupBox2.Enabled=false;
						//groupBox3.Enabled=false;
						desactivar_controles();
						textBox11.Enabled=false;
						button1.Enabled=false;
					}
				}else{
                    if (tipo_inicio == 4)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Escriba por lo menos un número de crédito", "ERROR");
                    }
				}
			}else{
                if (tipo_inicio == 4)
                {
                }
                else
                {
                    MessageBox.Show("Escriba el Registro Patronal a Buscar", "ERROR");
                }
			}
        }
        
        public void consulta_folio(){
        	if(maskedTextBox1.MaskCompleted==true){
				conex.conectar("base_principal");
				conex2.conectar("base_principal");
				tabla_estrados=conex.consultar("SELECT id_credito FROM estrados WHERE folio=\""+maskedTextBox1.Text+"\"");
				if(tabla_estrados.Rows.Count>0){
					if(!tabla_estrados.Rows[0][0].ToString().StartsWith("OFI")){
						tabla_datos_fact=conex2.consultar("SELECT registro_patronal,credito_cuotas,credito_multa FROM datos_factura WHERE id="+tabla_estrados.Rows[0][0].ToString()+"");
						
						if(tabla_datos_fact.Rows.Count>0){
							maskedTextBox11.Text=tabla_datos_fact.Rows[0][0].ToString();
							maskedTextBox12.Text=tabla_datos_fact.Rows[0][1].ToString();
							maskedTextBox13.Text=tabla_datos_fact.Rows[0][2].ToString();
							consulta();
						}
					}else{
						id_oficio=Convert.ToInt32(tabla_estrados.Rows[0][0].ToString().Substring(4,4));
						carga_oficio();
					}
				}
			}
        }
        
        public DateTime cuenta_dias_hab(DateTime dia_ini,int numd_dias){
            int i = 0, j = 0, k = 0;

            while (i < numd_dias)
            {
                dia_ini = dia_ini.AddDays(1);

                if (((dia_ini.DayOfWeek.ToString().Equals("Saturday") == true) || (dia_ini.DayOfWeek.ToString().Equals("Sunday") == true)))
                {
                    dia_ini = dia_ini.AddDays(1);
                }
                else
                {

                    while (j < tabla_dias_fest.Rows.Count)
                    {

                        if (dia_ini.ToString().Substring(0, 10).Equals(tabla_dias_fest.Rows[j][0].ToString().Substring(0, 10)))
                        {
                            //MessageBox.Show(dia_ini.ToString().Substring(0, 10) + "|" + tabla_dias_fest.Rows[j][0].ToString().Substring(0, 10) + "|" + j + "|" + i);
                            dia_ini = dia_ini.AddDays(1.0);
                            j = tabla_dias_fest.Rows.Count;
                            k = 1;
                        }
                        j++;
                    }

                    if (k == 0)
                    {
                        i++;
                    }
                    else
                    {
                        k = 0;
                    }

                    j = 0;
                }

                //MessageBox.Show(dia_ini.DayOfWeek.ToString());
            }
            return dia_ini;
        }
        
        public void guardar(){
        	String fecha_doc,fecha_act,fecha_firm,fecha_publi,fecha_ini_not,fecha_fin_not,fecha_ret_not,sql;
        	conex3.conectar("base_principal");
        	
        	fecha_doc=dateTimePicker1.Text;
        	fecha_act=dateTimePicker2.Text;
        	fecha_firm=dateTimePicker3.Text;
        	fecha_publi=dateTimePicker4.Text;
        	fecha_ini_not=dateTimePicker5.Text;
        	fecha_fin_not=dateTimePicker6.Text;
        	fecha_ret_not=dateTimePicker7.Text;
        	
        	fecha_doc=fecha_doc.Substring(6,4)+"-"+fecha_doc.Substring(3,2)+"-"+fecha_doc.Substring(0,2);
        	fecha_act=fecha_act.Substring(6,4)+"-"+fecha_act.Substring(3,2)+"-"+fecha_act.Substring(0,2);
        	fecha_firm=fecha_firm.Substring(6,4)+"-"+fecha_firm.Substring(3,2)+"-"+fecha_firm.Substring(0,2);
        	fecha_publi=fecha_publi.Substring(6,4)+"-"+fecha_publi.Substring(3,2)+"-"+fecha_publi.Substring(0,2);
        	fecha_ini_not=fecha_ini_not.Substring(6,4)+"-"+fecha_ini_not.Substring(3,2)+"-"+fecha_ini_not.Substring(0,2);
        	fecha_fin_not=fecha_fin_not.Substring(6,4)+"-"+fecha_fin_not.Substring(3,2)+"-"+fecha_fin_not.Substring(0,2);
        	fecha_ret_not=fecha_ret_not.Substring(6,4)+"-"+fecha_ret_not.Substring(3,2)+"-"+fecha_ret_not.Substring(0,2);
        	
        	//try{
            dataGridView1.DataSource = tabla_estrados;
            //MessageBox.Show("antes de guardar tabla estrados cuenta: " + tabla_estrados.Rows.Count);
        	if(tabla_estrados.Rows.Count>0){
               // MessageBox.Show("guarda como actualización");	
        		sql="UPDATE estrados SET nombre_documento=\""+comboBox3.Text+"\",fecha_emision_doc=\""+fecha_doc+"\",fecha_acta_circunstanciada=\""+fecha_act+"\",fojas="+maskedTextBox2.Text+",sub_emisora=\""+comboBox2.Text+"\",titular_sub=\""+textBox10.Text+"\",supuesto_estrados=\""+comboBox1.Text+"\",motivo_estrados=\""+textBox8.Text+"\","+
					 "fecha_firma_alta=\""+fecha_firm+"\",fecha_publicacion=\""+fecha_publi+"\",fecha_inicio_not=\""+fecha_ini_not+"\",fecha_fin_not=\""+fecha_fin_not+"\",fecha_retiro_not=\""+fecha_ret_not+"\",hora_not=\""+maskedTextBox3.Text+"\",observaciones=\""+textBox11.Text+"\" WHERE id_estrados="+tabla_estrados.Rows[0][0].ToString()+"";
        		//MessageBox.Show(sql);
        		conex3.consultar(sql);
        		conex3.consultar("UPDATE datos_factura SET fecha_notificacion=\""+fecha_ret_not+"\", status=\"NOTIFICADO\" WHERE id="+id_dat_fact+"");
        		conex3.guardar_evento("Se Modifica el Estrado con el id: "+tabla_estrados.Rows[0][0].ToString());
        		MessageBox.Show("Los Cambios Se Guardaron Exitosamente","¡Exito!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        	}else{
                //MessageBox.Show("guarda como nuevo");
        			//MessageBox.Show(""+id_dat_fact+"|"+tabla_estrados.Rows[0][0].ToString());
        		conex3.consultar("INSERT INTO estrados (folio,id_credito,nombre_documento,fecha_emision_doc,fecha_acta_circunstanciada,fojas,sub_emisora,titular_sub,supuesto_estrados,motivo_estrados,fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,hora_not,observaciones,notificador_estrados)" +
        		                 "VALUES(\""+maskedTextBox1.Text+"\","+id_dat_fact+",\""+comboBox3.Text+"\",\""+fecha_doc+"\",\""+fecha_act+"\","+maskedTextBox2.Text+",\""+comboBox2.Text+"\",\""+textBox10.Text+"\",\""+comboBox1.Text+"\",\""+textBox8.Text+"\",\""+fecha_firm+"\",\""+fecha_publi+"\",\""+fecha_ini_not+"\",\""+fecha_fin_not+"\",\""+fecha_ret_not+"\",\""+maskedTextBox3.Text+"\",\""+textBox11.Text+"\","+id_user+")");
        			
        		conex3.consultar("UPDATE datos_factura SET fecha_notificacion=\""+fecha_ret_not+"\", status=\"NOTIFICADO\" WHERE id="+id_dat_fact+"");
        		
        		conex3.guardar_evento("Se Ingresa el Estrado con el FOLIO: "+maskedTextBox1.Text);
        		MessageBox.Show("Se Ingresó Exitosamente el Estrado con el FOLIO: "+maskedTextBox1.Text,"Ingreso Exitoso",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        	}
        		comboBox1.SelectedIndex=-1;
        		comboBox1.Text="";
        		comboBox2.SelectedIndex=-1;
        		comboBox2.Text="";
        		comboBox3.SelectedIndex=-1;
        		comboBox3.Text="";
        		dateTimePicker1.Value=DateTime.Today;
        		dateTimePicker2.Value=DateTime.Today;
        		dateTimePicker3.Value=DateTime.Today;
        		dateTimePicker4.Value=DateTime.Today;
        		dateTimePicker5.Value=DateTime.Today;
        		dateTimePicker6.Value=DateTime.Today;
        		dateTimePicker7.Value=DateTime.Today;
        		maskedTextBox1.Clear();
        		maskedTextBox2.Clear();
        		maskedTextBox3.Clear();
        		textBox1.Text="";
        		textBox2.Text="";
        		textBox3.Text="";
        		textBox4.Text="";
        		textBox5.Text="";
        		textBox6.Text="";
        		textBox8.Text="";
        		textBox10.Text="";
        		textBox11.Text="";
        		textBox12.Text="";
        		textBox13.Text="";
        		//groupBox2.Enabled=false;
        		//groupBox3.Enabled=false;
        		desactivar_controles();
        		maskedTextBox11.Focus();
        		maskedTextBox1.ReadOnly=false;
        		button4.Enabled=true;
        		
        	//}catch(Exception es){
        	//	MessageBox.Show("Hubo un Error al Intentar Guardar los Datos. \n\n"+es,"Error");
        	//}
        	
        }
        
        public void guardar_ofi(){
        	String fecha_doc,fecha_act,fecha_firm,fecha_publi,fecha_ini_not,fecha_fin_not,fecha_ret_not,sql;
        	conex3.conectar("base_principal");
        	
        	fecha_doc=dateTimePicker1.Text;
        	fecha_act=dateTimePicker2.Text;
        	fecha_firm=dateTimePicker3.Text;
        	fecha_publi=dateTimePicker4.Text;
        	fecha_ini_not=dateTimePicker5.Text;
        	fecha_fin_not=dateTimePicker6.Text;
        	fecha_ret_not=dateTimePicker7.Text;
        	
        	fecha_doc=fecha_doc.Substring(6,4)+"-"+fecha_doc.Substring(3,2)+"-"+fecha_doc.Substring(0,2);
        	fecha_act=fecha_act.Substring(6,4)+"-"+fecha_act.Substring(3,2)+"-"+fecha_act.Substring(0,2);
        	fecha_firm=fecha_firm.Substring(6,4)+"-"+fecha_firm.Substring(3,2)+"-"+fecha_firm.Substring(0,2);
        	fecha_publi=fecha_publi.Substring(6,4)+"-"+fecha_publi.Substring(3,2)+"-"+fecha_publi.Substring(0,2);
        	fecha_ini_not=fecha_ini_not.Substring(6,4)+"-"+fecha_ini_not.Substring(3,2)+"-"+fecha_ini_not.Substring(0,2);
        	fecha_fin_not=fecha_fin_not.Substring(6,4)+"-"+fecha_fin_not.Substring(3,2)+"-"+fecha_fin_not.Substring(0,2);
        	fecha_ret_not=fecha_ret_not.Substring(6,4)+"-"+fecha_ret_not.Substring(3,2)+"-"+fecha_ret_not.Substring(0,2);
        	
        	try{
        	if(tabla_estrados.Rows.Count>0){
        			
        		sql="UPDATE estrados SET nombre_documento=\""+comboBox3.Text+"\",fecha_emision_doc=\""+fecha_doc+"\",fecha_acta_circunstanciada=\""+fecha_act+"\",fojas="+maskedTextBox2.Text+",sub_emisora=\""+comboBox2.Text+"\",titular_sub=\""+textBox10.Text+"\",supuesto_estrados=\""+comboBox1.Text+"\",motivo_estrados=\""+textBox8.Text+"\","+
					                               		"fecha_firma_alta=\""+fecha_firm+"\",fecha_publicacion=\""+fecha_publi+"\",fecha_inicio_not=\""+fecha_ini_not+"\",fecha_fin_not=\""+fecha_fin_not+"\",fecha_retiro_not=\""+fecha_ret_not+"\",hora_not=\""+maskedTextBox3.Text+"\",observaciones=\""+textBox11.Text+"\" WHERE id_estrados="+tabla_estrados.Rows[0][0].ToString()+"";
        		//MessageBox.Show(sql);
        		conex3.consultar(sql);
        		conex3.consultar("UPDATE oficios SET fecha_notificacion=\""+fecha_ret_not+"\", estatus=\"NOTIFICADO\", observaciones=\"NOTIFICADO POR ESTRADOS\",nn=\"-\" WHERE id_oficios="+id_oficio+" ");
        		conex3.guardar_evento("Se Modifica el Estrado con el id: "+tabla_estrados.Rows[0][0].ToString());
        		MessageBox.Show("Los Cambios Se Guardaron Exitosamente","¡Exito!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        	}else{
        		conex3.consultar("INSERT INTO estrados (folio,id_credito,nombre_documento,fecha_emision_doc,fecha_acta_circunstanciada,fojas,sub_emisora,titular_sub,supuesto_estrados,motivo_estrados,fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,hora_not,observaciones,notificador_estrados)" +
        		                 "VALUES(\""+maskedTextBox1.Text+"\",\"OFI_"+id_oficio+"\",\""+comboBox3.Text+"\",\""+fecha_doc+"\",\""+fecha_act+"\","+maskedTextBox2.Text+",\""+comboBox2.Text+"\",\""+textBox10.Text+"\",\""+comboBox1.Text+"\",\""+textBox8.Text+"\",\""+fecha_firm+"\",\""+fecha_publi+"\",\""+fecha_ini_not+"\",\""+fecha_fin_not+"\",\""+fecha_ret_not+"\",\""+maskedTextBox3.Text+"\",\""+textBox11.Text+"\","+id_user+")");
        			
        		conex3.consultar("UPDATE oficios SET fecha_notificacion=\""+fecha_ret_not+"\", estatus=\"NOTIFICADO\", observaciones=\"NOTIFICADO POR ESTRADOS\",nn=\"-\" WHERE id_oficios="+id_oficio+" ");
        		conex3.guardar_evento("Se Ingresa el Estrado con el FOLIO: "+maskedTextBox1.Text);
        		MessageBox.Show("Se Ingresó Exitosamente el Estrado con el FOLIO: "+maskedTextBox1.Text,"Ingreso Exitoso",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        	}     		
        		comboBox1.SelectedIndex=-1;
        		comboBox1.Text="";
        		comboBox2.SelectedIndex=-1;
        		comboBox2.Text="";
        		comboBox3.SelectedIndex=-1;
        		comboBox3.Text="";
        		dateTimePicker1.Value=DateTime.Today;
        		dateTimePicker2.Value=DateTime.Today;
        		dateTimePicker3.Value=DateTime.Today;
        		dateTimePicker4.Value=DateTime.Today;
        		dateTimePicker5.Value=DateTime.Today;
        		dateTimePicker6.Value=DateTime.Today;
        		dateTimePicker7.Value=DateTime.Today;
        		maskedTextBox1.Clear();
        		maskedTextBox2.Clear();
        		maskedTextBox3.Clear();
        		textBox1.Text="";
        		textBox2.Text="";
        		textBox3.Text="";
        		textBox4.Text="";
        		textBox5.Text="";
        		textBox6.Text="";
        		textBox8.Text="";
        		textBox10.Text="";
        		textBox11.Text="";
        		textBox12.Text="";
        		textBox13.Text="";
        		//groupBox2.Enabled=false;
        		//groupBox3.Enabled=false;
        		desactivar_controles();
        		maskedTextBox11.Focus();
        		maskedTextBox1.ReadOnly=false;
        		button4.Enabled=true;
        	}catch(Exception es){
        		MessageBox.Show("Hubo un Error al Intentar Guardar los Datos. \n\n"+es,"Error");
        	}
        	
        }
        
        public void carga_oficio(){
        	conex4.conectar("base_principal");
        	conex2.conectar("base_principal");
        	
        	comboBox1.SelectedIndex=-1;
        	comboBox1.Text="";
        	comboBox2.SelectedIndex=-1;
        	comboBox2.Text="";
        	comboBox3.SelectedIndex=-1;
        	comboBox3.Text="";
        	dateTimePicker1.Value=DateTime.Today;
        	dateTimePicker2.Value=DateTime.Today;
        	dateTimePicker3.Value=DateTime.Today;
        	dateTimePicker4.Value=DateTime.Today;
        	dateTimePicker5.Value=DateTime.Today;
        	dateTimePicker6.Value=DateTime.Today;
        	dateTimePicker7.Value=DateTime.Today;
        	maskedTextBox1.Clear();
        	maskedTextBox2.Clear();
        	maskedTextBox3.Clear();
        	maskedTextBox3.Text="12:00";
        	textBox1.Text="";
        	textBox2.Text="";
        	textBox3.Text="";
        	textBox4.Text="";
        	textBox5.Text="";
        	textBox6.Text="";
        	textBox8.Text="";
        	textBox10.Text="";
        	textBox11.Text="";
        	textBox12.Text="";
        	textBox13.Text="";
        	//groupBox2.Enabled=false;
        	//groupBox3.Enabled=false;
        	desactivar_controles();
        	
        	oficios=conex4.consultar("SELECT reg_nss,folio,acuerdo,fecha_oficio,razon_social,receptor,domicilio_oficio,localidad_oficio,cp_oficio,rfc_oficio,nombre_oficio FROM oficios WHERE id_oficios="+id_oficio+"");
        	if(oficios.Rows.Count>0){
        		label7.Text="Reg. Patronal/NSS:";
        		label5.Text="Folio:";
        		label6.Text="Acuerdo:";
        		label8.Text="Fec. Of:";
        		textBox1.Text=oficios.Rows[0][0].ToString();
        		textBox2.Text=oficios.Rows[0][1].ToString();
        		textBox3.Text=oficios.Rows[0][2].ToString();
        		textBox4.Text=oficios.Rows[0][3].ToString();
        		textBox12.Text=oficios.Rows[0][4].ToString();
        		textBox13.Text=oficios.Rows[0][5].ToString();
        		textBox5.Text=oficios.Rows[0][6].ToString()+" "+oficios.Rows[0][7].ToString()+" "+oficios.Rows[0][8].ToString();
        		textBox6.Text=oficios.Rows[0][9].ToString();
        		comboBox3.Text=oficios.Rows[0][10].ToString();
        		
        		tabla_estrados=conex2.consultar("SELECT id_estrados,nombre_documento,fecha_emision_doc,fecha_acta_circunstanciada,fojas,sub_emisora,titular_sub,folio,supuesto_estrados,motivo_estrados,"+
        		                                "fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,hora_not,observaciones,paquete FROM estrados WHERE id_credito=\"OFI_"+id_oficio+"\"");
        		
        		if(tabla_estrados.Rows.Count>0){
        			//textBox7.Text=tabla_estrados.Rows[0][1].ToString();
        			comboBox3.Text=tabla_estrados.Rows[0][1].ToString();
        			dateTimePicker1.Value=Convert.ToDateTime(tabla_estrados.Rows[0][2].ToString());
        			dateTimePicker2.Value=Convert.ToDateTime(tabla_estrados.Rows[0][3].ToString());
        			maskedTextBox2.Text=tabla_estrados.Rows[0][4].ToString();
        			//textBox5.Text=tabla_estrados.Rows[0][5].ToString();
        			comboBox2.Text=tabla_estrados.Rows[0][5].ToString();
        			textBox10.Text=tabla_estrados.Rows[0][6].ToString();
        			
        			maskedTextBox1.Text=tabla_estrados.Rows[0][7].ToString();
        			comboBox1.Text=tabla_estrados.Rows[0][8].ToString();
        			textBox8.Text=tabla_estrados.Rows[0][9].ToString();
        			dateTimePicker3.Value=Convert.ToDateTime(tabla_estrados.Rows[0][10].ToString());
        			dateTimePicker4.Value=Convert.ToDateTime(tabla_estrados.Rows[0][11].ToString());
        			dateTimePicker5.Value=Convert.ToDateTime(tabla_estrados.Rows[0][12].ToString());
        			dateTimePicker6.Value=Convert.ToDateTime(tabla_estrados.Rows[0][13].ToString());
        			dateTimePicker7.Value=Convert.ToDateTime(tabla_estrados.Rows[0][14].ToString());
        			maskedTextBox3.Text=tabla_estrados.Rows[0][15].ToString();
        			textBox11.Text=tabla_estrados.Rows[0][16].ToString();
                    textBox14.Text = tabla_estrados.Rows[0][17].ToString();
        			if(tipo_inicio==1){
								button3.Enabled=true;
								maskedTextBox1.ReadOnly=true;
							}
        		}else{
        			if(tipo_inicio==2){
        			}else{
        				llena_auto();
        			}
        			if(tipo_inicio==1){
								button3.Enabled=false;
							}
        		}
        		
        		if(puesto.Equals("Auxiliar Estrados")==true||puesto.Equals("Big Boss")==true){
        			//groupBox2.Enabled=true;
        			//groupBox3.Enabled=true;
        			activar_controles();
        			textBox11.Enabled=true;
        			button1.Enabled=true;
        		}
        	}else{
        		//groupBox2.Enabled=false;
        		//groupBox3.Enabled=false;
        		desactivar_controles();
        		textBox11.Enabled=false;
        		button1.Enabled=false;
        	}
        }
        
        public void activar_controles(){
        	
        	comboBox1.Enabled=true;
        	comboBox2.Enabled=true;
        	comboBox3.Enabled=true;   
			dateTimePicker1.Enabled=true;
			dateTimePicker2.Enabled=true;
			dateTimePicker3.Enabled=true;
			dateTimePicker4.Enabled=true;
			dateTimePicker5.Enabled=true;
			dateTimePicker6.Enabled=true;
			dateTimePicker7.Enabled=true;
        	maskedTextBox1.ReadOnly=false;
        	maskedTextBox2.ReadOnly=false;
        	maskedTextBox3.ReadOnly=false;
        	//textBox1.ReadOnly=false;
        	//textBox2.ReadOnly=false;
        	//textBox3.ReadOnly=false;
        	//textBox4.ReadOnly=false;
        	//textBox5.ReadOnly=false;
        	//textBox6.ReadOnly=false;
        	textBox8.ReadOnly=false;
        	textBox10.ReadOnly=false;
        	textBox11.ReadOnly=false;
        	//textBox12.ReadOnly=false;
        	//textBox13.ReadOnly=false;
        	maskedTextBox11.Focus();
        	button4.Enabled=false;
        }
        
        public void desactivar_controles(){
        	
        	comboBox1.Enabled=false;
        	comboBox2.Enabled=false;
        	comboBox3.Enabled=false;   
			dateTimePicker1.Enabled=false;
			dateTimePicker2.Enabled=false;
			dateTimePicker3.Enabled=false;
			dateTimePicker4.Enabled=false;
			dateTimePicker5.Enabled=false;
			dateTimePicker6.Enabled=false;
			dateTimePicker7.Enabled=false;
        	maskedTextBox1.ReadOnly=true;
        	maskedTextBox2.ReadOnly=true;
        	maskedTextBox3.ReadOnly=true;
        	textBox1.ReadOnly=true;
        	textBox2.ReadOnly=true;
        	textBox3.ReadOnly=true;
        	textBox4.ReadOnly=true;
        	textBox5.ReadOnly=true;
        	textBox6.ReadOnly=true;
        	textBox8.ReadOnly=true;
        	textBox10.ReadOnly=true;
        	textBox11.ReadOnly=true;
        	textBox12.ReadOnly=true;
        	textBox13.ReadOnly=true;
        	button4.Enabled=false;
        	
        }
        
		void Captura_info_estradosLoad(object sender, EventArgs e)
		{

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			String hor_box,min_box,no_del_sub;
			hor_box=""+DateTime.Now.Hour;
			min_box=""+DateTime.Now.Minute;
			string[] datos;
			
			desactivar_controles();
			maskedTextBox1.ReadOnly=false;
			button4.Enabled=true;
			if(hor_box.Length==1){
				hor_box="0"+hor_box;
			}
			
			if(min_box.Length==1){
				min_box="0"+min_box;
			}
			
			maskedTextBox3.Text=hor_box+min_box;
			
			conex0.conectar("base_principal");
			tabla_dias_fest = conex0.consultar("SELECT dia FROM dias_festivos");
			
			puesto = MainForm.datos_user_static[5];
			rango = MainForm.datos_user_static[2];
			id_us = MainForm.datos_user_static[7];
			no_del_sub=conex0.leer_config_sub()[1]+conex0.leer_config_sub()[4];
			maskedTextBox1.Text=no_del_sub+DateTime.Today.Year.ToString().Substring(2,2);
			leer_config();
			puesto = MainForm.datos_user_static[5];
			if(comboBox3.SelectedIndex>-1){
				toolTip1.SetToolTip(comboBox3,comboBox3.SelectedItem.ToString());
			}
			
			if(tipo_inicio==4){
				datos = new string[3];
				datos = datos_consu.Split(',');
				maskedTextBox11.Text=datos[0].ToString();
				maskedTextBox12.Text=datos[1].ToString();
				maskedTextBox13.Text=datos[2].ToString();
				tipo_inicio=2;
				consulta();
			}
			
			if(puesto.Equals("Auxiliar Estrados")==true||puesto.Equals("Big Boss")==true){
				id_user=Convert.ToInt32(id_us);
				if(tipo_inicio==3){
					carga_oficio();
				}
				
			}else{
				if(tipo_inicio==3){
					carga_oficio();
				}else{
					tipo_inicio=2;
				}
			}			
			
			timer1.Enabled=true;
            //conex6.conectar("base_principal");	
            
            
		}
		//boton consulta
		void Button2Click(object sender, EventArgs e)
		{
			consulta();
			//maskedTextBox11.Clear();
			//maskedTextBox12.Clear();
			//maskedTextBox13.Clear();
		}
		
		void MaskedTextBox2MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}
		
		void MaskedTextBox2Leave(object sender, EventArgs e)
		{	
			try{
				if(maskedTextBox2.Text.Substring(0,1).Equals(" ")==true){
					maskedTextBox2.Text=maskedTextBox2.Text.Remove(0,1);
				}
				
				if(maskedTextBox2.Text.Substring(1,1).Equals(" ")==true){
					maskedTextBox2.Text=maskedTextBox2.Text.Remove(1,1);
				}
				
				if(maskedTextBox2.Text.Substring(2,1).Equals(" ")==true){
					maskedTextBox2.Text=maskedTextBox2.Text.Remove(2,1);
				}
			
			}catch{}
			//maskedTextBox2.Text=maskedTextBox2.Text.Trim(' ');
			if(maskedTextBox2.Text.Length>0){
				if(Convert.ToInt32(maskedTextBox2.Text)<100 && Convert.ToInt32(maskedTextBox2.Text)>10){
					maskedTextBox2.Text="0"+maskedTextBox2.Text;
				}else{
					if(Convert.ToInt32(maskedTextBox2.Text)<10){
						maskedTextBox2.Text="00"+maskedTextBox2.Text;
					}
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//cuenta_dias_hab(dateTimePicker3.Value,5);
			int candado=0;
			String error="";
			DateTime hora;
			
			if(comboBox3.Text.Length<6){
				candado++;
				error=" •Nombre Documento\n";
			}
			try{
				if((Convert.ToInt32(maskedTextBox2.Text))<1){
					candado++;
					error+=" •Fojas\n";
				}
			}catch{
				candado++;
				error+=" •Fojas\n";
			}
			if(comboBox2.Text.Length<6){
				candado++;
				error+=" •Subdelegacion Emisora\n";
			}
			if(textBox10.Text.Length<6){
				candado++;
				error+=" •Titular Subdelegacion Emisora\n";
			}
			if(comboBox1.Text.Length<6){
				candado++;
				error+=" •Supuesto Estrados\n";
			}
			if(textBox8.Text.Length<20){
				candado++;
				error+=" •Motivo Estrados\n";
			}
			if((Convert.ToInt32(maskedTextBox3.Text.Substring(0,2))>23)||(Convert.ToInt32(maskedTextBox3.Text.Substring(3,2))>59)){
				candado++;
				error+=" •Hora Notificación\n";
			}
			
			if(candado==0){
				DialogResult resu=MessageBox.Show("Revise con cuidado las Fechas y la Hora de Notificación.\nEstá por Guardar Información en la Base de Datos.\n\n¿Desea Continuar?","Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				if(resu == DialogResult.Yes){
					if(puesto.Equals("Auxiliar Estrados")==true||puesto.Equals("Big Boss")==true){

                        if (tabla_estrados.Rows.Count > 0)
                        {
                            
                        }
                        else
                        {
                            colocar_folio();
                        }
                        						
						if(tipo_inicio==3){
							guardar_ofi();
						}else{
							guardar();
						}
					}
				}
			}else{
				if(candado==1){
					MessageBox.Show("No se puede continuar por que el siguiente campo está vacío o contiene un dato inválido:\n"+error,"Aviso");
				}else{
					MessageBox.Show("No se puede continuar por que los siguientes campos están vacíos o contienen un dato inválido:\n"+error,"Aviso");
				}
			}
		}
		
		void DateTimePicker3Leave(object sender, EventArgs e)
		{
			dateTimePicker4.Value=cuenta_dias_hab(dateTimePicker3.Value,1);
			dateTimePicker5.Value=cuenta_dias_hab(dateTimePicker3.Value,2);
			dateTimePicker6.Value=cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(datos_sub[11])+1));
            dateTimePicker7.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(datos_sub[11]) + 2));
		}
		
		void DateTimePicker3ValueChanged(object sender, EventArgs e)
		{
			dateTimePicker4.Value=cuenta_dias_hab(dateTimePicker3.Value,1);
			dateTimePicker5.Value=cuenta_dias_hab(dateTimePicker3.Value,2);
            dateTimePicker6.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(datos_sub[11]) + 1));
            dateTimePicker7.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(datos_sub[11]) + 2));
		}
		
		void MaskedTextBox11TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox11.MaskCompleted==true){
				maskedTextBox12.Focus();
			}
		}
		
		void MaskedTextBox12TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox12.MaskCompleted==true){
				maskedTextBox13.Focus();
			}
		}
		
		void MaskedTextBox13TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox13.MaskCompleted==true){
				button2.Focus();
			}
		}
		
		void MaskedTextBox11KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar== Convert.ToChar(Keys.Enter)){
				maskedTextBox12.Focus();
			}
		}
		
		void MaskedTextBox12KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar== Convert.ToChar(Keys.Enter)){
				maskedTextBox13.Focus();
			}
		}
		
		void MaskedTextBox13KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar== Convert.ToChar(Keys.Enter)){
				button2.Focus();
			}
		}
		
		void Button2Enter(object sender, EventArgs e)
		{
			button2.BackColor=Color.Navy; 			
		}
		
		void Button2Leave(object sender, EventArgs e)
		{
			button2.BackColor=Color.Transparent;
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar== Convert.ToChar(Keys.Enter)){
				comboBox2.Focus();
			}
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(tipo_inicio==1){
				String hor_box,min_box;
				hor_box=""+DateTime.Now.Hour;
				min_box=""+DateTime.Now.Minute;
				
				if(hor_box.Length==1){
					hor_box="0"+hor_box;
				}
				
				if(min_box.Length==1){
					min_box="0"+min_box;
				}
				
				maskedTextBox3.Text=hor_box+min_box;
			}
		}
		
		void DateTimePicker6ValueChanged(object sender, EventArgs e)
		{
			//dateTimePicker7.Value=cuenta_dias_hab(dateTimePicker6.Value,1);
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedIndex==0){
				textBox10.Text=subdele;
			}else{
				textBox10.Text="";
			}
		}
		//borrar
		void Button3Click(object sender, EventArgs e)
		{
			DialogResult resu = MessageBox.Show("Se Borrará la Información de este Estrado.\nEsta Acción no se puede deshacer.\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if(resu == DialogResult.Yes){
				conex5.conectar("base_principal");
				conex5.consultar("DELETE FROM estrados WHERE folio=\""+maskedTextBox1.Text+"\"");
				
				comboBox1.SelectedIndex=-1;
				comboBox1.Text="";
				comboBox2.SelectedIndex=-1;
				comboBox2.Text="";
				comboBox3.SelectedIndex=-1;
				comboBox3.Text="";
				dateTimePicker1.Value=DateTime.Today;
				dateTimePicker2.Value=DateTime.Today;
				dateTimePicker3.Value=DateTime.Today;
				dateTimePicker4.Value=DateTime.Today;
				dateTimePicker5.Value=DateTime.Today;
				dateTimePicker6.Value=DateTime.Today;
				dateTimePicker7.Value=DateTime.Today;
				maskedTextBox1.Clear();
				maskedTextBox2.Clear();
				maskedTextBox3.Clear();
				maskedTextBox3.Text="12:00";
				textBox1.Text="";
				textBox2.Text="";
				textBox3.Text="";
				textBox4.Text="";
				textBox5.Text="";
				textBox6.Text="";
				textBox8.Text="";
				textBox10.Text="";
				textBox11.Text="";
				textBox12.Text="";
				textBox13.Text="";
				//groupBox2.Enabled=false;
				//groupBox3.Enabled=false;
				desactivar_controles();
				maskedTextBox1.ReadOnly=false;
				button4.Enabled=true;
				MessageBox.Show("Registro Borrado Correctamente","AVISO");
			}
		}
		//buscar por folio
		void Button4Click(object sender, EventArgs e)
		{
			consulta_folio();
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				consulta_folio();
			}
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.Text.Length==14){
				button4.Focus();
			}
		}
		
		void MaskedTextBox1Enter(object sender, EventArgs e)
		{
			maskedTextBox1.SelectionStart=9;
		}

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
	}
}

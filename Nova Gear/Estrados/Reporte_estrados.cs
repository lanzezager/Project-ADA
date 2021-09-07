/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 09/10/2017
 * Hora: 03:37 p.m.
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
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Estrados
{
	/// <summary>
	/// Description of Reporte_estrados.
	/// </summary>
	public partial class Reporte_estrados : Form
	{
		public Reporte_estrados()
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
        Conexion conex4 = new Conexion();
		
        DataTable tabla_datos_fact = new DataTable();
        DataTable tabla_sindo = new DataTable();
        DataTable tabla_estrados = new DataTable();
        DataTable tabla_reporte = new DataTable();
        DataTable tabla_usuarios = new DataTable();
        DataTable tabla_datos_fact1 = new DataTable();
        DataTable packs = new DataTable();
        DataTable regs_sin_dom = new DataTable();

        int num = 0, ver_regs_sin_dom = 0, test_num = 0;
        String notis_estrados,noti_estrados,folio,test_no="";
        
        public string interprete_fechas(String fecha){
			String fecha_inter;
			
			fecha_inter=fecha.Substring(0,2);
			//MessageBox.Show(fecha.Substring(3,2));
			switch(fecha.Substring(3,2)){
					case "01": fecha_inter= fecha_inter+" de Enero de "+fecha.Substring(6,4);
					break;
					
					case "02": fecha_inter= fecha_inter+" de Febrero de "+fecha.Substring(6,4);
					break;
					
					case "03": fecha_inter= fecha_inter+" de Marzo de "+fecha.Substring(6,4);
					break;
					
					case "04": fecha_inter= fecha_inter+" de Abril de "+fecha.Substring(6,4);
					break;
					
					case "05": fecha_inter= fecha_inter+" de Mayo de "+fecha.Substring(6,4);
					break;
					
					case "06": fecha_inter= fecha_inter+" de Junio de "+fecha.Substring(6,4);
					break;
					
					case "07": fecha_inter= fecha_inter+" de Julio de "+fecha.Substring(6,4);
					break;
					
					case "08": fecha_inter= fecha_inter+" de Agosto de "+fecha.Substring(6,4);
					break;
					
					case "09": fecha_inter= fecha_inter+" de Septiembre de "+fecha.Substring(6,4);
					break;
					
					case "10": fecha_inter= fecha_inter+" de Octubre de "+fecha.Substring(6,4);
					break;
					
					case "11": fecha_inter= fecha_inter+" de Noviembre de "+fecha.Substring(6,4);
					break;
					
					case "12": fecha_inter= fecha_inter+" de Diciembre de "+fecha.Substring(6,4);
					break;
			}
			return fecha_inter;
		}
        
        public string interprete_numero(String nume){
        	String num_letra="";
        	int num_num;
        	num_num=Convert.ToInt32(nume);
        	
        	if(num_num>15 &&num_num<20){
        		num_letra="DIECI";
        		switch(num_num){
        				case 16: num_letra+="SEIS"; break;
        				case 17: num_letra+="SIETE"; break;
        				case 18: num_letra+="OCHO"; break;
        				case 19: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>20 &&num_num<30){
        		num_letra="VEINTI";
        		switch(num_num){
        				case 21: num_letra+="ÚN"; break;
        				case 22: num_letra+="DOS"; break;
        				case 23: num_letra+="TRES"; break;
        				case 24: num_letra+="CUATRO"; break;
        				case 25: num_letra+="CINCO"; break;
        				case 26: num_letra+="SEIS"; break;
        				case 27: num_letra+="SIETE"; break;
        				case 28: num_letra+="OCHO"; break;
        				case 29: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>30 &&num_num<40){
        		num_letra="TREINTA Y ";
        		switch(num_num){
        				case 31: num_letra+="ÚN"; break;
        				case 32: num_letra+="DOS"; break;
        				case 33: num_letra+="TRES"; break;
        				case 34: num_letra+="CUATRO"; break;
        				case 35: num_letra+="CINCO"; break;
        				case 36: num_letra+="SEIS"; break;
        				case 37: num_letra+="SIETE"; break;
        				case 38: num_letra+="OCHO"; break;
        				case 39: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>40 &&num_num<50){
        		num_letra="CUARENTA Y ";
        		switch(num_num){
        				case 41: num_letra+="ÚN"; break;
        				case 42: num_letra+="DOS"; break;
        				case 43: num_letra+="TRES"; break;
        				case 44: num_letra+="CUATRO"; break;
        				case 45: num_letra+="CINCO"; break;
        				case 46: num_letra+="SEIS"; break;
        				case 47: num_letra+="SIETE"; break;
        				case 48: num_letra+="OCHO"; break;
        				case 49: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>50 &&num_num<60){
        		num_letra="CINCUENTA Y ";
        		switch(num_num){
        				case 51: num_letra+="ÚN"; break;
        				case 52: num_letra+="DOS"; break;
        				case 53: num_letra+="TRES"; break;
        				case 54: num_letra+="CUATRO"; break;
        				case 55: num_letra+="CINCO"; break;
        				case 56: num_letra+="SEIS"; break;
        				case 57: num_letra+="SIETE"; break;
        				case 58: num_letra+="OCHO"; break;
        				case 59: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	switch(num_num){
        		case 0: num_letra="CERO"; break;
        		case 1: num_letra="ÚN"; break;
        		case 2: num_letra="DOS"; break;
        		case 3: num_letra="TRES"; break;
        		case 4: num_letra="CUATRO"; break;
        		case 5: num_letra="CINCO"; break;
        		case 6: num_letra="SEIS"; break;
        		case 7: num_letra="SIETE"; break;
        		case 8: num_letra="OCHO"; break;
        		case 9: num_letra="NUEVE"; break;
        		case 10: num_letra="DIEZ"; break;
        		case 11: num_letra="ONCE"; break;
        		case 12: num_letra="DOCE"; break;
        		case 13: num_letra="TRECE"; break;
        		case 14: num_letra="CATORCE"; break;
        		case 15: num_letra="QUINCE"; break;
        		case 20: num_letra="VEINTE"; break;
        		case 30: num_letra="TREINTA"; break;
        		case 40: num_letra="CUARENTA"; break;
        		case 50: num_letra="CINCUENTA"; break;
        	}
        	
        	return num_letra;
        }
        
        public void buscar_folio(){
        	
        	int j=0,k=0,listar=0,l=0,num_verif=0;
        	
        	
        	//tabla_estrados.Columns.Clear();
        	tabla_estrados=conex.consultar("SELECT folio,id_credito,fecha_emision_doc,nombre_documento,supuesto_estrados,motivo_estrados,fecha_acta_circunstanciada,fojas,fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,notificador_estrados,titular_sub,sub_emisora,hora_not FROM estrados WHERE folio=\""+folio+"\"");
        	
        	if(tabla_estrados.Rows.Count>0){
        		if(tabla_estrados.Rows[0][1].ToString().StartsWith("OFI_")==false){
        			/// <summary>
        			
        			/// //SI ES CREDITO NORMAL
        			/// </summary>
                    /// 

                    if (Int32.TryParse(tabla_estrados.Rows[0][1].ToString(), out num_verif) == true)
                    {
                        tabla_datos_fact = conex1.consultar("SELECT razon_social,registro_patronal1,credito_cuotas,credito_multa,notificador FROM datos_factura WHERE id=" + tabla_estrados.Rows[0][1].ToString() + "");
                    }
                    else
                    {
                        tabla_datos_fact.Rows.Clear();
                    }

        			//tabla_datos_fact=conex1.consultar("SELECT razon_social,registro_patronal1,credito_cuotas,credito_multa,notificador FROM datos_factura WHERE id="+tabla_estrados.Rows[0][1].ToString()+"");
        			//MessageBox.Show(""+tabla_usuarios.Rows.Count);
        			while(j<tabla_usuarios.Rows.Count){
        				if(tabla_usuarios.Rows[j][0].ToString().Equals(tabla_estrados.Rows[0][13].ToString())==true){
        					noti_estrados=tabla_usuarios.Rows[j][1].ToString()+" "+tabla_usuarios.Rows[j][2].ToString();
        				}
        				j++;
        			}
        			
        			if(tabla_datos_fact.Rows.Count>0){
        				tabla_sindo=conex2.consultar("SELECT id_sindo,domicilio,localidad,cp,rfc FROM sindo WHERE registro_patronal= \""+tabla_datos_fact.Rows[0][1].ToString()+"\"");

        				if(tabla_reporte.Rows.Count>0){
        					//tabla_reporte.Rows.Clear();
        				}else{
        					if(tabla_reporte.Columns.Count>0){
        						
        					}else{
        						tabla_reporte.Columns.Add("NUM");
        						tabla_reporte.Columns.Add("FOLIO");
        						tabla_reporte.Columns.Add("RAZON SOCIAL");
        						tabla_reporte.Columns.Add("REGISTRO PATRONAL");
        						tabla_reporte.Columns.Add("R.F.C.");
        						tabla_reporte.Columns.Add("DOMICILIO");
        						tabla_reporte.Columns.Add("TIPO DE DOCUMENTO");
        						tabla_reporte.Columns.Add("CREDITOS");
        						tabla_reporte.Columns.Add("FECHA_DOC");
        						tabla_reporte.Columns.Add("SUB");
        						tabla_reporte.Columns.Add("TITULAR_SUB");
        						tabla_reporte.Columns.Add("NOTIFICADOR");
        						tabla_reporte.Columns.Add("FECHA_ACTA");
        						tabla_reporte.Columns.Add("FOJAS");
        						tabla_reporte.Columns.Add("MOTIVO_ESTRADOS");
        						tabla_reporte.Columns.Add("SUPUESTO_ESTRADOS");
        						tabla_reporte.Columns.Add("FECHA_INI_NOT");
        						tabla_reporte.Columns.Add("FECHA_FIN_NOT");
        						tabla_reporte.Columns.Add("FECHA_RETIRO_NOT");
        						tabla_reporte.Columns.Add("NOTIFICADOR_ESTRADOS");
        						tabla_reporte.Columns.Add("NOTIFICADORES_ESTRADOS");
        						tabla_reporte.Columns.Add("HORA_NOT_HORAS");
        						tabla_reporte.Columns.Add("HORA_NOT_MINS");
        						tabla_reporte.Columns.Add("FECHA_FIRMA_ALTA");
        						tabla_reporte.Columns.Add("FECHA_PUBLICACION");
        					}
        				}
        				
        				while(l<tabla_reporte.Rows.Count){
        					if(tabla_reporte.Rows[l][1].ToString().Equals(tabla_estrados.Rows[0][0].ToString())==true){
        						listar=1;
        					}
        					l++;
        				}
        				
        				
        				if(listar==0){

                            if(tabla_sindo.Rows.Count<1){
                                if (comboBox1.SelectedIndex == 2)
                                {
                                    if (tabla_datos_fact.Rows[0][0].ToString().Length == 10)
                                    {
                                        regs_sin_dom.Rows.Add("0"+tabla_datos_fact.Rows[0][1].ToString(), tabla_datos_fact.Rows[0][0].ToString());
                                    }
                                    else
                                    {
                                        regs_sin_dom.Rows.Add(tabla_datos_fact.Rows[0][1].ToString(), tabla_datos_fact.Rows[0][0].ToString());
                                    }
                                    
                                }
                                else
                                {
                                    MessageBox.Show("El registro buscado no cuenta con información de domicilio en la Base de Datos", "AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                                }
                            }else{
        					    tabla_reporte.Rows.Add();
        					    tabla_reporte.Rows[num][0]=num+1;//num
        					    tabla_reporte.Rows[num][1]=tabla_estrados.Rows[0][0].ToString();//folio
        					    tabla_reporte.Rows[num][2]=tabla_datos_fact.Rows[0][0].ToString();//razon_social
        					    tabla_reporte.Rows[num][3]=tabla_datos_fact.Rows[0][1].ToString();//reg_pat
        					    tabla_reporte.Rows[num][4]=tabla_sindo.Rows[0][4].ToString();//rfc
        					    tabla_reporte.Rows[num][5]=tabla_sindo.Rows[0][1].ToString()+" "+tabla_sindo.Rows[0][2].ToString()+" "+tabla_sindo.Rows[0][3].ToString();//domicilio
        					    tabla_reporte.Rows[num][6]=tabla_estrados.Rows[0][3].ToString();//tipo documento
        					    tabla_reporte.Rows[num][7]=tabla_datos_fact.Rows[0][2].ToString()+" y "+tabla_datos_fact.Rows[0][3].ToString();//creditos
        					    tabla_reporte.Rows[num][8]=(interprete_fechas(tabla_estrados.Rows[0][2].ToString())).ToLower();//fecha_doc
        					    tabla_reporte.Rows[num][9]=tabla_estrados.Rows[0][15].ToString();//sub_emisora
        					    tabla_reporte.Rows[num][10]=tabla_estrados.Rows[0][14].ToString();//titular sub_emisora
        					    tabla_reporte.Rows[num][11]=tabla_datos_fact.Rows[0][4].ToString();//notificador
        					    tabla_reporte.Rows[num][12]=interprete_fechas(tabla_estrados.Rows[0][6].ToString());//fecha_acta
        					    tabla_reporte.Rows[num][13]=tabla_estrados.Rows[0][7].ToString();//fojas
        					    tabla_reporte.Rows[num][14]=tabla_estrados.Rows[0][5].ToString();//motivo_estrados
        					    tabla_reporte.Rows[num][15]=tabla_estrados.Rows[0][4].ToString();//supuesto_estrados
        					    tabla_reporte.Rows[num][16]=interprete_fechas(tabla_estrados.Rows[0][10].ToString());//fecha_ini_not
        					    tabla_reporte.Rows[num][17]=interprete_fechas(tabla_estrados.Rows[0][11].ToString());//fecha_fin_not
        					    tabla_reporte.Rows[num][18]=interprete_fechas(tabla_estrados.Rows[0][12].ToString());//fecha_ret_not
        					    tabla_reporte.Rows[num][19]=noti_estrados;//notificador estrados
        					    tabla_reporte.Rows[num][20]=notis_estrados;//notificadores estrados
        					    tabla_reporte.Rows[num][21]=interprete_numero(tabla_estrados.Rows[0][16].ToString().Substring(0,2));//hora_not_horas
        					    tabla_reporte.Rows[num][22]=interprete_numero(tabla_estrados.Rows[0][16].ToString().Substring(3,2));//hora_not_mins
        					    tabla_reporte.Rows[num][23]=interprete_fechas(tabla_estrados.Rows[0][8].ToString());//fecha_firma_alta
        					    tabla_reporte.Rows[num][24]=interprete_fechas(tabla_estrados.Rows[0][9].ToString());//fecha_publicacion
        					    dataGridView1.DataSource=tabla_reporte;
        					    num++;
                            }
        				}else{
        					MessageBox.Show("El Folio Buscado ya se encuentra en la lista","AVISO");
        				}
                    }
                    else
                    {
                        //MessageBox.Show("El Folio Buscado no se encuentra en Estrados", "AVISO");
                        test_num++;
                        test_no = tabla_estrados.Rows[0][1].ToString();
                    }
        		}else{//
        			/// <summary>
        			/// -------------*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-SI ES OFICIO
        			/// </summary>
                    /// 
                    if (Int32.TryParse(tabla_estrados.Rows[0][1].ToString(), out num_verif) == true)
                    {
                        tabla_datos_fact = conex1.consultar("SELECT reg_nss,folio,acuerdo,fecha_oficio,razon_social,receptor,domicilio_oficio,localidad_oficio,cp_oficio,rfc_oficio,nombre_oficio FROM oficios WHERE id_oficios=" + tabla_estrados.Rows[0][1].ToString().Substring(4, (tabla_estrados.Rows[0][1].ToString().Length - 4)) + "");
                    }
                    else
                    {
                        tabla_datos_fact.Rows.Clear();
                    }

                    //MessageBox.Show(""+tabla_usuarios.Rows.Count);
        			while(j<tabla_usuarios.Rows.Count){
        				if(tabla_usuarios.Rows[j][0].ToString().Equals(tabla_estrados.Rows[0][13].ToString())==true){
        					noti_estrados=tabla_usuarios.Rows[j][1].ToString()+" "+tabla_usuarios.Rows[j][2].ToString();
        				}
        				j++;
        			}
        			
        			if(tabla_datos_fact.Rows.Count>0){
        				//tabla_sindo=conex2.consultar("SELECT id_sindo,domicilio,localidad,cp,rfc FROM sindo WHERE registro_patronal= \""+tabla_datos_fact.Rows[0][1].ToString()+"\"");
        				if(tabla_reporte.Rows.Count>0){
        					//tabla_reporte.Rows.Clear();
        				}else{
        					if(tabla_reporte.Columns.Count>0){
        						
        					}else{
        						tabla_reporte.Columns.Add("NUM");
        						tabla_reporte.Columns.Add("FOLIO");
        						tabla_reporte.Columns.Add("RAZON SOCIAL");
        						tabla_reporte.Columns.Add("REGISTRO PATRONAL");
        						tabla_reporte.Columns.Add("R.F.C.");
        						tabla_reporte.Columns.Add("DOMICILIO");
        						tabla_reporte.Columns.Add("TIPO DE DOCUMENTO");
        						tabla_reporte.Columns.Add("CREDITOS");
        						tabla_reporte.Columns.Add("FECHA_DOC");
        						tabla_reporte.Columns.Add("SUB");
        						tabla_reporte.Columns.Add("TITULAR_SUB");
        						tabla_reporte.Columns.Add("NOTIFICADOR");
        						tabla_reporte.Columns.Add("FECHA_ACTA");
        						tabla_reporte.Columns.Add("FOJAS");
        						tabla_reporte.Columns.Add("MOTIVO_ESTRADOS");
        						tabla_reporte.Columns.Add("SUPUESTO_ESTRADOS");
        						tabla_reporte.Columns.Add("FECHA_INI_NOT");
        						tabla_reporte.Columns.Add("FECHA_FIN_NOT");
        						tabla_reporte.Columns.Add("FECHA_RETIRO_NOT");
        						tabla_reporte.Columns.Add("NOTIFICADOR_ESTRADOS");
        						tabla_reporte.Columns.Add("NOTIFICADORES_ESTRADOS");
        						tabla_reporte.Columns.Add("HORA_NOT_HORAS");
        						tabla_reporte.Columns.Add("HORA_NOT_MINS");
        						tabla_reporte.Columns.Add("FECHA_FIRMA_ALTA");
        						tabla_reporte.Columns.Add("FECHA_PUBLICACION");
        					}
        				}
        				
        				while(l<tabla_reporte.Rows.Count){
        					if(tabla_reporte.Rows[l][1].ToString().Equals(tabla_estrados.Rows[0][0].ToString())==true){
        						listar=1;
        					}
        					l++;
        				}
        				
        				
        				if(listar==0){
                            if(tabla_sindo.Rows.Count<1){
                                if (comboBox1.SelectedIndex == 2)
                                {
                                    regs_sin_dom.Rows.Add(tabla_datos_fact.Rows[0][1].ToString(), tabla_datos_fact.Rows[0][0].ToString());
                                }
                                else
                                {
                                    MessageBox.Show("El registro buscado no cuenta con información de domicilio en la Base de Datos", "AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                                }
                            }else{
        					    tabla_reporte.Rows.Add();
        					    tabla_reporte.Rows[num][0]=num+1;//num
        					    tabla_reporte.Rows[num][1]=tabla_estrados.Rows[0][0].ToString();//folio
        					    tabla_reporte.Rows[num][2]=tabla_datos_fact.Rows[0][4].ToString();//razon_social
        					    tabla_reporte.Rows[num][3]=tabla_datos_fact.Rows[0][0].ToString();//reg_pat
        					    tabla_reporte.Rows[num][4]=tabla_datos_fact.Rows[0][9].ToString();//rfc
        					    tabla_reporte.Rows[num][5]=tabla_datos_fact.Rows[0][6].ToString()+" "+tabla_datos_fact.Rows[0][7].ToString()+" "+tabla_datos_fact.Rows[0][8].ToString();//domicilio
        					    tabla_reporte.Rows[num][6]=tabla_estrados.Rows[0][3].ToString();//tipo documento
        					    tabla_reporte.Rows[num][7]=tabla_datos_fact.Rows[0][1].ToString()+" y "+tabla_datos_fact.Rows[0][2].ToString();//creditos
        					    tabla_reporte.Rows[num][8]=interprete_fechas(tabla_estrados.Rows[0][2].ToString());//fecha_doc
        					    tabla_reporte.Rows[num][9]=tabla_estrados.Rows[0][15].ToString();//sub_emisora
        					    tabla_reporte.Rows[num][10]=tabla_estrados.Rows[0][14].ToString();//titular sub_emisora
        					    tabla_reporte.Rows[num][11]=tabla_datos_fact.Rows[0][5].ToString();//notificador
        					    tabla_reporte.Rows[num][12]=interprete_fechas(tabla_estrados.Rows[0][6].ToString());//fecha_acta
        					    tabla_reporte.Rows[num][13]=tabla_estrados.Rows[0][7].ToString();//fojas
        					    tabla_reporte.Rows[num][14]=tabla_estrados.Rows[0][5].ToString();//motivo_estrados
        					    tabla_reporte.Rows[num][15]=tabla_estrados.Rows[0][4].ToString();//supuesto_estrados
        					    tabla_reporte.Rows[num][16]=interprete_fechas(tabla_estrados.Rows[0][10].ToString());//fecha_ini_not
        					    tabla_reporte.Rows[num][17]=interprete_fechas(tabla_estrados.Rows[0][11].ToString());//fecha_fin_not
        					    tabla_reporte.Rows[num][18]=interprete_fechas(tabla_estrados.Rows[0][12].ToString());//fecha_ret_not
        					    tabla_reporte.Rows[num][19]=noti_estrados;//notificador estrados
        					    tabla_reporte.Rows[num][20]=notis_estrados;//notificadores estrados
        					    tabla_reporte.Rows[num][21]=interprete_numero(tabla_estrados.Rows[0][16].ToString().Substring(0,2));//hora_not_horas
        					    tabla_reporte.Rows[num][22]=interprete_numero(tabla_estrados.Rows[0][16].ToString().Substring(3,2));//hora_not_mins
        					    tabla_reporte.Rows[num][23]=interprete_fechas(tabla_estrados.Rows[0][8].ToString());//fecha_firma_alta
        					    tabla_reporte.Rows[num][24]=interprete_fechas(tabla_estrados.Rows[0][9].ToString());//fecha_publicacion
        					    dataGridView1.DataSource=tabla_reporte;
        					    num++;
                            }
        				}else{
        					MessageBox.Show("El Folio Buscado ya se encuentra en la lista","AVISO");
        				}
        				
        			}else{
                        if (comboBox1.SelectedIndex != 2)
                        {
                            MessageBox.Show("El Folio Buscado no se encuentra en Estrados", "AVISO");
                        }
                        test_num++;
        			}
        		}
        		label2.Text="Registros: "+(dataGridView1.Rows.Count);
        		label2.Refresh();
        	}
        }
        
        public void buscar_en_datos_fact(){
        	
        	String reg_pat,cc="",cm="",cad="",id;
        	int i=0;
        	
        	reg_pat=maskedTextBox1.Text;
        	while (i < reg_pat.Length){
        		if (((reg_pat.Substring(i, 1)).Equals(" ")) || ((reg_pat.Substring(i, 1)).Equals("-"))){
        		}else{
        			cad += reg_pat.Substring(i, 1);
        		}
        		i++;
        	}
        	
        	if(cad.Length>9){
        		maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
        		reg_pat=cad;
        		
        	}else{
        		reg_pat="";
        	}
        	
        	if(reg_pat.Length>9){
        		if((maskedTextBox2.MaskCompleted==true)||(maskedTextBox3.MaskCompleted==true)){
        			if(maskedTextBox2.MaskCompleted==true){
        				cc=" AND credito_cuotas=\""+maskedTextBox2.Text+"\"";
        			}
        			
        			if(maskedTextBox3.MaskCompleted==true){
        				cm=" AND credito_multa=\""+maskedTextBox3.Text+"\"";
        			}
        		}        		
        		
        		conex.conectar("base_principal");
        		tabla_datos_fact1 = conex.consultar("SELECT id FROM datos_factura WHERE registro_patronal2=\""+reg_pat.Substring(0,10)+"\" "+cc+" "+cm+"");
        		
        		if(tabla_datos_fact1.Rows.Count>0){
        			tabla_estrados=conex.consultar("SELECT folio FROM estrados WHERE id_credito=\""+tabla_datos_fact1.Rows[0][0].ToString()+"\"");
        			
        			if(tabla_estrados.Rows.Count>0){
        				folio=tabla_estrados.Rows[0][0].ToString();
        				buscar_folio();
        			}else{
        				MessageBox.Show("El Crédito Buscado no se encuentra en Estrados","AVISO");
        			}
        		}
        		
        		
        	}
        }

        public void buscar_en_num_pack()
        {
            if (comboBox2.SelectedIndex > -1)
            {
                comboBox2.Enabled = false;
                packs = conex4.consultar("SELECT folio FROM estrados WHERE paquete="+comboBox2.SelectedItem.ToString()+"");
                //MessageBox.Show("" + packs.Rows.Count);
                test_num = 0;
                regs_sin_dom.Rows.Clear();
                tabla_reporte.Rows.Clear();
                num = 0;
                
                for (int i = 0; i < packs.Rows.Count;i++)
                {
                    folio = packs.Rows[i][0].ToString();
                    buscar_folio();
                }
                comboBox2.Enabled = true;

                MessageBox.Show("Se omitieron "+test_num+" registros que fueron removidos de la Base de Datos.\nNo se encontró en la base de datos el domicilio de "+regs_sin_dom.Rows.Count+" registros.", "REGISTROS CARGADOS CORRECTAMENTE",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                button3.Visible = true;
            }
            else
            {
                MessageBox.Show("Selecciona un número de paquete", "AVISO");
            }
        }

        public void llenar_Cb2()
        {
            conex4.conectar("base_principal");
            comboBox2.Items.Clear();
            int i = 0;
            packs = conex4.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC");
            while (i < packs.Rows.Count) 
            {
                if (packs.Rows[i][0].ToString() != "0")
                {
                    //comboBox2.Items.Add("TODOS");
                    comboBox2.Items.Add(packs.Rows[i][0].ToString());
                }
               
                i++;
            } 
            i = 0;
            conex4.cerrar();
        }
        
		void Button2Click(object sender, EventArgs e)
		{
            conex.conectar("base_principal");
            conex1.conectar("base_principal");
            conex2.conectar("base_principal");

			switch (comboBox1.SelectedIndex) {
					case 0: //si es folio
						folio=maskedTextBox1.Text;
						buscar_folio();
					break;
					
					case 1: //si es por registro patronal
						buscar_en_datos_fact();
					break;

                    case 2: //si es por num_pack
                        buscar_en_num_pack();
                    break;
					
					default:
					break;
				}

            conex.cerrar();
            conex1.cerrar();
            conex2.cerrar();
		}
		
		void Reporte_estradosLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            int i =0;
			conex3.conectar("base_principal");
			comboBox1.SelectedIndex=0;

			//maskedTextBox1.Text="1438"+DateTime.Today.Year.ToString().Substring(2,2);
            maskedTextBox1.Text = conex.leer_config_sub()[1] + conex.leer_config_sub()[4] + DateTime.Today.Year.ToString().Substring(2, 2);
			tabla_usuarios=conex3.consultar("SELECT id_usuario,nombre,apellido,puesto FROM usuarios WHERE puesto=\"Auxiliar Estrados\"");
			
			while(i<tabla_usuarios.Rows.Count){
				notis_estrados+=tabla_usuarios.Rows[i][1].ToString()+" "+tabla_usuarios.Rows[i][2].ToString()+" ó " ;
				i++;
			}

			notis_estrados=notis_estrados.Substring(0,notis_estrados.Length-3);
            llenar_Cb2();
            regs_sin_dom.Columns.Add("REGISTRO PATRONAL");
            regs_sin_dom.Columns.Add("RAZON SOCIAL");
            contextMenuStrip1.Enabled = false;
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				switch (comboBox1.SelectedIndex) {
					case 0: //si es folio
						folio=maskedTextBox1.Text;
						buscar_folio();
					break;
					
					case 1: //si es por registro patronal
						maskedTextBox2.Focus();
						//buscar_en_datos_fact();
					break;
					
					default:
					break;
				}
				
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				SaveFileDialog dialog_save = new SaveFileDialog();
				dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
				dialog_save.Title = "Guardar Resultados de Captura";//le damos un titulo a la ventana

				if (dialog_save.ShowDialog() == DialogResult.OK){
					XLWorkbook wb = new XLWorkbook();
					wb.Worksheets.Add(tabla_reporte, "hoja_lz");
					wb.SaveAs(@""+dialog_save.FileName+"");
					MessageBox.Show("Archivo guardado correctamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
				}
			}			
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (comboBox1.SelectedIndex) {
				case 0: //si es folio
						maskedTextBox1.Clear();
						maskedTextBox1.Mask="E0000/00-00000";
                        maskedTextBox1.Text = conex.leer_config_sub()[1] + conex.leer_config_sub()[4] + DateTime.Today.Year.ToString().Substring(2, 2);
						maskedTextBox1.Visible=true;
						textBox1.Visible=false;
						maskedTextBox2.Visible=false;
						maskedTextBox3.Visible=false;
						label3.Visible=false;
						label4.Visible=false;
                        comboBox2.Visible = false;
                        button3.Visible = false;
						button2.Location = new System.Drawing.Point((maskedTextBox1.Location.X)+152, 60);
						label13.ForeColor=Color.Lime;
						label13.Text="Folio";
						maskedTextBox1.Focus();
					break;
				case 1: //si es reg_pat
						label13.ForeColor=Color.PaleTurquoise;
						label13.Text="Registro Patronal";
						maskedTextBox1.Clear();
						maskedTextBox1.Mask="a00 - 00000 - 00 - 0";				
						maskedTextBox1.Visible=true;
						maskedTextBox2.Clear();
						maskedTextBox2.Visible=true;
						maskedTextBox3.Clear();
						maskedTextBox3.Visible=true;
						label3.Visible=true;
						label4.Visible=true;
						textBox1.Visible=false;
                        comboBox2.Visible = false;
                        button3.Visible = false;
						button2.Location = new System.Drawing.Point((maskedTextBox1.Location.X)+450, 60);
						maskedTextBox1.Focus();
					break;
                case 2: //si es reg_pat
                        label13.ForeColor=Color.DodgerBlue;
						label13.Text="Num Paquete";
                        maskedTextBox1.Visible = false;
                        maskedTextBox2.Visible=false;
						maskedTextBox3.Visible=false;
                        comboBox2.Visible = true;
                        button3.Visible = false;
                        label3.Visible=false;
						label4.Visible=false;
                    break;
				default:
					break;
			}
			
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				maskedTextBox3.Focus();
			}
		}
		
		void MaskedTextBox3KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				button2.Focus();
			}
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.MaskCompleted==true){
				switch (comboBox1.SelectedIndex) {
					case 0: //si es folio
						button2.Focus();
					break;
					
					case 1: //si es por registro patronal
						maskedTextBox2.Focus();
					break;
					
					default:
					break;
				}
			}
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.MaskCompleted==true){
				maskedTextBox3.Focus();
			}
		}
		
		void MaskedTextBox3TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox3.MaskCompleted==true){
				button2.Focus();
			}
		}

        private void button3_Click(object sender, EventArgs e)
        {
            if (ver_regs_sin_dom == 0)
            {
                dataGridView1.DataSource = regs_sin_dom;
                ver_regs_sin_dom++;
                button3.Text = "  Ver Registros del Reporte ";
                contextMenuStrip1.Enabled = true;
            }
            else
            {
                dataGridView1.DataSource = tabla_reporte;
                ver_regs_sin_dom = 0;
                button3.Text = "  Ver Registros sin Domicilio ";
                contextMenuStrip1.Enabled = false;
            }
            
            label2.Text = "Registros: " + (dataGridView1.Rows.Count);
            label2.Refresh();
        }

        private void capturarDomicilioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Capturar_Domicilio captu = new Capturar_Domicilio();
            captu.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                MainForm.reg_pat_sindo = dataGridView1[0, e.RowIndex].Value.ToString();
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[0, e.RowIndex];

                }
            }

            if (e.RowIndex > -1)
            {
                MainForm.reg_pat_sindo = dataGridView1[0, e.RowIndex].Value.ToString();
            }
        }
	}
}

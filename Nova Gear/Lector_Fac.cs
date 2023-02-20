/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 30/04/2015
 * Hora: 11:47 a. m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Lector_Fac.
	/// </summary>
	public partial class Lector_Fac : Form
	{
		public Lector_Fac()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			InitializeComponent();

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
			
	    //Declaración de Variables
		String ruta,linea,porcent_text,sector_n,reg_pat,ra_soc,periodo,temp_peri,temp_peri1,cred_cuo,sql,hoja,cad_con_ofis;
		String reg_pat1,reg_pat2,cred_mul,cuota_omi,cuota_ac,recargo,imp_multa,imp_total,linea2,linea_vac,archivo,tabla,ext,nvatabla;
		String nombre_per,nombre_periodo,municipio,delegacion,subdele,domicilio,giro,localidad,marca,subde,folio,cons_ofis,col;
		String cons_tabla,num_cred,num_trab,nom_cm_ac,num_per_cm,tipo_doc,nombre_per_oficio,peri_a_leer,tipo_doc_mul;
		string[] palabra,reg_part,text,aux;
		int tot_lin=0,porcent=0,i=0,j=0,k=0,bandera=0,contador=0,bandera2=0,num_cm=0,contador_lin=0,eco=0,error_nul=0,nvomini=0,candado=0,porcent2=0;
        int tipo_file = 0, gancho = 0, ver = 0, lin_var = 0, bloque = 0, finbloque = 0, sub = 0, filas = 0, tipocarga = 0, tot_row = 0, tot_col = 0, cm = 0, band_peri = 0, l = 0, no_del_real, no_sub_real, n = 0, tipo_rt = 0;
		double cuo_o=0,cuo_a=0,rec=0,imp_mul=0,imp_tot=0,por=0,tot1=0,tot2=0,totr=0;
		char espacio=' ';

        public static int tipo_anual = 0;

		Conexion conex = new Conexion();
		DataTable consultamysql = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;    
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;
        
		//Declaracion del Delegado y del Hilo para ejecutar un subproceso
		private Thread hilosecundario = null;
		private Thread hiloterciario = null;

		public void opciones(){
			
			//***********COP***********
			if((radioButton1.Checked)&&(radioButton7.Checked)){
				nombre_per="COP_ECO_EST_";
				tipo_file=1;
				eco=1;
			}
			
			if((radioButton1.Checked)&&(radioButton8.Checked)){
				nombre_per="COP_ECO_MEC_";
				tipo_file=1;
				eco=1;
			}
			
			if((radioButton1.Checked)&&(radioButton9.Checked)){
				nombre_per="COP_SIVEPA_ANU_";
				tipo_file=1;
				eco=0;
			}
			
			if((radioButton1.Checked)&&(radioButton10.Checked)){
				nombre_per="COP_SIVEPA_EXT_";
				tipo_file=1;
				eco=0;
			}
			
			if((radioButton1.Checked)&&(radioButton11.Checked)){
				nombre_per="COP_SIVEPA_OPO_";
				tipo_file=1;
				eco=0;
			}
			
			//***********RCV***********			
			if((radioButton2.Checked)&&(radioButton7.Checked)){
				nombre_per="RCV_ECO_EST_";
				eco=1;
				tipo_file=2;
			}
			
			if((radioButton2.Checked)&&(radioButton8.Checked)){
				nombre_per="RCV_ECO_MEC_";
				eco=1;
				tipo_file=2;
			}
			
			if((radioButton2.Checked)&&(radioButton9.Checked)){
				nombre_per="RCV_SIVEPA_ANU_";
				eco=0;
				tipo_file=2;
			}
			
			if((radioButton2.Checked)&&(radioButton10.Checked)){
				nombre_per="RCV_SIVEPA_EXT_";
				eco=0;
				tipo_file=2;
			}
			
			if((radioButton2.Checked)&&(radioButton11.Checked)){
				nombre_per="RCV_SIVEPA_OPO_";
				eco=0;
				tipo_file=2;
			}
			
			//EMA - EBA ********************
		    if(radioButton3.Checked){
				tipo_file=3;
			}
			
			//Multa(oficio) - Carga Manual******
			if(radioButton5.Checked){
				nombre_per="OFICIOS";
				tipo_file=4;
			}
			
			this.Text="Nova Gear: Lector de Facturas - "+nombre_per.Substring(0,nombre_per.Length-1);
			
		}

		public void inicio(){
			
			//try{
				
				//Comienza Lectura
				StreamReader t4 = new StreamReader(ruta);
				sector_n="";
				nvomini=0;
				//Lectura linea por linea
				while(!t4.EndOfStream){
					
					linea = t4.ReadLine();
					linea = linea.Replace("'","_");
					linea = linea.Replace("\"","_");
					//linea = linea+"\n";
					tot_lin++;
					Invoke(new MethodInvoker(delegate
					                         {
					                         	//  Ejemplo: Mostrar datos de los eventos
					                         	// enviados por el Thread en una ListBox
					                         	textBox1.AppendText(linea+"\n");
					                         	progreso_facturas();
					                         }));
					//textBox1.AppendText(linea+"\n");
					//SetText(linea);

                    n = tipo_rt;

					switch(tipo_file){
							
						case 1:	//busca lineas vacias COP------
							if(linea.Length <= 8)
							{
								bandera=0;
							}
							if(eco==0){
								//llama a la funcion que descifra la linea
								descomponer_linea_sivepa();
							}else{
								//llama a la funcion que descifra la linea
								descomponer_linea_eco();
							}
							//llama a la funcion que guarda la linea descrifrada
								capturar_datos();
							break;
							
						case 2:	//busca lineas vacias RCV-----
							if(linea.Length <= 8)
							{
								bandera=0;
							}
							
							if(eco==0){
								//llama a la funcion que descifra la linea
								descomponer_linea_sivepa();
							}else{
								//llama a la funcion que descifra la linea
								descomponer_linea_eco();
							}
							
								//llama a la funcion que guarda la linea descrifrada
								capturar_datos();
							break;
							
						case 4: //llama a la funcion que descifra y guarda la linea MULTAS OFICIOS-----
								descomponer_linea_multa();
								//descomponer_linea_multa_RT_2018();
							break;
							
							default: MessageBox.Show("Hola :D");
							break;
					}
					
				}//Fin While
				
				
				if((tipo_file==1)||(tipo_file==2)||(tipo_file==4)){
					nvomini=1;
					candado=1;
					insertar_datos();
				}
				
			//}catch(Exception ex) {
			//	MessageBox.Show("ERROR: \n" + ex, "Error al Procesar");
			//}
		}
		
		public void leer_periodo_inicial(){
			String line_z,lina_temp="";
			int m=0,n=0,o=0;
			peri_a_leer="";
			
			while(m<20){
				line_z=aux[m];
				if(nombre_per.Substring(4,3).Equals("ECO")){
					if(line_z.Contains("FACTURA SECTORIZADA")==true && o==0){
						peri_a_leer=line_z.Substring(120,10);
						
						while (n < peri_a_leer.Length){
							if (!(peri_a_leer.Substring(n, 1)).Equals(" ")){
								lina_temp += peri_a_leer.Substring(n, 1);
							}
							n++;
						}
						//MessageBox.Show(lina_temp);
						lina_temp=lina_temp.Substring(3,4)+lina_temp.Substring(0,2);
						peri_a_leer=lina_temp;
						o=1;
					}
					
				}else{
					if(line_z.Contains("PERIODO")==true && o==0){
						peri_a_leer=aux[m+1].Substring(68,7);
						peri_a_leer= peri_a_leer.Substring(3,4)+peri_a_leer.Substring(0,2);
						o=1;
					}
					
					if(line_z.Contains("CICLO")==true && o==0){
                        try
                        {
                            peri_a_leer = line_z.Substring(122, 8);
                            tipo_rt = 1;
                        }
                        catch(Exception exz)
                        {
                            peri_a_leer = line_z.Substring(121, 8);
                            tipo_rt = 0;
                        }

                        //MessageBox.Show("n="+n);
						peri_a_leer= peri_a_leer.Substring(0,4)+peri_a_leer.Substring(peri_a_leer.Length-2,2);
						o=1;
					}
					
				}
				m++;
			}
				
		}
		
		public void leer_linea(){
			
			try{
				//Controlar la extension del archivo dependiendo del tipo de archivo a leer
				
				switch(tipo_file){
					
					case 1: openFileDialog1.Filter = "Archivo de Factura (*.T;*.TXT)|*.T;*.TXT";
							openFileDialog1.Title = "Seleccione el archivo COP";//le damos un titulo a la ventana
					break;
					
					case 2: openFileDialog1.Filter = "Archivo de Oficio (*.T;*.TXT;*.R)|*.T;*.TXT;*.R";
							openFileDialog1.Title = "Seleccione el archivo RCV";//le damos un titulo a la ventana
				    break;
					
					case 4: openFileDialog1.Filter = "Archivo de Oficio (*.T;*.TXT;*.R)|*.T;*.TXT;*.R";
							openFileDialog1.Title = "Seleccione el archivo de Oficio";//le damos un titulo a la ventana
				    break;
				    
				   default: openFileDialog1.Filter = "Archivo de Factura (*.T;*.TXT)|*.T;*.TXT";
				   			openFileDialog1.Title = "Seleccione el archivo de Factura";//le damos un titulo a la ventana
				   break;
				
				}
				
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					//Inicializar variables
					textBox1.Text="";
					tot_lin=0;
					progressBar1.Value = 0;
					
					//Lectura Previa del archivo para conocer su longitud
					ruta = openFileDialog1.FileName;
					StreamReader t3 = new StreamReader(ruta);
					textBox1.Text = t3.ReadToEnd();
					aux=textBox1.Lines;
					tot1=aux.Length;
					leer_periodo_inicial();
					t3.Close();
					textBox1.Text="";
					textBox4.Text="";
					label2.Text="Archivo:  "+openFileDialog1.SafeFileName;
					
					MessageBox.Show("Archivo Abierto","Aviso");
					button5.Enabled = true;
				}

			}catch(Exception ex) {
				MessageBox.Show("ERROR: \n" + ex, "Error al Abrir");
			}
			
			
		}
		
		public void descomponer_linea_sivepa(){
			
			if(linea.Length>6){
				linea_vac=linea.Substring(1,4);
				
				if (linea_vac.Equals((no_del_real.ToString()+no_sub_real.ToString()))){//default1438
					//label4.Text ="";

					if((linea.Length)>90){
						//label2.Text = linea.Substring(22,6);
						
						//Extraer Sector de Notificación
						if((linea.Substring(131,6)).Equals("SECTOR")){
							sector_n = linea.Substring(158,2);
						}
						
						//Extraer Linea de Datos
						if(bandera==1){
							
							//Extraer la 1ra Parte de los Datos
							reg_pat = linea.Substring(15,14);
							ra_soc = linea.Substring(31,35);
							periodo = linea.Substring(68,7);
							band_peri=1;
							cred_cuo = linea.Substring(77,9);
							cred_mul = linea.Substring(88,9);
							
							//Extraer la 2da Parte de los Datos
							linea2 = linea.Substring(97,76);
							palabra = linea2.Split(espacio);
							contador=0;
							
							for (i=0; i<palabra.Length; i++){
								
								if(!palabra[i].Equals("")){
									
									if(contador==0){
										cuota_omi = palabra[i];
									}
									if(contador==1){
										cuota_ac = palabra[i];
									}
									if(contador==2){
										recargo = palabra[i];
									}
									if(contador==3){
										imp_multa = palabra[i];
									}
									if(contador==4){
										imp_total = palabra[i];
									}
									
									contador= contador+1;
									bandera2=1;
								}
							}
						}
						
						//Activar Bandera
						if(linea.Length >90){
							if((linea.Substring(15,4)).Equals("REG.")){
								bandera=1;
							}
						}
					}
				}
			}
		}
	
		public void descomponer_linea_eco(){
			
			if(linea.Length>6){
				linea_vac=linea.Substring(1,4);
				
				if (linea_vac.Equals((no_del_real.ToString()+no_sub_real.ToString()))){
					//label4.Text ="";

					if((linea.Length)>90){
						//label2.Text = linea.Substring(22,6);
						
						//Desactivar Bandera
						if((linea.Substring(108,6)).Equals("CUOTAS")){
							bandera=0;
						}
						
						//Extraer Sector de Notificación
						if((linea.Substring(102,6)).Equals("SECTOR")){
							sector_n = linea.Substring(129,2);
						}
						
						if((linea.Substring(57,7)).Equals("FACTURA")){
								periodo = linea.Substring(120,7);
								band_peri=1;
							}
						
						//Extraer Linea de Datos
						if(bandera==1){
							
							//Extraer la 1ra Parte de los Datos
							reg_pat = linea.Substring(16,14);
							ra_soc = linea.Substring(32,35);
							//periodo = linea.Substring(68,7);
							cred_cuo = linea.Substring(74,9);
							cred_mul = linea.Substring(85,9);
							
							//Extraer la 2da Parte de los Datos
							linea2 = linea.Substring(97,(linea.Length - 97));
							palabra = linea2.Split(espacio);
							contador=0;
							
							for (i=0; i<palabra.Length; i++){
								
								if(!palabra[i].Equals("")){
									
									if(contador==0){
										cuota_omi = palabra[i];
									}
									if(contador==1){
										cuota_ac = palabra[i];
									}
									if(contador==2){
										recargo = palabra[i];
									}
									if(contador==3){
										imp_multa = palabra[i];
									}
									if(contador==4){
										imp_total = palabra[i];
									}
									
									contador= contador+1;
									bandera2=1;
								}
							}
						}
						
						//Activar Bandera
						if(linea.Length >90){
							if((linea.Substring(75,6)).Equals("CUOTAS")){
								bandera=1;
							}
						}
					}
				}
			}
		}
		
		public void descomponer_linea_multa(){
             
			if((linea!="")&&(finbloque==0)){
				
				if((linea.Length>100)||(gancho==1)){
					if(linea.Length>=129){
						//MessageBox.Show(linea.Substring(114,5));
						if((linea.Substring(114+n,5)).Equals("CICLO")){
							nombre_per=linea.Substring(121+n,8);
						}
					}

                    //MessageBox.Show("n= " + n + ", cacho_linea_sub= " + linea.Substring(44 + n, 13) + " \n num_sub_founs= " + linea.Substring(59 + n, 2) + ", num_sub= " + no_sub_real.ToString());

                    if (((linea.Substring(44 + n, 13)).Equals("SUBDELEGACION")) && ((linea.Substring(59+n, 2)).Equals(no_sub_real.ToString())))
                    {
						delegacion=linea.Substring(12+n,2);
                        subdele = linea.Substring(59 + n, 2);
                        municipio = linea.Substring(76 + n, 3);
                        sector_n = linea.Substring(107 + n, 2);
						bloque=1;
						
					}else{
                        if ((linea.Substring(44 + n, 13)).Equals("SUBDELEGACION"))
                        {
                            subde = linea.Substring(59 + n, 2);
							if(subde != "=="){
							//MessageBox.Show(""+subde);
							sub= Convert.ToInt32(subde);
							if(sub>no_sub_real){
								finbloque=1;
							}
							}
						}
					
					}

                    if ((linea.Substring(51 + n, 5)).Equals("TOTAL"))
                    {
					bloque=0;
					}
					
					if((gancho==1)&&(bloque==1)){ 
						gancho=0;
                        marca = linea.Substring(13 + n, 4);
                        giro = linea.Substring(24 + n, 40);
						lin_var=((linea.Length)-65-n);
                        localidad = linea.Substring(65 + n, lin_var);
						
						//Ajustar valores para ingresarlos a la BD
						if(!nombre_per.Equals("")){
						    nombre_periodo=(nombre_per.Substring(0,5)+nombre_per.Substring(6,2));
						}

						reg_pat=reg_pat.Replace(" ","-");
						reg_pat=reg_pat.Substring(1,reg_pat.Length-1);

						reg_part = reg_pat.Split('-');

                        if(n>0){
                            reg_pat = reg_pat.Substring(0, 3) + "-" + reg_pat.Substring(4, 5) + "-" + reg_pat.Substring(9, 2) + "-" + reg_pat.Substring(12,1);
                        }
						
						for(j=0; j<reg_part.Length; j++){
							if(!reg_part[j].Equals("")){
								reg_pat1+=reg_part[j];
							}
						}
						reg_pat2 = reg_pat1.Substring(0,10);
						
						if(localidad.Length>40){
							localidad=localidad.Substring(0,39);
							localidad=localidad.TrimEnd(' ');
						
						}
						
						ra_soc=ra_soc.TrimEnd(' ');
						domicilio=domicilio.TrimEnd(' ');
						giro=giro.TrimEnd(' ');
						
						//ra_soc=ra_soc.Replace('\'',' ');
						//domicilio=domicilio.Replace('\'',' ');
						
						Invoke(new MethodInvoker(delegate
					{
						  textBox2.AppendText("INSERT INTO multas (nombre_periodo_multa,registro_patronal,registro_patronal1,registro_patronal2,razon_social,domicilio,giro_actividad,localidad,delegacion,subdelegacion,municipio,sector_notificacion_inicial,sector_notificacion_actualizado,marca,periodo,folio_credito)"+
						  "VALUES (\""+nombre_per_oficio+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+domicilio+"\",\""+giro+"\",\""+localidad+"\",\""+delegacion+"\",\""+subdele+"\",\""+municipio+"\",\""+sector_n+"\",\""+sector_n+"\",\""+marca+"\",\""+nombre_periodo+"\",\""+folio+"\")\n");
						                         
						  textBox4.AppendText("INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,subdelegacion,sector_notificacion_inicial,sector_notificacion_actualizado,observaciones,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa)"+
						                         	                    "VALUES (\""+nombre_per_oficio+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+subdele+"\",\""+sector_n+"\",\""+sector_n+"\",\""+marca+"\",\""+(nombre_periodo.Substring(0,4)+nombre_periodo.Substring(5,2))+"\",\""+folio+"\",\"-\",0.00,0.00)\n");
						                         }));
						reg_pat="";
						reg_pat1="";
						reg_pat2="";
						ra_soc="";
						domicilio="";
						giro="";
						localidad="";
						marca="";
					    contador_lin++;
					}
					
					if(((linea.Substring(0+n,8)).Equals("00000000"))&&(bloque==1)){
						gancho=1;
						reg_pat=linea.Substring(8+n,15);
						ra_soc=linea.Substring(23+n,40);
						domicilio=linea.Substring(64+n,40);
						folio=linea.Substring(105+n,9);
					}
				}
			}
			
			if(linea.Length>=129){
				if((linea.Substring(114+n,5)).Equals("CICLO")){
					nombre_per=linea.Substring(121+n,8);
				}	
			
				if((linea.Substring(44+n,13)).Equals("SUBDELEGACION")){
					subde=linea.Substring(59+n,2);
					if(subde != "=="){
						//MessageBox.Show(""+subde);
						sub= Convert.ToInt32(subde);
						if(sub==no_sub_real){
							finbloque=0;
						}
					}
				}
			}
			
		}
		
		public void descomponer_linea_multa_RT_2018(){
			String importe;
			
			if((linea!="")&&(finbloque==0)){
				if(linea.Length>87){
					//MessageBox.Show("|"+linea.Substring(77,5)+"|");
					if((linea.Substring(77,5)).Equals("MARCA")){
						marca=linea.Substring(83,4);
						//MessageBox.Show("|"+marca+"|");
					}
				}
				
				if((linea.Length>100)||(gancho==1)){
					if(linea.Length>=130){
						//MessageBox.Show(linea.Substring(115,5));
						if((linea.Substring(114,5)).Equals("CICLO")){
							nombre_per=linea.Substring(122,8);
						}
					}
						
					if(((linea.Substring(44,13)).Equals("SUBDELEGACION"))&&((linea.Substring(60,2)).Equals(no_sub_real.ToString()))){
						delegacion=linea.Substring(13,2);
						subdele=linea.Substring(60,2);
						municipio=linea.Substring(77,3);
						sector_n=linea.Substring(108,2);
						bloque=1;
						
					}else{
						if((linea.Substring(44,13)).Equals("SUBDELEGACION")){
							subde=linea.Substring(60,2);
							if(subde != "=="){
							//MessageBox.Show(""+subde);
							sub= Convert.ToInt32(subde);
							if(sub>no_sub_real){
								finbloque=1;
							}
							}
						}
					
					}
					
					/*
					if((linea.Substring(77,5)).Equals("MARCA")){
						marca=linea.Substring(83,4);
						MessageBox.Show("|"+marca+"|");
					}
					*/
					
					if((linea.Substring(76,5)).Equals("RECAUDACION")){
					bloque=0;
					}
					
					if((gancho==1)&&(bloque==1)){ 
						gancho=0;
						//marca=linea.Substring(5,4);
						giro=linea.Substring(25,40);
						lin_var=((linea.Length)-65);
						localidad=linea.Substring(65,lin_var);
						
						//Ajustar valores para ingresarlos a la BD
						if(!nombre_per.Equals("")){
						nombre_periodo=(nombre_per.Substring(0,5)+nombre_per.Substring(6,2));
						}
						reg_pat=reg_pat.Replace(" ","-");
						reg_pat=reg_pat.Substring(1,reg_pat.Length-1);
						reg_part = reg_pat.Split('-');
						
						for(j=0; j<reg_part.Length; j++){
							if(!reg_part[j].Equals("")){
								reg_pat1+=reg_part[j];
							}
						}
						reg_pat2 = reg_pat1.Substring(0,10);
						
						if(localidad.Length>40){
							localidad=localidad.Substring(0,39);
							localidad=localidad.TrimEnd(' ');
						
						}
						
						ra_soc=ra_soc.TrimEnd(' ');
						domicilio=domicilio.TrimEnd(' ');
						giro=giro.TrimEnd(' ');
						
						//ra_soc=ra_soc.Replace('\'',' ');
						//domicilio=domicilio.Replace('\'',' ');
						
						Invoke(new MethodInvoker(delegate
					{
						  //textBox2.AppendText("INSERT INTO multas (nombre_periodo_multa,registro_patronal,registro_patronal1,registro_patronal2,razon_social,domicilio,giro_actividad,localidad,delegacion,subdelegacion,municipio,sector_notificacion_inicial,sector_notificacion_actualizado,marca,periodo,folio_credito)"+
						  //"VALUES (\""+nombre_per_oficio+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+domicilio+"\",\""+giro+"\",\""+localidad+"\",\""+delegacion+"\",\""+subdele+"\",\""+municipio+"\",\""+sector_n+"\",\""+sector_n+"\",\""+marca+"\",\""+nombre_periodo+"\",\""+folio+"\")\n");
						                         
						  textBox4.AppendText("INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,subdelegacion,sector_notificacion_inicial,sector_notificacion_actualizado,observaciones,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa)"+
						                         	  //                  "VALUES (\""+nombre_per_oficio+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+subdele+"\",\""+sector_n+"\",\""+sector_n+"\",\""+marca+"\",\""+(nombre_periodo.Substring(0,4)+nombre_periodo.Substring(5,2))+"\",\""+folio+"\",\"-\",0.00,0.00)\n");
						                         	  "VALUES (\"COP_CLEM-"+nombre_per+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+subdele+"\",\""+sector_n+"\",\""+sector_n+"\",\""+marca+"\",\""+nombre_per.Substring(0,4)+nombre_per.Substring(6,2)+"\",\""+folio+"\",\"-\",0.00,0.00)\n");
						  }));
						reg_pat="";
						reg_pat1="";
						reg_pat2="";
						ra_soc="";
						domicilio="";
						giro="";
						localidad="";
						marca="";
					    contador_lin++;
					}
					
					if(((linea.Substring(0,8)).Equals("00000000"))&&(bloque==1)){
						gancho=1;
						reg_pat=linea.Substring(9,14);
						ra_soc=linea.Substring(24,40);
						domicilio=linea.Substring(65,40);
						folio=linea.Substring(106,9);
					}
				}
			}
			
			if(linea.Length>=129){
				if((linea.Substring(114,5)).Equals("CICLO")){
					nombre_per=linea.Substring(122,8);
				}	
			
				if((linea.Substring(44,13)).Equals("SUBDELEGACION")){
					subde=linea.Substring(60,2);
					if(subde != "=="){
						//MessageBox.Show(""+subde);
						sub= Convert.ToInt32(subde);
						if(sub==no_sub_real){
							finbloque=0;
						}
					}
				}
			}
			
		}
		
		public void capturar_datos(){
			
			if ((radioButton1.Checked)||(radioButton2.Checked)){
				opciones();
			}
			
			if(!double.TryParse(cuota_omi,out cuo_o)){
				bandera2=0;
			}
			
			
			if(bandera2==1){
				
				//Formateo de informacion antes de ingresarla a la BD
				if(band_peri==1){
					temp_peri = periodo.Substring(3,4);
					temp_peri1 =periodo.Substring(0,2);
					periodo = temp_peri+temp_peri1;
					band_peri = 0;
				}
				ra_soc=ra_soc.TrimEnd(' ');
				//ra_soc=ra_soc.Replace('\'',' ');
				
				//Descifrar registro patronal 1 y 2
				reg_part = reg_pat.Split('-');
				for(j=0; j<reg_part.Length; j++){
					if(!reg_part[j].Equals("")){
						reg_pat1+=reg_part[j];
					}
				}
				
				reg_pat2 = reg_pat1.Substring(0,10);
			   	//MessageBox.Show("VARIABLES: " +cuota_omi+","+cuota_ac+","+recargo+","+imp_multa+","+imp_total);
				
				
				//Converion de datos numericos al tipo correcto
				//if(double.TryParse(cuota_omi,out cuo_o)){
				   	cuo_o = Convert.ToDouble(cuota_omi);
				   	cuo_a = Convert.ToDouble(cuota_ac);
				   	rec = Convert.ToDouble(recargo);
				   	imp_mul = Convert.ToDouble(imp_multa);
				   	imp_tot = Convert.ToDouble(imp_total);
				//   }
				
				if(sector_n=="0"){
					sector_n="00";
				}
				
				//nombre_periodo=nombre_per+periodo;
				nombre_periodo=peri_a_leer;
				periodo=peri_a_leer.Substring(peri_a_leer.Length-6,6);
				/*if(rcv==1){
					nombre_periodo=nombre_periodo+"RCV";
				}*/
				if (eco==1){
					if(nombre_periodo.Substring(0,3).Equals("RCV")){
						tipo_doc="06";
						tipo_doc_mul="81";
					}else{
						tipo_doc="02";
						tipo_doc_mul="80";
					}
				}else{
					if(eco==0){
						tipo_doc="03";//
						if(nombre_periodo.Substring(0,3).Equals("RCV")){
							tipo_doc_mul="81";
						}else{
							tipo_doc_mul="80";
						}
					}
				}
				
				Invoke(new MethodInvoker(delegate
				{
				textBox2.AppendText("INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,sector_notificacion_actualizado,subdelegacion,incidencia,tipo_documento,tipo_documento_multa) "+
				                         	                    "VALUES (\""+nombre_periodo+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+periodo+"\",\""+cred_cuo+"\",\""+cred_mul+"\",\""+cuo_o+"\",\""+imp_mul+"\",\""+sector_n+"\",\""+sector_n+"\",\""+no_sub_real+"\",01,\""+tipo_doc+"\","+tipo_doc_mul+");\n");
				}));
				//lineatbx2="INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,periodo,credito_cuotas,credito_multa,cuotas_omitidas,importe_cuota,recargos,importe_multa,importe_total,sector_notificacion) "+
				//                    "VALUES (\""+nombre_periodo+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+periodo+"\",\""+cred_cuo+"\",\""+cred_mul+"\",\""+cuo_o+"\",\""+cuo_a+"\",\""+rec+"\",\""+imp_mul+"\",\""+imp_tot+"\",\""+sector_n+"\");\n";
				//SetText2(lineatbx2);
				
				reg_pat="";
				ra_soc="";
				//periodo="";
				cred_cuo="";
				cred_mul="";
				cuota_omi="";
				cuota_ac="";
				recargo="";
				imp_multa="";
				imp_total="";
				//sector_n="";
				bandera2=0;
				reg_pat1="";
				reg_pat2="";
				contador_lin++;
			}
		}
				
		public void insertar_datos(){
			
			conex.conectar("base_principal");
			//Insertar informacion en la BD
			k=0;
			//try{
			do{
				
				if(tipo_file < 4){
					text=textBox2.Lines;
					sql= text[k];
					conex.consultar(sql);
				}
				
				if(tipo_file==4){
					text=textBox4.Lines;
					sql= text[k];
					conex.consultar(sql);
				}
				
				
				k++;
				
				Invoke(new MethodInvoker(delegate
				                         {
				                         	progreso_facturas();
				                         }));
			}while(k<=(contador_lin-1));
			
			if(tipo_file==1||tipo_file==2){
				conex.guardar_evento("Se Inserta archivo del periodo: "+nombre_periodo+" con "+k+" registros");
				//conex.consultar("INSERT INTO estado_periodos (nombre_periodo) VALUES (\""+nombre_periodo+"\")");
			}else{
				if(tipo_file==4){
					conex.guardar_evento("Se Inserta archivo del periodo: "+nombre_per_oficio+" con "+k+" registros");
					conex.consultar("INSERT INTO estado_periodos (nombre_periodo) VALUES (\""+nombre_per_oficio+"\")");
				}
			}
			MessageBox.Show("Los datos fueron agregados exitosamente a la Base de Datos.\n\n\t\t"+k+" Registros Añadidos.", "Proceso Exitoso");
			Invoke(new MethodInvoker(delegate
			                         {
			                         	//button1.Enabled=true;
			                         	button3.Enabled=true;
			                         	//button5.Enabled=true;
			                         }));
			//	}catch(Exception exp) {
			//		MessageBox.Show("ERROR: \nK:"+k+"\nContador:"+contador_lin +"\n"+ exp, "Error al Insertar Datos en MySQL");
			//   }
			UseWaitCursor=false;
		}
		
		public void abrir_office(){
			
			//creamos un objeto OpenDialog que es un cuadro de dialogo para buscar archivos
			OpenFileDialog dialog = new OpenFileDialog();
			if((tipocarga==1)||(tipocarga==2)){
			dialog.Filter = "Archivos de Access (*.mdb;*.accdb)|*.mdb;*.accdb"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos Access
			dialog.Title = "Seleccione el archivo de Access";//le damos un titulo a la ventana
			}
			
			if((tipocarga==3)){
			dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
			//solo los archivos excel
			dialog.Title = "Seleccione el archivo de Excel";//le damos un titulo a la ventana
			}
			
			
			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				//el nombre del archivo sera asignado al textbox
				textBox3.Text = dialog.SafeFileName;
				archivo=dialog.FileName;
				//hoja = textBox4.Text; //la variable hoja tendra el valor del textbox donde colocamos el nombre de la hoja
				cargar_office(); //se manda a llamar al metodo
				//dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //se ajustan las
				//columnas al ancho del DataGridview para que no quede espacio en blanco (opcional)
			}
		}
	
		public void cargar_office(){

			//esta cadena es para archivos Access 2007 y 2010
			ext=archivo.Substring(((archivo.Length)-3),3);
			ext=ext.ToLower();
			
			if((tipocarga==1)||(tipocarga==2)){
			
				if((ext).Equals("mdb")){
					cad_con_ofis = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + archivo + "';";
					//MessageBox.Show("HOLA :D");
					
				}else{
					cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';";
				}
			}
			
			if((tipocarga==3)){
				//esta cadena es para archivos excel 2007 y 2010
				cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
				
				if(ext.Equals("lsx")){
					MessageBox.Show("Asegurate de Cerrar el archivo en Excel, Antes de abrirlo aqui","Advertencia");
				}
			}
			
			conexion = new OleDbConnection(cad_con_ofis);//creamos la conexion con la hoja de excel o Access
			conexion.Open(); //abrimos la conexion
			
			if((tipocarga==1)||(tipocarga==2)){
				System.Data.DataTable dt = conexion.GetSchema("TABLES");
				dataGridView2.DataSource =dt;
				filas=(dataGridView2.RowCount)-2;
                i = 0;
				do{
					if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
						if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
							tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
							comboBox1.Items.Add(tabla);
						}
					}
					i++;
				}while(i<=filas);
				
				
			}else{
				if(tipocarga==3){
					System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
					dataGridView2.DataSource =dt;
					filas=(dataGridView2.RowCount)-2;
					i=0;
					do{
						if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
							if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
								tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
								if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
									tabla = tabla.Remove((tabla.Length-1),1);
									comboBox1.Items.Add(tabla);
								}
							}
						}
						i++;
					}while(i<=filas);
					i=0;
					

				}
			}
			
		}

		public void conectar_office(){
			
			if(tipocarga==1){
				if(hoja.Length>5){
					cons_tabla=hoja.Substring(0,6);
				}else{
					cons_tabla=" ";
				}
				if(cons_tabla.Equals("CDEMPA")){
					
					cons_ofis = "SELECT EMIP_PATRON, EMIP_NOM_PATRON, EMIP_NUM_CRED, EMIP_NUM_TRAB_COT, EMIP_SEC, EMIP_DOM, EMIP_LOC, EMIP_IMP_EYM_FIJA, EMIP_IMP_EYM_EXCE, EMIP_IMP_EYM_PRED," +
								"EMIP_IMP_EYM_PREE, EMIP_IMP_INV_VIDA, EMIP_IMP_RIES_TRA, EMIP_IMP_GUAR, EMIP_IMP_EYM_EXCE_O, EMIP_IMP_EYM_PRED_O, EMIP_IMP_EYM_PREE_O, EMIP_IMP_INV_VIDA_O  FROM " + hoja + " ";
					button9.Enabled=true;
					
				}else{
					MessageBox.Show("Archivo o Tabla Incorrecto\n No se aplicará ningún filtro","¡Error!");
					cons_ofis = "Select * from " + hoja + " ";
					button9.Enabled=false;
				}
				
			}
		
			if(tipocarga==2){
				if(hoja.Length>5){
					cons_tabla=hoja.Substring(0,6);
				}else{
					cons_tabla=" ";
				}
				
				if(cons_tabla.Equals("CDEBPA")){
					
					cons_ofis = "SELECT EBIP_PATRON, EBIP_NOM_PATRON, EBIP_NUM_CRED, EBIP_NUM_TRAB_COT, EBIP_SEC, EBIP_DOM, EBIP_LOC, EBIP_SUMA FROM " + hoja + " ";
					button9.Enabled=true;
					
				}else{
						MessageBox.Show("Archivo o Tabla Incorrecto\n No se aplicará ningún filtro","¡Error!");
						cons_ofis = "Select * from " + hoja + " ";
						button9.Enabled=false;
						//esta cadena es para archivos excel 2007 y 2010
						//cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';";
					}
			}
			
			if((tipocarga==3)){
				cons_ofis = "Select * from [" + hoja + "$] ";
				//esta cadena es para archivos excel 2007 y 2010
				//cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
			}
			//para archivos de 97-2003 usar la siguiente cadena
			//string cadenaConexionArchivoExcel = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + archivo + "';Extended Properties=Excel 8.0;";
			
			//Validamos que el usuario ingrese el nombre de la hoja del archivo de excel a leer
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				try 
					
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_ofis, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					if(dataAdapter.Equals(null)){
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel");
						
					}else{
						dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
						dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
						conexion.Close();//cerramos la conexion
						dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
						tot_row=dataGridView1.RowCount;
						totr=Convert.ToDouble(tot_row);
						if(tipocarga == 3){
							totr = totr*2;
						}
						tot_col=dataGridView1.ColumnCount;
						label8.Text="Registros: "+tot_row;
						label8.Refresh();
						
					}
				}
				catch (AccessViolationException ex )
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n"+ex,"Error al Abrir Archivo de Excel");
				}
				
				}
		
		}
		
		public void crear_tabla_ema_eba(){
			
			try{
				
				do{
					Invoke(new MethodInvoker(delegate{
					                         	col+=dataGridView1.Columns[k].Name.ToString()+"\n";
					                         }));
					k++;
				}while(k<tot_col);
				
				//MessageBox.Show("Columnas:"+col);
				k=0;
				col="";
				
				periodo   = hoja.Substring(hoja.Length-6,6);
				periodo   = (periodo.Substring(2,4))+(periodo.Substring(0,2));
				
				if(tipocarga==1){
				conex.conectar("ema");
				sql="CREATE TABLE ema"+periodo+" (id INT(12) NOT NULL AUTO_INCREMENT, reg_pat VARCHAR(10) NOT NULL, reg_pat1 VARCHAR(11) NOT NULL, razon_social VARCHAR(100) NOT NULL, num_credito VARCHAR(10) NOT NULL, periodo VARCHAR(6) NOT NULL, num_trabajadores INT NOT NULL,"+
					"sector_not INT NOT NULL, importe DECIMAL(10,2) NOT NULL, importe_multa DECIMAL(10,2) NOT NULL, status VARCHAR(20) NOT NULL, domicilio VARCHAR(80) NOT NULL, localidad VARCHAR(60) NOT NULL, PRIMARY KEY (id),UNIQUE INDEX id_UNIQUE (id ASC));";
				conex.consultar(sql);
				leer_grid_ema();
				}else{
					if(tipocarga==2){
						conex.conectar("eba");
						sql="CREATE TABLE eba"+periodo+" (id INT(12) NOT NULL AUTO_INCREMENT, reg_pat VARCHAR(10) NOT NULL, reg_pat1 VARCHAR(11) NOT NULL, razon_social VARCHAR(100) NOT NULL, num_credito VARCHAR(10) NOT NULL, periodo VARCHAR(6) NOT NULL, num_trabajadores INT NOT NULL,"+
							"sector_not INT NOT NULL, importe DECIMAL(10,2) NOT NULL, importe_multa DECIMAL(10,2) NOT NULL,status VARCHAR(20) NOT NULL, domicilio VARCHAR(80) NOT NULL, localidad VARCHAR(60) NOT NULL, PRIMARY KEY (id),UNIQUE INDEX id_UNIQUE (id ASC));";
						conex.consultar(sql);
						leer_grid_eba();
					}
				}
				//MessageBox.Show("SQL:"+sql);
				
				
			}catch(Exception exp) {
				MessageBox.Show("ERROR: \n"+ exp, "Error al Insertar Datos en la BD");
			}
		}
		
		public void leer_grid_ema(){
			
			j=0;
			Invoke(new MethodInvoker(delegate{
			this.panel3.Visible = true;
			progressBar2.Value=0;
			button6.Enabled=false;
			button8.Enabled=false;
			button9.Enabled=false;
			comboBox1.Enabled=false;
			textBox3.Enabled=false;
			}));
			
			for(i=0;i<tot_row;i++){
				
				/*if((dataGridView1.Rows[i].Cells[j].Value.ToString()).Equals("")){
					//campo = "";
				}*/
				Invoke(new MethodInvoker(delegate{
				reg_pat2  = dataGridView1.Rows[i].Cells[0].Value.ToString();
				ra_soc    = dataGridView1.Rows[i].Cells[1].Value.ToString();
				num_cred  = dataGridView1.Rows[i].Cells[2].Value.ToString();
				num_trab  = dataGridView1.Rows[i].Cells[3].Value.ToString();
				sector_n  = dataGridView1.Rows[i].Cells[4].Value.ToString();
				domicilio = dataGridView1.Rows[i].Cells[5].Value.ToString();
				localidad = dataGridView1.Rows[i].Cells[6].Value.ToString();
				imp_tot   = Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[8].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[9].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[10].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[11].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[12].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[13].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[14].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[15].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[16].Value.ToString());
				imp_tot   = imp_tot + Convert.ToDouble(dataGridView1.Rows[i].Cells[17].Value.ToString());
				}));
				
				imp_mul   = imp_tot*0.40;
				reg_pat1  = reg_pat2.Substring(1,11);
				reg_pat   = reg_pat1.Substring(0,10);
				ra_soc	  = ra_soc.TrimEnd(' ');
				domicilio = domicilio.TrimEnd(' ');
				localidad = localidad.TrimEnd(' ');
				/*ra_soc=ra_soc.Replace('\'',' ');
				domicilio=domicilio.Replace('\'',' ');
				localidad=localidad.Replace('\'',' ');
				*/
				try{
				
				nvatabla="INSERT INTO ema"+periodo+" (reg_pat, reg_pat1, razon_social, num_credito, periodo, num_trabajadores, sector_not, importe, importe_multa, status, domicilio, localidad) VALUES"+
					"(\""+reg_pat+"\",\""+reg_pat1+"\",\""+ra_soc+"\",\""+num_cred.Substring(1,9)+"\",\""+periodo+"\",\""+num_trab+"\",\""+sector_n+"\",\""+imp_tot+"\",\""+imp_mul+"\",\"0\",\""+domicilio+"\",\""+localidad+"\")";
				conex.consultar(nvatabla);
				
				}catch(Exception exp) {
				MessageBox.Show("ERROR: \n"+ exp, "Error al Insertar Datos en la BD");
				Invoke(new MethodInvoker(delegate{
				button6.Enabled=true;
				button8.Enabled=true;
				button9.Enabled=true;
				comboBox1.Enabled=true;
				textBox3.Enabled=true;
				panel3.Visible=false;
				UseWaitCursor=false;
				}));
				}				
				Invoke(new MethodInvoker(delegate{
                    if (i == tot_row - 1) { i = tot_row; }
				progreso();
				}));
			}
			
				MessageBox.Show("Datos Ingresados Correctamente\n","Proceso Exitoso");
				conex.guardar_evento("Se Creo la tabla ema"+periodo+" en la Base de Datos de la Ema y se insertaron "+tot_row+" registros");
				Invoke(new MethodInvoker(delegate{
				button6.Enabled=true;
				button8.Enabled=true;
				button9.Enabled=true;
				comboBox1.Enabled=true;
				textBox3.Enabled=true;
				panel3.Visible=false;
				UseWaitCursor=false;
				MainForm mani = (MainForm)this.MdiParent;
				mani.toolStripStatusLabel1.Text="Listo";
				}));
		}

		public void leer_grid_eba(){
			
			j=0;
			Invoke(new MethodInvoker(delegate{
			panel3.Visible = true;
			progressBar2.Value=0;
			button6.Enabled=false;
			button8.Enabled=false;
			button9.Enabled=false;
			comboBox1.Enabled=false;
			textBox3.Enabled=false;
			}));
			
			for(i=0;i<tot_row;i++){
				
				/*if((dataGridView1.Rows[i].Cells[j].Value.ToString()).Equals("")){
					//campo = "";
				}*/
				Invoke(new MethodInvoker(delegate{
				reg_pat2  = dataGridView1.Rows[i].Cells[0].Value.ToString();
				ra_soc    = dataGridView1.Rows[i].Cells[1].Value.ToString();
				num_cred  = dataGridView1.Rows[i].Cells[2].Value.ToString();
				num_trab  = dataGridView1.Rows[i].Cells[3].Value.ToString();
				sector_n  = dataGridView1.Rows[i].Cells[4].Value.ToString();
				domicilio = dataGridView1.Rows[i].Cells[5].Value.ToString();
				localidad = dataGridView1.Rows[i].Cells[6].Value.ToString();
				imp_tot   = Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value.ToString());
				}));
				
				imp_mul   = Convert.ToDouble(imp_tot*0.40);
				reg_pat1   = reg_pat2.Substring(1,11);
				reg_pat  = reg_pat1.Substring(0,10);
				ra_soc	  = ra_soc.TrimEnd(' ');
				domicilio = domicilio.TrimEnd(' ');
				localidad = localidad.TrimEnd(' ');
				/*ra_soc=ra_soc.Replace('\'',' ');
				domicilio=domicilio.Replace('\'',' ');
				localidad=localidad.Replace('\'',' ');
				*/
				try{
				
				nvatabla="INSERT INTO eba"+periodo+" (reg_pat, reg_pat1, razon_social, num_credito, periodo, num_trabajadores, sector_not, importe, importe_multa, status, domicilio, localidad) VALUES"+
					"(\""+reg_pat+"\",\""+reg_pat1+"\",\""+ra_soc+"\",\""+num_cred.Substring(1,9)+"\",\""+periodo+"\",\""+num_trab+"\",\""+sector_n+"\",\""+imp_tot+"\",\""+imp_mul+"\",\"0\",\""+domicilio+"\",\""+localidad+"\")";
				conex.consultar(nvatabla);
				
				}catch(Exception exp) {
				MessageBox.Show("ERROR: \n"+ exp, "Error al Insertar Datos en la BD");
				Invoke(new MethodInvoker(delegate{
				button6.Enabled=true;
				button8.Enabled=true;
				button9.Enabled=true;
				comboBox1.Enabled=true;
				textBox3.Enabled=true;
				panel3.Visible=false;
				UseWaitCursor=false;
				}));
				}				
				Invoke(new MethodInvoker(delegate{
				progreso();
				}));
			}
			
				MessageBox.Show("Datos Ingresados Correctamente\n","Proceso Exitoso");
				conex.guardar_evento("Se Creo la tabla eba"+periodo+" en la Base de Datos de la Eba y se insertaron "+tot_row+" registros");
				Invoke(new MethodInvoker(delegate{
				button6.Enabled=true;
				button8.Enabled=true;
				button9.Enabled=true;
				comboBox1.Enabled=true;
				textBox3.Enabled=true;
				panel3.Visible=false;
				UseWaitCursor=false;
				MainForm mani = (MainForm)this.MdiParent;
				mani.toolStripStatusLabel1.Text="Listo";
				}));
		}
		
		public void leer_grid_cm(){
			conex.conectar("base_principal");
			j=0;
			error_nul=0;
			Invoke(new MethodInvoker(delegate{
			                         	textBox2.Text="";
			                         	panel3.Visible = true;
			                         	progressBar2.Value=0;
			                         	button6.Enabled=false;
			                         	button8.Enabled=false;
			                         	button9.Enabled=false;
			                         	comboBox1.Enabled=false;
			                         	textBox3.Enabled=false;
			                         }));
			num_cm = Convert.ToInt32(nom_cm_ac.Substring(7,nom_cm_ac.Length-7));
			num_cm = num_cm+1;
			
			if((num_cm < 10)){
				num_per_cm="000"+Convert.ToString(num_cm);
			}else{
				
				if((num_cm > 9)&&(num_cm < 100)){
					num_per_cm="00"+Convert.ToString(num_cm);
				}else{
                    if ((num_cm > 99) && (num_cm < 1000))
                    {
                        num_per_cm = "0" + Convert.ToString(num_cm);
                    }
                    else
                    {
                        if ((num_cm > 999))
                        {
                            num_per_cm = Convert.ToString(num_cm);
                        }
                    }
				}
			}

			if(cm==1){
				nombre_periodo="COP_CM_" + num_per_cm;
			}else{
				if(cm==2){
					nombre_periodo="RCV_CM_" + num_per_cm;
				}
			}
			
			for(i=0;i<tot_row;i++){
				
				if(dataGridView1.Rows[i].Cells[0].Value.ToString() == ""){
					i=tot_row;
				}else{
			
				Invoke(new MethodInvoker(delegate{
				                         	try{
				                         		if(!dataGridView1.Rows[i].Cells[0].Value.ToString().Equals("")){
				                         		reg_pat1  = dataGridView1.Rows[i].Cells[0].Value.ToString();
				                         		}else{
				                         			MessageBox.Show("El campo REGISTRO PATRONAL de la  linea: "+(i+2)+"\ncontinene un valor incorrecto o se encuentra vacío","Error de Integridad de Datos");
				                         			error_nul=1;
				                         		}
				                         		if(!dataGridView1.Rows[i].Cells[1].Value.ToString().Equals("")){
				                         		ra_soc    = dataGridView1.Rows[i].Cells[1].Value.ToString();
				                         		}else{
				                         			MessageBox.Show("El campo RAZÓN SOCIAL de la  linea: "+(i+2)+"\ncontinene un valor incorrecto o se encuentra vacío","Error de Integridad de Datos");
				                         			error_nul=1;
				                         		}
				                         		if(!dataGridView1.Rows[i].Cells[2].Value.ToString().Equals("")){
				                         			periodo   = dataGridView1.Rows[i].Cells[2].Value.ToString();
				                         		}else{
				                         			MessageBox.Show("El campo PERIODO de la  linea: "+(i+2)+"\ncontinene un valor incorrecto o se encuentra vacío","Error de Integridad de Datos");
				                         			error_nul=1;
				                         		}
				                         		if(!dataGridView1.Rows[i].Cells[3].Value.ToString().Equals("")){
				                         			tipo_doc  = dataGridView1.Rows[i].Cells[3].Value.ToString();
				                         		}else{
				                         			MessageBox.Show("El campo TIPO DOCUMENTO de la  linea: "+(i+2)+"\ncontinene un valor incorrecto o se encuentra vacío","Error de Integridad de Datos");
				                         			error_nul=1;
				                         		}
				                         		if(!dataGridView1.Rows[i].Cells[4].Value.ToString().Equals("")){
				                         			cred_cuo  = dataGridView1.Rows[i].Cells[4].Value.ToString();
				                         		}else{
				                         			cred_cuo = "-";
				                         		}
				                         		
				                         		if(!dataGridView1.Rows[i].Cells[5].Value.ToString().Equals("")){
				                         			cred_mul  = dataGridView1.Rows[i].Cells[5].Value.ToString();
				                         		}else{
				                         			cred_mul = "-";
				                         		}
				                         		
				                         		if(!dataGridView1.Rows[i].Cells[6].Value.ToString().Equals("")){
				                         			imp_tot   =	Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value.ToString());
				                         		}else{
				                         			imp_tot = 0.00;
				                         		}
				                         		
				                         		if(!dataGridView1.Rows[i].Cells[7].Value.ToString().Equals("")){
				                         			imp_mul   = Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value.ToString());
				                         		}else{
				                         			imp_mul = 0.00;
				                         		}
				                         		if(!dataGridView1.Rows[i].Cells[8].Value.ToString().Equals("")){
				                         			sector_n  = dataGridView1.Rows[i].Cells[8].Value.ToString();
				                         		}else{
				                         			MessageBox.Show("El campo SECTOR de la  linea: "+(i+2)+"\ncontinene un valor incorrecto o se encuentra vacío","Error de Integridad de Datos");
				                         			error_nul=1;
				                         		}
				                         		if(!dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("")){
				                         		subdele   = dataGridView1.Rows[i].Cells[9].Value.ToString();
				                         		}else{
				                         			MessageBox.Show("El campo SUBDELEGACIÓN de la  linea: "+(i+2)+"\ncontinene un valor incorrecto o se encuentra vacío","Error de Integridad de Datos");
				                         			error_nul=1;
				                         		}
				                         	}catch(FormatException e){
				                         		
				                         	}
				                         }));
				
				if(reg_pat1.Length == 10){
					reg_pat1=reg_pat1 + "0";
				}
				
				reg_pat   = reg_pat1.Substring(0,3)+"-"+reg_pat1.Substring(3,5)+"-"+reg_pat1.Substring(8,2)+"-"+reg_pat1.Substring(10,1);
				reg_pat2  = reg_pat1.Substring(0,10);
				ra_soc	  = ra_soc.TrimEnd(' ');

				Invoke(new MethodInvoker(delegate{
                    textBox2.AppendText("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, credito_multa, importe_cuota, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion) " +
                                    " VALUES (\"" + nombre_periodo + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + ra_soc + "\",\"" + periodo + "\",\"" + tipo_doc + "\",\"" + cred_cuo + "\",\"" + cred_mul + "\",\"" + imp_tot + "\",\"" + imp_mul + "\",\"" + sector_n + "\",\"" + sector_n + "\",\"" + subdele + "\");\n");
				
				                         	progreso();
				                         }));
				}
			}//Fin del For *********
			Invoke(new MethodInvoker(delegate{
			                         	text=textBox2.Lines;
			                         }));
			k=0;
			sql="";
			if(error_nul==0){
				try{
					do{
						sql= text[k];
						conex.consultar(sql);
						k++;
						i++;
						Invoke(new MethodInvoker(delegate{
						                         	progreso();
						                         }));
					}while(text[k] != "");
				}catch(Exception exp) {
					MessageBox.Show("ERROR: \n"+ exp, "Error al Insertar Datos en la BD");
					bandera=1;
					Invoke(new MethodInvoker(delegate{
					                         	button6.Enabled=true;
					                         	button8.Enabled=true;
					                         	button9.Enabled=true;
					                         	comboBox1.Enabled=true;
					                         	textBox3.Enabled=true;
					                         	panel3.Visible=false;
					                         	UseWaitCursor=false;
					                         	radioButton12.Enabled=false;
				                         		radioButton13.Enabled=false;
					                         	textBox2.Text="";
					                         }));
				}
				if(bandera==0){
				MessageBox.Show("Datos Ingresados Correctamente en el Periodo: "+nombre_periodo,"Proceso Exitoso");
				conex.guardar_evento("Se crearon "+k+" registros con el Nombre de Periodo: "+nombre_periodo);
				conex.consultar("INSERT INTO estado_periodos (nombre_periodo) VALUES (\""+nombre_periodo+"\")");
				Invoke(new MethodInvoker(delegate{
				                         	button6.Enabled=true;
				                         	button8.Enabled=true;
				                         	button9.Enabled=true;
				                         	comboBox1.Enabled=true;
				                         	textBox3.Enabled=true;
				                         	panel3.Visible=false;
				                         	UseWaitCursor=false;
				                         	radioButton12.Enabled=false;
				                         	radioButton13.Enabled=false;
				                         	textBox2.Text="";
				                         	MainForm mani = (MainForm)this.MdiParent;
				                         	mani.toolStripStatusLabel1.Text="Listo";
				                         	this.Close();
				                         }));
				}
			}else{
				MessageBox.Show("No se pudieron ingresar los datos,\nrevise el archivo de origen:\n","Proceso Interrumpido");
				Invoke(new MethodInvoker(delegate{
					                         	button6.Enabled=true;
					                         	button8.Enabled=true;
					                         	button9.Enabled=true;
					                         	comboBox1.Enabled=true;
					                         	textBox3.Enabled=true;
					                         	panel3.Visible=false;
					                         	UseWaitCursor=false;
					                         	radioButton12.Enabled=false;
				                         		radioButton13.Enabled=false;
					                         	textBox2.Text="";
					                         	
					                         }));
			}//Fin if-else
			bandera=0;
			i=0;
		}
		
		public void progreso(){
			
			por = (i*100)/totr;
			//label8.Text = (por.ToString("N3")+"%");
			//label8.Refresh();
			por = por - 0.500;
			porcent = Convert.ToInt32(por);
			
			
			if(por>99.80){
				porcent=100;
			}
			
			progressBar2.Value = porcent;
			porcent_text = Convert.ToString(porcent);
			label7.Text = porcent_text+"%";
			label7.Refresh();
			
			j++;
			
			
			if(j==10){
				label6.Text="Procesando.";
				label6.Refresh();
			}
			
			if(j==20){
				label6.Text="Procesando..";
				label6.Refresh();
			}
			
			if(j==30){
				label6.Text="Procesando...";
				label6.Refresh();
			}
			
			if(j==40){
				label6.Text="Procesando";
				label6.Refresh();
				j=0;
			}
			
			MainForm mani = (MainForm)this.MdiParent;
			if(tipocarga == 3){
				if(por<50){
					mani.toolStripStatusLabel1.Text="Leyendo Tabla: "+(i+3)+" Registros Leídos";
				}else{
					if((por>=50)&&(por<100)){
						l++;
						mani.toolStripStatusLabel1.Text="Escribiendo en la Base de Datos: "+l+" Registros Ingresados";
					}else{
						if(porcent >= 100){
							mani.toolStripStatusLabel1.Text="Listo";
						}
					}
					
				}
			}else{
				if((por>=0)&&(por<100)){
					mani.toolStripStatusLabel1.Text="Escribiendo en la Base de Datos: "+i+" Registros Ingresados";
				}else{
					if(porcent >= 100){
						
					}
				}
				
			}
		}
		
		public void progreso_facturas(){
			
			if(nvomini == 0){
			tot2 = tot_lin;
			por = ((tot2*100)/(tot1*2));
			por = por - 0.500;
			porcent = Convert.ToInt32(por);
			}else{
				if(candado == 1){
					//progressBar1.Minimum = porcent;
					porcent2 = porcent;
					candado = 0;
				}
				tot2=k;
				por = ((tot2*100)/(contador_lin*2))+porcent2+1.5;
				por = por - 0.500;
				porcent = Convert.ToInt32(por);
				
			}
			
			if(por>99.90){
				porcent = 100;
			}
			
			progressBar1.Value = porcent;
			porcent_text = Convert.ToString(porcent);
			label1.Text= porcent_text +"%";
			label1.Refresh();
			
			MainForm mani = (MainForm)this.MdiParent;
			
			if(por<50){
					mani.toolStripStatusLabel1.Text="Leyendo Archivo: "+tot_lin+" Líneas leídas";
			}else{
				if((por>=50)&&(por<100)){
					mani.toolStripStatusLabel1.Text="Escribiendo en la Base de Datos: "+k+" Registros Ingresados";
				}else{
					if(porcent >= 100){
					mani.toolStripStatusLabel1.Text="Listo";
					}
				}
				
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			opciones();
			contador_lin=0;
			leer_linea();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			
			if ((radioButton1.Checked)||(radioButton2.Checked)||(radioButton5.Checked)){
				
				if(radioButton1.Checked){
					this.Text="Nova Gear: Lector de Facturas - COP";
				}
				
				if(radioButton2.Checked){
					this.Text="Nova Gear: Lector de Facturas - RCV";
				}
				
				if(radioButton5.Checked){

                    Selector_RT_CLEM sel = new Selector_RT_CLEM();
                    sel.ShowDialog();

                    if (tipo_anual == 1)
                    {
                        this.Text = "Nova Gear: Lector de Facturas - Oficio";
                        tipo_anual = 0;
                    }
                    else
                    {
                        if (tipo_anual == 2)
                        {
                            Lector_CLEM lector = new Lector_CLEM();
                            tipo_anual = 0;                            
                            lector.Show();
                        }

                        this.Hide();
                    }

				}

                panel1.Visible = false;
                this.Height = 400;
                button1.Enabled = true;
                button5.Enabled = false;
			}
			
			if((radioButton3.Checked)||(radioButton4.Checked)||(radioButton6.Checked)){
                panel1.Visible = false;
				panel2.Visible=true;
				this.Height=535;
				
				if(radioButton3.Checked){
					button6.Text="Abrir      \nArchivo \nEMA      ";
					this.button6.Image = global::Nova_Gear.Properties.Resources.database_add;
					label5.Text="Tabla:";
					button8.Text="Cargar \nTabla   ";
					this.button8.Image = global::Nova_Gear.Properties.Resources.table_go;
					this.Text="Nova Gear - Lector de Facturas - EMA";
					label9.Visible=false;
					radioButton12.Visible=false;
					radioButton13.Visible=false;
					label5.Location = new System.Drawing.Point(170, 37);
					comboBox1.Location = new System.Drawing.Point(230, 38);
					label3.Location = new System.Drawing.Point(170, 10);
					textBox3.Location = new System.Drawing.Point(230, 10);				
					label4.Location = new System.Drawing.Point(11, 70);
					label8.Location = new System.Drawing.Point(533, 70);
					button8.Location = new System.Drawing.Point(425,10);
				}
				
				if(radioButton4.Checked){
					button6.Text="Abrir      \nArchivo \nEBA      ";
					this.button6.Image = global::Nova_Gear.Properties.Resources.database_add;
					label5.Text="Tabla:";
					button8.Text="Cargar \nTabla   ";
					this.button8.Image = global::Nova_Gear.Properties.Resources.table_go;
					this.Text="Nova Gear - Lector de Facturas - EBA";
					label9.Visible=false;
					radioButton12.Visible=false;
					radioButton13.Visible=false;
					label5.Location = new System.Drawing.Point(170, 37);
					comboBox1.Location = new System.Drawing.Point(230, 38);
					label3.Location = new System.Drawing.Point(170, 10);
					textBox3.Location = new System.Drawing.Point(230, 10);
					label4.Location = new System.Drawing.Point(11, 70);
					label8.Location = new System.Drawing.Point(533, 70);
					button8.Location = new System.Drawing.Point(425,10);
				}
			
				if(radioButton6.Checked){
					button6.Text="Abrir      \nArchivo \nExcel     ";
					this.button6.Image = global::Nova_Gear.Properties.Resources.excel_imports;
					label5.Text="Hoja:";
					button8.Text="Cargar \nHoja    ";
					this.button8.Image = global::Nova_Gear.Properties.Resources.table_excel;
					this.Text="Nova Gear - Lector de Facturas - Carga Manual";
					label9.Visible=true;
					radioButton12.Visible=true;
					radioButton13.Visible=true;
					label5.Location = new System.Drawing.Point(130, 37);
					comboBox1.Location = new System.Drawing.Point(190, 38);
					label3.Location = new System.Drawing.Point(130, 10);
					textBox3.Location = new System.Drawing.Point(190, 10);
					button8.Location = new System.Drawing.Point(355,10);
					MessageBox.Show("Para Ingresar correctamente el archivo de excel, debe contener las siguientes columnas (Respetando el orden y con encabezado):\n" +
					                "[Registro Patronal] 11 dígitos sin guiones\n"+
					                "[Razón Social] Máximo de 80 dígitos\n"+
					                "[Periodo] en formato AAAAMM\n"+
					                "[Tipo de Documento] dos dígitos\n"+
					                "[Crédito Cuota] 9 dígitos\n"+
					                "[Crédito Multa] 9 dígitos\n"+
					                "[Importe Cuota] Número con decimales\n"+
					                "[Importe Multa] Número con decimales\n"+
					                "[Sector Notificación] Numero sin decimales\n"+
					                "[Subdelegación] Número sin decimales\n","AVISO");
				}
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			panel1.Visible=true;
			this.Height=370;
			this.Refresh();
			eco=0;
			this.Text="Nova Gear - Lector de Facturas";
			textBox1.Text="";
			textBox2.Text="";
			textBox4.Text="";
		    label2.Text="Archivo:  ";

		}
		
		void Button4Click(object sender, EventArgs e)
		{
			
			if(ver==1){
				textBox1.Visible=true;
				textBox1.Show();
				button4.Text="Ocultar Proceso";
				button4.Refresh();
				ver=0;
				this.button4.Image = global::Nova_Gear.Properties.Resources.cog_delete;
				
			}else{
				textBox1.Visible=false;
				textBox1.Hide();
				button4.Text="Mostrar Proceso";
				button4.Refresh();
				ver=1;
				this.button4.Image = global::Nova_Gear.Properties.Resources.cog_add;
			}
						
		}
		
		void Button5Click(object sender, EventArgs e)
		{
            int leer=0;

			peri_a_leer=nombre_per.Substring(0,nombre_per.Length-1)+"_"+peri_a_leer;
			
			DialogResult resu = DialogResult.No;
			if(tipo_file==1||tipo_file==2){
				resu = MessageBox.Show("Esta por ingresar los datos de un "+peri_a_leer+".\nEsto afectará la base de datos.\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			}
			
			if(tipo_file==4){
				resu = MessageBox.Show("Esta por ingresar los datos de "+peri_a_leer+".\nEsto afectará la base de datos.\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			}
			
			if(resu == DialogResult.Yes){
				if(tipo_file == 4){
					conex.conectar("base_principal");
					dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM datos_factura WHERE nombre_periodo LIKE \"OFICIOS_%\" ORDER BY nombre_periodo DESC");
					if(dataGridView2.RowCount-1 > 0){
						nombre_per_oficio=dataGridView2.Rows[0].Cells[0].Value.ToString();
						i=0;
						if(Int32.TryParse(nombre_per_oficio.Substring(nombre_per_oficio.Length-3,3), out i)){
							i = Convert.ToInt32(nombre_per_oficio.Substring(nombre_per_oficio.Length-3,3));
							i++;
							
							if(i<10){
								nombre_per_oficio="OFICIOS_00"+i;
							}else{
								
								if(10<=i && i<100){
									nombre_per_oficio="OFICIOS_0"+i;
									
								}else{
									nombre_per_oficio="OFICIOS_"+i;
								}
							}
						}
						
						
					}else{
						nombre_per_oficio="OFICIOS_000";
					}
					conex.cerrar();
					i=0;
				}
				
				conex.conectar("base_principal");
				dataGridView2.DataSource = conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo =\""+peri_a_leer+"\" ");
                int regs_tot1 = Convert.ToInt32(dataGridView2.Rows[0].Cells[0].FormattedValue.ToString());

                if (regs_tot1 > 0)
                {
                    MessageBox.Show("El Periodo que desea ingresar ya se encuentra en la base de datos.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    leer++;
                }

                dataGridView2.DataSource = conex.consultar("SELECT fecha_impresa_documento FROM estado_periodos WHERE nombre_periodo =\"" + peri_a_leer + "\" ");
                string fecha_docs = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();

                if (fecha_docs.Length > 4)
                {
                    leer++;
                }
                else
                {
                    //conex.consultar("INSERT INTO estado_periodos (nombre_periodo) VALUES (\"" + peri_a_leer + "\")");
                    MessageBox.Show("Tiene que llenar primero los datos completos del periodo a ingresar.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Fechas_periodos fech_pers = new Fechas_periodos();
                    MainForm.nombre_periodo = peri_a_leer;                    
                    fech_pers.Show();
                    fech_pers.llena_fechas_forzosa();
                    this.Hide();
                }

				
				
	            if(leer>1){
					this.Height=480;
					UseWaitCursor=true;
					button1.Enabled=false;
					button3.Enabled=false;
					button5.Enabled=false;
					
					hilosecundario = new Thread(new ThreadStart(inicio));
					hilosecundario.Start();
				}
			}
			//inicio();
			
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			comboBox1.Items.Clear();
			comboBox1.Text="";
			l=0;
			if(radioButton3.Checked){
				tipocarga=1;
				label5.Text="Tabla:";
				button8.Text="Cargar \nTabla   ";
				}
				
			if(radioButton4.Checked){
				tipocarga=2;
				label5.Text="Tabla:";
				button8.Text="Cargar \nTabla   ";
				}
			
			if(radioButton6.Checked){
				tipocarga=3;
				label5.Text="Hoja:";
				button8.Text="Cargar \nHoja    ";
				}
			
			abrir_office();
			
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			panel2.Visible=false;
            panel1.Visible = true;
			textBox3.Text="";
			comboBox1.Items.Clear();
			comboBox1.Text="";
			dataGridView1.DataSource="";
			dataGridView2.DataSource="";
            this.Height=175;
			this.Text="Nova Gear - Lector de Facturas";
               
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			if((comboBox1.SelectedItem)!=  null){
			hoja = comboBox1.SelectedItem.ToString();
			
				conectar_office();
			    radioButton12.Enabled=true;
			    radioButton13.Enabled=true;
			
			}else{
				MessageBox.Show("Selecciona un Tabla", "Error");
			}
		}
		
		void Button9Click(object sender, EventArgs e)
		{
	
			//this.panel2.Visible = false;
			if(tipocarga==1){
				DialogResult resu = MessageBox.Show("Esta por ingresar una nueva EMA.\nEsto afectará la base de datos\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				
				if(resu == DialogResult.Yes){
					UseWaitCursor=true;
					this.panel3.Visible = true;
					hiloterciario = new Thread(new ThreadStart(crear_tabla_ema_eba));
					hiloterciario.Start();
					//crear_tabla_ema();
				}
			}
			
			if(tipocarga==2){
				DialogResult resu = MessageBox.Show("Esta por ingresar una nueva EBA.\nEsto afectará la base de datos\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				
				if(resu == DialogResult.Yes){
					UseWaitCursor=true;
					this.panel3.Visible = true;
					hiloterciario = new Thread(new ThreadStart(crear_tabla_ema_eba));
					hiloterciario.Start();
					//crear_tabla_ema();
				}
			}
			
			if(tipocarga==3){
				DialogResult resu = MessageBox.Show("Esta por ingresar una Carga Manual.\nEsto afectará la base de datos\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				
				if(resu == DialogResult.Yes){
				UseWaitCursor=true;
				this.panel3.Visible = true;
				hiloterciario = new Thread(new ThreadStart(leer_grid_cm));
				hiloterciario.Start();
				}
				
			}
			
		}
			
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			if((radioButton1.Checked)||(radioButton2.Checked)){
				this.Height=380;
				groupBox2.Visible=true;
				button2.Location = new System.Drawing.Point(288, 365);
				this.Refresh();

				
			}else{
				/*this.Height=180;
				this.Refresh();*/
			}
			
			if((radioButton3.Checked)||(radioButton4.Checked)||(radioButton5.Checked)||(radioButton6.Checked)){
				groupBox2.Visible=false;
				this.Height=300;
				button2.Location = new System.Drawing.Point(288, 170);
				/*button2.Location.X=290;
				button2.Location.Y=30;*/
				button2.Refresh();
			}
		
			
		}
		
		void RadioButton7CheckedChanged(object sender, EventArgs e)
		{
			this.Height=480;
			this.Refresh();
			
			if((radioButton7.Checked==true)||(radioButton8.Checked==true)){
				radioButton9.Checked=false;
				radioButton10.Checked=false;
				radioButton11.Checked=false;		
		}
		}
		
		void RadioButton9CheckedChanged(object sender, EventArgs e)
		{
			this.Height=480;
			this.Refresh();
			if((radioButton9.Checked==true)||(radioButton10.Checked==true)||(radioButton11.Checked==true)){
				radioButton7.Checked=false;
				radioButton8.Checked=false;
			
		    }
		}
		
		void Lector_FacLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			this.Height=175;
			
			//this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
			this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;
			this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
			
			radioButton1.Checked=false;
			radioButton2.Checked=false;
			radioButton3.Checked=false;
			radioButton4.Checked=false;
			radioButton5.Checked=false;
			radioButton6.Checked=false;
			radioButton7.Checked=false;
			radioButton8.Checked=false;
			radioButton9.Checked=false;
			radioButton10.Checked=false;
			radioButton11.Checked=false;
			
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            
            no_del_real=Convert.ToInt32(conex.leer_config_sub()[1]);
            no_sub_real=Convert.ToInt32(conex.leer_config_sub()[4]);
            
            if(no_del_real<1 || no_sub_real<1){
            	MessageBox.Show("Ocurrió un problema al momento de leer la información de la subdelegación.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            	this.Close();
            }
            
		}
		
		void RadioButton12CheckedChanged(object sender, EventArgs e)
		{
            int cm_m = 0,i_cm_m=0,ncm=0;
			if(radioButton12.Checked){
				if(dataGridView1.DataSource.Equals(null)){
					MessageBox.Show("Seleciona una Hoja válida");
				}else{
					cm=1;
					conex.conectar("base_principal");
                    sql = "SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo LIKE 'COP_CM_%' GROUP BY nombre_periodo ORDER BY nombre_periodo DESC";
					dataGridView2.DataSource = conex.consultar(sql);
					button9.Enabled=true;

                    for (int ccm = 0; ccm < dataGridView2.RowCount-1;ccm++)
                    {
                        ncm=Convert.ToInt32(dataGridView2[0,ccm].Value.ToString().Substring(7,(dataGridView2[0,ccm].Value.ToString().Length-7)));

                        if(ncm>cm_m){
                            cm_m = ncm;
                            i_cm_m = ccm;
                        }
                    }

					nom_cm_ac=dataGridView2.Rows[i_cm_m].Cells[0].Value.ToString();
				}
			}
			
			if(radioButton13.Checked){
				if(dataGridView1.DataSource.Equals(null)){
					MessageBox.Show("Seleciona una Hoja válida");
				}else{
					cm=2;
					conex.conectar("base_principal");
                    sql = "SELECT nombre_periodo FROM datos_factura WHERE nombre_periodo LIKE \"RCV_CM_%\" GROUP BY nombre_periodo ORDER BY nombre_periodo DESC";
					dataGridView2.DataSource = conex.consultar(sql);
					button9.Enabled=true;

                    for (int ccm = 0; ccm < dataGridView2.RowCount-1; ccm++)
                    {
                        ncm = Convert.ToInt32(dataGridView2[0, ccm].Value.ToString().Substring(7, (dataGridView2[0, ccm].Value.ToString().Length - 7)));

                        if (ncm > cm_m)
                        {
                            cm_m = ncm;
                            i_cm_m = ccm;
                        }
                    }

                    nom_cm_ac = dataGridView2.Rows[i_cm_m].Cells[0].Value.ToString();
				}
			}
		}
		
		public void escritura_bd()
		{
		 String sqlz,fecha1,fecha2,fecha3,cuota1,recarg,imptotal,capsis,td01;
		 //Invoke(new MethodInvoker(delegate{
		 conex.conectar("base_principal");
		 label8.Text="Registros: "+dataGridView1.RowCount;
	     j=61852;
		 do{
		 	
			sqlz="INSERT INTO datos_factura (subdelegacion,incidencia,tipo_documento,nombre_periodo,registro_patronal,registro_patronal1,"+
				"registro_patronal2,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,"+
				"sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,dias_retraso,"+
				"fecha_cartera,capturado_siscob,status,status_credito,folio_sipare_sua,importe_pago,porcentaje_pago,num_pago,fecha_pago,fecha_recepcion_documento,fecha_firma,observaciones) "+
				"VALUES ("+
				"\""+dataGridView1.Rows[j].Cells[1].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[2].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[3].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[4].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[5].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[6].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[7].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[8].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[9].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[10].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[11].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[12].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[13].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[14].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[15].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[16].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[17].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[18].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[19].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[20].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[21].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[22].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[23].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[24].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[25].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[26].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[27].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[28].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[29].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[30].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[31].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[32].FormattedValue.ToString()+"\","+
				"\""+dataGridView1.Rows[j].Cells[33].FormattedValue.ToString()+"\");";
				
			
			conex.consultar(sqlz);
			j++;
			//label8.Text="Registros: "+j;
			//label8.Refresh();
			
			MainForm mani = (MainForm)this.MdiParent;
			mani.toolStripStatusLabel1.Text="Escribiendo en la Base de Datos: "+(j+1)+" Registros añadidos";
			mani.statusStrip1.Refresh();
		 
			}while(dataGridView1.RowCount >j);			
		                       //   }));
		}
		
		public void escritura_bd2(){
	
		 String fecha1,fecha2,fecha3,id1,sqlz;
		 conex.conectar("base_principal");
		 label8.Text="Registros: "+dataGridView1.RowCount;
		 
		 do{
		 	
		 	fecha1 = dataGridView1.Rows[j].Cells[19].FormattedValue.ToString();
		 	fecha2=dataGridView1.Rows[j].Cells[20].FormattedValue.ToString();
		 	fecha3=dataGridView1.Rows[j].Cells[21].FormattedValue.ToString();
		 	
		 	id1=dataGridView1.Rows[j].Cells[0].FormattedValue.ToString();
		 	
		 	if(fecha1.Length<1){
		 		fecha1="-";
		 	}else{
		 		fecha1=dataGridView1.Rows[j].Cells[19].FormattedValue.ToString();
		 		fecha1=(fecha1.Substring(8,2))+"/"+(fecha1.Substring(5,2))+"/"+(fecha1.Substring(0,4));
		 	}
		 	
		 	if(fecha2.Length<1){
		 		fecha2="-";
		 	}else{
		 		fecha2=dataGridView1.Rows[j].Cells[20].FormattedValue.ToString();
		 		fecha2=(fecha2.Substring(8,2))+"/"+(fecha2.Substring(5,2))+"/"+(fecha2.Substring(0,4));
		 	}
		 	
		 	if(fecha3.Length<1){
		 		fecha3="-";
		 	}else{
		 		fecha3=dataGridView1.Rows[j].Cells[21].FormattedValue.ToString();
		 		fecha3=(fecha3.Substring(8,2))+"/"+(fecha3.Substring(5,2))+"/"+(fecha3.Substring(0,4));
		 	}
		 	
		 	
			sqlz="UPDATE datos_factura SET fecha_entrega=\""+fecha1+"\", fecha_recepcion=\""+fecha2+"\", fecha_notificacion =\""+fecha3+"\" WHERE id =\""+id1+"\"";
				
			conex.consultar(sqlz);
			j++;
			label8.Text="Registros: "+j;
			label8.Refresh();
			}while(dataGridView1.RowCount >j);			
		
		}
	
		void Button10Click(object sender, EventArgs e)
		{
			
			escritura_bd();
			
		}
	
	}
}

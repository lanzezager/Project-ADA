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
    public partial class Consulta_Mod40 : Form
    {
        public Consulta_Mod40()
        {
            InitializeComponent();
        }

        //Variables
        String periodo,mes_cad,per_count,per_count1,periodo_txt,linea1,linea2,ciclo,fecha_ini,fecha_fin,rango,valor_buscar,estado_sindo;
        int anio=0,mes=0,anio1=0,mes1=0,j=0,tot1=0,resu_tr=0,tipo_buscar=0;
        
        //Conexion MySQL
        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        DataTable consultamysql = new DataTable();
        DataTable consultamysql2 = new DataTable();
        DataTable dt = new DataTable();

        public bool buscar_valor(String valor_bus, int tipo_busq)
        {
        	this.valor_buscar = valor_bus;
        	this.tipo_buscar = tipo_busq;
        	
	        conex.conectar("base_principal");
        	
        	switch(tipo_busq){
        	
        		case 1: 
        			/*if(!valor_bus.StartsWith("N")){
	                	valor_bus = "N" + valor_bus;
	            	}*/
        			dataGridView1.DataSource=conex.consultar("SELECT periodo_pago,folio_sua,fecha_pago,importe_pago,salario_diario,dias_cotizados,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,observaciones,nombre_trabajador,nss,rfc,curp,registro_patronal,estado_sindo "+
        	                                         "FROM mod40_sua where nss= \""+valor_bus+"\" ORDER BY periodo_pago ASC,fecha_pago");
        			break;
        			
        		case 2:
                    dataGridView1.DataSource = conex.consultar("SELECT periodo_pago,folio_sua,fecha_pago,importe_pago,salario_diario,dias_cotizados,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,observaciones,nombre_trabajador,nss,rfc,curp,registro_patronal ,estado_sindo " +
                                                     "FROM mod40_sua where curp= \"" + valor_bus + "\" ORDER BY periodo_pago ASC,fecha_pago");
        			break;
        			
        		case 3:
                    dataGridView1.DataSource = conex.consultar("SELECT periodo_pago,folio_sua,fecha_pago,importe_pago,salario_diario,dias_cotizados,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,observaciones,nombre_trabajador,nss,rfc,curp,registro_patronal,estado_sindo " +
                                                     "FROM mod40_sua where rfc= \"" + valor_bus + "\" ORDER BY periodo_pago ASC,fecha_pago");
        			break;
        			
        		case 4:
                    //MessageBox.Show("|"+valor_bus.ToUpper()+"|");
                    dataGridView1.DataSource = conex.consultar("SELECT periodo_pago,folio_sua,fecha_pago,importe_pago,salario_diario,dias_cotizados,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,observaciones,nombre_trabajador,nss,rfc,curp,registro_patronal,estado_sindo " +
        			                                         "FROM mod40_sua where nombre_trabajador  = \"" + valor_bus.ToUpper() + "\" ORDER BY periodo_pago ASC,fecha_pago");
        			
        			break;
        	}
        	
        	
        	if(dataGridView1.RowCount>0){
        		
        		textBox1.Text=dataGridView1.Rows[0].Cells[15].FormattedValue.ToString().TrimEnd(' ');
        		textBox2.Text=dataGridView1.Rows[0].Cells[16].FormattedValue.ToString();
        		textBox3.Text=dataGridView1.Rows[0].Cells[17].FormattedValue.ToString();
        		textBox4.Text=dataGridView1.Rows[0].Cells[18].FormattedValue.ToString();
        		textBox5.Text=dataGridView1.Rows[0].Cells[19].FormattedValue.ToString();
        		textBox6.Text=dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
        		textBox7.Text=dataGridView1.Rows[dataGridView1.RowCount-1].Cells[0].FormattedValue.ToString();
                estado_sindo = dataGridView1.Rows[0].Cells[20].Value.ToString();
        		
        		dataGridView1.Columns[15].Visible=false;
        		dataGridView1.Columns[16].Visible=false;
        		dataGridView1.Columns[17].Visible=false;
        		dataGridView1.Columns[18].Visible=false;
        		dataGridView1.Columns[19].Visible=false;
                dataGridView1.Columns[20].Visible = false;
        		
        		dataGridView1.Columns[0].HeaderText="PERIODO";
        		dataGridView1.Columns[1].HeaderText="FOLIO SUA";
        		dataGridView1.Columns[2].HeaderText="FECHA PAGO";
        		dataGridView1.Columns[3].HeaderText="IMPORTE PAGO";
        		//dataGridView1.Columns[3].CellType
                dataGridView1.Columns[4].HeaderText = "SALARIO DIARIO";
                dataGridView1.Columns[5].HeaderText = "DIAS COTIZADOS";
        		dataGridView1.Columns[6].HeaderText="IMP_EYM_PREE";
        		dataGridView1.Columns[7].HeaderText="IMP_INV_VIDA";
        		dataGridView1.Columns[8].HeaderText="IMP_CESA_Y_VEJ";
        		dataGridView1.Columns[9].HeaderText="IMP_EYM_PREE_O";
        		dataGridView1.Columns[10].HeaderText="IMP_INV_VIDA_O";
        		dataGridView1.Columns[11].HeaderText="IMP_CESA_Y_VEJ_O";
        		dataGridView1.Columns[12].HeaderText="PERIODO\nCD SUA";
        		dataGridView1.Columns[13].HeaderText="TIPO DE\nMODALIDAD";
        		dataGridView1.Columns[14].HeaderText="OBSERVACIONES";
        		
        		label7.Text=""+dataGridView1.RowCount;
        		checar_faltantes();
        		
        	 	return true;	
        	}else{
        		MessageBox.Show("Búsqueda sin resultados","AVISO");
        		return false;
        	}
        }
        
        public void checar_faltantes(){
        	int linea2_ciclo=0;
        	j=0;
        	/***-----------------*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*PROCESO*/
        	tot1=dataGridView1.RowCount;
        	
        	if(System.DateTime.Today.Month < 10){
        			ciclo=System.DateTime.Today.Year.ToString()+"0"+System.DateTime.Today.Month.ToString();
        		}else{
        			ciclo=System.DateTime.Today.Year.ToString()+System.DateTime.Today.Month.ToString();
        		}
        	
        	//MessageBox.Show(ciclo);
        	
        	if (tot1 > 1)
        	{
        		
       			periodo = dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();

        		anio = Convert.ToInt32(periodo.Substring(0, 4));
        		mes = Convert.ToInt32(periodo.Substring(4, 2));
        		
        		if(mes < 10){
        			mes_cad = "0" + mes;
        		}else{
        			mes_cad = mes.ToString();
        		}
        		per_count = anio.ToString() + mes_cad;
        		
        		mes1 = mes;
        		anio1 = anio;
        		mes1=mes1+2;
        		
        		if (mes1 > 12)
        		{
        			mes1 = 2;
        			anio1++;
        		}
        		
        		if(mes1 < 10){
        			mes_cad = "0" + mes1;
        		}else{
        			mes_cad = mes1.ToString();
        		}
        		
        		per_count1=anio1.ToString()+mes_cad;
        		periodo_txt=per_count;
        		//MessageBox.Show(per_count+"|"+per_count1);
        		
        		do{//****-----------------------------------------------comparar periodos
        			
        			if((j+1)<tot1){
        				
        				linea1 = dataGridView1.Rows[j].Cells[0].FormattedValue.ToString();
        				linea2 = dataGridView1.Rows[j+1].Cells[0].FormattedValue.ToString();
        				
        			}else{
        				linea1 = dataGridView1.Rows[j].Cells[0].FormattedValue.ToString();
        				linea2 = ciclo;
        				linea2_ciclo=1;
        			}
        			 
        			//checar si va adelantado
        			if(Convert.ToInt32(per_count) > Convert.ToInt32(linea1)){
        				per_count=linea1;
        				
        				mes=Convert.ToInt32(per_count.Substring(4,2));
        				anio=Convert.ToInt32(per_count.Substring(0,4));
        				
        				mes1 = mes;
        				anio1 = anio;
        				
        				mes1=mes1+2;
        				if (mes1 > 12)
        				{
        					mes1 = 2;
        					anio1++;
        				}

        				//MessageBox.Show(linea2 + " l2|" + per_count+" pc| ci "+ciclo+"|pc2 "+per_count1);
        			}

        			if(linea1.Equals(linea2)){//*************************SI son PAR
        				
        				//MessageBox.Show(linea1+"|"+per_count+"|"+per_count1);
        				
        				
        				if(per_count.Equals(linea1)){
        					periodo_txt=linea2;
        					
        				}else{
        					mes=Convert.ToInt32(per_count.Substring(4,2));
        					anio=Convert.ToInt32(per_count.Substring(0,4));
        					
        					if(mes < 10){
        						mes_cad = "0" + mes;
        					}else{
        						mes_cad = mes.ToString();
        					}
        					
        					per_count=anio.ToString()+mes_cad;
        					
        					while(Convert.ToInt32(per_count)<Convert.ToInt32(linea1)){//*****----periodos faltantes
        						
        						listBox1.Items.Add(per_count);
        						
        						mes=mes+2;
        						if (mes > 12)
        						{
        							mes = 2;
        							anio++;
        						}
        						
        						if(mes < 10){
        							mes_cad = "0" + mes;
        						}else{
        							mes_cad = mes.ToString();
        						}
        						
        						per_count=anio.ToString()+mes_cad;
        						
        						mes1 = mes;
        						anio1 = anio;
        						
        						mes1=mes1+2;
        						
        						if (mes1 > 12)
        						{
        							mes1 = 2;
        							anio1++;
        						}
        						
        						if(mes1 < 10){
        							mes_cad = "0" + mes1;
        						}else{
        							mes_cad = mes1.ToString();
        						}
        						
        						per_count1=anio1.ToString()+mes_cad;
        					}
        				}
        				
        				j=j+2;
        				
        			}else{//******************SI SON IMPAR
        				
        				if(linea1.Equals(per_count)&&linea2.Equals(per_count1)){
        					//if(j>0){
        						listBox1.Items.Add(per_count);
        						
        					//}
        					
        				}else{
        					//hueco1
        					if(!linea1.Equals(per_count)){
        						
        						mes=Convert.ToInt32(per_count.Substring(4,2));
        						anio=Convert.ToInt32(per_count.Substring(0,4));
        						
        						listBox1.Items.Add(per_count);
        						
        						while(Convert.ToInt32(per_count)<Convert.ToInt32(linea1)){//*****-------------periodos faltantes
        							
        							mes=mes+2;
        							if (mes > 12)
        							{
        								mes = 2;
        								anio++;
        							}
        							
        							if(mes < 10){
        								mes_cad = "0" + mes;
        							}else{
        								mes_cad = mes.ToString();
        							}
        							per_count = anio.ToString() + mes_cad;
        							
        							listBox1.Items.Add(per_count);//agregar linea
        							
        							mes1 = mes;
        							anio1 = anio;
        							
        							mes1=mes1+2;
        							if (mes1 > 12)
        							{
        								mes1 = 2;
        								anio1++;
        							}
        							if(mes1 < 10){
        								mes_cad = "0" + mes1;
        							}else{
        								mes_cad = mes1.ToString();
        							}
        							
        							per_count1=anio1.ToString()+mes_cad;
        							
        						}
        						
        						listBox1.Items.Add(per_count);
        						
        					}//fin hueco h1
        					
        					
        					if(!linea2.Equals(per_count1)){//inicio hueco h2
        						
        						mes=Convert.ToInt32(per_count.Substring(4,2));
        						anio=Convert.ToInt32(per_count.Substring(0,4));
								
                                if (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
                                {
                                	listBox1.Items.Add(per_count);
                                }
        						
        						mes=mes+2;
        						if (mes > 12)
        						{
        							mes = 2;
        							anio++;
        						}
        						if(mes < 10){
        								mes_cad = "0" + mes;
        							}else{
        								mes_cad = mes.ToString();
        							}
        							per_count = anio.ToString() + mes_cad;
        						//arreglo
        						while(Convert.ToInt32(per_count)< Convert.ToInt32(linea2)){//*****-------------periodos faltantes
        							if(mes < 10){
        								mes_cad = "0" + mes;
        							}else{
        								mes_cad = mes.ToString();
        							}
        							per_count = anio.ToString() + mes_cad;
                                    if (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
                                    {
                                        listBox1.Items.Add(per_count);
                                    }
        							mes=mes+2;
        							if (mes > 12)
        							{
        								mes = 2;
        								anio++;
        							}
        							if(mes < 10){
        								mes_cad = "0" + mes;
        							}else{
        								mes_cad = mes.ToString();
        							}
        							
        							per_count = anio.ToString() + mes_cad;
        							
        							mes1 = mes;
        							anio1 = anio;
        							
        							mes1=mes1+2;
        							if (mes1 > 12)
        							{
        								mes1 = 2;
        								anio1++;
        							}
        							if(mes1 < 10){
        								mes_cad = "0" + mes1;
        							}else{
        								mes_cad = mes1.ToString();
        							}
        							
        							per_count1=anio1.ToString()+mes_cad;
        						}
        						
        						

                                if (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
                                {
                                	//if(j==(tot1-1)){//arreglo
                                    	listBox1.Items.Add(per_count);
                                	//}
                                }
        					}
        					
        				}
        				
        				j++;
        				periodo_txt=linea2;
        			}//****************FIN SINO
        			
        			
        			mes=mes+2;
        			if (mes > 12)
        			{
        				mes = 2;
        				anio++;
        			}
        			
        			if(mes < 10){
        				mes_cad = "0" + mes;
        			}else{
        				mes_cad = mes.ToString();
        			}
        			per_count = anio.ToString() + mes_cad;
        			
        			mes1=mes1+2;
        			if (mes1 > 12)
        			{
        				mes1 = 2;
        				anio1++;
        			}
        			if(mes1 < 10){
        				mes_cad = "0" + mes1;
        			}else{
        				mes_cad = mes1.ToString();
        			}
        			
        			per_count1=anio1.ToString()+mes_cad;
                    //MessageBox.Show(per_count+"|"+ciclo);
        		}while((j<tot1)&&(Convert.ToInt32(per_count)<Convert.ToInt32(ciclo)));//FIN CICLO PROCESO MULTIPLES RESULTADOS****************************

                //MessageBox.Show("l1"+linea1+"|"+linea2 + " l2|" + per_count+" pc| ci "+ciclo);
        		//MessageBox.Show(linea2+"|"+per_count+"|"+ciclo);
        		if(linea2_ciclo==1){
        			linea2=linea1;
        		}
        		linea2_ciclo=0;
        		
        		if(Convert.ToInt32(linea2) < Convert.ToInt32(ciclo)){

                    //checar si va adelantado
                    if (Convert.ToInt32(per_count) > Convert.ToInt32(linea2))
                    {
                        per_count = linea2;

                        mes = Convert.ToInt32(per_count.Substring(4, 2));
                        anio = Convert.ToInt32(per_count.Substring(0, 4));
                    }
                    
                    //MessageBox.Show("despues "+linea2 + " l2|" + per_count + " pc| ci " + ciclo);
                    while(Convert.ToInt32(per_count)<Convert.ToInt32(ciclo))
                    {
                        //*****-------------periodos faltantes
        					mes=mes+2;
        					if (mes > 12)
        					{
        						mes = 2;
        						anio++;
        					}
        					if(mes < 10){
        						mes_cad = "0" + mes;
        					}else{
        						mes_cad = mes.ToString();
        					}
        					
        					per_count = anio.ToString() + mes_cad;

                            listBox1.Items.Add(per_count);
                    }
        		}
        		
        		
        		
        	}else{
        		if(tot1==1){//UN SOLO RESULTADO
        			
        			linea1 = dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
        			
					anio = Convert.ToInt32(linea1.Substring(0, 4));
        			mes = Convert.ToInt32(linea1.Substring(4, 2));
        			
        			if(mes < 10){
        				mes_cad = "0" + mes;
        			}else{
        				mes_cad = mes.ToString();
        			}
        			per_count = anio.ToString() + mes_cad;
        			
        			if(!linea1.Equals(ciclo)){
        				
        				do{//*****-------------periodos faltantes
        					if(mes < 10){
        						mes_cad = "0" + mes;
        					}else{
        						mes_cad = mes.ToString();
        					}
        					per_count = anio.ToString() + mes_cad;
        					
        					listBox1.Items.Add(per_count);
        					
        					mes=mes+2;
        					if (mes > 12)
        					{
        						mes = 2;
        						anio++;
        					}
        					if(mes < 10){
        						mes_cad = "0" + mes;
        					}else{
        						mes_cad = mes.ToString();
        					}
        					
        					per_count = anio.ToString() + mes_cad;

        				}while(Convert.ToInt32(per_count)<=Convert.ToInt32(ciclo));

        			}
        		}
        	}

        }

        public void acomodar_periodos()
        {
            int i = 0, j = 0,k=0;
            String anio_rep,linea_1,linea_2;

            dataGridView2.DataSource = null;
            dataGridView2.Columns.Add("anio", "anio");
            dataGridView2.Columns.Add("ene", "ene");
            dataGridView2.Columns.Add("feb", "feb");
            dataGridView2.Columns.Add("mar", "mar");
            dataGridView2.Columns.Add("abr", "abr");
            dataGridView2.Columns.Add("may", "may");
            dataGridView2.Columns.Add("jun", "jun");
            dataGridView2.Columns.Add("jul", "jul");
            dataGridView2.Columns.Add("ago", "ago");
            dataGridView2.Columns.Add("sep", "sep");
            dataGridView2.Columns.Add("oct", "oct");
            dataGridView2.Columns.Add("nov", "nov");
            dataGridView2.Columns.Add("dic", "dic");

            anio_rep = dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
            dataGridView2.Rows.Add(4);
            dataGridView2.Rows[j].Cells[0].Value = anio_rep.Substring(0, 4);
            dataGridView2.Rows[j + 1].Cells[0].Value = anio_rep.Substring(0, 4);
            dataGridView2.Rows[j + 2].Cells[0].Value = anio_rep.Substring(0, 4);
            dataGridView2.Rows[j + 3].Cells[0].Value = anio_rep.Substring(0, 4);
            k = 1;

            do
            {
                dataGridView2.Rows[j].Cells[k].Value = "-";
                dataGridView2.Rows[j + 1].Cells[k].Value = "-";
                dataGridView2.Rows[j + 2].Cells[k].Value = "-";
                dataGridView2.Rows[j + 3].Cells[k].Value = "-";
                k++;
            } while (k < 13);
            //MessageBox.Show(anio_rep); 

            do{
                if (i + 1 < dataGridView1.RowCount)
                {
                    linea_1 = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                    linea_2 = dataGridView1.Rows[i + 1].Cells[0].FormattedValue.ToString();
                }
                else
                {
                    linea_1 = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                    linea_2 = ciclo;
                }

                if (!anio_rep.Substring(0,4).Equals(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Substring(0,4)))
                {
                    anio_rep = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                    dataGridView2.Rows.Add(4);
                    j = j + 4;

                    dataGridView2.Rows[j].Cells[0].Value = anio_rep.Substring(0, 4);
                    dataGridView2.Rows[j + 1].Cells[0].Value = anio_rep.Substring(0, 4);
                    dataGridView2.Rows[j + 2].Cells[0].Value = anio_rep.Substring(0, 4);
                    dataGridView2.Rows[j + 3].Cells[0].Value = anio_rep.Substring(0, 4);
                    k = 1;
                    do{
                        dataGridView2.Rows[j].Cells[k].Value = "-";
                        dataGridView2.Rows[j + 1].Cells[k].Value = "-";
                        dataGridView2.Rows[j + 2].Cells[k].Value = "-";
                        dataGridView2.Rows[j + 3].Cells[k].Value = "-";
                        k++;
                    }while(k<13);

                }

                anio_rep = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();

                if (linea_1.Equals(linea_2))
                {                	
                		
                		if (anio_rep.Substring(4, 2).Equals("02"))
                		{
                			//linea1
                			dataGridView2.Rows[j].Cells[1].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                			dataGridView2.Rows[j + 1].Cells[1].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                            dataGridView2.Rows[j + 2].Cells[1].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                            dataGridView2.Rows[j + 3].Cells[1].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                			try{
                			//linea2
                				dataGridView2.Rows[j].Cells[2].Value = dataGridView1.Rows[i + 1].Cells[2].FormattedValue.ToString();
                				dataGridView2.Rows[j + 1].Cells[2].Value = dataGridView1.Rows[i + 1].Cells[1].FormattedValue.ToString();
                                dataGridView2.Rows[j + 2].Cells[2].Value = dataGridView1.Rows[i+1].Cells[4].FormattedValue.ToString();
                                dataGridView2.Rows[j + 3].Cells[2].Value = dataGridView1.Rows[i+1].Cells[5].FormattedValue.ToString();
                			}catch{}
                		}

                		if (anio_rep.Substring(4, 2).Equals("04"))
                		{
                			//linea1
                			dataGridView2.Rows[j].Cells[3].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                			dataGridView2.Rows[j + 1].Cells[3].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                            dataGridView2.Rows[j + 2].Cells[3].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                            dataGridView2.Rows[j + 3].Cells[3].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                			try{
	                			//linea2
	                			dataGridView2.Rows[j].Cells[4].Value = dataGridView1.Rows[i + 1].Cells[2].FormattedValue.ToString();
	                			dataGridView2.Rows[j + 1].Cells[4].Value = dataGridView1.Rows[i + 1].Cells[1].FormattedValue.ToString();
                                dataGridView2.Rows[j + 2].Cells[4].Value = dataGridView1.Rows[i + 1].Cells[4].FormattedValue.ToString();
                                dataGridView2.Rows[j + 3].Cells[4].Value = dataGridView1.Rows[i + 1].Cells[5].FormattedValue.ToString();
                			}catch{}
                		}
                		if (anio_rep.Substring(4, 2).Equals("06"))
                		{
                			//linea1
                			dataGridView2.Rows[j].Cells[5].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                			dataGridView2.Rows[j + 1].Cells[5].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                            dataGridView2.Rows[j + 2].Cells[5].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                            dataGridView2.Rows[j + 3].Cells[5].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                			try{
	                			//linea2
	                			dataGridView2.Rows[j].Cells[6].Value = dataGridView1.Rows[i + 1].Cells[2].FormattedValue.ToString();
	                			dataGridView2.Rows[j + 1].Cells[6].Value = dataGridView1.Rows[i + 1].Cells[1].FormattedValue.ToString();
                                dataGridView2.Rows[j + 2].Cells[6].Value = dataGridView1.Rows[i + 1].Cells[4].FormattedValue.ToString();
                                dataGridView2.Rows[j + 3].Cells[6].Value = dataGridView1.Rows[i + 1].Cells[5].FormattedValue.ToString();
                			}catch{}
                		}

                		if (anio_rep.Substring(4, 2).Equals("08"))
                		{
                			//linea1
                			dataGridView2.Rows[j].Cells[7].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                			dataGridView2.Rows[j + 1].Cells[7].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                            dataGridView2.Rows[j + 2].Cells[7].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                            dataGridView2.Rows[j + 3].Cells[7].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                			try{
	                			//linea2
	                			dataGridView2.Rows[j].Cells[8].Value = dataGridView1.Rows[i + 1].Cells[2].FormattedValue.ToString();
	                			dataGridView2.Rows[j + 1].Cells[8].Value = dataGridView1.Rows[i + 1].Cells[1].FormattedValue.ToString();
                                dataGridView2.Rows[j + 2].Cells[8].Value = dataGridView1.Rows[i + 1].Cells[4].FormattedValue.ToString();
                                dataGridView2.Rows[j + 3].Cells[8].Value = dataGridView1.Rows[i + 1].Cells[5].FormattedValue.ToString();
                			}catch{}
                		}

                		if (anio_rep.Substring(4, 2).Equals("10"))
                		{
                			//linea1
                			dataGridView2.Rows[j].Cells[9].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                			dataGridView2.Rows[j + 1].Cells[9].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                            dataGridView2.Rows[j + 2].Cells[9].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                            dataGridView2.Rows[j + 3].Cells[9].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                			try{
	                			//linea2
	                			dataGridView2.Rows[j].Cells[10].Value = dataGridView1.Rows[i + 1].Cells[2].FormattedValue.ToString();
	                			dataGridView2.Rows[j + 1].Cells[10].Value = dataGridView1.Rows[i + 1].Cells[1].FormattedValue.ToString();
                                dataGridView2.Rows[j + 2].Cells[10].Value = dataGridView1.Rows[i + 1].Cells[4].FormattedValue.ToString();
                                dataGridView2.Rows[j + 3].Cells[10].Value = dataGridView1.Rows[i + 1].Cells[5].FormattedValue.ToString();
                			}catch{}
                		}

                		if (anio_rep.Substring(4, 2).Equals("12"))
                		{
                			//linea1
                			dataGridView2.Rows[j].Cells[11].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                			dataGridView2.Rows[j + 1].Cells[11].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                            dataGridView2.Rows[j + 2].Cells[11].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                            dataGridView2.Rows[j + 3].Cells[11].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                			//linea2
                			try
                			{
                				dataGridView2.Rows[j].Cells[12].Value = dataGridView1.Rows[i + 1].Cells[2].FormattedValue.ToString();
                				dataGridView2.Rows[j + 1].Cells[12].Value = dataGridView1.Rows[i + 1].Cells[1].FormattedValue.ToString();
                                dataGridView2.Rows[j + 2].Cells[12].Value = dataGridView1.Rows[i + 1].Cells[4].FormattedValue.ToString();
                                dataGridView2.Rows[j + 3].Cells[12].Value = dataGridView1.Rows[i + 1].Cells[5].FormattedValue.ToString();

                			}
                			catch (Exception ex)
                			{
                			}
                		}
                		
						i = i + 2;                	
                }
                else
                {                    
                    if (i > 0)
                    {
                        dataGridView2.Rows.Add();
                        //linea1
                        dataGridView2.Rows[j].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2))) - 1].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                        dataGridView2.Rows[j + 1].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2))) - 1].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                        dataGridView2.Rows[j + 2].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2))) - 1].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                        dataGridView2.Rows[j + 3].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2))) - 1].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                    }
                    else
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[j].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2)))].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                        dataGridView2.Rows[j + 1].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2)))].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                        dataGridView2.Rows[j + 2].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2)))].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                        dataGridView2.Rows[j + 3].Cells[(Convert.ToInt32(anio_rep.Substring(4, 2)))].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                    }
                    i = i + 1;
                }
            
            }while(i<dataGridView1.RowCount);

            //dataGridView2.Rows.RemoveAt(dataGridView2.RowCount - 1);
           
        }

        public void mandar_info()
        {
            
            String user;
            if (dt.Columns.Count <= 0)
            {

                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                j = 0;

                do
                {
                    try
                    {
                        dt.Rows.Add(dataGridView2.Rows[j].Cells[0].Value.ToString(),
                            dataGridView2.Rows[j].Cells[1].Value.ToString(),
                            dataGridView2.Rows[j].Cells[2].Value.ToString(),
                            dataGridView2.Rows[j].Cells[3].Value.ToString(),
                            dataGridView2.Rows[j].Cells[4].Value.ToString(),
                            dataGridView2.Rows[j].Cells[5].Value.ToString(),
                            dataGridView2.Rows[j].Cells[6].Value.ToString(),
                            dataGridView2.Rows[j].Cells[7].Value.ToString(),
                            dataGridView2.Rows[j].Cells[8].Value.ToString(),
                            dataGridView2.Rows[j].Cells[9].Value.ToString(),
                            dataGridView2.Rows[j].Cells[10].Value.ToString(),
                            dataGridView2.Rows[j].Cells[11].Value.ToString(),
                            dataGridView2.Rows[j].Cells[12].Value.ToString());
                    }catch(Exception ex){
                    }
                    j++;
                } while (j < dataGridView2.RowCount);
            }

            //MessageBox.Show(dt.Rows.Count.ToString());

            fecha_ini = dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
            fecha_fin = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].FormattedValue.ToString();
            user = MainForm.datos_user_static[4] +" "+ MainForm.datos_user_static[9];
            //MessageBox.Show(user);

            Visor_oficios40 vis_mod40 = new Visor_oficios40();
            vis_mod40.recibir_tabla(dt, textBox1.Text, textBox2.Text, textBox5.Text, fecha_ini, fecha_fin, conex.leer_config_sub()[3].ToString(), resu_tr, user);
            vis_mod40.Show();
            //dt = null;
        }
        
        public void reparar_periodos(){
        	//ARREGLAR PERIODOS DOBLES
        	String concat_nss,concat_per,sql,anterior;
        	int k = 0,tot_list=0, cont_caja=0,res_repe=0,compara=0;
        	DataTable caja_lista = new DataTable();
        	DataTable consulta_completa= new DataTable();
        	listBox3.Visible=false;
        	conex2.conectar("base_principal");
        	sql="SELECT fecha_pago,periodo_pago FROM base_principal.mod40_sua WHERE nss=\""+textBox2.Text+"\" order by fecha_pago ASC";
        	consulta_completa=conex2.consultar(sql);
        	DataView vista = new DataView(consulta_completa);
        	
        	//MessageBox.Show(consulta_completa.Columns[1].ColumnName);
        	
        	if(listBox1.Items.Count>0){
        		
        		
        		concat_nss = "-";
        		concat_per = "";
        		dataGridView3.DataSource=null;
        		dataGridView3.Columns.Clear();
        		anterior="";
        		caja_lista.Columns.Add();
        		do
        		{
        			cont_caja=0;
        			do{
        				if(caja_lista.Rows.Count>0){
        					//MessageBox.Show(caja_lista.Rows[cont_caja][0].ToString());
        					if(listBox1.Items[k].ToString().Equals(caja_lista.Rows[cont_caja][0].ToString())){
        						//MessageBox.Show(caja_lista.Rows[cont_caja][0].ToString());
        						res_repe=1;
        					}
        				}
        				cont_caja++;
        			}while(cont_caja < caja_lista.Rows.Count);
        			
        			if(res_repe==0){
        				if(!dataGridView1.Rows[0].Cells[0].FormattedValue.ToString().Equals(listBox1.Items[k].ToString())){
        					//sql="SELECT fecha_pago FROM base_principal.mod40_sua WHERE nss=\""+textBox2.Text+"\" and periodo_pago=\""+listBox1.Items[k].ToString()+"\"";
        					//dataGridView3.DataSource=conex2.consultar(sql);
        					//MessageBox.Show("periodo=" + sql + " resu=" + dataGridView3.RowCount);
        					dataGridView3.DataSource=null;
        					vista.RowFilter="periodo_pago ='"+listBox1.Items[k].ToString()+"'";
        					vista.Sort="fecha_pago ASC";
        					dataGridView3.DataSource=vista;
        					//MessageBox.Show("fecha_pago="+dataGridView3.Rows[0].Cells[0].FormattedValue.ToString()+"|periodo_pago='"+listBox1.Items[k].ToString()+"', |"+dataGridView3.RowCount+"|");
        					if (dataGridView3.RowCount==0)
        					{
        						listBox2.Items.Add((Convert.ToInt32(listBox1.Items[k].ToString()) - 1));
        						listBox2.Items.Add(listBox1.Items[k].ToString());
        						listBox2.Refresh();
        					}
        					else
        					{
        						if (dataGridView3.RowCount == 1)
        						{
        							if (dataGridView3.Rows[0].Cells[0].FormattedValue.ToString().Length > 6)
        							{
        								//periodo_pago
        								String periodo_bd = dataGridView3.Rows[0].Cells[0].FormattedValue.ToString().Substring(6,4) + dataGridView3.Rows[0].Cells[0].FormattedValue.ToString().Substring(3, 2);
        								concat_per=listBox1.Items[k].ToString();
        								//MessageBox.Show("fecha: "+periodo_bd+" periodo: "+concat_per);
        								
        								if (!periodo_bd.Equals(concat_per))
        								{
        									//MessageBox.Show(periodo_bd+"|"+listBox1.Items[k].ToString());
        									
        									if (periodo_bd.Equals((Convert.ToInt32(concat_per)-1).ToString()))
        									{
        										listBox2.Items.Add(concat_per);
        										
        									}else{
        										//MessageBox.Show(periodo_bd+"|"+listBox1.Items[k].ToString());
        										listBox2.Items.Add((Convert.ToInt32(concat_per)-1));
        										listBox2.Refresh();
        									}
        								}
        								else
        								{
        									//MessageBox.Show(periodo_bd+"|"+listBox1.Items[k].ToString()+" per: "+concat_per);
        									listBox2.Items.Add((Convert.ToInt32(concat_per)-1));
        									listBox2.Refresh();
        									
        								}
        								
        								
        							}else{
        								listBox2.Items.Add((Convert.ToInt32(listBox1.Items[k].ToString()) - 1));
        								listBox2.Items.Add(listBox1.Items[k].ToString());
        								listBox2.Refresh();
        							}
        						}else{
        							
        							if (dataGridView1.RowCount >= 2)
        							{

        							}
        						}
        					}
        				}
        				caja_lista.Rows.Add(listBox1.Items[k].ToString());
        			}
        			
        			res_repe=0;
        			
        			k++;
        		} while(k < listBox1.Items.Count);
        		
        		listBox1.Items.Clear();
        		tot_list=0;
        		if(Convert.ToInt32(System.DateTime.Today.Month.ToString())<10){
        			compara=Convert.ToInt32(System.DateTime.Today.Year.ToString()+"0"+System.DateTime.Today.Month.ToString());
        		}else{
        			compara=Convert.ToInt32(System.DateTime.Today.Year.ToString()+System.DateTime.Today.Month.ToString());
        		}
        		
        		while(tot_list < listBox2.Items.Count){
        			//MessageBox.Show(""+compara+"|"+listBox2.Items[tot_list].ToString());
        			if((Convert.ToInt32(listBox2.Items[tot_list].ToString()))<=compara){
	        			listBox1.Items.Add(listBox2.Items[tot_list].ToString());
        			}
	        			tot_list++;
        		}
        		//MessageBox.Show(listBox2.Items.Count+"|");
        		listBox2.Items.Clear();
        		listBox1.Refresh();
        		label11.Text="("+listBox1.Items.Count+")";
        		listBox3.Visible=false;
        	}
        	//FIN PERIODOS DOBLES
        }
        
        public void ultimo_cd_faltantes(){
        	String cd,cd_actual,mes_cad_cd,cd_actual_bd;
        	int k=0,anio_cd=0,mes_cd=0;
        	
        	toolStripDropDownButton1.DropDownItems.Clear();
        	
        	dataGridView2.DataSource=conex2.consultar("SELECT DISTINCT(per_cd_sua) FROM mod40_sua WHERE per_cd_sua <> \"-\" ORDER BY per_cd_sua ASC");
        	
        	cd=dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
        	anio_cd=Convert.ToInt32(cd.Substring(0,4));
        	mes_cd=Convert.ToInt32(cd.Substring(4,2));
        	
        	mes_cd=mes_cd+1;
        	
        	if (mes_cd > 12)
        	{
        	   mes_cd = 1;
        	   anio_cd++;
        	}
		            
		    if(mes_cd < 10){
        	 	mes_cad_cd = "0" + mes_cd.ToString();
        	}else{
        		mes_cad_cd = mes_cd.ToString();
        	}
        	
        	cd_actual=anio_cd.ToString()+mes_cad_cd;
        	k=1;
        	
        	while(k < dataGridView2.RowCount){
        		cd_actual_bd = dataGridView2.Rows[k].Cells[0].FormattedValue.ToString();
        		
        		if(cd_actual_bd==cd_actual){
        			
        			anio_cd=Convert.ToInt32(cd_actual.Substring(0,4));
        			mes_cd=Convert.ToInt32(cd_actual.Substring(4,2));
        			
        			mes_cd=mes_cd+1;
        			
        			if (mes_cd > 12)
        			{
        				mes_cd = 1;
        				anio_cd++;
        			}
        			
        			if(mes_cd < 10){
        				mes_cad_cd = "0" + mes_cd.ToString();
        			}else{
        				mes_cad_cd = mes_cd.ToString();
        			}
        			
        			cd_actual=anio_cd.ToString()+mes_cad_cd;
        		}else{
        			
        			while(cd_actual_bd!=cd_actual){
        				
        			toolStripDropDownButton1.DropDownItems.Add(cd_actual);	
        			
        			anio_cd=Convert.ToInt32(cd_actual.Substring(0,4));
        			mes_cd=Convert.ToInt32(cd_actual.Substring(4,2));
        			
        			mes_cd=mes_cd+1;
        			
        			if (mes_cd > 12)
        			{
        				mes_cd = 1;
        				anio_cd++;
        			}
        			
        			if(mes_cd < 10){
        				mes_cad_cd = "0" + mes_cd.ToString();
        			}else{
        				mes_cad_cd = mes_cd.ToString();
        			}
        			
        			cd_actual=anio_cd.ToString()+mes_cad_cd;
        			
        			}
        			
        			anio_cd=Convert.ToInt32(cd_actual.Substring(0,4));
        			mes_cd=Convert.ToInt32(cd_actual.Substring(4,2));
        			
        			mes_cd=mes_cd+1;
        			
        			if (mes_cd > 12)
        			{
        				mes_cd = 1;
        				anio_cd++;
        			}
        			
        			if(mes_cd < 10){
        				mes_cad_cd = "0" + mes_cd.ToString();
        			}else{
        				mes_cad_cd = mes_cd.ToString();
        			}
        			
        			cd_actual=anio_cd.ToString()+mes_cad_cd;
        		}
        		
        		k++;
        	}
        	
        toolStripStatusLabel2.Text=dataGridView2.Rows[k-1].Cells[0].FormattedValue.ToString();
        	
        }
        
        public String pasar_list(){
        
        	int c=0;
        	String items_list="";
        	
        	while(c<listBox1.Items.Count){
        		items_list+=listBox1.Items[c].ToString()+",";
        		c++;
        	}
        	items_list=items_list.TrimEnd(',');
        	return items_list;
        }

        public void consulta_sindo()
        {
            conex3.conectar("base_principal");
            consultamysql2 = conex3.consultar("SELECT * FROM mod40_sindo WHERE nss=\""+textBox2.Text+"\" ORDER BY fecha_consulta DESC");
            dataGridView4.DataSource = consultamysql2;
        }

        private void Consulta_Mod40_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            rango = MainForm.datos_user_static[2];
        	if(Convert.ToInt32(rango)==0||Convert.ToInt32(rango)==11){
        		button2.Enabled=true;
        		button3.Enabled=true;
        		button4.Enabled=true;
        		
        	}else{
        		button2.Visible=false;
        		button3.Visible=false;
        		button4.Visible=false;
        	}
        	
        	
        	//MessageBox.Show(rango);
			reparar_periodos();
			
			ultimo_cd_faltantes();
            consulta_sindo();

            if(estado_sindo=="2"){
                label14.ForeColor = System.Drawing.Color.Red;
                label14.Text = "BAJA";
            }else{
                label14.ForeColor = System.Drawing.Color.Lime;
                label14.Text = "VIGENTE";
            }
            
        }
        
        void TextBox1TextChanged(object sender, EventArgs e)
        {
        	
        }

        private void button1_Click(object sender, EventArgs e)
        {  
            
            Tipo_reporte tr = new Tipo_reporte();
            tr.ShowDialog(this);
            resu_tr = tr.mandar();

            if (resu_tr < 1)
            {
                //MessageBox.Show("Tiene que seleccionar el modo de captura de acuerdo a la sección en la que se va a efectuar la captura automática en SISCOB");
            }
            else
            {
                acomodar_periodos();
                mandar_info();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
          
        }
        
        void Label3Click(object sender, EventArgs e)
        {
        	
        }
        
        void Button2Click(object sender, EventArgs e)
        {
        	if(listBox1.SelectedIndex < 0){
        		listBox1.SelectedIndex=0;
        	}
        	
        	Insertar_40 nvo_pgo = new Insertar_40();
        	nvo_pgo.regresar(textBox2.Text,"40",listBox1.SelectedItem.ToString());
        	nvo_pgo.ShowDialog();
        	listBox1.Items.Clear();
        	listBox2.Items.Clear();
        	buscar_valor(valor_buscar,tipo_buscar);
        	reparar_periodos();
        }
         
        void Button4Click(object sender, EventArgs e)
        {
        	if(listBox1.SelectedIndex < 0){
        		listBox1.SelectedIndex=0;
        	}
        	
        	Insertar_40 nvo_pgo = new Insertar_40();
        	nvo_pgo.regresar(textBox2.Text,"10",listBox1.SelectedItem.ToString());
        	nvo_pgo.ShowDialog();
        	listBox1.Items.Clear();
        	listBox2.Items.Clear();
        	buscar_valor(valor_buscar,tipo_buscar);
        	reparar_periodos();
        }
        
        void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        	
        }
        
        void ToolStripStatusLabel4Click(object sender, EventArgs e)
        {
        	
        }
        
        void Button3Click(object sender, EventArgs e)
        {
        	Asig_Pag_m40 asig = new Asig_Pag_m40(textBox2.Text);
        	asig.ShowDialog();
        	listBox1.Items.Clear();
        	listBox2.Items.Clear();
        	buscar_valor(valor_buscar,tipo_buscar);
        	reparar_periodos();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

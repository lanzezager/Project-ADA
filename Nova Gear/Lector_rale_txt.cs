/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 13/11/2017
 * Hora: 05:36 p.m.
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
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Lector_rale_txt.
	/// </summary>
	public partial class Lector_rale_txt : Form
	{
		public Lector_rale_txt(int tipo_ini)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.ini_tipo=tipo_ini;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int ini_tipo,tot_credits=0;
		String ruta,tipo_rale="",fecha_rale="";
		
		Conexion conex = new Conexion();
		DataTable tabla_ral = new DataTable();
		DataTable rale = new DataTable();
			
		
		//Declaracion de Hilo
		private Thread hilosecundario = null;
		
		//leer rale
		public void leer_rale(){
			String linea,reg,reg_anterior="",mov,mov_anterior="",fech_mov,fech_mov_anterior="",sector,sector_anterior="",credito,ce,periodo,td,fech_alta,fech_not,inc,fech_inc,dias,importe,dc,sc;
			int i=0,j=0,activar=0,k=0,z=0,tot_lei=0,l=0;
			
			StreamReader t3 = new StreamReader(ruta);
			while(t3.EndOfStream != true){
				//while(j<101){
				linea=t3.ReadLine();
				//MessageBox.Show(linea);
				if(linea.Contains("COORDINACION DE COBRANZA")==true){
					while(i<10){
						linea=t3.ReadLine();
						if(linea.Contains("R.A.L.E.")==true && (k==0)){
							if(linea.Contains("R.C.V.")==true){
								tipo_rale="RCV";
								fecha_rale=linea.Substring(88,10);
							}else{
								tipo_rale="COP";
								fecha_rale=linea.Substring(80,10);
							}
							k=1;
							
						}
						i++;
					}
					i=0;
					activar=0;
				}
				
				if(linea.Contains("REG.")==true){
					linea=t3.ReadLine();
					activar=1;
					//MessageBox.Show(linea);
				}
				
				if(activar==1){
					//MessageBox.Show(linea);
					reg=linea.Substring(8,12);
					mov=linea.Substring(21,1);
					fech_mov=linea.Substring(23,10);
					sector=linea.Substring(35,2);
					credito=linea.Substring(40,9);
					ce= linea.Substring(50,1);
					periodo=linea.Substring(54,7);
					td=linea.Substring(62,2);
					fech_alta=linea.Substring(66,10);
					fech_not=linea.Substring(78,10);
					inc=linea.Substring(90,3);
					fech_inc=linea.Substring(95,10);
					dias=linea.Substring(107,4);
					importe=linea.Substring(111,15);
					dc=linea.Substring(127,2);
					sc=linea.Substring(130,2);
					
					if(credito.TrimStart(' ').Length>8){
						if(reg.TrimStart(' ').Length>9){
							reg_anterior=reg;
							mov_anterior=mov;
							fech_mov_anterior=fech_mov;
							sector_anterior=sector;
						}else{
							reg=reg_anterior;
							mov=mov_anterior;
							fech_mov=fech_mov_anterior;
							sector=sector_anterior;
						}
						
						if(fech_mov.TrimEnd(' ').Length>9){
							fech_mov=fech_mov.Substring(6,4)+"-"+fech_mov.Substring(3,2)+"-"+fech_mov.Substring(0,2);
						}else{
							fech_mov="0001-01-01";
						}
						
						if(fech_alta.TrimEnd(' ').Length>9){
							fech_alta=fech_alta.Substring(6,4)+"-"+fech_alta.Substring(3,2)+"-"+fech_alta.Substring(0,2);
						}else{
							fech_alta="0001-01-01";
						}
						
						if(fech_not.TrimEnd(' ').Length>9){
							fech_not=fech_not.Substring(6,4)+"-"+fech_not.Substring(3,2)+"-"+fech_not.Substring(0,2);
						}else{
							fech_not="0001-01-01";
						}
						
						if(fech_inc.TrimEnd(' ').Length>9){
							fech_inc=fech_inc.Substring(6,4)+"-"+fech_inc.Substring(3,2)+"-"+fech_inc.Substring(0,2);
						}else{
							fech_inc="0001-01-01";
						}
						
						periodo=periodo.Substring(3,4)+periodo.Substring(0,2);
						importe=importe.TrimStart(' ');
						inc=inc.TrimStart('0');
						if(inc.Length==0){
							inc="0";
						}
						
						ce=ce.TrimStart(' ');
						if(ce.Length==0){
							ce="-";
						}
						
						if(sector.Equals("00")==true){
							sector="0";
						}
						
						dias=dias.TrimStart('0');
						if(dias.Length==0){
							dias="0";
						}
						mov=mov.TrimStart(' ');
						if(mov.Length==0){
							mov="0";	
						}
						
						while(importe.Contains(",")==true){
							l=importe.IndexOf(',');
							importe=importe.Remove(l,1);
						}
						
						if(fech_inc.Contains("#")==true){
							fech_inc="0001-01-01";
						}
						
						
						Invoke(new MethodInvoker(delegate{
						                         	dataGridView1.Rows.Add(reg,mov,fech_mov,sector,credito,ce,periodo,td,fech_alta,fech_not,inc,fech_inc,dias,importe,dc,sc);
						                         	z=z+1;
						                         	
						                         	tot_lei=Convert.ToInt32((z*100)/tot_credits);
						                         	label4.Text=tot_lei+"%";
						                         	if(tot_lei<101){
						                         		progressBar1.Value=tot_lei;
						                         	}
						                         }));


					}
				}
				
				j++;
			}
			
			Invoke(new MethodInvoker(delegate{
			textBox1.Text=tipo_rale;
			textBox2.Text=fecha_rale;
			label3.Text="Registros Totales: "+dataGridView1.RowCount;
			
			if(dataGridView1.RowCount>0){
				button2.Enabled=true;
			}
			
			panel1.Visible=false;
			activar_controles();
			progressBar1.Value=100;		
			                         }));
		}
		//rale EMISIONES
		public void guardar_rale_principal(){
			
			String reg="",mov="",fech_mov="",sector="",credito="",ce="",periodo="",td="",fech_alta="",fech_not="",inc="",fech_inc="",dias="",importe="",dc,sc;
			int i=0,tot_dgv1=0,j=0,k=0,tot_guar=0,total=0;
			
			conex.conectar("base_principal");
			if(tipo_rale.Equals("COP")){
				tabla_ral=conex.consultar("SELECT count(idrale) from rale where tipo_rale=\"COP\"");
				if(Convert.ToInt32(tabla_ral.Rows[0][0].ToString())> 0){
					conex.consultar("DELETE FROM base_principal.rale WHERE tipo_rale=\"COP\"");
				}
			}else{
				if(tipo_rale.Equals("RCV")){
					tabla_ral=conex.consultar("SELECT count(idrale) from rale where tipo_rale=\"RCV\"");
					if(Convert.ToInt32(tabla_ral.Rows[0][0].ToString()) > 0){
						conex.consultar("DELETE FROM base_principal.rale WHERE tipo_rale=\"RCV\"");
					}
				}
			}
			
			Invoke(new MethodInvoker(delegate{
				tot_dgv1= dataGridView1.RowCount;
				progressBar1.Value=0;
				label4.Text="0%";
				label5.Text="Guardando...";
				bloquear_controles();
				panel1.Visible=true;
			                         }));
			
			rale.Rows.Clear();

			while(i<tot_dgv1){
				Invoke(new MethodInvoker(delegate{
				                         	reg=dataGridView1.Rows[i].Cells[0].Value.ToString();
				                         	mov=dataGridView1.Rows[i].Cells[1].Value.ToString();
				                         	fech_mov=dataGridView1.Rows[i].Cells[2].Value.ToString();
				                         	sector=dataGridView1.Rows[i].Cells[3].Value.ToString();
				                         	credito=dataGridView1.Rows[i].Cells[4].Value.ToString();
				                         	ce=dataGridView1.Rows[i].Cells[5].Value.ToString();
				                         	periodo=dataGridView1.Rows[i].Cells[6].Value.ToString();
				                         	td=dataGridView1.Rows[i].Cells[7].Value.ToString();
				                         	fech_alta=dataGridView1.Rows[i].Cells[8].Value.ToString();
				                         	fech_not=dataGridView1.Rows[i].Cells[9].Value.ToString();
				                         	inc=dataGridView1.Rows[i].Cells[10].Value.ToString();
				                         	fech_inc=dataGridView1.Rows[i].Cells[11].Value.ToString();
				                         	dias=dataGridView1.Rows[i].Cells[12].Value.ToString();
				                         	importe=dataGridView1.Rows[i].Cells[13].Value.ToString();
				                         	reg=reg.Substring(0,3)+reg.Substring(4,5)+reg.Substring(10,2);
				                         	
				                         	tot_guar=Convert.ToInt32((i*100)/tot_dgv1);
				                         	label4.Text=tot_guar+"%";
				                         	label4.Refresh();
				                         	if(tot_guar<101){
						                       progressBar1.Value=tot_guar;
						                    }
				                         }));
				
				if(tipo_rale.Equals("COP")==true){
                    /*if ((inc.Equals("20") == true) || (inc.Equals("21") == true) || (inc.Equals("22") == true) || (inc.Equals("23") == true) || (inc.Equals("25") == true))
                    {
					}else{
						if((td.Equals("00")==true)||(td.Equals("02")==true)||(td.Equals("03")==true)||(td.Equals("70")==true)||(td.Equals("80")==true)||(td.Equals("81")==true)||(td.Equals("82")==true)||(td.Equals("89")==true)){*/
							rale.Rows.Add(reg,mov,fech_mov,sector,credito,ce,periodo,td,fech_alta,fech_not,inc,fech_inc,dias,importe);
							/*j++;
						}else{
						}
					}*/
				}else{
					if(tipo_rale.Equals("RCV")==true){
                       /* if ((inc.Equals("20") == true) || (inc.Equals("21") == true) || (inc.Equals("22") == true) || (inc.Equals("23") == true) || (inc.Equals("25") == true))
                        {
						}else{
							if((td.Equals("03")==true)||(td.Equals("06")==true)){*/
								rale.Rows.Add(reg,mov,fech_mov,sector,credito,ce,periodo,td,fech_alta,fech_not,inc,fech_inc,dias,importe);
							/*	j++;
							}else{
							}
						}*/
					}
				}
				
				if(j==10){
					
					conex.consultar("INSERT INTO rale (registro_patronal,mov,fecha_mov,sector,credito,ce,periodo,td,fecha_alta,fecha_noti,incidencia,fecha_incidencia,dias,importe,tipo_rale) VALUES " +
					                "(\""+rale.Rows[0][0].ToString()+"\","+rale.Rows[0][1].ToString()+",\""+rale.Rows[0][2].ToString()+"\","+rale.Rows[0][3].ToString()+",\""+rale.Rows[0][4].ToString()+"\",\""+rale.Rows[0][5].ToString()+"\",\""+rale.Rows[0][6].ToString()+"\",\""+rale.Rows[0][7].ToString()+"\",\""+rale.Rows[0][8].ToString()+"\",\""+rale.Rows[0][9].ToString()+"\",\""+rale.Rows[0][10].ToString()+"\",\""+rale.Rows[0][11].ToString()+"\","+rale.Rows[0][12].ToString()+","+rale.Rows[0][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[1][0].ToString()+"\","+rale.Rows[1][1].ToString()+",\""+rale.Rows[1][2].ToString()+"\","+rale.Rows[1][3].ToString()+",\""+rale.Rows[1][4].ToString()+"\",\""+rale.Rows[1][5].ToString()+"\",\""+rale.Rows[1][6].ToString()+"\",\""+rale.Rows[1][7].ToString()+"\",\""+rale.Rows[1][8].ToString()+"\",\""+rale.Rows[1][9].ToString()+"\",\""+rale.Rows[1][10].ToString()+"\",\""+rale.Rows[1][11].ToString()+"\","+rale.Rows[1][12].ToString()+","+rale.Rows[1][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[2][0].ToString()+"\","+rale.Rows[2][1].ToString()+",\""+rale.Rows[2][2].ToString()+"\","+rale.Rows[2][3].ToString()+",\""+rale.Rows[2][4].ToString()+"\",\""+rale.Rows[2][5].ToString()+"\",\""+rale.Rows[2][6].ToString()+"\",\""+rale.Rows[2][7].ToString()+"\",\""+rale.Rows[2][8].ToString()+"\",\""+rale.Rows[2][9].ToString()+"\",\""+rale.Rows[2][10].ToString()+"\",\""+rale.Rows[2][11].ToString()+"\","+rale.Rows[2][12].ToString()+","+rale.Rows[2][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[3][0].ToString()+"\","+rale.Rows[3][1].ToString()+",\""+rale.Rows[3][2].ToString()+"\","+rale.Rows[3][3].ToString()+",\""+rale.Rows[3][4].ToString()+"\",\""+rale.Rows[3][5].ToString()+"\",\""+rale.Rows[3][6].ToString()+"\",\""+rale.Rows[3][7].ToString()+"\",\""+rale.Rows[3][8].ToString()+"\",\""+rale.Rows[3][9].ToString()+"\",\""+rale.Rows[3][10].ToString()+"\",\""+rale.Rows[3][11].ToString()+"\","+rale.Rows[3][12].ToString()+","+rale.Rows[3][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[4][0].ToString()+"\","+rale.Rows[4][1].ToString()+",\""+rale.Rows[4][2].ToString()+"\","+rale.Rows[4][3].ToString()+",\""+rale.Rows[4][4].ToString()+"\",\""+rale.Rows[4][5].ToString()+"\",\""+rale.Rows[4][6].ToString()+"\",\""+rale.Rows[4][7].ToString()+"\",\""+rale.Rows[4][8].ToString()+"\",\""+rale.Rows[4][9].ToString()+"\",\""+rale.Rows[4][10].ToString()+"\",\""+rale.Rows[4][11].ToString()+"\","+rale.Rows[4][12].ToString()+","+rale.Rows[4][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[5][0].ToString()+"\","+rale.Rows[5][1].ToString()+",\""+rale.Rows[5][2].ToString()+"\","+rale.Rows[5][3].ToString()+",\""+rale.Rows[5][4].ToString()+"\",\""+rale.Rows[5][5].ToString()+"\",\""+rale.Rows[5][6].ToString()+"\",\""+rale.Rows[5][7].ToString()+"\",\""+rale.Rows[5][8].ToString()+"\",\""+rale.Rows[5][9].ToString()+"\",\""+rale.Rows[5][10].ToString()+"\",\""+rale.Rows[5][11].ToString()+"\","+rale.Rows[5][12].ToString()+","+rale.Rows[5][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[6][0].ToString()+"\","+rale.Rows[6][1].ToString()+",\""+rale.Rows[6][2].ToString()+"\","+rale.Rows[6][3].ToString()+",\""+rale.Rows[6][4].ToString()+"\",\""+rale.Rows[6][5].ToString()+"\",\""+rale.Rows[6][6].ToString()+"\",\""+rale.Rows[6][7].ToString()+"\",\""+rale.Rows[6][8].ToString()+"\",\""+rale.Rows[6][9].ToString()+"\",\""+rale.Rows[6][10].ToString()+"\",\""+rale.Rows[6][11].ToString()+"\","+rale.Rows[6][12].ToString()+","+rale.Rows[6][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[7][0].ToString()+"\","+rale.Rows[7][1].ToString()+",\""+rale.Rows[7][2].ToString()+"\","+rale.Rows[7][3].ToString()+",\""+rale.Rows[7][4].ToString()+"\",\""+rale.Rows[7][5].ToString()+"\",\""+rale.Rows[7][6].ToString()+"\",\""+rale.Rows[7][7].ToString()+"\",\""+rale.Rows[7][8].ToString()+"\",\""+rale.Rows[7][9].ToString()+"\",\""+rale.Rows[7][10].ToString()+"\",\""+rale.Rows[7][11].ToString()+"\","+rale.Rows[7][12].ToString()+","+rale.Rows[7][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[8][0].ToString()+"\","+rale.Rows[8][1].ToString()+",\""+rale.Rows[8][2].ToString()+"\","+rale.Rows[8][3].ToString()+",\""+rale.Rows[8][4].ToString()+"\",\""+rale.Rows[8][5].ToString()+"\",\""+rale.Rows[8][6].ToString()+"\",\""+rale.Rows[8][7].ToString()+"\",\""+rale.Rows[8][8].ToString()+"\",\""+rale.Rows[8][9].ToString()+"\",\""+rale.Rows[8][10].ToString()+"\",\""+rale.Rows[8][11].ToString()+"\","+rale.Rows[8][12].ToString()+","+rale.Rows[8][13].ToString()+",\""+tipo_rale+"\"),"+
					                "(\""+rale.Rows[9][0].ToString()+"\","+rale.Rows[9][1].ToString()+",\""+rale.Rows[9][2].ToString()+"\","+rale.Rows[9][3].ToString()+",\""+rale.Rows[9][4].ToString()+"\",\""+rale.Rows[9][5].ToString()+"\",\""+rale.Rows[9][6].ToString()+"\",\""+rale.Rows[9][7].ToString()+"\",\""+rale.Rows[9][8].ToString()+"\",\""+rale.Rows[9][9].ToString()+"\",\""+rale.Rows[9][10].ToString()+"\",\""+rale.Rows[9][11].ToString()+"\","+rale.Rows[9][12].ToString()+","+rale.Rows[9][13].ToString()+",\""+tipo_rale+"\")");
					
					rale.Rows.Clear();
					total=total+10;
					j=0;
				}
				i++;
				
			}
			
			//MessageBox.Show(""+rale.Rows.Count);
			k=0;
			while(k<rale.Rows.Count){
				conex.consultar("INSERT INTO rale (registro_patronal,mov,fecha_mov,sector,credito,ce,periodo,td,fecha_alta,fecha_noti,incidencia,fecha_incidencia,dias,importe,tipo_rale) VALUES " +
				                "(\""+rale.Rows[k][0].ToString()+"\","+rale.Rows[k][1].ToString()+",\""+rale.Rows[k][2].ToString()+"\","+rale.Rows[k][3].ToString()+",\""+rale.Rows[k][4].ToString()+"\",\""+rale.Rows[k][5].ToString()+"\",\""+rale.Rows[k][6].ToString()+"\",\""+rale.Rows[k][7].ToString()+"\",\""+rale.Rows[k][8].ToString()+"\",\""+rale.Rows[k][9].ToString()+"\",\""+rale.Rows[k][10].ToString()+"\",\""+rale.Rows[k][11].ToString()+"\","+rale.Rows[k][12].ToString()+","+rale.Rows[k][13].ToString()+",\""+tipo_rale+"\")");
				k++;
				total++;
			}
			
			Invoke(new MethodInvoker(delegate{
				tot_dgv1= dataGridView1.RowCount;
				progressBar1.Value=100;
				label4.Text="100%";
				label5.Text="Guardando...";
				activar_controles();
				panel1.Visible=false;
			                         }));
			conex.guardar_evento("Se Ingreso el RALE_"+tipo_rale.ToUpper()+" de "+total+" casos, creado el dia: "+fecha_rale );
			MessageBox.Show("Se Ingreso el RALE "+tipo_rale.ToUpper()+" de "+total+" casos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
		}
		
		public void guardar_rale_inventario(){
			String reg="",mov="",fech_mov="",sector="",credito="",ce="",periodo_anio="",periodo_mes="",td="",fech_alta="",fech_not="",inc="",fech_inc="",dias="",importe="",dc,sc,fecha_rale="";
			int i=0,tot_dgv1=0,j=0,k=0,tot_guar=0,total=0;
			
			conex.conectar("inventario");
			if(rale.Columns.Count<15){
				rale.Columns.Add("columna15",tipo_rale.GetType());
			}
			if(tipo_rale.Equals("COP")){
				tabla_ral=conex.consultar("SELECT count(idbase_inventario) from base_inventario where clase_doc=\"COP\"");
				if(Convert.ToInt32(tabla_ral.Rows[0][0].ToString())> 0){
					conex.consultar("DELETE FROM base_inventario WHERE clase_doc=\"COP\"");
				}
			}else{
				if(tipo_rale.Equals("RCV")){
					tabla_ral=conex.consultar("SELECT count(idbase_inventario) from base_inventario where clase_doc=\"RCV\"");
					if(Convert.ToInt32(tabla_ral.Rows[0][0].ToString()) > 0){
						conex.consultar("DELETE FROM base_inventario WHERE clase_doc=\"RCV\"");
					}
				}
			}
			
			Invoke(new MethodInvoker(delegate{
				tot_dgv1= dataGridView1.RowCount;
				progressBar1.Value=0;
				label4.Text="0%";
				label5.Text="Guardando...";
				bloquear_controles();
				panel1.Visible=true;
			                         }));
			
			rale.Rows.Clear();

			while(i<tot_dgv1){
				Invoke(new MethodInvoker(delegate{
				                         	reg=dataGridView1.Rows[i].Cells[0].Value.ToString();
				                         	mov=dataGridView1.Rows[i].Cells[1].Value.ToString();
				                         	fech_mov=dataGridView1.Rows[i].Cells[2].Value.ToString();
				                         	sector=dataGridView1.Rows[i].Cells[3].Value.ToString();
				                         	credito=dataGridView1.Rows[i].Cells[4].Value.ToString();
				                         	ce=dataGridView1.Rows[i].Cells[5].Value.ToString();
				                         	periodo_anio=dataGridView1.Rows[i].Cells[6].Value.ToString().Substring(0,4);
				                         	periodo_mes=dataGridView1.Rows[i].Cells[6].Value.ToString().Substring(4,2);
				                         	td=dataGridView1.Rows[i].Cells[7].Value.ToString();
				                         	fech_alta=dataGridView1.Rows[i].Cells[8].Value.ToString();
				                         	fech_not=dataGridView1.Rows[i].Cells[9].Value.ToString();
				                         	inc=dataGridView1.Rows[i].Cells[10].Value.ToString();
				                         	fech_inc=dataGridView1.Rows[i].Cells[11].Value.ToString();
				                         	dias=dataGridView1.Rows[i].Cells[12].Value.ToString();
				                         	importe=dataGridView1.Rows[i].Cells[13].Value.ToString();
				                         	
				                         	tot_guar=Convert.ToInt32((i*100)/tot_dgv1);
				                         	label4.Text=tot_guar+"%";
				                         	label4.Refresh();
				                         	if(tot_guar<101){
						                       progressBar1.Value=tot_guar;
						                    }
				                         }));
				
				//if((inc.Equals("0")==true)){
				//}else{
					rale.Rows.Add(reg,mov,fech_mov,sector,credito,ce,periodo_anio,td,fech_alta,fech_not,inc,fech_inc,dias,importe,periodo_mes);
					j++;
				//}
				
				if(j==10){
					conex.consultar("INSERT INTO base_inventario (reg_pat,mov,fecha_mov,sector,credito,ce,periodo_anio,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,periodo_mes) VALUES " +
					                "(\""+rale.Rows[0][0].ToString()+"\","+rale.Rows[0][1].ToString()+",\""+rale.Rows[0][2].ToString()+"\","+rale.Rows[0][3].ToString()+",\""+rale.Rows[0][4].ToString()+"\",\""+rale.Rows[0][5].ToString()+"\",\""+rale.Rows[0][6].ToString()+"\",\""+rale.Rows[0][7].ToString()+"\",\""+rale.Rows[0][8].ToString()+"\",\""+rale.Rows[0][9].ToString()+"\",\""+rale.Rows[0][10].ToString()+"\",\""+rale.Rows[0][11].ToString()+"\","+rale.Rows[0][12].ToString()+","+rale.Rows[0][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[0][14].ToString()+"\"),"+
					                "(\""+rale.Rows[1][0].ToString()+"\","+rale.Rows[1][1].ToString()+",\""+rale.Rows[1][2].ToString()+"\","+rale.Rows[1][3].ToString()+",\""+rale.Rows[1][4].ToString()+"\",\""+rale.Rows[1][5].ToString()+"\",\""+rale.Rows[1][6].ToString()+"\",\""+rale.Rows[1][7].ToString()+"\",\""+rale.Rows[1][8].ToString()+"\",\""+rale.Rows[1][9].ToString()+"\",\""+rale.Rows[1][10].ToString()+"\",\""+rale.Rows[1][11].ToString()+"\","+rale.Rows[1][12].ToString()+","+rale.Rows[1][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[1][14].ToString()+"\"),"+
					                "(\""+rale.Rows[2][0].ToString()+"\","+rale.Rows[2][1].ToString()+",\""+rale.Rows[2][2].ToString()+"\","+rale.Rows[2][3].ToString()+",\""+rale.Rows[2][4].ToString()+"\",\""+rale.Rows[2][5].ToString()+"\",\""+rale.Rows[2][6].ToString()+"\",\""+rale.Rows[2][7].ToString()+"\",\""+rale.Rows[2][8].ToString()+"\",\""+rale.Rows[2][9].ToString()+"\",\""+rale.Rows[2][10].ToString()+"\",\""+rale.Rows[2][11].ToString()+"\","+rale.Rows[2][12].ToString()+","+rale.Rows[2][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[2][14].ToString()+"\"),"+
					                "(\""+rale.Rows[3][0].ToString()+"\","+rale.Rows[3][1].ToString()+",\""+rale.Rows[3][2].ToString()+"\","+rale.Rows[3][3].ToString()+",\""+rale.Rows[3][4].ToString()+"\",\""+rale.Rows[3][5].ToString()+"\",\""+rale.Rows[3][6].ToString()+"\",\""+rale.Rows[3][7].ToString()+"\",\""+rale.Rows[3][8].ToString()+"\",\""+rale.Rows[3][9].ToString()+"\",\""+rale.Rows[3][10].ToString()+"\",\""+rale.Rows[3][11].ToString()+"\","+rale.Rows[3][12].ToString()+","+rale.Rows[3][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[3][14].ToString()+"\"),"+
					                "(\""+rale.Rows[4][0].ToString()+"\","+rale.Rows[4][1].ToString()+",\""+rale.Rows[4][2].ToString()+"\","+rale.Rows[4][3].ToString()+",\""+rale.Rows[4][4].ToString()+"\",\""+rale.Rows[4][5].ToString()+"\",\""+rale.Rows[4][6].ToString()+"\",\""+rale.Rows[4][7].ToString()+"\",\""+rale.Rows[4][8].ToString()+"\",\""+rale.Rows[4][9].ToString()+"\",\""+rale.Rows[4][10].ToString()+"\",\""+rale.Rows[4][11].ToString()+"\","+rale.Rows[4][12].ToString()+","+rale.Rows[4][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[4][14].ToString()+"\"),"+
					                "(\""+rale.Rows[5][0].ToString()+"\","+rale.Rows[5][1].ToString()+",\""+rale.Rows[5][2].ToString()+"\","+rale.Rows[5][3].ToString()+",\""+rale.Rows[5][4].ToString()+"\",\""+rale.Rows[5][5].ToString()+"\",\""+rale.Rows[5][6].ToString()+"\",\""+rale.Rows[5][7].ToString()+"\",\""+rale.Rows[5][8].ToString()+"\",\""+rale.Rows[5][9].ToString()+"\",\""+rale.Rows[5][10].ToString()+"\",\""+rale.Rows[5][11].ToString()+"\","+rale.Rows[5][12].ToString()+","+rale.Rows[5][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[5][14].ToString()+"\"),"+
					                "(\""+rale.Rows[6][0].ToString()+"\","+rale.Rows[6][1].ToString()+",\""+rale.Rows[6][2].ToString()+"\","+rale.Rows[6][3].ToString()+",\""+rale.Rows[6][4].ToString()+"\",\""+rale.Rows[6][5].ToString()+"\",\""+rale.Rows[6][6].ToString()+"\",\""+rale.Rows[6][7].ToString()+"\",\""+rale.Rows[6][8].ToString()+"\",\""+rale.Rows[6][9].ToString()+"\",\""+rale.Rows[6][10].ToString()+"\",\""+rale.Rows[6][11].ToString()+"\","+rale.Rows[6][12].ToString()+","+rale.Rows[6][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[6][14].ToString()+"\"),"+
					                "(\""+rale.Rows[7][0].ToString()+"\","+rale.Rows[7][1].ToString()+",\""+rale.Rows[7][2].ToString()+"\","+rale.Rows[7][3].ToString()+",\""+rale.Rows[7][4].ToString()+"\",\""+rale.Rows[7][5].ToString()+"\",\""+rale.Rows[7][6].ToString()+"\",\""+rale.Rows[7][7].ToString()+"\",\""+rale.Rows[7][8].ToString()+"\",\""+rale.Rows[7][9].ToString()+"\",\""+rale.Rows[7][10].ToString()+"\",\""+rale.Rows[7][11].ToString()+"\","+rale.Rows[7][12].ToString()+","+rale.Rows[7][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[7][14].ToString()+"\"),"+
					                "(\""+rale.Rows[8][0].ToString()+"\","+rale.Rows[8][1].ToString()+",\""+rale.Rows[8][2].ToString()+"\","+rale.Rows[8][3].ToString()+",\""+rale.Rows[8][4].ToString()+"\",\""+rale.Rows[8][5].ToString()+"\",\""+rale.Rows[8][6].ToString()+"\",\""+rale.Rows[8][7].ToString()+"\",\""+rale.Rows[8][8].ToString()+"\",\""+rale.Rows[8][9].ToString()+"\",\""+rale.Rows[8][10].ToString()+"\",\""+rale.Rows[8][11].ToString()+"\","+rale.Rows[8][12].ToString()+","+rale.Rows[8][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[8][14].ToString()+"\"),"+
					                "(\""+rale.Rows[9][0].ToString()+"\","+rale.Rows[9][1].ToString()+",\""+rale.Rows[9][2].ToString()+"\","+rale.Rows[9][3].ToString()+",\""+rale.Rows[9][4].ToString()+"\",\""+rale.Rows[9][5].ToString()+"\",\""+rale.Rows[9][6].ToString()+"\",\""+rale.Rows[9][7].ToString()+"\",\""+rale.Rows[9][8].ToString()+"\",\""+rale.Rows[9][9].ToString()+"\",\""+rale.Rows[9][10].ToString()+"\",\""+rale.Rows[9][11].ToString()+"\","+rale.Rows[9][12].ToString()+","+rale.Rows[9][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[9][14].ToString()+"\")");
					
					rale.Rows.Clear();
					total=total+10;
					j=0;
				}
				i++;
				
			}
			
			//MessageBox.Show(""+rale.Rows.Count);
			k=0;
			while(k<rale.Rows.Count){
				conex.consultar("INSERT INTO base_inventario (reg_pat,mov,fecha_mov,sector,credito,ce,periodo_anio,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,periodo_mes) VALUES " +
				                "(\""+rale.Rows[k][0].ToString()+"\","+rale.Rows[k][1].ToString()+",\""+rale.Rows[k][2].ToString()+"\","+rale.Rows[k][3].ToString()+",\""+rale.Rows[k][4].ToString()+"\",\""+rale.Rows[k][5].ToString()+"\",\""+rale.Rows[k][6].ToString()+"\",\""+rale.Rows[k][7].ToString()+"\",\""+rale.Rows[k][8].ToString()+"\",\""+rale.Rows[k][9].ToString()+"\",\""+rale.Rows[k][10].ToString()+"\",\""+rale.Rows[k][11].ToString()+"\","+rale.Rows[k][12].ToString()+","+rale.Rows[k][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[k][14].ToString()+"\")");
				k++;
				total++;
			}
			
			Invoke(new MethodInvoker(delegate{
				tot_dgv1= dataGridView1.RowCount;
				progressBar1.Value=100;
				label4.Text="100%";
				label5.Text="Guardando...";
				activar_controles();
				panel1.Visible=false;
			                         }));
			conex.guardar_evento("Se Ingreso el RALE_"+tipo_rale.ToUpper()+" de "+total+" casos, creado el dia: "+fecha_rale );
			MessageBox.Show("Se Ingreso el RALE "+tipo_rale.ToUpper()+" de "+total+" casos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
		}
		
		public void guardar_rale_cartera_inventario(){
			String reg="",mov="",fech_mov="",sector="",credito="",ce="",periodo_anio="",periodo_mes="",td="",fech_alta="",fech_not="",inc="",fech_inc="",dias="",importe="",dc,sc,fecha_rale="";
			int i=0,tot_dgv1=0,j=0,k=0,tot_guar=0,total=0;
			
			conex.conectar("cartera_inv");
			conex.consultar("TRUNCATE cartera_temp");
			
			Invoke(new MethodInvoker(delegate{
				tot_dgv1= dataGridView1.RowCount;
				progressBar1.Value=0;
				label4.Text="0%";
				label5.Text="Guardando...";
				bloquear_controles();
				panel1.Visible=true;
			                         }));
			
			rale.Rows.Clear();

			while(i<tot_dgv1){
				Invoke(new MethodInvoker(delegate{
				                         	reg=dataGridView1.Rows[i].Cells[0].Value.ToString();
				                         	mov=dataGridView1.Rows[i].Cells[1].Value.ToString();
				                         	fech_mov=dataGridView1.Rows[i].Cells[2].Value.ToString();
				                         	sector=dataGridView1.Rows[i].Cells[3].Value.ToString();
				                         	credito=dataGridView1.Rows[i].Cells[4].Value.ToString();
				                         	ce=dataGridView1.Rows[i].Cells[5].Value.ToString();
				                         	periodo_anio=dataGridView1.Rows[i].Cells[6].Value.ToString();
				                         	//periodo_mes=dataGridView1.Rows[i].Cells[6].Value.ToString().Substring(4,2);
				                         	td=dataGridView1.Rows[i].Cells[7].Value.ToString();
				                         	fech_alta=dataGridView1.Rows[i].Cells[8].Value.ToString();
				                         	fech_not=dataGridView1.Rows[i].Cells[9].Value.ToString();
				                         	inc=dataGridView1.Rows[i].Cells[10].Value.ToString();
				                         	fech_inc=dataGridView1.Rows[i].Cells[11].Value.ToString();
				                         	dias=dataGridView1.Rows[i].Cells[12].Value.ToString();
				                         	importe=dataGridView1.Rows[i].Cells[13].Value.ToString();
				                         	
				                         	while(importe.Contains(",")==true){
				                         		importe=importe.Remove(importe.IndexOf(','),1);
				                         	}
				                         	
				                         	tot_guar=Convert.ToInt32((i*100)/tot_dgv1);
				                         	label4.Text=tot_guar+"%";
				                         	label4.Refresh();
				                         	if(tot_guar<101){
						                       progressBar1.Value=tot_guar;
						                    }
				                         }));
				
				//if((inc.Equals("0")==true)){
				//}else{
					rale.Rows.Add(reg,mov,fech_mov,sector,credito,ce,periodo_anio,td,fech_alta,fech_not,inc,fech_inc,dias,importe);
					j++;
				//}
				
				if(j==10){
					conex.consultar("INSERT INTO cartera_temp (reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,llave) VALUES " +
					                "(\""+rale.Rows[0][0].ToString()+"\","+rale.Rows[0][1].ToString()+",\""+rale.Rows[0][2].ToString()+"\","+rale.Rows[0][3].ToString()+",\""+rale.Rows[0][4].ToString()+"\",\""+rale.Rows[0][5].ToString()+"\",\""+rale.Rows[0][6].ToString()+"\",\""+rale.Rows[0][7].ToString()+"\",\""+rale.Rows[0][8].ToString()+"\",\""+rale.Rows[0][9].ToString()+"\",\""+rale.Rows[0][10].ToString()+"\",\""+rale.Rows[0][11].ToString()+"\","+rale.Rows[0][12].ToString()+","+rale.Rows[0][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[0][0].ToString()+rale.Rows[0][4].ToString()+rale.Rows[0][6].ToString()+"\"),"+
					                "(\""+rale.Rows[1][0].ToString()+"\","+rale.Rows[1][1].ToString()+",\""+rale.Rows[1][2].ToString()+"\","+rale.Rows[1][3].ToString()+",\""+rale.Rows[1][4].ToString()+"\",\""+rale.Rows[1][5].ToString()+"\",\""+rale.Rows[1][6].ToString()+"\",\""+rale.Rows[1][7].ToString()+"\",\""+rale.Rows[1][8].ToString()+"\",\""+rale.Rows[1][9].ToString()+"\",\""+rale.Rows[1][10].ToString()+"\",\""+rale.Rows[1][11].ToString()+"\","+rale.Rows[1][12].ToString()+","+rale.Rows[1][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[1][0].ToString()+rale.Rows[1][4].ToString()+rale.Rows[1][6].ToString()+"\"),"+
					                "(\""+rale.Rows[2][0].ToString()+"\","+rale.Rows[2][1].ToString()+",\""+rale.Rows[2][2].ToString()+"\","+rale.Rows[2][3].ToString()+",\""+rale.Rows[2][4].ToString()+"\",\""+rale.Rows[2][5].ToString()+"\",\""+rale.Rows[2][6].ToString()+"\",\""+rale.Rows[2][7].ToString()+"\",\""+rale.Rows[2][8].ToString()+"\",\""+rale.Rows[2][9].ToString()+"\",\""+rale.Rows[2][10].ToString()+"\",\""+rale.Rows[2][11].ToString()+"\","+rale.Rows[2][12].ToString()+","+rale.Rows[2][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[2][0].ToString()+rale.Rows[2][4].ToString()+rale.Rows[2][6].ToString()+"\"),"+
					                "(\""+rale.Rows[3][0].ToString()+"\","+rale.Rows[3][1].ToString()+",\""+rale.Rows[3][2].ToString()+"\","+rale.Rows[3][3].ToString()+",\""+rale.Rows[3][4].ToString()+"\",\""+rale.Rows[3][5].ToString()+"\",\""+rale.Rows[3][6].ToString()+"\",\""+rale.Rows[3][7].ToString()+"\",\""+rale.Rows[3][8].ToString()+"\",\""+rale.Rows[3][9].ToString()+"\",\""+rale.Rows[3][10].ToString()+"\",\""+rale.Rows[3][11].ToString()+"\","+rale.Rows[3][12].ToString()+","+rale.Rows[3][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[3][0].ToString()+rale.Rows[3][4].ToString()+rale.Rows[3][6].ToString()+"\"),"+
					                "(\""+rale.Rows[4][0].ToString()+"\","+rale.Rows[4][1].ToString()+",\""+rale.Rows[4][2].ToString()+"\","+rale.Rows[4][3].ToString()+",\""+rale.Rows[4][4].ToString()+"\",\""+rale.Rows[4][5].ToString()+"\",\""+rale.Rows[4][6].ToString()+"\",\""+rale.Rows[4][7].ToString()+"\",\""+rale.Rows[4][8].ToString()+"\",\""+rale.Rows[4][9].ToString()+"\",\""+rale.Rows[4][10].ToString()+"\",\""+rale.Rows[4][11].ToString()+"\","+rale.Rows[4][12].ToString()+","+rale.Rows[4][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[4][0].ToString()+rale.Rows[4][4].ToString()+rale.Rows[4][6].ToString()+"\"),"+
					                "(\""+rale.Rows[5][0].ToString()+"\","+rale.Rows[5][1].ToString()+",\""+rale.Rows[5][2].ToString()+"\","+rale.Rows[5][3].ToString()+",\""+rale.Rows[5][4].ToString()+"\",\""+rale.Rows[5][5].ToString()+"\",\""+rale.Rows[5][6].ToString()+"\",\""+rale.Rows[5][7].ToString()+"\",\""+rale.Rows[5][8].ToString()+"\",\""+rale.Rows[5][9].ToString()+"\",\""+rale.Rows[5][10].ToString()+"\",\""+rale.Rows[5][11].ToString()+"\","+rale.Rows[5][12].ToString()+","+rale.Rows[5][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[5][0].ToString()+rale.Rows[5][4].ToString()+rale.Rows[5][6].ToString()+"\"),"+
					                "(\""+rale.Rows[6][0].ToString()+"\","+rale.Rows[6][1].ToString()+",\""+rale.Rows[6][2].ToString()+"\","+rale.Rows[6][3].ToString()+",\""+rale.Rows[6][4].ToString()+"\",\""+rale.Rows[6][5].ToString()+"\",\""+rale.Rows[6][6].ToString()+"\",\""+rale.Rows[6][7].ToString()+"\",\""+rale.Rows[6][8].ToString()+"\",\""+rale.Rows[6][9].ToString()+"\",\""+rale.Rows[6][10].ToString()+"\",\""+rale.Rows[6][11].ToString()+"\","+rale.Rows[6][12].ToString()+","+rale.Rows[6][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[6][0].ToString()+rale.Rows[6][4].ToString()+rale.Rows[6][6].ToString()+"\"),"+
					                "(\""+rale.Rows[7][0].ToString()+"\","+rale.Rows[7][1].ToString()+",\""+rale.Rows[7][2].ToString()+"\","+rale.Rows[7][3].ToString()+",\""+rale.Rows[7][4].ToString()+"\",\""+rale.Rows[7][5].ToString()+"\",\""+rale.Rows[7][6].ToString()+"\",\""+rale.Rows[7][7].ToString()+"\",\""+rale.Rows[7][8].ToString()+"\",\""+rale.Rows[7][9].ToString()+"\",\""+rale.Rows[7][10].ToString()+"\",\""+rale.Rows[7][11].ToString()+"\","+rale.Rows[7][12].ToString()+","+rale.Rows[7][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[7][0].ToString()+rale.Rows[7][4].ToString()+rale.Rows[7][6].ToString()+"\"),"+
					                "(\""+rale.Rows[8][0].ToString()+"\","+rale.Rows[8][1].ToString()+",\""+rale.Rows[8][2].ToString()+"\","+rale.Rows[8][3].ToString()+",\""+rale.Rows[8][4].ToString()+"\",\""+rale.Rows[8][5].ToString()+"\",\""+rale.Rows[8][6].ToString()+"\",\""+rale.Rows[8][7].ToString()+"\",\""+rale.Rows[8][8].ToString()+"\",\""+rale.Rows[8][9].ToString()+"\",\""+rale.Rows[8][10].ToString()+"\",\""+rale.Rows[8][11].ToString()+"\","+rale.Rows[8][12].ToString()+","+rale.Rows[8][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[8][0].ToString()+rale.Rows[8][4].ToString()+rale.Rows[8][6].ToString()+"\"),"+
					                "(\""+rale.Rows[9][0].ToString()+"\","+rale.Rows[9][1].ToString()+",\""+rale.Rows[9][2].ToString()+"\","+rale.Rows[9][3].ToString()+",\""+rale.Rows[9][4].ToString()+"\",\""+rale.Rows[9][5].ToString()+"\",\""+rale.Rows[9][6].ToString()+"\",\""+rale.Rows[9][7].ToString()+"\",\""+rale.Rows[9][8].ToString()+"\",\""+rale.Rows[9][9].ToString()+"\",\""+rale.Rows[9][10].ToString()+"\",\""+rale.Rows[9][11].ToString()+"\","+rale.Rows[9][12].ToString()+","+rale.Rows[9][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[9][0].ToString()+rale.Rows[9][4].ToString()+rale.Rows[9][6].ToString()+"\")");
					
					rale.Rows.Clear();
					total=total+10;
					j=0;
				}
				i++;
				
			}
			
			//MessageBox.Show(""+rale.Rows.Count);
			k=0;
			while(k<rale.Rows.Count){
				conex.consultar("INSERT INTO cartera_temp (reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,llave) VALUES " +
				                "(\""+rale.Rows[k][0].ToString()+"\","+rale.Rows[k][1].ToString()+",\""+rale.Rows[k][2].ToString()+"\","+rale.Rows[k][3].ToString()+",\""+rale.Rows[k][4].ToString()+"\",\""+rale.Rows[k][5].ToString()+"\",\""+rale.Rows[k][6].ToString()+"\",\""+rale.Rows[k][7].ToString()+"\",\""+rale.Rows[k][8].ToString()+"\",\""+rale.Rows[k][9].ToString()+"\",\""+rale.Rows[k][10].ToString()+"\",\""+rale.Rows[k][11].ToString()+"\","+rale.Rows[k][12].ToString()+","+rale.Rows[k][13].ToString()+",\""+tipo_rale+"\",\""+rale.Rows[k][0].ToString()+rale.Rows[k][4].ToString()+rale.Rows[k][6].ToString()+"\")");
				k++;
				total++;
			}
			
			conex.consultar("UPDATE cartera_temp SET fecha_rale =\""+textBox2.Text.Substring(6,4)+"-"+textBox2.Text.Substring(3,2)+"-"+textBox2.Text.Substring(0,2)+"\", status=\"VIGENTE\" WHERE idcartera_temp > 0 ");
			
			Invoke(new MethodInvoker(delegate{
				tot_dgv1= dataGridView1.RowCount;
				progressBar1.Value=100;
				label4.Text="100%";
				label5.Text="Guardando...";
				activar_controles();
				panel1.Visible=false;
			                         }));
			conex.guardar_evento("Se Ingreso el RALE_"+tipo_rale.ToUpper()+" de "+total+" casos, creado el dia: "+fecha_rale );
			MessageBox.Show("Se Ingreso el RALE "+tipo_rale.ToUpper()+" de "+total+" casos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			Invoke(new MethodInvoker(delegate{
				this.Close();
			}));
		}
		
		public void bloquear_controles(){
			button1.Enabled=false;
			textBox1.Enabled=false;
			textBox2.Enabled=false;
			button2.Enabled=false;
			dataGridView1.Enabled=false;
		}
		
		public void activar_controles(){
			button1.Enabled=true;
			textBox1.Enabled=true;
			textBox2.Enabled=true;
			button2.Enabled=true;
			dataGridView1.Enabled=true;
		}

        public DataTable copiar_datagrid()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView1.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView1.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView1.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView1.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

		void Button1Click(object sender, EventArgs e)
		{
			int x=0,y=0;
			String lini="",tot_creds="";
			OpenFileDialog open_d = new OpenFileDialog();
			open_d.Filter = "Archivo de RALE (*.T;*.TXT)|*.T;*.TXT";
			open_d.Title = "Seleccione el archivo RALE";
			
			if(open_d.ShowDialog() == DialogResult.OK){
				ruta = open_d.FileName;
				
				StreamReader t1 = new StreamReader(ruta);
				textBox3.Text=t1.ReadToEnd();
				
				if(dataGridView1.RowCount>0){
					dataGridView1.Rows.Clear();
				}
				
				while(x<5){
					lini=textBox3.Lines[(textBox3.Lines.Length-1)-x];
					
					if(lini.Contains("TOTAL DE LA SUBDELEGACION:")==true){
						tot_creds=lini.Substring(80,16);
						tot_creds=tot_creds.TrimStart(' ');
						tot_creds=tot_creds.TrimEnd(' ');
						while(tot_creds.Contains(",")==true){
							y=tot_creds.IndexOf(',');
							tot_creds=tot_creds.Remove(y,1);
						}
						if(int.TryParse(tot_creds,out tot_credits)==true){
							tot_credits=Convert.ToInt32(tot_creds);
						}else{
							tot_credits=100000;
						}
						x=6;
					}
					
					x++;
				}
			
				t1.Close();
				//panel2.Enabled=false;
				bloquear_controles();
				panel1.Visible=true;
				//MessageBox.Show(""+tot_credits);
			
				hilosecundario = new Thread(new ThreadStart(leer_rale));
				hilosecundario.Start();
			}
		}
		
		void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void Lector_rale_txtLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			rale.Columns.Add("columna01",tipo_rale.GetType());
			rale.Columns.Add("columna02",tipo_rale.GetType());
			rale.Columns.Add("columna03",tipo_rale.GetType());
			rale.Columns.Add("columna04",tipo_rale.GetType());
			rale.Columns.Add("columna05",tipo_rale.GetType());
			rale.Columns.Add("columna06",tipo_rale.GetType());
			rale.Columns.Add("columna07",tipo_rale.GetType());
			rale.Columns.Add("columna08",tipo_rale.GetType());
			rale.Columns.Add("columna09",tipo_rale.GetType());
			rale.Columns.Add("columna10",tipo_rale.GetType());
			rale.Columns.Add("columna11",tipo_rale.GetType());
			rale.Columns.Add("columna12",tipo_rale.GetType());
			rale.Columns.Add("columna13",tipo_rale.GetType());
			rale.Columns.Add("columna14",tipo_rale.GetType());
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			DialogResult resu = MessageBox.Show("¿Desea Ingresar el archivo RALE leído?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);
			
			if(resu == DialogResult.Yes){
				bloquear_controles();
				if(ini_tipo==1){
					hilosecundario = new Thread(new ThreadStart(guardar_rale_principal));
					hilosecundario.Start();
					//guardar_rale_principal();
				}else{
					if(ini_tipo==2){
						hilosecundario = new Thread(new ThreadStart(guardar_rale_inventario));
						hilosecundario.Start();
						//guardar_rale_inventario();
					}else{
						if(ini_tipo==3){
							hilosecundario = new Thread(new ThreadStart(guardar_rale_cartera_inventario));
							hilosecundario.Start();
						}
					
					}
				}
			}
		}
		
		void Lector_rale_txtFormClosing(object sender, FormClosingEventArgs e)
		{
			if(ini_tipo==2){
				MessageBox.Show("Debe Ingresar en esta Ventana el RALE de COP y el RALE de RCV, si solo ingresa uno de los dos, no podrá añadir el otro posteriormente.","AVISO");
				DialogResult respuesta = MessageBox.Show("Está a punto de cerrar la Ventana.\n¿Desea Continuar?" ,"ATENCIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
				
				if(respuesta ==DialogResult.Yes){
					
				}else{
					e.Cancel=true;
				}
			}
		}

        private void exportarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                //GUARDAR EVIDENCIA EDICION
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    DataTable nn_excel = copiar_datagrid();

                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(nn_excel, "Lista");
                    wb.SaveAs(@"" + dialog_save.FileName + "");
                    //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
	}
}

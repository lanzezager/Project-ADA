/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 24/04/2017
 * Hora: 04:58 p.m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Actualizador.
	/// </summary>
	public partial class Actualizador : Form
	{
		public Actualizador()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		
		DataTable rales = new DataTable();
		DataTable base_completa = new DataTable();
		DataTable rale_completo = new DataTable();
		DataTable consultas = new DataTable();
		
		//Declaracion del Hilo para ejecutar un subproceso
		private Thread hilosecundario = null;
	
		public void analisis(){
			String rp,cred,peri,status,nn;
			int i=0,progreso=0,e0=0,tra=0,noti=0,estra=0,depu=0,cart=0,non=0,otros=0,desco=0;
			
			DataView vista = new DataView(base_completa);
			Invoke(new MethodInvoker(delegate
					             {
			progressBar1.Visible=true;
			progressBar1.Value=0;
			                         }));
			
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			
			do{
				rp=rale_completo.Rows[i][0].ToString();
				cred=rale_completo.Rows[i][1].ToString();
				peri=rale_completo.Rows[i][2].ToString();
				
				if(consultas.Rows.Count>0){
					consultas.Rows.Clear();
				}
				
				vista.RowFilter="registro_patronal2='"+rp+"' AND credito_cuotas='"+cred+"' AND periodo='"+peri+"'";
				consultas=vista.ToTable();
				
				if(consultas.Rows.Count>0){
					status=consultas.Rows[0][4].ToString();
					nn=consultas.Rows[0][5].ToString();
					
					if(nn.Equals("-")==true){
						if(status.StartsWith("DEPU")==true){
							Invoke(new MethodInvoker(delegate
					             {
								dataGridView6.Rows.Add(rp,cred,peri,status,nn);
								depu++;
							                         }));
						}else{
							Invoke(new MethodInvoker(delegate
					             {
							switch(status){
									case "0":	dataGridView2.Rows.Add(rp,cred,peri,status,nn);
									e0++;
									break;
									case "EN TRAMITE":	dataGridView3.Rows.Add(rp,cred,peri,status,nn);
									tra++;
									break;
									case "NOTIFICADO":	dataGridView4.Rows.Add(rp,cred,peri,status,nn);
									noti++;
									break;
									case "CARTERA":	dataGridView7.Rows.Add(rp,cred,peri,status,nn);
									cart++;
									break;
									default:	dataGridView9.Rows.Add(rp,cred,peri,status,nn);
									otros++;
									break;
							}
							                         }));
						}
					}else{
						if(nn.Equals("NN")==true){
							Invoke(new MethodInvoker(delegate
					             {
							dataGridView8.Rows.Add(rp,cred,peri,status,nn);
							non++;
							                         }));
						}else{
							if(nn.Equals("ESTRADOS")==true){
								Invoke(new MethodInvoker(delegate
					             {
								dataGridView5.Rows.Add(rp,cred,peri,status,nn);
								estra++;
								 }));
							}
						}
					}
					
				}else{
					if(consultas.Rows.Count>0){
						consultas.Rows.Clear();
					}
					vista.RowFilter="registro_patronal2='"+rp+"' AND credito_multa='"+cred+"' AND periodo='"+peri+"'";
					consultas=vista.ToTable();
					
					if(consultas.Rows.Count>0){
						status=consultas.Rows[0][4].ToString();
						nn=consultas.Rows[0][5].ToString();
						
						if(nn.Equals("-")==true){
					    	
							if(status.StartsWith("DEPU")==true){
								Invoke(new MethodInvoker(delegate
					             {
								dataGridView6.Rows.Add(rp,cred,peri,status,nn);
								depu++;
								                         }));
						}else{
								Invoke(new MethodInvoker(delegate
					             {
							switch(status){
									case "0":	dataGridView2.Rows.Add(rp,cred,peri,status,nn);
									e0++;
									break;
									case "EN TRAMITE":	dataGridView3.Rows.Add(rp,cred,peri,status,nn);
									tra++;
									break;
									case "NOTIFICADO":	dataGridView4.Rows.Add(rp,cred,peri,status,nn);
									noti++;
									break;
									case "CARTERA":	dataGridView7.Rows.Add(rp,cred,peri,status,nn);
									cart++;
									break;
									default:	dataGridView9.Rows.Add(rp,cred,peri,status,nn);
									otros++;
									break;
							}
								                         }));
						}
					    	                         
						}else{
							if(nn.Equals("NN")==true){
					    		Invoke(new MethodInvoker(delegate
					             {
								dataGridView8.Rows.Add(rp,cred,peri,status,nn);
								non++;
								                         }));
							}else{
								if(nn.Equals("ESTRADOS")==true){
					    			Invoke(new MethodInvoker(delegate
					             {
									dataGridView5.Rows.Add(rp,cred,peri,status,nn);
									estra++;
									                         }));
								}
							}
						}
					}else{
						Invoke(new MethodInvoker(delegate
					             {
						dataGridView11.Rows.Add(rp,cred,peri,"-","-");
                        desco++;
						                         }));
					}
				}
				i++;
				Invoke(new MethodInvoker(delegate
					             {
				label3.Text="Procesado "+i+" de "+rale_completo.Rows.Count;
				label3.Refresh();
				progreso=Convert.ToInt32((i*100)/rale_completo.Rows.Count);
				progressBar1.Value=progreso;
				progressBar1.Refresh();
				                         }));
			}while(i<rale_completo.Rows.Count);
			
			MessageBox.Show("Análisis Completado","Exito");
			Invoke(new MethodInvoker(delegate
					             {
			progressBar1.Visible=false;
			
			dataGridView1.Rows.Add("STATUS 0",e0);
			dataGridView1.Rows.Add("EN TRAMITE",tra);
			dataGridView1.Rows.Add("NOTIFICADOS",noti);
			dataGridView1.Rows.Add("ESTRADOS",estra);
			dataGridView1.Rows.Add("DEPURADOS",depu);
			dataGridView1.Rows.Add("EN CARTERA",cart);
			dataGridView1.Rows.Add("NN",non);
			dataGridView1.Rows.Add("OTROS",otros);
            dataGridView1.Rows.Add("DESCONOCIDOS", desco);
			dataGridView1.Rows.Add("TOTAL",(e0+tra+noti+estra+depu+cart+non+otros+desco));
			
			tabPage2.Text="Estatus '0' ("+e0+")";
			tabPage3.Text="En Trámite ("+tra+")";
			tabPage4.Text="Notificados ("+noti+")";
			tabPage5.Text="Estrados ("+estra+")";
			tabPage6.Text="Depurados ("+depu+")";
			tabPage7.Text="En Cartera ("+cart+")";
			tabPage8.Text="NN ("+non+")";
			tabPage9.Text="Otros ("+otros+")";
			
			label3.Text="";
			label3.Refresh();
			tabControl1.Visible=true;
			button4.Enabled=true;
			                         }));
		}
				
		public void analisis_v2(){
			String rp,cred,peri,status,nn;
			int i=0,progreso=0,e0=0,tra=0,noti=0,estra=0,depu=0,cart=0,non=0,otros=0;
			
			DataView vista = new DataView(base_completa);
			
			
			progressBar1.Visible=true;
			progressBar1.Value=0;
			
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			consultas.Columns.Add();
			
			do{
				rp=rale_completo.Rows[i][0].ToString();
				cred=rale_completo.Rows[i][1].ToString();
				peri=rale_completo.Rows[i][2].ToString();
				
				if(consultas.Rows.Count>0){
					consultas.Rows.Clear();
				}
				
				vista.RowFilter="registro_patronal2='"+rp+"' AND credito_cuotas='"+cred+"' AND periodo='"+peri+"'";
				consultas=vista.ToTable();
				
				if(consultas.Rows.Count>0){
					status=consultas.Rows[0][4].ToString();
					nn=consultas.Rows[0][5].ToString();
					
					if(nn.Equals("-")==true){
						if(status.StartsWith("DEPU")==true){
								dataGridView6.Rows.Add(rp,cred,peri,status,nn);
								depu++;
						}else{
							switch(status){
									case "0":	dataGridView2.Rows.Add(rp,cred,peri,status,nn);
									e0++;
									break;
									case "EN TRAMITE":	dataGridView3.Rows.Add(rp,cred,peri,status,nn);
									tra++;
									break;
									case "NOTIFICADO":	dataGridView4.Rows.Add(rp,cred,peri,status,nn);
									noti++;
									break;
									case "CARTERA":	dataGridView7.Rows.Add(rp,cred,peri,status,nn);
									cart++;
									break;
									default:	dataGridView9.Rows.Add(rp,cred,peri,status,nn);
									otros++;
									break;
							}
						}
					}else{
						if(nn.Equals("NN")==true){
							
							dataGridView8.Rows.Add(rp,cred,peri,status,nn);
							non++;
							                         
						}else{
							if(nn.Equals("ESTRADOS")==true){
								
								dataGridView5.Rows.Add(rp,cred,peri,status,nn);
								estra++;
								                         
							}
						}
					}
					
				}else{
					if(consultas.Rows.Count>0){
						consultas.Rows.Clear();
					}
					vista.RowFilter="registro_patronal2='"+rp+"' AND credito_multa='"+cred+"' AND periodo='"+peri+"'";
					consultas=vista.ToTable();
					
					if(consultas.Rows.Count>0){
						status=consultas.Rows[0][4].ToString();
						nn=consultas.Rows[0][5].ToString();
						
						if(nn.Equals("-")==true){
					    	
							if(status.StartsWith("DEPU")==true){
								dataGridView6.Rows.Add(rp,cred,peri,status,nn);
								depu++;
						}else{
							switch(status){
									case "0":	dataGridView2.Rows.Add(rp,cred,peri,status,nn);
									e0++;
									break;
									case "EN TRAMITE":	dataGridView3.Rows.Add(rp,cred,peri,status,nn);
									tra++;
									break;
									case "NOTIFICADO":	dataGridView4.Rows.Add(rp,cred,peri,status,nn);
									noti++;
									break;
									case "CARTERA":	dataGridView7.Rows.Add(rp,cred,peri,status,nn);
									cart++;
									break;
									default:	dataGridView9.Rows.Add(rp,cred,peri,status,nn);
									otros++;
									break;
							}
						}
					    	                         
						}else{
							if(nn.Equals("NN")==true){
					    		
								dataGridView8.Rows.Add(rp,cred,peri,status,nn);
								non++;
					    		                        
							}else{
								if(nn.Equals("ESTRADOS")==true){
					    			
									dataGridView5.Rows.Add(rp,cred,peri,status,nn);
									estra++;
					    			                       
								}
							}
						}
					}else{
						dataGridView9.Rows.Add(rp,cred,peri,"-","-");
						otros++;
					}
				}
				i++;
				
				label3.Text="Procesado "+i+" de "+rale_completo.Rows.Count;
				label3.Refresh();
				progreso=Convert.ToInt32((i*100)/rale_completo.Rows.Count);
				progressBar1.Value=progreso;
				progressBar1.Refresh();
				                        
			}while(i<150);
			
			MessageBox.Show("Análisis Completado","Exito");
			
			progressBar1.Visible=false;
			
			dataGridView1.Rows.Add("STATUS 0",e0);
			dataGridView1.Rows.Add("EN TRAMITE",tra);
			dataGridView1.Rows.Add("NOTIFICADOS",noti);
			dataGridView1.Rows.Add("ESTRADOS",estra);
			dataGridView1.Rows.Add("DEPURADOS",depu);
			dataGridView1.Rows.Add("EN CARTERA",cart);
			dataGridView1.Rows.Add("NN",non);
			dataGridView1.Rows.Add("Otros",otros);
			
			label3.Text="";
			label3.Refresh();
			                         
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void ActualizadorLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			String rale_cop,rale_rcv;
			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			rales=conex2.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_COP%\" order by idlog_eventos desc");
			rale_cop=rales.Rows[0][1].ToString().Substring(0,10);
			rales=conex2.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_RCV%\" order by idlog_eventos desc");
			rale_rcv=rales.Rows[0][1].ToString().Substring(0,10);
			label2.Text="Rale COP Ingresado el Día: "+rale_cop+"\nRale RCV Ingresado el Día: "+rale_rcv; 
			label2.Refresh();
			
			dataGridView1.Columns.Add("estatus","ESTATUS");
			dataGridView1.Columns.Add("total","TOTAL");
			
			dataGridView2.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView2.Columns.Add("CREDITO","CREDITO");
			dataGridView2.Columns.Add("PERIODO","PERIODO");
			dataGridView2.Columns.Add("STATUS","STATUS");
			dataGridView2.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");
			
			dataGridView3.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView3.Columns.Add("CREDITO","CREDITO");
			dataGridView3.Columns.Add("PERIODO","PERIODO");
			dataGridView3.Columns.Add("STATUS","STATUS");
			dataGridView3.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");
			
			dataGridView4.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView4.Columns.Add("CREDITO","CREDITO");
			dataGridView4.Columns.Add("PERIODO","PERIODO");
			dataGridView4.Columns.Add("STATUS","STATUS");
			dataGridView4.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");
			
			dataGridView5.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView5.Columns.Add("CREDITO","CREDITO");
			dataGridView5.Columns.Add("PERIODO","PERIODO");
			dataGridView5.Columns.Add("STATUS","STATUS");
			dataGridView5.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");;
			
			dataGridView6.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView6.Columns.Add("CREDITO","CREDITO");
			dataGridView6.Columns.Add("PERIODO","PERIODO");
			dataGridView6.Columns.Add("STATUS","STATUS");
			dataGridView6.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");
			
			dataGridView7.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView7.Columns.Add("CREDITO","CREDITO");
			dataGridView7.Columns.Add("PERIODO","PERIODO");
			dataGridView7.Columns.Add("STATUS","STATUS");
			dataGridView7.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");
			
			dataGridView8.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView8.Columns.Add("CREDITO","CREDITO");
			dataGridView8.Columns.Add("PERIODO","PERIODO");
			dataGridView8.Columns.Add("STATUS","STATUS");
			dataGridView8.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");
			
			dataGridView9.Columns.Add("REGISTRO PATRONAL","REGISTRO\nPATRONAL");
			dataGridView9.Columns.Add("CREDITO","CREDITO");
			dataGridView9.Columns.Add("PERIODO","PERIODO");
			dataGridView9.Columns.Add("STATUS","STATUS");
			dataGridView9.Columns.Add("ESTADO NOTIFICACION","ESTADO\nNOTIFICACION");

            dataGridView11.Columns.Add("REGISTRO PATRONAL", "REGISTRO\nPATRONAL");
            dataGridView11.Columns.Add("CREDITO", "CREDITO");
            dataGridView11.Columns.Add("PERIODO", "PERIODO");
            dataGridView11.Columns.Add("STATUS", "STATUS");
            dataGridView11.Columns.Add("ESTADO NOTIFICACION", "ESTADO\nNOTIFICACION");
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			DialogResult respuesta = MessageBox.Show("La Elaboración de este Análisis tomará unos minutos.\n\n¿Desea comenzar?",
			                                         "CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
			if(respuesta ==DialogResult.Yes){
				//tabControl1.Visible=false;
				base_completa=conex.consultar("SELECT registro_patronal2,credito_cuotas,credito_multa,periodo,status,nn FROM datos_factura where registro_patronal2 in (SELECT distinct registro_patronal FROM rale WHERE (incidencia=\"01\" OR incidencia=\"1\"))");
				rale_completo=conex2.consultar("SELECT registro_patronal,credito,periodo FROM rale WHERE (incidencia=\"01\" OR incidencia=\"1\")");
				button4.Enabled=false;
				hilosecundario = new Thread(new ThreadStart(analisis));
                hilosecundario.Start();
				//analisis_v2();
			}
		}
	}
}

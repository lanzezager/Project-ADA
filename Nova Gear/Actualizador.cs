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

        DataTable data1 = new DataTable();
        DataTable data2 = new DataTable();
        DataTable data3 = new DataTable();
        DataTable data4 = new DataTable();
        DataTable data5 = new DataTable();
        DataTable data6 = new DataTable();
        DataTable data7 = new DataTable();
        DataTable data8 = new DataTable();
        DataTable data9 = new DataTable();
        DataTable data10 = new DataTable();
		
		//Declaracion del Hilo para ejecutar un subproceso
		private Thread hilosecundario = null;

        public DataTable copiar_datagrid_1()
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

        public DataTable copiar_datagrid_2()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView2.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView2.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView2.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView2.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView2.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_3()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView3.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView3.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView3.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView3.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView3.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_4()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView4.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView4.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView4.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView4.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView4.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_5()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView5.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView5.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView5.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView5.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView5.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_6()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView6.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView6.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView6.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView6.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView6.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_7()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView7.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView7.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView7.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView7.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView7.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_8()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView8.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView8.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView8.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView8.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView8.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_9()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView9.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView9.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView9.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView9.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView9.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_11()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView11.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView11.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView11.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView11.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView11.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }
	
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0){
                //GUARDAR EXCEL
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                data1 = copiar_datagrid_1();
                data2 = copiar_datagrid_2();
                data3 = copiar_datagrid_3();
                data4 = copiar_datagrid_4();
                data5 = copiar_datagrid_5();
                data6 = copiar_datagrid_6();
                data7 = copiar_datagrid_7();
                data8 = copiar_datagrid_8();
                data9 = copiar_datagrid_9();
                data10 = copiar_datagrid_11();

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(data1, "Totales");
                    wb.Worksheets.Add(data2, "Estatus_0");
                    wb.Worksheets.Add(data3, "En_Tramite");
                    wb.Worksheets.Add(data4, "Notificados");
                    wb.Worksheets.Add(data5, "Estrados");
                    wb.Worksheets.Add(data6, "Depurados");
                    wb.Worksheets.Add(data7, "NN");
                    wb.Worksheets.Add(data8, "En_Cartera");
                    wb.Worksheets.Add(data9, "Otros");
                    wb.Worksheets.Add(data10, "Desconocidos");
                    wb.SaveAs(@"" + dialog_save.FileName + "");
                    //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("No hay datos que Exportar ", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
	}
}

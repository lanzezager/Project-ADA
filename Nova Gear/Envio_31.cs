/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 29/05/2018
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
using Ionic.Zip;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Envio_31.
	/// </summary>
	public partial class Envio_31 : Form
	{
		public Envio_31()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		Ordena_listviews ordena = new Ordena_listviews();
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();//usuarios
		Conexion conex3 = new Conexion();//detalle cores
		Conexion conex4 = new Conexion();//rale
		Conexion conex5 = new Conexion();//resultados
		Conexion conex6 = new Conexion();//rale completo
		
		DataTable tabla_cores = new DataTable();
		DataTable tabla_usuarios = new DataTable();
		DataTable tabla_detalle_core = new DataTable();		
		DataTable tabla_detalle_core_orde = new DataTable();
		DataTable tabla_detalle_core_orde2 = new DataTable();
		DataTable data_query = new DataTable();
		DataTable data_query_comple = new DataTable();
		DataTable tabla_results = new DataTable();
		DataTable guarda_results = new DataTable();
		
		
		String incid_rale="",td_rale="",nom_save="",tipo_env="",id_core="",folio_control="",observaciones="CAPTURADO SIN PROBLEMAS",user_id;
		FileStream fichero,fichero1,fichero2;
		int err=0,aciert=0,desde=0,hasta=0;
		
		public void borra_txbx (){
			textBox1.Text="";
			textBox2.Text="";
			textBox3.Text="";
			textBox4.Text="";
			textBox5.Text="";
			textBox6.Text="";
			textBox7.Text="";
			textBox8.Text="";
			textBox9.Text="";
			textBox10.Text="";
			textBox11.Text="";
			textBox12.Text="";
			textBox13.Text="";
			textBox14.Text="";
			textBox15.Text="";
		}
		
		public void inicio_carga_cores(){
			
			int i=0,pos_coma;
			String nombre="",importe;
			listView1.Items.Clear();
			borra_txbx();
			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			//tabla_cores=conex.consultar("SELECT tipo_core,clase_core,estatus,folio_notificacion,folio_control_registros,fecha_elaboracion,num_casos,importe_total,justificacion,usuario_autor,fecha_afectacion,num_aciertos,num_errores,usuario_afecta,observaciones,id_core FROM info_cores WHERE tipo_core=\"INC_31\" ORDER BY fecha_elaboracion DESC");
			tabla_cores=conex.consultar("SELECT tipo_core,clase_core,estatus,folio_notificacion,folio_control_registros,fecha_elaboracion,num_casos,importe_total,justificacion,usuario_autor,fecha_afectacion,num_aciertos,num_errores,usuario_afecta,observaciones,id_core FROM info_cores ORDER BY fecha_elaboracion DESC");
			tabla_usuarios=conex2.consultar("SELECT nom_usuario,id_usuario FROM usuarios ORDER BY id_usuario ASC");
			
			while(i<tabla_cores.Rows.Count){
				nombre=tabla_cores.Rows[i][1].ToString();
				nombre+="_"+tabla_cores.Rows[i][0].ToString()+"_"+tabla_cores.Rows[i][3].ToString().Substring(3,tabla_cores.Rows[i][3].ToString().Length-3);
				
				listView1.Items.Add(nombre);
				
				if(tabla_cores.Rows[i][2].ToString().ToUpper().Equals("EN ESPERA")){
					listView1.Items[i].ImageIndex=0;
				}else{
					if(tabla_cores.Rows[i][2].ToString().ToUpper().Equals("CAPTURA TOTAL")){
						listView1.Items[i].ImageIndex=1;
					}else{ 
						listView1.Items[i].ImageIndex=2;
					}
				}
								
				listView1.Items[i].SubItems.Add(tabla_cores.Rows[i][4].ToString());
				listView1.Items[i].SubItems.Add(tabla_cores.Rows[i][6].ToString());
				importe=tabla_cores.Rows[i][7].ToString();
				pos_coma=importe.IndexOf('.');
				while((pos_coma-3) >= 1){
					importe=importe.Insert((pos_coma-3),",");
					pos_coma=pos_coma-3;
				}
				listView1.Items[i].SubItems.Add(importe);
				if(tabla_cores.Rows[i][5].ToString().Length>10){
					listView1.Items[i].SubItems.Add(tabla_cores.Rows[i][5].ToString().Substring(0,10));
				}else{
					listView1.Items[i].SubItems.Add(tabla_cores.Rows[i][5].ToString());
				}
				i++;
			}
			
			
			this.listView1.ListViewItemSorter = ordena;
			label17.Text="N° Cores: "+listView1.Items.Count;
		}
		
		public void cargar_info_core(int indice){
			
			String importe;
			int pos_coma;
			
			if(indice >= 0){
				textBox8.Text=tabla_cores.Rows[indice][2].ToString();//estatus
				textBox2.Text=tabla_cores.Rows[indice][1].ToString();//clase_core
				textBox1.Text=tabla_cores.Rows[indice][0].ToString();//tipo_core
				textBox6.Text=tabla_cores.Rows[indice][3].ToString();//folio_not
				if(tabla_cores.Rows[indice][5].ToString().Length>9){
					textBox3.Text=tabla_cores.Rows[indice][5].ToString().Substring(0,10);//fecha_elab
				}else{
					textBox3.Text=tabla_cores.Rows[indice][5].ToString();//fecha_elab
				}
				textBox4.Text=tabla_cores.Rows[indice][6].ToString();//casos
				
				importe=tabla_cores.Rows[indice][7].ToString();
				pos_coma=importe.IndexOf('.');
				while((pos_coma-3) >= 1){
					importe=importe.Insert((pos_coma-3),",");
					pos_coma=pos_coma-3;
				}
				textBox5.Text=importe;//importe
				if(Convert.ToInt32(tabla_cores.Rows[indice][9].ToString())>0){
					textBox9.Text=tabla_usuarios.Rows[Convert.ToInt32(tabla_cores.Rows[indice][9].ToString())-1][0].ToString();//autor
				}else{
					textBox9.Text="-";//autor
				}
				
				textBox7.Text=tabla_cores.Rows[indice][8].ToString();//justificacion
				if(tabla_cores.Rows[indice][10].ToString().Length>9){
					textBox10.Text=tabla_cores.Rows[indice][10].ToString().Substring(0,10);//fecha_cap
				}else{
					textBox10.Text=tabla_cores.Rows[indice][10].ToString();//fecha_cap
				}
				textBox13.Text=tabla_cores.Rows[indice][4].ToString();//folio_control
				textBox11.Text=tabla_cores.Rows[indice][11].ToString();//aciertos
				textBox12.Text=tabla_cores.Rows[indice][12].ToString();//errores
				if(Convert.ToInt32(tabla_cores.Rows[indice][13].ToString())>0){
					textBox14.Text=tabla_usuarios.Rows[Convert.ToInt32(tabla_cores.Rows[indice][13].ToString())-1][0].ToString();//capturista
				}else{
					textBox14.Text="-";//capturista
				}
				
				textBox15.Text=tabla_cores.Rows[indice][14].ToString();//observaciones
				textBox16.Text=tabla_cores.Rows[indice][15].ToString();//id_core
				
				if(tabla_cores.Rows[indice][4].ToString().Equals("-")==false){
					textBox17.Text=tabla_cores.Rows[indice][4].ToString();//folio_control
				}else{
					textBox17.Text="";
				}
				
				dataGridView1.DataSource=null;
				dataGridView1.Columns.Clear();
				dataGridView1.Rows.Clear();
				label18.Text="N° Casos: 0";
			}
		}
		
		public void cargar_core(){
			if(textBox1.Text.Contains("INC")){
				cargar_core_inc31();
			}else{
				if(textBox1.Text.Contains("CM")){
					cargar_core_cm12();
				}
			}
		}
		
		public void cargar_core_inc31(){
			
			String core_buscar="",sql_rale="",res_capt="";
			int i=0,asunto=0;
			string[] reg_cred;
			reg_cred= new string[4];
			
			core_buscar=textBox2.Text;
			core_buscar=core_buscar+"_"+textBox1.Text.Substring((textBox1.Text.Length-2),2);
			core_buscar=core_buscar+"_"+textBox6.Text.Substring((textBox6.Text.LastIndexOf('_')+1),(textBox6.Text.Length-(textBox6.Text.LastIndexOf('_')+1)));
			id_core=textBox16.Text;
			
			conex3.conectar("base_principal");
			//MessageBox.Show(core_buscar);
			tabla_detalle_core=conex3.consultar("SELECT registro_patronal2,periodo,subdelegacion,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_notificacion,folio_not_core,id,capturado_siscob,tipo_documento FROM datos_factura WHERE folio_not_core LIKE \"%"+core_buscar+"_%\" ORDER BY registro_patronal2,credito_cuotas,credito_multa");
			
			//dataGridView1.DataSource=tabla_detalle_core;
			if(tabla_detalle_core.Rows.Count>0){
				
				conex4.conectar("base_principal");
				
				if(textBox2.Text.Equals("COP")){//COP
                    sql_rale = "SELECT registro_patronal,credito,periodo,incidencia,td FROM rale WHERE tipo_rale=\"COP\" AND (incidencia = \"01\" OR incidencia = \"1\" OR incidencia = \"2\" OR incidencia = \"02\") AND fecha_noti <> \"0000-00-00\"";
				}else{//RCV
                    sql_rale = "SELECT registro_patronal,credito,periodo,incidencia,td  FROM rale WHERE tipo_rale=\"RCV\" AND (incidencia = \"01\" OR incidencia = \"1\" OR incidencia = \"2\" OR incidencia = \"02\") AND fecha_noti <> \"0000-00-00\"";
				}
				
				data_query=conex4.consultar(sql_rale);
				//MessageBox.Show(""+data_query.Rows.Count);
				//dataGridView1.Rows.Clear();
				//dataGridView1.Columns.Clear();
				/*if(dataGridView1.Columns.Count==0){
					dataGridView1.Columns.Add("asunto","ASUNTO");
					dataGridView1.Columns.Add("reg_pat","REGISTRO PATRONAL");
					dataGridView1.Columns.Add("credito","CRÉDITO");
					dataGridView1.Columns.Add("periodo","PERIODO");
					dataGridView1.Columns.Add("td","TD");
					dataGridView1.Columns.Add("inc","INC");
					dataGridView1.Columns.Add("sub","SUB");
					dataGridView1.Columns.Add("importe","IMPORTE");
					dataGridView1.Columns.Add("fecha_not","FECHA NOTIFICACIÓN");
				}*/				
				
				tabla_detalle_core_orde.Rows.Clear();
				
				while(i<tabla_detalle_core.Rows.Count){
					
					reg_cred=tabla_detalle_core.Rows[i][10].ToString().Split('/','|');
					
					if(tabla_detalle_core.Rows[i][8].ToString().Contains((core_buscar+"_1"))==true){
						if(checar_rale(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
							asunto++;
							
							if(reg_cred[2].Equals("0")){
								res_capt="0";
							}
							if(reg_cred[2].Equals("1")){
								res_capt="CAPTURADO";
							}
							if(reg_cred[2].Equals("2")){
								res_capt="ERROR";
							}
							
							tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][5].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),res_capt);
						}else{
							asunto++;
							if(checar_rale_completo(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
								tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][5].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_INC_INV");
							}else{
								tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString(),"02",tabla_detalle_core.Rows[i][11].ToString(),tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][5].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_NO_VIG");
							}
						}
						
					}else{
						if(tabla_detalle_core.Rows[i][8].ToString().Contains((core_buscar+"_2"))==true){
							if(checar_rale(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
								asunto++;
								
								if(reg_cred[3].Equals("0")){
									res_capt="0";
								}
								if(reg_cred[3].Equals("1")){
									res_capt="CAPTURADO";
								}
								if(reg_cred[3].Equals("2")){
									res_capt="ERROR";
								}
								
								tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][6].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),res_capt);
							}else{
								asunto++;
								if(checar_rale_completo(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
									//MessageBox.Show(tabla_detalle_core.Rows[i][0].ToString()+"|"+tabla_detalle_core.Rows[i][4].ToString()+"|"+tabla_detalle_core.Rows[i][1].ToString());
									tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][6].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_INC_INV");
								}else{
									tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString(),"80",tabla_detalle_core.Rows[i][11].ToString(),tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][6].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_NO_VIG");
								}
							}
						}
						
						if(tabla_detalle_core.Rows[i][8].ToString().Contains((core_buscar+"_3"))==true){
							if(checar_rale(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
								asunto++;
								
								if(reg_cred[2].Equals("0")){
									res_capt="0";
								}
								if(reg_cred[2].Equals("1")){
									res_capt="CAPTURADO";
								}
								if(reg_cred[2].Equals("2")){
									res_capt="ERROR";
								}
								
								tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][5].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),res_capt);
							}else{
								asunto++;
								if(checar_rale_completo(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
									tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][5].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_INC_INV");
								}else{
									tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][3].ToString(),tabla_detalle_core.Rows[i][1].ToString(),"06",tabla_detalle_core.Rows[i][11].ToString(),tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][5].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_NO_VIG");
								}
							}
							
							if(checar_rale(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
								asunto++;
								
								if(reg_cred[3].Equals("0")){
									res_capt="0";
								}
								if(reg_cred[3].Equals("1")){
									res_capt="CAPTURADO";
								}
								if(reg_cred[3].Equals("2")){
									res_capt="ERROR";
								}
								
								tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][6].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),res_capt);
							}else{
								asunto++;
								if(checar_rale_completo(tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString()) == true){
									tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString(),td_rale,incid_rale,tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][6].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_INC_INV");
								}else{
									tabla_detalle_core_orde.Rows.Add(asunto,tabla_detalle_core.Rows[i][0].ToString(),tabla_detalle_core.Rows[i][4].ToString(),tabla_detalle_core.Rows[i][1].ToString(),"81",tabla_detalle_core.Rows[i][11].ToString(),tabla_detalle_core.Rows[i][2].ToString(),tabla_detalle_core.Rows[i][6].ToString(),tabla_detalle_core.Rows[i][7].ToString().Substring(0,10),"OMITIDO_NO_VIG");
								}
							}
						}
					}
					i++;
				}
				
				tabla_detalle_core_orde.DefaultView.Sort="REGISTRO PATRONAL ASC,CREDITO ASC";
				dataGridView1.DataSource=tabla_detalle_core_orde;
				dataGridView1.Columns[9].MinimumWidth=115;
				
				
				label18.Text="N° Casos: "+dataGridView1.RowCount;
				label18.Refresh();
				
				conex3.cerrar();
				conex4.cerrar();
			}
		}
		
		public void cargar_core_cm12(){
			String core_buscar="",res_capt="",td="";
			int i=0,cred=0,imp=0;
			
			core_buscar=textBox2.Text;
			core_buscar=core_buscar+"_"+textBox1.Text.Substring((textBox1.Text.Length-2),2);
			core_buscar=core_buscar+"_"+textBox6.Text.Substring((textBox6.Text.LastIndexOf('_')+1),(textBox6.Text.Length-(textBox6.Text.LastIndexOf('_')+1)));
			id_core=textBox16.Text;
			
			conex3.conectar("base_principal");
			//MessageBox.Show(core_buscar);
			tabla_detalle_core_orde2=conex3.consultar("SELECT registro_patronal2,credito_multa,periodo,tipo_documento,subdelegacion,importe_multa,folio_sipare_sua,folio_not_core,id,captura_cm12,credito_cuotas,importe_cuota FROM datos_factura WHERE folio_not_core LIKE \"%"+core_buscar+"_%\" ORDER BY registro_patronal2,credito_multa");
						
			if(dataGridView1.Columns.Count==0){
					dataGridView1.Columns.Add("asunto","ASUNTO");
					dataGridView1.Columns.Add("reg_pat","REGISTRO PATRONAL");
					dataGridView1.Columns.Add("credito","CRÉDITO");
					dataGridView1.Columns.Add("periodo","PERIODO");
					dataGridView1.Columns.Add("td","TD");
					dataGridView1.Columns.Add("inc","INC");
					dataGridView1.Columns.Add("sub","SUB");
					dataGridView1.Columns.Add("importe","IMPORTE");
					dataGridView1.Columns.Add("folio_pago","FOLIO PAGO");
					dataGridView1.Columns.Add("res_cap","RESULTADO CAPTURA");					
			}else{
				if(!dataGridView1.Columns[8].HeaderText.Equals("FOLIO PAGO")){
					dataGridView1.Columns.Add("folio_pago","FOLIO PAGO");
				}
				dataGridView1.Rows.Clear();
			}
			
			while(i<tabla_detalle_core_orde2.Rows.Count){
				if(tabla_detalle_core_orde2.Rows[i][7].ToString().Substring((tabla_detalle_core_orde2.Rows[i][7].ToString().Length-1),1).Equals("2")){
					cred=1;
					imp=5;
					if(tabla_detalle_core_orde2.Rows[i][9].ToString().Length>1){
						res_capt=tabla_detalle_core_orde2.Rows[i][9].ToString().Substring(2,1);
					}else{
						res_capt="POR CAPTURAR";
					}
					
					if(textBox2.Text.Equals("COP")){
						td="80";
					}else{
						td="81";
					}
				}else{
					if(tabla_detalle_core_orde2.Rows[i][7].ToString().Substring((tabla_detalle_core_orde2.Rows[i][7].ToString().Length-1),1).Equals("1")){
						cred=10;
						imp=11;
						if(tabla_detalle_core_orde2.Rows[i][9].ToString().Length>2){
							res_capt=tabla_detalle_core_orde2.Rows[i][9].ToString().Substring(0,1);
						}else{
							res_capt="POR CAPTURAR";
						}
						
						if(textBox2.Text.Equals("COP")){
							td="02";
						}else{
							td="06";
						}
					}
				}
				
				switch(res_capt){
					case "0": res_capt="POR CAPTURAR"; 
							  break;
					case "1": res_capt="CAPTURADO";
							  break;
					case "2": res_capt="ERROR"; 
							  break;
					default: break;
				}
				
				dataGridView1.Rows.Add((i+1).ToString(),tabla_detalle_core_orde2.Rows[i][0].ToString(),tabla_detalle_core_orde2.Rows[i][cred].ToString(),tabla_detalle_core_orde2.Rows[i][2].ToString(),td,"1",tabla_detalle_core_orde2.Rows[i][4].ToString(),tabla_detalle_core_orde2.Rows[i][imp].ToString(),tabla_detalle_core_orde2.Rows[i][6].ToString(),res_capt);
				i++;
			}
			
			dataGridView1.Columns[9].MinimumWidth=115;
			label18.Text="N° Casos: "+dataGridView1.RowCount;
			label18.Refresh();
			conex3.cerrar();
			
		}
		
		public bool checar_rale(String rp, String cred, String peri){
			//MessageBox.Show("rp: "+rp+" cred:"+cred+" periodo:"+peri);
			DataView vista = new DataView(data_query);
			
			vista.RowFilter="registro_patronal='"+rp+"' AND credito='"+cred+"' AND periodo='"+peri+"'";
			dataGridView2.DataSource=vista;
			
			if(dataGridView2.RowCount>0){
				incid_rale=dataGridView2.Rows[0].Cells[3].Value.ToString();
				td_rale=dataGridView2.Rows[0].Cells[4].Value.ToString();
				return true;
			}else{
				return false;
			}
            //return true;
		}
		
		public bool checar_rale_completo(String rp, String cred, String peri){
			//MessageBox.Show("rp: "+rp+" cred:"+cred+" periodo:"+peri);
			DataView vista = new DataView(data_query_comple);
			
			vista.RowFilter="registro_patronal='"+rp+"' AND credito='"+cred+"' AND periodo='"+peri+"'";
			dataGridView2.DataSource=vista;
			
			if(dataGridView2.RowCount>0){
				incid_rale=dataGridView2.Rows[0].Cells[3].Value.ToString();
				td_rale=dataGridView2.Rows[0].Cells[4].Value.ToString();
				return true;
			}else{
				return false;
			}
            //return true;
		}
		
		public int corrige_indice(int indice){
			String nombre="",clase="",tipo="",folio="";
			int i=0,indice_correcto=-1;
			string[] nombre_separado;
			
			nombre_separado = new string[5];
			nombre=listView1.Items[indice].Text;
			nombre_separado=nombre.Split('_');
					
			clase=nombre_separado[0];
			tipo=nombre_separado[1]+"_"+nombre_separado[2];
			folio=nombre_separado[2]+"_"+nombre_separado[3];
			
			while(i<tabla_cores.Rows.Count){
				//clase_core															+++tipo_core											+++folio_core
				if((tabla_cores.Rows[i][1].ToString().Equals(clase)==true) && (tabla_cores.Rows[i][0].ToString().Equals(tipo)==true) && (tabla_cores.Rows[i][3].ToString().Equals(folio)==true)){
					indice_correcto=i;
					i=tabla_cores.Rows.Count+1;
				}
				i++;
			}
			
			return indice_correcto;
			
		}
		
		public int guardar_resultados(){

			SaveFileDialog dialog_save = new SaveFileDialog();
			dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
			dialog_save.Title = "Guardar Resultados de Captura";//le damos un titulo a la ventana

			if (dialog_save.ShowDialog() == DialogResult.OK)
			{
				nom_save = dialog_save.FileName; //open file
				return 1;
				//MessageBox.Show("El archivo se generó correctamente.\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
			}else{
				return 0;
			}
		}
		
		public void crea_archivo_captura_inc31(){
			
			int i=0;
			String reg_pat="",credito="",per="",inc="";
			
			//Borrar archivos para comenzar de 0
			if(System.IO.File.Exists(@"capturator_inc/temp.txt")==true){
				System.IO.File.Delete(@"capturator_inc/temp.txt");
			}
			
			if(System.IO.File.Exists(@"capturator_inc/info_core.txt")==true){
				System.IO.File.Delete(@"capturator_inc/info_core.txt");
			}
						
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"capturator_inc/temp.txt");
						
			i=desde;
			while(i<hasta){
				//MessageBox.Show(""+dataGridView1.Columns.Count);
				if(dataGridView1.Rows[i].Cells[9].Value.ToString().Contains("OMITIDO")==false){
					reg_pat=dataGridView1.Rows[i].Cells[1].Value.ToString();
					credito=dataGridView1.Rows[i].Cells[2].Value.ToString();
					per=dataGridView1.Rows[i].Cells[3].Value.ToString();
					inc=textBox1.Text;
					inc=inc.Substring((inc.IndexOf('_')+1),inc.Length-(inc.IndexOf('_')+1));
					
					if(textBox2.Text.Equals("COP")){
						wr.WriteLine(reg_pat);
						wr.WriteLine(credito);
						wr.WriteLine(per.Substring(4, 2));
						wr.WriteLine(per.Substring(2, 2));
						wr.WriteLine(inc);//incidencia
						wr.WriteLine("$");
					}else{//SI ES CAPTURA RCV
						wr.WriteLine(reg_pat);
						wr.WriteLine(per.Substring(4, 2));
						wr.WriteLine(per.Substring(2, 2));
						wr.WriteLine(credito);					
						wr.WriteLine(inc);//incidencia
						wr.WriteLine("$");					
					}
				}
				i++;
			}
			
			wr.WriteLine("%&");
			wr.Close();

			StreamWriter wr1 = new StreamWriter(@"capturator_inc/temp_aux.txt");
			wr1.WriteLine("0");
			wr1.WriteLine((hasta-desde));
			wr1.Close();
			
			StreamWriter wr3 = new StreamWriter(@"capturator_inc/info_core.txt");
			wr3.WriteLine(listView1.Items[listView1.SelectedIndices[0]].Text);
			wr3.Close();
		}
		
		public void enviar_a_captura_inc31(){
			String ruta="",inc="",nom_zip="";
			int nom_dif=0;
			if (guardar_resultados()==1){
				try{
				MessageBox.Show("Asegúrese de seguir estos pasos antes de continuar:\n\n"+
				                "1.- Ejecute Siscob y abra la ventana donde se van capturar los datos\n"+
				                "2.- Coloque el cursor en el campo donde se va a comenzar a escribir\n"+
				                "3.- De click al boton Aceptar de este mensaje","ATENCIÓN",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
				                
				DialogResult respuesta = MessageBox.Show("Está a punto de comenzar el proceso de captura automática.\n"+
					            "Los Archivos Temporales de la Captura Anterior se Borrarán y no podrá recuperarlos si es que estos NO fueron guardados.\n"+                        
				                "Una vez comenzada la captura NO se deberá manipular el equipo\n"+
				                "hasta que haya finalizado el proceso de captura.\n"+
				                "El programa le informará cuando el proceso de captura haya concluido\n\n"+
				                "Teclas Especiales:\n"+
				                "[PAUSA] - Pausa el proceso de captura (no se recomienda)\n"+
				                "[INICIO] - Reanuda el proceso de captura\n"+
				                "[FIN] - Finaliza el proceso de captura\n\n"+
				                "¿Desea comenzar el proceso de captura?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
				
				if(respuesta ==DialogResult.Yes){
					/*
 					System.IO.File.Copy(@"capturator_inc/temp.txt",@"capturator_inc/respaldo/temp.txt",true);
					System.IO.File.Copy(@"capturator_inc/errores_siscob.txt",@"capturator_inc/respaldo/errores_siscob.txt",true);
	        	    System.IO.File.Copy(@"capturator_inc/acierto_siscob.txt",@"capturator_inc/respaldo/acierto_siscob.txt",true);
	        	    System.IO.File.Copy(@"capturator_inc/aciertos_siscob.txt",@"capturator_inc/respaldo/aciertos_siscob.txt",true);					
					System.IO.File.Copy(@"capturator_inc/info_core.txt",@"capturator_inc/respaldo/info_core.txt",true);		
					*/
					
					//Respaldar Archivos de Resultados Previos
					StreamReader rdri = new StreamReader(@"capturator_inc/info_core.txt");
					nom_zip=rdri.ReadLine();
					rdri.Close();
					
					try{
						ZipFile arch = new ZipFile();
						arch.AddFile(@"capturator_inc/temp.txt","");
						arch.AddFile(@"capturator_inc/errores_siscob.txt","");
						arch.AddFile(@"capturator_inc/acierto_siscob.txt","");
						arch.AddFile(@"capturator_inc/aciertos_siscob.txt","");
						arch.AddFile(@"capturator_inc/info_core.txt","");
						
						if(nom_zip.Length>0){
						}else{
							nom_zip="Respaldo_INC31";
						}
						
						if(File.Exists(@"capturator_inc/respaldo/"+nom_zip+".LZ")==true){
							while(File.Exists(@"capturator_inc/respaldo/"+nom_zip+"_"+nom_dif+".LZ")==true){
								nom_dif++;
							}
  							nom_zip=nom_zip+"_"+nom_dif+".LZ";
						}else{
							nom_zip=nom_zip+".LZ";
						}
					 	//arch.Save(arch_lz40);
						arch.Save(@"capturator_inc/respaldo/"+nom_zip);
						//MessageBox.Show("El archivo se guardó correctamente.");
						
					}catch(Exception es){
						MessageBox.Show("Ocurrió el siguiente problema al crear el respaldo del último envío:\n\n"+es);
					}
					
					//crear archivo que se va a capturar
					crea_archivo_captura_inc31();
					
					MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
	                //Borrar archivos para comenzar de 0
					System.IO.File.Delete(@"capturator_inc/errores_siscob.txt");
	        	    System.IO.File.Delete(@"capturator_inc/acierto_siscob.txt");
	        	    System.IO.File.Delete(@"capturator_inc/aciertos_siscob.txt");
	        	    //Crear archivos nuevos
	                fichero = System.IO.File.Create(@"capturator_inc/errores_siscob.txt");
					fichero1= System.IO.File.Create(@"capturator_inc/acierto_siscob.txt");
					fichero2= System.IO.File.Create(@"capturator_inc/aciertos_siscob.txt");
	                ruta = fichero.Name;
	                fichero.Close();
	                fichero1.Close();
	                fichero2.Close();
	                
	                StreamWriter wr1 = new StreamWriter(@"capture_inc.bat");
	                wr1.WriteLine("@echo off");
	                wr1.WriteLine("C:");
	                wr1.WriteLine("cd "+ruta.Substring(0,(ruta.Length-19)));
	                
	                if(textBox2.Text.Equals("COP")){
	                	wr1.WriteLine("start captron.exe");
	                }else{
	                	wr1.WriteLine("start captron_rcv.exe");
	                }
	                wr1.Close();
	               
	                inc=textBox1.Text;
					inc=inc.Substring((inc.IndexOf('_')+1),inc.Length-(inc.IndexOf('_')+1));
	              	tipo_env="INC31";
				   System.Diagnostics.Process.Start(@"capture_inc.bat");
				   conex5.guardar_evento("Se Mando a capturar en Siscob "+dataGridView1.RowCount.ToString()+" Registros de Cambio de Incidencia "+inc+" de "+textBox2.Text);
					button3.Enabled=false;
					button4.Enabled=true;
				}else{
					MessageBox.Show("El proceso no se iniciará.","Información");
				}
				
				button4.Enabled=true;
				button3.Enabled=false;
				button2.Enabled=false;
				listView1.Enabled=false;
				button7.Enabled=false;
				button6.Enabled=false;
				cargarToolStripMenuItem.Enabled=false;
				}catch(Exception e1){
					MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser iniciado adecuadamente.\n\n Error:\n"+e1,"Información",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}else{
				MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
			}
		}
	 	
		public void finalizar_captura_inc31(){
			
			int i=0;
			string[] reg_cred;
			String linea="";
			
			reg_cred= new string[4];
			err=0;
			aciert=0;
			
			StreamReader rdr = new StreamReader(@"capturator_inc/errores_siscob.txt");
        	StreamReader rdr1 = new StreamReader(@"capturator_inc/aciertos_siscob.txt");
        	
        	/*if(dataGridView1.Columns.Contains("res_cap")==false){
        		dataGridView1.Columns.Add("res_cap","RESULTADO CAPTURA");
        	}

			i=desde;
        	while(i<hasta){
        		dataGridView1.Rows[i].Cells[9].Value="0";
        		i++;
        	}*/
        	
        	i=0;
        	        	      	
        	while(!rdr.EndOfStream){
        		linea=rdr.ReadLine();
        		reg_cred= new string[2];
        		reg_cred = linea.Split('_');
        		
        		i=0;
        		while(i<dataGridView1.RowCount){
        			if((dataGridView1.Rows[i].Cells[1].Value.ToString().Equals(reg_cred[0])==true)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().Equals(reg_cred[1])==true)){
        				dataGridView1.Rows[i].Cells[9].Value="ERROR";
        				i=dataGridView1.RowCount;
        			}
        			i++;
        		}
        	}
        	
        	while(!rdr1.EndOfStream){
        		linea=rdr1.ReadLine();
        		reg_cred= new string[2];
        		reg_cred = linea.Split('_');
        		
        		i=0;
        		while(i<dataGridView1.RowCount){
        			if((dataGridView1.Rows[i].Cells[1].Value.ToString().Equals(reg_cred[0])==true)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().Equals(reg_cred[1])==true)){
        				dataGridView1.Rows[i].Cells[9].Value="CAPTURADO";
        				i=dataGridView1.RowCount;
        			}
        			i++;
        		}
        	}
        	
        	
        	i=desde;
        	while(i<hasta){
        		tabla_results.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[1].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[2].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[3].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[4].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[5].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[6].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[7].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[8].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[9].Value.ToString());
        		i++;
        	}
        	
        	XLWorkbook wb = new XLWorkbook();
        	wb.Worksheets.Add(tabla_results,"Resultados_Captura_LZ");
        	wb.SaveAs(nom_save);
        	
        	conex5.conectar("base_principal");
        	
        	i=0;
        	
        	while(i<dataGridView1.RowCount){
        		guarda_results=conex5.consultar("SELECT capturado_siscob FROM datos_factura WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        		if(guarda_results.Rows.Count>0){
        			reg_cred=guarda_results.Rows[0][0].ToString().Split('/','|');
        			if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("CAPTURADO")==true){
        				conex5.consultar("UPDATE datos_factura SET capturado_siscob=\""+reg_cred[0]+"/"+reg_cred[1]+"|1/"+reg_cred[3]+"\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        				aciert++;
        			}else{
        				if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("ERROR")==true){
        					conex5.consultar("UPDATE datos_factura SET capturado_siscob=\""+reg_cred[0]+"/"+reg_cred[1]+"|2/"+reg_cred[3]+"\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        					err++;
        				}
        			}
        		}
        		i++;
        	}
        	
        	i=desde;
        	while(i<hasta){
        		guarda_results=conex5.consultar("SELECT capturado_siscob FROM datos_factura WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        		if(guarda_results.Rows.Count>0){
        			reg_cred=guarda_results.Rows[0][0].ToString().Split('/','|');
        			if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("CAPTURADO")==true){
        				conex5.consultar("UPDATE datos_factura SET capturado_siscob=\""+reg_cred[0]+"/"+reg_cred[1]+"|"+reg_cred[2]+"/1\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        			}else{
        				if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("ERROR")==true){
        					conex5.consultar("UPDATE datos_factura SET capturado_siscob=\""+reg_cred[0]+"/"+reg_cred[1]+"|"+reg_cred[2]+"/2\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        				}
        			}
        		}
        		i++;
        	}
        	
        	textBox17.Enabled=false;
        	comboBox1.Enabled=true;
        	label19.Enabled=true;
			label20.Enabled=true;
			textBox18.Enabled=true;
			textBox19.Enabled=false;
			textBox20.Enabled=false;
			label22.Enabled=true;
			button4.Enabled=false;
        	panel1.Visible=true;
        	comboBox1.Focus();
        	
		}
		
		public void crea_archivo_captura_cm12(){
			
			int i=0;
			String reg_pat="",credito="",per="",am="",importe="";
			
			//Borrar archivos para comenzar de 0
			if(System.IO.File.Exists(@"capturator_am/temp.txt")==true){
				System.IO.File.Delete(@"capturator_am/temp.txt");
			}
			
			if(System.IO.File.Exists(@"capturator_am/info_core.txt")==true){
				System.IO.File.Delete(@"capturator_am/info_core.txt");
			}
						
			//Abrir archivo
			StreamWriter wr = new StreamWriter(@"capturator_am/temp.txt");
						
			i=desde;
			while(i<hasta){
				//MessageBox.Show(""+dataGridView1.Columns.Count);
				if(dataGridView1.Rows[i].Cells[9].Value.ToString().Contains("POR CAPTURAR")==true){
					reg_pat=dataGridView1.Rows[i].Cells[1].Value.ToString();
					credito=dataGridView1.Rows[i].Cells[2].Value.ToString();
					per=dataGridView1.Rows[i].Cells[3].Value.ToString();
					importe=dataGridView1.Rows[i].Cells[7].Value.ToString();
					am=textBox1.Text;
					am=am.Substring((am.IndexOf('_')+1),am.Length-(am.IndexOf('_')+1));
					
					if(textBox2.Text.Equals("COP")){
						wr.WriteLine(am);
						wr.WriteLine(reg_pat);
						wr.WriteLine(credito);
						wr.WriteLine(per.Substring(4, 2));
						wr.WriteLine(per.Substring(2, 2));
						wr.WriteLine(importe);
						wr.WriteLine("$");
					}else{//SI ES CAPTURA RCV
					/*	wr.WriteLine(reg_pat);
						wr.WriteLine(per.Substring(4, 2));
						wr.WriteLine(per.Substring(2, 2));
						wr.WriteLine(credito);					
						wr.WriteLine(inc);//incidencia
						wr.WriteLine("$");	*/				
					}
				}
				i++;
			}
			
			wr.WriteLine("%&");
			wr.Close();

			StreamWriter wr1 = new StreamWriter(@"capturator_am/temp_aux.txt");
			wr1.WriteLine("0");
			wr1.WriteLine((hasta-desde));
			wr1.Close();
			
			StreamWriter wr3 = new StreamWriter(@"capturator_am/info_core.txt");
			wr3.WriteLine(listView1.Items[listView1.SelectedIndices[0]].Text);
			wr3.Close();
		}
				
		public void enviar_a_captura_cm12(){
			String ruta="",cm="",nom_zip="";
			int nom_dif=0;
			if(textBox2.Text.Equals("COP")){
				if (guardar_resultados()==1){
					try{
						MessageBox.Show("Asegúrese de seguir estos pasos antes de continuar:\n\n"+
						                "1.- Ejecute Siscob y abra la ventana donde se van capturar los datos\n"+
						                "2.- Coloque el cursor en el campo donde se va a comenzar a escribir\n"+
						                "3.- De click al boton Aceptar de este mensaje","ATENCIÓN",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
						
						DialogResult respuesta = MessageBox.Show("Está a punto de comenzar el proceso de captura automática.\n"+
						                                         "Los Archivos Temporales de la Captura Anterior se Borrarán y no podrá recuperarlos si es que estos NO fueron guardados.\n"+
						                                         "Una vez comenzada la captura NO se deberá manipular el equipo\n"+
						                                         "hasta que haya finalizado el proceso de captura.\n"+
						                                         "El programa le informará cuando el proceso de captura haya concluido\n\n"+
						                                         "Teclas Especiales:\n"+
						                                         "[PAUSA] - Pausa el proceso de captura (no se recomienda)\n"+
						                                         "[INICIO] - Reanuda el proceso de captura\n"+
						                                         "[FIN] - Finaliza el proceso de captura\n\n"+
						                                         "¿Desea comenzar el proceso de captura?","CONFIRMAR",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
						
						if(respuesta ==DialogResult.Yes){
							
							
							/*
							System.IO.File.Copy(@"capturator_am/temp.txt",@"capturator_am/respaldo/temp.txt",true);
							System.IO.File.Copy(@"capturator_am/errores_siscob.txt",@"capturator_am/respaldo/errores_siscob.txt",true);
							System.IO.File.Copy(@"capturator_am/acierto_siscob.txt",@"capturator_am/respaldo/acierto_siscob.txt",true);
							System.IO.File.Copy(@"capturator_am/aciertos_siscob.txt",@"capturator_am/respaldo/aciertos_siscob.txt",true);
							System.IO.File.Copy(@"capturator_am/info_core.txt",@"capturator_am/respaldo/info_core.txt",true);
							*/
							
							//Respaldar Archivos de Resultados Previos
							StreamReader rdra = new StreamReader(@"capturator_am/info_core.txt");
							nom_zip=rdra.ReadLine();
							rdra.Close();
							
							try{
								ZipFile arch = new ZipFile();
								arch.AddFile(@"capturator_am/temp.txt","");
								arch.AddFile(@"capturator_am/errores_siscob.txt","");
								arch.AddFile(@"capturator_am/acierto_siscob.txt","");
								arch.AddFile(@"capturator_am/aciertos_siscob.txt","");
								arch.AddFile(@"capturator_am/info_core.txt","");
								
								if(nom_zip.Length>0){
								}else{
									nom_zip="Respaldo_CM12";
								}
								
								if(File.Exists(@"capturator_am/respaldo/"+nom_zip+".LZ")==true){
									while(File.Exists(@"capturator_am/respaldo/"+nom_zip+"_"+nom_dif+".LZ")==true){
										nom_dif++;
									}
									nom_zip=nom_zip+"_"+nom_dif+".LZ";
								}else{
									nom_zip=nom_zip+".LZ";
								}
								//arch.Save(arch_lz40);
								arch.Save(@"capturator_am/respaldo/"+nom_zip);
								//MessageBox.Show("El archivo se guardó correctamente.");
								
							}catch(Exception es){
								MessageBox.Show("Ocurrió el siguiente problema al crear el respaldo del último envío:\n\n"+es);
							}
							
							
							//crear archivo que se va a capturar
							crea_archivo_captura_cm12();
							
							MessageBox.Show("El proceso iniciará cuando de click en Aceptar","Información");
							//Borrar archivos para comenzar de 0
							System.IO.File.Delete(@"capturator_am/errores_siscob.txt");
							System.IO.File.Delete(@"capturator_am/acierto_siscob.txt");
							System.IO.File.Delete(@"capturator_am/aciertos_siscob.txt");
							//Crear archivos nuevos
							fichero = System.IO.File.Create(@"capturator_am/errores_siscob.txt");
							fichero1= System.IO.File.Create(@"capturator_am/acierto_siscob.txt");
							fichero2= System.IO.File.Create(@"capturator_am/aciertos_siscob.txt");
							ruta = fichero.Name;
							fichero.Close();
							fichero1.Close();
							fichero2.Close();
							
							StreamWriter wr1 = new StreamWriter(@"capture_am.bat");
							wr1.WriteLine("@echo off");
							wr1.WriteLine("C:");
							wr1.WriteLine("cd "+ruta.Substring(0,(ruta.Length-19)));
							
							if(textBox2.Text.Equals("COP")){
								wr1.WriteLine("start bumblecapt.exe");
							}else{
								//wr1.WriteLine("start bumblecapt_rcv.exe");
							}
							wr1.Close();
							
							tipo_env="CM12";
							cm=textBox1.Text;
							cm=cm.Substring((cm.IndexOf('_')+1),cm.Length-(cm.IndexOf('_')+1));
							System.Diagnostics.Process.Start(@"capture_am.bat");
							conex5.guardar_evento("Se Mando a capturar en Siscob "+dataGridView1.RowCount.ToString()+" Registros de Clave de Movimiento "+cm+" de "+textBox2.Text);
							button3.Enabled=false;
							button4.Enabled=true;
						}else{
							MessageBox.Show("El proceso no se iniciará.","Información");
						}
					}catch(Exception e1){
						MessageBox.Show(" Algo salio mal.\n El proceso no pudo ser iniciado adecuadamente.\n\n Error:\n"+e1,"Información",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}else{
					MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
				}
			}else{
				MessageBox.Show("Debido a que se requiere el desglose de los importes, el envío a CM12 de RCV no puede realizarse desde esta ventana.\n Para realizar en envío utilice la siguiente ventana..","Información",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				Autocob cm12rcv = new Autocob();
				cm12rcv.Show();
				this.Hide();
			}
		}
		
		public void finalizar_captura_cm12(){
			
			int i=0;
			string[] reg_cred;
			String linea="";
			
			reg_cred= new string[4];
			err=0;
			aciert=0;
			
			StreamReader rdr = new StreamReader(@"capturator_am/errores_siscob.txt");
        	StreamReader rdr1 = new StreamReader(@"capturator_am/aciertos_siscob.txt");
        	
        	/*if(dataGridView1.Columns.Contains("res_cap")==false){
        		dataGridView1.Columns.Add("res_cap","RESULTADO CAPTURA");
        	}

			i=desde;
        	while(i<hasta){
        		dataGridView1.Rows[i].Cells[9].Value="0";
        		i++;
        	}*/
        	
        	i=0;
        	        	      	
        	while(!rdr.EndOfStream){
        		linea=rdr.ReadLine();
        		reg_cred= new string[2];
        		reg_cred = linea.Split('_');
        		
        		i=0;
        		while(i<dataGridView1.RowCount){
        			if((dataGridView1.Rows[i].Cells[1].Value.ToString().Equals(reg_cred[0])==true)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().Equals(reg_cred[1])==true)){
        				dataGridView1.Rows[i].Cells[9].Value="ERROR";
        				i=dataGridView1.RowCount;
        			}
        			i++;
        		}
        	}
        	
        	while(!rdr1.EndOfStream){
        		linea=rdr1.ReadLine();
        		reg_cred= new string[2];
        		reg_cred = linea.Split('_');
        		
        		i=0;
        		while(i<dataGridView1.RowCount){
        			if((dataGridView1.Rows[i].Cells[1].Value.ToString().Equals(reg_cred[0])==true)&&(dataGridView1.Rows[i].Cells[2].Value.ToString().Equals(reg_cred[1])==true)){
        				dataGridView1.Rows[i].Cells[9].Value="CAPTURADO";
        				i=dataGridView1.RowCount;
        			}
        			i++;
        		}
        	}
        	
        	i=desde;
        	while(i<hasta){
        		tabla_results.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[1].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[2].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[3].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[4].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[5].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[6].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[7].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[8].Value.ToString(),
        		                       dataGridView1.Rows[i].Cells[9].Value.ToString());
        		i++;
        	}
        	
        	XLWorkbook wb = new XLWorkbook();
        	wb.Worksheets.Add(tabla_results,"Resultados_Captura_LZ");
        	wb.SaveAs(nom_save);
        	
        	conex5.conectar("base_principal");
        	
        	i=0;
        	
        	i=0;
        	
        	while(i<dataGridView1.RowCount){
        		guarda_results=conex5.consultar("SELECT captura_cm12 FROM datos_factura WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        		if(guarda_results.Rows.Count>0){
        			reg_cred=guarda_results.Rows[0][0].ToString().Split('/','|');
        			if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("CAPTURADO")==true){
        				conex5.consultar("UPDATE datos_factura SET captura_cm12=\"1/"+reg_cred[1]+"\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        				aciert++;
        			}else{
        				if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("ERROR")==true){
        					conex5.consultar("UPDATE datos_factura SET captura_cm12=\"2/"+reg_cred[1]+"\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_cuotas=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        					err++;
        				}
        			}
        		}
        		
        		i++;
        	}
        	
        	i=desde;
        	while(i<hasta){
        		guarda_results=conex5.consultar("SELECT captura_cm12 FROM datos_factura WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        		if(guarda_results.Rows.Count>0){
        			reg_cred=guarda_results.Rows[0][0].ToString().Split('/','|');
        			if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("CAPTURADO")==true){
        				conex5.consultar("UPDATE datos_factura SET captura_cm12=\""+reg_cred[0]+"/1\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        			}else{
        				if(dataGridView1.Rows[i].Cells[9].Value.ToString().Equals("ERROR")==true){
        					conex5.consultar("UPDATE datos_factura SET captura_cm12=\""+reg_cred[0]+"/2\" WHERE registro_patronal2=\""+dataGridView1.Rows[i].Cells[1].Value.ToString()+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[2].Value.ToString()+"\"");
        				}
        			}
        		}
        		
        		i++;
        	}
        	
        	textBox17.Enabled=false;
        	comboBox1.Enabled=true;
        	label19.Enabled=true;
			label20.Enabled=true;
			textBox18.Enabled=true;
			textBox19.Enabled=false;
			textBox20.Enabled=false;
			label22.Enabled=true;
			button4.Enabled=false;
        	panel1.Visible=true;
        	comboBox1.Focus();
		}
		
		void Envio_31Load(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			string area ="";
			int rango=0;
			
			inicio_carga_cores();
			
			tabla_detalle_core_orde.Columns.Add("ASUNTO");
			tabla_detalle_core_orde.Columns.Add("REGISTRO PATRONAL");
            tabla_detalle_core_orde.Columns.Add("CREDITO");
            tabla_detalle_core_orde.Columns.Add("PERIODO");
            tabla_detalle_core_orde.Columns.Add("TD");
 			tabla_detalle_core_orde.Columns.Add("INC");
            tabla_detalle_core_orde.Columns.Add("SUB");
           	tabla_detalle_core_orde.Columns.Add("IMPORTE");
            tabla_detalle_core_orde.Columns.Add("FECHA NOTIFICACIÓN");
            tabla_detalle_core_orde.Columns.Add("RESULTADO CAPTURA");
            
			tabla_detalle_core_orde2.Columns.Add("ASUNTO");
			tabla_detalle_core_orde2.Columns.Add("REGISTRO PATRONAL");
            tabla_detalle_core_orde2.Columns.Add("CREDITO");
            tabla_detalle_core_orde2.Columns.Add("PERIODO");
            tabla_detalle_core_orde2.Columns.Add("TD");
 			tabla_detalle_core_orde2.Columns.Add("INC");
            tabla_detalle_core_orde2.Columns.Add("SUB");
           	tabla_detalle_core_orde2.Columns.Add("IMPORTE");
            tabla_detalle_core_orde2.Columns.Add("FOLIO PAGO");
            tabla_detalle_core_orde2.Columns.Add("RESULTADO CAPTURA");
                         
            tabla_results.Columns.Add("ASUNTO");
			tabla_results.Columns.Add("REGISTRO PATRONAL");
            tabla_results.Columns.Add("CREDITO");
            tabla_results.Columns.Add("PERIODO");
            tabla_results.Columns.Add("TD");
 			tabla_results.Columns.Add("INC");
            tabla_results.Columns.Add("SUB");
           	tabla_results.Columns.Add("IMPORTE");
            tabla_results.Columns.Add("FECHA NOTIFICACIÓN");
            tabla_results.Columns.Add("RESULTADO CAPTURA");
            
            user_id = MainForm.datos_user_static[7];
            rango=Convert.ToInt32(MainForm.datos_user_static[2]);
            area=MainForm.datos_user_static[6];
            
            if(area.Equals("Registros")){
            	button3.Enabled=true;
            	button7.Enabled=true;
            }else{
            	if(rango==0){
            		button3.Enabled=true;
            		button7.Enabled=true;
            	}
            }
            
            conex6.conectar("base_principal");
            data_query_comple = conex6.consultar("SELECT registro_patronal,credito,periodo,incidencia,td FROM rale WHERE (incidencia = \"02\" OR incidencia = \"2\" OR incidencia = \"01\" OR incidencia = \"1\")");
			toolTip1.SetToolTip(textBox7, textBox7.Text);
		}
		
		void ListView1ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Determine if clicked column is already the column that is being sorted.
			if ( e.Column == ordena.SortColumn )
			{
				// Reverse the current sort direction for this column.
				if (ordena.Order == SortOrder.Ascending)
				{
					ordena.Order = SortOrder.Descending;
				}
				else
				{
					ordena.Order = SortOrder.Ascending;
				}
			}
			else
			{
				// Set the column number that is to be sorted; default to ascending.
				ordena.SortColumn = e.Column;
				ordena.Order = SortOrder.Ascending;
			}

			// Perform the sort with these new sort options.
			this.listView1.Sort();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(listView1.View == View.Details){
				listView1.View = View.Tile;
				button1.Image= global::Nova_Gear.Properties.Resources.application_view_tile;
			}else{
				if(listView1.View == View.Tile){
					listView1.View= View.LargeIcon;
					button1.Image= global::Nova_Gear.Properties.Resources.application_view_detail;
				}else{
					if(listView1.View == View.LargeIcon){
						listView1.View = View.Details;
						button1.Image= global::Nova_Gear.Properties.Resources.application_view_icons;
					}
					
				}
			}
		}
		
		void ListView1DoubleClick(object sender, EventArgs e)
		{
			
		}
		
		void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
		{
			
		}
		
		void ListView1ItemActivate(object sender, EventArgs e)
		{/*
			int indi=0;
			String nombre="";
			if(listView1.Items.Count>0){
				if(listView1.Items.Count>0){
					if(listView1.SelectedIndices.Count>0){
						//MessageBox.Show("indice clickeado: "+listView1.SelectedIndices[0]);
						
						nombre=listView1.Items[listView1.SelectedIndices[0]].Text;
						MessageBox.Show(nombre);
						cargar_info_core(listView1.SelectedIndices[0]);
					}
				}	
			}*/
		}
		
		void VerInformaciónToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(listView1.Items.Count>0){
				if(listView1.Items.Count>0){
					if(listView1.SelectedIndices.Count>0){
						//MessageBox.Show("indice clickeado: "+listView1.SelectedIndices[0]);
						cargar_info_core(corrige_indice(listView1.SelectedIndices[0]));
					}
				}	
			}			
		}
		
		void ListView1SelectedIndexChanged(object sender, EventArgs e)
		{
			
			if(listView1.Items.Count>0){
				if(listView1.Items.Count>0){
					if(listView1.SelectedIndices.Count>0){
						//MessageBox.Show("indice clickeado: "+listView1.SelectedIndices[0]);
										
						cargar_info_core(corrige_indice(listView1.SelectedIndices[0]));
					}
				}	
			}
			
		}
		
		void CargarToolStripMenuItemClick(object sender, EventArgs e)
		{
			cargar_core();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			cargar_core();
		}
		//enviar a captura
		void Button3Click(object sender, EventArgs e)
		{
			//if(textBox1.Text.Contains("INC")==true){
				if((textBox8.Text.Contains("CAPTURA TOTAL")==false)){
					if(dataGridView1.RowCount > 0){
						textBox19.Text="1";
						textBox20.Text=""+dataGridView1.RowCount;
						label19.Enabled=true;
						textBox17.Enabled=true;
						textBox17.ReadOnly=false;
						textBox18.Enabled=false;
						label21.Enabled=true;
						
						if(textBox17.Text.Length>5){
							textBox17.ReadOnly=true;
							label21.Enabled=true;
							textBox19.Enabled=true;
							textBox20.Enabled=true;
							textBox19.Focus();
						}
						
						panel1.Visible=true;
						textBox17.Focus();
					}else{
						MessageBox.Show("Para Continuar Cargue una CORE","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
				}else{
					MessageBox.Show("La CORE ya fue capturada en su totalidad.\nNo se Puede Enviar a Capturar esta CORE.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			//}
			
		}
		//resultados parciales
		void Button4Click(object sender, EventArgs e)
		{
			if(tipo_env=="INC31"){
				finalizar_captura_inc31();
			}else{
				if(tipo_env=="CM12"){
					finalizar_captura_cm12();
				}
			}
		}
		//finalizar
		void FinToolStripMenuItem1Click(object sender, EventArgs e)
		{
			if(tipo_env=="INC31"){
				finalizar_captura_inc31();
			}else{
				if(tipo_env=="CM12"){
					finalizar_captura_cm12();
				}
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			int pasa=0;
			String estatus="";
			if(textBox17.Enabled==true){
				if(textBox17.Text.Length>5){
					panel1.Visible=false;
					label19.Enabled=false;
					textBox17.Enabled=false;
					
					if(int.TryParse(textBox19.Text,out desde)){
						desde=(Convert.ToInt32(textBox19.Text)-1);
						pasa++;
					}
					
					if(int.TryParse(textBox20.Text,out hasta)){
						hasta=Convert.ToInt32(textBox20.Text);
						pasa++;
					}
					
					if(pasa==2){
						if(desde<hasta){
							pasa++;
						}
						
						if(desde<dataGridView1.RowCount && hasta<=dataGridView1.RowCount){
							pasa++;
						}
					}
					
					if(pasa==4){
						if(textBox1.Text.Contains("INC")==true){
							enviar_a_captura_inc31();
						}else{
							if(textBox1.Text.Contains("CM")==true){
								enviar_a_captura_cm12();
							}
						}
					}else{
						MessageBox.Show("El Rango de los Casos a Capturar es Incorrecto","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}
					
				}else{
					MessageBox.Show("Si no se Proporciona un Folio de Control Válido NO se podrá Proceder con la Captura Automática.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					panel1.Visible=false;
					label19.Enabled=false;
					textBox17.Enabled=false;
					textBox17.Text="";
				}
			}else{
				if(comboBox1.Enabled==true){
					
					String fecha="";
					fecha=System.DateTime.Today.ToShortDateString();
					fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
					
					if(comboBox1.SelectedIndex>-1){
						if(comboBox1.SelectedIndex==0){
							estatus="CAPTURA TOTAL";
						}
						
						if(comboBox1.SelectedIndex==1){
							estatus="CAPTURA PARCIAL";
						}
						
						if(comboBox1.SelectedIndex==2){
							estatus="CAPTURA FALLIDA";
						}
					}else{
						estatus="CAPTURA TOTAL";
					}
						
					if(textBox18.Text.Length>10){
						observaciones=textBox18.Text;
					}
										
					try{
						conex5.consultar("UPDATE info_cores SET fecha_afectacion=\""+fecha+"\",usuario_afecta="+user_id+",num_aciertos="+aciert+",num_errores="+err+",folio_control_registros=\""+textBox17.Text+"\",estatus=\""+estatus+"\",observaciones=\""+observaciones+"\" WHERE id_core="+id_core+"");
						panel1.Visible=false;
						label19.Enabled=false;
						textBox17.Enabled=false;
						MessageBox.Show("La información de la CORE Fue Capturada Correctamente","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        				button2.Enabled=true;
        				button3.Enabled=true;
        				button4.Enabled=false;
        				listView1.Enabled=true;
        				button7.Enabled=true;
        				button6.Enabled=true;
        				cargarToolStripMenuItem.Enabled=true;
        				comboBox1.SelectedIndex=-1;
        				textBox18.Text="";
        				inicio_carga_cores();
					}catch(Exception esa){
        				MessageBox.Show("Ocurrió un error al momento de guardar la información de la CORE","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
        			}
					comboBox1.Enabled=false;
				}
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			/*if(comboBox1.SelectedIndex==3){
				textBox18.Enabled=true;
				label22.Enabled=true;
				textBox18.Focus();
			}*/
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			inicio_carga_cores();
		}
		//leer resultados existentes
		void Button7Click(object sender, EventArgs e)
		{
			String nom_core="";
			if(textBox1.Text=="INC_31"){
				if(dataGridView1.RowCount>0){
					try{
						StreamReader sr0 = new StreamReader(@"capturator_inc/info_core.txt");
						nom_core=sr0.ReadLine();
						sr0.Close();
					}catch(Exception eds){
						MessageBox.Show("No se Encontró el archivo de información del Último envío","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
					
					if(nom_core.Equals(listView1.Items[listView1.SelectedIndices[0]].Text)){
						DialogResult res=MessageBox.Show("Se Leeran los ultimos resultados de envío de la CORE: "+nom_core+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
						if(res==DialogResult.Yes){
							if(guardar_resultados()==1){
								finalizar_captura_inc31();
							}else{
								MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
							}
						}
						
					}else{
						MessageBox.Show("No se Encontraron Archivos de Resultados de la CORE seleccionada","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}else{
					MessageBox.Show("Para Continuar Cargue una CORE","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}else{
				if(textBox1.Text=="CM_12"){
					if(dataGridView1.RowCount>0){
					try{
						StreamReader sr0 = new StreamReader(@"capturator_am/info_core.txt");
						nom_core=sr0.ReadLine();
						sr0.Close();
					}catch(Exception eds){
						MessageBox.Show("No se Encontró el archivo de información del Último envío","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
					
					if(nom_core.Equals(listView1.Items[listView1.SelectedIndices[0]].Text)){
						DialogResult res=MessageBox.Show("Se Leeran los ultimos resultados de envío de la CORE: "+nom_core+"\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
						if(res==DialogResult.Yes){
							if(guardar_resultados()==1){
								finalizar_captura_cm12();
							}else{
								MessageBox.Show("No podrá continuar si no proporciona un nombre para el archivo donde se guardarán los resultados del proceso", "Nova Gear - Captura Siscob");
							}
						}
						
					}else{
						MessageBox.Show("No se Encontraron Archivos de Resultados de la CORE seleccionada","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}else{
					MessageBox.Show("Para Continuar Cargue una CORE","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
				}
			}
		}
		
	}
}
 
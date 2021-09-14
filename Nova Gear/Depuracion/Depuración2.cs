/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 20/06/2017
 * Hora: 09:31 a.m.
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
using Microsoft.SqlServer.Types;
using DocumentFormat.OpenXml.Drawing.Charts;
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;

namespace Nova_Gear.Depuracion
{
	/// <summary>
	/// Description of Depuración2.
	/// </summary>
	public partial class Depuración2 : Form
	{
		public Depuración2()
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
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		Conexion conex5 = new Conexion();

		//Data Tables
		System.Data.DataTable dt = new System.Data.DataTable();
		System.Data.DataTable data_acumulador = new System.Data.DataTable();
		System.Data.DataTable data_pro_sip = new System.Data.DataTable();
		System.Data.DataTable data_depu_nova = new System.Data.DataTable();
		System.Data.DataTable data_pagos_repe = new System.Data.DataTable();
		System.Data.DataTable data_guardado = new System.Data.DataTable();
		System.Data.DataTable data_solo_pago = new System.Data.DataTable();
		System.Data.DataTable data_reporte = new System.Data.DataTable();
		System.Data.DataTable consultamysql = new System.Data.DataTable();
		System.Data.DataTable data_rale = new System.Data.DataTable();		

		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;

		//Declaracion de elementos para conexion office num 2
		OleDbConnection conexion1 = null;
		DataSet dataSet1 = null;
		OleDbDataAdapter dataAdapter1 = null;

		//Declaracion del Hilo para ejecutar un subproceso
		private Thread hilosecundario = null;
        
        //Declarar variables opciones
        String verif_rale = "", pago_min = "";

		public void llenar_Cb1() {
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			int i = 0;
			dataGridView1.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE nombre_periodo LIKE \"%_ECO_%\" ORDER BY nombre_periodo;");
			do {
				comboBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
				i++;
			} while (i < dataGridView1.RowCount);
			i = 0;
			conex.cerrar();
			//dataGridView1.Rows.Clear();
			//dataGridView1.Columns.Clear();
		}

		public void cargar_archivos(int tipo_carga) {
			String archivo;
			OpenFileDialog dialog = new OpenFileDialog();

			//Procesar
			if (tipo_carga == 1) {
				dialog.Title = "Seleccione el archivo PROCESAR";//le damos un titulo a la ventana
				dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
																				 //solo los archivos excel
			} else {
				if (tipo_carga == 2) {
					dialog.Title = "Seleccione el archivo SIPARE";//le damos un titulo a la ventana
					dialog.Filter = "Archivos de Excel (*.xls *.xlsx *.csv)|*.xls;*.xlsx;*.csv"; //le indicamos el tipo de filtro en este caso que busque
																								 //solo los archivos excel
				}
			}

			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

			//si al seleccionar el archivo damos Ok
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				archivo = dialog.FileName;
				dataGridView2.Rows.Add((dataGridView2.Rows.Count + 1), archivo, tipo_carga);
				/*if((archivo.Substring(archivo.Length-3,3).Equals("lsx")==true)||archivo.Substring(archivo.Length-3,3).Equals("xls")==true)
  					{
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" +archivo + "';Extended Properties=Excel 12.0;";
					conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
					conexion.Open(); //abrimos la conexion
					carga_chema_excel();
				}else{
							    
				}*/
			}
		}

		public void carga_chema_excel() {
			int i = 0;
			String tabla = "";
			comboBox3.Items.Clear();
			dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

			do {
				if (dt.Rows[i][3].ToString().Length > 0) {
					if ((dt.Rows[i][3].ToString()).Equals("TABLE") == true) {
						tabla = dt.Rows[i][2].ToString();
						if ((tabla.Substring((tabla.Length - 1), 1)).Equals("$")) {
							tabla = tabla.Remove((tabla.Length - 1), 1);
							comboBox3.Items.Add(tabla);
						}
					}
				}
				i++;
			} while (i < dt.Rows.Count);

			//MessageBox.Show(""+i+"|"+dt.Rows.Count);
			dt.Clear();
		}

		public void leer_xls(String archivo, int num_hoja, int tipo_lect) {

			String hoja = "", cons_exc = "", cad_con, periodo = "", reg_pat = "";
			int per_num = 0, n = 0, tot_rows_sql = 0, rcv = 0;

			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
			conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion.Open(); //abrimos la conexion

			Invoke(new MethodInvoker(delegate {
				periodo = comboBox1.SelectedItem.ToString();
			}));
			if (periodo.StartsWith("RCV")) {
				per_num = Convert.ToInt32(periodo.Substring(periodo.Length - 2, 2));
				per_num = per_num * 2;
				if (per_num > 9) {
					periodo = periodo.Substring(0, periodo.Length - 2) + per_num.ToString();
				} else {
					periodo = periodo.Substring(0, periodo.Length - 2) + "0" + per_num.ToString();
				}

			}
			periodo = periodo.Substring(periodo.Length - 6, 6);
			//MessageBox.Show(periodo);


			if (tipo_lect != 1) {//si no es procesar
				carga_chema_excel();
			}

			Invoke(new MethodInvoker(delegate {
				hoja = comboBox3.Items[num_hoja].ToString();
			}));

			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
			{
				if (tipo_lect == 1) {//procesar
					cons_exc = "Select [NRP],[F#SUA],[FECHA PAGO],[4SS],[RCV] from [" + hoja + "$] where [PERIODO PAGO] like \"" + periodo + "\" and [DIAG 1] <> 146 and [DIAG 1] <> 857";
				} else {
					if (tipo_lect == 2) {//sipare
						cons_exc = "Select [Registro Patronal],[Linea de Captura (LC)],[Importe IMSS],[Importe RCV],[Periodo de Pago] from [" + hoja + "$] where [Periodo de Pago] like \"" + periodo + "\" ";
						//MessageBox.Show(cons_exc);
					} else {
						if (tipo_lect == 3) {//gen pags
							cons_exc = "Select [REGISTRO],[RC_NUM_FOL],[RC_FEC_MOV],[RC_IMP_TOT],[RC_PER],[RC_CRED] from [" + hoja + "$] where [RC_PER] like \"" + periodo.Substring(0, 4) + "/" + periodo.Substring(4, 2) + "\" and ([RC_MOD] = 10 or [RC_MOD] = 13 or [RC_MOD] = 17) and ([RC_DOC] = 1 or [RC_DOC] = 2)";
						}
					}
				}
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet

					if (dataAdapter.Equals(null)) {

						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\ndel Archivo: " + archivo, "Error al Abrir Archivo de Excel/");

					} else {
						if (dataAdapter == null) { } else {
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
							Invoke(new MethodInvoker(delegate {
								dataGridView1.DataSource = dataSet.Tables[0];
								tot_rows_sql = dataGridView1.RowCount;  //le asignamos al DataGridView el contenido del dataSet
							}));
							if (tipo_lect == 1) {//procesar
								while (n < tot_rows_sql) {
									Invoke(new MethodInvoker(delegate {
										reg_pat = "";
										if (dataGridView1.Rows[n].Cells[0].Value.ToString().StartsWith("40") == true) {
											reg_pat = "0" + dataGridView1.Rows[n].Cells[0].Value.ToString();
										} else {
											reg_pat = dataGridView1.Rows[n].Cells[0].Value.ToString();
										}
										data_acumulador.Rows.Add(reg_pat.Substring(0, 10),
									dataGridView1.Rows[n].Cells[1].Value.ToString(),
									dataGridView1.Rows[n].Cells[2].Value.ToString(),
									dataGridView1.Rows[n].Cells[3].Value.ToString(),
									dataGridView1.Rows[n].Cells[4].Value.ToString(), periodo, archivo);
									}));
									n++;
								}
							} else {
								if (tipo_lect == 2) {//sipare
													 //MessageBox.Show(""+tot_rows_sql);
									while (n < tot_rows_sql) {
										Invoke(new MethodInvoker(delegate {
											reg_pat = "";
											if (dataGridView1.Rows[n].Cells[0].Value.ToString().StartsWith("40") == true) {
												reg_pat = "0" + dataGridView1.Rows[n].Cells[0].Value.ToString();
											} else {
												reg_pat = dataGridView1.Rows[n].Cells[0].Value.ToString();
											}

											data_acumulador.Rows.Add(reg_pat.Substring(0, 10),
											dataGridView1.Rows[n].Cells[1].Value.ToString(), " ",
											dataGridView1.Rows[n].Cells[2].Value.ToString(),
											dataGridView1.Rows[n].Cells[3].Value.ToString(),
											dataGridView1.Rows[n].Cells[4].Value.ToString(), archivo);
										}));
										n++;
									}

									if (tot_rows_sql == 0)
									{
										MessageBox.Show("El número de periodo del Archivo SIPARE no coincide con el número de periodo a Depurar por lo que no se cargó ningún crédito del Archivo.\nPERIODO REQUERIDO: " + periodo, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									}
								} else {
									if (tipo_lect == 3) {//gen pags
										while (n < tot_rows_sql) {
											Invoke(new MethodInvoker(delegate {
												reg_pat = "";
												if (dataGridView1.Rows[n].Cells[0].Value.ToString().StartsWith("40") == true) {
													reg_pat = "0" + dataGridView1.Rows[n].Cells[0].Value.ToString();
												} else {
													reg_pat = dataGridView1.Rows[n].Cells[0].Value.ToString();
												}
												//parar
												data_acumulador.Rows.Add(reg_pat.Substring(0, 10),
															 dataGridView1.Rows[n].Cells[1].Value.ToString(),//folio
															 dataGridView1.Rows[n].Cells[2].Value.ToString().Substring(0, 10),//fech
															 dataGridView1.Rows[n].Cells[3].Value.ToString(), "0.00",//imp_tot,imp_rcv=0 no hay pagos rcv en gral. pagos
															 dataGridView1.Rows[n].Cells[4].Value.ToString(), archivo,
															 dataGridView1.Rows[n].Cells[5].Value.ToString());//CREDITO SOLO GP
											}));
											n++;
										}
									}
								}
							}

							//data_acumulador.Merge(dataSet.Tables[0]);
							conexion.Close();//cerramos la conexion
											 //eliminamos la ultima fila del datagridview que se autoagrega

						}
					}
				}
				catch (OleDbException oleex)
				{
					//en caso de haber una excepcion que nos mande un mensaje de error
					MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n\n" + oleex, "Error al Abrir Archivo de Excel");
				}

			}
		}

		public void leer_pro_xls(int indice) {

			int i = 0, j = 0, items_count = 0;
			String cad_con = "";
			//MessageBox.Show("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" +dataGridView2.Rows[indice].Cells[1].Value.ToString() + "';Extended Properties=Excel 12.0;");
			Invoke(new MethodInvoker(delegate {
				cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dataGridView2.Rows[indice].Cells[1].Value.ToString() + "';Extended Properties=Excel 12.0;";
			}));
			conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion.Open(); //abrimos la conexion
			carga_chema_excel();
			conexion.Close();
			Invoke(new MethodInvoker(delegate {
				items_count = comboBox3.Items.Count;
			}));
			while (i < items_count) {

				try
				{
					Invoke(new MethodInvoker(delegate {
						leer_xls(dataGridView2.Rows[indice].Cells[1].Value.ToString(), i, 1);
					}));
				} catch (Exception)
				{
				}
				i++;
			}
		}

		public void leer_sip_csv(int indice) {
			StreamReader rdr;
			int i = 0, err = 0,per_num=0;
			String csv_line = "", arch, periodo = "",period_sipa="", period_sipa_err="";
			string[] csv_pre;

			arch = dataGridView2.Rows[indice].Cells[1].Value.ToString();
			//MessageBox.Show(arch);
			rdr = new StreamReader(arch);
			//MessageBox.Show(data_acumulador.Rows.Count.ToString());

			Invoke(new MethodInvoker(delegate {
				periodo = comboBox1.SelectedItem.ToString();
			}));
			if (periodo.StartsWith("RCV"))
			{
				per_num = Convert.ToInt32(periodo.Substring(periodo.Length - 2, 2));
				per_num = per_num * 2;
				if (per_num > 9)
				{
					periodo = periodo.Substring(0, periodo.Length - 2) + per_num.ToString();
				}
				else
				{
					periodo = periodo.Substring(0, periodo.Length - 2) + "0" + per_num.ToString();
				}

			}
			period_sipa = periodo.Substring(periodo.Length - 6, 6);

			//MessageBox.Show(period_sipa);
			while (!(rdr.EndOfStream)) {
				csv_line = rdr.ReadLine();
				csv_line = csv_line.Replace('"', ' ').Trim();
				csv_pre = csv_line.Split(',');
				if (i > 0) {

					
					if (csv_pre[1] == period_sipa)
					{
						data_acumulador.Rows.Add(csv_pre[0].Substring(0, 10), csv_pre[2], " ", Convert.ToDecimal(csv_pre[10]), Convert.ToDecimal(csv_pre[11]), csv_pre[1], arch);
					}
					else
					{
						err = 1;
						period_sipa_err = csv_pre[1];
						rdr.ReadToEnd();
					}

				}
				i++;
			}

			if (err == 1)
			{
				MessageBox.Show("El número de periodo del Archivo SIPARE no coincide con el número de periodo a Depurar por lo que no se cargó ningún crédito del Archivo.\nPERIODO REQUERIDO: "+period_sipa+ "\nPERIODO ARCHIVO SIPARE: "+period_sipa_err, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			//MessageBox.Show(data_acumulador.Rows.Count.ToString());
		}

		public void leer_archivos() {

			int i = 0, progreso = 0, tot_rows = 0;
			String archivo = "", tipo_lec = "";
			data_acumulador.Rows.Clear();
			Invoke(new MethodInvoker(delegate {
				tot_rows = dataGridView2.Rows.Count; }));
			while (i < tot_rows) {
				Invoke(new MethodInvoker(delegate {
					archivo = dataGridView2.Rows[i].Cells[1].Value.ToString();
					tipo_lec = dataGridView2.Rows[i].Cells[2].Value.ToString();
					progreso = Convert.ToInt32((((i) * 100) / tot_rows) / 2);
					label4.Text = "" + progreso + "%";
					progressBar1.Value = progreso;
					label5.Text = "Leyendo archivos: " + (i + 1) + " de " + tot_rows;
				}));

				if (archivo.Substring(archivo.Length - 3, 3).Equals("xls") == true || archivo.Substring(archivo.Length - 3, 3).Equals("lsx") == true) {
					if (tipo_lec.Equals("1") == true) {//si es archivo procesar
						leer_pro_xls(i);
					} else {
						if (tipo_lec.Equals("2") == true) {//si es archivo sipare
							leer_xls(archivo, 0, 2);
						} else {
							if (tipo_lec.Equals("3") == true) {//si es archivo GP
								leer_xls(archivo, 0, 3);
							}
						}

					}
				} else {
					if (archivo.Substring(archivo.Length - 3, 3).Equals("csv") == true) {
						leer_sip_csv(i);
					}
				}
				i++;

				Invoke(new MethodInvoker(delegate {

				}));

			}

			System.IO.File.Delete(@"temporal_combinado.xlsx");
			XLWorkbook wb0 = new XLWorkbook();
			wb0.Worksheets.Add(data_acumulador, "hoja_lz");
			wb0.SaveAs(@"temporal_combinado.xlsx");

			wb0.Dispose();
			//MessageBox.Show("Registros Recopilados: "+data_acumulador.Rows.Count);
		}

		public int depu_manual() {
			int tipo_depu_manu = 0, error_con_lista = 0, tot_rows = 0;
			String periodo_sel = "", cad_con, cons_exc, primera_linea = "";
			bool rb2 = false, rb3 = false, rb4 = false, rb5 = false;


			Invoke(new MethodInvoker(delegate
										 {
											 rb2 = radioButton2.Checked;
											 rb3 = radioButton3.Checked;
											 rb4 = radioButton4.Checked;
											 rb5 = radioButton5.Checked;

											 if (rb5 == false) {//RB
												 periodo_sel = comboBox1.SelectedItem.ToString();
											 }
										 }));

			if (rb2 == true) {
				//SI ES DEPURACION MANUAL

				if (rb5 == false) {
					if (rb3 == true) {//Todo
						tipo_depu_manu = 1;
						Invoke(new MethodInvoker(delegate
										 {
											 periodo_sel = comboBox1.SelectedItem.ToString();
										 }));
					}

					if (rb4 == true) {//Pagados
						tipo_depu_manu = 3;
						//periodo_sel="PAGADOS";
					}


					//MessageBox.Show("periodo: "+periodo_sel+"| tipo_depu: "+tipo_depu_manu);
					Depu_manu dep_man = new Depu_manu(periodo_sel, tipo_depu_manu);
					dep_man.ShowDialog();
					dep_man.mandar_lista_dep();
				}

				if (rb5 == true) {//RB
					tipo_depu_manu = 4;
					periodo_sel = "RB_SISCOB";
					DepuRB dep_RB = new DepuRB();
					dep_RB.ShowDialog();
				}

				if ((System.IO.File.Exists(@"temporal_datos_depu.xlsx")) == true)
				{
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_datos_depu.xlsx';Extended Properties=Excel 12.0;";
					conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
					conexion.Open(); //abrimos la conexion
									 //cons_exc = "Select * from [hoja_lz$] where [4SS] > 0.0 or [RCV] > 0.0 ";
					cons_exc = "Select * from [hoja_lz$]";

					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
					Invoke(new MethodInvoker(delegate
										 {
											 dataGridView1.DataSource = dataSet.Tables[0];
											 tot_rows = dataGridView1.RowCount;

											 //MessageBox.Show("lineas totales: "+tot_rows);
										 }));
					if (tot_rows > 0) {
						Invoke(new MethodInvoker(delegate
										 {
											 primera_linea = dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
										 }));
						if (primera_linea.Length == 0) {
							MessageBox.Show("Imposible continuar con la depuración.\nLista de Créditos Dañada", "AVISO");
							error_con_lista = 1;
							conexion.Close();
							Invoke(new MethodInvoker(delegate
													 {
														 this.Close(); }));
						}
					} else {
						MessageBox.Show("Imposible continuar con la depuración.\nLista de Créditos Vacía", "AVISO");
						error_con_lista = 1;
						Invoke(new MethodInvoker(delegate
													{
														this.Close(); }));
					}
					conexion.Close();
				} else {
					MessageBox.Show("Imposible continuar con la depuración.\nNo se Pudo Recuperar Lista de Créditos Seleccionados para Depurar", "AVISO");
					error_con_lista = 1;
					Invoke(new MethodInvoker(delegate
													{
														this.Close(); }));
				}
			}

			return error_con_lista;
		}

		public void analisis_depuracion()
		{
			String cad_con, cons_exc = "", periodo = "", sql = "", reg_pat_prosip, fecha_prosip = "", folio_prosip = "", data_pagos_validados = "", lista_ids_depu_manu = "", tipo_doc_a_dep = "";
			int i = 0, j = 0, tot_grid1 = 0, k = 0, progreso = 0, rale_validos = 0;
			decimal importe_prosip = 0;
			double porcent_comp = 0;

			System.Data.DataTable data_depu_manu = new System.Data.DataTable();
			data_guardado.Rows.Clear();
			data_solo_pago.Rows.Clear();

			Invoke(new MethodInvoker(delegate
			{
				periodo = comboBox1.SelectedItem.ToString();
				tipo_doc_a_dep = comboBox2.SelectedItem.ToString();
			}));

			//Leer Archivo Combinado de Procesar/SIPARE excluyendo importes en 0
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_combinado.xlsx';Extended Properties=Excel 12.0;";
			conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion.Open(); //abrimos la conexion
			if (periodo.StartsWith("COP"))
			{
				cons_exc = "Select * from [hoja_lz$] where [4SS] > 0.0";
			}
			else
			{
				if (periodo.StartsWith("RCV"))
				{
					cons_exc = "Select * from [hoja_lz$] where[RCV] > 0.0 ";
				}
				else
				{
					MessageBox.Show("¿Cómo Hiciste que pasara esto?");
				}
			}

			dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
			dataSet = new DataSet(); // creamos la instancia del objeto DataSet
			dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
			data_pro_sip = dataSet.Tables[0];

			if (radioButton1.Checked == true) {
				//Consulta el periodo en la BD SI ES DEPURACION AUTOMÁTICA
				sql = "SELECT id,registro_patronal2,credito_cuotas,importe_cuota,credito_multa,importe_multa,status,status_credito,folio_sipare_sua,importe_pago,porcentaje_pago,num_pago,fecha_pago,tipo_documento,subdelegacion FROM datos_factura WHERE " +
					//"nombre_periodo = \"" + periodo + "\" AND (status = \"EN TRAMITE\" OR status = \"0\") AND porcentaje_pago < 75.00 ORDER BY registro_patronal2,credito_multa";
                    "nombre_periodo = \"" + periodo + "\" AND (status = \"EN TRAMITE\" OR status = \"0\") AND porcentaje_pago < "+pago_min+" ORDER BY registro_patronal2,credito_multa";
			} else {
				if (radioButton2.Checked == true) {
					//Consulta el periodo en la BD SI ES DEPURACION MANUAL
					//Leer Archivo con patrones previamente seleccionados
					cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_datos_depu.xlsx';Extended Properties=Excel 12.0;";
					conexion1 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
					conexion1.Open(); //abrimos la conexion
					cons_exc = "Select * from [hoja_lz$]";


					dataAdapter1 = new OleDbDataAdapter(cons_exc, conexion1); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet1 = new DataSet(); // creamos la instancia del objeto DataSet
					dataAdapter1.Fill(dataSet1, "hoja_lz");//llenamos el dataset
					data_depu_manu = dataSet1.Tables[0];

					k = 0;
					lista_ids_depu_manu = "";
					while (k < data_depu_manu.Rows.Count) {
						lista_ids_depu_manu = lista_ids_depu_manu + "," + data_depu_manu.Rows[k][3].ToString();
						k++;
					}
					//MessageBox.Show(""+data_depu_manu.Rows.Count);
					k = 0;
					lista_ids_depu_manu = lista_ids_depu_manu.Substring(1, lista_ids_depu_manu.Length - 1);

					sql = "SELECT id,registro_patronal2,credito_cuotas,importe_cuota,credito_multa,importe_multa,status,status_credito,folio_sipare_sua,importe_pago,porcentaje_pago,num_pago,fecha_pago,tipo_documento,subdelegacion FROM datos_factura WHERE " +
					//"id in (" + lista_ids_depu_manu + ") AND (status = \"EN TRAMITE\" OR status = \"0\") AND porcentaje_pago < 75.00 ORDER BY registro_patronal2,credito_multa";
                    "id in (" + lista_ids_depu_manu + ") AND (status = \"EN TRAMITE\" OR status = \"0\") AND porcentaje_pago < "+pago_min+" ORDER BY registro_patronal2,credito_multa";
				}
			}

			data_depu_nova = conex.consultar(sql);

			/*
             * data_depu_nova[0] id*
             * data_depu_nova[1] registro_patronal2*
             * data_depu_nova[2] credito_cuotas*
             * data_depu_nova[3] importe_cuota*
             * data_depu_nova[4] credito_multa*
             * data_depu_nova[5] importe_multa*
             * data_depu_nova[6] status*
             * data_depu_nova[7] status_credito*
             * data_depu_nova[8] folio_sipare_sua*
             * data_depu_nova[9] importe_pago*
             * data_depu_nova[10] porcentaje_pago*
             * data_depu_nova[11] num_pago*
             * data_depu_nova[12] fecha_pago*
             * data_depu_nova[13] tipo_documento*
             * data_depu_nova[14] subdelegacion*
             */

			while (i < data_depu_nova.Rows.Count) {
				DataView vista = new DataView(data_pro_sip);
				//consulta patron en archivo combinado de pagos
				vista.RowFilter = "NRP ='" + data_depu_nova.Rows[i][1].ToString() + "'";
				vista.Sort = "F#SUA ASC";
				Invoke(new MethodInvoker(delegate
				{
					dataGridView1.Columns.Clear();
					dataGridView1.DataSource = vista;
					tot_grid1 = dataGridView1.RowCount;
				}));
				//Grid_checador checa1 = new Grid_checador(dataGridView1.DataSource as DataTable);
				//checa1.ShowDialog();
				//concatenar linea de pago y colocarla en tabla
				j = 0;


				while (j < tot_grid1) {
					Invoke(new MethodInvoker(delegate {
						data_pagos_repe.Rows.Add(dataGridView1.Rows[j].Cells[0].Value.ToString() + "_" + dataGridView1.Rows[j].Cells[1].Value.ToString() + "_" +
												 dataGridView1.Rows[j].Cells[3].Value.ToString() + "_" + dataGridView1.Rows[j].Cells[4].Value.ToString());
					}));
					j++;
				}

				//limpiar tabla de repetidos
				j = 0;
				while (j < tot_grid1) {
					k = 0;
					while (k < tot_grid1) {
						if (k != j) {
							if (data_pagos_repe.Rows[j][0].ToString().Length > 0) {
								if (data_pagos_repe.Rows[j][0].ToString().Equals(data_pagos_repe.Rows[k][0].ToString())) {
									data_pagos_repe.Rows[k][0] = "";
								}
							}
						}
						k++;
					}
					j++;
				}

				//guardar tabla de registros unicos
				j = 0;
				while (j < tot_grid1) {
					if (data_pagos_repe.Rows[j][0].ToString().Length > 0) {
						data_pagos_validados = "," + j + ",";
					}
					j++;
				}

				j = 0;
				while (j < tot_grid1) {
					//si hubo por lo menos un pago
					Invoke(new MethodInvoker(delegate {
						if (data_pagos_validados.Contains("," + j + ",") == true) {

							reg_pat_prosip = dataGridView1.Rows[j].Cells[0].Value.ToString();
							folio_prosip = dataGridView1.Rows[j].Cells[1].Value.ToString();
							fecha_prosip = dataGridView1.Rows[j].Cells[2].Value.ToString();
							if (periodo.StartsWith("COP")) {
								importe_prosip = Convert.ToDecimal(dataGridView1.Rows[j].Cells[3].Value.ToString());
							} else {
								importe_prosip = Convert.ToDecimal(dataGridView1.Rows[j].Cells[4].Value.ToString());
							}
						} else {
							reg_pat_prosip = "";
							folio_prosip = "";
							fecha_prosip = "";
							importe_prosip = 0;
						}
					}));

					if (folio_prosip.Length > 0) {
						if (data_depu_nova.Rows[i][8].ToString().Contains(folio_prosip) == false) {
							//si NO es duplicado ***************************************************************
							//se añade el nuevo folio
							if (data_depu_nova.Rows[i][8].ToString().Length == 1) {
								data_depu_nova.Rows[i][8] = "";
								data_depu_nova.Rows[i][8] = folio_prosip;
							} else {
								data_depu_nova.Rows[i][8] = data_depu_nova.Rows[i][8].ToString() + "," + folio_prosip;
							}
							//se suma el importe total pagado
							data_depu_nova.Rows[i][9] = (Convert.ToDecimal(data_depu_nova.Rows[i][9].ToString())) + importe_prosip;

							//se calcula el porcentaje de pago
							if ((tipo_doc_a_dep.Equals("80") == true) || (tipo_doc_a_dep.Equals("81") == true))
							{
								//si se depura la multa
								data_depu_nova.Rows[i][10] = Convert.ToDecimal(((Convert.ToDecimal(data_depu_nova.Rows[i][9].ToString()) * 100) / Convert.ToDecimal(data_depu_nova.Rows[i][3].ToString())));
							} else {
								//si se depura la cuota
								data_depu_nova.Rows[i][10] = Convert.ToDecimal(((Convert.ToDecimal(data_depu_nova.Rows[i][9].ToString()) * 100) / Convert.ToDecimal(data_depu_nova.Rows[i][5].ToString())));
							}

							//se cuenta el numero de pagos
							data_depu_nova.Rows[i][11] = Convert.ToInt32(data_depu_nova.Rows[i][11]) + 1;

							//se añade la fecha de pago
							if (fecha_prosip.Length > 0) {
								if (data_depu_nova.Rows[i][12].ToString().Length == 1) {
									data_depu_nova.Rows[i][12] = "";
									data_depu_nova.Rows[i][12] = fecha_prosip;
								} else {
									data_depu_nova.Rows[i][12] = data_depu_nova.Rows[i][12].ToString() + "," + fecha_prosip;
								}
							}

							//se recortan decimales
							if (data_depu_nova.Rows[i][10].ToString().Length > 8)
							{
								data_depu_nova.Rows[i][10] = data_depu_nova.Rows[i][10].ToString().Substring(0, 8);
							}

							//si es mayor o igual a 75 se apunta para que se depure
							if ((double.TryParse(data_depu_nova.Rows[i][10].ToString(), out porcent_comp)) == true)
							{
								//if ((Convert.ToDouble(data_depu_nova.Rows[i][10].ToString())) > 74.999)
                                if ((Convert.ToDouble(data_depu_nova.Rows[i][10].ToString())) > Convert.ToDouble((Convert.ToInt32(pago_min)-1).ToString()+".999"))
								{
									//VALIDACION EN EL RALE____MEJORA DEL 28/04/2020									
									Invoke(new MethodInvoker(delegate
									{
										label9.Text = comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().Length - 6, 6);
										label10.Text = "0";
									}));

									conex5.conectar("base_principal");
									if ((tipo_doc_a_dep.Equals("02") == true) || (tipo_doc_a_dep.Equals("06") == true))
									{
                                        data_rale = conex5.consultar("SELECT registro_patronal,credito,periodo,importe FROM rale WHERE registro_patronal=\"" + data_depu_nova.Rows[i][1].ToString() + "\" AND credito=\"" + data_depu_nova.Rows[i][2].ToString() + "\" AND periodo=\"" + label9.Text + "\" AND incidencia <> \"23\" AND incidencia <> \"25\"");
									}
									else
									{
                                        data_rale = conex5.consultar("SELECT registro_patronal,credito,periodo,importe FROM rale WHERE registro_patronal=\"" + data_depu_nova.Rows[i][1].ToString() + "\" AND credito=\"" + data_depu_nova.Rows[i][4].ToString() + "\" AND periodo=\"" + label9.Text + "\" AND incidencia <> \"23\" AND incidencia <> \"25\"");
									}

                                    if (verif_rale == "1")
                                    {
                                        if (data_rale.Rows.Count > 0)
                                        {
                                            //***No Mover linea de abajo, es la que guarda los indices que se trabajaran

                                            data_guardado.Rows.Add(i);
                                            rale_validos++;

                                            //label10.Text = rale_validos.ToString();
                                            /*
                                            Invoke(new MethodInvoker(delegate
                                            {
                                                label10.Text = rale_validos.ToString();
                                            }));*/
                                        }
                                        else
                                        {
                                            data_solo_pago.Rows.Add(i);
                                        }

                                    }
                                    else
                                    {
                                        data_guardado.Rows.Add(i);
                                        rale_validos++;
                                    }
								}
								else
								{//sino se apunta para solo anexar el pago(s)-----
									data_solo_pago.Rows.Add(i);
								}
							}
							else
							{
								//MessageBox.Show(data_depu_nova.Rows[i][10].ToString());
							}

						} else {
							//si es duplicado lo ignora
						}
					}

					j++;
				}//fin busqueda de pagos

				if (tot_grid1 == 0) {
					//si no hubo ningún pago
				}
				j = 0;
				i++;

				Invoke(new MethodInvoker(delegate {
					progreso = (Convert.ToInt32((((i + 1) * 100) / data_depu_nova.Rows.Count) / 2) + 50);

					if (progreso > 98) {
						progreso = 100;
					}

					progressBar1.Value = progreso;
					label4.Text = "" + progreso + "%";
					label5.Text = "Analizando Crédito: " + (i) + " de " + (data_depu_nova.Rows.Count);
				}));
			}//fin busqueda de patrones

			//CERRAR CONEXIONES XLS
			try
			{
				conexion.Close();
				conexion1.Close();
			}
			catch (Exception con_excep)
			{

			}

			//CHECAR RESULTADOS ********
			XLWorkbook wb = new XLWorkbook();
			wb.Worksheets.Add(data_depu_nova, "hoja_lz");
			wb.SaveAs(@"resultado_previo.xlsx");
			wb.Dispose();

			XLWorkbook wb1 = new XLWorkbook();
			wb1.Worksheets.Add(data_guardado, "hoja_lz");
			wb1.SaveAs(@"resultado_previo_depu_guardado.xlsx");
			wb1.Dispose();
			//            Grid_checador checa = new Grid_checador(data_depu_nova);
			//            MessageBox.Show(data_depu_nova.Rows.Count.ToString());
			//            checa.ShowDialog();
			Invoke(new MethodInvoker(delegate
			 {
				 groupBox6.Enabled = true;
				 textBox1.Text = "";
				 textBox1.AppendText("Periodo Analizado: " + comboBox1.SelectedItem.ToString().Substring(comboBox1.SelectedItem.ToString().Length - 6, 6) + "\r\n");
				 textBox1.AppendText("Pagos Encontrados: " + data_acumulador.Rows.Count + "\r\n");
				 textBox1.AppendText("Pagos Válidos: " + data_pro_sip.Rows.Count + "\r\n");
				 textBox1.AppendText("Créditos Analizados: " + data_depu_nova.Rows.Count + "\r\n");
				 textBox1.AppendText("Créditos Validados con RALE : " + rale_validos + "\r\n");
				 textBox1.AppendText("Créditos A Depurar: " + data_guardado.Rows.Count + "\r\n");
				 //textBox1.AppendText("Créditos con Pago Menor al 75% : " + data_solo_pago.Rows.Count + "\r\n");
                 textBox1.AppendText("Créditos con Pago Menor al "+pago_min+"% : " + data_solo_pago.Rows.Count + "\r\n");

				 
				 if (data_guardado.Rows.Count > 0) {
				 	groupBox7.Enabled = true;
				 } else {
				 	groupBox7.Enabled = false;
					groupBox1.Enabled = true;
					groupBox2.Enabled = true;
					groupBox4.Enabled = true;
				 }
				 button4.Enabled = false;
				 panel2.Visible = false;
			 }));

		}

		//control del proceso principal del analisis
		public void analisis() {
			//try{
			int error_con_lista = 0;
			bool rb5 = false;
			//0.-Si es Depuración Manual mostrar ventana de selección de créditos
			error_con_lista = depu_manual();
			try {
				Invoke(new MethodInvoker(delegate
				{ rb5 = radioButton5.Checked;

				}));
			} catch (InvalidOperationException f) {
				this.Close();
			}

			if (error_con_lista == 0) {

				if (rb5 == true) {
					rb_proceso();
				} else {
					//1.-Extraer Información de los Archivos y guardarla en un solo archivo(temporal_combinado.xls), sólo con las columnas necesarias
					leer_archivos();
					//2.-Limpiar el Archivo de lineas duplicadas y de importes con 0/Comparación de Pagos con créditos de la BD (Depuración) / Guardar Info de Afectación a la BD
					analisis_depuracion();
				}
			} else {
				try {
					Invoke(new MethodInvoker(delegate
					{
						groupBox1.Enabled = true;
						groupBox2.Enabled = true;
						groupBox4.Enabled = true;
					}));
				} catch (InvalidOperationException f1) {
					this.Close();
				}
			}
			//}catch{
			//Invoke(new MethodInvoker(delegate{ this.Close();}));
			//}
		}

		public void afectar_bd() {//guardar
			int i = 0, progreso = 0;
			String sql = "", tipo_doc_a_dep = "", id, status, folio_sua, importe_pag, porcent_pago, num_pago, fech_pago, fecha_hoy_depu, credito, importe_cred, periodo, del, subdel, per_combobox = "", folio_core_final = "", folio_core = "";
			bool rb1 = false, rb2 = false;

			Invoke(new MethodInvoker(delegate {
				if (radioButton5.Checked == true) {
					tipo_doc_a_dep = "80";
					per_combobox = "COP_ECO_MEC_209912";
				} else {
					tipo_doc_a_dep = comboBox2.SelectedItem.ToString();
					per_combobox = comboBox1.SelectedItem.ToString();
				}

				rb1 = radioButton1.Checked;
				rb2 = radioButton2.Checked;

			}));

			conex3.conectar("base_principal");
			if ((tipo_doc_a_dep.Equals("02") == true) || (tipo_doc_a_dep.Equals("06") == true)) {
				//datos para guardar la info de la core
				if (tipo_doc_a_dep.Equals("06") == true) {
					folio_core_final = "RCV_" + generar_folio_not("CM_12", "RCV") + "_1";
				} else {
					folio_core_final = "COP_" + generar_folio_not("CM_12", "COP") + "_1";
				}
			} else {
				//datos para guardar la info de la core
				folio_core_final = "COP_" + generar_folio_not("CM_12", "COP") + "_2";
			}
			//MessageBox.Show(folio_core_final);
			/*
             * data_depu_nova[0] id*
             * data_depu_nova[1] registro_patronal2*+++
             * data_depu_nova[2] credito_cuotas*+
             * data_depu_nova[3] importe_cuota*+
             * data_depu_nova[4] credito_multa*++
             * data_depu_nova[5] importe_multa*++
             * data_depu_nova[6] status*--
             * data_depu_nova[7] status_credito*--
             * data_depu_nova[8] folio_sipare_sua*-----+++
             * data_depu_nova[9] importe_pago*-----
             * data_depu_nova[10] porcentaje_pago*-----
             * data_depu_nova[11] num_pago*-----
             * data_depu_nova[12] fecha_pago*-----+++
             * data_depu_nova[13] tipo_documento*+++
             * data_depu_nova[14] subdelegacion*+++
             * data_depu_nova[15] periodo*+++ SOLO RB
             */

			//guardar depurados en la BD
			while (i < data_guardado.Rows.Count) {
				//obtener datos guardados
				Invoke(new MethodInvoker(delegate {
					progreso = (Convert.ToInt32((((i + 1) * 100) / data_guardado.Rows.Count) / 1.25));

					if (progreso > 98) {
						progreso = 100;
					}
					progressBar2.Value = progreso;

					label7.Text = "" + progreso + "%";
					label6.Text = "Actualizando Crédito: " + (i + 1) + " de " + (data_guardado.Rows.Count);
				}));

				id = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][0].ToString();
				folio_sua = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][8].ToString();
				importe_pag = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][9].ToString();
				porcent_pago = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][10].ToString();
				num_pago = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][11].ToString();
				fech_pago = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][12].ToString();

				if ((tipo_doc_a_dep.Equals("02") == true) || (tipo_doc_a_dep.Equals("06") == true)) {
					if (rb1 == true) {
						status = " status_credito=\"DEPURACION MANUAL\",";
					} else {
						if (rb2 == true) {
							status = " status_credito=\"DEPURACION MANUAL\",";
						} else {
							status = " ";
						}
					}
					credito = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][2].ToString();
					importe_cred = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][3].ToString();

				} else {
					if (rb1 == true) {
						status = " status=\"DEPURACION\",";
					} else {
						if (rb2 == true) {
							status = " status=\"DEPURACION\",";
						} else {
							status = " ";
						}
					}
					credito = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][4].ToString();
					importe_cred = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][5].ToString();

				}

				fecha_hoy_depu = System.DateTime.Today.Year.ToString();
				if (System.DateTime.Today.Month.ToString().Length == 1) {
					fecha_hoy_depu = fecha_hoy_depu + "-0" + System.DateTime.Today.Month.ToString();
				} else {
					fecha_hoy_depu = fecha_hoy_depu + "-" + System.DateTime.Today.Month.ToString();
				}

				if (System.DateTime.Today.Day.ToString().Length == 1) {
					fecha_hoy_depu = fecha_hoy_depu + "-0" + System.DateTime.Today.Day.ToString();
				} else {
					fecha_hoy_depu = fecha_hoy_depu + "-" + System.DateTime.Today.Day.ToString();
				}
				if (fech_pago.StartsWith(",") == true) {
					fech_pago = fech_pago.Substring(1, fech_pago.Length - 1);
					//fech_pago=fech_pago.Substring(7,4)+"-"+fech_pago.Substring(4,2)+"-"+fech_pago.Substring(1,2);
				} else {
					//fech_pago=
					//fech_pago=fech_pago.Substring(6,4)+"-"+fech_pago.Substring(3,2)+"-"+fech_pago.Substring(0,2);
				}

				if (folio_sua.StartsWith(",") == true) {
					folio_sua = folio_sua.Substring(1, folio_sua.Length - 1);
				}
				if (radioButton4.Checked == true) {//si fue depuracion de pagados
					sql = "UPDATE datos_factura SET " + status + " folio_sipare_sua=\"" + folio_sua + "\", importe_pago=" + importe_pag + ", porcentaje_pago=" + porcent_pago + ", num_pago=" + num_pago + ", fecha_pago=\"" + fech_pago + "\", fecha_depuracion= \"" + fecha_hoy_depu + "\", observaciones=\" \", folio_not_core=\"" + folio_core_final + "\"  " +
					"WHERE id =" + id + "";
				} else {
					if (radioButton5.Checked == true) {//si fue depuracion de rb
						sql = "UPDATE datos_factura SET " + status + " folio_sipare_sua=\"RB\", importe_pago=" + importe_pag + ", porcentaje_pago=" + porcent_pago + ", num_pago=" + num_pago + ", fecha_pago=\"" + fech_pago + "\", fecha_depuracion= \"" + fecha_hoy_depu + "\", observaciones=\" \", folio_not_core=\"" + folio_core_final + "\"  " +
						"WHERE id =" + id + "";
					} else {
						sql = "UPDATE datos_factura SET " + status + " folio_sipare_sua=\"" + folio_sua + "\", importe_pago=" + importe_pag + ", porcentaje_pago=" + porcent_pago + ", num_pago=" + num_pago + ", fecha_pago=\"" + fech_pago + "\", fecha_depuracion= \"" + fecha_hoy_depu + "\", folio_not_core=\"" + folio_core_final + "\" " +
							"WHERE id =" + id + "";
					}
				}


				//MessageBox.Show(sql);
				conex3.consultar(sql);
				if (checkBox3.Checked == true) {
					periodo = data_pro_sip.Rows[0][5].ToString().Substring(0, 4) + data_pro_sip.Rows[0][5].ToString().Substring(5, 2);
				} else {
					if (radioButton5.Checked == true) {
						periodo = data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][15].ToString();
					} else {
						if (per_combobox.StartsWith("RCV") == true) {
							periodo = per_combobox.Substring(per_combobox.Length - 6, 6);
						} else {
							periodo = data_pro_sip.Rows[0][5].ToString();
						}
					}
				}

				if (radioButton5.Checked == true) {//SI ES RB
					data_reporte.Rows.Add(data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][1].ToString(), credito, periodo, "-",
									  data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][14].ToString(), importe_cred, "RB", fech_pago);
				} else {//SI NO ES RB 
					data_reporte.Rows.Add(data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][1].ToString(), credito, periodo, tipo_doc_a_dep,
									  data_depu_nova.Rows[Convert.ToInt32(data_guardado.Rows[i][0].ToString())][14].ToString(), importe_cred, folio_sua, fech_pago);

				}
				i++;
			}

			i = 0;
			//guardar pagos de creditos no depurados
			while (i < data_solo_pago.Rows.Count) {
				//obtener datos guardados
				Invoke(new MethodInvoker(delegate {
					progreso = (Convert.ToInt32((((i + 1) * 100) / data_solo_pago.Rows.Count) / 5) + 80);
					label7.Text = "" + progreso + "%";
					progressBar2.Value = progreso;
					label6.Text = "Analizando Crédito: " + (i + 1) + " de " + (data_solo_pago.Rows.Count);
				}));

				id = data_depu_nova.Rows[Convert.ToInt32(data_solo_pago.Rows[i][0].ToString())][0].ToString();
				folio_sua = data_depu_nova.Rows[Convert.ToInt32(data_solo_pago.Rows[i][0].ToString())][8].ToString();
				importe_pag = data_depu_nova.Rows[Convert.ToInt32(data_solo_pago.Rows[i][0].ToString())][9].ToString();
				porcent_pago = data_depu_nova.Rows[Convert.ToInt32(data_solo_pago.Rows[i][0].ToString())][10].ToString();
				num_pago = data_depu_nova.Rows[Convert.ToInt32(data_solo_pago.Rows[i][0].ToString())][11].ToString();
				fech_pago = data_depu_nova.Rows[Convert.ToInt32(data_solo_pago.Rows[i][0].ToString())][12].ToString();

				if (fech_pago.StartsWith(",") == true) {
					fech_pago = fech_pago.Substring(1, fech_pago.Length - 1);
					//fech_pago=fech_pago.Substring(7,4)+"-"+fech_pago.Substring(4,2)+"-"+fech_pago.Substring(1,2);
				}

				//fech_pago=fech_pago.Substring(6,4)+"-"+fech_pago.Substring(3,2)+"-"+fech_pago.Substring(0,2);
				
                if(Convert.ToDouble(porcent_pago) < Convert.ToDouble(pago_min)){
                    sql = "UPDATE datos_factura SET folio_sipare_sua=\"" + folio_sua + "\", importe_pago=" + importe_pag + ", porcentaje_pago=" + porcent_pago + ", num_pago=" + num_pago + ", fecha_pago=\"" + fech_pago + "\" " +
					    "WHERE id =" + id + "";
                }else{
                    sql = "UPDATE datos_factura SET folio_sipare_sua=\"" + folio_sua + "\", importe_pago=" + importe_pag + ", porcentaje_pago=" + porcent_pago + ", num_pago=" + num_pago + ", fecha_pago=\"" + fech_pago + "\" ,status=\"DEPURACION\" " +
                    "WHERE id =" + id + "";
                }
				conex3.consultar(sql);

				i++;
			}

			Invoke(new MethodInvoker(delegate {
				label6.Text = "Generando Core...";

				if (data_reporte.Rows.Count > 0) {//si el guardado fue exitoso
					//button1.Visible = true;
					guarda_xlsx_final(data_reporte);
					label6.Text = "Core Generada";
					label7.Text = "100%";
					progressBar2.Value = 100;
					groupBox1.Enabled = true;
					groupBox2.Enabled = true;
					groupBox4.Enabled = true;
					groupBox5.Enabled = true;
					MessageBox.Show("El Proceso terminó correctamente", "Exito",MessageBoxButtons.OK,MessageBoxIcon.Information);
				}
				else {
					MessageBox.Show("No se Generará Ninguna Core", "Error");
				}
			}));
		}

		public void rb_proceso() {

			String cad_con, cons_exc, lista_ids_depu_manu, sql;
			int k = 0, i = 0;
			System.Data.DataTable data_depu_manu = new System.Data.DataTable();
			data_guardado.Rows.Clear();

			//Leer Archivo con patrones previamente seleccionados
			cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temporal_datos_depu.xlsx';Extended Properties=Excel 12.0;";
			conexion1 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
			conexion1.Open(); //abrimos la conexion
			cons_exc = "Select * from [hoja_lz$]";


			dataAdapter1 = new OleDbDataAdapter(cons_exc, conexion1); //traemos los datos de la hoja y las guardamos en un dataAdapter
			dataSet1 = new DataSet(); // creamos la instancia del objeto DataSet
			dataAdapter1.Fill(dataSet1, "hoja_lz");//llenamos el dataset
			data_depu_manu = dataSet1.Tables[0];

			k = 0;
			lista_ids_depu_manu = "";
			while (k < data_depu_manu.Rows.Count) {
				lista_ids_depu_manu = lista_ids_depu_manu + "," + data_depu_manu.Rows[k][3].ToString();
				k++;
			}
			//MessageBox.Show(""+data_depu_manu.Rows.Count);
			k = 0;
			lista_ids_depu_manu = lista_ids_depu_manu.Substring(1, lista_ids_depu_manu.Length - 1);

			sql = "SELECT id,registro_patronal2,credito_cuotas,importe_cuota,credito_multa,importe_multa,status,status_credito,folio_sipare_sua,importe_pago,porcentaje_pago,num_pago,fecha_pago,tipo_documento,subdelegacion,periodo FROM datos_factura WHERE " +
				"id in (" + lista_ids_depu_manu + ") ORDER BY registro_patronal2,credito_multa";
			data_depu_nova = conex.consultar(sql);

			while (i < data_depu_manu.Rows.Count) {
				data_guardado.Rows.Add(i);
				i++;
			}

			//CHECAR RESULTADOS ********
			XLWorkbook wb = new XLWorkbook();
			wb.Worksheets.Add(data_depu_nova, "hoja_lz");
			wb.SaveAs(@"resultado_previo.xlsx");
			wb.Dispose();
			//            Grid_checador checa = new Grid_checador(data_depu_nova);
			//            MessageBox.Show(data_depu_nova.Rows.Count.ToString());
			//            checa.ShowDialog();
			Invoke(new MethodInvoker(delegate
		   {
			   groupBox6.Enabled = true;
			   textBox1.Text = "";
			   textBox1.AppendText("Créditos Analizados: " + data_depu_nova.Rows.Count + "\r\n");
			   textBox1.AppendText("Créditos A Depurar: " + data_guardado.Rows.Count + "\r\n");

			   groupBox1.Enabled = true;
			   groupBox2.Enabled = true;
			   groupBox4.Enabled = true;
			   if (data_guardado.Rows.Count > 0) {
				   groupBox7.Enabled = true;
			   } else {
				   groupBox7.Enabled = false;
			   }
			   button4.Enabled = false;
			   panel2.Visible = false;
		   }));
		}

		public String generar_folio_not(String tipo_core, String clase_core)
		{
			String ultimo_folio, folio;
			int sig_folio = 0;

			conex3.conectar("base_principal");
			consultamysql = conex3.consultar("SELECT folio_notificacion FROM info_cores WHERE tipo_core=\"" + tipo_core + "\" AND clase_core=\"" + clase_core + "\" ORDER BY id_core DESC");
			if (consultamysql.Rows.Count > 0)
			{
				ultimo_folio = consultamysql.Rows[0][0].ToString();

				sig_folio = Convert.ToInt32(ultimo_folio.Substring(3, (ultimo_folio.Length - 3)));
				sig_folio++;
				folio = ultimo_folio.Substring(0, 3) + sig_folio.ToString();
				return folio;
			}
			else
			{
				folio = tipo_core.Substring(tipo_core.Length - 2, 2) + "_0";
				return folio;
			}
		}

		public void guarda_xlsx_final(System.Data.DataTable data_report){

			String cad_con = "",cons_exc="",periodo="",tipo_doc_a_dep="",archivo="";
			System.Data.DataTable data_report_temp = new System.Data.DataTable();

			Invoke(new MethodInvoker(delegate
			{
				periodo = comboBox1.SelectedItem.ToString();
				tipo_doc_a_dep = comboBox2.SelectedItem.ToString();
			}));

			if (periodo.StartsWith("COP"))
			{
				archivo = @"temp\depuracion_final_COP.xlsx";
			}
			else
			{
				archivo = @"temp\depuracion_final_RCV.xlsx";
			}

			if ((System.IO.File.Exists(archivo)) == true)
			{
				cad_con = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source='"+archivo+"';Extended Properties=Excel 12.0;";
				conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
				conexion.Open(); //abrimos la conexion

				cons_exc = "Select * from [hoja_lz$] ";//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
				dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
				dataSet = new DataSet(); // creamos la instancia del objeto DataSet

				if (dataAdapter.Equals(null)){
					conexion.Close();
					conexion.Dispose();
					System.IO.File.Delete(archivo);

					XLWorkbook wb_z = new XLWorkbook();
					wb_z.Worksheets.Add(data_report, "hoja_lz");
					wb_z.SaveAs(archivo);
					wb_z.Dispose();
				}else{
					dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
					data_report_temp = dataSet.Tables[0];
					data_report_temp.Merge(data_report);
					conexion.Close();
					conexion.Dispose();
					System.IO.File.Delete(archivo);

					XLWorkbook wb_z = new XLWorkbook();
					wb_z.Worksheets.Add(data_report_temp, "hoja_lz");
					wb_z.SaveAs(archivo);
					wb_z.Dispose();
				}				
			}else{
				XLWorkbook wb_z = new XLWorkbook();
				wb_z.Worksheets.Add(data_report, "hoja_lz");
				wb_z.SaveAs(archivo);
				wb_z.Dispose();
			}

			SaveFileDialog guarda_depu = new SaveFileDialog();
			guarda_depu.Filter = "Archivos de Excel (*.XLSX)|*.XLSX";
			guarda_depu.Title = "Guardar resultados de la Depuración";

			if (guarda_depu.ShowDialog() == DialogResult.OK)
			{
				archivo = guarda_depu.FileName;

				XLWorkbook wb_z0 = new XLWorkbook();
				wb_z0.Worksheets.Add(data_report, "hoja_lz");
				wb_z0.SaveAs(archivo);
				wb_z0.Dispose();
			}

		}
					
		void Depuración2Load(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			conex4.conectar("base_principal");
			dataGridView4.DataSource = conex4.consultar("SELECT * FROM log_eventos WHERE evento like \"Se Ingreso el RALE_COP%\" order by idlog_eventos desc");
			label8.Text = "Rale Ingresado el Día: " + dataGridView4.Rows[0].Cells[1].FormattedValue.ToString().Substring(0, 10);

			decimal tipo_decimal=0;
			llenar_Cb1();
			dataGridView2.DefaultCellStyle.ForeColor=System.Drawing.Color.Black;
			
			data_acumulador.Columns.Add("NRP");
            data_acumulador.Columns.Add("F#SUA",typeof(String));
            data_acumulador.Columns.Add("FECHA PAGO");
            data_acumulador.Columns.Add("4SS",tipo_decimal.GetType());
            data_acumulador.Columns.Add("RCV",tipo_decimal.GetType());
            data_acumulador.Columns.Add("PERIODO_PAGO");
            data_acumulador.Columns.Add("ARCHIVO_ORIGEN");
            data_acumulador.Columns.Add("CREDITO");//solo GP
            
            data_reporte.Columns.Add("NRP");
            data_reporte.Columns.Add("CREDITO");
            data_reporte.Columns.Add("PERIODO");
            data_reporte.Columns.Add("TD");
            data_reporte.Columns.Add("SUB");
            data_reporte.Columns.Add("IMPORTE");
            data_reporte.Columns.Add("FOLIO");
            data_reporte.Columns.Add("FECHA");           
            
            data_pagos_repe.Columns.Add("linea");
            data_guardado.Columns.Add("id");
            data_solo_pago.Columns.Add("id");

            leer_opciones();
                        
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)//automatizado
		{
			if(radioButton1.Checked==true){
				groupBox2.Enabled=true;
				groupBox3.Enabled=false;
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)//manual
		{
			if(radioButton2.Checked==true){
				groupBox2.Enabled=true;
				groupBox3.Enabled=true;
			}
		}
		
		void GroupBox2Enter(object sender, EventArgs e)
		{
			
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}		
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex > -1){
				comboBox2.Enabled=true;
				if((comboBox1.SelectedItem.ToString().Substring(0,3).Equals("COP"))==true){
					comboBox2.Items.Clear();
					comboBox2.SelectedIndex=-1;
					groupBox4.Enabled=false;
					comboBox2.Items.Add("02");//Cuota COP
					comboBox2.Items.Add("80");//Multa COP
				}else{
					if((comboBox1.SelectedItem.ToString().Substring(0,3).Equals("RCV"))==true){
						comboBox2.Items.Clear();
						comboBox2.SelectedIndex=-1;
						groupBox4.Enabled=false;
						comboBox2.Items.Add("06");//Cuota RCV
						comboBox2.Items.Add("81");//Multa RCV
					}
				}
			}else{
				groupBox4.Enabled=false;
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(radioButton5.Checked==true){
				if (comboBox1.SelectedIndex > -1){
					groupBox5.Enabled=true;
				}
			}else{
				if (comboBox1.SelectedIndex > -1){
					if(comboBox1.SelectedItem.ToString().StartsWith("RCV")){
						checkBox3.Enabled=false;
					}else{
						checkBox3.Enabled=true;
					}
					
					groupBox4.Enabled=true;
					timer1.Enabled=true;
				}else{
					groupBox4.Enabled=false;
					timer1.Enabled=false;
				}
			}
		}
		//PROCESAR
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true){
				checkBox3.Enabled=false;
				cargar_archivos(1);//PROCESAR
				
			}else{
				dataGridView2.Rows.Clear();
				if(checkBox2.Checked==true){
				checkBox3.Enabled=false;
				}else{
				checkBox3.Enabled=true;
				}
			}
		}
		//SIPARE
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox2.Checked==true){
				checkBox3.Enabled=false;
				cargar_archivos(2);//SIPARE
			}else{
				dataGridView2.Rows.Clear();
				if(checkBox1.Checked==true){
					checkBox3.Enabled=false;
				}else{
					checkBox3.Enabled=true;
				}
			}
		}
		//GENERAL DE PAGOS
		void CheckBox3CheckedChanged(object sender, EventArgs e)
		{


			if(checkBox3.Checked==true){
				checkBox1.Enabled=false;
				checkBox2.Enabled=false;
				
				String ext_arch,archivo_gp_x,atributos_archs;
				string[] archivos_tot;
				int xy=0,ultimo_punto=0,fin_cadena=0,tot_gp=0;
				
				
				FolderBrowserDialog fbd = new FolderBrowserDialog();
				fbd.Description = "Selecciona la carpeta que contiene los archivos de GENERAL DE PAGOS";
				fbd.ShowNewFolderButton =false;
				DialogResult result = fbd.ShowDialog();
				
				if(result == DialogResult.OK){
					archivos_tot = Directory.GetFiles(fbd.SelectedPath);
					//MessageBox.Show("archivo:"+archivos_tot[12].ToString()+"| Atributos:"+System.IO.File.GetAttributes(archivos_tot[12].ToString()).ToString());
				
					do{
						ultimo_punto = archivos_tot[xy].LastIndexOf('.')+1;
						fin_cadena = (archivos_tot[xy].Length-ultimo_punto);
						archivo_gp_x = archivos_tot[xy].ToString();
						ext_arch= archivo_gp_x.Substring(ultimo_punto,fin_cadena);
						atributos_archs=System.IO.File.GetAttributes(archivos_tot[xy].ToString()).ToString();
						
						if(ext_arch == "xlsx"){
							if(atributos_archs.Contains("Hidden")==false){
								tot_gp++;
								dataGridView2.Rows.Add((dataGridView2.Rows.Count+1),archivo_gp_x,3);
							}
						}
						
						if(ext_arch == "xls"){
							//archivos_gp[tot_gp] = archivo_gp_x;
							if(atributos_archs.Contains("Hidden")==false){
								tot_gp++;
								dataGridView2.Rows.Add((dataGridView2.Rows.Count+1),archivo_gp_x,3);
							}
						}
						//MessageBox.Show("total de archivos excel: "+tot_gp+" archivo 1: "+archivos_tot[0].ToString(), "Aviso");
						xy++;
					}while(xy<archivos_tot.Length);
					
					
					if(tot_gp>0){
						MessageBox.Show("Archivos Existentes En la Carpeta: " + archivos_tot.Length + "\nArchivos que se serán procesados: " +tot_gp+"",
						                                       "Aviso",MessageBoxButtons.OK,MessageBoxIcon.Information);
						
					}else{
						MessageBox.Show("La Carpeta Seleccionada no contiene ningún archivo compatible.", "Aviso");
					}
					
				}
				
			}else{
				checkBox1.Enabled=true;
				checkBox2.Enabled=true;
				dataGridView2.Rows.Clear();
			}
		}
		
		void ComboBox1Leave(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex < 0){
			    comboBox1.Text="";
			    groupBox4.Enabled=false;
			}
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true && checkBox2.Checked==true && checkBox3.Checked==false && dataGridView2.Rows.Count==2){
				groupBox5.Enabled=true;
			}else{
				if(checkBox1.Checked==false&& checkBox2.Checked==false && checkBox3.Checked==true && dataGridView2.Rows.Count>0){
					groupBox5.Enabled=true;
				}else{
					if(radioButton5.Checked==true){
						if(comboBox2.SelectedIndex>-1){
							groupBox5.Enabled=true;
						}
					}else{
						groupBox5.Enabled=false;
					}
				}
			}
		}
		//boton analisis
		void Button4Click(object sender, EventArgs e)
		{
			DialogResult result1 = MessageBox.Show("El Análisis se efectuará con los Ajustes Seleccionados y tomará varios minutos. \n\n¿Desea comenzar con el Análisis?",
						                                       "Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
			if(result1 == DialogResult.Yes){
				progressBar1.Value=0;
				panel2.Visible=true;
				label4.Text="0%";
				label5.Text="Analizando Archivos...";
				
				
				//leer_archivos();
				button4.Enabled=false;
				groupBox1.Enabled=false;
				groupBox2.Enabled=false;
				groupBox4.Enabled=false;
				hilosecundario = new Thread(new ThreadStart(analisis));
                hilosecundario.Start();
               
			}
		}
		//boton guardar
		void Button5Click(object sender, EventArgs e)
		{
			DialogResult result2 = MessageBox.Show("Este Proceso afectará la base de datos y no podrá ser revertido\n\n¿Desea Continuar de Todos Modos?",
						                                       "Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
			if(result2 == DialogResult.Yes){
				progressBar2.Value=0;
				panel3.Visible=true;
				label7.Text="0%";
				label6.Text="Guardando Información..";
				
				//leer_archivos();
				groupBox1.Enabled=false;
				groupBox2.Enabled=false;
				groupBox4.Enabled=false;
				groupBox5.Enabled=false;
				groupBox6.Enabled=false;
				hilosecundario = new Thread(new ThreadStart(afectar_bd));
                hilosecundario.Start();
               
			}
		}
		//boton ver core
		void Button1Click(object sender, EventArgs e)
		{
			Lector_Depus lec_dep = new Lector_Depus();
			lec_dep.Show();
			/*
            String jefe_sec, jefe_ofi;
            jefe_ofi = conex.leer_config_sub()[8];
            jefe_sec = conex.leer_config_sub()[9];
            String solicita = jefe_sec+"\nOficina de Emisiones P.O.", autoriza = jefe_ofi+"\nJefe Oficina de Emisiones y P.O.";

             	Visor_Reporte visor1 = new Visor_Reporte();
				visor1.envio(data_reporte, solicita,autoriza);
				//MessageBox.Show(" "+visor1.id_core());
				//guarda_info_core(visor1.id_core());
				visor1.Show();
			try
			{
				//conexion.Close();
				//conexion1.Close();
			}
			catch { }
			*/
		}
		
		void DataGridView2RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if(dataGridView2.RowCount>0){
				button4.Enabled=true;
			}else{
				button4.Enabled=false;
			}
		}
		
		void DataGridView2RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if(dataGridView2.RowCount>0){
				button4.Enabled=true;
			}else{
				button4.Enabled=false;
			}
		}
		
		void DataGridView2DataSourceChanged(object sender, EventArgs e)
		{
			if(dataGridView2.RowCount>0){
				button4.Enabled=true;
			}else{
				button4.Enabled=false;
			}
		}
		
		void RadioButton5CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton5.Checked==true){
				comboBox1.Enabled=false;
				comboBox2.Enabled=false;
				groupBox4.Enabled=false;
				groupBox5.Enabled=true;
			}else{
				comboBox1.Enabled=true;
				comboBox2.Enabled=true;
				//groupBox4.Enabled=true;
				groupBox5.Enabled=false;
			}
		}
		
		void RadioButton4CheckedChanged(object sender, EventArgs e)
		{
		/*	if(radioButton4.Checked==true){
				comboBox1.Enabled=false;
				comboBox2.Enabled=false;
				groupBox4.Enabled=false;
				groupBox5.Enabled=true;
			}else{
				comboBox1.Enabled=true;
				comboBox2.Enabled=true;
				//groupBox4.Enabled=true;
				groupBox5.Enabled=false;
			}*/
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult respuesta = MessageBox.Show("El Proceso de Depuración termino correctamente\n¿Desea Realizar otra Depuración?", "Proceso concluido Exitosamente", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);

			if (respuesta == DialogResult.Yes)
			{
				//MessageBox.Show("" + MainForm.reset);
				MainForm.reset = 1;
				//MessageBox.Show(""+MainForm.reset);
				this.Close();
			}
			else
			{
				this.Close();
			}
		}

        private void button3_Click(object sender, EventArgs e)
        {
            Opcs_depu opciones = new Opcs_depu();
            opciones.ShowDialog();
        }

        public void leer_opciones()
        {
            //String verif_rale = "", pago_min = "";
            try
            {
                StreamReader rdr1 = new StreamReader(@"opcs_depu.lz");
                verif_rale = rdr1.ReadLine();
                pago_min = rdr1.ReadLine();
                rdr1.Close();

                verif_rale = verif_rale.Substring(11, verif_rale.Length - 11);
                pago_min = pago_min.Substring(9, pago_min.Length - 9);

                //MessageBox.Show("verif: "+verif_rale+", pago: "+pago_min);
            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al leer el archivo de opciones de depuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 24/08/2015
 * Hora: 02:12 p. m.
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

namespace Nova_Gear.Fecha_Not
{
	/// <summary>
	/// Description of Fecha_Not_Cartera.
	/// </summary>
	public partial class Fecha_Not_Cartera : Form
	{
		public Fecha_Not_Cartera()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String per,sql,sql2,t_c,t_d,t_p_c,exp_fecha,fecha,dai,mont,anio,fecha_act,id,valor_buscar,valor_actual,exp_fecha_express,tipo_cred_cons,cifra_ctrl,peri_2;
		int tot_rows=0,i=0,dia=0,mes=0,result=0,diag_ind=0,j=0,tot_act=0,tot_errs=0,totact2=0,k=0,repe=0,m=0,n=0,l=0,tipo_cons=0,z=0,fila_act=0,tot_pag=0;
		string[] text,text1;
		int[] res_busq,contros;
		DateTime fecha1;
		DialogResult res;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable grid1errs = new DataTable();
		DataTable grid2errs = new DataTable();
		DataTable estado_pers = new DataTable();

        DataTable lista_estrados = new DataTable();
		
		//periodos
		public void llenar_Cb2(){
			conex.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE status = \"EN TRAMITE\"  ORDER BY nombre_periodo;");
			do{
				comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
            conex.cerrar();
		}	
		//controladores
		public void llenar_Cb4(){
			conex.conectar("base_principal");
			comboBox4.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre,id_usuario FROM base_principal.usuarios WHERE puesto =\"Controlador\" ORDER BY apellido;");
			contros = new int[dataGridView3.RowCount];
			do{
				comboBox4.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				contros[i] = Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			comboBox4.Items.Add("--NINGUNO--");
            conex.cerrar();
		}	
		//notificadores
		public void llenar_Cb3(){
			conex.conectar("base_principal");
			comboBox3.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
			do{
				comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			comboBox3.Items.Add("--NINGUNO--");
            conex.cerrar();
		}
		//notificadores v2
		public void llenar_Cb3_v2(){
			conex2.conectar("base_principal");
			comboBox3.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex2.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" AND controlador = \""+contros[comboBox4.SelectedIndex]+"\" ORDER BY apellido;");
			do{
				comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			comboBox3.Items.Add("--NINGUNO--");
            conex2.cerrar();
		}
		
		public void consultar_bd(){
			
			conex.conectar("base_principal");
			dataGridView1.DataBindings.Clear();
			dataGridView2.DataBindings.Clear();
			tot_rows=0;
			per="";
			totact2=0;

			/*if ((checkBox1.Checked==false))
            {
				MessageBox.Show("Selecciona un periodo válido para la consulta","Error de Consulta");
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
			}else{*/
				try{
                sql = "SELECT id,nombre_periodo,registro_patronal1,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,notificador,observaciones FROM datos_factura WHERE id > 0 "+peri_2+" ";
                sql2 = " AND (status = \"EN TRAMITE\" ) AND observaciones <> \"PAGADO\" AND nn = \"-\" ORDER BY sector_notificacion_actualizado ASC,registro_patronal1 ASC,credito_cuotas ASC";
                
                if (comboBox4.SelectedIndex != -1)
                {
                    if(!(comboBox4.SelectedItem.ToString().Equals("--NINGUNO--")))
                    {
                    	sql += "AND controlador = \"" + comboBox4.SelectedItem.ToString() + "\" ";
                    }
                }

                if (comboBox3.SelectedIndex != -1)
                {
                    if (!(comboBox3.SelectedItem.ToString().Equals("--NINGUNO--")))
                    {
                    	sql += "AND notificador = \"" + comboBox3.SelectedItem.ToString() + "\" ";
                    }
                }

				sql=sql+sql2;
				}catch{}
				
				 if(checkBox1.Checked==true){//si estrados = activo
					sql = "SELECT id,nombre_periodo,registro_patronal1,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,notificador,observaciones FROM datos_factura WHERE "+
                	" (status = \"EN TRAMITE\" OR status =\"0\" OR status =\"CARTERA\") AND nn = \"ESTRADOS\" AND fecha_notificacion IS NULL ORDER BY sector_notificacion_actualizado ASC,registro_patronal1 ASC,credito_cuotas ASC";
                }
				consultamysql = conex.consultar(sql);
				dataGridView1.DataSource = consultamysql;
				dataGridView2.DataSource = null;
				tot_rows = dataGridView1.RowCount;
				label1.Text="Registros: "+dataGridView1.RowCount;
				
				formato_grid();
				
				dataGridView2.Columns.Clear();
				dataGridView2.Rows.Clear();
				dataGridView2.Columns.Add("FECHA_NOTIFICACION","RESULTADO DE LA NOTIFICACIÓN");
				
				if(tot_rows>1){
				dataGridView2.Rows.Add(tot_rows);
				}else{
				dataGridView2.Rows.Add(1);
				}
				
				dataGridView2.Columns[0].Width=130;
				dataGridView2.Columns[0].HeaderText="RESULTADO DE LA NOTIFICACIÓN";
				
				dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
				dataGridView2.Focus();
				conex.cerrar();
				
				button3.Enabled=true;
				maskedTextBox2.Enabled=true;
				radioButton3.Enabled=true;
				radioButton4.Enabled=true;
			//}


			//MessageBox.Show("Consulta: "+sql);
		}
		
		public void consultar_bd_ord_reg(){
			
			conex.conectar("base_principal");
			dataGridView1.DataBindings.Clear();
			dataGridView2.DataBindings.Clear();
			tot_rows=0;
			per="";
			totact2=0;

          /*  if ((comboBox2.SelectedIndex == -1))
            {
				MessageBox.Show("Selecciona un periodo válido para la consulta","Error de Consulta");
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
			}else{
		 */
                sql = "SELECT id,nombre_periodo,registro_patronal1,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,notificador,observaciones FROM datos_factura WHERE id > 0 " + peri_2+ " ";
                sql2 = " AND (status = \"EN TRAMITE\" ) AND observaciones <> \"PAGADO\" AND nn = \"-\" ORDER BY registro_patronal1 ASC,credito_cuotas ASC,sector_notificacion_actualizado ASC";
                
                if (comboBox4.SelectedIndex != -1)
                {
                    if(!(comboBox4.SelectedItem.ToString().Equals("--NINGUNO--")))
                    {
                        sql += "AND controlador = \"" + comboBox4.SelectedItem.ToString() + "\" ";
                    }
                }

                if (comboBox3.SelectedIndex != -1)
                {
                    if (!(comboBox3.SelectedItem.ToString().Equals("--NINGUNO--")))
                    {
                        sql += "AND notificador = \"" + comboBox3.SelectedItem.ToString() + "\" ";
                    }
                }

				sql=sql+sql2;
				consultamysql = conex.consultar(sql);
				dataGridView1.DataSource = consultamysql;
				dataGridView2.DataSource = null;
				tot_rows = dataGridView1.RowCount;
				label1.Text="Registros: "+dataGridView1.RowCount;
				
				formato_grid();
				
				dataGridView2.Columns.Clear();
				dataGridView2.Rows.Clear();
				dataGridView2.Columns.Add("FECHA_NOTIFICACION","RESULTADO DE LA NOTIFICACIÓN");
				
				if(tot_rows>1){
				dataGridView2.Rows.Add(tot_rows);
				}else{
				dataGridView2.Rows.Add(1);
				}
				
				dataGridView2.Columns[0].Width=130;
				dataGridView2.Columns[0].HeaderText="RESULTADO DE LA NOTIFICACIÓN";
				
				dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
				dataGridView2.Focus();
				conex.cerrar();
				
				button3.Enabled=true;
				maskedTextBox2.Enabled=true;
				radioButton3.Enabled=true;
				radioButton4.Enabled=true;
			//}


			//MessageBox.Show("Consulta: "+sql);
		}

		public void consultar_bd_cred()
		{
			conex.conectar("base_principal");
			dataGridView1.DataBindings.Clear();
			dataGridView2.DataBindings.Clear();
			tot_rows = 0;
			per = "";
			totact2 = 0;
			comboBox2.SelectedIndex = -1;
			comboBox3.SelectedIndex = -1;
			comboBox4.SelectedIndex = -1;

			if (!(maskedTextBox1.Text.Equals("        ")))
			{
				if(radioButton1.Checked==true){
					tipo_cred_cons="credito_cuotas =\"" + maskedTextBox1.Text + "\"";
				}
				
				if(radioButton2.Checked==true){
					tipo_cred_cons="credito_multa =\"" + maskedTextBox1.Text + "\"";
				}
				
				sql = "SELECT id,nombre_periodo,registro_patronal1,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,notificador,observaciones FROM datos_factura WHERE "+tipo_cred_cons;
                sql2 = " AND (status = \"EN TRAMITE\" ) AND observaciones <> \"PAGADO\" AND nn = \"-\" ORDER BY  registro_patronal1 ASC,credito_cuotas ASC";

				sql = sql + sql2;
				consultamysql = conex.consultar(sql);
				dataGridView1.DataSource = consultamysql;
				dataGridView2.DataSource = null;
				tot_rows = dataGridView1.RowCount;
				label1.Text = "Registros: " + dataGridView1.RowCount;

				formato_grid();

				dataGridView2.Columns.Clear();
				dataGridView2.Rows.Clear();
				dataGridView2.Columns.Add("FECHA_NOTIFICACION", "RESULTADO DE LA NOTIFICACIÓN");

				if (tot_rows > 1)
				{
					dataGridView2.Rows.Add(tot_rows);
				}
				else
				{
					dataGridView2.Rows.Add(1);
				}

				dataGridView2.Columns[0].Width = 130;
				dataGridView2.Columns[0].HeaderText = "RESULTADO DE LA NOTIFICACIÓN";

				dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
				dataGridView2.Focus();
				conex.cerrar();

				button3.Enabled = true;
				maskedTextBox2.Enabled = true;
				colocar_periodo();
				radioButton3.Enabled=false;
				radioButton4.Enabled=false;
			}
		}

		public void verificar(int y){
			String fecha_hoy,pagad;
			DateTime fecha_not,fecha_min;
			TimeSpan dif_fecha;
			int fecha_num=0;
			//exp_fecha=@"\A[0-3][0-9]/?[0-1][0-9]/?[0-9]{2,4}\Z";
			//exp_fecha_express=@"[0-3][0-9]";
			fecha=dataGridView2.Rows[y].Cells[0].FormattedValue.ToString();
			//System.Text.RegularExpressions.Regex verificador = new System.Text.RegularExpressions.Regex(exp_fecha);
			if(!(fecha.ToString().Equals(""))){
				//if(verificador.IsMatch(fecha)){
				/*if(DateTime.TryParse(fecha,out fecha_not)){
					if(fecha.Contains("/")){
						
						diag_ind= fecha.IndexOf('/');
						if(diag_ind != (fecha.LastIndexOf('/'))){
						    dia  = Convert.ToInt32(fecha.Substring(0,2));
						    mes  = Convert.ToInt32(fecha.Substring(3,2));
						    dai  = fecha.Substring(0,2);
						    mont = fecha.Substring(3,2);
						    anio = fecha.Substring(6,fecha.Length-6);
						}else{
							if(diag_ind==2){
								dia  = Convert.ToInt32(fecha.Substring(0,2));
								mes  = Convert.ToInt32(fecha.Substring(3,2));
								dai  = fecha.Substring(0,2);
						    	mont = fecha.Substring(3,2);
								anio = fecha.Substring(5,fecha.Length-5);
							}else{
								dia  = Convert.ToInt32(fecha.Substring(0,2));
						    	mes  = Convert.ToInt32(fecha.Substring(2,2));
						    	dai  = fecha.Substring(0,2);
						    	mont = fecha.Substring(2,2);
						    	anio = fecha.Substring(5,fecha.Length-5);
							}
						}
					}else{
						dia  = Convert.ToInt32(fecha.Substring(0,2));
						mes  = Convert.ToInt32(fecha.Substring(2,2));
						dai  = fecha.Substring(0,2);
						mont = fecha.Substring(2,2);
						anio = fecha.Substring(4,fecha.Length-4);
					}
					
					if(anio.Length!=4){
						anio="20"+anio.Substring(anio.Length-2,2);
					}
					
					fecha_act =dai+"/"+mont+"/"+anio;
					try{
						fecha1 =Convert.ToDateTime(fecha_act);
						if(fecha1>System.DateTime.Today){
							result=1;
						}
					}catch(Exception ex){
						result=1;
					}
				 */
				if(int.TryParse(fecha,out fecha_num)){
					if(fecha.Length==6){
						fecha=fecha.Substring(0,2)+"/"+fecha.Substring(2,2)+"/"+fecha.Substring(4,2);
					}
				}
				
				if(DateTime.TryParse(fecha,out fecha_not)){
					
					fecha_min=System.DateTime.Today.Date;
					dif_fecha=fecha_min.Subtract(fecha_not);
					//MessageBox.Show(""+dif_fecha);
					if(fecha_not <= System.DateTime.Today && dif_fecha.Days <= 1826 ){
						result=0;
					}else{
						result=1;
					}
					
					if(result==0){
						dataGridView2.Rows[y].Cells[0].Value = fecha_not.ToShortDateString();
						dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
						dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Green;
						
					}else{
						dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
						dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Red;
					}
				}else{///Segunda comprobación
					if(!(maskedTextBox3.Text.Equals("  /"))){
						if ((fecha.Length == 1)&&(!(fecha.ToUpper().Equals("P"))))
						{
							fecha = "0" + fecha;
						}

						if ((Int32.TryParse(fecha, out z)) == true)
						{
							if (fecha.Length == 2 && ((Convert.ToInt32(fecha)) < 32))
							{
								dataGridView2.Rows[y].Cells[0].Value = fecha + "/" + maskedTextBox3.Text;
								verificar(y);
							}
							else
							{
								dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
								dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Red;
							}
						}
						else
						{
							dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
							dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Red;
						}
					}else{
						dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
						dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Red;
					}

					//MessageBox.Show(fecha);
					pagad=fecha.ToUpper().Substring(0,1);
					if((pagad.Equals("P"))||(fecha.ToUpper().Equals("PAGADO"))){
						dataGridView2.Rows[y].Cells[0].Value="PAGADO";
						dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
						dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.SlateBlue;
						//MessageBox.Show("pagado");
					}else{
						try{
							if((fecha.ToUpper().Substring(0,2).Equals("NN"))&&fecha.Length==2){
								dataGridView2.Rows[y].Cells[0].Value="NN";
								dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.Black;
								dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Gold;
							}else{
								//dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.White;
								//dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.Red;
							}
						}catch{
							
						}
					}
				}
			}else{
				dataGridView2.Rows[y].Cells[0].Style.ForeColor = System.Drawing.Color.Black;
				dataGridView2.Rows[y].Cells[0].Style.BackColor = System.Drawing.Color.White;
			}
			
			result=0;
		}
		
		public String arreglar_fecha(String fechaori){
			fechaori=fechaori.Substring(6,4)+"/"+fechaori.Substring(3,2)+"/"+fechaori.Substring(0,2);
			return fechaori;
		}

        public void guardar()
        {

            int tot_nn = 0, tbx1_tot = 0, tbx4_tot = 0, arch_estra = 0;
            String fecha, evento, estado_ctrl, fecha_arreg, nombre_arch_estra = "";

            for (j = 0; j < tot_rows; j++)
            {
                if (dataGridView2.Rows[j].Cells[0].Style.BackColor == System.Drawing.Color.Green)
                {
                    totact2++;
                }
                if (dataGridView2.Rows[j].Cells[0].Style.BackColor == System.Drawing.Color.SlateBlue)
                {
                    tot_pag++;
                }
                if (dataGridView2.Rows[j].Cells[0].Style.BackColor == System.Drawing.Color.Gold)
                {
                    tot_nn++;
                }
            }

            res = MessageBox.Show("Se guardará la Fecha de Notificación de " + totact2 + " Registros.\nSe guardará el estado de PAGADO de " + tot_pag + " Registros.\nSe guardará el estado de NN de " + tot_nn + " Registros. \nSe omitirán: " + (tot_rows - (totact2 + tot_pag + tot_nn)) + " Registros que no estan llenados correctamente o están vacíos.\n\n¿Desea Continuar?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if ((res == DialogResult.Yes) && ((totact2 > 0) || (tot_pag > 0) || (tot_nn > 0)))
            {
                //libro excel
                XLWorkbook wb = new XLWorkbook();

                if (tot_nn > 0)
                {
                    SaveFileDialog dialog_save = new SaveFileDialog();
                    dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                    dialog_save.Title = "Guardar Archivo de Marcación para Estrados";//le damos un titulo a la ventana

                    if (dialog_save.ShowDialog() == DialogResult.OK)////pedir ubicación del archivo
                    {
                        //tabla_excel
                        
                        //wb.Worksheets.Add(indice, PDF_nom_arch);
                        //wb.SaveAs(@"" + dialog_save.FileName + "");
                        nombre_arch_estra = dialog_save.FileName;
                        arch_estra = 1;

                        if(lista_estrados.Columns.Contains("Registro Patronal")==false){
                            lista_estrados.Columns.Add("Registro Patronal");
                            lista_estrados.Columns.Add("Credito Cuota");
                            lista_estrados.Columns.Add("Credito Multa");
                        }

                        if(lista_estrados.Rows.Count>0){
                            lista_estrados.Rows.Clear();
                        }
                    }
                    else
                    {
                        arch_estra = 0;
                    }                    
                }
                else
                {
                    arch_estra = 2;
                }

                if ((arch_estra == 2) || (arch_estra == 1))
                {
                    

                    MainForm mani = (MainForm)this.MdiParent;
                    repe = 1;
                    tot_act = 0;

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    conex.conectar("base_principal");

                    //try{
                    for (j = 0; j < tot_rows; j++)
                    {
                        if (dataGridView2.Rows[j].Cells[0].Style.BackColor == System.Drawing.Color.Green)
                        {
                            fecha_arreg = arreglar_fecha(dataGridView2.Rows[j].Cells[0].Value.ToString());
                            estado_ctrl = fech_cifra_ctrl(fecha_arreg);
                            id = dataGridView1.Rows[j].Cells[0].Value.ToString();
                            textBox1.AppendText("UPDATE datos_factura SET fecha_notificacion=\"" + fecha_arreg + "\",status=\"NOTIFICADO\", fecha_recepcion=\"" + arreglar_fecha(System.DateTime.Today.ToShortDateString()) + "\", estado_cifra_control=\"" + estado_ctrl + "\" WHERE id =\"" + id + "\"\r\n");
                            evento = conex.linea_evento("Se Actualizó el status del crédito con el ID: " + id + " a NOTIFICADO");
                            textBox1.AppendText(evento + "\r\n");
                            //textBox1.AppendText("UPDATE datos_factura SET status=\"PRUEBA\" WHERE id =\""+id+"\"\n");
                            //textBox1.AppendText("UPDATE datos_factura SET fecha_recepcion=\""+arreglar_fecha(System.DateTime.Today.ToShortDateString())+"\" WHERE id =\""+id+"\"\n");
                            tbx1_tot = tbx1_tot + 2;
                            //mani.toolStripStatusLabel1.Text = "Registros Leidos: " + tot_act + "/"+totact2;
                            //mani.statusStrip1.Refresh();
                        }
                        else
                        {
                            if (dataGridView2.Rows[j].Cells[0].Style.BackColor == System.Drawing.Color.SlateBlue)
                            {
                                id = dataGridView1.Rows[j].Cells[0].Value.ToString();
                                textBox4.AppendText("UPDATE datos_factura SET observaciones=\"PAGADO\", fecha_recepcion=\"" + arreglar_fecha(System.DateTime.Today.ToShortDateString()) + "\" WHERE id =\"" + id + "\"\r\n");
                                evento = conex.linea_evento("Se Actualizó el estado del crédito con el ID: " + id + " a PAGADO");
                                textBox4.AppendText(evento + "\r\n");
                                tbx4_tot = tbx4_tot + 2;
                            }
                            else
                            {
                                if (dataGridView2.Rows[j].Cells[0].Style.BackColor == System.Drawing.Color.Gold)
                                {
                                    id = dataGridView1.Rows[j].Cells[0].Value.ToString();
                                    textBox4.AppendText("UPDATE datos_factura SET nn=\"NN\", fecha_recepcion=\"" + arreglar_fecha(System.DateTime.Today.ToShortDateString()) + "\" WHERE id =\"" + id + "\"\r\n");
                                    evento = conex.linea_evento("Se Actualizó el estado del crédito con el ID: " + id + " a NN");
                                    textBox4.AppendText(evento + "\r\n");
                                    tbx4_tot = tbx4_tot + 2;

                                    lista_estrados.Rows.Add(dataGridView1.Rows[j].Cells[2].Value.ToString().Substring(0, 3) + "-" + dataGridView1.Rows[j].Cells[2].Value.ToString().Substring(3, 5) + "-" + dataGridView1.Rows[j].Cells[2].Value.ToString().Substring(8, 2) + "-" + dataGridView1.Rows[j].Cells[2].Value.ToString().Substring(10, 1),
                                                            dataGridView1.Rows[j].Cells[4].Value.ToString(),
                                                            dataGridView1.Rows[j].Cells[5].Value.ToString());
                                }
                                else
                                {
                                    textBox2.AppendText(dataGridView1.Rows[j].Cells[0].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[1].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[2].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[3].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[4].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[5].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[6].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[7].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[8].Value.ToString() + "$"
                                                       + dataGridView1.Rows[j].Cells[9].Value.ToString() + "\r\n"
                                                      );

                                    if (dataGridView2.Rows[j].Cells[0].Value == null)
                                    {
                                        textBox3.AppendText("vacio\n");
                                    }
                                    else
                                    {
                                        textBox3.AppendText(dataGridView2.Rows[j].Cells[0].Value.ToString() + "\n");
                                    }
                                    tot_errs++;
                                }
                            }
                        }
                    }

                    if (totact2 > 0)
                    {
                        k = 0;
                        do
                        {
                            text = textBox1.Lines;
                            sql = text[k];
                            conex.consultar(sql);
                            k++;
                            //mani.toolStripStatusLabel1.Text = "Registros actualizados: " + (k)+ "/"+totact2;
                            //mani.statusStrip1.Refresh();
                        } while (k < tbx1_tot);
                    }

                    if ((tot_pag + tot_nn) > 0)
                    {
                        k = 0;
                        do
                        {
                            text = textBox4.Lines;
                            sql = text[k];
                            conex.consultar(sql);
                            k++;
                            //mani.toolStripStatusLabel1.Text = "Registros actualizados: " + (k)+ "/"+totact2;
                            //mani.statusStrip1.Refresh();
                        } while (k < tbx4_tot);
                    }

                    k = 0;
                    grid1errs.Rows.Clear();
                    while (k < tot_errs)
                    {
                        text = textBox2.Lines;
                        sql = text[k];
                        text1 = sql.Split('$');
                        grid1errs.Rows.Add(text1[0], text1[1], text1[2], text1[3], text1[4], text1[5], text1[6], text1[7], text1[8], text1[9]);
                        k++;
                        //mani.toolStripStatusLabel1.Text = "Procesando Erroneos: " + (k)+ "/"+tot_errs;
                        //mani.statusStrip1.Refresh();
                    }

                    k = 0;
                    grid2errs.Rows.Clear();
                    while (k < tot_errs)
                    {
                        text = textBox3.Lines;
                        sql = text[k];
                        if (sql.Equals("vacio"))
                        {
                            grid2errs.Rows.Add();
                        }
                        else
                        {
                            grid2errs.Rows.Add(sql);
                        }
                        k++;
                        //mani.toolStripStatusLabel1.Text = "Procesando Erroneos: " + (k)+ "/"+tot_errs;
                        //mani.statusStrip1.Refresh();
                    }

                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;
                    if (repe == 1)
                    {
                        dataGridView2.Columns.Clear();
                    }

                    dataGridView1.DataSource = grid1errs;
                    dataGridView2.DataSource = grid2errs;

                    formato_grid();

                    tot_rows = dataGridView1.RowCount;
                    label1.Text = "Registros: " + dataGridView1.RowCount;

                    dataGridView2.Columns[0].Width = 170;
                    dataGridView2.Columns[0].HeaderText = "RESULTADO DE LA NOTIFICACIÓN";

                    dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Focus();
                    conex.cerrar();

                    for (k = 0; k < dataGridView2.RowCount; k++)
                    {
                        verificar(k);
                    }

                    MessageBox.Show("Se Actualizaron " + (totact2 + tot_pag + tot_nn) + " registros\nSe Omitieron " + tot_errs + " registros\n", "¡EXITO!");
                    /*try
                    {
                        if (tipo_cons == 1)
                        {
                            conex.guardar_evento("Se Actualizaron " + tot_act + " registros del periodo " + comboBox2.SelectedItem.ToString() + " como Notificados/Pagados");
                        }
                        else
                        {
                            conex.guardar_evento("Se Actualizó  el  crédito " + maskedTextBox1.Text + " como Notificado/Pagado");
                        }
                    }
                    catch { }*/
                    dataGridView2.Focus();
                    comboBox2.Enabled = true;
                    /*	}catch(Exception e){
                        MessageBox.Show(" Algo salio mal.\n El proceso no concluyó adecuadamente.\n\n Error:\n"+e,"Información",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }*/
                    if(arch_estra==1){
                        wb.Worksheets.Add(lista_estrados, "nn_a_estrados");
                        wb.SaveAs(@"" + nombre_arch_estra+ "");
                        MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    MessageBox.Show("Debe especificar el nombre y la ubicacion del archivo de Marcación para Estrados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se hará nada", "Aviso");
            }

            tot_act = 0;
            tot_errs = 0;
            totact2 = 0;
            tot_pag = 0;
            k = 0;
        }
		
		public void formato_grid(){
			
			Font fuente = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				
				dataGridView1.Columns[4].DefaultCellStyle.Font = fuente;
				dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			
				dataGridView1.Columns[0].HeaderText="ID";
				dataGridView1.Columns[0].Width=50;
				dataGridView1.Columns[1].HeaderText="NOMBRE DE PERIODO";
				dataGridView1.Columns[1].Width=150;
				dataGridView1.Columns[2].HeaderText="REGISTRO PATRONAL";
				dataGridView1.Columns[3].HeaderText="RAZÓN SOCIAL";	
				dataGridView1.Columns[3].Width=200;				
				dataGridView1.Columns[4].HeaderText="CRÉDITO CUOTA";
				dataGridView1.Columns[4].Width=85;
				dataGridView1.Columns[4].Name = "credito_cuo";
				dataGridView1.Columns[5].HeaderText="CRÉDITO MULTA";
				dataGridView1.Columns[5].Width=75;
				dataGridView1.Columns[6].HeaderText="IMPORTE CUOTA";
				dataGridView1.Columns[7].HeaderText="IMPORTE MULTA";
				dataGridView1.Columns[8].HeaderText="NOTIFICADOR";
				dataGridView1.Columns[8].Width=120;
				dataGridView1.Columns[9].HeaderText="OBSERVACIONES";
				dataGridView1.Columns[9].Width=110;

                dataGridView1.Columns[0].Visible = false;

                if (tipo_cons == 1)
                {
                    dataGridView1.Columns[1].Visible = false;
                }
                else
                {
                    dataGridView1.Columns[1].Visible = true;
                }

				for(i=0;i<=9;i++){
				dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				}
				dataGridView1.FirstDisplayedScrollingColumnIndex=2;	
		}
		
		public void buscar(){
			valor_buscar = maskedTextBox2.Text;
			res_busq = new int[tot_rows+1];
			m=0;
			n=0;
			//MessageBox.Show(valor_buscar);
			//MessageBox.Show(dataGridView1.Rows[m].Cells[4].Value.ToString());
			
			if(valor_buscar.Length >0){
				for(m=0;m<=(tot_rows-1);m++){
					valor_actual=dataGridView1.Rows[m].Cells[4].FormattedValue.ToString();
					if(valor_buscar.Equals(valor_actual)){
						res_busq[n] = m;
						n++;
					}
				}
			  //MessageBox.Show(valor_buscar+"_"+m+"_"+tot_rows);
			 
				l=0;
				if(n>=1){
					dataGridView1.Rows[res_busq[l]].Cells[4].Selected=true;
					if(res_busq[l]<5){
						dataGridView1.FirstDisplayedScrollingRowIndex=0;
					}else{
			   			dataGridView1.FirstDisplayedScrollingRowIndex=(res_busq[l])-5;
					}
					//dataGridView1.Focus();
			   		if(n>1){
			   		button5.Enabled=true;
			   		}
			   		
					label2.Text="Resultado "+(l+1)+" de "+n;
					dataGridView2.Focus();
					dataGridView2.Rows[res_busq[l]].Cells[0].Selected=true;
					
				}else{
					label2.Text="Búsqueda sin Resultados";	
				}
				 //label2.Text="Resultado 1 de: "+n;
			}
			
		}

        public void colocar_periodo(){
           int x = 0;
           if(dataGridView1.RowCount>0){
               do{
                   if(dataGridView1.Rows[0].Cells[1].FormattedValue.Equals(comboBox2.Items[x].ToString())){
                       comboBox2.SelectedIndex = x;
                   }
                   x++;
                }while(x<comboBox2.Items.Count);
           }
        }
        
		public String fech_cifra_ctrl(String fecha_not){
			String fecha_ctrl,estado_cif_ctrl="-";
			
			estado_pers=conex3.consultar("SELECT fecha_cifra_control_termino FROM estado_periodos WHERE nombre_periodo=\""+comboBox2.Text+"\"");
			if(estado_pers.Rows.Count>0){
				fecha_ctrl=estado_pers.Rows[0][0].ToString();
				if(fecha_ctrl.Length>5){
					if((Convert.ToDateTime(fecha_ctrl))>=(Convert.ToDateTime(fecha_not))){
						estado_cif_ctrl="DENTRO";
					}else{
						estado_cif_ctrl="FUERA";
					}
				}
			}
			return estado_cif_ctrl;
		}
		
		void Fecha_Not_CarteraLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			//this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2)-30;
			//this.Left = ((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2)-150;

			toolTip1.SetToolTip(button2,"Hacer consulta a\nla base de datos.");
			toolTip1.SetToolTip(button1,"Guardar fechas de\nnotificación ingresadas en la BD.");
		
			grid1errs.Columns.Add("ID");
			grid1errs.Columns.Add("NOMBRE DE PERIODO");
			grid1errs.Columns.Add("REGISTRO PATRONAL");
			grid1errs.Columns.Add("RAZÓN SOCIAL");
			grid1errs.Columns.Add("CRÉDITO CUOTA");
			grid1errs.Columns.Add("CRÉDITO MULTA");
			grid1errs.Columns.Add("IMPORTE CUOTA");
			grid1errs.Columns.Add("IMPORTE MULTA");
			grid1errs.Columns.Add("NOTIFICADOR");
			grid1errs.Columns.Add("FECHA DE NOTIFICACIÓN");
			grid1errs.Columns.Add("OBSERVACIONES");
				
			grid2errs.Columns.Add("FECHA DE NOTIFICACIÓN");
			
			//comboBox2.SelectedIndex=0;
			//comboBox3.SelectedIndex=0;
			//comboBox4.SelectedIndex=0;
			llenar_Cb2();
			llenar_Cb3();
			llenar_Cb4();
			
			conex3.conectar("base_principal");
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void Button2Click(object sender, EventArgs e)
		{
            if(maskedTextBox1.Text.Equals("")){
                tipo_cons = 1;
                consultar_bd();
            }else{
                tipo_cons = 2;
                consultar_bd_cred();
            }
           // MessageBox.Show("|"+maskedTextBox1.Text+"|"+tipo_cons);
			
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
               tipo_cons = 2;
				consultar_bd_cred();
		   }
		}
		
		void DataGridView1Scroll(object sender, ScrollEventArgs e)
		{
			if((dataGridView2.RowCount>0)&&(dataGridView1.RowCount>0)){
			dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
			dataGridView1.Focus();
			}
		}
		
		void DataGridView2Scroll(object sender, ScrollEventArgs e)
		{
			dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView2.FirstDisplayedScrollingRowIndex;
		}
		
		void DataGridView1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
				if(dataGridView1.RowCount>0){
					dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
				}
		   }
			
			if (e.KeyChar == (char)(Keys.Up))
           {
				if(dataGridView1.RowCount>0){
					dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
		   		}
			}
			
			if (e.KeyChar == (char)(Keys.Down))
           {
				if(dataGridView1.RowCount>0){
					dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
		   		}
			}
		}
		
		void DataGridView2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				if(dataGridView2.RowCount>0){
					dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView2.FirstDisplayedScrollingRowIndex;
				}
			}
			
			if (e.KeyChar == (char)(Keys.Up))
			{
				if(dataGridView2.RowCount>0){
					dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView2.FirstDisplayedScrollingRowIndex;
				}
			}
			if (e.KeyChar == (char)(Keys.Down))
			{
				if(dataGridView2.RowCount>0){
					dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView2.FirstDisplayedScrollingRowIndex;
				}
			}
			
			if (e.KeyChar == (char)(Keys.Delete))
			{
				if(dataGridView2.RowCount>0){
					dataGridView2.CurrentCell.Value = "";
				}
			}
		}
		
		void DataGridView1CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			try{
				if((dataGridView2.RowCount>0)&&(dataGridView1.RowCount>0)){
					dataGridView2.EndEdit();
					dataGridView2.ClearSelection();
					dataGridView2.Rows[e.RowIndex].Cells[0].Selected=true;
					verificar(e.RowIndex);
				}
			}catch(Exception exc){
				
			}
		}
		
		void DataGridView2CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			try{
				if((dataGridView1.RowCount>0)&&(dataGridView2.RowCount>0)){
					verificar(e.RowIndex);
				}
			}catch(Exception exc){
				
			}
		}
		
		void DataGridView1CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			if(dataGridView2.RowCount>0){
				//verificar(e.RowIndex);
				/*dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
				dataGridView2.Rows[dataGridView1.CurrentRow.Index].Cells[0].Selected=false;*/
			}
		   
		}
		
		void DataGridView2CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			if(dataGridView2.RowCount>0){
				//verificar(e.RowIndex);
				/*dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
				dataGridView2.Rows[dataGridView1.CurrentRow.Index].Cells[0].Selected=false;*/
			}
		}
		
		void DataGridView1CellClick(object sender, DataGridViewCellEventArgs e)
		{
			dataGridView2.EndEdit();
			dataGridView1.Focus();
		}
		
		void DataGridView2CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try{
			if((dataGridView1.RowCount>0)&&(dataGridView2.RowCount>0)){
				dataGridView1.Rows[e.RowIndex].Selected=true;
			}
			}catch(Exception es){
				
			}
		}
		
		void DataGridView2CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if(dataGridView2.RowCount>0){
				verificar(dataGridView2.CurrentRow.Index);
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			comboBox2.Enabled=false;
            maskedTextBox1.Enabled = false;
			guardar();
            comboBox2.Enabled = true;
            maskedTextBox1.Enabled = true;
            maskedTextBox1.Clear();
            maskedTextBox1.Focus();
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
				buscar();
		   }
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if((l-1)>=0){
				l=l-1;
				dataGridView1.Rows[res_busq[l]].Cells[0].Selected=true;
			   	if(res_busq[l]<5){
					dataGridView1.FirstDisplayedScrollingRowIndex=0;
				}else{
			   		dataGridView1.FirstDisplayedScrollingRowIndex=(res_busq[l])-5;
				}
				label2.Text="Resultado "+(l+1)+" de "+n;
				button5.Enabled=true;
				dataGridView1.Focus();
				if(l==n){
					button5.Enabled=true;
				}
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			if((l+1)<= n){
				l=l+1;
				dataGridView1.Rows[res_busq[l]].Cells[0].Selected=true;
			   	if(res_busq[l]<5){
					dataGridView1.FirstDisplayedScrollingRowIndex=0;
				}else{
			   		dataGridView1.FirstDisplayedScrollingRowIndex=(res_busq[l])-5;
				}
				label2.Text="Resultado "+(l+1)+" de "+n;
				button4.Enabled=true;
				dataGridView1.Focus();
				if(l==n){
					button5.Enabled=true;
				}
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			buscar();
		}
		
		void MaskedTextBox2Click(object sender, EventArgs e)
		{
			maskedTextBox2.SelectionStart=0;
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void MaskedTextBox2Enter(object sender, EventArgs e)
		{
			if(maskedTextBox2.Text.Length == 9){
				maskedTextBox2.Clear();
			}
		}
		
		void Label3Click(object sender, EventArgs e)
		{
			
		}
		
		void Label6Click(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox4SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox4.SelectedItem != null){
					if(!comboBox4.SelectedItem.ToString().Equals("--NINGUNO--")){
					llenar_Cb3_v2();
				}
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedIndex != -1){
				//button2.Enabled=true;
           	 	//comboBox3.Enabled = true;
            	//comboBox4.Enabled = true;
            	peri_2="AND nombre_periodo =\"" + comboBox2.SelectedItem.ToString() + "\" ";
			}else{
				//button2.Enabled=false;
            	//comboBox3.Enabled = false;
           		//comboBox4.Enabled = false;
           		peri_2="";
			}
		}
		
		void MaskedTextBox2MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton1.Checked==true){
				label3.Text="Crédito Cuota:";
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton2.Checked==true){
				label3.Text="Crédito Multa:";
			}
		}
		
		void ComboBox2TextUpdate(object sender, EventArgs e)
		{
			if((comboBox2.Items.Contains(comboBox2.Text))==true){
				button2.Enabled=true;
            	comboBox3.Enabled = true;
           		comboBox4.Enabled = true;
			}else{
				button2.Enabled=false;
            	comboBox3.Enabled = false;
           		comboBox4.Enabled = false;
			}
			
		}
		
		void DataGridView2Enter(object sender, EventArgs e)
		{
			
		}
		
		void DataGridView2CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void DataGridView2CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			
		}
		//sector
		void RadioButton4CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton4.Checked==true){
				DialogResult resu2= MessageBox.Show("Si cambia el orden se reiniciará la tabla y\n se borrará toda la información capturada.\n¿Desea Continuar?\n\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
				if(resu2 == DialogResult.Yes){
					consultar_bd();
				}
			}
		}
		//reg_pat
		void RadioButton3CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton3.Checked==true){
				DialogResult resu= MessageBox.Show("Si cambia el orden se reiniciará la tabla y\n se borrará toda la información capturada.\n¿Desea Continuar?\n\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
				if(resu == DialogResult.Yes){
					consultar_bd_ord_reg();
				}
			}
		}
		
		void MaskedTextBox1MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true){
				button2.Enabled=true;
			}else{
				if((comboBox2.SelectedIndex < 0)||(maskedTextBox1.Text.Length < 9)){
					button2.Enabled=false;
				}
			}
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void BuscarToolStripMenuItemClick(object sender, EventArgs e)
		{
			maskedTextBox2.Clear();
			maskedTextBox2.Focus();
		}
	}
}

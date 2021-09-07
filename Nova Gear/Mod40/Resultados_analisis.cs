/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 20/09/2016
 * Hora: 10:43 a.m.
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
using Ionic.Zip;

namespace Nova_Gear.Mod40
{
	/// <summary>
	/// Description of Resultados_analisis.
	/// </summary>
	public partial class Resultados_analisis : Form
	{
		
		public Resultados_analisis()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String cons_exc,cons_exc2,nss,hoja,arch_nombre_corto;
		int indice=0,permiso_impri=0;
		
		//Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        OleDbConnection conexion1 = null;
        DataSet dataSet = null;
        DataSet dataSet1 = null;
        OleDbDataAdapter dataAdapter = null;
        OleDbDataAdapter dataAdapter1 = null;
        
        DataTable tabla_varios_inicial = new DataTable();
        DataTable tabla_uno_solo_inicial = new DataTable();
        
        DataTable tabla_varios = new DataTable();
        DataTable tabla_uno_solo = new DataTable();
        
        DataTable tabla_impri = new DataTable();
		
        //ABRE ARCHIVO	
        public void recibe_arch(String archivo_mod40, String archivo_mod40_nombre_corto){
        	arch_nombre_corto=archivo_mod40_nombre_corto;
        	
        	try{
        		System.IO.File.Delete(@"temp/mod40_un_periodo.xlsx");
        		System.IO.File.Delete(@"temp/mod40_resultados.xlsx");
        	}catch(Exception es0){
        	}
        	
        	try{
        		ZipFile arch_m40 = ZipFile.Read(archivo_mod40);
        		
        		foreach (ZipEntry e in arch_m40)
        		{
        			e.Extract(@"temp/",ExtractExistingFileAction.OverwriteSilently);
        		}
        		
        		abrir_resultados();
        		abrir_un_solo_per();
        		formato_dgv1();
        		formato_dgv2();
        		contar_pers();
        		//marcado_inicial();
        		toolStripStatusLabel1.Text=archivo_mod40;
        		label1.Text="Casos: "+dataGridView1.RowCount;
        		label3.Text="Casos: "+dataGridView2.RowCount;
        	}catch(Exception es){
        		MessageBox.Show("Ocurrió el siguiente problema al leer el archivo de resultados,:\n\n"+es);
        		//this.Close();
        		
        	}
        }
        
        public void abrir_resultados(){
        	String cad_con;
        	cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temp/mod40_resultados.xlsx';Extended Properties=Excel 12.0;";
        	conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
        	conexion.Open();
        	carga_chema_excel();
        	carga_excel("Select * from ["+hoja+"$]");
        	conexion.Close();
        }
        
        public void carga_excel(String cons_exc){
			
			dataAdapter= new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
			// creamos la instancia del objeto DataSet
			dataSet = new DataSet();
			dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
			tabla_varios_inicial = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
			dataGridView1.DataSource = tabla_varios_inicial;
			//conexion3.Close();//cerramos la conexion
			dataGridView1.AllowUserToAddRows = false;
			
		}
        
        public void abrir_un_solo_per(){
        	String cad_con1;
        	cad_con1 = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='temp/mod40_un_periodo.xlsx';Extended Properties=Excel 12.0;";
        	conexion1 = new OleDbConnection(cad_con1);//creamos la conexion con la hoja de excel
        	conexion1.Open();
        	carga_excel1("Select * from ["+hoja+"$]");
        	conexion1.Close();
        }
		
		public void carga_excel1(String cons_exc2){
			
			dataAdapter1= new OleDbDataAdapter(cons_exc2, conexion1); //traemos los datos de la hoja y las guardamos en un dataSdapter
			// creamos la instancia del objeto DataSet
			dataSet1 = new DataSet();
			dataAdapter1.Fill(dataSet1, hoja);//llenamos el dataset
			tabla_uno_solo_inicial = dataSet1.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
			dataGridView2.DataSource = tabla_uno_solo_inicial; 
			//conexion3.Close();//cerramos la conexion
			dataGridView2.AllowUserToAddRows = false;
		}
        
        public void carga_chema_excel(){
        	int i=0,filas = 0;
        	String tabla="";
        	
        	comboBox1.Items.Clear();
        	System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        	dataGridView2.DataSource =dt;
        	filas=(dataGridView2.RowCount)-1;
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
        	
        	dt.Clear();
        	dataGridView2.DataSource = dt; //vaciar datagrid
        	hoja=comboBox1.Items[0].ToString();
            label5.Text = "Factura Analizada: " + hoja.Substring(8);
        }
        
        public void formato_dgv1(){
        	dataGridView1.Columns[0].HeaderText="ID";
        	dataGridView1.Columns[1].HeaderText="NUMERO DE PERIODOS";
        	dataGridView1.Columns[2].HeaderText="NSS";
        	dataGridView1.Columns[3].HeaderText="ASEGURADO";
        	dataGridView1.Columns[3].MinimumWidth=300;
        	dataGridView1.Columns[4].HeaderText="PERIODOS EN MORA";
        	dataGridView1.Columns[4].MinimumWidth=500;
        	dataGridView1.Columns[5].Visible=false;
        	
        	dataGridView1.Columns[0].SortMode= DataGridViewColumnSortMode.NotSortable;
        	dataGridView1.Columns[1].SortMode= DataGridViewColumnSortMode.NotSortable;
        	dataGridView1.Columns[2].SortMode= DataGridViewColumnSortMode.NotSortable;
        	dataGridView1.Columns[3].SortMode= DataGridViewColumnSortMode.NotSortable;
        	dataGridView1.Columns[4].SortMode= DataGridViewColumnSortMode.NotSortable;
        	dataGridView1.Columns[5].SortMode= DataGridViewColumnSortMode.NotSortable;
        	
        }
        
        public void formato_dgv2(){
        	dataGridView2.Columns[0].HeaderText="ID";
        	dataGridView2.Columns[1].HeaderText="NSS";
        	dataGridView2.Columns[2].HeaderText="ASEGURADO";
        	dataGridView2.Columns[2].MinimumWidth=300;
        	dataGridView2.Columns[3].HeaderText="PERIODO EN MORA";
        	dataGridView2.Columns[4].Visible=false;
        }
        
        public void contar_pers(){
        	int tot=0,y=0;
        	Font fuente2 = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	while(y<dataGridView1.RowCount){
        		tot=(dataGridView1.Rows[y].Cells[4].FormattedValue.ToString().Length + 1) / 7;
        		dataGridView1.Rows[y].Cells[1].Value=tot;
        		if(dataGridView1.Rows[y].Cells[1].Value.ToString().Equals("2")){
        			dataGridView1.Rows[y].Cells[0].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
	        		dataGridView1.Rows[y].Cells[0].Style.ForeColor=System.Drawing.Color.White;
	        		dataGridView1.Rows[y].Cells[1].Style.BackColor=System.Drawing.Color.DarkGreen;
	        		dataGridView1.Rows[y].Cells[1].Style.ForeColor=System.Drawing.Color.White;
	        		dataGridView1.Rows[y].Cells[1].Style.Font=fuente2;
	        		dataGridView1.Rows[y].Cells[2].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
	        		dataGridView1.Rows[y].Cells[2].Style.ForeColor=System.Drawing.Color.White;
	        		dataGridView1.Rows[y].Cells[3].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
	        		dataGridView1.Rows[y].Cells[3].Style.ForeColor=System.Drawing.Color.White;
	        		dataGridView1.Rows[y].Cells[4].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
	        		dataGridView1.Rows[y].Cells[4].Style.ForeColor=System.Drawing.Color.White;
	        		dataGridView1.Rows[y].Cells[5].Value="MARCADO";
        		}
        		y++;
        	}
        }
        
        public void marcado_inicial(){
        	int y=0,x=0,z=0;
        	Font fuente = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        	try{
        		while(x <dataGridView1.RowCount){
        			if(!dataGridView1.Rows[x].Cells[5].FormattedValue.ToString().Equals("MARCADO")){
        				z=z+1;
        				if(z==1){
        					y=x;
        				}
        			}
        			
        			//MessageBox.Show(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
        			if(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString().Equals("MARCADO")){
        				dataGridView1.Rows[x].Cells[0].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[x].Cells[0].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[x].Cells[1].Style.BackColor=System.Drawing.Color.DarkGreen;
        				dataGridView1.Rows[x].Cells[1].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[x].Cells[1].Style.Font=fuente;
        				dataGridView1.Rows[x].Cells[2].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[x].Cells[2].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[x].Cells[3].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[x].Cells[3].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[x].Cells[4].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[x].Cells[4].Style.ForeColor=System.Drawing.Color.White;
        			}
        			x++;
        		}
        		
        		dataGridView1.FirstDisplayedScrollingRowIndex = y;
        	}catch(Exception es){
        		
        	}
        }
        
        public void marcar_click(){
        	Font fuente = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	
        	try{
        	    dataGridView1.Rows[indice].Cells[0].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        		dataGridView1.Rows[indice].Cells[0].Style.ForeColor=System.Drawing.Color.White;
        		dataGridView1.Rows[indice].Cells[1].Style.BackColor=System.Drawing.Color.DarkGreen;
        		dataGridView1.Rows[indice].Cells[1].Style.ForeColor=System.Drawing.Color.White;
        		dataGridView1.Rows[indice].Cells[1].Style.Font=fuente;
        		dataGridView1.Rows[indice].Cells[2].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        		dataGridView1.Rows[indice].Cells[2].Style.ForeColor=System.Drawing.Color.White;
        		dataGridView1.Rows[indice].Cells[3].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        		dataGridView1.Rows[indice].Cells[3].Style.ForeColor=System.Drawing.Color.White;
        		dataGridView1.Rows[indice].Cells[4].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        		dataGridView1.Rows[indice].Cells[4].Style.ForeColor=System.Drawing.Color.White;
        		dataGridView1.Rows[indice].Cells[5].Value="MARCADO";
        		
        		timer2.Enabled=true;
        	}catch(Exception es){
        		
        	}
        }
        
        public void desmarcar_click(){
        	Font fuente1 = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	
        	try{
        	    dataGridView1.Rows[indice].Cells[0].Style.BackColor=System.Drawing.SystemColors.Window;
        		dataGridView1.Rows[indice].Cells[0].Style.ForeColor=System.Drawing.SystemColors.ControlText;
        		dataGridView1.Rows[indice].Cells[1].Style.BackColor=System.Drawing.SystemColors.Window;
        		dataGridView1.Rows[indice].Cells[1].Style.ForeColor=System.Drawing.SystemColors.ControlText;
        		dataGridView1.Rows[indice].Cells[1].Style.Font=fuente1;
        		dataGridView1.Rows[indice].Cells[2].Style.BackColor=System.Drawing.SystemColors.Window;
        		dataGridView1.Rows[indice].Cells[2].Style.ForeColor=System.Drawing.SystemColors.ControlText;
        		dataGridView1.Rows[indice].Cells[3].Style.BackColor=System.Drawing.SystemColors.Window;
        		dataGridView1.Rows[indice].Cells[3].Style.ForeColor=System.Drawing.SystemColors.ControlText;
        		dataGridView1.Rows[indice].Cells[4].Style.BackColor=System.Drawing.SystemColors.Window;
        		dataGridView1.Rows[indice].Cells[4].Style.ForeColor=System.Drawing.SystemColors.ControlText;
        		dataGridView1.Rows[indice].Cells[5].Value=" ";
        		
        		timer2.Enabled=true;
        	}catch(Exception es){
        		
        	}
        }
        
        public void contar_marcados(){
        	
        	int cuenta=0,x=0;
        	
        	while(x < dataGridView1.RowCount){
        		if(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString().Equals("MARCADO")){
        			cuenta++;
        		}
        		x++;
        	}
        	label2.Text="Marcados: "+cuenta;
        	
        	if(cuenta >= dataGridView1.RowCount){
        		permiso_impri=1;
        	}else{
        		permiso_impri=0;
        	}
        }
        
        public void conciliar(){
        	int tot=0;
        	String periodos="";
        	Font fuente = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        
        	Consulta_Mod40 cons = new Consulta_Mod40();
        	if(cons.buscar_valor(nss,1)==true){
        		if(panel1.Visible==true){
        			cons.ShowDialog();
        			periodos=cons.pasar_list();
        			
        			tot=(periodos.Length + 1) / 7;
        			//MessageBox.Show(""+tot);
        			periodos=cortar_periodos(periodos,tot);
        			tot=(periodos.Length + 1) / 7;
        			
        			dataGridView1.Rows[indice].Cells[4].Value=periodos;
        			dataGridView1.Rows[indice].Cells[1].Value=tot;
        			
        			//MessageBox.Show(""+dataGridView1.Rows[indice].Cells[4].Value.ToString().Length);
        			//MessageBox.Show(""+tot);
        			
        			if(tot==0){
        				MessageBox.Show("Este Asegurado ya no presenta periodos en mora hasta el periodo del Cd Analizado,\nse quitará de esta lista.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Information);
        				dataGridView1.Rows.RemoveAt(indice);
        				dataGridView1.Refresh();
        			}
        			
        			if(tot==1){
        				MessageBox.Show("Este Asegurado presenta sólo un periodo en mora hasta el periodo del Cd Analizado,\nse moverá a lista de asegurados con un sólo periodo en mora.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Information);
        				tabla_uno_solo_inicial.Rows.Add(((Convert.ToInt32(dataGridView2.Rows[dataGridView2.RowCount-1].Cells[0].FormattedValue.ToString()))+1).ToString(),
        				                       dataGridView1.Rows[indice].Cells[2].FormattedValue.ToString(),
        				                       dataGridView1.Rows[indice].Cells[3].FormattedValue.ToString(),
        				                       dataGridView1.Rows[indice].Cells[4].FormattedValue.ToString());
        				dataGridView1.Rows.RemoveAt(indice);
        				dataGridView1.Refresh();
        				dataGridView2.Refresh();
        			}
        			
        			if(tot>1){
        				/*dataGridView1.Rows[indice].Cells[0].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[indice].Cells[0].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[indice].Cells[1].Style.BackColor=System.Drawing.Color.DarkGreen;
        				dataGridView1.Rows[indice].Cells[1].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[indice].Cells[1].Style.Font=fuente;
        				dataGridView1.Rows[indice].Cells[2].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[indice].Cells[2].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[indice].Cells[3].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[indice].Cells[3].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[indice].Cells[4].Style.BackColor=System.Drawing.Color.MediumSeaGreen;
        				dataGridView1.Rows[indice].Cells[4].Style.ForeColor=System.Drawing.Color.White;
        				dataGridView1.Rows[indice].Cells[5].Value="MARCADO";/*/
        			}
        			
        			contar_marcados();
        		}else{
        			if(panel2.Visible==true){
        				cons.ShowDialog();
        				periodos=cons.pasar_list();
        				
        				tot=(periodos.Length + 1) / 7;
        				//MessageBox.Show(""+tot);
        				periodos=cortar_periodos(periodos,tot);
        				tot=(periodos.Length + 1) / 7;
        				
        				dataGridView1.Rows[indice].Cells[4].Value=periodos;
        				dataGridView1.Rows[indice].Cells[1].Value=tot;
        				
        				if(tot==0){
        					MessageBox.Show("Este Asegurado ya no presenta periodos en mora hasta el periodo del Cd Analizado,\nse quitará de esta lista.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Information);
        					dataGridView2.Rows.RemoveAt(indice);
        					dataGridView2.Refresh();
        				}
        				
        				if(tot==1){
        					
        				}
        				
        			}
        		}
        		label1.Text="Casos: "+dataGridView1.RowCount;
        		label3.Text="Casos: "+dataGridView2.RowCount;
        	}
        }
        
        public void guardar(){
        	int x=0,y=0;
			
			while(tabla_varios.Columns.Count < 6){
				tabla_varios.Columns.Add();
			}
			
			tabla_varios.Rows.Clear();
			
			while(x<dataGridView1.RowCount)
			{	
				
				tabla_varios.Rows.Add(dataGridView1.Rows[x].Cells[0].FormattedValue.ToString(),
				                      dataGridView1.Rows[x].Cells[1].FormattedValue.ToString(),
				                      dataGridView1.Rows[x].Cells[2].FormattedValue.ToString(),
				                      dataGridView1.Rows[x].Cells[3].FormattedValue.ToString(),
				                      dataGridView1.Rows[x].Cells[4].FormattedValue.ToString(),
				                      dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
				
				x++;
			}
			
			while(tabla_uno_solo.Columns.Count < 5){
				tabla_uno_solo.Columns.Add();
			}
			
			tabla_uno_solo.Rows.Clear();
			
			while(y<dataGridView2.RowCount)
			{
				tabla_uno_solo.Rows.Add(dataGridView2.Rows[y].Cells[0].FormattedValue.ToString(),
				                        dataGridView2.Rows[y].Cells[1].FormattedValue.ToString(),
				                        dataGridView2.Rows[y].Cells[2].FormattedValue.ToString(),
				                        dataGridView2.Rows[y].Cells[3].FormattedValue.ToString());
				y++;
			}			
			
			//tabla_excel
			XLWorkbook wb = new XLWorkbook();
			wb.Worksheets.Add(tabla_uno_solo,hoja);
			wb.SaveAs(@"mod40/mod40_un_periodo.xlsx");
			
			//tabla_baja_1
			XLWorkbook wb1 = new XLWorkbook();
			wb1.Worksheets.Add(tabla_varios,hoja);
			wb1.SaveAs(@"mod40/mod40_resultados.xlsx");
			
			/*SaveFileDialog dialogz = new SaveFileDialog();
        	String arch_lz40;
        	
        	dialogz.Filter = "Archivos de Nova Gear Modalidad 40 (*.LZ40)|*.LZ40"; //le indicamos el tipo de filtro en este caso que busque
        	//solo los archivos Access
        	dialogz.Title = "Guardar Archivo de Resultados";//le damos un titulo a la ventana

        	dialogz.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
        	
        	//si al seleccionar el archivo damos Ok
        	if (dialogz.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        	{
        		try{
        			arch_lz40 = dialogz.FileName;*/
			try{
        			ZipFile arch = new ZipFile();
        			arch.AddFile(@"mod40/mod40_un_periodo.xlsx","");
        			arch.AddFile(@"mod40/mod40_resultados.xlsx","");
        			//arch.Save(arch_lz40);
        			arch.Save(@"mod40/"+arch_nombre_corto);
        			MessageBox.Show("El archivo se guardó correctamente.");
        			
        		}catch(Exception es){
        			MessageBox.Show("Ocurrió el siguiente problema al crear el archivo de resultados:\n\n"+es);
        		}
        		
        	//}			
        }
        
        public String cortar_periodos(String periodos, int tot1){
        	int i=0;
        	String perios="";
        	string[] peris = new string[tot1];
        	
        	peris = periodos.Split(',');
        	while(i<tot1){
        
        		if(Convert.ToInt32(peris[i]) <= Convert.ToInt32(hoja.Substring(8))){
        			perios+=peris[i]+",";
        		}
        		i++;
        	}
        	
        	perios=perios.TrimEnd(',');
        	return perios;
        }
        
		void Resultados_analisisLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            marcado_inicial();
			contar_marcados();
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(radioButton1.Checked==true){
				panel1.Visible=true;
				panel2.Visible=false;
				//radioButton2.Checked=false;
			}
			
			if(radioButton2.Checked==true){
				panel1.Visible=false;
				panel2.Visible=true;
				//radioButton1.Checked=false;
			}
			timer1.Enabled=false;
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			timer1.Enabled=true;
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			timer1.Enabled=true;
		}
		//imprimir
		void Button1Click(object sender, EventArgs e)
		{
			int x=0,y=0;

			if(panel1.Visible==true){
				if(permiso_impri==1){
					
					while(tabla_varios.Columns.Count < 5){
						tabla_varios.Columns.Add();
					}
					
					while(tabla_impri.Columns.Count < 5){
						tabla_impri.Columns.Add();
					}
					
					tabla_varios.Rows.Clear();
					tabla_impri.Rows.Clear();
					
					while(x<dataGridView1.RowCount)
					{
						if(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString().Equals("MARCADO")){
							tabla_impri.Rows.Add(dataGridView1.Rows[x].Cells[0].FormattedValue.ToString(),
							                      dataGridView1.Rows[x].Cells[2].FormattedValue.ToString(),
							                      dataGridView1.Rows[x].Cells[3].FormattedValue.ToString(),
							                      dataGridView1.Rows[x].Cells[4].FormattedValue.ToString().Substring(0,13));
						}
						x++;
					}
					
					guardar();
					
					try{
						File.Copy(@"mod40/"+arch_nombre_corto,@"mod40/historial/"+arch_nombre_corto,true);
						File.Delete(@"mod40/"+arch_nombre_corto);
						
						Visor_oficios40 viso = new Visor_oficios40();
						viso.recibir_baja(tabla_impri);
						viso.Show();
						this.Hide();
					}catch(Exception es0){
					}
					
				}else{
					MessageBox.Show("Todavía no se han verificado todos los Asegurados.\nNo se puede generar el oficio de baja hasta que se verifiquen todos los asegurados.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
				
			}else{
				if(panel2.Visible==true){
					
					while(tabla_uno_solo.Columns.Count < 5){
						tabla_uno_solo.Columns.Add();
					}
					
					tabla_uno_solo.Rows.Clear();
					
					while(y<dataGridView2.RowCount)
					{
						tabla_uno_solo.Rows.Add(dataGridView2.Rows[y].Cells[0].FormattedValue.ToString(),
						                        dataGridView2.Rows[y].Cells[1].FormattedValue.ToString(),
						                        dataGridView2.Rows[y].Cells[2].FormattedValue.ToString(),
						                        dataGridView2.Rows[y].Cells[3].FormattedValue.ToString());
						y++;
					}
					
					Visor_oficios40 viso1 = new Visor_oficios40();
					viso1.recibir_baja_uno_solo(tabla_uno_solo);
					viso1.Show();
				}
			}
			
			
		}
		//conciliar
		void Button2Click(object sender, EventArgs e)
		{
			
			if(panel1.Visible==true){
				indice=dataGridView1.CurrentRow.Index;
				nss=dataGridView1.Rows[indice].Cells[2].FormattedValue.ToString();
				//MessageBox.Show(nss);
			}else{
				if(panel2.Visible==true){
					indice=dataGridView2.CurrentRow.Index;
					nss=dataGridView2.Rows[indice].Cells[1].FormattedValue.ToString();
					//MessageBox.Show(nss);
				}
			}
			
			conciliar();
		}
		//guardar
		void Button3Click(object sender, EventArgs e)
		{
			guardar();
		}

		void DataGridView1MouseDown(object sender, MouseEventArgs e)
		{
			try{
				if(panel1.Visible==true){
					if(dataGridView1.RowCount>0){
						if(e.Button == MouseButtons.Right){
							if ((0 <= (dataGridView1.HitTest(e.X, e.Y).RowIndex)) && ((dataGridView1.HitTest(e.X, e.Y).RowIndex) <= (dataGridView1.RowCount-1)))
							{
								dataGridView1.ClearSelection();
								indice=dataGridView1.HitTest(e.X,e.Y).RowIndex;
								dataGridView1.Rows[indice].Selected=true;
								dataGridView1.CurrentCell=dataGridView1[dataGridView1.HitTest(e.X, e.Y).ColumnIndex,dataGridView1.HitTest(e.X, e.Y).RowIndex];
								nss=dataGridView1.Rows[indice].Cells[2].FormattedValue.ToString();
								//MessageBox.Show(nss);
							}
						}
					}
				}
			}catch(Exception es){
				
			}
			
			/*Consulta_Mod40 cons = new Consulta_Mod40();
			if(cons.buscar_valor(nss,1)==true){
				cons.ShowDialog();
			}*/
		}
		
		void DataGridView2MouseDown(object sender, MouseEventArgs e)
		{
			try{
			if(panel2.Visible==true){
				if(dataGridView2.RowCount>0){
					if(e.Button == MouseButtons.Right){
						if ((0 <= (dataGridView2.HitTest(e.X, e.Y).RowIndex)) && ((dataGridView2.HitTest(e.X, e.Y).RowIndex) <= (dataGridView2.RowCount-1)))
						{
							dataGridView2.ClearSelection();
							indice=dataGridView2.HitTest(e.X,e.Y).RowIndex;
							dataGridView2.Rows[indice].Selected=true;
							dataGridView2.CurrentCell=dataGridView2[dataGridView2.HitTest(e.X, e.Y).ColumnIndex,dataGridView2.HitTest(e.X, e.Y).RowIndex];
							nss=dataGridView2.Rows[indice].Cells[1].FormattedValue.ToString();
							//MessageBox.Show(nss);
						}
					}
				}
			}
			}catch(Exception es){
				
			}
			
		}
		
		void ConciliarToolStripMenuItemClick(object sender, EventArgs e)
		{
			try{
				conciliar();
			}catch(Exception asd){
				
			}
		}
		//marcar
		void BorrarToolStripMenuItemClick(object sender, EventArgs e)
		{
			try{
				if(panel1.Visible==true){
					//MessageBox.Show(""+indice+"|"+dataGridView1.Rows[indice].Cells[1].FormattedValue.ToString());
					marcar_click();
				}else{
					if(panel2.Visible==true){
						//MessageBox.Show(""+indice+"|"+dataGridView2.Rows[indice].Cells[1].FormattedValue.ToString());
					}
				}
			}catch(Exception asd1){
				
			}
		}
		
		void DesmarcarToolStripMenuItemClick(object sender, EventArgs e)
		{
			try{
				if(panel1.Visible==true){
					//MessageBox.Show(""+indice+"|"+dataGridView1.Rows[indice].Cells[1].FormattedValue.ToString());
					desmarcar_click();
				}else{
					if(panel2.Visible==true){
						//MessageBox.Show(""+indice+"|"+dataGridView2.Rows[indice].Cells[1].FormattedValue.ToString());
					}
				}
			}catch(Exception asd1){
				
			}
		}
		
		void Panel2Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void Timer2Tick(object sender, EventArgs e)
		{
			contar_marcados();
			timer2.Enabled=false;
		}
		
		void Resultados_analisisFormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult respuesta = MessageBox.Show("Está a punto de salir.\n¿Desea Continuar?" ,"ATENCIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
			
			if(respuesta ==DialogResult.Yes){
				guardar();
				e.Cancel=false;
			}else{
				e.Cancel=true;
			}
		}
	}
}

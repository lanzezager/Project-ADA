/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 12/01/2016
 * Hora: 02:14 p. m.
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

namespace Nova_Gear.Depuracion
{
	/// <summary>
	/// Description of Depu_manu.
	/// </summary>
	public partial class Depu_manu : Form
	{
		public Depu_manu(String cad,int modo)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.per_buscar = cad;
			this.mode=modo;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String per_buscar,sql,reg_pat,pat_buscar,cred_bus,id_cred,fecha,tipo_cred_consul;
		int cont=0,marcados=0,cont2=0,cont3=0,cont4=0,indice=0,mode=0,i=0,j=0,l=0,consul_cred=0,salir=0,tipo_per=0;
		bool marca,v_inicial;
		int[] results_b;
		//string[] lista_dep;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable lista_dep = new DataTable();
		DataTable hoja_rep = new DataTable();
		
		DataTable lista_marcar = new DataTable();
		DataTable dt = new DataTable();
		
		//Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;

		//periodos
		public void llenar_Cb1(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			int i=0;
			dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			do{
				comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView2.RowCount);
			i=0;
            conex.cerrar();
		}
		
		public void estilo_grid() {
        
			dataGridView1.Columns[1].ReadOnly=true;
			dataGridView1.Columns[2].ReadOnly=true;
			dataGridView1.Columns[3].ReadOnly=true;
			dataGridView1.Columns[4].ReadOnly=true;
			dataGridView1.Columns[5].ReadOnly=true;
			dataGridView1.Columns[6].ReadOnly=true;
			dataGridView1.Columns[7].ReadOnly=true;
			
			dataGridView1.Columns[1].HeaderText= "REGISTRO PATRONAL";
			dataGridView1.Columns[2].HeaderText= "RAZÓN SOCIAL";
			dataGridView1.Columns[3].HeaderText= "CRÉDITO CUOTA";
			dataGridView1.Columns[4].HeaderText= "CRÉDITO MULTA";
			dataGridView1.Columns[5].HeaderText= "IMPORTE CUOTA";
			dataGridView1.Columns[6].HeaderText= "IMPORTE MULTA";
			if(mode==2){
				dataGridView1.Columns[7].HeaderText= "FECHA RECEPCIÓN";
				dataGridView1.Columns[9].HeaderText= "TIPO DOCUMENTO";
				dataGridView1.Columns[10].HeaderText= "PERIODO";
			}else{
				dataGridView1.Columns[7].HeaderText= "STATUS";
				try{
					dataGridView1.Columns[8].HeaderText= "ID";
				}catch(Exception s){}
			}
			dataGridView1.Columns[1].Width=130;
			dataGridView1.Columns[2].Width=340;
			dataGridView1.Columns[3].Width=130;
			dataGridView1.Columns[4].Width=130;
			dataGridView1.Columns[5].Width=130;
			dataGridView1.Columns[6].Width=130;
			dataGridView1.Columns[7].Width=200;
			
			dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
			
			cont=0;
			while(cont<consultamysql.Rows.Count){
				dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
				dataGridView1.Rows[cont].Cells[0].Value=false;
				cont++;
			}
        }
		
		public void activar_depu(){
			if(marcados>0){
				button1.Enabled=true;
			}else{
				button1.Enabled=false;
			}
		}
		
		public void checar_marcados(){
			
				cont3=0;
				marcados=0;
				if(dataGridView1.RowCount>0){
                    while(cont3 < dataGridView1.RowCount){
						/*if(Convert.ToBoolean(dataGridView1.Rows[cont3].Cells[0].Value) == true){
							marcados++;
						}
						
						if((Convert.ToBoolean(dataGridView1.Rows[cont3].Cells[0].Value) == false)||(dataGridView1.Rows[cont3].Cells[0].Value ==null)){
							if(marcados>0){
									marcados--;
									//if(Convert.ToBoolean(dataGridView1.Rows[cont3].Cells[0].Value) == false){
										
									//}
							}
						}*/
						
						if(Convert.ToBoolean(dataGridView1.Rows[cont3].Cells[0].Value) == true){
							marcados++;
						}
					
						cont3++;
					}
					
					label3.Text="Registros Marcados: "+marcados;
					label3.Refresh();
					activar_depu();
				}
		}
		
		public void checar_marcados_listbox(){
			cont3=0;
			if(dataGridView1.RowCount>0){
				listBox1.Items.Clear();
					
			    do{
					if(Convert.ToBoolean(dataGridView1.Rows[cont3].Cells[0].Value) == true){
						listBox1.Items.Add(dataGridView1.Rows[cont3].Cells[1].Value.ToString());
					}
						cont3++;
				}while(cont3<dataGridView1.RowCount);
			}
		}
		//lista depuracion
		public void mandar_lista_dep(){
			String cred_cuo,cred_mul,id;
			//lista_dep = new string[marcados];
			cont=0;
			lista_dep.Rows.Clear();
			while(cont<dataGridView1.RowCount){
				if(Convert.ToBoolean(dataGridView1.Rows[cont].Cells[0].Value)==true){
					reg_pat = dataGridView1.Rows[cont].Cells[1].FormattedValue.ToString();
					
					reg_pat = reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
					cred_cuo=dataGridView1.Rows[cont].Cells[3].FormattedValue.ToString();
					cred_mul=dataGridView1.Rows[cont].Cells[4].FormattedValue.ToString();
					id=dataGridView1.Rows[cont].Cells[8].FormattedValue.ToString();
					
					lista_dep.Rows.Add(reg_pat,cred_cuo,cred_mul,id);
				}
				cont++;
			}

            //System.IO.File.Delete(@"temporal_rale.xlsx");
            //System.IO.File.Delete(@"temporal_datos_depu.xlsx");
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(lista_dep, "hoja_lz");
            //wb.SaveAs(@"temporal_rale.xlsx");
            wb.SaveAs(@"temporal_datos_depu.xlsx");
            salir = 1;
		}
		//lista ESTRADOS
		public void mandar_lista_dep_nn(){
			//lista_dep = new string[marcados];
			cont=0;
			lista_dep.Rows.Clear();
			do{
				if(Convert.ToBoolean(dataGridView1.Rows[cont].Cells[0].Value)==true){
					reg_pat = dataGridView1.Rows[cont].Cells[8].FormattedValue.ToString();
					
					//reg_pat = reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
					//reg_pat = reg_pat.Trim('-');
					//reg_pat= reg_pat.Substring(0,reg_pat.Length-1);
					lista_dep.Rows.Add(reg_pat);
					hoja_rep.Rows.Add(dataGridView1.Rows[cont].Cells[1].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[2].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[3].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[4].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[5].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[6].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[9].FormattedValue.ToString(),
					dataGridView1.Rows[cont].Cells[10].FormattedValue.ToString());
				}
			cont++;
			}while(cont<dataGridView1.RowCount);
			//MessageBox.Show(""+lista_dep.Rows.Count);
			
			DialogResult resultado = MessageBox.Show("Esta por marcar como ESTRADOS un total de: "+lista_dep.Rows.Count+" Registros\nEsto afectará la base de datos.\n\n¿Está seguro que desea continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			if(resultado== DialogResult.Yes){
				dataGridView2.DataSource=lista_dep;
				
				sql="";
                conex2.conectar("base_principal");
				i=0;
				fecha=System.DateTime.Today.ToShortDateString();
				fecha= fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
				
				do{
					id_cred = dataGridView2.Rows[i].Cells[0].Value.ToString();
					sql="UPDATE datos_factura SET nn=\"ESTRADOS\",fecha_estrados=\""+fecha+"\",estado_cartera=\"-\" WHERE id="+id_cred+";";
					conex2.consultar(sql);
					conex.guardar_evento("Se marcó como ESTRADOS el Crédito con el ID: "+id_cred);
					i++;
				}while(i<dataGridView2.RowCount);
				
				//generar reporte
				Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
				entrega_cartera.envio3(hoja_rep,"RELACIÓN DE ENTREGA DE CRÉDITOS NO NOTIFICADOS A ESTRADOS"," ","ESTRADOS");
				entrega_cartera.Show();
				
				MessageBox.Show("Se actualizaron "+i+" Registros","¡Listo!");
				/*if(consul_cred==1){
					conex.guardar_evento("Se marcó como ESTRADOS el Crédito: "+per_buscar);
					
				}else{
					conex.guardar_evento("Se marcaron como ESTRADOS "+i+" Registros del periodo "+comboBox1.SelectedItem.ToString());
				}
				consul_cred=0;*/

                listBox1.Items.Clear();
                maskedTextBox1.Text = "";
				//this.Close();
			}
		}
		//lista FACTURAR
		public void facturar(){
			String cred_cuo,cred_mul;
			cont=0;
			lista_dep.Rows.Clear();
			conex2.conectar("base_principal");
			
			do{
				if(Convert.ToBoolean(dataGridView1.Rows[cont].Cells[0].Value)==true){
					reg_pat = dataGridView1.Rows[cont].Cells[1].FormattedValue.ToString();
					reg_pat = reg_pat.Substring(0,3)+reg_pat.Substring(4,5)+reg_pat.Substring(10,2);
					cred_cuo=dataGridView1.Rows[cont].Cells[3].FormattedValue.ToString();
					cred_mul=dataGridView1.Rows[cont].Cells[4].FormattedValue.ToString();
					if(tipo_per==0){
						conex2.consultar("UPDATE datos_factura SET status=\"POR_ACTUALIZAR\" WHERE registro_patronal2=\""+reg_pat+"\" AND credito_cuotas=\""+cred_cuo+"\" AND credito_multa=\""+cred_mul+"\" AND nombre_periodo=\""+per_buscar+"\"");
					}else{
						conex2.consultar("UPDATE datos_factura SET status=\"POR_ACTUALIZAR\" WHERE registro_patronal2=\""+reg_pat+"\" AND credito_cuotas=\""+cred_cuo+"\" AND credito_multa=\""+cred_mul+"\" AND periodo_factura=\""+per_buscar+"\"");
					}
				}else{
				}
				cont++;
			}while(cont<dataGridView1.RowCount);			
			this.Close();
		}
		
		public void buscar_credito(){
		
			cred_bus=maskedTextBox2.Text;
			if(cred_bus.Length>0){
				i=0;
				j=0;
				
				do{//buscar en credito_cuota
					if(cred_bus.Equals(dataGridView1.Rows[i].Cells[3].Value.ToString())){
						j++;
					}
					i++;
				}while(i<dataGridView1.RowCount);
				
				if(j==0){
					i=0;
					do{//buscar en credito_multa
						if(cred_bus.Equals(dataGridView1.Rows[i].Cells[4].Value.ToString())){
							j++;
						}
						i++;
					}while(i<dataGridView1.RowCount);
				}
				results_b = new int[j];
				i=0;
				j=0;
				
				do{//buscar en credito_cuota
					if(cred_bus.Equals(dataGridView1.Rows[i].Cells[3].Value.ToString())){
						results_b[j]=i;
						j++;
					}
					i++;
				}while(i<dataGridView1.RowCount);
				
				if(j==0){
					i=0;
					do{//buscar en credito_multa
						if(cred_bus.Equals(dataGridView1.Rows[i].Cells[4].Value.ToString())){
							results_b[j]=i;
							j++;
						}
						i++;
					}while(i<dataGridView1.RowCount);
				}
				
				l=0;
				if(j>=1){
					dataGridView1.Rows[results_b[l]].Cells[3].Selected=true;
					if(results_b[l]<5){
						dataGridView1.FirstDisplayedScrollingRowIndex=0;
					}else{
			   			dataGridView1.FirstDisplayedScrollingRowIndex=(results_b[l])-5;
					}
					dataGridView1.Focus();
			   		if(j>1){
			   		button6.Enabled=true;
			   		}
			   		
					label8.Text=""+(l+1)+" de "+j;
				}else{
					label8.Text="0 de 0";	
				}
				
				
			}
			
			
		}
		
		public void buscar_periodo_estrados(){
			if(comboBox1.SelectedIndex >-1){
				if(dataGridView1.RowCount>0){
					DialogResult resul = MessageBox.Show("Si actualiza el periodo cargado se borraran los Elementos marcados\n\n¿Desea Continuar?", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					
					if(resul==DialogResult.Yes){
						per_buscar = comboBox1.SelectedItem.ToString();
						listBox1.Items.Clear();
						conex.conectar("base_principal");
						sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,periodo,estado_cartera FROM datos_factura WHERE"+
							" nombre_periodo = \""+per_buscar+"\" AND nn = \"NN\" AND status=\"EN TRAMITE\" ORDER BY registro_patronal, credito_cuotas";
						consultamysql=conex.consultar(sql);
						dataGridView1.DataSource=consultamysql;
						label2.Text="Registros Totales: "+consultamysql.Rows.Count;
						dataGridView1.Columns[8].Visible=false;
						dataGridView1.Columns[11].Visible=false;
						estilo_grid();
						timer1.Enabled=true;
						//lista_dep.Columns.Add("NRP");
						button2.Enabled=true;
						button3.Enabled=true;
						consul_cred = 0;
						marcado_inicial();
					}
				}else{
					per_buscar = comboBox1.SelectedItem.ToString();
					listBox1.Items.Clear();
					conex.conectar("base_principal");
					sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,periodo,estado_cartera FROM datos_factura WHERE"+
						" nombre_periodo = \""+per_buscar+"\" AND nn = \"NN\" AND status=\"EN TRAMITE\"ORDER BY registro_patronal, credito_cuotas";
					consultamysql=conex.consultar(sql);
					dataGridView1.DataSource=consultamysql;
					label2.Text="Registros Totales: "+consultamysql.Rows.Count;
					dataGridView1.Columns[8].Visible=false;
					dataGridView1.Columns[11].Visible=false;
					estilo_grid();
					timer1.Enabled=true;
					if((lista_dep.Columns.Contains("NRP"))==false){
						lista_dep.Columns.Add("NRP");
					}
					button2.Enabled=true;
					button3.Enabled=true;
					button6.Enabled=true;
					button7.Enabled=true;
					button8.Enabled=true;
					maskedTextBox2.Enabled=true;
					marcado_inicial();
				}
			}
		}
		
		public void buscar_todo_estrados(){
			
				if(dataGridView1.RowCount>0){
					DialogResult resul = MessageBox.Show("Si actualiza el periodo cargado se borraran los Elementos marcados\n\n¿Desea Continuar?", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					
					if(resul==DialogResult.Yes){
						//per_buscar = comboBox1.SelectedItem.ToString();
						listBox1.Items.Clear();
						conex.conectar("base_principal");
						sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,periodo,estado_cartera FROM datos_factura WHERE"+
							" nn = \"NN\" AND status=\"EN TRAMITE\" ORDER BY registro_patronal, credito_cuotas";
						consultamysql=conex.consultar(sql);
						dataGridView1.DataSource=consultamysql;
						label2.Text="Registros Totales: "+consultamysql.Rows.Count;
						dataGridView1.Columns[8].Visible=false;
						dataGridView1.Columns[11].Visible=false;
						estilo_grid();
						timer1.Enabled=true;
						//lista_dep.Columns.Add("NRP");
						button2.Enabled=true;
						button3.Enabled=true;
						consul_cred = 0;
						marcado_inicial();
					}
				}else{
					//per_buscar = comboBox1.SelectedItem.ToString();
					listBox1.Items.Clear();
					conex.conectar("base_principal");
					sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,periodo,estado_cartera FROM datos_factura WHERE"+
						" nn = \"NN\" AND status=\"EN TRAMITE\"ORDER BY registro_patronal, credito_cuotas";
					consultamysql=conex.consultar(sql);
					dataGridView1.DataSource=consultamysql;
					label2.Text="Registros Totales: "+consultamysql.Rows.Count;
					dataGridView1.Columns[8].Visible=false;
					dataGridView1.Columns[11].Visible=false;
					estilo_grid();
					timer1.Enabled=true;
					if((lista_dep.Columns.Contains("NRP"))==false){
						lista_dep.Columns.Add("NRP");
					}
					
					button2.Enabled=true;
					button3.Enabled=true;
					button6.Enabled=true;
					button7.Enabled=true;
					button8.Enabled=true;
					maskedTextBox2.Enabled=true;
					marcado_inicial();
				}
			
		}
		
		public void buscar_credito_estrados(){
			
				//consul_cred=0;
			if(dataGridView1.RowCount>0){
				DialogResult resul = MessageBox.Show("Si actualiza se borraran los Elementos marcados\n\n¿Desea Continuar?", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				
				if(resul==DialogResult.Yes){
				per_buscar = maskedTextBox1.Text;
				listBox1.Items.Clear();
				conex.conectar("base_principal");
				
				if(radioButton1.Checked==true){
					tipo_cred_consul = " credito_cuotas = \""+per_buscar+"\" ";	
				}
				
				if(radioButton2.Checked==true){
					tipo_cred_consul = " credito_multa = \""+per_buscar+"\" ";	
				}
				
				sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,periodo,estado_cartera FROM datos_factura WHERE "+
					tipo_cred_consul+" AND nn = \"NN\" AND status=\"EN TRAMITE\" ORDER BY registro_patronal, credito_cuotas";
				
				consultamysql=conex.consultar(sql);
				dataGridView1.DataSource=consultamysql;
				label2.Text="Registros Totales: "+consultamysql.Rows.Count;
				dataGridView1.Columns[8].Visible=false;
				dataGridView1.Columns[11].Visible=false;
				estilo_grid();
				timer1.Enabled=true;
				//lista_dep.Columns.Add("NRP");
				button2.Enabled=true;
				button3.Enabled=true;
				consul_cred=1;
				marcado_inicial();
				//MessageBox.Show(""+consul_cred);
				}
			}else{
				per_buscar = maskedTextBox1.Text;
				listBox1.Items.Clear();
				
				if(radioButton1.Checked==true){
					tipo_cred_consul = " credito_cuotas = \""+per_buscar+"\" ";	
				}
				
				if(radioButton2.Checked==true){
					tipo_cred_consul = " credito_multa = \""+per_buscar+"\" ";	
				}
				
				
				conex.conectar("base_principal");
				sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,periodo,estado_cartera FROM datos_factura WHERE "+
					tipo_cred_consul+" AND nn = \"NN\" AND status=\"EN TRAMITE\" ORDER BY registro_patronal, credito_cuotas";
				//MessageBox.Show(sql);
				consultamysql=conex.consultar(sql);
				dataGridView1.DataSource=consultamysql;
				label2.Text="Registros Totales: "+consultamysql.Rows.Count;
				dataGridView1.Columns[8].Visible=false;
				dataGridView1.Columns[11].Visible=false;
				estilo_grid();
				timer1.Enabled=true;
				if(lista_dep.Columns.Contains("NRP")==false){
					lista_dep.Columns.Add("NRP");
				}
				button2.Enabled=true;
				button3.Enabled=true;
				button6.Enabled=true;
				button7.Enabled=true;	
				button8.Enabled=true;
				maskedTextBox2.Enabled=true;
				consul_cred=1;
				marcado_inicial();
			}
			
		}
		
		public void carga_chema_excel(){
			int i=0;
			String tabla="";
			comboBox2.Items.Clear();
			dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			
			do{
				if (dt.Rows[i][3].ToString().Length>0){
					if ((dt.Rows[i][3].ToString()).Equals("TABLE")==true){
						tabla=dt.Rows[i][2].ToString();
						if((tabla.Substring((tabla.Length-1),1)).Equals("$")){
							tabla = tabla.Remove((tabla.Length-1),1);
							comboBox2.Items.Add(tabla);
						}
					}
				}
				i++;
			}while(i<dt.Rows.Count);
		
			//MessageBox.Show(""+i+"|"+dt.Rows.Count);
			dt.Clear();	
		}
		
		public void seleccionar_fila(int indi){
		
			if(mode==2){
				//MessageBox.Show(dataGridView1.Rows[indi].Cells[8].FormattedValue.ToString()+"|valor:"+dataGridView1.Rows[indi].Cells[0].Value);
				if(Convert.ToBoolean(dataGridView1.Rows[indi].Cells[0].Value) == true){
					conex3.consultar("UPDATE datos_factura SET estado_cartera=\"ESTRADOS\" WHERE id=" + dataGridView1.Rows[indi].Cells[8].FormattedValue.ToString()+"");
				}else{
					conex3.consultar("UPDATE datos_factura SET estado_cartera=\"-\" WHERE id=" + dataGridView1.Rows[indi].Cells[8].FormattedValue.ToString()+"");
				}
			}
		}
		
		public void marcado_inicial(){
			int x=0,y=0;
			//MessageBox.Show("comenzar a marcar");
			while(x<dataGridView1.RowCount){
				if(dataGridView1.Rows[x].Cells[11].FormattedValue.ToString().Equals("ESTRADOS")){
					dataGridView1.Rows[x].Cells[0].Value=true;
					y=1;
					while(y<dataGridView1.Rows[x].Cells.Count){
						dataGridView1.Rows[x].Cells[y].Style.BackColor = System.Drawing.Color.LightSkyBlue;
						y++;
					}
				}
				x++;
			}
		}
		
		void Depu_manuLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			//mode=5;
			if (mode==1){//Depu Manu Opcion Todo
			panel1.Visible=false;
            System.IO.File.Delete(@"temporal_rale.xlsx");
		
			conex.conectar("base_principal");
			sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,status,id FROM datos_factura WHERE"+
				" nombre_periodo = \""+per_buscar+"\" AND (status = \"CARTERA\" OR status = \"EN TRAMITE\" OR status = \"NOTIFICADO\" OR status = \"0\" OR status like \"DEPURA%\") ORDER BY registro_patronal,credito_cuotas";
			consultamysql=conex.consultar(sql);
			dataGridView1.DataSource=consultamysql;
			label2.Text="Registros Totales: "+consultamysql.Rows.Count;
			estilo_grid();
			timer1.Enabled=true;
			lista_dep.Columns.Add("NRP");
			lista_dep.Columns.Add("CREDITO CUOTA");
            lista_dep.Columns.Add("CREDITO MULTA");
            lista_dep.Columns.Add("ID");
			button6.Enabled=true;
			button7.Enabled=true;
			button8.Enabled=true;
			maskedTextBox2.Enabled=true;
			}

            if (mode == 3)//Depu Manu Opcion PAGADOS
            {
                panel1.Visible = false;
                conex.conectar("base_principal");
                sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,status,id FROM datos_factura WHERE " +
                    //" nombre_periodo = \"" + per_buscar + "\" AND observaciones =\"PAGADO\" ORDER BY registro_patronal,credito_cuotas";
                	"observaciones =\"PAGADO\" AND status NOT LIKE \"DEPU%\" ORDER BY registro_patronal,credito_cuotas";
                consultamysql = conex.consultar(sql);
                dataGridView1.DataSource = consultamysql;
                label2.Text = "Registros Totales: " + consultamysql.Rows.Count;
                estilo_grid();
                timer1.Enabled = true;
                lista_dep.Columns.Add("NRP");
                lista_dep.Columns.Add("CREDITO CUOTA");
            	lista_dep.Columns.Add("CREDITO MULTA");
            	lista_dep.Columns.Add("ID");
                button6.Enabled=true;
                button7.Enabled=true;
                button8.Enabled=true;
                maskedTextBox2.Enabled=true;
            }

			if(mode==2){//ESTRADOS
				this.Text="Nova Gear: Estrados ";
				panel1.Visible=true;
				button2.Enabled=false;
				button3.Enabled=false;
				label4.Text="Marcar como Estrados";
				button1.Text="Guardar\ne Imprimir";
				button1.Image = global::Nova_Gear.Properties.Resources.save_close;
				dataGridView1.Columns[0].HeaderText="ESTRADOS";
				llenar_Cb1();
				comboBox1.AutoCompleteMode=AutoCompleteMode.SuggestAppend;
				comboBox1.AutoCompleteSource=AutoCompleteSource.ListItems;
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				hoja_rep.Columns.Add();
				conex3.conectar("base_principal");
				
			}
            
            if(mode==4){//RB SISCOB
            	panel1.Visible=false;
            	System.IO.File.Delete(@"temporal_rale.xlsx");
            	
            	conex.conectar("base_principal");
            	sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,status,id FROM datos_factura WHERE " +
            		"observaciones =\"PAGADO\" and status not like \"DEPU%\" ORDER BY registro_patronal,credito_cuotas";
            	consultamysql=conex.consultar(sql);
            	dataGridView1.DataSource=consultamysql;
            	label2.Text="Registros Totales: "+consultamysql.Rows.Count;
            	estilo_grid();
            	timer1.Enabled=true;
            	lista_dep.Columns.Add("NRP");
            	lista_dep.Columns.Add("CREDITO CUOTA");
            	lista_dep.Columns.Add("CREDITO MULTA");
            	lista_dep.Columns.Add("ID");
            	button6.Enabled=true;
            	button7.Enabled=true;
            	button8.Enabled=true;
            	maskedTextBox2.Enabled=true;
            }
            
            if(mode==5){//factura selectiva
            	panel1.Visible=false;
            	button9.Visible=true;
            	this.Text="Nova Gear: Generar Factura del Periodo: "+per_buscar;
            	label1.Text="Seleccione con un Click los Créditos que se van a Entregar al Notificador";
            	label1.Refresh();
            	label4.Text="Factura Selectiva para Entrega a Notificador "+per_buscar;
            	label4.Refresh();
            	button1.Text="Facturar";
            	System.IO.File.Delete(@"temporal_rale.xlsx");
            	
            	conex.conectar("base_principal");
            	sql="SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per_buscar+"\"";
            	consultamysql=conex.consultar(sql);
            	
            	if(Convert.ToInt32(consultamysql.Rows[0][0].ToString())>0){
            		sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,status FROM datos_factura WHERE " +
            		"fecha_entrega IS NULL AND nombre_periodo=\""+per_buscar+"\" AND status=\"0\" ORDER BY registro_patronal,credito_cuotas";
            		tipo_per=0;
            	}else{
            		sql="SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,status FROM datos_factura WHERE " +
            		"fecha_entrega IS NULL AND periodo_factura=\""+per_buscar+"\" AND status=\"0\" ORDER BY registro_patronal,credito_cuotas";
            		tipo_per=1;
            	}
            	
            	
            	consultamysql=conex.consultar(sql);
            	dataGridView1.DataSource=consultamysql;
            	label2.Text="Registros Totales: "+consultamysql.Rows.Count;
            	estilo_grid();
            	timer1.Enabled=true;
            	lista_dep.Columns.Add("NRP");
            	lista_dep.Columns.Add("CREDITO CUOTA");
            	lista_dep.Columns.Add("CREDITO MULTA");
            	
            	button6.Enabled=true;
				button7.Enabled=true;
				button8.Enabled=true;
				maskedTextBox2.Enabled=true;
            	
            }
			
		}
		
		void DataGridView1CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(dataGridView1.CurrentCell.ColumnIndex!=0){
					cont=1;
					//if(dataGridView1.CurrentRow.Cells[0].Value != null){
						marca=Convert.ToBoolean(dataGridView1.CurrentRow.Cells[0].Value);
					//}else{
					//	marca=false;
					//}
					
					if(marca == false){
						do{
							dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
							cont++;
						}while(cont<=7);
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = true;
						
	
					}else{
						cont=1;
						do{
							dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;
							cont++;
						}while(cont<=7);
						dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = false;
						
					}
			}
			checar_marcados();
			checar_marcados_listbox();
		
		}
		
		void DataGridView1CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			/*
			if(dataGridView1.CurrentCell.ColumnIndex==0){
				cont=1;
				if(dataGridView1.CurrentRow.Cells[0].Value != null){
					marca=Convert.ToBoolean(dataGridView1.CurrentRow.Cells[0].Value);
				}else{
					marca=false;
				}
				
				if(marca == false){
					do{
					dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
					cont++;
					}while(cont<=7);
					dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = true;
					
				}else{
					cont=1;
					do{
					dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;
					cont++;
					}while(cont<=7);
					dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = false;
					
				}
				checar_marcados();
			}
			activar_depu();
			*/
			//checar_marcados();
			checar_marcados_listbox();
		}
		
		void DataGridView1CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if(dataGridView1.CurrentCell.ColumnIndex==0){
				cont=1;
				if(dataGridView1.CurrentRow.Cells[0].Value != null){
					marca=Convert.ToBoolean(dataGridView1.CurrentRow.Cells[0].Value);
				}else{
					marca=false;
				}
				
				if(marca == true){
					do{
					dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
					cont++;
					}while(cont<=7);
					//dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = true;
				}else{
					cont=1;
					do{
					dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;
					cont++;
					}while(cont<=7);
					//dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = false;
				}
				
				
			}
			
		}
		
		void Button2Click(object sender, EventArgs e)//marcar todos
		{
			DialogResult res=MessageBox.Show("Se marcarán todos las lineas\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
			if(res== DialogResult.Yes){
				cont=0;
				cont2=1;
				if(dataGridView1.RowCount>0){
				do{
					dataGridView1.Rows[cont].Cells[0].Value=true;
					do{
						dataGridView1.Rows[cont].Cells[cont2].Style.BackColor = System.Drawing.Color.LightSkyBlue;
						cont2++;
					}while(cont2<=7);
					cont++;
					cont2=1;
				}while(cont<dataGridView1.RowCount);
				checar_marcados();
				}
			}
		}
		
		void Button3Click(object sender, EventArgs e)//desmarcar todos
		{
			DialogResult res=MessageBox.Show("Se desmarcarán todos las lineas marcadas\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
			if(res== DialogResult.Yes){
				cont=0;
				cont2=1;
				if(dataGridView1.RowCount>0){
				do{
					if(Convert.ToBoolean(dataGridView1.Rows[cont].Cells[0].Value) == true){
						conex3.consultar("UPDATE datos_factura SET estado_cartera=\"-\" WHERE id=" + dataGridView1.Rows[cont].Cells[8].FormattedValue.ToString()+"");	
					}
					dataGridView1.Rows[cont].Cells[0].Value=false;
					
					do{
						dataGridView1.Rows[cont].Cells[cont2].Style.BackColor = System.Drawing.Color.White;
						cont2++;
					}while(cont2<=7);
					cont++;
					cont2=1;
				}while(cont<dataGridView1.RowCount);
				checar_marcados();
				}
			}
		}
		
		void DataGridView1CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			
		}

		void Timer1Tick(object sender, EventArgs e)
		{
			checar_marcados();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(mode==1||mode==3||mode==4){
				checar_marcados();
				DialogResult resu = MessageBox.Show("Se Depurarán un total de: "+marcados+" Elementos\n\n¿Desea Continuar?", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
				if(resu==DialogResult.Yes){
					dataGridView1.Enabled=false;
					this.Visible=false;
                    salir = 1;
				}
			}
			
			if(mode==2){
				mandar_lista_dep_nn();
			}
			
			if(mode==5){
				DialogResult resu = MessageBox.Show("Se Facturarán un total de "+marcados+" Elementos\n\n¿Desea Continuar?", "AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
				if(resu==DialogResult.Yes){
					facturar();
				}
			}
		}
		
		void DataGridView1CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (dataGridView1.IsCurrentCellDirty) 
			{
				dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
				//checar_marcados();
			}
		}
		
		void DataGridView1MouseMove(object sender, MouseEventArgs e)
		{
			checar_marcados();
		}
		
		void Timer2Tick(object sender, EventArgs e)
		{
			
		}
		
		void DataGridView1CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			
		}
		
		void DataGridView1Click(object sender, EventArgs e)
		{
			
		}
		
		void DataGridView1CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			checar_marcados_listbox();
		}
		
		void ListBox1MouseDown(object sender, MouseEventArgs e)
		{
			if(listBox1.Items.Count>0){
				if(e.Button == MouseButtons.Right){
					if((0 <= (listBox1.IndexFromPoint(e.X,e.Y)))&&(listBox1.IndexFromPoint(e.X,e.Y)<= (listBox1.Items.Count-1))){
						pat_buscar=listBox1.Items[listBox1.IndexFromPoint(e.X,e.Y)].ToString();
						indice=listBox1.IndexFromPoint(e.X,e.Y);
						listBox1.SelectedIndex= listBox1.IndexFromPoint(e.X,e.Y);
						//MessageBox.Show(pat_buscar);
					}
				}
			}
		}
		
		void QuitarToolStripMenuItemClick(object sender, EventArgs e)
		{
			try{
			cont4=0;
			cont=1;
			do{
				if(pat_buscar.Equals(dataGridView1.Rows[cont4].Cells[1].Value.ToString())){
					do{
						dataGridView1.Rows[cont4].Cells[cont].Style.BackColor = System.Drawing.Color.White;
						cont++;
					}while(cont<=7);
					dataGridView1.Rows[cont4].Cells[0].Value = false;
				}
				cont4++;
			}while(cont4<dataGridView1.RowCount);
			listBox1.Items.RemoveAt(indice);
			}catch(Exception x){
				
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			//button4.Enabled=true;
		}
		
		void Depu_manuResizeEnd(object sender, EventArgs e)
		{
			
		}
		
		void Depu_manuSizeChanged(object sender, EventArgs e)
		{
			if(this.WindowState == FormWindowState.Maximized){
				//button5.Visible=true;
				if(mode==2){
					groupBox1.Visible=true;
					 }
					panel2.Visible=true;
				
			}else{
				button5.Visible=false;
				panel2.Visible=false;
				groupBox1.Visible=false;
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(maskedTextBox1.Text.Length==9||comboBox1.SelectedIndex>-1){
				if(comboBox1.SelectedIndex>-1){
					buscar_periodo_estrados();
				}else{
					buscar_credito_estrados();
				}
			}else{
				//MessageBox.Show("Ingrese un crédito o periodo valido para efectuar la búsqueda.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				buscar_todo_estrados();
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			buscar_credito();
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				buscar_credito();
			}
		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		void Button7Click(object sender, EventArgs e)
		{
			if((l+1)> j){
				l--;
					dataGridView1.Rows[results_b[l]].Cells[3].Selected=true;
					if(results_b[l]<5){
						dataGridView1.FirstDisplayedScrollingRowIndex=0;
					}else{
			   			dataGridView1.FirstDisplayedScrollingRowIndex=(results_b[l])-5;
					}
					
					label8.Text=""+(l+1)+" de "+j;
					
					dataGridView1.Focus();
					button6.Enabled=true;
					dataGridView1.Focus();
			   		if(l==j){
					button7.Enabled=true;
				}		
			}
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if((l+1)< j){
				l++;
					dataGridView1.Rows[results_b[l]].Cells[3].Selected=true;
					if(results_b[l]<5){
						dataGridView1.FirstDisplayedScrollingRowIndex=0;
					}else{
			   			dataGridView1.FirstDisplayedScrollingRowIndex=(results_b[l])-5;
					}
					
					label8.Text=""+(l+1)+" de "+j;
					
					dataGridView1.Focus();
					button7.Enabled=true;
					dataGridView1.Focus();
			   		if(l==j){
					button6.Enabled=true;
				}		
			}
		}
		
		void MaskedTextBox2Enter(object sender, EventArgs e)
		{
			if(maskedTextBox2.Text.Length == 9){
				maskedTextBox2.Clear();
			}
		}
		
		void MaskedTextBox2Click(object sender, EventArgs e)
		{
			maskedTextBox2.SelectionStart=0;
		}
		
		void MaskedTextBox1MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
			{
				if(maskedTextBox1.Text.Length==9){
					buscar_credito_estrados();
				}else{
					MessageBox.Show("Ingrese un crédito valido para efectuar la búsqueda.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton1.Checked==true){
				label9.Text="Crédito \nCuota:";
			}
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			if(radioButton2.Checked==true){
				label9.Text="Crédito \nMulta:";
			}
		}
		
		void ComboBox1TextUpdate(object sender, EventArgs e)
		{
			/*if((comboBox1.Items.Contains(comboBox1.Text))==true){
				button4.Enabled=true;
			}else{
				button4.Enabled=false;
			}*/
		}

		private void Depu_manu_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (mode == 1 || mode == 3)
            {
                if (salir == 0)
                {

                    DialogResult respuesta = MessageBox.Show("Si cierra la ventana sin dar click a la opción de DEPURAR no se podrá realizar ninguna depuración.\n\n¿Desea Salir?", "ATENCIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                    if (respuesta == DialogResult.Yes)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                if (mode == 2)
                {
                    try
                    {
                        seleccionar_fila(dataGridView1.CurrentRow.Index);
                    }
                    catch
                    {
                    }
                }
            }
		}
		
		void ComboBox1TextChanged(object sender, EventArgs e)
		{
			if(comboBox1.Text.Length==0){
				comboBox1.SelectedIndex=-1;
			}
		}
		
		void DataGridView1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Space))
			{
				checar_marcados_listbox();
			}
		}
		
		void DataGridView1RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void DataGridView1RowLeave(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void DataGridView1CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			if(dataGridView1.RowCount>0){
				if(!(Convert.ToBoolean(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value)).Equals(v_inicial)){
					//MessageBox.Show(dataGridView1.CurrentRow.Index.ToString());
					seleccionar_fila(dataGridView1.CurrentRow.Index);
				}
			}
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				String archivo="",cad_con,cons_exc,reg_patro,cre_c,cre_m,reg_patro_2;
				OpenFileDialog dialog = new OpenFileDialog();
				int i=0,j=0;
				
				dialog.Title = "Seleccione el archivo de Créditos a Marcar";//le damos un titulo a la ventana
				dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
				
				dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
				
				//si al seleccionar el archivo damos Ok
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					archivo = dialog.FileName;
					if((archivo.Substring(archivo.Length-3,3).Equals("lsx")==true)||archivo.Substring(archivo.Length-3,3).Equals("xls")==true)
					{
						cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" +archivo + "';Extended Properties=Excel 12.0;";
						conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
						conexion.Open(); //abrimos la conexion
						carga_chema_excel();
						cons_exc = "Select * from ["+comboBox2.Items[0].ToString()+"$]";
						//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
						dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
						dataSet = new DataSet(); // creamos la instancia del objeto DataSet
						
						if(dataAdapter.Equals(null)){
							MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
						}else{
							if(dataAdapter == null){}else{
								dataAdapter.Fill(dataSet, comboBox2.Items[0].ToString());//llenamos el dataset
								lista_marcar = dataSet.Tables[0];
								
								while(i<lista_marcar.Rows.Count){
									reg_patro=lista_marcar.Rows[i][0].ToString().ToUpper();
									cre_c=lista_marcar.Rows[i][1].ToString();
									cre_m=lista_marcar.Rows[i][2].ToString();
									while(j<dataGridView1.RowCount){
										reg_patro_2=dataGridView1.Rows[j].Cells[1].Value.ToString();
										reg_patro_2=reg_patro_2.Substring(0,3)+reg_patro_2.Substring(4,5)+reg_patro_2.Substring(10,2);
										if(reg_patro.Equals(reg_patro_2)==true){
											if(cre_c.Equals(dataGridView1.Rows[j].Cells[3].Value.ToString())==true){
												if(cre_m.Equals(dataGridView1.Rows[j].Cells[4].Value.ToString())==true){
													dataGridView1.Rows[j].Cells[0].Value=true;
												}
											}
										}
										j++;
									}
									j=0;
									i++;
								}
								
							}
						}
					}
				}
				checar_marcados();
			}
		}
			
		void DataGridView1MouseClick(object sender, MouseEventArgs e)
		{
			  
		}
		
		void DataGridView1CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			//if(dataGridView1.RowCount>0){
				
			//		MessageBox.Show(dataGridView1.CurrentRow.Index.ToString());
			//		seleccionar_fila(dataGridView1.CurrentRow.Index);
				
			//}
		}
		
		void DataGridView1CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if(dataGridView1.RowCount>0){
				v_inicial=Convert.ToBoolean(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
			}
		}
		
		void DataGridView1Enter(object sender, EventArgs e)
		{
			
		}
	}
}

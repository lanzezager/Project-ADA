/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 17/05/2017
 * Hora: 05:07 p.m.
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
	/// Description of Generador_factura.
	/// </summary>
	public partial class Generador_factura : Form
	{
		public Generador_factura()
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
		DataTable consulta_per = new DataTable();
		DataTable consulta_cont = new DataTable();
		DataTable consulta_not = new DataTable();
		DataTable lista_dep = new DataTable();
		DataTable tablarow = new DataTable();
        DataTable tabla_cifra = new DataTable();
		
		int i=0;
		int[] contros;
		String carpeta_sel,cifra,fech_cifra,peri;
		
		//periodos normales
		public void llenar_Cb1(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			int i=0;
			consulta_per = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			do{
				comboBox1.Items.Add(consulta_per.Rows[i][0].ToString());
				i++;
			}while(i<consulta_per.Rows.Count);
			i=0;
            conex.cerrar();
		}
        //periodos especiales
        public void llenar_Cb1_especiales()
        {
            conex.conectar("base_principal");
            comboBox1.Items.Clear();
            int i = 0;
            consulta_per = conex.consultar("SELECT DISTINCT(periodo_factura) FROM datos_factura WHERE periodo_factura <> \"-\" ORDER BY periodo_factura");
            do
            {
                comboBox1.Items.Add(consulta_per.Rows[i][0].ToString());
                i++;
            } while (i < consulta_per.Rows.Count);
            i = 0;
            conex.cerrar();
        }
		//controladores
		public void llenar_Cb3(){
			conex.conectar("base_principal");
			comboBox3.Items.Clear();
			i=0;
			consulta_cont = conex.consultar("SELECT apellido,nombre,id_usuario FROM base_principal.usuarios WHERE puesto =\"Controlador\" ORDER BY apellido;");
			//dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (controlador) FROM base_principal.datos_factura ORDER BY controlador;");
			contros = new int[consulta_cont.Rows.Count];
			do{
				comboBox3.Items.Add(consulta_cont.Rows[i][0].ToString()+" "+consulta_cont.Rows[i][1].ToString());
				contros[i] = Convert.ToInt32(consulta_cont.Rows[i][2].ToString());
				i++;
			}while(i<consulta_cont.Rows.Count);
			i=0;
			//comboBox4.Items.Add("--NINGUNO--");
            conex.cerrar();
		}	
		//notificadores
		public void llenar_Cb2(){
			conex.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			//dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
			consulta_not = conex.consultar("SELECT DISTINCT (notificador) FROM base_principal.datos_factura ORDER BY notificador;");
			do{
				//comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				comboBox2.Items.Add(consulta_not.Rows[i][0]);
				i++;
			}while(i<consulta_not.Rows.Count);
			i=0;
			//comboBox3.Items.Add("--NINGUNO--");
            conex.cerrar();
		}
		//notificadores v2
		public void llenar_Cb2_v2(){
			conex2.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			consulta_not= conex2.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" AND controlador = \""+contros[comboBox3.SelectedIndex]+"\" ORDER BY apellido;");
			do{
				comboBox2.Items.Add(consulta_not.Rows[i][0]+" "+consulta_not.Rows[i][1]);
				i++;
			}while(i<consulta_not.Rows.Count);
			i=0;
			//comboBox3.Items.Add("--NINGUNO--");
            conex2.cerrar();
		}
		
		public void consultar_db(){
			String per="",noti,contro,sql="";
			
			if(comboBox1.SelectedIndex>-1){

                if(radioButton1.Checked==true){
                    per = "nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\"";
                }

                if (radioButton2.Checked == true)
                {
                    per = "periodo_factura=\"" + comboBox1.SelectedItem.ToString() + "\"";
                }
				
				if(comboBox2.SelectedIndex>-1){
					noti=" AND notificador=\""+comboBox2.SelectedItem.ToString()+"\"";
				}else{
					noti="";
				}
				if(comboBox3.SelectedIndex>-1){
					contro=" AND controlador=\""+comboBox3.SelectedItem.ToString()+"\"";
				}else{
					contro="";
				}
				
				dataGridView1.DataSource=null;
				
				sql="SELECT registro_patronal,razon_social,credito_cuotas,periodo,tipo_documento,sector_notificacion_inicial,sector_notificacion_actualizado,importe_cuota,id,credito_multa,importe_multa,fecha_entrega,controlador,status,fecha_notificacion "+
					"FROM datos_factura WHERE "+per+" "+noti+" "+contro+" ORDER BY sector_notificacion_actualizado,registro_patronal,credito_cuotas";
				//MessageBox.Show(sql);
				dataGridView1.DataSource=conex3.consultar(sql);
				estilo_grid();
				label10.Text="CASOS: "+dataGridView1.RowCount;
				try{
				dateTimePicker1.Text=dataGridView1.Rows[0].Cells[11].Value.ToString();
				}catch{}
				Refresh();
				
				dataGridView1.Focus();
				
			}
		}
		
		public void estilo_grid(){
		
			dataGridView1.Columns[0].HeaderText="REGISTRO PATRONAL";
			dataGridView1.Columns[1].HeaderText="RAZON SOCIAL";
			dataGridView1.Columns[2].HeaderText="CREDITO CUOTA";
			dataGridView1.Columns[3].HeaderText="PERIODO";
			dataGridView1.Columns[4].HeaderText="TIPO\nDOC.";
			dataGridView1.Columns[5].HeaderText="SECTOR\nINICIAL";
			dataGridView1.Columns[6].HeaderText="SECTOR\nACTUAL";
			dataGridView1.Columns[7].HeaderText="IMPORTE CUOTA";
			dataGridView1.Columns[8].Visible=false;
			dataGridView1.Columns[9].Visible=false;
			dataGridView1.Columns[10].Visible=false;
			dataGridView1.Columns[11].Visible=false;
			dataGridView1.Columns[12].HeaderText="CONTROLADOR";
            dataGridView1.Columns[13].HeaderText = "STATUS";
            dataGridView1.Columns[14].HeaderText = "FECHA NOT.";
		
		}
		
		public void generar(){
			String col1,col2,col3,col4,col5,col6,col7,col8,col9,nn,fecha,combreg_nom,estatus,fech_not;
			
			col1 = "";
			col2 = "";
			col3 = "";
			col4 = "";
			col5 = "";
			col6 = "";
			col7 = "";
			col8 = "";
            estatus = "";
            fech_not = "";
			
			buscar_nn();
            peri = comboBox1.SelectedItem.ToString();
            cifra_control(peri);

            fecha = dateTimePicker1.Value.ToShortDateString();

			i=0;
			while(i<dataGridView1.RowCount){
				col1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
				col2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
				col3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
				col4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
				col5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
				col6 = dataGridView1.Rows[i].Cells[6].Value.ToString();
				col7 = dataGridView1.Rows[i].Cells[7].Value.ToString();
				col8 = comboBox2.SelectedItem.ToString();//notificador
				col9 = comboBox3.SelectedItem.ToString();//controlador
				nn = dataGridView3.Rows[i].Cells[0].Value.ToString();//nn
                estatus = dataGridView1.Rows[i].Cells[13].Value.ToString();
                fech_not = dataGridView1.Rows[i].Cells[14].Value.ToString();

				if (col2.Length >= 10)
				{
					combreg_nom = col1 + "   " + col2.Substring(0, 10);
				}
				else
				{
					combreg_nom = col1 + "   " + col2.Substring(0, col2.Length);
				}

				if(col3.Length<9){
					col3 = dataGridView1.Rows[i].Cells[9].Value.ToString()+" M";
					col7 = dataGridView1.Rows[i].Cells[10].Value.ToString();
				}

                if(fech_not.Length>10){
                    fech_not=fech_not.Substring(0, 10);
                }

                tablarow.Rows.Add(combreg_nom, col3, col4, col5, col6, col7, col8, col9, comboBox1.SelectedItem.ToString(), nn, dataGridView1.Rows[i].Cells[5].Value.ToString(), cifra, fech_cifra, fecha, conex.leer_config_sub()[0].ToUpper(), conex.leer_config_sub()[3].ToUpper(), textBox2.Text,estatus,fech_not);
				i++;
			}
			
			Visor_reporte_factura visor1 = new Visor_reporte_factura();
            visor1.envio2(tablarow, carpeta_sel);
			visor1.Show();
			tablarow.Rows.Clear();
			
			fecha = dateTimePicker1.Text;
			//fecha = System.DateTime.Today.ToShortDateString();
            //fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);
                
			
            //sql = "UPDATE datos_factura SET fecha_entrega=\""+fecha+"\" WHERE id ="+dataGridView1.Rows[i].Cells[8].FormattedValue.ToString()+"";
			//conex4.consultar(sql);
			
			MessageBox.Show("Se han generado el reporte adecuadamente","Listo!");
			//conex.guardar_evento("Se Generaron Reportes de Factura de Notificadores del periodo: "+per_sel);
			Process.Start("explorer.exe", carpeta_sel);
			
		}
		
		public void buscar_nn(){
        	int nn_found = 0;
        	i=0;
            
            conex4.conectar("base_principal");
            
            dataGridView3.Rows.Clear();
        	do{
            	if(dataGridView1.RowCount>0){
	        		dataGridView2.DataSource = conex4.consultar("SELECT COUNT(nn) FROM datos_factura WHERE nn =\"NN\" AND registro_patronal =\""+dataGridView1.Rows[i].Cells[0].Value.ToString()+"\"");
	        		if((Convert.ToInt32 (dataGridView2.Rows[0].Cells[0].Value.ToString())) > 0){
	        			dataGridView3.Rows.Add("NN (Revisar)");
	                    nn_found++;
	        		}else{
	        			if(((Convert.ToInt32 (dataGridView2.Rows[0].Cells[0].Value.ToString())) == 0)){
	        				dataGridView3.Rows.Add("");
	        			}
	        		}
	        		i++;
                }
        	}while(i<dataGridView1.RowCount);
            
            if(nn_found>0){
            	MessageBox.Show("Se encontraron "+nn_found+" patrones que han sido marcados anteriormente como NN");
            }else{
            	
            }
        }

        public void cifra_control(String tipo_per)
        {
            conex5.conectar("base_principal");
            tabla_cifra = conex5.consultar("SELECT cifra_control,fecha_cifra_control_inicio,fecha_cifra_control_termino FROM estado_periodos WHERE nombre_periodo=\"" + tipo_per + "\"");
            if (tabla_cifra.Rows.Count > 0)
            {
            	if(tabla_cifra.Rows[0][0].ToString().Length>1){
            		cifra = tabla_cifra.Rows[0][0].ToString();
            	}else{
            		cifra="";
            	}
                
                if((tabla_cifra.Rows[0][1].ToString().Length>9)&&(tabla_cifra.Rows[0][2].ToString().Length>9)){
                	fech_cifra = tabla_cifra.Rows[0][1].ToString().Substring(0,10) + "-" + tabla_cifra.Rows[0][2].ToString().Substring(0,10);
                }else{
                	fech_cifra="";
                }
                
            }else{
            	cifra="";
            	fech_cifra="";
            }
        }

		void Generador_facturaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llenar_Cb1();
			llenar_Cb2();
			llenar_Cb3();
			conex3.conectar("base_principal");
			
			tablarow.Columns.Add("id");
			tablarow.Columns.Add("reg_pat");
			tablarow.Columns.Add("credito_cuota");
			tablarow.Columns.Add("periodo");
			tablarow.Columns.Add("tipo doc");
			tablarow.Columns.Add("sector");
			tablarow.Columns.Add("importe");
			tablarow.Columns.Add("noti");
			tablarow.Columns.Add("contro");
            tablarow.Columns.Add("nom_periodo");
            tablarow.Columns.Add("sector_inicial");
            dataGridView3.Columns.Add("nn","nn");
            tablarow.Columns.Add("cifra");
            tablarow.Columns.Add("fecha_cifra");
            tablarow.Columns.Add("fecha_fact");
            tablarow.Columns.Add("del");
            tablarow.Columns.Add("subdel");
            tablarow.Columns.Add("jefe_secc_epo");
            tablarow.Columns.Add("estatus");
            tablarow.Columns.Add("fecha_not");

            label3.Text = "Delegación "+conex.leer_config_sub()[0].ToUpper()+"\nSubdelegación "+conex.leer_config_sub()[3].ToUpper()+"\nDepartamento de Cobranza";
            textBox2.Text = conex.leer_config_sub()[9];
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			consultar_db();
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			consultar_db();
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			/*if(comboBox3.SelectedIndex > -1){
				try{
					llenar_Cb2_v2();
				}catch(Exception es){
					comboBox2.Items.Clear();
					comboBox2.SelectedIndex=-1;
				}
			}else{
				llenar_Cb2();
			}*/
			
			consultar_db();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			int generar_lock=0;
			
			if(comboBox1.SelectedIndex>-1){
				generar_lock++;
			}
			
			if(comboBox2.SelectedIndex>-1){
				generar_lock++;
			}
			
			if(comboBox3.SelectedIndex>-1){
				generar_lock++;
			}
			
			if(dataGridView1.RowCount>0){
				generar_lock++;
			}
			
			if(generar_lock==4){
				tablarow.Clear();
				FolderBrowserDialog fbd = new FolderBrowserDialog();
				fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
				DialogResult result = fbd.ShowDialog();
				
				if (result == DialogResult.OK)
				{
					carpeta_sel= fbd.SelectedPath.ToString();
					generar();
				}
			}
		}
		
		void DataGridView1RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			label10.Text="CASOS: "+dataGridView1.RowCount;
			Refresh();
		}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            llenar_Cb1();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            llenar_Cb1_especiales();
        }
	}
}

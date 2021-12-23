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
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;

namespace Nova_Gear
{
    public partial class Cartera_reportes : Form
    {
        public Cartera_reportes()
        {
            InitializeComponent();
        }

        String sql,fecha,fecha_cartera,id_us;
        int i = 0,conta=0,indice=0,b=0;

        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
        DataTable consultamysql = new DataTable();
        DataTable consultamysql2 = new DataTable();

        //periodos
        public void llenar_Cb1()
        {
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE nombre_periodo NOT LIKE \"CLEM%\" ORDER BY nombre_periodo;");
            do
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void llenar_Cb1_clem()
        {
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura WHERE nombre_periodo LIKE \"CLEM%\" ORDER BY nombre_periodo;");
            do
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void nombre_cols()
        {
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[0].ReadOnly = false;
            dataGridView1.Columns[1].HeaderText = "REGISTRO\nPATRONAL1";
            dataGridView1.Columns[2].HeaderText = "RAZÓN\nSOCIAL";
            dataGridView1.Columns[3].HeaderText = "CRÉDITO\nCUOTA";
            dataGridView1.Columns[4].HeaderText = "CRÉDITO\nMULTA";
            dataGridView1.Columns[5].HeaderText = "IMPORTE\nCUOTA";
            dataGridView1.Columns[6].HeaderText = "IMPORTE\nMULTA";
            dataGridView1.Columns[7].HeaderText = "TIPO\nDOCUMENTO";
            dataGridView1.Columns[8].HeaderText = "PERIODO";
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            
            if(comboBox1.SelectedIndex == 2){
            	dataGridView1.Columns[10].HeaderText = "STATUS";
            }

            for (int ind_f = 0; ind_f < dataGridView1.RowCount;ind_f++)
            {
                dataGridView1.Rows[ind_f].Cells[0].Style.BackColor = System.Drawing.Color.DodgerBlue;
            }
        }
        
        public void marcar_de_inicio(){
        	int kl=0;
            int cols=0;
            while(kl<dataGridView1.RowCount){
            	//MessageBox.Show(""+dataGridView1.Rows[kl].Cells[9].Value.ToString());
            	if (dataGridView1.Rows[kl].Cells[10].Value.ToString().StartsWith("PENDIENTE"))
            	{
            		cols=1;
                    dataGridView1[0, kl].Value = true;

                    while (cols < dataGridView1.Columns.Count)
            		{
            			dataGridView1.Rows[kl].Cells[cols].Style.BackColor = System.Drawing.Color.MediumSlateBlue;
            			dataGridView1.Rows[kl].Cells[cols].Style.ForeColor = System.Drawing.Color.White;
            			
            			cols++;
            		}
            		//  if(conta<dataGridView1.RowCount){
            		//	conta=conta+1;
            		//}
            	}
            	kl++;
            }
        }

        private void Cartera_reportes_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            llenar_Cb1();

            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            conex3.conectar("base_principal");

            id_us = MainForm.datos_user_static[7];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        	if ((comboBox1.SelectedIndex == 1))
            {
        		label1.Visible = false;
                comboBox2.Visible = false;
                panel1.Visible = true;
                //button2.Enabled = true;               
            }
            else
            {
            	if(comboBox1.SelectedIndex>-1 && comboBox1.SelectedIndex<4){
                    llenar_Cb1();
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    panel1.Visible = false;
                    //button2.Enabled = false;
                    comboBox2.SelectedIndex = -1;
            	}

                if ((comboBox1.SelectedIndex == 4))
                {
                    llenar_Cb1_clem();
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    panel1.Visible = false;
                    //button2.Enabled = false;
                    comboBox2.SelectedIndex = -1;              
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //button2.Enabled = true;
        }
		//cargar
		private void button2_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0)
			{//notificados
				if(comboBox2.SelectedIndex>-1){
					sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera "+
                        "FROM datos_factura WHERE nombre_periodo = \"" + comboBox2.SelectedItem.ToString() + "\" AND status = \"NOTIFICADO\" AND nn <> \"NN\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_"+id_us+"\") ORDER BY registro_patronal,credito_cuotas";
				}else{
					sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera "+
                        "FROM datos_factura WHERE status = \"NOTIFICADO\" AND nn <> \"NN\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
				}
			}else
			{
                if (comboBox1.SelectedIndex > -1 && comboBox1.SelectedIndex < 4)
                {
					//no notificados
					if (comboBox1.SelectedIndex == 1){
						fecha = dateTimePicker1.Text;
						fecha = fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
						//MessageBox.Show(fecha);
						sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera " +
                            "FROM datos_factura WHERE fecha_recepcion >= \"" + fecha + "\" AND nn=\"NN\" AND (status <> \"CARTERA\" AND status NOT LIKE \"DEPU%\") AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
					}else{
						if (comboBox1.SelectedIndex == 2){//mixto
							if(comboBox2.SelectedIndex>-1){
								sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera,status "+
                                    "FROM datos_factura WHERE nombre_periodo = \"" + comboBox2.SelectedItem.ToString() + "\" AND status <> \"CARTERA\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
							}else{
								sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera,status "+
                                    "FROM datos_factura WHERE status <> \"CARTERA\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
							}
						}else{//estrados
							if(comboBox2.SelectedIndex>-1){
								sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera "+
                                    "FROM datos_factura WHERE nombre_periodo = \"" + comboBox2.SelectedItem.ToString() + "\" AND fecha_cartera IS NULL AND nn = \"ESTRADOS\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
							}else{
								sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera "+
                                    "FROM datos_factura WHERE fecha_cartera IS NULL AND nn = \"ESTRADOS\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
							}
						}
					}
				}else{

                    if (comboBox1.SelectedIndex == 4)
                    {
                        if (comboBox2.SelectedIndex > -1)
                        {
                            sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera " +
                                "FROM datos_factura WHERE nombre_periodo = \"" + comboBox2.SelectedItem.ToString() + "\" AND status = \"NOTIFICADO\" AND nn <> \"NN\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
                        }
                        else
                        {
                            sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera " +
                                "FROM datos_factura WHERE nombre_periodo LIKE \"CLEM%\" AND status = \"NOTIFICADO\" AND nn <> \"NN\" AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\") ORDER BY registro_patronal,credito_cuotas";
                        }                    
                    }
                    else
                    {
                        MessageBox.Show("Elige un Tipo de Entrega.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
				}
			}
			if(comboBox1.SelectedIndex>-1){
				//MessageBox.Show(sql);
				conex.conectar("base_principal");
                consultamysql2=conex.consultar(sql);
               /* for (int f = 0; f < consultamysql2.Rows.Count;f++)
                {
                    dataGridView1.Rows.Add( consultamysql2.Rows[f][0].ToString(),
                                            consultamysql2.Rows[f][1].ToString(),
                                            consultamysql2.Rows[f][2].ToString(),
                                            consultamysql2.Rows[f][3].ToString(),
                                            consultamysql2.Rows[f][4].ToString(),
                                            consultamysql2.Rows[f][5].ToString(),
                                            consultamysql2.Rows[f][6].ToString(),
                                            consultamysql2.Rows[f][7].ToString(),
                                            consultamysql2.Rows[f][8].ToString(),
                                            consultamysql2.Rows[f][9].ToString());
                }*/

                dataGridView1.DataSource = consultamysql2;
				nombre_cols();
				label4.Text = "Registros Cargados: "+dataGridView1.RowCount;
				label4.Refresh();
				if (dataGridView1.RowCount > 0)
				{
					button1.Enabled = true;
				}
				else
				{
					button1.Enabled = false;
				}
				conex.cerrar();
				
				marcar_de_inicio();
			}
		}

        private void label4_Click(object sender, EventArgs e)
        {

        }
		//guardar
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult resu = MessageBox.Show("Se va a Generar el Reporte de entrega a cartera de\n"+conta+" créditos "+comboBox1.SelectedItem.ToString()+".\nEsto afectará la Base de Datos."+
                "\n\n¿Está seguro que desea continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
        
            if(resu == DialogResult.Yes){

                i = 0;
                conex2.conectar("base_principal");
                consultamysql.Rows.Clear();
                fecha_cartera = System.DateTime.Today.ToShortDateString();
                fecha_cartera = fecha_cartera.Substring(6, 4) + "-" + fecha_cartera.Substring(3, 2) + "-" + fecha_cartera.Substring(0, 2);
                do{
                	if(dataGridView1.Rows[i].Cells[1].Style.BackColor == System.Drawing.Color.MediumSlateBlue){
                		consultamysql.Rows.Add(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[2].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[3].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[4].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[5].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[6].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[7].FormattedValue.ToString(),
                		                       dataGridView1.Rows[i].Cells[8].FormattedValue.ToString());

                        if (comboBox1.SelectedIndex != 4)
                        {
                		    sql = "UPDATE datos_factura SET status=\"CARTERA\",fecha_cartera =\""+fecha_cartera+"\",estado_cartera=\"ENTREGADO\" WHERE id=" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString()+"";
                		}else{
                            sql = "UPDATE datos_factura SET status=\"C_EMPRESAS\",fecha_cartera =\"" + fecha_cartera + "\",estado_cartera=\"ENTREGADO\" WHERE id=" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + "";
                        }

                        conex2.consultar(sql);
                		conex2.guardar_evento("Se entrega a cartera el crédito con el ID: "+dataGridView1.Rows[i].Cells[9].FormattedValue.ToString()+ " el dia: "+fecha_cartera);
                	}
                    i++;
                }while(i<dataGridView1.RowCount);
                conex2.cerrar();
                
                if (comboBox1.SelectedIndex == 0)
                {//notificados
                    Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
                    if(comboBox2.SelectedIndex > -1){
                    	entrega_cartera.envio3(consultamysql,"RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS A CARTERA",comboBox2.SelectedItem.ToString(),"CARTERA");
                    }else{
                        entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS A CARTERA", " ", "CARTERA");
                    	//conex2.guardar_evento("Se Generó Reporte de Entrega a cartera de créditos Notificados del Periodo: "+comboBox2.SelectedItem.ToString());
                    }
                    entrega_cartera.Show();
                    //this.Hide();
                }
                else
                {
                	if (comboBox1.SelectedIndex == 2){//mixto
                		Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
                		if(comboBox2.SelectedIndex > -1){
                            entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NO NOTIFICADOS A CARTERA", comboBox2.SelectedItem.ToString() + " MIXTO", "CARTERA");
                		}else{
                            entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NO NOTIFICADOS A CARTERA", "MIXTO", "CARTERA");
                		}
                		//conex2.guardar_evento("Se Generó Reporte de Entrega a cartera de créditos NO Notificados procesados el "+fecha);
	                    entrega_cartera.Show();
                		
                	}else{
                		if (comboBox1.SelectedIndex == 1){//no notificados
			                    Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
                                entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NO NOTIFICADOS A CARTERA", "NO NOTIFICADOS", "CARTERA");
			                   //conex2.guardar_evento("Se Generó Reporte de Entrega a cartera de créditos NO Notificados procesados el "+fecha);
			                    entrega_cartera.Show();
                		}else{
                            if (comboBox1.SelectedIndex == 3)
                            {
                                Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
                                if (comboBox2.SelectedIndex > -1)
                                {
                                    entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS POR ESTRADOS A CARTERA", comboBox2.SelectedItem.ToString() + " ESTRADOS", "CARTERA");
                                }
                                else
                                {
                                    entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS POR ESTRADOS A CARTERA", "ESTRADOS", "CARTERA");
                                }
                                //conex2.guardar_evento("Se Generó Reporte de Entrega a cartera de créditos NO Notificados procesados el "+fecha);
                                entrega_cartera.Show();
                            }
                            else
                            {
                                if (comboBox1.SelectedIndex == 4)
                                {//notificados CLEM 
                                    Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
                                    if (comboBox2.SelectedIndex > -1)
                                    {
                                        entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS A CLASIFICACIÓN DE EMPRESAS", comboBox2.SelectedItem.ToString(), "C. de EMPRESAS");
                                    }
                                    else
                                    {
                                        entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS A CLASIFICACIÓN DE EMPRESAS", " ", "C. de EMPRESAS");
                                        //conex2.guardar_evento("Se Generó Reporte de Entrega a cartera de créditos Notificados del Periodo: "+comboBox2.SelectedItem.ToString());
                                    }
                                    entrega_cartera.Show();
                                    //this.Hide();
                                }
                            }
                			
                		}
                	}
                }
                
            	dataGridView1.DataSource=null;    
            
            }
        }
		//marcar con click
        public void seleccionar_fila()
        {
            int cols = 1;
            //MessageBox.Show("|"+dataGridView1.Rows[indice].Cells[cols].Style.BackColor.ToString()+"|");
            if (dataGridView1.Rows[indice].Cells[cols].Style.BackColor == System.Drawing.SystemColors.Window || dataGridView1.Rows[indice].Cells[cols].Style.BackColor.ToString() == "Color [Empty]")
            {
                dataGridView1[0, indice].Value = true;
                while (cols < dataGridView1.Columns.Count)
                {
                    dataGridView1.Rows[indice].Cells[cols].Style.BackColor = System.Drawing.Color.MediumSlateBlue;
                    dataGridView1.Rows[indice].Cells[cols].Style.ForeColor = System.Drawing.Color.White;					
                    cols++;
                }
              //  if(conta<dataGridView1.RowCount){
                //	conta=conta+1;
                //}
                //MessageBox.Show(dataGridView1.Rows[indice].Cells[9].FormattedValue.ToString() + " marca");
                conex3.consultar("UPDATE datos_factura SET estado_cartera=\"PENDIENTE_"+id_us+"\" WHERE id=" + dataGridView1.Rows[indice].Cells[9].FormattedValue.ToString()+"");
            }
            else
            {
                dataGridView1[0, indice].Value = false;
                while (cols < dataGridView1.Columns.Count)
                {
                    dataGridView1.Rows[indice].Cells[cols].Style.BackColor = System.Drawing.SystemColors.Window;
                    dataGridView1.Rows[indice].Cells[cols].Style.ForeColor = System.Drawing.SystemColors.ControlText;					
                    cols++;
                }
                //MessageBox.Show(dataGridView1.Rows[indice].Cells[9].FormattedValue.ToString()+" desmarca");
                conex3.consultar("UPDATE datos_factura SET estado_cartera=\"-\" WHERE id=" + dataGridView1.Rows[indice].Cells[9].FormattedValue.ToString()+"");
              //  if(conta>0){
                //	conta=conta-1;
               // }
            }
            
            
        }
        
        public void buscar_credito(){
        	String buscar_cred;
        	int b_aux=0;
        	if(dataGridView1.RowCount>0){
        		if(maskedTextBox2.Text.Length==9){
        			buscar_cred=maskedTextBox2.Text;
        			while(b<dataGridView1.RowCount){
        				if(radioButton1.Checked==true){//cuota
        					if(dataGridView1.Rows[b].Cells[3].FormattedValue.ToString().Equals(buscar_cred)){
        						dataGridView1.ClearSelection();
        						dataGridView1.Rows[b].Cells[0].Selected=true;
        						dataGridView1.FirstDisplayedScrollingRowIndex=b;
        						b_aux=b;
        						b=dataGridView1.RowCount+1;
        					}
        				}else{//multa
        					if(dataGridView1.Rows[b].Cells[4].FormattedValue.ToString().Equals(buscar_cred)){
        						dataGridView1.ClearSelection();
        						dataGridView1.Rows[b].Cells[0].Selected=true;
        						dataGridView1.FirstDisplayedScrollingRowIndex=b;
        						b_aux=b;
        						b=dataGridView1.RowCount+1;
        					}
        				}
        				b++;
        			}
        			b=b_aux+1;
        		}else{
        			
        		}
        	}
        }     

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        	/*try{
        		if (e.KeyChar == Convert.ToChar(Keys.NumPad0)) {
        			indice=dataGridView1.CurrentRow.Index;
        			seleccionar_fila();
        		}
        		
        		if (e.KeyChar == Convert.ToChar(Keys.Add)) {
        			indice=dataGridView1.CurrentRow.Index;
        			seleccionar_fila();
        		}
        		
        		if (e.KeyChar == Convert.ToChar(Keys.Space)) {
        			indice=dataGridView1.CurrentRow.Index;
        			seleccionar_fila();
        		}
        	}catch{}*/
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==0){
                indice =e.RowIndex;
                if(indice>-1){
                    seleccionar_fila();
                }
            }
        }

        void DataGridView1Click(object sender, EventArgs e)
        {

        }

        void DataGridView1MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //if(e.Button== MouseButtons.Right){
                indice = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                if (indice > -1)
                {
                    seleccionar_fila();
                }
                //}
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {/*
            if (e.ColumnIndex == 0)
            {
                indice = e.RowIndex;
                if (indice > -1)
                {
                    seleccionar_fila();
                }
            }*/
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                indice = e.RowIndex;
                if (indice > -1)
                {
                    seleccionar_fila();
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
        	try{
	            if( e.KeyCode == Keys.NumPad0){
	        		indice=dataGridView1.CurrentRow.Index;
	                seleccionar_fila();
	            }
        		
        		if( e.KeyCode== Keys.Add){
	        		indice=dataGridView1.CurrentRow.Index;
	                seleccionar_fila();
	            }
        		
        		if( e.KeyCode== Keys.Decimal){
	        		indice=dataGridView1.CurrentRow.Index;
	                seleccionar_fila();
	            }
        	}catch(Exception es){
        		
        	}
        }
               
        //marcar
        void Button3Click(object sender, EventArgs e)
        {
        	try{
			int cols=0;
			indice=0;
			
			do{
				cols=1;
				 do
                {
                    dataGridView1.Rows[indice].Cells[cols].Style.BackColor = System.Drawing.Color.MediumSlateBlue;
                    dataGridView1.Rows[indice].Cells[cols].Style.ForeColor = System.Drawing.Color.White;
                     cols++;
                } while (cols < dataGridView1.Columns.Count);
				indice++;
			}while(indice < dataGridView1.RowCount);
			
			conta=dataGridView1.RowCount;
			label6.Text="Registros Marcados: "+conta;		
			label6.Refresh();
        	}catch(Exception d){
        		MessageBox.Show("No hay nada que marcar","AVISO");
        	}
        }
        //desmarcar
        void Button4Click(object sender, EventArgs e)
        {
        	DialogResult res = MessageBox.Show("Se Desmarcarán todos los Créditos marcados actualmente.\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
        	if(res==DialogResult.Yes){
        		try{
        			int cols=0;
        			indice=0;
        			
        			do{
        				cols=1;
        				if(dataGridView1.Rows[indice].Cells[1].Style.BackColor == System.Drawing.Color.MediumSlateBlue){
        					conex3.consultar("UPDATE datos_factura SET estado_cartera=\"\" WHERE id=" + dataGridView1.Rows[indice].Cells[9].FormattedValue.ToString()+"");
        				}

                        dataGridView1[0, indice].Value = false;

        				do
        				{
        					dataGridView1.Rows[indice].Cells[cols].Style.BackColor = System.Drawing.SystemColors.Window;
        					dataGridView1.Rows[indice].Cells[cols].Style.ForeColor = System.Drawing.SystemColors.ControlText;
        					cols++;
        				} while (cols < dataGridView1.Columns.Count);
        				
        				indice++;
        			}while(indice < dataGridView1.RowCount);
        			conta=0;
        			label6.Text="Registros Marcados: "+conta;
        			label6.Refresh();
        		}catch(Exception f){
        			MessageBox.Show("No hay nada que desmarcar","AVISO");
        		}
        	}
        }
        
        void DataGridView1KeyUp(object sender, KeyEventArgs e)
        {
        	 /*try{
	            if( e.KeyCode == Keys.NumPad0){
	        		indice=dataGridView1.CurrentRow.Index;
	                seleccionar_fila();
	            }
        		
        		if( e.KeyCode== Keys.Add){
	        		indice=dataGridView1.CurrentRow.Index;
	                seleccionar_fila();
	            }
        		
        		if( e.KeyCode== Keys.Space){
	        		indice=dataGridView1.CurrentRow.Index;
	                seleccionar_fila();
	            }
        	}catch(Exception es){
        		
        	}*/
        }
        
        void Timer1Tick(object sender, EventArgs e)
        {
        	if(dataGridView1.RowCount>0){
        		int l=0;
        		conta=0;
                while (l < dataGridView1.RowCount) 
                {
        			if(dataGridView1.Rows[l].Cells[1].Style.BackColor == System.Drawing.Color.MediumSlateBlue){
        				conta++;
        			}
        			l++;
        		}
        		
        		label6.Text="Registros Marcados: "+conta;
				label6.Refresh();
        		
        	}
        }
        
        void ToolTip1Popup(object sender, PopupEventArgs e)
        {
        	
        }
        //guardar marcados - desmarcados
        void Button5Click(object sender, EventArgs e)
        {
        	try{
			int jk=0,seleccionados=0;
			conex3.conectar("base_principal");
            while (jk < dataGridView1.RowCount) 
            {
				//MessageBox.Show(""+dataGridView1.Rows[jk].Cells[0].Style.BackColor.Name);
				if(dataGridView1.Rows[jk].Cells[1].Style.BackColor.Name.Equals("MediumSlateBlue")){
                    MessageBox.Show(dataGridView1.Rows[jk].Cells[9].FormattedValue.ToString());
					conex3.consultar("UPDATE datos_factura SET estado_cartera=\"PENDIENTE\" WHERE id=" + dataGridView1.Rows[jk].Cells[9].FormattedValue.ToString()+"");
					seleccionados++;
				}else{
					conex3.consultar("UPDATE datos_factura SET estado_cartera=\"\" WHERE id=" + dataGridView1.Rows[jk].Cells[9].FormattedValue.ToString()+"");
				}
				jk++;
			}  
			conex3.cerrar();
			MessageBox.Show("Se guardó la selección de "+seleccionados+" registros","AVISO");
        	}catch(Exception d){
        		MessageBox.Show("No hay registros que guardar","AVISO");
        	}
        }
        
        void ComboBox1TextUpdate(object sender, EventArgs e)
        {
        	if(comboBox1.Text.Length==0){
        		comboBox1.SelectedIndex=-1;
        	}
        }
        
        void ComboBox2TextUpdate(object sender, EventArgs e)
        {
        	if(comboBox2.Text.Length==0){
        		comboBox2.SelectedIndex=-1;
        	}
        }
        
        void Button6Click(object sender, EventArgs e)
        {
        	if(panel2.Visible==true){
        		panel2.Visible=false;
        		panel2.Enabled=false;
        	}else{
        		panel2.Visible=true;
        		panel2.Enabled=true;
        		maskedTextBox2.Focus();
        	}
        }
        
        void BuscarCreditoToolStripMenuItemClick(object sender, EventArgs e)
        {
			if(panel2.Visible==true){
        		panel2.Visible=false;
        		panel2.Enabled=false;
        	}else{
        		panel2.Visible=true;
        		panel2.Enabled=true;
        		maskedTextBox2.Focus();
        	}      	
        }
        
        void Button7Click(object sender, EventArgs e)
        {
        	buscar_credito();
        }
        
        void MaskedTextBox2TextChanged(object sender, EventArgs e)
        {
        	if(maskedTextBox2.Text.Length<9){
        		b=0;
        	}
        }
        
        void Button8Click(object sender, EventArgs e)
        {
        	panel2.Visible=false;
        	panel2.Enabled=false;
        }
        
        void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
        {
        	if(maskedTextBox2.Text.Length==9){ 
	        	if (e.KeyChar == (char)(Keys.Enter))
	        	{
	        		buscar_credito();
	        	}
        	}
        }

        private void maskedTextBox2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox2.Text.Length == 9)
            {
                maskedTextBox2.SelectionStart = 0;
            }
            else
            {
                maskedTextBox2.SelectionStart = maskedTextBox2.Text.Length;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Reportes_cartera_Nvo repo = new Reportes_cartera_Nvo();
            //this.Hide();
            repo.Show();
        }

    }
}

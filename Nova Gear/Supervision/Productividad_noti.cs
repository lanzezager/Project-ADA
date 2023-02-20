/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 28/12/2016
 * Hora: 11:38 a.m.
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

namespace Nova_Gear.Supervision
{
	/// <summary>
	/// Description of Productividad_noti.
	/// </summary>
	public partial class Productividad_noti : Form
	{
		public Productividad_noti()
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
		Conexion conex1 = new Conexion();
		Conexion conex2= new Conexion();
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		Conexion conex5 = new Conexion();
		
		Conexion conex6 = new Conexion();
		Conexion conex7 = new Conexion();
		Conexion conex8= new Conexion();
		Conexion conex9 = new Conexion();
		
		Conexion conex10 = new Conexion();
		Conexion conex11 = new Conexion();		
		Conexion conex12 = new Conexion();
		Conexion conex13 = new Conexion();

        Conexion conex14 = new Conexion();

		DataTable tabla_totales = new DataTable();
		DataTable tabla_entramite = new DataTable();
		DataTable tabla_notificados = new DataTable();
		DataTable tabla_pagados = new DataTable();
		DataTable tabla_depurados = new DataTable();
		DataTable tabla_nn = new DataTable();
        DataTable tabla_estrados = new DataTable();
		DataTable tabla_periodos = new DataTable();
		
		DataTable tabla_totales_o= new DataTable();
		DataTable tabla_entramite_o = new DataTable();
		DataTable tabla_notificados_o = new DataTable();
		DataTable tabla_nn_o = new DataTable();
		
		DataTable tabla_totales_e = new DataTable();
		DataTable tabla_entramite_e = new DataTable();
		DataTable tabla_notificados_e = new DataTable();
		DataTable tabla_nn_e = new DataTable();
		
		DataTable tabla_totales_suma = new DataTable();
		DataTable tabla_totales_cred = new DataTable();
		DataTable tabla_totales_ofis = new DataTable();
		DataTable tabla_totales_props = new DataTable();
		
		SaveFileDialog fichero = new SaveFileDialog();
				
		public void llenar_Cb1_todos()
        {
			conex.conectar("base_principal");
            comboBox1.Items.Clear();
            int i = 0;
            tabla_periodos = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
            do
            {
            	comboBox1.Items.Add(tabla_periodos.Rows[i][0].ToString());
                i++;
            } while (i < tabla_periodos.Rows.Count);
            
            i = 0;
            conex.cerrar();
        }
		
		public void llenar_dgv7(){
			int i=0,j=0,tot=0,porcent=0;
			
			dataGridView7.Rows.Clear();
			
			//entregados
			while(i<dataGridView1.RowCount){
				dataGridView7.Rows.Add();
				dataGridView7.Rows[i].Cells[0].Value=dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
				dataGridView7.Rows[i].Cells[1].Value=dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
				i++;
			}
			
			i=0;
			
			//en_tramite
			while(i<dataGridView2.RowCount){
				while(j<dataGridView7.RowCount){
					if(dataGridView7.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView2.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView7.Rows[j].Cells[2].Value=dataGridView2.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView7.RowCount){
				if(dataGridView7.Rows[i].Cells[2].FormattedValue.ToString().Length==0){
					dataGridView7.Rows[i].Cells[2].Value=0;
				}
				i++;
			}
			
			i=0;
			
			//notificados
			while(i<dataGridView3.RowCount){
				while(j<dataGridView7.RowCount){
					if(dataGridView7.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView3.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView7.Rows[j].Cells[3].Value=dataGridView3.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView7.RowCount){
				if(dataGridView7.Rows[i].Cells[3].FormattedValue.ToString().Length==0){
					dataGridView7.Rows[i].Cells[3].Value="0";
				}
				i++;
			}
			
			i=0;
			
			//pagados
			while(i<dataGridView4.RowCount){
				while(j<dataGridView7.RowCount){
					if(dataGridView7.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView4.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView7.Rows[j].Cells[4].Value=dataGridView4.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView7.RowCount){
				if(dataGridView7.Rows[i].Cells[4].FormattedValue.ToString().Length==0){
					dataGridView7.Rows[i].Cells[4].Value="0";
				}
				i++;
			}
			
			i=0;
			
			//depurados
			while(i<dataGridView6.RowCount){
				while(j<dataGridView7.RowCount){
					if(dataGridView7.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView6.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView7.Rows[j].Cells[5].Value=dataGridView6.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView7.RowCount){
				if(dataGridView7.Rows[i].Cells[5].FormattedValue.ToString().Length==0){
					dataGridView7.Rows[i].Cells[5].Value="0";
				}
				i++;
			}
			
			i=0;
			
			//nn
			while(i<dataGridView5.RowCount){
				while(j<dataGridView7.RowCount){
					if(dataGridView7.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView5.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView7.Rows[j].Cells[6].Value=dataGridView5.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView7.RowCount){
				if(dataGridView7.Rows[i].Cells[6].FormattedValue.ToString().Length==0){
					dataGridView7.Rows[i].Cells[6].Value="0";
				}
				i++;
			}

            i = 0;

            //Estrados
            while (i < dataGridView19.RowCount)
            {
                while (j < dataGridView7.RowCount)
                {
                    if (dataGridView7.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView19.Rows[i].Cells[0].FormattedValue.ToString()))
                    {
                        dataGridView7.Rows[j].Cells[7].Value = dataGridView19.Rows[i].Cells[1].FormattedValue.ToString();
                    }
                    j++;
                }
                j = 0;
                i++;
            }

            i = 0;
            j = 0;

            while (i < dataGridView7.RowCount)
            {
                if (dataGridView7.Rows[i].Cells[7].FormattedValue.ToString().Length == 0)
                {
                    dataGridView7.Rows[i].Cells[7].Value = "0";
                }
                i++;
            }
			
			//totales_trabajados
			i=0;
			j=0;
			
			while(i<dataGridView7.RowCount){
				tot=0;
				porcent=0;
				
				tot=Convert.ToInt32(dataGridView7.Rows[i].Cells[3].FormattedValue.ToString())+
					Convert.ToInt32(dataGridView7.Rows[i].Cells[4].FormattedValue.ToString())+
					/*Convert.ToInt32(dataGridView7.Rows[i].Cells[5].FormattedValue.ToString())+*/
					Convert.ToInt32(dataGridView7.Rows[i].Cells[6].FormattedValue.ToString());
                    Convert.ToInt32(dataGridView7.Rows[i].Cells[7].FormattedValue.ToString());
					
				dataGridView7.Rows[i].Cells[8].Value=tot;
				try{
					porcent=Convert.ToInt32((tot*100)/Convert.ToInt32(dataGridView7.Rows[i].Cells[1].FormattedValue.ToString()));
				}catch(Exception esd){
					porcent=0;
				}
				dataGridView7.Rows[i].Cells[9].Value=porcent;
				i++;
			}
			
		}
		
		public void llenar_dgv8(){
			int i=0,j=0,tot=0,porcent=0;
			
			dataGridView8.Rows.Clear();
			
			//entregados
			while(i<dataGridView9.RowCount){
				dataGridView8.Rows.Add();
				dataGridView8.Rows[i].Cells[0].Value=dataGridView9.Rows[i].Cells[0].FormattedValue.ToString();
				dataGridView8.Rows[i].Cells[1].Value=dataGridView9.Rows[i].Cells[1].FormattedValue.ToString();
				i++;
			}
			
			i=0;
			
			//en_tramite
			while(i<dataGridView10.RowCount){
				while(j<dataGridView8.RowCount){
					if(dataGridView8.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView10.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView8.Rows[j].Cells[2].Value=dataGridView10.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView8.RowCount){
				if(dataGridView8.Rows[i].Cells[2].FormattedValue.ToString().Length==0){
					dataGridView8.Rows[i].Cells[2].Value=0;
				}
				i++;
			}
			
			i=0;
			
			//notificados
			while(i<dataGridView11.RowCount){
				while(j<dataGridView8.RowCount){
					if(dataGridView8.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView11.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView8.Rows[j].Cells[3].Value=dataGridView11.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView8.RowCount){
				if(dataGridView8.Rows[i].Cells[3].FormattedValue.ToString().Length==0){
					dataGridView8.Rows[i].Cells[3].Value="0";
				}
				i++;
			}
			
			i=0;
			
			
			//nn
			while(i<dataGridView12.RowCount){
				while(j<dataGridView8.RowCount){
					if(dataGridView8.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView12.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView8.Rows[j].Cells[4].Value=dataGridView12.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView8.RowCount){
				if(dataGridView8.Rows[i].Cells[4].FormattedValue.ToString().Length==0){
					dataGridView8.Rows[i].Cells[4].Value="0";
				}
				i++;
			}
			
			//totales_trabajados
			i=0;
			j=0;
			
			while(i<dataGridView8.RowCount){
				tot=0;
				porcent=0;
				
				tot=Convert.ToInt32(dataGridView8.Rows[i].Cells[3].FormattedValue.ToString())+
					Convert.ToInt32(dataGridView8.Rows[i].Cells[4].FormattedValue.ToString());
					
				dataGridView8.Rows[i].Cells[5].Value=tot;
				try{
					porcent=Convert.ToInt32((tot*100)/Convert.ToInt32(dataGridView8.Rows[i].Cells[1].FormattedValue.ToString()));
				}catch(Exception esd){
					porcent=0;
				}
				dataGridView8.Rows[i].Cells[6].Value=porcent;
				i++;
			}
			
		}
		
		public void llenar_dgv13(){
			int i=0,j=0,tot=0,porcent=0;
			
			dataGridView13.Rows.Clear();
			
			//entregados
			while(i<dataGridView14.RowCount){
				dataGridView13.Rows.Add();
				dataGridView13.Rows[i].Cells[0].Value=dataGridView14.Rows[i].Cells[0].FormattedValue.ToString();
				dataGridView13.Rows[i].Cells[1].Value=dataGridView14.Rows[i].Cells[1].FormattedValue.ToString();
				i++;
			}
			
			i=0;
			
			//en_tramite
			while(i<dataGridView15.RowCount){
				while(j<dataGridView13.RowCount){
					if(dataGridView13.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView15.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView13.Rows[j].Cells[2].Value=dataGridView15.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView13.RowCount){
				if(dataGridView13.Rows[i].Cells[2].FormattedValue.ToString().Length==0){
					dataGridView13.Rows[i].Cells[2].Value=0;
				}
				i++;
			}
			
			i=0;
			
			//notificados
			while(i<dataGridView16.RowCount){
				while(j<dataGridView13.RowCount){
					if(dataGridView13.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView16.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView13.Rows[j].Cells[3].Value=dataGridView16.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView13.RowCount){
				if(dataGridView13.Rows[i].Cells[3].FormattedValue.ToString().Length==0){
					dataGridView13.Rows[i].Cells[3].Value="0";
				}
				i++;
			}
			
			i=0;
			
			
			//nn
			while(i<dataGridView17.RowCount){
				while(j<dataGridView13.RowCount){
					if(dataGridView13.Rows[j].Cells[0].FormattedValue.ToString().Equals(dataGridView17.Rows[i].Cells[0].FormattedValue.ToString())){
						dataGridView13.Rows[j].Cells[4].Value=dataGridView17.Rows[i].Cells[1].FormattedValue.ToString();
					}
					j++;
				}
				j=0;
				i++;
			}
			
			i=0;
			j=0;
			
			while(i<dataGridView13.RowCount){
				if(dataGridView13.Rows[i].Cells[4].FormattedValue.ToString().Length==0){
					dataGridView13.Rows[i].Cells[4].Value="0";
				}
				i++;
			}
			
			//totales_trabajados
			i=0;
			j=0;
			
			while(i<dataGridView13.RowCount){
				tot=0;
				porcent=0;
				
				tot=Convert.ToInt32(dataGridView13.Rows[i].Cells[3].FormattedValue.ToString())+
					Convert.ToInt32(dataGridView13.Rows[i].Cells[4].FormattedValue.ToString());
					
				dataGridView13.Rows[i].Cells[5].Value=tot;
				try{
					porcent=Convert.ToInt32((tot*100)/Convert.ToInt32(dataGridView13.Rows[i].Cells[1].FormattedValue.ToString()));
				}catch(Exception esd){
					porcent=0;
				}
				dataGridView13.Rows[i].Cells[6].Value=porcent;
				i++;
			}
						
		}
		
		public void productividad_creditos(){
			String fecha_desde,fecha_hasta,des,has;
            DateTime desde, hasta;
            
                conex.conectar("base_principal");
                conex1.conectar("base_principal");
                conex2.conectar("base_principal");
                conex3.conectar("base_principal");
                conex4.conectar("base_principal");
                conex5.conectar("base_principal");
                conex14.conectar("base_principal");

                tabla_totales.Clear();
                tabla_depurados.Clear();
                tabla_entramite.Clear();
                tabla_nn.Clear();
                tabla_notificados.Clear();
                tabla_pagados.Clear();
                tabla_estrados.Clear();

            if (comboBox1.SelectedIndex > -1) //Productividad por periodo
            {
                fecha_desde = dateTimePicker1.Text.ToString();
                fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);

                fecha_hasta = dateTimePicker2.Text.ToString();
                fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

                //Totales
                tabla_totales = conex.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView1.DataSource = tabla_totales;

                //En Tramite
                tabla_entramite = conex1.consultar("SELECT notificador, COUNT(id) as Total  FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND status =\"EN TRAMITE\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView2.DataSource = tabla_entramite;

                //Notificados
                tabla_notificados = conex2.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_notificacion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (status =\"NOTIFICADO\" OR status =\"CARTERA\") AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView3.DataSource = tabla_notificados;

                //Pagados
                tabla_pagados = conex3.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE ((fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") OR fecha_recepcion is NULL) AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\"  AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView4.DataSource = tabla_pagados;

                //NN
                tabla_nn = conex4.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (nn=\"NN\" ) AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView5.DataSource = tabla_nn;

                //ESTRADOS
                tabla_estrados = conex14.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (nn=\"ESTRADOS\") AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView19.DataSource = tabla_estrados;

                //Depurados
                tabla_depurados = conex5.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND status like\"%DEPU%\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView6.DataSource = tabla_depurados;

                
            }
            else  //productividad por fechas
            {
                fecha_desde = dateTimePicker1.Text.ToString();
                fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);

                fecha_hasta = dateTimePicker2.Text.ToString();
                fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

                desde = dateTimePicker1.Value;
                desde = desde.AddDays(1);
                des = desde.ToShortDateString();
                des = des.Substring(6, 4) + "-" + des.Substring(3, 2) + "-" + des.Substring(0, 2);

                hasta = dateTimePicker2.Value;
                hasta = hasta.AddDays(1);
                has = hasta.ToShortDateString();
                has = has.Substring(6, 4) + "-" + has.Substring(3, 2) + "-" + has.Substring(0, 2);

                //MessageBox.Show("un dia mas desde: " + des + " hasta: " + has);
                //Totales
                tabla_totales = conex.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_entrega BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") GROUP BY notificador ORDER BY notificador ASC");
                dataGridView1.DataSource = tabla_totales;

                //En Tramite
                tabla_entramite = conex1.consultar("SELECT notificador, COUNT(id) as Total  FROM datos_factura WHERE (fecha_entrega BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND status =\"EN TRAMITE\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView2.DataSource = tabla_entramite;

                //Notificados
                tabla_notificados = conex2.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_notificacion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND (status =\"NOTIFICADO\" OR status =\"CARTERA\") AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView3.DataSource = tabla_notificados;

                //Pagados
                tabla_pagados = conex3.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE ((fecha_recepcion BETWEEN \"" + des + "\" AND \"" + has + "\") OR fecha_recepcion is NULL) AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView4.DataSource = tabla_pagados;

                //NN
                tabla_nn = conex4.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_recepcion BETWEEN \"" + des + "\" AND \"" + has + "\") AND (nn=\"NN\") AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView5.DataSource = tabla_nn;

                //ESTRADOS
                tabla_estrados = conex14.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_recepcion BETWEEN \"" + des + "\" AND \"" + has + "\") AND (nn=\"ESTRADOS\") AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView19.DataSource = tabla_estrados;

                //Depurados
                tabla_depurados = conex5.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_entrega BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND status like\"%DEPU%\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView6.DataSource = tabla_depurados;

            }

            llenar_dgv7();
            
            conex.cerrar();
            conex1.cerrar();
            conex2.cerrar();
            conex3.cerrar();
            conex4.cerrar();
            conex5.cerrar();    
            	
		}
		
		public void productividad_oficios(){
			String fecha_desde,fecha_hasta,des,has;
            DateTime desde, hasta;
            
                conex6.conectar("base_principal");
                conex7.conectar("base_principal");
                conex8.conectar("base_principal");
                conex9.conectar("base_principal");

                tabla_totales_o.Clear();
                tabla_entramite_o.Clear();
                tabla_notificados_o.Clear();
                tabla_nn_o.Clear();
                

           /* if (comboBox1.SelectedIndex > -1)
            {
                fecha_desde = dateTimePicker1.Text.ToString();
                fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);

                fecha_hasta = dateTimePicker2.Text.ToString();
                fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

                //Totales
                tabla_totales = conex.consultar("SELECT receptor, COUNT(id) as Total FROM oficios WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView1.DataSource = tabla_totales;

                //En Tramite
                tabla_entramite = conex1.consultar("SELECT notificador, COUNT(id) as Total  FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND status =\"EN TRAMITE\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView2.DataSource = tabla_entramite;

                //Notificados
                tabla_notificados = conex2.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_notificacion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (status =\"NOTIFICADO\" OR status =\"CARTERA\") AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView3.DataSource = tabla_notificados;

                //Pagados
                tabla_pagados = conex3.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE ((fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") OR fecha_recepcion is NULL) AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\"  AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView4.DataSource = tabla_pagados;

                //NN
                tabla_nn = conex4.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (nn=\"NN\" OR nn=\"ESTRADOS\") AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView5.DataSource = tabla_nn;

                //Depurados
                tabla_depurados = conex5.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND status like\"%DEPU%\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView6.DataSource = tabla_depurados;

                
            }
            else  //productividad por fechas
            {*/
                fecha_desde = dateTimePicker1.Text.ToString();
                fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);

                fecha_hasta = dateTimePicker2.Text.ToString();
                fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

                desde = dateTimePicker1.Value;
                desde = desde.AddDays(1);
                des = desde.ToShortDateString();
                des = des.Substring(6, 4) + "-" + des.Substring(3, 2) + "-" + des.Substring(0, 2);

                hasta = dateTimePicker2.Value;
                hasta = hasta.AddDays(1);
                has = hasta.ToShortDateString();
                has = has.Substring(6, 4) + "-" + has.Substring(3, 2) + "-" + has.Substring(0, 2);

                //MessageBox.Show("un dia mas desde: " + des + " hasta: " + has);
                //Totales
                tabla_totales_o = conex6.consultar("SELECT receptor, COUNT(id_oficios) as Total FROM oficios WHERE (fecha_recep_contro BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") GROUP BY receptor ORDER BY receptor ASC");
                dataGridView9.DataSource = tabla_totales_o;

                //En Tramite
                tabla_entramite_o = conex7.consultar("SELECT receptor, COUNT(id_oficios) as Total  FROM oficios WHERE (fecha_recep_contro BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND estatus =\"EN TRAMITE\" AND nn<>\"NN\" GROUP BY receptor ORDER BY receptor ASC");
                dataGridView10.DataSource = tabla_entramite_o;

                //Notificados
                tabla_notificados_o = conex8.consultar("SELECT receptor, COUNT(id_oficios) as Total FROM oficios WHERE (fecha_notificacion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND (estatus =\"NOTIFICADO\" OR estatus =\"DEVUELTO\") AND nn<>\"NN\" GROUP BY receptor ORDER BY receptor ASC");
                dataGridView11.DataSource = tabla_notificados_o;

                //NN
                tabla_nn_o = conex9.consultar("SELECT receptor, COUNT(id_oficios) as Total FROM oficios WHERE (fecha_visita BETWEEN \"" + des + "\" AND \"" + has + "\") AND nn=\"NN\" GROUP BY receptor ORDER BY receptor ASC");
                dataGridView12.DataSource = tabla_nn_o;

                
            //}

            llenar_dgv8();
            
            conex6.cerrar();
            conex7.cerrar();
            conex8.cerrar();
            conex9.cerrar();
            
		}
		
		public void productividad_ema(){
			String fecha_desde,fecha_hasta,des,has;
            DateTime desde, hasta;
            
                conex10.conectar("base_principal");
                conex11.conectar("base_principal");
                conex12.conectar("base_principal");
                conex13.conectar("base_principal");

                tabla_totales_e.Clear();                
                tabla_entramite_e.Clear();
                tabla_notificados_e.Clear();
                tabla_nn_e.Clear();
                
            /*if (comboBox1.SelectedIndex > -1)
            {
                fecha_desde = dateTimePicker1.Text.ToString();
                fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);

                fecha_hasta = dateTimePicker2.Text.ToString();
                fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

                //Totales
                tabla_totales = conex.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView1.DataSource = tabla_totales;

                //En Tramite
                tabla_entramite = conex1.consultar("SELECT notificador, COUNT(id) as Total  FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND status =\"EN TRAMITE\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView2.DataSource = tabla_entramite;

                //Notificados
                tabla_notificados = conex2.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_notificacion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (status =\"NOTIFICADO\" OR status =\"CARTERA\") AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView3.DataSource = tabla_notificados;

                //Pagados
                tabla_pagados = conex3.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE ((fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") OR fecha_recepcion is NULL) AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\"  AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView4.DataSource = tabla_pagados;

                //NN
                tabla_nn = conex4.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE (fecha_recepcion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND (nn=\"NN\" OR nn=\"ESTRADOS\") AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView5.DataSource = tabla_nn;

                //Depurados
                tabla_depurados = conex5.consultar("SELECT notificador, COUNT(id) as Total FROM datos_factura WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" AND status like\"%DEPU%\" AND nn<>\"NN\" AND nn<>\"ESTRADOS\" AND observaciones NOT LIKE \"%PAGADO%\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView6.DataSource = tabla_depurados;

                
            }
            else  //productividad por fechas
            {*/
                fecha_desde = dateTimePicker1.Text.ToString();
                fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);

                fecha_hasta = dateTimePicker2.Text.ToString();
                fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

                desde = dateTimePicker1.Value;
                desde = desde.AddDays(1);
                des = desde.ToShortDateString();
                des = des.Substring(6, 4) + "-" + des.Substring(3, 2) + "-" + des.Substring(0, 2);

                hasta = dateTimePicker2.Value;
                hasta = hasta.AddDays(1);
                has = hasta.ToShortDateString();
                has = has.Substring(6, 4) + "-" + has.Substring(3, 2) + "-" + has.Substring(0, 2);

                //MessageBox.Show("un dia mas desde: " + des + " hasta: " + has);
                //Totales
                tabla_totales_e = conex10.consultar("SELECT notificador, COUNT(id_ema_sepomex) as Total FROM ema_sepomex WHERE (fecha_entrega BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") GROUP BY notificador ORDER BY notificador ASC");
                dataGridView14.DataSource = tabla_totales_e;

                //En Tramite
                tabla_entramite_e = conex11.consultar("SELECT notificador, COUNT(id_ema_sepomex) as Total FROM ema_sepomex WHERE (fecha_entrega BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND status =\"EN TRAMITE\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView15.DataSource = tabla_entramite_e;

                //Notificados
                tabla_notificados_e = conex12.consultar("SELECT notificador, COUNT(id_ema_sepomex) as Total FROM ema_sepomex WHERE (fecha_notificacion BETWEEN \"" + fecha_desde + "\" AND \"" + fecha_hasta + "\") AND status =\"NOTIFICADO\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView16.DataSource = tabla_notificados_e;

                //NN
                tabla_nn_e = conex13.consultar("SELECT notificador, COUNT(id_ema_sepomex) as Total FROM ema_sepomex WHERE (fecha_recepcion BETWEEN \"" + des + "\" AND \"" + has + "\") AND status =\"NN\" GROUP BY notificador ORDER BY notificador ASC");
                dataGridView17.DataSource = tabla_nn_e;

           // }

            llenar_dgv13();
            
            conex10.cerrar();
            conex11.cerrar();
            conex12.cerrar();
            conex13.cerrar();
		}
		
		public void suma(){
			int ii=0,jj=0,entregados=0,en_tramite=0,notif=0,pagados=0,nn=0,estrados=0,total=0,pos_punto=0;
			string nombre="";
			decimal productividad;
			
			while(ii<dataGridView7.RowCount){
				dataGridView18.Rows.Add();
				dataGridView18.Rows[ii].Cells[0].Value=dataGridView7.Rows[ii].Cells[0].Value.ToString();
				ii++;
			}
			
			ii=0;
			
			while(ii<dataGridView18.RowCount){
				
				nombre=dataGridView18.Rows[ii].Cells[0].Value.ToString();
				//creditos
				while(jj<dataGridView7.RowCount){
					if(nombre.Equals(dataGridView7.Rows[jj].Cells[0].Value.ToString())){
						entregados=Convert.ToInt32(dataGridView7.Rows[jj].Cells[1].Value.ToString());
						en_tramite=Convert.ToInt32(dataGridView7.Rows[jj].Cells[2].Value.ToString());
						notif=Convert.ToInt32(dataGridView7.Rows[jj].Cells[3].Value.ToString());
						pagados=Convert.ToInt32(dataGridView7.Rows[jj].Cells[4].Value.ToString());
						nn=Convert.ToInt32(dataGridView7.Rows[jj].Cells[6].Value.ToString());
                        estrados = Convert.ToInt32(dataGridView7.Rows[jj].Cells[7].Value.ToString());
					}
					jj++;
				}
				jj=0;
				//oficios
				while(jj<dataGridView8.RowCount){
					if(nombre.ToUpper().Equals(dataGridView8.Rows[jj].Cells[0].Value.ToString())){
						entregados=entregados+Convert.ToInt32(dataGridView8.Rows[jj].Cells[1].Value.ToString());
						en_tramite=en_tramite+Convert.ToInt32(dataGridView8.Rows[jj].Cells[2].Value.ToString());
						notif=notif+Convert.ToInt32(dataGridView8.Rows[jj].Cells[3].Value.ToString());
						nn=nn+Convert.ToInt32(dataGridView8.Rows[jj].Cells[4].Value.ToString());
					}
					jj++;
				}
				jj=0;
				//propuesta
				while(jj<dataGridView13.RowCount){
					if(nombre.Equals(dataGridView13.Rows[jj].Cells[0].Value.ToString())){
						entregados=entregados+Convert.ToInt32(dataGridView13.Rows[jj].Cells[1].Value.ToString());
						en_tramite=en_tramite+Convert.ToInt32(dataGridView13.Rows[jj].Cells[2].Value.ToString());
						notif=notif+Convert.ToInt32(dataGridView13.Rows[jj].Cells[3].Value.ToString()); 
						nn=nn+Convert.ToInt32(dataGridView13.Rows[jj].Cells[4].Value.ToString());
					}
					jj++;
				}
				jj=0;
				
				dataGridView18.Rows[ii].Cells[1].Value=entregados;
				dataGridView18.Rows[ii].Cells[2].Value=en_tramite;
				dataGridView18.Rows[ii].Cells[3].Value=notif;
				dataGridView18.Rows[ii].Cells[4].Value=pagados;
				dataGridView18.Rows[ii].Cells[5].Value=nn;
                dataGridView18.Rows[ii].Cells[6].Value = estrados;
				total=notif+pagados+nn;
				dataGridView18.Rows[ii].Cells[7].Value=total;
				if(entregados<=0){
					dataGridView18.Rows[ii].Cells[8].Value=0;
				}else{
					dataGridView18.Rows[ii].Cells[8].Value=(total*100)/entregados;
				}
				if(cuenta_dias()<=0){
					productividad=0;
				}else{
					productividad=Convert.ToDecimal(Convert.ToDecimal(total)/Convert.ToDecimal(cuenta_dias()));
				}
				pos_punto=productividad.ToString().IndexOf('.');

				if(pos_punto>-1){
                    if (productividad.ToString().Length >= pos_punto + 3)
                    {
                        dataGridView18.Rows[ii].Cells[9].Value=Convert.ToDecimal(productividad.ToString().Substring(0,pos_punto+3));
                    }else{
                        if (productividad.ToString().Length >= pos_punto + 2)
                        {
                            dataGridView18.Rows[ii].Cells[9].Value = Convert.ToDecimal(productividad.ToString().Substring(0, pos_punto + 2) + "0");
                        }
                        else
                        {
                            dataGridView18.Rows[ii].Cells[9].Value = Convert.ToDecimal(productividad.ToString().Substring(0, pos_punto + 1) + "00");
                        }
                    }
				}else{
					dataGridView18.Rows[ii].Cells[9].Value=Convert.ToDecimal(productividad.ToString());
				}
				
				entregados=0;
				en_tramite=0;
				notif=0;
				pagados=0;
				nn=0;
				total=0;
		
				ii++;
			}
			
			
		}
		
		public int cuenta_dias(){
			DateTime desde,hasta,dia;
			int dias_habiles=0;
			
			desde = dateTimePicker1.Value;
			hasta = dateTimePicker2.Value;
			dia = desde;
			hasta= hasta.AddDays(1);
			while(dia<hasta){
				
				if((dia.DayOfWeek.ToString().Equals("Saturday")==true)||(dia.DayOfWeek.ToString().Equals("Sunday")==true)){
				}else{
					dias_habiles++;
				}
				dia = dia.AddDays(1);
				
				//MessageBox.Show(""+);
			}
			
			label3.Text="Dias Hábiles Productividad: "+dias_habiles;
			label3.Refresh();
			return dias_habiles;
		}
		
	    void Productividad_notiLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llenar_Cb1_todos();
	    	
	    	tabla_totales_suma.Columns.Add("Notificador");
	    	tabla_totales_suma.Columns.Add("Entregados");
	    	tabla_totales_suma.Columns.Add("En Tramite");
	    	tabla_totales_suma.Columns.Add("Notificados");
	    	tabla_totales_suma.Columns.Add("Pagados");
	    	tabla_totales_suma.Columns.Add("NN");
	    	tabla_totales_suma.Columns.Add("Total Trabajado");
	    	tabla_totales_suma.Columns.Add("Porcentaje Trabajado");
	    	tabla_totales_suma.Columns.Add("Productividad");
	    	
	    	tabla_totales_cred.Columns.Add("Notificador");
	    	tabla_totales_cred.Columns.Add("Entregados");
	    	tabla_totales_cred.Columns.Add("En Tramite");
	    	tabla_totales_cred.Columns.Add("Notificados");
	    	tabla_totales_cred.Columns.Add("Pagados");
	    	tabla_totales_cred.Columns.Add("Depurados");
	    	tabla_totales_cred.Columns.Add("NN");
	    	tabla_totales_cred.Columns.Add("Total Trabajado");
	    	tabla_totales_cred.Columns.Add("Porcentaje Trabajado");
	    	
	    	tabla_totales_ofis.Columns.Add("Notificador");
	    	tabla_totales_ofis.Columns.Add("Entregados");
	    	tabla_totales_ofis.Columns.Add("En Tramite");
	    	tabla_totales_ofis.Columns.Add("Notificados");
	    	tabla_totales_ofis.Columns.Add("NN");
	    	tabla_totales_ofis.Columns.Add("Total Trabajado");
	    	tabla_totales_ofis.Columns.Add("Porcentaje Trabajado");
	    	
	    	tabla_totales_props.Columns.Add("Notificador");
	    	tabla_totales_props.Columns.Add("Entregados");
	    	tabla_totales_props.Columns.Add("En Tramite");
	    	tabla_totales_props.Columns.Add("Notificados");
	    	tabla_totales_props.Columns.Add("NN");
	    	tabla_totales_props.Columns.Add("Total Trabajado");
	    	tabla_totales_props.Columns.Add("Porcentaje Trabajado");
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			cuenta_dias();
			
			if(checkBox1.Checked==true){
				productividad_creditos();
			}
			
			if(checkBox2.Checked==true){
				productividad_oficios();
			}
			
			if(checkBox3.Checked==true){
				productividad_ema();
			}
			
			dataGridView18.Rows.Clear();
			suma();
		}

        private void label2_Click(object sender, EventArgs e)
        {

        }
		
		void Button1Click(object sender, EventArgs e)
		{
			fichero.Title = "Guardar archivo de Excel";
			fichero.Filter = "Archivo Excel (*.XLSX)|*.xlsx";
			
			if(fichero.ShowDialog() == DialogResult.OK){
				try{
					int x=0;
					//suma
					while(x<dataGridView18.RowCount){
						tabla_totales_suma.Rows.Add(dataGridView18.Rows[x].Cells[0].Value,
						                            dataGridView18.Rows[x].Cells[1].Value,
						                            dataGridView18.Rows[x].Cells[2].Value,
						                            dataGridView18.Rows[x].Cells[3].Value,
						                            dataGridView18.Rows[x].Cells[4].Value,
						                            dataGridView18.Rows[x].Cells[5].Value,
						                            dataGridView18.Rows[x].Cells[6].Value,
						                            dataGridView18.Rows[x].Cells[7].Value,
						                            dataGridView18.Rows[x].Cells[8].Value);
						x++;
					}
					x=0;
					//creds
					while(x<dataGridView7.RowCount){
						tabla_totales_cred.Rows.Add(dataGridView7.Rows[x].Cells[0].Value,
						                            dataGridView7.Rows[x].Cells[1].Value,
						                            dataGridView7.Rows[x].Cells[2].Value,
						                            dataGridView7.Rows[x].Cells[3].Value,
						                            dataGridView7.Rows[x].Cells[4].Value,
						                            dataGridView7.Rows[x].Cells[5].Value,
						                            dataGridView7.Rows[x].Cells[6].Value,
						                            dataGridView7.Rows[x].Cells[7].Value,
						                            dataGridView7.Rows[x].Cells[8].Value);
						x++;
					}
					x=0;
					//oficios
					while(x<dataGridView8.RowCount){
						tabla_totales_ofis.Rows.Add(dataGridView8.Rows[x].Cells[0].Value,
						                            dataGridView8.Rows[x].Cells[1].Value,
						                            dataGridView8.Rows[x].Cells[2].Value,
						                            dataGridView8.Rows[x].Cells[3].Value,
						                            dataGridView8.Rows[x].Cells[4].Value,
						                            dataGridView8.Rows[x].Cells[5].Value,
						                            dataGridView8.Rows[x].Cells[6].Value);
						x++;
					}
					x=0;
					//propuesta
					while(x<dataGridView13.RowCount){
						tabla_totales_props.Rows.Add(dataGridView13.Rows[x].Cells[0].Value,
						                            dataGridView13.Rows[x].Cells[1].Value,
						                            dataGridView13.Rows[x].Cells[2].Value,
						                            dataGridView13.Rows[x].Cells[3].Value,
						                            dataGridView13.Rows[x].Cells[4].Value,
						                            dataGridView13.Rows[x].Cells[5].Value,
						                            dataGridView13.Rows[x].Cells[6].Value);
						x++;
					}
					x=0;
					
					XLWorkbook wb = new XLWorkbook();	
					wb.Worksheets.Add(tabla_totales_suma,"Totales");
					wb.Worksheets.Add(tabla_totales_cred,"Creditos");
					wb.Worksheets.Add(tabla_totales_ofis,"Oficios");
					wb.Worksheets.Add(tabla_totales_props,"Propuesta-Pago Oportuno");
					wb.SaveAs(fichero.FileName);
					MessageBox.Show("Se Ha creado correctamente el archivo:\n"+fichero.FileName,"Exito");
				}catch(Exception es){
					MessageBox.Show("Ha ocurrido un error al intentar crear el archivo:\n"+fichero.FileName+"\n\n"+es,"ERROR");
				}
			}
		}

        private void button2_Click(object sender, EventArgs e)
        {
            detalle_prod_ind d_noti = new detalle_prod_ind();
            d_noti.Show();
            d_noti.Focus();
        }
	}
}

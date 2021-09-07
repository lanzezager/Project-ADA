using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear
{
    public partial class Visor_reporte_mensual : Form
    {
        public Visor_reporte_mensual()
        {
            InitializeComponent();
        }

        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();
        Depuracion.Formatos_Reporte.Reporte_mensual rep_men = new Depuracion.Formatos_Reporte.Reporte_mensual();

        int i = 0,x=0,j=0;

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();

        //periodos
        public void llenar_Cb1()
        {
            conex.conectar("base_principal");
            comboBox1.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
            do
            {
                comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            conex.cerrar();
        }
        
        public void dame_datos2()
        {
            x = 0;
           
            do
            {
                Depuracion.DataSet_Depu.reporte_periodosRow fila = depu.reporte_periodos.Newreporte_periodosRow();
                
                fila.cifra_cont = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.fecha_not_desde_cifra = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.fecha_not_hasta_cifra = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.tipo_documento = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.fecha_recepcion = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.fecha_depu_desde = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.fecha_depu_hasta = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.fecha_firma_desde= dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.fecha_firma_hasta = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();
                fila.fecha_not_desde= dataGridView1.Rows[x].Cells[9].FormattedValue.ToString();
                fila.fecha_not_hasta = dataGridView1.Rows[x].Cells[10].FormattedValue.ToString();
                fila.recibidos = dataGridView1.Rows[x].Cells[11].FormattedValue.ToString();
                fila.notificados = dataGridView1.Rows[x].Cells[12].FormattedValue.ToString();
                fila.no_localizados = dataGridView1.Rows[x].Cells[13].FormattedValue.ToString();
                fila.cancelados_dep = dataGridView1.Rows[x].Cells[14].FormattedValue.ToString();
                fila.otros = dataGridView1.Rows[x].Cells[15].FormattedValue.ToString();

                depu.reporte_periodos.Addreporte_periodosRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } while (x < dataGridView1.RowCount);
        }

        public void llena_grid()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("cifra_control", "cifra_control");                    //0
            dataGridView1.Columns.Add("fecha_noti_ini_cifra", "fecha_noti_ini_cifra");      //1
            dataGridView1.Columns.Add("fecha_noti_term_cifra", "fecha_noti_term_cifra");    //2
            dataGridView1.Columns.Add("tipo_doc", "tipo_doc");                              //3
            dataGridView1.Columns.Add("fecha_recep", "fecha_recep");                        //4
            dataGridView1.Columns.Add("fecha_depu_ini", "fecha_depu_ini");                  //5
            dataGridView1.Columns.Add("fecha_depu_term", "fecha_depu_term");                //6
            dataGridView1.Columns.Add("fecha_firma_ini", "fecha_firma_ini");                //7
            dataGridView1.Columns.Add("fecha_firma_term", "fecha_firma_term");              //8
            dataGridView1.Columns.Add("fecha_noti_ini", "fecha_noti_ini");                  //9
            dataGridView1.Columns.Add("fecha_noti_term", "fecha_noti_term");                //10
            dataGridView1.Columns.Add("recibidos", "recibidos");                            //11
            dataGridView1.Columns.Add("notificados", "notificados");                        //12
            dataGridView1.Columns.Add("nl", "nl");                                          //13
            dataGridView1.Columns.Add("canc_dep", "canc_dep");                              //14
            dataGridView1.Columns.Add("otros", "otros");                                    //15

            i = 0;
            conex.conectar("base_principal");
            do{
                dataGridView2.DataSource= conex.consultar("SELECT cifra_control,fecha_cifra_control_inicio"+
                    ",fecha_cifra_control_termino,fecha_recibo_producto,fecha_depuracion_inicio,fecha_depuracion_termino,fecha_firma_inicio,fecha_firma_termino"+
                " FROM estado_periodos WHERE nombre_periodo =\""+comboBox1.Items[i].ToString()+"\"");
                dataGridView1.Rows.Add();
                if (dataGridView2.RowCount > 0)
                {
                  //  if (dataGridView2.Rows[0].Cells[0].Value.ToString().Length == 0)
                    //{
                        dataGridView1.Rows[i].Cells[0].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    //}
                    //else
                    //{
                      //  dataGridView1.Rows[0].Cells[j].Value = "-";
                    //}

                    dataGridView1.Rows[i].Cells[1].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();
                    dataGridView1.Rows[i].Cells[2].Value = dataGridView2.Rows[0].Cells[2].FormattedValue.ToString();
                    dataGridView1.Rows[i].Cells[3].Value = comboBox1.Items[i].ToString();
                    dataGridView1.Rows[i].Cells[4].Value = dataGridView2.Rows[0].Cells[3].FormattedValue.ToString();
                    dataGridView1.Rows[i].Cells[5].Value = dataGridView2.Rows[0].Cells[4].FormattedValue.ToString();
                    dataGridView1.Rows[i].Cells[6].Value = dataGridView2.Rows[0].Cells[5].FormattedValue.ToString();
                    dataGridView1.Rows[i].Cells[7].Value = dataGridView2.Rows[0].Cells[6].FormattedValue.ToString();
                    dataGridView1.Rows[i].Cells[8].Value = dataGridView2.Rows[0].Cells[7].FormattedValue.ToString();

                    j = 0;
                    do
                    {
                        //MessageBox.Show("|"+dataGridView2.Rows[0].Cells[j].Value.ToString().Length+"|");
                        if (dataGridView2.Rows[0].Cells[j].Value == null || dataGridView2.Rows[0].Cells[j].Value.ToString().Length==0)
                        {
                            if (j <= 2)
                            {
                                //MessageBox.Show("1|" + dataGridView2.Rows[0].Cells[j].Value.ToString().Length + "|");
                                dataGridView1.Rows[i].Cells[j].Value = "-";
                            }
                            else
                            {
                                if(j>=3){
                                    dataGridView1.Rows[i].Cells[j+1].Value = "-";
                                    //MessageBox.Show("2|" + dataGridView2.Rows[0].Cells[j].Value.ToString().Length + "|");
                                }
                            }
                        }
                        j++;
                    } while (j < dataGridView2.ColumnCount);

                    
                    
                }
                else
                {
                    
                    dataGridView1.Rows[i].Cells[0].Value = "-";
                    dataGridView1.Rows[i].Cells[1].Value = "-";
                    dataGridView1.Rows[i].Cells[2].Value = "-";
                    dataGridView1.Rows[i].Cells[3].Value = comboBox1.Items[i].ToString();
                    dataGridView1.Rows[i].Cells[4].Value = "-";
                    dataGridView1.Rows[i].Cells[5].Value = "-";
                    dataGridView1.Rows[i].Cells[6].Value = "-";
                    dataGridView1.Rows[i].Cells[7].Value = "-";
                    dataGridView1.Rows[i].Cells[8].Value = "-";
                }

                dataGridView2.DataSource = conex.consultar("SELECT MIN(fecha_notificacion) FROM datos_factura WHERE nombre_periodo =\""+comboBox1.Items[i].ToString()+"\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null || dataGridView2.Rows[0].Cells[0].Value.ToString().Length==0)
                {
                        dataGridView1.Rows[i].Cells[9].Value = "-";
                    }else{
                        dataGridView1.Rows[i].Cells[9].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }
                
                
                dataGridView2.DataSource = conex.consultar("SELECT MAX(fecha_notificacion) FROM datos_factura WHERE nombre_periodo =\"" + comboBox1.Items[i].ToString() + "\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null || dataGridView2.Rows[0].Cells[0].Value.ToString().Length == 0)
                    {
                        dataGridView1.Rows[i].Cells[10].Value = "-";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[10].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }

                dataGridView2.DataSource = conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo =\"" + comboBox1.Items[i].ToString() + "\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[11].Value = "-";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[11].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }
                
                dataGridView2.DataSource = conex.consultar("SELECT COUNT(status) FROM datos_factura WHERE nombre_periodo =\"" + comboBox1.Items[i].ToString() + "\" AND (status = \"NOTIFICADO\" OR status = \"CARTERA\") AND nn <> \"NN\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[12].Value = "-";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[12].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }
                
                dataGridView2.DataSource = conex.consultar("SELECT COUNT(nn) FROM datos_factura WHERE nombre_periodo =\"" + comboBox1.Items[i].ToString() + "\" AND nn=\"NN\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[13].Value = "-";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[13].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }

                dataGridView2.DataSource = conex.consultar("SELECT COUNT(status) FROM datos_factura WHERE nombre_periodo =\"" + comboBox1.Items[i].ToString() + "\" AND status LIKE\"DEPU%\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[14].Value = "-";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[14].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }
                
                dataGridView2.DataSource = conex.consultar("SELECT COUNT(status) FROM datos_factura WHERE nombre_periodo =\"" + comboBox1.Items[i].ToString() + "\" AND status NOT LIKE\"DEPU%\" AND status <>\"NOTIFICADO\" AND status <>\"CARTERA\" AND nn <> \"NN\"");
                if (dataGridView2.Rows[0].Cells[0].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[15].Value = "-";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[15].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                    }
               
                i++;
            }while(i<comboBox1.Items.Count);
            //dataGridView1.Columns.Add("observaciones", "observaciones");
        }

        public void reporte_mensual()
        {
            rep_men.SetDataSource(depu);
            //MessageBox.Show(tipo_per);


            crystalReportViewer1.ReportSource = rep_men;
            //crystalReportViewer1.RefreshReport();
            //fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + tipo_per + "\\" + noti + ".pdf");
            MessageBox.Show("El Reporte se Ha Generado Correctamente","¡LISTO!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);

        }

        private void Visor_reporte_mensual_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            this.Hide();
            llenar_Cb1();
            llena_grid();
            dame_datos2();
            reporte_mensual();
            this.Show();
        }
    }
}

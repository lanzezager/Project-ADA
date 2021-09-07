using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear
{
    public partial class Visor_reporte_cartera : Form 
    {
        public Visor_reporte_cartera()
        {
            InitializeComponent();
        }

        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();

        Depuracion.Formatos_Reporte.Entrega_cartera cartera = new Depuracion.Formatos_Reporte.Entrega_cartera();
        Depuracion.Formatos_Reporte.Entrega_cartera_estrados cartera_estrados = new Depuracion.Formatos_Reporte.Entrega_cartera_estrados();

        String tipo_entrega, nombre_per, noti,recep;
        int x = 0, tipo_report = 0, y = 0;
        double sumatoria = 0;

        public void envio3(DataTable tabla_report,String tipo_ent,String nom_per,String tipo_receptor)
        {
            tabla = tabla_report;
            dataGridView1.DataSource = tabla;
            this.tipo_entrega = tipo_ent;
            this.nombre_per = nom_per;
            this.recep = tipo_receptor;
            if (tabla.Rows.Count > 0)
            {
                dame_datos3();
            }
            else
            {
                MessageBox.Show("Este Periodo está vacio");
                this.Hide();
            }
        }

        public void envio_estrados(DataTable tabla_report, String tipo_ent, String nom_per, String tipo_receptor)
        {
            tabla = tabla_report;
            dataGridView1.DataSource = tabla;
            this.tipo_entrega = tipo_ent;
            this.nombre_per = nom_per;
            this.recep = tipo_receptor;
            if (tabla.Rows.Count > 0)
            {
                dame_datos_estrados();
            }
            else
            {
                MessageBox.Show("Este Periodo está vacio");
                this.Hide();
            }
        }

        public void dame_datos3()
        {
            x = 0;
           
            sumatoria = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            noti = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
            do
            {
                Depuracion.DataSet_Depu.carteraRow fila = depu.cartera.NewcarteraRow();

                fila.id = (x+1).ToString();
                fila.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.razon_social = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.credito_cuo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.credito_mul = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.imp_cuo = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.imp_mul = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.td = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                //fila.nom_per = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();

                depu.cartera.AddcarteraRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } while (x < tabla.Rows.Count);
        }

        public void dame_datos_estrados()
        {
            x = 0;

            sumatoria = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            noti = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
            do
            {
                Depuracion.DataSet_Depu.cartera_estradosRow fila = depu.cartera_estrados.Newcartera_estradosRow();

                fila.id = (x + 1).ToString();
                fila.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.razon_social = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.credito_cuo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.credito_mul = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.imp_cuo = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.imp_mul = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.td = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.folio_estrado = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();

                depu.cartera_estrados.Addcartera_estradosRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } while (x < tabla.Rows.Count);
        }

        public void reporte_entrega_cartera()
        {
            //this.Text = "Nova Gear: Reporte de Prefactura de Notificadores ";
            cartera.SetDataSource(depu);
            cartera.SetParameterValue("tipo_entrega", tipo_entrega);
            cartera.SetParameterValue("nombre_periodo", nombre_per);
            cartera.SetParameterValue("tipo_receptor", recep);
            crystalReportViewer1.ReportSource = cartera;
            //crystalReportViewer1.RefreshReport();
            //MessageBox.Show(""+y);
            //fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + tipo_per + "\\" + noti + ".pdf");
            //this.Close();
        }

        public void reporte_entrega_cartera_estrados()
        {
            //this.Text = "Nova Gear: Reporte de Prefactura de Notificadores ";
            cartera_estrados.SetDataSource(depu);
            cartera_estrados.SetParameterValue("tipo_entrega", tipo_entrega);
            cartera_estrados.SetParameterValue("nombre_periodo", nombre_per);
            cartera_estrados.SetParameterValue("tipo_receptor", recep);
            crystalReportViewer1.ReportSource = cartera_estrados;
            //crystalReportViewer1.RefreshReport();
            //MessageBox.Show(""+y);
            //fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + tipo_per + "\\" + noti + ".pdf");
            //this.Close();
        }

        private void Visor_reporte_cartera_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            if(MainForm.cartera_estrados==0){
                reporte_entrega_cartera();
            }else{
                reporte_entrega_cartera_estrados();
            }
        }
    }
}

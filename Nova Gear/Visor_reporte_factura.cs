using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Microsoft.Reporting.WinForms;
//using Microsoft.ReportingServices;

namespace Nova_Gear
{
    public partial class Visor_reporte_factura : Form
    {
        public Visor_reporte_factura()
        {
            InitializeComponent();
            
        }

        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();
        DataTable tabla_cifra = new DataTable();
        Depuracion.Formatos_Reporte.Depu_report depu_rep = new Depuracion.Formatos_Reporte.Depu_report();
        Depuracion.Formatos_Reporte.Factura_Notificador fact_not = new Depuracion.Formatos_Reporte.Factura_Notificador();
        Depuracion.Formatos_Reporte.reporte_sector00 sect00 = new Depuracion.Formatos_Reporte.reporte_sector00();
        Depuracion.Formatos_Reporte.Prefactura prefact = new Depuracion.Formatos_Reporte.Prefactura();

        Conexion conex = new Conexion();

        String tipo_per, importe_total,noti,cifra,fech;
        int x = 0, tipo_report = 0;
        double sumatoria = 0;

        public void dame_datos2()
        {
            x = 0;
            sumatoria = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            noti = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();

            do
            {
                Depuracion.DataSet_Depu.factura_notRow fila = depu.factura_not.Newfactura_notRow();
                fila.num = (x + 1).ToString();
                fila.reg_pat_nom = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.credito_cuota = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.tipo_doc = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.sector = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.importe = Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                fila.notificador = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.controlador = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.nom_per = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();
                fila.nn = dataGridView1.Rows[x].Cells[9].FormattedValue.ToString();
                fila.sector_original = dataGridView1.Rows[x].Cells[10].FormattedValue.ToString();
                fila.cifra = dataGridView1.Rows[x].Cells[11].FormattedValue.ToString();
                fila.fecha_cifra = dataGridView1.Rows[x].Cells[12].FormattedValue.ToString();
                fila.factu_fecha = dataGridView1.Rows[x].Cells[13].FormattedValue.ToString();
                fila.dele = dataGridView1.Rows[x].Cells[14].FormattedValue.ToString().ToUpper();
                fila.subdele = dataGridView1.Rows[x].Cells[15].FormattedValue.ToString().ToUpper();
                fila.jefe_secc_epo = dataGridView1.Rows[x].Cells[16].FormattedValue.ToString().ToUpper();
                fila.estatus = dataGridView1.Rows[x].Cells[17].FormattedValue.ToString().ToUpper();
                fila.fecha_not = dataGridView1.Rows[x].Cells[18].FormattedValue.ToString().ToUpper();
                depu.factura_not.Addfactura_notRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } while (x < tabla.Rows.Count);
        }

        public void envio2(DataTable tabla_report, String tipo)
        {
            this.tipo_per = tipo;
            tabla = tabla_report;
            dataGridView1.DataSource = tabla;

            if (tabla.Rows.Count > 0)
            {
                dame_datos2();
            }
            else
            {
                MessageBox.Show("Este Periodo está vacio");
                this.Hide();
            }
        }

        public void reporte_factura_noti()
        {
            fact_not.SetDataSource(depu);
            //MessageBox.Show(tipo_per);
           
            crystalReportViewer1.ReportSource = fact_not;

            crystalReportViewer1.RefreshReport();
            fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + tipo_per + "\\" + noti + ".pdf");
            this.Close();

        }

        public void cifra_control()
        {
            conex.conectar("base_principal");
            tabla_cifra = conex.consultar("SELECT cifra_control,fecha_cifra_control_inicio,fecha_cifra_control_termino FROM estado_periodos WHERE nombre_periodo=\""+tipo_per+"\"");
            if(tabla_cifra.Rows.Count > 0){
                cifra=tabla_cifra.Rows[0][0].ToString();
                fech = tabla_cifra.Rows[0][1].ToString() + "-" + tabla_cifra.Rows[0][2].ToString();
            }
        }

        private void Visor_reporte_factura_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
            
            this.Visible = false;
            reporte_factura_noti();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class Visor_Prefactura : Form
    {
        public Visor_Prefactura()
        {
            InitializeComponent();
        }

        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();

        Depuracion.Formatos_Reporte.Prefactura prefact = new Depuracion.Formatos_Reporte.Prefactura();

        String tipo_per, importe_total, noti;
        int x = 0, tipo_report = 0, y = 0;
        double sumatoria = 0;

        public void envio3(DataTable tabla_report)
        {
            tabla = tabla_report;
            dataGridView1.DataSource = tabla;
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

        public void dame_datos3()
        {
            x = 0;
            y = 1;
            sumatoria = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            noti = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
            do
            {
                Depuracion.DataSet_Depu.prefacturaRow fila = depu.prefactura.NewprefacturaRow();

                fila.id = (y).ToString();
                fila.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.razon_social = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.sector = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.notificador = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.domicilio = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.localidad = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.nombre_per = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                //fila.nom_per = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();

                depu.prefactura.AddprefacturaRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                y++;

                if (x < (tabla.Rows.Count - 1))
                {
                    if (!(dataGridView1.Rows[x].Cells[4].FormattedValue.ToString().Equals(dataGridView1.Rows[x + 1].Cells[4].FormattedValue.ToString())))
                    {
                        y = 1;
                    }
                }

                x++;
            } while (x < tabla.Rows.Count);
        }

        public void reporte_prefactura()
        {
            this.Text = "Nova Gear: Reporte de Prefactura de Notificadores ";

            prefact.SetDataSource(depu);
            crystalReportViewer1.ReportSource = prefact;
            crystalReportViewer1.RefreshReport();
            //MessageBox.Show(""+y);
            //fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + tipo_per + "\\" + noti + ".pdf");
            //this.Close();
        }

        private void Visor_Prefactura_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            reporte_prefactura();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Nova_Gear.Oficios
{
    public partial class Visor_oficios_factura : Form
    {
        public Visor_oficios_factura()
        {
            InitializeComponent();
        }
        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();

        Depuracion.Formatos_Reporte.Oficios_factura ofi_fact = new Depuracion.Formatos_Reporte.Oficios_factura();

        String carpeta,nombre_per,not,contro,nombre_fact,doc,mensaje_factur;
        int x = 0;

        Conexion conex = new Conexion();
        public void dame_datos2()
        {
            x = 0;
            
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            
            do
            {
                Depuracion.DataSet_Depu.oficios_facturaRow fila = depu.oficios_factura.Newoficios_facturaRow();
               
                fila.num = (x + 1).ToString();
                fila.reg_pat_nss = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.nombre = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.folio = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.acuerdo = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.emite = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.fecha_of = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.fecha_notificacion = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.fecha_visita = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();

                depu.oficios_factura.Addoficios_facturaRow(fila);
              
                x++;
            } while (x < tabla.Rows.Count);
        }

        public void envio(DataTable tabla_report, String nom_fact, String nom_per, String noti, String cont, String carp, String arch,String mensaje)
        {
            this.nombre_fact = nom_fact;
            tabla = tabla_report;
            dataGridView1.DataSource = tabla;
            nombre_per = nom_per;
            not = noti;
            contro = cont;
            carpeta = carp;
            doc = arch;
            mensaje_factur = mensaje;

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
            ofi_fact.SetDataSource(depu);
            crystalReportViewer1.ReportSource = ofi_fact;
            //MessageBox.Show(nombre_per + " " + nombre_fact + " " + not + " " + contro);
            ofi_fact.SetParameterValue("nombre_periodo", nombre_per);
            ofi_fact.SetParameterValue("tipo_factura", nombre_fact);
            ofi_fact.SetParameterValue("notificador", not);
            ofi_fact.SetParameterValue("controlador", contro);
            ofi_fact.SetParameterValue("leyenda", mensaje_factur);
            ofi_fact.SetParameterValue("delegacion",conex.leer_config_sub()[0].ToUpper());
            ofi_fact.SetParameterValue("subdelegacion", conex.leer_config_sub()[3].ToUpper());

            try
            {
                //crystalReportViewer1.RefreshReport();
                ofi_fact.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + carpeta + "\\" + doc + ".pdf");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ocurrió un error al momento de generar la factura","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void Visor_oficios_factura_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            reporte_factura_noti();
        }
    }
}

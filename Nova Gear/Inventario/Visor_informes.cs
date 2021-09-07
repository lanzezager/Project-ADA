using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Nova_Gear.Inventario
{
    public partial class Visor_informes : Form
    {
        public Visor_informes()
        {
            InitializeComponent();
        }
        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();
        String sub,del;

        Formatos.informe_dia informe_dia = new Formatos.informe_dia();
        Formatos.Resumen_creditos_rev res_cred_rev = new Formatos.Resumen_creditos_rev();
        Formatos.Resumen_general res_gral = new Formatos.Resumen_general();


        public void leer_config()
        {
            String del1, del_num, mpio, subdele, sub_num, jefe_cob, jefe_emi, jefe_afi, ref_baja, ref_ofi;
            try
            {
                StreamReader rdr = new StreamReader(@"sub_config.lz");

                del = rdr.ReadLine();
                del_num = rdr.ReadLine();
                mpio = rdr.ReadLine();
                sub = rdr.ReadLine();
                sub_num = rdr.ReadLine();
                rdr.Close();

                del = del.Substring(11, del.Length - 11);
                del_num = del_num.Substring(7, del_num.Length - 7);
                mpio = mpio.Substring(10, mpio.Length - 10);
                sub = sub.Substring(14, sub.Length - 14);
                sub_num = sub_num.Substring(7, sub_num.Length - 7);
            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL: \n"+error, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void envio_inf_dia(DataTable tabla,  String tipo_inf)
        {
            int x = 0;
            leer_config();

            while(x<tabla.Rows.Count){
                Depuracion.DataSet_Depu.avance_por_diaRow fila = depu.avance_por_dia.Newavance_por_diaRow();
                
                fila.dia = tabla.Rows[x][0].ToString();
                fila.fecha = tabla.Rows[x][1].ToString();
                fila.casos = tabla.Rows[x][2].ToString();
                fila.importe = tabla.Rows[x][3].ToString();
                fila.porcentaje = tabla.Rows[x][4].ToString();
               
                depu.avance_por_dia.Addavance_por_diaRow(fila);
                
                x++;
            }

            informe_dia.SetDataSource(depu);
            crystalReportViewer1.ReportSource = informe_dia;
            informe_dia.SetParameterValue("subdele", sub.ToUpper());
            informe_dia.SetParameterValue("dele", del.ToUpper());
            informe_dia.SetParameterValue("tipo_inf", tipo_inf);

        }

        public void envio_cred_rev(DataTable tabla, String tipo, String fecha, String tipo_inf)
        {
            int x = 0;
            leer_config();

            while (x < tabla.Rows.Count)
            {
                Depuracion.DataSet_Depu.resumen_revisadosRow fila = depu.resumen_revisados.Newresumen_revisadosRow();

                fila.descripcion= tabla.Rows[x][0].ToString();
                fila.numero = tabla.Rows[x][1].ToString();
                fila.coincidencia_total = tabla.Rows[x][2].ToString();
                fila.coincidencia_parcial = tabla.Rows[x][3].ToString();
                fila.por_investigar = tabla.Rows[x][4].ToString();
                fila.total = tabla.Rows[x][5].ToString();
                fila.de = tabla.Rows[x][6].ToString();
                fila.faltan = tabla.Rows[x][7].ToString();

                depu.resumen_revisados.Addresumen_revisadosRow(fila);

                x++;
            }

            res_cred_rev.SetDataSource(depu);
            crystalReportViewer1.ReportSource = res_cred_rev;
            res_cred_rev.SetParameterValue("subdele", sub.ToUpper());
            res_cred_rev.SetParameterValue("fecha", fecha);
            res_cred_rev.SetParameterValue("tipo", tipo);
            res_cred_rev.SetParameterValue("tipo_inf", tipo_inf);
            res_cred_rev.SetParameterValue("dele", del.ToUpper());
        }

        public void envio_res_gral(DataTable tabla, String fecha, String tipo_inf)
        {
            int x = 0;
            leer_config();

            while (x < tabla.Rows.Count)
            {
                Depuracion.DataSet_Depu.resumen_gral_invRow fila = depu.resumen_gral_inv.Newresumen_gral_invRow();

                fila.no_libro = tabla.Rows[x][0].ToString();
                fila.responsable = tabla.Rows[x][1].ToString();
                fila.porcentaje_asignado = tabla.Rows[x][2].ToString();
                fila.creditos_asignados = tabla.Rows[x][3].ToString();
                fila.coincidencia_total = tabla.Rows[x][4].ToString();
                fila.coincidencia_parcial = tabla.Rows[x][5].ToString();
                fila.por_investigar = tabla.Rows[x][6].ToString();
                fila.total = tabla.Rows[x][7].ToString();
                fila.porcentaje_avance = tabla.Rows[x][8].ToString();
                fila.faltantes = tabla.Rows[x][9].ToString();
                fila.importe__revisado = tabla.Rows[x][10].ToString();
                fila.importe_total = tabla.Rows[x][11].ToString();

                depu.resumen_gral_inv.Addresumen_gral_invRow(fila);

                x++;
            }

            res_gral.SetDataSource(depu);
            crystalReportViewer1.ReportSource = res_gral;
            res_gral.SetParameterValue("subdele", sub.ToUpper());
            res_gral.SetParameterValue("fecha", fecha);
            res_gral.SetParameterValue("tipo_inf", tipo_inf);
            res_gral.SetParameterValue("dele", del.ToUpper());

        }

        private void Visor_informes_Load(object sender, EventArgs e)
        {

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


        }
    }
}

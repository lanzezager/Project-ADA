using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace Nova_Gear.Mod40
{
    public partial class Visor_oficios40 : Form
    {
        public Visor_oficios40()
        {
            InitializeComponent();
        }

        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();

        Depuracion.Formatos_Reporte.Mod40_reporte_oficial mod40_ofi = new Depuracion.Formatos_Reporte.Mod40_reporte_oficial();
        Depuracion.Formatos_Reporte.Mod40_reporte_informal mod40_inf = new Depuracion.Formatos_Reporte.Mod40_reporte_informal();
        Depuracion.Formatos_Reporte.Mod40_reporte_baja mod40_baja = new Depuracion.Formatos_Reporte.Mod40_reporte_baja();
        Depuracion.Formatos_Reporte.Mod40_reporte_baja_un_per mod40_un_per = new Depuracion.Formatos_Reporte.Mod40_reporte_baja_un_per();
        
        String nss_prev, del, del_num, mpio, sub_1, sub_num, subdele, jefe_cob, jefe_emi, jefe_afi, ref_baja, ref_ofi,fecha_formal,fecha;
        string[] datos_ofi;
        Conexion conex = new Conexion();

        public void leer_config()
        {
            try
            {
               /* StreamReader rdr = new StreamReader(@"mod40_info_config.lz");

                del = rdr.ReadLine();
                del_num = rdr.ReadLine();
                mpio = rdr.ReadLine();
                sub_1 = rdr.ReadLine();
                sub_num = rdr.ReadLine();
                subdele = rdr.ReadLine();
                jefe_cob = rdr.ReadLine();
                jefe_emi = rdr.ReadLine();
                jefe_afi = rdr.ReadLine();
                ref_baja = rdr.ReadLine();
                ref_ofi = rdr.ReadLine();
                rdr.Close();

                del = del.Substring(11, del.Length - 11);
                del_num = del_num.Substring(15, del_num.Length - 15);
                mpio = mpio.Substring(10, mpio.Length - 10);
                sub_1 = sub_1.Substring(14, sub_1.Length - 14);
                sub_num = sub_num.Substring(18, sub_num.Length - 18);
                subdele = subdele.Substring(12, subdele.Length - 12);
                jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
                jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
                jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);
                ref_baja = ref_baja.Substring(9, ref_baja.Length - 9);
                ref_ofi = ref_ofi.Substring(8, ref_ofi.Length - 8);*/

                sub_1=conex.leer_config_sub()[3];
                jefe_cob=conex.leer_config_sub()[7];
                jefe_emi=conex.leer_config_sub()[8];
                jefe_afi=conex.leer_config_sub()[6]; 
                subdele=conex.leer_config_sub()[5];
                mpio=conex.leer_config_sub()[2];
                del=conex.leer_config_sub()[0];
                
                sub_1 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sub_1.ToLower());
                jefe_cob = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(jefe_cob.ToLower());
                jefe_emi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(jefe_emi.ToLower());
                jefe_afi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(jefe_afi.ToLower());
                subdele = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subdele.ToLower());
            }
            catch (Exception error)
            {
                //MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void formato_fecha()
        {
            fecha_formal = System.DateTime.Today.ToShortDateString();
            fecha = fecha_formal;

            if (fecha.Substring(3, 2).Equals("01"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Enero de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("02"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Febrero de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("03"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Marzo de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("04"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Abril de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("05"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Mayo de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("06"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Junio de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("07"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Julio de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("08"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Agosto de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("09"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Septiembre de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("10"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Octubre de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("11"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Noviembre de " + fecha.Substring(6, 4);
            }

            if (fecha.Substring(3, 2).Equals("12"))
            {
                fecha_formal = fecha.Substring(0, 2) + " de Diciembre de " + fecha.Substring(6, 4);
            }

            fecha_formal = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mpio.ToLower()) + ", " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(del.ToLower()) + " a  " + fecha_formal;
        }

        public void recibir_tabla(DataTable table, String nom_trab, String nss, String reg_pat, String fecha_ini, String fecha_fin, String sub,int resu_tipo_r, String user )
        {
            int x = 0;
            dataGridView1.DataSource = table;

            do
            {
                Depuracion.DataSet_Depu.Mod40Row fila = depu.Mod40.NewMod40Row();
                
                fila.anio= dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.ene = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.feb = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.mar = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.abr = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.may = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.jun = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.jul = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.ago = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();
                fila.sep = dataGridView1.Rows[x].Cells[9].FormattedValue.ToString();
                fila.oct = dataGridView1.Rows[x].Cells[10].FormattedValue.ToString();
                fila.nov = dataGridView1.Rows[x].Cells[11].FormattedValue.ToString();
                fila.dic = dataGridView1.Rows[x].Cells[12].FormattedValue.ToString();

                depu.Mod40.AddMod40Row(fila);
                x++;
            } while (x <dataGridView1.RowCount);

            if (resu_tipo_r == 1)
            {
                Datos_inf40 dats40 = new Datos_inf40();
                dats40.leer_config(1);
                dats40.ShowDialog();
                datos_ofi = dats40.regresar();
                leer_config();
                formato_fecha();

                mod40_ofi.SetDataSource(depu);
                mod40_ofi.SetParameterValue("nombre_asegurado", nom_trab);
                mod40_ofi.SetParameterValue("nss", nss);
                mod40_ofi.SetParameterValue("reg_pat", reg_pat);
                mod40_ofi.SetParameterValue("sub", sub);
                mod40_ofi.SetParameterValue("fecha_ini", fecha_ini);
                mod40_ofi.SetParameterValue("fecha_fin", fecha_fin);
                mod40_ofi.SetParameterValue("fecha_escrito",datos_ofi[0]);
                mod40_ofi.SetParameterValue("titular_nombre", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(datos_ofi[1].ToLower()));
                mod40_ofi.SetParameterValue("titular_puesto", datos_ofi[2]);
                mod40_ofi.SetParameterValue("fecha_formal", fecha_formal);
                mod40_ofi.SetParameterValue("n_o", datos_ofi[3]);
                mod40_ofi.SetParameterValue("dele", del.ToUpper());
                mod40_ofi.SetParameterValue("sub_dele", sub_1);
                mod40_ofi.SetParameterValue("sub_mayus", sub.ToUpper());

                crystalReportViewer1.ReportSource = mod40_ofi;
            }
            else
            {
                if (resu_tipo_r == 2)
                {
                    leer_config();
                    formato_fecha();
                    user = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user);

                    mod40_inf.SetDataSource(depu);
                    mod40_inf.SetParameterValue("nombre_asegurado", nom_trab);
                    mod40_inf.SetParameterValue("nss", nss);
                    mod40_inf.SetParameterValue("reg_pat", reg_pat);
                    mod40_inf.SetParameterValue("sub", sub);
                    mod40_inf.SetParameterValue("fecha_ini", fecha_ini);
                    mod40_inf.SetParameterValue("fecha_fin", fecha_fin);
                    mod40_inf.SetParameterValue("nom_user", user);
                    mod40_inf.SetParameterValue("fecha_formal", fecha_formal);
                    mod40_inf.SetParameterValue("dele", del.ToUpper());
                    mod40_inf.SetParameterValue("subdele", sub_1.ToUpper());

                    crystalReportViewer1.ReportSource = mod40_inf;
                }
            }
        }

        public void recibir_baja(DataTable tabla_bajas)
        {
            int x = 0;
            dataGridView1.DataSource = tabla_bajas;
            nss_prev = "-";
            //MessageBox.Show(""+tabla_bajas.Rows.Count);
            do
            {
                //if(!nss_prev.Equals(dataGridView1.Rows[x].Cells[1].FormattedValue.ToString())){
                    Depuracion.DataSet_Depu.Mod40_bajaRow fila = depu.Mod40_baja.NewMod40_bajaRow();
                    fila.id = (x+1).ToString();
                    fila.nss = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                    fila.nom_trabajador = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                    fila.periodos = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();

                    depu.Mod40_baja.AddMod40_bajaRow(fila);
                //}

                //nss_prev = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                x++;
            } while (x < dataGridView1.RowCount);

            Datos_inf40 dats40 = new Datos_inf40();
            dats40.leer_config(2);
            dats40.ShowDialog();
            datos_ofi = dats40.regresar1();
            leer_config();
            formato_fecha();
            conex.guardar_evento("Se generó el oficio de baja con la clave: "+datos_ofi[0]+", conteniendo "+x+" casos.");

            mod40_baja.SetDataSource(depu);
            mod40_baja.SetParameterValue("fecha_formal", fecha_formal);
            mod40_baja.SetParameterValue("dele", del.ToUpper());
            mod40_baja.SetParameterValue("sub_dele", sub_1);
            mod40_baja.SetParameterValue("jefe_afi_vig", jefe_afi);
            mod40_baja.SetParameterValue("jefe_cob", jefe_cob);
            mod40_baja.SetParameterValue("n_o_b", datos_ofi[0]);
            mod40_baja.SetParameterValue("sub_mayus", sub_1.ToUpper());
            
            crystalReportViewer1.ReportSource = mod40_baja;
        }
        
        public void recibir_baja_uno_solo(DataTable tabla_bajas)
        {
            int x = 0;
            dataGridView1.DataSource = tabla_bajas;
            nss_prev = "-";
            //MessageBox.Show(""+tabla_bajas.Rows.Count);
            do
            {
            	//if(!nss_prev.Equals(dataGridView1.Rows[x].Cells[1].FormattedValue.ToString())){
            	Depuracion.DataSet_Depu.Mod40_un_periRow fila = depu.Mod40_un_peri.NewMod40_un_periRow();
            	
            	fila.id = (x+1).ToString();
            	fila.nss = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
            	fila.nom_trabajador = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
            	fila.periodos = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
            	depu.Mod40_un_peri.AddMod40_un_periRow(fila);
            	
            	//}

            	//nss_prev = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
            	x++;
            } while (x < dataGridView1.RowCount);

            leer_config();
            formato_fecha();
			
            mod40_un_per.SetDataSource(depu);
            mod40_un_per.SetParameterValue("fecha_formal", fecha_formal);
            mod40_un_per.SetParameterValue("dele", del.ToUpper());
            mod40_un_per.SetParameterValue("sub_mayus", sub_1.ToUpper());
            
            crystalReportViewer1.ReportSource = mod40_un_per;
        }

        private void Visor_oficios40_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

    }
}

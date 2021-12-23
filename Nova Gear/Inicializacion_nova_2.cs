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
    public partial class Inicializacion_nova_2 : Form
    {
        public Inicializacion_nova_2(DateTime fecha_cop, DateTime fecha_rcv)
        {
            InitializeComponent();
            this.f_cop = fecha_cop;
            this.f_rcv = fecha_rcv;
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();
        Conexion conex1 = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        Conexion conex4 = new Conexion();

        DataTable tabla_totales = new DataTable();
        DataTable tabla_totales2 = new DataTable();
        DataTable verif = new DataTable();
        DataTable sindo = new DataTable();
        DataTable exis = new DataTable();
        DataTable omitidos_cop = new DataTable();
        DataTable omitidos_rcv = new DataTable();

        //Declaracion del Hilo para ejecutar un subproceso
        private Thread hilosecundario = null;

        DateTime f_cop, f_rcv;
        int progreso_barra = 0;

        public void consulta_previa()
        {
            String fecha_copr="",fecha_rcvr="";

            fecha_copr=f_cop.ToShortDateString();
            fecha_rcvr=f_rcv.ToShortDateString();
            fecha_copr=fecha_copr.Substring(6,4)+"-"+fecha_copr.Substring(3,2)+"-"+fecha_copr.Substring(0,2);
            fecha_rcvr=fecha_rcvr.Substring(6,4)+"-"+fecha_rcvr.Substring(3,2)+"-"+fecha_rcvr.Substring(0,2);

            conex.conectar("base_principal");
            tabla_totales = conex.consultar("SELECT registro_patronal,credito,periodo,importe,sector,td FROM base_principal.rale WHERE tipo_rale=\"COP\" AND incidencia =\"1\" AND (td=\"02\" OR td=\"03\") AND fecha_alta <=\""+fecha_copr+"\" ORDER BY fecha_alta");
            dataGridView1.DataSource = tabla_totales;
            label1.Text = "Registros: " + tabla_totales.Rows.Count;
            conex.cerrar();
            /*
            conex.conectar("base_principal");
            tabla_totales = conex.consultar("SELECT * FROM base_principal.rale WHERE tipo_rale=\"COP\" AND incidencia =\"1\" AND (td=\"80\" OR td=\"81\" OR td=\"82\") AND fecha_alta <=\"" + fecha_copr + "\" ORDER BY fecha_alta");
            dataGridView2.DataSource = tabla_totales;
            label2.Text = "Registros: " + tabla_totales.Rows.Count;
            conex.cerrar();*/

            conex1.conectar("base_principal");
            tabla_totales2 = conex1.consultar("SELECT registro_patronal,credito,periodo,importe,sector,td FROM base_principal.rale WHERE tipo_rale=\"RCV\" AND incidencia =\"1\" AND (td=\"03\" OR td=\"06\") AND fecha_alta <=\"" + fecha_copr + "\" ORDER BY fecha_alta");
            dataGridView3.DataSource = tabla_totales2;
            label3.Text = "Registros: " + tabla_totales2.Rows.Count;
            conex1.cerrar();
            /*
            conex.conectar("base_principal");
            tabla_totales = conex.consultar("SELECT * FROM base_principal.rale WHERE tipo_rale=\"RCV\" AND incidencia =\"1\" AND (td=\"02\" OR td=\"03\") AND fecha_alta <=\"" + fecha_copr + "\" ORDER BY fecha_alta");
            dataGridView4.DataSource = tabla_totales;
            label4.Text = "Registros: " + tabla_totales.Rows.Count;
            conex.cerrar();*/

        }

        public String consulta_sindo(String rp)
        {
            conex3.conectar("base_principal");
            sindo = conex3.consultar("SELECT nombre FROM sindo WHERE registro_patronal LIKE \""+rp+"%\" ");
            conex3.cerrar();

            if (sindo.Rows.Count > 0)
            {
                if (sindo.Rows[0][0].ToString().Length > 0)
                {
                    return sindo.Rows[0][0].ToString();
                }
                else
                {
                    return "-";
                }
            }
            else
            {
                return "-";
            }            
        }

        public void guardar()
        {
            Invoke(new MethodInvoker(delegate
            {
                int num_cop = 0, num_rcv = 0, guardados = 0;
                String nombre_periodo = "", nombre_periodo_rcv = "", reg_pat = "", reg_pat1 = "", reg_pat2 = "", ra_soc = "", periodo = "", tipo_doc = "", cred_cuo = "", imp_tot = "", sector_n = "", subdele = "";

                if ((dataGridView1.RowCount + dataGridView3.RowCount) > 0)
                {
                    DialogResult resul = MessageBox.Show("Se ingresarán " + (dataGridView1.RowCount + dataGridView3.RowCount) + " registros a la base de datos.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    if (resul == DialogResult.Yes)
                    {
                        conex2.conectar("base_principal");
                        verif = conex2.consultar("SELECT COUNT(id) FROM datos_factura");

                        //Inicializa la barra de progreso
                        progreso_barra = 0;
                        progressBar1.Value = 0;
                        progressBar1.Visible = true;
                        label5.Text = "0%";
                        label5.Visible = true;

                        omitidos_cop.Rows.Clear();
                        omitidos_rcv.Rows.Clear();

                        //SI HAY CREDITOS EN LA BD
                        if (Convert.ToInt32(verif.Rows[0][0].ToString()) > 0)
                        {
                            verif = conex2.consultar("SELECT DISTINCT(nombre_periodo) FROM datos_factura WHERE nombre_periodo LIKE \"COP_INI_%\" ORDER BY nombre_periodo DESC LIMIT 1");

                            if (verif.Rows.Count > 0)
                            {
                                num_cop = Convert.ToInt32(verif.Rows[0][0].ToString().Substring(8, (verif.Rows[0][0].ToString().Length - 8)));
                                if (num_cop < 10)
                                {
                                    nombre_periodo = "COP_INI_0" + (num_cop + 1);
                                }
                                else
                                {
                                    nombre_periodo = "COP_INI_" + (num_cop + 1);
                                }
                            }
                            else
                            {
                                nombre_periodo = "COP_INI_00";
                            }

                            verif = conex2.consultar("SELECT DISTINCT(nombre_periodo) FROM datos_factura WHERE nombre_periodo LIKE \"RCV_INI_%\" ORDER BY nombre_periodo DESC LIMIT 1");

                            if (verif.Rows.Count > 0)
                            {
                                num_rcv = Convert.ToInt32(verif.Rows[0][0].ToString().Substring(8, (verif.Rows[0][0].ToString().Length - 8)));
                                if (num_rcv < 10)
                                {
                                    nombre_periodo_rcv = "RCV_INI_0" + (num_rcv + 1);
                                }
                                else
                                {
                                    nombre_periodo_rcv = "RCV_INI_" + (num_rcv + 1);
                                }
                            }
                            else
                            {
                                nombre_periodo_rcv = "RCV_INI_00";
                            }

                            conex4.conectar("base_principal");
                            for (int i = 0; i < dataGridView1.RowCount; i++)
                            {
                                //COP
                                reg_pat2 = dataGridView1[0, i].Value.ToString();
                                reg_pat1 = reg_pat2 + "0";
                                reg_pat = reg_pat1.Substring(0, 3) + "-" + reg_pat1.Substring(3, 5) + "-" + reg_pat1.Substring(8, 2) + "-0";

                                periodo = dataGridView1[2, i].Value.ToString();
                                tipo_doc = (Convert.ToInt32(dataGridView1[5, i].Value.ToString()).ToString());
                                cred_cuo = dataGridView1[1, i].Value.ToString();
                                imp_tot = dataGridView1[3, i].Value.ToString();
                                sector_n = dataGridView1[4, i].Value.ToString();
                                subdele = conex.leer_config_sub()[4];
                                ra_soc = consulta_sindo(reg_pat2);

                                exis = conex4.consultar("SELECT id FROM datos_factura WHERE registro_patronal2=\"" + reg_pat2 + "\" AND credito_cuotas=\"" + cred_cuo + "\" AND periodo=\"" + periodo + "\"");

                                if (exis.Rows.Count > 0)
                                {
                                    omitidos_cop.Rows.Add(reg_pat2, cred_cuo, periodo, imp_tot, sector_n, tipo_doc);
                                }
                                else
                                {
                                    conex2.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, credito_multa, importe_cuota, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion) " +
                                        " VALUES (\"" + nombre_periodo + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + ra_soc + "\",\"" + periodo + "\",\"" + tipo_doc + "\",\"" + cred_cuo + "\",\"-\"," + imp_tot + ",0.00 ,\"" + sector_n + "\",\"" + sector_n + "\",\"" + subdele + "\")");
                                    guardados++;
                                }

                                progreso(i, (dataGridView1.RowCount + dataGridView3.RowCount));
                            }

                            for (int i = 0; i < dataGridView3.RowCount; i++)
                            {
                                //RCV
                                reg_pat2 = dataGridView3[0, i].Value.ToString();
                                reg_pat1 = reg_pat2 + "0";
                                reg_pat = reg_pat1.Substring(0, 3) + "-" + reg_pat1.Substring(3, 5) + "-" + reg_pat1.Substring(8, 2) + "-0";

                                periodo = dataGridView3[2, i].Value.ToString();
                                tipo_doc = (Convert.ToInt32(dataGridView3[5, i].Value.ToString()).ToString());
                                cred_cuo = dataGridView3[1, i].Value.ToString();
                                imp_tot = dataGridView3[3, i].Value.ToString();
                                sector_n = dataGridView3[4, i].Value.ToString();
                                subdele = conex.leer_config_sub()[4];
                                ra_soc = consulta_sindo(reg_pat2);

                                exis = conex4.consultar("SELECT id FROM datos_factura WHERE registro_patronal2=\"" + reg_pat2 + "\" AND credito_cuotas=\"" + cred_cuo + "\" AND periodo=\"" + periodo + "\"");

                                if (exis.Rows.Count > 0)
                                {
                                    omitidos_rcv.Rows.Add(reg_pat2, cred_cuo, periodo, imp_tot, sector_n, tipo_doc);
                                }
                                else
                                {
                                    conex2.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, credito_multa, importe_cuota, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion) " +
                                        " VALUES (\"" + nombre_periodo_rcv + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + ra_soc + "\",\"" + periodo + "\",\"" + tipo_doc + "\",\"" + cred_cuo + "\",\"-\"," + imp_tot + ",0.00 ,\"" + sector_n + "\",\"" + sector_n + "\",\"" + subdele + "\")");
                                    guardados++;
                                }

                                progreso((i+dataGridView1.RowCount), (dataGridView1.RowCount + dataGridView3.RowCount));
                            }
                            conex4.cerrar();
                        }
                        else//SI NO HAY CREDITOS EN LA BD
                        {
                            nombre_periodo = "COP_INI_00";
                            nombre_periodo_rcv = "RCV_INI_00";

                            conex4.conectar("base_principal");
                            for (int i = 0; i < dataGridView1.RowCount; i++)
                            {
                                //COP
                                reg_pat2 = dataGridView1[0, i].Value.ToString();
                                reg_pat1 = reg_pat2 + "0";
                                reg_pat = reg_pat1.Substring(0, 3) + "-" + reg_pat1.Substring(3, 5) + "-" + reg_pat1.Substring(8, 2) + "-0";

                                periodo = dataGridView1[2, i].Value.ToString();
                                tipo_doc = (Convert.ToInt32(dataGridView1[5, i].Value.ToString()).ToString());
                                cred_cuo = dataGridView1[1, i].Value.ToString();
                                imp_tot = dataGridView1[3, i].Value.ToString();
                                sector_n = dataGridView1[4, i].Value.ToString();
                                subdele = conex.leer_config_sub()[4];
                                ra_soc = consulta_sindo(reg_pat2);

                                /* exis = conex4.consultar("SELECT id FROM datos_factura WHERE registro_patronal2=\"" + reg_pat2 + "\" AND credito_cuotas=\"" + cred_cuo + "\" AND periodo=\"" + periodo + "\"");

                                 if (exis.Rows.Count > 0)
                                 {
                                     omitidos_cop.Rows.Add(reg_pat2, cred_cuo, periodo, imp_tot, sector_n, tipo_doc);
                                 }
                                 else
                                 {*/
                                conex2.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, credito_multa, importe_cuota, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion) " +
                                    " VALUES (\"" + nombre_periodo + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + ra_soc + "\",\"" + periodo + "\",\"" + tipo_doc + "\",\"" + cred_cuo + "\",\"-\"," + imp_tot + ",0.00 ,\"" + sector_n + "\",\"" + sector_n + "\",\"" + subdele + "\")");
                                guardados++;
                                //}

                                progreso(i, (dataGridView1.RowCount + dataGridView3.RowCount));
                            }

                            for (int i = 0; i < dataGridView3.RowCount; i++)
                            {
                                //RCV
                                reg_pat2 = dataGridView3[0, i].Value.ToString();
                                reg_pat1 = reg_pat2 + "0";
                                reg_pat = reg_pat1.Substring(0, 3) + "-" + reg_pat1.Substring(3, 5) + "-" + reg_pat1.Substring(8, 2) + "-0";

                                periodo = dataGridView3[2, i].Value.ToString();
                                tipo_doc = (Convert.ToInt32(dataGridView3[5, i].Value.ToString()).ToString());
                                cred_cuo = dataGridView3[1, i].Value.ToString();
                                imp_tot = dataGridView3[3, i].Value.ToString();
                                sector_n = dataGridView3[4, i].Value.ToString();
                                subdele = conex.leer_config_sub()[4];
                                ra_soc = consulta_sindo(reg_pat2);

                                /*exis = conex4.consultar("SELECT id FROM datos_factura WHERE registro_patronal2=\"" + reg_pat2 + "\" AND credito_cuotas=\"" + cred_cuo + "\" AND periodo=\"" + periodo + "\"");

                                if (exis.Rows.Count > 0)
                                {
                                    omitidos_rcv.Rows.Add(reg_pat2, cred_cuo, periodo, imp_tot, sector_n, tipo_doc);
                                }
                                else
                                {*/
                                conex2.consultar("INSERT INTO datos_factura (nombre_periodo, registro_patronal, registro_patronal1, registro_patronal2, razon_social, periodo, tipo_documento, credito_cuotas, credito_multa, importe_cuota, importe_multa, sector_notificacion_inicial, sector_notificacion_actualizado,subdelegacion) " +
                                    " VALUES (\"" + nombre_periodo_rcv + "\",\"" + reg_pat + "\",\"" + reg_pat1 + "\",\"" + reg_pat2 + "\",\"" + ra_soc + "\",\"" + periodo + "\",\"" + tipo_doc + "\",\"" + cred_cuo + "\",\"-\"," + imp_tot + ",0.00 ,\"" + sector_n + "\",\"" + sector_n + "\",\"" + subdele + "\")");
                                guardados++;
                                //}

                                progreso((i + dataGridView1.RowCount), (dataGridView1.RowCount + dataGridView3.RowCount));
                            }
                            conex4.cerrar();
                        }

                        MessageBox.Show("El proceso concluyó exitosamente.\nSe guardaron " + guardados + " registros.\nSe omitieron " + (omitidos_cop.Rows.Count + omitidos_rcv.Rows.Count) + " registros.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dataGridView1.DataSource = omitidos_cop;
                        label1.Text = "Registros: " + omitidos_cop.Rows.Count;
                        dataGridView3.DataSource = omitidos_rcv;
                        label3.Text = "Registros: " + omitidos_rcv.Rows.Count;                        
                        progressBar1.Value = 100;
                        label5.Text = "100%";
                        button1.Enabled = false;
                    }
                }
            }));
        }

        public void progreso(int paso, int tot)
        {
            progreso_barra = Convert.ToInt32(((paso+1)*100)/tot);
            
            if(paso==tot){
                progreso_barra = 100;
                label5.Text = "100%";
            }

            if(progreso_barra<100){
                progressBar1.Value = progreso_barra;
                label5.Text = progreso_barra + " %"; ;
            }else{
                progressBar1.Value = 100;
                label5.Text = "100%";
            }

            //MessageBox.Show("paso: "+paso+" tot: "+tot);
        }
        
        private void Inicializacion_nova_2_Load(object sender, EventArgs e)
        {
            consulta_previa();

            omitidos_cop.Columns.Add("Registro Patronal");
            omitidos_cop.Columns.Add("Credito");
            omitidos_cop.Columns.Add("Periodo");
            omitidos_cop.Columns.Add("Importe");
            omitidos_cop.Columns.Add("Sector");
            omitidos_cop.Columns.Add("Tipo Documento");

            omitidos_rcv.Columns.Add("Registro Patronal");
            omitidos_rcv.Columns.Add("Credito");
            omitidos_rcv.Columns.Add("Periodo");
            omitidos_rcv.Columns.Add("Importe");
            omitidos_rcv.Columns.Add("Sector");
            omitidos_rcv.Columns.Add("Tipo Documento");

        }

        private void button1_Click(object sender, EventArgs e)
        {                        
            hilosecundario = new Thread(new ThreadStart(guardar));
            hilosecundario.Start();
        }
    
    
    }
}

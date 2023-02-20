using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.IO;
using Office = Microsoft.Office.Interop.Word;

namespace Nova_Gear.Estrados
{
    public partial class control_por_volumen_estrados : Form
    {
        public control_por_volumen_estrados()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();//guardar_datos--
        Conexion conex0 = new Conexion();//dias_festivos
        Conexion conex1 = new Conexion();//carga periodos
        Conexion conex2 = new Conexion();//consultar folios--
        Conexion conex3 = new Conexion();//consultar fecha documentos--

        DataTable consultamysql = new DataTable();
        DataTable consultamysql2 = new DataTable();
        DataTable consultamysql3 = new DataTable();
        DataTable consultamysql4 = new DataTable();
        DataTable consultamysql_verifica = new DataTable();
        DataTable tabla_dias_fest = new DataTable();
        DataTable datos_a_guardar = new DataTable();
        DataTable datos_a_guardar1 = new DataTable();
        DataTable datos_para_usuario = new DataTable();
        DataTable num_packs = new DataTable();
        DataTable data_acumulador = new DataTable();
        DataTable fechas_docs = new DataTable();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;

        int id_user = 0;
        String subdele,sub_nom,del_num,sub_num,dias_not,fech_doc="";

        private Thread hilo2 = null;
        private Thread hilo3 = null;

        private void button6_Click(object sender, EventArgs e)
        {

        }
        //ENTRAR A RECIBIR NN
        private void button1_Click(object sender, EventArgs e)
        {
            int num_pack=0;
            llenar_Cb1();
            panel1.Show();

            conex0.conectar("base_principal");
            tabla_dias_fest = conex0.consultar("SELECT dia FROM dias_festivos");
            timer1.Enabled = true;

            id_user = Convert.ToInt32(MainForm.datos_user_static[7]);
            dateTimePicker4.Value = cuenta_dias_hab(dateTimePicker3.Value, 1);
            dateTimePicker5.Value = cuenta_dias_hab(dateTimePicker3.Value, 2);
            dateTimePicker6.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(dias_not)+1));
            dateTimePicker7.Value = cuenta_dias_hab(dateTimePicker3.Value, Convert.ToInt32(dias_not) + 2);

            if (datos_a_guardar.Columns.Contains("id_credito") == false)
            {
                datos_a_guardar.Columns.Add("id_credito");
                datos_a_guardar.Columns.Add("tipo_doc");
                datos_a_guardar.Columns.Add("nom_per");
                datos_a_guardar.Columns.Add("fecha_doc");

                datos_para_usuario.Columns.Add("#");
                datos_para_usuario.Columns.Add("FOLIO");
                datos_para_usuario.Columns.Add("ID_CREDITO");
                datos_para_usuario.Columns.Add("FECHA_EMISION_DOC");
                datos_para_usuario.Columns.Add("NOMBRE_DOCUMENTO");
                datos_para_usuario.Columns.Add("SUPUESTO_ESTRADOS");
                datos_para_usuario.Columns.Add("MOTIVO_ESTRADOS");
                datos_para_usuario.Columns.Add("FECHA_ACTA_CIRCUNSTANCIADA");
                datos_para_usuario.Columns.Add("FOJAS");
                datos_para_usuario.Columns.Add("FECHA_FIRMA_ALTA");
                datos_para_usuario.Columns.Add("FECHA_PUBLICACION");
                datos_para_usuario.Columns.Add("FECHA_INICIO_NOT");
                datos_para_usuario.Columns.Add("FECHA_FIN_NOT");
                datos_para_usuario.Columns.Add("FECHA_RETIRO_NOT");
                datos_para_usuario.Columns.Add("NOTIFICADOR_ESTRADOS");
                datos_para_usuario.Columns.Add("SUBDELEGADO");
                datos_para_usuario.Columns.Add("SUBDELEGACION_EMISORA");
                datos_para_usuario.Columns.Add("HORA_NOT");
                datos_para_usuario.Columns.Add("OBSERVACIONES");
                datos_para_usuario.Columns.Add("NUM_PAQUETE");
            }

            conex2.conectar("base_principal");
            consultamysql3 = conex2.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC LIMIT 1");

            if (consultamysql3.Rows.Count > 0)
            {
                num_pack = Convert.ToInt32(consultamysql3.Rows[0][0].ToString());
                num_pack = num_pack + 1;
            }
            else
            {
                num_pack = 1;
            }

            label12.Text = "Sig. Paquete: "+num_pack;
            conex2.cerrar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reportes_cartera_Nvo repo = new Reportes_cartera_Nvo();
            MainForm.cartera_estrados = 1;
            repo.Show();
            this.Close();
        }
        //------LOAD
        private void control_por_volumen_estrados_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            leer_config();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public DateTime cuenta_dias_hab(DateTime dia_ini, int numd_dias)
        {
            int i = 0, j = 0,k=0;

            while (i < numd_dias)
            {
                dia_ini = dia_ini.AddDays(1);

                if (((dia_ini.DayOfWeek.ToString().Equals("Saturday") == true) || (dia_ini.DayOfWeek.ToString().Equals("Sunday") == true)))
                {
                    dia_ini = dia_ini.AddDays(1);
                }
                else
                {                    
                    while (j < tabla_dias_fest.Rows.Count)
                    {
                        if (dia_ini.ToString().Substring(0, 10).Equals(tabla_dias_fest.Rows[j][0].ToString().Substring(0, 10)))
                        {
                            //MessageBox.Show(dia_ini.ToString().Substring(0, 10) + "|" + tabla_dias_fest.Rows[j][0].ToString().Substring(0, 10) + "|" + j + "|" + i);
                            dia_ini = dia_ini.AddDays(1.0);
                            j = tabla_dias_fest.Rows.Count;
                            k = 1;
                        }
                        j++;
                    }

                    if (k == 0)
                    {
                        i++;
                    }
                    else
                    {
                        k = 0;
                    }

                    j = 0;                   
                }
                
                //MessageBox.Show(dia_ini.DayOfWeek.ToString());
            }
            return dia_ini;
        }

        public DateTime cuenta_dias_hab_reversa(DateTime dia_ini, int numd_dias)
        {
            int i = 0, j = 0, k = 0;

            while (i < numd_dias)
            {
                dia_ini = dia_ini.AddDays(-1);

                if (((dia_ini.DayOfWeek.ToString().Equals("Saturday") == true) || (dia_ini.DayOfWeek.ToString().Equals("Sunday") == true)))
                {
                    dia_ini = dia_ini.AddDays(-1);
                }
                else
                {
                    while (j < tabla_dias_fest.Rows.Count)
                    {
                        if (dia_ini.ToString().Substring(0, 10).Equals(tabla_dias_fest.Rows[j][0].ToString().Substring(0, 10)))
                        {
                            //MessageBox.Show(dia_ini.ToString().Substring(0, 10) + "|" + tabla_dias_fest.Rows[j][0].ToString().Substring(0, 10) + "|" + j + "|" + i);
                            dia_ini = dia_ini.AddDays(-1);
                            j = tabla_dias_fest.Rows.Count;
                            k = 1;
                        }
                        j++;
                    }

                    if (k == 0)
                    {
                        i++;
                    }
                    else
                    {
                        k = 0;
                    }

                    j = 0;
                }

                //MessageBox.Show(dia_ini.DayOfWeek.ToString());
            }
            return dia_ini;
        }
        //periodos
        public void llenar_Cb1()
        {
            conex.conectar("base_principal");
            comboBox1.Items.Clear();
            int i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
            do
            {
                comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void llenar_Cb2()
        {
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            int i = 0;
            dataGridView2.DataSource = conex.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC");
            do
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() != "0")
                {
                    comboBox2.Items.Add("TODOS");
                }
                else
                {
                    comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                }
                i++;
            } while (i < dataGridView2.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void llenar_Cb4()
        {
            conex.conectar("base_principal");
            comboBox4.Items.Clear();
            int i = 0;
            //dataGridView4.DataSource = conex.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC");
            dataGridView4.DataSource = conex.consultar("SELECT paquete FROM estrados group by paquete order by paquete DESC");
            
            do
            {
                if (dataGridView4.Rows[i].Cells[0].Value.ToString() != "0")
                {
                    //comboBox4.Items.Add("TODOS");
                    if (verificar_paquetes(dataGridView4.Rows[i].Cells[0].Value.ToString()) == true)
                    {
                        comboBox4.Items.Add(dataGridView4.Rows[i].Cells[0].Value.ToString());
                    }
                }
                else
                {
                    //comboBox4.Items.Add(dataGridView4.Rows[i].Cells[0].Value.ToString());
                }
                i++;
            } while (i < dataGridView4.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void estilo_grid()
        {
            int cont = 0;

            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].ReadOnly = true;

            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = true;
            dataGridView1.Columns[11].Visible = true;

            dataGridView1.Columns[1].HeaderText = "REGISTRO PATRONAL";
            dataGridView1.Columns[2].HeaderText = "RAZÓN SOCIAL";
            dataGridView1.Columns[3].HeaderText = "CRÉDITO CUOTA";
            dataGridView1.Columns[4].HeaderText = "CRÉDITO MULTA";
            dataGridView1.Columns[5].HeaderText = "IMPORTE CUOTA";
            dataGridView1.Columns[6].HeaderText = "IMPORTE MULTA";
            dataGridView1.Columns[7].HeaderText = "FECHA RECEPCION";
            dataGridView1.Columns[8].HeaderText = "ID";
            dataGridView1.Columns[10].HeaderText = "NOMBRE PERIODO";
            dataGridView1.Columns[11].HeaderText = "NUM PAGS PDF";
               
            
            dataGridView1.Columns[1].Width = 130;
            dataGridView1.Columns[2].Width = 340;
            dataGridView1.Columns[3].Width = 130;
            dataGridView1.Columns[4].Width = 130;
            dataGridView1.Columns[5].Width = 130;
            dataGridView1.Columns[6].Width = 130;
            dataGridView1.Columns[7].Width = 130;
            dataGridView1.Columns[8].Width = 130;

           //dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
           //dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dataGridView1.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;

            cont = 0;
            while (cont < consultamysql.Rows.Count)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
            }
        }

        public void estilo_grid1()
        {
            int cont = 0;

            dataGridView3.Columns[1].ReadOnly = true;
            dataGridView3.Columns[2].ReadOnly = true;
            dataGridView3.Columns[3].ReadOnly = true;
            dataGridView3.Columns[4].ReadOnly = true;
            dataGridView3.Columns[5].ReadOnly = true;
            dataGridView3.Columns[6].ReadOnly = true;
            dataGridView3.Columns[7].ReadOnly = true;
            dataGridView3.Columns[8].ReadOnly = true;
            dataGridView3.Columns[9].Visible = false;

            dataGridView3.Columns[1].HeaderText = "FOLIO";
            dataGridView3.Columns[2].HeaderText = "REGISTRO PATRONAL";
            dataGridView3.Columns[3].HeaderText = "RAZÓN SOCIAL";
            dataGridView3.Columns[4].HeaderText = "PERIODO";
            dataGridView3.Columns[5].HeaderText = "CREDITO CUOTA";
            dataGridView3.Columns[6].HeaderText = "CREDITO MULTA";
            dataGridView3.Columns[7].HeaderText = "FECHA NOTIFICACION";
            dataGridView3.Columns[8].HeaderText = "NUM PAQUETE";

            dataGridView3.Columns[1].Width = 120;
            dataGridView3.Columns[2].Width = 120;
            dataGridView3.Columns[3].Width = 320;
            dataGridView3.Columns[4].Width = 80;
            dataGridView3.Columns[5].Width = 100;
            dataGridView3.Columns[6].Width = 100;
            dataGridView3.Columns[7].Width = 120;
            dataGridView3.Columns[8].Width = 100;

            dataGridView3.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView3.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;

            cont = 0;
            while (cont < dataGridView3.Rows.Count)
            {
                dataGridView3.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView3.Rows[cont].Cells[0].Value = false;
                cont++;
            }
        }

        public void estilo_grid2()
        {
            int cont = 0;

            dataGridView5.Columns[1].ReadOnly = true;
            dataGridView5.Columns[2].ReadOnly = true;
            dataGridView5.Columns[3].ReadOnly = true;
            dataGridView5.Columns[4].ReadOnly = true;
            dataGridView5.Columns[5].ReadOnly = true;
            dataGridView5.Columns[6].ReadOnly = true;
            dataGridView5.Columns[7].ReadOnly = true;
            dataGridView5.Columns[8].ReadOnly = true;
            //dataGridView5.Columns[9].Visible = false;

            dataGridView5.Columns[1].HeaderText = "FOLIO";
            dataGridView5.Columns[2].HeaderText = "REGISTRO PATRONAL";
            dataGridView5.Columns[3].HeaderText = "RAZÓN SOCIAL";
            dataGridView5.Columns[4].HeaderText = "PERIODO";
            dataGridView5.Columns[5].HeaderText = "CREDITO CUOTA";
            dataGridView5.Columns[6].HeaderText = "CREDITO MULTA";
            dataGridView5.Columns[7].HeaderText = "NUM PAQUETE";
            dataGridView5.Columns[8].HeaderText = "ID CREDITO";

            dataGridView5.Columns[1].Width = 120;
            dataGridView5.Columns[2].Width = 120;
            dataGridView5.Columns[3].Width = 320;
            dataGridView5.Columns[4].Width = 80;
            dataGridView5.Columns[5].Width = 100;
            dataGridView5.Columns[6].Width = 100;
            dataGridView5.Columns[7].Width = 120;
            dataGridView5.Columns[8].Width = 100;

            dataGridView5.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView5.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;

            cont = 0;
            while (cont < dataGridView5.Rows.Count)
            {
                dataGridView5.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView5.Rows[cont].Cells[0].Value = false;
                cont++;
            }
        }

        public void leer_config()
        {
            String del,mpio, sub, jefe_cob, jefe_emi, jefe_afi, ref_baja, ref_ofi;
            //try
            //{
            StreamReader rdr = new StreamReader(@"sub_config.lz");

            del = rdr.ReadLine();
            del_num = rdr.ReadLine();
            mpio = rdr.ReadLine();
            sub = rdr.ReadLine();
            sub_num = rdr.ReadLine();
            subdele = rdr.ReadLine();
            jefe_afi = rdr.ReadLine();
            jefe_cob = rdr.ReadLine();
            jefe_emi = rdr.ReadLine();
            dias_not = rdr.ReadLine();
            dias_not = rdr.ReadLine();
            dias_not = rdr.ReadLine();
            //ref_baja = rdr.ReadLine();
            //ref_ofi = rdr.ReadLine();
            rdr.Close();

            del = del.Substring(11, del.Length - 11);
            del_num = del_num.Substring(7, del_num.Length - 7);
            mpio = mpio.Substring(10, mpio.Length - 10);
            sub = sub.Substring(14, sub.Length - 14);
            sub_num = sub_num.Substring(7, sub_num.Length - 7);
            subdele = subdele.Substring(12, subdele.Length - 12);
            jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);
            jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
            jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
            dias_not = dias_not.Substring(9, dias_not.Length - 9);

            sub_nom = sub;
            //ref_baja = ref_baja.Substring(9, ref_baja.Length - 9);
            //ref_ofi = ref_ofi.Substring(8, ref_ofi.Length - 8);

            //}
            //catch (Exception error)
            //{
            //MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}
        }

        public void cargar_datos_nn()
        {
            String sql = "", sql1 = "";
            conex1.conectar("base_principal");
            conex3.conectar("base_principal");
            /*
            if (comboBox1.SelectedIndex == -1)
            {
                 DialogResult resul = MessageBox.Show("Si no se selecciona un periodo en particular, no se podrá determinar de forma automática la fecha de emisión del documento.\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                 if (resul == DialogResult.Yes)
                 {*/
                     sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,nombre_periodo,pags_pdf,fecha_traspaso as \"FECHA DOCUMENTO\" FROM datos_factura WHERE" +
                         " nn = \"NN\" AND status=\"EN TRAMITE\" ORDER BY fecha_recepcion,registro_patronal, credito_cuotas";

                     consultamysql = conex1.consultar(sql);

                     for (int i = 0; i < consultamysql.Rows.Count; i++)
                     {
                         sql1 = "SELECT fecha_impresa_documento FROM estado_periodos WHERE nombre_periodo=\"" + consultamysql.Rows[i][9] + "\"";
                         fechas_docs= conex3.consultar(sql1);

                         if (fechas_docs.Rows.Count > 0)
                         {
                             consultamysql.Rows[i][11] = fechas_docs.Rows[0][0].ToString();
                         }
                         else
                         {
                             consultamysql.Rows[i][11] = dateTimePicker3.Value;
                             //fech_doc = dateTimePicker3.Text;
                             //fech_doc = fech_doc.Substring(6, 4) + "-" + fech_doc.Substring(3, 2) + "-" + fech_doc.Substring(0, 2);
                             //consultamysql.Rows[i][11] = fech_doc;
                         }                         
                     }                     
                /*}
            }
            else
           {
                sql = "SELECT fecha_impresa_documento FROM estado_periodos WHERE nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\"";
                consultamysql = conex1.consultar(sql);

                if (consultamysql.Rows.Count > 0)
                {
                    fech_doc = consultamysql.Rows[0][0].ToString();

                    sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,fecha_recepcion,id,tipo_documento,nombre_periodo,pags_pdf,fecha_traspaso as \"FECHA DOCUMENTO\" FROM datos_factura WHERE" +
                    " nn = \"NN\" AND status=\"EN TRAMITE\" AND nombre_periodo=\"" + comboBox1.SelectedItem.ToString() + "\" ORDER BY fecha_recepcion,registro_patronal, credito_cuotas";
                    consultamysql = conex1.consultar(sql);
                }
                else
                {
                    MessageBox.Show("El periodo seleccionado no cuenta con fecha de documento almacenada en la base de datos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }*/
            
            dataGridView1.DataSource = consultamysql;
            label3.Text = "Registros Totales: " + consultamysql.Rows.Count;

            if(consultamysql.Rows.Count>0){
                estilo_grid();
            }
        }

        public void cargar_datos_confirma_fechas()
        {
            Invoke(new MethodInvoker(delegate
            {
                String sql = "", sql1 = "", folio = "", fecha_not = "",fech="";
                int porc = 0,num_pack=0,num=0;
                DateTime fecha_hoy = DateTime.Today;

                conex0.conectar("base_principal");
                conex1.conectar("base_principal");

                sql = "SELECT id,registro_patronal,razon_social,periodo,credito_cuotas,credito_multa FROM datos_factura WHERE status=\"EN TRAMITE\" and nn=\"ESTRADOS\"";
                consultamysql = conex0.consultar(sql);

                if (consultamysql2.Columns.Contains("FOLIO") == false)
                {
                    consultamysql2.Columns.Add("FOLIO");
                    consultamysql2.Columns.Add("REGISTRO_PATRONAL");
                    consultamysql2.Columns.Add("RAZON SOCIAL");
                    consultamysql2.Columns.Add("PERIODO");
                    consultamysql2.Columns.Add("CREDITO_CUOTA");
                    consultamysql2.Columns.Add("CREDITO_MULTA");
                    consultamysql2.Columns.Add("FECHA_NOTIFICACION",typeof(DateTime));
                    consultamysql2.Columns.Add("NUM_PAQUETE");
                    consultamysql2.Columns.Add("ID_CREDITO");
                }

                //fech = (fecha_hoy.Year) + "-" + (fecha_hoy.Month) + "-" + (fecha_hoy.Day);
                fech = "2015-04-15";

                //MessageBox.Show(""+consultamysql.Rows.Count);
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel1.Text = "Cargando...";
                consultamysql2.Rows.Clear();

                for (int i = 0; i < consultamysql.Rows.Count; i++)
                {
                    sql1 = "SELECT folio,fecha_retiro_not,paquete FROM estrados WHERE id_credito=\"" + consultamysql.Rows[i][0].ToString() + "\" AND fecha_retiro_not >= \"" + fech + "\"";
                    //sql1 = "SELECT folio,fecha_retiro_not,paquete FROM estrados WHERE id_credito=\"" + consultamysql.Rows[i][0].ToString() + "\"";
                    consultamysql_verifica = conex1.consultar(sql1);

                    if (consultamysql_verifica.Rows.Count > 0)
                    {
                        folio = consultamysql_verifica.Rows[0][0].ToString();
                        fecha_not = consultamysql_verifica.Rows[0][1].ToString().Substring(0,10);
                        num_pack = Convert.ToInt32(consultamysql_verifica.Rows[0][2].ToString());

                        consultamysql2.Rows.Add(
                            folio,//FOLIO
                            consultamysql.Rows[i][1].ToString(),//REG_PAT
                            consultamysql.Rows[i][2].ToString(),//RAZ_SOC
                            consultamysql.Rows[i][3].ToString(),//PERIODO
                            consultamysql.Rows[i][4].ToString(),//CRED_CUO
                            consultamysql.Rows[i][5].ToString(),//CRED_MUL
                            fecha_not,//FECHA_NOT
                            num_pack,//NUM PACK
                            consultamysql.Rows[i][0].ToString()//ID_CREDITO
                        );
                    }

                    toolStripStatusLabel1.Text = "Cargando " + (i+1) + " de " + consultamysql.Rows.Count;
                    porc = Convert.ToInt32(((i + 1) * 100) / consultamysql.Rows.Count);
                    toolStripProgressBar1.Value = porc;
                    toolStripStatusLabel2.Text = porc + "%";
                }

                toolStripProgressBar1.Value = 100;
                toolStripStatusLabel2.Text = "100%";

                DataView dv = new DataView(consultamysql2);
                num_packs = dv.ToTable(true, "NUM_PAQUETE");

                num_packs.DefaultView.Sort = "NUM_PAQUETE desc";
                num_packs = num_packs.DefaultView.ToTable();

                if(comboBox2.Items.Count>0){
                    comboBox2.Items.Clear();
                }

                for (int i = 0; i < num_packs.Rows.Count;i++)
                {
                    comboBox2.Items.Add(num_packs.Rows[i][0].ToString());
                }

                comboBox2.Items.Add("TODOS");

                consultamysql2.DefaultView.Sort = "FOLIO asc, CREDITO_CUOTA asc";
                consultamysql2 = consultamysql2.DefaultView.ToTable();

                dataGridView3.DataSource = consultamysql2;
                //label8.Text = "Registros Totales: " + consultamysql2.Rows.Count;
                label8.Text = "Registros Totales: " + dataGridView3.RowCount;
                estilo_grid1();

                toolStripStatusLabel1.Text = "Listo";
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel2.Text = "0%";
                
            }));
        }

        public void cargar_datos_edita_fechas()
        {
            //Invoke(new MethodInvoker(delegate
            //{
                String sql = "", sql1 = "", reg_pat = "", ra_soc = "", per = "",cred_c = "",cred_m = "",nom_per="",num_pags="";
                int porc = 0, num_pack = 0, num = 0;
                DateTime fecha_hoy = DateTime.Today;

                conex2.conectar("base_principal");
                conex1.conectar("base_principal");

                sql = "SELECT folio,id_credito,paquete FROM estrados WHERE paquete="+comboBox4.SelectedItem.ToString()+"";
                //sql = "SELECT id,registro_patronal,razon_social,periodo,credito_cuotas,credito_multa FROM datos_factura WHERE status=\"EN TRAMITE\" and nn=\"ESTRADOS\"";
                consultamysql = conex2.consultar(sql);

                if (consultamysql4.Columns.Contains("FOLIO") == false)
                {
                    consultamysql4.Columns.Add("FOLIO");
                    consultamysql4.Columns.Add("REGISTRO_PATRONAL");
                    consultamysql4.Columns.Add("RAZON SOCIAL");
                    consultamysql4.Columns.Add("PERIODO");
                    consultamysql4.Columns.Add("CREDITO_CUOTA");
                    consultamysql4.Columns.Add("CREDITO_MULTA");
                    consultamysql4.Columns.Add("NUM_PAQUETE");
                    consultamysql4.Columns.Add("ID_CREDITO");
                    consultamysql4.Columns.Add("NOMBRE_PER");
                    consultamysql4.Columns.Add("NUM_PAGS");
                }

                //fech = (fecha_hoy.Year) + "-" + (fecha_hoy.Month) + "-" + (fecha_hoy.Day);

                //MessageBox.Show(""+consultamysql.Rows.Count);
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel1.Text = "Cargando...";
                consultamysql4.Rows.Clear();

                for (int i = 0; i < consultamysql.Rows.Count; i++)
                {
                    sql1 = "SELECT registro_patronal,razon_social,periodo,credito_cuotas,credito_multa,nombre_periodo,pags_pdf FROM datos_factura WHERE id=" + consultamysql.Rows[i][1].ToString() + " and status=\"EN TRAMITE\" and nn=\"ESTRADOS\"";
                    //MessageBox.Show(sql1);
                    //sql = "SELECT folio,fecha_retiro_not,paquete FROM estrados WHERE id_credito=\"" + consultamysql.Rows[i][0].ToString() + "\" AND fecha_retiro_not <= \"" + fech + "\"";
                    consultamysql_verifica = conex1.consultar(sql1);

                    if (consultamysql_verifica.Rows.Count > 0)
                    {
                        reg_pat = consultamysql_verifica.Rows[0][0].ToString();
                        ra_soc = consultamysql_verifica.Rows[0][1].ToString();
                        per = consultamysql_verifica.Rows[0][2].ToString();
                        cred_c = consultamysql_verifica.Rows[0][3].ToString();
                        cred_m = consultamysql_verifica.Rows[0][4].ToString();
                        nom_per = consultamysql_verifica.Rows[0][5].ToString();
                        num_pags = consultamysql_verifica.Rows[0][6].ToString();

                        consultamysql4.Rows.Add(
                            consultamysql.Rows[i][0].ToString(),//FOLIO
                            reg_pat,//REG_PAT
                            ra_soc,//RAZ_SOC
                            per,//PERIODO
                            cred_c,//CRED_CUO
                            cred_m,//CRED_MUL
                            consultamysql.Rows[i][2].ToString(),//NUM PACK
                            consultamysql.Rows[i][1].ToString(),//ID_CREDITO
                            nom_per,//nom_per
                            num_pags//pags_pdf
                        );
                    }

                    toolStripStatusLabel1.Text = "Cargando " + (i + 1) + " de " + consultamysql.Rows.Count;
                    porc = Convert.ToInt32(((i + 1) * 100) / consultamysql.Rows.Count);
                    toolStripProgressBar1.Value = porc;
                    toolStripStatusLabel2.Text = porc + "%";
                }

                toolStripProgressBar1.Value = 100;
                toolStripStatusLabel2.Text = "100%";
                /*
                DataView dv = new DataView(consultamysql2);
                num_packs = dv.ToTable(true, "NUM_PAQUETE");
                
                num_packs.DefaultView.Sort = "NUM_PAQUETE desc";
                num_packs = num_packs.DefaultView.ToTable();

                for (int i = 0; i < num_packs.Rows.Count; i++)
                {
                    comboBox4.Items.Add(num_packs.Rows[i][0].ToString());
                }

                comboBox4.Items.Add("TODOS");
                */
                consultamysql4.DefaultView.Sort = "FOLIO asc, CREDITO_CUOTA asc";
                consultamysql4 = consultamysql4.DefaultView.ToTable();

                dataGridView5.DataSource = consultamysql4;
                //label8.Text = "Registros Totales: " + consultamysql2.Rows.Count;
                label28.Text = "Registros Totales: " + dataGridView5.RowCount;
                estilo_grid2();

                toolStripStatusLabel1.Text = "Listo";
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel2.Text = "0%";

            //}));
        }

        public void actualizar_fechas()
        {
            Invoke(new MethodInvoker(delegate
            {
                listBox1.Items.Clear();
                datos_a_guardar1.Rows.Clear();
                int porc = 0;

                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView3.Rows[i].Cells[0].Value.ToString()) == true)
                    {
                        //listBox1.Items.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        datos_a_guardar1.Rows.Add(dataGridView3.Rows[i].Cells[1].Value.ToString(), dataGridView3.Rows[i].Cells[8].Value.ToString(), dateTimePicker1.Text);
                    }
                }

                String fecha_ret_not = "", fecha_firma_alta1, fecha_publicacion1, fecha_ini_not1, fecha_fin_not1;
                DateTime fecha_firma_alta, fecha_publicacion, fecha_ini_not, fecha_fin_not;

                fecha_firma_alta = cuenta_dias_hab_reversa(dateTimePicker1.Value, Convert.ToInt32(dias_not)+2);
                fecha_publicacion = cuenta_dias_hab_reversa(dateTimePicker1.Value, Convert.ToInt32(dias_not)+1);
                fecha_ini_not = cuenta_dias_hab_reversa(dateTimePicker1.Value, Convert.ToInt32(dias_not));
                fecha_fin_not = cuenta_dias_hab_reversa(dateTimePicker1.Value, 1);

                fecha_firma_alta1 = fecha_firma_alta.ToShortDateString();
                fecha_publicacion1 = fecha_publicacion.ToShortDateString();
                fecha_ini_not1 = fecha_ini_not.ToShortDateString();
                fecha_fin_not1 = fecha_fin_not.ToShortDateString();

                fecha_firma_alta1 = fecha_firma_alta1.Substring(6, 4) + "-" + fecha_firma_alta1.Substring(3, 2) + "-" + fecha_firma_alta1.Substring(0, 2);
                fecha_publicacion1 = fecha_publicacion1.Substring(6, 4) + "-" + fecha_publicacion1.Substring(3, 2) + "-" + fecha_publicacion1.Substring(0, 2);
                fecha_ini_not1 = fecha_ini_not1.Substring(6, 4) + "-" + fecha_ini_not1.Substring(3, 2) + "-" + fecha_ini_not1.Substring(0, 2);
                fecha_fin_not1 = fecha_fin_not1.Substring(6, 4) + "-" + fecha_fin_not1.Substring(3, 2) + "-" + fecha_fin_not1.Substring(0, 2);

               /* MessageBox.Show("Fecha Alta: " + fecha_firma_alta1+"\n"+
                    "Fecha publi: " + fecha_publicacion1 + "\n" +
                    "Fecha ini_not: " + fecha_ini_not1 + "\n" +
                    "Fecha fin_not: " + fecha_fin_not1 + "\n");*/

                if (datos_a_guardar1.Rows.Count > 0)
                {
                    DialogResult resul = MessageBox.Show("Se colocará la Fecha: " + dateTimePicker1.Text + " como FECHA DE NOTIFICACION a cada uno de los " + datos_a_guardar1.Rows.Count + " créditos marcados.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    if (resul == DialogResult.Yes)
                    {
                        for (int i = 0; i < datos_a_guardar1.Rows.Count; i++)
                        {
                            fecha_ret_not = dateTimePicker1.Text;
                            fecha_ret_not = fecha_ret_not.Substring(6, 4) + "-" + fecha_ret_not.Substring(3, 2) + "-" + fecha_ret_not.Substring(0, 2);
                            conex1.consultar("UPDATE estrados SET fecha_retiro_not=\"" + fecha_ret_not + "\",fecha_fin_not=\"" + fecha_fin_not1 + "\",fecha_inicio_not=\"" + fecha_ini_not1 + "\",fecha_publicacion=\"" + fecha_publicacion1 + "\",fecha_firma_alta=\"" + fecha_firma_alta1 + "\" WHERE folio=\"" + datos_a_guardar1.Rows[i][0] + "\"");

                            toolStripStatusLabel1.Text = "Guardando " + (i + 1) + " de " + datos_a_guardar1.Rows.Count;
                            porc = Convert.ToInt32(((i+1) * 100) / datos_a_guardar1.Rows.Count);
                            toolStripProgressBar1.Value = porc;
                            toolStripStatusLabel2.Text = porc + "%";
                        }

                        MessageBox.Show("Fechas Actualizadas correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

                        toolStripStatusLabel1.Text = "Listo!";                        
                        toolStripProgressBar1.Value = 0;
                        toolStripStatusLabel2.Text = porc + "0%";

                        hilo3 = new Thread(new ThreadStart(cargar_datos_confirma_fechas));
                        hilo3.Start();
                    }
                }
            }));
        }

        public void confirmar_fechas_not()
        {
            Invoke(new MethodInvoker(delegate
            {
                listBox1.Items.Clear();
                datos_a_guardar1.Rows.Clear();
                int porc = 0;
                

                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView3.Rows[i].Cells[0].Value.ToString()) == true)
                    {
                        //listBox1.Items.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        datos_a_guardar1.Rows.Add(dataGridView3.Rows[i].Cells[1].Value.ToString(), dataGridView3.Rows[i].Cells[9].Value.ToString(), dataGridView3.Rows[i].Cells[7].Value.ToString());
                    }
                }

                String fecha_ret_not = "";

                if (datos_a_guardar1.Rows.Count > 0)
                {
                    DialogResult resul = MessageBox.Show("Se colocará el estatus de NOTIFICADO y la FECHA DE NOTIFICACIÓN correspondiente a cada uno de los " + datos_a_guardar1.Rows.Count + " créditos marcados.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    if (resul == DialogResult.Yes)
                    {
                        for (int i = 0; i < datos_a_guardar1.Rows.Count; i++)
                        {
                            fecha_ret_not = datos_a_guardar1.Rows[i][2].ToString();
                            fecha_ret_not = fecha_ret_not.Substring(6, 4) + "-" + fecha_ret_not.Substring(3, 2) + "-" + fecha_ret_not.Substring(0, 2);
                            //MessageBox.Show("UPDATE datos_factura SET fecha_notificacion=\"" + fecha_ret_not + "\", status=\"NOTIFICADO\" WHERE id=\"" + datos_a_guardar1.Rows[i][1] + "\"");
                            conex1.consultar("UPDATE datos_factura SET fecha_notificacion=\"" + fecha_ret_not + "\", status=\"NOTIFICADO\" WHERE id=\"" + datos_a_guardar1.Rows[i][1] + "\"");
                            conex1.guardar_evento("Se marcó como NOTIFICADO el ESTRADO con el Crédito: " + datos_a_guardar1.Rows[i][1].ToString() + " y FOLIO: " + datos_a_guardar1.Rows[i][0].ToString());

                            toolStripStatusLabel1.Text = "Guardando " + (i + 1) + " de " + datos_a_guardar1.Rows.Count;
                            porc = Convert.ToInt32(((i + 1) * 100) / datos_a_guardar1.Rows.Count);
                            toolStripProgressBar1.Value = porc;
                            toolStripStatusLabel2.Text = porc + "%";
                        }

                        MessageBox.Show("Estatus y Fecha de Notificación Actualizados correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);

                        toolStripStatusLabel1.Text = "Listo!";
                        toolStripProgressBar1.Value = 0;
                        toolStripStatusLabel2.Text = porc + "0%";

                        hilo3 = new Thread(new ThreadStart(cargar_datos_confirma_fechas));
                        hilo3.Start();
                    }
                }
            }));
        }

        public void guardar_datos()
        {
            Invoke(new MethodInvoker(delegate
            {
                listBox1.Items.Clear();
                datos_a_guardar.Rows.Clear();
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value.ToString()) == true)
                    {
                        //listBox1.Items.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        datos_a_guardar.Rows.Add(dataGridView1.Rows[i].Cells[8].Value.ToString(), dataGridView1.Rows[i].Cells[9].Value.ToString(), dataGridView1.Rows[i].Cells[10].Value.ToString(), dataGridView1.Rows[i].Cells[12].Value.ToString());
                    }
                }

                if (datos_a_guardar.Rows.Count > 0)
                {

                    DialogResult resul = MessageBox.Show("Se van a Registrar como Estrados y se les va a asignar un Folio a cada uno de los " + datos_a_guardar.Rows.Count + " créditos marcados.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    if (resul == DialogResult.Yes)
                    {
                        //datos
                        String fojas, supuesto, motivo, observaciones, folio_act, fecha;
                        //fechas
                        String fecha_doc, fecha_act, fecha_firm, fecha_publi, fecha_ini_not, fecha_fin_not, fecha_ret_not, sql;
                        //numeros
                        int num_fol = 0, cons = 0,porcentaje=0,num_pack=0;

                        datos_para_usuario.Rows.Clear();

                        //fecha_doc = fech_doc;
                        fecha_act = dateTimePicker3.Text;
                        fecha_firm = dateTimePicker3.Text;
                        fecha_publi = dateTimePicker4.Text;
                        fecha_ini_not = dateTimePicker5.Text;
                        fecha_fin_not = dateTimePicker6.Text;
                        fecha_ret_not = dateTimePicker7.Text;

                        //fecha_doc = fecha_doc.Substring(6, 4) + "-" + fecha_doc.Substring(3, 2) + "-" + fecha_doc.Substring(0, 2);
                        fecha_act = fecha_act.Substring(6, 4) + "-" + fecha_act.Substring(3, 2) + "-" + fecha_act.Substring(0, 2);
                        fecha_firm = fecha_firm.Substring(6, 4) + "-" + fecha_firm.Substring(3, 2) + "-" + fecha_firm.Substring(0, 2);
                        fecha_publi = fecha_publi.Substring(6, 4) + "-" + fecha_publi.Substring(3, 2) + "-" + fecha_publi.Substring(0, 2);
                        fecha_ini_not = fecha_ini_not.Substring(6, 4) + "-" + fecha_ini_not.Substring(3, 2) + "-" + fecha_ini_not.Substring(0, 2);
                        fecha_fin_not = fecha_fin_not.Substring(6, 4) + "-" + fecha_fin_not.Substring(3, 2) + "-" + fecha_fin_not.Substring(0, 2);
                        fecha_ret_not = fecha_ret_not.Substring(6, 4) + "-" + fecha_ret_not.Substring(3, 2) + "-" + fecha_ret_not.Substring(0, 2);

                        fojas = "10";
                        supuesto = "No es localizable en el domicilio fiscal que señalo";
                        motivo = "REVISAR ACTA CIRCUNSTANCIADA DE HECHOS";
                        observaciones = "-";
                        fecha = System.DateTime.Today.ToShortDateString();
                        fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);

                        dataGridView1.Enabled = false;

                        conex2.conectar("base_principal");
                        conex.conectar("base_principal");

                        consultamysql3 = conex2.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC LIMIT 1");

                        if (consultamysql3.Rows.Count > 0)
                        {
                            num_pack = Convert.ToInt32(consultamysql3.Rows[0][0].ToString());
                            num_pack = num_pack + 1;
                        }
                        else
                        {
                            num_pack = 1;
                        }

                        for (int i = 0; i < datos_a_guardar.Rows.Count; i++)
                        {
                            consultamysql_verifica.Rows.Clear();
                            //verificar que no haya duplicados
                            consultamysql_verifica = conex0.consultar("SELECT id_credito FROM estrados WHERE id_credito=\"" + datos_a_guardar.Rows[i][0].ToString() + "\"");

                            if (consultamysql_verifica.Rows.Count > 0)
                            {//SI HAY DUPLICADOS
                                //algo
                            }
                            else
                            {
                                //verificar folio
                                consultamysql_verifica = conex0.consultar("SELECT folio FROM estrados ORDER BY id_estrados DESC LIMIT 1 ");
                                folio_act = "";
                                if (consultamysql_verifica.Rows.Count > 0)
                                {
                                    folio_act = consultamysql_verifica.Rows[0][0].ToString();
                                    if ((Convert.ToInt32(folio_act.Substring(6, 2))) < (Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2, 2))))
                                    {
                                        folio_act = "E" + del_num + "" + sub_num + "/" + (Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2, 2))) + "-00001";
                                    }
                                    else
                                    {
                                        num_fol = (Convert.ToInt32(folio_act.Substring(9,folio_act.Length-9)));
                                        num_fol = num_fol + 1;
                                        if (num_fol < 10)
                                        {
                                            folio_act = folio_act.Substring(0, 9) + "0000" + num_fol.ToString();
                                        }
                                        else
                                        {
                                            if (num_fol < 100)
                                            {
                                                folio_act = folio_act.Substring(0, 9) + "000" + num_fol.ToString();
                                            }
                                            else
                                            {
                                                if (num_fol < 1000)
                                                {
                                                    folio_act = folio_act.Substring(0, 9) + "00" + num_fol.ToString();
                                                }
                                                else
                                                {
                                                    if (num_fol < 10000)
                                                    {
                                                        folio_act = folio_act.Substring(0, 9) + "0" + num_fol.ToString();
                                                    }
                                                    else
                                                    {
                                                        folio_act = folio_act.Substring(0, 9) + num_fol.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    folio_act = "E" + del_num + "" + sub_num + "/" + (Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2, 2))) + "-0000";
                                }

                                //GUARDAR ESTRADOS
                                fecha_doc = datos_a_guardar.Rows[i][3].ToString();
                                fecha_doc = fecha_doc.Substring(6, 4) + "-" + fecha_doc.Substring(3, 2) + "-" + fecha_doc.Substring(0, 2);
                                sql = "INSERT INTO estrados (folio,id_credito,nombre_documento,fecha_emision_doc,fecha_acta_circunstanciada,fojas,sub_emisora,titular_sub,supuesto_estrados,motivo_estrados,fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,hora_not,observaciones,notificador_estrados,paquete)" +
                                     "VALUES(\"" + folio_act + "\"," + datos_a_guardar.Rows[i][0].ToString() + ",\"" + nombre_docto_full(datos_a_guardar.Rows[i][1].ToString(), datos_a_guardar.Rows[i][2].ToString()) + "\",\"" + fecha_doc + "\",\"" + fecha_act + "\"," + fojas + ",\"" + sub_nom + "\",\"" + subdele + "\",\"" + supuesto + "\",\"" + motivo + "\",\"" + fecha_firm + "\",\"" + fecha_publi + "\",\"" + fecha_ini_not + "\",\"" + fecha_fin_not + "\",\"" + fecha_ret_not + "\",\"" + maskedTextBox3.Text + "\",\"" + observaciones + "\"," + id_user + ","+num_pack+")";

                                conex.consultar(sql);
                                conex.guardar_evento("Se Ingresa el Estrado con el FOLIO: " + folio_act);

                                sql = "UPDATE datos_factura SET nn=\"ESTRADOS\",fecha_estrados=\"" + fecha + "\",estado_cartera=\"-\" WHERE id=" + datos_a_guardar.Rows[i][0].ToString() + ";";
                                conex.consultar(sql);
                                conex.guardar_evento("Se marcó como ESTRADOS el Crédito con el ID: " + datos_a_guardar.Rows[i][0].ToString());

                                cons++;
                                datos_para_usuario.Rows.Add(cons.ToString(), folio_act, datos_a_guardar.Rows[i][0].ToString(), fecha_doc, nombre_docto_full(datos_a_guardar.Rows[i][1].ToString(), datos_a_guardar.Rows[i][2].ToString()), supuesto, motivo, fecha_act, fojas, fecha_firm, fecha_publi, fecha_ini_not, fecha_fin_not, fecha_ret_not, id_user, subdele, sub_nom, maskedTextBox3.Text, observaciones,num_pack);

                                porcentaje = Convert.ToInt32(((i + 1) * 100) / datos_a_guardar.Rows.Count);
                                toolStripStatusLabel1.Text = (i + 1) + "/" + datos_a_guardar.Rows.Count;
                                toolStripStatusLabel2.Text = porcentaje.ToString() + "%";
                                if (porcentaje < 100)
                                {                                    
                                    toolStripProgressBar1.Value = porcentaje;
                                }
                            }
                        }//fin for

                        toolStripStatusLabel1.Text = datos_a_guardar.Rows.Count + "/" + datos_a_guardar.Rows.Count;
                        toolStripStatusLabel1.Text = "100%";                        
                        toolStripProgressBar1.Value = 100;

                        if (datos_para_usuario.Rows.Count > 0)
                        {

                            //GUARDAR EVIDENCIA ESTRADOS
                            SaveFileDialog dialog_save = new SaveFileDialog();
                            dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                            dialog_save.Title = "Guardar Archivo de Evidencia de Estrados";//le damos un titulo a la ventana

                            if (dialog_save.ShowDialog() == DialogResult.OK)
                            {
                                //tabla_excel
                                XLWorkbook wb = new XLWorkbook();
                                wb.Worksheets.Add(datos_para_usuario, "ESTRADOS_AGREGADOS");
                                wb.SaveAs(@"" + dialog_save.FileName + "");
                                //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                MessageBox.Show("Los Estrados se han Guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                                cargar_datos_nn();//recargar grid
                                dataGridView1.Enabled = true;

                                //conex2.conectar("base_principal");
                                consultamysql3 = conex2.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC LIMIT 1");

                                if (consultamysql3.Rows.Count > 0)
                                {
                                    num_pack = Convert.ToInt32(consultamysql3.Rows[0][0].ToString());
                                    num_pack = num_pack + 1;
                                }
                                else
                                {
                                    num_pack = 1;
                                }

                                label12.Text = "Sig. Paquete: " + num_pack;
                                conex2.cerrar();
                            }
                        }

                    }
                }
            }));
        }        

        public string nombre_docto_full(String tipo_doc, String nom_per)
        {
            String nom_docto_txt = "";
            switch (tipo_doc)
            {
                case "0": nom_docto_txt = "CÉDULA DE LIQUIDACIÓN DE CAPITALES CONSTITUTIVOS";
                    break;
                case "2": nom_docto_txt="CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACIÓN Y PAGO CUOTAS";
                    break;
                case "3":
                    if (nom_per.StartsWith("COP") == true)
                    {
                        nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACIÓN Y PAGO DE CUOTAS";
                    }
                    else
                    {
                        nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACIÓN Y PAGO DE CUOTAS CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
                    }
                    break;
                case "6": nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACIÓN Y PAGO DE CUOTAS CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
                    break;

                case "89": nom_docto_txt = "RESOLUCIÓN PARA CUBRIR LA PRIMA MEDIA";
                    break;

                default: nom_docto_txt = "DOCUMENTO SIN NOMBRE ESPECIFICADO";
                    break;
            }

            return nom_docto_txt;
        }

        public void carga_chema_excel()
        {
            int i = 0;
            int filas = 0;
            String tabla;

            comboBox3.Items.Clear();
            System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            dataGridView2.DataSource = dt;
            filas = dataGridView2.RowCount;
            do
            {
                if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals(""))
                {
                    if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE"))
                    {
                        tabla = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        if ((tabla.Substring((tabla.Length - 1), 1)).Equals("$"))
                        {
                            tabla = tabla.Remove((tabla.Length - 1), 1);
                            comboBox3.Items.Add(tabla);
                        }
                    }
                }
                i++;
            } while (i < filas);

            dt.Clear();
            dataGridView2.DataSource = dt; //vaciar datagrid
            i = 0;
        }

        public void cargar_hoja_excel_procesar()
        {
            String hoja = "",cons_exc;
            comboBox3.SelectedIndex = 0;
            hoja = comboBox3.SelectedItem.ToString();

            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                cons_exc = "Select * From [" + hoja + "$]";

                try
                {
                    //Si el usuario escribio el nombre de la hoja se procedera con la busqueda
                    dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
                    dataSet = new DataSet(); // creamos la instancia del objeto DataSet

                    if (dataAdapter.Equals(null))
                    {

                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");

                    }
                    else
                    {
                        if (dataAdapter == null) { }
                        else
                        {
                            dataAdapter.Fill(dataSet, hoja);//llenamos el dataset

                            //dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet                            
                            data_acumulador.Merge(dataSet.Tables[0]);
                            //MessageBox.Show("" + data_acumulador.Rows.Count+"\n hojas: "+comboBox3.Items.Count+"\n hoja: "+hoja);
                            //conexion.Close();//cerramos la conexion
                            //dataGridView2.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega

                        }
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja.\n\n" + ex, "Error al Abrir Archivo de Excel");
                }

            }

        }

        public void marcar_fila_manual(int indice)
        {
            int cont = 1;

            dataGridView1[0, indice].Value = true;

            do{
                dataGridView1.Rows[indice].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                cont++;
            }while (cont <= 8);
        }

        public void marcar_desde_excel()
        {
            int marca = 0;

            //MessageBox.Show(""+data_acumulador.Rows.Count);
            for (int i = 0; i < data_acumulador.Rows.Count;i++)
            {
                for (int j = 0; j < dataGridView1.RowCount;j++ )
                {
                    //MessageBox.Show("" + data_acumulador.Rows[i][0].ToString().ToUpper() + "=" + dataGridView1[0, j].Value.ToString().ToUpper());
                                                                //registro_patronal                                                                                      credito_cuota                                                                                          credito_multa
                    if ((data_acumulador.Rows[i][0].ToString().ToUpper() == dataGridView1[1, j].Value.ToString().ToUpper()) && (data_acumulador.Rows[i][1].ToString().ToUpper() == dataGridView1[3, j].Value.ToString().ToUpper()) && (data_acumulador.Rows[i][2].ToString().ToUpper() == dataGridView1[4, j].Value.ToString().ToUpper()))
                    {
                        marcar_fila_manual(j);
                        j = dataGridView1.RowCount + 1;
                        marca++;
                    }
                }
            }

            MessageBox.Show("Se marcaron exitosamente "+marca+" registros", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void guardar_fechas()
        {
            Invoke(new MethodInvoker(delegate
            {
                //fechas
                String fecha_firm, fecha_publi, fecha_ini_not, fecha_fin_not, fecha_ret_not, sql="";
                //numeros
                int tot = 0, porc = 0, k=0;
                               
                fecha_firm = dateTimePicker11.Text;
                fecha_publi = dateTimePicker10.Text;
                fecha_ini_not = dateTimePicker9.Text;
                fecha_fin_not = dateTimePicker8.Text;
                fecha_ret_not = dateTimePicker2.Text;
                
                fecha_firm = fecha_firm.Substring(6, 4) + "-" + fecha_firm.Substring(3, 2) + "-" + fecha_firm.Substring(0, 2);
                fecha_publi = fecha_publi.Substring(6, 4) + "-" + fecha_publi.Substring(3, 2) + "-" + fecha_publi.Substring(0, 2);
                fecha_ini_not = fecha_ini_not.Substring(6, 4) + "-" + fecha_ini_not.Substring(3, 2) + "-" + fecha_ini_not.Substring(0, 2);
                fecha_fin_not = fecha_fin_not.Substring(6, 4) + "-" + fecha_fin_not.Substring(3, 2) + "-" + fecha_fin_not.Substring(0, 2);
                fecha_ret_not = fecha_ret_not.Substring(6, 4) + "-" + fecha_ret_not.Substring(3, 2) + "-" + fecha_ret_not.Substring(0, 2);

                for (int j = 0; j < dataGridView5.Rows.Count;j++ )
                {
                    //MessageBox.Show(dataGridView5[0, j].Value.ToString());
                    if (Convert.ToBoolean(dataGridView5[0, j].Value.ToString()) == true)
                    {
                        tot++;
                    }
                }

                DialogResult resul = MessageBox.Show("Se actualizaran las fechas de "+tot+" registros.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                conex.conectar("base_principal");
                if (resul == DialogResult.Yes)
                {
                    if(dataGridView5.Rows.Count > 0){
                        for (int i = 0; i < dataGridView5.Rows.Count;i++)
                        {
                            if (Convert.ToBoolean(dataGridView5[0, i].Value.ToString()) == true)
                            {
                                sql = "UPDATE estrados SET fecha_firma_alta=\"" + fecha_firm + "\",fecha_publicacion=\"" + fecha_publi + "\",fecha_inicio_not=\"" + fecha_ini_not + "\",fecha_fin_not=\"" + fecha_fin_not + "\",fecha_retiro_not=\"" + fecha_ret_not + "\",hora_not=\"" + maskedTextBox5.Text + "\" WHERE folio=\""+dataGridView5[1,i].Value.ToString()+"\"";
                                conex.consultar(sql);

                                conex.guardar_evento("Se editaron las fechas del estrado con Folio: " + dataGridView5[1, i].Value.ToString());

                                toolStripStatusLabel1.Text = "Cargando " + (k + 1) + " de " + tot;
                                porc = Convert.ToInt32(((k + 1) * 100) / tot);
                                toolStripProgressBar1.Value = porc;
                                toolStripStatusLabel2.Text = porc + "%";
                                k++;
                            }
                        }
                    }

                    toolStripProgressBar1.Value = 100;
                    toolStripStatusLabel2.Text = "100%";
                    toolStripStatusLabel1.Text = "Listo";
                    MessageBox.Show("Fechas Actualizadas correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    if(dataGridView5.RowCount>0){
                        //dataGridView5.Rows.Clear();
                        dataGridView5.DataSource = null;
                    }
                }
                conex.cerrar();
            }));
        }

        public bool verificar_paquetes(string nom_pack)
        {
            Conexion conex5 = new Conexion();
            Conexion conex6 = new Conexion();
            DataTable verifi_pack = new DataTable();
            DataTable verifi_pack2 = new DataTable();

            int tot = 0;

            conex5.conectar("base_principal");
            conex6.conectar("base_principal");
            verifi_pack = conex5.consultar("SELECT id_credito FROM estrados WHERE paquete=" + nom_pack + "");

            for (int i = 0; i < verifi_pack.Rows.Count; i++)
            {
                verifi_pack2 = conex6.consultar("SELECT id FROM datos_factura WHERE id= " + verifi_pack.Rows[i][0].ToString() + " and status=\"EN TRAMITE\"");
                if (verifi_pack2.Rows.Count > 0)
                {
                    tot++;
                    //MessageBox.Show("" + verifi_pack.Rows[i][0].ToString());
                }
            }

            conex5.cerrar();
            conex6.cerrar();

            if (tot > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable copiar_datagrid_nn()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView1.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView1.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView1.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView1.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        public DataTable copiar_datagrid_fechas()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView5.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView5.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView5.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView5.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView5.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }
        //CARGAR
        private void button5_Click(object sender, EventArgs e)
        {
            cargar_datos_nn();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex != 0)
            {
                int cont = 1;
                //if(dataGridView1.CurrentRow.Cells[0].Value != null){
                bool marca = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[0].Value);
                //}else{
                //	marca=false;
                //}

                if (marca == false)
                {
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;                        
                        cont++;
                    } while (cont <= 8);
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = true;                   
                }
                else
                {
                    cont = 1;
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;
                        cont++;
                    } while (cont <= 8);
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = false;

                    
                }
            }
            //checar_marcados();
            //checar_marcados_listbox();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                int cont = 1;
                bool marca;

                if (dataGridView1.CurrentRow.Cells[0].Value != null)
                {
                    marca = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[0].Value);
                }
                else
                {
                    marca = false;
                }

                if (marca == true)
                {
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;                        
                        cont++;
                    } while (cont <= 8);
                   
                }
                else
                {
                    cont = 1;
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;                        
                        cont++;
                    } while (cont <= 8);
                    
                }
            }
        }
        //GUARDAR
        private void button4_Click(object sender, EventArgs e)
        {
            hilo2 = new Thread(new ThreadStart(guardar_datos));
            hilo2.Start();
        }
        //BUSCAR CREDITO
        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            int foun = 0;
            if(dataGridView1.RowCount>0){
                if(maskedTextBox1.Text.Length==9){
                    for (int i = 0; i < dataGridView1.RowCount;i++)
                    {
                        if(dataGridView1.Rows[i].Cells[3].Value.ToString()==maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[3].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            //dataGridView1.Rows[i].Cells[0].Value = true;
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                            foun = 1;                            
                        }

                        if (dataGridView1.Rows[i].Cells[4].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[4].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                            //dataGridView1.Rows[i].Cells[0].Value = true;
                            foun = 1;
                        }

                        if (foun == 1)
                        {                            
                            i = dataGridView1.RowCount + 1;
                        }
                    }

                    if(foun==0){
                        MessageBox.Show("Crédito no Encontrado", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        maskedTextBox1.SelectionStart = 0;
                        maskedTextBox1.Focus();
                    }

                    maskedTextBox1.Clear();
                }
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker4.Value = cuenta_dias_hab(dateTimePicker3.Value, 1);
            dateTimePicker5.Value = cuenta_dias_hab(dateTimePicker3.Value, 2);
            dateTimePicker6.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(dias_not)+1));
            dateTimePicker7.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(dias_not)+2));
        }

        private void dateTimePicker3_Leave(object sender, EventArgs e)
        {
            dateTimePicker4.Value = cuenta_dias_hab(dateTimePicker3.Value, 1);
            dateTimePicker5.Value = cuenta_dias_hab(dateTimePicker3.Value, 2);
            dateTimePicker6.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(dias_not) + 1));
            dateTimePicker7.Value = cuenta_dias_hab(dateTimePicker3.Value, (Convert.ToInt32(dias_not) + 2));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String hor_box, min_box;
            hor_box = "" + DateTime.Now.Hour;
            min_box = "" + DateTime.Now.Minute;

            if (hor_box.Length == 1)
            {
                hor_box = "0" + hor_box;
            }

            if (min_box.Length == 1)
            {
                min_box = "0" + min_box;
            }

            maskedTextBox3.Text = hor_box + min_box;
            maskedTextBox5.Text = hor_box + min_box;
           
        }
        //ENTRAR A CONFIRMAR FECHA NOTIFICACION
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Today;
            String fecha_hoy = fecha.Year.ToString();
            panel2.Show();
            maskedTextBox4.Text = "E" + del_num.ToString() + sub_num.ToString()+(fecha_hoy.Substring(0,2));
            //llenar_Cb2();

            if (datos_a_guardar1.Columns.Contains("id_credito") == false)
            {
                datos_a_guardar1.Columns.Add("id_credito");
                datos_a_guardar1.Columns.Add("folio");
                datos_a_guardar1.Columns.Add("fech_not");
            }

            if(num_packs.Columns.Contains("num")==false){
                num_packs.Columns.Add("num");
            }

            hilo2 = new Thread(new ThreadStart(cargar_datos_confirma_fechas));
            hilo2.Start();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            int foun = 0;
            if (dataGridView3.RowCount > 0)
            {
                if (maskedTextBox2.Text.Length == 9)
                {
                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        if (dataGridView3.Rows[i].Cells[5].Value.ToString() == maskedTextBox2.Text.ToString())
                        {
                            dataGridView3.Focus();
                            dataGridView3.Rows[i].Cells[5].Selected = true;
                            dataGridView3.FirstDisplayedScrollingRowIndex = i;
                            //dataGridView1.Rows[i].Cells[0].Value = true;
                            dataGridView3.CurrentCell = dataGridView3.Rows[i].Cells[0];
                            foun = 1;
                        }

                        if (dataGridView3.Rows[i].Cells[6].Value.ToString() == maskedTextBox2.Text.ToString())
                        {
                            dataGridView3.Focus();
                            dataGridView3.Rows[i].Cells[6].Selected = true;
                            dataGridView3.FirstDisplayedScrollingRowIndex = i;
                            dataGridView3.CurrentCell = dataGridView3.Rows[i].Cells[0];
                            //dataGridView1.Rows[i].Cells[0].Value = true;
                            foun = 1;
                        }

                        if (foun == 1)
                        {
                            i = dataGridView3.RowCount + 1;
                        }
                    }

                    if (foun == 0)
                    {
                        MessageBox.Show("Crédito no Encontrado", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        maskedTextBox2.SelectionStart = 0;
                        maskedTextBox2.Focus();
                    }

                    maskedTextBox2.Clear();
                }
            }
        }

        private void maskedTextBox4_TextChanged(object sender, EventArgs e)
        {
           // if(maskedTextBox4.Mask.Length<1)
           // {
                DateTime fecha = DateTime.Today;
                String fecha_hoy = fecha.Year.ToString(), text_folio = "";
                int foun = 0;

                text_folio = maskedTextBox4.Text;
                //MessageBox.Show("|"+text_folio+"|");

                if (text_folio == "E    /  -")
                {
                    maskedTextBox4.Text = "E" + del_num.ToString() + sub_num.ToString() + (fecha_hoy.Substring(0, 2));
                }

                if(maskedTextBox4.MaskCompleted==true){
                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        if (dataGridView3.Rows[i].Cells[1].Value.ToString() == maskedTextBox4.Text.ToString())
                        {
                            dataGridView3.Focus();
                            dataGridView3.Rows[i].Cells[1].Selected = true;
                            dataGridView3.FirstDisplayedScrollingRowIndex = i;
                            //dataGridView1.Rows[i].Cells[0].Value = true;
                            dataGridView3.CurrentCell = dataGridView3.Rows[i].Cells[0];
                            foun = 1;
                        }

                        if (foun == 1)
                        {
                            i = dataGridView3.RowCount + 1;
                        }
                    }

                    if (foun == 0)
                    {
                        MessageBox.Show("Crédito no Encontrado", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        maskedTextBox4.SelectionStart = 0;
                        maskedTextBox4.Focus();
                    }

                    maskedTextBox4.Clear();
                }
           // }
        }
        //ACTUALIZAR FECHAS
        private void button7_Click(object sender, EventArgs e)
        {
            hilo2 = new Thread(new ThreadStart(actualizar_fechas));
            hilo2.Start();
        }
        //CONFIRMAR FECHAS
        private void button6_Click_1(object sender, EventArgs e)
        {
            hilo2 = new Thread(new ThreadStart(confirmar_fechas_not));
            hilo2.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            consultamysql2.DefaultView.Sort = "FOLIO asc, CREDITO_CUOTA asc";
            consultamysql2 = consultamysql2.DefaultView.ToTable();

            if(comboBox2.SelectedItem.ToString() != "TODOS"){                
                DataView dv = new DataView(consultamysql2);
                dv.RowFilter = "NUM_PAQUETE = '" + comboBox2.SelectedItem + "'";
                dataGridView3.DataSource = dv.ToTable();                
            }else{
                dataGridView3.DataSource = consultamysql2;
            }
            label8.Text = "Registros Totales: " + dataGridView3.RowCount;
            estilo_grid1();
        }

        private void marcarTodosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1[0, i].Value = true;
                }
            }

            if(dataGridView3.RowCount > 0)
            {
                for (int i = 0; i < dataGridView3.RowCount;i++)
                {
                    dataGridView3[0,i].Value = true;
                }
            }

            if (dataGridView5.RowCount > 0)
            {
                for (int i = 0; i < dataGridView5.RowCount; i++)
                {
                    dataGridView5[0, i].Value = true;
                }
            }
        }

        private void desmarcarTodosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1[0, i].Value = false;
                }
            }

            if (dataGridView3.RowCount > 0)
            {
                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    dataGridView3[0, i].Value = false;
                }
            }

            if (dataGridView5.RowCount > 0)
            {
                for (int i = 0; i < dataGridView5.RowCount; i++)
                {
                    dataGridView5[0, i].Value = false;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount>0){
                OpenFileDialog dialog = new OpenFileDialog();
                String cad_con="";

                dialog.Title = "Seleccione el archivo con los registros de Estrados a marcar";//le damos un titulo a la ventana
                dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque

                dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + dialog.FileName + "';Extended Properties=Excel 12.0;";
                    conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                    conexion.Open(); //abrimos la conexion
                    data_acumulador.Clear();
                    carga_chema_excel();
                    /*int j = 0;
                    while (j < comboBox3.Items.Count)
                    {
                        comboBox3.SelectedIndex = j;*/
                        cargar_hoja_excel_procesar();
                        //j++;
                    //}

                    marcar_desde_excel();
                }
            }
        }
        //ENTRAR EDITAR FECHAS
        private void button15_Click(object sender, EventArgs e)
        {
            conex0.conectar("base_principal");
            tabla_dias_fest = conex0.consultar("SELECT dia FROM dias_festivos");
            
            dateTimePicker10.Value = cuenta_dias_hab(dateTimePicker11.Value, 1);
            dateTimePicker9.Value = cuenta_dias_hab(dateTimePicker11.Value, 2);
            dateTimePicker8.Value = cuenta_dias_hab(dateTimePicker11.Value, (Convert.ToInt32(dias_not) + 1));
            dateTimePicker2.Value = cuenta_dias_hab(dateTimePicker11.Value, (Convert.ToInt32(dias_not) + 2));
            
            llenar_Cb4();

            timer1.Enabled = true;

            panel3.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            try
            {
                dataGridView5.DataSource = null;
            }
            catch (Exception ew) { }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if(comboBox4.SelectedIndex > -1){
                cargar_datos_edita_fechas();
            }
        }

        private void dateTimePicker11_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker10.Value = cuenta_dias_hab(dateTimePicker11.Value, 1);
            dateTimePicker9.Value = cuenta_dias_hab(dateTimePicker11.Value, 2);
            dateTimePicker8.Value = cuenta_dias_hab(dateTimePicker11.Value, (Convert.ToInt32(dias_not) + 1));
            dateTimePicker2.Value = cuenta_dias_hab(dateTimePicker11.Value, (Convert.ToInt32(dias_not) + 2));
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker11.Value = cuenta_dias_hab_reversa(dateTimePicker2.Value, (Convert.ToInt32(dias_not) + 2));
            dateTimePicker10.Value = cuenta_dias_hab_reversa(dateTimePicker2.Value, (Convert.ToInt32(dias_not) + 1));
            dateTimePicker9.Value = cuenta_dias_hab_reversa(dateTimePicker2.Value, (Convert.ToInt32(dias_not)));
            dateTimePicker8.Value = cuenta_dias_hab_reversa(dateTimePicker2.Value, 1);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            hilo2 = new Thread(new ThreadStart(guardar_fechas));
            hilo2.Start();
        }

        private void dataGridView5_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridView5_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int cont = 0;
            while (cont < consultamysql.Rows.Count)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
            }
        }

        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int cont = 0;
            while (cont < consultamysql.Rows.Count)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
            }
        }

        private void exportarAExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataGridView1.RowCount > 0)
            {
                //GUARDAR EVIDENCIA ESTRADOS
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    DataTable nn_excel = copiar_datagrid_nn();

                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(nn_excel, "Lista_de_NN");
                    wb.SaveAs(@"" + dialog_save.FileName + "");
                    //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);                    
                }
            }

            if (dataGridView5.RowCount > 0)
            {
                //GUARDAR EVIDENCIA ESTRADOS
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Evidencia de Estrados";//le damos un titulo a la ventana

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    DataTable estrados_excel = copiar_datagrid_fechas();

                    estrados_excel.Columns.Add("INFO_COMBINADA");

                    int col_info = 0, tot_let=0;
                    String num = "";

                    col_info = estrados_excel.Columns.Count - 1;
                    tot_let = (estrados_excel.Rows.Count.ToString()).Length;

                    for (int i = 0; i < estrados_excel.Rows.Count; i++)
                    {
                        num = (i+1).ToString();

                        if (tot_let > 1)
                        {
                            while (num.Length < tot_let)
                            {
                                num = "0" + num;
                            }
                        }
                        else
                        {
                            num = "0" + num;
                        }

                        estrados_excel.Rows[i][col_info] = "PAQUETE-" + estrados_excel.Rows[i][7].ToString() + 
                            "_" + (num) + 
                            "_" + estrados_excel.Rows[i][2].ToString() + 
                            "_" + estrados_excel.Rows[i][5].ToString() + 
                            "_" + estrados_excel.Rows[i][6].ToString() + 
                            "_CREDITO_" + (estrados_excel.Rows[i][1].ToString()).Substring((estrados_excel.Rows[i][1].ToString().Length-5),5);
                    }

                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(estrados_excel, "Lista_de_Estrados");
                    wb.SaveAs(@"" + dialog_save.FileName + "");
                    //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);                    
                }
            }

            if (dataGridView3.RowCount > 0)
            {
                MessageBox.Show("Esta Funcionalidad no está Implementada", "AVISO");
            }
        }

        private void dateTimePicker7_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, (Convert.ToInt32(dias_not) + 2));
            dateTimePicker4.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, (Convert.ToInt32(dias_not) + 1));
            dateTimePicker5.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, Convert.ToInt32(dias_not));
            dateTimePicker6.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, 1);
        }

        private void dateTimePicker7_Leave(object sender, EventArgs e)
        {
            dateTimePicker3.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, (Convert.ToInt32(dias_not) + 2));
            dateTimePicker4.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, (Convert.ToInt32(dias_not) + 1));
            dateTimePicker5.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, Convert.ToInt32(dias_not));
            dateTimePicker6.Value = cuenta_dias_hab_reversa(dateTimePicker7.Value, 1);
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}

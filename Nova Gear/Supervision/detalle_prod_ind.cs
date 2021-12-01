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
    public partial class detalle_prod_ind : Form
    {
        public detalle_prod_ind()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();
        Conexion conex1 = new Conexion();
        Conexion conex2= new Conexion();
        Conexion conex3 = new Conexion();


        DataTable tabla_notis = new DataTable();
        DataTable tabla_detalle = new DataTable();
        DataTable tabla_estrados = new DataTable();
        DataTable tabla_nn = new DataTable();
        DataTable tabla_status = new DataTable();

        public void llena_noti(){
            conex1.conectar("base_principal");
            tabla_notis = conex1.consultar("SELECT DISTINCT(notificador) FROM datos_factura ORDER BY notificador ASC");

            for (int i = 0; i < tabla_notis.Rows.Count;i++)
            {
                comboBox1.Items.Add(tabla_notis.Rows[i][0].ToString());
            }

            comboBox1.SelectedIndex = 0;
            conex1.cerrar();
        }

        public void consulta_detalle()
        {
            String sql = "", sql2 = "", sql3 = "", sql4 = "", noti = "", fecha_desde = "", fecha_hasta = "";
            int tot_estra = 0, tot_nn = 0, tot=0, menos=0;

            conex.conectar("base_principal");
            conex1.conectar("base_principal");
            conex2.conectar("base_principal");
            conex3.conectar("base_principal");

            noti=comboBox1.SelectedItem.ToString();
            fecha_desde=dateTimePicker1.Value.ToShortDateString();
            fecha_hasta=dateTimePicker2.Value.ToShortDateString();

            fecha_desde = fecha_desde.Substring(6, 4) + "-" + fecha_desde.Substring(3, 2) + "-" + fecha_desde.Substring(0, 2);
            fecha_hasta = fecha_hasta.Substring(6, 4) + "-" + fecha_hasta.Substring(3, 2) + "-" + fecha_hasta.Substring(0, 2);

            sql="SELECT fecha_notificacion, COUNT(id) AS \"Total\" FROM datos_factura WHERE notificador=\""+noti+"\" AND fecha_notificacion IS NOT NULL AND (fecha_notificacion BETWEEN \""+fecha_desde+"\" AND \""+fecha_hasta+"\") GROUP BY fecha_notificacion ORDER BY fecha_notificacion ASC";
            //MessageBox.Show(sql);
            sql2 = "SELECT status, COUNT(id) AS \"Total\" FROM datos_factura WHERE notificador=\"" + noti + "\" AND fecha_entrega >= \"" + fecha_desde + "\" AND fecha_recepcion <=\"" + fecha_hasta + "\" GROUP BY status ORDER BY status ASC";

            sql3 = "SELECT status, COUNT(id) AS \"Total\" FROM datos_factura WHERE notificador=\"" + noti + "\" AND fecha_entrega >= \"" + fecha_desde + "\" AND fecha_recepcion <=\"" + fecha_hasta + "\" AND NN=\"ESTRADOS\" GROUP BY status ORDER BY status ASC";

            sql4 = "SELECT status, COUNT(id) AS \"Total\" FROM datos_factura WHERE notificador=\"" + noti + "\" AND fecha_entrega >= \"" + fecha_desde + "\" AND fecha_recepcion <=\"" + fecha_hasta + "\" AND NN=\"nn\" GROUP BY status ORDER BY status ASC";

            tabla_detalle = conex.consultar(sql);
            tabla_status = conex1.consultar(sql2);
            tabla_estrados = conex2.consultar(sql3);
            tabla_nn = conex3.consultar(sql4);

            for (int i = 0; i<tabla_estrados.Rows.Count;i++)
            {
                for (int j = 0; j < tabla_status.Rows.Count;j++)
                {
                    if (tabla_estrados.Rows[i][0].ToString() == tabla_status.Rows[j][0].ToString())
                    {
                        tot = Convert.ToInt32(tabla_status.Rows[j][1].ToString());
                        menos = Convert.ToInt32(tabla_estrados.Rows[i][1].ToString());
                        tot_estra = tot_estra + menos;
                        tabla_status.Rows[j][1] = (tot - menos).ToString();
                    }
                }
            }

            for (int i = 0; i < tabla_nn.Rows.Count; i++)
            {
                for (int j = 0; j < tabla_status.Rows.Count; j++)
                {
                    if (tabla_nn.Rows[i][0].ToString() == tabla_status.Rows[j][0].ToString())
                    {
                        tot = Convert.ToInt32(tabla_status.Rows[j][1].ToString());
                        menos = Convert.ToInt32(tabla_nn.Rows[i][1].ToString());
                        tot_nn = tot_nn + menos;
                        tabla_status.Rows[j][1] = (tot - menos).ToString();
                    }
                }
            }

            tabla_status.Rows.Add("ESTRADOS", tot_estra);
            tabla_status.Rows.Add("NN", tot_nn);            

            dataGridView1.DataSource = tabla_detalle;
            dataGridView2.DataSource = tabla_status;

            label7.Text = "Dias con Notificación: " + dataGridView1.RowCount;
            /*
            conex.cerrar();
            conex1.cerrar();
            conex2.cerrar();
            conex3.cerrar();
            */
        }

        private void detalle_prod_ind_Load(object sender, EventArgs e)
        {
            llena_noti();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value >= dateTimePicker1.Value)
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    consulta_detalle();
                }
                else
                {
                    MessageBox.Show("Seleccione un Notificador a Detallar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un intervalo de fechas válido", "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
			fichero.Title = "Guardar archivo de Excel";
			fichero.Filter = "Archivo Excel (*.XLSX)|*.xlsx";
			
			if(fichero.ShowDialog() == DialogResult.OK){
				try{

                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(tabla_detalle, "Detalle_Notificacion");
                    wb.Worksheets.Add(tabla_status, "Totales_Estatus");
                    wb.SaveAs(fichero.FileName);
                    MessageBox.Show("Se Ha creado correctamente el archivo:\n" + fichero.FileName, "Exito");
                }
                catch (Exception es)
                {
                    MessageBox.Show("Ha ocurrido un error al intentar crear el archivo:\n" + fichero.FileName + "\n\n" + es, "ERROR");
                }
            }
        }
    }
}

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
    public partial class Inicializacion_nova : Form
    {
        public Inicializacion_nova()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();
        DataTable tabla_totales = new DataTable();

        int ctrl_msg = 0, ctrl_msg1 = 0;

        public void verif_tots_rale()
        {
            conex.conectar("base_principal");
            tabla_totales=conex.consultar("SELECT COUNT(idrale) FROM rale WHERE tipo_rale=\"COP\"");
            label5.Text = tabla_totales.Rows[0][0].ToString();
            tabla_totales = conex.consultar("SELECT COUNT(idrale) FROM rale WHERE tipo_rale=\"RCV\"");
            label6.Text = tabla_totales.Rows[0][0].ToString();
            conex.cerrar();

            if (Convert.ToInt32(label5.Text) > 0)
            {
                label1.Image = Nova_Gear.Properties.Resources.check_box_tick_light_blue;
            }
            else
            {
                label1.Image = Nova_Gear.Properties.Resources.check_box_uncheck;
            }

            if (Convert.ToInt32(label6.Text) > 0)
            {
                label2.Image = Nova_Gear.Properties.Resources.check_box_tick_light_blue;
            }
            else
            {
                label2.Image = Nova_Gear.Properties.Resources.check_box_uncheck;
            }
        }

        public void verif_fechas_rale()
        {
            int r_cop = 0, r_rcv = 0;
            DateTime fecha;

            r_cop = Convert.ToInt32(label5.Text);
            r_rcv = Convert.ToInt32(label6.Text);

            conex.conectar("base_principal");           

            if(r_cop > 0){
                tabla_totales = conex.consultar("SELECT fecha_alta FROM base_principal.rale WHERE tipo_rale=\"COP\" AND incidencia =\"1\" AND (td=\"02\" OR td=\"03\") ORDER BY fecha_alta desc LIMIT 1");
                fecha = Convert.ToDateTime(tabla_totales.Rows[0][0].ToString().Substring(0,10));
                fecha = fecha.AddDays(-90);
                dateTimePicker1.Value=fecha;
                groupBox2.Enabled = true;
            }

            if (r_rcv > 0){
                tabla_totales = conex.consultar("SELECT fecha_alta FROM base_principal.rale WHERE tipo_rale=\"RCV\" AND incidencia =\"1\" AND (td=\"03\" OR td=\"06\") ORDER BY fecha_alta desc LIMIT 1");
                fecha = Convert.ToDateTime(tabla_totales.Rows[0][0].ToString().Substring(0, 10));
                fecha = fecha.AddDays(-90);
                dateTimePicker2.Value = fecha;
                groupBox2.Enabled = true;
            }
            
            conex.cerrar();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Lector_rale_txt rale_txt = new Lector_rale_txt(1);
            rale_txt.ShowDialog();
            rale_txt.Focus();

            verif_tots_rale();
            verif_fechas_rale();
        }

        private void Inicializacion_nova_Load(object sender, EventArgs e)
        {
            verif_tots_rale();
            verif_fechas_rale();
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            if(ctrl_msg==0){
                MessageBox.Show("Se recomienda incluir únicamente los créditos con fecha de alta mayor a 3 meses (90 dias).","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

            ctrl_msg++;
        }

        private void dateTimePicker1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            if (ctrl_msg1 == 0)
            {
                MessageBox.Show("Se recomienda incluir únicamente los créditos con fecha de alta mayor a 3 meses (90 dias).", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ctrl_msg1++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inicializacion_nova_2 ini_nova2 = new Inicializacion_nova_2(dateTimePicker1.Value, dateTimePicker2.Value);
            this.Hide();
            ini_nova2.Show();
        }
    }
}

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

namespace Nova_Gear.Aclaraciones
{
    public partial class Consulta_aclaraciones : Form
    {
        public Consulta_aclaraciones()
        {
            InitializeComponent();
        }

        int b = 0;

        String carga_bd = "fecha_core is null ", tipo_r=" tipo_rale = \"COP\" ";

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();//principal

        DataTable consultamysql = new DataTable();
        DataTable data1 = new DataTable();

        public DataTable copiar_datagrid_1()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView1.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView1.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 1; k < dataGridView1.ColumnCount; k++)
                {
                    if(k>0 && k<8){
                        fila_copia[k-1] = dataGridView1.Rows[j].Cells[k].Value.ToString();
                    }

                    if(k==9){
                        if (dataGridView1.Rows[j].Cells[k].Value.ToString() == "0.00")
                        { }
                        else
                        {
                            fila_copia[6] = dataGridView1.Rows[j].Cells[k].Value.ToString();
                        }
                    }
                }

                if (Convert.ToBoolean(dataGridView1.Rows[j].Cells[0].Value.ToString()) ==true)
                {
                    tabla_destino.Rows.Add(fila_copia);
                }
            }

            return tabla_destino;
        }

        public void buscar_credito()
        {
            String buscar_cred;
            int b_aux = 0;
            if (dataGridView1.RowCount > 0)
            {
                if (maskedTextBox2.Text.Length == 9)
                {
                    buscar_cred = maskedTextBox2.Text;
                    while (b < dataGridView1.RowCount)
                    {
                        if (dataGridView1.Rows[b].Cells[2].FormattedValue.ToString().Equals(buscar_cred))
                        {
                            dataGridView1.ClearSelection();
                            dataGridView1.Rows[b].Cells[0].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = b;
                            b_aux = b;
                            b = dataGridView1.RowCount + 1;
                        }
                        b++;
                    }
                    b = b_aux + 1;
                }
                else
                {

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
                panel2.Enabled = false;
            }
            else
            {
                panel2.Visible = true;
                panel2.Enabled = true;
                maskedTextBox2.Focus();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel2.Enabled = false;
        }

        public void carga_tabla(){

            conex.conectar("base_principal");

            consultamysql = conex.consultar("SELECT registro_patronal,credito,periodo,td,incidencia,sub,importe,ajuste_importe,importe_corregido,fecha_noti,tipo_rale,fecha_marca,motivo,fecha_core FROM rale_aclaraciones WHERE " + carga_bd + " AND " + tipo_r + " ORDER BY tipo_rale,registro_patronal,credito");
            
            dataGridView1.DataSource = consultamysql;

            dataGridView1.Columns[1].HeaderText = "Registro Patronal";
            dataGridView1.Columns[2].HeaderText = "Credito";
            dataGridView1.Columns[3].HeaderText = "Periodo";
            dataGridView1.Columns[4].HeaderText = "TD";
            dataGridView1.Columns[5].HeaderText = "INC";
            dataGridView1.Columns[6].HeaderText = "SUB";
            dataGridView1.Columns[7].HeaderText = "Importe Original";
            dataGridView1.Columns[8].HeaderText = "Ajuste";
            dataGridView1.Columns[9].HeaderText = "Importe Corregido";
            dataGridView1.Columns[10].HeaderText = "Fecha Notificación";            
            dataGridView1.Columns[11].HeaderText = "Tipo RALE";
            dataGridView1.Columns[12].HeaderText = "Fecha Aclaracion";
            dataGridView1.Columns[13].HeaderText = "Motivo";
            dataGridView1.Columns[14].HeaderText = "Fecha Core";

            dataGridView1.Columns[0].ReadOnly = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.Columns[10].ReadOnly = true;
            dataGridView1.Columns[11].ReadOnly = true;
            dataGridView1.Columns[12].ReadOnly = true;
            dataGridView1.Columns[13].ReadOnly = true;
            dataGridView1.Columns[14].ReadOnly = true;

            int cont = 0;
            while (cont < consultamysql.Rows.Count)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
            }

            //dataGridView1.Columns[5].HeaderText = "Incidencia";
            //dataGridView1.Columns[6].HeaderText = "Fecha Incidencia";
            //dataGridView1.Columns[7].HeaderText = "Mov";
            //dataGridView1.Columns[8].HeaderText = "Fecha Mov";
            //dataGridView1.Columns[9].HeaderText = "Fecha Alta";            
            //dataGridView1.Columns[11].HeaderText = "Sector";
            //dataGridView1.Columns[12].HeaderText = "Dias";
            //dataGridView1.Columns[13].HeaderText = "CE";            

            label4.Text = "Registros Cargados: " + dataGridView1.Rows.Count;
        }

        private void Consulta_aclaraciones_Load(object sender, EventArgs e)
        {
            comboBox4.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            carga_tabla();            
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox2.Text.Length < 9)
            {
                b = 0;
            }
        }

        private void maskedTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (maskedTextBox2.Text.Length == 9)
            {
                if (e.KeyChar == (char)(Keys.Enter))
                {
                    buscar_credito();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            buscar_credito();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String tipo_ra="";

            if (dataGridView1.Rows.Count > 0)
            {               
                data1 = copiar_datagrid_1();

                String jefe_sec, jefe_ofi;
                jefe_ofi = conex.leer_config_sub()[8];
                jefe_sec = conex.leer_config_sub()[9];
                String solicita = jefe_sec + "\nOficina de Emisiones P.O.", autoriza = jefe_ofi + "\nJefe Oficina de Emisiones y P.O.";

                if (comboBox1.SelectedIndex == 0)
                {
                    tipo_ra = "COP";
                }
                else
                {
                    tipo_ra = "RCV";
                }

                DialogResult respuesta = MessageBox.Show("Se Creará una CORE de tipo "+tipo_ra+" con los "+data1.Rows.Count+" créditos seleccionados\n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (respuesta == DialogResult.Yes)
                {
                    Depuracion.Visor_Reporte visor1 = new Depuracion.Visor_Reporte();
                    visor1.envio_aclaracion(data1, solicita, autoriza,tipo_ra);
                    visor1.Show();
                }                
            }
            else
            {
                MessageBox.Show("No hay datos que Exportar ", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex)>-1)
            {
                String r="", c="", p="";

                r = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                c = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                p = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                //MessageBox.Show("r:"+r+"|c:"+c+"|p:"+p);

                Aclaraciones.Detalle_Aclaracion detalle = new Detalle_Aclaracion(r,c,p);
                detalle.Show();
                detalle.Focus();
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox4.SelectedIndex==0){
                carga_bd = "fecha_core is null";
                carga_tabla();
            }else{
                carga_bd = "fecha_core is not null";
                carga_tabla();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int col = 1;
            if (Convert.ToBoolean(dataGridView1[0, e.RowIndex].Value.ToString()) == true)
            {
                while(col<dataGridView1.Columns.Count){
                    dataGridView1.Rows[e.RowIndex].Cells[col].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                    col++;
                }
            }else{
                while (col < dataGridView1.Columns.Count)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[col].Style.BackColor = System.Drawing.Color.White;
                    col++;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (comboBox1.SelectedIndex == 0)
            {
                tipo_r = " tipo_rale = \"COP\" ";
                carga_tabla();
            }
            else
            {
                tipo_r = " tipo_rale = \"RCV\" ";
                carga_tabla();
            }
        }
    }
}

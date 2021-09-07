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

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();//principal

        DataTable consultamysql = new DataTable();

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
                        if (dataGridView1.Rows[b].Cells[1].FormattedValue.ToString().Equals(buscar_cred))
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

        private void Consulta_aclaraciones_Load(object sender, EventArgs e)
        {
            conex.conectar("base_principal");
            consultamysql = conex.consultar("SELECT registro_patronal,credito,periodo,td,importe,incidencia,fecha_incidencia,mov,fecha_mov,fecha_alta,fecha_noti,sector,dias,ce,sub,tipo_rale,num_marca,fecha_marca FROM rale_aclaraciones ORDER BY tipo_rale,registro_patronal,credito");
            dataGridView1.DataSource = consultamysql;

            dataGridView1.Columns[0].HeaderText= "Registro Patronal";
            dataGridView1.Columns[1].HeaderText= "Credito";
            dataGridView1.Columns[2].HeaderText= "Periodo";
            dataGridView1.Columns[3].HeaderText= "TD";
            dataGridView1.Columns[4].HeaderText= "Importe";
            dataGridView1.Columns[5].HeaderText= "Incidencia";
            dataGridView1.Columns[6].HeaderText= "Fecha Incidencia";
            dataGridView1.Columns[7].HeaderText= "Mov";
            dataGridView1.Columns[8].HeaderText= "Fecha Mov";
            dataGridView1.Columns[9].HeaderText= "Fecha Alta";
            dataGridView1.Columns[10].HeaderText= "Fecha Notificación";
            dataGridView1.Columns[11].HeaderText= "Sector";
            dataGridView1.Columns[12].HeaderText= "Dias";
            dataGridView1.Columns[13].HeaderText= "CE";
            dataGridView1.Columns[14].HeaderText = "SUB";
            dataGridView1.Columns[15].HeaderText= "Tipo RALE";
            dataGridView1.Columns[16].HeaderText = "Num. Envío";
            dataGridView1.Columns[17].HeaderText = "Fecha Envío";

            label4.Text = "Registros Cargados: " + dataGridView1.Rows.Count;
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
    }
}

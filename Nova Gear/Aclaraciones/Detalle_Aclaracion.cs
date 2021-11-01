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
    public partial class Detalle_Aclaracion : Form
    {
        public Detalle_Aclaracion(String reg, String cred, String per)
        {
            InitializeComponent();
            this.reg_pat = reg;
            this.cred_rale = cred;
            this.per_rale = per;
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();//principal
        Conexion conex1 = new Conexion();//info_sindo
        Conexion conex2 = new Conexion();//guardar

        DataTable consultamysql = new DataTable();
        DataTable consultamysql1 = new DataTable();

        String reg_pat, cred_rale, per_rale, id="";

        public void consulta()
        {
            conex.conectar("base_principal");
            conex1.conectar("base_principal");

            consultamysql = conex.consultar("SELECT registro_patronal,credito,periodo,td,importe,fecha_noti,tipo_rale,num_marca,fecha_marca,ajuste_importe,importe_corregido,fecha_core,motivo,idrale_aclaraciones FROM rale_aclaraciones WHERE registro_patronal=\"" + reg_pat + "\" AND credito=\"" + cred_rale + "\" AND periodo=\"" + per_rale + "\"");

            if (consultamysql.Rows.Count > 0)
            {
                consultamysql1 = conex1.consultar("SELECT nombre FROM sindo WHERE registro_patronal like \"" + reg_pat + "%\"");

                if (consultamysql1.Rows.Count > 0)
                {
                    textBox6.Text = consultamysql1.Rows[0][0].ToString(); //razon social
                }

                textBox4.Text = consultamysql.Rows[0][0].ToString(); //reg_pat
                textBox5.Text = consultamysql.Rows[0][1].ToString(); //credito
                textBox7.Text = consultamysql.Rows[0][2].ToString(); //periodo
                textBox3.Text = consultamysql.Rows[0][3].ToString(); //td
                textBox9.Text = consultamysql.Rows[0][4].ToString(); //importe
                
                textBox13.Text = consultamysql.Rows[0][6].ToString(); //tipo_rale
                
                textBox10.Text = consultamysql.Rows[0][9].ToString(); //ajuste
                textBox11.Text = consultamysql.Rows[0][10].ToString(); //importe corregido
                
                textBox8.Text = consultamysql.Rows[0][12].ToString(); //motivo
                id = consultamysql.Rows[0][13].ToString(); //id

                if (consultamysql.Rows[0][5].ToString().Length < 9)
                {
                    textBox2.Text = "";
                }
                else
                {
                    textBox2.Text = consultamysql.Rows[0][5].ToString().Substring(0, 10); //fecha_noti
                }

                if (consultamysql.Rows[0][8].ToString().Length < 9)
                {
                    textBox1.Text = "";

                }
                else
                {
                    textBox1.Text = consultamysql.Rows[0][8].ToString().Substring(0, 10); //fecha_aclaracion
                }

                if (consultamysql.Rows[0][11].ToString().Length < 9)
                {
                    textBox12.Text = "";
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    textBox8.ReadOnly = true;
                    textBox10.ReadOnly = true;
                    textBox12.Text = consultamysql.Rows[0][11].ToString().Substring(0, 10); //fecha_core
                }

            }
        }

        private void Detalle_Aclaracion_Load(object sender, EventArgs e)
        {
            consulta();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conex2.conectar("base_principal");

            if (textBox8.BackColor == System.Drawing.Color.Green && textBox10.BackColor == System.Drawing.Color.Green)
            {
                DialogResult respuesta = MessageBox.Show("Se actualizará la información del crédito en la base de datos.\n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (respuesta == DialogResult.Yes)
                {
                    conex2.consultar("UPDATE rale_aclaraciones SET ajuste_importe=" + textBox10.Text + ", motivo=" + textBox8.Text + ",importe_corregido=" + textBox11.Text + " WHERE idrale_aclaraciones=" + id + "");
                    MessageBox.Show("La información se ha Actualizado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    consulta();
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            String texto=textBox8.Text;
            int num=0;

            if (int.TryParse(texto, out num))
            {
                textBox8.BackColor = System.Drawing.Color.Green;
                textBox8.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                textBox8.BackColor = System.Drawing.Color.Red;
                textBox8.ForeColor = System.Drawing.Color.White;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            String texto = textBox10.Text;
            Decimal num = 0, imp_tot = 0, imp_o = 0;

            if (Decimal.TryParse(texto, out num))
            {
                textBox10.BackColor = System.Drawing.Color.Green;
                textBox10.ForeColor = System.Drawing.Color.White;

                imp_o= Convert.ToDecimal(textBox9.Text);
                imp_tot = imp_o - num;
                textBox11.Text = imp_tot.ToString();
            }
            else
            {
                textBox10.BackColor = System.Drawing.Color.Red;
                textBox10.ForeColor = System.Drawing.Color.White;
                textBox11.Text = "";
            }
        }
    }
}

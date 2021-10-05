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
        Conexion conex1 = new Conexion();//guardar

        DataTable consultamysql = new DataTable();
        DataTable consultamysql1 = new DataTable();

        String reg_pat, cred_rale, per_rale;

        private void Detalle_Aclaracion_Load(object sender, EventArgs e)
        {
            conex.conectar("base_principal");
            conex1.conectar("base_principal");

            consultamysql = conex.consultar("SELECT registro_patronal,credito,periodo,td,importe,fecha_noti,tipo_rale,num_marca,fecha_marca,ajuste_importe,importe_corregido,fecha_core,motivo FROM rale_aclaraciones WHERE registro_patronal=\"" + reg_pat + "\" AND credito=\"" + cred_rale + "\" AND periodo=\"" +per_rale+ "\"");
            
            if(consultamysql.Rows.Count>0){
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
                textBox2.Text = consultamysql.Rows[0][5].ToString().Substring(0,10); //fecha_noti
                textBox13.Text = consultamysql.Rows[0][6].ToString(); //tipo_rale
                textBox1.Text = consultamysql.Rows[0][8].ToString().Substring(0,10); //fecha_aclaracion
                textBox10.Text = consultamysql.Rows[0][9].ToString(); //ajuste
                textBox11.Text = consultamysql.Rows[0][10].ToString(); //importe corregido
                textBox12.Text = consultamysql.Rows[0][11].ToString().Substring(0,10); //fecha_core
                textBox8.Text = consultamysql.Rows[0][12].ToString(); //motivo

                if (textBox2.Text == "01/01/0001")
                {
                    textBox2.Text = "";
                    
                }

                if (textBox1.Text == "01/01/0001")
                {
                    textBox1.Text = "";

                }

                if (textBox12.Text == "01/01/0001")
                {
                    textBox12.Text = "";
                    button1.Enabled=true;
                }
                else
                {
                    button1.Enabled = false;
                }

            }
        }
    }
}

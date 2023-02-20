using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear
{
    public partial class Limpiador_Nova : Form
    {
        public Limpiador_Nova()
        {
            InitializeComponent();
        }

        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();

        DataTable por_borrar = new DataTable();

        String condicion = "";

        //Declaracion de Hilo
        private Thread hilosecundario = null;

        private void Limpiador_Nova_Load(object sender, EventArgs e)
        {
            DateTime fecha_ini;            

            fecha_ini = DateTime.Today.AddYears(-1);
            dateTimePicker1.MaxDate = fecha_ini;
            dateTimePicker1.Value = fecha_ini;

            panel1.Visible = false;
            panel2.Visible = false;
            statusStrip1.Visible = false;
        }

        public void proceso_respaldo()
        {
            String sql_base = "", sql = "", sql1 = "", cadena = "";
            int tot = 0, porcentaje = 0, entero = 0, corriendo=0;
            decimal decimaal = 0m;

            sql = "INSERT INTO datos_factura_archivo (";

            conex2.conectar("base_principal");

            for (int i = 0; i < por_borrar.Columns.Count; i++)
            {
                sql += por_borrar.Columns[i].ColumnName + ", ";
            }

            sql = sql.Substring(0, sql.Length - 2);

            sql_base = sql + ") VALUES ";

            sql = sql_base;

            //MessageBox.Show(por_borrar.Rows.Count.ToString());

            for (int i = 0; i < por_borrar.Rows.Count; i++)
            {

                sql1 = "(";
                for (int j = 0; j < por_borrar.Columns.Count; j++)
                {
                    String valor_fecha = "";

                    if (por_borrar.Columns[j].ColumnName.Contains("fecha") == true)
                    {
                        if (por_borrar.Columns[j].ColumnName != "fecha_pago")
                        {
                            if (por_borrar.Rows[i][j].ToString().Length > 1)
                            {
                                valor_fecha = por_borrar.Rows[i][j].ToString();
                                valor_fecha = valor_fecha.Substring(0, 10);
                                valor_fecha = valor_fecha.Substring(6, 4) + "-" + valor_fecha.Substring(3, 2) + "-" + valor_fecha.Substring(0, 2);
                                sql1 += "\"" + valor_fecha + "\", ";
                            }
                            else
                            {
                                sql1 += "\"1900-01-01\"" + ", ";
                            }
                        }
                        else
                        {
                            sql1 += "\"" + por_borrar.Rows[i][j] + "\", ";
                        }
                    }
                    else
                    {
                        //si el tipo de dato es string
                        if (por_borrar.Rows[i][j].GetType() == cadena.GetType())
                        {
                            if (por_borrar.Rows[i][j].ToString().Length > 0)
                            {
                                //MessageBox.Show("String: " + por_borrar.Columns[j].ColumnName.ToString());
                                sql1 += "\"" + por_borrar.Rows[i][j] + "\", ";
                            }
                            else
                            {
                                sql1 += "\"-\", ";
                            }
                            
                        }
                        else
                        {
                            if (por_borrar.Rows[i][j].ToString().Length > 0)
                            {
                                sql1 += por_borrar.Rows[i][j] + ", ";
                            }
                            else
                            {
                                sql1 += "NULL, ";
                            }
                        }

                    }
                }

                sql1 = sql1.Substring(0, sql1.Length - 2);
                sql1 = sql1 + "), ";


                sql += sql1;

                tot++;

                if (tot == 100)
                {
                    sql = sql.Substring(0, sql.Length - 2);
                    //MessageBox.Show(sql);
                    conex2.consultar(sql);
                    sql = sql_base;
                    tot = 0;
                    corriendo++;
                }
                
                Invoke(new MethodInvoker(delegate
                {
                    porcentaje = Convert.ToInt32((i * 100) / por_borrar.Rows.Count);
                    toolStripStatusLabel1.Text = porcentaje + "%";
                    toolStripProgressBar1.Value = porcentaje;
                    toolStripProgressBar1.ToolTipText = porcentaje.ToString();

                    if(corriendo==0){
                        toolStripStatusLabel2.Text = "Copiando  " + i + " de " + por_borrar.Rows.Count;
                    }

                    if (corriendo == 1)
                    {
                        toolStripStatusLabel2.Text = "Copiando.  " + i + " de " + por_borrar.Rows.Count;
                    }

                    if (corriendo == 2)
                    {
                        toolStripStatusLabel2.Text = "Copiando..  " + i + " de " + por_borrar.Rows.Count;
                    }

                    if (corriendo == 3)
                    {
                        toolStripStatusLabel2.Text = "Copiando...  " + i + " de " + por_borrar.Rows.Count;                        
                    }

                    if (corriendo == 4)
                    {
                        toolStripStatusLabel2.Text = "Copiando...  " + i + " de " + por_borrar.Rows.Count;
                        corriendo = 0;
                    }
                }));

            } //fin proceso copiado


            sql = sql.Substring(0, sql.Length - 2);
            //MessageBox.Show(sql);
            conex2.consultar(sql);

            Invoke(new MethodInvoker(delegate
            {
                porcentaje = 100;
                toolStripStatusLabel1.Text = porcentaje + "%";
                toolStripProgressBar1.Value = porcentaje;
                toolStripProgressBar1.ToolTipText = porcentaje.ToString();
                toolStripStatusLabel2.Text = "Copiando...  " + por_borrar.Rows.Count + " de " + por_borrar.Rows.Count;
            }));

            Invoke(new MethodInvoker(delegate
            {
                toolStripStatusLabel2.Text = "Liberando Base de Datos";
            }));


            conex2.consultar("DELETE FROM datos_factura WHERE "+condicion);            

            MessageBox.Show("Se movieron correctamente los " + por_borrar.Rows.Count + " registros a la base de datos secundaria", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String fecha = "";            

            toolStripStatusLabel2.Text = "Consultando...";
            dateTimePicker1.Enabled = false;
            fecha = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day;
            //MessageBox.Show(fecha);

            condicion = "fecha_notificacion <= \"" + fecha + "\" AND (status != \"EN TRAMITE\" AND status !=\"0\" )";

            conex.conectar("base_principal");
            por_borrar = conex.consultar("SELECT * FROM base_principal.datos_factura WHERE "+condicion);
            conex.cerrar();

            toolStripStatusLabel2.Text = "Casos Encontrados: " + por_borrar.Rows.Count;

            if(por_borrar.Rows.Count>0)
            {
                button1.Enabled = false;
                dateTimePicker1.Enabled=false;
                button2.Enabled = true;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult resu = MessageBox.Show("Este Proceso moverá los registros antiguos a una base de datos secundaria, por lo que dejarán de estar disponibles para su utilización de forma normal en el programa.\n\n¿Desea Realizar la Optimización?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (resu == DialogResult.Yes)
            {
                button2.Enabled = false;
                //proceso_respaldo();
                hilosecundario = new Thread(new ThreadStart(proceso_respaldo));
                hilosecundario.Start();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            statusStrip1.Visible = true;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            //statusStrip1.Visible = true;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            String reg_pat = "", credito_cuota = "", credito_multa = "", periodo = "", condicion="";

            if(maskedTextBox1.MaskCompleted==true){

                reg_pat = maskedTextBox1.Text;
                reg_pat = reg_pat.Replace("-", "");
                reg_pat = reg_pat.Substring(0, 10);

                reg_pat = "registro_patronal2= \"" + reg_pat + "\" ";

                if ((textBox1.Text.Length == 9))
                {
                    credito_cuota = textBox1.Text;
                    credito_cuota = "AND credito_cuotas=\"" + credito_cuota + "\" ";
                }
                else
                {
                    credito_cuota = " ";
                }

                if ((textBox2.Text.Length == 9))
                {
                    credito_multa = textBox2.Text;
                    credito_multa = "AND credito_multa=\"" + credito_multa + "\" ";
                }
                else
                {
                    credito_multa = " ";
                }

                if ((textBox3.Text.Length == 6))
                {
                    periodo = textBox3.Text;
                    periodo = "AND periodo=\"" + periodo + "\"";
                }
                else
                {
                    periodo = " ";
                }

                //MessageBox.Show("SELECT * FROM datos_factura_archivo WHERE " + condicion);
                condicion = reg_pat + credito_cuota + credito_multa + periodo;
                
                conex.conectar("base_principal");
                por_borrar = conex.consultar("SELECT * FROM datos_factura_archivo WHERE "+reg_pat+credito_cuota+credito_multa+periodo);
                conex.cerrar();

                if (por_borrar.Rows.Count > 0)
                {
                   Limpiador_Nova_restaurador consulta = new Limpiador_Nova_restaurador(por_borrar,condicion);
                   consulta.ShowDialog();
                   consulta.Focus();
                }
                else
                {
                   MessageBox.Show("No se encontró ninguna coincidencia", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }                
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(maskedTextBox1.MaskCompleted){
                maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
                textBox1.Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length==9){
                textBox2.Focus();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 9)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 6)
            {
                button3.Focus();
            }
        }       
        
    }
}

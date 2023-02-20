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
    public partial class Limpiador_Nova_restaurador : Form
    {
        public Limpiador_Nova_restaurador(DataTable tabla, String condicion)
        {
            InitializeComponent();
            this.consulta = tabla;
            condicion_consulta = condicion;
        }

        DataTable consulta = new DataTable();
        DataTable por_borrar = new DataTable();

        String condicion_consulta = "";

        Conexion conex = new Conexion();

        public DataTable copiar_datagrid()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 1; j < dataGridView1.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView1.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 1; k < dataGridView1.ColumnCount; k++)
                {
                    fila_copia[k - 1] = dataGridView1.Rows[j].Cells[k].Value;
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        private void Limpiador_Nova_restaurador_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = consulta;
            label1.Text = "Registros: "+dataGridView1.Rows.Count;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (i > 0)
                {
                    dataGridView1.Columns[i].ReadOnly = true;
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                else
                {
                    dataGridView1.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.CornflowerBlue;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int tot_marcados = 0, tot = 0;
            String sql_base = "", sql = "", sql1 = "", cadena="";

            for(int i=0; i<dataGridView1.Rows.Count; i++){
                if (dataGridView1[0, i].Value != null)
                {
                    if(dataGridView1[0,i].Value.ToString()=="True"){
                        tot_marcados++;
                    }
                }
            }

            DialogResult resu = MessageBox.Show("Se procederá a restaurar "+tot_marcados+" registro(s), se moverán de regreso a la base de datos principal.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (resu == DialogResult.Yes)
            {
                por_borrar = copiar_datagrid();

                sql = "INSERT INTO datos_factura (";

                conex.conectar("base_principal");

                for (int i = 0; i < por_borrar.Columns.Count; i++)
                {
                    if(i>0){
                        sql += por_borrar.Columns[i].ColumnName + ", ";
                    }
                }

                sql = sql.Substring(0, sql.Length - 2);

                sql_base = sql + ") VALUES ";

                sql = sql_base;

                //MessageBox.Show(por_borrar.Rows.Count.ToString());

                for (int i = 0; i < por_borrar.Rows.Count; i++)
                {

                    if (dataGridView1[0, i].Value != null)
                    {
                        if (dataGridView1[0, i].Value.ToString() == "True")
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
                                    if ((por_borrar.Columns[j].ColumnName != "id") && (por_borrar.Columns[j].ColumnName != "SELECCIONAR"))
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
                            }

                            sql1 = sql1.Substring(0, sql1.Length - 2);
                            sql1 = sql1 + "), ";

                            sql += sql1;

                            tot++;

                            if (tot == 10)
                            {
                                sql = sql.Substring(0, sql.Length - 2);
                                //MessageBox.Show(sql);
                                conex.consultar(sql);
                                sql = sql_base;
                                tot = 0;
                            }
                        }
                    }
                } //fin proceso copiado

                sql = sql.Substring(0, sql.Length - 2);
                //MessageBox.Show(sql);
                conex.consultar(sql);

                conex.consultar("DELETE FROM datos_factura_archivo WHERE " + condicion_consulta);
                conex.guardar_evento("Se movieron correctamente los " + tot_marcados + " registros marcados a la base de datos principal");
                MessageBox.Show("Se movieron correctamente los "+tot_marcados+" registros marcados a la base de datos principal", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                this.Close();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex;

            if (dataGridView1[0, indice].Value != null)
            {
                //MessageBox.Show(dataGridView1[0, indice].Value.ToString());
                if (dataGridView1[0, indice].Value.ToString() == "True")
                {                   
                    dataGridView1.Rows[indice].DefaultCellStyle.BackColor = System.Drawing.Color.DodgerBlue;
                    dataGridView1.Rows[indice].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    dataGridView1.Rows[indice].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dataGridView1.Rows[indice].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }

                dataGridView1.Rows[indice].Cells[0].Style.BackColor = System.Drawing.Color.CornflowerBlue;
            }
        }
    }
}

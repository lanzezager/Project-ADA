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
    public partial class Marcar_Aclaraciones : Form
    {
        public Marcar_Aclaraciones()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();//principal
        Conexion conex1 = new Conexion();//guardar

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;

        DataTable data_acumulador = new DataTable();
        DataTable consultamysql = new DataTable();
        DataTable tabla_imprime = new DataTable();
        DataTable excel = new DataTable();
        DataTable excel_temp = new DataTable();
        DataTable consultamysql2 = new DataTable();

        int b = 0;

        public void estilo_tabla()
        {
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

        public void buscar_en_bd(String rp, String cred, String per, String tr)
        {
            String linea_busca="", linea_rp="", linea_cred="",linea_per="";

            linea_busca = " tipo_rale=\""+tr+"\"";

            if(rp.Length>0){
                linea_rp= "AND registro_patronal=\"" + rp + "\" ";
            }

            if(cred.Length>0){
                linea_cred = "AND credito=\"" + cred + "\" ";
            }

            if(per.Length>0){
                linea_per = "AND periodo=\"" + per + "\" ";
            }

            if(linea_cred.Length>0){

                linea_busca = linea_busca + linea_rp + linea_cred + linea_per;

                consultamysql = conex.consultar("SELECT registro_patronal,credito,periodo,td,importe,incidencia,fecha_incidencia,mov,fecha_mov,fecha_alta,fecha_noti,sector,dias,ce,tipo_rale FROM rale WHERE " + linea_busca + " ORDER BY registro_patronal,credito");

                if (consultamysql.Rows.Count > 0)
                {
                    dataGridView1.Rows.Add(consultamysql.Rows[0][0].ToString(),
                                           consultamysql.Rows[0][1].ToString(),
                                           consultamysql.Rows[0][2].ToString(),
                                           consultamysql.Rows[0][3].ToString(),
                                           consultamysql.Rows[0][4].ToString(),
                                           consultamysql.Rows[0][5].ToString(),
                                           consultamysql.Rows[0][6].ToString().Substring(0, 10),
                                           consultamysql.Rows[0][7].ToString(),
                                           consultamysql.Rows[0][8].ToString().Substring(0, 10),
                                           consultamysql.Rows[0][9].ToString().Substring(0, 10),
                                           consultamysql.Rows[0][10].ToString().Substring(0, 10),
                                           consultamysql.Rows[0][11].ToString(),
                                           consultamysql.Rows[0][12].ToString(),
                                           consultamysql.Rows[0][13].ToString(),
                                           consultamysql.Rows[0][14].ToString()
                        );

                    //dataGridView1.DataSource = consultamysql;
                    label4.Text = "Registros Cargados: " + dataGridView1.RowCount;

                    if (dataGridView1.Rows.Count > 0)
                    {
                        button1.Enabled = true;
                    }

                    maskedTextBox4.Clear();
                    maskedTextBox1.Clear();
                    maskedTextBox3.Clear();
                    maskedTextBox4.Focus();
                }
                else
                {
                    MessageBox.Show("Registro no Encontrado", "AVISO");
                }
            }
            else
            {
                MessageBox.Show("Digíte el crédito a buscar", "AVISO");
            }
        }

        public void carga_chema_excel()
        {
            int i = 0;
            int filas = 0;
            String tabla;

            comboBox8.Items.Clear();
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
                            comboBox8.Items.Add(tabla);
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
            String hoja = "", cons_exc;
            comboBox8.SelectedIndex = 0;
            hoja = comboBox8.SelectedItem.ToString();

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
                            data_acumulador = dataSet.Tables[0];
                            //dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet                            
                            //data_acumulador.Merge(dataSet.Tables[0]);
                            //MessageBox.Show("" + data_acumulador.Rows.Count + "\n hojas: " + comboBox3.Items.Count + "\n hoja: " + hoja);
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

        public void buscar_desde_excel()
        {
            for (int i = 0; i < data_acumulador.Rows.Count;i++)
            {
                buscar_en_bd(data_acumulador.Rows[i][0].ToString(),
                             data_acumulador.Rows[i][1].ToString(),
                             data_acumulador.Rows[i][2].ToString(),
                             data_acumulador.Rows[i][14].ToString());
            }

        }

        private void Marcar_Aclaraciones_Load(object sender, EventArgs e)
        {
            conex.conectar("base_principal");

            dataGridView1.Columns.Add("registro_patronal", "Registro Patronal");
            dataGridView1.Columns.Add("credito", "Credito");
            dataGridView1.Columns.Add("periodo", "Periodo");
            dataGridView1.Columns.Add("td", "TD");
            dataGridView1.Columns.Add("importe", "Importe");
            dataGridView1.Columns.Add("incidencia", "Incidencia");
            dataGridView1.Columns.Add("fecha_incidencia", "Fecha Incidencia");
            dataGridView1.Columns.Add("mov", "Mov");
            dataGridView1.Columns.Add("fecha_mov", "Fecha Mov");
            dataGridView1.Columns.Add("fecha_alta","Fecha Alta");
            dataGridView1.Columns.Add("fecha_noti", "Fecha Notificación");
            dataGridView1.Columns.Add("sector", "Sector");
            dataGridView1.Columns.Add("dias","Dias");
            dataGridView1.Columns.Add("ce", "CE");
            dataGridView1.Columns.Add("tipo_rale", "Tipo RALE");

            tabla_imprime.Columns.Add("asunto");
            tabla_imprime.Columns.Add("reg_pat");
            tabla_imprime.Columns.Add("credito");
            tabla_imprime.Columns.Add("periodo");
            tabla_imprime.Columns.Add("tipo_doc");
            tabla_imprime.Columns.Add("incidencia");
            tabla_imprime.Columns.Add("sub");
            tabla_imprime.Columns.Add("importe");
            tabla_imprime.Columns.Add("folio1");
            tabla_imprime.Columns.Add("folio2");
            tabla_imprime.Columns.Add("folio3");
            tabla_imprime.Columns.Add("clave");

            excel.Columns.Add("registro_patronal");
            excel.Columns.Add("credito");
            excel.Columns.Add("periodo");
            excel.Columns.Add("td");
            excel.Columns.Add("importe");
            excel.Columns.Add("incidencia");
            excel.Columns.Add("fecha_incidencia");
            excel.Columns.Add("mov");
            excel.Columns.Add("fecha_mov");
            excel.Columns.Add("fecha_alta");
            excel.Columns.Add("fecha_noti");
            excel.Columns.Add("sector");
            excel.Columns.Add("dias");
            excel.Columns.Add("ce");
            excel.Columns.Add("tipo_rale");
            excel.Columns.Add("num_envio");
            excel.Columns.Add("fecha_envio");

            excel_temp.Columns.Add("registro_patronal");
            excel_temp.Columns.Add("credito");
            excel_temp.Columns.Add("periodo");
            excel_temp.Columns.Add("td");
            excel_temp.Columns.Add("importe");
            excel_temp.Columns.Add("incidencia");
            excel_temp.Columns.Add("fecha_incidencia");
            excel_temp.Columns.Add("mov");
            excel_temp.Columns.Add("fecha_mov");
            excel_temp.Columns.Add("fecha_alta");
            excel_temp.Columns.Add("fecha_noti");
            excel_temp.Columns.Add("sector");
            excel_temp.Columns.Add("dias");
            excel_temp.Columns.Add("ce");
            excel_temp.Columns.Add("tipo_rale");

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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox4.Focus();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            buscar_credito();
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

        private void button1_Click(object sender, EventArgs e)
        {
            String sub, jefe_sec,jefe_ofi, solicita,autoriza,sql="",fecha_mov="",fecha_alta="",fecha_noti="",fecha_inc="",fecha_hoy="";
            int num_marca = 0, omitidos=0, guardados=0;
	
            sub = conex.leer_config_sub()[4];                    
            jefe_ofi = conex.leer_config_sub()[8];                    
            jefe_sec = conex.leer_config_sub()[9];
            solicita = jefe_sec + "\nOficina de Emisiones P.O.";
            autoriza = jefe_ofi + "\nJefe Oficina de Emisiones y P.O.";

            DialogResult respuesta = MessageBox.Show("Se Guardarán los créditos listados en la BD de Aclaraciones\n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (respuesta == DialogResult.Yes)
            {
                tabla_imprime.Rows.Clear();
                excel.Rows.Clear();

                conex1.conectar("base_principal");
                consultamysql = conex1.consultar("SELECT num_marca FROM rale_aclaraciones ORDER BY num_marca DESC LIMIT 1");

                if (consultamysql.Rows.Count > 0)
                {
                    num_marca = Convert.ToInt32(consultamysql.Rows[0][0].ToString());
                    num_marca = num_marca + 1;
                }
                else
                {
                    num_marca = 1;
                }

                fecha_hoy = System.DateTime.Today.Year + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Day;

                for (int i = 0; i < dataGridView1.RowCount;i++)
                {
                    tabla_imprime.Rows.Add( dataGridView1[0,i].Value.ToString(),"",
                                            dataGridView1[1,i].Value.ToString(),"",
                                            dataGridView1[4, i].Value.ToString(),"",
                                            dataGridView1[3,i].Value.ToString(),
                                            dataGridView1[2,i].Value.ToString()                                          
                        );

                    excel.Rows.Add( dataGridView1[0,i].Value.ToString(),
                                    dataGridView1[1,i].Value.ToString(),
                                    dataGridView1[2,i].Value.ToString(),
                                    dataGridView1[3,i].Value.ToString(),
                                    dataGridView1[4,i].Value.ToString(),
                                    dataGridView1[5,i].Value.ToString(),
                                    dataGridView1[6,i].Value.ToString(),
                                    dataGridView1[7,i].Value.ToString(),
                                    dataGridView1[8,i].Value.ToString(),
                                    dataGridView1[9,i].Value.ToString(),
                                    dataGridView1[10,i].Value.ToString(),
                                    dataGridView1[11,i].Value.ToString(),
                                    dataGridView1[12,i].Value.ToString(),
                                    dataGridView1[13,i].Value.ToString(),
                                    dataGridView1[14,i].Value.ToString(),
                                    num_marca,//dataGridView1[15, i].Value.ToString(),
                                    fecha_hoy//dataGridView1[16,i].Value.ToString()
                                    );
                }

                /*Depuracion.Visor_Reporte visor1 = new Depuracion.Visor_Reporte();
                visor1.envio(tabla_imprime, solicita, autoriza);
                visor1.Show();*/

                //DataTable excel = (DataTable)(dataGridView1.DataSource);

                    for (int i = 0; i < excel.Rows.Count; i++)
                    {
                        fecha_inc = excel.Rows[i][6].ToString().Substring(6, 4) + "-" + excel.Rows[i][6].ToString().Substring(3, 2) + "-" + excel.Rows[i][6].ToString().Substring(0,2);
                        fecha_mov = excel.Rows[i][8].ToString().Substring(6, 4) + "-" + excel.Rows[i][8].ToString().Substring(3, 2) + "-" + excel.Rows[i][8].ToString().Substring(0, 2);
                        fecha_alta = excel.Rows[i][9].ToString().Substring(6, 4) + "-" + excel.Rows[i][9].ToString().Substring(3, 2) + "-" + excel.Rows[i][9].ToString().Substring(0, 2);

                        if (excel.Rows[i][10].ToString().Length>9)
                        {
                            fecha_noti = excel.Rows[i][10].ToString().Substring(6, 4) + "-" + excel.Rows[i][10].ToString().Substring(3, 2) + "-" + excel.Rows[i][10].ToString().Substring(0, 2);
                        }

                        sql = "SELECT * FROM rale_aclaraciones WHERE registro_patronal=\"" + excel.Rows[i][0].ToString() + "\" AND credito=\"" + excel.Rows[i][1].ToString() + "\" AND periodo =\"" + excel.Rows[i][2].ToString() + "\"";
                        consultamysql2= conex1.consultar(sql);

                        if (consultamysql2.Rows.Count == 0)
                        {
                            sql = "INSERT INTO rale_Aclaraciones (registro_patronal,credito,periodo,td,importe,incidencia,fecha_incidencia,mov,fecha_mov,fecha_alta,fecha_noti,sector,dias,ce,sub,tipo_rale,num_marca,fecha_marca) " +
                                  "VALUES(  \"" + excel.Rows[i][0].ToString() + "\"," +
                                         "\"" + excel.Rows[i][1].ToString() + "\"," +
                                         "\"" + excel.Rows[i][2].ToString() + "\"," +
                                         "\"" + excel.Rows[i][3].ToString() + "\"," +
                                         "  " + excel.Rows[i][4].ToString() + "," +
                                         "\"" + excel.Rows[i][5].ToString() + "\"," +
                                         "\"" + fecha_inc + "\"," +
                                         "  " + excel.Rows[i][7].ToString() + "," +
                                         "\"" + fecha_mov + "\"," +
                                         "\"" + fecha_alta + "\"," +
                                         "\"" + fecha_noti + "\"," +
                                         "  " + excel.Rows[i][11].ToString() + "," +
                                         "  " + excel.Rows[i][12].ToString() + "," +
                                         "\"" + excel.Rows[i][13].ToString() + "\"," +
                                         "\"" + sub + "\"," +
                                         "\"" + excel.Rows[i][14].ToString() + "\"," +
                                         "\"" + num_marca + "\"," +
                                         "\"" + fecha_hoy + "\"" +
                                ")";

                            conex1.consultar(sql);
                            guardados++;

                        }
                        else
                        {
                            omitidos++;
                        }
                    }

                    if(omitidos>0){
                        conex.guardar_evento("Se Guardaron "+guardados+" Créditos Correctamente para su correspondiente Aclaración. Se Omitieron "+omitidos+" Créditos debido a que ya se encuentran en proceso de Aclaración");
                        MessageBox.Show("Se Guardaron "+guardados+" Créditos Correctamente para su correspondiente Aclaración \nSe Omitieron "+omitidos+" Créditos debido a que ya se encuentran en proceso de Aclaración", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    
                    }else{
                        conex.guardar_evento("Se Guardaron " + guardados + " Créditos Correctamente para su correspondiente Aclaración");
                        MessageBox.Show("Se Guardaron " + guardados + " Créditos Correctamente para su correspondiente Aclaración", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }

                    
                    dataGridView1.Rows.Clear();
                    label4.Text = "Registros Cargados: 0";

                    /*
                    //GUARDAR EXCEL
                    SaveFileDialog dialog_save = new SaveFileDialog();
                    dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                    dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                    if (dialog_save.ShowDialog() == DialogResult.OK)
                    {
                        //tabla_excel
                        XLWorkbook wb = new XLWorkbook();
                        wb.Worksheets.Add(excel, "Aclaraciones");
                        wb.SaveAs(@"" + dialog_save.FileName + "");
                        //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();
                        entrega_cartera.envio3(tabla_imprime, "CRÉDITOS DEL RALE PARA ACLARAR", "", "E.P.O.");
                        entrega_cartera.Show();
                    }
                    */
            }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String rp = "", credito="", periodo="",tipo_rale="";

            if (comboBox4.SelectedIndex > -1)
            {
                if(maskedTextBox4.MaskCompleted== true){
                    rp = maskedTextBox4.Text;
                    rp = rp.Substring(0, 3) + rp.Substring(4, 5) + rp.Substring(10, 2);
                }

                if(maskedTextBox1.MaskCompleted== true){
                    credito = maskedTextBox1.Text;
                }
                
                if(maskedTextBox3.MaskCompleted== true){
                    periodo = maskedTextBox3.Text;
                    periodo = periodo.Substring(0, 4) + periodo.Substring(5, 2);
                }

                if (comboBox4.SelectedIndex == 0)
                {
                    tipo_rale = "COP";
                }
                else
                {
                    tipo_rale = "RCV";
                }
            
                buscar_en_bd(rp, credito, periodo,tipo_rale);
            }
            else
            {
                MessageBox.Show("Debe seleccionar el tipo de RALE en el que se buscará el crédito", "AVISO");
            }
        }

        private void maskedTextBox4_TextChanged(object sender, EventArgs e)
        {
            if(maskedTextBox4.MaskCompleted==true){
                maskedTextBox1.Focus();
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox1.MaskCompleted == true)
            {
                maskedTextBox3.Focus();
            }
        }

        private void maskedTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox3.MaskCompleted == true)
            {
                button2.Focus();
            }
        }

        private void removerCréditoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dataGridView1[2,dataGridView1.CurrentCell.RowIndex].Value.ToString());

            DialogResult respuesta = MessageBox.Show("Se eliminará la linea seleccionada de la lista \n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (respuesta == DialogResult.Yes)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);

                label4.Text = "Registros Cargados: " + dataGridView1.RowCount;

                maskedTextBox4.Clear();
                maskedTextBox1.Clear();
                maskedTextBox3.Clear();
                maskedTextBox4.Focus();

                if (dataGridView1.Rows.Count > 0)
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[0, e.RowIndex];
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0){
                DialogResult respuesta = MessageBox.Show("Se guardarán los créditos en un archivo de Excel \nEsto no afectará la base de datos.\n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (respuesta == DialogResult.Yes)
                {
                    excel_temp.Rows.Clear();

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {                        
                        excel_temp.Rows.Add(dataGridView1[0, i].Value.ToString(),
                                        dataGridView1[1, i].Value.ToString(),
                                        dataGridView1[2, i].Value.ToString(),
                                        dataGridView1[3, i].Value.ToString(),
                                        dataGridView1[4, i].Value.ToString(),
                                        dataGridView1[5, i].Value.ToString(),
                                        dataGridView1[6, i].Value.ToString(),
                                        dataGridView1[7, i].Value.ToString(),
                                        dataGridView1[8, i].Value.ToString(),
                                        dataGridView1[9, i].Value.ToString(),
                                        dataGridView1[10, i].Value.ToString(),
                                        dataGridView1[11, i].Value.ToString(),
                                        dataGridView1[12, i].Value.ToString(),
                                        dataGridView1[13, i].Value.ToString(),
                                        dataGridView1[14, i].Value.ToString()
                                        );
                    }

                    //GUARDAR EXCEL
                    SaveFileDialog dialog_save = new SaveFileDialog();
                    dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                    dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                    if (dialog_save.ShowDialog() == DialogResult.OK)
                    {
                        //tabla_excel
                        XLWorkbook wb = new XLWorkbook();
                        wb.Worksheets.Add(excel_temp, "Aclaraciones_Temporal");
                        wb.SaveAs(@"" + dialog_save.FileName + "");
                        //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            String cad_con = "";

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

                //dataGridView1.DataSource = data_acumulador;

                buscar_desde_excel();

                MessageBox.Show("El archivo se cargó Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void maskedTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            String rp = "", credito = "", periodo = "", tipo_rale = "";
            
            if (e.KeyChar == (char)(Keys.Enter))
            {     
                if (comboBox4.SelectedIndex > -1)
                {
                    if (maskedTextBox4.MaskCompleted == true)
                    {
                        rp = maskedTextBox4.Text;
                        rp = rp.Substring(0, 3) + rp.Substring(4, 5) + rp.Substring(10, 2);
                    }

                    if (maskedTextBox1.MaskCompleted == true)
                    {
                        credito = maskedTextBox1.Text;
                    }

                    if (maskedTextBox3.MaskCompleted == true)
                    {
                        periodo = maskedTextBox3.Text;
                        periodo = periodo.Substring(0, 4) + periodo.Substring(5, 2);
                    }

                    if (comboBox4.SelectedIndex == 0)
                    {
                        tipo_rale = "COP";
                    }
                    else
                    {
                        tipo_rale = "RCV";
                    }

                    buscar_en_bd(rp, credito, periodo, tipo_rale);
                }
                else
                {
                    MessageBox.Show("Debe seleccionar el tipo de RALE en el que se buscará el crédito", "AVISO");
                }
            }
        }
    }
}

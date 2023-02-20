using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Universal
{
    public partial class Edicion_en_Volumen : Form
    {
        public Edicion_en_Volumen()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        Conexion conex4 = new Conexion();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;

        DataTable data_acumulador = new DataTable();
        DataTable data_1 = new DataTable();

        //Variables
        int[] contros;
        String guarda = "",guarda_aviso="";

        //controladores
        public void llenar_Cb1()
        {
            conex.conectar("base_principal");
            comboBox1.Items.Clear();
            int i = 0;
            dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre,id_usuario FROM base_principal.usuarios WHERE puesto =\"Controlador\" ORDER BY apellido;");
            //dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (controlador) FROM base_principal.datos_factura ORDER BY controlador;");
            contros = new int[dataGridView3.RowCount];
            do
            {
                comboBox1.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString() + " " + dataGridView3.Rows[i].Cells[1].Value.ToString());
                contros[i] = Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value.ToString());
                i++;
            } while (i < dataGridView3.RowCount);
            i = 0;
            //comboBox4.Items.Add("--NINGUNO--");
            conex.cerrar();
        }
        //notificadores
        public void llenar_Cb2()
        {
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            int i = 0;
            //dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
            dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (notificador) FROM base_principal.datos_factura ORDER BY notificador;");
            do
            {
                //comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
                comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView3.RowCount);
            i = 0;
            //comboBox3.Items.Add("--NINGUNO--");
            conex.cerrar();
        }
        //notificadores v2
        public void llenar_Cb2_v2()
        {
            conex3.conectar("base_principal");
            comboBox2.Items.Clear();
            int i = 0;
            dataGridView3.DataSource = conex3.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" AND controlador = \"" + contros[comboBox1.SelectedIndex] + "\" ORDER BY apellido;");
            do
            {
                comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString() + " " + dataGridView3.Rows[i].Cells[1].Value.ToString());
                i++;
            } while (i < dataGridView3.RowCount);
            i = 0;
            //comboBox3.Items.Add("--NINGUNO--");
            conex3.cerrar();
        }
        //periodos
        public void llenar_Cb4_todos()
        {
            conex.conectar("base_principal");
            comboBox4.Items.Clear();
            int i = 0;
            dataGridView3.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");

            comboBox4.Items.Add("-");

            do
            {
                comboBox4.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView3.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void llenar_Cb9_todos()
        {
            conex.conectar("base_principal");
            comboBox9.Items.Clear();
            int i = 0;
            dataGridView3.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");

            comboBox9.Items.Add("-");

            do
            {
                comboBox9.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView3.RowCount);
            i = 0;
            conex.cerrar();
        }

        public void formato_tabla()
        {

            for (int i = 1; i < (dataGridView1.ColumnCount)-1;i++)
            {
                dataGridView1.Columns[i].ReadOnly = true;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            int cont = 0;
            while (cont < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
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

                            //dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet                            
                            data_acumulador.Merge(dataSet.Tables[0]);
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

        public void buscar_con_excel()
        {
            String sql="",cad_ids="";
            int tot = 0;

            conex2.conectar("base_principal");
            //extraer ids
            for (int i = 0; i < data_acumulador.Rows.Count; i++)
            {
                sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + data_acumulador.Rows[i][0].ToString() + "\"AND credito_cuotas=\"" + data_acumulador.Rows[i][1].ToString() + "\" AND credito_multa=\"" + data_acumulador.Rows[i][2].ToString() + "\"";
                data_1=conex2.consultar(sql);
                if (data_1.Rows.Count>0)
                {
                    cad_ids += data_1.Rows[0][0].ToString()+",";
                }
                tot++;
            }

            //MessageBox.Show(""+tot);
            if (cad_ids.Length>0)
            {
                cad_ids = cad_ids.Substring(0, cad_ids.Length - 1);

                //consulta final
                sql = "SELECT id,nombre_periodo,registro_patronal,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,fecha_cartera,fecha_depuracion,status,nn,fecha_estrados,estado_cartera,observaciones FROM datos_factura WHERE id IN ("+cad_ids+")";
                dataGridView1.DataSource = conex2.consultar(sql);
                label4.Text = "Registros: " + dataGridView1.RowCount;
                conex.cerrar();

                formato_tabla();
            }
        }

        public void buscar_normal()
        {
            String periodo_sql="",estatus_sql="",sql="";
            int filtro_sql = 0;
            conex2.conectar("base_principal");

            if (comboBox4.SelectedIndex > 0)
            {
                periodo_sql = "nombre_periodo=\"" + comboBox4.SelectedItem + "\"";
            }
            else
            {
                filtro_sql++;
            }

            if (comboBox5.SelectedIndex > 0)
            {
                if (comboBox4.SelectedIndex > 0)
                {
                    estatus_sql = "AND status=\"" + comboBox5.SelectedItem + "\"";
                }
                else
                {
                    estatus_sql = "status=\"" + comboBox5.SelectedItem + "\"";
                }
            }
            else
            {
                filtro_sql++;
            }

            if (filtro_sql == 2)
            {
                sql = "SELECT id,nombre_periodo,registro_patronal,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,fecha_cartera,fecha_depuracion,status,nn,fecha_estrados,estado_cartera,observaciones FROM datos_factura";
            }
            else
            {
                sql = "SELECT id,nombre_periodo,registro_patronal,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,sector_notificacion_actualizado,controlador,notificador,fecha_entrega,fecha_recepcion,fecha_notificacion,fecha_cartera,fecha_depuracion,status,nn,fecha_estrados,estado_cartera,observaciones FROM datos_factura WHERE " + periodo_sql + estatus_sql;
            }

            dataGridView1.DataSource = conex2.consultar(sql);
            label4.Text = "Registros: " + dataGridView1.RowCount;
            conex2.cerrar();

            formato_tabla();
        }

        public void contar_seleccionados()
        {
            int seleccionados = 0;

            for (int i = 0; i < dataGridView1.RowCount;i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                {
                    seleccionados++;
                }
            }

            label6.Text = "Seleccionados: " + seleccionados;
        }

        public void guardar_datos_noti()
        {
            guarda = "";
            guarda_aviso = "";

            if(maskedTextBox9.MaskCompleted==true){
                guarda += "sector_notificacion_actualizado="+maskedTextBox9.Text+",";
                guarda_aviso += "•Sector Notificación: " + maskedTextBox9.Text + "\n";
            }

            if(comboBox1.SelectedIndex>-1){
                guarda += "controlador=\"" + comboBox1.SelectedItem + "\",";
                guarda_aviso += "•Controlador: " + comboBox1.SelectedItem + "\n";
            }

            if (comboBox2.SelectedIndex > -1)
            {
                guarda += "notificador=\"" + comboBox2.SelectedItem + "\",";
                guarda_aviso += "•Notificador: " + comboBox2.SelectedItem + "\n";
            }

            if (comboBox6.SelectedIndex > -1)
            {
                if(comboBox6.SelectedIndex==0){
                    guarda += "nn=\"-\",";
                    guarda_aviso += "•Estado Notificacion: NORMAL\n";
                }else{
                    guarda += "nn=\"" + comboBox6.SelectedItem + "\",";
                    guarda_aviso += "•Estado Notificacion: " + comboBox6.SelectedItem + "\n";
                }
            }    

            if (checkBox8.Checked == true)
            {
                guarda += "fecha_entrega=NULL,";
                guarda_aviso += "•Fecha Entrega: VACÍO \n";
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    string dia = dateTimePicker1.Value.ToShortDateString();
                    guarda += "fecha_entrega=\"" + dia.Substring(6, 4) + "-" + dia.Substring(3, 2) + "-" + dia.Substring(0, 2) + "\",";
                    guarda_aviso += "•Fecha Entrega: "+dia+"\n";
                }
            }            

            if (checkBox9.Checked == true)
            {
                guarda += "fecha_recepcion=NULL,";
                guarda_aviso += "•Fecha Recepción: VACÍO\n";
            }
            else
            {
                if (checkBox3.Checked == true)
                {
                    string dia = dateTimePicker2.Value.ToShortDateString();
                    guarda += "fecha_recepcion=\"" + dia.Substring(6, 4) + "-" + dia.Substring(3, 2) + "-" + dia.Substring(0, 2) + "\",";
                    guarda_aviso += "•Fecha Recepción: " + dia + "\n";
                }
            }            

            if (checkBox10.Checked == true)
            {
                guarda += "fecha_notificacion=NULL,";
                guarda_aviso += "•Fecha Notificación: VACÍO\n";
            }
            else
            {
                if (checkBox4.Checked == true)
                {
                    string dia = dateTimePicker3.Value.ToShortDateString();
                    guarda += "fecha_notificacion=\"" + dia.Substring(6, 4) + "-" + dia.Substring(3, 2) + "-" + dia.Substring(0, 2) + "\",";
                    guarda_aviso += "•Fecha Notificación: " + dia + "\n";
                }
            }
        }

        public void guardar_datos_estado_credito()
        {
            if(comboBox3.SelectedIndex > -1)
            {
                guarda += "status=\"" + comboBox3.SelectedItem + "\",";
                guarda_aviso += "•Estatus: " + comboBox3.SelectedItem + "\n";
            }

            if(textBox7.Text.Length>1){
                guarda += "observaciones=\"" + textBox7.Text + "\",";
                guarda_aviso += "•Observaciones: "+textBox7.Text+"\n";
            }

            if (comboBox7.SelectedIndex > -1)
            {
                if(comboBox7.SelectedIndex==0){
                    guarda += "estado_cartera=\"-\",";
                    guarda_aviso += "•Estado Cartera: VACÍO\n";
                }else{
                    guarda += "estado_cartera=\"" + comboBox7.SelectedItem + "\",";
                    guarda_aviso += "•Estado Cartera: " + comboBox7.SelectedItem + "\n";
                }
            }

            if (checkBox11.Checked == true)
            {
                guarda += "fecha_cartera=NULL,";
                guarda_aviso += "•Fecha Cartera: VACÍO\n";
            }
            else
            {
                if (checkBox5.Checked == true)
                {
                    string dia = dateTimePicker4.Value.ToShortDateString();
                    guarda += "fecha_cartera=\"" + dia.Substring(6, 4) + "-" + dia.Substring(3, 2) + "-" + dia.Substring(0, 2) + "\",";
                    guarda_aviso += "•Fecha Cartera: " + dia + "\n";
                }
            }

            if (checkBox12.Checked == true)
            {
                guarda += "fecha_depuracion=NULL,";
                guarda_aviso += "•Fecha Depuración: VACÍO\n";
            }
            else
            {
                if (checkBox6.Checked == true)
                {
                    string dia = dateTimePicker5.Value.ToShortDateString();
                    guarda += "fecha_depuracion=\"" + dia.Substring(6, 4) + "-" + dia.Substring(3, 2) + "-" + dia.Substring(0, 2) + "\",";
                    guarda_aviso += "•Fecha Depuración: " + dia + "\n";
                }
            }

            if(comboBox9.SelectedIndex>-1){
                guarda += "nombre_periodo=\"" + comboBox9.SelectedItem + "\",";
                guarda_aviso += "•Nombre Periodo: " + comboBox9.SelectedItem + "\n";
            }
        }

        public void guardar()
        {
            String sql = "",ids="";
            int cred_sel = 0;

            guardar_datos_noti();
            guardar_datos_estado_credito();

            for (int i = 0; i < dataGridView1.RowCount;i++ )
            {
                if (Convert.ToBoolean(dataGridView1[0, i].Value)==true)
                {
                    ids += dataGridView1[1,i].Value.ToString()+",";
                    cred_sel++;
                }               
            }

            if (cred_sel > 0)
            {
                DialogResult resul = MessageBox.Show("Se colocarán los siguientes valores a cada uno de los " + cred_sel + " créditos marcados.\n\n"+guarda_aviso+"\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (resul == DialogResult.Yes)
                {
                    conex4.conectar("base_principal");
                    ids = ids.Substring(0, ids.Length - 1);

                    guarda = guarda.Substring(0, guarda.Length - 1);

                    sql = "UPDATE datos_factura SET " + guarda + " WHERE id IN (" + ids + ")";
                    //MessageBox.Show(sql);
                    try
                    {
                        conex4.consultar(sql);
                        conex4.guardar_evento("Se editó información de los creditos con los siguientes IDs: "+ids);
                        MessageBox.Show("Los créditos seleccionados fueron actualizados correctamente","Actualización Exitosa");

                        llenar_Cb4_todos();
                        llenar_Cb9_todos();
                        dataGridView1.DataSource = null;

                        maskedTextBox9.Clear();
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        comboBox6.SelectedIndex = -1;
                        checkBox2.Checked = false;
                        checkBox3.Checked = false;
                        checkBox4.Checked = false;
                        checkBox8.Checked = false;
                        checkBox9.Checked = false;
                        checkBox10.Checked = false;
                        textBox9.Visible = false;
                        textBox10.Visible = false;
                        textBox11.Visible = false;
                        dateTimePicker1.Enabled = false;
                        dateTimePicker2.Enabled = false;
                        dateTimePicker3.Enabled = false;
                        dateTimePicker1.Value = System.DateTime.Today;
                        dateTimePicker2.Value = System.DateTime.Today;
                        dateTimePicker3.Value = System.DateTime.Today;
                        comboBox3.SelectedIndex = -1;
                        textBox7.Text = "";
                        comboBox7.SelectedIndex = -1;
                        checkBox5.Checked = false;
                        checkBox6.Checked = false;
                        checkBox11.Checked = false;
                        checkBox12.Checked = false;
                        textBox12.Visible = false;
                        textBox13.Visible = false;
                        dateTimePicker4.Enabled = false;
                        dateTimePicker5.Enabled = false;
                        dateTimePicker4.Value = System.DateTime.Today;
                        dateTimePicker5.Value = System.DateTime.Today;
                        comboBox9.SelectedIndex = -1;
                        comboBox10.SelectedIndex = -1;
                        maskedTextBox14.Clear();

                    }
                    catch (Exception ex)
                    {

                    }
                    conex4.cerrar();
                }
            }            
        }

        public DataTable copiar_datagrid()
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

        private void Edicion_en_Volumen_Load(object sender, EventArgs e)
        {
            llenar_Cb1();
            llenar_Cb2();
            llenar_Cb4_todos();
            llenar_Cb9_todos();
            //timer1.Start();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                textBox9.Visible = false;
            }
            else
            {
                dateTimePicker1.Enabled = false;

            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                dateTimePicker2.Enabled = true;
                textBox10.Visible = false;
            }
            else
            {
                dateTimePicker2.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                dateTimePicker3.Enabled = true;
                textBox11.Visible = false;
            }
            else
            {
                dateTimePicker3.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                dateTimePicker4.Enabled = true;
                textBox12.Visible = false;
            }
            else
            {
                dateTimePicker4.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                dateTimePicker5.Enabled = true;
                textBox13.Visible = false;
            }
            else
            {
                dateTimePicker5.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                try
                {
                    llenar_Cb2_v2();
                }
                catch (Exception es)
                {
                    comboBox2.Items.Clear();
                }
            }
            else
            {
                if (comboBox1.Text.Length < 1)
                {
                    comboBox2.Items.Clear();
                }
                else
                {
                    llenar_Cb2();
                }
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                textBox9.Visible = true;
            }
            else
            {
                textBox9.Visible = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                textBox10.Visible = true;
            }
            else
            {
                textBox10.Visible = false;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked == true)
            {
                textBox11.Visible = true;
            }
            else
            {
                textBox11.Visible = false;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked == true)
            {
                textBox12.Visible = true;
            }
            else
            {
                textBox12.Visible = false;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
            {
                textBox13.Visible = true;
            }
            else
            {
                textBox13.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.RowCount > 0)
            //{
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

                    buscar_con_excel();                  
                }
           //}
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            buscar_normal();
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
                    for (int i = 1; i < dataGridView1.ColumnCount;i++)
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                        cont++;
                    }
                    
                }
                else
                {
                    cont = 1;
                    for (int i = 1; i < dataGridView1.ColumnCount;i++){
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Style.BackColor = System.Drawing.Color.White;
                        cont++;
                    }
                    
                }

                if (Convert.ToBoolean(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value) == true)
                {
                    //seleccionados++;
                }
                else
                {
                    //seleccionados--;
                }

                //contar_seleccionados();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex != 0)
            {
                int cont = 1;
                bool marca = Convert.ToBoolean(dataGridView1.CurrentRow.Cells[0].Value);

                if (marca == false)
                {
                    for (int i = 1; i < (dataGridView1.ColumnCount );i++){
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                        cont++;
                    }
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = true;
                    //seleccionados++;
                }
                else
                {
                    cont = 1;
                    for (int i = 1; i < (dataGridView1.ColumnCount ); i++)
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[i].Style.BackColor = System.Drawing.Color.White;
                        cont++;
                    }
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value = false;
                    //seleccionados--;
                }
                //contar_seleccionados();
            }        
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //contar_seleccionados();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            int foun = 0;
            if (dataGridView1.RowCount > 0)
            {
                if (maskedTextBox1.Text.Length == 9)
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[6].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[6].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            //dataGridView1.Rows[i].Cells[0].Value = true;
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                            foun = 1;
                        }

                        if (dataGridView1.Rows[i].Cells[7].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[7].Selected = true;
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

                    if (foun == 0)
                    {
                        MessageBox.Show("Crédito no Encontrado", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        maskedTextBox1.SelectionStart = 0;
                        maskedTextBox1.Focus();
                    }

                    maskedTextBox1.Clear();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            contar_seleccionados();
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex > -1)
            {
                //valores[11] = comboBox4.SelectedItem.ToString();
                comboBox9.BackColor = Color.PaleGreen;
                this.GetNextControl(ActiveControl, true);
            }
            else
            {
                //valores[11] = "";
                comboBox9.BackColor = Color.Tomato;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String periodo_nvo;
            int j = 0, k = 0, mayor = 0;

            comboBox10.Enabled = true;

            if ((maskedTextBox14.BackColor.Name.Equals("PaleGreen")) && (comboBox10.SelectedIndex > -1))
            {
                if (maskedTextBox14.Text.Length > 5)
                {
                    //rcv
                    if (comboBox10.SelectedItem.ToString().StartsWith("RCV"))
                    {
                        if (Convert.ToInt32(maskedTextBox14.Text.Substring(4, 2)) < 7)
                        {
                            periodo_nvo = comboBox10.SelectedItem.ToString() + "_" + maskedTextBox14.Text;

                            while (j < comboBox9.Items.Count)
                            {
                                if (comboBox9.Items[j].ToString().Equals(periodo_nvo))
                                {
                                    k = 1;
                                }
                                j++;
                            }

                            if (k == 0)
                            {
                                comboBox9.Items.Add(periodo_nvo);
                                comboBox9.SelectedIndex = comboBox9.Items.Count - 1;
                                comboBox9.Enabled = false;
                                comboBox10.Enabled = false;
                                maskedTextBox14.Enabled = false;
                                button5.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("El periodo que intenta crear, ya existe", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                    else
                    {//COP
                        periodo_nvo = comboBox10.SelectedItem.ToString() + "_" + maskedTextBox14.Text;
                        while (j < comboBox9.Items.Count)
                        {
                            if (comboBox9.Items[j].ToString().Equals(periodo_nvo))
                            {
                                k = 1;
                            }
                            j++;
                        }

                        if (k == 0)
                        {
                            comboBox9.Items.Add(periodo_nvo);
                            comboBox9.SelectedIndex = comboBox9.Items.Count - 1;
                            comboBox9.Enabled = false;
                            comboBox10.Enabled = false;
                            maskedTextBox14.Enabled = false;
                            button5.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("El periodo que intenta crear ya existe", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else
                {
                    periodo_nvo = comboBox10.SelectedItem.ToString();
                    j = 0;
                    mayor = 0;
                    //MessageBox.Show(periodo_nvo+"|"+j+"|"+comboBox4.Items[j].ToString().Substring(0,5));
                    while (j < comboBox9.Items.Count)
                    {
                        if (comboBox9.Items[j].ToString().Length > 6)
                        {
                            if (periodo_nvo.Equals(comboBox9.Items[j].ToString().Substring(0, 6)))
                            {
                                if (Convert.ToInt32(comboBox9.Items[j].ToString().Substring(7)) > mayor)
                                {
                                    mayor = Convert.ToInt32(comboBox9.Items[j].ToString().Substring(7));
                                }
                            }
                        }
                        j++;
                    }

                    periodo_nvo = periodo_nvo + "_" + (mayor + 1);
                    comboBox9.Items.Add(periodo_nvo);
                    comboBox9.SelectedIndex = j;
                    comboBox9.Enabled = false;
                    comboBox10.Enabled = false;
                    maskedTextBox14.Enabled = false;
                    button5.Enabled = false;
                }
            }
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox10.SelectedIndex == 5 || comboBox10.SelectedIndex == 11)
            {
                maskedTextBox14.Enabled = false;
                maskedTextBox14.BackColor = Color.PaleGreen;
            }
            else
            {
                maskedTextBox14.Enabled = true;
                maskedTextBox14.BackColor = Color.White;
            }
        }

        private void maskedTextBox14_TextChanged(object sender, EventArgs e)
        {
            int anio = 0, mes = 0;

            if (maskedTextBox14.Text.Length == 6)
            {
                anio = Convert.ToInt32(maskedTextBox14.Text.Substring(0, 4));
                mes = Convert.ToInt32(maskedTextBox14.Text.Substring(4, 2));

                if (anio > 2000)
                {
                    if (mes > 0 && mes < 13)
                    {
                        //valores[1] = maskedTextBox14.Text;
                        maskedTextBox14.BackColor = Color.PaleGreen;

                    }
                    else
                    {
                        //valores[1] = "";
                        maskedTextBox14.BackColor = Color.Tomato;
                    }
                }
                else
                {
                    //valores[1] = "";
                    maskedTextBox14.BackColor = Color.Tomato;
                }
            }
            else
            {
                //valores[1] = "";
                if (maskedTextBox14.Mask.Equals("000"))
                {
                    maskedTextBox14.BackColor = Color.PaleGreen;
                }
                else
                {
                    maskedTextBox14.BackColor = Color.Tomato;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount>0){
                guardar();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.BackColor = System.Drawing.Color.MediumBlue;
            }
            else
            {
                checkBox1.BackColor = System.Drawing.Color.Transparent;
            }
        }

        private void seleccionarTodosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount>0){
                for (int j = 0; j < (dataGridView1.RowCount); j++){
                
                    for (int i = 1; i < (dataGridView1.ColumnCount); i++)
                    {
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                    }

                    dataGridView1.Rows[j].Cells[0].Value = true;
                }
                
            }else{
            }
        }

        private void desmarcarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                for (int j = 0; j < (dataGridView1.RowCount); j++)
                {
                    for (int i = 1; i < (dataGridView1.ColumnCount); i++)
                    {
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = System.Drawing.Color.White;
                        //dataGridView1.Rows[j].Cells[i].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                    }
                    dataGridView1.Rows[j].Cells[0].Value = false;
                }
            }else{
            }
        }

        private void exportarAExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                //GUARDAR EVIDENCIA EDICION
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    DataTable nn_excel = copiar_datagrid();

                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(nn_excel, "Lista");
                    wb.SaveAs(@"" + dialog_save.FileName + "");
                    //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
    }
}

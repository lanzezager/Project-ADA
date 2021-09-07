/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 15/08/2016
 * Hora: 10:31 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
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

namespace Nova_Gear.Mod40
{
	/// <summary>
	/// Description of Carga_excel40.
	/// </summary>
    public partial class Carga_excel40 : Form
    {
        public Carga_excel40()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        String archivo, ext, cad_con, tabla, hoja, cons_exc, fecha, sql, nombre, nom_per;
        int i = 0, filas = 0, tot_rows = 0, tot_cols = 0, tot_rows2 = 0, j = 0, iguales = 0, temp_igual = 0;
        double totr = 0;

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;

        //Conexion MySQL
        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();

        //Tablas
        DataTable marcados = new DataTable();
        DataTable totales = new DataTable();
        DataTable consulta = new DataTable();
        DataTable inexistentes = new DataTable();
        DataTable consultadora = new DataTable();

        //Declaracion de Hilo
        private Thread hilosecundario = null;

        public void carga_excel()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Seleccione el archivo de Excel";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archivo = dialog.FileName;
                label1.Text = dialog.SafeFileName;
                ext = archivo.Substring(((archivo.Length) - 3), 3);
                ext = ext.ToLower();

                if (ext.Equals("lsx"))
                {
                    MessageBox.Show("Asegurate de Cerrar el archivo en Excel, Antes de abrirlo aqui", "Advertencia");
                }

                //esta cadena es para archivos excel 2007 y 2010
                cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
                conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion

                carga_chema_excel();

            }
        }

        public void carga_chema_excel()
        {
            i = 0;
            filas = 0;
            comboBox1.Items.Clear();
            System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            dataGridView2.DataSource = dt;
            filas = (dataGridView2.RowCount) - 1;
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
                            comboBox1.Items.Add(tabla);
                        }
                    }
                }
                i++;
            } while (i <= filas);

            dt.Clear();
            dataGridView2.DataSource = dt; //vaciar datagrid
        }

        public void cargar_hoja_excel()
        {

            hoja = comboBox1.SelectedItem.ToString();

            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                cons_exc = "Select * from [" + hoja + "$] ";

                try
                {
                    //Si el usuario escribio el nombre de la hoja se procedera con la busqueda
                    //conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                    //conexion.Open(); //abrimos la conexion
                    dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                    if (dataAdapter.Equals(null))
                    {

                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");

                    }
                    else
                    {
                        dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                        dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        conexion.Close();//cerramos la conexion
                        dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
                        tot_rows = dataGridView1.RowCount;
                        totr = Convert.ToDouble(tot_rows);
                        tot_cols = dataGridView1.ColumnCount;
                        label2.Text = "Registros: " + tot_rows;
                        label2.Refresh();
                        //button10.Enabled=true;		
                        //button2.Enabled=false;	
                        //maskedTextBox2.Enabled=true;

                        //estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView1.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView1.ColumnCount);
                        i = 0;
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }

            }

        }

        public void carga_excel2()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Seleccione el archivo de Excel";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archivo = dialog.FileName;
                label3.Text = dialog.SafeFileName;
                ext = archivo.Substring(((archivo.Length) - 3), 3);
                ext = ext.ToLower();

                if (ext.Equals("lsx"))
                {
                    MessageBox.Show("Asegurate de Cerrar el archivo en Excel, Antes de abrirlo aqui", "Advertencia");
                }

                //esta cadena es para archivos excel 2007 y 2010
                cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
                conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion

                carga_chema_excel2();
            }
        }

        public void carga_chema_excel2()
        {
            i = 0;
            filas = 0;
            comboBox2.Items.Clear();
            System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            dataGridView2.DataSource = dt;
            filas = (dataGridView2.RowCount) - 1;
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
                            comboBox2.Items.Add(tabla);
                        }
                    }
                }
                i++;
            } while (i <= filas);

            dt.Clear();
            dataGridView2.DataSource = dt; //vaciar datagrid
        }

        public void cargar_hoja_excel2()
        {

            hoja = comboBox2.SelectedItem.ToString();

            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                cons_exc = "Select * from [" + hoja + "$] ";
                //cons_exc = "Select [NRP] from [" + hoja + "$] where [NRP] =\"Z293854410\"";
                try
                {
                    //Si el usuario escribio el nombre de la hoja se procedera con la busqueda
                    //conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                    //conexion.Open(); //abrimos la conexion
                    dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                    if (dataAdapter.Equals(null))
                    {

                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");

                    }
                    else
                    {
                        dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                        dataGridView2.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        conexion.Close();//cerramos la conexion
                        dataGridView2.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
                        tot_rows2 = dataGridView2.RowCount;
                        label4.Text = "Registros: " + tot_rows2;
                        label4.Refresh();
                        //button10.Enabled=true;		
                        //button2.Enabled=false;	
                        //maskedTextBox2.Enabled=true;

                        //estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView2.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView2.ColumnCount);
                        i = 0;
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }

            }

        }

        public void insertar40()
        {

            i = 0;
            dataGridView1.Enabled = false;
            conex.conectar("base_principal");
            do
            {
                dataGridView1.Rows[i].Cells[3].Value = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString().Substring(1);
                nombre = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                nombre = nombre.Replace('$', ' ');
                nombre = nombre.Replace('/', 'Ñ');
                nombre = nombre.TrimEnd(' ');
                dataGridView1.Rows[i].Cells[4].Value = nombre;

                fecha = dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);
                //MessageBox.Show(fecha);
                dataGridView1.Rows[i].Cells[6].Value = fecha;

                if (radioButton1.Checked == true)
                {
                    sql = "INSERT INTO mod40_sua (del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago,folio_sua,fecha_pago,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,importe_pago,observaciones) " +
                                            "VALUES(\"14\"," +//del
                                            "\"38\"," +//sub 
                                            "\"" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "\"," +//cve_unit
                                            "\"" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + "\"," +//NRP					
                                            "\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\"," +//nss
                                            "\"AAAA010101\"," +//rfc
                                            "\" \"," +//curp
                                            "\"" + dataGridView1.Rows[i].Cells[4].FormattedValue.ToString() + "\"," +//nombre
                                            "30," +//dias_cot
                                            "000," +	//pza_suc
                                            "\"" + dataGridView1.Rows[i].Cells[5].FormattedValue.ToString() + "\"," +//periodo_pago
                                            "\"" + dataGridView1.Rows[i].Cells[7].FormattedValue.ToString() + "\"," +//folio_sua
                                            "\"" + dataGridView1.Rows[i].Cells[6].FormattedValue.ToString() + "\"," +//fecha_pago
                                            "" + "0.00" + "," +//eym_pree
                                            "" + "0.00" + "," +//eym_vida
                                            "" + "0.00" + "," +//eym_cesa_vej
                                            "" + "0.00" + "," +//eym_pree_o
                                            "" + "0.00" + "," +//eym_vida_o
                                            "" + "0.00" + "," +//eym_cesa_vej_o
                                            "\"-\"," +//per_cd_sua
                                            "\"10\"," +//tipo_modalidad
                                            "" + "0.00" + "," +//importe pago
                                            "\"" + dataGridView1.Rows[i].Cells[8].FormattedValue.ToString() + "\"" +//observaciones
                                            " )";
                }

                if (radioButton2.Checked == true)
                {
                    sql = "INSERT INTO mod40_sua (del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago,folio_sua,fecha_pago,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,importe_pago,observaciones) " +
                                           "VALUES(\"14\"," +//del
                                           "\"38\"," +//sub 
                                           "\"" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "\"," +//cve_unit
                                           "\"" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + "\"," +//NRP					
                                           "\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\"," +//nss
                                           "\"AAAA010101\"," +//rfc
                                           "\" \"," +//curp
                                           "\"" + dataGridView1.Rows[i].Cells[4].FormattedValue.ToString() + "\"," +//nombre
                                           "30," +//dias_cot
                                           "000," +	//pza_suc
                                           "\"" + dataGridView1.Rows[i].Cells[5].FormattedValue.ToString() + "\"," +//periodo_pago
                                           "\"" + dataGridView1.Rows[i].Cells[7].FormattedValue.ToString() + "\"," +//folio_sua
                                           "\"" + dataGridView1.Rows[i].Cells[6].FormattedValue.ToString() + "\"," +//fecha_pago
                                           "" + "0.00" + "," +//eym_pree
                                           "" + "0.00" + "," +//eym_vida
                                           "" + "0.00" + "," +//eym_cesa_vej
                                           "" + "0.00" + "," +//eym_pree_o
                                           "" + "0.00" + "," +//eym_vida_o
                                           "" + "0.00" + "," +//eym_cesa_vej_o
                                           "\"-\"," +//per_cd_sua
                                           "\"" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString().Substring(4, 2) + "\"," +//tipo_modalidad
                                           "" + "0.00" + "," +//importe pago
                                           "\"" + dataGridView1.Rows[i].Cells[8].FormattedValue.ToString() + " | " + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + "\"" +//observaciones
                                           " )";
                }

                if (radioButton3.Checked == true)
                {
                    sql = "INSERT INTO mod40_sua (del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago,folio_sua,fecha_pago,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,importe_pago,observaciones) " +
                                           "VALUES(\"14\"," +//del
                                           "\"38\"," +//sub 
                                           "\"" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "\"," +//cve_unit
                                           "\"" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + "\"," +//NRP					
                                           "\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\"," +//nss
                                           "\"AAAA010101\"," +//rfc
                                           "\" \"," +//curp
                                           "\"" + dataGridView1.Rows[i].Cells[4].FormattedValue.ToString() + "\"," +//nombre
                                           "30," +//dias_cot
                                           "000," +	//pza_suc
                                           "\"" + dataGridView1.Rows[i].Cells[5].FormattedValue.ToString() + "\"," +//periodo_pago
                                           "\"" + dataGridView1.Rows[i].Cells[7].FormattedValue.ToString() + "\"," +//folio_sua
                                           "\"" + dataGridView1.Rows[i].Cells[6].FormattedValue.ToString() + "\"," +//fecha_pago
                                           "" + "0.00" + "," +//eym_pree
                                           "" + "0.00" + "," +//eym_vida
                                           "" + "0.00" + "," +//eym_cesa_vej
                                           "" + "0.00" + "," +//eym_pree_o
                                           "" + "0.00" + "," +//eym_vida_o
                                           "" + "0.00" + "," +//eym_cesa_vej_o
                                           "\"-\"," +//per_cd_sua
                                           "\"40\"," +//tipo_modalidad
                                           "" + "0.00" + "," +//importe pago
                                           "\"" + dataGridView1.Rows[i].Cells[8].FormattedValue.ToString() + " | " + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + "\"" +//observaciones
                                           " )";
                }

                conex.consultar(sql);
                i++;
            } while (i < dataGridView1.RowCount);

            dataGridView1.Enabled = true;
            MessageBox.Show("Listo");

        }

        public void reparar_bd()
        {

            i = 0;
            dataGridView1.Enabled = false;
            conex.conectar("base_principal");
            conex2.conectar("base_principal");

            do
            {
                if (dataGridView1.Rows[i].Cells[1].FormattedValue.ToString().Length == 9 && dataGridView1.Rows[i].Cells[2].FormattedValue.ToString().Length == 9)
                {
                    sql = "SELECT nombre_periodo,id FROM datos_factura WHERE registro_patronal2=\"" + dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Substring(0, 10) + "\" AND " +
                        "periodo=\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\" AND credito_cuotas=\"" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "\" AND " +
                        "credito_multa=\"" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + "\"";
                }
                else
                {
                    if (dataGridView1.Rows[i].Cells[1].FormattedValue.ToString().Length == 9)
                    {
                        sql = "SELECT nombre_periodo,id FROM datos_factura WHERE registro_patronal2=\"" + dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Substring(0, 10) + "\" AND " +
                        "periodo=\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\" AND credito_cuotas=\"" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "\"";
                    }
                    else
                    {
                        if (dataGridView1.Rows[i].Cells[2].FormattedValue.ToString().Length == 9)
                        {
                            sql = "SELECT nombre_periodo,id FROM datos_factura WHERE registro_patronal2=\"" + dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Substring(0, 10) + "\" AND " +
                            "periodo=\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\" AND credito_multa=\"" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + "\"";
                        }
                        else
                        {
                            if (dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Length > 9 && dataGridView1.Rows[i].Cells[3].FormattedValue.ToString().Length > 5)
                            {
                                sql = "SELECT nombre_periodo,id FROM datos_factura WHERE registro_patronal2=\"" + dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Substring(0, 10) + "\" AND " +
                                "periodo=\"" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\"";
                            }
                        }
                    }
                }

                dataGridView2.DataSource = conex.consultar(sql);

                if (dataGridView2.RowCount > 0)
                {
                    if (dataGridView1.Rows[i].Cells[4].FormattedValue.ToString().Length > 0)
                    {
                        sql = "UPDATE datos_factura SET observaciones=\"ENTREGADO A NOTIFICADORES COMO: OFICIOS_20160815\", fecha_entrega=\"2016-08-15\", status=\"0\", " +
                            "sector_notificacion_actualizado=" + dataGridView1.Rows[i].Cells[4].FormattedValue.ToString() + " WHERE id=" + dataGridView2.Rows[0].Cells[1].FormattedValue.ToString() + ";";
                    }
                    //MessageBox.Show(sql);
                    conex2.consultar(sql);
                }
                else
                {
                    textBox1.AppendText(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString() + ", " +
                                        dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + ", " +
                                        dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + ", " +
                                        dataGridView1.Rows[i].Cells[3].FormattedValue.ToString() + "\r\n");
                }

                i++;
            } while (i < dataGridView1.RowCount);
            MessageBox.Show("Listo");
            textBox1.Visible = true;
        }

        public void comparar()
        {
            i = 0;
            j = 0;
            iguales = 0;
            do
            {
                j = 0;
                do
                {
                    if (dataGridView1.Rows[i].Cells[0].FormattedValue.ToString().Equals(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()))
                    {
                        iguales++;
                    }
                    j++;
                } while (j < dataGridView2.RowCount);

                if (temp_igual == iguales)
                {
                    textBox1.AppendText(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString() + "\r\n");
                }
                temp_igual = iguales;

                i++;
                label5.Text = "Comparado: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Filas Iguales entre los dos archivos: " + iguales);
            textBox1.Visible = true;
        }

        public void comparar2()
        {
            i = 0;
            j = 0;
            iguales = 0;
            int encontrado = 0, difs = 0, ultimo_encontrado = 0;
            String combinado1 = "", combinado2 = "";
            do
            {
                j = ultimo_encontrado;
                combinado1 = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString() + "-" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "-" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                do
                {
                    combinado2 = dataGridView2.Rows[j].Cells[0].FormattedValue.ToString() + "-" + dataGridView2.Rows[j].Cells[1].FormattedValue.ToString() + "-" + dataGridView2.Rows[j].Cells[2].FormattedValue.ToString();

                    if (combinado1.Equals(combinado2) == true)
                    {
                        iguales++;
                        textBox1.AppendText(combinado2 + "\r\n");
                        encontrado = 1;
                        ultimo_encontrado = j;
                        j = 10;
                    }

                    j++;
                } while (j < 5);

                if (encontrado == 0)
                {
                    //MessageBox.Show("no encontrado: "+i);
                    textBox2.AppendText(combinado1 + " " + combinado2 + "\r\n");
                    difs++;

                }
                encontrado = 0;
                combinado1 = "";
                combinado2 = "";
                i++;
                label5.Text = "Comparado: " + i;
                label5.Refresh();
                //} while (i < 50);
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Filas Iguales entre los dos archivos: " + iguales + "\n Filas Diferentes: " + difs);
            textBox1.Visible = true;
            textBox2.Visible = true;
        }

        public void marcar_en_bd_v2()
        {
            i = 0;
            int j = 0;
            String id, pass, fecha1, fecha2, fecha3, fecha4, periodo_mes, periodo_anio;

            conex.conectar("inventario");

            do
            {
                //fecha=dataGridView1.Rows[i].Cells[10].FormattedValue.ToString();
                //fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
                try
                {
                    fecha1 = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString().Substring(6, 4) + "-" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString().Substring(3, 2) + "-" + dataGridView1.Rows[i].Cells[3].FormattedValue.ToString().Substring(0, 2);

                    if ((fecha1.Contains("20")) || (fecha1.Contains("19"))) { }
                    else
                    {
                        fecha1 = "0000-00-00";
                    }

                }
                catch (Exception e1)
                {
                    fecha1 = "0000-00-00";
                }

                try
                {
                    fecha2 = dataGridView1.Rows[i].Cells[9].FormattedValue.ToString().Substring(6, 4) + "-" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString().Substring(3, 2) + "-" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString().Substring(0, 2);
                    if ((fecha2.Contains("20")) || (fecha2.Contains("19"))) { }
                    else
                    {
                        fecha2 = "0000-00-00";
                    }
                }
                catch (Exception e1)
                {
                    fecha2 = "0000-00-00";
                }

                try
                {
                    fecha3 = dataGridView1.Rows[i].Cells[10].FormattedValue.ToString().Substring(6, 4) + "-" + dataGridView1.Rows[i].Cells[10].FormattedValue.ToString().Substring(3, 2) + "-" + dataGridView1.Rows[i].Cells[10].FormattedValue.ToString().Substring(0, 2);
                    if ((fecha3.Contains("20")) || (fecha3.Contains("19"))) { }
                    else
                    {
                        fecha3 = "0000-00-00";
                    }
                }
                catch (Exception e1)
                {
                    fecha3 = "0000-00-00";
                }

                try
                {
                    fecha4 = dataGridView1.Rows[i].Cells[12].FormattedValue.ToString().Substring(6, 4) + "-" + dataGridView1.Rows[i].Cells[12].FormattedValue.ToString().Substring(3, 2) + "-" + dataGridView1.Rows[i].Cells[12].FormattedValue.ToString().Substring(0, 2);
                    if ((fecha4.Contains("20")) || (fecha4.Contains("19"))) { }
                    else
                    {
                        fecha4 = "0000-00-00";
                    }
                }
                catch (Exception e1)
                {
                    fecha4 = "0000-00-00";
                }

                periodo_mes = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString().Substring(0, 2);
                periodo_anio = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString().Substring(3, 4);

                //sql="UPDATE usuarios SET contrasena_segura = AES_ENCRYPT(\""+pass+"\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\") WHERE id_usuario=\""+id+"\"";
                sql = "INSERT INTO base_inventario (clase_doc,reg_pat,mov,fecha_mov,sector,credito,ce,periodo_mes,periodo_anio,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,no_cuaderno) " +
                      "VALUES ( \"" + dataGridView1.Rows[i].Cells[0].FormattedValue.ToString() + "\"," +//clase doc
                               "\"" + dataGridView1.Rows[i].Cells[1].FormattedValue.ToString() + "\"," +//reg_pat
                               "" + dataGridView1.Rows[i].Cells[2].FormattedValue.ToString() + "," +//mov
                               "\"" + fecha1 + "\"," +//fecha_mov--
                               "" + dataGridView1.Rows[i].Cells[4].FormattedValue.ToString() + "," +//sector
                               "\"" + dataGridView1.Rows[i].Cells[5].FormattedValue.ToString() + "\"," +//credito
                               "\"" + dataGridView1.Rows[i].Cells[6].FormattedValue.ToString() + "\"," +//ce
                               "\"" + periodo_mes + "\"," +//periodo_mes
                               "\"" + periodo_anio + "\"," +//periodo_anio		
                               "\"" + dataGridView1.Rows[i].Cells[8].FormattedValue.ToString() + "\"," +//tipo_doc
                               "\"" + fecha2 + "\"," +//fecha_alta--
                               "\"" + fecha3 + "\"," +//fecha_not--
                               "\"" + dataGridView1.Rows[i].Cells[11].FormattedValue.ToString() + "\"," +//incidencia
                               "\"" + fecha4 + "\"," +//fecha_incidencia--
                               "\"" + dataGridView1.Rows[i].Cells[13].FormattedValue.ToString() + "\"," +//dias
                               "" + dataGridView1.Rows[i].Cells[14].FormattedValue.ToString() + "," +//importe
                               "" + dataGridView1.Rows[i].Cells[15].FormattedValue.ToString() + ")";//no_cuaderno

                conex.consultar(sql);

                //sql="SELECT observaciones FROM datos_factura WHERE registro_patronal2=\""+rp+"\" AND credito_multa=\""+dataGridView1.Rows[i].Cells[6].FormattedValue.ToString()+"\";";
                //dataGridView2.DataSource=
                //conex.consultar(sql);
                //MessageBox.Show(sql);

                /*if(dataGridView2.Rows[0].Cells[0].FormattedValue.ToString().Equals("SE DEPURA CREDITO POR PRESENTAR RB EN SISCOB")){
                    textBox1.AppendText(dataGridView1.Rows[i].Cells[2].FormattedValue.ToString()+", "+dataGridView1.Rows[i].Cells[5].FormattedValue.ToString()+"\r\n");
                    j++;
                }*/

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);


            textBox1.Visible = true;
            MessageBox.Show("Listo " + i + "|" + j);
        }

        public void marcar_en_bd_1()
        {
            conex.conectar("base_principal");
            String reg_pat, cred_cuo, cred_mul, periodo;
            i = 0;
            do
            {
                reg_pat = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                cred_cuo = dataGridView1.Rows[i].Cells[9].FormattedValue.ToString();
                cred_mul = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                //periodo = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();

                /*consulta.Clear();
        		
                sql="SELECT columna_lz FROM datos_factura WHERE registro_patronal1=\""+reg_pat+"\" AND credito_cuotas=\""+cred_cuo+"\" AND credito_multa=\""+cred_mul+"\"";
                consulta=conex.consultar(sql);
                periodo=consulta.Rows[0][0].ToString();
                */
                //cred_cuo = cred_cuo.Substring(6,4) + "-" + cred_cuo.Substring(3,2) + "-" + cred_cuo.Substring(0,2);

                sql = "UPDATE datos_factura SET status=\"DEPURACION MANUAL\",folio_sipare_sua=\"" + cred_cuo + "\",fecha_depuracion=\"2017-02-14\",observaciones=\"CORE_2017_02_14\",columna_lz=\"afectar_depu_manu_14-02-2017_2\" WHERE registro_patronal2=\"" + reg_pat + "\" AND credito_multa=\"" + cred_mul + "\"";
                conex.consultar(sql);

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
        }

        public void marcar_en_bd_v3()
        {

            String per_m = "", per_a = "", reg_pat = "", credito = "", periodo, sql, marca = "";
            int dgv1 = 0, dgv2 = 0;

            conex.conectar("base_principal");
            conex2.conectar("inventario");

            marcados.Columns.Add("reg_pat");
            marcados.Columns.Add("credito");
            marcados.Columns.Add("periodo");
            marcados.Columns.Add("marca");

            totales.Columns.Add("reg_pat");
            totales.Columns.Add("credito");
            totales.Columns.Add("periodo");
            totales.Columns.Add("marca");

            inexistentes.Columns.Add("reg_pat");
            inexistentes.Columns.Add("credito");
            inexistentes.Columns.Add("periodo");

            sql = "SELECT reg_pat,credito,periodo_mes,periodo_anio FROM inventario.base_inventario WHERE coincidente  =\"0\" or coincidente =\"-\"";
            Invoke(new MethodInvoker(delegate
            {
                dataGridView1.DataSource = conex2.consultar(sql);
                dgv1 = dataGridView1.RowCount;
            }));
            i = 15000;

            do
            {

                Invoke(new MethodInvoker(delegate
                {
                    reg_pat = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    credito = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    per_m = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    per_a = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    //marca=dataGridView1.Rows[i].Cells[12].Value.ToString();
                }));
                reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2);


                periodo = per_a + per_m;

                sql = "UPDATE datos_factura SET columna_lz= \"no_inventariado\" WHERE registro_patronal2=\"" + reg_pat + "\" AND credito_cuotas=\"" + credito + "\" AND periodo=\"" + periodo + "\" ";
                consulta = conex.consultar(sql);

                sql = "UPDATE datos_factura SET columna_lz= \"no_inventariado\" WHERE registro_patronal2=\"" + reg_pat + "\" AND credito_multa=\"" + credito + "\" AND periodo=\"" + periodo + "\" ";
                consulta = conex.consultar(sql);

                /*DataView vista = new DataView(consulta);
        		
                vista.RowFilter="registro_patronal2='"+reg_pat+"' AND credito_cuotas='"+credito+"' AND periodo='"+periodo+"'";
                Invoke(new MethodInvoker(delegate{dataGridView2.DataSource=vista;
                                            dgv2=dataGridView2.RowCount;
                                         }));
        		
                if(dgv2 > 0){
                    if(marca.Equals("1")||marca.Equals("2")||marca.Equals("3")){
                        marcados.Rows.Add(reg_pat,credito,periodo);
                    }else{
                        totales.Rows.Add(reg_pat,credito,periodo);
                    }
                }else{
                    DataView vista_multa = new DataView(consulta);
                    vista.RowFilter="registro_patronal2='"+reg_pat+"' AND credito_multa='"+credito+"' AND periodo='"+periodo+"'";
                    Invoke(new MethodInvoker(delegate{dataGridView2.DataSource=vista;
                                            dgv2=dataGridView2.RowCount;
                                         }));
					
                    if(dgv2 > 0){
                        if(marca.Equals("1")||marca.Equals("2")||marca.Equals("3")){
                            marcados.Rows.Add(reg_pat,credito,periodo,marca);
                        }else{
                            totales.Rows.Add(reg_pat,credito,periodo);
                        }
                    }else{
                        inexistentes.Rows.Add(reg_pat,credito,periodo);
                    }
                }*/
                //dataGridView2.Rows.Clear();
                i++;
                Invoke(new MethodInvoker(delegate
                {/*dataGridView2.DataSource=vista;*/
                    label5.Text = "Cuenta: " + i;
                    label5.Refresh();
                }));

            } while (i < dgv1);


            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(marcados, "hoja_lz");
                wb.SaveAs(@"mod40/marcados.xlsx");

            }
            catch (Exception es)
            {

            }

            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(totales, "hoja_lz");
                wb.SaveAs(@"mod40/totales.xlsx");

            }
            catch (Exception es)
            {

            }

            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(inexistentes, "hoja_lz");
                wb.SaveAs(@"mod40/inexistentes.xlsx");

            }
            catch (Exception es)
            {

            }

            MessageBox.Show("Listo " + i);
        }

        public void marcar_en_bd_oficios_original()
        {
            conex.conectar("base_principal");
            String reg_nss, ra_soc, folio, acuerdo, fecha_of, emisor, sector, receptor, fecha_capt, fecha_recep_contro, fecha_noti, fecha_entrega_contro, fecha_not_espe, controlador, oficios, nn, observaciones;
            i = 0;

            do
            {
                reg_nss = dataGridView1.Rows[i].Cells[0].Value.ToString();
                ra_soc = dataGridView1.Rows[i].Cells[1].Value.ToString();
                folio = dataGridView1.Rows[i].Cells[2].Value.ToString();
                acuerdo = dataGridView1.Rows[i].Cells[3].Value.ToString();
                fecha_of = dataGridView1.Rows[i].Cells[4].Value.ToString();
                emisor = dataGridView1.Rows[i].Cells[5].Value.ToString();
                sector = dataGridView1.Rows[i].Cells[6].Value.ToString();
                receptor = dataGridView1.Rows[i].Cells[7].Value.ToString();
                fecha_capt = dataGridView1.Rows[i].Cells[8].Value.ToString();
                fecha_recep_contro = dataGridView1.Rows[i].Cells[9].Value.ToString();
                fecha_noti = dataGridView1.Rows[i].Cells[10].Value.ToString();
                fecha_entrega_contro = dataGridView1.Rows[i].Cells[11].Value.ToString();
                fecha_not_espe = dataGridView1.Rows[i].Cells[12].Value.ToString();
                controlador = dataGridView1.Rows[i].Cells[13].Value.ToString();

                oficios = dataGridView1.Rows[i].Cells[14].Value.ToString();
                observaciones = dataGridView1.Rows[i].Cells[15].Value.ToString();
                nn = dataGridView1.Rows[i].Cells[16].Value.ToString();

                if (fecha_of.Length > 9)
                {
                    fecha_of = "\"" + fecha_of.Substring(6, 4) + "/" + fecha_of.Substring(3, 2) + "/" + fecha_of.Substring(0, 2) + "\"";
                }
                else
                {
                    fecha_of = "NULL";
                }

                if (fecha_capt.Length > 9)
                {
                    fecha_capt = "\"" + fecha_capt.Substring(6, 4) + "/" + fecha_capt.Substring(3, 2) + "/" + fecha_capt.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_capt = "NULL";
                }

                if (fecha_recep_contro.Length > 9)
                {
                    fecha_recep_contro = "\"" + fecha_recep_contro.Substring(6, 4) + "/" + fecha_recep_contro.Substring(3, 2) + "/" + fecha_recep_contro.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_recep_contro = "NULL";
                }

                if (fecha_noti.Length > 9)
                {
                    fecha_noti = "\"" + fecha_noti.Substring(6, 4) + "/" + fecha_noti.Substring(3, 2) + "/" + fecha_noti.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_noti = "NULL";
                }

                if (fecha_entrega_contro.Length > 9)
                {
                    fecha_entrega_contro = "\"" + fecha_entrega_contro.Substring(6, 4) + "/" + fecha_entrega_contro.Substring(3, 2) + "/" + fecha_entrega_contro.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_entrega_contro = "NULL";
                }

                if (fecha_not_espe.Length > 9)
                {
                    fecha_not_espe = "\"" + fecha_not_espe.Substring(6, 4) + "/" + fecha_not_espe.Substring(3, 2) + "/" + fecha_not_espe.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_not_espe = "NULL";
                }

                sql = "INSERT INTO oficios (reg_nss,razon_social,folio,acuerdo,fecha_oficio,emisor,sector,receptor,fecha_captura,fecha_recep_contro,fecha_notificacion,fecha_devolucion_not,fecha_visita,controlador,periodo_oficio,nn,observaciones) VALUES " +
                      " (\"" + reg_nss + "\",\"" + ra_soc + "\",\"" + folio + "\",\"" + acuerdo + "\"," + fecha_of + ",\"" + emisor + "\",\"" + sector + "\",\"" + receptor + "\"," + fecha_capt + "," + fecha_recep_contro + "," + fecha_noti + "," + fecha_entrega_contro + "," + fecha_not_espe + ",\"" + controlador + "\",\"" + oficios + "\",\"" + nn + "\",\"" + observaciones + "\")";
                conex.consultar(sql);

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
        }

        public void marcar_en_bd_4()
        {
            conex.conectar("base_principal");
            String reg_nss, ra_soc, folio, acuerdo, fecha_of, emisor, sector, receptor, fecha_capt, fecha_recep_contro, fecha_noti, fecha_entrega_contro, fecha_not_espe, controlador, oficios, nn, observaciones;
            i = 0;

            do
            {
                reg_nss = dataGridView1.Rows[i].Cells[0].Value.ToString();
                ra_soc = dataGridView1.Rows[i].Cells[1].Value.ToString();
                folio = dataGridView1.Rows[i].Cells[2].Value.ToString();
                acuerdo = dataGridView1.Rows[i].Cells[3].Value.ToString();
                fecha_of = dataGridView1.Rows[i].Cells[4].Value.ToString();
                emisor = dataGridView1.Rows[i].Cells[5].Value.ToString();
                sector = dataGridView1.Rows[i].Cells[6].Value.ToString();
                receptor = dataGridView1.Rows[i].Cells[7].Value.ToString();
                fecha_capt = dataGridView1.Rows[i].Cells[8].Value.ToString();
                fecha_recep_contro = dataGridView1.Rows[i].Cells[9].Value.ToString();
                fecha_noti = dataGridView1.Rows[i].Cells[10].Value.ToString();
                fecha_entrega_contro = dataGridView1.Rows[i].Cells[11].Value.ToString();
                fecha_not_espe = dataGridView1.Rows[i].Cells[12].Value.ToString();
                controlador = dataGridView1.Rows[i].Cells[13].Value.ToString();

                //oficios=dataGridView1.Rows[i].Cells[14].Value.ToString();
                //observaciones=dataGridView1.Rows[i].Cells[15].Value.ToString();
                //nn=dataGridView1.Rows[i].Cells[16].Value.ToString();

                if (fecha_of.Length > 9)
                {
                    fecha_of = "\"" + fecha_of.Substring(6, 4) + "/" + fecha_of.Substring(3, 2) + "/" + fecha_of.Substring(0, 2) + "\"";
                }
                else
                {
                    fecha_of = "NULL";
                }

                if (fecha_capt.Length > 9)
                {
                    fecha_capt = "\"" + fecha_capt.Substring(6, 4) + "/" + fecha_capt.Substring(3, 2) + "/" + fecha_capt.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_capt = "NULL";
                }

                if (fecha_recep_contro.Length > 9)
                {
                    fecha_recep_contro = "\"" + fecha_recep_contro.Substring(6, 4) + "/" + fecha_recep_contro.Substring(3, 2) + "/" + fecha_recep_contro.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_recep_contro = "NULL";
                }

                if (fecha_noti.Length > 9)
                {
                    fecha_noti = fecha_noti.Substring(6, 4) + "-" + fecha_noti.Substring(3, 2) + "-" + fecha_noti.Substring(0, 2);
                    fecha_noti = "fecha_notificacion=\"" + fecha_noti + "\",fecha_visita=\"" + fecha_noti + "\"";
                }
                else
                {
                    if (fecha_noti.Equals("NN"))
                    {
                        if (fecha_not_espe.Length > 9)
                        {
                            fecha_not_espe = fecha_not_espe.Substring(6, 4) + "-" + fecha_not_espe.Substring(3, 2) + "-" + fecha_not_espe.Substring(0, 2);
                            fecha_noti = "nn=\"NN\",fecha_visita=\"" + fecha_not_espe + "\"";
                        }
                        else
                        {
                            fecha_noti = "nn=\"NN\"";
                        }
                    }
                }

                if (fecha_entrega_contro.Length > 9)
                {
                    fecha_entrega_contro = "\"" + fecha_entrega_contro.Substring(6, 4) + "/" + fecha_entrega_contro.Substring(3, 2) + "/" + fecha_entrega_contro.Substring(0, 2) + "\" ";
                }
                else
                {
                    fecha_entrega_contro = "NULL";
                }

                if (fecha_noti.Length > 0)
                {
                    sql = "UPDATE oficios SET " + fecha_noti + " WHERE reg_nss like \"%" + reg_nss + "%\" AND razon_social like \"%" + ra_soc + "%\" AND folio like \"%" + folio + "%\" AND acuerdo like \"%" + acuerdo + "%\"";
                    conex.consultar(sql);
                }
                //MessageBox.Show(sql);
                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
        }
        //multas
        public void marcar_en_bd2()
        {

            conex.conectar("base_principal");
            String reg_nss, reg_nss1, ra_soc, credito_cop, sector_ori, sector_nvo, observaciones, dom, loc, cp, credito_multa, imp_mul, rfc;
            i = 0;

            do
            {
                reg_nss = dataGridView1.Rows[i].Cells[0].Value.ToString();
                reg_nss1 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                ra_soc = dataGridView1.Rows[i].Cells[2].Value.ToString();
                observaciones = dataGridView1.Rows[i].Cells[3].Value.ToString();
                credito_cop = dataGridView1.Rows[i].Cells[4].Value.ToString();
                credito_multa = dataGridView1.Rows[i].Cells[5].Value.ToString();
                //	loc=dataGridView1.Rows[i].Cells[6].Value.ToString();
                //  cp=dataGridView1.Rows[i].Cells[7].Value.ToString();
                //	rfc=dataGridView1.Rows[i].Cells[8].Value.ToString();
                //  sector_ori = dataGridView1.Rows[i].Cells[9].Value.ToString();

                reg_nss = reg_nss + reg_nss1;
                reg_nss1 = reg_nss.Substring(0, 3) + "-" + reg_nss.Substring(3, 5) + "-" + reg_nss.Substring(8, 2) + "-" + reg_nss1;

                //MessageBox.Show("|"+reg_nss1+"|"+reg_nss);
                //loc=loc.TrimEnd(null)+" CP:"+cp;
                observaciones = "FOLIO ORIGINAL DE LA MULTA: " + observaciones;

                sql = "INSERT INTO datos_factura(nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,sector_notificacion_actualizado,subdelegacion,incidencia,tipo_documento,columna_lz,observaciones)" +
                        "values(\"CLEM26D-2017\",\"" + reg_nss1 + "\",\"" + reg_nss + "\",\"" + reg_nss.Substring(0, 10) + "\",\"" + ra_soc + "\",\"201703\",\"" + credito_cop + "\",\"" + credito_multa + "\",\"0.00\",\"0.00\",\"0\",\"0\",\"38\",\"01\",\"89\",\"INGRESO MULTAS 15/05/2017\",\"" + observaciones + "\")";

                //sql="INSERT INTO ema201702 (reg_pat, reg_pat1, razon_social, num_credito, periodo, num_trabajadores, sector_not, importe, importe_multa, status, domicilio, localidad) VALUES"+
                //	"(\""+reg_nss.Substring(0,10)+"\",\""+reg_nss+"\",\""+ra_soc+"\",\""+credito_cop+"\",\"201702\",\"0\",\""+sector_ori+"\",\"0.00\",\"0.00\",\"0\",\""+dom+"\",\""+loc+"\")";

                conex.consultar(sql);

                //MessageBox.Show(sql);
                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
        }
        //depur
        public void marcar_en_bd_depu()
        {
            conex.conectar("base_principal");
            String id, stat, fech_est, fech_not, fech_cart;
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//id
                stat = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//status
                //fech_est=dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//fecha_estrados
                //fech_not= dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
                //fech_cart = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();

                /*consulta.Clear();
        		
                sql="SELECT columna_lz FROM datos_factura WHERE registro_patronal1=\""+reg_pat+"\" AND credito_cuotas=\""+cred_cuo+"\" AND credito_multa=\""+cred_mul+"\"";
                consulta=conex.consultar(sql);
                periodo=consulta.Rows[0][0].ToString();
                */
                //cred_cuo = cred_cuo.Substring(6,4) + "-" + cred_cuo.Substring(3,2) + "-" + cred_cuo.Substring(0,2);23/03/2017

                /* if (fech_cart.Equals("NULL"))
                 {
                     fech_cart = "fecha_cartera=null,";
                 }
                 else
                 {
                     fech_cart = "fecha_cartera=\""+fech_cart+"\",";
                 }
 */
                sql = "UPDATE datos_factura SET status=\"0\",fecha_entrega=null, columna_lz=\"cambio_sectores_11-04-2017\" WHERE registro_patronal2=\"" + id + "\" and credito_cuotas=\"" + stat + "\"";
                conex.consultar(sql);

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
        }
		
        public void marcar_en_bd_()
        {
            conex.conectar("base_principal");
            String id, stat, per, folio;
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                stat = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//stat
                //folio= dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//folio
                per = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//observaciones

                sql = "UPDATE datos_factura SET fecha_notificacion=NULL, status=\"" + stat + "\", observaciones=\"" + per + "\", columna_lz=\"Se Modifica fecha y status 19/06/2017\" WHERE id=" + id + " ";
                conex.consultar(sql);

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
        }

        public void marcar_en_bd_inv17()
        {
            conex.conectar("inventario");
            String id, inc = "", ti_doc, obser_rale, fecha_not, obser;
            //MessageBox.Show("marcar bd chida");
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                ti_doc = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//cred_cuotas
                inc = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//per_mes
                obser_rale = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();//per_anio
                //fecha_not= dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();//fech not
                //obser=dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();//fech not
                //id=id.Substring(0,10);
                sql = "UPDATE base_inventario SET tipo_inv=\"MEX\" WHERE reg_pat=\"" + id + "\" and credito=\"" + ti_doc + "\" and periodo_mes=\"" + inc + "\" and periodo_anio=\"" + obser_rale + "\" ";
                //MessageBox.Show(sql);
                conex.consultar(sql);

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            //textBox1.Visible=true;
        }
        //inv
        public void marcar_en_bd__()
        {
            int id = 0, accion = 0;
            Random azar = new Random();

            conex.conectar("inventario");
            while (id < 55000)
            {
                accion = azar.Next(0, 100);


                if (accion <= 55)
                {//total
                    conex.consultar("UPDATE base_inventario SET coincidente=\"TOTAL\" WHERE idbase_inventario=" + id + "");
                }

                if ((accion > 55) && (accion <= 65))
                {//parcial
                    conex.consultar("UPDATE base_inventario SET coincidente=\"PARCIAL\" WHERE idbase_inventario=" + id + "");
                }

                if ((accion > 65) && (accion <= 75))
                {//otro
                    conex.consultar("UPDATE base_inventario SET coincidente=\"OTRO\" WHERE idbase_inventario=" + id + "");
                }

                if (accion > 75)
                {//vacio

                }

                id++;
                label5.Text = "Cuenta: " + id;
                label5.Refresh();


            }
        }
        //conseguir id --------------
         public void marcar_en_bd_b()
        {
            conex.conectar("base_principal");
            String id, inc = "", ti_doc, obser_rale, fecha_not, folio, status;
            //MessageBox.Show("marcar bd chida");
            textBox1.Text = "";
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                ti_doc = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//cred_cuotas
                inc = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//cred_mul
                //obser_rale= dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();//per
                //fecha_not= dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();//status
                //folio= dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                //status=dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                //id=id.Substring(0,10);

                //sql = "UPDATE datos_factura SET status=\"0\", fecha_entrega=null, fecha_recepcion=null,fecha_notificacion=null,fecha_cartera=null, columna_lz=\"Se reasignan creditos para emilio 240817\" "+
                //sql = "UPDATE datos_factura SET folio_not_core=nombre_periodo,status=\"0\", sector_notificacion_actualizado="+fecha_not+", fecha_entrega=null, fecha_recepcion=null,fecha_notificacion=null,fecha_cartera=null, columna_lz=\"Se reasignan creditos y se entregan como ESPECIAL_04\" "+
                //sql="UPDATE datos_factura SET nombre_periodo=\"ESPECIAL_04\" "+
                //"WHERE registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\" and nombre_periodo=\""+obser_rale+"\" ";
                //sql="UPDATE datos_factura SET sector_notificacion_actualizado="+52+", fecha_entrega=null, status=\"0\", folio_not_core=nombre_periodo, columna_lz=\"Se reasignan creditos para notificador Jimenez Lozano Francisco 040917\" WHERE registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\"";
                conex2.conectar("base_principal");
                //folio="OFICIOS_0"+inc.Substring(8,(inc.Length-8));   
                //sql = "UPDATE datos_factura SET nn=\"ESTRADOS\" ,status=\"" + fecha_not + "\", columna_lz=\"se cambia status a creditos de estrados 13/10/2017\" WHERE registro_patronal=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" AND credito_multa=\"" + inc + "\" AND periodo=\"" + obser_rale + "\"";
                if (ti_doc.Length > 5 && inc.Length > 5)
                {
                    sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" AND credito_multa=\"" + inc + "\"";
                }
                else
                {
                    if (ti_doc.Length > 5)
                    {
                        sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" ";
                    }
                    else
                    {
                        sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + id + "\" AND credito_multa=\"" + inc + "\" ";
                    }
                }
                consulta = conex2.consultar(sql);

                if (consulta.Rows.Count > 0)
                {
                    textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " " + consulta.Rows[0][0].ToString() + "\r\n");
                }
                else
                {
                    textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " NO EXISTE\r\n");
                }
                //MessageBox.Show(sql);
                /* sql="select registro_patronal2,credito_cuotas,periodo,columna_lz from datos_factura where registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\" and periodo=\""+obser_rale+"\"";
                 dataGridView2.DataSource=conex.consultar(sql);
                 if(dataGridView2.RowCount>0){
                     if(dataGridView2.Rows[j].Cells[3].Value.ToString().Contains("créditos de estrados")==true){
                         textBox1.AppendText(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[1].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[2].FormattedValue.ToString()+" CAPTURADO\r\n");
                     }else{
                         textBox1.AppendText(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[1].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[2].FormattedValue.ToString()+"  FALTANTE\r\n");
                     }
                 }else{
                     textBox1.AppendText(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+"  "+dataGridView1.Rows[i].Cells[1].FormattedValue.ToString()+"  "+dataGridView1.Rows[i].Cells[2].FormattedValue.ToString()+"  NOEXISTE\r\n");
                 }*/

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            textBox1.Visible = true;
        }
        //conseguir id variante RALE --------------
        public void marcar_en_bd_rale()
        {
            conex.conectar("base_principal");
            String id, inc = "", ti_doc, obser_rale, fecha_not, folio, status;
            //MessageBox.Show("marcar bd chida");
            textBox1.Text = "";
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                ti_doc = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//cred
                inc = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//per
                //obser_rale= dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();//per
                //fecha_not= dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();//status
                //folio= dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                //status=dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                //id=id.Substring(0,10);

                //sql = "UPDATE datos_factura SET status=\"0\", fecha_entrega=null, fecha_recepcion=null,fecha_notificacion=null,fecha_cartera=null, columna_lz=\"Se reasignan creditos para emilio 240817\" "+
                //sql = "UPDATE datos_factura SET folio_not_core=nombre_periodo,status=\"0\", sector_notificacion_actualizado="+fecha_not+", fecha_entrega=null, fecha_recepcion=null,fecha_notificacion=null,fecha_cartera=null, columna_lz=\"Se reasignan creditos y se entregan como ESPECIAL_04\" "+
                //sql="UPDATE datos_factura SET nombre_periodo=\"ESPECIAL_04\" "+
                //"WHERE registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\" and nombre_periodo=\""+obser_rale+"\" ";
                //sql="UPDATE datos_factura SET sector_notificacion_actualizado="+52+", fecha_entrega=null, status=\"0\", folio_not_core=nombre_periodo, columna_lz=\"Se reasignan creditos para notificador Jimenez Lozano Francisco 040917\" WHERE registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\"";
                conex2.conectar("base_principal");
                //folio="OFICIOS_0"+inc.Substring(8,(inc.Length-8));   
                //sql = "UPDATE datos_factura SET nn=\"ESTRADOS\" ,status=\"" + fecha_not + "\", columna_lz=\"se cambia status a creditos de estrados 13/10/2017\" WHERE registro_patronal=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" AND credito_multa=\"" + inc + "\" AND periodo=\"" + obser_rale + "\"";

                sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + id + "\" AND credito_multa=\"" + ti_doc + "\" AND periodo=\"" + inc + "\"";
                consulta = conex2.consultar(sql);
                if (consulta.Rows.Count > 0){
                    textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " " + consulta.Rows[0][0].ToString() + "\r\n");
                }
                else{
                    sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" AND periodo=\"" + inc + "\"";
                	consulta = conex2.consultar(sql);
                	if (consulta.Rows.Count > 0){
                		textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " " + consulta.Rows[0][0].ToString() + "\r\n");
                	}else{
                		textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " NO EXISTE\r\n");
                	}
                }
                
                  
                
                
                //MessageBox.Show(sql);
                /* sql="select registro_patronal2,credito_cuotas,periodo,columna_lz from datos_factura where registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\" and periodo=\""+obser_rale+"\"";
                 dataGridView2.DataSource=conex.consultar(sql);
                 if(dataGridView2.RowCount>0){
                     if(dataGridView2.Rows[j].Cells[3].Value.ToString().Contains("créditos de estrados")==true){
                         textBox1.AppendText(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[1].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[2].FormattedValue.ToString()+" CAPTURADO\r\n");
                     }else{
                         textBox1.AppendText(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[1].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[2].FormattedValue.ToString()+"  FALTANTE\r\n");
                     }
                 }else{
                     textBox1.AppendText(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+"  "+dataGridView1.Rows[i].Cells[1].FormattedValue.ToString()+"  "+dataGridView1.Rows[i].Cells[2].FormattedValue.ToString()+"  NOEXISTE\r\n");
                 }*/

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            textBox1.Visible = true;
        }
         //conseguir id variante RALE version solo cuotas --------------
        public void marcar_en_bd_rale_v2()
        {
            conex.conectar("base_principal");
            String id, inc = "", ti_doc, obser_rale, fecha_not, folio, status;
            //MessageBox.Show("marcar bd chida");
            textBox1.Text = "";
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                ti_doc = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//cred
                inc = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//per
                //obser_rale= dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();//per
                //fecha_not= dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();//status
                //folio= dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                //status=dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                //id=id.Substring(0,10);

                //sql = "UPDATE datos_factura SET status=\"0\", fecha_entrega=null, fecha_recepcion=null,fecha_notificacion=null,fecha_cartera=null, columna_lz=\"Se reasignan creditos para emilio 240817\" "+
                //sql = "UPDATE datos_factura SET folio_not_core=nombre_periodo,status=\"0\", sector_notificacion_actualizado="+fecha_not+", fecha_entrega=null, fecha_recepcion=null,fecha_notificacion=null,fecha_cartera=null, columna_lz=\"Se reasignan creditos y se entregan como ESPECIAL_04\" "+
                //sql="UPDATE datos_factura SET nombre_periodo=\"ESPECIAL_04\" "+
                //"WHERE registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\" and nombre_periodo=\""+obser_rale+"\" ";
                //sql="UPDATE datos_factura SET sector_notificacion_actualizado="+52+", fecha_entrega=null, status=\"0\", folio_not_core=nombre_periodo, columna_lz=\"Se reasignan creditos para notificador Jimenez Lozano Francisco 040917\" WHERE registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\"";
                conex2.conectar("base_principal");
                //folio="OFICIOS_0"+inc.Substring(8,(inc.Length-8));   
                //sql = "UPDATE datos_factura SET nn=\"ESTRADOS\" ,status=\"" + fecha_not + "\", columna_lz=\"se cambia status a creditos de estrados 13/10/2017\" WHERE registro_patronal=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" AND credito_multa=\"" + inc + "\" AND periodo=\"" + obser_rale + "\"";

                sql = "SELECT id FROM datos_factura WHERE registro_patronal2=\"" + id + "\" AND credito_cuotas=\"" + ti_doc + "\" AND periodo=\"" + inc + "\"";
                consulta = conex2.consultar(sql);
                if (consulta.Rows.Count > 0){
                    textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " " + consulta.Rows[0][0].ToString() + "\r\n");
                }
                else{
                   textBox1.AppendText("" + id + " " + ti_doc + " " + inc + " NO EXISTE\r\n");
                }
                
                  
                
                
                //MessageBox.Show(sql);
                /* sql="select registro_patronal2,credito_cuotas,periodo,columna_lz from datos_factura where registro_patronal2=\""+id+"\" and credito_cuotas=\""+ti_doc+"\" and periodo=\""+obser_rale+"\"";
                 dataGridView2.DataSource=conex.consultar(sql);
                 if(dataGridView2.RowCount>0){
                     if(dataGridView2.Rows[j].Cells[3].Value.ToString().Contains("créditos de estrados")==true){
                         textBox1.AppendText(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[1].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[2].FormattedValue.ToString()+" CAPTURADO\r\n");
                     }else{
                         textBox1.AppendText(dataGridView2.Rows[j].Cells[0].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[1].FormattedValue.ToString()+"  "+dataGridView2.Rows[j].Cells[2].FormattedValue.ToString()+"  FALTANTE\r\n");
                     }
                 }else{
                     textBox1.AppendText(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString()+"  "+dataGridView1.Rows[i].Cells[1].FormattedValue.ToString()+"  "+dataGridView1.Rows[i].Cells[2].FormattedValue.ToString()+"  NOEXISTE\r\n");
                 }*/

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            textBox1.Visible = true;
        }
		//chafa
        public void marcar_en_bd(){
        	conex.conectar("base_principal");
            String id, inc = "", ti_doc, obser_rale, fecha_not, folio, status;
            //MessageBox.Show("marcar bd chida");
            textBox1.Text = "";
            i = 0;
            do
            {
            	id=dataGridView1.Rows[i].Cells[0].Value.ToString();//reg
            	//obser_rale=dataGridView1.Rows[i].Cells[2].Value.ToString();//cred
            	//ti_doc=dataGridView1.Rows[i].Cells[3].Value.ToString();//per
            	folio=dataGridView1.Rows[i].Cells[2].Value.ToString();
            	//fecha_not=dataGridView1.Rows[i].Cells[9].Value.ToString();
            	//folio="1803"+folio.Substring(1,5);
            	
            	conex.consultar("UPDATE datos_factura SET status=\"DEPURACION MANUAL\",fecha_depuracion=\"2018-06-27\",folio_sipare_sua=\""+folio+"\",num_pago=1,observaciones=\"-\" WHERE id=\""+id+"\"");
            	
            	label5.Text = "Cuenta: " + i;
                label5.Refresh();
                i++;
                
        	} while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            textBox1.Visible = true;
        
        }
        //resultados fechas
        public void marcar_en_bd_nvo(){
        	conex.conectar("base_principal");
            String reg="",cred="",per="",fech="",res="",fecha="";
            string[] res_cap;
            int mul=0,j=0;
            res_cap= new string[4];
            //MessageBox.Show("marcar bd chida");
            textBox1.Text = "";
            i = 0;
            do
            {
            	reg=dataGridView1.Rows[i].Cells[0].Value.ToString();//reg
            	cred=dataGridView1.Rows[i].Cells[1].Value.ToString();//cred
            	per=dataGridView1.Rows[i].Cells[2].Value.ToString();//per
            	fecha=dataGridView1.Rows[i].Cells[3].Value.ToString();//fecha
            	res=dataGridView1.Rows[i].Cells[4].Value.ToString();//res
            	
            	consultadora=conex.consultar("SELECT capturado_siscob FROM datos_factura WHERE registro_patronal2=\""+reg+"\" AND credito_multa=\""+cred+"\" and periodo=\""+per+"\" and fecha_notificacion=\""+fecha+"\"");
            	mul=0;
            	if(consultadora.Rows.Count==0){
            		consultadora=conex.consultar("SELECT capturado_siscob FROM datos_factura WHERE registro_patronal2=\""+reg+"\" AND credito_cuotas=\""+cred+"\" and periodo=\""+per+"\" and fecha_notificacion=\""+fecha+"\"");
            		mul=1;
            	}
            	
            	if(consultadora.Rows.Count>0){
	            	res_cap=consultadora.Rows[0][0].ToString().Split('/','|');
	            	
	            	if(mul==0){
	            		//MessageBox.Show("UPDATE datos_factura SET capturado_siscob=\""+res_cap[0]+"/"+res+"|"+res_cap[2]+"/"+res_cap[3]+"\" WHERE registro_patronal2=\""+reg+"\" AND credito_multa=\""+cred+"\" and periodo=\""+per+"\"");
	            		conex.consultar("UPDATE datos_factura SET capturado_siscob=\""+res_cap[0]+"/"+res+"|"+res_cap[2]+"/"+res_cap[3]+"\" WHERE registro_patronal2=\""+reg+"\" AND credito_multa=\""+cred+"\" and periodo=\""+per+"\" and fecha_notificacion=\""+fecha+"\"");
	            	}else{
	            		//MessageBox.Show("UPDATE datos_factura SET capturado_siscob=\""+res+"/"+res_cap[1]+"|"+res_cap[2]+"/"+res_cap[3]+"\" WHERE registro_patronal2=\""+reg+"\" AND credito_cuotas=\""+cred+"\" and periodo=\""+per+"\"");
	            	//	conex.consultar("UPDATE datos_factura SET capturado_siscob=\""+res+"/"+res_cap[1]+"|"+res_cap[2]+"/"+res_cap[3]+"\" WHERE registro_patronal2=\""+reg+"\" AND credito_cuotas=\""+cred+"\" and periodo=\""+per+"\" and fecha_notificacion=\""+fecha+"\"");
	            	}
            	}else{
            		j++;
            		textBox1.AppendText(""+res+" "+cred+" "+per);
            	}
            	
            	label5.Text = "Cuenta: " + i;
                label5.Refresh();
                i++;
                
        	} while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i+"\nOmitidos: "+j);
            textBox1.Visible = true;
        }
        
        public void marcar_en_bd_estra()
        {//importacion estrados
            conex.conectar("base_principal");
            String folio, id_credito, fecha_cred, nom_doc, supues_estr, motivo_estra, motivo_estrab, motivo_estrac, motivo_estrad, fecha_acta, fojas, fech_firma, fech_publi, fech_ini, fech_fin, fech_not, noti, subdel, tit_sub;
            //MessageBox.Show("marcar bd chida");
            textBox1.Text = "";
            i = 0;
            do
            {
                folio = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                id_credito = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                fecha_cred = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                nom_doc = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
                supues_estr = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                motivo_estra = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                motivo_estrab = dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                motivo_estrac = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                motivo_estrad = dataGridView1.Rows[i].Cells[8].FormattedValue.ToString();
                fecha_acta = dataGridView1.Rows[i].Cells[9].FormattedValue.ToString();
                fojas = dataGridView1.Rows[i].Cells[10].FormattedValue.ToString();
                fech_firma = dataGridView1.Rows[i].Cells[11].FormattedValue.ToString();
                fech_publi = dataGridView1.Rows[i].Cells[12].FormattedValue.ToString();
                fech_ini = dataGridView1.Rows[i].Cells[13].FormattedValue.ToString();
                fech_fin = dataGridView1.Rows[i].Cells[14].FormattedValue.ToString();
                fech_not = dataGridView1.Rows[i].Cells[15].FormattedValue.ToString();
                noti = dataGridView1.Rows[i].Cells[16].FormattedValue.ToString();
                subdel = dataGridView1.Rows[i].Cells[17].FormattedValue.ToString();
                tit_sub = dataGridView1.Rows[i].Cells[18].FormattedValue.ToString();

                if (motivo_estrab.Length > 1)
                {
                    motivo_estra = motivo_estra + motivo_estrab;
                }

                if (motivo_estrac.Length > 1)
                {
                    motivo_estra = motivo_estra + motivo_estrac;
                }

                if (motivo_estrad.Length > 1)
                {
                    motivo_estra = motivo_estra + motivo_estrad;
                }
                conex.consultar("INSERT INTO estrados(folio,id_credito,fecha_emision_doc,nombre_documento,supuesto_estrados,motivo_estrados,fecha_acta_circunstanciada,fojas,fecha_firma_alta,fecha_publicacion,fecha_inicio_not,fecha_fin_not,fecha_retiro_not,notificador_estrados,titular_sub,sub_emisora,hora_not) " +
                                "VALUES(\"" + folio + "\",\"" + id_credito + "\",\"" + fecha_cred + "\",\"" + nom_doc + "\",\"" + supues_estr + "\",\"" + motivo_estra + "\",\"" + fecha_acta + "\"," + fojas + ",\"" + fech_firma + "\",\"" + fech_publi + "\",\"" + fech_ini + "\",\"" + fech_fin + "\",\"" + fech_not + "\",\"" + noti + "\",\"" + tit_sub + "\",\"" + subdel + "\",\"11:11\")");
                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            textBox1.Visible = true;
        }
        
        public void ingresa_nuevo_inv_20108(){
        	conex.conectar("inventario");
            String id, inc = "", ti_doc, obser_rale, fecha_not, obser,imp;
            //MessageBox.Show("marcar bd chida");
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                ti_doc = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//cred_cuotas
                inc = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//per_mes
                obser_rale = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();//per_anio
                fecha_not= dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();//fech not
                obser=dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();//fech not
                imp=dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();//fech not
                //id=id.Substring(0,10);
                sql = "INSERT INTO base_inventario(clase_doc,reg_pat,mov,fecha_mov,sector,credito,ce,periodo_mes,periodo_anio,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,tipo_inv,no_cuaderno)" +
					  "VALUES (\"COP\",\""+id+"\",0,\"0000-00-00\",0,\""+ti_doc+"\",\"-\",\""+inc+"\",\""+obser_rale+"\", \""+fecha_not+"\",\"0000-00-00\",\"0000-00-00\",\""+imp+"\",\"0000-00-00\",\"0\",\""+obser+"\",\"MEX\",20)";
                //MessageBox.Show(sql);
                conex.consultar(sql);

                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            //textBox1.Visible=true;
        }
		
        public void ingresa_nuevo_inv_20108_1(){
        	conex.conectar("inventario");
            String inc = "", ti_doc, obser_rale, fecha_not, obser,imp;
            var id="";
            
            //MessageBox.Show("marcar bd chida");
            i = 0;
            do
            {
                id = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();//reg_pat
                ti_doc = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();//cred_cuotas
                inc = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();//per_mes
                obser_rale = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();//per_anio
                fecha_not= dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();//fech not
                obser=dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();//fech not
                imp=dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();//fech not
                //id=id.Substring(0,10);
                sql = "UPDATE base_inventario SET observaciones=\""+inc+"\" where  idbase_inventario = "+id+"";
                //MessageBox.Show(sql);
                conex.consultar(sql);
				
                i++;
                label5.Text = "Cuenta: " + i;
                label5.Refresh();
            } while (i < dataGridView1.RowCount);

            MessageBox.Show("Listo " + i);
            //textBox1.Visible=true;
        }
        
        public void carga_desde_txt(){
        	OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.t *.txt)|*.t;*.txt"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Seleccione el archivo de Texto";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			int i=0;
			string x;
            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
            	StreamReader rdr = new StreamReader(@""+dialog.FileName);
            	dataGridView1.Columns.Add("reg_pat","reg_pat");
            	dataGridView1.Columns.Add("per_mes","per_mes");
            	dataGridView1.Columns.Add("per_anio","per_anio");
            	dataGridView1.Columns.Add("cred","cred");
            	dataGridView1.Columns.Add("fecha","fecha");
            	
            	while(!(rdr.EndOfStream)){
            		dataGridView1.Rows.Add();
            		dataGridView1.Rows[i].Cells[0].Value=rdr.ReadLine();
            		dataGridView1.Rows[i].Cells[1].Value=rdr.ReadLine();
            		dataGridView1.Rows[i].Cells[2].Value=rdr.ReadLine();
            		dataGridView1.Rows[i].Cells[3].Value=rdr.ReadLine();
            		dataGridView1.Rows[i].Cells[4].Value=rdr.ReadLine();
            		x=rdr.ReadLine();
            		i++;
            	}
            }
        }
        
        void Carga_excel40Load(object sender, EventArgs e)
        {

        }

        void Button1Click(object sender, EventArgs e)
        {
            carga_excel();
        }

        void Button2Click(object sender, EventArgs e)
        {
            cargar_hoja_excel();
        }

        void Button3Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                reparar_bd();
            }
            else
            {
                if (radioButton5.Checked == true)
                {
                    comparar2();
                }
                else
                {
                    if (radioButton6.Checked == true)
                    {
                    	ingresa_nuevo_inv_20108_1();
                    	//ingresa_nuevo_inv_20108();
                    	//marcar_en_bd_rale();
                    	//marcar_en_bd_b();
                    	//marcar_en_bd_nvo();
                    	//carga_desde_txt();
                        //hilosecundario = new Thread(new ThreadStart(marcar_en_bd));
                        //hilosecundario.Start();
                    }
                    else
                    {
                        insertar40();
                    }
                }
            }

        }

        void Timer1Tick(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                button2.Enabled = true;
            }

            if (comboBox2.SelectedIndex > -1)
            {
                button4.Enabled = true;
            }
        }

        void RadioButton1CheckedChanged(object sender, EventArgs e)
        {

        }

        void Button5Click(object sender, EventArgs e)
        {
            carga_excel2();
        }

        void Button4Click(object sender, EventArgs e)
        {
            cargar_hoja_excel2();
        }

        void RadioButton6CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"temp\CT.exe");
        }
		void Button6Click(object sender, EventArgs e)
		{
			lector_pdf lec = new lector_pdf();
			lec.Show();
		}

        private void button7_Click(object sender, EventArgs e)
        {
            String folio = "";
            conex.conectar("base_principal");
            consulta = conex.consultar("SELECT folio,id_estrados FROM estrados ORDER BY id_estrados DESC ");

            conex2.conectar("base_principal");

            for (int i = 0; i < consulta.Rows.Count-1; i++)
            {
                folio = consulta.Rows[i][0].ToString();
                folio = folio.Substring(0, 9) + "0" + folio.Substring(9,folio.Length-9);
                conex2.consultar("UPDATE estrados SET folio=\""+folio+"\" WHERE id_estrados = "+consulta.Rows[i][1].ToString()+"");
            }

            MessageBox.Show("Listo");

            conex.cerrar();
            conex2.cerrar();
        }

    }
}

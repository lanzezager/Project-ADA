using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;

namespace Nova_Gear.Mod40
{
    public partial class Sindo_Mod40 : Form
    {
        public Sindo_Mod40()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;
        DataTable tablarow = new DataTable();
        DataTable tablarow_consulta = new DataTable();
        DataTable tablasindo = new DataTable();

        //Declaracion del Delegado y del Hilo para ejecutar un subproceso
        private Thread hilosecundario = null;

        int i = 0, z = 0, tot_ing = 0, tot_act = 0, tot_errs = 0;
        String archivo, ext, ruta_sindo;
 
        public void carga_excel()
        {
            String cad_con;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Seleccione el archivo de Excel";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archivo = dialog.FileName;
                label2.Text = dialog.SafeFileName;
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

            int filas = 0;
            String tabla;
            i = 0;
            comboBox1.Items.Clear();
            System.Data.DataTable dt = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            dataGridView2.DataSource = dt;
            filas = (dataGridView2.RowCount);
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
            } while (i < filas);

            dt.Clear();
            dataGridView2.DataSource = dt; //vaciar datagrid
        }

        public void cargar_hoja_excel()
        {
            String hoja, cons_exc;
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
                        dataGridView3.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        conexion.Close();//cerramos la conexion
                        dataGridView3.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
                        label5.Text = "Registros: " + dataGridView3.RowCount;
                        label5.Refresh();

                        //estilo datagrid
                        i = 0;
                        do
                        {
                            dataGridView3.Columns[i].HeaderText = dataGridView3.Columns[i].HeaderText.ToUpper();
                            i++;
                        } while (i < dataGridView3.ColumnCount);
                        i = 0;

                        if (dataGridView3.RowCount > 0)
                        {
                            button4.Enabled = true;
                            //button7.Enabled=true;							
                        }
                        else
                        {
                            button4.Enabled = false;
                            //button7.Enabled=false;	
                        }
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }

            }

        }

        public void leer_excel()
        {
            String rp="", ruta="", nss="";
            int rp_num = 0, j = 0;
            long nss_num = 0;
            FileStream fichero, fichero1;
            
            try
            {
                i = 0;
                tablarow.Columns.Clear();
                tablarow_consulta.Columns.Clear();
                if (tablarow.Columns.Contains("REGISTRO PATRONAL") == false)
                {
                    tablarow.Columns.Add("NSS");
                    tablarow.Columns.Add("REGISTRO PATRONAL");
                    tablarow_consulta.Columns.Add("NSS");
                    tablarow_consulta.Columns.Add("REGISTRO PATRONAL");
                }
                //Borrar archivos para comenzar de 0
                System.IO.File.Delete(@"mod40/sindo/lista_nrp.txt");
                System.IO.File.Delete(@"mod40/sindo/config_sindo.txt");

                if (File.Exists(@"mod40/sindo/consulta_sindo.txt") == true)
                {
                    System.IO.File.Delete(@"mod40/sindo/consulta_sindo.txt");
                }

                //Crear archivos nuevos
                fichero = System.IO.File.Create(@"mod40/sindo/lista_nrp.txt");
                fichero1 = System.IO.File.Create(@"mod40/sindo/config_sindo.txt");

                ruta = fichero.Name;

                fichero.Close();
                fichero1.Close();

                //Abrir archivo
                StreamWriter wr = new StreamWriter(@"mod40/sindo/lista_nrp.txt");

                while(i < dataGridView3.RowCount){
                    nss = dataGridView3.Rows[i].Cells[0].Value.ToString();
                    rp = dataGridView3.Rows[i].Cells[1].Value.ToString();
                   
                    if ((rp.Length == 10)&&(nss.Length>=10))
                    {
                       //MessageBox.Show(nss + "," + rp+" 1");
                        //MessageBox.Show(""+long.TryParse(nss, out nss_num));
                        if (long.TryParse(nss, out nss_num) == true)
                        {
                            //MessageBox.Show(nss + "," + rp + " 2");
                            if (int.TryParse(rp.Substring(1, 9), out rp_num) == true)
                            {
                                //MessageBox.Show(nss + "," + rp + " 3");
                                nss = nss.Substring(0, 10);
                                wr.WriteLine(nss+","+rp);
                                tablarow_consulta.Rows.Add(nss,rp);
                                j++;
                            }
                            else
                            {
                                tablarow.Rows.Add(nss,rp);
                            }
                        }
                        else
                        {
                            tablarow.Rows.Add(nss, rp);
                        }
                    }
                    else
                    {
                        tablarow.Rows.Add(nss, rp);
                    }
                    i++;
                }
                wr.WriteLine("%&");
                wr.Close();

                StreamWriter wr1 = new StreamWriter(@"mod40/sindo/config_sindo.txt");
                if (textBox1.Text.Equals("consulta_sindo.txt"))
                {
                    ruta_sindo = ruta.Substring(0, (ruta.Length - 14)) + "\\" + textBox1.Text;
                    wr1.WriteLine(ruta_sindo);
                }
                else
                {
                    ruta_sindo = textBox1.Text;
                    wr1.WriteLine(textBox1.Text);
                }
                wr1.WriteLine(j);
                wr1.Close();

                dataGridView3.DataSource = tablarow;

                DialogResult respuesta = MessageBox.Show("Se Consultarán: " + j + " Asegurados y se Omitirán: " + tablarow.Rows.Count + " Registros por no estar bien escritos o contener algún error.\n " +
                                            "Está a punto de comenzar el proceso de consulta automática.\n" +
                                            "Una vez comenzada la consulta automática NO se deberá manipular el" +
                                            "equipo hasta que haya finalizado el proceso de consulta.\n" +
                                            "El programa le informará cuando el proceso haya concluido\n\n" +

                                "¿Desea comenzar el proceso de consulta?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Yes)
                {

                    MessageBox.Show("El proceso iniciará cuando de click en Aceptar", "Información");
                    StreamWriter wr2 = new StreamWriter(@"mod40_busca_sindo.bat");
                    wr2.WriteLine("@echo off");
                    wr2.WriteLine(ruta.Substring(0,1)+":");
                    wr2.WriteLine("cd \"" + ruta.Substring(0, (ruta.Length - 14)) + "\"");
                    wr2.WriteLine("start sindo_prime.exe");
                    wr2.Close();
                    System.Diagnostics.Process.Start(@"mod40_busca_sindo.bat");
                }

            }
            catch { 
            }
        }

        public void lectura_archivo()
        {
            Invoke(new MethodInvoker(delegate {
                int tot = 0, j = 0, k = 0;
                string[] aux;
                String linea, linea_pasa, fecha_consulta_sindo="", nss="", nom_ase="", curp="", del="", reg_pat="";
                String fech_recep="", inicio_o="", inicio_t="", inicio_fecha="", salario="", salario_tipo="", sem_jor="", ex="", sup_srv="", final_o="",final_t="", final_fecha="",ti="",ts="";

                StreamReader sr = new StreamReader(ruta_sindo);
                tot = 0;
                while((sr.EndOfStream)==false){
                    linea = sr.ReadLine();
                    if (linea.Contains("S.IN.D.O.")){
                        tot++;
                    }
                }
                //textBox2.Text = sr.ReadToEnd();
                //aux = textBox2.Lines;
                //tot = aux.Length;
                sr.Close();
                progressBar1.Value = 0;
               // MessageBox.Show("lineas:" + tot);
                tablasindo.Rows.Clear();

                StreamReader sr1 = new StreamReader(ruta_sindo);
                //MessageBox.Show("lineas:"+tot);
                label8.Text = "Leyendo Archivo";
                label8.Refresh();
                tot_errs = 0;

                while ((sr1.EndOfStream) == false)
                {
                    linea=sr1.ReadLine();
                    //MessageBox.Show(linea);
                    try{
                        if (linea.Contains("S.IN.D.O."))//inicio bloque
                        {
                            //resetear variables
                            fecha_consulta_sindo = "-";
                            nss = "-";
                            nom_ase = "0";
                            curp = "-";
                            del = "0";
                            reg_pat = "0";
                            fech_recep = "0";
                            inicio_o = "0";
                            inicio_t = "0";
                            inicio_fecha = "0";
                            salario = "0";
                            salario_tipo = "0";
                            sem_jor = "0";
                            ex = "0";
                            sup_srv = "0";
                            final_o = "0";
                            final_t = "0";
                            final_fecha = "0";
                            ti = "0";
                            ts = "0";

                            aux = linea.Split(new char[]{' ','\t'},StringSplitOptions.RemoveEmptyEntries);
                            fecha_consulta_sindo = aux[aux.Length - 1];
                            //MessageBox.Show(fecha_consulta_sindo);

                            linea = sr1.ReadLine();
                            if (linea.Contains("NUM. DE SEG. SOC.  "))
                            {
                                aux = linea.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                nss=aux[4]+aux[6];
                                //MessageBox.Show(nss);
                            }
                           
                            linea = sr1.ReadLine();
                            if (linea.Contains("ASEG."))
                            {
                                aux = linea.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                if (aux.Length == 4)
                                {
                                    nom_ase = aux[1] + "-" + aux[2];
                                }
                                else
                                {
                                    nom_ase = aux[1];
                                }
                                //MessageBox.Show(nom_ase);
                            }
                            
                            linea = sr1.ReadLine();
                            if (linea.Contains("CURP"))
                            {
                                aux = linea.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                curp = aux[1];
                                //MessageBox.Show(curp);
                            }

                            linea_pasa = sr1.ReadLine();
                            linea_pasa = sr1.ReadLine();

                            linea = sr1.ReadLine();
                            
                            aux = linea.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            //MessageBox.Show("aux leght:" + aux.Length);
                            del = aux[0];
                            reg_pat = aux[1];
                            fech_recep = aux[2];
                            inicio_o = aux[3];
                            inicio_t = aux[4];
                            inicio_fecha = aux[5];
                            salario = aux[6];
                            salario_tipo = aux[7];
                            sem_jor = aux[8];
                            ex = aux[9];
                            sup_srv = aux[10];

                            if (aux.Length == 15)
                            {
                                final_o = aux[11];
                                final_t = aux[12];

                                if (aux[13] == "00/00/0000")
                                {
                                    final_fecha = "01/01/1900";
                                }
                                else
                                {
                                    final_fecha = aux[13]; 
                                }

                                ti = aux[14];
                            }
                            else
                            {
                                if (aux[11] == "00/00/0000")
                                {
                                    final_fecha = "01/01/1900";
                                }
                                else
                                {
                                    final_fecha = aux[11];
                                }

                                ti = aux[12];

                                if (aux.Length == 16)
                                {
                                    ts = aux[15];
                                }
                            }

                            //MessageBox.Show("tablasindo count:" + tablasindo.Rows.Count);
                            tablasindo.Rows.Add(fecha_consulta_sindo,nss,nom_ase,curp,del,reg_pat,fech_recep,inicio_o,inicio_t,inicio_fecha,salario,salario_tipo,sem_jor,ex,sup_srv,final_o,final_t,final_fecha,ti,ts);
                            j++;
                            progreso(tot, j);
                        }
                        
                    }catch { }                   
                }

                sr1.Close();
                dataGridView1.DataSource = tablasindo;
                //dataGridView1.Columns[12].Visible = false;
                dataGridView1.Visible = true;
                label5.Text = "Registros: " + dataGridView1.RowCount;
                label5.Refresh();

                progressBar1.Value = 100;
                label7.Text = "100%";
                label8.Text = "Proceso Completado Exitosamente";
                MessageBox.Show("El proceso terminó correctamente.\n", "¡EXITO!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                panel3.Visible = false;
                button4.Enabled = true;
                button8.Enabled = true;
                button9.Visible = true;
                button10.Visible = true;
            }));
        }

        public void ingreso_bd()
        {
            Invoke(new MethodInvoker(delegate
            {
                int tot = 0, j = 0,est_sin=0;
                String sql = "";

                conex.conectar("base_principal");
                tot = tablasindo.Rows.Count;
                progressBar1.Value = 0;
                while (j < tot)
                {
                    sql = "INSERT INTO mod40_sindo (fecha_consulta,nss,nombre_asegurado,curp,delegacion,registro_patronal,fecha_recep,inicio_o,inicio_t,inicio_fecha,salario,salario_tipo,sem_jor,ex,sup_srv,final_o,final_t,final_fecha,ti,ts) " +
                        "VALUES (\"" + formato_fecha(tablasindo.Rows[j][0].ToString()) + "\"," +
                        "\"" + tablasindo.Rows[j][1].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][2].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][3].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][4].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][5].ToString() + "\"," +
                        "\"" + formato_fecha(tablasindo.Rows[j][6].ToString()) + "\"," +
                        "\"" + tablasindo.Rows[j][7].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][8].ToString() + "\"," +
                        "\"" + formato_fecha(tablasindo.Rows[j][9].ToString()) + "\"," +
                        "" + tablasindo.Rows[j][10].ToString() + "," +
                        "\"" + tablasindo.Rows[j][11].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][12].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][13].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][14].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][15].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][16].ToString() + "\"," +//estado_sindo
                        "\"" + formato_fecha(tablasindo.Rows[j][17].ToString()) + "\"," +
                        "\"" + tablasindo.Rows[j][18].ToString() + "\"," +
                        "\"" + tablasindo.Rows[j][19].ToString() + "\")";
                    conex.consultar(sql);

                    if(tablasindo.Rows[j][16].ToString()!="2"){
                        est_sin=1;
                    }else{
                        est_sin=2;
                    }

                    sql = "UPDATE mod40_sua SET estado_sindo="+est_sin+" WHERE nss=\""+tablasindo.Rows[j][1].ToString()+"\"";
                    conex.consultar(sql);
                    j++;
                    progreso2(tot, j);                    
                }

                progressBar1.Value = 100;
                label7.Text = "100%";
                label8.Text = "Proceso Completado Exitosamente";
                MessageBox.Show("El proceso terminó correctamente.\n", "¡EXITO!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                panel3.Visible = false;
                button4.Enabled = false;
                button8.Enabled = true;
                button9.Visible = true;
                button10.Visible = false;
            }));
        }

        public void progreso(int total, int progreso)
        {
            int porcent_act = 0, prog_tot = 0, x = 0;
            prog_tot = total;
            x = progreso;

            porcent_act = Convert.ToInt32(((x * 100) / prog_tot));
            label7.Text = (porcent_act) + "%";
            label7.Refresh();
            if (porcent_act >= 99)
            {
                porcent_act = 100;
            }
            progressBar1.Value = porcent_act;
            progressBar1.Refresh();
            z++;

            if (z == 5)
            {
                label8.Text = "Leyendo Archivo.";
                label8.Refresh();
            }

            if (z == 10)
            {
                label8.Text = "Leyendo Archivo..";
                label8.Refresh();
            }

            if (z == 15)
            {
                label8.Text = "Leyendo Archivo...";
                label8.Refresh();
            }

            if (z == 20)
            {
                label8.Text = "Leyendo Archivo";
                label8.Refresh();
                z = 0;
            }
        }

        public void progreso2(int total, int progreso)
        {
            int porcent_act = 0, prog_tot = 0, x = 0;
            prog_tot = total;
            x = progreso;

            porcent_act = Convert.ToInt32((x * 100) / prog_tot);
            label7.Text = (porcent_act) + "%";
            label7.Refresh();
            if (porcent_act >= 99)
            {
                porcent_act = 100;
            }
            progressBar1.Value = porcent_act;
            progressBar1.Refresh();

            z++;

            if (z == 5)
            {
                label8.Text = "Ingresando Información a la Base de Datos.";
                label8.Refresh();
            }

            if (z == 10)
            {
                label8.Text = "Ingresando Información a la Base de Datos..";
                label8.Refresh();
            }

            if (z == 15)
            {
                label8.Text = "Ingresando Información a la Base de Datos...";
                label8.Refresh();
            }

            if (z == 20)
            {
                label8.Text = "Ingresando Información a la Base de Datos";
                label8.Refresh();
                z = 0;
            }
        }		

        public String formato_fecha(String fecha){
            String fecha_correcta = "";
            fecha_correcta= fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
            return fecha_correcta;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                cargar_hoja_excel();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            carga_excel();
        }

        private void Sindo_Mod40_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
            
            panel2.Show();

            tablasindo.Columns.Add("FECHA CONSULTA");
            tablasindo.Columns.Add("NSS");
            tablasindo.Columns.Add("NOMBRE ASEGURADO");
            tablasindo.Columns.Add("CURP");
            tablasindo.Columns.Add("DELEGACION");
            tablasindo.Columns.Add("REG. PAT");
            tablasindo.Columns.Add("F. RECEP");
            tablasindo.Columns.Add("INICIO O");
            tablasindo.Columns.Add("INICIO T");
            tablasindo.Columns.Add("INICIO FECHA");
            tablasindo.Columns.Add("SALARIO");
            tablasindo.Columns.Add("SALARIO_TIPO");
            tablasindo.Columns.Add("SEM JOR");
            tablasindo.Columns.Add("EX");
            tablasindo.Columns.Add("SUP_SRV");
            tablasindo.Columns.Add("FINAL O");
            tablasindo.Columns.Add("FINAL T");
            tablasindo.Columns.Add("FINAL FECHA");
            tablasindo.Columns.Add("T I");
            tablasindo.Columns.Add("T S");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Archivos de texto (*.txt)|*.txt"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Guardar Archivo de Texto";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            leer_excel();
        }

        private void terminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button8.Enabled = false;
            label8.Text = "Comenzando Lectura de Archivo";
            label8.Refresh();
            panel3.Visible = true;
            dataGridView1.Visible = true;

            hilosecundario = new Thread(new ThreadStart(lectura_archivo));
            hilosecundario.Start();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if ((progressBar1.Value == 100))
            {
                if (dataGridView1.Visible == true)
                {
                    dataGridView1.Visible = false;
                    label5.Text = "Registros: " + dataGridView3.RowCount;
                    label5.Refresh();
                    button9.Text = "Ocultar\n Omitidos";

                }
                else
                {
                    dataGridView1.Visible = true;
                    label5.Text = "Registros: " + dataGridView1.RowCount;
                    label5.Refresh();
                    button9.Text = " Ver\n Omitidos";
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String ruta;
            FileStream fichero;

            if (textBox1.Text.Equals("consulta_sindo.txt"))
            {
                fichero = System.IO.File.Create(@"mod40/sindo/vacio.txt");
                ruta = fichero.Name;
                fichero.Close();

                ruta_sindo = ruta.Substring(0, (ruta.Length - 10)) + "\\" + textBox1.Text;
            }
            else
            {
                ruta_sindo = textBox1.Text;
            }

            DialogResult res = MessageBox.Show("Se Llevará a cabo la lectura de archivo:\n•" + ruta_sindo + "\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (res == DialogResult.Yes)
            {
                try
                {
                    button4.Enabled = false;
                    button8.Enabled = false;
                    label8.Text = "Comenzando Lectura de Archivo";
                    label8.Refresh();
                    panel3.Visible = true;
                    hilosecundario = new Thread(new ThreadStart(lectura_archivo));
                    hilosecundario.Start();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Ocurrió el siguiente error al leer el archivo:\n" + er, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Esto ingresará informacion a la base de datos\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (res == DialogResult.Yes)
            {
                try
                {
                    button4.Enabled = false;
                    button8.Enabled = false;
                    label8.Text = "Ingresando Información a la Base de Datos";
                    label8.Refresh();
                    panel3.Visible = true;
                    hilosecundario = new Thread(new ThreadStart(ingreso_bd));
                    hilosecundario.Start();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Ocurrió el siguiente error al leer el archivo:\n" + er, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

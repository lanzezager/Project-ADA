/*Creado el 10/08/2020 Por LZ*/
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

namespace Nova_Gear.Estrados
{
    public partial class Procesar_PDFs : Form
    {
        public Procesar_PDFs()
        {
            InitializeComponent();
        }

        private Thread hilo2 = null;
        DataTable indice = new DataTable();
        String PDF_nom_arch = "",num_del="",nom_sub="",num_sub="";
        string[] datos_sub;

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();//guardar_datos--

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;
        

        private void button6_Click(object sender, EventArgs e)
        {
            indice.Rows.Clear();
            dataGridView1.DataSource = null;
            hilo2 = new Thread(new ThreadStart(indexar_PDF));
            hilo2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de PDF (*.pdf)|*.pdf"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Seleccione el archivo de PDF";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
                PDF_nom_arch = dialog.SafeFileName;
                button6.Enabled = true;
            }
        }

        private void Procesar_PDFs_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
            
            progressBar1.Value = 0;
            label3.Text = "0/0";
            label4.Text = "0%";
            indice.Columns.Add("#");
            indice.Columns.Add("Registro Patronal");
            indice.Columns.Add("Credito Cuota");
            indice.Columns.Add("Credito Multa");
            indice.Columns.Add("Pagina Inicio");
            indice.Columns.Add("Pagina Final");
            dataGridView1.Columns[0].ReadOnly = false;
            datos_sub = conex.leer_config_sub();
            num_del = datos_sub[1];
            nom_sub = datos_sub[3];
            num_sub = datos_sub[4];
            //MessageBox.Show(nom_sub +","+num_sub);
        }        

        public void indexar_PDF()
        {
            String archivo = "", reg_pat = "", reg_pat_ant = "000000000", cred_cuo = "", cred_mult = "", mini_extracto = "", mini_extracto2 = "";
            int i = 0, porcentaje=0;
            Invoke(new MethodInvoker(delegate
            {
                archivo=textBox1.Text;
            }));

            PdfReader reader = new PdfReader((string)archivo);
            int num_pages = reader.NumberOfPages;

            //MessageBox.Show(""+num_pages);

            for (int page = 1; page <= num_pages; page++)
            {
                Invoke(new MethodInvoker(delegate
                {
                    label3.Text = page + "/" + num_pages;
                    label3.Refresh();
                }));

                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

                String s = PdfTextExtractor.GetTextFromPage(reader,page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));

                if(s.Length>750){
                    Invoke(new MethodInvoker(delegate
                    {
                        textBox2.AppendText(s + "\r\n-------------------------LANZE ZAGER-------------------------\r\n");
                    }));
                    
                   // MessageBox.Show("filtro 0");
                    if (s.Substring(0, 500).Contains("CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ") == false)
                    {
                       // MessageBox.Show("filtro 0+1 COP");
                        if ((s.Substring(0, 500).Contains("Página: \n 1 de") == true) || (s.Substring(0, 500).Contains("Página: \n1 de") == true))
                        {
                          //  MessageBox.Show("filtro 1");
                            //if (s.Substring(s.Length - 500, 500).Contains("14 38") == true)
                            if (s.Substring(s.Length - 500, 500).Contains(num_del+" "+num_sub) == true)
                            {
                              //  MessageBox.Show("filtro 2");
                                //mini_extracto = s.Substring(s.IndexOf("14 38"), 100);
                                mini_extracto = s.Substring(s.IndexOf(num_del + " " + num_sub), 100);
                                //if (mini_extracto.Contains("Hidalgo") == true)
                                if (mini_extracto.Contains(nom_sub) == true)
                                {
                                    //try
                                    //{
                                    //mini_extracto2 = mini_extracto.Substring(mini_extracto.IndexOf("Hidalgo"), (mini_extracto.IndexOf("/") - mini_extracto.IndexOf("Hidalgo")) + 1);
                                    mini_extracto2 = mini_extracto.Substring(mini_extracto.IndexOf(nom_sub), (mini_extracto.IndexOf("/") - mini_extracto.IndexOf(nom_sub)) + 1);

                                    //reg_pat = mini_extracto.Substring(mini_extracto.IndexOf("14 38") + 6, 10);
                                    reg_pat = mini_extracto.Substring(mini_extracto.IndexOf(num_del + " " + num_sub) + 6, 10);
                                    cred_cuo = mini_extracto2.Substring(mini_extracto2.IndexOf("/") - 23, 9);
                                    cred_mult = mini_extracto2.Substring(mini_extracto2.IndexOf("/") - 13, 9);

                                    if (!(reg_pat.Equals(reg_pat_ant)))
                                    {
                                        if (indice.Rows.Count >= 1)
                                        {
                                            indice.Rows[i - 1][5] = page - 1;
                                        }

                                        i++;
                                        indice.Rows.Add(i, reg_pat, cred_cuo, cred_mult, page, page);
                                    }

                                    reg_pat_ant = reg_pat;
                                    //}catch(Exception exs){
                                    //}
                                }
                            }
                        }//fin if s.Length>500
                    }
                    else
                    {
                        //MessageBox.Show("filtro 0+1 RCV\n " + s.Substring(0, 750));
                        if ((s.Substring(0, 750).Contains("Bimestral// 1") == true) || (s.Substring(0, 750).Contains("Página: \n1 de") == true))
                        {
                            //MessageBox.Show("filtro 1 rcv");
                            //if (s.Substring(s.Length - 750, 750).Contains("14 38") == true)
                            if (s.Substring(s.Length - 750, 750).Contains(num_del + " " + num_sub) == true)
                            {
                               // MessageBox.Show("filtro 2 rcv " + s.Substring(0, 750));
                                if(s.Substring(0,750).Contains("CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACIÓN Y PAGO DE CUOTAS \nCORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ")==false){
                                   // MessageBox.Show("filtro 2.5 cop \n" + mini_extracto);
                                    //mini_extracto = s.Substring(s.IndexOf("14 38"), 100);
                                    mini_extracto = s.Substring(s.IndexOf(num_del + " " + num_sub), 100);
                                    //if (mini_extracto.Contains("Hidalgo") == true)
                                    if (mini_extracto.Contains(nom_sub) == true)
                                    {
                                        //MessageBox.Show("filtro 3 rcv");
                                        //try
                                        //{
                                        //mini_extracto2 = mini_extracto.Substring(mini_extracto.IndexOf("Hidalgo"), (mini_extracto.IndexOf("/") - mini_extracto.IndexOf("Hidalgo")) + 1);
                                        mini_extracto2 = mini_extracto.Substring(mini_extracto.IndexOf(nom_sub), (mini_extracto.IndexOf("/") - mini_extracto.IndexOf(nom_sub)) + 1);
                                        
                                        //reg_pat = mini_extracto.Substring(mini_extracto.IndexOf("14 38") + 6, 10);
                                        reg_pat = mini_extracto.Substring(mini_extracto.IndexOf(num_del + " " + num_sub) + 6, 10);
                                        cred_cuo = mini_extracto2.Substring(mini_extracto2.IndexOf("/") - 23, 9);
                                        cred_mult = mini_extracto2.Substring(mini_extracto2.IndexOf("/") - 13, 9);

                                        if (!(reg_pat.Equals(reg_pat_ant)))
                                        {
                                            if (indice.Rows.Count >= 1)
                                            {
                                                indice.Rows[i - 1][5] = page - 1;
                                            }

                                            i++;
                                            indice.Rows.Add(i, reg_pat, cred_cuo, cred_mult, page, page);
                                        }

                                        reg_pat_ant = reg_pat;
                                        //}catch(Exception exs){
                                        //}
                                    }
                                }else{
                                    
                                    //mini_extracto = s.Substring(s.IndexOf("14 38")-350, 350);
                                    mini_extracto = s.Substring(s.IndexOf(num_del + " " + num_sub)-350, 350);
                                    //MessageBox.Show("filtro 2.5 rcv \n" + mini_extracto);
                                    //if (mini_extracto.Contains("Estatal Jalisco\nHidalgo") == true)
                                    if (mini_extracto.Contains("Estatal Jalisco\n"+nom_sub) == true)
                                    {
                                        //MessageBox.Show("filtro 3 rcv");
                                        //try
                                        //{
                                        //mini_extracto2 = mini_extracto.Substring(mini_extracto.IndexOf("Hidalgo"), (mini_extracto.IndexOf("/") - mini_extracto.IndexOf("Hidalgo")) + 1);
                                        mini_extracto2 = mini_extracto.Substring(mini_extracto.IndexOf(nom_sub), (mini_extracto.IndexOf("/") - mini_extracto.IndexOf(nom_sub)) + 1);
                                        
                                        //reg_pat = mini_extracto.Substring(mini_extracto.IndexOf("Hidalgo") + 8, 10);
                                        reg_pat = mini_extracto.Substring(mini_extracto.IndexOf(nom_sub) + (nom_sub.Length+1), 10);
                                        cred_cuo = mini_extracto2.Substring(mini_extracto2.IndexOf("/") - 23, 9);
                                        cred_mult = mini_extracto2.Substring(mini_extracto2.IndexOf("/") - 13, 9);

                                        if (!(reg_pat.Equals(reg_pat_ant)))
                                        {
                                            if (indice.Rows.Count >= 1)
                                            {
                                                indice.Rows[i - 1][5] = page - 1;
                                            }

                                            i++;
                                            indice.Rows.Add(i, reg_pat, cred_cuo, cred_mult, page, page);
                                        }

                                        reg_pat_ant = reg_pat;
                                        //}catch(Exception exs){
                                        //}
                                    }
                                }
                            }
                        }//fin if s.Length>500
                    }
                }

                Invoke(new MethodInvoker(delegate
                {
                    porcentaje = Convert.ToInt32((page * 100) / num_pages);
                    label4.Text = porcentaje.ToString() + "%";
                    label1.Text = "Registros: " + indice.Rows.Count;
                    if (porcentaje < 100)
                    {
                        progressBar1.Value = porcentaje;
                    }
                    //textBox2.AppendText(reg_pat +"-"+cred_cuo+"-"+cred_mult+"\r\n-------------------------LANZE ZAGER-------------------------\r\n");
                }));
                
                Invoke(new MethodInvoker(delegate
                                {
                //textBox2.AppendText(s + "\r\n-------------------------LANZE ZAGER-------------------------\r\n");
                                }));
                 
                s = "";
            }

            Invoke(new MethodInvoker(delegate{
                if (indice.Rows.Count >= 1)
                {
                    indice.Rows[indice.Rows.Count-1][5] = num_pages;
                }

                dataGridView1.DataSource = indice;
                progressBar1.Value = 100;
                label4.Text = "100%";
                label1.Text = "Registros: "+indice.Rows.Count;

                if(indice.Rows.Count >0){
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
            }));
            MessageBox.Show("Indice Creado Correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            reader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (indice.Rows.Count > 0)
            {
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Indice";//le damos un titulo a la ventana

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(indice, PDF_nom_arch);
                    wb.SaveAs(@"" + dialog_save.FileName + "");

                    MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Indice Vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void extraer_Hojas_PDF(string sourcePdfPath, string outputPath, int[] extractThesePages)
        {
            PdfReader reader = new PdfReader(sourcePdfPath);
            Document sourceDocument = new Document(reader.GetPageSize(1));
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;
            try
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file:                

                sourceDocument = new Document(reader.GetPageSizeWithRotation(extractThesePages[0]));

                // Initialize an instance of the PdfCopyClass with the source
                // document and an output file stream:
                pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPath, System.IO.FileMode.Create));

                sourceDocument.Open();

                //Stopwatch sw = Stopwatch.StartNew();
                //DateTime dateTimeStart = DateTime.Now;
                // Walk the array and add the page copies to the output file:
                foreach (int pageNumber in extractThesePages)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, pageNumber);
                    pdfCopyProvider.AddPage(importedPage);
                }

                //string text = PdfTextExtractor.GetTextFromPage(reader, 1, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy());
                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] crear_arreglo_paginas(int pag_ini, int pag_fin)
        {
            
            int i = 0,j=0;
            List<int> termsList = new List<int>();
            
            j=((pag_fin+1)-pag_ini)/2;
            i=pag_ini+j;

            while(pag_ini<i){
                termsList.Add(pag_ini);
                pag_ini++;
            }

            int[] lista_paginas = termsList.ToArray();

            return lista_paginas;
        }

        public void estilo_grid()
        {
            int cont = 0;

            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;

            dataGridView1.Columns[1].HeaderText = "#";
            dataGridView1.Columns[2].HeaderText = "REGISTRO PATRONAL";
            dataGridView1.Columns[3].HeaderText = "CRÉDITO CUOTA";
            dataGridView1.Columns[4].HeaderText = "CRÉDITO MULTA";
            dataGridView1.Columns[5].HeaderText = "PÁG. INICIO";
            dataGridView1.Columns[6].HeaderText = "PÁG. FIN";

            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[0].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[6].Width = 100;

            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;

            cont = 0;
            while (cont < dataGridView1.RowCount)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
            }
        }

        public void generar_extractos(String carpeta_sel,int indice)
        {
            int[] lista_paginas;

            carpeta_sel = carpeta_sel + "\\" + (dataGridView1.Rows[indice].Cells[2].Value.ToString() + "_" + dataGridView1.Rows[indice].Cells[3].Value.ToString() + "_" + dataGridView1.Rows[indice].Cells[4].Value.ToString()) + ".pdf";
            lista_paginas = crear_arreglo_paginas((Convert.ToInt32(dataGridView1.Rows[indice].Cells[5].Value.ToString())), (Convert.ToInt32(dataGridView1.Rows[indice].Cells[6].Value.ToString())));
            extraer_Hojas_PDF(textBox1.Text, carpeta_sel, lista_paginas);
        }
        //exportar pdf
        private void button3_Click(object sender, EventArgs e)
        {
            List<int> lista_extractos = new List<int>();
            int index=0;

            if ((dataGridView1.RowCount > 0) && (textBox1.Text.Length > 3))
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value.ToString()) == true)
                    {
                        lista_extractos.Add(i);
                        index++;
                    }
                }

                int[] lista = lista_extractos.ToArray();

                DialogResult resul0 = MessageBox.Show("Se van Extraer " + lista.Length + " Créditos del Archivo PDF.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (resul0 == DialogResult.Yes)
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los créditos Extraídos:";
                    DialogResult result = fbd.ShowDialog();
                    String carpeta_sel = "";

                    if (result == DialogResult.OK)
                    {
                        dataGridView1.Enabled = false;
                        carpeta_sel = fbd.SelectedPath.ToString();

                        for (int i = 0; i < lista.Length; i++)
                        {
                            generar_extractos(carpeta_sel,lista[i]);
                        }

                        MessageBox.Show("Crédito(s) Extraído(s) correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show("Operacion Cancelada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Operacion Cancelada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Imposible Extraer Crédito, la Tabla se encuentra vacía o el Archivo PDF no se cargó", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dataGridView1.Enabled = true;
        }       

        public void carga_chema_excel()
        {
            int filas = 0;
            String tabla;
            int i = 0;
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
            //hoja = comboBox1.SelectedItem.ToString();
            hoja = comboBox1.Items[0].ToString();

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
                        button3.Enabled = false;
                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");
                    }
                    else
                    {
                        dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                        dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        conexion.Close();//cerramos la conexion
                        label1.Text = "Registros: " + +dataGridView1.RowCount;
                        label1.Refresh();
                        button3.Enabled = true;
                        
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos excel
            dialog.Title = "Seleccione el archivo de Índice Excel";//le damos un titulo a la ventana
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
            String archivo_index = "",cad_con="";

            //si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archivo_index = dialog.FileName;
                button6.Enabled = true;
                //esta cadena es para archivos excel 2007 y 2010
                cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo_index + "';Extended Properties=Excel 12.0;";
                conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                conexion.Open(); //abrimos la conexion

                carga_chema_excel();
                cargar_hoja_excel();
                estilo_grid();
            }
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
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                        cont++;
                    } while (cont <= 6);

                }
                else
                {
                    cont = 1;
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;
                        cont++;
                    } while (cont <= 6);

                }
            }
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
                        if (dataGridView1.Rows[i].Cells[3].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[3].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            //.Rows[i].Cells[0].Value = true;
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                            foun = 1;
                        }

                        if (dataGridView1.Rows[i].Cells[4].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[4].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                            //.Rows[i].Cells[0].Value = true;
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

        private void maskedTextBox1_Click(object sender, EventArgs e)
        {
            maskedTextBox1.SelectionStart = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount>0){
                DialogResult resul = MessageBox.Show("Se guardará el número de paginas en la Base de Datos.\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            
                if (resul == DialogResult.Yes)
                {
                    conex.conectar("base_principal");
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        conex.consultar("UPDATE datos_factura SET pags_pdf=\"" + dataGridView1[5, i].Value.ToString() + "-" + dataGridView1[6, i].Value.ToString() + "\" WHERE registro_patronal2=\"" + dataGridView1[2, i].Value.ToString() + "\" and credito_cuotas=\"" + dataGridView1[3, i].Value.ToString() + "\" and credito_multa=\"" + dataGridView1[4, i].Value.ToString() + "\"");
                    }

                    conex.guardar_evento("Se añadieron los número de pagina de archivo PDF de "+dataGridView1.RowCount+" creditos");
                    conex.cerrar();
                    MessageBox.Show("La información fue guardada correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

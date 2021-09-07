using System;
using System.Drawing;
using System.Windows.Forms;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
using System.Text;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Linq;

namespace Nova_Gear
{
    public partial class Correspondencia_V2 : Form
    {
        public Correspondencia_V2()
        {
            InitializeComponent();
        }

        String archivo, ext, del_nom, sub_nom,subdele,carpeta_sel="",not_sel="",url="";
        string[] datos_word;
        string[] word_datos;
        int j = 0, modo_origen_datos=0,ini_grid=0;
              

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;

        //Tablas
        DataTable tabla_word = new DataTable();
        DataTable tabla_datos = new DataTable();
        DataTable tabla_datos_ordenada = new DataTable();
        DataTable notificadores = new DataTable();
        DataTable patrones = new DataTable();
        DataTable noti_load = new DataTable();
        DataTable noti_load_persis = new DataTable();
        DataTable consultamysql = new DataTable();
        DataTable datos_creditos = new DataTable();
        DataTable data_sindo = new DataTable();
        DataTable datos_totales = new DataTable();
        DataTable data_notif = new DataTable();


        //Declaracion del Delegado y del Hilo para ejecutar un subproceso
        private Thread hilo2 = null;
        //private Thread hilo3 = null;

        //notificadores
        public void llenar_Cb2()
        {
            int i = 0;
            conex.conectar("base_principal");
            comboBox2.Items.Clear();
            //dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
            //noti_load = conex.consultar("SELECT DISTINCT (notificador) FROM base_principal.datos_factura WHERE (fecha_notificacion is null and status =\"0\" or status=\"EN TRAMITE\" and fecha_recepcion is null) ORDER BY notificador;");
            noti_load = conex.consultar("SELECT apellido,nombre,num_mat,contrato_ini,contrato_fin FROM usuarios WHERE puesto =\"notificador\" and controlador <> \"0\" order by apellido;");
            dataGridView2.DataSource = noti_load;
            do
            {
                //comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
                comboBox2.Items.Add(noti_load.Rows[i][0].ToString() + " " + noti_load.Rows[i][1].ToString());
                i++;
            } while (i < noti_load.Rows.Count);

            i = 0;
            //comboBox3.Items.Add("--NINGUNO--");
            /*
            for (int j = 0; j < dataGridView2.ColumnCount; j++)
            {
                noti_load_persis.Columns.Add();
            }

            for (int j = 0; j < dataGridView2.RowCount; j++)
            {
                DataRow fila_copia = noti_load_persis.NewRow();
                for (int k = 0; k < dataGridView2.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView2.Rows[j].Cells[k].Value.ToString();
                }
                noti_load_persis.Rows.Add(fila_copia);
            }*/

            noti_load_persis = copiar_tabla(noti_load);
            conex.cerrar();
        }

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
            int i = 0,not_col=0;
            Boolean col_found = false;

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
                        tabla_datos = dataSet.Tables[0];
                        //tabla_datos_ordenada = tabla_datos;
                        /*tabla_datos.DefaultView.Sort = "NOTIFICADOR desc, NRP desc, CREDITO desc";
                        tabla_datos = tabla_datos.DefaultView.ToTable();*/
                        notificadores = tabla_datos.DefaultView.ToTable(true, "NOTIFICADOR");
                        notificadores.DefaultView.Sort = "NOTIFICADOR asc";
                        notificadores = notificadores.DefaultView.ToTable();
                        //dataGridView1.DataSource = tabla_datos; //le asignamos al DataGridView el contenido del dataSet
                        modo_origen_datos = 1;
                        ini_grid = 0;
                        dataGridView1.DataSource = tabla_datos;                        
                        conexion.Close();//cerramos la conexion
                        dataGridView1.AllowUserToAddRows = false;       //eliminamos la ultima fila del datagridview que se autoagrega
                        label5.Text = "Registros: " + dataGridView1.RowCount;
                        label5.Refresh();

                        estilo_grid();

                        i = 0;

                        if (dataGridView1.RowCount > 0)
                        {
                            button4.Enabled = true;
                            //button7.Enabled=true;							
                        }
                        else
                        {
                            button4.Enabled = false;
                            //button7.Enabled=false;	
                        }

                        //MessageBox.Show("|" + notificadores.Columns[0].ColumnName.ToUpper()+"|", "ERROR");

                        for(int k=0; k<notificadores.Columns.Count;k++){
                            if (notificadores.Columns[k].ColumnName.ToUpper().ToString() == "NOTIFICADOR")
                            {
                                not_col = k;
                                col_found = true;
                            }
                        }

                        if(col_found==true){
                            for (int j = 0; j < notificadores.Rows.Count;j++ )
                            {
                                comboBox3.Items.Add(notificadores.Rows[j][not_col].ToString());
                            }
                        }else{
                            MessageBox.Show("No se encontró la columna NOTIFICADOR en la hoja de Excel, el proceso no puede continuar, la ventana se cerrará.", "ERROR");
                            //this.Close();
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

        public string interprete_fechas(String fecha)
        {
            String fecha_inter;

            if (fecha.Length > 5)
            {
                fecha_inter = fecha.Substring(0, 2);
                //MessageBox.Show(fecha.Substring(3,2));
                switch (fecha.Substring(3, 2))
                {
                    case "01": fecha_inter = interprete_numero(fecha_inter) + " DE ENERO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "02": fecha_inter = interprete_numero(fecha_inter) + " DE FEBRERO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "03": fecha_inter = interprete_numero(fecha_inter) + " DE MARZO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "04": fecha_inter = interprete_numero(fecha_inter) + " DE ABRIL DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "05": fecha_inter = interprete_numero(fecha_inter) + " DE MAYO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "06": fecha_inter = interprete_numero(fecha_inter) + " DE JUNIO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "07": fecha_inter = interprete_numero(fecha_inter) + " DE JULIO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "08": fecha_inter = interprete_numero(fecha_inter) + " DE AGOSTO DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "09": fecha_inter = interprete_numero(fecha_inter) + " DE SEPTIEMBRE DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "10": fecha_inter = interprete_numero(fecha_inter) + " DE OCTUBRE DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "11": fecha_inter = interprete_numero(fecha_inter) + " DE NOVIEMBRE DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;

                    case "12": fecha_inter = interprete_numero(fecha_inter) + " DE DICIEMBRE DE DOS MIL " + interprete_numero(fecha.Substring(8, 2));
                        break;
                }
                return fecha_inter;
            }
            else
            {
                return " ";
            }
        }

        public string interprete_fechas_solo_mes(String fecha)
        {
            String fecha_inter = "";

            fecha_inter = fecha.Substring(0, 2);
            //MessageBox.Show(fecha.Substring(3,2));
            switch (fecha.Substring(3, 2))
            {
                case "01": fecha_inter = fecha_inter + " DE ENERO DE " + fecha.Substring(6, 4);
                    break;

                case "02": fecha_inter = fecha_inter + " DE FEBRERO DE " + fecha.Substring(6, 4);
                    break;

                case "03": fecha_inter = fecha_inter + " DE MARZO DE " + fecha.Substring(6, 4);
                    break;

                case "04": fecha_inter = fecha_inter + " DE ABRIL DE " + fecha.Substring(6, 4);
                    break;

                case "05": fecha_inter = fecha_inter + " DE MAYO DE " + fecha.Substring(6, 4);
                    break;

                case "06": fecha_inter = fecha_inter + " DE JUNIO DE " + fecha.Substring(6, 4);
                    break;

                case "07": fecha_inter = fecha_inter + " DE JULIO DE " + fecha.Substring(6, 4);
                    break;

                case "08": fecha_inter = fecha_inter + " DE AGOSTO DE " + fecha.Substring(6, 4);
                    break;

                case "09": fecha_inter = fecha_inter + " DE SEPTIEMBRE DE " + fecha.Substring(6, 4);
                    break;

                case "10": fecha_inter = fecha_inter + " DE OCTUBRE DE " + fecha.Substring(6, 4);
                    break;

                case "11": fecha_inter = fecha_inter + " DE NOVIEMBRE DE " + fecha.Substring(6, 4);
                    break;

                case "12": fecha_inter = fecha_inter + " DE DICIEMBRE DE " + fecha.Substring(6, 4);
                    break;
            }
            return fecha_inter;
        }

        public string interprete_numero(String nume)
        {
            String num_letra = "";
            int num_num;
            num_num = Convert.ToInt32(nume);

            if (num_num > 15 && num_num < 20)
            {
                num_letra = "DIECI";
                switch (num_num)
                {
                    case 16: num_letra += "SEIS"; break;
                    case 17: num_letra += "SIETE"; break;
                    case 18: num_letra += "OCHO"; break;
                    case 19: num_letra += "NUEVE"; break;
                }
            }

            if (num_num > 20 && num_num < 30)
            {
                num_letra = "VEINTI";
                switch (num_num)
                {
                    case 21: num_letra += "UNO"; break;
                    case 22: num_letra += "DOS"; break;
                    case 23: num_letra += "TRES"; break;
                    case 24: num_letra += "CUATRO"; break;
                    case 25: num_letra += "CINCO"; break;
                    case 26: num_letra += "SEIS"; break;
                    case 27: num_letra += "SIETE"; break;
                    case 28: num_letra += "OCHO"; break;
                    case 29: num_letra += "NUEVE"; break;
                }
            }

            if (num_num > 30 && num_num < 40)
            {
                num_letra = "TREINTA Y ";
                switch (num_num)
                {
                    case 31: num_letra += "UNO"; break;
                    case 32: num_letra += "DOS"; break;
                    case 33: num_letra += "TRES"; break;
                    case 34: num_letra += "CUATRO"; break;
                    case 35: num_letra += "CINCO"; break;
                    case 36: num_letra += "SEIS"; break;
                    case 37: num_letra += "SIETE"; break;
                    case 38: num_letra += "OCHO"; break;
                    case 39: num_letra += "NUEVE"; break;
                }
            }

            if (num_num > 40 && num_num < 50)
            {
                num_letra = "CUARENTA Y ";
                switch (num_num)
                {
                    case 41: num_letra += "UNO"; break;
                    case 42: num_letra += "DOS"; break;
                    case 43: num_letra += "TRES"; break;
                    case 44: num_letra += "CUATRO"; break;
                    case 45: num_letra += "CINCO"; break;
                    case 46: num_letra += "SEIS"; break;
                    case 47: num_letra += "SIETE"; break;
                    case 48: num_letra += "OCHO"; break;
                    case 49: num_letra += "NUEVE"; break;
                }
            }

            if (num_num > 50 && num_num < 60)
            {
                num_letra = "CINCUENTA Y ";
                switch (num_num)
                {
                    case 51: num_letra += "UNO"; break;
                    case 52: num_letra += "DOS"; break;
                    case 53: num_letra += "TRES"; break;
                    case 54: num_letra += "CUATRO"; break;
                    case 55: num_letra += "CINCO"; break;
                    case 56: num_letra += "SEIS"; break;
                    case 57: num_letra += "SIETE"; break;
                    case 58: num_letra += "OCHO"; break;
                    case 59: num_letra += "NUEVE"; break;
                }
            }

            switch (num_num)
            {
                case 0: num_letra = "CERO"; break;
                case 1: num_letra = "UNO"; break;
                case 2: num_letra = "DOS"; break;
                case 3: num_letra = "TRES"; break;
                case 4: num_letra = "CUATRO"; break;
                case 5: num_letra = "CINCO"; break;
                case 6: num_letra = "SEIS"; break;
                case 7: num_letra = "SIETE"; break;
                case 8: num_letra = "OCHO"; break;
                case 9: num_letra = "NUEVE"; break;
                case 10: num_letra = "DIEZ"; break;
                case 11: num_letra = "ONCE"; break;
                case 12: num_letra = "DOCE"; break;
                case 13: num_letra = "TRECE"; break;
                case 14: num_letra = "CATORCE"; break;
                case 15: num_letra = "QUINCE"; break;
                case 20: num_letra = "VEINTE"; break;
                case 30: num_letra = "TREINTA"; break;
                case 40: num_letra = "CUARENTA"; break;
                case 50: num_letra = "CINCUENTA"; break;
            }

            return num_letra;
        }

        public void llena_datos_word_excel(int indice)
        {
            int i = indice;
             
                datos_word[0] = tabla_datos_ordenada.Rows[i][8].ToString();//razon_social
                datos_word[1] = tabla_datos_ordenada.Rows[i][7].ToString();//reg_pat
                datos_word[2] = tabla_datos_ordenada.Rows[i][9].ToString();//rfc
                datos_word[3] = tabla_datos_ordenada.Rows[i][10].ToString();//domicilio
                datos_word[4] = tabla_datos_ordenada.Rows[i][25].ToString();//notificador_nombre
                datos_word[5] = tabla_datos_ordenada.Rows[i][20].ToString();//frac_art_150
                datos_word[6] = tabla_datos_ordenada.Rows[i][5].ToString();//frac_art_155
                datos_word[7] = tabla_datos_ordenada.Rows[i][6].ToString();//inciso_art_155
                datos_word[8] = tabla_datos_ordenada.Rows[i][3].ToString();//subdelegacion
                datos_word[9] = tabla_datos_ordenada.Rows[i][4].ToString();//subdelegacion_domicilio
                datos_word[10] = tabla_datos_ordenada.Rows[i][1].ToString();//delegacion
                datos_word[11] = tabla_datos_ordenada.Rows[i][21].ToString();//sector_noti
                datos_word[12] = tabla_datos_ordenada.Rows[i][26].ToString();//not_constancia
                datos_word[13] = tabla_datos_ordenada.Rows[i][27].ToString();//not_constancia_fecha
                datos_word[14] = tabla_datos_ordenada.Rows[i][28].ToString();//not_constancia_vig
                datos_word[15] = tabla_datos_ordenada.Rows[i][29].ToString();//firmante
                datos_word[16] = tabla_datos_ordenada.Rows[i][30].ToString();//cargo_firmante    
                datos_word[17] = tabla_datos_ordenada.Rows[i][31].ToString();//folio_doc
        }

        public void llena_datos_word_sql(int indice)
        {
            int i = indice;

            datos_word[0] = tabla_datos_ordenada.Rows[i][1].ToString();//razon_social
            datos_word[1] = tabla_datos_ordenada.Rows[i][2].ToString();//reg_pat
            datos_word[2] = tabla_datos_ordenada.Rows[i][3].ToString();//rfc
            datos_word[3] = tabla_datos_ordenada.Rows[i][4].ToString();//domicilio
            datos_word[4] = tabla_datos_ordenada.Rows[i][14].ToString();//notificador_nombre
            datos_word[5] = tabla_datos_ordenada.Rows[i][10].ToString();//frac_art_150
            datos_word[6] = tabla_datos_ordenada.Rows[i][11].ToString();//frac_art_155
            datos_word[7] = tabla_datos_ordenada.Rows[i][12].ToString();//inciso_art_155
            datos_word[8] = tabla_datos_ordenada.Rows[i][19].ToString();//subdelegacion
            datos_word[9] = tabla_datos_ordenada.Rows[i][20].ToString();//subdelegacion_domicilio
            datos_word[10] = tabla_datos_ordenada.Rows[i][18].ToString();//delegacion
            datos_word[11] = tabla_datos_ordenada.Rows[i][13].ToString();//sector_noti
            datos_word[12] = tabla_datos_ordenada.Rows[i][15].ToString();//not_constancia
            datos_word[13] = tabla_datos_ordenada.Rows[i][16].ToString();//not_constancia_fecha
            datos_word[14] = tabla_datos_ordenada.Rows[i][17].ToString();//not_constancia_vig
            datos_word[15] = tabla_datos_ordenada.Rows[i][21].ToString();//firmante
            datos_word[16] = tabla_datos_ordenada.Rows[i][22].ToString();//cargo_firmante    
            datos_word[17] = tabla_datos_ordenada.Rows[i][0].ToString();//folio_doc
        }

        public void exportar_word()
        {
            //Invoke(new MethodInvoker(delegate
           // {
                //OBJECT OF MISSING "NULL VALUE"   

                Object oMissing = System.Reflection.Missing.Value;
                Object oTemplatePath = @"" + url ;

                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();

                wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);

                word_datos = datos_word;

                //LLENAR CAMPOS ENCABEZADO DE PAGINA
                Microsoft.Office.Interop.Word.Range rngHeader = wordDoc.Sections[1].Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                foreach (Microsoft.Office.Interop.Word.Field fld in rngHeader.Fields)
                {
                    Range rngFieldCode_foot = fld.Code;

                    String fieldText = rngFieldCode_foot.Text;

                    // ONLY GETTING THE MAILMERGE FIELDS

                    if (fieldText.StartsWith(" MERGEFIELD"))
                    {
                        // THE TEXT COMES IN THE FORMAT OF

                        // MERGEFIELD  MyFieldName  \\* MERGEFORMAT

                        // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

                        Int32 endMerge = fieldText.IndexOf("\\");

                        Int32 fieldNameLength = fieldText.Length - endMerge;

                        String fieldName = fieldText.Substring(11, endMerge - 11);

                        // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE

                        fieldName = fieldName.Trim();

                        if (fieldName == "subdelegacion")
                        {
                            fld.Select();
                            wordApp.Selection.TypeText(word_datos[8]);
                        }
                        if (fieldName == "delegacion")
                        {
                            fld.Select();
                            wordApp.Selection.TypeText(word_datos[10]);
                        }
                    }
                    //MessageBox.Show(""+fld.Code);
                }

                //LLENAR CAMPOS CUERPO DEL DOCUMENTO
                foreach (Field myMergeField in wordDoc.Fields)
                {
                    Range rngFieldCode = myMergeField.Code;

                    String fieldText = rngFieldCode.Text;

                    // ONLY GETTING THE MAILMERGE FIELDS

                    if (fieldText.StartsWith(" MERGEFIELD"))
                    {

                        // THE TEXT COMES IN THE FORMAT OF

                        // MERGEFIELD  MyFieldName  \\* MERGEFORMAT

                        // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

                        Int32 endMerge = fieldText.IndexOf("\\");

                        Int32 fieldNameLength = fieldText.Length - endMerge;

                        String fieldName = fieldText.Substring(11, endMerge - 11);

                        // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE

                        fieldName = fieldName.Trim();

                        // **** FIELD REPLACEMENT IMPLEMENTATION GOES HERE ****//

                        // THE PROGRAMMER CAN HAVE HIS OWN IMPLEMENTATIONS HERE

                        /*
                           datos_word[0] = tabla_datos_ordenada.Rows[i][0].ToString();//razon_social
                           datos_word[1] = tabla_datos_ordenada.Rows[i][1].ToString();//reg_pat
                           datos_word[2] = tabla_datos_ordenada.Rows[i][2].ToString();//rfc
                           datos_word[3] = tabla_datos_ordenada.Rows[i][3].ToString();//domicilio
                           datos_word[4] = tabla_datos_ordenada.Rows[i][9].ToString();//notificador_nombre
                           datos_word[5] = tabla_datos_ordenada.Rows[i][10].ToString();//frac_art_150
                           datos_word[6] = tabla_datos_ordenada.Rows[i][11].ToString();//frac_art_155
                           datos_word[7] = tabla_datos_ordenada.Rows[i][12].ToString();//inciso_art_155
                           datos_word[8] = tabla_datos_ordenada.Rows[i][13].ToString();//subdelegacion
                           datos_word[9] = tabla_datos_ordenada.Rows[i][14].ToString();//subdelegacion_domicilio
                           datos_word[10] = tabla_datos_ordenada.Rows[i][16].ToString();//delegacion
                           datos_word[11] = tabla_datos_ordenada.Rows[i][24].ToString();//sector_noti
                           datos_word[12] = tabla_datos_ordenada.Rows[i][15].ToString();//not_constancia
                           datos_word[13] = tabla_datos_ordenada.Rows[i][28].ToString();//not_constancia_fecha
                           datos_word[14] = tabla_datos_ordenada.Rows[i][29].ToString();//not_constancia_vig
                           datos_word[15] = tabla_datos_ordenada.Rows[i][30].ToString();//firmante
                           datos_word[16] = tabla_datos_ordenada.Rows[i][31].ToString();//cargo_firmante    
                           datos_word[17] = tabla_datos_ordenada.Rows[i][32].ToString();//folio_doc 
                            */

                        if (fieldName == "Razon_social")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[0]);
                        }

                        if (fieldName == "reg_pat")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[1]);
                        }
                        if (fieldName == "rfc")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[2]);
                        }
                        if (fieldName == "domicilio")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[3]);
                        }
                        if (fieldName == "not_nombre")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[4]);
                        }
                        if (fieldName == "frac_art_150")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[5]);
                        }
                        if (fieldName == "frac_art_155")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[6]);
                        }
                        if (fieldName == "inciso_art_155")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[7]);
                        }
                        if (fieldName == "subdelegacion")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[8]);
                        }
                        if (fieldName == "delegacion")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[10]);
                        }
                        if (fieldName == "sub_domicilio")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[9].ToUpper());
                        }
                        if (fieldName == "sector_not")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[11]);
                        }
                        if (fieldName == "folio_documento")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[17]);
                        }
                        if (fieldName == "not_constancia")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[12]);
                        }
                        if (fieldName == "not_cons_fech_ddmmaaaa")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(interprete_fechas(word_datos[13]));
                        }
                        if (fieldName == "not_cons_vig_ddmmaaaa")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(interprete_fechas(word_datos[14]));
                        }
                        if (fieldName == "constancia_firmante")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[15].ToUpper());
                        }
                        if (fieldName == "constancia_cargo_firmante")
                        {
                            myMergeField.Select();
                            wordApp.Selection.TypeText(word_datos[16]);
                        }

                    }
                }

                //LLENAR CAMPOS  PIE DE PAGINA
                Microsoft.Office.Interop.Word.Range rngFooter = wordDoc.Sections[1].Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                foreach (Microsoft.Office.Interop.Word.Field fld in rngFooter.Fields)
                {
                    Range rngFieldCode_foot = fld.Code;

                    String fieldText = rngFieldCode_foot.Text;

                    // ONLY GETTING THE MAILMERGE FIELDS

                    if (fieldText.StartsWith(" MERGEFIELD"))
                    {

                        // THE TEXT COMES IN THE FORMAT OF

                        // MERGEFIELD  MyFieldName  \\* MERGEFORMAT

                        // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

                        Int32 endMerge = fieldText.IndexOf("\\");

                        Int32 fieldNameLength = fieldText.Length - endMerge;

                        String fieldName = fieldText.Substring(11, endMerge - 11);

                        // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE

                        fieldName = fieldName.Trim();


                        if (fieldName == "sub_domicilio")
                        {
                            fld.Select();
                            wordApp.Selection.TypeText(word_datos[9].ToUpper());
                        }
                        if (fieldName == "sector_not")
                        {
                            fld.Select();
                            wordApp.Selection.TypeText(word_datos[11]);
                        }
                        if (fieldName == "folio_documento")
                        {
                            fld.Select();
                            wordApp.Selection.TypeText(word_datos[17]);
                        }

                    }
                    //MessageBox.Show(""+fld.Code);
                }

                object objMiss = System.Reflection.Missing.Value;
                object objEndOfDocFlag = "tabla_creditos";
                Microsoft.Office.Interop.Word.Range objWordRng = wordDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range;

                Microsoft.Office.Interop.Word.Table objTab1 = wordDoc.Tables.Add(objWordRng, tabla_word.Rows.Count, 5/*num_cols*/, ref objMiss, ref objMiss);
                objTab1.Range.ParagraphFormat.SpaceAfter = 1;
                objTab1.Range.Columns[1].Width = wordApp.InchesToPoints(9.50f / 2.54f);
                objTab1.Range.Columns[2].Width = wordApp.InchesToPoints(2.00f / 2.54f);
                objTab1.Range.Columns[3].Width = wordApp.InchesToPoints(2.00f / 2.54f);
                objTab1.Range.Columns[4].Width = wordApp.InchesToPoints(2.00f / 2.54f);
                objTab1.Range.Columns[5].Width = wordApp.InchesToPoints(2.00f / 2.54f);

                objTab1.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                objTab1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

                int iRow = 1, iCols = 1;
                for (iRow = 1; iRow <= tabla_word.Rows.Count; iRow++)
                {
                    for (iCols = 1; iCols <= 5; iCols++)
                    {
                        if (iRow == 1)
                        {
                            objTab1.Cell(iRow, iCols).Range.Font.Size = 9.5f;
                            objTab1.Cell(iRow, iCols).Range.Font.Name = "Arial";
                            objTab1.Cell(iRow, iCols).Range.Font.Bold = 1;
                            objTab1.Cell(iRow, iCols).Shading.BackgroundPatternColor = WdColor.wdColorGray15;
                            objTab1.Cell(iRow, iCols).Range.Text = tabla_word.Rows[iRow - 1][iCols - 1].ToString(); //add some text to cell
                        }
                        else
                        {
                            if (iCols == 1)
                            {
                                objTab1.Cell(iRow, iCols).Range.Font.Size = 8;
                                objTab1.Cell(iRow, iCols).Range.Font.Name = "Arial";
                                objTab1.Cell(iRow, iCols).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                                objTab1.Cell(iRow, iCols).Range.Text = tabla_word.Rows[iRow - 1][iCols - 1].ToString(); //add some text to cell     
                            }
                            else
                            {
                                objTab1.Cell(iRow, iCols).Range.Font.Size = 8;
                                objTab1.Cell(iRow, iCols).Range.Font.Name = "Arial";
                                objTab1.Cell(iRow, iCols).Range.Text = tabla_word.Rows[iRow - 1][iCols - 1].ToString(); //add some text to cell
                            }
                        }
                    }
                }

                if(word_datos[4].Contains("/")==true){
                    word_datos[4] = word_datos[4].Replace("/", "-");
                }

                if (word_datos[4].Contains("\\") == true)
                {
                    word_datos[4] = word_datos[4].Replace("\\", "-");
                }

                if (word_datos[4].Contains("?") == true)
                {
                    word_datos[4] = word_datos[4].Replace("?", "-");
                }

                if (word_datos[4].Contains(":") == true)
                {
                    word_datos[4] = word_datos[4].Replace(":", "-");
                }

                if (word_datos[4].Contains("<") == true)
                {
                    word_datos[4] = word_datos[4].Replace("<", "-");
                }

                if (word_datos[4].Contains(">") == true)
                {
                    word_datos[4] = word_datos[4].Replace(">", "-");
                }

                if (word_datos[4].Contains("|") == true)
                {
                    word_datos[4] = word_datos[4].Replace("|", "-");
                }

                if (word_datos[4].Contains("*") == true)
                {
                    word_datos[4] = word_datos[4].Replace("*", "-");
                }

                if (word_datos[4].Contains("\"") == true)
                {
                    word_datos[4] = word_datos[4].Replace("\"", "-");
                }

                if (word_datos[4].Contains("\\") == true)
                {
                    word_datos[4] = word_datos[4].Replace("\\", "-");
                }

                word_datos[4] = word_datos[4].TrimEnd(' ');
                //MessageBox.Show("|" + word_datos[4] + "|");
                if ((Directory.Exists(carpeta_sel + "\\" + word_datos[4]))==false)
                {
                    Directory.CreateDirectory(carpeta_sel + "\\" + word_datos[4]);
                }
                
                wordDoc.SaveAs(carpeta_sel + "\\" + word_datos[4] + "\\" + word_datos[1] + ".doc");
                wordApp.Documents.Open(carpeta_sel + "\\" + word_datos[4] + "\\" + word_datos[1] + ".doc");
                wordApp.Application.Quit();
                //j++;
            //}));
        }
        //ANALISIS COMPLETO
        public void analizar_excel()
        {
            Invoke(new MethodInvoker(delegate
           {
               //tabla_datos_ordenada = tabla_datos;
               String rp = "", rp_ant = "";
               int porce = 0;

               if (modo_origen_datos == 1)
               {//EXCEL
                    tabla_datos_ordenada.DefaultView.Sort = "NOTIFICADOR asc, NRP asc, CREDITO asc";
               }else{
                   tabla_datos_ordenada.DefaultView.Sort = "NOTIFICADOR asc, REGISTRO PATRONAL asc, CREDITO CUOTA asc";
               }

               tabla_datos_ordenada = tabla_datos_ordenada.DefaultView.ToTable();

               if (not_sel != "NINGUNO_LZ")
               {
                   DataView dv = new DataView(tabla_datos_ordenada);
                   dv.RowFilter = "NOTIFICADOR = '" + not_sel + "'";
                   tabla_datos_ordenada = dv.ToTable();
                   //MessageBox.Show("" + tabla_datos_ordenada.Rows.Count);
               }

               if (modo_origen_datos == 1)
               {//EXCEL
                   patrones = tabla_datos_ordenada.DefaultView.ToTable(true, "NRP");
               }
               else
               {
                   patrones = tabla_datos_ordenada.DefaultView.ToTable(true, "REGISTRO PATRONAL");
               }

               

               datos_word = new string[18];
               tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");

                FileStream fichero;

                //Borrar archivos para comenzar de 0
                System.IO.File.Delete(@"word.txt");

                //Crear archivos nuevos
                fichero = System.IO.File.Create(@"word.txt");

                url = fichero.Name;
                //MessageBox.Show(url);
                fichero.Close();

                url = url.Substring(0, (url.Length - 8));

                if (radioButton1.Checked == true)
                {
                    url = @"" + url + "recursos\\ActaDeNotificacionMultiple_v4 - LZ_FINAL_GP.dotx";
                }
                else
                {
                    url = @"" + url + "recursos\\CitatorioDeNotificacionMultiple_v4 - LZ.dotx";
                }
                if (modo_origen_datos == 1)
                {//EXCEL
                    rp_ant = tabla_datos_ordenada.Rows[0][7].ToString();
                }
                else
                {
                    rp_ant = tabla_datos_ordenada.Rows[0][2].ToString();
                }

                j = 0;

                dataGridView2.DataSource = tabla_datos_ordenada;

                for (int i = 0; i < tabla_datos_ordenada.Rows.Count; i++)//--------------------------------------------------------
                {
                        if (modo_origen_datos == 1)
                        {//EXCEL
                            rp = tabla_datos_ordenada.Rows[i][7].ToString();
                        }
                        else
                        {
                            rp = tabla_datos_ordenada.Rows[i][2].ToString();
                        }

                        if ((rp != rp_ant) || (i == tabla_datos_ordenada.Rows.Count - 1) || (tabla_datos_ordenada.Rows.Count == 1))//ENVIAR A FORMATO WORD
                        {
                            if (((rp == rp_ant) && (i == tabla_datos_ordenada.Rows.Count - 1)) || (tabla_datos_ordenada.Rows.Count==1))
                            {
                                if (modo_origen_datos == 1)
                                {//EXCEL
                                    //tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
                                    tabla_word.Rows.Add(tabla_datos_ordenada.Rows[i][17].ToString(),//documento
                                                       tabla_datos_ordenada.Rows[i][14].ToString(),//credito
                                                       tabla_datos_ordenada.Rows[i][15].ToString(),//multa
                                                       tabla_datos_ordenada.Rows[i][13].ToString(),//periodo
                                                       tabla_datos_ordenada.Rows[i][18].ToString().Substring(0, 10)//fecha
                                                       );

                                    llena_datos_word_excel(i);
                                }
                                else// DE BD
                                {
                                    //tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
                                    tabla_word.Rows.Add(tabla_datos_ordenada.Rows[i][5].ToString(),//documento
                                                       tabla_datos_ordenada.Rows[i][6].ToString(),//credito
                                                       tabla_datos_ordenada.Rows[i][7].ToString(),//multa
                                                       tabla_datos_ordenada.Rows[i][8].ToString(),//periodo
                                                       tabla_datos_ordenada.Rows[i][9].ToString()//fecha
                                                       );

                                    llena_datos_word_sql(i);
                                }
                            }


                            //ENVIAR
                            exportar_word();


                            //RESETEAR TABLA
                            tabla_word.Rows.Clear();

                            if (modo_origen_datos == 1)
                            {//EXCEL
                                tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
                                tabla_word.Rows.Add(tabla_datos_ordenada.Rows[i][17].ToString(),//documento
                                                   tabla_datos_ordenada.Rows[i][14].ToString(),//credito
                                                   tabla_datos_ordenada.Rows[i][15].ToString(),//multa
                                                   tabla_datos_ordenada.Rows[i][13].ToString(),//periodo
                                                   tabla_datos_ordenada.Rows[i][18].ToString().Substring(0, 10)//fecha
                                                   );

                                llena_datos_word_excel(i);
                            }
                            else// DE BD
                            {
                                tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
                                tabla_word.Rows.Add(tabla_datos_ordenada.Rows[i][5].ToString(),//documento
                                                   tabla_datos_ordenada.Rows[i][6].ToString(),//credito
                                                   tabla_datos_ordenada.Rows[i][7].ToString(),//multa
                                                   tabla_datos_ordenada.Rows[i][8].ToString(),//periodo
                                                   tabla_datos_ordenada.Rows[i][9].ToString()//fecha
                                                   );
                                llena_datos_word_sql(i);
                            }
                            j++;
                        }
                        else
                        {
                            if (modo_origen_datos == 1)
                            {//EXCEL
                                //tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
                                tabla_word.Rows.Add(tabla_datos_ordenada.Rows[i][17].ToString(),//documento
                                                   tabla_datos_ordenada.Rows[i][14].ToString(),//credito
                                                   tabla_datos_ordenada.Rows[i][15].ToString(),//multa
                                                   tabla_datos_ordenada.Rows[i][13].ToString(),//periodo
                                                   tabla_datos_ordenada.Rows[i][18].ToString().Substring(0, 10)//fecha
                                                   );

                                llena_datos_word_excel(i);
                            }
                            else// DE BD
                            {
                                //tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
                                tabla_word.Rows.Add(tabla_datos_ordenada.Rows[i][5].ToString(),//documento
                                                   tabla_datos_ordenada.Rows[i][6].ToString(),//credito
                                                   tabla_datos_ordenada.Rows[i][7].ToString(),//multa
                                                   tabla_datos_ordenada.Rows[i][8].ToString(),//periodo
                                                   tabla_datos_ordenada.Rows[i][9].ToString()//fecha
                                                   );

                                llena_datos_word_sql(i);
                            }
                        }

                        toolStripStatusLabel1.Text = "Generando " + (j) + " de " + (patrones.Rows.Count);
                        porce = Convert.ToInt32((j * 100) / patrones.Rows.Count);
                        toolStripProgressBar1.Value = porce;
                        toolStripStatusLabel2.Text = porce + "%";

                        if (((rp != rp_ant) && (i == tabla_datos_ordenada.Rows.Count - 1)))//ENVIAR A FORMATO WORD ULTIMA FILA DIFERENTE
                        {
                            //ENVIAR
                            exportar_word();
                            j++;
                        }
                        /*
                        if (((rp == rp_ant) && (i == tabla_datos_ordenada.Rows.Count - 1)))//ENVIAR A FORMATO WORD ULTIMA FILA IGUAL
                        {
                            //ENVIAR
                            exportar_word();
                            j++;
                        }*/

                        rp_ant = rp;

                        toolStripStatusLabel1.Text = "Generando " + (j) + " de " + (patrones.Rows.Count);
                        porce = Convert.ToInt32((j * 100) / patrones.Rows.Count);
                        toolStripProgressBar1.Value = porce;
                        toolStripStatusLabel2.Text = porce + "%";
                    }//---------------------------------------------------------------------------------------------------------------------------
                
               MessageBox.Show("Los Archivos se han generado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
               dataGridView1.Enabled = true;
           }));
        }

        public void leer_config()
        {
            String del, mpio, sub, jefe_cob, jefe_emi, jefe_afi, ref_baja, ref_ofi,del_num,sub_num;
            //try
            //{
            StreamReader rdr = new StreamReader(@"sub_config.lz");            
            del_nom = rdr.ReadLine();
            del_num = rdr.ReadLine();
            mpio = rdr.ReadLine();
            sub = rdr.ReadLine();
            sub_num = rdr.ReadLine();
            subdele = rdr.ReadLine();
            jefe_afi = rdr.ReadLine();
            jefe_cob = rdr.ReadLine();
            jefe_emi = rdr.ReadLine();
            //ref_baja = rdr.ReadLine();
            //ref_ofi = rdr.ReadLine();
            rdr.Close();

            del_nom = del_nom.Substring(11, del_nom.Length - 11);
            del_num = del_num.Substring(7, del_num.Length - 7);
            mpio = mpio.Substring(10, mpio.Length - 10);
            sub = sub.Substring(14, sub.Length - 14);
            sub_num = sub_num.Substring(7, sub_num.Length - 7);
            subdele = subdele.Substring(12, subdele.Length - 12);
            jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);
            jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
            jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);

            sub_nom = sub;
            //ref_baja = ref_baja.Substring(9, ref_baja.Length - 9);
            //ref_ofi = ref_ofi.Substring(8, ref_ofi.Length - 8);

            //}
            //catch (Exception error)
            //{
            //MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}
            
        }

        public DataTable copiar_tabla(DataTable tabla_origen)
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < tabla_origen.Columns.Count;j++)
            {
                tabla_destino.Columns.Add(tabla_origen.Columns[j].ColumnName);
            }

            for (int j = 0; j < tabla_origen.Rows.Count; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < tabla_origen.Columns.Count; k++)
                {
                    fila_copia[k] = tabla_origen.Rows[j][k].ToString();
                }
                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
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

        public void formar_tabla()
        {
            if (datos_totales.Columns.Contains("FOLIO DOC.")!=true)
            {
                datos_totales.Columns.Add("FOLIO DOC.");
                datos_totales.Columns.Add("RAZON SOCIAL");
                datos_totales.Columns.Add("REGISTRO PATRONAL");
                datos_totales.Columns.Add("RFC");
                datos_totales.Columns.Add("DOMICILIO PATRON");
                datos_totales.Columns.Add("NOMBRE DOCUMENTO");
                datos_totales.Columns.Add("CREDITO CUOTA");
                datos_totales.Columns.Add("CREDITO MULTA");
                datos_totales.Columns.Add("PERIODO");
                datos_totales.Columns.Add("FECHA DOCUMENTO");
                datos_totales.Columns.Add("FRACC. ART 150");
                datos_totales.Columns.Add("FRACC. ART 155");
                datos_totales.Columns.Add("INCISO. ART 155");
                datos_totales.Columns.Add("SECTOR NOTIF.");
                datos_totales.Columns.Add("NOTIFICADOR");
                datos_totales.Columns.Add("NUM. CONSTANCIA");
                datos_totales.Columns.Add("FECHA CONSTANCIA");
                datos_totales.Columns.Add("VIGENCIA CONSTANCIA");
                datos_totales.Columns.Add("DELEGACION");
                datos_totales.Columns.Add("SUBDELEGACION");
                datos_totales.Columns.Add("DOMICILIO SUB.");
                datos_totales.Columns.Add("FIRMANTE");
                datos_totales.Columns.Add("CARGO FIRMANTE");
            }
            /*
            dataGridView1.Columns.Add("folio_doc", "FOLIO DOC.");
            dataGridView1.Columns.Add("razon_social", "RAZON SOCIAL");
            dataGridView1.Columns.Add("registro_patronal", "REGISTRO PATRONAL");
            dataGridView1.Columns.Add("rfc", "RFC");
            dataGridView1.Columns.Add("domicilio", "DOMICILIO PATRON");
            dataGridView1.Columns.Add("nom_doc", "NOMBRE DOCUMENTO");
            dataGridView1.Columns.Add("credito_cuotas", "CREDITO CUOTA");
            dataGridView1.Columns.Add("credito_multa", "CREDITO MULTA");
            dataGridView1.Columns.Add("periodo", "PERIODO");
            dataGridView1.Columns.Add("fecha_doc", "FECHA DOCUMENTO");
            dataGridView1.Columns.Add("fracc_150", "FRACC. ART 150");
            dataGridView1.Columns.Add("fracc_155", "FRACC. ART 155");
            dataGridView1.Columns.Add("inciso_155", "INCISO. ART 155");
            dataGridView1.Columns.Add("sector_not", "SECTOR NOTIF.");
            dataGridView1.Columns.Add("notificador", "NOTIFICADOR");
            dataGridView1.Columns.Add("constancia", "NUM. CONSTANCIA");
            dataGridView1.Columns.Add("fech_constancia", "FECHA CONSTANCIA");
            dataGridView1.Columns.Add("vig_constancia", "VIGENCIA CONSTANCIA");
            dataGridView1.Columns.Add("delegacion", "DELEGACION");
            dataGridView1.Columns.Add("subdelegacion", "SUBDELEGACION");
            dataGridView1.Columns.Add("sub_domicilio", "DOMICILIO SUB.");
            dataGridView1.Columns.Add("firmante", "FIRMANTE");
            dataGridView1.Columns.Add("cargo_firmante", "CARGO FIRMANTE");
            */
        }

        public string[] extractor_sindo(String reg_pat)
        {
            string[] datos_patron;
            datos_patron = new string[2];
            DataView vista = new DataView(data_sindo);
            vista.RowFilter = "registro_patronal='" + reg_pat + "'";
            dataGridView2.DataSource = vista;
            if (dataGridView2.RowCount > 0)
            {
                datos_patron[0] = dataGridView2.Rows[0].Cells[1].Value.ToString() + ", " + dataGridView2.Rows[0].Cells[2].Value.ToString() + ", " + dataGridView2.Rows[0].Cells[3].Value.ToString();
                datos_patron[1] = dataGridView2.Rows[0].Cells[4].Value.ToString();
            }
            else
            {
                datos_patron[0] = "DOMICILIO NO CARGADO ";
                datos_patron[1] = "RFC NO CARGADO";
            }

            return datos_patron;
        }

        public string nombre_docto_full(String tipo_doc, String nom_per)
        {
            String nom_docto_txt = "";
            switch (tipo_doc)
            {
                case "0": nom_docto_txt = "CÉDULA DE LIQUIDACIÓN DE CAPITALES CONSTITUTIVOS";
                    break;
                case "2": nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACIÓN Y PAGO CUOTAS";
                    break;
                case "3":
                    if (nom_per.StartsWith("COP") == true)
                    {
                        nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACIÓN Y PAGO DE CUOTAS";
                    }
                    else
                    {
                        nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACIÓN Y PAGO DE CUOTAS CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
                    }
                    break;
                case "6": nom_docto_txt = "CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACIÓN Y PAGO DE CUOTAS CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
                    break;

                case "89": nom_docto_txt = "RESOLUCIÓN PARA CUBRIR LA PRIMA MEDIA";
                    break;

                default: nom_docto_txt = "DOCUMENTO SIN NOMBRE ESPECIFICADO";
                    break;
            }

            return nom_docto_txt;
        }

        public void estilo_grid()
        {
            for (int i = 0; i < dataGridView1.ColumnCount;i++ )
            {
                dataGridView1.Columns[i].ReadOnly = true;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].HeaderText = dataGridView1.Columns[i].HeaderText.ToUpper();
            }

            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[0].ReadOnly = false;
            
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[i].Cells[0].Value = false;
            }
        }

        public DataTable checa_marcados()
        {
            DataTable tabla_destino = new DataTable();

            List<int> lista_extractos = new List<int>();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value.ToString()) == true)
                {
                    lista_extractos.Add(i);
                }
            }

            int[] lista = lista_extractos.ToArray();

            for (int j = 1; j < dataGridView1.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView1.Columns[j].HeaderText);
            }

            for (int j = 0; j < lista.Length; j++)
            {

                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView1.ColumnCount-1; k++)
                {
                    fila_copia[k] = dataGridView1.Rows[lista[j]].Cells[k+1].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);

            }

            return tabla_destino;
        }

        public void blanquear_filas()
        {
            int i = 0,col_count=0;

            while (i < dataGridView1.RowCount)
            {
                if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)) != true)
                {
                    col_count = 1;
                    while (col_count < dataGridView1.ColumnCount)
                    {
                        dataGridView1.Rows[i].Cells[col_count].Style.BackColor = System.Drawing.Color.White;
                        col_count++;
                    }
                }
               i++;
            } 
        }

        public void blanquear_desmarcar_filas()
        {
            int i = 0, col_count = 0;

            while (i < dataGridView1.RowCount)
            {
                //if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)) != true)
                //{
                dataGridView1.Rows[i].Cells[0].Value = false;
                col_count = 1;
                while (col_count < dataGridView1.ColumnCount)
                {
                    dataGridView1.Rows[i].Cells[col_count].Style.BackColor = System.Drawing.Color.White;                        
                    col_count++;
                }
                //}
                i++;
            }
        }

        //GENERAR ARCHIVOS
        private void button4_Click(object sender, EventArgs e)
        {   
            String tipo_arch="";
            if(radioButton1.Checked==true){
                tipo_arch = "las Actas de Notificación";
            }else{
                tipo_arch = "los Citatorios";
            }

             DialogResult resul0 = MessageBox.Show("Se van Generar los Archivos de Word de "+tipo_arch+".\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

             if (resul0 == DialogResult.Yes)
             {
                 FolderBrowserDialog fbd = new FolderBrowserDialog();
                 fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden las Actas Y Citatorios una vez que se generen:";
                 DialogResult result = fbd.ShowDialog();

                 if (result == DialogResult.OK)
                 {
                     carpeta_sel = fbd.SelectedPath.ToString();
                     dataGridView1.Enabled = false;
                     int indice_notificador = -1;

                     if (checa_marcados().Rows.Count > 0)
                     {
                         tabla_datos_ordenada = checa_marcados();
                     }
                     else
                     {
                         tabla_datos_ordenada = copiar_datagrid();
                     }

                     if (modo_origen_datos == 1)
                     {//excel
                         if((comboBox3.SelectedIndex>-1)&& (checkBox1.Checked == true)){
                             indice_notificador = comboBox3.SelectedIndex;
                             not_sel = comboBox3.SelectedItem.ToString();
                         }else{
                             not_sel = "NINGUNO_LZ";                             
                         }
                     }
                     else//de BD
                     {
                         indice_notificador = comboBox2.SelectedIndex;
                         not_sel = comboBox2.SelectedItem.ToString();                         
                         //tabla_datos_ordenada = copiar_tabla(datos_totales);
                     }

                     //MessageBox.Show(carpeta_sel);
                     //if ((indice_notificador > -1) && (checkBox1.Checked == true))
                     //{
                         //MessageBox.Show("" +comboBox3.SelectedItem.ToString());
                         //not_sel = comboBox3.SelectedItem.ToString();
                         hilo2 = new Thread(new ThreadStart(analizar_excel));
                         hilo2.Start();
                    // }
                    // else
                    // {
                      //   hilo2 = new Thread(new ThreadStart(analizar_excel));
                      //   hilo2.Start();
                     //}
                 }
             }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            carga_excel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                cargar_hoja_excel();
                modo_origen_datos = 1;
            }
        }

        private void Correspondencia_V2_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
            
            tabla_word.Columns.Add("documento");
            tabla_word.Columns.Add("credito");
            tabla_word.Columns.Add("multa");
            tabla_word.Columns.Add("periodo");
            tabla_word.Columns.Add("fecha");

            comboBox3.SelectedIndex = -1;
            checkBox1.Checked = false;

            //conseguir notificadores
            llenar_Cb2();

           /* tabla_word.Rows.Add("Documento", "Crédito", "Multa", "Periodo", "Fecha");
            tabla_word.Rows.Add("CEDULA DE LIQUIDACION XXX", "200456798", "201456789", "202009", "20/09/2020");
            tabla_word.Rows.Add("CEDULA DE LIQUIDACION XXX", "200456798", "201456789", "202009", "21/09/2020");
            tabla_word.Rows.Add("CEDULA DE LIQUIDACION XXX", "200456798", "201456789", "202009", "22/09/2020");
            tabla_word.Rows.Add("CEDULA DE LIQUIDACION XXX", "200456798", "201456789", "202009", "23/09/2020");
            tabla_word.Rows.Add("CEDULA DE LIQUIDACION XXX", "200456798", "201456789", "202009", "24/09/2020");*/
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                comboBox3.Enabled = true;
            }
            else
            {
                comboBox3.Enabled = false;
                comboBox3.SelectedIndex = -1;
            }
        }
        //modo excel
        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            checkBox1.Enabled = true;
        }
        //modo_sql
        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            checkBox1.Enabled = false;
            
            //conseguir patrones
            conex.conectar("base_principal");
            consultamysql = conex.consultar("SELECT registro_patronal,domicilio,localidad,cp,rfc FROM sindo");
            data_sindo = copiar_tabla(consultamysql);
            leer_config();
        }
        //consulta_bd
        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex > -1){
                conex.conectar("base_principal");
                consultamysql = conex.consultar("SELECT razon_social,registro_patronal1,registro_patronal,tipo_documento,nombre_periodo,credito_cuotas,credito_multa,periodo,sector_notificacion_actualizado,notificador,subdelegacion FROM base_principal.datos_factura where (fecha_notificacion is null and status =\"0\" or status=\"EN TRAMITE\" and fecha_recepcion is null) and notificador=\"" + comboBox2.SelectedItem.ToString() + "\" order by registro_patronal1 asc, periodo asc");

                datos_creditos = copiar_tabla(consultamysql);

                formar_tabla();
                /*
                    datos_totales.Columns.Add("FOLIO DOC.");
                    datos_totales.Columns.Add("RAZON SOCIAL");
                    datos_totales.Columns.Add("REGISTRO PATRONAL");
                    datos_totales.Columns.Add("RFC");
                    datos_totales.Columns.Add("DOMICILIO PATRON");
                    datos_totales.Columns.Add("NOMBRE DOCUMENTO");
                    datos_totales.Columns.Add("CREDITO CUOTA");
                    datos_totales.Columns.Add("CREDITO MULTA");
                    datos_totales.Columns.Add("PERIODO");
                    datos_totales.Columns.Add("FECHA DOCUMENTO");
                    datos_totales.Columns.Add("FRACC. ART 150");
                    datos_totales.Columns.Add("FRACC. ART 155");
                    datos_totales.Columns.Add("INCISO. ART 155");
                    datos_totales.Columns.Add("SECTOR NOTIF.");
                    datos_totales.Columns.Add("NOTIFICADOR");
                    datos_totales.Columns.Add("NUM. CONSTANCIA");
                    datos_totales.Columns.Add("FECHA CONSTANCIA");
                    datos_totales.Columns.Add("VIGENCIA CONSTANCIA");
                    datos_totales.Columns.Add("DELEGACION");
                    datos_totales.Columns.Add("SUBDELEGACION");
                    datos_totales.Columns.Add("DOMICILIO SUB.");
                    datos_totales.Columns.Add("FIRMANTE");
                    datos_totales.Columns.Add("CARGO FIRMANTE");
                 */
                

                for (int i = 0; i < datos_creditos.Rows.Count; i++)
                {
                    string[] datos_patron = new string[2];

                    datos_patron = extractor_sindo(datos_creditos.Rows[i][1].ToString());
                    datos_totales.Rows.Add(
                        (i+1),//FOLIO DOC
                        datos_creditos.Rows[i][0].ToString(),//RAZON SOCIAL
                        datos_creditos.Rows[i][2].ToString(),//REGISTRO PATRONAL
                        datos_patron[1],//RFC
                        datos_patron[0],//DOMICILIO
                        nombre_docto_full(datos_creditos.Rows[i][3].ToString(),datos_creditos.Rows[i][4].ToString()),//NOM DOCUMENTO
                        datos_creditos.Rows[i][5].ToString(),//CREDITO CUOTA
                        datos_creditos.Rows[i][6].ToString(),//CREDITO MULTA
                        datos_creditos.Rows[i][7].ToString(),//PERIODO
                        "01/01/0001",//FECHA DOCUMENTO
                        "IX",//FRAC ART 150
                        "XIII",//FRAC ART 155
                        "b)",//INCISO ART 155
                        datos_creditos.Rows[i][8].ToString(),//SECTOR NOTIF
                        (noti_load_persis.Rows[comboBox2.SelectedIndex][0].ToString()+" "+noti_load_persis.Rows[comboBox2.SelectedIndex][1].ToString()).ToUpper(),//NOTIFICADOR
                        noti_load_persis.Rows[comboBox2.SelectedIndex][2].ToString(),//CONSTANCIA
                        noti_load_persis.Rows[comboBox2.SelectedIndex][3].ToString().Substring(0,10),//CONSTANCIA INI
                        noti_load_persis.Rows[comboBox2.SelectedIndex][4].ToString().Substring(0, 10),//CONSTANCIA FIN
                        "ESTATAL "+del_nom.ToUpper(),//DELEGACION
                        sub_nom.ToUpper(),//SUBDELEGACION
                        "Avenida Ávila Camacho No. 1696, Col. Niños Héroes, Sector Hidalgo,  C.P. 44260, Guadalajara, Jalisco.",//DOMICILIO SUB
                        subdele,//FIRMANTE
                        "TITULAR"//CARGO FIRMANTE
                        );
                }

                modo_origen_datos = 2;
                ini_grid = 0;
                dataGridView1.DataSource = datos_totales;
                label5.Text = "Registros: "+dataGridView1.RowCount;

                

                if (dataGridView1.RowCount > 0)
                {
                    button4.Enabled = true;
                    //button7.Enabled=true;							
                }
                else
                {
                    button4.Enabled = false;
                    //button7.Enabled=false;	
                }

                estilo_grid();             

                //dataGridView2.DataSource = noti_load_persis;
                //MessageBox.Show("notificadores:" + noti_load_persis.Rows.Count+"\n sindo: "+data_sindo.Rows.Count+"\n datos creditos"+datos_creditos.Rows.Count);
            }
        }

        private void checkBox1_EnabledChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = -1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            orden_columnas orden = new orden_columnas();
            orden.Show();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(tabla_datos);
            dv.RowFilter = "NOTIFICADOR = '" + comboBox3.SelectedItem + "'";
            data_notif = dv.ToTable();

            if (modo_origen_datos == 1)
            {//EXCEL
                data_notif.DefaultView.Sort = "NOTIFICADOR asc, NRP asc, CREDITO asc";
            }
            else
            {
                data_notif.DefaultView.Sort = "NOTIFICADOR asc, REGISTRO PATRONAL asc, CREDITO CUOTA asc";
            }

            ini_grid = 0;
            dataGridView1.DataSource = data_notif;
            estilo_grid();
            label5.Text = "Registros: " + dataGridView1.RowCount;
            label5.Refresh();
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
                    } while (cont <= dataGridView1.ColumnCount-1);
                    maskedTextBox1.Focus();
                    maskedTextBox1.SelectionStart = 0;
                }
                else
                {
                    cont = 1;
                    do
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[cont].Style.BackColor = System.Drawing.Color.White;
                        cont++;
                    } while (cont <= dataGridView1.ColumnCount-1);

                }
                
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            int foun = 0,cred_cuo=0,cred_mul=0;

            if (modo_origen_datos == 1)
            {//EXCEL
                cred_cuo = 15;
                cred_mul = 16;
            }
            else
            {
                cred_cuo = 7;
                cred_mul = 8;
            }

            if (dataGridView1.RowCount > 0)
            {
                if (maskedTextBox1.Text.Length == 9)
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[cred_cuo].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[cred_cuo].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            //.Rows[i].Cells[0].Value = true;
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                            foun = 1;
                        }

                        if (dataGridView1.Rows[i].Cells[cred_mul].Value.ToString() == maskedTextBox1.Text.ToString())
                        {
                            dataGridView1.Focus();
                            dataGridView1.Rows[i].Cells[cred_mul].Selected = true;
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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int indice = -1, cont=0,i=0;
            string rp="",rp_verif="";
            
            indice = dataGridView1.CurrentRow.Index;
            
            //MessageBox.Show(""+indice+"\n "+dataGridView1.Rows[indice].Cells[8].Value.ToString());
            if (ini_grid > 0)
            {
                if (indice > -1)
                {
                    blanquear_filas();

                    if (modo_origen_datos == 1)
                    {//EXCEL
                        rp = dataGridView1.Rows[indice].Cells[8].Value.ToString();
                    }
                    else
                    {
                        rp = dataGridView1.Rows[indice].Cells[2].Value.ToString();
                    }

                    i = indice;


                    //colorear previos
                    while (i >= 0)
                    {

                        if (modo_origen_datos == 1)
                        {//EXCEL
                            rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        }
                        else
                        {
                            rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        }

                        if (rp_verif == rp)
                        {//COLOREAR FILA 
                            if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)) != true)
                            {
                                cont = 1;
                                while (cont < dataGridView1.ColumnCount)
                                {
                                    dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.PaleTurquoise;
                                    cont++;
                                }
                            }
                            i--;
                        }
                        else
                        {
                            i = -1;
                        }

                    }

                    i = indice;

                    //colorear siguientes
                    while (i < dataGridView1.RowCount)
                    {
                        if (modo_origen_datos == 1)
                        {//EXCEL
                            rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        }
                        else
                        {
                            rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        }

                        //MessageBox.Show("rp: "+rp+"|rp_verif: "+rp_verif);

                        if (rp_verif == rp)
                        {//COLOREAR FILA 
                            if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)) != true)
                            {
                                cont = 1;
                                while (cont < dataGridView1.ColumnCount)
                                {
                                    dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.PaleTurquoise;
                                    cont++;
                                }
                            }
                            i++;
                        }
                        else
                        {
                            i = dataGridView1.RowCount + 1;
                        }

                    }

                }

            }//ini_grid

            ini_grid++;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void marcarPatronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indice = -1, cont = 0, i = 0;
            string rp = "", rp_verif = "";

            indice = dataGridView1.CurrentRow.Index;

            if (indice > -1)
            {
                if (modo_origen_datos == 1)
                {//EXCEL
                    rp = dataGridView1.Rows[indice].Cells[8].Value.ToString();
                }
                else
                {
                    rp = dataGridView1.Rows[indice].Cells[2].Value.ToString();
                }

                i = indice;


                //marcar previos
                while (i >= 0)
                {
                    if (modo_origen_datos == 1)
                    {//EXCEL
                        rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                    }
                    else
                    {
                        rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    }

                    if (rp_verif == rp)
                    {//COLOREAR FILA 
                        cont = 1;

                        while (cont < dataGridView1.ColumnCount)
                        {
                            dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                            cont++;
                        }

                        dataGridView1.Rows[i].Cells[0].Value = true;
                        i--;
                    }
                    else
                    {
                        i = -1;
                    }

                }

                i = indice;

                //colorear siguientes
                while (i < dataGridView1.RowCount)
                {
                    if (modo_origen_datos == 1)
                    {//EXCEL
                        rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                    }
                    else
                    {
                        rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    }

                    //MessageBox.Show("rp: "+rp+"|rp_verif: "+rp_verif);

                    if (rp_verif == rp)
                    {//COLOREAR FILA 
                        dataGridView1.Rows[i].Cells[0].Value = true;
                        cont = 1;

                        while (cont < dataGridView1.ColumnCount)
                        {
                            dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                            cont++;
                        }

                        i++;
                    }
                    else
                    {
                        i = dataGridView1.RowCount + 1;
                    }

                }
                dataGridView1.Rows[indice].Cells[0].Value = true;
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            /*if (e.Button == MouseButtons.Right)
            {
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].Selected = true;
            }*/
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

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            
            //if (e.KeyChar == Convert.ToChar(' '))
           /* if(e.KeyCode== Keys.Space)//verificar tecla barra espaciadora
            {
                maskedTextBox1.Focus();
            }*/
        }

        private void desmarcarPatronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indice = -1, cont = 0, i = 0;
            string rp = "", rp_verif = "";

            indice = dataGridView1.CurrentRow.Index;
            //MessageBox.Show("" + dataGridView1.Rows[indice].Cells[9].Value.ToString());
            //dataGridView1.Rows[indice].Cells[0].Value = false;

            if (indice > -1)
            {
                if (modo_origen_datos == 1)
                {//EXCEL
                    rp = dataGridView1.Rows[indice].Cells[8].Value.ToString();
                }
                else
                {
                    rp = dataGridView1.Rows[indice].Cells[2].Value.ToString();
                }

                i = indice;


                //marcar previos
                while (i >= 0)
                {
                    if (modo_origen_datos == 1)
                    {//EXCEL
                        rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                    }
                    else
                    {
                        rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    }

                    if (rp_verif == rp)
                    {//COLOREAR FILA 
                        cont = 1;

                        while (cont < dataGridView1.ColumnCount)
                        {
                            dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.PaleTurquoise;
                            cont++;
                        }

                        dataGridView1.Rows[i].Cells[0].Value = false;
                        i--;
                    }
                    else
                    {
                        i = -1;
                    }

                }

                i = indice;

                //colorear siguientes
                while (i < dataGridView1.RowCount)
                {
                    if (modo_origen_datos == 1)
                    {//EXCEL
                        rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                    }
                    else
                    {
                        rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    }

                    //MessageBox.Show("rp: "+rp+"|rp_verif: "+rp_verif);

                    if (rp_verif == rp)
                    {//COLOREAR FILA 
                        dataGridView1.Rows[i].Cells[0].Value = false;
                        cont = 1;

                        while (cont < dataGridView1.ColumnCount)
                        {
                            dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.PaleTurquoise;
                            cont++;
                        }

                        i++;
                    }
                    else
                    {
                        i = dataGridView1.RowCount + 1;
                    }

                }
                dataGridView1.Rows[indice].Cells[0].Value = false;
            }
        }

        private void desmarcarTodosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indice = -1, cont = 0, i = 0;
            string rp = "", rp_verif = "";

            indice = dataGridView1.CurrentRow.Index;

            //MessageBox.Show(""+indice+"\n "+dataGridView1.Rows[indice].Cells[8].Value.ToString());
            if (ini_grid > 0)
            {
                if (indice > -1)
                {
                    //blanquear_filas();
                    blanquear_desmarcar_filas();

                    if (modo_origen_datos == 1)
                    {//EXCEL
                        rp = dataGridView1.Rows[indice].Cells[8].Value.ToString();
                    }
                    else
                    {
                        rp = dataGridView1.Rows[indice].Cells[2].Value.ToString();
                    }

                    i = indice;


                    //colorear previos
                    while (i >= 0)
                    {

                        if (modo_origen_datos == 1)
                        {//EXCEL
                            rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        }
                        else
                        {
                            rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        }

                        if (rp_verif == rp)
                        {//COLOREAR FILA 
                            if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)) != true)
                            {
                                cont = 1;
                                while (cont < dataGridView1.ColumnCount)
                                {
                                    dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.PaleTurquoise;
                                    cont++;
                                }
                            }
                            i--;
                        }
                        else
                        {
                            i = -1;
                        }

                    }

                    i = indice;

                    //colorear siguientes
                    while (i < dataGridView1.RowCount)
                    {
                        if (modo_origen_datos == 1)
                        {//EXCEL
                            rp_verif = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        }
                        else
                        {
                            rp_verif = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        }

                        //MessageBox.Show("rp: "+rp+"|rp_verif: "+rp_verif);

                        if (rp_verif == rp)
                        {//COLOREAR FILA 
                            if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value)) != true)
                            {
                                cont = 1;
                                while (cont < dataGridView1.ColumnCount)
                                {
                                    dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.PaleTurquoise;
                                    cont++;
                                }
                            }
                            i++;
                        }
                        else
                        {
                            i = dataGridView1.RowCount + 1;
                        }
                    }
                }

            }//ini_grid

            ini_grid++;
        }
    }
}

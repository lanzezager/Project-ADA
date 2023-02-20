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

namespace Nova_Gear
{
    public partial class Lector_CLEM : Form
    {
        public Lector_CLEM()
        {
            InitializeComponent();
        }

        Conexion conex = new Conexion();
		DataTable hojas = new DataTable();
        DataTable clem = new DataTable();
        DataTable verif_per = new DataTable();

        //Declaracion de elementos para conexion office
		OleDbConnection conexion = null;
		DataSet dataSet = null;
		OleDbDataAdapter dataAdapter = null;

        //Declaracion de Hilo
		private Thread hilosecundario = null;

        public void formato_grid()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
        }

        public void carga_chema_excel(){
			int i=0,filas = 0;
			String tabla;
			
			hojas.Rows.Clear();
			dataGridView2.DataSource  = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			
			filas=(dataGridView2.RowCount)-1;
			do{
				if (!(dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("")){
					if ((dataGridView2.Rows[i].Cells[3].Value.ToString()).Equals("TABLE")){
						tabla=dataGridView2.Rows[i].Cells[2].Value.ToString();
                        //MessageBox.Show(tabla);
						if((tabla.Contains("$"))==true){
							//tabla = tabla.Remove((tabla.Length-1),1);
							hojas.Rows.Add(tabla);
						}
					}
				}
				i++;
			}while(i<=filas);
			
			//dataGridView2.DataSource=null;
		}

        public void cargar_hoja(){
		
			String hoja,cons_exc="";
            //dataGridView2.DataSource = hojas;
			hoja = hojas.Rows[0][0].ToString();
			
			//MessageBox.Show(hojas.Rows[0][0].ToString());
				
			if (string.IsNullOrEmpty(hoja))
			{
				MessageBox.Show("No hay una hoja para leer");
			}
			else
            {

                cons_exc = "Select [SubDelegacion],[Tipo_Documento],[Nombre_Periodo],[Registro_Patronal],[Razon_Social],[Periodo],"+
                           "[Credito_Cuota],[Credito_Multa],[Importe_Cuota],[Importe_Multa],[Sector],[Domicilio],[Localidad],[Fecha_Documento],[Hoja_Pag_Inicial],[Hoja_Pag_Termino] from [" + hoja + "] ";
				
				try
				{
					//Si el usuario escribio el nombre de la hoja se procedera con la busqueda
					//conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
					//conexion.Open(); //abrimos la conexion
					dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataAdapter
					dataSet = new DataSet(); // creamos la instancia del objeto DataSet
					//tablarow3.Rows.Clear();
					//dataSet.Tables.Add(tablarow3);	
					if(dataAdapter.Equals(null)){
						MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n","Error al Abrir Archivo de Excel/");
					}else{
						if(dataAdapter == null){}else{
							dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
							clem.Rows.Clear();
							clem=dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
							
							conexion.Close();//cerramos la conexion
                            dataGridView1.DataSource = clem;
                            formato_grid();


                            label3.Text = "Registros Totales: " + dataGridView1.RowCount;
                            textBox1.Text = clem.Rows[0][2].ToString();
						}
					}
				}catch(Exception fd){
					MessageBox.Show("Ocurrió un error al momento de leer el archivo:\n"+fd,"ERROR");
				}
			}
		}

        public void guarda_CLEM()
        {
            conex.conectar("base_principal");

            String sql = "", sql2 = "", subdel = "", num_subdel = "00";
            String nombre_per = "", fecha_doc = "", reg_pat0="",reg_pat1 = "", reg_pat2 = "", per = "", pags="",tipo_doc="",ra_soc="";

            int i=0,j=0,tot_guar=0;
            sql = "INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,subdelegacion,sector_notificacion_inicial,sector_notificacion_actualizado,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,pags_pdf,tipo_documento) VALUES ";
            
            subdel=conex.leer_config_sub()[3];
            

            nombre_per=clem.Rows[0][2].ToString();
            fecha_doc=clem.Rows[0][13].ToString();

            if (clem.Rows[0][0].ToString().ToUpper()==subdel.ToUpper())
            {
                num_subdel = conex.leer_config_sub()[4];
            }

            for (i = 0; i < clem.Rows.Count; i++)
            {
                reg_pat0 = clem.Rows[i][3].ToString();

                if(reg_pat0.StartsWith("4")==true){
                    reg_pat0=reg_pat0.Insert(0,"0");
                }

                reg_pat0 = reg_pat0.Substring(0, 11);

                reg_pat1 = reg_pat0;
                reg_pat2=reg_pat0;
                reg_pat1=reg_pat1.Insert(3,"-");
                reg_pat1=reg_pat1.Insert(9,"-");
                reg_pat1=reg_pat1.Insert(12,"-");

                reg_pat2=reg_pat2.Substring(0,10);

                per = clem.Rows[i][5].ToString();

                per = per.Remove(4, 1);

                pags = clem.Rows[i][14].ToString() + "-" + clem.Rows[i][15].ToString();

                tipo_doc = clem.Rows[i][1].ToString();

                ra_soc = clem.Rows[i][4].ToString();

                ra_soc = ra_soc.Replace('"','*');
                ra_soc = ra_soc.Replace('\'', '*');

                sql2 += "(\"" + nombre_per + "\",\"" + reg_pat1 + "\",\"" + reg_pat0 + "\",\"" + reg_pat2 + "\",\"" + ra_soc + "\"," + num_subdel + ",\"" + clem.Rows[i][10].ToString() + "\",\"" + clem.Rows[i][10].ToString() + "\",\"" + per + "\",\"" + clem.Rows[i][6].ToString() + "\",\"" + clem.Rows[i][7].ToString() + "\"," + clem.Rows[i][8].ToString() + "," + clem.Rows[i][9].ToString() + ",\"" + pags + "\"," + tipo_doc + "),";

                if(j==1000){
                    sql2 = sql2.Substring(0, sql2.Length - 1);
                    conex.consultar(sql + sql2);
                    sql2 = "";
                    j = 0;
                }

                j++;

                Invoke(new MethodInvoker(delegate
                {
                    tot_guar = Convert.ToInt32((i * 100) / clem.Rows.Count);
                    toolStripStatusLabel1.Text= tot_guar + "%";

                    if (tot_guar < 101)
                    {
                        toolStripProgressBar1.Value = tot_guar;
                    }
                }));
            }

            sql2 = sql2.Substring(0, sql2.Length - 1);
            conex.consultar(sql + sql2);

            Invoke(new MethodInvoker(delegate
            {
                tot_guar = 100;
                toolStripStatusLabel1.Text = tot_guar + "%";

                if (tot_guar < 101)
                {
                    toolStripProgressBar1.Value = tot_guar;
                }
            }));

            conex.guardar_evento("Se Ingreso el CLEM " + nombre_per + " de " + clem.Rows.Count + " casos");
            MessageBox.Show("Se Ingreso el CLEM " + nombre_per + " de " + clem.Rows.Count + " casos.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            conex.cerrar();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String archivo, ext, cad_con;

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Seleccione el archivo CLEM";//le damos un titulo a la ventana
            dialog.Filter = "Archivos de Excel (*.xls *.xlsx)|*.xls;*.xlsx"; //le indicamos el tipo de filtro en este caso que busque

            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archivo = dialog.FileName;
                //label1.Text="RALE: "+archivo;
                ext = archivo.Substring(((archivo.Length) - 3), 3);
                ext = ext.ToLower();

                if ((ext.Equals("xls")) || (ext.Equals("lsx")))
                {

                    //MessageBox.Show("archivo: "+archivo);
                    try
                    {
                        //esta cadena es para archivos excel 2007 y 2010
                        cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
                        conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                        conexion.Open(); //abrimos la conexion
                        carga_chema_excel();
                        cargar_hoja();
                        
                        if(dataGridView1.RowCount>0){
                            button2.Enabled = true;
                        }
                    }
                    catch (Exception es)
                    {

                    }
                }
            }

        }

        private void Lector_CLEM_Load(object sender, EventArgs e)
        {
            hojas.Columns.Add("hoja");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conex.conectar("base_principal");
            verif_per.Rows.Clear();
            verif_per=conex.consultar("SELECT * FROM base_principal.datos_factura where nombre_periodo=\""+textBox1.Text+"\" LIMIT 1");
            conex.cerrar();

            if (verif_per.Rows.Count == 0)
            {
                DialogResult resul0 = MessageBox.Show("Se van a ingresar los " + dataGridView1.RowCount + " registros a la base de datos como periodo: " + textBox1.Text + ".\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (resul0 == DialogResult.Yes)
                {
                   

                    //MessageBox.Show("nombre sub "+ conex.leer_config_sub()[3]+" num_sub "+conex.leer_config_sub()[4] );
                    hilosecundario = new Thread(new ThreadStart(guarda_CLEM));
                    hilosecundario.Start();
                }
            }
            else
            {
                MessageBox.Show("El periodo a ingresar ya existe en la base de datos.\n No se puede continuar.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
         }
    }
}

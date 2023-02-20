/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 09/06/2016
 * Hora: 10:25 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
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
using Ionic.Zip;


namespace Nova_Gear.Mod40
{
	/// <summary>
	/// Description of Carga_access.
	/// </summary>
	public partial class Carga_access : Form
	{
		public Carga_access()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        //Variables
        String cad_con_ofis,sql_ofis,archivo,nrp,periodo,ext,cve_unit,fecha,nombre,linea,ciclo,nrp_text,nss,periodo_txt;
        String per_count, mes_cad, periodo_nor, nom_tabla_pa, nom_tabla_as, nom_tabla_mo, primer_per, cad_con, quita_non, lista_nrps;
        int tot_row = 0, i = 0, porcentaje = 0, tot1 = 0, band1 = 0, tot_rowsdgv3 = 0, tot_rowsdgv2 = 0,k=0;
        decimal imp_tot=0;
        string[] aux;

        //Conexion MySQL
        Conexion conex = new Conexion();
        DataTable tabla_baja = new DataTable();
        DataTable tabla_baja_1 = new DataTable();
        DataTable tabla_excel = new DataTable();
        DataTable tabla_excel1= new DataTable();
        DataTable tabla_mov = new DataTable();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        OleDbConnection conexion1 = null;
        OleDbConnection conexion_mov = null;
        DataSet dataSet = null;
        DataSet dataSet1 = null;
        DataSet dataSet_mov = null;
        OleDbDataAdapter dataAdapter = null;
        OleDbDataAdapter dataAdapter1 = null;
        OleDbDataAdapter dataAdapter_mov = null;

        //Declaracion de elementos para conexion excel
        OleDbConnection conexion2 = null;
        DataSet dataSet2 = null;
        OleDbDataAdapter dataAdapter2 = null;
        DataTable tablanrps = new DataTable();
        DataTable tablanrp_guarda = new DataTable();

        //Declaracion del Delegado y del Hilo para ejecutar un subproceso
        private Thread hilosecundario = null;

        public void cargar_access_sua(String archivop,String nrpp,String periodop)
        {
            this.archivo = archivop;
            this.nrp = nrpp;
            this.periodo = periodop;
            this.periodo_nor=periodop;
        }

        public void proceso_carga_sua()
        {
        	int j=0,k=0;
        	//esta cadena es para archivos Access 2007 y 2010
        	ext = archivo.Substring(((archivo.Length) - 3), 3);
        	periodo = periodo.Substring(4, 2) + periodo.Substring(0, 4);

        	nom_tabla_pa = "CDSUPA" + periodo;
        	nom_tabla_as = "CDSUAS" + periodo;
            nom_tabla_mo = "CDSUMO" + periodo;

        	if ((ext.ToLower()).Equals("mdb"))
        	{
        		cad_con_ofis = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + archivo + "';";
        	}
        	else
        	{
        		cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';";
        	}
            Invoke(new MethodInvoker(delegate
                                     {
                                         dataGridView3.DataSource = null;
                                         dataGridView3.Columns.Clear();
                                         dataGridView3.Rows.Clear();
                                         label2.Text = "Abriendo archivo...";
                                         label2.Refresh();
                                         label1.Text = "4%";
                                         label1.Refresh();
                                         progressBar1.Value = 4;
                                         progressBar1.Refresh();
                                         //MessageBox.Show(lista_nrps);

                                         //************Componer NRP ******
                                         string[] partes = lista_nrps.Split(',');
                                         String lista_nrps_temp = "";
                                         int guion_ind = 0;

                                         for (int ii = 0; ii < partes.Length; ii++)
                                         {
                                             while((guion_ind) > -1){
                                                 guion_ind = partes[ii].IndexOf('-');
                                                 if(guion_ind > -1){
                                                    partes[ii] = partes[ii].Remove(guion_ind, 1);
                                                 }
                                             }

                                             partes[ii] = partes[ii].Insert(1,"P");
                                             lista_nrps_temp += partes[ii]+",";
                                             guion_ind = 0;
                                         }

                                         lista_nrps = lista_nrps_temp.Substring(0, lista_nrps_temp.Length - 1);
                                         //************Componer NRP ******

                                         //MessageBox.Show(lista_nrps);

                                         sql_ofis = "SELECT RCPS_DEL,RCPS_SUB,RCPS_CVE_UNIT,RCPS_PZA_SUC,RCPS_PER_PAGO,RCPS_NUM_FOL_SUA,RCPS_FECHA_PAGO,RCPS_PATRON " +
                                                 //"FROM " + nom_tabla_pa + " WHERE RCPS_PATRON = \"" + nrp + "\";";
                                                 "FROM " + nom_tabla_pa + " WHERE RCPS_PATRON IN ( "+lista_nrps + ");";

                                         label2.Text = "Generando Consulta...";
                                         label2.Refresh();
                                         label1.Text = "7%";
                                         label1.Refresh();
                                         progressBar1.Value = 7;
                                         progressBar1.Refresh();
                                     }));
        	//try
        	//{
        		conexion = new OleDbConnection(cad_con_ofis);//creamos la conexion con la hoja de excel o Access
        		conexion.Open(); //abrimos la conexion
                Invoke(new MethodInvoker(delegate
                                         {
                                             label2.Text = "Recopilando Datos del Patrón...";
                                             label2.Refresh();
                                             label1.Text = "10%";
                                             label1.Refresh();
                                             progressBar1.Value = 10;
                                             progressBar1.Refresh();
                                             this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";

                                             dataAdapter = new OleDbDataAdapter(sql_ofis, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                                             dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                                             dataAdapter.Fill(dataSet, nom_tabla_pa);//llenamos el dataset
                                             dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                                             tot_row = dataGridView1.RowCount;
                                             //MessageBox.Show("" + dataSet.Tables[0].Rows.Count);

                                             dataGridView3.Columns.Add("RCPS_DEL", "RCPS_DEL");//data_index 0
                                             dataGridView3.Columns.Add("RCPS_SUB", "RCPS_SUB"); //1
                                             dataGridView3.Columns.Add("RCPS_CVE_UNIT", "RCPS_CVE_UNIT");//2
                                             dataGridView3.Columns.Add("RCPS_PZA_SUC", "RCPS_PZA_SUC");//3
                                             dataGridView3.Columns.Add("RCPS_PER_PAGO", "RCPS_PER_PAGO");//4
                                             dataGridView3.Columns.Add("RCPS_NUM_FOL_SUA", "RCPS_NUM_FOL_SUA");//5
                                             dataGridView3.Columns.Add("RCPS_FECHA_PAGO", "RCPS_FECHA_PAGO");//6****
                                             dataGridView3.Columns.Add("RCAS_NUM_AFIL", "RCAS_NUM_AFIL");//data_index 7
                                             dataGridView3.Columns.Add("RCAS_RFC", "RCAS_RFC"); //8
                                             dataGridView3.Columns.Add("RCAS_CURP", "RCAS_CURP");//9
                                             dataGridView3.Columns.Add("RCAS_NOM_TRAB", "RCAS_NOM_TRAB");//10
                                             dataGridView3.Columns.Add("RCAS_DIAS_COT_MES", "RCAS_DIAS_COT_MES");//11
                                             dataGridView3.Columns.Add("RCAS_IMP_RET", "RCAS_IMP_RET");//12
                                             dataGridView3.Columns.Add("RCAS_IMP_EYM_ESPE", "RCAS_IMP_EYM_ESPE");//data_index 13
                                             dataGridView3.Columns.Add("RCAS_IMP_IV", "RCAS_IMP_IV"); //14
                                             dataGridView3.Columns.Add("RCAS_IMP_CYV", "RCAS_IMP_CYV");//15
                                             dataGridView3.Columns.Add("RCPS_PATRON", "RCPS_PATRON");//16
                                             dataGridView3.Columns.Add("RCMS_SAL_DIA_INT", "RCMS_SAL_DIA_INT");//17

                                             if (Convert.ToInt32(periodo.Substring(2, 4) + periodo.Substring(0, 2)) > 200602)
                                             {
                                                 dataGridView3.Columns.Add("RCAS_IMP_EYM_ESPE_O", "RCAS_IMP_EYM_ESPE_O");//18
                                                 dataGridView3.Columns.Add("RCAS_IMP_IV_O", "RCAS_IMP_IV_O");//19
                                                 dataGridView3.Columns.Add("RCAS_IMP_CYV_O", "RCAS_IMP_CYV_O");//20
                                             }

                                             //dataGridView3.Columns.Add("RCPS_PATRON", "RCPS_PATRON");//19

                                         }));
        		i = 0;

        		do{
                    Invoke(new MethodInvoker(delegate
                                     {
                                         label2.Text = "Extrayendo Informacion de Asegurados... " + (i + 1) + " de " + tot_row;
                                         label2.Refresh();
                                         barra_progreso1();
                                         this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                                         cve_unit = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                     }));
        			if (Convert.ToInt32(periodo.Substring(2, 4) + periodo.Substring(0, 2)) <= 200602)
        			{
        				sql_ofis = "SELECT RCAS_NUM_AFIL,RCAS_RFC,RCAS_CURP,RCAS_NOM_TRAB,RCAS_DIAS_COT_MES,RCAS_IMP_RET,RCAS_IMP_EYM_ESPE,RCAS_IMP_IV,RCAS_IMP_CYV " +
        					"FROM " + nom_tabla_as + " WHERE RCAS_CVE_UNIT = \"" + cve_unit + "\" ";
        			}else{
        				sql_ofis = "SELECT RCAS_NUM_AFIL,RCAS_RFC,RCAS_CURP,RCAS_NOM_TRAB,RCAS_DIAS_COT_MES,RCAS_IMP_RET,RCAS_IMP_EYM_ESPE,RCAS_IMP_IV,RCAS_IMP_CYV,RCAS_IMP_EYM_ESPE_O,RCAS_IMP_IV_O,RCAS_IMP_CYV_O " +
        					"FROM " + nom_tabla_as + " WHERE RCAS_CVE_UNIT = \"" + cve_unit + "\" ";
        				
        			}
        			
        			dataAdapter = new OleDbDataAdapter(sql_ofis, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
        			dataSet1 = new DataSet(); // creamos la instancia del objeto DataSet
        			dataAdapter.Fill(dataSet1, nom_tabla_as);//llenamos el dataset

                    Invoke(new MethodInvoker(delegate
                                     {
                                         dataGridView2.DataSource = dataSet1.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                                         //conexion.Close();//cerramos la conexion
                                         k = dataGridView2.RowCount;
                                     }));
        			j=0;

        			do{
                        Invoke(new MethodInvoker(delegate
                                     {
                                         dataGridView3.Rows.Add();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[0].Value = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[1].Value = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[2].Value = dataGridView1.Rows[i].Cells[2].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[3].Value = dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[4].Value = dataGridView1.Rows[i].Cells[4].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[5].Value = dataGridView1.Rows[i].Cells[5].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[6].Value = dataGridView1.Rows[i].Cells[6].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[7].Value = dataGridView2.Rows[j].Cells[0].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[8].Value = dataGridView2.Rows[j].Cells[1].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[9].Value = dataGridView2.Rows[j].Cells[2].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[10].Value = dataGridView2.Rows[j].Cells[3].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[11].Value = dataGridView2.Rows[j].Cells[4].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[12].Value = dataGridView2.Rows[j].Cells[5].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[13].Value = dataGridView2.Rows[j].Cells[6].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[14].Value = dataGridView2.Rows[j].Cells[7].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[15].Value = dataGridView2.Rows[j].Cells[8].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[16].Value = dataGridView1.Rows[i].Cells[7].FormattedValue.ToString();
                                         dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[17].Value = "0.00";

                                         if (Convert.ToInt32(periodo.Substring(2, 4) + periodo.Substring(0, 2)) > 200602)
                                         {
                                             dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[18].Value = dataGridView2.Rows[j].Cells[9].FormattedValue.ToString();
                                             dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[19].Value = dataGridView2.Rows[j].Cells[10].FormattedValue.ToString();
                                             dataGridView3.Rows[(dataGridView3.RowCount - 1)].Cells[20].Value = dataGridView2.Rows[j].Cells[11].FormattedValue.ToString();
                                         }


                                     }));
	        			j++;
        			}while(j<k);   

        			i++;
                    Invoke(new MethodInvoker(delegate
                    {
                        dataGridView2.DataSource = null;
                    }));
        			
        		}while(i < tot_row);


                //EXTRAER SALARIO INICIO -----------------------------------------------------
                String cve_u="", num_a="";
 
                for (int gi = 0; gi < dataGridView3.RowCount; gi++)
                {

                    cve_u = dataGridView3[2, gi].Value.ToString();
                    num_a = dataGridView3[7, gi].Value.ToString();

                    sql_ofis = "SELECT RCMS_SAL_DIA_INT " +
                               "FROM " + nom_tabla_mo + " WHERE RCMS_CVE_UNIT = \"" + cve_u + "\" AND RCMS_NUM_AFIL =\""+num_a+"\" ORDER BY RCMS_NUM_AFIL DESC";

                    dataAdapter_mov = new OleDbDataAdapter(sql_ofis, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet_mov = new DataSet(); // creamos la instancia del objeto DataSet
                    dataAdapter_mov.Fill(dataSet_mov, nom_tabla_mo);//llenamos el dataset

                    tabla_mov = dataSet_mov.Tables[0]; //le asignamos al DataGridView el contenido del dataSet

                    
                   
                    if (tabla_mov.Rows.Count > 0)
                    {
                        dataGridView3[17, gi].Value = tabla_mov.Rows[0][0].ToString();
                        //MessageBox.Show("clave: " + cve_u + ", num_afil: " + num_a + ", total_tabla_mov: " + tabla_mov.Rows.Count.ToString() + ", salario: " + tabla_mov.Rows[0][0].ToString());                        
                    }

                    Invoke(new MethodInvoker(delegate
                    {
                        label2.Text = "Extrayendo Salarios de Asegurados... " + (gi + 1) + " de " + dataGridView3.RowCount;
                        label2.Refresh();
                    }));
             
                }

                //EXTRAER SALARIO FIN -----------------------------------------------------

        		//MessageBox.Show(""+dataGridView3.Rows.Count);
        		conexion.Close();//cerramos la conexion
        		
        		conex.conectar("base_principal");
        		i=0;
                Invoke(new MethodInvoker(delegate
                        {
                            tot_row = dataGridView3.RowCount;
                            label1.Text = "40%";
                            label1.Refresh();
                            progressBar1.Value = 40;
                            progressBar1.Refresh();
                        }));

        		do{//*************************************
                    Invoke(new MethodInvoker(delegate
                    {
                        label2.Text = "Guardando la Informacion de los Asegurados en la B.D... " + (i + 1) + " de " + tot_row;
                        label2.Refresh();
                        barra_progreso2();
                        this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";

                        fecha = dataGridView3.Rows[i].Cells[6].FormattedValue.ToString();
                        fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);

                        nombre = dataGridView3.Rows[i].Cells[10].FormattedValue.ToString();
                        
                        quita_non=dataGridView3.Rows[i].Cells[4].FormattedValue.ToString();
                        
                        if(quita_non.Length>5){
                        	quita_non=quita_non.Substring(4,2);
                        	
                        	switch(quita_non){
                        			
                        		case "01": quita_non="02";
                        				 break;
                        		case "03": quita_non="04";
                        				 break;
                        		case "05": quita_non="06";
                        				 break;
                        		case "07": quita_non="08";
                        				 break;
                        		case "09": quita_non="10";
                        				 break;
                        		case "11": quita_non="12";
                        				 break;
                        				default: break;
                        	}
                        	
                        	dataGridView3.Rows[i].Cells[4].Value=dataGridView3.Rows[i].Cells[4].FormattedValue.ToString().Substring(0,4)+quita_non;
                        	
                        }
                        
                    }));
                        nombre = nombre.Replace('$', ' ');
                        nombre = nombre.Replace('/', 'Ñ');
                        nombre = nombre.Replace('#', 'Ñ');
                        nombre = nombre.Replace('?', 'Ñ');
                        nombre = nombre.TrimEnd(' ');

                        if (Convert.ToInt32(periodo.Substring(2, 4) + periodo.Substring(0, 2)) <= 200602)
                        {
                            Invoke(new MethodInvoker(delegate
                            {
                            imp_tot = Convert.ToDecimal(dataGridView3.Rows[i].Cells[12].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[13].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[14].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[15].FormattedValue.ToString());
							 }));
                        	Invoke(new MethodInvoker(delegate
                            {
                            conex.consultar("INSERT INTO mod40_sua (del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal," +
                                "periodo_pago,folio_sua,fecha_pago,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,importe_pago,imp_ret,salario_diario) " +
                                            "VALUES(\"" + dataGridView3.Rows[i].Cells[0].FormattedValue.ToString() + "\"," +//del
                                            "\"" + dataGridView3.Rows[i].Cells[1].FormattedValue.ToString() + "\"," +  //sub
                                            "\"" + dataGridView3.Rows[i].Cells[2].FormattedValue.ToString() + "\"," +//cve_unit
                                            //"\"" + nrp + "\"," +														//NRP
                                            "\"" + dataGridView3.Rows[i].Cells[16].FormattedValue.ToString() + "\"," +
                                            "\"" + dataGridView3.Rows[i].Cells[7].FormattedValue.ToString().Substring(1) + "\"," +//nss
                                            "\"" + dataGridView3.Rows[i].Cells[8].FormattedValue.ToString() + "\"," +//rfc
                                            "\"" + dataGridView3.Rows[i].Cells[9].FormattedValue.ToString() + "\"," +//curp
                                            "\"" + nombre + "\"," +//nombre
                                            "" + dataGridView3.Rows[i].Cells[11].FormattedValue.ToString() + "," +//dias_cot
                                            "\"" + dataGridView3.Rows[i].Cells[3].FormattedValue.ToString() + "\"," +//pza_suc
                                            "\"" + dataGridView3.Rows[i].Cells[4].FormattedValue.ToString() + "\"," +//periodo_pago
                                            "\"" + dataGridView3.Rows[i].Cells[5].FormattedValue.ToString() + "\"," +//folio_sua
                                            "\"" + fecha + "\"," +//fecha_pago
                                            "" + dataGridView3.Rows[i].Cells[13].FormattedValue.ToString() + "," +//eym_pree
                                            "" + dataGridView3.Rows[i].Cells[14].FormattedValue.ToString() + "," +//eym_vida
                                            "" + dataGridView3.Rows[i].Cells[15].FormattedValue.ToString() + "," +//eym_cesa_vej
                                            "" + "0.00" + "," +//eym_pree_o
                                            "" + "0.00" + "," +//eym_vida_o
                                            "" + "0.00" + "," +//eym_cesa_vej_o
                                            "" + periodo_nor + "," +//per_cd_sua
                                            "\"40\"," +//tipo_modalidad
                                            "" + imp_tot + "," +//importe pago
                                            "" + dataGridView3.Rows[i].Cells[12].FormattedValue.ToString() + "," +//importe RET
                                            "" + dataGridView3.Rows[i].Cells[17].FormattedValue.ToString() + "" +//sal_diario
                                            " )");
                            }));

                        }
                        else
                        {
                            Invoke(new MethodInvoker(delegate
                            {
                            imp_tot = Convert.ToDecimal(dataGridView3.Rows[i].Cells[12].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[13].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[14].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[15].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[18].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[19].FormattedValue.ToString()) +
                                Convert.ToDecimal(dataGridView3.Rows[i].Cells[20].FormattedValue.ToString());
							}));
                        	Invoke(new MethodInvoker(delegate
                            {
                            conex.consultar("INSERT INTO mod40_sua (del,sub,cve_unit,registro_patronal,nss,rfc,curp,nombre_trabajador,dias_cotizados,pza_sucursal,periodo_pago," +
                                "folio_sua,fecha_pago,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,importe_pago,imp_ret,salario_diario) " +
                                            "VALUES(\"" + dataGridView3.Rows[i].Cells[0].FormattedValue.ToString() + "\"," +//del
                                            "\"" + dataGridView3.Rows[i].Cells[1].FormattedValue.ToString() + "\"," +  //sub
                                            "\"" + dataGridView3.Rows[i].Cells[2].FormattedValue.ToString() + "\"," +//cve_unit
                                            "\"" + dataGridView3.Rows[i].Cells[16].FormattedValue.ToString() + "\"," +														//NRP
                                            "\"" + dataGridView3.Rows[i].Cells[7].FormattedValue.ToString().Substring(1) + "\"," +//nss
                                            "\"" + dataGridView3.Rows[i].Cells[8].FormattedValue.ToString() + "\"," +//rfc
                                            "\"" + dataGridView3.Rows[i].Cells[9].FormattedValue.ToString() + "\"," +//curp
                                            "\"" + nombre + "\"," +//nombre
                                            "" + dataGridView3.Rows[i].Cells[11].FormattedValue.ToString() + "," +//dias_cot
                                            "\"" + dataGridView3.Rows[i].Cells[3].FormattedValue.ToString() + "\"," +//pza_suc
                                            "\"" + dataGridView3.Rows[i].Cells[4].FormattedValue.ToString() + "\"," +//periodo_pago
                                            "\"" + dataGridView3.Rows[i].Cells[5].FormattedValue.ToString() + "\"," +//folio_sua
                                            "\"" + fecha + "\"," +//fecha_pago
                                            "" + dataGridView3.Rows[i].Cells[13].FormattedValue.ToString() + "," +//eym_pree
                                            "" + dataGridView3.Rows[i].Cells[14].FormattedValue.ToString() + "," +//eym_vida
                                            "" + dataGridView3.Rows[i].Cells[15].FormattedValue.ToString() + "," +//eym_cesa_vej
                                            "" + dataGridView3.Rows[i].Cells[18].FormattedValue.ToString() + "," +//eym_pree_o
                                            "" + dataGridView3.Rows[i].Cells[19].FormattedValue.ToString() + "," +//eym_vida_o
                                            "" + dataGridView3.Rows[i].Cells[20].FormattedValue.ToString() + "," +//eym_cesa_vej_o
                                            "" + periodo_nor + "," +//per_cd_sua
                                            "\"40\"," +//tipo_modalidad
                                            "" + imp_tot + "," +//importe pago
                                            "" + dataGridView3.Rows[i].Cells[12].FormattedValue.ToString() + "," +//importe RET
                                            "" + dataGridView3.Rows[i].Cells[17].FormattedValue.ToString() + "" +//sal_diario
                                            " )");
                                }));
                        }

                    
        			i++;
        		}while(i<tot_row);
                Invoke(new MethodInvoker(delegate
                    {
                        label1.Text = "100%";
                        label1.Refresh();
                        progressBar1.Value = 100;
                        progressBar1.Refresh();
                        this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                    }));
                MessageBox.Show("La Importación de Asegurados en Modalidad 40 ha terminado Exitosamente.\nSe Importaron los datos de: "+i+" Asegurados.","¡EXITO!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                conex.guardar_evento("Se Ingresaron datos de "+i+" asegurados  el CD SUA del periodo:"+periodo_nor);
                //this.Close();
        	//}
        	//catch (Exception ex)
        	//{
        		//en caso de haber una excepcion que nos mande un mensaje de error
        	//	MessageBox.Show("Error, Verificar el archivo o el nombre de la tabla\n" + ex, "Error al Abrir Archivo de Access");
        	//}
        }

        public void cargar_txt(String archivop,String nrpp)
        {
            this.archivo = archivop;
            this.nrp_text = nrpp;
        }

        public void proceso_carga_txt()
        {
        	int anio = 0, mes = 0, cont=0, j=0, anio1=0,mes1=0;
        	String linea1="",linea2="",per_count1,concat_per,concat_nss;

        	//Lectura Previa del archivo para conocer su longitud
        	StreamReader t3 = new StreamReader(archivo);
        	textBox1.Text = t3.ReadToEnd();
        	aux = textBox1.Lines;
        	tot1 = aux.Length;
        	t3.Close();
        	textBox1.Text = "";
            k = 0;
        	//Comienza Lectura
        	i = 0;
        	Invoke(new MethodInvoker(delegate
        	                         {
        	                         	dataGridView1.DataSource = null;
        	                         	dataGridView1.Columns.Add("NSS", "NSS");
        	                         }));
        	StreamReader t4 = new StreamReader(archivo);
        	 //try
        	// {
        	Invoke(new MethodInvoker(delegate
        	                         {
        	                         	label2.Text = "Abriendo archivo...";
        	                         	label2.Refresh();
        	                         	label1.Text = "0%";
        	                         	label1.Refresh();
        	                         	progressBar1.Value = 0;
        	                         	progressBar1.Refresh();
        	                         }));

        	//Lectura linea por linea
        	while (!t4.EndOfStream)
        	{
        		Invoke(new MethodInvoker(delegate
        		                         {
        		                         	label2.Text = "Leyendo archivo...";
        		                         	label2.Refresh();
        		                         	barra_progreso_txt1();
        		                         	this.Text="Cargar Archivo/CD SUA "+progressBar1.Value+"%";
        		                         }));
        		linea = t4.ReadLine();
        		if (linea.Length > 115)
        		{
        			if (linea.Substring(97, 5).Equals("CICLO"))
        			{
        				ciclo = linea.Substring(105, 7);
        				ciclo = ciclo.Substring(3, 4) + ciclo.Substring(0, 2);
        				//MessageBox.Show(ciclo);
        			}

                    for (int x = 0; x < tablanrps.Rows.Count; x++)
                    {
                      //if (linea.Substring(2, 14).Equals(nrp_text))
                        //MessageBox.Show(tablanrps.Rows[x][0].ToString());
                        if (linea.Substring(2, 14).Equals(tablanrps.Rows[x][0].ToString()))
                        {
                            nss = linea.Substring(19, 15);
                            nss = nss.Substring(0, 2) + nss.Substring(3, 2) + nss.Substring(6, 2) + nss.Substring(9, 4) + nss.Substring(14, 1);
                            Invoke(new MethodInvoker(delegate
                                                     {
                                                         dataGridView1.Rows.Add(nss);
                                                     }));

                            tablanrp_guarda.Rows.Add(tablanrps.Rows[x][0].ToString());
                        }
                    }

        		}
        		i++;
        	}//fin lectura

            //MessageBox.Show(ciclo);
        	//*********Proceso****************************************************
        	
        	conex.conectar("base_principal");
        	Invoke(new MethodInvoker(delegate
        	                         {
        	                         	tot_row=dataGridView1.RowCount;
        	                         	dataGridView3.Columns.Add("REG_PAT", "REG_PAT");
        	                         	dataGridView3.Columns.Add("NSS", "NSS");
        	                         	dataGridView3.Columns.Add("NOMBRE", "NOMBRE");
        	                         	dataGridView3.Columns.Add("PERIODO", "PERIODO");
        	                         	dataGridView3.Columns.Add("PRIMER PERIODO", "PRIMER PERIODO");
        	                         	
        	                         	label2.Text = "Obteniendo Periodos sin Pago...";
        	                         	label2.Refresh();
        	                         	label1.Text = "10%";
        	                         	label1.Refresh();
        	                         	progressBar1.Value = 10;
        	                         	progressBar1.Refresh();
        	                         	this.Text="Cargar Archivo/CD SUA "+progressBar1.Value+"%";
        	                         }));
        	i = 0;
        	j = 0;
        	band1 = 0;
        	/***-----------------*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*PROCESO*/
            while (i < tot_row){
        		Invoke(new MethodInvoker(delegate
        		                         {
        		                         	dataGridView2.DataSource = null;
        		                         	nss = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                                            nrp_text = tablanrp_guarda.Rows[i][0].ToString();
                                            //MessageBox.Show(ciclo + "-" + calcula_periodo_min(ciclo)+" nss: "+nss);
                                            dataGridView2.DataSource = conex.consultar("SELECT nombre_trabajador,periodo_pago FROM mod40_sua WHERE nss=\"" + nss + "\" AND  (periodo_pago BETWEEN \"" + calcula_periodo_min(ciclo) + "\" AND  \"" + ciclo + "\") ORDER BY periodo_pago ASC");///////esta es la linea a modiifcar
        		                         	tot1=dataGridView2.RowCount;
                                            //MessageBox.Show(ciclo + "-" + calcula_periodo_min(ciclo) + " coincidencias en la bd: " + tot1);
        		                         	j=0;
        		                         }));
        		
        		
        		if (tot1 > 1)
        		{
        			Invoke(new MethodInvoker(delegate
        			                         {
        			                         	periodo = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();
        			                         	nombre = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();//nombre_trabajador
        			                         	primer_per=periodo;
        			                         }));

        			anio = Convert.ToInt32(periodo.Substring(0, 4));
        			mes = Convert.ToInt32(periodo.Substring(4, 2));
        			
        			if(mes < 10){
        				mes_cad = "0" + mes;
        			}else{
        				mes_cad = mes.ToString();
        			}
        			per_count = anio.ToString() + mes_cad;
        			
        			mes1 = mes;
        			anio1 = anio;
        			mes1=mes1+2;
        			
        			if (mes1 > 12)
        			{
        				mes1 = 2;
        				anio1++;
        			}
        			
        			if(mes1 < 10){
        				mes_cad = "0" + mes1;
        			}else{
        				mes_cad = mes1.ToString();
        			}
        			
        			per_count1=anio1.ToString()+mes_cad;
        			periodo_txt=per_count;
        			int linea2_ciclo=0;
        			//MessageBox.Show(per_count+"|"+per_count1);
        			
        			do{//****-----------------------------------------------comparar periodos
        				
        				if((j+1)<tot1){
        					Invoke(new MethodInvoker(delegate{
        					                         	linea1 = dataGridView2.Rows[j].Cells[1].FormattedValue.ToString();
        					                         	linea2 = dataGridView2.Rows[j+1].Cells[1].FormattedValue.ToString();
        					                         }));
        				}else{
        					linea1 = dataGridView2.Rows[j].Cells[1].FormattedValue.ToString();
        					linea2 = ciclo;
        					linea2_ciclo=1;
        				}
        				
        				//checar si va adelantado
        				if(Convert.ToInt32(per_count) > Convert.ToInt32(linea1)){
        					per_count=linea1;
        					
        					mes=Convert.ToInt32(per_count.Substring(4,2));
        					anio=Convert.ToInt32(per_count.Substring(0,4));
        					
        					mes1 = mes;
        					anio1 = anio;
        					
        					mes1=mes1+2;
        					if (mes1 > 12)
        					{
        						mes1 = 2;
        						anio1++;
        					}

        					//MessageBox.Show(linea2 + " l2|" + per_count+" pc| ci "+ciclo+"|pc2 "+per_count1);
        				}
        				
        				if(linea1.Equals(linea2)){//*************************SI son PAR
        					
        					//MessageBox.Show(linea1+"|"+per_count+"|"+per_count1);
        					
        					if(per_count.Equals(linea1)){
        						periodo_txt=linea2;
        						
        					}else{
        						mes=Convert.ToInt32(per_count.Substring(4,2));
        						anio=Convert.ToInt32(per_count.Substring(0,4));
        						
        						if(mes < 10){
        							mes_cad = "0" + mes;
        						}else{
        							mes_cad = mes.ToString();
        						}
        						
        						per_count=anio.ToString()+mes_cad;
        						
        						while(Convert.ToInt32(per_count)<Convert.ToInt32(linea1)){//*****----periodos faltantes
        							
        							Invoke(new MethodInvoker(delegate{
        							                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        							                         }));
        							
        							mes=mes+2;
        							if (mes > 12)
        							{
        								mes = 2;
        								anio++;
        							}
        							
        							if(mes < 10){
        								mes_cad = "0" + mes;
        							}else{
        								mes_cad = mes.ToString();
        							}
        							
        							per_count=anio.ToString()+mes_cad;
        							
        							mes1 = mes;
        							anio1 = anio;
        							
        							mes1=mes1+2;
        							
        							if (mes1 > 12)
        							{
        								mes1 = 2;
        								anio1++;
        							}
        							
        							if(mes1 < 10){
        								mes_cad = "0" + mes1;
        							}else{
        								mes_cad = mes1.ToString();
        							}
        							
        							per_count1=anio1.ToString()+mes_cad;
        						}
        					}
        					
        					j=j+2;
        					
        				}else{//******************SI SON IMPAR
        					
        					if(linea1.Equals(per_count)&&linea2.Equals(per_count1)){
        						//if(j>0){
        							Invoke(new MethodInvoker(delegate{
        							                         	dataGridView3.Rows.Add(nrp_text, nss, nombre,linea1,primer_per);
        							                         }));
        						//}
        						
        					}else{
        						
        						if(!linea1.Equals(per_count)){//inicio******hueco1
        							
        							mes=Convert.ToInt32(per_count.Substring(4,2));
        							anio=Convert.ToInt32(per_count.Substring(0,4));
        							
        							Invoke(new MethodInvoker(delegate{
        							                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        							                         }));
        							
        							while(Convert.ToInt32(per_count)<Convert.ToInt32(linea1)){//*****-------------periodos faltantes
        								
        								mes=mes+2;
	        							if (mes > 12)
	        							{
	        								mes = 2;
	        								anio++;
	        							}
	        							
	        							if(mes < 10){
	        								mes_cad = "0" + mes;
	        							}else{
	        								mes_cad = mes.ToString();
	        							}
	        							per_count = anio.ToString() + mes_cad;
        								
        								Invoke(new MethodInvoker(delegate{
        								                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        								                         }));
        								
        								mes1 = mes;
        								anio1 = anio;
        								
        								mes1=mes1+2;
        								if (mes1 > 12)
        								{
        									mes1 = 2;
        									anio1++;
        								}
        								if(mes1 < 10){
        									mes_cad = "0" + mes1;
        								}else{
        									mes_cad = mes1.ToString();
        								}
        								
        								per_count1=anio1.ToString()+mes_cad;
        								
        							}
        							
        							Invoke(new MethodInvoker(delegate{
        							                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        							                         }));
        							
        						}//fin hueco h1
        						
        						
        						if(!linea2.Equals(per_count1)){//inicio hueco h2
        							
        							mes=Convert.ToInt32(per_count.Substring(4,2));
        							anio=Convert.ToInt32(per_count.Substring(0,4));
        							
        							if (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
                                	{
        							Invoke(new MethodInvoker(delegate{
        							                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        							                         }));
        							}
        							
        							mes=mes+2;
        							if (mes > 12)
        							{
        								mes = 2;
        								anio++;
        							}
        							
        							if(mes < 10){
        								mes_cad = "0" + mes;
        							}else{
        								mes_cad = mes.ToString();
        							}
        							per_count = anio.ToString() + mes_cad;
        							
        							while(Convert.ToInt32(per_count)< Convert.ToInt32(linea2)){//*****-------------periodos faltantes
        								if(mes < 10){
        									mes_cad = "0" + mes;
        								}else{
        									mes_cad = mes.ToString();
        								}
        								per_count = anio.ToString() + mes_cad;
        								if (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
                                    	{
        									Invoke(new MethodInvoker(delegate{
        								                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        								                         }));
        								}
        								mes=mes+2;
        								if (mes > 12)
        								{
        									mes = 2;
        									anio++;
        								}
        								if(mes < 10){
        									mes_cad = "0" + mes;
        								}else{
        									mes_cad = mes.ToString();
        								}
        								
        								per_count = anio.ToString() + mes_cad;
        								
        								mes1 = mes;
        								anio1 = anio;
        								
        								mes1=mes1+2;
        								if (mes1 > 12)
        								{
        									mes1 = 2;
        									anio1++;
        								}
        								if(mes1 < 10){
        									mes_cad = "0" + mes1;
        								}else{
        									mes_cad = mes1.ToString();
        								}
        								
        								per_count1=anio1.ToString()+mes_cad;
        								
        							}
        							 if (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
                                	{
        								Invoke(new MethodInvoker(delegate{
        							                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        							                         }));
        							 }
        						}
        						
        					}
        					j++;
        					periodo_txt=linea2;
        				}//****************FIN SINO
        				
        				
        				mes=mes+2;
        				if (mes > 12)
        				{
        					mes = 2;
        					anio++;
        				}
        				
        				if(mes < 10){
        					mes_cad = "0" + mes;
        				}else{
        					mes_cad = mes.ToString();
        				}
        				per_count = anio.ToString() + mes_cad;
        				
        				mes1=mes1+2;
        				if (mes1 > 12)
        				{
        					mes1 = 2;
        					anio1++;
        				}
        				if(mes1 < 10){
        					mes_cad = "0" + mes1;
        				}else{
        					mes_cad = mes1.ToString();
        				}
        				
        				per_count1=anio1.ToString()+mes_cad;
        				
        			}while ((j<tot1)&&(Convert.ToInt32(per_count)<Convert.ToInt32(ciclo)));
        			//FIN CICLO PROCESO MULTIPLES RESULTADOS****************************************************
					
        			if(linea2_ciclo==1){
        				linea2=linea1;
        			}
        			linea2_ciclo=0;

        			if (Convert.ToInt32(linea2) < Convert.ToInt32(ciclo))
        			{
                        //checar si va adelantado
                        if (Convert.ToInt32(per_count) > Convert.ToInt32(linea2))
                        {
                            per_count = linea2;

                            mes = Convert.ToInt32(per_count.Substring(4, 2));
                            anio = Convert.ToInt32(per_count.Substring(0, 4));
                        }

        				while (Convert.ToInt32(per_count) < Convert.ToInt32(ciclo))
        				{
        					//*****-------------periodos faltantes
        					mes = mes + 2;
        					if (mes > 12)
        					{
        						mes = 2;
        						anio++;
        					}
        					if (mes < 10)
        					{
        						mes_cad = "0" + mes;
        					}
        					else
        					{
        						mes_cad = mes.ToString();
        					}

        					per_count = anio.ToString() + mes_cad;

        					Invoke(new MethodInvoker(delegate
        					                         {
        					                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        					                         }));
        				}
        			}

        		}else{
        			if(tot1==1){//UN SOLO RESULTADO
        				Invoke(new MethodInvoker(delegate{
        				                         	linea1 = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();
        				                         	nombre = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();//nombre_trabajador
        				                         }));

        				anio = Convert.ToInt32(linea1.Substring(0, 4));
        				mes = Convert.ToInt32(linea1.Substring(4, 2));
        				
        				if(mes < 10){
        					mes_cad = "0" + mes;
        				}else{
        					mes_cad = mes.ToString();
        				}
        				per_count = anio.ToString() + mes_cad;
        				
        				if(!linea1.Equals(ciclo)){
        					
        					do{//*****-------------periodos faltantes
        						if(mes < 10){
        							mes_cad = "0" + mes;
        						}else{
        							mes_cad = mes.ToString();
        						}
        						per_count = anio.ToString() + mes_cad;
        						
        						Invoke(new MethodInvoker(delegate{
        						                         	dataGridView3.Rows.Add(nrp_text, nss, nombre, per_count,primer_per);
        						                         }));
        						
        						mes=mes+2;
        						if (mes > 12)
        						{
        							mes = 2;
        							anio++;
        						}
        						if(mes < 10){
        							mes_cad = "0" + mes;
        						}else{
        							mes_cad = mes.ToString();
        						}
        						
        						per_count = anio.ToString() + mes_cad;

        					}while(Convert.ToInt32(per_count)<=Convert.ToInt32(ciclo));

        				}
        			}
        		}

        		Invoke(new MethodInvoker(delegate
        		                         {
                                            label2.Text = "Obteniendo Periodos sin Pago... " + i + " de " + tot_row;
        		                         	label2.Refresh();
        		                         	barra_progreso_txt2();
        		                         	this.Text="Cargar Archivo/CD SUA "+progressBar1.Value+"%";
        		                         }));
        		i++;
            }//fin proceso 1**************--*-*-*-*-*-*-*-**--**
			
        	tot_rowsdgv3=0;
            tabla_excel.Columns.Add();
            tabla_excel.Columns.Add();
            tabla_excel.Columns.Add();
            tabla_excel.Columns.Add();
            tabla_excel.Columns.Add();
            
            int cont_tabl = 0;

        	Invoke(new MethodInvoker(delegate{

                //label2.Text = "Obteniendo Periodos sin Pago... " + (i+1) + " de " + tot_row;
                tot_rowsdgv3=dataGridView3.Rows.Count;
                //MessageBox.Show("Llegó aqui : antes del while de formato de celdas y tot_rowsdgv3= "+tot_rowsdgv3);
                while (cont_tabl < tot_rowsdgv3)
                {
                    tabla_excel.Rows.Add(dataGridView3.Rows[cont_tabl].Cells[0].FormattedValue.ToString(),
                                         dataGridView3.Rows[cont_tabl].Cells[1].FormattedValue.ToString(),
                                         dataGridView3.Rows[cont_tabl].Cells[2].FormattedValue.ToString(),
                                         dataGridView3.Rows[cont_tabl].Cells[3].FormattedValue.ToString(),
                                         dataGridView3.Rows[cont_tabl].Cells[4].FormattedValue.ToString());
                    cont_tabl++;
                }
                //MessageBox.Show("tot_tabl: " + tabla_excel.Rows.Count + " 1er: " + tabla_excel.Rows[k][3].ToString());
                dataGridView3.Rows.Clear();
                tot_rowsdgv3 = tabla_excel.Rows.Count;
        	                         }));

            //MessageBox.Show("Llegó aqui : antes del if Arreglar periodos dobles, tot_rowsdgv3= " + tot_rowsdgv3);
            if (tot_rowsdgv3 > 0)
            {
                //MessageBox.Show("Llegó aqui : Arreglar periodos dobles (entró al if ");
                //ARREGLAR PERIODOS DOBLES
                k = 0;
                DataTable caja_lista = new DataTable();
                DataTable consulta_completa = new DataTable();
                DataView vista = new DataView(consulta_completa);

                if (tabla_baja_1.Columns.Count <= 0)
                {
                    tabla_baja.Columns.Add();
                    tabla_baja.Columns.Add();
                    tabla_baja.Columns.Add();
                    tabla_baja.Columns.Add();

                    tabla_baja_1.Columns.Add();
                    tabla_baja_1.Columns.Add();
                    tabla_baja_1.Columns.Add();
                    tabla_baja_1.Columns.Add();

                    conex.conectar("base_principal");
                    concat_nss = "-";
                    concat_per = "";
                    Invoke(new MethodInvoker(delegate
                                             {
                                                 dataGridView1.DataSource = null;
                                                 dataGridView1.Columns.Clear();

                                             }));
                    String sql, anterior, actual = "", actual_nss = "", periodo_bd = "", anterior_nss = "";
                    anterior = "";
                    int cont_caja = 0, res_repe = 0, tot_filas = 0, guardar_en_grid = 0;


                    do
                    {
                        cont_caja = 0;
                        Invoke(new MethodInvoker(delegate
                                             {

                                                 actual = tabla_excel.Rows[k][3].ToString();
                                                 actual_nss = tabla_excel.Rows[k][1].ToString();
                                                 primer_per = tabla_excel.Rows[k][4].ToString();

                                                 //actual = dataGridView3.Rows[k].Cells[3].FormattedValue.ToString();
                                                 //actual_nss = dataGridView3.Rows[k].Cells[1].FormattedValue.ToString();
                                                 //primer_per = dataGridView3.Rows[k].Cells[4].FormattedValue.ToString();

                                                 label2.Text = "Analizando Datos Asegurados... " + k + " de " + tot_rowsdgv3;
                                                 label2.Refresh();
                                                 barra_progreso_txt3();
                                                 this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                                             }));

                        if (!actual_nss.Equals(anterior_nss))
                        {
                            sql = "SELECT fecha_pago,periodo_pago FROM base_principal.mod40_sua WHERE nss=\"" + actual_nss + "\"";
                            consulta_completa = conex.consultar(sql);
                            vista.Table = consulta_completa;
                            anterior_nss = actual_nss;
                        }

                        do
                        {
                            if (caja_lista.Rows.Count > 0)
                            {
                                //MessageBox.Show(caja_lista.Rows[cont_caja][0].ToString());
                                if (actual.Equals(caja_lista.Rows[cont_caja][0].ToString()))
                                {
                                    //MessageBox.Show(caja_lista.Rows[cont_caja][0].ToString());
                                    res_repe = 1;
                                }
                            }
                            cont_caja++;
                        } while (cont_caja < caja_lista.Rows.Count);

                        if (res_repe == 0)
                        {
                            if (!actual.Equals(primer_per))
                            {

                                Invoke(new MethodInvoker(delegate
                                {
                                    vista.RowFilter = "periodo_pago='" + actual + "'";
                                    vista.Sort = "fecha_pago ASC";
                                    dataGridView1.DataSource = vista;
                                    tot_filas = dataGridView1.Rows.Count;
                                }));
                                //MessageBox.Show(sql);

                                if (tot_filas == 0)
                                {
                                    concat_per = actual;
                                    guardar_en_grid = 2;
                                    //concat_per = (Convert.ToInt32(actual) - 1).ToString() + "," + actual.ToString();
                                }
                                else
                                {
                                    Invoke(new MethodInvoker(delegate
                                    {
                                        periodo_bd = dataGridView1.Rows[0].Cells[0].FormattedValue.ToString();
                                    }));

                                    if (tot_filas == 1)
                                    {

                                        if (periodo_bd.Length > 6)
                                        {
                                            //periodo - fecha pago
                                            periodo_bd = periodo_bd.Substring(6, 4) + periodo_bd.Substring(3, 2);
                                            concat_per = actual;

                                            if (!periodo_bd.Equals(concat_per))
                                            {
                                                if (periodo_bd.Equals((Convert.ToInt32(concat_per) - 1).ToString()))
                                                {
                                                    guardar_en_grid = 1;
                                                }
                                                else
                                                {
                                                    guardar_en_grid = 3;
                                                }
                                            }
                                            else
                                            {
                                                guardar_en_grid = 3;
                                            }
                                        }
                                        else
                                        {
                                            guardar_en_grid = 2;
                                        }
                                    }
                                    else
                                    {
                                        if (tot_filas >= 2)
                                        {
                                            concat_per = actual;
                                        }
                                    }
                                }
                            }
                            anterior = actual;

                            Invoke(new MethodInvoker(delegate
                            {
                                if (guardar_en_grid == 1)
                                {
                                    dataGridView3.Rows.Add(tabla_excel.Rows[k][0].ToString(),
                                                           tabla_excel.Rows[k][1].ToString(),
                                                           tabla_excel.Rows[k][2].ToString(),
                                                           concat_per,
                                                           tabla_excel.Rows[k][4].ToString());
                                }
                                else
                                {
                                    if (guardar_en_grid == 2)
                                    {
                                        dataGridView3.Rows.Add(tabla_excel.Rows[k][0].ToString(),
                                                           tabla_excel.Rows[k][1].ToString(),
                                                           tabla_excel.Rows[k][2].ToString(),
                                                           (Convert.ToInt32(concat_per) - 1),
                                                           tabla_excel.Rows[k][4].ToString());

                                        dataGridView3.Rows.Add(tabla_excel.Rows[k][0].ToString(),
                                                           tabla_excel.Rows[k][1].ToString(),
                                                           tabla_excel.Rows[k][2].ToString(),
                                                           concat_per,
                                                           tabla_excel.Rows[k][4].ToString());
                                    }
                                    else
                                    {
                                        if (guardar_en_grid == 3)
                                        {
                                            dataGridView3.Rows.Add(tabla_excel.Rows[k][0].ToString(),
                                                               tabla_excel.Rows[k][1].ToString(),
                                                               tabla_excel.Rows[k][2].ToString(),
                                                               (Convert.ToInt32(concat_per) - 1),
                                                               tabla_excel.Rows[k][4].ToString());
                                        }
                                    }
                                }
                                guardar_en_grid = 0;
                            }));
                        }
                        res_repe = 0;
                        k++;
                    } while (k < tot_rowsdgv3);
                    //FIN PERIODOS DOBLES

                    tabla_excel1.Columns.Add("REG_PAT");
                    tabla_excel1.Columns.Add("NSS");
                    tabla_excel1.Columns.Add("NOMBRE");
                    tabla_excel1.Columns.Add("PERIODO");
                    tabla_excel1.Columns.Add("PERIODO_INICIAL");

                    cont_tabl = 0;


                    Invoke(new MethodInvoker(delegate
                    {
                        while (cont_tabl < dataGridView3.Rows.Count)
                        {
                            tabla_excel1.Rows.Add(dataGridView3.Rows[cont_tabl].Cells[0].FormattedValue.ToString(),
                                             dataGridView3.Rows[cont_tabl].Cells[1].FormattedValue.ToString(),
                                             dataGridView3.Rows[cont_tabl].Cells[2].FormattedValue.ToString(),
                                             dataGridView3.Rows[cont_tabl].Cells[3].FormattedValue.ToString(),
                                             dataGridView3.Rows[cont_tabl].Cells[4].FormattedValue.ToString());
                            cont_tabl++;
                        }
                    }));

                    try
                    {
                        System.IO.File.Delete(@"mod40/temporal_mod40.xlsx");
                    }
                    catch (Exception es0)
                    {
                    }

                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        wb.Worksheets.Add(tabla_excel1, "hoja_lz");
                        wb.SaveAs(@"mod40/temporal_mod40.xlsx");

                    }
                    catch (Exception es)
                    {

                    }

                    //CONSULTA A LA TABLA DE EXCEL
                    periodo_bd = "";
                    guardar_en_grid = 0;
                    //MessageBox.Show(""+ciclo+"|"+((Convert.ToInt32(ciclo))%2).ToString());
                    if (((Convert.ToInt32(ciclo)) % 2) != 0)
                    {//periodo de factura non
                        anio = Convert.ToInt32(ciclo.Substring(0, 4));
                        mes = Convert.ToInt32(ciclo.Substring(4, 2));

                        mes = mes + 1;

                        if (mes > 12)
                        {
                            mes = 2;
                            anio++;
                        }

                        if (mes < 10)
                        {
                            mes_cad = "0" + mes;
                        }
                        else
                        {
                            mes_cad = mes.ToString();
                        }

                        periodo_bd = anio.ToString() + mes_cad;

                        guardar_en_grid = 1;

                    }

                    //MessageBox.Show("periodo="+periodo_bd);

                    Invoke(new MethodInvoker(delegate
                    {
                        cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='mod40/temporal_mod40.xlsx';Extended Properties=Excel 12.0;";
                        conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                        conexion.Open();

                        carga_excel("Select distinct ([NSS]) from [hoja_lz$]");//carga en dgv1
                        tot_rowsdgv3 = dataGridView1.RowCount;
                        conexion.Close();

                        dataGridView3.Rows.Clear();
                        dataGridView3.DataSource = null;
                    }));

                    cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='mod40/temporal_mod40.xlsx';Extended Properties=Excel 12.0;";
                    conexion1 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
                    conexion1.Open();
                    k = 0;
                    tabla_baja.Rows.Clear();
                    tabla_excel1.Rows.Clear();


                    do
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            //progreso inicio
                            label2.Text = "Compactando Datos Asegurados... " + k + " de " + tot_rowsdgv3;
                            label2.Refresh();
                            barra_progreso_txt4();
                            this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                            //progreso fin                         	
                        }));

                        sql = "Select distinct([PERIODO]),[REG_PAT],[NSS],[NOMBRE],[PERIODO_INICIAL] from [hoja_lz$] where [NSS]=\"" + dataGridView1.Rows[k].Cells[0].FormattedValue.ToString() + "\" order by [PERIODO] asc";
                        dataAdapter1 = new OleDbDataAdapter(sql, conexion1); //traemos los datos de la hoja y las guardamos en un dataSdapter
                        dataSet1 = new DataSet();
                        dataAdapter1.Fill(dataSet1, "hoja_lz");//llenamos el dataset
                        //dataGridView3.DataSource = dataSet1.Tables[0]; //le asignamos al DataGridView el contenido del dataSet

                        tot_filas = 0;
                        concat_per = "";
                        //MessageBox.Show("dgv3_rc: "+dataSet1.Tables[0].Rows.Count);
                        //	MessageBox.Show("apunta: "+dataSet1.Tables[0].Rows[tot_filas][2].ToString());

                        do
                        {
                            if (guardar_en_grid == 1)
                            {
                                if (!dataSet1.Tables[0].Rows[tot_filas][0].ToString().Equals(periodo_bd))
                                {
                                    concat_per += dataSet1.Tables[0].Rows[tot_filas][0].ToString() + ",";
                                }

                            }
                            else
                            {
                                concat_per += dataSet1.Tables[0].Rows[tot_filas][0].ToString() + ",";
                            }
                            tot_filas++;
                        } while (tot_filas < dataSet1.Tables[0].Rows.Count);

                        concat_per = concat_per.TrimEnd(',');
                        tabla_baja.Rows.Add(dataSet1.Tables[0].Rows[0][2].ToString(),
                                            dataSet1.Tables[0].Rows[0][3].ToString(), concat_per);

                        k++;
                    } while (k < dataGridView1.RowCount);

                    Invoke(new MethodInvoker(delegate
                    {
                        dataGridView2.DataSource = tabla_baja;
                        tot_rowsdgv2 = dataGridView2.RowCount;
                    }));
                    //COMBINACION DE ASEGURADOS

                    /*k=0;
                    concat_per = "";
                    Invoke(new MethodInvoker(delegate
                    {
                        tot_rowsdgv3 = dataGridView3.RowCount;

                        do
                        {
                        	
                            //progreso inicio
                            label2.Text = "Compactando Datos Asegurados... " + k + " de " + tot_rowsdgv3;
                            label2.Refresh();
                            barra_progreso_txt4();
                            this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                            //progreso fin

                            if ((k + 1) < tot_rowsdgv3)
                            {
                                concat_nss = dataGridView3.Rows[k + 1].Cells[1].FormattedValue.ToString();
                            }
                            else
                            {
                                concat_nss = dataGridView3.Rows[k - 1].Cells[1].FormattedValue.ToString();
                            }

                            if (concat_nss.Equals(dataGridView3.Rows[k].Cells[1].FormattedValue.ToString()))
                            {
                                if (dataGridView3.Rows[k].Cells[3].FormattedValue.ToString().Length>5)
                                {
                                    if ((Convert.ToInt32(dataGridView3.Rows[k].Cells[3].FormattedValue.ToString()) <= Convert.ToInt32(ciclo)))
                                    {
                                        if (k == (dataGridView3.RowCount - 1))
                                        {
                                            concat_per += dataGridView3.Rows[k].Cells[3].FormattedValue.ToString();
                                            concat_per = concat_per.TrimEnd(',');
                                            tabla_baja.Rows.Add(dataGridView3.Rows[k].Cells[1].FormattedValue.ToString(),
                                                                dataGridView3.Rows[k].Cells[2].FormattedValue.ToString(), concat_per);
                                            concat_per = "";
                                        }else{
                                            if (!dataGridView3.Rows[k].Cells[3].FormattedValue.ToString().Equals(dataGridView3.Rows[k+1].Cells[3].FormattedValue.ToString()))
                                            {
                                                concat_per += dataGridView3.Rows[k].Cells[3].FormattedValue.ToString() + ",";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dataGridView3.Rows[k].Cells[3].FormattedValue.ToString().Length > 5)
                                {
                                    if ((Convert.ToInt32(dataGridView3.Rows[k].Cells[3].FormattedValue.ToString()) <= Convert.ToInt32(ciclo)))
                                    {
                                        concat_per += dataGridView3.Rows[k].Cells[3].FormattedValue.ToString() + ",";
                                    }
                                    
                                    concat_per = concat_per.TrimEnd(',');
                                    tabla_baja.Rows.Add(dataGridView3.Rows[k].Cells[1].FormattedValue.ToString(),
                                                        dataGridView3.Rows[k].Cells[2].FormattedValue.ToString(), concat_per);
                                    concat_per = "";
                                    //concat_per+=dataGridView3.Rows[k+1].Cells[3].FormattedValue.ToString()+",";

                                    if (k == dataGridView3.RowCount - 1)
                                    {
                                        concat_per += dataGridView3.Rows[k].Cells[3].FormattedValue.ToString();
                                        tabla_baja.Rows.Add(dataGridView3.Rows[k].Cells[1].FormattedValue.ToString(),
                                                            dataGridView3.Rows[k].Cells[2].FormattedValue.ToString(), concat_per);
                                        concat_per = "";
                                    }
                                }
                            }

                            k++;
                        } while (k < tot_rowsdgv3);

                        dataGridView2.DataSource = tabla_baja;
                        tot_rowsdgv2 = dataGridView2.RowCount;
                    }));*/

                    //FIN COMBINACION ASEGURADOS

                    //QUITAR DUPLICADOS INICIO
                    /*k=0;
                    concat_per = "";
                    tot_filas=0;
                    actual="";
                    anterior="";
                    res_repe=0;
                    cont_caja=0;
                   
                    while(k < tot_rowsdgv2){
                    
                    	anterior=dataGridView2.Rows[k].Cells[2].FormattedValue.ToString();
                    	aux = anterior.Split(',');
                    	tot_filas=0;
                    	
                    	while(tot_filas < aux.Length){
                    		actual=aux[tot_filas];
                    		cont_caja=0;
                    		res_repe=0;
                    		
                    		while(cont_caja<aux.Length){
                    			if(aux[cont_caja].Equals(actual)){
                    				res_repe++;
                    			}
                    			
                    			if(res_repe>1){
                    			aux[cont_caja]="";
                    			}
                    			
                    			cont_caja++;
                    	    }
                    		
                    		tot_filas++;
                    	}
                    	
                    	cont_caja=0;
                    	concat_per="";
                    	while(cont_caja < aux.Length){
                    		if(aux[cont_caja].Length>2){
                    			concat_per+=aux[cont_caja].ToString()+",";
                    		}
                    		cont_caja++;
                    	}
                    	concat_per = concat_per.TrimEnd(',');
                    	//MessageBox.Show(concat_per);
                    	
                    	dataGridView2.Rows[k].Cells[2].Value=concat_per;
                    	k++;
                    }
                    tabla_excel.Clear();
                    tabla_excel = (DataTable)(dataGridView2.DataSource);
                    //QUITAR DUPLICADOS FIN
                    */
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        wb.Worksheets.Add(tabla_baja, "hoja_lz");
                        wb.SaveAs(@"mod40/temporal_mod40_resultados_sin_recorte.xlsx");
                    }
                    catch (Exception es)
                    {

                    }

                    //RECORTE PERIODOS/SEPARACION DE MUCHOS Y UNO
                    Invoke(new MethodInvoker(delegate
                    {

                        tabla_excel.Rows.Clear();
                        tabla_baja_1.Rows.Clear();

                        tabla_excel.Columns.Add();
                        tabla_baja_1.Columns.Add();
                        tabla_baja_1.Columns.Add();
                        tabla_baja_1.Columns.Add();

                        k = 0;
                        concat_per = "";
                        int apa = 0, x = 1, y = 1;
                        do
                        {
                            //progreso inicio
                            label2.Text = "Confirmando Asegurados en Mora... " + k + " de " + tot_rowsdgv2;
                            label2.Refresh();
                            barra_progreso_txt5();
                            this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                            //progreso fin
                            concat_per = dataGridView2.Rows[k].Cells[2].FormattedValue.ToString();
                            if (concat_per.Contains(","))
                            {
                                apa = 0;
                                foreach (char c in concat_per)
                                {
                                    if (c.Equals(','))
                                    {
                                        apa++;
                                    }
                                }
                                //MessageBox.Show(concat_per+" "+apa);
                                if (apa >= 1)
                                {
                                    //concat_per=concat_per.Substring(0,13);
                                    tabla_baja_1.Rows.Add(x, " ", dataGridView2.Rows[k].Cells[0].FormattedValue.ToString(),
                                                          dataGridView2.Rows[k].Cells[1].FormattedValue.ToString(), concat_per, " ");
                                    x++;
                                }
                            }
                            else
                            {
                                if (concat_per.Length > 2)
                                {
                                    tabla_excel.Rows.Add(y, dataGridView2.Rows[k].Cells[0].FormattedValue.ToString(),
                                                              dataGridView2.Rows[k].Cells[1].FormattedValue.ToString(), concat_per, " ");
                                    y++;
                                }
                            }

                            k++;
                        } while (k < tot_rowsdgv2);

                    }));
                    //FIN RECORTE PERIODOS

                }
                //RESULTADOS

                if (tabla_baja_1.Rows.Count > 0)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        button4.Visible = true;
                        try
                        {
                            k = 0;
                            while (tabla_excel.Columns.Count > 5)
                            {
                                tabla_excel.Columns.RemoveAt(tabla_excel.Columns.Count - 1);
                            }

                            while (tabla_baja_1.Columns.Count > 6)
                            {
                                tabla_baja_1.Columns.RemoveAt(tabla_baja_1.Columns.Count - 1);
                            }

                            //tabla_excel
                            XLWorkbook wb = new XLWorkbook();
                            wb.Worksheets.Add(tabla_excel, "hoja_lz_" + ciclo);
                            wb.SaveAs(@"mod40/mod40_un_periodo.xlsx");
                            //tabla_baja_1
                            XLWorkbook wb1 = new XLWorkbook();
                            wb1.Worksheets.Add(tabla_baja_1, "hoja_lz_" + ciclo);
                            wb1.SaveAs(@"mod40/mod40_resultados.xlsx");

                            ZipFile arch = new ZipFile();
                            arch.AddFile(@"mod40/mod40_un_periodo.xlsx", "");
                            arch.AddFile(@"mod40/mod40_resultados.xlsx", "");
                            arch.Save(@"mod40/analisis_cd_" + ciclo + ".LZ40");

                        }
                        catch (Exception es)
                        {

                        }
                    }));
                }
                else
                {
                    MessageBox.Show("No Se encontraron Asegurados con Pagos Pendientes", "AVISO");
                }

                Invoke(new MethodInvoker(delegate
                {
                    label2.Text = "Se Encontraron " + tabla_baja_1.Rows.Count + " Asegurados con dos o mas Pagos Pendientes";
                    label2.Refresh();
                    label1.Text = "100%";
                    label1.Refresh();
                    progressBar1.Value = 100;
                    progressBar1.Refresh();
                    this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                    conex.guardar_evento("Se realizó una comparación de asegurados con una factura del periodo " + ciclo + ", encontrándose " + tabla_baja_1.Rows.Count + " Asegurados con dos o mas Pagos Pendientes");
                }));
            }
            else
            {
                Invoke(new MethodInvoker(delegate
                {
                    label2.Text = "Ocurrió algo inesperado...";
                    label2.Refresh();
                    label1.Text = "100%";
                    label1.Refresh();
                    progressBar1.Value = 100;
                    progressBar1.Refresh();
                    this.Text = "Cargar Archivo/CD SUA " + progressBar1.Value + "%";
                }));
                MessageBox.Show("Todos los periodos se encuentran sin pago, esto ocurre cuando la base de datos se encuentra desactualizada, ingrese los CDs correspondientes para solucionar este error.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.Close();
            }
        	 //}
        	// catch (Exception error)
        	// {
        	//    MessageBox.Show("Se presento el siguiente error en el proceso de lectura del archivo:\n\n"+error);
        //	}
        }
        
        public void barra_progreso1(){
        	porcentaje=Convert.ToInt32((((i*100)/tot_row)/3.33)+10);
        	progressBar1.Value=porcentaje;
        	progressBar1.Refresh();
        	label1.Text=porcentaje+"%";
        	label1.Refresh();
        }
        
        public void barra_progreso2(){
        	porcentaje=Convert.ToInt32((((i*100)/tot_row)/1.6666)+40);
            if (porcentaje <= 100)
            {
                progressBar1.Value = porcentaje;
            }
            else
            {
                progressBar1.Value = 100;
            }

        	progressBar1.Refresh();
        	label1.Text=porcentaje+"%";
        	label1.Refresh();  	
        }

        public void barra_progreso_txt1()
        {
            porcentaje = Convert.ToInt32((((i * 100) / tot1) / 10));//10
            progressBar1.Value = porcentaje;
            progressBar1.Value = porcentaje;
            progressBar1.Refresh();
            label1.Text = porcentaje + "%";
            label1.Refresh();
        }
        
        public void barra_progreso_txt2()
        {
            porcentaje = Convert.ToInt32((((i * 100) / tot_row) / 5) + 10);//30
            progressBar1.Value = porcentaje;
            progressBar1.Refresh();
            label1.Text = porcentaje + "%";
            label1.Refresh();
        }

        public void barra_progreso_txt3()
        {
            porcentaje = Convert.ToInt32((((k * 100) / tot_rowsdgv3) /2 ) + 30);//80
            progressBar1.Value = porcentaje;
            progressBar1.Refresh();
            label1.Text = porcentaje + "%";
            label1.Refresh();
        }

        public void barra_progreso_txt4()
        {
            porcentaje = Convert.ToInt32((((k * 100) / tot_rowsdgv3) / 6.6666) + 80);//95
            progressBar1.Value = porcentaje;
            progressBar1.Refresh();
            label1.Text = porcentaje + "%";
            label1.Refresh();
        }

        public void barra_progreso_txt5()
        {
            porcentaje = Convert.ToInt32((((k * 100) / tot_rowsdgv2) / 20) + 95);//100
            if (porcentaje <= 100)
            {
                progressBar1.Value = porcentaje;
            }
            else
            {
                progressBar1.Value = 100;
            }
            progressBar1.Refresh();
            label1.Text = porcentaje + "%";
            label1.Refresh();
        }

        public string calcula_periodo_min(String per_max)
        {
            String per_min="";
            int per_anio = 0, per_mes = 0;
            //proceso para bajarle 4 meses al periodo
            per_anio = Convert.ToInt32(per_max.Substring(0, 4));//2020
            per_mes = Convert.ToInt32(per_max.Substring(4, 2));//06

            //resta_periodo
            if (per_mes == 4)
            {
                per_anio = per_anio - 1;
                per_mes = 12;
            }
            else
            {
                if (per_mes == 2)
                {
                    per_anio = per_anio - 1;
                    per_mes = 10;
                }
                else
                {
                    per_mes = per_mes - 4;
                }
            }

            if (per_mes < 10)
            {
                per_min = per_anio.ToString() + "0"+ per_mes.ToString();
            }
            else
            {
                per_min = per_anio.ToString() + per_mes.ToString();
            }            

            return per_min;
        }

        public void leer_nrps_sub()
        {
            String cad_con = "", hoja = "", cons_exc = "";

            tablanrp_guarda.Columns.Add("NRP");
            tablanrp_guarda.Rows.Clear();

            //esta cadena es para archivos excel 2007 y 2010
            cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='lista_nrps_mod40.xlsx';Extended Properties=Excel 12.0;";
            conexion2 = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
            conexion2.Open(); //abrimos la conexion

            hoja = "Hoja1";

            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                cons_exc = "Select [NRP] from [" + hoja + "$] WHERE [NUM_SUB]=\"" + conex.leer_config_sub()[4] + "\"";
                try
                {
                    //Si el usuario escribio el nombre de la hoja se procedera con la busqueda
                    //conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                    //conexion.Open(); //abrimos la conexion
                    dataAdapter2 = new OleDbDataAdapter(cons_exc, conexion2); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet2 = new DataSet(); // creamos la instancia del objeto DataSet
                    if (dataAdapter2.Equals(null))
                    {
                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");
                    }
                    else
                    {
                        dataAdapter2.Fill(dataSet2, hoja);//llenamos el dataset
                        tablanrps = dataSet2.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        conexion2.Close();//cerramos la conexion
                        //MessageBox.Show(tablanrps.Rows[0][0].ToString());
                        lista_nrps="";
                        for(int x=0; x<tablanrps.Rows.Count;x++){
                            lista_nrps += "\""+tablanrps.Rows[x][0].ToString()+"\",";
                        }

                        lista_nrps=lista_nrps.Substring(0, lista_nrps.Length - 1);

                        //MessageBox.Show(lista_nrps);
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }
            }
        }

        private void Carga_access_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            leer_nrps_sub();

            if (nrp != null)
            {
            	conex.conectar("base_principal");
            	dataGridView3.DataSource=conex.consultar("SELECT COUNT(per_cd_sua) FROM base_principal.mod40_sua WHERE per_cd_sua=\""+periodo_nor+"\"");
            	
            	if(Convert.ToInt32(dataGridView3.Rows[0].Cells[0].FormattedValue.ToString())==0){
                //if(Convert.ToInt32(dataGridView3.Rows[0].Cells[0].FormattedValue.ToString())>=1){
	            	hilosecundario = new Thread(new ThreadStart(proceso_carga_sua));
	                hilosecundario.Start();
            	}else{
            		//MessageBox.Show(dataGridView3.Rows[0].Cells[0].FormattedValue.ToString());
            		MessageBox.Show("El periodo que intenta ingresar ya fue ingresado anteriormente.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            		this.Close();
            	}
                //proceso_carga_sua();
            }
            else
            {
                hilosecundario = new Thread(new ThreadStart(proceso_carga_txt));
                hilosecundario.Start();  
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
        	/*Visor_oficios40 viso = new Visor_oficios40();
                viso.recibir_baja(tabla_baja_1);
                viso.Show();*/
        	
        /*	SaveFileDialog dialogz = new SaveFileDialog();
        	String arch_lz40;
        	
        	dialogz.Filter = "Archivos de Nova Gear Modalidad 40 (*.LZ40)|*.LZ40"; //le indicamos el tipo de filtro en este caso que busque
        	//solo los archivos Access
        	dialogz.Title = "Guardar Archivo de Resultados";//le damos un titulo a la ventana

        	dialogz.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
        	
        	//si al seleccionar el archivo damos Ok
        	if (dialogz.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        	{*/
        		try{
        			/*arch_lz40 = dialogz.FileName;
        			ZipFile arch = new ZipFile();
        			arch.AddFile(@"mod40/mod40_un_periodo.xlsx","");
        			arch.AddFile(@"mod40/mod40_resultados.xlsx","");
        			arch.Save(arch_lz40);*/
        			MessageBox.Show("El archivo se creó correctamente, se procederá a la ventana de los resultados.","EXITO");
        			Resultados_analisis resu_check = new Resultados_analisis();
        			resu_check.recibe_arch(@"mod40/analisis_cd_"+ciclo+".LZ40","analisis_cd_"+ciclo+".LZ40");
        			resu_check.Show();
        			
        		}catch(Exception es){
        			MessageBox.Show("Ocurrió el siguiente problema al crear el archivo de resultados:\n\n"+es);
        		}
        		
        	//}
        }
		
		void DataGridView2CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		public void carga_excel(String cons_exc2){
			
			dataAdapter= new OleDbDataAdapter(cons_exc2, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
			// creamos la instancia del objeto DataSet
			dataSet = new DataSet();
			dataAdapter.Fill(dataSet, "hoja_lz");//llenamos el dataset
			dataGridView1.DataSource = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
			//conexion3.Close();//cerramos la conexion
			dataGridView1.AllowUserToAddRows = false;
		}
		
		public void carga_excel1(String cons_exc2){
			
			dataAdapter1= new OleDbDataAdapter(cons_exc2, conexion1); //traemos los datos de la hoja y las guardamos en un dataSdapter
			// creamos la instancia del objeto DataSet
			dataSet1 = new DataSet();
			dataAdapter1.Fill(dataSet1, "hoja_lz");//llenamos el dataset
			dataGridView3.DataSource = dataSet1.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
			//conexion3.Close();//cerramos la conexion
			dataGridView3.AllowUserToAddRows = false;
		}
	}
}

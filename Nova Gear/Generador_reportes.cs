/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 11/02/2016
 * Hora: 03:10 p. m.
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
using Microsoft.SqlServer.Types;
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Generador_reportes.
	/// </summary>
	public partial class Generador_reportes : Form
	{
		public Generador_reportes()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String per_sel,col1,col2,col3,col4,col5,col6,col7,col8,col9,combreg_nom,carpeta_sel,reg_pat,sql,nn,fecha,cifra,fech_cifra,fecha_fact;
		int i=0,tipo_fact=0,vacios=0,j=0,progreso=0,nn_found=0,retros_found=0;
        DateTime manana;

		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
		DataTable consultamysql = new DataTable();
		DataTable tablarow = new DataTable();//En Uso
        DataTable tabla_cifra = new DataTable();//En Uso
		
		//periodos normales 
		public void llenar_Cb1(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			
			i=0;
			dataGridView1.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			do{
				comboBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView1.RowCount);
			i=0;
			comboBox1.SelectedIndex=-1;
			comboBox1.Text="";
            conex.cerrar();
		}
		
		//periodos especiales
		public void llenar_Cb1_esp(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			
			i=0;
			dataGridView1.DataSource = conex.consultar("SELECT DISTINCT periodo_factura FROM base_principal.datos_factura WHERE periodo_factura <> \"-\" ORDER BY periodo_factura;");
			do{
				comboBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView1.RowCount);
			i=0;
			comboBox1.SelectedIndex=-1;
			comboBox1.Text="";
            conex.cerrar();
		}
		
        //ema
        public void llenar_Cb2()
        {
            conex2.conectar("ema");
            comboBox2.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex2.consultar("SHOW TABLES FROM ema ");
            do
            {
                comboBox2.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i <dataGridView2.RowCount);
            i = 0;
            //MessageBox.Show(dataGridView2.Rows[0].Cells[0].Value.ToString());
            conex2.cerrar();
        }
        
        public void prefactura_sector00_ema(){
        	if (tipo_fact == 1) // -------PREFACTURA
            {
                DialogResult result;
                result = MessageBox.Show("Se creará la Prefactura del Periodo: " + per_sel + "\nSe utilizarán los domicilios de la EMA"+
                     "\n\n¿Desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    comboBox2.Enabled = false;
                    label3.Text = "Asignando Notificadores...";
                    label3.Visible = true;
                    Asigna_noti asinot = new Asigna_noti(per_sel,comboBox3.SelectedIndex);
                    asinot.Show();
                    label3.Text = "Conectando con tabla de la EMA";
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = conex.consultar(sql);
                    
                    
                    if(dataGridView1.Columns.Count==6){
                   		dataGridView1.Columns.Add("domicilio", "domicilio");
                   		dataGridView1.Columns.Add("localidad", "localidad");
                    }
                    
                    conex2.conectar("ema");

                    i = 0;
					vacios=0;
					j=comboBox2.Items.Count-1;

                    do{
						carpeta_sel=comboBox2.Items[j].ToString();
                        label3.Text = "Extrayendo domicilios";
                        label3.Refresh();
	                    do
	                    {
	                       
	                        if((dataGridView1.Rows[i].Cells[6].Value == null)){
	                        	
		                        reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
		                        reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
		                        dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad FROM " + carpeta_sel + " WHERE reg_pat1 =\"" + reg_pat + "\"");
		                        if (dataGridView2.RowCount > 0)
		                        {
		                            dataGridView1.Rows[i].Cells[6].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
		                            dataGridView1.Rows[i].Cells[7].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();
                                }
		                        else
		                        {
		                        	if(j==0){
		                            	dataGridView1.Rows[i].Cells[6].Value = "-";
		                            	dataGridView1.Rows[i].Cells[7].Value = "-";
                                        vacios++;
		                        	}
		                            
		                        }     
	                        }
	                       
                            if (progreso == 10)
                            {
                                label3.Text = "Extrayendo domicilios.";
                                label3.Refresh();
                            }
                            if (progreso == 20)
                            {
                                label3.Text = "Extrayendo domicilios..";
                                label3.Refresh();
                            }
                            if (progreso == 30)
                            {
                                label3.Text = "Extrayendo domicilios...";
                                label3.Refresh();
                            }
                            if (progreso == 40)
                            {
                                label3.Text = "Extrayendo domicilios";
                                label3.Refresh();
                                progreso = 0;
                            }

                            i++;
                            progreso++;
	                    } while (i < dataGridView1.RowCount);
                       
	                    i=0;
	                    
	                    j--;
                    }while(j>=0);

                    //conex2.cerrar();
                    
                    i = 0;
                    progreso = 0;
                   // dataGridView2.Rows.Clear();

                    col1 = "";
                    col2 = "";
                    col3 = "";
                    col4 = "";
                    col5 = "";
                    col6 = "";
                    col7 = "";
                    col8 = "";

                    label3.Text = "Exportando datos";
                    label3.Refresh();

                    do{

                        col1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        col2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        col3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        col4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        col5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        col6 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        col7 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        col8 = dataGridView1.Rows[i].Cells[7].Value.ToString();

                        tablarow.Rows.Add(col1, col2, col3, col4, col5, col7, col8, col6);

                        col1 = "";
                        col2 = "";
                        col3 = "";
                        col4 = "";
                        col5 = "";
                        col6 = "";
                        col7 = "";
                        col8 = "";

                        if (progreso == 10)
                        {
                            label3.Text = "Exportando datos.";
                            label3.Refresh();
                        }

                        if (progreso == 20)
                        {
                            label3.Text = "Exportando datos..";
                            label3.Refresh();
                        }

                        if (progreso == 30)
                        {
                            label3.Text = "Exportando datos...";
                            label3.Refresh();
                        }

                        if (progreso == 40)
                        {
                            label3.Text = "Exportando datos";
                            label3.Refresh();
                            progreso = 0;
                        }
                        i++;
                        progreso++;
                    } while (i < dataGridView1.RowCount);

                    label3.Text = "Generando Prefactura";
                    Visor_Prefactura visor2 = new Visor_Prefactura();
                    visor2.envio3(tablarow);
                    visor2.Show();
                    tablarow.Rows.Clear();
                    if(vacios>0){
                    	MessageBox.Show("No se encontró el domicilio de: "+vacios+" Patrones", "Listo!");
                    }else{
                    	MessageBox.Show("Se encontró el domicilio de todos los Patrones", "Listo!");
                    }
                    MessageBox.Show("Se ha generado Reporte adecuadamente", "Listo!");
                    conex.guardar_evento("Se Genereró Reporte de Prefactura del Periodo: "+per_sel);
                    label3.Visible = false;

                }
            }
            else//SECTOR 00 
            {
                DialogResult result;
                result = MessageBox.Show("Se creará el reporte de los Sectores No Asignados del Periodo: " + per_sel + "\nSe utilizarán los domicilios de la EMA" +
                     "\n\n¿Desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    comboBox2.Enabled = false;
                    label3.Text = "Asignando Notificadores...";
                    label3.Visible = true;
                    Asigna_noti asinot = new Asigna_noti(per_sel,comboBox3.SelectedIndex);
                    retros_found = asinot.retro_lectura();
                    asinot.Show();
                    label3.Text = "Conectando con tabla de la EMA";
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = conex.consultar(sql);
                    
                    
                    if(dataGridView1.Columns.Count==6){
                    	dataGridView1.Columns.Add("domicilio", "domicilio");
                    	dataGridView1.Columns.Add("localidad", "localidad");
                    }
                    
                    conex2.conectar("ema");
                    i = 0;
                    vacios = 0;
                    j = comboBox2.Items.Count - 1;

                    do
                    {
                        carpeta_sel = comboBox2.Items[j].ToString();
                        label3.Text = "Extrayendo domicilios";
                        label3.Refresh();
                        //MessageBox.Show(carpeta_sel+" "+dataGridView1.RowCount);
                        
                        while(i < dataGridView1.RowCount){

                            if ((dataGridView1.Rows[i].Cells[6].Value == null))
                            {

                                reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                                reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
                                dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad FROM " + carpeta_sel + " WHERE reg_pat1 =\"" + reg_pat + "\"");
                                if (dataGridView2.RowCount > 0)
                                {
                                    dataGridView1.Rows[i].Cells[6].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                                    dataGridView1.Rows[i].Cells[7].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        dataGridView1.Rows[i].Cells[6].Value = "-";
                                        dataGridView1.Rows[i].Cells[7].Value = "-";
                                        vacios++;
                                    }

                                }
                            }

                            if (progreso == 10)
                            {
                                label3.Text = "Extrayendo domicilios.";
                                label3.Refresh();
                            }
                            if (progreso == 20)
                            {
                                label3.Text = "Extrayendo domicilios..";
                                label3.Refresh();
                            }
                            if (progreso == 30)
                            {
                                label3.Text = "Extrayendo domicilios...";
                                label3.Refresh();
                            }
                            if (progreso == 40)
                            {
                                label3.Text = "Extrayendo domicilios";
                                label3.Refresh();
                                progreso = 0;
                            }

                            i++;
                            progreso++;
                        }

                        i = 0;

                        j--;
                    } while (j >= 0);

                    //conex2.cerrar();

                    i = 0;
                    progreso = 0;
                    // dataGridView2.Rows.Clear();

                    col1 = "";
                    col2 = "";
                    col3 = "";
                    col4 = "";
                    col5 = "";
                    col6 = "";
                    col7 = "";
                    col8 = "";

                    label3.Text = "Exportando datos";
                    label3.Refresh();

                     while(i < dataGridView1.RowCount){

                        col1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        col2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        col3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        col4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        col5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        col6 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        col7 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        col8 = dataGridView1.Rows[i].Cells[7].Value.ToString();

                        tablarow.Rows.Add(col1, col2, col3, col4, col5, col7, col8, col6);

                        col1 = "";
                        col2 = "";
                        col3 = "";
                        col4 = "";
                        col5 = "";
                        col6 = "";
                        col7 = "";
                        col8 = "";

                        if (progreso == 10)
                        {
                            label3.Text = "Exportando datos.";
                            label3.Refresh();
                        }

                        if (progreso == 20)
                        {
                            label3.Text = "Exportando datos..";
                            label3.Refresh();
                        }

                        if (progreso == 30)
                        {
                            label3.Text = "Exportando datos...";
                            label3.Refresh();
                        }

                        if (progreso == 40)
                        {
                            label3.Text = "Exportando datos";
                            label3.Refresh();
                            progreso = 0;
                        }
                        i++;
                        progreso++;
                    }

                    label3.Text = "Generando Prefactura";
                    Visor_Sector00 visor3 = new Visor_Sector00();
                    visor3.envio4(tablarow);
                    visor3.Show();
                    tablarow.Rows.Clear();
                    MessageBox.Show("Se Reutilizarán los Sectores de "+retros_found+" Registros Patronales.");
                    
                    if(vacios>0){
                    	MessageBox.Show("No se encontró el domicilio de: "+vacios+" Patrones", "Listo!");
                    }else{
                    	MessageBox.Show("Se encontró el domicilio de todos los Patrones", "Listo!");
                    }
                    MessageBox.Show("Se ha generado el Reporte adecuadamente", "Listo!");
                    conex.guardar_evento("Se Generó el Reporte de Sectores No Asignados del Periodo: "+per_sel);
                    label3.Visible = false;

                }
            }
            conex.cerrar();
            conex2.cerrar();
        }
        
        public void prefactura_sector00_sindo(){
        	if (tipo_fact == 1) // -------PREFACTURA
            {
                DialogResult result;
                result = MessageBox.Show("Se creará la Prefactura del Periodo: " + per_sel + "\nSe utilizarán los domicilios de SINDO"+
                     "\n\n¿Desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    comboBox2.Enabled = false;
                    label3.Text = "Asignando Notificadores...";
                    label3.Visible = true;
                    Asigna_noti asinot = new Asigna_noti(per_sel,comboBox3.SelectedIndex);
                    asinot.Show();
                    label3.Text = "Conectando con tabla de domicilios de SINDO";
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = conex.consultar(sql);
                    
                    
                    if(dataGridView1.Columns.Count==6){
                   		dataGridView1.Columns.Add("domicilio", "domicilio");
                   		dataGridView1.Columns.Add("localidad", "localidad");
                    }
                    
                    conex2.conectar("base_principal");

                    i = 0;
					vacios=0;
					

                   
                        label3.Text = "Extrayendo domicilios";
                        label3.Refresh();
	                    do
	                    {
	                       
	                        if((dataGridView1.Rows[i].Cells[6].Value == null)){
	                        	
		                        reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
		                        reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
		                        dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad,cp FROM sindo WHERE registro_patronal =\"" + reg_pat + "\"");
		                        if (dataGridView2.RowCount > 0)
		                        {
		                            dataGridView1.Rows[i].Cells[6].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
		                            dataGridView1.Rows[i].Cells[7].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString()+" CP:"+ dataGridView2.Rows[0].Cells[2].FormattedValue.ToString();
                                }
		                        else
		                        {
		                        	if(j==0){
		                            	dataGridView1.Rows[i].Cells[6].Value = "-";
		                            	dataGridView1.Rows[i].Cells[7].Value = "-";
                                        vacios++;
		                        	}
		                            
		                        }     
	                        }
	                       
                            if (progreso == 10)
                            {
                                label3.Text = "Extrayendo domicilios.";
                                label3.Refresh();
                            }
                            if (progreso == 20)
                            {
                                label3.Text = "Extrayendo domicilios..";
                                label3.Refresh();
                            }
                            if (progreso == 30)
                            {
                                label3.Text = "Extrayendo domicilios...";
                                label3.Refresh();
                            }
                            if (progreso == 40)
                            {
                                label3.Text = "Extrayendo domicilios";
                                label3.Refresh();
                                progreso = 0;
                            }

                            i++;
                            progreso++;
	                    } while (i < dataGridView1.RowCount);
                       
	                    i=0;
	                    
	                

                    //conex2.cerrar();
                    
                    i = 0;
                    progreso = 0;
                   // dataGridView2.Rows.Clear();

                    col1 = "";
                    col2 = "";
                    col3 = "";
                    col4 = "";
                    col5 = "";
                    col6 = "";
                    col7 = "";
                    col8 = "";

                    label3.Text = "Exportando datos";
                    label3.Refresh();

                    do{

                        col1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        col2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        col3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        col4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        col5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        col6 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        col7 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        col8 = dataGridView1.Rows[i].Cells[7].Value.ToString();

                        tablarow.Rows.Add(col1, col2, col3, col4, col5, col7, col8, col6);

                        col1 = "";
                        col2 = "";
                        col3 = "";
                        col4 = "";
                        col5 = "";
                        col6 = "";
                        col7 = "";
                        col8 = "";

                        if (progreso == 10)
                        {
                            label3.Text = "Exportando datos.";
                            label3.Refresh();
                        }

                        if (progreso == 20)
                        {
                            label3.Text = "Exportando datos..";
                            label3.Refresh();
                        }

                        if (progreso == 30)
                        {
                            label3.Text = "Exportando datos...";
                            label3.Refresh();
                        }

                        if (progreso == 40)
                        {
                            label3.Text = "Exportando datos";
                            label3.Refresh();
                            progreso = 0;
                        }
                        i++;
                        progreso++;
                    } while (i < dataGridView1.RowCount);

                    label3.Text = "Generando Prefactura";
                    Visor_Prefactura visor2 = new Visor_Prefactura();
                    visor2.envio3(tablarow);
                    visor2.Show();
                    tablarow.Rows.Clear();
                    if(vacios>0){
                    	MessageBox.Show("No se encontró el domicilio de: "+vacios+" Patrones", "Listo!");
                    }else{
                    	MessageBox.Show("Se encontró el domicilio de todos los Patrones", "Listo!");
                    }
                    MessageBox.Show("Se ha generado Reporte adecuadamente", "Listo!");
                    conex.guardar_evento("Se Genereró Reporte de Prefactura del Periodo: "+per_sel);
                    label3.Visible = false;

                }
            }
            else//SECTOR 00 
            {
                DialogResult result;
                result = MessageBox.Show("Se creará el reporte de los Sectores No Asignados del Periodo: " + per_sel + "\nSe utilizarán los domicilios de la EMA" +
                     "\n\n¿Desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    comboBox2.Enabled = false;
                    label3.Text = "Asignando Notificadores...";
                    label3.Visible = true;
                    Asigna_noti asinot = new Asigna_noti(per_sel,comboBox3.SelectedIndex);
                    retros_found = asinot.retro_lectura();
                    asinot.Show();
                    label3.Text = "Conectando con tabla de la EMA";
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = conex.consultar(sql);
                    
                    
                    if(dataGridView1.Columns.Count==6){
                    	dataGridView1.Columns.Add("domicilio", "domicilio");
                    	dataGridView1.Columns.Add("localidad", "localidad");
                    }
                    
                    conex2.conectar("base_principal");
                    i = 0;
                    vacios = 0;
                    
                        label3.Text = "Extrayendo domicilios";
                        label3.Refresh();
                        //MessageBox.Show(carpeta_sel+" "+dataGridView1.RowCount);
                        
                        while(i < dataGridView1.RowCount){

                            if ((dataGridView1.Rows[i].Cells[6].Value == null))
                            {

                                reg_pat = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
                                reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
                                dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad,cp FROM sindo WHERE registro_patronal =\"" + reg_pat + "\"");
                                if (dataGridView2.RowCount > 0)
                                {
                                    dataGridView1.Rows[i].Cells[6].Value = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
                                   dataGridView1.Rows[i].Cells[7].Value = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString()+" CP:"+ dataGridView2.Rows[0].Cells[2].FormattedValue.ToString();
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        dataGridView1.Rows[i].Cells[6].Value = "-";
                                        dataGridView1.Rows[i].Cells[7].Value = "-";
                                        vacios++;
                                    }

                                }
                            }

                            if (progreso == 10)
                            {
                                label3.Text = "Extrayendo domicilios.";
                                label3.Refresh();
                            }
                            if (progreso == 20)
                            {
                                label3.Text = "Extrayendo domicilios..";
                                label3.Refresh();
                            }
                            if (progreso == 30)
                            {
                                label3.Text = "Extrayendo domicilios...";
                                label3.Refresh();
                            }
                            if (progreso == 40)
                            {
                                label3.Text = "Extrayendo domicilios";
                                label3.Refresh();
                                progreso = 0;
                            }

                            i++;
                            progreso++;
                        }

                        i = 0;

                       
                    //conex2.cerrar();

                    i = 0;
                    progreso = 0;
                    // dataGridView2.Rows.Clear();

                    col1 = "";
                    col2 = "";
                    col3 = "";
                    col4 = "";
                    col5 = "";
                    col6 = "";
                    col7 = "";
                    col8 = "";

                    label3.Text = "Exportando datos";
                    label3.Refresh();

                     while(i < dataGridView1.RowCount){

                        col1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        col2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        col3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        col4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        col5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        col6 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        col7 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        col8 = dataGridView1.Rows[i].Cells[7].Value.ToString();

                        tablarow.Rows.Add(col1, col2, col3, col4, col5, col7, col8, col6);

                        col1 = "";
                        col2 = "";
                        col3 = "";
                        col4 = "";
                        col5 = "";
                        col6 = "";
                        col7 = "";
                        col8 = "";

                        if (progreso == 10)
                        {
                            label3.Text = "Exportando datos.";
                            label3.Refresh();
                        }

                        if (progreso == 20)
                        {
                            label3.Text = "Exportando datos..";
                            label3.Refresh();
                        }

                        if (progreso == 30)
                        {
                            label3.Text = "Exportando datos...";
                            label3.Refresh();
                        }

                        if (progreso == 40)
                        {
                            label3.Text = "Exportando datos";
                            label3.Refresh();
                            progreso = 0;
                        }
                        i++;
                        progreso++;
                    }

                    label3.Text = "Generando Prefactura";
                    Visor_Sector00 visor3 = new Visor_Sector00();
                    visor3.envio4(tablarow);
                    visor3.Show();
                    tablarow.Rows.Clear();
                    MessageBox.Show("Se Reutilizarán los Sectores de "+retros_found+" Registros Patronales.");
                    
                    if(vacios>0){
                    	MessageBox.Show("No se encontró el domicilio de: "+vacios+" Patrones", "Listo!");
                    }else{
                    	MessageBox.Show("Se encontró el domicilio de todos los Patrones", "Listo!");
                    }
                    MessageBox.Show("Se ha generado Reporte adecuadamente", "Listo!");
                    conex.guardar_evento("Se Genereró Reporte de Sectores No Asignados del Periodo: "+per_sel);
                    label3.Visible = false;

                }
            }
            conex.cerrar();
            conex2.cerrar();
        }
        
        public void buscar_nn(){
        	
        	label3.Text = "Buscando NN...";
        	label3.Refresh();
			label3.Visible = true;
				
        	conex2.conectar("base_principal");
        	i=0;
            nn_found = 0;
        	do{
            	if(dataGridView1.RowCount>0){
	        		dataGridView2.DataSource = conex2.consultar("SELECT COUNT(nn) FROM datos_factura WHERE nn =\"NN\" AND registro_patronal =\""+dataGridView1.Rows[i].Cells[0].Value.ToString()+"\"");
	        		if((Convert.ToInt32 (dataGridView2.Rows[0].Cells[0].Value.ToString())) > 0){
	        			dataGridView1.Rows[i].Cells[14].Value = "NN (Revisar)";
	                    nn_found++;
	        		}else{
	        			if(((Convert.ToInt32 (dataGridView2.Rows[0].Cells[0].Value.ToString())) == 0)){
	        				dataGridView1.Rows[i].Cells[14].Value = " ";	
	        			}
	        		}
	        		i++;
	        		label3.Text = "Buscando NN... "+i+" de "+dataGridView1.RowCount;
	        		label3.Refresh();
                }
        	}while(i<dataGridView1.RowCount);
            
            if(nn_found>0){
            	MessageBox.Show("Se encontraron "+nn_found+" patrones que han sido marcados anteriormente como NN");
            }else{
            	
            }
        }

        public int cifra_control(String tipo_per)
        {
            conex3.conectar("base_principal");
            tabla_cifra = conex3.consultar("SELECT cifra_control,fecha_cifra_control_inicio,fecha_cifra_control_termino FROM estado_periodos WHERE nombre_periodo=\"" + tipo_per + "\"");
            if (tabla_cifra.Rows.Count > 0)
            {
            	if(tabla_cifra.Rows[0][0].ToString().Length>1){
                	cifra = tabla_cifra.Rows[0][0].ToString();
                	fech_cifra = tabla_cifra.Rows[0][1].ToString().Substring(0,10) + "-" + tabla_cifra.Rows[0][2].ToString().Substring(0,10);
                	return 1;
            	}else{
            		//MessageBox.Show("No Se Encontró la Cifra Control","Aviso");
            		cifra="";
            		fech_cifra="";
            		return 0;
            	}
            }else{
            	return 0;
            }
        }

        public void factura(){
        	col1 = "";
			col2 = "";
			col3 = "";
			col4 = "";
			col5 = "";
			col6 = "";
			col7 = "";
			col8 = "";
            
			conex.conectar("base_principal");
			tablarow.Clear();
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
			DialogResult result = fbd.ShowDialog();
			
			if (result == DialogResult.OK)
			{
				button1.Enabled = false;
				button2.Enabled = false;
				button3.Enabled = false;
				comboBox1.Enabled = false;
				comboBox3.Enabled = false;
                dateTimePicker1.Enabled = false;
				label3.Text = "Asignando Notificadores...";
				label3.Visible = true;

				Asigna_noti asinot = new Asigna_noti(per_sel,comboBox3.SelectedIndex);
				asinot.Show();
				
				//Factura Selectiva
				Depuracion.Depu_manu sel_fac = new Nova_Gear.Depuracion.Depu_manu(per_sel,5);
				sel_fac.ShowDialog();
				
				
				dataGridView1.Columns.Clear();
				if(comboBox3.SelectedIndex==0){
					dataGridView1.DataSource = conex.consultar("SELECT registro_patronal,razon_social,credito_cuotas,periodo,tipo_documento,sector_notificacion_actualizado,importe_cuota,notificador,controlador,nombre_periodo,id,credito_multa,importe_multa,sector_notificacion_inicial" +
                                                           //" FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND status=\"POR_ACTUALIZAR\" ORDER BY controlador,notificador,sector_notificacion_actualizado,registro_patronal,credito_cuotas");
                                                         " FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND status =\"POR_ACTUALIZAR\" ORDER BY controlador,notificador,credito_cuotas,registro_patronal");
				}else{
					dataGridView1.DataSource = conex.consultar("SELECT registro_patronal,razon_social,credito_cuotas,periodo,tipo_documento,sector_notificacion_actualizado,importe_cuota,notificador,controlador,periodo_factura,id,credito_multa,importe_multa,sector_notificacion_inicial" +
                                                           //" FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND status=\"POR_ACTUALIZAR\" ORDER BY controlador,notificador,sector_notificacion_actualizado,registro_patronal,credito_cuotas");
                                                         " FROM datos_factura WHERE periodo_factura =\"" + per_sel + "\" AND status =\"POR_ACTUALIZAR\" ORDER BY controlador,notificador,credito_cuotas,registro_patronal");
				}
				
				
                if(dataGridView1.RowCount==0){
                   MessageBox.Show("No Hay Ningún Crédito para entregar.");
                   this.Dispose();
                   this.Close();
                   
                }
                                                         
                //if(dataGridView1.Columns.Count == 16){ ????????
                if(dataGridView1.Columns.Count == 14){
					dataGridView1.Columns.Add("nn","NN");
				}
                                 
				buscar_nn();
                

                label3.Text = "Generando Reportes...";
                label3.Refresh();
				carpeta_sel= fbd.SelectedPath.ToString();
				//MessageBox.Show(per_sel);

				i=0;
                conex2.conectar("base_principal");
                fecha = dateTimePicker1.Value.ToShortDateString();
                fecha_fact = fecha;
                fecha = fecha.Substring(6, 4) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(0, 2);
				tablarow.Rows.Clear();
				
				while(i<dataGridView1.RowCount){
					col1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
					col2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
					col3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
					col4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
					col5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
					col6 = dataGridView1.Rows[i].Cells[5].Value.ToString();
					col7 = dataGridView1.Rows[i].Cells[6].Value.ToString();
					col8 = dataGridView1.Rows[i].Cells[7].Value.ToString();//notificador
					col9 = dataGridView1.Rows[i].Cells[8].Value.ToString();//controlador
                    nn = dataGridView1.Rows[i].Cells[14].Value.ToString();//nn
                    

					if (col2.Length >= 10)
					{
						combreg_nom = col1 + "   " + col2.Substring(0, 10);
					}
					else
					{
						combreg_nom = col1 + "   " + col2.Substring(0, col2.Length);
					}

                    if(col3.Length<9){
                        col3 = dataGridView1.Rows[i].Cells[11].Value.ToString()+" M";
                        col7 = dataGridView1.Rows[i].Cells[12].Value.ToString();
                    }
					
					tablarow.Rows.Add(combreg_nom,col3,col4,col5,col6,col7,col8,col9,dataGridView1.Rows[i].Cells[9].Value.ToString(),nn,dataGridView1.Rows[i].Cells[13].Value.ToString(),cifra,fech_cifra,fecha_fact,conex.leer_config_sub()[0],conex.leer_config_sub()[3],conex.leer_config_sub()[9],"","");
				
                    if (i + 1 < dataGridView1.RowCount)
					{
						if (col8 != dataGridView1.Rows[(i + 1)].Cells[7].Value.ToString())
						{
                            Visor_reporte_factura visor1 = new Visor_reporte_factura();
							visor1.envio2(tablarow, carpeta_sel);
							visor1.Show();
							tablarow.Rows.Clear();
						}
					}
					else
					{
						if (i <= dataGridView1.RowCount)
						{
                            Visor_reporte_factura visor1 = new Visor_reporte_factura();
                            visor1.envio2(tablarow, carpeta_sel);
							visor1.Show();
							tablarow.Rows.Clear();
						}
					}
                    
                    sql = "UPDATE datos_factura SET fecha_entrega=\""+fecha+"\", status=\"EN TRAMITE\" WHERE id ="+dataGridView1.Rows[i].Cells[10].FormattedValue.ToString()+"";
                    conex2.consultar(sql);
                    i++;
				}
                conex2.cerrar();
				//MessageBox.Show("total de regs:" + i);
				i = 0;
				//Visor_reporte_factura visor1 = new Visor_reporte_factura();
				//visor1.envio2(tablarow,"8");
				//visor1.Show();
				MessageBox.Show("Se han generado todos los Reportes adecuadamente","Listo!");
				conex.guardar_evento("Se Generaron Reportes de Factura de Notificadores del periodo: "+per_sel);
				button1.Enabled = true;
				button2.Enabled = true;
				button3.Enabled = true;
				comboBox1.Enabled = true;
				comboBox3.Enabled = true;
				dateTimePicker1.Enabled=true;
				label3.Visible = false;
				Process.Start("explorer.exe", carpeta_sel);
			}

        }
        
		void Generador_reportesLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            tablarow.Columns.Add("id");
			tablarow.Columns.Add("reg_pat");
			tablarow.Columns.Add("credito_cuota");
			tablarow.Columns.Add("periodo");
			tablarow.Columns.Add("tipo doc");
			tablarow.Columns.Add("sector");
			tablarow.Columns.Add("importe");
			tablarow.Columns.Add("noti");
			tablarow.Columns.Add("contro");
            tablarow.Columns.Add("nom_periodo");
            tablarow.Columns.Add("sector_inicial");
            tablarow.Columns.Add("cifra");
            tablarow.Columns.Add("fecha_cifra");
            tablarow.Columns.Add("fecha_factura");
            tablarow.Columns.Add("del");
            tablarow.Columns.Add("subdel");
            tablarow.Columns.Add("jefe_secc_epo");
            tablarow.Columns.Add("estatus");
            tablarow.Columns.Add("fecha_not");

            //llenar_Cb2();
			llenar_Cb1();
			comboBox3.SelectedIndex=0;
            manana=DateTime.Now.AddDays(1);
            dateTimePicker1.Value = manana;
            
             String rango = MainForm.datos_user_static[2];//rango
            
             if(Convert.ToInt32(rango)<4){
             	button9.Enabled=true;
            	button10.Enabled=true;
            	button11.Enabled=true;
            	//button12.Enabled=true;
            }
		}
		//Boton Prefactura
		void Button1Click(object sender, EventArgs e)
		{
            col1 = "";
            col2 = "";
            col3 = "";
            col4 = "";
            col5 = "";
            col6 = "";
            col7 = "";
            col8 = "";
            conex.conectar("base_principal");
            tablarow.Clear();
			tipo_fact=1;
			panel1.Visible=true;
            comboBox1.Enabled = false;
            comboBox3.Enabled = false;
           
		}
		//Boton Sector 00 y Sectores sin Not
		void Button3Click(object sender, EventArgs e)
		{
            col1 = "";
            col2 = "";
            col3 = "";
            col4 = "";
            col5 = "";
            col6 = "";
            col7 = "";
            col8 = "";
            conex.conectar("base_principal");
            tablarow.Clear(); 
			tipo_fact=2;
			panel1.Visible=true;
            comboBox1.Enabled = false;
            comboBox3.Enabled = false;
		}
		//Boton Factura
		void Button2Click(object sender, EventArgs e)
		{
			if(cifra_control(per_sel)==1){
				factura();
			}else{
				DialogResult re= MessageBox.Show("No se encontró la cifra control de este periodo \n\n¿Desea Continuar de Todas Formas?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					if(re== DialogResult.Yes){
						factura();
					}
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			per_sel=comboBox1.SelectedItem.ToString();
			
			//MessageBox.Show(per_sel);
			button1.Enabled=true;
			button2.Enabled=true;
			button3.Enabled=true;
		}
		
		void Button4Click(object sender, EventArgs e)
		{
				
			if(radioButton1.Checked==true){//EMA
                //panel2.Visible = true;
                llenar_Cb2();
                if (tipo_fact == 1)
                { //Prefactura
                    sql="SELECT registro_patronal,razon_social,periodo,sector_notificacion_actualizado,notificador,nombre_periodo" +
                                                           " FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND controlador <> \"-\" AND status=\"0\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal,credito_cuotas";
                }
                else
                {
                    if (tipo_fact == 2)
                    { //Sector 00
                    sql="SELECT registro_patronal,razon_social,periodo,sector_notificacion_actualizado,notificador,nombre_periodo" +
                                                           " FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND controlador = \"-\" AND status=\"0\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal,credito_cuotas";
                    }
                }
				
                prefactura_sector00_ema();
			}else{
				if(radioButton2.Checked==true){//SINDO
					
					if (tipo_fact == 1)
                { //Prefactura
                    sql="SELECT registro_patronal,razon_social,periodo,sector_notificacion_actualizado,notificador,nombre_periodo" +
                                                           " FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND controlador <> \"-\" AND status=\"0\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal,credito_cuotas";
                }
                else
                {
                    if (tipo_fact == 2)
                    { //Sector 00
                    sql="SELECT registro_patronal,razon_social,periodo,sector_notificacion_actualizado,notificador,nombre_periodo" +
                                                           " FROM datos_factura WHERE nombre_periodo =\"" + per_sel + "\" AND controlador = \"-\" AND status=\"0\" ORDER BY notificador,sector_notificacion_actualizado,registro_patronal,credito_cuotas";
                    }
                }
					
					prefactura_sector00_sindo();
				}
			}
			
			
		}
		//EMA
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			button4.Enabled=true;
		}
		//SINDO
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			button4.Enabled=true;
		}

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            carpeta_sel = comboBox2.SelectedItem.ToString();//tabla de la EMA
            button7.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled=true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
        	prefactura_sector00_ema();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Cartera_reportes carte = new Cartera_reportes();
            carte.Show();
            this.Hide();
            if(carte.IsDisposed == true){
            	this.Close();
            	
            }
        }
		
		void Button9Click(object sender, EventArgs e)
		{
			panel3.Visible=false;
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			Generador_factura gen_fact = new Generador_factura();
			gen_fact.Show();
			this.Hide();
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			DialogResult resus = MessageBox.Show("La Elaboración del reporte tomará varios minutos.\n\n Puede que su equipo se ralentice durante el proceso.\n\n¿Está seguro de querer continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			if (resus == DialogResult.Yes){
				Visor_reporte_mensual repor = new Visor_reporte_mensual();
				//repor.MdiParent = this;
	            repor.Show();
	            this.Hide();
	            repor.Focus();
			}
		}
		
		void Button12Click(object sender, EventArgs e)
		{
            /*
			Correspondencia_acta_citatorio actas = new Correspondencia_acta_citatorio();
			actas.Show();
			this.Hide();
			actas.Focus();*/

            Menu_correspondencia actas = new Menu_correspondencia();            
            actas.Show();
            this.Hide();
            actas.Focus();
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox3.SelectedIndex > -1){
				if(comboBox3.SelectedIndex == 0){
					llenar_Cb1();
				}else{
					llenar_Cb1_esp();
				}
			}
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			Cartera_reportes carte = new Cartera_reportes();
			carte.Show();
			this.Hide();
			if(carte.IsDisposed == true){
				this.Close();
				
			}
		}
	}
}

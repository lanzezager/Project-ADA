using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Types;
//using Microsoft.Reporting.WinForms;
//using Microsoft.ReportingServices;
using MySql.Data.MySqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using Microsoft.Reporting.WebForms;

namespace Nova_Gear.Depuracion
{
    public partial class Visor_Reporte : Form
    {
        public Visor_Reporte()
        {
            InitializeComponent();  
        }

        //ReportParameter[] parametros = new ReportParameter[8];
        DataSet_Depu depu = new DataSet_Depu();
        DataSet_Depu depu2 = new DataSet_Depu();
        DataTable tabla = new DataTable();
        Formatos_Reporte.Depu_report Depu_report1 = new Formatos_Reporte.Depu_report();
        Formatos_Reporte.Depu_report3 Depu_report31 = new Formatos_Reporte.Depu_report3();
        Formatos_Reporte.Reporte_mestro master = new Formatos_Reporte.Reporte_mestro();
        Formatos_Reporte.Factura_Notificador fact_not = new Formatos_Reporte.Factura_Notificador();
        Formatos_Reporte.reporte_sector00 sect00 = new Formatos_Reporte.reporte_sector00();
        Formatos_Reporte.Prefactura prefact = new Formatos_Reporte.Prefactura();


        String tipo_per, importe_total, noti, nom_save, nom_save_forzado, user, justi, soli, autori, foli_not, user_id,folio="";
        int x = 0,tipo_report=0,y=0,i=0,tipo_core=0;
        double sumatoria = 0,tot_hojas=0;
        string[] lista = { "portada_core.pdf", "contenido_core.pdf" };

        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        DataTable consultamysql;

        public void dame_datos()
        {
            x = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[3].FormattedValue.ToString());
           do{
                DataSet_Depu.core_depuRow fila = depu.core_depu.Newcore_depuRow();
                fila.asunto = (x+1);
                fila.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.credito = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.tipo_doc = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.inc = " 1 ";
                fila.sub = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.importe = Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue);
                fila.folio = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.folio1 = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.folio2 = " ";
                fila.clave_error = " ";
                depu.core_depu.Addcore_depuRow(fila);
                
                x++;
                if (tipo_report == 1)
                {
                    if(x==25){
                        x = tabla.Rows.Count;
                    }
                }
            }while(x < tabla.Rows.Count);

            sumatoria = 0;
            i = 0;

            do{
                sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[i].Cells[5].FormattedValue.ToString());
                i++;
            }while(i<tabla.Rows.Count);
            //tabla = depu.core_depu;
            //return tabla;
        }

        public void dame_datos_sub2()
        {
            x = 25;
            //MessageBox.Show(""+x);
            //sumatoria = 0;
            while(x < tabla.Rows.Count){
                
                DataSet_Depu.core_depu1Row fila1 = depu.core_depu1.Newcore_depu1Row();
                fila1.asunto = (x + 1);
                fila1.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila1.credito = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila1.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila1.tipo_doc = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila1.inc = " 01 ";
                fila1.sub = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila1.importe = Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue);
                fila1.folio = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila1.folio1 = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila1.folio2 = " ";
                fila1.clave_error = " ";
                depu.core_depu1.Addcore_depu1Row(fila1);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } 
			
            importe_total = sumatoria.ToString();
            importe_total = importe_total.Substring(0, ((importe_total.IndexOf('.')) + 2));
            //tabla = depu.core_depu;
            //return tabla;
        }
        
        public void dame_datos_inc31()
        {
            x = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[3].FormattedValue.ToString());
           do{
                DataSet_Depu.core_depuRow fila = depu.core_depu.Newcore_depuRow();
                fila.asunto = (x+1);
                fila.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.credito = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.tipo_doc = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.inc = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.sub = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.importe = Convert.ToDouble(dataGridView1.Rows[x].Cells[6].FormattedValue);
                fila.folio = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.folio1 = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();
                fila.folio2 = " ";
                fila.clave_error = " ";
                depu.core_depu.Addcore_depuRow(fila);
                
                x++;
                if (tipo_report == 1)
                {
                    if(x==25){
                        x = tabla.Rows.Count;
                    }
                }
            }while(x < tabla.Rows.Count);

            sumatoria = 0;
            i = 0;

            do{
                sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[i].Cells[6].FormattedValue.ToString());
                i++;
            }while(i<tabla.Rows.Count);
            //tabla = depu.core_depu;
            //return tabla;
        }

        public void dame_datos_sub2_inc31()
        {
           
            x = 25;
            //MessageBox.Show(""+x);
            //sumatoria = 0;
            while(x < tabla.Rows.Count){
                
                DataSet_Depu.core_depu1Row fila1 = depu.core_depu1.Newcore_depu1Row();
                fila1.asunto = (x + 1);
                fila1.reg_pat = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila1.credito = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila1.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila1.tipo_doc = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila1.inc = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila1.sub = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila1.importe = Convert.ToDouble(dataGridView1.Rows[x].Cells[6].FormattedValue);
                fila1.folio = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila1.folio1 = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();
                fila1.folio2 = " ";
                fila1.clave_error = " ";
                depu.core_depu1.Addcore_depu1Row(fila1);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } 
			
            importe_total = sumatoria.ToString();
            importe_total = importe_total.Substring(0, ((importe_total.IndexOf('.')) + 2));
            //tabla = depu.core_depu;
            //return tabla;
        }
        
        public void dame_datos2(){
            x = 0;
            sumatoria = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            noti = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
            do
            {
                DataSet_Depu.factura_notRow fila = depu.factura_not.Newfactura_notRow();
                fila.num = (x + 1).ToString();
                fila.reg_pat_nom = dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.credito_cuota = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.tipo_doc = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.sector = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.importe = Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                fila.notificador = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.controlador = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                fila.nom_per = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();
           
                depu.factura_not.Addfactura_notRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                x++;
            } while (x < tabla.Rows.Count); 
        }

        public void dame_datos3()
        {
            x = 0;
            y = 1;
            sumatoria = 0;
            //MessageBox.Show(dataGridView1.Rows[x].Cells[8].FormattedValue.ToString());
            noti = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
            do
            {
                DataSet_Depu.prefacturaRow fila = depu.prefactura.NewprefacturaRow();
               
                fila.id = (y).ToString();
                fila.reg_pat= dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
                fila.razon_social = dataGridView1.Rows[x].Cells[1].FormattedValue.ToString();
                fila.periodo = dataGridView1.Rows[x].Cells[2].FormattedValue.ToString();
                fila.sector = dataGridView1.Rows[x].Cells[3].FormattedValue.ToString();
                fila.notificador = dataGridView1.Rows[x].Cells[4].FormattedValue.ToString();
                fila.domicilio = dataGridView1.Rows[x].Cells[5].FormattedValue.ToString();
                fila.localidad = dataGridView1.Rows[x].Cells[6].FormattedValue.ToString();
                fila.nombre_per = dataGridView1.Rows[x].Cells[7].FormattedValue.ToString();
                //fila.nom_per = dataGridView1.Rows[x].Cells[8].FormattedValue.ToString();

                depu.prefactura.AddprefacturaRow(fila);
                //sumatoria = sumatoria + Convert.ToDouble(dataGridView1.Rows[x].Cells[5].FormattedValue.ToString());
                y++;

                if (x < (tabla.Rows.Count - 1))
                {
                    if (!(dataGridView1.Rows[x].Cells[4].FormattedValue.ToString().Equals(dataGridView1.Rows[x + 1].Cells[4].FormattedValue.ToString())))
                    {
                        y = 1;
                    }
                }

                x++;
            } while (x < tabla.Rows.Count); 
        }
        
        public void envio(DataTable tabla_report,String solicita, String autoriza){
        	tabla=tabla_report;
            dataGridView1.DataSource = tabla;
            this.soli=solicita;
            this.autori=autoriza;
            tipo_core=1;
            foli_not=generar_folio_not("CM_12", "COP");
            if (tabla.Rows.Count > 0)
            {
                tipo_report = 1;
                dame_datos();
                dame_datos_sub2();  
            }
            else
            {
                MessageBox.Show("Este Periodo No se Depuró");
                this.Hide();
            }
        }
        
        public void envio_inc31(DataTable tabla_report,int tipo_per_inc,String solicita, String autoriza){
        	tipo_per="";
        	if(tipo_per_inc==1){
        		tipo_per ="cop";
        		foli_not=generar_folio_not("INC_31", "COP");
        	}else{
        		if(tipo_per_inc==2){
        			tipo_per ="rcv";
        			foli_not = generar_folio_not("INC_31", "RCV");
        		}
        	}
        	//MessageBox.Show(""+tipo_per_inc+"|"+tipo_per+"");
        	tabla=tabla_report;
            dataGridView1.DataSource = tabla;
            this.soli=solicita;
            this.autori=autoriza;
            tipo_core=2;
            
            
            if (tabla.Rows.Count > 0)
            {
                tipo_report = 1;
                dame_datos_inc31();
                dame_datos_sub2_inc31();  
            }
            else
            {
                MessageBox.Show("No se Enviará Ningún Crédito a la Inc 31");
                this.Hide();
            }
        }
        
        public void envio2(DataTable tabla_report,String tipo){
        	this.tipo_per = tipo;
        	tabla=tabla_report;
            dataGridView1.DataSource = tabla;
            if (tabla.Rows.Count > 0)
            {
                dame_datos2();
            }
            else
            {
                MessageBox.Show("Este Periodo está vacio");
                this.Hide();
            }
        }

        public void envio3(DataTable tabla_report){
            tabla = tabla_report;
            dataGridView1.DataSource = tabla;
            if (tabla.Rows.Count > 0)
            {
                dame_datos3();
            }
            else
            {
                MessageBox.Show("Este Periodo está vacio");
                this.Hide();
            }
        }
        
        public void reporte_core()
        {
            /*
 			parametros[0] = new ReportParameter("folio", "12345657");
            parametros[1] = new ReportParameter("cop", "XXX");
            parametros[2] = new ReportParameter("rcv", "  ");
            parametros[3] = new ReportParameter("fecha", "27/11/2015");
            parametros[4] = new ReportParameter("tipo_dep", "12");
            parametros[5] = new ReportParameter("casos", "1441");
            parametros[6] = new ReportParameter("importe_total", "$1'213,243.05");
            parametros[7] = new ReportParameter("justificacion", "por que me da la gana");

            //this.reportViewer1.LocalReport.ReportPath=@"Formatos_Reporte/Report1.rdlc";
            this.reportViewer1.LocalReport.SetParameters(parametros);
            this.reportViewer1.LocalReport.DisplayName = "Reporte Depuracion";
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1",dame_datos()));
            this.reportViewer1.RefreshReport();
            */

            //crystalReportViewer1.ReportSource = master;
            //MessageBox.Show("" + depu.Tables[0].Rows.Count);
            Depu_report1.SetDataSource(depu);
            crystalReportViewer1.ReportSource = Depu_report1;

           /* if ((tipo_per.Equals("1")) || (tipo_per.Equals("2")))
            {
                Depu_report1.SetParameterValue("cop", "XXX");
                Depu_report1.SetParameterValue("rcv", "  ");
            }
            else
            {
                Depu_report1.SetParameterValue("cop", "  ");
                Depu_report1.SetParameterValue("rcv", "XXX");
            }

            */
           

            Depu_report1.SetParameterValue("folio_not",foli_not );
            Depu_report1.SetParameterValue("cop", "XXX");
            Depu_report1.SetParameterValue("rcv", "  ");
            Depu_report1.SetParameterValue("fecha", System.DateTime.Now.ToShortDateString());
            Depu_report1.SetParameterValue("clase_emision", "  ");
            Depu_report1.SetParameterValue("incidencia", "  ");
            Depu_report1.SetParameterValue("clave_ajuste", "12");
            Depu_report1.SetParameterValue("casos", tabla.Rows.Count.ToString());
            Depu_report1.SetParameterValue("solicita", soli);
            Depu_report1.SetParameterValue("autoriza", autori);
            Depu_report1.SetParameterValue("delegacion", conex.leer_config_sub()[0].ToUpper());
            Depu_report1.SetParameterValue("subdelegacion", conex.leer_config_sub()[3].ToUpper());
     
            sumatoria = Convert.ToDouble(importe_total);
            Depu_report1.SetParameterValue("importe_total", sumatoria);
            Depu_report1.SetParameterValue("justificacion", justi);
            Depu_report1.SetParameterValue("usuario", user);
            tot_hojas = (tabla.Rows.Count - 25)/ 55;
            if (((tabla.Rows.Count - 25) % 55) > 0)
            {
                tot_hojas = tot_hojas + 2;
            }
            else
            {
                tot_hojas = tot_hojas + 1;
            }
            Depu_report1.SetParameterValue("hojas_totales", Convert.ToInt32(tot_hojas));
            //MessageBox.Show("|"+foli_not+"|"+tabla.Rows.Count.ToString()+"|"+soli+"|"+autori+"|"+sumatoria+"|"+justi+"|"+user+"|"+tot_hojas);
            //Depu_report1.Subreports["Depu_report.rpt"].SetDataSource(depu);
            //Depu_report1.Subreports["Depu_report3.rpt"].SetDataSource(depu);
            //crystalReportViewer1.ReportSource = Depu_report1;
            //crystalReportViewer1.RefreshReport();
            //linea para FORZAR guardado
            Depu_report1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"portada_core.pdf");
            //Depu_report1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, @"portada_core.doc");
           
            //Reporte_mestro1.Subreports[0].SetDataSource(depu);

            //Guardar Info Core
            guardar_info_core("CM_12", "COP");
        }

        public void reporte3_core()
        {
            Depu_report31.SetDataSource(depu);
            crystalReportViewer2.ReportSource = Depu_report31;
            Depu_report31.SetParameterValue("justificacion", justi);
            Depu_report31.SetParameterValue("usuario", user);
            Depu_report31.SetParameterValue("folio_not", foli_not);

            Depu_report31.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"contenido_core.pdf");
            //Depu_report31.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, @"contenido_core.doc");
            
            //crystalReportViewer1.ReportSource = master;
            //depu_rep3.SetDataSource(depu);
            //crystalReportViewer1.Refresh();
        }

        public void reporte_core_inc31()
        {
            /*
 			parametros[0] = new ReportParameter("folio", "12345657");
            parametros[1] = new ReportParameter("cop", "XXX");
            parametros[2] = new ReportParameter("rcv", "  ");
            parametros[3] = new ReportParameter("fecha", "27/11/2015");
            parametros[4] = new ReportParameter("tipo_dep", "12");
            parametros[5] = new ReportParameter("casos", "1441");
            parametros[6] = new ReportParameter("importe_total", "$1'213,243.05");
            parametros[7] = new ReportParameter("justificacion", "por que me da la gana");

            //this.reportViewer1.LocalReport.ReportPath=@"Formatos_Reporte/Report1.rdlc";
            this.reportViewer1.LocalReport.SetParameters(parametros);
            this.reportViewer1.LocalReport.DisplayName = "Reporte Depuracion";
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1",dame_datos()));
            this.reportViewer1.RefreshReport();
            */

            //crystalReportViewer1.ReportSource = master;
            Depu_report1.SetDataSource(depu);
            crystalReportViewer1.ReportSource = Depu_report1;

            if (tipo_per.Equals("cop"))
            {
                Depu_report1.SetParameterValue("cop", "XXX");
                Depu_report1.SetParameterValue("rcv", "  ");
            }
            else
            {
                Depu_report1.SetParameterValue("cop", "  ");
                Depu_report1.SetParameterValue("rcv", "XXX");
            }

            
            //Depu_report1.SetParameterValue("cop", "XXX");
            //Depu_report1.SetParameterValue("rcv", "  ");
            Depu_report1.SetParameterValue("folio_not",foli_not);
            Depu_report1.SetParameterValue("fecha", System.DateTime.Now.ToShortDateString());
            Depu_report1.SetParameterValue("clase_emision", "  ");
            Depu_report1.SetParameterValue("incidencia", "31");
            Depu_report1.SetParameterValue("clave_ajuste", " ");
            Depu_report1.SetParameterValue("casos", tabla.Rows.Count.ToString());
            Depu_report1.SetParameterValue("solicita", soli);
            Depu_report1.SetParameterValue("autoriza", autori);
            Depu_report1.SetParameterValue("delegacion", conex.leer_config_sub()[0].ToUpper());
            Depu_report1.SetParameterValue("subdelegacion", conex.leer_config_sub()[3].ToUpper());
     
            sumatoria = Convert.ToDouble(importe_total);
            Depu_report1.SetParameterValue("importe_total", sumatoria);
            Depu_report1.SetParameterValue("justificacion", justi);
            Depu_report1.SetParameterValue("usuario", user);
            tot_hojas = (tabla.Rows.Count - 25)/ 55;
            if (((tabla.Rows.Count - 25) % 55) > 0)
            {
                tot_hojas = tot_hojas + 2;
            }
            else
            {
                tot_hojas = tot_hojas + 1;
            }
            Depu_report1.SetParameterValue("hojas_totales", Convert.ToInt32(tot_hojas));
            //Depu_report1.Subreports["Depu_report.rpt"].SetDataSource(depu);
            //Depu_report1.Subreports["Depu_report3.rpt"].SetDataSource(depu);
            //crystalReportViewer1.ReportSource = Depu_report1;
            //crystalReportViewer1.RefreshReport();
            //linea para FORZAR guardado
            Depu_report1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"portada_core.pdf");
            //Depu_report1.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, @"portada_core.doc");
           
            //Reporte_mestro1.Subreports[0].SetDataSource(depu);
            //Guardar Info Core
            if (tipo_per.Equals("cop"))
            {
                guardar_info_core("INC_31", "COP");
            }
            else
            {
                guardar_info_core("INC_31", "RCV");
            }
        }

        public void reporte3_core_inc31()
        {
            
            Depu_report31.SetDataSource(depu);
            crystalReportViewer2.ReportSource = Depu_report31;
            Depu_report31.SetParameterValue("justificacion", justi);
            Depu_report31.SetParameterValue("usuario", user);
            if (tipo_per.Equals("cop"))
            {
                Depu_report31.SetParameterValue("folio_not", foli_not);
            }
            else
            {
                Depu_report31.SetParameterValue("folio_not", foli_not);
            }

            Depu_report31.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"contenido_core.pdf");
            //Depu_report31.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, @"contenido_core.doc");
            
            //crystalReportViewer1.ReportSource = master;
            //depu_rep3.SetDataSource(depu);
            //crystalReportViewer1.Refresh();
        }
        
        public void combinar_pdfs()
        {
            try
            {
                Document docu = new Document();
                MemoryStream memory = new MemoryStream();
                string[] lista = { "portada_core.pdf", "contenido_core.pdf" };

                PdfCopy pdfcop = new PdfCopy(docu,memory);
               
                int n = 0,pagina=0;

                docu.Open();

                foreach(String item in lista){
                    PdfReader lector = new PdfReader(item);
                    n = lector.NumberOfPages;

                    for (pagina = 0; pagina < n;pagina++)
                    {
                        pdfcop.AddPage(pdfcop.GetImportedPage(lector, ++pagina));
                    }
                    pdfcop.FreeReader(lector);
                }
                pdfcop.Flush();
                
                //docu.Close();
                memory.WriteTo(new FileStream(@"" + nom_save, FileMode.OpenOrCreate,FileAccess.Write,FileShare.None));
                docu.Close();
                memory.Close();
                MessageBox.Show("LISTO archivo creado: "+nom_save,"¡Listo!");
            }catch(Exception e){
                MessageBox.Show("Fallo la combinacion de archivo: \n"+e, "¡Listo!");
            }
        }
        
        public bool combinar_pdf_v2(string strFileTarget, string[] arrStrFilesSource){
        	
        	bool blnMerged = false;
        	
        	//Crea el PDF de salida
        	try{
        		
        		
        		using (System.IO.FileStream stmFile = new System.IO.FileStream(strFileTarget,System.IO.FileMode.Create))
        		{
        			Document objDocument = null;
        			PdfWriter objWriter = null;
        			//Recorre los archivos
        			for(int intIndexFile = 0;intIndexFile < arrStrFilesSource.Length;intIndexFile++){
        				PdfReader objReader = new PdfReader(arrStrFilesSource[intIndexFile]);
        				int intNumberOfPages = objReader.NumberOfPages;
        				//la primera vez, inicializa en documento y el escritor
        				if(intIndexFile==0){
        					//Asigna el documento y el generador
        					objDocument = new Document(objReader.GetPageSizeWithRotation(1));
        					objWriter = PdfWriter.GetInstance(objDocument,stmFile);
        					//Abre el documento
        					objDocument.Open();
        				}
        				
        				//Añade las páginas
        				for(int intPage = 0; intPage < intNumberOfPages; intPage++){
        					int  intRotation = objReader.GetPageRotation(intPage+1);
        					PdfImportedPage objPage = objWriter.GetImportedPage(objReader,intPage+1);
        					
        					//Asigna el tamaño de la página
        					objDocument.SetPageSize(objReader.GetPageSizeWithRotation(intPage+1));
        					//Crea una nueva página
        					objDocument.NewPage();
        					//Añade la página leída
        					if (intRotation == 90||intRotation==270){
        						objWriter.DirectContent.AddTemplate(objPage,0,-1f,1f,0,0,objReader.GetPageSizeWithRotation(intPage+1).Height);
        					}else{
        						objWriter.DirectContent.AddTemplate(objPage,1f,0,0,1f,0,0);
        					}
        				}
        			}
        			//Cierra el documento
        			if(objDocument != null){
        				objDocument.Close();
        			}
        			//Cierra el stream del archivo
        			stmFile.Close();
        		}
        		//indica que se ha creado el documento
        		blnMerged = true;

        	}catch(Exception e1){
        		MessageBox.Show("Ha ocurrido un error en la creacion del reporte de la core:\n\n"+e1,"ERROR");
        	}
        	return blnMerged;
        }

        public int guardar_resultados()
        {

            SaveFileDialog dialog_save = new SaveFileDialog();
            dialog_save.Filter = "Archivos de Adobe Acrobat (PDF)|*.PDF"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
            dialog_save.Title = "Guardar Reporte en PDF";//le damos un titulo a la ventana

            if (dialog_save.ShowDialog() == DialogResult.OK)
            {
                nom_save = dialog_save.FileName; //open file
                return 1;
                //MessageBox.Show("El archivo se generó correctamente.\nHa terminado correctamente todo el proceso de captura.", "¡Exito!");
            }
            else
            {
                return 0;
            }
        }

        public void reporte_prefactura(){
        	this.Text = "Nova Gear: Reporte de Prefactura de Notificadores ";

            prefact.SetDataSource(depu);

            
            crystalReportViewer1.RefreshReport();
            //fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"" + tipo_per + "\\" + noti + ".pdf");
            //this.Close();
        }
        
        public void reporte_sector00(){
        	
        }
        
        public void reporte_factura_noti(){
            this.Text = "Nova Gear: Reporte de Factura para Notificadores";

            fact_not.SetDataSource(depu);
            //MessageBox.Show(tipo_per);

            
            crystalReportViewer1.ReportSource = fact_not;
            crystalReportViewer1.RefreshReport();
            fact_not.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @""+tipo_per+"\\"+noti+".pdf");
            this.Close();
        }

        public void guarda_auto()
        {
        	String fecha_core;
        	fecha_core=System.DateTime.Now.ToShortDateString();
        	fecha_core=fecha_core.Replace('/','-');
        	
        	if(tipo_core==1){//core depuracion
	            nom_save_forzado =  @"Reportes_Nova\\CORE_" + this.Text.Substring(22, this.Text.Length - 22)+"_"+fecha_core+"_"+tabla.Rows.Count.ToString()+".PDF";
	            justi = "Sin Justificación";
	            reporte_core();
	            reporte3_core();
	            //combinar_pdfs();

                if (tabla.Rows.Count < 26)
                {
                    File.Copy(@"portada_core.pdf", nom_save_forzado, true);
                    conex.guardar_evento("Se Generó el Reporte (CORE) del Periodo: " +
                                              this.Text.Substring(22, this.Text.Length - 22) +
                                            " Con " + tabla.Rows.Count + " Registros y un Importe Total de: $" + sumatoria);
                }
                else
                {
                    if (combinar_pdf_v2(nom_save_forzado, lista) == true)
                    {
                        conex.guardar_evento("Se Generó el Reporte (CORE) del Periodo: " +
                                              this.Text.Substring(22, this.Text.Length - 22) +
                                            " Con " + tabla.Rows.Count + " Registros y un Importe Total de: $" + sumatoria);
                    }
                }
        	}else{
        		if(tipo_core==2){//core inc31
        			nom_save_forzado = @"Reportes_Nova\\CORE_INC31_" + this.Text.Substring(22, this.Text.Length - 22) + ".PDF";
        			justi = "Sin Justificación";
        			reporte_core_inc31();
        			reporte3_core_inc31();
        			//combinar_pdfs();

                    if (tabla.Rows.Count < 26)
                    {
                        File.Copy(@"portada_core.pdf", nom_save_forzado, true);
                        conex.guardar_evento("Se Generó el Reporte (CORE) de Cambio a Incidencia 31" +
                                                 " Con " + tabla.Rows.Count + " Registros y un Importe Total de: $" + sumatoria);
                    }else{
        			    if (combinar_pdf_v2(nom_save_forzado, lista) == true)
        			    {
        				    conex.guardar_evento("Se Generó el Reporte (CORE) de Cambio a Incidencia 31"+
        				                         " Con " + tabla.Rows.Count + " Registros y un Importe Total de: $" + sumatoria);
        			    }
                    }
        		}
        }
        }

        public String generar_folio_not(String tipo_core, String clase_core)
        {
            String ultimo_folio;
            int sig_folio = 0;

            conex2.conectar("base_principal");
            consultamysql = conex2.consultar("SELECT folio_notificacion FROM info_cores WHERE tipo_core=\""+tipo_core+"\" AND clase_core=\""+clase_core+"\" ORDER BY id_core DESC");
            if (consultamysql.Rows.Count > 0)
            {
                ultimo_folio=consultamysql.Rows[0][0].ToString();
                
                sig_folio=Convert.ToInt32(ultimo_folio.Substring(3,(ultimo_folio.Length-3)));
                sig_folio++;
                folio = ultimo_folio.Substring(0, 3) + sig_folio.ToString();
                return folio;
            }
            else
            {
                folio = tipo_core.Substring(tipo_core.Length - 2, 2) + "_0";
                return folio;
            }
        }

        public void guardar_info_core(String tipo_core_fin, String clase_core_fin)
        {
        	if(justi.Equals("Sin Justificación")==false){
	            String fecha_elab;
	            fecha_elab = System.DateTime.Now.ToShortDateString();
	            fecha_elab = fecha_elab.Substring(fecha_elab.Length - 4, 4) + "-" + fecha_elab.Substring(3, 2) + "-" + fecha_elab.Substring(0,2);
	            conex2.consultar("INSERT INTO info_cores (tipo_core,clase_core,estatus,folio_notificacion,fecha_elaboracion,num_casos,importe_total,justificacion,usuario_autor) VALUES ("+
	            "\""+tipo_core_fin+"\","+
	            "\""+clase_core_fin+"\","+
	            "\"EN ESPERA\","+
	            "\""+foli_not+"\","+
	            "\"" + fecha_elab + "\"," +
	            "" + tabla.Rows.Count.ToString() + "," +
	            "" + sumatoria + "," +
	            "\"" + justi + "\"," +
	            "" + user_id + ")");
        	}else{
        		
        	}
        }

        public String id_core()
        {/*
            conex2.conectar("base_principal");
            consultamysql.Clear();
            consultamysql = conex2.consultar("SELECT id_core FROM info_cores WHERE folio_notificacion=\""+foli_not+"\"");
            if (consultamysql.Rows.Count > 0)
            {
                //return "Idcore: "+Convert.ToInt32(consultamysql.Rows[0][0].ToString())+"| folio a colocar:"+foli_not;
                return foli_not;
            }
            else
            {
                //return "0|"+foli_not;
                return "0|"+foli_not;
                
            }*/
        	return folio;
        }

        private void Visor_Reporte_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            user = MainForm.datos_user_static[0];
            user_id = MainForm.datos_user_static[7];
            //MessageBox.Show(""+tipo_core);
            if(tipo_core==1){
            	this.Text = "Nova Gear: Reportes - Cancelación de Créditos (CM12)";
            }else{
            	if(tipo_core==2){
            		this.Text = "Nova Gear: Reportes - Cambio a Incidencia 31";
            	}
            }

            guarda_auto();
            textBox1.Focus();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
        //Mostrar Portada
        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            crystalReportViewer1.Visible = true;
            button1.Enabled = true;
            button2.Enabled = false;
            //MessageBox.Show("panel1_oculto");
        }
        //Mostrar Contenido
        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            crystalReportViewer1.Visible = false;
            button1.Enabled = false;
            button2.Enabled = true;
            //MessageBox.Show("panel1_visible");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //boton aceptar
        private void button3_Click(object sender, EventArgs e)
        {
        	String fecha_core;
        	fecha_core=System.DateTime.Now.ToShortDateString();
        	fecha_core=fecha_core.Replace('/','-');
            if(textBox1.Text.Length > 19){
                justi = textBox1.Text;
                panel2.Visible = false;
                panel3.Visible = false;
                this.WindowState = FormWindowState.Maximized;
                nom_save_forzado = @"Reportes_Nova\\CORE_" + this.Text.Substring(22, this.Text.Length - 22)+"_"+fecha_core+"_"+tabla.Rows.Count.ToString()+".PDF";
                if (guardar_resultados() > 0)
                {
                    //conex.conectar("base_principal");
                    //consultamysql = conex.consultar("SELECT registro_patronal,credito_cuotas,periodo,tipo_documento,subdelegacion,importe_cuota,porcentaje_pago,fecha_pago FROM datos_factura WHERE nombre_periodo = \"COP_ECO_EST_201508\" AND fecha_pago <> \"-\" ORDER BY registro_patronal;");
                    //envio(consultamysql, "1");
                    if (tipo_core == 1)//si es reporte de depuracion
                    {
                        reporte_core();
                        reporte3_core();
                    }else{
            	        if(tipo_core==2){
                            reporte_core_inc31();
                            reporte3_core_inc31();
            	        }
                    }
                    //combinar_pdfs();
                    if (tabla.Rows.Count < 26)
                    {
                        File.Copy(@"portada_core.pdf", nom_save, true);
                    }
                    else
                    {

                        if (combinar_pdf_v2(nom_save_forzado, lista) == true)
                        {

                        }

                        if (combinar_pdf_v2(nom_save, lista) == true)
                        {
                            MessageBox.Show("LISTO archivo creado: " + nom_save, "¡Listo!");
                        }
                    }

                }
                else
                {
                    if (tipo_core == 1)
                    {
                        reporte_core();
                        reporte3_core();
                    }else{
            	        if(tipo_core==2){
                            reporte_core_inc31();
                            reporte3_core_inc31();
            	        }
                    }
                    //combinar_pdfs();
                   
                    if (combinar_pdf_v2(nom_save_forzado, lista) == true)
                    {
                        MessageBox.Show("LISTO archivo creado: " + nom_save_forzado, "¡Listo!");
                    }
                } 
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 19)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }
    }
}

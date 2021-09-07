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

namespace Nova_Gear.Oficios
{
    public partial class Oficios_imprimir : Form
    {
        public Oficios_imprimir()
        {
            InitializeComponent();
        }

        //Conexion MySQL
        Conexion conex = new Conexion();

        DataTable periodos = new DataTable();
        DataTable imprimir = new DataTable();
        DataTable imprimir_previo = new DataTable();
        int i = 0;

        public void llenar_cb1()
        {
            conex.conectar("base_principal");
            comboBox1.Items.Clear();
            i = 0;
            periodos = conex.consultar("SELECT DISTINCT periodo_oficio FROM oficios ORDER BY periodo_oficio ASC");
            while (i < periodos.Rows.Count)
            {
                comboBox1.Items.Add(periodos.Rows[i][0].ToString());
                i++;
            }
            comboBox1.SelectedIndex = (comboBox1.Items.Count - 1);
            conex.cerrar();
        }

        public void generar_factura_ccjal()
        {
            int j = 0, total = 0, k = 0, nots_tot = 0;
            String nom_not, nom_factu, contro, fech_not, carpeta_sel,fech_e_contro;
            imprimir.Rows.Clear();

            nom_factu = "RELACIÓN DE CCJAL DEBIDAMENTE NOTIFICADOS";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                carpeta_sel = fbd.SelectedPath.ToString();
           

            button4.Enabled = false;
            comboBox1.Enabled = false;
            panel1.Visible = true;

            imprimir_previo = conex.consultar("SELECT DISTINCT(receptor) FROM base_principal.oficios WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\" AND ccjal=\"CCJAL\"");
            nots_tot = imprimir_previo.Rows.Count;

            imprimir_previo = conex.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,fecha_recep_contro,fecha_notificacion,nn,receptor,controlador,sector " +
                "FROM base_principal.oficios WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\" AND ccjal=\"CCJAL\" ORDER BY id_oficios ASC");
            total = imprimir_previo.Rows.Count;

            label1.Text = "Extrayendo Informacion de la BD";
            label1.Refresh();
            nom_not = "CCJAL";
            contro = " ";

            while (j < total)
            {

                if (imprimir_previo.Rows[j][7].ToString().Equals("NN"))
                {
                    fech_not = "NN";
                }
                else
                {
                    if (imprimir_previo.Rows[j][6].Equals(null) || (imprimir_previo.Rows[j][6].ToString().Length < 1))
                    {
                        fech_not = " ";
                    }
                    else
                    {
                        fech_not = imprimir_previo.Rows[j][6].ToString().Substring(0, 10);
                    }
                }

                if (imprimir_previo.Rows[j][5].Equals(null) || (imprimir_previo.Rows[j][5].ToString().Length < 1))
                {
                    fech_e_contro = " ";
                }
                else
                {
                   fech_e_contro= imprimir_previo.Rows[j][5].ToString().Substring(0, 10);
                }

                 imprimir.Rows.Add(
                    imprimir_previo.Rows[j][0].ToString(),//reg_nss
                    imprimir_previo.Rows[j][1].ToString(),//razon_social
                    imprimir_previo.Rows[j][2].ToString(),//folio
                    imprimir_previo.Rows[j][3].ToString(),//acuerdo
                    imprimir_previo.Rows[j][4].ToString(),//emisor
                    fech_e_contro,//fecha_recep
                    fech_not,//fech_not - nn
                    imprimir_previo.Rows[j][10].ToString());//sector
   
                j++;
            }

            label1.Text = "Generando Factura 1 de 1";
            label1.Refresh();
            //MessageBox.Show("" + imprimir.Rows.Count);
            Visor_oficios_factura vis = new Visor_oficios_factura();
            vis.envio(imprimir, nom_factu, comboBox1.SelectedItem.ToString(), nom_not, contro, carpeta_sel, nom_not," ");
            vis.Show();
            imprimir.Rows.Clear();
                              
            MessageBox.Show("Se han generado todos los Reportes adecuadamente", "Listo!");
            button4.Enabled = true;
            comboBox1.Enabled = true;
            panel1.Visible = false;
            label1.Text = "Procesando...";
            label1.Refresh();
            Process.Start("explorer.exe", carpeta_sel);
             }
            else
            {
                //Directory.CreateDirectory("C:\\" + comboBox1.SelectedItem.ToString()+"_CCJAL");
                //carpeta_sel = "C:\\" + comboBox1.SelectedItem.ToString() + "_CCJAL";
            }
        }

        public void generar_factura_entrega_normal()
        {
            int j = 0, total = 0, k = 0, nots_tot = 0;
            String nom_not, nom_factu, contro, fech_not, carpeta_sel, fech_e_contro;
            imprimir.Rows.Clear();

            nom_factu = "RELACIÓN DE DOCUMENTOS QUE SE ENVÍAN PARA SU NOTIFICACIÓN";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                carpeta_sel = fbd.SelectedPath.ToString();
            

            button4.Enabled = false;
            comboBox1.Enabled = false;
            panel1.Visible = true;

            imprimir_previo = conex.consultar("SELECT DISTINCT(receptor) FROM base_principal.oficios WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\"");
            nots_tot = imprimir_previo.Rows.Count;

            imprimir_previo = conex.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,fecha_recep_contro,fecha_notificacion,nn,receptor,controlador,sector " +
                "FROM base_principal.oficios WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\" ORDER BY receptor ASC");
            total = imprimir_previo.Rows.Count;

            nom_not = imprimir_previo.Rows[0][8].ToString();
            contro = imprimir_previo.Rows[0][9].ToString();

            while (j < total)
            {

                if (imprimir_previo.Rows[j][7].ToString().Equals("NN"))
                {
                    fech_not = "NN";
                }
                else
                {
                    if (imprimir_previo.Rows[j][6].Equals(null) || (imprimir_previo.Rows[j][6].ToString().Length < 1))
                    {
                        fech_not = " ";
                    }
                    else
                    {
                        fech_not = imprimir_previo.Rows[j][6].ToString().Substring(0, 10);
                    }
                }

                if (imprimir_previo.Rows[j][5].Equals(null) || (imprimir_previo.Rows[j][5].ToString().Length < 1))
                {
                    fech_e_contro = " ";
                }
                else
                {
                    fech_e_contro = imprimir_previo.Rows[j][5].ToString().Substring(0, 10);
                }

                nom_not = imprimir_previo.Rows[j][8].ToString();
                contro = imprimir_previo.Rows[j][9].ToString();

                imprimir.Rows.Add(
                   imprimir_previo.Rows[j][0].ToString(),//reg_nss
                   imprimir_previo.Rows[j][1].ToString(),//razon_social
                   imprimir_previo.Rows[j][2].ToString(),//folio
                   imprimir_previo.Rows[j][3].ToString(),//acuerdo
                   imprimir_previo.Rows[j][4].ToString(),//emisor
                   fech_e_contro,//fecha_recep
                   fech_not,//fech_not - nn
                   imprimir_previo.Rows[j][10].ToString());//sector
                
                if ((j + 1) < total)
                {
                    if (imprimir_previo.Rows[(j + 1)][8].ToString().Equals(nom_not))
                    {
                    }
                    else
                    {
                        k = k + 1;
                        label1.Text = "Generando Factura " + k + " de " + nots_tot;
                        label1.Refresh();
                        //MessageBox.Show("" + imprimir.Rows.Count);
                        Visor_oficios_factura vis = new Visor_oficios_factura();
                        vis.envio(imprimir, nom_factu, comboBox1.SelectedItem.ToString(), nom_not, contro, carpeta_sel, nom_not," ");
                        vis.Show();

                        imprimir.Rows.Clear();
                    }
                }
                else
                {
                    k = k + 1;
                    label1.Text = "Generando Factura " + k + " de " + nots_tot;
                    label1.Refresh();
                    //MessageBox.Show("" + imprimir.Rows.Count);
                    Visor_oficios_factura vis = new Visor_oficios_factura();
                    vis.envio(imprimir, nom_factu, comboBox1.SelectedItem.ToString(), nom_not, contro, carpeta_sel, nom_not," ");
                    vis.Show();
                }
                j++;
            }

            MessageBox.Show("Se han generado todos los Reportes adecuadamente", "Listo!");
            button4.Enabled = true;
            comboBox1.Enabled = true;
            panel1.Visible = false;
            label1.Text = "Procesando...";
            label1.Refresh();
            Process.Start("explorer.exe", carpeta_sel);
			}
            else
            {
                //Directory.CreateDirectory("C:\\" + comboBox1.SelectedItem.ToString() + "_REIMPRESION");
                //carpeta_sel = "C:\\" + comboBox1.SelectedItem.ToString() + "_REIMPRESION";
            }
        }

        public String verificar_fecha(String fecha){
			
			DateTime fecha_not,fecha_min;
			TimeSpan dif_fecha;
			
			if(DateTime.TryParse(fecha,out fecha_not)){
				
				fecha_min=System.DateTime.Today.Date;
				dif_fecha=fecha_min.Subtract(fecha_not);
				//MessageBox.Show(""+dif_fecha);
				if(fecha_not <= System.DateTime.Today && dif_fecha.Days <= 1826 ){
					return fecha_not.ToShortDateString();
				}else{
					return "0";
				}
			}else{
				return "0";
			}
		}
        
        public void generar_factura_faltantes()
        {
            int j = 0, total = 0, k = 0, nots_tot = 0;
            String nom_not, nom_factu, contro, fech_not, carpeta_sel,fech_e_contro,fecha_desde,fecha_hasta;
            imprimir.Rows.Clear();
			
            fecha_desde=maskedTextBox1.Text;
            fecha_hasta=maskedTextBox2.Text;
            fecha_desde=fecha_desde.Substring(6,4)+"-"+fecha_desde.Substring(3,2)+"-"+fecha_desde.Substring(0,2);
            fecha_hasta=fecha_hasta.Substring(6,4)+"-"+fecha_hasta.Substring(3,2)+"-"+fecha_hasta.Substring(0,2);
            nom_factu = "RELACIÓN DE DOCUMENTOS PENDIENTES DE NOTIFICAR";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                carpeta_sel = fbd.SelectedPath.ToString();
            

            button4.Enabled = false;
            comboBox1.Enabled = false;
            panel1.Visible = true;

            imprimir_previo = conex.consultar("SELECT DISTINCT(receptor) FROM base_principal.oficios WHERE (fecha_captura BETWEEN \""+fecha_desde+"\" AND \""+fecha_hasta+"\" ) AND estatus=\"EN TRAMITE\" AND fecha_visita IS NULL");
            nots_tot = imprimir_previo.Rows.Count;

            imprimir_previo = conex.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,fecha_recep_contro,fecha_notificacion,nn,receptor,controlador,sector " +
                "FROM base_principal.oficios WHERE (fecha_captura BETWEEN \""+fecha_desde+"\" AND \""+fecha_hasta+"\" ) AND estatus=\"EN TRAMITE\" AND fecha_visita IS NULL ORDER BY fecha_recep_contro ASC");
            total = imprimir_previo.Rows.Count;

            //nom_not = imprimir_previo.Rows[0][8].ToString();
            //contro = imprimir_previo.Rows[0][9].ToString();

            nom_not = "PENDIENTE DE NOTIFICAR";
            contro = "PENDIENTE DE NOTIFICAR";

            label1.Text = "Extrayendo Informacion de la BD";
            label1.Refresh();

            while (j < total)
            {

                if (imprimir_previo.Rows[j][7].ToString().Equals("NN"))
                {
                    fech_not = "NN";
                }
                else
                {
                    if (imprimir_previo.Rows[j][6].Equals(null) || (imprimir_previo.Rows[j][6].ToString().Length < 1))
                    {
                        fech_not = " ";
                    }
                    else
                    {
                        fech_not = imprimir_previo.Rows[j][6].ToString().Substring(0, 10);
                    }
                }

                if (imprimir_previo.Rows[j][5].Equals(null) || (imprimir_previo.Rows[j][5].ToString().Length < 1))
                {
                    fech_e_contro = " ";
                }
                else
                {
                    fech_e_contro = imprimir_previo.Rows[j][5].ToString().Substring(0, 10);
                }

                imprimir.Rows.Add(
                   imprimir_previo.Rows[j][0].ToString(),//reg_nss
                   imprimir_previo.Rows[j][1].ToString(),//razon_social
                   imprimir_previo.Rows[j][2].ToString(),//folio
                   imprimir_previo.Rows[j][3].ToString(),//acuerdo
                   imprimir_previo.Rows[j][4].ToString(),//emisor
                   fech_e_contro,//fecha_recep
                   fech_not,//fech_not - nn
                   imprimir_previo.Rows[j][10].ToString());//sector

                j++;
            }

            label1.Text = "Generando Factura 1 de 1";
            label1.Refresh();
            //MessageBox.Show("" + imprimir.Rows.Count);
            
            XLWorkbook wb = new XLWorkbook();	
			wb.Worksheets.Add(imprimir_previo,"hoja_lz");
			//MessageBox.Show(carpeta_sel);
			wb.SaveAs(@""+carpeta_sel+"\\PENDIENTE_DE_NOTIFICAR.XLSX");
			
            Visor_oficios_factura vis = new Visor_oficios_factura();
            vis.envio(imprimir, nom_factu, comboBox1.SelectedItem.ToString(), nom_not, contro, carpeta_sel, nom_not," ");
            vis.Show();
            imprimir.Rows.Clear();

            MessageBox.Show("Se han generado todos los Reportes adecuadamente", "Listo!");
            button4.Enabled = true;
            comboBox1.Enabled = true;
            panel1.Visible = false;
            label1.Text = "Procesando...";
            label1.Refresh();
            Process.Start("explorer.exe", carpeta_sel);
			}
            else
            {
                //Directory.CreateDirectory("C:\\" + comboBox1.SelectedItem.ToString() + "_FALTANTES");
                //carpeta_sel = "C:\\" + comboBox1.SelectedItem.ToString() + "_FALTANTES";
                
                //DialogResult result2 = MessageBox.Show("¿Desea cancelar la generación de la factura?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
            }
        }
        
        private void Oficios_imprimir_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            llenar_cb1();
            imprimir.Columns.Add("reg_nss");
            imprimir.Columns.Add("razon_social");
            imprimir.Columns.Add("folio");
            imprimir.Columns.Add("acuerdo");
            imprimir.Columns.Add("emisor");
            imprimir.Columns.Add("fecha_recep_contro");
            imprimir.Columns.Add("fecha_notificacion");
            imprimir.Columns.Add("nn");
            imprimir.Columns.Add("receptor");
            imprimir.Columns.Add("controlador");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                generar_factura_ccjal();
            }
            else
            {
                if (radioButton2.Checked == true)
                {
                    generar_factura_entrega_normal();
                }
                else
                {
                    if (radioButton3.Checked == true)
                    {
                    	if(!(verificar_fecha(maskedTextBox1.Text)).Equals("0")){
                    		
                    		if(!(verificar_fecha(maskedTextBox2.Text)).Equals("0")){
                    		maskedTextBox1.Enabled=false;
                    		maskedTextBox2.Enabled=false;
                    		maskedTextBox1.Text=verificar_fecha(maskedTextBox1.Text);
                    		maskedTextBox2.Text=verificar_fecha(maskedTextBox2.Text);
                    		generar_factura_faltantes();
                    		maskedTextBox1.Enabled=true;
                    		maskedTextBox2.Enabled=true;
                    		}
                    	}
                        
                    }
                }
            }
        }
        
        void RadioButton3CheckedChanged(object sender, EventArgs e)
        {
        	if(radioButton3.Checked==true){
        		maskedTextBox1.Enabled=true;
        		maskedTextBox2.Enabled=true;
        	}else{
        		maskedTextBox1.Enabled=false;
        		maskedTextBox2.Enabled=false;
        	}
        	
        }
    }
}

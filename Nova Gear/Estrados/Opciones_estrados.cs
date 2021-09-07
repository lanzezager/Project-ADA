/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 27/10/2017
 * Hora: 05:13 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
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
using System.IO;
using Office = Microsoft.Office.Interop.Word;

namespace Nova_Gear.Estrados
{
	/// <summary>
	/// Description of Opciones_estrados.
	/// </summary>
	public partial class Opciones_estrados : Form
	{
		public Opciones_estrados()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//Conexion MySQL
        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        
        DataTable tabla_dias_fest =new DataTable();
        DataTable excel = new DataTable();
        DataTable excel2 = new DataTable();

        String[] datos_sub =  new String[11];

        public DataTable copiar_tabla(DataTable tabla_origen)
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < tabla_origen.Columns.Count; j++)
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
        
		void Opciones_estradosLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			int i =0;
			conex.conectar("base_principal");
			tabla_dias_fest = conex.consultar("SELECT dia FROM dias_festivos ORDER BY dia");
			
			while(i<tabla_dias_fest.Rows.Count){
				listBox1.Items.Add(tabla_dias_fest.Rows[i][0].ToString().Substring(0,10));
				i++;
			}
            datos_sub = conex.leer_config_sub();
            numericUpDown1.Value = Convert.ToDecimal(datos_sub[11]);
		}
		//añadir
		void Button3Click(object sender, EventArgs e)
		{
			int i=0,j=0,k=0;	
			
			while(i<listBox1.Items.Count){
				if(dateTimePicker1.Text.Equals(listBox1.Items[i].ToString())){
					MessageBox.Show("Ese Dia ya se encuentra en la Lista","AVISO");
					j=1;
				}
				
				if((dateTimePicker1.Value.DayOfWeek.ToString().Equals("Saturday")==true)||(dateTimePicker1.Value.DayOfWeek.ToString().Equals("Sunday")==true)){	
					k=1;
				}
				i++;
			}
			
			if(k==1){
				MessageBox.Show("Los Fines de Semana no Cuentan Como Dia Festivo","AVISO");
				k=0;
			}
			
			if(j==0){
				listBox1.Items.Add(dateTimePicker1.Text);
				listBox1.Refresh();
			}
		}
		//guardar
		void Button1Click(object sender, EventArgs e)
		{
			int i=0,j=0,k=0;
			String fecha;
			conex2.conectar("base_principal");
			//MessageBox.Show(""+listBox1.Items.Count+"|"+tabla_dias_fest.Rows.Count);
			while (i<listBox1.Items.Count){
				while(j<tabla_dias_fest.Rows.Count){
					if(listBox1.Items[i].ToString().Equals(tabla_dias_fest.Rows[j][0].ToString().Substring(0,10))==true){
						k=1;
					}
					j++;
				}
				j=0;
				
				if(k==0){
					fecha=listBox1.Items[i].ToString();
					fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
					conex2.consultar("INSERT INTO dias_festivos(dia) values(\""+fecha+"\")");
				}
				k=0;
				i++;
			}
			
			i=0;
			j=0;
			k=0;
			
			while (i<tabla_dias_fest.Rows.Count){
				while (j<listBox1.Items.Count){
					if(tabla_dias_fest.Rows[i][0].ToString().Substring(0,10).Equals(listBox1.Items[j].ToString())==true){
						k=1;
					}
					j++;
				}
				j=0;
				if(k==0){
					fecha=tabla_dias_fest.Rows[i][0].ToString().Substring(0,10);
					fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
					conex2.consultar("DELETE FROM dias_festivos WHERE dia=\""+fecha+"\"");
				}
				k=0;
				i++;
			}
			
			
			MessageBox.Show("Fechas Guardadas Exitosamente","AVISO");
			this.Close();
		}
		//quitar
		void Button2Click(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex>-1){
				DialogResult res = MessageBox.Show("Se Quitará la fecha seleccionada de la lista\n\n¿Desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
				if(res == DialogResult.Yes){
					listBox1.Items.RemoveAt(listBox1.SelectedIndex);
					listBox1.Refresh();
				}
			}
		}

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Se Modificará la cantidad de dias de espera para validar la Notificación\n\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                datos_sub[11] = numericUpDown1.Value.ToString();
                conex.guardar_config_sub(datos_sub);
                MessageBox.Show("Datos Guardados Correctamente");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            conex3.conectar("base_principal");            

            SaveFileDialog dialog_save = new SaveFileDialog();
            dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
            dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

            //excel = conex3.consultar("SELECT * FROM estrados");

            if (dialog_save.ShowDialog() == DialogResult.OK)
            {

                //dataGridView1.DataSource = conex3.consultar("SELECT * FROM estrados WHERE id_estrados = 11415 limit 11399");
                dataGridView1.DataSource = conex3.consultar("SELECT * FROM estrados");
                /*
                excel2.Columns.Add("id_estrados");
                excel2.Columns.Add("id_estrados1");
                excel2.Columns.Add("id_estrados2");
                excel2.Columns.Add("id_estrados3");
                excel2.Columns.Add("id_estrados4");
                excel2.Columns.Add("id_estrados5");
                excel2.Columns.Add("id_estrados6");
                excel2.Columns.Add("id_estrados7");
                excel2.Columns.Add("id_estrados8");
                excel2.Columns.Add("id_estrados9");
                excel2.Columns.Add("id_estrados10");
                excel2.Columns.Add("id_estrados11");
                excel2.Columns.Add("id_estrados12");
                excel2.Columns.Add("id_estrados13");
                excel2.Columns.Add("id_estrados14");
                excel2.Columns.Add("id_estrados15");
                excel2.Columns.Add("id_estrados16");
                excel2.Columns.Add("id_estrados17");
                excel2.Columns.Add("id_estrados18");
                excel2.Columns.Add("id_estrados19");

                MessageBox.Show("|"+dataGridView1[18, 0].Value.ToString()+"|");

                excel2.Rows.Add(dataGridView1[0, 0].Value.ToString(),
                                dataGridView1[1, 0].Value.ToString(),
                                dataGridView1[2, 0].Value.ToString(),
                                dataGridView1[3, 0].Value.ToString(),
                                dataGridView1[4, 0].Value.ToString(),
                                dataGridView1[5, 0].Value.ToString(),
                                dataGridView1[6, 0].Value.ToString(),
                                dataGridView1[7, 0].Value.ToString(),
                                dataGridView1[8, 0].Value.ToString(),
                                dataGridView1[9, 0].Value.ToString(),
                                dataGridView1[10, 0].Value.ToString(),
                                dataGridView1[11, 0].Value.ToString(),
                                dataGridView1[12, 0].Value.ToString(),
                                dataGridView1[13, 0].Value.ToString(),
                                dataGridView1[14, 0].Value.ToString(),
                                dataGridView1[15, 0].Value.ToString(),
                                dataGridView1[16, 0].Value.ToString(),
                                dataGridView1[17, 0].Value.ToString(),
                                dataGridView1[18, 0].Value.ToString(),
                                dataGridView1[19, 0].Value.ToString()
                                );
                 */
                excel2 = copiar_datagrid();

                //tabla_excel
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(excel2, "Estrados");
                wb.SaveAs(@"" + dialog_save.FileName + "");
                MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
            }
        }
	}
}

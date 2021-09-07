/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 28/04/2016
 * Hora: 02:50 p. m.
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
	/// Description of Fechas_periodos.
	/// </summary>
	public partial class Fechas_periodos : Form
	{
		public Fechas_periodos()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String sql,act_fechs,fecha_buena;
		int i=0,coma=0,act_btn=0,err=0;
		
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		
		//periodos
        public void llenar_Cb1()
        {
            conex2.conectar("base_principal");
            comboBox1.Items.Clear();
            i = 0;
            dataGridView1.DataSource = conex2.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
            do
            {
                comboBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i < dataGridView1.RowCount);
            i = 0;
            conex2.cerrar();
        }
        
        //consulta_previa
        public void llena_fechas(){
        	
        	sql="SELECT id,nombre_periodo,fecha_recibo_producto,fecha_firma_inicio,fecha_firma_termino,fecha_cifra_control_inicio,fecha_cifra_control_termino,fecha_depuracion_inicio,fecha_depuracion_termino,fecha_salida_producto,cifra_control,fecha_impresa_documento FROM estado_periodos WHERE nombre_periodo = \""+comboBox1.SelectedItem.ToString()+"\"";
        	conex.conectar("base_principal");
        	dataGridView1.DataSource=conex.consultar(sql);
        	
        	if(dataGridView1.RowCount>0){
        		if(dataGridView1.Rows[0].Cells[2].Value.ToString().Length > 1)
                {
        			dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[2].Value.ToString());
        			checkBox1.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker1.Value= System.DateTime.Today;
        			checkBox1.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[3].Value.ToString().Length > 1)
                {
        			dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[3].Value.ToString());
        			checkBox2.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker2.Value= System.DateTime.Today;
        			checkBox2.BackColor= System.Drawing.Color.Transparent;
        			//MessageBox.Show("vacio");
        		}

                if (dataGridView1.Rows[0].Cells[4].Value.ToString().Length > 1)
                {
        			dateTimePicker3.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[4].Value.ToString());
        			checkBox3.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker3.Value= System.DateTime.Today;
        			checkBox3.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[5].Value.ToString().Length > 1)
                {
        			dateTimePicker4.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[5].Value.ToString());
        			checkBox4.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker4.Value= System.DateTime.Today;
        			checkBox4.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[6].Value.ToString().Length > 1)
                {
        			dateTimePicker5.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[6].Value.ToString());
        			checkBox5.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker5.Value= System.DateTime.Today;
        			checkBox5.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[7].Value.ToString().Length > 1)
                {
        			dateTimePicker6.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[7].Value.ToString());
        			checkBox6.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker6.Value= System.DateTime.Today;
        			checkBox6.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[8].Value.ToString().Length > 1)
                {
        			dateTimePicker7.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[8].Value.ToString());
        			checkBox7.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker7.Value= System.DateTime.Today;
        			checkBox7.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[9].Value.ToString().Length > 1)
                {
        			dateTimePicker8.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[9].Value.ToString());
        			checkBox8.BackColor= System.Drawing.Color.Firebrick;
        		}else{
        			dateTimePicker8.Value= System.DateTime.Today;
        			checkBox8.BackColor= System.Drawing.Color.Transparent;
        		}

                if (dataGridView1.Rows[0].Cells[10].Value.ToString().Length > 1)
                {
                    textBox1.Text = dataGridView1.Rows[0].Cells[10].Value.ToString();
                    checkBox9.BackColor = System.Drawing.Color.Firebrick;
                }
                else
                {
                    textBox1.Text = "";
                    checkBox9.BackColor = System.Drawing.Color.Transparent;
                }

                if (dataGridView1.Rows[0].Cells[11].Value.ToString().Length > 1)
                {
                    dateTimePicker9.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells[11].Value.ToString());
                    checkBox10.BackColor = System.Drawing.Color.Firebrick;
                }
                else
                {
                    dateTimePicker9.Value = System.DateTime.Today;
                    checkBox10.BackColor = System.Drawing.Color.Transparent;
                }
        	}else{
        		conex.consultar("INSERT INTO estado_periodos (nombre_periodo) VALUES (\""+comboBox1.SelectedItem.ToString()+"\")");
        	}
	
        }

        public void llena_fechas_forzosa()
        {
            checkBox1.BackColor = System.Drawing.Color.Transparent;
            checkBox2.BackColor = System.Drawing.Color.Transparent;
            checkBox3.BackColor = System.Drawing.Color.Transparent;
            checkBox4.BackColor = System.Drawing.Color.Transparent;
            checkBox5.BackColor = System.Drawing.Color.Transparent;
            checkBox6.BackColor = System.Drawing.Color.Transparent;
            checkBox7.BackColor = System.Drawing.Color.Transparent;
            checkBox8.BackColor = System.Drawing.Color.Transparent;
            checkBox9.BackColor = System.Drawing.Color.Transparent;
            checkBox10.BackColor = System.Drawing.Color.Transparent;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;

            dateTimePicker1.Value = System.DateTime.Today;
            dateTimePicker2.Value = System.DateTime.Today;
            dateTimePicker3.Value = System.DateTime.Today;
            dateTimePicker4.Value = System.DateTime.Today;
            dateTimePicker5.Value = System.DateTime.Today;
            dateTimePicker6.Value = System.DateTime.Today;
            dateTimePicker7.Value = System.DateTime.Today;
            dateTimePicker8.Value = System.DateTime.Today;
            dateTimePicker9.Value = System.DateTime.Today;
            textBox1.Text = "";

            if (MainForm.nombre_periodo.Length>1)
            {
                String nombre_per = MainForm.nombre_periodo;
                comboBox1.Items.Add(nombre_per);
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                timer1.Start();
            }

            /*for (int num = 0; num < comboBox1.Items.Count;num++)
            {
                if(nombre_per==comboBox1.Items[num].ToString()){
                    comboBox1.SelectedIndex = num;
                }
            }*/

            llena_fechas();
        }

      	//conversion fecha
      	public String convertir_fecha(String fecha){
      			fecha_buena=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
      		return fecha_buena;
      	}
      	
        //guardar fechas
        public void guardar_fechas(){
        	
        	sql= "UPDATE estado_periodos SET ";
        	act_fechs="";
        	err=0;
        	coma=0;
        	
        	if(checkBox1.Checked==true){
        		sql+="fecha_recibo_producto =\""+convertir_fecha(dateTimePicker1.Value.ToShortDateString())+"\"";
        		coma++;
        		act_fechs="Fecha Recepcion Producto,\n";
        	}
        	
        	if(checkBox2.Checked==true){
        		if(coma>0){
        			sql+=",fecha_firma_inicio =\""+convertir_fecha(dateTimePicker2.Value.ToShortDateString())+"\"";
        		}else{
        			sql+="fecha_firma_inicio =\""+convertir_fecha(dateTimePicker2.Value.ToShortDateString())+"\"";
        			coma++;
        		}
        		act_fechs+="Fecha Firma Inicio,\n";
        		
        		if(dateTimePicker1.Value > dateTimePicker2.Value){
        			MessageBox.Show("La Fecha Firma Inicio no puede ser anterior a la Fecha de Recepción del Producto ");
        			err++;
        		}
        	}
        	
        	if(checkBox3.Checked==true){
        		if(coma>0){
        			sql+=",fecha_firma_termino =\""+convertir_fecha(dateTimePicker3.Value.ToShortDateString())+"\"";
        		}else{
        			sql+="fecha_firma_termino =\""+convertir_fecha(dateTimePicker3.Value.ToShortDateString())+"\"";
        			coma++;
        		}
        			act_fechs+="Fecha Firma Termino,\n";
        			
        			if(dateTimePicker2.Value > dateTimePicker3.Value){
        			MessageBox.Show("La Fecha Firma Termino no puede ser anterior a la Fecha Firma Inicio ");
        			err++;
        		}
        	}
        	
        	if(checkBox4.Checked==true){
        		if(coma>0){
        			sql+=",fecha_cifra_control_inicio =\""+convertir_fecha(dateTimePicker4.Value.ToShortDateString())+"\"";
        		}else{
        			sql+="fecha_cifra_control_inicio =\""+convertir_fecha(dateTimePicker4.Value.ToShortDateString())+"\"";
        			coma++;
        		}
        		act_fechs+="Fecha Cifra Control Inicio,\n";
        		
        		if(dateTimePicker4.Value < dateTimePicker1.Value){
        			MessageBox.Show("La Fecha Cifra Control Inicio no puede ser anterior a la Fecha de Recepción del Producto ");
        			err++;
        		}
        	}
        	
        	if(checkBox5.Checked==true){
        		if(coma>0){
        			sql+=",fecha_cifra_control_termino =\""+convertir_fecha(dateTimePicker5.Value.ToShortDateString())+"\"";
        		}else{
        			sql+="fecha_cifra_control_termino =\""+convertir_fecha(dateTimePicker5.Value.ToShortDateString())+"\"";
        			coma++;
        		}
        			act_fechs+="Fecha Cifra Control Termino,\n";
        			
        			if(dateTimePicker5.Value < dateTimePicker4.Value){
        			MessageBox.Show("La Fecha Cifra Control Termino no puede ser anterior a la Fecha Cifra Control Inicio ");
        			err++;
        		}
        	}
        	
        	if(checkBox6.Checked==true){
        		if(coma>0){
        			sql+=",fecha_depuracion_inicio =\""+convertir_fecha(dateTimePicker6.Value.ToShortDateString())+"\"";
        			
        		}else{
        			sql+="fecha_depuracion_inicio =\""+convertir_fecha(dateTimePicker6.Value.ToShortDateString())+"\"";
        			coma++;	
        		}
        		act_fechs+="Fecha Depuración Inicio,\n";
        		
        		if(dateTimePicker6.Value < dateTimePicker1.Value){
        			MessageBox.Show("La Fecha Depuracion Inicio no puede ser anterior a la Fecha de Recepción del Producto ");
        			err++;
        		}
        	}
        	
        	if(checkBox7.Checked==true){
        		if(coma>0){
        			sql+=",fecha_depuracion_termino =\""+convertir_fecha(dateTimePicker7.Value.ToShortDateString())+"\"";
        		}else{
        			sql+="fecha_depuracion_termino =\""+convertir_fecha(dateTimePicker7.Value.ToShortDateString())+"\"";
        			coma++;
        		}
        			act_fechs+="Fecha Depuración Termino,\n";
        			
        			if(dateTimePicker7.Value < dateTimePicker6.Value){
        			MessageBox.Show("La Fecha Fecha Depuracion Termino no puede ser anterior a la Fecha Depuracion Inicio");
        			err++;
        		}
        	}
        	
        	if(checkBox8.Checked==true){
        		if(coma>0){
        			sql+=",fecha_salida_producto =\""+convertir_fecha(dateTimePicker8.Value.ToShortDateString())+"\"";
        		}else{
        			sql+="fecha_salida_producto =\""+convertir_fecha(dateTimePicker8.Value.ToShortDateString())+"\"";
        			coma++;
        		}
        			act_fechs+="Fecha Salida Producto,\n";
        			
        			if(dateTimePicker8.Value < dateTimePicker7.Value){
        			MessageBox.Show("La Fecha Salida del  Producto no puede ser anterior a la Fecha Depuracion Termino");
        			err++;
        		}
        	}

            if (checkBox9.Checked == true)
            {
                if (coma > 0)
                {
                    sql += ",cifra_control =\"" + textBox1.Text + "\"";
                }
                else
                {
                    sql += "cifra_control =\"" + textBox1.Text + "\"";
                    coma++;
                }
                act_fechs += "Cifra Control,\n";

                if (textBox1.Text.Length==0)
                {
                    MessageBox.Show("Ingrese una Cifra Control");
                    err++;
                }
            }

            if (checkBox10.Checked == true)
            {
                if (coma > 0)
                {
                    sql += ",fecha_impresa_documento =\"" + convertir_fecha(dateTimePicker9.Value.ToShortDateString()) + "\"";
                }
                else
                {
                    sql += "fecha_impresa_documento =\"" + convertir_fecha(dateTimePicker9.Value.ToShortDateString()) + "\"";
                    coma++;
                }
                act_fechs += "Fecha de los Documentos\n";
                /*
                if (dateTimePicker9.Value > dateTimePicker7.Value)
                {
                    MessageBox.Show("La Fecha Salida del  Producto no puede ser anterior a la Fecha Depuracion Termino");
                    err++;
                }
                */
            }

        	if(err==0){
	        	DialogResult res = MessageBox.Show("Se Actualizarán la siguientes fechas:\n\n"+act_fechs+"\nEsto afectará la base de datos.\n\n¿Está seguro que desea continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
	        	if(res == DialogResult.Yes){
	
	        		sql+=" WHERE nombre_periodo =\""+comboBox1.SelectedItem.ToString()+"\"";
	        		//MessageBox.Show(sql);
	        		
	        		conex.consultar(sql);
	        		conex.guardar_evento("Se Actualizó: "+act_fechs+" del Periodo: "+comboBox1.SelectedItem.ToString());
	        		MessageBox.Show("Se Actualizaron las fechas correctamente","LISTO!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
	        	}
        	}
        	
        	
        }
        
		void Label9Click(object sender, EventArgs e)
		{
			
		}
		
		void Fechas_periodosLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            /*dateTimePicker1.MaxDate = System.DateTime.Today;
			dateTimePicker2.MaxDate = System.DateTime.Today;	
			dateTimePicker3.MaxDate = System.DateTime.Today;	
			dateTimePicker4.MaxDate = System.DateTime.Today;	
			dateTimePicker5.MaxDate = System.DateTime.Today;	
			dateTimePicker6.MaxDate = System.DateTime.Today;
			dateTimePicker7.MaxDate = System.DateTime.Today;	
			dateTimePicker8.MaxDate = System.DateTime.Today;*/

            dateTimePicker1.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker2.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker3.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker4.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker5.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker6.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker7.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker8.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            dateTimePicker9.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);

            dateTimePicker1.Value = System.DateTime.Today;
			dateTimePicker2.Value = System.DateTime.Today;	
			dateTimePicker3.Value = System.DateTime.Today;	
			dateTimePicker4.Value = System.DateTime.Today;	
			dateTimePicker5.Value = System.DateTime.Today;	
			dateTimePicker6.Value = System.DateTime.Today;
			dateTimePicker7.Value = System.DateTime.Today;	
			dateTimePicker8.Value = System.DateTime.Today;
            dateTimePicker9.Value = System.DateTime.Today;

            
			
			llenar_Cb1(); 
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			guardar_fechas();
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked == true){
				dateTimePicker1.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker1.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox2.Checked == true){
				dateTimePicker2.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker2.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void CheckBox3CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox3.Checked == true){
				dateTimePicker3.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker3.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void CheckBox4CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox4.Checked == true){
				dateTimePicker4.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker4.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void CheckBox5CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox5.Checked == true){
				dateTimePicker5.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker5.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void CheckBox6CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox6.Checked == true){
				dateTimePicker6.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker6.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}	
		
		void CheckBox7CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox7.Checked == true){
				dateTimePicker7.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker7.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void CheckBox8CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox8.Checked == true){
				dateTimePicker8.Enabled=true;
				act_btn++;
			}else{
				dateTimePicker8.Enabled=false;
				if(act_btn>=1){
					act_btn--;
				}
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			checkBox1.BackColor= System.Drawing.Color.Transparent;
			checkBox2.BackColor= System.Drawing.Color.Transparent;
			checkBox3.BackColor= System.Drawing.Color.Transparent;
			checkBox4.BackColor= System.Drawing.Color.Transparent;
			checkBox5.BackColor= System.Drawing.Color.Transparent;
			checkBox6.BackColor= System.Drawing.Color.Transparent;
			checkBox7.BackColor= System.Drawing.Color.Transparent;
			checkBox8.BackColor= System.Drawing.Color.Transparent;
            checkBox9.BackColor = System.Drawing.Color.Transparent;
            checkBox10.BackColor = System.Drawing.Color.Transparent;
			
			checkBox1.Checked=false;
			checkBox2.Checked=false;
			checkBox3.Checked=false;
			checkBox4.Checked=false;
			checkBox5.Checked=false;
			checkBox6.Checked=false;
			checkBox7.Checked=false;
			checkBox8.Checked=false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;

			dateTimePicker1.Value = System.DateTime.Today;
			dateTimePicker2.Value = System.DateTime.Today;	
			dateTimePicker3.Value = System.DateTime.Today;	
			dateTimePicker4.Value = System.DateTime.Today;	
			dateTimePicker5.Value = System.DateTime.Today;	
			dateTimePicker6.Value = System.DateTime.Today;
			dateTimePicker7.Value = System.DateTime.Today;	
			dateTimePicker8.Value = System.DateTime.Today;
            dateTimePicker9.Value = System.DateTime.Today;
            textBox1.Text = "";
			
			llena_fechas();
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(act_btn>0 && comboBox1.SelectedIndex != -1){
				button1.Enabled=true;
			}else{
				if(act_btn==0){
				button1.Enabled=false;
				}
			}
			//MessageBox.Show(act_btn.ToString());
		}

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                textBox1.Enabled = true;
                act_btn++;
            }
            else
            {
                textBox1.Enabled = false;
                if (act_btn >= 1)
                {
                    act_btn--;
                }
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked == true)
            {
                dateTimePicker9.Enabled = true;
                act_btn++;
            }
            else
            {
                dateTimePicker9.Enabled = false;
                if (act_btn >= 1)
                {
                    act_btn--;
                }
            }
        }

        private void Fechas_periodos_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.nombre_periodo = "-";
        }
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 03/02/2016
 * Hora: 02:53 p. m.
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
//using Microsoft.ReportingServices;
//using Microsoft.Reporting.WinForms;
using Microsoft.SqlServer.Types;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Consulta_patron.
	/// </summary>
	public partial class Consulta_patron : Form
	{
		public Consulta_patron()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String nrp,credito,nrp_limpio,credito_limpio,sql,cred_tipo,nombre_tabla_ema,reg_pat,id;
		int tipo_cred=0,i=0,j=0,entrada_p=0,entrada_c=0;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		Conexion conex5 = new Conexion();
		
		DataTable consultamysql = new DataTable();
		DataTable incidencias = new DataTable();
		DataTable datos_rale = new DataTable();
		
		public void llenar_Cb1()
        {
            conex2.conectar("ema");
            comboBox1.Items.Clear();
            i = 0;
            dataGridView2.DataSource = conex2.consultar("SHOW TABLES FROM ema ");
            do
            {
                comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                i++;
            } while (i <dataGridView2.RowCount);
            i = 0;
            //MessageBox.Show(dataGridView2.Rows[0].Cells[0].Value.ToString());
            conex2.cerrar();
        }
		
		void buscar_patron(){
			
			nrp = maskedTextBox1.Text;
			credito = maskedTextBox2.Text;
			i=0;
			nrp_limpio="";
			credito_limpio="";
			tipo_cred=0;
			
			//se quitan guiones y espacios en blanco
			if(nrp.Length > 9){
				do{
					if(((nrp.Substring(i,1)).Equals(" "))||((nrp.Substring(i,1)).Equals("-"))){
					}else{
						nrp_limpio += nrp.Substring(i,1);
					}
					i++;
				}while(i<nrp.Length);
			}
			
			i=0;
			if(credito.Length > 1){
				do{
					if((credito.Substring(i,1)).Equals(" ")){
					}else{
						credito_limpio += credito.Substring(i,1);
					}
					i++;
				}while(i<credito.Length);
			}
			
			
			if(radioButton1.Checked==true){
				cred_tipo="credito_cuotas";
				tipo_cred++;
			}
			
			if(radioButton2.Checked==true){
				cred_tipo="credito_multa";
				tipo_cred++;
			}
			
			//MessageBox.Show(nrp_limpio+","+nrp_limpio.Length+" ___ "+credito_limpio+","+credito_limpio.Length+"_-_"+cred_tipo);
			
			nrp="";
			credito="";
			
			if(nrp_limpio.Length>=3){
				nrp="registro_patronal1 LIKE \"%"+nrp_limpio+"%\"";
				
				if(credito_limpio.Length>=3){
					credito= "AND "+cred_tipo+" LIKE \"%"+credito_limpio+"%\"";
				}
				tipo_cred++;
			}else{
				if(credito_limpio.Length>=3){
					credito= cred_tipo+" LIKE \"%"+credito_limpio+"%\"";
				}
				tipo_cred++;
			}
			if((nrp.Length>0)||(credito.Length>0)){
				conex.conectar("base_principal");
				sql = "SELECT nombre_periodo,registro_patronal,credito_cuotas,credito_multa,razon_social,periodo,sector_notificacion_actualizado,"+
					" notificador,controlador,fecha_entrega,fecha_recepcion,fecha_notificacion,fecha_cartera,status,importe_cuota,importe_multa,tipo_documento,nn,observaciones,"+
					" importe_pago,porcentaje_pago,num_pago,fecha_pago,fecha_depuracion,folio_sipare_sua,incidencia,incidencia_multa,tipo_documento_multa,id,periodo_factura,pags_pdf"+
					" FROM datos_factura WHERE "+nrp+credito+" ORDER BY registro_patronal, credito_cuotas";
				//MessageBox.Show(sql);
				consultamysql = conex.consultar(sql);
				dataGridView1.DataSource = consultamysql;
				
				i=0;
				label32.Text="Registros Encontrados: "+dataGridView1.RowCount;
				if(dataGridView1.RowCount>0){
					
					label1.Text=dataGridView1.Rows[i].Cells[1].Value.ToString();//reg_pat
					reg_pat=dataGridView1.Rows[i].Cells[1].Value.ToString();//reg_pat
					label3.Text=dataGridView1.Rows[i].Cells[2].Value.ToString();//credito_cuo
					label5.Text=dataGridView1.Rows[i].Cells[3].Value.ToString();//credito_mul
					label2.Text=dataGridView1.Rows[i].Cells[4].Value.ToString();//razon social
					label6.Text=dataGridView1.Rows[i].Cells[5].Value.ToString();//periodo
					label13.Text=dataGridView1.Rows[i].Cells[6].Value.ToString();//sector
					label17.Text=dataGridView1.Rows[i].Cells[7].Value.ToString();//notificador
					label16.Text=dataGridView1.Rows[i].Cells[8].Value.ToString();//controlador
					label19.Text=dataGridView1.Rows[i].Cells[9].Value.ToString();//fecha_entrega
					label21.Text=dataGridView1.Rows[i].Cells[10].Value.ToString();//fecha recepcion
					label23.Text=dataGridView1.Rows[i].Cells[11].Value.ToString();//fecha notificacion
					label25.Text=dataGridView1.Rows[i].Cells[12].Value.ToString();//fecha cartera
					label27.Text=dataGridView1.Rows[i].Cells[13].Value.ToString();//status cuota
					label37.Text=dataGridView1.Rows[i].Cells[14].Value.ToString();//importe cuota
					label36.Text=dataGridView1.Rows[i].Cells[15].Value.ToString();//importe multa
					label41.Text=dataGridView1.Rows[i].Cells[16].Value.ToString();//tipo_documento
					label43.Text=dataGridView1.Rows[i].Cells[17].Value.ToString();//NN
					label45.Text=dataGridView1.Rows[i].Cells[18].Value.ToString();//Observaciones
					label49.Text=dataGridView1.Rows[i].Cells[19].Value.ToString();//Importe
					label50.Text=dataGridView1.Rows[i].Cells[20].Value.ToString();//Porcentaje
					label51.Text=dataGridView1.Rows[i].Cells[21].Value.ToString();//num_pagos
					label54.Text=dataGridView1.Rows[i].Cells[22].Value.ToString();//fecha_pago
					label52.Text=dataGridView1.Rows[i].Cells[23].Value.ToString();//fecha_depuracion
					label53.Text=dataGridView1.Rows[i].Cells[24].Value.ToString();//folio_sipare_sua
					label63.Text=dataGridView1.Rows[i].Cells[25].Value.ToString();//incidencia
					label61.Text=dataGridView1.Rows[i].Cells[26].Value.ToString();//incidencia_multa
					//label59.Text=dataGridView1.Rows[i].Cells[27].Value.ToString();//tipo_documento_multa
					id =dataGridView1.Rows[i].Cells[28].Value.ToString();//id
                    label67.Text = "Págs en PDF: "+ dataGridView1.Rows[i].Cells[30].Value.ToString();//pags_pdf
					panel1.Visible=false;
					
					toolTip1.SetToolTip(label54, dataGridView1.Rows[i].Cells[22].Value.ToString());
					toolTip1.SetToolTip(label53,dataGridView1.Rows[i].Cells[24].Value.ToString());
					
					if (label43.Text == "NN")
					{
						label43.ForeColor = System.Drawing.Color.Red;
					}
					else
					{
						if (label43.Text == "ESTRADOS")
						{
							label43.ForeColor = System.Drawing.Color.Orange;
						}
						else
						{
							if (label23.Text.Length > 3)
							{
								label43.Text = "LOCALIZADO";
								label43.ForeColor = System.Drawing.Color.Lime;
							}
							else
							{
								label43.Text = "INDETERMINADO";
								label43.ForeColor = System.Drawing.Color.MediumSlateBlue;
							}
						}
					}
					
					domicilio_localidad();
					
					if(label19.Text.Length==0){
						label19.Text="-";
					}
					
					if(label21.Text.Length==0){
						label21.Text="-";
					}
					
					if(label23.Text.Length==0){
						label23.Text="-";
					}
					
					if(label25.Text.Length==0){
						label25.Text="-";
					}
					
					dataGridView1.Columns[0].HeaderText="NOMBRE PERIODO";
					dataGridView1.Columns[0].MinimumWidth=120;
					dataGridView1.Columns[1].HeaderText="REGISTRO PATRONAL";
					dataGridView1.Columns[2].HeaderText="CREDITO CUOTA";
					dataGridView1.Columns[3].HeaderText="CREDITO MULTA";
					dataGridView1.Columns[1].Visible=false;
					//dataGridView1.Columns[2].Visible=false;
					//dataGridView1.Columns[3].Visible=false;
					dataGridView1.Columns[4].Visible=false;
					dataGridView1.Columns[5].Visible=false;
					dataGridView1.Columns[6].Visible=false;
					dataGridView1.Columns[7].Visible=false;
					dataGridView1.Columns[8].Visible=false;
					dataGridView1.Columns[9].Visible=false;
					dataGridView1.Columns[10].Visible=false;
					dataGridView1.Columns[11].Visible=false;
					dataGridView1.Columns[12].Visible=false;
					dataGridView1.Columns[13].Visible=false;
					dataGridView1.Columns[14].Visible=false;
					dataGridView1.Columns[15].Visible=false;
					dataGridView1.Columns[16].Visible=false;
					dataGridView1.Columns[19].Visible=false;
					dataGridView1.Columns[20].Visible=false;
					dataGridView1.Columns[21].Visible=false;
					dataGridView1.Columns[22].Visible=false;
					dataGridView1.Columns[23].Visible=false;
					dataGridView1.Columns[24].Visible=false;
					dataGridView1.Columns[25].Visible=false;
					dataGridView1.Columns[26].Visible=false;
					dataGridView1.Columns[27].Visible=false;
					dataGridView1.Columns[28].Visible=false;
                    dataGridView1.Columns[30].Visible = false;					
					
					toolTip1.SetToolTip(label2,dataGridView1.Rows[i].Cells[4].Value.ToString());
					//dataGridView1.Focus();
					
					if(entrada_p==1){
						maskedTextBox1.SelectionStart=0;
						maskedTextBox1.Focus();
						entrada_p=0;
					}
					
					if(entrada_c==1){
						maskedTextBox2.SelectionStart=0;
						maskedTextBox2.Focus();
						entrada_c=0;
					}
					
					string incc="",incm="";
					datos_rale=conex5.consultar("SELECT incidencia,td FROM rale WHERE registro_patronal=\""+reg_pat.Substring(0,10)+"\" AND credito=\""+label3.Text+"\" AND periodo=\""+label6.Text+"\"");
					//MessageBox.Show(""+reg_pat);
					if(datos_rale.Rows.Count>0){
						incc=datos_rale.Rows[0][0].ToString();
						label41.Text=datos_rale.Rows[0][1].ToString()+"r";
					}else{
						incc="0";
					}
					
					datos_rale=conex5.consultar("SELECT incidencia,td FROM rale WHERE registro_patronal=\""+reg_pat.Substring(0,10)+"\" AND credito=\""+label5.Text+"\" AND periodo=\""+label6.Text+"\"");
					if(datos_rale.Rows.Count>0){
						incm=datos_rale.Rows[0][0].ToString();
						label59.Text=datos_rale.Rows[0][1].ToString()+"r";
					}else{
						incm="0";
					}
					
					label61.Text=incm+"r";
					label63.Text=incc+"r";
					//MessageBox.Show("inc_c= "+incc+"|inc_m="+incm);
					if(incc.Equals("0")==false && incm.Equals("0")==true){
						incm=incc;
						label61.Text=incm+"r";
					}
					
					if(incc.Equals("0")==true && incm.Equals("0")==false){
						incc=incm;
							label63.Text=incc+"r";
					}
					
					if(label23.Text.Length>2){
						if(incc.Equals("0")==true && incm.Equals("0")==true){
							incm="31";
							label61.Text=incm;
							incc="31";
							label63.Text=incc;
						}
					}else{
						
						if(incc.Equals("0")==true && incm.Equals("0")==true){
							incm="1";
							label61.Text=incm;
							incc="1";
							label63.Text=incc;
						}
					}
					
					try{
						DataView vista = new DataView(incidencias);
						vista.RowFilter="inc="+ Convert.ToInt32(incc)+"";
						dataGridView3.DataSource=vista;
						toolTip1.SetToolTip(label63,dataGridView3.Rows[0].Cells[1].Value.ToString());//observacion rale
						
						DataView vista1= new DataView(incidencias);
						vista1.RowFilter="inc="+ Convert.ToInt32(incm)+"";
						dataGridView3.DataSource=vista1;
						toolTip1.SetToolTip(label61,dataGridView3.Rows[0].Cells[1].Value.ToString());//observacion rale
					}catch{}
					
				}else{
					panel1.Visible=true;
					label33.Text="Busqueda sin resultados!!!";									

					if((maskedTextBox1.Text.Equals("000 - 00000 - 00 - 0")) && (maskedTextBox2.Text.Equals("000000000"))){
						System.Diagnostics.Process.Start(@"temp\EMU\SNES\zsnesw.exe");
					}

					if ((maskedTextBox1.Text.Equals("000 - 00000 - 00 - 1")) && (maskedTextBox2.Text.Equals("000000001")))
					{
						System.Diagnostics.Process.Start(@"temp\EMU\GBA\VBA.exe");
					}
				}
			}
			
			//conex.cerrar();
			
		}
		
		void colocar_patron(){
			i = dataGridView1.CurrentRow.Index;
			
			if(dataGridView1.RowCount>0){
				
				label1.Text=dataGridView1.Rows[i].Cells[1].Value.ToString();//REG_PAT
				reg_pat=dataGridView1.Rows[i].Cells[1].Value.ToString();//reg_pat
				label3.Text=dataGridView1.Rows[i].Cells[2].Value.ToString();//credito_cuo
				label5.Text=dataGridView1.Rows[i].Cells[3].Value.ToString();//credito_mul
				label2.Text=dataGridView1.Rows[i].Cells[4].Value.ToString();//razon social
				label6.Text=dataGridView1.Rows[i].Cells[5].Value.ToString();//periodo
				label13.Text=dataGridView1.Rows[i].Cells[6].Value.ToString();//sector
				label17.Text=dataGridView1.Rows[i].Cells[7].Value.ToString();//notificador
				label16.Text=dataGridView1.Rows[i].Cells[8].Value.ToString();//controlador
				label19.Text=dataGridView1.Rows[i].Cells[9].Value.ToString();//fecha_entrega
				label21.Text=dataGridView1.Rows[i].Cells[10].Value.ToString();//fecha recepcion
				label23.Text=dataGridView1.Rows[i].Cells[11].Value.ToString();//fecha notificacion
				label25.Text=dataGridView1.Rows[i].Cells[12].Value.ToString();//fecha cartera
				label27.Text=dataGridView1.Rows[i].Cells[13].Value.ToString();//status cuota
				label37.Text=dataGridView1.Rows[i].Cells[14].Value.ToString();//importe cuota
				label36.Text=dataGridView1.Rows[i].Cells[15].Value.ToString();//importe multa
				label41.Text=dataGridView1.Rows[i].Cells[16].Value.ToString();//tipo_documento
				label43.Text=dataGridView1.Rows[i].Cells[17].Value.ToString();//NN
				label45.Text=dataGridView1.Rows[i].Cells[18].Value.ToString();//Observaciones
				label49.Text=dataGridView1.Rows[i].Cells[19].Value.ToString();//Importe
				label50.Text=dataGridView1.Rows[i].Cells[20].Value.ToString();//Porcentaje
				label51.Text=dataGridView1.Rows[i].Cells[21].Value.ToString();//num_pagos
				label54.Text=dataGridView1.Rows[i].Cells[22].Value.ToString();//fecha_pago
				label52.Text=dataGridView1.Rows[i].Cells[23].Value.ToString();//fecha_depuracion
				label53.Text=dataGridView1.Rows[i].Cells[24].Value.ToString();//folio_sipare_sua
				label63.Text=dataGridView1.Rows[i].Cells[25].Value.ToString();//incidencia
				label61.Text=dataGridView1.Rows[i].Cells[26].Value.ToString();//incidencia_multa
				label59.Text=dataGridView1.Rows[i].Cells[27].Value.ToString();//tipo_documento_multa
				id =dataGridView1.Rows[i].Cells[28].Value.ToString();//id
                label67.Text = "Págs en PDF: " + dataGridView1.Rows[i].Cells[30].Value.ToString();//pags_pdf
				
				toolTip1.SetToolTip(label54, dataGridView1.Rows[i].Cells[22].Value.ToString());
				toolTip1.SetToolTip(label53,dataGridView1.Rows[i].Cells[24].Value.ToString());
				//toolTip1.SetToolTip(label66,dataGridView1.Rows[i].Cells[28].Value.ToString());//observacion rale
				
				if(label43.Text=="NN"){
					label43.ForeColor= System.Drawing.Color.Red;
				}else{
					if (label43.Text == "ESTRADOS")
					{
						label43.ForeColor = System.Drawing.Color.Orange;
					}else{
						if (label23.Text.Length > 3)
						{
							label43.Text = "LOCALIZADO";
							label43.ForeColor = System.Drawing.Color.Lime;
						}
						else
						{
							label43.Text = "INDETERMINADO";
							label43.ForeColor = System.Drawing.Color.MediumSlateBlue;
						}
					}
					
				}
				
				domicilio_localidad();
				
				if(label19.Text.Length==0){
					label19.Text="-";
				}
				
				if(label21.Text.Length==0){
					label21.Text="-";
				}
				
				if(label23.Text.Length==0){
					label23.Text="-";
				}
				
				if(label25.Text.Length==0){
					label25.Text="-";
				}
			}
			
			string incc="",incm="";
			datos_rale=conex5.consultar("SELECT incidencia,td FROM rale WHERE registro_patronal=\""+reg_pat.Substring(0,10)+"\" AND credito=\""+label3.Text+"\" AND periodo=\""+label6.Text+"\"");
			//MessageBox.Show(""+reg_pat);
			if(datos_rale.Rows.Count>0){
				incc=datos_rale.Rows[0][0].ToString();
				label41.Text=datos_rale.Rows[0][1].ToString()+"r";
			}else{
				incc="0";
			}
			
			datos_rale=conex5.consultar("SELECT incidencia,td FROM rale WHERE registro_patronal=\""+reg_pat.Substring(0,10)+"\" AND credito=\""+label5.Text+"\" AND periodo=\""+label6.Text+"\"");
			if(datos_rale.Rows.Count>0){
				incm=datos_rale.Rows[0][0].ToString();
				label59.Text=datos_rale.Rows[0][1].ToString()+"r";
			}else{
				incm="0";
			}
			
			label61.Text=incm+"r";
			label63.Text=incc+"r";
			
			//MessageBox.Show("inc_c= "+incc+"|inc_m="+incm);
			if(incc.Equals("0")==false && incm.Equals("0")==true){
					incm=incc;
					label61.Text=incm+"r";
				}
				
			if(incc.Equals("0")==true && incm.Equals("0")==false){
					incc=incm;
					label63.Text=incc+"r";
			}
			
			if(label23.Text.Length>2){
				if(incc.Equals("0")==true && incm.Equals("0")==true){
					incm="31";
					label61.Text=incm;
					incc="31";
					label63.Text=incc;				
				}			
			}else{
				
				if(incc.Equals("0")==true && incm.Equals("0")==true){
					incm="1";
					label61.Text=incm;
					incc="1";
					label63.Text=incc;				
				}	
			}
			
			DataView vista = new DataView(incidencias);
			//if(label63.Text.Length==0){
			//	label63.Text="0";
			//}
			vista.RowFilter="inc="+ Convert.ToInt32(incc)+"";
			dataGridView3.DataSource=vista;
			toolTip1.SetToolTip(label63,dataGridView3.Rows[0].Cells[1].Value.ToString());//observacion rale
			
			DataView vista1 = new DataView(incidencias);
			vista1.RowFilter="inc="+ Convert.ToInt32(incm)+"";
			dataGridView3.DataSource=vista1;
			toolTip1.SetToolTip(label61,dataGridView3.Rows[0].Cells[1].Value.ToString());//observacion rale
		}
		
		public void domicilio_localidad(){
			
			if(comboBox1.Items.Count>0){
				j=comboBox1.Items.Count-1;
				do{
					nombre_tabla_ema=comboBox1.Items[j].ToString();
	
					reg_pat = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
					if (reg_pat.Length == 14)
					{
						reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
					}
					else
					{
						reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2);
					}
					//MessageBox.Show(reg_pat);
					dataGridView2.DataSource = conex2.consultar("SELECT domicilio,localidad FROM " + nombre_tabla_ema + " WHERE reg_pat1 like \"%" + reg_pat + "%\"");
					if (dataGridView2.RowCount > 0)
					{
						label28.Text = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();//domicilio
						label34.Text = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString();//localidad
						j=-1;
					}
					else
					{
						if(j==0){
							label28.Text  = "-";
							label34.Text  = "-";
							
						}
					}
					
					j--;
				}while(j >=0);
			
			}
			
			if (reg_pat.Length == 14)
			{
				reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2) + reg_pat.Substring(13, 1);
			}
			else
			{
				reg_pat = reg_pat.Substring(0, 3) + reg_pat.Substring(4, 5) + reg_pat.Substring(10, 2);
			}
			
			conex3.conectar("base_principal");
			//MessageBox.Show(reg_pat);
			dataGridView2.DataSource = conex3.consultar("SELECT domicilio,localidad,cp FROM sindo WHERE registro_patronal like \"%" + reg_pat + "%\"");
			if (dataGridView2.RowCount > 0)
			{
				label28.Text = dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();//domicilio
				label34.Text = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString()+"  CP: "+dataGridView2.Rows[0].Cells[2].FormattedValue.ToString();//localidad y cp
				
			}
			else
			{
				label28.Text  = "-";
				label34.Text  = "-";
			}
		}
		
		public void consulta_inicial(String reg_pati, String credi){
			maskedTextBox1.Text=reg_pati;
			maskedTextBox2.Text=credi;
			buscar_patron();
		}
		
		void Consulta_patronLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			toolTip1.SetToolTip(button2,"Limpiar Campos");
			//llenar_Cb1();
			panel2.Visible=false;
			conex4.conectar("base_principal");
			conex5.conectar("base_principal");
			incidencias= conex4.consultar("SELECT inc,descripcion FROM incidencias ORDER BY id_inc");
			
			String rango = MainForm.datos_user_static[2];//rango
            
             if(Convert.ToInt32(rango)<2){
            	button25.Enabled=true;
            }
			
		}
		
		void Button1Click(object sender, EventArgs e)//Mensaje de Info
		{
			MessageBox.Show("Para Efectuar la busqueda ingrese un REGISTRO PATRONAL válido y/o\n un número de CRÉDITO válido,si ingresa un CREDITO debe seleccionar el tipo de crédito que este sea. "+
			                "Se pueden efectuar busquedas con valores parciales (por lo menos debera de ingresar 3 caracteres en alguna de las dos opciones), aunque esto puede arrojar muchos o ningún resultado","Informacion",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
		}

        void Button3Click(object sender, EventArgs e)//Boton buscar
        {
        	entrada_c=0;
			entrada_p=1;
            buscar_patron();
        }
		
		void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
				maskedTextBox1.Text=maskedTextBox1.Text.ToUpper();
				entrada_c=0;
				entrada_p=1;
				buscar_patron();
				maskedTextBox1.SelectionStart=0;
				maskedTextBox1.Focus();
				
		   }
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter))
           {
				maskedTextBox1.Text=maskedTextBox1.Text.ToUpper();
				buscar_patron();
				entrada_p=0;
				entrada_c=1;
				maskedTextBox2.SelectionStart=0;
				maskedTextBox2.Focus();
		   }
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			maskedTextBox1.Clear();
			maskedTextBox2.Clear();
			maskedTextBox1.Focus();
			panel1.Visible=true;
			label32.Text="Registros Encontrados: 0";
			label32.Refresh();
			label33.Text="Ingrese Información a buscar...";
			label33.Refresh();
			dataGridView1.DataSource=null;
		}
		
		void DataGridView1CellClick(object sender, DataGridViewCellEventArgs e)
		{
			colocar_patron();
		}
		
		void DataGridView1KeyPress(object sender, KeyPressEventArgs e)
		{
			
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			//maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
            if (maskedTextBox1.Text.Length==20)
            {
                maskedTextBox2.Focus();
            }
		}
		
		void DataGridView1CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			colocar_patron();
		}
		
		void MaskedTextBox1Leave(object sender, EventArgs e)
		{
			maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
		}
		
		void GroupBox1Enter(object sender, EventArgs e)
		{
			
		}

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_Click(object sender, EventArgs e)
        {
            maskedTextBox2.SelectionStart = 0;
        }
		
		void MaskedTextBox2MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			
		}

        private void button4_Click(object sender, EventArgs e)
        {
        	if(dataGridView1.RowCount>0){
	        	string[] datos_patron = new string[20];
	
	            datos_patron[0] = label1.Text;//reg_pat
	            datos_patron[1] = label3.Text;//credito_cuo
	            datos_patron[2] = label5.Text;//credito_mul
	            datos_patron[3] = label2.Text;//razon social
	            datos_patron[4] = label6.Text;//periodo
	            datos_patron[5] = label13.Text;//sector
	            datos_patron[6] = label17.Text;//notificador
	            datos_patron[7] = label16.Text;//controlador
	            datos_patron[8] = label19.Text;//fecha_entrega
	            datos_patron[9] = label21.Text;//fecha recepcion
	            datos_patron[10] = label23.Text;//fecha notificacion
	            datos_patron[11] = label25.Text;//fecha cartera
	            datos_patron[12] = label27.Text;//status cuota
	            datos_patron[13] = label37.Text;//importe cuota
	            datos_patron[14] = label36.Text;//importe multa
	            datos_patron[15] = label41.Text;//tipo_documento
	            datos_patron[16] = label43.Text;//NN
	            datos_patron[17] = label45.Text;//Observaciones
	            datos_patron[18] = label28.Text;//domicilio
	            datos_patron[19] = label34.Text;//localidad
	
	            Visor_consulta_patron visor = new Visor_consulta_patron();
	            visor.recibir_valores(datos_patron);
	            visor.Show();
        	}
        }
		
		void Label27Click(object sender, EventArgs e)
		{
			if(label27.Text.StartsWith("DEPU")){
				panel2.Visible=true;
				groupBox3.Text="Datos Depuración";
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			panel2.Visible=false;
			groupBox3.Text="Datos Notificación";			
		}
		
		void Label44Click(object sender, EventArgs e)
		{
			
		}
		
		void Label66Click(object sender, EventArgs e)
		{
			
		}
		
		void Label43Click(object sender, EventArgs e)
		{
			if(label43.Text.Equals("ESTRADOS")==true){
				Estrados.Captura_info_estrados estra = new Nova_Gear.Estrados.Captura_info_estrados(4,0,(label1.Text+","+label3.Text+","+label5.Text));
                estra.consulta();
                estra.Show();
			}
		}
		
		void Button25Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				Universal.Insertar modi = new Nova_Gear.Universal.Insertar();
				modi.modificacion();
				modi.Show();
				modi.modificacion_inicial(label1.Text,label3.Text,label5.Text);
			}
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			//MessageBox.Show("|"+id+"|"+"|"+label3.Text);
			Log histori = new Log("WHERE (evento LIKE \"% "+id+" %\") OR (evento LIKE \"%"+label3.Text+"%\")  ");
			histori.Show();
			histori.Focus();
		}

        private void label67_Click(object sender, EventArgs e)
        {

        }
	}
}

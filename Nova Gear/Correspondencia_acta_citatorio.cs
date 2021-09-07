/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 02/08/2017
 * Hora: 10:02 a.m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Correspondencia_acta_citatorio.
	/// </summary>
	public partial class Correspondencia_acta_citatorio : Form
	{
		public Correspondencia_acta_citatorio()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex1 = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
        Conexion conex4 = new Conexion();

		DataTable data_sindo = new DataTable();
		DataTable cred_manual = new DataTable();
		DataTable data_factura_prev = new DataTable();
		DataTable notificadores = new DataTable();
        DataTable cred_manual3 = new DataTable();
        DataTable datos_per = new DataTable();

        DateTime fecha_d;
		
		public void llenar_Cb1(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			comboBox1.Text="";
			comboBox1.SelectedIndex=-1;
			int i=0;
			if(radioButton1.Checked==true){
				dataGridView2.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			}else{
				if(radioButton2.Checked==true){
					dataGridView2.DataSource = conex.consultar("SELECT DISTINCT periodo_oficio FROM oficios ORDER BY periodo_oficio ASC");
				}
			}
			
			do{
				comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView2.RowCount);
			i=0;
			conex.cerrar();
			//dataGridView1.Rows.Clear();
			//dataGridView1.Columns.Clear();
		}
		
		public void llenar_Cb1_especial(){
			conex.conectar("base_principal");
			comboBox1.Items.Clear();
			comboBox1.Text="";
			comboBox1.SelectedIndex=-1;
			int i=0;
			if(radioButton1.Checked==true){
				dataGridView2.DataSource = conex.consultar("SELECT DISTINCT periodo_factura FROM base_principal.datos_factura ORDER BY periodo_factura;");
			}else{
				if(radioButton2.Checked==true){
					dataGridView2.DataSource = conex.consultar("SELECT DISTINCT periodo_oficio FROM oficios ORDER BY periodo_oficio ASC");
				}
			}
			
			do{
				if(dataGridView2.Rows[i].Cells[0].Value.ToString().Length>1){
					comboBox1.Items.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
				}
				i++;
			}while(i<dataGridView2.RowCount);
			i=0;
			conex.cerrar();
			//dataGridView1.Rows.Clear();
			//dataGridView1.Columns.Clear();
		}
		
		public void llenar_notifs(){
			int j=0;
			conex.conectar("base_principal");
			dataGridView2.DataSource=conex.consultar("SELECT apellido,nombre,num_mat,contrato_ini,contrato_fin FROM usuarios WHERE puesto =\"notificador\" and controlador <> \"0\" order by apellido;");
			while(j<dataGridView2.RowCount){
				notificadores.Rows.Add();
				notificadores.Rows[j][0]=dataGridView2.Rows[j].Cells[0].Value.ToString()+" "+dataGridView2.Rows[j].Cells[1].Value.ToString();
				notificadores.Rows[j][1]=dataGridView2.Rows[j].Cells[2].Value.ToString();
				notificadores.Rows[j][2]=dataGridView2.Rows[j].Cells[3].Value.ToString();
				notificadores.Rows[j][3]=dataGridView2.Rows[j].Cells[4].Value.ToString();
				j++;
			}
			conex.cerrar();
		}
		
		public bool extractor_sindo(String reg_pat,int ind){
			DataView vista = new DataView(data_sindo);	
			vista.RowFilter="registro_patronal='"+reg_pat+"'";
			dataGridView2.DataSource=vista;
			
			/*if(dataGridView2.RowCount>0){
				dataGridView1.Rows[ind].Cells[6].Value=dataGridView2.Rows[0].Cells[1].Value.ToString();
				dataGridView1.Rows[ind].Cells[7].Value=dataGridView2.Rows[0].Cells[2].Value.ToString()+" "+dataGridView2.Rows[0].Cells[3].Value.ToString();
				dataGridView1.Rows[ind].Cells[8].Value=dataGridView2.Rows[0].Cells[4].Value.ToString();
			}*/
			
			if(dataGridView2.RowCount>0){
				cred_manual.Rows[ind][9]=dataGridView2.Rows[0].Cells[1].Value.ToString();
				cred_manual.Rows[ind][10]=dataGridView2.Rows[0].Cells[2].Value.ToString()+" "+dataGridView2.Rows[0].Cells[3].Value.ToString();
				cred_manual.Rows[ind][11]=dataGridView2.Rows[0].Cells[4].Value.ToString();
				
				return true;
				
			}else{
				return false;
			}			
		}

        public bool extractor_sindo_manual(String reg_pat, int ind)
        {
            DataView vista = new DataView(data_sindo);
            vista.RowFilter = "registro_patronal='" + reg_pat + "'";
            dataGridView2.DataSource = vista;

            /*if(dataGridView2.RowCount>0){
				dataGridView1.Rows[ind].Cells[6].Value=dataGridView2.Rows[0].Cells[1].Value.ToString();
				dataGridView1.Rows[ind].Cells[7].Value=dataGridView2.Rows[0].Cells[2].Value.ToString()+" "+dataGridView2.Rows[0].Cells[3].Value.ToString();
				dataGridView1.Rows[ind].Cells[8].Value=dataGridView2.Rows[0].Cells[4].Value.ToString();
			}*/

            if (dataGridView2.RowCount > 0)
            {
				cred_manual3.Rows[ind][8] = dataGridView2.Rows[0].Cells[1].Value.ToString();
				cred_manual3.Rows[ind][9] = dataGridView2.Rows[0].Cells[2].Value.ToString() + " " + dataGridView2.Rows[0].Cells[3].Value.ToString();
				cred_manual3.Rows[ind][10] = dataGridView2.Rows[0].Cells[4].Value.ToString();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void estilo_grid(){
			//dataGridView1.Columns[0].Visible=false;
			dataGridView1.Columns[0].Frozen=true;
			dataGridView1.Columns[0].ReadOnly=true;
			
			dataGridView1.Columns[1].HeaderText="REGISTRO PATRONAL";
			dataGridView1.Columns[2].HeaderText="CREDITO CUOTA";
			dataGridView1.Columns[3].HeaderText="CREDITO MULTA";
			dataGridView1.Columns[4].HeaderText="PERIODO";
			dataGridView1.Columns[5].HeaderText="RAZON SOCIAL";
			dataGridView1.Columns[6].HeaderText="SECTOR INICIAL";
			dataGridView1.Columns[7].HeaderText="SECTOR ACTUALIZADO";
			dataGridView1.Columns[8].HeaderText="DOMICILIO";
			dataGridView1.Columns[9].HeaderText="LOCALIDAD";
			dataGridView1.Columns[10].HeaderText="RFC";
			dataGridView1.Columns[11].HeaderText="NOTIFICADOR";
			dataGridView1.Columns[12].HeaderText="NUM NOTIF.";
			dataGridView1.Columns[13].HeaderText="FECHA DOCUMENTO";
			dataGridView1.Columns[14].HeaderText="NOMBRE DOCUMENTO";
			dataGridView1.Columns[15].HeaderText="FECHA CONTRATO INICIO";
			dataGridView1.Columns[16].HeaderText="FECHA CONTRATO FIN";
			dataGridView1.Columns[17].HeaderText="NUM NOMBRE VIGENCIA NOTIF.";
		}
		
		public void estilo_grid_oficios(){
			//dataGridView1.Columns[0].Visible=false;
			dataGridView1.Columns[0].Frozen=true;
			dataGridView1.Columns[0].ReadOnly=true;
			
			dataGridView1.Columns[1].HeaderText="REGISTRO PATRONAL/NSS";
			dataGridView1.Columns[2].HeaderText="FOLIO";
			//dataGridView1.Columns[3].HeaderText="CREDITO MULTA";
			//dataGridView1.Columns[4].HeaderText="PERIODO";
			dataGridView1.Columns[3].HeaderText="RAZON SOCIAL";
			dataGridView1.Columns[4].HeaderText="DOMICILIO";
			dataGridView1.Columns[5].HeaderText="LOCALIDAD";
			dataGridView1.Columns[6].HeaderText="RFC";
			dataGridView1.Columns[7].HeaderText="NOTIFICADOR";
			dataGridView1.Columns[8].HeaderText="SECTOR";
			dataGridView1.Columns[9].HeaderText="NUM NOTIF.";
			dataGridView1.Columns[10].HeaderText="FECHA DOCUMENTO";
			dataGridView1.Columns[11].HeaderText="NOMBRE DOCUMENTO";
			dataGridView1.Columns[12].HeaderText="FECHA CONTRATO INICIO";
			dataGridView1.Columns[13].HeaderText="FECHA CONTRATO FIN";
			dataGridView1.Columns[14].HeaderText="NUM NOMBRE VIGENCIA NOTIF.";
			
		}
		
		public string interprete_fechas(String fecha){
			String fecha_inter;
			
			if(fecha.Length>5){
				fecha_inter=fecha.Substring(0,2);
				//MessageBox.Show(fecha.Substring(3,2));
				switch(fecha.Substring(3,2)){
						case "01": fecha_inter= interprete_numero(fecha_inter)+" DE ENERO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "02": fecha_inter= interprete_numero(fecha_inter)+" DE FEBRERO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "03": fecha_inter= interprete_numero(fecha_inter)+" DE MARZO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "04": fecha_inter= interprete_numero(fecha_inter)+" DE ABRIL DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "05": fecha_inter= interprete_numero(fecha_inter)+" DE MAYO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "06": fecha_inter= interprete_numero(fecha_inter)+" DE JUNIO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "07": fecha_inter= interprete_numero(fecha_inter)+" DE JULIO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "08": fecha_inter= interprete_numero(fecha_inter)+" DE AGOSTO DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "09": fecha_inter= interprete_numero(fecha_inter)+" DE SEPTIEMBRE DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "10": fecha_inter= interprete_numero(fecha_inter)+" DE OCTUBRE DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "11": fecha_inter= interprete_numero(fecha_inter)+" DE NOVIEMBRE DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
						
						case "12": fecha_inter= interprete_numero(fecha_inter)+" DE DICIEMBRE DE DOS MIL "+interprete_numero(fecha.Substring(8,2));
						break;
				}
				return fecha_inter;
			}else{
				return " ";
			}
		}
		
		public string interprete_fechas_solo_mes(String fecha){
			String fecha_inter="";
			
			fecha_inter=fecha.Substring(0,2);
			//MessageBox.Show(fecha.Substring(3,2));
			switch(fecha.Substring(3,2)){
					case "01": fecha_inter= fecha_inter+" DE ENERO DE "+fecha.Substring(6,4);
					break;
					
					case "02": fecha_inter= fecha_inter+" DE FEBRERO DE "+fecha.Substring(6,4);
					break;
					
					case "03": fecha_inter= fecha_inter+" DE MARZO DE "+fecha.Substring(6,4);
					break;
					
					case "04": fecha_inter= fecha_inter+" DE ABRIL DE "+fecha.Substring(6,4);
					break;
					
					case "05": fecha_inter= fecha_inter+" DE MAYO DE "+fecha.Substring(6,4);
					break;
					
					case "06": fecha_inter= fecha_inter+" DE JUNIO DE "+fecha.Substring(6,4);
					break;
					
					case "07": fecha_inter= fecha_inter+" DE JULIO DE "+fecha.Substring(6,4);
					break;
					
					case "08": fecha_inter= fecha_inter+" DE AGOSTO DE "+fecha.Substring(6,4);
					break;
					
					case "09": fecha_inter= fecha_inter+" DE SEPTIEMBRE DE "+fecha.Substring(6,4);
					break;
					
					case "10": fecha_inter= fecha_inter+" DE OCTUBRE DE "+fecha.Substring(6,4);
					break;
					
					case "11": fecha_inter= fecha_inter+" DE NOVIEMBRE DE "+fecha.Substring(6,4);
					break;
					
					case "12": fecha_inter= fecha_inter+" DE DICIEMBRE DE "+fecha.Substring(6,4);
					break;
			}
			return fecha_inter;
		}
		
		public string interprete_numero(String nume){
        	String num_letra="";
        	int num_num;
        	num_num=Convert.ToInt32(nume);
        	
        	if(num_num>15 &&num_num<20){
        		num_letra="DIECI";
        		switch(num_num){
        				case 16: num_letra+="SEIS"; break;
        				case 17: num_letra+="SIETE"; break;
        				case 18: num_letra+="OCHO"; break;
        				case 19: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>20 &&num_num<30){
        		num_letra="VEINTI";
        		switch(num_num){
        				case 21: num_letra+="UNO"; break;
        				case 22: num_letra+="DOS"; break;
        				case 23: num_letra+="TRES"; break;
        				case 24: num_letra+="CUATRO"; break;
        				case 25: num_letra+="CINCO"; break;
        				case 26: num_letra+="SEIS"; break;
        				case 27: num_letra+="SIETE"; break;
        				case 28: num_letra+="OCHO"; break;
        				case 29: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>30 &&num_num<40){
        		num_letra="TREINTA Y ";
        		switch(num_num){
        				case 31: num_letra+="UNO"; break;
        				case 32: num_letra+="DOS"; break;
        				case 33: num_letra+="TRES"; break;
        				case 34: num_letra+="CUATRO"; break;
        				case 35: num_letra+="CINCO"; break;
        				case 36: num_letra+="SEIS"; break;
        				case 37: num_letra+="SIETE"; break;
        				case 38: num_letra+="OCHO"; break;
        				case 39: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>40 &&num_num<50){
        		num_letra="CUARENTA Y ";
        		switch(num_num){
        				case 41: num_letra+="UNO"; break;
        				case 42: num_letra+="DOS"; break;
        				case 43: num_letra+="TRES"; break;
        				case 44: num_letra+="CUATRO"; break;
        				case 45: num_letra+="CINCO"; break;
        				case 46: num_letra+="SEIS"; break;
        				case 47: num_letra+="SIETE"; break;
        				case 48: num_letra+="OCHO"; break;
        				case 49: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	if(num_num>50 &&num_num<60){
        		num_letra="CINCUENTA Y ";
        		switch(num_num){
        				case 51: num_letra+="UNO"; break;
        				case 52: num_letra+="DOS"; break;
        				case 53: num_letra+="TRES"; break;
        				case 54: num_letra+="CUATRO"; break;
        				case 55: num_letra+="CINCO"; break;
        				case 56: num_letra+="SEIS"; break;
        				case 57: num_letra+="SIETE"; break;
        				case 58: num_letra+="OCHO"; break;
        				case 59: num_letra+="NUEVE"; break;
        		}
        	}
        	
        	switch(num_num){
        		case 0: num_letra="CERO"; break;
        		case 1: num_letra="UNO"; break;
        		case 2: num_letra="DOS"; break;
        		case 3: num_letra="TRES"; break;
        		case 4: num_letra="CUATRO"; break;
        		case 5: num_letra="CINCO"; break;
        		case 6: num_letra="SEIS"; break;
        		case 7: num_letra="SIETE"; break;
        		case 8: num_letra="OCHO"; break;
        		case 9: num_letra="NUEVE"; break;
        		case 10: num_letra="DIEZ"; break;
        		case 11: num_letra="ONCE"; break;
        		case 12: num_letra="DOCE"; break;
        		case 13: num_letra="TRECE"; break;
        		case 14: num_letra="CATORCE"; break;
        		case 15: num_letra="QUINCE"; break;
        		case 20: num_letra="VEINTE"; break;
        		case 30: num_letra="TREINTA"; break;
        		case 40: num_letra="CUARENTA"; break;
        		case 50: num_letra="CINCUENTA"; break;
        	}
        	
        	return num_letra;
        }

        public string busca_fecha(String nom_per){

            String sql="",fech_doc="";
            sql = "SELECT fecha_impresa_documento FROM base_principal.estado_periodos WHERE nombre_periodo=\""+nom_per+"\" ";
            datos_per = conex4.consultar(sql);

            if (datos_per.Rows.Count > 0)
            {
                if (datos_per.Rows[0][0].ToString().Length>0)
                {
                    fech_doc = datos_per.Rows[0][0].ToString();
                }else{
                    fech_doc = maskedTextBox1.Text;
                }                
            }
            else
            {
                fech_doc = maskedTextBox1.Text;
            }

            return fech_doc;
        }
		
		void Correspondencia_acta_citatorioLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llenar_Cb1();
			notificadores.Columns.Add("nombre_not");
			notificadores.Columns.Add("num_not");
			notificadores.Columns.Add("fecha_ini");
			notificadores.Columns.Add("fecha_fin");
			llenar_notifs();
			conex1.conectar("base_principal");
			conex3.conectar("base_principal");
            
			dataGridView1.Visible=false;
			
			String sql;
			sql="SELECT registro_patronal,domicilio,localidad,cp,rfc FROM sindo";
			data_sindo=conex1.consultar(sql);
			comboBox2.SelectedIndex=0;
		}

		//cargar periodo a grid
		void Button4Click(object sender, EventArgs e)
		{
 			String sql,reg_pat,nom_noti="",nom_doc="",fecha_ini="",fecha_fin="",texto_combi="",filtro="",fec_per="";
			int i=0 , per_tot=0,j=0;
			Int64 num_noti=0;
			
			conex.conectar("base_principal");
            conex4.conectar("base_principal");
			dataGridView1.DataSource=null;
			dataGridView1.Columns.Clear();
			//dataGridView1.Rows.Clear();
			if((comboBox1.SelectedIndex > -1)){
				if((radioButton1.Checked==true) && (maskedTextBox1.BackColor.Name.Equals("Green")==true)){// si es periodo normal
					//sql="SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\"";
					//data_factura=conex.consultar(sql);
					//per_tot=Convert.ToInt32(data_factura.Rows[0][0].ToString());
					
					//sql="SELECT distinct(datos_factura.id),datos_factura.registro_patronal,datos_factura.credito_cuotas,datos_factura.credito_multa,datos_factura.periodo,sindo.nombre,sindo.domicilio,concat(sindo.localidad,\" \",sindo.cp) as localidad,sindo.rfc,datos_factura.notificador "+
					//	"FROM datos_factura,sindo WHERE datos_factura.nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" and datos_factura.registro_patronal1 = sindo.registro_patronal ORDER BY datos_factura.registro_patronal1,datos_factura.credito_cuotas";
					
					if(radioButton3.Checked==false){
						if(radioButton4.Checked==true){
							filtro=" AND status=\"EN TRAMITE\" ";
						}else{
							if(radioButton5.Checked==true){
								filtro=" AND status=\"0\" ";
							}
						}
					}else{
						filtro="";
					}
					
					if(comboBox2.SelectedIndex==0){
						sql="SELECT registro_patronal,credito_cuotas,credito_multa,periodo,razon_social,registro_patronal1,notificador,sector_notificacion_inicial,sector_notificacion_actualizado "+
							"FROM datos_factura WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\" "+filtro+" ORDER BY controlador,notificador";
						//MessageBox.Show(sql);
						
					}else{//******------+++++si es periodo especial
						sql="SELECT registro_patronal,credito_cuotas,credito_multa,periodo,razon_social,registro_patronal1,notificador,sector_notificacion_inicial,sector_notificacion_actualizado,nombre_periodo "+
							"FROM datos_factura WHERE periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" "+filtro+" ORDER BY controlador,notificador";
						//MessageBox.Show(sql);
						
						data_factura_prev=conex3.consultar(sql);
						
						sql="SELECT registro_patronal,credito_cuotas,credito_multa,periodo,razon_social,registro_patronal1,notificador,sector_notificacion_inicial,sector_notificacion_actualizado "+
							"FROM datos_factura WHERE periodo_factura=\""+comboBox1.SelectedItem.ToString()+"\" "+filtro+" ORDER BY controlador,notificador";
					}
					
					cred_manual=conex.consultar(sql);
					
					cred_manual.Columns.Add("domicilio");//9 dom
					cred_manual.Columns.Add("localidad");//10 loc
					cred_manual.Columns.Add("rfc");//11 rfc
					cred_manual.Columns.Add("num_notif");//12 num_not
					cred_manual.Columns.Add("fecha_documento");//13 fech_doc
					cred_manual.Columns.Add("nombre_documento");//14 nom_doc
					cred_manual.Columns.Add("Num",typeof(Int32));//15 num
					cred_manual.Columns.Add("fecha_contrato_inicio");//16 contrato_ini
					cred_manual.Columns.Add("fecha_contrato_fin");//17 contrato_fin
					cred_manual.Columns.Add("num_notif-nombre_notif-vigencia_notif");//18 texto para formato
					
					
					//MessageBox.Show(sql +|"+data_factura.Rows.Count+"i="+i);
					//dataGridView1.Rows.Add(data_factura.Rows.Count);
					//dataGridView1.Columns.Add("NUM_NOTIF","NUM NOTIF");
					//dataGridView1.Columns.Add("FECHA_DOC","FECHA DOC");
					//dataGridView1.Columns.Add("NOMBRE_DOC","NOMBRE DOCUMENTO");
					//estilo_grid();
					while(i<cred_manual.Rows.Count){
						cred_manual.Rows[i][3]=cred_manual.Rows[i][3].ToString().Substring(4,2)+"/"+cred_manual.Rows[i][3].ToString().Substring(0,4);
						cred_manual.Rows[i][15]=(i+1);
						
						//dataGridView1.Rows[i].Cells[0].Value=(i+1);
						//dataGridView1.Rows[i].Cells[1].Value=data_factura.Rows[i][0].ToString();
						//dataGridView1.Rows[i].Cells[2].Value=data_factura.Rows[i][1].ToString();
						//dataGridView1.Rows[i].Cells[3].Value=data_factura.Rows[i][2].ToString();
						//dataGridView1.Rows[i].Cells[4].Value=data_factura.Rows[i][3].ToString();
						//dataGridView1.Rows[i].Cells[5].Value=data_factura.Rows[i][4].ToString();
						
						//llena con datos sindo las columnas 6,7,8
						if((extractor_sindo(cred_manual.Rows[i][5].ToString(),i)==false)){
						   	per_tot++;
						}
						
						//llena las columnas fecha_ini,fecha_fin,num_notificador
						if(nom_noti.Equals(cred_manual.Rows[i][6].ToString())==false){
							j=0;
							num_noti=0;
							fecha_ini="";
							fecha_fin="";
							texto_combi="";
							nom_noti=cred_manual.Rows[i][6].ToString();
							while(j<notificadores.Rows.Count){
								if(notificadores.Rows[j][0].ToString().Equals(cred_manual.Rows[i][6].ToString())==true){
									num_noti=Convert.ToInt64(notificadores.Rows[j][1].ToString());
									fecha_ini=notificadores.Rows[j][2].ToString();
									fecha_fin=notificadores.Rows[j][3].ToString();
									j=notificadores.Rows.Count+1;
								}
								j++;	
							}
							
							cred_manual.Rows[i][12]=num_noti;
							if(fecha_ini.Length>9){
								cred_manual.Rows[i][16]=interprete_fechas_solo_mes(fecha_ini);
								cred_manual.Rows[i][17]=interprete_fechas_solo_mes(fecha_fin);
							}
							
							if(cred_manual.Rows[i][12].ToString().Length >8){
								cred_manual.Rows[i][18]="mediante identificación Oficial IMSS que contiene el número de matricula "+cred_manual.Rows[i][12].ToString()+", a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+",";
							}else{
								cred_manual.Rows[i][18]="mediante constancia de identificación número "+cred_manual.Rows[i][12].ToString()+", de fecha "+interprete_fechas(fecha_ini)+" a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+
													 ", con vigencia del "+interprete_fechas(fecha_ini)+" al "+interprete_fechas(fecha_fin)+",";
							}
						}else{
							cred_manual.Rows[i][12]=num_noti;
							if(fecha_ini.Length>9){
								cred_manual.Rows[i][16]=interprete_fechas_solo_mes(fecha_ini);
								cred_manual.Rows[i][17]=interprete_fechas_solo_mes(fecha_fin);
							}
							
							if(cred_manual.Rows[i][12].ToString().Length >8){
								cred_manual.Rows[i][18]="mediante identificación Oficial IMSS que contiene el número de matricula "+cred_manual.Rows[i][12].ToString()+", a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+",";
							}else{
								cred_manual.Rows[i][18]="mediante constancia de identificación número "+cred_manual.Rows[i][12].ToString()+", de fecha "+interprete_fechas(fecha_ini)+" a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+
													 ", con vigencia del "+interprete_fechas(fecha_ini)+" al "+interprete_fechas(fecha_fin)+",";
							}
						}
						cred_manual.Rows[i][6]=cred_manual.Rows[i][6].ToString().ToUpper();
						
						
						//llena la fecha del documento
                        if (comboBox2.SelectedIndex == 0)
                        {   //si es CLEM o CM no tiene fecha en la BD
                            if ((comboBox1.SelectedItem.ToString().Contains("CLEM")) || (comboBox1.SelectedItem.ToString().Contains("CM")))
                            {
                                cred_manual.Rows[i][13] = interprete_fechas_solo_mes(maskedTextBox1.Text);
                            }else{
                                //Buscar periodo en la BD del periodo seleccionado
                                cred_manual.Rows[i][13] = interprete_fechas_solo_mes(busca_fecha(comboBox1.SelectedItem.ToString()));
                            }                            
                        }
                        else
                        {
                            //data_factura_prev.Rows[i][9].ToString()
                            //MessageBox.Show("NOM PER: " + data_factura_prev.Rows[i][9].ToString());
                            fec_per = busca_fecha(data_factura_prev.Rows[i][9].ToString());
                            cred_manual.Rows[i][13] = interprete_fechas_solo_mes(fec_per);
                        }
						
						if(comboBox2.SelectedIndex==0){
						//llena el nombre del documento
							if(textBox1.Text.Length < 6){
								if(comboBox1.SelectedItem.ToString().Contains("ECO")==true){
									nom_doc="CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACION Y PAGO DE CUOTAS";
								}else{
									if(comboBox1.SelectedItem.ToString().Contains("SIVEPA")==true){
										nom_doc="CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACION Y PAGO DE CUOTAS";
									}
								}
								
								if(comboBox1.SelectedItem.ToString().Contains("RCV")==true){
									nom_doc=nom_doc+" CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
								}
								
								cred_manual.Rows[i][14]=nom_doc;
							}else{
								cred_manual.Rows[i][14]=textBox1.Text.ToUpper();
							}
						}else{
							//MessageBox.Show(data_factura_prev.Rows[i][9].ToString());
							if(textBox1.Text.Length < 6){
								nom_doc="";
								if(data_factura_prev.Rows[i][9].ToString().Contains("ECO")==true){
									nom_doc="CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACION Y PAGO DE CUOTAS";
								}else{
									if(data_factura_prev.Rows[i][9].ToString().Contains("SIVEPA")==true){
										nom_doc="CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACION Y PAGO DE CUOTAS";
									}
								}
								
								if(data_factura_prev.Rows[i][9].ToString().Contains("RCV")==true){
									nom_doc=nom_doc+" CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
								}
								
								cred_manual.Rows[i][14]=nom_doc;
							}else{
								cred_manual.Rows[i][14]=textBox1.Text.ToUpper();
							}
						}
						//dataGridView1.Rows[i].Cells[9].Value=data_factura.Rows[i][6].ToString();*/
						i++;
					}
					
					cred_manual.Columns.RemoveAt(5);
					cred_manual.Columns[14].SetOrdinal(0);
					cred_manual.Columns[6].SetOrdinal(11);
					dataGridView1.DataSource=cred_manual;
					estilo_grid();
					label3.Text="Registros: "+(dataGridView1.RowCount-1);
					label3.Refresh();
					label4.Text="Registros Sin Domicilio: "+(per_tot);
					label4.Refresh();
					dataGridView1.Visible=true;
					dataGridView1.Focus();
					
				}else{
					if(radioButton2.Checked==true){//si es periodo de oficios*****************************************************************
						
						if(radioButton3.Checked==false){
							if(radioButton4.Checked==true){
								filtro=" AND estatus=\"EN TRAMITE\" ";
							}else{
								if(radioButton5.Checked==true){
									filtro=" AND estatus=\"0\" ";
								}
							}
						}else{
							filtro="";
						}
						
						sql="SELECT reg_nss,folio,razon_social,domicilio_oficio,concat(localidad_oficio,\" \",cp_oficio) as localidad,rfc_oficio,receptor as notificador,fecha_oficio,nombre_oficio,sector FROM oficios WHERE periodo_oficio like \""+comboBox1.SelectedItem.ToString()+"\" "+filtro+" ";
						cred_manual=conex.consultar(sql);
						
						cred_manual.Columns.Add("num_notif");//10 num_not
						//data_factura.Columns.Add();//10 fech_doc
						//data_factura.Columns.Add();//11 nom_doc
						cred_manual.Columns.Add("Num",typeof(Int32));//11 num
						cred_manual.Columns.Add("fecha_contrato_inicio");//12 contrato_ini
						cred_manual.Columns.Add("fecha_contrato_fin");//13 contrato_fin
						cred_manual.Columns.Add("num_notif-nombre_notif-vigencia_notif");//14 texto para formato
						cred_manual.Columns.Add("fecha_documento");//15 fecha_doc_txt
						
						while(i<cred_manual.Rows.Count){
							cred_manual.Rows[i][11]=(i+1);
							
							if(cred_manual.Rows[i][3].ToString().Length<5){
								per_tot++;
							}
							
							//llena las columnas fecha_ini,fecha_fin,num_notificador
							if(nom_noti.Equals(cred_manual.Rows[i][6].ToString().ToUpper())==false){
								j=0;
								num_noti=0;
								fecha_ini="";
								fecha_fin="";
								texto_combi="";
								nom_noti=cred_manual.Rows[i][6].ToString();
								while(j<notificadores.Rows.Count){
									if(notificadores.Rows[j][0].ToString().ToUpper().Equals(cred_manual.Rows[i][6].ToString())==true){
										num_noti=Convert.ToInt64(notificadores.Rows[j][1].ToString());
										fecha_ini=notificadores.Rows[j][2].ToString();
										fecha_fin=notificadores.Rows[j][3].ToString();
										j=notificadores.Rows.Count+1;
									}
									j++;	
								}
							
								cred_manual.Rows[i][10]=num_noti;
								if(fecha_ini.Length>9){
									cred_manual.Rows[i][12]=interprete_fechas_solo_mes(fecha_ini);
									cred_manual.Rows[i][13]=interprete_fechas_solo_mes(fecha_fin);
								}
							
								if(cred_manual.Rows[i][10].ToString().Length >3){
									cred_manual.Rows[i][14]="mediante identificación Oficial IMSS que contiene el número de matricula "+cred_manual.Rows[i][10].ToString()+", a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+",";
								}else{
									cred_manual.Rows[i][14]="mediante constancia de identificación número "+cred_manual.Rows[i][10].ToString()+", de fecha "+interprete_fechas(fecha_ini)+" a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+
														 ", con vigencia del "+interprete_fechas(fecha_ini)+" al "+interprete_fechas(fecha_fin)+",";
								}
							}else{
								cred_manual.Rows[i][10]=num_noti;
								if(fecha_ini.Length>9){
									cred_manual.Rows[i][12]=interprete_fechas_solo_mes(fecha_ini);
									cred_manual.Rows[i][13]=interprete_fechas_solo_mes(fecha_fin);
								}
							
								if(cred_manual.Rows[i][10].ToString().Length >3){
									cred_manual.Rows[i][14]="mediante identificación Oficial IMSS que contiene el número de matricula "+cred_manual.Rows[i][10].ToString()+", a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+",";
								}else{
									cred_manual.Rows[i][14]="mediante constancia de identificación número "+cred_manual.Rows[i][10].ToString()+", de fecha "+interprete_fechas(fecha_ini)+" a nombre del(la) C. "+cred_manual.Rows[i][6].ToString().ToUpper()+
														 ", con vigencia del "+interprete_fechas(fecha_ini)+" al "+interprete_fechas(fecha_fin)+",";
								}
							}
							
						cred_manual.Rows[i][6]=cred_manual.Rows[i][6].ToString().ToUpper();
						//llena la fecha del documento
						//MessageBox.Show("|"+interprete_fechas(data_factura.Rows[i][7].ToString().Substring(0,10))+"|");
						cred_manual.Rows[i][15]=interprete_fechas_solo_mes(cred_manual.Rows[i][7].ToString().Substring(0,10));
							
							i++;
						}
						
						cred_manual.Columns[11].SetOrdinal(0);
						
						cred_manual.Columns[8].SetOrdinal(15);
						cred_manual.Columns[14].SetOrdinal(8);
						cred_manual.Columns[11].SetOrdinal(8);
						cred_manual.Columns[11].SetOrdinal(8);
						cred_manual.Columns.RemoveAt(15);
						
						dataGridView1.DataSource=cred_manual;
						estilo_grid_oficios();
						label3.Text="Registros: "+(dataGridView1.RowCount-1);
						label3.Refresh();
						label4.Text="Registros Sin Domicilio: "+(per_tot);
						label4.Refresh();
						dataGridView1.Visible=true;
						dataGridView1.Focus();
						
					}
				}
			}

            conex4.cerrar();
		}
		
		void GroupBox2Enter(object sender, EventArgs e)
		{
			
		}
		
		void RadioButton2CheckedChanged(object sender, EventArgs e)
		{
			llenar_Cb1();
			dataGridView1.Columns.Clear();
			if(radioButton2.Checked==true){
				maskedTextBox1.Enabled=false;
			}else{
				maskedTextBox1.Enabled=true;
			}
		}
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			llenar_Cb1();
			dataGridView1.Columns.Clear();
			if(radioButton2.Checked==true){
				maskedTextBox1.Enabled=false;
			}else{
				maskedTextBox1.Enabled=true;
			}
		}
		
		void DataGridView1CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void Correspondencia_acta_citatorioKeyDown(object sender, KeyEventArgs e)
		{
			
		}
		
		void MaskedTextBox1Leave(object sender, EventArgs e)
		{
			if(maskedTextBox1.MaskCompleted==true){
				if((DateTime.TryParse(maskedTextBox1.Text,out fecha_d)==true)){
					maskedTextBox1.BackColor= System.Drawing.Color.Green;
					maskedTextBox1.ForeColor=System.Drawing.Color.White;
				   }else{
				   	maskedTextBox1.BackColor= System.Drawing.Color.Red;
					maskedTextBox1.ForeColor=System.Drawing.Color.White;
				   }
				
			}else{
				maskedTextBox1.BackColor= System.Drawing.Color.Red;
				maskedTextBox1.ForeColor=System.Drawing.Color.White;
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			String fecha_impresa="";
			
			if(dataGridView1.RowCount>0){
				SaveFileDialog dialog_save = new SaveFileDialog();
				dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
				dialog_save.Title = "Guardar Resultados de Captura";//le damos un titulo a la ventana

				if (dialog_save.ShowDialog() == DialogResult.OK){
					try {
						if(radioButton1.Checked==true){//si es fecha de creditos
							conex2.conectar("base_principal");
							fecha_impresa=maskedTextBox1.Text;
							fecha_impresa=fecha_impresa.Substring(6,4)+"-"+fecha_impresa.Substring(3,2)+"-"+fecha_impresa.Substring(0,2);
							//conex2.consultar("UPDATE estado_periodos SET fecha_impresa_documento=\""+fecha_impresa+"\" WHERE nombre_periodo=\""+comboBox1.SelectedItem.ToString()+"\"");
						}	
					} catch (Exception es) {
						
					}
					XLWorkbook wb = new XLWorkbook();
					if (cred_manual3.Rows.Count>0)
					{
						wb.Worksheets.Add(cred_manual3, "hoja_lz");
					}
					else
					{
						wb.Worksheets.Add(cred_manual, "hoja_lz");
					}
					
					wb.SaveAs(@""+dialog_save.FileName+"");
					MessageBox.Show("Archivo guardado correctamente","Exito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
				}
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex>-1){
				maskedTextBox1.Focus();
			}
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.MaskCompleted==true){
				button4.Focus();
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox2.SelectedIndex==0){
				llenar_Cb1();
			}else{
				llenar_Cb1_especial();
			}
		}

        private void Button1_Click(object sender, EventArgs e)
        {
            if ( maskedTextBox2.MaskCompleted==true && (maskedTextBox2.BackColor == System.Drawing.Color.Green))
            {
                if (textBox2.Text.Length == 9 || textBox3.Text.Length == 9)
                {
					String credito_bus = "", fecha = "", sql_manual = "",nom_doc1="", fecha_i="",fecha_t="";

					if (textBox2.Text.Length==9)
					{
						credito_bus = textBox2.Text;

						sql_manual = "SELECT registro_patronal,credito_cuotas,credito_multa,periodo,razon_social,registro_patronal1,notificador,sector_notificacion_inicial,sector_notificacion_actualizado,nombre_periodo  " +
							   "FROM datos_factura WHERE credito_cuotas=\"" + credito_bus + "\"  ORDER BY controlador,notificador";
					}
					else
					{
						credito_bus = textBox3.Text;						

						sql_manual = "SELECT registro_patronal,credito_cuotas,credito_multa,periodo,razon_social,registro_patronal1,notificador,sector_notificacion_inicial,sector_notificacion_actualizado,nombre_periodo " +
							   "FROM datos_factura WHERE credito_multa=\"" + credito_bus + "\"  ORDER BY controlador,notificador";
					}

					fecha = maskedTextBox2.Text;
					int cont = 0;
                    DataTable cred_manual2 = new DataTable();
                    cred_manual2 = conex3.consultar(sql_manual);

                    if (cred_manual2.Rows.Count>0)
                    {
                        if (cred_manual3.Rows.Count==0) {
                            cred_manual3.Columns.Add("num");//0
                            cred_manual3.Columns.Add("registro_patronal");//1 
                            cred_manual3.Columns.Add("credito_cuota");//2
                            cred_manual3.Columns.Add("credito_multa");//3 
                            cred_manual3.Columns.Add("periodo");//4 
                            cred_manual3.Columns.Add("razon_social");//5 
                            cred_manual3.Columns.Add("sector_inicial");//6 
                            cred_manual3.Columns.Add("sector_actualizado");//7 
                            cred_manual3.Columns.Add("domicilio");//8 
                            cred_manual3.Columns.Add("localidad");//9 dom
                            cred_manual3.Columns.Add("rfc");//10 loc
                            cred_manual3.Columns.Add("notificador");//11 rfc
                            cred_manual3.Columns.Add("num_notif");//12 num_not
                            cred_manual3.Columns.Add("fecha_documento");//13 fech_doc
                            cred_manual3.Columns.Add("nombre_documento");//14 nom_doc
                            cred_manual3.Columns.Add("fecha_contrato_inicio");//15 contrato_ini
                            cred_manual3.Columns.Add("fecha_contrato_fin");//16 contrato_fin
                            cred_manual3.Columns.Add("num_notif-nombre_notif-vigencia_notif");//17 texto para formato
						}

                        cred_manual3.Rows.Add();

                        int tot_fils = (cred_manual3.Rows.Count-1);
                        
						cred_manual3.Rows[tot_fils][0] = tot_fils+1;
                        cred_manual3.Rows[tot_fils][1] = cred_manual2.Rows[0][0].ToString();
                        cred_manual3.Rows[tot_fils][2] = cred_manual2.Rows[0][1].ToString();
                        cred_manual3.Rows[tot_fils][3] = cred_manual2.Rows[0][2].ToString();
                        cred_manual3.Rows[tot_fils][4] = cred_manual2.Rows[0][3].ToString().Substring(4, 2) + '/' + cred_manual2.Rows[0][3].ToString().Substring(0, 4);
						cred_manual3.Rows[tot_fils][5] = cred_manual2.Rows[0][4].ToString();
                        cred_manual3.Rows[tot_fils][6] = cred_manual2.Rows[0][7].ToString();
                        cred_manual3.Rows[tot_fils][7] = cred_manual2.Rows[0][8].ToString();
                        cred_manual3.Rows[tot_fils][11] = cred_manual2.Rows[0][6].ToString().ToUpper();

						cred_manual3.Rows[tot_fils][13] = interprete_fechas_solo_mes(fecha);
						extractor_sindo_manual(cred_manual2.Rows[0][5].ToString(), tot_fils);

						int tot_fils_not = notificadores.Rows.Count;

						while (cont<tot_fils_not)
						{
							if (notificadores.Rows[cont][0].ToString() == cred_manual2.Rows[0][6].ToString())
							{
								cred_manual3.Rows[tot_fils][12] = notificadores.Rows[cont][1].ToString();
								cred_manual3.Rows[tot_fils][15] = interprete_fechas_solo_mes(notificadores.Rows[cont][2].ToString());
								cred_manual3.Rows[tot_fils][16] = interprete_fechas_solo_mes(notificadores.Rows[cont][3].ToString());
								fecha_i = notificadores.Rows[cont][2].ToString();
								fecha_t = notificadores.Rows[cont][3].ToString();
							}
							cont++;
						}

						if (cred_manual3.Rows[tot_fils][12].ToString().Length > 8)
						{
							cred_manual3.Rows[tot_fils][17] = "mediante identificación Oficial IMSS que contiene el número de matricula " + cred_manual3.Rows[tot_fils][12].ToString() + ", a nombre del(la) C. " + cred_manual3.Rows[tot_fils][11].ToString().ToUpper() + ",";
						}
						else
						{
							cred_manual3.Rows[tot_fils][17] = "mediante constancia de identificación número " + cred_manual3.Rows[tot_fils][12].ToString() + ", de fecha " + interprete_fechas(fecha_i) + " a nombre del(la) C. " + cred_manual3.Rows[tot_fils][11].ToString().ToUpper() +
												 ", con vigencia del " + interprete_fechas(fecha_i) + " al " + interprete_fechas(fecha_t) + ",";
						}

						if (cred_manual2.Rows[0][9].ToString().Contains("ECO") == true)
						{
							nom_doc1 = "CÉDULA DE LIQUIDACIÓN POR LA OMISIÓN TOTAL EN LA DETERMINACION Y PAGO DE CUOTAS";
						}
						else
						{
							if (cred_manual2.Rows[0][9].ToString().Contains("SIVEPA") == true)
							{
								nom_doc1 = "CÉDULA DE LIQUIDACIÓN POR DIFERENCIAS EN LA DETERMINACION Y PAGO DE CUOTAS";
							}
						}

						if (cred_manual2.Rows[0][9].ToString().Contains("RCV") == true)
						{
							nom_doc1 = nom_doc1 + " CORRESPONDIENTES AL SEGURO DE RETIRO, CESANTÍA EN EDAD AVANZADA Y VEJEZ";
						}

						cred_manual3.Rows[tot_fils][14] = nom_doc1;

						dataGridView1.DataSource = cred_manual3;
						//dataGridView1.DataSource = notificadores;
						estilo_grid();
						dataGridView1.Visible = true;
                        dataGridView1.Focus();
						label3.Text = "Registros: " + dataGridView1.RowCount;
						label3.Refresh();
						textBox2.Clear();
						textBox3.Clear();
						maskedTextBox2.Clear();
						textBox2.Focus();

						//MessageBox.Show("cred" + credito_bus + " fecha" + fecha);

						/*
						dataGridView1.Columns[1].HeaderText="REGISTRO PATRONAL";
						dataGridView1.Columns[2].HeaderText="CREDITO CUOTA";
						dataGridView1.Columns[3].HeaderText="CREDITO MULTA";
						dataGridView1.Columns[4].HeaderText="PERIODO";
						dataGridView1.Columns[5].HeaderText="RAZON SOCIAL";
						dataGridView1.Columns[6].HeaderText="SECTOR INICIAL";
						dataGridView1.Columns[7].HeaderText="SECTOR ACTUALIZADO";
						dataGridView1.Columns[8].HeaderText="DOMICILIO";
						dataGridView1.Columns[9].HeaderText="LOCALIDAD";
						dataGridView1.Columns[10].HeaderText="RFC";
						dataGridView1.Columns[11].HeaderText="NOTIFICADOR";
						dataGridView1.Columns[12].HeaderText="NUM NOTIF.";
						dataGridView1.Columns[13].HeaderText="FECHA DOCUMENTO";
						dataGridView1.Columns[14].HeaderText="NOMBRE DOCUMENTO";
						dataGridView1.Columns[15].HeaderText="FECHA CONTRATO INICIO";
						dataGridView1.Columns[16].HeaderText="FECHA CONTRATO FIN";
						dataGridView1.Columns[17].HeaderText="NUM NOMBRE VIGENCIA NOTIF.";
*/
					}
				}
			}
			else
			{
				MessageBox.Show("Fecha de Documento Invalida");
			}
		}

		private void TextBox2_TextChanged(object sender, EventArgs e)
		{
			if (textBox2.Text.Length==9)
			{
				maskedTextBox2.Focus();
			}
		}

		private void MaskedTextBox2_TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.MaskCompleted == true)
			{
				button1.Focus();
			}
		}

		private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void TextBox3_TextChanged(object sender, EventArgs e)
		{
			if (textBox3.Text.Length == 9)
			{
				maskedTextBox2.Focus();
			}
		}

		private void maskedTextBox2_Leave(object sender, EventArgs e)
		{
			if (maskedTextBox2.MaskCompleted == true)
			{
				if ((DateTime.TryParse(maskedTextBox2.Text, out fecha_d) == true))
				{
					maskedTextBox2.BackColor = System.Drawing.Color.Green;
					maskedTextBox2.ForeColor = System.Drawing.Color.White;
				}
				else
				{
					maskedTextBox2.BackColor = System.Drawing.Color.Red;
					maskedTextBox2.ForeColor = System.Drawing.Color.White;
				}

			}
			else
			{
				maskedTextBox2.BackColor = System.Drawing.Color.Red;
				maskedTextBox2.ForeColor = System.Drawing.Color.White;
			}
		}
	}
}

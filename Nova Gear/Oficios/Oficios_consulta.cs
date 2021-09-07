/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 14/02/2017
 * Hora: 04:05 p.m.
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
using System.ComponentModel;

namespace Nova_Gear.Oficios
{
	/// <summary>
	/// Description of Oficios_consulta.
	/// </summary>
	public partial class Oficios_consulta : Form
	{
		public Oficios_consulta()
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
		Conexion conex1 = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();	
		Conexion conex5 = new Conexion();
		
		DataTable periodos = new DataTable();
		DataTable sectores = new DataTable();
		DataTable notificadores = new DataTable();
		DataTable controladores = new DataTable();
		DataTable permisos = new DataTable();	
		DataTable noms_docs = new DataTable();
        
		int ran=6;
		String  id_usu,nombre_puesto;
		
		
        public void buscar_oficio(){
        	
        	String tipo_bus="",sql;
        	int id_of=0,err_id_of=0;
        	try{
        		if(comboBox2.SelectedIndex > -1 && textBox1.Text.Length>0){
        			
        			switch(comboBox2.SelectedIndex){
        					case 0: if((int.TryParse(textBox1.Text,out id_of))==true){
        								tipo_bus=" id_oficios ="+id_of+" ";//id
		        					}else{
        								MessageBox.Show("El Valor Ingresado no se puede Buscar.","Aviso");
        								//textBox1.Text="";
        								err_id_of=1;
		        					}
        					break;
        					case 1: tipo_bus=" reg_nss like \"%"+textBox1.Text+"%\" ";//reg_nss
        					break;
        					case 2: tipo_bus=" razon_social like \"%"+textBox1.Text+"%\" ";//razon_social
        					break;
        					case 3: tipo_bus=" folio like \"%"+textBox1.Text+"%\" ";//folio
        					break;
        					case 4: tipo_bus=" acuerdo like \"%"+textBox1.Text+"%\" ";//acuerdo
        					break;
        					case 5: tipo_bus=" estatus ="+textBox1.Text+" ";//estatus
        					break;
        					case 6: tipo_bus=" periodo_oficio like \"%"+textBox1.Text+"%\" ";//periodo
        					break;
        			}
        			if(err_id_of==0){
        			//                   0              1       2            3     4      5             6      7      8       9           10
        			sql="SELECT id_oficios,periodo_oficio,reg_nss,razon_social,folio,acuerdo,fecha_oficio,emisor,sector,receptor,controlador,"+
        				//	11     12            13                 14                 15          16   17                  18             19               20               21               22        23        24    25
        				"ccjal,estatus,fecha_captura,fecha_recep_contro,fecha_notificacion,fecha_visita,nn,fecha_devolucion_not,observaciones,fecha_devo_origen,domicilio_oficio,localidad_oficio,cp_oficio,rfc_oficio,nombre_oficio FROM oficios WHERE "+tipo_bus;
        			conex.conectar("base_principal");
        			dataGridView1.DataSource=conex.consultar(sql);
        			
        			dataGridView1.Columns[0].HeaderText="ID OFICIO";
        			dataGridView1.Columns[1].HeaderText="PERIODO";
        			dataGridView1.Columns[2].HeaderText="REG.PAT./NSS";
        			dataGridView1.Columns[2].MinimumWidth=120;
        			dataGridView1.Columns[3].HeaderText="RAZON SOCIAL";
        			dataGridView1.Columns[3].MinimumWidth=300;
        			dataGridView1.Columns[4].HeaderText="FOLIO";
        			dataGridView1.Columns[5].HeaderText="ACUERDO";
        			dataGridView1.Columns[6].Visible=false;
        			dataGridView1.Columns[7].Visible=false;
        			dataGridView1.Columns[8].Visible=false;
        			dataGridView1.Columns[9].Visible=false;
        			dataGridView1.Columns[10].Visible=false;
        			dataGridView1.Columns[11].Visible=false;
        			dataGridView1.Columns[12].HeaderText="ESTATUS";
        			dataGridView1.Columns[13].Visible=false;
        			dataGridView1.Columns[14].Visible=false;
        			dataGridView1.Columns[15].Visible=false;
        			dataGridView1.Columns[16].Visible=false;
        			dataGridView1.Columns[17].Visible=false;
        			dataGridView1.Columns[18].Visible=false;
        			dataGridView1.Columns[19].Visible=false;
        			dataGridView1.Columns[20].Visible=false;
        			dataGridView1.Columns[21].Visible=false;
        			dataGridView1.Columns[22].Visible=false;
        			dataGridView1.Columns[23].Visible=false;
        			dataGridView1.Columns[24].Visible=false;
        			dataGridView1.Columns[25].Visible=false;
        			
        			timer1.Enabled=true;
        			label22.Text="Registros Totales: "+dataGridView1.RowCount;
        			label22.Refresh();
        			dataGridView1.Focus();
        			
        			 
        			}
        			err_id_of=0;
        		}
        	}catch{}
        }
        
        public void llenar_sectores_notificadores(){
        	conex3.conectar("base_principal");
        	conex1.conectar("base_principal");
        	conex2.conectar("base_principal");
        	sectores=conex3.consultar("SELECT * FROM sectores");
        	notificadores=conex1.consultar("SELECT id_usuario,apellido,nombre FROM usuarios WHERE controlador <> 0 ORDER BY apellido ASC");
        	controladores=conex2.consultar("SELECT id_usuario,apellido,nombre FROM usuarios WHERE puesto=\"controlador\"");
        	conex3.cerrar();
        	conex1.cerrar();
        	conex2.cerrar();
        }
		
		public void llenar_cb3(){
        	conex1.conectar("base_principal");
        	comboBox3.Items.Clear();
        	int i=0;
        	noms_docs= conex1.consultar("SELECT DISTINCT nombre_oficio FROM oficios ORDER BY nombre_oficio");
        	while(i < noms_docs.Rows.Count){
        		comboBox3.Items.Add(noms_docs.Rows[i][0].ToString().ToUpper());
        		i++;
        	}
        	conex1.cerrar();
        }
		
		public void estilo_consulta(){
			textBox2.BackColor=System.Drawing.Color.Black;
			textBox2.ForeColor=System.Drawing.Color.SpringGreen;
			textBox2.BorderStyle=BorderStyle.FixedSingle;
			textBox20.BackColor=System.Drawing.Color.Black;
			textBox20.ForeColor=System.Drawing.Color.SpringGreen;
			textBox20.BorderStyle=BorderStyle.FixedSingle;
			textBox4.BackColor=System.Drawing.Color.Black;
			textBox4.ForeColor=System.Drawing.Color.SpringGreen;
			textBox4.BorderStyle=BorderStyle.FixedSingle;
			textBox5.BackColor=System.Drawing.Color.Black;
			textBox5.ForeColor=System.Drawing.Color.SpringGreen;
			textBox5.BorderStyle=BorderStyle.FixedSingle;
			textBox6.BackColor=System.Drawing.Color.Black;
			textBox6.ForeColor=System.Drawing.Color.SpringGreen;
			textBox6.BorderStyle=BorderStyle.FixedSingle;
			textBox7.BackColor=System.Drawing.Color.Black;
			textBox7.ForeColor=System.Drawing.Color.SpringGreen;
			textBox7.BorderStyle=BorderStyle.FixedSingle;
			textBox8.BackColor=System.Drawing.Color.Black;
			textBox8.ForeColor=System.Drawing.Color.SpringGreen;
			textBox8.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox1.BackColor=System.Drawing.Color.Black;
			maskedTextBox1.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox1.BorderStyle=BorderStyle.FixedSingle;
			//textBox9.BackColor=System.Drawing.Color.Black;
			//textBox9.ForeColor=System.Drawing.Color.White;
			textBox10.BackColor=System.Drawing.Color.Black;
			textBox10.ForeColor=System.Drawing.Color.SpringGreen;
			textBox10.BorderStyle=BorderStyle.FixedSingle;
			textBox11.BackColor=System.Drawing.Color.Black;
			textBox11.ForeColor=System.Drawing.Color.SpringGreen;
			textBox11.BorderStyle=BorderStyle.FixedSingle;
			textBox12.BackColor=System.Drawing.Color.Black;
			textBox12.ForeColor=System.Drawing.Color.SpringGreen;
			textBox12.BorderStyle=BorderStyle.FixedSingle;
			textBox13.BackColor=System.Drawing.Color.Black;
			textBox13.ForeColor=System.Drawing.Color.SpringGreen;
			textBox13.BorderStyle=BorderStyle.FixedSingle;
			textBox14.BackColor=System.Drawing.Color.Black;
			textBox14.ForeColor=System.Drawing.Color.SpringGreen;
			textBox14.BorderStyle=BorderStyle.FixedSingle;
			textBox19.BackColor=System.Drawing.Color.Black;
			textBox19.ForeColor=System.Drawing.Color.SpringGreen;
			textBox19.BorderStyle=BorderStyle.FixedSingle;
			textBox15.BackColor=System.Drawing.Color.Black;
			textBox15.ForeColor=System.Drawing.Color.SpringGreen;
			textBox15.BorderStyle=BorderStyle.FixedSingle;
			textBox16.BackColor=System.Drawing.Color.Black;
			textBox16.ForeColor=System.Drawing.Color.SpringGreen;
			textBox16.BorderStyle=BorderStyle.FixedSingle;
			textBox17.BackColor=System.Drawing.Color.Black;
			textBox17.ForeColor=System.Drawing.Color.SpringGreen;
			textBox17.BorderStyle=BorderStyle.FixedSingle;
			textBox18.BackColor=System.Drawing.Color.Black;
			textBox18.ForeColor=System.Drawing.Color.SpringGreen;
			textBox18.BorderStyle=BorderStyle.FixedSingle;
			textBox21.BackColor=System.Drawing.Color.Black;
			textBox21.ForeColor=System.Drawing.Color.SpringGreen;
			textBox21.BorderStyle=BorderStyle.FixedSingle;
			textBox23.BackColor=System.Drawing.Color.Black;
			textBox23.ForeColor=System.Drawing.Color.SpringGreen;
			textBox23.BorderStyle=BorderStyle.FixedSingle;
			textBox24.BackColor=System.Drawing.Color.Black;
			textBox24.ForeColor=System.Drawing.Color.SpringGreen;
			textBox24.BorderStyle=BorderStyle.FixedSingle;
			textBox25.BackColor=System.Drawing.Color.Black;
			textBox25.ForeColor=System.Drawing.Color.SpringGreen;
			textBox25.BorderStyle=BorderStyle.FixedSingle;
			textBox26.BackColor=System.Drawing.Color.Black;
			textBox26.ForeColor=System.Drawing.Color.SpringGreen;
			textBox26.BorderStyle=BorderStyle.FixedSingle;
			textBox27.BackColor=System.Drawing.Color.Black;
			textBox27.ForeColor=System.Drawing.Color.SpringGreen;
			textBox27.BorderStyle=BorderStyle.FixedSingle;
			comboBox1.BackColor=System.Drawing.Color.Black;
			comboBox1.ForeColor=System.Drawing.Color.SpringGreen;
			comboBox3.BackColor=System.Drawing.Color.Black;
			comboBox3.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox2.BackColor=System.Drawing.Color.Black;
			maskedTextBox2.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox2.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox3.BackColor=System.Drawing.Color.Black;
			maskedTextBox3.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox3.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox4.BackColor=System.Drawing.Color.Black;
			maskedTextBox4.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox4.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox5.BackColor=System.Drawing.Color.Black;
			maskedTextBox5.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox5.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox6.BackColor=System.Drawing.Color.Black;
			maskedTextBox6.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox6.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox7.BackColor=System.Drawing.Color.Black;
			maskedTextBox7.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox7.BorderStyle=BorderStyle.FixedSingle;
			maskedTextBox8.BackColor=System.Drawing.Color.Black;
			maskedTextBox8.ForeColor=System.Drawing.Color.SpringGreen;
			maskedTextBox8.BorderStyle=BorderStyle.FixedSingle;
		}
		
		public void estilo_edicion(){
			textBox2.BackColor=System.Drawing.SystemColors.Window;
			textBox2.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox2.BorderStyle=BorderStyle.Fixed3D;
			textBox20.BackColor=System.Drawing.SystemColors.Window;
			textBox20.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox20.BorderStyle=BorderStyle.Fixed3D;
			textBox4.BackColor=System.Drawing.SystemColors.Window;
			textBox4.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox4.BorderStyle=BorderStyle.Fixed3D;
			textBox5.BackColor=System.Drawing.SystemColors.Window;
			textBox5.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox5.BorderStyle=BorderStyle.Fixed3D;
			textBox6.BackColor=System.Drawing.SystemColors.Window;
			textBox6.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox6.BorderStyle=BorderStyle.Fixed3D;
			textBox7.BackColor=System.Drawing.SystemColors.Window;
			textBox7.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox7.BorderStyle=BorderStyle.Fixed3D;
			textBox8.BackColor=System.Drawing.SystemColors.Window;
			textBox8.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox8.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox1.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox1.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox1.BorderStyle=BorderStyle.Fixed3D;
			//textBox9.BackColor=System.Drawing.SystemColors.Window;
			//textBox9.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox10.BackColor=System.Drawing.SystemColors.Window;
			textBox10.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox10.BorderStyle=BorderStyle.Fixed3D;
			textBox11.BackColor=System.Drawing.SystemColors.Window;
			textBox11.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox11.BorderStyle=BorderStyle.Fixed3D;
			/*textBox12.BackColor=System.Drawing.SystemColors.Window;
			textBox12.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox12.BorderStyle=BorderStyle.Fixed3D;
			textBox13.BackColor=System.Drawing.SystemColors.Window;
			textBox13.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox13.BorderStyle=BorderStyle.Fixed3D;
			textBox14.BackColor=System.Drawing.SystemColors.Window;
			textBox14.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox14.BorderStyle=BorderStyle.Fixed3D;*/
			textBox19.BackColor=System.Drawing.SystemColors.Window;
			textBox19.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox19.BorderStyle=BorderStyle.Fixed3D;
			textBox15.BackColor=System.Drawing.SystemColors.Window;
			textBox15.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox15.BorderStyle=BorderStyle.Fixed3D;
			textBox16.BackColor=System.Drawing.SystemColors.Window;
			textBox16.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox16.BorderStyle=BorderStyle.Fixed3D;
			textBox17.BackColor=System.Drawing.SystemColors.Window;
			textBox17.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox17.BorderStyle=BorderStyle.Fixed3D;
			textBox18.BackColor=System.Drawing.SystemColors.Window;
			textBox18.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox18.BorderStyle=BorderStyle.Fixed3D;
			textBox21.BackColor=System.Drawing.SystemColors.Window;
			textBox21.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox21.BorderStyle=BorderStyle.Fixed3D;
			textBox23.BackColor=System.Drawing.SystemColors.Window;
			textBox23.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox23.BorderStyle=BorderStyle.Fixed3D;
			textBox24.BackColor=System.Drawing.SystemColors.Window;
			textBox24.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox24.BorderStyle=BorderStyle.Fixed3D;
			textBox25.BackColor=System.Drawing.SystemColors.Window;
			textBox25.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox25.BorderStyle=BorderStyle.Fixed3D;
			textBox26.BackColor=System.Drawing.SystemColors.Window;
			textBox26.ForeColor=System.Drawing.SystemColors.ControlText;
			textBox26.BorderStyle=BorderStyle.Fixed3D;
			textBox27.Visible=false;
			comboBox1.BackColor=System.Drawing.SystemColors.Window;
			comboBox1.ForeColor=System.Drawing.SystemColors.ControlText;
			comboBox3.BackColor=System.Drawing.SystemColors.Window;
			comboBox3.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox2.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox2.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox2.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox3.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox3.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox4.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox4.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox5.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox5.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox6.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox6.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox6.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox7.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox7.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox7.BorderStyle=BorderStyle.Fixed3D;
			maskedTextBox8.BackColor=System.Drawing.SystemColors.Window;
			maskedTextBox8.ForeColor=System.Drawing.SystemColors.ControlText;
			maskedTextBox8.BorderStyle=BorderStyle.Fixed3D;
			
		}
        
		public void sector_auto(){
			String sector,id_not="",id_contro="",receptor="",controlador="";
			int i=0;
			
			sector=maskedTextBox1.Text;
			if(sector.Length==1){
				sector="0"+sector;
				
			}
			
			maskedTextBox1.Text=sector;
			textBox10.Text="";
			textBox11.Text="";
			i=0;
			if(sector.Length>0){
				while(i<sectores.Rows.Count){
					if(Convert.ToInt32(sector) == Convert.ToInt32(sectores.Rows[i][3].ToString())){
						id_contro=sectores.Rows[i][1].ToString();
						id_not=sectores.Rows[i][2].ToString();
						i=sectores.Rows.Count;
					}
					i++;
				}
			}else{
				textBox10.Text="";
				textBox11.Text="";
			}
			
			if(id_not.Length>0 && id_contro.Length>0){
			
				i=0;
				while(i<notificadores.Rows.Count){
					if(id_not.Equals(notificadores.Rows[i][0].ToString())){
						receptor=notificadores.Rows[i][1].ToString()+" "+notificadores.Rows[i][2].ToString();
						i=notificadores.Rows.Count;
					}
					i++;
				}
				
				i=0;
				while(i<controladores.Rows.Count){
					if(id_contro.Equals(controladores.Rows[i][0].ToString())){
						controlador=controladores.Rows[i][1].ToString()+" "+controladores.Rows[i][2].ToString();
						i=notificadores.Rows.Count;
					}
					i++;
				}
				
				textBox10.Text=receptor;
				textBox11.Text=controlador;
				
			}else{
				if(sector.Equals("72")){
					textBox10.Text="SUBDELEGACION LIBERTAD-REFORMA";
					textBox11.Text="SUBDELEGACION LIBERTAD-REFORMA";
				}else{
					if(sector.Equals("73")){
						textBox10.Text="SUBDELEGACION JUAREZ";
						textBox11.Text="SUBDELEGACION JUAREZ";
					}else{
						if(sector.Equals("76")){
							textBox10.Text="FORANEOS";
							textBox11.Text="FORANEOS";
						}else{
							
						}
					}
				}
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
		
		public void guardar(){
			
			String id,sql,sql2,msg_error,ccjal,nn,status="",msg_cambios="",fecha_of,fecha_capt,fecha_recep,fecha_not,fecha_vis,fecha_devo,fecha_origen_devo;
			int fila=0,error=0,acierto=0;
			
			if(dataGridView1.RowCount>0){
				fila=dataGridView1.CurrentCell.RowIndex;
				id=dataGridView1.Rows[fila].Cells[0].FormattedValue.ToString();
				
				sql="UPDATE oficios SET ";
				sql2="WHERE id_oficios="+id;
				msg_error="El (Los) Siguiente(s) Campo(s) esta(n) vacío(s) o con datos incorrectos:\n";
				
				//msg_cambios="El (Los) Siguiente(s) Campo(s) será(n) actualizado(s):\n";
				
				if(!(textBox2.Text.Equals(dataGridView1.Rows[fila].Cells[2].FormattedValue.ToString()))){
					if(textBox2.Text.Length>4||textBox2.Text.Equals("-")){
						sql+=" reg_nss=\""+textBox2.Text+"\",";
						acierto++;
						msg_cambios+="•REG. PAT./N.S.S.\n";
					}else{
						error++;
						msg_error+="•REG. PAT./N.S.S.\n";
					}
				}
				
				if(!(textBox20.Text.Equals(dataGridView1.Rows[fila].Cells[1].FormattedValue.ToString()))){
					if(textBox20.Text.Length>4||textBox20.Text.Equals("-")){
						sql+=" periodo_oficio=\""+textBox20.Text+"\",";
						acierto++;
						msg_cambios+="•Periodo Oficio\n";
					}else{
						error++;
						msg_error+="•Periodo Oficio\n";
					}
				}
				
				if(!(textBox4.Text.Equals(dataGridView1.Rows[fila].Cells[3].FormattedValue.ToString()))){
					if(textBox4.Text.Length>4||textBox4.Text.Equals("-")){
						sql+=" razon_social=\""+textBox4.Text+"\",";
						acierto++;
						msg_cambios+="•Razón Social\n";
					}else{
						error++;
						msg_error+="•Razón Social\n";
					}
				}
				
				if(!(textBox23.Text.Equals(dataGridView1.Rows[fila].Cells[21].FormattedValue.ToString()))){
					if(textBox23.Text.Length>5||textBox23.Text.Equals("-")){
						sql+=" domicilio_oficio=\""+textBox23.Text+"\",";
						acierto++;
						msg_cambios+="•Domicilio\n";
					}else{
						error++;
						msg_error+="•Domicilio\n";
					}
				}
				
				if(!(textBox24.Text.Equals(dataGridView1.Rows[fila].Cells[22].FormattedValue.ToString()))){
					if(textBox24.Text.Length>4||textBox24.Text.Equals("-")){
						sql+=" localidad_oficio=\""+textBox24.Text+"\",";
						acierto++;
						msg_cambios+="•Localidad\n";
					}else{
						error++;
						msg_error+="•Localidad\n";
					}
				}
				
				if(!(textBox25.Text.Equals(dataGridView1.Rows[fila].Cells[23].FormattedValue.ToString()))){
					if(textBox25.Text.Length>4||textBox25.Text.Equals("-")){
						sql+=" cp_oficio=\""+textBox25.Text+"\",";
						acierto++;
						msg_cambios+="•C.P.(Código Postal)\n";
					}else{
						error++;
						msg_error+="•C.P.(Código Postal)\n";
					}
				}
				
				if(!(textBox26.Text.Equals(dataGridView1.Rows[fila].Cells[24].FormattedValue.ToString()))){
					if(textBox26.Text.Length>6||textBox26.Text.Equals("-")){
						sql+="rfc_oficio=\""+textBox26.Text+"\",";
						acierto++;
						msg_cambios+="•R.F.C.(Registro Federal de Contribuyentes)\n";
					}else{
						error++;
						msg_error+="•R.F.C.(Registro Federal de Contribuyentes)\n";
					}
				}
				
				if(!(textBox5.Text.Equals(dataGridView1.Rows[fila].Cells[4].FormattedValue.ToString()))){
					if(textBox5.Text.Length>2||textBox5.Text.Equals("-")){
						sql+=" folio=\""+textBox5.Text+"\",";
						acierto++;
						msg_cambios+="•Folio\n";
					}else{
						error++;
						msg_error+="•Folio\n";
					}
				}
				
				if(!(textBox6.Text.Equals(dataGridView1.Rows[fila].Cells[5].FormattedValue.ToString()))){
					if(textBox6.Text.Length>2||textBox6.Text.Equals("-")){
						sql+=" acuerdo=\""+textBox6.Text+"\",";
						acierto++;
						msg_cambios+="•Acuerdo\n";
					}else{
						error++;
						msg_error+="•Acuerdo\n";
					}
				}
				
				if(maskedTextBox7.Text.Equals("  /  /")){
					fecha_of="";
				}else{
					fecha_of=maskedTextBox7.Text;
				}
				                
				if(!(fecha_of.Equals(dataGridView1.Rows[fila].Cells[6].FormattedValue.ToString()))){
					if(maskedTextBox7.Text.Length==10){
						if(maskedTextBox7.BackColor.Name.Equals("Window")){
							sql+=" fecha_oficio=\""+(maskedTextBox7.Text.Substring(6,4)+"-"+maskedTextBox7.Text.Substring(3,2)+"-"+maskedTextBox7.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios+="•Fecha Oficio\n";
						}else{
							error++;
							msg_error+="•Fecha Oficio\n";
						}
					}else{
						if(maskedTextBox7.Text.Equals("  /  /")){
							sql+=" fecha_oficio=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha Oficio\n";
						}
					}
				}
				
				if(!(textBox8.Text.Equals(dataGridView1.Rows[fila].Cells[7].FormattedValue.ToString()))){
					if(textBox8.Text.Length>2||textBox8.Text.Equals("-")){
						sql+=" emisor=\""+textBox8.Text+"\",";
						acierto++;
						msg_cambios+="•Emisor\n";
					}else{
						error++;
						msg_error+="•Emisor\n";
					}
				}
				
				if(!(maskedTextBox1.Text.Equals(dataGridView1.Rows[fila].Cells[8].FormattedValue.ToString()))){
					if(maskedTextBox1.BackColor.Name.Equals("Window")){
						if(textBox10.Text.Length>0 && textBox11.Text.Length>0){
							sql+=" sector=\""+maskedTextBox1.Text+"\",";
							acierto++;
							msg_cambios+="•Sector\n";
						}else{
							error++;
							msg_error+="•Sector\n";
						}
					}else{
						error++;
						msg_error+="•Sector\n";
					}
				}
				
				if(!(textBox10.Text.Equals(dataGridView1.Rows[fila].Cells[9].FormattedValue.ToString()))){
					if(textBox10.Text.Length>2||textBox10.Equals("-")){
						sql+=" receptor=\""+textBox10.Text+"\",";
						acierto++;
						msg_cambios+="•Receptor\n";
					}else{
						error++;
						msg_error+="•Receptor\n";
					}
				}
				
				if(!(textBox11.Text.Equals(dataGridView1.Rows[fila].Cells[10].FormattedValue.ToString()))){
					if(textBox11.Text.Length>2||textBox11.Text.Equals("-")){
						sql+=" controlador=\""+textBox11.Text+"\",";
						acierto++;
						msg_cambios+="•Controlador\n";
					}else{
						error++;
						msg_error+="•Controlador\n";
					}
				}
				
				if(checkBox1.Checked==true){
					ccjal="CCJAL";
				}else{
					ccjal="-";
				}
				
				if(!(ccjal.Equals(dataGridView1.Rows[fila].Cells[11].FormattedValue.ToString()))){
					sql+=" ccjal=\""+ccjal+"\",";
					acierto++;
					msg_cambios+="•CCJAL\n";
				}
				
				if(checkBox2.Checked==true){
					nn="NN";
				}else{
					nn="-";
				}
				
				if(!(nn.Equals(dataGridView1.Rows[fila].Cells[17].FormattedValue.ToString()))){
					sql+=" nn=\""+nn+"\",";
					acierto++;
					msg_cambios+="•NN\n";
				}
				
				switch(comboBox1.SelectedIndex){
					case 0: status="0";
					break;
					case 1: status="EN TRAMITE";
					break;
					case 2: status="NOTIFICADO";
					break;
					case 3: status="DEVUELTO";
					break;
				}
				
				if(!(status.Equals(dataGridView1.Rows[fila].Cells[12].FormattedValue.ToString()))){
					sql+=" estatus=\""+status+"\",";
					acierto++;
					msg_cambios+="•Estatus\n";
				}
				
				if(!(textBox19.Text.Equals(dataGridView1.Rows[fila].Cells[19].FormattedValue.ToString()))){
					if(textBox19.Text.Length>0 ||textBox19.Text.Equals("-")){
						sql+=" observaciones=\""+textBox19.Text+"\",";
						acierto++;
						msg_cambios+="•Observaciones\n";
					}else{
						error++;
						msg_error+="•Observaciones\n";
					}
				}
				
				if(!(comboBox3.Text.Equals(dataGridView1.Rows[fila].Cells[25].FormattedValue.ToString()))){
					if(comboBox3.Text.Length>5 || comboBox3.Text.Equals("-")){
						sql+=" nombre_oficio=\""+comboBox3.Text+"\",";
						acierto++;
						msg_cambios+="•Nombre Documento\n";
					}else{
						error++;
						msg_error+="•Nombre Documento\n";
					}
				}
				
				if(maskedTextBox2.Text.Equals("  /  /")){
					fecha_capt="";
				}else{
					fecha_capt=maskedTextBox2.Text;
				}
				
				if(!(fecha_capt.Equals(dataGridView1.Rows[fila].Cells[13].FormattedValue.ToString()))){
					if(maskedTextBox2.Text.Length==10){
						if(maskedTextBox2.BackColor.Name.Equals("Window")){
							sql+=" fecha_captura=\""+(maskedTextBox2.Text.Substring(6,4)+"-"+maskedTextBox2.Text.Substring(3,2)+"-"+maskedTextBox2.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios="•Fecha de Captura\n";
						}else{
							error++;
							msg_error+="•Fecha de Captura\n";
						}
					}else{
						if(maskedTextBox2.Text.Equals("  /  /")){
							sql+=" fecha_captura=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha de Captura\n";
						}
					}
				}
				
				if(maskedTextBox3.Text.Equals("  /  /")){
					fecha_recep="";
				}else{
					fecha_recep=maskedTextBox3.Text;
				}
				
				if(!(fecha_recep.Equals(dataGridView1.Rows[fila].Cells[14].FormattedValue.ToString()))){
					if(maskedTextBox3.Text.Length==10){
						if(maskedTextBox3.BackColor.Name.Equals("Window")){
							sql+=" fecha_recep_contro=\""+(maskedTextBox3.Text.Substring(6,4)+"-"+maskedTextBox3.Text.Substring(3,2)+"-"+maskedTextBox3.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios+="•Fecha de Recepción Controlador\n";
						}else{
							error++;
							msg_error+="•Fecha de Recepción Controlador\n";
						}
					}else{
						if(maskedTextBox3.Text.Equals("  /  /")){
							sql+=" fecha_recep_contro=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha de Recepción Controlador\n";
						}
					}
				}
				
				if(maskedTextBox4.Text.Equals("  /  /")){
					fecha_not="";
				}else{
					fecha_not=maskedTextBox4.Text;
				}
				
				if(!(fecha_not.Equals(dataGridView1.Rows[fila].Cells[15].FormattedValue.ToString()))){
					if(maskedTextBox4.Text.Length==10){
						if(maskedTextBox4.BackColor.Name.Equals("Window")){
							sql+=" fecha_notificacion=\""+(maskedTextBox4.Text.Substring(6,4)+"-"+maskedTextBox4.Text.Substring(3,2)+"-"+maskedTextBox4.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios+="•Fecha de Notificación\n";
						}else{
							error++;
							msg_error+="•Fecha de Notificación\n";
						}
					}else{
						if(maskedTextBox4.Text.Equals("  /  /")){
							sql+=" fecha_notificacion=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha de Notificación\n";
						}
					}
				}
				
				if(maskedTextBox5.Text.Equals("  /  /")){
					fecha_vis="";
				}else{
					fecha_vis=maskedTextBox5.Text;
				}
				
				if(!(fecha_vis.Equals(dataGridView1.Rows[fila].Cells[16].FormattedValue.ToString()))){
					if(maskedTextBox5.Text.Length==10){
						if(maskedTextBox5.BackColor.Name.Equals("Window")){
							sql+=" fecha_visita=\""+(maskedTextBox5.Text.Substring(6,4)+"-"+maskedTextBox5.Text.Substring(3,2)+"-"+maskedTextBox5.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios+="•Fecha de Visita\n";
						}else{
							error++;
							msg_error+="•Fecha de Visita\n";
						}
					}else{
						if(maskedTextBox5.Text.Equals("  /  /")){
							sql+=" fecha_visita=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha de Visita\n";
						}
					}
				}
				
				if(maskedTextBox6.Text.Equals("  /  /")){
					fecha_devo="";
				}else{
					fecha_devo=maskedTextBox6.Text;
				}
				
				if(!(fecha_devo.Equals(dataGridView1.Rows[fila].Cells[18].FormattedValue.ToString()))){
					if(maskedTextBox6.Text.Length==10){
						if(maskedTextBox6.BackColor.Name.Equals("Window")){
							sql+=" fecha_devolucion_not=\""+(maskedTextBox6.Text.Substring(6,4)+"-"+maskedTextBox6.Text.Substring(3,2)+"-"+maskedTextBox6.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios+="•Fecha de Devolución\n";
						}else{
							error++;
							msg_error+="•Fecha de Devolución\n";
						}
					}else{
						if(maskedTextBox6.Text.Equals("  /  /")){
							sql+=" fecha_devolucion_not=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha de Devolución\n";
						}
					}
				}
				
				if(maskedTextBox8.Text.Equals("  /  /")){
					fecha_origen_devo="";
				}else{
					fecha_origen_devo=maskedTextBox8.Text;
				}
				
				if(!(fecha_origen_devo.Equals(dataGridView1.Rows[fila].Cells[20].FormattedValue.ToString()))){
					if(maskedTextBox8.Text.Length==10){
						if(maskedTextBox8.BackColor.Name.Equals("Window")){
							sql+=" fecha_devo_origen=\""+(maskedTextBox8.Text.Substring(6,4)+"-"+maskedTextBox8.Text.Substring(3,2)+"-"+maskedTextBox8.Text.Substring(0,2))+"\",";
							acierto++;
							msg_cambios+="•Fecha de Devolución a Origen\n";
						}else{
							error++;
							msg_error+="•Fecha de Devolución a Origen\n";
						}
					}else{
						if(maskedTextBox8.Text.Equals("  /  /")){
							sql+=" fecha_devo_origen=NULL,";
							acierto++;
							msg_cambios+="•Se Borrará la Fecha de Devolución a Origen\n";
						}
					}
				}
				
				if(error==0){
					if(acierto>0){
					sql=sql.Substring(0,sql.Length-1);
					sql=sql+"  "+sql2;
					//MessageBox.Show(sql);
					DialogResult resu= MessageBox.Show("Se van a modificar los siguientes campos de este registro:\n\n"+msg_cambios+"\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					if(resu == DialogResult.Yes){
						conex.conectar("base_principal");
						conex.consultar(sql);
						conex.guardar_evento("Se Modificó el OFICIO con el ID: "+id);
						MessageBox.Show("Modificación realizada correctamente ","¡Exito!",MessageBoxButtons.OK,MessageBoxIcon.Information);
					
						estilo_consulta();
						textBox2.ReadOnly=true;
						textBox20.ReadOnly=true;
						textBox4.ReadOnly=true;
						textBox23.ReadOnly=true;
						textBox24.ReadOnly=true;
						textBox25.ReadOnly=true;
						textBox26.ReadOnly=true;
						textBox5.ReadOnly=true;
						textBox6.ReadOnly=true;
						textBox7.ReadOnly=true;
						textBox8.ReadOnly=true;
						maskedTextBox1.ReadOnly=true;
						textBox10.ReadOnly=true;
						textBox11.ReadOnly=true;
						checkBox1.Enabled=false;
						checkBox2.Enabled=false;
						textBox14.Visible=true;
						comboBox1.Enabled=false;
						textBox19.ReadOnly=true;
						comboBox3.Text="";
						comboBox3.SelectedIndex=-1;
						maskedTextBox2.ReadOnly=true;
						maskedTextBox3.ReadOnly=true;
						maskedTextBox4.ReadOnly=true;
						maskedTextBox5.ReadOnly=true;
						maskedTextBox6.ReadOnly=true;
						maskedTextBox7.ReadOnly=true;
						button1.Enabled=false;
						dataGridView1.Enabled=true;
						buscar_oficio();
						comboBox2.Enabled=true;						
						button2.Enabled=true;
						textBox1.ReadOnly=false;
						textBox1.Focus();
						comboBox3.Enabled=false;
						textBox27.Visible=true;
					}
					}
				}else{
					MessageBox.Show(msg_error,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}
			
		}
		
		void Oficios_consultaLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			comboBox2.SelectedIndex=0;
			
			ran=Convert.ToInt32(MainForm.datos_user_static[2]);//rango
			id_usu=MainForm.datos_user_static[7];//id_usuario
            nombre_puesto = MainForm.datos_user_static[5];//puesto

			if(ran<3||ran==6){
				button3.Enabled=true;
				
				/*conex5.conectar("base_principal");
				permisos=conex5.consultar("SELECT count(idpermisos) FROM permisos WHERE apartado=\"oficios\" and permitidos like \"%,"+id_usu+",%\"");
				if(Convert.ToInt32(permisos.Rows[0][0].ToString())>0){
						
				}else{
					maskedTextBox2.Enabled=false;
					maskedTextBox3.Enabled=false;
					maskedTextBox6.Enabled=false;
					maskedTextBox8.Enabled=false;
					
				}*/
			}else{
				button3.Enabled=false;
			}
			llenar_cb3();
			llenar_sectores_notificadores();
			estilo_consulta();
			
			if((ran==5)||(ran==0))
            {
                button4.Visible = true;
            }
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			buscar_oficio();
			
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter)){
				buscar_oficio();
			}
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			try{
				textBox2.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].FormattedValue.ToString();
				textBox20.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].FormattedValue.ToString();
				textBox3.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].FormattedValue.ToString();
				textBox4.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].FormattedValue.ToString();
				textBox23.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[21].FormattedValue.ToString();
				textBox24.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[22].FormattedValue.ToString();
				textBox25.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[23].FormattedValue.ToString();
				textBox26.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[24].FormattedValue.ToString();
				textBox5.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].FormattedValue.ToString();
				textBox6.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[5].FormattedValue.ToString();
				textBox7.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].FormattedValue.ToString();
				textBox8.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].FormattedValue.ToString();
				//textBox9.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].FormattedValue.ToString();
				maskedTextBox1.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].FormattedValue.ToString();
				textBox10.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[9].FormattedValue.ToString();
				textBox11.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[10].FormattedValue.ToString();
				textBox12.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[11].FormattedValue.ToString();
				textBox13.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[17].FormattedValue.ToString();
				textBox14.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[12].FormattedValue.ToString();
				comboBox3.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[25].FormattedValue.ToString().ToUpper();
				textBox27.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[25].FormattedValue.ToString().ToUpper();
				if(textBox12.Text.Equals("CCJAL")){
					checkBox1.Checked=true;
				}else{
					checkBox1.Checked=false;
				}
				
				if(textBox13.Text.Equals("NN")){
					checkBox2.Checked=true;
				}else{
					checkBox2.Checked=false;
				}
				
				switch(textBox14.Text){
						
					case "0": comboBox1.SelectedIndex=0;
					break;
					case "EN TRAMITE": comboBox1.SelectedIndex=1;
					break;
					case "NOTIFICADO": comboBox1.SelectedIndex=2;
					break;
					case "DEVUELTO": comboBox1.SelectedIndex=3;
					break;
					
				}
				
				textBox19.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[19].FormattedValue.ToString();
				
				maskedTextBox2.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[13].FormattedValue.ToString();
				maskedTextBox3.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[14].FormattedValue.ToString();
				maskedTextBox4.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[15].FormattedValue.ToString();
				maskedTextBox5.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[16].FormattedValue.ToString();
				maskedTextBox6.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[18].FormattedValue.ToString();
				maskedTextBox8.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[20].FormattedValue.ToString();
				maskedTextBox7.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].FormattedValue.ToString();
				
				/*textBox15.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[13].FormattedValue.ToString();
				textBox16.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[14].FormattedValue.ToString();
				textBox17.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[15].FormattedValue.ToString();
				textBox18.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[16].FormattedValue.ToString();
				textBox21.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[18].FormattedValue.ToString();
				*/
				
				if(textBox3.Text.Length>0){
        			button4.Enabled=true;
				}else{
					button4.Enabled=false;
				}
			}catch{}
		}
		//guardar
		void Button1Click(object sender, EventArgs e)
		{
			guardar();
		}
		//editar
		void Button3Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				if(textBox2.ReadOnly==true){
					timer1.Enabled=false;
					estilo_edicion();
					textBox2.ReadOnly=false;
					textBox20.ReadOnly=false;
					textBox4.ReadOnly=false;
					textBox23.ReadOnly=false;
					textBox24.ReadOnly=false;
					textBox25.ReadOnly=false;
					textBox26.ReadOnly=false;
					textBox5.ReadOnly=false;
					textBox6.ReadOnly=false;
					textBox7.ReadOnly=false;
					textBox8.ReadOnly=false;
					maskedTextBox1.ReadOnly=false;
					//textBox9.ReadOnly=false;
					//textBox10.ReadOnly=false;
					//textBox11.ReadOnly=false;
					checkBox1.Enabled=true;
					checkBox2.Enabled=true;
					comboBox1.Enabled=true;
					comboBox3.Enabled=true;
					textBox14.Visible=false;
					textBox27.Visible=false;
					//textBox12.ReadOnly=false;
					//textBox13.ReadOnly=false;
					//textBox14.ReadOnly=false;
					
					textBox19.ReadOnly=false;
					maskedTextBox2.ReadOnly=false;
					maskedTextBox3.ReadOnly=false;
					maskedTextBox4.ReadOnly=false;
					maskedTextBox5.ReadOnly=false;
					maskedTextBox6.ReadOnly=false;
					maskedTextBox7.ReadOnly=false;
					maskedTextBox8.ReadOnly=false;
					/*textBox15.ReadOnly=false;
					textBox16.ReadOnly=false;
					textBox17.ReadOnly=false;
					textBox18.ReadOnly=false;
					textBox21.ReadOnly=false;*/
					button1.Enabled=true;
					dataGridView1.Enabled=false;
					textBox1.ReadOnly=true;
					comboBox2.Enabled=false;
					button2.Enabled=false;
				}else{
					//comboBox1.Enabled=false;
					estilo_consulta();
					textBox2.ReadOnly=true;
					textBox20.ReadOnly=true;
					textBox4.ReadOnly=true;
					textBox23.ReadOnly=true;
					textBox24.ReadOnly=true;
					textBox25.ReadOnly=true;
					textBox26.ReadOnly=true;
					textBox5.ReadOnly=true;
					textBox6.ReadOnly=true;
					textBox7.ReadOnly=true;
					textBox8.ReadOnly=true;
					maskedTextBox1.ReadOnly=true;
					//textBox9.ReadOnly=true;
					textBox10.ReadOnly=true;
					textBox11.ReadOnly=true;
					checkBox1.Enabled=false;
					checkBox2.Enabled=false;
					textBox14.Visible=true;
					textBox27.Visible=true;
					comboBox1.Enabled=false;
					//textBox12.ReadOnly=false;
					//textBox13.ReadOnly=false;
					//textBox14.ReadOnly=false;
					textBox19.ReadOnly=true;
					maskedTextBox2.ReadOnly=true;
					maskedTextBox3.ReadOnly=true;
					maskedTextBox4.ReadOnly=true;
					maskedTextBox5.ReadOnly=true;
					maskedTextBox6.ReadOnly=true;
					maskedTextBox7.ReadOnly=true;
					maskedTextBox8.ReadOnly=true;
					/*textBox15.ReadOnly=true;
					textBox16.ReadOnly=true;
					textBox17.ReadOnly=true;
					textBox18.ReadOnly=true;
					textBox21.ReadOnly=true;*/
					button1.Enabled=false;
					dataGridView1.Enabled=true;
					textBox1.ReadOnly=false;
					comboBox2.Enabled=true;
					comboBox3.Enabled=false;
					button2.Enabled=true;
					timer1.Enabled=true;
				}
			}
		}
		
		void MaskedTextBox1Leave(object sender, EventArgs e)
		{
			sector_auto();
		}
		
		void TextBox7ReadOnlyChanged(object sender, EventArgs e)
		{
			
		}
		
		void MaskedTextBox1TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox1.Text.Length==2){
				sector_auto();
			}
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true){
				checkBox1.BackColor=System.Drawing.Color.MediumSeaGreen;
			}else{
				checkBox1.BackColor=System.Drawing.Color.Transparent;
			}
		}
		
		void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox2.Checked==true){
				checkBox2.BackColor=System.Drawing.Color.Red;
			}else{
				checkBox2.BackColor=System.Drawing.Color.Transparent;
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(comboBox1.SelectedIndex){
						
					case 0: textBox14.Text="0";
					break;
					case 1: textBox14.Text="EN TRAMITE";
					break;
					case 2: textBox14.Text="NOTIFICADO";
					break;
					case 3: textBox14.Text="DEVUELTO";
					break;
					
				}
		}
		
		void MaskedTextBox7TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox7.Text.Length==10){
				if(verificar_fecha(maskedTextBox7.Text).Length>1){
					maskedTextBox7.Text=verificar_fecha(maskedTextBox7.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox7.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox7.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					maskedTextBox7.BackColor=System.Drawing.Color.Red;
					maskedTextBox7.ForeColor=System.Drawing.Color.White;
				}
			}
		}
		
		void MaskedTextBox7Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox7.Text).Length>1){
					maskedTextBox7.Text=verificar_fecha(maskedTextBox7.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox7.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox7.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox7.BackColor=System.Drawing.Color.Red;
						maskedTextBox7.ForeColor=System.Drawing.Color.White;
					}
			}
		}
		
		void MaskedTextBox2TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox2.Text.Length==10){
				if(verificar_fecha(maskedTextBox2.Text).Length>1){
					maskedTextBox2.Text=verificar_fecha(maskedTextBox2.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox2.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox2.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					maskedTextBox2.BackColor=System.Drawing.Color.Red;
					maskedTextBox2.ForeColor=System.Drawing.Color.White;
				}
			}
		}
		
		void MaskedTextBox2Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox2.Text).Length>1){
					maskedTextBox2.Text=verificar_fecha(maskedTextBox2.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox2.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox2.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox2.BackColor=System.Drawing.Color.Red;
						maskedTextBox2.ForeColor=System.Drawing.Color.White;
					}
				}
		}
		
		void MaskedTextBox3TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox3.Text.Length==10){
				if(verificar_fecha(maskedTextBox3.Text).Length>1){
					maskedTextBox3.Text=verificar_fecha(maskedTextBox3.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox3.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					maskedTextBox3.BackColor=System.Drawing.Color.Red;
					maskedTextBox3.ForeColor=System.Drawing.Color.White;
				}
			}
		}
		
		void MaskedTextBox3Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox3.Text).Length>1){
					maskedTextBox3.Text=verificar_fecha(maskedTextBox3.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox3.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox3.BackColor=System.Drawing.Color.Red;
						maskedTextBox3.ForeColor=System.Drawing.Color.White;
					}
				}
		}
			
		void MaskedTextBox4TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox4.Text.Length==10){
				if(verificar_fecha(maskedTextBox4.Text).Length>1){
					maskedTextBox4.Text=verificar_fecha(maskedTextBox4.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox4.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					maskedTextBox4.BackColor=System.Drawing.Color.Red;
					maskedTextBox4.ForeColor=System.Drawing.Color.White;
				}
			}
		}
		
		void MaskedTextBox4Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox4.Text).Length>1){
					maskedTextBox4.Text=verificar_fecha(maskedTextBox4.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox4.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox4.BackColor=System.Drawing.Color.Red;
						maskedTextBox4.ForeColor=System.Drawing.Color.White;
					}
				}
		}
		
		void MaskedTextBox5TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox5.Text.Length==10){
				if(verificar_fecha(maskedTextBox5.Text).Length>1){
					maskedTextBox5.Text=verificar_fecha(maskedTextBox5.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox5.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					maskedTextBox5.BackColor=System.Drawing.Color.Red;
					maskedTextBox5.ForeColor=System.Drawing.Color.White;
				}
			}
		}
		
		void MaskedTextBox5Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox5.Text).Length>1){
					maskedTextBox5.Text=verificar_fecha(maskedTextBox5.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox5.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox5.BackColor=System.Drawing.Color.Red;
						maskedTextBox5.ForeColor=System.Drawing.Color.White;
					}
				}
		}
		
		void MaskedTextBox6TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox6.Text.Length==10){
				if(verificar_fecha(maskedTextBox6.Text).Length>1){
					maskedTextBox6.Text=verificar_fecha(maskedTextBox6.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox6.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox6.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox6.BackColor=System.Drawing.Color.Red;
						maskedTextBox6.ForeColor=System.Drawing.Color.White;
					}
				}
			}
		}
		
		void MaskedTextBox6Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox6.Text).Length>1){
					maskedTextBox6.Text=verificar_fecha(maskedTextBox6.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox6.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox6.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox6.BackColor=System.Drawing.Color.Red;
						maskedTextBox6.ForeColor=System.Drawing.Color.White;
					}
				}
		}
		
		void maskedTextBox8TextChanged(object sender, EventArgs e)
		{
			if(maskedTextBox8.Text.Length==10){
				if(verificar_fecha(maskedTextBox8.Text).Length>1){
					maskedTextBox8.Text=verificar_fecha(maskedTextBox8.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox8.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox8.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox8.BackColor=System.Drawing.Color.Red;
						maskedTextBox8.ForeColor=System.Drawing.Color.White;
					}
				}
			}
		}
		
		void maskedTextBox8Leave(object sender, EventArgs e)
		{
			if(verificar_fecha(maskedTextBox8.Text).Length>1){
					maskedTextBox8.Text=verificar_fecha(maskedTextBox8.Text);
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox8.BackColor=System.Drawing.SystemColors.Window;
						maskedTextBox8.ForeColor=System.Drawing.SystemColors.ControlText;
					}
					
				}else{
					if(textBox2.BackColor.Name.Equals("Window")){
						maskedTextBox8.BackColor=System.Drawing.Color.Red;
						maskedTextBox8.ForeColor=System.Drawing.Color.White;
					}
				}
		}
		
		void TextBox9TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox2SelectionChangeCommitted(object sender, EventArgs e)
		{
			textBox1.Focus();
			if(comboBox2.SelectedIndex==6){
				textBox1.Clear();
				textBox1.Text="Oficios_";
				textBox1.SelectionStart=8;
			}
			
			
		}

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox3.Text.Length>0&&dataGridView1.RowCount>0){
                Estrados.Captura_info_estrados ofi_estra = new Estrados.Captura_info_estrados(3,Convert.ToInt32(textBox3.Text),"");
                ofi_estra.Show();
                this.Hide();
            }
        }
	}
}

/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 30/08/2016
 * Hora: 09:50 a.m.
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
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Supervision
{
	/// <summary>
	/// Description of Estado_noti.
	/// </summary>
	public partial class Estado_noti : Form
	{
		public Estado_noti()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		int i=0,z=0, indice=0;
		int[] contros;
		String sql,sql2,sql3,sql4,periodo,controlador,notificador,estado_0,en_tramite,notificado,cartera,depurado,nn,cancelado,traspaso,otros,pag,periodo_contro;
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();	
		Conexion conex4 = new Conexion();		
		
		DataTable tabla_consulta = new DataTable();
		
		//periodos
		public void llenar_Cb2(){
			conex.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT nombre_periodo FROM base_principal.datos_factura ORDER BY nombre_periodo;");
			do{
				comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
            conex.cerrar();
		}	
		//periodos especiales
		public void llenar_Cb2_esp(){
			conex.conectar("base_principal");
			comboBox2.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT periodo_factura FROM base_principal.datos_factura WHERE periodo_factura <> \"-\" ORDER BY periodo_factura;");
			do{
				comboBox2.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
            conex.cerrar();
		}
		//controladores
		public void llenar_Cb4(){
			conex.conectar("base_principal");
			comboBox4.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre,id_usuario FROM base_principal.usuarios WHERE puesto =\"Controlador\" ORDER BY apellido;");
			//dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (controlador) FROM base_principal.datos_factura ORDER BY controlador;");
			contros = new int[dataGridView3.RowCount];
			do{
				comboBox4.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				//comboBox4.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				contros[i] = Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			//comboBox4.Items.Add("--NINGUNO--");
            conex.cerrar();
		}	
		//notificadores
		public void llenar_Cb3(){
			conex.conectar("base_principal");
			comboBox3.Items.Clear();
			i=0;
			//dataGridView3.DataSource = conex.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" ORDER BY apellido;");
			dataGridView3.DataSource = conex.consultar("SELECT DISTINCT (notificador) FROM base_principal.datos_factura ORDER BY notificador;");
			do{
				//comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			//comboBox3.Items.Add("--NINGUNO--");
            conex.cerrar();
		}
		//notificadores v2
		public void llenar_Cb3_v2(){
			conex2.conectar("base_principal");
			comboBox3.Items.Clear();
			i=0;
			dataGridView3.DataSource = conex2.consultar("SELECT apellido,nombre FROM base_principal.usuarios WHERE puesto =\"Notificador\" AND controlador = \""+contros[comboBox4.SelectedIndex]+"\" ORDER BY apellido;");
			do{
				comboBox3.Items.Add(dataGridView3.Rows[i].Cells[0].Value.ToString()+" "+dataGridView3.Rows[i].Cells[1].Value.ToString());
				i++;
			}while(i<dataGridView3.RowCount);
			i=0;
			//comboBox3.Items.Add("--NINGUNO--");
            conex2.cerrar();
		}
		
		public void estado_inicial(){
			
			conex.conectar("base_principal");
			conex3.conectar("base_principal");
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"0\" AND nn<>\"NN\";");
			textBox1.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			//dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"EN TRAMITE\" AND nn<>\"NN\" AND observaciones<>\"PAGADO\";");
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"EN TRAMITE\" AND nn<>\"NN\";");
			textBox2.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			//dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status LIKE \"DEPURA%\" AND nn<>\"NN\"  OR (status=\"EN TRAMITE\" AND observaciones=\"PAGADO\");");
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status LIKE \"DEPURA%\" AND nn<>\"NN\";");
			textBox3.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"NOTIFICADO\" AND nn<>\"NN\";");
			textBox4.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"CARTERA\" AND nn<>\"NN\";");
			textBox5.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"CANCELADO\" AND nn<>\"NN\";");
			textBox6.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"TRASPASO\" AND nn<>\"NN\";");
			textBox7.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"REPOSICION\" AND nn<>\"NN\";");
			textBox11.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status <>\"0\" AND status <>\"EN TRAMITE\" AND "+
			"status NOT LIKE \"DEPURA%\" AND status <>\"NOTIFICADO\" AND status <>\"CARTERA\" AND status <>\"CANCELADO\" AND status <>\"TRASPASO\" AND nn<>\"NN\"");
			textBox8.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE nn=\"NN\";");
			textBox9.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			
			
			textBox10.Text=(Convert.ToInt32(textBox1.Text)+Convert.ToInt32(textBox2.Text)+Convert.ToInt32(textBox3.Text)+Convert.ToInt32(textBox4.Text)+
			               Convert.ToInt32(textBox5.Text)+Convert.ToInt32(textBox6.Text)+Convert.ToInt32(textBox7.Text)+Convert.ToInt32(textBox8.Text)+Convert.ToInt32(textBox9.Text)+Convert.ToInt32(textBox11.Text)).ToString();
			
		}
		
		public void estado_periodo(){
			
			int x=0,s0=0,en_tram=0,depur=0,noti=0,carter=0,enn=0,cance=0,tras=0,otro=0,repo=0;
			String statu,mm,fecha_entrega,fecha_recepcion,nombre_per,nombre_per_previo="";
			TimeSpan dias_retraso;
			DateTime fecha_not;
			
			conex4.conectar("base_principal");
			DataTable tabla_consulta_e_p = new DataTable();
			
			while(x<dataGridView1.RowCount){
				statu=dataGridView1.Rows[x].Cells[17].FormattedValue.ToString();
				mm=dataGridView1.Rows[x].Cells[18].FormattedValue.ToString();
				fecha_entrega=dataGridView1.Rows[x].Cells[11].FormattedValue.ToString();
				fecha_recepcion=dataGridView1.Rows[x].Cells[12].FormattedValue.ToString();
				nombre_per=dataGridView1.Rows[x].Cells[0].FormattedValue.ToString();
				
				if(dataGridView1.Rows[x].Cells[13].FormattedValue.ToString().Length>0){
					fecha_not=Convert.ToDateTime(dataGridView1.Rows[x].Cells[13].FormattedValue.ToString());
				}else{
					fecha_not=System.DateTime.Now;
				}
				//MessageBox.Show(statu);
				switch(statu){
						
						case "0":  if(!mm.Equals("NN")){
										s0++;
									}else{
										enn++;
									}
						break;
						
						case "EN TRAMITE":  if(!mm.Equals("NN")){
												en_tram++;
											}else{
												enn++;
											}
						break;
						
						case "NOTIFICADO":  if(!mm.Equals("NN")){
												noti++;
											}else{
												enn++;
											}
						break;
						
						case "CARTERA":  if(!mm.Equals("NN")){
												carter++;
											}else{
												enn++;
											}
						break;
						
						case "CANCELADO":  if(!mm.Equals("NN")){
												cance++;
											}else{
												enn++;
											}
						break;
						
						case "TRASPASO":  if(!mm.Equals("NN")){
												tras++;
											}else{
												enn++;
											}
						break;
						
						case "REPOSICION":  if(!mm.Equals("NN")){
												repo++;
											}else{
												enn++;
											}
						break;
						
						default:  
						
						if(!mm.Equals("NN")){
							if(statu.StartsWith("DEPU")){
								depur++;
							}else{
								otro++;
							}
						}else{
							enn++;
						}
						break;
				}
				
				if(fecha_entrega.Length==10){
					if(fecha_recepcion.Length==10){
						dias_retraso=(Convert.ToDateTime(fecha_recepcion))-(Convert.ToDateTime(fecha_entrega));
						tabla_consulta.Rows[x][14]=dias_retraso.Days;
					}else{
						dias_retraso=(System.DateTime.Today)-(Convert.ToDateTime(fecha_entrega));
						tabla_consulta.Rows[x][14]=dias_retraso.Days;
					}
				}
				
				//verificar cifra control
				if(nombre_per.Length>0){
					try{
						if(nombre_per!=nombre_per_previo){
							tabla_consulta_e_p=conex4.consultar("SELECT fecha_cifra_control_termino FROM estado_periodos WHERE nombre_periodo=\""+nombre_per+"\"");
							nombre_per_previo=nombre_per;
						}
						
						if(tabla_consulta_e_p.Rows.Count>0){
							if(Convert.ToDateTime(tabla_consulta_e_p.Rows[0][0].ToString())>=fecha_not){
								tabla_consulta.Rows[x][16]="DENTRO";
							}else{
								tabla_consulta.Rows[x][16]="FUERA";
							}
						}
					}catch(Exception f){
						
					}
				}
				//fin verificacion cifra control
				x++;
			}
			
			textBox1.Text=s0.ToString();
			textBox2.Text=en_tram.ToString();
			textBox3.Text=depur.ToString();
			textBox4.Text=noti.ToString();
			textBox5.Text=carter.ToString();
			textBox9.Text=enn.ToString();
			textBox6.Text=cance.ToString();
			textBox7.Text=tras.ToString();
			textBox8.Text=otro.ToString();
			textBox11.Text=repo.ToString();
			textBox10.Text=(s0+en_tram+depur+noti+carter+enn+cance+tras+otro).ToString();
			/*
			
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM dat" AND status=\"0\" AND nn<>\"NN\" ;");
			textBox1.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			//dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status=\"EN TRAMITE\" AND nn<>\"NN\" AND observaciones<>\"PAGADO\";");
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM dater+"\" AND status=\"EN TRAMITE\" AND nn<>\"NN\";");
			textBox2.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			//dataGridView2.DataSource=conex.consultar("SELECT COUNT(id) FROM datos_factura WHERE status LIKE \"DEPURA%\" AND nn<>\"NN\"  OR (status=\"EN TRAMITE\" AND observaciones=\"PAGADO\");");
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND status LIKE \"DEPURA%\" AND nn<>\"NN\";");
			textBox3.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND status=\"NOTIFICADO\" AND nn<>\"NN\";");
			textBox4.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND status=\"CARTERA\" AND nn<>\"NN\";");
			textBox5.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND status=\"CANCELADO\" AND nn<>\"NN\";");
			textBox6.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND status=\"TRASPASO\" AND nn<>\"NN\";");
			textBox7.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND status <>\"0\" AND status <>\"EN TRAMITE\" AND "+
			"status NOT LIKE \"DEPURA%\" AND status <>\"NOTIFICADO\" AND status <>\"CARTERA\" AND status <>\"CANCELADO\" AND status <>\"TRASPASO\" AND nn<>\"NN\"");
			textBox8.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			dataGridView2.DataSource=conex3.consultar("SELECT COUNT(id) FROM datos_factura WHERE nombre_periodo=\""+per+"\" AND nn=\"NN\";");
			textBox9.Text=dataGridView2.Rows[0].Cells[0].Value.ToString();
			
			textBox10.Text=(Convert.ToInt32(textBox1.Text)+Convert.ToInt32(textBox2.Text)+Convert.ToInt32(textBox3.Text)+Convert.ToInt32(textBox4.Text)+
			               Convert.ToInt32(textBox5.Text)+Convert.ToInt32(textBox6.Text)+Convert.ToInt32(textBox7.Text)+Convert.ToInt32(textBox8.Text)+Convert.ToInt32(textBox9.Text)).ToString();
			*/
		}
		
		public String consulta_parte1(){
		
			periodo="";
			controlador="";
			notificador="";
			sql2="";
			
			if(comboBox2.SelectedIndex > -1){
				if(comboBox1.SelectedIndex==0){
					periodo="nombre_periodo=\""+comboBox2.SelectedItem.ToString()+"\" ";
				}else{
					periodo="periodo_factura=\""+comboBox2.SelectedItem.ToString()+"\" ";
				}
			}
			
			if(comboBox4.SelectedIndex > -1){
				controlador="AND controlador=\""+comboBox4.SelectedItem.ToString()+"\" ";
			}
			
			if(comboBox3.SelectedIndex > -1){
				notificador="AND notificador=\""+comboBox3.SelectedItem.ToString()+"\" ";
			}
			
			sql2=periodo+controlador+notificador;
			
			if(sql2.StartsWith("AND")){
			   	sql2=sql2.Substring(3);
			}
			
			return sql2;
		}
		
		public String consulta_status(){
			
			estado_0="";
			en_tramite="";
			depurado="";
			notificado="";
			cartera="";
			cancelado="";
			traspaso="";
			otros="";
			sql3="";
			
			if(checkBox1.Checked==true){
				estado_0="status=\"0\" ";
			}else{
				otros+="status<>\"0\" ";
			}
			
			if(checkBox2.Checked==true){
				en_tramite="OR status=\"EN TRAMITE\" ";
			}else{
				otros+="AND status <>\"EN TRAMITE\" ";
			}
			
			if(checkBox3.Checked==true){
				depurado="OR status LIKE \"DEPURACION%\" ";
			}else{
				otros+="AND status NOT LIKE \"DEPURACION%\" ";
			}
			
			if(checkBox4.Checked==true){
				notificado="OR status=\"NOTIFICADO\" ";
			}else{
				otros+="AND status <>\"NOTIFICADO\" ";
			}
			
			if(checkBox5.Checked==true){
				cartera="OR status=\"CARTERA\" ";
			}else{
				otros+="AND status <>\"CARTERA\" ";
			}
			
			if(checkBox7.Checked==true){
				cancelado="OR status=\"CANCELADO\" ";
			}else{
				otros+="AND status <>\"CANCELADO\" ";
			}
			
			if(checkBox8.Checked==true){
				traspaso="OR status=\"TRASPASO\" ";
			}else{
				otros+="AND status <>\"TRASPASO\" ";
			}
			
			if(checkBox12.Checked==true){
				traspaso="OR status=\"REPOSICION\" ";
			}else{
				otros+="AND status <>\"REPOSICION\" ";
			}

			sql3=estado_0+en_tramite+depurado+notificado+cartera+cancelado+traspaso;
			
			if(checkBox9.Checked==true){//otros
				
				if(otros.StartsWith("AND")){
			   		otros=otros.Substring(3);
				}
				sql3=otros;
			}
			
			if(checkBox10.Checked==true){//0TODO
				sql3="";
			}
			
			if(sql3.StartsWith("OR")){
			   		sql3=sql3.Substring(2);
			}
			
			return sql3;
		}
		
		public String consulta_parte3(){
			
			nn="";
			sql4="";
			
			if(radioButton1.Checked==true){//NN
				nn=" nn=\"NN\" ";	
			}else{
				if(radioButton2.Checked==true){//ESTRADOS
					nn=" nn=\"ESTRADOS\" ";
				}else{//"-"
					if(radioButton3.Checked==true){//"-"
						nn="(nn<>\"NN\" AND nn<>\"ESTRADOS\") ";
					}else{
						if(radioButton7.Checked==true){//Todo
							nn="";
						}
					}
				}
			}

			if(radioButton5.Checked==true){//Sin PAGADOS
				pag="AND observaciones NOT LIKE \"PAGADO%\"";	
			}else{
				if(radioButton6.Checked==true){//Sólo
					pag="AND observaciones LIKE \"PAGADO%\"";	
				}else{//Con
					pag="";
				}
			}
			
			sql4=nn+pag;
			
			if(sql4.StartsWith("AND")){
			   	sql4=sql4.Substring(3);
			}
				
			
			return sql4;
		}
		
		public bool checa_vacio(){
			int llave=0;
			
			if(comboBox2.SelectedIndex>-1){
				llave++;
			}
			
			if(comboBox4.SelectedIndex>-1){
				llave++;
			}
			
			if(comboBox3.SelectedIndex>-1){
				llave++;
			}
			
			if(checkBox1.Checked==true){
				llave++;
			}
			
			if(checkBox2.Checked==true){
				llave++;
			}
			
			if(checkBox3.Checked==true){
				llave++;
			}
			
			if(checkBox4.Checked==true){
				llave++;
			}
			
			if(checkBox5.Checked==true){
				llave++;
			}
			
			if(checkBox12.Checked==true){
				llave++;
			}
			
			if(checkBox7.Checked==true){
				llave++;
			}
			
			if(checkBox8.Checked==true){
				llave++;
			}
			
			if(checkBox9.Checked==true){
				llave++;
			}
			
			if(checkBox10.Checked==true){
				llave++;
			}
			
			if(radioButton1.Checked==true||radioButton2.Checked==true||radioButton3.Checked==true){
				llave++;
			}
			
			if(radioButton5.Checked==true||radioButton6.Checked==true){
				llave++;
			}
			
			if(llave==0){
				return true;
			}else{
				return false;
			}
		}
		
		public void recibe(String per)
		{
			this.periodo_contro = per;
			z=1;
		}
		
		public void recibe_consulta(){
			String nom,ape,contro;
			int c=0;
			nom = MainForm.datos_user_static[4];
			ape = MainForm.datos_user_static[9];
			contro=ape+" "+nom;
			//contro="Palacios Alonso Francisco Javier";
			
			while(c < comboBox2.Items.Count){
				if(periodo_contro.Equals(comboBox2.Items[c].ToString())){
					comboBox2.SelectedIndex=c;
				}
				c++;
			}
			
			c=0;
			
			while(c < comboBox4.Items.Count){
				if(contro.Equals(comboBox4.Items[c].ToString())){
					comboBox4.SelectedIndex=c;
				}
				c++;
			}
			
			comboBox2.Text=periodo_contro;
			comboBox4.Text=contro;
			checkBox2.Checked=true;
						
			consulta();
			
		}
		
		public void consulta(){
			try{
				
				sql="SELECT nombre_periodo,registro_patronal,periodo,credito_cuotas,credito_multa,importe_cuota,importe_multa,sector_notificacion_inicial,sector_notificacion_actualizado,controlador,notificador, "+
					"fecha_entrega,fecha_recepcion,fecha_notificacion,dias_retraso,fecha_cartera,estado_cifra_control,status,nn,folio_sipare_sua,importe_pago,porcentaje_pago,num_pago,fecha_pago,fecha_depuracion,observaciones FROM datos_factura ";
				
				sql2=consulta_parte1();
				sql3=consulta_status();
				sql4=consulta_parte3();
				
				//TimeSpan dias_retraso;
				//dias_retraso= System.DateTime.Today - Convert.ToDateTime("30-09-2016");
				//MessageBox.Show(dias_retraso.Days.ToString());
				//MessageBox.Show(sql2+"\n"+sql3+"\n"+sql4);
				if(checa_vacio()==false){
					
					if(sql2.Length>0){
						sql2=" WHERE "+sql2;
					}else{
						if(sql3.Length>0){
							sql3=" WHERE ("+sql3+")";
						}else{
							if(sql4.Length>0){
								sql4=" WHERE "+sql4;
							}else{
							}
						}
					}
					
					//MessageBox.Show(sql2+"\n"+sql3+"\n"+sql4);
					
					if(sql2.Length>0){
						sql=sql+sql2;
					}
					
					if(sql3.Length>0){
						if(sql2.Length>0){
							sql=sql+" AND ("+sql3+")";
						}else{
							sql=sql+" "+sql3;
						}
					}
					
					if(sql4.Length>0){
						if(sql3.Length>0 || sql2.Length>0){
							sql=sql+" AND "+sql4;	
						}else{
							sql=sql+" "+sql4;
						}
					}
					
					//MessageBox.Show(sql);
					tabla_consulta=conex.consultar(sql);
					dataGridView1.DataSource=tabla_consulta;
					label14.Text="Total: "+dataGridView1.RowCount;
					label14.Refresh();
					
					estado_periodo();
				
				}else{
					//MessageBox.Show(sql);
					estado_inicial();
					dataGridView1.DataSource=conex.consultar(sql);
					label14.Text="Total: "+dataGridView1.RowCount;
					label14.Refresh();
				}
				
				//comboBox2.SelectedIndex=-1;
				//comboBox3.SelectedIndex=-1;
				//comboBox4.SelectedIndex=-1;
			}catch(Exception errorsql){
				MessageBox.Show(errorsql.ToString());
			}
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
		
		void CheckBox10CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox10.Checked==true){
				checkBox1.Checked=false;
				checkBox2.Checked=false;
				checkBox3.Checked=false;
				checkBox4.Checked=false;
				checkBox5.Checked=false;
				checkBox7.Checked=false;
				checkBox8.Checked=false;
				checkBox9.Checked=false;
				checkBox12.Checked=false;
			}
		}
		
		void Estado_notiLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

			llenar_Cb2();
			llenar_Cb3();
			llenar_Cb4();
			comboBox1.SelectedIndex=0;
			
			int d=0;
			String nom,ape,contro;
			
			nom = MainForm.datos_user_static[4];
			ape = MainForm.datos_user_static[9];
			contro=ape+" "+nom;
			
			if(z==0){
				//estado_inicial();
			}else{
				recibe_consulta();
			}
			
			
			if(Convert.ToInt32(MainForm.datos_user_static[2].ToString()) < 2){
				modificarToolStripMenuItem.Visible=true;
				contextMenuStrip1.Refresh();
			}
			
			/*if(MainForm.datos_user_static[5].Equals("Controlador")){
				d=0;
				
				while(d < comboBox4.Items.Count){
					if(contro.Equals(comboBox4.Items[d].ToString())){
						comboBox4.SelectedIndex=d;
					}
					d++;
				}
				
				comboBox4.Text=contro;

                checkBox1.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
                checkBox12.Enabled = false;
                //checkBox10.Enabled = false;
                panel1.Visible = false;
                checkBox11.Checked = false;
                
			}*/
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			consulta();			
		}
		
		void CheckBox9CheckedChanged(object sender, EventArgs e)
		{
			
		}
		
		void CheckBox11CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox11.Checked==true){
				panel1.Visible=true;
				if(textBox10.Text.Length<1){
					estado_inicial();
				}
			}else{
				panel1.Visible=false;
			}
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			if(checkBox10.Checked==true){
				if(checkBox1.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox2.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox3.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox4.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox5.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox12.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox7.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox8.Checked==true){
					checkBox10.Checked=false;
				}
				if(checkBox9.Checked==true){
					checkBox10.Checked=false;
				}
			
			}
			
			if(comboBox2.Text.Length==0){
				//comboBox2.SelectedIndex=-1;
			}
			
			if(comboBox3.Text.Length==0){
				//comboBox3.SelectedIndex=-1;
			}
			
			if(comboBox4.Text.Length==0){
				//comboBox4.SelectedIndex=-1;
				//llenar_Cb3();
			}
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox4SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox4.SelectedIndex > -1){
				try{
					llenar_Cb3_v2();
				}catch(Exception es){
					comboBox3.Items.Clear();
				}
			}else{
				llenar_Cb3();
			}
			
		}
		
		void CheckBox6CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox6.Checked==true){
				panel2.Height=44;
				groupBox2.Enabled=false;
				groupBox3.Enabled=false;
				groupBox4.Enabled=false;
				comboBox2.Enabled=false;
				comboBox3.Enabled=false;
				comboBox4.Enabled=false;
				checkBox6.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_down;
				label14.Location= new System.Drawing.Point(label14.Location.X, 48);
				checkBox11.Location=new System.Drawing.Point(checkBox11.Location.X, 48);
				dataGridView1.Location=new System.Drawing.Point(dataGridView1.Location.X,72);
				dataGridView1.Height=(button3.Location.Y - label14.Location.Y)-28;
				panel1.Location= new System.Drawing.Point(panel1.Location.X,72);
				panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
				dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
				
			}else{
				panel2.Height=210;
				groupBox2.Enabled=true;
				groupBox3.Enabled=true;
				groupBox4.Enabled=true;
				comboBox2.Enabled=true;
				comboBox3.Enabled=true;
				comboBox4.Enabled=true;
				checkBox6.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_up;
				label14.Location= new System.Drawing.Point(label14.Location.X, 210);
				checkBox11.Location=new System.Drawing.Point(checkBox11.Location.X, 213);
				dataGridView1.Location=new System.Drawing.Point(dataGridView1.Location.X, 237);
				dataGridView1.Height=(button3.Location.Y - label14.Location.Y)-30;
				panel1.Location=new System.Drawing.Point(panel1.Location.X,237);
				panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
				dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			}
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void ComboBox4TextUpdate(object sender, EventArgs e)
		{
			if(comboBox4.Text.Length < 1){
				comboBox4.SelectedIndex=-1;
			}
		}
		
		void ComboBox2TextUpdate(object sender, EventArgs e)
		{
			if(comboBox2.Text.Length==0){
				comboBox2.SelectedIndex=-1;
			}
		}
		
		void ComboBox3TextUpdate(object sender, EventArgs e)
		{
			if(comboBox3.Text.Length==0){
				comboBox3.SelectedIndex=-1;
			}
		}
		
		void DataGridView1MouseDown(object sender, MouseEventArgs e)
		{
			if(dataGridView1.RowCount>0){
						if(e.Button == MouseButtons.Right){
							if ((0 <= (dataGridView1.HitTest(e.X, e.Y).RowIndex)) && ((dataGridView1.HitTest(e.X, e.Y).RowIndex) <= (dataGridView1.RowCount-1)))
							{
								dataGridView1.ClearSelection();
								indice=dataGridView1.HitTest(e.X,e.Y).RowIndex;
								dataGridView1.Rows[indice].Selected=true;
								dataGridView1.CurrentCell=dataGridView1[dataGridView1.HitTest(e.X, e.Y).ColumnIndex,dataGridView1.HitTest(e.X, e.Y).RowIndex];
								//MessageBox.Show(nss);
							}
						}
					}
		}
		
		void ConsultaIndividualToolStripMenuItemClick(object sender, EventArgs e)
		{
			Consulta_patron consu = new Consulta_patron();
			consu.Show();
			consu.consulta_inicial(dataGridView1.Rows[indice].Cells[1].FormattedValue.ToString(),dataGridView1.Rows[indice].Cells[3].FormattedValue.ToString());
			
		}
		
		void ModificarToolStripMenuItemClick(object sender, EventArgs e)
		{
			Universal.Insertar modi = new Nova_Gear.Universal.Insertar();
			modi.modificacion();
			modi.Show();
			modi.modificacion_inicial(dataGridView1.Rows[indice].Cells[1].FormattedValue.ToString(),dataGridView1.Rows[indice].Cells[3].FormattedValue.ToString(),
			                          dataGridView1.Rows[indice].Cells[4].FormattedValue.ToString());
			
            //modi.Focus();
		}
		
		void Label12Click(object sender, EventArgs e)
		{
			//Productividad_noti produ = new Productividad_noti();
			//produ.Show();
		}
		
		void GroupBox3Enter(object sender, EventArgs e)
		{
			
		}
		
		void Button33Click(object sender, EventArgs e)
		{
			Supervision.Productividad_noti produ = new Nova_Gear.Supervision.Productividad_noti();
			produ.Show();
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(comboBox1.SelectedIndex==0){
				llenar_Cb2();
			}else{
				llenar_Cb2_esp();
			}
		}

        private void exportarAExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                //GUARDAR EVIDENCIA EDICION
                SaveFileDialog dialog_save = new SaveFileDialog();
                dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
                dialog_save.Title = "Guardar Archivo de Excel";//le damos un titulo a la ventana

                if (dialog_save.ShowDialog() == DialogResult.OK)
                {
                    DataTable nn_excel = copiar_datagrid();

                    //tabla_excel
                    XLWorkbook wb = new XLWorkbook();
                    wb.Worksheets.Add(nn_excel, "Lista");
                    wb.SaveAs(@"" + dialog_save.FileName + "");
                    //MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    MessageBox.Show("El archivo se ha guardado Correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
	}
}

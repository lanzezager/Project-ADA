using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Nova_Gear.Inventario
{
    public partial class Consulta_verificacion : Form
    {
        public Consulta_verificacion()
        {
            InitializeComponent();
        }
        
        int reg_ant=0,cambiar=0,fila_activa=0,regreso=0;
        String responsable,auxiliar,sql_persist,libro,reg_persist,cred_persist;	
        //Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		DataTable user_verif = new DataTable();
		DataTable consultamysql = new DataTable();
		DataTable tabla2 = new DataTable();
		DataTable consulta = new DataTable();
		DataTable noms_pat = new DataTable();
		
		public void buscar(){
        	
        	String sql,nrp,nrp_limpio,credito;
        	int i=0,consultar=0,j=0;
        	
        	reg_persist="";
        	cred_persist="";
        	nrp=maskedTextBox1.Text;
        	credito=maskedTextBox2.Text;
        	nrp_limpio="";
        	tabla2.Rows.Clear();

            label25.Visible = false;
            button6.Visible = false;
            maskedTextBox6.Visible = false;
            maskedTextBox6.Clear();
            maskedTextBox6.BackColor = System.Drawing.SystemColors.Window;
        	
        	//se quitan guiones y espacios en blanco
        	while(i<nrp.Length){
        		if((nrp.Substring(i,1)).Equals(" ")){
        		}else{
        			nrp_limpio += nrp.Substring(i,1);
        		}
        		i++;
        	}
        	
        	i=0;
        	
        	while(i<credito.Length){
        		if((credito.Substring(i,1)).Equals(" ")){
        		}else{
        			cred_persist += credito.Substring(i,1);
        		}
        		i++;
        	}
        	
        	reg_persist=nrp_limpio;
        	
        	
        	nrp="";
        	if(nrp_limpio.Length>=3){
				nrp="reg_pat LIKE \"%"+nrp_limpio+"%\" ";
				
				if(credito.Length>=3){
			      credito= "AND credito like\"%"+maskedTextBox2.Text+"%\" ";
				}else{
					credito="";
				}
            	consultar=1;
			}else{
				if(credito.Length>=3){
			      credito= "credito like\"%"+maskedTextBox2.Text+"%\" ";
			      consultar=1;
				}else{
					credito="";
				}
							
			}
        	
        	if(consultar>0){
        		sql="SELECT idbase_inventario,clase_doc,reg_pat,credito,periodo_mes,periodo_anio,importe,tipo_doc,incidencia,sector,mov,fecha_mov,ce,fecha_alta,fecha_not,fecha_incidencia,dias,coincidente "+
        			"FROM base_inventario WHERE "+nrp+credito+" ORDER BY credito ";
        		sql_persist=sql;
        		consulta=conex.consultar(sql);
        		DataView vista = new DataView(consulta);
        		
        		vista.RowFilter="coincidente ='-'";
        		vista.Sort="credito ASC";
        		tabla2=vista.ToTable();
        		dataGridView1.DataSource=tabla2;
        		label22.Text="Créditos Encontrados: "+dataGridView1.RowCount;
        		
        		//panel1.Visible=false;
        		//panel2.Visible=false;
        		//panel3.Visible=false;
        		
        		if(dataGridView1.RowCount<1){
        			panel2.Visible=true;
        			label16.Visible=true;
        			button1.Enabled=false;
					
        			if(consulta.Rows.Count>tabla2.Rows.Count){
	        			if((consulta.Rows.Count-tabla2.Rows.Count)==consulta.Rows.Count){
	        				MessageBox.Show("El Registro y Crédito que se buscó ya fue contabilizado","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
	        				label16.Text="Búsqueda sin Resultados!";
	        				maskedTextBox1.Clear();
	        				maskedTextBox2.Clear();
	        				maskedTextBox2.Focus();
        				}
        			}else{
        					
        					label25.Visible=true;
        					maskedTextBox6.Visible=true;
        					button6.Visible=true;
        					maskedTextBox6.Focus();
        					label16.Text="Crédito no Encontrado!\nIngrese el Periodo y dé Click en Guardar\npara registrarlo como Sobrante";
        					
        			}
        		
        		}else{
        			if(0>1){
        				
        			}else{
        			
        				if(((consulta.Rows.Count-tabla2.Rows.Count)>=1)){
        				MessageBox.Show("Uno o más Créditos del Registro que se buscó ya fueron contabilizados, se omitirán de los resultados","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        			}	
        				
        			panel2.Visible=false;
        			button1.Enabled=true;
        			radioButton1.Checked=false;
        			radioButton2.Checked=false;
        			button1.Visible=true;
        			reg_ant=0;
        			
	        		
        			j=0;
        			
        			/*while(j<dataGridView1.RowCount){
        				if(dataGridView1.Rows[j].Cells[17].FormattedValue.ToString().Length > 1){
        					tabla2.Rows.Add(j);
        				}
        				j++;
        			}
        			
        			j=0;
        			
        			while(j<tabla2.Rows.Count){
        				dataGridView1.Rows[j].Visible=false;
        				j++;
        			}
        			*/
        			
	        		dataGridView1.Columns[0].HeaderText="ID";
	        		dataGridView1.Columns[1].HeaderText="CLASE\nDOCUMENTO";
	        		dataGridView1.Columns[2].HeaderText="REGISTRO\nPATRONAL";
	        		dataGridView1.Columns[3].HeaderText="CREDITO";
	        		dataGridView1.Columns[4].HeaderText="MES PERIODO";
	        		dataGridView1.Columns[5].HeaderText="AÑO PERIODO";
	        		dataGridView1.Columns[6].HeaderText="IMPORTE";
	        		dataGridView1.Columns[7].HeaderText="TIPO\nDOCUMENTO";
	        		dataGridView1.Columns[8].HeaderText="INCIDENCIA";//----
	        		dataGridView1.Columns[9].HeaderText="SECTOR";
	        		dataGridView1.Columns[10].HeaderText="MOVIMIENTO";
	        		dataGridView1.Columns[11].HeaderText="FECHA\nMOVIMIENTO";
	        		dataGridView1.Columns[12].HeaderText="CE";
	        		dataGridView1.Columns[13].HeaderText="FECHA\nALTA";
	        		dataGridView1.Columns[14].HeaderText="FECHA\nNOTIFICACION";
	        		dataGridView1.Columns[15].HeaderText="FECHA\nINCIDENCIA";
	        		dataGridView1.Columns[16].HeaderText="DIAS";
	        		dataGridView1.Columns[17].Visible=false;
	        		
	        		label1.Text=dataGridView1.Rows[0].Cells[2].FormattedValue.ToString();//reg_pat
	        		label3.Text=dataGridView1.Rows[0].Cells[3].FormattedValue.ToString();//credito
	        		label9.Text=dataGridView1.Rows[0].Cells[6].FormattedValue.ToString();//importe
	        		label11.Text=dataGridView1.Rows[0].Cells[7].FormattedValue.ToString();//tipo_doc
	        		label6.Text=dataGridView1.Rows[0].Cells[4].FormattedValue.ToString()+"/"+dataGridView1.Rows[0].Cells[5].FormattedValue.ToString();//periodo
	        		label12.Text=dataGridView1.Rows[0].Cells[8].FormattedValue.ToString();//incidencia
	        		label29.Text=buscar_nom_pat(label1.Text);
	        		
	        		dataGridView1.Focus();
	        		
        			}
        		}
        	}else{
        		MessageBox.Show("No se puede realizar una búsqueda con los datos ingresados","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        	
        		radioButton1.Checked=false;
				radioButton2.Checked=false;
				radioButton3.Checked=false;
				textBox1.Clear();
				textBox2.Clear();
				maskedTextBox1.Clear();
				maskedTextBox2.Clear();
				maskedTextBox3.Clear();
				maskedTextBox4.Clear();
				maskedTextBox5.Clear();
				maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
				textBox2.BackColor=System.Drawing.SystemColors.Window;
				panel3.Visible=false;
				textBox1.Visible=false;
				label5.Visible=false;
				label16.Text="Ingrese Registro Patronal y Credito a buscar...";
				maskedTextBox1.Focus();
        	
        	}
        }
		
		public void colocar_patron(){
			int candado=0;
			
			if(dataGridView1.RowCount>1){
				//MessageBox.Show("actual:"+fila_activa+" anterior: "+reg_ant+" regreso: "+regreso);
				if((dataGridView1.CurrentRow.Index != reg_ant)&& regreso==0){
					DialogResult res= MessageBox.Show("Todavía no Marca el registro actual.\n¿Desea cambiar de registro sin afectar el actual?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
					
					if(res == DialogResult.Yes){
						
						try{
							
							label1.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].FormattedValue.ToString();//reg_pat
							label3.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].FormattedValue.ToString();//credito
							label9.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].FormattedValue.ToString();//importe
							label11.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].FormattedValue.ToString();//tipo_doc
							label6.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].FormattedValue.ToString()+"/"+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[5].FormattedValue.ToString();//periodo
							label12.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].FormattedValue.ToString();//incidencia
							label29.Text=buscar_nom_pat(label1.Text);
							dataGridView1.ClearSelection();
							dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected=true;
							
						}catch(Exception es){
							
						}
						
					}else{
						regreso=1;
        				try{
							
							dataGridView1.CurrentCell=dataGridView1[0,reg_ant];
							label1.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].FormattedValue.ToString();//reg_pat
							label3.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].FormattedValue.ToString();//credito
							label9.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].FormattedValue.ToString();//importe
							label11.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7].FormattedValue.ToString();//tipo_doc
							label6.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].FormattedValue.ToString()+"/"+dataGridView1.Rows[0].Cells[5].FormattedValue.ToString();//periodo
							label12.Text=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[8].FormattedValue.ToString();//incidencia
							label29.Text=buscar_nom_pat(label1.Text);
							dataGridView1.ClearSelection();
							dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected=true;
							//MessageBox.Show("primera vuelta");
						}catch(Exception es){
							
						}
						fila_activa=reg_ant;
					}
				}else{
					regreso=0;
				}
				
			}	
		}
		
		public void guardar(){
			
			String sql,nrp,nrp_limpio="",credito,credito_limpio="",periodo_limpio="",periodo,importe_limpio="",mensaje_info="",observaciones,fecha,td_limpio;
			int i=0, validar=0,blanco=0,verde=0;
			decimal importe=0;
			
			fecha = (System.DateTime.Today.Year.ToString()+
			         "-"+System.DateTime.Today.Month.ToString()+
			         "-"+System.DateTime.Today.Day.ToString()+
			         " "+System.DateTime.Now.Hour.ToString()+
			         ":"+System.DateTime.Now.Minute.ToString()+
			         ":"+System.DateTime.Now.Second.ToString());
			
			/*-----Coincidencia Total-------*/
			if(radioButton1.Checked==true){
				DialogResult res= MessageBox.Show("Se Marcará Como COINCIDENCIA TOTAL (1) el siguiente registro:\n Registro Patronal: "+label1.Text+" con el Crédito: "+label3.Text+".\n¿Desea continuar y marcarlo?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				if(res== DialogResult.Yes){
					sql="UPDATE base_inventario SET coincidente=\"TOTAL\", responsable=\""+responsable+"\", auxiliar=\""+auxiliar+"\",fecha_verificacion=\""+fecha+"\" WHERE idbase_inventario="+dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].FormattedValue.ToString()+"";
					conex.consultar(sql);
					
					actualizar_al_terminar();
				}
			}
			
			/*-----Coincidencia Parcial------*/
			if(radioButton2.Checked==true){
				
				if(maskedTextBox3.BackColor.Name.Equals("LightGreen")||maskedTextBox3.BackColor.Name.Equals("Window")){
					validar++;
					if(maskedTextBox3.BackColor.Name.Equals("Window")){
						blanco++;
					}
				}
				
				if(maskedTextBox4.BackColor.Name.Equals("LightGreen")||maskedTextBox4.BackColor.Name.Equals("Window")){
					validar++;
					if(maskedTextBox4.BackColor.Name.Equals("Window")){
						blanco++;
					}
				}
				
				if(maskedTextBox5.BackColor.Name.Equals("LightGreen")||maskedTextBox5.BackColor.Name.Equals("Window")){
					validar++;
					if(maskedTextBox5.BackColor.Name.Equals("Window")){
						blanco++;
					}
				}
				
				if(textBox2.BackColor.Name.Equals("LightGreen")||textBox2.BackColor.Name.Equals("Window")){
					validar++;
					if(textBox2.BackColor.Name.Equals("Window")){
						blanco++;
					}
				}
				
				if(textBox4.BackColor.Name.Equals("LightGreen")||textBox4.BackColor.Name.Equals("Window")){
					validar++;
					if(textBox4.BackColor.Name.Equals("Window")){
						blanco++;
					}
				}
				
				verde=validar-blanco;
				if((verde+blanco)==5){
					nrp=maskedTextBox3.Text;
					credito=maskedTextBox4.Text;
					
					//se quitan guiones y espacios en blanco
					while(i<nrp.Length){
						if((nrp.Substring(i,1)).Equals(" ")){
						}else{
							nrp_limpio += nrp.Substring(i,1);
						}
						i++;
					}
					
					if(nrp_limpio.Length < 12){
						nrp_limpio="-";
					}
					
					i=0;
					
					while(i<credito.Length){
						if((credito.Substring(i,1)).Equals(" ")){
						}else{
							credito_limpio+= credito.Substring(i,1);
						}
						i++;
					}
					
					if(credito_limpio.Length < 9){
						credito_limpio="-";
					}
					try{
						if(maskedTextBox5.Text.Length>=7){
							periodo=maskedTextBox5.Text;
							
							if((Convert.ToInt32(periodo.Substring(0,2))) > 0 && (Convert.ToInt32(periodo.Substring(0,2)))< 13){
								
								if((Convert.ToInt32(periodo.Substring(3,4)))>1996 && (Convert.ToInt32(periodo.Substring(3,4))) <= System.DateTime.Today.Year){
									periodo_limpio=periodo;
								}else{
									periodo_limpio="-";
								}
							}else{
								periodo_limpio="-";
							}
						}else{
							periodo_limpio="-";
						}
					}catch(Exception es){
						periodo_limpio="-";
						maskedTextBox5.BackColor=System.Drawing.Color.Tomato;
					}
					
					if(decimal.TryParse(textBox2.Text,out importe)){
						importe=Convert.ToDecimal(textBox2.Text);
					}else{
						importe=0;
					}
					
					importe_limpio=Convert.ToString(importe);
					if(importe==0){
						importe_limpio="-";
					}
					
					
					if(textBox4.Text.Length==0){
						td_limpio="-";
					}else{
						td_limpio=textBox4.Text;
					}
					
					//MessageBox.Show(nrp_limpio.Length+"|"+credito_limpio.Length+"|"+periodo_limpio.Length+"|"+importe_limpio.Length);
					if(nrp_limpio.Length==1 && credito_limpio.Length==1 && periodo_limpio.Length==1 && importe_limpio.Length==1 && td_limpio.Length==1){
						MessageBox.Show("Es necesario que especifique correctamente el/los datos que difiere(n) entre la base de datos y el crédito físico.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					}else{
						
						if(nrp_limpio.Length > 2){
							mensaje_info="Registro Patronal: "+nrp_limpio+".\n";
						}
						
						if(credito_limpio.Length > 1){
							mensaje_info+="Crédito: "+credito_limpio+".\n";
						}
						
						if(periodo_limpio.Length > 1){
							mensaje_info+="Periodo: "+periodo_limpio+".\n";
						}
						
						if(importe_limpio.Length > 1){
							mensaje_info+="Importe: "+importe_limpio+".\n";
						}
						
						if(td_limpio.Length > 1){
							mensaje_info+="Tipo Documento: "+td_limpio+".\n";
						}
						
						DialogResult res1= MessageBox.Show("Se Marcará Como COINCIDENCIA PARCIAL (2) el siguiente registro:\n Registro Patronal: "+label1.Text+" con el Crédito: "+label3.Text+".\n\nTambién se Ingresará la siguiente información a la Base de Datos:\n"+
						                                   "\n"+mensaje_info+"\n¿Desea continuar y marcarlo?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
						if(res1== DialogResult.Yes){
							sql="UPDATE base_inventario SET coincidente=\"PARCIAL\", responsable=\""+responsable+"\", auxiliar=\""+auxiliar+"\",fecha_verificacion=\""+fecha+"\",doc_reg_pat=\""+nrp_limpio+"\", "+
								"doc_credito=\""+credito_limpio+"\", doc_periodo=\""+periodo_limpio+"\", doc_importe=\""+importe_limpio+"\", doc_td=\""+td_limpio+"\" WHERE idbase_inventario="+dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].FormattedValue.ToString()+"";
							conex.consultar(sql);
							
							actualizar_al_terminar();	
						}
					}
					
				}else{
					MessageBox.Show("Debe ingresar correctamente los datos del crédito físico que difieren de los de la base de datos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}
			
			/*-----Coincidencia OTRO-------*/
			if(radioButton3.Checked==true){
				
				if(textBox1.Text.Length>24){
					observaciones=textBox1.Text;
					
					DialogResult res1= MessageBox.Show("Se Marcará Como OTRO (3) el siguiente registro:\n Registro Patronal: "+label1.Text+" con el Crédito: "+label3.Text+".\n¿Desea continuar y marcarlo?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
					if(res1== DialogResult.Yes){
						sql="UPDATE base_inventario SET coincidente=\"OTRO\", responsable=\""+responsable+"\", auxiliar=\""+auxiliar+"\",fecha_verificacion=\""+fecha+"\",observaciones=\""+observaciones+"\" WHERE idbase_inventario="+dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].FormattedValue.ToString()+"";
						conex.consultar(sql);
						
						actualizar_al_terminar();
					}
					
				}else{
					MessageBox.Show("Debe de escribir mas detalladamente la observacion.\nSe requieren mínimo 25 caracteres.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}
			
			
			
		}
		
		public void actualizar_al_terminar(){
			
			int i=0,id=0;
			
			if(dataGridView1.RowCount>1){
				
				id=Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].FormattedValue.ToString());
				
				consulta=conex.consultar(sql_persist);
				//MessageBox.Show(""+consulta.Rows.Count);
				
				/*while(i<consulta.Rows.Count){
					if((Convert.ToInt32(consulta.Rows[i][17].ToString()))==id){
						consulta.Rows[i][17]="MODIFICADO";
					}
				}*/
				DataView vista1 = new DataView(consulta);
				
				vista1.RowFilter="coincidente ='-'";
				vista1.Sort="credito ASC";
				tabla2=vista1.ToTable();
				dataGridView1.DataSource=tabla2;
				
				label1.Text=dataGridView1.Rows[0].Cells[2].FormattedValue.ToString();//reg_pat
				label3.Text=dataGridView1.Rows[0].Cells[3].FormattedValue.ToString();//credito
				label9.Text=dataGridView1.Rows[0].Cells[6].FormattedValue.ToString();//importe
				label11.Text=dataGridView1.Rows[0].Cells[7].FormattedValue.ToString();//tipo_doc
				label6.Text=dataGridView1.Rows[0].Cells[4].FormattedValue.ToString()+"/"+dataGridView1.Rows[0].Cells[5].FormattedValue.ToString();//periodo
				label12.Text=dataGridView1.Rows[0].Cells[8].FormattedValue.ToString();//incidencia
				label29.Text=buscar_nom_pat(label1.Text);
				label22.Text="Créditos Encontrados: "+dataGridView1.RowCount;
				
				radioButton1.Checked=false;
				radioButton2.Checked=false;
				radioButton3.Checked=false;
				textBox1.Clear();
				textBox2.Clear();
				//maskedTextBox1.Clear();
				maskedTextBox2.Clear();
				maskedTextBox3.Clear();
				maskedTextBox4.Clear();
				maskedTextBox5.Clear();
				maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
				textBox2.BackColor=System.Drawing.SystemColors.Window;
				panel3.Visible=false;
				textBox1.Visible=false;
				label5.Visible=false;
				label16.Text="Ingrese Registro Patronal y Credito a buscar...";
				maskedTextBox2.Focus();
			}else{
				radioButton1.Checked=false;
				radioButton2.Checked=false;
				radioButton3.Checked=false;
				textBox1.Clear();
				textBox2.Clear();
				//maskedTextBox1.Clear();
				maskedTextBox2.Clear();
				maskedTextBox3.Clear();
				maskedTextBox4.Clear();
				maskedTextBox5.Clear();
				maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
				maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
				textBox2.BackColor=System.Drawing.SystemColors.Window;
				panel3.Visible=false;
				textBox1.Visible=false;
				label5.Visible=false;
				panel2.Visible=true;
				label16.Text="Ingrese Registro Patronal y Credito a buscar...";
				maskedTextBox2.Focus();
			}
			
			maskedTextBox6.Clear();
			label25.Visible=false;
			maskedTextBox6.Visible=false;
			button6.Visible=false;
			maskedTextBox6.BackColor=System.Drawing.SystemColors.Window;
			
			MessageBox.Show("Registro Guardado correctamente.","EXITO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			actualizar_progreso();
		}
		
		public void actualizar_progreso(){
			
			String inicio,final,total,avance,ultimo_reg,ultimo_cred;
			
			try{
        	consultamysql=conex.consultar("SELECT libro,rango_inicial,rango_final FROM responsables WHERE id_usuario="+MainForm.datos_user_static[7]);
        	libro=consultamysql.Rows[0][0].ToString();
        	inicio=consultamysql.Rows[0][1].ToString();
        	final=consultamysql.Rows[0][2].ToString();
        		
        	consultamysql=conex.consultar("SELECT count(idbase_inventario) FROM base_inventario WHERE idbase_inventario >= "+inicio+" AND idbase_inventario <= "+final+" ");
        	total=consultamysql.Rows[0][0].ToString();
        	
        	consultamysql=conex.consultar("SELECT count(idbase_inventario) FROM base_inventario WHERE no_cuaderno = "+libro+" AND coincidente <> '-' ");
        	avance=consultamysql.Rows[0][0].ToString();
        	
        	consultamysql=conex.consultar("SELECT idbase_inventario, reg_pat,credito FROM inventario.base_inventario WHERE coincidente <> \"-\" and no_cuaderno="+libro+" order by idbase_inventario desc limit 1");
        	ultimo_reg=consultamysql.Rows[0][1].ToString();
        	ultimo_cred=consultamysql.Rows[0][2].ToString();
        	
        	label23.Text="Libro: "+libro+"     Avance: "+avance+"/"+total+"";
        	label24.Text="Último: "+ultimo_reg+" | "+ultimo_cred;
        	
        	toolTip1.SetToolTip(label23, "Restan "+(Convert.ToInt32(total)-Convert.ToInt32(avance))+" Créditos por Trabajar.");
        	
        	}catch(Exception es){
        		//this.Close();
        	}
		}
		
		public String buscar_nom_pat(String nrp){
			String nom_p="";
			conex4.conectar("base_principal");
			nrp=nrp.Substring(0,3)+nrp.Substring(4,5)+nrp.Substring(10,2);
			noms_pat=conex4.consultar("SELECT nombre FROM sindo WHERE registro_patronal like\""+nrp+"%\"");
			if(noms_pat.Rows.Count>0){
				nom_p=noms_pat.Rows[0][0].ToString();
			}else{
				nom_p="-";
			}
			
			return nom_p;
		}
		
		void Consulta_verificacionLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            int j =0;
			String user,inicio,final,total="",avance="",ultimo_reg,ultimo_cred,id_user_log;
			
        	conex.conectar("inventario");
        	
        	consultamysql=conex.consultar("SELECT nombre_auxiliar FROM auxiliares");
        	while(j<consultamysql.Rows.Count){
        		comboBox1.Items.Add(consultamysql.Rows[j][0].ToString());
        		j++;
        	}
        	
        	id_user_log=MainForm.datos_user_static[7];
        	
        	conex3.conectar("inventario");
        	user_verif=conex3.consultar("SELECT idresponsables FROM responsables WHERE id_usuario="+id_user_log+"");
        	if(user_verif.Rows.Count==0){
        		textBox3.Visible=true;
        		label26.Visible=true;
        	}else{
        	
	        	try{
	        	consultamysql=conex.consultar("SELECT libro,rango_inicial,rango_final FROM responsables WHERE id_usuario="+MainForm.datos_user_static[7]);
	            
	            libro=consultamysql.Rows[0][0].ToString();
	        	inicio=consultamysql.Rows[0][1].ToString();
	        	final=consultamysql.Rows[0][2].ToString();
	           
	                    		
	        	consultamysql=conex.consultar("SELECT count(idbase_inventario) FROM base_inventario WHERE idbase_inventario >= "+inicio+" AND idbase_inventario <= "+final+" ");
	        	total=consultamysql.Rows[0][0].ToString();
	            //MessageBox.Show("total: " + total);
	
	        	consultamysql=conex.consultar("SELECT count(idbase_inventario) FROM base_inventario WHERE no_cuaderno = "+libro+" AND coincidente <> '-' ");
	        	avance=consultamysql.Rows[0][0].ToString();
	            label23.Text = "Libro: " + libro + "     Avance: " + avance + "/" + total + "";
	            //MessageBox.Show("avance: " + avance); 
	            }catch(Exception es){
	        		//this.Close();
	                label23.Text = "Libro: 00     Avance: 0000/0000";
	        	}
	
	            try{
	        	consultamysql=conex.consultar("SELECT idbase_inventario, reg_pat,credito FROM inventario.base_inventario WHERE coincidente <> \"-\" and no_cuaderno="+libro+" order by idbase_inventario desc limit 1");
	        	ultimo_reg=consultamysql.Rows[0][1].ToString();
	        	ultimo_cred=consultamysql.Rows[0][2].ToString();
	           // MessageBox.Show("ultimo_reg: " + ultimo_reg + " ultimo_cred: " + ultimo_cred); 
	
	        	//label23.Text="Libro: "+libro+"     Avance: "+avance+"/"+total+"";
	        	label24.Text="Último: "+ultimo_reg+" | "+ultimo_cred;
	        	
	        	toolTip1.SetToolTip(label23, "Restan "+(Convert.ToInt32(total)-Convert.ToInt32(avance))+" Créditos por Trabajar.");
	        	
	        	}catch(Exception es){
	                //label23.Text = "Libro: 00     Avance: 0000/0000";
	                label24.Text = "Último: X00-00000-00|000000000";
	        	}
        	
			}
        	
        	user = MainForm.datos_user_static[9]+" "+MainForm.datos_user_static[4];
        	responsable=user;
        	label13.Text="Responsable: "+user;
        	
        	tabla2.Columns.Add("indice");
        	
        	maskedTextBox1.Enabled=false;
        	maskedTextBox2.Enabled=false;
        	button1.Enabled=false;
        	button2.Enabled=false;
        	button3.Enabled=false;
        	//button4.Enabled=false;
        	radioButton1.Enabled=false;
        	radioButton2.Enabled=false;
        	label5.Visible=false;
        	textBox1.Visible=false;
        	button1.Enabled=false;
        	
        	
        	
        }
		
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        
        void MaskedTextBox1MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        	
        }
        
        void MaskedTextBox1TextChanged(object sender, EventArgs e)
        {
        	 if (maskedTextBox1.Text.Length==16)
            {
                maskedTextBox2.Focus();
            }
        }
        
        void Button2Click(object sender, EventArgs e)
        {
        	maskedTextBox1.Clear();
        	maskedTextBox2.Clear();
        	maskedTextBox1.Focus();
        }
        
        void Button3Click(object sender, EventArgs e)
        {
        	buscar();
        }
        
        void MaskedTextBox2TextChanged(object sender, EventArgs e)
        {
        	if(maskedTextBox2.Text.Length==9){
        		button3.Focus();
        	}
        }
        
        void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
        {
        	if (e.KeyChar == (char)(Keys.Enter))
            {
        		buscar();
        	}
        }
        
        void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
        {
        	if (e.KeyChar == (char)(Keys.Enter))
            {
        		buscar();
        	}
        }
        
        void Button5Click(object sender, EventArgs e)
        {
        	int can=0,cond=0;
        	string mensaje="";
        	
        	if(textBox3.Text.Length>14){
        		can++;
        	}else{
        		mensaje+="•Escriba Correctamente su Nombre (Minímo 15 carateres)\n";
        	}
        	
        	if((comboBox1.SelectedIndex >-1)){
        		can++;
        	}else{
        		mensaje+="•Elija un auxiliar\n";
        	}
        	
        	if(textBox3.Visible==true){
        		cond=2;
        	}else{
        		cond=1;
        	}
        	
        	if(can==cond){
        		
        		if(textBox3.Visible==true){
        			responsable=textBox3.Text;
        			label13.Text="Responsable: "+textBox3.Text;
        		}
        		
        		panel1.Visible=false;
        		maskedTextBox1.Enabled=true;
        		maskedTextBox2.Enabled=true;
        		button1.Enabled=true;
        		button2.Enabled=true;
        		button3.Enabled=true;
        		//button4.Enabled=false;
        		radioButton1.Enabled=true;
        		radioButton2.Enabled=true;
        		panel2.Visible=true;
        		label16.Text="Ingrese Registro Patronal y Credito a buscar...";
        		label16.Visible=true;
        		label14.Text="Auxiliar: "+comboBox1.SelectedItem.ToString();
        		auxiliar=comboBox1.SelectedItem.ToString();
        		maskedTextBox1.Focus();
        	}else{
        		MessageBox.Show("Para continuar: \n"+mensaje,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        	}
        }
        
        void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        	
        }
        
        void DataGridView1Enter(object sender, EventArgs e)
        {
        	
        }
        
        void DataGridView1CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        	fila_activa=e.RowIndex;
        	colocar_patron();
        }
        
        void DataGridView1CellClick(object sender, DataGridViewCellEventArgs e)
        {
        	
        }
        
        void RadioButton1CheckedChanged(object sender, EventArgs e)
        {
        	if(radioButton1.Checked==true){
        		panel3.Visible=false;
	        	label5.Visible=false;
	        	textBox1.Visible=false;
                button1.Focus();
        	}
        }
        
        void RadioButton2CheckedChanged(object sender, EventArgs e)
        {
        	if (radioButton2.Checked==true){
        		panel3.Visible=true;
        	}
        }
        
        void RadioButton3CheckedChanged(object sender, EventArgs e)
        {
        	if(radioButton3.Checked==true){
        		panel3.Visible=false;
	        	label5.Visible=true;
	        	textBox1.Visible=true;
        	}
        }
        
        void MaskedTextBox1Leave(object sender, EventArgs e)
        {
        	maskedTextBox1.Text=maskedTextBox1.Text.ToUpper();
        }
        
        void MaskedTextBox5MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        	
        }
        
        void Button1Click(object sender, EventArgs e)
        {
        	guardar();
        }
        
        void TextBox2TextChanged(object sender, EventArgs e)
        {
        	decimal importe;
        	if(textBox2.Text.Length>0){
        	 if(decimal.TryParse(textBox2.Text,out importe)){
			    	importe=Convert.ToDecimal(textBox2.Text);
			    	textBox2.BackColor=System.Drawing.Color.LightGreen;
        	}else{
			    	importe=0;
			    	textBox2.BackColor=System.Drawing.Color.Tomato;
			    }
        		
        		}else{
        		textBox2.BackColor=System.Drawing.SystemColors.Window;
        	}
        }
        
        void MaskedTextBox3Leave(object sender, EventArgs e)
        {
        	String nrp,nrp_limpio,nrp_clear="";
        	int i=0;
        	maskedTextBox3.Text=maskedTextBox3.Text.ToUpper();
        	nrp=maskedTextBox3.Text;
        	nrp_limpio="";
        	
        	//se quitan guiones y espacios en blanco
        	while(i<nrp.Length){
        		if((nrp.Substring(i,1)).Equals(" ")||(nrp.Substring(i,1)).Equals("-")){
        		}else{
        			nrp_clear += nrp.Substring(i,1);
        		}
        		i++;
        	}
        	i=0;
        	
        	if(nrp_clear.Length>0){
        	//se quitan guiones y espacios en blanco
        	while(i<nrp.Length){
        		if((nrp.Substring(i,1)).Equals(" ")){
        		}else{
        			nrp_limpio += nrp.Substring(i,1);
        		}
        		i++;
        	}
        	//MessageBox.Show("nrp: "+nrp_limpio+" |"+nrp_limpio.Length);
        	if(nrp_limpio.Length==12){
        		maskedTextBox3.BackColor=System.Drawing.Color.LightGreen;
        	}else{
        		maskedTextBox3.BackColor=System.Drawing.Color.Tomato;
        	}
        	
        	}else{
        		maskedTextBox3.BackColor=System.Drawing.SystemColors.Window;
        	}
        }
       
        void MaskedTextBox4Leave(object sender, EventArgs e)
        {
        	String credito,credito_limpio;
        	int i=0;
        	
        	credito=maskedTextBox4.Text;
        	credito_limpio="";
        	
        	if(maskedTextBox4.Text.Length>0){
        		while(i<credito.Length){
        			if((credito.Substring(i,1)).Equals(" ")){
        			}else{
        				credito_limpio+= credito.Substring(i,1);
        			}
        			i++;
        		}
        		
        		if(credito_limpio.Length==9){
        			maskedTextBox4.BackColor=System.Drawing.Color.LightGreen;
        		}else{
        			maskedTextBox4.BackColor=System.Drawing.Color.Tomato;
        		}
        		
        	}else{
        			maskedTextBox4.BackColor=System.Drawing.SystemColors.Window;
        	}
        }
        
        void MaskedTextBox5Leave(object sender, EventArgs e)
        {
        	String periodo,periodo_limpio="";
        	int i=0;
        	
        	periodo=maskedTextBox5.Text;
        	
        	while(i<periodo.Length){
        		if((periodo.Substring(i,1)).Equals(" ")||(periodo.Substring(i,1)).Equals("/")){
        		}else{
        			periodo_limpio+= periodo.Substring(i,1);
        		}
        		i++;
        	}
        	
        	if(periodo_limpio.Length>0){
			    	periodo=maskedTextBox5.Text;
			    	try{
			    	if((Convert.ToInt32(periodo.Substring(0,2))) > 0 && (Convert.ToInt32(periodo.Substring(0,2)))< 13){
			    		
			    		if((Convert.ToInt32(periodo.Substring(3,4)))>1996 && (Convert.ToInt32(periodo.Substring(3,4))) <= System.DateTime.Today.Year){
			    	   		periodo_limpio=periodo;
			    	   		maskedTextBox5.BackColor=System.Drawing.Color.LightGreen;
			    	   	   }else{
			    	   			periodo_limpio="-";
			    	   			maskedTextBox5.BackColor=System.Drawing.Color.Tomato;
			    	   		}
			    	}else{
			    	   		periodo_limpio="-";
			    	   		maskedTextBox5.BackColor=System.Drawing.Color.Tomato;
			    	} 
			    	}catch(Exception es){
			    		periodo_limpio="-";
			    	   	maskedTextBox5.BackColor=System.Drawing.Color.Tomato;
			    	}
        	}else{
        		maskedTextBox5.BackColor=System.Drawing.SystemColors.Window;
        	}
        }
        
        void TextBox2Leave(object sender, EventArgs e)
        {
        	if(textBox2.Text.Length==0){
        		textBox2.BackColor=System.Drawing.SystemColors.Window;
        	}
        }
        
        void Timer1Tick(object sender, EventArgs e)
        {
        	int activa=0,k=0;
        	
        	
        	if(dataGridView1.RowCount>0){
        		activa=dataGridView1.CurrentRow.Index;
        		
        		while(k<dataGridView1.RowCount){
        			if((dataGridView1.Rows[k].Cells[0].Selected==true)&& k != activa){
        				dataGridView1.CurrentCell=dataGridView1[0,k];
        			}else{
        				
        			}
        			k++;
        		}
        	}
        	
        }
        
        void DataGridView1CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        	reg_ant=e.RowIndex;
        	
        	
        }
        
        void DataGridView1CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        	
        }
        
        void Label16Click(object sender, EventArgs e)
        {
        	
        }
        
        void DataGridView1CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
        	
        }
        
        void DataGridView1CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
        	
        }
        
        void DataGridView1CellStyleChanged(object sender, DataGridViewCellEventArgs e)
        {
        	MessageBox.Show(e.RowIndex+"| "+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Style.BackColor.Name);
        }
        
        void Button6Click(object sender, EventArgs e)
        {
        	String sql,reg_sob="",cred_sob="",fech_sob="";
        	int i=0;
        	conex2.conectar("inventario");
        	
        	//se quitan guiones y espacios en blanco
        	while(i<maskedTextBox1.Text.Length){
        		if((maskedTextBox1.Text.Substring(i,1)).Equals(" ")){
        		}else{
        			reg_sob += maskedTextBox1.Text.Substring(i,1);
        		}
        		i++;
        	}
        	
        	i=0;
        	
        	while(i<maskedTextBox2.Text.Length){
        		if((maskedTextBox2.Text.Substring(i,1)).Equals(" ")){
        		}else{
        			cred_sob+= maskedTextBox2.Text.Substring(i,1);
        		}
        		i++;
        	}
        	
        	if(reg_sob.Length==12 && cred_sob.Length==9){
        		if(maskedTextBox6.BackColor.Name.Equals("LightGreen")){

                    sql = "SELECT * FROM sobrante WHERE reg_pat=\"" + reg_sob + "\" AND credito=\"" + cred_sob + "\" and periodo=\"" + maskedTextBox6.Text + "\"";
                    dataGridView2.DataSource=conex2.consultar(sql);
                   
                    if (dataGridView2.Rows.Count == 0)
                    {
                        fech_sob = DateTime.Today.ToShortDateString();
                        fech_sob = fech_sob.Substring(6, 4) + "-" + fech_sob.Substring(3, 2) + "-" + fech_sob.Substring(0, 2);
                    	sql = "INSERT INTO sobrante (reg_pat,credito,periodo,libro,responsable,fecha_sobrante) VALUES (\"" + reg_sob + "\",\"" + cred_sob + "\",\"" + maskedTextBox6.Text + "\",\""+libro+"\",\""+responsable+"\",\""+fech_sob+"\")";
                        conex2.consultar(sql);
                        label16.Text = "Ingrese Registro Patronal y Credito a buscar...";

                        //maskedTextBox1.Clear();
                        maskedTextBox2.Clear();
                        maskedTextBox6.Clear();
                        label25.Visible = false;
                        maskedTextBox6.Visible = false;
                        button6.Visible = false;
                        maskedTextBox6.BackColor = System.Drawing.SystemColors.Window;
                        MessageBox.Show("Registro Anotado como SOBRANTE correctamente.", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        label16.Text = "Ingrese Registro Patronal y Credito a buscar...";
                        maskedTextBox2.Focus();
                    }
                    else
                    {
                        MessageBox.Show("No se puede guardar este crédito por que ya fue registrado como SOBRANTE con anterioridad", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        maskedTextBox1.Clear();
                        maskedTextBox2.Clear();
                        maskedTextBox2.Focus();
                    }
        		}else{
        			MessageBox.Show("Se debe ingresar correctamente el Periodo para poder guardar como SOBRANTE","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        			maskedTextBox6.Focus();
        		}
        	}else{
        		MessageBox.Show("Se debe ingresar correctamente el Registro y el Crédito para poder anotarlos como SOBRANTES","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		maskedTextBox1.Focus();
        	}
        }
        
        void MaskedTextBox6Leave(object sender, EventArgs e)
        {
        	String periodo,periodo_limpio="";
        	int i=0;
        	
        	periodo=maskedTextBox6.Text;
        	
        	while(i<periodo.Length){
        		if((periodo.Substring(i,1)).Equals(" ")||(periodo.Substring(i,1)).Equals("/")){
        		}else{
        			periodo_limpio+= periodo.Substring(i,1);
        		}
        		i++;
        	}
        	
        	if(periodo_limpio.Length>0){
			    	periodo=maskedTextBox6.Text;
			    	try{
			    	if((Convert.ToInt32(periodo.Substring(0,2))) > 0 && (Convert.ToInt32(periodo.Substring(0,2)))< 13){
			    		
			    		if((Convert.ToInt32(periodo.Substring(3,4)))>1996 && (Convert.ToInt32(periodo.Substring(3,4))) <= System.DateTime.Today.Year){
			    	   		periodo_limpio=periodo;
			    	   		maskedTextBox6.BackColor=System.Drawing.Color.LightGreen;
			    	   	   }else{
			    	   			periodo_limpio="-";
			    	   			maskedTextBox6.BackColor=System.Drawing.Color.Tomato;
			    	   		}
			    	}else{
			    	   		periodo_limpio="-";
			    	   		maskedTextBox6.BackColor=System.Drawing.Color.Tomato;
			    	} 
			    	}catch(Exception es){
			    		periodo_limpio="-";
			    	   	maskedTextBox6.BackColor=System.Drawing.Color.Tomato;
			    	}
        	}else{
        		maskedTextBox6.BackColor=System.Drawing.SystemColors.Window;
        	}
        }

        private void maskedTextBox6_TextChanged(object sender, EventArgs e)
        {
            if(maskedTextBox6.Text.Length==7){
                button6.Focus();
            }
        }
        
        void Button4Click(object sender, EventArgs e)
        {
			FileStream fichero;
			String ruta="";
			if(File.Exists(@"temp/cookie.lz")==true){
				System.IO.File.Delete(@"temp/cookie.lz");
			}
			fichero=System.IO.File.Create(@"temp/cookie.lz");
			ruta=fichero.Name;
			ruta=ruta.Substring(0,ruta.Length-9);
			ruta=ruta+"Manual_Nova.pdf#page=95";
			Navegador_web nav = new Navegador_web("file:///"+ruta);
			nav.Show();		        	
        }
        
        void Label12Click(object sender, EventArgs e)
        {
        	
        }
        
        void Label10Click(object sender, EventArgs e)
        {
        	
        }
        
        void TextBox4TextChanged(object sender, EventArgs e)
        {
        	int td=0; 
        	if(textBox4.Text.Length>0){
        		if(int.TryParse(textBox4.Text,out td)){
			    	td=Convert.ToInt32(textBox4.Text);
			    	textBox4.BackColor=System.Drawing.Color.LightGreen;
        		}else{
			    	td=0;
			    	textBox4.BackColor=System.Drawing.Color.Tomato;
			    }
        		
        		}else{
        		textBox4.BackColor=System.Drawing.SystemColors.Window;
        	}
        }
        
        void Label19Click(object sender, EventArgs e)
        {
        	
        }
        
        void Label6Click(object sender, System.EventArgs e)
        {
        	
        }

        private void Consulta_verificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void n1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            button1.Focus();
        }

        private void n2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }

        private void n3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
            textBox1.Focus();
        }
    }
}

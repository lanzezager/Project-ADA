/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 16/01/2017
 * Hora: 03:38 p.m.
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
	/// Description of Oficios_proceso.
	/// </summary>
	public partial class Oficios_proceso : Form
	{
		public Oficios_proceso()
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
        Conexion conex4 = new Conexion();
        Conexion conex5 = new Conexion();
        Conexion conex6 = new Conexion();
        
        DataTable periodos = new DataTable();
        DataTable res_previo = new DataTable();
        DataTable imprimir = new DataTable();
        DataTable imprimir_previo = new DataTable();
        DataTable sectores = new DataTable();
        DataTable imprimir_nots = new DataTable();

        int i=0,b=0,b1=0,b2=0;
        String carpeta_sel,tipo,cad_buscar="",cad_buscar1="",cad_buscar2="",cad_sectores="",cad_sectores_temp="";
	
		public void llenar_cb1(){
        	conex.conectar("base_principal");
        	comboBox1.Items.Clear();
        	i=0;
        	periodos= conex.consultar("SELECT DISTINCT periodo_oficio FROM oficios ORDER BY periodo_oficio ASC");
        	while(i< periodos.Rows.Count){
        		comboBox1.Items.Add(periodos.Rows[i][0].ToString());
        		i++;
        	}
        	comboBox1.SelectedIndex=(comboBox1.Items.Count-1);
        	conex.cerrar();   
        }
        
        public void llenar_dgv2(){
        	i=0;
        	
        	conex3.conectar("base_principal");
        	if(checkBox1.Checked==false){
        		dataGridView2.DataSource=conex3.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,sector,receptor,fecha_recep_contro,id_oficios FROM base_principal.oficios WHERE periodo_oficio=\""+comboBox1.SelectedItem.ToString()+"\" AND estatus=\"EN TRAMITE\" AND fecha_notificacion IS NULL AND nn <>\"NN\" "+cad_sectores+" ORDER BY razon_social");
        	}else{
        		dataGridView2.DataSource=conex3.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,sector,receptor,fecha_recep_contro,id_oficios FROM base_principal.oficios WHERE estatus=\"EN TRAMITE\" AND fecha_notificacion IS NULL AND nn <>\"NN\" "+cad_sectores+" ORDER BY razon_social");
        	}
        	//MessageBox.Show(cad_sectores);
        	conex3.cerrar();
        	
        	if(dataGridView2.RowCount > 0){
        		//columna 0 fecha
        		//columna 1 checkbox
        		dataGridView2.Columns[2].HeaderText="REG. PAT./N.S.S.";
        		dataGridView2.Columns[3].HeaderText="RAZON SOCIAL";
        		dataGridView2.Columns[4].HeaderText="FOLIO";
        		dataGridView2.Columns[5].HeaderText="ACUERDO";
        		dataGridView2.Columns[6].HeaderText="EMISOR";
        		dataGridView2.Columns[7].HeaderText="SECTOR";
        		dataGridView2.Columns[8].HeaderText="RECEPTOR";
        		dataGridView2.Columns[9].HeaderText="FECHA DE RECEPCION";
        		dataGridView2.Columns[10].Visible=false;
        		
        		dataGridView2.Columns[3].MinimumWidth=300;
        		
        		dataGridView2.Columns[2].ReadOnly=true;
        		dataGridView2.Columns[3].ReadOnly=true;
        		dataGridView2.Columns[4].ReadOnly=true;
        		dataGridView2.Columns[5].ReadOnly=true;
        		dataGridView2.Columns[6].ReadOnly=true;
        		dataGridView2.Columns[7].ReadOnly=true;
        		dataGridView2.Columns[8].ReadOnly=true;
        		dataGridView2.Columns[9].ReadOnly=true;
        		
        		dataGridView2.Columns[0].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[1].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[2].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[3].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[4].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[5].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[6].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[7].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[8].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView2.Columns[9].SortMode=DataGridViewColumnSortMode.NotSortable;
        	}
        	
        	while(i < dataGridView2.RowCount){
        		dataGridView2.Rows[i].Cells[0].Value="";
        		//dataGridView2.Rows[i].Cells[1].Value=false;
        		i++;
        	}
            label3.Text = "Registros: " + dataGridView2.RowCount;
            label3.Refresh();
        }
        
        public void llenar_dgv3(){
        	i=0;
        	
        	conex4.conectar("base_principal");
        	dataGridView3.DataSource=conex4.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,sector,receptor,fecha_visita,id_oficios,fecha_notificacion,nn,fecha_recep_contro FROM base_principal.oficios WHERE (fecha_notificacion IS NOT NULL OR fecha_visita IS NOT NULL OR  nn=\"NN\") AND estatus <> \"DEVUELTO\" ORDER BY razon_social");
        	conex4.cerrar();
        	
        	if(dataGridView3.RowCount > 0){
        		//columna 0 fecha
        		dataGridView3.Columns[1].HeaderText="REG. PAT./N.S.S.";
        		dataGridView3.Columns[2].HeaderText="RAZON SOCIAL";
        		dataGridView3.Columns[3].HeaderText="FOLIO";
        		dataGridView3.Columns[4].HeaderText="ACUERDO";
        		dataGridView3.Columns[5].HeaderText="EMISOR";
        		dataGridView3.Columns[6].HeaderText="SECTOR";
        		dataGridView3.Columns[7].HeaderText="RECEPTOR";
        		dataGridView3.Columns[8].HeaderText="FECHA DE VISITA";
        		dataGridView3.Columns[9].Visible=false;
        		dataGridView3.Columns[10].HeaderText="FECHA DE NOTIFICACION";
        		dataGridView3.Columns[11].HeaderText="NN";
        		dataGridView3.Columns[12].HeaderText="FECHA DE RECEPCION";
        		
        		dataGridView3.Columns[2].MinimumWidth=300;
        		
        		dataGridView3.Columns[1].ReadOnly=true;
        		dataGridView3.Columns[2].ReadOnly=true;
        		dataGridView3.Columns[3].ReadOnly=true;
        		dataGridView3.Columns[4].ReadOnly=true;
        		dataGridView3.Columns[5].ReadOnly=true;
        		dataGridView3.Columns[6].ReadOnly=true;
        		dataGridView3.Columns[7].ReadOnly=true;
        		dataGridView3.Columns[8].ReadOnly=true;
        		dataGridView3.Columns[10].ReadOnly=true;
        		dataGridView3.Columns[11].ReadOnly=true;
        		dataGridView3.Columns[12].ReadOnly=true;
        		
        		dataGridView3.Columns[0].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[1].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[2].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[3].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[4].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[5].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[6].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[7].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[8].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[10].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[11].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView3.Columns[12].SortMode=DataGridViewColumnSortMode.NotSortable;
        		
        	}
        	
        	while(i < dataGridView3.RowCount){
        		dataGridView3.Rows[i].Cells[0].Value=false;
        		//dataGridView3.Rows[i].Cells[1].Value=false;
        		i++;
        	}

            label7.Text = "Registros: " + dataGridView3.RowCount;
            label7.Refresh();
        
        }
        
        public void llenar_dgv1(){
        	i=0;
        	
        	conex6.conectar("base_principal");
        	dataGridView1.DataSource=conex6.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,sector,receptor,fecha_visita,id_oficios,fecha_notificacion,nn,fecha_recep_contro,fecha_devolucion_not FROM base_principal.oficios WHERE estatus = \"DEVUELTO\" and fecha_devo_origen IS NULL ORDER BY razon_social");
        	conex6.cerrar();
        	
        	if(dataGridView1.RowCount > 0){
        		//columna 0 fecha
        		dataGridView1.Columns[1].HeaderText="REG. PAT./N.S.S.";
        		dataGridView1.Columns[2].HeaderText="RAZON SOCIAL";
        		dataGridView1.Columns[3].HeaderText="FOLIO";
        		dataGridView1.Columns[4].HeaderText="ACUERDO";
        		dataGridView1.Columns[5].HeaderText="EMISOR";
        		dataGridView1.Columns[6].HeaderText="SECTOR";
        		dataGridView1.Columns[7].HeaderText="RECEPTOR";
        		dataGridView1.Columns[8].HeaderText="FECHA DE VISITA";
        		dataGridView1.Columns[9].Visible=false;
        		dataGridView1.Columns[10].HeaderText="FECHA DE NOTIFICACION";
        		dataGridView1.Columns[11].HeaderText="NN";
        		dataGridView1.Columns[12].HeaderText="FECHA DE RECEPCION";
        		dataGridView1.Columns[13].HeaderText="FECHA DE DEVOLUCION";
        		
        		dataGridView1.Columns[2].MinimumWidth=300;
        		
        		dataGridView1.Columns[1].ReadOnly=true;
        		dataGridView1.Columns[2].ReadOnly=true;
        		dataGridView1.Columns[3].ReadOnly=true;
        		dataGridView1.Columns[4].ReadOnly=true;
        		dataGridView1.Columns[5].ReadOnly=true;
        		dataGridView1.Columns[6].ReadOnly=true;
        		dataGridView1.Columns[7].ReadOnly=true;
        		dataGridView1.Columns[8].ReadOnly=true;
        		dataGridView1.Columns[10].ReadOnly=true;
        		dataGridView1.Columns[11].ReadOnly=true;
        		dataGridView1.Columns[12].ReadOnly=true;
        		dataGridView1.Columns[13].ReadOnly=true;
        		
        		dataGridView1.Columns[0].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[1].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[2].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[3].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[4].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[5].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[6].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[7].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[8].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[10].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[11].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[12].SortMode=DataGridViewColumnSortMode.NotSortable;
        		dataGridView1.Columns[13].SortMode=DataGridViewColumnSortMode.NotSortable;
        		
        	}
        	
        	while(i < dataGridView1.RowCount){
        		dataGridView1.Rows[i].Cells[0].Value=false;
        		//dataGridView1.Rows[i].Cells[1].Value=false;
        		i++;
        	}

            label12.Text = "Registros: " + dataGridView1.RowCount;
            label12.Refresh();
        
        }

        public void generar_factura(){
       	int j=0,total=0,k=0,nots_tot=0;
       	String nom_not,nom_factu,contro,fech_not,fecha_hoy,dia,mes,mensaje_factu;
       	imprimir.Rows.Clear();

       	label6.Text = "Generando Facturas...";
       	label6.Refresh();
       	nom_factu="RELACIÓN DE DOCUMENTOS QUE SE ENVÍAN PARA SU NOTIFICACIÓN";
        if (checkBox2.Checked == true)
        {
            mensaje_factu = textBox4.Text;
        }
        else
        {
            mensaje_factu = " ";
        }
        
       	FolderBrowserDialog fbd = new FolderBrowserDialog();
       	fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
       	DialogResult result = fbd.ShowDialog();

       	if (result == DialogResult.OK)
       	{
       		carpeta_sel = fbd.SelectedPath.ToString();
       	}
       	else
       	{
       		Directory.CreateDirectory("C:\\" + comboBox1.SelectedItem.ToString());
       		carpeta_sel = "C:\\" + comboBox1.SelectedItem.ToString();
       	}

       	button1.Enabled = false;
       	comboBox1.Enabled = false;

       	imprimir_previo = conex.consultar("SELECT DISTINCT(receptor) FROM base_principal.oficios WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\" AND estatus=\"0\" AND fecha_recep_contro IS NULL");
       	nots_tot = imprimir_previo.Rows.Count;

       	imprimir_previo = conex.consultar("SELECT reg_nss,razon_social,folio,acuerdo,emisor,fecha_recep_contro,fecha_notificacion,nn,receptor,controlador,sector "+
       	                                  "FROM base_principal.oficios WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\" AND estatus=\"0\" AND fecha_recep_contro IS NULL ORDER BY receptor ASC");
       	total = imprimir_previo.Rows.Count;

       	nom_not = imprimir_previo.Rows[0][8].ToString();
       	contro=imprimir_previo.Rows[0][9].ToString();
       	
       	while (j < total){
       		
       		if (imprimir_previo.Rows[j][7].ToString().Equals("NN"))
       		{
       			fech_not = "NN";
       		}
       		else
       		{
       			fech_not = " ";
       			//fech_not = imprimir_previo.Rows[j][6].ToString().Substring(0, 10);//fecha_not
       		}

       		nom_not = imprimir_previo.Rows[j][8].ToString();
       		contro = imprimir_previo.Rows[j][9].ToString();
       		dia = System.DateTime.Today.Day.ToString();
       		mes = System.DateTime.Today.Month.ToString();

       		if(Convert.ToInt32(dia)<10){
       			dia = "0" + dia;
       		}

       		if (Convert.ToInt32(mes) < 10)
       		{
       			mes = "0" + mes;
       		}

       		fecha_hoy = dia+"/"+mes+"/"+System.DateTime.Today.Year.ToString();

       		imprimir.Rows.Add(
       			imprimir_previo.Rows[j][0].ToString(),//reg_nss
       			imprimir_previo.Rows[j][1].ToString(),//razon_social
       			imprimir_previo.Rows[j][2].ToString(),//folio
       			imprimir_previo.Rows[j][3].ToString(),//acuerdo
       			imprimir_previo.Rows[j][4].ToString(),//emisor
       			fecha_hoy,//fecha_recep
       			fech_not,//fech_not - nn
       			imprimir_previo.Rows[j][10].ToString());//sector

       		if ((j + 1) < total)
       		{
       			if (imprimir_previo.Rows[(j + 1)][8].ToString().Equals(nom_not))
       			{
       			}
       			else
       			{
       				k=k + 1;
       				label6.Text = "Generando Factura "+k+" de "+nots_tot;
       				label6.Refresh();
       				//MessageBox.Show("" + imprimir.Rows.Count);
       				Visor_oficios_factura vis = new Visor_oficios_factura();
       				vis.envio(imprimir, nom_factu, comboBox1.SelectedItem.ToString(), nom_not, contro, carpeta_sel, nom_not,mensaje_factu);
       				vis.Show();

       				imprimir.Rows.Clear();
       			}
       		}
       		else
       		{
       			k = k + 1;
       			label6.Text = "Generando Factura " + k + " de " + nots_tot;
       			label6.Refresh();
       			//MessageBox.Show("" + imprimir.Rows.Count);
       			Visor_oficios_factura vis = new Visor_oficios_factura();
       			vis.envio(imprimir, nom_factu, comboBox1.SelectedItem.ToString(), nom_not, contro, carpeta_sel, nom_not,mensaje_factu);
       			vis.Show();
       		}
       		j++;
       	}

       	//MessageBox.Show("Se han generado todos los Reportes adecuadamente", "Listo!");
       	
       	Process.Start("explorer.exe", carpeta_sel);
       	
       	
       }
       
        public void buscar_fech_not(){
        	
       		int buscar_col=0,temp_b=0;
       		if(dataGridView2.RowCount>0){
       			if(textBox1.Text.Length >0){
       				
       			if(!cad_buscar.Equals(textBox1.Text)){
       				b=0;
       			}
	       			cad_buscar=textBox1.Text;
	       			buscar_col=comboBox2.SelectedIndex + 2;
	       			
	       			while(b<dataGridView2.RowCount){
	       				if(dataGridView2.Rows[b].Cells[buscar_col].FormattedValue.ToString().ToUpper().Contains(cad_buscar.ToUpper())){
	       					dataGridView2.ClearSelection();
        					dataGridView2.Rows[b].Cells[buscar_col].Selected=true;
        					dataGridView2.FirstDisplayedScrollingRowIndex=b;
        					temp_b=b;
        					b=dataGridView2.RowCount+1;
	       				}
	       				b++;
	       			}
	       			//MessageBox.Show(""+temp_b);
	       			b=temp_b+1;
       			}
       		}
       	
       }
       
        public void buscar_devo(){
        	int buscar_col=0,temp_b=0;
       		if(dataGridView3.RowCount>0){
        		if(textBox2.Text.Length >0){
        			if(!cad_buscar1.Equals(textBox2.Text)){
        				b1=0;
        			}
        			cad_buscar1=textBox2.Text;
        			buscar_col=comboBox3.SelectedIndex + 1;
        			
        			while(b1<dataGridView3.RowCount){
        				if(dataGridView3.Rows[b1].Cells[buscar_col].FormattedValue.ToString().ToUpper().Contains(cad_buscar1.ToUpper())){
        					dataGridView3.ClearSelection();
        					dataGridView3.Rows[b1].Cells[buscar_col].Selected=true;
        					dataGridView3.FirstDisplayedScrollingRowIndex=b1;
        					temp_b=b1;
        					b1=dataGridView3.RowCount+1;
        				}
        				b1++;
        			}
        			//MessageBox.Show(""+temp_b);
        			b1=temp_b+1;
        		}
       		}
        }
        
        public void buscar_devo_origen(){
        	int buscar_col=0,temp_b=0;
       		if(dataGridView1.RowCount>0){
        		if(textBox3.Text.Length >0){
        			if(!cad_buscar2.Equals(textBox3.Text)){
        				b2=0;
        			}
        			cad_buscar2=textBox3.Text;
        			buscar_col=comboBox4.SelectedIndex + 1;
        			if(comboBox4.SelectedIndex==7){
        				buscar_col=13;
          			}
        			
        			while(b2<dataGridView1.RowCount){
        				if(dataGridView1.Rows[b2].Cells[buscar_col].FormattedValue.ToString().ToUpper().Contains(cad_buscar2.ToUpper())){
        					dataGridView1.ClearSelection();
        					dataGridView1.Rows[b2].Cells[buscar_col].Selected=true;
        					dataGridView1.FirstDisplayedScrollingRowIndex=b2;
        					temp_b=b2;
        					b2=dataGridView1.RowCount+1;
        				}
        				b2++;
        			}
        			//MessageBox.Show(""+temp_b);
        			b2=temp_b+1;
        		}
       		}
        }
        
        public void separar_contro(){
        	String id_usu;
        	int k=0;
        	try{
        	id_usu=MainForm.datos_user_static[7];//id_usuario
        	//MessageBox.Show(""+id_usu);
        	conex5.conectar("base_principal");
        	sectores=conex5.consultar("SELECT sector FROM base_principal.sectores where id_controlador=\""+id_usu+"\";");
        	cad_sectores="";
        	
        	while(k<sectores.Rows.Count){
        		
        		if(Convert.ToInt32(sectores.Rows[k][0].ToString())<10){
        			cad_sectores+="\"0"+sectores.Rows[k][0].ToString()+"\",";
        		}else{
        			cad_sectores+="\""+sectores.Rows[k][0].ToString()+"\",";
        		}
        		k++;
        	}
        	
        	cad_sectores=cad_sectores.Substring(0,cad_sectores.Length-1);
        	cad_sectores=" AND sector IN ("+cad_sectores+") ";
        	//MessageBox.Show(cad_sectores);
        	}catch{
        		cad_sectores="";
        	}
        }
        
        public void generar_factura_nots(){
        	String nom_fa;
        	
        	FolderBrowserDialog fbd = new FolderBrowserDialog();
        	fbd.Description = "Selecciona o crea la carpeta en la que deseas que se guarden los reportes una vez que se generen:";
        	DialogResult result = fbd.ShowDialog();

        	if (result == DialogResult.OK)
        	{
        		carpeta_sel = fbd.SelectedPath.ToString();
        	}else{
        		Directory.CreateDirectory("C:\\OFICIOS_DEVUELTOS");
                carpeta_sel = "C:\\OFICIOS_DEVUELTOS";
        	}
        		nom_fa="RELACIÓN DE DOCUMENTOS DEBIDAMENTE NOTIFICADOS";
        		Visor_oficios_factura vis = new Visor_oficios_factura();
        		vis.envio(imprimir_nots, nom_fa," ", " ", " ", carpeta_sel,"OFICIOS_POR_DEVOLVER"," ");
        		vis.Show();
        	Process.Start("explorer.exe", carpeta_sel);
        	
        }
        
        void Oficios_procesoLoad(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


            String ran;
        	
        	imprimir.Columns.Add("reg_nss");//
            imprimir.Columns.Add("razon_social");//
            imprimir.Columns.Add("folio");//
            imprimir.Columns.Add("acuerdo");//
            imprimir.Columns.Add("emisor");//
            imprimir.Columns.Add("fecha_recep_contro");//
            imprimir.Columns.Add("fecha_notificacion");//
            imprimir.Columns.Add("nn");//
            imprimir.Columns.Add("receptor");
            imprimir.Columns.Add("controlador");
            
            imprimir_nots.Columns.Add("reg_nss");//
            imprimir_nots.Columns.Add("razon_social");//
            imprimir_nots.Columns.Add("folio");//
            imprimir_nots.Columns.Add("acuerdo");//
            imprimir_nots.Columns.Add("emisor");//
            imprimir_nots.Columns.Add("fecha_recep_contro");//
            imprimir_nots.Columns.Add("fecha_notificacion");//
            imprimir_nots.Columns.Add("nn");//

            tipo=MainForm.datos_user_static[5];//puesto
            ran=MainForm.datos_user_static[2];//puesto
            
            if (tipo.Equals("Controlador"))
            {
                button1.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = false;
                button10.Enabled= false;
                separar_contro();
                
            }
            else
            {
            	if(Convert.ToInt32(ran)<3){
            		button1.Enabled = true;
	                button3.Enabled = true;
	                button4.Enabled = true;
	                button10.Enabled= true;
            	
            	}else{
	                button1.Enabled = true;
	                button3.Enabled = false;
	                button4.Enabled = true;
	                button10.Enabled= true;
            	}
            }
            
            llenar_cb1();
			llenar_dgv1();
			llenar_dgv2();
			llenar_dgv3();
			
            comboBox2.SelectedIndex=0;
            comboBox3.SelectedIndex=0;
            comboBox4.SelectedIndex=0;
		}
				
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(tabControl1.SelectedIndex==1){
				//llenar_dgv1();
			}
			
			if(tabControl1.SelectedIndex==2){
				//llenar_dgv2();
			}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			//llenar_dgv1();
			llenar_dgv2();
			//llenar_dgv3();
		}
		//entrega
		void Button1Click(object sender, EventArgs e)
		{
			String sql="",fecha_hoy="";
			
				conex.conectar("base_principal");
				res_previo.Clear();
				
				res_previo=conex.consultar("SELECT COUNT(id_oficios) FROM base_principal.oficios WHERE periodo_oficio=\""+comboBox1.SelectedItem.ToString()+"\" AND fecha_recep_contro IS NULL");
				
				if(Convert.ToInt32(res_previo.Rows[0][0].ToString()) > 0){
					DialogResult resu= MessageBox.Show("Se actualizarán "+res_previo.Rows[0][0].ToString()+ " documentos del periodo: "+comboBox1.SelectedItem.ToString()+" como Entregados al Controlador.\n\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				
					if(resu == DialogResult.Yes){
                        panel1.Visible = true;
                        generar_factura();
						conex.conectar("base_principal");
						fecha_hoy=System.DateTime.Today.Year.ToString()+"-"+System.DateTime.Today.Month.ToString()+"-"+System.DateTime.Today.Day.ToString();
                        sql = "UPDATE oficios SET fecha_recep_contro=\"" + fecha_hoy + "\",estatus=\"EN TRAMITE\" WHERE periodo_oficio=\"" + comboBox1.SelectedItem.ToString() + "\" AND fecha_recep_contro is NULL";
						conex.consultar(sql);
                        conex.guardar_evento("SE GENERARON LAS FACTURAS DE OFICIOS DEL PERIODO: " + comboBox1.SelectedItem.ToString());
						MessageBox.Show("Reportes Generados Exitosamente.\nActualización Completa","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        button1.Enabled = true;
                        comboBox1.Enabled = true;
                        panel1.Visible = false;
                        label6.Text = "Procesando...";
                        label6.Refresh();
                    }
				}else{
					MessageBox.Show("Este Periodo ya fue entregado en su totalidad","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			
		}
        //fecha_not
		void Button3Click(object sender, EventArgs e)
		{
			int marcados=0;
			String fecha_noti,fecha_vis,nn,estatus;
			i=0;
			while(i < dataGridView2.RowCount){
				if(dataGridView2.Rows[i].Cells[0].Style.BackColor.Name.Equals("SeaGreen")){
					marcados++;
				}
				i++;
			}
			
			if(marcados > 0){
				DialogResult resu= MessageBox.Show("Se registrará la fecha de notificacion de " +marcados+ " registros.\nSe Omitiran "+(dataGridView2.RowCount-marcados)+" registros por no contar con una fecha válida.\n\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
				
				if(resu == DialogResult.Yes){
					
					i=0;
					conex.conectar("base_principal");
					
					while(i < dataGridView2.RowCount){
						if(dataGridView2.Rows[i].Cells[0].Style.BackColor.Name.Equals("SeaGreen")){
							nn="";
							fecha_noti="";
							fecha_vis=dataGridView2.Rows[i].Cells[0].FormattedValue.ToString();
							fecha_vis=fecha_vis.Substring(6,4)+"-"+fecha_vis.Substring(3,2)+"-"+fecha_vis.Substring(0,2);
							
							if(dataGridView2.Rows[i].Cells[1].Value == null ){
								dataGridView2.Rows[i].Cells[1].Value=false;
							}
							
							if(dataGridView2.Rows[i].Cells[1].Value.Equals(true)){
								nn=",nn=\"NN\"";
								fecha_noti="";
								estatus="";
							}else{
								nn="";
								fecha_noti=",fecha_notificacion=\""+fecha_vis+"\"";
								estatus="estatus=\"NOTIFICADO\",";
							}
							
							conex.consultar("UPDATE oficios SET "+estatus+" fecha_visita=\""+fecha_vis+"\""+fecha_noti+""+nn+" WHERE id_oficios="+dataGridView2.Rows[i].Cells[10].FormattedValue.ToString()+" ");
                            conex.guardar_evento("SE GUARDO LA FECHA DE NOTIFICACION/NN DEL OFICIO CON EL ID: " + dataGridView2.Rows[i].Cells[10].FormattedValue.ToString());
						}
						i++;
					}
					llenar_dgv2();
				}
			}else{
				MessageBox.Show("No hay ninguna fecha que guardar.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			
		}
		//devolucion
		void Button4Click(object sender, EventArgs e)
		{
			int marcados=0;
			String  fecha_hoy,fech_not;
			i=0;
			while(i < dataGridView3.RowCount){
                if (dataGridView3.Rows[i].Cells[0].Value == null)
                {
                    dataGridView3.Rows[i].Cells[0].Value = false;
                }
				if(dataGridView3.Rows[i].Cells[0].Value.Equals(true)){
					marcados++;
				}
				i++;
			}
			
			DialogResult resu= MessageBox.Show("Se recibirán " +marcados+ " registros.\n\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
			
			if(resu == DialogResult.Yes){
				i=0;
				conex.conectar("base_principal");
				fecha_hoy=System.DateTime.Today.Year.ToString()+"-"+System.DateTime.Today.Month.ToString()+"-"+System.DateTime.Today.Day.ToString();
				while(i < dataGridView3.RowCount){
					if(dataGridView3.Rows[i].Cells[0].Value.Equals(true)){
						conex.consultar("UPDATE oficios SET estatus=\"DEVUELTO\",fecha_devolucion_not=\""+fecha_hoy+"\" WHERE id_oficios="+dataGridView3.Rows[i].Cells[9].FormattedValue.ToString()+" ");
						conex.guardar_evento("SE MARCO COMO DEVUELTO EL OFICIO CON EL ID: " + dataGridView3.Rows[i].Cells[9].FormattedValue.ToString());
					}
					i++;
				}
				
				llenar_dgv3();
			}
			
			
		}
		//devolucion a Origen
		void Button10Click(object sender, EventArgs e)
		{
			int marcados=0;
			String  fecha_hoy,fech_not;
			i=0;
			while(i < dataGridView1.RowCount){
                if (dataGridView1.Rows[i].Cells[0].Value == null)
                {
                    dataGridView1.Rows[i].Cells[0].Value = false;
                }
				if(dataGridView1.Rows[i].Cells[0].Value.Equals(true)){
					marcados++;
				}
				i++;
			}
			
			DialogResult resu= MessageBox.Show("Se enviarán al Origen " +marcados+ " registros.\n\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
			
			if(resu == DialogResult.Yes){
				imprimir_nots.Rows.Clear();
				i=0;
				conex.conectar("base_principal");
				fecha_hoy=System.DateTime.Today.Year.ToString()+"-"+System.DateTime.Today.Month.ToString()+"-"+System.DateTime.Today.Day.ToString();
				while(i < dataGridView1.RowCount){
					if(dataGridView1.Rows[i].Cells[0].Value.Equals(true)){
						conex.consultar("UPDATE oficios SET fecha_devo_origen=\""+fecha_hoy+"\" WHERE id_oficios="+dataGridView1.Rows[i].Cells[9].FormattedValue.ToString()+" ");
						conex.guardar_evento("SE MARCO COMO DEVUELTO AL ORIGEN EL OFICIO CON EL ID: " + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString());
						
						if (dataGridView1.Rows[i].Cells[11].FormattedValue.ToString().Equals("NN"))
						{
							fech_not = "NN";
						}
						else
						{
							if (dataGridView1.Rows[i].Cells[10].Value.Equals(null) || (dataGridView1.Rows[i].Cells[10].FormattedValue.ToString().Length < 1))
							{
								fech_not = " ";
							}
							else
							{
								fech_not = dataGridView1.Rows[i].Cells[10].FormattedValue.ToString().Substring(0, 10);
							}
						}
						
						imprimir_nots.Rows.Add(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString(),//reg-nss
						                       dataGridView1.Rows[i].Cells[2].FormattedValue.ToString(),//razon social
						                       dataGridView1.Rows[i].Cells[3].FormattedValue.ToString(),//folio
						                       dataGridView1.Rows[i].Cells[4].FormattedValue.ToString(),//acuerdo
						                       dataGridView1.Rows[i].Cells[5].FormattedValue.ToString(),//emisor
						                       dataGridView1.Rows[i].Cells[12].FormattedValue.ToString(),//fecha_recep
						                       fech_not,//fecha_not
						                       dataGridView1.Rows[i].Cells[6].FormattedValue.ToString());//sector
					}
					i++;
				}
				
				llenar_dgv1();
				generar_factura_nots();
			}
			
			
		}
		
		void DataGridView2CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void DataGridView2CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DateTime fecha_not;
			String fecha_hoy,fecha_noti;
			if(dataGridView2.RowCount > 0){
				fecha_noti=dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].FormattedValue.ToString();
				if(DateTime.TryParse(fecha_noti,out fecha_not)){
				
					if(fecha_not <= System.DateTime.Today){
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value=fecha_not.ToShortDateString();
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.BackColor=System.Drawing.Color.SeaGreen;
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.ForeColor=System.Drawing.Color.White;
					}else{
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.BackColor=System.Drawing.Color.Red;
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.ForeColor=System.Drawing.Color.White;
					}
						
				}else{
					if(fecha_noti.Length>0){
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.BackColor=System.Drawing.Color.Red;
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.ForeColor=System.Drawing.Color.White;
					}else{
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.BackColor=System.Drawing.SystemColors.Window;
						dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Style.ForeColor=System.Drawing.SystemColors.ControlText;
					}
				}
				
			}
		}
		
		void DataGridView2CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		//buscar fecha_not
		void Button2Click(object sender, EventArgs e)
		{
			buscar_fech_not();
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			llenar_dgv2();		
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			llenar_dgv3();
		}
		//buscar devolucion
		void Button5Click(object sender, EventArgs e)
		{
			buscar_devo();
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter)){
				buscar_fech_not();
			}
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter)){
				buscar_devo();
			}
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked==true){
				checkBox1.BackColor=System.Drawing.Color.DodgerBlue;
			}else{
				checkBox1.BackColor=System.Drawing.Color.Transparent;
			}
		}
		//buscar devolucion a origen
		void Button9Click(object sender, EventArgs e)
		{
			buscar_devo_origen();
		}
		
		void TextBox3KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)(Keys.Enter)){
				buscar_devo_origen();
			}
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			llenar_dgv1();
		}

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox4.Enabled = true;
            }
            else
            {
                textBox4.Enabled = false;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
		
		void CheckBox3CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox3.Checked==true){
				cad_sectores_temp=cad_sectores;
				cad_sectores="";
			}else{
				cad_sectores=cad_sectores_temp;
			}
		}
	}
}

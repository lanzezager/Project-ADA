/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 07/06/2016
 * Hora: 10:49 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Nova_Gear.Mod40
{
	/// <summary>
	/// Description of Menu40.
	/// </summary>
	public partial class Menu40 : Form
	{
		public Menu40()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        String nrp_dele, archivo, nss_bus,periodo,nom_arch_m40,nom_arch_m40_corto;
        int tipo_bus = 1,permiso_analisis=0;

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;
        DataTable tablanrps = new DataTable();

        public void abrir_proceso()
        {
            this.Opacity = .50;
            Carga_access loadpro = new Carga_access();
            loadpro.ShowDialog();
            this.Opacity = 1;
            loadpro.Focus();
        }

        public bool checar_mask(int num_msktbx){
            String cad="",nrp;
            int num=0,i=0,anio=0,mes=0;
            bool regresar = false;

            switch (num_msktbx)
            {
                //NSS            
                case 1:
                    cad = maskedTextBox1.Text;
                    if(cad.Length>=8){
                        regresar = true;
                        nss_bus = cad.ToUpper();
                    }
                    break;
                //Reg_Pat
                case 2:
                   nrp = maskedTextBox2.Text;
                    
                    //se quitan guiones y espacios en blanco 
                    if (nrp.Length > 9)
                    {
                        do
                        {
                            if (((nrp.Substring(i, 1)).Equals(" ")) || ((nrp.Substring(i, 1)).Equals("-")))
                            {
                            }
                            else
                            {
                                cad += nrp.Substring(i, 1);
                            }
                            i++;
                        } while (i < nrp.Length);
                    }
                   //MessageBox.Show(cad);
                    if (cad.Length==12)
                    {
                        if (cad.Contains(" ") == false)
                        {
                            nrp_dele = cad.ToUpper(); ;
                            regresar = true;
                        }
                    }
                    
                    break;
                //Periodo
                case 3:
                    cad = maskedTextBox3.Text;
                    if(cad.Length>5){
                        if((int.TryParse(cad,out num))==true){

                            anio = Convert.ToInt32(cad.Substring(0, 4));
                            mes = Convert.ToInt32(cad.Substring(4,2));

                            if(((anio>1996)&&(anio<=System.DateTime.Today.Year))&&((mes>0)&&(mes<13))){
                                regresar = true;
                                periodo = cad;
                            }
                        }
                    }
                    break;

                //Reg_Pat factura txt
                case 4:
                    nrp = maskedTextBox4.Text;

                    //se quitan guiones y espacios en blanco 
                    if (nrp.Length > 9)
                    {
                        do
                        {
                            if ((nrp.Substring(i, 1)).Equals(" "))
                            {
                            }
                            else
                            {
                                cad += nrp.Substring(i, 1);
                            }
                            i++;
                        } while (i < nrp.Length);
                    }
                    //MessageBox.Show(cad);
                    if (cad.Length == 15)
                    {
                        if (cad.Contains(" ") == false)
                        {
                            nrp_dele = cad.ToUpper().Substring(1,cad.Length-1); ;
                            regresar = true;
                        }
                    }

                    break;
            }

            return regresar;
        }
        
        public void checar_existe_factura(){
        	string[] archivos = new string[Directory.GetFiles(@"mod40/").Length];
            int c=0;
            archivos = Directory.GetFiles(@"mod40/");
            
            permiso_analisis=0;
            
            while(c<archivos.Length){
            	
            	if((archivos[c].Substring(archivos[c].Length-4,4)).Equals("LZ40")){
            		permiso_analisis=1;
            		nom_arch_m40=archivos[c];
            	}
            	c++;
            }
        }

        public void leer_nrps_sub()
        {
            String cad_con = "", hoja = "", cons_exc="";
            //esta cadena es para archivos excel 2007 y 2010
            cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='lista_nrps_mod40.xlsx';Extended Properties=Excel 12.0;";
            conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
            conexion.Open(); //abrimos la conexion
            
            hoja = "Hoja1";

            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                cons_exc = "Select [NRP] from [" + hoja + "$] WHERE [NUM_SUB]=\"" +conex.leer_config_sub()[4]+ "\"";
                try
                {
                    //Si el usuario escribio el nombre de la hoja se procedera con la busqueda
                    //conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                    //conexion.Open(); //abrimos la conexion
                    dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                    if (dataAdapter.Equals(null))
                    {
                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");
                    }
                    else
                    {
                        dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                        tablanrps = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        conexion.Close();//cerramos la conexion
                        //MessageBox.Show(tablanrps.Rows[0][0].ToString());
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }
            }
        }

		void Panel2Paint(object sender, PaintEventArgs e)
		{
			
		}

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Navy;
            button2.BackColor = System.Drawing.Color.Transparent;
            button3.BackColor = System.Drawing.Color.Transparent;
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Transparent;
            button2.BackColor = System.Drawing.Color.Navy;
            button3.BackColor = System.Drawing.Color.Transparent;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
        }

		private void Button3Click(object sender, EventArgs e)
		{
            button1.BackColor = System.Drawing.Color.Transparent;
            button2.BackColor = System.Drawing.Color.Transparent;
            button3.BackColor = System.Drawing.Color.Navy;
            panel1.Visible = true; 
            panel2.Visible = false;
            panel3.Visible = false;
		}

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Menu40_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            button1.BackColor = System.Drawing.Color.Navy;
            panel2.Visible = true;
            
           String rango= MainForm.datos_user_static[2];
        	if(Convert.ToInt32(rango)==0||Convert.ToInt32(rango)==11){
        		button3.Enabled=true;        		
        	}else{
        		button3.Enabled=false;
        	}

            //leer_nrps_sub();
        }

        private void maskedTextBox2_Click(object sender, EventArgs e)
        {
            maskedTextBox2.Clear();
            maskedTextBox2.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
            {
                if (checar_mask(2) == true)//REG_PAT
                {
                    if (checar_mask(3) == true)//PERIODO
                    {
                        if(!label5.Text.Equals("...")){
                            button4.Enabled = true;
                        }
                    }
                    else
                    {
                        button4.Enabled = false;
                    }
                }
                else
                {
                    button4.Enabled = false;
                }
            }

            if (panel2.Visible == true)
            {
                if (checar_mask(1) == true)//NSS,CURP,RFC,Nombre
                {
                    button4.Enabled = true;
                }
                else
                {
                    button4.Enabled = false;
                }
            }

            if (panel1.Visible == true)
            {
                if(!label2.Text.Equals("..."))//archivoTXT
                {
                    if (checar_mask(4) == true)//Reg_Pat2
                    {
                        button4.Enabled = true;
                    }
                    else
                    {
                        button4.Enabled = false;
                    }
                }
                else
                {
                    button4.Enabled = false;
                }
                
            }
        }
        //boton continuar
        private void button4_Click(object sender, EventArgs e)
        {
            if (panel3.Visible)
            {
                DialogResult res = MessageBox.Show("Se Cargará el archivo Access/SUA:\n" + archivo + "\n\nEsto ingresará información a la Base de Datos.\n¿Desea Continuar?\n\n", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (res == DialogResult.Yes)
                {
                    Carga_access carga = new Carga_access();
                    //MessageBox.Show(archivo+" "+nrp_dele);
                    carga.cargar_access_sua(archivo,nrp_dele,periodo);
                    this.Opacity = 0;
                    carga.ShowDialog();
                    this.Opacity = 1;
                }
            }

            if(panel2.Visible==true)//Consulta de asegurado
            {
                Consulta_Mod40 cons = new Consulta_Mod40();
                //MessageBox.Show(nss_bus+" "+tipo_bus);
                if(cons.buscar_valor(nss_bus, tipo_bus)==true){
	                this.Opacity = 0;
	                cons.Show();
	                this.Opacity = 1;
                }
            }

            if (panel1.Visible)
            {
                DialogResult res = MessageBox.Show("Se Analizará la Factura de Texto de Asegurados:\n" + archivo + "\n\nEste proceso puede tardar algunos minutos.\n¿Desea Continuar?\n\n", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (res == DialogResult.Yes)
                {
                    Carga_access carga = new Carga_access();
                    //MessageBox.Show(archivo);
                    carga.cargar_txt(archivo,nrp_dele);
                    this.Opacity = 0;
                    carga.ShowDialog();
                    this.Opacity = 1;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true){
                label1.Text = "NSS:";
                tipo_bus = 1;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                label1.Text = "CURP:";
                tipo_bus = 2;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                label1.Text = "RFC:";
                tipo_bus = 3;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                label1.Text = "Nombre:";
                tipo_bus = 4;
            } 
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void maskedTextBox2_Leave(object sender, EventArgs e)
        {
            maskedTextBox2.Text = maskedTextBox2.Text.ToUpper();
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            maskedTextBox1.Text = maskedTextBox1.Text.ToUpper();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
           
            dialog.Filter = "Archivos de Access (*.mdb;*.accdb)|*.mdb;*.accdb"; //le indicamos el tipo de filtro en este caso que busque
            //solo los archivos Access
            dialog.Title = "Seleccione el archivo de Access del CDSUA";//le damos un titulo a la ventana

            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archivo = dialog.FileName;
                label5.Text = dialog.SafeFileName;
                
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
        	checar_existe_factura();
        	try{
        		if(permiso_analisis==0){
        			OpenFileDialog dialog = new OpenFileDialog();

        			dialog.Filter = "Archivos de Texto (*.P;*.TXT)|*.p;*.txt"; //le indicamos el tipo de filtro en este caso que busque
        			//solo los archivos Access
        			dialog.Title = "Seleccione la Factura de Texto de Asegurados";//le damos un titulo a la ventana

        			dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo

        			//si al seleccionar el archivo damos Ok
        			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        			{
        				archivo = dialog.FileName;
        				label2.Text = dialog.SafeFileName;

        			}
        		}else{
        			MessageBox.Show("No se ha generado el Oficio de Baja del último análisis.\nNo se puede efectuar un nuevo análisis hasta que se genere un Oficio de Baja.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        		}
        	}catch(Exception ed){
        		
        	}
        	
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }    
		
        void MaskedTextBox1KeyPress(object sender, KeyPressEventArgs e)
        {
        	if (e.KeyChar == (char)(Keys.Enter))
        	{
        		if(panel2.Visible==true)//Consulta de asegurado
        		{
        			if(tipo_bus==1){
        				if(maskedTextBox1.Text.Length>10){
        					Consulta_Mod40 cons = new Consulta_Mod40();
        					//MessageBox.Show(nss_bus+" "+tipo_bus);
        					if(cons.buscar_valor(nss_bus, tipo_bus)==true){
        						this.Opacity = 0;
        						cons.ShowDialog();
        						this.Opacity = 1;
        					}
        				}
        			}else{
        				Consulta_Mod40 cons = new Consulta_Mod40();
        				//MessageBox.Show(nss_bus+" "+tipo_bus);
        				if(cons.buscar_valor(nss_bus, tipo_bus)==true){
        					this.Opacity = 0;
        					cons.ShowDialog();
        					this.Opacity = 1;
        				}
        			}
        		}
        	}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Info40 info_40 = new Info40();
            info_40.ShowDialog();
        }
		
		void Label7Click(object sender, EventArgs e)
		{
			//Carga_excel40 carga40 = new Carga_excel40();
			//carga40.Show();
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			try{
				checar_existe_factura();
				
				if(permiso_analisis==1){
					/*OpenFileDialog dialog1 = new OpenFileDialog();
				String archivo1;

				dialog1.Filter = "Archivos de Nova Gear Modalidad 40 (*.LZ40)|*.LZ40"; //le indicamos el tipo de filtro en este caso que busque
				//solo los archivos Access
				dialog1.Title = "Seleccione el archivo";//le damos un titulo a la ventana

				dialog1.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
				
				//si al seleccionar el archivo damos Ok
				if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{*/
					nom_arch_m40_corto=nom_arch_m40.Substring(6);
					Resultados_analisis resu_check = new Resultados_analisis();
					resu_check.recibe_arch(nom_arch_m40,nom_arch_m40_corto);
					resu_check.Show();
					//}
				}else{
					MessageBox.Show("No hay resultados que leer.\nEfectue un nuevo análisis de factura para que se pueda crear un nuevo archivo de resultados.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}catch(Exception ed){
				
			}
		}

        private void button9_Click(object sender, EventArgs e)
        {
            Sindo_Mod40 sin40 = new Sindo_Mod40();
            sin40.Show();
        }
	}
}

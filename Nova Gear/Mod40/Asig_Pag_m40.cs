/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 16/02/2018
 * Hora: 10:14 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;

namespace Nova_Gear.Mod40
{
	/// <summary>
	/// Description of Asig_Pag_m40.
	/// </summary>
	public partial class Asig_Pag_m40 : Form
	{
		public Asig_Pag_m40(String nss_persistente)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			this.nss_persist= nss_persistente;
		}
		
		String nss_persist;
		
		 //Conexion MySQL
        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        DataTable consultamysql = new DataTable();
		
		void Asig_Pag_m40Load(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

		}

		public void asigna_pags(){
			if(textBox2.Text.Length>4){
				conex.conectar("base_principal");
				consultamysql=conex.consultar("SELECT periodo_pago,folio_sua,fecha_pago,importe_pago,imp_eym_pree,imp_inv_vida,imp_cesa_y_vej,imp_eym_pree_o,imp_inv_vida_o,imp_cesa_y_vej_o,per_cd_sua,tipo_modalidad,observaciones,nombre_trabajador,nss "+
				                              "FROM mod40_sua where nss= \""+textBox2.Text+"\" ORDER BY periodo_pago ASC,fecha_pago");
				
				if(consultamysql.Rows.Count>0){
					dataGridView1.DataSource=consultamysql;
					textBox1.Text=dataGridView1.Rows[0].Cells[13].FormattedValue.ToString().TrimEnd(' ');
					dataGridView1.Columns[13].Visible=false;
					dataGridView1.Columns[14].Visible=false;
					
					dataGridView1.Columns[0].HeaderText="PERIODO";
					dataGridView1.Columns[1].HeaderText="FOLIO SUA";
					dataGridView1.Columns[2].HeaderText="FECHA PAGO";
					dataGridView1.Columns[3].HeaderText="IMPORTE PAGO";
					dataGridView1.Columns[4].HeaderText="IMP_EYM_PREE";
					dataGridView1.Columns[5].HeaderText="IMP_INV_VIDA";
					dataGridView1.Columns[6].HeaderText="IMP_CESA_Y_VEJ";
					dataGridView1.Columns[7].HeaderText="IMP_EYM_PREE_O";
					dataGridView1.Columns[8].HeaderText="IMP_INV_VIDA_O";
					dataGridView1.Columns[9].HeaderText="IMP_CESA_Y_VEJ_O";
					dataGridView1.Columns[10].HeaderText="PERIODO\nCD SUA";
					dataGridView1.Columns[11].HeaderText="TIPO DE\nMODALIDAD";
					dataGridView1.Columns[12].HeaderText="OBSERVACIONES";
					
					label4.Text="Periodos Encontrados: "+dataGridView1.RowCount;
					button1.Enabled=true;
					textBox2.Enabled=false;
					button3.Enabled=false;
				}else{
					MessageBox.Show("Búsqueda sin resultados","AVISO");
					button1.Enabled=false;
				}
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			asigna_pags();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(dataGridView1.RowCount>0){
				if(dataGridView1.Rows[0].Cells[14].Value.ToString().Equals(nss_persist)==true){
					DialogResult resu = MessageBox.Show("Se Asignaran los Pagos de\nEste NSS:"+textBox2.Text+" a este NSS:"+nss_persist+".\nEste Cambio no pueder ser Revertido.\n\n¿Está seguro que desea Continuar?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					
					if(resu==DialogResult.Yes){
						conex2.conectar("base_principal");
						conex2.consultar("UPDATE mod40_sua SET nss=\""+nss_persist+"\" WHERE nss=\""+textBox2.Text+"\" ");
						MessageBox.Show("¡Los Pagos se han reasignado Exitosamente!","EXITO");
						this.Close();
					}
				}else{
					MessageBox.Show("No se Pueden Reasignar los Pagos del Mismo Asegurado","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				}
			}else{
				MessageBox.Show("No hay Pagos que Reasignar","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar==Convert.ToChar(Keys.Enter)){
				asigna_pags();
			}
		}
	}
}

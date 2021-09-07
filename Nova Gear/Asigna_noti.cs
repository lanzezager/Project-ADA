/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 22/03/2016
 * Hora: 05:03 p. m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Asigna_noti.
	/// </summary>
	public partial class Asigna_noti : Form
	{
		public Asigna_noti(String peri,int tipo_peri)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.periodo = peri;
			this.tipo_per= tipo_peri;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		String sql,periodo,sector,noti,contro,rp_bus;
		int i=0,j=0,retros=0,tipo_per=0;
		
		
		//Declaracion de elementos para conexion mysql
		Conexion conex = new Conexion();
		Conexion conex2 = new Conexion();
		Conexion conex3 = new Conexion();
			
		DataTable consultamysql = new DataTable();
		DataTable consulta2 = new DataTable();
		
		public int retro_lectura(){
			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			conex3.conectar("base_principal");
			if(tipo_per==0){
				sql="SELECT registro_patronal2,sector_notificacion_actualizado FROM datos_factura WHERE nombre_periodo=\""+periodo+"\";";
			}else{
				sql="SELECT registro_patronal2,sector_notificacion_actualizado FROM datos_factura WHERE periodo_factura=\""+periodo+"\";";
			}
			
			dataGridView1.DataSource=conex.consultar(sql);
			
			i=0;
			j=0;
			retros=0;
			do{
				rp_bus = dataGridView1.Rows[i].Cells[0].FormattedValue.ToString();
				sector = dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
				sql="SELECT sector_notificacion_actualizado FROM datos_factura WHERE registro_patronal2 =\""+rp_bus+"\" AND nn <> \"NN\" AND (status=\"NOTIFICADO\" OR status =\"CARTERA\") ORDER BY fecha_entrega DESC LIMIT 1";
				dataGridView2.DataSource=conex2.consultar(sql);
				if(dataGridView2.RowCount > 0){
					if(dataGridView2.Rows[0].Cells[0].FormattedValue.ToString().Length>0){
						if(!(dataGridView2.Rows[0].Cells[0].FormattedValue.ToString().Equals(sector))){
							if(tipo_per==0){
								sql="UPDATE datos_factura SET sector_notificacion_actualizado="+dataGridView2.Rows[0].Cells[0].FormattedValue.ToString()+" WHERE registro_patronal2=\""+rp_bus+"\" AND nombre_periodo=\""+periodo+"\";";
							}else{
								sql="UPDATE datos_factura SET sector_notificacion_actualizado="+dataGridView2.Rows[0].Cells[0].FormattedValue.ToString()+" WHERE registro_patronal2=\""+rp_bus+"\" AND periodo_factura=\""+periodo+"\";";
							}
							conex3.consultar(sql);
							retros++;
						}
					}
				}
				i++;
			}while(i<dataGridView1.RowCount);
			
			return retros;
		}
		
		void Asigna_notiLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
			
			this.Visible=false;
			conex.conectar("base_principal");
			conex2.conectar("base_principal");
			//conex3.conectar("base_principal");
			i=0;
			sql="SELECT * FROM sectores";
			consultamysql=conex.consultar(sql);
			dataGridView1.DataSource=consultamysql;			
			
			do{
		
				//sacar nombre notificador
				sector=dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();	
				sql="SELECT nombre,apellido FROM usuarios WHERE id_usuario="+dataGridView1.Rows[i].Cells[2].FormattedValue.ToString()+";";
				dataGridView2.DataSource=conex2.consultar(sql);
				noti= dataGridView2.Rows[0].Cells[1].FormattedValue.ToString()+" "+dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
				
				//sacar nombre controlador
				sector=dataGridView1.Rows[i].Cells[3].FormattedValue.ToString();
				sql="SELECT nombre,apellido FROM usuarios WHERE id_usuario="+dataGridView1.Rows[i].Cells[1].FormattedValue.ToString()+";";
				dataGridView2.DataSource=conex2.consultar(sql);
				contro = dataGridView2.Rows[0].Cells[1].FormattedValue.ToString()+" "+dataGridView2.Rows[0].Cells[0].FormattedValue.ToString();
				if(tipo_per==0){
					sql="UPDATE datos_factura SET notificador=\""+noti+"\",controlador=\""+contro+"\" WHERE nombre_periodo=\""+periodo+"\" AND sector_notificacion_actualizado="+sector+" AND fecha_entrega IS NULL;";
				}else{
					sql="UPDATE datos_factura SET notificador=\""+noti+"\",controlador=\""+contro+"\" WHERE periodo_factura=\""+periodo+"\" AND sector_notificacion_actualizado="+sector+" AND fecha_entrega IS NULL;";
				}
				conex2.consultar(sql);
				i++;
				this.Text="cuenta: "+i;
			}while(i<dataGridView1.RowCount);
			
			//Colocar Inexistente
			if(tipo_per==0){
				sql="UPDATE datos_factura SET notificador=\"Inexistente Notificador\", controlador=\"Inexistente Controlador\"  WHERE nombre_periodo=\""+periodo+"\" AND notificador= \"-\" AND fecha_entrega IS NULL";
			}else{
				sql="UPDATE datos_factura SET notificador=\"Inexistente Notificador\", controlador=\"Inexistente Controlador\"  WHERE periodo_factura=\""+periodo+"\" AND notificador= \"-\" AND fecha_entrega IS NULL";
			}
			conex2.consultar(sql);
			
			//MessageBox.Show("Se asigno el nombre de notificador y controlador exitosamente","Exito");
		}
	}
}

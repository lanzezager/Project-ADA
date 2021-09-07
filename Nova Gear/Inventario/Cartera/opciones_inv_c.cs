/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 06/03/2018
 * Hora: 11:47 p. m.
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

namespace Nova_Gear.Inventario.Cartera
{
	/// <summary>
	/// Description of opciones_inv_c.
	/// </summary>
	public partial class opciones_inv_c : Form
	{
		public opciones_inv_c()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		Conexion conex = new Conexion();
		Conexion conex1 = new Conexion();
		Conexion conex2 = new Conexion();
		
		DataTable rale_igual = new DataTable();
		DataTable rale_no_vig = new DataTable();
		DataTable rale_nuevos = new DataTable();
		
		int iguales=0,nuevos=0,no_vig=0;
		
		public void procesa_rales()
		{
			int rale_nvos_tot=0;
			String tipo_rale="";
			conex2.conectar("cartera_inv");
			rale_nuevos=conex2.consultar("SELECT clase_doc FROM cartera_temp LIMIT 1");
			tipo_rale=rale_nuevos.Rows[0][0].ToString();
			
			rale_nuevos=conex2.consultar("SELECT COUNT(idcartera_inv) FROM cartera_inv_base WHERE clase_doc=\""+tipo_rale+"\"");
			
			if((Convert.ToInt32(rale_nuevos.Rows[0][0].ToString()))>0){
				//TOTALES
				conex.conectar("cartera_inv");
				rale_igual=conex.consultar("SELECT COUNT(idcartera_inv) FROM cartera_inv_base WHERE cartera_inv_base.clase_doc=\""+tipo_rale+"\" AND cartera_inv_base.llave IN (SELECT cartera_temp.llave FROM cartera_temp)");
				iguales=Convert.ToInt32(rale_igual.Rows[0][0].ToString());
				
				conex1.conectar("cartera_inv");
				rale_no_vig=conex1.consultar("SELECT COUNT(idcartera_inv) FROM cartera_inv_base WHERE cartera_inv_base.clase_doc=\""+tipo_rale+"\" AND cartera_inv_base.status=\"VIGENTE\" AND cartera_inv_base.llave NOT IN (SELECT cartera_temp.llave FROM cartera_temp) ");
				no_vig=Convert.ToInt32(rale_no_vig.Rows[0][0].ToString());
				
				rale_nuevos=rale_no_vig=conex1.consultar("SELECT COUNT(idcartera_temp) FROM cartera_temp WHERE cartera_temp.llave NOT IN (SELECT cartera_inv_base.llave FROM cartera_inv_base WHERE cartera_inv_base.clase_doc=\""+tipo_rale+"\")");
				nuevos=Convert.ToInt32(rale_nuevos.Rows[0][0].ToString());
				
				//OPERACIONES
				
				//Modificar Iguales --Actualizar Incidencias
				conex.consultar("UPDATE cartera_inv_base INNER JOIN cartera_temp ON cartera_inv_base.llave = cartera_temp.llave SET cartera_inv_base.incidencia=cartera_temp.incidencia,cartera_inv_base.fecha_incidencia=cartera_temp.fecha_incidencia");
				//Marcar NO_VIGs
				conex1.consultar("UPDATE cartera_inv_base SET status=\"NO_VIGENTE\" WHERE cartera_inv_base.clase_doc=\""+tipo_rale+"\" AND cartera_inv_base.status=\"VIGENTE\" AND cartera_inv_base.llave NOT IN (SELECT cartera_temp.llave FROM cartera_temp)");
				//Insertar nuevos en inv_base
				conex2.consultar("INSERT INTO cartera_inv_base (reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,fecha_rale,llave) "+
				                 "SELECT cartera_temp.reg_pat,cartera_temp.mov,cartera_temp.fecha_mov,cartera_temp.sector,cartera_temp.credito,cartera_temp.ce,cartera_temp.periodo,cartera_temp.tipo_doc,cartera_temp.fecha_alta,cartera_temp.fecha_not,cartera_temp.incidencia,cartera_temp.fecha_incidencia,cartera_temp.dias,cartera_temp.importe,cartera_temp.clase_doc,cartera_temp.fecha_rale,cartera_temp.llave FROM cartera_temp " +
				                 "WHERE cartera_temp.llave NOT IN (SELECT cartera_inv_base.llave FROM cartera_inv_base WHERE cartera_inv_base.clase_doc=\""+tipo_rale+"\")");
				
				MessageBox.Show("El Análisis concluyo exitosamente:\nSe Actualizaron: "+iguales+" Créditos ya Existentes.\nSe marcaron: "+no_vig+" créditos como NO_VIGENTES. \n Se Agregaron: "+nuevos+" créditos nuevos ");
						
			}else{
				//rale_nuevos=conex2.consultar("SELECT reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,fecha_rale FROM cartera_temp");
				//guarda_nuevos();
				conex2.consultar("INSERT INTO cartera_inv_base (reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,fecha_rale,llave,status) "+
				                 "SELECT cartera_temp.reg_pat,cartera_temp.mov,cartera_temp.fecha_mov,cartera_temp.sector,cartera_temp.credito,cartera_temp.ce,cartera_temp.periodo,cartera_temp.tipo_doc,cartera_temp.fecha_alta,cartera_temp.fecha_not,cartera_temp.incidencia,cartera_temp.fecha_incidencia,cartera_temp.dias,cartera_temp.importe,cartera_temp.clase_doc,cartera_temp.fecha_rale,cartera_temp.llave,cartera_temp.status FROM cartera_temp ");
				rale_nuevos=conex2.consultar("SELECT COUNT(idcartera_temp) FROM cartera_temp");
				MessageBox.Show("Se Ingresaron: "+rale_nuevos.Rows[0][0].ToString()+" casos del RALE recien procesado","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
			}
			
		}
		
		public void guarda_nuevos(){
			
			int i=0,k=0;
			String f1,f2,f3,f4,f5,f6,f7,f8,f9,f10;
			conex.conectar("cartera_inv");
			while(i<rale_nuevos.Rows.Count){
				if((i+10)<=rale_nuevos.Rows.Count){
					f1=rale_nuevos.Rows[i+0][15].ToString();
					f1=f1.Substring(6,4)+f1.Substring(3,2)+f1.Substring(0,2);
					f2=rale_nuevos.Rows[i+1][15].ToString();
					f2=f2.Substring(6,4)+f2.Substring(3,2)+f2.Substring(0,2);
					f3=rale_nuevos.Rows[i+2][15].ToString();
					f3=f3.Substring(6,4)+f3.Substring(3,2)+f3.Substring(0,2);
					f4=rale_nuevos.Rows[i+3][15].ToString();
					f4=f4.Substring(6,4)+f4.Substring(3,2)+f4.Substring(0,2);
					f5=rale_nuevos.Rows[i+4][15].ToString();
					f5=f5.Substring(6,4)+f5.Substring(3,2)+f5.Substring(0,2);
					f6=rale_nuevos.Rows[i+5][15].ToString();
					f6=f6.Substring(6,4)+f6.Substring(3,2)+f6.Substring(0,2);
					f7=rale_nuevos.Rows[i+6][15].ToString();
					f7=f7.Substring(6,4)+f7.Substring(3,2)+f7.Substring(0,2);
					f8=rale_nuevos.Rows[i+7][15].ToString();
					f8=f8.Substring(6,4)+f8.Substring(3,2)+f8.Substring(0,2);
					f9=rale_nuevos.Rows[i+8][15].ToString();
					f9=f9.Substring(6,4)+f9.Substring(3,2)+f9.Substring(0,2);
					f10=rale_nuevos.Rows[i+9][15].ToString();
					f10=f10.Substring(6,4)+f10.Substring(3,2)+f10.Substring(0,2);
					
					conex.consultar("INSERT INTO cartera_inv_base (reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,fecha_rale) VALUES " +
					                "(\""+rale_nuevos.Rows[i+0][0].ToString()+"\","+rale_nuevos.Rows[i+0][1].ToString()+",\""+rale_nuevos.Rows[i+0][2].ToString()+"\","+rale_nuevos.Rows[i+0][3].ToString()+",\""+rale_nuevos.Rows[i+0][4].ToString()+"\",\""+rale_nuevos.Rows[i+0][5].ToString()+"\",\""+rale_nuevos.Rows[i+0][6].ToString()+"\",\""+rale_nuevos.Rows[i+0][7].ToString()+"\",\""+rale_nuevos.Rows[i+0][8].ToString()+"\",\""+rale_nuevos.Rows[i+0][9].ToString()+"\",\""+rale_nuevos.Rows[i+0][10].ToString()+"\",\""+rale_nuevos.Rows[i+0][11].ToString()+"\","+rale_nuevos.Rows[i+0][12].ToString()+","+rale_nuevos.Rows[i+0][13].ToString()+",\""+rale_nuevos.Rows[i+0][14].ToString()+"\",\""+f1+"\"),"+
					                "(\""+rale_nuevos.Rows[i+1][0].ToString()+"\","+rale_nuevos.Rows[i+1][1].ToString()+",\""+rale_nuevos.Rows[i+1][2].ToString()+"\","+rale_nuevos.Rows[i+1][3].ToString()+",\""+rale_nuevos.Rows[i+1][4].ToString()+"\",\""+rale_nuevos.Rows[i+1][5].ToString()+"\",\""+rale_nuevos.Rows[i+1][6].ToString()+"\",\""+rale_nuevos.Rows[i+1][7].ToString()+"\",\""+rale_nuevos.Rows[i+1][8].ToString()+"\",\""+rale_nuevos.Rows[i+1][9].ToString()+"\",\""+rale_nuevos.Rows[i+1][10].ToString()+"\",\""+rale_nuevos.Rows[i+1][11].ToString()+"\","+rale_nuevos.Rows[i+1][12].ToString()+","+rale_nuevos.Rows[i+1][13].ToString()+",\""+rale_nuevos.Rows[i+1][14].ToString()+"\",\""+f2+"\"),"+
					                "(\""+rale_nuevos.Rows[i+2][0].ToString()+"\","+rale_nuevos.Rows[i+2][1].ToString()+",\""+rale_nuevos.Rows[i+2][2].ToString()+"\","+rale_nuevos.Rows[i+2][3].ToString()+",\""+rale_nuevos.Rows[i+2][4].ToString()+"\",\""+rale_nuevos.Rows[i+2][5].ToString()+"\",\""+rale_nuevos.Rows[i+2][6].ToString()+"\",\""+rale_nuevos.Rows[i+2][7].ToString()+"\",\""+rale_nuevos.Rows[i+2][8].ToString()+"\",\""+rale_nuevos.Rows[i+2][9].ToString()+"\",\""+rale_nuevos.Rows[i+2][10].ToString()+"\",\""+rale_nuevos.Rows[i+2][11].ToString()+"\","+rale_nuevos.Rows[i+2][12].ToString()+","+rale_nuevos.Rows[i+2][13].ToString()+",\""+rale_nuevos.Rows[i+2][14].ToString()+"\",\""+f3+"\"),"+
					                "(\""+rale_nuevos.Rows[i+3][0].ToString()+"\","+rale_nuevos.Rows[i+3][1].ToString()+",\""+rale_nuevos.Rows[i+3][2].ToString()+"\","+rale_nuevos.Rows[i+3][3].ToString()+",\""+rale_nuevos.Rows[i+3][4].ToString()+"\",\""+rale_nuevos.Rows[i+3][5].ToString()+"\",\""+rale_nuevos.Rows[i+3][6].ToString()+"\",\""+rale_nuevos.Rows[i+3][7].ToString()+"\",\""+rale_nuevos.Rows[i+3][8].ToString()+"\",\""+rale_nuevos.Rows[i+3][9].ToString()+"\",\""+rale_nuevos.Rows[i+3][10].ToString()+"\",\""+rale_nuevos.Rows[i+3][11].ToString()+"\","+rale_nuevos.Rows[i+3][12].ToString()+","+rale_nuevos.Rows[i+3][13].ToString()+",\""+rale_nuevos.Rows[i+3][14].ToString()+"\",\""+f4+"\"),"+
					                "(\""+rale_nuevos.Rows[i+4][0].ToString()+"\","+rale_nuevos.Rows[i+4][1].ToString()+",\""+rale_nuevos.Rows[i+4][2].ToString()+"\","+rale_nuevos.Rows[i+4][3].ToString()+",\""+rale_nuevos.Rows[i+4][4].ToString()+"\",\""+rale_nuevos.Rows[i+4][5].ToString()+"\",\""+rale_nuevos.Rows[i+4][6].ToString()+"\",\""+rale_nuevos.Rows[i+4][7].ToString()+"\",\""+rale_nuevos.Rows[i+4][8].ToString()+"\",\""+rale_nuevos.Rows[i+4][9].ToString()+"\",\""+rale_nuevos.Rows[i+4][10].ToString()+"\",\""+rale_nuevos.Rows[i+4][11].ToString()+"\","+rale_nuevos.Rows[i+4][12].ToString()+","+rale_nuevos.Rows[i+4][13].ToString()+",\""+rale_nuevos.Rows[i+4][14].ToString()+"\",\""+f5+"\"),"+
					                "(\""+rale_nuevos.Rows[i+5][0].ToString()+"\","+rale_nuevos.Rows[i+5][1].ToString()+",\""+rale_nuevos.Rows[i+5][2].ToString()+"\","+rale_nuevos.Rows[i+5][3].ToString()+",\""+rale_nuevos.Rows[i+5][4].ToString()+"\",\""+rale_nuevos.Rows[i+5][5].ToString()+"\",\""+rale_nuevos.Rows[i+5][6].ToString()+"\",\""+rale_nuevos.Rows[i+5][7].ToString()+"\",\""+rale_nuevos.Rows[i+5][8].ToString()+"\",\""+rale_nuevos.Rows[i+5][9].ToString()+"\",\""+rale_nuevos.Rows[i+5][10].ToString()+"\",\""+rale_nuevos.Rows[i+5][11].ToString()+"\","+rale_nuevos.Rows[i+5][12].ToString()+","+rale_nuevos.Rows[i+5][13].ToString()+",\""+rale_nuevos.Rows[i+5][14].ToString()+"\",\""+f6+"\"),"+
					                "(\""+rale_nuevos.Rows[i+6][0].ToString()+"\","+rale_nuevos.Rows[i+6][1].ToString()+",\""+rale_nuevos.Rows[i+6][2].ToString()+"\","+rale_nuevos.Rows[i+6][3].ToString()+",\""+rale_nuevos.Rows[i+6][4].ToString()+"\",\""+rale_nuevos.Rows[i+6][5].ToString()+"\",\""+rale_nuevos.Rows[i+6][6].ToString()+"\",\""+rale_nuevos.Rows[i+6][7].ToString()+"\",\""+rale_nuevos.Rows[i+6][8].ToString()+"\",\""+rale_nuevos.Rows[i+6][9].ToString()+"\",\""+rale_nuevos.Rows[i+6][10].ToString()+"\",\""+rale_nuevos.Rows[i+6][11].ToString()+"\","+rale_nuevos.Rows[i+6][12].ToString()+","+rale_nuevos.Rows[i+6][13].ToString()+",\""+rale_nuevos.Rows[i+6][14].ToString()+"\",\""+f7+"\"),"+
					                "(\""+rale_nuevos.Rows[i+7][0].ToString()+"\","+rale_nuevos.Rows[i+7][1].ToString()+",\""+rale_nuevos.Rows[i+7][2].ToString()+"\","+rale_nuevos.Rows[i+7][3].ToString()+",\""+rale_nuevos.Rows[i+7][4].ToString()+"\",\""+rale_nuevos.Rows[i+7][5].ToString()+"\",\""+rale_nuevos.Rows[i+7][6].ToString()+"\",\""+rale_nuevos.Rows[i+7][7].ToString()+"\",\""+rale_nuevos.Rows[i+7][8].ToString()+"\",\""+rale_nuevos.Rows[i+7][9].ToString()+"\",\""+rale_nuevos.Rows[i+7][10].ToString()+"\",\""+rale_nuevos.Rows[i+7][11].ToString()+"\","+rale_nuevos.Rows[i+7][12].ToString()+","+rale_nuevos.Rows[i+7][13].ToString()+",\""+rale_nuevos.Rows[i+7][14].ToString()+"\",\""+f8+"\"),"+
					                "(\""+rale_nuevos.Rows[i+8][0].ToString()+"\","+rale_nuevos.Rows[i+8][1].ToString()+",\""+rale_nuevos.Rows[i+8][2].ToString()+"\","+rale_nuevos.Rows[i+8][3].ToString()+",\""+rale_nuevos.Rows[i+8][4].ToString()+"\",\""+rale_nuevos.Rows[i+8][5].ToString()+"\",\""+rale_nuevos.Rows[i+8][6].ToString()+"\",\""+rale_nuevos.Rows[i+8][7].ToString()+"\",\""+rale_nuevos.Rows[i+8][8].ToString()+"\",\""+rale_nuevos.Rows[i+8][9].ToString()+"\",\""+rale_nuevos.Rows[i+8][10].ToString()+"\",\""+rale_nuevos.Rows[i+8][11].ToString()+"\","+rale_nuevos.Rows[i+8][12].ToString()+","+rale_nuevos.Rows[i+8][13].ToString()+",\""+rale_nuevos.Rows[i+8][14].ToString()+"\",\""+f9+"\"),"+
					                "(\""+rale_nuevos.Rows[i+9][0].ToString()+"\","+rale_nuevos.Rows[i+9][1].ToString()+",\""+rale_nuevos.Rows[i+9][2].ToString()+"\","+rale_nuevos.Rows[i+9][3].ToString()+",\""+rale_nuevos.Rows[i+9][4].ToString()+"\",\""+rale_nuevos.Rows[i+9][5].ToString()+"\",\""+rale_nuevos.Rows[i+9][6].ToString()+"\",\""+rale_nuevos.Rows[i+9][7].ToString()+"\",\""+rale_nuevos.Rows[i+9][8].ToString()+"\",\""+rale_nuevos.Rows[i+9][9].ToString()+"\",\""+rale_nuevos.Rows[i+9][10].ToString()+"\",\""+rale_nuevos.Rows[i+9][11].ToString()+"\","+rale_nuevos.Rows[i+9][12].ToString()+","+rale_nuevos.Rows[i+9][13].ToString()+",\""+rale_nuevos.Rows[i+9][14].ToString()+"\",\""+f10+"\")");
					i=i+10;
				}else{
					k=i;
					i=rale_nuevos.Rows.Count;
				}
			}
			
			i=k;
			
			while(i<rale_nuevos.Rows.Count){
				f1=rale_nuevos.Rows[i+0][15].ToString();
				f1=f1.Substring(6,4)+f1.Substring(3,2)+f1.Substring(0,2);
				conex.consultar("INSERT INTO cartera_inv_base (reg_pat,mov,fecha_mov,sector,credito,ce,periodo,tipo_doc,fecha_alta,fecha_not,incidencia,fecha_incidencia,dias,importe,clase_doc,fecha_rale) VALUES " +
				                "(\""+rale_nuevos.Rows[i+0][0].ToString()+"\","+rale_nuevos.Rows[i+0][1].ToString()+",\""+rale_nuevos.Rows[i+0][2].ToString()+"\","+rale_nuevos.Rows[i+0][3].ToString()+",\""+rale_nuevos.Rows[i+0][4].ToString()+"\",\""+rale_nuevos.Rows[i+0][5].ToString()+"\",\""+rale_nuevos.Rows[i+0][6].ToString()+"\",\""+rale_nuevos.Rows[i+0][7].ToString()+"\",\""+rale_nuevos.Rows[i+0][8].ToString()+"\",\""+rale_nuevos.Rows[i+0][9].ToString()+"\",\""+rale_nuevos.Rows[i+0][10].ToString()+"\",\""+rale_nuevos.Rows[i+0][11].ToString()+"\","+rale_nuevos.Rows[i+0][12].ToString()+","+rale_nuevos.Rows[i+0][13].ToString()+",\""+rale_nuevos.Rows[i+0][14].ToString()+"\",\""+f1+"\")");
				i++;
			}
			
			MessageBox.Show("Se Ingreso el RALE "+rale_nuevos.Rows[1][14].ToString().ToUpper()+" de "+rale_nuevos.Rows.Count+" casos.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Lector_rale_txt lee_rale = new Lector_rale_txt(3);
			lee_rale.ShowDialog();
			MessageBox.Show("Da Click para Comenzar con el Análisis.","AVISO");
			procesa_rales();
			
		}

		private void opciones_inv_c_Load(object sender, EventArgs e)
		{

            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


		}

		void Button2Click(object sender, EventArgs e)
		{
			No_vig nop = new No_vig();
			nop.Show();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			Vigs sip = new Vigs();
			sip.Show();
		}
	}
}

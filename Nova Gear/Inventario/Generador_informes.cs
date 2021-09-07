/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 21/11/2017
 * Hora: 06:49 p.m.
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

namespace Nova_Gear.Inventario
{
	/// <summary>
	/// Description of Generador_informes.
	/// </summary>
	public partial class Generador_informes : Form
	{
		public Generador_informes()
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
		Conexion conex3 = new Conexion();
		Conexion conex4 = new Conexion();
		Conexion conex5 = new Conexion();
        Conexion conex6 = new Conexion();
        Conexion conex7 = new Conexion();
        Conexion conex8 = new Conexion();
        Conexion conex9 = new Conexion();
        Conexion conex10 = new Conexion();
		
		DataTable base_inv = new DataTable();
		DataTable responsables = new DataTable();
		DataTable inc = new DataTable();
		DataTable td = new DataTable();
        DataTable sobrante = new DataTable();
        DataTable listado = new DataTable();
        DataTable imprimir = new DataTable();
        DataTable correccion= new DataTable();
        DataTable faltante = new DataTable();
        
        String tipo_inf="",fech_rep="",fech_impre="";
        int tipo_info = 0,temp_i=0,temp_j=0,temp2_i=0,temp2_j=0,temp3_i=0,temp3_j=0;
		
		void TabPage1Click(object sender, EventArgs e)
		{
			
		}
		
		public string interprete_fechas(String fecha){
			String fecha_inter;
			
			fecha_inter=fecha.Substring(0,2);
			//MessageBox.Show(fecha.Substring(3,2));
			switch(fecha.Substring(3,2)){
					case "01": fecha_inter= fecha_inter+" de Enero de "+fecha.Substring(6,4);
					break;
					
					case "02": fecha_inter= fecha_inter+" de Febrero de "+fecha.Substring(6,4);
					break;
					
					case "03": fecha_inter= fecha_inter+" de Marzo de "+fecha.Substring(6,4);
					break;
					
					case "04": fecha_inter= fecha_inter+" de Abril de "+fecha.Substring(6,4);
					break;
					
					case "05": fecha_inter= fecha_inter+" de Mayo de "+fecha.Substring(6,4);
					break;
					
					case "06": fecha_inter= fecha_inter+" de Junio de "+fecha.Substring(6,4);
					break;
					
					case "07": fecha_inter= fecha_inter+" de Julio de "+fecha.Substring(6,4);
					break;
					
					case "08": fecha_inter= fecha_inter+" de Agosto de "+fecha.Substring(6,4);
					break;
					
					case "09": fecha_inter= fecha_inter+" de Septiembre de "+fecha.Substring(6,4);
					break;
					
					case "10": fecha_inter= fecha_inter+" de Octubre de "+fecha.Substring(6,4);
					break;
					
					case "11": fecha_inter= fecha_inter+" de Noviembre de "+fecha.Substring(6,4);
					break;
					
					case "12": fecha_inter= fecha_inter+" de Diciembre de "+fecha.Substring(6,4);
					break;
			}
			return fecha_inter;
		}
		
		public string recorta_decimales(decimal nro){
			string nro_dos_dec;
			int pos_pto=0;
			nro_dos_dec=nro.ToString();
			pos_pto=nro_dos_dec.IndexOf('.');
			try{
				nro_dos_dec=nro_dos_dec.Substring(0,(pos_pto+3));
			}catch{}
			return nro_dos_dec;
		}
		
		public void llena_resumen_gral(){
			int i=0,total_base=0,total=0,parcial=0,otro=0,suma=0,pos_coma=0;
			decimal porcentaje_individual=0,porcentaje_avance=0,asignados=0,porcentaje_asig_sum=0,porcentaje_avan_sum=0,importe_revi_sum=0,importe_tot_sum=0,asigs_sum=0,total_sum=0,parcial_sum=0,otro_sum=0,tot_sum=0,faltantes_sum=0,total_porcent=0;
			String importe;
			conex.conectar("inventario");
			base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE idbase_inventario > 0 "+tipo_inf );
			total_base=Convert.ToInt32(base_inv.Rows[0][0].ToString());
			dataGridView1.Rows.Clear();
			while(i<responsables.Rows.Count){
				dataGridView1.Rows.Add();
				dataGridView1.Rows[i].Cells[0].Value=Convert.ToInt32(responsables.Rows[i][1].ToString());
				dataGridView1.Rows[i].Cells[1].Value=responsables.Rows[i][0].ToString();
				
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE no_cuaderno="+responsables.Rows[i][1].ToString()+" "+tipo_inf);
				if(base_inv.Rows.Count>0){
					dataGridView1.Rows[i].Cells[3].Value=Convert.ToDecimal(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView1.Rows[i].Cells[3].Value=Convert.ToDecimal(0);
				}
				porcentaje_individual=(((Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString()))*100)/total_base);
				dataGridView1.Rows[i].Cells[2].Value=Convert.ToDecimal(recorta_decimales(porcentaje_individual));
				porcentaje_asig_sum=porcentaje_asig_sum+(Convert.ToDecimal(recorta_decimales(porcentaje_individual)));
				asigs_sum=asigs_sum+(Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString()));
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE no_cuaderno="+responsables.Rows[i][1].ToString()+" AND coincidente=\"TOTAL\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView1.Rows[i].Cells[4].Value=Convert.ToDecimal(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView1.Rows[i].Cells[4].Value=0;
				}
				total=Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value.ToString());
				total_sum=total_sum+total;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE no_cuaderno="+responsables.Rows[i][1].ToString()+" AND coincidente=\"PARCIAL\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView1.Rows[i].Cells[5].Value=Convert.ToDecimal(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView1.Rows[i].Cells[5].Value=0;
				}
				parcial=Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value.ToString());
				parcial_sum=parcial_sum+parcial;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE no_cuaderno="+responsables.Rows[i][1].ToString()+" AND coincidente=\"OTRO\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView1.Rows[i].Cells[6].Value=Convert.ToDecimal(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView1.Rows[i].Cells[6].Value=0;
				}
				otro=Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString());
				otro_sum=otro_sum+otro;
				
				suma=(total+parcial+otro);
				tot_sum=tot_sum+suma;
				dataGridView1.Rows[i].Cells[7].Value=Convert.ToDecimal(suma);
				asignados=Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
				if(asignados>0){
					porcentaje_avance=((suma*100)/asignados);
				}else{
					porcentaje_avance=0;
				}
				porcentaje_avan_sum=porcentaje_avan_sum+(Convert.ToDecimal(recorta_decimales(porcentaje_avance)));
				dataGridView1.Rows[i].Cells[8].Value=Convert.ToDecimal(recorta_decimales(porcentaje_avance));
				dataGridView1.Rows[i].Cells[9].Value=Convert.ToDecimal(asignados-suma);
				faltantes_sum=faltantes_sum+(Convert.ToInt32(asignados-suma));
				
				base_inv=conex.consultar("SELECT SUM(importe) FROM base_inventario WHERE no_cuaderno="+responsables.Rows[i][1].ToString()+" AND coincidente <> \"-\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					importe=base_inv.Rows[0][0].ToString();
					if(importe.Length==0){
						importe="0";
					}
					importe_revi_sum=importe_revi_sum+(Convert.ToDecimal(importe));
					pos_coma=importe.IndexOf('.');
					while((pos_coma-3) >= 1){
						importe=importe.Insert((pos_coma-3),",");
						pos_coma=pos_coma-3;
					}
					dataGridView1.Rows[i].Cells[10].Value=Convert.ToDecimal(importe);
				}else{
					dataGridView1.Rows[i].Cells[10].Value=0.00;
				}
				
				base_inv=conex.consultar("SELECT SUM(importe) FROM base_inventario WHERE no_cuaderno="+responsables.Rows[i][1].ToString()+" "+tipo_inf);
				if(base_inv.Rows.Count>0){
					importe=base_inv.Rows[0][0].ToString();
					if(importe.Length==0){
						importe="0";
					}
					importe_tot_sum=importe_tot_sum+(Convert.ToDecimal(importe));
					pos_coma=importe.IndexOf('.');
					while((pos_coma-3) >= 1){
						importe=importe.Insert((pos_coma-3),",");
						pos_coma=pos_coma-3;
					}
					dataGridView1.Rows[i].Cells[11].Value=Convert.ToDecimal(importe);
				}else{
					dataGridView1.Rows[i].Cells[11].Value=0.00;
				}
				
				i++;
			}
			
			//TOTAL
			dataGridView1.Rows.Add();
			dataGridView1.Rows[i].Cells[0].Value=(i+1);
			dataGridView1.Rows[i].Cells[1].Value="TOTAL";
			if((Convert.ToDecimal(recorta_decimales(porcentaje_asig_sum)))>= Convert.ToDecimal(99.90 )){
				dataGridView1.Rows[i].Cells[2].Value=Convert.ToDecimal(100.00);
			}else{
				dataGridView1.Rows[i].Cells[2].Value=porcentaje_asig_sum;
			}
			
			dataGridView1.Rows[i].Cells[3].Value=asigs_sum;
			dataGridView1.Rows[i].Cells[4].Value=total_sum;
			dataGridView1.Rows[i].Cells[5].Value=parcial_sum;
			dataGridView1.Rows[i].Cells[6].Value=otro_sum;
			dataGridView1.Rows[i].Cells[7].Value=tot_sum;
			dataGridView1.Rows[i].Cells[8].Value=porcentaje_avan_sum;
			dataGridView1.Rows[i].Cells[9].Value=faltantes_sum;
			importe=recorta_decimales(importe_revi_sum);
			pos_coma=importe.IndexOf('.');
			while((pos_coma-3) >= 1){
				importe=importe.Insert((pos_coma-3),",");
				pos_coma=pos_coma-3;
			}
			dataGridView1.Rows[i].Cells[10].Value=Convert.ToDecimal(importe);
			
			importe=recorta_decimales(importe_tot_sum);
			pos_coma=importe.IndexOf('.');
			while((pos_coma-3) >= 1){
				importe=importe.Insert((pos_coma-3),",");
				pos_coma=pos_coma-3;
			}
			dataGridView1.Rows[i].Cells[11].Value=Convert.ToDecimal(importe);
			
			//PROMEDIO
			dataGridView1.Rows.Add();
			dataGridView1.Rows[i+1].Cells[0].Value=(i+2);
			dataGridView1.Rows[i+1].Cells[1].Value="PROMEDIO";
			if((Convert.ToDecimal(recorta_decimales(porcentaje_asig_sum/(i))))>= Convert.ToDecimal(99.80 )){
				dataGridView1.Rows[i+1].Cells[2].Value=Convert.ToDecimal(100.00);
			}else{
				dataGridView1.Rows[i+1].Cells[2].Value=Convert.ToDecimal(recorta_decimales(porcentaje_asig_sum/(i)));
			}
			
			dataGridView1.Rows[i+1].Cells[3].Value=Convert.ToDecimal(recorta_decimales(asigs_sum/(i)));
			dataGridView1.Rows[i+1].Cells[4].Value=Convert.ToDecimal(recorta_decimales(total_sum/(i)));
			dataGridView1.Rows[i+1].Cells[5].Value=Convert.ToDecimal(recorta_decimales(parcial_sum/(i)));
			dataGridView1.Rows[i+1].Cells[6].Value=Convert.ToDecimal(recorta_decimales(otro_sum/(i)));
			dataGridView1.Rows[i+1].Cells[7].Value=Convert.ToDecimal(recorta_decimales(tot_sum/(i)));
			dataGridView1.Rows[i+1].Cells[8].Value=Convert.ToDecimal(recorta_decimales(porcentaje_avan_sum/(i)));
			total_porcent=Convert.ToDecimal(recorta_decimales(porcentaje_avan_sum/(i)));
			dataGridView1.Rows[i+1].Cells[9].Value=Convert.ToDecimal(recorta_decimales(faltantes_sum/(i)));
			importe=recorta_decimales((importe_revi_sum/(i)));
			pos_coma=importe.IndexOf('.');
			while((pos_coma-3) >= 1){
				importe=importe.Insert((pos_coma-3),",");
				pos_coma=pos_coma-3;
			}
			dataGridView1.Rows[i+1].Cells[10].Value=Convert.ToDecimal(importe);
			
			importe=recorta_decimales((importe_tot_sum/(i)));
			pos_coma=importe.IndexOf('.');
			while((pos_coma-3) >= 1){
				importe=importe.Insert((pos_coma-3),",");
				pos_coma=pos_coma-3;
			}
			dataGridView1.Rows[i+1].Cells[11].Value=Convert.ToDecimal(importe);
			
			dataGridView1.Rows[i].Cells[8].Value=total_porcent;
			dataGridView1.Rows.RemoveAt(i+1);
		}
		
		public void llena_resumen_inc(){
			int i=0,total=0,parcial=0,otro=0,suma=0,todo=0,total_sum=0,parcial_sum=0,otro_sum=0,tot_sum=0,de_sum=0,faltan_sum=0;
			String incc="";
			conex.conectar("inventario");
			dataGridView2.Rows.Clear();
			while(i<inc.Rows.Count){
				dataGridView2.Rows.Add();
				incc=inc.Rows[i][1].ToString();
				if(incc.Length==1){
					incc="0"+incc;
				}
				dataGridView2.Rows[i].Cells[0].Value=inc.Rows[i][0].ToString();
				dataGridView2.Rows[i].Cells[1].Value=incc;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE incidencia=\""+Convert.ToInt32(inc.Rows[i][1].ToString())+"\" AND coincidente=\"TOTAL\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView2.Rows[i].Cells[2].Value=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView2.Rows[i].Cells[2].Value=0;
				}
				total=Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value.ToString());
				total_sum=total_sum+total;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE incidencia=\""+Convert.ToInt32(inc.Rows[i][1].ToString())+"\" AND coincidente=\"PARCIAL\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView2.Rows[i].Cells[3].Value=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView2.Rows[i].Cells[3].Value=0;
				}
				parcial=Convert.ToInt32(dataGridView2.Rows[i].Cells[3].Value.ToString());
				parcial_sum=parcial_sum+parcial;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE incidencia=\""+Convert.ToInt32(inc.Rows[i][1].ToString())+"\" AND coincidente=\"OTRO\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView2.Rows[i].Cells[4].Value=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView2.Rows[i].Cells[4].Value=0;
				}
				otro=Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value.ToString());
				otro_sum=otro_sum+otro;
				dataGridView2.Rows[i].Cells[5].Value=Convert.ToInt32(total+parcial+otro);
				tot_sum=tot_sum+(total+parcial+otro);
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE incidencia=\""+Convert.ToInt32(inc.Rows[i][1].ToString())+"\" "+tipo_inf+" ");
				if(base_inv.Rows.Count>0){
					todo=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					total=0;
				}
				
				dataGridView2.Rows[i].Cells[6].Value=todo;
				de_sum=de_sum+todo;
				dataGridView2.Rows[i].Cells[7].Value=todo-(total+parcial+otro);
				faltan_sum=faltan_sum+(todo-(total+parcial+otro));
				i++;
			}
			
			//TOTAL
			dataGridView2.Rows.Add();
			dataGridView2.Rows[i].Cells[0].Value="TOTAL";
			dataGridView2.Rows[i].Cells[1].Value="99";
			dataGridView2.Rows[i].Cells[2].Value=total_sum;
			dataGridView2.Rows[i].Cells[3].Value=parcial_sum;
			dataGridView2.Rows[i].Cells[4].Value=otro_sum;
			dataGridView2.Rows[i].Cells[5].Value=tot_sum;
			dataGridView2.Rows[i].Cells[6].Value=de_sum;
			dataGridView2.Rows[i].Cells[7].Value=faltan_sum;
			
		}
		
		public void llena_resumen_td(){
			int i=0,total=0,parcial=0,otro=0,suma=0,todo=0,total_sum=0,parcial_sum=0,otro_sum=0,tot_sum=0,de_sum=0,faltan_sum=0;
			String  t_d="";
			dataGridView3.Rows.Clear();
			
			while(i<td.Rows.Count){
				dataGridView3.Rows.Add();
				t_d=td.Rows[i][1].ToString();
				if(t_d.Length==1){
					t_d="0"+t_d;
				}
				dataGridView3.Rows[i].Cells[0].Value=td.Rows[i][0].ToString();
				dataGridView3.Rows[i].Cells[1].Value=t_d;
				
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE tipo_doc=\""+t_d+"\" AND coincidente=\"TOTAL\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView3.Rows[i].Cells[2].Value=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView3.Rows[i].Cells[2].Value=0;
				}
				total=Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value.ToString());
				total_sum=total_sum+total;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE tipo_doc=\""+t_d+"\" AND coincidente=\"PARCIAL\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView3.Rows[i].Cells[3].Value=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView3.Rows[i].Cells[3].Value=0;
				}
				parcial=Convert.ToInt32(dataGridView3.Rows[i].Cells[3].Value.ToString());
				parcial_sum=parcial_sum+parcial;
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE tipo_doc=\""+t_d+"\" AND coincidente=\"OTRO\" "+tipo_inf+" "+fech_rep);
				if(base_inv.Rows.Count>0){
					dataGridView3.Rows[i].Cells[4].Value=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					dataGridView3.Rows[i].Cells[4].Value=0;
				}
				otro=Convert.ToInt32(dataGridView3.Rows[i].Cells[4].Value.ToString());
				otro_sum=otro_sum+otro;
				dataGridView3.Rows[i].Cells[5].Value=(total+parcial+otro);
				tot_sum=tot_sum+(total+parcial+otro);
				base_inv=conex.consultar("SELECT COUNT(idbase_inventario) FROM base_inventario WHERE tipo_doc=\""+t_d+"\" "+tipo_inf+" ");
				if(base_inv.Rows.Count>0){
					todo=Convert.ToInt32(base_inv.Rows[0][0].ToString());
				}else{
					todo=0;
				}
				dataGridView3.Rows[i].Cells[6].Value=todo;
				de_sum=de_sum+todo;
				dataGridView3.Rows[i].Cells[7].Value=todo-(total+parcial+otro);
				faltan_sum=faltan_sum+(todo-(total+parcial+otro));
				i++;
			}
			
			//TOTAL
			dataGridView3.Rows.Add();
			dataGridView3.Rows[i].Cells[0].Value="TOTAL";
			dataGridView3.Rows[i].Cells[1].Value="99";
			dataGridView3.Rows[i].Cells[2].Value=total_sum;
			dataGridView3.Rows[i].Cells[3].Value=parcial_sum;
			dataGridView3.Rows[i].Cells[4].Value=otro_sum;
			dataGridView3.Rows[i].Cells[5].Value=tot_sum;
			dataGridView3.Rows[i].Cells[6].Value=de_sum;
			dataGridView3.Rows[i].Cells[7].Value=faltan_sum;
			
		}
		
		public void estilo_dgv1(){
			int i=0,j=0;
			while(i<dataGridView1.RowCount){
				if((dataGridView1.Rows[i].Cells[1].Value.ToString().Equals("TOTAL"))||(dataGridView1.Rows[i].Cells[1].Value.ToString().Equals("PROMEDIO"))){
					j=0;
					while(j<dataGridView1.ColumnCount){
						dataGridView1.Rows[i].Cells[j].Style.BackColor=Color.Gainsboro;
							j++;
					}
				}
				i++;
			}
		}
		
		public void estilo_dgv2(){
			int i=0,j=0;
			while(i<dataGridView2.RowCount){
				if(dataGridView2.Rows[i].Cells[0].Value.ToString().Equals("TOTAL")){
					j=0;
					while(j<dataGridView2.ColumnCount){
						dataGridView2.Rows[i].Cells[j].Style.BackColor=Color.Gainsboro;
							j++;
					}
				}
				i++;
			}
		}
		
		public void estilo_dgv3(){
			int i=0,j=0;
			while(i<dataGridView3.RowCount){
				if(dataGridView3.Rows[i].Cells[0].Value.ToString().Equals("TOTAL")){
					j=0;
					while(j<dataGridView3.ColumnCount){
						dataGridView3.Rows[i].Cells[j].Style.BackColor=Color.Gainsboro;
							j++;
					}
				}
				i++;
			}
		}
		
		public void estilo_dgv4(){
			int i=0,j=0;
			while(i<dataGridView4.RowCount){
				if(dataGridView4.Rows[i].Cells[0].Value.ToString().Equals("TOTAL")){
					j=0;
					while(j<dataGridView4.ColumnCount){
						dataGridView4.Rows[i].Cells[j].Style.BackColor=Color.DeepSkyBlue;
							j++;
					}
				}
				
				if(dataGridView4.Rows[i].Cells[0].Value.ToString().Equals("REVISADOS")){
					j=0;
					while(j<dataGridView4.ColumnCount){
						dataGridView4.Rows[i].Cells[j].Style.BackColor=Color.LimeGreen;
							j++;
					}
				}
				
				if(dataGridView4.Rows[i].Cells[0].Value.ToString().Equals("FALTANTES")){
					j=0;
					while(j<dataGridView4.ColumnCount){
						dataGridView4.Rows[i].Cells[j].Style.BackColor=Color.Gold;
							j++;
					}
				}
				i++;
			}
		}
		
		public void buscar_sobrante(){
			int i=0,j=0;
			if(dataGridView5.RowCount>0){
				i=temp_i;
				j=temp_j;
				while(i<dataGridView5.RowCount){
					while(j<dataGridView5.ColumnCount){
						if(dataGridView5.Rows[i].Cells[j].Value.ToString().ToUpper().Contains(textBox1.Text.ToUpper())==true){
							temp_j=j+1;
							temp_i=i;
							dataGridView5.ClearSelection();
							dataGridView5.Rows[i].Cells[j].Selected=true;
							if(i<5){
								dataGridView5.FirstDisplayedScrollingRowIndex=0;
							}else{
								dataGridView5.FirstDisplayedScrollingRowIndex=(i)-3;
							}
							j=dataGridView5.ColumnCount+1;
							i=dataGridView5.RowCount+1;
						}
						j++;
					}
					j=0;
					i++;
				}
				
				if((temp_i>=dataGridView5.RowCount)||(i==dataGridView5.RowCount)){
					MessageBox.Show("Se terminó de revisar toda la Lista y no se Encontró el valor buscado.","AVISO");
				}
			}
		}
		
		public void buscar_invent(){
			int i=0,j=0;
			if(dataGridView6.RowCount>0){
				i=temp2_i;
				j=temp2_j;
				while(i<dataGridView6.RowCount){
					while(j<dataGridView6.ColumnCount){
						if(dataGridView6.Rows[i].Cells[j].Value.ToString().ToUpper().Contains(textBox2.Text.ToUpper())==true){
							temp2_j=j+1;
							temp2_i=i;
							dataGridView6.ClearSelection();
							dataGridView6.Rows[i].Cells[j].Selected=true;
							if(i<5){
								dataGridView6.FirstDisplayedScrollingRowIndex=0;
							}else{
								dataGridView6.FirstDisplayedScrollingRowIndex=(i)-3;
							}
							
							if(j<5){
								dataGridView6.FirstDisplayedScrollingColumnIndex=0;
							}else{
								dataGridView6.FirstDisplayedScrollingColumnIndex=(j)-3;
							}
							
							j=dataGridView6.ColumnCount+1;
							i=dataGridView6.RowCount+1;
						}
						j++;
					}
					j=0;
					i++;
				}
				
				if((temp2_i>=dataGridView6.RowCount)||(i==dataGridView6.RowCount)){
					MessageBox.Show("Se terminó de revisar toda la Lista y no se Encontró el valor buscado.","AVISO");
				}
			}
		}
		
		public void buscar_faltantes(){
			int i=0,j=0;
			if(dataGridView8.RowCount>0){
				i=temp3_i;
				j=temp3_j;
				while(i<dataGridView8.RowCount){
					while(j<dataGridView8.ColumnCount){
						if(dataGridView8.Rows[i].Cells[j].Value.ToString().ToUpper().Contains(textBox6.Text.ToUpper())==true){
							temp3_j=j+1;
							temp3_i=i;
							dataGridView8.ClearSelection();
							dataGridView8.Rows[i].Cells[j].Selected=true;
							if(i<5){
								dataGridView8.FirstDisplayedScrollingRowIndex=0;
							}else{
								dataGridView8.FirstDisplayedScrollingRowIndex=(i)-3;
							}
							j=dataGridView8.ColumnCount+1;
							i=dataGridView8.RowCount+1;
						}
						j++;
					}
					j=0;
					i++;
				}
				
				if((temp3_i>=dataGridView8.RowCount)||(i==dataGridView8.RowCount)){
					MessageBox.Show("Se terminó de revisar toda la Lista y no se Encontró el valor buscado.","AVISO");
				}
			}
		}
		
		public void busca_correciones(){
			String reg="";
			
			conex8.conectar("inventario");
			if(maskedTextBox1.MaskCompleted==true){
				if(maskedTextBox2.MaskCompleted==true){
					reg=maskedTextBox1.Text;
					reg=reg.Substring(0,3)+"-"+reg.Substring(6,5)+"-"+reg.Substring(14,2);
					reg=reg.ToUpper();
					//MessageBox.Show(reg);
					correccion=conex8.consultar("SELECT idbase_inventario, reg_pat, credito,periodo_mes,periodo_anio,tipo_doc,incidencia,coincidente,observaciones,responsable,fecha_verificacion,no_cuaderno,doc_reg_pat,doc_credito,doc_periodo,doc_importe,doc_td,tipo_inv FROM base_inventario WHERE reg_pat=\""+reg+"\" and credito=\""+maskedTextBox2.Text+"\"");
					dataGridView7.DataSource=correccion;
					maskedTextBox3.Clear();
					maskedTextBox4.Clear();
					maskedTextBox5.Clear();
					comboBox2.SelectedIndex=-1;
					textBox3.Text="";
					textBox4.Text="";
					textBox5.Text="";
				}
			}
		}
		
		void Generador_informesLoad(object sender, EventArgs e)
		{
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;


			//label4.Text="Informes al Día: "+interprete_fechas(DateTime.Today.ToShortDateString());

			fech_impre = "del día "+DateTime.Today.ToShortDateString();
			conex1.conectar("inventario");
			conex2.conectar("base_principal");
			conex3.conectar("base_principal");
			
			responsables=conex1.consultar("SELECT nombre,libro,casos FROM responsables ORDER BY idresponsables");
			inc=conex2.consultar("SELECT descripcion,inc FROM incidencias ORDER by inc");
			td=conex3.consultar("SELECT descripcion,td FROM tipos_doc ORDER by td");
			
			llena_resumen_gral();
			estilo_dgv1();
			
			llena_resumen_inc();
			estilo_dgv2();
			
			llena_resumen_td();
			estilo_dgv3();

			comboBox1.SelectedIndex=0;
			this.Focus();
		}
		//impresion
		void Button2Click(object sender, EventArgs e)
		{
            int i = 0,j=0,pos_coma=0;
            String importe_rev, importe_tot,porcent,tip_inf="";
            //MessageBox.Show(""+tabControl1.SelectedIndex);
            imprimir.Clear();

            if(tipo_info==0){
                tip_inf = "COP Y RCV";
            }

            if (tipo_info == 1)
            {
                tip_inf = "COP";
            }

            if (tipo_info == 2)
            {
                tip_inf = "RCV";
            }

            if (tipo_info == 3)
            {
                tip_inf = "MÉXICO";
            }

            

            if (tabControl1.SelectedIndex==0)//resumen general
            {
                while (i < dataGridView1.ColumnCount)
                {
                    imprimir.Columns.Add();
                    i++;
                }

                i = 0;
               
                while(i<dataGridView1.RowCount){
                    importe_rev=dataGridView1.Rows[i].Cells[10].Value.ToString();
                    pos_coma = importe_rev.IndexOf('.');
                    while ((pos_coma - 3) >= 1)
                    {
                        importe_rev = importe_rev.Insert((pos_coma - 3), ",");
                        pos_coma = pos_coma - 3;
                    }
                    importe_rev = "$ " + importe_rev;

                    importe_tot = dataGridView1.Rows[i].Cells[11].Value.ToString();
                    pos_coma = importe_tot.IndexOf('.');
                    while ((pos_coma - 3) >= 1)
                    {
                        importe_tot = importe_tot.Insert((pos_coma - 3), ",");
                        pos_coma = pos_coma - 3;
                    }
                    importe_tot = "$ " + importe_tot;



                    imprimir.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(),dataGridView1.Rows[i].Cells[1].Value.ToString(),
                                      dataGridView1.Rows[i].Cells[2].Value.ToString()+" %",dataGridView1.Rows[i].Cells[3].Value.ToString(),
                                      dataGridView1.Rows[i].Cells[4].Value.ToString(),dataGridView1.Rows[i].Cells[5].Value.ToString(),
                                      dataGridView1.Rows[i].Cells[6].Value.ToString(),dataGridView1.Rows[i].Cells[7].Value.ToString(),
                                      dataGridView1.Rows[i].Cells[8].Value.ToString() + " %", dataGridView1.Rows[i].Cells[9].Value.ToString(),
                                      importe_rev,importe_tot);
                    i++;
                }

                Visor_informes inform = new Visor_informes();
                inform.envio_res_gral(imprimir, fech_impre,tip_inf);
                inform.Show();
            }
            //---------------------------------//
            if (tabControl1.SelectedIndex == 1)//resumen revisados incidencia
            {
                while (i < dataGridView2.ColumnCount)
                {
                    imprimir.Columns.Add();
                    i++;
                }

                i = 0;

                while (i < dataGridView2.RowCount)
                {
                    imprimir.Rows.Add(dataGridView2.Rows[i].Cells[0].Value.ToString(), dataGridView2.Rows[i].Cells[1].Value.ToString(),
                                      dataGridView2.Rows[i].Cells[2].Value.ToString(), dataGridView2.Rows[i].Cells[3].Value.ToString(),
                                      dataGridView2.Rows[i].Cells[4].Value.ToString(), dataGridView2.Rows[i].Cells[5].Value.ToString(),
                                      dataGridView2.Rows[i].Cells[6].Value.ToString(), dataGridView2.Rows[i].Cells[7].Value.ToString());
                    i++;
                }

                Visor_informes inform = new Visor_informes();
                inform.envio_cred_rev(imprimir,"INCIDENCIA", fech_impre,tip_inf);
                inform.Show();
            }
            //---------------------------------//
            if (tabControl1.SelectedIndex == 2)//resumen td
            {
                while (i < dataGridView3.ColumnCount)
                {
                    imprimir.Columns.Add();
                    i++;
                }

                i = 0;

                while (i < dataGridView3.RowCount)
                {
                    imprimir.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(), dataGridView3.Rows[i].Cells[1].Value.ToString(),
                                      dataGridView3.Rows[i].Cells[2].Value.ToString(), dataGridView3.Rows[i].Cells[3].Value.ToString(),
                                      dataGridView3.Rows[i].Cells[4].Value.ToString(), dataGridView3.Rows[i].Cells[5].Value.ToString(),
                                      dataGridView3.Rows[i].Cells[6].Value.ToString(), dataGridView3.Rows[i].Cells[7].Value.ToString());
                    i++;
                }

                Visor_informes inform = new Visor_informes();
                inform.envio_cred_rev(imprimir, "TIPO DE DOCUMENTO",  fech_impre,tip_inf);
                inform.Show();
            }
            //---------------------------------//
            if (tabControl1.SelectedIndex == 3)//resumen dia
            {
                while (i < dataGridView4.ColumnCount)
                {
                    imprimir.Columns.Add();
                    i++;
                }

                i = 0;

                while (i < dataGridView4.RowCount)
                {
                    importe_tot = dataGridView4.Rows[i].Cells[3].Value.ToString();
                    pos_coma = importe_tot.IndexOf('.');
                    while ((pos_coma - 3) >= 1)
                    {
                        importe_tot = importe_tot.Insert((pos_coma - 3), ",");
                        pos_coma = pos_coma - 3;
                    }
                    importe_tot = "$ " + importe_tot;

                    porcent = dataGridView4.Rows[i].Cells[4].Value.ToString();

                    if (porcent.Contains(".")==true)
                    {
                         pos_coma = porcent.IndexOf('.');
                         if (((porcent.Length - pos_coma) - 1) >= 3)
                         {//numero de decimales
                             porcent = porcent.Substring(0, pos_coma + 4) + " %";
                         }
                         else
                         {
                             porcent = porcent + " %";
                         }
                    }else{
                        porcent = porcent + " %";
                    }
                    imprimir.Rows.Add(dataGridView4.Rows[i].Cells[0].Value.ToString(), dataGridView4.Rows[i].Cells[1].Value.ToString(),
                                      dataGridView4.Rows[i].Cells[2].Value.ToString(), importe_tot, porcent);
                    i++;
                }

                Visor_informes inform = new Visor_informes();
                inform.envio_inf_dia(imprimir,tip_inf);
                inform.Show();
            }
		}
		
		void DataGridView1ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			estilo_dgv1();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			DateTime desde, hasta;
			String fecha="",dia_semana="";
			int i=0;
			decimal tot_importe=0,porcentaje=0,tot_casos=0,casos_sum=0,importe_sum=0,porcentaje_sum=0;
			
			desde=dateTimePicker1.Value;
			hasta=dateTimePicker2.Value;
			
			conex5.conectar("inventario");
			base_inv=conex5.consultar("SELECT count(idbase_inventario), sum(importe) FROM base_inventario WHERE idbase_inventario > 0 "+tipo_inf);
			if(base_inv.Rows.Count>0){
				tot_casos=Convert.ToDecimal(base_inv.Rows[0][0].ToString());
				tot_importe=Convert.ToDecimal(base_inv.Rows[0][1].ToString());
			}
			dataGridView4.Rows.Clear();
			while(desde<=hasta){
				fecha=desde.ToShortDateString();
				fecha=fecha.Substring(6,4)+"-"+fecha.Substring(3,2)+"-"+fecha.Substring(0,2);
				dia_semana=desde.DayOfWeek.ToString();
				
				switch(dia_semana){
						case "Sunday": dia_semana="Domingo";
						break;
						case "Monday": dia_semana="Lunes";
						break;
						case "Tuesday": dia_semana="Martes";
						break;
						case "Wednesday": dia_semana="Miércoles";
						break;
						case "Thursday": dia_semana="Jueves";
						break;
						case "Friday": dia_semana="Viernes";
						break;
						case "Saturday": dia_semana="Sábado";
						break;
				}
				
				dataGridView4.Rows.Add();
				dataGridView4.Rows[i].Cells[0].Value=dia_semana;
                dataGridView4.Rows[i].Cells[1].Value =desde.ToShortDateString();

				base_inv=conex5.consultar("SELECT count(idbase_inventario), sum(importe) FROM base_inventario where fecha_verificacion between \""+fecha+" 00:00\" and \""+fecha+" 23:59\" "+tipo_inf);
				if(base_inv.Rows.Count>0){
					dataGridView4.Rows[i].Cells[2].Value=Convert.ToDecimal(base_inv.Rows[0][0].ToString());
					if(base_inv.Rows[0][1].ToString().Length>0){
						dataGridView4.Rows[i].Cells[3].Value=Convert.ToDecimal(base_inv.Rows[0][1].ToString());
					}else{
						dataGridView4.Rows[i].Cells[3].Value=Convert.ToDecimal(0);
					}
					
					porcentaje=((Convert.ToDecimal(base_inv.Rows[0][0].ToString())*100)/tot_casos);
					dataGridView4.Rows[i].Cells[4].Value=porcentaje;
				}else{
					dataGridView4.Rows[i].Cells[2].Value=Convert.ToDecimal(0);
					dataGridView4.Rows[i].Cells[3].Value=Convert.ToDecimal(0);
					dataGridView4.Rows[i].Cells[4].Value=Convert.ToDecimal(0);
				}
				
				casos_sum=casos_sum+(Convert.ToDecimal(dataGridView4.Rows[i].Cells[2].Value.ToString()));
				importe_sum=importe_sum+(Convert.ToDecimal(dataGridView4.Rows[i].Cells[3].Value.ToString()));
				porcentaje_sum=porcentaje_sum+(Convert.ToDecimal(dataGridView4.Rows[i].Cells[4].Value.ToString()));
				desde=desde.AddDays(1);
				i++;
			}
			
			//REVISADOS
			dataGridView4.Rows.Add();
			dataGridView4.Rows[i].Cells[0].Value="REVISADOS";
            dataGridView4.Rows[i].Cells[1].Value = " ";
			dataGridView4.Rows[i].Cells[2].Value=casos_sum;
			dataGridView4.Rows[i].Cells[3].Value=importe_sum;
			dataGridView4.Rows[i].Cells[4].Value=porcentaje_sum;
			
			//FALTANTES
			dataGridView4.Rows.Add();
			dataGridView4.Rows[i+1].Cells[0].Value="FALTANTES";
            dataGridView4.Rows[i+1].Cells[1].Value = " ";
			dataGridView4.Rows[i+1].Cells[2].Value=(tot_casos-casos_sum);
			dataGridView4.Rows[i+1].Cells[3].Value=(tot_importe-importe_sum);
			dataGridView4.Rows[i+1].Cells[4].Value=Convert.ToDecimal(100-porcentaje_sum);
			
			//TOTAL
			dataGridView4.Rows.Add();
			dataGridView4.Rows[i+2].Cells[0].Value="TOTAL";
            dataGridView4.Rows[i+2].Cells[1].Value = " ";
			dataGridView4.Rows[i+2].Cells[2].Value=tot_casos;
			dataGridView4.Rows[i+2].Cells[3].Value=tot_importe;
			dataGridView4.Rows[i+2].Cells[4].Value=Convert.ToDecimal(100);
			
			estilo_dgv4();
		}
		//exportar excel
		private void button3_Click(object sender, EventArgs e)
		{
			String nombre_hoja="",tip_inf="";
			int i = 0;
			SaveFileDialog dialog_save = new SaveFileDialog();
			dialog_save.Filter = "Archivos de Excel (*.XLSX)|*.XLSX"; //le indicamos el tipo de filtro en este caso que busque solo los archivos excel
			dialog_save.Title = "Guardar Respaldo de la Base";//le damos un titulo a la ventana
			imprimir.Columns.Clear();
			imprimir.Rows.Clear();

			if (dialog_save.ShowDialog() == DialogResult.OK)
			{
				XLWorkbook wb = new XLWorkbook();
				
				if (tipo_info == 0)
				{
					tip_inf = "COP Y RCV";
				}

				if (tipo_info == 1)
				{
					tip_inf = "COP";
				}

				if (tipo_info == 2)
				{
					tip_inf = "RCV";
				}

				if (tipo_info == 3)
				{
					tip_inf = "MEX";
				}

				if (tabControl1.SelectedIndex < 4)//resumen general
				{

					
					while (i < dataGridView1.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView1.Columns[i].HeaderText);
						i++;
					}

					i = 0;

					while (i < dataGridView1.RowCount)
					{
						imprimir.Rows.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView1.Rows[i].Cells[2].Value.ToString(), dataGridView1.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView1.Rows[i].Cells[4].Value.ToString(), dataGridView1.Rows[i].Cells[5].Value.ToString(),
						                  dataGridView1.Rows[i].Cells[6].Value.ToString(), dataGridView1.Rows[i].Cells[7].Value.ToString(),
						                  dataGridView1.Rows[i].Cells[8].Value.ToString(), dataGridView1.Rows[i].Cells[9].Value.ToString(),
						                  dataGridView1.Rows[i].Cells[10].Value.ToString(),dataGridView1.Rows[i].Cells[11].Value.ToString());
						i++;
					}

					nombre_hoja = "Resumen_General_" + tip_inf;
					wb.Worksheets.Add(imprimir, nombre_hoja);
					imprimir.Columns.Clear();
					imprimir.Rows.Clear();
					i = 0;
					//}

					//if (tabControl1.SelectedIndex == 1)//Resumen_Por_Incidencia
					//{
					while (i < dataGridView2.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView2.Columns[i].HeaderText);
						i++;
					}

					i = 0;

					while (i < dataGridView2.RowCount)
					{
						imprimir.Rows.Add(dataGridView2.Rows[i].Cells[0].Value.ToString(), dataGridView2.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView2.Rows[i].Cells[2].Value.ToString(), dataGridView2.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView2.Rows[i].Cells[4].Value.ToString(), dataGridView2.Rows[i].Cells[5].Value.ToString(),
						                  dataGridView2.Rows[i].Cells[6].Value.ToString(), dataGridView2.Rows[i].Cells[7].Value.ToString());
						i++;
					}
					
					//}

					nombre_hoja = "Resumen_Por_Inc_" + tip_inf;
					wb.Worksheets.Add(imprimir, nombre_hoja);
					imprimir.Columns.Clear();
					imprimir.Rows.Clear();
					i = 0;

					//if (tabControl1.SelectedIndex == 2)//Resumen_Por_Tipo_de_Documento
					//{
					while (i < dataGridView3.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView3.Columns[i].HeaderText);
						i++;
					}

					i = 0;

					while (i < dataGridView3.RowCount)
					{
						imprimir.Rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString(), dataGridView3.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView3.Rows[i].Cells[2].Value.ToString(), dataGridView3.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView3.Rows[i].Cells[4].Value.ToString(), dataGridView3.Rows[i].Cells[5].Value.ToString(),
						                  dataGridView3.Rows[i].Cells[6].Value.ToString(), dataGridView3.Rows[i].Cells[7].Value.ToString());
						i++;
					}
					nombre_hoja = "Resumen_Por_TD_" + tip_inf;
					wb.Worksheets.Add(imprimir, nombre_hoja);
					imprimir.Columns.Clear();
					imprimir.Rows.Clear();
					i = 0;
					//}
					

					

					//if (tabControl1.SelectedIndex == 3)//Resumen_de_Avance_por_Dia
					// {
					while (i < dataGridView4.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView4.Columns[i].HeaderText);
						i++;
					}

					i = 0;

					while (i < dataGridView4.RowCount)
					{
						imprimir.Rows.Add(dataGridView4.Rows[i].Cells[0].Value.ToString(), dataGridView4.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView4.Rows[i].Cells[2].Value.ToString(), dataGridView4.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView4.Rows[i].Cells[4].Value.ToString());
						i++;
					}
					nombre_hoja = "Resumen_de_Avance_"+tip_inf;
					wb.Worksheets.Add(imprimir, nombre_hoja);
				}
				
				if(tabControl1.SelectedIndex == 6)//Sobrante
				{
					
					imprimir.Rows.Clear();
					imprimir.Columns.Clear();
					i = 0;
					
					while (i < dataGridView5.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView5.Columns[i].HeaderText.ToUpper());
						i++;
					}

					i = 0;

					while (i < dataGridView5.RowCount)
					{
						imprimir.Rows.Add(dataGridView5.Rows[i].Cells[0].Value.ToString(), dataGridView5.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView5.Rows[i].Cells[2].Value.ToString(), dataGridView5.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView5.Rows[i].Cells[4].Value.ToString(), dataGridView5.Rows[i].Cells[5].Value.ToString(),
						                  dataGridView5.Rows[i].Cells[6].Value.ToString());
						i++;
					}
					nombre_hoja = "Sobrante";
					wb.Worksheets.Add(imprimir, nombre_hoja);
					
				}
				
				if(tabControl1.SelectedIndex == 4)//Inventario
				{
					
					imprimir.Rows.Clear();
					imprimir.Columns.Clear();
					i = 0;
					
					while (i < dataGridView6.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView6.Columns[i].HeaderText.ToUpper());
						i++;
					}

					i = 0;

					while (i < dataGridView6.RowCount)
					{
						imprimir.Rows.Add(dataGridView6.Rows[i].Cells[0].Value.ToString(), dataGridView6.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[2].Value.ToString(), dataGridView6.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[4].Value.ToString(), dataGridView6.Rows[i].Cells[5].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[6].Value.ToString(), dataGridView6.Rows[i].Cells[7].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[8].Value.ToString(), dataGridView6.Rows[i].Cells[9].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[10].Value.ToString(), dataGridView6.Rows[i].Cells[11].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[12].Value.ToString(), dataGridView6.Rows[i].Cells[13].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[14].Value.ToString(), dataGridView6.Rows[i].Cells[15].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[16].Value.ToString(), dataGridView6.Rows[i].Cells[17].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[18].Value.ToString(), dataGridView6.Rows[i].Cells[19].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[20].Value.ToString(), dataGridView6.Rows[i].Cells[21].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[22].Value.ToString(), dataGridView6.Rows[i].Cells[23].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[24].Value.ToString(), dataGridView6.Rows[i].Cells[25].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[26].Value.ToString(), dataGridView6.Rows[i].Cells[27].Value.ToString(),
						                  dataGridView6.Rows[i].Cells[28].Value.ToString());
						i++;
					}
					nombre_hoja = "Inventariado";
					wb.Worksheets.Add(imprimir, nombre_hoja);
				}
				
				if(tabControl1.SelectedIndex == 5)//Faltantes
				{
					
					imprimir.Rows.Clear();
					imprimir.Columns.Clear();
					i = 0;
					
					while (i < dataGridView8.ColumnCount)
					{
						imprimir.Columns.Add(dataGridView8.Columns[i].HeaderText.ToUpper());
						i++;
					}

					i = 0;

					while (i < dataGridView8.RowCount)
					{
						imprimir.Rows.Add(dataGridView8.Rows[i].Cells[0].Value.ToString(), dataGridView8.Rows[i].Cells[1].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[2].Value.ToString(), dataGridView8.Rows[i].Cells[3].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[4].Value.ToString(), dataGridView8.Rows[i].Cells[5].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[6].Value.ToString(), dataGridView8.Rows[i].Cells[7].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[8].Value.ToString(), dataGridView8.Rows[i].Cells[9].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[10].Value.ToString(), dataGridView8.Rows[i].Cells[11].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[12].Value.ToString(), dataGridView8.Rows[i].Cells[13].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[14].Value.ToString(), dataGridView8.Rows[i].Cells[15].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[16].Value.ToString(), dataGridView8.Rows[i].Cells[17].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[18].Value.ToString(), dataGridView8.Rows[i].Cells[19].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[20].Value.ToString(), dataGridView8.Rows[i].Cells[21].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[22].Value.ToString(), dataGridView8.Rows[i].Cells[23].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[24].Value.ToString(), dataGridView8.Rows[i].Cells[25].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[26].Value.ToString(), dataGridView8.Rows[i].Cells[27].Value.ToString(),
						                  dataGridView8.Rows[i].Cells[28].Value.ToString());
						i++;
					}
					nombre_hoja = "Por Inventariar";
					wb.Worksheets.Add(imprimir, nombre_hoja);
				}
				
				if(tabControl1.SelectedIndex <=6)//Si no son Correcciones
				{
					wb.SaveAs(@""+dialog_save.FileName);
					MessageBox.Show("Archivo guardado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				

			}
		}
		
		void Button4Click(object sender, EventArgs e)//actualizar
		{
			if(comboBox1.SelectedIndex==0){
				tipo_inf="";
                tipo_info = 0;
			}
			
			if(comboBox1.SelectedIndex==1){
				tipo_inf=" AND clase_doc=\"COP\"";
                tipo_info = 1;
			}
			
			if(comboBox1.SelectedIndex==2){
				tipo_inf=" AND clase_doc=\"RCV\"";
                tipo_info = 2;
			}

            if (comboBox1.SelectedIndex == 3)
            {
                tipo_inf = " AND tipo_inv=\"MEX\"";
                tipo_info = 3;
            }
			
			llena_resumen_gral();
			estilo_dgv1();
			
			llena_resumen_inc();
			estilo_dgv2();
			
			llena_resumen_td();
			estilo_dgv3();
			
			dataGridView4.Rows.Clear();
		}

        private void button5_Click(object sender, EventArgs e)
        {
            conex6.conectar("inventario");
            sobrante = conex6.consultar("SELECT idsobrante as \"ID\",reg_pat as \"REGISTRO PATRONAL\", credito as \"CREDITO\",periodo as \"PERIODO\",libro as \"LIBRO\",responsable as \"RESPONSABLE\",fecha_sobrante as \"FECHA SOBRANTE\" FROM sobrante");
            dataGridView5.DataSource = sobrante;
            label10.Text = "Total:\n" + sobrante.Rows.Count;
            label10.Refresh();
            
            try
            {
            	dataGridView5.Columns[0].Width = 60;
                dataGridView5.Columns[3].Width = 80;
                dataGridView5.Columns[4].Width = 50;
                dataGridView5.Columns[5].Width = 300;
            }catch{
            }
        }
		
		void Button6Click(object sender, EventArgs e)
		{
			buscar_sobrante();
		}
		
		void TabPage5Click(object sender, EventArgs e)
		{
			
		}
		
		void Label10Click(object sender, EventArgs e)
		{
			
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			temp_i=0;
			temp_j=0;
		}
		
		void TextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				buscar_sobrante();
			}
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			String desde="", hasta="";
			conex7.conectar("inventario");
			
			desde = dateTimePicker4.Value.ToShortDateString();
			hasta = dateTimePicker3.Value.ToShortDateString();
			
			desde= desde.Substring(6,4)+"-"+desde.Substring(3,2)+"-"+desde.Substring(0,2);
			hasta= hasta.Substring(6,4)+"-"+hasta.Substring(3,2)+"-"+hasta.Substring(0,2);
			
            listado = conex7.consultar("SELECT * FROM base_inventario WHERE fecha_verificacion BETWEEN \""+desde+" 00:00\" AND \""+hasta+" 23:59\" ");
            dataGridView6.DataSource = listado;
            label14.Text = "Total:\n" + listado.Rows.Count;
            label14.Refresh();
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			temp2_i=0;
			temp2_j=0;
		}
		
		void TextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				buscar_invent();
			}
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			buscar_invent();
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			busca_correciones();
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			String sql="",coinci="",observ="",reg_pat="",cred="",period="",import="",tdo="",reg="",cad_mod="",user="",obs_prev="";
			int modif=0,tdd=0;
			decimal imp=0;
			if(dataGridView7.RowCount>0){
				user = MainForm.datos_user_static[9]+" "+MainForm.datos_user_static[4];
				if(comboBox2.SelectedIndex < 3){
					if(comboBox2.SelectedIndex>-1){
						coinci=" coincidente=\""+comboBox2.SelectedItem.ToString()+"\",";
						modif=1;
						cad_mod="•COINCIDENTE: "+comboBox2.SelectedItem.ToString()+"\n";
					}
					
					if(textBox3.Text.Length>5){
						observ=" observaciones=\""+textBox3.Text+" | MODIFICADO POR: "+user+" EL DIA: "+DateTime.Today.ToShortDateString()+"\",";
						modif=1;
						cad_mod+="•OBSERVACIONES: "+textBox3.Text+"\n";
					}
					
					if(maskedTextBox3.MaskCompleted==true){
						reg=maskedTextBox3.Text;
						reg=reg.Substring(0,3)+"-"+reg.Substring(6,5)+"-"+reg.Substring(14,2);
						reg=reg.ToUpper();
						reg_pat=" doc_reg_pat=\""+reg+"\",";
						cad_mod+="•REGISTRO PATRONAL: "+reg+"\n";
						modif=1;
					}
					
					if(maskedTextBox4.MaskCompleted==true){
						cred=" doc_credito=\""+maskedTextBox4.Text+"\",";
						cad_mod+="•CREDITO: "+maskedTextBox4.Text+"\n";
						modif=1;
					}
					
					if(maskedTextBox5.MaskCompleted==true){
						period=" doc_periodo=\""+maskedTextBox5.Text+"\",";
						cad_mod+="•PERIODO: "+maskedTextBox5.Text+"\n";
						modif=1;
					}
					
					if(textBox4.Text.Length>3){
						if(decimal.TryParse(textBox4.Text,out imp)==true ){
							import=" doc_importe=\""+textBox4.Text+"\",";
							modif=1;
							cad_mod+="•IMPORTE: "+textBox4.Text+"\n";
						}
						
					}
					
					if(textBox5.Text.Length>0){
						if(int.TryParse(textBox5.Text,out tdd)==true){
							tdo=" doc_td=\""+textBox5.Text+"\",";
							modif=1;
							cad_mod+="•TIPO DOCUMENTO: "+textBox5.Text+"\n";
						}
						
					}
					
					if(modif==1){
						DialogResult res =MessageBox.Show("Se va a Modificar este Registro:\n"+dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[1].Value.ToString()+" "+dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[2].Value.ToString()+" "+dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[3].Value.ToString()+"/"+dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[4].Value.ToString()+"\n"+
						                                  "Con la siguiente informacion:\n"+cad_mod+"\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
						if(res==DialogResult.Yes){
							conex9.conectar("inventario");
							
							obs_prev=dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[8].Value.ToString();
							if(observ.Length==0){
								observ="observaciones=\""+obs_prev+"| MODIFICADO POR: "+user+" EL DIA: "+DateTime.Today.ToShortDateString()+"\",";
							}
							sql="UPDATE base_inventario SET "+coinci+observ+reg_pat+cred+period+import+tdo;
							sql=sql.Substring(0,sql.Length-1);
							sql=sql+" WHERE idbase_inventario= "+dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[0].Value.ToString();
							conex.consultar(sql);
							MessageBox.Show("¡Registro Actualizado Exitosamente!","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
							maskedTextBox1.Clear();
							maskedTextBox2.Clear();
							maskedTextBox3.Clear();
							maskedTextBox4.Clear();
							maskedTextBox5.Clear();
							comboBox2.SelectedIndex=-1;
							textBox3.Text="";
							textBox4.Text="";
							textBox5.Text="";
							dataGridView7.DataSource=null;
							maskedTextBox1.Focus();
							//dataGridView7.Rows.Clear();
						}
					}
				}else{
					DialogResult res =MessageBox.Show("Se va a BORRAR la Marca y datos colocados en este Registro\n¿Desea Continuar?\n","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
					if(res==DialogResult.Yes){
						conex9.conectar("inventario");
						sql="UPDATE base_inventario SET coincidente=\"-\",observaciones=\"-\",responsable=\"-\",fecha_verificacion=null,auxiliar=\"-\",doc_reg_pat=\"-\",doc_credito=\"-\",doc_periodo=\"-\",doc_importe=\"-\",doc_td=\"-\" WHERE idbase_inventario= "+dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[0].Value.ToString()+"";
						conex.consultar(sql);
						MessageBox.Show("¡Registro Actualizado Exitosamente!","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
						maskedTextBox1.Clear();
						maskedTextBox2.Clear();
						maskedTextBox3.Clear();
						maskedTextBox4.Clear();
						maskedTextBox5.Clear();
						comboBox2.SelectedIndex=-1;
						textBox3.Text="";
						textBox4.Text="";
						textBox5.Text="";
						dataGridView7.DataSource=null;
						maskedTextBox1.Focus();
					}
				}
			}
		}
		
		void MaskedTextBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				if(maskedTextBox2.MaskCompleted==true){
					busca_correciones();
				}
			}
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			String desd="",hast="",fech_imp="",fech_imp2="";
			
			if(dateTimePicker5.Value==dateTimePicker6.Value){
				hast=dateTimePicker6.Text;
				fech_imp=hast;
				hast=hast.Substring(6,4)+"-"+hast.Substring(3,2)+"-"+hast.Substring(0,2);
				fech_rep="AND fecha_verificacion BETWEEN  \""+hast+" 00:00\" AND \""+hast+" 23:59\"";
				llena_resumen_gral();
				estilo_dgv1();
				
				llena_resumen_inc();
				estilo_dgv2();
				
				llena_resumen_td();
				estilo_dgv3();
				
				fech_impre="del día "+fech_imp;
			}else{
				if(dateTimePicker5.Value<dateTimePicker6.Value){
					desd=dateTimePicker5.Text;
					fech_imp=desd;
					desd=desd.Substring(6,4)+"-"+desd.Substring(3,2)+"-"+desd.Substring(0,2);
					hast=dateTimePicker6.Text;
					fech_imp2=hast;
					hast=hast.Substring(6,4)+"-"+hast.Substring(3,2)+"-"+hast.Substring(0,2);
					
					fech_rep="AND fecha_verificacion BETWEEN  \""+desd+" 00:00\" AND \""+hast+" 23:59\"";
					llena_resumen_gral();
					estilo_dgv1();
					
					llena_resumen_inc();
					estilo_dgv2();
					
					llena_resumen_td();
					estilo_dgv3();
					
					fech_impre="del día "+fech_imp+" al día "+fech_imp2;
				}else{
					MessageBox.Show("La Fecha Final NO puede ser anterior al a Fecha Inicial","AVISO");
					
				}
				
			}
			
			
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			maskedTextBox1.Clear();
			maskedTextBox2.Clear();
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			conex10.conectar("inventario");
			faltante = conex10.consultar("SELECT * FROM base_inventario WHERE coincidente=\"-\" ");
            dataGridView8.DataSource = faltante;
            label28.Text = "Total:\n" + faltante.Rows.Count;
            label28.Refresh();
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			buscar_faltantes();
		}
		
		void TextBox6KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.Equals(Convert.ToChar(Keys.Enter))==true){
				buscar_faltantes();
			}
		}
	}
}

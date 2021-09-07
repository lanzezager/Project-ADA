using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear
{
    public partial class Visor_consulta_patron : Form
    {
        public Visor_consulta_patron()
        {
            InitializeComponent();
        }

        Depuracion.DataSet_Depu depu = new Depuracion.DataSet_Depu();
        DataTable tabla = new DataTable();

        Conexion conex = new Conexion();

        Depuracion.Formatos_Reporte.Formato_impresion_cons_pat consulta = new Depuracion.Formatos_Reporte.Formato_impresion_cons_pat();
        string[] datos_imprimir;

        public void recibir_valores(string[] datos_patron)
        {
            datos_imprimir = datos_patron;
        }

        public void mostrar()
        {
        	try{
        		datos_imprimir[8]=datos_imprimir[8].Substring(0,10);
        	}catch(Exception a){
        		
        	}
        	
        	try{
        		datos_imprimir[9]=datos_imprimir[9].Substring(0,10);
        	}catch(Exception a){
        		
        	}
        	
        	try{
        		datos_imprimir[10]=datos_imprimir[10].Substring(0,10);
        	}catch(Exception a){
        		
        	}
        	
        	try{
        		datos_imprimir[11]=datos_imprimir[11].Substring(0,10);
        	}catch(Exception a){
        		
        	}

            consulta.SetParameterValue("reg_pat", datos_imprimir[0]);
            consulta.SetParameterValue("credito_cuota", datos_imprimir[1]);
            consulta.SetParameterValue("credito_multa", datos_imprimir[2]);
            consulta.SetParameterValue("razon_social", datos_imprimir[3]);
            consulta.SetParameterValue("periodo", datos_imprimir[4]);
            consulta.SetParameterValue("sector", datos_imprimir[5]);
            consulta.SetParameterValue("notificador", datos_imprimir[6]);
            consulta.SetParameterValue("controlador", datos_imprimir[7]);
            consulta.SetParameterValue("fecha_entrega", datos_imprimir[8]);
            consulta.SetParameterValue("fecha_recepcion", datos_imprimir[9]);
            consulta.SetParameterValue("fecha_notificacion", datos_imprimir[10]);
            consulta.SetParameterValue("fecha_cartera", datos_imprimir[11]);
            consulta.SetParameterValue("estatus_doc", datos_imprimir[12]);
            consulta.SetParameterValue("importe_cuota", datos_imprimir[13]);
            consulta.SetParameterValue("importe_multa", datos_imprimir[14]);
            consulta.SetParameterValue("tipo_doc", datos_imprimir[15]);
            consulta.SetParameterValue("situacion_patron", datos_imprimir[16]);
            consulta.SetParameterValue("observacion", datos_imprimir[17]);
            consulta.SetParameterValue("domicilio", datos_imprimir[18]);
            consulta.SetParameterValue("localidad", datos_imprimir[19]);
            consulta.SetParameterValue("delegacion", conex.leer_config_sub()[0].ToUpper());
            consulta.SetParameterValue("subdelegacion", conex.leer_config_sub()[3].ToUpper());

            
            crystalReportViewer1.ReportSource = consulta;
        }

        private void Visor_consulta_patron_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            mostrar();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

            String window_name = this.Text;
            window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;

        }
    }
}

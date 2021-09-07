using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nova_Gear.Registros
{
    public partial class Visor_Formato_Registro : Form
    {
        public Visor_Formato_Registro()
        {
            InitializeComponent();
        }

        Depuracion.Formatos_Reporte.Formato_Registro_Usuario reg_user = new Depuracion.Formatos_Reporte.Formato_Registro_Usuario();
        string[] datos_user;
        Conexion conex = new Conexion();

        public void recibir_datos(string[] info_user)
        {
            this.datos_user = info_user;
        }

        public void mostrar()
        {
            /*
 			nombre,0
			apellido,1
			doc_ide,2
			num_ide,3
			edo_civil,4
			fech_nac,5
			domicilio,6
			e-mail,7
			celular,8
			escolaridad,9
			profesion,10
			categoria,11
			mesa,12
			area,13
			puesto,14
			tipo_trab,15
			contrato_ini,16
			contrato_fin,17
			nss_trab,18
			num_mat,19
			unidad,20
			nom_usuario,21
			contrasena,22
			url_imagen,23
			estatus,24
			rango,25
			 */
            String ruta = conex.leer_config();

            ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';') - 1) - (ruta.IndexOf('='))));
            ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";

            reg_user.SetParameterValue("foto", ruta + datos_user[23]);
            reg_user.SetParameterValue("delegacion", conex.leer_config_sub()[0].ToUpper());
            reg_user.SetParameterValue("subdelegacion", conex.leer_config_sub()[3].ToUpper());
            reg_user.SetParameterValue("nombre", datos_user[0]);
            reg_user.SetParameterValue("apellido", datos_user[1]);
            reg_user.SetParameterValue("doc_ide", datos_user[2]);
            reg_user.SetParameterValue("num_doc_ide", datos_user[3]);
            reg_user.SetParameterValue("edo_civil", datos_user[4]);
            reg_user.SetParameterValue("fech_nac", datos_user[5]);
            reg_user.SetParameterValue("domicilio", datos_user[6]);
            reg_user.SetParameterValue("email", datos_user[7]);
            reg_user.SetParameterValue("celular", datos_user[8]);
            reg_user.SetParameterValue("escolaridad", datos_user[9]);
            reg_user.SetParameterValue("profesion", datos_user[10]);
            reg_user.SetParameterValue("categoria", datos_user[11]);
            reg_user.SetParameterValue("cargo_mesa", datos_user[12]);
            reg_user.SetParameterValue("depto", datos_user[13]);
            reg_user.SetParameterValue("actividad_nova", datos_user[14]);
            reg_user.SetParameterValue("tipo_trab", datos_user[15]);
            if (datos_user[15].Equals("Operativo TTD"))
            {
                reg_user.SetParameterValue("fech_cont_ini", datos_user[16]);
                reg_user.SetParameterValue("fech_cont_fin", datos_user[17]);
            }else{
                reg_user.SetParameterValue("fech_cont_ini", "N/A");
                reg_user.SetParameterValue("fech_cont_fin", "N/A");
            }
            reg_user.SetParameterValue("nss", datos_user[18]);
            reg_user.SetParameterValue("matricula", datos_user[19]);
            reg_user.SetParameterValue("nom_usuario", datos_user[21]);
            
            //reg_user.SetParameterValue("autoriza", conex.leer_config_sub()[8]);

            crystalReportViewer1.ReportSource = reg_user;
        }

        private void Visor_Formato_Registro_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            mostrar();
            MessageBox.Show("Para poder ingresar al Sistema imprima este Documento y Acuda con su Jefe Inmediato para que el usuario que acaba de registrar sea Activado","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }
    }
}

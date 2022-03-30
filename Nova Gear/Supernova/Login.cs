using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace Nova_Gear.Supernova
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //Conexion MySQL
        Super_conexion conex = new Super_conexion();
        DataTable tabla = new DataTable();
        DataTable tabla1 = new DataTable();

        int error_fatal = 0;

        public void entrar()
        {
            String sql;
            sql = "SELECT nombre_usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena FROM usuarios WHERE nombre_usuario=\"lanzezager\" AND idusuarios=1";
            tabla = conex.consultar(sql);

            if (tabla.Rows.Count > 0)// si existe user lanze
            {
                if ((tabla.Rows[0][1].ToString()).Equals("Rojinegro 100% Novatlas"))// si la contraseña es valida
                {
                    sql = "SELECT nombre,puesto,nivel,idusuarios,lugar_trabajo,lugar_trabajo_num FROM usuarios WHERE nombre_usuario = \"" + textBox1.Text + "\" AND contrasena = AES_ENCRYPT(\"" + textBox2.Text + "\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\")";
                    tabla1 = conex.consultar(sql);

                    if (tabla1.Rows.Count > 0)
                    {
                        Menu_supernova menu_super = new Menu_supernova(tabla1);
                        menu_super.Show();
                        menu_super.Focus();
                        this.Hide();
                    }
                    else
                    {
                        pictureBox1.Visible = true;
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox1.Focus();
                    }
                }
                else
                {
                    error_fatal = 1;
                }
            }
            else
            {
                error_fatal = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            entrar();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            String bd_user, bd_pass, bd_server, bd_base;
            StreamReader rdr = new StreamReader(@"mysql_config.lz");
                bd_user = rdr.ReadLine();
                bd_pass = rdr.ReadLine();
                bd_server = rdr.ReadLine();
                bd_base = rdr.ReadLine();
                rdr.Close();

                bd_user = bd_user.Substring(5, bd_user.Length - 5);
                bd_pass = bd_pass.Substring(9, bd_pass.Length - 9);
                bd_server = bd_server.Substring(7, bd_server.Length - 7);
                bd_base = bd_base.Substring(9, bd_base.Length - 9);

                conex.conectar("supernova", bd_server, bd_user, bd_pass);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                entrar();
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("Está a punto de salir de Supervisión Nova\n¿Desea Continuar?", "ATENCIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (respuesta == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("close_ng.exe");
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}

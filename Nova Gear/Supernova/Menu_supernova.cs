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
    public partial class Menu_supernova : Form
    {
        public Menu_supernova(DataTable info_user)
        {
            InitializeComponent();
            this.usuario = info_user;
        }

        DataTable usuario = new DataTable();

        private void Menu_supernova_Load(object sender, EventArgs e)
        {
            label2.Text = usuario.Rows[0][0].ToString()+"\n--"+usuario.Rows[0][1].ToString()+"--";

            if(Convert.ToInt32(usuario.Rows[0][2].ToString())==0){
                button2.Visible = true;
                button3.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Supernova_usuarios users = new Supernova_usuarios(usuario);
            users.ShowDialog();
            users.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Supernova_conexiones conexiones = new Supernova_conexiones();
            conexiones.ShowDialog();
            conexiones.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Consulta super_consulta = new Consulta(usuario);
            super_consulta.ShowDialog();
            super_consulta.Focus();

        }

        private void Menu_supernova_FormClosing(object sender, FormClosingEventArgs e)
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

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
    public partial class Supernova_usuarios : Form
    {
        public Supernova_usuarios(DataTable user)
        {
            InitializeComponent();
            this.usuario_actual = user;
        }

        Super_conexion conex = new Super_conexion();
        DataTable usuario_actual = new DataTable();

        int fila_sel = -1;

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[0, e.RowIndex];
                    fila_sel = e.RowIndex;
                }
            }
        }

        private void Supernova_usuarios_Load(object sender, EventArgs e)
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
            dataGridView1.DataSource = conex.consultar("SELECT idusuarios as ID, nombre as Nombre , puesto as Puesto, nombre_usuario as Usuario FROM usuarios");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Supernova_add_users agregar_users = new Supernova_add_users(1,0);
            agregar_users.ShowDialog();
            agregar_users.Focus();
            //dataGridView1.Rows.Clear();
            dataGridView1.DataSource = conex.consultar("SELECT idusuarios as ID, nombre as Nombre , puesto as Puesto, nombre_usuario as Usuario FROM usuarios");
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id_u = 0;

            id_u = Convert.ToInt32(dataGridView1[0, fila_sel].Value.ToString());

            if (id_u != (Convert.ToInt32(usuario_actual.Rows[0][3].ToString())))
            {
                Supernova_add_users agregar_users = new Supernova_add_users(2, id_u);
                agregar_users.ShowDialog();
                agregar_users.Focus();
                //dataGridView1.Rows.Clear();
                dataGridView1.DataSource = conex.consultar("SELECT idusuarios as ID, nombre as Nombre , puesto as Puesto, nombre_usuario as Usuario FROM usuarios");
            }
            else
            {
                MessageBox.Show("No puedes modificar tu propio Usuario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String nombre = dataGridView1[1,fila_sel].Value.ToString();
            DialogResult resul = MessageBox.Show("¡ATENCION! Esta Acción no podrá deshacerse.\nSe eliminará al usuario:\n"+nombre+"\n¿Está Seguro de querer continuar?" +
                                        "", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (resul == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dataGridView1[0,fila_sel].Value.ToString());
                
                if(id != 1){
                    if (id != (Convert.ToInt32(usuario_actual.Rows[0][3].ToString())))
                    {
                        conex.consultar("DELETE FROM usuarios WHERE idusuarios=" + id);
                        //dataGridView1.Rows.Clear();
                        dataGridView1.DataSource = conex.consultar("SELECT idusuarios as ID, nombre as Nombre , puesto as Puesto, nombre_usuario as Usuario FROM usuarios");
                    }
                    else
                    {
                        MessageBox.Show("No puedes eliminar tu propio Usuario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}

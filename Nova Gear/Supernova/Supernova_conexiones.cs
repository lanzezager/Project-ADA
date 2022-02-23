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
    public partial class Supernova_conexiones : Form
    {
        public Supernova_conexiones()
        {
            InitializeComponent();
        }

        Super_conexion conex = new Super_conexion();
        int ind_fila = -1;

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[0, e.RowIndex];
                    ind_fila = e.RowIndex;
                }
            }
        }

        private void Supernova_conexiones_Load(object sender, EventArgs e)
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
            dataGridView1.DataSource = conex.consultar("SELECT idconexiones as ID, del_num as Num_Del, sub_num as Num_Sub, del_nom as Delegacion, sub_nom as Subdelegacion, estado as Estado, municipio as Municipio FROM conexiones");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Supernova_add_conex agregar_conex = new Supernova_add_conex(1,0);
            agregar_conex.ShowDialog();
            agregar_conex.Focus();
            dataGridView1.DataSource = conex.consultar("SELECT idconexiones as ID, del_num as Num_Del, sub_num as Num_Sub, del_nom as Delegacion, sub_nom as Subdelegacion, estado as Estado, municipio as Municipio FROM conexiones");
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id_con = 0;

            id_con = Convert.ToInt32(dataGridView1[0, ind_fila].Value.ToString());
            Supernova_add_conex agregar_conex = new Supernova_add_conex(2, id_con);
            agregar_conex.ShowDialog();
            agregar_conex.Focus();
            dataGridView1.DataSource = conex.consultar("SELECT idconexiones as ID, del_num as Num_Del, sub_num as Num_Sub, del_nom as Delegacion, sub_nom as Subdelegacion, estado as Estado, municipio as Municipio FROM conexiones");
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String nombre = dataGridView1[4, ind_fila].Value.ToString();
            DialogResult resul = MessageBox.Show("¡ATENCION! Esta Acción no podrá deshacerse.\nSe eliminará la conexión :\n" + nombre + "\n¿Está Seguro de querer continuar?" +
                                        "", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (resul == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dataGridView1[0, ind_fila].Value.ToString());

                conex.consultar("DELETE FROM conexiones WHERE idconexiones=" + id);
                //dataGridView1.Rows.Clear();
                dataGridView1.DataSource = conex.consultar("SELECT idconexiones as ID, del_num as Num_Del, sub_num as Num_Sub, del_nom as Delegacion, sub_nom as Subdelegacion, estado as Estado, municipio as Municipio FROM conexiones");
           
            }
        }
    }
}

        

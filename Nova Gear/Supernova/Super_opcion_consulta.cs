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
    public partial class Super_opcion_consulta : Form
    {
        public Super_opcion_consulta(String t, String nn, String v)
        {
            InitializeComponent();
            this.tipo = t;
            this.nom_num = nn;
            this.valor = v;
        }

        Super_conexion base_conex = new Super_conexion();
        DataTable consulta = new DataTable();

        String tipo = "", nom_num = "", valor = "";

        private void Super_opcion_consulta_Load(object sender, EventArgs e)
        {
            String bd_user, bd_pass, bd_server, bd_base,tip="";
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
            base_conex.conectar("supernova", bd_server, bd_user, bd_pass);

            //MessageBox.Show(tipo+","+nom_num+","+valor);

            if (tipo == "Delegación"){
                if (nom_num == "Número"){
                   tip = "del_num";
                   valor = "" + valor + "";
                }else{
                   tip = "del_nom";
                   valor = "\"" + valor + "\"";
                }
            }         
           
            if (tipo == "Estado")
            {
                tip = "estado";
                valor = "\"" + valor + "\"";
            }

            if (tipo == "Municipio")
            {
                tip = "municipio";
                valor = "\"" + valor + "\"";
            }            

            consulta = base_conex.consultar("SELECT idconexiones as \"ID\",sub_nom as \"SUB_NOM\",sub_num as \"SUB_NUM\",del_nom as \"DEL_NOM\",del_num as \"DEL_NUM\",estado as \"ESTADO\",municipio as \"MUNICIPIO\" FROM conexiones WHERE "+tip+"="+valor);
            dataGridView1.DataSource = consulta;

            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;

            int cont = 0;
            while (cont < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[cont].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                dataGridView1.Rows[cont].Cells[0].Value = false;
                cont++;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {    
            if(dataGridView1.RowCount>0){
                for (int i = 0; i < dataGridView1.RowCount;i++ )
                {
                    int cont = 1;
                    dataGridView1[0, i].Value = true;

                    while (cont <= 7)
                    {
                        dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                        cont++;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    int cont = 1;
                    dataGridView1[0, i].Value = false;

                    while (cont <= 7)
                    {
                        dataGridView1.Rows[i].Cells[cont].Style.BackColor = System.Drawing.Color.White;
                        cont++;
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int cont = 1;

            if (dataGridView1.RowCount > 0)
            {
                if (dataGridView1[0, e.RowIndex].Value != null)
                {
                    if (Convert.ToBoolean(dataGridView1[0, e.RowIndex].Value.ToString()) == false)
                    {
                        while (cont <= 7)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[cont].Style.BackColor = System.Drawing.Color.White;
                            cont++;
                        }
                    }

                    if (Convert.ToBoolean(dataGridView1[0, e.RowIndex].Value.ToString()) == true)
                    {
                        while (cont <= 7)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[cont].Style.BackColor = System.Drawing.Color.LightSkyBlue;
                            cont++;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                int tot = 0;

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView1[0,i].Value.ToString()) == true)
                    {
                        tot++;
                        Consulta.subs_con+= dataGridView1[1,i].Value.ToString()+",";
                    }
                }

                if(tot>0){

                    DialogResult resul = MessageBox.Show("Se consultarán "+tot+" Subdelegaciones\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    if (resul == DialogResult.Yes)
                    {
                        Consulta.subs_con = Consulta.subs_con.Substring(0, Consulta.subs_con.Length - 1);
                        this.Close();
                    }
                    else
                    {
                        Consulta.subs_con = "";
                    }
                }
            }
        }
    }
}

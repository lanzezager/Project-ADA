using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Linq;

namespace Nova_Gear
{
    public partial class Reportes_cartera_Nvo : Form
    {
        public Reportes_cartera_Nvo()
        {
            InitializeComponent();
        }

        Conexion conex = new Conexion();
        Conexion conex2 = new Conexion();
        Conexion conex3 = new Conexion();
        Conexion conex4 = new Conexion();
       
        DataTable consulta = new DataTable();
        DataTable consultamysql = new DataTable();
        DataTable packs = new DataTable();
        DataTable packs_dfactura = new DataTable();

        int opo = 0, veri_acti = 1;
        String id_us = "";

        public void llenar_Cb2()
        {
            conex4.conectar("base_principal");
            comboBox2.Items.Clear();
            int i = 0;
            packs = conex4.consultar("SELECT DISTINCT(paquete) FROM estrados ORDER BY paquete DESC");
            comboBox2.Items.Add("NINGUNO");

            while (i < packs.Rows.Count)
            {
                if (packs.Rows[i][0].ToString() != "0")
                {
                    //comboBox2.Items.Add("TODOS");
                    if (veri_acti == 1)
                    {
                        if (verificar_paquetes(packs.Rows[i][0].ToString()) == true)
                        {
                            comboBox2.Items.Add(packs.Rows[i][0].ToString());
                        }
                    }
                    else
                    {
                        comboBox2.Items.Add(packs.Rows[i][0].ToString());
                    }
                }
                else
                {
                    
                }

                i++;
            }
            i = 0;
            conex4.cerrar();
        }

        public bool verificar_paquetes(string nom_pack)
        {
            Conexion conex5 = new Conexion();
            Conexion conex6= new Conexion();
            DataTable verifi_pack = new DataTable();
            DataTable verifi_pack2 = new DataTable();
            
            int tot=0;

            conex5.conectar("base_principal");
            conex6.conectar("base_principal"); 
            verifi_pack=conex5.consultar("SELECT id_credito FROM estrados WHERE paquete="+nom_pack+"");

            for (int i = 0; i < verifi_pack.Rows.Count;i++ )
            {
                verifi_pack2 = conex6.consultar("SELECT id FROM datos_factura WHERE id= "+verifi_pack.Rows[i][0].ToString()+" and status=\"NOTIFICADO\"");
                if(verifi_pack2.Rows.Count>0){
                    tot++;
                }
            }
            
            conex5.cerrar();
            conex6.cerrar();

            if(tot>0){
                return true;
            }else{
                return false;
            }
        }

        public DataTable copiar_datagrid()
        {
            DataTable tabla_destino = new DataTable();

            for (int j = 0; j < dataGridView1.ColumnCount; j++)
            {
                tabla_destino.Columns.Add(dataGridView1.Columns[j].HeaderText);
            }

            for (int j = 0; j < dataGridView1.RowCount; j++)
            {
                DataRow fila_copia = tabla_destino.NewRow();
                for (int k = 0; k < dataGridView1.ColumnCount; k++)
                {
                    fila_copia[k] = dataGridView1.Rows[j].Cells[k].Value.ToString();
                }

                tabla_destino.Rows.Add(fila_copia);
            }

            return tabla_destino;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox1.MaskCompleted == true)
            {
                maskedTextBox2.Focus();
            }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox2.MaskCompleted == true)
            {
                maskedTextBox3.Focus();
            }
        }

        private void maskedTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox3.MaskCompleted == true)
            {
                button2.Focus();
            }
        }
        //añadir
        private void button2_Click(object sender, EventArgs e)
        {
            String sql, rp = " ", per = " ", cred = " ", tipo = " ";
            int coin = 0;

            if (maskedTextBox1.MaskCompleted == true)
            {
                rp = maskedTextBox1.Text;
                rp = rp.Substring(0, 3) + rp.Substring(4, 5) + rp.Substring(10, 2);
                rp = " AND registro_patronal2=\"" + rp + "\" ";
            }

            if (maskedTextBox2.MaskCompleted == true)
            {
                cred = maskedTextBox2.Text;
                cred = " AND credito_cuotas=\"" + cred + "\" ";
            }

            if (maskedTextBox3.MaskCompleted == true)
            {
                per = maskedTextBox3.Text;
                per = per.Substring(0, 4) + per.Substring(5, 2);
                per = " AND periodo=\"" + per + "\" ";
            }


            if (comboBox1.SelectedIndex == 0)//NOTIFICADO
            {
                tipo = " nombre_periodo NOT LIKE \"CLEM%\" AND status = \"NOTIFICADO\" AND nn <> \"NN\"  ";
            }

            if (comboBox1.SelectedIndex == 1)//NO NOTIFICADO
            {
                tipo = " nombre_periodo NOT LIKE \"CLEM%\" AND (status <> \"CARTERA\" AND status NOT LIKE \"DEPU%\") AND nn=\"NN\" ";
            }

            if (comboBox1.SelectedIndex == 2)//MIXTO
            {
                tipo = " nombre_periodo NOT LIKE \"CLEM%\" AND status <> \"CARTERA\" ";
            }

            if (comboBox1.SelectedIndex == 4)//CLEM
            {
                tipo = " nombre_periodo LIKE \"CLEM%\" AND status <> \"CARTERA\" ";
            }  

            if (comboBox1.SelectedIndex == 3)//ESTRADOS
            {
                
                //tipo = " id>0 ";
                tipo = " nombre_periodo NOT LIKE \"CLEM%\" AND fecha_cartera IS NULL AND nn = \"ESTRADOS\" ";

                if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))//NUM PAQUETE
                {
                    conex4.conectar("base_principal");
                    if (comboBox2.SelectedItem.ToString()=="NINGUNO")
                    {
                        //MessageBox.Show("Selecc", "AVISO");                    
                    }else{
                        packs = conex4.consultar("SELECT id_credito,folio FROM estrados WHERE paquete=" + comboBox2.SelectedItem.ToString() + "");
                        conex4.cerrar();
                        //MessageBox.Show("" + packs.Rows.Count);
                        /*
                        if (consulta.Columns.Contains("registro_patronal") == false)
                        {
                            consulta.Columns.Add("registro_patronal");
                            consulta.Columns.Add("razon_social");
                            consulta.Columns.Add("credito_cuotas");
                            consulta.Columns.Add("credito_multa");
                            consulta.Columns.Add("importe_cuota");
                            consulta.Columns.Add("importe_multa");
                            consulta.Columns.Add("tipo_documento");
                            consulta.Columns.Add("periodo");
                            consulta.Columns.Add("id");
                            consulta.Columns.Add("estado_cartera");
                            consulta.Columns.Add("folio");
                        }
                        */
                        for (int i = 0; i < packs.Rows.Count; i++)
                        {
                            if(veri_acti==1){
                                sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera " +
                                  "FROM datos_factura WHERE id=" + packs.Rows[i][0].ToString() + " ";
                            }else{
                                sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera " +
                              "FROM datos_factura WHERE id=" + packs.Rows[i][0].ToString() + " ";
                            }

                            packs_dfactura = conex.consultar(sql);

                            if (packs_dfactura.Rows.Count > 0)
                            {
                                int repe=0;

                                for(int j=0; j<dataGridView1.RowCount;j++){
                                    if (packs_dfactura.Rows[0][8].ToString()==dataGridView1[9,j].Value.ToString())
                                    {
                                        repe = 1;
                                        j = dataGridView1.RowCount + 1;
                                    }
                                }

                                if(repe==1){
                                }else{
                                    dataGridView1.Rows.Add(true,
                                        packs_dfactura.Rows[0][0].ToString(),
                                        packs_dfactura.Rows[0][1].ToString(),
                                        packs_dfactura.Rows[0][2].ToString(),
                                        packs_dfactura.Rows[0][3].ToString(),
                                        packs_dfactura.Rows[0][4].ToString(),
                                        packs_dfactura.Rows[0][5].ToString(),
                                        packs_dfactura.Rows[0][6].ToString(),
                                        packs_dfactura.Rows[0][7].ToString(),
                                        packs_dfactura.Rows[0][8].ToString(),
                                        packs_dfactura.Rows[0][9].ToString(),
                                        packs.Rows[i][1].ToString()
                                        );

                                    dataGridView1.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.SteelBlue;
                                   /* 
                                    dataGridView1.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[2].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[3].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[5].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[6].Style.BackColor = System.Drawing.Color.SteelBlue; 
                                    dataGridView1.Rows[i].Cells[7].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[8].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[9].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[10].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    dataGridView1.Rows[i].Cells[11].Style.BackColor = System.Drawing.Color.SteelBlue;
                                    */
                                }
                            }
                            //MessageBox.Show("cuenta: " + packs_dfactura.Rows.Count + " folio: " + packs.Rows[i][1].ToString());
                        
                        }
                    }
                }
                else//busqueda clasica
                {}
            }

            if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))//NUM PAQUETE ESTRADOS
            {}
            else
            {
                if (cred == " ")
                {
                }else{
                    sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera,nombre_periodo " +
                          "FROM datos_factura WHERE  " + tipo + " AND (estado_cartera = \"-\" OR estado_cartera = \"PENDIENTE_" + id_us + "\")" + rp + cred + per;
                    //MessageBox.Show(sql, "AVISO");
                    consulta = conex.consultar(sql);
                    //MessageBox.Show("" + consulta.Rows.Count);
                    if (comboBox1.SelectedIndex == 3)//solo estrados
                    {
                        if (consulta.Rows.Count>0)
                        {
                            conex4.conectar("base_principal");
                            packs = conex4.consultar("SELECT folio FROM estrados WHERE id_credito=" + consulta.Rows[0][8].ToString() + "");
                            conex4.cerrar();
                            //MessageBox.Show("" + packs.Rows.Count);

                            if (consulta.Columns.Contains("folio") == false)
                            {
                               consulta.Columns.Add("folio");
                            }

                            if (packs.Rows.Count > 0)
                            {
                                consulta.Rows[0][10] = packs.Rows[0][0].ToString();
                            }
                        }
                    }
                }
            }
            
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;               
            }
            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))//NUM PAQUETE ESTRADOS
                { }
                else
                {
                    if (consulta.Rows.Count > 0)
                    {
                        if (consulta.Rows[0][1].ToString() == dataGridView1[1,i].Value.ToString())
                        {
                            coin = 1;//si hay repetidos
                        }
                    }
                }
            }

            if (coin == 0)
            {
                if (consulta.Rows.Count > 0)
                {
                    if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))//NUM PAQUETE ESTRADOS
                    {/*
                        for (int j = 0; j < consulta.Rows.Count; j++)
                        {
                            dataGridView1.Rows.Add(
                                consulta.Rows[j][0].ToString(),
                                consulta.Rows[j][1].ToString(),
                                consulta.Rows[j][2].ToString(),
                                consulta.Rows[j][3].ToString(),
                                consulta.Rows[j][4].ToString(),
                                consulta.Rows[j][5].ToString(),
                                consulta.Rows[j][6].ToString(),
                                consulta.Rows[j][7].ToString(),
                                consulta.Rows[j][8].ToString(),
                                consulta.Rows[j][9].ToString()
                                );
                        }*/
                        dataGridView1.Columns[9].HeaderText = "Folio";
                    }
                    else
                    {
                        dataGridView1.Columns[11].HeaderText = "Nom Paquete";
                        dataGridView1.Rows.Add(true,
                            consulta.Rows[0][0].ToString(),
                            consulta.Rows[0][1].ToString(),
                            consulta.Rows[0][2].ToString(),
                            consulta.Rows[0][3].ToString(),
                            consulta.Rows[0][4].ToString(),
                            consulta.Rows[0][5].ToString(),
                            consulta.Rows[0][6].ToString(),
                            consulta.Rows[0][7].ToString(),
                            consulta.Rows[0][8].ToString(),
                            consulta.Rows[0][9].ToString(),
                            consulta.Rows[0][10].ToString()
                            );
                    }

                    maskedTextBox1.Text = "";
                    maskedTextBox2.Text = "";
                    maskedTextBox3.Text = "";
                    comboBox2.SelectedIndex = -1;
                    maskedTextBox1.Focus();
                }
            }

            label5.Text = "Total: " + dataGridView1.Rows.Count;
            //comboBox2.SelectedIndex = -1;
            if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))//NUM PAQUETE ESTRADOS
            {
                comboBox2.Enabled = false;
                button2.Enabled = false;
            }
            maskedTextBox1.Focus();
        }

        private void Reportes_cartera_Nvo_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;
            
            conex.conectar("base_principal");
            conex3.conectar("base_principal");
            comboBox1.SelectedIndex = 0;
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();
            consultamysql.Columns.Add();

            id_us = MainForm.datos_user_static[7];

            llenar_Cb2();

            if (MainForm.cartera_estrados == 1)
            {
                comboBox2.Visible = true;
                label6.Visible = true;
                comboBox1.SelectedIndex = 3;
                comboBox1.Enabled = false;
                checkBox1.Visible = true;
                button5.Visible = true;

                dataGridView1.Columns[0].ReadOnly = false;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[10].ReadOnly = true;
                dataGridView1.Columns[11].ReadOnly = true;

                dataGridView1.Columns[1].Frozen = false;                
            }
            else
            {
                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[10].ReadOnly = true;
                dataGridView1.Columns[11].ReadOnly = true;
            }
        }
        //generar_reporte
        private void button1_Click(object sender, EventArgs e)
        {
            String fecha_cartera = "", sql = "";
            int tot = 0;
            DialogResult resu;
            conex2.conectar("base_principal");

            if (comboBox1.SelectedIndex == 3){//si es estrado

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value.ToString()) == true)
                    {
                        tot = tot + 1;
                    }
                }

                 resu = MessageBox.Show("Se va a Generar el Reporte de entrega a cartera de\n" + tot + " créditos " + comboBox1.SelectedItem.ToString() + ".\nEsto afectará la Base de Datos." +
               "\n\n¿Está seguro que desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            }else{
                resu = MessageBox.Show("Se va a Generar el Reporte de entrega a cartera de\n" + dataGridView1.Rows.Count + " créditos " + comboBox1.SelectedItem.ToString() + ".\nEsto afectará la Base de Datos." +
               "\n\n¿Está seguro que desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            }            

            if (resu == DialogResult.Yes)
            {
                fecha_cartera = System.DateTime.Today.ToShortDateString();
                fecha_cartera = fecha_cartera.Substring(6, 4) + "-" + fecha_cartera.Substring(3, 2) + "-" + fecha_cartera.Substring(0, 2);
                consultamysql.Rows.Clear();

                if (comboBox1.SelectedIndex == 3)
                {
                    consultamysql.Columns.Add();
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))
                    if(comboBox1.SelectedIndex==3)//estrados
                    {
                        if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value.ToString()) == true)
                        {
                            consultamysql.Rows.Add(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[2].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[3].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[4].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[5].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[6].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[7].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[8].FormattedValue.ToString(),
                                                      dataGridView1.Rows[i].Cells[11].FormattedValue.ToString());
                        }
                    }
                    else
                    {
                        consultamysql.Rows.Add(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[2].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[3].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[4].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[5].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[6].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[7].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[8].FormattedValue.ToString());
                    }

                    if(comboBox1.SelectedIndex != 4){
                        sql = "UPDATE datos_factura SET status=\"CARTERA\",fecha_cartera =\"" + fecha_cartera + "\",estado_cartera=\"ENTREGADO\" WHERE id=" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + "";
                    }else{
                        sql = "UPDATE datos_factura SET status=\"C_EMPRESAS\",fecha_cartera =\"" + fecha_cartera + "\",estado_cartera=\"ENTREGADO\" WHERE id=" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + "";
                    
                    }
                        //MessageBox.Show(sql);
                    conex2.consultar(sql);
                    conex2.guardar_evento("Se entrega a cartera el crédito con el ID: " + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + " el dia: " + fecha_cartera);
                }

                conex2.cerrar();

                Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();

                if (comboBox1.SelectedIndex == 0)//NOTIFICADO
                {
                    entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS A CARTERA", " ", "CARTERA");
                    entrega_cartera.Show();
                }

                if (comboBox1.SelectedIndex == 1)// NO NOTIFICADO
                {
                    entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NO NOTIFICADOS A CARTERA", "NO NOTIFICADOS", "CARTERA");
                    entrega_cartera.Show();
                }

                if (comboBox1.SelectedIndex == 2)//MIXTO
                {
                    entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NO NOTIFICADOS A CARTERA", "MIXTO", "CARTERA");
                    entrega_cartera.Show();
                }

                if (comboBox1.SelectedIndex == 3)//ESTRADOS
                {
                    if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))
                    {
                        entrega_cartera.envio_estrados(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS POR ESTRADOS A CARTERA", "ESTRADOS | PAQUETE N° " + comboBox2.SelectedItem, "CARTERA");
                    }
                    else
                    {
                        entrega_cartera.envio_estrados(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS POR ESTRADOS A CARTERA", "ESTRADOS", "CARTERA");
                    }
                    entrega_cartera.Show();
                }

                if (comboBox1.SelectedIndex == 4)//CLEM
                {
                    entrega_cartera.envio3(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS A CLASIFICACIÓN DE EMPRESAS", " ", "C. de EMPRESAS");
                    entrega_cartera.Show();
                }

                dataGridView1.Rows.Clear();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (opo > 0)
            {
                comboBox1.Enabled = false;
            }

            if(comboBox1.SelectedIndex==3){
                comboBox2.Visible = true;
                label6.Visible = true;
            }
            dataGridView1.Rows.Clear();
            opo++;
        }
        //guardar
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                conex3.consultar("UPDATE datos_factura SET estado_cartera=\"PENDIENTE_" + id_us + "\" WHERE id=" + dataGridView1.Rows[i].Cells[9].FormattedValue.ToString() + "");
            }
            MessageBox.Show("Credito(s) Guardado(s) Correctamente", "AVISO");
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            conex3.consultar("UPDATE datos_factura SET estado_cartera=\"-\" WHERE id=" + dataGridView1.Rows[e.Row.Index].Cells[9].FormattedValue.ToString() + "");
            label5.Text = "Total: " + dataGridView1.Rows.Count;
        }
        //cargar_guardados
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult resu = MessageBox.Show("La lista se limpiará para cargar los créditos que se hayan guardado anteriormente." +
                "\n\n¿Está seguro que desea continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (resu == DialogResult.Yes)
            {
                String sql;
                dataGridView1.Rows.Clear();

                sql = "SELECT registro_patronal,razon_social,credito_cuotas,credito_multa,importe_cuota,importe_multa,tipo_documento,periodo,id,estado_cartera " +
                      "FROM datos_factura WHERE  estado_cartera = \"PENDIENTE_" + id_us + "\"";

                consulta = conex.consultar(sql);

                for (int i = 0; i < consulta.Rows.Count; i++)
                {
                    conex4.conectar("base_principal");
                    packs = conex4.consultar("SELECT folio FROM estrados WHERE id_credito="+consulta.Rows[i][8].ToString()+"");
                    conex4.cerrar();

                    dataGridView1.Rows.Add(
                            consulta.Rows[i][0].ToString(),
                            consulta.Rows[i][1].ToString(),
                            consulta.Rows[i][2].ToString(),
                            consulta.Rows[i][3].ToString(),
                            consulta.Rows[i][4].ToString(),
                            consulta.Rows[i][5].ToString(),
                            consulta.Rows[i][6].ToString(),
                            consulta.Rows[i][7].ToString(),
                            consulta.Rows[i][8].ToString(),
                            consulta.Rows[i][9].ToString(),
                            packs.Rows[0][0].ToString()
                            );
                }

                label5.Text = "Total: " + dataGridView1.Rows.Count;
            }

        }

        private void Reportes_cartera_Nvo_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.cartera_estrados = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)//desactivar
            {
                button5.Enabled = true;
                button1.Enabled = false;
                veri_acti = 0;
                dataGridView1.Rows.Clear();
                label5.Text = "Total: 0";
                llenar_Cb2();
            }
            else //activar
            {
                button5.Enabled = false;
                button1.Enabled = true;
                veri_acti = 1;
                dataGridView1.Rows.Clear();
                label5.Text = "Total: 0";
                llenar_Cb2();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Visor_reporte_cartera entrega_cartera = new Visor_reporte_cartera();

            if (comboBox1.SelectedIndex == 3)//ESTRADOS
            {
                consultamysql.Columns.Add();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                   if (comboBox1.SelectedIndex == 3)//estrados
                    {
                        consultamysql.Rows.Add(dataGridView1.Rows[i].Cells[1].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[2].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[3].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[4].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[5].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[6].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[7].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[8].FormattedValue.ToString(),
                                                  dataGridView1.Rows[i].Cells[11].FormattedValue.ToString());
                    }                    
                }

                if ((comboBox2.SelectedIndex > -1) && (comboBox2.SelectedItem.ToString() != "NIGUNO"))
                {
                    entrega_cartera.envio_estrados(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS POR ESTRADOS A CARTERA", "ESTRADOS | PAQUETE N° " + comboBox2.SelectedItem, "CARTERA");
                }
                else
                {
                    entrega_cartera.envio_estrados(consultamysql, "RELACIÓN DE ENTREGA DE CRÉDITOS NOTIFICADOS POR ESTRADOS A CARTERA", "ESTRADOS", "CARTERA");
                }
                entrega_cartera.Show();
            }    
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


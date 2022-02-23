using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Supernova
{
    public partial class Consulta : Form
    {
        public Consulta(DataTable user)
        {
            InitializeComponent();

            this.usuario = user;
        }

        DataTable usuario = new DataTable();
        DataTable consulta = new DataTable();
        DataTable info_base = new DataTable();
        DataTable consulta_subs = new DataTable();
        DataTable consulta_prin = new DataTable();
        DataTable tabla_detalle = new DataTable();

        Super_conexion base_conex = new Super_conexion();
        Super_conexion conex_sub = new Super_conexion();

        public static String subs_con="";       

        string[] estados ={"Aguascalientes","Baja California Norte","Baja California Sur","Campeche","Coahuila","Colima","Chiapas","Chihuahua","Durango","Distrito Federal (Ciudad de México)",
                          "Guanajuato","Guerrero","Hidalgo","Jalisco","México","Michoacán","Morelos","Nayarit","Nuevo León","Oaxaca","Puebla","Querétaro","Quintana Roo","San Luis Potosí","Sinaloa","Sonora","Tabasco","Tamaulipas","Tlaxcala","Veracruz","Yucatán","Zacatecas"};
        
        public void llena_estado()
        {
            consulta=base_conex.consultar("SELECT DISTINCT(estado) FROM conexiones ORDER BY estado ASC");

            for (int i = 0; i < consulta.Rows.Count; i++)
            {
                info_base.Rows.Add();
                info_base.Rows[i][0] = consulta.Rows[i][0].ToString();                
            }
        }

        public void llena_del_nom()
        {
            consulta = base_conex.consultar("SELECT DISTINCT(del_nom) FROM conexiones ORDER BY del_nom ASC");

            for (int i = 0; i < consulta.Rows.Count; i++)
            {
                if(info_base.Rows.Count < (i+1)){
                    info_base.Rows.Add();
                }

                info_base.Rows[i][1] = consulta.Rows[i][0].ToString();
            }
        }

        public void llena_del_num()
        {
            consulta = base_conex.consultar("SELECT DISTINCT(del_num) FROM conexiones ORDER BY del_num ASC");

            for (int i = 0; i < consulta.Rows.Count; i++)
            {
                if (info_base.Rows.Count < (i + 1))
                {
                    info_base.Rows.Add();
                }

                info_base.Rows[i][2] = consulta.Rows[i][0].ToString();
            }
        }

        public void llena_sub_nom()
        {
            consulta = base_conex.consultar("SELECT DISTINCT(sub_nom) FROM conexiones ORDER BY sub_nom ASC");

            for (int i = 0; i < consulta.Rows.Count; i++)
            {
                if (info_base.Rows.Count < (i + 1))
                {
                    info_base.Rows.Add();
                }

                info_base.Rows[i][3] = consulta.Rows[i][0].ToString();
            }
        }

        public void llena_sub_num()
        {
            consulta = base_conex.consultar("SELECT DISTINCT(sub_num) FROM conexiones ORDER BY sub_num ASC");

            for (int i = 0; i < consulta.Rows.Count; i++)
            {
                if (info_base.Rows.Count < (i + 1))
                {
                    info_base.Rows.Add();
                }

                info_base.Rows[i][4] = consulta.Rows[i][0].ToString();
            }
        }

        public void llena_municipio()
        {
            consulta = base_conex.consultar("SELECT DISTINCT(municipio) FROM conexiones ORDER BY municipio ASC");

            for (int i = 0; i < consulta.Rows.Count; i++)
            {
                if (info_base.Rows.Count < (i + 1))
                {
                    info_base.Rows.Add();
                }

                info_base.Rows[i][5] = consulta.Rows[i][0].ToString();
            }
        }

        public void consulta_subdeles()
        {
            string sql = "", sql_base="",sub_error="";

            if (!comboBox1.SelectedItem.ToString().Equals("Subdelegación"))
            {
                sql_base = "SELECT sub_num,sub_nom,usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena,ip_servidor,nombre_bd FROM conexiones WHERE idconexiones in ("+subs_con+")";
            }
            else
            {
                if (comboBox3.SelectedIndex == 0)//nombre
                {
                    sql_base = "SELECT sub_num,sub_nom,usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena,ip_servidor,nombre_bd FROM conexiones WHERE sub_nom = \""+comboBox2.SelectedItem.ToString()+"\"";
                }

                if (comboBox3.SelectedIndex == 1)//numero
                {
                    sql_base = "SELECT sub_num,sub_nom,usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena,ip_servidor,nombre_bd FROM conexiones WHERE sub_num = \"" + comboBox2.SelectedItem.ToString() + "\"";
                }
            }

            //lista de subs
            consulta_prin=base_conex.consultar(sql_base);

            dataGridView1.Columns.Add("No_Sub", "N°_SUB");
            dataGridView1.Columns.Add("Nombre_Sub", "NOMBRE_SUB");
            dataGridView1.Columns.Add("total", "TOTAL");

            sql = "SELECT status, COUNT(id) AS \"Total\" FROM datos_factura WHERE id>0 GROUP BY status ORDER BY status ASC";
            
            for (int i = 0; i < consulta_prin.Rows.Count;i++)
            {
                //lista de status   
                String cad_con = @"server=" + consulta_prin.Rows[i][4].ToString() + "; userid=" + consulta_prin.Rows[i][2].ToString() + "; password=" +consulta_prin.Rows[i][3].ToString() + "; database=" + consulta_prin.Rows[i][5].ToString() + ";";

                if (conex_sub.probar(cad_con) == true)
                {
                    conex_sub.conectar(consulta_prin.Rows[i][5].ToString(), consulta_prin.Rows[i][4].ToString(), consulta_prin.Rows[i][2].ToString(), consulta_prin.Rows[i][3].ToString());
                    consulta_subs = conex_sub.consultar(sql);

                    int tot = 0;

                    if (i == 0)
                    {
                        for (int k = 0; k < consulta_subs.Rows.Count; k++)
                        {
                            dataGridView1.Columns.Add(consulta_subs.Rows[k][0].ToString(), consulta_subs.Rows[k][0].ToString());
                        }
                    }

                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = consulta_prin.Rows[i][0].ToString();
                    dataGridView1[1, i].Value = consulta_prin.Rows[i][1].ToString();

                    for (int j = 0; j < consulta_subs.Rows.Count; j++)
                    {
                        int no_existe = 0;
                        for (int l = 0; l < dataGridView1.ColumnCount; l++)
                        {
                            if (consulta_subs.Rows[j][0].ToString() == dataGridView1.Columns[l].HeaderText)
                            {
                                dataGridView1[l, i].Value = consulta_subs.Rows[j][1].ToString();
                                no_existe = 1;
                            }
                        }

                        if (no_existe == 0)
                        {
                            dataGridView1.Columns.Add(consulta_subs.Rows[j][0].ToString(), consulta_subs.Rows[j][0].ToString());
                            dataGridView1[dataGridView1.ColumnCount, i].Value = consulta_subs.Rows[j][1].ToString();
                        }

                        tot = tot + (Convert.ToInt32(consulta_subs.Rows[j][1].ToString()));
                    }

                    dataGridView1[2, i].Value = tot;

                    conex_sub.cerrar();
                }
                else
                {
                    sub_error = sub_error + consulta_prin.Rows[i][1].ToString()+",";
                }
            }

            if (sub_error.Length == 0)
            {
                MessageBox.Show("Todas las consultas fueron efectuadas exitosamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                sub_error = sub_error.Substring(0, sub_error.Length - 1);
                MessageBox.Show("No se pudo establecer conexión con la(s) subdelegación(es) siguientes: \n"+sub_error, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void Consulta_Load(object sender, EventArgs e)
        {
            String lugar = "";           

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

            base_conex.conectar("supernova", bd_server, bd_user, bd_pass);            

            info_base.Columns.Add("estado");
            info_base.Columns.Add("delegacion_nom");
            info_base.Columns.Add("delegacion_num");
            info_base.Columns.Add("subdelegacion_nom");
            info_base.Columns.Add("subdelegacion_num");
            info_base.Columns.Add("municipio");

            llena_estado();
            llena_del_nom();
            llena_del_num();
            llena_sub_nom();
            llena_sub_num();
            llena_municipio();

            comboBox3.SelectedIndex = -1;

            switch(Convert.ToInt32(usuario.Rows[0][4].ToString()))
            {
                case 0: lugar = "Motherbase";
                        comboBox1.Items.Add("Delegación");
                        comboBox1.Items.Add("Subdelegación");
                        comboBox1.Items.Add("Estado");
                        comboBox1.Items.Add("Municipio");
                        comboBox1.SelectedIndex = 0;
                    break;
                case 1: lugar = "Subdelegación";
                        comboBox1.Items.Add("Subdelegación");
                        comboBox1.SelectedIndex = 0;
                        comboBox1.Enabled = false;
                        comboBox2.Items.Add(usuario.Rows[0][5].ToString());
                        comboBox2.SelectedIndex = 0;
                        comboBox2.Enabled = false;
                    break;
                case 2: lugar = "Delegación";
                        comboBox1.Items.Add("Delegación");
                        comboBox1.Items.Add("Subdelegación");
                        comboBox1.SelectedIndex = 0;
                        //comboBox1.Enabled = false;
                        comboBox2.Items.Add(usuario.Rows[0][5].ToString());
                        comboBox2.SelectedIndex = 0;
                    break;
                case 3: lugar = "Nivel Central";
                        comboBox1.Items.Add("Delegación");
                        comboBox1.Items.Add("Subdelegación");
                        comboBox1.Items.Add("Estado");
                        comboBox1.Items.Add("Municipio");
                        comboBox1.SelectedIndex = 0;
                    break;
            }

            label1.Text= usuario.Rows[0][0].ToString()+" - "+lugar+" "+usuario.Rows[0][5].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int valido = 0;

            if (!comboBox1.SelectedItem.ToString().Equals("Subdelegación"))
            {                          
                if (comboBox2.SelectedIndex > -1)
                {
                    String t = comboBox1.SelectedItem.ToString(), nn = comboBox3.SelectedItem.ToString(), v = comboBox2.SelectedItem.ToString();
                    Super_opcion_consulta opc = new Super_opcion_consulta(t, nn, v);
                    opc.ShowDialog();
                    opc.Focus();

                    if (subs_con.Length < 1)
                    {
                        MessageBox.Show("No se ha seleccionado ninguna subdelegación para consultar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        valido = 1;
                        //MessageBox.Show(subs_con);     
                    }
                }
            }

            if(valido==0){
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                consulta_subdeles();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            if (comboBox1.SelectedItem.ToString() == "Estado")
            {
                for (int i = 0; i < info_base.Rows.Count;i++)
                {
                    if(info_base.Rows[i][0].ToString().Length>1){
                        comboBox2.Items.Add(info_base.Rows[i][0].ToString());
                    }
                }

                comboBox3.SelectedIndex = 0;
                comboBox3.Enabled = false;
            }
            
            if (comboBox1.SelectedItem.ToString() == "Municipio")
            {
                for (int i = 0; i < info_base.Rows.Count; i++)
                {
                     if (info_base.Rows[i][5].ToString().Length > 1)
                     {
                         comboBox2.Items.Add(info_base.Rows[i][5].ToString());
                     }
                }

                comboBox3.SelectedIndex = 0;
                comboBox3.Enabled = false;
            }

            if (comboBox1.SelectedItem.ToString() == "Delegación")
            {
                //comboBox3.SelectedIndex = 0;
                comboBox3.Enabled = true;

                if (comboBox3.SelectedIndex == 0)//nombre
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][1].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][1].ToString());
                        }
                    }
                }

                if (comboBox3.SelectedIndex == 1)//numero
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][2].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][2].ToString());
                        }
                    }
                }
            }

            if (comboBox1.SelectedItem.ToString() == "Subdelegación")
            {
                //comboBox3.SelectedIndex = 0;
                comboBox3.Enabled = true;

                if (comboBox3.SelectedIndex == 0)//nombre
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][3].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][3].ToString());
                        }
                    }
                }

                if (comboBox3.SelectedIndex == 1)//numero
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][4].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][4].ToString());
                        }
                    }
                }
            }            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            if(comboBox3.SelectedIndex==0){//nombre
                if (comboBox1.SelectedItem.ToString() == "Delegación"){
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][1].ToString().Length > 1)
                        {                            
                            comboBox2.Items.Add(info_base.Rows[i][1].ToString());
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "Subdelegación"){
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][3].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][3].ToString());
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "Estado"){
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][0].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][0].ToString());
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "Municipio")
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][5].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][5].ToString());
                        }
                    }
                }
            }

            if (comboBox3.SelectedIndex == 1)//numero
            {
                if (comboBox1.SelectedItem.ToString() == "Delegación")
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][2].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][2].ToString());
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "Subdelegación")
                {
                    for (int i = 0; i < info_base.Rows.Count; i++)
                    {
                        if (info_base.Rows[i][4].ToString().Length > 1)
                        {
                            comboBox2.Items.Add(info_base.Rows[i][4].ToString());
                        }
                    }
                }
            }
        }

        private void exportarAExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Title = "Guardar archivo de Excel";
            fichero.Filter = "Archivo Excel (*.XLSX)|*.xlsx";

            if (fichero.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XLWorkbook wb = new XLWorkbook();
                    tabla_detalle = copiar_datagrid();
                    wb.Worksheets.Add(tabla_detalle, "Estado_Notificacion");
                    wb.SaveAs(fichero.FileName);
                    MessageBox.Show("Se Ha creado correctamente el archivo:\n" + fichero.FileName, "Exito");
                }
                catch (Exception es)
                {
                    MessageBox.Show("Ha ocurrido un error al intentar crear el archivo:\n" + fichero.FileName + "\n\n" + es, "ERROR");
                }
            }
        }
    
    }
}

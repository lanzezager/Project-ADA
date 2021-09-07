using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Mod40
{
    public partial class Info40 : Form
    {
        public Info40()
        {
            InitializeComponent();
        }

        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();

        //Declaracion de elementos para conexion office
        OleDbConnection conexion = null;
        DataSet dataSet = null;
        OleDbDataAdapter dataAdapter = null;
        DataTable tablanrps = new DataTable();
        DataTable tablanrps_guarda = new DataTable();
        DataTable tablanrps_sub = new DataTable();
        DataTable tablanrps_eliminados = new DataTable();
        DataTable tablanrps_adicionados = new DataTable();
        DataTable tablanrps_por_eliminar = new DataTable();
        
        public void guardar_config()
        {
            try
            {
                //Borrar archivo existente
                System.IO.File.Delete(@"mod40_info_config.lz");
                //Abrir archivo
                StreamWriter wr = new StreamWriter(@"mod40_info_config.lz");

                /*wr.WriteLine("delegacion:" + textBox1.Text);
                wr.WriteLine("num_delegacion:" + textBox2.Text);
                wr.WriteLine("municipio:" + textBox3.Text);
                wr.WriteLine("subdelegacion:" + textBox4.Text);
                wr.WriteLine("num_subdelegacion:" + textBox5.Text);
                wr.WriteLine("subdelegado:" + textBox6.Text);
                wr.WriteLine("jefe_cob:" + textBox7.Text);
                wr.WriteLine("jefe_epo:" + textBox8.Text);
                wr.WriteLine("jefe_afi_vig:" + textBox9.Text);*/
                wr.WriteLine("ref_baja:" + textBox10.Text);
                wr.WriteLine("ref_ofi:" + textBox11.Text);
                wr.WriteLine("DON'T CHANGE THIS SETTINGS!!!!!");
                wr.WriteLine("By LZ");
                wr.WriteLine("Arriba el Atlas!!!!!");
                wr.Close();

                guardar_nrps();

                MessageBox.Show("Ajustes Guardados correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo guardar la configuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void leer_config()
        {
            String del, del_num, mpio, sub, sub_num, subdele, jefe_cob, jefe_emi, jefe_afi,ref_baja,ref_ofi;
            try
            {
                StreamReader rdr = new StreamReader(@"mod40_info_config.lz");

               /* del=rdr.ReadLine();
                del_num=rdr.ReadLine();
                mpio=rdr.ReadLine();
                sub=rdr.ReadLine(); 
                sub_num=rdr.ReadLine(); 
                subdele=rdr.ReadLine(); 
                jefe_cob=rdr.ReadLine(); 
                jefe_emi=rdr.ReadLine(); 
                jefe_afi=rdr.ReadLine();*/
                ref_baja = rdr.ReadLine();
                ref_ofi = rdr.ReadLine();
                rdr.Close();

               /* del=del.Substring(11, del.Length - 11);
                del_num=del_num.Substring(15, del_num.Length - 15);
                mpio = mpio.Substring(10, mpio.Length - 10);
                sub = sub.Substring(14, sub.Length - 14);
                sub_num = sub_num.Substring(18, sub_num.Length - 18);
                subdele = subdele.Substring(12, subdele.Length - 12);
                jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
                jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
                jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);*/
                ref_baja = ref_baja.Substring(9, ref_baja.Length - 9);
                ref_ofi = ref_ofi.Substring(8, ref_ofi.Length - 8);

              /*  textBox1.Text = del;
                textBox2.Text = del_num;
                textBox3.Text = mpio;
                textBox4.Text = sub;
                textBox5.Text = sub_num;
                textBox6.Text = subdele;
                textBox7.Text = jefe_cob;
                textBox8.Text = jefe_emi;
                textBox9.Text = jefe_afi;*/
                textBox10.Text = ref_baja;
                textBox11.Text = ref_ofi;
                

            }
            catch (Exception error)
            {
                //MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }

        public void leer_nrps_sub()
        {
            String cad_con = "", hoja = "", cons_exc = "";
            //esta cadena es para archivos excel 2007 y 2010
            cad_con = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='lista_nrps_mod40.xlsx';Extended Properties=Excel 12.0;";
            conexion = new OleDbConnection(cad_con);//creamos la conexion con la hoja de excel
            conexion.Open(); //abrimos la conexion

            hoja = "Hoja1";

            if (string.IsNullOrEmpty(hoja))
            {
                MessageBox.Show("No hay una hoja para leer");
            }
            else
            {
                //cons_exc = "Select [NRP] from [" + hoja + "$] WHERE [NUM_SUB]=\"" + conex.leer_config_sub()[4] + "\"";
                cons_exc = "Select * from [" + hoja + "$] ";
                try
                {
                    //Si el usuario escribio el nombre de la hoja se procedera con la busqueda
                    //conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
                    //conexion.Open(); //abrimos la conexion
                    dataAdapter = new OleDbDataAdapter(cons_exc, conexion); //traemos los datos de la hoja y las guardamos en un dataSdapter
                    dataSet = new DataSet(); // creamos la instancia del objeto DataSet
                    if (dataAdapter.Equals(null))
                    {
                        MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n", "Error al Abrir Archivo de Excel/");
                    }
                    else
                    {
                        dataAdapter.Fill(dataSet, hoja);//llenamos el dataset
                        tablanrps = dataSet.Tables[0]; //le asignamos al DataGridView el contenido del dataSet
                        DataView dv = new DataView(tablanrps);
                        dv.RowFilter = "NUM_SUB = '" + conex.leer_config_sub()[4] + "'";
                        tablanrps_sub = dv.ToTable();                        

                        conexion.Close();//cerramos la conexion
                        //MessageBox.Show(tablanrps.Rows[0][0].ToString());
                    }
                }
                catch (AccessViolationException ex)
                {
                    //en caso de haber una excepcion que nos mande un mensaje de error
                    MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja\n" + ex, "Error al Abrir Archivo de Excel");
                }
            }
        }

        public void guardar_nrps()
        {
            //verificar eliminados
            if(tablanrps_eliminados.Rows.Count>0){

                tablanrps_guarda.Columns.Add("NRP");
                tablanrps_guarda.Columns.Add("NUM_SUB");
                tablanrps_guarda.Columns.Add("NOM_SUB"); 

                for (int i = 0; i < tablanrps.Rows.Count - 1;i++)
                {
                    int eli = 0;
                    for (int j = 0; j < tablanrps_eliminados.Rows.Count - 1; j++)
                    {
                        if (tablanrps.Rows[i][0].ToString() == tablanrps_eliminados.Rows[j][0].ToString())
                        {
                            eli = 1;
                        }
                        else
                        {
                           
                        }
                    }

                    if(eli==0){
                        tablanrps_guarda.Rows.Add(tablanrps.Rows[i][0].ToString(), tablanrps.Rows[i][1].ToString(), tablanrps.Rows[i][2].ToString());
                    }
                }

                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(tablanrps_guarda, "Hoja1");
                wb.SaveAs(@"lista_nrps_mod40.xlsx");
            }

            //verificar añadidos
            if(tablanrps_adicionados.Rows.Count>0){
                tablanrps_guarda.Merge(tablanrps);
                tablanrps_guarda.Merge(tablanrps_adicionados);

                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(tablanrps_guarda, "Hoja1");
                wb.SaveAs(@"lista_nrps_mod40.xlsx");
            }

        }

        private void Info40_Load(object sender, EventArgs e)
        {
            String window_name = this.Text;
            //window_name = window_name.Replace("Gear Prime", "Nova Gear");
            this.Text = window_name;
            this.Icon = Nova_Gear.Properties.Resources.logo_nova_white_2;

            leer_config();
            leer_nrps_sub();

            int i=0;
            while (i < tablanrps_sub.Rows.Count)
            {
                listBox1.Items.Add(tablanrps_sub.Rows[i][0].ToString());
                i++;
            }
            label3.Text = "Total: " + tablanrps_sub.Rows.Count;

            tablanrps_eliminados.Columns.Add("NRP");
            tablanrps_eliminados.Columns.Add("NUM_SUB");
            tablanrps_eliminados.Columns.Add("NOM_SUB");

            tablanrps_adicionados.Columns.Add("NRP");
            tablanrps_adicionados.Columns.Add("NUM_SUB");
            tablanrps_adicionados.Columns.Add("NOM_SUB");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            guardar_config();   
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int llave=0,importe=0;
            /*
            if (textBox1.Text.Length > 5)
            {
                llave++;
            }

            if (textBox2.Text.Length > 1)
            {
                if (int.TryParse(textBox2.Text, out importe))
                {
                    llave++;
                }
            }

            if (textBox3.Text.Length > 5)
            {
                llave++;
            }

            if (textBox4.Text.Length > 5)
            {
                llave++;
            }

            if (textBox5.Text.Length > 1)
            {
                if (int.TryParse(textBox5.Text, out importe))
                {
                    llave++;
                }
                
            }

            if (textBox6.Text.Length > 5)
            {
                llave++;
            }

            if (textBox7.Text.Length > 5)
            {
                llave++;
            }

            if (textBox8.Text.Length > 5)
            {
                llave++;
            }

            if (textBox9.Text.Length > 5)
            {
                llave++;
            }
            */
            if (textBox10.Text.Length > 5)
            {
                llave++;
            }

            if (textBox11.Text.Length > 5)
            {
                llave++;
            }

            if (llave >= 2)
            {
                button4.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
            }
            llave = 0;
        }

        private void quitarRegistroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex > -1){
                DialogResult res = MessageBox.Show("Se Removerá este Registro Patronal: "+listBox1.Items[listBox1.SelectedIndex].ToString()+"\n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (res == DialogResult.Yes)
                {
                    //try
                    //{
                        DataView dv = new DataView(tablanrps);
                        dv.RowFilter = "NRP = '" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'";
                        tablanrps_por_eliminar = dv.ToTable();
                        tablanrps_eliminados.Rows.Add(tablanrps_por_eliminar.Rows[0][0].ToString(), tablanrps_por_eliminar.Rows[0][1].ToString(), tablanrps_por_eliminar.Rows[0][2].ToString());
                  /*  }
                    catch
                    {
                        try
                        {
                            DataView dv = new DataView(tablanrps_adicionados);
                            dv.RowFilter = "NRP = '" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'";
                            tablanrps_por_eliminar = dv.ToTable();
                            tablanrps_eliminados.Rows.Add(tablanrps_por_eliminar.Rows[0][0].ToString(), tablanrps_por_eliminar.Rows[0][1].ToString(), tablanrps_por_eliminar.Rows[0][2].ToString());
                        }
                        catch
                        {
                        }
                    }*/
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    //MessageBox.Show(tablanrps_eliminados.Rows[0][0].ToString());
                    label3.Text = "Total: " + listBox1.Items.Count;
                    maskedTextBox4.Enabled = false;
                    quitarRegistroToolStripMenuItem.Enabled = false;
                }

            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("" + e.Button.ToString());
            
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                listBox1.SelectedIndex = listBox1.IndexFromPoint(e.X, e.Y);
            }
        }

        private void maskedTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox4.MaskCompleted == true)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Se Añadira este Registro Patronal: "+maskedTextBox4.Text+"\n ¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (res == DialogResult.Yes)
            {
                String nrp_nvo = "";

                nrp_nvo = maskedTextBox4.Text;
                nrp_nvo = nrp_nvo.Substring(1, nrp_nvo.Length - 1);
                nrp_nvo = nrp_nvo.Replace(" - ", "");

                tablanrps_adicionados.Rows.Add(nrp_nvo, conex.leer_config_sub()[4], conex.leer_config_sub()[3]);

                //MessageBox.Show(""+tablanrps_adicionados.Rows[0][0].ToString()+","+ tablanrps_adicionados.Rows[0][1].ToString()+","+tablanrps_adicionados.Rows[0][2].ToString());
                maskedTextBox4.Clear();

                listBox1.Items.Add(nrp_nvo);
                label3.Text = "Total: " + listBox1.Items.Count;

                quitarRegistroToolStripMenuItem.Enabled = false;
            }
        }

    }
}

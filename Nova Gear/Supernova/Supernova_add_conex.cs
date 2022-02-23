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
    public partial class Supernova_add_conex : Form
    {
        public Supernova_add_conex(int modo, int id_conexion)
        {
            InitializeComponent();

            this.mode = modo;
            this.id_con = id_conexion;
        }

        int id_con,mode;
        Super_conexion conex = new Super_conexion();
        DataTable datos_conex = new DataTable();
        DataTable datos_verif = new DataTable();
        DataTable datos_verif2 = new DataTable();

        private void button3_Click(object sender, EventArgs e)
        {
            int error = 0, num_sub = 0;

            if (textBox1.Text.Length < 5)
            {
                error = 1;
                MessageBox.Show("DELEGACIÓN debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (textBox2.Text.Length == 0 && error == 0)
            {
                error = 1;
                MessageBox.Show("NÚMERO DE DELEGACIÓN no puede quedar vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (int.TryParse(textBox2.Text, out num_sub) )
            { }
            else
            {
                if(error == 0){
                    error = 1;
                    MessageBox.Show("NÚMERO DE DELEGACIÓN debe de ser un Número", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (textBox3.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("SUBDELEGACIÓN debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

            if (textBox4.Text.Length == 0 && error == 0)
            {
                error = 1;
                MessageBox.Show("NÚMERO DE SUBDELEGACIÓN no puede quedar vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (int.TryParse(textBox4.Text, out num_sub))
            { }
            else
            {
                if (error == 0)
                {
                    error = 1;
                    MessageBox.Show("NÚMERO DE SUBDELEGACIÓN debe de ser un Número", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (comboBox1.SelectedIndex == -1 && error == 0)
            {
                error = 1;
                MessageBox.Show("Debe seleccionar el ESTADO correspondiente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (textBox5.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("MUNICIPIO debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if ((textBox6.BackColor == Color.MediumSpringGreen && textBox7.BackColor == Color.MediumSpringGreen && textBox8.BackColor == Color.MediumSpringGreen && textBox9.BackColor == Color.MediumSpringGreen))
            { }
            else
            {
                if (error == 0)
                {
                    error = 1;
                    MessageBox.Show("No ha probado con éxito los DATOS de CONEXIÓN", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }

            if(error==0){
                 DialogResult resul = MessageBox.Show("Los datos se almacenarán en la base de datos.\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                 if (resul == DialogResult.Yes){
                     datos_verif=conex.consultar("SELECT * FROM conexiones WHERE sub_num=" + textBox4.Text +"");

                     if(datos_verif.Rows.Count==0){
                         datos_verif2 = conex.consultar("SELECT * FROM conexiones WHERE sub_nom=\"" + textBox3.Text + "\"");

                         if (datos_verif2.Rows.Count == 0)
                         {
                             if (mode == 1)
                             {
                                 conex.consultar("INSERT INTO conexiones (del_nom,del_num,sub_nom,sub_num,estado,municipio,usuario,contrasena,nombre_bd,ip_servidor) " +
                                                " VALUES (\"" + textBox1.Text + "\"," + textBox2.Text + ",\"" + textBox3.Text + "\"," + textBox4.Text + ",\"" + comboBox1.SelectedItem.ToString() + "\",\"" + textBox5.Text + "\",\"" + textBox6.Text + "\",AES_ENCRYPT(\"" + textBox7.Text + "\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\"),\"" + textBox8.Text + "\",\"" + textBox9.Text + "\")");
                             }
                             else
                             {
                                 conex.consultar("UPDATE conexiones SET del_nom=\"" + textBox1.Text + "\",del_num=" + textBox2.Text + ",sub_nom=\"" + textBox3.Text + "\",sub_num=" + textBox4.Text + ",estado=\"" + comboBox1.SelectedItem.ToString() + "\",municipio=\"" + textBox5.Text + "\",usuario=\"" + textBox6.Text + "\",contrasena=AES_ENCRYPT(\"" + textBox7.Text + "\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\"),nombre_bd=\"" + textBox8.Text + "\", ip_servidor=\"" + textBox9.Text + "\"  WHERE idconexiones=" + id_con);
                             }

                             MessageBox.Show("Datos Guardados Correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                             this.Close();
                         }
                         else
                         {
                             MessageBox.Show("El Nombre de Subdelegación que se intenta ingresar ya fue registrado previamente con otra información, ", "Error de Duplicación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                         }
                     }else{
                         MessageBox.Show("El Número de Subdelegación que se intenta ingresar ya fue registrado previamente con otra información, ", "Error de Duplicación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                 }
            }
        }

        private void Supernova_add_conex_Load(object sender, EventArgs e)
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

            comboBox1.SelectedIndex = -1;

            if (mode == 2)
            {//modo edición
                label1.Text = "Editar Conexión";
                this.Text = "Nova Gear - Supervisión [Editar Conexión]";

                datos_conex = conex.consultar("SELECT del_nom,del_num,sub_nom,sub_num,estado,municipio,usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena,nombre_bd,ip_servidor FROM conexiones WHERE idconexiones=" + id_con);

                if (datos_conex.Rows.Count > 0)
                {
                    textBox1.Text = datos_conex.Rows[0][0].ToString();
                    textBox2.Text = datos_conex.Rows[0][1].ToString();
                    textBox3.Text = datos_conex.Rows[0][2].ToString();
                    textBox4.Text = datos_conex.Rows[0][3].ToString();
                    comboBox1.SelectedItem = datos_conex.Rows[0][4].ToString();
                    //MessageBox.Show(datos_conex.Rows[0][4].ToString());

                    /*for (int i = 0; i < comboBox1.Items.Count;i++)
                    {
                        //MessageBox.Show(comboBox1.Items[i].ToString());
                        if (datos_conex.Rows[0][4].ToString()==comboBox1.Items[i].ToString())
                        {
                            MessageBox.Show("Igual");
                            comboBox1.SelectedIndex = i;
                            i = comboBox1.Items.Count + 1;
                        }
                    }*/

                    textBox5.Text = datos_conex.Rows[0][5].ToString();

                    textBox6.Text = datos_conex.Rows[0][6].ToString();
                    textBox7.Text = datos_conex.Rows[0][7].ToString();
                    textBox8.Text = datos_conex.Rows[0][8].ToString();
                    textBox9.Text = datos_conex.Rows[0][9].ToString();
                }
                else
                {
                    MessageBox.Show("Conexión Inexistente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int error = 0;

            if (textBox6.Text.Length < 3)
            {
                error = 1;
                MessageBox.Show("NOMBRE USUARIO debe contener al menos 3 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (textBox7.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("CONTRASEÑA debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (textBox8.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("NOMBRE BASE DE DATOS debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (textBox9.Text.Length < 9 && error == 0)
            {
                error = 1;
                MessageBox.Show("DIRECCIÓN IP debe tener el siguiente formato:\nXXX.XXX.XXX.XXX", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (error == 0)
            {
                String cad_con = @"server=" + textBox9.Text + "; userid=" + textBox6.Text + "; password=" + textBox7.Text + "; database=" + textBox8.Text + ";";

                if ((conex.probar(cad_con)) == true)
                {
                    MessageBox.Show("Conexión realizada exitosamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                    button1.Enabled = false;

                    textBox6.BackColor = Color.MediumSpringGreen;
                    textBox7.BackColor = Color.MediumSpringGreen;
                    textBox8.BackColor = Color.MediumSpringGreen;
                    textBox9.BackColor = Color.MediumSpringGreen;
                }
                else
                {
                    MessageBox.Show("No se pudo realizar la conexión a la Base de Datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

    }
}

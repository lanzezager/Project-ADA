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
    public partial class Supernova_add_users : Form
    {
        public Supernova_add_users(int mode,int id_usuario)
        {
            InitializeComponent();
            this.modo = mode;
            this.id_user = id_usuario;
        }

        int modo,id_user;

        Super_conexion conex = new Super_conexion();
        DataTable datos_usuario = new DataTable();

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                textBox5.Enabled = false;
                textBox5.Text = "";
            }
            else
            {
                textBox5.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int error=0,num_sub=0;

            if(textBox1.Text.Length<5){
                error=1;
                MessageBox.Show("NOMBRE debe contener al menos 5 caracteres","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

            if (textBox2.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("PUESTO debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            if(comboBox1.SelectedIndex== -1 && error == 0){
                error = 1;
                MessageBox.Show("Debe seleccionar un LUGAR DE TRABAJO", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if ((comboBox1.SelectedIndex > -1 && comboBox1.SelectedIndex < 2) && textBox5.Text.Length == 0 && error == 0)
            {
                error = 1;

                if (comboBox1.SelectedIndex == 0)
                {
                     MessageBox.Show("NUMERO DE SUBDELEGACIÓN no puede quedar vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (comboBox1.SelectedIndex == 1)
                {
                    MessageBox.Show("NUMERO DE DELEGACIÓN no puede quedar vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }

            if (int.TryParse(textBox5.Text, out num_sub))
            { }
            else
            {
                error = 1;
                if (comboBox1.SelectedIndex == 0)
                {
                    MessageBox.Show("NUMERO DE SUBDELEGACIÓN debe de ser un Número", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (comboBox1.SelectedIndex == 1)
                {
                    MessageBox.Show("NUMERO DE DELEGACIÓN debe de ser un Número", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (textBox3.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("USUARIO debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (textBox4.Text.Length < 5 && error == 0)
            {
                error = 1;
                MessageBox.Show("CONTRASEÑA debe contener al menos 5 caracteres", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (comboBox2.SelectedIndex == -1 && error==0)
            {
                error = 1;
                MessageBox.Show("Debe seleccionar un tipo de PERMISO", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if(error==0){
                DialogResult resul = MessageBox.Show("Los datos se almacenarán en la base de datos.\n¿Desea Continuar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (resul == DialogResult.Yes)
                {
                    String lugar_trabajo_num = textBox5.Text;
                    int nivel = -1, tipo_trabajo=-1;

                    if(textBox5.Text.Length==0){
                        lugar_trabajo_num = "0";
                    }

                    if (comboBox2.SelectedIndex == 1)
                    {
                        nivel = 0;
                    }
                    else
                    {
                        nivel = 1;
                    }

                    tipo_trabajo = comboBox1.SelectedIndex + 1;

                    if(modo==1){//creacion
                        conex.consultar("INSERT INTO usuarios (nombre_usuario,contrasena,nombre,puesto,lugar_trabajo,lugar_trabajo_num,nivel) "+
                                        " VALUES (\"" + textBox3.Text + "\",AES_ENCRYPT(\"" + textBox4.Text + "\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\"),\"" + textBox1.Text + "\",\"" + textBox2.Text + "\"," + tipo_trabajo + "," + lugar_trabajo_num + ","+nivel+")");
                    }else{//edicion
                        if(id_user>1){
                            conex.consultar("UPDATE usuarios SET nombre_usuario =\"" + textBox3.Text + "\",contrasena = AES_ENCRYPT(\"" + textBox4.Text + "\", \"Nova Gear & AKD ATLAS & LZ RULES!!!\"),nombre =\"" + textBox1.Text + "\",puesto =\"" + textBox2.Text + "\",lugar_trabajo=" + tipo_trabajo + ",lugar_trabajo_num =" + lugar_trabajo_num + ", nivel="+nivel+"  WHERE idusuarios=" + id_user);
                        }
                    }

                    MessageBox.Show("Datos Guardados Correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Close();
                }
            }
        }

        private void Supernova_add_users_Load(object sender, EventArgs e)
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
            comboBox2.SelectedIndex = -1;

            if(modo==2){//modo edición
                label1.Text = "Editar Usuario";
                this.Text = "Nova Gear - Supervisión [Editar Usuario]";

                datos_usuario = conex.consultar("SELECT nombre_usuario,CAST(AES_DECRYPT(contrasena, \"Nova Gear & AKD ATLAS & LZ RULES!!!\") AS CHAR(32)) as contrasena,nombre,puesto,lugar_trabajo,lugar_trabajo_num,nivel FROM usuarios WHERE idusuarios=" + id_user);

                if (datos_usuario.Rows.Count > 0)
                {
                    textBox1.Text = datos_usuario.Rows[0][2].ToString();
                    textBox2.Text = datos_usuario.Rows[0][3].ToString();
                    textBox3.Text = datos_usuario.Rows[0][0].ToString();
                    textBox4.Text = datos_usuario.Rows[0][1].ToString();
                    textBox5.Text = datos_usuario.Rows[0][5].ToString();

                    comboBox1.SelectedIndex = (Convert.ToInt32(datos_usuario.Rows[0][4].ToString()));

                    if (Convert.ToInt32(datos_usuario.Rows[0][6].ToString()) == 0)
                    {
                        comboBox2.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Usuario Inexistente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

            }
        }
    
    }
}

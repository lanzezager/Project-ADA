/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 19/01/2022
 * Hora: 11:48 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
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
    class Super_conexion
    {
        String cad_con, cado_con, bd_load = "", servi_ = "", user_ = "", pass_ = "", fecha, user, bd_user, bd_pass, bd_server, bd_base;

        //Declaración de los Elementos de la BD
        MySqlDataAdapter adaptador;
        DataTable fuente;
        DataSet Datazet = new DataSet();
        MySqlConnection con;
        MySqlConnection con_test;

        /// <summary>Conecta a la Base de Datos de MySQL.</summary>
        /// <param name="bd">Nombre de la Base de Datos a la que se realizará la conexión.</param>
        /// <param name="servi">Direccion al servidor al que se realizará la conexión.</param>
        /// <param name="usuario">Nombre de usuario que realizará la conexión.</param>
        /// <param name="password">Contraseña del usuario que realizará la conexión</param>
        public void conectar(String bd, String servi, String usuario, String password)
        {
            this.bd_load = bd;
            this.servi_ = servi;
            this.user_ = usuario;
            this.pass_ = password;

            cad_con = leer_config();
            //cad_con = @"server=localhost; userid=lanzezager; password=mario; database="+bd+"";

            try
            {
                con = new MySqlConnection(cad_con);
                con.Open();
                //MessageBox.Show("MySQL version : "+ con.Database);

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }

        }

        public bool probar(String cad)
        {
            cad_con = cad;

            try
            {
                con_test = new MySqlConnection(cad_con);
                con_test.Open();

                // MessageBox.Show("MySQL version : "+ con_test.Database);
                //MessageBox.Show( "tiempo: "+ con_test.GetLifetimeService().ToString());
                // MessageBox.Show("tiempo2:"+con_test.ConnectionTimeout.ToString());
                con_test.Close();
                return true;

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                return false;
            }
        }

        /// <summary>Realiza una Consulta a la Base de Datos de MySQL.</summary>
        /// <param name="sql">Cadena con la sentencia SQL que se efectuará </param>
        /// <returns>Regresa un DataTable con los datos de la consulta. Si NO se utilizó la Sentencia SQL: SELECT no regresará Nada</returns>
        public DataTable consultar(String sql)
        {
            try
            {
                adaptador = new MySqlDataAdapter(sql, con);
                Datazet.Reset();
                if (!(adaptador.Equals(null)))
                {
                    fuente = new DataTable();
                    adaptador.Fill(Datazet);
                    try
                    {
                        fuente = Datazet.Tables[0];
                    }
                    catch (IndexOutOfRangeException ex)
                    {

                    }
                }
                return fuente;
            }
            catch (Exception error)
            {
                if (sql.Contains("contrasena") == true)
                {
                    sql = "";
                }
                MessageBox.Show("Ocurrió un error durante la conexion a la Base de Datos:\n" + error + "\n\n Al realizar esta operacion:\n" + sql, "ERROR");
                return fuente;
            }
        }

        public void cerrar()
        {
            con.Close();
        }

        public void guardar_evento(String evento)
        {

            user = MainForm.datos_user_static[0];
            fecha = (System.DateTime.Today.Year.ToString() +
                    "-" + System.DateTime.Today.Month.ToString() +
                    "-" + System.DateTime.Today.Day.ToString() +
                    " " + System.DateTime.Now.Hour.ToString() +
                    ":" + System.DateTime.Now.Minute.ToString() +
                    ":" + System.DateTime.Now.Second.ToString());
            //MessageBox.Show(user);
            //conectar("base_principal");
            consultar("INSERT INTO log_eventos (dia_hora,evento,usuario) VALUES (\"" + fecha + "\",\"" + evento + "\",\"" + user + "\" )");

        }

        public String linea_evento(String evento)
        {
            String linea;
            user = MainForm.datos_user_static[0];
            fecha = (System.DateTime.Today.Year.ToString() +
                    "-" + System.DateTime.Today.Month.ToString() +
                    "-" + System.DateTime.Today.Day.ToString() +
                    " " + System.DateTime.Now.Hour.ToString() +
                    ":" + System.DateTime.Now.Minute.ToString() +
                    ":" + System.DateTime.Now.Second.ToString());
            //MessageBox.Show(user);

            linea = "INSERT INTO log_eventos (dia_hora,evento,usuario) VALUES (\"" + fecha + "\",\"" + evento + "\",\"" + user + "\" )";
            return linea;
        }

        public String leer_config()
        {
            try
            {
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

                if (bd_load.Length > 1)
                {
                    cado_con = @"server=" + servi_ + "; userid=" + user_ + "; password=" + pass_ + "; database=" + bd_load + "";
                }
                else
                {
                    cado_con = @"server=" + bd_server + "; userid=" + bd_user + "; password=" + bd_pass + "; ";
                }

                //MessageBox.Show(cado_con);
                /*String ruta;
                ruta = cado_con;
                ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';')-1) - (ruta.IndexOf('='))));
                ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
                MessageBox.Show(ruta);*/
                return cado_con;
            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }

        }

        public string[] leer_config_2()
        {
            string[] datos_conex = new string[4];
            try
            {
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

                datos_conex[0] = bd_user;
                datos_conex[1] = bd_pass;
                datos_conex[2] = bd_server;
                datos_conex[3] = bd_base;
                cado_con = @"server=" + bd_server + "; userid=" + bd_user + "; password=" + bd_pass + "; database=" + bd_load + "";
                //MessageBox.Show(cado_con);
                /*String ruta;
                ruta = cado_con;
                ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';')-1) - (ruta.IndexOf('='))));
                ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
                MessageBox.Show(ruta);*/
                return datos_conex;
            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de MySQL", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return datos_conex;
            }

        }

        public string[] leer_config_sub()
        {

            String del, no_del, muni, sub, no_sub, subdele, jefe_afi, jefe_cob, jefe_emi, sec_emi, jefe_cobro, dias_not, info;

            string[] datos_ubicacion = new string[12];
            try
            {
                StreamReader rdr1 = new StreamReader(@"sub_config.lz");
                del = rdr1.ReadLine();
                no_del = rdr1.ReadLine();
                muni = rdr1.ReadLine();
                sub = rdr1.ReadLine();
                no_sub = rdr1.ReadLine();
                subdele = rdr1.ReadLine();
                jefe_afi = rdr1.ReadLine();
                jefe_cob = rdr1.ReadLine();
                jefe_emi = rdr1.ReadLine();
                sec_emi = rdr1.ReadLine();
                jefe_cobro = rdr1.ReadLine();
                dias_not = rdr1.ReadLine();
                rdr1.Close();

                del = del.Substring(11, del.Length - 11);
                no_del = no_del.Substring(7, no_del.Length - 7);
                muni = muni.Substring(10, muni.Length - 10);
                sub = sub.Substring(14, sub.Length - 14);
                no_sub = no_sub.Substring(7, no_sub.Length - 7);
                subdele = subdele.Substring(12, subdele.Length - 12);
                jefe_afi = jefe_afi.Substring(13, jefe_afi.Length - 13);
                jefe_cob = jefe_cob.Substring(9, jefe_cob.Length - 9);
                jefe_emi = jefe_emi.Substring(9, jefe_emi.Length - 9);
                sec_emi = sec_emi.Substring(14, sec_emi.Length - 14);
                jefe_cobro = jefe_cobro.Substring(12, jefe_cobro.Length - 12);
                dias_not = dias_not.Substring(9, dias_not.Length - 9);

                datos_ubicacion[0] = del;
                datos_ubicacion[1] = no_del;
                datos_ubicacion[2] = muni;
                datos_ubicacion[3] = sub;
                datos_ubicacion[4] = no_sub;
                datos_ubicacion[5] = subdele;
                datos_ubicacion[6] = jefe_afi;
                datos_ubicacion[7] = jefe_cob;
                datos_ubicacion[8] = jefe_emi;
                datos_ubicacion[9] = sec_emi;
                datos_ubicacion[10] = jefe_cobro;
                datos_ubicacion[11] = dias_not;
                //info = del+"|"+no_del+"|"+muni+"|"+sub+"|"+no_sub;
                //MessageBox.Show(info);
                /*String ruta;
                ruta = cado_con;
                ruta = ruta.Substring((ruta.IndexOf('=')) + 1, ((ruta.IndexOf(';')-1) - (ruta.IndexOf('='))));
                ruta = @"\\" + ruta + @"\Nova_Gear\Recursos\Imagenes\Usuarios\";
                MessageBox.Show(ruta);*/
                return datos_ubicacion;
            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al leer el archivo de configuración de ubicación", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return datos_ubicacion;
            }
        }

        public void guardar_config_sub(string[] datos)
        {
            String ruta;
            FileStream fichero;

            try
            {
                //Borrar archivos para comenzar de 0
                if (File.Exists(@"sub_config.lz") == true)
                {
                    System.IO.File.Delete(@"sub_config.lz");

                    //Crear archivos nuevos
                    fichero = System.IO.File.Create(@"sub_config.lz");

                    ruta = fichero.Name;
                    fichero.Close();

                    //Abrir archivo
                    StreamWriter wr = new StreamWriter(@"sub_config.lz");
                    wr.WriteLine("delegacion:" + datos[0]);
                    wr.WriteLine("no_del:" + datos[1]);
                    wr.WriteLine("municipio:" + datos[2]);
                    wr.WriteLine("subdelegacion:" + datos[3]);
                    wr.WriteLine("no_sub:" + datos[4]);
                    wr.WriteLine("subdelegado:" + datos[5]);
                    wr.WriteLine("jefe_afi_vig:" + datos[6]);
                    wr.WriteLine("jefe_cob:" + datos[7]);
                    wr.WriteLine("jefe_epo:" + datos[8]);
                    wr.WriteLine("jefe_secc_epo:" + datos[9]);
                    wr.WriteLine("jefe_cobros:" + datos[10]);
                    wr.WriteLine("dias_not:" + datos[11]);
                    wr.WriteLine("DON'T CHANGE THIS SETTINGS!!!!!");
                    wr.WriteLine("By LZ");
                    wr.WriteLine("Arriba el Atlas!!!!!");
                    wr.Close();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Ha ocurrido un error al guardar la configuración", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
	
    }

}

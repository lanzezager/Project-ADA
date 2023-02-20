using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Threading;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace Nova_Gear.Depuracion
{
    public partial class Carga_procesar : Form
    {
        public Carga_procesar()
        {
            InitializeComponent();
        }

        Conexion conex = new Conexion();
        DataTable anti_dupli = new DataTable();

        String fecha_ulti="", fecha_ingreso="";
        int i = 0;



        //Declaracion de Hilo
        private Thread hilosecundario = null;

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog open_d = new OpenFileDialog();
            open_d.Filter = "Archivo de PROCESAR (*.T;*.TXT;*.C001)|*.T;*.TXT;*.C001";
            open_d.Title = "Seleccione el archivo PROCESAR";

            if (open_d.ShowDialog() == DialogResult.OK)
            {
                lectura_procesar(open_d.FileName);                
            }
        }

        public void lectura_procesar(String ruta_file)
        {
            String linea = "",reg_pat="",folio="",periodo="",fecha="",c4ss="",rcv="", fecha_ante="",diag1="";
            int error = 0,numdiag1=0;
            
            StreamReader t1 = new StreamReader(ruta_file);
            conex.conectar("base_principal");

            while(t1.EndOfStream!=true){
                linea= t1.ReadLine();

                string[] columnas = linea.Split(',');

                reg_pat = columnas[1];
                folio = columnas[2];
                periodo = columnas[3];
                fecha = columnas[4];
                c4ss = columnas[5];
                rcv = columnas[6];
                diag1 = columnas[8];
                diag1 = diag1.Trim();

                c4ss = arregla_numeros(c4ss);
                rcv = arregla_numeros(rcv);

                if (int.TryParse(diag1, out numdiag1) == true)
                {
                    numdiag1 = Convert.ToInt32(diag1);
                }
                else
                {
                    numdiag1 = 0;
                }

                
                fecha = fecha.Insert(4, "-");
                fecha = fecha.Insert(7, "-");
                

                //if(fecha_ante != fecha){
                    
                    anti_dupli = conex.consultar("SELECT id FROM procesar where reg_pat=\"" + reg_pat + "\" AND f_sua=\"" + folio + "\" AND periodo_pago=\"" + periodo + "\" AND fecha_pago=\""+fecha+"\" AND 4ss="+c4ss+" AND rcv = "+rcv+"");
                    
                    //MessageBox.Show(fecha_ante + ", " + fecha);
                    //fecha_ante = fecha;
                    //MessageBox.Show(fecha_ante + ", " + fecha);
                //}

                //if ((Convert.ToDateTime(fecha_ulti)) < (Convert.ToDateTime(fecha)))
                if(anti_dupli.Rows.Count==0)
                {
                    i++;
                    dataGridView1.Rows.Add(i, reg_pat, folio, periodo, fecha, c4ss, rcv,numdiag1,fecha_ingreso);
                }
                else
                {
                    //error = 1;
                    //break;
                }
             }

            conex.cerrar();
            button2.Enabled = true;

            label3.Text = "Registros Totales: "+dataGridView1.Rows.Count;
            if(error==0){
                MessageBox.Show("Lectura Exitosa del Archivo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }else{
                MessageBox.Show("El archivo Procesar que intenta ingresar ya fue ingresado previamente.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public String arregla_numeros(String origen)
        {
            int importe=0;
            String destino=""; 

            if(int.TryParse(origen, out importe) == true){

                importe = Convert.ToInt32(origen);

                if (importe > 0)
                {
                    destino = importe.ToString();

                    if (destino.Length > 1)
                    {
                        destino = destino.Insert((destino.Length - 2), ".");
                    }
                    else
                    {
                        destino = destino.Insert(0, ".");
                        destino = "0" + destino + "0";
                    }

                    if(destino.Length==3){
                        destino = "0" + destino;
                    }
                }
                else
                {
                    destino = "0.00";
                }
            }
            else
            {
                destino = "0.00"; 
            }

            return destino;
        }

        public void guarda_procesar()
        {
            conex.conectar("base_principal");
            String sql = "INSERT INTO procesar (reg_pat,f_sua,periodo_pago,fecha_pago,4ss,rcv,diag1,fecha_ingreso) VALUES ";
            String sql2 = "";

            int tot_temp = 0;
            int k = 0;
            int tot_guar = 0;

            while (k < dataGridView1.Rows.Count)
            {

                sql2 += " (\"" + dataGridView1[1, k].Value.ToString() + "\",\"" + dataGridView1[2, k].Value.ToString() + "\",\"" + dataGridView1[3, k].Value.ToString() + "\",\"" + dataGridView1[4, k].Value.ToString() + "\"," + dataGridView1[5, k].Value.ToString() + "," + dataGridView1[6, k].Value.ToString() + "," + dataGridView1[7, k].Value.ToString() + ",\"" + dataGridView1[8, k].Value.ToString() + "\"),";

                if (tot_temp == 100)
                {
                    sql2 = sql2.Substring(0, sql2.Length - 1);
                    conex.consultar(sql + sql2);
                    tot_temp = 0;
                    sql2 = "";
                }

                tot_temp++;
                k++;

                Invoke(new MethodInvoker(delegate
                {
                    tot_guar = Convert.ToInt32(((k * 100) / dataGridView1.Rows.Count));
                    toolStripStatusLabel1.Text = tot_guar + "%";
                    if (tot_guar < 101)
                    {
                        toolStripProgressBar1.Value = tot_guar;
                    }
                }));
            }

            sql2 = sql2.Substring(0, sql2.Length - 1);
            conex.consultar(sql + sql2);            
            conex.cerrar();

            MessageBox.Show("Se Ingreso exitosamente el archivo PROCESAR con " + k + " casos.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Invoke(new MethodInvoker(delegate
                {
                    this.Close();
                }));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult resu = MessageBox.Show("¿Desea Ingresar el archivo PROCESAR leído?","AVISO",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1);

            if (resu == DialogResult.Yes)
            {
                hilosecundario = new Thread(new ThreadStart(guarda_procesar));
                hilosecundario.Start();                
            }
        }

        private void Carga_procesar_Load(object sender, EventArgs e)
        {
            /*
            conex.conectar("base_principal");
            anti_dupli = conex.consultar("SELECT fecha_pago FROM procesar ORDER BY fecha_pago DESC LIMIT 1");
            conex.cerrar();
            fecha_ulti = anti_dupli.Rows[0][0].ToString().Substring(0,10);
            fecha_ulti = fecha_ulti.Substring(6, 4) + "-" + fecha_ulti.Substring(3, 2) + "-" + fecha_ulti.Substring(0, 2);
            */
            //MessageBox.Show(fecha);
            fecha_ingreso = DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();
            
            conex.conectar("base_principal");
            anti_dupli = conex.consultar("SELECT fecha_ingreso FROM procesar order by fecha_ingreso desc limit 1");
            fecha_ulti = anti_dupli.Rows[0][0].ToString().Substring(0,10);
            conex.cerrar();

            //
            //MessageBox.Show("f_hoy: "+fecha_ingreso+" \n fecha_ulti:"+fecha_ulti);

            if(fecha_ingreso==fecha_ulti){
                MessageBox.Show("El archivo PROCESAR correspondiente al dia de hoy ya fue ingresado.\nNo se recomienda ingresar mas de 1 archivo al día.", "¡ATENCION!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //this.Close();
            }

            fecha_ingreso = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString();
        }
    
    }
}

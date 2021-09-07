/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 30/03/2016
 * Hora: 10:35 a. m.
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
using System.Threading;

namespace Nova_Gear
{
    public partial class Sicofi : Form
    {
        public Sicofi()
        {
            InitializeComponent();
        }

        String nom_archivo,linea,texto,reg_pat,raz_soc,folio,fecha_mov,periodo,credito,importe,sql,anterior;
        string[] palabra; 
        int i=0,j=0,k=0,l=0,captura=0,salto=0,tot_lin=0;
        char espacio = ' ';
        
        //Declaracion de elementos para conexion mysql
        Conexion conex = new Conexion();
        DataTable consultamysql = new DataTable();

        private void Sicofi_Load(object sender, EventArgs e)
        {

            String window_name = this.Text;
            window_name = window_name.Replace("Nova Gear", "Gear Prime");
            this.Text = window_name;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Texto(*.Txx)|*.T*"; //le indicamos el tipo de filtro en este caso que busque        
            dialog.Title = "Seleccione el archivo que contiene los Folios SICOFI";//le damos un titulo a la ventana
        
            dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
			
			//si al seleccionar el archivo damos Ok
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = dialog.SafeFileName;
                nom_archivo = dialog.FileName;
                MessageBox.Show("Archivo Abierto","AVISO");
            }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader t4 = new StreamReader(nom_archivo);
            conex.conectar("base_principal");
			//Lectura linea por linea
            textBox1.Text = "";
            while (!t4.EndOfStream)
            {
                linea = t4.ReadLine();
                anterior = "";
                texto = "";
                tot_lin++;
                palabra = linea.Split(espacio);
                    for (i = 0; i < palabra.Length; i++)
                    {
                        if (salto == 0)
                        {
                          if (!palabra[i].Equals(""))
                            {
                                anterior = texto;
                                texto = palabra[i];

                                if (captura == 2)
                                {
                                    if (k == 0)
                                    {
                                        periodo = texto;
                                    }

                                    if (k == 1)
                                    {
                                        credito = texto;
                                    }

                                    if (k == 2)
                                    {
                                        importe = texto;
                                        k = 0;
                                        captura = 0;
                                        do{
                                            raz_soc = raz_soc.Remove(raz_soc.Length - 1, 1);
                                        }while(!raz_soc.EndsWith(" "));

                                        textBox1.AppendText("INSERT INTO sicofi (registro_patronal,razon_social,periodo,credito,importe_credito,fecha_mov,folio_sicofi)" +
                                       "VALUES (\"" + reg_pat + "\",\"" + raz_soc + "\",\"" + periodo + "\",\"" + credito + "\",\"" + importe + "\",\"" + fecha_mov + "\",\"" + folio + "\");\r\n");
                                    }
                                    k++;
                                }

                                if ((captura == 1))
                                {
                                    if (k == 1)
                                    {
                                        folio = texto;
                                    }

                                    if (k == 5)
                                    {
                                        fecha_mov = texto;
                                    }

                                    if (k == 7)
                                    {
                                        periodo = texto;
                                    }

                                    if (k == 8)
                                    {
                                        credito = texto;
                                    }

                                    if (k == 9)
                                    {
                                        importe = texto;
                                        k = 0;
                                        captura = 2;
                                        do
                                        {
                                            raz_soc = raz_soc.Remove(raz_soc.Length - 1, 1);
                                        } while (!raz_soc.EndsWith(" "));

                                        textBox1.AppendText("INSERT INTO sicofi (registro_patronal,razon_social,periodo,credito,importe_credito,fecha_mov,folio_sicofi)" +
                                       "VALUES (\"" + reg_pat + "\",\"" + raz_soc + "\",\"" + periodo + "\",\"" + credito + "\",\"" + importe + "\",\"" + fecha_mov + "\",\"" + folio + "\");\n\r");
                                    }
                                    k++;
                                }

                                if (anterior.Equals("PATR.:"))
                                {
                                    reg_pat = texto;
                                    //MessageBox.Show(reg_pat);
                                }

                                if (anterior.Equals("PATRON:"))
                                {
                                    j = i;
                                    raz_soc = "";

                                    do
                                    {
                                        raz_soc += palabra[j] + " ";
                                        j++;
                                    } while (j<palabra.Length);

                                    captura = 1;
                                    salto = 1;
                                }

                                if (texto.Equals("FOLIO"))
                                {
                                    captura = 1;
                                    salto = 1;
                                }

                            }//fin if de espacios
                        }
                    }//fin for
                    salto = 0;
            }//Fin lectura archivo
        }//fin evento click_boton
    }
}

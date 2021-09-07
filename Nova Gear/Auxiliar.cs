/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 03/06/2015
 * Hora: 10:59 a. m.
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

namespace Nova_Gear
{
	/// <summary>
	/// Description of Auxiliar.
	/// </summary>
	public class Auxiliar
	{
		public Auxiliar()
		{
		}
		 
		 DataSet datadata = new DataSet();
		 
		 public void receptor(DataSet data){
		 	datadata = data;
		 	
		 	
		 }
	}
}

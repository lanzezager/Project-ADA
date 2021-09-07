/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 30/04/2015
 * Hora: 11:27 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Windows.Forms;

namespace Nova_Gear
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		
		[STAThread]
		private static void Main(string[] args)
		{
            
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            
			Application.Run(new Pantalla());
		}
		
	}
}

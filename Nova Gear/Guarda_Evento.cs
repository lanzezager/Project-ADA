/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 22/04/2016
 * Hora: 02:57 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;

namespace Nova_Gear
{
	/// <summary>
	/// Description of Guarda_Evento.
	/// </summary>
	public class Guarda_Evento
	{
		public Guarda_Evento()
		{
		}
		
		Conexion conex = new Conexion();
		String fecha,user;
		
		public void obtener_user(String usu){
			user=usu;
		}
		
		public void guardar(String evento){
			
			fecha = (System.DateTime.Today.Year.ToString()+
			        "-"+System.DateTime.Today.Month.ToString()+
			        "-"+System.DateTime.Today.Day.ToString()+
			        " "+System.DateTime.Now.Hour.ToString()+
			        ":"+System.DateTime.Now.Minute.ToString());
			
			conex.conectar("base_principal");
			conex.consultar("INSERT INTO log_eventos (dia_hora,evento,usuario) VALUES (\""+fecha+"\",\""+evento+"\",\""+user+"\" )");
			
		}
	}
}

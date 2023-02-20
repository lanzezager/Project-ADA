'
' * Creado por SharpDevelop.
' * Usuario: Lanze Zager
' * Fecha: 30/04/2015
' * Hora: 11:48 a. m.
' * 
' * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
' 

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Data.OleDb

''' <summary>
''' Description of Conexion.
''' </summary>
Public Class Conexion
			'this.sql = sql;
	Public Sub New()
	End Sub

	Private cad_con As [String]

	'Declaración de los Elementos de la BD
	Private adaptador As MySqlDataAdapter
	Private fuente As DataTable
	Private DataSet As New DataSet()
	Private con As MySqlConnection


	Public Sub conectar(bd As [String])

		cad_con = "server=localhost; userid=lanzezager; password=mario; database=" & bd & ""

		Try

			con = New MySqlConnection(cad_con)
				'MessageBox.Show("MySQL version : "+ con.Database);


			con.Open()
		Catch ex As MySqlException

			MessageBox.Show("Error: " & ex.ToString())
		End Try

	End Sub

	Public Sub consultar(sql As [String])
		adaptador = New MySqlDataAdapter(sql, con)
		DataSet.Reset()
		fuente = New DataTable()
		adaptador.Fill(DataSet)
	End Sub

End Class

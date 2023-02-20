'
' * Creado por SharpDevelop.
' * Usuario: Lanze Zager
' * Fecha: 30/04/2015
' * Hora: 11:27 a. m.
' * 
' * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
' 

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports MySql.Data.MySqlClient


''' <summary>
''' Description of MainForm.
''' </summary>
Public Partial Class MainForm
	Inherits Form
	Public Sub New()
		'
		' The InitializeComponent() call is required for Windows Forms designer support.
		'


			'Conexion conect = new Conexion();

			'
			' TODO: Add constructor code after the InitializeComponent() call.
			'
		InitializeComponent()
	End Sub

	'String con = @"server=localhost; userid=lanzezager; password=mario; database=base_principal";



	Private Sub LectorDeFacturasToolStripMenuItemClick(sender As Object, e As EventArgs)

		Dim form1 As New Lector_Fac()
		form1.MdiParent = Me
		form1.Show()
	End Sub
End Class

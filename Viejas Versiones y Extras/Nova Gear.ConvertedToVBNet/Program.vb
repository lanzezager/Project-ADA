'
' * Creado por SharpDevelop.
' * Usuario: Lanze Zager
' * Fecha: 30/04/2015
' * Hora: 11:27 a. m.
' * 
' * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
' 

Imports System.Windows.Forms

''' <summary>
''' Class with program entry point.
''' </summary>
Friend NotInheritable Class Program
	''' <summary>
	''' Program entry point.
	''' </summary>

	<STAThread> _
	Friend Shared Sub Main(args As String())
		Application.EnableVisualStyles()
		Application.SetCompatibleTextRenderingDefault(False)
		Application.Run(New MainForm())
	End Sub

End Class

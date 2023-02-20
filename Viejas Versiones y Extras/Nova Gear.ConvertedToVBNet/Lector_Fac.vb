'
' * Creado por SharpDevelop.
' * Usuario: Lanze Zager
' * Fecha: 30/04/2015
' * Hora: 11:47 a. m.
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
Imports System.Threading

''' <summary>
''' Description of Lector_Fac.
''' </summary>
Public Partial Class Lector_Fac
	Inherits Form
	Public Sub New()
		'
		' The InitializeComponent() call is required for Windows Forms designer support.
		'
			'this.cad_con = con;


			'
			' TODO: Add constructor code after the InitializeComponent() call.
			'
		InitializeComponent()
	End Sub

	'Declaración de Variables
	Private ruta As [String], linea As [String], porcent_text As [String], sector_n As [String], reg_pat As [String], ra_soc As [String], _
		periodo As [String], temp_peri As [String], temp_peri1 As [String], cred_cuo As [String], sql As [String], hoja As [String], _
		cad_con_ofis As [String]
	Private reg_pat1 As [String], reg_pat2 As [String], cred_mul As [String], cuota_omi As [String], cuota_ac As [String], recargo As [String], _
		imp_multa As [String], imp_total As [String], linea2 As [String], linea_vac As [String], archivo As [String], tabla As [String], _
		ext As [String], nvatabla As [String]
	Private nombre_per As [String], nombre_periodo As [String], municipio As [String], delegacion As [String], subdele As [String], domicilio As [String], _
		giro As [String], localidad As [String], marca As [String], subde As [String], folio As [String], cons_ofis As [String], _
		col As [String]
	Private cons_tabla As [String], num_cred As [String], num_trab As [String], lineatbx2 As [String]
	Private palabra As String(), reg_part As String(), text As String(), aux As String()
	Private tot_lin As Integer = 0, porcent As Integer = 0, i As Integer = 0, j As Integer = 0, k As Integer = 0, bandera As Integer = 0, _
		contador As Integer = 0, bandera2 As Integer = 0, bandera3 As Integer = 0, contador_lin As Integer = 0, rcv As Integer = 0
	Private tipo_file As Integer = 0, gancho As Integer = 0, ver As Integer = 0, lin_var As Integer = 0, bloque As Integer = 0, finbloque As Integer = 0, _
		[sub] As Integer = 0, filas As Integer = 0, tipocarga As Integer = 0, tot_row As Integer = 0, tot_col As Integer = 0
	Private tot1 As Single = 0, tot2 As Single = 0, por As Single = 0
	Private cuo_o As Double = 0, cuo_a As Double = 0, rec As Double = 0, imp_mul As Double = 0, imp_tot As Double = 0
	Private espacio As Char = " "C


	Private conex As New Conexion()

	'Declaracion de elementos para conexion office
	Private conexion As OleDbConnection = Nothing
	Private dataSet As DataSet = Nothing
	Private dataAdapter As OleDbDataAdapter = Nothing

	'Declaracion del Delegado y del Hilo para ejecutar un subproceso
	Private Delegate Sub SetTextCallback(text As [String])
	Private Delegate Sub SetTextCallback2(text2 As [String])
	Private Delegate Sub SetTextCallback1(progreso As Integer)


	Private hilosecundario As Thread = Nothing



	Private Sub SetText(text As [String])
		' InvokeRequired required compares the thread ID of the
		' calling thread to the thread ID of the creating thread.
		' If these threads are different, it returns true.
		If Me.textBox1.InvokeRequired Then
			Dim d As New SetTextCallback(AddressOf SetText)
			Me.Invoke(d, New Object() {text})
		Else
			Me.textBox1.AppendText(text)
		End If
	End Sub

	Private Sub SetText1(progreso As Integer)
		' InvokeRequired required compares the thread ID of the
		' calling thread to the thread ID of the creating thread.
		' If these threads are different, it returns true.
		If Me.textBox1.InvokeRequired Then
			Dim d1 As New SetTextCallback1(AddressOf SetText1)
			Me.Invoke(d1, New Object() {progreso})
		Else
			progressBar1.Value = progreso
			porcent_text = Convert.ToString(progreso)
			label1.Text = porcent_text & "%"
			label1.Refresh()
		End If
	End Sub

	Private Sub SetText2(text2 As [String])
		' InvokeRequired required compares the thread ID of the
		' calling thread to the thread ID of the creating thread.
		' If these threads are different, it returns true.
		If Me.textBox1.InvokeRequired Then
			Dim d2 As New SetTextCallback(AddressOf SetText2)
			Me.Invoke(d2, New Object() {text2})
		Else
			Me.textBox2.AppendText(text2)
		End If
	End Sub

	Public Sub opciones()

		'***********COP***********
		If (radioButton1.Checked) AndAlso (radioButton7.Checked) Then
			nombre_per = "ECOEST"
			tipo_file = 1
		End If

		If (radioButton1.Checked) AndAlso (radioButton8.Checked) Then
			nombre_per = "ECOMEC"
			tipo_file = 1
		End If

		If (radioButton1.Checked) AndAlso (radioButton9.Checked) Then
			nombre_per = "SIVEPAANU"
			tipo_file = 1
		End If

		If (radioButton1.Checked) AndAlso (radioButton10.Checked) Then
			nombre_per = "SIVEPAEXT"
			tipo_file = 1
		End If

		If (radioButton1.Checked) AndAlso (radioButton11.Checked) Then
			nombre_per = "SIVEPAOPO"
			tipo_file = 1
		End If

		'***********RCV***********
		If (radioButton2.Checked) AndAlso (radioButton7.Checked) Then
			nombre_per = "ECOEST"
			rcv = 1
			tipo_file = 1
		End If

		If (radioButton2.Checked) AndAlso (radioButton8.Checked) Then
			nombre_per = "ECOMEC"
			rcv = 1
			tipo_file = 1
		End If

		If (radioButton2.Checked) AndAlso (radioButton9.Checked) Then
			nombre_per = "SIVEPAANU"
			rcv = 1
			tipo_file = 1
		End If

		If (radioButton2.Checked) AndAlso (radioButton10.Checked) Then
			nombre_per = "SIVEPAEXT"
			rcv = 1
			tipo_file = 1
		End If

		If (radioButton2.Checked) AndAlso (radioButton11.Checked) Then
			nombre_per = "SIVEPAOPO"
			rcv = 1
			tipo_file = 1
		End If

		'EMA - EBA ********************
		If radioButton3.Checked Then
			tipo_file = 2
		End If

		'Multa(oficio) - Carga Manual******
		If radioButton5.Checked Then
			tipo_file = 4
		End If



	End Sub

	Public Sub inicio()

		Try

			'Comienza Lectura
			Dim t4 As New StreamReader(ruta)
			sector_n = ""

			'Lectura linea por linea
			While Not t4.EndOfStream

				linea = t4.ReadLine()
				'linea = linea+"\n";
				'  Ejemplo: Mostrar datos de los eventos
				' enviados por el Thread en una ListBox
				Invoke(New MethodInvoker(Sub() textBox1.AppendText(linea & vbLf)))
				'textBox1.AppendText(linea+"\n");
				'SetText(linea);
				tot_lin += 1
				tot2 = tot_lin
				por = ((tot2 * 100) / tot1)
				porcent = Convert.ToInt32(por)

				Select Case tipo_file

					Case 1
						'busca lineas vacias
						If linea.Length <= 8 Then
							bandera = 0
						End If

						'llama a la funcion que descifra la linea
						descomponer_linea_cop()

						'llama a la funcion que guarda la linea descrifrada
						capturar_datos_cop()
						Exit Select

					Case 4
						'llama a la funcion que descifra la linea
						descomponer_linea_multa()
						Exit Select
					Case Else

						MessageBox.Show("Hola :D")
						Exit Select
				End Select

				If por < 99 Then
					Invoke(New MethodInvoker(Sub() 
					progressBar1.Value = porcent
					porcent_text = Convert.ToString(porcent)
					label1.Text = porcent_text & "%"
					label1.Refresh()

						'SetText1(porcent);
End Sub))
				End If
			End While

			If (tipo_file = 1) OrElse (tipo_file = 4) Then
				insertar_datos()

			End If
		Catch ex As Exception
			MessageBox.Show("ERROR: " & vbLf & Convert.ToString(ex), "Error al Procesar")
		End Try
	End Sub

	Public Sub leer_linea()

		Try
			'Controlar la extension del archivo dependiendo del tipo de archivo a leer

			Select Case tipo_file

				Case 1
					openFileDialog1.Filter = "Archivo de Factura (*.T;*.TXT)|*.T;*.TXT"
					openFileDialog1.Title = "Seleccione el archivo COP"
					'le damos un titulo a la ventana
					Exit Select

				Case 4
					openFileDialog1.Filter = "Archivo de Oficio (*.T;*.TXT;*.R)|*.T;*.TXT;*.R"
					openFileDialog1.Title = "Seleccione el archivo RCV"
					'le damos un titulo a la ventana
					Exit Select
				Case Else

					openFileDialog1.Filter = "Archivo de Factura (*.T;*.TXT)|*.T;*.TXT"
					openFileDialog1.Title = "Seleccione el archivo de Multa"
					'le damos un titulo a la ventana
					Exit Select

			End Select

			If openFileDialog1.ShowDialog() = DialogResult.OK Then
				'Inicializar variables
				textBox1.Text = ""
				tot_lin = 0
				progressBar1.Value = 0

				'Lectura Previa del archivo para conocer su longitud
				ruta = openFileDialog1.FileName
				Dim t3 As New StreamReader(ruta)
				textBox1.Text = t3.ReadToEnd()
				aux = textBox1.Lines
				tot1 = aux.Length
				t3.Close()
				textBox1.Text = ""
				label2.Text = "Archivo:  " & openFileDialog1.SafeFileName

				MessageBox.Show("Archivo Abierto")

			End If
		Catch ex As Exception
			MessageBox.Show("ERROR: " & vbLf & Convert.ToString(ex), "Error al Abrir")
		End Try
		button5.Enabled = True

	End Sub

	Public Sub descomponer_linea_cop()

		linea_vac = linea.Substring(1, 7)

		If linea_vac.Equals("1438501") Then
			'label4.Text ="";

			If (linea.Length) > 90 Then
				'label2.Text = linea.Substring(22,6);

				'Extraer Sector de Notificación
				If (linea.Substring(131, 6)).Equals("SECTOR") Then
					sector_n = linea.Substring(158, 2)
				End If

				'Extraer Linea de Datos
				If (bandera = 1) OrElse (bandera3 = 1) Then

					'Extraer la 1ra Parte de los Datos
					reg_pat = linea.Substring(15, 14)
					ra_soc = linea.Substring(31, 35)
					periodo = linea.Substring(68, 7)
					cred_cuo = linea.Substring(77, 9)
					cred_mul = linea.Substring(88, 9)

					'Extraer la 2da Parte de los Datos
					linea2 = linea.Substring(97, 76)
					palabra = linea2.Split(espacio)
					contador = 0

					For i = 0 To palabra.Length - 1

						If Not palabra(i).Equals("") Then

							If contador = 0 Then
								cuota_omi = palabra(i)
							End If
							If contador = 1 Then
								cuota_ac = palabra(i)
							End If
							If contador = 2 Then
								recargo = palabra(i)
							End If
							If contador = 3 Then
								imp_multa = palabra(i)
							End If
							If contador = 4 Then
								imp_total = palabra(i)
							End If

							contador = contador + 1
							bandera2 = 1
						End If
					Next
				End If

				'Activar Bandera
				If linea.Length > 90 Then
					If (linea.Substring(15, 4)).Equals("REG.") Then
						bandera = 1
					End If
				End If
			End If
		End If

	End Sub

	Public Sub capturar_datos_cop()

		If (radioButton1.Checked) OrElse (radioButton2.Checked) Then
			opciones()
		End If

		If bandera2 = 1 Then
			'Formateo de informacion antes de ingresarla a la BD
			temp_peri = periodo.Substring(3, 4)
			temp_peri1 = periodo.Substring(0, 2)
			periodo = temp_peri & temp_peri1
			ra_soc = ra_soc.TrimEnd(" "C)
			'ra_soc=ra_soc.Replace('\'',' ');

			'Descifrar registro patronal 1 y 2
			reg_part = reg_pat.Split("-"C)
			For j = 0 To reg_part.Length - 1
				If Not reg_part(j).Equals("") Then
					reg_pat1 += reg_part(j)
				End If
			Next

			reg_pat2 = reg_pat1.Substring(0, 10)
			'MessageBox.Show("VARIABLES: " +cuota_omi+","+cuota_ac+","+recargo+","+imp_multa+","+imp_total);


			'Converion de datos numericos al tipo correcto
			cuo_o = Convert.ToDouble(cuota_omi)
			cuo_a = Convert.ToDouble(cuota_ac)
			rec = Convert.ToDouble(recargo)
			imp_mul = Convert.ToDouble(imp_multa)
			imp_tot = Convert.ToDouble(imp_total)

			If sector_n = "0" Then
				sector_n = "00"
			End If

			nombre_periodo = nombre_per & periodo

			If rcv = 1 Then
				nombre_periodo = nombre_periodo & "RCV"
			End If

			Invoke(New MethodInvoker(Sub() textBox2.AppendText("INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,periodo,credito_cuotas,credito_multa,cuotas_omitidas,importe_cuota,recargos,importe_multa,importe_total,sector_notificacion) " & "VALUES (""" & nombre_periodo & """,""" & reg_pat & """,""" & reg_pat1 & """,""" & reg_pat2 & """,""" & ra_soc & """,""" & periodo & """,""" & cred_cuo & """,""" & cred_mul & """,""" & cuo_o & """,""" & cuo_a & """,""" & rec & """,""" & imp_mul & """,""" & imp_tot & """,""" & sector_n & """);" & vbLf)))
			'lineatbx2="INSERT INTO datos_factura (nombre_periodo,registro_patronal,registro_patronal1,registro_patronal2,razon_social,periodo,credito_cuotas,credito_multa,cuotas_omitidas,importe_cuota,recargos,importe_multa,importe_total,sector_notificacion) "+
			'                    "VALUES (\""+nombre_periodo+"\",\""+reg_pat+"\",\""+reg_pat1+"\",\""+reg_pat2+"\",\""+ra_soc+"\",\""+periodo+"\",\""+cred_cuo+"\",\""+cred_mul+"\",\""+cuo_o+"\",\""+cuo_a+"\",\""+rec+"\",\""+imp_mul+"\",\""+imp_tot+"\",\""+sector_n+"\");\n";
			'SetText2(lineatbx2);

			reg_pat = ""
			ra_soc = ""
			periodo = ""
			cred_cuo = ""
			cred_mul = ""
			cuota_omi = ""
			cuota_ac = ""
			recargo = ""
			imp_multa = ""
			imp_total = ""
			'sector_n="";
			bandera2 = 0
			reg_pat1 = ""
			reg_pat2 = ""
			contador_lin += 1
		End If
	End Sub

	Public Sub insertar_datos()

		conex.conectar("base_principal")
		'Insertar informacion en la BD
		k = 0
		Try
			Do
				text = textBox2.Lines
				sql = text(k)
				conex.consultar(sql)
				k += 1
			Loop While k <= (contador_lin - 1)
			Invoke(New MethodInvoker(Sub() 
			progressBar1.Value = 100
			label1.Text = "100%"
			label1.Refresh()

End Sub))

			MessageBox.Show("Los datos fueron agregados exitosamente a la Base de Datos." & vbLf & vbLf & vbTab & vbTab & k & " Registros Añadidos.", "Proceso Exitoso")
		Catch exp As Exception
			MessageBox.Show("ERROR: " & vbLf & "K:" & k & vbLf & "Contador:" & contador_lin & vbLf & Convert.ToString(exp), "Error al Insertar Datos en MySQL")
		End Try
	End Sub

	Public Sub descomponer_linea_multa()

		If (linea <> "") AndAlso (finbloque = 0) Then

			If (linea.Length > 100) OrElse (gancho = 1) Then
				If linea.Length >= 129 Then
					If (linea.Substring(114, 5)).Equals("CICLO") Then
						nombre_per = linea.Substring(121, 8)
					End If
				End If

				If ((linea.Substring(44, 13)).Equals("SUBDELEGACION")) AndAlso ((linea.Substring(59, 2)).Equals("38")) Then
					delegacion = linea.Substring(12, 2)
					subdele = linea.Substring(59, 2)
					municipio = linea.Substring(76, 3)
					sector_n = linea.Substring(107, 2)

					bloque = 1
				Else
					If (linea.Substring(44, 13)).Equals("SUBDELEGACION") Then
						subde = linea.Substring(59, 2)
						[sub] = Convert.ToInt32(subde)
						If [sub] > 38 Then
							finbloque = 1
						End If

					End If
				End If
				If (linea.Substring(21, 5)).Equals("TOTAL") Then
					bloque = 0
				End If

				If (gancho = 1) AndAlso (bloque = 1) Then
					gancho = 0
					marca = linea.Substring(13, 4)
					giro = linea.Substring(24, 40)
					lin_var = ((linea.Length) - 65)
					localidad = linea.Substring(65, lin_var)

					'Ajustar valores para ingresarlos a la BD
					If Not nombre_per.Equals("") Then
						nombre_periodo = (nombre_per.Substring(0, 5) & nombre_per.Substring(6, 2))
					End If
					reg_pat = reg_pat.Replace(" ", "-")
					reg_part = reg_pat.Split("-"C)

					For j = 0 To reg_part.Length - 1
						If Not reg_part(j).Equals("") Then
							reg_pat1 += reg_part(j)
						End If
					Next
					reg_pat2 = reg_pat1.Substring(0, 10)

					If localidad.Length > 40 Then
						localidad = localidad.Substring(0, 39)

						localidad = localidad.TrimEnd(" "C)
					End If

					ra_soc = ra_soc.TrimEnd(" "C)
					domicilio = domicilio.TrimEnd(" "C)
					giro = giro.TrimEnd(" "C)

					'ra_soc=ra_soc.Replace('\'',' ');
					'domicilio=domicilio.Replace('\'',' ');

					textBox2.AppendText("INSERT INTO multas (registro_patronal,registro_patronal1,registro_patronal2,razon_social,domicilio,giro_actividad,localidad,delegacion,subdelegacion,municipio,sector_notificacion,marca,periodo,folio_credito)" & "VALUES (""" & reg_pat & """,""" & reg_pat1 & """,""" & reg_pat2 & """,""" & ra_soc & """,""" & domicilio & """,""" & giro & """,""" & localidad & """,""" & delegacion & """,""" & subdele & """,""" & municipio & """,""" & sector_n & """,""" & marca & """,""" & nombre_periodo & """,""" & folio & """)" & vbLf)
					reg_pat = ""
					reg_pat1 = ""
					reg_pat2 = ""
					ra_soc = ""
					domicilio = ""
					giro = ""
					localidad = ""
					marca = ""
					contador_lin += 1
				End If

				If ((linea.Substring(0, 8)).Equals("00000000")) AndAlso (bloque = 1) Then
					gancho = 1
					reg_pat = linea.Substring(9, 14)
					ra_soc = linea.Substring(24, 40)
					domicilio = linea.Substring(65, 40)
					folio = linea.Substring(105, 9)
				End If
			End If
		End If
	End Sub

	Public Sub abrir_office()

		'creamos un objeto OpenDialog que es un cuadro de dialogo para buscar archivos
		Dim dialog As New OpenFileDialog()
		If (tipocarga = 1) OrElse (tipocarga = 2) Then
			dialog.Filter = "Archivos de Access (*.mdb;*.accdb)|*.mdb;*.accdb"
			'le indicamos el tipo de filtro en este caso que busque
			'solo los archivos Access
				'le damos un titulo a la ventana
			dialog.Title = "Seleccione el archivo de Access"
		End If

		If (tipocarga = 3) Then
			dialog.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx"
			'le indicamos el tipo de filtro en este caso que busque
			'solo los archivos excel
				'le damos un titulo a la ventana
			dialog.Title = "Seleccione el archivo de Excel"
		End If


		dialog.FileName = String.Empty
		'inicializamos con vacio el nombre del archivo
		'si al seleccionar el archivo damos Ok
		If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			'el nombre del archivo sera asignado al textbox
			textBox3.Text = dialog.SafeFileName
			archivo = dialog.FileName
			'hoja = textBox4.Text; //la variable hoja tendra el valor del textbox donde colocamos el nombre de la hoja
				'se manda a llamar al metodo
				'dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //se ajustan las
				'columnas al ancho del DataGridview para que no quede espacio en blanco (opcional)
			cargar_office()
		End If
	End Sub

	Public Sub cargar_office()

		If (tipocarga = 1) OrElse (tipocarga = 2) Then
			'esta cadena es para archivos Access 2007 y 2010
			ext = archivo.Substring(((archivo.Length) - 3), 3)
			ext = ext.ToLower()
			If (ext).Equals("mdb") Then
					'MessageBox.Show("HOLA :D");

				cad_con_ofis = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & archivo & "';"
			Else
				cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & archivo & "';"
			End If
		End If

		If (tipocarga = 3) Then
			'esta cadena es para archivos excel 2007 y 2010
			cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & archivo & "';Extended Properties=Excel 12.0;"
		End If

		conexion = New OleDbConnection(cad_con_ofis)
		'creamos la conexion con la hoja de excel o Access
		conexion.Open()
		'abrimos la conexion
		If (tipocarga = 1) OrElse (tipocarga = 2) Then
			Dim dt As System.Data.DataTable = conexion.GetSchema("TABLES")
			dataGridView2.DataSource = dt
			filas = (dataGridView2.RowCount) - 2
			Do
				If Not (dataGridView2.Rows(i).Cells(3).Value.ToString()).Equals("") Then
					If (dataGridView2.Rows(i).Cells(3).Value.ToString()).Equals("TABLE") Then
						tabla = dataGridView2.Rows(i).Cells(2).Value.ToString()
						comboBox1.Items.Add(tabla)
					End If
				End If
				i += 1
			Loop While i <= filas

			i = 0
		End If

	End Sub

	Public Sub conectar_office()

		If tipocarga = 1 Then
			If hoja.Length > 5 Then
				cons_tabla = hoja.Substring(0, 6)
			End If
			If cons_tabla.Equals("CDEMPA") Then

				cons_ofis = "Select * from " & hoja & " "

				button9.Enabled = True
			End If
		End If

		If tipocarga = 2 Then
			If hoja.Length > 5 Then
				cons_tabla = hoja.Substring(0, 6)
			End If
			If cons_tabla.Equals("CDEBPA") Then

				cons_ofis = "Select * from " & hoja & " "

				button9.Enabled = True
			Else
				cons_ofis = "Select * from " & hoja & " "
					'esta cadena es para archivos excel 2007 y 2010
					'cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';";
				button9.Enabled = False
			End If
		End If

		If (tipocarga = 3) Then
				'esta cadena es para archivos excel 2007 y 2010
				'cad_con_ofis = "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";
			cons_ofis = "Select * from [" & hoja & "$] "
		End If
		'para archivos de 97-2003 usar la siguiente cadena
		'string cadenaConexionArchivoExcel = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + archivo + "';Extended Properties=Excel 8.0;";

		'Validamos que el usuario ingrese el nombre de la hoja del archivo de excel a leer
		If String.IsNullOrEmpty(hoja) Then
			MessageBox.Show("No hay una hoja para leer")
		Else
			Try
				'Si el usuario escribio el nombre de la hoja se procedera con la busqueda
				'conexion = new OleDbConnection(cadenaConexionArchivoExcel);//creamos la conexion con la hoja de excel
				'conexion.Open(); //abrimos la conexion
				dataAdapter = New OleDbDataAdapter(cons_ofis, conexion)
				'traemos los datos de la hoja y las guardamos en un dataSdapter
				dataSet = New DataSet()
				' creamos la instancia del objeto DataSet
				dataAdapter.Fill(dataSet, hoja)
				'llenamos el dataset
				dataGridView1.DataSource = dataSet.Tables(0)
				'le asignamos al DataGridView el contenido del dataSet	
				conexion.Close()
				'cerramos la conexion
					'eliminamos la ultima fila del datagridview que se autoagrega
				dataGridView1.AllowUserToAddRows = False
			Catch ex As Exception
				'en caso de haber una excepcion que nos mande un mensaje de error
				MessageBox.Show("Error, Verificar el archivo o el nombre de la hoja" & vbLf & Convert.ToString(ex), "Error al Abrir Archivo de Excel")
			End Try
		End If
	End Sub

	Public Sub crear_tabla_ema()

		Try
			tot_row = dataGridView1.RowCount
			tot_col = dataGridView1.ColumnCount

			Do
				col += dataGridView1.Columns(k).Name.ToString() & vbLf
				k += 1
			Loop While k < tot_col

			'MessageBox.Show("Columnas:"+col);
			k = 0
			col = ""

			periodo = hoja.Substring(hoja.Length - 6, 6)
			periodo = (periodo.Substring(2, 4)) & (periodo.Substring(0, 2))

			conex.conectar("ema")
			sql = "CREATE TABLE EMA" & periodo & " (id INT(12) NOT NULL AUTO_INCREMENT, reg_pat VARCHAR(10) NOT NULL, reg_pat1 VARCHAR(11) NOT NULL, razon_social VARCHAR(55) NOT NULL, num_credito VARCHAR(10) NOT NULL, periodo VARCHAR(6) NOT NULL, num_trabajadores INT NOT NULL," & "sector_not INT NOT NULL, importe DECIMAL(10,2) NOT NULL, importe_multa DECIMAL(10,2) NOT NULL, status VARCHAR(20) NOT NULL, domicilio VARCHAR(80) NOT NULL, localidad VARCHAR(60) NOT NULL, PRIMARY KEY (id),UNIQUE INDEX id_UNIQUE (id ASC));"

			'MessageBox.Show("SQL:"+sql);
			conex.consultar(sql)


			leer_grid()
		Catch exp As Exception
			MessageBox.Show("ERROR: " & vbLf & Convert.ToString(exp), "Error al Insertar Datos en MySQL")
		End Try
	End Sub

	Public Sub leer_grid()

		j = 0
		panel3.Visible = True
		progressBar2.Value = 0

		For i = 0 To tot_row - 1

			'if((dataGridView1.Rows[i].Cells[j].Value.ToString()).Equals("")){
'					//campo = "";
'				}


			reg_pat2 = dataGridView1.Rows(i).Cells(3).Value.ToString()
			reg_pat1 = reg_pat2.Substring(1, 11)
			reg_pat = reg_pat1.Substring(0, 10)
			ra_soc = dataGridView1.Rows(i).Cells(5).Value.ToString()
			num_cred = dataGridView1.Rows(i).Cells(6).Value.ToString()
			num_trab = dataGridView1.Rows(i).Cells(12).Value.ToString()
			sector_n = dataGridView1.Rows(i).Cells(21).Value.ToString()
			domicilio = dataGridView1.Rows(i).Cells(23).Value.ToString()
			localidad = dataGridView1.Rows(i).Cells(24).Value.ToString()
			imp_tot = Convert.ToDouble(dataGridView1.Rows(i).Cells(13).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(14).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(15).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(16).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(17).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(18).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(19).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(31).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(32).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(33).Value.ToString())
			imp_tot = imp_tot + Convert.ToDouble(dataGridView1.Rows(i).Cells(34).Value.ToString())
			imp_mul = imp_tot * 0.4

			ra_soc = ra_soc.TrimEnd(" "C)
			domicilio = domicilio.TrimEnd(" "C)
			localidad = localidad.TrimEnd(" "C)
			'ra_soc=ra_soc.Replace('\'',' ');
'				domicilio=domicilio.Replace('\'',' ');
'				localidad=localidad.Replace('\'',' ');
'				

			Try

				nvatabla = "INSERT INTO " & hoja & " (reg_pat, reg_pat1, razon_social, num_credito, periodo, num_trabajadores, sector_not, importe, importe_multa, status, domicilio, localidad) VALUES" & "(""" & reg_pat & """,""" & reg_pat1 & """,""" & ra_soc & """,""" & num_cred & """,""" & periodo & """,""" & num_trab & """,""" & sector_n & """,""" & imp_tot & """,""" & imp_mul & """,""0"",""" & domicilio & """,""" & localidad & """)"

				conex.consultar(nvatabla)
			Catch exp As Exception
				MessageBox.Show("ERROR: " & vbLf & Convert.ToString(exp), "Error al Insertar Datos en MySQL")
			End Try

			por = ((i * 100) \ tot_row)
			porcent = Convert.ToInt32(por)

			If por > 99.6 Then
				porcent = 100
			End If
			progressBar2.Value = porcent
			porcent_text = Convert.ToString(porcent)
			label7.Text = porcent & " %"
			label7.Refresh()
			j += 1


			If j = 10 Then
				label6.Text = "Procesando."
				label6.Refresh()
			End If

			If j = 20 Then
				label6.Text = "Procesando.."
				label6.Refresh()
			End If

			If j = 30 Then
				label6.Text = "Procesando..."
				label6.Refresh()
			End If

			If j = 40 Then
				label6.Text = "Procesando"
				label6.Refresh()
				j = 0


			End If
		Next

		MessageBox.Show("Datos Ingresados Correctamente:" & vbLf, "Proceso Exitoso")
		panel3.Visible = False

	End Sub

	Private Sub Button1Click(sender As Object, e As EventArgs)
		opciones()
		contador_lin = 0
		leer_linea()
	End Sub

	Private Sub Button2Click(sender As Object, e As EventArgs)
		If (radioButton1.Checked) OrElse (radioButton2.Checked) OrElse (radioButton5.Checked) Then
			panel1.Visible = False
			Me.Height = 400
		End If

		If (radioButton3.Checked) OrElse (radioButton4.Checked) OrElse (radioButton6.Checked) Then
			panel2.Visible = True
			Me.Height = 480

			If radioButton3.Checked Then
				button6.Text = "Abrir Archivo EMA"
				label5.Text = "Tabla:"
				button8.Text = "Cargar Tabla"
			End If

			If radioButton4.Checked Then
				button6.Text = "Abrir Archivo EBA"
				label5.Text = "Tabla:"
				button8.Text = "Cargar Tabla"
			End If

			If radioButton6.Checked Then
				button6.Text = "Abrir Archivo"
				label5.Text = "Hoja:"
				button8.Text = "Cargar Hoja"
			End If
		End If
	End Sub

	Private Sub Button3Click(sender As Object, e As EventArgs)
		panel1.Visible = True
		Me.Height = 375
		Me.Refresh()
		rcv = 0

	End Sub

	Private Sub Button4Click(sender As Object, e As EventArgs)

		If ver = 1 Then
			textBox1.Visible = True
			textBox1.Show()
			button4.Text = "Ocultar Proceso"
			button4.Refresh()

			ver = 0
		Else
			textBox1.Visible = False
			textBox1.Hide()
			button4.Text = "Mostrar Proceso"
			button4.Refresh()
			ver = 1
		End If

	End Sub

	Private Sub Button5Click(sender As Object, e As EventArgs)
		Me.Height = 480
		hilosecundario = New Thread(New ThreadStart(AddressOf inicio))
		hilosecundario.Start()
		'inicio();
		button5.Enabled = False
	End Sub

	Private Sub Button6Click(sender As Object, e As EventArgs)
		comboBox1.Items.Clear()
		comboBox1.Text = ""
		If radioButton3.Checked Then
			tipocarga = 1
			label5.Text = "Tabla:"
			button8.Text = "Cargar Tabla"
		End If

		If radioButton4.Checked Then
			tipocarga = 2
			label5.Text = "Tabla:"
			button8.Text = "Cargar Tabla"
		End If

		If radioButton6.Checked Then
			tipocarga = 3
			label5.Text = "Hoja:"
			button8.Text = "Cargar Hoja"
		End If

		abrir_office()

	End Sub

	Private Sub Button7Click(sender As Object, e As EventArgs)
		panel2.Visible = False
		textBox3.Text = ""
		comboBox1.Items.Clear()
		comboBox1.Text = ""
		dataGridView1.DataSource = ""
		dataGridView2.DataSource = ""
		Me.Height = 180

	End Sub

	Private Sub Button8Click(sender As Object, e As EventArgs)
		hoja = comboBox1.SelectedItem.ToString()
		conectar_office()
	End Sub

	Private Sub Button9Click(sender As Object, e As EventArgs)
		crear_tabla_ema()
	End Sub

	Private Sub RadioButton1CheckedChanged(sender As Object, e As EventArgs)
		If (radioButton1.Checked) OrElse (radioButton2.Checked) Then
			Me.Height = 375
			groupBox2.Visible = True
			button2.Location = New System.Drawing.Point(268, 338)
			Me.Refresh()
			If radioButton2.Checked Then
				rcv = 1

			End If
				'this.Height=180;
'				this.Refresh();

		Else
		End If

		If (radioButton3.Checked) OrElse (radioButton4.Checked) OrElse (radioButton5.Checked) OrElse (radioButton6.Checked) Then
			groupBox2.Visible = False
			Me.Height = 300
			button2.Location = New System.Drawing.Point(268, 170)
			'button2.Location.X=290;
'				button2.Location.Y=30;

			button2.Refresh()
		End If


	End Sub

	Private Sub RadioButton7CheckedChanged(sender As Object, e As EventArgs)
		Me.Height = 480
		Me.Refresh()

		If (radioButton7.Checked = True) OrElse (radioButton8.Checked = True) Then
			radioButton9.Checked = False
			radioButton10.Checked = False
			radioButton11.Checked = False
		End If
	End Sub

	Private Sub RadioButton9CheckedChanged(sender As Object, e As EventArgs)
		Me.Height = 480
		Me.Refresh()
		If (radioButton9.Checked = True) OrElse (radioButton10.Checked = True) OrElse (radioButton11.Checked = True) Then
			radioButton7.Checked = False

			radioButton8.Checked = False
		End If
	End Sub

	Private Sub Lector_FacLoad(sender As Object, e As EventArgs)
		Me.Height = 180

		radioButton1.Checked = False
		radioButton2.Checked = False
		radioButton3.Checked = False
		radioButton4.Checked = False
		radioButton5.Checked = False
		radioButton6.Checked = False
		radioButton7.Checked = False
		radioButton8.Checked = False
		radioButton9.Checked = False
		radioButton10.Checked = False
		radioButton11.Checked = False

		dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
		dataGridView1.MultiSelect = False

	End Sub

	Private Sub TextBox3Enter(sender As Object, e As EventArgs)

	End Sub

End Class

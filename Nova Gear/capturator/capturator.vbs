Set objfso = createobject("scripting.filesystemobject")
Set objfsoaux = createobject("scripting.filesystemobject")
Set objfsoaux2 = createobject("scripting.filesystemobject")

Set archivotexto = objfso.opentextfile("C:\Users\usuario\Documents\SharpDevelop Projects\Project ADA\Nova Gear\bin\Debug\temp.txt",1)
Set archivotextoaux = objfsoaux.opentextfile("temp_aux.txt",1,true)

set capturar = WScript.CreateObject("WScript.Shell")
WScript.Sleep 1000
capturar.AppActivate "siscob"
capturar.SendKeys "% x"
linea = ""
i=0
contarch= ""
bandera=0
dim cont as single

Set archivotextoaux = objfsoaux.opentextfile("temp_aux.txt",1)
contarch = archivotextoaux.readline
archivotextoaux.close
WScript.Sleep 1000
MsgBox "contarch: "&contarch
WScript.Sleep 1000
cont = contarch

do
	If cont = 0 Then
		bandera = 1	
	else 
		WScript.Sleep 1000
		MsgBox "contarch: "&contarch&"i: "&i
		WScript.Sleep 1000
		If i > cont then
		bandera = 1
		else
		bandera =0
		End If
	End If

	linea = archivotexto.readline	

	if bandera = 1 then
		WScript.Sleep 500
		if (linea <> "$") then
			capturar.Sendkeys linea
			capturar.Sendkeys "{TAB}"
		else
			WScript.Sleep 500
			capturar.Sendkeys "{ENTER}"
			capturar.Sendkeys "{ENTER}"
			WScript.Sleep 300
		end if
		WScript.Sleep 500
	End If

	i = i+1
	Set archivotextoaux2 = objfsoaux2.opentextfile("temp_aux.txt",2,true)
	archivotextoaux2.writeline i
	archivotextoaux2.close

loop until (linea = "%&")

archivotexto.close
Set archivotextoaux2 = objfsoaux2.opentextfile("temp_aux.txt",2,true)
archivotextoaux2.writeline "0"
archivotextoaux2.close
MsgBox "Captura Terminada"
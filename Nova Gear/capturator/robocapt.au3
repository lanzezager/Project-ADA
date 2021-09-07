#include <ScreenCapture.au3>
#include <MsgBoxConstants.au3>

Dim $altopant = @DesktopHeight, $anchopant = @DesktopWidth, $alto =0, $ancho=0, $reg_pat=""
   $alto=$altopant-50
   $ancho=$anchopant/2

captura()

Func captura()
$file = FileOpen ("C:\Users\usuario\Documents\SharpDevelop Projects\Project ADA\Nova Gear\bin\Debug\temp.txt",0)
$file2 = FileOpen ("temp_aux.txt",0)

If $file =-1 Then
   MsgBox(0,"Error","Error al leer archivo temporal")
EndIf

If $file =-1 Then
   MsgBox(0,"Error","Error al leer archivo auxiliar")
EndIf

Sleep(1000)

WinActivate("Siscob")
Send("! x")

Dim $i=0, $j=0, $cont_arch="", $bandera=0, $cont=0, $linea=""

$contarch = FileReadLine($file2)
FileClose($file2)

$cont = int($contarch)
MsgBox(0,"","Cont: "&$cont)

Do
   If $cont = 0 Then
	  $bandera = 1
   Else
	  If $i > $cont Then
		 $bandera = 1
	  Else
		 $bandera = 0
	  EndIf
   EndIf

   $linea = FileReadLine($file)

   If $bandera = 1 Then
	  Sleep(500)

	  if $j=0 or $j = 6 Then
	  $reg_pat = $linea
	  $j=0
	  EndIf

	  If $linea <> "$" Then
		 Send($linea)
		 Send("{TAB}")
	  Else
		 Send("{ENTER}")
		 Sleep(1000)

		 buscarpixel()

		 Send("{ENTER}")
	   EndIf
	   Sleep(500)
	   $j=$j+1;
	EndIf

	$i = $i + 1
	$file2 = FileOpen ("temp_aux.txt",2)
	FileWriteLine($file2, $i)
	FileFlush($file2)
	FileClose($file2)

Until @error = -1 Or $linea = "%&"

FileClose($file)
$file2 = FileOpen ("temp_aux.txt",2)
FileWriteLine($file2, "0")
FileFlush($file2)
FileClose($file2)
EndFunc

Func imppant()
   ; Capture region
   ;MsgBox (0,"Hola", "Alto= "&$alto&@LF&" Ancho= "&$ancho)
    Sleep(2000)
    _ScreenCapture_SetBMPFormat(0)
	_ScreenCapture_Capture(@MyDocumentsDir & "\dumps\"&$reg_pat&".bmp", 0, $alto, $ancho, $altopant)
   ; ShellExecute(@MyDocumentsDir & "\dumps\GDIPlus_Image3.bmp")

EndFunc

Func buscarpixel()
   ; Find a pure red pixel in the range 0,0-20,300
Local $aCoord = PixelSearch(0, $alto, $ancho, $altopant, 0xFF0000,5)

If Not @error Then
	imppant()
	;MsgBox($MB_SYSTEMMODAL, "", "X and Y are: " & $aCoord[0] & "," & $aCoord[1])
 EndIf

EndFunc
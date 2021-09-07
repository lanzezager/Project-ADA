
#include <ScreenCapture.au3>

Dim $altopant = @DesktopHeight, $anchopant = @DesktopWidth, $alto =0, $ancho=0

HotKeySet("{END}", "cerrarcapt")
HotKeySet("{HOME}","continuar")
HotKeySet("{ESC}","cerrar")
HotKeySet("{F6}","capturapantalla")

;MsgBox (0,"Hola", "Alto= "&$altopant&@LF&" Ancho= "&$anchopant)

While 1
	Sleep(100)
 WEnd

 Func cerrarcapt()
	ProcessClose("capt.exe")
 EndFunc

 Func continuar()
	Run("capt.exe")
 EndFunc

 Func capturapantalla()
   ; Capture region
   $alto=$altopant-50
   $ancho=$anchopant/2
   ;MsgBox (0,"Hola", "Alto= "&$alto&@LF&" Ancho= "&$ancho)
    _ScreenCapture_SetBMPFormat(0)
	_ScreenCapture_Capture(@MyDocumentsDir & "\GDIPlus_Image3.bmp", 0, $alto, $ancho, $altopant)
    ShellExecute(@MyDocumentsDir & "\GDIPlus_Image3.bmp")
 EndFunc

 Func cerrar()
   MsgBox (0,"Adios", "Hasta Luego")
   ProcessClose("capt.exe")
	Exit
 EndFunc
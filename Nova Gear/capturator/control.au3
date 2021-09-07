
HotKeySet("{END}", "cerrarcapt")
HotKeySet("{HOME}","continuar")
HotKeySet("{ESC}","cerrar")

;MsgBox (0,"Hola", "Alto= "&$altopant&@LF&" Ancho= "&$anchopant)

While 1
	Sleep(100)
 WEnd

 Func cerrarcapt()
	ProcessClose("robocapt.exe")
 EndFunc

 Func continuar()
	Run("robocapt.exe")
 EndFunc

 Func cerrar()
   MsgBox (0,"Adios", "Hasta Luego")
   ProcessClose("robocapt.exe")
	Exit
 EndFunc
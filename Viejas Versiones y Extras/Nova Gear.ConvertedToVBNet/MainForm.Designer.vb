'
' * Creado por SharpDevelop.
' * Usuario: Lanze Zager
' * Fecha: 30/04/2015
' * Hora: 11:27 a. m.
' * 
' * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
' 

Partial Class MainForm
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer = Nothing

	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
		Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.menuStrip1 = New System.Windows.Forms.MenuStrip()
		Me.archivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.lectorDeFacturasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.menuStrip1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' statusStrip1
		' 
		Me.statusStrip1.Location = New System.Drawing.Point(0, 286)
		Me.statusStrip1.Name = "statusStrip1"
		Me.statusStrip1.Size = New System.Drawing.Size(684, 22)
		Me.statusStrip1.TabIndex = 1
		Me.statusStrip1.Text = "statusStrip1"
		' 
		' menuStrip1
		' 
		Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.archivoToolStripMenuItem})
		Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.menuStrip1.Name = "menuStrip1"
		Me.menuStrip1.Size = New System.Drawing.Size(684, 24)
		Me.menuStrip1.TabIndex = 2
		Me.menuStrip1.Text = "menuStrip1"
		' 
		' archivoToolStripMenuItem
		' 
		Me.archivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lectorDeFacturasToolStripMenuItem})
		Me.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem"
		Me.archivoToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
		Me.archivoToolStripMenuItem.Text = "Archivo"
		' 
		' lectorDeFacturasToolStripMenuItem
		' 
		Me.lectorDeFacturasToolStripMenuItem.Name = "lectorDeFacturasToolStripMenuItem"
		Me.lectorDeFacturasToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
		Me.lectorDeFacturasToolStripMenuItem.Text = "Lector de Facturas"
		AddHandler Me.lectorDeFacturasToolStripMenuItem.Click, New System.EventHandler(AddressOf Me.LectorDeFacturasToolStripMenuItemClick)
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.BackgroundImage = DirectCast(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
		Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
		Me.ClientSize = New System.Drawing.Size(684, 308)
		Me.Controls.Add(Me.statusStrip1)
		Me.Controls.Add(Me.menuStrip1)
		Me.IsMdiContainer = True
		Me.MainMenuStrip = Me.menuStrip1
		Me.Name = "MainForm"
		Me.Text = "Nova Gear"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.menuStrip1.ResumeLayout(False)
		Me.menuStrip1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
	Private lectorDeFacturasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private archivoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private menuStrip1 As System.Windows.Forms.MenuStrip
	Private statusStrip1 As System.Windows.Forms.StatusStrip
End Class

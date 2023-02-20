'
' * Creado por SharpDevelop.
' * Usuario: Lanze Zager
' * Fecha: 30/04/2015
' * Hora: 11:47 a. m.
' * 
' * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
' 

Partial Class Lector_Fac
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	'private System.ComponentModel.IContainer components = null;

	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	'	protected override void Dispose(bool disposing)
'		{
'			if (disposing) {
'				if (components != null) {
'					components.Dispose();
'				}
'			}
'			base.Dispose(disposing);
'		}


	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
		Me.button1 = New System.Windows.Forms.Button()
		Me.progressBar1 = New System.Windows.Forms.ProgressBar()
		Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.label1 = New System.Windows.Forms.Label()
		Me.textBox1 = New System.Windows.Forms.TextBox()
		Me.textBox2 = New System.Windows.Forms.TextBox()
		Me.panel1 = New System.Windows.Forms.Panel()
		Me.groupBox2 = New System.Windows.Forms.GroupBox()
		Me.groupBox4 = New System.Windows.Forms.GroupBox()
		Me.radioButton10 = New System.Windows.Forms.RadioButton()
		Me.radioButton11 = New System.Windows.Forms.RadioButton()
		Me.radioButton9 = New System.Windows.Forms.RadioButton()
		Me.groupBox3 = New System.Windows.Forms.GroupBox()
		Me.radioButton7 = New System.Windows.Forms.RadioButton()
		Me.radioButton8 = New System.Windows.Forms.RadioButton()
		Me.button2 = New System.Windows.Forms.Button()
		Me.groupBox1 = New System.Windows.Forms.GroupBox()
		Me.radioButton6 = New System.Windows.Forms.RadioButton()
		Me.radioButton5 = New System.Windows.Forms.RadioButton()
		Me.radioButton4 = New System.Windows.Forms.RadioButton()
		Me.radioButton3 = New System.Windows.Forms.RadioButton()
		Me.radioButton2 = New System.Windows.Forms.RadioButton()
		Me.radioButton1 = New System.Windows.Forms.RadioButton()
		Me.button3 = New System.Windows.Forms.Button()
		Me.button4 = New System.Windows.Forms.Button()
		Me.button5 = New System.Windows.Forms.Button()
		Me.label2 = New System.Windows.Forms.Label()
		Me.panel2 = New System.Windows.Forms.Panel()
		Me.panel3 = New System.Windows.Forms.Panel()
		Me.progressBar2 = New System.Windows.Forms.ProgressBar()
		Me.label7 = New System.Windows.Forms.Label()
		Me.label6 = New System.Windows.Forms.Label()
		Me.button9 = New System.Windows.Forms.Button()
		Me.button8 = New System.Windows.Forms.Button()
		Me.dataGridView2 = New System.Windows.Forms.DataGridView()
		Me.comboBox1 = New System.Windows.Forms.ComboBox()
		Me.label5 = New System.Windows.Forms.Label()
		Me.label4 = New System.Windows.Forms.Label()
		Me.textBox3 = New System.Windows.Forms.TextBox()
		Me.label3 = New System.Windows.Forms.Label()
		Me.button7 = New System.Windows.Forms.Button()
		Me.button6 = New System.Windows.Forms.Button()
		Me.dataGridView1 = New System.Windows.Forms.DataGridView()
		Me.panel1.SuspendLayout()
		Me.groupBox2.SuspendLayout()
		Me.groupBox4.SuspendLayout()
		Me.groupBox3.SuspendLayout()
		Me.groupBox1.SuspendLayout()
		Me.panel2.SuspendLayout()
		Me.panel3.SuspendLayout()
		DirectCast(Me.dataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		' 
		' button1
		' 
		Me.button1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button1.Location = New System.Drawing.Point(26, 20)
		Me.button1.Name = "button1"
		Me.button1.Size = New System.Drawing.Size(86, 64)
		Me.button1.TabIndex = 0
		Me.button1.Text = "Abrir Factura"
		Me.button1.UseVisualStyleBackColor = True
		AddHandler Me.button1.Click, New System.EventHandler(AddressOf Me.Button1Click)
		' 
		' progressBar1
		' 
		Me.progressBar1.Location = New System.Drawing.Point(139, 377)
		Me.progressBar1.Name = "progressBar1"
		Me.progressBar1.Size = New System.Drawing.Size(542, 41)
		Me.progressBar1.[Step] = 1
		Me.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
		Me.progressBar1.TabIndex = 1
		' 
		' label1
		' 
		Me.label1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label1.Location = New System.Drawing.Point(27, 377)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(86, 41)
		Me.label1.TabIndex = 2
		Me.label1.Text = "0 %"
		Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		' 
		' textBox1
		' 
		Me.textBox1.BackColor = System.Drawing.SystemColors.Window
		Me.textBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.textBox1.Location = New System.Drawing.Point(137, 48)
		Me.textBox1.MaxLength = 100000000
		Me.textBox1.Multiline = True
		Me.textBox1.Name = "textBox1"
		Me.textBox1.[ReadOnly] = True
		Me.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.textBox1.Size = New System.Drawing.Size(544, 299)
		Me.textBox1.TabIndex = 3
		Me.textBox1.WordWrap = False
		' 
		' textBox2
		' 
		Me.textBox2.Location = New System.Drawing.Point(129, 90)
		Me.textBox2.Multiline = True
		Me.textBox2.Name = "textBox2"
		Me.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.textBox2.Size = New System.Drawing.Size(562, 99)
		Me.textBox2.TabIndex = 4
		Me.textBox2.Visible = False
		Me.textBox2.WordWrap = False
		' 
		' panel1
		' 
		Me.panel1.Controls.Add(Me.groupBox2)
		Me.panel1.Controls.Add(Me.button2)
		Me.panel1.Controls.Add(Me.groupBox1)
		Me.panel1.Location = New System.Drawing.Point(12, 12)
		Me.panel1.Name = "panel1"
		Me.panel1.Size = New System.Drawing.Size(690, 427)
		Me.panel1.TabIndex = 5
		' 
		' groupBox2
		' 
		Me.groupBox2.Controls.Add(Me.groupBox4)
		Me.groupBox2.Controls.Add(Me.groupBox3)
		Me.groupBox2.Font = New System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.groupBox2.Location = New System.Drawing.Point(14, 141)
		Me.groupBox2.Name = "groupBox2"
		Me.groupBox2.Size = New System.Drawing.Size(655, 178)
		Me.groupBox2.TabIndex = 1
		Me.groupBox2.TabStop = False
		Me.groupBox2.Text = "Periodo"
		' 
		' groupBox4
		' 
		Me.groupBox4.Controls.Add(Me.radioButton10)
		Me.groupBox4.Controls.Add(Me.radioButton11)
		Me.groupBox4.Controls.Add(Me.radioButton9)
		Me.groupBox4.Location = New System.Drawing.Point(345, 28)
		Me.groupBox4.Name = "groupBox4"
		Me.groupBox4.Size = New System.Drawing.Size(168, 125)
		Me.groupBox4.TabIndex = 14
		Me.groupBox4.TabStop = False
		Me.groupBox4.Text = "SIVEPA"
		' 
		' radioButton10
		' 
		Me.radioButton10.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.radioButton10.Location = New System.Drawing.Point(26, 54)
		Me.radioButton10.Name = "radioButton10"
		Me.radioButton10.Size = New System.Drawing.Size(136, 24)
		Me.radioButton10.TabIndex = 9
		Me.radioButton10.TabStop = True
		Me.radioButton10.Text = "EXTEMPORANEO"
		Me.radioButton10.UseVisualStyleBackColor = True
		AddHandler Me.radioButton10.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton9CheckedChanged)
		' 
		' radioButton11
		' 
		Me.radioButton11.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.radioButton11.Location = New System.Drawing.Point(26, 84)
		Me.radioButton11.Name = "radioButton11"
		Me.radioButton11.Size = New System.Drawing.Size(104, 24)
		Me.radioButton11.TabIndex = 10
		Me.radioButton11.TabStop = True
		Me.radioButton11.Text = "OPORTUNO"
		Me.radioButton11.UseVisualStyleBackColor = True
		AddHandler Me.radioButton11.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton9CheckedChanged)
		' 
		' radioButton9
		' 
		Me.radioButton9.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.radioButton9.Location = New System.Drawing.Point(26, 24)
		Me.radioButton9.Name = "radioButton9"
		Me.radioButton9.Size = New System.Drawing.Size(104, 24)
		Me.radioButton9.TabIndex = 8
		Me.radioButton9.TabStop = True
		Me.radioButton9.Text = "ANUAL"
		Me.radioButton9.UseVisualStyleBackColor = True
		AddHandler Me.radioButton9.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton9CheckedChanged)
		' 
		' groupBox3
		' 
		Me.groupBox3.BackColor = System.Drawing.SystemColors.Control
		Me.groupBox3.Controls.Add(Me.radioButton7)
		Me.groupBox3.Controls.Add(Me.radioButton8)
		Me.groupBox3.Location = New System.Drawing.Point(134, 28)
		Me.groupBox3.Name = "groupBox3"
		Me.groupBox3.Size = New System.Drawing.Size(168, 125)
		Me.groupBox3.TabIndex = 13
		Me.groupBox3.TabStop = False
		Me.groupBox3.Text = "ECO"
		' 
		' radioButton7
		' 
		Me.radioButton7.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.radioButton7.Location = New System.Drawing.Point(26, 38)
		Me.radioButton7.Name = "radioButton7"
		Me.radioButton7.Size = New System.Drawing.Size(129, 24)
		Me.radioButton7.TabIndex = 11
		Me.radioButton7.TabStop = True
		Me.radioButton7.Text = "ESTRATEGICO"
		Me.radioButton7.UseVisualStyleBackColor = True
		AddHandler Me.radioButton7.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton7CheckedChanged)
		' 
		' radioButton8
		' 
		Me.radioButton8.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.radioButton8.Location = New System.Drawing.Point(26, 68)
		Me.radioButton8.Name = "radioButton8"
		Me.radioButton8.Size = New System.Drawing.Size(129, 24)
		Me.radioButton8.TabIndex = 12
		Me.radioButton8.TabStop = True
		Me.radioButton8.Text = "MECANIZADO"
		Me.radioButton8.UseVisualStyleBackColor = True
		AddHandler Me.radioButton8.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton7CheckedChanged)
		' 
		' button2
		' 
		Me.button2.Font = New System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button2.Location = New System.Drawing.Point(266, 338)
		Me.button2.Name = "button2"
		Me.button2.Size = New System.Drawing.Size(140, 50)
		Me.button2.TabIndex = 2
		Me.button2.Text = "Continuar >>"
		Me.button2.UseVisualStyleBackColor = True
		AddHandler Me.button2.Click, New System.EventHandler(AddressOf Me.Button2Click)
		' 
		' groupBox1
		' 
		Me.groupBox1.BackColor = System.Drawing.SystemColors.Control
		Me.groupBox1.Controls.Add(Me.radioButton6)
		Me.groupBox1.Controls.Add(Me.radioButton5)
		Me.groupBox1.Controls.Add(Me.radioButton4)
		Me.groupBox1.Controls.Add(Me.radioButton3)
		Me.groupBox1.Controls.Add(Me.radioButton2)
		Me.groupBox1.Controls.Add(Me.radioButton1)
		Me.groupBox1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.groupBox1.Location = New System.Drawing.Point(14, 16)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(655, 103)
		Me.groupBox1.TabIndex = 0
		Me.groupBox1.TabStop = False
		Me.groupBox1.Text = "Tipo de Documento"
		' 
		' radioButton6
		' 
		Me.radioButton6.Location = New System.Drawing.Point(545, 29)
		Me.radioButton6.Name = "radioButton6"
		Me.radioButton6.Size = New System.Drawing.Size(80, 47)
		Me.radioButton6.TabIndex = 5
		Me.radioButton6.Text = "Carga Manual"
		Me.radioButton6.UseVisualStyleBackColor = True
		AddHandler Me.radioButton6.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton1CheckedChanged)
		' 
		' radioButton5
		' 
		Me.radioButton5.Location = New System.Drawing.Point(435, 40)
		Me.radioButton5.Name = "radioButton5"
		Me.radioButton5.Size = New System.Drawing.Size(80, 24)
		Me.radioButton5.TabIndex = 4
		Me.radioButton5.Text = "Oficios"
		Me.radioButton5.UseVisualStyleBackColor = True
		AddHandler Me.radioButton5.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton1CheckedChanged)
		' 
		' radioButton4
		' 
		Me.radioButton4.Location = New System.Drawing.Point(335, 40)
		Me.radioButton4.Name = "radioButton4"
		Me.radioButton4.Size = New System.Drawing.Size(70, 24)
		Me.radioButton4.TabIndex = 3
		Me.radioButton4.Text = "EBA*"
		Me.radioButton4.UseVisualStyleBackColor = True
		AddHandler Me.radioButton4.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton1CheckedChanged)
		' 
		' radioButton3
		' 
		Me.radioButton3.Location = New System.Drawing.Point(235, 40)
		Me.radioButton3.Name = "radioButton3"
		Me.radioButton3.Size = New System.Drawing.Size(70, 24)
		Me.radioButton3.TabIndex = 2
		Me.radioButton3.Text = "EMA*"
		Me.radioButton3.UseVisualStyleBackColor = True
		AddHandler Me.radioButton3.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton1CheckedChanged)
		' 
		' radioButton2
		' 
		Me.radioButton2.Location = New System.Drawing.Point(135, 40)
		Me.radioButton2.Name = "radioButton2"
		Me.radioButton2.Size = New System.Drawing.Size(70, 24)
		Me.radioButton2.TabIndex = 1
		Me.radioButton2.Text = "RCV*"
		Me.radioButton2.UseVisualStyleBackColor = True
		AddHandler Me.radioButton2.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton1CheckedChanged)
		' 
		' radioButton1
		' 
		Me.radioButton1.Location = New System.Drawing.Point(35, 40)
		Me.radioButton1.Name = "radioButton1"
		Me.radioButton1.Size = New System.Drawing.Size(70, 24)
		Me.radioButton1.TabIndex = 0
		Me.radioButton1.Text = "COP*"
		Me.radioButton1.UseVisualStyleBackColor = True
		AddHandler Me.radioButton1.CheckedChanged, New System.EventHandler(AddressOf Me.RadioButton1CheckedChanged)
		' 
		' button3
		' 
		Me.button3.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button3.Location = New System.Drawing.Point(27, 309)
		Me.button3.Name = "button3"
		Me.button3.Size = New System.Drawing.Size(86, 38)
		Me.button3.TabIndex = 6
		Me.button3.Text = "<<  Atras"
		Me.button3.UseVisualStyleBackColor = True
		AddHandler Me.button3.Click, New System.EventHandler(AddressOf Me.Button3Click)
		' 
		' button4
		' 
		Me.button4.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button4.Location = New System.Drawing.Point(26, 256)
		Me.button4.Name = "button4"
		Me.button4.Size = New System.Drawing.Size(87, 41)
		Me.button4.TabIndex = 7
		Me.button4.Text = "Ocultar Proceso"
		Me.button4.UseVisualStyleBackColor = True
		AddHandler Me.button4.Click, New System.EventHandler(AddressOf Me.Button4Click)
		' 
		' button5
		' 
		Me.button5.Enabled = False
		Me.button5.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button5.Location = New System.Drawing.Point(27, 90)
		Me.button5.Name = "button5"
		Me.button5.Size = New System.Drawing.Size(87, 41)
		Me.button5.TabIndex = 2
		Me.button5.Text = "Leer"
		Me.button5.UseVisualStyleBackColor = True
		AddHandler Me.button5.Click, New System.EventHandler(AddressOf Me.Button5Click)
		' 
		' label2
		' 
		Me.label2.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label2.Location = New System.Drawing.Point(137, 22)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(544, 23)
		Me.label2.TabIndex = 8
		Me.label2.Text = "Factura:"
		Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		' 
		' panel2
		' 
		Me.panel2.Controls.Add(Me.panel3)
		Me.panel2.Controls.Add(Me.button9)
		Me.panel2.Controls.Add(Me.button8)
		Me.panel2.Controls.Add(Me.dataGridView2)
		Me.panel2.Controls.Add(Me.comboBox1)
		Me.panel2.Controls.Add(Me.label5)
		Me.panel2.Controls.Add(Me.label4)
		Me.panel2.Controls.Add(Me.textBox3)
		Me.panel2.Controls.Add(Me.label3)
		Me.panel2.Controls.Add(Me.button7)
		Me.panel2.Controls.Add(Me.button6)
		Me.panel2.Controls.Add(Me.dataGridView1)
		Me.panel2.Location = New System.Drawing.Point(7, 1)
		Me.panel2.Name = "panel2"
		Me.panel2.Size = New System.Drawing.Size(700, 448)
		Me.panel2.TabIndex = 9
		Me.panel2.Visible = False
		' 
		' panel3
		' 
		Me.panel3.Controls.Add(Me.progressBar2)
		Me.panel3.Controls.Add(Me.label7)
		Me.panel3.Controls.Add(Me.label6)
		Me.panel3.Location = New System.Drawing.Point(15, 73)
		Me.panel3.Name = "panel3"
		Me.panel3.Size = New System.Drawing.Size(669, 365)
		Me.panel3.TabIndex = 13
		Me.panel3.Visible = False
		' 
		' progressBar2
		' 
		Me.progressBar2.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.progressBar2.Location = New System.Drawing.Point(50, 170)
		Me.progressBar2.Name = "progressBar2"
		Me.progressBar2.Size = New System.Drawing.Size(571, 65)
		Me.progressBar2.[Step] = 1
		Me.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous
		Me.progressBar2.TabIndex = 2
		' 
		' label7
		' 
		Me.label7.Font = New System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label7.Location = New System.Drawing.Point(286, 119)
		Me.label7.Name = "label7"
		Me.label7.Size = New System.Drawing.Size(99, 36)
		Me.label7.TabIndex = 1
		Me.label7.Text = "0%"
		Me.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		' 
		' label6
		' 
		Me.label6.Font = New System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label6.Location = New System.Drawing.Point(241, 257)
		Me.label6.Name = "label6"
		Me.label6.Size = New System.Drawing.Size(213, 36)
		Me.label6.TabIndex = 0
		Me.label6.Text = "Procesando"
		Me.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		' 
		' button9
		' 
		Me.button9.Enabled = False
		Me.button9.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button9.Location = New System.Drawing.Point(556, 16)
		Me.button9.Name = "button9"
		Me.button9.Size = New System.Drawing.Size(118, 51)
		Me.button9.TabIndex = 12
		Me.button9.Text = "Insertar en la Base de Datos"
		Me.button9.UseVisualStyleBackColor = True
		AddHandler Me.button9.Click, New System.EventHandler(AddressOf Me.Button9Click)
		' 
		' button8
		' 
		Me.button8.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button8.Location = New System.Drawing.Point(417, 16)
		Me.button8.Name = "button8"
		Me.button8.Size = New System.Drawing.Size(87, 51)
		Me.button8.TabIndex = 10
		Me.button8.Text = "Cargar Tabla"
		Me.button8.UseVisualStyleBackColor = True
		AddHandler Me.button8.Click, New System.EventHandler(AddressOf Me.Button8Click)
		' 
		' dataGridView2
		' 
		Me.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dataGridView2.Location = New System.Drawing.Point(54, 108)
		Me.dataGridView2.Name = "dataGridView2"
		Me.dataGridView2.Size = New System.Drawing.Size(137, 150)
		Me.dataGridView2.TabIndex = 9
		Me.dataGridView2.Visible = False
		' 
		' comboBox1
		' 
		Me.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.comboBox1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.comboBox1.Location = New System.Drawing.Point(230, 45)
		Me.comboBox1.Name = "comboBox1"
		Me.comboBox1.Size = New System.Drawing.Size(140, 22)
		Me.comboBox1.Sorted = True
		Me.comboBox1.TabIndex = 8
		' 
		' label5
		' 
		Me.label5.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label5.Location = New System.Drawing.Point(168, 46)
		Me.label5.Name = "label5"
		Me.label5.Size = New System.Drawing.Size(61, 23)
		Me.label5.TabIndex = 6
		Me.label5.Text = "Tabla:"
		Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		' 
		' label4
		' 
		Me.label4.Font = New System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label4.Location = New System.Drawing.Point(25, 68)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(649, 23)
		Me.label4.TabIndex = 5
		Me.label4.Text = "Vista Previa: "
		Me.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft
		' 
		' textBox3
		' 
		Me.textBox3.BackColor = System.Drawing.SystemColors.Window
		Me.textBox3.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.textBox3.Location = New System.Drawing.Point(230, 17)
		Me.textBox3.Name = "textBox3"
		Me.textBox3.[ReadOnly] = True
		Me.textBox3.Size = New System.Drawing.Size(140, 21)
		Me.textBox3.TabIndex = 4
		Me.textBox3.WordWrap = False
		AddHandler Me.textBox3.Enter, New System.EventHandler(AddressOf Me.TextBox3Enter)
		' 
		' label3
		' 
		Me.label3.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.label3.Location = New System.Drawing.Point(168, 17)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(61, 23)
		Me.label3.TabIndex = 3
		Me.label3.Text = "Archivo:"
		Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		' 
		' button7
		' 
		Me.button7.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button7.Location = New System.Drawing.Point(25, 400)
		Me.button7.Name = "button7"
		Me.button7.Size = New System.Drawing.Size(80, 34)
		Me.button7.TabIndex = 2
		Me.button7.Text = "<<  Atras"
		Me.button7.UseVisualStyleBackColor = True
		AddHandler Me.button7.Click, New System.EventHandler(AddressOf Me.Button7Click)
		' 
		' button6
		' 
		Me.button6.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.button6.Location = New System.Drawing.Point(25, 16)
		Me.button6.Name = "button6"
		Me.button6.Size = New System.Drawing.Size(113, 51)
		Me.button6.TabIndex = 1
		Me.button6.Text = "Abrir Archivo EMA"
		Me.button6.UseVisualStyleBackColor = True
		AddHandler Me.button6.Click, New System.EventHandler(AddressOf Me.Button6Click)
		' 
		' dataGridView1
		' 
		Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dataGridView1.Location = New System.Drawing.Point(25, 94)
		Me.dataGridView1.Name = "dataGridView1"
		Me.dataGridView1.Size = New System.Drawing.Size(649, 286)
		Me.dataGridView1.TabIndex = 0
		Me.dataGridView1.VirtualMode = True
		' 
		' Lector_Fac
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(714, 451)
		Me.Controls.Add(Me.panel2)
		Me.Controls.Add(Me.panel1)
		Me.Controls.Add(Me.textBox2)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.progressBar1)
		Me.Controls.Add(Me.button1)
		Me.Controls.Add(Me.button3)
		Me.Controls.Add(Me.button4)
		Me.Controls.Add(Me.textBox1)
		Me.Controls.Add(Me.button5)
		Me.Controls.Add(Me.label2)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximumSize = New System.Drawing.Size(720, 480)
		Me.MinimumSize = New System.Drawing.Size(720, 180)
		Me.Name = "Lector_Fac"
		Me.Opacity = 0.8
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Nova Gear - Lector de Facturas"
		AddHandler Me.Load, New System.EventHandler(AddressOf Me.Lector_FacLoad)
		Me.panel1.ResumeLayout(False)
		Me.groupBox2.ResumeLayout(False)
		Me.groupBox4.ResumeLayout(False)
		Me.groupBox3.ResumeLayout(False)
		Me.groupBox1.ResumeLayout(False)
		Me.panel2.ResumeLayout(False)
		Me.panel2.PerformLayout()
		Me.panel3.ResumeLayout(False)
		DirectCast(Me.dataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
	Private label6 As System.Windows.Forms.Label
	Private label7 As System.Windows.Forms.Label
	Private progressBar2 As System.Windows.Forms.ProgressBar
	Private panel3 As System.Windows.Forms.Panel
	Private button9 As System.Windows.Forms.Button
	Private button8 As System.Windows.Forms.Button
	Private dataGridView2 As System.Windows.Forms.DataGridView
	Private comboBox1 As System.Windows.Forms.ComboBox
	Private label5 As System.Windows.Forms.Label
	Private label4 As System.Windows.Forms.Label
	Private textBox3 As System.Windows.Forms.TextBox
	Private label3 As System.Windows.Forms.Label
	Private button7 As System.Windows.Forms.Button
	Private button6 As System.Windows.Forms.Button
	Private dataGridView1 As System.Windows.Forms.DataGridView
	Private panel2 As System.Windows.Forms.Panel
	Private label2 As System.Windows.Forms.Label
	Private button5 As System.Windows.Forms.Button
	Private button4 As System.Windows.Forms.Button
	Private button3 As System.Windows.Forms.Button
	Private groupBox3 As System.Windows.Forms.GroupBox
	Private groupBox4 As System.Windows.Forms.GroupBox
	Private radioButton1 As System.Windows.Forms.RadioButton
	Private radioButton2 As System.Windows.Forms.RadioButton
	Private radioButton3 As System.Windows.Forms.RadioButton
	Private radioButton4 As System.Windows.Forms.RadioButton
	Private radioButton5 As System.Windows.Forms.RadioButton
	Private radioButton6 As System.Windows.Forms.RadioButton
	Private radioButton7 As System.Windows.Forms.RadioButton
	Private radioButton8 As System.Windows.Forms.RadioButton
	Private radioButton9 As System.Windows.Forms.RadioButton
	Private radioButton10 As System.Windows.Forms.RadioButton
	Private radioButton11 As System.Windows.Forms.RadioButton
	Private groupBox2 As System.Windows.Forms.GroupBox
	Private button2 As System.Windows.Forms.Button
	Private groupBox1 As System.Windows.Forms.GroupBox
	Private panel1 As System.Windows.Forms.Panel
	Private textBox2 As System.Windows.Forms.TextBox
	Private textBox1 As System.Windows.Forms.TextBox
	Private label1 As System.Windows.Forms.Label
	Private openFileDialog1 As System.Windows.Forms.OpenFileDialog
	Private progressBar1 As System.Windows.Forms.ProgressBar
	Private button1 As System.Windows.Forms.Button
End Class

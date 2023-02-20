/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 30/04/2015
 * Hora: 11:27 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace Nova_Gear
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Ingresar Facturas");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Capturar Fecha de Notificacion");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Depuracion");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Generar Reportes");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Consultar Patrón");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Capturar NN");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Sectorización");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Consulta de Estadísticas");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Ingresar Credito de Carga Manual");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Procesos de Notificación", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Capturar Fecha Notificación");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Capturar Cambio de Incidencia");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Capturar CM12, CM42");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Capturar SICOFI");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Automatización Siscob", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14});
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Agregar Usuario");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Editar Usuarios");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Capturar Fechas Cifra Control");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Generar Reporte Total");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Historial de Eventos");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Modificar Registros");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Modalidad 40");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Ingresar RALE");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Inventario");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Oficios");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Productividad Notificadores");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Supervisión", new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22,
            treeNode23,
            treeNode24,
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Nova Gear", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode15,
            treeNode27});
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lectorDeFacturasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.siscobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automatizadorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.procesoFinalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ocultarperfilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fechaNotificacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incidenciasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depuracionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autocobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button50 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.button39 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button49 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button36 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button47 = new System.Windows.Forms.Button();
            this.button45 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.button43 = new System.Windows.Forms.Button();
            this.button41 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button40 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button37 = new System.Windows.Forms.Button();
            this.button33 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button38 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button24 = new System.Windows.Forms.Button();
            this.button42 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button46 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button28 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button29 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusStrip1.BackgroundImage")));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(225, 719);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1044, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StatusStrip1ItemClicked);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.toolStripStatusLabel1.MergeIndex = 1;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(700, 17);
            this.toolStripStatusLabel1.Text = "Listo";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AutoToolTip = true;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusLabel2.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuStrip1.BackgroundImage")));
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuStrip1.Enabled = false;
            this.menuStrip1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.siscobToolStripMenuItem,
            this.ocultarperfilToolStripMenuItem,
            this.fechaNotificacionToolStripMenuItem,
            this.incidenciasToolStripMenuItem,
            this.depuracionToolStripMenuItem,
            this.reporteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(225, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(803, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lectorDeFacturasToolStripMenuItem,
            this.registrarToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.archivoToolStripMenuItem.Text = "Inicio";
            // 
            // lectorDeFacturasToolStripMenuItem
            // 
            this.lectorDeFacturasToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.lectorDeFacturasToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.lectorDeFacturasToolStripMenuItem.Name = "lectorDeFacturasToolStripMenuItem";
            this.lectorDeFacturasToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.lectorDeFacturasToolStripMenuItem.Text = "Lector de Facturas";
            this.lectorDeFacturasToolStripMenuItem.Click += new System.EventHandler(this.LectorDeFacturasToolStripMenuItemClick);
            // 
            // registrarToolStripMenuItem
            // 
            this.registrarToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.registrarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuariosToolStripMenuItem});
            this.registrarToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.registrarToolStripMenuItem.Name = "registrarToolStripMenuItem";
            this.registrarToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.registrarToolStripMenuItem.Text = "Registrar";
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.usuariosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.U)));
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.usuariosToolStripMenuItem.Text = "Usuarios";
            this.usuariosToolStripMenuItem.Click += new System.EventHandler(this.UsuariosToolStripMenuItemClick);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.salirToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.SalirToolStripMenuItemClick);
            // 
            // siscobToolStripMenuItem
            // 
            this.siscobToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.automatizadorToolStripMenuItem});
            this.siscobToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.siscobToolStripMenuItem.MergeIndex = 2;
            this.siscobToolStripMenuItem.Name = "siscobToolStripMenuItem";
            this.siscobToolStripMenuItem.Size = new System.Drawing.Size(69, 21);
            this.siscobToolStripMenuItem.Text = "Siscob";
            // 
            // automatizadorToolStripMenuItem
            // 
            this.automatizadorToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.automatizadorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.procesoFinalToolStripMenuItem});
            this.automatizadorToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.automatizadorToolStripMenuItem.Name = "automatizadorToolStripMenuItem";
            this.automatizadorToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.automatizadorToolStripMenuItem.Text = "Auto-Capturador";
            this.automatizadorToolStripMenuItem.Click += new System.EventHandler(this.AutomatizadorToolStripMenuItemClick);
            // 
            // procesoFinalToolStripMenuItem
            // 
            this.procesoFinalToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.procesoFinalToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.procesoFinalToolStripMenuItem.Name = "procesoFinalToolStripMenuItem";
            this.procesoFinalToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.procesoFinalToolStripMenuItem.Text = "Proceso Final";
            this.procesoFinalToolStripMenuItem.Visible = false;
            // 
            // ocultarperfilToolStripMenuItem
            // 
            this.ocultarperfilToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.ocultarperfilToolStripMenuItem.Name = "ocultarperfilToolStripMenuItem";
            this.ocultarperfilToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.Down)));
            this.ocultarperfilToolStripMenuItem.Size = new System.Drawing.Size(118, 21);
            this.ocultarperfilToolStripMenuItem.Text = "ocultar_perfil";
            this.ocultarperfilToolStripMenuItem.Visible = false;
            this.ocultarperfilToolStripMenuItem.Click += new System.EventHandler(this.OcultarperfilToolStripMenuItemClick);
            // 
            // fechaNotificacionToolStripMenuItem
            // 
            this.fechaNotificacionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fechaNotificacionToolStripMenuItem.Name = "fechaNotificacionToolStripMenuItem";
            this.fechaNotificacionToolStripMenuItem.Size = new System.Drawing.Size(150, 21);
            this.fechaNotificacionToolStripMenuItem.Text = "fecha notificacion";
            this.fechaNotificacionToolStripMenuItem.Click += new System.EventHandler(this.FechaNotificacionToolStripMenuItemClick);
            // 
            // incidenciasToolStripMenuItem
            // 
            this.incidenciasToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.incidenciasToolStripMenuItem.Name = "incidenciasToolStripMenuItem";
            this.incidenciasToolStripMenuItem.Size = new System.Drawing.Size(104, 21);
            this.incidenciasToolStripMenuItem.Text = "Incidencias";
            this.incidenciasToolStripMenuItem.Click += new System.EventHandler(this.IncidenciasToolStripMenuItemClick);
            // 
            // depuracionToolStripMenuItem
            // 
            this.depuracionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.depuracionToolStripMenuItem.Name = "depuracionToolStripMenuItem";
            this.depuracionToolStripMenuItem.Size = new System.Drawing.Size(103, 21);
            this.depuracionToolStripMenuItem.Text = "depuracion";
            this.depuracionToolStripMenuItem.Click += new System.EventHandler(this.DepuracionToolStripMenuItemClick);
            // 
            // reporteToolStripMenuItem
            // 
            this.reporteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consultaToolStripMenuItem,
            this.autocobToolStripMenuItem});
            this.reporteToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteToolStripMenuItem.Name = "reporteToolStripMenuItem";
            this.reporteToolStripMenuItem.Size = new System.Drawing.Size(75, 21);
            this.reporteToolStripMenuItem.Text = "reporte";
            this.reporteToolStripMenuItem.Click += new System.EventHandler(this.ReporteToolStripMenuItemClick);
            // 
            // consultaToolStripMenuItem
            // 
            this.consultaToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.consultaToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.consultaToolStripMenuItem.Name = "consultaToolStripMenuItem";
            this.consultaToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.consultaToolStripMenuItem.Text = "consulta";
            this.consultaToolStripMenuItem.Click += new System.EventHandler(this.ConsultaToolStripMenuItemClick);
            // 
            // autocobToolStripMenuItem
            // 
            this.autocobToolStripMenuItem.BackColor = System.Drawing.Color.Black;
            this.autocobToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.autocobToolStripMenuItem.Name = "autocobToolStripMenuItem";
            this.autocobToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.autocobToolStripMenuItem.Text = "auto_cob";
            this.autocobToolStripMenuItem.Click += new System.EventHandler(this.AutocobToolStripMenuItemClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(489, 11);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(205, 54);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(252, 130);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(240, 31);
            this.textBox1.TabIndex = 6;
            this.textBox1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 741);
            this.panel1.TabIndex = 8;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1Paint);
            // 
            // radioButton1
            // 
            this.radioButton1.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton1.Checked = true;
            this.radioButton1.FlatAppearance.BorderSize = 0;
            this.radioButton1.FlatAppearance.CheckedBackColor = System.Drawing.Color.DodgerBlue;
            this.radioButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.radioButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.ForeColor = System.Drawing.Color.White;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(218, 43);
            this.radioButton1.TabIndex = 21;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Procesos de\r\nNotificación";
            this.radioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.RadioButton1CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton2.FlatAppearance.BorderSize = 0;
            this.radioButton2.FlatAppearance.CheckedBackColor = System.Drawing.Color.DodgerBlue;
            this.radioButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.radioButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.ForeColor = System.Drawing.Color.White;
            this.radioButton2.Location = new System.Drawing.Point(3, 49);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(218, 43);
            this.radioButton2.TabIndex = 22;
            this.radioButton2.Text = "Automatización\r\nSISCOB/SINDO";
            this.radioButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.RadioButton2CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton3.FlatAppearance.BorderSize = 0;
            this.radioButton3.FlatAppearance.CheckedBackColor = System.Drawing.Color.DodgerBlue;
            this.radioButton3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.radioButton3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3.ForeColor = System.Drawing.Color.White;
            this.radioButton3.Location = new System.Drawing.Point(3, 95);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(218, 43);
            this.radioButton3.TabIndex = 23;
            this.radioButton3.Text = "Otros\r\nProcesos";
            this.radioButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.RadioButton3CheckedChanged);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.Controls.Add(this.button50);
            this.panel7.Controls.Add(this.button48);
            this.panel7.Controls.Add(this.button32);
            this.panel7.Controls.Add(this.button26);
            this.panel7.Controls.Add(this.button31);
            this.panel7.Controls.Add(this.button39);
            this.panel7.Location = new System.Drawing.Point(2, 166);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(221, 172);
            this.panel7.TabIndex = 38;
            this.panel7.Visible = false;
            // 
            // button50
            // 
            this.button50.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button50.Enabled = false;
            this.button50.FlatAppearance.BorderSize = 0;
            this.button50.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button50.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button50.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button50.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button50.ForeColor = System.Drawing.Color.White;
            this.button50.Image = global::Nova_Gear.Properties.Resources.speedometer;
            this.button50.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button50.Location = new System.Drawing.Point(148, 81);
            this.button50.Name = "button50";
            this.button50.Size = new System.Drawing.Size(70, 71);
            this.button50.TabIndex = 34;
            this.button50.Text = "Optimizar \r\nNova";
            this.button50.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button50.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button50.UseVisualStyleBackColor = true;
            this.button50.Visible = false;
            this.button50.Click += new System.EventHandler(this.button50_Click);
            // 
            // button48
            // 
            this.button48.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button48.Enabled = false;
            this.button48.FlatAppearance.BorderSize = 0;
            this.button48.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button48.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button48.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button48.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button48.ForeColor = System.Drawing.Color.White;
            this.button48.Image = global::Nova_Gear.Properties.Resources.document_valid;
            this.button48.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button48.Location = new System.Drawing.Point(4, 80);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(70, 72);
            this.button48.TabIndex = 27;
            this.button48.Text = "Aclaracion";
            this.button48.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button48.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button48.UseVisualStyleBackColor = true;
            this.button48.Click += new System.EventHandler(this.button48_Click);
            // 
            // button32
            // 
            this.button32.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button32.Enabled = false;
            this.button32.FlatAppearance.BorderSize = 0;
            this.button32.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button32.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button32.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button32.ForeColor = System.Drawing.Color.White;
            this.button32.Image = global::Nova_Gear.Properties.Resources.document_mark_as_final;
            this.button32.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button32.Location = new System.Drawing.Point(150, 3);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(70, 72);
            this.button32.TabIndex = 26;
            this.button32.Text = "Inventarios";
            this.button32.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new System.EventHandler(this.Button32Click);
            // 
            // button26
            // 
            this.button26.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button26.Enabled = false;
            this.button26.FlatAppearance.BorderSize = 0;
            this.button26.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button26.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button26.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button26.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button26.ForeColor = System.Drawing.Color.White;
            this.button26.Image = global::Nova_Gear.Properties.Resources.odbs_database;
            this.button26.Location = new System.Drawing.Point(77, 3);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(70, 72);
            this.button26.TabIndex = 22;
            this.button26.Text = "Modalidad 40";
            this.button26.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button31
            // 
            this.button31.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button31.Enabled = false;
            this.button31.FlatAppearance.BorderSize = 0;
            this.button31.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button31.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button31.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button31.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button31.ForeColor = System.Drawing.Color.White;
            this.button31.Image = global::Nova_Gear.Properties.Resources.financial_functions;
            this.button31.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button31.Location = new System.Drawing.Point(4, 3);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(70, 72);
            this.button31.TabIndex = 25;
            this.button31.Text = "R.A.L.E.";
            this.button31.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button31.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new System.EventHandler(this.Button31Click);
            // 
            // button39
            // 
            this.button39.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button39.Enabled = false;
            this.button39.FlatAppearance.BorderSize = 0;
            this.button39.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button39.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button39.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button39.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button39.ForeColor = System.Drawing.Color.White;
            this.button39.Image = global::Nova_Gear.Properties.Resources.document_inspector;
            this.button39.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button39.Location = new System.Drawing.Point(77, 81);
            this.button39.Name = "button39";
            this.button39.Size = new System.Drawing.Size(70, 71);
            this.button39.TabIndex = 33;
            this.button39.Text = "Iniciar\r\nNova";
            this.button39.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button39.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button39.UseVisualStyleBackColor = true;
            this.button39.Visible = false;
            this.button39.Click += new System.EventHandler(this.Button39Click);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(2, 135);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(220, 18);
            this.label8.TabIndex = 19;
            this.label8.Text = "_______________________";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.Controls.Add(this.button49);
            this.panel6.Controls.Add(this.button6);
            this.panel6.Controls.Add(this.button12);
            this.panel6.Controls.Add(this.button36);
            this.panel6.Controls.Add(this.button16);
            this.panel6.Controls.Add(this.button35);
            this.panel6.Controls.Add(this.button18);
            this.panel6.Location = new System.Drawing.Point(2, 166);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(221, 230);
            this.panel6.TabIndex = 37;
            this.panel6.Visible = false;
            // 
            // button49
            // 
            this.button49.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button49.Enabled = false;
            this.button49.FlatAppearance.BorderSize = 0;
            this.button49.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button49.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button49.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button49.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button49.ForeColor = System.Drawing.Color.White;
            this.button49.Image = global::Nova_Gear.Properties.Resources.user_bender;
            this.button49.Location = new System.Drawing.Point(4, 155);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(70, 72);
            this.button49.TabIndex = 31;
            this.button49.Text = "Auto \r\nFENIX";
            this.button49.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button49.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button49, "Automatización SISCOB\r\nModificacion de Sectores");
            this.button49.UseVisualStyleBackColor = true;
            this.button49.Click += new System.EventHandler(this.button49_Click);
            // 
            // button6
            // 
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button6.Enabled = false;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Image = global::Nova_Gear.Properties.Resources.user_robocop;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button6.Location = new System.Drawing.Point(4, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(70, 72);
            this.button6.TabIndex = 7;
            this.button6.Text = "SISCOB Fechas";
            this.button6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6Click);
            // 
            // button12
            // 
            this.button12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button12.Enabled = false;
            this.button12.FlatAppearance.BorderSize = 0;
            this.button12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button12.ForeColor = System.Drawing.Color.White;
            this.button12.Image = global::Nova_Gear.Properties.Resources.user_ironman;
            this.button12.Location = new System.Drawing.Point(77, 3);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(70, 72);
            this.button12.TabIndex = 8;
            this.button12.Text = "SISCOB Incidencia";
            this.button12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.Button12Click);
            // 
            // button36
            // 
            this.button36.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button36.Enabled = false;
            this.button36.FlatAppearance.BorderSize = 0;
            this.button36.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button36.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button36.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button36.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button36.ForeColor = System.Drawing.Color.White;
            this.button36.Image = global::Nova_Gear.Properties.Resources.user_c3po;
            this.button36.Location = new System.Drawing.Point(77, 80);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(70, 72);
            this.button36.TabIndex = 30;
            this.button36.Text = "SISCOB Sectores";
            this.button36.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button36.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button36, "Automatización SISCOB\r\nModificacion de Sectores");
            this.button36.UseVisualStyleBackColor = true;
            this.button36.Click += new System.EventHandler(this.Button36Click);
            // 
            // button16
            // 
            this.button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button16.Enabled = false;
            this.button16.FlatAppearance.BorderSize = 0;
            this.button16.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button16.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button16.ForeColor = System.Drawing.Color.White;
            this.button16.Image = global::Nova_Gear.Properties.Resources.sharepoint;
            this.button16.Location = new System.Drawing.Point(150, 3);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(70, 72);
            this.button16.TabIndex = 9;
            this.button16.Text = "SISCOB Envíos";
            this.button16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.Button16Click);
            // 
            // button35
            // 
            this.button35.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button35.Enabled = false;
            this.button35.FlatAppearance.BorderSize = 0;
            this.button35.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button35.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button35.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button35.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button35.ForeColor = System.Drawing.Color.White;
            this.button35.Image = global::Nova_Gear.Properties.Resources.user_r2d2;
            this.button35.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button35.Location = new System.Drawing.Point(4, 80);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(70, 72);
            this.button35.TabIndex = 29;
            this.button35.Text = "Consulta SINDO";
            this.button35.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button35.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button35, "Automatizacion SINDO\r\nConsulta de Patrones");
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.Button35Click);
            // 
            // button18
            // 
            this.button18.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button18.Enabled = false;
            this.button18.FlatAppearance.BorderSize = 0;
            this.button18.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button18.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button18.ForeColor = System.Drawing.Color.White;
            this.button18.Image = global::Nova_Gear.Properties.Resources.user_trooper_captain;
            this.button18.Location = new System.Drawing.Point(150, 80);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(70, 72);
            this.button18.TabIndex = 10;
            this.button18.Text = "SISCOB Alt";
            this.button18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.Button18Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_right;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.Location = new System.Drawing.Point(8, 697);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(132, 32);
            this.button4.TabIndex = 18;
            this.button4.Text = "Cerrar";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.Button4Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.button47);
            this.panel4.Controls.Add(this.button45);
            this.panel4.Controls.Add(this.button14);
            this.panel4.Controls.Add(this.button44);
            this.panel4.Controls.Add(this.button43);
            this.panel4.Controls.Add(this.button41);
            this.panel4.Controls.Add(this.button5);
            this.panel4.Controls.Add(this.button8);
            this.panel4.Controls.Add(this.button13);
            this.panel4.Controls.Add(this.button40);
            this.panel4.Controls.Add(this.button15);
            this.panel4.Controls.Add(this.button20);
            this.panel4.Controls.Add(this.button34);
            this.panel4.Controls.Add(this.button21);
            this.panel4.Controls.Add(this.button25);
            this.panel4.Controls.Add(this.button27);
            this.panel4.Controls.Add(this.button30);
            this.panel4.Location = new System.Drawing.Point(2, 166);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(221, 510);
            this.panel4.TabIndex = 14;
            // 
            // button47
            // 
            this.button47.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button47.Enabled = false;
            this.button47.FlatAppearance.BorderSize = 0;
            this.button47.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button47.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button47.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button47.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button47.ForeColor = System.Drawing.Color.White;
            this.button47.Image = global::Nova_Gear.Properties.Resources.to_do_list_checked_all;
            this.button47.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button47.Location = new System.Drawing.Point(77, 390);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(70, 72);
            this.button47.TabIndex = 39;
            this.button47.Text = "Edición Multiple";
            this.button47.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button47.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button47.UseVisualStyleBackColor = true;
            this.button47.Click += new System.EventHandler(this.button47_Click);
            // 
            // button45
            // 
            this.button45.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button45.Enabled = false;
            this.button45.FlatAppearance.BorderSize = 0;
            this.button45.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button45.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button45.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button45.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button45.ForeColor = System.Drawing.Color.White;
            this.button45.Image = global::Nova_Gear.Properties.Resources.file_extension_pdf;
            this.button45.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button45.Location = new System.Drawing.Point(4, 390);
            this.button45.Name = "button45";
            this.button45.Size = new System.Drawing.Size(70, 72);
            this.button45.TabIndex = 38;
            this.button45.Text = "Indexación PDFs";
            this.button45.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button45.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button45, "Sección Para la Diligencia \r\nde Créditos de la Propuesta");
            this.button45.UseVisualStyleBackColor = true;
            this.button45.Click += new System.EventHandler(this.button45_Click);
            // 
            // button14
            // 
            this.button14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button14.Enabled = false;
            this.button14.FlatAppearance.BorderSize = 0;
            this.button14.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button14.ForeColor = System.Drawing.Color.White;
            this.button14.Image = global::Nova_Gear.Properties.Resources.magnifier;
            this.button14.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button14.Location = new System.Drawing.Point(77, 235);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(70, 72);
            this.button14.TabIndex = 4;
            this.button14.Text = "Consulta";
            this.button14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.Button14Click);
            // 
            // button44
            // 
            this.button44.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button44.Enabled = false;
            this.button44.FlatAppearance.BorderSize = 0;
            this.button44.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button44.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button44.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button44.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button44.ForeColor = System.Drawing.Color.White;
            this.button44.Image = global::Nova_Gear.Properties.Resources.mailing_list;
            this.button44.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button44.Location = new System.Drawing.Point(150, 312);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(70, 72);
            this.button44.TabIndex = 37;
            this.button44.Text = "Propuesta";
            this.button44.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button44.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button44, "Sección Para la Diligencia \r\nde Créditos de la Propuesta");
            this.button44.UseVisualStyleBackColor = true;
            this.button44.Click += new System.EventHandler(this.Button44Click);
            // 
            // button43
            // 
            this.button43.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button43.Enabled = false;
            this.button43.FlatAppearance.BorderSize = 0;
            this.button43.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button43.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button43.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button43.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button43.ForeColor = System.Drawing.Color.White;
            this.button43.Image = global::Nova_Gear.Properties.Resources.meeting_workspace;
            this.button43.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button43.Location = new System.Drawing.Point(77, 312);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(70, 72);
            this.button43.TabIndex = 36;
            this.button43.Text = "Entrega Especial";
            this.button43.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button43.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button43, "Entrega Especial");
            this.button43.UseVisualStyleBackColor = true;
            this.button43.Click += new System.EventHandler(this.Button43Click);
            // 
            // button41
            // 
            this.button41.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button41.Enabled = false;
            this.button41.FlatAppearance.BorderSize = 0;
            this.button41.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button41.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button41.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button41.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button41.ForeColor = System.Drawing.Color.White;
            this.button41.Image = global::Nova_Gear.Properties.Resources.user_level_filtering;
            this.button41.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button41.Location = new System.Drawing.Point(4, 312);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(70, 72);
            this.button41.TabIndex = 35;
            this.button41.Text = "Rol Sectores";
            this.button41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button41.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button41, "Asignar Notificadores y Sectores");
            this.button41.UseVisualStyleBackColor = true;
            this.button41.Click += new System.EventHandler(this.Button41Click);
            // 
            // button5
            // 
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button5.Enabled = false;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Image = global::Nova_Gear.Properties.Resources.text_imports;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button5.Location = new System.Drawing.Point(4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(70, 72);
            this.button5.TabIndex = 0;
            this.button5.Text = "Lector Factura";
            this.button5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5Click);
            // 
            // button8
            // 
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button8.Enabled = false;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button8.ForeColor = System.Drawing.Color.White;
            this.button8.Image = global::Nova_Gear.Properties.Resources.date_edit;
            this.button8.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button8.Location = new System.Drawing.Point(150, 81);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(70, 72);
            this.button8.TabIndex = 1;
            this.button8.Text = "Captura Resultados";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Button8Click);
            this.button8.MouseEnter += new System.EventHandler(this.Button8MouseEnter);
            // 
            // button13
            // 
            this.button13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button13.Enabled = false;
            this.button13.FlatAppearance.BorderSize = 0;
            this.button13.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button13.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.ForeColor = System.Drawing.Color.White;
            this.button13.Image = global::Nova_Gear.Properties.Resources.table_analysis;
            this.button13.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button13.Location = new System.Drawing.Point(150, 4);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(70, 72);
            this.button13.TabIndex = 2;
            this.button13.Text = "Depuracion";
            this.button13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.Button13Click);
            // 
            // button40
            // 
            this.button40.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button40.Enabled = false;
            this.button40.FlatAppearance.BorderSize = 0;
            this.button40.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button40.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button40.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button40.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button40.ForeColor = System.Drawing.Color.White;
            this.button40.Image = global::Nova_Gear.Properties.Resources.upload_for_cloud_white;
            this.button40.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button40.Location = new System.Drawing.Point(4, 158);
            this.button40.Name = "button40";
            this.button40.Size = new System.Drawing.Size(70, 72);
            this.button40.TabIndex = 34;
            this.button40.Text = "Estrados";
            this.button40.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button40.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button40.UseVisualStyleBackColor = true;
            this.button40.Click += new System.EventHandler(this.Button40Click);
            // 
            // button15
            // 
            this.button15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button15.Enabled = false;
            this.button15.FlatAppearance.BorderSize = 0;
            this.button15.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.ForeColor = System.Drawing.Color.White;
            this.button15.Image = global::Nova_Gear.Properties.Resources.document_index;
            this.button15.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button15.Location = new System.Drawing.Point(77, 81);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(70, 72);
            this.button15.TabIndex = 3;
            this.button15.Text = " Generar Reportes";
            this.button15.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.Button15Click);
            // 
            // button20
            // 
            this.button20.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button20.Enabled = false;
            this.button20.FlatAppearance.BorderSize = 0;
            this.button20.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button20.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button20.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button20.ForeColor = System.Drawing.Color.White;
            this.button20.Image = global::Nova_Gear.Properties.Resources.google_map;
            this.button20.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button20.Location = new System.Drawing.Point(4, 81);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(70, 72);
            this.button20.TabIndex = 6;
            this.button20.Text = "Sectorizar";
            this.button20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.Button20Click);
            // 
            // button34
            // 
            this.button34.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button34.Enabled = false;
            this.button34.FlatAppearance.BorderSize = 0;
            this.button34.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button34.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button34.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button34.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button34.ForeColor = System.Drawing.Color.White;
            this.button34.Image = global::Nova_Gear.Properties.Resources.document_protect;
            this.button34.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button34.Location = new System.Drawing.Point(77, 158);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(70, 72);
            this.button34.TabIndex = 28;
            this.button34.Text = "Oficios";
            this.button34.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button34.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.Button34Click);
            // 
            // button21
            // 
            this.button21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button21.Enabled = false;
            this.button21.FlatAppearance.BorderSize = 0;
            this.button21.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button21.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button21.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button21.ForeColor = System.Drawing.Color.White;
            this.button21.Image = global::Nova_Gear.Properties.Resources.date_add;
            this.button21.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button21.Location = new System.Drawing.Point(77, 4);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(70, 72);
            this.button21.TabIndex = 13;
            this.button21.Text = "Registro Productos";
            this.button21.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.Button21Click);
            // 
            // button25
            // 
            this.button25.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button25.Enabled = false;
            this.button25.FlatAppearance.BorderSize = 0;
            this.button25.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button25.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button25.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button25.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button25.ForeColor = System.Drawing.Color.White;
            this.button25.Image = global::Nova_Gear.Properties.Resources.application_form_edit;
            this.button25.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button25.Location = new System.Drawing.Point(150, 235);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(70, 72);
            this.button25.TabIndex = 21;
            this.button25.Text = "Actualizar Crédito";
            this.button25.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button25.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.Button25Click);
            // 
            // button27
            // 
            this.button27.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button27.Enabled = false;
            this.button27.FlatAppearance.BorderSize = 0;
            this.button27.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button27.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button27.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button27.ForeColor = System.Drawing.Color.White;
            this.button27.Image = global::Nova_Gear.Properties.Resources.statistics;
            this.button27.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button27.Location = new System.Drawing.Point(4, 235);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(70, 72);
            this.button27.TabIndex = 23;
            this.button27.Text = "Supervisión Notificación";
            this.button27.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button27.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.Button27Click);
            // 
            // button30
            // 
            this.button30.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button30.Enabled = false;
            this.button30.FlatAppearance.BorderSize = 0;
            this.button30.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button30.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button30.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button30.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.button30.ForeColor = System.Drawing.Color.White;
            this.button30.Image = global::Nova_Gear.Properties.Resources.application_form_add;
            this.button30.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button30.Location = new System.Drawing.Point(150, 158);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(70, 72);
            this.button30.TabIndex = 24;
            this.button30.Text = "Carga Manual";
            this.button30.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button30.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button30.UseVisualStyleBackColor = true;
            this.button30.Click += new System.EventHandler(this.Button30Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(417, 319);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(164, 36);
            this.label9.TabIndex = 20;
            this.label9.Text = "Otros \r\nProcesos";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label9.Visible = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(417, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 41);
            this.label7.TabIndex = 18;
            this.label7.Text = "Automatización\r\nSISCOB/SINDO";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label7.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(417, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 18);
            this.label6.TabIndex = 17;
            this.label6.Text = "________________";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(334, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 41);
            this.label5.TabIndex = 16;
            this.label5.Text = "Procesos de \r\nNotificación";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Visible = false;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // button37
            // 
            this.button37.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button37.FlatAppearance.BorderSize = 0;
            this.button37.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button37.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button37.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button37.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button37.ForeColor = System.Drawing.Color.White;
            this.button37.Image = global::Nova_Gear.Properties.Resources.table_chart;
            this.button37.Location = new System.Drawing.Point(226, 501);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(40, 40);
            this.button37.TabIndex = 31;
            this.button37.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.button37, "Analisis de \r\nVigentes \r\ncon RALE");
            this.button37.UseVisualStyleBackColor = true;
            this.button37.Visible = false;
            this.button37.Click += new System.EventHandler(this.Button37Click);
            // 
            // button33
            // 
            this.button33.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button33.FlatAppearance.BorderSize = 0;
            this.button33.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button33.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button33.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button33.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button33.ForeColor = System.Drawing.Color.White;
            this.button33.Image = global::Nova_Gear.Properties.Resources.chart_bar;
            this.button33.Location = new System.Drawing.Point(272, 501);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(40, 40);
            this.button33.TabIndex = 27;
            this.button33.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Visible = false;
            this.button33.Click += new System.EventHandler(this.Button33Click);
            // 
            // button23
            // 
            this.button23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button23.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button23.Enabled = false;
            this.button23.FlatAppearance.BorderSize = 0;
            this.button23.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button23.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button23.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button23.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button23.ForeColor = System.Drawing.Color.White;
            this.button23.Image = global::Nova_Gear.Properties.Resources.file_extension_log;
            this.button23.Location = new System.Drawing.Point(74, 111);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(60, 76);
            this.button23.TabIndex = 15;
            this.button23.Text = "Historial\r\nEventos";
            this.button23.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button23.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.Button23Click);
            // 
            // button17
            // 
            this.button17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button17.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button17.Enabled = false;
            this.button17.FlatAppearance.BorderSize = 0;
            this.button17.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button17.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.ForeColor = System.Drawing.Color.White;
            this.button17.Image = global::Nova_Gear.Properties.Resources.allow_users_edit_ranges;
            this.button17.Location = new System.Drawing.Point(8, 111);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(62, 76);
            this.button17.TabIndex = 12;
            this.button17.Text = "Ajustes Usuarios";
            this.button17.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.Button17Click);
            // 
            // button7
            // 
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Image = global::Nova_Gear.Properties.Resources.user_add;
            this.button7.Location = new System.Drawing.Point(226, 440);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(40, 40);
            this.button7.TabIndex = 11;
            this.button7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.Button7Click);
            // 
            // button38
            // 
            this.button38.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button38.FlatAppearance.BorderSize = 0;
            this.button38.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button38.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button38.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button38.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button38.ForeColor = System.Drawing.Color.White;
            this.button38.Image = global::Nova_Gear.Properties.Resources.document_editing;
            this.button38.Location = new System.Drawing.Point(226, 362);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(40, 40);
            this.button38.TabIndex = 32;
            this.button38.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.button38, "Reimpresion\r\nFactura \r\nIndividual");
            this.button38.UseVisualStyleBackColor = true;
            this.button38.Visible = false;
            this.button38.Click += new System.EventHandler(this.Button38Click);
            // 
            // button22
            // 
            this.button22.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button22.FlatAppearance.BorderSize = 0;
            this.button22.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button22.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button22.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button22.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button22.ForeColor = System.Drawing.Color.White;
            this.button22.Image = global::Nova_Gear.Properties.Resources.table_layout_grand_totals;
            this.button22.Location = new System.Drawing.Point(272, 362);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(40, 40);
            this.button22.TabIndex = 14;
            this.button22.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Visible = false;
            this.button22.Click += new System.EventHandler(this.Button22Click);
            // 
            // button19
            // 
            this.button19.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button19.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button19.ForeColor = System.Drawing.Color.White;
            this.button19.Image = global::Nova_Gear.Properties.Resources.http_status_not_found;
            this.button19.Location = new System.Drawing.Point(226, 201);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(40, 40);
            this.button19.TabIndex = 5;
            this.button19.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Visible = false;
            this.button19.Click += new System.EventHandler(this.Button19Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = global::Nova_Gear.Properties.Resources.picture_error;
            this.pictureBox1.InitialImage = global::Nova_Gear.Properties.Resources.user_silhouette;
            this.pictureBox1.Location = new System.Drawing.Point(32, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(63, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(101, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 67);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nombre Apellido";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(652, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Usuario";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(645, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Area";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(645, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Puesto";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Visible = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Image = global::Nova_Gear.Properties.Resources.key;
            this.button3.Location = new System.Drawing.Point(104, 189);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 69);
            this.button3.TabIndex = 19;
            this.button3.Text = "Cerrar Sesión";
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Image = global::Nova_Gear.Properties.Resources.setting_tools_1;
            this.button2.Location = new System.Drawing.Point(138, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 76);
            this.button2.TabIndex = 20;
            this.button2.Text = "Ajustes\r\nPrime";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_left;
            this.button1.Location = new System.Drawing.Point(232, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 16;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Nova Gear";
            // 
            // button24
            // 
            this.button24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button24.BackColor = System.Drawing.Color.Transparent;
            this.button24.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button24.FlatAppearance.BorderSize = 0;
            this.button24.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button24.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(15)))), ((int)(((byte)(115)))));
            this.button24.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button24.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button24.Image = global::Nova_Gear.Properties.Resources.information_1;
            this.button24.Location = new System.Drawing.Point(0, 0);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(24, 24);
            this.button24.TabIndex = 17;
            this.toolTip1.SetToolTip(this.button24, "Acerca de...");
            this.button24.UseVisualStyleBackColor = false;
            this.button24.Click += new System.EventHandler(this.Button24Click);
            // 
            // button42
            // 
            this.button42.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.button42.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button42.FlatAppearance.BorderSize = 0;
            this.button42.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button42.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button42.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button42.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button42.Image = global::Nova_Gear.Properties.Resources.user_astronaut;
            this.button42.Location = new System.Drawing.Point(1235, 12);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(24, 24);
            this.button42.TabIndex = 35;
            this.toolTip1.SetToolTip(this.button42, "Ajustes\r\nExtras");
            this.button42.UseVisualStyleBackColor = false;
            this.button42.Click += new System.EventHandler(this.Button42Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.button46);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.button17);
            this.panel3.Controls.Add(this.button23);
            this.panel3.Controls.Add(this.button24);
            this.panel3.Location = new System.Drawing.Point(986, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(268, 271);
            this.panel3.TabIndex = 12;
            this.panel3.Visible = false;
            // 
            // button46
            // 
            this.button46.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button46.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button46.FlatAppearance.BorderSize = 0;
            this.button46.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button46.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button46.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button46.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button46.ForeColor = System.Drawing.Color.White;
            this.button46.Image = global::Nova_Gear.Properties.Resources.nova_button_2;
            this.button46.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button46.Location = new System.Drawing.Point(204, 111);
            this.button46.Name = "button46";
            this.button46.Size = new System.Drawing.Size(62, 76);
            this.button46.TabIndex = 21;
            this.button46.Text = "\r\nManual Nova";
            this.button46.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button46.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button46.UseVisualStyleBackColor = true;
            this.button46.Click += new System.EventHandler(this.button46_Click);
            // 
            // button11
            // 
            this.button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button11.FlatAppearance.BorderSize = 0;
            this.button11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.ForeColor = System.Drawing.Color.White;
            this.button11.Image = global::Nova_Gear.Properties.Resources.inbox_table;
            this.button11.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button11.Location = new System.Drawing.Point(532, 158);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(66, 67);
            this.button11.TabIndex = 13;
            this.button11.Text = "Captura\r\nCartera";
            this.button11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Visible = false;
            this.button11.Click += new System.EventHandler(this.Button11Click);
            // 
            // button9
            // 
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49)))));
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.Image = global::Nova_Gear.Properties.Resources.table_key;
            this.button9.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button9.Location = new System.Drawing.Point(652, 130);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(84, 80);
            this.button9.TabIndex = 12;
            this.button9.Text = "Captura\r\nSupervisión";
            this.button9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            // 
            // button10
            // 
            this.button10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(6)))), ((int)(((byte)(49)))));
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.ForeColor = System.Drawing.Color.White;
            this.button10.Image = global::Nova_Gear.Properties.Resources.table_select;
            this.button10.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button10.Location = new System.Drawing.Point(513, 81);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(85, 80);
            this.button10.TabIndex = 11;
            this.button10.Text = "Caso Específico";
            this.button10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.button28);
            this.panel2.Controls.Add(this.dataGridView2);
            this.panel2.Location = new System.Drawing.Point(789, 530);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(478, 187);
            this.panel2.TabIndex = 19;
            this.panel2.Visible = false;
            // 
            // button28
            // 
            this.button28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button28.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button28.FlatAppearance.BorderSize = 0;
            this.button28.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button28.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.button28.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button28.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button28.ForeColor = System.Drawing.Color.White;
            this.button28.Image = global::Nova_Gear.Properties.Resources.to_do_list_checked_1;
            this.button28.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button28.Location = new System.Drawing.Point(389, 70);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(80, 57);
            this.button28.TabIndex = 24;
            this.button28.Text = "Revisar";
            this.button28.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new System.EventHandler(this.Button28Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(14, 36);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(367, 137);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView2CellContentClick);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.button29);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Location = new System.Drawing.Point(791, 532);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(474, 23);
            this.panel5.TabIndex = 1;
            this.panel5.Visible = false;
            // 
            // button29
            // 
            this.button29.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button29.FlatAppearance.BorderSize = 0;
            this.button29.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.button29.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.button29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button29.ForeColor = System.Drawing.Color.White;
            this.button29.Image = global::Nova_Gear.Properties.Resources.bullet_arrow_down;
            this.button29.Location = new System.Drawing.Point(446, 0);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(25, 21);
            this.button29.TabIndex = 21;
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new System.EventHandler(this.Button29Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(16, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(196, 21);
            this.label10.TabIndex = 0;
            this.label10.Text = "Créditos en trámite";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.FullRowSelect = true;
            this.treeView1.HotTracking = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(595, 286);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Nodo2";
            treeNode1.Text = "Ingresar Facturas";
            treeNode2.Name = "Nodo13";
            treeNode2.Text = "Capturar Fecha de Notificacion";
            treeNode3.Name = "Nodo14";
            treeNode3.Text = "Depuracion";
            treeNode4.Name = "Nodo15";
            treeNode4.Text = "Generar Reportes";
            treeNode5.Name = "Nodo16";
            treeNode5.Text = "Consultar Patrón";
            treeNode6.Name = "Nodo17";
            treeNode6.Text = "Capturar NN";
            treeNode7.Name = "Nodo18";
            treeNode7.Text = "Sectorización";
            treeNode8.Name = "Nodo19";
            treeNode8.Text = "Consulta de Estadísticas";
            treeNode9.Name = "Nodo20";
            treeNode9.Text = "Ingresar Credito de Carga Manual";
            treeNode10.Name = "Nodo1";
            treeNode10.Text = "Procesos de Notificación";
            treeNode11.Name = "Nodo8";
            treeNode11.Text = "Capturar Fecha Notificación";
            treeNode12.Name = "Nodo9";
            treeNode12.Text = "Capturar Cambio de Incidencia";
            treeNode13.Name = "Nodo21";
            treeNode13.Text = "Capturar CM12, CM42";
            treeNode14.Name = "Nodo22";
            treeNode14.Text = "Capturar SICOFI";
            treeNode15.Name = "Nodo6";
            treeNode15.Text = "Automatización Siscob";
            treeNode16.Name = "Nodo10";
            treeNode16.Text = "Agregar Usuario";
            treeNode17.Name = "Nodo23";
            treeNode17.Text = "Editar Usuarios";
            treeNode18.Name = "Nodo24";
            treeNode18.Text = "Capturar Fechas Cifra Control";
            treeNode19.Name = "Nodo25";
            treeNode19.Text = "Generar Reporte Total";
            treeNode20.Name = "Nodo26";
            treeNode20.Text = "Historial de Eventos";
            treeNode21.Name = "Nodo27";
            treeNode21.Text = "Modificar Registros";
            treeNode22.Name = "Nodo28";
            treeNode22.Text = "Modalidad 40";
            treeNode23.Name = "Nodo29";
            treeNode23.Text = "Ingresar RALE";
            treeNode24.Name = "Nodo30";
            treeNode24.Text = "Inventario";
            treeNode25.Name = "Nodo31";
            treeNode25.Text = "Oficios";
            treeNode26.Name = "Nodo32";
            treeNode26.Text = "Productividad Notificadores";
            treeNode27.Name = "Nodo7";
            treeNode27.Text = "Supervisión";
            treeNode28.Name = "Nodo0";
            treeNode28.Text = "Nova Gear";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode28});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(212, 262);
            this.treeView1.TabIndex = 21;
            this.treeView1.Visible = false;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "account_balances.png");
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1269, 741);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button42);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button37);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button38);
            this.Controls.Add(this.button33);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gear Prime";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button button44;
		private System.Windows.Forms.Button button41;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button button42;
		private System.Windows.Forms.Button button40;
		private System.Windows.Forms.Button button39;
		private System.Windows.Forms.Button button38;
		private System.Windows.Forms.Button button37;
		private System.Windows.Forms.Button button36;
		private System.Windows.Forms.Button button35;
		private System.Windows.Forms.Button button34;
		private System.Windows.Forms.Button button33;
		private System.Windows.Forms.Button button32;
		private System.Windows.Forms.Button button31;
		private System.Windows.Forms.Button button30;
		private System.Windows.Forms.Button button29;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button button28;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button button27;
		private System.Windows.Forms.Button button25;
		private System.Windows.Forms.Button button24;
		private System.Windows.Forms.Button button23;
		private System.Windows.Forms.Button button22;
		private System.Windows.Forms.Button button21;
		private System.Windows.Forms.Button button20;
		private System.Windows.Forms.Button button19;
		private System.Windows.Forms.Button button18;
		private System.Windows.Forms.Button button17;
		private System.Windows.Forms.ToolStripMenuItem autocobToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem consultaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reporteToolStripMenuItem;
		private System.Windows.Forms.Button button16;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.ToolStripMenuItem depuracionToolStripMenuItem;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.ToolStripMenuItem incidenciasToolStripMenuItem;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.ToolStripMenuItem fechaNotificacionToolStripMenuItem;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ToolStripMenuItem ocultarperfilToolStripMenuItem;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
		public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripMenuItem usuariosToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem registrarToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.ToolStripMenuItem procesoFinalToolStripMenuItem;
		public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripMenuItem automatizadorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem siscobToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lectorDeFacturasToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button43;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button45;
        private System.Windows.Forms.Button button46;
        private System.Windows.Forms.Button button47;
        private System.Windows.Forms.Button button48;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Button button49;
        private System.Windows.Forms.Button button50;
    }
}

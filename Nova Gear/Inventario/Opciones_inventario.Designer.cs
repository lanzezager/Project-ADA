/*
 * Creado por SharpDevelop.
 * Usuario: LanzeZager
 * Fecha: 31/10/2017
 * Hora: 03:04 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace Nova_Gear.Inventario
{
	partial class Opciones_inventario
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button12 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button11 = new System.Windows.Forms.Button();
			this.button10 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.button9 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			this.tabPage3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabControl1.Location = new System.Drawing.Point(13, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(662, 341);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.SteelBlue;
			this.tabPage1.Controls.Add(this.button12);
			this.tabPage1.Controls.Add(this.button3);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.dataGridView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 23);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(654, 314);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Responsables";
			// 
			// button12
			// 
			this.button12.Image = global::Nova_Gear.Properties.Resources.table_save;
			this.button12.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button12.Location = new System.Drawing.Point(572, 214);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(75, 94);
			this.button12.TabIndex = 4;
			this.button12.Text = "Guardar\r\nLibro en\r\nla B.D.";
			this.button12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolTip1.SetToolTip(this.button12, "Actualizar número de Libro en \r\nlos Créditos de la base de datos.");
			this.button12.UseVisualStyleBackColor = true;
			this.button12.Click += new System.EventHandler(this.Button12Click);
			// 
			// button3
			// 
			this.button3.Image = global::Nova_Gear.Properties.Resources.user_delete;
			this.button3.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button3.Location = new System.Drawing.Point(572, 145);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 63);
			this.button3.TabIndex = 3;
			this.button3.Text = "Quitar";
			this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.Button3Click);
			// 
			// button2
			// 
			this.button2.Image = global::Nova_Gear.Properties.Resources.user_edit;
			this.button2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button2.Location = new System.Drawing.Point(572, 76);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 63);
			this.button2.TabIndex = 2;
			this.button2.Text = "Editar";
			this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// button1
			// 
			this.button1.Image = global::Nova_Gear.Properties.Resources.user_add;
			this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button1.Location = new System.Drawing.Point(573, 7);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 63);
			this.button1.TabIndex = 1;
			this.button1.Text = "Agregar";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(6, 7);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.Size = new System.Drawing.Size(560, 301);
			this.dataGridView1.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.tabPage2.Controls.Add(this.button4);
			this.tabPage2.Controls.Add(this.button5);
			this.tabPage2.Controls.Add(this.button6);
			this.tabPage2.Controls.Add(this.dataGridView2);
			this.tabPage2.Location = new System.Drawing.Point(4, 23);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(654, 314);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Auxiliares";
			// 
			// button4
			// 
			this.button4.Image = global::Nova_Gear.Properties.Resources.user_delete;
			this.button4.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button4.Location = new System.Drawing.Point(572, 145);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 63);
			this.button4.TabIndex = 7;
			this.button4.Text = "Quitar";
			this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.Button4Click);
			// 
			// button5
			// 
			this.button5.Image = global::Nova_Gear.Properties.Resources.user_edit;
			this.button5.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button5.Location = new System.Drawing.Point(572, 76);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 63);
			this.button5.TabIndex = 6;
			this.button5.Text = "Editar";
			this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.Button5Click);
			// 
			// button6
			// 
			this.button6.Image = global::Nova_Gear.Properties.Resources.user_add;
			this.button6.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button6.Location = new System.Drawing.Point(573, 7);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 63);
			this.button6.TabIndex = 5;
			this.button6.Text = "Agregar";
			this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.Button6Click);
			// 
			// dataGridView2
			// 
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ControlLight;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Location = new System.Drawing.Point(6, 7);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.ReadOnly = true;
			this.dataGridView2.Size = new System.Drawing.Size(560, 301);
			this.dataGridView2.TabIndex = 4;
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.Color.DimGray;
			this.tabPage3.Controls.Add(this.panel1);
			this.tabPage3.Controls.Add(this.progressBar1);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.button9);
			this.tabPage3.Controls.Add(this.button8);
			this.tabPage3.Controls.Add(this.button7);
			this.tabPage3.Controls.Add(this.label4);
			this.tabPage3.Location = new System.Drawing.Point(4, 23);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(654, 314);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Respaldo y Restauración";
			this.tabPage3.Click += new System.EventHandler(this.TabPage3Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button11);
			this.panel1.Controls.Add(this.button10);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Location = new System.Drawing.Point(58, 172);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(548, 136);
			this.panel1.TabIndex = 11;
			this.panel1.Visible = false;
			// 
			// button11
			// 
			this.button11.Image = global::Nova_Gear.Properties.Resources.excel_imports;
			this.button11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button11.Location = new System.Drawing.Point(289, 39);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(135, 45);
			this.button11.TabIndex = 12;
			this.button11.Text = "Respaldo";
			this.button11.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += new System.EventHandler(this.Button11Click);
			// 
			// button10
			// 
			this.button10.Image = global::Nova_Gear.Properties.Resources.database;
			this.button10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button10.Location = new System.Drawing.Point(125, 39);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(135, 45);
			this.button10.TabIndex = 11;
			this.button10.Text = "Archivo RALE";
			this.button10.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.Button10Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(0, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(548, 23);
			this.label2.TabIndex = 10;
			this.label2.Text = "Cargar Datos desde:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(58, 246);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(548, 23);
			this.progressBar1.TabIndex = 13;
			this.progressBar1.Visible = false;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(58, 211);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(548, 23);
			this.label3.TabIndex = 12;
			this.label3.Text = "000%";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label3.Visible = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(6, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(642, 23);
			this.label1.TabIndex = 9;
			this.label1.Text = "Opciones de Respaldo y Restauración";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Click += new System.EventHandler(this.Label1Click);
			// 
			// button9
			// 
			this.button9.Image = global::Nova_Gear.Properties.Resources.excel_exports;
			this.button9.Location = new System.Drawing.Point(264, 75);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(127, 92);
			this.button9.TabIndex = 8;
			this.button9.Text = "Respaldar en \r\nExcel";
			this.button9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.Button9Click);
			// 
			// button8
			// 
			this.button8.Image = global::Nova_Gear.Properties.Resources.database_delete;
			this.button8.Location = new System.Drawing.Point(479, 75);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(127, 92);
			this.button8.TabIndex = 7;
			this.button8.Text = "Reiniciar\r\nBase de Datos";
			this.button8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.Button8Click);
			// 
			// button7
			// 
			this.button7.Image = global::Nova_Gear.Properties.Resources.database_add;
			this.button7.Location = new System.Drawing.Point(58, 75);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(127, 92);
			this.button7.TabIndex = 6;
			this.button7.Text = "Llenar\r\nBase de Datos";
			this.button7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.Button7Click);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(58, 272);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(548, 23);
			this.label4.TabIndex = 14;
			this.label4.Text = "Creando conexión con Excel...";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label4.Visible = false;
			// 
			// Opciones_inventario
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(687, 365);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::Nova_Gear.Properties.Resources.logo_nova_white_2;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Opciones_inventario";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nova Gear [Inventario] - Ajustes";
			this.Load += new System.EventHandler(this.Opciones_inventarioLoad);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
	}
}

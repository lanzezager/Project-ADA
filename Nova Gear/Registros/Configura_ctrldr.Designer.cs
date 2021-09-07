/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 11/04/2018
 * Hora: 09:42 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace Nova_Gear.Registros
{
	partial class Configura_ctrldr
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configura_ctrldr));
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.asignarNotificadorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editarSectoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label4 = new System.Windows.Forms.Label();
			this.listBox3 = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			this.contextMenuStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.listBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(17, 88);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(240, 289);
			this.listBox1.Sorted = true;
			this.listBox1.TabIndex = 0;
			this.listBox1.Click += new System.EventHandler(this.ListBox1Click);
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
			this.listBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListBox1KeyPress);
			this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox1MouseDown);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.asignarNotificadorToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(179, 26);
			// 
			// asignarNotificadorToolStripMenuItem
			// 
			this.asignarNotificadorToolStripMenuItem.Image = global::Nova_Gear.Properties.Resources.user_edit_1;
			this.asignarNotificadorToolStripMenuItem.Name = "asignarNotificadorToolStripMenuItem";
			this.asignarNotificadorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.asignarNotificadorToolStripMenuItem.Text = "Editar Notificadores";
			this.asignarNotificadorToolStripMenuItem.Click += new System.EventHandler(this.AsignarNotificadorToolStripMenuItemClick);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(17, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(684, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Control de Rol de Notificadores/Sectores";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Click += new System.EventHandler(this.Label1Click);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(17, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(240, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Controladores";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(311, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(240, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Notificadores";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// listBox2
			// 
			this.listBox2.ContextMenuStrip = this.contextMenuStrip2;
			this.listBox2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox2.FormattingEnabled = true;
			this.listBox2.ItemHeight = 15;
			this.listBox2.Location = new System.Drawing.Point(311, 88);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(240, 289);
			this.listBox2.Sorted = true;
			this.listBox2.TabIndex = 3;
			this.listBox2.SelectedIndexChanged += new System.EventHandler(this.ListBox2SelectedIndexChanged);
			this.listBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListBox2KeyPress);
			this.listBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox2MouseDown);
			// 
			// contextMenuStrip2
			// 
			this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.editarSectoresToolStripMenuItem});
			this.contextMenuStrip2.Name = "contextMenuStrip1";
			this.contextMenuStrip2.Size = new System.Drawing.Size(152, 26);
			// 
			// editarSectoresToolStripMenuItem
			// 
			this.editarSectoresToolStripMenuItem.Image = global::Nova_Gear.Properties.Resources.google_map_1;
			this.editarSectoresToolStripMenuItem.Name = "editarSectoresToolStripMenuItem";
			this.editarSectoresToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.editarSectoresToolStripMenuItem.Text = "Editar Sectores";
			this.editarSectoresToolStripMenuItem.Click += new System.EventHandler(this.EditarSectoresToolStripMenuItemClick);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(606, 53);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "Sectores";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// listBox3
			// 
			this.listBox3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox3.FormattingEnabled = true;
			this.listBox3.ItemHeight = 18;
			this.listBox3.Location = new System.Drawing.Point(606, 88);
			this.listBox3.Name = "listBox3";
			this.listBox3.Size = new System.Drawing.Size(95, 292);
			this.listBox3.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Image = global::Nova_Gear.Properties.Resources.resultset_next;
			this.label5.Location = new System.Drawing.Point(263, 211);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 43);
			this.label5.TabIndex = 8;
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Image = global::Nova_Gear.Properties.Resources.resultset_next;
			this.label6.Location = new System.Drawing.Point(559, 211);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 43);
			this.label6.TabIndex = 9;
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Transparent;
			this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
			this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.ForeColor = System.Drawing.Color.White;
			this.button1.Image = global::Nova_Gear.Properties.Resources.user_edit_1;
			this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button1.Location = new System.Drawing.Point(17, 383);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(240, 31);
			this.button1.TabIndex = 15;
			this.button1.Text = "   Editar Notificadores";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Transparent;
			this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.button2.FlatAppearance.BorderSize = 0;
			this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
			this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.ForeColor = System.Drawing.Color.White;
			this.button2.Image = global::Nova_Gear.Properties.Resources.google_map_1;
			this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button2.Location = new System.Drawing.Point(311, 383);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(240, 31);
			this.button2.TabIndex = 16;
			this.button2.Text = "  Editar Sectores";
			this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// Configura_ctrldr
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(717, 438);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.listBox3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::Nova_Gear.Properties.Resources.logo_nova_white_2;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Configura_ctrldr";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nova Gear - Control de Rol de Notificadores/Sectores";
			this.Load += new System.EventHandler(this.Configura_ctrldrLoad);
			this.contextMenuStrip1.ResumeLayout(false);
			this.contextMenuStrip2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolStripMenuItem editarSectoresToolStripMenuItem;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
		private System.Windows.Forms.ToolStripMenuItem asignarNotificadorToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox listBox3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBox1;
	}
}

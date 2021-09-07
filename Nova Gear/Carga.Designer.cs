/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 13/08/2015
 * Hora: 02:15 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace Nova_Gear
{
	partial class Carga
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Carga));
			this.label1 = new System.Windows.Forms.Label();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.button10 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(263, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "MODO DE CAPTURA:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// radioButton1
			// 
			this.radioButton1.BackColor = System.Drawing.Color.Transparent;
			this.radioButton1.Enabled = false;
			this.radioButton1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radioButton1.ForeColor = System.Drawing.Color.White;
			this.radioButton1.Location = new System.Drawing.Point(30, 48);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(74, 24);
			this.radioButton1.TabIndex = 1;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "COP";
			this.radioButton1.UseVisualStyleBackColor = false;
			// 
			// radioButton2
			// 
			this.radioButton2.BackColor = System.Drawing.Color.Transparent;
			this.radioButton2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radioButton2.ForeColor = System.Drawing.Color.White;
			this.radioButton2.Location = new System.Drawing.Point(30, 80);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(74, 24);
			this.radioButton2.TabIndex = 2;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "RCV";
			this.radioButton2.UseVisualStyleBackColor = false;
			// 
			// button10
			// 
			this.button10.BackColor = System.Drawing.Color.Transparent;
			this.button10.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button10.FlatAppearance.BorderColor = System.Drawing.SystemColors.Menu;
			this.button10.FlatAppearance.BorderSize = 0;
			this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
			this.button10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
			this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button10.ForeColor = System.Drawing.Color.White;
			this.button10.Image = global::Nova_Gear.Properties.Resources.user_r2d2;
			this.button10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button10.Location = new System.Drawing.Point(136, 48);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(126, 56);
			this.button10.TabIndex = 17;
			this.button10.Text = " Aceptar";
			this.button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button10.UseVisualStyleBackColor = false;
			this.button10.Click += new System.EventHandler(this.Button10Click);
			// 
			// Carga
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(287, 127);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::Nova_Gear.Properties.Resources.logo_nova_white_2;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Carga";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nova Gear: Auto-Capturador Siscob";
			this.Load += new System.EventHandler(this.CargaLoad);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Label label1;
	}
}

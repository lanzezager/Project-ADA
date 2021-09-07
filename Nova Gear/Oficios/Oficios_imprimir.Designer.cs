namespace Nova_Gear.Oficios
{
    partial class Oficios_imprimir
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.label2 = new System.Windows.Forms.Label();
        	this.comboBox1 = new System.Windows.Forms.ComboBox();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
        	this.radioButton3 = new System.Windows.Forms.RadioButton();
        	this.radioButton2 = new System.Windows.Forms.RadioButton();
        	this.radioButton1 = new System.Windows.Forms.RadioButton();
        	this.button4 = new System.Windows.Forms.Button();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.label1 = new System.Windows.Forms.Label();
        	this.groupBox1.SuspendLayout();
        	this.panel1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// label2
        	// 
        	this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
        	this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label2.ForeColor = System.Drawing.Color.White;
        	this.label2.Location = new System.Drawing.Point(116, 12);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(76, 23);
        	this.label2.TabIndex = 5;
        	this.label2.Text = "Periodo";
        	this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// comboBox1
        	// 
        	this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
        	this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.comboBox1.FormattingEnabled = true;
        	this.comboBox1.Location = new System.Drawing.Point(62, 38);
        	this.comboBox1.Name = "comboBox1";
        	this.comboBox1.Size = new System.Drawing.Size(185, 25);
        	this.comboBox1.TabIndex = 4;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.maskedTextBox2);
        	this.groupBox1.Controls.Add(this.label3);
        	this.groupBox1.Controls.Add(this.maskedTextBox1);
        	this.groupBox1.Controls.Add(this.radioButton3);
        	this.groupBox1.Controls.Add(this.radioButton2);
        	this.groupBox1.Controls.Add(this.radioButton1);
        	this.groupBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.groupBox1.ForeColor = System.Drawing.Color.White;
        	this.groupBox1.Location = new System.Drawing.Point(62, 69);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(185, 263);
        	this.groupBox1.TabIndex = 6;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Factura ";
        	// 
        	// maskedTextBox2
        	// 
        	this.maskedTextBox2.Enabled = false;
        	this.maskedTextBox2.Location = new System.Drawing.Point(39, 217);
        	this.maskedTextBox2.Mask = "00/00/0000";
        	this.maskedTextBox2.Name = "maskedTextBox2";
        	this.maskedTextBox2.Size = new System.Drawing.Size(107, 23);
        	this.maskedTextBox2.TabIndex = 5;
        	this.maskedTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        	this.maskedTextBox2.ValidatingType = typeof(System.DateTime);
        	// 
        	// label3
        	// 
        	this.label3.Location = new System.Drawing.Point(63, 191);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(56, 23);
        	this.label3.TabIndex = 4;
        	this.label3.Text = "Hasta:";
        	this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        	// 
        	// maskedTextBox1
        	// 
        	this.maskedTextBox1.Enabled = false;
        	this.maskedTextBox1.Location = new System.Drawing.Point(39, 165);
        	this.maskedTextBox1.Mask = "00/00/0000";
        	this.maskedTextBox1.Name = "maskedTextBox1";
        	this.maskedTextBox1.Size = new System.Drawing.Size(107, 23);
        	this.maskedTextBox1.TabIndex = 3;
        	this.maskedTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        	this.maskedTextBox1.ValidatingType = typeof(System.DateTime);
        	// 
        	// radioButton3
        	// 
        	this.radioButton3.Location = new System.Drawing.Point(30, 121);
        	this.radioButton3.Name = "radioButton3";
        	this.radioButton3.Size = new System.Drawing.Size(129, 41);
        	this.radioButton3.TabIndex = 2;
        	this.radioButton3.TabStop = true;
        	this.radioButton3.Text = "Faltantes de\r\nNotificar desde:";
        	this.radioButton3.UseVisualStyleBackColor = true;
        	this.radioButton3.CheckedChanged += new System.EventHandler(this.RadioButton3CheckedChanged);
        	// 
        	// radioButton2
        	// 
        	this.radioButton2.Location = new System.Drawing.Point(30, 62);
        	this.radioButton2.Name = "radioButton2";
        	this.radioButton2.Size = new System.Drawing.Size(129, 43);
        	this.radioButton2.TabIndex = 1;
        	this.radioButton2.TabStop = true;
        	this.radioButton2.Text = "Entregado a \r\nControlador";
        	this.radioButton2.UseVisualStyleBackColor = true;
        	// 
        	// radioButton1
        	// 
        	this.radioButton1.Checked = true;
        	this.radioButton1.Location = new System.Drawing.Point(30, 23);
        	this.radioButton1.Name = "radioButton1";
        	this.radioButton1.Size = new System.Drawing.Size(129, 19);
        	this.radioButton1.TabIndex = 0;
        	this.radioButton1.TabStop = true;
        	this.radioButton1.Text = "CCJAL";
        	this.radioButton1.UseVisualStyleBackColor = true;
        	// 
        	// button4
        	// 
        	this.button4.BackColor = System.Drawing.Color.Transparent;
        	this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
        	this.button4.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
        	this.button4.FlatAppearance.BorderSize = 0;
        	this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
        	this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumBlue;
        	this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.button4.ForeColor = System.Drawing.Color.White;
        	this.button4.Image = global::Nova_Gear.Properties.Resources.printer_1;
        	this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        	this.button4.Location = new System.Drawing.Point(82, 345);
        	this.button4.Name = "button4";
        	this.button4.Size = new System.Drawing.Size(148, 53);
        	this.button4.TabIndex = 76;
        	this.button4.Text = "    Generar\r\n    Factura";
        	this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
        	this.button4.UseVisualStyleBackColor = false;
        	this.button4.Click += new System.EventHandler(this.button4_Click);
        	// 
        	// panel1
        	// 
        	this.panel1.Controls.Add(this.label1);
        	this.panel1.Location = new System.Drawing.Point(42, 338);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(235, 67);
        	this.panel1.TabIndex = 77;
        	this.panel1.Visible = false;
        	// 
        	// label1
        	// 
        	this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
        	this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label1.ForeColor = System.Drawing.Color.White;
        	this.label1.Location = new System.Drawing.Point(9, 8);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(216, 53);
        	this.label1.TabIndex = 6;
        	this.label1.Text = "Procesando...";
        	this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        	// 
        	// Oficios_imprimir
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.Black;
        	this.ClientSize = new System.Drawing.Size(309, 417);
        	this.Controls.Add(this.panel1);
        	this.Controls.Add(this.button4);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.comboBox1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        	this.Icon = global::Nova_Gear.Properties.Resources.logo_nova_white_2;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "Oficios_imprimir";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Nova Gear: Oficios - Impresion de Facturas";
        	this.Load += new System.EventHandler(this.Oficios_imprimir_Load);
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	this.panel1.ResumeLayout(false);
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox maskedTextBox2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}
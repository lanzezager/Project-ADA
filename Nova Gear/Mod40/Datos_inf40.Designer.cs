namespace Nova_Gear.Mod40
{
    partial class Datos_inf40
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
        	this.components = new System.ComponentModel.Container();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Datos_inf40));
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.label3 = new System.Windows.Forms.Label();
        	this.label4 = new System.Windows.Forms.Label();
        	this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
        	this.comboBox1 = new System.Windows.Forms.ComboBox();
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.button4 = new System.Windows.Forms.Button();
        	this.timer1 = new System.Windows.Forms.Timer(this.components);
        	this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        	this.SuspendLayout();
        	// 
        	// label1
        	// 
        	this.label1.BackColor = System.Drawing.Color.Transparent;
        	this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label1.ForeColor = System.Drawing.Color.White;
        	this.label1.Location = new System.Drawing.Point(80, 9);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(192, 35);
        	this.label1.TabIndex = 0;
        	this.label1.Text = "Datos específicos \r\npara el informe Oficial";
        	this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.BackColor = System.Drawing.Color.Transparent;
        	this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label2.ForeColor = System.Drawing.Color.White;
        	this.label2.Location = new System.Drawing.Point(114, 55);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(124, 15);
        	this.label2.TabIndex = 1;
        	this.label2.Text = "Fecha del Escrito:";
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.BackColor = System.Drawing.Color.Transparent;
        	this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label3.ForeColor = System.Drawing.Color.White;
        	this.label3.Location = new System.Drawing.Point(143, 105);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(66, 15);
        	this.label3.TabIndex = 2;
        	this.label3.Text = "Autoriza:";
        	// 
        	// label4
        	// 
        	this.label4.AutoSize = true;
        	this.label4.BackColor = System.Drawing.Color.Transparent;
        	this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label4.ForeColor = System.Drawing.Color.White;
        	this.label4.Location = new System.Drawing.Point(108, 153);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(136, 15);
        	this.label4.TabIndex = 3;
        	this.label4.Text = "Folio de Referencia:";
        	// 
        	// dateTimePicker1
        	// 
        	this.dateTimePicker1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
        	this.dateTimePicker1.Location = new System.Drawing.Point(99, 74);
        	this.dateTimePicker1.Name = "dateTimePicker1";
        	this.dateTimePicker1.Size = new System.Drawing.Size(154, 23);
        	this.dateTimePicker1.TabIndex = 4;
        	this.toolTip1.SetToolTip(this.dateTimePicker1, "Fecha en que el asegurado presentó el escrito\r\nsolicitando la certificacion de lo" +
        	        	"s pagos. ");
        	// 
        	// comboBox1
        	// 
        	this.comboBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.comboBox1.FormattingEnabled = true;
        	this.comboBox1.Location = new System.Drawing.Point(35, 123);
        	this.comboBox1.Name = "comboBox1";
        	this.comboBox1.Size = new System.Drawing.Size(282, 23);
        	this.comboBox1.TabIndex = 5;
        	this.toolTip1.SetToolTip(this.comboBox1, "Jefe que autorizó la certificación.");
        	// 
        	// textBox1
        	// 
        	this.textBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.textBox1.Location = new System.Drawing.Point(35, 172);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.Size = new System.Drawing.Size(282, 23);
        	this.textBox1.TabIndex = 6;
        	this.toolTip1.SetToolTip(this.textBox1, "Folio asignado al Informe de Certtficación");
        	// 
        	// button4
        	// 
        	this.button4.BackColor = System.Drawing.Color.Transparent;
        	this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
        	this.button4.Enabled = false;
        	this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Navy;
        	this.button4.FlatAppearance.BorderSize = 0;
        	this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
        	this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
        	this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.button4.ForeColor = System.Drawing.Color.White;
        	this.button4.Image = global::Nova_Gear.Properties.Resources.resultset_next_1;
        	this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.button4.Location = new System.Drawing.Point(119, 214);
        	this.button4.Name = "button4";
        	this.button4.Size = new System.Drawing.Size(114, 40);
        	this.button4.TabIndex = 10;
        	this.button4.Text = "Continuar";
        	this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        	this.button4.UseVisualStyleBackColor = false;
        	this.button4.Click += new System.EventHandler(this.button4_Click);
        	// 
        	// timer1
        	// 
        	this.timer1.Enabled = true;
        	this.timer1.Interval = 500;
        	this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        	// 
        	// Datos_inf40
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.Black;
        	this.ClientSize = new System.Drawing.Size(352, 273);
        	this.ControlBox = false;
        	this.Controls.Add(this.button4);
        	this.Controls.Add(this.textBox1);
        	this.Controls.Add(this.comboBox1);
        	this.Controls.Add(this.dateTimePicker1);
        	this.Controls.Add(this.label4);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MaximizeBox = false;
        	this.Name = "Datos_inf40";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Nova Gear: Datos Informe ";
        	this.Load += new System.EventHandler(this.Datos_inf40_Load);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
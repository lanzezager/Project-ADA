namespace Nova_Gear.Depuracion
{
    partial class Sel_archivo
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
        	this.radioButton1 = new System.Windows.Forms.RadioButton();
        	this.radioButton2 = new System.Windows.Forms.RadioButton();
        	this.button1 = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// radioButton1
        	// 
        	this.radioButton1.AutoSize = true;
        	this.radioButton1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.radioButton1.Location = new System.Drawing.Point(38, 13);
        	this.radioButton1.Name = "radioButton1";
        	this.radioButton1.Size = new System.Drawing.Size(158, 21);
        	this.radioButton1.TabIndex = 0;
        	this.radioButton1.TabStop = true;
        	this.radioButton1.Text = "Procesar y Sipare";
        	this.radioButton1.UseVisualStyleBackColor = true;
        	// 
        	// radioButton2
        	// 
        	this.radioButton2.AutoSize = true;
        	this.radioButton2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.radioButton2.Location = new System.Drawing.Point(38, 40);
        	this.radioButton2.Name = "radioButton2";
        	this.radioButton2.Size = new System.Drawing.Size(156, 21);
        	this.radioButton2.TabIndex = 1;
        	this.radioButton2.TabStop = true;
        	this.radioButton2.Text = "General de Pagos";
        	this.radioButton2.UseVisualStyleBackColor = true;
        	// 
        	// button1
        	// 
        	this.button1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.button1.Location = new System.Drawing.Point(68, 76);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(98, 33);
        	this.button1.TabIndex = 2;
        	this.button1.Text = "Aceptar";
        	this.button1.UseVisualStyleBackColor = true;
        	this.button1.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// Sel_archivo
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.SystemColors.Control;
        	this.ClientSize = new System.Drawing.Size(241, 121);
        	this.Controls.Add(this.button1);
        	this.Controls.Add(this.radioButton2);
        	this.Controls.Add(this.radioButton1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Icon = global::Nova_Gear.Properties.Resources.logo_nova_white_2;
        	this.Name = "Sel_archivo";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Archivo(s) a Cargar:";
        	this.Load += new System.EventHandler(this.Sel_archivo_Load);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
    }
}
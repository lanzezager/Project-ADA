﻿namespace Nova_Gear
{
    partial class Setup_mysql
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup_mysql));
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.label3 = new System.Windows.Forms.Label();
        	this.textBox2 = new System.Windows.Forms.TextBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.textBox3 = new System.Windows.Forms.TextBox();
        	this.label5 = new System.Windows.Forms.Label();
        	this.textBox4 = new System.Windows.Forms.TextBox();
        	this.button1 = new System.Windows.Forms.Button();
        	this.button2 = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// textBox1
        	// 
        	this.textBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.textBox1.Location = new System.Drawing.Point(146, 68);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.Size = new System.Drawing.Size(202, 26);
        	this.textBox1.TabIndex = 0;
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.BackColor = System.Drawing.Color.Transparent;
        	this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label1.ForeColor = System.Drawing.Color.White;
        	this.label1.Location = new System.Drawing.Point(77, 15);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(226, 34);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Configuración de la Conexión \r\na la Base de Datos";
        	this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        	this.label1.Click += new System.EventHandler(this.Label1Click);
        	// 
        	// label2
        	// 
        	this.label2.BackColor = System.Drawing.Color.Transparent;
        	this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label2.ForeColor = System.Drawing.Color.White;
        	this.label2.Location = new System.Drawing.Point(12, 68);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(74, 22);
        	this.label2.TabIndex = 2;
        	this.label2.Text = "Usuario:";
        	this.label2.Click += new System.EventHandler(this.label2_Click);
        	// 
        	// label3
        	// 
        	this.label3.BackColor = System.Drawing.Color.Transparent;
        	this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label3.ForeColor = System.Drawing.Color.White;
        	this.label3.Location = new System.Drawing.Point(12, 111);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(99, 22);
        	this.label3.TabIndex = 4;
        	this.label3.Text = "Contraseña:";
        	// 
        	// textBox2
        	// 
        	this.textBox2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.textBox2.Location = new System.Drawing.Point(146, 107);
        	this.textBox2.Name = "textBox2";
        	this.textBox2.PasswordChar = '•';
        	this.textBox2.Size = new System.Drawing.Size(202, 26);
        	this.textBox2.TabIndex = 3;
        	// 
        	// label4
        	// 
        	this.label4.BackColor = System.Drawing.Color.Transparent;
        	this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label4.ForeColor = System.Drawing.Color.White;
        	this.label4.Location = new System.Drawing.Point(12, 152);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(78, 22);
        	this.label4.TabIndex = 6;
        	this.label4.Text = "Servidor:";
        	// 
        	// textBox3
        	// 
        	this.textBox3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.textBox3.Location = new System.Drawing.Point(146, 148);
        	this.textBox3.Name = "textBox3";
        	this.textBox3.Size = new System.Drawing.Size(202, 26);
        	this.textBox3.TabIndex = 5;
        	// 
        	// label5
        	// 
        	this.label5.BackColor = System.Drawing.Color.Transparent;
        	this.label5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label5.ForeColor = System.Drawing.Color.White;
        	this.label5.Location = new System.Drawing.Point(12, 192);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(128, 22);
        	this.label5.TabIndex = 8;
        	this.label5.Text = "Base de Datos:";
        	// 
        	// textBox4
        	// 
        	this.textBox4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.textBox4.Location = new System.Drawing.Point(146, 188);
        	this.textBox4.Name = "textBox4";
        	this.textBox4.Size = new System.Drawing.Size(202, 26);
        	this.textBox4.TabIndex = 7;
        	// 
        	// button1
        	// 
        	this.button1.BackColor = System.Drawing.Color.Transparent;
        	this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
        	this.button1.FlatAppearance.BorderSize = 0;
        	this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
        	this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(15)))), ((int)(((byte)(115)))));
        	this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.button1.ForeColor = System.Drawing.Color.White;
        	this.button1.Image = global::Nova_Gear.Properties.Resources.network_wireless;
        	this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.button1.Location = new System.Drawing.Point(15, 233);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(125, 44);
        	this.button1.TabIndex = 29;
        	this.button1.Text = "Probar\r\nConexión";
        	this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	this.button1.UseVisualStyleBackColor = false;
        	this.button1.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// button2
        	// 
        	this.button2.BackColor = System.Drawing.Color.Transparent;
        	this.button2.Enabled = false;
        	this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
        	this.button2.FlatAppearance.BorderSize = 0;
        	this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
        	this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(15)))), ((int)(((byte)(115)))));
        	this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.button2.ForeColor = System.Drawing.Color.White;
        	this.button2.Image = global::Nova_Gear.Properties.Resources.database_connect;
        	this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.button2.Location = new System.Drawing.Point(223, 233);
        	this.button2.Name = "button2";
        	this.button2.Size = new System.Drawing.Size(125, 44);
        	this.button2.TabIndex = 30;
        	this.button2.Text = "Guardar\r\n Ajustes";
        	this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	this.button2.UseVisualStyleBackColor = false;
        	this.button2.Click += new System.EventHandler(this.button2_Click);
        	// 
        	// Setup_mysql
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.Black;
        	this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
        	this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.ClientSize = new System.Drawing.Size(365, 289);
        	this.Controls.Add(this.button2);
        	this.Controls.Add(this.button1);
        	this.Controls.Add(this.label5);
        	this.Controls.Add(this.textBox4);
        	this.Controls.Add(this.label4);
        	this.Controls.Add(this.textBox3);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.textBox2);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.textBox1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "Setup_mysql";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Ajuste de Conexión con la Base de Datos";
        	this.Load += new System.EventHandler(this.Setup_mysql_Load);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
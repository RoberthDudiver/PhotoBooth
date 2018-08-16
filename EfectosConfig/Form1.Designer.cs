namespace EfectosConfig
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Pcontenedor = new System.Windows.Forms.Panel();
            this.imagenc = new System.Windows.Forms.Panel();
            this.Pmenu = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnl2 = new System.Windows.Forms.Button();
            this.btnl1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.Pcontenedor.SuspendLayout();
            this.Pmenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Pcontenedor
            // 
            this.Pcontenedor.AutoScroll = true;
            this.Pcontenedor.BackColor = System.Drawing.Color.Gray;
            this.Pcontenedor.Controls.Add(this.imagenc);
            this.Pcontenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pcontenedor.Location = new System.Drawing.Point(128, 0);
            this.Pcontenedor.Name = "Pcontenedor";
            this.Pcontenedor.Size = new System.Drawing.Size(930, 738);
            this.Pcontenedor.TabIndex = 0;
            this.Pcontenedor.Paint += new System.Windows.Forms.PaintEventHandler(this.Pcontenedor_Paint);
            // 
            // imagenc
            // 
            this.imagenc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imagenc.BackgroundImage")));
            this.imagenc.Location = new System.Drawing.Point(0, 3);
            this.imagenc.Name = "imagenc";
            this.imagenc.Size = new System.Drawing.Size(1, 1);
            this.imagenc.TabIndex = 0;
            // 
            // Pmenu
            // 
            this.Pmenu.Controls.Add(this.button1);
            this.Pmenu.Controls.Add(this.pictureBox1);
            this.Pmenu.Controls.Add(this.button4);
            this.Pmenu.Controls.Add(this.button5);
            this.Pmenu.Controls.Add(this.button2);
            this.Pmenu.Controls.Add(this.btnl2);
            this.Pmenu.Controls.Add(this.btnl1);
            this.Pmenu.Controls.Add(this.button3);
            this.Pmenu.Controls.Add(this.button7);
            this.Pmenu.Controls.Add(this.pictureBox2);
            this.Pmenu.Controls.Add(this.button6);
            this.Pmenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pmenu.Location = new System.Drawing.Point(0, 0);
            this.Pmenu.Name = "Pmenu";
            this.Pmenu.Size = new System.Drawing.Size(128, 738);
            this.Pmenu.TabIndex = 0;
            this.Pmenu.Paint += new System.Windows.Forms.PaintEventHandler(this.Pmenu_Paint);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 658);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 72);
            this.button1.TabIndex = 4;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EfectosConfig.Properties.Resources.Pareja_silhouette_icon;
            this.pictureBox1.Location = new System.Drawing.Point(0, 677);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.Location = new System.Drawing.Point(0, 586);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(128, 72);
            this.button4.TabIndex = 1;
            this.button4.Text = "Agregar Posicion De Fotos";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Top;
            this.button5.Location = new System.Drawing.Point(0, 514);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(128, 72);
            this.button5.TabIndex = 2;
            this.button5.Text = "Agregar Banner";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 442);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 72);
            this.button2.TabIndex = 0;
            this.button2.Text = "Agregar ImagenFondo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnl2
            // 
            this.btnl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnl2.Location = new System.Drawing.Point(0, 371);
            this.btnl2.Name = "btnl2";
            this.btnl2.Size = new System.Drawing.Size(128, 71);
            this.btnl2.TabIndex = 0;
            this.btnl2.Text = "Lienzo 63x63";
            this.btnl2.UseVisualStyleBackColor = true;
            this.btnl2.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnl1
            // 
            this.btnl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnl1.Location = new System.Drawing.Point(0, 300);
            this.btnl1.Name = "btnl1";
            this.btnl1.Size = new System.Drawing.Size(128, 71);
            this.btnl1.TabIndex = 0;
            this.btnl1.Text = "Lienzo 42x63";
            this.btnl1.UseVisualStyleBackColor = true;
            this.btnl1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.Location = new System.Drawing.Point(0, 229);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 71);
            this.button3.TabIndex = 5;
            this.button3.Text = "Lienzo 63X42";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_2);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox2.Location = new System.Drawing.Point(0, 71);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(128, 87);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // button6
            // 
            this.button6.Dock = System.Windows.Forms.DockStyle.Top;
            this.button6.Location = new System.Drawing.Point(0, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(128, 71);
            this.button6.TabIndex = 7;
            this.button6.Text = "Imagen Boton";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Dock = System.Windows.Forms.DockStyle.Top;
            this.button7.Location = new System.Drawing.Point(0, 158);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(128, 71);
            this.button7.TabIndex = 8;
            this.button7.Text = "Lienzo800X800";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 738);
            this.Controls.Add(this.Pcontenedor);
            this.Controls.Add(this.Pmenu);
            this.Name = "Form1";
            this.Text = "Configguracion de Cuadrucula";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Pcontenedor.ResumeLayout(false);
            this.Pmenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Pcontenedor;
        private System.Windows.Forms.Panel Pmenu;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnl1;
        private System.Windows.Forms.Button btnl2;
        private System.Windows.Forms.Panel imagenc;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}


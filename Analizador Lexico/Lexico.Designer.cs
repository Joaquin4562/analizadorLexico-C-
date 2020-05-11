namespace Analizador_Lexico
{
    partial class Lexico
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
            this.dgvTokens = new System.Windows.Forms.DataGridView();
            this.Token = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lexema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.espacio_de_texto = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.filePath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTokens)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTokens
            // 
            this.dgvTokens.AllowUserToAddRows = false;
            this.dgvTokens.AllowUserToDeleteRows = false;
            this.dgvTokens.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTokens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Token,
            this.Lexema,
            this.Index,
            this.Linea});
            this.dgvTokens.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvTokens.Location = new System.Drawing.Point(8, 229);
            this.dgvTokens.Name = "dgvTokens";
            this.dgvTokens.ReadOnly = true;
            this.dgvTokens.RowHeadersVisible = false;
            this.dgvTokens.Size = new System.Drawing.Size(629, 212);
            this.dgvTokens.TabIndex = 3;
            // 
            // Token
            // 
            this.Token.HeaderText = "Token";
            this.Token.Name = "Token";
            this.Token.ReadOnly = true;
            this.Token.Width = 250;
            // 
            // Lexema
            // 
            this.Lexema.HeaderText = "Lexema";
            this.Lexema.Name = "Lexema";
            this.Lexema.ReadOnly = true;
            this.Lexema.Width = 250;
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Width = 62;
            // 
            // Linea
            // 
            this.Linea.HeaderText = "Linea";
            this.Linea.Name = "Linea";
            this.Linea.ReadOnly = true;
            this.Linea.Width = 62;
            // 
            // espacio_de_texto
            // 
            this.espacio_de_texto.BackColor = System.Drawing.SystemColors.MenuText;
            this.espacio_de_texto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.espacio_de_texto.Font = new System.Drawing.Font("Consolas", 11.25F);
            this.espacio_de_texto.ForeColor = System.Drawing.Color.Lime;
            this.espacio_de_texto.Location = new System.Drawing.Point(6, 1);
            this.espacio_de_texto.Name = "espacio_de_texto";
            this.espacio_de_texto.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.espacio_de_texto.Size = new System.Drawing.Size(631, 222);
            this.espacio_de_texto.TabIndex = 4;
            this.espacio_de_texto.Text = "";
            this.espacio_de_texto.TextChanged += new System.EventHandler(this.espacio_de_texto_TextChanged_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(550, 445);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "abrir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // filePath
            // 
            this.filePath.Location = new System.Drawing.Point(6, 447);
            this.filePath.Name = "filePath";
            this.filePath.Size = new System.Drawing.Size(538, 20);
            this.filePath.TabIndex = 6;
            // 
            // Lexico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(649, 475);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.espacio_de_texto);
            this.Controls.Add(this.dgvTokens);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Lexico";
            this.Text = "Lexico";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTokens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTokens;
        private System.Windows.Forms.DataGridViewTextBoxColumn Token;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lexema;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linea;
        private System.Windows.Forms.RichTextBox espacio_de_texto;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox filePath;
    }
}
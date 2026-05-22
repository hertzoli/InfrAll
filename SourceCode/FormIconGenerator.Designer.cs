
namespace GerenciadorSistemas
{
    partial class FormIconGenerator
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonGerarIcone = new System.Windows.Forms.Button();
            this.textBoxCaminhoImagem = new System.Windows.Forms.TextBox();
            this.pictureBoxImagem = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonProcurar = new System.Windows.Forms.Button();
            this.textBoxNomeIconeSaida = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagem)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGerarIcone
            // 
            this.buttonGerarIcone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGerarIcone.Location = new System.Drawing.Point(281, 306);
            this.buttonGerarIcone.Name = "buttonGerarIcone";
            this.buttonGerarIcone.Size = new System.Drawing.Size(75, 23);
            this.buttonGerarIcone.TabIndex = 0;
            this.buttonGerarIcone.Text = "Gerar";
            this.buttonGerarIcone.UseVisualStyleBackColor = true;
            this.buttonGerarIcone.Click += new System.EventHandler(this.buttonGerarIcone_Click);
            // 
            // textBoxCaminhoImagem
            // 
            this.textBoxCaminhoImagem.AllowDrop = true;
            this.textBoxCaminhoImagem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCaminhoImagem.Location = new System.Drawing.Point(23, 49);
            this.textBoxCaminhoImagem.Name = "textBoxCaminhoImagem";
            this.textBoxCaminhoImagem.Size = new System.Drawing.Size(264, 20);
            this.textBoxCaminhoImagem.TabIndex = 1;
            this.textBoxCaminhoImagem.TextChanged += new System.EventHandler(this.textBoxCaminhoImagem_TextChanged);
            this.textBoxCaminhoImagem.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxCaminhoImagem_DragDrop);
            this.textBoxCaminhoImagem.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxCaminhoImagem_DragEnter);
            // 
            // pictureBoxImagem
            // 
            this.pictureBoxImagem.AllowDrop = true;
            this.pictureBoxImagem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImagem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxImagem.Location = new System.Drawing.Point(23, 89);
            this.pictureBoxImagem.Name = "pictureBoxImagem";
            this.pictureBoxImagem.Size = new System.Drawing.Size(333, 199);
            this.pictureBoxImagem.TabIndex = 2;
            this.pictureBoxImagem.TabStop = false;
            this.pictureBoxImagem.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBoxImagem_DragDrop);
            this.pictureBoxImagem.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBoxImagem_DragEnter);
            this.pictureBoxImagem.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxImagem_Paint);
            this.pictureBoxImagem.Resize += new System.EventHandler(this.pictureBoxImagem_Resize);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Caminho Imagem (local / url):";
            // 
            // buttonProcurar
            // 
            this.buttonProcurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProcurar.Location = new System.Drawing.Point(293, 47);
            this.buttonProcurar.Name = "buttonProcurar";
            this.buttonProcurar.Size = new System.Drawing.Size(63, 23);
            this.buttonProcurar.TabIndex = 4;
            this.buttonProcurar.Text = "Procurar";
            this.buttonProcurar.UseVisualStyleBackColor = true;
            this.buttonProcurar.Click += new System.EventHandler(this.buttonProcurar_Click);
            // 
            // textBoxNomeIconeSaida
            // 
            this.textBoxNomeIconeSaida.AllowDrop = true;
            this.textBoxNomeIconeSaida.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNomeIconeSaida.Location = new System.Drawing.Point(149, 309);
            this.textBoxNomeIconeSaida.Name = "textBoxNomeIconeSaida";
            this.textBoxNomeIconeSaida.Size = new System.Drawing.Size(107, 20);
            this.textBoxNomeIconeSaida.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Nome do icone Gerado:";
            // 
            // FormIconGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxNomeIconeSaida);
            this.Controls.Add(this.buttonProcurar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxImagem);
            this.Controls.Add(this.textBoxCaminhoImagem);
            this.Controls.Add(this.buttonGerarIcone);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "FormIconGenerator";
            this.Text = "Gerar icone";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGerarIcone;
        private System.Windows.Forms.TextBox textBoxCaminhoImagem;
        private System.Windows.Forms.PictureBox pictureBoxImagem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonProcurar;
        private System.Windows.Forms.TextBox textBoxNomeIconeSaida;
        private System.Windows.Forms.Label label2;
    }
}


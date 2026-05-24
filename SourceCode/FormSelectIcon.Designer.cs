
namespace GerenciadorSistemas
{
    partial class FormSelectIcon
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
            this.label3 = new System.Windows.Forms.Label();
            this.TreeViewImages = new System.Windows.Forms.TreeView();
            this.botaoOk = new System.Windows.Forms.Button();
            this.botaoCancelar = new System.Windows.Forms.Button();
            this.buttonDefinirIconePadrao = new System.Windows.Forms.Button();
            this.buttonAbrirPastaImagens = new System.Windows.Forms.Button();
            this.groupBoxGerarIcone = new System.Windows.Forms.GroupBox();
            this.pictureBoxPreviwIcon = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCaminhoImagem = new System.Windows.Forms.TextBox();
            this.buttonProcurar = new System.Windows.Forms.Button();
            this.pictureBoxImagem = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNomeIconeSaida = new System.Windows.Forms.TextBox();
            this.buttonGerarIcone = new System.Windows.Forms.Button();
            this.groupBoxGerarIcone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreviwIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagem)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Icone:";
            // 
            // TreeViewImages
            // 
            this.TreeViewImages.HideSelection = false;
            this.TreeViewImages.Location = new System.Drawing.Point(15, 31);
            this.TreeViewImages.Name = "TreeViewImages";
            this.TreeViewImages.Size = new System.Drawing.Size(376, 285);
            this.TreeViewImages.TabIndex = 20;
            // 
            // botaoOk
            // 
            this.botaoOk.Location = new System.Drawing.Point(699, 392);
            this.botaoOk.Name = "botaoOk";
            this.botaoOk.Size = new System.Drawing.Size(75, 23);
            this.botaoOk.TabIndex = 22;
            this.botaoOk.Text = "OK";
            this.botaoOk.UseVisualStyleBackColor = true;
            // 
            // botaoCancelar
            // 
            this.botaoCancelar.Location = new System.Drawing.Point(618, 392);
            this.botaoCancelar.Name = "botaoCancelar";
            this.botaoCancelar.Size = new System.Drawing.Size(75, 23);
            this.botaoCancelar.TabIndex = 23;
            this.botaoCancelar.Text = "Cancelar";
            this.botaoCancelar.UseVisualStyleBackColor = true;
            // 
            // buttonDefinirIconePadrao
            // 
            this.buttonDefinirIconePadrao.Location = new System.Drawing.Point(15, 322);
            this.buttonDefinirIconePadrao.Name = "buttonDefinirIconePadrao";
            this.buttonDefinirIconePadrao.Size = new System.Drawing.Size(121, 22);
            this.buttonDefinirIconePadrao.TabIndex = 24;
            this.buttonDefinirIconePadrao.Text = "Definir Icone Padrão";
            this.buttonDefinirIconePadrao.UseVisualStyleBackColor = true;
            this.buttonDefinirIconePadrao.Click += new System.EventHandler(this.buttonDefinirIconePadrao_Click);
            // 
            // buttonAbrirPastaImagens
            // 
            this.buttonAbrirPastaImagens.Location = new System.Drawing.Point(257, 322);
            this.buttonAbrirPastaImagens.Name = "buttonAbrirPastaImagens";
            this.buttonAbrirPastaImagens.Size = new System.Drawing.Size(134, 22);
            this.buttonAbrirPastaImagens.TabIndex = 25;
            this.buttonAbrirPastaImagens.Text = "Abrir a pasta dos icones";
            this.buttonAbrirPastaImagens.UseVisualStyleBackColor = true;
            this.buttonAbrirPastaImagens.Click += new System.EventHandler(this.buttonAbrirPastaImagens_Click);
            // 
            // groupBoxGerarIcone
            // 
            this.groupBoxGerarIcone.Controls.Add(this.pictureBoxPreviwIcon);
            this.groupBoxGerarIcone.Controls.Add(this.label4);
            this.groupBoxGerarIcone.Controls.Add(this.label1);
            this.groupBoxGerarIcone.Controls.Add(this.textBoxCaminhoImagem);
            this.groupBoxGerarIcone.Controls.Add(this.buttonProcurar);
            this.groupBoxGerarIcone.Controls.Add(this.pictureBoxImagem);
            this.groupBoxGerarIcone.Controls.Add(this.label2);
            this.groupBoxGerarIcone.Controls.Add(this.textBoxNomeIconeSaida);
            this.groupBoxGerarIcone.Controls.Add(this.buttonGerarIcone);
            this.groupBoxGerarIcone.Location = new System.Drawing.Point(409, 15);
            this.groupBoxGerarIcone.Name = "groupBoxGerarIcone";
            this.groupBoxGerarIcone.Size = new System.Drawing.Size(365, 353);
            this.groupBoxGerarIcone.TabIndex = 26;
            this.groupBoxGerarIcone.TabStop = false;
            this.groupBoxGerarIcone.Text = "Gerar icone";
            // 
            // pictureBoxPreviwIcon
            // 
            this.pictureBoxPreviwIcon.Location = new System.Drawing.Point(69, 321);
            this.pictureBoxPreviwIcon.Name = "pictureBoxPreviwIcon";
            this.pictureBoxPreviwIcon.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxPreviwIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxPreviwIcon.TabIndex = 21;
            this.pictureBoxPreviwIcon.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Preview:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Caminho Imagem (local / url):";
            // 
            // textBoxCaminhoImagem
            // 
            this.textBoxCaminhoImagem.AllowDrop = true;
            this.textBoxCaminhoImagem.Location = new System.Drawing.Point(16, 43);
            this.textBoxCaminhoImagem.Name = "textBoxCaminhoImagem";
            this.textBoxCaminhoImagem.Size = new System.Drawing.Size(268, 20);
            this.textBoxCaminhoImagem.TabIndex = 1;
            this.textBoxCaminhoImagem.TextChanged += new System.EventHandler(this.textBoxCaminhoImagem_TextChanged);
            this.textBoxCaminhoImagem.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxCaminhoImagem_DragDrop);
            this.textBoxCaminhoImagem.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxCaminhoImagem_DragEnter);
            // 
            // buttonProcurar
            // 
            this.buttonProcurar.Location = new System.Drawing.Point(290, 41);
            this.buttonProcurar.Name = "buttonProcurar";
            this.buttonProcurar.Size = new System.Drawing.Size(63, 23);
            this.buttonProcurar.TabIndex = 2;
            this.buttonProcurar.Text = "Procurar";
            this.buttonProcurar.UseVisualStyleBackColor = true;
            this.buttonProcurar.Click += new System.EventHandler(this.buttonProcurar_Click);
            // 
            // pictureBoxImagem
            // 
            this.pictureBoxImagem.AllowDrop = true;
            this.pictureBoxImagem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxImagem.Location = new System.Drawing.Point(16, 80);
            this.pictureBoxImagem.Name = "pictureBoxImagem";
            this.pictureBoxImagem.Size = new System.Drawing.Size(337, 199);
            this.pictureBoxImagem.TabIndex = 3;
            this.pictureBoxImagem.TabStop = false;
            this.pictureBoxImagem.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBoxImagem_DragDrop);
            this.pictureBoxImagem.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBoxImagem_DragEnter);
            this.pictureBoxImagem.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxImagem_Paint);
            this.pictureBoxImagem.Resize += new System.EventHandler(this.pictureBoxImagem_Resize);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 294);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nome do icone Gerado:";
            // 
            // textBoxNomeIconeSaida
            // 
            this.textBoxNomeIconeSaida.Location = new System.Drawing.Point(142, 291);
            this.textBoxNomeIconeSaida.Name = "textBoxNomeIconeSaida";
            this.textBoxNomeIconeSaida.Size = new System.Drawing.Size(130, 20);
            this.textBoxNomeIconeSaida.TabIndex = 5;
            // 
            // buttonGerarIcone
            // 
            this.buttonGerarIcone.Location = new System.Drawing.Point(284, 324);
            this.buttonGerarIcone.Name = "buttonGerarIcone";
            this.buttonGerarIcone.Size = new System.Drawing.Size(75, 23);
            this.buttonGerarIcone.TabIndex = 6;
            this.buttonGerarIcone.Text = "Gerar";
            this.buttonGerarIcone.UseVisualStyleBackColor = true;
            this.buttonGerarIcone.Click += new System.EventHandler(this.buttonGerarIcone_Click);
            // 
            // FormSelectIcon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 427);
            this.Controls.Add(this.groupBoxGerarIcone);
            this.Controls.Add(this.buttonAbrirPastaImagens);
            this.Controls.Add(this.buttonDefinirIconePadrao);
            this.Controls.Add(this.botaoCancelar);
            this.Controls.Add(this.botaoOk);
            this.Controls.Add(this.TreeViewImages);
            this.Controls.Add(this.label3);
            this.Name = "FormSelectIcon";
            this.Text = "Selecionar icone";
            this.groupBoxGerarIcone.ResumeLayout(false);
            this.groupBoxGerarIcone.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreviwIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView TreeViewImages;
        private System.Windows.Forms.Button botaoOk;
        private System.Windows.Forms.Button botaoCancelar;
        private System.Windows.Forms.Button buttonDefinirIconePadrao;
        private System.Windows.Forms.Button buttonAbrirPastaImagens;
        private System.Windows.Forms.GroupBox groupBoxGerarIcone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCaminhoImagem;
        private System.Windows.Forms.Button buttonProcurar;
        private System.Windows.Forms.PictureBox pictureBoxImagem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNomeIconeSaida;
        private System.Windows.Forms.Button buttonGerarIcone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBoxPreviwIcon;
    }
}

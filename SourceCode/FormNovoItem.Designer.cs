
namespace GerenciadorSistemas
{
    partial class FormNovoItem
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
            this._textBoxObservacao = new System.Windows.Forms.TextBox();
            this._textBoxDescricao = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._textBoxNome = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TreeViewImages = new System.Windows.Forms.TreeView();
            this.buttonNovoIcone = new System.Windows.Forms.Button();
            this.botaoOk = new System.Windows.Forms.Button();
            this.botaoCancelar = new System.Windows.Forms.Button();
            this.buttonDefinirIconePadrao = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _textBoxObservacao
            // 
            this._textBoxObservacao.Location = new System.Drawing.Point(12, 444);
            this._textBoxObservacao.Multiline = true;
            this._textBoxObservacao.Name = "_textBoxObservacao";
            this._textBoxObservacao.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._textBoxObservacao.Size = new System.Drawing.Size(376, 78);
            this._textBoxObservacao.TabIndex = 17;
            // 
            // _textBoxDescricao
            // 
            this._textBoxDescricao.Location = new System.Drawing.Point(12, 339);
            this._textBoxDescricao.Multiline = true;
            this._textBoxDescricao.Name = "_textBoxDescricao";
            this._textBoxDescricao.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._textBoxDescricao.Size = new System.Drawing.Size(376, 69);
            this._textBoxDescricao.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 428);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Observação:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 323);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Descrição:";
            // 
            // _textBoxNome
            // 
            this._textBoxNome.Location = new System.Drawing.Point(59, 12);
            this._textBoxNome.Name = "_textBoxNome";
            this._textBoxNome.Size = new System.Drawing.Size(332, 20);
            this._textBoxNome.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Nome:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Icone:";
            // 
            // TreeViewImages
            // 
            this.TreeViewImages.HideSelection = false;
            this.TreeViewImages.Location = new System.Drawing.Point(15, 79);
            this.TreeViewImages.Name = "TreeViewImages";
            this.TreeViewImages.Size = new System.Drawing.Size(376, 199);
            this.TreeViewImages.TabIndex = 20;
            // 
            // buttonNovoIcone
            // 
            this.buttonNovoIcone.Location = new System.Drawing.Point(299, 281);
            this.buttonNovoIcone.Name = "buttonNovoIcone";
            this.buttonNovoIcone.Size = new System.Drawing.Size(92, 22);
            this.buttonNovoIcone.TabIndex = 21;
            this.buttonNovoIcone.Text = "Novo Icone";
            this.buttonNovoIcone.UseVisualStyleBackColor = true;
            this.buttonNovoIcone.Click += new System.EventHandler(this.buttonNovoIcone_Click);
            // 
            // botaoOk
            // 
            this.botaoOk.Location = new System.Drawing.Point(313, 540);
            this.botaoOk.Name = "botaoOk";
            this.botaoOk.Size = new System.Drawing.Size(75, 23);
            this.botaoOk.TabIndex = 22;
            this.botaoOk.Text = "OK";
            this.botaoOk.UseVisualStyleBackColor = true;
            // 
            // botaoCancelar
            // 
            this.botaoCancelar.Location = new System.Drawing.Point(232, 540);
            this.botaoCancelar.Name = "botaoCancelar";
            this.botaoCancelar.Size = new System.Drawing.Size(75, 23);
            this.botaoCancelar.TabIndex = 23;
            this.botaoCancelar.Text = "Cancelar";
            this.botaoCancelar.UseVisualStyleBackColor = true;
            // 
            // buttonDefinirIconePadrao
            // 
            this.buttonDefinirIconePadrao.Location = new System.Drawing.Point(15, 281);
            this.buttonDefinirIconePadrao.Name = "buttonDefinirIconePadrao";
            this.buttonDefinirIconePadrao.Size = new System.Drawing.Size(121, 22);
            this.buttonDefinirIconePadrao.TabIndex = 24;
            this.buttonDefinirIconePadrao.Text = "Definir Icone Padrão";
            this.buttonDefinirIconePadrao.UseVisualStyleBackColor = true;
            this.buttonDefinirIconePadrao.Click += new System.EventHandler(this.buttonDefinirIconePadrao_Click);
            // 
            // FormNovoItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 575);
            this.Controls.Add(this.buttonDefinirIconePadrao);
            this.Controls.Add(this.botaoCancelar);
            this.Controls.Add(this.botaoOk);
            this.Controls.Add(this.buttonNovoIcone);
            this.Controls.Add(this.TreeViewImages);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._textBoxObservacao);
            this.Controls.Add(this._textBoxDescricao);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._textBoxNome);
            this.Controls.Add(this.label1);
            this.Name = "FormNovoItem";
            this.Text = "Novo item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBoxObservacao;
        private System.Windows.Forms.TextBox _textBoxDescricao;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _textBoxNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView TreeViewImages;
        private System.Windows.Forms.Button buttonNovoIcone;
        private System.Windows.Forms.Button botaoOk;
        private System.Windows.Forms.Button botaoCancelar;
        private System.Windows.Forms.Button buttonDefinirIconePadrao;
    }
}

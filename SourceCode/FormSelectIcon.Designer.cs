
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
            this.buttonNovoIcone = new System.Windows.Forms.Button();
            this.botaoOk = new System.Windows.Forms.Button();
            this.botaoCancelar = new System.Windows.Forms.Button();
            this.buttonDefinirIconePadrao = new System.Windows.Forms.Button();
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
            this.TreeViewImages.Size = new System.Drawing.Size(376, 247);
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
            this.botaoOk.Location = new System.Drawing.Point(313, 323);
            this.botaoOk.Name = "botaoOk";
            this.botaoOk.Size = new System.Drawing.Size(75, 23);
            this.botaoOk.TabIndex = 22;
            this.botaoOk.Text = "OK";
            this.botaoOk.UseVisualStyleBackColor = true;
            // 
            // botaoCancelar
            // 
            this.botaoCancelar.Location = new System.Drawing.Point(232, 323);
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
            // FormSelectIcon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 358);
            this.Controls.Add(this.buttonDefinirIconePadrao);
            this.Controls.Add(this.botaoCancelar);
            this.Controls.Add(this.botaoOk);
            this.Controls.Add(this.buttonNovoIcone);
            this.Controls.Add(this.TreeViewImages);
            this.Controls.Add(this.label3);
            this.Name = "FormSelectIcon";
            this.Text = "Selecionar icone";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView TreeViewImages;
        private System.Windows.Forms.Button buttonNovoIcone;
        private System.Windows.Forms.Button botaoOk;
        private System.Windows.Forms.Button botaoCancelar;
        private System.Windows.Forms.Button buttonDefinirIconePadrao;
    }
}

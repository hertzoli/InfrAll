
namespace GerenciadorSistemas
{
    partial class FormPesquisar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPesquisar));
            this.DataGridViewItemPesquisar = new System.Windows.Forms.DataGridView();
            this.textBoxPesquisar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPesquisar = new System.Windows.Forms.Button();
            this.buttonFiltrar = new System.Windows.Forms.Button();
            this.labelBarraStatus = new System.Windows.Forms.Label();
            this.groupBoxEdicaoEmMassa = new System.Windows.Forms.GroupBox();
            this.buttonSalvarEmMassa = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxEditarDescricaoEmMassa = new System.Windows.Forms.TextBox();
            this.comboBoxEditarTipoEmMassa = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAlterarIconeEmMassa = new System.Windows.Forms.Button();
            this.pictureBoxImagemEdicaoEmMassa = new System.Windows.Forms.PictureBox();
            this.textBoxEditarNomeEmMassa = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RichTextBoxEditarValorEmMassa = new GerenciadorSistemas.RichTextBoxSemSnap();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItemPesquisar)).BeginInit();
            this.groupBoxEdicaoEmMassa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagemEdicaoEmMassa)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewItemPesquisar
            // 
            this.DataGridViewItemPesquisar.AllowUserToAddRows = false;
            this.DataGridViewItemPesquisar.AllowUserToDeleteRows = false;
            this.DataGridViewItemPesquisar.AllowUserToResizeRows = false;
            this.DataGridViewItemPesquisar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewItemPesquisar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewItemPesquisar.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataGridViewItemPesquisar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewItemPesquisar.Location = new System.Drawing.Point(3, 40);
            this.DataGridViewItemPesquisar.MultiSelect = false;
            this.DataGridViewItemPesquisar.Name = "DataGridViewItemPesquisar";
            this.DataGridViewItemPesquisar.ReadOnly = true;
            this.DataGridViewItemPesquisar.RowHeadersVisible = false;
            this.DataGridViewItemPesquisar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridViewItemPesquisar.Size = new System.Drawing.Size(1032, 317);
            this.DataGridViewItemPesquisar.TabIndex = 21;
            // 
            // textBoxPesquisar
            // 
            this.textBoxPesquisar.Location = new System.Drawing.Point(74, 12);
            this.textBoxPesquisar.Name = "textBoxPesquisar";
            this.textBoxPesquisar.Size = new System.Drawing.Size(484, 20);
            this.textBoxPesquisar.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Pesquisar:";
            // 
            // buttonPesquisar
            // 
            this.buttonPesquisar.Image = global::GerenciadorSistemas.Properties.Resources.Pesquisar;
            this.buttonPesquisar.Location = new System.Drawing.Point(564, 10);
            this.buttonPesquisar.Name = "buttonPesquisar";
            this.buttonPesquisar.Size = new System.Drawing.Size(24, 24);
            this.buttonPesquisar.TabIndex = 24;
            this.buttonPesquisar.UseVisualStyleBackColor = true;
            // 
            // buttonFiltrar
            // 
            this.buttonFiltrar.Image = global::GerenciadorSistemas.Properties.Resources.Filtro;
            this.buttonFiltrar.Location = new System.Drawing.Point(594, 10);
            this.buttonFiltrar.Name = "buttonFiltrar";
            this.buttonFiltrar.Size = new System.Drawing.Size(24, 24);
            this.buttonFiltrar.TabIndex = 23;
            this.buttonFiltrar.UseVisualStyleBackColor = true;
            // 
            // labelBarraStatus
            // 
            this.labelBarraStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelBarraStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelBarraStatus.Location = new System.Drawing.Point(0, 470);
            this.labelBarraStatus.Name = "labelBarraStatus";
            this.labelBarraStatus.Size = new System.Drawing.Size(1035, 44);
            this.labelBarraStatus.TabIndex = 26;
            // 
            // groupBoxEdicaoEmMassa
            // 
            this.groupBoxEdicaoEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxEdicaoEmMassa.Controls.Add(this.buttonSalvarEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.label4);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.textBoxEditarDescricaoEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.comboBoxEditarTipoEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.label7);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.label3);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.RichTextBoxEditarValorEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.buttonAlterarIconeEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.pictureBoxImagemEdicaoEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.textBoxEditarNomeEmMassa);
            this.groupBoxEdicaoEmMassa.Controls.Add(this.label2);
            this.groupBoxEdicaoEmMassa.Location = new System.Drawing.Point(8, 363);
            this.groupBoxEdicaoEmMassa.Name = "groupBoxEdicaoEmMassa";
            this.groupBoxEdicaoEmMassa.Size = new System.Drawing.Size(1015, 99);
            this.groupBoxEdicaoEmMassa.TabIndex = 27;
            this.groupBoxEdicaoEmMassa.TabStop = false;
            this.groupBoxEdicaoEmMassa.Text = "Editar Todos os itens selecionados";
            // 
            // buttonSalvarEmMassa
            // 
            this.buttonSalvarEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSalvarEmMassa.Image = global::GerenciadorSistemas.Properties.Resources.Save;
            this.buttonSalvarEmMassa.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSalvarEmMassa.Location = new System.Drawing.Point(921, 41);
            this.buttonSalvarEmMassa.Name = "buttonSalvarEmMassa";
            this.buttonSalvarEmMassa.Size = new System.Drawing.Size(79, 33);
            this.buttonSalvarEmMassa.TabIndex = 63;
            this.buttonSalvarEmMassa.Text = "Salvar";
            this.buttonSalvarEmMassa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSalvarEmMassa.UseVisualStyleBackColor = true;
            this.buttonSalvarEmMassa.Click += new System.EventHandler(this.buttonSalvarEmMassa_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(646, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Descrição:";
            // 
            // textBoxEditarDescricaoEmMassa
            // 
            this.textBoxEditarDescricaoEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxEditarDescricaoEmMassa.Location = new System.Drawing.Point(646, 31);
            this.textBoxEditarDescricaoEmMassa.Multiline = true;
            this.textBoxEditarDescricaoEmMassa.Name = "textBoxEditarDescricaoEmMassa";
            this.textBoxEditarDescricaoEmMassa.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxEditarDescricaoEmMassa.Size = new System.Drawing.Size(264, 62);
            this.textBoxEditarDescricaoEmMassa.TabIndex = 62;
            // 
            // comboBoxEditarTipoEmMassa
            // 
            this.comboBoxEditarTipoEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxEditarTipoEmMassa.FormattingEnabled = true;
            this.comboBoxEditarTipoEmMassa.Location = new System.Drawing.Point(49, 72);
            this.comboBoxEditarTipoEmMassa.Name = "comboBoxEditarTipoEmMassa";
            this.comboBoxEditarTipoEmMassa.Size = new System.Drawing.Size(279, 21);
            this.comboBoxEditarTipoEmMassa.TabIndex = 58;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 57;
            this.label7.Text = "Tipo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(344, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Valor:";
            // 
            // buttonAlterarIconeEmMassa
            // 
            this.buttonAlterarIconeEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAlterarIconeEmMassa.Location = new System.Drawing.Point(53, 19);
            this.buttonAlterarIconeEmMassa.Name = "buttonAlterarIconeEmMassa";
            this.buttonAlterarIconeEmMassa.Size = new System.Drawing.Size(103, 22);
            this.buttonAlterarIconeEmMassa.TabIndex = 54;
            this.buttonAlterarIconeEmMassa.Text = "Alterar Icone";
            this.buttonAlterarIconeEmMassa.UseVisualStyleBackColor = true;
            // 
            // pictureBoxImagemEdicaoEmMassa
            // 
            this.pictureBoxImagemEdicaoEmMassa.Location = new System.Drawing.Point(18, 21);
            this.pictureBoxImagemEdicaoEmMassa.Name = "pictureBoxImagemEdicaoEmMassa";
            this.pictureBoxImagemEdicaoEmMassa.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxImagemEdicaoEmMassa.TabIndex = 53;
            this.pictureBoxImagemEdicaoEmMassa.TabStop = false;
            // 
            // textBoxEditarNomeEmMassa
            // 
            this.textBoxEditarNomeEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxEditarNomeEmMassa.Location = new System.Drawing.Point(49, 48);
            this.textBoxEditarNomeEmMassa.Name = "textBoxEditarNomeEmMassa";
            this.textBoxEditarNomeEmMassa.Size = new System.Drawing.Size(279, 20);
            this.textBoxEditarNomeEmMassa.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Nome:";
            // 
            // RichTextBoxEditarValorEmMassa
            // 
            this.RichTextBoxEditarValorEmMassa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RichTextBoxEditarValorEmMassa.BackColor = System.Drawing.SystemColors.Window;
            this.RichTextBoxEditarValorEmMassa.Location = new System.Drawing.Point(344, 28);
            this.RichTextBoxEditarValorEmMassa.Name = "RichTextBoxEditarValorEmMassa";
            this.RichTextBoxEditarValorEmMassa.Size = new System.Drawing.Size(288, 65);
            this.RichTextBoxEditarValorEmMassa.TabIndex = 56;
            this.RichTextBoxEditarValorEmMassa.Text = "";
            // 
            // FormPesquisar
            // 
            this.AcceptButton = this.buttonPesquisar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 514);
            this.Controls.Add(this.groupBoxEdicaoEmMassa);
            this.Controls.Add(this.labelBarraStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPesquisar);
            this.Controls.Add(this.buttonFiltrar);
            this.Controls.Add(this.textBoxPesquisar);
            this.Controls.Add(this.DataGridViewItemPesquisar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1051, 350);
            this.Name = "FormPesquisar";
            this.Text = "FormPesquisar";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItemPesquisar)).EndInit();
            this.groupBoxEdicaoEmMassa.ResumeLayout(false);
            this.groupBoxEdicaoEmMassa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagemEdicaoEmMassa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridViewItemPesquisar;
        private System.Windows.Forms.TextBox textBoxPesquisar;
        private System.Windows.Forms.Button buttonFiltrar;
        private System.Windows.Forms.Button buttonPesquisar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelBarraStatus;
        private System.Windows.Forms.GroupBox groupBoxEdicaoEmMassa;
        private System.Windows.Forms.Button buttonAlterarIconeEmMassa;
        private System.Windows.Forms.PictureBox pictureBoxImagemEdicaoEmMassa;
        private System.Windows.Forms.TextBox textBoxEditarNomeEmMassa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private RichTextBoxSemSnap RichTextBoxEditarValorEmMassa;
        private System.Windows.Forms.ComboBox comboBoxEditarTipoEmMassa;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxEditarDescricaoEmMassa;
        private System.Windows.Forms.Button buttonSalvarEmMassa;
    }
}
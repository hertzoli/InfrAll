
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
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItemPesquisar)).BeginInit();
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
            this.DataGridViewItemPesquisar.Size = new System.Drawing.Size(624, 323);
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
            this.labelBarraStatus.Location = new System.Drawing.Point(0, 366);
            this.labelBarraStatus.Name = "labelBarraStatus";
            this.labelBarraStatus.Size = new System.Drawing.Size(627, 44);
            this.labelBarraStatus.TabIndex = 26;
            // 
            // FormPesquisar
            // 
            this.AcceptButton = this.buttonPesquisar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 410);
            this.Controls.Add(this.labelBarraStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPesquisar);
            this.Controls.Add(this.buttonFiltrar);
            this.Controls.Add(this.textBoxPesquisar);
            this.Controls.Add(this.DataGridViewItemPesquisar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPesquisar";
            this.Text = "FormPesquisar";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItemPesquisar)).EndInit();
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
    }
}
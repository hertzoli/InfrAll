
namespace GerenciadorSistemas
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeViewItens = new System.Windows.Forms.TreeView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.propertyGridItem = new System.Windows.Forms.PropertyGrid();
            this.panel5 = new System.Windows.Forms.Panel();
            this.buttonNovaSubPropriedade = new System.Windows.Forms.Button();
            this.buttonExcluirPropriedade = new System.Windows.Forms.Button();
            this.buttonNovaPropriedade = new System.Windows.Forms.Button();
            this.buttonDuplicar = new System.Windows.Forms.Button();
            this.buttonExcluirItem = new System.Windows.Forms.Button();
            this.buttonNovoSubItem = new System.Windows.Forms.Button();
            this.buttonNovoItem = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNome = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxValor = new System.Windows.Forms.TextBox();
            this.textBoxDescrição = new System.Windows.Forms.TextBox();
            this.textBoxCategoria = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLocal = new System.Windows.Forms.TextBox();
            this.buttonSalvar = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxReferenciaPropriedade = new System.Windows.Forms.TextBox();
            this.buttonCopyPlaceholder = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Salvar.png");
            this.imageList1.Images.SetKeyName(1, "SSH.png");
            this.imageList1.Images.SetKeyName(2, "SSH_16x16.png");
            this.imageList1.Images.SetKeyName(3, "Tool_16x16.png");
            this.imageList1.Images.SetKeyName(4, "tool_32.ico");
            this.imageList1.Images.SetKeyName(5, "tool_32_e_16.ico");
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 596);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 115);
            this.panel1.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Control;
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 590);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1184, 6);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(762, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(422, 590);
            this.panel3.TabIndex = 15;
            // 
            // splitter4
            // 
            this.splitter4.BackColor = System.Drawing.SystemColors.Control;
            this.splitter4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter4.Location = new System.Drawing.Point(756, 0);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(6, 590);
            this.splitter4.TabIndex = 16;
            this.splitter4.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.treeViewItens);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(339, 590);
            this.panel2.TabIndex = 17;
            // 
            // treeViewItens
            // 
            this.treeViewItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewItens.HideSelection = false;
            this.treeViewItens.ImageIndex = 0;
            this.treeViewItens.ImageList = this.imageList1;
            this.treeViewItens.Location = new System.Drawing.Point(0, 46);
            this.treeViewItens.Name = "treeViewItens";
            this.treeViewItens.SelectedImageIndex = 0;
            this.treeViewItens.Size = new System.Drawing.Size(339, 544);
            this.treeViewItens.TabIndex = 14;
            this.treeViewItens.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewItens_DrawNode);
            this.treeViewItens.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewItens_ItemDrag);
            this.treeViewItens.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewItens_AfterSelect);
            this.treeViewItens.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewItens_NodeMouseClick);
            this.treeViewItens.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewItens_DragDrop);
            this.treeViewItens.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewItens_DragEnter);
            this.treeViewItens.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewItens_DragOver);
            this.treeViewItens.DragLeave += new System.EventHandler(this.treeViewItens_DragLeave);
            this.treeViewItens.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewItens_KeyDown);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.buttonDuplicar);
            this.panel6.Controls.Add(this.buttonExcluirItem);
            this.panel6.Controls.Add(this.buttonNovoSubItem);
            this.panel6.Controls.Add(this.buttonNovoItem);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(339, 46);
            this.panel6.TabIndex = 1;
            // 
            // splitter3
            // 
            this.splitter3.BackColor = System.Drawing.SystemColors.Control;
            this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter3.Location = new System.Drawing.Point(339, 0);
            this.splitter3.MinExtra = 10;
            this.splitter3.MinSize = 5;
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(6, 590);
            this.splitter3.TabIndex = 18;
            this.splitter3.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.propertyGridItem);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(345, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(411, 590);
            this.panel4.TabIndex = 19;
            // 
            // propertyGridItem
            // 
            this.propertyGridItem.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.propertyGridItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGridItem.Location = new System.Drawing.Point(0, 46);
            this.propertyGridItem.Name = "propertyGridItem";
            this.propertyGridItem.Size = new System.Drawing.Size(411, 544);
            this.propertyGridItem.TabIndex = 19;
            this.propertyGridItem.UseWaitCursor = true;
            this.propertyGridItem.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGridItem_SelectedGridItemChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.buttonNovaSubPropriedade);
            this.panel5.Controls.Add(this.buttonExcluirPropriedade);
            this.panel5.Controls.Add(this.buttonNovaPropriedade);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(411, 46);
            this.panel5.TabIndex = 0;
            // 
            // buttonNovaSubPropriedade
            // 
            this.buttonNovaSubPropriedade.Image = global::GerenciadorSistemas.Properties.Resources.Nova_Sub_propriedade_32x32;
            this.buttonNovaSubPropriedade.Location = new System.Drawing.Point(42, 2);
            this.buttonNovaSubPropriedade.Name = "buttonNovaSubPropriedade";
            this.buttonNovaSubPropriedade.Size = new System.Drawing.Size(40, 40);
            this.buttonNovaSubPropriedade.TabIndex = 10;
            this.buttonNovaSubPropriedade.UseVisualStyleBackColor = true;
            this.buttonNovaSubPropriedade.Click += new System.EventHandler(this.buttonNovaSubPropriedade_Click);
            // 
            // buttonExcluirPropriedade
            // 
            this.buttonExcluirPropriedade.Image = global::GerenciadorSistemas.Properties.Resources.Excluir_propriedade_32x32;
            this.buttonExcluirPropriedade.Location = new System.Drawing.Point(82, 2);
            this.buttonExcluirPropriedade.Name = "buttonExcluirPropriedade";
            this.buttonExcluirPropriedade.Size = new System.Drawing.Size(40, 40);
            this.buttonExcluirPropriedade.TabIndex = 9;
            this.buttonExcluirPropriedade.UseVisualStyleBackColor = true;
            this.buttonExcluirPropriedade.Click += new System.EventHandler(this.buttonExcluirPropriedade_Click);
            // 
            // buttonNovaPropriedade
            // 
            this.buttonNovaPropriedade.Image = global::GerenciadorSistemas.Properties.Resources.Nova_propriedade_32x32;
            this.buttonNovaPropriedade.Location = new System.Drawing.Point(2, 2);
            this.buttonNovaPropriedade.Name = "buttonNovaPropriedade";
            this.buttonNovaPropriedade.Size = new System.Drawing.Size(40, 40);
            this.buttonNovaPropriedade.TabIndex = 7;
            this.buttonNovaPropriedade.UseVisualStyleBackColor = true;
            this.buttonNovaPropriedade.Click += new System.EventHandler(this.buttonNovaPropriedade_Click);
            // 
            // buttonDuplicar
            // 
            this.buttonDuplicar.Image = global::GerenciadorSistemas.Properties.Resources.Duplicar_Item_32x32;
            this.buttonDuplicar.Location = new System.Drawing.Point(82, 2);
            this.buttonDuplicar.Name = "buttonDuplicar";
            this.buttonDuplicar.Size = new System.Drawing.Size(40, 40);
            this.buttonDuplicar.TabIndex = 9;
            this.buttonDuplicar.UseVisualStyleBackColor = true;
            this.buttonDuplicar.Click += new System.EventHandler(this.buttonDuplicar_Click);
            // 
            // buttonExcluirItem
            // 
            this.buttonExcluirItem.Image = global::GerenciadorSistemas.Properties.Resources.Excluir_Item_32x32;
            this.buttonExcluirItem.Location = new System.Drawing.Point(122, 2);
            this.buttonExcluirItem.Name = "buttonExcluirItem";
            this.buttonExcluirItem.Size = new System.Drawing.Size(40, 40);
            this.buttonExcluirItem.TabIndex = 8;
            this.buttonExcluirItem.UseVisualStyleBackColor = true;
            this.buttonExcluirItem.Click += new System.EventHandler(this.buttonExcluirItem_Click);
            // 
            // buttonNovoSubItem
            // 
            this.buttonNovoSubItem.Image = global::GerenciadorSistemas.Properties.Resources.New_sub_item_32x32_2;
            this.buttonNovoSubItem.Location = new System.Drawing.Point(42, 2);
            this.buttonNovoSubItem.Name = "buttonNovoSubItem";
            this.buttonNovoSubItem.Size = new System.Drawing.Size(40, 40);
            this.buttonNovoSubItem.TabIndex = 7;
            this.buttonNovoSubItem.UseVisualStyleBackColor = true;
            this.buttonNovoSubItem.Click += new System.EventHandler(this.buttonNovoSubItem_Click);
            // 
            // buttonNovoItem
            // 
            this.buttonNovoItem.Image = ((System.Drawing.Image)(resources.GetObject("buttonNovoItem.Image")));
            this.buttonNovoItem.Location = new System.Drawing.Point(2, 2);
            this.buttonNovoItem.Name = "buttonNovoItem";
            this.buttonNovoItem.Size = new System.Drawing.Size(40, 40);
            this.buttonNovoItem.TabIndex = 6;
            this.buttonNovoItem.UseVisualStyleBackColor = true;
            this.buttonNovoItem.Click += new System.EventHandler(this.buttonNovoItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxNome);
            this.groupBox1.Controls.Add(this.buttonCopyPlaceholder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxReferenciaPropriedade);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonCopy);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.buttonRun);
            this.groupBox1.Controls.Add(this.textBoxValor);
            this.groupBox1.Controls.Add(this.buttonSalvar);
            this.groupBox1.Controls.Add(this.textBoxDescrição);
            this.groupBox1.Controls.Add(this.textBoxLocal);
            this.groupBox1.Controls.Add(this.textBoxCategoria);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 555);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Propriedade";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome:";
            // 
            // textBoxNome
            // 
            this.textBoxNome.Location = new System.Drawing.Point(23, 41);
            this.textBoxNome.Name = "textBoxNome";
            this.textBoxNome.Size = new System.Drawing.Size(286, 20);
            this.textBoxNome.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Valor:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Descrição:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 380);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Categoria:";
            // 
            // textBoxValor
            // 
            this.textBoxValor.Location = new System.Drawing.Point(23, 91);
            this.textBoxValor.Multiline = true;
            this.textBoxValor.Name = "textBoxValor";
            this.textBoxValor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValor.Size = new System.Drawing.Size(286, 81);
            this.textBoxValor.TabIndex = 5;
            // 
            // textBoxDescrição
            // 
            this.textBoxDescrição.Location = new System.Drawing.Point(23, 204);
            this.textBoxDescrição.Multiline = true;
            this.textBoxDescrição.Name = "textBoxDescrição";
            this.textBoxDescrição.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescrição.Size = new System.Drawing.Size(286, 108);
            this.textBoxDescrição.TabIndex = 6;
            // 
            // textBoxCategoria
            // 
            this.textBoxCategoria.Location = new System.Drawing.Point(23, 396);
            this.textBoxCategoria.Name = "textBoxCategoria";
            this.textBoxCategoria.Size = new System.Drawing.Size(174, 20);
            this.textBoxCategoria.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 341);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Local:";
            // 
            // textBoxLocal
            // 
            this.textBoxLocal.Location = new System.Drawing.Point(23, 357);
            this.textBoxLocal.Name = "textBoxLocal";
            this.textBoxLocal.Size = new System.Drawing.Size(174, 20);
            this.textBoxLocal.TabIndex = 9;
            // 
            // buttonSalvar
            // 
            this.buttonSalvar.Image = global::GerenciadorSistemas.Properties.Resources.Save;
            this.buttonSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSalvar.Location = new System.Drawing.Point(157, 501);
            this.buttonSalvar.Name = "buttonSalvar";
            this.buttonSalvar.Size = new System.Drawing.Size(70, 29);
            this.buttonSalvar.TabIndex = 12;
            this.buttonSalvar.Text = "Salvar";
            this.buttonSalvar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSalvar.UseVisualStyleBackColor = true;
            this.buttonSalvar.Click += new System.EventHandler(this.buttonSalvar_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(315, 91);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(47, 29);
            this.buttonRun.TabIndex = 13;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(315, 143);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(47, 29);
            this.buttonCopy.TabIndex = 14;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 433);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Referencia:";
            // 
            // textBoxReferenciaPropriedade
            // 
            this.textBoxReferenciaPropriedade.Location = new System.Drawing.Point(23, 449);
            this.textBoxReferenciaPropriedade.Name = "textBoxReferenciaPropriedade";
            this.textBoxReferenciaPropriedade.ReadOnly = true;
            this.textBoxReferenciaPropriedade.Size = new System.Drawing.Size(285, 20);
            this.textBoxReferenciaPropriedade.TabIndex = 16;
            // 
            // buttonCopyPlaceholder
            // 
            this.buttonCopyPlaceholder.Location = new System.Drawing.Point(314, 447);
            this.buttonCopyPlaceholder.Name = "buttonCopyPlaceholder";
            this.buttonCopyPlaceholder.Size = new System.Drawing.Size(48, 23);
            this.buttonCopyPlaceholder.TabIndex = 17;
            this.buttonCopyPlaceholder.Text = "Copy";
            this.buttonCopyPlaceholder.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 711);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1200, 750);
            this.Name = "Form1";
            this.Text = "InfrAll";
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView treeViewItens;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button buttonExcluirItem;
        private System.Windows.Forms.Button buttonNovoSubItem;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PropertyGrid propertyGridItem;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button buttonExcluirPropriedade;
        private System.Windows.Forms.Button buttonNovaPropriedade;
        private System.Windows.Forms.Button buttonNovaSubPropriedade;
        private System.Windows.Forms.Button buttonDuplicar;
        private System.Windows.Forms.Button buttonNovoItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxNome;
        private System.Windows.Forms.Button buttonCopyPlaceholder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxReferenciaPropriedade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TextBox textBoxValor;
        private System.Windows.Forms.Button buttonSalvar;
        private System.Windows.Forms.TextBox textBoxDescrição;
        private System.Windows.Forms.TextBox textBoxLocal;
        private System.Windows.Forms.TextBox textBoxCategoria;
        private System.Windows.Forms.Label label5;
    }
}


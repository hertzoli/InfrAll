
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
            this.buttonIssue = new System.Windows.Forms.Button();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeViewItens = new System.Windows.Forms.TreeView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.buttonEditar = new System.Windows.Forms.Button();
            this.buttonDuplicar = new System.Windows.Forms.Button();
            this.buttonExcluirItem = new System.Windows.Forms.Button();
            this.buttonNovoSubItem = new System.Windows.Forms.Button();
            this.buttonNovoItem = new System.Windows.Forms.Button();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.DataGridViewItem = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAlterarIcone = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDataEdicao = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDataCriacao = new System.Windows.Forms.TextBox();
            this.pictureBoxImagem = new System.Windows.Forms.PictureBox();
            this.comboBoxTipo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxNome = new System.Windows.Forms.TextBox();
            this.buttonCopyPlaceholder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxReferenciaPropriedade = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonSalvar = new System.Windows.Forms.Button();
            this.textBoxDescricao = new System.Windows.Forms.TextBox();
            this.textBoxLocal = new System.Windows.Forms.TextBox();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RichTextBoxValor = new GerenciadorSistemas.RichTextBoxSemSnap();
            this.labelInfo = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItem)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagem)).BeginInit();
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
            this.imageList1.Images.SetKeyName(6, "Vazio.png");
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
            this.panel3.Controls.Add(this.labelInfo);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.buttonIssue);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(858, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(326, 590);
            this.panel3.TabIndex = 15;
            // 
            // buttonIssue
            // 
            this.buttonIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonIssue.Location = new System.Drawing.Point(4, 2);
            this.buttonIssue.Name = "buttonIssue";
            this.buttonIssue.Size = new System.Drawing.Size(41, 22);
            this.buttonIssue.TabIndex = 19;
            this.buttonIssue.Text = "Issue";
            this.buttonIssue.UseVisualStyleBackColor = true;
            this.buttonIssue.Click += new System.EventHandler(this.buttonIssue_Click);
            // 
            // splitter4
            // 
            this.splitter4.BackColor = System.Drawing.SystemColors.Control;
            this.splitter4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter4.Location = new System.Drawing.Point(852, 0);
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
            this.treeViewItens.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewItens_NodeMouseDoubleClick);
            this.treeViewItens.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewItens_DragDrop);
            this.treeViewItens.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewItens_DragEnter);
            this.treeViewItens.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewItens_DragOver);
            this.treeViewItens.DragLeave += new System.EventHandler(this.treeViewItens_DragLeave);
            this.treeViewItens.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewItens_KeyDown);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.buttonEditar);
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
            // buttonEditar
            // 
            this.buttonEditar.Image = global::GerenciadorSistemas.Properties.Resources.Edit_item_32x32;
            this.buttonEditar.Location = new System.Drawing.Point(162, 2);
            this.buttonEditar.Name = "buttonEditar";
            this.buttonEditar.Size = new System.Drawing.Size(40, 40);
            this.buttonEditar.TabIndex = 10;
            this.buttonEditar.UseVisualStyleBackColor = true;
            this.buttonEditar.Click += new System.EventHandler(this.buttonEditar_Click);
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
            this.panel4.Controls.Add(this.DataGridViewItem);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(345, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(507, 590);
            this.panel4.TabIndex = 19;
            // 
            // DataGridViewItem
            // 
            this.DataGridViewItem.AllowUserToAddRows = false;
            this.DataGridViewItem.AllowUserToDeleteRows = false;
            this.DataGridViewItem.AllowUserToResizeRows = false;
            this.DataGridViewItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewItem.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataGridViewItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridViewItem.Location = new System.Drawing.Point(0, 0);
            this.DataGridViewItem.MultiSelect = false;
            this.DataGridViewItem.Name = "DataGridViewItem";
            this.DataGridViewItem.ReadOnly = true;
            this.DataGridViewItem.RowHeadersVisible = false;
            this.DataGridViewItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridViewItem.Size = new System.Drawing.Size(507, 590);
            this.DataGridViewItem.TabIndex = 20;
            this.DataGridViewItem.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewItem_CellDoubleClick);
            this.DataGridViewItem.SelectionChanged += new System.EventHandler(this.DataGridViewItem_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonAlterarIcone);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBoxDataEdicao);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxDataCriacao);
            this.groupBox1.Controls.Add(this.pictureBoxImagem);
            this.groupBox1.Controls.Add(this.comboBoxTipo);
            this.groupBox1.Controls.Add(this.label7);
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
            this.groupBox1.Controls.Add(this.RichTextBoxValor);
            this.groupBox1.Controls.Add(this.buttonSalvar);
            this.groupBox1.Controls.Add(this.textBoxDescricao);
            this.groupBox1.Controls.Add(this.textBoxLocal);
            this.groupBox1.Controls.Add(this.textBoxID);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(8, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 549);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Item";
            // 
            // buttonAlterarIcone
            // 
            this.buttonAlterarIcone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAlterarIcone.Location = new System.Drawing.Point(53, 16);
            this.buttonAlterarIcone.Name = "buttonAlterarIcone";
            this.buttonAlterarIcone.Size = new System.Drawing.Size(103, 22);
            this.buttonAlterarIcone.TabIndex = 50;
            this.buttonAlterarIcone.Text = "Alterar Icone";
            this.buttonAlterarIcone.UseVisualStyleBackColor = true;
            this.buttonAlterarIcone.Click += new System.EventHandler(this.buttonAlterarIcone_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 482);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Editado em:";
            // 
            // textBoxDataEdicao
            // 
            this.textBoxDataEdicao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDataEdicao.Location = new System.Drawing.Point(78, 479);
            this.textBoxDataEdicao.Name = "textBoxDataEdicao";
            this.textBoxDataEdicao.ReadOnly = true;
            this.textBoxDataEdicao.Size = new System.Drawing.Size(102, 20);
            this.textBoxDataEdicao.TabIndex = 48;
            this.textBoxDataEdicao.Text = "10/10/2025 18:32";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 457);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "Criado em:";
            // 
            // textBoxDataCriacao
            // 
            this.textBoxDataCriacao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDataCriacao.Location = new System.Drawing.Point(78, 453);
            this.textBoxDataCriacao.Name = "textBoxDataCriacao";
            this.textBoxDataCriacao.ReadOnly = true;
            this.textBoxDataCriacao.Size = new System.Drawing.Size(103, 20);
            this.textBoxDataCriacao.TabIndex = 46;
            this.textBoxDataCriacao.Text = "10/10/2025 18:32";
            // 
            // pictureBoxImagem
            // 
            this.pictureBoxImagem.Location = new System.Drawing.Point(23, 18);
            this.pictureBoxImagem.Name = "pictureBoxImagem";
            this.pictureBoxImagem.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxImagem.TabIndex = 45;
            this.pictureBoxImagem.TabStop = false;
            // 
            // comboBoxTipo
            // 
            this.comboBoxTipo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTipo.FormattingEnabled = true;
            this.comboBoxTipo.Location = new System.Drawing.Point(52, 213);
            this.comboBoxTipo.Name = "comboBoxTipo";
            this.comboBoxTipo.Size = new System.Drawing.Size(249, 21);
            this.comboBoxTipo.TabIndex = 44;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Tipo:";
            // 
            // textBoxNome
            // 
            this.textBoxNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNome.Location = new System.Drawing.Point(54, 45);
            this.textBoxNome.Name = "textBoxNome";
            this.textBoxNome.Size = new System.Drawing.Size(246, 20);
            this.textBoxNome.TabIndex = 28;
            // 
            // buttonCopyPlaceholder
            // 
            this.buttonCopyPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyPlaceholder.Location = new System.Drawing.Point(253, 425);
            this.buttonCopyPlaceholder.Name = "buttonCopyPlaceholder";
            this.buttonCopyPlaceholder.Size = new System.Drawing.Size(48, 22);
            this.buttonCopyPlaceholder.TabIndex = 42;
            this.buttonCopyPlaceholder.Text = "Copy";
            this.buttonCopyPlaceholder.UseVisualStyleBackColor = true;
            this.buttonCopyPlaceholder.Click += new System.EventHandler(this.buttonCopyPlaceholder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Nome:";
            // 
            // textBoxReferenciaPropriedade
            // 
            this.textBoxReferenciaPropriedade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReferenciaPropriedade.Location = new System.Drawing.Point(143, 426);
            this.textBoxReferenciaPropriedade.Name = "textBoxReferenciaPropriedade";
            this.textBoxReferenciaPropriedade.ReadOnly = true;
            this.textBoxReferenciaPropriedade.Size = new System.Drawing.Size(105, 20);
            this.textBoxReferenciaPropriedade.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Valor:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(79, 428);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Referencia:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Descrição:";
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Location = new System.Drawing.Point(256, 80);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(44, 21);
            this.buttonCopy.TabIndex = 39;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 428);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "ID:";
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(84, 80);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(44, 21);
            this.buttonRun.TabIndex = 38;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonSalvar
            // 
            this.buttonSalvar.Image = global::GerenciadorSistemas.Properties.Resources.Save;
            this.buttonSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSalvar.Location = new System.Drawing.Point(187, 495);
            this.buttonSalvar.Name = "buttonSalvar";
            this.buttonSalvar.Size = new System.Drawing.Size(113, 46);
            this.buttonSalvar.TabIndex = 37;
            this.buttonSalvar.Text = "Salvar";
            this.buttonSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSalvar.UseVisualStyleBackColor = true;
            this.buttonSalvar.Click += new System.EventHandler(this.buttonSalvar_Click);
            // 
            // textBoxDescricao
            // 
            this.textBoxDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescricao.Location = new System.Drawing.Point(15, 273);
            this.textBoxDescricao.Multiline = true;
            this.textBoxDescricao.Name = "textBoxDescricao";
            this.textBoxDescricao.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescricao.Size = new System.Drawing.Size(286, 108);
            this.textBoxDescricao.TabIndex = 33;
            // 
            // textBoxLocal
            // 
            this.textBoxLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLocal.Location = new System.Drawing.Point(84, 400);
            this.textBoxLocal.Name = "textBoxLocal";
            this.textBoxLocal.Size = new System.Drawing.Size(217, 20);
            this.textBoxLocal.TabIndex = 36;
            // 
            // textBoxID
            // 
            this.textBoxID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxID.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxID.Location = new System.Drawing.Point(37, 424);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.ReadOnly = true;
            this.textBoxID.Size = new System.Drawing.Size(39, 20);
            this.textBoxID.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 403);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Local:";
            // 
            // RichTextBoxValor
            // 
            this.RichTextBoxValor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxValor.Location = new System.Drawing.Point(15, 102);
            this.RichTextBoxValor.Name = "RichTextBoxValor";
            this.RichTextBoxValor.Size = new System.Drawing.Size(286, 105);
            this.RichTextBoxValor.TabIndex = 32;
            this.RichTextBoxValor.Text = "";
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(72, 14);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(0, 13);
            this.labelInfo.TabIndex = 30;
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
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItem)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImagem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button buttonExcluirItem;
        private System.Windows.Forms.Button buttonNovoSubItem;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView DataGridViewItem;
        private System.Windows.Forms.Button buttonDuplicar;
        private System.Windows.Forms.Button buttonNovoItem;
        private System.Windows.Forms.Button buttonEditar;
        private System.Windows.Forms.TreeView treeViewItens;
        private System.Windows.Forms.Button buttonIssue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonAlterarIcone;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDataEdicao;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxDataCriacao;
        private System.Windows.Forms.PictureBox pictureBoxImagem;
        private System.Windows.Forms.ComboBox comboBoxTipo;
        private System.Windows.Forms.Label label7;
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
        private RichTextBoxSemSnap RichTextBoxValor;
        private System.Windows.Forms.Button buttonSalvar;
        private System.Windows.Forms.TextBox textBoxDescricao;
        private System.Windows.Forms.TextBox textBoxLocal;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelInfo;
    }
}


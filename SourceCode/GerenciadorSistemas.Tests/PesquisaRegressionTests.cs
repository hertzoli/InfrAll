using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    // Executable regression checks, without external test packages.
    internal static class PesquisaRegressionTests
    {
        private static int _verificacoes;
        private static void Verificar(bool condicao, string descricao)
        {
            if (!condicao) throw new Exception(descricao);
            _verificacoes++;
        }
        private static FiltroPesquisa F(int coluna, string relacao, string valor, bool excluir = false)
        {
            return new FiltroPesquisa { Habilitado = true, Coluna = coluna, Relacao = relacao, Valor = valor, Excluir = excluir };
        }
        private static ResultadoPesquisaItem Item(string id, string nome, string local, DateTime data)
        {
            return new ResultadoPesquisaItem { ID = id, CriadoEm = data, EditadoEm = data,
                Campos = new[] { "server.png", nome, "srv valor", "Texto", "Descrição", local, id,
                    "~REF[" + id + "_" + nome + "]~", data.ToString("dd/MM/yyyy HH:mm:ss"), data.ToString("dd/MM/yyyy HH:mm:ss") } };
        }
        [STAThread]
        private static int Main(string[] args)
        {
            try
            {
                var itens = new List<ResultadoPesquisaItem> {
                    Item("a1", "srv web", "Produção", new DateTime(2025, 12, 31, 12, 0, 0)),
                    Item("b2", "srv api", "Teste", new DateTime(2026, 1, 1, 12, 0, 0)),
                    Item("c3", "srv web", "Produção", new DateTime(2026, 1, 2, 12, 0, 0)) };
                Verificar(PesquisaItensService.Pesquisar(itens, " SRV ").Count == 3, "Trim e caixa");
                Verificar(PesquisaItensService.Pesquisar(itens, "producao").Count == 0, "Acentos distintos");
                Verificar(PesquisaItensService.Pesquisar(itens, "a1_srv").Count == 1, "Referencia");
                Verificar(PesquisaItensService.Pesquisar(itens, "web srv").Count == 0, "Sequência literal");
                Verificar(PesquisaItensService.Pesquisar(itens.Concat(itens), "srv").Count == 3, "Um resultado por ID");
                foreach (string invalido in new[] { "", " ", " a " })
                {
                    bool rejeitado = false;
                    try { PesquisaItensService.Pesquisar(itens, invalido); } catch (ArgumentException) { rejeitado = true; }
                    Verificar(rejeitado, "Mínimo de dois caracteres");
                }
                for (int c = 0; c < 10; c++)
                {
                    var isolado = Item("x", "", "", DateTime.Now);
                    isolado.Campos = new string[10];
                    isolado.Campos[c] = "alvo";
                    Verificar(PesquisaItensService.Pesquisar(new[] { isolado }, "alvo").Count == 1, "Busca campo " + c);
                }
                var filtros = new[] { F(1, "contém", "web"), F(1, "contém", "api"), F(3, "é", "Texto"), F(5, "contém", "Teste", true) };
                Verificar(PesquisaItensService.Filtrar(itens, filtros).Count == 2, "OU, E e exclusão");
                Verificar(PesquisaItensService.Filtrar(itens, new[] { F(1, "contém", "web"), F(5, "é", "Teste") }).Count == 0, "E entre colunas");
                Verificar(PesquisaItensService.Filtrar(itens, new[] { F(5, "é", "Teste", true) }).Count == 2, "Somente exclusão");
                filtros[3].Habilitado = false;
                Verificar(PesquisaItensService.Filtrar(itens, filtros).Count == 3, "Desabilitar restaura");
                Verificar(PesquisaItensService.Filtrar(itens, new FiltroPesquisa[0]).Count == 3, "Sem filtros");
                Verificar(PesquisaItensService.Filtrar(itens, new[] { F(8, "maior que", "31/12/2025") }).Count == 2, "Cronologia entre anos");
                Verificar(PesquisaItensService.Filtrar(itens, new[] { F(8, "é", "01/01/2026") }).Count == 1, "Dia inteiro");
                Verificar(PesquisaItensService.Filtrar(itens, new[] { F(9, "menor que", "01/01/2026") }).Count == 1, "Data menor");
                Verificar(PesquisaItensService.Filtrar(itens, new[] { F(9, "é", "01/01/2026 12:00:00") }).Count == 1, "Hora exata");
                foreach (var filtro in new[] { F(8, "é", "31/02/2026"), F(1, "maior que", "2"), F(1, "contém", " ") })
                {
                    bool rejeitado = false;
                    try { PesquisaItensService.Validar(filtro); } catch (ArgumentException) { rejeitado = true; }
                    Verificar(rejeitado, "Filtro inválido");
                }
                PesquisaItensService.Validar(F(1, "é", ""));
                foreach (string relacao in PesquisaItensService.RelacoesTexto)
                {
                    var f = F(1, relacao, relacao.EndsWith("com") ? (relacao.StartsWith("inicia") ? "srv" : "web") : "srv web");
                    int esperado = relacao == "não é" || relacao == "não contém" ? 1 : relacao == "inicia com" ? 3 : 2;
                    Verificar(PesquisaItensService.Filtrar(itens, new[] { f }).Count == esperado, relacao);
                }
                var grande = Enumerable.Range(0, 10000).Select(n => Item(n.ToString(), "srv web", "Produção", new DateTime(2026, 1, 1))).ToList();
                var relogio = Stopwatch.StartNew();
                var resultado = PesquisaItensService.Filtrar(PesquisaItensService.Pesquisar(grande, "srv"), filtros);
                relogio.Stop();
                Verificar(resultado.Count == 10000, "Massa de 10000 itens");
                Console.WriteLine("10000 itens: pesquisa + filtros em {0} ms", relogio.ElapsedMilliseconds);
                if (args.Length > 0) VerificarFormularios(args[0]);
                Console.WriteLine("PASS: {0} verificações", _verificacoes);
                return 0;
            }
            catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
        }
        private static int _aplicacoes;
        private static object _ultimoAplicado;
        private static void ReceberFiltros(object filtros)
        {
            _aplicacoes++;
            _ultimoAplicado = filtros;
        }
        private static object Campo(object obj, string nome)
        {
            return obj.GetType().GetField(nome, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
        }
        private static List<T> Snapshot<T>() where T : new()
        {
            var item = new T();
            typeof(T).GetProperty("ID").SetValue(item, "id123");
            typeof(T).GetProperty("Campos").SetValue(item, new[] { "", "srv teste", "valor", "Texto", "", "", "id123", "", "", "" });
            return new List<T> { item };
        }
        private static void AguardarPesquisa(Form formulario)
        {
            var relogio = Stopwatch.StartNew();
            while (!((Button)Campo(formulario, "buttonPesquisar")).Enabled && relogio.ElapsedMilliseconds < 5000)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }
            Verificar(((Button)Campo(formulario, "buttonPesquisar")).Enabled, "Pesquisa assíncrona conclui");
        }
        private static void VerificarCicloPesquisa(Assembly assembly)
        {
            var tipoItem = assembly.GetType("GerenciadorSistemas.ResultadoPesquisaItem");
            var tipoLista = typeof(List<>).MakeGenericType(tipoItem);
            var obter = Delegate.CreateDelegate(typeof(Func<>).MakeGenericType(tipoLista),
                typeof(PesquisaRegressionTests).GetMethod("Snapshot", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(tipoItem));
            var construtor = assembly.GetType("GerenciadorSistemas.FormPesquisar")
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single();
            string idSelecionado = null;
            using (var principal = new Form())
            using (var formulario = (Form)construtor.Invoke(new object[] { obter,
                new Func<string, bool>(id => { idSelecionado = id; return true; }), null }))
            {
                principal.Show();
                formulario.Show(principal);
                Application.DoEvents();
                var texto = (TextBox)Campo(formulario, "textBoxPesquisar");
                var grid = (DataGridView)Campo(formulario, "DataGridViewItemPesquisar");
                texto.Text = "srv";
                Application.DoEvents();
                Verificar(grid.Rows.Count == 0, "Digitar não inicia pesquisa");
                ((Button)Campo(formulario, "buttonPesquisar")).PerformClick();
                AguardarPesquisa(formulario);
                Verificar(grid.Rows.Count == 1 && (string)grid.Rows[0].Tag == "id123", "Publica resultado com ID");
                DispararDuploClique(grid, -1, 1);
                Verificar(idSelecionado == null, "Cabeçalho não navega");
                DispararDuploClique(grid, 0, 1);
                Verificar(idSelecionado == "id123" && formulario.Visible && !formulario.IsDisposed,
                    "Duplo clique navega sem fechar pesquisa");
                Verificar(principal.Enabled && grid.Rows.Count == 1 && texto.Text == "srv",
                    "Janela principal acessível e resultado preservado");
                texto.Text = "outro texto não submetido";
                var tipoFiltro = assembly.GetType("GerenciadorSistemas.FiltroPesquisa");
                var lista = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(tipoFiltro));
                var filtro = Activator.CreateInstance(tipoFiltro);
                tipoFiltro.GetProperty("Habilitado").SetValue(filtro, true);
                tipoFiltro.GetProperty("Coluna").SetValue(filtro, 1);
                tipoFiltro.GetProperty("Relacao").SetValue(filtro, "contém");
                tipoFiltro.GetProperty("Valor").SetValue(filtro, "srv");
                tipoFiltro.GetProperty("Excluir").SetValue(filtro, true);
                lista.Add(filtro);
                var aplicar = formulario.GetType().GetMethod("AplicarFiltros", BindingFlags.Instance | BindingFlags.NonPublic);
                aplicar.Invoke(formulario, new object[] { lista });
                Verificar(grid.Rows.Count == 0, "Exclusão aplicada");
                lista.Clear();
                aplicar.Invoke(formulario, new object[] { lista });
                Verificar(grid.Rows.Count == 1, "Remover filtro restaura consulta original sem usar texto não submetido");
                ((Button)Campo(formulario, "buttonPesquisar")).PerformClick();
                AguardarPesquisa(formulario);
                Verificar(grid.Rows.Count == 0, "Nova consulta substitui conjunto original");
                formulario.Close();
            }
        }
        private static void DispararDuploClique(DataGridView grid, int linha, int coluna)
        {
            typeof(DataGridView).GetMethod("OnCellDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(grid, new object[] { new DataGridViewCellEventArgs(coluna, linha) });
        }
        private static void VerificarCicloFiltros(Assembly assembly)
        {
            Type tipoFiltro = assembly.GetType("GerenciadorSistemas.FiltroPesquisa");
            Type tipoLista = typeof(List<>).MakeGenericType(tipoFiltro);
            Type tipoAcao = typeof(Action<>).MakeGenericType(tipoLista);
            var callback = Delegate.CreateDelegate(tipoAcao, typeof(PesquisaRegressionTests)
                .GetMethod("ReceberFiltros", BindingFlags.Static | BindingFlags.NonPublic));
            var construtor = assembly.GetType("GerenciadorSistemas.FormFiltroPesquisa")
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
            _aplicacoes = 0;
            using (var formulario = (Form)construtor.Invoke(new[] { Activator.CreateInstance(tipoLista), callback }))
            {
                formulario.Show();
                Application.DoEvents();
                ((ComboBox)Campo(formulario, "comboBoxColuna")).SelectedIndex = 1;
                ((TextBox)Campo(formulario, "textBoxValor")).Text = "srv";
                ((Button)Campo(formulario, "buttonAdicionar")).PerformClick();
                var grid = (DataGridView)Campo(formulario, "DataGridViewFilter");
                Verificar(grid.Rows.Count == 1 && _aplicacoes == 0, "Adicionar não aplica");
                DispararDuploClique(grid, -1, 1);
                Verificar(grid.Rows.Count == 1, "Cabeçalho não remove filtro");
                ((ComboBox)Campo(formulario, "comboBoxColuna")).SelectedIndex = 8;
                ((TextBox)Campo(formulario, "textBoxValor")).Text = "01/01/2026";
                ((ComboBox)Campo(formulario, "comboBoxAcao")).SelectedIndex = 1;
                DispararDuploClique(grid, 0, 1);
                Verificar(grid.Rows.Count == 0 && _aplicacoes == 0, "Duplo clique retira somente da lista de trabalho");
                Verificar(((ComboBox)Campo(formulario, "comboBoxColuna")).SelectedIndex == 1
                    && ((ComboBox)Campo(formulario, "comboBoxRelacao")).SelectedItem.ToString() == "é"
                    && ((TextBox)Campo(formulario, "textBoxValor")).Text == "srv"
                    && ((ComboBox)Campo(formulario, "comboBoxAcao")).SelectedIndex == 0,
                    "Carrega coluna, relação, valor e ação do filtro");
                ((TextBox)Campo(formulario, "textBoxValor")).Text = "srv editado";
                ((Button)Campo(formulario, "buttonAdicionar")).PerformClick();
                Verificar(grid.Rows.Count == 1 && (string)grid.Rows[0].Cells[3].Value == "srv editado",
                    "Filtro editado pode ser adicionado novamente");
                ((Button)Campo(formulario, "buttonAplicar")).PerformClick();
                Verificar(_aplicacoes == 1 && formulario.Visible, "Aplicar mantém diálogo");
                grid.Rows[0].Cells[0].Value = false;
                ((Button)Campo(formulario, "buttonCancelar")).PerformClick();
                var anterior = (System.Collections.IList)_ultimoAplicado;
                Verificar(_aplicacoes == 1 && (bool)tipoFiltro.GetProperty("Habilitado").GetValue(anterior[0]),
                    "Cancelar não altera cópia aplicada");
            }
            using (var formulario = (Form)construtor.Invoke(new[] { _ultimoAplicado, callback }))
            {
                formulario.Show();
                Application.DoEvents();
                var grid = (DataGridView)Campo(formulario, "DataGridViewFilter");
                grid.Rows[0].Cells[0].Value = false;
                DispararDuploClique(grid, 0, 1);
                ((Button)Campo(formulario, "buttonAdicionar")).PerformClick();
                Verificar(!Convert.ToBoolean(grid.Rows[0].Cells[0].Value), "Editar preserva filtro desabilitado");
                ((Button)Campo(formulario, "buttonOK")).PerformClick();
                var atual = (System.Collections.IList)_ultimoAplicado;
                Verificar(_aplicacoes == 2 && !formulario.Visible && !(bool)tipoFiltro.GetProperty("Habilitado").GetValue(atual[0]),
                    "OK confirma checkbox e fecha");
            }
        }
        private static void VerificarFormularios(string caminho)
        {
            var assembly = Assembly.LoadFrom(caminho);
            using (var pesquisa = (Form)Activator.CreateInstance(assembly.GetType("GerenciadorSistemas.FormPesquisar")))
            {
                var grid = (DataGridView)Campo(pesquisa, "DataGridViewItemPesquisar");
                Verificar(grid.Columns.Count == 10 && grid.ReadOnly, "Grid pesquisa plano e somente leitura");
                var resultadoTipo = assembly.GetType("GerenciadorSistemas.ResultadoPesquisaItem");
                var listaVazia = Activator.CreateInstance(typeof(List<>).MakeGenericType(resultadoTipo));
                pesquisa.GetType().GetMethod("MostrarResultados", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(pesquisa, new[] { listaVazia });
                Verificar(grid.Rows.Count == 0, "Resultado vazio preserva cabeçalhos");
                Verificar(pesquisa.AcceptButton == Campo(pesquisa, "buttonPesquisar"), "Enter inicia pesquisa");
                Verificar(((Label)Campo(pesquisa, "labelBarraStatus")).Text == "", "Status reservado");
            }
            VerificarCicloPesquisa(assembly);
            VerificarCicloFiltros(assembly);
            using (var filtro = (Form)Activator.CreateInstance(assembly.GetType("GerenciadorSistemas.FormFiltroPesquisa")))
            {
                var grid = (DataGridView)Campo(filtro, "DataGridViewFilter");
                Verificar(grid.Columns.Count == 5 && !grid.Columns[0].ReadOnly && grid.Columns[1].ReadOnly, "Colunas de filtros");
                Verificar(filtro.CancelButton == Campo(filtro, "buttonCancelar"), "Esc cancela");
                var colunas = (ComboBox)Campo(filtro, "comboBoxColuna");
                colunas.SelectedIndex = 8;
                Verificar(((ComboBox)Campo(filtro, "comboBoxRelacao")).Items.Count == 4, "Relações por tipo");
            }
        }
    }
}

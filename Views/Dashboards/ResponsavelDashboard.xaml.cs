using StudyFlow.Data.Models;
using StudyFlow.Data;
using StudyFlow.Views.Relatorios;
using StudyFlow.Views.Autenticacao;

namespace StudyFlow.Views.Dashboards;

public partial class ResponsavelDashboard : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuarioLogado;

    public ResponsavelDashboard(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuarioLogado = usuario;
        lblBoasVindas.Text = $"Bem-vindo, {usuario.Nome}";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarFilhos();
    }

    // 1. CARREGA A LISTA DE FILHOS VINCULADOS
    private async Task CarregarFilhos()
    {
        await _db.InitAsync();

        // Pega o registro de Responsável do banco
        var responsaveis = await _db.ListarResponsaveisAsync();
        var resp = responsaveis.FirstOrDefault(r => r.IdUsuario == _usuarioLogado.IdUsuario);

        if (resp != null)
        {
            var todosAlunos = await _db.ListarAlunosAsync();
            // Filtra os alunos que possuem o Id deste responsável
            var meusFilhos = todosAlunos.Where(a => a.IdResponsavel == resp.IdResponsavel).ToList();
            listaFilhos.ItemsSource = meusFilhos;
        }
    }

    // 2. LÓGICA DE VÍNCULO POR CPF 
    private async void OnVincularFilhoClicked(object sender, EventArgs e)
    {
        var cpfDigitado = entryCpfFilho.Text?.Trim();

        if (string.IsNullOrWhiteSpace(cpfDigitado))
        {
            await DisplayAlert("Atenção", "Digite o CPF do aluno.", "OK");
            return;
        }

        await _db.InitAsync();

        // Procura o aluno pelo CPF informado
        var todosAlunos = await _db.ListarAlunosAsync();
        var alunoEncontrado = todosAlunos.FirstOrDefault(a => a.CPF == cpfDigitado);

        if (alunoEncontrado == null)
        {
            await DisplayAlert("Erro", "Aluno não encontrado com este CPF.", "OK");
            return;
        }

        // Verifica qual o ID do Responsável logado
        var responsaveis = await _db.ListarResponsaveisAsync();
        var resp = responsaveis.FirstOrDefault(r => r.IdUsuario == _usuarioLogado.IdUsuario);

        if (resp != null)
        {
            // Atribui o responsável ao aluno e salva no banco
            alunoEncontrado.IdResponsavel = resp.IdResponsavel;
            await _db.UpdateAsync(alunoEncontrado);

            await DisplayAlert("Sucesso", "Filho vinculado com sucesso!", "OK");
            entryCpfFilho.Text = string.Empty;
            await CarregarFilhos(); // Recarrega a lista na tela
        }
    }

    // 3. ACESSAR RELATÓRIO AO TOCAR NO CARD DO FILHO
    private async void OnFilhoSelected(object sender, TappedEventArgs e)
    {
        var frame = (Frame)sender;
        var alunoSelecionado = (Aluno)frame.BindingContext;

        if (alunoSelecionado != null)
        {
            // Navega para a página de relatórios passando o ID do aluno
            await Navigation.PushAsync(new ListaRelatoriosPage(alunoSelecionado.IdAluno));
        }
    }

    // 4. BOTÃO GERAL DE COMPORTAMENTO
    private async void OnVerComportamentoClicked(object sender, EventArgs e)
    {
 
        var lista = (List<Aluno>)listaFilhos.ItemsSource;

        if (lista != null && lista.Count == 1)
        {
            await Navigation.PushAsync(new ListaRelatoriosPage(lista[0].IdAluno));
        }
        else
        {
            await DisplayAlert("Aviso", "Toque diretamente no card do filho que deseja avaliar o comportamento.", "OK");
        }
    }

    // 5. SAIR
    private async void OnSairClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Sair", "Deseja realmente sair?", "Sim", "Não");
        if (confirm)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
using StudyFlow.Data.Models;
using StudyFlow.Views.Tarefas;
using StudyFlow.Data;
using StudyFlow.Views.Relatorios;

namespace StudyFlow.Views.Dashboards;

public partial class AlunoDashboard : ContentPage
{
    // Criamos uma variável para guardar os dados do usuário nesta tela
    private Usuario _usuarioLogado;
    private readonly StudyFlowDatabaseService _db;

    // Alterar o construtor para RECEBER o Usuario
    public AlunoDashboard(Usuario usuario)
    {
        InitializeComponent();

        _usuarioLogado = usuario;
        _db = new StudyFlowDatabaseService();

        lblBoasVindas.Text = $"Bem-vindo, {usuario.Nome}";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        // Volta para a tela de login resetando a pilha
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private async void OnVerTarefasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TarefasAlunoPage(_usuarioLogado));
    }

    private async void OnVerRelatoriosClicked(object sender, EventArgs e)
    {
        // 1. Pega o objeto Aluno vinculado ao usuário logado
        var alunos = await _db.ListarAlunosAsync();
        var eu = alunos.FirstOrDefault(a => a.IdUsuario == _usuarioLogado.IdUsuario);

        // 2. Abre a página passando o ID dele mesmo
        await Navigation.PushAsync(new ListaRelatoriosPage(eu.IdAluno)); ;
    }
}
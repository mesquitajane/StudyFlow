using StudyFlow.Data;
using StudyFlow.Data.Models;
using StudyFlow.Views.Tarefas;
using StudyFlow.Views.Relatorios;
using StudyFlow.Views.SecretariaPages;

namespace StudyFlow.Views.Dashboards;

public partial class ProfessorDashboard : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuario;

    public ProfessorDashboard(Usuario usuario)
    {
        InitializeComponent();

        _usuario = usuario;
        _db = new StudyFlowDatabaseService();

        lblBoasVindas.Text = $"Prof. {usuario.Nome}";

        CarregarDisciplina();
    }

    private async void CarregarDisciplina()
    {
        var professores = await _db.ListarProfessoresAsync();

        var professor = professores.FirstOrDefault(p => p.IdUsuario == _usuario.IdUsuario);

        if (professor != null)
            lblDisciplina.Text = $"Disciplina: {professor.Disciplina}";
        else
            lblDisciplina.Text = "Disciplina: Não cadastrada";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
    private async void OnCadastrarTarefaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastrarTarefaPage(_usuario));
    }
    private async void OnVerTarefasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MinhasTarefasPage(_usuario));
    }

    private async void OnRelatorioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CriarRelatorioPage(_usuario));
    }

    private async void OnVerComportamentoClicked(object sender, EventArgs e)
    {
        var professores = await _db.ListarProfessoresAsync();
        var professor = professores.FirstOrDefault(p => p.IdUsuario == _usuario.IdUsuario);

        if (professor == null)
        {
            await DisplayAlert("Erro", "Professor não encontrado.", "OK");
            return;
        }

        await Navigation.PushAsync(new ListaRelatoriosProfPage(professor.IdProfessor));
    }

    private async void OnVerAvisosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MuralAvisosPage(_usuario));
    }
    private async void OnLancarNotasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LancamentoNotasPage(_usuario));
    }
}

using StudyFlow.Data;
using StudyFlow.Data.Models;

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
}

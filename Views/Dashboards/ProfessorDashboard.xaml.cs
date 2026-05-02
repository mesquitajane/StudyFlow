using StudyFlow.Data.Models;

namespace StudyFlow.Views.Dashboards;

public partial class ProfessorDashboard : ContentPage
{
    private readonly Usuario _usuario;
    public ProfessorDashboard(Usuario usuario)
    {
        InitializeComponent();
        _usuario = usuario;
        lblBoasVindas.Text = $"Prof. {usuario.Nome}";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private async void OnCadastrarTarefaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastrarTarefaPage(_usuario));
    }
}
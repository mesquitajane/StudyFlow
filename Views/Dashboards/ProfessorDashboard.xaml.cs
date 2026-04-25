using StudyFlow.Data.Models;

namespace StudyFlow.Views.Dashboards;

public partial class ProfessorDashboard : ContentPage
{
    public ProfessorDashboard(Usuario usuario)
    {
        InitializeComponent();
        lblBoasVindas.Text = $"Prof. {usuario.Nome}";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
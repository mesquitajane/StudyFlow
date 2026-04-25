using StudyFlow.Data.Models;

namespace StudyFlow.Views.Dashboards;

public partial class ResponsavelDashboard : ContentPage
{
    public ResponsavelDashboard(Usuario usuario)
    {
        InitializeComponent();
        lblBoasVindas.Text = $"Bem-vindo, {usuario.Nome}";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
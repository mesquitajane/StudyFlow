using StudyFlow.Data.Models;

namespace StudyFlow.Views.Dashboards;

public partial class SecretariaDashboard : ContentPage
{
    public SecretariaDashboard(Usuario usuario)
    {
        InitializeComponent();
        lblInfo.Text = $"Operador: {usuario.Nome}";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
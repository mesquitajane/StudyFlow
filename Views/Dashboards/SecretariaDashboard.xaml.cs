using StudyFlow.Data.Models;
using StudyFlow.Views.SecretariaPages;

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

    private async void OnGerenciarUsuarioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GerenciarUsuarioPage());
    }

     private async void OnMatriculasTurmasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MatriculasTurmasPage());
    }

}
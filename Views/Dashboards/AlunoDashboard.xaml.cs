using StudyFlow.Data.Models;

namespace StudyFlow.Views.Dashboards;

public partial class AlunoDashboard : ContentPage
{
    // Criamos uma variável para guardar os dados do usuário nesta tela
    private Usuario _usuarioLogado;

    // Alterar o construtor para RECEBER o Usuario
    public AlunoDashboard(Usuario usuario)
    {
        InitializeComponent();

        _usuarioLogado = usuario;

        lblBoasVindas.Text = $"Bem-vindo, {usuario.Nome}";
    }

    private async void OnSairClicked(object sender, EventArgs e)
    {
        // Volta para a tela de login resetando a pilha
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private void OnVerTarefasClicked(object sender, EventArgs e)
    {

    }
}
using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow;

public partial class LoginPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;

    public LoginPage()
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = entryEmail.Text?.Trim();
        string senha = entrySenha.Text;

        lblErro.IsVisible = false;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            lblErro.Text = "Preencha todos os campos.";
            lblErro.IsVisible = true;
            return;
        }

        await _db.InitAsync();

        var usuarios = await _db.ListarUsuariosAsync();

        var usuario = usuarios
            .FirstOrDefault(u => u.Email == email && u.SenhaHash == senha);

        if (usuario == null)
        {
            lblErro.Text = "E-mail ou senha inválidos.";
            lblErro.IsVisible = true;
            return;
        }

        // LOGIN OK
        await DisplayAlert("Sucesso", $"Bem-vindo {usuario.TipoUsuario}", "OK");

        // Por enquanto só mostra o tipo
        switch (usuario.TipoUsuario.ToLower())
        {
            case "aluno":
                await DisplayAlert("Login", "Usuário é ALUNO", "OK");
                break;

            case "professor":
                await DisplayAlert("Login", "Usuário é PROFESSOR", "OK");
                break;

            case "responsavel":
                await DisplayAlert("Login", "Usuário é RESPONSÁVEL", "OK");
                break;

            case "secretaria":
                await DisplayAlert("Login", "Usuário é SECRETARIA", "OK");
                break;

            default:
                await DisplayAlert("Login", "Tipo desconhecido", "OK");
                break;
        }
    }

     private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//InicioPage");
    }
}
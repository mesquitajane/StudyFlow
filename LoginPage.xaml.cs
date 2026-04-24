namespace StudyFlow;

public partial class LoginPage : ContentPage
{

    public LoginPage()
	{
		InitializeComponent();
	}

    private async void Entrar_Clicked(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        string senha = txtSenha.Text;

        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
        {
            await DisplayAlert("Sucesso", "Login realizado!", "OK");
        }
        else
        {
            await DisplayAlert("Erro", "Preencha todos os campos", "OK");
        }

    }

    private async void Cadastrar_Tapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Cadastro", "Ir para tela de cadastro", "OK");
    }
}
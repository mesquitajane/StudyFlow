namespace StudyFlow;

public partial class InicioPage : ContentPage
{
	public InicioPage()
	{
		InitializeComponent();
	}

    private async void Login_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void Cadastrar_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Cadastro", "Tela de cadastro ainda será criada", "OK");
    }
}
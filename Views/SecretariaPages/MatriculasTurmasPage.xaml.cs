using StudyFlow.Views.Dashboards;
namespace StudyFlow.Views.SecretariaPages;

public partial class MatriculasTurmasPage : ContentPage
{
	public MatriculasTurmasPage()
	{
		InitializeComponent();
	}

    private async void OnCadastrarTurmaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastrarTurmaPage());
    }

    private async void OnListarTurmasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListarTurmasPage());
    }

    private async void OnMatricularAlunoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MatricularAlunoPage());
    }

    private async void OnListarMatriculasClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Listar Matrículas (próximo passo)", "OK");
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
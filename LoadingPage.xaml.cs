namespace StudyFlow;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
        AnimarCarregamento();
    }

    private async void AnimarCarregamento()
    {
        for (double i = 0; i <= 1; i += 0.01)
        {
            barraProgresso.Progress = i;
            await Task.Delay(30); // controla a velocidade
        }

        await Shell.Current.GoToAsync("//LoginPage");
    }
}
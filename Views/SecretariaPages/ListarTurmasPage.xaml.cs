using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class ListarTurmasPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new StudyFlowDatabaseService();

    public ListarTurmasPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _db.InitAsync();
        var turmas = await _db.ListarTurmasAsync();

        listaTurmas.ItemsSource = turmas;
    }
}
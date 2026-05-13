using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class CadastrarTurmaPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new StudyFlowDatabaseService();
    public CadastrarTurmaPage()
	{
		InitializeComponent();
	}

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        var turma = new Turma
        {
            Nome = entryNome.Text,
            Periodo = pickerPeriodo.SelectedItem?.ToString(),
            Nivel = pickerNivel.SelectedItem?.ToString(),
        };

        await _db.InitAsync();
        await _db.InserirTurmaAsync(turma);

        await DisplayAlert("Sucesso", "Turma salva!", "OK");

        await Navigation.PopAsync();
    }
}
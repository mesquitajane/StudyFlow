using StudyFlow.Data.Models;
using StudyFlow.Data;

namespace StudyFlow.Views.Relatorios;

public partial class MinhasNotasPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly int _idAluno;

    public MinhasNotasPage(int idAluno)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _idAluno = idAluno;
        CarregarNotas();
    }

    private async void CarregarNotas()
    {
        var notas = await _db.ListarNotasAlunoAsync(_idAluno);
        listaNotas.ItemsSource = notas;

        if (notas.Any())
        {
            double media = notas.Average(n => n.Nota);
            lblMediaGeral.Text = media.ToString("F1");
        }
    }

    private async void OnVoltarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}
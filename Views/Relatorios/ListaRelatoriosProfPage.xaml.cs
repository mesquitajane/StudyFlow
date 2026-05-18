namespace StudyFlow.Views.Relatorios;
using StudyFlow.Data;

public partial class ListaRelatoriosProfPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly int _idProfessorAlvo;

    public ListaRelatoriosProfPage(int idProfessor)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _idProfessorAlvo = idProfessor;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();

        var relatorios = await _db.ListarRelatoriosPorProfessorAsync(_idProfessorAlvo);
        listaRelatorios.ItemsSource = relatorios;

        if (relatorios.Count == 0)
        {
            await DisplayAlert("Informação", "Nenhum relatório registrado por este professor até o momento.", "OK");
        }
    }
}
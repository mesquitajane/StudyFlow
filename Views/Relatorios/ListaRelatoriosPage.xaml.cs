namespace StudyFlow.Views.Relatorios;
using StudyFlow.Data;

public partial class ListaRelatoriosPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly int _idAlunoAlvo;

    public ListaRelatoriosPage(int idAluno)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _idAlunoAlvo = idAluno;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();

        var relatorios = await _db.ListarRelatoriosDetalhesAsync(_idAlunoAlvo);
        listaRelatorios.ItemsSource = relatorios;

        if (relatorios.Count == 0)
        {
            await DisplayAlert("Informação", "Nenhum relatório registrado até o momento.", "OK");
        }
    }
}
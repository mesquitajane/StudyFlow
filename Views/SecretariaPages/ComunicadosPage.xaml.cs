using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class ComunicadosPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private List<Comunicado> _todosComunicados;

    public ComunicadosPage()
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarComunicados();
    }

    private async Task CarregarComunicados()
    {
        _todosComunicados = await _db.ListarComunicadosAsync();
        listaComunicados.ItemsSource = _todosComunicados;
    }

    private void OnPesquisarTextChanged(object sender, TextChangedEventArgs e)
    {
        var termo = e.NewTextValue?.ToLower() ?? "";
        listaComunicados.ItemsSource = string.IsNullOrWhiteSpace(termo)
            ? _todosComunicados
            : _todosComunicados.Where(c => c.Titulo.ToLower().Contains(termo)).ToList();
    }

    private async void OnNovoComunicadoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastrarComunicadoPage(null)); // null significa "Novo"
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var comunicado = (Comunicado)btn.CommandParameter;
        await Navigation.PushAsync(new CadastrarComunicadoPage(comunicado)); // Passa o objeto para editar
    }

    private async void OnExcluirClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Atenção", "Tem certeza que deseja excluir este comunicado?", "Sim", "Não");
        if (confirmar)
        {
            var btn = (Button)sender;
            int id = (int)btn.CommandParameter;
            await _db.ExcluirComunicadoAsync(id);
            await CarregarComunicados(); // Recarrega a lista
        }
    }

    private async void OnVoltarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}
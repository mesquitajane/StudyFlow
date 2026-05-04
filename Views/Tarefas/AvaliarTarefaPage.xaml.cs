using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.Tarefas;

public partial class AvaliarTarefaPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Tarefa _tarefa;

    public AvaliarTarefaPage(Tarefa tarefa)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _tarefa = tarefa;
        lblTarefaNome.Text = _tarefa.Titulo;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();

        var todasEntregas = await _db.ListarEntregasAsync();
        // Filtra apenas as entregas DESTA tarefa que não sejam Rascunho
        var entregasParaCorrigir = todasEntregas
            .Where(e => e.IdTarefa == _tarefa.IdTarefa && e.Status != "Rascunho")
            .ToList();

        listaEntregas.ItemsSource = entregasParaCorrigir;
    }

    private async void OnAvaliarClicked(object sender, EventArgs e)
    {
        var entrega = (Entrega)((Button)sender).CommandParameter;

        // Abre um prompt para o professor digitar a nota
        string resultado = await DisplayPromptAsync("Avaliação",
            $"Texto: {entrega.RespostaTexto}\n\nDigite a nota (0-10):",
            "Salvar", "Cancelar", "Nota aqui", keyboard: Keyboard.Numeric);

        if (!string.IsNullOrEmpty(resultado) && double.TryParse(resultado, out double nota))
        {
            entrega.Nota = nota;
            entrega.Status = "Avaliado";
            await _db.AtualizarEntregaAsync(entrega);

            await DisplayAlert("Sucesso", "Nota atribuída!", "OK");
            OnAppearing(); // Recarrega a lista
        }
    }
}
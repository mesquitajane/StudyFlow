using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.Tarefas;

public partial class MinhasTarefasPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuario;

    public MinhasTarefasPage(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuario = usuario;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitAsync();

        var professores = await _db.ListarProfessoresAsync();
        var professorLogado = professores.FirstOrDefault(p => p.IdUsuario == _usuario.IdUsuario);

        if (professorLogado != null)
        {
            var todas = await _db.ListarTarefasAsync();
            // FILTRO: Onde o IdProfessor da tarefa é o ID do professor logado
            var minhasTarefas = todas.Where(t => t.IdProfessor == professorLogado.IdProfessor).ToList();

            listaTarefas.ItemsSource = minhasTarefas;
            lblSemTarefas.IsVisible = !minhasTarefas.Any();
        }
    }

    private async void OnExcluirClicked(object sender, EventArgs e)
    {
        var tarefa = (Tarefa)((Button)sender).CommandParameter;
        if (await DisplayAlert("Excluir", "Deseja apagar esta tarefa?", "Sim", "Não"))
        {
            await _db.DeletarTarefaAsync(tarefa.IdTarefa);
            OnAppearing();
        }
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var tarefa = (Tarefa)((Button)sender).CommandParameter;
        await Navigation.PushAsync(new EditarTarefaPage(tarefa));
    }

    private async void Button_Clicked(object sender, EventArgs e) => await Navigation.PopAsync();
}

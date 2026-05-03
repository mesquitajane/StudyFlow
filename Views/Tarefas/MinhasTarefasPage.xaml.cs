using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views;

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

        var professores = await _db.ListarProfessoresAsync();

        var professor = professores.FirstOrDefault(p => p.IdUsuario == _usuario.IdUsuario);

        if (professor == null)
        {
            await DisplayAlert("Erro", "Professor não encontrado.", "OK");
            return;
        }

        var tarefas = await _db.ListarTarefasAsync();

        var tarefasDoProfessor = tarefas.ToList();

        listaTarefas.ItemsSource = tarefasDoProfessor;

        lblSemTarefas.IsVisible = tarefasDoProfessor.Count == 0;
        listaTarefas.IsVisible = tarefasDoProfessor.Count > 0;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnExcluirClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var tarefa = (Tarefa)button.CommandParameter;

        bool confirmar = await DisplayAlert("Excluir", "Deseja excluir essa tarefa?", "Sim", "Não");

        if (!confirmar) return;

        await _db.DeletarTarefaAsync(tarefa.IdTarefa);

        OnAppearing(); // recarrega lista
    }
    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var tarefa = (Tarefa)button.CommandParameter;

        await Navigation.PushAsync(new EditarTarefaPage(tarefa));
    }
    private async void OnConcluirClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var tarefa = (Tarefa)button.CommandParameter;

        tarefa.Status = "Concluída";

        await _db.UpdateAsync(tarefa);

        await DisplayAlert("Sucesso", "Tarefa concluída!", "OK");

        OnAppearing(); // recarrega a lista
    }
}

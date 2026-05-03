using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views;

public partial class EditarTarefaPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private Tarefa _tarefa;

    public EditarTarefaPage(Tarefa tarefa)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _tarefa = tarefa;

        // Preenche os campos
        entryTitulo.Text = tarefa.Titulo;
        editorDescricao.Text = tarefa.Descricao;
        dateEntrega.Date = tarefa.DataEntrega;
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        _tarefa.Titulo = entryTitulo.Text;
        _tarefa.Descricao = editorDescricao.Text;
        _tarefa.DataEntrega = dateEntrega.Date;

        await _db.InitAsync();
        await _db.UpdateAsync(_tarefa);

        await DisplayAlert("Sucesso", "Tarefa atualizada!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

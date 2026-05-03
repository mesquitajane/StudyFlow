using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views;

public partial class CadastrarTarefaPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuario;

    public CadastrarTarefaPage(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuario = usuario;
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryTitulo.Text))
        {
            await DisplayAlert("Erro", "Digite um título.", "OK");
            return;
        }

        await _db.InitAsync();
        var professores = await _db.ListarProfessoresAsync();

        var professor = professores.FirstOrDefault(p => p.IdUsuario == _usuario.IdUsuario);

        if (professor == null)
        {
            await DisplayAlert("Erro", "Professor não encontrado!", "OK");
            return;
        }

        var tarefa = new Tarefa
        {
            Titulo = entryTitulo.Text,
            Descricao = entryDescricao.Text,
            DataEntrega = dateEntrega.Date,
            Status = "Pendente",
            Turma = entryTurma.Text
        };

        await _db.InserirTarefaAsync(tarefa);

        await DisplayAlert("Sucesso", "Tarefa cadastrada!", "OK");

        await Navigation.PopAsync();
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

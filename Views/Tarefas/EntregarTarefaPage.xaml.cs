using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.Tarefas;

public partial class EntregarTarefaPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Tarefa _tarefa;
    private readonly Aluno _aluno;
    private string _caminhoArquivo = "";

    public EntregarTarefaPage(Tarefa tarefa, Aluno aluno)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _tarefa = tarefa;
        _aluno = aluno;

        PreencherDadosTarefa();
        CarregarEntregaExistente();
    }

    private void PreencherDadosTarefa()
    {
        lblTitulo.Text = _tarefa.Titulo;
        lblDescricao.Text = _tarefa.Descricao;
        lblData.Text = $"Prazo: {_tarefa.DataEntrega:dd/MM/yyyy}";
    }

    private async void CarregarEntregaExistente()
    {
        await _db.InitAsync();
        var entregas = await _db.ListarEntregasAsync(); // Método a criar no DBService
        var entrega = entregas.FirstOrDefault(e => e.IdTarefa == _tarefa.IdTarefa && e.IdAluno == _aluno.IdAluno);

        if (entrega != null)
        {
            editorResposta.Text = entrega.RespostaTexto;
            _caminhoArquivo = entrega.CaminhoArquivo;
            if (!string.IsNullOrEmpty(_caminhoArquivo))
                lblArquivo.Text = "Arquivo já anexado anteriormente.";
        }
    }

    private async void OnAnexarClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();
        if (result != null)
        {
            _caminhoArquivo = result.FullPath;
            lblArquivo.Text = $"Arquivo: {result.FileName}";
        }
    }

    private async void OnSalvarRascunhoClicked(object sender, EventArgs e) => await Salvar("Rascunho");
    private async void OnEnviarClicked(object sender, EventArgs e) => await Salvar("Entregue");

    private async Task Salvar(string status)
    {
        var entrega = new Entrega
        {
            IdTarefa = _tarefa.IdTarefa,
            IdAluno = _aluno.IdAluno,
            RespostaTexto = editorResposta.Text,
            CaminhoArquivo = _caminhoArquivo,
            Status = status,
            DataEntrega = DateTime.Now
        };

        // Lógica de Insert or Replace (depende do seu DBService)
        await _db.SalvarEntregaAsync(entrega);
        await DisplayAlert("Sucesso", status == "Rascunho" ? "Rascunho salvo!" : "Tarefa enviada!", "OK");
        await Navigation.PopAsync();
    }
}
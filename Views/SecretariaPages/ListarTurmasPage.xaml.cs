using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class ListarTurmasPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new StudyFlowDatabaseService();

    private Turma? turmaEmEdicao;

    public ListarTurmasPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _db.InitAsync();
        var turmas = await _db.ListarTurmasAsync();

        listaTurmas.ItemsSource = turmas;

        
    }

    private void OnEditarTurmaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.Parent is HorizontalStackLayout botoes &&
            botoes.Parent is VerticalStackLayout layout)
        {
            foreach (var view in layout.Children)
            {
                if (view is Entry entry)
                    entry.IsVisible = true;

                if (view is Picker picker)
                    picker.IsVisible = true;

                if (view is HorizontalStackLayout painelBotoes)
                {
                    foreach (var item in painelBotoes.Children)
                    {
                        if (item is Button btn && btn.Text == "Salvar")
                            btn.IsVisible = true;
                    }
                }
            }
        }
    }

    private async void OnExcluirTurmaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var turma = button?.BindingContext as Turma;

        if (turma == null)
            return;

        bool confirmar = await DisplayAlert("Excluir", $"Deseja excluir a turma {turma.Nome}?", "Sim", "Não");

        if (!confirmar)
            return;

        await _db.DeletarTurmaAsync(turma);

        listaTurmas.ItemsSource = await _db.ListarTurmasAsync();
    }

    private async void OnSalvarTurmaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var turma = button?.BindingContext as Turma;

        if (turma == null)
            return;

        await _db.AtualizarTurmaAsync(turma);

        if (button?.Parent is HorizontalStackLayout botoes &&
            botoes.Parent is VerticalStackLayout layout)
        {
            foreach (var view in layout.Children)
            {
                if (view is Entry entry)
                    entry.IsVisible = false;

                if (view is Picker picker)
                    picker.IsVisible = false;

                if (view is Button btn && btn.Text == "Salvar")
                    btn.IsVisible = false;
            }

            await DisplayAlert("Sucesso", "Turma atualizada com sucesso!", "OK");

            listaTurmas.ItemsSource = await _db.ListarTurmasAsync();
        }
    }

    private async void OnFiltroChanged(object sender, EventArgs e)
    {
      
        string? nome = searchNomeFiltro.Text;
        string? nivel = pickerNivelFiltro.SelectedItem?.ToString();
        string? periodo = pickerPeriodoFiltro.SelectedItem?.ToString();

        // Limpa filtros “Todos” ou vazios
        if (string.IsNullOrWhiteSpace(nome))
            nome = null;

        if (string.IsNullOrWhiteSpace(nivel) || nivel == "Todos")
            nivel = null;

        if (string.IsNullOrWhiteSpace(periodo) || periodo == "Todos")
            periodo = null;

        var turmasFiltradas = await _db.FiltrarTurmasAsync(nome, nivel, periodo);

        listaTurmas.ItemsSource = turmasFiltradas;
    }
}

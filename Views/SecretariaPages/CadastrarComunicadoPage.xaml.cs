using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class CadastrarComunicadoPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private Comunicado _comunicadoAtual;

    // Construtor recebe o comunicado. Se for null, é um cadastro novo.
    public CadastrarComunicadoPage(Comunicado comunicado)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _comunicadoAtual = comunicado;

        ConfigurarPagina();
    }

    private void ConfigurarPagina()
    {
        if (_comunicadoAtual != null)
        {
            lblTituloPagina.Text = "Editar Comunicado";
            entryTitulo.Text = _comunicadoAtual.Titulo;
            editorMensagem.Text = _comunicadoAtual.Mensagem;
            pickerPublico.SelectedItem = _comunicadoAtual.PublicoAlvo;
        }
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(entryTitulo.Text) ||
            string.IsNullOrWhiteSpace(editorMensagem.Text) ||
            pickerPublico.SelectedIndex == -1)
        {
            await DisplayAlert("Erro", "Preencha todos os campos antes de salvar.", "OK");
            return;
        }

        // Se for novo, instancia um novo objeto. Se for edição, aproveita o Id atual.
        if (_comunicadoAtual == null)
        {
            _comunicadoAtual = new Comunicado();
        }

        _comunicadoAtual.Titulo = entryTitulo.Text;
        _comunicadoAtual.Mensagem = editorMensagem.Text;
        _comunicadoAtual.PublicoAlvo = pickerPublico.SelectedItem.ToString();
        _comunicadoAtual.DataPublicacao = DateTime.Now; // Atualiza a data para o momento da postagem/edição

        await _db.SalvarComunicadoAsync(_comunicadoAtual);

        await DisplayAlert("Sucesso", "Comunicado salvo com sucesso!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnCancelarClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}
using StudyFlow.Data;
using System.Linq;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class GerenciarUsuarioPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;

    private int? _usuarioEmEdicao = null;
    public GerenciarUsuarioPage()
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();

        CarregarUsuarios();
    }

    private async void OnCadastrarUsuarioClicked(object sender, EventArgs e)
    {
        string nome = entryNome.Text?.Trim();
        string email = entryEmail.Text?.Trim();
        string senha = entrySenha.Text?.Trim();
        string tipo = pickerTipoUsuario.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(nome) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(senha) ||
            string.IsNullOrEmpty(tipo))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }
        if (_usuarioEmEdicao != null)
        {
            Usuario usuarioAtualizado = new Usuario
            {
                IdUsuario = _usuarioEmEdicao.Value,
                Nome = nome,
                Email = email,
                SenhaHash = senha,
                TipoUsuario = tipo
            };

            await _db.AtualizarUsuarioAsync(usuarioAtualizado);

            await DisplayAlert("Sucesso", "Usuário atualizado com sucesso!", "OK");

            _usuarioEmEdicao = null;

            // Limpar campos
            entryNome.Text = string.Empty;
            entryEmail.Text = string.Empty;
            entrySenha.Text = string.Empty;
            pickerTipoUsuario.SelectedIndex = -1;

            // Atualizar lista
            await CarregarUsuarios();
        }
        else
        {


            Usuario novoUsuario = new Usuario
            {
                Nome = nome,
                Email = email,
                SenhaHash = senha,
                TipoUsuario = tipo
            };

            await _db.InserirUsuarioAsync(novoUsuario);

            await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");

            // Limpar campos
            entryNome.Text = string.Empty;
            entryEmail.Text = string.Empty;
            entrySenha.Text = string.Empty;
            pickerTipoUsuario.SelectedIndex = -1;

            // Atualizar lista
            await CarregarUsuarios();
        }
    }

    private async Task CarregarUsuarios()
    {
        listaUsuarios.ItemsSource = await _db.ListarUsuariosAsync();
    }

    private async void OnExcluirUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.CommandParameter == null)
            return;

        int idUsuario = Convert.ToInt32(button.CommandParameter);

        bool confirmar = await DisplayAlert(
            "Confirmar Exclusão",
            "Deseja realmente excluir este usuário?",
            "Sim",
            "Não"
        );

        if (!confirmar)
            return;

        await _db.DeletarUsuarioAsync(idUsuario);

        await DisplayAlert("Sucesso", "Usuário excluído com sucesso!", "OK");

        await CarregarUsuarios();
    }

    private async void OnEditarUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.CommandParameter == null)
            return;

        int idUsuario = Convert.ToInt32(button.CommandParameter);

        var usuarios = await _db.ListarUsuariosAsync();
        var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

        if (usuario == null)
            return;

        _usuarioEmEdicao = usuario.IdUsuario;

        entryNome.Text = usuario.Nome;
        entryEmail.Text = usuario.Email;
        entrySenha.Text = usuario.SenhaHash;

        pickerTipoUsuario.SelectedItem = usuario.TipoUsuario;

        await scrollPagina.ScrollToAsync(0, 0, true);

        await DisplayAlert(
            "Modo Edição",
            "Agora altere os campos acima e clique em Cadastrar Usuário para salvar as alterações.",
            "OK"
        );
    }

    private async void OnPesquisarUsuarioTextChanged(object sender, TextChangedEventArgs e)
    {
        string textoPesquisa = e.NewTextValue?.ToLower() ?? "";

        var usuarios = await _db.ListarUsuariosAsync();

        var usuariosFiltrados = usuarios.Where(u =>
            u.Nome.ToLower().Contains(textoPesquisa) ||
            u.Email.ToLower().Contains(textoPesquisa)
        ).ToList();

        listaUsuarios.ItemsSource = usuariosFiltrados;
    }
}
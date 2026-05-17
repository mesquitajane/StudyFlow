using StudyFlow.Data;

namespace StudyFlow.Views.SecretariaPages;

public partial class ConfiguracoesSistemaPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db = new();

    public ConfiguracoesSistemaPage()
    {
        InitializeComponent();
    }


    private async void OnSalvarSenhaClicked(object sender, EventArgs e)
    {
        string novaSenha = entryNovaSenha.Text?.Trim() ?? "";
        string confirmarSenha = entryConfirmarSenha.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(novaSenha) ||
            string.IsNullOrWhiteSpace(confirmarSenha))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }

        if (novaSenha != confirmarSenha)
        {
            await DisplayAlert("Erro", "As senhas não coincidem.", "OK");
            return;
        }

        var usuarios = await _db.ListarUsuariosAsync();

        var secretarias = usuarios
            .Where(u => u.TipoUsuario == "Secretaria")
            .ToList();

        if (!secretarias.Any())
        {
            await DisplayAlert("Erro", "Nenhuma secretaria encontrada.", "OK");
            return;
        }

        foreach (var secretaria in secretarias)
        {
            secretaria.SenhaHash = novaSenha;
            await _db.AtualizarUsuarioAsync(secretaria);
        }

        await DisplayAlert("Sucesso", "Senha atualizada com sucesso!", "OK");

        entryNovaSenha.Text = string.Empty;
        entryConfirmarSenha.Text = string.Empty;
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSalvarEmailClicked(object sender, EventArgs e)
    {
        string novoEmail = entryNovoEmail.Text?.Trim() ?? "";
        string confirmarEmail = entryConfirmarEmail.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(novoEmail) ||
            string.IsNullOrWhiteSpace(confirmarEmail))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }

        if (novoEmail != confirmarEmail)
        {
            await DisplayAlert("Erro", "Os emails não coincidem.", "OK");
            return;
        }

        var usuarios = await _db.ListarUsuariosAsync();

        var secretarias = usuarios
            .Where(u => u.TipoUsuario == "Secretaria")
            .ToList();

        if (!secretarias.Any())
        {
            await DisplayAlert("Erro", "Nenhuma secretaria encontrada.", "OK");
            return;
        }

        foreach (var secretaria in secretarias)
        {
            secretaria.Email = novoEmail;
            await _db.AtualizarUsuarioAsync(secretaria);
        }

        await DisplayAlert("Sucesso", "Email atualizado com sucesso!", "OK");

        entryNovoEmail.Text = string.Empty;
        entryConfirmarEmail.Text = string.Empty;
    }
}
using StudyFlow.Data.Models;
using StudyFlow.Data; 

namespace StudyFlow.Views.Autenticacao;

public partial class RegisterPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;

    public RegisterPage()
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
    }

    private void OnTipoUsuarioChanged(object sender, EventArgs e)
    {
        var selecionado = pickerTipo.SelectedItem?.ToString();
        layoutAluno.IsVisible = (selecionado == "Aluno");
        layoutProfessor.IsVisible = (selecionado == "Professor");
        layoutResponsavel.IsVisible = (selecionado == "Responsável");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Validações Iniciais
            if (string.IsNullOrWhiteSpace(entryNome.Text) ||
                string.IsNullOrWhiteSpace(entryEmail.Text) ||
                string.IsNullOrWhiteSpace(entrySenha.Text) ||
                pickerTipo.SelectedIndex == -1)
            {
                await DisplayAlert("Atenção", "Por favor, preencha todos os campos obrigatórios.", "OK");
                return;
            }

            if (!entryEmail.Text.Contains("@"))
            {
                await DisplayAlert("Erro", "Insira um e-mail válido.", "OK");
                return;
            }

            await _db.InitAsync();

            // 2. Criar o objeto Usuario
            var novoUsuario = new Usuario
            {
                Nome = entryNome.Text.Trim(),
                Email = entryEmail.Text.Trim().ToLower(),
                SenhaHash = entrySenha.Text, // Em um app real, use criptografia aqui
                TipoUsuario = pickerTipo.SelectedItem.ToString()
            };

            // 3. Salvar Usuário no Banco
            await _db.InserirUsuarioAsync(novoUsuario);

            // 4. Salvar dados específicos baseados no tipo
            string tipo = pickerTipo.SelectedItem.ToString();
            bool sucessoPerfil = false;

            switch (tipo)
            {
                case "Aluno":
                    // Validação do CPF e Turma
                    if (string.IsNullOrWhiteSpace(entryCpfAluno.Text) || entryCpfAluno.Text.Length < 11)
                        throw new Exception("Por favor, insira um CPF válido com 11 dígitos.");

                    if (pickerTurma.SelectedIndex == -1)
                        throw new Exception("Informe a turma do aluno.");

                    var aluno = new Aluno
                    {
                        IdUsuario = novoUsuario.IdUsuario,
                        CPF = entryCpfAluno.Text.Trim(), // SALVANDO O CPF AQUI
                        Turma = pickerTurma.SelectedItem.ToString(),
                        MatriculaAtiva = true
                    };

                    await _db.InserirAlunoAsync(aluno);
                    sucessoPerfil = true;
                    break;

                case "Professor":
                    if (pickerDisciplina.SelectedIndex == -1) throw new Exception("Informe a disciplina do professor.");
                    var prof = new Professor { IdUsuario = novoUsuario.IdUsuario, Disciplina = pickerDisciplina.SelectedItem.ToString() };
                    await _db.InserirProfessorAsync(prof);
                    sucessoPerfil = true;
                    break;

                case "Responsável":
                    if (string.IsNullOrWhiteSpace(entryParentesco.Text)) throw new Exception("Informe o grau de parentesco.");
                    var resp = new Responsavel { IdUsuario = novoUsuario.IdUsuario, Parentesco = entryParentesco.Text };
                    await _db.InserirResponsavelAsync(resp);
                    sucessoPerfil = true;
                    break;

                case "Secretaria":
                    var sec = new Secretaria { IdUsuario = novoUsuario.IdUsuario };
                    await _db.InserirSecretariaAsync(sec);
                    sucessoPerfil = true;
                    break;
            }

            if (sucessoPerfil)
            {
                await DisplayAlert("Sucesso", "Cadastro realizado! Você já pode entrar.", "OK");
                await Navigation.PopAsync(); // Volta para a tela de Login
            }
        }
        catch (Exception ex)
        {
            // Tratamento de erros (ex: e-mail duplicado ou erro de conexão)
            await DisplayAlert("Erro ao Cadastrar", $"Detalhes: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
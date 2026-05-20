using StudyFlow.Data;
using StudyFlow.Data.Models;

namespace StudyFlow.Views.SecretariaPages;

public partial class MuralAvisosPage : ContentPage
{
    private readonly StudyFlowDatabaseService _db;
    private readonly Usuario _usuarioLogado;

    // Recebemos o usuário logado no construtor para saber qual filtro aplicar
    public MuralAvisosPage(Usuario usuario)
    {
        InitializeComponent();
        _db = new StudyFlowDatabaseService();
        _usuarioLogado = usuario;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarAvisos();
    }

    private async Task CarregarAvisos()
    {
        // Todo mundo vê os comunicados marcados como "Todos"
        List<string> alvosPermitidos = new List<string> { "Todos" };

        // Adiciona os alvos específicos baseados em quem está logado
        if (_usuarioLogado.TipoUsuario == "Aluno")
        {
            alvosPermitidos.Add("Alunos");
        }
        else if (_usuarioLogado.TipoUsuario == "Professor")
        {
            alvosPermitidos.Add("Professores");
        }
        else if (_usuarioLogado.TipoUsuario == "Responsavel")
        {
            // O Responsável vê os avisos dele E os dos alunos
            alvosPermitidos.Add("Responsáveis"); 
            alvosPermitidos.Add("Alunos");
        }

        // Chama o serviço passando a lista inteira de permissões
        var avisos = await _db.ListarComunicadosPorPublicoAsync(alvosPermitidos);

        listaAvisos.ItemsSource = avisos;
    }
}
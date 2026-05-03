using SQLite;
using System;

namespace StudyFlow.Data.Models;

public class Usuario
{
    [PrimaryKey, AutoIncrement]
    public int IdUsuario { get; set; }

    [NotNull]
    public string Nome { get; set; } = string.Empty;

    [NotNull]
    public string Email { get; set; } = string.Empty;

    [NotNull]
    public string SenhaHash { get; set; } = string.Empty;

    [NotNull]
    public string TipoUsuario { get; set; } = string.Empty;
}

public class Aluno
{
    [PrimaryKey, AutoIncrement]
    public int IdAluno { get; set; }

    [Indexed, NotNull]
    public int IdUsuario { get; set; }

    [NotNull]
    public string Turma { get; set; } = string.Empty;

    public bool MatriculaAtiva { get; set; } = true;
}

public class Professor
{
    [PrimaryKey, AutoIncrement]
    public int IdProfessor { get; set; }

    [Indexed, NotNull]
    public int IdUsuario { get; set; }

    [NotNull]
    public string Disciplina { get; set; } = string.Empty;
}

public class Responsavel
{
    [PrimaryKey, AutoIncrement]
    public int IdResponsavel { get; set; }

    [Indexed, NotNull]
    public int IdUsuario { get; set; }

    [NotNull]
    public string Parentesco { get; set; } = string.Empty;
}

public class Secretaria
{
    [PrimaryKey, AutoIncrement]
    public int IdSecretaria { get; set; }

    [Indexed, NotNull]
    public int IdUsuario { get; set; }
}

public class Tarefa
{
    [PrimaryKey, AutoIncrement]
    public int IdTarefa { get; set; }

    [NotNull]
    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    [NotNull]
    public DateTime DataEntrega { get; set; }

    [NotNull]
    public string Status { get; set; } = string.Empty;
    public string Turma { get; set; } = string.Empty;

    [Indexed, NotNull]
    public int IdAluno { get; set; }

    [Indexed, NotNull]
    public int IdProfessor { get; set; }
}

public class Desempenho
{
    [PrimaryKey, AutoIncrement]
    public int IdDesempenho { get; set; }

    public double Nota { get; set; }

    [NotNull]
    public string TipoAvaliacao { get; set; } = string.Empty;

    public double Media { get; set; }

    [Indexed, NotNull]
    public int IdAluno { get; set; }

    [Indexed, NotNull]
    public int IdProfessor { get; set; }
}

public class RelatorioComportamental
{
    [PrimaryKey, AutoIncrement]
    public int IdRelatorio { get; set; }

    [Indexed, NotNull]
    public int IdAluno { get; set; }

    [Indexed, NotNull]
    public int IdProfessor { get; set; }

    [NotNull]
    public string Comportamento { get; set; } = string.Empty;

    public string? Observacoes { get; set; }

    public DateTime DataRegistro { get; set; } = DateTime.Now;
}

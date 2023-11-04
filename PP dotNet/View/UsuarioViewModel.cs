namespace PP_dotNet.View;

/// <summary>
/// Representa as informações de um dos usuários cadastrados no sistema.
/// </summary>
public readonly struct UsuarioViewModel
{
    /// <summary>
    /// O nome do usuário.
    /// </summary>
    public string Nome { get; init; }

    /// <summary>
    /// A data de nascimento do usuário.
    /// </summary>
    public DateOnly DataDeNascimento { get; init; }

    /// <summary>
    /// A data na qual o usuário fará 100 anos de idade.
    /// </summary>
    public DateOnly Centenario { get; init; }

    /// <summary>
    /// Prepara as informações de um usuário para ser exibida.
    /// </summary>
    /// <param name="nome"> O nome do usuário. </param>
    /// <param name="dataDeNascimento"> A data de nascimento do usuário. </param>
    /// <param name="centenario"> A data na qual o usuário fará 100 anos de idade. </param>
    public UsuarioViewModel(string nome, DateOnly dataDeNascimento, DateOnly centenario)
    {
        Nome = nome;
        DataDeNascimento = dataDeNascimento;
        Centenario = centenario;
    }

    public override string ToString()
    {
        return string.Format("{0}\t{1}\t{2}", Nome, DataDeNascimento, Centenario);
    }
}
namespace PP_dotNet.View;

/// <summary>
/// Representa um campo de um formulário.
/// </summary>
public class Campo
{
    /// <summary>
    /// O título deste campo.
    /// </summary>
    public string Titulo { get; init; }

    /// <summary>
    /// A entrada que o usuário digitou.
    /// </summary>
    public string Valor { get; private set; }

    /// <summary>
    /// Instancia um novo campo de formulário.
    /// </summary>
    /// <param name="titulo"> O título que é exibido na tela junto a um formulário. </param>
    public Campo(string titulo)
    {
        Titulo = titulo;
        Valor = string.Empty;
    }

    /// <summary>
    /// Lê a entrada do console e armazena-a dentro deste campo.
    /// </summary>
    public void Ler() => Valor = Console.ReadLine() ?? "N/A";
}

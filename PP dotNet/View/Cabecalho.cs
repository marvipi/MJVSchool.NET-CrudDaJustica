using System.Text;

namespace PP_dotNet.View;

/// <summary>
/// Representa as informações que são exibidas no topo do console.
/// </summary>
public class Cabecalho
{
    // Define como o nome do usuário será exibido.
    private const string FORMATO_USUARIO = "USUARIO: {0}";

    /// <summary>
    /// O nome do usuário que está autenticado no sistema.
    /// </summary>
    public string Usuario { get; private set; }

    /// <summary>
    /// O caminho da operação que está sendo exibida.
    /// </summary>
    public Stack<string> Caminho { get; init; }

    /// <summary>
    /// Uma imagem em ASCII.
    /// </summary>
    public string Logotipo { get; private set; } // TODO Mostrar logotipo

    /// <summary>
    /// Instancia um novo cabeçalho.
    /// </summary>
    /// <param name="usuario"> O usuário que está autenticado no sistema. </param>
    /// <param name="caminho"> O caminho da view que está sendo exibida. </param>
    /// <param name="logotipo"> Uma imagem em ASCII. </param>
    public Cabecalho(string usuario, string caminho, string logotipo)
    {
        Usuario = usuario;
        Caminho = new Stack<string>();
        Caminho.Push(caminho);
        Logotipo = logotipo;
    }

    /// <summary>
    /// Transforma este cabeçalho em uma string.
    /// </summary>
    /// <returns>
    /// Uma <see cref="string"/> que mostra o nome do usuário na primeira linha e o caminho na segunda.
    /// </returns>
    public override string ToString()
    {
        var stringBuilder =  new StringBuilder()
            .AppendFormat(FORMATO_USUARIO, Usuario)
            .AppendLine();

        foreach (var local in Caminho.Reverse())
        {
            stringBuilder.Append(local);
        }

        return stringBuilder.ToString();
    }
}

using System.Text;

namespace PP_dotNet.Data;

/// <summary>
/// Representa uma página de dados num repositório.
/// </summary>
public readonly struct Pagina
{
    /// <summary>
    /// O número da página.
    /// </summary>
    public int Numero { get; init; }

    /// <summary>
    /// A quantidade de elementos na página.
    /// </summary>
    public int Quantidade { get; init; }

    /// <summary>
    /// Cria uma nova página.
    /// </summary>
    /// <param name="numero"> O número da nova página. </param>
    /// <param name="quantidade"> A quantidade de elementos na página. </param>
    public Pagina(int numero, int quantidade)
    {
        if (int.IsNegative(numero))
        {
            var msg = new StringBuilder()
                .AppendFormat("O parâmetro {0} não pode ser negativo.", nameof(numero))
                .ToString();
            throw new ArgumentOutOfRangeException(msg);
        }

        if (quantidade < 1)
        {
            var msg = new StringBuilder()
                .AppendFormat("O parâmetro {0} não pode ser menor que 1.", nameof(quantidade))
                .ToString();
            throw new ArgumentOutOfRangeException(msg);
        }

        Numero = numero;
        Quantidade = quantidade;
    }
}

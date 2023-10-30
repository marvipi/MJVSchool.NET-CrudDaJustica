using System.Text;

namespace PP_dotNet.View;

/// <summary>
/// Representa as instruções que uma exibição pode receber.
/// </summary>
public class Instrucoes
{
    // O formato na qual as instruções serão exibidas.
    private const string FORMATO = "{0}: {1}";

    // A quantidade de espaço em branco entre instruções
    private int espacamento;

    /// <summary>
    /// Uma tecla e a operação atribuída a ela.
    /// </summary>
    private IEnumerable<KeyValuePair<ConsoleKey, string>> instrucoes;

    /// <summary>
    /// Instancia uma sequência de instruções que mapeia uma tecla a uma operação.
    /// </summary>
    /// <param name="instrucoes"> Um mapeamento de teclas a operações. </param>
    /// <param name="espacamento"> A quantidade de espaço entre instruções. </param>
    public Instrucoes(IEnumerable<KeyValuePair<ConsoleKey, string>> instrucoes, int espacamento)
    {
        this.instrucoes = instrucoes;
        this.espacamento = espacamento;
    }

    /// <summary>
    /// Transforma estas instruções em uma string.
    /// </summary>
    /// <returns>
    /// Uma <see cref="string"/> que contém as intruções no formato CHAVE: INSTRUÇÃO.
    /// </returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach (var instrucao in instrucoes)
        {
            var tecla = instrucao.Key.ToString().ToUpper();
            stringBuilder
                .AppendFormat(FORMATO, tecla, instrucao.Value)
                .Append(' ', espacamento);
        }

        return stringBuilder.ToString();
    }
}

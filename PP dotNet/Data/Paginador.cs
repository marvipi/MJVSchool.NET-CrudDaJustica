using System.Text;

namespace PP_dotNet.Data;

/// <summary>
/// Representa um serviço que faz paginação de dados.
/// </summary>
public class Paginador
{
    private int tamanhoRepositorio;

    /// <summary>
    /// A primera página da qual dados podem ser lidos.
    /// </summary>
    public const int PRIMEIRA_PAGINA = 1;

    /// <summary>
    /// A última página da qual dados podem ser lidos.
    /// </summary>
    public int UltimaPagina { get; private set; }

    /// <summary>
    /// A quantidade de dados armazenadas no repositório.
    /// </summary>
    /// <remarks>
    /// Necessário para calcular a <see cref="UltimaPagina"/>.
    /// </remarks>
    public int TamanhoRepositorio
    {
        get => tamanhoRepositorio;
        set
        {
            tamanhoRepositorio = value;
            var qtdPaginasNecessarias = tamanhoRepositorio / (double)Quantidade;
            var ultimaPagina = Math.Ceiling(qtdPaginasNecessarias);
            UltimaPagina = Convert.ToInt32(ultimaPagina);
        }
    }

    /// <summary>
    /// A página de onde os dados estão sendo lidos.
    /// </summary>
    public int PaginaAtual { get; private set; }

    /// <summary>
    /// A quantidade de dados lidos por paginação.
    /// </summary>
    public int Quantidade { get; private set; }

    /// <summary>
    /// Instancia um novo paginador.
    /// </summary>
    /// <param name="tamanhoRepositorio"> A quantidade de dados presente num repositório. </param>
    /// <param name="quantidade"> A quantidade de dados para buscar em cada paginação. </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Paginador(int tamanhoRepositorio, int quantidade)
    {
        if (int.IsNegative(tamanhoRepositorio))
        {
            var msg = new StringBuilder()
                .AppendFormat("O parâmetro {0} não pode ser negativo.", nameof(tamanhoRepositorio))
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

        Quantidade = quantidade;
        TamanhoRepositorio = tamanhoRepositorio;
        PaginaAtual = PRIMEIRA_PAGINA;
    }

    /// <summary>
    /// Produz a página atual.
    /// </summary>
    /// <returns>
    /// Um <see cref="Pagina"/> que contém o número da página atual e a quantidade de dados que são lidos por paginação.
    /// </returns>
    public Pagina Atual()
    {
        return new Pagina(PaginaAtual, Quantidade);
    }

    /// <summary>   
    /// Começa a ler dados a partir da próxima página.
    /// </summary>
    public void Avancar()
    {
        if (PaginaAtual < UltimaPagina)
        {
            PaginaAtual++;
        }
    }

    /// <summary>
    /// Começa a ler dados a partir da página anterior.
    /// </summary>
    public void Voltar()
    {
        if (PaginaAtual > PRIMEIRA_PAGINA)
        {
            PaginaAtual--;
        }
    }
}

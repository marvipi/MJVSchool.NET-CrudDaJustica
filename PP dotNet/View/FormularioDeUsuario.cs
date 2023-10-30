using System.Globalization;

namespace PP_dotNet.View;

/// <summary>
/// Representa um formulário que lê dados sobre usuários do console.
/// </summary>
public class FormularioDeUsuario
{
    private Campo nome;
    private Campo dataDeNascimento;
    private readonly CultureInfo culturaLocal;
    private readonly Queue<Campo> campos;


    /// <summary>
    /// Indica se todos os campos obrigatórios foram preenchidos.
    /// </summary>
    public bool Preenchido => !campos.Any();

    /// <summary>
    /// Cria um novo formulário de usuário.
    /// </summary>
    /// <param name="culturaLocal"> A cultura do sistema operacional no qual este programa está rodando. </param>
    public FormularioDeUsuario(CultureInfo culturaLocal)
    {
        this.culturaLocal = culturaLocal;

        nome = new Campo("Nome");

        var tituloDataDeNascimento = string.Format("Data de nascimento ({0})", culturaLocal.DateTimeFormat.ShortDatePattern);
        dataDeNascimento = new Campo(tituloDataDeNascimento);

        campos = new();
        campos.Enqueue(nome);
        campos.Enqueue(dataDeNascimento);
    }

    /// <summary>
    /// Lista todos os campos deste formulário no console.
    /// </summary>
    /// <returns>
    /// Um <see cref="System.Collections.Queue"/> que contém as coordenadas do console onde a entrada do usuário será exibida.
    /// </returns>
    public Queue<(int, int)> ListarCampos()
    {
        var coordenadas = new Queue<(int, int)>();
        foreach (var campo in campos)
        {
            Console.Write("{0}: ", campo.Titulo);
            coordenadas.Enqueue(Console.GetCursorPosition());
            Console.WriteLine();
        }

        return coordenadas;
    }

    /// <summary>
    /// Lê o próximo campo deste formulário.
    /// </summary>
    public void LerProximo()
    {
        campos.Peek().Ler();
        campos.Dequeue();
    }

    /// <summary>
    /// Salva os dados lidos do console.
    /// </summary>
    /// <returns>
    /// Um <see cref="UsuarioViewModel"/> que contém todos os dados lidos do console.
    /// </returns>
    public UsuarioViewModel Salvar() // TODO Validar dados
    {
        DateOnly dataValida;
        var formatoLocal = culturaLocal.DateTimeFormat.ShortDatePattern;
        DateOnly.TryParseExact(dataDeNascimento.Valor, formatoLocal, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataValida);
        return new UsuarioViewModel(nome.Valor, dataValida, dataValida); // TODO Refatorar gambiarra
    }
}

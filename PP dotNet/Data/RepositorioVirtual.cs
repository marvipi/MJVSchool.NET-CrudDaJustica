using PP_dotNet.Model;

namespace PP_dotNet.Data;

/// <summary>
/// Representa um repositório de dados que é apagado quando o programa for encerrado.
/// </summary>
public class RepositorioVirtual : IRepositorio
{
    private Usuario[] usuarios;

    public int QtdUsuarios => UltimoIndicePreenchido(usuarios) + 1;

    /// <summary>
    /// Instancia um novo repositório temporário.
    /// </summary>
    /// <remarks>
    /// Todos os dados serão perdidos quando o sistema for encerrado.
    /// </remarks>
    public RepositorioVirtual()
    {
        usuarios = new Usuario[10];

        // TODO Remover após fase de testes
        foreach (var i in Enumerable.Range(0, 25 + 1))
        {
            CadastrarUsuario(i.ToString(), DateOnly.FromDateTime(DateTime.Now));
        }
    }

    public void CadastrarUsuario(string nome, DateOnly dataDeNascimento)
    {
        if (QtdUsuarios == usuarios.Length)
        {
            Array.Resize(ref usuarios, usuarios.Length * 2);
        }

        var novoUsuario = new Usuario(nome, dataDeNascimento);
        var primeiroEspacoVazio = UltimoIndicePreenchido(usuarios) + 1;
        usuarios[primeiroEspacoVazio] = novoUsuario;
    }

    public IEnumerable<Usuario> PegarUsuarios(Pagina pagina)
    {
        var pular = (pagina.Numero - 1) * pagina.Quantidade;
        var pegar = pagina.Numero * pagina.Quantidade;
        var paginaDeUsuarios = usuarios[pular..pegar];

        var qtdNaoNulos = UltimoIndicePreenchido(paginaDeUsuarios) + 1;
        var paginaSemNulos = paginaDeUsuarios[..qtdNaoNulos];
        return paginaSemNulos;
    }

    private int UltimoIndicePreenchido(Usuario[] usuarios)
    {
        var primeroIndiceVazio = Array.IndexOf(usuarios, null);
        return int.IsNegative(primeroIndiceVazio)
            ? usuarios.Length - 1
            : primeroIndiceVazio - 1;
    }

    public void DeletarUsuario(Pagina pagina, int linha)
    {
        var pular = (pagina.Numero - 1) * pagina.Quantidade + linha;
        var qtdAteUltimoIndice = usuarios.Length - 1 - pular;

        foreach (var i in Enumerable.Range(pular, qtdAteUltimoIndice))
        {
            usuarios[i] = usuarios[i + 1];
        }
    }

    public void AtualizarUsuario(Pagina pagina, int linha, string nome, DateOnly dataDeNascimento)
    {
        var indiceDoUsuario = (pagina.Numero - 1) * pagina.Quantidade + linha;
        var usuario = usuarios[indiceDoUsuario];
        usuario.Nome = nome;
        usuario.DataDeNascimento = dataDeNascimento;
    }
}

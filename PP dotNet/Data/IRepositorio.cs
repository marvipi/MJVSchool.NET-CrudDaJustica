using PP_dotNet.Model;

namespace PP_dotNet.Data;

/// <summary>
/// Representa um repositório onde os dados do sistema são armazenados.
/// </summary>
public interface IRepositorio
{
    /// <summary>
    /// A quantidade de usuários registrados neste repositório.
    /// </summary>
    public int QtdUsuarios { get; }

    /// <summary>
    /// Cadastra um novo usuário no sistema.
    /// </summary>
    /// <param name="nome"> O nome do novo usuário. </param>
    /// <param name="dataDeNascimento"> A data de nascimento do novo usuário. </param>
    public void CadastrarUsuario(string nome, DateOnly dataDeNascimento);

    /// <summary>
    /// Busca dados sobre os usuários cadastrados no sistema.
    /// </summary>
    /// <param name="pagina"> A página onde os usuários estão cadastrados. </param>
    /// <returns> Os usuários cadastrados na <paramref name="pagina"/>. </returns>
    public IEnumerable<Usuario> PegarUsuarios(Pagina pagina);

    /// <summary>
    /// Deleta um usuário cadastrado no sistema.
    /// </summary>
    /// <param name="pagina"> A página onde o usuário está cadastrado. </param>
    /// <param name="linha"> A linha da <paramref name="pagina"/> onde o usuário está cadastrado. </param>
    public void DeletarUsuario(Pagina pagina, int linha);

    /// <summary>
    /// Atualiza um dos usuários cadastrados no sistema.
    /// </summary>
    /// <param name="pagina"> A página onde o usuário está cadastrado. </param>
    /// <param name="linha"> A linha da <paramref name="pagina"/> onde o usuário está cadastrado. </param>
    /// <param name="nome"> O novo nome que o usuário terá. </param>
    /// <param name="dataDeNascimento"> A nova data de nascimento que o usuário tera. </param>
    public void AtualizarUsuario(Pagina pagina, int linha, string nome, DateOnly dataDeNascimento);
}

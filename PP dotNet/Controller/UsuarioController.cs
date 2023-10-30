using PP_dotNet.Data;
using PP_dotNet.Model;
using PP_dotNet.View;

namespace PP_dotNet.Controller;

/// <summary>
/// Representa um controlador que gere as informações dos usuários.
/// </summary>
public class UsuarioController
{
    private IRepositorio repositorio;

    private Paginador paginador;

    /// <summary>
    /// Instancia um novo controlador de usuário.
    /// </summary>
    /// <param name="repositorio"> O repositório onde os dados do usuário estão armazenados. </param>
    /// <param name="paginador"> O serviço responsável por fazer paginação de dados. </param>
    public UsuarioController(IRepositorio repositorio, Paginador paginador)
    {
        this.repositorio = repositorio;
        this.paginador = paginador;
    }

    /// <summary>
    /// Cadastra um novo usuário no sistema.
    /// </summary>
    /// <param name="nome"> O nome do novo usuário. </param>
    /// <param name="dataDeNascimento"> A data de nascimento do novo usuário. </param>
    public void Cadastrar(string nome, DateOnly dataDeNascimento)
    {
        repositorio.CadastrarUsuario(nome, dataDeNascimento);
        paginador.TamanhoRepositorio = repositorio.QtdUsuarios;
    }

    /// <summary>
    /// Lista informações sobre os usuários cadastrados na página atual.
    /// </summary>
    /// <returns> 
    /// Uma lista que contém informações dos usuários cadastrados na página atual do sistema. 
    /// </returns>
    public IEnumerable<UsuarioViewModel> Listar()
    {
        var pagina = paginador.Atual();
        var usuarios = repositorio.PegarUsuarios(pagina);


        var usuarioViewModels = usuarios
            .Select(usuario => new UsuarioViewModel(usuario.Nome, usuario.DataDeNascimento, usuario.Centenario))
            .ToList();

        return usuarioViewModels;
    }

    /// <summary>
    /// Avança para a próxima página.
    /// </summary>
    /// <remarks>
    /// Não faz nada se a página atual for a última página.
    /// </remarks>
    public void AvancarPagina()
    {
        paginador.Avancar();
    }

    /// <summary>
    /// Volta para a página anterior.
    /// </summary>
    /// <remarks>
    /// Não faz nada se a página atual for a primeira página.
    /// </remarks>
    public void VoltarPagina()
    {
        paginador.Voltar();
    }

    /// <summary>
    /// Remove um usuário do sistema.
    /// </summary>
    /// <param name="linha"> A linha da página atual onde o usuário está cadastrado. </param>
    public void Deletar(int linha)
    {
        repositorio.DeletarUsuario(paginador.Atual(), linha);
        paginador.TamanhoRepositorio = repositorio.QtdUsuarios;
    }

    /// <summary>
    /// Atualiza um dos usuários cadastrados no sistema.
    /// </summary>
    /// <param name="nome"> O novo nome do usuário. </param>
    /// <param name="dataDeNascimento"> A nova data de nascimento do usuário. </param>    
    /// <param name="linha"> A linha da página atual onde o usuário está cadastrado. </param>
    public void Atualizar(string nome, DateOnly dataDeNascimento, int linha)
    {
        repositorio.AtualizarUsuario(paginador.Atual(), linha, nome, dataDeNascimento);
    }
}
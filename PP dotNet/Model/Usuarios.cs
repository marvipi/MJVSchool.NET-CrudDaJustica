using PP_dotNet.Data;
using System.Text;

namespace PP_dotNet.Model;

/*/// <summary>
/// Contém todos os usuários cadastrados no sistema.
/// </summary>
public class Usuarios
{
    private Usuario[] usuarios;

    /// <summary>
    /// Todos os usuários cadastrados na página atual.
    /// </summary>
    public Usuario[] Pagina => usuarios[..UltimoIndicePreenchido()];

    /// <summary>
    /// A quantidade de usuários cadastrados neste sistema.
    /// </summary>
    public int Tamanho => UltimoIndicePreenchido() + 1;
    public Usuarios()
    {
        usuarios = new Usuario[10];

        // TODO Remover após fase de testes
        foreach (var i in Enumerable.Range(0, 27))
        {
            var usuario = new Usuario(i.ToString(), DateOnly.FromDateTime(DateTime.Now));
            Cadastrar(usuario);
        }
    }

    private Usuarios(Usuario[] usuarios)
    {
        this.usuarios = usuarios;
    }
}
*/
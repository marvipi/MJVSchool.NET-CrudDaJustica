using PP_dotNet.View;
using System.Collections.Generic;

namespace PP_dotNet.Services;

/// <summary>
/// Representa um serviço que desenha a interface de usuário no console.
/// </summary>
public class EscritorDeConsole
{
    private readonly Cabecalho cabecalho;

    // O caminho da primeira exibição.
    private string raiz;

    /// <summary>
    /// Instancia um novo escritor de console.
    /// </summary>
    /// <param name="cabecalho"> Cabeçalho cujas informações sempre serão exibidas no console. </param>
    public EscritorDeConsole(Cabecalho cabecalho)
    {
        this.cabecalho = cabecalho;
        raiz = cabecalho.Caminho.Peek();
    }

    /// <summary>
    /// Exibe o cabeçalho e uma sequência de instruções no console.
    /// </summary>
    /// <param name="instrucoes"> As instruções que serão exibidas. </param>
    public void Exibir(Instrucoes instrucoes)
    {
        Console.CursorVisible = true;
        ExibirCabecalho();
        ExibirInstrucoes(instrucoes);
    }

    /// <summary>
    /// Seleciona o usuário sendo exibido numa linha específica.
    /// </summary>
    /// <param name="instrucoes"> As instruções sendo exibidas. </param>
    /// <param name="usuarios"> Os usuários sendo exibidos. </param>
    /// <param name="linha"> A linha da exibição onde o usuário se encontra. </param>
    public void Selecionar(Instrucoes instrucoes, IEnumerable<UsuarioViewModel> usuarios, int linha)
    {
        var primeiraLinha = Listar(instrucoes, usuarios);
        Console.SetCursorPosition(0, primeiraLinha + linha);
        Console.Write(">");
    }

    /// <summary>
    /// Lista informações sobre usuários cadastrados no sistema.
    /// </summary>
    /// <param name="usuarios"> Uma coleção de informações sobre os usuários cadastrados no sistema. </param>
    public void Exibir(Instrucoes instrucoes, IEnumerable<UsuarioViewModel> usuarios)
    {
        Listar(instrucoes, usuarios);
    }

    // Retorna
    private int Listar(Instrucoes instrucoes, IEnumerable<UsuarioViewModel> usuarios)
    {
        Console.CursorVisible = true;
        ExibirCabecalho();
        ExibirInstrucoes(instrucoes);
        Console.CursorVisible = false;

        var primeiraLinha = Console.GetCursorPosition().Top;
        foreach (var usuario in usuarios)
        {
            Console.WriteLine("  {0}      {1}     {2}", // TODO tabular corretamente
                usuario.Nome,
                usuario.DataDeNascimento,
                usuario.Centenario);
        }

        Console.SetCursorPosition(0, primeiraLinha + primeiraLinha);
        Console.Write(">");

        return primeiraLinha;
    }


    /// <summary>
    /// Adiciona o nome de uma exibição ao final do caminho atual.
    /// </summary>
    /// <remarks>
    /// Não faz nada se esta exibição já estiver no final do caminho.
    /// </remarks>
    /// <param name="exibicao"> O nome da exibição que será adicionada. </param>
    public void AvancarCaminho(string exibicao)
    {
        if (cabecalho.Caminho.Peek() != exibicao)
        {
            cabecalho.Caminho.Push(string.Format("/{0}", exibicao));
        }
    }

    /// <summary>
    /// Remove o último item do caminho.
    /// </summary>
    /// <remarks>
    /// Não reverte se o usuário estiver na página inicial.
    /// </remarks>
    public void ReverterCaminho()
    {
        if (cabecalho.Caminho.Peek() != raiz)
        {
            cabecalho.Caminho.Pop();
        }
    }

    private void ExibirCabecalho()
    {
        Console.Clear();
        Console.WriteLine(cabecalho);
    }

    private void ExibirInstrucoes(Instrucoes instrucoes)
    {
        Console.WriteLine(instrucoes);
    }
}

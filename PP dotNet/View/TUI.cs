using PP_dotNet.Controller;
using PP_dotNet.Services;
using System.Globalization;

namespace PP_dotNet.View;

/// <summary>
/// Representa uma interface de linha de comando pela qual o usuário interage com o sistema.
/// </summary>
public class TUI
{
    private readonly UsuarioController usuarioController;
    private readonly EscritorDeConsole escritorDeConsole;
    private readonly CultureInfo culturaLocal;

    /// <summary>
    /// Instancia uma nova interface de linha de comando.
    /// </summary>
    /// <param name="usuarioController"> O controlador responsável pelos usuários do sistema. </param>
    /// <param name="escritorDeConsole"> O serviço responsável por desenhar a interface de linha de comando. </param>
    /// <param name="culturaLocal"> Informações sobre a cultura do sistema na qual esta interface será exibida. </param>
    public TUI(UsuarioController usuarioController,
        EscritorDeConsole escritorDeConsole,
        CultureInfo culturaLocal)
    {
        this.usuarioController = usuarioController;
        this.escritorDeConsole = escritorDeConsole;
        this.culturaLocal = culturaLocal;
    }

    /// <summary>
    /// Exibe a interface de linha de comando.
    /// </summary>
    public void Iniciar()
    {
        Inicio();
        Environment.Exit(0);
    }

    // O ponto de entrada para todas as outras exibições.
    private void Inicio()
    {
        var dictInstrucoes = new Dictionary<ConsoleKey, string>()
        {
            {ConsoleKey.Escape, "Sair"},
            {ConsoleKey.H, "Herois" },
            {ConsoleKey.V, "Viloes" },
        };
        var instrucoes = new Instrucoes(dictInstrucoes, 5);

        while (true)
        {
            escritorDeConsole.Exibir(instrucoes);

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.H:
                    Herois();
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }

    private void Herois()
    {
        escritorDeConsole.AvancarCaminho(nameof(Herois).ToUpper());

        var dictInstrucoes = new Dictionary<ConsoleKey, string>()
        {
            {ConsoleKey.Escape, "Voltar"},
            {ConsoleKey.C, "Cadastrar" },
            {ConsoleKey.A, "Atualizar" },
            {ConsoleKey.D, "Deletar" },
        };
        var instrucoes = new Instrucoes(dictInstrucoes, 5);

        while (true)
        {
            var listagemDeUsuarios = usuarioController.Listar();
            escritorDeConsole.Exibir(instrucoes, listagemDeUsuarios);

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.C:
                    Cadastrar();
                    break;

                case ConsoleKey.A:
                    Atualizar();
                    break;

                case ConsoleKey.D:
                    Deletar();
                    break;

                case ConsoleKey.RightArrow:
                    usuarioController.AvancarPagina();
                    break;

                case ConsoleKey.LeftArrow:
                    usuarioController.VoltarPagina();
                    break;

                case ConsoleKey.Escape:
                    escritorDeConsole.ReverterCaminho();
                    return;
            }
        }
    }

    private void Cadastrar()
    {
        escritorDeConsole.AvancarCaminho(nameof(Cadastrar).ToUpper());

        var dictInstrucoes = new Dictionary<ConsoleKey, string>()
        {
            {ConsoleKey.Escape, "Voltar"},
            {ConsoleKey.Enter, "Confirmar"},
        };
        var instrucoes = new Instrucoes(dictInstrucoes, 5);

        while (true)
        {

            var novoUsuario = escritorDeConsole.Exibir(instrucoes, new FormularioDeUsuario(culturaLocal));

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Enter:
                    usuarioController.Cadastrar(novoUsuario.Nome, novoUsuario.DataDeNascimento);
                    return;

                case ConsoleKey.Escape:
                    escritorDeConsole.ReverterCaminho();
                    return;

            }
        }
    }

    private void Deletar()
    {
        escritorDeConsole.AvancarCaminho(nameof(Deletar).ToUpper());

        var dictInstrucoes = new Dictionary<ConsoleKey, string>()
        {
            {ConsoleKey.Escape, "Cancelar"},
            {ConsoleKey.UpArrow, "Cima"},
            {ConsoleKey.DownArrow, "Baixo"},
            {ConsoleKey.D, "Deletar"},
        };

        var instrucoes = new Instrucoes(dictInstrucoes, 5);
        var posicao = 0;

        while (true)
        {
            var listagemDeUsuarios = usuarioController.Listar();

            if (!listagemDeUsuarios.Any())
            {
                Console.Beep(1000, 100);
                escritorDeConsole.ReverterCaminho();
                return;
            }

            posicao = listagemDeUsuarios.Count() < posicao
                ? listagemDeUsuarios.Count()
                : posicao;

            escritorDeConsole.Selecionar(instrucoes, listagemDeUsuarios, posicao);

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    posicao = posicao > 0 ? posicao - 1 : posicao;
                    break;

                case ConsoleKey.DownArrow:
                    posicao = posicao < listagemDeUsuarios.Count() - 1 ? posicao + 1 : posicao;
                    break;

                case ConsoleKey.RightArrow:
                    usuarioController.AvancarPagina();
                    listagemDeUsuarios = usuarioController.Listar();
                    posicao = listagemDeUsuarios.Count() < posicao
                        ? listagemDeUsuarios.Count() - 1
                        : posicao;
                    break;

                case ConsoleKey.LeftArrow:
                    usuarioController.VoltarPagina();
                    break;

                case ConsoleKey.D:
                    usuarioController.Deletar(posicao);
                    listagemDeUsuarios = usuarioController.Listar();
                    if (!listagemDeUsuarios.Any())
                    {
                        usuarioController.VoltarPagina();
                    }
                    posicao = listagemDeUsuarios.Count() <= posicao
                        ? listagemDeUsuarios.Count() - 1
                        : posicao;
                    break;

                case ConsoleKey.Escape:
                    escritorDeConsole.ReverterCaminho();
                    return;
            }
        }
    }

    private void Atualizar()
    {
        escritorDeConsole.AvancarCaminho(nameof(Atualizar));

        var dictInstrucoes = new Dictionary<ConsoleKey, string>()
        {
            {ConsoleKey.Escape, "Cancelar"},
            {ConsoleKey.UpArrow, "Cima"},
            {ConsoleKey.DownArrow, "Baixo"},
            {ConsoleKey.A, "Atualizar"},
        };
        var instrucoes = new Instrucoes(dictInstrucoes, 5);
        var posicao = 0;

        while (true)
        {
            var listagemDeUsuarios = usuarioController.Listar();
            posicao = listagemDeUsuarios.Count() < posicao
                ? listagemDeUsuarios.Count()
                : posicao;

            escritorDeConsole.Selecionar(instrucoes, listagemDeUsuarios, posicao);

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    posicao = posicao > 0 ? posicao - 1 : posicao;
                    break;

                case ConsoleKey.DownArrow:
                    posicao = posicao < listagemDeUsuarios.Count() - 1 ? posicao + 1 : posicao;
                    break;

                case ConsoleKey.RightArrow:
                    usuarioController.AvancarPagina();
                    listagemDeUsuarios = usuarioController.Listar();
                    posicao = listagemDeUsuarios.Count() < posicao
                        ? listagemDeUsuarios.Count() - 1
                        : posicao;
                    break;

                case ConsoleKey.LeftArrow:
                    usuarioController.VoltarPagina();
                    break;

                case ConsoleKey.A:
                    var dados = escritorDeConsole.Exibir(instrucoes, new FormularioDeUsuario(culturaLocal));
                    usuarioController.Atualizar(dados.Nome, dados.DataDeNascimento, posicao);
                    break;

                case ConsoleKey.Escape:
                    escritorDeConsole.ReverterCaminho();
                    return;
            }
        }
    }
}

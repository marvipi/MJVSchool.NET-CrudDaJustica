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
    private readonly Stack<string> path;

    // Summary: Transforms the path into a string, separating each view with a forward slash.
    // Returns: A string in the form FIRST_VIEW/SECOND_VIEW/THIRD_VIEW...
    private string PathToString() => path
        .Aggregate((s, r) => r + "/" + s);

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
        path = new();
    }

    /// <summary>
    /// Exibe a interface de linha de comando.
    /// </summary>
    public void Iniciar()
    {
        Home();
        Environment.Exit(0);
    }

    private void Exit() => Environment.Exit(0);

    // O ponto de entrada para todas as outras exibições.
    private void Home()
    {
        path.Push(nameof(Home).ToUpper());
        var inicioView = new ViewBuilder()
            .AppendLine("USER::ADMIN: J'ONN J'ONNZ")
            .AppendLine(PathToString())
            .Add(new BoundKeyMap(Exit, ConsoleKey.Escape, "ESC: EXIT"))
            .Add(new BoundKeyMap(Heroes, ConsoleKey.H, "H: HEROES"))
            .Add(new BoundKeyMap(Villains, ConsoleKey.V, "V: VILLAINS"))
            .Build();

        while (true)
        {
            inicioView.Display();
            inicioView.Invoke(Console.ReadKey(true).Key);
        }
    }

    private void Villains() => Exit();

    private void Heroes()
    {
        path.Push(nameof(Heroes).ToUpper());

        var header = new ViewBuilder()
            .AppendLine("USER::ADMIN: J'ONN J'ONNZ")
            .AppendLine(PathToString())
            .Add(new UnboundKeyMap(ConsoleKey.Escape, "ESC: Back"))
            .Add(new BoundKeyMap(Create, ConsoleKey.C, "C: Create"))
            .Add(new BoundKeyMap(Update, ConsoleKey.U, "U: Update"))
            .Add(new BoundKeyMap(Delete, ConsoleKey.D, "D: Delete"))
            .Build();

        var nextPage = new BoundKeyMap(usuarioController.AvancarPagina, ConsoleKey.RightArrow, "->: Next page");
        var prevPage = new BoundKeyMap(usuarioController.VoltarPagina, ConsoleKey.LeftArrow, "<-: Previous page");
        var nextElem = new UnboundKeyMap(ConsoleKey.DownArrow, "\\/: Next element");
        var prevElem = new UnboundKeyMap(ConsoleKey.UpArrow, "/\\: Previous element");
        var listing = new Listing<UsuarioViewModel>(usuarioController.Listar, nextPage, prevPage, nextElem, prevElem);

        while (true)
        {
            header.Display();
            listing.Display();

            var input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Escape)
            {
                path.Pop();
                return;
            }

            header.Invoke(input);
            listing.Invoke(input);
        }
    }

    private void Create()
    {
        path.Push(nameof(Create).ToUpper());

        var header = new ViewBuilder()
            .AppendLine("USER::ADMIN: J'ONN J'ONNZ")
            .AppendLine(PathToString())
            .Add(new UnboundKeyMap(ConsoleKey.Escape, "ENTER: Next"))
            .Build();

        var form = new FormBuilder(" {0}: ")
            .Append(new Field("Nome"))
            .Append(new Field("Data de nascimento"))
            .Add(new UnboundKeyMap(ConsoleKey.Escape, "ESC: Cancel"))
            .Add(new UnboundKeyMap(ConsoleKey.End, "ENTER: Save"))
            .Build();

        header.Display();

        var formData = form.Display();

        while (true)
        {
            var input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Escape)
            {
                path.Pop();
                return;
            }

            if (input == ConsoleKey.Enter)
            {
                var nome = formData.Data["Nome"] ?? "N/A"; // TODO Validar
                var dataNasc = formData.Data["Data de nascimento"]; // TODO Validar
                DateOnly.TryParseExact(dataNasc, culturaLocal.DateTimeFormat.ShortDatePattern, culturaLocal, DateTimeStyles.None, out var dataValida);
                usuarioController.Cadastrar(nome, dataValida);
                path.Pop();
                return;
            }

            Console.Beep(800, 200);
        }
    }

    private void Delete()
    {
        path.Push(nameof(Delete).ToUpper());

        var heroDeleteView = new ViewBuilder()
            .AppendLine("USER::ADMIN: J'ONN J'ONNZ")
            .AppendLine(PathToString())
            .Add(new UnboundKeyMap(ConsoleKey.Escape, "ESC: Back"))
            .Add(new UnboundKeyMap(ConsoleKey.D, "D: Delete"))
            .Build();

        var nextPage = new BoundKeyMap(usuarioController.AvancarPagina, ConsoleKey.RightArrow, "->: Next page");
        var prevPage = new BoundKeyMap(usuarioController.VoltarPagina, ConsoleKey.LeftArrow, "<-: Previous page");
        var nextElem = new UnboundKeyMap(ConsoleKey.DownArrow, "\\/: Next element");
        var prevElem = new UnboundKeyMap(ConsoleKey.UpArrow, "/\\: Previous element");
        var listing = new Listing<UsuarioViewModel>(usuarioController.Listar, nextPage, prevPage, nextElem, prevElem);

        while (true)
        {
            heroDeleteView.Display();
            listing.Display();

            var input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.Escape)
            {
                path.Pop();
                return;
            }

            if (input == ConsoleKey.D)
            {
                break;
            }

            heroDeleteView.Invoke(input);
            listing.Invoke(input);
        }

        Console.Clear();

        while (true)
        {
            heroDeleteView.Display();
            listing.Display();

            var input = Console.ReadKey(true).Key;
            listing.Invoke(input);

            if (input == ConsoleKey.D)
            {
                usuarioController.Deletar(listing.CurrentlySelectedElement);
            }

            if (input == ConsoleKey.Escape)
            {
                path.Pop();
                return;
            }
        }
    }

    private void Update()
    {
        path.Push(nameof(Update).ToUpper());

        var form = new FormBuilder(" {0}: ")
            .Append(new Field("Nome"))
            .Append(new Field("Data de nascimento"))
            .Add(new UnboundKeyMap(ConsoleKey.Escape, "ESC: Cancel"))
            .Add(new UnboundKeyMap(ConsoleKey.Enter, "ENTER: Next"))
            .Build();

        var header = new ViewBuilder()
            .AppendLine("USER::ADMIN: J'ONN J'ONNZ")
            .AppendLine(PathToString())
            .Add(new UnboundKeyMap(ConsoleKey.Escape, "ESC: Back"))
            .Add(new UnboundKeyMap(ConsoleKey.U, "U: Update"))
            .Build();

        var nextPage = new BoundKeyMap(usuarioController.AvancarPagina, ConsoleKey.RightArrow, "->: Next page");
        var prevPage = new BoundKeyMap(usuarioController.VoltarPagina, ConsoleKey.LeftArrow, "<-: Previous page");
        var nextElem = new UnboundKeyMap(ConsoleKey.DownArrow, "\\/: Next element");
        var prevElem = new UnboundKeyMap(ConsoleKey.UpArrow, "/\\: Previous element");
        var listing = new Listing<UsuarioViewModel>(usuarioController.Listar, nextPage, prevPage, nextElem, prevElem);
        while (true)
        {
            header.Display();
            listing.Display();

            var input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.Escape)
            {
                path.Pop();
                return;
            }

            if (input == ConsoleKey.U)
            {
                break;
            }

            header.Invoke(input);
            listing.Invoke(input);
        }

        header.Display();
        var formData = form.Display();
        while (true)
        {
            var input = Console.ReadKey().Key;
            if (input == ConsoleKey.Escape)
            {
                path.Pop();
                return;
            }

            if (input == ConsoleKey.Enter)
            {
                var nome = formData.Data["Nome"] ?? "N/A"; // TODO Validar
                var dataNasc = formData.Data["Data de nascimento"]; // TODO Validar
                DateOnly.TryParseExact(dataNasc, culturaLocal.DateTimeFormat.ShortDatePattern, culturaLocal, DateTimeStyles.None, out var dataValida);
                usuarioController.Atualizar(nome, dataValida, listing.CurrentlySelectedElement);
                path.Pop();
                return;
            }

            Console.Beep(800, 200);
        }
    }
}

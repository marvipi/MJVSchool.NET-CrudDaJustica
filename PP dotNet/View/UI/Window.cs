using PP_dotNet.View.Keybindings;

namespace PP_dotNet.View.UI;

/// <summary>
/// Represents a window in the user interface.
/// </summary>
public abstract class Window
{
    protected readonly List<Keybinding> KeyBindings; // TODO Change to data type that doesn't accept duplicates.
    protected bool ExitKeyPressed { get; set; }
    protected Header? Header { get; init; }

    protected Window(RebindableKey exitKey, IEnumerable<Keybinding> keybindings, Header header)
    {
        KeyBindings = keybindings.ToList();
        KeyBindings.Insert(0, exitKey.Bind(Exit));
        ExitKeyPressed = false;
        Header = header;
    }

    protected Window(RebindableKey exitKey, IEnumerable<Keybinding> keybindings)
    {
        KeyBindings = keybindings.ToList();
        KeyBindings.Insert(0, exitKey.Bind(Exit));
        ExitKeyPressed = false;
    }

    protected Window(RebindableKey exitKey)
    {
        KeyBindings = new();
        KeyBindings.Insert(0, exitKey.Bind(Exit));
        ExitKeyPressed = false;
    }

    /// <summary>
    /// Displays the current window in the console.
    /// </summary>
    public abstract void Display();

    protected void DisplayKeybindings()
    {
        DisplayKeybindings(KeyBindings);
    }

    protected void DisplayKeybindings(IEnumerable<Keybinding> keybindings)
    {
        foreach (var keyMap in keybindings)
        {
            Console.Write(keyMap);
            Console.Write("\t");
        }

        Console.WriteLine();
    }

    protected void DisplayKeybindings(IEnumerable<Keybinding<int>> keybindings)
    {
        foreach (var keyMap in keybindings)
        {
            Console.Write(keyMap);
            Console.Write("\t");
        }

        Console.WriteLine();
    }

    protected void Invoke(ConsoleKey key)
    {
        Invoke(key, KeyBindings);
    }

    protected void Invoke(ConsoleKey key, IEnumerable<Keybinding> keybindings)
    {
        keybindings
            .FirstOrDefault(kb => kb.Key == key)
            ?.Invoke();
    }

    protected void Exit() => ExitKeyPressed = true;
}

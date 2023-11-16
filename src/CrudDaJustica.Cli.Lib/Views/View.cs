using CrudDaJustica.Cli.Lib.Keybindings;

namespace CrudDaJustica.Cli.Lib.Views;

/// <summary>
/// Represents a window in the user interface.
/// </summary>
public abstract class View
{
    // Summary: Keyboard keys used to invoke events.
    protected readonly List<Keybinding> Keybindings; // TODO Change to data type that doesn't accept duplicates.

    // Summary: Indicates whether a view should terminate its display method.
    // Remarks: It's up to the subclasses whether to bind a key to the exit method.
    protected bool ExitKeyPressed { get; set; }

    protected View()
    {
        Keybindings = new();
        ExitKeyPressed = false;
    }

    /// <summary>
    /// Disables the console cursor.
    /// </summary>
    public virtual void Display()
    {
        Console.CursorVisible = false;
    }

    // Summary: Invokes the first keybinding associated with a given key, if any exist.
    protected void Invoke(ConsoleKey key)
    {
        Keybindings
            .FirstOrDefault(kb => kb.Key == key)
            ?.Invoke();
    }

    // Summary: Signals the view that it's time to exit.
    protected void Exit() => ExitKeyPressed = true;
}

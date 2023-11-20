namespace CrudDaJustica.Cli.Lib.Window;

/// <summary>
/// Represents the mapping of a <see cref="ConsoleKey"/> to an <see cref="Action"/>.
/// </summary>
public class Keybinding
{
    // Summary: The text of this keybinding to display in a console window.
    private readonly string displayText;

    // Summary: The action to call when this keybinding is invoked.
    private readonly Action action;

    /// <summary>
    /// A console key that triggers this keybinding's action.
    /// </summary>
    public ConsoleKey Key { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Keybinding"/> class.
    /// </summary>
    /// <param name="key"> A console key to map to an <see cref="Action"/>. </param>
    /// <param name="action"> The action to call when this keybinding is invoked. </param>
    /// <param name="displayText"> The text representation of this keybinding to display in a console window. </param>
    public Keybinding(ConsoleKey key, Action action, string displayText)
    {
        Key = key;
        this.action = action;
        this.displayText = displayText;
    }

    /// <summary>
    /// Invokes the <see cref="Action"/> bound to this keybinding.
    /// </summary>
    public void Invoke() => action.Invoke();

    public override string ToString() => displayText;
}
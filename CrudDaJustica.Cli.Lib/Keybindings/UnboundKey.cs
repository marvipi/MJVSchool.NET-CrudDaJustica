namespace CrudDaJustica.Cli.Lib.Keybindings;

/// <summary>
/// Represents a <see cref="ConsoleKey"/> that can be displayed in the console window.
/// </summary>
public abstract class UnboundKey
{
    // Summary: The text to show in the console window.
    private readonly string displayText;

    /// <summary>
    /// A console key that can be displayed in the console window.
    /// </summary>
    public ConsoleKey Key { get; init; }

    protected UnboundKey(ConsoleKey key, string displayText)
    {
        Key = key;
        this.displayText = displayText;
    }

    /// <summary>
    /// Produces a string that represents this <see cref="Key"/>.
    /// </summary>
    /// <returns>
    /// A string that represents this <see cref="Key"/>.
    /// </returns>
    public override string ToString() => displayText;

}
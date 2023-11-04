namespace PP_dotNet.View;

/// <summary>
/// Represents a mapping of a keyboard key to an <see cref="Action"/>.
/// </summary>
public abstract class KeyMap
{
    private readonly string displayText;

    /// <summary>
    /// A keyboard key mapped that can be mapped to an <see cref="Action"/>.
    /// </summary>
    public ConsoleKey Key { get; init; }

    protected KeyMap(ConsoleKey key, string displayText)
    {
        Key = key;
        this.displayText = displayText;
    }

    public override string ToString()
    {
        return displayText;
    }
}
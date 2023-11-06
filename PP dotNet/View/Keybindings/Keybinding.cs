namespace PP_dotNet.View.Keybindings;

/// <summary>
/// Represents the mapping of a <see cref="ConsoleKey"/> to an <see cref="Action"/>.
/// </summary>
public class Keybinding : UnboundKey
{
    // Summary: The action to call when this keybinding is invoked.
    private readonly Action action;

    /// <summary>
    /// Initializes a new instance of the <see cref="Keybinding"/> class.
    /// </summary>
    /// <param name="action"> The action to call when this <see cref="Keybinding"/> is invoked. </param>
    /// <param name="key"> A console key to map to a <see cref="Action"/>. </param>
    /// <param name="displayText"> A text representation of this <see cref="Keybinding"/> to display on the user interface. </param>
    public Keybinding(Action action, ConsoleKey key, string displayText) : base(key, displayText)
    {
        this.action = action;
    }

    /// <summary>
    /// Invokes the <see cref="Action"/> bound to this <see cref="Keybinding"/>.
    /// </summary>
    public void Invoke() => action.Invoke();
}
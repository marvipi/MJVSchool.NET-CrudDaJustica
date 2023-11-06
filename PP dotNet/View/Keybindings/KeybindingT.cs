namespace PP_dotNet.View.Keybindings;

/// <summary>
/// Represents a mapping of a <see cref="ConsoleKey"/> to an <see cref="Action{T}"/>.
/// </summary>
/// <typeparam name="T"> The type parameter of the bound action. </typeparam>
public class Keybinding<T> : UnboundKey
{
    // Summary: The action to call when this keybinding is invoked.
    private readonly Action<T> action;

    /// <summary>
    /// Initializes a new instance of the <see cref="Keybinding{T}"/> class.
    /// </summary>
    /// <param name="action"> The action to call when this <see cref="Keybinding{T}"/> is invoked. </param>
    /// <param name="key"> A console key to map to an <see cref="Action{T}"/>. </param>
    /// <param name="displayText"> A text representation of this <see cref="Keybinding{T}"/> to display on the user interface. </param>
    public Keybinding(Action<T> action, ConsoleKey key, string displayText) : base(key, displayText)
    {
        this.action = action;
    }

    /// <summary>
    /// Invokes the <see cref="Action{T}"/> bound to this <see cref="Keybinding{T}"/>.
    /// </summary>
    /// <param name="arg"> The argument to pass to this keybinding's action. </param>
    public void Invoke(T arg) => action.Invoke(arg);
}
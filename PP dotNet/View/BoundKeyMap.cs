namespace PP_dotNet.View;

/// <summary>
/// Represents a mapping of a keyboard key to an <see cref="Action"/>.
/// </summary>
public class BoundKeyMap : KeyMap
{
    // The action to call when this Key is pressed.
    private readonly Action action;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BoundKeyMap"/> class.
    /// </summary>
    /// <param name="action"> The action to call when this <see cref="BoundKeyMap"/> is invoked. </param>
    /// <param name="key"> A console key to map to an <paramref name="action"/>. </param>
    /// <param name="displayText"> A text representation of this mapping to display on the user interface. </param>
    public BoundKeyMap(Action action, ConsoleKey key, string displayText) : base(key, displayText)
    {
        this.action = action;
    }

    /// <summary>
    /// Invokes the action mapped to this <see cref="Key"/>.
    /// </summary>
    public void Invoke() => action.Invoke();
}
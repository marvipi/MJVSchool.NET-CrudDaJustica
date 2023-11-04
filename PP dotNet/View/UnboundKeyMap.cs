namespace PP_dotNet.View;


/// <summary>
/// Represents a <see cref="ConsoleKey"/> that will later be mapped to an <see cref="Action"/>.
/// </summary>
public class UnboundKeyMap : KeyMap
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnboundKeyMap"/> class.
    /// </summary>
    /// <param name="key"> The keyboard key that will later be bound to an <see cref="Action"/>. </param>
    /// <param name="displayText"> The text to display when this <see cref="UnboundKeyMap"/> is shown on screen. </param>
    public UnboundKeyMap(ConsoleKey key, string displayText) : base(key, displayText) { }

    /// <summary>
    /// Binds this key map's <see cref="ConsoleKey"/> to an <see cref="Action"/>.
    /// </summary>
    /// <param name="action"> The action to bind. </param>
    /// <returns> A new instance of the <see cref="BoundKeyMap"/> class. </returns>
    public BoundKeyMap Bind(Action action) => new(action, Key, ToString());
}

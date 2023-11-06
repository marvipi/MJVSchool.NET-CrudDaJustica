using System.Text;
using PP_dotNet.View.Keybindings;
using PP_dotNet.View.UI;

namespace PP_dotNet.View.Builders;

/// <summary>
/// Represents a view with an arbitrary number of lines.
/// </summary>
public class ViewBuilder
{
    RebindableKey? exitKey;
    private readonly StringBuilder lines;
    private readonly List<Keybinding> keyBindings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewBuilder"/> class.
    /// </summary>
    public ViewBuilder()
    {
        lines = new();
        keyBindings = new();
    }

    /// <summary>
    /// Appends a string at the bottom of the view, anchored to the far-left of the console window.
    /// </summary>
    /// <param name="value"> The string to append. </param>
    /// <returns> A reference to this instance after the append operation has completed. </returns>
    public ViewBuilder AppendLine(string value)
    {
        lines.AppendLine(value);
        return this;
    }

    public ViewBuilder BindKey(ConsoleKey key, string displayText, Action action)
    {
        keyBindings.Add(new Keybinding(action, key, displayText));
        return this;
    }

    public ViewBuilder BindExitKey(ConsoleKey key, string displayText)
    {
        exitKey = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Produces a new <see cref="Header"/>.
    /// </summary>
    /// <returns>
    /// A new <see cref="Header"/> that contains all appended elements.
    /// </returns>
    public Header Build() => new(exitKey, keyBindings, lines);

}

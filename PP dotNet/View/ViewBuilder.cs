using System.Text;

namespace PP_dotNet.View;

/// <summary>
/// Represents a view with an arbitrary number of lines.
/// </summary>
public class ViewBuilder
{
    private readonly StringBuilder lines;
    private readonly List<BoundKeyMap> keyMapping;
    private readonly List<string> keyMappingDisplayTexts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewBuilder"/> class.
    /// </summary>
    public ViewBuilder()
    {
        lines = new();
        keyMapping = new();
        keyMappingDisplayTexts = new();
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

    /// <summary>
    /// Adds a <see cref="BoundKeyMap"/> to the view.
    /// </summary>
    /// <param name="keyMap"> A mapping a keyboard key used to interact with the view. </param>
    /// <returns> A reference to this instance after the append operation has completed. </returns>
    public ViewBuilder Add(BoundKeyMap keyMap)
    {
        keyMapping.Add(keyMap);
        keyMappingDisplayTexts.Add(keyMap.ToString());
        return this;
    }

    /// <summary>
    /// Adds a new <see cref="UnboundKeyMap"/> to the view.
    /// </summary>
    /// <param name="keyMap"> A mapping a keyboard key that will be handled externally. </param>
    /// <returns> A reference to this instance after the append operation has completed. </returns>
    public ViewBuilder Add(UnboundKeyMap keyMap)
    {
        keyMappingDisplayTexts.Add(keyMap.ToString());
        return this;
    }

    /// <summary>
    /// Produces a new <see cref="View"/>.
    /// </summary>
    /// <returns>
    /// A new <see cref="View"/> that contains all appended elements.
    /// </returns>
    public View Build() => new(lines, keyMapping, keyMappingDisplayTexts);
}

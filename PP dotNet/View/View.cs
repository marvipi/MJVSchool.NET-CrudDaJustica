using System.Text;

namespace PP_dotNet.View;

/// <summary>
/// Represents a page used to display information to the user.
/// </summary>
public class View
{
    private readonly StringBuilder lines;
    private readonly IEnumerable<BoundKeyMap> keyMapping;
    private readonly IEnumerable<string> keyMappingDisplayTexts;

    /// <summary>
    /// Initializes a new instance of the <see cref="View"/> class.
    /// </summary>
    /// <param name="lines"> The lines to display. </param>
    /// <param name="keyMapping"> A collection of keybindings used to interact with this <see cref="View"/>. </param>
    /// <param name="keyMappingDisplayTexts"> The string representation of the key maps bound to this <see cref="View"/>. </param>
    public View(StringBuilder lines, IEnumerable<BoundKeyMap> keyMapping, IEnumerable<string> keyMappingDisplayTexts)
    {
        this.lines = lines;
        this.keyMapping = keyMapping;
        this.keyMappingDisplayTexts = keyMappingDisplayTexts;
    }

    /// <summary>
    /// Calls the action associated with a keyboard key.
    /// </summary>
    /// <remarks>
    /// Does nothing if the <paramref name="key"/> is not mapped to this <see cref="View"/>.
    /// </remarks>
    /// <param name="key"> A standard keyboard key. </param>
    public void Invoke(ConsoleKey key)
    {
        var keyMap = keyMapping.FirstOrDefault(keyMap => keyMap.Key == key);
        keyMap?.Invoke();
    }

    /// <summary>
    /// Displays this view in the console.
    /// </summary>
    /// <remarks>
    /// Views are drawn sequentially from the top to the bottom of the console.
    /// </remarks>
    public void Display()
    {
        Console.Clear();
        Console.WriteLine(lines);
        foreach (var keyMap in keyMappingDisplayTexts)
        {
            Console.Write(keyMap);
            Console.Write("\t");
        }
        Console.WriteLine();
    }
}

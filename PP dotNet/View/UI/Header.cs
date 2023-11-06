using System.Text;
using PP_dotNet.View.Keybindings;

namespace PP_dotNet.View.UI;

/// <summary>
/// Represents a view used to display information at the top of the console.
/// </summary>
public class Header : Window
{
    // Summary: A StringBuilder that contains the visual representation of this Header.
    private readonly StringBuilder lines;

    /// <summary>
    /// Initializes a new instance of the <see cref="Header"/> class.
    /// </summary>
    /// <param name="exitKey"> An unbound console key that will cause the program to exit. </param>
    /// <param name="keybindings"> A collection of keybindings used to interact with this <see cref="Header"/>. </param>
    /// <param name="lines"> The lines to display. </param>
    public Header(RebindableKey exitKey, IEnumerable<Keybinding> keybindings, StringBuilder lines) : base(exitKey, keybindings)
    {
        // TODO Remove exit key
        // TODO Remove keybindings
        this.lines = lines;
    }

    public override void Display()
    {
        Console.Clear();
        Console.Write(lines);
        DisplayKeybindings();
    }
}

using System.Text;

namespace CrudDaJustica.CliLib.Views;

/// <summary>
/// Represents an element used to display information at the top of the console.
/// </summary>
public class Header
{
    // Summary: A StringBuilder that contains the visual representation of this Header.
    private readonly StringBuilder content;

    /// <summary>
    /// Initializes a new instance of the <see cref="Header"/> class.
    /// </summary>
    /// <param name="content"> The contents of the header that will be displayed in the user interface. </param>
    public Header(StringBuilder content)
    {
        this.content = content;
    }

    /// <summary>
    /// Displays the contents of the header at the current console cursor position.
    /// </summary>
    public void Display()
    {
        Console.Write(content);
    }
}

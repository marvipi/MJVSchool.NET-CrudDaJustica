namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents static information to display on the upper part of a console window.
/// </summary>
public class Header : HorizontalLineDisplayer, IDisplayable
{
    private const int FIRST_BUFFER_COLUMN = 0;
    private const char SEPARATOR_LEFT_BORDER = '├';
    private const char SEPARATOR_RIGHT_BORDER = '┤';

    private readonly IEnumerable<string> contents;

    public Header(IEnumerable<string> contents)
    {
        this.contents = contents;
    }

    public void Display()
    {
        var row = 1;
        foreach (var content in contents)
        {
            Console.SetCursorPosition(1, row);
            Console.Write(content);
            row++;
        }
        var separator = DrawHorizontalLine(SEPARATOR_LEFT_BORDER, SEPARATOR_RIGHT_BORDER);
        var currentConsoleRow = Console.GetCursorPosition().Top;
        Console.SetCursorPosition(FIRST_BUFFER_COLUMN, currentConsoleRow + 1);
        Console.Write(separator);
    }
}

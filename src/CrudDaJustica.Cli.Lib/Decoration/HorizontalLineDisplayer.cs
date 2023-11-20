namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents a window decoration that draws horizontal lines in the console window.
/// </summary>
public abstract class HorizontalLineDisplayer
{
    protected const int TWO_CORNERS = 2;
    protected const char HORIZONTAL_CHAR = '─';
    protected static int LineWidth => Console.BufferWidth - TWO_CORNERS;

    protected static string DrawHorizontalLine(char leftCorner, char rightCorner)
    {
        var line = string.Empty.PadRight(LineWidth, HORIZONTAL_CHAR);
        var separator = string.Format("{0}{1}{2}", leftCorner, line, rightCorner);

        return separator;
    }
}

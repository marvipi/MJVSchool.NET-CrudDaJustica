namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents the characters used to draw the borders of the console windows.
/// </summary>
internal static class BorderDrawingConstants
{
    public const int AMOUNT_OF_CORNERS = 2;

    public const int FIRST_VERT_BORDER_ROW = 1;
    public const int FIRST_BUFFER_COLUMN = 0;

    public const char HORIZONTAL_CHAR = '─';
    public const char VERTICAL_CHAR = '│';

    public const char LEFT_TITLE_BORDER = '┤';
    public const char RIGHT_TITLE_BORDER = '├';

    public const char DOWNWARD_LEFT_CORNER = '┌';
    public const char DOWNWARD_RIGHT_CORNER = '┐';

    public const char UPWARD_LEFT_CORNER = '└';
    public const char UPWARD_RIGHT_CORNER = '┘';
}

namespace CrudDaJustica.Cli.Lib.Decoration.Header;

/// <summary>
/// Represents a division between a <see cref="Header"/> and the rest of a console window.
/// </summary>
internal class Separator : IDisplayable
{
    public void Display()
    {
        var horizontalLine = string.Empty.PadRight
            (Console.BufferWidth - BorderDrawingConstants.AMOUNT_OF_CORNERS,
            BorderDrawingConstants.HORIZONTAL_CHAR);

        var lowerBorder = string.Format("{0}{1}{2}",
            BorderDrawingConstants.SEPARATOR_LEFT_BORDER,
            horizontalLine,
            BorderDrawingConstants.SEPARATOR_RIGHT_BORDER);

        // Assumes separators will always be drawn right after a Header.
        var currentConsoleRow = Console.GetCursorPosition().Top;
        Console.SetCursorPosition(BorderDrawingConstants.FIRST_BUFFER_COLUMN, currentConsoleRow + 1);
        Console.Write(lowerBorder);
    }
}
namespace CrudDaJustica.Cli.Lib.Decoration.Border;

/// <summary>
/// Represents the left border of a console window.
/// </summary>
internal class BorderLeft : IDisplayable
{
    public void Display()
    {
        int lastBufferColumn = Console.BufferHeight - 1;

        foreach (var row in Enumerable.Range(
            BorderDrawingConstants.FIRST_VERT_BORDER_ROW,
            lastBufferColumn - BorderDrawingConstants.AMOUNT_OF_CORNERS))
        {
            Console.SetCursorPosition(BorderDrawingConstants.FIRST_BUFFER_COLUMN, row);
            Console.Write(BorderDrawingConstants.VERTICAL_CHAR);
        }
    }
}

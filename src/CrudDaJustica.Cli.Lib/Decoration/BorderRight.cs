namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents the right border of a console window.
/// </summary>
internal class BorderRight : IDisplayable
{
    public void Display()
    {
        foreach (var row in Enumerable.Range(
            BorderDrawingConstants.FIRST_VERT_BORDER_ROW, 
            Console.BufferHeight - 1 - BorderDrawingConstants.AMOUNT_OF_CORNERS))
        {
            Console.SetCursorPosition(Console.BufferWidth - 1, row);
            Console.Write(BorderDrawingConstants.VERTICAL_CHAR);
        }
    }
}

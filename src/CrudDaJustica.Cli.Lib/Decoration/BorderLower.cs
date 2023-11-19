namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents the lower border of a console window.
/// </summary>
internal class BorderLower : IDisplayable
{
    public void Display()
    {
        var horizontalLine = string.Empty.PadRight
            (Console.BufferWidth - BorderDrawingConstants.AMOUNT_OF_CORNERS, 
            BorderDrawingConstants.HORIZONTAL_CHAR);

        var lowerBorder = string.Format("{0}{1}{2}", 
            BorderDrawingConstants.UPWARD_LEFT_CORNER, 
            horizontalLine, 
            BorderDrawingConstants.UPWARD_RIGHT_CORNER);

        var penultimateRow = Console.BufferHeight - 2;
        Console.SetCursorPosition(0, penultimateRow);
        Console.WriteLine(lowerBorder);
    }
}

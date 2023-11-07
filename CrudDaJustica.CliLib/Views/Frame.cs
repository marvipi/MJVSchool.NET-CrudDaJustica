namespace CrudDaJustica.CliLib.Views;

/// <summary>
/// Represents a screen in the user interface.
/// </summary>
public abstract class Frame : View
{
    // Summary: A text to display on the center of the top border.
    private readonly string title;

    // Summary: The ASCII character used draw the horizontal borders on the console window.
    private readonly char borderChar;

    // Summary: Information to display on the upper console window.
    private readonly Header header;

    protected Frame(string title, char borderChar, Header header) : base()
    {
        this.title = title;
        this.borderChar = borderChar;
        this.header = header;
    }

    /// <summary>
    /// Draws the upper border, the header, a separator and the lower border.
    /// </summary>
    public override void Display()
    {
        Console.Clear();
        base.Display();

        DrawUpperBorder();
        header.Display();
        var rowAfterHeader = Console.GetCursorPosition().Top;
        DrawLine(rowAfterHeader);
        DrawLowerBorder();

        Console.SetCursorPosition(left: 0, rowAfterHeader + 1);
    }

    // Summary: Draws the borderChar along the first row of the console buffer.
    //          Writes the title in the middle of the row.
    private void DrawUpperBorder()
    {
        Console.Clear();

        var titleLength = this.title.Length;
        var title = this.title
            .PadLeft(titleLength + 1)
            .PadRight(titleLength + 2);

        var leftpaddingSize = (Console.BufferWidth + title.Length) / 2;
        var rightPaddingSize = Console.BufferWidth;
        var upperBorder = title
            .PadLeft(leftpaddingSize, borderChar)
            .PadRight(rightPaddingSize, borderChar);

        Console.WriteLine(upperBorder);
    }

    // Summary: Draws the borderChar across the last row of the console buffer.
    private void DrawLowerBorder()
    {
        var penultimateRow = Console.BufferHeight - 2;
        DrawLine(penultimateRow);
    }

    // Summary: Draws the borderChar across a given row of the console buffer.
    private void DrawLine(int row)
    {
        var horizontalBorder = borderChar
            .ToString()
            .PadRight(Console.BufferWidth, borderChar);

        Console.SetCursorPosition(0, row);
        Console.WriteLine(horizontalBorder);
    }
}

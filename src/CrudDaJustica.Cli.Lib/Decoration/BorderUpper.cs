namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents the upper border of a console window.
/// </summary>
internal class BorderUpper : IDisplayable
{
    // Summary: A title to display in the center of the upper border.
    private readonly string title;

    /// <summary>
    /// Initializes a new instance of the <see cref="BorderUpper"/> class.
    /// </summary>
    /// <param name="title"> A title to display in the center of the upper border. </param>
    /// <exception cref="ArgumentException"></exception>
    public BorderUpper(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            var msg = string.Format("{0} cannot be null, empty or whitespace.", nameof(title));
            throw new ArgumentException(msg);
        }

        this.title = title.Trim();
    }

    public void Display()
    {
        var upperBorder = string.Format("{0} {1} {2}", 
            BorderDrawingConstants.LEFT_TITLE_BORDER, 
            title, 
            BorderDrawingConstants.RIGHT_TITLE_BORDER);

        var leftHalfWidth = (Console.BufferWidth + upperBorder.Length) / 2;
        var rightHalfWidth = Console.BufferWidth;

        var upperBorderWithoutCorners = upperBorder
            .PadLeft(leftHalfWidth - BorderDrawingConstants.AMOUNT_OF_CORNERS, BorderDrawingConstants.HORIZONTAL_CHAR)
            .PadRight(rightHalfWidth - BorderDrawingConstants.AMOUNT_OF_CORNERS, BorderDrawingConstants.HORIZONTAL_CHAR);

        Console.WriteLine("{0}{1}{2}", 
            BorderDrawingConstants.DOWNWARD_LEFT_CORNER, 
            upperBorderWithoutCorners, 
            BorderDrawingConstants.DOWNWARD_RIGHT_CORNER);
    }
}

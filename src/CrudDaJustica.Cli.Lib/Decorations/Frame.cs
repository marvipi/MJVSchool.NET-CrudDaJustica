using System.Text;

namespace CrudDaJustica.Cli.Lib.Decorations;

/// <summary>
/// Represents the borders around a console window.
/// </summary>
public class Frame : HorizontalLineDisplayer, IDisplayable
{
    private const int FIRST_EMPTY_ROW = 1;

    private const char UPPER_LEFT_CORNER = '┌';
    private const char UPPER_RIGHT_CORNER = '┐';

    private const char TITLE_LEFT_BORDER = '┤';
    private const char TITLE_RIGHT_BORDER = '├';

    private const char VERTICAL_CHAR = '│';

    private const char LOWER_LEFT_CORNER = '└';
    private const char LOWER_RIGHT_CORNER = '┘';

    private readonly string title;

    /// <summary>
    /// Initializes a new instance of the <see cref="Frame"/> class.
    /// </summary>
    /// <param name="title"> The title of the window. </param>
    public Frame(string title)
    {
        this.title = title.Trim();
    }

    public void Display()
    {
        Console.Clear();
        var borders = new StringBuilder();
        AppendUpperBorder(borders);
        AppendVerticalBorders(borders);
        AppendLowerBorder(borders);
        Console.Write(borders);
    }

    // Summary: Appends a line along the first row of the console buffer, placing downward corners on each end.
    //          Puts the title in the middle of the row.
    private void AppendUpperBorder(StringBuilder borders)
    {
        var formattedTitle = string.Format("{0} {1} {2}", TITLE_LEFT_BORDER, title, TITLE_RIGHT_BORDER);

        var halfWidth = (Console.BufferWidth - formattedTitle.Length + TWO_CORNERS) / 2;
        var lineLength = halfWidth - TWO_CORNERS;

        // Prevents a gap in the left half of the upper border.
        var leftWidth = int.IsOddInteger(title.Length)
            ? lineLength + 1
            : lineLength;

        borders.Append(UPPER_LEFT_CORNER);
        borders.Append(HORIZONTAL_CHAR, lineLength);
        borders.Append(formattedTitle);
        borders.Append(HORIZONTAL_CHAR, leftWidth);
        borders.Append(UPPER_RIGHT_CORNER);
        borders.AppendLine();
    }

    // Summary: Appends vertical borders down to the antipenultimate row of the console buffer.
    private static void AppendVerticalBorders(StringBuilder borders)
    {
        int antipenultimateRow = Console.BufferHeight - 3;
        foreach (var _ in Enumerable.Range(FIRST_EMPTY_ROW, antipenultimateRow))
        {
            borders.Append(VERTICAL_CHAR);
            borders.Append(' ', LineWidth);
            borders.Append(VERTICAL_CHAR);
            borders.AppendLine();
        }
    }

    // Summary: Appends a horizontal line to the borders, placing upward corners on each end.
    private static void AppendLowerBorder(StringBuilder borders)
    {
        var lowerBorder = DrawHorizontalLine(LOWER_LEFT_CORNER, LOWER_RIGHT_CORNER);
        borders.AppendLine(lowerBorder);
    }
}

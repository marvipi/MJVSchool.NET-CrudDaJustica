using PP_dotNet.View.Keybindings;
using System.Collections.Immutable;

namespace PP_dotNet.View.UI;

/// <summary>
/// Represents a screen in the user interface.
/// </summary>
public class View
{
    private readonly string title;
    private readonly Keybinding exitKeybinding;
    private bool exitKeyPressed;
    private readonly char borderChar;
    private readonly ImmutableList<Window> windows;

    /// <summary>
    /// Initializes a new instance of the <see cref="View"/> class.
    /// </summary>
    /// <param name="title"> The title to display on top of the screen. </param>
    /// <param name="exitKey"> The console key that will exit this view. </param>
    /// <param name="borderChar"> A character to draw on the borders of the console buffer. </param>
    /// <param name="windows"> The list of windows nested within this view. </param>
    public View(string title, RebindableKey exitKey, char borderChar, IEnumerable<Window> windows)
    {
        this.title = title;
        exitKeybinding = exitKey.Bind(Exit);
        exitKeyPressed = false;
        this.borderChar = borderChar;
        this.windows = windows.ToImmutableList();
    }

    /// <summary>
    /// Draws the border around the view then displays each nested window.
    /// </summary>
    public void Display()
    {
        Console.CursorVisible = false;
        while (!exitKeyPressed)
        {
            DrawBorder();
            DisplayExitKey();
            windows.ForEach(w => w.Display());

            var input = Console.ReadKey(true).Key;
            if (input == exitKeybinding.Key)
            {
                exitKeybinding.Invoke();
            }
        }
        Console.CursorVisible = true;
    }

    // Summary: Draw the borderChar around the entire screen buffer.
    private void DrawBorder()
    {
        DrawUpperBorder();
        DrawLowerBorder();
        DisplayExitKey();
    }

    // Summary: Draws the borderChar along the first row of the console buffer, writes the title in the middle the row.
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

    // Summary: Draws the borderChar across the row of the console buffer.
    private void DrawLowerBorder()
    {
        var lowerBorder = borderChar
            .ToString()
            .PadRight(Console.BufferWidth, borderChar);

        var penultimateRow = Console.BufferHeight - 2;
        Console.SetCursorPosition(0, penultimateRow);
        Console.WriteLine(lowerBorder);
    }

    // Summary: Displays the exitKeybinding display text on the first line of this view.
    private void DisplayExitKey()
    {
        const int spacing = 1;
        Console.SetCursorPosition(spacing, spacing);
        Console.WriteLine(exitKeybinding);
    }

    // Summary: Signals this View that it's time to exit.
    private void Exit() => exitKeyPressed = true;
}

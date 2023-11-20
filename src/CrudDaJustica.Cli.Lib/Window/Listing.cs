using System.Text;

namespace CrudDaJustica.Cli.Lib.Window;

/// <summary>
/// Represents an interactable list of <see cref="T"/>.
/// </summary>
/// <typeparam name="T"> The type of the element to list on the screen. </typeparam>
public class Listing<T> : Window where T : notnull
{
    // Summary: The column of the console buffer where the ">" is drawn.
    private const int SELECTOR_COLUMN = 2;

    // Summary: The keybindings used to interact with a listing.
    private readonly List<Keybinding> keybindings;

    // Summary: The row of the first element of the listing being displayed, in console buffer coordinates.
    private int firstRow;

    // Summary: The row where the ">" is currently located, in console buffer coordinates.
    private int currentRow;

    // Summary: The row of the last element of the listing being displayed, in console buffer coordinates.
    private int lastRow;

    /// <summary>
    /// The elements being listed on screen.
    /// </summary>
    public IEnumerable<T> Elements { get; set; }

    /// <summary>
    /// The number of the page being displayed on screen.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// The position of the currently selected element in the current data page.
    /// </summary>
    public int CurrentlySelectedElement => currentRow - firstRow;

    /// <summary>
    /// Initializes a new instance of the <see cref="Listing{T}"/> class.
    /// </summary>
    /// <param name="frame"> The borders around a listing. </param>
    /// <param name="header"> Information to display above the listing. </param>
    public Listing(IDisplayable frame, IDisplayable header) : base(frame, header)
    {
        Elements = new List<T>();
        keybindings = new List<Keybinding>();
    }

    /// <summary>
    /// Displays this listing on the console window.
    /// </summary>
    public override void Display()
    {
        while (!ShouldExit)
        {
            Console.CursorVisible = false;
            base.Display();

            DisplayKeybindings();
            DisplayCurrentPage();

            firstRow = Console.GetCursorPosition().Top;
            ListElements();

            // Stops the cursor from appearing at the top left of the console when the listing is first displayed.
            if (currentRow <= firstRow)
            {
                Select(firstRow);
            }

            // Moves back up to the last row if the previous page had more elements than the current one.
            else if (currentRow > lastRow)
            {
                Select(lastRow);
            }

            else
            {
                Select(currentRow);
            }

            var input = Console.ReadKey(true).Key;
            keybindings
                .First(kb => kb.Key == input)
                ?.Invoke();
        }

        ShouldExit = false;
        Console.CursorVisible = true;
    }

    /// <summary>
    /// Adds a collection of keybindings to this <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="keybindings"> The keybindings to add. </param>
    public void AddKeybindings(IEnumerable<Keybinding> keybindings) => this.keybindings.AddRange(keybindings);

    // Summary: Displays all keybindings in a single line of the console window.
    private void DisplayKeybindings()
    {
        var keybindingDisplay = new StringBuilder();
        foreach (var keybinding in keybindings)
        {
            keybindingDisplay.AppendFormat(" {0} ", keybinding);
        }

        Console.SetCursorPosition(SELECTOR_COLUMN, Console.GetCursorPosition().Top + 1);
        Console.Write(keybindingDisplay);
        Console.SetCursorPosition(SELECTOR_COLUMN, Console.GetCursorPosition().Top + 1);
    }

    // Summary: Displays the current page in the console window.
    private void DisplayCurrentPage()
    {
        Console.Write(" Page: {0}", CurrentPage);
        Console.SetCursorPosition(SELECTOR_COLUMN, Console.GetCursorPosition().Top + 1);
    }

    // Summary: Retrieves and lists a collection of elements, if any exist.
    private void ListElements()
    {
        if (!Elements.Any())
        {
            return;
        }

        var consoleRow = firstRow;
        foreach (var element in Elements)
        {
            var elementAsString = element
                ?.ToString();

            var displayElement = string.Format(" {0}", elementAsString);

            Console.SetCursorPosition(SELECTOR_COLUMN + 1, consoleRow);
            Console.Write(displayElement);
            consoleRow++;
        }
        lastRow = Console.GetCursorPosition().Top;
    }

    /// <summary>
    /// Selects the next element of the listing.
    /// </summary>
    /// <remarks>
    /// Wraps back to the first row if the user tries to move past the last row.
    /// </remarks>
    public void Next()
    {
        currentRow = currentRow < lastRow
            ? currentRow + 1
            : firstRow;

        Select(currentRow);
    }

    /// <summary>
    /// Selects the previous element of the listing.
    /// </summary>
    /// <remarks>
    /// Wraps around to the last row if the user tries to move behind the first row.
    /// </remarks>
    public void Previous()
    {
        currentRow = currentRow > firstRow
            ? currentRow - 1
            : lastRow;

        Select(currentRow);
    }

    // Summary: Selects the element at a given row of the listing, if any elements are being displayed.
    private void Select(int row)
    {
        // Stops the ">" from being drawn on the screen when the listing has no elements.
        if (Elements.Any())
        {
            Console.SetCursorPosition(SELECTOR_COLUMN, row);
            Console.Write(">");
            currentRow = row;
        }
    }

    /// <summary>
    /// Signals the listing that it should exit during the next frame.
    /// </summary>
    public void Exit() => ShouldExit = true;
}

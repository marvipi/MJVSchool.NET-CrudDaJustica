using CrudDaJustica.Cli.Lib.Keybindings;

namespace CrudDaJustica.Cli.Lib.Views;

/// <summary>
/// Represents an interactable list of <see cref="T"/>.
/// </summary>
/// <typeparam name="T"> The type of the element to list on the screen. </typeparam>
public class Listing<T> : Frame
{
    // Summary: The first column of the console buffer.
    private const int FAR_LEFT_COORD = 0;

    // Summary: The row of the first element of the listing being displayed, in console buffer coordinates.
    private int firstRow;

    // Summary: The row where the ">" is currently located, in console buffer coordinates.
    private int currentRow;

    // Summary: The row of the last element of the listing being displayed, in console buffer coordinates.
    private int lastRow;

    // Summary: The elements being listed on screen.
    private IEnumerable<T> elements;

    // Summary: A delegate that retrives the elements to be listed.
    private readonly Func<IEnumerable<T>> listRetriever;

    /// <summary>
    /// The position of the currently selected element in the current data page.
    /// </summary>
    public int CurrentlySelectedElement => currentRow - firstRow;

    /// <summary>
    /// Initializes a new instance of the <see cref="Listing{T}"/> class.
    /// </summary>
    /// <param name="title"> The title to display on top of the screen. </param>
    /// <param name="borderChar"> A character to draw on the borders of the console buffer. </param>
    /// <param name="header"> Information to display on the top of the console buffer. </param>
    /// <param name="listRetriever"> A reference to a method that retrieves the elements to be listed. </param>
    /// <param name="exitKey"> The console key that will exit this view. </param>
    /// <param name="create"> A keybinding used to open up a creation form. </param>
    /// <param name="nextPage"> A key map used to get the next page in the <see cref="Listing{T}"/>. </param>
    /// <param name="previousPage"> A key map used to return to the previous page <see cref="Listing{T}"/>. </param>
    /// <param name="nextElement"> A key map used to select the next element in the <see cref="Listing{T}"/>. </param>
    /// <param name="previousElement"> A key map used to select the previous element in the <see cref="Listing{T}"/>. </param>
    public Listing(string title,
        char borderChar,
        Header header,
        Func<IEnumerable<T>> listRetriever,
        RebindableKey exitKey,
        Keybinding create,
        Keybinding update,
        Keybinding delete,
        Keybinding nextPage,
        Keybinding previousPage,
        RebindableKey nextElement,
        RebindableKey previousElement) : base(title, borderChar, header)
    {
        elements = new List<T>();
        this.listRetriever = listRetriever;
        var keybindings = new List<Keybinding>()
        {
            exitKey.Bind(Exit),
            create,
            update,
            delete,
            nextPage,
            previousPage,
            nextElement.Bind(Next),
            previousElement.Bind(Previous),
        };
        Keybindings.AddRange(keybindings);
    }

    /// <summary>
    /// Displays this listing on the console window.
    /// </summary>
    public override void Display()
    {
        while (!ExitKeyPressed)
        {
            base.Display();

            DisplayKeybindings();

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

            Invoke(input);
        }

        ExitKeyPressed = false;
    }

    // Summary: Retrieves and lists a collection of elements, if any exist.
    private void ListElements()
    {
        elements = listRetriever.Invoke();
        if (!elements.Any())
        {
            return;
        }

        foreach (var element in elements)
        {
            Console.Write(" ");
            Console.Write(element?.ToString());
            Console.WriteLine();
        }
        lastRow = Console.GetCursorPosition().Top - 1;
    }

    // Summary: Selects the next element of the listing.
    // Remarks: Wraps back to the first row if the user tries to move past the last row.
    private void Next()
    {
        currentRow = currentRow < lastRow
            ? currentRow + 1
            : firstRow;

        Select(currentRow);
    }

    // Summary: Selects the previous element of the listing
    // Remarks: Wraps around to the last row if the user tries to move behind the first row.
    private void Previous()
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
        if (elements.Any())
        {
            Console.SetCursorPosition(FAR_LEFT_COORD, row);
            Console.Write(">");
            currentRow = row;
        }
    }
}

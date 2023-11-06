using PP_dotNet.View.Keybindings;

namespace PP_dotNet.View.UI;

/// <summary>
/// Represents an interactable list of <see cref="T"/>.
/// </summary>
/// <typeparam name="T"> The type of the element to list on the screen. </typeparam>
public class Listing<T> : Window
{
    private const int FAR_LEFT_COORD = 0;
    private IEnumerable<T> elements;
    private readonly Func<IEnumerable<T>> listRetriever;
    private readonly List<Keybinding<int>> selectors;
    private int firstRow;
    private int currentRow;
    private int lastRow;

    /// <summary>
    /// The position of the currently selected element in the current page.
    /// </summary>
    public int CurrentlySelectedElement => currentRow - firstRow;

    /// <summary>
    /// Initializes a new instance of the <see cref="Listing{T}"/> class.
    /// </summary>
    /// <param name="exitKey"> An unbound console key that will cause the program to exit. </param>
    /// <param name="listRetriever"> A reference to a method that retrieves the elements to be listed. </param>
    /// <param name="selectors"> Methods that operate on the currently selected element of the listing. </param>
    /// <param name="nextPage"> A key map used to get the next page in the <see cref="Listing{T}"/>. </param>
    /// <param name="previousPage"> A key map used to return to the previous page <see cref="Listing{T}"/>. </param>
    /// <param name="nextElement"> A key map used to select the next element in the <see cref="Listing{T}"/>. </param>
    /// <param name="previousElement"> A key map used to select the previous element in the <see cref="Listing{T}"/>. </param>
    public Listing(RebindableKey exitKey,
        Header? header,
        Func<IEnumerable<T>> listRetriever,
        IEnumerable<Keybinding<int>> selectors,
        Keybinding nextPage,
        Keybinding previousPage,
        RebindableKey nextElement,
        RebindableKey previousElement,
        IEnumerable<Keybinding> optionalKeybindings) : base(exitKey, optionalKeybindings, header)
    {
        elements = new List<T>();
        this.listRetriever = listRetriever;
        this.selectors = selectors.ToList();
        var navigationKeys = new List<Keybinding>()
        {
            nextPage,
            previousPage,
            nextElement.Bind(Next),
            previousElement.Bind(Previous),
        };
        KeyBindings.AddRange(navigationKeys);
    }

    /// <summary>
    /// Displays each element of this listing sequentially, ignoring <see cref="null"/> elements.
    /// </summary>
    /// <remarks>
    /// Elements are displayed using their ToString method.
    /// </remarks>
    public override void Display()
    {
        Console.CursorVisible = false;

        while (!ExitKeyPressed)
        {
            Console.Clear();
            Header?.Display();

            selectors.ForEach(selector => Console.Write("{0}\t", selector));

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

            selectors
                .FirstOrDefault(s => s.Key == input)
                ?.Invoke(CurrentlySelectedElement);

            Invoke(input);
        }

        ExitKeyPressed = false;
        Console.CursorVisible = true;
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

    // Summary: Selects the element at a given row of the listing, if it has any elements.
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

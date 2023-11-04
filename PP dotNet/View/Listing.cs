namespace PP_dotNet.View;

/// <summary>
/// Represents an interactable list of <see cref="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Listing<T>
{
    private const int FAR_LEFT_COORD = 0;
    private IEnumerable<T> elements;
    private readonly Func<IEnumerable<T>> listRetriever;
    private readonly IEnumerable<BoundKeyMap> navigationKeys;
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
    /// <param name="listRetriever"> A reference to a method that retrieves the elements to be listed. </param>
    /// <param name="nextPage"> A key map used to get the next page in the <see cref="Listing{T}"/>. </param>
    /// <param name="previousPage"> A key map used to return to the previous page <see cref="Listing{T}"/>. </param>
    /// <param name="nextElement"> A key map used to select the next element in the <see cref="Listing{T}"/>. </param>
    /// <param name="previousElement"> A key map used to select the previous element in the <see cref="Listing{T}"/>. </param>
    public Listing(Func<IEnumerable<T>> listRetriever,
        BoundKeyMap nextPage,
        BoundKeyMap previousPage,
        UnboundKeyMap nextElement,
        UnboundKeyMap previousElement)
    {
        elements = new List<T>();
        this.listRetriever = listRetriever;
        navigationKeys = new List<BoundKeyMap>()
        {
            nextPage,
            previousPage,
            nextElement.Bind(Next),
            previousElement.Bind(Previous),
        };
    }

    /// <summary>
    /// Displays each element of this listing sequentially, ignoring <see cref="null"/> elements.
    /// </summary>
    /// <remarks>
    /// Elements are displayed using their ToString method.
    /// </remarks>
    public void Display()
    {
        DisplayKeyMaps();
        ListElements();

        // Stops the cursor from appearing at the top left of the console when the listing is first displayed.
        if (currentRow <= firstRow)
        {
            Select(firstRow);
            return;
        }

        // Moves back up to the last row if the previous page had more elements than the current one.
        if (currentRow > lastRow)
        {
            Select(lastRow);
            return;
        }

        Select(currentRow);
    }

    // Summary: Displays each key map on the same line, separated by a tabulation character.
    private void DisplayKeyMaps()
    {
        foreach (var keyMap in navigationKeys)
        {
            Console.Write(keyMap);
            Console.Write("\t");
        }
        Console.WriteLine();
        firstRow = Console.GetCursorPosition().Top;
    }

    // Summary: Retrieves and lists a collection of elements, if any exist.
    private void ListElements()
    {
        elements = listRetriever.Invoke();
        if (!elements.Any())
        {
            return;
        }

        Console.CursorVisible = false;
        firstRow = Console.GetCursorPosition().Top;

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

    /// <summary>
    /// Invokes the <see cref="Action"/> mapped to a keyboard key.
    /// </summary>
    /// <param name="key"> A standard keyboard key. </param>
    public void Invoke(ConsoleKey key)
    {
        var keyMap = navigationKeys.FirstOrDefault(keyMap => keyMap.Key == key);
        keyMap?.Invoke();
    }
}

using PP_dotNet.View.Keybindings;
using PP_dotNet.View.UI;

namespace PP_dotNet.View.Builders;

/// <summary>
/// Represents a builder simplifies the creation of a <see cref="Listing{T}"/>.
/// </summary>
/// <typeparam name="T"> The type of the elements displayed by the <see cref="Listing{T}"/>. </typeparam>
public class ListingBuilder<T>
{
    private RebindableKey exitKey;
    private Keybinding nextPage;
    private Keybinding previousPage;
    private RebindableKey nextElement;
    private RebindableKey previousElem;

    private Func<IEnumerable<T>> retriever;
    private readonly List<Keybinding<int>> selectors;

    private Header? header;
    private readonly List<Keybinding> optionalKeybindings;

    /// <summary>
    /// Initializes a new instance of the <see cref="Listing{T}"/>.
    /// </summary>
    public ListingBuilder()
    {
        selectors = new();
        optionalKeybindings = new();
    }


    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to an action that exits the <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindExitKey(ConsoleKey key, string displayText)
    {
        exitKey = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to an <see cref="Action"/> that shows the next page of the <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <param name="nextPage"> The action to bind. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindNextPageKey(ConsoleKey key, string displayText, Action nextPage)
    {
        this.nextPage = new Keybinding(nextPage, key, displayText);
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to an <see cref="Action"/> that shows the previous page of the <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <param name="previousPage"> The action to bind. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindPreviousPageKey(ConsoleKey key, string displayText, Action previousPage)
    {
        this.previousPage = new Keybinding(previousPage, key, displayText);
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to an action that selects the next element of the <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindNextElementKey(ConsoleKey key, string displayText)
    {
        nextElement = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to an action that selects the previous element of the <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindPreviousElementKey(ConsoleKey key, string displayText)
    {
        previousElem = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Adds a delegate that retrieves a collection of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="retriever"> A delegate that returns a collection <typeparamref name="T"/>. </param>
    /// <returns> A reference to this instance after the adding operation has completed. </returns>
    public ListingBuilder<T> AddRetriever(Func<IEnumerable<T>> retriever)
    {
        this.retriever = retriever;
        return this;
    }

    /// <summary>
    /// Binds an <see cref="Action{int}"/> to a <see cref="ConsoleKey"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <param name="selector"> The action to bind. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindSelector(ConsoleKey key, string displayText, Action<int> selector)
    {
        var newSelector = new Keybinding<int>(selector, key, displayText);
        selectors.Add(newSelector);
        return this;
    }

    /// <summary>
    /// Adds a header to the top of the <see cref="Listing{T}"/>.
    /// </summary>
    /// <param name="header"> The header to add. </param>
    /// <returns> A reference to this instance after the adding operation has completed. </returns>
    public ListingBuilder<T> AddHeader(Header header)
    {
        this.header = header;
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to an <see cref="Action"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> The text that informs the user of the binding. </param>
    /// <param name="action"> The action to bind. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public ListingBuilder<T> BindKey(ConsoleKey key, string displayText, Action action)
    {
        var newKeybinding = new Keybinding(action, key, displayText);
        optionalKeybindings.Add(newKeybinding);
        return this;
    }

    /// <summary>
    /// Builds a new <see cref="Listing{T}"/> out of all the elements added so far.
    /// </summary>
    /// <returns>
    /// A new <see cref="Listing{T}"/> ready to be displayed.
    /// </returns>
    public Listing<T> Build() => new(exitKey, header, retriever, selectors, nextPage, previousPage, nextElement, previousElem, optionalKeybindings);
}

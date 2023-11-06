using PP_dotNet.View.Forms;
using PP_dotNet.View.Keybindings;
using PP_dotNet.View.UI;

namespace PP_dotNet.View.Builders;

/// <summary>
/// Represents a builder that simplifies the creation of a new <see cref="Form{T}"/>.
/// </summary>
/// <typeparam name="T"> The type of the class used to generate the form. </typeparam>
public class FormBuilder<T> where T : new()
{
    private RebindableKey nextKey;
    private RebindableKey cancelKey;
    private RebindableKey confirmKey;
    
    private Action<T> creator;
    private Action<T, int> updater;

    private Header? header;
    
    // TODO Guide the creation through all required instance variables.

    /// <summary>
    /// Binds the <see cref="Form{T}.Create()"/> method to an <see cref="Action{T}"/>.
    /// </summary>
    /// <param name="action"> The action to bind. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public FormBuilder<T> BindCreateTo(Action<T> action)
    {
        creator = action;
        return this;
    }

    /// <summary>
    /// Binds the <see cref="Form{T}.Update(int)()"/> method to an <see cref="Action{T,int}"/>.
    /// </summary>
    /// <param name="action"> The action to bind. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public FormBuilder<T> BindUpdateTo(Action<T, int> action)
    {
        updater = action;
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to the cancellation prompt of the <see cref="Form{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> A text to display on the screen when the cancellation prompt is shown. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public FormBuilder<T> BindCancelKey(ConsoleKey key, string displayText)
    {
        cancelKey = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to the confirmation prompt of the <see cref="Form{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> A text to display on the screen when the confirmation prompt is shown. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public FormBuilder<T> BindConfirmKey(ConsoleKey key, string displayText)
    {
        confirmKey = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Binds a <see cref="ConsoleKey"/> to the next prompt of the <see cref="Form{T}"/>.
    /// </summary>
    /// <param name="key"> The console key to bind. </param>
    /// <param name="displayText"> A text to display on the screen when the next prompt is shown. </param>
    /// <returns> A reference to this instance after the binding operation has completed. </returns>
    public FormBuilder<T> BindNextKey(ConsoleKey key, string displayText)
    {
        nextKey = new RebindableKey(key, displayText);
        return this;
    }

    /// <summary>
    /// Adds a header to the top of the <see cref="Form{T}"/>.
    /// </summary>
    /// <param name="header"> The header to add. </param>
    /// <returns> A reference to this instance after the adding operation has completed. </returns>
    public FormBuilder<T> AddHeader(Header header)
    {
        this.header = header;
        return this;
    }

    /// <summary>
    /// Builds a new <see cref="Form{T}"/> out of all the elements added so far.
    /// </summary>
    /// <returns>
    /// A new <see cref="Form{T}"/> ready to be used.
    /// </returns>
    public Form<T> Build() => new(header, cancelKey, confirmKey, new(), nextKey, updater, creator);
}

namespace CrudDaJustica.CliLib.Forms;

/// <summary>
/// Represents a field <see cref="Form{T}"/>.
/// </summary>
public class Field
{
    /// <summary>
    /// The text to display when this field is shown on screen.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// The user input read from the console.
    /// </summary>
    public string? Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Field"/> class.
    /// </summary>
    /// <param name="title"> The text to display when this field is shown on screen. </param>
    /// <exception cref="ArgumentNullException"></exception>
    public Field(string title)
    {
        if (title is null)
        {
            var errorMsg = string.Format("{0} cannot be null.", nameof(title));
            throw new ArgumentNullException(errorMsg);
        }

        Title = title;
    }

    /// <summary>
    /// Reads input from the console window and stores it in <see cref="Value"/>.
    /// </summary>
    /// <remarks>
    /// Tries to read input from <see cref="Console.In"/>.
    /// </remarks>
    public void Read() => Value = Console.ReadLine();

}
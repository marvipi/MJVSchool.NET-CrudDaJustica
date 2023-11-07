using CrudDaJustica.Cli.Lib.Keybindings;
using CrudDaJustica.Cli.Lib.Views;
using System.Reflection;

namespace CrudDaJustica.Cli.Lib.Forms;

/// <summary>
/// Represents a form that generates fields automatically for a <typeparamref name="T"/> and uses them to read input from the user.
/// </summary>
/// <typeparam name="T"> The type of the class used to generate the fields. </typeparam>
public class Form<T> : Frame where T : new()
{
    // Summary: The fields that have not been read yet.
    private readonly Queue<Field> fields;

    // Summary: All public properties of T.
    // Remarks: Used to generate forms dynamically.
    private readonly Queue<PropertyInfo> properties;

    /// <summary>
    /// The data read from the user.
    /// </summary>
    /// <remarks>
    /// The contents of of a form are overwritten every time it is displayed.
    /// </remarks>
    public T FormData { get; init; }

    // Summary: The key that will trigger an event that utilizes the form data.
    private readonly Keybinding confirmKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="Form{T}"/> class.
    /// </summary>
    /// <param name="title"> The title to display on top of the screen. </param>
    /// <param name="borderChar"> A character to draw on the borders of the console buffer. </param>
    /// <param name="cancelKey"> An unbound console key that will discard all that in the the form. </param>
    /// <param name="header"> Information to display on the top of the console buffer. </param>
    /// <param name="confirmKey"> An unbound console key that will save the data in the form. </param>
    public Form(string title,
        char borderChar,
        Header header,
        RebindableKey cancelKey,
        Keybinding confirmKey) : base(title, borderChar, header)
    {
        fields = new();
        properties = new();
        FormData = new();
        this.confirmKey = confirmKey;
        var keybindings = new List<Keybinding>()
        {
            cancelKey.Bind(Exit),
            confirmKey, // Used to display the keybinding when the confirmation prompt is shown.
        };
        Keybindings.AddRange(keybindings);
    }

    /// <summary>
    /// Displays this form on the console window.
    /// </summary>
    public override void Display()
    {
        Console.Clear();
        base.Display();

        InitializeFields();

        var screenCoords = DrawFields();

        ReadAllFields(screenCoords);

        DisplayConfirmationPrompt();
    }

    // Summary: Prompts the user to save or discard the data entered in the form.
    private void DisplayConfirmationPrompt()
    {
        DisplayKeybindings();
        while (!ExitKeyPressed)
        {
            var input = Console.ReadKey(true).Key;
            if (input == confirmKey.Key)
            {
                confirmKey.Invoke();
                Exit();
            }
            else
            {
                Invoke(input);
            }
        }
        ExitKeyPressed = false;
    }

    // Summary: Creates a field for each public property in T.
    private void InitializeFields()
    {
        foreach (var prop in FormData.GetType().GetProperties())
        {
            fields.Enqueue(new Field(prop.Name));
            properties.Enqueue(prop);
        }
    }

    // Summary: Displays the fields vertically on the console screen.
    // Returns: The console coordinates right after each field.
    private Queue<(int column, int row)> DrawFields()
    {
        var coords = new Queue<(int column, int row)>();

        foreach (var prop in properties)
        {
            Console.Write(" {0}: ", prop.Name);
            coords.Enqueue(Console.GetCursorPosition());
            Console.WriteLine();
        }

        return coords;
    }

    // Summary: Reads input from each field in the form from top to bottom.
    private void ReadAllFields(Queue<(int column, int row)> screenCoords)
    {
        while (screenCoords.Any())
        {
            (var column, var row) = screenCoords.Peek();
            Console.SetCursorPosition(column, row);
            ReadField();
            screenCoords.Dequeue();
        }
    }

    // Summary: Reads input from the current field of the user form.
    private void ReadField()
    {
        Console.CursorVisible = true;
        var currentField = fields.Peek();
        currentField.Read();

        properties
            .Peek()
            .SetValue(FormData, currentField.Value);

        fields.Dequeue();
        properties.Dequeue();
        Console.CursorVisible = false;
    }
}
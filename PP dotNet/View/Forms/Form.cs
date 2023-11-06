using System.Reflection;
using PP_dotNet.View.Keybindings;
using PP_dotNet.View.UI;

namespace PP_dotNet.View.Forms;

/// <summary>
/// Represents a form that generates fields automatically for a <typeparamref name="T"/> and uses them to read input from the user.
/// </summary>
/// <typeparam name="T"> The type of the class used to generate the fields. </typeparam>
public class Form<T> : Window
{
    private readonly Queue<Field> fields;
    private Queue<PropertyInfo> properties;
    private readonly RebindableKey confirm;
    private readonly T formData;
    private readonly Action<T, int> updater;
    private readonly Action<T> creator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Form{T}"/> class.
    /// </summary>
    /// <param name="header"> </param>
    /// <param name="cancelKey"> An unbound console key that will exit from the form. </param>
    /// <param name="nextKey"> An unbound console key that will cause the form to advance to the next field. </param>
    /// <param name="createKey"> An unbound console key that will save the data read from the user. </param>
    public Form(Header header,
        RebindableKey cancelKey,
        RebindableKey confirm,
        T emptyData,
        RebindableKey nextKey,
        Action<T, int> updater,
        Action<T> creator) : base(cancelKey)
    {
        fields = new();
        KeyBindings.Add(nextKey.Bind(ReadNext));
        Header = header;
        this.confirm = confirm;
        formData = emptyData;
        this.updater = updater;
        this.creator = creator;
        properties = new();
    }

    public override void Display()
    {
        InitializeFields();

        Console.Clear();
        Header?.Display();
        var screenCoords = DisplayFields();

        DisplayFields(screenCoords);

        Console.Write(KeyBindings.First()); // Displays the exit key
        Console.Write("\t");
        Console.Write(confirm.Key);
        Console.Write("\t");
    }

    public void Create()
    {
        Display();

        while (!ExitKeyPressed)
        {
            var input = Console.ReadKey(true).Key;
            if (input == confirm?.Key)
            {
                creator?.Invoke(formData);
                break;
            }
            else
            {
                Invoke(input);
            }
        }

        ExitKeyPressed = false;
        properties = new();
    }

    public void Update(int row)
    {
        Display();

        while (!ExitKeyPressed)
        {
            var input = Console.ReadKey(true).Key;
            if (input == confirm?.Key)
            {
                updater?.Invoke(formData, row);
                break;
            }
            else
            {
                Invoke(input);
            }
        }

        ExitKeyPressed = false;
        properties = new();
    }

    private void InitializeFields()
    {
        foreach (var prop in formData.GetType().GetProperties())
        {
            fields.Enqueue(new Field(prop.Name));
            properties.Enqueue(prop);
        }
    }


    // Summary: Displays all field sequentially.
    private void DisplayFields(Queue<(int column, int row)> screenCoords)
    {
        while (fields.Any())
        {
            (var column, var row) = screenCoords.Peek();
            Console.SetCursorPosition(column, row);
            ReadNext();
            screenCoords.Dequeue();
        }
    }

    // Summary: Displays the fields in the screen sequentially.
    // Returns: The console coordinates after each field.
    // Remarks: The returned coordinates are affected by the format given during instantiation.
    private Queue<(int column, int row)> DisplayFields()
    {
        var coords = new Queue<(int column, int row)>();

        foreach (var prop in properties)
        {
            Console.Write("{0}: ", prop.Name);
            coords.Enqueue(Console.GetCursorPosition());
            Console.WriteLine();
        }

        return coords;
    }

    // Summary: Reads input from the next field of the user form.
    private void ReadNext()
    {
        Console.CursorVisible = true;
        var currentField = fields.Peek();
        currentField.Read();

        properties
            .Peek()
            .SetValue(formData, currentField.Value);

        fields.Dequeue();
        properties.Dequeue();
        Console.CursorVisible = false;
    }
}
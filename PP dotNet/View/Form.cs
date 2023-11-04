namespace PP_dotNet.View;

/// <summary>
/// Represents a form that reads input from the user.
/// </summary>
public class Form
{
    private readonly Queue<Field> fields;
    private readonly string outputFormat;
    private readonly Dictionary<string, string?> formData;
    private readonly IEnumerable<string> keyMapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="Form"/> class.
    /// </summary>
    /// <remarks>
    /// Form operation must be handled externally.
    /// </remarks>
    /// <param name="fields"> A collection of fields to display alongside this form. </param>
    /// <param name="outputFormat"> The <see cref="string"/> format in which the fields will be displayed. </param>
    /// <param name="keyMappingDisplayTexts"> A collection of strings that will be displayed on top of the form. </param>
    public Form(Queue<Field> fields, string outputFormat, IEnumerable<string> keyMappingDisplayTexts)
    {
        this.fields = fields;
        this.outputFormat = outputFormat;
        formData = new();
        this.keyMapping = keyMappingDisplayTexts;
    }

    /// <summary>
    /// Displays the fields in the screen sequentially and then reads input for each field.
    /// </summary>
    /// <returns>
    /// A <see cref="FormData"/> that contains all data read from the user.
    /// </returns>
    public FormData Display()
    {
        Console.CursorVisible = true;
        var screenCoords = DisplayFields();

        DisplayFields(screenCoords);

        DisplayKeyMapping();

        return new(formData);
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

    // Summary: Displays each key map in a single row, separated by a tabulation character.
    private void DisplayKeyMapping()
    {
        foreach (var keyMap in keyMapping)
        {
            Console.Write(keyMap);
            Console.Write("\t");
        }
    }

    // Summary: Displays the fields in the screen sequentially.
    // Returns: The console coordinates after each field.
    // Remarks: The returned coordinates are affected by the format given during instantiation.
    private Queue<(int column, int row)> DisplayFields()
    {
        var coords = new Queue<(int column, int row)>();

        foreach (var field in fields)
        {
            Console.Write(outputFormat, field.Title);
            coords.Enqueue(Console.GetCursorPosition());
            Console.WriteLine();
        }

        return coords;
    }

    // Summary: Reads input from the next field of the user form.
    private void ReadNext()
    {
        var currentField = fields.Peek();
        currentField.Read();
        formData[currentField.Title] = currentField.Value;
        fields.Dequeue();
    }
}
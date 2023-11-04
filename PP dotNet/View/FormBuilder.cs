namespace PP_dotNet.View;

/// <summary>
/// Represents a user form with an arbitrary number of fields.
/// </summary>
public class FormBuilder
{
    private readonly Queue<Field> fields;
    private readonly string outputFormat;
    private readonly List<string> keyMappingDisplayTexts;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormBuilder"/> class.
    /// </summary>
    /// <param name="outputFormat"> The <see cref="string"/> format in which the fields will be displayed. </param>
    public FormBuilder(string outputFormat)
    {
        fields = new();
        this.outputFormat = outputFormat; // TODO Validate outputFormat
        keyMappingDisplayTexts = new();
    }

    /// <summary>
    /// Appends a new <see cref="Field"/> to the end of the form.
    /// </summary>
    /// <param name="newField"> The <see cref="Field"/> to append. </param>
    /// <returns> A reference to this instance after the append operation has completed. </returns>
    public FormBuilder Append(Field newField) // TODO Deny duplicate field titles
    {
        fields.Enqueue(newField);
        return this;
    }

    /// <summary>
    /// Adds a new <see cref="UnboundKeyMap"/> to the form.
    /// </summary>
    /// <param name="keyMap"> The <see cref="UnboundKeyMap"/> to add. </param>
    /// <returns> A reference to this instance after the append operation has completed. </returns>
    public FormBuilder Add(UnboundKeyMap keyMap)
    {
        keyMappingDisplayTexts.Add(keyMap.ToString());
        return this;
    }

    /// <summary>
    /// Transforms this form builder into a new <see cref="Form"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Form"/> ready to be displayed.
    /// </returns>
    public Form Build() => new(fields, outputFormat, keyMappingDisplayTexts);
}

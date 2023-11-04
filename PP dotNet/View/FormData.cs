namespace PP_dotNet.View;

/// <summary>
/// Represents a collection of information read from a <see cref="Form"/>.
/// </summary>
public readonly struct FormData
{
    /// <summary>
    /// Data read from the user.
    /// </summary>
    public IReadOnlyDictionary<string, string?> Data { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormData"/> class.
    /// </summary>
    /// <param name="formData"> A collection of data read from a form data. </param>
    public FormData(IReadOnlyDictionary<string, string?> formData)
    {
        Data = formData;
    }
}
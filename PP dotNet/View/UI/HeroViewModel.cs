namespace PP_dotNet.View.UI;

/// <summary>
/// Contains information about a hero that can be displayed in the user interface.
/// </summary>
public readonly struct HeroViewModel
{
    /// <summary>
    /// The name of a hero's secret identity.
    /// </summary>
    public string Alias { get; init; }

    /// <summary>
    /// The first name of the person behind the secret identity.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// The middle name of the person behind the secret identity.
    /// </summary>
    public string? MiddleName { get; init; }

    /// <summary>
    /// The last name of the person behind the secret identity.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// A date when a hero was first seen.
    /// </summary>
    public DateOnly Debut { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeroViewModel"/> struct.
    /// </summary>
    /// <param name="alias"> The name of a hero's secret identity. </param>
    /// <param name="firstName"> The first name of the person behind the secret identity. </param>
    /// <param name="middleName"> The middle name of the person behind the secret identity. </param>
    /// <param name="lastName"> The last name of the person behind the secret identity. </param>
    /// <param name="debut"> A date when a hero was first seen. </param>
    public HeroViewModel(string alias, string firstName, string middleName, string lastName, DateOnly debut) : this(alias, debut)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeroViewModel"/> struct.
    /// </summary>
    /// <param name="alias"> The name of a hero's secret identity. </param>
    /// <param name="debut"> A date when a hero was first seen. </param>
    public HeroViewModel(string alias, DateOnly debut)
    {
        Alias = alias;
        Debut = debut;
    }

    public override string ToString()
    {
        return string.Format("{0} - {1} - {2} - {3} - {4}", Alias, FirstName, MiddleName, LastName, Debut);
    }
}
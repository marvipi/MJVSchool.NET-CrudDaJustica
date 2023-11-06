using System.Text;

namespace PP_dotNet.Model;

/// <summary>
/// Represents a hero from the DC Universe.
/// </summary>
public class HeroEntity
{
    /// <summary>
    /// The name of a hero's secret identity.
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// The first name of the person behind the secret identity.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The middle name of the person behind the secret identity.
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// The last name of the person behind the secret identity.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// A date when a hero was first seen.
    /// </summary>
    public DateOnly Debut { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeroEntity"/> class.
    /// </summary>
    /// <param name="alias"> The name of a hero's secret identity. </param>
    /// <param name="firstName"> The first name of the person behind the secret identity. </param>
    /// <param name="middleName"> The middle name of the person behind the secret identity. </param>
    /// <param name="lastName"> The last name of the person behind the secret identity. </param>
    /// <param name="debut"> A date when a hero was first seen. </param>
    public HeroEntity(string alias, string firstName, string middleName, string lastName, DateOnly debut) : this(alias, debut)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HeroEntity"/> class.
    /// </summary>
    /// <param name="alias"> The name of a hero's secret identity. </param>
    /// <param name="debut"> A date when a hero was first seen. </param>
    public HeroEntity(string alias, DateOnly debut)
    {
        Alias = alias;
        Debut = debut;
    }
}

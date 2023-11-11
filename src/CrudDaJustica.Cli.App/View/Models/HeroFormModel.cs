namespace CrudDaJustica.Cli.App.View.Models;

/// <summary>
/// Represents the hero data that can be read from a form.
/// </summary>
public class HeroFormModel
{
    /// <summary>
    /// The name of a hero's secret identity.
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// The date when a hero was first seen.
    /// </summary>
    public string? Debut { get; set; }

    /// <summary>
    /// The first name of the person behind the secret identity.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The last name of the person behind the secret identity.
    /// </summary>
    public string? LastName { get; set; }
}
namespace CrudDaJustica.Website.Models;

/// <summary>
/// Represents information about a hero to display in the website.
/// </summary>
/// <param name="Id"> A unique identifier that distinguishes the hero from all others. </param>
/// <param name="Alias"> The secret identity of the hero. </param>
/// <param name="Debut"> The date when the hero was first seen. </param>
/// <param name="FirstName"> The hero's first name. </param>
/// <param name="LastName"> The hero's last name. </param>
public record HeroViewModel(Guid Id, string Alias, DateOnly Debut, string FirstName, string LastName);

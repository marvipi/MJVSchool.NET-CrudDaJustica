using System.ComponentModel.DataAnnotations;

namespace CrudDaJustica.Website.Models;

/// <summary>
/// Represents hero information read from a form.
/// </summary>
/// <param name="Alias"> The name of a hero's secret identity. </param>
/// <param name="Debut"> The date when a hero was first seen. </param>
/// <param name="FirstName"> The first name of the person behind the secret identity. </param>
/// <param name="LastName"> The last name of the person behind the secret identity. </param>
public record HeroFormModel(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Alias is required")] string Alias,
    [Required(ErrorMessage = "Debut is required")] DateOnly Debut,
    [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")] string FirstName,
    [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")] string LastName);
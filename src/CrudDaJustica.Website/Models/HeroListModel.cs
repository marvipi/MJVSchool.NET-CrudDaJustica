namespace CrudDaJustica.Website.Models;

/// <summary>
/// Represents a list containing hero information to display on the website.
/// </summary>
/// <param name="HeroViewModels"> A collection of hero information to display on the website. </param>
/// <param name="PageRange"> The amount of pages of hero information registered in the system. </param>
/// <param name="CurrentPage"> The current page of information being listed. </param>
public record HeroListModel(IEnumerable<HeroViewModel> HeroViewModels, IEnumerable<int> PageRange, int CurrentPage);
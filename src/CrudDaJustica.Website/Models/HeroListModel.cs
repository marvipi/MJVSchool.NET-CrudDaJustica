namespace CrudDaJustica.Website.Models;

/// <summary>
/// Represents a list containing non-confidential hero information.
/// </summary>
/// <param name="heroViewModels"> A collection of non-confidential hero information. </param>
/// <param name="PageRange"> The amount of pages of hero information registered in the system. </param>
/// <param name="CurrentPage"> The current page of information being listed. </param>
public record HeroListModel(IEnumerable<HeroViewModel> heroViewModels, IEnumerable<int> PageRange, int CurrentPage);
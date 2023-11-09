using PP_dotNet.Data;
using PP_dotNet.Model;
using PP_dotNet.Services;
using PP_dotNet.View.Models;
using System.Globalization;

namespace PP_dotNet.Controller;

/// <summary>
/// Represents a controller that manages communication between the hero repository and the user interface.
/// </summary>
public class HeroController
{
    private readonly IHeroRepository heroRepository;
    private readonly PagingService pagingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HeroController"/> class.
    /// </summary>
    /// <param name="heroRepository"> The repository that stores information about the heroes. </param>
    /// <param name="pagingService"> A service responsible for paging the data in the repositories. </param>
    public HeroController(IHeroRepository heroRepository, PagingService pagingService)
    {
        this.heroRepository = heroRepository;
        this.pagingService = pagingService;
        this.pagingService.RepositorySize = heroRepository.RepositorySize;
    }

    /// <summary>
    /// Creates a new hero in the repository>.
    /// </summary>
    /// <param name="heroFormModel"> A form model that contains data about the new hero. </param>
    public void Create(HeroFormModel heroFormModel)
    {
        DateOnly.TryParseExact(heroFormModel.Debut,
            CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
            CultureInfo.CurrentCulture,
            DateTimeStyles.None,
            out var validDate); // TODO Validate
        heroRepository.RegisterHero(new HeroEntity(heroFormModel.Alias, validDate, heroFormModel.FirstName, heroFormModel.LastName)); // TODO Refactor
        pagingService.RepositorySize = heroRepository.RepositorySize;
    }

    /// <summary>
    /// Lists all heroes registered in the current page of the repository.
    /// </summary>
    /// <returns> 
    /// A collection of all heroes registered in the current page of the repository. 
    /// </returns>
    public IEnumerable<HeroViewModel> List()
    {
        var currentPage = pagingService.GetCurrentPage();
        var heroes = heroRepository.GetHeroes(currentPage);

        var heroViewModels = heroes
            .Select(hero => new HeroViewModel(hero.Alias, hero.Debut, hero.FirstName, hero.LastName))
            .ToList();

        return heroViewModels;
    }

    /// <summary>
    /// Updates the information about a hero.
    /// </summary>
    /// <param name="heroFormModel"> The updated information about a hero. </param>
    /// <param name="row"> The row of the current page where the hero is registered. </param>
    public void Update(HeroFormModel heroFormModel, int row)
    {
        DateOnly.TryParseExact(heroFormModel.Debut,
            CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
            CultureInfo.CurrentCulture,
            DateTimeStyles.None,
            out var validDate); // TODO Validate
        heroRepository.UpdateHero(pagingService.GetCurrentPage(), row,
            new HeroEntity(heroFormModel.Alias, validDate, heroFormModel.FirstName, heroFormModel.LastName)); // TODO Refactor
    }

    /// <summary>
    /// Removes a hero from the repository.
    /// </summary>
    /// <param name="row"> The row of the current page where the hero is registered. </param>
    public void Delete(int row)
    {
        heroRepository.DeleteHero(pagingService.GetCurrentPage(), row);
        pagingService.RepositorySize = heroRepository.RepositorySize;
    }

    /// <summary>
    /// Moves to the next page of the repository, up to the last page.
    /// </summary>
    public void NextPage() => pagingService.NextPage();

    /// <summary>
    /// Returns to the previous page of the repository, down to the first page.
    /// </summary>
    public void PreviousPage() => pagingService.PreviousPage();
}
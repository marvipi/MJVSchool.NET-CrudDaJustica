using PP_dotNet.Model;

namespace PP_dotNet.Data;

/// <summary>
/// Represents a repository that stores information about <see cref="HeroEntity"/>.
/// </summary>
public interface IHeroRepository
{
    /// <summary>
    /// The amount of information stored in this repository
    /// </summary>
    public int RepositorySize { get; }

    /// <summary>
    /// Registers a new <see cref="HeroEntity"/> in this repository.
    /// </summary>
    /// <param name="newHero"> The hero to register. </param>
    public void RegisterHero(HeroEntity newHero);

    /// <summary>
    /// Retrieves information about <see cref="HeroEntity"/> registered in a given page.
    /// </summary>
    /// <param name="page"> The page where information will be retrieved. </param>
    /// <returns> A collection of all <see cref="HeroEntity"/> registered in the page. </returns>
    public IEnumerable<HeroEntity> GetHeroes(DataPage page);

    /// <summary>
    /// Deletes a <see cref="HeroEntity"/> from the repository.
    /// </summary>
    /// <param name="page"> The page where the hero is registered. </param>
    /// <param name="row"> The row where the hero is registered, in relation to <paramref name="page"/>. </param>
    public void DeleteHero(DataPage page, int row);

    /// <summary>
    /// Updates the information about a registered <see cref="HeroEntity"/>.
    /// </summary>
    /// <param name="page"> The page where the hero is registered. </param>
    /// <param name="row"> The row where the hero is registered, in relation to <paramref name="page"/>. </param>
    /// <param name="updatedHero"> A <see cref="HeroEntity"/> containing update to date information about the hero. </param>
    public void UpdateHero(DataPage page, int row, HeroEntity updatedHero);
}

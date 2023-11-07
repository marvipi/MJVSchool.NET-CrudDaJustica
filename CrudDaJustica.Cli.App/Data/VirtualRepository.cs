using PP_dotNet.Model;

namespace PP_dotNet.Data;

/// <summary>
/// Represents a repository that stores data in memory.
/// </summary>
/// <remarks>
/// All data will be lost when the system is shutdown.
/// </remarks>
public class VirtualRepository : IHeroRepository
{
    // Summary: All heroes registered in this repository.
    private HeroEntity[] heroes;

    public int RepositorySize => LastFilledIndex(heroes) + 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualRepository"/> class.
    /// </summary>
    /// <param name="initialSize"> The amount of rows the repository initially. </param>
    public VirtualRepository(uint initialSize)
    {
        heroes = new HeroEntity[initialSize];
    }

    public void RegisterHero(HeroEntity newHero)
    {
        if (RepositorySize == heroes.Length)
        {
            Array.Resize(ref heroes, heroes.Length * 2);
        }
        var firstEmptyIndex = LastFilledIndex(heroes) + 1;
        heroes[firstEmptyIndex] = newHero;
    }

    public IEnumerable<HeroEntity> GetHeroes(DataPage page)
    {
        var skip = Skip(page);
        var take = Take(page);
        var heroesPage = heroes[skip..take];

        var amountNonNull = LastFilledIndex(heroesPage) + 1;
        var nonNullHeroes = heroesPage[..amountNonNull];
        return nonNullHeroes;
    }

    public void UpdateHero(DataPage page, int row, HeroEntity updatedHero)
    {
        var heroIndex = Skip(page) + row;
        heroes[heroIndex] = updatedHero;
    }

    public void DeleteHero(DataPage page, int row)
    {
        var skip = Skip(page) + row;
        var heroesUntilLastIndex = heroes.Length - 1 - skip;

        foreach (var i in Enumerable.Range(skip, heroesUntilLastIndex))
        {
            heroes[i] = heroes[i + 1];
        }
    }

    // Summary: Calculates how many rows of data to skip in a data page.
    // Remarks: Used for paging data.
    private static int Skip(DataPage page) => (page.Number - 1) * page.Rows;

    // Summary: Calculates how many rows of data to take in a data page.
    // Remarks: Used for paging data.
    private static int Take(DataPage page) => page.Number * page.Rows;


    // Summary: Produces the last filled index in an array of T.
    private static int LastFilledIndex<T>(T[] array)
    {
        var firstEmptyIndex = Array.IndexOf(array, null);
        return int.IsNegative(firstEmptyIndex)
            ? array.Length - 1
            : firstEmptyIndex - 1;
    }
}

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

        var newHeroes = new List<HeroEntity>()
        {
            new HeroEntity("Martian Manhunter", new(1955, 11, 1), "J'onn", "J'onzz"),
             new HeroEntity("Superman", new(1938, 6, 1), "Clark", "Kent"),
             new HeroEntity("Batman", new(1939, 5, 1), "Bruce", "Wayne"),
             new HeroEntity("Wonder Woman", new(1939, 5, 1), "Diana", "of Themyscira"),
             new HeroEntity("The Flash", new(1956, 10, 1), "Barry", "Allen"),
             new HeroEntity("Green Lantern", new(1959, 10, 1), "Hal", "Jordan"),
             new HeroEntity("Aquaman", new(1941, 11, 1), "Arthur", "Curry"),
             new HeroEntity("Hawkgirl", new(1941, 7, 1), "Shiera", "Hall"),
             new HeroEntity("Hawkman", new(1940, 1, 1), "Carter", "Hall"),
             new HeroEntity("Atom", new(1961, 10, 1), "Ray", "Palmer"),
            new HeroEntity("Zatanna", new(1964, 11, 1), "Zatanna", "Zatara"),
        };

        newHeroes.ForEach(h => RegisterHero(h));
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

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
    private HeroEntity[] heroes;

    public int RepositorySize => LastFilledIndex(heroes) + 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualRepository"/> class.
    /// </summary>
    /// <param name="initialSize"> The amount of rows the repository initially. </param>
    public VirtualRepository(uint initialSize)
    {
        heroes = new HeroEntity[initialSize];

        // TODO Remove after prototype is done.
        foreach (var i in Enumerable.Range(0, 25 + 1))
        {
            var newHero = new HeroEntity(i.ToString(), DateOnly.FromDateTime(DateTime.Now));
            RegisterHero(newHero);
        }
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

    public IEnumerable<HeroEntity> GetHeroes(DataPage pagina)
    {
        var skip = (pagina.Number - 1) * pagina.Rows;
        var take = pagina.Number * pagina.Rows;
        var heroesPage = heroes[skip..take];

        var amountNonNull = LastFilledIndex(heroesPage) + 1;
        var nonNullHeroes = heroesPage[..amountNonNull];
        return nonNullHeroes;
    }

    public void UpdateHero(DataPage page, int row, HeroEntity updatedHero)
    {
        var heroIndex = (page.Number - 1) * page.Rows + row;
        heroes[heroIndex] = updatedHero;
    }

    public void DeleteHero(DataPage page, int row)
    {
        var skip = (page.Number - 1) * page.Rows + row;
        var heroesUntilLastIndex = heroes.Length - 1 - skip;

        foreach (var i in Enumerable.Range(skip, heroesUntilLastIndex))
        {
            heroes[i] = heroes[i + 1];
        }
    }

    // Summary: Produces the last filled index in an array of T.
    private int LastFilledIndex<T>(T[] array)
    {
        var firstEmptyIndex = Array.IndexOf(array, null);
        return int.IsNegative(firstEmptyIndex)
            ? array.Length - 1
            : firstEmptyIndex - 1;
    }
}

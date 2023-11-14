using CrudDaJustica.Data.Lib.Model;
using CrudDaJustica.Data.Lib.Services;

namespace CrudDaJustica.Data.Lib.Data;

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
		var skip = (page.Number - 1) * page.Rows;
		var take = page.Number * page.Rows;
		var heroesPage = heroes[skip..take];

		var amountNonNull = LastFilledIndex(heroesPage) + 1;
		var nonNullHeroes = heroesPage[..amountNonNull];
		return nonNullHeroes;
	}

	public bool UpdateHero(Guid id, HeroEntity updatedHero)
	{
		var index = 0;

		foreach (var hero in heroes[..])
		{
			if (hero.Id == id)
			{
				heroes[index] = new HeroEntity()
				{
					Id = id,
					Alias = updatedHero.Alias,
					Debut = updatedHero.Debut,
					FirstName = updatedHero.FirstName,
					LastName = updatedHero.LastName,
				};
				return true;
			}
			index++;
		}

		return false;
	}

	public bool DeleteHero(Guid id)
	{
		var indexToDelete = -1;

		foreach (var i in Enumerable.Range(0, RepositorySize))
		{
			if (heroes[i].Id == id)
			{
				indexToDelete = i;
				break;
			}

		}

		if (indexToDelete < 0)
		{
			return false;
		}

		foreach (var i in Enumerable.Range(indexToDelete, heroes.Length - 2))
		{
			heroes[i] = heroes[i + 1];
		}

		return true;
	}

	// Summary: Produces the last filled index in an array of T.
	private static int LastFilledIndex<T>(T[] array)
	{
		var firstEmptyIndex = Array.IndexOf(array, null);
		return int.IsNegative(firstEmptyIndex)
			? array.Length - 1
			: firstEmptyIndex - 1;
	}
}

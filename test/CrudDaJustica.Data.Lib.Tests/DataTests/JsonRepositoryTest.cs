using CrudDaJustica.Data.Lib.Data;
using CrudDaJustica.Data.Lib.Model;
using CrudDaJustica.Data.Lib.Services;
using System.Text.Json;

namespace CrudDaJustica.Data.Lib.Test.DataTests;

[TestFixture]
public class JsonRepositoryTest
{
	private const string testDirName = "Json Repository Test";
	private const string testFileName = "heroTestData.json";
	private static string TestFilePath => Path.Combine(TestContext.CurrentContext.WorkDirectory, testDirName, testFileName);
	private static void DeleteTestDir() => Directory.Delete(Path.GetDirectoryName(TestFilePath)!, true);

	[Test]
	public void Constructor_HeroDataFilePathIsNullOrRoot_ThrowsArgumentException()
	{
		Assert.Multiple(() =>
			{
				Assert.Throws<ArgumentException>(() => new JsonRepository(null!));
				Assert.Throws<ArgumentException>(() => new JsonRepository("C:"));
				Assert.Throws<ArgumentException>(() => new JsonRepository("/"));
			});
	}

	[Test]
	public void RegisterHero_EmptyRepository_AppendsNewHeroToTheEndOfTheFile()
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var initialRepositorySize = jsonRepo.RepositorySize;

			var newHero = new HeroEntity("Superman", new(1938, 6, 1), "Clark", "Kent");
			jsonRepo.RegisterHero(newHero);
			var newRepositorySize = jsonRepo.RepositorySize;
			const int AMOUNT_HEROES_REGISTERED = 1;

			using (var streamReader = File.OpenText(TestFilePath))
			{
				var heroInFile = streamReader.ReadLine();
				var newHeroAsJson = JsonSerializer.Serialize(newHero);
				Assert.Multiple(() =>
				{
					Assert.That(heroInFile, Is.EqualTo(newHeroAsJson));
					Assert.That(newRepositorySize, Is.EqualTo(AMOUNT_HEROES_REGISTERED));
				});
			}
		}
		finally
		{
			DeleteTestDir();
		}
	}

	[Test]
	public void RegisterHero_NonEmptyRepository_AppendsNewHeroToTheEndOfTheFile()
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var initialRepositorySize = jsonRepo.RepositorySize;

			var heroAlreadyInRepo = new HeroEntity("Batman", new(1939, 5, 1), "Bruce", "Wayne");
			jsonRepo.RegisterHero(heroAlreadyInRepo);
			var newHero = new HeroEntity("Wonder Woman", new(1939, 5, 1), "Diana", "of Themyscira");
			jsonRepo.RegisterHero(newHero);
			var newRepositorySize = jsonRepo.RepositorySize;
			const int AMOUNT_HEROES_REGISTERED = 2;

			using (var streamReader = File.OpenText(TestFilePath))
			{
				var newHeroAsJson = JsonSerializer.Serialize(newHero);
				streamReader.ReadLine();
				var lastHeroInRepo = streamReader.ReadLine();

				Assert.Multiple(() =>
				{
					Assert.That(lastHeroInRepo, Is.EqualTo(newHeroAsJson));
					Assert.That(newRepositorySize, Is.EqualTo(AMOUNT_HEROES_REGISTERED));
				});
			}
		}
		finally
		{
			DeleteTestDir();
		}
	}

	[Test]
	public void GetHeroes_EmptyRepository_ReturnsAnEmptyCollection()
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);

			var dataPage = new DataPage(1, 1);
			var getHeroesResult = jsonRepo.GetHeroes(dataPage);

			Assert.That(getHeroesResult, Is.Empty);
		}
		finally
		{
			DeleteTestDir();
		}

	}

	[Test]
	public void GetHeroes_DataPageIsEqualToRepositorySize_ReturnsAllHeroesInTheRepository()
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var hero1 = new HeroEntity("Martian Manhunter", new(1955, 11, 1), "J'onn", "J'onzz");
			var hero2 = new HeroEntity("Zatanna", new(1964, 11, 1), "Zatanna", "Zatara");
			var hero3 = new HeroEntity("Hawkman", new(1940, 1, 1), "Carter", "Hall");
			jsonRepo.RegisterHero(hero1);
			jsonRepo.RegisterHero(hero2);
			jsonRepo.RegisterHero(hero3);

			var dataPage = new DataPage(number: 1, rows: jsonRepo.RepositorySize);
			var getHeroesResult = jsonRepo.GetHeroes(dataPage);
			var heroesInRepo = new List<HeroEntity>() { hero1, hero2, hero3 };

			Assert.That(getHeroesResult, Is.EquivalentTo(heroesInRepo));
		}
		finally
		{
			DeleteTestDir();
		}
	}

	[Test]
	public void GetHeroes_DataPageIsLessThanRepositorySize_ReturnsOnlyTheHeroesInThePage()
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var hero1 = new HeroEntity("Green Lantern", new(1959, 10, 1), "Harold", "Jordan");
			var hero2 = new HeroEntity("Aquaman", new(1941, 11, 1), "Arthur", "Curry");
			jsonRepo.RegisterHero(hero1);
			jsonRepo.RegisterHero(hero2);

			var dataPage = new DataPage(number: 1, rows: jsonRepo.RepositorySize - 1);
			var getHeroesResult = jsonRepo.GetHeroes(dataPage);
			var heroesInPage = new List<HeroEntity>() { hero1 };

			Assert.That(getHeroesResult, Is.EquivalentTo(heroesInPage));
		}
		finally
		{
			DeleteTestDir();
		}
	}

	[Test]
	public void UpdateHero_PageIsEmpty_DoesNothing()
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var hero1 = new HeroEntity("Atom", new(1961, 10, 1), "Ray", "Palmer");
			var hero2 = new HeroEntity("Flash", new(1940, 1, 1), "Barry", "Allen");
			jsonRepo.RegisterHero(hero1);
			jsonRepo.RegisterHero(hero2);

			var emptyDataPage = new DataPage(number: 2, rows: 2);
			var updatedHero = new HeroEntity("Specter", new(1940, 2, 1), "James", "Corrigan");
			jsonRepo.UpdateHero(emptyDataPage, row: 1, updatedHero);

			var heroesInEmptyPage = jsonRepo.GetHeroes(emptyDataPage);
			Assert.That(heroesInEmptyPage, Is.Empty);
		}
		finally
		{
			DeleteTestDir();
		}
	}

	[TestCase(0)]
	[TestCase(1)]
	[TestCase(2)]
	public void UpdateHero_DataPageIsNotEmpty_OverwritesOnlyTheHeroAtTheGivenRowOfTheGivenPage(int rowToUpdate)
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var hero1 = new HeroEntity("Doctor Occult", new(1935, 10, 1), "Richard", "Occult");
			var hero2 = new HeroEntity("Rose Psychic", new(1935, 10, 1), "Rose", "Spiritus");
			var hero3 = new HeroEntity("Zatara", new(1938, 6, 1), "Giovanni", "Zatara");
			jsonRepo.RegisterHero(hero1);
			jsonRepo.RegisterHero(hero2);
			jsonRepo.RegisterHero(hero3);

			var dataPage = new DataPage(number: 1, rows: 3);
			var updatedHero = new HeroEntity("Crimson Avenger", new(1938, 10, 1), "Lee", "Travis");
			jsonRepo.UpdateHero(dataPage, rowToUpdate, updatedHero);

			var heroesInDataPage = jsonRepo.GetHeroes(dataPage).ToList();
			Assert.That(heroesInDataPage[rowToUpdate], Is.EqualTo(updatedHero));
		}
		finally
		{
			DeleteTestDir();
		}
	}

	[TestCase(0, 1, 2)]
	[TestCase(1, 0, 2)]
	[TestCase(2, 0, 1)]
	public void DeleteHero_PageIsNotEmpty_DeletesTheHeroAtTheGivenRowOfTheGivenPage(int rowToDelete, int firstRowToKeep, int secondRowToKeep)
	{
		try
		{
			var jsonRepo = new JsonRepository(TestFilePath);
			var hero1 = new HeroEntity("Americommando", new(1938, 6, 1), "Harold", "Thompson");
			var hero2 = new HeroEntity("Arrow", new(1938, 9, 1), "Ralph", "Payne");
			var hero3 = new HeroEntity("The Guardian Angel", new(1939, 4, 1), "Hop", "Harrigan");
			jsonRepo.RegisterHero(hero1);
			jsonRepo.RegisterHero(hero2);
			jsonRepo.RegisterHero(hero3);
			var dataPage = new DataPage(number: 1, rows: 3);
			var heroesBeforeDeletion = jsonRepo.GetHeroes(dataPage).ToList();
			var initialRepositorySize = jsonRepo.RepositorySize;

			jsonRepo.DeleteHero(dataPage, rowToDelete);
			var newRepositorySize = jsonRepo.RepositorySize;

			var heroesAfterDeletion = jsonRepo.GetHeroes(dataPage);
			var deletedHero = heroesBeforeDeletion[rowToDelete];
			Assert.Multiple(() =>
			{
				Assert.That(heroesAfterDeletion, Does.Contain(heroesBeforeDeletion[firstRowToKeep]));
				Assert.That(heroesAfterDeletion, Does.Contain(heroesBeforeDeletion[secondRowToKeep]));
				Assert.That(heroesAfterDeletion, Does.Not.Contain(deletedHero));
				Assert.That(newRepositorySize, Is.EqualTo(initialRepositorySize - 1));
			});
		}
		finally
		{
			DeleteTestDir();
		}
	}
}

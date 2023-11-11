using CrudDaJustica.Cli.App.Data;
using CrudDaJustica.Cli.App.Services;

namespace CrudDaJustica.Cli.App.Test.ServicesTests;

[TestFixture]
public class PagingServiceTest
{
	[Test]
	public void Constructor_RowsPerPageIsLessThanOne_RaisesArgumentOutOfRangeException()
	{
		const int NOT_BEING_TESTED = 1;

		var heroRepo = InitializeHeroRepository(NOT_BEING_TESTED);

		Assert.Throws<ArgumentOutOfRangeException>(() => new PagingService(heroRepo, 0));
	}

	[TestCase(10, 10, 1)]
	[TestCase(40, 05, 8)]
	[TestCase(11, 03, 4)]
	[TestCase(01, 07, 1)]
	public void Constructor_ValidParameters_CalculatesTheLastPageCorrectly
		(int repositorySize, int rowsPerPage, int expected)
	{
		var heroRepo = InitializeHeroRepository(repositorySize);
		var pagingService = new PagingService(heroRepo, rowsPerPage);

		var actual = pagingService.LastPage;

		Assert.That(actual, Is.EqualTo(expected));
	}


	[TestCase(250, 150)]
	[TestCase(5, 100)]
	public void GetCurrentPage_DataPageCreation_NumberAndRowsAreEqualToCurrentPageAndRowsPerPage
		(int validRepositorySize, int validRowsPerPage)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);

		var currentPage = pagingService.GetCurrentPage();
		Assert.Multiple(() =>
		{
			Assert.That(currentPage.Number, Is.EqualTo(pagingService.CurrentPage));
			Assert.That(currentPage.Rows, Is.EqualTo(pagingService.RowsPerPage));
		});
	}

	[TestCase(200, 20)]
	public void NextPage_CurrentPageIsLessThanLastPage_IncrementsCurrentPage
		(int validRepositorySize, int validRowsPerPage)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);
		var initialCurrentPage = pagingService.CurrentPage;

		pagingService.NextPage();

		var expected = initialCurrentPage + 1;
		Assert.That(pagingService.CurrentPage, Is.EqualTo(expected));
	}

	[TestCase(1, 10)]
	public void NextPage_CurrentPageIsEqualToLastPage_DoesntIncrementCurrentPage
		(int validRepositorySize, int validRowsPerPage)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);
		var initialCurrentPage = pagingService.CurrentPage;

		pagingService.NextPage();

		var newCurrentPage = pagingService.CurrentPage;
		Assert.That(initialCurrentPage, Is.EqualTo(newCurrentPage));
	}

	[TestCase(10, 10, 2)]
	public void NextPage_RepositoryChangedSize_UpdatesTheLastPage
	(int validRepositorySize, int validRowsPerPage, int expected)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);

		heroRepo.RegisterHero(new("Doesn't matter", new(1, 1, 1), "Doesn't matter", "Doesn't matter"));
		pagingService.NextPage();

		var updatedLastPage = pagingService.LastPage;
		Assert.That(updatedLastPage, Is.EqualTo(expected));
	}

	[TestCase(10, 5)]
	public void PreviousPage_CurrentPageIsGreaterThanFirstPage_DecrementsCurrentPage
		(int validRepositorySize, int validRowsPerPage)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);
		var initialCurrentPage = pagingService.CurrentPage;

		pagingService.NextPage();
		pagingService.PreviousPage();

		var newCurrentPage = pagingService.CurrentPage;

		Assert.That(newCurrentPage, Is.EqualTo(initialCurrentPage));
	}

	[TestCase(75, 10)]
	public void PreviousPage_CurrentPageIsEqualToFirstPage_DoesntDecrementCurrentPage
		(int validRepositorySize, int validRowsPerPage)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);
		var numFirstPage = pagingService.CurrentPage;

		pagingService.PreviousPage();

		Assert.That(pagingService.CurrentPage, Is.EqualTo(numFirstPage));
	}

	[TestCase(20, 5, 5)]
	public void PreviousPage_RepositoryChangedSize_UpdatesTheLastPage
		(int validRepositorySize, int validRowsPerPage, int expected)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);

		heroRepo.RegisterHero(new("Doesn't matter", new(1, 1, 1), "Doesn't matter", "Doesn't matter"));
		heroRepo.RegisterHero(new("Doesn't matter", new(1, 1, 1), "Doesn't matter", "Doesn't matter"));
		pagingService.PreviousPage();

		var updatedLastPage = pagingService.LastPage;
		Assert.That(updatedLastPage, Is.EqualTo(expected));
	}

	private static IHeroRepository InitializeHeroRepository(int size)
	{
		var virtualRepository = new VirtualRepository((uint)size);
		foreach (var i in Enumerable.Range(0, size))
		{
			virtualRepository.RegisterHero(new(i.ToString(), new(1, 1, 1), i.ToString(), i.ToString()));
		}
		return virtualRepository;
	}

}

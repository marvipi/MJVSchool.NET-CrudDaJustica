using CrudDaJustica.Data.Lib.Data;
using CrudDaJustica.Data.Lib.Services;

namespace CrudDaJustica.Data.Lib.Test.ServicesTests;

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

	[TestCase(10, 10, 2)]
	public void JumpToPage_RepositoryChangedSize_UpdatesTheLastPage
	(int validRepositorySize, int validRowsPerPage, int expected)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);

		heroRepo.RegisterHero(new("Doesn't matter", new(1, 1, 1), "Doesn't matter", "Doesn't matter"));
		pagingService.JumpToPage(expected);

		var updatedLastPage = pagingService.LastPage;
		Assert.That(updatedLastPage, Is.EqualTo(expected));
	}

	[TestCase(20, 5, -1, PagingService.FIRST_PAGE)]
	[TestCase(30, 10, 2, 2)]
	[TestCase(100, 25, 5, 4)]
	public void JumpToPage_AllPointsOfVariance_AlwaysStaysBetweenFirstAndLastPage
		(int validRepositorySize, int validRowsPerPage, int pageNumber, int expected)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);

		pagingService.JumpToPage(pageNumber);
		var actual = pagingService.CurrentPage;

		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCase(25, 25, 1, 1)]
	[TestCase(10, 5, 1, 2)]
	[TestCase(50, 5, 1, 10)]
	public void PageRange_VariousPageRanges_AlwaysReturnsARangeBetweenFirstPageAndLastPage
		(int validRepositorySize, int validRowsPerPage, int expectedRangeStart, int expectedRangeEnd)
	{
		var heroRepo = InitializeHeroRepository(validRepositorySize);
		var pagingService = new PagingService(heroRepo, validRowsPerPage);

		var actual = pagingService.PageRange;
		var expected = Enumerable.Range(expectedRangeStart, expectedRangeEnd);

		Assert.That(actual, Is.EquivalentTo(expected));
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

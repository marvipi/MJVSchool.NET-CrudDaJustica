using CrudDaJustica.Cli.App.Services;

namespace CrudDaJustica.Cli.App.Test.ServicesTests;

[TestFixture]
public class PagingServiceTest
{
	[TestCase(10, 10, 1)]
	[TestCase(40, 05, 8)]
	[TestCase(11, 03, 4)]
	[TestCase(01, 07, 1)]
	public void LastPage_RepositorySizeAndRowsPerPageAreValid_CalculatesTheLastPageCorrectly
		(int repositorySize, int rowsPerPage, int expected)
	{
		var pagingService = new PagingService(repositorySize, rowsPerPage);

		var actual = pagingService.LastPage;

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Constructor_NegativeRepositorySize_RaisesArgumentOutOfRangeException()
	{
		var notBeingTested = 10;

		Assert.Throws<ArgumentOutOfRangeException>(() => new PagingService(-1, notBeingTested));
	}

	[Test]
	public void Constructor_RowsPerPageIsLessThanOne_RaisesArgumentOutOfRangeException()
	{
		var notBeingTested = 10;

		Assert.Throws<ArgumentOutOfRangeException>(() => new PagingService(notBeingTested, 0));
	}

	[TestCase(10, 5, 20, 4)]
	public void RepositorySize_Setting_AlsoUpdatesLastPage
		(int initialRepositorySize, int validRowsPerPage, int newRepositorySize, int expected)
	{
		var pagingService = new PagingService(initialRepositorySize, validRowsPerPage);

		pagingService.RepositorySize = newRepositorySize;

		Assert.That(pagingService.LastPage, Is.EqualTo(expected));
	}

	[TestCase(250, 150)]
	public void GetCurrentPage_DataPageNumberIsEqualToCurrentPage(int validRepositorySize, int validRowsPerPage)
	{
		var pagingService = new PagingService(validRepositorySize, validRowsPerPage);

		var currentPage = pagingService.GetCurrentPage();

		Assert.That(currentPage.Number, Is.EqualTo(pagingService.CurrentPage));
	}

	[TestCase(5, 98)]
	public void GetCurrentPage_DataPageRowsIsEqualToRowsPerPage(int validRepositorySize, int validRowsPerPage)
	{
		var pagingService = new PagingService(validRepositorySize, validRowsPerPage);

		var currentPage = pagingService.GetCurrentPage();

		Assert.That(currentPage.Rows, Is.EqualTo(pagingService.RowsPerPage));
	}

	[TestCase(200, 20)]
	public void NextPage_CurrentPageIsLessThanLastPage_IncrementsCurrentPage(int validRepositorySize, int validRowsPerPage)
	{
		var pagingService = new PagingService(validRepositorySize, validRowsPerPage);
		var initialCurrentPage = pagingService.CurrentPage;

		pagingService.NextPage();

		var expected = initialCurrentPage + 1;

		Assert.That(pagingService.CurrentPage, Is.EqualTo(expected));
	}

	[TestCase(1, 10)]
	public void NextPage_CurrentPageIsEqualToLastPage_DoesntIncrementCurrentPage(int validRepositorySize, int validRowsPerPage)
	{
		var pagingService = new PagingService(validRepositorySize, validRowsPerPage);
		var initialCurrentPage = pagingService.CurrentPage;

		pagingService.NextPage();

		var newCurrentPage = pagingService.CurrentPage;
		Assert.That(initialCurrentPage, Is.EqualTo(newCurrentPage));
	}

	[TestCase(10, 6)]
	public void PreviousPage_CurrentPageIsGreaterThanFirstPage_DecrementsCurrentPage(int validRepositorySize, int validRowsPerPage)
	{
		var pagingService = new PagingService(validRepositorySize, validRowsPerPage);
		var initialCurrentPage = pagingService.CurrentPage;

		pagingService.NextPage();
		pagingService.PreviousPage();

		var newCurrentPage = pagingService.CurrentPage;

		Assert.That(newCurrentPage, Is.EqualTo(initialCurrentPage));
	}

	[TestCase(75, 4)]
	public void PreviousPage_CurrentPageIsEqualToFirstPage_DoesntDecrementCurrentPage(int validRepositorySize, int validRowsPerPage)
	{
		var pagingService = new PagingService(validRepositorySize, validRowsPerPage);
		var numFirstPage = pagingService.CurrentPage;

		pagingService.PreviousPage();

		Assert.That(pagingService.CurrentPage, Is.EqualTo(numFirstPage));
	}
}

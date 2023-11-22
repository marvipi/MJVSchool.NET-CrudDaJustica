using CrudDaJustica.Data.Lib.Repositories;
using CrudDaJustica.Data.Lib.Services;

namespace CrudDaJustica.Data.Lib.Test.ServiceTests;

[TestFixture]
internal class PagingServiceTest
{
    [TestCase(10, 10, 1)]
    [TestCase(40, 12, 4)]
    [TestCase(11, 10, 2)]
    [TestCase(01, 15, 1)]
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
    public void DataPage_DataPageCreation_NumberAndRowsAreEqualToCurrentPageAndRowsPerPage
        (int validRepositorySize, int validRowsPerPage)
    {
        var heroRepo = InitializeHeroRepository(validRepositorySize);
        var pagingService = new PagingService(heroRepo, validRowsPerPage);

        var currentPage = pagingService.DataPage;
        Assert.Multiple(() =>
        {
            Assert.That(currentPage.Number, Is.EqualTo(pagingService.CurrentPage));
            Assert.That(currentPage.Rows, Is.EqualTo(pagingService.RowsPerPage));
        });
    }

    [TestCase(100, PagingService.MIN_ROWS_PER_PAGE - 1, PagingService.MIN_ROWS_PER_PAGE)]
    [TestCase(50, (PagingService.MIN_ROWS_PER_PAGE + PagingService.MAX_ROWS_PER_PAGE) / 2, (PagingService.MIN_ROWS_PER_PAGE + PagingService.MAX_ROWS_PER_PAGE) / 2)]
    [TestCase(37, PagingService.MAX_ROWS_PER_PAGE + 1, PagingService.MAX_ROWS_PER_PAGE)]
    public void RowsPerPage_Setting_AlwaysStaysWithinRange(int validRepositorySize, int value, int expected)
    {
        var heroRepo = InitializeHeroRepository(validRepositorySize);

        var pagingService = new PagingService(heroRepo, PagingService.MIN_ROWS_PER_PAGE);

        pagingService.RowsPerPage = value;

        Assert.That(pagingService.RowsPerPage, Is.EqualTo(expected));
    }

    [TestCase(10, 10, 2)]
    public void JumpToPage_RepositoryChangedSize_UpdatesTheLastPage
    (int validRepositorySize, int validRowsPerPage, int expected)
    {
        var heroRepo = InitializeHeroRepository(validRepositorySize);
        var pagingService = new PagingService(heroRepo, validRowsPerPage);

        heroRepo.RegisterHero(new(Guid.NewGuid(), "Doesn't matter", new(1, 1, 1), "Doesn't matter", "Doesn't matter"));
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
    [TestCase(100, 50, 1, 2)]
    [TestCase(500, 50, 1, 10)]
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
            virtualRepository.RegisterHero(new(Guid.NewGuid(), i.ToString(), new(1, 1, 1), i.ToString(), i.ToString()));
        }
        return virtualRepository;
    }

}

using CrudDaJustica.Cli.App.Data;
using System.Text;

namespace CrudDaJustica.Cli.App.Services;

/// <summary>
/// Represents a service that does data paging in a repository.
/// </summary>
public class PagingService
{
    // Summary: The hero repository being paged by this service.
    private IHeroRepository heroRepository;

    /// <summary>
    /// The first page of data in the repository.
    /// </summary>
    public const int FIRST_PAGE = 1;

    /// <summary>
    /// The current page of the repository.
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// The last page of data in the repository.
    /// </summary>
    public int LastPage { get; private set; }

    /// <summary>
    /// The amount of rows read per page.
    /// </summary>
    public int RowsPerPage { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagingService"/> class.
    /// </summary>
    /// <param name="heroRepository"> The hero repository that will be paged by this service. </param>
    /// <param name="rowsPerPage"> The amount of rows read per page. </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public PagingService(IHeroRepository heroRepository, int rowsPerPage)
    {
        if (rowsPerPage < 1)
        {
            var msg = new StringBuilder()
                .AppendFormat("O parâmetro {0} não pode ser menor que 1.", nameof(rowsPerPage))
                .ToString();
            throw new ArgumentOutOfRangeException(msg);
        }

        this.heroRepository = heroRepository;
        RowsPerPage = rowsPerPage;
        CurrentPage = FIRST_PAGE;
        CalculateLastPage();
    }

    /// <summary>
    /// Gets the <see cref="CurrentPage"/> and the <see cref="RowsPerPage"/> as a <see cref="DataPage"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="DataPage"/> that can be used to retrieve data from the repository.
    /// </returns>
    public DataPage GetCurrentPage() => new DataPage(CurrentPage, RowsPerPage);


    /// <summary>   
    /// Advances to the next page in the repository, up to the <see cref="LastPage"/>.
    /// </summary>
    public void NextPage()
    {
        CalculateLastPage();
        if (CurrentPage < LastPage)
        {
            CurrentPage++;
        }
    }

    /// <summary>
    /// Returns to the previous page of the repository, down to the <see cref="FIRST_PAGE"/>.
    /// </summary>
    public void PreviousPage()
    {
        CalculateLastPage();
        if (CurrentPage > FIRST_PAGE)
        {
            CurrentPage--;
        }
    }

    // Summary: Calculates the last page of the repository using on its size.
    private void CalculateLastPage()
    {
        var numPagesRequired = heroRepository.RepositorySize / (double)RowsPerPage;
        var lastPage = Math.Ceiling(numPagesRequired);
        LastPage = Convert.ToInt32(lastPage);
    }
}

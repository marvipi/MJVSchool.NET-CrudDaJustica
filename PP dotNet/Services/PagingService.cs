using System.Text;
using PP_dotNet.Data;

namespace PP_dotNet.Services;

/// <summary>
/// Represents a service that does data paging in a repository.
/// </summary>
public class PagingService
{
    private int repositorySize;

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
    /// The total amount of data rows in the repository.
    /// </summary>
    /// <remarks>
    /// Required for calculating the <see cref="LastPage"/> of a repository.
    /// </remarks>
    public int RepositorySize
    {
        get => repositorySize;
        set
        {
            repositorySize = value;
            var numPagesRequired = repositorySize / (double)RowsPerPage;
            var lastPage = Math.Ceiling(numPagesRequired);
            LastPage = Convert.ToInt32(lastPage);
        }
    }

    /// <summary>
    /// The amount of rows read per page.
    /// </summary>
    public int RowsPerPage { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagingService"/> class.
    /// </summary>
    /// <param name="repositorySize"> The total amount of data rows in the repository. </param>
    /// <param name="rowsPerPage"> The amount of rows read per page. </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public PagingService(int repositorySize, int rowsPerPage)
    {
        if (int.IsNegative(repositorySize))
        {
            var msg = new StringBuilder()
                .AppendFormat("O parâmetro {0} não pode ser negativo.", nameof(repositorySize))
                .ToString();
            throw new ArgumentOutOfRangeException(msg);
        }

        if (rowsPerPage < 1)
        {
            var msg = new StringBuilder()
                .AppendFormat("O parâmetro {0} não pode ser menor que 1.", nameof(rowsPerPage))
                .ToString();
            throw new ArgumentOutOfRangeException(msg);
        }

        RowsPerPage = rowsPerPage;
        RepositorySize = repositorySize;
        CurrentPage = FIRST_PAGE;
    }

    /// <summary>
    /// Gets the <see cref="CurrentPage"/> and the <see cref="RowsPerPage"/> as a <see cref="DataPage"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="DataPage"/> that can be used to retrieve data from the repository.
    /// </returns>
    public DataPage GetCurrentPage()
    {
        return new DataPage(CurrentPage, RowsPerPage);
    }

    /// <summary>   
    /// Advances to the next page in the repository, up to the <see cref="LastPage"/>.
    /// </summary>
    public void NextPage()
    {
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
        if (CurrentPage > FIRST_PAGE)
        {
            CurrentPage--;
        }
    }
}

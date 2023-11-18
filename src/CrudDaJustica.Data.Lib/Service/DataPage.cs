using System.Text;

namespace CrudDaJustica.Data.Lib.Service;

/// <summary>
/// Represents a page of information in a repository.
/// </summary>
public readonly struct DataPage
{
    /// <summary>
    /// The number of the page.
    /// </summary>
    public int Number { get; init; }

    /// <summary>
    /// The amount of rows in the page.
    /// </summary>
    public int Rows { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataPage"/> struct.
    /// </summary>
    /// <param name="number"> The number of the page. </param>
    /// <param name="rows"> The amount of rows in the page. </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DataPage(int number, int rows)
    {
        if (number < PagingService.FIRST_PAGE)
        {
            var errorMsg = new StringBuilder()
                .AppendFormat("The parameter {0} cannot be less than {1}.", nameof(number), PagingService.FIRST_PAGE)
                .ToString();
            throw new ArgumentOutOfRangeException(errorMsg);
        }

        if (rows < PagingService.MIN_ROWS_PER_PAGE)
        {
            var errorMsg = new StringBuilder()
                .AppendFormat("The parameter {0} cannot be less than {1}.", nameof(rows), PagingService.MIN_ROWS_PER_PAGE)
                .ToString();
            throw new ArgumentOutOfRangeException(errorMsg);
        }

        Number = number;
        Rows = rows;
    }
}

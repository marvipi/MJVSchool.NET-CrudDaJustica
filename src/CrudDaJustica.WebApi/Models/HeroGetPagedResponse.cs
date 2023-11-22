using CrudDaJustica.Data.Lib.Service;

namespace CrudDaJustica.WebApi.Models;

/// <summary>
/// Initializes a new instance of the <see cref="HeroGetPagedResponse"/> record.
/// </summary>
/// <param name="Heroes"> A collection of hero information. </param>
/// <param name="PageRange"> A range of integers that represent all pages in a hero repository. </param>
/// <param name="DataPage"> The data page where the <paramref name="Heroes"/> are registered. </param>
public record HeroGetPagedResponse(IEnumerable<HeroGetResponse> Heroes, IEnumerable<int> PageRange, DataPage DataPage);

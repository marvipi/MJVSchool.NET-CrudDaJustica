using PP_dotNet.Model;
using PP_dotNet.Services;
using System.Text.Json;

namespace PP_dotNet.Data;

/// <summary>
/// Represents a repository that stores information in a JSON file.
/// </summary>
public class JsonRepository : IHeroRepository
{
    // Summary: The path of the json file where hero data is stored.
    private readonly string heroDataFilePath;

    // Summary: A temporary file used to update or delete heroes from the repository.
    private string HeroDataTempFilePath => Path.Combine(Path.GetTempPath(), new FileInfo(heroDataFilePath).Name);

    public int RepositorySize { get; private set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="JsonRepository"/> class.
    /// </summary>
    /// <param name="heroDataFilePath"> The absolute path where the hero data file is or will be stored. </param>
    public JsonRepository(string heroDataFilePath)
    {
        var heroDataDirPath = Path.GetDirectoryName(heroDataFilePath);
        var heroDataDir = new DirectoryInfo(heroDataDirPath);

        if (!heroDataDir.Exists)
        {
            heroDataDir.Create();
        }

        var heroDataFile = new FileInfo(heroDataFilePath);
        if (!heroDataFile.Exists)
        {
            heroDataFile
                .CreateText()
                .Close();
        }

        this.heroDataFilePath = heroDataFilePath;
    }

    public void RegisterHero(HeroEntity newHero)
    {
        var heroAsJson = JsonSerializer.Serialize(newHero);
        using (var streamWriter = File.AppendText(heroDataFilePath))
        {
            streamWriter.WriteLine(heroAsJson);
        }
        RepositorySize++;
    }

    public IEnumerable<HeroEntity> GetHeroes(DataPage page) => File.ReadLines(heroDataFilePath)
                                                                    .Skip(RowsToSkip(page))
                                                                    .Take(RowToTake(page))
                                                                    .Select(line => JsonSerializer.Deserialize<HeroEntity>(line))
                                                                    .Cast<HeroEntity>();

    public void UpdateHero(DataPage page, int row, HeroEntity updatedHero)
    {
        if (GetHeroes(page).Any())
        {
            OverwriteData(page, row, JsonSerializer.Serialize(updatedHero));
        }
    }

    public void DeleteHero(DataPage page, int row)
    {
        if (GetHeroes(page).Any())
        {
            OverwriteData(page, row);
            RepositorySize--;
        }
    }

    // Summary: Copies all data from the current data file into a temporary one, altering it as necessary.
    //          Replaces the old data file with the temporary one, once the operation has completed.
    //
    // Remarks: If newData is null then the contents of row will be deleted from the file,
    //          otherwise they will be overwritten with newData.
    //          All other rows in the repository will be left untouched.
    private void OverwriteData(DataPage page, int row, string? newData = null)
    {
        var currentLine = 1;
        var lineToAlter = RowsToSkip(page) + row;
        var dataRow = string.Empty;

        using (var streamReader = new StreamReader(heroDataFilePath))
        {
            using var streamWriter = new StreamWriter(HeroDataTempFilePath);
            while ((dataRow = streamReader.ReadLine()) is not null)
            {
                if (currentLine != lineToAlter)
                {
                    streamWriter.WriteLine(dataRow);
                }
                else if (newData is null)
                {
                    continue; // Delete
                }
                else
                {
                    streamWriter.WriteLine(newData); // Update
                }
                currentLine++;
            }
        }

        File.Move(HeroDataTempFilePath, heroDataFilePath, true);
    }

    // Summary: Calculates how many rows of data to skip to reach a given data page.
    private static int RowsToSkip(DataPage page) => (page.Number - 1) * page.Rows;

    // Summary: Calculates how many rows of data to take in a given data page.
    private static int RowToTake(DataPage page) => page.Number * page.Rows;

}
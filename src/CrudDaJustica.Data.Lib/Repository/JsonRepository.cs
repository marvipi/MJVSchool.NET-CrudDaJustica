using CrudDaJustica.Data.Lib.Model;
using CrudDaJustica.Data.Lib.Service;
using System.Text.Json;

namespace CrudDaJustica.Data.Lib.Repository;

/// <summary>
/// Represents a repository that stores information in a JSON file.
/// </summary>
public class JsonRepository : IHeroRepository
{
    // Summary: The path of the json file where hero data is stored.
    private readonly string heroDataFilePath;

    // Summary: The path of the directory where the hero data file is stored.
    private readonly string heroDataDirPath;

    // Summary: A temporary file used to update or delete heroes from the repository.
    private string HeroDataTempFilePath => Path.Combine(heroDataDirPath, "heroTemp.json");

    public int RepositorySize { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonRepository"/> class.
    /// </summary>
    /// <param name="heroDataFilePath"> The absolute path where the hero data file is or will be stored. </param>
    /// <exception cref="ArgumentException"></exception>
    public JsonRepository(string heroDataFilePath)
    {
        var dirPath = Path.GetDirectoryName(heroDataFilePath);
        if (string.IsNullOrEmpty(dirPath))
        {
            var errorMsg = string.Format("{0} cannot be a root directory nor null. Given path: {1}",
                nameof(heroDataDirPath),
                heroDataDirPath);

            throw new ArgumentException(errorMsg);
        }

        heroDataDirPath = dirPath;
        if (!Directory.Exists(heroDataDirPath))
        {
            var heroDataDir = new DirectoryInfo(heroDataDirPath);
            heroDataDir.Create();
        }

        this.heroDataFilePath = heroDataFilePath;
        if (!File.Exists(this.heroDataFilePath))
        {
            var heroDataFile = new FileInfo(this.heroDataFilePath);
            heroDataFile
                .CreateText()
                .Close();
        }

        RepositorySize = File.ReadLines(this.heroDataFilePath).Count();
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
                                                                    .Cast<HeroEntity>()
                                                                    .ToList();

    public HeroEntity? GetHero(Guid id) => File.ReadLines(heroDataFilePath)
                                                .Select(line => JsonSerializer.Deserialize<HeroEntity>(line))
                                                .FirstOrDefault(he => he?.Id == id, null);

    public bool UpdateHero(Guid id, HeroEntity updatedHero) => OverwriteData(id, updatedHero);

    public bool DeleteHero(Guid id)
    {
        var success = OverwriteData(id);

        if (success)
        {
            RepositorySize--;
        }

        return success;
    }

    // Summary: Deletes or updates a hero whose id matches then given one.
    //			Copies all data from the current data file into a temporary one, altering it as necessary.
    //          Replaces the old data file with the temporary one, once the operation has completed.
    //
    // Remarks: If updatedInformation is null then the hero will be deleted from the file,
    //          otherwise they will be overwritten with updatedInformation.
    //          All other rows in the repository will be left untouched.
    private bool OverwriteData(Guid id, HeroEntity? updatedInformation = null)
    {
        var currentLine = 0;
        var dataRow = string.Empty;
        var fileChanged = false;

        using (var streamReader = new StreamReader(heroDataFilePath))
        {
            using var streamWriter = new StreamWriter(HeroDataTempFilePath);
            while ((dataRow = streamReader.ReadLine()) is not null)
            {
                var currentHero = JsonSerializer.Deserialize<HeroEntity>(dataRow);

                if (currentHero?.Id != id)
                {
                    streamWriter.WriteLine(dataRow);
                }
                else if (updatedInformation is null)
                {
                    fileChanged = true; // Delete
                }
                else if (updatedInformation is not null)
                {
                    var updatedHero = new HeroEntity()
                    {
                        Id = id,
                        Alias = updatedInformation.Alias,
                        Debut = updatedInformation.Debut,
                        FirstName = updatedInformation.FirstName,
                        LastName = updatedInformation.LastName,
                    };

                    var updatedHeroAsJson = JsonSerializer.Serialize(updatedHero);
                    streamWriter.WriteLine(updatedHeroAsJson); // Update

                    fileChanged = true;
                }

                currentLine++;
            }
        }

        File.Move(HeroDataTempFilePath, heroDataFilePath, true);
        return fileChanged;
    }

    // Summary: Calculates how many rows of data to skip to reach a given data page.
    private static int RowsToSkip(DataPage page) => (page.Number - 1) * page.Rows;

    // Summary: Calculates how many rows of data to take in a given data page.
    private static int RowToTake(DataPage page) => page.Number * page.Rows;
}
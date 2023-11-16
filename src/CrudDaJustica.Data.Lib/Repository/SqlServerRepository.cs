using CrudDaJustica.Data.Lib.Model;
using CrudDaJustica.Data.Lib.Service;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;

namespace CrudDaJustica.Data.Lib.Repository;

/// <summary>
/// Represents a SQL Server database that stores information about heroes.
/// </summary>
public class SqlServerRepository : IHeroRepository
{
    private readonly SqlConnection sqlConnection;

    // Summary: Counts how many heroes are registered in the database.
    // Remarks: Used to calculate the repository size.
    private readonly SqlCommand countHeroCommand;

    private const string COUNT_HERO =
        @"SELECT COUNT(*) as HeroRepositorySize
          FROM Hero;";

    private const string INSERT_HERO =
        @"INSERT INTO Hero 
                 (Id, Alias, Debut, FirstName, LastName)
          VALUES (@id, @alias, @debut, @firstName, @lastName);";

    private const string GET_HEROES_PAGED =
        @"SELECT Id, Alias, Debut, FirstName, LastName
          FROM Hero
          ORDER BY Alias
          OFFSET (@page - 1) * @rows ROWS
          FETCH NEXT @rows ROWS ONLY;";

    private const string GET_HERO =
        @"SELECT Id, Alias, Debut, FirstName, LastName
          FROM Hero
          WHERE Id = @id;";

    private const string UPDATE_HERO =
        @"UPDATE Hero
          SET Alias = @alias,
              Debut = @debut,
              FirstName = @firstName,
              LastName = @lastName
          WHERE Id = @id;";

    private const string DELETE_HERO =
        @"DELETE FROM Hero
          WHERE Id = @id;";

    public int RepositorySize
    {
        get
        {
            countHeroCommand.Connection.Open();
            var heroRepositorySize = (int)countHeroCommand.ExecuteScalar();
            countHeroCommand.Connection.Close();
            return heroRepositorySize;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerRepository"/> class.
    /// </summary>
    /// <param name="connectionString"> A connection string for a SQL Server database. </param>
    public SqlServerRepository(string connectionString)
    {
        sqlConnection = new(connectionString);
        countHeroCommand = new(COUNT_HERO, sqlConnection);

        sqlConnection.Open();
        Console.WriteLine("Opened connection to sql server");
        sqlConnection.Close();
        Console.WriteLine("Closed connection to sql server");
        Console.WriteLine("Connection test succeeded");
    }

    public bool RegisterHero(HeroEntity newHero)
    {
        var createHeroCommand = new SqlCommand(INSERT_HERO, sqlConnection);
        createHeroCommand.Parameters.AddRange(new SqlParameter[]
        {
            new("id", newHero.Id),
            new("alias", newHero.Alias),
            new("debut", newHero.Debut),
            new("firstName", newHero.FirstName),
            new("lastName", newHero.LastName),
        });

        createHeroCommand.Connection.Open();
        var amountOfHeroesCreated = createHeroCommand.ExecuteNonQuery();
        createHeroCommand.Connection.Close();

        return amountOfHeroesCreated > 0;
    }

    public IEnumerable<HeroEntity> GetHeroes(DataPage page)
    {
        var getHeroesCommand = new SqlCommand(GET_HEROES_PAGED, sqlConnection);
        getHeroesCommand.Parameters.AddRange(new SqlParameter[]
        {
            new("page", page.Number),
            new("rows", page.Rows),
        });

        getHeroesCommand.Connection.Open();

        var heroes = new List<HeroEntity>();
        using (var sqlReader = getHeroesCommand.ExecuteReader(CommandBehavior.CloseConnection))
        {
            while (sqlReader.Read())
            {
                heroes.Add(ReadHeroData(sqlReader));
            }
        }

        return heroes;
    }

    public HeroEntity? GetHero(Guid id)
    {
        var getHeroCommand = new SqlCommand(GET_HERO, sqlConnection);
        getHeroCommand.Parameters.AddWithValue("id", id);

        getHeroCommand.Connection.Open();

        HeroEntity? hero = null;
        using (var sqlReader = getHeroCommand.ExecuteReader(CommandBehavior.CloseConnection))
        {
            if (sqlReader.Read())
            {
                hero = ReadHeroData(sqlReader);
            }
        }

        return hero;
    }

    // Summary: Reads hero data from a SqlDataReader then uses it to initialize a new HeroEntity.
    // Remarks: Assumes hero data cannot be null.
    private static HeroEntity ReadHeroData(SqlDataReader sqlReader)
    {
        var idFromQuery = sqlReader.GetGuid(0);
        var alias = sqlReader.GetString(1);
        var debut = DateOnly.FromDateTime(sqlReader.GetDateTime(2));
        var firstName = sqlReader.GetString(3);
        var lastName = sqlReader.GetString(4);

        return new HeroEntity { Id = idFromQuery, Alias = alias, Debut = debut, FirstName = firstName, LastName = lastName };
    }

    public bool UpdateHero(Guid id, HeroEntity updatedHero)
    {
        var updateHeroCommand = new SqlCommand(UPDATE_HERO, sqlConnection);
        updateHeroCommand.Parameters.AddRange(new SqlParameter[]
        {
            new("alias", updatedHero.Alias),
            new("debut", updatedHero.Debut),
            new("firstName", updatedHero.FirstName),
            new("lastName", updatedHero.LastName),
            new("id", id),
        });

        updateHeroCommand.Connection.Open();
        var rowsUpdated = updateHeroCommand.ExecuteNonQuery();
        updateHeroCommand.Connection.Close();

        return rowsUpdated > 0;
    }

    public bool DeleteHero(Guid id)
    {
        var deleteHeroCommand = new SqlCommand(DELETE_HERO, sqlConnection);
        deleteHeroCommand.Parameters.AddWithValue("id", id);

        deleteHeroCommand.Connection.Open();
        var rowsDeleted = deleteHeroCommand.ExecuteNonQuery();
        deleteHeroCommand.Connection.Close();

        return rowsDeleted > 0;
    }

}

using CrudDaJustica.Cli.App.Controller;
using CrudDaJustica.Cli.App.View;
using CrudDaJustica.Data.Lib.Repository;
using CrudDaJustica.Data.Lib.Service;

var sqlServerUsername = Environment.GetEnvironmentVariable("MJVSCHOOLDB_USERNAME");
var sqlServerPassword = Environment.GetEnvironmentVariable("MJVSCHOOLDB_PASSWORD");
var connectionString = string.Format("Server=DESKTOP-GI663U1\\SQLEXPPERSONAL;Database=CrudDaJustica;User Id={0};Password={1};TrustServerCertificate=true;", sqlServerUsername, sqlServerPassword);

var heroRepository = new SqlServerRepository(connectionString);
var pagingService = new PagingService(heroRepository, PagingService.MIN_ROWS_PER_PAGE);
var heroController = new HeroController(heroRepository, pagingService);

var cli = new CLI(heroController);
cli.Start();


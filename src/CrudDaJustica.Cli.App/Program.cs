using CrudDaJustica.Cli.App.Controller;
using CrudDaJustica.Cli.App.View;
using CrudDaJustica.Data.Lib.Repository;
using CrudDaJustica.Data.Lib.Service;

const int INITIAL_ROWS_PER_PAGE = 10;

var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var heroDataFilePath = Path.Combine(appDataDirPath, "CRUD da Justica", "herodata.json");

var heroRepository = new JsonRepository(heroDataFilePath);
var pagingService = new PagingService(heroRepository, INITIAL_ROWS_PER_PAGE);
var heroController = new HeroController(heroRepository, pagingService);

var cli = new CLI(heroController);
cli.Start();


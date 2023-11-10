using CrudDaJustica.Cli.App.Controller;
using CrudDaJustica.Cli.App.Data;
using CrudDaJustica.Cli.App.Services;
using CrudDaJustica.Cli.App.View.UI;

const int INITIAL_ROWS_PER_PAGE = 10;

var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var heroDataFilePath = Path.Combine(appDataDirPath, "CRUD da Justica", "herodata.json");

var heroRepository = new JsonRepository(heroDataFilePath);
var pagingService = new PagingService(heroRepository, INITIAL_ROWS_PER_PAGE);
var heroController = new HeroController(heroRepository, pagingService);

var cli = new CLI(heroController);
cli.Start();


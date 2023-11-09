using PP_dotNet.Controller;
using PP_dotNet.Data;
using PP_dotNet.Services;
using PP_dotNet.View.UI;

const int INITIAL_PAGING_SIZE = 10;
const int INITIAL_ROWS_PER_PAGE = 10;

var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var heroDataFilePath = Path.Combine(appDataDirPath, "CRUD da Justica", "herodata.json");

IHeroRepository repo = new JsonRepository(heroDataFilePath);
var pagingService = new PagingService(INITIAL_PAGING_SIZE, INITIAL_ROWS_PER_PAGE);
var heroController = new HeroController(repo, pagingService);

var cli = new CLI(heroController);
cli.Start();


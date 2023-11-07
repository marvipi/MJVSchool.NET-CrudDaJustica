using PP_dotNet.Controller;
using PP_dotNet.Data;
using PP_dotNet.Services;
using PP_dotNet.View.UI;

const int INITIAL_REPO_SIZE = 10;
const int ROWS_PER_PAGE = 10;

IHeroRepository virtualRepo = new VirtualRepository(INITIAL_REPO_SIZE);
var pagingService = new PagingService(virtualRepo.RepositorySize, ROWS_PER_PAGE);
var heroController = new HeroController(virtualRepo, pagingService);

var cli = new CLI(heroController);
cli.Start();


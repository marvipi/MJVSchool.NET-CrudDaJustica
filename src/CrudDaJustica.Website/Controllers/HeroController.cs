using CrudDaJustica.Data.Lib.Model;
using CrudDaJustica.Data.Lib.Repository;
using CrudDaJustica.Data.Lib.Service;
using CrudDaJustica.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CrudDaJustica.Website.Controllers;

/// <summary>
/// Represents a controller that handles information about heroes.
/// </summary>
public class HeroController : Controller
{
    private readonly ILogger<HeroController> logger;
    private readonly IHeroRepository heroRepository;
    private readonly PagingService pagingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HeroController"/> class.
    /// </summary>
    /// <param name="logger"> A service responsible for logging the behavior of the website. </param>
    /// <param name="heroRepository"> The data repository that stores hero information. </param>
    /// <param name="pagingService"> The service responsible for data paging. </param>
    public HeroController(ILogger<HeroController> logger, IHeroRepository heroRepository, PagingService pagingService)
    {
        this.logger = logger;
        this.heroRepository = heroRepository;
        this.pagingService = pagingService;
    }

    /// <summary>
    /// Lists all heroes in a given data page.
    /// </summary>
    /// <returns> A view containing a list of heroes. </returns>
    public IActionResult Index(int page = PagingService.FIRST_PAGE, int rows = PagingService.MIN_ROWS_PER_PAGE)
    {
        logger.LogInformation("{timestamp}: displaying a list of heroes", DateTime.Now);

        pagingService.RowsPerPage = rows;
        pagingService.JumpToPage(page);

        // Used to display this page when another view redirects to here.
        TempData["Rows"] = pagingService.RowsPerPage;
        TempData["Page"] = pagingService.CurrentPage;

        var pageToList = pagingService.DataPage;

        var heroesInCurrentPage = heroRepository
            .GetHeroes(pageToList)
            .Select(he => new HeroViewModel(he.Id, he.Alias, he.Debut, he.FirstName, he.LastName));

        var heroListModel = new HeroListModel(heroesInCurrentPage, pagingService.PageRange, pageToList.Number);

        return View(heroListModel);
    }

    /// <summary>
    /// Opens the hero creation form.
    /// </summary>
    /// <returns> A view that allows you to create new heroes. </returns>
    [HttpGet]
    public IActionResult Create()
    {
        var emptyForm = new HeroFormModel(string.Empty, DateOnly.FromDateTime(DateTime.Today), string.Empty, string.Empty);
        return View(emptyForm);
    }

    /// <summary>
    /// Creates and registers a new hero.
    /// </summary>
    /// <param name="heroFormModel"> Hero data read from a form. </param>
    /// <returns> The hero creation form. </returns>
    [HttpPost]
    public IActionResult Create(HeroFormModel heroFormModel)
    {
        logger.LogInformation("{timestamp}: Attempting to create a new hero", DateTime.Now);

        if (ModelState.IsValid)
        {
            var newHero = new HeroEntity(heroFormModel.Alias, heroFormModel.Debut, heroFormModel.FirstName, heroFormModel.LastName);
            heroRepository.RegisterHero(newHero);
            logger.LogInformation("{timestamp}: Hero successfully created", DateTime.Now);
        }
        else
        {
            logger.LogWarning("{timestamp}: Failed to create a new hero", DateTime.Now);
        }

        return Create();
    }

    /// <summary>
    /// Opens the hero update form.
    /// </summary>
    /// <param name="id"> The id of the hero to update. </param>
    /// <returns> The hero update form pre-filled with information. </returns>
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        var hero = heroRepository.GetHero(id);

        if (hero is null)
        {
            logger.LogWarning("{timestamp}: there was an attempt at updating a non-registered hero", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        var heroFormModel = new HeroFormModel(hero.Alias, hero.Debut, hero.FirstName, hero.LastName);

        return View((heroFormModel, id));
    }

    /// <summary>
    /// Updates the information registered about a hero.
    /// </summary>
    /// <param name="heroFormModel"> New information about the hero. </param>
    /// <param name="id"> The id of the hero to update. </param>
    /// <returns> The hero update form. </returns>
    [HttpPost]
    public IActionResult Update(HeroFormModel heroFormModel, Guid id)
    {
        logger.LogInformation("{timestamp}: Attempting to update hero", DateTime.Now);

        if (ModelState.IsValid)
        {
            var updatedInformation = new HeroEntity
            {
                Alias = heroFormModel.Alias,
                Debut = heroFormModel.Debut,
                FirstName = heroFormModel.FirstName,
                LastName = heroFormModel.LastName,
            };

            if (heroRepository.UpdateHero(id, updatedInformation))
            {
                logger.LogInformation("{timestamp}: Hero successfully updated", DateTime.Now);
            }
            else
            {
                logger.LogWarning("{timestamp}: Failed to update hero", DateTime.Now);
            }
        }

        return View((heroFormModel, id));
    }

    /// <summary>
    /// Deletes a hero from the repository.
    /// </summary>
    /// <param name="id"> The unique identifier of the hero to delete. </param>
    /// <returns> A redirection to the Index page. </returns>
    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        logger.LogInformation("{timestamp}: Attempting to delete hero", DateTime.Now);

        if (heroRepository.DeleteHero(id))
        {
            logger.LogInformation("{timestamp}: Hero successfully deleted", DateTime.Now);
        }
        else
        {
            logger.LogWarning("{timestamp}: Failed to delete hero", DateTime.Now);
        }

        return RedirectToLastVisitedIndexPage();
    }

    /// <summary>
    /// Redirects to the page of the Index view last visited by the user.
    /// </summary>
    /// <returns> A <see cref="RedirectToActionResult"/> that points to last visited page of the index. </returns>
    public IActionResult RedirectToLastVisitedIndexPage()
    {
        return RedirectToIndexPage(Convert.ToInt32(TempData["Page"]));
    }

    /// <summary>
    /// Redirects to a page of the Index view, displaying the same amount of rows as before.
    /// </summary>
    /// <param name="page"> The page to redirect to. </param>
    /// <returns> A <see cref="RedirectToActionResult"/> that points to a specific page of the Index. </returns>
    public IActionResult RedirectToIndexPage(int page)
    {
        return RedirectToAction(nameof(Index), new { page, rows = Convert.ToInt32(TempData["Rows"]) });
    }

    /// <summary>
    /// Changes the amount of rows being displayed in the Index.
    /// </summary>
    /// <param name="rows"> The amount of rows to display from now on. </param>
    /// <returns> A <see cref="RedirectToActionResult"/> that points to last visited page of the index. </returns>
    public IActionResult UpdateRows(int rows)
    {
        TempData["Rows"] = rows;
        return RedirectToLastVisitedIndexPage();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

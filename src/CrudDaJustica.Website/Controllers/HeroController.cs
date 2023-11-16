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
    public IActionResult Index(int page = 1)
    {
        logger.LogInformation("Displaying a list of heroes");

        pagingService.JumpToPage(page);
        var pageToList = pagingService.GetCurrentPage();

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
        logger.LogInformation("Attempting to create a new hero");

        if (ModelState.IsValid)
        {
            var newHero = new HeroEntity(heroFormModel.Alias, heroFormModel.Debut, heroFormModel.FirstName, heroFormModel.LastName);
            heroRepository.RegisterHero(newHero);
            logger.LogInformation("Hero successfully created");
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
            logger.LogWarning("{timestamp}: there was an attempt at updating a non-registered hero", DateTime.Now.ToString());
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
        logger.LogInformation("Attempting to update a hero");

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
                logger.LogInformation("Hero successfully updated");
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
        logger.LogInformation("Attempting to delete a hero");

        if (heroRepository.DeleteHero(id))
        {
            logger.LogInformation("Hero successfully deleted");
        }

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

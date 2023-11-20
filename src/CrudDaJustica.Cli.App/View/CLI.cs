using CrudDaJustica.Cli.App.Controller;
using CrudDaJustica.Cli.App.Model;
using CrudDaJustica.Cli.Lib.Decoration;
using CrudDaJustica.Cli.Lib.Window;

namespace CrudDaJustica.Cli.App.View;

/// <summary>
/// Represents a command-line interface used to interact with this system.
/// </summary>
public class CLI
{
    private readonly HeroController heroController;

    // Summary: A View that lists heroes in the console window.
    private readonly Listing<HeroViewModel> heroListing;

    // Summary: A form used to register new heroes.
    private readonly Form<HeroFormModel> heroCreateForm;

    // Summary: A form used to change information about a registered hero.
    private readonly Form<HeroFormModel> heroUpdateForm;

    // Summary: The hero currently selected in the heroListing.
    private HeroViewModel CurrentlySelectedHero => Enumerable.ElementAt(heroListing.Elements, heroListing.CurrentlySelectedElement);

    /// <summary>
    /// Initializes a new instance of the <see cref="CLI"/> class.
    /// </summary>
    /// <param name="heroController"> The controller responsible for handling information about the heroes. </param>
    public CLI(HeroController heroController)
    {
        this.heroController = heroController;

        // WARNING: Boilerplate ahead!

        var formHeader = new Header(new string[]
        {
            "  @@@@@ @@      Welcome, J'ONN J'ONZZ",
            "     @@ @@      Role: ADMIN",
            "  @  @@ @@      ",
            " @@  @@ @@      ",
            " @@@@@@ @@@@@@  Press ENTER to advance to the next field",
        });

        var createHeroFormFrame = new Frame("MONITOR_WOMB::MAINFRAME::HOME/HEROES/CREATE");
        heroCreateForm = new Form<HeroFormModel>(createHeroFormFrame, formHeader, heroController.Validate);
        heroCreateForm.AddKeybindings(
            new(ConsoleKey.Escape, heroCreateForm.Cancel, "ESC: Cancel"),
            new(ConsoleKey.Enter, OnCreateHero, "ENTER: Confirm"),
            new(ConsoleKey.Spacebar, heroCreateForm.Retry, "SPACE: Retry"));

        var updateHeroFormFrame = new Frame("MONITOR_WOMB::MAINFRAME::HOME/HEROES/UPDATE");
        heroUpdateForm = new Form<HeroFormModel>(updateHeroFormFrame, formHeader, heroController.Validate);
        heroUpdateForm.AddKeybindings(
            new(ConsoleKey.Escape, heroUpdateForm.Cancel, "ESC: Cancel"),
            new(ConsoleKey.Enter, OnUpdateHero, "ENTER: Confirm"),
            new(ConsoleKey.Spacebar, heroUpdateForm.Retry, "SPACE: Retry"));


        var heroListingFrame = new Frame("MONITOR_WOMB::MAINFRAME::HOME/HEROES");
        var heroListingHeader = new Header(new string[]
        {
            "  @@@@@ @@      Welcome, J'ONN J'ONZZ",
            "     @@ @@      Role: ADMIN",
            "  @  @@ @@      ",
            " @@  @@ @@      ",
            " @@@@@@ @@@@@@  ",
        });

        heroListing = new Listing<HeroViewModel>(heroListingFrame, heroListingHeader);
        heroListing.AddKeybindings(new Keybinding[]
        {
            new(ConsoleKey.Escape, heroListing.Exit, "ESC: Exit"),
            new(ConsoleKey.C, heroCreateForm.Display, "C: Create"),
            new(ConsoleKey.U, heroUpdateForm.Display, "U: Update"),
            new(ConsoleKey.D, OnDeleteHero, "D: Delete"),
            new(ConsoleKey.RightArrow, OnNextHeroPage, "RIGHT: Next page"),
            new(ConsoleKey.LeftArrow, OnPreviousHeroPage, "LEFT: Previous page"),
            new(ConsoleKey.DownArrow, heroListing.Next, "DOWN: Next element"),
            new(ConsoleKey.UpArrow, heroListing.Previous, "UP: Previous element"),
        });
    }

    /// <summary>
    /// Starts the command-line interface.
    /// </summary>
    public void Start()
    {
        UpdateListing();
        heroListing.Display();
        Console.Clear();
    }

    // Summary: Advances to the next data page and updates the contents of the hero listing.
    private void OnNextHeroPage()
    {
        heroController.NextPage();
        UpdateListing();
    }

    // Summary: Returns to the previous data page and updates the contents of the hero listing.
    private void OnPreviousHeroPage()
    {
        heroController.PreviousPage();
        UpdateListing();
    }

    // Summary: Updates the elements displayed in the hero listing and the number of the data page being displayed.
    private void UpdateListing()
    {
        heroListing.Elements = heroController.List();
        heroListing.CurrentPage = heroController.CurrentPage;
    }

    // Summary: Creates a new hero from the form data read from the heroCreateForm.
    private void OnCreateHero()
    {
        heroController.Create(heroCreateForm.FormData);
        UpdateListing();
    }

    // Summary: Updates the currently selected hero in the heroListing with data read from the heroUpdateForm.
    private void OnUpdateHero()
    {
        if (heroListing.Elements.Any())
        {
            heroController.Update(CurrentlySelectedHero.Id, heroUpdateForm.FormData);
            UpdateListing();
        }
    }

    // Summary: Deletes the currently selected hero in the heroListing.
    private void OnDeleteHero()
    {
        if (heroListing.Elements.Any())
        {
            heroController.Delete(CurrentlySelectedHero.Id);
            UpdateListing();
        }
    }
}

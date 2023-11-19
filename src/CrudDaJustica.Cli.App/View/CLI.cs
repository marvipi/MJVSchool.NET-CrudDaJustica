using CrudDaJustica.Cli.App.Controller;
using CrudDaJustica.Cli.App.Model;
using CrudDaJustica.Cli.Lib.Decoration;
using CrudDaJustica.Cli.Lib.Decoration.Header;
using CrudDaJustica.Cli.Lib.Forms;
using CrudDaJustica.Cli.Lib.Keybindings;
using CrudDaJustica.Cli.Lib.Views;

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

        var formHeader = new string[]
        {
            "  @@@@@ @@      Welcome, J'ONN J'ONZZ",
            "     @@ @@      Role: ADMIN",
            "  @  @@ @@      ",
            " @@  @@ @@      ",
            " @@@@@@ @@@@@@  Press ENTER to advance to the next field",
        };

        heroCreateForm = new Form<HeroFormModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES/CREATE",
            formHeader,
            new BindableKey(ConsoleKey.Escape, "ESC: Cancel"),
            new Keybinding(OnCreateHero, ConsoleKey.Enter, "ENTER: Confirm"),
            new BindableKey(ConsoleKey.Spacebar, "SPACE: Retry"),
            heroController.Validate);


        heroUpdateForm = new Form<HeroFormModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES/UPDATE",
            formHeader,
            new BindableKey(ConsoleKey.Escape, "ESC: Cancel"),
            new Keybinding(OnUpdateHero, ConsoleKey.Enter, "ENTER: Confirm"),
            new BindableKey(ConsoleKey.Spacebar, "SPACE: Retry"),
            heroController.Validate);

        var listingHeader = new string[]
        {
            "  @@@@@ @@      Welcome, J'ONN J'ONZZ",
            "     @@ @@      Role: ADMIN",
            "  @  @@ @@      ",
            " @@  @@ @@      ",
            " @@@@@@ @@@@@@  ",
        };

        heroListing = new Listing<HeroViewModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES",
            listingHeader,
            new BindableKey(ConsoleKey.Escape, "ESC: Exit"),
            new Keybinding(heroCreateForm.Display, ConsoleKey.C, "C: Create"),
            new Keybinding(heroUpdateForm.Display, ConsoleKey.U, "U: Update"),
            new Keybinding(OnDeleteHero, ConsoleKey.D, "D: Delete"),
            new Keybinding(OnNextHeroPage, ConsoleKey.RightArrow, "RIGHT: Next page"),
            new Keybinding(OnPreviousHeroPage, ConsoleKey.LeftArrow, "LEFT: Previous page"),
            new BindableKey(ConsoleKey.DownArrow, "DOWN: Next element"),
            new BindableKey(ConsoleKey.UpArrow, "UP: Previous element"));
    }

    /// <summary>
    /// Starts the command-line interface.
    /// </summary>
    public void Start()
    {
        //UpdateListing();
        //heroListing.Display();
        var frame = new Cli.Lib.Decoration.Border.Frame("MONITOR_WOMB::MAINFRAME::HOME/HEROES");
        frame.Display();
        var header = new Header(new string[] {
            "  @@@@@ @@      Welcome, J'ONN J'ONZZ",
            "     @@ @@      Role: ADMIN",
            "  @  @@ @@      ",
            " @@  @@ @@      ",
            " @@@@@@ @@@@@@  ",
        });
        header.Display();
        //Console.Clear();
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

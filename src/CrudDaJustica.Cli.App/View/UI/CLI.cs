using CrudDaJustica.Cli.App.Controller;
using CrudDaJustica.Cli.App.View.Models;
using CrudDaJustica.Cli.Lib.Forms;
using CrudDaJustica.Cli.Lib.Keybindings;
using CrudDaJustica.Cli.Lib.Views;
using System.Text;

namespace CrudDaJustica.Cli.App.View.UI;

/// <summary>
/// Represents a command-line interface used to interact with this system.
/// </summary>
public class CLI
{
    private readonly HeroController heroController;
    private readonly Listing<HeroViewModel> heroListing;
    private readonly Form<HeroFormModel> heroCreateForm;
    private readonly Form<HeroFormModel> heroUpdateForm;

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
            new Keybinding(OnCreateHero, ConsoleKey.Enter, "ENTER: Confirm"));


        heroUpdateForm = new Form<HeroFormModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES/UPDATE",
            formHeader,
            new BindableKey(ConsoleKey.Escape, "ESC: Cancel"),
            new Keybinding(OnUpdateHero, ConsoleKey.Enter, "ENTER: Confirm"));

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
            heroController.List,
            new BindableKey(ConsoleKey.Escape, "ESC: Exit"),
            new Keybinding(heroCreateForm.Display, ConsoleKey.C, "C: Create"),
            new Keybinding(heroUpdateForm.Display, ConsoleKey.U, "U: Update"),
            new Keybinding(OnDeleteHero, ConsoleKey.D, "D: Delete"),
            new Keybinding(heroController.NextPage, ConsoleKey.RightArrow, "RIGHT: Next page"),
            new Keybinding(heroController.PreviousPage, ConsoleKey.LeftArrow, "LEFT: Previous page"),
            new BindableKey(ConsoleKey.DownArrow, "DOWN: Next element"),
            new BindableKey(ConsoleKey.UpArrow, "UP: Previous element"));
    }

    /// <summary>
    /// Starts the command-line interface.
    /// </summary>
    public void Start()
    {
        heroListing.Display();
        Console.Clear();
    }

    private void OnCreateHero()
    {
        heroController.Create(heroCreateForm.FormData);
    }

    private void OnUpdateHero()
    {
        heroController.Update(heroUpdateForm.FormData, heroListing.CurrentlySelectedElement);
    }

    private void OnDeleteHero()
    {
        heroController.Delete(heroListing.CurrentlySelectedElement);
    }
}

using PP_dotNet.Controller;
using PP_dotNet.View.Forms;
using PP_dotNet.View.Keybindings;
using System.Text;

namespace PP_dotNet.View.UI;

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

        var formHeaderContent = new StringBuilder()
            .AppendLine(" USER: J'ONN J'ONZZ")
            .AppendLine(" Press ENTER to advance to the next field");
        var formHeader = new Header(formHeaderContent);

        heroCreateForm = new Form<HeroFormModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES/CREATE",
            '=',
            formHeader,
            new RebindableKey(ConsoleKey.Escape, "ESC: Cancel"),
            new Keybinding(OnCreateHero, ConsoleKey.Enter, "ENTER: Confirm"));


        heroUpdateForm = new Form<HeroFormModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES/UPDATE",
            '=',
            formHeader,
            new RebindableKey(ConsoleKey.Escape, "ESC: Cancel"),
            new Keybinding(OnUpdateHero, ConsoleKey.Enter, "ENTER: Confirm"));


        var listingHeaderContents = new StringBuilder()
            .AppendLine(" USER: J'ONN J'ONZZ");
        var listingHeader = new Header(listingHeaderContents);

        heroListing = new Listing<HeroViewModel>(
            "MONITOR_WOMB::MAINFRAME::HOME/HEROES",
            '=',
            listingHeader,
            heroController.List,
            new RebindableKey(ConsoleKey.Escape, "ESC: Exit"),
            new Keybinding(heroCreateForm.Display, ConsoleKey.C, "C: Create"),
            new Keybinding(heroUpdateForm.Display, ConsoleKey.U, "U: Update"),
            new Keybinding(OnDeleteHero, ConsoleKey.D, "D: Delete"),
            new Keybinding(heroController.NextPage, ConsoleKey.RightArrow, "RIGHT: Next page"),
            new Keybinding(heroController.PreviousPage, ConsoleKey.LeftArrow, "LEFT: Previous page"),
            new RebindableKey(ConsoleKey.DownArrow, "DOWN: Next element"),
            new RebindableKey(ConsoleKey.UpArrow, "UP: Previous element"));
    }

    /// <summary>
    /// Starts the command-line interface.
    /// </summary>
    public void Start()
    {
        heroListing.Display();
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

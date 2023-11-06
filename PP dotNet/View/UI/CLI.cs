using PP_dotNet.Controller;
using PP_dotNet.View.Builders;
using PP_dotNet.View.Forms;

namespace PP_dotNet.View.UI;

/// <summary>
/// Represents a command-line interface used to interact with this system.
/// </summary>
public class CLI
{
    private readonly HeroController heroController;

    /// <summary>
    /// Initializes a new instance of the <see cref="CLI"/> class.
    /// </summary>
    /// <param name="heroController"> The controller responsible for handling information about the heroes. </param>
    public CLI(HeroController heroController)
    {
        this.heroController = heroController;
    }

    /// <summary>
    /// Starts the command-line interface.
    /// </summary>
    public void Start()
    {

        var header = new ViewBuilder()
            .AppendLine("ADMIN :: J'ONN J'ONNZ")
            .BindExitKey(ConsoleKey.Escape, string.Empty)
            .Build();

        var heroForm = new FormBuilder<HeroFormModel>()
            .AddHeader(header)
            .BindCancelKey(ConsoleKey.Escape, "ESC: Cancel")
            .BindConfirmKey(ConsoleKey.Enter, "ENTER: Confirm")
            .BindNextKey(ConsoleKey.Enter, "ENTER: Next")
            .BindCreateTo(heroController.Create)
            .BindUpdateTo(heroController.Update)
            .Build();

        var heroListing = new ListingBuilder<HeroViewModel>()
            .AddHeader(header)
            .BindExitKey(ConsoleKey.Escape, "ESC: Exit")
            .AddRetriever(heroController.List)
            .BindNextPageKey(ConsoleKey.RightArrow, "RIGHT: Next page", heroController.NextPage)
            .BindPreviousPageKey(ConsoleKey.LeftArrow, "LEFT: Previous page", heroController.PreviousPage)
            .BindNextElementKey(ConsoleKey.DownArrow, "DOWN: Next element")
            .BindPreviousElementKey(ConsoleKey.UpArrow, "UP: Previous element")
            .BindSelector(ConsoleKey.D, "D: Delete", heroController.Delete)
            .BindKey(ConsoleKey.C, "C: Create", heroForm.Create)
            .BindSelector(ConsoleKey.U, "U: Update", heroForm.Update)
            .Build();

        heroListing.Display();
    }
}

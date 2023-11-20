namespace CrudDaJustica.Cli.Lib.Window;

/// <summary>
/// Represents a view displayed in the console.
/// </summary>
public abstract class Window : IDisplayable
{
    private readonly IDisplayable frame;
    private readonly IDisplayable header;

    // Summary: Whether the window should be closed at the start of the next frame.
    protected bool ShouldExit;

    protected Window(IDisplayable frame, IDisplayable header)
    {
        this.frame = frame;
        this.header = header;
    }

    public virtual void Display()
    {
        Console.Clear();
        frame.Display();
        header.Display();
    }
}

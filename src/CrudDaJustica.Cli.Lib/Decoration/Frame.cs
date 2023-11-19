namespace CrudDaJustica.Cli.Lib.Decoration;

/// <summary>
/// Represents the borders around a console window.
/// </summary>
public class Frame : IDisplayable
{
    private readonly BorderUpper upperBorder;
    private readonly BorderLeft leftBorder;
    private readonly BorderRight rightBorder;
    private readonly BorderLower lowerBorder;

    /// <summary>
    /// Initializes a new instance of the <see cref="Frame"/> class.
    /// </summary>
    /// <param name="title"> The title of the window. </param>
    public Frame(string title)
    {
        upperBorder = new BorderUpper(title);
        leftBorder = new BorderLeft();
        rightBorder = new BorderRight();
        lowerBorder = new BorderLower();
    }

    public void Display()
    {
        upperBorder.Display();
        leftBorder.Display();
        rightBorder.Display();
        lowerBorder.Display();
    }
}

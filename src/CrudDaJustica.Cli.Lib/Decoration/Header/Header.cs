namespace CrudDaJustica.Cli.Lib.Decoration.Header;

/// <summary>
/// Represents static information to display on the upper part of a console window.
/// </summary>
public class Header : IDisplayable
{
    private readonly IEnumerable<string> contents;
    private readonly Separator separator;

    public Header(IEnumerable<string> contents)
    {
        this.contents = contents;
        separator = new Separator();
    }

    public void Display()
    {
        var row = 1;
        foreach (var content in contents)
        {
            Console.SetCursorPosition(1, row);
            Console.Write(content);
            row++;
        }
        separator.Display();
    }
}

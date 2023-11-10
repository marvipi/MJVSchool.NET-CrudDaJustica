using System.Text;

namespace CrudDaJustica.Cli.Lib.Views;

/// <summary>
/// Represents a screen in the user interface.
/// </summary>
public abstract class Frame : View
{
	// Summary: A text to display on the center of the top border.
	private readonly string title;

	// Summary: Information to display on the upper console window.
	private readonly string[] header;

	// Summary: The amount of corners on each end of a horizontal line.
	// Remarks: Used for drawing horizontal lines with corners.
	private const int AMOUNT_OF_CORNERS = 2;

	// Summary: The width of horizontal line taking the amount of corners into account.
	private static int LineWidth => Console.BufferWidth - AMOUNT_OF_CORNERS;


	protected Frame(string title, string[] header) : base()
	{
		this.title = title;
		this.header = header;
	}

	/// <summary>
	/// Draws the upper border, the header, a separator and the lower border.
	/// </summary>
	public override void Display()
	{
		Console.Clear();
		base.Display();

		DrawUpperBorder();
		DrawHeader();
		var rowAfterHeader = Console.GetCursorPosition().Top;
		DrawSeparator(rowAfterHeader);
		DrawLowerBorder();

		Console.SetCursorPosition(left: 0, rowAfterHeader + 1);
	}

	// Summary: Displays all keybindings in a single line of the console window.
	protected void DisplayKeybindings()
	{
		var keybindingDisplay = new StringBuilder();
		foreach (var keybinding in Keybindings)
		{
			keybindingDisplay.AppendFormat(" {0} ", keybinding);
		}

		DrawVerticalBorders(keybindingDisplay.ToString(), Console.WriteLine);
		DrawVerticalBorders(string.Empty, Console.WriteLine);
	}

	// Summary: Draws vertical borders around a given line, padding it to the width of the console buffer.
	//			Expects either Console.WriteLine or Console.Write as consoleWriter.
	protected static void DrawVerticalBorders(string line, Action<string> consoleWriter)
	{
		var lineWithBorders = string.Format("│{0}│", line.PadRight(LineWidth));
		consoleWriter.Invoke(lineWithBorders);
	}

	// Summary: Draws vertical borders starting from this row down to the penultimate row of the console buffer.
	protected static void DrawVerticalBorders()
	{

		int startingRow = Console.GetCursorPosition().Top;
		int penultimateRow = Console.BufferHeight - 2;
		foreach (var _ in Enumerable.Range(startingRow, penultimateRow - startingRow))
		{
			DrawVerticalBorders(string.Empty, Console.WriteLine);
		}
	}

	// Summary: Draws a line along the first row of the console buffer, placing downward corners on each end.
	//          Writes the title in the middle of the row.
	private void DrawUpperBorder()
	{
		var title = string.Format("┤ {0} ├", this.title.Trim());

		var leftHalfWidth = (Console.BufferWidth + title.Length) / 2;
		var rightHalfWidth = Console.BufferWidth;

		var upperBorderWithoutCorners = title
			.PadLeft(leftHalfWidth - AMOUNT_OF_CORNERS, '─')
			.PadRight(rightHalfWidth - AMOUNT_OF_CORNERS, '─');

		Console.WriteLine("┌{0}┐", upperBorderWithoutCorners);
	}

	// Summary: Draws each line in the header between vertical borders.
	private void DrawHeader()
	{
		foreach (var line in header)
		{
			DrawVerticalBorders(line, Console.WriteLine);
		}
	}

	// Summary: Draw a separator line in a given row of the console.
	private static void DrawSeparator(int row)
	{
		DrawWithCorners(row, "├", '┤');
	}

	// Summary: Draws a line across the penultimate row of the console buffer, placing upward corners on each end.
	// Remarks: Drawing on the last row causes a scroll bar to appear.
	private static void DrawLowerBorder()
	{
		var penultimateRow = Console.BufferHeight - 2;
		DrawWithCorners(penultimateRow, "└", '┘');
	}

	// Summary: Draw a line across a given row of the console buffer, placing corners on each end.
	private static void DrawWithCorners(int row, string leftCorner, char rightCorner)
	{
		var line = string.Empty.PadRight(LineWidth, '─');
		var separator = string.Format("{0}{1}{2}", leftCorner, line, rightCorner);

		Console.SetCursorPosition(0, row);
		Console.WriteLine(separator);
	}
}

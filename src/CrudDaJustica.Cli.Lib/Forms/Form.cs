using CrudDaJustica.Cli.Lib.Keybindings;
using CrudDaJustica.Cli.Lib.Views;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CrudDaJustica.Cli.Lib.Forms;

/// <summary>
/// Represents a form that generates fields automatically for a <typeparamref name="T"/> and uses them to read input from the user.
/// </summary>
/// <typeparam name="T"> The type of the class used to generate the fields. </typeparam>
public class Form<T> : Frame where T : new()
{
	// Summary: The fields that have not been read yet.
	private readonly Queue<Field> fields;

	// Summary: All public properties of T.
	// Remarks: Used to generate forms dynamically.
	private readonly Queue<PropertyInfo> properties;

	/// <summary>
	/// The data read from the user.
	/// </summary>
	/// <remarks>
	/// The contents of of a form are overwritten every time it is displayed.
	/// </remarks>
	public T FormData { get; init; }

	// Summary: The key that will trigger an event that utilizes the form data.
	private readonly Keybinding confirmKey;

	/// <summary>
	/// Initializes a new instance of the <see cref="Form{T}"/> class.
	/// </summary>
	/// <param name="title"> The title to display on top of the screen. </param>
	/// <param name="cancelKey"> An unbound console key that will discard all that in the the form. </param>
	/// <param name="header"> Information to display on the top of the console buffer. </param>
	/// <param name="confirmKey"> An unbound console key that will save the data in the form. </param>
	public Form(string title,
		string[] header,
		BindableKey cancelKey,
		Keybinding confirmKey) : base(title, header)
	{
		fields = new();
		properties = new();
		FormData = new();
		this.confirmKey = confirmKey;
		var keybindings = new List<Keybinding>()
		{
			cancelKey.Bind(Exit),
			confirmKey, // Used to display the keybinding when the confirmation prompt is shown.
        };
		Keybindings.AddRange(keybindings);
	}

	/// <summary>
	/// Displays this form on the console window.
	/// </summary>
	public override void Display()
	{
		base.Display();

		InitializeFields();

		var screenCoords = DrawFields();

		ReadAllFields(screenCoords);

		DisplayConfirmationPrompt();
	}

	// Summary: Creates a field for each public property in T.
	private void InitializeFields()
	{
		foreach (var prop in FormData.GetType().GetProperties())
		{
			var formattedPropName = FormatPropName(prop.Name);
			fields.Enqueue(new Field(title: formattedPropName));
			properties.Enqueue(prop);
		}
	}

	// Summary: Splits propName by uppercase characters. Converts every word to lowercase, except for the first one.
	private string FormatPropName(string propName)
	{
		var fieldTitle = Regex
				.Split(propName, @"([A-Z]+[a-z]*)")
				.Where(word => !string.IsNullOrWhiteSpace(word))
				.ToArray();

		if (fieldTitle.Length == 1)
		{
			return propName;
		}

		var formattedFieldTitle = fieldTitle[0];
		foreach (var i in Enumerable.Range(1, fieldTitle.Length - 1))
		{
			formattedFieldTitle += string.Format(" {0}", fieldTitle[i].ToLower());
		}

		return formattedFieldTitle;
	}

	// Summary: Displays the fields vertically on the console screen.
	// Returns: The console coordinates right after each field.
	private Queue<(int column, int row)> DrawFields()
	{
		var coords = new Queue<(int column, int row)>();

		foreach (var field in fields)
		{
			var displayText = string.Format(" {0}: ", field.Title);
			DrawVerticalBorders(displayText, Console.Write);

			(var column, var row) = Console.GetCursorPosition();
			Console.SetCursorPosition(displayText.Length + 1, row);
			coords.Enqueue(Console.GetCursorPosition());

			Console.WriteLine();
		}

		DrawVerticalBorders();

		return coords;
	}

	// Summary: Reads input from each field in the form from top to bottom.
	private void ReadAllFields(Queue<(int column, int row)> screenCoords)
	{
		Console.CursorVisible = true;
		while (screenCoords.Any())
		{
			(var column, var row) = screenCoords.Peek();
			Console.SetCursorPosition(column, row);
			ReadField();
			screenCoords.Dequeue();
		}
		Console.CursorVisible = false;
	}

	// Summary: Reads input from the current field of the user form.
	private void ReadField()
	{
		var currentField = fields.Peek();
		currentField.Read();

		properties
			.Peek()
			.SetValue(FormData, currentField.Value);

		fields.Dequeue();
		properties.Dequeue();
	}

	// Summary: Prompts the user to save or discard the data entered in the form.
	private void DisplayConfirmationPrompt()
	{
		DisplayKeybindings();
		while (!ExitKeyPressed)
		{
			var input = Console.ReadKey(true).Key;
			if (input == confirmKey.Key)
			{
				confirmKey.Invoke();
				Exit();
			}
			else
			{
				Invoke(input);
			}
		}
		ExitKeyPressed = false;
	}
}
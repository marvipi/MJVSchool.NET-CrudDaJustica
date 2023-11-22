using System.Reflection;
using System.Text.RegularExpressions;

namespace CrudDaJustica.Cli.Lib.Windows;

/// <summary>
/// Represents a form that generates fields automatically for a <typeparamref name="T"/> and uses them to read input from the user.
/// </summary>
/// <typeparam name="T"> The type of the class used to generate the fields. </typeparam>
public class Form<T> : Window where T : new()
{
    // Summary: The fields that have not been read yet.
    private readonly Queue<Field> fields;

    // Summary: All public properties of T.
    // Remarks: Used to generate forms dynamically.
    private readonly Queue<PropertyInfo> properties;

    // Summary: Used to discard all data and exit the form.
    private Keybinding cancel = null!;

    // Summary: The key that will trigger an event that utilizes the form data.
    private Keybinding confirm = null!;

    // Summary: Used to redisplay the form after validation problems occur.
    private Keybinding retry = null!;

    // Summary: Delegate that retrieves the problems from the lastest form data read from the user.
    private readonly Func<T, IEnumerable<string>> validationProblemsRetriver;

    /// <summary>
    /// The data read from the user.
    /// </summary>
    /// <remarks>
    /// The contents of of a form are overwritten every time it is displayed.
    /// </remarks>
    public T FormData { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Form{T}"/> class.
    /// </summary>
    /// <param name="frame"> The borders around a form. </param>
    /// <param name="header"> Information to display above the form. </param>
    /// <param name="validationProblemsRetriver"> Delegate that retrieves the problems from the lastest form data read from the user. </param>
    public Form(IDisplayable frame, IDisplayable header, Func<T, IEnumerable<string>> validationProblemsRetriver) : base(frame, header)
    {
        fields = new();
        properties = new();
        FormData = new();
        this.validationProblemsRetriver = validationProblemsRetriver;
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

        DisplayCorrectPrompt();
    }

    /// <summary>
    /// Adds the keybindings required for this form to function.
    /// </summary>
    /// <param name="cancel"> Used to discard all that and exit the form. </param>
    /// <param name="confirm"> Used to submit form data and exit the form. </param>
    /// <param name="retry"> Used to retry filling up the form when validation problems arise. </param>
    public void AddKeybindings(Keybinding cancel, Keybinding confirm, Keybinding retry)
    {
        this.cancel = cancel;
        this.confirm = confirm;
        this.retry = retry;
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

        var currentRow = Console.GetCursorPosition().Top;
        foreach (var field in fields)
        {
            currentRow++;
            var displayText = string.Format(" {0}: ", field.Title);
            Console.SetCursorPosition(1, currentRow);
            Console.Write(displayText);

            Console.SetCursorPosition(displayText.Length + 1, currentRow);
            coords.Enqueue(Console.GetCursorPosition());

            Console.WriteLine();

            currentRow++;
        }

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

    // Summary: Displays the retry prompt if validation problems are found. Otherwise, displays the confirmation prompt.
    private void DisplayCorrectPrompt()
    {
        var hasProblems = DisplayValidationProblems();
        if (hasProblems)
        {
            DisplayRetryPrompt();
        }
        else
        {
            DisplayConfirmationPrompt();
        }
    }

    // Summary: Displays each validation problem in a different line.
    private bool DisplayValidationProblems()
    {
        var validationProblems = validationProblemsRetriver.Invoke(FormData);
        bool hasValidationProblems;

        if (hasValidationProblems = validationProblems.Any())
        {
            var currentRow = Console.GetCursorPosition().Top;
            Console.SetCursorPosition(1, currentRow++); // Empty line between fields and validation problems

            foreach (var validationProblem in validationProblems)
            {
                var formattedValidationProblem = string.Format(" {0} ", validationProblem);
                Console.Write(" {0} ", formattedValidationProblem);

                Console.SetCursorPosition(1, currentRow++);
            }
        }

        return hasValidationProblems;
    }

    // Summary: Prompts the user to try again or to discard the data entered in the form.
    // Remarks: The cancel is invoked before the form is displayed again, assuring only one form is displayed at a time.
    private void DisplayRetryPrompt() => DisplayPrompt(retry, cancel, retry);

    // Summary: Prompts the user to save or discard the data entered in the form.
    // Remarks: The cancel is invoked after the form data is submitted, to exit the form.
    private void DisplayConfirmationPrompt() => DisplayPrompt(confirm, confirm, cancel);

    // Summary: Displays the cancel key on the left and the first key on the right.
    //			If firstKey is pressed then invokes the actions associated with the second and third keys, respectively.
    private void DisplayPrompt(Keybinding firstKey, Keybinding secondKey, Keybinding thirdKey)
    {
        Console.SetCursorPosition(1, Console.GetCursorPosition().Top + 1); // Empty line between previous content and prompt

        var prompt = string.Format(" {0}    {1} ", cancel, firstKey);
        Console.Write(prompt);

        ShouldExit = false;

        while (!ShouldExit)
        {
            var input = Console.ReadKey(true).Key;
            if (input == firstKey.Key)
            {
                secondKey.Invoke();
                thirdKey.Invoke();
            }
            else if (input == cancel.Key)
            {
                cancel.Invoke();
            }
        }
    }

    /// <summary>
    /// Signals the form to exit at the start of the next frame.
    /// </summary>
    public void Cancel() => Exit();

    /// <summary>
    /// Starts the form-filling process from the beginning.
    /// </summary>
    public void Retry()
    {
        Exit();
        Display();
    }

    // Summary: Signals the form to exit at the start of the next frame.
    private void Exit() => ShouldExit = true;
}
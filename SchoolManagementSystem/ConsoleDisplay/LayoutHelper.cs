using System.Globalization;
using ProjectHelperLibrary.Validations;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.Named;
using Spectre.Console;

namespace SchoolManagementSystem.ConsoleDisplay;

public static class LayoutHelper
{
    private const string AppTitle = "School Management System";
    private const string AppSubtitle = "Academic Administration Platform";

    // ─── Unauthenticated Screen ────────────────────────────────────────────

    public static void RenderWelcomeScreen()
    {
        AnsiConsole.Clear();

        var titleContent = new Markup(
            $"[bold steelblue1]{AppTitle}[/]\n[grey]{AppSubtitle}[/]"
        );

        var panel = new Panel(Align.Center(titleContent, VerticalAlignment.Middle))
            .Border(BoxBorder.Double)
            .BorderColor(Color.SteelBlue1)
            .Padding(4, 1)
            .Expand();

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    // ─── Authenticated Screen ──────────────────────────────────────────────

    public static void RenderUserHeader(string fullName, string role)
    {
        AnsiConsole.Clear();

        var roleColor = GetRoleColor(role);

        var headerContent = new Markup(
            $"[bold white]{fullName}[/]   [grey]|[/]   [{roleColor}]{role.ToUpper()}[/]"
        );

        var header = new Panel(Align.Center(headerContent, VerticalAlignment.Middle))
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.SteelBlue1_1)
            .Padding(1, 0)
            .Expand();

        AnsiConsole.Write(header);
        AnsiConsole.WriteLine();
    }

    // ─── Menu Rendering ────────────────────────────────────────────────────

    public static void RenderMenuOptions<T>(List<T> options) where T : NamedModel
    {
        foreach(var option in options)
        {
            AnsiConsole.MarkupLine($"  [bold steelblue1]{option.Id}[/]  {option.GetName()}");
        }

        AnsiConsole.WriteLine();
    }

    public static void RenderMenuOptions(List<string> options)
    {
        for (int i = 0; i < options.Count; i++)
        {
            var isLast = i == options.Count - 1;

            // Visually separate the last option (Logout/Exit)
            if (isLast && options.Count > 1)
                AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"  [bold steelblue1]{i + 1}[/]  {options[i]}");
        }

        AnsiConsole.WriteLine();
    } 

    public static void RenderGradeTable(string title, decimal average, int final)
    {
        var table = new Table()
            .Title($"[bold steelblue1]{title}[/]")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.SteelBlue1)
            .HideHeaders()
            .AddColumn(new TableColumn(""))
            .AddColumn(new TableColumn("").RightAligned());

        table.AddRow("[grey]Average[/]", $"[bold]{average:F2}[/]");
        table.AddRow("[grey]Final[/]",   $"[bold]{final}[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        Pause();
    }
    
    public static void RenderStudentGradeTable(
        List<Subject> subjects, 
        List<decimal> subjectsAverages, 
        List<int> subjectsFinals, 
        decimal average, 
        int final)
    {
        var table = new Table()
            .Title("[bold steelblue1]My Grades[/]")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.SteelBlue1)
            .AddColumn(new TableColumn("[grey]Subject[/]"))
            .AddColumn(new TableColumn("[grey]Average[/]").RightAligned())
            .AddColumn(new TableColumn("[grey]Final[/]").RightAligned());

        for (int i = 0; i < subjects.Count; i++)
        {
            table.AddRow(subjects[i].GetName(), $"{subjectsAverages[i]:F2}", $"{subjectsFinals[i]:F2}");
        }

        table.AddEmptyRow();

        table.AddRow("[bold]Overall[/]", $"[bold]{average:F2}[/]", $"[bold]{final:F2}[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        Pause();
    }
    
    public static void RenderSectionTitle(string title)
    {
        var rule = new Rule($"[grey]{title}[/]")
        {
            Justification = Justify.Left,
            Style = Style.Parse("grey dim")
        };

        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();
    }

    // ─── Input ─────────────────────────────────────────────────────────────

    public static int GetMenuChoice(List<string> options)
    {
        RenderMenuOptions(options);
        return GetMenuChoice(1, options.Count);
    }

    public static int GetMenuChoice(int min, int max)
    {
        while (true)
        {
            AnsiConsole.Markup("[grey]>[/] ");
            var input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                return choice;

            AnsiConsole.MarkupLine("[red]Invalid option.[/]");
        }
    }

    
    public static int GetMenuChoice<T>(List<T> choices) where T : NamedModel
    {
        RenderMenuOptions(choices);
        List<int> choiceIds = choices.Select(teacher => teacher.Id).ToList();
        while (true)
        {
            AnsiConsole.Markup("[grey]>[/] ");
            var input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int choice) && choiceIds.Contains(choice))
                return choice;
            
            AnsiConsole.MarkupLine("[red]Invalid option.[/]");
        }
    }
    
    public static string GetInput(string label, bool secret = false)
    {
        string finalInput = string.Empty;
        while (finalInput == string.Empty)
        {
            AnsiConsole.Markup($"  [grey]{label}:[/] ");

            var input = secret
                ? AnsiConsole.Prompt(new TextPrompt<string>("").Secret())
                : Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
            {
                if (input.Contains('@'))
                {
                    var emailResponse = input.ValidateEmail();

                    if (emailResponse.Success)
                    {
                        finalInput = input;
                    }
                    else
                    {
                        ShowError("Email you entered was invalid");
                    }
                }       
                else if (input.All(char.IsDigit))
                {
                    if (input.Length == 11)
                    {
                        finalInput = input;
                    }
                    else
                    {
                        ShowError("National ID you provided was invalid");
                    }
                }
                else
                {
                    finalInput = input;
                        
                }
            }
            else
            {
                ShowError($"{label} cannot be empty.");
            }
        }

        return finalInput;
    }
    
    public static DateTime GetDateInput(string label, string format = "dd/MM/yyyy")
    {
        while (true)
        {
            var input = GetInput($"{label} ({format})");
        
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;
        
            ShowError($"Invalid date. Please use the format {format}.");
        }
    }

    // ─── Feedback ──────────────────────────────────────────────────────────

    public static void ShowSuccess(string message)
    {
        AnsiConsole.MarkupLine($"\n  [green]{message}[/]");
        Pause();
    }

    public static void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"\n  [red]{message}[/]");
        Pause();
    }

    public static void ShowInfo(string message)
    {
        AnsiConsole.MarkupLine($"\n  [grey]{message}[/]");
    }

    public static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]  Press any key to continue...[/]");
        Console.ReadKey(intercept: true);
    }

    // ─── Helpers ───────────────────────────────────────────────────────────

    private static string GetRoleColor(string role) => role.ToLower() switch
    {
        "superadmin"  => "red1",
        "principal"   => "darkorange",
        "teacher"     => "steelblue1",
        "student"     => "mediumspringgreen",
        _             => "grey"
    };

    
}
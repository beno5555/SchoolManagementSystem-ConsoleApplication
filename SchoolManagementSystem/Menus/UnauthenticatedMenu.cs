using SchoolManagementSystem.ConsoleDisplay;

namespace SchoolManagementSystem.Menus;

public enum UnauthenticatedAction { SignIn, SignUp, Exit }

public static class UnauthenticatedMenu
{
    private static readonly List<string> Options = new()
    {
        "Sign In",
        "Sign Up",
        "Exit"
    };

    public static UnauthenticatedAction Run()
    {
        LayoutHelper.RenderWelcomeScreen();
        LayoutHelper.RenderMenuOptions(Options);

        var choice = LayoutHelper.GetMenuChoice(1, Options.Count);

        return choice switch
        {
            1 => UnauthenticatedAction.SignIn,
            2 => UnauthenticatedAction.SignUp,
            _ => UnauthenticatedAction.Exit
        };
    }
}
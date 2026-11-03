namespace SolutionSetup.UI;

/// <summary>
/// The user interface of the garage application. Starts the interaction with the user.
/// </summary>
public interface IUI
{
    /// <summary>
    /// Runs the interaction loop until the user chooses to exit.
    /// </summary>
    void Run();
}

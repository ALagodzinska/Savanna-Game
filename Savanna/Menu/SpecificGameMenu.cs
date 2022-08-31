namespace Savanna.Menu
{
    using Savanna.Entities.Menu;

    /// <summary>
    /// Enum with values for main menu options.
    /// </summary>
    public enum MainMenuOptions
    {
        PlayGame = 0,
        ExitGame = 1,
    }

    /// <summary>
    /// Menu for this game.
    /// </summary>
    public class SpecificGameMenu : GameMenu
    {
        /// <summary>
        /// Creates menu with passed menu information and options.
        /// </summary>
        /// <param name="intro">Title and menu control rules.</param>
        public SpecificGameMenu(string intro) : base(new MenuOption[Enum.GetNames(typeof(MainMenuOptions)).Length], intro)
        {
            Options[0] = new MenuOption()
            {
                Index = (int)MainMenuOptions.PlayGame,
                Title = "Play Game"
            };
            Options[1] = new MenuOption()
            {
                Index = (int)MainMenuOptions.ExitGame,
                Title = "Exit Game"
            };
        }
    }
}

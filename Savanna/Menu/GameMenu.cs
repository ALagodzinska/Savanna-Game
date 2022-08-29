namespace Savanna.Menu
{
    using Savanna.Entities.Menu;

    /// <summary>
    /// Enum with values for main menu options.
    /// </summary>
    public enum MainMenuOptions
    {
        PlayGame,
        ExitGame
    }

    /// <summary>
    /// Menu for this game.
    /// </summary>
    public class GameMenu : Menu<MainMenuOptions>
    {
        /// <summary>
        /// Creates menu with passed menu information and options.
        /// </summary>
        /// <param name="intro">Title and menu control rules.</param>
        public GameMenu(string intro) : base(new MenuOption<MainMenuOptions>[Enum.GetNames(typeof(MainMenuOptions)).Length], intro)
        {
            Options[0] = new MenuOption<MainMenuOptions>()
            {
                Index = MainMenuOptions.PlayGame,
                Title = "Play Game"
            };
            Options[1] = new MenuOption<MainMenuOptions>()
            {
                Index = MainMenuOptions.ExitGame,
                Title = "Exit Game"
            };
        }
    }
}

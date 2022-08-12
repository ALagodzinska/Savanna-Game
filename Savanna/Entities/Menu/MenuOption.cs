namespace Savanna.Entities.Menu
{
    public enum MainMenuOptions
    {
        PlayGame,
        ExitGame
    }

    public class MenuOption<T>
    {
        public T? Index { get; set; }
        public string? Title { get; set; }

        public static MenuOption<MainMenuOptions>[] CreateMainMenuOptions()
        {
            MenuOption<MainMenuOptions>[] options =
                {
                new MenuOption<MainMenuOptions>() {
                    Index = MainMenuOptions.PlayGame,
                    Title = "Play Game"
                },
                new MenuOption<MainMenuOptions>() {
                    Index = MainMenuOptions.ExitGame,
                    Title = "Exit Game"
                },
            };
            return options;
        }
    }

    
}

using Savanna.Entities.Menu;

namespace Savanna.Logic_Layer
{
    public enum MainMenuOptions
    {
        PlayGame,
        ExitGame
    }
    public class GameMenu : Menu<MainMenuOptions>
    {
        public GameMenu(string intro) : base(new MenuOption<MainMenuOptions>[2], intro)
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

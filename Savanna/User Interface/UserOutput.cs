using Savanna.Entities.Menu;
using System.Resources;
using System.Reflection;

namespace Savanna
{
    /// <summary>
    /// Class responsible for displaying information to user.
    /// </summary>
    public class UserOutput
    {
        ResourceManager resourceManager = new ResourceManager("Savanna.Resources.ResourceFile", Assembly.GetExecutingAssembly());
        /// <summary>
        /// Displays game rules to a user.
        /// </summary>
        public void DisplayGameRules()
        {
            Console.Clear();
            Console.WriteLine(resourceManager.GetString("GameRules"));

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{Convert.ToChar(1)}");
            Console.ResetColor();
            Console.Write(" - ANTELOPE | ");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{Convert.ToChar(2)}");
            Console.ResetColor();
            Console.Write(" - LION");
        }

        public Menu<MainMenuOptions> MainMenu()
        {
            var menuIntro = resourceManager.GetString("MainMenuIntro"); ;

            var options = MenuOption<MainMenuOptions>.CreateMainMenuOptions();
            Menu<MainMenuOptions> mainMenu = new Menu<MainMenuOptions>(options, menuIntro);

            return mainMenu;
        }
    }
}
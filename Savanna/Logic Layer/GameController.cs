namespace Savanna.Logic_Layer
{
    using Savanna.Entities.Animals;
    using System.Resources;
    using System.Reflection;
    using Savanna.Menu;
    using Savanna.Resources;

    /// <summary>
    /// Controls main aspects of the game.
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// Stores main game logic.
        /// </summary>
        GameLogic gameLogic = new();

        /// <summary>
        /// Stores menu data.
        /// </summary>
        GameMenu menu;

        /// <summary>
        /// Variable used to declare if a user exits an application.
        /// </summary>
        bool exit = false;

        /// <summary>
        /// Assign menu values to a menu field.
        /// </summary>
        public GameController()
        {
            menu = new GameMenu(ResourceFile.MainMenuIntro);
        }

        /// <summary>
        /// Display main menu and runs it.
        /// </summary>
        public void RunGame()
        {
            Console.Title = "SAVANNA GAME";

            while (!exit)
            {
                Console.Clear();

                var selection = menu.SelectFromMenu();

                switch (selection.Index)
                {
                    case MainMenuOptions.PlayGame:
                        DisplayGameRules();
                        GameActions();
                        break;
                    case MainMenuOptions.ExitGame:
                        ExitGame();
                        break;
                }
            }
        }

        /// <summary>
        /// Display game rules to user.
        /// </summary>
        public void DisplayGameRules()
        {
            Console.Clear();
            Console.WriteLine(ResourceFile.GameRules);

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

        /// <summary>
        /// Contains and launches all possible methods for the game process.
        /// </summary>
        private void GameActions()
        {
            bool exit = false;

            // Create empty borders.
            gameLogic.gameFieldLogic.DrawBorder();

            do
            {
                Thread.Sleep(1000);
                gameLogic.ActionsOnIteration();

                ConsoleKey? consoleKey = Console.KeyAvailable ? Console.ReadKey(true).Key : null;
                if (consoleKey != null)
                {
                    switch (consoleKey)
                    {
                        case ConsoleKey.A:
                            var antelope = gameLogic.CreateNewAnimal(typeof(Antelope));
                            gameLogic.AddAnimalToAnimalList(antelope);
                            break;

                        case ConsoleKey.L:
                            var lion = gameLogic.CreateNewAnimal(typeof(Lion));
                            gameLogic.AddAnimalToAnimalList(lion);
                            break;

                        case ConsoleKey.Escape:
                            exit = true;
                            break;

                        default:
                            break;
                    }
                }

            } while (!exit);
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        private void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            exit = true;
            Environment.Exit(0);
        }
    }
}

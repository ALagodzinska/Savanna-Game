using Savanna.Entities.Animals;
using Savanna.Entities.GameField;
using Savanna.Entities.Menu;
using System.Resources;
using System.Reflection;

namespace Savanna.Logic_Layer
{
    public class GameController
    {
        // Resource data
        ResourceManager resourceManager = new ResourceManager("Savanna.Resources.ResourceFile", Assembly.GetExecutingAssembly());

        // Controls main aspects of games
        GameLogic gameLogic = new();
        GameMenu menu;

        /// <summary>
        /// Variable used to declare if a user exits an application.
        /// </summary>
        bool exit = false;

        public GameController()
        {
            menu = new GameMenu(resourceManager.GetString("MainMenuIntro"));
        }

        /// <summary>
        /// Display main menu.
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

        /// <summary>
        /// Contains and launches all possible methods for the game process.
        /// </summary>
        private void GameActions()
        {
            bool exit = false;

            // Create empty borders
            gameLogic.gameFieldLogic.DrawBorder();

            do
            {
                Thread.Sleep(1000);
                gameLogic.AnimalsActionsOnMove();

                ConsoleKey? consoleKey = Console.KeyAvailable ? Console.ReadKey(true).Key : null;
                if (consoleKey != null)
                {
                    switch (consoleKey)
                    {
                        case ConsoleKey.A:
                            //gameLogic.CreateNewAntelope();
                            var antelope = gameLogic.CreateNewAnimal(typeof(Antelope));
                            gameLogic.AddAnimalToAnimalList(antelope);
                            break;

                        case ConsoleKey.L:
                            //gameLogic.CreateNewLion();
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

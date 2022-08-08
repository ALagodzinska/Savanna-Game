namespace Savanna
{
    using Savanna.Entities;

    /// <summary>
    /// Class responsible for running the game.
    /// </summary>
    public class StartGame
    {
        GameLogic gameLogic = new();        

        /// <summary>
        /// Starts game.
        /// </summary>
        public void Start()
        {
            Console.Title = "SAVANNA GAME";
            RunMainMenu();
        }

        /// <summary>
        /// Display main menu.
        /// </summary>
        private void RunMainMenu()
        {
            var menuIntro = @"
 _____   ___   _   _   ___   _   _  _   _   ___  
/  ___| / _ \ | | | | / _ \ | \ | || \ | | / _ \ 
\ `--. / /_\ \| | | |/ /_\ \|  \| ||  \| |/ /_\ \
 `--. \|  _  || | | ||  _  || . ` || . ` ||  _  |
/\__/ /| | | |\ \_/ /| | | || |\  || |\  || | | |
\____/ \_| |_/ \___/ \_| |_/\_| \_/\_| \_/\_| |_/
                                                 
                                                 
Welcome to the Savanna. What would ypu like to do?
(Use the arrow to cycle through options and press enter to select an option.)" + "\n";
            string[] options = { "Play Game", "Exit Game" };

            Menu mainMenu = new Menu(options, menuIntro);
            var selectedIndex = mainMenu.SelectFromMenu();            

            switch (selectedIndex)
            {
                case 0:                    
                    gameLogic.PlayGame();
                    break;
                case 1:
                    ExitGame();
                    break;
            }
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        private void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}

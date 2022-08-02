using Savanna.Entities;

namespace Savanna
{
    public class StartGame
    {
        GameLogic gameLogic = new();
        UserOutput userOutput = new();

        

        public void Start()
        {
            Console.Title = "GAME OF LIFE";
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
                    gameLogic.ExitGame();
                    break;
            }
        }        
    }
}

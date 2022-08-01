namespace Savanna
{
    public class StartGame
    {
        GameLogic gameLogic = new();

        const int fieldHeight = 25;
        const int fieldWidth = 60;
        const int topStartPoint = 5;
        const ConsoleColor borderColor = ConsoleColor.DarkGreen;

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

            FieldBorder fieldBorder = new FieldBorder(fieldWidth + 1, fieldHeight + 1, topStartPoint, borderColor);

            switch (selectedIndex)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine(@$"THIS IS SAVANNA
PRESS 'L' TO ADD A LION, PRESS 'A' TO ADD AN ANTELOPE TO GAME FIELD
{Convert.ToChar(1)} - ANTELOPE | {Convert.ToChar(2)} - LION");
                    fieldBorder.DrawBorder();
                    gameLogic.ActionToMake();
                    //gameLogic.DrawXOnField(fieldHeight, fieldWidth, topStartPoint);
                    Console.ReadLine();
                    break;
                case 1:
                    gameLogic.ExitGame();
                    break;
            }
        }        
    }
}

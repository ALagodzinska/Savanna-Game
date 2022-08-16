namespace Savanna.Entities.Menu
{
    /// <summary>
    /// Used for creating and displaying menu to user.
    /// </summary>
    public class Menu<T>
    {
        /// <summary>
        /// Set initial values for menu fields.
        /// </summary>
        /// <param name="options">Menu options to choose from.</param>
        /// <param name="menuIntro">Title and menu control rules.</param>
        public Menu(MenuOption<T>[] options, string menuIntro)
        {
            Options = options;
            SelectedOptionIndex = 0;
            MenuIntro = menuIntro;
        }

        /// <summary>
        /// Menu options to choose from.
        /// </summary>
        protected MenuOption<T>[] Options;

        /// <summary>
        /// Index value of selected option.
        /// </summary>
        private int SelectedOptionIndex;

        /// <summary>
        /// Title and menu control rules.
        /// </summary>
        private string MenuIntro;

        /// <summary>
        /// Displays menu to user.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine(MenuIntro);
            for (int i = 0; i < Options.Length; i++)
            {
                bool isSelected = i == SelectedOptionIndex;
                OptionStyle(isSelected, Options[i].Title);
            }

            Console.ResetColor();
        }

        /// <summary>
        /// By parameters selects how to display the option in the menu.
        /// </summary>
        /// <param name="isSelected">Used to understand if this menu option is selected by a user or not.</param>
        /// <param name="currentOption">One option to display.</param>
        private void OptionStyle(bool isSelected, string currentOption)
        {
            string sideSymbol = isSelected ? Convert.ToChar(3).ToString() : Convert.ToChar(4).ToString();
            string prefix = isSelected ? Convert.ToChar(16).ToString() : " ";
            Console.BackgroundColor = isSelected ? ConsoleColor.Green : ConsoleColor.Black;
            Console.ForegroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
            Console.WriteLine($"{prefix} {sideSymbol} {currentOption} {sideSymbol}");
        }

        /// <summary>
        /// Allows to select an option from the menu.
        /// </summary>
        /// <returns>Selected option index.</returns>
        public MenuOption<T> SelectFromMenu()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.SetCursorPosition(0, 0);
                DisplayMenu();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedOptionIndex--;

                    if (SelectedOptionIndex == -1)
                    {
                        //move to the last option
                        SelectedOptionIndex = Options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedOptionIndex++;

                    if (SelectedOptionIndex == Options.Length)
                    {
                        //move to first option
                        SelectedOptionIndex = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter);

            return Options[SelectedOptionIndex];
        }
    }
}

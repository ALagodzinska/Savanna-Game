namespace Savanna
{
    public class Menu
    {
        public Menu(string[] options, string menuIntro)
        {
            SelectedOptionIndex = 0;
            Options = options;
            MenuIntro = menuIntro;
        }

       
        private int SelectedOptionIndex;

        
        private string[] Options;

        
        private string MenuIntro;

       
        private void DisplayMenu()
        {
            Console.WriteLine(MenuIntro);
            for (int i = 0; i < Options.Length; i++)
            {
                bool isSelected = i == SelectedOptionIndex;
                OptionStyle(isSelected, Options[i]);
            }

            Console.ResetColor();
        }

        public void OptionStyle(bool isSelelcted, string currentOption)
        {
            string sideSymbol = isSelelcted ? Convert.ToChar(3).ToString() : Convert.ToChar(4).ToString();
            string prefix = isSelelcted ? Convert.ToChar(16).ToString() : " ";
            Console.BackgroundColor = isSelelcted ? ConsoleColor.Green : ConsoleColor.Black;
            Console.ForegroundColor = isSelelcted ? ConsoleColor.Black : ConsoleColor.White;
            Console.WriteLine($"{prefix} {sideSymbol} {currentOption} {sideSymbol}");
        }

        public int SelectFromMenu()
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
                        SelectedOptionIndex = Options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedOptionIndex++;
                    if (SelectedOptionIndex == Options.Length)
                    {
                        SelectedOptionIndex = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter);

            return SelectedOptionIndex;
        }
    }
}

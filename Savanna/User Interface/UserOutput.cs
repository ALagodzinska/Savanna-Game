
namespace Savanna
{
    /// <summary>
    /// Class responsible for displaying information to user.
    /// </summary>
    public class UserOutput
    {
        /// <summary>
        /// Displays game rules to a user.
        /// </summary>
        public void DisplayGameRules()
        {
            Console.Clear();
            Console.WriteLine(@$"THIS IS SAVANNA
PRESS 'L' TO ADD A LION, PRESS 'A' TO ADD AN ANTELOPE TO GAME FIELD
PRESS 'ESC' TO STOP THE GAME AND GO BACK TO MAIN MENU");

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
    }
}

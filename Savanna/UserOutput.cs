namespace Savanna
{
    public class UserOutput
    {
        public void DisplayGameRules()
        {
            Console.Clear();
            Console.WriteLine(@$"THIS IS SAVANNA
PRESS 'L' TO ADD A LION, PRESS 'A' TO ADD AN ANTELOPE TO GAME FIELD");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{Convert.ToChar(1)}");
            Console.ResetColor();
            Console.Write(" - ANTELOPE | ");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{Convert.ToChar(2)}");
            Console.ResetColor();
            Console.Write(" - LION");
        }
    }
}

using Savanna.Entities;

namespace Savanna
{
    public class GameLogic
    {

        public void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public void DrawXOnField(int borderHeight, int borderWidth, int offsetFromTop)
        {
            for (int y = offsetFromTop + 1; y < borderHeight + offsetFromTop + 2; y++)
            {
                for(int x = 1; x < borderWidth + 2; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                }
            }
        }
    }
}

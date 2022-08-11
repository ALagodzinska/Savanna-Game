namespace Savanna.Entities
{
    /// <summary>
    /// Stores data about game field and method to draw game border.
    /// </summary>
    public class GameField
    {
        /// <summary>
        /// Creates new game field with assigned values.
        /// </summary>
        public GameField()
        {
            Height = 5;
            Width = 5;
            TopPosition = 5;
            BorderColor = ConsoleColor.DarkGreen;            
        }

        /// <summary>
        /// Height of the game field.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Width of the game field.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Game field offset from top.
        /// </summary>
        public int TopPosition { get; set; }

        /// <summary>
        /// Border color for game field.
        /// </summary>
        private ConsoleColor BorderColor;

        /// <summary>
        /// Draws a border line for game field.
        /// </summary>
        public void DrawBorder()
        {
            string startCorner = "╔";
            string cornerBottomLeft = "╚";
            string cornerTopRight = "╗";
            string cornerBottomRight = "╝";
            string verticalLine = "║";
            string horizontalLine = "═";

            //draw top of the border
            for (int w = 0; w < Width; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerTopRight + "\n";

            //draw vertical lines
            for (int h = 0; h < Height; h++)
            {
                startCorner += verticalLine + new string(' ', Width) + verticalLine + "\n";
            }
            startCorner += cornerBottomLeft;

            //draw bottom of the border
            for (int w = 0; w < Width; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerBottomRight;

            Console.SetCursorPosition(0, TopPosition);
            Console.ForegroundColor = BorderColor;
            Console.Write(startCorner);
            Console.ResetColor();
        }
    }
}

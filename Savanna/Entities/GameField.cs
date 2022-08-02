namespace Savanna.Entities
{
    public class GameField
    {
        public GameField(int width, int height, int topPosition, ConsoleColor borderColor)
        {
            Width = width;
            Height = height;
            BorderColor = borderColor;
            TopPosition = topPosition;
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public int TopPosition { get; set; }
        private ConsoleColor BorderColor;
        private int IndentForBorder;

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

            //bottom of the border
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

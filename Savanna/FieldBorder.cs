namespace Savanna
{
    public class FieldBorder
    {
        public FieldBorder(int width, int height, int topPosition, ConsoleColor borderColor)
        {
            Width = width;
            Height = height;
            BorderColor = borderColor;
            TopPosition = topPosition;
        }

        private int Height;
        private int Width;
        private int TopPosition;
        private ConsoleColor BorderColor;

        public void DrawBorder()
        {
            string startCorner = "╔";
            string cornerBottomLeft = "╚";
            string cornerTopRight = "╗";
            string cornerBottomRight = "╝";
            string verticalLine = "║";
            string horizontalLine = "═";

            //draw top of the border
            for (int w = 0; w < this.Width; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerTopRight + "\n";

            //draw vertical lines
            for (int h = 0; h < this.Height; h++)
            {
                startCorner += verticalLine + new string(' ', this.Width) + verticalLine + "\n";
            }
            startCorner += cornerBottomLeft;

            for (int w = 0; w < this.Width; w++)
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

namespace Savanna.Entities.GameField
{
    /// <summary>
    /// Stores data about game field.
    /// </summary>
    public class GameField
    {
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
        public ConsoleColor BorderColor;

        /// <summary>
        /// Creates new game field with assigned values.
        /// </summary>
        public GameField()
        {
            Height = 20;
            Width = 50;
            TopPosition = 5;
            BorderColor = ConsoleColor.DarkGreen;
        }
    }
}

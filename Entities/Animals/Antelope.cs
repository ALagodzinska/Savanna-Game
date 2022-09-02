namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Class for animal - antelope.
    /// </summary>
    public class Antelope : Animal
    {
        /// <summary>
        /// Stores data about animal color.
        /// </summary>
        public new ConsoleColor AnimalColor { get => SetAntelopeColor(); }

        /// <summary>
        /// Creates new antelope with assigned values.
        /// </summary>
        public Antelope()
        {
            Health = 15;
            VisionRange = 4;
        }

        /// <summary>
        /// Method to set antelopes color based on health level.
        /// </summary>
        /// <returns>Color for antelope.</returns>
        public ConsoleColor SetAntelopeColor()
        {
            var antelopeColor = this.Health < 2 == true ? ConsoleColor.DarkGray : ConsoleColor.White;

            return antelopeColor;
        }
    }
}

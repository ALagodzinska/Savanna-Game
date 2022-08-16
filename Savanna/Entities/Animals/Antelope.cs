namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Class for animal - antelope.
    /// </summary>
    public class Antelope : Animal
    {
        public new ConsoleColor AnimalColor { get => SetAntelopeColor(); }

        /// <summary>
        /// Creates new antelope with assigned values.
        /// </summary>
        public Antelope()
        {
            IsAlive = true;
            Health = 15;
            VisionRange = 4;
        }

        public ConsoleColor SetAntelopeColor()
        {
            var antelopeColor = this.Health < 2 == true ? ConsoleColor.DarkGray : ConsoleColor.White;

            return antelopeColor;
        }
    }
}

namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Class for animal - lion.
    /// </summary>
    public class Lion : Animal
    {
        /// <summary>
        /// Used to understand if animal had eaten another animal.
        /// </summary>
        public bool Ate { get; set; }

        public new ConsoleColor AnimalColor { get => SetLionColor(); }

        /// <summary>
        /// Creates new lion with assigned values.
        /// </summary>
        public Lion()
        {
            VisionRange = 2;
            Health = 30;
            IsAlive = true;
        }

        public ConsoleColor SetLionColor()
        {
            var lionColor = this.Health < 2 == true ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;

            if (this.Ate)
            {
                lionColor = ConsoleColor.Red;
            }
            return lionColor;
        }
    }
}

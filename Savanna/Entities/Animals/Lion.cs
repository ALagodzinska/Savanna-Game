//namespace Savanna.Entities.Animals
//{
//    /// <summary>
//    /// Class for animal - lion.
//    /// </summary>
//    public class Lion : Animal
//    {
//        /// <summary>
//        /// Used to understand if animal had eaten another animal.
//        /// </summary>
//        public bool DoesAte { get; set; }

//        /// <summary>
//        /// Stores data about animal color.
//        /// </summary>
//        public new ConsoleColor AnimalColor { get => SetLionColor(); }

//        /// <summary>
//        /// Creates new lion with assigned values.
//        /// </summary>
//        public Lion()
//        {
//            VisionRange = 2;
//            Health = 30;
//            IsAlive = true;
//        }

//        /// <summary>
//        /// Method to set lions color based on health level and ate parameter.
//        /// </summary>
//        /// <returns>Color for lion.</returns>
//        public ConsoleColor SetLionColor()
//        {
//            var lionColor = this.Health < 2 == true ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;

//            if (this.DoesAte)
//            {
//                lionColor = ConsoleColor.Red;
//            }

//            return lionColor;
//        }
//    }
//}

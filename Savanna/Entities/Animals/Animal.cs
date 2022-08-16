using Savanna.Entities.GameField;

namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Base class for all animals with main properties.
    /// </summary>
    public class Animal
    {
        /// <summary>
        /// Unique animal identifier.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Stores two coordinates for current animal position. [0] is width coordinate. [1] is height coordinate.
        /// </summary>
        public Coordinates CurrentPosition { get; set; }

        /// <summary>
        /// Stores two coordinates for next animal position. [0] is width coordinate. [1] is height coordinate.
        /// </summary>
        public Coordinates NextPosition { get; set; }

        public double Health { get; set; }

        /// <summary>
        /// Stores data about animal life status. If true - alive. If false - dead.
        /// </summary>
        public bool? IsAlive { get; set; }

        /// <summary>
        /// How many cells around the animal sees.
        /// </summary>
        public int VisionRange { get; set; }

        public ConsoleColor AnimalColor { get; set; }

        /// <summary>
        /// Field used for incrementing animals ID.
        /// </summary>
        protected static int globalAnimalId = 0;

        /// <summary>
        /// Creates animal with assigned ID.
        /// </summary>
        public Animal()
        {
            ID = Interlocked.Increment(ref globalAnimalId);
        }
    }
}

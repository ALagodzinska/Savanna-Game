namespace Savanna.Entities.Animals
{
    using Savanna.Entities.GameField;
    using System;

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
        /// Stores two coordinates for current animal position.
        /// </summary>
        public Coordinates CurrentPosition { get; set; }

        /// <summary>
        /// Stores two coordinates for next animal position.
        /// </summary>
        public Coordinates NextPosition { get; set; }

        /// <summary>
        /// Animal health.
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        /// Stores data about animal life status. If true - alive. If false - dead.
        /// </summary>
        public bool? IsAlive { get; set; }

        /// <summary>
        /// How many cells around the animal it sees.
        /// </summary>
        public int VisionRange { get; set; }

        /// <summary>
        /// Color of the animal, it may be different because of type, health level.
        /// </summary>
        public ConsoleColor AnimalColor { get; set; }

        /// <summary>
        /// Field used for incrementing animals ID.
        /// </summary>
        static int globalAnimalId = 0;

        /// <summary>
        /// Creates animal with assigned ID.
        /// </summary>
        public Animal()
        {
            ID = Interlocked.Increment(ref globalAnimalId);
        }
    }
}

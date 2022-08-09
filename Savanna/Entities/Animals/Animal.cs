﻿namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Base class for all animals with main properties.
    /// </summary>
    public class Animal
    {
        /// <summary>
        /// Stores two coordinates for current animal position. [0] is width coordinate. [1] is height coordinate.
        /// </summary>
        public int[]? CurrentPosition { get; set; }

        /// <summary>
        /// Stores two coordinates for next animal position. [0] is width coordinate. [1] is height coordinate.
        /// </summary>
        public int[]? NextPosition { get; set; }

        /// <summary>
        /// Stores data about animal life status. If true - alive. If false - dead.
        /// </summary>
        public bool? IsAlive { get; set; }

        /// <summary>
        /// How many cells around the animal sees.
        /// </summary>
        public int VisionRange { get; set; }        
    }
}

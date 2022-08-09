namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Class for animal - lion.
    /// </summary>
    public class Lion : Animal
    {
        /// <summary>
        /// Creates new lion with assigned values.
        /// </summary>
        public Lion()
        {
            VisionRange = 1;
            IsAlive = true;            
        }

        /// <summary>
        /// Used to understand if animal had eaten another animal.
        /// </summary>
        public bool Ate { get; set; }
    }
}

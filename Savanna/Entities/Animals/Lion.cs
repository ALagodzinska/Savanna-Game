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
            IsAlive = true;
            VisionRange = 1;
            Type = "Lion";
        }
    }
}

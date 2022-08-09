namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Class for animal - antelope.
    /// </summary>
    public class Antelope : Animal
    {
        /// <summary>
        /// Creates new antelope with assigned values.
        /// </summary>
        public Antelope()
        {
            IsAlive = true;
            Health = 20;
            VisionRange = 2;            
        }
    }
}

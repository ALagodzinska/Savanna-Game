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
            Type = "Antelope";
            IsAlive = true;
            VisionRange = 2;            
        }
    }
}

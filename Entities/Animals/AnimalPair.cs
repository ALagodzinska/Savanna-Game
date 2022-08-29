namespace Savanna.Entities.Animals
{
    /// <summary>
    /// Stores data about animal pairs on a game field.
    /// </summary>
    public class AnimalPair
    {
        /// <summary>
        /// Animal in pair with largest ID number.
        /// </summary>
        public Animal AnimalWithLargestID { get; set; }

        /// <summary>
        /// Animal in pair with smallest ID number.
        /// </summary>
        public Animal AnimalWithSmallestID { get; set; }

        /// <summary>
        /// How many iterations animals spent together.
        /// </summary>
        public int RoundsTogether { get; set; }

        /// <summary>
        /// Stores data about animal pair status. False - if pair is still together. True - if pair broke up.
        /// </summary>
        public bool DoesBrokeUp { get; set; }

        /// <summary>
        /// Creates animal pair for two animals.
        /// </summary>
        /// <param name="animal">Animal to make a pair.</param>
        /// <param name="animalPartner">Animal to make a pair.</param>
        public AnimalPair(Animal animal, Animal animalPartner)
        {
            if (animal.ID > animalPartner.ID)
            {
                AnimalWithLargestID = animal;
                AnimalWithSmallestID = animalPartner;
            }
            else
            {
                AnimalWithLargestID = animalPartner;
                AnimalWithSmallestID = animal;
            }

            RoundsTogether = 1;
            DoesBrokeUp = false;
        }
    }
}

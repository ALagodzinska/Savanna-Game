namespace Savanna.Entities.Animals
{
    public class AnimalPair
    {
        public Animal AnimalWithLargestID { get; set; }

        public Animal AnimalWithSmallestID { get; set; }

        public int RoundsTogether { get; set; }

        public bool BrokeUp { get; set; }

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
            BrokeUp = false;
        }
    }
}

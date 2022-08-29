namespace AnimalBehaviorInterfaces.Entities
{
    public interface IAnimalPair
    {
        IAnimal AnimalWithLargestID { get; set; }

        IAnimal AnimalWithSmallestID { get; set; }

        int RoundsTogether { get; set; }

        bool DoesBrokeUp { get; set; }
    }
}

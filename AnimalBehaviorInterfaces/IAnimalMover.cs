namespace AnimalBehaviorInterfaces
{
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;

    public interface IAnimalMover
    {
        int FieldHeight { get; set; }

        int FieldWidth { get; set; }

        List<Animal> Animals { get; set; }

        void SetNewAnimalCurrentPosition(Animal animal);

        Animal? GetAnimalByCurrentCoordinates(Coordinates coordinates);

        void SetNextPositionForAnimal(Animal animal);

        List<Coordinates> PossibleMoves(Animal animal);

        bool DoesPlaceWillBeTakenInNextStep(Coordinates coordinates);

        double FindDistanceBetweenTwoCoordinates(Coordinates neighbourAnimalCoordinates, Coordinates currentAnimalCoordinates);

        void MakeMove(Animal animal);
        
        void AnimalsExceptions(Animal animal); 
    }
}

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

        List<Animal> AnimalsInVisionRange(Animal animal);

        List<Coordinates> PossibleMoves(Animal animal);

        bool DoesPlaceWillBeTakenInNextStep(Coordinates coordinates);

        Antelope? GetClosestAntelope(List<Antelope> antelopesAround, Animal currentAnimal);

        Coordinates RandomMovePosition(List<Coordinates> moves);

        double FindDistanceBetweenTwoCoordinates(Coordinates neighbourAnimalCoordinates, Coordinates currentAnimalCoordinates);

        void LionsNextAction(Lion lionToMove, Antelope closestAntelope, List<Coordinates> possibleSpacesToMove);

        void LionEatAntelope(Lion lion, Antelope antelope);

        Coordinates GetClosestSpaceToAntelope(List<Coordinates> freeSpaceToMove, Antelope closestAntelope);

        Coordinates GetFarsetSpaceFromLion(List<Coordinates> freeSpaceToMove, List<Lion> lionsInTheVisionRange, Antelope antelope);

        List<double[]> ReturnListOfDistancePoints(List<Coordinates> freeSpaceToMove, List<Lion> lionsInTheVisionRange, Antelope antelope);

        void MakeMove(Animal animal);
    }
}

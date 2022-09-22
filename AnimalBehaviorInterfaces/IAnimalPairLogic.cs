using Savanna.Entities.Animals;
namespace AnimalBehaviorInterfaces
{
    using Savanna.Entities.GameField;

    public interface IAnimalPairLogic
    {
        List<AnimalPair> AnimalPairs { get; set; }

        List<Animal> AnimalsToBeBorn { get; set; }

        //IAnimalMover AnimalMovers { get; set; }

        //void AnimalPairsCreated();

        void AddNewbornsToGame();

        void CheckIfAnimalHavePair(Animal mainAnimal);

        //void AddNewPair(AnimalPair animalPair);

        void ActionForPairsOnMove();

        //List<Animal> AnimalsNearbyWithSameType(Animal animal);

        //void AnimalToBeBorn(AnimalPair animalPair);

        //List<Coordinates> GetListWithUniqueFreeSpacesAroundParents(List<Coordinates> spacesAroundFirstParent, List<Coordinates> spacesAroundSecondParent);

        //Coordinates? GetPlaceToBorn(Animal oneParent, Animal secondParent);
    }
}

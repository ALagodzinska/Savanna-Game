using AnimalBehaviorInterfaces.Entities;
using Interfaces.GameField;
using Savanna.Entities.Animals;
using Savanna.Entities.GameField;

namespace AnimalBehaviorInterfaces
{
    public interface IAnimalPairLogic
    {
        List<AnimalPair> AnimalPairs { get;}

        List<Animal> AnimalsToBeBorn { get;}

        IAnimalMover AnimalMovers { get; set; }

        void AnimalPairsCreated();

        void AddNewbornsToGame();

        void CheckIfAnimalHavePair(Animal mainAnimal);

        void AddNewPair(AnimalPair animalPair);

        void ActionForPairsOnMove();

        List<Animal> AnimalsNearbyWithSameType(Animal animal);

        void AnimalToBeBorn(AnimalPair animalPair);

        List<Coordinates> GetListWithUniqueFreeSpacesAroundParents(List<Coordinates> spacesAroundFirstParent, List<Coordinates> spacesAroundSecondParent);

        Coordinates? GetPlaceToBorn(Animal oneParent, Animal secondParent);
    }
}

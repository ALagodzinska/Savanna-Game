using Savanna.Entities.Animals;
namespace AnimalBehaviorInterfaces
{
    using Savanna.Entities.GameField;

    public interface IAnimalPairLogic
    {
        List<AnimalPair> AnimalPairs { get; set; }

        List<Animal> AnimalsToBeBorn { get; set; }

        void AddNewbornsToGame();

        void CheckIfAnimalHavePair(Animal mainAnimal);

        void ActionForPairsOnMove();
    }
}

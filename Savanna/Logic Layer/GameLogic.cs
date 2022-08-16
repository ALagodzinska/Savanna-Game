using Savanna.Entities.Animals;
using Savanna.Entities.GameField;

namespace Savanna.Logic_Layer
{
    public class GameLogic
    {
        public static GameField gameField = new();
        public GameFieldLogic gameFieldLogic = new(gameField);

        /// <summary>
        /// Finds next step for the animal and displays it to the user.
        /// </summary>
        public void AnimalsActionsOnMove()
        {
            var animals = gameFieldLogic.Animals;

            if (animals.Count != 0)
            {
                gameFieldLogic.SetNextPositionForAnimals();

                gameFieldLogic.AnimalPairsCreated();

                foreach (var animal in animals)
                {
                    gameFieldLogic.MakeMove(animal);
                }

                gameFieldLogic.AddNewbornsToGame();

                animals.RemoveAll(a => a.IsAlive == false);

                gameFieldLogic.DrawGame();
            }
        }

        public Animal CreateNewAnimal(Type typeOfAnimal)
        {
            Animal createdAnimal;
            if(typeOfAnimal == typeof(Lion))
            {
                createdAnimal = new Lion();
            }
            else
            {
                createdAnimal = new Antelope();
            }

            gameFieldLogic.SetAnimalPosition(createdAnimal);

            return createdAnimal;
        }

        public void AddAnimalToAnimalList(Animal animal)
        {
            gameFieldLogic.Animals.Add(animal);
        }
    }
}

namespace Savanna.Logic_Layer
{
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;

    /// <summary>
    /// Class stores main game logic in game.
    /// </summary>
    public class GameLogic
    {
        /// <summary>
        /// Creates new game field.
        /// </summary>
        public static GameField gameField = new();

        /// <summary>
        /// Gets game field logic for the game.
        /// </summary>
        public GameFieldLogic gameFieldLogic = new(gameField);

        /// <summary>
        /// Does action for each animal in the game.
        /// </summary>
        public void ActionsOnIteration()
        {
            var animals = gameFieldLogic.Animals;

            if (animals.Count != 0)
            {
                gameFieldLogic.AnimalsActionsOnMove();
            }
        }

        /// <summary>
        /// Creates new animal based on passed type.
        /// </summary>
        /// <param name="typeOfAnimal">Animal type.</param>
        /// <returns>Created animal.</returns>
        public Animal? CreateNewAnimal(Type typeOfAnimal)
        {
            Animal createdAnimal;

            if (gameFieldLogic.DoesGameFieldHaveFreeSpaces())
            {
                if (typeOfAnimal == typeof(Lion))
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

            return null;
        }

        /// <summary>
        /// Adds created animal to animals list.
        /// </summary>
        /// <param name="animal">Animal to add to a list.</param>
        public void AddAnimalToAnimalList(Animal animal)
        {
            if(animal != null)
            {
                gameFieldLogic.Animals.Add(animal);
            }            
        }
    }
}

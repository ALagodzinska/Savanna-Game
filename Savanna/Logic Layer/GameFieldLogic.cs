using Savanna.Entities.Animals;
using Savanna.Entities.GameField;

namespace Savanna.Logic_Layer
{
    public class GameFieldLogic
    {
        public List<Animal> Animals;

        GameField GameField;

        AnimalMover AnimalMover;

        AnimalPairLogic AnimalPairLogic;
        public GameFieldLogic(GameField gameField)
        {

            Animals = new List<Animal>();
            AnimalMover = new AnimalMover(gameField.Height, gameField.Width, Animals);
            AnimalPairLogic = new AnimalPairLogic(AnimalMover);
            GameField = gameField;
        }
        /// <summary>
        /// Draws a border line for game field.
        /// </summary>
        public void DrawBorder()
        {
            string startCorner = "╔";
            string cornerBottomLeft = "╚";
            string cornerTopRight = "╗";
            string cornerBottomRight = "╝";
            string verticalLine = "║";
            string horizontalLine = "═";

            //draw top of the border
            for (int w = 0; w < GameField.Width; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerTopRight + "\n";

            //draw vertical lines
            for (int h = 0; h < GameField.Height; h++)
            {
                startCorner += verticalLine + new string(' ', GameField.Width) + verticalLine + "\n";
            }
            startCorner += cornerBottomLeft;

            //draw bottom of the border
            for (int w = 0; w < GameField.Height; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerBottomRight;

            Console.SetCursorPosition(0, GameField.TopPosition);
            Console.ForegroundColor = GameField.BorderColor;
            Console.Write(startCorner);
            Console.ResetColor();
        }

        public void SetAnimalPosition(Animal animal)
        {
            AnimalMover.SetNewAnimalCurrentPosition(animal);
            DrawAnimal(animal);
        }

        public void SetNextPositionForAnimals()
        {
            foreach (var animal in Animals)
            {
                AnimalMover.NextPositionForAnimals(animal);
            }
        }

        public void DrawGame()
        {
            DrawBorder();
            if (Animals.Count > 0)
            {
                foreach (var animal in Animals)
                {
                    DrawAnimal(animal);
                }
            }
        }


        /// <summary>
        /// Display animal on a game screen.
        /// </summary>
        /// <param name="animal">Animal from game.</param>
        public void DrawAnimal(Animal animal)
        {
            if (animal.IsAlive == true)
            {
                Console.SetCursorPosition(animal.CurrentPosition.X + 1, animal.CurrentPosition.Y + GameField.TopPosition + 1);
                var isLion = animal.GetType() == typeof(Lion);

                var symbol = isLion ? Convert.ToChar(2) : Convert.ToChar(1);
                Console.BackgroundColor = ConsoleColor.Black;

                if (isLion)
                {
                    var lion = (Lion)animal;

                    Console.ForegroundColor = lion.AnimalColor;
                    lion.Ate = false;
                }
                else
                {
                    var antelope = (Antelope)animal;

                    Console.ForegroundColor = antelope.AnimalColor;
                }

                Console.Write(symbol);
            }

            Console.ResetColor();
        }

        public void MakeMove(Animal animal)
        {
            animal.CurrentPosition = animal.NextPosition;
            animal.NextPosition = null;
            animal.Health -= 0.5;
            if (animal.Health <= 0)
            {
                animal.IsAlive = false;
            }
        }

        public void AnimalPairsCreated()
        {
            //check if together for next iteration
            AnimalPairLogic.CheckIfPairsAreTogether();

            //create couple if together in this and next iteration
            foreach (var animal in Animals)
            {
                AnimalPairLogic.CheckIfAnimalHavePair(animal);
            }

            AnimalPairLogic.animalPairs.RemoveAll(c => c.BrokeUp == true);
        }

        public void AddNewbornsToGame()
        {
            if (AnimalPairLogic.animalsToBeBorn.Count > 0)
            {
                foreach (var newborn in AnimalPairLogic.animalsToBeBorn)
                {
                    Animals.Add(newborn);
                }
                AnimalPairLogic.animalsToBeBorn.Clear();
            }
        }
    }
}

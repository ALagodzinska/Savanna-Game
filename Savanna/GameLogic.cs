using Savanna.Entities;

namespace Savanna
{
    public class GameLogic
    {
        UserOutput userOutput = new();


        const int fieldHeight = 25;
        const int fieldWidth = 60;
        const int topStartPoint = 5;
        const ConsoleColor borderColor = ConsoleColor.DarkGreen;

        List<Animal> animals = new List<Animal>();
        public void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public GameField CreateGameField() => new GameField(fieldWidth, fieldHeight, topStartPoint, borderColor);

        public void PlayGame()
        {
            userOutput.DisplayGameRules();
            var gameField = CreateGameField();
            gameField.DrawBorder();
            //DrawXOnField(25, 60, 5);
            ActionToMake(gameField);
        }

        public void DrawXOnField(int borderHeight, int borderWidth, int offsetFromTop)
        {
            for (int y = offsetFromTop + 1; y < borderHeight + offsetFromTop + 1; y++)
            {
                for (int x = 1; x < borderWidth + 1; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                }
            }
        }

        public void ActionToMake(GameField gameField)
        {
            ConsoleKeyInfo pressedKey;

            do
            {
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.A:

                        var createdAntelope = new Antelope();
                        animals.Add(createdAntelope);
                        while (createdAntelope.CurrentPosition == null)
                        {
                            AddAnimalToGameField(createdAntelope, gameField);
                        }
                        DrawAnimal(createdAntelope);
                        break;

                    case ConsoleKey.L:
                        var createdLion = new Lion();
                        animals.Add(createdLion);
                        while (createdLion.CurrentPosition == null)
                        {
                            AddAnimalToGameField(createdLion, gameField);
                        }
                        DrawAnimal(createdLion);
                        break;

                    default:
                        Console.SetCursorPosition(0, 3);
                        Console.WriteLine("Wrong input");
                        break;
                }
            } while (pressedKey.Key != ConsoleKey.Escape);

        }

        public void DrawAnimal(Animal animal)
        {
            Console.SetCursorPosition(animal.CurrentPosition[0] + 1, animal.CurrentPosition[1] + topStartPoint + 1);
            var symbol = animal.Type == "Lion" ? Convert.ToChar(2) : Convert.ToChar(1);
            Console.Write(symbol);
        }

        public void AddAnimalToGameField(Animal animal, GameField gameField)
        {
            Random random = new Random();
            var randomWidthPosition = random.Next(0, gameField.Width - 1);
            var randomHeightPosition = random.Next(0, gameField.Height - 1);
            int[] coordinates = new int[2] { randomWidthPosition, randomHeightPosition };
            if (CheckIfPlaceIsTaken(coordinates) == false)
            {
                animal.CurrentPosition = coordinates;
            }
        }

        public bool CheckIfPlaceIsTaken(int[] coordinates)
        {
            var foundAnimal = GetAnimalByCoordinates(coordinates);
            return foundAnimal != null ? true : false;
        }

        public Animal? GetAnimalByCoordinates(int[] coordinates)
        {
            return animals.FirstOrDefault(a => a.CurrentPosition == coordinates);
        }



        public void AnimalsAround(Animal animal, GameField gameField)
        {
            int[] coordinates = new int[2];
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition[1] - animal.VisionRange; h <= animal.CurrentPosition[1] + animal.VisionRange; h++)
            {
                for (int w = animal.CurrentPosition[0] - animal.VisionRange; w <= animal.CurrentPosition[0] + animal.VisionRange; w++)
                {
                    if (h >= gameField.Height || h < 0
                        || w >= gameField.Width || w < 0
                        || h == animal.CurrentPosition[1] && w == animal.CurrentPosition[0])
                    {
                        continue;
                    }
                    coordinates[0] = w;
                    coordinates[1] = h;

                    var foundAnimal = GetAnimalByCoordinates(coordinates);
                    if (foundAnimal == null)
                    {
                        continue;
                    }

                    closestAnimalList.Add(foundAnimal);
                }
            }
            if (animal.Type == "Lion")
            {
                var movePossibility = PossibleMoves(animal);
                if(movePossibility == null)
                {
                    animal.NextPosition = animal.CurrentPosition;
                }
                var antelopesAround = closestAnimalList.FindAll(a => a.Type == "Antelope");

                if (antelopesAround != null)
                {
                    var closestAntelope = GetClosestAnimal(antelopesAround, animal);
                    NextLionAction(animal, closestAntelope, movePossibility);
                }
                else
                {
                    animal.NextPosition = RandomMove(movePossibility);
                }
            }
        }

        public Animal GetClosestAnimal(List<Animal> animalsAround, Animal currentAnimal)
        {
            int minDistance = currentAnimal.VisionRange;
            int[] closestAnimalCoordinats = new int[2];

            foreach (var animal in animalsAround)
            {
                var distance = FindDistanceBetweenTwoCoordinates(animal.CurrentPosition, currentAnimal);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAnimalCoordinats = animal.CurrentPosition;
                }
            }
            return animals.FirstOrDefault(a => a.CurrentPosition == closestAnimalCoordinats);
        }

        public List<int[]> PossibleMoves(Animal animal)
        {
            List<int[]> moves = new List<int[]>();

            for (int h = animal.CurrentPosition[1] - 1; h < animal.CurrentPosition[1] + 2; h++)
            {
                for (int w = animal.CurrentPosition[0] - 1; w < animal.CurrentPosition[0] + 2; w++)
                {
                    int[] foundCoordinates = { w, h };

                    if (h >= animal.CurrentPosition[1] || h < 0
                        || w >= animal.CurrentPosition[0] || w < 0
                        || h == animal.CurrentPosition[1] && w == animal.CurrentPosition[0]
                        || CheckIfPlaceIsTaken(foundCoordinates))
                    {
                        continue;
                    }

                    moves.Add(foundCoordinates);
                }
            }
            return moves;            
        }

        public int[] RandomMove(List<int[]> moves)
        {
            Random random = new Random();
            var moveIndex = random.Next(moves.Count);
            return moves[moveIndex];
        }

        public int FindDistanceBetweenTwoCoordinates(int[] animalCoordinates, Animal currentAnimal)
        {
            var widthDifference = animalCoordinates[0] - currentAnimal.CurrentPosition[0];
            var heightDifference = animalCoordinates[1] - currentAnimal.CurrentPosition[1];
            var distance = Math.Sqrt((widthDifference * widthDifference) + (heightDifference * heightDifference));
            return Convert.ToInt32(distance);
        }

        public void NextLionAction(Animal lionToMove, Animal closestAntelope, List<int[]> possibleSpacesToMove)
        {
            var distance = FindDistanceBetweenTwoCoordinates(closestAntelope.CurrentPosition, lionToMove);
            if (distance == 1)
            {
                lionToMove.NextPosition = closestAntelope.CurrentPosition;
            }
            else
            {
                lionToMove.NextPosition = ClosestMoveToAntelope(possibleSpacesToMove, closestAntelope);
            }                
        }

        public int[] ClosestMoveToAntelope(List<int[]> freeSpaceToMove, Animal closestAntelope)
        {
            int minDistance = 10;
            int[] closestMoveCoordinate = new int[2];

            foreach (var space in freeSpaceToMove)
            {
                var distance = FindDistanceBetweenTwoCoordinates(space, closestAntelope);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestMoveCoordinate = space;
                }
            }
            return closestMoveCoordinate;
        }

        //public void LionEatAntelope(Animal lion, Animal antelope)
        //{
        //    lion.NextPosition = antelope.CurrentPosition;
        //    antelope.IsAlive = false;
        //}
    }
}

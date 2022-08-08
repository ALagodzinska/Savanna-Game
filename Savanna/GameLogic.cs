using Savanna.Entities;

namespace Savanna
{
    public class GameLogic
    {
        UserOutput userOutput = new();


        const int fieldHeight = 25;
        const int fieldWidth = 45;
        const int topStartPoint = 5;
        const ConsoleColor borderColor = ConsoleColor.DarkGreen;

        public GameField gameField = new GameField(fieldWidth, fieldHeight, topStartPoint, borderColor);



        public static List<Animal> animals = new List<Animal>();
        public void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public void PlayGame()
        {
            userOutput.DisplayGameRules();
            gameField.DrawBorder();
            NestedLoop();
        }

        public void NestedLoop()
        {
            bool exit = false;
            do
            {
                Thread.Sleep(1000);
                if (animals.Count != 0)
                {
                    MoveAllAnimalsToNextPosition();
                }

                Task.Factory.StartNew(() =>
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.A)
                    {
                        var createdAntelope = CreateNewAntelope();
                        DrawAnimal(createdAntelope);
                    }
                    if (key == ConsoleKey.L)
                    {
                        var createdLion = CreateNewLion();
                        DrawAnimal(createdLion);
                    }
                    if (key == ConsoleKey.Escape)
                    {
                        exit = true;
                    }
                });
            } while (!exit);
        }

        public Antelope CreateNewAntelope()
        {
            var createdAntelope = new Antelope();
            while (createdAntelope.CurrentPosition == null)
            {
                AddAnimalToGameField(createdAntelope);
            }
            animals.Add(createdAntelope);

            return createdAntelope;
        }

        public Lion CreateNewLion()
        {
            var createdLion = new Lion();
            while (createdLion.CurrentPosition == null)
            {
                AddAnimalToGameField(createdLion);
            }
            animals.Add(createdLion);

            return createdLion;
        }

        public void DrawAnimal(Animal animal)
        {
            if (animal.IsAlive == true)
            {
                Console.SetCursorPosition(animal.CurrentPosition[0] + 1, animal.CurrentPosition[1] + topStartPoint + 1);
                var symbol = animal.Type == "Lion" ? Convert.ToChar(2) : Convert.ToChar(1);
                Console.BackgroundColor = ConsoleColor.Black;
                if (animal.Type == "Lion")
                {
                    Console.ForegroundColor = animal.Ate == true ? ConsoleColor.DarkRed : ConsoleColor.Red;
                    animal.Ate = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(symbol);
            }

            Console.ResetColor();
        }

        public void AddAnimalToGameField(Animal animal)
        {
            Random random = new Random();
            var randomWidthPosition = random.Next(0, gameField.Width - 1);
            var randomHeightPosition = random.Next(0, gameField.Height - 1);
            int[] coordinates = new int[2] { randomWidthPosition, randomHeightPosition };
            if (!CheckIfPlaceIsTaken(coordinates))
            {
                animal.CurrentPosition = coordinates;
            }
        }

        public bool CheckIfPlaceIsTaken(int[] coordinates)
        {
            var foundAnimal = GetAnimalByCurrentCoordinates(coordinates);
            return foundAnimal != null ? true : false;
        }

        public bool CheckIfPlaceWillBeTakenInNextStep(int[] coordinates)
        {
            var foundAnimal = GetAnimalByNextCoordinates(coordinates);
            return foundAnimal != null ? true : false;
        }

        public Animal? GetAnimalByNextCoordinates(int[] coordinates)
        {
            var animal = animals.FirstOrDefault(a => a.NextPosition != null
            && a.NextPosition[0] == coordinates[0]
            && a.NextPosition[1] == coordinates[1]);
            return animal;
        }
        public Animal? GetAnimalByCurrentCoordinates(int[] coordinates)
        {
            var animal = animals.Where(a => a.IsAlive == true).FirstOrDefault(a => a.CurrentPosition != null
            && a.CurrentPosition[0] == coordinates[0]
            && a.CurrentPosition[1] == coordinates[1]);
            return animal;
        }
        public List<Animal> AnimalsInVisionRange(Animal animal)
        {
            int[] coordinates = new int[2];
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition[1] - animal.VisionRange; h <= animal.CurrentPosition[1] + animal.VisionRange; h++)
            {
                for (int w = animal.CurrentPosition[0] - animal.VisionRange; w <= animal.CurrentPosition[0] + animal.VisionRange; w++)
                {
                    if (h > gameField.Height || h < 0
                        || w > gameField.Width || w < 0
                        || h == animal.CurrentPosition[1] && w == animal.CurrentPosition[0])
                    {
                        continue;
                    }
                    coordinates[0] = w;
                    coordinates[1] = h;

                    var foundAnimal = GetAnimalByCurrentCoordinates(coordinates);
                    if (foundAnimal == null)
                    {
                        continue;
                    }

                    closestAnimalList.Add(foundAnimal);
                }
            }
            return closestAnimalList;
        }

        public void MoveAllAnimalsToNextPosition()
        {
            foreach (var animal in animals)
            {
                NextMoveForAnimals(animal);
            }
            gameField.DrawBorder();
            foreach (var animal in animals)
            {
                animal.CurrentPosition = animal.NextPosition;
                animal.NextPosition = null;
                DrawAnimal(animal);
            }
            animals.RemoveAll(a => a.IsAlive == false);

        }

        public void NextMoveForAnimals(Animal animal)
        {
            var closestAnimalList = AnimalsInVisionRange(animal);
            var movePossibility = PossibleMoves(animal);

            if (movePossibility.Count == 0)
            {
                animal.NextPosition = animal.CurrentPosition;
            }

            else if (animal.Type == "Lion")
            {
                var antelopesAround = closestAnimalList.FindAll(a => a.Type == "Antelope");

                if (antelopesAround.Count != 0)
                {
                    var closestAntelope = GetClosestAnimal(antelopesAround, animal);
                    NextLionAction(animal, closestAntelope, movePossibility);
                }
                else
                {
                    animal.NextPosition = RandomMove(movePossibility);
                }
            }
            else if (animal.Type == "Antelope")
            {
                var lionsAround = closestAnimalList.FindAll(a => a.Type == "Lion");

                if (lionsAround.Count != 0)
                {
                    animal.NextPosition = MoveFromLions(movePossibility, lionsAround, animal);
                }
                else
                {
                    animal.NextPosition = RandomMove(movePossibility);
                }
            }
        }

        public Animal GetClosestAnimal(List<Animal> animalsAround, Animal currentAnimal)
        {
            double minDistance = currentAnimal.VisionRange * 2;
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

                    if (h >= gameField.Height || h < 0
                        || w >= gameField.Width || w < 0
                        || h == animal.CurrentPosition[1] && w == animal.CurrentPosition[0]
                        || CheckIfPlaceIsTaken(foundCoordinates)
                        || CheckIfPlaceWillBeTakenInNextStep(foundCoordinates))
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

        public double FindDistanceBetweenTwoCoordinates(int[] animalCoordinates, Animal currentAnimal)
        {
            var widthDifference = animalCoordinates[0] - currentAnimal.CurrentPosition[0];
            var heightDifference = animalCoordinates[1] - currentAnimal.CurrentPosition[1];
            var distance = Math.Sqrt((widthDifference * widthDifference) + (heightDifference * heightDifference));
            return distance;
        }

        public void NextLionAction(Animal lionToMove, Animal closestAntelope, List<int[]> possibleSpacesToMove)
        {
            var distance = FindDistanceBetweenTwoCoordinates(closestAntelope.CurrentPosition, lionToMove);
            if (distance == 1 && !CheckIfPlaceWillBeTakenInNextStep(closestAntelope.CurrentPosition))
            {
                LionEatAntelope(lionToMove, closestAntelope);
            }
            else if (distance > 1 && distance < 2 && !CheckIfPlaceWillBeTakenInNextStep(closestAntelope.CurrentPosition))
            {
                lionToMove.NextPosition = closestAntelope.CurrentPosition;
            }
            else
            {
                lionToMove.NextPosition = MoveCloserToAntelope(possibleSpacesToMove, closestAntelope);
            }
        }

        public int[] MoveCloserToAntelope(List<int[]> freeSpaceToMove, Animal closestAntelope)
        {
            double minDistance = 10;
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

        public int[] MoveFromLions(List<int[]> freeSpaceToMove, List<Animal> lionsInTheVisionRange, Animal antelope)
        {
            var distances = new List<double[]>();
            double distanceMaxSum = 0;
            double distanceSum = 0;
            int[] maxDistance = new int[2];

            for (int l = 0; l < lionsInTheVisionRange.Count; l++)
            {
                var previousDistance = FindDistanceBetweenTwoCoordinates(lionsInTheVisionRange[l].CurrentPosition, antelope);
                double[] distancesUntilLions = new double[freeSpaceToMove.Count];
                for (int i = 0; i < freeSpaceToMove.Count; i++)
                {
                    var currentDistance = FindDistanceBetweenTwoCoordinates(freeSpaceToMove[i], lionsInTheVisionRange[l]);
                    if (currentDistance <= previousDistance)
                    {
                        distancesUntilLions[i] = 0.5;
                    }
                    else if (previousDistance <= 1 && currentDistance > previousDistance)
                    {
                        distancesUntilLions[i] = 4;
                    }
                    else if (previousDistance > 1 && previousDistance < 2 && currentDistance > previousDistance)
                    {
                        distancesUntilLions[i] = 3;
                    }
                    else if (previousDistance == 2 && currentDistance > previousDistance)
                    {
                        distancesUntilLions[i] = 1.5;
                    }
                    else if (previousDistance > 2 && currentDistance > previousDistance)
                    {
                        distancesUntilLions[i] = 1;
                    }
                }
                distances.Add(distancesUntilLions);
            }

            for (int i = 0; i < freeSpaceToMove.Count; i++)
            {
                foreach (var distance in distances)
                {
                    distanceSum += distance[i];
                }

                if (distanceSum > distanceMaxSum)
                {
                    distanceMaxSum = distanceSum;
                    maxDistance = freeSpaceToMove[i];
                }
                distanceSum = 0;
            }
            return maxDistance;
        }

        public void LionEatAntelope(Animal lion, Animal antelope)
        {
            lion.NextPosition = antelope.CurrentPosition;
            lion.Ate = true;
            antelope.IsAlive = false;
        }
    }
}

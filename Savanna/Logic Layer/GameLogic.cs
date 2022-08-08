namespace Savanna
{
    using Savanna.Entities;
    using Savanna.Entities.Animals;

    /// <summary>
    /// Contain methods that apply game rules and logic.
    /// </summary>
    public class GameLogic
    {
        UserOutput userOutput = new();
        GameField gameField = new();

        /// <summary>
        /// List to store all animals in game.
        /// </summary>
        private static List<Animal> animals = new();        

        /// <summary>
        /// Method to start the game.
        /// </summary>
        public void PlayGame()
        {
            userOutput.DisplayGameRules();
            gameField.DrawBorder();
            GameActions();
        }

        /// <summary>
        /// Contains and launches all possible methods for the game process.
        /// </summary>
        private void GameActions()
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
                    else if (key == ConsoleKey.L)
                    {
                        var createdLion = CreateNewLion();
                        DrawAnimal(createdLion);
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        exit = true;
                    }
                });
            } while (!exit);
        }

        /// <summary>
        /// Creates new antelope and adds it to the game.
        /// </summary>
        /// <returns>Created animal - antelope.</returns>
        private Antelope CreateNewAntelope()
        {
            var createdAntelope = new Antelope();

            while (createdAntelope.CurrentPosition == null)
            {
                AddAnimalToGameField(createdAntelope);
            }

            animals.Add(createdAntelope);

            return createdAntelope;
        }

        /// <summary>
        /// Creates new lion and adds it to the game.
        /// </summary>
        /// <returns>CReated animal - lion.</returns>
        private Lion CreateNewLion()
        {
            var createdLion = new Lion();

            while (createdLion.CurrentPosition == null)
            {
                AddAnimalToGameField(createdLion);
            }

            animals.Add(createdLion);

            return createdLion;
        }

        /// <summary>
        /// Randomly set new animal current position.
        /// </summary>
        /// <param name="animal">Created animal.</param>
        private void AddAnimalToGameField(Animal animal)
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

        /// <summary>
        /// Display animal on a game screen.
        /// </summary>
        /// <param name="animal">Animal from game.</param>
        private void DrawAnimal(Animal animal)
        {
            if (animal.IsAlive == true)
            {
                Console.SetCursorPosition(animal.CurrentPosition[0] + 1, animal.CurrentPosition[1] + gameField.TopPosition + 1);

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

        /// <summary>
        /// Check if place on a game field is taken by another animal.
        /// </summary>
        /// <param name="coordinates">Coordinates to place an animal.</param>
        /// <returns>True if place is taken, false if it is free.</returns>
        private bool CheckIfPlaceIsTaken(int[] coordinates)
        {
            var foundAnimal = GetAnimalByCurrentCoordinates(coordinates);

            return foundAnimal != null ? true : false;
        }

        /// <summary>
        /// Get animal by current position on a game field.
        /// </summary>
        /// <param name="coordinates">Animal current position.</param>
        /// <returns>Animal.</returns>
        private Animal? GetAnimalByCurrentCoordinates(int[] coordinates)
        {
            var animal = animals.Where(a => a.IsAlive == true).FirstOrDefault(a => a.CurrentPosition != null
            && a.CurrentPosition[0] == coordinates[0]
            && a.CurrentPosition[1] == coordinates[1]);

            return animal;
        }

        /// <summary>
        /// Check if place on a game field will be taken by another animal in next iteration.
        /// </summary>
        /// <param name="coordinates">Coordinates to place an animal.</param>
        /// <returns>True if place is taken, false if it is free.</returns>
        private bool CheckIfPlaceWillBeTakenInNextStep(int[] coordinates)
        {
            var foundAnimal = animals.FirstOrDefault(a => a.NextPosition != null
            && a.NextPosition[0] == coordinates[0]
            && a.NextPosition[1] == coordinates[1]);

            return foundAnimal != null ? true : false;
        }

        /// <summary>
        /// Finds next step for the animal and displays it to the user.
        /// </summary>
        private void MoveAllAnimalsToNextPosition()
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

        /// <summary>
        /// Based on animal type and location decides where next move should be done.
        /// </summary>
        /// <param name="animal">Animal to make a move.</param>
        private void NextMoveForAnimals(Animal animal)
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

        /// <summary>
        /// Finds all nearby animals in the vision range.
        /// </summary>
        /// <param name="animal">Animal whose nearest animals are to be found.</param>
        /// <returns>Returns list of animals that are in a vison range of one animal.</returns>
        private List<Animal> AnimalsInVisionRange(Animal animal)
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

        /// <summary>
        /// Finds free spaces around animal to make a next move.
        /// </summary>
        /// <param name="animal">One animal.</param>
        /// <returns>Return list with coordinates with free spaces to move.</returns>
        private List<int[]> PossibleMoves(Animal animal)
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

        /// <summary>
        /// Finds closest animal to animal.
        /// </summary>
        /// <param name="animalsAround">List with animals in the vision range.</param>
        /// <param name="currentAnimal">Animal whose nearest animal are to be found.</param>
        /// <returns>Nearest animal.</returns>
        private Animal? GetClosestAnimal(List<Animal> animalsAround, Animal currentAnimal)
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

        /// <summary>
        /// Make a random move on a game field.
        /// </summary>
        /// <param name="moves">List with available places to move for an animal.</param>
        /// <returns>Coordinates for next move.</returns>
        private int[] RandomMove(List<int[]> moves)
        {
            Random random = new Random();
            var moveIndex = random.Next(moves.Count);

            return moves[moveIndex];
        }

        /// <summary>
        /// Finds distance between two animals coordinates.
        /// </summary>
        /// <param name="animalCoordinates">Animal coordinates to which we are looking for distance.</param>
        /// <param name="currentAnimal">Animal from which we are looking for distance.</param>
        /// <returns>Return distance value between two coordinates.</returns>
        private double FindDistanceBetweenTwoCoordinates(int[] animalCoordinates, Animal currentAnimal)
        {
            var widthDifference = animalCoordinates[0] - currentAnimal.CurrentPosition[0];
            var heightDifference = animalCoordinates[1] - currentAnimal.CurrentPosition[1];
            var distance = Math.Sqrt((widthDifference * widthDifference) + (heightDifference * heightDifference));

            return distance;
        }

        /// <summary>
        /// Sets next lions move or action based on closest animal and free spaces to move.
        /// </summary>
        /// <param name="lionToMove">Lion to make a move.</param>
        /// <param name="closestAntelope">Nearest antelope in vision range.</param>
        /// <param name="possibleSpacesToMove">List of possible places to make a move.</param>
        private void NextLionAction(Animal lionToMove, Animal closestAntelope, List<int[]> possibleSpacesToMove)
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

        /// <summary>
        /// To choose lion closest place to catch nearest antelope.
        /// </summary>
        /// <param name="freeSpaceToMove">Possibilities for lion to move.</param>
        /// <param name="closestAntelope">Closest antelope to lion.</param>
        /// <returns>Coordinates for lion to make next move.</returns>
        private int[] MoveCloserToAntelope(List<int[]> freeSpaceToMove, Animal closestAntelope)
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

        /// <summary>
        /// Finds by distance points far place from all lions in the vision range.
        /// </summary>
        /// <param name="freeSpaceToMove">Possible places to move for antelope.</param>
        /// <param name="lionsInTheVisionRange">List of lions in the vision range.</param>
        /// <param name="antelope">Antelope to make a move.</param>
        /// <returns>Coordinates for antelope to run from lions.</returns>
        private int[] MoveFromLions(List<int[]> freeSpaceToMove, List<Animal> lionsInTheVisionRange, Animal antelope)
        {            
            double distanceMaxSum = 0;
            double distanceSum = 0;
            int[] maxDistance = new int[2];

            var distances = ReturnListOfDistancePoints(freeSpaceToMove, lionsInTheVisionRange, antelope);

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

        /// <summary>
        /// Collects all distance points that allows to find the most far place for antelope to move from lions.
        /// </summary>
        /// <param name="freeSpaceToMove">Possible places to move for antelope.</param>
        /// <param name="lionsInTheVisionRange">List of lions in the vision range.</param>
        /// <param name="antelope">Antelope to find distances.</param>
        /// <returns>List of arrays with distance points till lions on each free space for antelope to move.</returns>
        private List<double[]> ReturnListOfDistancePoints(List<int[]> freeSpaceToMove, List<Animal> lionsInTheVisionRange, Animal antelope)
        {
            var distances = new List<double[]>();

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

            return distances;
        }

        /// <summary>
        /// Eating action for lion when he gets close to antelope. Apply changes to object properties.
        /// </summary>
        /// <param name="lion">Lion.</param>
        /// <param name="antelope">Antelope near lion.</param>
        private void LionEatAntelope(Animal lion, Animal antelope)
        {
            lion.NextPosition = antelope.CurrentPosition;
            lion.Ate = true;
            antelope.IsAlive = false;
        }        
    }
}
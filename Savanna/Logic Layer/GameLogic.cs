namespace Savanna
{
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;
    using Savanna.Entities.Menu;

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

        private static List<Couple> couples = new();

        private static List<Animal> animalsToBeBorn = new();

        /// <summary>
        /// Variable used to declare if a user exits an application.
        /// </summary>
        bool exit = false;

        /// <summary>
        /// Starts game.
        /// </summary>
        public void Start()
        {
            Console.Title = "SAVANNA GAME";
            RunMainMenu();
        }

        /// <summary>
        /// Display main menu.
        /// </summary>
        private void RunMainMenu()
        {            

            while (!exit)
            {
                Console.Clear();

                var mainMenu = userOutput.MainMenu();
                var selectedOption = mainMenu.SelectFromMenu();

                switch (selectedOption.Index)
                {
                    case MainMenuOptions.PlayGame:
                        PlayGame();
                        break;
                    case MainMenuOptions.ExitGame:
                        ExitGame();
                        break;
                }
            }
        }

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
            Coordinates coordinates = new Coordinates { X = randomWidthPosition, Y = randomHeightPosition };

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
                Console.SetCursorPosition(animal.CurrentPosition.X + 1, animal.CurrentPosition.Y + gameField.TopPosition + 1);
                var isLion = animal.GetType() == typeof(Lion);

                var symbol = isLion ? Convert.ToChar(2) : Convert.ToChar(1);
                Console.BackgroundColor = ConsoleColor.Black;

                if (isLion)
                {
                    var lion = (Lion)animal;
                    Console.ForegroundColor = lion.Ate == true ? ConsoleColor.Red : ConsoleColor.DarkYellow;
                    //changes lion color when only 3 moves left
                    if (lion.Health < 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    lion.Ate = false;
                }
                else
                {
                    Console.ForegroundColor = animal.Health < 2 == true ? ConsoleColor.DarkGray : ConsoleColor.White;
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
        private bool CheckIfPlaceIsTaken(Coordinates coordinates)
        {
            var foundAnimal = GetAnimalByCurrentCoordinates(coordinates);

            return foundAnimal != null ? true : false;
        }

        /// <summary>
        /// Get animal by current position on a game field.
        /// </summary>
        /// <param name="coordinates">Animal current position.</param>
        /// <returns>Animal.</returns>
        private Animal? GetAnimalByCurrentCoordinates(Coordinates coordinates)
        {
            var animal = animals.Where(a => a.IsAlive == true).FirstOrDefault(a => a.CurrentPosition != null
            && a.CurrentPosition.X == coordinates.X
            && a.CurrentPosition.Y == coordinates.Y);

            return animal;
        }

        /// <summary>
        /// Check if place on a game field will be taken by another animal in next iteration.
        /// </summary>
        /// <param name="coordinates">Coordinates to place an animal.</param>
        /// <returns>True if place is taken, false if it is free.</returns>
        private bool CheckIfPlaceWillBeTakenInNextStep(Coordinates coordinates)
        {
            var foundAnimal = animals.FirstOrDefault(a => a.NextPosition != null
            && a.NextPosition.X == coordinates.X
            && a.NextPosition.Y == coordinates.Y);

            return foundAnimal != null ? true : false;
        }

        /// <summary>
        /// Finds next step for the animal and displays it to the user.
        /// </summary>
        private void MoveAllAnimalsToNextPosition()
        {
            foreach (var animal in animals)
            {
                NextPositionForAnimals(animal);
            }

            //check if together for next iteration
            CheckIfCouplesAreTogether();

            //create couple if together in this and next iteration
            foreach (var animal in animals)
            {
                CheckIfAnimalHaveCouple(animal);
            }                     

            gameField.DrawBorder();            

            foreach (var animal in animals)
            {
                animal.CurrentPosition = animal.NextPosition;
                animal.NextPosition = null;
                DrawAnimal(animal);
                animal.Health -= 0.5;
                if (animal.Health <= 0)
                {
                    animal.IsAlive = false;
                }
            }

            if (animalsToBeBorn.Count > 0)
            {
                foreach (var newborn in animalsToBeBorn)
                {
                    animals.Add(newborn);
                    DrawAnimal(newborn);
                }
                animalsToBeBorn.Clear();
            }

            animals.RemoveAll(a => a.IsAlive == false);
            couples.RemoveAll(c => c.BrokeUp == true);

            //Used to redraw field if in this iteration all animals died.
            if (animals.Count == 0)
            {
                Thread.Sleep(1000);
                gameField.DrawBorder();
            }
        }

        /// <summary>
        /// Based on animal type and location decides where next move should be done.
        /// </summary>
        /// <param name="animal">Animal to make a move.</param>
        private void NextPositionForAnimals(Animal animal)
        {
            var closestAnimalList = AnimalsInVisionRange(animal);
            var movePossibility = PossibleMoves(animal);

            if (movePossibility.Count == 0)
            {
                animal.NextPosition = animal.CurrentPosition;
            }
            else if (animal.GetType() == typeof(Lion))
            {
                var antelopes = closestAnimalList.OfType<Antelope>().ToList();

                if (antelopes.Count != 0)
                {
                    var closestAntelope = (Antelope)GetClosestAntelope(antelopes, animal);

                    if (closestAntelope != null)
                    {
                        NextLionAction((Lion)animal, closestAntelope, movePossibility);
                    }
                }
                else
                {
                    animal.NextPosition = RandomMove(movePossibility);
                }
            }
            else if (animal.GetType() == typeof(Antelope))
            {
                var lions = closestAnimalList.OfType<Lion>().ToList();

                animal.NextPosition = lions.Count != 0 ? MoveFromLions(movePossibility, lions, (Antelope)animal) : RandomMove(movePossibility);
            }
        }

        /// <summary>
        /// Finds all nearby animals in the vision range.
        /// </summary>
        /// <param name="animal">Animal whose nearest animals are to be found.</param>
        /// <returns>Returns list of animals that are in a vison range of one animal.</returns>
        private List<Animal> AnimalsInVisionRange(Animal animal)
        {
            Coordinates coordinates = new();
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition.Y - animal.VisionRange; h <= animal.CurrentPosition.Y + animal.VisionRange; h++)
            {
                for (int w = animal.CurrentPosition.X - animal.VisionRange; w <= animal.CurrentPosition.X + animal.VisionRange; w++)
                {
                    if (h > gameField.Height || h < 0
                        || w > gameField.Width || w < 0
                        || h == animal.CurrentPosition.Y && w == animal.CurrentPosition.X)
                    {
                        continue;
                    }

                    coordinates.X = w;
                    coordinates.Y = h;

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
        private List<Coordinates> PossibleMoves(Animal animal)
        {
            List<Coordinates> moves = new();

            for (int h = animal.CurrentPosition.Y - 1; h < animal.CurrentPosition.Y + 2; h++)
            {
                for (int w = animal.CurrentPosition.X - 1; w < animal.CurrentPosition.X + 2; w++)
                {
                    Coordinates foundCoordinates = new(){ X = w, Y = h };

                    if (h >= gameField.Height || h < 0
                        || w >= gameField.Width || w < 0
                        || h == animal.CurrentPosition.Y && w == animal.CurrentPosition.X
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
        private Animal? GetClosestAntelope(List<Antelope> animalsAround, Animal currentAnimal)
        {
            double minDistance = currentAnimal.VisionRange * 2;
            Coordinates closestAnimalCoordinats = new();

            foreach (var animal in animalsAround)
            {
                var distance = FindDistanceBetweenTwoCoordinates(animal.CurrentPosition, currentAnimal.CurrentPosition);

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
        private Coordinates RandomMove(List<Coordinates> moves)
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
        private double FindDistanceBetweenTwoCoordinates(Coordinates animalCoordinates, Coordinates currentAnimalCoordinates)
        {
            var widthDifference = animalCoordinates.X - currentAnimalCoordinates.X;
            var heightDifference = animalCoordinates.Y - currentAnimalCoordinates.Y;
            var distance = Math.Sqrt((widthDifference * widthDifference) + (heightDifference * heightDifference));

            return distance;
        }

        /// <summary>
        /// Sets next lions move or action based on closest animal and free spaces to move.
        /// </summary>
        /// <param name="lionToMove">Lion to make a move.</param>
        /// <param name="closestAntelope">Nearest antelope in vision range.</param>
        /// <param name="possibleSpacesToMove">List of possible places to make a move.</param>
        private void NextLionAction(Lion lionToMove, Antelope closestAntelope, List<Coordinates> possibleSpacesToMove)
        {
            var distance = FindDistanceBetweenTwoCoordinates(closestAntelope.CurrentPosition, lionToMove.CurrentPosition);

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
        private Coordinates MoveCloserToAntelope(List<Coordinates> freeSpaceToMove, Antelope closestAntelope)
        {
            double minDistance = 10;
            Coordinates closestMoveCoordinate = new();

            foreach (var space in freeSpaceToMove)
            {
                var distance = FindDistanceBetweenTwoCoordinates(space, closestAntelope.CurrentPosition);
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
        private Coordinates MoveFromLions(List<Coordinates> freeSpaceToMove, List<Lion> lionsInTheVisionRange, Antelope antelope)
        {
            double distanceMaxSum = 0;
            double distanceSum = 0;
            Coordinates maxDistance = new();

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
        private List<double[]> ReturnListOfDistancePoints(List<Coordinates> freeSpaceToMove, List<Lion> lionsInTheVisionRange, Antelope antelope)
        {
            var distances = new List<double[]>();

            for (int l = 0; l < lionsInTheVisionRange.Count; l++)
            {
                var previousDistance = FindDistanceBetweenTwoCoordinates(lionsInTheVisionRange[l].CurrentPosition, antelope.CurrentPosition);
                double[] distancesUntilLions = new double[freeSpaceToMove.Count];

                for (int i = 0; i < freeSpaceToMove.Count; i++)
                {
                    var currentDistance = FindDistanceBetweenTwoCoordinates(freeSpaceToMove[i], lionsInTheVisionRange[l].CurrentPosition);

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
        private void LionEatAntelope(Lion lion, Antelope antelope)
        {
            lion.NextPosition = antelope.CurrentPosition;
            lion.Ate = true;
            lion.Health += 10;
            antelope.IsAlive = false;
        }

        private void CheckIfAnimalHaveCouple(Animal mainAnimal)
        {
            var listWithCloseAnimalsOneType = ClosestAnimalsWithSameType(mainAnimal);
            if (listWithCloseAnimalsOneType.Count > 0)
            {
                foreach (var animal in listWithCloseAnimalsOneType)
                {
                    var couple = new Couple(mainAnimal, animal);

                    if (couples.FirstOrDefault(c => c.AnimalWithLargestID == couple.AnimalWithLargestID 
                    && c.AnimalWithSmallestID == couple.AnimalWithSmallestID) == null)
                    {
                        var distanceOnNextMove = FindDistanceBetweenTwoCoordinates(animal.NextPosition, mainAnimal.NextPosition);

                        if (distanceOnNextMove == 1 && mainAnimal.IsAlive == true && animal.IsAlive == true)
                        {
                            couples.Add(couple);
                        }
                    }                    
                }
            }
        }

        private void CheckIfCouplesAreTogether()
        {
            if(couples.Count > 0)
            {
                foreach(var couple in couples)
                {
                    var distanceOnMove = FindDistanceBetweenTwoCoordinates(couple.AnimalWithLargestID.CurrentPosition, couple.AnimalWithSmallestID.CurrentPosition);
                    if(distanceOnMove == 1)
                    {
                        couple.RoundsTogether++;
                        if(couple.RoundsTogether == 3)
                        {
                            AnimalToBeBorn(couple);
                            couple.BrokeUp = true;
                        }
                    }
                    else
                    {
                        couple.BrokeUp = true;
                    }
                }
            }            
        }

        private List<Animal> ClosestAnimalsWithSameType(Animal animal)
        {
            Coordinates coordinates = new Coordinates();
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition.Y - 1; h <= animal.CurrentPosition.Y + 1; h++)
            {
                for (int w = animal.CurrentPosition.X - 1; w <= animal.CurrentPosition.X + 1; w++)
                {
                    coordinates.X = w;
                    coordinates.Y = h;

                    var distance = FindDistanceBetweenTwoCoordinates(coordinates, animal.CurrentPosition);

                    if (h > gameField.Height || h < 0
                        || w > gameField.Width || w < 0
                        || h == animal.CurrentPosition.Y && w == animal.CurrentPosition.X
                        || distance > 1)
                    {
                        continue;
                    }

                    var foundAnimal = GetAnimalByCurrentCoordinates(coordinates);

                    if (foundAnimal != null && foundAnimal.GetType() == animal.GetType())
                    {
                        closestAnimalList.Add(foundAnimal);
                    }
                }
            }

            return closestAnimalList;
        }

        private void AnimalToBeBorn(Couple couple)
        {
            //return animal to be born 
            var bornAnimalCoordinates = GetPlaceToBorn(couple.AnimalWithLargestID, couple.AnimalWithSmallestID);

            if (couple.AnimalWithSmallestID.GetType() == typeof(Lion))
            {
                var createdLion = new Lion();
                createdLion.CurrentPosition = bornAnimalCoordinates;
                animalsToBeBorn.Add(createdLion);
            }
            else
            {
                var createdAntelope = new Antelope();
                createdAntelope.CurrentPosition = bornAnimalCoordinates;
                animalsToBeBorn.Add(createdAntelope);
            }
        }

        private List<Coordinates> GetListWithUniqueCoordinates(List<Coordinates> spacesAroundFirstParent, List<Coordinates> spacesAroundSecondParent)
        {
            for (int i = 0; i < spacesAroundFirstParent.Count; i++)
            {
                for (int j = 0; j < spacesAroundSecondParent.Count; j++)
                {
                    if (spacesAroundFirstParent[i].X == spacesAroundSecondParent[j].X 
                        && spacesAroundFirstParent[i].Y == spacesAroundSecondParent[j].Y)
                    {
                        spacesAroundSecondParent.Remove(spacesAroundSecondParent[j]);
                    }
                }
            }
            var list = spacesAroundFirstParent.Concat(spacesAroundSecondParent).ToList();

            return list;
        }

        private Coordinates GetPlaceToBorn(Animal mainAnimal, Animal sameTypeAnimalNear)
        {
            var animalMoves = PossibleMoves(mainAnimal);
            var sameAnimalTypeMoves = PossibleMoves(sameTypeAnimalNear);

            var listWithFreeSpaces = GetListWithUniqueCoordinates(animalMoves, sameAnimalTypeMoves);

            foreach (var move in listWithFreeSpaces)
            {
                if (CheckIfPlaceWillBeTakenInNextStep(move))
                {
                    listWithFreeSpaces.Remove(move);
                }
            }

            Random random = new Random();

            var placeToBornIndex = random.Next(0, listWithFreeSpaces.Count);

            return listWithFreeSpaces[placeToBornIndex];
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        private void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            exit = true;
            Environment.Exit(0);
        }
    }
}
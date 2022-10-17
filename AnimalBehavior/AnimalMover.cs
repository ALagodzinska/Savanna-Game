namespace Savanna.Logic_Layer
{
    using AnimalBehaviorInterfaces;
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;

    /// <summary>
    /// Class responsible for animal placement on a game field.
    /// </summary>
    public class AnimalMover: IAnimalMover
    {
        /// <summary>
        /// Height of game field.
        /// </summary>
        public int FieldHeight { get; set; }

        /// <summary>
        /// Width of game field.
        /// </summary>
        public int FieldWidth { get; set; }

        /// <summary>
        /// List of animals.
        /// </summary>
        public List<Animal> Animals { get; set; }

        /// <summary>
        /// Assign values to class properties.
        /// </summary>
        /// <param name="fieldHeight">Height of the game field.</param>
        /// <param name="fieldWidth">Width of the game field.</param>
        /// <param name="animals">List of animals.</param>
        public AnimalMover(int fieldHeight, int fieldWidth, List<Animal> animals)
        {
            FieldHeight = fieldHeight;
            FieldWidth = fieldWidth;
            Animals = animals;
        }

        /// <summary>
        /// Randomly set new animal current position.
        /// </summary>
        /// <param name="animal">Created animal.</param>
        public void SetNewAnimalCurrentPosition(Animal animal)
        {
            Random random = new Random();

            while (animal.CurrentPosition == null)
            {
                var randomWidthPosition = random.Next(0, FieldWidth - 1);
                var randomHeightPosition = random.Next(0, FieldHeight - 1);
                Coordinates coordinates = new Coordinates { X = randomWidthPosition, Y = randomHeightPosition };

                bool isTaken = GetAnimalByCurrentCoordinates(coordinates) != null ? true : false;
                if (!isTaken)
                {
                    animal.CurrentPosition = coordinates;
                }
            }
        }

        /// <summary>
        /// Get animal by coordinates on a game field.
        /// </summary>
        /// <param name="coordinates">Animal position.</param>
        /// <returns>Animal.</returns>
        public Animal? GetAnimalByCurrentCoordinates(Coordinates coordinates)
        {
            var animal = Animals.Where(a => a.IsAlive == true).FirstOrDefault(a => a.CurrentPosition != null
            && a.CurrentPosition.X == coordinates.X
            && a.CurrentPosition.Y == coordinates.Y);

            return animal;
        }

        /// <summary>
        /// Based on animal type and location sets next position for an animal.
        /// </summary>
        /// <param name="animal">Animal to set next position to.</param>
        public void SetNextPositionForAnimal(Animal animal)
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
                    var closestAntelope = GetClosestAntelope(antelopes, animal);

                    if (closestAntelope != null)
                    {
                        LionsNextAction((Lion)animal, closestAntelope, movePossibility);
                    }
                }
                else
                {
                    animal.NextPosition = RandomMovePosition(movePossibility);
                }
            }
            else if (animal.GetType() == typeof(Antelope))
            {
                var lions = closestAnimalList.OfType<Lion>().ToList();

                animal.NextPosition = lions.Count != 0 ? GetFarsetSpaceFromLion(movePossibility, lions, (Antelope)animal) : RandomMovePosition(movePossibility);
            }
        }

        /// <summary>
        /// Finds all nearby animals in the vision range.
        /// </summary>
        /// <param name="animal">Animal whose nearest animals are to be found.</param>
        /// <returns>Returns list of animals that are in a vison range of one animal.</returns>
        public List<Animal> AnimalsInVisionRange(Animal animal)
        {
            Coordinates coordinates = new();
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition.Y - animal.VisionRange; h <= animal.CurrentPosition.Y + animal.VisionRange; h++)
            {
                for (int w = animal.CurrentPosition.X - animal.VisionRange; w <= animal.CurrentPosition.X + animal.VisionRange; w++)
                {
                    if (h > FieldHeight || h < 0
                        || w > FieldWidth || w < 0
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
        /// <param name="animal">Animal.</param>
        /// <returns>Return list with coordinates with free spaces to move.</returns>
        public List<Coordinates> PossibleMoves(Animal animal)
        {
            List<Coordinates> moves = new();

            for (int h = animal.CurrentPosition.Y - 1; h < animal.CurrentPosition.Y + 2; h++)
            {
                for (int w = animal.CurrentPosition.X - 1; w < animal.CurrentPosition.X + 2; w++)
                {
                    Coordinates foundCoordinates = new() { X = w, Y = h };

                    if (h >= FieldHeight || h < 0
                        || w >= FieldWidth || w < 0
                        || h == animal.CurrentPosition.Y && w == animal.CurrentPosition.X
                        || (GetAnimalByCurrentCoordinates(foundCoordinates) != null)
                        || DoesPlaceWillBeTakenInNextStep(foundCoordinates))
                    {
                        continue;
                    }

                    moves.Add(foundCoordinates);
                }
            }

            return moves;
        }

        /// <summary>
        /// Check if place on a game field will be taken by another animal in next iteration.
        /// </summary>
        /// <param name="coordinates">Coordinates to place an animal.</param>
        /// <returns>True if place is taken, false if it is free.</returns>
        public bool DoesPlaceWillBeTakenInNextStep(Coordinates coordinates)
        {
            var foundAnimal = Animals.FirstOrDefault(a => a.NextPosition != null
            && a.NextPosition.X == coordinates.X
            && a.NextPosition.Y == coordinates.Y);

            return foundAnimal != null ? true : false;
        }

        /// <summary>
        /// Finds closest antelope to an animal.
        /// </summary>
        /// <param name="antelopesAround">List of antelopes in animals vision range.</param>
        /// <param name="currentAnimal">Animal.</param>
        /// <returns>Closest antelope to an animal.</returns>
        public Antelope? GetClosestAntelope(List<Antelope> antelopesAround, Animal currentAnimal)
        {
            double minDistance = currentAnimal.VisionRange * 2;
            Coordinates closestAnimalCoordinats = new();

            foreach (var animal in antelopesAround)
            {
                var distance = FindDistanceBetweenTwoCoordinates(animal.CurrentPosition, currentAnimal.CurrentPosition);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAnimalCoordinats = animal.CurrentPosition;
                }
            }

            return antelopesAround.FirstOrDefault(a => a.CurrentPosition == closestAnimalCoordinats);
        }

        /// <summary>
        /// Find random position on a game field for nrxt move.
        /// </summary>
        /// <param name="moves">List with available places to move for an animal.</param>
        /// <returns>Coordinates for next move.</returns>
        public Coordinates RandomMovePosition(List<Coordinates> moves)
        {
            Random random = new Random();
            var moveIndex = random.Next(moves.Count);

            return moves[moveIndex];
        }

        /// <summary>
        /// Finds distance between two animals coordinates.
        /// </summary>
        /// <param name="neighbourAnimalCoordinates">Animal coordinates to which we are looking for distance.</param>
        /// <param name="currentAnimalCoordinates">Animal from which we are looking for distance.</param>
        /// <returns>Return distance value between two coordinates.</returns>
        public double FindDistanceBetweenTwoCoordinates(Coordinates neighbourAnimalCoordinates, Coordinates currentAnimalCoordinates)
        {
            var widthDifference = neighbourAnimalCoordinates.X - currentAnimalCoordinates.X;
            var heightDifference = neighbourAnimalCoordinates.Y - currentAnimalCoordinates.Y;
            var distance = Math.Sqrt((widthDifference * widthDifference) + (heightDifference * heightDifference));

            return distance;
        }

        /// <summary>
        /// Sets next lions position for move or action based on closest animal and free spaces to move.
        /// </summary>
        /// <param name="lionToMove">Lion to make a move.</param>
        /// <param name="closestAntelope">Nearest antelope in vision range.</param>
        /// <param name="possibleSpacesToMove">List of possible places to make a move.</param>
        public void LionsNextAction(Lion lionToMove, Antelope closestAntelope, List<Coordinates> possibleSpacesToMove)
        {
            var distance = FindDistanceBetweenTwoCoordinates(closestAntelope.CurrentPosition, lionToMove.CurrentPosition);

            if (distance == 1 && !DoesPlaceWillBeTakenInNextStep(closestAntelope.CurrentPosition))
            {
                LionEatAntelope(lionToMove, closestAntelope);
            }
            else if (distance > 1 && distance < 2 && !DoesPlaceWillBeTakenInNextStep(closestAntelope.CurrentPosition))
            {
                lionToMove.NextPosition = closestAntelope.CurrentPosition;
            }
            else
            {
                lionToMove.NextPosition = GetClosestSpaceToAntelope(possibleSpacesToMove, closestAntelope);
            }
        }

        /// <summary>
        /// Eating action for lion when he gets close to antelope. Apply changes to object properties.
        /// </summary>
        /// <param name="lion">Lion.</param>
        /// <param name="antelope">Antelope near lion.</param>
        public void LionEatAntelope(Lion lion, Antelope antelope)
        {
            lion.NextPosition = antelope.CurrentPosition;
            lion.DoesAte = true;
            lion.Health += 10;
            antelope.IsAlive = false;
        }

        /// <summary>
        /// Get closest place for lion to catch nearest antelope.
        /// </summary>
        /// <param name="freeSpaceToMove">Possibilities for lion to move.</param>
        /// <param name="closestAntelope">Closest antelope to lion.</param>
        /// <returns>Coordinates for lion to make next move.</returns>
        public Coordinates GetClosestSpaceToAntelope(List<Coordinates> freeSpaceToMove, Antelope closestAntelope)
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
        /// Finds by distance points far place from all lions in the antelopes vision range.
        /// </summary>
        /// <param name="freeSpaceToMove">Possible places to move for antelope.</param>
        /// <param name="lionsInTheVisionRange">List of lions in the vision range.</param>
        /// <param name="antelope">Antelope to make a move.</param>
        /// <returns>Coordinates for antelope to run from lions.</returns>
        public Coordinates GetFarsetSpaceFromLion(List<Coordinates> freeSpaceToMove, List<Lion> lionsInTheVisionRange, Antelope antelope)
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
        public List<double[]> ReturnListOfDistancePoints(List<Coordinates> freeSpaceToMove, List<Lion> lionsInTheVisionRange, Antelope antelope)
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
        /// Apply logic for next animals move.
        /// </summary>
        /// <param name="animal">Animal to move.</param>
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
    }    
}
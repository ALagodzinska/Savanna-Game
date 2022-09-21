namespace Savanna.Logic_Layer
{
    using AnimalBehaviorInterfaces;
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;

    /// <summary>
    /// Responsible for animal pair creation logic and making animal babies.
    /// </summary>
    public class AnimalPairLogic: IAnimalPairLogic
    {
        /// <summary>
        /// List of animal pairs.
        /// </summary>
        public List<AnimalPair> AnimalPairs { get; set; }

        /// <summary>
        /// List of animal babies.
        /// </summary>
        public List<Animal> AnimalsToBeBorn { get; set; }

        /// <summary>
        /// Field to use AnimalMover logic.
        /// </summary>
        private IAnimalMover AnimalMovers { get; set; }

        /// <summary>
        /// Assign values to class properties.
        /// </summary>
        /// <param name="animalMover">Instance of AnimalMover class.</param>
        public AnimalPairLogic(IAnimalMover animalMover)
        {
            AnimalMovers = animalMover;
            AnimalPairs = new();
            AnimalsToBeBorn = new();
        }

        /// <summary>
        /// Applay pairs logic.
        /// </summary>
        public void AnimalPairsCreated()
        {
            //check if together for next iteration
            ActionForPairsOnMove();

            //create couple if together in this and next iteration
            foreach (var animal in AnimalMovers.Animals)
            {
                CheckIfAnimalHavePair(animal);
            }

            AnimalPairs.RemoveAll(c => c.DoesBrokeUp == true);
        }

        /// <summary>
        /// Apply logic for adding newborn animals to game.
        /// </summary>
        public void AddNewbornsToGame()
        {
            if (AnimalsToBeBorn.Count > 0)
            {
                foreach (var newborn in AnimalsToBeBorn)
                {
                    AnimalMovers.Animals.Add(newborn);
                }

                AnimalsToBeBorn.Clear();
            }
        }

        /// <summary>
        /// Create pair for animals that stand nearby, check if they will stay together in next iteration and adds them to pair list.
        /// </summary>
        /// <param name="mainAnimal">Animal to find a pair for.</param>
        private void CheckIfAnimalHavePair(Animal mainAnimal)
        {
            var listWithCloseAnimalsOneType = AnimalsNearbyWithSameType(mainAnimal);

            if (listWithCloseAnimalsOneType.Count > 0)
            {
                foreach (var closeAnimal in listWithCloseAnimalsOneType)
                {
                    if(closeAnimal.NextPosition == null || mainAnimal.NextPosition == null)
                    {
                        throw new Exception("Animals next position is not set.");
                    }

                    var animalPair = new AnimalPair(mainAnimal, closeAnimal);

                    if (AnimalPairs.FirstOrDefault(c => c.AnimalWithLargestID == animalPair.AnimalWithLargestID
                    && c.AnimalWithSmallestID == animalPair.AnimalWithSmallestID) == null)
                    {
                        var distanceOnNextMove = AnimalMovers.FindDistanceBetweenTwoCoordinates(closeAnimal.NextPosition, mainAnimal.NextPosition);

                        if (distanceOnNextMove == 1 && mainAnimal.IsAlive == true && closeAnimal.IsAlive == true)
                        {
                            AddNewPair(animalPair);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds new animal pair to an animal pair list.
        /// </summary>
        /// <param name="animalPair">Animal pair.</param>
        private void AddNewPair(AnimalPair animalPair) => AnimalPairs.Add(animalPair);

        /// <summary>
        /// Checks if animals stay together for the next round.
        /// </summary>
        private void ActionForPairsOnMove()
        {
            if (AnimalPairs.Count > 0)
            {
                foreach (var couple in AnimalPairs)
                {
                    var distanceOnMove = AnimalMovers.FindDistanceBetweenTwoCoordinates(couple.AnimalWithLargestID.CurrentPosition, couple.AnimalWithSmallestID.CurrentPosition);

                    if (distanceOnMove == 1)
                    {
                        couple.RoundsTogether++;
                        if (couple.RoundsTogether == 3)
                        {
                            AnimalToBeBorn(couple);
                            couple.DoesBrokeUp = true;
                        }
                    }
                    else
                    {
                        couple.DoesBrokeUp = true;
                    }
                }
            }
        }

        /// <summary>
        /// Find all animals nearby(next to each other) with the same type.
        /// </summary>
        /// <param name="animal">Animal to find neighbours for.</param>
        /// <returns>List of animals nearby.</returns>
        private List<Animal> AnimalsNearbyWithSameType(Animal animal)
        {
            var exceptions = new List<Exception>();

            if (animal.CurrentPosition == null)
            {
                exceptions.Add(new Exception("Current Position for animal is not set."));
            }

            if (animal.GetType() == typeof(Animal))
            {
                exceptions.Add(new Exception("Type for animal is not set."));
            }

            if (exceptions.Any())
            {
                throw new AggregateException("Invalid data", exceptions);
            }

            Coordinates coordinates = new Coordinates();
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition.Y - 1; h <= animal.CurrentPosition.Y + 1; h++)
            {
                for (int w = animal.CurrentPosition.X - 1; w <= animal.CurrentPosition.X + 1; w++)
                {
                    coordinates.X = w;
                    coordinates.Y = h;

                    var distance = AnimalMovers.FindDistanceBetweenTwoCoordinates(coordinates, animal.CurrentPosition);

                    if (h > AnimalMovers.FieldHeight || h < 0
                        || w > AnimalMovers.FieldWidth || w < 0
                        || h == animal.CurrentPosition.Y && w == animal.CurrentPosition.X
                        || distance > 1)
                    {
                        continue;
                    }

                    var foundAnimal = AnimalMovers.GetAnimalByCurrentCoordinates(coordinates);

                    if (foundAnimal != null && foundAnimal.GetType() == animal.GetType())
                    {
                        closestAnimalList.Add(foundAnimal);
                    }
                }
            }

            return closestAnimalList;
        }

        /// <summary>
        /// Creates new animal and add baby to the newborn list.
        /// </summary>
        /// <param name="animalPair">Couple to have a baby.</param>
        private void AnimalToBeBorn(AnimalPair animalPair)
        {
            //return place for animal to be born 
            var bornAnimalCoordinates = GetPlaceToBorn(animalPair.AnimalWithLargestID, animalPair.AnimalWithSmallestID);

            if(bornAnimalCoordinates != null)
            {
                if (animalPair.AnimalWithSmallestID.GetType() == typeof(Lion))
                {
                    var createdLion = new Lion();
                    createdLion.CurrentPosition = bornAnimalCoordinates;
                    AnimalsToBeBorn.Add(createdLion);
                }
                else
                {
                    var createdAntelope = new Antelope();
                    createdAntelope.CurrentPosition = bornAnimalCoordinates;
                    AnimalsToBeBorn.Add(createdAntelope);
                }
            }            
        }

        /// <summary>
        /// Finds free spaces around both parents, return list of free space without repetition.
        /// </summary>
        /// <param name="spacesAroundFirstParent">List of free spaces to move of first parent.</param>
        /// <param name="spacesAroundSecondParent">List of free spaces to move of second parent.</param>
        /// <returns>List of unrepetitive places to place a newborn animal to.</returns>
        private List<Coordinates> GetListWithUniqueFreeSpacesAroundParents(List<Coordinates> spacesAroundFirstParent, List<Coordinates> spacesAroundSecondParent)
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

        /// <summary>
        /// Gets position on the game field to add newborn animal.
        /// </summary>
        /// <param name="oneParent">One animal from the pair.</param>
        /// <param name="secondParent">Second animal from the pair.</param>
        /// <returns>Coordinates for newborn animals position on game field.</returns>
        private Coordinates? GetPlaceToBorn(Animal oneParent, Animal secondParent)
        {
            var exceptions = new List<Exception>();

            if (oneParent.CurrentPosition == null || secondParent.CurrentPosition == null)
            {
                exceptions.Add(new Exception("Current Position for animal is not set."));
            }

            if (oneParent.GetType() == typeof(Animal) || secondParent.GetType() == typeof(Animal))
            {
                exceptions.Add(new Exception("Type for animal is not set."));
            }

            if (exceptions.Any())
            {
                throw new AggregateException("Invalid data", exceptions);
            }

            var animalMoves = AnimalMovers.PossibleMoves(oneParent);
            var sameAnimalTypeMoves = AnimalMovers.PossibleMoves(secondParent);

            var listWithFreeSpaces = GetListWithUniqueFreeSpacesAroundParents(animalMoves, sameAnimalTypeMoves);

            if (listWithFreeSpaces.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var move in listWithFreeSpaces)
                {
                    if (AnimalMovers.DoesPlaceWillBeTakenInNextStep(move))
                    {
                        listWithFreeSpaces.Remove(move);
                    }
                }

                Random random = new Random();
                var placeToBornIndex = random.Next(0, listWithFreeSpaces.Count);

                return listWithFreeSpaces[placeToBornIndex];
            }
        }
    }
}

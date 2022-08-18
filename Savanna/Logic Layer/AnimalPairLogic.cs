namespace Savanna.Logic_Layer
{
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;

    /// <summary>
    /// Responsible for animal pair creation logic and making animal babies.
    /// </summary>
    public class AnimalPairLogic
    {
        /// <summary>
        /// List of animal pairs.
        /// </summary>
        public List<AnimalPair> animalPairs = new();

        /// <summary>
        /// List of animal babies.
        /// </summary>
        public List<Animal> animalsToBeBorn = new();

        /// <summary>
        /// Field to use AnimalMover logic.
        /// </summary>
        AnimalMover AnimalMover;

        /// <summary>
        /// Assign value to class properties.
        /// </summary>
        /// <param name="animalMover">Instance of AnimalMover class.</param>
        public AnimalPairLogic(AnimalMover animalMover)
        {
            AnimalMover = animalMover;
        }

        /// <summary>
        /// Create pair for animals that stand nearby, check if they will stay together in next iteration and adds them to pair list.
        /// </summary>
        /// <param name="mainAnimal">Animal to find a pair for.</param>
        public void CheckIfAnimalHavePair(Animal mainAnimal)
        {
            var listWithCloseAnimalsOneType = AnimalsNearbyWithSameType(mainAnimal);

            if (listWithCloseAnimalsOneType.Count > 0)
            {
                foreach (var closeAnimal in listWithCloseAnimalsOneType)
                {
                    var animalPair = new AnimalPair(mainAnimal, closeAnimal);

                    if (animalPairs.FirstOrDefault(c => c.AnimalWithLargestID == animalPair.AnimalWithLargestID
                    && c.AnimalWithSmallestID == animalPair.AnimalWithSmallestID) == null)
                    {
                        var distanceOnNextMove = AnimalMover.FindDistanceBetweenTwoCoordinates(closeAnimal.NextPosition, mainAnimal.NextPosition);

                        if (distanceOnNextMove == 1 && mainAnimal.IsAlive == true && closeAnimal.IsAlive == true)
                        {
                            animalPairs.Add(animalPair);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if animals stay together for the next round.
        /// </summary>
        public void CheckIfPairsAreTogether()
        {
            if (animalPairs.Count > 0)
            {
                foreach (var couple in animalPairs)
                {
                    var distanceOnMove = AnimalMover.FindDistanceBetweenTwoCoordinates(couple.AnimalWithLargestID.CurrentPosition, couple.AnimalWithSmallestID.CurrentPosition);

                    if (distanceOnMove == 1)
                    {
                        couple.RoundsTogether++;
                        if (couple.RoundsTogether == 3)
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

        /// <summary>
        /// Find all animals nearby(next to each other) with the same type.
        /// </summary>
        /// <param name="animal">Animal to find neighbours for.</param>
        /// <returns>List of animals nearby.</returns>
        private List<Animal> AnimalsNearbyWithSameType(Animal animal)
        {
            Coordinates coordinates = new Coordinates();
            List<Animal> closestAnimalList = new List<Animal>();

            for (int h = animal.CurrentPosition.Y - 1; h <= animal.CurrentPosition.Y + 1; h++)
            {
                for (int w = animal.CurrentPosition.X - 1; w <= animal.CurrentPosition.X + 1; w++)
                {
                    coordinates.X = w;
                    coordinates.Y = h;

                    var distance = AnimalMover.FindDistanceBetweenTwoCoordinates(coordinates, animal.CurrentPosition);

                    if (h > AnimalMover.FieldHeight || h < 0
                        || w > AnimalMover.FieldWidth || w < 0
                        || h == animal.CurrentPosition.Y && w == animal.CurrentPosition.X
                        || distance > 1)
                    {
                        continue;
                    }

                    var foundAnimal = AnimalMover.GetAnimalByCurrentCoordinates(coordinates);

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
                    animalsToBeBorn.Add(createdLion);
                }
                else
                {
                    var createdAntelope = new Antelope();
                    createdAntelope.CurrentPosition = bornAnimalCoordinates;
                    animalsToBeBorn.Add(createdAntelope);
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
            var animalMoves = AnimalMover.PossibleMoves(oneParent);
            var sameAnimalTypeMoves = AnimalMover.PossibleMoves(secondParent);

            var listWithFreeSpaces = GetListWithUniqueFreeSpacesAroundParents(animalMoves, sameAnimalTypeMoves);

            if (listWithFreeSpaces.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var move in listWithFreeSpaces)
                {
                    if (AnimalMover.CheckIfPlaceWillBeTakenInNextStep(move))
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

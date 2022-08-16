using Savanna.Entities.Animals;
using Savanna.Entities.GameField;

namespace Savanna.Logic_Layer
{
    public class AnimalPairLogic
    {
        public List<AnimalPair> animalPairs = new();

        public List<Animal> animalsToBeBorn = new();

        AnimalMover AnimalMover;

        public AnimalPairLogic(AnimalMover animalMover)
        {
            AnimalMover = animalMover;
        }
        public void CheckIfAnimalHavePair(Animal mainAnimal)
        {
            var listWithCloseAnimalsOneType = ClosestAnimalsWithSameType(mainAnimal);
            if (listWithCloseAnimalsOneType.Count > 0)
            {
                foreach (var animal in listWithCloseAnimalsOneType)
                {
                    var couple = new AnimalPair(mainAnimal, animal);

                    if (animalPairs.FirstOrDefault(c => c.AnimalWithLargestID == couple.AnimalWithLargestID
                    && c.AnimalWithSmallestID == couple.AnimalWithSmallestID) == null)
                    {
                        var distanceOnNextMove = AnimalMover.FindDistanceBetweenTwoCoordinates(animal.NextPosition, mainAnimal.NextPosition);

                        if (distanceOnNextMove == 1 && mainAnimal.IsAlive == true && animal.IsAlive == true)
                        {
                            animalPairs.Add(couple);
                        }
                    }
                }
            }
        }

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

        private void AnimalToBeBorn(AnimalPair animalPair)
        {
            //return animal to be born 
            var bornAnimalCoordinates = GetPlaceToBorn(animalPair.AnimalWithLargestID, animalPair.AnimalWithSmallestID);

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
            var animalMoves = AnimalMover.PossibleMoves(mainAnimal);
            var sameAnimalTypeMoves = AnimalMover.PossibleMoves(sameTypeAnimalNear);

            var listWithFreeSpaces = GetListWithUniqueCoordinates(animalMoves, sameAnimalTypeMoves);

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

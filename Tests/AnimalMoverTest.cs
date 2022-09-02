namespace Tests
{
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;
    using Savanna.Logic_Layer;

    public class AnimalMoverTest
    {
        [Fact]
        public void SetNewAnimalCurrentPosition_NewPosition_ValidPosition()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal();
            // test that animal position is empty
            Assert.Null(animal.CurrentPosition);
            // generate current position
            mover.SetNewAnimalCurrentPosition(animal);
            // check the position isn't null
            Assert.NotNull(animal.CurrentPosition);
            // check in range of field
            Assert.InRange(animal.CurrentPosition.X, 0, 10);
            Assert.InRange(animal.CurrentPosition.Y, 0, 10);
        }

        [Fact]
        public void SetNewAnimalCurrentPosition_PositionAlreadySet_DoesntChangePosition()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal();
            animal.CurrentPosition = new Coordinates()
            {
                X = 5,
                Y = 8
            };
            // generate current position
            mover.SetNewAnimalCurrentPosition(animal);
            // check the position isn't null
            Assert.NotNull(animal.CurrentPosition);
            // check the position did not change
            Assert.Equal(5, animal.CurrentPosition.X);
            Assert.Equal(8, animal.CurrentPosition.Y);
        }

        [Fact]
        public void SetNewAnimalCurrentPosition_NoFreeSpaceOnGameField_PositionIsNotSet()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
                new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 1} },
                new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 0} },
                new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
            };
            var mover = new AnimalMover(2, 2, listOfAnimals);            
            var animal = new Animal();

            // generate current position
            mover.SetNewAnimalCurrentPosition(animal);
            // check the position is not setted - is null
            Assert.Null(animal.CurrentPosition);
        }

        [Fact]
        public void GetAnimalByCurrentCoordinates_AnimalWithSuchCoordinatesExists_ReturnsAnimal()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { CurrentPosition = new Coordinates{ X = 8, Y = 5} },
            };
            var mover = new AnimalMover(10, 10, listOfAnimals);
            var coordinates = new Coordinates { X = 8, Y = 5 };

            // get animal by position
            var animal = mover.GetAnimalByCurrentCoordinates(coordinates);

            // check animal is not null
            Assert.NotNull(animal);
            // check the position have same values
            Assert.Equal(8, animal.CurrentPosition.X);
            Assert.Equal(5, animal.CurrentPosition.Y);
        }

        [Fact]
        public void GetAnimalByCurrentCoordinates_AnimalWithSuchCoordinatesDontExist_ReturnsNull()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var coordinates = new Coordinates { X = 8, Y = 5 };

            // get animal by position
            var animal = mover.GetAnimalByCurrentCoordinates(coordinates);

            // check animal is null
            Assert.Null(animal);
        }

        [Fact]
        public void GetAnimalByCurrentCoordinates_AnimalWithSuchCoordinatesIsDead_ReturnsNull()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { CurrentPosition = new Coordinates{ X = 8, Y = 5} , IsAlive = false},
            };
            var mover = new AnimalMover(10, 10, listOfAnimals);
            var coordinates = new Coordinates { X = 8, Y = 5 };

            // get animal by position
            var animal = mover.GetAnimalByCurrentCoordinates(coordinates);

            // check animal is null
            Assert.Null(animal);
        }

        [Fact]
        public void AnimalsInVisionRange_AntelopeSeeAnimalInVisionRange_ReturnListOfAnimalsInVisionRange()
        {
            var listOfAnimals = new List<Animal>
            {
                // the one it cant see
                new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
                // the one it can see
                new Animal { CurrentPosition = new Coordinates{ X = 3, Y = 1} },
            };

            var mover = new AnimalMover(4, 4, listOfAnimals);
            var animal = new Antelope { VisionRange = 1, CurrentPosition = new Coordinates { X = 2, Y = 1 } };

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is not empty
            Assert.NotEmpty(listOfAnimalsInVisionRange);
            // check if count of animals in the list is equals to one
            Assert.Single(listOfAnimalsInVisionRange);
            // check if this animal has coordinates values as expected
            Assert.Equal(3, listOfAnimalsInVisionRange.First().CurrentPosition.X);
            Assert.Equal(1, listOfAnimalsInVisionRange.First().CurrentPosition.Y);
        }

        [Fact]
        public void AnimalsInVisionRange_AntelopeDontSeeAnimalInVisionRange_ReturnEmptyList()
        {
            var mover = new AnimalMover(4, 4, new List<Animal>());
            var animal = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 1 } };

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is empty
            Assert.Empty(listOfAnimalsInVisionRange);
        }

        [Fact]
        public void AnimalsInVisionRange_LionSeeAnimalInVisionRange_ReturnListOfAnimalsInVisionRange()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
                new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
            };

            var mover = new AnimalMover(5, 5, listOfAnimals);
            var animal = new Lion { VisionRange = 2, CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is not empty
            Assert.NotEmpty(listOfAnimalsInVisionRange);
            // check count of animals in the list
            Assert.Equal(2, listOfAnimalsInVisionRange.Count);
        }

        [Fact]
        public void AnimalsInVisionRange_LionDontSeeAnimalInVisionRange_ReturnEmptyList()
        {
            var mover = new AnimalMover(4, 4, new List<Animal>());
            var animal = new Lion { CurrentPosition = new Coordinates { X = 2, Y = 1 } };

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is empty
            Assert.Empty(listOfAnimalsInVisionRange);
        }

        [Fact]
        public void AnimalsInVisionRange_AnimalHasNoType_ReturnEmpltyList()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
                new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
            };

            var mover = new AnimalMover(5, 5, listOfAnimals);
            var animal = new Animal { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is empty
            Assert.Empty(listOfAnimalsInVisionRange);
        }

        [Fact]
        public void AnimalsInVisionRange_AnimalCoordinatesNotInFieldRange_ReturnEmpltyList()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
                new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
            };

            var mover = new AnimalMover(5, 5, listOfAnimals);
            var animal = new Animal { CurrentPosition = new Coordinates { X = 6, Y = 7 } };

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is empty
            Assert.Empty(listOfAnimalsInVisionRange);
        }

        [Fact]
        public void AnimalsInVisionRange_AnimalCurrentPositionIsNull_ReturnEmpltyList()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());
            var animal = new Animal();

            // get animals around antelope
            var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);

            // check list is not null
            Assert.NotNull(listOfAnimalsInVisionRange);
            // check is empty
            Assert.Empty(listOfAnimalsInVisionRange);
        }

        [Fact]
        public void DoesPlaceWillBeTakenInNextStep_PlaceIsTakenByOtherAnimal_ReturnsTrue()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { NextPosition = new Coordinates{ X = 8, Y = 5} },
            };
            var mover = new AnimalMover(10, 10, listOfAnimals);
            var coordinates = new Coordinates { X = 8, Y = 5 };

            // check if place will be taken
            var isPlaceTaken = mover.DoesPlaceWillBeTakenInNextStep(coordinates);

            // check if is true
            Assert.True(isPlaceTaken);
        }

        [Fact]
        public void DoesPlaceWillBeTakenInNextStep_PlaceIsFree_ReturnsFalse()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var coordinates = new Coordinates { X = 8, Y = 5 };

            // check if place will be free
            var isPlaceTaken = mover.DoesPlaceWillBeTakenInNextStep(coordinates);

            // check if is false
            Assert.False(isPlaceTaken);
        }

        [Fact]
        public void PossibleMoves_AroundAnimalAreFreeSpacesToMove_ReturnListOfPossibleMoves()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal { CurrentPosition = new Coordinates { X = 5, Y = 5, } };

            // get possible moves
            var possibleMoves = mover.PossibleMoves(animal);

            // check list is not null
            Assert.NotNull(possibleMoves);
            // check is not empty
            Assert.NotEmpty(possibleMoves);
            // check count of animals in the list
            Assert.Equal(8, possibleMoves.Count);
        }

        [Fact]
        public void PossibleMoves_AroundAnimalNoFreeSpaces_ReturnEmptyList()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { NextPosition = new Coordinates{ X = 0, Y = 0} },
                new Animal { NextPosition = new Coordinates{ X = 0, Y = 1} },
                new Animal { NextPosition = new Coordinates{ X = 1, Y = 1} },

            };

            var mover = new AnimalMover(2, 2, listOfAnimals);
            var animal = new Animal { CurrentPosition = new Coordinates { X = 1, Y = 0, } };

            // get possible moves
            var possibleMoves = mover.PossibleMoves(animal);

            // check list is not null
            Assert.NotNull(possibleMoves);
            // check is not empty
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void PossibleMoves_AnimalCurrentPositionIsOutOfFieldRange_ReturnEmptyList()
        {
            var listOfAnimals = new List<Animal>
            {
                new Animal { NextPosition = new Coordinates{ X = 10, Y = 10} },

            };

            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal() { CurrentPosition = new Coordinates { X = 11, Y = 10} };

            var possibleMoves = mover.PossibleMoves(animal);

            Assert.NotNull(possibleMoves);
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void PossibleMoves_AnimalCurrentPositionIsNull_ReturnEmptyList()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal();

            var possibleMoves = mover.PossibleMoves(animal);

            Assert.NotNull(possibleMoves);
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void FindDistanceBetweenTwoCoordinates_Double_ReturnsAsExpected()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var coordinates1 = new Coordinates { X = 5, Y = 6 };
            var coordinates2 = new Coordinates { X = 7, Y = 7 };

            var distance = mover.FindDistanceBetweenTwoCoordinates(coordinates1, coordinates2);

            Assert.Equal(2.236, distance, 3);
        }

        [Fact]
        public void GetClosestAntelope_FoundClosestAntelope_ReturnsClosestAntelope()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());
            var listOfAntelopes = new List<Antelope>
            {
                new Antelope { CurrentPosition = new Coordinates{ X = 3, Y = 4} },
                new Antelope { CurrentPosition = new Coordinates{ X = 2, Y = 2} },
            };

            var animal = new Animal() { VisionRange = 2, CurrentPosition = new Coordinates { X = 4, Y = 3 } };

            var closestAntelope = mover.GetClosestAntelope(listOfAntelopes, animal);

            Assert.NotNull(closestAntelope);
            Assert.IsType<Antelope>(closestAntelope);
            Assert.Equal(3, closestAntelope.CurrentPosition.X);
            Assert.Equal(4, closestAntelope.CurrentPosition.Y);

        }

        [Fact]
        public void GetClosestAntelope_NoCloseAntelopes_ReturnsNull()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());

            var animal = new Animal() { VisionRange = 2, CurrentPosition = new Coordinates { X = 4, Y = 3 } };

            var closestAntelope = mover.GetClosestAntelope(new List<Antelope>(), animal);

            Assert.Null(closestAntelope);
        }

        [Fact]
        public void GetClosestAntelope_AnimalVisionRangeOrPositionIsNotSet_ThrowsExeption()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());

            var listOfAntelopes = new List<Antelope>
            {
                new Antelope { CurrentPosition = new Coordinates{ X = 3, Y = 4} },
                new Antelope { CurrentPosition = new Coordinates{ X = 2, Y = 2} },
            };
            var animal = new Animal();

            var result = Assert.Throws<Exception>(() => mover.GetClosestAntelope(listOfAntelopes, animal));

            Assert.Equal("Animal has null parameters VisionRange or/and CurrentPosition!", result.Message);
        }

        [Fact]
        public void RandomMovePosition_ListWithCoordinates_RandomPositionFromTheList()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());

            var listOfCoordinates = new List<Coordinates>
            {
                new Coordinates{X = 3, Y = 4},
                new Coordinates{X = 2, Y = 2},
                new Coordinates{X = 1, Y = 1},
            };

            var randomCoordinatesFromList = mover.RandomMovePosition(listOfCoordinates);
            Assert.Contains(randomCoordinatesFromList, listOfCoordinates);
        }

        [Fact]
        public void RandomMovePosition_EmptyList_RandomPositionFromTheList()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());

            var listOfCoordinates = new List<Coordinates>();

            var result = Assert.Throws<Exception>(() => mover.RandomMovePosition(listOfCoordinates));

            Assert.Equal("List with possible moves is empty, nothing to return.", result.Message);
        }
    }
}
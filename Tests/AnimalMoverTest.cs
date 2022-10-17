namespace Tests
{
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;
    using Savanna.Logic_Layer;

    public class AnimalMoverTest
    {
        private AnimalMover _animalMover;
        public AnimalMoverTest()
        {
            _animalMover = new AnimalMover(10, 10, new List<Animal>());
        }

        [Fact]
        public void SetNewAnimalCurrentPosition_NewPosition_ValidPosition()
        {
            var animal = new Animal();
            // check that animal position is empty
            Assert.Null(animal.CurrentPosition);
            // set current position
            _animalMover.SetNewAnimalCurrentPosition(animal);
            // check the position isn't null
            Assert.NotNull(animal.CurrentPosition);
            // check in range of field
            Assert.InRange(animal.CurrentPosition.X, 0, 10);
            Assert.InRange(animal.CurrentPosition.Y, 0, 10);
        }

        [Fact]
        public void SetNewAnimalCurrentPosition_PositionAlreadySet_DoesntChangePosition()
        {
            var animal = new Animal();
            animal.CurrentPosition = new Coordinates()
            {
                X = 5,
                Y = 8
            };
            // generate current position
            _animalMover.SetNewAnimalCurrentPosition(animal);
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
            var coordinates = new Coordinates { X = 8, Y = 5 };
            // get animal by position
            var animal = _animalMover.GetAnimalByCurrentCoordinates(coordinates);
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
            var coordinates = new Coordinates { X = 8, Y = 5 };
            // check if place will be free
            var isPlaceTaken = _animalMover.DoesPlaceWillBeTakenInNextStep(coordinates);
            // check if is false
            Assert.False(isPlaceTaken);
        }

        [Fact]
        public void PossibleMoves_AroundAnimalAreFreeSpacesToMove_ReturnListOfPossibleMoves()
        {
            var animal = new Animal { CurrentPosition = new Coordinates { X = 5, Y = 5, } };
            // get possible moves
            var possibleMoves = _animalMover.PossibleMoves(animal);
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
            var animal = new Animal() { CurrentPosition = new Coordinates { X = 11, Y = 10 } };
            // get possible moves
            var possibleMoves = _animalMover.PossibleMoves(animal);
            // check the list is not null but is empty
            Assert.NotNull(possibleMoves);
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void PossibleMoves_AnimalCurrentPositionIsNull_ReturnEmptyList()
        {
            var animal = new Animal();
            // get list of possible moves
            var possibleMoves = _animalMover.PossibleMoves(animal);
            //check if not null and empty
            Assert.NotNull(possibleMoves);
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void FindDistanceBetweenTwoCoordinates_Double_ReturnsAsExpected()
        {
            var coordinates1 = new Coordinates { X = 5, Y = 6 };
            var coordinates2 = new Coordinates { X = 7, Y = 7 };
            // get distance between two coordinates
            var distance = _animalMover.FindDistanceBetweenTwoCoordinates(coordinates1, coordinates2);
            // check if result is as expected
            Assert.Equal(2.236, distance, 3);
        }

        [Fact]
        public void MakeMove_AnimalAsExpected_AnimalPropertiesAreChanged()
        {
            var animal = new Animal
            {
                CurrentPosition = new Coordinates { X = 3, Y = 4 },
                NextPosition = new Coordinates { X = 4, Y = 7 },
                Health = 10
            };

            _animalMover.MakeMove(animal);

            Assert.Equal(4, animal.CurrentPosition.X);
            Assert.Equal(7, animal.CurrentPosition.Y);
            Assert.Null(animal.NextPosition);
            Assert.Equal(9.5, animal.Health);
            Assert.True(animal.IsAlive);
        }

        [Fact]
        public void MakeMove_AnimalWillDie_PropertiesForIsAliveIsChanged()
        {
            var animal = new Animal
            {
                CurrentPosition = new Coordinates { X = 3, Y = 4 },
                NextPosition = new Coordinates { X = 4, Y = 7 },
                Health = 0.5
            };

            _animalMover.MakeMove(animal);

            Assert.Equal(4, animal.CurrentPosition.X);
            Assert.Equal(7, animal.CurrentPosition.Y);
            Assert.Null(animal.NextPosition);
            Assert.Equal(0, animal.Health);
            Assert.False(animal.IsAlive);
        }

        [Fact]
        public void MakeMove_AnimalHasNoPropertiesSet_ThrowAnException()
        {
            var animal = new Animal();

            var result = Assert.Throws<AggregateException>(() => _animalMover.MakeMove(animal));
            Assert.Equal("Invalid data (Current Position for an animal is not set.) (Next Position for an animal is not set.)", result.Message);
        }

        [Fact]
        public void SetNextPositionForAnimal_NoFreeSpacesToMove_AnimalStaysOnSamePosition()
        {
            var animals = new List<Animal>
            {
                new Animal { NextPosition = new Coordinates{ X = 0, Y = 0} },
                new Animal { NextPosition = new Coordinates{ X = 0, Y = 1 } },
                new Animal { NextPosition = new Coordinates{ X = 1, Y = 0 } },
            };
            var mover = new AnimalMover(2, 2, animals);
            var animal = new Antelope { CurrentPosition = new Coordinates { X = 1, Y = 1}};

            mover.SetNextPositionForAnimal(animal);

            Assert.NotNull(animal.NextPosition);
            Assert.Equal(1, animal.NextPosition.X);
            Assert.Equal(1, animal.NextPosition.Y);
        }

        [Fact]
        public void SetNextPositionForAnimal_LionHasAntelopeInVisionRange_LionMoveCloserToAntelope()
        {
            var animals = new List<Animal>
            {
                new Antelope { CurrentPosition = new Coordinates {X = 1, Y = 1 },
                               NextPosition = new Coordinates{ X = 0, Y = 0 } },
            };

            var mover = new AnimalMover(5, 5, animals);
            var animal = new Lion { VisionRange = 2, CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            mover.SetNextPositionForAnimal(animal);

            Assert.NotNull(animal.NextPosition);
            Assert.Equal(1, animal.NextPosition.X);
            Assert.Equal(1, animal.NextPosition.Y);
        }

        [Fact]
        public void SetNextPositionForAnimal_LionDontSeeAntelope_LionMoveRandomly()
        {
            var animal = new Lion { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            _animalMover.SetNextPositionForAnimal(animal);

            Assert.NotNull(animal.NextPosition);
            Assert.InRange(animal.NextPosition.X, 1, 3);
            Assert.InRange(animal.NextPosition.Y, 1, 3);
        }

        [Fact]
        public void SetNextPositionForAnimal_AntelopeSeeLion_AntelopeMoveFromLions()
        {
            var animals = new List<Animal>
            {
                new Lion { CurrentPosition = new Coordinates {X = 1, Y = 1 } },
                new Lion { CurrentPosition = new Coordinates {X = 1, Y = 3 } },
                new Lion { CurrentPosition = new Coordinates {X = 3, Y = 1 } },
            };

            var mover = new AnimalMover(5, 5, animals);
            var animal = new Antelope { VisionRange = 2, CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            mover.SetNextPositionForAnimal(animal);

            Assert.NotNull(animal.NextPosition);
            Assert.Equal(3, animal.NextPosition.X);
            Assert.Equal(3, animal.NextPosition.Y);
        }

        [Fact]
        public void SetNextPositionForAnimal_AntelopeDontSeeLions_AntelopeMoveRandomly()
        {
            var animal = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            _animalMover.SetNextPositionForAnimal(animal);

            Assert.NotNull(animal.NextPosition);
            Assert.InRange(animal.NextPosition.X, 1, 3);
            Assert.InRange(animal.NextPosition.Y, 1, 3);
        }

        [Fact]
        public void SetNextPositionForAnimal_AnimalHasNoTypePropertiesAreNotSet_ThrowException()
        {
            var animal = new Animal();

            var result = Assert.Throws<AggregateException>(() => _animalMover.SetNextPositionForAnimal(animal));
            Assert.Equal($"Invalid data (Current Position for animal is not set.) (Type for animal is not set.)", result.Message);
        }

        [Fact]
        public void AnimalsExceptions_AnimalsCurrentPositionIsNotSet_ThrowException()
        {
            var animal = new Antelope();

            var result = Assert.Throws<AggregateException>(() => _animalMover.AnimalsExceptions(animal));
            Assert.Equal($"Invalid data (Current Position for animal is not set.)", result.Message);
        }

        [Fact]
        public void AnimalsExceptions_AnimalsTypeIsNotSet_ThrowException()
        {
            var animal = new Animal { CurrentPosition = new Coordinates { X= 5, Y = 3} };

            var result = Assert.Throws<AggregateException>(() => _animalMover.SetNextPositionForAnimal(animal));
            Assert.Equal($"Invalid data (Type for animal is not set.)", result.Message);
        }
    }
}
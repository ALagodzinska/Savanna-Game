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
            // check that animal position is empty
            Assert.Null(animal.CurrentPosition);
            // set current position
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

        //[Fact]
        //public void AnimalsInVisionRange_AntelopeSeeAnimalInVisionRange_ReturnListOfAnimalsInVisionRange()
        //{
        //    var listOfAnimals = new List<Animal>
        //    {
        //        // the one it cant see
        //        new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
        //        // the one it can see
        //        new Animal { CurrentPosition = new Coordinates{ X = 3, Y = 1} },
        //    };
        //    var mover = new AnimalMover(4, 4, listOfAnimals);
        //    var animal = new Antelope { VisionRange = 1, CurrentPosition = new Coordinates { X = 2, Y = 1 } };
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is not empty
        //    Assert.NotEmpty(listOfAnimalsInVisionRange);
        //    // check if count of animals in the list is equals to one
        //    Assert.Single(listOfAnimalsInVisionRange);
        //    // check if this animal has coordinates values as expected
        //    Assert.Equal(3, listOfAnimalsInVisionRange.First().CurrentPosition.X);
        //    Assert.Equal(1, listOfAnimalsInVisionRange.First().CurrentPosition.Y);
        //}

        //[Fact]
        //public void AnimalsInVisionRange_AntelopeDontSeeAnimalInVisionRange_ReturnEmptyList()
        //{
        //    var mover = new AnimalMover(4, 4, new List<Animal>());
        //    var animal = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 1 } };
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is empty
        //    Assert.Empty(listOfAnimalsInVisionRange);
        //}

        //[Fact]
        //public void AnimalsInVisionRange_LionSeeAnimalInVisionRange_ReturnListOfAnimalsInVisionRange()
        //{
        //    var listOfAnimals = new List<Animal>
        //    {
        //        new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
        //        new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
        //    };
        //    var mover = new AnimalMover(5, 5, listOfAnimals);
        //    var animal = new Lion { VisionRange = 2, CurrentPosition = new Coordinates { X = 2, Y = 2 } };
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is not empty
        //    Assert.NotEmpty(listOfAnimalsInVisionRange);
        //    // check count of animals in the list
        //    Assert.Equal(2, listOfAnimalsInVisionRange.Count);
        //}

        //[Fact]
        //public void AnimalsInVisionRange_LionDontSeeAnimalInVisionRange_ReturnEmptyList()
        //{
        //    var mover = new AnimalMover(4, 4, new List<Animal>());
        //    var animal = new Lion { CurrentPosition = new Coordinates { X = 2, Y = 1 } };
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is empty
        //    Assert.Empty(listOfAnimalsInVisionRange);
        //}

        //[Fact]
        //public void AnimalsInVisionRange_AnimalHasNoType_ReturnEmpltyList()
        //{
        //    var listOfAnimals = new List<Animal>
        //    {
        //        new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
        //        new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
        //    };
        //    var mover = new AnimalMover(5, 5, listOfAnimals);
        //    var animal = new Animal { CurrentPosition = new Coordinates { X = 2, Y = 2 } };
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is empty
        //    Assert.Empty(listOfAnimalsInVisionRange);
        //}

        //[Fact]
        //public void AnimalsInVisionRange_AnimalCoordinatesNotInFieldRange_ReturnEmpltyList()
        //{
        //    var listOfAnimals = new List<Animal>
        //    {
        //        new Animal { CurrentPosition = new Coordinates{ X = 0, Y = 0} },
        //        new Animal { CurrentPosition = new Coordinates{ X = 1, Y = 1} },
        //    };
        //    var mover = new AnimalMover(5, 5, listOfAnimals);
        //    var animal = new Animal { CurrentPosition = new Coordinates { X = 6, Y = 7 } };
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is empty
        //    Assert.Empty(listOfAnimalsInVisionRange);
        //}

        //[Fact]
        //public void AnimalsInVisionRange_AnimalCurrentPositionIsNull_ReturnEmpltyList()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());
        //    var animal = new Animal();
        //    // get animals around antelope
        //    var listOfAnimalsInVisionRange = mover.AnimalsInVisionRange(animal);
        //    // check list is not null
        //    Assert.NotNull(listOfAnimalsInVisionRange);
        //    // check is empty
        //    Assert.Empty(listOfAnimalsInVisionRange);
        //}

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
            var animal = new Animal() { CurrentPosition = new Coordinates { X = 11, Y = 10 } };
            // get possible moves
            var possibleMoves = mover.PossibleMoves(animal);
            // check the list is not null but is empty
            Assert.NotNull(possibleMoves);
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void PossibleMoves_AnimalCurrentPositionIsNull_ReturnEmptyList()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal();
            // get list of possible moves
            var possibleMoves = mover.PossibleMoves(animal);
            //check if not null and empty
            Assert.NotNull(possibleMoves);
            Assert.Empty(possibleMoves);
        }

        [Fact]
        public void FindDistanceBetweenTwoCoordinates_Double_ReturnsAsExpected()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var coordinates1 = new Coordinates { X = 5, Y = 6 };
            var coordinates2 = new Coordinates { X = 7, Y = 7 };
            // get distance between two coordinates
            var distance = mover.FindDistanceBetweenTwoCoordinates(coordinates1, coordinates2);
            // check if result is as expected
            Assert.Equal(2.236, distance, 3);
        }

        //[Fact]
        //public void GetClosestAntelope_FoundClosestAntelope_ReturnsClosestAntelope()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());
        //    var listOfAntelopes = new List<Antelope>
        //    {
        //        // closest antelope
        //        new Antelope { CurrentPosition = new Coordinates{ X = 3, Y = 4} },
        //        new Antelope { CurrentPosition = new Coordinates{ X = 2, Y = 2} },
        //    };
        //    var animal = new Animal() { VisionRange = 2, CurrentPosition = new Coordinates { X = 4, Y = 3 } };
        //    // get closest antelope
        //    var closestAntelope = mover.GetClosestAntelope(listOfAntelopes, animal);
        //    // check if antelope is the one expected
        //    Assert.NotNull(closestAntelope);
        //    Assert.IsType<Antelope>(closestAntelope);
        //    Assert.Equal(3, closestAntelope.CurrentPosition.X);
        //    Assert.Equal(4, closestAntelope.CurrentPosition.Y);

        //}

        //[Fact]
        //public void GetClosestAntelope_NoCloseAntelopes_ReturnsNull()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());

        //    var animal = new Animal() { VisionRange = 2, CurrentPosition = new Coordinates { X = 4, Y = 3 } };

        //    var closestAntelope = mover.GetClosestAntelope(new List<Antelope>(), animal);

        //    Assert.Null(closestAntelope);
        //}

        //[Fact]
        //public void GetClosestAntelope_AnimalPositionIsNotSet_ThrowsExeption()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());

        //    var listOfAntelopes = new List<Antelope>
        //    {
        //        new Antelope { CurrentPosition = new Coordinates{ X = 3, Y = 4} },
        //        new Antelope { CurrentPosition = new Coordinates{ X = 2, Y = 2} },
        //    };
        //    var animal = new Animal();

        //    var result = Assert.Throws<Exception>(() => mover.GetClosestAntelope(listOfAntelopes, animal));

        //    Assert.Equal("Animal has null parameter CurrentPosition!", result.Message);
        //}

        //[Fact]
        //public void RandomMovePosition_ListWithCoordinates_RandomPositionFromTheList()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());

        //    var listOfCoordinates = new List<Coordinates>
        //    {
        //        new Coordinates{X = 3, Y = 4},
        //        new Coordinates{X = 2, Y = 2},
        //        new Coordinates{X = 1, Y = 1},
        //    };

        //    var randomCoordinatesFromList = mover.RandomMovePosition(listOfCoordinates);
        //    Assert.Contains(randomCoordinatesFromList, listOfCoordinates);
        //}

        //[Fact]
        //public void RandomMovePosition_EmptyList_RandomPositionFromTheList()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());

        //    var listOfCoordinates = new List<Coordinates>();

        //    var result = Assert.Throws<Exception>(() => mover.RandomMovePosition(listOfCoordinates));

        //    Assert.Equal("List with possible moves is empty, nothing to return.", result.Message);
        //}

        //[Fact]
        //public void GetClosestSpaceToAntelope_ClosestSpace_ExpectedPosition()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());
        //    var listOfSpacesToMove = new List<Coordinates>
        //    {
        //        new Coordinates{ X = 2, Y = 2},
        //        //closest coordinates to antelope
        //        new Coordinates{ X = 3, Y = 5},
        //        new Coordinates{ X = 4, Y = 2},
        //    };
        //    var lion = new Lion();
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 5 } };
        //    // find closest space
        //    var closestPlace = mover.GetClosestSpaceToAntelope(listOfSpacesToMove, antelope, lion);
        //    // check if not null
        //    Assert.NotNull(closestPlace);
        //    // check if are expected values
        //    Assert.Equal(3, closestPlace.X);
        //    Assert.Equal(5, closestPlace.Y);
        //}

        //[Fact]
        //public void GetClosestSpaceToAntelope_NoSpacesForMove_ExecptionIsThrown()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());
        //    var listOfSpacesToMove = new List<Coordinates>();
        //    var lion = new Lion();
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 5 } };
        //    // check throws an exception
        //    var result = Assert.Throws<AggregateException>(() => mover.GetClosestSpaceToAntelope(listOfSpacesToMove, antelope, lion));
        //    Assert.Equal("Invalid data (List with spaces to move is empty.)", result.Message);
        //}

        //[Fact]
        //public void GetClosestSpaceToAntelope_AntelopesPositionIsNotSet_ExecptionIsThrown()
        //{
        //    var mover = new AnimalMover(5, 5, new List<Animal>());
        //    var listOfSpacesToMove = new List<Coordinates>
        //    {
        //        new Coordinates{ X = 3, Y = 5},
        //        new Coordinates{ X = 4, Y = 2},
        //    };
        //    var lion = new Lion();
        //    var antelope = new Antelope();
        //    // check throws an exception
        //    var result = Assert.Throws<AggregateException>(() => mover.GetClosestSpaceToAntelope(listOfSpacesToMove, antelope, lion));
        //    Assert.Equal("Invalid data (Antelopes position is not set.)", result.Message);
        //}

        //[Fact]
        //public void LionEatAntelope_ExpectedAnimals_UpdatedAnimalProperties()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion { Health = 30 };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 6 } };

        //    mover.LionEatAntelope(lion, antelope);

        //    Assert.True(lion.DoesAte);
        //    Assert.False(antelope.IsAlive);
        //    Assert.Equal(40, lion.Health);

        //    Assert.Equal(5, lion.NextPosition.X);
        //    Assert.Equal(6, lion.NextPosition.Y);
        //}

        //[Fact]
        //public void LionEatAntelope_AntelopeIsAlreadyDead_PropertiesDidNotChange()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion { Health = 30 };
        //    var antelope = new Antelope { IsAlive = false, CurrentPosition = new Coordinates { X = 5, Y = 6 } };

        //    mover.LionEatAntelope(lion, antelope);

        //    Assert.False(lion.DoesAte);
        //    Assert.False(antelope.IsAlive);
        //    Assert.Equal(30, lion.Health);

        //    Assert.Null(lion.NextPosition);
        //}

        //[Fact]
        //public void LionEatAntelope_AntelopePositionIsNull_ExceptionIsThrown()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion { Health = 30 };
        //    var antelope = new Antelope { IsAlive = false };

        //    var result = Assert.Throws<Exception>(() => mover.LionEatAntelope(lion, antelope));
        //    Assert.Equal("Antelopes CurrentPosition is null!", result.Message);
        //}

        [Fact]
        public void MakeMove_AnimalAsExpected_AnimalPropertiesAreChanged()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal
            {
                CurrentPosition = new Coordinates { X = 3, Y = 4 },
                NextPosition = new Coordinates { X = 4, Y = 7 },
                Health = 10
            };

            mover.MakeMove(animal);

            Assert.Equal(4, animal.CurrentPosition.X);
            Assert.Equal(7, animal.CurrentPosition.Y);
            Assert.Null(animal.NextPosition);
            Assert.Equal(9.5, animal.Health);
            Assert.True(animal.IsAlive);
        }

        [Fact]
        public void MakeMove_AnimalWillDie_PropertiesForIsAliveIsChanged()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal
            {
                CurrentPosition = new Coordinates { X = 3, Y = 4 },
                NextPosition = new Coordinates { X = 4, Y = 7 },
                Health = 0.5
            };

            mover.MakeMove(animal);

            Assert.Equal(4, animal.CurrentPosition.X);
            Assert.Equal(7, animal.CurrentPosition.Y);
            Assert.Null(animal.NextPosition);
            Assert.Equal(0, animal.Health);
            Assert.False(animal.IsAlive);
        }

        [Fact]
        public void MakeMove_AnimalHasNoPropertiesSet_ThrowAnException()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var animal = new Animal();

            var result = Assert.Throws<AggregateException>(() => mover.MakeMove(animal));
            Assert.Equal("Invalid data (Current Position for an animal is not set.) (Next Position for an animal is not set.)", result.Message);
        }

        //[Fact]
        //public void LionsNextAction_LionCloseToAntelope_LionEatsAntelope()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion { CurrentPosition = new Coordinates { X = 6, Y = 7 }, Health = 30 };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 7 } };
        //    var spacesToMove = new List<Coordinates>();

        //    mover.LionsNextAction(lion, antelope, spacesToMove);

        //    Assert.True(lion.DoesAte);
        //    Assert.False(antelope.IsAlive);
        //    Assert.Equal(40, lion.Health);

        //    Assert.Equal(5, lion.NextPosition.X);
        //    Assert.Equal(7, lion.NextPosition.Y);
        //}

        //[Fact]
        //public void LionsNextAction_LionIsOnDiagonalToAntelope_LionTakesAntelopesPlace()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion { CurrentPosition = new Coordinates { X = 4, Y = 8 } };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 7 } };
        //    var spacesToMove = new List<Coordinates>();

        //    mover.LionsNextAction(lion, antelope, spacesToMove);

        //    Assert.Equal(5, lion.NextPosition.X);
        //    Assert.Equal(7, lion.NextPosition.Y);
        //}

        //[Fact]
        //public void LionsNextAction_LionSeeClosestAntelope_LionChooseClosestPosition()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion { CurrentPosition = new Coordinates { X = 4, Y = 8 } };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 6, Y = 7 } };
        //    var spacesToMove = new List<Coordinates>
        //    {
        //        new Coordinates { X = 4, Y = 7 },
        //        // closest position
        //        new Coordinates { X = 5, Y = 8 },
        //        new Coordinates { X = 5, Y = 9 }
        //    };

        //    mover.LionsNextAction(lion, antelope, spacesToMove);

        //    Assert.Equal(5, lion.NextPosition.X);
        //    Assert.Equal(8, lion.NextPosition.Y);
        //}

        //[Fact]
        //public void LionsNextAction_AnimalPropertiesIsNotSet_ThrowsException()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var lion = new Lion();
        //    var antelope = new Antelope();
        //    var spacesToMove = new List<Coordinates>();

        //    var result = Assert.Throws<AggregateException>(() => mover.LionsNextAction(lion, antelope, spacesToMove));
        //    Assert.Equal("Invalid data (Current Position for lion is not set.) (Next Position for antelope is not set.)", result.Message);
        //}

        //[Fact]
        //public void ReturnListOfDistancePoints_ListOfPoints_ReturnExpectedListOfPoints()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var spacesForMove = new List<Coordinates>
        //    {
        //        new Coordinates{ X = 3, Y = 2},
        //        new Coordinates{ X = 1, Y = 3},

        //    };
        //    var lions = new List<Lion>
        //    {
        //        new Lion{ CurrentPosition = new Coordinates{ X = 4, Y = 2}},
        //        new Lion{ CurrentPosition = new Coordinates{ X = 1, Y = 1}}
        //    };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

        //    var distancePoints = mover.ReturnListOfDistancePoints(spacesForMove, lions, antelope);

        //    //00 10  20  30 40 50
        //    //01 11  21  31 41 51
        //    //02 12 (22) 32 42 52
        //    //03 13  23  33 43 53
        //    //04 14  24  34 44 54
        //    //05 15  25  35 45 55

        //    // 0.5 - nothing chdnged or got smaller,
        //    // 4 - run from the closest lion,
        //    // 3 - run from lion that stays on close diagonal
        //    // 1.5 - run from lion that has two cells to catch antelope
        //    // 1 - run from any farest lion

        //    var expectedList = new List<double[]>
        //    {
        //        // points for first lion
        //        new double[]{0.5, 1.5},
        //        // points for second lion
        //        new double[]{3, 3},
        //    };

        //    Assert.Equal(0.5, distancePoints[0][0]);
        //    Assert.Equal(1.5, distancePoints[0][1]);
        //    Assert.Equal(3, distancePoints[1][0]);
        //    Assert.Equal(3, distancePoints[1][1]);
        //}

        //[Fact]
        //public void ReturnListOfDistancePoints_NoSpacesForMove_ReturnsEmptyList()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var spacesForMove = new List<Coordinates>();
        //    var lions = new List<Lion>
        //    {
        //        new Lion{ CurrentPosition = new Coordinates{ X = 4, Y = 2}},
        //        new Lion{ CurrentPosition = new Coordinates{ X = 1, Y = 1}}
        //    };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

        //    var distancePoints = mover.ReturnListOfDistancePoints(spacesForMove, lions, antelope);

        //    Assert.Empty(distancePoints);
        //}

        //[Fact]
        //public void ReturnListOfDistancePoints_AnimalsHaveNoPropertiesSet_ThrowException()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var spacesForMove = new List<Coordinates>
        //    {
        //        new Coordinates{ X = 3, Y = 2},
        //        new Coordinates{ X = 1, Y = 3},

        //    };
        //    var lion = new Lion();
        //    var lions = new List<Lion> { lion };
        //    var antelope = new Antelope();

        //    var result = Assert.Throws<AggregateException>(() => mover.ReturnListOfDistancePoints(spacesForMove, lions, antelope));
        //    Assert.Equal($"Invalid data (Current Position for an antelope is not set.) (Current Position for lion with ID-{lion.ID} is not set.)", result.Message);
        //}

        //[Fact]
        //public void GetFarsetSpaceFromLion_ListOfAllAnimalsAndSpaces_FoundFarestPlaceFromLions()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var spacesForMove = new List<Coordinates>
        //    {
        //        new Coordinates{ X = 3, Y = 2},
        //        new Coordinates{ X = 1, Y = 3},

        //    };
        //    var lions = new List<Lion>
        //    {
        //        new Lion{ CurrentPosition = new Coordinates{ X = 4, Y = 2}},
        //        new Lion{ CurrentPosition = new Coordinates{ X = 1, Y = 1}}
        //    };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

        //    var coordinates = mover.GetFarsetSpaceFromLion(spacesForMove, lions, antelope);

        //    Assert.NotNull(coordinates);
        //    Assert.Equal(1, coordinates.X);
        //    Assert.Equal(3, coordinates.Y);
        //}

        //[Fact]
        //public void GetFarsetSpaceFromLion_NoPlacesToMove_ReturnSamePositionForAntelope()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var spacesForMove = new List<Coordinates>();
        //    var lions = new List<Lion>
        //    {
        //        new Lion{ CurrentPosition = new Coordinates{ X = 4, Y = 2}},
        //        new Lion{ CurrentPosition = new Coordinates{ X = 1, Y = 1}}
        //    };
        //    var antelope = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

        //    var coordinates = mover.GetFarsetSpaceFromLion(spacesForMove, lions, antelope);

        //    Assert.NotNull(coordinates);
        //    Assert.Equal(2, coordinates.X);
        //    Assert.Equal(2, coordinates.Y);
        //}

        //[Fact]
        //public void GetFarsetSpaceFromLion_AnimalsPositionAreNotSet_ThrowsException()
        //{
        //    var mover = new AnimalMover(10, 10, new List<Animal>());
        //    var spacesForMove = new List<Coordinates>
        //    {
        //        new Coordinates{ X = 3, Y = 2},
        //        new Coordinates{ X = 1, Y = 3},

        //    };

        //    var lion = new Lion();
        //    var lions = new List<Lion> { lion };
        //    var antelope = new Antelope();

        //    var result = Assert.Throws<AggregateException>(() => mover.GetFarsetSpaceFromLion(spacesForMove, lions, antelope));
        //    Assert.Equal($"Invalid data (Current Position for an antelope is not set.) (Current Position for lion with ID-{lion.ID} is not set.)", result.Message);
        //}

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
            var mover = new AnimalMover(5, 5, new List<Animal>());
            var animal = new Lion { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            mover.SetNextPositionForAnimal(animal);

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
            var mover = new AnimalMover(5, 5, new List<Animal>());
            var animal = new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 2 } };

            mover.SetNextPositionForAnimal(animal);

            Assert.NotNull(animal.NextPosition);
            Assert.InRange(animal.NextPosition.X, 1, 3);
            Assert.InRange(animal.NextPosition.Y, 1, 3);
        }

        [Fact]
        public void SetNextPositionForAnimal_AnimalHasNoTypePropertiesAreNotSet_ThrowException()
        {
            var mover = new AnimalMover(5, 5, new List<Animal>());
            var animal = new Animal();

            var result = Assert.Throws<AggregateException>(() => mover.SetNextPositionForAnimal(animal));
            Assert.Equal($"Invalid data (Current Position for an animal is not set.) (Animal has no type!)", result.Message);
        }
    }
}
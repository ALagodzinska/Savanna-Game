namespace Tests
{
    using FluentAssertions;
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;
    using Savanna.Logic_Layer;

    public class AnimalPairLogicTest
    {
        [Fact]
        public void AddNewbornsToGame_NewbornAnimalsExists_AddsNewbornsToAnimalsList()
        {
            var animalsList = new List<Animal>();
            var mover = new AnimalMover(10, 10, animalsList);
            var pairLogic = new AnimalPairLogic(mover);
            pairLogic.AnimalsToBeBorn.Add(new Animal());

            pairLogic.AddNewbornsToGame();

            pairLogic.AnimalsToBeBorn.Should().BeEmpty();
            mover.Animals.Should().ContainSingle();
        }

        [Fact]
        public void AddNewbornsToGame_NoNewborns_NoAnimalsAdded()
        {
            var animalsList = new List<Animal>();
            var mover = new AnimalMover(10, 10, animalsList);
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AddNewbornsToGame();

            mover.Animals.Should().BeEmpty();
        }

        [Fact]
        public void AddNewPair_NewAnimalPair_AddsPairToList()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);
            var animalPair = new AnimalPair(new Animal(), new Animal());

            pairLogic.AddNewPair(animalPair);

            pairLogic.AnimalPairs.Should().Contain(animalPair);
            pairLogic.AnimalPairs.Should().ContainSingle();
        }

        [Fact]
        public void AnimalsNearbyWithSameType_AnimalNearbySameType_ReturnListOfCloseAnimalSameType()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 5, Y = 4} },
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Antelope { CurrentPosition = new Coordinates { X = 6, Y = 4 } };

            var closestAnimals = pairLogic.AnimalsNearbyWithSameType(animal);

            closestAnimals.Should().ContainSingle();
            // couldnt make it fluent
            Assert.Equal(5, closestAnimals.First().CurrentPosition.X);
            Assert.Equal(4, closestAnimals.First().CurrentPosition.Y);
        }

        [Fact]
        public void AnimalsNearbyWithSameType_AnimalNearbyDifferentType_ReturnEmptyList()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 5, Y = 4} },
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Lion { CurrentPosition = new Coordinates { X = 6, Y = 4 } };

            var closestAnimals = pairLogic.AnimalsNearbyWithSameType(animal);

            closestAnimals.Should().BeEmpty();
        }

        [Fact]
        public void AnimalsNearbyWithSameType_AnimalIsFar_ReturnEmptyList()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 7, Y = 4} },
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Lion { CurrentPosition = new Coordinates { X = 6, Y = 4 } };

            var closestAnimals = pairLogic.AnimalsNearbyWithSameType(animal);

            closestAnimals.Should().BeEmpty();
        }

        [Fact]
        public void AnimalsNearbyWithSameType_AnimalPropertiesAreNotSet_ThrowException()
        {
            var animals = new List<Animal>
            {
                new Animal(),
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Animal();

            Action action = () => pairLogic.AnimalsNearbyWithSameType(animal);
            action.Should().Throw<AggregateException>()
            .WithMessage("Invalid data (Current Position for animal is not set.) (Type for animal is not set.)");
        }

        [Fact]
        public void GetListWithUniqueFreeSpacesAroundParents_ParentsHaveDifferentCoordinates_ListWithAllCoordinates()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            var firstListOfCoordinates = new List<Coordinates>()
            {
                new Coordinates { X = 5, Y = 4 },
            };
            var secondListOfCoordinates = new List<Coordinates>()
            {
                new Coordinates { X = 3, Y = 5 },
            };

            var allPositions = pairLogic.GetListWithUniqueFreeSpacesAroundParents(firstListOfCoordinates, secondListOfCoordinates);

            Assert.Equal(2, allPositions.Count);
            allPositions.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void GetListWithUniqueFreeSpacesAroundParents_ListsWithCoordinatesAreEmpty_EmptyList()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            var firstListOfCoordinates = new List<Coordinates>();
            var secondListOfCoordinates = new List<Coordinates>();

            var allPositions = pairLogic.GetListWithUniqueFreeSpacesAroundParents(firstListOfCoordinates, secondListOfCoordinates);

            allPositions.Should().BeEmpty();
        }

        [Fact]
        public void GetListWithUniqueFreeSpacesAroundParents_ParentsHaveSameCoordinates_ListWithOneCoordinate()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            var firstListOfCoordinates = new List<Coordinates>()
            {
                new Coordinates { X = 5, Y = 4 },
            };
            var secondListOfCoordinates = new List<Coordinates>()
            {
                new Coordinates { X = 5, Y = 4 },
            };

            var allPositions = pairLogic.GetListWithUniqueFreeSpacesAroundParents(firstListOfCoordinates, secondListOfCoordinates);

            allPositions.Should().ContainSingle();
            allPositions.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void GetPlaceToBorn_ParentsHaveSpacesAround_FreeRandomCoordinateCloseToParents()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);
            var animalOne = new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 5 } };
            var animalTwo = new Antelope { CurrentPosition = new Coordinates { X = 4, Y = 5 } };

            var coordinates = pairLogic.GetPlaceToBorn(animalOne, animalTwo);

            coordinates.Should().NotBeNull();
            Assert.InRange(coordinates.X, 3, 6);
            Assert.InRange(coordinates.Y, 4, 6);
        }

        [Fact]
        public void GetPlaceToBorn_NoFreeSpacesAround_ReturnNullCoordinates()
        {
            var animals = new List<Animal>()
            {
                new Animal { CurrentPosition = new Coordinates { X = 0, Y = 0 }, NextPosition = new Coordinates { X = 0, Y = 1 } },
                new Animal { CurrentPosition = new Coordinates { X = 0, Y = 1 }, NextPosition = new Coordinates { X = 0, Y = 0 } },
            };

            var animalOne = new Antelope { CurrentPosition = new Coordinates { X = 1, Y = 1 }, NextPosition = new Coordinates { X = 1, Y = 0 } };
            var animalTwo = new Antelope { CurrentPosition = new Coordinates { X = 1, Y = 0 }, NextPosition = new Coordinates { X = 1, Y = 1 } };
            animals.Add(animalOne);
            animals.Add(animalTwo);

            var mover = new AnimalMover(2, 2, animals);
            var pairLogic = new AnimalPairLogic(mover);

            var coordinates = pairLogic.GetPlaceToBorn(animalOne, animalTwo);

            coordinates.Should().BeNull();
        }

        [Fact]
        public void GetPlaceToBorn_AnimalPropertiesAreNotSet_ThrowException()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);
            var animalOne = new Animal();
            var animalTwo = new Animal();

            Action action = () => pairLogic.GetPlaceToBorn(animalOne, animalTwo);
            action.Should().Throw<AggregateException>()
            .WithMessage("Invalid data (Current Position for animal is not set.) (Type for animal is not set.)");
        }

        [Fact]
        public void AnimalToBeBorn_AnimalPairAreLions_AddsToNewbornNewLion()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);
            var lionPair = new AnimalPair(new Lion { CurrentPosition = new Coordinates { X = 5, Y = 5 } },
                new Lion { CurrentPosition = new Coordinates { X = 5, Y = 6 } });

            pairLogic.AnimalsToBeBorn.Should().BeEmpty();
            pairLogic.AnimalToBeBorn(lionPair);

            pairLogic.AnimalsToBeBorn.Should().ContainSingle();
            pairLogic.AnimalsToBeBorn.First().Should().BeOfType<Lion>();
        }

        [Fact]
        public void AnimalToBeBorn_AnimalPairAreAntelopes_AddsToNewbornNewAntelope()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);
            var antelopePair = new AnimalPair(new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 5 } },
                new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 6 } });

            pairLogic.AnimalsToBeBorn.Should().BeEmpty();
            pairLogic.AnimalToBeBorn(antelopePair);

            pairLogic.AnimalsToBeBorn.Should().ContainSingle();
            pairLogic.AnimalsToBeBorn.First().Should().BeOfType<Antelope>();
        }

        [Fact]
        public void AnimalToBeBorn_NoPlaceToPlaceNewAnimal_DontAddNewAnimal()
        {
            var animals = new List<Animal>()
            {
                new Animal { CurrentPosition = new Coordinates { X = 0, Y = 0 }, NextPosition = new Coordinates { X = 0, Y = 1 } },
                new Animal { CurrentPosition = new Coordinates { X = 0, Y = 1 }, NextPosition = new Coordinates { X = 0, Y = 0 } },
            };

            var animalOne = new Antelope { CurrentPosition = new Coordinates { X = 1, Y = 1 }, NextPosition = new Coordinates { X = 1, Y = 0 } };
            var animalTwo = new Antelope { CurrentPosition = new Coordinates { X = 1, Y = 0 }, NextPosition = new Coordinates { X = 1, Y = 1 } };
            animals.Add(animalOne);
            animals.Add(animalTwo);

            var mover = new AnimalMover(2, 2, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var antelopePair = new AnimalPair(animalOne, animalTwo);

            pairLogic.AnimalsToBeBorn.Should().BeEmpty();
            pairLogic.AnimalToBeBorn(antelopePair);
            pairLogic.AnimalsToBeBorn.Should().BeEmpty();
        }

        [Fact]
        public void AnimalToBeBorn_AnimalPairAnimalsNotSet_ThrowsAnException()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);
            var lionPair = new AnimalPair(new Animal(), new Animal());

            Action action = () => pairLogic.AnimalToBeBorn(lionPair);
            action.Should().Throw<AggregateException>()
            .WithMessage("Invalid data (Current Position for animal is not set.) (Type for animal is not set.)");
        }

        [Fact]
        public void CheckIfAnimalHavePair_AnimalHasPairForTwoRounds_NewAnimalPairCreated()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 2, Y = 1},
                    NextPosition = new Coordinates {X = 1, Y = 1} },
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Antelope
            {
                CurrentPosition = new Coordinates { X = 3, Y = 1 },
                NextPosition = new Coordinates { X = 2, Y = 1 }
            };

            pairLogic.AnimalPairs.Should().BeEmpty();
            pairLogic.CheckIfAnimalHavePair(animal);

            pairLogic.AnimalPairs.Should().ContainSingle();
        }

        [Fact]
        public void CheckIfAnimalHavePair_AnimalHasPairForOneRound_NoPairCreated()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 2, Y = 1},
                    NextPosition = new Coordinates {X = 1, Y = 1} },
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Antelope
            {
                CurrentPosition = new Coordinates { X = 3, Y = 1 },
                NextPosition = new Coordinates { X = 4, Y = 1 }
            };

            pairLogic.CheckIfAnimalHavePair(animal);

            pairLogic.AnimalPairs.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfAnimalHavePair_NoAnimalAroundToCreatePair_NoPairCreated()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 6, Y = 7 },
                    NextPosition = new Coordinates {X = 7, Y = 7} },
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Antelope
            {
                CurrentPosition = new Coordinates { X = 3, Y = 1 },
                NextPosition = new Coordinates { X = 4, Y = 1 }
            };

            pairLogic.CheckIfAnimalHavePair(animal);

            pairLogic.AnimalPairs.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfAnimalHavePair_AnimalIsMissingProperties_ThrowsException()
        {
            var animals = new List<Animal> { new Animal() };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Animal();

            Action action = () => pairLogic.CheckIfAnimalHavePair(animal);
            action.Should().Throw<AggregateException>()
            .WithMessage("Invalid data (Current Position for animal is not set.) (Type for animal is not set.)");
        }

        [Fact]
        public void CheckIfAnimalHavePair_AnimalIsCloseAndMissingNextPosition_ThrowsException()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 2, Y = 1 } }
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);
            var animal = new Antelope
            {
                CurrentPosition = new Coordinates { X = 3, Y = 1 }
            };

            Action action = () => pairLogic.CheckIfAnimalHavePair(animal);
            action.Should().Throw<Exception>()
            .WithMessage("Animals next position is not set.");
        }

        [Fact]
        public void ActionForPairsOnMove_PairsTogetherForRound_PairsStayInPairList()
        {
            var animalPair = new AnimalPair(
                  new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 1 } },
                  new Antelope { CurrentPosition = new Coordinates { X = 3, Y = 1 } });
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AnimalPairs.Add(animalPair);

            pairLogic.AnimalPairs.Should().HaveCount(1);
            Assert.Equal(1, pairLogic.AnimalPairs.First().RoundsTogether);

            pairLogic.ActionForPairsOnMove();
            pairLogic.AnimalPairs.Should().HaveCount(1);
            pairLogic.AnimalPairs.First().DoesBrokeUp.Should().BeFalse();
            Assert.Equal(2, pairLogic.AnimalPairs.First().RoundsTogether);
        }

        [Fact]
        public void ActionForPairsOnMove_PairsTogetherThirdRound_PairHaveBabyAndBreakUp()
        {
            var animalPair = new AnimalPair(
                  new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 1 } },
                  new Antelope { CurrentPosition = new Coordinates { X = 3, Y = 1 } });
            animalPair.RoundsTogether = 2;

            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AnimalPairs.Add(animalPair);

            pairLogic.AnimalPairs.Should().HaveCount(1);
            Assert.Equal(2, pairLogic.AnimalPairs.First().RoundsTogether);

            pairLogic.ActionForPairsOnMove();

            Assert.Equal(3, pairLogic.AnimalPairs.First().RoundsTogether);
            pairLogic.AnimalPairs.First().DoesBrokeUp.Should().BeTrue();
            pairLogic.AnimalsToBeBorn.Should().HaveCount(1);
            pairLogic.AnimalsToBeBorn.First().Should().BeOfType<Antelope>();
        }

        [Fact]
        public void ActionForPairsOnMove_PairsIsNotNear_PairBreakUp()
        {
            var animalPair = new AnimalPair(
                  new Antelope { CurrentPosition = new Coordinates { X = 2, Y = 1 } },
                  new Antelope { CurrentPosition = new Coordinates { X = 5, Y = 1 } });

            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AnimalPairs.Add(animalPair);

            pairLogic.AnimalPairs.Should().HaveCount(1);
            Assert.Equal(1, pairLogic.AnimalPairs.First().RoundsTogether);

            pairLogic.ActionForPairsOnMove();

            Assert.Equal(1, pairLogic.AnimalPairs.First().RoundsTogether);
            pairLogic.AnimalPairs.First().DoesBrokeUp.Should().BeTrue();
        }

        [Fact]
        public void ActionForPairsOnMove_PairListIsEmpty_NothingChanges()
        {
            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AnimalPairs.Should().BeEmpty();

            pairLogic.ActionForPairsOnMove();

            pairLogic.AnimalPairs.Should().BeEmpty();
        }

        [Fact]
        public void AnimalPairsCreated_TwoAnimalsNearForTwoConsecutiveRound_PairIsCreated()
        {
            var animals = new List<Animal>
            {
                new Antelope{ CurrentPosition = new Coordinates { X = 6, Y = 7 },
                    NextPosition = new Coordinates {X = 7, Y = 7} },
                new Antelope{ CurrentPosition = new Coordinates { X = 6, Y = 6 },
                    NextPosition = new Coordinates {X = 7, Y = 6} }
            };
            var mover = new AnimalMover(10, 10, animals);
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AnimalPairs.Should().BeEmpty();

            pairLogic.AnimalPairsCreated();

            pairLogic.AnimalPairs.Should().HaveCount(1);
        }

        [Fact]
        public void AnimalPairsCreated_ExistingPairsToBrokeUp_PairsRemovedFromList()
        {
            var animalPair = new AnimalPair(
                new Antelope
                {

                    CurrentPosition = new Coordinates { X = 6, Y = 7 },
                    NextPosition = new Coordinates { X = 7, Y = 7 }
                },
                new Antelope
                {
                    CurrentPosition = new Coordinates { X = 6, Y = 6 },
                    NextPosition = new Coordinates { X = 7, Y = 6 }
                });
            animalPair.DoesBrokeUp = true;

            var mover = new AnimalMover(10, 10, new List<Animal>());
            var pairLogic = new AnimalPairLogic(mover);

            pairLogic.AnimalPairs.Add(animalPair);
            pairLogic.AnimalPairs.Should().HaveCount(1);

            pairLogic.AnimalPairsCreated();

            pairLogic.AnimalPairs.Should().BeEmpty();
        }        
    }
}
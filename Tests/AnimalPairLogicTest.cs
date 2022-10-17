namespace Tests
{
    using AnimalBehaviorInterfaces;
    using FluentAssertions;
    using Moq;
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
    }
}
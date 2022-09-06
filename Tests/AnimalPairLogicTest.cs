using FluentAssertions;
using Savanna.Entities.Animals;
using Savanna.Entities.GameField;
using Savanna.Logic_Layer;

namespace Tests
{
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
            var animal = new Antelope { ID =1, CurrentPosition = new Coordinates { X = 6, Y = 4 } };

            var closestAnimals = pairLogic.AnimalsNearbyWithSameType(animal);

            closestAnimals.Should().ContainSingle();
            // couldnt make it fluent
            Assert.Equal(1, closestAnimals.First().ID);
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
            .WithMessage("Invalid data (Current Position for animal is not set.) (Type for animal is not set.)") ;
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
        //No places to place animal.
        //Animals has no types or setted values.
    }
}
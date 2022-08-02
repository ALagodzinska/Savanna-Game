using Savanna.Entities;

namespace Savanna
{
    public class GameLogic
    {
        UserOutput userOutput = new();
        

        const int fieldHeight = 25;
        const int fieldWidth = 60;
        const int topStartPoint = 5;
        const ConsoleColor borderColor = ConsoleColor.DarkGreen;

        List<Animal> animals = new List<Animal>();
        public void ExitGame()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public GameField CreateGameField() => new GameField(fieldWidth, fieldHeight, topStartPoint, borderColor);

        public void PlayGame()
        {
            userOutput.DisplayGameRules();
            var gameField = CreateGameField();
            gameField.DrawBorder();
            //DrawXOnField(25, 60, 5);
            ActionToMake(gameField);
        }

        public void DrawXOnField(int borderHeight, int borderWidth, int offsetFromTop)
        {
            for (int y = offsetFromTop + 1; y < borderHeight + offsetFromTop + 1; y++)
            {
                for (int x = 1; x < borderWidth + 1; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                }
            }
        }

        public void ActionToMake(GameField gameField)
        {
            ConsoleKeyInfo pressedKey;

            do
            {
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.A:
                        
                        var createdAntelope = new Antelope();
                        animals.Add(createdAntelope);
                        while (createdAntelope.CurrentPosition == null)
                        {
                            AddAnimalToGameField(createdAntelope, gameField);
                        }
                        DrawAnimal(createdAntelope);
                        break;

                    case ConsoleKey.L:
                        var createdLion = new Lion();
                        animals.Add(createdLion);
                        while (createdLion.CurrentPosition == null)
                        {
                            AddAnimalToGameField(createdLion, gameField);
                        }
                        DrawAnimal(createdLion);
                        break;

                    default:
                        Console.SetCursorPosition(0, 3);
                        Console.WriteLine("Wrong input");
                        break;
                }
            } while (pressedKey.Key != ConsoleKey.Escape);
            
        }

        public void DrawAnimal(Animal animal)
        {
            Console.SetCursorPosition(animal.CurrentPosition[0] + 1, animal.CurrentPosition[1] + topStartPoint + 1);
            var symbol = animal.Type == "Lion" ? Convert.ToChar(2) : Convert.ToChar(1);
            Console.Write(symbol);
        }

        public void AddAnimalToGameField(Animal animal, GameField gameField)
        {
            Random random = new Random();
            var randomWidthPosition = random.Next(0, gameField.Width - 1);
            var randomHeightPosition = random.Next(0, gameField.Height - 1);
            int[] coordinates = new int[2] { randomWidthPosition, randomHeightPosition };
            if(CheckIfPlaceIsTaken(coordinates) == false)
            {
                animal.CurrentPosition = coordinates;               
            }
        }

        public bool CheckIfPlaceIsTaken(int[] coordinates)
        {
            var foundAnimal = GetAnimalByCoordinates(coordinates);
            return foundAnimal != null ? true : false;
        }

        public Animal? GetAnimalByCoordinates(int[] coordinates)
        {
            return animals.FirstOrDefault(a => a.CurrentPosition == coordinates);
        }



        public void AnimalsAround(Animal animal, GameField gameField)
        {
            int[] coordinates = new int[2];
            List<Animal> closestAnimalList = new List<Animal>();
            List<int[]> emtyPath = new List<int[]>();

            for (int h = animal.CurrentPosition[1] - animal.VisionRange; h <= animal.CurrentPosition[1] + animal.VisionRange; h++)
            {
                for (int w = animal.CurrentPosition[0] - animal.VisionRange; w <= animal.CurrentPosition[0] + animal.VisionRange; w++)
                {
                    if (h >= gameField.Height || h < 0
                        || w >= gameField.Width || w < 0
                        || h == animal.CurrentPosition[1] && w == animal.CurrentPosition[0])
                    {
                        continue;
                    }
                    coordinates[0] = w;
                    coordinates[1] = h;

                    var foundAnimal = GetAnimalByCoordinates(coordinates);
                    if(foundAnimal == null)
                    {
                        emtyPath.Add(coordinates);
                        continue;
                    }

                    closestAnimalList.Add(foundAnimal);
                }
            }
            if(animal.Type == "Lion")
            {
                var antelopesAround = closestAnimalList.FindAll(a => a.Type == "Antelope");
                if(antelopesAround != null)
                {
                    var random = new Random();
                    int index = random.Next(antelopesAround.Count);
                    animal.CurrentPosition = antelopesAround[index].CurrentPosition;
                }
                else
                {

                }
            }
        }

        public void FindClosestAntilope(Lion lion, List<Animal> animalsAround )
        {
            var antelopesAround = animalsAround.FindAll(a => a.Type == "Antelope");
            //List<Animal> closestAnimalList = new List<Animal>();
            //if (antelopesAround != null)
            //{
            //    foreach(var antelope in antelopesAround)
            //    {
            //        if ((antelope.CurrentPosition[0]>= lion.CurrentPosition[0] - 1 || antelope.CurrentPosition[0] >= lion.CurrentPosition[0] + 1) &&
            //            (antelope.CurrentPosition[1] >= lion.CurrentPosition[1] - 1 || antelope.CurrentPosition[1] >= lion.CurrentPosition[1] + 1))
            //        {
            //            closestAnimalList.Add(antelope);
            //        }
            //    }
            //}
        }
    }
}

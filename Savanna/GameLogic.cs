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
                        var antelope = Animal.CreateAnAnimal(ConsoleKey.A);
                        animals.Add(antelope);
                        while (antelope.CurrentPosition == null)
                        {
                            AddAnimalToGameField(antelope, gameField);
                        }
                        DrawAnimal(antelope);
                        break;

                    case ConsoleKey.L:
                        var lion = Animal.CreateAnAnimal(ConsoleKey.L);
                        animals.Add(lion);
                        while (lion.CurrentPosition == null)
                        {
                            AddAnimalToGameField(lion, gameField);
                        }
                        DrawAnimal(lion);
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
            //+6 because of off top
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
            var foundAnimal = animals.FirstOrDefault(a => a.CurrentPosition == coordinates);
            return foundAnimal != null ? true : false;
        }
    }
}

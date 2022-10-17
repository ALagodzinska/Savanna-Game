namespace Savanna.Logic_Layer
{
    using AnimalBehaviorInterfaces;
    using Savanna.Entities.Animals;
    using Savanna.Entities.GameField;
    using System.Reflection;

    /// <summary>
    /// Stores main game logic.
    /// </summary>
    public class GameFieldLogic
    {
        /// <summary>
        /// List of animals
        /// </summary>
        public List<Animal> Animals;

        /// <summary>
        /// Game field.
        /// </summary>
        GameField GameField;

        /// <summary>
        /// Animal moving logic plugin.
        /// </summary>
        IAnimalMover _animalMoverPlugin = null;

        /// <summary>
        /// Animal pair logic plugin.
        /// </summary>
        IAnimalPairLogic _animalPairLogicPlugin = null;

        /// <summary>
        /// Assign values to this class fields.
        /// </summary>
        /// <param name="gameField">Created game field.</param>
        public GameFieldLogic(GameField gameField)
        {

            Animals = new List<Animal>();
            GameField = gameField;
            ReadExtensions();           
        }

        /// <summary>
        /// Draws a border line for game field.
        /// </summary>
        public void DrawBorder()
        {
            string startCorner = "╔";
            string cornerBottomLeft = "╚";
            string cornerTopRight = "╗";
            string cornerBottomRight = "╝";
            string verticalLine = "║";
            string horizontalLine = "═";

            //draw top of the border
            for (int w = 0; w < GameField.Width; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerTopRight + "\n";

            //draw vertical lines
            for (int h = 0; h < GameField.Height; h++)
            {
                startCorner += verticalLine + new string(' ', GameField.Width) + verticalLine + "\n";
            }
            startCorner += cornerBottomLeft;

            //draw bottom of the border
            for (int w = 0; w < GameField.Width; w++)
            {
                startCorner += horizontalLine;
            }
            startCorner += cornerBottomRight;

            Console.SetCursorPosition(0, GameField.TopPosition);
            Console.ForegroundColor = GameField.BorderColor;
            Console.Write(startCorner);
            Console.ResetColor();
        }

        /// <summary>
        /// Sets animal position and display it on a game field.
        /// </summary>
        /// <param name="animal">Animal.</param>
        public void SetAnimalPosition(Animal animal)
        {
            _animalMoverPlugin.SetNewAnimalCurrentPosition(animal);
            DrawAnimal(animal);
        }

        /// <summary>
        /// Draws game with border and animals inside game field.
        /// </summary>
        public void DrawGame()
        {
            DrawBorder();
            if (Animals.Count > 0)
            {
                foreach (var animal in Animals)
                {
                    DrawAnimal(animal);
                }
            }
        }

        /// <summary>
        /// Display animal on a game screen.
        /// </summary>
        /// <param name="animal">Animal from game.</param>
        public void DrawAnimal(Animal animal)
        {
            if (animal.IsAlive == true)
            {
                Console.SetCursorPosition(animal.CurrentPosition.X + 1, animal.CurrentPosition.Y + GameField.TopPosition + 1);
                var isLion = animal.GetType() == typeof(Lion);

                var symbol = isLion ? Convert.ToChar(2) : Convert.ToChar(1);
                Console.BackgroundColor = ConsoleColor.Black;

                if (isLion)
                {
                    var lion = (Lion)animal;

                    Console.ForegroundColor = lion.AnimalColor;
                    lion.DoesAte = false;
                }
                else
                {
                    var antelope = (Antelope)animal;

                    Console.ForegroundColor = antelope.AnimalColor;
                }

                Console.Write(symbol);
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Check if there is enough free space to add new animal.
        /// </summary>
        /// <returns></returns>
        public bool DoesGameFieldHaveFreeSpaces()
        {
            return Animals.Count <= (GameField.Height * GameField.Width) / 2;
        }

        /// <summary>
        /// Does all animals actions on each iteration.
        /// </summary>
        public void AnimalsActionsOnMove()
        {
            //set next position to each animal
            foreach (var animal in Animals)
            {
                _animalMoverPlugin.SetNextPositionForAnimal(animal);
            }

            //apply pair logic, create pairs
            _animalPairLogicPlugin.AnimalPairsCreated();

            //make move for each animal
            foreach (var animal in Animals)
            {
                _animalMoverPlugin.MakeMove(animal);
            }

            //add newborns in this round to a game
            _animalPairLogicPlugin.AddNewbornsToGame();

            //delete all dead animals
            Animals.RemoveAll(a => a.IsAlive == false);

            DrawGame();
        }

        /// <summary>
        /// Read extensions and assign values to plugins.
        /// </summary>
        public void ReadExtensions()
        {
            // 1- Read the dll files from the extensions folder.
            var files = Directory.GetFiles("extensions", "*.dll");

            // 2- Read the assembly from files.
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

                // 3- Extract all the types that implements IPlugin
                var pluginAnimalMover = assembly.GetTypes()
                            .Where(type => typeof(IAnimalMover).IsAssignableFrom(type)
                            && !type.IsInterface).First();

                var pluginAnimalPairLogic = assembly.GetTypes()
                            .Where(type => typeof(IAnimalPairLogic).IsAssignableFrom(type)
                            && !type.IsInterface).First();

                // 4- Create an instance from the extracted type.
                var pluginAnimalMoverInstance = Activator.CreateInstance(pluginAnimalMover, GameField.Height, GameField.Width, Animals) as IAnimalMover;
                var pluginAnimalPairLogicInstance = Activator.CreateInstance(pluginAnimalPairLogic, pluginAnimalMoverInstance) as IAnimalPairLogic;

                // 5- Assign values to fields.
                _animalMoverPlugin = pluginAnimalMoverInstance;
                _animalPairLogicPlugin = pluginAnimalPairLogicInstance;
            }
        }
    }
}

namespace Savanna.Entities
{
    public class Animal
    {
        public string Type {get;set;}
        public int[]? CurrentPosition { get; set; }
        public bool? IsAlive { get; set; }
        public int? VisionRange { get; set; }

        public static Animal CreateAnAnimal(ConsoleKey keyPressed)
        {
            if (keyPressed == ConsoleKey.A)
            {
                return new Animal()
                {
                    Type = "Antelope",
                    IsAlive = true,
                    VisionRange = 3
                };
            }

            else
            {
                return new Animal()
                {
                    Type = "Lion",
                    IsAlive = true,
                    VisionRange = 3
                };
            }
        }
    }    
}

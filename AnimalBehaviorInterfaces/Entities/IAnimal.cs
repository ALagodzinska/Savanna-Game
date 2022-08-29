using Interfaces.GameField;

namespace AnimalBehaviorInterfaces.Entities
{
    public interface IAnimal
    {
        int ID { get; set; }

        ICoordinates CurrentPosition { get; set; }

        ICoordinates NextPosition { get; set; }

        double Health { get; set; }

        bool? IsAlive { get; set; }

        int VisionRange { get; set; }

        ConsoleColor AnimalColor { get; set; }

        static int globalAnimalId { get; set; }
    }
}

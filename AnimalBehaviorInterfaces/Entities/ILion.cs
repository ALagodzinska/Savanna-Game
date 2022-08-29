namespace AnimalBehaviorInterfaces.Entities
{
    public interface ILion: IAnimal
    {
        bool DoesAte { get; set; }

        new ConsoleColor AnimalColor { get => SetLionColor(); }

        ConsoleColor SetLionColor();
    }
}

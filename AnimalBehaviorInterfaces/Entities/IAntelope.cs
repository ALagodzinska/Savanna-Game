namespace AnimalBehaviorInterfaces.Entities
{
    public interface IAntelope: IAnimal
    {
        new ConsoleColor AnimalColor { get => SetAntelopeColor(); }

        ConsoleColor SetAntelopeColor();
    }
}

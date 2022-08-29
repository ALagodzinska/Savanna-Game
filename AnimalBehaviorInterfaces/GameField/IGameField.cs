namespace Interfaces.GameField
{
    public interface IGameField
    {
        int Height { get; set; }

        int Width { get; set; }

        int TopPosition { get; set; }

        ConsoleColor BorderColor { get; set; }
    }
}

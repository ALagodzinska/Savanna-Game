namespace Savanna.Entities
{
    public class Animal
    {
        public string Type { get; set; }
        public int[]? CurrentPosition { get; set; }
        public bool? IsAlive { get; set; }
        public int? VisionRange { get; set; }
    }
}

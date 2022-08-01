namespace Savanna.Entities
{
    public class Animal
    {
        public enum AnimalType {Lion, Antelope}
        public int[,]? CurrentPosition { get; set; }
        public bool? IsAlive { get; set; }
        public int? VisionRange { get; set; }        
    }
}

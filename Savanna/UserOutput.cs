namespace Savanna
{
    public class UserOutput
    {
        public void DisplayGameRules()
        {
            Console.Clear();
            Console.WriteLine(@$"THIS IS SAVANNA
PRESS 'L' TO ADD A LION, PRESS 'A' TO ADD AN ANTELOPE TO GAME FIELD
{Convert.ToChar(1)} - ANTELOPE | {Convert.ToChar(2)} - LION");
        }
    }
}

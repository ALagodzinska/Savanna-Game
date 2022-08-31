namespace Savanna.Entities.Menu
{
    /// <summary>
    /// Stores option data for a menu.
    /// </summary>
    public class MenuOption
    {
        /// <summary>
        /// Option from enum T.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Title of option to display to a user.
        /// </summary>
        public string? Title { get; set; }
    }    
}

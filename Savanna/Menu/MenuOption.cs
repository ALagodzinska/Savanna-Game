namespace Savanna.Entities.Menu
{
    /// <summary>
    /// Stores option data for a menu.
    /// </summary>
    /// <typeparam name="T">Enum of options.</typeparam>
    public class MenuOption<T>
    {
        /// <summary>
        /// Option from enum T.
        /// </summary>
        public T? Index { get; set; }

        /// <summary>
        /// Title of option to display to a user.
        /// </summary>
        public string? Title { get; set; }
    }    
}

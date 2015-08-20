using System.Threading.Tasks;

namespace MyAnimeList
{
    /// <summary>
    /// Provides an interact
    /// </summary>
    public interface IMyAnimeList
    {
        /// <summary>
        /// Confirms if the login details are correct
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True is a success code is returned</returns>
        Task<bool> ConfirmLoginDetails(string username, string password);
    }

    /// <summary>
    /// Supported status for manga
    /// </summary>
    public enum MangaStatus
    {
        Unknown = 0,
        Reading = 1,
        Completed = 2,
        OnHold = 3,
        Dropped = 4,
        PlanToRead = 6
    }
}

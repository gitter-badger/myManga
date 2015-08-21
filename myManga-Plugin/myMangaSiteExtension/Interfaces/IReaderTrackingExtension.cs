using myMangaSiteExtension.Attributes;
using myMangaSiteExtension.Objects;
using System;
using System.Collections.Generic;

namespace myMangaSiteExtension.Interfaces
{
    public interface IMangaTrackingExtension
    {
        IMangaTrackingExtensionDescriptionAttribute ReaderTrackingExtensionDescriptionAttribute { get; }
        
        /// <summary>
        /// Used to login user
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>Boolean of login status</returns>
        Boolean LoginUser(params Object[] arguments);

        /// <summary>
        /// Log user out
        /// </summary>
        /// <returns>Boolean of logout status</returns>
        Boolean LogoutUser();

        /// <summary>
        /// Get list of manga user is reading
        /// </summary>
        /// <returns>List of MangaObjects</returns>
        List<MangaObject> GetUserManga();

        /// <summary>
        /// Update user's manga list
        /// </summary>
        /// <param name="mangaItems">MangaObject to update/add</param>
        /// <returns>Boolean of update/add status</returns>
        Boolean UpdateUserManga(MangaObject mangaItems);

        /// <summary>
        /// Remove user's manga list
        /// </summary>
        /// <param name="mangaItems">MangaObject to remove</param>
        /// <returns>Boolean of remove status</returns>
        Boolean RemoveUserManga(MangaObject mangaItems);
    }
}

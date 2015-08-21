using myMangaSiteExtension.Attributes;
using myMangaSiteExtension.Objects;
using System;
using System.Collections.Generic;

namespace myMangaSiteExtension.Interfaces
{
    public interface IReaderTrackingExtension
    {
        IReaderTrackingExtensionDescriptionAttribute ReaderTrackingExtensionDescriptionAttribute { get; }

        /// <summary>
        /// Used to login user with a username/password
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        /// <returns>Boolean of login status</returns>
        Boolean LoginUser(String username, String password);
        /// <summary>
        /// Used to login user with a token
        /// </summary>
        /// <param name="token">User's login token</param>
        /// <returns>Boolean of login status</returns>
        Boolean LoginUser(String token);
        /// <summary>
        /// Used to login user with unknown argument types
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>Boolean of login status</returns>
        Boolean LoginUser(params Object[] arguments);
        Boolean LogoutUser();

        List<MangaObject> GetUserManga();
        Boolean UpdateUserManga(List<MangaObject> mangaItems);
    }
}

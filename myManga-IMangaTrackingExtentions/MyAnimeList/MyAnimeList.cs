using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using Core.Other;
using HtmlAgilityPack;
using myMangaSiteExtension.Attributes;
using myMangaSiteExtension.Enums;
using myMangaSiteExtension.Interfaces;
using myMangaSiteExtension.Objects;

namespace MyAnimeList
{
    [IMangaTrackingExtensionDescription(
        "MyAnimeList",
        "Troy Rijkaard",
        "0.0.1")]
    public class MyAnimeList : IMangaTrackingExtension
    {
        private IMangaTrackingExtensionDescriptionAttribute mangaTrackingExtensionDescriptionAttribute;
        public IMangaTrackingExtensionDescriptionAttribute MangaTrackingExtensionDescriptionAttribute
        public IMangaTrackingExtensionDescriptionAttribute ReaderTrackingExtensionDescriptionAttribute
        { get { return mangaTrackingExtensionDescriptionAttribute ?? (mangaTrackingExtensionDescriptionAttribute = GetType().GetCustomAttribute<IMangaTrackingExtensionDescriptionAttribute>(false)); } }

        /// <summary>
        /// MyAnimeList uses username/password
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public Boolean LoginUser(params Object[] arguments)
        {
            String username = arguments[0] as String,
                   password = arguments[1] as String;
            return false;
        }

        /// <summary>
        /// Not needed for MyAnimeList
        /// </summary>
        /// <returns></returns>
        public Boolean LogoutUser()
        { return true; }

        public List<MangaObject> GetUserManga()
        {
            return null;
        }

        public Boolean UpdateUserManga(MangaObject mangaItems)
        {
            return false;
        }

        public Boolean RemoveUserManga(MangaObject mangaItems)
        {
            return false;
        }
    }
}

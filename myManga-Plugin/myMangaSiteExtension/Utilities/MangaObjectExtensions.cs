﻿using myMangaSiteExtension.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myMangaSiteExtension.Utilities
{
    public static class MangaObjectExtensions
    {
        public static void Merge(this MangaObject mangaObject, params MangaObject[] list) { mangaObject.Merge(list.AsEnumerable()); }
        public static void Merge(this MangaObject mangaObject, IEnumerable<MangaObject> list)
        {
            if (list.Count() > 0)
            {
                // Name
                if (String.IsNullOrWhiteSpace(mangaObject.Name))
                {
                    MangaObject FoD_Obj = list.FirstOrDefault(mO => !String.IsNullOrWhiteSpace(mO.Name));
                    if (FoD_Obj != null)
                        mangaObject.Name = FoD_Obj.Name;
                }

                // Released
                if (mangaObject.Released.Equals(DateTime.MinValue))
                {
                    MangaObject FoD_Obj = list.FirstOrDefault(mO => mO.Released > DateTime.MinValue);
                    if (FoD_Obj != null)
                        mangaObject.Released = FoD_Obj.Released;
                }

                // Rating
                if (mangaObject.Rating < 0)
                {
                    MangaObject FoD_Obj = list.FirstOrDefault(mO => mO.Rating >= 0);
                    if (FoD_Obj != null)
                        mangaObject.Rating = FoD_Obj.Rating;
                }

                // Description
                if (String.IsNullOrWhiteSpace(mangaObject.Description))
                {
                    MangaObject FoD_Obj = list.FirstOrDefault(mO => !String.IsNullOrWhiteSpace(mO.Description));
                    if (FoD_Obj != null)
                        mangaObject.Description = FoD_Obj.Description;
                }

                // MangaType & PageFlowDirection
                if (mangaObject.MangaType == Enums.MangaObjectType.Unknown)
                {
                    MangaObject FoD_Obj = list.FirstOrDefault(mO => mO.MangaType != Enums.MangaObjectType.Unknown);
                    if (FoD_Obj != null)
                    {
                        mangaObject.MangaType = FoD_Obj.MangaType;
                        mangaObject.PageFlowDirection = FoD_Obj.PageFlowDirection;
                    }
                }

                // Locations
                foreach (List<LocationObject> Locations in (from MangaObject obj in list where obj != null select obj.Locations))
                    foreach (LocationObject Location in Locations)
                        if (!mangaObject.Locations.Any(o => o.ExtensionName == Location.ExtensionName))
                            mangaObject.Locations.Add(Location);
                        else
                        {
                            // Update existing location url
                            Int32 idx = mangaObject.Locations.FindIndex(o => o.ExtensionName == Location.ExtensionName);
                            mangaObject.Locations.RemoveAt(idx);
                            mangaObject.Locations.Insert(idx, Location);
                        }

                // DatabaseLocations
                foreach (List<LocationObject> DatabaseLocations in (from MangaObject obj in list where obj != null select obj.DatabaseLocations))
                    foreach (LocationObject DatabaseLocation in DatabaseLocations)
                        if (!mangaObject.DatabaseLocations.Any(o => o.ExtensionName == DatabaseLocation.ExtensionName))
                            mangaObject.DatabaseLocations.Add(DatabaseLocation);

                // Artists
                foreach (List<String> Artists in (from MangaObject obj in list where obj != null select obj.Artists))
                    foreach (String Artist in Artists)
                        if (Artist != null && !mangaObject.Artists.Any(o => o.ToLower() == Artist.ToLower()))
                            mangaObject.Artists.Add(Artist);

                // Authors
                foreach (List<String> Authors in (from MangaObject obj in list where obj != null select obj.Authors))
                    foreach (String Author in Authors)
                        if (Author != null && !mangaObject.Authors.Any(o => o.ToLower() == Author.ToLower()))
                            mangaObject.Authors.Add(Author);

                // Covers
                foreach (List<String> Covers in (from MangaObject obj in list where obj != null select obj.Covers))
                    foreach (String Cover in Covers)
                        if (Cover != null && !mangaObject.Covers.Any(o => o == Cover))
                            mangaObject.Covers.Add(Cover);
                mangaObject.Covers.RemoveAll(c => String.IsNullOrWhiteSpace(c));

                // AlternateNames
                foreach (String Name in (from MangaObject obj in list where obj != null select obj.Name))
                    if (!mangaObject.AlternateNames.Any(o => o.ToLower() == Name.ToLower()) && Name != null)
                        mangaObject.AlternateNames.Add(Name);
                foreach (List<String> AlternateNames in (from MangaObject obj in list where obj != null select obj.AlternateNames))
                    foreach (String AlternateName in AlternateNames)
                        if (AlternateName != null && !mangaObject.AlternateNames.Any(o => o.ToLower() == AlternateName.ToLower()))
                            mangaObject.AlternateNames.Add(AlternateName);

                // Genres
                foreach (List<String> Genres in (from MangaObject obj in list where obj != null select obj.Genres))
                    foreach (String Genre in Genres)
                        if (Genre != null && !mangaObject.Genres.Any(o => o.ToLower() == Genre.ToLower()))
                            mangaObject.Genres.Add(Genre);

                // Chapters
                foreach (List<ChapterObject> Chapters in (from MangaObject obj in list where obj != null select obj.Chapters))
                    foreach (ChapterObject Chapter in Chapters)
                        if (Chapter != null)
                        {
                            ChapterObject chapterObject = mangaObject.Chapters.FirstOrDefault(o =>
                            {
                                if (!Int32.Equals(o.Chapter, Chapter.Chapter)) return false;
                                if (!Int32.Equals(o.SubChapter, Chapter.SubChapter)) return false;
                                return true;
                            });
                            if (ChapterObject.Equals(chapterObject, null))
                                mangaObject.Chapters.Add(Chapter);
                            else
                                chapterObject.Merge(Chapter);
                        }

                mangaObject.SortChapters();
            }
        }

        public static void AttachDatabase(this MangaObject value, DatabaseObject databaseObject, Boolean databaseAsMaster = false)
        {
            value.DatabaseLocations = databaseObject.Locations;
            if (databaseAsMaster)
            {
                if (value.Name != null && !value.AlternateNames.Any(o => o.ToLower() == value.Name.ToLower()))
                    value.AlternateNames.Add(value.Name);
                value.Name = databaseObject.Name;
            }
            else if (databaseObject.Name != null && !value.AlternateNames.Any(o => o.ToLower() == databaseObject.Name.ToLower()))
                value.AlternateNames.Add(databaseObject.Name);
            // AlternateNames
            foreach (String AlternateName in databaseObject.AlternateNames)
                if (AlternateName != null && !value.AlternateNames.Any(o => o.ToLower() == AlternateName.ToLower()))
                    value.AlternateNames.Add(AlternateName);

            if (databaseAsMaster || String.Equals(value.Description, null) || String.Equals(value.Description, String.Empty))
                if(String.Equals(databaseObject.Description, null) && String.Equals(databaseObject.Description, String.Empty))
                    value.Description = databaseObject.Description;

            // Genres
            foreach (String Genre in databaseObject.Genres)
                if (Genre != null && !value.Genres.Any(o => o.ToLower() == Genre.ToLower()))
                    value.Genres.Add(Genre);

            // DatabaseLocations
            foreach (LocationObject DatabaseLocation in databaseObject.Locations)
                if (!value.DatabaseLocations.Any(o => o.ExtensionName == DatabaseLocation.ExtensionName))
                    value.DatabaseLocations.Add(DatabaseLocation);

            // Covers
            foreach (String Cover in databaseObject.Covers)
                if (Cover != null && !value.Covers.Any(o => o == Cover))
                    value.Covers.Insert(0, Cover);

            // Released
            if (value.Released.Equals(DateTime.MinValue) && databaseObject.ReleaseYear > 0)
            { value.Released = new DateTime(databaseObject.ReleaseYear, 1, 1); }
        }

        public static void SortChapters(this MangaObject value)
        {
            // Try to find a place for Chapters with Volume as 0
            /*
            foreach (ChapterObject Chapter in value.Chapters.Where(c => c.Volume <= 0))
            {
                ChapterObject prevChapter = value.Chapters.FirstOrDefault(o => o.Chapter == (Chapter.Chapter + 1));
                if (prevChapter != null)
                { Chapter.Volume = prevChapter.Volume; }
            }
            //*/
            value.Chapters = value.Chapters.OrderBy(c => c.Chapter).ThenBy(c => c.SubChapter).ThenBy(c => c.Volume).ToList();
        }

        public static String MangaFileName(this MangaObject value)
        { return (value != null && value.Name != null) ? new String(value.Name.Where(Char.IsLetterOrDigit).ToArray()) : String.Empty; }

        //Yes the archive is a zip file, read the docs
        public static String MangaArchiveName(this MangaObject value, String Extention = "zip")
        { return (value != null && value.Name != null) ? String.Format("{0}.{1}", value.MangaFileName(), Extention) : String.Empty; }

        public static Boolean IsLocal(this MangaObject value, String Folder, String Extention = "zip")
        { return (value != null) ? System.IO.File.Exists(System.IO.Path.Combine(Folder, value.MangaArchiveName(Extention))) : false; }

        public static Boolean IsNameMatch(this MangaObject value, String name)
        {
            if (value == null)
                return false;
            String _name = new String(name.Where(Char.IsLetterOrDigit).ToArray()).ToLower();
            return new String(value.Name.Where(Char.IsLetterOrDigit).ToArray()).ToLower().Contains(_name) ||
                value.AlternateNames.FirstOrDefault(o => new String(o.Where(Char.IsLetterOrDigit).ToArray()).ToLower().Contains(_name)) != null;
        }

        public static String SelectedCover(this MangaObject value)
        { return value.Covers.Count > value.PreferredCover ? value.Covers[value.PreferredCover] : value.Covers.FirstOrDefault(); }

        public static Int32 IndexOfChapterObject(this MangaObject value, ChapterObject chapter_object)
        { return value.Chapters.FindIndex(c => c.Volume == chapter_object.Volume && c.Chapter == chapter_object.Chapter && c.SubChapter == chapter_object.SubChapter); }

        public static ChapterObject NextChapterObject(this MangaObject value, ChapterObject chapter_object)
        {
            Int32 index = value.IndexOfChapterObject(chapter_object) + 1;
            if (index >= value.Chapters.Count) return null;
            return value.Chapters[index];
        }

        public static ChapterObject PrevChapterObject(this MangaObject value, ChapterObject chapter_object)
        {
            Int32 index = value.IndexOfChapterObject(chapter_object) - 1;
            if (index < 0) return null;
            return value.Chapters[index];
        }

        public static ChapterObject ChapterObjectOfBookmarkObject(this MangaObject value, BookmarkObject bookmark_object)
        { return value.Chapters.Find(c => c.Volume == bookmark_object.Volume && c.Chapter == bookmark_object.Chapter && c.SubChapter == bookmark_object.SubChapter); }
    }
}

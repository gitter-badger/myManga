﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;
using myMangaSiteExtension.Attributes;
using myMangaSiteExtension.Enums;
using myMangaSiteExtension.Interfaces;
using myMangaSiteExtension.Objects;
using System.Text;

namespace MangaHere
{
    [ISiteExtensionDescription(
        "MangaHere",
        "mangahere.co",
        "http://www.mangahere.co/",
        RootUrl = "http://www.mangahere.co",
        Author = "James Parks",
        Version = "0.0.1",
        SupportedObjects = SupportedObjects.All,
        Language = "English")]
    public sealed class MangaHere : ISiteExtension
    {
        private ISiteExtensionDescriptionAttribute siteExtensionDescriptionAttribute;
        public ISiteExtensionDescriptionAttribute SiteExtensionDescriptionAttribute
        { get { return siteExtensionDescriptionAttribute ?? (siteExtensionDescriptionAttribute = GetType().GetCustomAttribute<ISiteExtensionDescriptionAttribute>(false)); } }

        public SearchRequestObject GetSearchRequestObject(String searchTerm)
        {
            return new SearchRequestObject()
            {
                Url = String.Format("{0}/search.php?name={1}", SiteExtensionDescriptionAttribute.RootUrl, Uri.EscapeUriString(searchTerm)),
                Method = SearchMethod.GET,
                Referer = SiteExtensionDescriptionAttribute.RefererHeader
            };
        }

        public MangaObject ParseMangaObject(string content)
        {
            if (content.ToLower().Contains("has been licensed, it is not available in MangaHere.".ToLower()))
                return null;
            HtmlDocument MangaObjectDocument = new HtmlDocument();
            MangaObjectDocument.LoadHtml(content);

            HtmlNode TitleNode = MangaObjectDocument.DocumentNode.SelectSingleNode("//h1[contains(@class,'title')]"),
                MangaDetailsNode = MangaObjectDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'manga_detail')]"),
                MangaPropertiesNode = MangaDetailsNode.SelectSingleNode(".//div[1]"),
                MangaDesciptionNode = MangaObjectDocument.GetElementbyId("show"),
                AuthorsNode = MangaPropertiesNode.SelectSingleNode(".//ul/li[5]"),
                ArtistsNode = MangaPropertiesNode.SelectSingleNode(".//ul/li[6]"),
                GenresNode = MangaPropertiesNode.SelectSingleNode(".//ul/li[4]");
            HtmlNodeCollection AuthorsNodeCollection = AuthorsNode.SelectNodes(".//a"),
                ArtistsNodeCollection = ArtistsNode.SelectNodes(".//a");
            String Desciption = MangaDesciptionNode != null ? MangaDesciptionNode.FirstChild.InnerText : String.Empty,
                MangaName = HtmlEntity.DeEntitize(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(TitleNode.LastChild.InnerText.ToLower()));

            String[] AlternateNames = MangaPropertiesNode.SelectSingleNode(".//ul/li[3]").LastChild.InnerText.Split(new String[] { "; " }, StringSplitOptions.RemoveEmptyEntries),
                Authors = { }, Artists = { },
                Genres = MangaPropertiesNode.SelectSingleNode(".//ul/li[4]").LastChild.InnerText.Split(new String[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            if (AuthorsNodeCollection != null)
                Authors = (from HtmlNode AuthorNode in AuthorsNodeCollection select AuthorNode.InnerText).ToArray();
            if (ArtistsNodeCollection != null)
                Artists = (from HtmlNode ArtistNode in ArtistsNodeCollection select ArtistNode.InnerText).ToArray();

            List<ChapterObject> Chapters = new List<ChapterObject>();
            HtmlNodeCollection RawChapterList = MangaDetailsNode.SelectNodes(".//div[contains(@class,'detail_list')]/ul[1]/li");
            foreach (HtmlNode ChapterNode in RawChapterList)
            {
                String volNode = ChapterNode.SelectSingleNode(".//span[1]/span").InnerText;
                String[] volChapSub = { (volNode != null && volNode.Length > 0) ? volNode.Substring(4).Trim() : "0" };
                String ChapterTitle = ChapterNode.SelectSingleNode(".//span[1]/a").InnerText.Trim();
                String ChapterNumber = ChapterTitle.Substring(ChapterTitle.LastIndexOf(' ') + 1).Trim();
                volChapSub = volChapSub.Concat(ChapterNumber.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();

                ChapterObject Chapter = new ChapterObject()
                {
                    Name = ChapterTitle,
                    Volume = UInt32.Parse(volChapSub[0]),
                    Chapter = UInt32.Parse(volChapSub[1]),
                    Locations = { 
                        new LocationObject() { 
                                ExtensionName = SiteExtensionDescriptionAttribute.Name, 
                                Url = ChapterNode.SelectSingleNode(".//span[1]/a").Attributes["href"].Value } 
                        },
                    Released = ChapterNode.SelectSingleNode(".//span[2]").InnerText.ToLower().Equals("today") ? DateTime.Today : (ChapterNode.SelectSingleNode(".//span[2]").InnerText.ToLower().Equals("yesterday") ? DateTime.Today.AddDays(-1) : DateTime.Parse(ChapterNode.SelectSingleNode(".//span[2]").InnerText))
                };
                if (volChapSub.Length == 3)
                    Chapter.SubChapter = UInt32.Parse(volChapSub[2]);
                Chapters.Add(Chapter);
            }
            Chapters.Reverse();
            String Cover = MangaPropertiesNode.SelectSingleNode(".//img[1]/@src").Attributes["src"].Value;
            Cover = Cover.Substring(0, Cover.LastIndexOf('?'));

            MangaObject MangaObj = new MangaObject()
            {
                Name = MangaName,
                Description = HtmlEntity.DeEntitize(Desciption),
                AlternateNames = AlternateNames.ToList(),
                Covers = { Cover },
                Authors = Authors.ToList(),
                Artists = Artists.ToList(),
                Genres = Genres.ToList(),
                Released = Chapters.First().Released,
                Chapters = Chapters
            };
            MangaObj.AlternateNames.RemoveAll(an => an.ToLower().Equals("none"));
            MangaObj.Genres.RemoveAll(g => g.ToLower().Equals("none"));
            return MangaObj;
        }

        public ChapterObject ParseChapterObject(string content)
        {
            HtmlDocument ChapterObjectDocument = new HtmlDocument();
            ChapterObjectDocument.LoadHtml(content);

            return new ChapterObject()
            {
                Pages = (from HtmlNode PageNode in ChapterObjectDocument.DocumentNode.SelectNodes("//section[contains(@class,'readpage_top')]/div[contains(@class,'go_page')]/span/select/option")
                         select new PageObject()
                         {
                             Url = PageNode.Attributes["value"].Value,
                             PageNumber = UInt32.Parse(PageNode.NextSibling.InnerText)
                         }).ToList()
            };
        }

        public PageObject ParsePageObject(string content)
        {
            HtmlDocument PageObjectDocument = new HtmlDocument();
            PageObjectDocument.LoadHtml(content);

            HtmlNode PageNode = PageObjectDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'go_page')]/span/select/option[@selected]"),
                PrevNode = PageNode.SelectSingleNode(".//preceding-sibling::option"),
                NextNode = PageNode.SelectSingleNode(".//following-sibling::option");

            String ImgSrc = PageObjectDocument.GetElementbyId("image").Attributes["src"].Value;
            ImgSrc = ImgSrc.Substring(0, ImgSrc.LastIndexOf('?'));
            Uri ImageLink = new Uri(ImgSrc);
            String Name = ImageLink.ToString().Split('/').Last();

            return new PageObject()
            {
                Name = Name,
                PageNumber = UInt32.Parse(PageNode.NextSibling.InnerText),
                Url = PageNode.Attributes["value"].Value,
                NextUrl = (NextNode != null) ? NextNode.Attributes["value"].Value : null,
                PrevUrl = (PrevNode != null) ? PrevNode.Attributes["value"].Value : null,
                ImgUrl = ImageLink.ToString()
            };
        }

        public List<SearchResultObject> ParseSearch(string content)
        {
            List<SearchResultObject> SearchResults = new List<SearchResultObject>();

            HtmlDocument SearchResultDocument = new HtmlDocument();
            SearchResultDocument.LoadHtml(content);
            HtmlWeb HtmlWeb = new HtmlWeb();
            HtmlNodeCollection HtmlSearchResults = SearchResultDocument.DocumentNode.SelectNodes(".//div[contains(@class,'result_search')]/dl");
            if (HtmlSearchResults != null && !HtmlSearchResults[0].InnerText.ToLower().Equals("No Manga Series".ToLower()))
            {
                foreach (HtmlNode SearchResultNode in HtmlSearchResults)
                {
                    String Name = SearchResultNode.SelectSingleNode(".//dt/a[1]").Attributes["rel"].Value,
                        Url = SearchResultNode.SelectSingleNode(".//dt/a[1]").Attributes["href"].Value;
                    HtmlWeb.PreRequest = new HtmlAgilityPack.HtmlWeb.PreRequestHandler(req =>
                    {
                        req.Method = "POST";
                        req.ContentType = "application/x-www-form-urlencoded";
                        String PayloadContent = String.Format("name={0}", Uri.EscapeDataString(Name));
                        Byte[] PayloadBuffer = Encoding.UTF8.GetBytes(PayloadContent.ToCharArray());
                        req.ContentLength = PayloadBuffer.Length;
                        req.GetRequestStream().Write(PayloadBuffer, 0, PayloadBuffer.Length);

                        return true;
                    });
                    String[] Details = HtmlWeb.Load(
                        String.Format("{0}/ajax/series.php", SiteExtensionDescriptionAttribute.RootUrl)
                        ).DocumentNode.InnerText.Replace("\\/","/").Split(new String[]{"\",\""}, StringSplitOptions.None);
                    String CoverUrl = Details[1].Substring(0, Details[1].LastIndexOf('?'));
                    Double Rating = -1;
                    Double.TryParse(Details[3], out Rating);

                    SearchResults.Add(new SearchResultObject()
                    {
                        Name = Name,
                        Rating = Rating,
                        Description = HtmlEntity.DeEntitize(Details[8]),
                        Artists = (from String Staff in Details[5].Split(new String[] { ", " }, StringSplitOptions.RemoveEmptyEntries) select Staff.Trim()).ToList(),
                        Authors = (from String Staff in Details[5].Split(new String[] { ", " }, StringSplitOptions.RemoveEmptyEntries) select Staff.Trim()).ToList(),
                        CoverUrl = CoverUrl,
                        Url = Url,
                        ExtensionName = SiteExtensionDescriptionAttribute.Name
                    });

                    HtmlWeb.PreRequest = null;
                }
            }

            return SearchResults;
        }
    }
}

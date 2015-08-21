using System;
using System.Reflection;
using myMangaSiteExtension.Attributes;
using myMangaSiteExtension.Interfaces;

namespace myMangaSiteExtension.Collections
{
    public class IMangaTrackingExtensionCollection : GenericCollection<IMangaTrackingExtension>
    {
        public virtual IMangaTrackingExtension this[String name]
        {
            get { return innerList[IndexOf(name)]; }
            set { innerList[IndexOf(name)] = value; }
        }

        public virtual Int32 IndexOf(String name)
        {
            foreach (IMangaTrackingExtension mangaTrackingExtensionItem in innerList)
            {
                IMangaTrackingExtensionDescriptionAttribute siteExtensionAttribute = mangaTrackingExtensionItem.GetType().GetCustomAttribute<IMangaTrackingExtensionDescriptionAttribute>(true);
                if (siteExtensionAttribute.Name.Equals(name))
                    return innerList.IndexOf(mangaTrackingExtensionItem);
            }
            return -1;
        }

        public virtual Boolean Contains(String name)
        {
            return IndexOf(name) >= 0;
        }
    }
}

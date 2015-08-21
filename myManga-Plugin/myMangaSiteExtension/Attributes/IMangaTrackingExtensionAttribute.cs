using System;
using System.Diagnostics;
using myMangaSiteExtension.Enums;

namespace myMangaSiteExtension.Attributes
{
    [DebuggerStepThrough, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class IMangaTrackingExtensionDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Name of the supported site.
        /// </summary>
        public String Name;
        /// <summary>
        /// Author fo the ISiteExtension. (Your name)
        /// </summary>
        public String Author = "";
        /// <summary>
        /// Version of the ISiteExtension
        /// </summary>
        public String Version = "0.0.0";

        /// <summary>
        /// ISiteExtension attribute.
        /// </summary>
        /// <param name="Name">Name of the site</param>
        /// <param name="URLFormat">Format of the sites url</param>
        /// <param name="RefererHeader">Referer header to use when connecting to the site</param>
        public IMangaTrackingExtensionDescriptionAttribute(String Name, String Author, String Version)
        {
            this.Name = Name;
            this.Author = Author;
            this.Version = Version;
        }

        public override string ToString()
        {
            return String.Format("{0} by {1} version {2}", Name, Author, Version);
        }
    }
}

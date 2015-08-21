using System;
using System.Diagnostics;
using myMangaSiteExtension.Enums;

namespace myMangaSiteExtension.Attributes
{
    [DebuggerStepThrough, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class IReaderTrackingExtensionDescriptionAttribute : Attribute
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

        public UserLoginType UserLoginType = UserLoginType.None;

        public override string ToString()
        {
            return String.Format("{0} by {1} version {2}", Name, Author, Version);
        }
    }
}

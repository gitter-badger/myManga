﻿using Core.IO;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace myMangaSiteExtension.Objects
{
    [Serializable, XmlRoot, DebuggerStepThrough]
    public class PageObject : SerializableObject, INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region NotifyPropertyChange
        public event PropertyChangingEventHandler PropertyChanging;
        protected void OnPropertyChanging([CallerMemberName] String caller = "")
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(caller));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String caller = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        #endregion

        #region Protected
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected String name;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected UInt32 page_number;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected String url;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected String nexturl;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected String prevurl;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected String imgurl;
        #endregion

        #region Public
        [XmlAttribute]
        public String Name
        {
            get { return name; }
            set
            {
                OnPropertyChanging();
                name = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public UInt32 PageNumber
        {
            get { return page_number; }
            set
            {
                OnPropertyChanging();
                page_number = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public String Url
        {
            get { return url; }
            set
            {
                OnPropertyChanging();
                url = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public String NextUrl
        {
            get { return nexturl; }
            set
            {
                OnPropertyChanging();
                nexturl = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public String PrevUrl
        {
            get { return prevurl; }
            set
            {
                OnPropertyChanging();
                prevurl = value;
                OnPropertyChanged();
            }
        }

        [XmlElement]
        public String ImgUrl
        {
            get { return imgurl; }
            set
            {
                OnPropertyChanging();
                imgurl = value;
                OnPropertyChanged();
            }
        }

        public PageObject() : base() { }
        public PageObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
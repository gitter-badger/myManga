﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using Core.IO;
using Core.MVVM;

namespace myMangaSiteExtension.Objects
{
    [Serializable, XmlRoot, DebuggerStepThrough]
    public class MangaObject : SerializableObject, INotifyPropertyChanging, INotifyPropertyChanged
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
        protected List<String> alternate_names;

        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected List<String> authors;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected List<String> artists;

        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected List<String> genres;

        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected FlowDirection pageFlowDirection;

        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected Dictionary<String, String> locations;
        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected List<String> covers;

        [NonSerialized, XmlIgnore, EditorBrowsable(EditorBrowsableState.Never)]
        protected ChapterObjectCollection chapters;
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

        [XmlArray, XmlArrayItem("Name")]
        public List<String> AlternateNames
        {
            get { return alternate_names ?? (alternate_names = new List<String>()); }
            set
            {
                OnPropertyChanging();
                alternate_names = value;
                OnPropertyChanged();
            }
        }

        [XmlArray, XmlArrayItem("Name")]
        public List<String> Authors
        {
            get { return authors ?? (authors = new List<String>()); }
            set
            {
                OnPropertyChanging();
                authors = value;
                OnPropertyChanged();
            }
        }

        [XmlArray, XmlArrayItem("Name")]
        public List<String> Artists
        {
            get { return artists ?? (artists = new List<String>()); }
            set
            {
                OnPropertyChanging();
                artists = value;
                OnPropertyChanged();
            }
        }

        [XmlArray, XmlArrayItem("Name")]
        public List<String> Genres
        {
            get { return genres ?? (genres = new List<String>()); }
            set
            {
                OnPropertyChanging();
                genres = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute]
        public FlowDirection PageFlowDirection
        {
            get { return pageFlowDirection; }
            set
            {
                OnPropertyChanging();
                pageFlowDirection = value;
                OnPropertyChanged();
            }
        }

        [XmlArray, XmlArrayItem("Cover")]
        public List<String> Covers
        {
            get { return covers ?? (covers = new List<String>()); }
            set
            {
                OnPropertyChanging();
                covers = value;
                OnPropertyChanged();
            }
        }

        [XmlArray, XmlArrayItem]
        public ChapterObjectCollection Chapters
        {
            get { return chapters ?? (chapters = new ChapterObjectCollection()); }
            set
            {
                OnPropertyChanging();
                chapters = value;
                OnPropertyChanged();
            }
        }

        public MangaObject() : base() { }
        public MangaObject(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        #endregion
    }
}
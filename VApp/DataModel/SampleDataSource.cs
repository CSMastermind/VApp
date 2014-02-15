﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using VApp.UserControls;

namespace VApp.Data
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : VApp.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    // Generic item data model.
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, Type userControl, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._control = userControl;
            this._group = group;
        }

        private Type _control = null;
        public Type ControlType
        {
            get { return this._control; }
            set { this.SetProperty(ref this._control, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    // Generic group data model.
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    // Creates a collection of groups and items with hard-coded content.
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            var group1 = new SampleDataGroup("Group-1",
                    "Personal Information",
                    "",
                    "Assets/DarkGray.png",
                    "My HealtheVet (MHV) has made it easy to keep track of your Personal Information. My HealtheVet is all about you and your health. Part of your personal online health journal is your identification. When you registered for My HealtheVet, you entered important information about yourself. This is where you will find it, along with other important facts like your login information, blood type and emergency contacts.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "My Profile",
                    "",
                    "Assets/LightGray.png",
                    "Your name, address and identifying information",
                    typeof(ProfileUserControl),
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                    "Download My Data",
                    "",
                    "Assets/DarkGray.png",
                    "Use the Blue Button to easily download your health information",
                    null,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                    "Change your Password",
                    "",
                    "Assets/MediumGray.png",
                    "Change your My HealtheVet password here",
                    null,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                    "In Case of Emergency",
                    "",
                    "Assets/DarkGray.png",
                    "Keep your emergency contacts in one place",
                    null,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                    "My Account",
                    "",
                    "Assets/MediumGray.png",
                    "Manage your account, authentication",
                    null,
                    group1));
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                    "Get Care",
                    "",
                    "Assets/LightGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Care Givers",
                    "",
                    "Assets/DarkGray.png",
                    "Keep track of health care providers in one place",
                    null,
                    group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                    "Treatment Facilities",
                    "",
                    "Assets/MediumGray.png",
                    "Record the places you have been treated",
                    null,
                    group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                    "My Coverage",
                    "",
                    "Assets/LightGray.png",
                    "Keep your insurance information in one place",
                    null,
                    group2));
            this.AllGroups.Add(group2);

            var group3 = new SampleDataGroup("Group-3",
                    "Pharmacy",
                    "",
                    "Assets/MediumGray.png",
                    "Since its introduction in August 2005, Prescription Refill continues to be the most popular feature of My HealtheVet. Prescription Refill, Prescription History and your Medications health log can be found here in the Pharmacy section.");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Refill My Prescriptions",
                    "",
                    "Assets/MediumGray.png",
                    "Do you need to refill your prescriptions? Do it online",
                    null,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                    "Prescription Refill History",
                    "",
                    "Assets/LightGray.png",
                    "See the prescriptions you have had refilled online",
                    null,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-3",
                    "My Medications + Supplements",
                    "",
                    "Assets/DarkGray.png",
                    "Track medicines, herbals & supplements you take",
                    null,
                    group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-4",
                    "My VA Medication List",
                    "",
                    "Assets/LightGray.png",
                    "See the medicines your VA Doctors have prescribed",
                    null,
                    group3));
            this.AllGroups.Add(group3);

            var group4 = new SampleDataGroup("Group-4",
                    "Track Health",
                    "",
                    "Assets/LightGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group4.Items.Add(new SampleDataItem("Group-4-Item-1",
                    "Vitals + Readings",
                    "",
                    "Assets/DarkGray.png",
                    "Nine ways to monitor your health statistics online",
                    null,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-2",
                    "Labs + Tests",
                    "",
                    "Assets/LightGray.png",
                    "Keep track of your lab results and tests here",
                    null,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-3",
                    "Journals",
                    "",
                    "Assets/LightGray.png",
                    "Record your daily activity and food intake",
                    null,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-4",
                    "My Goals",
                    "",
                    "Assets/MediumGray.png",
                    "Record and track your personal Goals",
                    null,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-6",
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group4));
            this.AllGroups.Add(group4);

            var group5 = new SampleDataGroup("Group-5",
                    "Research Health",
                    "",
                    "Assets/MediumGray.png",
                    "The Research Health section of My HealtheVet is where you can get health information, research a topic, and simply learn more about your health. Read about common conditions and VA health programs. Get answers to your health questions from trusted medical resources. My HealtheVet makes it easy to stay informed by bringing a wealth of information right to your fingertips.");
            group5.Items.Add(new SampleDataItem("Group-5-Item-1",
                    "Healthy Living Centers",
                    "",
                    "Assets/LightGray.png",
                    "Healthy living means taking certain steps to help avoid illness",
                    null,
                    group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-2",
                    "Diseases + Condition Centers",
                    "",
                    "Assets/DarkGray.png",
                    "Find information for common illnesses and conditions",
                    null,
                    group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-3",
                    "Mental Health",
                    "",
                    "Assets/LightGray.png",
                    "From holiday blues to the stresses of being a soldier",
                    null,
                    group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-4",
                    "Medical Library",
                    "",
                    "Assets/MediumGray.png",
                    "My HealtheVet provides two extensive online medical libraries",
                    null,
                    group5));
            this.AllGroups.Add(group5);

            var group6 = new SampleDataGroup("Group-6",
                    "Group Title: 6",
                    "Group Subtitle: 6",
                    "Assets/DarkGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group6.Items.Add(new SampleDataItem("Group-6-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-6",
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-7",
                    "Item Title: 7",
                    "Item Subtitle: 7",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-8",
                    "Item Title: 8",
                    "Item Subtitle: 8",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    null,
                    group6));
            this.AllGroups.Add(group6);
        }
    }
}
